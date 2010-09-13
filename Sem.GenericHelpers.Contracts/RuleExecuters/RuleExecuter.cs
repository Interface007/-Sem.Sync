// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleExecuter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleExecuter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
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

        protected internal bool LastValidationResult { get; set; }

        private readonly string myNamespace = typeof(TResultClass).Namespace;

        protected RuleExecuter(string valueName, TData value)
        {
            this.Value = value;
            this.ValueName = valueName;
        }

        protected RuleExecuter(Expression<Func<TData>> data)
        {
            var member = data.Body as MemberExpression;
            this.ValueName = member != null
                                 ? member.Member.Name
                                 : "anonymous value";

            try
            {
                this.Value = data.Compile().Invoke();
            }
            catch (NullReferenceException)
            {
                // suppress null reference exceptions while invoking the
                // expression - in this case one member of the property 
                // paths is null and we simply assume the result to be
                // null, too.
                this.Value = default(TData);
            }
        }

        #endregion

        public static readonly List<Action<RuleValidationResult>> AfterInvokeAction = new List<Action<RuleValidationResult>>();

        public TResultClass Assert(Func<TData, bool> rule)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data) };
            return this.Assert(ruleClass);
        }

        public TResultClass Assert(RuleBase<TData, object> rule)
        {
            this.Assert(rule, null);
            return (TResultClass)this;
        }

        public TResultClass Assert(IEnumerable<RuleBase<TData, object>> ruleSet)
        {
            return this.Assert(ruleSet, null);
        }

        public TResultClass Assert<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data, ruleParameter) };
            return this.Assert(ruleClass, ruleParameter);
        }

        public TResultClass Assert<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter)
        {
            this.ExecuteRuleExpression(rule, ruleParameter, this.ValueName);
            return (TResultClass)this;
        }

        public TResultClass Assert<TParameter>(IEnumerable<RuleBase<TData, TParameter>> ruleSet, TParameter ruleParameter)
        {
            foreach (var rule in ruleSet)
            {
                this.ExecuteRuleExpression(rule, ruleParameter, this.ValueName);
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
                var ruleAttributes = propertyInfo.GetCustomAttributes(typeof(ContractRuleAttribute), true);
                if (ruleAttributes.Count() == 0)
                {
                    continue;
                }

                // first we need to construct a new rule executer type, unfortunately this is a generic type, 
                // so we need to do it via reflection (we don't have the type of the property at design time)
                var propertyName = this.ValueName + "." + propertyInfo.Name;
                var ruleExecuter = this.CreateRuleExecuter(
                    propertyInfo.PropertyType,
                    propertyName,
                    propertyInfo.GetValue(this.Value, null));

                // now enumerate the attributes of the property (there might be more than one)
                foreach (ContractRuleAttribute ruleAttribute in ruleAttributes)
                {
                    var ruleParameter = ruleAttribute.Parameter;
                    var namespaceFilter = ruleAttribute.Namespace;

                    // here we filter by namespace
                    if (!string.IsNullOrEmpty(namespaceFilter))
                    {
                        var callingNamespace = this.GetCallingNamespace();
                        if (!(callingNamespace == namespaceFilter || callingNamespace.StartsWith(namespaceFilter)))
                        {
                            continue;
                        }
                    }

                    var assertMethod = ruleExecuter.GetType().GetMethod("ExecuteRuleExpression");

                    assertMethod =
                        assertMethod.MakeGenericMethod(
                            ruleParameter != null
                            ? ruleParameter.GetType()
                            : typeof(object));

                    try
                    {
                        // create an instance of the rule and invoke the Assert statement
                        var rule = ruleAttribute.Type.GetConstructor(Type.EmptyTypes).Invoke(null);
                        var ruleType = rule.GetType();
                        var messageAccessor = ruleType.GetProperty("Message");
                        if (!string.IsNullOrEmpty(ruleAttribute.Message))
                        {
                            messageAccessor.SetValue(rule, ruleAttribute.Message, null);
                        }

                        var result = new RuleValidationResult(
                            ruleType,
                            string.Format(
                                "The rule {0} did fail for value name >>{1}<<: {2}",
                                ruleType.Namespace + "." + ruleType.Name,
                                propertyName,
                                string.Format((string)messageAccessor.GetValue(rule, null), ruleParameter, propertyName)),
                            propertyName,
                            (bool)assertMethod.Invoke(ruleExecuter, new[] { rule, ruleParameter, propertyName }));

                        this.AfterInvoke(result);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }

            return (TResultClass)this;
        }

        public bool ExecuteRuleExpression<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter, string valueName)
        {
            // if there is no rule, we cannot say that the rule is validated
            if (rule == null)
            {
                return true;
            }

            // let the concrete execution class decide if we want to execute the expression
            if (!this.BeforeInvoke(rule, ruleParameter, valueName))
            {
                this.LastValidationResult = true;
                return true;
            }

            try
            {
                // execute the expression
                this.LastValidationResult = rule.CheckExpression(this.Value, ruleParameter);
            }
            catch (NullReferenceException)
            {
                this.LastValidationResult = false;
            }
            catch (Exception ex)
            {
                if (!this.HandleInvokeException(ex, rule, ruleParameter, valueName))
                {
                    throw;
                }
            }

            Type ruleType = rule.GetType();
            var result = new RuleValidationResult(
                ruleType,
                string.Format(
                    "The rule {0} did fail for value name >>{1}<<: {2}",
                    ruleType.Namespace + "." + ruleType.Name,
                    valueName,
                    string.Format(rule.Message, ruleParameter, valueName)),
                valueName,
                this.LastValidationResult);

            foreach (var action in AfterInvokeAction)
            {
                action.Invoke(result);
            }

            this.AfterInvoke(result);
            return this.LastValidationResult;
        }

        protected abstract void AfterInvoke(RuleValidationResult invocationResult);

        protected virtual bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return true;
        }

        protected virtual bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return false;
        }

        /// <summary>
        /// Performs a stack trace frame walk to find the first frame, that does not match
        /// the namespace of this class.
        /// </summary>
        /// <returns>the calling namespace name</returns>
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

                    if (callingNamespace != null && !callingNamespace.StartsWith(this.myNamespace))
                    {
                        name = callingNamespace;
                        break;
                    }
                }
            }

            return name;
        }

        private object CreateRuleExecuter(Type valueType, string name, object value)
        {
            return this.GetType()
                .GetGenericTypeDefinition()
                .MakeGenericType(valueType)
                .GetConstructor(new[] { typeof(string), valueType })
                .Invoke(new[] { name, value });
        }
    }
}
