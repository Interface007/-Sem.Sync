// --------------------------------------------------------------------------------------------------------------------
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

    public partial class ExceptionOkToSend : Form
    {
        public ExceptionOkToSend()
        {
            InitializeComponent();
        }

        public bool AskForOk(string content)
        {
            this.Content.Text = content;

            return this.ShowDialog() == System.Windows.Forms.DialogResult.Yes;
        }
    }
}
