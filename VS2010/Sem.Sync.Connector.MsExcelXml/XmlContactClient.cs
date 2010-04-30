// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsExcelXml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// Writes a list of "something" into an excel XML file - does explicitly NOT support XSLX-Files!
    /// </summary>
    [ConnectorDescription(
        DisplayName = "Microsoft Excel Xml Spreadsheet 2003",
        CanReadContacts = true,
        CanWriteContacts = true,
        Internal = false)]
    [ClientStoragePathDescription(
        Mandatory = true, 
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    public class XmlContactClient : StdClient
    {
        /// <summary>
        /// Exporting / writing will simply overwrite the destination, so we should override this method in order 
        /// to not read from the target before writing to it.
        /// </summary>
        /// <param name="elements"> The elements. </param>
        /// <param name="clientFolderName"> The client folder name. </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Reads all elements from the excel file
        /// </summary>
        /// <param name="clientFolderName"> The path to the excel file. </param>
        /// <returns> the list of contacts </returns>
        public override List<StdElement> GetAll(string clientFolderName)
        {
            return ExcelXml.ImportFromWorksheetXml<StdContact>(File.ReadAllText(clientFolderName)).ToStdElements();
        }

        /// <summary>
        /// Writes the elements to the destination.
        /// </summary>
        /// <param name="elements"> The elements. </param>
        /// <param name="clientFolderName"> The name of the file to write to. </param>
        /// <param name="skipIfExisting"> The flag whether to skip the item if it exist - in this case it's simply ignored, because the target will be overwritten. </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (File.Exists(clientFolderName))
            {
                File.Delete(clientFolderName);
            }

            File.WriteAllText(clientFolderName, ExcelXml.ExportToWorksheetXml(elements.ToStdContacts()), Encoding.UTF8);
        }
    }
}
