﻿using Sem.Sync.SharedUI.WinForms.UI;

namespace SemSyncOutlookWithXing.UI
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Binding;
    using Sem.Sync.SyncBase.EventArgs;
    using Sem.Sync.SyncBase.Interfaces;

    public partial class MainForm : Form
    {
        private readonly SyncEngine engine = new SyncEngine
                                                 {
                                                     WorkingFolder = Path.Combine(
                                                         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                         "SemSyncXing2Outlook")
                                                 };

        public MainForm()
        {
            InitializeComponent();
            new Disclaimer().ShowDialog();
        }

        private void buttonSyncWithXing_Click(object sender, EventArgs e)
        {
            engine.ProcessingEvent += this.LogThis;
            engine.ProgressEvent += this.UpdateProgress;
            engine.QueryForLogOnCredentialsEvent += AskForLogin;

            var list = LoadSyncList("commands.xml");
            this.engine.ConflictSolver = new MergeWindow();
            this.engine.Execute(list);

            engine.ProcessingEvent -= this.LogThis;
            engine.ProgressEvent -= this.UpdateProgress;
            engine.QueryForLogOnCredentialsEvent -= AskForLogin;
        }

        private void UpdateProgress(object sender, ProgressEventArgs e)
        {
            this.progressBar.Value = e.PercentageDone;
            this.progressBar.Maximum = 100;
        }

        private void LogThis(object sender, ProcessingEventArgs e)
        {
            this.listLog.Items.Add(e.Message + (e.Item != null ? ((StdContact)e.Item).GetFullName() : ""));
            this.listLog.TopIndex = this.listLog.Items.Count - 1;
        }

        private static void AskForLogin(object s, QueryForLogOnCredentialsEventArgs eargs)
        {
            new LogInGui().SetLoginCredentials((IClientBase)s, eargs);
        }

        internal SyncCollection LoadSyncList(string p)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));
            using (var file = new FileStream(p, FileMode.Open))
            {
                return (SyncCollection)formatter.Deserialize(file);
            }
        }
    }
}