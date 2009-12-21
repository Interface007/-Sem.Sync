// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceSpecificAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class describes attributes that are specific to a given connector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    /// <summary>
    /// This class describes attributes that are specific to a given connector.
    /// </summary>
    public class SourceSpecificAttribute
    {
        /// <summary>
        /// Gets or sets the namespace of the connector this attribute belongs to.
        /// </summary>
        public string SourceConnector { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the value of the attribute.
        /// </summary>
        public string AttributeValue { get; set; }
    }
}