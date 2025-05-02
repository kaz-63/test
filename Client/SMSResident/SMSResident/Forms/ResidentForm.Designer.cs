namespace SMSResident.Forms
{
    partial class ResidentForm
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
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tbpNichiji = new System.Windows.Forms.TabPage();
            this.tbpMail = new System.Windows.Forms.TabPage();
            this.tbpSKSRenkei = new System.Windows.Forms.TabPage();
            this.shimeResidentMonitor1 = new SMSResident.Controls.ShimeResidentMonitor();
            this.mailSendResidentMonitor1 = new SMSResident.Controls.MailSendResidentMonitor();
            this.sksRenkeiResidentMonitor1 = new SMSResident.Controls.SKSRenkeiResidentMonitor();
            this.pnlMain.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tbpNichiji.SuspendLayout();
            this.tbpMail.SuspendLayout();
            this.tbpSKSRenkei.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tbcMain);
            this.pnlMain.Controls.SetChildIndex(this.tbcMain, 0);
            this.pnlMain.Controls.SetChildIndex(this.lblCorner, 0);
            this.pnlMain.Controls.SetChildIndex(this.fbrFunction, 0);
            // 
            // fbrFunction
            // 
            // 
            // fbrFunction.F06Button
            // 
            this.fbrFunction.F06Button.Text = "Clear";
            this.fbrFunction.F06ButtonText = "Clear";
            // 
            // pnlTitleSpace
            // 
            this.pnlTitleSpace.Location = new System.Drawing.Point(18, 4);
            this.pnlTitleSpace.Size = new System.Drawing.Size(684, 28);
            // 
            // tbcMain
            // 
            this.tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcMain.Controls.Add(this.tbpNichiji);
            this.tbcMain.Controls.Add(this.tbpMail);
            this.tbcMain.Controls.Add(this.tbpSKSRenkei);
            this.tbcMain.Location = new System.Drawing.Point(12, 6);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(922, 529);
            this.tbcMain.TabIndex = 2;
            // 
            // tbpNichiji
            // 
            this.tbpNichiji.Controls.Add(this.shimeResidentMonitor1);
            this.tbpNichiji.Location = new System.Drawing.Point(4, 22);
            this.tbpNichiji.Name = "tbpNichiji";
            this.tbpNichiji.Padding = new System.Windows.Forms.Padding(3);
            this.tbpNichiji.Size = new System.Drawing.Size(914, 503);
            this.tbpNichiji.TabIndex = 0;
            this.tbpNichiji.Text = "日次";
            this.tbpNichiji.UseVisualStyleBackColor = true;
            // 
            // tbpMail
            // 
            this.tbpMail.Controls.Add(this.mailSendResidentMonitor1);
            this.tbpMail.Location = new System.Drawing.Point(4, 22);
            this.tbpMail.Name = "tbpMail";
            this.tbpMail.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMail.Size = new System.Drawing.Size(914, 503);
            this.tbpMail.TabIndex = 1;
            this.tbpMail.Text = "メール送信";
            this.tbpMail.UseVisualStyleBackColor = true;
            // 
            // tbpSKSRenkei
            // 
            this.tbpSKSRenkei.Controls.Add(this.sksRenkeiResidentMonitor1);
            this.tbpSKSRenkei.Location = new System.Drawing.Point(4, 22);
            this.tbpSKSRenkei.Name = "tbpSKSRenkei";
            this.tbpSKSRenkei.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSKSRenkei.Size = new System.Drawing.Size(914, 503);
            this.tbpSKSRenkei.TabIndex = 2;
            this.tbpSKSRenkei.Text = "SKS連携";
            this.tbpSKSRenkei.UseVisualStyleBackColor = true;
            // 
            // shimeResidentMonitor1
            // 
            this.shimeResidentMonitor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shimeResidentMonitor1.CheckTime = 20;
            this.shimeResidentMonitor1.Location = new System.Drawing.Point(6, 6);
            this.shimeResidentMonitor1.Name = "shimeResidentMonitor1";
            this.shimeResidentMonitor1.Size = new System.Drawing.Size(902, 494);
            this.shimeResidentMonitor1.TabIndex = 0;
            // 
            // mailSendResidentMonitor1
            // 
            this.mailSendResidentMonitor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mailSendResidentMonitor1.CheckTime = 30;
            this.mailSendResidentMonitor1.Location = new System.Drawing.Point(6, 6);
            this.mailSendResidentMonitor1.Name = "mailSendResidentMonitor1";
            this.mailSendResidentMonitor1.Size = new System.Drawing.Size(902, 494);
            this.mailSendResidentMonitor1.TabIndex = 0;
            // 
            // sksRenkeiResidentMonitor1
            // 
            this.sksRenkeiResidentMonitor1.CheckTime = 30;
            this.sksRenkeiResidentMonitor1.Location = new System.Drawing.Point(6, 6);
            this.sksRenkeiResidentMonitor1.Name = "sksRenkeiResidentMonitor1";
            this.sksRenkeiResidentMonitor1.Size = new System.Drawing.Size(902, 494);
            this.sksRenkeiResidentMonitor1.TabIndex = 0;
            // 
            // ResidentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 664);
            this.Name = "ResidentForm";
            this.Text = "ResidentForm";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.tbcMain.ResumeLayout(false);
            this.tbpNichiji.ResumeLayout(false);
            this.tbpMail.ResumeLayout(false);
            this.tbpSKSRenkei.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpNichiji;
        private SMSResident.Controls.ShimeResidentMonitor shimeResidentMonitor1;
        private System.Windows.Forms.TabPage tbpMail;
        private SMSResident.Controls.MailSendResidentMonitor mailSendResidentMonitor1;
        private System.Windows.Forms.TabPage tbpSKSRenkei;
        private SMSResident.Controls.SKSRenkeiResidentMonitor sksRenkeiResidentMonitor1;
    }
}