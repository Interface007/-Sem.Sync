// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncWorkFlow.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines a concrete workflow based on a template.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.Entities
{
    using SyncBase.DetailData;

    /// <summary>
    /// Defines a concrete workflow based on a template.
    /// </summary>
    public class SyncWorkFlow
    {
        /// <summary>
        /// Gets or sets the human readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the connector information for the source.
        /// </summary>
        public ConnectorInformation Source { get; set; }

        /// <summary>
        /// Gets or sets the connector information for the source.
        /// </summary>
        public ConnectorInformation Target { get; set; }

        /// <summary>
        /// Gets or sets the Template to use.
        /// </summary>
        public string Template { get; set; }
    }
}
