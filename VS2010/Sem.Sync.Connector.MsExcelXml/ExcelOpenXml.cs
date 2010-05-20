// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelOpenXml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The excel open xml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Xml;
    using System.Xml.Linq;

    using Sem.GenericHelpers;

    /// <summary>
    /// The excel open xml.
    /// </summary>
    public static class ExcelOpenXml
    {
        #region Constants and Fields

        /// <summary>
        /// The document relationship type.
        /// </summary>
        private const string documentRelationshipType =
            "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";

        /// <summary>
        /// The main name space.
        /// </summary>
        private static readonly XNamespace mainNameSpace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

        /// <summary>
        /// The relation ship.
        /// </summary>
        private static readonly XNamespace relationShip =
            "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is under development!
        /// </summary>
        /// <param name="packageFileName">
        /// The package file name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> ImportFromFromOpenXmlPackageFile<T>(string packageFileName) where T : class, new()
        {
            var list = new List<T>();

            using (var excelPackage = Package.Open(packageFileName, FileMode.Open, FileAccess.Read))
            {
                PackagePart documentPart = null;

                Uri documentUri = null;

                // Get the main document part (workbook.xml).
                foreach (var relationship in excelPackage.GetRelationshipsByType(documentRelationshipType))
                {
                    // There should only be one document part in the package. 
                    documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri);
                    documentPart = excelPackage.GetPart(documentUri);

                    // There should only be one instance, but get out no matter what.
                    break;
                }

                if (documentPart != null)
                {
                    // Load the contents of the workbook.
                    using (var reader = XmlReader.Create(documentPart.GetStream()))
                    {
                        var workBook = XDocument.Load(reader);

                        // initialize with an empty list in order to get no NULL reference exception
                        // if the data cannot be found
                        IEnumerable<XElement> sheets = new List<XElement>();

                        // suppress nulls by creating new elements if needed - 
                        // we simply need the Rows. If there are no rown, we get NULL, 
                        // what does exactly represent what we want.
                        // ReSharper disable PossibleNullReferenceException
                        workBook.MapIfExist2(
                            x => x.Root.Element(mainNameSpace + "sheets").Elements(mainNameSpace + "sheet"), ref sheets);

                        foreach (var sheet in sheets)
                        {
                            var rel = sheet.Attribute(relationShip + "id");
                            if (rel == null)
                            {
                                continue;
                            }

                            var relId = rel.Value;

                            // get the relation between the document and the sheet.
                            var sheetRelation = documentPart.GetRelationship(relId);
                            var sheetUri = System.IO.Packaging.PackUriHelper.ResolvePartUri(
                                documentUri, sheetRelation.TargetUri);
                            var sheetPart = excelPackage.GetPart(sheetUri);

                            using (var sheetReader = XmlReader.Create(sheetPart.GetStream()))
                            {
                                var sheetXml = XDocument.Load(sheetReader);

                                IEnumerable<XElement> data = new List<XElement>();

                                // suppress nulls by creating new elements if needed - 
                                // we simply need the Rows. If there are no rown, we get NULL, 
                                // what does exactly represent what we want.
                                // ReSharper disable PossibleNullReferenceException
                                sheetXml.MapIfExist2(
                                    x => x.Root.Element(mainNameSpace + "sheetData").Elements(mainNameSpace + "row"), 
                                    ref data);

                                XmlHelper.DeserializeList(data, list, mainNameSpace + "c");
                            }

                            break;
                        }
                    }
                }
            }

            return list;
        }

        #endregion
    }
}