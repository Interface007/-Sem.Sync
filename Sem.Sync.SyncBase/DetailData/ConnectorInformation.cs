namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    using Attributes;

    using GenericHelpers;
    using GenericHelpers.Entities;

    /// <summary>
    /// The ConnectorInformation class does provide information about a single connector.
    /// This includes the name of the type as well as the path inside the storage and
    /// the credentials. The credentials are serialized in a save manner by encrypting
    /// them with the current users .net encryption key.
    /// </summary>
    public class ConnectorInformation
    {
        private string _name;
        private readonly Factory _factory = new Factory("Sem.Sync.SyncBase");

        public ConnectorInformation()
        {
            this.LogonCredentials = new Credentials();
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;

                this.ShowSelectFileDialog = false;
                this.ShowSelectPathDialog = false;

                var typeName = value;
                if (value.ToUpperInvariant().Contains(" OF "))
                {
                    typeName = value.Split(new[] { " of " }, StringSplitOptions.RemoveEmptyEntries)[0] + "`1";
                }

                var type = Type.GetType(this._factory.EnrichClassName(typeName));
                this.ConnectorPathDescription = new ClientStoragePathDescriptionAttribute();
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

                this.RaisePropertyChanged(string.Empty);
            }
        }

        private string _path;

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
                this.RaisePropertyChanged("Path");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowSelectPathDialog { get; set; }
        public bool ShowSelectFileDialog { get; set; }

        /// <summary>
        /// Gets the <see cref="Credentials"/> to access the storage behind the
        /// connector. The password will be stored encrpyted when serialized using the <see cref="XmlSerializer"/>.
        /// </summary>
        public Credentials LogonCredentials { get; set; }

        /// <summary>
        /// Gets the ConnectorDescription - this property will not be 
        /// serialized, because it depends only on the type of the connector
        /// and does not represent any kind of state.
        /// </summary>
        [XmlIgnore]
        public ConnectorDescriptionAttribute ConnectorDescription { get; private set; }

        [XmlIgnore]
        public ClientStoragePathDescriptionAttribute ConnectorPathDescription { get; set; }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}