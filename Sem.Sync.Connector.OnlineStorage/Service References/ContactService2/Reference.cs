// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The contact list container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OnlineStorage.ContactService2
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    /// <summary>
    /// The contact list container.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ContactListContainer", 
        Namespace = "http://schemas.datacontract.org/2004/07/Sem.Sync.OnlineStorage2")]
    [System.SerializableAttribute]
    public class ContactListContainer : object, 
                                        System.Runtime.Serialization.IExtensibleDataObject, 
                                        System.ComponentModel.INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// The contact list field.
        /// </summary>
        [System.Runtime.Serialization.OptionalFieldAttribute]
        private string ContactListField;

        /// <summary>
        /// The first element index field.
        /// </summary>
        [System.Runtime.Serialization.OptionalFieldAttribute]
        private int FirstElementIndexField;

        /// <summary>
        /// The total elements field.
        /// </summary>
        [System.Runtime.Serialization.OptionalFieldAttribute]
        private int TotalElementsField;

        /// <summary>
        /// The extension data field.
        /// </summary>
        [System.NonSerializedAttribute]
        private ExtensionDataObject extensionDataField;

        #endregion

        #region Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ContactList.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public string ContactList
        {
            get
            {
                return this.ContactListField;
            }

            set
            {
                if (object.ReferenceEquals(this.ContactListField, value) != true)
                {
                    this.ContactListField = value;
                    this.RaisePropertyChanged("ContactList");
                }
            }
        }

        /// <summary>
        /// Gets or sets ExtensionData.
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }

            set
            {
                this.extensionDataField = value;
            }
        }

        /// <summary>
        /// Gets or sets FirstElementIndex.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public int FirstElementIndex
        {
            get
            {
                return this.FirstElementIndexField;
            }

            set
            {
                if (this.FirstElementIndexField.Equals(value) != true)
                {
                    this.FirstElementIndexField = value;
                    this.RaisePropertyChanged("FirstElementIndex");
                }
            }
        }

        /// <summary>
        /// Gets or sets TotalElements.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public int TotalElements
        {
            get
            {
                return this.TotalElementsField;
            }

            set
            {
                if (this.TotalElementsField.Equals(value) != true)
                {
                    this.TotalElementsField = value;
                    this.RaisePropertyChanged("TotalElements");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    /// <summary>
    /// The i contact service.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage", 
        ConfigurationName = "ContactService2.IContactService")]
    public interface IContactService
    {
        #region Public Methods

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="startElementIndex">
        /// The start element index.
        /// </param>
        /// <param name="countOfElements">
        /// The count of elements.
        /// </param>
        /// <returns>
        /// </returns>
        [System.ServiceModel.OperationContractAttribute(
            Action = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/GetAll", 
            ReplyAction = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/GetAllResponse")]
        ContactListContainer GetAll(
            string clientFolderName, int startElementIndex, int countOfElements);

        /// <summary>
        /// The write full list.
        /// </summary>
        /// <param name="elements">
        /// The elements.
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="skipIfExisting">
        /// The skip if existing.
        /// </param>
        /// <returns>
        /// The write full list.
        /// </returns>
        [System.ServiceModel.OperationContractAttribute(
            Action = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/WriteFullList", 
            ReplyAction = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/WriteFullListRes" + "ponse")
        ]
        bool WriteFullList(
            ContactListContainer elements, 
            string clientFolderName, 
            bool skipIfExisting);

        #endregion
    }

    /// <summary>
    /// The i contact service channel.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IContactServiceChannel : Sem.Sync.Connector.OnlineStorage.ContactService2.IContactService, 
                                              System.ServiceModel.IClientChannel
    {
    }

    /// <summary>
    /// The contact service client.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public class ContactServiceClient :
        System.ServiceModel.ClientBase<IContactService>, 
        Sem.Sync.Connector.OnlineStorage.ContactService2.IContactService
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceClient"/> class.
        /// </summary>
        public ContactServiceClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        public ContactServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceClient"/> class.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactServiceClient(
            Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        #endregion

        #region Implemented Interfaces

        #region IContactService

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="startElementIndex">
        /// The start element index.
        /// </param>
        /// <param name="countOfElements">
        /// The count of elements.
        /// </param>
        /// <returns>
        /// </returns>
        public ContactListContainer GetAll(
            string clientFolderName, int startElementIndex, int countOfElements)
        {
            return base.Channel.GetAll(clientFolderName, startElementIndex, countOfElements);
        }

        /// <summary>
        /// The write full list.
        /// </summary>
        /// <param name="elements">
        /// The elements.
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="skipIfExisting">
        /// The skip if existing.
        /// </param>
        /// <returns>
        /// The write full list.
        /// </returns>
        public bool WriteFullList(
            ContactListContainer elements, 
            string clientFolderName, 
            bool skipIfExisting)
        {
            return base.Channel.WriteFullList(elements, clientFolderName, skipIfExisting);
        }

        #endregion

        #endregion
    }
}