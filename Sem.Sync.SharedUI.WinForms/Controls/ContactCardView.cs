namespace Sem.Sync.SharedUI.WinForms.Controls
{
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.DetailData;

    public partial class ContactCardView : UserControl
    {
        public ContactCardView()
        {
            this.InitializeComponent();
        }

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
                        this.CardImage.Image = new Bitmap(imageStream);
                    }
                }

                this.PrivateAddress.Address = contact.PersonalAddressPrimary;
                this.BusinessAddress.Address = contact.BusinessAddressPrimary;
            }
        }
    }
}