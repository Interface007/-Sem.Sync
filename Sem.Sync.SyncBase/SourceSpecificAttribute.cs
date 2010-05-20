// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceSpecificAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements a data type for Information that is specific to one single connector / source.
//   E.g. in some CRM systems there are user defined properties that cannot be mapped.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    /// <summary>
    /// Implements a data type for Information that is specific to one single connector / source.
    ///   E.g. in some CRM systems there are user defined properties that cannot be mapped.
    /// </summary>
    public class SourceSpecificAttribute
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the name of the attribute.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        ///   Gets or sets the value of the attribute.
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        ///   Gets or sets the full qualified class name of the SourceConnector.
        /// </summary>
        public string SourceConnector { get; set; }

        #endregion
    }
}