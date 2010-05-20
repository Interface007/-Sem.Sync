// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Base class for result sets with a list of technical messages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class for result sets with a list of technical messages.
    /// </summary>
    public abstract class ResultBase
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a list of messages to be exchanged in conjunction with the contact item list.
        /// </summary>
        public List<TechnicalMessage> Messages { get; set; }

        #endregion
    }
}