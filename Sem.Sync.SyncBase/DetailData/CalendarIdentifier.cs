namespace Sem.Sync.SyncBase.DetailData
{
    public enum CalendarIdentifierType
    {
        Default = 0,
        Outlook,
        Google,
    }
    
    public class CalendarIdentifier
    {
        public CalendarIdentifierType IdentifierType { get; set; }
        public string Identifier { get; set; }
    }
}
