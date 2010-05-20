// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewContact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The view contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The view contact.
    /// </summary>
    public class ViewContact
    {
        #region Properties

        /// <summary>
        /// Gets or sets City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Picture.
        /// </summary>
        public BitmapImage Picture { get; set; }

        /// <summary>
        /// Gets or sets Street.
        /// </summary>
        public string Street { get; set; }

        #endregion
    }
}