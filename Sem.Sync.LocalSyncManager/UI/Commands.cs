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

    /// <summary>
    /// the main for of the application
    /// </summary>
    public partial class Commands : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Commands"/> class.
        /// </summary>
        public Commands()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the DataContext for binding and working with the commands.
        /// </summary>
        internal ClientViewModel DataContext { get; set; }

        /// <summary>
        /// performs the setup of the form after loading the controls
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The empty event arguments. </param>
        private void LocalSync_Load(object sender, EventArgs e)
        {
            // todo: this should be handled by the viewmodel instead of the code behind

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

            this.dataGridView1.KeyDown += (kps, kpe) =>
                {
                    if (kpe.KeyCode == Keys.Enter)
                    {
                        this.RunSelectedRow();
                    }
                };

            this.dataGridView1.CellDoubleClick += (gvs, gve) => this.RunRowCommand(gve.RowIndex);
        }

        /// <summary>
        /// Event handler for the progress event
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments with the information about the progress. </param>
        private void OnProgressEvent(object sender, ProgressEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.PercentageDone;
        }

        /// <summary>
        /// Event handler for the log event
        /// </summary>
        /// <param name="sender">the instance that requests a logging action</param>
        /// <param name="args">the event arguments containing the information to be logged</param>
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

        /// <summary>
        /// Handels the click event of the "run selected" button
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The empty event arguments. </param>
        private void RunSelected_Click(object sender, EventArgs e)
        {
            this.RunSelectedRow();
        }

        /// <summary>
        /// Handels the click event of the "run all" button
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The empty event arguments. </param>
        private void RunAll_Click(object sender, EventArgs e)
        {
            this.DataContext.Execute();
        }

        /// <summary>
        /// runs the currently selected command
        /// </summary>
        private void RunSelectedRow()
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                this.RunRowCommand(this.dataGridView1.SelectedRows[0].Index);
            }
        }

        /// <summary>
        /// Runs the command by specifying the row index
        /// </summary>
        /// <param name="gridRowIndex">the number of the row</param>
        private void RunRowCommand(int gridRowIndex)
        {
            this.DataContext.Execute((SyncDescription)dataGridView1.Rows[gridRowIndex].DataBoundItem);
        }
        #endregion
    }
}