namespace Sem.Sync.LocalSyncManager.Business
{
    using System;
    using System.Xml.Serialization;

    using GenericHelpers;
    using GenericHelpers.Entities;

    using SyncBase.Attributes;

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

                string typeName = value;
                if (value.ToLowerInvariant().Contains(" of "))
                {
                    typeName = value.Split(new[] { " of " }, StringSplitOptions.RemoveEmptyEntries)[0] + "`1";
                }

                var type = Type.GetType(this._factory.EnrichClassName(typeName));
                var sourceTypeAttributes = type.GetCustomAttributes(typeof(ClientStoragePathDescriptionAttribute), false);
                foreach (ClientStoragePathDescriptionAttribute attribute in sourceTypeAttributes)
                {
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
            }
        }

        public string Path { get; set; }
        public bool ShowSelectPathDialog { get; set; }
        public bool ShowSelectFileDialog { get; set; }

        public Credentials LogonCredentials { get; set; }

        [XmlIgnore]
        public ConnectorDescriptionAttribute ConnectorDescription { get; set; }
    }
}