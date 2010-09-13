// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalExecution.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConditionalExecution type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Linq.Expressions;

    public class ConditionalExecution<TData> : RuleExecuter<TData, ConditionalExecution<TData>>
    {
        private bool conditionTrue = true;

        public ConditionalExecution(string valueName, TData value)
            : base(valueName, value)
        {
        }

        public ConditionalExecution(Expression<Func<TData>> data)
            : base(data)
        {
        }

        public ConditionalExecution<TData> Execute(Action action)
        {
            if (this.conditionTrue)
            {
                action.Invoke();
            }

            return this;
        }

        protected override void AfterInvoke(RuleValidationResult invocationResult)
        {
            this.conditionTrue &= invocationResult.Result;
        }
    }
}