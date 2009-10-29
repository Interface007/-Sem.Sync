// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XingContactReference.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   contact references do contain the tags, so we need a class to hold the url to doenload the
//   vCard and the string containing the tags
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Xing
{
    /// <summary>
    /// contact references do contain the tags, so we need a class to hold the url to doenload the
    /// vCard and the string containing the tags
    /// </summary>
    public class XingContactReference
    {
        /// <summary>
        /// Gets or sets Url to download the vCard.
        /// </summary>
        public string vCardUrl { get; set; }

        /// <summary>
        /// Gets or sets Url to download the vCard.
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// Gets or sets a string containing the Tags seperated by a character sequence ", ".
        /// </summary>
        public string Tags { get; set; }
    }
}
