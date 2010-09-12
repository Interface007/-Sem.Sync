// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleExecuter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleExecuter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class RuleExecuter<TData, TResultClass>
        where TResultClass : RuleExecuter<TData, TResultClass>
    {
        #region data
        internal string ValueName { get; set; }
        internal TData Value { get; set; }

        private readonly string _MyNamespace = typeof(TResultClass).Namespace;

        protected RuleExecuter(string valueName, TData value)
        {
            this.Value = value;
            this.ValueName = valueName;
        }

        protected RuleExecuter(Expression<Func<TData>> data)
        {
            var member = data.Body as MemberExpression;
            var name = member != null
                        ? member.Member.Name
                        : "anonymous value";

            this.Value = data.Compile().Invoke();
            this.ValueName = name;
        }

        #endregion

        public abstract TResultClass AssertInternal<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter);

        public TResultClass Assert(ConfigurationValidatorBase validator)
        {
            var ruleClass = new RuleBase<TData, object>
                {
                    CheckExpression = (data, parameter) =>
                        {
                            try
                            {
                                validator.Validate(data);
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                };

            return this.Assert(ruleClass);
        }

        public TResultClass Assert(Func<TData, bool> rule)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data) };
            return this.Assert(ruleClass);
        }

        public TResultClass Assert(RuleBase<TData, object> rule)
        {
            return this.AssertInternal(rule, null);
        }

        public TResultClass Assert(IEnumerable<RuleBase<TData, object>> ruleSet)
        {
            return this.Assert(ruleSet, null);
        }

        public TResultClass Assert<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data, ruleParameter) };
            return this.AssertInternal(ruleClass, ruleParameter);
        }

        public TResultClass Assert<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter)
        {
            return this.AssertInternal(rule, ruleParameter);
        }

        public TResultClass Assert<TParameter>(IEnumerable<RuleBase<TData, TParameter>> ruleSet, TParameter parameter)
        {
            foreach (var rule in ruleSet)
            {
                this.AssertInternal(rule, parameter);
            }

            return (TResultClass)this;
        }

        public TResultClass Assert()
        {
            var ruleSet = RuleSets.GetRulesForType<TData, object>();
            foreach (var typeRule in ruleSet)
            {
                var ruleBase = (RuleBase<TData, object>)typeRule.Rule;
                this.Assert(ruleBase, null);
            }

            // now we look up the properties for rules
            foreach (var propertyInfo in typeof(TData).GetProperties())
            {
                var customAttributes = propertyInfo.GetCustomAttributes(typeof(ContractRuleAttribute), true);
                if (customAttributes.Count() == 0)
                {
                    continue;
                }

                // first we need to construct a new CheckData type, unfortunately this is a generic type, so we 
                // need to do it via reflection (we don't have the type of the property at design time)
                var data = Create(
                    propertyInfo.PropertyType,
                    this.ValueName + "." + propertyInfo.Name,
                    propertyInfo.GetValue(this.Value, null));

                foreach (ContractRuleAttribute ruleAttribute in customAttributes)
                {
                    if (!string.IsNullOrEmpty(ruleAttribute.Namespace))
                    {
                        var callingNamespace = GetCallingNamespace();
                        if (!(callingNamespace == ruleAttribute.Namespace || callingNamespace.StartsWith(ruleAttribute.Namespace)))
                        {
                            continue;
                        }
                    }

                    var rule = ruleAttribute.Type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    var assertMethod = data.GetType().GetMethods().Where(x => x.IsGenericMethod).FirstOrDefault();
                    assertMethod =
                        assertMethod.MakeGenericMethod(ruleAttribute.Parameter != null
                        ? ruleAttribute.Parameter.GetType()
                        : typeof(object));

                    try
                    {
                        assertMethod.Invoke(data, new[] { rule, ruleAttribute.Parameter });
                        this.AfterInvoke(rule, ruleAttribute.Parameter, this.ValueName + "." + propertyInfo.Name);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }

            return (TResultClass)this;
        }

        protected virtual void AfterInvoke(object ruleType, object ruleParameter, string valueName)
        {
        }

        private string GetCallingNamespace()
        {
            var stackTrace = new StackTrace(false);
            var stackFrames = stackTrace.GetFrames();
            var name = string.Empty;
            if (stackFrames != null)
            {
                foreach (var stackFrame in stackFrames)
                {
                    var callingNamespace = stackFrame.GetMethod().DeclaringType.Namespace;

                    if (callingNamespace != null && !callingNamespace.StartsWith(this._MyNamespace))
                    {
                        name = callingNamespace;
                        break;
                    }
                }
            }

            return name;
        }

        private object Create(Type valueType, string name, object value)
        {
            return this.GetType()
                .GetGenericTypeDefinition()
                .MakeGenericType(valueType)
                .GetConstructor(new[] { typeof(string), valueType })
                .Invoke(new[] { name, value });
        }
    }
}
