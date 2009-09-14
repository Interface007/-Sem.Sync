//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class contains generic extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a list of strings and concatenates them with a separator.
        /// </summary>
        /// <param name="sourceValue">the list of source values in a list of strings</param>
        /// <param name="separator">the seperator to insert between the string items</param>
        /// <returns>one string containing all provided substrings seperated by the specified separator</returns>
        public static string ConcatElementsToString(this List<string> sourceValue, string separator)
        {
            var result = new StringBuilder();
            var addSeparator = false;
            foreach (var element in sourceValue)
            {
                if (addSeparator)
                {
                    result.Append(separator);
                }

                result.Append(element);
                addSeparator = true;
            }

            return result.ToString();
        }

        /// <summary>
        /// Performs an invoke of an <see cref="Action{T}"/> with the element of the IEnumerable as the only parameter for all elements inside the IEnumerable 
        /// </summary>
        /// <param name="enumerable"> The IEnumerable to get the elements from. </param>
        /// <param name="action"> The action to be performed (eg. a lambda). </param>
        /// <typeparam name="T"> the type parameter of the IEnumerable member </typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable)
            {
                action.Invoke(element);
            }
        }

        /// <summary>
        /// Performs an invoke of an <see cref="Action{T}"/> with the element of the IEnumerable and another parameter for all elements inside the IEnumerable 
        /// </summary>
        /// <param name="enumerable"> The IEnumerable to get the elements from.  </param>
        /// <param name="action"> The action to be performed (eg. a lambda).  </param>
        /// <param name="parameter1"> The parameter to call the action. </param>
        /// <typeparam name="T1"> the type parameter of the IEnumerable member  </typeparam>
        /// <typeparam name="T2"> the type parameter of the parameter <paramref name="parameter1"/> member  </typeparam>
        public static void ForEach<T1, T2>(this IEnumerable<T1> enumerable, Action<T1, T2> action, T2 parameter1)
        {
            foreach (var element in enumerable)
            {
                action.Invoke(element, parameter1);
            }
        }
    }
}
