//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers
{
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
    }
}
