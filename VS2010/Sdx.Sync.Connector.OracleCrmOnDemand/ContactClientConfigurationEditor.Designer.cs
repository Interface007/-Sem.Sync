namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    partial class ContactClientConfigurationEditor
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
            this.OkButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.Filter = new System.Windows.Forms.ListBox();
            this.Argument = new System.Windows.Forms.TextBox();
            this.Condition = new System.Windows.Forms.ComboBox();
            this.Field = new System.Windows.Forms.ComboBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.ReadAllAttributes = new System.Windows.Forms.CheckBox();
            this.IgnoreCertificateErrors = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Remove = new System.Windows.Forms.Button();
            this.PageSize = new System.Windows.Forms.MaskedTextBox();
            this.filterForTestAccount = new System.Windows.Forms.CheckBox();
            this.filterForMapAccounts = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(472, 281);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(93, 28);
            this.OkButton.TabIndex = 10;
            this.OkButton.Text = "&Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AbortButton.Location = new System.Drawing.Point(373, 281);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(93, 28);
            this.AbortButton.TabIndex = 9;
            this.AbortButton.Text = "&Cancel";
            this.AbortButton.UseVisualStyleBackColor = true;
            // 
            // Filter
            // 
            this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Filter.FormattingEnabled = true;
            this.Filter.Location = new System.Drawing.Point(12, 39);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(506, 134);
            this.Filter.TabIndex = 4;
            // 
            // Argument
            // 
            this.Argument.Location = new System.Drawing.Point(300, 12);
            this.Argument.Name = "Argument";
            this.Argument.Size = new System.Drawing.Size(218, 20);
            this.Argument.TabIndex = 2;
            // 
            // Condition
            // 
            this.Condition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Condition.FormattingEnabled = true;
            this.Condition.Items.AddRange(new object[] {
            "~LIKE",
            "~=",
            "LIKE",
            "=",
            "<",
            "<=",
            ">",
            ">=",
            "<>"});
            this.Condition.Location = new System.Drawing.Point(202, 12);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(92, 21);
            this.Condition.TabIndex = 1;
            // 
            // Field
            // 
            this.Field.FormattingEnabled = true;
            this.Field.Location = new System.Drawing.Point(12, 12);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(184, 21);
            this.Field.TabIndex = 0;
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.Location = new System.Drawing.Point(524, 10);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(41, 23);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "a&dd";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // ReadAllAttributes
            // 
            this.ReadAllAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReadAllAttributes.AutoSize = true;
            this.ReadAllAttributes.Location = new System.Drawing.Point(12, 179);
            this.ReadAllAttributes.Name = "ReadAllAttributes";
            this.ReadAllAttributes.Size = new System.Drawing.Size(156, 17);
            this.ReadAllAttributes.TabIndex = 5;
            this.ReadAllAttributes.Text = "read &all attributes from CRM";
            this.ReadAllAttributes.UseVisualStyleBackColor = true;
            // 
            // IgnoreCertificateErrors
            // 
            this.IgnoreCertificateErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IgnoreCertificateErrors.AutoSize = true;
            this.IgnoreCertificateErrors.Location = new System.Drawing.Point(12, 202);
            this.IgnoreCertificateErrors.Name = "IgnoreCertificateErrors";
            this.IgnoreCertificateErrors.Size = new System.Drawing.Size(133, 17);
            this.IgnoreCertificateErrors.TabIndex = 6;
            this.IgnoreCertificateErrors.Text = "&ignore certificate errors";
            this.IgnoreCertificateErrors.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "&Page size:";
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(524, 39);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(41, 23);
            this.Remove.TabIndex = 11;
            this.Remove.Text = "d&el";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // PageSize
            // 
            this.PageSize.Location = new System.Drawing.Point(70, 225);
            this.PageSize.Mask = "000";
            this.PageSize.Name = "PageSize";
            this.PageSize.Size = new System.Drawing.Size(37, 20);
            this.PageSize.TabIndex = 12;
            this.PageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // filterForTestAccount
            // 
            this.filterForTestAccount.AutoSize = true;
            this.filterForTestAccount.Location = new System.Drawing.Point(202, 179);
            this.filterForTestAccount.Name = "filterForTestAccount";
            this.filterForTestAccount.Size = new System.Drawing.Size(192, 17);
            this.filterForTestAccount.TabIndex = 13;
            this.filterForTestAccount.Text = "filter for account \'Z. SDX AG (Test)\'";
            this.filterForTestAccount.UseVisualStyleBackColor = true;
            // 
            // filterForMapAccounts
            // 
            this.filterForMapAccounts.AutoSize = true;
            this.filterForMapAccounts.Location = new System.Drawing.Point(202, 202);
            this.filterForMapAccounts.Name = "filterForMapAccounts";
            this.filterForMapAccounts.Size = new System.Drawing.Size(175, 17);
            this.filterForMapAccounts.TabIndex = 13;
            this.filterForMapAccounts.Text = "filter for account MAP accounts";
            this.filterForMapAccounts.UseVisualStyleBackColor = true;
            // 
            // ContactClientConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.AbortButton;
            this.ClientSize = new System.Drawing.Size(577, 321);
            this.Controls.Add(this.filterForMapAccounts);
            this.Controls.Add(this.filterForTestAccount);
            this.Controls.Add(this.PageSize);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IgnoreCertificateErrors);
            this.Controls.Add(this.ReadAllAttributes);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.Field);
            this.Controls.Add(this.Condition);
            this.Controls.Add(this.Argument);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.OkButton);
            this.Name = "ContactClientConfigurationEditor";
            this.Text = "Contact Client Configuration Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.ListBox Filter;
        private System.Windows.Forms.TextBox Argument;
        private System.Windows.Forms.ComboBox Condition;
        private System.Windows.Forms.ComboBox Field;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.CheckBox ReadAllAttributes;
        private System.Windows.Forms.CheckBox IgnoreCertificateErrors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.MaskedTextBox PageSize;
        private System.Windows.Forms.CheckBox filterForTestAccount;
        private System.Windows.Forms.CheckBox filterForMapAccounts;
    }
}