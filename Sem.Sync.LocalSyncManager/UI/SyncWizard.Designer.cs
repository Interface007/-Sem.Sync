namespace Sem.Sync.LocalSyncManager.UI
{
    partial class SyncWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncWizard));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtPasswordSource = new System.Windows.Forms.TextBox();
            this.txtUidSource = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.contextDataSource = new System.Windows.Forms.BindingSource(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.txtPasswordTarget = new System.Windows.Forms.TextBox();
            this.txtUidTarget = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboTarget = new System.Windows.Forms.ComboBox();
            this.contextDataTarget = new System.Windows.Forms.BindingSource(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.lblDialogStatus = new System.Windows.Forms.Label();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.chkShowImage = new System.Windows.Forms.CheckBox();
            this.currentPersonImage = new System.Windows.Forms.PictureBox();
            this.lblProgressStatus = new System.Windows.Forms.Label();
            this.SyncProgress = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPathSource = new System.Windows.Forms.TextBox();
            this.txtPathTarget = new System.Windows.Forms.TextBox();
            this.btnPathTarget = new System.Windows.Forms.Button();
            this.btnPathSource = new System.Windows.Forms.Button();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDomainSource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDomainTarget = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cboWorkFlowTemplates = new System.Windows.Forms.ComboBox();
            this.contextDataWorkflows = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCurrentProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seperatorStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplettesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateSampleProfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboWorkFlowData = new System.Windows.Forms.ComboBox();
            this.contextDataWorkflowData = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openNetworksViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).BeginInit();
            this.pnlProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentPersonImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataWorkflows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataWorkflowData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            resources.ApplyResources(this.btnRun, "btnRun");
            this.btnRun.Name = "btnRun";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // txtPasswordSource
            // 
            resources.ApplyResources(this.txtPasswordSource, "txtPasswordSource");
            this.txtPasswordSource.Name = "txtPasswordSource";
            // 
            // txtUidSource
            // 
            resources.ApplyResources(this.txtUidSource, "txtUidSource");
            this.txtUidSource.Name = "txtUidSource";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label8.Name = "label8";
            // 
            // cboSource
            // 
            this.cboSource.DataSource = this.contextDataSource;
            this.cboSource.DisplayMember = "Source";
            this.cboSource.FormattingEnabled = true;
            resources.ApplyResources(this.cboSource, "cboSource");
            this.cboSource.Name = "cboSource";
            // 
            // contextDataSource
            // 
            this.contextDataSource.AllowNew = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label9.Name = "label9";
            // 
            // txtPasswordTarget
            // 
            resources.ApplyResources(this.txtPasswordTarget, "txtPasswordTarget");
            this.txtPasswordTarget.Name = "txtPasswordTarget";
            // 
            // txtUidTarget
            // 
            resources.ApplyResources(this.txtUidTarget, "txtUidTarget");
            this.txtUidTarget.Name = "txtUidTarget";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label11.Name = "label11";
            // 
            // cboTarget
            // 
            this.cboTarget.DataSource = this.contextDataTarget;
            this.cboTarget.DisplayMember = "Target";
            this.cboTarget.FormattingEnabled = true;
            resources.ApplyResources(this.cboTarget, "cboTarget");
            this.cboTarget.Name = "cboTarget";
            // 
            // contextDataTarget
            // 
            this.contextDataTarget.AllowNew = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label12.Name = "label12";
            // 
            // lblDialogStatus
            // 
            resources.ApplyResources(this.lblDialogStatus, "lblDialogStatus");
            this.lblDialogStatus.Name = "lblDialogStatus";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Controls.Add(this.chkShowImage);
            this.pnlProgress.Controls.Add(this.currentPersonImage);
            this.pnlProgress.Controls.Add(this.lblProgressStatus);
            this.pnlProgress.Controls.Add(this.SyncProgress);
            this.pnlProgress.Controls.Add(this.label3);
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.Name = "pnlProgress";
            // 
            // chkShowImage
            // 
            resources.ApplyResources(this.chkShowImage, "chkShowImage");
            this.chkShowImage.Checked = true;
            this.chkShowImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowImage.Name = "chkShowImage";
            this.chkShowImage.UseVisualStyleBackColor = true;
            // 
            // currentPersonImage
            // 
            resources.ApplyResources(this.currentPersonImage, "currentPersonImage");
            this.currentPersonImage.Name = "currentPersonImage";
            this.currentPersonImage.TabStop = false;
            // 
            // lblProgressStatus
            // 
            resources.ApplyResources(this.lblProgressStatus, "lblProgressStatus");
            this.lblProgressStatus.Name = "lblProgressStatus";
            // 
            // SyncProgress
            // 
            resources.ApplyResources(this.SyncProgress, "SyncProgress");
            this.SyncProgress.Name = "SyncProgress";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label4.Name = "label4";
            // 
            // txtPathSource
            // 
            resources.ApplyResources(this.txtPathSource, "txtPathSource");
            this.txtPathSource.Name = "txtPathSource";
            // 
            // txtPathTarget
            // 
            resources.ApplyResources(this.txtPathTarget, "txtPathTarget");
            this.txtPathTarget.Name = "txtPathTarget";
            // 
            // btnPathTarget
            // 
            resources.ApplyResources(this.btnPathTarget, "btnPathTarget");
            this.btnPathTarget.Name = "btnPathTarget";
            this.btnPathTarget.UseVisualStyleBackColor = true;
            // 
            // btnPathSource
            // 
            resources.ApplyResources(this.btnPathSource, "btnPathSource");
            this.btnPathSource.Name = "btnPathSource";
            this.btnPathSource.UseVisualStyleBackColor = true;
            // 
            // folderBrowser
            // 
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label5.Name = "label5";
            // 
            // txtDomainSource
            // 
            resources.ApplyResources(this.txtDomainSource, "txtDomainSource");
            this.txtDomainSource.Name = "txtDomainSource";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Name = "label6";
            // 
            // txtDomainTarget
            // 
            resources.ApplyResources(this.txtDomainTarget, "txtDomainTarget");
            this.txtDomainTarget.Name = "txtDomainTarget";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cboWorkFlowTemplates
            // 
            this.cboWorkFlowTemplates.DataSource = this.contextDataWorkflows;
            this.cboWorkFlowTemplates.FormattingEnabled = true;
            resources.ApplyResources(this.cboWorkFlowTemplates, "cboWorkFlowTemplates");
            this.cboWorkFlowTemplates.Name = "cboWorkFlowTemplates";
            // 
            // contextDataWorkflows
            // 
            this.contextDataWorkflows.AllowNew = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sem.Sync.LocalSyncManager.Properties.Resources._1;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorkingFolderToolStripMenuItem,
            this.deleteCurrentProfileToolStripMenuItem,
            this.seperatorStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openWorkingFolderToolStripMenuItem
            // 
            this.openWorkingFolderToolStripMenuItem.Name = "openWorkingFolderToolStripMenuItem";
            resources.ApplyResources(this.openWorkingFolderToolStripMenuItem, "openWorkingFolderToolStripMenuItem");
            // 
            // deleteCurrentProfileToolStripMenuItem
            // 
            this.deleteCurrentProfileToolStripMenuItem.Name = "deleteCurrentProfileToolStripMenuItem";
            resources.ApplyResources(this.deleteCurrentProfileToolStripMenuItem, "deleteCurrentProfileToolStripMenuItem");
            // 
            // seperatorStripMenuItem1
            // 
            this.seperatorStripMenuItem1.Name = "seperatorStripMenuItem1";
            resources.ApplyResources(this.seperatorStripMenuItem1, "seperatorStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeDuplettesToolStripMenuItem,
            this.generateSampleProfilesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.openNetworksViewToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // removeDuplettesToolStripMenuItem
            // 
            this.removeDuplettesToolStripMenuItem.Name = "removeDuplettesToolStripMenuItem";
            resources.ApplyResources(this.removeDuplettesToolStripMenuItem, "removeDuplettesToolStripMenuItem");
            // 
            // generateSampleProfilesToolStripMenuItem
            // 
            this.generateSampleProfilesToolStripMenuItem.Name = "generateSampleProfilesToolStripMenuItem";
            resources.ApplyResources(this.generateSampleProfilesToolStripMenuItem, "generateSampleProfilesToolStripMenuItem");
            // 
            // cboWorkFlowData
            // 
            this.cboWorkFlowData.DataSource = this.contextDataWorkflowData;
            this.cboWorkFlowData.FormattingEnabled = true;
            resources.ApplyResources(this.cboWorkFlowData, "cboWorkFlowData");
            this.cboWorkFlowData.Name = "cboWorkFlowData";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // openNetworksViewToolStripMenuItem
            // 
            this.openNetworksViewToolStripMenuItem.Name = "openNetworksViewToolStripMenuItem";
            resources.ApplyResources(this.openNetworksViewToolStripMenuItem, "openNetworksViewToolStripMenuItem");
            // 
            // SyncWizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.cboWorkFlowData);
            this.Controls.Add(this.cboWorkFlowTemplates);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPathSource);
            this.Controls.Add(this.btnPathTarget);
            this.Controls.Add(this.txtPathTarget);
            this.Controls.Add(this.txtPathSource);
            this.Controls.Add(this.txtDomainTarget);
            this.Controls.Add(this.txtPasswordTarget);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtUidTarget);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboTarget);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtDomainSource);
            this.Controls.Add(this.txtPasswordSource);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUidSource);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDialogStatus);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SyncWizard";
            this.Load += new System.EventHandler(this.SyncWizard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentPersonImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataWorkflowData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtPasswordSource;
        private System.Windows.Forms.TextBox txtUidSource;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPasswordTarget;
        private System.Windows.Forms.TextBox txtUidTarget;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboTarget;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblDialogStatus;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgressStatus;
        private System.Windows.Forms.ProgressBar SyncProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPathSource;
        private System.Windows.Forms.TextBox txtPathTarget;
        private System.Windows.Forms.BindingSource contextDataSource;
        private System.Windows.Forms.BindingSource contextDataTarget;
        private System.Windows.Forms.Button btnPathTarget;
        private System.Windows.Forms.Button btnPathSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDomainSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDomainTarget;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cboWorkFlowTemplates;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator seperatorStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.BindingSource contextDataWorkflows;
        private System.Windows.Forms.ComboBox cboWorkFlowData;
        private System.Windows.Forms.BindingSource contextDataWorkflowData;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDuplettesToolStripMenuItem;
        private System.Windows.Forms.PictureBox currentPersonImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowImage;
        private System.Windows.Forms.ToolStripMenuItem generateSampleProfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCurrentProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openNetworksViewToolStripMenuItem;
    }
}