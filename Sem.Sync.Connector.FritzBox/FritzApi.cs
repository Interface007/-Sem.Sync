// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FritzApi.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the FritzApi type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox
{
    using System;
    using System.Collections;

    using Sem.Sync.Connector.FritzBox.Entities;

    /// <summary>
    /// Wrapper for the <see cref="FritzBoxNET"/> classes.
    /// </summary>
    public class FritzApi
    {
        /// <summary>
        /// Control connection for FritzBox - this will query the connection parameters for the TCP client
        /// </summary>
        private readonly FritzBoxNET.UPnP.Phonebook phonebookControl = new FritzBoxNET.UPnP.Phonebook();

        /// <summary>
        /// Data connection for FritzBox - this will get the data from the box
        /// </summary>
        private readonly FritzBoxNET.Network.Phonebook phonebookAccess = new FritzBoxNET.Network.Phonebook();

        /// <summary>
        /// Gets or sets the host setting for the fritz box.
        /// </summary>
        public Uri Host { get; set; }

        /// <summary>
        /// Gets or sets the fritz box password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets the phone book entries of phonebook 0
        /// </summary>
        /// <returns> a list of deserialized entries </returns>
        public PhoneBook GetPhoneBook()
        {
            var result = new PhoneBook();
            
            this.phonebookControl.host = this.Host.Host;
            this.phonebookControl.HTTPpassword = this.UserPassword;
            var phoneBookResult = this.phonebookControl.OpenPort() as Hashtable;

            this.phonebookAccess.host = this.Host.Host;
            if (this.phonebookAccess.port == "0")
            {
                if (phoneBookResult != null)
                {
                    this.phonebookAccess.port = (string)phoneBookResult["Port"];
                }
            }

            var phonebookEntryCount = (int)this.phonebookAccess.GetEntryCount();

            for (var i = 0; i < phonebookEntryCount; i++)
            {
                var x = this.phonebookAccess.GetEntry(i) as System.Xml.XmlNodeList;
                if (x == null || x.Count < 1)
                {
                    continue;
                }

                var contact = GenericHelpers.Tools.LoadFromString<Contact>(x[0].OuterXml);
                result.Add(contact);
            }

            return result;
        }
    }
}