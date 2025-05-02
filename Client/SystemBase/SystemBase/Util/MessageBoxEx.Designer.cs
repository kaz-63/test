namespace SystemBase.Util
{

    partial class MessageBoxEx
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxEx));
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lblMainMessage = new System.Windows.Forms.Label();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.cmsDetail = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDetail = new System.Windows.Forms.Button();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMessage = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnOne = new System.Windows.Forms.Button();
            this.btnTwo = new System.Windows.Forms.Button();
            this.btnThree = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tmrClose = new System.Windows.Forms.Timer(this.components);
            this.sfdErrorFile = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.cmsDetail.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tlpMessage.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            resources.ApplyResources(this.picIcon, "picIcon");
            this.picIcon.Name = "picIcon";
            this.picIcon.TabStop = false;
            // 
            // lblMainMessage
            // 
            resources.ApplyResources(this.lblMainMessage, "lblMainMessage");
            this.lblMainMessage.MaximumSize = new System.Drawing.Size(400, 0);
            this.lblMainMessage.Name = "lblMainMessage";
            // 
            // txtDetail
            // 
            this.txtDetail.ContextMenuStrip = this.cmsDetail;
            resources.ApplyResources(this.txtDetail, "txtDetail");
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            // 
            // cmsDetail
            // 
            this.cmsDetail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClipboardToolStripMenuItem,
            this.SaveFileToolStripMenuItem});
            this.cmsDetail.Name = "cmsDetail";
            resources.ApplyResources(this.cmsDetail, "cmsDetail");
            // 
            // ClipboardToolStripMenuItem
            // 
            this.ClipboardToolStripMenuItem.Name = "ClipboardToolStripMenuItem";
            resources.ApplyResources(this.ClipboardToolStripMenuItem, "ClipboardToolStripMenuItem");
            this.ClipboardToolStripMenuItem.Click += new System.EventHandler(this.ClipboardToolStripMenuItem_Click);
            // 
            // SaveFileToolStripMenuItem
            // 
            this.SaveFileToolStripMenuItem.Name = "SaveFileToolStripMenuItem";
            resources.ApplyResources(this.SaveFileToolStripMenuItem, "SaveFileToolStripMenuItem");
            this.SaveFileToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // btnDetail
            // 
            this.btnDetail.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            resources.ApplyResources(this.btnDetail, "btnDetail");
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.UseVisualStyleBackColor = true;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // tlpMain
            // 
            resources.ApplyResources(this.tlpMain, "tlpMain");
            this.tlpMain.Controls.Add(this.txtDetail, 0, 2);
            this.tlpMain.Controls.Add(this.tlpMessage, 0, 0);
            this.tlpMain.Controls.Add(this.tlpButton, 0, 3);
            this.tlpMain.Controls.Add(this.btnDetail, 0, 1);
            this.tlpMain.Name = "tlpMain";
            // 
            // tlpMessage
            // 
            resources.ApplyResources(this.tlpMessage, "tlpMessage");
            this.tlpMessage.Controls.Add(this.picIcon, 0, 0);
            this.tlpMessage.Controls.Add(this.lblMainMessage, 1, 0);
            this.tlpMessage.Name = "tlpMessage";
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.btnOne, 0, 0);
            this.tlpButton.Controls.Add(this.btnTwo, 1, 0);
            this.tlpButton.Controls.Add(this.btnThree, 2, 0);
            this.tlpButton.Controls.Add(this.btnHelp, 3, 0);
            this.tlpButton.Name = "tlpButton";
            // 
            // btnOne
            // 
            resources.ApplyResources(this.btnOne, "btnOne");
            this.btnOne.Name = "btnOne";
            this.btnOne.UseVisualStyleBackColor = true;
            this.btnOne.Click += new System.EventHandler(this.btnClickClose);
            // 
            // btnTwo
            // 
            resources.ApplyResources(this.btnTwo, "btnTwo");
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.UseVisualStyleBackColor = true;
            this.btnTwo.Click += new System.EventHandler(this.btnClickClose);
            // 
            // btnThree
            // 
            resources.ApplyResources(this.btnThree, "btnThree");
            this.btnThree.Name = "btnThree";
            this.btnThree.UseVisualStyleBackColor = true;
            this.btnThree.Click += new System.EventHandler(this.btnClickClose);
            // 
            // btnHelp
            // 
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // tmrClose
            // 
            this.tmrClose.Tick += new System.EventHandler(this.tmrClose_Tick);
            // 
            // sfdErrorFile
            // 
            this.sfdErrorFile.DefaultExt = "txt";
            resources.ApplyResources(this.sfdErrorFile, "sfdErrorFile");
            // 
            // MessageBoxEx
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxEx";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.cmsDetail.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpMessage.ResumeLayout(false);
            this.tlpMessage.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button btnOne;
        private System.Windows.Forms.Button btnTwo;
        private System.Windows.Forms.Button btnThree;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lblMainMessage;
        private System.Windows.Forms.TextBox txtDetail;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpMessage;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Timer tmrClose;
        private System.Windows.Forms.ContextMenuStrip cmsDetail;
        private System.Windows.Forms.ToolStripMenuItem ClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdErrorFile;
    }
}