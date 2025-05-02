using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DSWControl.DSWRichTextBox
{
    internal partial class InputRegulationSetting : Form
    {
        #region プロパティ
        /// --------------------------------------------------
        /// <summary>
        /// 入力規制する種別
        /// </summary>
        /// <create>T.Sakiori 2008/04/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public String InputRegulation
        {
            get { return txtType.Text; }
            set { txtType.Text = value; }
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 対象を使用可能にするのか使用不可にするのか
        /// </summary>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public bool IsInputRegulation
        {
            get { return chkIsUse.Checked; }
            set { chkIsUse.Checked = value; }
        }
        #endregion

        #region コンストラクタ
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        public InputRegulationSetting()
            : this(true)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="enableIsInputRegulation">対象を使用可能にするかどうか</param>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public InputRegulationSetting(bool enableIsInputRegulation)
        {
            InitializeComponent();
            chkIsUse.Enabled = enableIsInputRegulation;
        }
        #endregion

        #region イベント
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// OKボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：大文字アルファベット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideUpAlpha_Click(object sender, EventArgs e)
        {
            AddTypeText("A");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：大文字アルファベット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowUpAlpha_Click(object sender, EventArgs e)
        {
            AddTypeText("a");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：小文字アルファベット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideLowAlpha_Click(object sender, EventArgs e)
        {
            AddTypeText("B");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：小文字アルファベット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowLowAlpha_Click(object sender, EventArgs e)
        {
            AddTypeText("b");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideNumeric_Click(object sender, EventArgs e)
        {
            AddTypeText("N");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowNumeric_Click(object sender, EventArgs e)
        {
            AddTypeText("n");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：数字および数字関連記号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideNumericSign_Click(object sender, EventArgs e)
        {
            AddTypeText("R");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：数字および数字関連記号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowNumericSign_Click(object sender, EventArgs e)
        {
            AddTypeText("r");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：記号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideSign_Click(object sender, EventArgs e)
        {
            AddTypeText("L");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：記号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowSign_Click(object sender, EventArgs e)
        {
            AddTypeText("l");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：カタカナ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideKatakana_Click(object sender, EventArgs e)
        {
            AddTypeText("K");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：カタカナ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowKatakana_Click(object sender, EventArgs e)
        {
            AddTypeText("k");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 全角：スペース
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnWideSpace_Click(object sender, EventArgs e)
        {
            AddTypeText("S");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角：スペース
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowSpace_Click(object sender, EventArgs e)
        {
            AddTypeText("s");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 半角文字(記号無し)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnNarrowChar_Click(object sender, EventArgs e)
        {
            AddTypeText("z");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// ひらがな
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnHiragana_Click(object sender, EventArgs e)
        {
            AddTypeText("H");
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// サロゲート文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void btnSurrogate_Click(object sender, EventArgs e)
        {
            AddTypeText("F");
        }
        #endregion

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// 文字の追加
        /// </summary>
        /// <param name="str"></param>
        /// <create>[2008/12/16] T.Sakiori</create>
        /// <update></update>
        /// ---------------------------------------------------------------------------
        private void AddTypeText(String str)
        {
            if (txtType.Text.IndexOf(str) == -1)
            {
                txtType.Text += str;
            }
        }
    }
}