﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelXml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Enthält Hilfsfunktionen zum Erzeugen von Excel-Dateien mit SpreadsheetML.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MicrosoftExcel2010
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Sem.GenericHelpers;

    /// <summary>
    /// The ExcelWriter class does provide functionality to write SpreadsheetML excel sheets.
    /// </summary>
    public class ExcelXml
    {
        /// <summary>
        /// Namespace declaration for office specific properties
        /// </summary>
        private static readonly XNamespace MicrosoftOffice = "urn:schemas-microsoft-com:office:office";

        /// <summary>
        /// Namespace declaration for excel specific properties
        /// </summary>
        private static readonly XNamespace OfficeExcel = "urn:schemas-microsoft-com:office:excel";

        /// <summary>
        /// Namespace declaration for spread sheet data
        /// </summary>
        private static readonly XNamespace SpreadSheet = "urn:schemas-microsoft-com:office:spreadsheet";

        private static readonly XName SpreadSheetWorkbook = SpreadSheet + "Workbook";
        private static readonly XName SpreadSheetWorksheet = SpreadSheet + "Worksheet";
        private static readonly XName SpreadSheetTable = SpreadSheet + "Table";
        private static readonly XName SpreadSheetRow = SpreadSheet + "Row";
        private static readonly XName SpreadSheetCell = SpreadSheet + "Cell";

        /// <summary>
        /// Generates a SpreadsheetML worksheet from a list of objects using XLinq - this may be 
        /// an issue for huge lists of data, but who cares ;-).
        /// </summary>
        /// <typeparam name="T"> The type of object to serialize. </typeparam>
        /// <param name="listToConvert"> List of objects to be converted - each property path will be serialized to one column  </param>
        /// <returns> The exported worksheet.  </returns>
        public static string ExportToWorksheet<T>(List<T> listToConvert)
        {
            var properties = Tools.GetPropertyList(string.Empty, typeof(T));

            var nresult = new XDocument(
                new XProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""),
                new XElement(SpreadSheetWorkbook,
                    new XElement(MicrosoftOffice + "DocumentProperties",
                        new XElement(MicrosoftOffice + "Version", "14.00")
                        ),
                    new XElement(MicrosoftOffice + "OfficeDocumentSettings",
                        new XElement(MicrosoftOffice + "AllowPNG")
                        ),
                    new XElement(OfficeExcel + "ExcelWorkbook",
                        new XElement(SpreadSheet + "WindowHeight", "13000"),
                        new XElement(SpreadSheet + "WindowWidth", "15000"),
                        new XElement(SpreadSheet + "WindowTopX", "0"),
                        new XElement(SpreadSheet + "WindowTopY", "0"),
                        new XElement(SpreadSheet + "ProtectStructure", "False"),
                        new XElement(SpreadSheet + "ProtectWindows", "False")
                        ),
                    new XElement(SpreadSheet + "Styles",
                        new XElement(SpreadSheet + "Style",
                            new XAttribute(SpreadSheet + "ID", "Default"),
                            new XAttribute(SpreadSheet + "Name", "Normal"),
                            new XElement(SpreadSheet + "Alignment",
                                new XAttribute(SpreadSheet + "Vertical", "Bottom")
                                )
                            )
                        ),
                    new XElement(SpreadSheetWorksheet,
                        new XAttribute(SpreadSheet + "Name", "SyncData"),

                        // this will create the data area for the sheet
                        new XElement(SpreadSheetTable,
                            new XAttribute(SpreadSheet + "ExpandedColumnCount", properties.Count.ToString()),

                            // the row count is the number of elements + 1 because we will add the 
                // paths of the properties as the heading (= one additional row)
                            new XAttribute(SpreadSheet + "ExpandedRowCount", (listToConvert.Count + 1).ToString()),
                            new XAttribute(OfficeExcel + "FullColumns", "1"),
                            new XAttribute(OfficeExcel + "FullRows", "1"),
                            new XAttribute(SpreadSheet + "DefaultColumnWidth", "120"),

                            // this will include the paths of the properties as the first row
                            new XElement(SpreadSheetRow,
                                from propertPath in properties
                                select
                                    new XElement(SpreadSheetCell,
                                        new XElement(SpreadSheet + "Data",
                                            new XAttribute(SpreadSheet + "Type", "String"),
                                            propertPath
                                        )
                                    )
                            ),

                            // create a list of rows
                            from element in listToConvert
                            select
                                new XElement(SpreadSheetRow,

                                    // create a list of cells for this row
                                    from propertPath in properties
                                    select
                                        new XElement(SpreadSheetCell,
                                            new XElement(SpreadSheet + "Data",
                                                new XAttribute(SpreadSheet + "Type", "String"),
                                                Tools.GetPropertyValueString(element, propertPath)
                                            )
                                        )
                                )
                            )
                        )
                    )
                );

            // Excel does need this little string at the beginning of the file... so we simply add it as a string
            return "<?xml version=\"1.0\"?>" + nresult;
        }

        /// <summary>
        /// Imports data from a worksheet into a list of objects. Currently the column headers need to be the
        /// property paths - we will change this to allow a configuration file for the mapping in a future 
        /// version.
        /// </summary>
        /// <param name="xml"> The xml from where to import the data. </param>
        /// <typeparam name="T"> The type of object for the list. </typeparam>
        /// <returns> A newly created list of objects. </returns>
        public static List<T> ImportFromWorksheet<T>(string xml)
            where T : class, new()
        {
            var document = XDocument.Parse(xml);
            var list = new List<T>();

            // initialize with an empty list in order to get no NULL reference exception
            // if the data cannot be found
            IEnumerable<XElement> data = new List<XElement>();

            // suppress nulls by creating new elements if needed - 
            // we simply need the Rows. If there are no rown, we get NULL, 
            // what does exactly represent what we want.
            // ReSharper disable PossibleNullReferenceException
            document.MapIfExist2(
                        x => x.Element(SpreadSheetWorkbook)
                         .Element(SpreadSheetWorksheet)
                         .Element(SpreadSheetTable)
                         .Elements(SpreadSheetRow), 
                        ref data);

            // ReSharper restore PossibleNullReferenceException
            var columns = new XElement[0];
            var isFirstRow = true;

            foreach (var row in data)
            {
                // extract the "paths" to the properties of the object.
                // this should be more comfortable by allowing to specify a 
                // configuration file for column headers/paths
                if (isFirstRow)
                {
                    columns = row.Elements(SpreadSheetCell).ToArray();
                    isFirstRow = false;
                    continue;
                }

                var cellIndex = 0;
                var newElement = new T();
                foreach (var cell in row.Elements(SpreadSheetCell))
                {
                    Tools.SetPropertyValue(newElement, columns[cellIndex].Value, cell.Value);
                    cellIndex++;
                }

                list.Add(newElement);
            }

            return list;
        }
    }
}