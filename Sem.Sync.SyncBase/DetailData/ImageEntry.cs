// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ImageEntry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    public class ImageEntry
    {
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }
    }
}