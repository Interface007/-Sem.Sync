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
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Exceptions;

    /// <summary>
    /// Check class including the data to perform rule checking. A validation error will
    /// cause a <see cref="RuleValidationException"/>.
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class CheckData<TData> : RuleExecuter<TData, CheckData<TData>>
    {
        public CheckData(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public CheckData(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public CheckData(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(valueName, value, methodAttribs)
        {
        }

        public CheckData(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(data, methodAttribs)
        {
        }

        /// <summary>
        /// Creates a new <see cref="CheckData{TData}"/> structure that has the current CheckData attached.
        /// This way you can build up validation chains that can be executed with a 
        /// single <see cref="RuleExecuter{TData,TResultClass}.Assert()"/> method call.
        /// </summary>
        /// <typeparam name="TDataNew">The type of the new data to ve validated.</typeparam>
        /// <param name="data">The data to ve validated as a lambda expression.</param>
        /// <returns>The new CheckData instance.</returns>
        public CheckData<TDataNew> ForCheckData<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newExecuter = new CheckData<TDataNew>(data, this.MethodRuleAttributes);
            this.previousExecuter = () => newExecuter.Assert();
            return newExecuter;
        }

        /// <summary>
        /// Performs the rule execution result check. If the rule execution results in "false",
        /// this method will throw the <see cref="RuleValidationException"/>.
        /// </summary>
        /// <param name="invocationResult">The rule validation result structure with information about the rule validation process.</param>
        /// <exception cref="RuleValidationException">If the validation result is "false".</exception>
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