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

    public static class Bouncer 
    {
        /// <summary>
        /// Creates a data structure for executing rules
        /// by specifying a lambra expression:
        /// <para>Bouncer.For(() => MessageOneOk).Assert();</para>
        /// The expression will be executed only once.
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

        public static MessageCollection<TData> ForMessages<TData>(Expression<Func<TData>> data)
        {
            return new MessageCollection<TData>(data);
        }

        public static MessageCollection<TData> ForMessages<TData>(TData data, string name)
        {
            return new MessageCollection<TData>(name, data);
        }
    }
}
