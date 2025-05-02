using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// パスワード変更コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/07/02</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondUserPassword : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userID;
        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _password;
        /// --------------------------------------------------
        /// <summary>
        /// 新しいパスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _newPassword;
        /// --------------------------------------------------
        /// <summary>
        /// パスワード確認入力
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _confirmPassword;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondUserPassword()
            : this(new LoginInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">ログインユーザー情報</param>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public CondUserPassword(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        #endregion

        #region パスワード

        /// --------------------------------------------------
        /// <summary>
        /// パスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        #endregion

        #region 新しいパスワード

        /// --------------------------------------------------
        /// <summary>
        /// 新しいパスワード
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NewPassword
        {
            get { return this._newPassword; }
            set { this._newPassword = value; }
        }

        #endregion

        #region パスワード確認入力

        /// --------------------------------------------------
        /// <summary>
        /// パスワード確認入力
        /// </summary>
        /// <create>Y.Higuchi 2010/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ConfirmPassword
        {
            get { return this._confirmPassword; }
            set { this._confirmPassword = value; }
        }

        #endregion
    }
}
