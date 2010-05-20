// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalException.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Technical exception class. This class provides information for the exception handling class <see cref="ExceptionHandler" />
//   including access to entities that might help to reproduce and fix the problem. Always add the causing exception as
//   the inner exception in order to provide information about the root cause of this exception. Keep in mind that
//   <see cref="RelatedEntities" /> will be serialized and may contain business information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Technical exception class. This class provides information for the exception handling class <see cref="ExceptionHandler"/>
    ///   including access to entities that might help to reproduce and fix the problem. Always add the causing exception as
    ///   the inner exception in order to provide information about the root cause of this exception. Keep in mind that 
    ///   <see cref="RelatedEntities"/> will be serialized and may contain business information.
    /// </summary>
    [Serializable]
    public class TechnicalException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TechnicalException" /> class.
        /// </summary>
        public TechnicalException()
        {
            this.RelatedEntities = new List<KeyValuePair<string, object>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message. 
        /// </param>
        public TechnicalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message. 
        /// </param>
        /// <param name="innerException">
        /// The inner exception. 
        /// </param>
        public TechnicalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message of the exception. 
        /// </param>
        /// <param name="innerException">
        /// The inner (causing) exception. 
        /// </param>
        /// <param name="relatedEntities">
        /// The related entities - add entitries to the exception that might help reproduce the exception. 
        /// </param>
        public TechnicalException(
            string message, Exception innerException, params KeyValuePair<string, object>[] relatedEntities)
            : base(message, innerException)
        {
            this.RelatedEntities = new List<KeyValuePair<string, object>>(relatedEntities);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalException"/> class.
        /// </summary>
        /// <param name="info">
        /// The serialization info. 
        /// </param>
        /// <param name="context">
        /// The streaming context. 
        /// </param>
        protected TechnicalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets exception related entities that should be logged.
        /// </summary>
        public List<KeyValuePair<string, object>> RelatedEntities { get; private set; }

        #endregion
    }
}