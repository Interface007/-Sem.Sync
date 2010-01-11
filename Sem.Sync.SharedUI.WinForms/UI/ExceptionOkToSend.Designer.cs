namespace Sem.Sync.SharedUI.WinForms.UI
{
    partial class ExceptionOkToSend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionOkToSend));
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Content = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.AccessibleDescription = null;
            this.CancelButton.AccessibleName = null;
            resources.ApplyResources(this.CancelButton, "CancelButton");
            this.CancelButton.BackgroundImage = null;
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.CancelButton.Font = null;
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.AccessibleDescription = null;
            this.OkButton.AccessibleName = null;
            resources.ApplyResources(this.OkButton, "OkButton");
            this.OkButton.BackgroundImage = null;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.OkButton.Font = null;
            this.OkButton.Name = "OkButton";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // Content
            // 
            this.Content.AccessibleDescription = null;
            this.Content.AccessibleName = null;
            resources.ApplyResources(this.Content, "Content");
            this.Content.BackgroundImage = null;
            this.Content.Font = null;
            this.Content.Name = "Content";
            // 
            // ExceptionOkToSend
            // 
            this.AcceptButton = this.OkButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.Content);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelButton);
            this.Font = null;
            this.Icon = null;
            this.Name = "ExceptionOkToSend";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Content;
    }
}