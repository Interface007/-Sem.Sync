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
    
    using Sem.Sync.SharedUI.WinForms.Tools;
    using Sem.Sync.SharedUI.WinForms.UI;

    using Properties;

    public partial class MainForm : Form
    {
        private readonly SyncEngine _engine = new SyncEngine
                                                 {
                                                    WorkingFolder = Path.Combine(
                                                         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                         "SemSyncXing2Outlook"),
                                                    UiProvider = new UiDispatcher()
                                                 };

        public MainForm()
        {
            InitializeComponent();
            new Disclaimer().ShowDialog();
        }

        private void buttonSyncWithXing_Click(object sender, EventArgs e)
        {
            _engine.ProcessingEvent += this.LogThis;
            _engine.ProgressEvent += this.UpdateProgress;
            _engine.QueryForLogOnCredentialsEvent += AskForLogin;

            var list = LoadSyncList("commands.xml");
            this._engine.Execute(list);

            _engine.ProcessingEvent -= this.LogThis;
            _engine.ProgressEvent -= this.UpdateProgress;
            _engine.QueryForLogOnCredentialsEvent -= AskForLogin;

            MessageBox.Show(Resources.messageFinished);
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
            new LogOn().SetLoginCredentials((IClientBase)s, eargs);
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