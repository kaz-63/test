using System;
using System.Collections.Generic;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 製番共通処理用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondSms : CondBase
    {
        #region Fields

        // 採番フラグ
        private string _saibanFlag = null;
        // ARUS
        private string _arus = null;
        // リスト区分
        private string _listFlag = null;
        // 出荷区分
        private string _shukkaFlag = null;
        // 納入先コード
        private string _NonyusakiCD = null;
        // 納入先名
        private string _nonyusakiName = null;
        // 出荷便
        private string _ship = null;
        // AR No.
        private string _arNo = null;
        // 状況フラグ
        private string _jyokyoFlag = null;
        // 更新フラグ
        private bool _updateFlag = true;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondSms()
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
        public CondSms(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 採番フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 採番フラグ(採番用)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SaibanFlag
        {
            get { return _saibanFlag; }
            set { _saibanFlag = value; }
        }

        #endregion

        #region ARUS

        /// --------------------------------------------------
        /// <summary>
        /// ARUS(採番用)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARUS
        {
            get { return _arus; }
            set { _arus = value; }
        }

        #endregion

        #region リスト区分

        /// --------------------------------------------------
        /// <summary>
        /// リスト区分(採番用/AR情報取得)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ListFlag
        {
            get { return _listFlag; }
            set { _listFlag = value; }
        }

        #endregion
        
        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分(納入先、便、AR No.チェック)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlag; }
            set { this._shukkaFlag = value; }
        }

        #endregion

        #region 納入先コード

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード(AR情報取得)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._NonyusakiCD; }
            set { this._NonyusakiCD = value; }
        }

        #endregion

        #region 納入先名

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名(納入先、便、AR No.チェック)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get { return this._nonyusakiName; }
            set { this._nonyusakiName = value; }
        }

        #endregion
        
        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便(納入先、便、AR No.チェック)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.(納入先、便、AR No.チェック/AR情報取得)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 状況フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 状況フラグ(AR情報取得)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyokyoFlag
        {
            get { return this._jyokyoFlag; }
            set { this._jyokyoFlag = value; }
        }

        #endregion

        #region 更新フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 更新フラグ（採番時更新不要の場合使用する）
        /// </summary>
        /// <create>J.Chen 2024/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool UpdateFlag
        {
            get { return this._updateFlag; }
            set { this._updateFlag = value; }
        }

        #endregion
    }
}
