using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// K03用コンディション
    /// </summary>
    /// <create>M.Tsutsumi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondK03 : CondBase
    {
        #region Fields

        // 工事識別管理NO
        private string _kojiNo = null;

        // 工事識別名称
        private string _kojiName = null;

        // 出荷便
        private string _ship = null;

        // 内部管理用キー
        private string _caseId = null;

        // 登録区分
        private string _torokuFlag = null;

        // 出荷区分
        private string _shukkaFlag = null;

        // 物件管理No
        private string _bukkenNo = null;

        // 木枠登録種別
        private string _kiwakuInsertType = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK03()
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
        public CondK03(LoginInfo target)
            : base(target)
        {
        }
        
        #endregion

        #region 工事識別管理NO

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNo
        {
            get { return this._kojiNo; }
            set { this._kojiNo = value; }
        }

        #endregion

        #region 工事識別名称

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別名称
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiName
        {
            get { return this._kojiName; }
            set { this._kojiName = value; }
        }

        #endregion

        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region 内部管理用キー

        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キー
        /// </summary>
        /// <create>M.Tsutsumi 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseId
        {
            get { return this._caseId; }
            set { this._caseId = value; }
        }
        
        #endregion

        #region 登録区分

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>M.Tsutsumi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string torokuFlag
        {
            get { return this._torokuFlag; }
            set { this._torokuFlag = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>H.Tajimi 2018/12/25</create>
        /// <update>K.Tsutsumi 2019/01/13 スタックオーバーのエラー対応</update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlag; }
            set { this._shukkaFlag = value; }
        }

        #endregion

        #region 物件管理No

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>H.Tajimi 2018/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo
        {
            get { return this._bukkenNo; }
            set { this._bukkenNo = value; }
        }

        #endregion

        #region 木枠登録種別

        /// --------------------------------------------------
        /// <summary>
        /// 木枠登録種別
        /// </summary>
        /// <create>H.Tajimi 2019/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuInsertType
        {
            get { return this._kiwakuInsertType; }
            set { this._kiwakuInsertType = value; }
        }

        #endregion
    }
}
