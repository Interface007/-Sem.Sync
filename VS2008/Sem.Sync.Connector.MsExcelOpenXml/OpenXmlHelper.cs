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
    using System.Text;
    using System.Text.RegularExpressions;
    using DocumentFormat.OpenXml.Spreadsheet;

    /// <summary>
    /// Helper class for OpenXml manipulation
    /// </summary>
    public static class OpenXmlHelper
    {
        /// <summary>
        /// The regular expression for integer extraction.
        /// </summary>
        private static readonly Regex RegexIntegers = new Regex(@"\d+", RegexOptions.Compiled);

        /// <summary>
        /// Reads the string value of a cell (including string table lookup).
        /// </summary>
        /// <param name="cell"> The cell to get the value from. </param>
        /// <param name="items"> The string lookup array for string reference cells. </param>
        /// <returns>The string value of the cell</returns>
        public static string GetCellValue(Cell cell, SharedStringItem[] items)
        {
            if (cell.DataType == null || cell.DataType.Value != CellValues.SharedString)
            {
                return cell.CellValue.Text;
            }

            return items[int.Parse(cell.CellValue.Text)].InnerText;
        }

        /// <summary>
        /// Extracts one group using a Regex and returns it as int
        /// </summary>
        /// <param name="stringToMatch"> The string to match. </param>
        /// <param name="regex"> The regex to extract information. </param>
        /// <returns> the first group of the match as int</returns>
        public static int GetRegExResultInt(this string stringToMatch, string regex)
        {
            return int.Parse(stringToMatch.GetRegExResult(regex));
        }

        /// <summary>
        /// Extracts one group using a Regex
        /// </summary>
        /// <param name="stringToMatch"> The string to match. </param>
        /// <param name="regex"> The regex to extract information. </param>
        /// <returns> the first group of the match </returns>
        public static string GetRegExResult(this string stringToMatch, string regex)
        {
            return new Regex(regex).Match(stringToMatch).Groups[1].ToString();
        }

        /// <summary>
        /// Convert an excel character-like index (column) to an integer. e.g. "A" = 1, "B" = 2 ...
        /// <para>This conversion will stop an any character that is not inthe range of A..Z</para>
        /// </summary>
        /// <param name="letters"> The letters to be translated. </param>
        /// <returns> the column index </returns>
        public static int LettersToIndex(this string letters)
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

        /// <summary>
        /// Calculates the column index character sequence for ExCel. 1=A, 2=B, ...
        /// <para>In case of high numbers (> 26) additional characters will be added (AB, AC, AD...).</para>
        /// </summary>
        /// <param name="index"> The index to be translated. </param>
        /// <returns> The column index as characters </returns>
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

        /// <summary>
        /// Given a cell name, parses the specified cell to get the row index.
        /// </summary>
        /// <param name="cellName">the alpha numeric cell reference (e.g. "AB:167" => 167)</param>
        /// <returns>the row index</returns>
        public static uint GetRowIndex(this string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            var match = RegexIntegers.Match(cellName);

            return uint.Parse(match.Value);
        }
    }
}