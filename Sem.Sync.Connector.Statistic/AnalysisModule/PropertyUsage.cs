// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyUsage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the PropertyUsageCounter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.AnalysisModule
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Analyzes the usage of properties by count
    /// </summary>
    public static class PropertyUsage
    {
        /// <summary>
        /// Analyzes the usage of properties by count
        /// </summary>
        /// <param name="elements"> The elements to be analyzed. </param>
        /// <returns> a list of key value pairs containing the properties and the usage count </returns>
        public static List<KeyValuePair> GetAnalysisItemResult(ICollection elements)
        {
            var result = new List<KeyValuePair>();
            var propList = new Dictionary<string, int>();
            if (elements.Count > 0)
            {
                foreach (var element in elements)
                {
                    AddPropertyCounts(element, "\\", propList);
                }

                result.AddRange(from i in propList
                                where i.Value > 0
                                select new KeyValuePair { Key = i.Key, Value = i.Value.ToString(CultureInfo.CurrentCulture) });
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
                if (info.PropertyType.IsValueType)
                {
                    if (info.PropertyType.IsEnum && ((int)info.GetValue(element, null)) == 0)
                    {
                        continue;
                    }
                }

                if (info.PropertyType.Name == "String" && string.IsNullOrEmpty((string)info.GetValue(element, null)))
                {
                    continue;
                }

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
