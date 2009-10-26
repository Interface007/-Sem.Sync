// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Console
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    [ClientStoragePathDescriptionAttribute(Irrelevant = true)]
    [ConnectorDescription(
        DisplayName = "Console output",
        CanWriteContacts = true,
        CanReadContacts = false)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(StdContact));

        /// <summary>
        /// Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Console output Connector for individual contacts";
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name of the path that does contain the contact files.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            throw new NotImplementedException("Reading from the command line id not implemented.");
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full path that will get the contact files while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            Tools.EnsurePathExist(clientFolderName);
            foreach (var element in elements)
            {
                ContactListFormatter.Serialize(Console.Out, element);
            }
        }
    }
}
