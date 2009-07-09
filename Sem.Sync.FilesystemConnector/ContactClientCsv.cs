// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientCsv.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
// This client implementation does write to a CSV file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.FilesystemConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Helpers;

    #endregion usings

    ///<summary>
    /// This client implementation does write to a CSV file. 
    ///</summary>
    /// <remarks>
    /// The implementation does try to read the
    /// configuration for the columns and headers from a second file specified inside the TargetStorePath
    /// separated to the destination file by a \n character. The following example shows a possible way
    /// to specify a column definition file. By adding the file extension ".{write}" the file will not
    /// be read, but written with the standard values. After writing such a file, you can modify this file
    /// and remove the file extensionm ".{write}".
    /// <code>
    /// &lt;TargetStorePath>
    ///     {FS:WorkingFolder}\test.csv
    ///     {FS:WorkingFolder}\test.csv.config.{write}
    /// &lt;/TargetStorePath>
    /// </code>
    /// </remarks>
    public class ContactClientCsv : StdClient
    {
        /// <summary>
        /// Gets the user readable name of the client implementation called "Comma Seperated Values connector".
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Comma Seperated Values connector";
            }
        }

        /// <summary>
        /// This is a write only client, so this method will simply return the <paramref name="result"/>.
        /// </summary>
        /// <param name="clientFolderName">this parameter is not used</param>
        /// <param name="result">this list will be returned by the method</param>
        /// <returns>the list from parameter <paramref name="result"/></returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var filename = GetFileName(clientFolderName);
            if (File.Exists(filename))
            {
                result = new List<StdElement>();
                using (var file = new StreamReader(filename, new UnicodeEncoding(false, true)))
                {
                    var columnDefinition = GetColumnDefinition(GetColumnDefinitionFileName(clientFolderName));
                    if (!file.EndOfStream)
                    {
                        file.ReadLine();

                        while (!file.EndOfStream)
                        {
                            var lineColumns = file.ReadLine().Split(new[] { "\t" }, StringSplitOptions.None);
                            var element = new StdContact();
                            var columnIndex = 0;
                            foreach (var valueString in lineColumns)
                            {
                                if (columnIndex < columnDefinition.Count)
                                {
                                    if (!columnDefinition[columnIndex].Selector.EndsWith("()", StringComparison.Ordinal))
                                    {
                                        SyncTools.SetPropertyValue(
                                            element, columnDefinition[columnIndex].Selector, valueString);
                                    }
                                }
                                columnIndex++;
                            }

                            result.Add(element);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Write method for full list of elements. This will write the properties of the <see cref="StdElement"/>
        /// to the file specified by the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">The path and file name to where the elements should be written.</param>
        /// <param name="skipIfExisting">this parameter is ignored in this client implementation</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var columnDefinition = GetColumnDefinition(GetColumnDefinitionFileName(clientFolderName));

            using (var file = new StreamWriter(GetFileName(clientFolderName), false, new UnicodeEncoding(false, true)))
            {
                foreach (var column in columnDefinition)
                {
                    file.Write(column.Title + "\t");
                }

                file.WriteLine();

                foreach (StdContact element in elements)
                {
                    SyncTools.ClearNulls(element, typeof(StdContact));
                    if (element.Name == null)
                    {
                        continue;
                    }

                    var line = new StringBuilder();
                    foreach (var column in columnDefinition)
                    {
                        var valueString = SyncTools.GetPropertyValue(element, column.Selector);
                        // skipping "empty" dates
                        if (valueString != "01.01.0001 00:00:00")
                        {
                            line.Append(valueString.Replace("\t", " "));
                        }

                        line.Append("\t");
                    }

                    file.WriteLine(line.ToString().Replace("\n", " ").Replace("\r", " "));
                }
            }
        }

        private static string GetColumnDefinitionFileName(string clientFolderName)
        {
            return clientFolderName.Contains("\n")
                       ? clientFolderName.Split(
                             new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[1]
                       : string.Empty;
        }

        private static string GetFileName(string clientFolderName)
        {
            var fileName = clientFolderName;
            if (fileName.Contains("\n"))
            {
                fileName = fileName.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            return fileName;
        }

        /// <summary>
        /// Read the column definition from the column definition file specified with the
        /// poarameter <paramref name="columnDefinitionFile"/>. If there is no such file 
        /// specified, a list of such entries will be created by searching the object 
        /// recursively for properties.
        /// </summary>
        /// <param name="columnDefinitionFile">the file that does contain a list of <see cref="ColumnDefinition"/></param>
        /// <returns>a list of <see cref="ColumnDefinition"/> to describe the columns</returns>
        private static List<ColumnDefinition> GetColumnDefinition(string columnDefinitionFile)
        {
            var result = new List<ColumnDefinition>();

            if (
                !string.IsNullOrEmpty(columnDefinitionFile)
                && File.Exists(columnDefinitionFile.Replace(".{write}", "")))
            {
                result = result.LoadFrom(columnDefinitionFile.Replace(".{write}", ""), new[] { typeof(ColumnDefinition) });
                if (result.LongCount() > 0)
                {
                    return result;
                }
            }

            result = (from x in GetPropertyList("", typeof(StdContact)) select new ColumnDefinition(x)).ToList();

            if (!string.IsNullOrEmpty(columnDefinitionFile) && Path.GetExtension(columnDefinitionFile) == ".{write}")
            {
                result.SaveTo(
                    columnDefinitionFile.Remove(columnDefinitionFile.Length - 8), new[] { typeof(ColumnDefinition) });
            }

            return result;
        }

        /// <summary>
        /// Gets a list of paths to public properties of the object and its sub objects. It also includes
        /// paths to methods tagged with the <see cref="AddAsPropertyAttribute"/> attribute.
        /// </summary>
        /// <param name="parentName">the path that should be used as a root path for the string specification of the path</param>
        /// <param name="type">the type to be inspected</param>
        /// <returns>a list of strings with the paths of properties detected</returns>
        private static List<string> GetPropertyList(string parentName, Type type)
        {
            if (parentName.Length > 0)
            {
                parentName += ".";
            }

            var resultList = new List<string>();

            var methodsToAdd =
                from x in type.GetMethods()
                where
                x.GetParameters().Length == 0
                && x.GetCustomAttributes(false).Contains(new AddAsPropertyAttribute())
                select parentName + x.Name + "()";

            resultList.AddRange(methodsToAdd.ToList());

            var members = type.GetProperties();

            foreach (var item in members)
            {
                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType.FullName == "System.Enum")
                {
                    typeName = "Enum";
                }

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "Int32":
                        resultList.Add(parentName + item.Name);
                        break;

                    case "Byte[]":
                        // we don't want to save byte arrays to the CSV file (may be some time)
                        break;

                    default:
                        resultList.AddRange(GetPropertyList(parentName + item.Name, item.PropertyType));
                        break;
                }
            }

            return resultList;
        }
    }
}
