namespace Sem.Sync.SyncBase
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using EventArgs;
    using Interfaces;
    using Properties;

    /// <summary>
    /// This class can (should) be used as a base class for "client" classes. It already implements some of the aspects 
    /// of such a client class, so you only need to implement the specific methods.
    /// </summary>
    public abstract class StdClient : SyncComponent, IClientBase
    {
        #region internal implementation
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLoginCredentialsEvent;
        
        protected void QueryForLogOnCredentials(string message)
        {
            if (QueryForLoginCredentialsEvent == null) return;

            var args = new QueryForLogOnCredentialsEventArgs
                           {
                               MessageForUser = message,
                               LoginUserId = this.LoginUserId,
                               LoginPassword = this.LoginPassword,
                           };

            QueryForLoginCredentialsEvent(this, args);
        }

        protected string GetConfigValue(string configName)
        {
            var value = ConfigurationManager.AppSettings[FriendlyClientName + "-" + configName];
            return value ?? string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification="The virtual method that is called is a property that always should just return a string value and does not need any class initialization.")]
        protected StdClient()
        {
            this.LoginUserId = GetConfigValue("LoginUserId");
            this.LoginPassword = GetConfigValue("LoginPassword");
        }

        #endregion

        #region interface IClientBase

        #region abstract methods
        protected abstract List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result);
        protected abstract void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting);
        #endregion abstract methods

        protected virtual void BeforeStorageAccess(string clientFolderName)
        {
        }

        public virtual void RemoveDuplicates(string clientFolderName)
        {
        }

        public string LoginUserId { get; set; }
        public string LoginPassword { get; set; }

        public virtual List<StdElement> GetAll(string clientFolderName)
        {
            var result = new List<StdElement>();

            LogProcessingEvent(Resources.uiReadingElements);
            BeforeStorageAccess(clientFolderName);
            result = ReadFullList(clientFolderName, result);

            LogProcessingEvent(Resources.uiSorting);
            result.Sort();
            LogProcessingEvent(Resources.uiSorted);

            return result;
        }

        public virtual void AddItem(StdElement element, string clientFolderName)
        {
            LogProcessingEvent(element, Resources.uiAddingElement);
            var data = this.GetAll(clientFolderName);
            WriteElement(data, element);
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(element, Resources.uiElementAdded);
        }

        public virtual void AddRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements);
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, elements.Count));
        }

        public virtual void MergeMissingItem(StdElement element, string clientFolderName)
        {
            LogProcessingEvent(element, Resources.uiAddingMissingElement);
            var data = this.GetAll(clientFolderName);
            var added = WriteElement(data, element, true);
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(element, added ? Resources.uiElementAdded : Resources.uiElementSkipped);
        }

        public virtual void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));
            var data = this.GetAll(clientFolderName);
            WriteElementRange(data, elements, true);
            data.Sort();
            this.WriteRange(data, clientFolderName);
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, elements.Count));
        }

        public virtual void WriteRange(List<StdElement> elements, string clientFolderName)
        {
            LogProcessingEvent(Resources.uiWritingElements);
            BeforeStorageAccess(clientFolderName);
            WriteFullList(elements, clientFolderName, false);
            LogProcessingEvent(Resources.uiWritingElementsDone);
        }

        public virtual List<StdElement> Normalize(List<StdElement> elements)
        {
            foreach (var element in elements)
            {
                LogProcessingEvent(element, Resources.uiNormalizing);
                element.NormalizeContent();
            }

            return elements;
        }

        public abstract string FriendlyClientName
        {
            get;
        }

        #endregion

        private static void WriteElement(ICollection<StdElement> list, StdElement element)
        {
            WriteElement(list, element, false);
        }

        private static bool WriteElement(ICollection<StdElement> list, StdElement element, bool skipIfExisting)
        {
            var listEntry = (from entry in list where entry.Id == element.Id select entry).FirstOrDefault();

            if (listEntry != null)
            {
                if (skipIfExisting) return false;
                list.Remove(listEntry);
            }

            list.Add(element);
            return true;
        }

        private static void WriteElementRange(ICollection<StdElement> list, IEnumerable<StdElement> elements)
        {
            WriteElementRange(list, elements, false);
        }

        private static void WriteElementRange(ICollection<StdElement> list, IEnumerable<StdElement> elements, bool skipIfExisting)
        {
            foreach (var element in elements)
            {
                WriteElement(list, element, skipIfExisting);
            }
        }
    }
}
