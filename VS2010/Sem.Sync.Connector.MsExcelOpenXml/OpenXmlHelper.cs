// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenXmlHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Helper class for OpenXml manipulation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    /// <summary>
    /// Helper class for OpenXml manipulation
    /// </summary>
    internal static class OpenXmlHelper
    {
        #region Constants and Fields

        /// <summary>
        ///   internal ID for the sheet to be created
        /// </summary>
        private const string SheetId = "rId1";

        /// <summary>
        ///   The regular expression for integer extraction.
        /// </summary>
        private static readonly Regex RegexIntegers = new Regex(@"\d+", RegexOptions.Compiled);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "OpenXmlHelper" /> class.
        /// </summary>
        static OpenXmlHelper()
        {
            WorkSheetName = "Sem.Sync";
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the name of the worksheet to be created - default = "Sem.Sync".
        /// </summary>
        internal static string WorkSheetName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds cells to a row
        /// </summary>
        /// <param name="row">
        /// The row to add the cells to. 
        /// </param>
        /// <param name="reference">
        /// The reference. 
        /// </param>
        /// <param name="text">
        /// The text of the cell. 
        /// </param>
        internal static void CreateCell(this Row row, string reference, string text)
        {
            var cell1 = new Cell { CellReference = reference, DataType = CellValues.String };
            var cellValue1 = new CellValue { Text = text };

            cell1.Append(cellValue1);
            row.Append(cell1);
        }

        /// <summary>
        /// Reads the string value of a cell (including string table lookup).
        /// </summary>
        /// <param name="cell">
        /// The cell to get the value from. 
        /// </param>
        /// <param name="items">
        /// The string lookup array for string reference cells. 
        /// </param>
        /// <returns>
        /// The string value of the cell
        /// </returns>
        internal static string GetCellValue(Cell cell, SharedStringItem[] items)
        {
            if (cell.DataType == null || cell.DataType.Value != CellValues.SharedString)
            {
                return cell.CellValue.Text;
            }

            return items[int.Parse(cell.CellValue.Text, CultureInfo.InvariantCulture)].InnerText;
        }

        /// <summary>
        /// Extracts one group using a Regex
        /// </summary>
        /// <param name="stringToMatch">
        /// The string to match. 
        /// </param>
        /// <param name="regex">
        /// The regex to extract information. 
        /// </param>
        /// <returns>
        /// the first group of the match 
        /// </returns>
        internal static string GetRegExResult(this string stringToMatch, string regex)
        {
            return new Regex(regex).Match(stringToMatch).Groups[1].ToString();
        }

        /// <summary>
        /// Extracts one group using a Regex and returns it as int
        /// </summary>
        /// <param name="stringToMatch">
        /// The string to match. 
        /// </param>
        /// <param name="regex">
        /// The regex to extract information. 
        /// </param>
        /// <returns>
        /// the first group of the match as int
        /// </returns>
        internal static int GetRegExResultInt(this string stringToMatch, string regex)
        {
            return int.Parse(stringToMatch.GetRegExResult(regex), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the row index.
        /// </summary>
        /// <param name="cellName">
        /// the alpha numeric cell reference (e.g. "AB:167" =&gt; 167)
        /// </param>
        /// <returns>
        /// the row index
        /// </returns>
        internal static int GetRowIndex(this string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            var match = RegexIntegers.Match(cellName);

            return int.Parse(match.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Extracts an array of strings from the first sheet of an excel file
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        /// <returns>
        /// a two dimensional array of strings 
        /// </returns>
        internal static string[,] GetValueArrayFromExcelFile(string clientFolderName)
        {
            // in case of no valid data we will return an empty array
            var valueArray = new string[0, 0];

            // open the document
            using (var document = SpreadsheetDocument.Open(clientFolderName, false))
            {
                // get the first sheet
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().First();
                if (sheet == null)
                {
                    return valueArray;
                }

                var worksheet = ((WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id)).Worksheet;

                // Get the cells as one list (no deferred execution!)
                IEnumerable<Cell> cells = worksheet.Descendants<Cell>().ToList();

                if (cells.Count() == 0)
                {
                    return valueArray;
                }

                // we should transform the shared strings into an array to have really quick access
                var shareStringArray = GetSharedStringArray(document);

                // find out dimension to setup the array of cell values and setup a corresponding value array
                valueArray = SetupArrayByDimension(worksheet, cells);

                // iterate through the cells and copy the content into the correct value array element
                foreach (var cell in cells)
                {
                    var value = cell.CellReference.Value;
                    var indexRow = value.GetRowIndex() - 1;
                    var indexCol = value.LettersToIndex() - 1;

                    // the helper will handle string table lookups
                    valueArray[indexRow, indexCol] = OpenXmlHelper.GetCellValue(cell, shareStringArray);
                }
            }

            return valueArray;
        }

        /// <summary>
        /// Calculates the column index character sequence for ExCel. 1=A, 2=B, ...
        ///   <para>
        /// In case of high numbers (&gt; 26) additional characters will be added (AB, AC, AD...).
        /// </para>
        /// </summary>
        /// <param name="index">
        /// The index to be translated. 
        /// </param>
        /// <returns>
        /// The column index as characters 
        /// </returns>
        internal static string IndexToLetters(this int index)
        {
            var result = string.Empty;
            index--;

            while (true)
            {
                result = Encoding.ASCII.GetString(new[] { (byte)((index % 26) + 65) }) + result;
                if (index < 26)
                {
                    return result;
                }

                index = (index / 26) - 1;
            }
        }

        /// <summary>
        /// Convert an excel character-like index (column) to an integer. e.g. "A" = 1, "B" = 2 ...
        ///   <para>
        /// This conversion will stop an any character that is not inthe range of A..Z
        /// </para>
        /// </summary>
        /// <param name="letters">
        /// The letters to be translated. 
        /// </param>
        /// <returns>
        /// the column index 
        /// </returns>
        internal static int LettersToIndex(this string letters)
        {
            var array = letters.ToCharArray();
            var result = 0;
            foreach (var character in array)
            {
                if (character.CompareTo('A') < 0 || character.CompareTo('Z') > 0)
                {
                    return result;
                }

                result = result * 26;
                result += Convert.ToByte(character) - 64;
            }

            return result;
        }

        /// <summary>
        /// Generates an ExCel file from an array of strings
        /// </summary>
        /// <param name="filePath">
        /// The file path to the file to be created.  
        /// </param>
        /// <param name="strings">
        /// The strings of the cells to be created. 
        /// </param>
        internal static void WriteStringArrayToExcelfile(string filePath, string[,] strings)
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
                    var row = new Row
                        {
                            RowIndex = (uint)i, 
                            DyDescent = 0.25D, 
                            Spans = new ListValue<StringValue> { InnerText = "1:" + colCount } 
 };
                    for (var j = 1; j < colCount; j++)
                    {
                        row.CreateCell(j.IndexToLetters() + i, strings[i, j]);
                    }

                    sheetData.Append(row);
                }

                // attach the sheet data to the worksheet
                worksheet.Append(sheetData);

                SetGenericDocumentProperties(package);
            }
        }

        /// <summary>
        /// Generates a workbook for one sheet
        /// </summary>
        /// <param name="package">
        /// The target package of for the workbook. 
        /// </param>
        /// <returns>
        /// a new generated worksheet 
        /// </returns>
        private static Worksheet BuildWorkbookPart(SpreadsheetDocument package)
        {
            // create the workbook as the base of the document
            var workbook = new Workbook();
            workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            var workbookPart = package.AddWorkbookPart();
            workbookPart.Workbook = workbook;

            // create workbook view and attach it to the workbook
            var bookViews = new BookViews();
            bookViews.Append(
                new WorkbookView { XWindow = 240, YWindow = 60, WindowWidth = 14355U, WindowHeight = 6720U });
            workbook.Append(bookViews);

            // create the sheet with a predefined name and attach it to the workbook
            var sheets = new Sheets();
            sheets.Append(new Sheet { Name = WorkSheetName, SheetId = 1U, Id = SheetId });
            workbook.Append(sheets);

            // create worksheet as the base for the data
            var worksheet = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration(
                "r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            // create the sheet view and attach that to the worksheet
            var sheetViews = new SheetViews();
            sheetViews.Append(new SheetView { TabSelected = true, WorkbookViewId = 0U });
            worksheet.Append(sheetViews);

            // create a new worksheet part from the workbook to contain the worksheet
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>(SheetId);
            worksheetPart.Worksheet = worksheet;

            return worksheet;
        }

        /// <summary>
        /// Extracts the lookup string table.
        /// </summary>
        /// <param name="document">
        /// The document that may contain such a lookup table. 
        /// </param>
        /// <returns>
        /// The extracted strings as a one dimensional array (the array may be empty, but not null) 
        /// </returns>
        private static SharedStringItem[] GetSharedStringArray(SpreadsheetDocument document)
        {
            var sharedStrings = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>();
            var shareStringArray = new SharedStringItem[] { };
            if (sharedStrings.Count() > 0)
            {
                var sharedStringTablePart = sharedStrings.First();
                shareStringArray = sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray();
            }

            return shareStringArray;
        }

        /// <summary>
        /// setup some generic sheet properties
        /// </summary>
        /// <param name="package">
        /// the package to update
        /// </param>
        private static void SetGenericDocumentProperties(OpenXmlPackage package)
        {
            package.PackageProperties.Creator = System.Environment.UserName;
            package.PackageProperties.Created = DateTime.Now;
            package.PackageProperties.Modified = DateTime.Now;
            package.PackageProperties.LastModifiedBy = Environment.UserName;
        }

        /// <summary>
        /// Reads the dimensions of a worksheets or probes the cells in order to find the
        ///   dimensions for the value array and sets up an array of strings for the values.
        /// </summary>
        /// <param name="worksheet">
        /// The worksheet to analyze. 
        /// </param>
        /// <param name="cells">
        /// The columns containing the data.
        /// </param>
        /// <returns>
        /// The array of strings for the values of the cells. 
        /// </returns>
        private static string[,] SetupArrayByDimension(Worksheet worksheet, IEnumerable<Cell> cells)
        {
            var maxColIndex = 0;
            var maxRowIndex = 0;

            if (worksheet.SheetDimension != null)
            {
                var dimension = worksheet.SheetDimension;
                var dimensionValue = dimension.Reference.Value;
                maxRowIndex = dimensionValue.GetRegExResultInt(@".+\:[A-Za-z]+(\d+)");
                maxColIndex = dimensionValue.GetRegExResult(@".+\:([A-Za-z]+)\d+").LettersToIndex();
            }
            else
            {
                foreach (var cell in cells)
                {
                    var cellReference = cell.CellReference.Value;
                    var cellColIndex = cellReference.LettersToIndex();
                    if (cellColIndex > maxColIndex)
                    {
                        maxColIndex = cellColIndex;
                    }

                    var cellRowIndex = cellReference.GetRegExResultInt(@"[A-Za-z]+(\d+)");
                    if (cellRowIndex > maxRowIndex)
                    {
                        maxRowIndex = cellRowIndex;
                    }
                }
            }

            return new string[maxRowIndex, maxColIndex];
        }

        #endregion
    }
}