namespace Sem.GenericHelpers
{
    using EventArgs;
    public delegate void ProcessEvent(object entity, ProcessingEventArgs eventArgs);
    public delegate void ProgressEvent(object entity, ProgressEventArgs eventArgs);
    
}
