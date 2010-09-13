// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bouncer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Bouncer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Bouncer http://en.wiktionary.org/wiki/bouncer : „A member of security personnel employed by bars, 
    /// nightclubs, etc to maintain order and deal with patrons who cause trouble.“).
    /// <para>A bouncer can be placed on top of a method to protect against "problematic" data.</para>
    /// </summary>
    public static class Bouncer 
    {
        /// <summary>
        /// Creates a data structure for executing rules
        /// by specifying a lambra expression:
        /// <para>Bouncer.For(() => MessageOneOk).Assert();</para>
        /// The expression will be executed only once. Specifying lambda expression
        /// provides the benefit of strong typing for the data name, because the lambda expression
        /// can be inspected for the variable name.
        /// </summary>
        /// <typeparam name="TData">the type of data the expression returns</typeparam>
        /// <param name="data">the expression</param>
        /// <returns>a <see cref="CheckData{TData}"/> to execute the tests with</returns>
        public static CheckData<TData> ForCheckData<TData>(Expression<Func<TData>> data)
        {
            return new CheckData<TData>(data);
        }

        /// <summary>
        /// Creates a data structure for executing rules:
        /// <para>Bouncer.For(0, "myInt").Assert();</para>
        /// </summary>
        /// <typeparam name="TData">the type of data to be checked</typeparam>
        /// <param name="data">the data to be checked</param>
        /// <param name="name">the name of the parameter/variable to be checked (might be included into check-result messages)</param>
        /// <returns>a <see cref="CheckData{TData}"/> to execute the tests with</returns>
        public static CheckData<TData> ForCheckData<TData>(TData data, string name)
        {
            return new CheckData<TData>(name, data);
        }

        /// <summary>
        /// Creates a rule execution class for a lambda expression that collects the result of the rules
        /// as a collection of <see cref="RuleValidationResult"/>. Specifying lambda expression
        /// provides the benefit of strong typing for the data name, because the lambda expression
        /// can be inspected for the variable name.
        /// </summary>
        /// <typeparam name="TData">the type of data the expression returns</typeparam>
        /// <param name="data">the expression</param>
        /// <returns>a <see cref="MessageCollection{TData}"/> to execute the tests with</returns>
        public static MessageCollection<TData> ForMessages<TData>(Expression<Func<TData>> data)
        {
            return new MessageCollection<TData>(data);
        }

        public static MessageCollection<TData> ForMessages<TData>(TData data, string name)
        {
            return new MessageCollection<TData>(name, data);
        }

        public static ConditionalExecution<TData> ForExecution<TData>(Expression<Func<TData>> data)
        {
            return new ConditionalExecution<TData>(data);
        }

        public static ConditionalExecution<TData> ForExecution<TData>(TData data, string name)
        {
            return new ConditionalExecution<TData>(name, data);
        }
    }
}
