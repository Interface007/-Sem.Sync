namespace Sem.Sync.SharedUI.WinForms.Controls
{
    partial class ContactCardView
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CardImage = new System.Windows.Forms.PictureBox();
            this.FullName = new System.Windows.Forms.Label();
            this.BusinessPosition = new System.Windows.Forms.Label();
            this.BusinessAddress = new Sem.Sync.SharedUI.WinForms.Controls.ContactAddressView();
            this.PrivateAddress = new Sem.Sync.SharedUI.WinForms.Controls.ContactAddressView();
            ((System.ComponentModel.ISupportInitialize)(this.CardImage)).BeginInit();
            this.SuspendLayout();
            // 
            // CardImage
            // 
            this.CardImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.CardImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CardImage.Location = new System.Drawing.Point(4, 57);
            this.CardImage.Name = "CardImage";
            this.CardImage.Size = new System.Drawing.Size(100, 143);
            this.CardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CardImage.TabIndex = 0;
            this.CardImage.TabStop = false;
            // 
            // FullName
            // 
            this.FullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FullName.BackColor = System.Drawing.Color.Ivory;
            this.FullName.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullName.Location = new System.Drawing.Point(6, 5);
            this.FullName.Name = "FullName";
            this.FullName.Size = new System.Drawing.Size(305, 36);
            this.FullName.TabIndex = 1;
            this.FullName.Text = "The full name of this person is extremely long";
            this.FullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BusinessPosition
            // 
            this.BusinessPosition.AutoSize = true;
            this.BusinessPosition.BackColor = System.Drawing.Color.Ivory;
            this.BusinessPosition.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BusinessPosition.Location = new System.Drawing.Point(3, 41);
            this.BusinessPosition.Name = "BusinessPosition";
            this.BusinessPosition.Size = new System.Drawing.Size(82, 13);
            this.BusinessPosition.TabIndex = 1;
            this.BusinessPosition.Text = "Businessman";
            // 
            // BusinessAddress
            // 
            this.BusinessAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BusinessAddress.BackColor = System.Drawing.Color.Ivory;
            this.BusinessAddress.Location = new System.Drawing.Point(106, 131);
            this.BusinessAddress.Name = "BusinessAddress";
            this.BusinessAddress.Size = new System.Drawing.Size(204, 69);
            this.BusinessAddress.TabIndex = 7;
            // 
            // PrivateAddress
            // 
            this.PrivateAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PrivateAddress.BackColor = System.Drawing.Color.Ivory;
            this.PrivateAddress.Location = new System.Drawing.Point(106, 57);
            this.PrivateAddress.Name = "PrivateAddress";
            this.PrivateAddress.Size = new System.Drawing.Size(204, 55);
            this.PrivateAddress.TabIndex = 6;
            // 
            // ContactCardView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.Controls.Add(this.BusinessAddress);
            this.Controls.Add(this.PrivateAddress);
            this.Controls.Add(this.BusinessPosition);
            this.Controls.Add(this.FullName);
            this.Controls.Add(this.CardImage);
            this.Name = "ContactCardView";
            this.Size = new System.Drawing.Size(316, 204);
            ((System.ComponentModel.ISupportInitialize)(this.CardImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CardImage;
        private System.Windows.Forms.Label FullName;
        private System.Windows.Forms.Label BusinessPosition;
        private ContactAddressView PrivateAddress;
        private ContactAddressView BusinessAddress;
    }
}