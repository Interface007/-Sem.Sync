// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class MessageCollection<TData> : RuleExecuter<TData, MessageCollection<TData>>
    {
        public List<RuleValidationResult> Results { get; private set; }

        public MessageCollection(string valueName, TData value)
            : base(valueName, value)
        {
            this.Results = new List<RuleValidationResult>();
        }

        public MessageCollection(Expression<Func<TData>> data)
            : base(data)
        {
            this.Results = new List<RuleValidationResult>();
        }

        protected override void AfterInvoke(RuleValidationResult invocationResult)
        {
            if (!invocationResult.Result)
            {
                this.Results.Add(invocationResult);
            }
        }
    }
}