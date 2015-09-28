// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonCategory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the different types of persons
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    /// <summary>
    /// Implements the different types of persons
    /// </summary>
    public enum PersonCategory
    {
        /// <summary>
        /// Normal contact (will not ring in case of ring-suppression active)
        /// </summary>
        [XmlEnum(Name = "0")]
        Default = 0,
        
        /// <summary>
        /// Sie sind auch bei aktivierter Klingelsperre für diese Person erreichbar.
        /// </summary>
        [XmlEnum(Name = "1")]
        Important
    }
}
