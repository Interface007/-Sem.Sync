// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The xml helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelXml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Sem.GenericHelpers;

    /// <summary>
    /// The xml helper.
    /// </summary>
    internal static class XmlHelper
    {
        #region Public Methods

        /// <summary>
        /// The deserialize list.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="cellSelector">
        /// The cell selector.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
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

        #endregion
    }
}