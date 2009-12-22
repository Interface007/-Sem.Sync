namespace Sem.Sync.LocalSyncManager.UI
{
    using System.Windows.Forms;

    using Sem.Sync.LocalSyncManager.Business;

    public partial class ViewContacts : Form
    {
        public ContactsFolder ContactsFolder { get; set; }

        public ViewContacts()
        {
            this.InitializeComponent();
        }
    }
}
