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
    using System.Xml.Serialization;

    using GenericHelpers.EventArgs;
    using Properties;
    
    using SharedUI.WinForms.Tools;
    using SharedUI.WinForms.UI;
    
    using SyncBase;
    using SyncBase.Binding;
    using SyncBase.Interfaces;

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
            new Disclaimer().ShowDialog();
        }

        /// <summary>
        /// handels the event of needed log on credentials
        /// </summary>
        /// <param name="s"> The sender of this event. </param>
        /// <param name="eargs"> The <see cref="QueryForLogOnCredentialsEventArgs"/> containing the message and preset user information. </param>
        private static void AskForLogon(object s, QueryForLogOnCredentialsEventArgs eargs)
        {
            new LogOn().SetLogonCredentials((IClientBase)s, eargs);
        }

        /// <summary>
        /// Loads the commands to be executed from a file
        /// </summary>
        /// <param name="p"> The path of the file to load. </param>
        /// <returns> a collection of <see cref="SyncDescription"/> containing the commands to execute </returns>
        private static SyncCollection LoadSyncList(string p)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));

            // opening readonly will enable using a config file in the programs folder
            using (var file = new FileStream(p, FileMode.Open, FileAccess.Read))
            {
                return (SyncCollection)formatter.Deserialize(file);
            }
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
            this.engine.QueryForLogOnCredentialsEvent += AskForLogon;

            var list = LoadSyncList("commands.xml");
            this.engine.Execute(list);

            this.engine.ProcessingEvent -= this.LogThis;
            this.engine.ProgressEvent -= this.UpdateProgress;
            this.engine.QueryForLogOnCredentialsEvent -= AskForLogon;

            MessageBox.Show(Resources.messageFinished);
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
            this.listLog.Items.Add(e.Message + (e.Item != null ? ((StdContact)e.Item).GetFullName() : string.Empty));
            this.listLog.TopIndex = this.listLog.Items.Count - 1;
        }
    }
}