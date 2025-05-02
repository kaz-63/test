using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// ログインユーザー情報管理クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class UserInfo
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userID = null;

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userName = null;

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更日
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime _passwrodChangeDate = new DateTime();

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _pcName = null;

        /// --------------------------------------------------
        /// <summary>
        /// IPアドレス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _ipAddress = new string[] { "" };

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleID = null;

        /// --------------------------------------------------
        /// <summary>
        /// 権限名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleName = null;

        /// --------------------------------------------------
        /// <summary>
        /// システム情報管理クラス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private SystemInfo _sysInfo = new SystemInfo();
        /// -------------------------------------------------- 
        /// <summary>
        /// 言語設定
        /// </summary>
        /// <create>S.Furugo 2018/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _language = null;
        /// -------------------------------------------------- 
        /// <summary>
        /// カルチャ情報
        /// </summary>
        /// <create>D.Okumura 2018/09/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private CultureInfo _cultureInfo = null;
        #endregion

        #region ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        #endregion

        #region ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        #endregion

        #region パスワード変更日

        /// --------------------------------------------------
        /// <summary>
        /// パスワード変更日
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime PasswordChangeDate
        {
            get { return this._passwrodChangeDate; }
            set { this._passwrodChangeDate = value; }
        }

        #endregion

        #region PC名

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PcName
        {
            get { return this._pcName; }
            set { this._pcName = value; }
        }

        #endregion

        #region IPアドレス

        /// --------------------------------------------------
        /// <summary>
        /// IPアドレス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string[] IPAddress
        {
            get { return this._ipAddress; }
            set { this._ipAddress = value; }
        }

        #endregion

        #region 権限ID

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleID
        {
            get { return this._roleID; }
            set { this._roleID = value; }
        }

        #endregion

        #region 権限名

        /// --------------------------------------------------
        /// <summary>
        /// 権限名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleName
        {
            get { return this._roleName; }
            set { this._roleName = value; }
        }

        #endregion

        #region システム情報管理クラス

        /// --------------------------------------------------
        /// <summary>
        /// システム情報管理クラス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public SystemInfo SysInfo
        {
            get { return this._sysInfo; }
            set { this._sysInfo = value; }
        }

        #endregion

        #region 言語設定

        /// --------------------------------------------------
        /// <summary>
        /// 言語設定
        /// </summary>
        /// <create>S.Furugo 2018/08/23</create>
        /// <update></update>
        /// <remarks>
        /// DBの言語切り替えのための情報。
        /// JP/USなどが設定され、DBの問合せで使用される
        /// </remarks>
        /// --------------------------------------------------
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        #endregion

        #region カルチャ情報
        /// --------------------------------------------------
        /// <summary>
        /// カルチャ情報
        /// </summary>
        /// <create>D.Okumura 2018/09/26</create>
        /// <update></update>
        /// <remarks>
        /// UIのカルチャ情報。
        /// BegineInvoke等でカルチャ情報が引き継がれないときに
        /// この情報を用いて対象のスレッドにカルチャ情報を設定する。
        /// UIのために使用されるため、LoginInfoへの引継ぎは行われない。
        /// </remarks>
        /// --------------------------------------------------
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { _cultureInfo = value; }
        }
        #endregion

        #region 製番追加分
        #endregion

    }
}
