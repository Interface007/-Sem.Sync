// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckData.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Exceptions;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class CheckData<TData> : RuleExecuter<TData, CheckData<TData>>
    {
        public CheckData(string valueName, TData value)
            : base(valueName, value)
        {
        }

        public CheckData(Expression<Func<TData>> data)
            : base(data)
        {
        }

        public static CheckData<TData> For(Expression<Func<TData>> data)
        {
            return new CheckData<TData>(data);
        }

        public override CheckData<TData> AssertInternal<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter)
        {
            if (rule != null)
            {
                bool result;
                try
                {
                    result = rule.CheckExpression(this.Value, ruleParameter);
                }
                catch (NullReferenceException)
                {
                    result = false;
                }

                this.HandleRuleResult(
                    result, 
                    rule.GetType(), 
                    rule.Message, 
                    ruleParameter);
            }

            return this;
        }

        private void HandleRuleResult<TParameter>(bool result, Type ruleType, string message, TParameter ruleParameter)
        {
            if (result)
            {
                return;
            }

            throw new RuleValidationException(ruleType, string.Format("The rule {0} did fail: {1}", ruleType.FullName, string.Format(message, ruleParameter)), this.ValueName);
        }
    }
}