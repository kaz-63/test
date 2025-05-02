using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 受入用コンディション
    /// </summary>
    /// <create>H.Tsunamura 2010/08/06</create>
    /// <update>T.Wakamatsu 2013/09/05 不要なProperty削除</update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondU01 : CondBase
    {
        #region Fields
        // 受入No
        private string _ukeireNo = null;
        // 受入日
        private string _ukeireDate = null;
        // 状態区分
        private string _jyotaiFlag = null;
        // 納入先コード
        private string _nonyusakiCD = null;
        // 出荷フラグ
        private string _shukkaFlag = null;

        /// --------------------------------------------------
        /// <summary>
        /// 文字列：梱包済み
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textKonpozumi = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：出荷
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textShukka = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：受入
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textUkeire = null;
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:詳細
        /// </summary>
        /// <create>S.Furugo 2018/10/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _textShosai = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondU01()
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
        public CondU01(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 受入No

        /// --------------------------------------------------
        /// <summary>
        /// 受入No
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UkeireNo
        {
            get { return this._ukeireNo; }
            set { this._ukeireNo = value; }
        }

        #endregion

        #region 受入日

        /// --------------------------------------------------
        /// <summary>
        /// 受入日
        /// </summary>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>T.Wakamatsu 2013/09/05 objectからstringへ変更</update>
        /// --------------------------------------------------
        public string UkeireDate
        {
            get { return this._ukeireDate; }
            set { this._ukeireDate = value; }
        }

        #endregion

        #region 状態区分
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>H.Tsunamura 2010/08/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JyotaiFlag
        {
            get { return this._jyotaiFlag; }
            set { this._jyotaiFlag = value; }
        }

        #endregion

        #region 納入先コード
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>H.Tsunamura 2010/08/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion

        #region 出荷フラグ
        /// --------------------------------------------------
        /// <summary>
        /// ShukkaFlag
        /// </summary>
        /// <create>H.Tsunamura 2010/08/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaFlag
        {
            get { return this._shukkaFlag; }
            set { this._shukkaFlag = value; }
        }

        #endregion

        #region 文字列：梱包済み
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：梱包済み
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextKonpozumi
        {
            get { return this._textKonpozumi; }
            set { this._textKonpozumi = value; }
        }
        #endregion

        #region 文字列：出荷
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：出荷
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextShukka
        {
            get { return this._textShukka; }
            set { this._textShukka = value; }
        }
        #endregion

        #region 文字列：受入
        /// --------------------------------------------------
        /// <summary>
        /// 文字列：受入
        /// </summary>
        /// <create>D.Okumura 2018/10/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextUkeire
        {
            get { return this._textUkeire; }
            set { this._textUkeire = value; }
        }
        #endregion

        #region 文字列:詳細
        /// --------------------------------------------------
        /// <summary>
        /// 文字列:詳細
        /// </summary>
        /// <create>S.Fuugo 2018/10/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TextShosai
        {
            get { return this._textShosai; }
            set { this._textShosai = value; }
        }

        #endregion


    }
}
