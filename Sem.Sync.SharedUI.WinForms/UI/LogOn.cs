using System.Windows.Forms;
using Sem.Sync.SyncBase.EventArgs;
using Sem.Sync.SyncBase.Interfaces;

namespace Sem.Sync.SharedUI.WinForms.UI
{
    public partial class LogOn : Form
    {
        public LogOn()
        {
            InitializeComponent();
        }

        public void SetLoginCredentials(IClientBase client, QueryForLogOnCredentialsEventArgs arguments)
        {
            this.UserMessage.Text = arguments.MessageForUser;
            this.textBoxUserId.Text = arguments.LoginUserId;
            this.textBoxPassword.Text = arguments.LoginPassword;

            if (this.ShowDialog() == DialogResult.OK)
            {
                client.LogOnUserId = textBoxUserId.Text;
                client.LogOnPassword = textBoxPassword.Text;
            }
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}