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
    using SyncBase;
    using SyncBase.Attributes;

    /// <summary>
    /// 
    /// </summary>
    [ConnectorDescription(DisplayName = "Generic-Memory-Client",
    CanReadContacts = true,
    CanWriteContacts = true,
    Internal = true)]
    public class GenericClient : StdClient
    {

    }
}
