// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Triple.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   A class implementing three generic values
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    /// <summary>
    /// A class implementing three generic values
    /// </summary>
    /// <typeparam name="T1"> The type of the first value </typeparam>
    /// <typeparam name="T2"> The type of the second value </typeparam>
    /// <typeparam name="T3"> The type of the thirt value </typeparam>
    public class Triple<T1, T2, T3>
    {
        /// <summary>
        /// Gets or sets Value1.
        /// </summary>
        public T1 Value1 { get; set; }

        /// <summary>
        /// Gets or sets Value2.
        /// </summary>
        public T2 Value2 { get; set; }

        /// <summary>
        /// Gets or sets Value3.
        /// </summary>
        public T3 Value3 { get; set; }
    }
}
