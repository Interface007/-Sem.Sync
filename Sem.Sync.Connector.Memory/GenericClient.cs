// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling elements
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Memory
{
    using System.Collections.Generic;

    using SyncBase;
    using SyncBase.Attributes;

    /// <summary>
    /// Class that provides a memory only connector to speed up operations.
    /// By adding a <see cref="ConnectorDescriptionAttribute"/> with CanRead = false and CanWrite = false
    /// it's invisible to the client GUI. This attribute is not respected by the engine - only by the GUI.
    /// </summary>
    [ConnectorDescription(DisplayName = "Generic-Memory-Client",
        CanReadContacts = true,
        CanWriteContacts = true,
        Internal = true)]
    public class GenericClient : StdClient
    {
        /// <summary>
        /// private storage in memory
        /// </summary>
        private static readonly Dictionary<string, List<StdElement>> Content =
            new Dictionary<string, List<StdElement>>();

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Memory Generic Connector";
            }
        }

        /// <summary>
        /// Special method to reset the memory storage
        /// </summary>
        public static void Clear()
        {
            Content.Clear();
        }

        /// <summary>
        /// Deletes a content folder
        /// </summary>
        /// <param name="elementsToDelete">ignored - this will always delete the complete "folder" </param>
        /// <param name="clientFolderName"> The client folder name to be deleted. </param>
        public override void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
            if (Content.ContainsKey(clientFolderName))
            {
                Content.Remove(clientFolderName);
            }
        }

        /// <summary>
        /// adds a range of elements
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <param name="clientFolderName">The client folder name.</param>
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
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (!Content.ContainsKey(clientFolderName))
            {
                Content.Add(clientFolderName, new List<StdElement>());
            }

            return Content[clientFolderName];
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (Content.ContainsKey(clientFolderName))
            {
                Content.Remove(clientFolderName);
            }

            Content.Add(clientFolderName, elements);
        }
    }
}