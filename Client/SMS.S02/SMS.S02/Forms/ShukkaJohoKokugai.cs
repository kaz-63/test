using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefAttachFile;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using SMS.E01;
using WsConnection.WebRefS02;
using SMS.S02.Forms;
using SMS.S02.Properties;

namespace SMS.S02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ShippingDocument作成
    /// </summary>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaJohoKokugai : SystemBase.Forms.CustomOrderForm
    {

        #region enum

        /// --------------------------------------------------
        /// <summary>
        /// DB取得時のフィールドカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        enum DB_FIELD
        {
            UNSOKAISHA_NAME = 0,
            INVOICE_NO,
            SHUKKA_FLAG,
            BUKKEN_NAME,
            SHIP,
            AR_NO,
            ECS_NO,
            ATTN,
            CONSIGNEE,
            CONSIGNEE_ATTN,
            DELIVERYTO,
            DELIVERYTO_ATTN,
            INTERNAL_PO_NO,
            TRADE_TERMS_FLAG,
            TRADE_TERMS_ATTR,
            SUBJECT,
            VERSION,
            ESTIMATE_FLAG,
            PACKING_NO,
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートインデックス
        /// </summary>
        /// <create>T.Nakata 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        enum SHEET_COL
        {
            PRINT = 0,
            UNSOKAISYA,
            INVOICE,
            AR,
            ITEM,
            SHIP,
            AR_NO,
            ECS_NO,
            ATTN,
            CONSIGNEE,
            CONSIGNEE_ATTN,
            DELIVERYTO,
            DELIVERYTO_ATTN,
            INTERNAL_PO_NO,
            TRADE_TERMS_FLAG,
            TRADE_TERMS_ATTR,
            SUBJECT,
            ESTIMATE_FLAG,
            LINE,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// シートフィールド名：印刷チェックボックス
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string DB_FLD_PRINT = "PRINT";
        /// --------------------------------------------------
        /// <summary>
        /// シートフィールド名：行数
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string DB_FLD_LINE = "LINE";
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int TOPLEFT_COL = 1;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 一覧表示用データ退避
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSheet = null;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受マスタ
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtConsign = null;
        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタ
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDeliver = null;
        /// --------------------------------------------------
        /// <summary>
        /// 貿易条件フラグ
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTradeTermsFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 更新用テーブル
        /// </summary>
        /// <create>T.Nakata 2018/12/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtUpdTable = null;
        /// --------------------------------------------------
        /// <summary>
        /// 帳票テンプレート
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _DownloadFileList = new string[] { ComDefine.EXCEL_FILE_TEMP_INVOICE
                                                          , ComDefine.EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT
                                                          , ComDefine.EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT};
        /// --------------------------------------------------
        /// <summary>
        /// 帳票保存パス
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _SaveFilePath = null;
        /// --------------------------------------------------
        /// <summary>
        /// Consignee列変更管理フラグ
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isConsigneeValueChanged = false;
        /// --------------------------------------------------
        /// <summary>
        /// DeliveryTo列変更管理フラグ
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isDeliveryToValueChanged = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaJohoKokugai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtShipping);
                this.shtShipping.DataBindingMode = DataBindingMode.Tight;

                // シートのタイトルを設定
                int i = 0;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Print;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Unsokaisha;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Invoice;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Ar;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Item;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Ship;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_ArNo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_EcsNo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Attn;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Consignee;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_ConsigneeAttn;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_DeliveryTo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_DeliveryToAttn;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_InternalPoNo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_TradeTermsFlag;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_TradeTermsAttr;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokugai_Subject;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboHakkoFlag, HAKKO_FLAG.GROUPCD);

                // データ取得
                this.GetConsign();          // 荷受マスタ
                this.GetDeliver();          // 配送先マスタ
                this.GetTradeTermsFlag();   // 貿易条件

                ////ここでコンボボックスの設定をしたいが読み込み時エラーとなる為各行毎にコンボボックスを設定する
                //this.SetComboDt((int)SHEET_COL.CONSIGNEE, -1, this.GetConsign(), Def_M_CONSIGN.NAME, Def_M_CONSIGN.CONSIGN_CD, false, false);
                //this.SetComboDt((int)SHEET_COL.DELIVERYTO, -1, this.GetDeliver(), Def_M_DELIVER.NAME, Def_M_DELIVER.DELIVER_CD, false, false);
                //this.SetComboDt((int)SHEET_COL.TRADE_TERMS_FLAG, -1, this.GetTradeTermsFlag(), Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false, false);

                this.EditMode = SystemBase.EditMode.None;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.dtpShukkaDateFrom.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region クリア

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update>K.Tsutsumi 2019/03/23 シート制御修正</update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // 最も左上に表示されているセルの設定
            if (0 < this.shtShipping.MaxRows)
            {
                this.shtShipping.TopLeft = new Position(TOPLEFT_COL, this.shtShipping.TopLeft.Row);
            }
            this.shtShipping.DataSource = null;
            this.shtShipping.AllowUserToAddRows = false;
            this.shtShipping.MaxRows = 0;
            this.shtShipping.Enabled = false;

            // 保持データのクリア
            _dtSheet = null;
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 出荷日のクリア
                this.dtpShukkaDateFrom.Value = DateTime.Today;

                // グリッドのクリア
                this.SheetClear();

                // フォーカス移動
                this.dtpShukkaDateFrom.Focus();

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region ファンクションバーのEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>T.Nakata 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            this.fbrFunction.F01Button.Enabled = isEnabled;
            this.fbrFunction.F06Button.Enabled = isEnabled;
        }

        #endregion
        
        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                // テンプレートダウンロード
                if (!this.ExcelTemplateFileDownload())
                {
                    // TemplateのDownloadに失敗しました。
                    this.ShowMessage("A7777777003");
                    return;
                }

                this.EditMode = SystemBase.EditMode.Insert;
                base.fbrFunction_F01Button_Click(sender, e);
                this.EditMode = SystemBase.EditMode.None;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

         /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>K.Tsutsumi 2019/03/23 シート制御修正</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;

                this.shtShipping.Redraw = false;

                // グリッドのクリア
                this.SheetClear();

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtShipping.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>K.Tsutsumi 2019/03/23 シート制御修正</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;

                this.shtShipping.Redraw = false;
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtShipping.Redraw = true;
            }
        }

        #endregion

        #region ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>K.Tsutsumi 2019/03/23 シート制御修正</update>
        /// <update>T.Nukaga 2020/09/16 EFA_SMS-126 対応</update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DateTime ShukkaDate = UtilConvert.ToDateTime(this.dtpShukkaDateFrom.Value).Date;
                string HakkouFlag = (this.cboHakkoFlag.SelectedValue == null ? HAKKO_FLAG.DEFAULT_VALUE1 : this.cboHakkoFlag.SelectedValue.ToString());

                this.shtShipping.Redraw = false;
                // グリッドクリア
                this.SheetClear();

                // 検索条件のロック
                this.grpSearch.Enabled = false;

                DataTable dtNisugata = this.GetNisugataList(ShukkaDate, HakkouFlag);
                if (dtNisugata == null || (dtNisugata != null && dtNisugata.Rows.Count <= 0))
                {
                    // 該当する荷姿情報はありません。
                    this.ShowMessage("S0200040001");
                    // 検索条件のアンロック
                    this.grpSearch.Enabled = true;
                    // フォーカス
                    this.dtpShukkaDateFrom.Focus();
                    return;
                }
                dtNisugata.Columns.Add(DB_FLD_PRINT, typeof(decimal));//印刷チェックボックス用FIELD
                dtNisugata.Columns.Add(DB_FLD_LINE, typeof(decimal));//行数用FIELD
                _dtSheet = dtNisugata.Copy();//退避
                this.shtShipping.DataSource = dtNisugata;
                this.shtShipping.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);

                // フォーカス
                this.shtShipping.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtShipping.Redraw = true;
                Cursor.Current = Cursors.Arrow;
            }
        }

        #endregion

        #region シート

        #region RowFilling

        /// --------------------------------------------------
        /// <summary>
        /// データソース設定時の行追加時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtShipping_RowFilling(object sender, RowFillingEventArgs e)
        {
            try
            {
                if (e.OperationMode == OperationMode.Add)
                {
                    Sheet tmpShet = sender as Sheet;
                    int Row = e.DestRow;//データソース側での行数でありシート側の行数とは異なるが追加時点では同一の為イコールとして扱う

                    //----- データ取得 -----
                    string EstimateFlag = ESTIMATE_FLAG.DEFAULT_VALUE1;//初期値:未指定

                    if (((object[])e.SourceRow)[(int)DB_FIELD.ESTIMATE_FLAG] != null
                        && !string.IsNullOrEmpty(((object[])e.SourceRow)[(int)DB_FIELD.ESTIMATE_FLAG].ToString()))
                    {   // 有償/無償
                        EstimateFlag = ((object[])e.SourceRow)[(int)DB_FIELD.ESTIMATE_FLAG].ToString();
                    }

                    //----- コンボボックス設定 -----
                    //列で同一なので列単位で設定したいがBindingErrorが出る為1行毎にコンボを設定する
                    this.SetComboDt((int)SHEET_COL.CONSIGNEE, Row, this.GetConsign(), Def_M_CONSIGN.NAME, Def_M_CONSIGN.CONSIGN_CD, true, true);
                    this.SetComboDt((int)SHEET_COL.DELIVERYTO, Row, this.GetDeliver(), Def_M_DELIVER.NAME, Def_M_DELIVER.DELIVER_CD, true, true);
                    this.SetComboDt((int)SHEET_COL.TRADE_TERMS_FLAG, Row, this.GetTradeTermsFlag(), Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false, false);

                    //----- デフォルト値設定 -----
                    //if (((object[])e.SourceRow)[(int)DB_FIELD.CONSIGNEE] == null && _dtConsign != null && _dtConsign.Rows.Count > 0)
                    //    ((object[])e.SourceRow)[(int)DB_FIELD.CONSIGNEE] = _dtConsign.Rows[0][Def_M_CONSIGN.CONSIGN_CD];
                    //if (((object[])e.SourceRow)[(int)DB_FIELD.DELIVERYTO] == null && _dtDeliver != null && _dtDeliver.Rows.Count > 0)
                    //    ((object[])e.SourceRow)[(int)DB_FIELD.DELIVERYTO] = _dtDeliver.Rows[0][Def_M_DELIVER.DELIVER_CD];
                    if (((object[])e.SourceRow)[(int)DB_FIELD.TRADE_TERMS_FLAG] == null && _dtTradeTermsFlag != null && _dtTradeTermsFlag.Rows.Count > 0)
                        ((object[])e.SourceRow)[(int)DB_FIELD.TRADE_TERMS_FLAG] = TRADE_TERMS_FLAG.DEFAULT_VALUE1;

                    //----- Enable操作 -----
                    tmpShet[(int)SHEET_COL.DELIVERYTO, Row].Enabled = true;
                    tmpShet[(int)SHEET_COL.DELIVERYTO_ATTN, Row].Enabled = true;
                    tmpShet[(int)SHEET_COL.TRADE_TERMS_FLAG, Row].Enabled = true;
                    tmpShet[(int)SHEET_COL.TRADE_TERMS_ATTR, Row].Enabled = true;
                    if (EstimateFlag != ESTIMATE_FLAG.DEFAULT_VALUE1)
                    {   // 項目非活性化
                        if (EstimateFlag == ESTIMATE_FLAG.ONEROUS_VALUE1)
                        {   // 有償の場合
                            tmpShet[(int)SHEET_COL.DELIVERYTO, Row].Enabled = false;        // DeliveryTo
                            tmpShet[(int)SHEET_COL.DELIVERYTO, Row].Value = null;
                            tmpShet[(int)SHEET_COL.DELIVERYTO_ATTN, Row].Enabled = false;   // DeliveryTo(ATTN)
                            tmpShet[(int)SHEET_COL.DELIVERYTO_ATTN, Row].Value = null;
                        }
                        else if (EstimateFlag == ESTIMATE_FLAG.GRATIS_VALUE1)
                        {   // 無償の場合
                            tmpShet[(int)SHEET_COL.INTERNAL_PO_NO, Row].Enabled = false;    // 社内見積番号
                            tmpShet[(int)SHEET_COL.INTERNAL_PO_NO, Row].Value = null;
                            tmpShet[(int)SHEET_COL.SUBJECT, Row].Enabled = false;           // タイトル
                            tmpShet[(int)SHEET_COL.SUBJECT, Row].Value = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// CellNotify Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtShipping_CellNotify(object sender, CellNotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtShipping_CellNotify(col, row)(" + e.Position.Column.ToString() + ", " + e.Position.Row.ToString() + ") : " + e.Name + ", " + e.CellEvent.ToString());
            if (e.Position.Column != (int)SHEET_COL.CONSIGNEE && e.Position.Column != (int)SHEET_COL.DELIVERYTO) return;
            try
            {
                int row = e.Position.Row;

                if (e.Name == CellNotifyEvents.SelectedIndexChanged ||
                    e.Name == CellNotifyEvents.TextChanged)
                {
                    if (e.Position.Column == (int)SHEET_COL.CONSIGNEE)
                    {
                        this._isConsigneeValueChanged = true;
                    }
                    else if (e.Position.Column == (int)SHEET_COL.DELIVERYTO)
                    {
                        this._isDeliveryToValueChanged = true;
                    }
                }
                

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region LeaveCell

        /// --------------------------------------------------
        /// <summary>
        /// LeaveCell Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtShipping_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtShipping_LeaveCell(newCol, newRow)(" + e.NewPosition.Column.ToString() + ", " + e.NewPosition.Row.ToString() + ") : " + e.MoveStatus.ToString());
            try
            {

                if (this.shtShipping.ActivePosition.Column == (int)SHEET_COL.CONSIGNEE ||
                    this.shtShipping.ActivePosition.Column == (int)SHEET_COL.DELIVERYTO)
                {

                    if (!this._isConsigneeValueChanged || !this._isDeliveryToValueChanged)
                    {

                        // コンボボックス内のアイテム文字列を入力した形にする。
                        if (!string.IsNullOrEmpty(this.shtShipping.ActiveCell.Text))
                        {
                            var editor = this.shtShipping.ActiveCell.Editor as SuperiorComboEditor;
                            var dt = editor.DataSource as DataTable;
                            string search = this.shtShipping.ActiveCell.Text;
                            var dr = dt.AsEnumerable().Where(d => UtilData.GetFld(d, editor.DisplayMember).StartsWith(search)).FirstOrDefault();
                            if (dr != null)
                            {
                                this.shtShipping.ActiveCell.Text = UtilData.GetFld(dr, editor.DisplayMember);
                                this.shtShipping.ActiveCell.Value = UtilData.GetFld(dr, editor.ValueMember);
                            }
                            else
                            {
                                this.shtShipping.ActiveCell.Value = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this._isConsigneeValueChanged = false;
                this._isDeliveryToValueChanged = false;
            }
        }

        #endregion

        #region Leave

        /// --------------------------------------------------
        /// <summary>
        /// Leave Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/23</create>
        /// <update></update>
        /// <remarks>LeaveCell Eventと同様の処理。きれいにしたかったけど時間がなかった。</remarks>
        /// --------------------------------------------------
        private void shtShipping_Leave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtShipping_Leave(activeCol, activeRow)(" + this.shtShipping.ActivePosition.Column.ToString() + ", " + this.shtShipping.ActivePosition.Row.ToString() + ")");
            try
            {

                if (this.shtShipping.ActivePosition.Column == (int)SHEET_COL.CONSIGNEE ||
                    this.shtShipping.ActivePosition.Column == (int)SHEET_COL.DELIVERYTO)
                {

                    if (!this._isConsigneeValueChanged || !this._isDeliveryToValueChanged)
                    {

                        // コンボボックス内のアイテム文字列を入力した形にする。
                        if (!string.IsNullOrEmpty(this.shtShipping.ActiveCell.Text))
                        {
                            var editor = this.shtShipping.ActiveCell.Editor as SuperiorComboEditor;
                            var dt = editor.DataSource as DataTable;
                            string search = this.shtShipping.ActiveCell.Text;
                            var dr = dt.AsEnumerable().Where(d => UtilData.GetFld(d, editor.DisplayMember).StartsWith(search)).FirstOrDefault();
                            if (dr != null)
                            {
                                this.shtShipping.ActiveCell.Text = UtilData.GetFld(dr, editor.DisplayMember);
                                this.shtShipping.ActiveCell.Value = UtilData.GetFld(dr, editor.ValueMember);
                            }
                            else
                            {
                                this.shtShipping.ActiveCell.Value = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this._isConsigneeValueChanged = false;
                this._isDeliveryToValueChanged = false;
            }
        }

        #endregion

        #region BindingError

        /// --------------------------------------------------
        /// <summary>
        /// データバインド時のエラー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtShipping_BindingError(object sender, BindingErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("shtShipping_BindingError(" + e.Position.Column.ToString() + ", " + e.Position.Row.ToString() + ")[" + e.Value.ToString() + "] : " + e.ErrorMessage);
        }

        #endregion

        #endregion

        #endregion

        #region シート操作

        #region コンボボックス作成

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックス設定
        /// </summary>
        /// <param name="Col">列</param>
        /// <param name="Row">行</param>
        /// <param name="dt">セットするデータテーブル</param>
        /// <param name="key1">DataRow取得キー(Text用)</param>
        /// <param name="key2">DataRow取得キー(Value用)</param>
        /// <param name="AutoSelect">AutoSelect有効/無効</param>
        /// <create>K.Tsutsumi 2019/03/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboDt(int Col, int Row, DataTable dt, string KeyText, string keyValue, bool AutoSelect, bool Editable)
        {
            try
            {
                // コンボボックスセット
                SuperiorComboEditor objCombo = new SuperiorComboEditor();
                if (dt != null)
                {
                    objCombo.DataSource = dt.Copy();
                    objCombo.DisplayMember = KeyText;
                    objCombo.ValueMember = keyValue;
                    //objCombo.AutoSelect = AutoSelect;
                    objCombo.AutoDropDown = AutoSelect;
                    objCombo.Editable = Editable;
                    objCombo.ValueAsIndex = false;

                    this.shtShipping[Col, Row].Value = null;
                    this.shtShipping[Col, Row].Editor = objCombo;
                }
                else
                {
                    objCombo.Items.Add("");
                    this.shtShipping[Col, Row].Editor = objCombo;
                    this.shtShipping[Col, Row].Value = null;
                    this.shtShipping[Col, Row].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return;
            }
        }

        #endregion

        #endregion

        #region データ取得

        //// --------------------------------------------------
        // <summary>
        /// 荷受マスタ取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetConsign()
        {
            try
            {
                if (_dtConsign != null) return _dtConsign.Copy();

                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                DataSet ds = conn.GetConsign(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_CONSIGN.Name))
                {
                    // 取得NG
                    return null;
                }
                // テーブル退避
                _dtConsign = ds.Tables[Def_M_CONSIGN.Name].Copy();
                return ds.Tables[Def_M_CONSIGN.Name];
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタ取得
        /// </summary>
        /// <param name="ShukkaFlag"></param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDeliver()
        {
            try
            {
                if (_dtDeliver != null) return _dtDeliver.Copy();

                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                DataSet ds = conn.GetDeliver(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_DELIVER.Name))
                {
                    // 取得NG
                    return null;
                }
                // テーブル退避
                _dtDeliver = ds.Tables[Def_M_DELIVER.Name].Copy();
                return ds.Tables[Def_M_DELIVER.Name];

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        //// --------------------------------------------------
        // <summary>
        /// 貿易条件フラグ取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetTradeTermsFlag()
        {
            try
            {
                if (_dtTradeTermsFlag != null) return _dtTradeTermsFlag.Copy();

                DataSet ds = this.GetCommon(TRADE_TERMS_FLAG.GROUPCD);
                _dtTradeTermsFlag = ds.Tables[Def_M_COMMON.Name].Copy();
                return ds.Tables[Def_M_COMMON.Name];
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿情報取得
        /// </summary>
        /// <param name="ShukkaDate">出荷日</param>
        /// <param name="HakkouFlag">状態</param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>T.Nukaga 2020/09/16 EFA_SMS-126 対応</update>
        /// --------------------------------------------------
        private DataTable GetNisugataList(DateTime ShukkaDate, string HakkouFlag)
        {
            try
            {
                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                cond.Kokunaigai = KOKUNAI_GAI_FLAG.GAI_VALUE1;
                cond.ShukkaDate = ShukkaDate;
                if (HakkouFlag != HAKKO_FLAG.ALL_VALUE1)
                {
                    cond.HakkouFlag = HakkouFlag;
                }
                DataSet ds = conn.GetNisugata(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_PACKING.Name))
                {
                    // 取得NG
                    return null;
                }
                // テーブル退避
                return ds.Tables[Def_T_PACKING.Name];

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>D.Okumura 2020/09/03 保存ダイアログをファイルパス取得するのみへ変更</update>
        /// <update>D.Okumura 2020/09/03 Consinee(ATTN)、DeliveryTo(ATTN)を任意入力変更</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode != SystemBase.EditMode.Insert) return ret;

                // 初期化
                _SaveFilePath = null;
                _dtUpdTable = null;

                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                int PrintCount = 0;
                for (int i = 0; i < this.shtShipping.Rows.Count; i++)
                {
                    int PrintFlag = (this.shtShipping[(int)SHEET_COL.PRINT, i].Value == null ? 0 : int.Parse(this.shtShipping[(int)SHEET_COL.PRINT, i].Value.ToString()));
                    if (PrintFlag == 1)
                    {
                        PrintCount++;

                        this.shtShipping[(int)SHEET_COL.LINE, i].Value = (i + 1);

                        //===== Consignee/Consignee(ATTN) 未入力確認 =====
                        string CONSIGNEE = (this.shtShipping[(int)SHEET_COL.CONSIGNEE, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.CONSIGNEE, i].Value.ToString());
                        string CONSIGNEE_ATTN = (this.shtShipping[(int)SHEET_COL.CONSIGNEE_ATTN, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.CONSIGNEE_ATTN, i].Value.ToString());
                        if (string.IsNullOrEmpty(CONSIGNEE))
                        {
                            // {0}行目のConsineeを入力してください。
                            ComFunc.AddMultiMessage(dtMessage, "S0200040003", (i + 1).ToString());
                        }
                        //if (string.IsNullOrEmpty(CONSIGNEE_ATTN))
                        //{
                        //    // {0}行目のConsinee(ATTN)を入力してください。
                        //    ComFunc.AddMultiMessage(dtMessage, "S0200040004", (i + 1).ToString());
                        //}

                        //===== 社内見積番号/タイトル/DeliveryTo/DeliveryTo(ATTN)の未入力確認 =====
                        string EstimateFlag = (this.shtShipping[(int)SHEET_COL.ESTIMATE_FLAG, i].Value == null ?
                                                ESTIMATE_FLAG.NEUTRAL_VALUE1 : this.shtShipping[(int)SHEET_COL.ESTIMATE_FLAG, i].Value.ToString());
                        string INTERNAL_PO_NO = (this.shtShipping[(int)SHEET_COL.INTERNAL_PO_NO, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.INTERNAL_PO_NO, i].Value.ToString());
                        string SUBJECT = (this.shtShipping[(int)SHEET_COL.SUBJECT, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.SUBJECT, i].Value.ToString());
                        string DELIVERYTO = (this.shtShipping[(int)SHEET_COL.DELIVERYTO, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.DELIVERYTO, i].Value.ToString());
                        string DELIVERYTO_ATTN = (this.shtShipping[(int)SHEET_COL.DELIVERYTO_ATTN, i].Value == null ? string.Empty : this.shtShipping[(int)SHEET_COL.DELIVERYTO_ATTN, i].Value.ToString());
                        if (EstimateFlag == ESTIMATE_FLAG.ONEROUS_VALUE1)
                        {
                            if (string.IsNullOrEmpty(INTERNAL_PO_NO))
                            {
                                // {0}行目の社内見積番号を入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040007", (i + 1).ToString());
                            }
                            if (string.IsNullOrEmpty(SUBJECT))
                            {
                                // {0}行目のタイトルを入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040008", (i + 1).ToString());
                            }
                        }
                        else if (EstimateFlag == ESTIMATE_FLAG.GRATIS_VALUE1)
                        {
                            if (string.IsNullOrEmpty(DELIVERYTO))
                            {
                                // {0}行目のDeliveryToを入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040005", (i + 1).ToString());
                            }
                            //if (string.IsNullOrEmpty(DELIVERYTO_ATTN))
                            //{
                            //    // {0}行目のDeliveryTo(ATTN)を入力してください。
                            //    ComFunc.AddMultiMessage(dtMessage, "S0200040006", (i + 1).ToString());
                            //}
                        }
                    }
                }

                if (PrintCount > 0)
                {
                    // シートのデータをデータソースに反映する。
                    this.shtShipping.UpdateData();
                    
                    // 対象のデータを抜き出しサーバー側のチェックを実施する
                    DataTable dtSheet = (this.shtShipping.DataSource as DataTable).Copy();
                    dtSheet.AcceptChanges();
                    DataTable dt = dtSheet.Clone();
                    foreach (DataRow dr in dtSheet.Rows)
                    {
                        string tmpFlag = ComFunc.GetFld(dr, DB_FLD_PRINT);
                        int PrintFlag = (tmpFlag == string.Empty ? 0 : int.Parse(tmpFlag));
                        if (PrintFlag == 1)
                        {
                            DataRow dr_dst = dt.NewRow();
                            dr_dst.ItemArray = dr.ItemArray;
                            dt.Rows.Add(dr_dst);
                        }
                    }
                    CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                    ConnS02 conn = new ConnS02();
                    DataTable dtMessageS = conn.CheckInvoice(cond, dt);

                    // エラーメッセージのマージ
                    DataTable dtMessageM = ComFunc.GetSchemeMultiMessage();
                    dtMessageM.Merge(dtMessage);
                    dtMessageM.Merge(dtMessageS);

                    if (dtMessageM.Rows.Count > 0)
                    {
                        // 入力値にエラーがあります。\r\n下記のエラーを修正し再度作成を実施して下さい。
                        this.ShowMultiMessage(dtMessageM, "S0200040002");
                        ret = false;
                    }
                    else
                    {
                        // 保存フォルダの選択
                        string FilePath = this.ShowSaveFolderDialog(this, this.Text, null);
                        if (FilePath != null)
                        {
                            ret = true;

                            //保存パスを退避
                            _SaveFilePath = FilePath;
                            
                            // 更新用テーブルにデータ退避
                            _dtUpdTable = dt.Copy();
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                }
                else
                {
                    // 作成対象が選択されていません
                    this.ShowMessage("S0200040009");
                    ret = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            this.ClearMessage();

            this.IsRunEditAfterClear = false;
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドのクリア
                    this.SheetClear();
                    // 検索条件のロック解除
                    this.grpSearch.Enabled = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);
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

        #region 登録/変更処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録/変更処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Nakata 2018/12/11</create>
        /// <update>M.Shimizu 2020/05/28 EFA_SMS-80 対応（無償INVOICE、無償Packing List の「CONSIGNED TO :」「DELIVERY TO: 」に「ATTN」を設定</update>
        /// <update>M.Shimizu 2020/06/09 EFA_SMS-80 対応（有償INVOICEに貿易条件が初回出力されない）</update>
        /// <update>T.Nukaga 2020/09/16 EFA_SMS-126 対応</update>
        /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                if (_dtUpdTable == null) return false;

                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                cond.ShukkaDate = UtilConvert.ToDateTime(this.dtpShukkaDateFrom.Value).Date;

                // ↓↓↓ M.Shimizu 2020/05/28 EFA_SMS-80 対応（無償INVOICE、無償Packing List の「CONSIGNED TO :」「DELIVERY TO: 」に「ATTN」を設定
                // 画面入力項目が帳票に反映されていなかったため、保存時の保存処理と帳票出力処理の順序を変更
                // DB更新
                string errMsgID;
                string[] args;
                
                if (!conn.UpdShippingData(cond, _dtUpdTable, out errMsgID, out args))
                {
                    // エラーメッセージ出力
                    this.ShowMessage(errMsgID, args);
                　   return false;
                }
                
                cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                conn = new ConnS02();
                cond.ShukkaDate = UtilConvert.ToDateTime(this.dtpShukkaDateFrom.Value).Date;
                // ↑↑↑ M.Shimizu 2020/05/28 EFA_SMS-80 対応（無償INVOICE、無償Packing List の「CONSIGNED TO :」「DELIVERY TO: 」に「ATTN」を設定

                //===== 帳票出力 =====
                foreach (DataRow dr in _dtUpdTable.Rows)
                {
                    //----- データ収集 -----
                    // テーブルA取得
                    cond.PackingNo = ComFunc.GetFld(dr, Def_T_PACKING.PACKING_NO);
                    cond.InvoiceNo = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);
                    cond.ConsignCd = ComFunc.GetFld(dr, Def_T_PACKING.CONSIGN_CD);
                    cond.DeliverCd = ComFunc.GetFld(dr, Def_T_PACKING.DELIVER_CD);
                    cond.UnsokaishaNo = ComFunc.GetFld(dr, Def_T_PACKING.UNSOKAISHA_CD);

                    // ↓↓↓ M.Shimizu 2020/06/09 EFA_SMS-80 対応（有償INVOICEに貿易条件が初回出力されない）
                    string ttFlag = string.Empty;
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_PACKING.TRADE_TERMS_FLAG)))
                    {
                        DataRow ttf = _dtTradeTermsFlag.AsEnumerable()
                            .Where(x => x.Field<string>("GROUP_CD") == TRADE_TERMS_FLAG.GROUPCD
                                && x.Field<string>("VALUE1") == ComFunc.GetFld(dr, Def_T_PACKING.TRADE_TERMS_FLAG))
                            .FirstOrDefault();
                        
                        ttFlag = ComFunc.GetFld(ttf, Def_M_COMMON.ITEM_NAME);
                    }
                    // ↑↑↑ M.Shimizu 2020/06/09 EFA_SMS-80 対応（有償INVOICEに貿易条件が初回出力されない）

                    // 帳票データ
                    DataSet dsDataGr1 = conn.GetShippingDocument(cond);
                    if (dsDataGr1 == null)
                    {
                        // データ取得エラー
                        continue;
                    }


                    // データマージ
                    DataTable dtTyouhyouDataA = dsDataGr1.Tables[ComDefine.DTTBL_SIPPING_A];
                    // 貿易条件
                    UtilData.SetFld(dtTyouhyouDataA, 0, ComDefine.FLD_TERMS, ttFlag + " " + ComFunc.GetFld(dr, Def_T_PACKING.TRADE_TERMS_ATTR));
                    // REF. O/# (有償の場合のみ)
                    UtilData.SetFld(dtTyouhyouDataA, 0, ComDefine.FLD_INTERNAL_PO_NO, ComFunc.GetFld(dr, Def_T_PACKING.INTERNAL_PO_NO));
                    // タイトル (有償の場合のみ)
                    UtilData.SetFld(dtTyouhyouDataA, 0, ComDefine.FLD_TITLE, ComFunc.GetFld(dr, Def_T_PACKING.SUBJECT));
                    // TRADE_TERMS_FLAG
                    UtilData.SetFld(dtTyouhyouDataA, 0, Def_T_PACKING.TRADE_TERMS_FLAG, ComFunc.GetFld(dr, Def_T_PACKING.TRADE_TERMS_FLAG));

                    // テーブル再構築
                    DataSet dsDataGr2 = new DataSet();
                    dsDataGr2.Tables.Add(dsDataGr1.Tables[ComDefine.DTTBL_SIPPING_D].Copy());

                    DataSet dsDataGr3 = new DataSet();
                    dsDataGr3.Tables.Add(dsDataGr1.Tables[ComDefine.DTTBL_SIPPING_E].Copy());

                    //----- 帳票出力 -----
                    //===================================================================================================
                    //_SaveFilePath:保存パス
                    //=======================
                    //EXCEL_FILE_TEMP_INVOICE:INVOICEのExcelテンプレートファイル名
                    //EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT:まとめ表パーツ便物量実績のExcelテンプレートファイル名
                    //EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT:まとめ表パーツ便出荷のExcelテンプレートファイル名
                    //=======================
                    //EXCEL_FILE_INVOICE:INVOICEのExcelファイル名
                    //EXCEL_FILE_QUANTITY_OF_PARTSSHIPMENT:まとめ表パーツ便物量実績のExcelファイル名
                    //EXCEL_FILE_SUMMARY_OF_PARTSSHIPMENT:まとめ表パーツ便出荷のExcelファイル名
                    //===================================================================================================
                    string tmpAttn = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.ATTN);
                    string tmpItem = ComFunc.GetFld(dsDataGr1, ComDefine.DTTBL_SIPPING_H, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    string tmpInvoice = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);
                    // SHIP,ARNOは複数の場合を考慮して出荷明細から設定
                    string tmpShip = string.Empty;
                    string tmpARNo = string.Empty;
                    // SHIP,AR_NOの重複は除外
                    foreach (DataRow tmpDr in dsDataGr1.Tables[ComDefine.DTTBL_SIPPING_H].DefaultView.ToTable(true, Def_M_NONYUSAKI.SHIP, Def_T_SHUKKA_MEISAI.AR_NO).Rows)
                    {
                        string tmpS = ComFunc.GetFld(tmpDr, Def_M_NONYUSAKI.SHIP).Trim();
                        string tmpA = ComFunc.GetFld(tmpDr, Def_T_SHUKKA_MEISAI.AR_NO).Trim();
                        tmpShip += tmpS + this.SetFileNameSeparator(tmpS);
                        tmpARNo += tmpA + this.SetFileNameSeparator(tmpA);
                    }
                    string tmpShukkaDate = ComFunc.GetFld(dsDataGr1, ComDefine.DTTBL_SIPPING_H, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE).Replace("/", "");
                    object[] fileNameParams = new object[] { 
                        //0:宛先
                        tmpAttn + this.SetFileNameSeparator(tmpAttn),
                        //1:Invoice
                        tmpInvoice + this.SetFileNameSeparator(tmpInvoice),
                        //2:Item + " "
                        tmpItem + this.SetFileNameSeparator(tmpItem),
                        //3:SHIP + " "
                        tmpShip + this.SetFileNameSeparator(tmpShip),
                        //4:AR_NO + " "
                        tmpARNo + this.SetFileNameSeparator(tmpARNo),
                        //5:出荷日
                        tmpShukkaDate
                    };
                    var pathInvoice = ComFunc.GetSafePathLength(this._SaveFilePath, ComFunc.GetSafeFileName(string.Format(ComDefine.EXCEL_FILE_INVOICE, fileNameParams), " "), "");
                    var pathSummaryOfParts = ComFunc.GetSafePathLength(this._SaveFilePath, ComFunc.GetSafeFileName(string.Format(ComDefine.EXCEL_FILE_SUMMARY_OF_PARTSSHIPMENT, fileNameParams), " "), "");
                    var pathQuantityOfParts = ComFunc.GetSafePathLength(this._SaveFilePath, ComFunc.GetSafeFileName(string.Format(ComDefine.EXCEL_FILE_QUANTITY_OF_PARTSSHIPMENT, fileNameParams), " "), "");
                    // INVOICE
                    ExportShippingDocument exportSipping = new ExportShippingDocument();
                    string Excel_msgID;
                    string[] Excel_args;
                    exportSipping.ExportExcel(pathInvoice, dsDataGr1, true, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }

                    // パーツ便出荷まとめ
                    ExportPartsMatomeList exportpartsMatomeList = new ExportPartsMatomeList();
                    exportpartsMatomeList.ExportExcel(pathSummaryOfParts, dsDataGr2, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }

                    // パーツ便出荷物量実績
                    ExportPartsJissekiList exportpartsJissekiList = new ExportPartsJissekiList();
                    exportpartsJissekiList.ExportExcel(pathQuantityOfParts, dsDataGr3, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイル名の区切り文字設定
        /// </summary>
        /// <param name="tgtStr">ファイル名に設定する文字列(SHIP or ARNO)</param>
        /// <returns>引数が空なら空文字、存在すれば区切り文字</returns>
        /// <create>T.Nukaga 2020/09/16 EFA_SMS-126対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private string SetFileNameSeparator(string tgtStr)
        {
            string rtn = " "; // 区切り文字

            if (string.IsNullOrEmpty(tgtStr))
            {
                rtn = string.Empty;
            }
            return rtn;
        }

        #endregion

        #region ファイルダウンロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExcelTemplateFileDownload()
        {
            bool ret = false;
            try
            {
                if (_DownloadFileList.Length <= 0) return false;

                var conn = new ConnAttachFile();
                foreach (string FileName in _DownloadFileList)
                {
                    var dlPackage = new FileDownloadPackage();
                    dlPackage.FileName = FileName;
                    dlPackage.FileType = FileType.Template;

                    // TemplateファイルのDL
                    var retFile = conn.FileDownload(dlPackage);
                    if (retFile.IsExistsFile)
                    {
                        if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                        {
                            Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                        }
                        using (var fs = new FileStream(Path.Combine(ComDefine.DOWNLOAD_DIR, FileName), FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(retFile.FileData, 0, retFile.FileData.Length);
                            fs.Close();
                        }
                        ret = true;
                    }
                    else
                    {
                        ret = false;
#if DEBUG
                        ret = File.Exists(Path.Combine(ComDefine.DOWNLOAD_DIR, FileName));
#endif
                        break;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


    }
}