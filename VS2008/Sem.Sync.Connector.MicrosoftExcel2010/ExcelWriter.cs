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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    using Sem.GenericHelpers;

    /// <summary>
    /// The ExcelWriter class does provide functionality to write SpreadsheetML excel sheets.
    /// </summary>
    public class ExcelWriter
    {
        /// <summary>
        /// Generates a SpreadsheetML worksheet from a list of objects.
        /// </summary>
        /// <typeparam name="T"> The type of object to serialize. </typeparam>
        /// <param name="listToConvert"> List of objects to be converted - each property path will be serialized to one column  </param>
        /// <returns> The exported worksheet.  </returns>
        public static string ExportToWorksheet<T>(List<T> listToConvert)
        {
            var result = new StringBuilder();
            using (var stringwriter = new StringWriter(result))
            {
                var writer = new XmlTextWriter(stringwriter)
                                 {
                                     Formatting = Formatting.Indented
                                 };

                writer.WriteStartDocument();
                writer.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                writer.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
                writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
                writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");

                writer.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");
                writer.WriteElementString("Author", typeof(ExcelWriter).FullName);
                writer.WriteElementString("LastAuthor", Environment.UserName);
                writer.WriteElementString("Created", DateTime.Now.ToString("u") + "Z");
                writer.WriteElementString("Company", "Sem.Sync");
                writer.WriteElementString("Version", "11.8122");
                writer.WriteEndElement();

                writer.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");
                writer.WriteElementString("WindowHeight", "13170");
                writer.WriteElementString("WindowWidth", "17580");
                writer.WriteElementString("WindowTopX", "120");
                writer.WriteElementString("WindowTopY", "60");
                writer.WriteElementString("ProtectStructure", "False");
                writer.WriteElementString("ProtectWindows", "False");
                writer.WriteEndElement();

                writer.WriteStartElement("Styles");
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("ss", "ID", null, "Default");
                writer.WriteAttributeString("ss", "Name", null, "Normal");
                writer.WriteStartElement("Alignment");
                writer.WriteAttributeString("ss", "Vertical", null, "Bottom");
                writer.WriteEndElement();
                writer.WriteElementString("Borders", null);
                writer.WriteElementString("Font", null);
                writer.WriteElementString("Interior", null);
                writer.WriteElementString("NumberFormat", null);
                writer.WriteElementString("Protection", null);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("Worksheet");
                writer.WriteAttributeString("ss", "Name", null, "SyncData");

                var properties = Tools.GetPropertyList(string.Empty, typeof(T));

                writer.WriteStartElement("Table");
                writer.WriteAttributeString("ss", "ExpandedColumnCount", null, properties.Count.ToString());
                writer.WriteAttributeString("ss", "ExpandedRowCount", null, listToConvert.Count.ToString());
                writer.WriteAttributeString("x", "FullColumns", null, "1");
                writer.WriteAttributeString("x", "FullRows", null, "1");
                writer.WriteAttributeString("ss", "DefaultColumnWidth", null, "120");

                foreach (var element in listToConvert)
                {
                    writer.WriteStartElement("Row");

                    foreach (var propertPath in properties)
                    {
                        writer.WriteStartElement("Cell");

                        writer.WriteStartElement("Data");
                        writer.WriteAttributeString("ss", "Type", null, "String");
                        writer.WriteValue(Tools.GetPropertyValueString(element, propertPath));
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteStartElement("WorksheetOptions", "urn:schemas-microsoft-com:office:excel");

                writer.WriteStartElement("PageSetup");
                writer.WriteStartElement("Header");
                writer.WriteAttributeString("x", "Margin", null, "0.5");
                writer.WriteEndElement();
                writer.WriteStartElement("Footer");
                writer.WriteAttributeString("x", "Margin", null, "0.5");
                writer.WriteEndElement();
                writer.WriteStartElement("PageMargins");
                writer.WriteAttributeString("x", "Bottom", null, "1");
                writer.WriteAttributeString("x", "Left", null, "1");
                writer.WriteAttributeString("x", "Right", null, "1");
                writer.WriteAttributeString("x", "Top", null, "1");
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteElementString("Selected", null);

                writer.WriteStartElement("Panes");
                writer.WriteStartElement("Pane");
                writer.WriteElementString("Number", "1");
                writer.WriteElementString("ActiveRow", "1");
                writer.WriteElementString("ActiveCol", "1");
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteElementString("ProtectObjects", "False");
                writer.WriteElementString("ProtectScenarios", "False");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                // enforce write
                writer.Flush();
            }
 
            return result.ToString();
        }
    }
}