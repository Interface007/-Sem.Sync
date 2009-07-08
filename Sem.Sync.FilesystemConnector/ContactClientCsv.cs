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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            var fileName = clientFolderName;
            if (fileName.Contains("\n"))
            {
                fileName = fileName.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            using (var file = new StreamWriter(fileName, false, new UnicodeEncoding(false, true)))
            {
                var columnDefinitionFile = clientFolderName.Contains("\n")
                                               ? clientFolderName.Split(
                                                     new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                               : string.Empty;

                var columnDefinition = GetColumnDefinition(columnDefinitionFile);

                foreach (var column in columnDefinition)
                {
                    file.Write(column.Title + "\t");
                }
                file.WriteLine();

                foreach (StdContact element in elements)
                {
                    SyncTools.ClearNulls(element, typeof(StdContact));
                    if (element.Name != null)
                    {
                        var line = new StringBuilder();
                        foreach (var column in columnDefinition)
                        {
                            // skipping "empty" dates
                            if (GetPropertyValue(element, column.Selector) != "01.01.0001 00:00:00")
                            {
                                line.Append(
                                    GetPropertyValue(element, column.Selector)
                                        .ToString(CultureInfo.CurrentCulture)
                                        .Replace("\t", " "));

                            }
                            line.Append("\t");
                        }
                        file.WriteLine(line.ToString().Replace("\n", " ").Replace("\r", " "));
                    }
                }
            }
        }

        /// <summary>
        /// Reads a property by its path. E.g. you might specify the path "PersonalAddressPrimary.Phone.AreaCode"
        /// to access the AreaCode property of the Phone property inside the PersonalAddressPrimary property of 
        /// the entity
        /// </summary>
        /// <typeparam name="T">the type of the object to read from</typeparam>
        /// <param name="objectToReadFrom">the object to read from</param>
        /// <param name="pathToProperty">the path to the property</param>
        /// <returns>the value of the property rendered as a string</returns>
        private static string GetPropertyValue<T>(T objectToReadFrom, string pathToProperty)
        {
            if (!Equals(objectToReadFrom, default(T)))
            {
                if (pathToProperty.StartsWith("."))
                {
                    pathToProperty = pathToProperty.Substring(1);
                }

                var type = objectToReadFrom.GetType();
                var propName = pathToProperty.Contains('.') ? pathToProperty.Substring(0, pathToProperty.IndexOf('.')) : pathToProperty;

                if (propName == "ToString()")
                {
                    return type.GetMethod("ToString").Invoke(objectToReadFrom, null).ToString();
                }

                if (type.GetProperty(propName) != null)
                {
                    if (pathToProperty.Contains('.'))
                    {
                        return
                            GetPropertyValue(
                                type
                                    .GetProperty(pathToProperty.Substring(0, pathToProperty.IndexOf('.')))
                                    .GetValue(objectToReadFrom, null),
                                pathToProperty.Substring(pathToProperty.IndexOf('.') + 1));
                    }

                    return
                        ((type
                            .GetProperty(pathToProperty)
                            .GetValue(objectToReadFrom, null)) ?? "")
                            .ToString();
                }
            }

            return string.Empty;
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
                && Path.GetExtension(columnDefinitionFile) != ".{write}"
                && File.Exists(columnDefinitionFile))
            {
                result = result.LoadFrom(columnDefinitionFile, new[] { typeof(ColumnDefinition) });
                if (result.LongCount()>0)
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
        /// Gets a list of paths to public properties of the object and its sub objects.
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
                    case "Byte[]":
                        resultList.Add(parentName + item.Name);
                        break;

                    default:
                        resultList.AddRange(GetPropertyList(parentName + item.Name, item.PropertyType));
                        var tostring =
                            (from x in item.PropertyType.GetMethods()
                             where 
                             x.Name == "ToString" 
                             && x.GetParameters().Length == 0
                             && x.GetCustomAttributes(false).Contains(new AddAsPropertyAttribute())
                             select x).FirstOrDefault();
                        if (tostring != null)
                        {
                            resultList.Add(parentName + item.Name + ".ToString()");
                        }
                        break;
                }
            }

            return resultList;
        }
    }
}
