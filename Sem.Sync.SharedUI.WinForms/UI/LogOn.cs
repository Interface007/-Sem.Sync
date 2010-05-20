// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogOn.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Log on dialog
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Windows.Forms;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.EventArgs;
    using Sem.GenericHelpers.Interfaces;

    /// <summary>
    /// Log on dialog
    /// </summary>
    public partial class LogOn : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LogOn" /> class.
        /// </summary>
        public LogOn()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the dialog to the user and sets the resulting information for the client via the interface <see cref="ICredentialAware"/>.
        /// </summary>
        /// <param name="client">
        /// The client that implements <see cref="ICredentialAware"/> and should get the credentials. 
        /// </param>
        /// <param name="arguments">
        /// The arguments with preselected credentials and a message for the user. 
        /// </param>
        public void SetLogonCredentials(ICredentialAware client, QueryForLogOnCredentialsEventArgs arguments)
        {
            this.SetLogonCredentials(
                new LogonCredentialRequest(client, arguments.MessageForUser, arguments.MessageForUser));
        }

        /// <summary>
        /// Shows the dialog to the user and sets the resulting information for the client via the interface <see cref="ICredentialAware"/>.
        /// </summary>
        /// <param name="request">
        /// an object containing all information to request the credentiols from the user and pass them back to the callee
        /// </param>
        /// <returns>
        /// A value indicating whether the user did press the "ok"-button (true) or the "cancel"-button (false). 
        /// </returns>
        public bool SetLogonCredentials(LogonCredentialRequest request)
        {
            this.UserMessage.Text = request.MessageForUser;
            this.textBoxUserId.Text = request.LogOnCredentials.LogOnUserId;
            this.textBoxPassword.Text = request.LogOnCredentials.LogOnPassword;

            if (this.ShowDialog() == DialogResult.OK)
            {
                request.LogOnCredentials.LogOnUserId = this.textBoxUserId.Text;
                request.LogOnCredentials.LogOnPassword = this.textBoxPassword.Text;
                request.WriteToCacheAllowed = this.chkAllowSaving.Checked;
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cancels the dialog
        /// </summary>
        /// <param name="sender">
        /// The sender button instance. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Accepts the credentials shown in the dialog
        /// </summary>
        /// <param name="sender">
        /// The sender button instance. 
        /// </param>
        /// <param name="e">
        /// The event arguments. 
        /// </param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}