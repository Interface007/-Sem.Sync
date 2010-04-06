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

            // Open the document as read-only.
            using (var document = SpreadsheetDocument.Open(GetFileName(clientFolderName), false))
            {
                // The specified worksheet does not exist.
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().First();
                var worksheet = ((WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id)).Worksheet;

                // Get the cells in the specified column and order them by row.
                IEnumerable<Cell> cells =
                        worksheet.
                        Descendants<Cell>().
                        OrderBy(r => r.CellReference.Value.GetRowIndex());

                if (cells.Count() == 0)
                {
                    // The specified column does not exist.
                    return result;
                }

                var dimension = worksheet.SheetDimension.Reference.Value;

                var rowStart = dimension.GetRegExResultInt(@"[A-Za-z]+(\d+)\:.+");
                var rowEnd = dimension.GetRegExResultInt(@".+\:[A-Za-z]+(\d+)");

                var colStartIndex = dimension.GetRegExResult(@"([A-Za-z]+)\d+\:.+").LettersToIndex();
                var colEndIndex = dimension.GetRegExResult(@".+\:([A-Za-z]+)\d+").LettersToIndex();

                for (var rowId = rowStart + 1; rowId <= rowEnd; rowId++)
                {
                    var colIndex = 0;
                    var newElement = new StdContact();
                    for (var colId = colStartIndex; colId <= colEndIndex; colId++)
                    {
                        var colSelector = colId.IndexToLetters() + rowId;
                        var cell = cells.Where(x => x.CellReference == colSelector).FirstOrDefault();
                        var cellValue = OpenXmlHelper.GetCellValue(document, cell);

                        Tools.SetPropertyValue(newElement, mapping[colIndex].Selector, cellValue);

                        colIndex++;
                    }

                    result.Add(newElement);
                }
            }

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
                var colIndex = 1;
                foreach (var columnDefinition in mapping)
                {
                    matrix[rowIndex, colIndex] = Tools.GetPropertyValueString(element, columnDefinition.Selector);
                    colIndex++;
                }

                rowIndex++;
            }

            OpenXmlGenerator.CreatePackage(GetFileName(clientFolderName), matrix);
        }
    }
}
