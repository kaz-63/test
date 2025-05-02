using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// ログイン情報
    /// (Commons.UserInfoを使用するとクライアント側で名前空間をフルで定義しないと
    /// クラスが判別できなくなるので必要な情報だけのクラスを定義)
    /// </summary>
    /// <create>Y.Higuchi 2010/06/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public class LoginInfo : ICloneable
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userID = null;

        /// --------------------------------------------------
        /// <summary>
        /// ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _userName = null;

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _pcName = null;

        /// --------------------------------------------------
        /// <summary>
        /// IPアドレス
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _ipAddress = new string[] { "" };

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleID = null;

        /// --------------------------------------------------
        /// <summary>
        /// 権限名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleName = null;

        /// -------------------------------------------------- 
        /// <summary>
        /// 言語設定
        /// </summary>
        /// <create>S.Furugo 2018/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _language = null;

        #endregion

        #region ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
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
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        #endregion

        #region PC名

        /// --------------------------------------------------
        /// <summary>
        /// PC名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/29</create>
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
        /// <create>Y.Higuchi 2010/06/29</create>
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
        /// <create>Y.Higuchi 2010/06/29</create>
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
        /// <create>Y.Higuchi 2010/06/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleName
        {
            get { return this._roleName; }
            set { this._roleName = value; }
        }

        #endregion

        #region 言語設定

        /// --------------------------------------------------
        /// <summary>
        /// 言語設定
        /// </summary>
        /// <create>S.Furugo 2018/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        #endregion

        #region Clone

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public LoginInfo Clone()
        {
            return (LoginInfo)this.MemberwiseClone();
        }

        #region ICloneable メンバ

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #endregion
    }
}
