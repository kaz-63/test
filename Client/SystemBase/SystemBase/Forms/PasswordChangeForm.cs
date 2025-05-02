using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// パスワード変更
    /// </summary>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PasswordChangeForm : SystemBase.Forms.CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private PasswordChangeForm()
            : this(new UserInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public PasswordChangeForm(UserInfo userInfo)
            : base(userInfo, ComDefine.TITLE_PASSWORDCHANGE)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化
                this.Icon = ComFunc.BitmapToIcon(ComResource.Login);
                // 入力サイズの設定
                if (this.UserInfo != null && this.UserInfo.SysInfo != null)
                {
                    this.txtOldPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                    this.txtNewPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                    this.txtConfirmPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                }

                // 編集モードの設定
                this.EditMode = EditMode.Update;

                // 修正確認メッセージ
                this.MsgUpdateConfirm = "";
                // 修正完了メッセージ
                this.MsgUpdateEnd = "";
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
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.txtOldPassword.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // クリア処理
                this.txtOldPassword.Text = string.Empty;
                this.txtNewPassword.Text = string.Empty;
                this.txtConfirmPassword.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                string errMsgID;
                string[] args = null;
                ret = ComFunc.CheckInputPassword(this.txtOldPassword.Text, this.txtNewPassword.Text, this.txtConfirmPassword.Text, this.UserInfo.SysInfo.MinPassword, this.UserInfo.SysInfo.MaxPassword, this.UserInfo.SysInfo.PasswordCheck, false, out errMsgID,out args);
                if (!ret)
                {
                    if(string.IsNullOrEmpty(errMsgID))
                    {
                        CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT);
                        return false;
                    }
                    if (args == null)
                    {
                        this.ShowMessage(errMsgID);
                    }
                    else
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    // フォーカスをセットする
                    this.ErrorSetFocus(errMsgID);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                CondUserPassword cond = new CondUserPassword(this.UserInfo);
                ConnCommon conn = new ConnCommon();

                // ログイン情報セット
                cond.UserID = this.UserInfo.UserID;
                cond.Password = this.txtOldPassword.Text;
                cond.NewPassword = this.txtNewPassword.Text;
                cond.ConfirmPassword = this.txtConfirmPassword.Text;

                string errMsgID;
                string[] args = null;
                DataSet ds = new DataSet();
                bool ret = conn.UpdUserPassword(cond, out ds, out errMsgID, out args);
                if (!ret)
                {
                    if (string.IsNullOrEmpty(errMsgID))
                    {
                        CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT);
                        return false;
                    }
                    if (args == null)
                    {
                        this.ShowMessage(errMsgID);
                    }
                    else
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    // フォーカスをセットする
                    this.ErrorSetFocus(errMsgID);
                }
                this.UserInfo.PasswordChangeDate = ComFunc.GetFldToDateTime(ds,Def_M_USER.Name,0,Def_M_USER.PASSWORD_CHANGE_DATE);

                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region イベント

        #region 変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                // 編集モードは変更固定
                this.EditMode = EditMode.Update;
                if (this.RunEdit())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 閉じるボタン

        /// --------------------------------------------------
        /// <summary>
        /// 閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/02</create>
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

        #region エラー時のセットフォーカス

        /// --------------------------------------------------
        /// <summary>
        /// エラー時のセットフォーカス
        /// </summary>
        /// <param name="msgID">メッセージID</param>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ErrorSetFocus(string msgID)
        {
            switch (ComFunc.GetPasswordErrorType(msgID))
            {
                case PasswordErrorType.Old:
                    this.txtOldPassword.Focus();
                    break;
                case PasswordErrorType.New:
                    this.txtNewPassword.Focus();
                    break;
                case PasswordErrorType.Confirm:
                    this.txtConfirmPassword.Focus();
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
