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
    using System.ComponentModel;
    using System.Windows.Forms;

    using Business;

    using GenericHelpers.EventArgs;

    using SyncBase;

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
            this.DataContext.PropertyChanged += this.ReadFromContext;

            this.contextDataSource.DataSource = this.DataContext;
            this.contextDataSource.DataMember = "ClientsSource";
            this.contextDataSource.CurrentChanged += (s, ev) => this.DataContext.Source.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key;

            this.contextDataTarget.DataSource = this.DataContext;
            this.contextDataTarget.DataMember = "ClientsTarget";
            this.contextDataTarget.CurrentChanged += (s, ev) => this.DataContext.Target.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key;

            this.cboSource.DisplayMember = "Value";
            this.cboSource.ValueMember = "Key";
            this.cboTarget.DisplayMember = "Value";
            this.cboTarget.ValueMember = "Key";

            this.btnCancel.Click += (s, ev) => this.Close();
            this.btnRun.Click += (s, ev) => this.RunCommands();
            this.btnPathSource.Click += (s, ev) => this.ShowFolderDialog(this.txtPathSource, this.DataContext.Source.ShowSelectFileDialog, false);
            this.btnPathTarget.Click += (s, ev) => this.ShowFolderDialog(this.txtPathTarget, this.DataContext.Target.ShowSelectFileDialog, true);

            this.btnLoad.Click += (s, ev) => this.DataContext.LoadFrom("wizard.xml");
            this.btnSave.Click += (s, ev) => this.DataContext.SaveTo("wizard.xml", "test");

            this.txtPathSource.TextChanged += (s, ev) => { this.DataContext.Source.Path = ((Control)s).Text; };
            this.txtPathTarget.TextChanged += (s, ev) => { this.DataContext.Target.Path = ((Control)s).Text; };

            this.txtUidSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnUserId = ((Control)s).Text; };
            this.txtUidTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnUserId = ((Control)s).Text; };
            this.txtPasswordSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnPassword = ((Control)s).Text; };
            this.txtPasswordTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnPassword = ((Control)s).Text; };
            this.txtDomainSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnDomain = ((Control)s).Text; };
            this.txtDomainTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnDomain = ((Control)s).Text; };

            this.cboSource.SelectedIndex = 0;
            this.cboTarget.SelectedIndex = 0;
        }

        /// <summary>
        /// Runs the currently command sequence stored inside "SyncLists\\Wizard.XSyncList" with the context
        /// (the context does contain source and target of the data with additional information like credentials)
        /// </summary>
        private void RunCommands()
        {
            this.pnlProgress.Visible = true;
            this.DataContext.Run(
                "SyncLists\\Syncronize.XSyncList",
                delegate(object entity, ProcessingEventArgs eventArgs)
                    {
                        this.lblProgressStatus.Text = eventArgs.Message +
                                                      (entity as StdContact == null ? string.Empty : entity.ToString());
                        this.lblProgressStatus.Refresh();
                    });
            this.pnlProgress.Visible = false;
            this.lblDialogStatus.Text = "process finished";
        }

        /// <summary>
        /// In this method all properties are read from the context and pumped into the GUI 
        /// elements attributes.
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The change event parameter. </param>
        private void ReadFromContext(object sender, PropertyChangedEventArgs e)
        {
            this.txtPathSource.Text = this.DataContext.Source.Path;
            this.txtPathTarget.Text = this.DataContext.Target.Path;

            this.txtUidSource.Text = this.DataContext.Source.LogonCredentials.LogOnUserId;
            this.txtPasswordSource.Text = this.DataContext.Source.LogonCredentials.LogOnPassword;
            this.txtDomainSource.Text = this.DataContext.Source.LogonCredentials.LogOnDomain;

            this.txtPasswordTarget.Text = this.DataContext.Target.LogonCredentials.LogOnPassword;
            this.txtUidTarget.Text = this.DataContext.Target.LogonCredentials.LogOnUserId;
            this.txtDomainTarget.Text = this.DataContext.Target.LogonCredentials.LogOnDomain;

            this.cboSource.SelectedValue = this.DataContext.Source.Name ?? this.cboSource.SelectedValue;
            this.cboTarget.SelectedValue = this.DataContext.Target.Name ?? this.cboTarget.SelectedValue;

            if (this.DataContext.Source.ConnectorDescription != null)
            {
                this.btnPathSource.Visible = !this.DataContext.Source.ConnectorPathDescription.Irrelevant && (this.DataContext.Source.ShowSelectPathDialog || this.DataContext.Source.ShowSelectFileDialog);
                this.txtPathSource.Visible = !this.DataContext.Source.ConnectorPathDescription.Irrelevant;
                this.txtPasswordSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.txtUidSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.txtDomainSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
            }

            if (this.DataContext.Target.ConnectorDescription != null)
            {
                this.btnPathTarget.Visible = !this.DataContext.Target.ConnectorPathDescription.Irrelevant && (this.DataContext.Target.ShowSelectPathDialog || this.DataContext.Target.ShowSelectFileDialog);
                this.txtPathTarget.Visible = !this.DataContext.Target.ConnectorPathDescription.Irrelevant;
                this.txtPasswordTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.txtUidTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.txtDomainTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
            }
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