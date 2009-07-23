namespace Sem.Sync.SyncBase.Attributes
{
    using System;

    /// <summary>
    /// Specifies information about the connectors capabilities
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConnectorDescriptionAttribute : Attribute
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool NeedsCredentials { get; set; }

        public ConnectorDescriptionAttribute()
        {
            this.CanRead = true;
            this.CanWrite = true;
            this.NeedsCredentials = false;
        }
    }
}
