using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタ用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondRole : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 権限種別
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 権限名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleName = null;
        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _roleID = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondRole()
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
        public CondRole(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 権限ID

        /// --------------------------------------------------
        /// <summary>
        /// 権限ID
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleName
        {
            get { return this._roleName; }
            set { this._roleName = value; }
        }

        #endregion

        #region 権限種別

        /// --------------------------------------------------
        /// <summary>
        /// 権限種別
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string RoleFlag
        {
            get { return this._roleFlag; }
            set { this._roleFlag = value; }
        }

        #endregion

    }
}
