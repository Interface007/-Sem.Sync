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
            this.acknowledged = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ContactImage)).BeginInit();
            this.SuspendLayout();
            // 
            // FadeOutTimer
            // 
            this.FadeOutTimer.Enabled = true;
            this.FadeOutTimer.Interval = 200;
            this.FadeOutTimer.Tick += new System.EventHandler(this.FadeOutTimer_Tick);
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
            this.label1.Size = new System.Drawing.Size(218, 44);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // acknowledged
            // 
            this.acknowledged.AutoSize = true;
            this.acknowledged.Location = new System.Drawing.Point(63, 48);
            this.acknowledged.Name = "acknowledged";
            this.acknowledged.Size = new System.Drawing.Size(96, 17);
            this.acknowledged.TabIndex = 2;
            this.acknowledged.Text = "acknowledged";
            this.acknowledged.UseVisualStyleBackColor = true;
            this.acknowledged.CheckedChanged += new System.EventHandler(this.Acknowledged_CheckedChanged);
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 68);
            this.ControlBox = false;
            this.Controls.Add(this.acknowledged);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContactImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Notification";
            this.Opacity = 0;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Notification";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ContactImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer FadeOutTimer;
        private System.Windows.Forms.PictureBox ContactImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox acknowledged;
    }
}