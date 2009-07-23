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
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDomainSource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDomainTarget = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).BeginInit();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(363, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "&cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(453, 325);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 24;
            this.btnRun.Text = "&run";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // txtPasswordSource
            // 
            this.txtPasswordSource.Location = new System.Drawing.Point(395, 155);
            this.txtPasswordSource.Name = "txtPasswordSource";
            this.txtPasswordSource.PasswordChar = '*';
            this.txtPasswordSource.Size = new System.Drawing.Size(132, 20);
            this.txtPasswordSource.TabIndex = 10;
            // 
            // txtUidSource
            // 
            this.txtUidSource.Location = new System.Drawing.Point(395, 128);
            this.txtUidSource.Name = "txtUidSource";
            this.txtUidSource.Size = new System.Drawing.Size(132, 20);
            this.txtUidSource.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(316, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "&password:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(316, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "&user id / name:";
            // 
            // cboSource
            // 
            this.cboSource.DataSource = this.contextDataSource;
            this.cboSource.DisplayMember = "Source";
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(58, 143);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(249, 21);
            this.cboSource.TabIndex = 3;
            // 
            // contextDataSource
            // 
            this.contextDataSource.AllowNew = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "s&ource:";
            // 
            // txtPasswordTarget
            // 
            this.txtPasswordTarget.Location = new System.Drawing.Point(395, 255);
            this.txtPasswordTarget.Name = "txtPasswordTarget";
            this.txtPasswordTarget.PasswordChar = '*';
            this.txtPasswordTarget.Size = new System.Drawing.Size(132, 20);
            this.txtPasswordTarget.TabIndex = 21;
            // 
            // txtUidTarget
            // 
            this.txtUidTarget.Location = new System.Drawing.Point(395, 228);
            this.txtUidTarget.Name = "txtUidTarget";
            this.txtUidTarget.Size = new System.Drawing.Size(132, 20);
            this.txtUidTarget.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(316, 258);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "p&assword:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(316, 231);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "user id / &name:";
            // 
            // cboTarget
            // 
            this.cboTarget.DataSource = this.contextDataTarget;
            this.cboTarget.DisplayMember = "Target";
            this.cboTarget.FormattingEnabled = true;
            this.cboTarget.Location = new System.Drawing.Point(58, 243);
            this.cboTarget.Name = "cboTarget";
            this.cboTarget.Size = new System.Drawing.Size(249, 21);
            this.cboTarget.TabIndex = 14;
            // 
            // contextDataTarget
            // 
            this.contextDataTarget.AllowNew = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 246);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "&target:";
            // 
            // lblDialogStatus
            // 
            this.lblDialogStatus.Location = new System.Drawing.Point(117, 20);
            this.lblDialogStatus.Name = "lblDialogStatus";
            this.lblDialogStatus.Size = new System.Drawing.Size(326, 51);
            this.lblDialogStatus.TabIndex = 1;
            this.lblDialogStatus.Text = "Please select the source and destination of the synchronization process. You also" +
                " may need to provide a user name and a password for the source and/or the destin" +
                "ation.";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Controls.Add(this.lblProgressStatus);
            this.pnlProgress.Controls.Add(this.SyncProgress);
            this.pnlProgress.Controls.Add(this.label3);
            this.pnlProgress.Location = new System.Drawing.Point(12, 12);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(516, 100);
            this.pnlProgress.TabIndex = 0;
            this.pnlProgress.Visible = false;
            // 
            // lblProgressStatus
            // 
            this.lblProgressStatus.Location = new System.Drawing.Point(18, 13);
            this.lblProgressStatus.Name = "lblProgressStatus";
            this.lblProgressStatus.Size = new System.Drawing.Size(477, 46);
            this.lblProgressStatus.TabIndex = 0;
            this.lblProgressStatus.Text = "please wait ...";
            // 
            // SyncProgress
            // 
            this.SyncProgress.Location = new System.Drawing.Point(100, 62);
            this.SyncProgress.Name = "SyncProgress";
            this.SyncProgress.Size = new System.Drawing.Size(303, 23);
            this.SyncProgress.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-2, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&target:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "pat&h:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "pat&h:";
            // 
            // txtPathSource
            // 
            this.txtPathSource.Location = new System.Drawing.Point(58, 170);
            this.txtPathSource.Name = "txtPathSource";
            this.txtPathSource.Size = new System.Drawing.Size(218, 20);
            this.txtPathSource.TabIndex = 5;
            // 
            // txtPathTarget
            // 
            this.txtPathTarget.Location = new System.Drawing.Point(58, 270);
            this.txtPathTarget.Name = "txtPathTarget";
            this.txtPathTarget.Size = new System.Drawing.Size(218, 20);
            this.txtPathTarget.TabIndex = 16;
            // 
            // btnPathTarget
            // 
            this.btnPathTarget.Location = new System.Drawing.Point(279, 268);
            this.btnPathTarget.Name = "btnPathTarget";
            this.btnPathTarget.Size = new System.Drawing.Size(27, 23);
            this.btnPathTarget.TabIndex = 17;
            this.btnPathTarget.Text = "...";
            this.btnPathTarget.UseVisualStyleBackColor = true;
            // 
            // btnPathSource
            // 
            this.btnPathSource.Location = new System.Drawing.Point(279, 168);
            this.btnPathSource.Name = "btnPathSource";
            this.btnPathSource.Size = new System.Drawing.Size(27, 23);
            this.btnPathSource.TabIndex = 6;
            this.btnPathSource.Text = "...";
            this.btnPathSource.UseVisualStyleBackColor = true;
            // 
            // folderBrowser
            // 
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 325);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 26;
            this.btnLoad.Text = "&load...";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(93, 325);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "&save ...";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(316, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "&domain";
            // 
            // txtDomainSource
            // 
            this.txtDomainSource.Location = new System.Drawing.Point(395, 181);
            this.txtDomainSource.Name = "txtDomainSource";
            this.txtDomainSource.Size = new System.Drawing.Size(132, 20);
            this.txtDomainSource.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(316, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "do&main:";
            // 
            // txtDomainTarget
            // 
            this.txtDomainTarget.Location = new System.Drawing.Point(395, 281);
            this.txtDomainTarget.Name = "txtDomainTarget";
            this.txtDomainTarget.Size = new System.Drawing.Size(132, 20);
            this.txtDomainTarget.TabIndex = 23;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SyncWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 360);
            this.Controls.Add(this.pnlProgress);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
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
            this.Name = "SyncWizard";
            this.Text = "SyncWizard";
            this.Load += new System.EventHandler(this.SyncWizard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).EndInit();
            this.pnlProgress.ResumeLayout(false);
            this.pnlProgress.PerformLayout();
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
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDomainSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDomainTarget;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}