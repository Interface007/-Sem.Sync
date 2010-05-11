﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OnlineStorage.ContactService2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ContactListContainer", Namespace="http://schemas.datacontract.org/2004/07/Sem.Sync.OnlineStorage2")]
    [System.SerializableAttribute()]
    public partial class ContactListContainer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContactListField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int FirstElementIndexField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TotalElementsField;
        
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
        public string ContactList {
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
        public int FirstElementIndex {
            get {
                return this.FirstElementIndexField;
            }
            set {
                if ((this.FirstElementIndexField.Equals(value) != true)) {
                    this.FirstElementIndexField = value;
                    this.RaisePropertyChanged("FirstElementIndex");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalElements {
            get {
                return this.TotalElementsField;
            }
            set {
                if ((this.TotalElementsField.Equals(value) != true)) {
                    this.TotalElementsField = value;
                    this.RaisePropertyChanged("TotalElements");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://svenerikmatzen.com/Sem/Sync/OnlineStorage", ConfigurationName="ContactService2.IContactService")]
    public interface IContactService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/GetAll", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/GetAllResponse")]
        Sem.Sync.Connector.OnlineStorage.ContactService2.ContactListContainer GetAll(string clientFolderName, int startElementIndex, int countOfElements);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/WriteFullList", ReplyAction="http://svenerikmatzen.com/Sem/Sync/OnlineStorage/IContactService/WriteFullListRes" +
            "ponse")]
        bool WriteFullList(Sem.Sync.Connector.OnlineStorage.ContactService2.ContactListContainer elements, string clientFolderName, bool skipIfExisting);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IContactServiceChannel : Sem.Sync.Connector.OnlineStorage.ContactService2.IContactService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ContactServiceClient : System.ServiceModel.ClientBase<Sem.Sync.Connector.OnlineStorage.ContactService2.IContactService>, Sem.Sync.Connector.OnlineStorage.ContactService2.IContactService {
        
        public ContactServiceClient() {
        }
        
        public ContactServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ContactServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContactServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContactServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Sem.Sync.Connector.OnlineStorage.ContactService2.ContactListContainer GetAll(string clientFolderName, int startElementIndex, int countOfElements) {
            return base.Channel.GetAll(clientFolderName, startElementIndex, countOfElements);
        }
        
        public bool WriteFullList(Sem.Sync.Connector.OnlineStorage.ContactService2.ContactListContainer elements, string clientFolderName, bool skipIfExisting) {
            return base.Channel.WriteFullList(elements, clientFolderName, skipIfExisting);
        }
    }
}
