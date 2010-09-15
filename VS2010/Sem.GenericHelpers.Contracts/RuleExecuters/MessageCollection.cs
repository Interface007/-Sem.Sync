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

    using Sem.GenericHelpers.Contracts.Attributes;

    /// <summary>
    /// Check class including the data to perform rule checking. Each rule violation
    /// adds a new entry to the <see cref="Results"/> list (this is a <see cref="List{T}"/>
    /// of <see cref="RuleValidationResult"/>).
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class MessageCollection<TData> : RuleExecuter<TData, MessageCollection<TData>>
    {
        /// <summary>
        /// The result list of <see cref="RuleValidationResult"/>. Each violated rule while
        /// asserting adds a new entry to this list.
        /// </summary>
        public List<RuleValidationResult> Results { get; private set; }

        public MessageCollection(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public MessageCollection(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public MessageCollection(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(valueName, value, methodAttribs)
        {
            this.Results = new List<RuleValidationResult>();
        }
        
        public MessageCollection(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttribs)
            : base(data, methodAttribs)
        {
            this.Results = new List<RuleValidationResult>();
        }

        /// <summary>
        /// Creates a <see cref="MessageCollection{TData}"/> for collecting warnings about rule violations 
        /// by specifying a lambda expression:
        /// <para>var result = Bouncer.ForMessages(() => MessageOneOk).ForMessages(() => MessageOneOk).Assert().Results;</para>
        /// This way you can build up validation chains that can be executed with a 
        /// single <see cref="RuleExecuter{TDataNew,TResultClass}.Assert()"/> method call.
        /// The expression will be executed only once. 
        /// </summary>
        /// <typeparam name="TDataNew">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="MessageCollection{TDataNew}"/> to check the rules.</returns>
        public MessageCollection<TDataNew> ForMessages<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newExecuter = new MessageCollection<TDataNew>(data, this.MethodRuleAttributes);
            this.previousExecuter = () => newExecuter.Assert();
            return newExecuter;
        }

        /// <summary>
        /// Adds the entry to the <see cref="Results"/>, if the validadtion did fail.
        /// </summary>
        /// <param name="validationResult">The rule validation result structure with information about the rule validation process.</param>
        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            if (!validationResult.Result)
            {
                this.Results.Add(validationResult);
            }
        }
    }
}