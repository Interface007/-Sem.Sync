namespace Sem.Sync.Cloud
{
    /// <summary>
    /// describes the meaning of the message.
    /// </summary>
    public enum TechnicalMessageCode
    {
        /// <summary>
        /// This is a pure informational message and does not 
        /// report an error or exception.
        /// </summary>
        InformationalMessage = 0,

        /// <summary>
        /// On the server side an exception has been thrown.
        /// No data has been altered while this roundtrip.
        /// </summary>
        ExceptionWhileServerActionNoDataAltered = 1,

        /// <summary>
        /// On the server side an exception has been thrown.
        /// Data has been altered while this roundtrip and needs to be
        /// compensated.
        /// </summary>
        ExceptionWhileServerActionDataHasBeenAltered = 2,
    }
}