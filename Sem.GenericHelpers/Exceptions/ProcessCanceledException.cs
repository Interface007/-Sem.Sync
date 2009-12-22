// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessCanceledException.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ProcessCanceledException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.GenericHelpers.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// signals the cancelation of the process through all layers and components
    /// </summary>
    [Serializable]
    public class ProcessAbortException : TechnicalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAbortException"/> class.
        /// </summary>
        public ProcessAbortException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAbortException"/> class.
        /// </summary>
        /// <param name="message"> The message to be shown for the user. </param>
        public ProcessAbortException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAbortException"/> class.
        /// </summary>
        /// <param name="message"> The message to be shown to the user. </param>
        /// <param name="innerException"> The inner exception to be embedded. </param>
        public ProcessAbortException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAbortException"/> class.
        /// </summary>
        /// <param name="info"> The serialization information to reconstruct the exception. </param>
        /// <param name="context"> The streaming context to reconstruct the exception. </param>
        protected ProcessAbortException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}