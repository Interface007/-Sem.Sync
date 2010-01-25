// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewContact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ViewContact type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System.Windows.Media.Imaging;

    public class ViewContact
    {
        public string FullName { get; set; }

        public BitmapImage Picture { get; set; }

        public string Street { get; set; }

        public string City { get; set; }
    }
}