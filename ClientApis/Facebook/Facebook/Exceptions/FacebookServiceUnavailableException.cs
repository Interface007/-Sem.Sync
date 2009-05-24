using System;
using System.Runtime.Serialization;

namespace Facebook.Exceptions
{
    /// <summary>
    /// Exception returned for ERRORNO 2
    /// </summary>
    [Serializable]
    public class FacebookServiceUnavailableException : FacebookException
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public FacebookServiceUnavailableException()
            : base()
        { }

        /// <summary>
        /// Constructor with Error Message.
        /// </summary>
        public FacebookServiceUnavailableException(string message)
            : base(message)
        { }

        /// <summary>
        /// Exception constructor with a custom message after catching an exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Exception caught.</param>
        public FacebookServiceUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Constructor used for serialization.
        /// </summary>
        /// <param name="si">The info.</param>
        /// <param name="sc">The context.</param>
        protected FacebookServiceUnavailableException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        { }
    }
}
