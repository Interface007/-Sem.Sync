// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox
{
    using System.Collections.Generic;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// This class is the client class for handling elements. This class leaks some of the contact 
    ///   related features of "ContactClient", but provides the ability to handle other types that do 
    ///   inherit from StdElement
    /// </summary>
    [ClientStoragePathDescription(
        Mandatory = true,
        Default = @"http://fritz.box/",
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Fritz!Box AddressBook Client",
        NeedsCredentials = true,
        NeedsCredentialsDomain = true)]
    public class ContactClient : StdClient
    {
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var http = new HttpHelper(clientFolderName, true);

            return result;
        }
    }
}
