// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalMessage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   A message about a technical event the client should be aware of
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A message about a technical event the client should be aware of
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class TechnicalMessage
    {
        /// <summary>
        /// describes the meaning of the message.
        /// </summary>
        public enum TechnicalMessageCodes
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

        /// <summary>Gets or sets the message for the event.</summary>
        public string Message { get; set; }
        public int MessageId { get; set; }
        public MessageClassification Classification { get; set; }
    }

    public enum MessageClassification
    {
        Default = 0,

        Warning = 1,
        Error = 2,
        Critical = 3,

    }
}