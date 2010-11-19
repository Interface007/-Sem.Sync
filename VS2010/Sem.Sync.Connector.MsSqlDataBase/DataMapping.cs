// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataMapping.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Mapping class - might be replaced with a generic one in a base library
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    /// <summary>
    /// Mapping class - might be replaced with a generic one in a base library
    /// </summary>
    public class DataMapping
    {
        /// <summary>
        /// Gets or sets DatabaseField.
        /// </summary>
        public string DatabaseField { get; set; }

        /// <summary>
        /// Gets or sets DatabaseTable.
        /// </summary>
        public string DatabaseTable { get; set; }

        /// <summary>
        /// Gets or sets PathToProperty.
        /// </summary>
        public string PathToProperty { get; set; }

        /// <summary>
        /// Gets or sets ReferenceToDatabaseTable.
        /// </summary>
        public string ReferenceToDatabaseTable { get; set; }
    }
}