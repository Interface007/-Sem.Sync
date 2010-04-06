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
    /// </summary>
    public static class OpenXmlGenerator
    {
        /// <summary>
        /// Creates a SpreadsheetDocument
        /// </summary>
        /// <param name="filePath"> The file path. </param>
        public static void CreatePackage(string filePath, string[,] strings)
        {
            using (var package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = package.AddWorkbookPart();

                var workbook = new Workbook();
                workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

                var sheets = new Sheets();
                var sheet = new Sheet { Name = "Sem.Sync", SheetId = 1U, Id = "rId1" };

                sheets.Append(sheet);

                var bookViews = new BookViews();
                var workbookView = new WorkbookView { XWindow = 240, YWindow = 60, WindowWidth = 14355U, WindowHeight = 6720U };
                bookViews.Append(workbookView);
                workbook.Append(bookViews);

                workbook.Append(sheets);

                workbookPart.Workbook = workbook;

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");
                var worksheet = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
                worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

                var sheetData1 = new SheetData();

                var rowCount = strings.GetLength(0);
                var colCount = strings.GetLength(1);

                for (var i = 1; i < rowCount; i++)
                {
                    var row = new Row
                                  {
                                      RowIndex = (uint)i,
                                      DyDescent = 0.25D, 
                                      Spans = new ListValue<StringValue>
                                                  {
                                                      InnerText = "1:" + colCount
                                                  }
                                  };

                    for (var j = 1; j < colCount; j++)
                    {
                        row.CreateCell(j.IndexToLetters() + i, strings[i, j]);
                    }

                    sheetData1.Append(row);
                }

                var sheetDimension = new SheetDimension
                                         {
                                             Reference = "A1:" + colCount.IndexToLetters() + rowCount
                                         };
                worksheet.Append(sheetDimension);

                var sheetViews = new SheetViews();
                var sheetView = new SheetView { TabSelected = true, WorkbookViewId = 0U };
                sheetViews.Append(sheetView);
                worksheet.Append(sheetViews);
                
                worksheet.Append(sheetData1);

                worksheetPart.Worksheet = worksheet;
                
                package.PackageProperties.Creator = System.Environment.UserName;
                package.PackageProperties.Created = DateTime.Now;
                package.PackageProperties.Modified = DateTime.Now;
                package.PackageProperties.LastModifiedBy = Environment.UserName;
            }
        }
    }
}