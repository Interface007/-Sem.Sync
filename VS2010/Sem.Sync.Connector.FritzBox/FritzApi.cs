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

    /// <summary>
    /// </summary>
    public class FritzApi
    {
        private string controlUrl = "/upnp/control/phonebook";

        private string fileName = "/phonebook-scpd.xml";

        public Uri Host { get; set; }

        public string UserName { get; set; }
        
        public string UserPassword { get; set; }

        private string schema = "urn:schemas-any-com:service:phonebook:1";

        public string GetPhoneBook()
        {
            ////string xmlCode = this.SOAPAction("OpenPort")
            return string.Empty;
        }
    }
}