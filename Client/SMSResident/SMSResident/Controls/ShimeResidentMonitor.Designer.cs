namespace SMSResident.Controls
{
    partial class ShimeResidentMonitor
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNichijiDate = new DSWControl.DSWLabel.DSWLabel();
            this.txtNichijiDate = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblResidentStatus.ChildPanel.SuspendLayout();
            this.lblResidentStatus.SuspendLayout();
            this.lblNichijiDate.ChildPanel.SuspendLayout();
            this.lblNichijiDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResidentStatus
            // 
            this.lblResidentStatus.Location = new System.Drawing.Point(248, 4);
            this.lblResidentStatus.Visible = false;
            // 
            // lstMessage
            // 
            this.lstMessage.Location = new System.Drawing.Point(0, 35);
            this.lstMessage.Size = new System.Drawing.Size(486, 196);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(326, 3);
            this.btnSetting.Visible = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(407, 3);
            this.btnClear.Visible = false;
            // 
            // lblNichijiDate
            // 
            // 
            // lblNichijiDate.ChildPanel
            // 
            this.lblNichijiDate.ChildPanel.Controls.Add(this.txtNichijiDate);
            this.lblNichijiDate.IsFocusChangeColor = false;
            this.lblNichijiDate.LabelWidth = 110;
            this.lblNichijiDate.Location = new System.Drawing.Point(3, 4);
            this.lblNichijiDate.Name = "lblNichijiDate";
            this.lblNichijiDate.Size = new System.Drawing.Size(248, 19);
            this.lblNichijiDate.SplitterWidth = 0;
            this.lblNichijiDate.TabIndex = 5;
            this.lblNichijiDate.Text = "前回日次実行日時";
            // 
            // txtNichijiDate
            // 
            this.txtNichijiDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNichijiDate.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtNichijiDate.InputRegulation = "";
            this.txtNichijiDate.Location = new System.Drawing.Point(0, 0);
            this.txtNichijiDate.Name = "txtNichijiDate";
            this.txtNichijiDate.PadType = DSWControl.DSWTextBox.DSWTextBox.RightLeftType.Left;
            this.txtNichijiDate.ProhibitionChar = null;
            this.txtNichijiDate.ReadOnly = true;
            this.txtNichijiDate.Size = new System.Drawing.Size(138, 19);
            this.txtNichijiDate.TabIndex = 0;
            // 
            // ShimeResidentMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.lblNichijiDate);
            this.Name = "ShimeResidentMonitor";
            this.Controls.SetChildIndex(this.lblNichijiDate, 0);
            this.Controls.SetChildIndex(this.lblResidentStatus, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.lstMessage, 0);
            this.lblResidentStatus.ChildPanel.ResumeLayout(false);
            this.lblResidentStatus.ChildPanel.PerformLayout();
            this.lblResidentStatus.ResumeLayout(false);
            this.lblNichijiDate.ChildPanel.ResumeLayout(false);
            this.lblNichijiDate.ChildPanel.PerformLayout();
            this.lblNichijiDate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblNichijiDate;
        private DSWControl.DSWTextBox.DSWTextBox txtNichijiDate;
    }
}
