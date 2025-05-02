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
    /// ボックスリスト発行
    /// </summary>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BoxListHakko : SystemBase.Forms.CustomOrderForm
    {
        #region 定数
        // 印刷チェックボックスカラム
        private const int SHEET_COL_INSATU = 0;
        // BOXNoカラム
        private const int SHEET_COL_BOXNO = 1;
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
        public BoxListHakko(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.InitializeSheet(this.shtBoxList);
                this.shtBoxList.ShortCuts.Remove(Keys.Enter);
                this.shtBoxList.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
                this.shtBoxList.KeepHighlighted = true;

                // シートのタイトルを設定
                shtBoxList.ColumnHeaders[0].Caption = Resources.BoxListHakko_Print;
                shtBoxList.ColumnHeaders[1].Caption = Resources.BoxListHakko_BoxNo;
                shtBoxList.ColumnHeaders[2].Caption = Resources.BoxListHakko_DeliveryDestination;
                shtBoxList.ColumnHeaders[3].Caption = Resources.BoxListHakko_Ship;
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
                if (0 < this.shtBoxList.MaxRows)
                {
                    this.shtBoxList.TopLeft = new Position(SHEET_COL_INSATU, this.shtBoxList.TopLeft.Row);
                }

                this.shtBoxList.DataSource = null;
                this.shtBoxList.MaxRows = 0;
                this.shtBoxList.Enabled = false;
                this.rdoSinki.Checked = false;
                this.rdoSaiHakko.Checked = false;
                this.btnAllCheck.Enabled = false;
                this.btnAllNotCheck.Enabled = false;

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

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
            this.shtBoxList.Redraw = false;

            shtBoxList.CellRange = new Range("A:A");
            shtBoxList.CellValue = 1;

            this.shtBoxList.Redraw = true;
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
            this.shtBoxList.Redraw = false;

            shtBoxList.CellRange = new Range("A:A");
            shtBoxList.CellValue = 0;

            this.shtBoxList.Redraw = true;

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
            objRanges = shtBoxList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtBoxList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtBoxList.CellRange = tmprange;
                shtBoxList.CellValue = 1;

            }
            this.shtBoxList.Redraw = true;
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
            objRanges = shtBoxList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtBoxList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtBoxList.CellRange = tmprange;
                shtBoxList.CellValue = 0;

            }
            this.shtBoxList.Redraw = true;
        }

        # endregion

        #region ラジオボタン
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
            if (0 < this.shtBoxList.MaxRows)
            {
                this.shtBoxList.TopLeft = new Position(SHEET_COL_INSATU, this.shtBoxList.TopLeft.Row);
            }

            // 新規行削除
            this.shtBoxList.AllowUserToAddRows = false;

            // ボックスNo編集不可
            this.shtBoxList.Columns[SHEET_COL_BOXNO].Enabled = false;

            WsConnection.ConnK02 connK02 = new ConnK02();
            DataTable dt = connK02.GetBoxList();

            if (dt.Rows.Count != 0)
            {
                this.shtBoxList.Enabled = true;
                this.shtBoxList.DataSource = dt;
                this.btnAllCheck.Enabled = true;
                this.btnAllNotCheck.Enabled = true;
                this.btnRangeCheck.Enabled = true;
                this.btnRangeNotCheck.Enabled = true;
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
            else
            {
                this.shtBoxList.Enabled = false;
                this.shtBoxList.DataSource = null;
                this.shtBoxList.MaxRows = 0;

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
            if (0 < this.shtBoxList.MaxRows)
            {
                this.shtBoxList.TopLeft = new Position(SHEET_COL_INSATU, this.shtBoxList.TopLeft.Row);
            }

            // グリッドのクリア
            this.shtBoxList.Enabled = true;
            this.shtBoxList.DataSource = null;
            this.shtBoxList.MaxRows = 0;

            // 新規行追加
            this.shtBoxList.InsertRow(0, false);

            // ボックスNo編集可能
            this.shtBoxList.Columns[SHEET_COL_BOXNO].Enabled = true;
            this.shtBoxList.ActivePosition = new Position(SHEET_COL_BOXNO, 0);
            this.shtBoxList.Focus();

            this.btnAllCheck.Enabled = false;
            this.btnAllNotCheck.Enabled = false;
            this.btnRangeCheck.Enabled = false;
            this.btnRangeNotCheck.Enabled = false;

            this.fbrFunction.F01Button.Enabled = false;
            this.fbrFunction.F02Button.Enabled = false;
        }

        #endregion

        #region グリッド
        /// --------------------------------------------------
        /// <summary>
        /// グリッドのコピー対策
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtBoxList_ClippingData(object sender, ClippingDataEventArgs e)
        {
            switch (e.ClippingAction)
            {
                case ClippingAction.Paste:
                    switch (this.shtBoxList.ActivePosition.Column)
                    {
                        case SHEET_COL_BOXNO:
                            // BoxNo.列の貼り付けはキャンセルする。
                            e.Cancel = true;
                            break;
                    }
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// BoxNo入力確定後のチェック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtBoxList_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                // ボックスNo入力確定時検索処理
                if (this.shtBoxList.ActivePosition.Column != SHEET_COL_BOXNO || string.IsNullOrEmpty(this.shtBoxList.ActiveCell.Text))
                {
                    return;
                }
                this.ClearMessage();

                // 頭文字'B'の追加と字埋め
                if (UtilString.Substring(this.shtBoxList.ActiveCell.Text, 0, 1) != ComDefine.PREFIX_BOXNO)
                {
                    this.shtBoxList.ActiveCell.Text = ComDefine.PREFIX_BOXNO + UtilString.PadLeft(this.shtBoxList.ActiveCell.Text, 5, '0');
                }

                for (int i = 0; i < this.shtBoxList.Rows.Count; i++)
                {
                    if (i == this.shtBoxList.ActivePosition.Row) continue;
                    if (this.shtBoxList[SHEET_COL_BOXNO, i].Text == this.shtBoxList.ActiveCell.Text)
                    {
                        // {0}行目のBoxNo.[{1}]は存在しています。
                        this.ShowMessage("K0200020005", (this.shtBoxList.ActivePosition.Row + 1).ToString(), this.shtBoxList.ActiveCell.Text);
                        e.Cancel = true;
                        return;
                    }
                }

                WsConnection.ConnK02 connK02 = new ConnK02();
                CondK02 cond = new CondK02(this.UserInfo);
                cond.BoxNo = this.shtBoxList.ActiveCell.Text;

                DataTable dt = connK02.GetNonyusaki(cond);

                if (dt.Rows.Count == 0)
                {
                    // 該当BoxNo.は存在しません。
                    this.ShowMessage("K0200020002");
                    e.Cancel = true;
                    return;
                }

                if (ComFunc.GetFld(dt, 0, LISTHAKKO_FLAG.GROUPCD) == LISTHAKKO_FLAG.MIHAKKO_VALUE1)
                {
                    // 該当BoxNo.は未発行です。
                    this.ShowMessage("K0200020003");
                    e.Cancel = true;
                    return;
                }

                this.shtBoxList.Redraw = false;
                this.shtBoxList.ActiveCell.Enabled = false;

                // グリッドデータの更新
                if (this.shtBoxList.Rows.Count == 1)
                {
                    this.shtBoxList.DataSource = dt;
                }
                else
                {
                    this.shtBoxList[0, this.shtBoxList.ActivePosition.Row].Value = "1";
                    this.shtBoxList[1, this.shtBoxList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_T_BOXLIST_MANAGE.BOX_NO);
                    this.shtBoxList[2, this.shtBoxList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    this.shtBoxList[3, this.shtBoxList.ActivePosition.Row].Value = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.SHIP);
                    
                }

                this.shtBoxList.InsertRow(this.shtBoxList.Rows.Count, false);

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
                this.shtBoxList.Redraw = true;
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
            if (this.shtBoxList.EditState) return null;

            this.ClearMessage();

            // 現在表示中のboxlist取得
            DataTable boxList = this.shtBoxList.DataSource as DataTable;

            // 印刷用ヘッダテーブル
            DataTable dtHeader = boxList.Clone();

            // boxlistから印刷するBoxNoを取得
            foreach (DataRow item in boxList.Select(ComDefine.FLD_INSATU + " = 1 "))
            {
                if (!string.IsNullOrEmpty(ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.BOX_NO)))
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
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>K.Tsutsumi 2020/10/02 M-NO２行表示対応</update>
        /// <update>T.Nukaga 2021/04/07 機種桁数拡張対応</update>
        /// --------------------------------------------------
        private void printData(bool isPreview, DataTable dtHeader)
        {
            if (dtHeader == null) return;

            if (!isPreview)
            {
                // 印刷してよろしいですか？ダイアログ
                if (this.ShowMessage("A9999999035") != DialogResult.OK)
                {
                    return;
                }
            }

            try
            {
                // 明細を取得
                WsConnection.ConnK02 connK02 = new ConnK02();
                DataSet retds = connK02.GetMeisai(dtHeader);

                // 機種分割用の列追加(サーバー側のSQL更新を止めて、ここで列を追加)
                dtHeader.Columns.Add(ComDefine.FLD_KISHU_1, typeof(string));
                dtHeader.Columns.Add(ComDefine.FLD_KISHU_2, typeof(string));

                List<object> reports = new List<object>();

                foreach (DataRow dr in dtHeader.Rows)
                {
                    DataSet ds = retds.Copy();
                    DataTable tagList = dtHeader.Clone();
                    string boxNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO);

                    string[] area = UtilString.DivideString(ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.AREA), 8);
                    string[] floor = UtilString.DivideString(ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.FLOOR), 6);
                    // 2011/03/01 K.Tsutsumi Change 見た目で分割
                    //string[] nonyusakiName = UtilString.DivideString(ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME), 21);
                    string[] nonyusakiName = ComFunc.DivideStringEx(ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME), 21);
                    // ↑
                    string[] m_No = ComFunc.DivideStringEx(ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.M_NO), 9);
                    // 2021/04/07 T.Nukaga KISHU桁数拡張対応
                    string[] kishu = UtilString.DivideString(ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.KISHU), 16);
                    // ↑
                    // 1件目のエリアとフロア・機種・M-Noを取得
                    dr[Def_T_SHUKKA_MEISAI.AREA] = ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.AREA);
                    dr[Def_T_SHUKKA_MEISAI.FLOOR] = ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.FLOOR);
                    dr[Def_T_SHUKKA_MEISAI.KISHU] = ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.KISHU);
                    // 2015/12/09 H.Tajimi M_NO対応
                    dr[Def_T_SHUKKA_MEISAI.M_NO] = ComFunc.GetFld(retds, boxNo, 0, Def_T_SHUKKA_MEISAI.M_NO);
                    // ↑

                    dr[ComDefine.FLD_AREA_1] = area[0];
                    dr[ComDefine.FLD_AREA_2] = area[1];
                    dr[ComDefine.FLD_FLOOR_1] = floor[0];
                    dr[ComDefine.FLD_FLOOR_2] = floor[1];
                    dr[ComDefine.FLD_M_NO_1] = m_No[0];
                    dr[ComDefine.FLD_M_NO_2] = m_No[1];
                    dr[ComDefine.FLD_NONYUSAKI_NAME_1] = nonyusakiName[0];
                    dr[ComDefine.FLD_NONYUSAKI_NAME_2] = nonyusakiName[1];
                    // 2021/04/07 T.Nukaga KISHU桁数拡張対応
                    dr[ComDefine.FLD_KISHU_1] = kishu[0];
                    dr[ComDefine.FLD_KISHU_2] = kishu[1];
                    // ↑

                    // BOXの中の図番/形式分割
                    // 念のため存在確認
                    if (!string.IsNullOrEmpty(boxNo) && ds.Tables.Contains(boxNo))
                    {
                        // 図番/形式分割用の列追加
                        ds.Tables[boxNo].Columns.Add(ComDefine.FLD_ZUMEN_KEISHIKI_1, typeof(string));
                        ds.Tables[boxNo].Columns.Add(ComDefine.FLD_ZUMEN_KEISHIKI_2, typeof(string));

                        foreach (DataRow drMeisai in ds.Tables[boxNo].Rows)
                        {
                            string[] zumenKeishiki = UtilString.DivideString(ComFunc.GetFld(drMeisai, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI), 15);
                            drMeisai[ComDefine.FLD_ZUMEN_KEISHIKI_1] = zumenKeishiki[0];
                            drMeisai[ComDefine.FLD_ZUMEN_KEISHIKI_2] = zumenKeishiki[1];
                        }
                    }


                    // TagListは１データにつき3件表示 BoxListは1データで1件表示
                    tagList.Rows.Add(dr.ItemArray);
                    ds.Tables.Add(tagList.Copy());

                    tagList.Rows.Add(dr.ItemArray);
                    tagList.Rows.Add(dr.ItemArray);

                    // BoxList
                    reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100060_CLASS_NAME, ds, boxNo));
                    // TagList
                    reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100050_CLASS_NAME, tagList));

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


                    // BoxNoリストを渡して発効日のアップデート 
                    connK02.UpdBoxNoKanri(condK02, dtHeader);

                    //　グリッド更新

                    this.shtBoxList.DataSource = null;
                    this.shtBoxList.MaxRows = 0;
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
