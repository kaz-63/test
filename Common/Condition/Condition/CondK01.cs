using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 集荷開始用コンディション
    /// </summary>
    /// <create>H.Tsunamura 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondK01 : CondBase
    {
        #region Fields
        // 出荷区分
        private string _shukkaFlg = null;
        // 納入先名称
        private string _nonyusakiName = null;
        // 便
        private string _ship = null;
        // 製番
        private string _seiban = null;
        // コード
        private string _code = null;
        // 管理番号
        private string _nounyusakiCD = null;
        // 表示選択
        private string _displaySelect = null;
        // ARNo
        private string _arNo = null;
        // 操作区分
        private string _operationFlag = null;
        // 更新PC名
        private string _updatePCName = null;
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK01()
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
        public CondK01(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlg; }
            set { this._shukkaFlg = value; }
        }

        #endregion

        #region 納入先名称

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名称
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiName
        {
            get { return this._nonyusakiName; }
            set { this._nonyusakiName = value; }
        }

        #endregion

        #region 便

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get { return this._ship; }
            set { this._ship = value; }
        }

        #endregion

        #region 製番

        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Seiban
        {
            get { return this._seiban; }
            set { this._seiban = value; }
        }

        #endregion

        #region コード

        /// --------------------------------------------------
        /// <summary>
        /// コード
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
        }

        #endregion

        #region 管理番号

        /// --------------------------------------------------
        /// <summary>
        /// 管理番号
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NounyusakiCD
        {
            get { return this._nounyusakiCD; }
            set { this._nounyusakiCD = value; }
        }

        #endregion

        #region 表示選択

        /// --------------------------------------------------
        /// <summary>
        /// 表示選択
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DisplaySelect
        {
            get { return this._displaySelect; }
            set { this._displaySelect = value; }
        }

        #endregion

        #region ARNo

        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 操作区分
        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>J.Chen 2024/10/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string OperationFlag
        {
            get { return this._operationFlag; }
            set { this._operationFlag = value; }
        }
        #endregion

        #region 更新PC名
        /// --------------------------------------------------
        /// <summary>
        /// 更新PC名
        /// </summary>
        /// <create>J.Chen 2024/10/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UpdatePCName
        {
            get { return this._updatePCName; }
            set { this._updatePCName = value; }
        }
        #endregion
    }
}
