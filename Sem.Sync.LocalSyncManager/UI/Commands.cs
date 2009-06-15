// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Commands.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the LocalSync type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.LocalSyncManager.UI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using SharedUI.WinForms.UI;

    using SyncBase;
    using SyncBase.EventArgs;
    using SyncBase.Interfaces;

    public partial class LocalSync : Form
    {
        internal ClientViewModel DataContext { get; set; }

        public LocalSync()
        {
            InitializeComponent();
        }

        private void LocalSync_Load(object sender, EventArgs e)
        {
            // todo: this should be handled by the viewmodel instead mof the code behind

            // route the events
            this.DataContext.ProcessingEvent += this.LogMessage;
            this.DataContext.ProgressEvent += this.OnProgressEvent;
            this.DataContext.QueryForLogOnCredentials += (s, eargs) => new LogOn().SetLoginCredentials((IClientBase)s, eargs);

            // get the data for the combo box from the file system paths
            this.SyncListSelection.DataSource =
               (from x in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.SyncList")
                select new
                           {
                               Name = Path.GetFileName(x),
                               Path = x
                           }).ToList();

            // display file name only, but store full path
            this.SyncListSelection.ValueMember = "Path";
            this.SyncListSelection.DisplayMember = "Name";

            // add event handler that will use the >>value<< instead of the text
            // to load the grid
            this.SyncListSelection.SelectedValueChanged +=
                (cbs, cbe) =>
                {
                    var item = ((ComboBox)cbs).SelectedValue;
                    if (item != null)
                    {
                        this.DataContext.LoadSyncList(item.ToString());
                    }

                    this.syncListBindingSource.DataSource = this.DataContext.SyncCommands;
                };

            // unselect first entry, then select one, if there is one ;-) - this will fire the SelectedValueChanged 
            this.SyncListSelection.SelectedIndex = -1;
            if (this.SyncListSelection.Items.Count > 0)
            {
                this.SyncListSelection.SelectedIndex = 0;
            }

            this.dataGridView1.KeyDown += (kps, kpe) => { if (kpe.KeyCode == Keys.Enter) this.RunSelectedRow(); };
            this.dataGridView1.CellDoubleClick += (gvs, gve) => this.RunSelectedCommand(gve.RowIndex);
        }

        private void OnProgressEvent(object sender, ProgressEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.PercentageDone;
        }
        private void LogMessage(object sender, ProcessingEventArgs args)
        {
            var logEntry = new StringBuilder();

            var client = sender as IClientBase;
            if (client != null)
            {
                logEntry.Append(client.FriendlyClientName);
                logEntry.Append(": ");
            }

            logEntry.Append(args.Message);
            if (args.Item != null)
            {
                logEntry.Append(" (");
                logEntry.Append(args.Item.ToString());
                logEntry.Append(")");
            }

            StatusLabel.Text = args.Message;
            LogList.Items.Add(logEntry.ToString());
            LogList.TopIndex = LogList.Items.Count - 1;
        }

        #region eventhandler
        private void runSelected_Click(object sender, EventArgs e)
        {
            this.RunSelectedRow();
        }

        private void RunSelectedRow()
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.RunSelectedCommand(this.dataGridView1.SelectedRows[0].Index);
            }
        }

        private void runAll_Click(object sender, EventArgs e)
        {
            this.DataContext.Execute();
        }

        private void RunSelectedCommand(int gridRowIndex)
        {
            this.DataContext.Execute((SyncDescription)dataGridView1.Rows[gridRowIndex].DataBoundItem);
        }
        #endregion

        public bool AskForLogOnCredentials(ICredentialAware client, string messageForUser, string logOnUserId, string logOnPassword)
        {
            return new LogOn().SetLoginCredentials(client, messageForUser, logOnUserId, logOnPassword);
        }

        public bool AskForConfirm(string messageForUser, string title)
        {
            return MessageBox.Show(messageForUser, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }
    }
}