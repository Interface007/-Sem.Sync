// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryForLogOnCredentialsEventArgs.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Event argument for requesting log on information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.EventArgs
{
    /// <summary>
    /// Event argument for requesting log on information
    /// </summary>
    public class QueryForLogOnCredentialsEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the password to be used while log on
        /// </summary>
        public string LogonPassword { get; set; }

        /// <summary>
        ///   Gets or sets the log on id (e.g. user name) to be used while log on
        /// </summary>
        public string LogonUserId { get; set; }

        /// <summary>
        ///   Gets or sets the message to be shown to the user
        /// </summary>
        public string MessageForUser { get; set; }

        #endregion
    }
}