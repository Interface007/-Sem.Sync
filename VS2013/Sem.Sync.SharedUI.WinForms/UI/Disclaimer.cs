// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disclaimer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The disclaimer provides a warning to the end user that using this application
//   will alter data and the end user is responsible for proper backup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The disclaimer provides a warning to the end user that using this application
    ///   will alter data and the end user is responsible for proper backup.
    /// </summary>
    public partial class Disclaimer : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Disclaimer" /> class that 
        ///   represents a warning the user has to accept.
        /// </summary>
        public Disclaimer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for the click event of the "I understand"-checkbox
        /// </summary>
        /// <param name="sender">
        /// The "I understand"-checkbox. 
        /// </param>
        /// <param name="e">
        /// empty event-args - checkbox CheckedChanged events do not have arguments. 
        /// </param>
        private void IDoUnterstand_CheckedChanged(object sender, EventArgs e)
        {
            this.yes.Enabled = this.iDoUnterstand.Checked;
        }

        /// <summary>
        /// Event handler for the click event of the "NO"-button
        /// </summary>
        /// <param name="sender">
        /// The No-button. 
        /// </param>
        /// <param name="e">
        /// empty event-args - button click events do not have arguments. 
        /// </param>
        private void No_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Event handler for the click event of the "YES"-button
        /// </summary>
        /// <param name="sender">
        /// The Yes-button. 
        /// </param>
        /// <param name="e">
        /// empty event-args - button click events do not have arguments. 
        /// </param>
        private void Yes_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}