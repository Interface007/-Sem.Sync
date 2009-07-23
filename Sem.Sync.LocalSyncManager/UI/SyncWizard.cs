// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncWizard.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   User interface for wizard-like interaction
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Business;

    using GenericHelpers.EventArgs;

    /// <summary>
    /// User interface for wizard-like interaction
    /// </summary>
    public partial class SyncWizard : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncWizard"/> class.
        /// </summary>
        public SyncWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        public SyncWizardContext DataContext { get; set; }

        /// <summary>
        /// Setup of the event handler routing and "databinding"
        /// </summary>
        /// <param name="sender">the sender of the event - in this case the form instance </param>
        /// <param name="e"> empty event arguments </param>
        private void SyncWizard_Load(object sender, EventArgs e)
        {
            this.DataContext = new SyncWizardContext();

            this.contextDataSource.DataSource = this.DataContext;
            this.contextDataTarget.DataMember = "Clients";
            this.contextDataSource.CurrentChanged += (s, ev) =>
                {
                    this.DataContext.Source.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key;
                    this.ReadFromContext();
                };

            this.contextDataTarget.DataSource = this.DataContext;
            this.contextDataSource.DataMember = "Clients";
            this.contextDataTarget.CurrentChanged += (s, ev) =>
                {
                    this.DataContext.Target.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key;
                    this.ReadFromContext();
                };

            this.btnCancel.Click += (s, ev) => this.Close();
            this.btnRun.Click += (s, ev) => this.RunCommands();
            this.btnPathSource.Click += (s, ev) => this.ShowFolderDialog(this.txtPathSource, this.DataContext.Source.ShowSelectFileDialog, false);
            this.btnPathTarget.Click += (s, ev) => this.ShowFolderDialog(this.txtPathTarget, this.DataContext.Target.ShowSelectFileDialog, true);

            this.btnLoad.Click += (s, ev) => { this.DataContext.LoadFrom("wizard.xml"); this.ReadFromContext(); };
            this.btnSave.Click += (s, ev) => this.DataContext.SaveTo("wizard.xml");

            this.txtPathSource.TextChanged += (s, ev) => { this.DataContext.Source.Path = ((TextBox)s).Text; };
            this.txtPathTarget.TextChanged += (s, ev) => { this.DataContext.Target.Path = ((TextBox)s).Text; };

            this.txtUidSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnUserId = ((TextBox)s).Text; };
            this.txtUidTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnUserId = ((TextBox)s).Text; };
            this.txtPasswordSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnPassword = ((TextBox)s).Text; };
            this.txtPasswordTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnPassword = ((TextBox)s).Text; };
            this.txtDomainSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnDomain = ((TextBox)s).Text; };
            this.txtDomainTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnDomain = ((TextBox)s).Text; };
        }

        private void RunCommands()
        {
            this.pnlProgress.Visible = true;
            this.DataContext.Run(
                "SyncLists\\Wizard.XSyncList",
                delegate(object entity, ProcessingEventArgs eventArgs)
                { this.lblProgressStatus.Text = eventArgs.Message; });
            this.pnlProgress.Visible = false;
            this.lblDialogStatus.Text = "process finished";
        }

        private void ReadFromContext()
        {
            this.txtPathSource.Text = this.DataContext.Source.Path;
            this.txtPathTarget.Text = this.DataContext.Target.Path;

            this.txtUidSource.Text = this.DataContext.Source.LogonCredentials.LogOnUserId;
            this.txtUidTarget.Text = this.DataContext.Target.LogonCredentials.LogOnUserId;
            this.txtPasswordSource.Text = this.DataContext.Source.LogonCredentials.LogOnPassword;
            this.txtPasswordTarget.Text = this.DataContext.Target.LogonCredentials.LogOnPassword;
            this.txtDomainSource.Text = this.DataContext.Source.LogonCredentials.LogOnDomain;
            this.txtDomainTarget.Text = this.DataContext.Target.LogonCredentials.LogOnDomain;

            this.cboSource.Text = this.DataContext.Source.Name;
            this.cboTarget.Text = this.DataContext.Target.Name;

            this.btnPathSource.Visible = this.DataContext.Source.ShowSelectPathDialog || this.DataContext.Source.ShowSelectFileDialog;
            this.btnPathTarget.Visible = this.DataContext.Target.ShowSelectPathDialog || this.DataContext.Target.ShowSelectFileDialog;
        }

        /// <summary>
        /// Show the folder browse dialog for a specified textbox
        /// </summary>
        /// <param name="textBox"> The text box that should be updated with the path. </param>
        /// <param name="useFileDialog"> Indicates whether the file dialog should be shown instead of the path selection. </param>
        /// <param name="useSave"> Indicates whether the dialog should ask the user to save instead of to load data. </param>
        private void ShowFolderDialog(Control textBox, bool useFileDialog, bool useSave)
        {
            if (!useFileDialog)
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = folderBrowser.SelectedPath;
                }
                return;
            }

            if (useSave)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = saveFileDialog1.FileName;
                }
                return;
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = openFileDialog1.FileName;
            }
        }
    }
}