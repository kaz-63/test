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
using ActiveReportsHelper;
using WsConnection.WebRefK02;
using SMS.K02.Properties;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト発行
    /// </summary>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PalletListHakko : SystemBase.Forms.CustomOrderForm
    {
        #region 定数
        // 印刷チェックボックスカラム
        private const int SHEET_COL_INSATU = 0;
        // パレットNoカラム
        private const int SHEET_COL_PALLETNO = 1;
        // BOX数カウント
        private const string COL_BOX_COUNT = "COUNT_BOX";
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public PalletListHakko(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                this.MakeCmbBox(this.cboHaccoSelect, PALLET_LIST_PRINT.GROUPCD);

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.InitializeSheet(this.shtPalletList);
                this.shtPalletList.ShortCuts.Remove(Keys.Enter);
                this.shtPalletList.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
                this.shtPalletList.KeepHighlighted = true;

                // シートのタイトルを設定
                shtPalletList.ColumnHeaders[0].Caption = Resources.PalletListHakko_Print;
                shtPalletList.ColumnHeaders[1].Caption = Resources.PalletListHakko_PalletNo;
                shtPalletList.ColumnHeaders[2].Caption = Resources.PalletListHakko_DeliveryDestination;
                shtPalletList.ColumnHeaders[3].Caption = Resources.PalletListHakko_Ship;
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 最も左上に表示されているセルの設定
                if (0 < this.shtPalletList.MaxRows)
                {
                    this.shtPalletList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPalletList.TopLeft.Row);
                }

                this.shtPalletList.DataSource = null;
                this.shtPalletList.MaxRows = 0;
                this.shtPalletList.Enabled = false;
                this.rdoSinki.Checked = false;
                this.rdoSaiHakko.Checked = false;
                this.btnAllCheck.Enabled = false;
                this.btnAllNotCheck.Enabled = false;
                this.btnRangeCheck.Enabled = false;
                this.btnRangeNotCheck.Enabled = false;
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.cboHaccoSelect.SelectedValue = PALLET_LIST_PRINT.DEFAULT_VALUE1;
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
        /// <create>H.Tsunamura 2010/07/15</create>
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
        /// <create>H.Tsunamura 2010/07/15</create>
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
        /// <create>H.Tsunamura 2010/07/15</create>
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
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
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


        #region イベント

        #region ファンクションボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F01Button_Click(sender, e);

                printData(false, getHeader());
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F02Button_Click(sender, e);

                printData(true, getHeader());

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);

            try
            {
                this.ClearMessage();
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        # region ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// --------------------------------------------------
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            this.shtPalletList.Redraw = false;

            shtPalletList.CellRange = new Range("A:A");
            shtPalletList.CellValue = 1;

            this.shtPalletList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// --------------------------------------------------
        private void btnAllNotCheck_Click(object sender, EventArgs e)
        {
            this.shtPalletList.Redraw = false;

            shtPalletList.CellRange = new Range("A:A");
            shtPalletList.CellValue = 0;

            this.shtPalletList.Redraw = true;

        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeCheck_Click(object sender, EventArgs e)
        {
            GrapeCity.Win.ElTabelle.Range[] objRanges;
            //選択されているセル範囲をすべて取得する
            objRanges = shtPalletList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtPalletList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtPalletList.CellRange = tmprange;
                shtPalletList.CellValue = 1;

            }
            this.shtPalletList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/11/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeNotCheck_Click(object sender, EventArgs e)
        {
            GrapeCity.Win.ElTabelle.Range[] objRanges;
            //選択されているセル範囲をすべて取得する
            objRanges = shtPalletList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtPalletList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtPalletList.CellRange = tmprange;
                shtPalletList.CellValue = 0;

            }
            this.shtPalletList.Redraw = true;
        }

        # endregion

        #region ラジオボタン切り替え
        /// --------------------------------------------------
        /// <summary>
        /// 新規発行状態切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoSinki_CheckedChanged(object sender, EventArgs e)
        {
            this.ClearMessage();

            if (!this.rdoSinki.Checked)
            {
                return;
            }

            // 最も左上に表示されているセルの設定
            if (0 < this.shtPalletList.MaxRows)
            {
                this.shtPalletList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPalletList.TopLeft.Row);
            }

            // 新規行削除
            this.shtPalletList.AllowUserToAddRows = false;

            // パレットNo編集不可
            this.shtPalletList.Columns[1].Enabled = false;

            WsConnection.ConnK02 connK02 = new ConnK02();
            DataTable dt = connK02.GetPalletList();


            if (dt.Rows.Count != 0)
            {
                this.shtPalletList.Enabled = true;
                this.shtPalletList.DataSource = dt;
                this.btnAllCheck.Enabled = true;
                this.btnAllNotCheck.Enabled = true;
                this.btnRangeCheck.Enabled = true;
                this.btnRangeNotCheck.Enabled = true;
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
            else
            {
                this.shtPalletList.Enabled = false;
                this.shtPalletList.DataSource = null;
                this.shtPalletList.MaxRows = 0;

                // 該当の明細は存在しません。
                this.ShowMessage("A9999999022");
                this.btnAllCheck.Enabled = false;
                this.btnAllNotCheck.Enabled = false;
                this.btnRangeCheck.Enabled = false;
                this.btnRangeNotCheck.Enabled = false;
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// 再発行状態切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoSaiHakko_CheckedChanged(object sender, EventArgs e)
        {
            this.ClearMessage();

            if (!this.rdoSaiHakko.Checked)
            {
                return;
            }

            // 最も左上に表示されているセルの設定
            if (0 < this.shtPalletList.MaxRows)
            {
                this.shtPalletList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPalletList.TopLeft.Row);
            }

            // グリッドのクリア
            this.shtPalletList.Enabled = true;
            this.shtPalletList.DataSource = null;
            this.shtPalletList.MaxRows = 0;

            // 新規行追加
            this.shtPalletList.InsertRow(0, false);

            // パレットNo編集可能
            this.shtPalletList.Columns[SHEET_COL_PALLETNO].Enabled = true;
            this.shtPalletList.ActivePosition = new Position(1, 0);
            this.shtPalletList.Focus();

            this.btnAllCheck.Enabled = false;
            this.btnAllNotCheck.Enabled = false;

            this.fbrFunction.F01Button.Enabled = false;
            this.fbrFunction.F02Button.Enabled = false;
        }

        #endregion

        #region グリッド

        /// --------------------------------------------------
        /// <summary>
        /// パレットNo入力確定時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtPalletList_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                // パレットNo入力確定時検索処理

                if (this.shtPalletList.ActivePosition.Column != 1 || string.IsNullOrEmpty(this.shtPalletList.ActiveCell.Text))
                {
                    return;
                }
                this.ClearMessage();

                // 頭文字'P'の追加と字埋め
                if (this.shtPalletList.ActiveCell.Text.Substring(0, 1) != ComDefine.PREFIX_PALLETNO)
                {
                    this.shtPalletList.ActiveCell.Text = ComDefine.PREFIX_PALLETNO + this.shtPalletList.ActiveCell.Text.PadLeft(5, '0');
                }

                for (int i = 0; i < this.shtPalletList.Rows.Count; i++)
                {
                    if (i == this.shtPalletList.ActivePosition.Row) continue;
                    if (this.shtPalletList[SHEET_COL_PALLETNO, i].Text == this.shtPalletList.ActiveCell.Text)
                    {
                        // {0}行目のPalletNo.[{1}]は既に存在します。
                        this.ShowMessage("K0200030005", (this.shtPalletList.ActivePosition.Row + 1).ToString(), this.shtPalletList.ActiveCell.Text);
                        e.Cancel = true;
                        return;
                    }
                }

                WsConnection.ConnK02 connK02 = new ConnK02();
                CondK02 cond = new CondK02(this.UserInfo);
                cond.PalletNo = this.shtPalletList.ActiveCell.Text;

                DataTable dt = connK02.GetPalletNo(cond);

                if (dt.Rows.Count == 0)
                {
                    // 該当パレットNo.は存在しません。
                    this.ShowMessage("K0200030002");
                    e.Cancel = true;
                    return;
                }

                if (ComFunc.GetFld(dt, 0, LISTHAKKO_FLAG.GROUPCD) == LISTHAKKO_FLAG.MIHAKKO_VALUE1)
                {
                    // 該当パレットNo.は未発行です。
                    this.ShowMessage("K0200030003");
                    e.Cancel = true;
                    return;
                }

                this.shtPalletList.Redraw = false;
                this.shtPalletList.ActiveCell.Enabled = false;

                // グリッドデータの更新
                if (this.shtPalletList.Rows.Count == 1)
                {
                    this.shtPalletList.DataSource = dt;
                }
                else
                {
                    this.shtPalletList[0, this.shtPalletList.ActivePosition.Row].Value = "1";
                    this.shtPalletList[1, this.shtPalletList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_T_PALLETLIST_MANAGE.PALLET_NO);
                    this.shtPalletList[2, this.shtPalletList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    this.shtPalletList[3, this.shtPalletList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.SHIP);
                    
                }

                this.shtPalletList.InsertRow(this.shtPalletList.Rows.Count, false);

                this.btnAllCheck.Enabled = true;
                this.btnAllNotCheck.Enabled = true;
                this.btnRangeCheck.Enabled = true;
                this.btnRangeNotCheck.Enabled = true;
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtPalletList.Redraw = true;
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// ペーストをキャンセルする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtPalletList_ClippingData(object sender, ClippingDataEventArgs e)
        {
            switch (e.ClippingAction)
            {
                case ClippingAction.Paste:
                    switch (this.shtPalletList.ActivePosition.Column)
                    {
                        case SHEET_COL_PALLETNO:
                            // PalletNo.列の貼り付けはキャンセルする。
                            e.Cancel = true;
                            break;
                    }
                    break;
            }
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ情報の取得
        /// </summary>
        /// <returns>印刷用ヘッダ情報</returns>
        /// <create>H.Tsunamura 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable getHeader()
        {

            if (this.shtPalletList.EditState) return null;

            this.ClearMessage();

            // 現在表示中のPalletlist取得
            DataTable palletList = this.shtPalletList.DataSource as DataTable;

            // 印刷用ヘッダテーブル
            DataTable dtHeader = palletList.Clone();

            // Palletlistから印刷するPalletNoを取得
            foreach (DataRow item in palletList.Select(ComDefine.FLD_INSATU + " = 1 "))
            {
                if (!string.IsNullOrEmpty(ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.PALLET_NO)))
                {
                    dtHeader.Rows.Add(item.ItemArray);
                }
            }

            if (dtHeader.Rows.Count == 0)
            {
                // 印刷するデータがありません。
                this.ShowMessage("A9999999025");
                return null;
            }
            return dtHeader;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細取得・印刷/プレビュー
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <param name="dtHeader">印刷用ヘッダ情報</param>
        /// <create>H.Tsunamura 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void printData(bool isPreview, DataTable dtHeader)
        {
            if (dtHeader == null) return;
            if (!isPreview)
            {
                // 印刷してよろしいですか？ダイアログ
                DialogResult ret = this.ShowMessage("A9999999035");

                if (ret == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }

            try
            {
                // 明細を取得
                WsConnection.ConnK02 connK02 = new ConnK02();
                DataSet retds = connK02.GetBoxData(dtHeader);

                List<object> reports = new List<object>();

                foreach (DataRow dr in dtHeader.Rows)
                {
                    DataSet ds = retds.Copy();
                    DataTable tagList = dtHeader.Clone();

                    string palletNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO);

                    // 重複したBoxNoを除去し、BoxNoのカウントをする。
                    DataView dvs = new DataView(retds.Tables[palletNo]);
                    DataTable dtf = dvs.ToTable(true, Def_T_SHUKKA_MEISAI.BOX_NO);
                    dr[COL_BOX_COUNT] = dtf.Rows.Count;

                    tagList.Rows.Add(dr.ItemArray);
                    ds.Tables.Add(tagList.Copy());

                    if (this.cboHaccoSelect.SelectedValue.ToString() == PALLET_LIST_PRINT.LABEL1_LIST1_VALUE1)
                    {
                        // PalletListを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));
                        // PalletLabelを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                    }
                    else if (this.cboHaccoSelect.SelectedValue.ToString() == PALLET_LIST_PRINT.LABEL1_LIST2_VALUE1)
                    {
                        // PalletListを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));
                        // PalletLabelを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                    }


                    else if (this.cboHaccoSelect.SelectedValue.ToString() == PALLET_LIST_PRINT.LABEL2_LIST1_VALUE1)
                    {
                        // PalletListを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));
                        // PalletLabelを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                    }
                    else if (this.cboHaccoSelect.SelectedValue.ToString() == PALLET_LIST_PRINT.LABEL2_LIST2_VALUE1)
                    {
                        // PalletListを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100070_CLASS_NAME, ds, palletNo));

                        // PalletLabelを追加
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100110_CLASS_NAME, ds, Def_T_PALLETLIST_MANAGE.Name));
                    }
                }

                if (isPreview)
                {
                    PreviewPrinterSetting pps = new PreviewPrinterSetting();
                    pps.Landscape = true;
                    pps.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    // プレビュー
                    ReportHelper.Preview(this, reports.ToArray(), pps);

                    return;
                }

                ReportHelper.Print(LocalSetting.GetNormalPrinter(), reports.ToArray());

                // 新規発行時のみ印刷時発行日アップデート
                if (this.rdoSinki.Checked)
                {
                    CondK02 condK02 = new CondK02(this.UserInfo);


                    // PalletNoリストを渡して発効日のアップデート 
                    connK02.UpdPalletNoKanri(condK02, dtHeader);

                    //　グリッド更新

                    this.shtPalletList.DataSource = null;
                    this.shtPalletList.MaxRows = 0;
                    this.rdoSinki.Checked = false;
                    this.rdoSaiHakko.Checked = false;
                    this.btnAllCheck.Enabled = false;
                    this.btnAllNotCheck.Enabled = false;
                    this.btnRangeCheck.Enabled = false;
                    this.btnRangeNotCheck.Enabled = false;
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

