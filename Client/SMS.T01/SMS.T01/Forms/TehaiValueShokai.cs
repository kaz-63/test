using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using Commons;
using DSWUtil;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using SMS.T01.Properties;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefT01;
using ElTabelleHelper;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// VALUE照会
    /// </summary>
    /// <create>J.Chen 2024/02/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiValueShokai : SystemBase.Forms.CustomOrderForm
    {
        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>J.Chen 2024/02/15</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>J.Chen 2024/02/15</create>
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 見積回数のカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CNT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// PO No.のカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PO_NO_COL = 1;
        /// --------------------------------------------------
        /// <summary>
        /// PO金額のカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PO_AMOUNT_COL = 2;
        /// --------------------------------------------------
        /// <summary>
        /// レートのカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_RATE_JPY_COL = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 仕切り金額のカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PARTITION_AMOUNT_COL = 4;
        /// --------------------------------------------------
        /// <summary>
        /// INVOICE_NOのカラム開始インデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INVOICE_NO_START_COL = 5;
        /// --------------------------------------------------
        /// <summary>
        /// 使用済みValue合計のカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int INVOICE_NO_ONE_COL_AFTER = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 残Valueのカラムインデックス
        /// </summary>
        /// <create>J.Chen 2024/02/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int INVOICE_NO_TWO_COLS_AFTER = 2;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;

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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiValueShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // フォームの状態を初期化
                this.DisplayClearAll();

                // シートの初期化
                this.InitializeSheet(this.shtResult);
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboBukkenName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックスの設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの設定
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            // プロジェクトマスタより物件一覧取得
            ConnT01 connT01 = new ConnT01();

            DataSet ds = connT01.GetBukkenName();
            DataTable dt = ds.Tables[Def_M_PROJECT.Name];

            this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
            this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
            this.cboBukkenName.DataSource = dt;
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update>J.jeong 2024/07/19 下段ラベル追加</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // グリッド
                this.SheetClear();

                this.ChangeMode(DisplayMode.Initialize);

                this.txtTotalPOAmount.Clear();
                this.txtTotalPartitionAmount.Clear();
                this.txtlblTotalUsedValue.Clear();
                this.txtRemainingValueTotal.Clear();

                this.lbCurrency1.Text = CURRENCY_FLAG.JPY_NAME;
                this.lbCurrency3.Text = CURRENCY_FLAG.JPY_NAME;
                this.lbCurrency4.Text = CURRENCY_FLAG.JPY_NAME;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全画面クリア処理
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearAll()
        {
            try
            {
                this.DisplayClear();
                // コンボボックスの状態を更新
                SetComboBox();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>J.Chen 2024/02/15</create>
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update>J.Jeong 2024/07/19 下段ラベル追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                if (cboBukkenName.SelectedValue == null)
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("T0100010001", this.lblBukkenName.Text);
                    return false;
                }

                cond.ProjectNo = cboBukkenName.SelectedValue.ToString();
                DataSet ds = conn.GetTehaiMitsumoriValue(cond);

                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_ESTIMATE.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する見積情報はありません。
                    this.ShowMessage("T0100090001");
                    return false;
                }

                string[] invoiceNumbers = ds.Tables[Def_T_TEHAI_ESTIMATE.Name].AsEnumerable()
                                     .Select(row => row.Field<string>(Def_T_SHUKKA_MEISAI.INVOICE_NO))
                                     .Where(invoice => !string.IsNullOrEmpty(invoice)) 
                                     .SelectMany(invoice => invoice.Split(','))
                                     .Select(invoice => invoice.Trim())
                                     .OrderBy(invoice => invoice) // INVOICE_NOを昇順で並び替える
                                     .Distinct() // 重複を除去する
                                     .ToArray();

                // シート初期化
                InitializeResultSheet(invoiceNumbers,true);

                // 各合計計算
                DataTable dt = ds.Tables[Def_T_TEHAI_ESTIMATE.Name];
                DataRow sumRow = dt.NewRow();
                sumRow[Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT] = "合計";

                // 行ごとの合計を計算して代入
                foreach (DataRow row in dt.Rows)
                {
                    decimal rowSum = 0;
                    decimal PoAmount = ComFunc.GetFldToDecimal(row, Def_T_TEHAI_ESTIMATE.PO_AMOUNT);
                    foreach (string columnName in invoiceNumbers)
                    {
                        rowSum += Convert.ToDecimal(row[columnName]);
                    }
                    row[ComDefine.FLD_USED_VALUE] = rowSum;
                    row[ComDefine.FLD_REMAINING_VALUE] = PoAmount - rowSum;
                }

                // INVOICE NO各列の合計を計算します
                foreach (string columnName in invoiceNumbers)
                {
                    decimal sumValue = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        sumValue += Convert.ToDecimal(row[columnName]);
                    }
                    sumRow[columnName] = sumValue;
                }
                ds.Tables[Def_T_TEHAI_ESTIMATE.Name].Rows.Add(sumRow);

                this.shtResult.DataSource = ds.Tables[Def_T_TEHAI_ESTIMATE.Name];

                this.shtResult.Enabled = true;

                // 最も左上に表示されているセルの設定
                if (0 < this.shtResult.MaxRows)
                {
                    this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
                }

                this.shtResult.Focus();

                // 集計データ
                decimal totalPoAmount = 0;
                decimal totalPartitionAmount = 0;
                decimal totalUsedValue = 0;
                decimal remainingValueTotal = 0;
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    DataRow row = dt.Rows[i];

                    // PO金額合計
                    if (row[Def_T_TEHAI_ESTIMATE.PO_AMOUNT] != DBNull.Value)
                    {
                        totalPoAmount += Convert.ToDecimal(row[Def_T_TEHAI_ESTIMATE.PO_AMOUNT]);
                    }

                    // 仕切り金額合計
                    string partitionAmountStr = row[Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT].ToString().Replace(",", "");
                    decimal partitionAmount;
                    if (decimal.TryParse(partitionAmountStr, out partitionAmount))
                    {
                        totalPartitionAmount += partitionAmount;
                    }

                    // 使用済みValue合計
                    if (row[ComDefine.FLD_USED_VALUE] != DBNull.Value)
                    {
                        totalUsedValue += Convert.ToDecimal(row[ComDefine.FLD_USED_VALUE]);
                    }

                    // 残Value合計
                    if (row[ComDefine.FLD_REMAINING_VALUE] != DBNull.Value)
                    {
                        remainingValueTotal += Convert.ToDecimal(row[ComDefine.FLD_REMAINING_VALUE]);
                    }

                    string currency = ComFunc.GetFld(row, Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG);
                    this.lbCurrency1.Text = currency;
                    this.lbCurrency3.Text = currency;
                    this.lbCurrency4.Text = currency;

                }

                this.txtTotalPOAmount.Value = totalPoAmount;
                this.txtTotalPartitionAmount.Value = totalPartitionAmount;
                this.txtlblTotalUsedValue.Value = totalUsedValue;
                this.txtRemainingValueTotal.Value = remainingValueTotal;

                this.ChangeMode(DisplayMode.EndSearch);

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                DisplayClear();
                this.cboBukkenName.Focus();
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClearAll();
                this.cboBukkenName.Focus();
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
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
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

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtResult.Redraw = false;

            // シート初期化
            InitializeResultSheet(null, false);

            // 最も左上に表示されているセルの設定
            if (0 < this.shtResult.MaxRows)
            {
                this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
            }
            this.shtResult.DataSource = null;
            this.shtResult.MaxRows = 0;
            this.shtResult.Enabled = false;
            this.shtResult.Redraw = true;
        }

        #endregion

        #region シート初期化

        /// --------------------------------------------------
        /// <summary>
        /// シート初期化
        /// </summary>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeResultSheet(string[] invoiceNumbers, bool isSearch)
        {
            try
            {
                this.shtResult.Redraw = false;

                // シートの初期化
                this.shtResult.MaxColumns = 0;

                decimal maxValue = 999999999999.99m;
                decimal minValue = -999999999999.99m;

                // テキストカラム
                var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                // 金額カラム
                var amountEditor = ElTabelleSheetHelper.NewNumberEditor();
                amountEditor.Format = new NumberFormat("###########0.##", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                amountEditor.DisplayFormat = new NumberFormat("###,###,###,##0.##", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                amountEditor.MaxValue = maxValue;
                amountEditor.MinValue = minValue;
                // レートカラム
                var rateEditor = ElTabelleSheetHelper.NewNumberEditor();
                rateEditor.Format = new NumberFormat("########0.##", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                rateEditor.DisplayFormat = new NumberFormat("########0.##", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                rateEditor.MaxValue = maxValue;
                rateEditor.MinValue = minValue;

                // 見積回数
                this.SetElTabelleColumn(this.shtResult, SHEET_COL_CNT_COL, Resources.TehaiValueShokai_EstimationCount, false, false, ComDefine.FLD_ESTIMATION_COUNT, txtEditor, 80);
                this.shtResult.Columns[SHEET_COL_CNT_COL].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_CNT_COL, 0, SHEET_COL_CNT_COL, 1));
                this.shtResult.ColumnHeaders[SHEET_COL_CNT_COL, 0].Caption = Resources.TehaiValueShokai_EstimationCount;
                // PO No.
                this.SetElTabelleColumn(this.shtResult, SHEET_COL_PO_NO_COL, Resources.TehaiValueShokai_PoNo, false, false, Def_T_TEHAI_ESTIMATE.PO_NO, txtEditor, 70);
                this.shtResult.Columns[SHEET_COL_PO_NO_COL].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_PO_NO_COL, 0, SHEET_COL_PO_NO_COL, 1));
                this.shtResult.ColumnHeaders[SHEET_COL_PO_NO_COL, 0].Caption = Resources.TehaiValueShokai_PoNo;
                // PO金額
                this.SetElTabelleColumn(this.shtResult, SHEET_COL_PO_AMOUNT_COL, Resources.TehaiValueShokai_POAmount, false, false, Def_T_TEHAI_ESTIMATE.PO_AMOUNT, amountEditor, 80);
                this.shtResult.Columns[SHEET_COL_PO_AMOUNT_COL].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_PO_AMOUNT_COL, 0, SHEET_COL_PO_AMOUNT_COL, 1));
                this.shtResult.ColumnHeaders[SHEET_COL_PO_AMOUNT_COL, 0].Caption = Resources.TehaiValueShokai_POAmount;
                // レート
                this.SetElTabelleColumn(this.shtResult, SHEET_COL_RATE_JPY_COL, Resources.TehaiValueShokai_Rate_JPY, false, false, Def_T_TEHAI_ESTIMATE.RATE_JPY, rateEditor, 40);
                this.shtResult.Columns[SHEET_COL_RATE_JPY_COL].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_RATE_JPY_COL, 0, SHEET_COL_RATE_JPY_COL, 1));
                this.shtResult.ColumnHeaders[SHEET_COL_RATE_JPY_COL, 0].Caption = Resources.TehaiValueShokai_Rate_JPY;
                // 仕切り金額
                this.SetElTabelleColumn(this.shtResult, SHEET_COL_PARTITION_AMOUNT_COL, Resources.TehaiValueShokai_PartitionAmount, false, false, Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT, txtEditor, 80);
                this.shtResult.Columns[SHEET_COL_PARTITION_AMOUNT_COL].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_PARTITION_AMOUNT_COL, 0, SHEET_COL_PARTITION_AMOUNT_COL, 1));
                this.shtResult.ColumnHeaders[SHEET_COL_PARTITION_AMOUNT_COL, 0].Caption = Resources.TehaiValueShokai_PartitionAmount;
                this.shtResult.Columns[SHEET_COL_PARTITION_AMOUNT_COL].AlignHorizontal = AlignHorizontal.Right;

                // INVOICE NO
                var invoiceCol = SHEET_COL_INVOICE_NO_START_COL;
                if (isSearch)
                {
                    if (invoiceNumbers.Length > 0)
                    {
                        for (int i = 0; i < invoiceNumbers.Length; i++)
                        {
                            this.SetElTabelleColumn(this.shtResult, invoiceCol, Resources.TehaiValueShokai_InvoiceNo, false, false, invoiceNumbers[i], amountEditor, 70);
                            this.shtResult.Columns[invoiceCol].Enabled = false;
                            shtResult.ColumnHeaders[invoiceCol, 1].Caption = invoiceNumbers[i];

                            if (i < invoiceNumbers.Length - 1)
                            {
                                invoiceCol++;
                            }
                        }
                        this.shtResult.ColumnHeaders.Merge(new Range(SHEET_COL_INVOICE_NO_START_COL, 0, invoiceCol, 0));
                    }
                    else 
                    {
                        this.SetElTabelleColumn(this.shtResult, SHEET_COL_INVOICE_NO_START_COL, "", false, false, "", txtEditor, 70);
                        this.shtResult.Columns[SHEET_COL_INVOICE_NO_START_COL].Enabled = false;
                        
                    }
                    this.shtResult.ColumnHeaders[SHEET_COL_INVOICE_NO_START_COL, 0].Caption = Resources.TehaiValueShokai_InvoiceNo;
                }
                else
                {
                    this.SetElTabelleColumn(this.shtResult, invoiceCol, Resources.TehaiValueShokai_InvoiceNo, false, false, "", amountEditor, 70);
                    this.shtResult.Columns[invoiceCol].Enabled = false;
                    this.shtResult.ColumnHeaders[invoiceCol, 0].Caption = Resources.TehaiValueShokai_InvoiceNo;
                    shtResult.ColumnHeaders[invoiceCol, 1].Caption = "";
                }
                
                // 使用済みValue合計
                this.SetElTabelleColumn(this.shtResult, invoiceCol + INVOICE_NO_ONE_COL_AFTER, Resources.TehaiValueShokai_UsedValue, false, false, ComDefine.FLD_USED_VALUE, amountEditor, 110);
                this.shtResult.Columns[invoiceCol + INVOICE_NO_ONE_COL_AFTER].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(invoiceCol + INVOICE_NO_ONE_COL_AFTER, 0, invoiceCol + INVOICE_NO_ONE_COL_AFTER, 1));
                this.shtResult.ColumnHeaders[invoiceCol + INVOICE_NO_ONE_COL_AFTER, 0].Caption = Resources.TehaiValueShokai_UsedValue;
                // 残Value
                this.SetElTabelleColumn(this.shtResult, invoiceCol + INVOICE_NO_TWO_COLS_AFTER, Resources.TehaiValueShokai_RemainingValue, false, false, ComDefine.FLD_REMAINING_VALUE, amountEditor, 100);
                this.shtResult.Columns[invoiceCol + INVOICE_NO_TWO_COLS_AFTER].Enabled = false;
                this.shtResult.ColumnHeaders.Merge(new Range(invoiceCol + INVOICE_NO_TWO_COLS_AFTER, 0, invoiceCol + INVOICE_NO_TWO_COLS_AFTER, 1));
                this.shtResult.ColumnHeaders[invoiceCol + INVOICE_NO_TWO_COLS_AFTER, 0].Caption = Resources.TehaiValueShokai_RemainingValue;

                // グリッド線
                shtResult.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < shtResult.Columns.Count; i++)
                {
                    shtResult.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    shtResult.Columns[i].DisabledForeColor = Color.Black;
                }

                // 列固定
                this.shtResult.FreezeColumns = 5;

                this.shtResult.Redraw = true;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>J.Chen 2024/02/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.lblBukkenName.Enabled = true;
                        this.btnSearch.Enabled = true;
                        this.fbrFunction.F06Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.lblBukkenName.Enabled = false;
                        this.btnSearch.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

    }
}
