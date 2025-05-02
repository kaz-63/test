using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Commons;
using SystemBase.Util;

namespace SMS.K03.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 印刷C/NO入力ダイアログ
    /// </summary>
    /// <create>H.Tajimi 2015/11/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PrintCNoInput : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        private string _ship = string.Empty;

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public string Ship 
        {
            get { return this._ship; }
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public PrintCNoInput()
            : this(null, null)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>H.Tajimi 2015/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public PrintCNoInput(UserInfo userInfo)
            : this(userInfo, null)
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
        public PrintCNoInput(UserInfo userInfo, string ship)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_K0300030;
            this._ship = ship;
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
                this.txtShip.Text = this._ship;
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
                this.txtShip.Focus();
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
                if (string.IsNullOrEmpty(this.txtShip.Text))
                {
                    // 便を入力して下さい。
                    this.ShowMessage("A9999999017");
                    this.txtShip.Focus();
                    return;
                }
                this._ship = this.txtShip.Text;
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
