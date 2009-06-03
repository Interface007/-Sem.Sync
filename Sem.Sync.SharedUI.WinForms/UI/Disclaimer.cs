// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disclaimer.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Disclaimer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SharedUI.WinForms.UI
{
    using System;
    using System.Windows.Forms;

    public partial class Disclaimer : Form
    {
        public Disclaimer()
        {
            InitializeComponent();
        }

        private void no_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void yes_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iDoUnterstand_CheckedChanged(object sender, EventArgs e)
        {
            yes.Enabled = iDoUnterstand.Checked;
        }
    }
}