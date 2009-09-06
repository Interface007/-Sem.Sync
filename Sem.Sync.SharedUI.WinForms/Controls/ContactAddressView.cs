// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactAddressView.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Control that displays a contact address.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.Controls
{
    using System.Windows.Forms;

    using SyncBase.DetailData;

    /// <summary>
    /// Control that displays a contact address.
    /// </summary>
    public partial class ContactAddressView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressView"/> class.
        /// </summary>
        public ContactAddressView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the address information to display for the control
        /// </summary>
        public AddressDetail Address
        {
            set
            {
                var address = value ?? new AddressDetail();
                
                this.StreetName.Text = address.StreetName;
                this.CityName.Text = address.CityName;
                this.PostalCode.Text = address.PostalCode;
                this.Room.Text = address.Room;

                this.StateName.Text = address.StateName ?? string.Empty;
                
                this.StateName.Text +=
                    !string.IsNullOrEmpty(address.CountryName) && !string.IsNullOrEmpty(address.StateName)
                    ? " / "
                    : string.Empty;

                this.StateName.Text += address.CountryName ?? string.Empty;

                this.Phone.Text = (address.Phone ?? new PhoneNumber()).ToString();
            }
        }
    }
}
