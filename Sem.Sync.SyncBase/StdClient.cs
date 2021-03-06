﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class can (should) be used as a base class for "client" classes. It already implements some of the aspects
//   of such a client class, so you only need to implement the specific methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.EventArgs;
    using Sem.GenericHelpers.Interfaces;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;
    using Sem.Sync.SyncBase.Properties;

    /// <summary>
    /// This class can (should) be used as a base class for "client" classes. 
    /// It already implements some of the aspects of such a client class, 
    /// so you only need to implement the specific methods.
    /// </summary>
    public abstract class StdClient : SyncComponent, IClientBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StdClient" /> class. This will also read the saved credentials 
        ///   for this Client type from the app.config file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors",
            Justification =
                "The virtual method that is called is a property that always should just return a string value and does not need any class initialization."
            )]
        protected StdClient()
        {
            this.LogOnUserId = this.GetConfigValue("LogonUserId");
            this.LogOnPassword = this.GetConfigValue("LogonPassword");
        }

        #endregion

        #region Events

        /// <summary>
        ///   Informs the subscriber of this event that we need some credentials.
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogonCredentialsEvent;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public virtual string FriendlyClientName
        {
            get
            {
                var attributes =
                    this.GetType().GetCustomAttributes(typeof(ConnectorDescriptionAttribute), false) as
                    ConnectorDescriptionAttribute[];
                if (attributes != null && attributes.Length > 0)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!string.IsNullOrEmpty(attribute.DisplayName))
                        {
                            return attribute.DisplayName;
                        }
                    }
                }

                return this.GetType().Name;
            }
        }

        /// <summary>
        ///   Gets or sets the domain part of the user credentials to access the target system
        /// </summary>
        public string LogOnDomain { get; set; }

        /// <summary>
        ///   Gets or sets the password part of the user credentials to access the target system
        /// </summary>
        public string LogOnPassword { get; set; }

        /// <summary>
        ///   Gets or sets the user id part of the user credentials to access the target system
        /// </summary>
        public string LogOnUserId { get; set; }

        /// <summary>
        ///   Gets or sets the domain part of the user credentials to access the target system
        /// </summary>
        public IUiInteraction UiDispatcher { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Normalizes the information inside the list. This includes removing leading and tailing white space etc.
        ///   This default implementation does simply call the NormalizeContent() method of the elements.
        /// </summary>
        /// <param name="elements">The list of elements to be normalized.</param>
        /// <returns>A list of processed elements.</returns>
        public virtual List<StdElement> Normalize(List<StdElement> elements)
        {
            foreach (var element in elements)
            {
                this.LogProcessingEvent(element, Resources.uiNormalizing);
                element.NormalizeContent();
            }

            return elements;
        }

        /// <summary>
        /// Overrides the ToString method inherited from object with returning the friendly name.
        /// </summary>
        /// <returns>The friendly name of this client.</returns>
        public override string ToString()
        {
            return this.FriendlyClientName;
        }

        #endregion

        #region IClientBase

        /// <summary>
        /// Overridable implementation of the process of writing a single element. In the
        ///   default implementation this calls <see cref="GetAll"/>, WriteElement and 
        ///   <see cref="WriteRange"/> to write the element and performs logging calls.
        /// </summary>
        /// <param name="element"> The element to be added. </param>
        /// <param name="clientFolderName">
        ///   The information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
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
        ///   If the elements are already in place, they will be overridden.
        /// </summary>
        /// <param name="elements">
        /// the elements to be added in a list of elements
        /// </param>
        /// <param name="clientFolderName">
        ///   The information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements);
            this.WriteRange(data, clientFolderName);
        }

        /// <summary>
        /// Deletes a list/collection of entities stecified by the identifiers.
        /// </summary>
        /// <param name="elementsToDelete">
        ///   The elements to be to deleted. This depends on the internal implementation of the storage - mostly
        ///   only the id read from <see cref="StdElement.ExternalIdentifier"/> is needed to delete an element.
        /// </param>
        /// <param name="clientFolderName">
        ///   The information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
        }

        /// <summary>
        /// Overridable implementation of the process of retrieving the full list of elements. In the
        ///   default implementation this calls <see cref="BeforeStorageAccess"/> and 
        ///   <see cref="ReadFullList"/> to get the elements and performs sorting and logging calls. 
        ///   Override this method if you need additional control over the read process.
        /// </summary>
        /// <param name="clientFolderName">
        ///   The information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <returns>The list with the newly added elements</returns>
        public virtual List<StdElement> GetAll(string clientFolderName)
        {
            var result = new List<StdElement>();

            this.LogProcessingEvent(Resources.uiReadingElements);
            this.BeforeStorageAccess(clientFolderName);
            result = this.ReadFullList(clientFolderName, result);

            this.LogProcessingEvent(Resources.uiSorting);
            result.Sort();
            this.Normalize(result);
            this.LogProcessingEvent(Resources.uiSorted);

            return result;
        }

        /// <summary>
        /// Implementation of the process of writing a single element and skipping this process if this 
        ///   element is already present. If the element does not exist, it will be added. If it does exist
        ///   the element will not be added and not be overridden.
        /// </summary>
        /// <param name="element">The element to be added</param>
        /// <param name="clientFolderName">
        ///   The information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void MergeMissingItem(StdElement element, string clientFolderName)
        {
            this.LogProcessingEvent(element, Resources.uiAddingMissingElement);
            var data = this.GetAll(clientFolderName);
            var added = WriteElement(data, element, true);
            this.WriteRange(data, clientFolderName);
            this.LogProcessingEvent(element, added ? Resources.uiElementAdded : Resources.uiElementSkipped);
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        ///   skipping this process if an element is already present. Missing elements will be added, existing 
        ///   elements will not be altered.
        /// </summary>
        /// <param name="elements"> The elements to be added in a list of elements </param>
        /// <param name="clientFolderName">
        /// The information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            ////LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements, true);
            data.Sort();
            this.WriteRange(data, clientFolderName);

            ////LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, elements.Count));
        }

        /// <summary>
        /// virtual method that should be (optionally) implemented by the client to remove duplicate entities
        /// </summary>
        /// <param name="clientFolderName">
        /// the information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void RemoveDuplicates(string clientFolderName)
        {
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        ///   overwriting the elements if they do already exist. Missing elements will be added, existing 
        ///   elements will overwritten with the new elements.
        /// </summary>
        /// <param name="elements"> The elements to be added in a list of elements </param>
        /// <param name="clientFolderName">
        /// The information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public virtual void WriteRange(List<StdElement> elements, string clientFolderName)
        {
            this.LogProcessingEvent(Resources.uiWritingElements);
            this.BeforeStorageAccess(clientFolderName);
            this.WriteFullList(elements, clientFolderName, false);

            ////LogProcessingEvent(Resources.uiWritingElementsDone);
        }

        /// <summary>
        /// Adds a pause (Thread.Sleep) with the default of 8789 milliseconds.
        /// </summary>
        protected void ThinkTime()
        {
            this.ThinkTime(8789);
        }

        /// <summary>
        /// Adds a random pause with a max value of <paramref name="max"/> milliseconds.
        /// </summary>
        /// <param name="max"></param>
        protected void ThinkTime(int max)
        {
            Thread.Sleep(new Random().Next(230, max));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs a cleanup of the elements of the list
        /// </summary>
        /// <param name="elements"> The elements. </param>
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
        /// Extracts the column definition file name of a multi line parameter
        /// </summary>
        /// <param name="clientFolderName"> The client folder name that may contain two lines. </param>
        /// <returns> the file name of the column definition file. </returns>
        protected static string GetColumnDefinitionFileName(string clientFolderName)
        {
            var fileName = clientFolderName.Contains("\n") || clientFolderName.Contains("|")
                               ? clientFolderName.Split(new[] { "\n", "|" }, StringSplitOptions.RemoveEmptyEntries)[1].
                                     Trim()
                               : string.Empty;

            if (!fileName.Contains("\\") && clientFolderName.Contains("\\"))
            {
                var path = GetFileName(clientFolderName);
                return Path.Combine(Path.GetDirectoryName(path) ?? "", fileName);
            }

            return fileName;
        }

        /// <summary>
        /// Extracts the source/target file name of a multi line parameter
        /// </summary>
        /// <param name="clientFolderName"> The client folder name that may contain two lines. </param>
        /// <returns> the source/target file name </returns>
        protected static string GetFileName(string clientFolderName)
        {
            var fileName = clientFolderName;
            if (fileName.Contains("\n") || fileName.Contains("|"))
            {
                fileName = fileName.Split(new[] { "\n", "|" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            }

            return fileName;
        }

        /// <summary>
        /// Virtual method that will be called just before accessing the target/source systems storage path. This
        ///   enables concrete client implementations to do checks and preparations needed to access the target system
        /// </summary>
        /// <param name="clientFolderName">
        ///     The information where inside the source the elements reside - 
        ///     This does not need to be a real "path", but need to be 
        ///     something that can be expressed as a string.
        /// </param>
        protected virtual void BeforeStorageAccess(string clientFolderName)
        {
        }

        /// <summary>
        /// Read the column definition from the column definition file specified with the
        ///   parameter <paramref name="columnDefinitionFile"/>. If there is no such file 
        ///   specified, a list of such entries will be created by searching the object 
        ///   recursively for properties.
        /// </summary>
        /// <param name="columnDefinitionFile"> the file that does contain a list of <see cref="ColumnDefinition"/>  </param>
        /// <param name="type"> The type to create the definition for </param>
        /// <returns> a list of <see cref="ColumnDefinition"/> to describe the columns  </returns>
        protected List<ColumnDefinition> GetColumnDefinition(string columnDefinitionFile, Type type)
        {
            var result = new List<ColumnDefinition>();
            var definitionFileName = columnDefinitionFile.Replace(".{write}", string.Empty);
            this.LogProcessingEvent("reading/building column definition");

            if (!string.IsNullOrEmpty(columnDefinitionFile) && File.Exists(definitionFileName))
            {
                result = result.LoadFrom(definitionFileName, new[] { typeof(ColumnDefinition) });
                if (result.LongCount() > 0)
                {
                    this.LogProcessingEvent(
                        "definition file with {0} columns used ({1}).", result.LongCount(), definitionFileName);
                    return result;
                }
            }

            this.LogProcessingEvent("building column definition from type {0}...", type.Name);
            result = (from x in Tools.GetPropertyList(string.Empty, type) select new ColumnDefinition(x)).ToList();

            if (!string.IsNullOrEmpty(columnDefinitionFile) && Path.GetExtension(columnDefinitionFile) == ".{write}")
            {
                this.LogProcessingEvent("saving column definition to file {0}...", definitionFileName);
                result.SaveTo(definitionFileName, new[] { typeof(ColumnDefinition) });
            }

            return result;
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        ///   This does concatenate the specified value name with the FriendlyClientName to make the 
        ///   name unique for this client type.
        /// </summary>
        /// <param name="configName"> the name of the value </param>
        /// <returns> the value read from the config file - string.Empty, if there is no such value. </returns>
        protected string GetConfigValue(string configName)
        {
            return this.GetConfigValue(configName, string.Empty);
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        ///   This does concatenate the specified value name with the FriendlyClientName to make the 
        ///   name unique for this client type.
        /// </summary>
        /// <param name="configName"> the name of the value </param>
        /// <param name="defaultValue"> The default Value for the case where there is noc value inside the app.config. </param>
        /// <returns> the value read from the config file - string.Empty, if there is no such value. </returns>
        protected string GetConfigValue(string configName, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[this.FriendlyClientName + "-" + configName];
            return value ?? defaultValue;
        }

        /// <summary>
        /// Reads a value from the app config file, returns defaultValue if the value is not set or not
        /// interpretable as an integer.
        ///   This does concatenate the specified value name with the FriendlyClientName to make the 
        ///   name unique for this client type.
        /// </summary>
        /// <param name="configName"> the name of the value  </param>
        /// <param name="defaultValue"> The default Value for the case where there is noc value inside the app.config. </param>
        /// <returns> the value read from the config file - defaultValue if the value cannot be read as an int.  </returns>
        protected int GetConfigValueInt(string configName, int defaultValue)
        {
            var value = ConfigurationManager.AppSettings[this.FriendlyClientName + "-" + configName];
            int result;
            return int.TryParse(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        ///   This does concatenates the specified value name with the FriendlyClientName to make the 
        ///   name unique for this client type.
        /// </summary>
        /// <param name="configName"> the name of the value </param>
        /// <returns> the value read from the config file - false, if there is no such value. </returns>
        protected bool GetConfigValueBoolean(string configName)
        {
            return this.GetConfigValueBoolean(configName, false);
        }

        /// <summary>
        /// Reads a value from the app config file, returns string.Empty if the value is not set.
        ///   This does concatenates the specified value name with the FriendlyClientName to make the 
        ///   name unique for this client type.
        /// </summary>
        /// <param name="configName"> the name of the value </param>
        /// <param name="defaultValue"> the value to be returned if no value is specified in the config file </param>
        /// <returns> the value read from the config file - false, if there is no such value. </returns>
        protected bool GetConfigValueBoolean(string configName, bool defaultValue)
        {
            var value = this.GetConfigValue(configName);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            // in case of a non-parsable value, TryParse returns "false", so we can simply return
            // the AND operation of TryParse and the parsed return value (if there is one).
            bool returnValue;
            return bool.TryParse(value, out returnValue) && returnValue;
        }

        /// <summary>
        /// Uses the event handler QueryForLogonCredentialsEvent to query the calling instance for Credentials.
        /// </summary>
        /// <param name="message"> the message to be displayed to the user </param>
        protected void QueryForLogOnCredentials(string message)
        {
            if (this.UiDispatcher != null)
            {
                var logonCredentialRequest = new LogonCredentialRequest(this, message, message);
                this.UiDispatcher.AskForLogOnCredentials(logonCredentialRequest);
                return;
            }

            if (this.QueryForLogonCredentialsEvent == null)
            {
                return;
            }

            var args = new QueryForLogOnCredentialsEventArgs
                { MessageForUser = message, LogonUserId = this.LogOnUserId, LogonPassword = this.LogOnPassword, };

            this.QueryForLogonCredentialsEvent(this, args);
        }

        /// <summary>
        /// Virtual read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        ///     The information from where inside the source the elements should be read - 
        ///     This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        ///     The list of elements that should get the elements. The elements should be added to
        ///     the list instead of replacing it.
        /// </param>
        /// <returns> The list with the newly added elements </returns>
        protected virtual List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            throw new NotImplementedException("Reading contacts has not been implemented for this connector.");
        }

        /// <summary>
        /// Virtual write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">The list of elements that should be written to the target system. </param>
        /// <param name="clientFolderName">
        ///     The information to where inside the source the elements should be written - 
        ///     This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting"> specifies whether existing elements should be updated or simply left as they are </param>
        protected virtual void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException("Writing contacts has not been implemented for this connector.");
        }

        /// <summary>
        /// Writes a single element to the list of elements; overwrites an existing element with the same id
        /// </summary>
        /// <param name="list"> The list of elements the new element should be added to </param>
        /// <param name="element"> The new element that should be added </param>
        private static void WriteElement(ICollection<StdElement> list, StdElement element)
        {
            WriteElement(list, element, false);
        }

        /// <summary>
        /// Writes a single element to the list of elements
        /// </summary>
        /// <param name="list"> The list of elements the new element should be added to </param>
        /// <param name="element"> The new element that should be added </param>
        /// <param name="skipIfExisting"> If false: overwrites an existing element with the same id </param>
        /// <returns> True if writing was successfull, false if the entry has been skipped </returns>
        private static bool WriteElement(ICollection<StdElement> list, StdElement element, bool skipIfExisting)
        {
            var asContact = element as StdContact;
            StdElement listEntry;

            if (asContact != null)
            {
                listEntry = (from entry in list
                             where
                                 entry.Id == element.Id || entry.ExternalIdentifier.Equals(asContact.ExternalIdentifier)
                             select entry).FirstOrDefault();
            }
            else
            {
                listEntry = (from entry in list where entry.Id == element.Id select entry).FirstOrDefault();
            }

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
        /// Writes a list of elements to the list of elements
        /// </summary>
        /// <param name="list"> The list of elements the new element should be added to </param>
        /// <param name="elements"> The new elements that should be added </param>
        /// <param name="skipIfExisting"> If false: overwrites an existing element with the same id </param>
        private static void WriteElementRange(
            ICollection<StdElement> list, IEnumerable<StdElement> elements, bool skipIfExisting = false)
        {
            foreach (var element in elements)
            {
                WriteElement(list, element, skipIfExisting);
            }
        }

        #endregion
    }
}