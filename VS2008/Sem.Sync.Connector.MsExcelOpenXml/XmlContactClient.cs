// --------------------------------------------------------------------------------------------------------------------
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

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    using Sem.GenericHelpers;
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
            var result = new List<StdElement>();

            var mappingFileName = GetColumnDefinitionFileName(clientFolderName);
            var mapping = this.GetColumnDefinition<StdContact>(mappingFileName);

            this.LogProcessingEvent("Opening Excel file...");

            // Open the document as read-only.
            using (var document = SpreadsheetDocument.Open(GetFileName(clientFolderName), false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().First();
                if (sheet == null)
                {
                    // exit if we don't have a sheet
                    return result;
                }

                var worksheet = ((WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id)).Worksheet;

                // we should transform the shared strings into an array to have really quick access
                var sharedStrings = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>();
                var shareStringArray = new SharedStringItem[0];
                if (sharedStrings.Count() > 0)
                {
                    var sharedStringTablePart = sharedStrings.First();
                    shareStringArray = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                }

                // Get the cells as one list (no deferred execution!)
                IEnumerable<Cell> cells = worksheet.Descendants<Cell>().ToList();

                if (cells.Count() == 0)
                {
                    this.LogProcessingEvent("no cells inside file - read aborted");

                    // exit if we don't have cells
                    return result;
                }

                // find out dimension to setup the array of cell values
                var dimension = worksheet.SheetDimension.Reference.Value;

                var rowStart = dimension.GetRegExResultInt(@"[A-Za-z]+(\d+)\:.+");
                var rowEnd = dimension.GetRegExResultInt(@".+\:[A-Za-z]+(\d+)");

                var colStartIndex = dimension.GetRegExResult(@"([A-Za-z]+)\d+\:.+").LettersToIndex();
                var colEndIndex = dimension.GetRegExResult(@".+\:([A-Za-z]+)\d+").LettersToIndex();

                this.LogProcessingEvent("{0} values in {1} rows and {2} columns found.", (rowEnd - rowStart + 1) * (colEndIndex - colStartIndex + 1), (rowEnd - rowStart + 1), (colEndIndex - colStartIndex + 1));

                var valueArray = new string[rowEnd - rowStart + 1, colEndIndex - colStartIndex + 1];

                // iterate through the cells and copy the content into the correct value array element
                foreach (var cell in cells)
                {
                    var value = cell.CellReference.Value;
                    var indexRow = value.GetRowIndex() - rowStart;
                    var indexCol = value.LettersToIndex() - colStartIndex;

                    // the helper will handle string table lookups
                    valueArray[indexRow, indexCol] = OpenXmlHelper.GetCellValue(cell, shareStringArray);
                }

                this.LogProcessingEvent("data successfully read from excel sheet");

                // copy the values into objects
                for (var rowId = 1; rowId < rowEnd - rowStart + 1; rowId++)
                {
                    var colIndex = 0;
                    var newElement = new StdContact();
                    for (var colId = 0; colId < mapping.Count - 1; colId++)
                    {
                        Tools.SetPropertyValue(newElement, mapping[colIndex].Selector, valueArray[rowId, colId]);
                        colIndex++;
                    }

                    result.Add(newElement);
                    this.LogProcessingEvent(newElement, "contact read");
                }
            }

            this.LogProcessingEvent("cleaning up entities");
            CleanUpEntities(result);

            this.LogProcessingEvent("reading finished");
            return result;
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

            var mappingFileName = GetColumnDefinitionFileName(clientFolderName);
            var mapping = this.GetColumnDefinition<StdContact>(mappingFileName);

            this.LogProcessingEvent("preparing data for {0} contacts and {1} mappings...", elements.Count, mapping.Count);
            CleanUpEntities(elements);

            var matrix = new string[elements.Count + 2, mapping.Count + 1];

            var titleIndex = 1;
            foreach (var columnDefinition in mapping)
            {
                matrix[1, titleIndex] = columnDefinition.Title;
                titleIndex++;
            }

            var rowIndex = 2;
            foreach (var element in elements)
            {
                this.LogProcessingEvent(element, "writing contact...");

                var colIndex = 1;
                foreach (var columnDefinition in mapping)
                {
                    matrix[rowIndex, colIndex] = Tools.GetPropertyValueString(element, columnDefinition.Selector);
                    colIndex++;
                }

                rowIndex++;
            }

            this.LogProcessingEvent("writing data to excel file...");

            OpenXmlGenerator.CreatePackage(GetFileName(clientFolderName), matrix);

            this.LogProcessingEvent("writing finished");
        }
    }
}
