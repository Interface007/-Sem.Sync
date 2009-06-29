namespace Sem.Sync.StatisticConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    public class SimpleReport : StdClient
    {
        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Simple Statistic";
            }
        }

        /// <summary>
        /// This client is a write only client, so reading is not supported
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            return result;
        }

        /// <summary>
        /// Writing will write a simple XML with some statistical information.
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var statistic = new SimpleStatisticResult
                {
                    NumberOfElements = elements.Count,
                    PropertyUsage = GetPropertyUsage(elements)
                };

            SyncTools.SaveToFile(statistic, Path.Combine(clientFolderName, this.FriendlyClientName + ".xml"));
        }

        private static List<object> GetPropertyUsage(ICollection<StdElement> elements)
        {
            var result = new List<object>();
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
                    AddPropertyCounts(info.GetValue(element, null), root + "\\" + info.Name, propList);
                }
            }
        }
    }
}