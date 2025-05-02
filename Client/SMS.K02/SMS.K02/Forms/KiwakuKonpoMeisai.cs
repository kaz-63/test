using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;

using WsConnection.WebRefK02;
using SMS.K02.Properties;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包明細登録
    /// </summary>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KiwakuKonpoMeisai : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// メッセージの種類
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum DispMessageType
        {
            ClearQuestion,
            CheckInputLen,
            CheckDuplication,
            UupdateVersionError,
        }

        #endregion

        #region Fields

        private Dictionary<DispMessageType, string> _dispMessages = new Dictionary<DispMessageType, string>();
        private string _kojiNo = string.Empty;
        private string _caseID = string.Empty;
        private DataTable _dtMeisai = null;
        private DataSet _dsGetData = new DataSet();

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// シートで使用するフィールド名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected const string FLD_TARGET_NO = "TargetNo";

        /// --------------------------------------------------
        /// <summary>
        /// シートで使用する入力Noの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected const int COL_TARGET_NO = 0;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNoの長さ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected const int LEN_PALLET_NO = 6;

        /// --------------------------------------------------
        /// <summary>
        /// パレットNoの行数
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CNT_PALLET_NO = 10;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private KiwakuKonpoMeisai()
            : base(new UserInfo())
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="kojiNo">工事識別NO</param>
        /// <param name="caseID">内部管理用キー</param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update>H.Tajimi 2019/01/07 木枠梱包画面で明細表示前のグリッド位置復元</update>
        /// --------------------------------------------------
        public KiwakuKonpoMeisai(UserInfo userInfo, string kojiNo, string caseID)
            : base(userInfo)
        {
            InitializeComponent();
            this.Title = ComDefine.TITLE_K0200050;
            this.KojiNo = kojiNo;
            this.CaseID = caseID;
            this.CondKojiNo = kojiNo;
            this.CondCaseID = caseID;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 画面メッセージ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual Dictionary<DispMessageType, string> DispMessages
        {
            get { return this._dispMessages; }
            set { this._dispMessages = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 内部管理用キー
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string CaseID
        {
            get { return this._caseID; }
            set { this._caseID = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別NO
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string KojiNo
        {
            get { return this._kojiNo; }
            set { this._kojiNo = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string TorokuFlag
        {
            get { return TOROKU_FLAG.NAI_VALUE1; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細グリッドに使用するデータテーブル
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DataTable DtMeisai
        {
            get
            {
                if (this._dtMeisai == null)
                {
                    this._dtMeisai = this.GetSchemeSheetDataSource();
                }
                return this._dtMeisai;
            }
            set
            {
                if (this._dtMeisai == null)
                {
                    this._dtMeisai = this.GetSchemeSheetDataSource();
                }
                this._dtMeisai.Rows.Clear();
                if (value != null)
                {
                    this._dtMeisai.Merge(value);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 表示に使用したDataSet
        /// </summary>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DataSet DsGetData
        {
            get { return this._dsGetData; }
            set { this._dsGetData = value; }
        }

        #region 表示する際に使用した条件

        /// --------------------------------------------------
        /// <summary>
        /// 表示する際に使用した工事識別No
        /// </summary>
        /// <create>H.Tajimi 2019/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CondKojiNo { get; private set; }
        /// --------------------------------------------------
        /// <summary>
        /// 表示する際に使用したケースID
        /// </summary>
        /// <create>H.Tajimi 2019/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CondCaseID { get; private set; }

        #endregion

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            if (this.DesignMode) return;
            try
            {
                // 更新処理固定
                this.EditMode = SystemBase.EditMode.Update;
                // ----- メッセージの設定 -----
                this.SetDispMessageID(DispMessageType.CheckInputLen, null);
                // パレットNo.をクリアします。\r\nよろしいですか？
                this.SetDispMessageID(DispMessageType.ClearQuestion, "K0200050001");
                // {0}行目のパレットNo.[{1}]は存在しています。
                this.SetDispMessageID(DispMessageType.CheckDuplication, "K0200050002");
                // 他端末で更新された為、更新できませんでした。
                this.SetDispMessageID(DispMessageType.UupdateVersionError, "A9999999027");
                // グリッドの初期化
                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.ShortCuts.Remove(Keys.Enter);
                this.shtMeisai.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextCellWithWrap });
                this.shtMeisai.Columns[0].DataField = FLD_TARGET_NO;
                this.shtMeisai.DataSource = this.DtMeisai;
                if (!this.RunSearch())
                {
                    this.fbrFunction.F01Button.Enabled = false;
                    this.fbrFunction.F03Button.Enabled = false;
                    this.fbrFunction.F04Button.Enabled = false;
                }
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;
                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.KiwakuKonpoMeisai_PalletNo;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.nudGrossW.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // 入力値のクリア
                this.txtKojiName.Text = string.Empty;
                this.txtShip.Text = string.Empty;
                this.txtKojiNo.Text = string.Empty;
                this.txtCaseNo.Text = string.Empty;
                this.txtStyle.Text = string.Empty;
                this.txtDimentionL.Text = string.Empty;
                this.txtDimentionW.Text = string.Empty;
                this.txtDimentionH.Text = string.Empty;
                this.txtMMNet.Text = string.Empty;
                this.txtMokuzaiJyuryo.Text = string.Empty;
                this.nudGrossW.Value = 0;
                this.txtNetW.Text = string.Empty;
                this.txtItem.Text = string.Empty;
                this.txtDescription1.Text = string.Empty;
                this.txtDescription2.Text = string.Empty;
                // グリッド
                this.SheetClear();

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update>H.Tajimi 2019/01/08 木枠梱包業務改善</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.shtMeisai.EditState) return false;
                this.ClearMessage();
                List<string> inputNoList = new List<string>();
                for (int i = 0; i < this.DtMeisai.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO))) continue;
                    string inputNo = this.GetSheetInputFormat(ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO));
                    // フォーマット後一応再セット
                    this.DtMeisai.Rows[i][FLD_TARGET_NO] = inputNo;
                    if (inputNoList.Contains(inputNo))
                    {
                        this.ShowMessage(this.DispMessages[DispMessageType.CheckDuplication], (i + 1).ToString(), inputNo);
                        this.shtMeisai.ActivePosition = new Position(COL_TARGET_NO, i);
                        this.shtMeisai.Focus();
                        return false;
                    }
                    inputNoList.Add(inputNo);
                }

                if (UtilData.ExistsData(this.DsGetData, Def_T_KIWAKU.Name))
                {
                    var nonyusakiCd = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.NONYUSAKI_CD);
                    if (!string.IsNullOrEmpty(nonyusakiCd))
                    {
                        var insertType = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.INSERT_TYPE);
                        if (insertType == KIWAKU_INSERT_TYPE.AR_VALUE1 || insertType == KIWAKU_INSERT_TYPE.REGULAR_VALUE1)
                        {
                            // 登録(AR)、登録(本体)の場合は便間移動不可なのでNOP
                        }
                        else
                        {
                            // 登録(本体便まとめ)の場合は便間移動チェック処理
                            if (this.UserInfo.SysInfo.KiwakuKonpoMoveShipFlag == KIWAKU_KONPO_MOVE_SHIP.ENABLE_VALUE1)
                            {
                                if (!this.ConfirmMoveShip())
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();
                cond.KojiNo = this.KojiNo;
                cond.CaseID = this.CaseID;
                cond.TorokuFlag = this.TorokuFlag;

                string errMsgID;
                string[] args;
                DataSet ds = conn.GetKiwakuKonpoMeisaiData(cond, out errMsgID, out args);
                // 取得データを退避
                this.DsGetData = ds;
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                DataTable dt = ds.Tables[Def_T_KIWAKU_MEISAI.Name];

                this.txtKojiName.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.KOJI_NAME);
                this.txtShip.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.SHIP);
                this.txtKojiNo.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.KOJI_NO);
                this.txtCaseNo.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.CASE_NO);
                this.txtStyle.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.STYLE);
                this.txtDimentionL.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_L);
                this.txtDimentionW.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_W);
                this.txtDimentionH.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DIMENSION_H);
                this.txtMMNet.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.MMNET);
                this.txtMokuzaiJyuryo.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO);
                this.nudGrossW.Value = ComFunc.GetFldToDecimal(dt, 0, Def_T_KIWAKU_MEISAI.GROSS_W);
                this.txtNetW.Value = this.nudGrossW.Value - this.txtMokuzaiJyuryo.Value;
                this.txtItem.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.ITEM);
                this.txtDescription1.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DESCRIPTION_1);
                this.txtDescription2.Text = ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.DESCRIPTION_2);

                this.SheetClear();
                if (this.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
                {
                    this.SetSheetDispData(dt);
                }
                else
                {
                    this.SetSheetDispData(ds.Tables[Def_T_SHAGAI_KIWAKU_MEISAI.Name]);
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 画面を閉じる
                    this.DialogResult = DialogResult.OK;
                    // メッセージが出ないようにする。
                    this.IsCloseQuestion = false;
                    this.Close();
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update>H.Tajimi 2019/01/08 木枠梱包業務改善</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 画面表示データ取得
                DataSet ds = this.GetDisplayData();

                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();
                cond.KojiNo = this.KojiNo;
                cond.CaseID = this.CaseID;
                cond.TorokuFlag = this.TorokuFlag;
                cond.KiwakuKonpoMoveShipFlag = this.UserInfo.SysInfo.KiwakuKonpoMoveShipFlag;
                cond.KiwakuNonyusakiCD = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.NONYUSAKI_CD);
                cond.KiwakuInsertType = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.INSERT_TYPE);

                string errMsgID;
                string[] args;
                if (!conn.UpdKiwakuKonpoMeisaiToroku(ds, cond, out errMsgID, out args))
                {
                    if (this.IsRefresh(errMsgID))
                    {
                        this.DisplayRefresh();
                    }
                    this.ShowMessage(errMsgID, args);
                    if (args != null && -1 < UtilConvert.ToInt32(args[0], 0))
                    {
                        int rowIndex = UtilConvert.ToInt32(args[0], 0) - 1;
                        this.shtMeisai.ActivePosition = new Position(COL_TARGET_NO, rowIndex);
                        this.shtMeisai.Focus();
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                if (this.ShowMessage(this.DispMessages[DispMessageType.ClearQuestion]) != DialogResult.OK) return;
                this.SheetClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region GROSS/W

        /// --------------------------------------------------
        /// <summary>
        /// フォーカスを受け取った場合のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void nudGrossW_Enter(object sender, EventArgs e)
        {
            try
            {
                this.nudGrossW.Select(0, nudGrossW.Text.Length);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検証イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void nudGrossW_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtNetW.Value = this.nudGrossW.Value - this.txtMokuzaiJyuryo.Value;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 明細グリッド

        #region ClippingData

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリップボード操作が行われたときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void shtMeisai_ClippingData(object sender, ClippingDataEventArgs e)
        {
            try
            {
                switch (e.ClippingAction)
                {
                    case ClippingAction.Paste:
                        switch (this.shtMeisai.ActivePosition.Column)
                        {
                            case COL_TARGET_NO:
                                // パレットNo.列の貼り付けはキャンセルする。
                                e.Cancel = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region LeaveEdit

        /// --------------------------------------------------
        /// <summary>
        /// セルが非編集モードに入る場合に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                if (this.shtMeisai.ActivePosition.Column != COL_TARGET_NO || string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Text))
                {
                    return;
                }
                this.ClearMessage();
                // 入力値の整形
                this.shtMeisai.ActiveCell.Text = this.GetSheetInputFormat(this.shtMeisai.ActiveCell.Text);
                this.shtMeisai.Refresh();
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (i == this.shtMeisai.ActivePosition.Row) continue;
                    if (this.shtMeisai[COL_TARGET_NO, i].Text == this.shtMeisai.ActiveCell.Text)
                    {
                        this.ShowMessage(this.DispMessages[DispMessageType.CheckDuplication], (this.shtMeisai.ActivePosition.Row + 1).ToString(), this.shtMeisai.ActiveCell.Text);
                        e.Cancel = true;
                        return;
                    }
                }
                // 入力長チェック
                if (!this.CheckSheetInputLength(this.shtMeisai.ActiveCell.Text))
                {
                    if (!string.IsNullOrEmpty(this.DispMessages[DispMessageType.CheckInputLen]))
                    {
                        this.ShowMessage(this.DispMessages[DispMessageType.CheckInputLen], (this.shtMeisai.ActivePosition.Row + 1).ToString());
                    }
                    e.Cancel = true;
                    return;
                }
                // 行入力チェック
                if (!this.CheckSheetLine(this.shtMeisai.ActivePosition.Row))
                {
                    e.Cancel = true;
                    return;
                }
                if (this.shtMeisai.ActivePosition.Row == 0)
                {
                    // 1行目の入力値チェック
                    if (!this.CheckSheetLineOne())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                e.Cancel = true;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            this.DtMeisai = this.GetSchemeSheetDataSource();
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 表示用メッセージIDの設定

        /// --------------------------------------------------
        /// <summary>
        /// 表示用メッセージの設定
        /// </summary>
        /// <param name="type">メッセージ種類</param>
        /// <param name="messageID">メッセージID</param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetDispMessageID(DispMessageType type, string messageID)
        {
            if (!this.DispMessages.ContainsKey(type))
            {
                this.DispMessages.Add(type, messageID);
            }
            else
            {
                this.DispMessages[type] = messageID;
            }
        }
        #endregion

        #region メッセージIDチェック

        /// --------------------------------------------------
        /// <summary>
        /// 画面を再描画させる必要のあるメッセージかチェック
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>true:必要/false:不要</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool IsRefresh(string messageID)
        {
            List<string> msgs = new List<string>();
            msgs.Add(this.DispMessages[DispMessageType.UupdateVersionError]);

            if (msgs.Contains(messageID))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 画面再描画(再検索)

        /// --------------------------------------------------
        /// <summary>
        /// 画面再描画(再検索)
        /// </summary>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void DisplayRefresh()
        {
            // 再描画
            if (!this.RunSearch())
            {
                // 再描画時に失敗したので画面をクリアする。
                this.DisplayClear();
            }
        }

        #endregion

        #region グリッドで使用するデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// グリッドで使用するデータテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DataTable GetSchemeSheetDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(FLD_TARGET_NO, typeof(string));
            for (int i = 0; i < CNT_PALLET_NO; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }

        #endregion

        #region シートの入力値のフォーマット

        /// --------------------------------------------------
        /// <summary>
        /// シートの入力値のフォーマット
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetSheetInputFormat(string value)
        {
            string ret = value;
            if (!value.StartsWith(ComDefine.PREFIX_PALLETNO) && value.Length < LEN_PALLET_NO)
            {
                ret = ComDefine.PREFIX_PALLETNO + value.PadLeft(LEN_PALLET_NO - 1, '0');
            }
            return ret;
        }

        #endregion

        #region グリッドの行入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの行入力チェック
        /// </summary>
        /// <param name="targetRow">入力された行インデックス</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool CheckSheetLine(int targetRow)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region グリッドの1行目の入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの1行目の入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update>H.Tajimi 2015/11/24 M-No対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected virtual bool CheckSheetLineOne()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();
                cond.PalletNo = this.shtMeisai.ActiveCell.Text;
                DataSet ds = conn.GetKiwakuKonpoShukkaMeisaiFirstRowData(cond);

                // 1行目で入力した内容と木枠の納入先/便を比較
                if (!this.CheckSheetListOneDiffKiwakuAndShukka(ds, this.shtMeisai.ActiveCell.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.txtItem.Text))
                {
                    this.txtItem.Text = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KISHU);
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region グリッドの入力値の長さチェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの入力値の長さチェック
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool CheckSheetInputLength(string value)
        {
            return true;
        }

        #endregion

        #region グリッドの表示データ作成

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの表示データ作成
        /// </summary>
        /// <param name="dt">表示するデータ</param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetSheetDispData(DataTable dt)
        {
            for (int i = this.DtMeisai.Rows.Count; i < CNT_PALLET_NO; i++)
            {
                this.DtMeisai.Rows.Add(this.DtMeisai.NewRow());
            }
            this.DtMeisai.Rows[0][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_1);
            this.DtMeisai.Rows[1][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_2);
            this.DtMeisai.Rows[2][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_3);
            this.DtMeisai.Rows[3][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_4);
            this.DtMeisai.Rows[4][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_5);
            this.DtMeisai.Rows[5][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_6);
            this.DtMeisai.Rows[6][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_7);
            this.DtMeisai.Rows[7][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_8);
            this.DtMeisai.Rows[8][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_9);
            this.DtMeisai.Rows[9][FLD_TARGET_NO] = ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_10);
        }

        #endregion

        #region 更新用木枠明細のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 更新用木枠明細のデータテーブル
        /// </summary>
        /// <returns>更新用木枠明細のデータテーブル</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
        /// --------------------------------------------------
        protected virtual DataTable GetSchemeKiwakuMeisai()
        {
            DataTable dt = new DataTable(Def_T_KIWAKU_MEISAI.Name);

            dt.Columns.Add(Def_T_KIWAKU_MEISAI.KOJI_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.CASE_ID, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.ITEM, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DESCRIPTION_1, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.DESCRIPTION_2, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.NET_W, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.GROSS_W, typeof(decimal));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_1, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_2, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_3, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_4, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_5, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_6, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_7, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_8, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_9, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.PALLET_NO_10, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU_MEISAI.VERSION, typeof(DateTime));

            return dt;
        }

        #endregion

        #region 画面表示内容取得

        /// --------------------------------------------------
        /// <summary>
        /// 画面表示内容取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DataSet GetDisplayData()
        {
            DataTable dt = this.GetSchemeKiwakuMeisai();
            DataRow dr = dt.NewRow();
            dr[Def_T_KIWAKU_MEISAI.KOJI_NO] = this.KojiNo;
            dr[Def_T_KIWAKU_MEISAI.CASE_ID] = this.CaseID;
            dr[Def_T_KIWAKU_MEISAI.ITEM] = this.txtItem.Text;
            dr[Def_T_KIWAKU_MEISAI.DESCRIPTION_1] = this.txtDescription1.Text;
            dr[Def_T_KIWAKU_MEISAI.DESCRIPTION_2] = this.txtDescription2.Text;
            dr[Def_T_KIWAKU_MEISAI.NET_W] = this.txtNetW.Value;
            dr[Def_T_KIWAKU_MEISAI.GROSS_W] = this.nudGrossW.Value;
            dr[Def_T_KIWAKU_MEISAI.VERSION] = ComFunc.GetFldObject(this.DsGetData, Def_T_KIWAKU_MEISAI.Name, 0, Def_T_KIWAKU_MEISAI.VERSION);
            // パレットNo.の取得
            string[] fields = new string[] { Def_T_KIWAKU_MEISAI.PALLET_NO_1,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_2,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_3,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_4,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_5,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_6,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_7,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_8,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_9,
                                            Def_T_KIWAKU_MEISAI.PALLET_NO_10};
            for (int i = 0; i < CNT_PALLET_NO; i++)
            {
                if (!string.IsNullOrEmpty(ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO)))
                {
                    dr[fields[i]] = ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO);
                }
            }
            dt.Rows.Add(dr);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        #endregion

        #region 便間移動処理継続確認

        /// --------------------------------------------------
        /// <summary>
        /// 便間移動処理継続確認
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool ConfirmMoveShip()
        {
            // 画面表示データ取得
            DataSet ds = this.GetDisplayData();

            CondK02 cond = new CondK02(this.UserInfo);
            ConnK02 conn = new ConnK02();
            cond.KojiNo = this.KojiNo;
            cond.CaseID = this.CaseID;
            cond.TorokuFlag = this.TorokuFlag;
            cond.KiwakuKonpoMoveShipFlag = this.UserInfo.SysInfo.KiwakuKonpoMoveShipFlag;

            string errMsgID = string.Empty;
            string[] args;

            // 便間移動データ取得
            var dsMoveShip = conn.GetMoveShipData(ds, cond, out errMsgID, out args);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                this.ShowMessage(errMsgID, args);
                return false;
            }

            // 処理継続確認メッセージを表示
            if (this.ShowConfirmMoveShip(dsMoveShip) != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 処理継続確認メッセージを表示

        /// --------------------------------------------------
        /// <summary>
        /// 処理継続確認メッセージを表示
        /// </summary>
        /// <param name="dsMoveShip">便間移動データ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DialogResult ShowConfirmMoveShip(DataSet dsMoveShip)
        {
            var ret = DialogResult.OK;
            if (ComFunc.IsExistsData(dsMoveShip, ComDefine.DTTBL_MOVE_SHIP_PALLET))
            {
                // 便間移動確認メッセージを表示する場合は、既存の確認メッセージを非表示にする
                this.MsgUpdateConfirm = string.Empty;
                var dtPallet = dsMoveShip.Tables[ComDefine.DTTBL_MOVE_SHIP_PALLET];
                var dtMultiMsg = ComFunc.GetSchemeMultiMessage();
                foreach (DataRow drPallet in dtPallet.Rows)
                {
                    ComFunc.AddMultiMessage(dtMultiMsg, "K0200050013", ComFunc.GetFld(drPallet, Def_T_SHUKKA_MEISAI.PALLET_NO));
                }
                ret = this.ShowMultiMessage(dtMultiMsg, "K0200050012");
            }
            return ret;
        }

        #endregion

        #region グリッドの1行目入力チェック(木枠と出荷明細)

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの1行目入力チェック(木枠と出荷明細)
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="cellText">パレットNoまたは現品TagNo</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool CheckSheetListOneDiffKiwakuAndShukka(DataSet ds, string cellText)
        {
            if (UtilData.ExistsData(this.DsGetData, Def_T_KIWAKU.Name))
            {
                // 木枠データの登録種別を取得し、登録(本体)または登録(AR)の場合は出荷明細と木枠の納入先を比較する
                var kiwakuNonyusakiCd = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.NONYUSAKI_CD);
                var insertType = ComFunc.GetFld(this.DsGetData, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.INSERT_TYPE);
                if (!string.IsNullOrEmpty(kiwakuNonyusakiCd))
                {
                    if (insertType == KIWAKU_INSERT_TYPE.AR_VALUE1 || insertType == KIWAKU_INSERT_TYPE.REGULAR_VALUE1)
                    {
                        if (UtilData.ExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                        {
                            var shukkaNonyusakiCd = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                            // 登録(AR)、登録(本体)の場合
                            if (kiwakuNonyusakiCd != shukkaNonyusakiCd)
                            {
                                // 木枠の納入先と出荷明細の納入先が異なる
                                this.ShowMessageDiffKiwakuAndShukka(cellText);
                                return false;
                            }
                        }
                        else
                        {
                            // 木枠の納入先と出荷明細の納入先が異なる
                            this.ShowMessageDiffKiwakuAndShukka(cellText);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region 木枠と出荷明細の納入先/便が異なる場合のエラーメッセージ出力

        /// --------------------------------------------------
        /// <summary>
        /// 木枠と出荷明細の納入先/便が異なる場合のエラーメッセージ出力
        /// </summary>
        /// <param name="no">パレットNo</param>
        /// <create>H.Tajimi 2019/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void ShowMessageDiffKiwakuAndShukka(string no)
        {
            // 木枠とPalletNo.[{0}]は異なる納入先、便です。
            this.ShowMessage("K0200050015", no);
        }

        #endregion
    }
}
