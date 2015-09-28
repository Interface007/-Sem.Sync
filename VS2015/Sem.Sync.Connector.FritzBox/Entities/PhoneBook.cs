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

    using Sem.GenericHelpers;

    /// <summary>
    /// Implements a list of contact entries
    /// </summary>
    [XmlType(TypeName = "phonebook")]
    public class PhoneBook : List<Contact>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneBook"/> class.
        /// </summary>
        public PhoneBook()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneBook"/> class.
        /// </summary>
        /// <param name="contacts"> The instance of an IEnumerable to fill the PhoneBook instance. </param>
        public PhoneBook(IEnumerable<Contact> contacts)
        {
            contacts.ForEach(this.Add);
        }
    }
}
