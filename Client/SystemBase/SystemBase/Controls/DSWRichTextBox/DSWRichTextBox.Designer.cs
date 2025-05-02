namespace DSWControl.DSWRichTextBox
{
    partial class DSWRichTextBox
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
            this.richTextBoxPasteArea = new DSWControl.DSWRichTextBox.RichTextBox5();
            this.SuspendLayout();
            // 
            // richTextBoxPasteArea
            // 
            this.richTextBoxPasteArea.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxPasteArea.MaxLength = 0;
            this.richTextBoxPasteArea.Name = "richTextBoxPasteArea";
            this.richTextBoxPasteArea.Size = new System.Drawing.Size(100, 96);
            this.richTextBoxPasteArea.TabIndex = 0;
            this.richTextBoxPasteArea.TabStop = false;
            this.richTextBoxPasteArea.Text = "";
            this.richTextBoxPasteArea.Visible = false;
            this.richTextBoxPasteArea.WordWrap = false;
            this.ResumeLayout(false);

        }

        #endregion

        private DSWControl.DSWRichTextBox.RichTextBox5 richTextBoxPasteArea;
    }
}
