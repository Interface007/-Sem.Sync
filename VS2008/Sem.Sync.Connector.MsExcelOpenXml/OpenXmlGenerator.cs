// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenXmlGenerator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the GeneratedClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    /// <summary>
    /// Generates an ExCel file from an array of strings
    /// </summary>
    public static class OpenXmlGenerator
    {
        /// <summary>
        /// internal ID for the sheet to be created
        /// </summary>
        private const string SheetId = "rId1";

        /// <summary>
        /// Generates an ExCel file from an array of strings
        /// </summary>
        /// <param name="filePath"> The file path to the file to be created.  </param>
        /// <param name="strings"> The strings of the cells to be created. </param>
        public static void CreatePackage(string filePath, string[,] strings)
        {
            using (var package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                // create a WorkbookView
                var worksheet = BuildWorkbookPart(package);
                var sheetData = new SheetData();

                var rowCount = strings.GetLength(0);
                var colCount = strings.GetLength(1);

                for (var i = 1; i < rowCount; i++)
                {
                    var row = new Row { RowIndex = (uint)i, DyDescent = 0.25D, Spans = new ListValue<StringValue> { InnerText = "1:" + colCount } };
                    for (var j = 1; j < colCount; j++)
                    {
                        row.CreateCell(j.IndexToLetters() + i, strings[i, j]);
                    }

                    sheetData.Append(row);
                }

                // attach the sheet data to the worksheet
                worksheet.Append(sheetData);
                worksheet.Append(new SheetDimension { Reference = "A1:" + colCount.IndexToLetters() + rowCount });
            
                SetGenericDocumentProperties(package);
            }
        }

        /// <summary>
        /// Generates a workbook for one sheet
        /// </summary>
        /// <param name="package"> The target package of for the workbook. </param>
        /// <returns> a new generated worksheet </returns>
        private static Worksheet BuildWorkbookPart(SpreadsheetDocument package)
        {
            // create the workbook and attach the sheet and the bookview
            var workbook = new Workbook();
            workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            var workbookPart = package.AddWorkbookPart();
            workbookPart.Workbook = workbook;

            // create workbook view
            var bookViews = new BookViews();
            bookViews.Append(new WorkbookView { XWindow = 240, YWindow = 60, WindowWidth = 14355U, WindowHeight = 6720U });
            workbook.Append(bookViews);

            // create the sheet 
            var sheets = new Sheets();
            sheets.Append(new Sheet { Name = "Sem.Sync", SheetId = 1U, Id = SheetId });
            workbook.Append(sheets);

            // create worksheet
            var worksheet = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            
            // create the sheet view
            var sheetViews = new SheetViews();
            sheetViews.Append(new SheetView { TabSelected = true, WorkbookViewId = 0U });
            worksheet.Append(sheetViews);
            
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>(SheetId);
            worksheetPart.Worksheet = worksheet;

            return worksheet;
        }

        /// <summary>
        /// setup some generic sheet properties
        /// </summary>
        /// <param name="package">the package to update</param>
        private static void SetGenericDocumentProperties(OpenXmlPackage package)
        {
            package.PackageProperties.Creator = System.Environment.UserName;
            package.PackageProperties.Created = DateTime.Now;
            package.PackageProperties.Modified = DateTime.Now;
            package.PackageProperties.LastModifiedBy = Environment.UserName;
        }
    }
}