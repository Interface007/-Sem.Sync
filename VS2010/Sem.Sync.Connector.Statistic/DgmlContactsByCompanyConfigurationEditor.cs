// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompanyConfigurationEditor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlContactsByCompanyConfigurationEditor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.IO;
    using System.Windows.Forms;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// This editor is a really simple one (no MVVM or similar pattern).
    /// </summary>
    public partial class DgmlContactsByCompanyConfigurationEditor : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlContactsByCompanyConfigurationEditor"/> class.
        /// </summary>
        public DgmlContactsByCompanyConfigurationEditor()
        {
            InitializeComponent();
            
            // fill the combo box with all properties and methods that might be used for grouping
            this.cboGroupingProperty.Items.AddRange(Tools.GetPropertyList(typeof(StdContact)).ToArray());
        }

        public DialogResult ShowDialog(DgmlContactsByCompanyConfigurationData editorData)
        {
            this.cboGroupingProperty.Text = editorData.GroupingPropertName;
            this.txtFileName.Text = editorData.DestinationPath;
            var dialogResult = this.ShowDialog();
            editorData.GroupingPropertName = this.cboGroupingProperty.Text;
            editorData.DestinationPath = this.txtFileName.Text;

            return dialogResult;
        }

        private void BtnOkClick(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void BtnFileName(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtFileName.Text))
            {
                saveFileDialog.FileName = Path.GetFileName(this.txtFileName.Text);
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(this.txtFileName.Text);
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtFileName.Text = saveFileDialog.FileName;
            }
        }
    }
}
