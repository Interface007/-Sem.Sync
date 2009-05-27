namespace Sem.Sync.SharedUI.WinForms.UI
{
    partial class MatchEntries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchEntries));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnFinished = new System.Windows.Forms.Button();
            this.btnMatch = new System.Windows.Forms.Button();
            this.chkMatchedOnly = new System.Windows.Forms.CheckBox();
            this.btnUnMatch = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView4);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView3);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
            // 
            // btnFinished
            // 
            resources.ApplyResources(this.btnFinished, "btnFinished");
            this.btnFinished.Name = "btnFinished";
            this.btnFinished.UseVisualStyleBackColor = true;
            // 
            // btnMatch
            // 
            resources.ApplyResources(this.btnMatch, "btnMatch");
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.UseVisualStyleBackColor = true;
            // 
            // chkMatchedOnly
            // 
            resources.ApplyResources(this.chkMatchedOnly, "chkMatchedOnly");
            this.chkMatchedOnly.Name = "chkMatchedOnly";
            this.chkMatchedOnly.UseVisualStyleBackColor = true;
            // 
            // btnUnMatch
            // 
            resources.ApplyResources(this.btnUnMatch, "btnUnMatch");
            this.btnUnMatch.Name = "btnUnMatch";
            this.btnUnMatch.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Name = "dataGridView1";
            // 
            // dataGridView2
            // 
            resources.ApplyResources(this.dataGridView2, "dataGridView2");
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Name = "dataGridView2";
            // 
            // dataGridView3
            // 
            resources.ApplyResources(this.dataGridView3, "dataGridView3");
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Name = "dataGridView3";
            // 
            // dataGridView4
            // 
            resources.ApplyResources(this.dataGridView4, "dataGridView4");
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Name = "dataGridView4";
            // 
            // MatchEntries
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkMatchedOnly);
            this.Controls.Add(this.btnUnMatch);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnFinished);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatchEntries";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnFinished;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.CheckBox chkMatchedOnly;
        private System.Windows.Forms.Button btnUnMatch;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}