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
            this.versionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSyncWithXing
            // 
            resources.ApplyResources(this.buttonSyncWithXing, "buttonSyncWithXing");
            this.buttonSyncWithXing.Name = "buttonSyncWithXing";
            this.buttonSyncWithXing.UseVisualStyleBackColor = true;
            this.buttonSyncWithXing.Click += new System.EventHandler(this.ButtonSyncWithXing_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // listLog
            // 
            resources.ApplyResources(this.listLog, "listLog");
            this.listLog.FormattingEnabled = true;
            this.listLog.Name = "listLog";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonSyncWithXing);
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSyncWithXing;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox listLog;
        private System.Windows.Forms.Label versionLabel;
    }
}