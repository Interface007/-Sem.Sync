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
        #region Properties

        /// <summary>
        ///   Gets or sets the classification (info, warning, error, critical).
        /// </summary>
        public MessageClassification Classification { get; set; }

        /// <summary>
        ///   Gets or sets the message for the event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///   Gets or sets an ID that identifies this single kind of message.
        /// </summary>
        public int MessageId { get; set; }

        #endregion
    }
}