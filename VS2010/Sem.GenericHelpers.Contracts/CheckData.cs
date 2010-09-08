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
    using System.Collections.Generic;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class CheckData<TData>
    {
        #region
        internal string ValueName { get; set; }
        internal TData Value { get; set; }

        

        public CheckData()
        {
        }

        public CheckData(string valueName, TData value)
        {
            this.Value = value;
            this.ValueName = valueName;
        }
        #endregion

        #region assert
        public CheckData<TData> Assert()
        {
            var ruleSet = RuleSets.GetRulesForType(typeof(TData));
            foreach (var typeRule in ruleSet)
            {
                this.Assert((Rule<TData>)typeRule.Rule);
            }
            return this;
        }

        public CheckData<TData> Assert(Rule<TData> rule)
        {
            rule.AssertFor(this);
            return this;
        }

        public CheckData<TData> Assert(IEnumerable<Rule<TData>> ruleSet)
        {
            foreach (Rule<TData> rule in ruleSet)
            {
                rule.AssertFor(this);
            }

            return this;
        }

        public CheckData<TData> Assert(Func<TData, bool> rule)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule };
            return Assert(ruleClass);
        }

        public CheckData<TData> Assert<TParameter>(Rule<TData, TParameter> rule, TParameter ruleParameter)
        {
            rule.AssertFor(this, ruleParameter);
            return this;
        }

        public CheckData<TData> Assert(Func<TData, bool> rule, string message)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule, Message = message };
            return Assert(ruleClass);
        }

        public CheckData<TData> Assert<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter, string message)
        {
            var ruleClass = new Rule<TData, TParameter> { CheckExpression = rule, Message = message };
            return Assert(ruleClass, ruleParameter);
        }
        #endregion

        #region assume
        public CheckData<TData> Assume(Rule<TData> rule)
        {
            rule.AssumeFor(this);
            return this;
        }

        public CheckData<TData> Assume<TParameter>(Rule<TData, TParameter> rule, TParameter ruleParameter)
        {
            rule.AssumeFor(this, ruleParameter);
            return this;
        }

        public CheckData<TData> Assume(Func<TData, bool> rule)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule};
            Assume(ruleClass);
            return this;
        }

        public CheckData<TData> Assume(Func<TData, bool> rule, string message)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule, Message = message };
            Assume(ruleClass); 
            return this;
        }

        public CheckData<TData> Assume<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter, string message)
        {
            var ruleClass = new Rule<TData, TParameter> { CheckExpression = rule, Message = message };
            Assume(ruleClass, ruleParameter);
            return this;
        }
        #endregion

    }
}