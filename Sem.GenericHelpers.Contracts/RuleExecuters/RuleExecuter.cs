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

    public interface IRuleExecuter
    {
        RuleValidationResult InvokeRuleExecution(object ruleExecuter, ContractRuleAttribute ruleAttribute, object ruleParameter, string propertyName);
    }

    public abstract class RuleExecuter<TData, TResultClass> : IRuleExecuter
        where TResultClass : RuleExecuter<TData, TResultClass>
    {
        #region data

        public static readonly List<Action<RuleValidationResult>> AfterInvokeAction = new List<Action<RuleValidationResult>>();

        internal string ValueName { get; set; }

        internal TData Value { get; set; }

        internal Action previousExecuter;

        internal IEnumerable<MethodRuleAttribute> MethodRuleAttributes;

        protected internal bool LastValidationResult { get; set; }

        private readonly string myNamespace = typeof(TResultClass).Namespace;

        private static Dictionary<PropertyInfo, object[]> AttributeCache = new Dictionary<PropertyInfo, object[]>();

        private static Dictionary<MethodBase, List<MethodRuleAttribute>> RuleAttributeCache = new Dictionary<MethodBase, List<MethodRuleAttribute>>();

        #endregion
        
        #region ctors
        protected RuleExecuter(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodRuleAttributes)
        {
            this.MethodRuleAttributes = methodRuleAttributes ?? GetMethodRuleAttributes(2);
            this.Value = value;
            this.ValueName = valueName;
        }

        protected RuleExecuter(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodRuleAttributes)
        {
            this.MethodRuleAttributes = methodRuleAttributes ?? GetMethodRuleAttributes(2);
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

        #region asserts
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
            AssertForType();
            AssertForProperties();
            AssertForMethodAttributes();

            return (TResultClass)this;
        }

        private void AssertForMethodAttributes()
        {
            if (this.MethodRuleAttributes == null)
            {
                return;
            }

            var ruleAttributes = (from r in this.MethodRuleAttributes where r.MethodParameterName == this.ValueName select r);

            foreach (var ruleAttribute in ruleAttributes)
            {
                var ruleExecuter = new CheckData<TData>(this.ValueName, this.Value);
                ruleExecuter.InvokeRuleExecution(ruleExecuter, ruleAttribute, ruleAttribute.Parameter, this.ValueName);
            }
        }

        private void AssertForProperties()
        {
            foreach (var propertyInfo in typeof(TData).GetProperties())
            {
                if (!AttributeCache.ContainsKey(propertyInfo))
                {
                    AttributeCache.Add(propertyInfo, propertyInfo.GetCustomAttributes(typeof(ContractRuleAttribute), true));
                }

                var ruleAttributes = AttributeCache[propertyInfo];
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

                    var result = InvokeRuleExecution(ruleExecuter, ruleAttribute, ruleParameter, propertyName);
                    this.AfterInvoke(result);
                }
            }
        }

        private void AssertForType()
        {
            var ruleSet = RuleSets.GetRulesForType<TData, object>();
            foreach (var typeRule in ruleSet)
            {
                var ruleBase = (RuleBase<TData, object>)typeRule.Rule;
                this.Assert(ruleBase, null);
            }
        }

        #endregion

        public RuleValidationResult InvokeRuleExecution(object ruleExecuter, ContractRuleAttribute ruleAttribute, object ruleParameter, string propertyName)
        {
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

                return result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
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
            
            if (previousExecuter != null)
            {
                previousExecuter.Invoke();
            }

            return this.LastValidationResult;
        }

        #region overridables

        protected abstract void AfterInvoke(RuleValidationResult invocationResult);

        protected virtual bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return true;
        }

        protected virtual bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return false;
        }

        #endregion

        public static List<MethodRuleAttribute> GetMethodRuleAttributes(int skipFrames)
        {
            StackTrace stack = new StackTrace(skipFrames, false);

            var methodInfo = stack.GetFrame(0).GetMethod();

            for (int i = 0; i < stack.FrameCount; i++)
            {
                methodInfo = stack.GetFrame(i).GetMethod();
                var declaringType = methodInfo.DeclaringType;
                if (declaringType.Namespace != null
                    && !declaringType.Namespace.StartsWith(typeof(Bouncer).Namespace ?? string.Empty)
                    && declaringType.GetInterface("IRuleExecuter", false) == null)
                {
                    break;
                }
            }

            if (!RuleAttributeCache.ContainsKey(methodInfo))
            {
                var customAttributes = methodInfo.GetCustomAttributes(typeof(MethodRuleAttribute), true);
                var methodRuleAttributes1 = (from x in customAttributes select (MethodRuleAttribute)x).ToList();
                RuleAttributeCache.Add(methodInfo, methodRuleAttributes1);
            }

            return RuleAttributeCache[methodInfo];
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
                .GetConstructor(new[] { typeof(string), valueType, typeof(IEnumerable<MethodRuleAttribute>) })
                .Invoke(new[] { name, value, this.MethodRuleAttributes });
        }
    }
}
