// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MicrosoftExcel2010
{
    using System.IO;
    using System.Text;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Writes a list of "something" into an excel XML file - does explicitly NOT support XSLX-Files!
    /// </summary>
    [ConnectorDescription(
        DisplayName = "Excel-XML-Client",
        CanReadContacts = false,
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
        public override void AddRange(System.Collections.Generic.List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Writes the elements to the destination.
        /// </summary>
        /// <param name="elements"> The elements. </param>
        /// <param name="clientFolderName"> The name of the file to write to. </param>
        /// <param name="skipIfExisting"> The flag whether to skip the item if it exist - in this case it's simply ignored, because the target will be overwritten. </param>
        protected override void WriteFullList(System.Collections.Generic.List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (File.Exists(clientFolderName))
            {
                File.Delete(clientFolderName);
            }

            File.WriteAllText(clientFolderName, ExcelWriter.ExportToWorksheet(elements.ToContacts()), Encoding.UTF8);
        }
    }
}
