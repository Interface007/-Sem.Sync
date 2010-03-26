// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Writes a list of "something" into an excel OpenXml file
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.GenericHelpers;

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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

            var mappingFileName = GetColumnDefinitionFileName(clientFolderName);
            var mapping = this.GetColumnDefinition<StdContact>(mappingFileName);

            // Open the document as read-only.
            using (var document = SpreadsheetDocument.Open(GetFileName(clientFolderName), false))
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
                    return result;
                }

                var rowStart =
                    int.Parse(
                        new Regex(@"[A-Za-z]+(\d+)\:.+").Match(worksheetPart.Worksheet.SheetDimension.Reference.Value).
                            Groups[1].ToString());
                var rowEnd =
                    int.Parse(
                        new Regex(@".+\:[A-Za-z]+(\d+)").Match(worksheetPart.Worksheet.SheetDimension.Reference.Value).
                            Groups[1].ToString());

                var colStart =
                    new Regex(@"([A-Za-z]+)\d+\:.+").Match(worksheetPart.Worksheet.SheetDimension.Reference.Value).
                        Groups[1].ToString();
                var colEnd =
                    new Regex(@".+\:([A-Za-z]+)\d+").Match(worksheetPart.Worksheet.SheetDimension.Reference.Value).
                        Groups[1].ToString();

                var colStartIndex = Convert.ToByte(colStart.ToCharArray()[0]) - 64;
                var array = colEnd.ToCharArray();
                var colEndIndex = Convert.ToByte(array[0]) - 64;
                if (array.Length > 1)
                {
                    colEndIndex = (colEndIndex * 26) + Convert.ToByte(array[1]) - 64;
                }

                for (var rowId = rowStart + 1; rowId <= rowEnd; rowId++)
                {
                    var colIndex = 0;
                    var newElement = new StdContact();
                    for (var colId = colStartIndex; colId <= colEndIndex; colId++)
                    {
                        var colSelectorChars = GetColSelectorChars(colId);
                        var id = rowId;
                        var cell = cells.Where(x => x.CellReference == colSelectorChars + id).FirstOrDefault();
                        string cellValue;

                        // If the content of the first cell is stored as a shared string, get the text of the first cell
                        // from the SharedStringTablePart and return it. Otherwise, return the string value of the cell.
                        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                        {
                            var shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                            var items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                            cellValue = items[int.Parse(cell.CellValue.Text)].InnerText;
                        }
                        else
                        {
                            cellValue = cell.CellValue.Text;
                        }

                        Tools.SetPropertyValue(newElement, mapping[colIndex].Selector, cellValue);

                        colIndex++;
                    }

                    result.Add(newElement);
                }
            }

            return result;
        }

        public static string GetColSelectorChars(int colId)
        {
            var result = string.Empty;
            colId--;

            while (true)
            {
                result = Encoding.ASCII.GetString(new[] { (byte)((colId % 26) + 65) }) + result;
                if (colId < 26)
                {
                    return result;
                }

                colId = (colId / 26) - 1;
            }
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
