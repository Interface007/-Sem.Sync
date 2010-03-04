// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MicrosoftExcel2010
{
    using System.IO;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Writes a list of "something" into an excel file
    /// </summary>
    [ConnectorDescription(DisplayName = "Generic-Excel-Client",
    CanReadContacts = false,
    CanWriteContacts = true,
    Internal = false)]
    [ClientStoragePathDescription(Mandatory = true, ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    public class GenericClient : StdClient
    {
        public override void AddRange(System.Collections.Generic.List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        protected override void WriteFullList(System.Collections.Generic.List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (File.Exists(clientFolderName))
            {
                File.Delete(clientFolderName);
            }

            File.WriteAllText(clientFolderName, ExcelWriter.ExportToWorksheet(elements.ToContacts()));
        }
    }
}
