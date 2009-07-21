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
            this.contextDataSource.CurrentChanged +=
                (s, ev) => { this.DataContext.Source.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key; };

            this.contextDataTarget.DataSource = this.DataContext;
            this.contextDataSource.DataMember = "Clients";
            this.contextDataTarget.CurrentChanged +=
                (s, ev) => { this.DataContext.Target.Name = ((KeyValuePair<string, string>)((BindingSource)s).Current).Key; };

            this.btnCancel.Click += (s, ev) => this.Close();
            this.btnRun.Click += (s, ev) => this.DataContext.Run();
            this.btnPathSource.Click += (s, ev) => this.ShowFolderDialog(txtPathSource);
            this.btnPathTarget.Click += (s, ev) => this.ShowFolderDialog(txtPathTarget);

            this.txtPathSource.TextChanged += (s, ev) => { this.DataContext.Source.Path = ((TextBox)s).Text; };
            this.txtPathTarget.TextChanged += (s, ev) => { this.DataContext.Target.Path = ((TextBox)s).Text; };
        }

        /// <summary>
        /// Showa the folder browse dialog for a specified textbox
        /// </summary>
        /// <param name="textBox"> The text box that should be updated with the path. </param>
        private void ShowFolderDialog(Control textBox)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = folderBrowser.SelectedPath;
            } 
        }
    }
}