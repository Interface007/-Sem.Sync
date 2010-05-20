// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The view contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer.ContactService
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;

    /// <summary>
    /// The view contact.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ViewContact", 
        Namespace = "http://schemas.datacontract.org/2004/07/Sem.Sync.OnlineStorage")]
    public class ViewContact : object, System.ComponentModel.INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// The city field.
        /// </summary>
        private string CityField;

        /// <summary>
        /// The full name field.
        /// </summary>
        private string FullNameField;

        /// <summary>
        /// The picture field.
        /// </summary>
        private byte[] PictureField;

        /// <summary>
        /// The street field.
        /// </summary>
        private string StreetField;

        #endregion

        #region Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets City.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public string City
        {
            get
            {
                return this.CityField;
            }

            set
            {
                if (object.ReferenceEquals(this.CityField, value) != true)
                {
                    this.CityField = value;
                    this.RaisePropertyChanged("City");
                }
            }
        }

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public string FullName
        {
            get
            {
                return this.FullNameField;
            }

            set
            {
                if (object.ReferenceEquals(this.FullNameField, value) != true)
                {
                    this.FullNameField = value;
                    this.RaisePropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets or sets Picture.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public byte[] Picture
        {
            get
            {
                return this.PictureField;
            }

            set
            {
                if (object.ReferenceEquals(this.PictureField, value) != true)
                {
                    this.PictureField = value;
                    this.RaisePropertyChanged("Picture");
                }
            }
        }

        /// <summary>
        /// Gets or sets Street.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute]
        public string Street
        {
            get
            {
                return this.StreetField;
            }

            set
            {
                if (object.ReferenceEquals(this.StreetField, value) != true)
                {
                    this.StreetField = value;
                    this.RaisePropertyChanged("Street");
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
    /// The i contact view service.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ContactService.IContactViewService")]
    public interface IContactViewService
    {
        #region Public Methods

        /// <summary>
        /// The begin get all.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="asyncState">
        /// The async state.
        /// </param>
        /// <returns>
        /// </returns>
        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, 
            Action = "http://tempuri.org/IContactViewService/GetAll", 
            ReplyAction = "http://tempuri.org/IContactViewService/GetAllResponse")]
        IAsyncResult BeginGetAll(string clientFolderName, AsyncCallback callback, object asyncState);

        /// <summary>
        /// The end get all.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// </returns>
        ObservableCollection<ViewContact> EndGetAll(
            IAsyncResult result);

        #endregion
    }

    /// <summary>
    /// The i contact view service channel.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IContactViewServiceChannel : ContactViewer.ContactService.IContactViewService, 
                                                  System.ServiceModel.IClientChannel
    {
    }

    /// <summary>
    /// The get all completed event args.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public class GetAllCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        #region Constants and Fields

        /// <summary>
        /// The results.
        /// </summary>
        private readonly object[] results;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="cancelled">
        /// The cancelled.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public GetAllCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Result.
        /// </summary>
        public ObservableCollection<ViewContact> Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return
                    (System.Collections.ObjectModel.ObservableCollection<ViewContact>)
                     (this.results[0]);
            }
        }

        #endregion
    }

    /// <summary>
    /// The contact view service client.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public class ContactViewServiceClient :
        System.ServiceModel.ClientBase<IContactViewService>, 
        ContactViewer.ContactService.IContactViewService
    {
        #region Constants and Fields

        /// <summary>
        /// The on begin close delegate.
        /// </summary>
        private BeginOperationDelegate onBeginCloseDelegate;

        /// <summary>
        /// The on begin get all delegate.
        /// </summary>
        private BeginOperationDelegate onBeginGetAllDelegate;

        /// <summary>
        /// The on begin open delegate.
        /// </summary>
        private BeginOperationDelegate onBeginOpenDelegate;

        /// <summary>
        /// The on close completed delegate.
        /// </summary>
        private SendOrPostCallback onCloseCompletedDelegate;

        /// <summary>
        /// The on end close delegate.
        /// </summary>
        private EndOperationDelegate onEndCloseDelegate;

        /// <summary>
        /// The on end get all delegate.
        /// </summary>
        private EndOperationDelegate onEndGetAllDelegate;

        /// <summary>
        /// The on end open delegate.
        /// </summary>
        private EndOperationDelegate onEndOpenDelegate;

        /// <summary>
        /// The on get all completed delegate.
        /// </summary>
        private SendOrPostCallback onGetAllCompletedDelegate;

        /// <summary>
        /// The on open completed delegate.
        /// </summary>
        private SendOrPostCallback onOpenCompletedDelegate;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactViewServiceClient"/> class.
        /// </summary>
        public ContactViewServiceClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactViewServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        public ContactViewServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactViewServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactViewServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactViewServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactViewServiceClient(
            string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactViewServiceClient"/> class.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ContactViewServiceClient(
            Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// The close completed.
        /// </summary>
        public event EventHandler<AsyncCompletedEventArgs> CloseCompleted;

        /// <summary>
        /// The get all completed.
        /// </summary>
        public event EventHandler<GetAllCompletedEventArgs> GetAllCompleted;

        /// <summary>
        /// The open completed.
        /// </summary>
        public event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets CookieContainer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public CookieContainer CookieContainer
        {
            get
            {
                var httpCookieContainerManager =
                    this.InnerChannel.GetProperty<IHttpCookieContainerManager>();
                if (httpCookieContainerManager != null)
                {
                    return httpCookieContainerManager.CookieContainer;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                var httpCookieContainerManager =
                    this.InnerChannel.GetProperty<IHttpCookieContainerManager>();
                if (httpCookieContainerManager != null)
                {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                        "ookieContainerBindingElement.");
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The close async.
        /// </summary>
        public void CloseAsync()
        {
            this.CloseAsync(null);
        }

        /// <summary>
        /// The close async.
        /// </summary>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public void CloseAsync(object userState)
        {
            if (this.onBeginCloseDelegate == null)
            {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }

            if (this.onEndCloseDelegate == null)
            {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }

            if (this.onCloseCompletedDelegate == null)
            {
                this.onCloseCompletedDelegate = new SendOrPostCallback(this.OnCloseCompleted);
            }

            this.InvokeAsync(
                this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        public void GetAllAsync(string clientFolderName)
        {
            this.GetAllAsync(clientFolderName, null);
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public void GetAllAsync(string clientFolderName, object userState)
        {
            if (this.onBeginGetAllDelegate == null)
            {
                this.onBeginGetAllDelegate = new BeginOperationDelegate(this.OnBeginGetAll);
            }

            if (this.onEndGetAllDelegate == null)
            {
                this.onEndGetAllDelegate = new EndOperationDelegate(this.OnEndGetAll);
            }

            if (this.onGetAllCompletedDelegate == null)
            {
                this.onGetAllCompletedDelegate = new SendOrPostCallback(this.OnGetAllCompleted);
            }

            this.InvokeAsync(
                this.onBeginGetAllDelegate, 
                new object[] { clientFolderName }, 
                this.onEndGetAllDelegate, 
                this.onGetAllCompletedDelegate, 
                userState);
        }

        /// <summary>
        /// The open async.
        /// </summary>
        public void OpenAsync()
        {
            this.OpenAsync(null);
        }

        /// <summary>
        /// The open async.
        /// </summary>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public void OpenAsync(object userState)
        {
            if (this.onBeginOpenDelegate == null)
            {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }

            if (this.onEndOpenDelegate == null)
            {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }

            if (this.onOpenCompletedDelegate == null)
            {
                this.onOpenCompletedDelegate = new SendOrPostCallback(this.OnOpenCompleted);
            }

            this.InvokeAsync(
                this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }

        #endregion

        #region Implemented Interfaces

        #region IContactViewService

        /// <summary>
        /// The begin get all.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="asyncState">
        /// The async state.
        /// </param>
        /// <returns>
        /// </returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IAsyncResult ContactViewer.ContactService.IContactViewService.BeginGetAll(
            string clientFolderName, AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginGetAll(clientFolderName, callback, asyncState);
        }

        /// <summary>
        /// The end get all.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// </returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObservableCollection<ViewContact> ContactViewer.ContactService.IContactViewService.EndGetAll(IAsyncResult result)
        {
            return base.Channel.EndGetAll(result);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The create channel.
        /// </summary>
        /// <returns>
        /// </returns>
        protected override IContactViewService CreateChannel()
        {
            return new ContactViewServiceClientChannel(this);
        }

        /// <summary>
        /// The on begin close.
        /// </summary>
        /// <param name="inValues">
        /// The in values.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="asyncState">
        /// The async state.
        /// </param>
        /// <returns>
        /// </returns>
        private IAsyncResult OnBeginClose(object[] inValues, AsyncCallback callback, object asyncState)
        {
            return ((System.ServiceModel.ICommunicationObject)this).BeginClose(callback, asyncState);
        }

        /// <summary>
        /// The on begin get all.
        /// </summary>
        /// <param name="inValues">
        /// The in values.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="asyncState">
        /// The async state.
        /// </param>
        /// <returns>
        /// </returns>
        private IAsyncResult OnBeginGetAll(object[] inValues, AsyncCallback callback, object asyncState)
        {
            var clientFolderName = (string)(inValues[0]);
            return ((ContactViewer.ContactService.IContactViewService)this).BeginGetAll(
                clientFolderName, callback, asyncState);
        }

        /// <summary>
        /// The on begin open.
        /// </summary>
        /// <param name="inValues">
        /// The in values.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="asyncState">
        /// The async state.
        /// </param>
        /// <returns>
        /// </returns>
        private IAsyncResult OnBeginOpen(object[] inValues, AsyncCallback callback, object asyncState)
        {
            return ((System.ServiceModel.ICommunicationObject)this).BeginOpen(callback, asyncState);
        }

        /// <summary>
        /// The on close completed.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void OnCloseCompleted(object state)
        {
            if (this.CloseCompleted != null)
            {
                var e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(
                    this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }

        /// <summary>
        /// The on end close.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// </returns>
        private object[] OnEndClose(IAsyncResult result)
        {
            ((System.ServiceModel.ICommunicationObject)this).EndClose(result);
            return null;
        }

        /// <summary>
        /// The on end get all.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// </returns>
        private object[] OnEndGetAll(IAsyncResult result)
        {
            ObservableCollection<ViewContact> retVal =
                ((ContactViewer.ContactService.IContactViewService)this).EndGetAll(result);
            return new object[] { retVal };
        }

        /// <summary>
        /// The on end open.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// </returns>
        private object[] OnEndOpen(IAsyncResult result)
        {
            ((System.ServiceModel.ICommunicationObject)this).EndOpen(result);
            return null;
        }

        /// <summary>
        /// The on get all completed.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void OnGetAllCompleted(object state)
        {
            if (this.GetAllCompleted != null)
            {
                var e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetAllCompleted(this, new GetAllCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }

        /// <summary>
        /// The on open completed.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void OnOpenCompleted(object state)
        {
            if (this.OpenCompleted != null)
            {
                var e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(
                    this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }

        #endregion

        /// <summary>
        /// The contact view service client channel.
        /// </summary>
        private class ContactViewServiceClientChannel : ChannelBase<IContactViewService>, 
                                                        ContactViewer.ContactService.IContactViewService
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ContactViewServiceClientChannel"/> class.
            /// </summary>
            /// <param name="client">
            /// The client.
            /// </param>
            public ContactViewServiceClientChannel(
                ClientBase<IContactViewService> client)
                : base(client)
            {
            }

            #endregion

            #region Implemented Interfaces

            #region IContactViewService

            /// <summary>
            /// The begin get all.
            /// </summary>
            /// <param name="clientFolderName">
            /// The client folder name.
            /// </param>
            /// <param name="callback">
            /// The callback.
            /// </param>
            /// <param name="asyncState">
            /// The async state.
            /// </param>
            /// <returns>
            /// </returns>
            public IAsyncResult BeginGetAll(
                string clientFolderName, AsyncCallback callback, object asyncState)
            {
                var _args = new object[1];
                _args[0] = clientFolderName;
                IAsyncResult _result = this.BeginInvoke("GetAll", _args, callback, asyncState);
                return _result;
            }

            /// <summary>
            /// The end get all.
            /// </summary>
            /// <param name="result">
            /// The result.
            /// </param>
            /// <returns>
            /// </returns>
            public ObservableCollection<ViewContact> EndGetAll(IAsyncResult result)
            {
                var _args = new object[0];
                var _result =
                    (System.Collections.ObjectModel.ObservableCollection<ViewContact>)
                     (this.EndInvoke("GetAll", _args, result));
                return _result;
            }

            #endregion

            #endregion
        }
    }
}