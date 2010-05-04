// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyUsageCounter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the PropertyUsageCounter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    using GenericHelpers.Entities;

    using SyncBase.Helpers;

    /// <summary>
    /// Analyzes the usage of properties by count
    /// </summary>
    internal static class PropertyUsageCounter
    {
        /// <summary>
        /// Analyzes the usage of properties by count
        /// </summary>
        /// <param name="elements"> The elements to be analyzed. </param>
        /// <returns> a list of key value pairs containing the properties and the usage count </returns>
        internal static List<KeyValuePair> GetPropertyUsage(ICollection elements)
        {
            var result = new List<KeyValuePair>();
            var propList = new Dictionary<string, int>();
            if (elements.Count > 0)
            {
                foreach (var element in elements)
                {
                    AddPropertyCounts(element, "\\", propList);
                }

                foreach (var i in propList)
                {
                    if (i.Value > 0)
                    {
                        result.Add(new KeyValuePair { Key = i.Key, Value = i.Value.ToString(CultureInfo.CurrentCulture) });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// analyzes an object for property usage
        /// </summary>
        /// <param name="element"> The element to be analyzed. </param>
        /// <param name="root"> The root of the property path. </param>
        /// <param name="propList"> The prop list to be updated. </param>
        private static void AddPropertyCounts(object element, string root, IDictionary<string, int> propList)
        {
            var myType = element.GetType();
            if (myType.Name == "List`1" || myType.Name == "ProfileIdentifiers")
            {
                return;
            }

            foreach (var info in myType.GetProperties())
            {
                var infoName = root + info.Name;
                if (info.GetValue(element, null) == null)
                {
                    continue;
                }

                if (!propList.ContainsKey(infoName))
                {
                    propList.Add(infoName, 0);
                }
                
                propList[infoName]++;

                if (info.PropertyType.IsClass 
                    && !info.PropertyType.IsPrimitive
                    && !info.PropertyType.IsArray
                    && !info.PropertyType.Name.IsOneOf("String", "DateTime"))
                {
                    AddPropertyCounts(info.GetValue(element, null), root + info.Name + "\\", propList);
                }
            }
        }
    }
}
