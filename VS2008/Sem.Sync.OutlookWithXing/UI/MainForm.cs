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
    using GenericHelpers.Exceptions;

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
        /// Instance of the sync engine to work with. In this case we do use
        /// and instance with a default winforms <see cref="UiDispatcher"/> 
        /// and provide a working folder that points to the application data
        /// folder. Assuming Windows 7 as an OS this will be:
        /// C:\Users\{username}\AppData\Roaming\SemSyncXing2Outlook
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
            this.InitializeComponent();
            this.DoubleClick += (sender, e) => System.Diagnostics.Process.Start(ExceptionHandler.ExceptionWriter[0].Destination);
            this.versionLabel.Text = "Version " + new VersionCheck().ToString(false);
        }

        /// <summary>
        /// handels the sync button click event
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The empty event args for event handling system. </param>
        private void ButtonSyncWithXing_Click(object sender, EventArgs e)
        {
            // using a lambda here is much less code
            EventHandler<ProgressEventArgs> progress = (s, evnt) => this.progressBar.Value = evnt.PercentageDone; 
            
            // first attach to the events to update the progress bar and the
            // log list for the user.
            this.engine.ProcessingEvent += this.LogThis;
            this.engine.ProgressEvent += progress;

            // load a SyncCollection from the xml file (this can be 
            // modified in order to change behavior)
            var list = SyncCollection.LoadSyncList("commands.xml");

            // now execute the list of commands
            this.engine.Execute(list);

            // detach from the events - there's nothing we should get from there
            // any more.
            this.engine.ProcessingEvent -= this.LogThis;
            this.engine.ProgressEvent -= progress;

            // tell the user we have finished
            MessageBox.Show(Resources.messageFinished, this.Text);
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