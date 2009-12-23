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

    using GenericHelpers.EventArgs;
    using GenericHelpers.Interfaces;

    /// <summary>
    /// Log on dialog
    /// </summary>
    public partial class LogOn : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogOn"/> class.
        /// </summary>
        public LogOn()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the dialog to the user and sets the resulting information for the client via the interface <see cref="ICredentialAware"/>.
        /// </summary>
        /// <param name="client"> The client that implements <see cref="ICredentialAware"/> and should get the credentials. </param>
        /// <param name="arguments"> The arguments with preselected credentials and a message for the user. </param>
        public void SetLogonCredentials(ICredentialAware client, QueryForLogOnCredentialsEventArgs arguments)
        {
            this.SetLogonCredentials(client, arguments.MessageForUser, arguments.LogonUserId, arguments.LogonPassword);
        }

        /// <summary>
        /// Shows the dialog to the user and sets the resulting information for the client via the interface <see cref="ICredentialAware"/>.
        /// </summary>
        /// <param name="client"> The client that implements <see cref="ICredentialAware"/> and should get the credentials.   </param>
        /// <param name="messageForUser"> The message for the user to know what credentials to enter.  </param>
        /// <param name="logOnUserId"> The preselected user id part of the credentials.  </param>
        /// <param name="logOnPassword"> The preselected password part of the credentials. </param>
        /// <returns> A value indicating whether the user did press the "ok"-button (true) or the "cancel"-button (false). </returns>
        public bool SetLogonCredentials(ICredentialAware client, string messageForUser, string logOnUserId, string logOnPassword)
        {
            this.UserMessage.Text = messageForUser;
            this.textBoxUserId.Text = logOnUserId;
            this.textBoxPassword.Text = logOnPassword;

            if (this.ShowDialog() == DialogResult.OK)
            {
                client.LogOnUserId = this.textBoxUserId.Text;
                client.LogOnPassword = this.textBoxPassword.Text;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Accepts the credentials shown in the dialog
        /// </summary>
        /// <param name="sender"> The sender button instance. </param>
        /// <param name="e"> The event arguments. </param>
        private void OkButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancels the dialog
        /// </summary>
        /// <param name="sender"> The sender button instance. </param>
        /// <param name="e"> The event arguments. </param>
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}