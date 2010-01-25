namespace Sem.Sync.LocalSyncManager.UI
{
    partial class Networks
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
            this.btnLocalStore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocalStore
            // 
            this.btnLocalStore.Location = new System.Drawing.Point(292, 173);
            this.btnLocalStore.Name = "btnLocalStore";
            this.btnLocalStore.Size = new System.Drawing.Size(75, 46);
            this.btnLocalStore.TabIndex = 0;
            this.btnLocalStore.Text = "local store";
            this.btnLocalStore.UseVisualStyleBackColor = true;
            // 
            // Networks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 458);
            this.Controls.Add(this.btnLocalStore);
            this.Name = "Networks";
            this.Text = "Networks";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocalStore;

    }
}