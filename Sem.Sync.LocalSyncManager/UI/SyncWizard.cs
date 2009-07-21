namespace Sem.Sync.LocalSyncManager.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public partial class SyncWizard : Form
    {
        public SyncWizardContext dataContext { get; set; }

        public SyncWizard()
        {
            InitializeComponent();
        }

        private void SyncWizard_Load(object sender, EventArgs e)
        {
            this.dataContext = new SyncWizardContext();

            this.contextDataSource.DataSource = this.dataContext;
            this.contextDataTarget.DataSource = this.dataContext;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.dataContext.Run();
        }

        private void contextDataSource_CurrentChanged(object sender, EventArgs e)
        {
            this.dataContext.Source.Name = ((KeyValuePair<string, string>)((BindingSource)sender).Current).Key;
        }

        private void contextDataTarget_CurrentChanged(object sender, EventArgs e)
        {
            this.dataContext.Source.Name = ((KeyValuePair<string, string>)((BindingSource)sender).Current).Key;
        }
    }

    
}
