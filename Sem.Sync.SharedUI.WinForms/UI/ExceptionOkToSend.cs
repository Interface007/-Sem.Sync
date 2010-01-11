﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionOkToSend.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ExceptionOkToSend type.
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionOkToSend"/> class.
        /// </summary>
        public ExceptionOkToSend()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Asks the user to let the program send exception information.
        /// </summary>
        /// <param name="content"> The content to be sent. </param>
        /// <returns> true if the user wants to send the information. </returns>
        public bool AskForOk(string content)
        {
            this.Content.Text = content;

            return this.ShowDialog() == System.Windows.Forms.DialogResult.Yes;
        }
    }
}
