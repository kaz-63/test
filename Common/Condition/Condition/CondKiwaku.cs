using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

namespace Condition
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ用コンディション
    /// </summary>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CondKiwaku : CondBase
    {

        #region Fields

        // 工事識別管理NO
        private string _kojiNo = null;

        // 工事識別名称
        private string _kojiName = null;

        // 出荷便
        private string _ship = null;

        // 登録区分
        private string _torokuFlag = null;

        // (A)CASE MARK
        private string _caseMarkFile = null;

        // (B)DELIVERY NO
        private string _deliveryNo = null;

        // (C)PORT OF DESTINATION
        private string _portOfDestination = null;

        // (D)AIR/BOAT
        private string _airBoat = null;

        // (E)DELIVERY DATE
        private string _deliveryDate = null;

        // (F)DELIVERY POINT
        private string _delivertyPoint = null;

        // (G)FACTORY
        private string _factory = null;

        // REMARKS
        private string _remarks = null;

        // 作業区分
        private string _sagyoFlag = null;

        // 出荷日付
        private string _shukkaDate = null;

        // 出荷ユーザーID
        private string _shukkaUserID = null;

        // 出荷ユーザー名
        private string _shukkaUserName = null;

        // 運送会社
        private string _unsokaishaName = null;

        // インボイスNO
        private string _invoiceNo = null;

        // 送り状NO
        private string _okurijyoNo = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>K.Tsutsumi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondKiwaku()
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
        public CondKiwaku(LoginInfo target)
            : base(target)
        {
        }

        #endregion

        #region 工事識別管理NO

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別管理NO
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiNo
        {
            get
            {
                return _kojiNo;
            }
            set
            {
                _kojiNo = value;
            }
        }

        #endregion

        #region 工事識別名称

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別名称
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string KojiName
        {
            get
            {
                return _kojiName;
            }
            set
            {
                _kojiName = value;
            }
        }

        #endregion
        
        #region 出荷便

        /// --------------------------------------------------
        /// <summary>
        /// 出荷便
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ship
        {
            get
            {
                return _ship;
            }
            set
            {
                _ship = value;
            }
        }

        #endregion

        #region 登録区分

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string TorokuFlag
        {
            get
            {
                return _torokuFlag;
            }
            set
            {
                _torokuFlag = value;
            }
        }

        #endregion

        #region (A)CASE MARK

        /// --------------------------------------------------
        /// <summary>
        /// (A)CASE MARK
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CaseMarkFile
        {
            get
            {
                return _caseMarkFile;
            }
            set
            {
                _caseMarkFile = value;
            }
        }

        #endregion

        #region (B)DELIVERY NO

        /// --------------------------------------------------
        /// <summary>
        /// (B)DELIVERY NO
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DeliveryNo
        {
            get
            {
                return _deliveryNo;
            }
            set
            {
                _deliveryNo = value;
            }
        }

        #endregion

        #region (C)PORT OF DESTINATION

        /// --------------------------------------------------
        /// <summary>
        /// (C)PORT OF DESTINATION
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string PortOfDestination
        {
            get
            {
                return _portOfDestination;
            }
            set
            {
                _portOfDestination = value;
            }
        }

        #endregion

        #region (D)AIR/BOAT

        /// --------------------------------------------------
        /// <summary>
        /// (D)AIR/BOAT
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string AirBoat
        {
            get
            {
                return _airBoat;
            }
            set
            {
                _airBoat = value;
            }
        }

        #endregion

        #region (E)DELIVERY DATE

        /// --------------------------------------------------
        /// <summary>
        /// (E)DELIVERY DATE
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DeliveryDate
        {
            get
            {
                return _deliveryDate;
            }
            set
            {
                _deliveryDate = value;
            }
        }

        #endregion

        #region (F)DELIVERY POINT

        /// --------------------------------------------------
        /// <summary>
        /// (F)DELIVERY POINT
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string DelivertyPoint
        {
            get
            {
                return _delivertyPoint;
            }
            set
            {
                _delivertyPoint = value;
            }
        }

        #endregion

        #region (G)FACTORY

        /// --------------------------------------------------
        /// <summary>
        /// (G)FACTORY
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                _factory = value;
            }
        }

        #endregion

        #region REMARKS

        /// --------------------------------------------------
        /// <summary>
        /// REMARKS
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                _remarks = value;
            }
        }

        #endregion

        #region 作業区分

        /// --------------------------------------------------
        /// <summary>
        /// 作業区分
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SagyoFlag
        {
            get
            {
                return _sagyoFlag;
            }
            set
            {
                _sagyoFlag = value;
            }
        }

        #endregion

        #region 出荷日付

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日付
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaDate
        {
            get
            {
                return _shukkaDate;
            }
            set
            {
                _shukkaDate = value;
            }
        }

        #endregion

        #region 出荷ユーザーID

        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザーID
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaUserID
        {
            get
            {
                return _shukkaUserID;
            }
            set
            {
                _shukkaUserID = value;
            }
        }

        #endregion

        #region 出荷ユーザー名

        /// --------------------------------------------------
        /// <summary>
        /// 出荷ユーザー名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ShukkaUserName
        {
            get
            {
                return _shukkaUserName;
            }
            set
            {
                _shukkaUserName = value;
            }
        }

        #endregion

        #region 運送会社

        /// --------------------------------------------------
        /// <summary>
        /// 運送会社
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string UnsokaishaName
        {
            get
            {
                return _unsokaishaName;
            }
            set
            {
                _unsokaishaName = value;
            }
        }

        #endregion

        #region インボイスNO

        /// --------------------------------------------------
        /// <summary>
        /// インボイスNO
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string InvoiceNo
        {
            get
            {
                return _invoiceNo;
            }
            set
            {
                _invoiceNo = value;
            }
        }

        #endregion

        #region 送り状NO

        /// --------------------------------------------------
        /// <summary>
        /// 送り状NO
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string OkurijyoNo
        {
            get
            {
                return _okurijyoNo;
            }
            set
            {
                _okurijyoNo = value;
            }
        }

        #endregion

        #region 木枠梱包

        /// --------------------------------------------------
        /// <summary>
        /// 木枠梱包
        /// </summary>
        /// <create>T.Wakamatsu 2016/03/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool KiwakuKonpo { get; set; }

        #endregion

    }
}
