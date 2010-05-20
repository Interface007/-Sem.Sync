// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressEventArgs.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Event argument for the progress of an action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.EventArgs
{
    /// <summary>
    /// Event argument for the progress of an action.
    /// </summary>
    public class ProgressEventArgs : System.EventArgs
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the percentage of work being done.
        /// </summary>
        public int PercentageDone { get; set; }

        #endregion
    }
}