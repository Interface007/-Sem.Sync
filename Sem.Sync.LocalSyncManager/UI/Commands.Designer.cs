namespace Sem.Sync.LocalSyncManager.UI
{
    using Sem.Sync.SyncBase.Binding;

    partial class Commands
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Commands));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LogList = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceConnector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceStorePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetConnector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetStorePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommandParameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.syncListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.runSelected = new System.Windows.Forms.Button();
            this.runAll = new System.Windows.Forms.Button();
            this.SyncListSelection = new System.Windows.Forms.ComboBox();
            this.syncListsSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wizardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.syncListBindingSource)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.syncListsSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LogList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            // 
            // LogList
            // 
            resources.ApplyResources(this.LogList, "LogList");
            this.LogList.FormattingEnabled = true;
            this.LogList.Name = "LogList";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.Command,
            this.SourceConnector,
            this.SourceStorePath,
            this.TargetConnector,
            this.TargetStorePath,
            this.CommandParameter});
            this.dataGridView1.DataSource = this.syncListBindingSource;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn, "nameDataGridViewTextBoxColumn");
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Command
            // 
            this.Command.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Command.DataPropertyName = "Command";
            resources.ApplyResources(this.Command, "Command");
            this.Command.Name = "Command";
            this.Command.ReadOnly = true;
            // 
            // SourceConnector
            // 
            this.SourceConnector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SourceConnector.DataPropertyName = "SourceConnector";
            resources.ApplyResources(this.SourceConnector, "SourceConnector");
            this.SourceConnector.Name = "SourceConnector";
            this.SourceConnector.ReadOnly = true;
            // 
            // SourceStorePath
            // 
            this.SourceStorePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SourceStorePath.DataPropertyName = "SourceStorePath";
            resources.ApplyResources(this.SourceStorePath, "SourceStorePath");
            this.SourceStorePath.Name = "SourceStorePath";
            this.SourceStorePath.ReadOnly = true;
            // 
            // TargetConnector
            // 
            this.TargetConnector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TargetConnector.DataPropertyName = "TargetConnector";
            resources.ApplyResources(this.TargetConnector, "TargetConnector");
            this.TargetConnector.Name = "TargetConnector";
            this.TargetConnector.ReadOnly = true;
            // 
            // TargetStorePath
            // 
            this.TargetStorePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TargetStorePath.DataPropertyName = "TargetStorePath";
            resources.ApplyResources(this.TargetStorePath, "TargetStorePath");
            this.TargetStorePath.Name = "TargetStorePath";
            this.TargetStorePath.ReadOnly = true;
            // 
            // CommandParameter
            // 
            this.CommandParameter.DataPropertyName = "CommandParameter";
            resources.ApplyResources(this.CommandParameter, "CommandParameter");
            this.CommandParameter.Name = "CommandParameter";
            this.CommandParameter.ReadOnly = true;
            // 
            // syncListBindingSource
            // 
            this.syncListBindingSource.DataSource = typeof(Sem.Sync.SyncBase.Binding.SyncCollection);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.StatusLabel});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.Name = "StatusLabel";
            // 
            // runSelected
            // 
            resources.ApplyResources(this.runSelected, "runSelected");
            this.runSelected.Name = "runSelected";
            this.runSelected.UseVisualStyleBackColor = true;
            this.runSelected.Click += new System.EventHandler(this.RunSelected_Click);
            // 
            // runAll
            // 
            resources.ApplyResources(this.runAll, "runAll");
            this.runAll.Name = "runAll";
            this.runAll.UseVisualStyleBackColor = true;
            this.runAll.Click += new System.EventHandler(this.RunAll_Click);
            // 
            // SyncListSelection
            // 
            resources.ApplyResources(this.SyncListSelection, "SyncListSelection");
            this.SyncListSelection.DataSource = this.syncListsSource;
            this.SyncListSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SyncListSelection.FormattingEnabled = true;
            this.SyncListSelection.Name = "SyncListSelection";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.wizardsToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorkingFolderToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openWorkingFolderToolStripMenuItem
            // 
            this.openWorkingFolderToolStripMenuItem.Name = "openWorkingFolderToolStripMenuItem";
            resources.ApplyResources(this.openWorkingFolderToolStripMenuItem, "openWorkingFolderToolStripMenuItem");
            this.openWorkingFolderToolStripMenuItem.Click += new System.EventHandler(this.openWorkingFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // wizardsToolStripMenuItem
            // 
            this.wizardsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simpleSyncToolStripMenuItem});
            this.wizardsToolStripMenuItem.Name = "wizardsToolStripMenuItem";
            resources.ApplyResources(this.wizardsToolStripMenuItem, "wizardsToolStripMenuItem");
            // 
            // simpleSyncToolStripMenuItem
            // 
            this.simpleSyncToolStripMenuItem.Name = "simpleSyncToolStripMenuItem";
            resources.ApplyResources(this.simpleSyncToolStripMenuItem, "simpleSyncToolStripMenuItem");
            this.simpleSyncToolStripMenuItem.Click += new System.EventHandler(this.simpleSyncToolStripMenuItem_Click);
            // 
            // Commands
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SyncListSelection);
            this.Controls.Add(this.runAll);
            this.Controls.Add(this.runSelected);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Commands";
            this.Load += new System.EventHandler(this.LocalSync_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.syncListBindingSource)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.syncListsSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LogList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource syncListBindingSource;
        private System.Windows.Forms.Button runSelected;
        private System.Windows.Forms.Button runAll;
        private System.Windows.Forms.ComboBox SyncListSelection;
        private System.Windows.Forms.BindingSource syncListsSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceConnector;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceStorePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetConnector;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetStorePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandParameter;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wizardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleSyncToolStripMenuItem;
    }
}