﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4016
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorageConnector.Cloud {
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
        Sem.Sync.OnlineStorageConnector.Cloud.ContactListContainer GetAll(string blobId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/WriteFullList", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/WriteFullListResponse")]
        bool WriteFullList(Sem.Sync.OnlineStorageConnector.Cloud.ContactListContainer elements, string blobId, bool skipIfExisting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/DeleteBlob", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IStorage/DeleteBlobResponse")]
        void DeleteBlob(string blobId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IStorageChannel : Sem.Sync.OnlineStorageConnector.Cloud.IStorage, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class StorageClient : System.ServiceModel.ClientBase<Sem.Sync.OnlineStorageConnector.Cloud.IStorage>, Sem.Sync.OnlineStorageConnector.Cloud.IStorage {
        
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
        
        public Sem.Sync.OnlineStorageConnector.Cloud.ContactListContainer GetAll(string blobId) {
            return base.Channel.GetAll(blobId);
        }
        
        public bool WriteFullList(Sem.Sync.OnlineStorageConnector.Cloud.ContactListContainer elements, string blobId, bool skipIfExisting) {
            return base.Channel.WriteFullList(elements, blobId, skipIfExisting);
        }
        
        public void DeleteBlob(string blobId) {
            base.Channel.DeleteBlob(blobId);
        }
    }
}
