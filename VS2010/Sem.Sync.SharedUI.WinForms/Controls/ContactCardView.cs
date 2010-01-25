// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactCardView.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   defines a user control representing a contact entry as a business card
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.Controls
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.DetailData;

    /// <summary>
    /// defines a user control representing a contact entry as a business card
    /// </summary>
    public partial class ContactCardView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactCardView"/> class.
        /// </summary>
        public ContactCardView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the contact information to be shown
        /// </summary>
        public StdContact Contact
        {
            set
            {
                var contact = value ?? new StdContact();

                this.FullName.Text = (contact.Name ?? new PersonName()).ToString();
                this.BusinessPosition.Text = contact.BusinessPosition;

                this.CardImage.Image = null;

                if (contact.PictureData != null && contact.PictureData.Length > 0)
                {
                    using (var imageStream = new MemoryStream(contact.PictureData))
                    {
                        try
                        {
                            this.CardImage.Image = new Bitmap(imageStream);
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                }

                this.PrivateAddress.Address = contact.PersonalAddressPrimary;
                this.BusinessAddress.Address = contact.BusinessAddressPrimary;
            }
        }
    }
}