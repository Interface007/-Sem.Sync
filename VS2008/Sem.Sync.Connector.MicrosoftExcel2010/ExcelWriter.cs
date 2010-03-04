// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelWriter.cs" company="Sven Erik Matzen">
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
    public class ExcelWriter
    {
        /// <summary>
        /// Generates a SpreadsheetML worksheet from a list of objects using XLinq - this may be 
        /// an issue for huge lists of data, but who cares ;-).
        /// </summary>
        /// <typeparam name="T"> The type of object to serialize. </typeparam>
        /// <param name="listToConvert"> List of objects to be converted - each property path will be serialized to one column  </param>
        /// <returns> The exported worksheet.  </returns>
        public static string ExportToWorksheet<T>(List<T> listToConvert)
        {
            XNamespace oo = "urn:schemas-microsoft-com:office:office";
            XNamespace ox = "urn:schemas-microsoft-com:office:excel";
            XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
            
            var properties = Tools.GetPropertyList(string.Empty, typeof(T));

            var nresult = new XDocument(
                new XProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""),
                new XElement(ss + "Workbook",
                    new XElement(oo + "DocumentProperties",
                        new XElement(oo + "Version", "14.00") 
                        ),
                    new XElement(oo + "OfficeDocumentSettings",
                        new XElement(oo + "AllowPNG") 
                        ),
                    new XElement(ox + "ExcelWorkbook",
                        new XElement(ss + "WindowHeight", "13000"),
                        new XElement(ss + "WindowWidth", "15000"),
                        new XElement(ss + "WindowTopX", "0"),
                        new XElement(ss + "WindowTopY", "0"),
                        new XElement(ss + "ProtectStructure", "False"),
                        new XElement(ss + "ProtectWindows", "False") 
                        ),
                    new XElement(ss + "Styles",
                        new XElement(ss + "Style",
                            new XAttribute(ss + "ID", "Default"),
                            new XAttribute(ss + "Name", "Normal"),
                            new XElement(ss + "Alignment",
                                new XAttribute(ss + "Vertical", "Bottom")
                                )
                            )
                        ),
                    new XElement(ss + "Worksheet",
                        new XAttribute(ss + "Name", "SyncData"),

                        // this will create the data area for the sheet
                        new XElement(ss + "Table",
                            new XAttribute(ss + "ExpandedColumnCount", properties.Count.ToString()),
                            
                            // the row count is the number of elements + 1 because we will add the 
                            // paths of the properties as the heading (= one additional row)
                            new XAttribute(ss + "ExpandedRowCount", (listToConvert.Count + 1).ToString()),
                            new XAttribute(ox + "FullColumns", "1"),
                            new XAttribute(ox + "FullRows", "1"),
                            new XAttribute(ss + "DefaultColumnWidth", "120"),

                            // this will include the paths of the properties as the first row
                            new XElement(ss + "Row",
                                from propertPath in properties
                                select
                                    new XElement(ss + "Cell",
                                        new XElement(ss + "Data",
                                            new XAttribute(ss + "Type", "String"),
                                            propertPath
                                        )
                                    )
                            ),
                            
                            // create a list of rows
                            from element in listToConvert
                            select
                                new XElement(ss + "Row", 
                                    
                                    // create a list of cells for this row
                                    from propertPath in properties
                                    select 
                                        new XElement(ss + "Cell",
                                            new XElement(ss + "Data",
                                                new XAttribute(ss + "Type", "String"),
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
    }
}