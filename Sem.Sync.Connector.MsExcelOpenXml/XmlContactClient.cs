// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Writes a list of "something" into an excel OpenXml file. The <see cref="StdElement.Id" />
//   MUST be specified, because there is no such thing as a unique row identifier
//   in Microsoft ExCel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelOpenXml
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// Writes a list of "something" into an excel OpenXml file. The <see cref="StdElement.Id"/> 
    ///   MUST be specified, because there is no such thing as a unique row identifier 
    ///   in Microsoft ExCel.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [ConnectorDescription(DisplayName = "Microsoft Excel OpenXml", CanReadContacts = true, CanWriteContacts = true, 
        CanReadCalendarEntries = true, CanWriteCalendarEntries = true, Internal = false)]
    [ClientStoragePathDescription(Mandatory = true, ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    public class XmlContactClient<T> : StdClient
        where T : StdElement, new()
    {
        #region Public Methods

        /// <summary>
        /// Exporting / writing will simply overwrite the destination, so we should override this method in order 
        ///   to not read from the target before writing to it.
        /// </summary>
        /// <param name="elements">
        /// The elements. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Reads all elements from the excel file
        /// </summary>
        /// <param name="clientFolderName">
        /// The path to the excel file. 
        /// </param>
        /// <returns>
        /// the list of contacts 
        /// </returns>
        public override List<StdElement> GetAll(string clientFolderName)
        {
            var result = new List<StdElement>();

            var mappingFileName = GetColumnDefinitionFileName(clientFolderName);
            var mapping = this.GetColumnDefinition(mappingFileName, typeof(T));
            var excelFile = GetFileName(clientFolderName);

            this.LogProcessingEvent("Opening Excel file {0} ...", excelFile);

            // get the array of values from the first sheet inside the array
            var valueArray = OpenXmlHelper.GetValueArrayFromExcelFile(excelFile);

            if (valueArray.Length <= 0)
            {
                this.LogProcessingEvent("no cells inside file - read aborted");

                // exit if we don't have cells
                return result;
            }

            this.LogProcessingEvent(
                "{0} values found in {1} rows and {2} columns.", 
                valueArray.Length, 
                valueArray.GetLength(0), 
                valueArray.GetLength(1));

            var exceptionCounter = 0;
            var exceptionReport = excelFile + ".ExceptionReport.xml";
            XmlWriter exceptionFile = null;

            // copy the values into objects
            for (var rowId = 1; rowId < valueArray.GetLength(0); rowId++)
            {
                var colIndex = 0;
                var newElement = new T();
                for (var colId = 0; colId < mapping.Count - 1; colId++)
                {
                    try
                    {
                        Tools.SetPropertyValue(newElement, mapping[colIndex].Selector, valueArray[rowId, colId]);
                    }
                    catch (Exception ex)
                    {
                        exceptionFile = WriteExceptionFile(
                            valueArray, rowId, colId, newElement, exceptionFile, exceptionReport, ex);
                        exceptionCounter++;
                    }

                    colIndex++;
                }

                result.Add(newElement);
                this.LogProcessingEvent(newElement, "contact read");
            }

            if (exceptionCounter > 0)
            {
                if (exceptionFile != null)
                {
                    exceptionFile.WriteEndElement();
                    exceptionFile.WriteEndDocument();
                    exceptionFile.Close();
                }

                this.LogProcessingEvent(
                    "{0} exceptions while reading data ... see exception report ({1}) for details.", 
                    exceptionCounter, 
                    exceptionReport);
            }

            this.LogProcessingEvent("cleaning up entities");
            CleanUpEntities(result);

            this.LogProcessingEvent("reading finished");
            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the elements to the destination.
        /// </summary>
        /// <param name="elements">
        /// The elements. 
        /// </param>
        /// <param name="clientFolderName">
        /// The name of the file to write to. 
        /// </param>
        /// <param name="skipIfExisting">
        /// The flag whether to skip the item if it exist - in this case it's simply ignored, because the target will be overwritten. 
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (File.Exists(clientFolderName))
            {
                File.Delete(clientFolderName);
            }

            var mappingFileName = GetColumnDefinitionFileName(clientFolderName);
            var mapping = this.GetColumnDefinition(mappingFileName, typeof(T));

            this.LogProcessingEvent(
                "preparing data for {0} contacts and {1} mappings...", elements.Count, mapping.Count);
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

            OpenXmlHelper.WriteStringArrayToExcelfile(GetFileName(clientFolderName), matrix);

            this.LogProcessingEvent("writing finished");
        }

        /// <summary>
        /// Writes the exception together with some meta information to an <see cref="XmlWriter"/>. If that <see cref="XmlWriter"/> is
        ///   null, a new instance of an <see cref="XmlWriter"/> is being created for writing into the file specified as the parameter
        ///   <paramref name="exceptionReportFileName"/>.
        /// </summary>
        /// <remarks>
        /// While creating the exception file a start tag is being written for the root element. The calling method is 
        ///   responsible to write the corresponding closing tag in order to finish the file.
        /// </remarks>
        /// <param name="valueArray">
        /// The value array working with while the exception has been thrown. 
        /// </param>
        /// <param name="rowId">
        /// The row id of the data row that caused the exception. 
        /// </param>
        /// <param name="colId">
        /// The col id of the data column that caused the exception.  
        /// </param>
        /// <param name="newElement">
        /// The element that is being processed while the exception did occure. 
        /// </param>
        /// <param name="exceptionFile">
        /// The <see cref="XmlWriter"/> for the exception file - if this element is NULL, a new file stream will be created. 
        /// </param>
        /// <param name="exceptionReportFileName">
        /// The exception report file name - in case of a null reference in the parameter <paramref name="exceptionFile"/> this file name will be used to create a new file stream for the XmlWriter. 
        /// </param>
        /// <param name="ex">
        /// The exception to be written to the file. 
        /// </param>
        /// <returns>
        /// The instance of the <see cref="XmlWriter"/> that has been utilized to write the exception. 
        /// </returns>
        private static XmlWriter WriteExceptionFile(
            string[,] valueArray, 
            int rowId, 
            int colId, 
            StdElement newElement, 
            XmlWriter exceptionFile, 
            string exceptionReportFileName, 
            Exception ex)
        {
            if (exceptionFile == null)
            {
                exceptionFile = XmlWriter.Create(File.AppendText(exceptionReportFileName));

                if (exceptionFile != null)
                {
                    exceptionFile.WriteStartDocument(true);
                    exceptionFile.WriteStartElement("ExceptionReport");
                }
            }

            if (exceptionFile != null)
            {
                exceptionFile.WriteStartElement("Exception");
                exceptionFile.WriteAttributeString("PersonName", newElement.ToString());
                exceptionFile.WriteAttributeString("Row", rowId.ToString(CultureInfo.InvariantCulture));
                exceptionFile.WriteAttributeString("Column", colId.ToString(CultureInfo.InvariantCulture));
                exceptionFile.WriteAttributeString("ExcelReference", colId.IndexToLetters() + rowId);
                exceptionFile.WriteElementString("CellValue", valueArray[rowId, colId]);
                exceptionFile.WriteElementString("Exception", ex.ToString());
                exceptionFile.WriteEndElement();
            }

            return exceptionFile;
        }

        #endregion
    }
}