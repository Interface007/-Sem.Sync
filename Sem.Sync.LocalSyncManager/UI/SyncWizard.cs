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

    using GenericHelpers;
    using GenericHelpers.EventArgs;
    using GenericHelpers.Exceptions;

    using Properties;

    using SharedUI.Common;
    using SharedUI.WinForms.Tools;
    using SyncBase;
    
    using Tools;

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
        public SyncWizardContext<UiDispatcher> DataContext { get; set; }

        /// <summary>
        /// Setup of the event handler routing and "databinding"
        /// </summary>
        /// <param name="sender">the sender of the event - in this case the form instance </param>
        /// <param name="e"> empty event arguments </param>
        private void SyncWizard_Load(object sender, EventArgs e)
        {
            ////var data = ExceptionHandler.ExceptionWriter[0].Read();
            ////if (data != null)
            ////{
            ////    HttpHelper.DefaultInstance.UiDispatcher = this.DataContext.s

            ////    HttpHelper.DefaultInstance.GetContentPost(
            ////        "http://www.svenerikmatzen.info/exceptionsink/receive.aspx",
            ////        "[NOCACHE]",
            ////        data.ToString());
            ////}

            ExceptionHandler.ExceptionWriter.ForEach(writer => writer.Clean());

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
            this.btnClose.Click += (s, ev) => this.Close();
            this.btnRun.Click += (s, ev) => this.RunCommands();
            this.btnCancel.Click += (s, ev) => this.DataContext.Cancel = true;
            this.btnSave.Click += (s, ev) => this.DataContext.SaveTo(this.AskForDestinationFile(Path.GetFileNameWithoutExtension(this.DataContext.CurrentSyncWorkflowData)));
            this.btnPathSource.Click += (s, ev) => this.ShowFolderDialog(this.txtPathSource, this.DataContext.Source.ShowSelectFileDialog, false);
            this.btnPathTarget.Click += (s, ev) => this.ShowFolderDialog(this.txtPathTarget, this.DataContext.Target.ShowSelectFileDialog, true);

            // we don't need to detach from these events, so we don't need to save the lambda into a variable for the detaching.
            this.openWorkingFolderToolStripMenuItem.Click += (s, ev) => SyncWizardContext<UiDispatcher>.OpenWorkingFolder();
            this.exitToolStripMenuItem.Click += (s, ev) => this.Close();
            this.removeDuplettesToolStripMenuItem.Click += (s, ev) => this.DataContext.Run("SyncLists\\RemoveDuplicatesFromOutlook.SyncList");
            this.generateSampleProfilesToolStripMenuItem.Click += (s, ev) => this.DataContext.GenerateSamples();
            this.deleteCurrentProfileToolStripMenuItem.Click += (s, ev) => this.DataContext.DeleteWorkflowData(this.DataContext.CurrentSyncWorkflowData);
            this.openCommandsViewToolStripMenuItem.Click += (s, ev) => new Commands { DataContext = new ClientViewModel() } .Show();
            this.starteSynchronisationToolStripMenuItem.Click += (s, ev) => this.RunCommands();

            // setup event handling
            this.DataContext.ProcessingEvent = this.ProcessingEventHandler;
            this.DataContext.ProgressEvent = (object s, ProgressEventArgs eventArgs) =>
                {
                    this.Invoke(
                        new MethodInvoker(
                            () =>
                            {
                                this.SyncProgress.Value = eventArgs.PercentageDone;
                                this.SyncProgress.Refresh();
                                return;
                            }));
                };

            this.DataContext.FinishedEvent += s => this.Invoke(
                                                       new MethodInvoker(
                                                       () =>
                                                           {
                                                               this.DataContext.Cancel = false;
                                                               this.pnlProgress.Visible = false;
                                                               this.lblDialogStatus.Text = Resources.ProcessFinishedMessage;
                                                               this.DataContext.Locked = false;
                                                           }));

            // initialize the gui
            this.cboSource.SelectedIndex = -1;
            this.cboTarget.SelectedIndex = -1;
            this.cboWorkFlowTemplates.SelectedIndex = -1;

            this.DataContext.PropertyChanged += this.ReadFromContext;
            this.cboWorkFlowData.SelectedValue = Config.LastUsedSyncTemplateData ?? this.cboWorkFlowData.SelectedValue;
        }

        /// <summary>
        /// Handels the processing events
        /// </summary>
        /// <param name="entity"> The entity. </param>
        /// <param name="eventArgs"> The event args. </param>
        private void ProcessingEventHandler(object entity, ProcessingEventArgs eventArgs)
        {
            this.Invoke(new MethodInvoker(() =>
                {
                    var currentObject = entity ?? eventArgs.Item;
                    var currentContact = entity as StdContact ?? eventArgs.Item as StdContact;
                    var message = eventArgs.Message + " " + 
                        (currentObject == null
                        || currentObject.GetType() == typeof(SyncEngine)
                        || currentObject as StdClient != null 
                        ? string.Empty 
                        : currentObject.ToString());

                    this.lblProgressStatus.Text = message;
                    this.LogList.Items.Add(message);
                    this.LogList.TopIndex = this.LogList.Items.Count > 10 ? this.LogList.Items.Count - 10 : 0;
                    this.lblProgressStatus.Refresh();
                    if (currentContact != null)
                    {
                        // update image, if there is one and image display is switched on.
                        if (chkShowImage.Checked && currentContact.PictureData != null && currentContact.PictureData.Length > 10)
                        {
                            using (var imageStream = new MemoryStream(currentContact.PictureData))
                            {
                                try
                                {
                                    this.currentPersonImage.Image = new Bitmap(imageStream);
                                }
                                catch (ArgumentException)
                                {
                                }
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
                }));
        }

        /// <summary>
        /// Runs the currently command sequence stored inside "SyncLists\\Wizard.XSyncList" with the context
        /// (the context does contain source and target of the data with additional information like credentials)
        /// </summary>
        private void RunCommands()
        {
            if (this.DataContext.Locked)
            {
                MessageBox.Show(Resources.ProcessRunningMessage);
                return;
            }

            this.DataContext.Locked = true;
            Config.LastUsedSyncTemplateData = this.cboWorkFlowData.SelectedValue.ToString();

            this.LogList.Items.Clear();
            this.pnlProgress.Visible = true;

            var bw = new BackgroundWorker();
            bw.DoWork += (sender, e) => this.DataContext.Run(this.DataContext.CurrentSyncWorkflowTemplate);
            bw.RunWorkerAsync();
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
                this.txtUidSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.txtDomainSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.txtPasswordSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;

                this.lblPathSource.Visible = !this.DataContext.Source.ConnectorPathDescription.Irrelevant;
                this.lblUidSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.lblDomainSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
                this.lblPasswordSource.Visible = this.DataContext.Source.ConnectorDescription.NeedsCredentials;
            }

            if (this.DataContext.Target.ConnectorDescription != null)
            {
                this.btnPathTarget.Visible = !this.DataContext.Target.ConnectorPathDescription.Irrelevant && (this.DataContext.Target.ShowSelectPathDialog || this.DataContext.Target.ShowSelectFileDialog);
                this.txtPathTarget.Visible = !this.DataContext.Target.ConnectorPathDescription.Irrelevant;
                this.txtUidTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.txtDomainTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.txtPasswordTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;

                this.lblPathTarget.Visible = !this.DataContext.Target.ConnectorPathDescription.Irrelevant;
                this.lblUidTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.lblDomainTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
                this.lblPasswordTarget.Visible = this.DataContext.Target.ConnectorDescription.NeedsCredentials;
            }

            if (e.PropertyName != "CurrentSyncWorkflowData")
            {
                this.contextDataWorkflowData.DataSource = null;
                this.contextDataWorkflowData.DataMember = string.Empty;

                this.contextDataWorkflowData.DataSource = this.DataContext;
                this.contextDataWorkflowData.DataMember = "SyncWorkflowData";

                this.contextDataWorkflowData.ResetBindings(false);
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
            this.DataContext.PropertyChanged += (s, ev) => control.SelectedValue = GenericHelpers.Tools.GetPropertyValueString(this.DataContext, targetPath) ?? control.SelectedValue;
        }

        /// <summary>
        /// UI-Action to get a file path
        /// </summary>
        /// <param name="currentFileName"> The current File Name to be displayed. </param>
        /// <returns> the user entered path to a file </returns>
        private string AskForDestinationFile(string currentFileName)
        {
            this.saveFileDialog1.DefaultExt = SyncWizardContext<UiDispatcher>.SyncListDataFileExtension;
            this.saveFileDialog1.Filter = "SyncWizard|*" + SyncWizardContext<UiDispatcher>.SyncListDataFileExtension;
            this.saveFileDialog1.FilterIndex = 0;
            this.saveFileDialog1.AddExtension = true;
            this.saveFileDialog1.FileName = currentFileName;
            this.saveFileDialog1.InitialDirectory = SyncWizardContext<UiDispatcher>.WorkingFolderData;
            return
                this.saveFileDialog1.ShowDialog() == DialogResult.OK
                ? this.saveFileDialog1.FileName
                : string.Empty;
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
                this.folderBrowser.SelectedPath = SyncWizardContext<UiDispatcher>.ResolvePath(textBox.Text);
                if (this.folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = this.folderBrowser.SelectedPath;
                }

                return;
            }

            if (useSave)
            {
                this.saveFileDialog1.DefaultExt = string.Empty;
                this.saveFileDialog1.Filter = Resources.AllFilesFileFilter;
                this.saveFileDialog1.AddExtension = true;
                this.saveFileDialog1.FileName = textBox.Text;
                try
                {
                    if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = this.saveFileDialog1.FileName;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    this.ProcessingEventHandler(null, new ProcessingEventArgs { Message = ex.Message });
                }

                return;
            }

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = this.openFileDialog1.FileName;
            }
        }
    }
}