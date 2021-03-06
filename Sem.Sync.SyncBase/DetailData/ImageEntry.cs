// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEntry.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Entry of an unspecific image with data and a name
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// Entry of an unspecific image with data and a name
    /// </summary>
    [Serializable]
    public class ImageEntry
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the binary image data.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        ///   Gets or sets the image name.
        /// </summary>
        public string ImageName { get; set; }

        #endregion
    }
}