using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// I01用コンディション
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Xml.Serialization.SoapType()]
    [System.Serializable()]
    public class CondI01 : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _location = null;
        /// --------------------------------------------------
        /// <summary>
        /// 在庫No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _zaikoNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 在庫日付
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string _stockDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string _sagyoFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmeiJp = null;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _hinmei = null;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _zumenKeishiki = null;
        /// --------------------------------------------------
        /// <summary>
        /// Area
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _area = null;
        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _floor = null;
        /// --------------------------------------------------
        /// <summary>
        /// 区割No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kuwariNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// TagCode
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagCode = null;
        /// --------------------------------------------------
        /// <summary>
        /// TagNo.
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tagNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _boxNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _palletNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 木枠 No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kiwakuNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// Case No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _caseNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 実績日付From
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string _jissekiDateFrom = null;
        /// --------------------------------------------------
        /// <summary>
        /// 実績日付To
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string _jissekiDateTo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _workUserID = null;
        /// --------------------------------------------------
        /// <summary>
        /// 棚卸年月日
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _inventDate = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondI01()
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
        public CondI01(LoginInfo target)
            : base(target)
        {
        }
        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
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
        /// 納入先コード
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion

        #region 物件管理No.

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo
        {
            get { return this._bukkenNo; }
            set { this._bukkenNo = value; }
        }

        #endregion

        #region ロケーション

        /// --------------------------------------------------
        /// <summary>
        /// ロケーション
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Location
        {
            get { return this._location; }
            set { this._location = value; }
        }

        #endregion

        #region 在庫No.

        /// --------------------------------------------------
        /// <summary>
        /// 在庫No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZaikoNo
        {
            get { return this._zaikoNo; }
            set { this._zaikoNo = value; }
        }

        #endregion

        #region 在庫年月日

        /// --------------------------------------------------
        /// <summary>
        /// 在庫年月日
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string StockDate
        {
            get { return this._stockDate; }
            set { this._stockDate = value; }
        }

        #endregion

        #region 作業区分

        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SagyoFlag
        {
            get { return this._sagyoFlag; }
            set { this._sagyoFlag = value; }
        }

        #endregion

        #region AR No.

        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo
        {
            get { return this._arNo; }
            set { this._arNo = value; }
        }

        #endregion

        #region 品名(和文)

        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HinmeiJp
        {
            get { return this._hinmeiJp; }
            set { this._hinmeiJp = value; }
        }

        #endregion

        #region 品名

        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Hinmei
        {
            get { return this._hinmei; }
            set { this._hinmei = value; }
        }

        #endregion

        #region 図番/形式

        /// --------------------------------------------------
        /// <summary>
        /// 図番/形式
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ZumenKeishiki
        {
            get { return this._zumenKeishiki; }
            set { this._zumenKeishiki = value; }
        }

        #endregion

        #region Area

        /// --------------------------------------------------
        /// <summary>
        /// Area
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Area
        {
            get { return this._area; }
            set { this._area = value; }
        }

        #endregion

        #region Floor

        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Floor
        {
            get { return this._floor; }
            set { this._floor = value; }
        }

        #endregion

        #region 区割No.

        /// --------------------------------------------------
        /// <summary>
        /// 区割No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KuwariNo
        {
            get { return this._kuwariNo; }
            set { this._kuwariNo = value; }
        }

        #endregion

        #region TagCode

        /// --------------------------------------------------
        /// <summary>
        /// TagCode
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagCode
        {
            get { return this._tagCode; }
            set { this._tagCode = value; }
        }

        #endregion

        #region Tag No.

        /// --------------------------------------------------
        /// <summary>
        /// Tag No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo
        {
            get { return this._tagNo; }
            set { this._tagNo = value; }
        }

        #endregion

        #region Box No.

        /// --------------------------------------------------
        /// <summary>
        /// Box No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get { return this._boxNo; }
            set { this._boxNo = value; }
        }

        #endregion

        #region Pallet No.

        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo
        {
            get { return this._palletNo; }
            set { this._palletNo = value; }
        }

        #endregion

        #region 木枠 No.

        /// --------------------------------------------------
        /// <summary>
        /// 木枠 No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KiwakuNo
        {
            get { return this._kiwakuNo; }
            set { this._kiwakuNo = value; }
        }

        #endregion

        #region Case No.

        /// --------------------------------------------------
        /// <summary>
        /// Case No.
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseNo
        {
            get { return this._caseNo; }
            set { this._caseNo = value; }
        }

        #endregion

        #region 実績日付From

        /// --------------------------------------------------
        /// <summary>
        /// 実績日付From
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JissekiDateFrom
        {
            get { return this._jissekiDateFrom; }
            set { this._jissekiDateFrom = value; }
        }

        #endregion

        #region 実績日付To

        /// --------------------------------------------------
        /// <summary>
        /// 実績日付To
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public string JissekiDateTo
        {
            get { return this._jissekiDateTo; }
            set { this._jissekiDateTo = value; }
        }

        #endregion

        #region 作業者

        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string WorkUserID
        {
            get { return this._workUserID; }
            set { this._workUserID = value; }
        }

        #endregion

        #region 棚卸年月日

        /// --------------------------------------------------
        /// <summary>
        /// 作業者
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public string InventDate
        {
            get { return this._inventDate; }
            set { this._inventDate = value; }
        }

        #endregion

    }
}
