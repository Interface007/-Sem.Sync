using Sem.Sync.SyncBase.EventArgs;

namespace Sem.Sync.LocalSyncManager.UI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Text;

    using SyncBase.Interfaces;
    using SyncBase;

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
            this.DataContext.ProcessingEvent += LogMessage;
            this.DataContext.ProgressEvent += OnProgressEvent;
            this.DataContext.QueryForLoginCredentials += (s, eargs) => new SharedUI.WinForms.UI.LogInGui().SetLoginCredentials((IClientBase)s, eargs);

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
                    if (item != null) this.DataContext.LoadSyncList(item.ToString());
                    this.syncListBindingSource.DataSource = this.DataContext.SyncCommands;
                };

            // unselect first entry, then select one, if there is one ;-) - this will fire the SelectedValueChanged 
            this.SyncListSelection.SelectedIndex = -1;
            if (this.SyncListSelection.Items.Count > 0)
                this.SyncListSelection.SelectedIndex = 0;
        }

        private void OnProgressEvent(object sender, ProgressEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.PercentageDone;
        }
        private void LogMessage(object sender, ProcessingEventArgs args)
        {
            var logEntry = new StringBuilder();

            if (sender is IClientBase)
            {
                logEntry.Append(((IClientBase)sender).FriendlyClientName);
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
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            RunSelectedCommand(e.RowIndex);
        }

        private void runSelected_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
                RunSelectedCommand(dataGridView1.SelectedRows[0].Index);
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
    }
}