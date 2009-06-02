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
            SetLoginCredentials(client, arguments.MessageForUser,arguments.LoginUserId,arguments.LoginPassword);
        }

        public bool SetLoginCredentials(IClientBase client, string messageForUser, string logOnUserId, string logOnPassword)
        {
            this.UserMessage.Text = messageForUser;
            this.textBoxUserId.Text = logOnUserId;
            this.textBoxPassword.Text = logOnPassword;

            if (this.ShowDialog() == DialogResult.OK)
            {
                client.LogOnUserId = textBoxUserId.Text;
                client.LogOnPassword = textBoxPassword.Text;
                return true;
            }

            return false;
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