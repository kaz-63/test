namespace SMSResident.Controls
{
    partial class SKSRenkeiResidentMonitor
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSKSRenkeiDate = new DSWControl.DSWLabel.DSWLabel();
            this.txtSKSRenkeiDate = new DSWControl.DSWTextBox.DSWTextBox();
            this.lblResidentStatus.ChildPanel.SuspendLayout();
            this.lblResidentStatus.SuspendLayout();
            this.lblSKSRenkeiDate.ChildPanel.SuspendLayout();
            this.lblSKSRenkeiDate.SuspendLayout();
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
            // lblSKSRenkeiDate
            // 
            // 
            // lblSKSRenkeiDate.ChildPanel
            // 
            this.lblSKSRenkeiDate.ChildPanel.Controls.Add(this.txtSKSRenkeiDate);
            this.lblSKSRenkeiDate.IsFocusChangeColor = false;
            this.lblSKSRenkeiDate.LabelWidth = 110;
            this.lblSKSRenkeiDate.Location = new System.Drawing.Point(3, 4);
            this.lblSKSRenkeiDate.Name = "lblSKSRenkeiDate";
            this.lblSKSRenkeiDate.Size = new System.Drawing.Size(248, 19);
            this.lblSKSRenkeiDate.SplitterWidth = 0;
            this.lblSKSRenkeiDate.TabIndex = 6;
            this.lblSKSRenkeiDate.Text = "前回連携実行日時";
            // 
            // txtSKSRenkeiDate
            // 
            this.txtSKSRenkeiDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSKSRenkeiDate.FocusBackColor = System.Drawing.SystemColors.Window;
            this.txtSKSRenkeiDate.InputRegulation = "";
            this.txtSKSRenkeiDate.Location = new System.Drawing.Point(0, 0);
            this.txtSKSRenkeiDate.Name = "txtSKSRenkeiDate";
            this.txtSKSRenkeiDate.ProhibitionChar = null;
            this.txtSKSRenkeiDate.ReadOnly = true;
            this.txtSKSRenkeiDate.Size = new System.Drawing.Size(138, 19);
            this.txtSKSRenkeiDate.TabIndex = 0;
            // 
            // SKSRenkeiResidentMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSKSRenkeiDate);
            this.Name = "SKSRenkeiResidentMonitor";
            this.Controls.SetChildIndex(this.lblResidentStatus, 0);
            this.Controls.SetChildIndex(this.lstMessage, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.lblSKSRenkeiDate, 0);
            this.lblResidentStatus.ChildPanel.ResumeLayout(false);
            this.lblResidentStatus.ChildPanel.PerformLayout();
            this.lblResidentStatus.ResumeLayout(false);
            this.lblSKSRenkeiDate.ChildPanel.ResumeLayout(false);
            this.lblSKSRenkeiDate.ChildPanel.PerformLayout();
            this.lblSKSRenkeiDate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DSWControl.DSWLabel.DSWLabel lblSKSRenkeiDate;
        private DSWControl.DSWTextBox.DSWTextBox txtSKSRenkeiDate;
    }
}
