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
        public static string ConcatElementsToString(this IEnumerable<string> sourceValue, string separator)
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

        /// <summary>
        /// Tests a value for being NULL and returns a new object of the same type if it is null.
        /// Returns the tested object if it was not null.
        /// </summary>
        /// <param name="testObject"> The test object. </param>
        /// <typeparam name="T"> The type to test and return </typeparam>
        /// <returns> the non-null value or a new object if the original value was null </returns>
        public static T NewIfNull<T>(this T testObject) where T : class, new()
        {
            return testObject ?? new T();
        }

        /// <summary>
        /// Tests a an array of values for being NULL or not containing the desired index 
        /// and returns a new object of the element type of the array if either the array
        /// is null or the element does not exist.
        /// Returns the array element if it does exist.
        /// </summary>
        /// <param name="testObject"> The array to get the element from.  </param>
        /// <param name="index"> The index. </param>
        /// <typeparam name="T"> The type of elements inside the array </typeparam>
        /// <returns> the existing element or a new element, if the element does not exist  </returns>
        public static T NewIfNull<T>(this T[] testObject, int index) where T : class, new()
        {
            var x = testObject ?? new T[0];
            return x.Length > index ? x[index] : new T();
        }

        /// <summary>
        /// Tests a an List of values for being NULL or not containing the desired index 
        /// and returns a new object of the element type of the List if either the List
        /// is null or the element does not exist.
        /// Returns the List element if it does exist.
        /// </summary>
        /// <param name="testObject"> The List of elements.  </param>
        /// <param name="index"> The index. </param>
        /// <returns> the existing element or a new element, if the element does not exist  </returns>
        public static string NewIfNull(this List<string> testObject, int index)
        {
            var x = testObject ?? new List<string>();
            return x.Count > index ? x[index] : string.Empty;
        }

        /// <summary>
        /// Tests a an List of values for being NULL or not containing the desired index 
        /// and returns a new object of the element type of the List if either the List
        /// is null or the element does not exist.
        /// Returns the List element if it does exist.
        /// </summary>
        /// <param name="testObject"> The List of elements.  </param>
        /// <param name="index"> The index. </param>
        /// <typeparam name="T"> The type of elements inside the collection </typeparam>
        /// <returns> the existing element or a new element, if the element does not exist  </returns>
        public static T NewIfNull<T>(this List<T> testObject, int index)
            where T : class, new()
        {
            var x = testObject ?? new List<T>();
            return x.Count > index ? x[index] : new T();
        }

        /// <summary>
        /// Tests a a Dictionary of values for being NULL or not containing the desired Key
        /// and returns a new object of the values type of the Dictionary if either the Dictionary
        /// is null or the element does not exist.
        /// Returns the Dictionary element if it does exist.
        /// </summary>
        /// <param name="testObject"> The Dictionary of elements.  </param>
        /// <param name="key"> The key of the element to search. </param>
        /// <typeparam name="T"> The type of elements inside the Dictionary </typeparam>
        /// <returns> the existing element or a new element, if the element does not exist  </returns>
        public static T NewIfNull<T>(this Dictionary<string, T> testObject, string key) where T : class, new()
        {
            var x = testObject ?? new Dictionary<string, T>();
            return x.ContainsKey(key) ? x[key] : new T();
        }

        /// <summary>
        /// Returns a default value in case of an empty/null-string
        /// </summary>
        /// <param name="toCheck"> The string to check. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The default string if the checked string is null or empty, the checked string otherwise </returns>
        public static string DefaultIfNullOrEmpty(this string toCheck, string defaultValue)
        {
            return string.IsNullOrEmpty(toCheck) ? defaultValue : toCheck;
        }
    }
}
