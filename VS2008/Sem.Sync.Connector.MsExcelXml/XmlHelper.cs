namespace Sem.Sync.Connector.MsExcelXml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using GenericHelpers;

    internal static class XmlHelper
    {
        public static void DeserializeList<T>(IEnumerable<XElement> data, List<T> list, XName cellSelector)
            where T : class, new()
        {
            var columns = new XElement[0];
            var isFirstRow = true;

            foreach (var row in data)
            {
                // extract the "paths" to the properties of the object.
                // this should be more comfortable by allowing to specify a 
                // configuration file for column headers/paths
                if (isFirstRow)
                {
                    columns = row.Elements(cellSelector).ToArray();
                    isFirstRow = false;
                    continue;
                }

                var cellIndex = 0;
                var newElement = new T();
                foreach (var cell in row.Elements(cellSelector))
                {
                    Tools.SetPropertyValue(newElement, columns[cellIndex].Value, cell.Value);
                    cellIndex++;
                }

                list.Add(newElement);
            }
        }

    }
}
