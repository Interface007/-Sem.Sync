// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneBook.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the PhoneBook type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType(TypeName = "phonebook")]
    public class PhoneBook : List<Contact>
    {
    }
}
