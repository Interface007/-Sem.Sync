namespace Sem.Sync.Test.Contracts.Tests
{
    using System;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class VetoExecutor<TData> : RuleExecuter<TData, MessageCollection<TData>>
    {
        public VetoExecutor(string valueName, TData value)
            : base(valueName, value)
        {
        }

        public VetoExecutor(Expression<Func<TData>> data)
            : base(data)
        {
        }

        protected override bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return valueName != "veto";
        }

        protected override void AfterInvoke(RuleValidationResult invocationResult)
        {
        }

        public bool LastValidation
        {
            get
            {
                return this.LastValidationResult;
            }
        }
    }
}