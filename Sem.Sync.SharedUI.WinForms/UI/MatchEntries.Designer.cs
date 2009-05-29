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
            this.dataGridSourceDetail = new System.Windows.Forms.DataGridView();
            this.dataGridSourceCandidates = new System.Windows.Forms.DataGridView();
            this.dataGridTargetDetail = new System.Windows.Forms.DataGridView();
            this.dataGridTargetCandidates = new System.Windows.Forms.DataGridView();
            this.btnFinished = new System.Windows.Forms.Button();
            this.btnMatch = new System.Windows.Forms.Button();
            this.chkMatchedOnly = new System.Windows.Forms.CheckBox();
            this.btnUnMatch = new System.Windows.Forms.Button();
            this.dataGridMatches = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceCandidates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTargetDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTargetCandidates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMatches)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridSourceDetail);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridSourceCandidates);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridTargetDetail);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridTargetCandidates);
            // 
            // dataGridSourceDetail
            // 
            this.dataGridSourceDetail.AllowUserToAddRows = false;
            this.dataGridSourceDetail.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridSourceDetail, "dataGridSourceDetail");
            this.dataGridSourceDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSourceDetail.Name = "dataGridSourceDetail";
            this.dataGridSourceDetail.ReadOnly = true;
            // 
            // dataGridSourceCandidates
            // 
            this.dataGridSourceCandidates.AllowUserToAddRows = false;
            this.dataGridSourceCandidates.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridSourceCandidates, "dataGridSourceCandidates");
            this.dataGridSourceCandidates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSourceCandidates.Name = "dataGridSourceCandidates";
            this.dataGridSourceCandidates.ReadOnly = true;
            this.dataGridSourceCandidates.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridSourceCandidatesCellEnter);
            // 
            // dataGridTargetDetail
            // 
            this.dataGridTargetDetail.AllowUserToAddRows = false;
            this.dataGridTargetDetail.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridTargetDetail, "dataGridTargetDetail");
            this.dataGridTargetDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTargetDetail.Name = "dataGridTargetDetail";
            this.dataGridTargetDetail.ReadOnly = true;
            // 
            // dataGridTargetCandidates
            // 
            this.dataGridTargetCandidates.AllowUserToAddRows = false;
            this.dataGridTargetCandidates.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridTargetCandidates, "dataGridTargetCandidates");
            this.dataGridTargetCandidates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTargetCandidates.Name = "dataGridTargetCandidates";
            this.dataGridTargetCandidates.ReadOnly = true;
            this.dataGridTargetCandidates.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridTargetCandidatesCellEnter);
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
            // dataGridMatches
            // 
            this.dataGridMatches.AllowUserToAddRows = false;
            this.dataGridMatches.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dataGridMatches, "dataGridMatches");
            this.dataGridMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMatches.Name = "dataGridMatches";
            this.dataGridMatches.ReadOnly = true;
            // 
            // MatchEntries
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridMatches);
            this.Controls.Add(this.chkMatchedOnly);
            this.Controls.Add(this.btnUnMatch);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnFinished);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatchEntries";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSourceCandidates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTargetDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTargetCandidates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMatches)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnFinished;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.CheckBox chkMatchedOnly;
        private System.Windows.Forms.Button btnUnMatch;
        private System.Windows.Forms.DataGridView dataGridSourceDetail;
        private System.Windows.Forms.DataGridView dataGridSourceCandidates;
        private System.Windows.Forms.DataGridView dataGridTargetDetail;
        private System.Windows.Forms.DataGridView dataGridTargetCandidates;
        private System.Windows.Forms.DataGridView dataGridMatches;
    }
}