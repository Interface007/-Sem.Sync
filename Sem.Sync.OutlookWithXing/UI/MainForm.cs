// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   the simple user interface for executing the sync
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OutlookWithXing.UI
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using GenericHelpers;
    using GenericHelpers.EventArgs;
    using Properties;
    
    using SharedUI.WinForms.Tools;
    
    using SyncBase;
    using SyncBase.Binding;

    /// <summary>
    /// the simple user interface for executing the sync
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Instance of the sync engine to work with
        /// </summary>
        private readonly SyncEngine engine = new SyncEngine
                                                 {
                                                    WorkingFolder = Path.Combine(
                                                         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                         "SemSyncXing2Outlook"),
                                                    UiProvider = new UiDispatcher()
                                                 };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// handels the sync button click event
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The empty event args for event handling system. </param>
        private void ButtonSyncWithXing_Click(object sender, EventArgs e)
        {
            this.engine.ProcessingEvent += this.LogThis;
            this.engine.ProgressEvent += this.UpdateProgress;

            var list = SyncCollection.LoadSyncList("commands.xml");
            this.engine.Execute(list);

            this.engine.ProcessingEvent -= this.LogThis;
            this.engine.ProgressEvent -= this.UpdateProgress;

            MessageBox.Show(Resources.messageFinished, this.Text);
        }

        /// <summary>
        /// handels the update of the gui for a progress event
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The <see cref="ProgressEventArgs"/> containing the percentage done. </param>
        private void UpdateProgress(object sender, ProgressEventArgs e)
        {
            this.progressBar.Value = e.PercentageDone;
            this.progressBar.Maximum = 100;
        }

        /// <summary>
        /// handels logging events
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The <see cref="ProcessingEventArgs"/> containing event information about the current processing step. </param>
        private void LogThis(object sender, ProcessingEventArgs e)
        {
            this.listLog.Items.Add(e.Message + ((StdContact)e.Item).NewIfNull().GetFullName());
            this.listLog.TopIndex = this.listLog.Items.Count - 1;
        }
    }
}