using System;
using System.Windows.Forms;

namespace Sem.Sync.SharedUI.WinForms.UI
{
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