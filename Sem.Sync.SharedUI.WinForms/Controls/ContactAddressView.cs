namespace Sem.Sync.SharedUI.WinForms.Controls
{
    using System;
    using System.Windows.Forms;

    using SyncBase.DetailData;

    public partial class ContactAddressView : UserControl
    {
        public ContactAddressView()
        {
            InitializeComponent();
        }

        public AddressDetail Address
        {
            set
            {
                var address = value ?? new AddressDetail();
                
                this.StreetName.Text = address.StreetName;
                this.CityName.Text = address.CityName;
                this.PostalCode.Text = address.PostalCode;
                this.Room.Text = address.Room;

                this.StateName.Text = address.StateName + 
                    (string.IsNullOrEmpty(address.CountryName)
                    ?string.Empty
                    : " / " + address.CountryName);

                this.Phone.Text = (address.Phone ?? new PhoneNumber()).ToString();
            }
        }

        private void ContactAddressView_Load(object sender, EventArgs e)
        {

        }
    }
}
