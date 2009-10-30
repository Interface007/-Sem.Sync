namespace Sem.Sync.LocalSyncManager.Business
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TechnicalException : Exception
    {
        public TechnicalException()
        {
        }

        public TechnicalException(string message)
            : base(message)
        {
        }

        public TechnicalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TechnicalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}