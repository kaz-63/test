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
        #region �v���p�e�B
        /// --------------------------------------------------
        /// <summary>
        /// ���͋K��������
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
        /// �Ώۂ��g�p�\�ɂ���̂��g�p�s�ɂ���̂�
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

        #region �R���X�g���N�^
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// �R���X�g���N�^
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
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="enableIsInputRegulation">�Ώۂ��g�p�\�ɂ��邩�ǂ���</param>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public InputRegulationSetting(bool enableIsInputRegulation)
        {
            InitializeComponent();
            chkIsUse.Enabled = enableIsInputRegulation;
        }
        #endregion

        #region �C�x���g
        /// ---------------------------------------------------------------------------
        /// <summary>
        /// OK�{�^���N���b�N
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
        /// �L�����Z���{�^���N���b�N
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
        /// �S�p�F�啶���A���t�@�x�b�g
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
        /// ���p�F�啶���A���t�@�x�b�g
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
        /// �S�p�F�������A���t�@�x�b�g
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
        /// ���p�F�������A���t�@�x�b�g
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
        /// �S�p�F����
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
        /// ���p�F����
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
        /// �S�p�F��������ѐ����֘A�L��
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
        /// ���p�F��������ѐ����֘A�L��
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
        /// �S�p�F�L��
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
        /// ���p�F�L��
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
        /// �S�p�F�J�^�J�i
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
        /// ���p�F�J�^�J�i
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
        /// �S�p�F�X�y�[�X
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
        /// ���p�F�X�y�[�X
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
        /// ���p����(�L������)
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
        /// �Ђ炪��
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
        /// �T���Q�[�g����
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
        /// �����̒ǉ�
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