namespace Sem.Sync.LocalSyncManager.UI
{
    using SharedUI.WinForms.Controls;
    
    partial class SyncDetails
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.contactCardView1 = new Sem.Sync.SharedUI.WinForms.Controls.ContactCardView();
            this.contactCardView2 = new Sem.Sync.SharedUI.WinForms.Controls.ContactCardView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contactCardView1);
            this.panel1.Controls.Add(this.contactCardView2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 501);
            this.panel1.TabIndex = 0;
            // 
            // contactCardView1
            // 
            this.contactCardView1.BackColor = System.Drawing.Color.White;
            this.contactCardView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contactCardView1.Location = new System.Drawing.Point(12, 12);
            this.contactCardView1.Name = "contactCardView1";
            this.contactCardView1.Size = new System.Drawing.Size(303, 193);
            this.contactCardView1.TabIndex = 0;
            // 
            // contactCardView2
            // 
            this.contactCardView2.BackColor = System.Drawing.Color.White;
            this.contactCardView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contactCardView2.Location = new System.Drawing.Point(321, 12);
            this.contactCardView2.Name = "contactCardView2";
            this.contactCardView2.Size = new System.Drawing.Size(303, 193);
            this.contactCardView2.TabIndex = 0;
            // 
            // SyncDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 500);
            this.Controls.Add(this.panel1);
            this.Name = "SyncDetails";
            this.Text = "SyncDetails";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ContactCardView contactCardView2;
        private ContactCardView contactCardView1;
    }
}