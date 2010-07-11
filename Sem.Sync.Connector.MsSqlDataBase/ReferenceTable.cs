namespace Sem.Sync.Connector.MsSqlDatabase
{
    public class ReferenceTable
    {
        public int Source { get; set; }
        public int Target { get; set; }
    }
    
    public class ReferenceTableString
    {
        public int Source { get; set; }
        public string Target { get; set; }
    }
}