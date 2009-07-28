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
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using Business;

    using GenericHelpers.EventArgs;

    using Properties;

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
            // setup the data binding for combo boxes
            this.SetupBind(this.contextDataSource, "ClientsSource", this.cboSource, "Source.Name");
            this.SetupBind(this.contextDataTarget, "ClientsTarget", this.cboTarget, "Target.Name");
            this.SetupBind(this.contextDataWorkflows, "SyncWorkflowsTemplates", this.cboWorkFlowTemplates, "CurrentSyncWorkflowTemplate");
            this.SetupBind(this.contextDataWorkflowData, "SyncWorkflowData", this.cboWorkFlowData, "CurrentSyncWorkflowData");

            // setup data propagation from control to business object
            // todo: this needs to be changed to be included into databinding setup
            this.txtPathSource.TextChanged += (s, ev) => { this.DataContext.Source.Path = ((Control)s).Text; };
            this.txtPathTarget.TextChanged += (s, ev) => { this.DataContext.Target.Path = ((Control)s).Text; };
            this.txtUidSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnUserId = ((Control)s).Text; };
            this.txtUidTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnUserId = ((Control)s).Text; };
            this.txtPasswordSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnPassword = ((Control)s).Text; };
            this.txtPasswordTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnPassword = ((Control)s).Text; };
            this.txtDomainSource.TextChanged += (s, ev) => { this.DataContext.Source.LogonCredentials.LogOnDomain = ((Control)s).Text; };
            this.txtDomainTarget.TextChanged += (s, ev) => { this.DataContext.Target.LogonCredentials.LogOnDomain = ((Control)s).Text; };

            // setup the click handling
            this.btnCancel.Click += (s, ev) => this.Close();
            this.btnRun.Click += (s, ev) => this.RunCommands();
            this.btnPathSource.Click += (s, ev) => this.ShowFolderDialog(this.txtPathSource, this.DataContext.Source.ShowSelectFileDialog, false);
            this.btnPathTarget.Click += (s, ev) => this.ShowFolderDialog(this.txtPathTarget, this.DataContext.Target.ShowSelectFileDialog, true);
            this.btnSave.Click += (s, ev) => this.DataContext.SaveTo(this.cboWorkFlowData.Text, this.cboWorkFlowData.Text);
            this.openWorkingFolderToolStripMenuItem.Click += (s, ev) => SyncWizardContext.OpenWorkingFolder();
            this.exitToolStripMenuItem.Click += (s, ev) => this.Close();
            this.removeDuplettesToolStripMenuItem.Click += (s, ev) => this.DataContext.Run("SyncLists\\RemoveDuplicatesFromOutlook.SyncList");

            // setup event handling
            this.DataContext.ProcessingEvent = (object entity, ProcessingEventArgs eventArgs) =>
                {
                    this.lblProgressStatus.Text = eventArgs.Message + " " + (entity as StdContact == null ? string.Empty : entity.ToString());
                    this.lblProgressStatus.Refresh();
                    var currentContact = (entity as StdContact) ?? (eventArgs.Item as StdContact);
                    if (currentContact != null)
                    {
                        // update image, if there is one and image display is switched on.
                        if (chkShowImage.CheckState == CheckState.Checked && currentContact.PictureData != null && currentContact.PictureData.Length > 10)
                        {
                            using (var imageStream = new MemoryStream(currentContact.PictureData))
                            {
                                this.currentPersonImage.Image = new Bitmap(imageStream);
                            }

                            this.currentPersonImage.Visible = true;
                            this.currentPersonImage.Refresh();

                            // !!! HERE WE EXIT THE METHOD !!!
                            return;
                        }
                    }

                    // hide the image if we need to.
                    if (this.currentPersonImage.Visible)
                    {
                        this.currentPersonImage.Visible = false;
                    }

                    return;
                };
            
            this.DataContext.ProgressEvent = (ProgressEventArgs eventArgs) =>
                {
                    this.SyncProgress.Value = eventArgs.PercentageDone;
                    this.SyncProgress.Refresh();
                    return;
                };
            
            // initialize the gui
            this.cboSource.SelectedIndex = (this.cboSource.Items.Count > 0) ? 0 : this.cboSource.SelectedIndex;
            this.cboTarget.SelectedIndex = (this.cboTarget.Items.Count > 0) ? 0 : this.cboTarget.Items.Count;
            this.cboWorkFlowData.SelectedIndex = (this.cboWorkFlowData.Items.Count > 0) ? 0 : this.cboWorkFlowData.Items.Count;
            this.cboWorkFlowTemplates.SelectedIndex = (this.cboWorkFlowTemplates.Items.Count > 0) ? 0 : this.cboWorkFlowTemplates.Items.Count;

            this.DataContext.PropertyChanged += this.ReadFromContext;
            this.ReadFromContext(null, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Runs the currently command sequence stored inside "SyncLists\\Wizard.XSyncList" with the context
        /// (the context does contain source and target of the data with additional information like credentials)
        /// </summary>
        private void RunCommands()
        {
            this.pnlProgress.Visible = true;
            this.DataContext.Run(this.DataContext.CurrentSyncWorkflowTemplate);
            this.pnlProgress.Visible = false;
            this.lblDialogStatus.Text = Resources.ProcessFinishedMessage;
        }

        /// <summary>
        /// In this method all properties are read from the context and pumped into the GUI 
        /// elements attributes.
        /// </summary>
        /// <param name="sender"> The sender of this event. </param>
        /// <param name="e"> The change event parameter. </param>
        private void ReadFromContext(object sender, PropertyChangedEventArgs e)
        {
            // todo: this needs to be changed to be included into databinding setup
            this.txtPathSource.Text = this.DataContext.Source.Path;
            this.txtPathTarget.Text = this.DataContext.Target.Path;

            this.txtUidSource.Text = this.DataContext.Source.LogonCredentials.LogOnUserId;
            this.txtPasswordSource.Text = this.DataContext.Source.LogonCredentials.LogOnPassword;
            this.txtDomainSource.Text = this.DataContext.Source.LogonCredentials.LogOnDomain;

            this.txtPasswordTarget.Text = this.DataContext.Target.LogonCredentials.LogOnPassword;
            this.txtUidTarget.Text = this.DataContext.Target.LogonCredentials.LogOnUserId;
            this.txtDomainTarget.Text = this.DataContext.Target.LogonCredentials.LogOnDomain;

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
        /// Sets up a databinding of a combo box to an object data source.
        /// </summary>
        /// <param name="bindingSource"> The binding source. </param>
        /// <param name="dataMember"> The data member. </param>
        /// <param name="control">the control to bind</param>
        /// <param name="targetPath"> The target path. </param>
        private void SetupBind(BindingSource bindingSource, string dataMember, ListControl control, string targetPath)
        {
            // todo: currently the binding source is still needed - this has to be removed 
            // todo: the binding setup has to be moved to the generic tools assembly and be generalized for more control types
            bindingSource.DataSource = this.DataContext;
            bindingSource.DataMember = dataMember;
            control.DisplayMember = "Value";
            control.ValueMember = "Key";
            control.SelectedValueChanged += (s, ev) => GenericHelpers.Tools.SetPropertyValue(this.DataContext, targetPath, (((ComboBox)s).SelectedValue ?? string.Empty).ToString());
            this.DataContext.PropertyChanged += (s, ev) => control.SelectedValue = GenericHelpers.Tools.GetPropertyValue(this.DataContext, targetPath) ?? control.SelectedValue;
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