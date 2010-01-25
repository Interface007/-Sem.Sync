namespace Sem.Sync.SharedUI.WinForms.UI
{
    partial class Disclaimer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Disclaimer));
            this.no = new System.Windows.Forms.Button();
            this.yes = new System.Windows.Forms.Button();
            this.warning = new System.Windows.Forms.Label();
            this.iDoUnterstand = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // no
            // 
            this.no.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.no, "no");
            this.no.Name = "no";
            this.no.UseVisualStyleBackColor = true;
            this.no.Click += new System.EventHandler(this.No_Click);
            // 
            // yes
            // 
            resources.ApplyResources(this.yes, "yes");
            this.yes.Name = "yes";
            this.yes.UseVisualStyleBackColor = true;
            this.yes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // warning
            // 
            this.warning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.warning, "warning");
            this.warning.Name = "warning";
            // 
            // iDoUnterstand
            // 
            resources.ApplyResources(this.iDoUnterstand, "iDoUnterstand");
            this.iDoUnterstand.Name = "iDoUnterstand";
            this.iDoUnterstand.UseVisualStyleBackColor = true;
            this.iDoUnterstand.CheckedChanged += new System.EventHandler(this.IDoUnterstand_CheckedChanged);
            // 
            // Disclaimer
            // 
            this.AcceptButton = this.yes;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.no;
            this.ControlBox = false;
            this.Controls.Add(this.iDoUnterstand);
            this.Controls.Add(this.warning);
            this.Controls.Add(this.yes);
            this.Controls.Add(this.no);
            this.Name = "Disclaimer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button no;
        private System.Windows.Forms.Button yes;
        private System.Windows.Forms.Label warning;
        private System.Windows.Forms.CheckBox iDoUnterstand;
    }
}