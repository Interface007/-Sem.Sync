// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectorInformation.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The ConnectorInformation class does provide information about a single connector.
//   This includes the name of the type as well as the path inside the storage and
//   the credentials. The credentials are serialized in a save manner by encrypting
//   them with the current users .net encryption key.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// The ConnectorInformation class does provide information about a single connector.
    ///   This includes the name of the type as well as the path inside the storage and
    ///   the credentials. The credentials are serialized in a save manner by encrypting
    ///   them with the current users .net encryption key.
    /// </summary>
    [Serializable]
    public class ConnectorInformation : INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        ///   provides a factory that contains the correct default-namespace
        /// </summary>
        private readonly Factory factory = new Factory("Sem.Sync.SyncBase");

        /// <summary>
        ///   contains the name of the set of connector information
        /// </summary>
        private string name;

        /// <summary>
        ///   contains the client path for the connector
        /// </summary>
        private string path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConnectorInformation" /> class.
        /// </summary>
        public ConnectorInformation()
        {
            this.LogonCredentials = new Credentials();
        }

        #endregion

        #region Events

        /// <summary>
        ///   implements the PropertyChanged for the INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the <see cref = "ConnectorDescription" /> - this property will not be 
        ///   serialized, because it depends only on the type of the connector
        ///   and does not represent any kind of state.
        /// </summary>
        [XmlIgnore]
        public ConnectorDescriptionAttribute ConnectorDescription { get; private set; }

        /// <summary>
        ///   Gets or sets the <see cref = "ConnectorPathDescription" /> - this property will not be 
        ///   serialized, because it depends only on the type of the connector
        ///   and does not represent any kind of state.
        /// </summary>
        [XmlIgnore]
        public ClientStoragePathDescriptionAttribute ConnectorPathDescription { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref = "Credentials" /> to access the storage behind the
        ///   connector. The password will be stored encrpyted when serialized using the <see cref = "XmlSerializer" />.
        /// </summary>
        public Credentials LogonCredentials { get; set; }

        /// <summary>
        ///   Gets or sets the name of this set of information .
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;

                this.ShowSelectFileDialog = false;
                this.ShowSelectPathDialog = false;

                var typeName = value;
                if (!string.IsNullOrEmpty(value) && value.ToUpperInvariant().Contains(" OF "))
                {
                    typeName = value.Split(new[] { " of ", " OF ", " Of ", " oF " }, StringSplitOptions.RemoveEmptyEntries)[0] + "`1";
                }

                var type = Type.GetType(this.factory.EnrichClassName(typeName));
                this.ConnectorPathDescription = new ClientStoragePathDescriptionAttribute();
                if (type == null)
                {
                    return;
                }

                var sourceTypeAttributes = type.GetCustomAttributes(typeof(ClientStoragePathDescriptionAttribute), false);
                foreach (ClientStoragePathDescriptionAttribute attribute in sourceTypeAttributes)
                {
                    this.ConnectorPathDescription = attribute;
                    this.ShowSelectFileDialog = attribute.ReferenceType == ClientPathType.FileSystemFileNameAndPath;
                    this.ShowSelectPathDialog = attribute.ReferenceType == ClientPathType.FileSystemPath;
                    if (string.IsNullOrEmpty(this.Path))
                    {
                        this.Path = attribute.Default;
                    }
                }

                sourceTypeAttributes = type.GetCustomAttributes(typeof(ConnectorDescriptionAttribute), false);
                if (sourceTypeAttributes.Length > 0)
                {
                    var attribute = (ConnectorDescriptionAttribute)sourceTypeAttributes[0];
                    this.ConnectorDescription = attribute;
                }
                else
                {
                    this.ConnectorDescription = new ConnectorDescriptionAttribute();
                }

                this.RaisePropertyChanged("Name");
                this.RaisePropertyChanged("Path");
                this.RaisePropertyChanged("ConnectorDescription");
            }
        }

        /// <summary>
        ///   Gets or sets the file system path for this set of information
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }

            set
            {
                this.path = value;
                this.RaisePropertyChanged("Path");
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to show a select file dialog.
        /// </summary>
        public bool ShowSelectFileDialog { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to show a select path dialog.
        /// </summary>
        public bool ShowSelectPathDialog { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calls the event to inform other classes about an internal change of this objects 
        ///   state - this will cause the GUI to read the data from this object.
        /// </summary>
        /// <param name="propertyName">
        /// The property name that has been changed. 
        /// </param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}