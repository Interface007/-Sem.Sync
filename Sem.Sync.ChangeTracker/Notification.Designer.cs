namespace Sem.Sync.ChangeTracker
{
    partial class Notification
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
            this.FadeOutTimer = new System.Windows.Forms.Timer(this.components);
            this.ContactImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ContactImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ContactImage
            // 
            this.ContactImage.Location = new System.Drawing.Point(0, 0);
            this.ContactImage.Name = "ContactImage";
            this.ContactImage.Size = new System.Drawing.Size(56, 64);
            this.ContactImage.TabIndex = 0;
            this.ContactImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(62, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 64);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 68);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContactImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Notification";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Notification";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ContactImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer FadeOutTimer;
        private System.Windows.Forms.PictureBox ContactImage;
        private System.Windows.Forms.Label label1;
    }
}