// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericClientCsv.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This client implementation does write to a CSV file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Filesystem
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This client implementation does write to a CSV file.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements to be handled by this client
    /// </typeparam>
    /// <remarks>
    /// The implementation does try to read the
    ///   configuration for the columns and headers from a second file specified inside the TargetStorePath
    ///   separated to the destination file by a \n character. The following example shows a possible way
    ///   to specify a column definition file. By adding the file extension ".{write}" the file will not
    ///   be read, but written with the standard values. After writing such a file, you can modify this file
    ///   and remove the file extension ".{write}".
    ///   <code>
    /// &lt;TargetStorePath&gt;
    ///     {FS:WorkingFolder}\test.csv
    ///     {FS:WorkingFolder}\test.csv.config.{write}
    ///     &lt;/TargetStorePath&gt;
    ///   </code>
    /// The class is a generic class, so you cen export StdContacts as well as StdCalendarItems
    /// </remarks>
    [ClientStoragePathDescription(Mandatory = true, Default = "{FS:WorkingFolder}\\Elements.csv", 
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    [ConnectorDescription(DisplayName = "Filesystem CSV", IsGeneric = true, CanReadCalendarEntries = true, 
        CanWriteCalendarEntries = true, CanReadContacts = true, CanWriteContacts = true)]
    public class GenericClientCsv<T> : StdClient
        where T : StdElement, new()
    {
        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation called "Comma Seperated Values connector".
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Comma Seperated Values connector";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is a write only client, so this method will simply return the <paramref name="result"/>.
        /// </summary>
        /// <param name="clientFolderName">
        /// this parameter is not used
        /// </param>
        /// <param name="result">
        /// this list will be returned by the method
        /// </param>
        /// <returns>
        /// the list from parameter <paramref name="result"/>
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var filename = GetFileName(clientFolderName);
            if (File.Exists(filename))
            {
                result = new List<StdElement>();
                using (var file = new StreamReader(filename, new UnicodeEncoding(false, true)))
                {
                    var columnDefinition = this.GetColumnDefinition(
                        GetColumnDefinitionFileName(clientFolderName), typeof(T));
                    if (!file.EndOfStream)
                    {
                        // skip the headers
                        file.ReadLine();

                        while (!file.EndOfStream)
                        {
                            // read one line and slpit to columns
                            var lineColumns = file.ReadLine().Split(new[] { "\t" }, StringSplitOptions.None);

                            // create a new element
                            var element = new T();

                            // to get the column definition for each element read from the line, we need a counter
                            var columnIndex = 0;
                            foreach (var valueString in lineColumns)
                            {
                                if (columnIndex < columnDefinition.Count)
                                {
                                    // skip all method definitions, because we cannot set the value of a method
                                    if (!columnDefinition[columnIndex].Selector.EndsWith("()", StringComparison.Ordinal))
                                    {
                                        Tools.SetPropertyValue(
                                            element, columnDefinition[columnIndex].Selector, valueString);
                                    }
                                }

                                columnIndex++;
                            }

                            this.LogProcessingEvent(element, "add element");
                            result.Add(element);
                        }
                    }
                }
            }

            this.LogProcessingEvent("read completed");

            return result;
        }

        /// <summary>
        /// Write method for full list of elements. This will write the properties of the <see cref="StdElement"/>
        ///   to the file specified by the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// The path and file name to where the elements should be written.
        /// </param>
        /// <param name="skipIfExisting">
        /// this parameter is ignored in this client implementation
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var columnDefinition = this.GetColumnDefinition(GetColumnDefinitionFileName(clientFolderName), typeof(T));

            using (var file = new StreamWriter(GetFileName(clientFolderName), false, new UnicodeEncoding(false, true)))
            {
                foreach (var column in columnDefinition)
                {
                    file.Write(column.Title + "\t");
                }

                file.WriteLine();

                foreach (T element in elements)
                {
                    this.LogProcessingEvent(element, "writing element ...");
                    SyncTools.ClearNulls(element, typeof(T));
                    if (element == null)
                    {
                        this.LogProcessingEvent("NULL-element skipped");
                        continue;
                    }

                    var line = new StringBuilder();
                    foreach (var column in columnDefinition)
                    {
                        var valueString = Tools.GetPropertyValueString(element, column.Selector);

                        // skipping "empty" dates
                        if (valueString != "01.01.0001 00:00:00")
                        {
                            line.Append(valueString.Replace("\t", " "));
                        }

                        line.Append("\t");
                    }

                    file.WriteLine(line.ToString().Replace("\n", " ").Replace("\r", " "));
                }

                this.LogProcessingEvent("write completed");
            }
        }

        #endregion
    }
}