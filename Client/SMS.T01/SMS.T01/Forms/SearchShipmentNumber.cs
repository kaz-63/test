using System;
using System.Data;
using System.Linq;
using DSWUtil;
using WsConnection;
using Commons;
using GrapeCity.Win.ElTabelle;
using SMS.T01.Properties;
using WsConnection.WebRefT01;


namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// Ship参照画面
    /// </summary>
    /// <create>K.Tsutsumi 2019/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class SearchShipmentNumber : SystemBase.Forms.CustomOrderForm
    {

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>K.Tsutsumi 2019/03/08 手配情報表示</update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 7;

        /// --------------------------------------------------
        /// <summary>
        /// 外部連携用
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _tehaiRenkeiNo = string.Empty;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <param name="tehaiRenkeiNo">手配連携No.</param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public SearchShipmentNumber(UserInfo userInfo, string title, string tehaiRenkeiNo)
            : base(userInfo, title)
        {
            InitializeComponent();

            this._tehaiRenkeiNo = tehaiRenkeiNo;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // 画面の編集モード変更
                this.EditMode = SystemBase.EditMode.View;
                // 固定列設定
                this.shtMeisai.FreezeColumns = SHEET_COL_TOPLEFT_COL;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトルを設定
                // 列移動の際は、デザイン側も合わせて修正すること!!
                int row = 0;
                // 0
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_State;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ShipARNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_SetteiDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_NouhinSaki;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_SyukkaSaki;
                // 5
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_TagNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ProductNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Code;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Area;
                // 10
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Floor;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Model;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_MNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_JpName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Name;
                // 15
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_DrawingNoFormat;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_SectioningNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Quantity;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Free1;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Free2;
                // 20
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ZumenKeishiki2;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Maker;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_UnitPrice;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_Memo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_STNo;
                // 25
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ARNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_AssemblyDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_BoxNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_PalletNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_WoodFrameNo;
                // 30
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_BoxPackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_PalletPackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_WoodFramePackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ShippingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ShippingCompany;
                // 35
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_InvoiceNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_OkurijyoNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_BLNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_AcceptanceDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_AcceptanceStaff;
                // 40
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_GWRT;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_FileName;
                shtMeisai.ColumnHeaders[row++].Caption = string.Empty;                  // JYOTAI_FLAG(HIDE)
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_TehaiRenkeiNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_ShukkaFlag;
                // 45
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_TagNonyusakiCd;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_BukkenNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_EcsQuota;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_TehaiFlagName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_QuantityUnitName;
                // 50
                shtMeisai.ColumnHeaders[row++].Caption = Resources.SearchShipmentNumber_EstimateFlagName;

                // 検索
                this.RunSearch();

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                if (this.shtMeisai.MaxRows > 0)
                {
                    this.shtMeisai.Focus();
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 検索条件クリア
                this.txtProjectName.Clear();
                this.txtRenkeiNo.Clear();
                this.txtHinmeiJp.Clear();
                this.txtHinmei.Clear();
                this.txtTehaiQty.Clear();
                this.txtShukkaQty.Clear();
                this.txtShipmentNumbers.Clear();

                // シートクリア
                this.SheetClear();

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var cond = new CondT01(this.UserInfo);
                cond.TehaiRenkeiNo = this._tehaiRenkeiNo;

                var conn = new ConnT01();
                string errMsgID;
                string[] args;
                var ds = conn.GetShukkaMeisai(cond, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // 表示 - 検索条件
                this.txtProjectName.Text = UtilData.GetFld(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_M_PROJECT.BUKKEN_NAME, string.Empty);
                this.txtRenkeiNo.Text = UtilData.GetFld(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, string.Empty);
                this.txtHinmeiJp.Text = UtilData.GetFld(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_T_TEHAI_MEISAI.HINMEI_JP, string.Empty);
                this.txtHinmei.Text = UtilData.GetFld(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_T_TEHAI_MEISAI.HINMEI, string.Empty);
                this.txtTehaiQty.Text = UtilData.GetFldToInt32(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_T_TEHAI_MEISAI.TEHAI_QTY, 0).ToString("###,##0");
                this.txtShukkaQty.Text = UtilData.GetFldToInt32(ds, Def_T_TEHAI_MEISAI.Name, 0, Def_T_TEHAI_MEISAI.SHUKKA_QTY, 0).ToString("###,##0");

                // 表示 - シート
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
                this.shtMeisai.Enabled = true;
                this.shtMeisai.Redraw = true;

                var ships = ds.Tables[Def_T_SHUKKA_MEISAI.Name].AsEnumerable().Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP)).Distinct().ToArray();
                this.txtShipmentNumbers.Text = string.Join(", ", ships);
               
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

    }
}
