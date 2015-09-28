namespace Sem.Sync.SharedUI.WinForms.UI
{
    partial class CaptchaResolve
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
            this.btnOk = new System.Windows.Forms.Button();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.txtResolved = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(331, 229);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // picCaptcha
            // 
            this.picCaptcha.Location = new System.Drawing.Point(12, 106);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(394, 117);
            this.picCaptcha.TabIndex = 1;
            this.picCaptcha.TabStop = false;
            // 
            // txtResolved
            // 
            this.txtResolved.Location = new System.Drawing.Point(12, 229);
            this.txtResolved.Name = "txtResolved";
            this.txtResolved.Size = new System.Drawing.Size(212, 20);
            this.txtResolved.TabIndex = 2;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(14, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(391, 84);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "The site you are working with does want you to solve a captcha. Simply type in th" +
                "e text you read in the image into the textbox and press ok";
            // 
            // CaptchaResolve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 261);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtResolved);
            this.Controls.Add(this.picCaptcha);
            this.Controls.Add(this.btnOk);
            this.Name = "CaptchaResolve";
            this.Text = "CaptchaResolve";
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.TextBox txtResolved;
        private System.Windows.Forms.Label lblMessage;
    }
}