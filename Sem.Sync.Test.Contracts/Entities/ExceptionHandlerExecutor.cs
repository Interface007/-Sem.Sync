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
    public class ExceptionHandlerExecutor<TData> : RuleExecuter<TData, MessageCollection<TData>>
    {
        public ExceptionHandlerExecutor(string valueName, TData value)
            : base(valueName, value, null)
        {
        }

        public ExceptionHandlerExecutor(Expression<Func<TData>> data)
            : base(data, null)
        {
        }

        protected override bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            this.ExceptionHandled = valueName == "handle";
            return this.ExceptionHandled;
        }

        protected override void AfterInvoke(RuleValidationResult invocationResult)
        {
        }

        public bool ExceptionHandled { get; set; }
    }
}