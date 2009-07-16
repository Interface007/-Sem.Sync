using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.GenericHelpers
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a list of strings and concatenates them with a seperator.
        /// </summary>
        /// <param name="sourceValue">the list of source values in a list of strings</param>
        /// <param name="seperator">the seperator to insert between the string items</param>
        /// <returns>one string containing all provided substrings seperated by the specified seperator</returns>
        public static string ConcatElementsToString(this List<string> sourceValue, string seperator)
        {
            var result = new StringBuilder();
            var addSeperator = false;
            foreach (var element in sourceValue)
            {
                if (addSeperator)
                {
                    result.Append(seperator);
                }

                result.Append(element);
                addSeperator = true;
            }

            return result.ToString();
        }
    }
}
