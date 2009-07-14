namespace Sem.Sync.StatisticConnector
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    using GenericHelpers.Entities;

    using SyncBase.Helpers;

    internal static class PropertyUsageCounter
    {
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

        private static void AddPropertyCounts(object element, string root, IDictionary<string, int> propList)
        {
            var myType = element.GetType();
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
