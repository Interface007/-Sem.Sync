namespace Sem.Sync.OutlookWithXing.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonSyncWithXing = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.listLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonSyncWithXing
            // 
            this.buttonSyncWithXing.AccessibleDescription = null;
            this.buttonSyncWithXing.AccessibleName = null;
            resources.ApplyResources(this.buttonSyncWithXing, "buttonSyncWithXing");
            this.buttonSyncWithXing.BackgroundImage = null;
            this.buttonSyncWithXing.Font = null;
            this.buttonSyncWithXing.Name = "buttonSyncWithXing";
            this.buttonSyncWithXing.UseVisualStyleBackColor = true;
            this.buttonSyncWithXing.Click += new System.EventHandler(this.buttonSyncWithXing_Click);
            // 
            // progressBar
            // 
            this.progressBar.AccessibleDescription = null;
            this.progressBar.AccessibleName = null;
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.BackgroundImage = null;
            this.progressBar.Font = null;
            this.progressBar.Name = "progressBar";
            // 
            // listLog
            // 
            this.listLog.AccessibleDescription = null;
            this.listLog.AccessibleName = null;
            resources.ApplyResources(this.listLog, "listLog");
            this.listLog.BackgroundImage = null;
            this.listLog.Font = null;
            this.listLog.FormattingEnabled = true;
            this.listLog.Name = "listLog";
            // 
            // MainForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonSyncWithXing);
            this.Font = null;
            this.Icon = null;
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSyncWithXing;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox listLog;
    }
}