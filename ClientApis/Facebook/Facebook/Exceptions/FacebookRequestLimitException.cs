using System;
using System.Runtime.Serialization;

namespace Facebook.Exceptions
{
    /// <summary>
    /// Exception returned for ERRORNO 4
    /// </summary>
    [Serializable]
    public class FacebookRequestLimitException : FacebookException
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public FacebookRequestLimitException()
            : base()
        { }

        /// <summary>
        /// Constructor with Error Message.
        /// </summary>
        public FacebookRequestLimitException(string message)
            : base(message)
        { }

        /// <summary>
        /// Exception constructor with a custom message after catching an exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Exception caught.</param>
        public FacebookRequestLimitException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Constructor used for serialization.
        /// </summary>
        /// <param name="si">The info.</param>
        /// <param name="sc">The context.</param>
        protected FacebookRequestLimitException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        { }
    }
}
