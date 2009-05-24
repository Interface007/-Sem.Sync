namespace Sem.Sync.SyncBase.EventArgs
{
    public class QueryForLogOnCredentialsEventArgs : System.EventArgs
    {
        public string MessageForUser { get; set; }
        public string LoginUserId { get; set; }
        public string LoginPassword { get; set; }
    }
}