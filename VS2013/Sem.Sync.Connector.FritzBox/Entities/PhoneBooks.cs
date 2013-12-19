// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneBooks.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the PhoneBooks type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements a list of phone book entries
    /// </summary>
    [XmlType(TypeName = "phonebooks")]
    public class PhoneBooks : List<PhoneBook>
    {
    }
}
