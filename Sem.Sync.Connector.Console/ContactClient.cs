﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
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

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(DisplayName = "Console output", CanWriteContacts = true, CanReadContacts = false)]
    public class ContactClient : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(StdContact));

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Console output Connector for individual contacts";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes a range of elements to the standard connector.
        /// </summary>
        /// <param name="elements">
        /// The elements. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements">
        /// The elements to be exported. 
        /// </param>
        /// <param name="clientFolderName">
        /// the full path that will get the contact files while exporting data.
        /// </param>
        /// <param name="skipIfExisting">
        /// this value is not used in this client.
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            foreach (var element in elements)
            {
                this.LogProcessingEvent(element, "writing ...");
                ContactListFormatter.Serialize(Console.Out, element);
            }
        }

        #endregion
    }
}