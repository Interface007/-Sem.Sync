// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobStorageStdClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   implements a <see cref="StdClient" /> accessing the Azure blob storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.CloudStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.StorageClient;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// implements a <see cref="StdClient"/> accessing the Azure blob storage.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = false)]
    [ConnectorDescription(CanReadContacts = false, CanWriteContacts = false, DisplayName = "Azure Cloud Blob Storage")]
    public class BlobStorageStdClient : StdClient
    {
        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Azure Cloud Blob Storage";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var accountInfo = CloudStorageAccount.FromConfigurationSetting("semsync");

            var client = accountInfo.CreateCloudBlobClient();
            var container = client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
            container.CreateIfNotExist();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            var blob = container.GetBlockBlobReference(clientFolderName);
            result = new List<StdElement>();

            try
            {
                var blobText = blob.DownloadText();
                var formatter = new XmlSerializer(typeof(List<StdElement>), new[] { typeof(List<StdContact>) });
                var reader = new StringReader(blobText);
                result = (List<StdElement>)formatter.Deserialize(reader);

                return result;
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.BlobNotFound)
                {
                    return result;
                }

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// the information to where inside the source the elements should be written - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting">
        /// specifies whether existing elements should be updated or simply left as they are
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var accountInfo = CloudStorageAccount.FromConfigurationSetting("semsync");

            var client = accountInfo.CreateCloudBlobClient();
            var container = client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
            container.CreateIfNotExist();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            var blob = container.GetBlockBlobReference(clientFolderName);

            var blobText = elements.SaveToString(new[] { typeof(StdContact) });

            blob.UploadText(blobText);
        }

        #endregion
    }
}