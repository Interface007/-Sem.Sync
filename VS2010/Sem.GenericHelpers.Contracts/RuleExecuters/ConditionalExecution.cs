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
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ConditionalExecution<TData> : RuleExecuter<TData, ConditionalExecution<TData>>
    {
        private bool conditionTrue = true;

        public ConditionalExecution(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public ConditionalExecution(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public ConditionalExecution(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(valueName, value, methodAttribs)
        {
        }
        
        public ConditionalExecution(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(data, methodAttribs)
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

        public CheckData<TDataNew> ForExecution<TDataNew>(Expression<Func<TDataNew>> data)
        {
            CheckData<TDataNew> newExecuter = new CheckData<TDataNew>(data, this.MethodRuleAttributes);
            this.previousExecuter = () => newExecuter.Assert();
            return newExecuter;
        }
    }
}