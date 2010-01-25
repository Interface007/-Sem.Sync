// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ImageEntry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Entry of an unspecific image with data and a name
    /// </summary>
    public class ImageEntry
    {
        /// <summary>
        /// Gets or sets the image name.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the binary image data.
        /// </summary>
        public byte[] ImageData { get; set; }
    }
}