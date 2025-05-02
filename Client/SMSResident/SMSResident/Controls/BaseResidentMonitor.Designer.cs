namespace SMSResident.Controls
{
    partial class BaseResidentMonitor
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
            this.lblResidentStatus = new DSWControl.DSWLabel.DSWLabel();
            this.rdoStop = new System.Windows.Forms.RadioButton();
            this.rdoStart = new System.Windows.Forms.RadioButton();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.btnSetting = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.btnClear = new DSWControl.DSWFunctionButton.DSWFunctionButton();
            this.lblResidentStatus.ChildPanel.SuspendLayout();
            this.lblResidentStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResidentStatus
            // 
            this.lblResidentStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // lblResidentStatus.ChildPanel
            // 
            this.lblResidentStatus.ChildPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblResidentStatus.ChildPanel.Controls.Add(this.rdoStop);
            this.lblResidentStatus.ChildPanel.Controls.Add(this.rdoStart);
            this.lblResidentStatus.IsFocusChangeColor = false;
            this.lblResidentStatus.LabelWidth = 70;
            this.lblResidentStatus.Location = new System.Drawing.Point(0, 0);
            this.lblResidentStatus.Name = "lblResidentStatus";
            this.lblResidentStatus.Size = new System.Drawing.Size(215, 22);
            this.lblResidentStatus.SplitterWidth = 0;
            this.lblResidentStatus.TabIndex = 1;
            this.lblResidentStatus.Text = "実行状態";
            // 
            // rdoStop
            // 
            this.rdoStop.AutoSize = true;
            this.rdoStop.Checked = true;
            this.rdoStop.Location = new System.Drawing.Point(78, 1);
            this.rdoStop.Name = "rdoStop";
            this.rdoStop.Size = new System.Drawing.Size(47, 16);
            this.rdoStop.TabIndex = 1;
            this.rdoStop.TabStop = true;
            this.rdoStop.Text = "停止";
            this.rdoStop.UseVisualStyleBackColor = true;
            // 
            // rdoStart
            // 
            this.rdoStart.AutoSize = true;
            this.rdoStart.Location = new System.Drawing.Point(12, 1);
            this.rdoStart.Name = "rdoStart";
            this.rdoStart.Size = new System.Drawing.Size(47, 16);
            this.rdoStart.TabIndex = 0;
            this.rdoStart.Text = "実行";
            this.rdoStart.UseVisualStyleBackColor = true;
            // 
            // lstMessage
            // 
            this.lstMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.ItemHeight = 12;
            this.lstMessage.Location = new System.Drawing.Point(0, 28);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.ScrollAlwaysVisible = true;
            this.lstMessage.Size = new System.Drawing.Size(486, 172);
            this.lstMessage.TabIndex = 2;
            this.lstMessage.TabStop = false;
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSetting.Location = new System.Drawing.Point(0, 208);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "設定";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClear.Location = new System.Drawing.Point(103, 208);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // BaseResidentMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.lstMessage);
            this.Controls.Add(this.lblResidentStatus);
            this.Name = "BaseResidentMonitor";
            this.Size = new System.Drawing.Size(486, 231);
            this.lblResidentStatus.ChildPanel.ResumeLayout(false);
            this.lblResidentStatus.ChildPanel.PerformLayout();
            this.lblResidentStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DSWControl.DSWLabel.DSWLabel lblResidentStatus;
        protected System.Windows.Forms.RadioButton rdoStop;
        protected System.Windows.Forms.RadioButton rdoStart;
        protected System.Windows.Forms.ListBox lstMessage;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnSetting;
        protected DSWControl.DSWFunctionButton.DSWFunctionButton btnClear;
    }
}
