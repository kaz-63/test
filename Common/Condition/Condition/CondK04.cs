using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// K04用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondK04 : CondBase
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tempID = null;
        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _torikomiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _statusFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private object _torikomiDate = null;
        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _errorNum = 0;
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _boxNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _palletNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = null;
        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/10/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _result = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiNo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private decimal _nyukaQty = 0M;
        /// --------------------------------------------------
        /// <summary>
        /// 保留のリトライかどうか
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _horyuRetry = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK04()
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
        public CondK04(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 一時取込ID

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込ID
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TempID
        {
            get { return this._tempID; }
            set { this._tempID = value; }
        }

        #endregion

        #region 取込区分

        /// --------------------------------------------------
        /// <summary>
        /// 取込区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TorikomiFlag
        {
            get { return this._torikomiFlag; }
            set { this._torikomiFlag = value; }
        }

        #endregion

        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public string StatusFlag
        {
            get { return this._statusFlag; }
            set { this._statusFlag = value; }
        }

        #endregion

        #region 取込日時

        /// --------------------------------------------------
        /// <summary>
        /// 取込日時
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public object TorikomiDate
        {
            get { return this._torikomiDate; }
            set { this._torikomiDate = value; }
        }

        #endregion

        #region エラー数

        /// --------------------------------------------------
        /// <summary>
        /// エラー数
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public int ErrorNum
        {
            get { return this._errorNum; }
            set { this._errorNum = value; }
        }

        #endregion

        #region BoxNo

        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BoxNo
        {
            get { return this._boxNo; }
            set { this._boxNo = value; }
        }

        #endregion

        #region パレットNo

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PalletNo
        {
            get { return this._palletNo; }
            set { this._palletNo = value; }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/19</create>
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
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public string NonyusakiCD
        {
            get { return this._nonyusakiCD; }
            set { this._nonyusakiCD = value; }
        }

        #endregion

        #region 状態区分

        /// --------------------------------------------------
        /// <summary>
        /// 状態区分
        /// </summary>
        /// <create>Y.Higuchi 2010/10/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Result
        {
            get { return this._result; }
            set { this._result = value; }
        }

        #endregion

        #region 手配No

        /// --------------------------------------------------
        /// <summary>
        /// 手配No
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TehaiNo
        {
            get { return this._tehaiNo; }
            set { this._tehaiNo = value; }
        }

        #endregion

        #region 入荷数

        /// --------------------------------------------------
        /// <summary>
        /// 入荷数
        /// </summary>
        /// <create>H.Tajimi 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public decimal NyukaQty
        {
            get { return this._nyukaQty; }
            set { this._nyukaQty = value; }
        }

        #endregion

        #region 操作区分

        /// --------------------------------------------------
        /// <summary>
        /// 操作区分
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HandyOpeFlag { get; set; }

        #endregion

        #region 日付(From)

        /// --------------------------------------------------
        /// <summary>
        /// 日付(From)
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime DateFrom { get; set; }

        #endregion

        #region 日付(To)

        /// --------------------------------------------------
        /// <summary>
        /// 日付(To)
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTime DateTo { get; set; }

        #endregion

        #region Handyログインユーザー

        /// --------------------------------------------------
        /// <summary>
        /// Handyログインユーザー
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HandyLoginID { get; set; }

        #endregion

        #region 便

        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship { get; set; }

        #endregion

        #region ARNo.

        /// --------------------------------------------------
        /// <summary>
        /// ARNo.
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARNo { get; set; }

        #endregion

        #region TagNo.

        /// --------------------------------------------------
        /// <summary>
        /// TagNo.
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TagNo { get; set; }

        #endregion

        #region ProjectNo

        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ProjectNo { get; set; }

        #endregion

        #region 物件No

        /// --------------------------------------------------
        /// <summary>
        /// 物件No
        /// </summary>
        /// <create>H.Tajimi 2019/08/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public string BukkenNo { get; set; }
        
        #endregion

        #region 保留リトライ

        /// --------------------------------------------------
        /// <summary>
        /// 保留リトライ
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool HoryuRetry
        {
            get { return this._horyuRetry; }
            set { this._horyuRetry = value; }
        }

        #endregion
    }
}
