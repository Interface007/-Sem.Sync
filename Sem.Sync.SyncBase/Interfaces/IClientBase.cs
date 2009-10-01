//-----------------------------------------------------------------------
// <copyright file="IClientBase.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Interfaces
{
    using System;
    using System.Collections.Generic;

    using GenericHelpers.EventArgs;
    using GenericHelpers.Interfaces;

    /// <summary>
    /// Base interface for a synchronization client. This interface does provide access to
    /// generic synchronization functionality as well as support for event handling.
    /// </summary>
    public interface IClientBase : ICredentialAware
    {
        /// <summary>
        /// Handels processing events and informs subscribers about the internal
        /// events of the processing.
        /// </summary>
        event EventHandler<ProcessingEventArgs> ProcessingEvent;

        /// <summary>
        /// Handels logon requests. Some storage needs log on credentials, so the client does
        /// query the subscribers of this event to provide the needed information.
        /// </summary>
        event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogonCredentialsEvent;

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        string FriendlyClientName { get; }

        /// <summary>
        /// Method to remove duplicate entities - this is an optional implementation requirement. A client 
        /// can implment this simply with an empty method.
        /// </summary>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void RemoveDuplicates(string clientFolderName);

        /// <summary>
        /// Implementation of the process of retrieving the full list of elements.
        /// </summary>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <returns>The list with the newly added elements</returns>
        List<StdElement> GetAll(string clientFolderName);

        /// <summary>
        /// Implementation of the process of writing a single element. If the element is already in place, it will be overridden.
        /// </summary>
        /// <param name="element">the element to be added</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void AddItem(StdElement element, string clientFolderName);
        
        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements. 
        /// If the elements are already in place, they will be overridden.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void AddRange(List<StdElement> elements, string clientFolderName);

        /// <summary>
        /// Implementation of the process of writing a single element and skipping this process if this 
        /// element is already present. If the element does not exist, it will be added. If it does exist
        /// the element will not be added and not be overridden.
        /// </summary>
        /// <param name="element">the element to be added</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void MergeMissingItem(StdElement element, string clientFolderName);

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        /// skipping this process if an element is already present. Missing elements will be added, existing 
        /// elements will not be altered.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void MergeMissingRange(List<StdElement> elements, string clientFolderName);

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        /// overwriting the elements if they do already exist. Missing elements will be added, existing 
        /// elements will overwritten with the new elements.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void WriteRange(List<StdElement> elements, string clientFolderName);

        /// <summary>
        /// Deletes a list/collection of entities stecified by the identifiers.
        /// </summary>
        /// <param name="elementsToDelete">
        /// The elements to be to deleted. This depends on the internal implementation of the storage - mostly
        /// only the id read from <see cref="StdContact.PersonalProfileIdentifiers"/> is needed to delete an element.
        /// </param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName);
    }
}