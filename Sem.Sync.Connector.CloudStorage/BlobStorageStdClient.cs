// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobStorageStdClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   implements a <see cref="StdClient" /> accessing the Azure blob storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Sem.Sync.Connector.CloudStorage
{
    using System.Collections.Generic;

    using Helper;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Helpers;

    /// <summary>
    /// implements a <see cref="StdClient"/> accessing the Azure blob storage.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = false)]
    [ConnectorDescription(CanReadContacts = false, CanWriteContacts = false, DisplayName = "Azure Cloud Blob Storage")]
    public class BlobStorageStdClient : StdClient
    {
        /// <summary>
        /// Blob container name
        /// </summary>
        private const string ContainerName = "contactcontainer";

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Azure Cloud Blob Storage";
            }
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var accountInfo = CloudStorageAccount.FromConfigurationSetting("SemSync");
            
            var client = accountInfo.CreateCloudBlobClient();
            var container = client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
            container.CreateIfNotExist();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            var blob = container.GetBlockBlobReference(clientFolderName);
            
            result = new List<StdElement>();
            var blobText = blob.DownloadText();
            result.LoadFromString(blobText);
            return result;
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var accountInfo = CloudStorageAccount.FromConfigurationSetting("SemSync");
            var client = accountInfo.CreateCloudBlobClient();
            var blob = client.GetBlockBlob(clientFolderName);

            var blobText = elements.SaveToString();

            blob.UploadText(blobText);
        }
    }
}