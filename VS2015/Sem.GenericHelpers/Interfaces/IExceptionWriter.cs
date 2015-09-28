// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionWriter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interface to provide writing support for exception - does also include optional read support.
//   In case of <see cref="Read" /> expect a NULL value if read is not supported.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Interfaces
{
    using System.Xml.Linq;

    /// <summary>
    /// Interface to provide writing support for exception - does also include optional read support.
    ///   In case of <see cref="Read"/> expect a NULL value if read is not supported.
    /// </summary>
    public interface IExceptionWriter
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the destination of the writer - in case of a file system writer this might be the 
        ///   path, in case of a database writer this might be the connection string.
        /// </summary>
        string Destination { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs a cleanup of the collected exception information by deleting all of such information.
        ///   Simply do nothing in case of not supporting this operation.
        ///   DO NOT THROW AN EXCEPTION.
        /// </summary>
        void Clean();

        /// <summary>
        /// Reads all collected exception information. 
        ///   Return NULL in case of not supporting this operation.
        ///   DO NOT THROW AN EXCEPTION.
        /// </summary>
        /// <returns>
        /// An <see cref="XElement"/> containing all exception information that should be read.
        /// </returns>
        XElement Read();

        /// <summary>
        /// Writes the exception information to the destination.
        /// </summary>
        /// <param name="information">
        /// The exception information that should be written. 
        /// </param>
        void Write(XElement information);

        #endregion
    }
}