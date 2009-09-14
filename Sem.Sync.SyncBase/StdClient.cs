//-----------------------------------------------------------------------
// <copyright file="StdClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;

    using GenericHelpers.EventArgs;
    using GenericHelpers.Interfaces;

    using Helpers;

    using Interfaces;
    using Properties;
    
    /// <summary>
    /// This class can (should) be used as a base class for "client" classes. It already implements some of the aspects 
    /// of such a client class, so you only need to implement the specific methods.
    /// </summary>
    public abstract class StdClient : SyncComponent, IClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdClient"/> class. This will also read the saved credentials 
        /// for this Client type from the app.config file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "The virtual method that is called is a property that always should just return a string value and does not need any class initialization.")]
        protected StdClient()
        {
            this.LogOnUserId = this.GetConfigValue("LogonUserId");
            this.LogOnPassword = this.GetConfigValue("LogonPassword");
        }

        /// <summary>
        /// Informs the subscriber of this event that we need some credentials.
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogonCredentialsEvent;

        /// <summary>
        /// Gets or sets the domain part of the user credentials to access the target system
        /// </summary>
        public IUiInteraction UiDispatcher { get; set; }

        /// <summary>
        /// Gets or sets the domain part of the user credentials to access the target system
        /// </summary>
        public string LogOnDomain { get; set; }

        /// <summary>
        /// Gets or sets the user id part of the user credentials to access the target system
        /// </summary>
        public string LogOnUserId { get; set; }

        /// <summary>
        /// Gets or sets the password part of the user credentials to access the target system
        /// </summary>
        public string LogOnPassword { get; set; }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public abstract string FriendlyClientName
        {
            get;
        }
        
        /// <summary>
        /// virtual method that should be (optionally) implemented by the client to remove duplicate entities
        /// </summary>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void RemoveDuplicates(string clientFolderName)
        {
        }

        /// <summary>
        /// Deletes a list/collection of entities stecified by the identifiers.
        /// </summary>
        /// <param name="elementsToDelete">
        /// The elements to be to deleted. This depends on the internal implementation of the storage - mostly
        /// only the id read from <see cref="StdContact.PersonalProfileIdentifiers"/> is needed to delete an element.
        /// </param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void DeleteElements(IEnumerable<StdElement> elementsToDelete, string clientFolderName)
        {
        }

        /// <summary>
        /// Overridable implementation of the process of retrieving the full list of elements. In the
        /// default implementation this calls <see cref="BeforeStorageAccess"/> and 
        /// <see cref="ReadFullList"/> to get the elements and performs sorting and logging calls. 
        /// Override this method if you need additional control over the read process.
        /// </summary>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <returns>The list with the newly added elements</returns>
        public virtual List<StdElement> GetAll(string clientFolderName)
        {
            var result = new List<StdElement>();

            LogProcessingEvent(Resources.uiReadingElements);
            this.BeforeStorageAccess(clientFolderName);
            result = this.ReadFullList(clientFolderName, result);

            this.LogProcessingEvent(Resources.uiSorting);
            result.Sort();
            this.LogProcessingEvent(Resources.uiSorted);

            return result;
        }

        /// <summary>
        /// Overridable implementation of the process of writing a single element. In the
        /// default implementation this calls <see cref="GetAll"/>, WriteElement and 
        /// <see cref="WriteRange"/> to write the element and performs logging calls. 
        /// </summary>
        /// <param name="element">the element to be added</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void AddItem(StdElement element, string clientFolderName)
        {
            this.LogProcessingEvent(element, Resources.uiAddingElement);
            var data = this.GetAll(clientFolderName);
            WriteElement(data, element);
            this.WriteRange(data, clientFolderName);
            this.LogProcessingEvent(element, Resources.uiElementAdded);
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements. 
        /// If the elements are already in place, they will be overridden.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void AddRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements);
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, elements.Count));
        }

        /// <summary>
        /// Implementation of the process of writing a single element and skipping this process if this 
        /// element is already present. If the element does not exist, it will be added. If it does exist
        /// the element will not be added and not be overridden.
        /// </summary>
        /// <param name="element">the element to be added</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void MergeMissingItem(StdElement element, string clientFolderName)
        {
            LogProcessingEvent(element, Resources.uiAddingMissingElement);
            var data = this.GetAll(clientFolderName);
            var added = WriteElement(data, element, true);
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(element, added ? Resources.uiElementAdded : Resources.uiElementSkipped);
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        /// skipping this process if an element is already present. Missing elements will be added, existing 
        /// elements will not be altered.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements, true);
            data.Sort();
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, elements.Count));
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        /// overwriting the elements if they do already exist. Missing elements will be added, existing 
        /// elements will overwritten with the new elements.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public virtual void WriteRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(Resources.uiWritingElements);
            this.BeforeStorageAccess(clientFolderName);
            this.WriteFullList(elements, clientFolderName, false);
            LogProcessingEvent(Resources.uiWritingElementsDone);
        }

        /// <summary>
        /// Normalizes the information inside the list. This includes removing leading and tailing white space etc.
        /// This default implementation does simply call the NormalizeContent() method of the elements.
        /// </summary>
        /// <param name="elements">the list of elements to be normalized</param>
        /// <returns>a list of processed elements</returns>
        public virtual List<StdElement> Normalize(List<StdElement> elements)
        {
            foreach (var element in elements)
            {
                LogProcessingEvent(element, Resources.uiNormalizing);
                element.NormalizeContent();
            }

            return elements;
        }

        /// <summary>
        /// Overrides the ToString method inherited from object with returning the friendly name.
        /// </summary>
        /// <returns>
        /// The friendly name of this client
        /// </returns>
        public override string ToString()
        {
            return this.FriendlyClientName;
        }

        /// <summary>
        /// Performs a cleanup of the elements of the list
        /// </summary>
        /// <param name="elements">
        /// The elements.
        /// </param>
        protected static void CleanUpEntities(List<StdElement> elements)
        {
            var itemsToRemove = new List<StdElement>();
            foreach (var element in elements)
            {
                SyncTools.ClearNulls(element, element.GetType());

                var contact = element as StdContact;
                if (contact != null && contact.Name == null)
                {
                    itemsToRemove.Add(element);
                }
            }

            foreach (var element in itemsToRemove)
            {
                elements.Remove(element);
            }
        }

        /// <summary>
        /// Uses the event handler QueryForLogonCredentialsEvent to query the calling instance for 
        /// Credentials.
        /// </summary>
        /// <param name="message">the message to be displayed to the user</param>
        protected void QueryForLogOnCredentials(string message)
        {
            if (this.UiDispatcher != null)
            {
                this.UiDispatcher.AskForLogOnCredentials(this, message, this.LogOnUserId, this.LogOnPassword);
                return;
            }

            if (this.QueryForLogonCredentialsEvent == null)
            {
                return;
            }

            var args = new QueryForLogOnCredentialsEventArgs
            {
                MessageForUser = message,
                LogonUserId = this.LogOnUserId,
                LogonPassword = this.LogOnPassword,
            };

            this.QueryForLogonCredentialsEvent(this, args);
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        /// This does concatenates the specified value name with the FriendlyClientName to make the 
        /// name unique for this client type.
        /// </summary>
        /// <param name="configName">the name of the value</param>
        /// <returns>the value read from the config file - string.Empty, if there is no such value.</returns>
        protected string GetConfigValue(string configName)
        {
            var value = ConfigurationManager.AppSettings[this.FriendlyClientName + "-" + configName];
            return value ?? string.Empty;
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        /// This does concatenates the specified value name with the FriendlyClientName to make the 
        /// name unique for this client type.
        /// </summary>
        /// <param name="configName">the name of the value</param>
        /// <returns>the value read from the config file - false, if there is no such value.</returns>
        protected bool GetConfigValueBoolean(string configName)
        {
            var value = this.GetConfigValue(configName);
            bool returnValue;
            if (bool.TryParse(value, out returnValue))
            {
                return returnValue;
            }

            return false;
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected abstract List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result);

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected abstract void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting);

        /// <summary>
        /// virtual method that will be called just before accessing the target/source systems storage path. This
        /// enables concrete client implementations to do checks and preparations needed to access the target system
        /// </summary>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        protected virtual void BeforeStorageAccess(string clientFolderName)
        {
        }

        /// <summary>
        /// Writes a single element to the list of elements; overwrites an existing element with the same id
        /// </summary>
        /// <param name="list">the list of elements the new element should be added to</param>
        /// <param name="element">the new element that should be added</param>
        private static void WriteElement(ICollection<StdElement> list, StdElement element)
        {
            WriteElement(list, element, false);
        }

        /// <summary>
        /// Writes a single element to the list of elements
        /// </summary>
        /// <param name="list">the list of elements the new element should be added to</param>
        /// <param name="element">the new element that should be added</param>
        /// <param name="skipIfExisting">if false: overwrites an existing element with the same id</param>
        /// <returns>true if writing was successfull, false if the entry has been skipped</returns>
        private static bool WriteElement(ICollection<StdElement> list, StdElement element, bool skipIfExisting)
        {
            var listEntry = (from entry in list where entry.Id == element.Id select entry).FirstOrDefault();

            if (listEntry != null)
            {
                if (skipIfExisting)
                {
                    return false;
                }

                list.Remove(listEntry);
            }

            list.Add(element);
            return true;
        }

        /// <summary>
        /// Writes a list of elements to the list of elements; overwrites an existing element with the same id
        /// </summary>
        /// <param name="list">the list of elements the new element should be added to</param>
        /// <param name="elements">the new elements that should be added</param>
        private static void WriteElementRange(ICollection<StdElement> list, IEnumerable<StdElement> elements)
        {
            WriteElementRange(list, elements, false);
        }

        /// <summary>
        /// Writes a list of elements to the list of elements
        /// </summary>
        /// <param name="list">the list of elements the new element should be added to</param>
        /// <param name="elements">the new elements that should be added</param>
        /// <param name="skipIfExisting">if false: overwrites an existing element with the same id</param>
        private static void WriteElementRange(ICollection<StdElement> list, IEnumerable<StdElement> elements, bool skipIfExisting)
        {
            foreach (var element in elements)
            {
                WriteElement(list, element, skipIfExisting);
            }
        }
    }
}
