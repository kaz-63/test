namespace SystemBase.Controls
{
    partial class MessageLabel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageLabel));
            this.picImage = new System.Windows.Forms.PictureBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.imlMessage = new System.Windows.Forms.ImageList(this.components);
            this.tmrBlink = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.picImage.Location = new System.Drawing.Point(0, 0);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(16, 16);
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(16, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(134, 16);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imlMessage
            // 
            this.imlMessage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlMessage.ImageStream")));
            this.imlMessage.TransparentColor = System.Drawing.Color.Transparent;
            this.imlMessage.Images.SetKeyName(0, "Error");
            this.imlMessage.Images.SetKeyName(1, "Information");
            this.imlMessage.Images.SetKeyName(2, "Question");
            this.imlMessage.Images.SetKeyName(3, "Warning");
            // 
            // tmrBlink
            // 
            this.tmrBlink.Tick += new System.EventHandler(this.tmrBlink_Tick);
            // 
            // MessageLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.picImage);
            this.Name = "MessageLabel";
            this.Size = new System.Drawing.Size(150, 16);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ImageList imlMessage;
        private System.Windows.Forms.Timer tmrBlink;
    }
}
