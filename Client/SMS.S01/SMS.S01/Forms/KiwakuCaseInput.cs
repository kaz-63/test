using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Commons;
using SystemBase.Util;

using WsConnection.WebRefS01;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 印刷C/NO入力ダイアログ
    /// </summary>
    /// <create>H.Tajimi 2015/11/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KiwakuCaseInput : SystemBase.Forms.CustomOrderForm
    {
        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 工事No.
        /// </summary>
        /// <create>T.Wakamatsu 2016/04/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNo { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// ケースNo.
        /// </summary>
        /// <create>T.Wakamatsu 2016/04/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseNo { get; set; }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷ケースNo.
        /// </summary>
        /// <create>T.Wakamatsu 2016/04/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PrintCaseNo { get; set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public KiwakuCaseInput()
            : this(null, null, null, null, null, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="ship">出荷便</param>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public KiwakuCaseInput(UserInfo userInfo, string kojiNo, string kojiName,
            string ship, string c_no, string prnC_no)
            : base(userInfo)
        {
            this.InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_K0300030;
            this.KojiNo = kojiNo;
            this.txtKojiName.Text = kojiName;
            this.txtShip.Text = ship;
            this.txtCaseNo.Text = c_no;
            this.txtPrintCaseNo.Text = prnC_no;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region イベント

        #region 選択

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCaseNo.Text))
                {
                    // C/NOを入力して下さい。
                    this.ShowMessage("A9999999017");
                    this.txtKojiName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtPrintCaseNo.Text))
                {
                    // 印刷C/NOを入力して下さい。
                    this.ShowMessage("A9999999017");
                    this.txtKojiName.Focus();
                    return;
                }
                CondS01 cond = new CondS01(this.UserInfo);
                cond.KojiNoDest = this.KojiNo;
                cond.CaseNo = this.txtCaseNo.Text;
                cond.PrintCaseNo = this.txtPrintCaseNo.Text;

                // TODO:重複チェック

                // C/NOセット
                this.CaseNo = cond.CaseNo;
                this.PrintCaseNo = cond.PrintCaseNo;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 閉じる

        /// --------------------------------------------------
        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion
    }
}
