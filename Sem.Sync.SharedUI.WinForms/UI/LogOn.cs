// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogOn.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the LogOn type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System.Windows.Forms;

    using SyncBase.EventArgs;
    using SyncBase.Interfaces;

    public partial class LogOn : Form
    {
        public LogOn()
        {
            InitializeComponent();
        }

        public void SetLoginCredentials(IClientBase client, QueryForLogOnCredentialsEventArgs arguments)
        {
            this.SetLoginCredentials(client, arguments.MessageForUser, arguments.LoginUserId, arguments.LoginPassword);
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