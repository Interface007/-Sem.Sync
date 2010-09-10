// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class MessageCollection<TData> : RuleExecuter<TData, MessageCollection<TData>>, IMessageCollection
    {
        public MessageCollection(string valueName, TData value)
            : base(valueName, value)
        {
            this.Results = new List<RuleValidationResult>();
        }

        public List<RuleValidationResult> Results { get; private set; }

        protected override void AfterInvoke(object rule, object ruleParameter, string valueName)
        {
            HandleResult((RuleBaseInformation)rule, ruleParameter, rule.GetType(), valueName);
        }

        public override MessageCollection<TData> AssertInternal<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter)
        {
            if (rule != null)
            {
                if (!rule.CheckExpression(this.Value, ruleParameter))
                {
                    HandleResult(rule, ruleParameter, rule.GetType(), this.ValueName);
                }
            }

            return this;
        }

        private void HandleResult<TParameter>(RuleBaseInformation rule, TParameter ruleParameter, Type ruleType, string valueName)
        {
            this.Results.Add(
                new RuleValidationResult(
                    ruleType,
                    string.Format("The rule {0} did fail for value name >>{1}<<: {2}", ruleType.Name, valueName, string.Format(rule.Message, ruleParameter)), 
                    valueName));
        }
    }
}