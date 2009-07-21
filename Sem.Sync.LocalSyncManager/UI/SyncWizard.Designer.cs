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
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.contextDataSource = new System.Windows.Forms.BindingSource(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboTarget = new System.Windows.Forms.ComboBox();
            this.contextDataTarget = new System.Windows.Forms.BindingSource(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SyncProgress = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPathSource = new System.Windows.Forms.TextBox();
            this.txtPathTarget = new System.Windows.Forms.TextBox();
            this.btnPathTarget = new System.Windows.Forms.Button();
            this.btnPathSource = new System.Windows.Forms.Button();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(363, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(453, 301);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 19;
            this.btnRun.Text = "&run";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(363, 155);
            this.textBox5.Name = "textBox5";
            this.textBox5.PasswordChar = '*';
            this.textBox5.Size = new System.Drawing.Size(165, 20);
            this.textBox5.TabIndex = 8;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(363, 128);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(165, 20);
            this.textBox6.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(275, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "&password:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(275, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "&user id / name:";
            // 
            // cboSource
            // 
            this.cboSource.DataSource = this.contextDataSource;
            this.cboSource.DisplayMember = "Source";
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(58, 128);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(198, 21);
            this.cboSource.TabIndex = 1;
            // 
            // contextDataSource
            // 
            this.contextDataSource.AllowNew = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "&source:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(363, 255);
            this.textBox7.Name = "textBox7";
            this.textBox7.PasswordChar = '*';
            this.textBox7.Size = new System.Drawing.Size(165, 20);
            this.textBox7.TabIndex = 17;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(363, 228);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(165, 20);
            this.textBox8.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(275, 258);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "p&assword:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(275, 231);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "user id / &name:";
            // 
            // cboTarget
            // 
            this.cboTarget.DataSource = this.contextDataTarget;
            this.cboTarget.DisplayMember = "Target";
            this.cboTarget.FormattingEnabled = true;
            this.cboTarget.Location = new System.Drawing.Point(58, 228);
            this.cboTarget.Name = "cboTarget";
            this.cboTarget.Size = new System.Drawing.Size(198, 21);
            this.cboTarget.TabIndex = 10;
            // 
            // contextDataTarget
            // 
            this.contextDataTarget.AllowNew = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 231);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "&target:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(117, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 51);
            this.label1.TabIndex = 21;
            this.label1.Text = "Please select the source and destination of the synchronization process. You also" +
                " may need to provide a user name and a password for the source and/or the destin" +
                "ation.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.SyncProgress);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(516, 100);
            this.panel1.TabIndex = 20;
            this.panel1.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(18, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(477, 46);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "please wait ...";
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
            this.label2.Location = new System.Drawing.Point(12, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "pat&h:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "pat&h:";
            // 
            // txtPathSource
            // 
            this.txtPathSource.Location = new System.Drawing.Point(58, 155);
            this.txtPathSource.Name = "txtPathSource";
            this.txtPathSource.Size = new System.Drawing.Size(167, 20);
            this.txtPathSource.TabIndex = 3;
            // 
            // txtPathTarget
            // 
            this.txtPathTarget.Location = new System.Drawing.Point(58, 255);
            this.txtPathTarget.Name = "txtPathTarget";
            this.txtPathTarget.Size = new System.Drawing.Size(167, 20);
            this.txtPathTarget.TabIndex = 12;
            // 
            // btnPathTarget
            // 
            this.btnPathTarget.Location = new System.Drawing.Point(229, 253);
            this.btnPathTarget.Name = "btnPathTarget";
            this.btnPathTarget.Size = new System.Drawing.Size(27, 23);
            this.btnPathTarget.TabIndex = 13;
            this.btnPathTarget.Text = "...";
            this.btnPathTarget.UseVisualStyleBackColor = true;
            // 
            // btnPathSource
            // 
            this.btnPathSource.Location = new System.Drawing.Point(229, 153);
            this.btnPathSource.Name = "btnPathSource";
            this.btnPathSource.Size = new System.Drawing.Size(27, 23);
            this.btnPathSource.TabIndex = 4;
            this.btnPathSource.Text = "...";
            this.btnPathSource.UseVisualStyleBackColor = true;
            // 
            // folderBrowser
            // 
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // SyncWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 336);
            this.Controls.Add(this.btnPathSource);
            this.Controls.Add(this.btnPathTarget);
            this.Controls.Add(this.txtPathTarget);
            this.Controls.Add(this.txtPathSource);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboTarget);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "SyncWizard";
            this.Text = "SyncWizard";
            this.Load += new System.EventHandler(this.SyncWizard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contextDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextDataTarget)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboTarget;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
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
    }
}