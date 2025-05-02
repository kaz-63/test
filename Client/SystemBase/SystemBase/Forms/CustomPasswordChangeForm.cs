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
    /// ログイン画面用 パスワード変更
    /// </summary>
    /// <create>H.Tajimi 2018/10/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CustomPasswordChangeForm : SystemBase.Forms.PasswordChangeForm
    {
        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserID { get; private set; }
        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Password { get; private set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomPasswordChangeForm()
            : this(new UserInfo(), string.Empty, string.Empty)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="userID">ユーザーID</param>
        /// <param name="password">パスワード</param>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomPasswordChangeForm(UserInfo userInfo, string userID, string password)
            : base(userInfo)
        {
            InitializeComponent();
            this.UserID = userID;
            this.Password = password;
            this.txtRoleName.Visible = false;
            this.txtUserName.Visible = false;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                this.IsChangedCloseQuestion = true;

                this.txtUserID.MaxLength = this.UserInfo.SysInfo.MaxUserID;
                this.txtUserID.SelectionPos = DSWControl.DSWTextBox.DSWTextBox.SelectionType.All;
                this.txtUserID.Text = this.UserID;
                this.txtOldPassword.Text = this.Password;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Shown時の初期化
        /// </summary>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                if (string.IsNullOrEmpty(this.txtUserID.Text))
                {
                    this.txtUserID.Focus();
                }
                else if (string.IsNullOrEmpty(this.txtOldPassword.Text))
                {
                    this.txtOldPassword.Focus();
                }
                else
                {
                    this.txtNewPassword.Focus();
                }
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
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            if (string.IsNullOrEmpty(this.txtUserID.Text))
            {
                // ユーザーが入力されていません。
                this.ShowMessage("FW010010001");
                this.txtUserID.Focus();
                return false;
            }
            return base.CheckInput();
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 既存のユーザーID+パスワードでログインできるか
                var cond = new CondCommon(this.UserInfo);
                var conn = new ConnCommon();

                // ログイン情報セット
                cond.UserID = this.txtUserID.Text;
                cond.Password = this.txtOldPassword.Text;
                DataTable dt = conn.GetLoginUser(cond).Tables[Def_M_USER.Name];
                if (!UtilData.ExistsData(dt))
                {
                    // ログイン情報が正しくありません。
                    this.ShowMessage("FW010010003");
                    return false;
                }
                this.UserInfo.UserID = ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID);
                this.UserInfo.UserName = ComFunc.GetFld(dt, 0, Def_M_USER.USER_NAME);
                this.UserInfo.PasswordChangeDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_USER.PASSWORD_CHANGE_DATE);
                this.UserInfo.RoleID = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_ID);
                this.UserInfo.RoleName = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_NAME);

                if (base.RunEditUpdate())
                {
                    this.UserID = this.txtUserID.Text;
                    this.Password = this.txtNewPassword.Text;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion
    }
}
