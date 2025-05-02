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
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using SMS.T01.Properties;
using SMS.E01;
using SystemBase.Util;
using WsConnection.WebRefT01;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配情報照会
    /// </summary>
    /// <create>S.Furugo 2018/10/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiShokai : SystemBase.Forms.CustomOrderForm
    {
        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>S.Furugo 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>S.Furugo 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
        }
        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_TOPLEFT_COL = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 列定義
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_TEHAI_RENKEI_NO = 0;
        private const int SHEET_NOUHIN_SAKI = 7;
        private const int SHEET_SYUKKA_SAKI = 8;
        private const int SHEET_FLOOR = 16;
        private const int SHEET_KISHU = 17;
        private const int SHEET_ST_NO = 18;
        private const int SHEET_HINMEI_JP = 19;
        private const int SHEET_HINMEI = 20;
        private const int SHEET_ZUMEN_KEISHIKI = 22;
        private const int SHEET_ZUMEN_KEISHIKI2 = 34;

        private const int SHEET_ECS_QUOTA = 14;
        private const int SHEET_ECS_NO = 15;

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 表示中のモードレスダイアログの数
        /// </summary>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _modelessNum = 0;

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
        /// <create>S.Furugo 2018/10/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                this.SetComboBox();
                this.DisplayClear();
                this.ChangeMode(DisplayMode.Initialize);
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet">シートオブジェクト</param>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update>K.Tsutsumi 2019/03/09 編集、Ship照会追加</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            try
            {

                // K.Tsutsumi 2019/03/09
                // El TabelleのAutoFilterは行の高さを0とすることでフィルタリング状態を作り出しており
                // ユーザ操作により行の高さ変更が行われると、フィルタリング状態と表示データの不整合が
                // 発生する可能性があるため、ユーザ操作による行の高さ変更を不可とする
                this.shtMeisai.RowHeaders.AllowResize = false;
                
                // シートのタイトルを設定
                sheet.ColumnHeaders[0].Caption = Resources.TehaiShokai_CooperationNo;
                sheet.ColumnHeaders[1].Caption = Resources.TehaiShokai_RequestedDue;
                sheet.ColumnHeaders[2].Caption = Resources.TehaiShokai_DeliveryState;
                sheet.ColumnHeaders[3].Caption = Resources.TehaiShokai_StockSituation;
                sheet.ColumnHeaders[4].Caption = Resources.TehaiShokai_AssemblySituation;
                sheet.ColumnHeaders[5].Caption = Resources.TehaiShokai_TagRegistrationSituation;
                sheet.ColumnHeaders[6].Caption = Resources.TehaiShokai_ShippingSituation;
                sheet.ColumnHeaders[7].Caption = Resources.TehaiShokai_DeliveryDestination;
                sheet.ColumnHeaders[8].Caption = Resources.TehaiShokai_ShippingDestination;
                sheet.ColumnHeaders[9].Caption = Resources.TehaiShokai_BukkenName; //hidden
                sheet.ColumnHeaders[10].Caption = Resources.TehaiShokai_ProductNo;
                sheet.ColumnHeaders[11].Caption = Resources.TehaiShokai_Code;
                sheet.ColumnHeaders[12].Caption = Resources.TehaiShokai_AddNo;
                sheet.ColumnHeaders[13].Caption = Resources.TehaiShokai_ARNo;
                sheet.ColumnHeaders[14].Caption = Resources.TehaiShokai_ECSQuota; //hidden
                sheet.ColumnHeaders[15].Caption = Resources.TehaiShokai_ECSNo;
                sheet.ColumnHeaders[16].Caption = Resources.TehaiShokai_Floor;
                sheet.ColumnHeaders[17].Caption = Resources.TehaiShokai_Model;
                sheet.ColumnHeaders[18].Caption = Resources.TehaiShokai_STNo;
                sheet.ColumnHeaders[19].Caption = Resources.TehaiShokai_JPName;
                sheet.ColumnHeaders[20].Caption = Resources.TehaiShokai_Name;
                sheet.ColumnHeaders[21].Caption = Resources.TehaiShokai_INVName;
                sheet.ColumnHeaders[22].Caption = Resources.TehaiShokai_PartNo;
                sheet.ColumnHeaders[23].Caption = Resources.TehaiShokai_ArrangementQty;
                sheet.ColumnHeaders[24].Caption = Resources.TehaiShokai_ArrangementType;
                sheet.ColumnHeaders[25].Caption = Resources.TehaiShokai_OrderQty;
                sheet.ColumnHeaders[26].Caption = Resources.TehaiShokai_StockPerformance;
                sheet.ColumnHeaders[27].Caption = Resources.TehaiShokai_AssemblyPerformance;
                sheet.ColumnHeaders[28].Caption = Resources.TehaiShokai_ShippingQty;
                sheet.ColumnHeaders[29].Caption = Resources.TehaiShokai_TagRegistrationQty;
                sheet.ColumnHeaders[30].Caption = Resources.TehaiShokai_ShippingPerformance;
                sheet.ColumnHeaders[31].Caption = Resources.TehaiShokai_Free1;
                sheet.ColumnHeaders[32].Caption = Resources.TehaiShokai_Free2;
                sheet.ColumnHeaders[33].Caption = Resources.TehaiShokai_QtyUnit;
                sheet.ColumnHeaders[34].Caption = Resources.TehaiShokai_PartNo2;
                sheet.ColumnHeaders[35].Caption = Resources.TehaiShokai_Notes;
                sheet.ColumnHeaders[36].Caption = Resources.TehaiShokai_Maker;
                sheet.ColumnHeaders[37].Caption = Resources.TehaiShokai_JPYUnitPrice;
                sheet.ColumnHeaders[38].Caption = Resources.TehaiShokai_Compensation;
                //sheet.ColumnHeaders[39].Caption : 隠し項目、見積状態色

                this.SetEnableAutoFilter(false);
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
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                lblBukkenName.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region コンボボックスの設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの設定
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            try
            {
                ConnT01 connT01 = new ConnT01();

                DataSet ds = connT01.GetBukkenName();
                DataTable dt = ds.Tables[Def_M_PROJECT.Name];

                this.SuspendLayout();
                if (UtilData.ExistsData(ds, Def_M_PROJECT.Name))
                {
                    // 先頭行に「全て」を追加
                    DataRow dr = dt.NewRow();

                    dr[Def_M_PROJECT.PROJECT_NO] = Resources.cboAll;
                    dr[Def_M_PROJECT.BUKKEN_NAME] = Resources.cboAll;
                    dt.Rows.InsertAt(dr, 0);

                    this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                    this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
                    this.cboBukkenName.DataSource = dt;

                    this.cboBukkenName.SelectedValue = Resources.cboAll;      // デフォルト選択
                    RememberDefaultValue(this.cboBukkenName);
                }

                this.MakeCmbBox(this.cboTehaiFlag, DISP_TEHAI_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboYusho, DISP_ESTIMATE_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboNyukaFlag, TEHAI_NYUKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboAssyFlag, TEHAI_ASSY_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboTagTourokuFlag, TEHAI_TAG_TOUROKU_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboSyukkaFlag, TEHAI_SYUKKA_FLAG.GROUPCD);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        #endregion

        #endregion //初期化

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.SearchCondClear();
                this.SheetClear();

                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void SearchCondClear()
        {
            try
            {
                this.RestoreDefaultValue(this.cboBukkenName);
                this.RestoreDefaultValue(this.cboTehaiFlag);
                this.RestoreDefaultValue(this.cboYusho);
                this.RestoreDefaultValue(this.cboNyukaFlag);
                this.RestoreDefaultValue(this.cboAssyFlag);
                this.RestoreDefaultValue(this.cboTagTourokuFlag);
                this.RestoreDefaultValue(this.cboSyukkaFlag);

                this.txtSeiban.Clear();
                this.txtCode.Clear();
                this.txtEcsNo.Clear();
                this.txtARNo.Clear();
                this.txtOiban.Clear();
                this.txtNohinsaki.Clear();
                this.txtShukkasaki.Clear();
                this.txtTehaiRenkeiNo.Clear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.Rows.Count)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Redraw = true;
        }


        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (   string.IsNullOrEmpty(txtTehaiRenkeiNo.Text)
                    && string.IsNullOrEmpty(txtSeiban.Text)
                    && string.IsNullOrEmpty(txtCode.Text)
                    && string.IsNullOrEmpty(txtEcsNo.Text)
                    && string.IsNullOrEmpty(txtARNo.Text)
                    && string.IsNullOrEmpty(txtOiban.Text)
                    && string.IsNullOrEmpty(txtNohinsaki.Text)
                    && string.IsNullOrEmpty(txtShukkasaki.Text)
                    && IsDefaultValue(cboBukkenName)
                    && IsDefaultValue(cboTehaiFlag, DISP_TEHAI_FLAG.ALL_VALUE1, DISP_TEHAI_FLAG.ALL_EXCEPT_CANCEL_VALUE1)
                    && IsDefaultValue(cboYusho)
                    && IsDefaultValue(cboNyukaFlag)
                    && IsDefaultValue(cboAssyFlag)
                    && IsDefaultValue(cboTagTourokuFlag)
                    && IsDefaultValue(cboSyukkaFlag)
                    )
                {
                    // シートのクリア
                    this.SheetClear();
                    // 検索条件を少なくとも一つは入力して下さい。
                    this.ShowMessage("T0100030001");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>S.Furugo 2018/12/13</create>
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
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();
                cond.ProjectNo = Resources.cboAll.Equals(cboBukkenName.SelectedValue) ? null : cboBukkenName.SelectedValue.ToString();
                cond.TehaiRenkeiNo = txtTehaiRenkeiNo.Text;
                cond.Seiban = txtSeiban.Text;
                cond.Code = txtCode.Text;
                cond.EcsNo = txtEcsNo.Text;
                if (!string.IsNullOrEmpty(txtARNo.Text))
                    cond.ARNo = "AR" + txtARNo.Text;
                cond.Oiban = txtOiban.Text;
                cond.Nouhinsaki = txtNohinsaki.Text;
                cond.Shukkasaki = txtShukkasaki.Text;
                cond.TehaiKubun = cboTehaiFlag.SelectedValue.ToString();
                cond.Yusho = cboYusho.SelectedValue.ToString();
                cond.NyukaFlag = cboNyukaFlag.SelectedValue.ToString();
                cond.AssyFlag = cboAssyFlag.SelectedValue.ToString();
                cond.TagTourokuFlag = cboTagTourokuFlag.SelectedValue.ToString();
                cond.ShukkaFlag = cboSyukkaFlag.SelectedValue.ToString();

                DataSet ds = conn.GetTehaiShokai(cond);

                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する手配明細はありません。
                    this.ShowMessage("T0100030002");
                    return false;
                }

                this.shtMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.Rows.Count && this.shtMeisai.TopLeft.Row > -1)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }
                this.ChangeMode(DisplayMode.EndSearch);

                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 終了処理

        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (0 < this._modelessNum)
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
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
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {

            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);

            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var ecsQuota = UtilConvert.ToInt32(this.shtMeisai[SHEET_ECS_QUOTA, row].Text);
                var ecsNo = this.shtMeisai[SHEET_ECS_NO, row].Text;

                using (var frm = new TehaiMeisai(this.UserInfo, ComDefine.TITLE_T0100010, ecsQuota, ecsNo))
                {
                    if (DialogResult.OK == frm.ShowDialog())
                    {
                        // 再検索を行いますか？
                        if (DialogResult.OK == this.ShowMessage("T0100030003"))
                        {
                            // 再検索を行う
                            this.btnStart_Click(sender, e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {

            this.ClearMessage();
            base.fbrFunction_F02Button_Click(sender, e);

            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var tehaiRenkeiNo = this.shtMeisai[SHEET_TEHAI_RENKEI_NO, row].Text;

                var frm = new SearchShipmentNumber(this.UserInfo, ComDefine.TITLE_T0100080, tehaiRenkeiNo);
                frm.Icon = this.Icon;
                frm.FormClosed += new FormClosedEventHandler(SearchShipmentNumber_FormClosed);
                frm.Show();
                this._modelessNum++;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;
                // 検索結果クリア
                this.SheetClear();
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.SheetClear();
                this.DisplayClear();
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.TehaiShokai_sfdExcel_Title;
                    frm.Filter = Resources.TehaiShokai_sfdExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_TEHAI_MEISAI;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                    ExportTehaiMeisai export = new ExportTehaiMeisai();
                    string msgID;
                    string[] args;
                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion //ファンクションボタン

        #region 開始ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region シートイベント

        /// --------------------------------------------------
        /// <summary>
        /// データソースに DataRow が追加または削除され、シートの行が追加または削除される前に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_RowFilling(object sender, RowFillingEventArgs e)
        {
            var sheet = sender as Sheet;
            if (sheet == null)
                return;

            if (e.DestRow != -1 && e.OperationMode == OperationMode.Add)
            {
                var row = e.DestRow;
                var dt = sheet.DataSource as DataTable;
                var offset = dt.Columns[ComDefine.FLD_ESTIMATE_COLOR].Ordinal;
                var col = ((object[])e.SourceRow)[offset] as string;

                SetupRowColor(sheet.Rows[row], col, sheet.GridLine, Borders.All);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update>D.Okumura 2019/01/08 罫線が消えてしまう問題を修正</update>
        /// --------------------------------------------------
        private static void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
                return;
            var cols = input.Split(',');
            if (cols.Length < 2)
                return;

            var backcolor = GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                row.BackColor =  backcolor?? row.BackColor;
                row.DisabledBackColor =backcolor ?? row.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                row.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// RRGGBB形式の文字列からColorオブジェクトを生成する
        /// </summary>
        /// <param name="input">RGB文字列</param>
        /// <returns>色</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private static Color? GetColorFromRgb(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            int val;
            if (!int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out val))
                return null;
            return Color.FromArgb(val);
        }
        #endregion //シートイベント

        #endregion //イベント

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>S.Furugo 2018/12/13</create>
        /// <update>K.Tsutsumi 2019/03/09 編集、Ship照会ボタン追加</update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.grpSearch.Enabled = true;
                        this.shtMeisai.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;

                        this.EditMode = SystemBase.EditMode.None;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F02Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true; //検索後も条件を変更して再検索できるようにする
                        this.shtMeisai.Enabled = true;
                        this.fbrFunction.F06Button.Enabled = true;

                        this.EditMode = SystemBase.EditMode.View;
                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F02Button.Enabled = true;
                        this.fbrFunction.F10Button.Enabled = true;
                        break;
                    default:
                        break;
                }
                this.fbrFunction.F07Button.Enabled = true;
                this.fbrFunction.F12Button.Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックス ヘルパ
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスを作成するヘルパ
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="groupCD">グループCD</param>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void MakeCmbBox(DSWComboBox cmbBox, string groupCD)
        {
            base.MakeCmbBox(cmbBox, groupCD);
            RememberDefaultValue(cmbBox);
        }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスに設定されている値をデフォルト値として保持
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void RememberDefaultValue(DSWComboBox cmbBox)
        {
            // デフォルト値をTAGに保持する
            cmbBox.Tag = cmbBox.SelectedValue;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスに設定されている値がデフォルト値であるかどうかを検査
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <returns>true:デフォルト値/false:その他</returns>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool IsDefaultValue(DSWComboBox cmbBox)
        {
            if (cmbBox.Tag == null)
                return false;
            return cmbBox.Tag == cmbBox.SelectedValue;
        }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスに設定されている値がデフォルト値またはparamで指定された値であるかどうかを検査
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <param name="param">チェック対象のデータ</param>
        /// <returns>true:デフォルト値/false:その他</returns>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool IsDefaultValue<T>(DSWComboBox cmbBox, params T[] param)
        {
            if (IsDefaultValue(cmbBox))
                return true;
            foreach (var item in param)
                if (item.Equals(cmbBox.SelectedValue))
                    return true;
            return false;
        }
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのデフォルト値を復元する
        /// </summary>
        /// <param name="cmbBox">コンボボックス</param>
        /// <create>D.Okumura 2018/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void RestoreDefaultValue(DSWComboBox cmbBox)
        {
            if (cmbBox.Tag == null)
                return;
            cmbBox.SelectedValue = cmbBox.Tag;
        }
        #endregion

        #region AutoFilter設定

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear)
        {
            this.shtMeisai.Redraw = false;

            this.SetEnableAutoFilter(isForceClear, SHEET_NOUHIN_SAKI);
            this.SetEnableAutoFilter(isForceClear, SHEET_SYUKKA_SAKI);
            this.SetEnableAutoFilter(isForceClear, SHEET_FLOOR);
            this.SetEnableAutoFilter(isForceClear, SHEET_KISHU);
            this.SetEnableAutoFilter(isForceClear, SHEET_ST_NO);
            this.SetEnableAutoFilter(isForceClear, SHEET_HINMEI_JP);
            this.SetEnableAutoFilter(isForceClear, SHEET_HINMEI);
            this.SetEnableAutoFilter(isForceClear, SHEET_ZUMEN_KEISHIKI);
            this.SetEnableAutoFilter(isForceClear, SHEET_ZUMEN_KEISHIKI2);


            this.shtMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定 - 列指定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <param name="col">列番号</param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear, int col)
        {
            if (isForceClear)
            {
                this.shtMeisai.ColumnHeaders[col].DropDown = null;
            }
            else
            {
                if (this.shtMeisai.ColumnHeaders[col].DropDown == null)
                {
                    var headerDropDown = new HeaderDropDown();
                    headerDropDown.EnableAutoFilter = true;
                    headerDropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                    this.shtMeisai.ColumnHeaders[col].DropDown = headerDropDown;
                }
                else
                {
                    this.shtMeisai.ColumnHeaders[col].DropDown.EnableAutoFilter = true;
                    this.shtMeisai.ColumnHeaders[col].DropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                }
            }
        }

        #endregion

        #region 子画面が閉じた時の処理

        /// --------------------------------------------------
        /// <summary>
        /// 子画面が閉じた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SearchShipmentNumber_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this._modelessNum--;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

    }
}
