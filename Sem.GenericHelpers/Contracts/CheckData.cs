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

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class CheckData<TData>
    {
        internal string ValueName { get; set; }
        internal TData Value { get; set; }

        public CheckData<TData> Assert(Rule<TData> rule)
        {
            rule.AssertFor(this.ValueName, this.Value);
            return this;
        }

        public CheckData<TData> Assert<TParameter>(Rule<TData, TParameter> rule, TParameter ruleParameter)
        {
            rule.AssertFor(this.ValueName, this.Value, ruleParameter);
            return this;
        }

        public CheckData<TData> Assert(Func<TData, bool> rule)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule};
            Assert(ruleClass);
            return this;
        }

        public CheckData<TData> Assert(Func<TData, bool> rule, string message)
        {
            var ruleClass = new Rule<TData> { CheckExpression = rule, Message = message };
            Assert(ruleClass);
            return this;
        }

        public CheckData<TData> Assert<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter, string message)
        {
            var ruleClass = new Rule<TData, TParameter> { CheckExpression = rule, Message = message };
            Assert(ruleClass, ruleParameter);
            return this;
        }

        public CheckData<TData> Assume(Rule<TData> rule)
        {
            rule.AssumeFor(this.ValueName, this.Value);
            return this;
        }

        public CheckData<TData> Assume<TParameter>(Rule<TData, TParameter> rule, TParameter ruleParameter)
        {
            rule.AssumeFor(this.ValueName, this.Value, ruleParameter);
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
    }
}