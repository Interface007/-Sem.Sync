namespace Sem.Sync.SharedUI.WinForms.Controls
{
    partial class ContactAddressView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StreetName = new System.Windows.Forms.Label();
            this.Room = new System.Windows.Forms.Label();
            this.Phone = new System.Windows.Forms.Label();
            this.CityName = new System.Windows.Forms.Label();
            this.PostalCode = new System.Windows.Forms.Label();
            this.StateName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StreetName
            // 
            this.StreetName.AutoSize = true;
            this.StreetName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StreetName.Location = new System.Drawing.Point(3, 0);
            this.StreetName.Name = "StreetName";
            this.StreetName.Size = new System.Drawing.Size(122, 13);
            this.StreetName.TabIndex = 0;
            this.StreetName.Text = "Sesamstreet 4697 a";
            // 
            // Room
            // 
            this.Room.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Room.AutoSize = true;
            this.Room.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Room.Location = new System.Drawing.Point(121, 0);
            this.Room.Name = "Room";
            this.Room.Size = new System.Drawing.Size(40, 13);
            this.Room.TabIndex = 0;
            this.Room.Text = "Room";
            // 
            // Phone
            // 
            this.Phone.AutoSize = true;
            this.Phone.Font = new System.Drawing.Font("Courier New", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phone.Location = new System.Drawing.Point(3, 39);
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(105, 15);
            this.Phone.TabIndex = 0;
            this.Phone.Text = "+49-6441-30336";
            // 
            // CityName
            // 
            this.CityName.AutoSize = true;
            this.CityName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CityName.Location = new System.Drawing.Point(52, 13);
            this.CityName.Name = "CityName";
            this.CityName.Size = new System.Drawing.Size(75, 13);
            this.CityName.TabIndex = 0;
            this.CityName.Text = "Los Angeles";
            // 
            // PostalCode
            // 
            this.PostalCode.AutoSize = true;
            this.PostalCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PostalCode.Location = new System.Drawing.Point(3, 13);
            this.PostalCode.Name = "PostalCode";
            this.PostalCode.Size = new System.Drawing.Size(42, 13);
            this.PostalCode.TabIndex = 0;
            this.PostalCode.Text = "35586";
            // 
            // StateName
            // 
            this.StateName.AutoSize = true;
            this.StateName.Font = new System.Drawing.Font("Verdana", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StateName.Location = new System.Drawing.Point(4, 27);
            this.StateName.Name = "StateName";
            this.StateName.Size = new System.Drawing.Size(36, 12);
            this.StateName.TabIndex = 0;
            this.StateName.Text = "Texas";
            // 
            // ContactAddressView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StateName);
            this.Controls.Add(this.PostalCode);
            this.Controls.Add(this.CityName);
            this.Controls.Add(this.Phone);
            this.Controls.Add(this.Room);
            this.Controls.Add(this.StreetName);
            this.Name = "ContactAddressView";
            this.Size = new System.Drawing.Size(177, 56);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StreetName;
        private System.Windows.Forms.Label Room;
        private System.Windows.Forms.Label Phone;
        private System.Windows.Forms.Label CityName;
        private System.Windows.Forms.Label PostalCode;
        private System.Windows.Forms.Label StateName;

    }
}
