// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Class that provides a memory only connector to speed up operations.
//   By adding a <see cref="ConnectorDescriptionAttribute" /> with CanRead = false and CanWrite = false
//   it's invisible to the client GUI. This attribute is not respected by the engine - only by the GUI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Memory
{
    using System.Collections.Generic;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// Class that provides a memory only connector to speed up operations.
    ///   By adding a <see cref="ConnectorDescriptionAttribute"/> with CanRead = false and CanWrite = false
    ///   it's invisible to the client GUI. This attribute is not respected by the engine - only by the GUI.
    /// </summary>
    [ConnectorDescription(DisplayName = "Generic-Memory-Client", CanReadContacts = true, CanWriteContacts = true, 
        Internal = true)]
    public class GenericClient : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   private storage in memory
        /// </summary>
        private static readonly Dictionary<string, List<StdElement>> Content =
            new Dictionary<string, List<StdElement>>();

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Memory Generic Connector";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Special method to reset the memory storage
        /// </summary>
        public static void Clear()
        {
            Content.Clear();
        }

        /// <summary>
        /// adds a range of elements
        /// </summary>
        /// <param name="elements">
        /// The elements.
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            if (!Content.ContainsKey(clientFolderName))
            {
                Content.Add(clientFolderName, elements);
                return;
            }

            var storage = Content[clientFolderName];
            elements.ForEach(x => storage.Remove(x));
            storage.AddRange(elements);
        }

        /// <summary>
        /// Deletes a content folder
        /// </summary>
        /// <param name="elementsToDelete">
        /// ignored - this will always delete the complete "folder" 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name to be deleted. 
        /// </param>
        public override void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
            if (Content.ContainsKey(clientFolderName))
            {
                Content.Remove(clientFolderName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads a list of <see cref="StdElement"/> from an internal dictionary.
        /// </summary>
        /// <param name="clientFolderName">
        /// The key of the dictionary that is assigned to the list
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list provided with the parameter <paramref name="result"/>
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (!Content.ContainsKey(clientFolderName))
            {
                Content.Add(clientFolderName, new List<StdElement>());
            }

            return Content[clientFolderName];
        }

        /// <summary>
        /// Writes the contact list into a dictionary of contact lists.
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// The key of the dictionary the list is written to.
        /// </param>
        /// <param name="skipIfExisting">
        /// This parameter is not used in this implementation.
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (Content.ContainsKey(clientFolderName))
            {
                Content.Remove(clientFolderName);
            }

            Content.Add(clientFolderName, elements);
        }

        #endregion
    }
}