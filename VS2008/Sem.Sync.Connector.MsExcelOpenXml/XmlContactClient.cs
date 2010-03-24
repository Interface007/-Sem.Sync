﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Writes a list of "something" into an excel OpenXml file
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// Writes a list of "something" into an excel OpenXml file
    /// </summary>
    [ConnectorDescription(
        DisplayName = "Microsoft Excel OpenXml",
        CanReadContacts = true,
        CanWriteContacts = true,
        Internal = false)]
    [ClientStoragePathDescription(
        Mandatory = true,
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    public class XmlContactClient : StdClient
    {
        /// <summary>
        /// Exporting / writing will simply overwrite the destination, so we should override this method in order 
        /// to not read from the target before writing to it.
        /// </summary>
        /// <param name="elements"> The elements. </param>
        /// <param name="clientFolderName"> The client folder name. </param>
        public override void AddRange(System.Collections.Generic.List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Reads all elements from the excel file
        /// </summary>
        /// <param name="clientFolderName"> The path to the excel file. </param>
        /// <returns> the list of contacts </returns>
        public override List<StdElement> GetAll(string clientFolderName)
        {
            var worksheetName = "1";
            var result = new List<StdElement>();

            // Open the document as read-only.
            using (var document = SpreadsheetDocument.Open(clientFolderName, false))
            {
                var sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName);
                if (sheets.Count() == 0)
                {
                    // The specified worksheet does not exist.
                    sheets = document.WorkbookPart.Workbook.Descendants<Sheet>();
                }

                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);

                // Get the cells in the specified column and order them by row.
                IEnumerable<Cell> cells = 
                    worksheetPart.
                    Worksheet.
                    Descendants<Cell>().
                    OrderBy(r => GetRowIndex(r.CellReference));
                
                if (cells.Count() == 0)
                {
                    // The specified column does not exist.
                    return null;
                }
                var colNameRegEx = new Regex("[A-Za-z]+");
;

                // Get the first cell in the column.
                ////var headCells = cells.Where(x => colNameRegEx.x.CellReference);
                string cellText;

                // If the content of the first cell is stored as a shared string, get the text of the first cell
                // from the SharedStringTablePart and return it. Otherwise, return the string value of the cell.
                ////GetCellValue(document, headCell);


            }

            return result;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell headCell)
        {
            string cellText;
            if (headCell.DataType != null && headCell.DataType.Value == CellValues.SharedString)
            {
                var shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                var items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                return items[int.Parse(headCell.CellValue.Text)].InnerText;
            }
            else
            {
                return headCell.CellValue.Text;
            }
        }

        /// <summary>
        /// Writes the elements to the destination.
        /// </summary>
        /// <param name="elements"> The elements. </param>
        /// <param name="clientFolderName"> The name of the file to write to. </param>
        /// <param name="skipIfExisting"> The flag whether to skip the item if it exist - in this case it's simply ignored, because the target will be overwritten. </param>
        protected override void WriteFullList(System.Collections.Generic.List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (File.Exists(clientFolderName))
            {
                File.Delete(clientFolderName);
            }

            ////File.WriteAllText(clientFolderName, ExcelXml.ExportToWorksheetXml(elements.ToContacts()), Encoding.UTF8);
        }
        
        // Given a cell name, parses the specified cell to get the row index.
        private static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            var regex = new Regex(@"\d+");
            var match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
    }
}
