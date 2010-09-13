// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckData.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
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

        protected override void AfterInvoke(RuleValidationResult invocationResult)
        {
            if (invocationResult.Result)
            {
                return;
            }

            throw new RuleValidationException(invocationResult.RuleType, invocationResult.Message, invocationResult.ValueName);
        }
    }
}