namespace Sem.Sync.SyncBase.EventArgs
{
    public class ProcessingEventArgs : System.EventArgs
    {
        public object Item { get; set; }
        public string Message { get; set; }
    }
}