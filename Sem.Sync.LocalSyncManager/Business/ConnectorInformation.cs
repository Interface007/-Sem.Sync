namespace Sem.Sync.LocalSyncManager
{
    using System;

    using GenericHelpers;
    using SyncBase.Attributes;

    public class ConnectorInformation
    {
        private string _name;
        private readonly Factory _factory = new Factory("Sem.Sync.SyncBase");
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

                var type = Type.GetType(_factory.EnrichClassName(typeName));
                var sourceTypeAttributes = type.GetCustomAttributes(typeof(ClientStoragePathDescriptionAttribute), false);
                if (sourceTypeAttributes != null && sourceTypeAttributes.Length > 0)
                {
                    var attribute = (ClientStoragePathDescriptionAttribute)sourceTypeAttributes[0];
                    this.ShowSelectFileDialog = attribute.ReferenceType == ClientPathType.FileSystemFileNameAndPath;
                    this.ShowSelectPathDialog = attribute.ReferenceType == ClientPathType.FileSystemPath;
                    if (string.IsNullOrEmpty(this.Path))
                    {
                        this.Path = attribute.Default;
                    }
                }

            }
        }

        public string Path { get; set; }
        public bool ShowSelectPathDialog { get; set; }
        public bool ShowSelectFileDialog { get; set; }

    }
}