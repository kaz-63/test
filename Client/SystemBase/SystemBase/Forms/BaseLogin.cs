using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Util;
using System.IO;
using SystemBase.Properties;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ログインベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BaseLogin : CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private BaseLogin()
            : this(new UserInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseLogin(UserInfo userInfo)
            // 2011/02/21 K.Tsutsumi Change カタカナ禁止
            //: this(userInfo, "ログイン")
            : this(userInfo, Resources.BaseLogin_Login)
            // ↑
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="Title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseLogin(UserInfo userInfo, string Title)
            : base(userInfo, Title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// ロード時の初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            try
            {
                base.InitializeLoadControl();

                // 入力サイズの設定
                if (this.UserInfo != null && this.UserInfo.SysInfo != null)
                {
                    this.txtUserID.MaxLength = this.UserInfo.SysInfo.MaxUserID;
                    this.txtPassword.MaxLength = this.UserInfo.SysInfo.MaxPassword;
                }

                // 技連ファイルの全削除
                if (Directory.Exists(ComDefine.DOWNLOAD_DIR))
                {
                    Directory.Delete(ComDefine.DOWNLOAD_DIR, true);
                }

                this.Icon = ComFunc.BitmapToIcon(ComResource.Login);
            }
            catch { }

        }

        /// --------------------------------------------------
        /// <summary>
        /// Shown時の初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            this.txtUserID.Focus();
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ユーザーID
                this.txtUserID.Text = string.Empty;
                // パスワード
                this.txtPassword.Text = string.Empty;
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
        /// <create>Y.Higuchi 2010/04/23</create>
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

            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                // パスワードが入力されていません。
                this.ShowMessage("FW010010002");
                this.txtPassword.Focus();
                return false;
            }
            return true;
        }

        #endregion

        #region ユーザー情報取得

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        /// <param name="password">パスワード</param>
        /// <param name="errorMsgID">エラーメッセージID</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// --------------------------------------------------
        protected virtual bool ExecuteLogin(string userID, string password, ref string errorMsgID)
        {
            try
            {
                // ユーザー情報取得
                ConnCommon conn = new ConnCommon();
                CondCommon cond = new CondCommon(this.UserInfo);
                cond.UserID = userID;
                cond.Password = password;
                DataTable dt = conn.GetLoginUser(cond).Tables[Def_M_USER.Name];

                // ユーザー情報設定
                if (0 < dt.Rows.Count)
                {
                    this.UserInfo.UserID = ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID);
                    this.UserInfo.UserName = ComFunc.GetFld(dt, 0, Def_M_USER.USER_NAME);
                    this.UserInfo.PasswordChangeDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_USER.PASSWORD_CHANGE_DATE);
                    this.UserInfo.RoleID = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_ID);
                    this.UserInfo.RoleName = ComFunc.GetFld(dt, 0, Def_M_ROLE.ROLE_NAME);

                    return BaseFunc.CheckUserPassExpiration(this.UserInfo, ref errorMsgID);
                }
                else
                {
                    // ログイン情報が正しくありません。
                    errorMsgID = "FW010010003";
                    return false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region メニュー画面を取得

        /// --------------------------------------------------
        /// <summary>
        /// メニュー画面を取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual BaseForm GetMenuForm()
        {
            return new BaseSDIMenu(this.UserInfo);
        }

        #endregion

        #region イベント

        #region ログインボタン

        /// --------------------------------------------------
        /// <summary>
        /// ログインボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckInput()) return;
                string errorMsgID = string.Empty;
                if (this.ExecuteLogin(this.txtUserID.Text, this.txtPassword.Text, ref errorMsgID))
                {
                    DialogResult result = DialogResult.Cancel;
                    try
                    {
                        this.Hide();
                        using (BaseForm frm = this.GetMenuForm())
                        {
                            result = frm.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                    }
                    finally
                    {
                        if (result == DialogResult.OK )
                        {
                            UserInfo userInfo = this.UserInfo;
                            BaseFunc.GetSystemInitializeData(ref userInfo);
                            this.SetUserInfo(userInfo);
                            this.DisplayClear();
                            this.Show();
                            this.txtUserID.Focus();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    // ログイン情報が正しくありません。
                    if (!string.IsNullOrEmpty(errorMsgID))
                    {
                        this.ShowMessage(errorMsgID);
                    }
                    this.txtUserID.Focus();
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
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region パスワード変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnPasswordChange_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new CustomPasswordChangeForm(this.UserInfo, this.txtUserID.Text, this.txtPassword.Text))
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    this.txtUserID.Text = frm.UserID;
                    this.txtPassword.Text = frm.Password;
                    this.btnLogin.Focus();
                }
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
