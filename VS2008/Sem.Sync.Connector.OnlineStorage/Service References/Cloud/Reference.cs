﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4016
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OnlineStorage.Cloud {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ContactListContainer", Namespace="http://schemas.datacontract.org/2004/07/Sem.Sync.Cloud")]
    [System.SerializableAttribute()]
    public partial class ContactListContainer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Sem.Sync.SyncBase.StdContact[] ContactListField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Sem.Sync.Connector.OnlineStorage.Cloud.CloudCredentials CredentialsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Sem.Sync.Connector.OnlineStorage.Cloud.TechnicalMessage[] MessagesField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Sem.Sync.SyncBase.StdContact[] ContactList {
            get {
                return this.ContactListField;
            }
            set {
                if ((object.ReferenceEquals(this.ContactListField, value) != true)) {
                    this.ContactListField = value;
                    this.RaisePropertyChanged("ContactList");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Sem.Sync.Connector.OnlineStorage.Cloud.CloudCredentials Credentials {
            get {
                return this.CredentialsField;
            }
            set {
                if ((object.ReferenceEquals(this.CredentialsField, value) != true)) {
                    this.CredentialsField = value;
                    this.RaisePropertyChanged("Credentials");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Sem.Sync.Connector.OnlineStorage.Cloud.TechnicalMessage[] Messages {
            get {
                return this.MessagesField;
            }
            set {
                if ((object.ReferenceEquals(this.MessagesField, value) != true)) {
                    this.MessagesField = value;
                    this.RaisePropertyChanged("Messages");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CloudCredentials", Namespace="http://schemas.datacontract.org/2004/07/Sem.Sync.Cloud")]
    [System.SerializableAttribute()]
    public partial class CloudCredentials : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountDomainField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountPasswordField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountDomain {
            get {
                return this.AccountDomainField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountDomainField, value) != true)) {
                    this.AccountDomainField = value;
                    this.RaisePropertyChanged("AccountDomain");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountId {
            get {
                return this.AccountIdField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountIdField, value) != true)) {
                    this.AccountIdField = value;
                    this.RaisePropertyChanged("AccountId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountPassword {
            get {
                return this.AccountPasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountPasswordField, value) != true)) {
                    this.AccountPasswordField = value;
                    this.RaisePropertyChanged("AccountPassword");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TechnicalMessage", Namespace="http://schemas.datacontract.org/2004/07/Sem.Sync.Cloud")]
    [System.SerializableAttribute()]
    public partial class TechnicalMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://svenerikmatzen.com/Sem/Sync/OnlineStorage", ConfigurationName="Cloud.IStorage")]
    public interface IStorage {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/GetAll", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/GetAllResponse")]
        Sem.Sync.Connector.OnlineStorage.Cloud.ContactListContainer GetAll(string blobId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/WriteFullList", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/WriteFullListResponse")]
        bool WriteFullList(Sem.Sync.Connector.OnlineStorage.Cloud.ContactListContainer elements, string blobId, bool skipIfExisting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/DeleteBlob", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/DeleteBlobResponse")]
        void DeleteBlob(string blobId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IStorageChannel : Sem.Sync.Connector.OnlineStorage.Cloud.IStorage, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class StorageClient : System.ServiceModel.ClientBase<Sem.Sync.Connector.OnlineStorage.Cloud.IStorage>, Sem.Sync.Connector.OnlineStorage.Cloud.IStorage {
        
        public StorageClient() {
        }
        
        public StorageClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StorageClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StorageClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StorageClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Sem.Sync.Connector.OnlineStorage.Cloud.ContactListContainer GetAll(string blobId) {
            return base.Channel.GetAll(blobId);
        }
        
        public bool WriteFullList(Sem.Sync.Connector.OnlineStorage.Cloud.ContactListContainer elements, string blobId, bool skipIfExisting) {
            return base.Channel.WriteFullList(elements, blobId, skipIfExisting);
        }
        
        public void DeleteBlob(string blobId) {
            base.Channel.DeleteBlob(blobId);
        }
    }
}