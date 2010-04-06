// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenXmlHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the OpenXmlHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    public static class OpenXmlHelper
    {
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            if (cell.DataType == null || cell.DataType.Value != CellValues.SharedString)
            {
                return cell.CellValue.Text;
            }

            var shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            var items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();
            return items[int.Parse(cell.CellValue.Text)].InnerText;
        }

        public static int GetRegExResultInt(this string stringToMatch, string regex)
        {
            return int.Parse(stringToMatch.GetRegExResult(regex));
        }

        public static string GetRegExResult(this string stringToMatch, string regex)
        {
            return new Regex(regex).Match(stringToMatch).Groups[1].ToString();
        }

        public static int LettersToIndex(this string letters)
        {
            var array = letters.ToCharArray();
            var result = 0;
            foreach (var character in array)
            {
                result = result * 26;
                result += Convert.ToByte(character) - 64;
            }

            return result;
        }
        
        /// <summary>
        /// Adds cells to a row
        /// </summary>
        /// <param name="row"> The row to add the cells to. </param>
        /// <param name="reference"> The reference. </param>
        /// <param name="text"> The text of the cell. </param>
        public static void CreateCell(this Row row, string reference, string text)
        {
            var cell1 = new Cell { CellReference = reference, DataType = CellValues.String };
            var cellValue1 = new CellValue { Text = text };

            cell1.Append(cellValue1);
            row.Append(cell1);
        }

        public static string IndexToLetters(this int index)
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

        // Given a cell name, parses the specified cell to get the row index.
        public static uint GetRowIndex(this string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            var regex = new Regex(@"\d+");
            var match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
    }
}