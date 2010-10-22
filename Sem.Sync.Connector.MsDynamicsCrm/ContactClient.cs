// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsDynamicsCrm
{
    using System.Collections.Generic;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    [ConnectorDescription(DisplayName = "Microsoft Dynamics CRM 4.0", 
        CanReadContacts = true, CanWriteContacts = true, 
        MatchingIdentifier = ProfileIdentifierType.MicrosoftDynamicsCrm)]
    public class ContactClient : StdClient
    {
        public override System.Collections.Generic.List<StdElement> GetAll(string clientFolderName)
        {
            return new List<StdElement>();
        }
    }
}
