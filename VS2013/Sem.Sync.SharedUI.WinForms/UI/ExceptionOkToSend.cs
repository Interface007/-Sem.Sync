// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionOkToSend.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Form to ask for sending exception information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System.Windows.Forms;

    /// <summary>
    /// Form to ask for sending exception information.
    /// </summary>
    public partial class ExceptionOkToSend : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ExceptionOkToSend" /> class.
        /// </summary>
        public ExceptionOkToSend()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Asks the user to let the program send exception information.
        /// </summary>
        /// <param name="contentText">
        /// The content to be sent. 
        /// </param>
        /// <returns>
        /// true if the user wants to send the information. 
        /// </returns>
        public bool AskForOk(string contentText)
        {
            this.content.Text = contentText;

            return this.ShowDialog() == System.Windows.Forms.DialogResult.Yes;
        }

        #endregion
    }
}