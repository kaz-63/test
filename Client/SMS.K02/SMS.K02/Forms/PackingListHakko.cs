using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using DSWUtil;
using WsConnection;
using Commons;
using ElTabelleHelper;
using WsConnection.WebRefK02;
using ActiveReportsHelper;
using WsConnection.WebRefAttachFile;
using SMS.E01;
using System.IO;
using System.Drawing;
using GrapeCity.Win.ElTabelle;
using SystemBase.Util;
using System.Globalization;
using SMS.K02.Properties;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// パッキングリスト発行
    /// </summary>
    /// <create>H.Tsunamura 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PackingListHakko : SystemBase.Forms.CustomOrderForm
    {

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// リス発行モード
        /// </summary>
        /// <create>T.Wakamatsu 2016/02/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum HakkoMode
        {
            Search,     // 検索
            Koji,       // 工事識別No.
            Select,     // 発行選択
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷カラム
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INSATU = 0;
        /// --------------------------------------------------
        /// <summary>
        /// リスト選択コンボボックスカラム
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_LISTSELECT = 1;
        /// --------------------------------------------------
        /// <summary>
        /// リスト選択カラム
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SELECTCODE = 6;
        /// --------------------------------------------------
        /// <summary>
        /// RowNoのフィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_ROW_NO = "ROW_NO";
        /// --------------------------------------------------
        /// <summary>
        /// CNOの文字フィールド名
        /// </summary>
        /// <create>H.Tsunamura 2010/08/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_CNO_CODE = "CNO_CODE";

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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public PackingListHakko(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                this.MakeCmbBox(this.cboHakkoSelect, HAKKOU_SELECT_PACKING.GROUPCD);

                this.InitializeSheet(this.shtPackingList);

                // シートのタイトルを設定
                shtPackingList.ColumnHeaders[0].Caption = Resources.PackingListHakko_Print;
                shtPackingList.ColumnHeaders[1].Caption = Resources.PackingListHakko_ListSelect;
                shtPackingList.ColumnHeaders[2].Caption = Resources.PackingListHakko_ConstructionIdentityNo;
                shtPackingList.ColumnHeaders[3].Caption = Resources.PackingListHakko_Ship;
                shtPackingList.ColumnHeaders[4].Caption = Resources.PackingListHakko_WorkSituation;
                shtPackingList.ColumnHeaders[5].Caption = Resources.PackingListHakko_ManagementNo;

                DataTable dt = this.GetCommon(LIST_SELECT.GROUPCD).Tables[Def_M_COMMON.Name];

                GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor sce = ElTabelleSheetHelper.NewSuperiorComboEditor();
                sce.ValueMember = Def_M_COMMON.VALUE1;
                sce.DisplayMember = Def_M_COMMON.ITEM_NAME;
                sce.ValueAsIndex = false;
                sce.DataSource = dt;
                sce.Editable = false;
                this.shtPackingList.Columns[SHEET_COL_LISTSELECT].Editor = sce;

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.fbrFunction.F06Button.Enabled = false;
                this.fbrFunction.F10Button.Enabled = false;

                this.shtPackingList.MaxRows = 0;

                this.shtPackingList.KeepHighlighted = true;
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // フォーカスセット
                txtKojiShikibetsu.Focus();
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 最も左上に表示されているセルの設定
                if (0 < this.shtPackingList.MaxRows)
                {
                    this.shtPackingList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPackingList.TopLeft.Row);
                }

                this.txtShip.Text = "";
                this.txtKojiShikibetsu.Text = "";

                this.ChangeEnabled(HakkoMode.Search);
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                return true;
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 発行選択が操作不可の場合は工事識別＋便での検索のため
                if (!this.cboHakkoSelect.Enabled)
                {
                    // コントロールの値が入っているかどうかのチェック
                    if (string.IsNullOrEmpty(this.txtKojiShikibetsu.Text))
                    {
                        // 工事識別NOを入力して下さい。
                        this.ShowMessage("A9999999029");
                        return false;
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

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tsunamura 2010/07/27</create>
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                WsConnection.ConnK02 connK02 = new ConnK02();


                cond.HakkoSelect = this.cboHakkoSelect.SelectedValue.ToString();
                cond.KojiName = this.txtKojiShikibetsu.Text;
                cond.Ship = this.txtShip.Text;

                this.shtPackingList.Redraw = true;
                DataSet ds = connK02.GetPackingList(cond);

                // 工事識別の存在チェック・工事識別Noが無かった場合のメッセージ表示
                if (ds == null)
                {
                    // 該当する工事識別Noはありません。
                    this.ShowMessage("A9999999030");
                    return false;
                }
                this.shtPackingList.DataSource = ds.Tables[Def_T_KIWAKU.Name];
                this.SetSheetComboValue();

                if (ds.Tables[Def_T_KIWAKU.Name].Rows.Count != 0)
                {
                    this.shtPackingList.Enabled = true;
                }
                else
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");

                    this.ChangeEnabled(HakkoMode.Search);
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtPackingList.Redraw = true;
                this.shtPackingList.Invalidate();
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F01Button_Click(sender, e);
                DataTable header = getHeader();

                if (header.Rows.Count == 0)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                DataSet retds = getData(header);
                // 印刷してよろしいですか？ダイアログ
                if (this.ShowMessage("A9999999035") != DialogResult.OK)
                {
                    return;
                }

                printData(false, retds);
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F02Button_Click(sender, e);

                DataTable header = getHeader();

                if (header.Rows.Count == 0)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                printData(true, getData(header));
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                this.SheetClear();
                this.ChangeEnabled(HakkoMode.Search);
                this.txtKojiShikibetsu.Focus();
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
        /// <create>H.Tajimi 2015/12/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
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
                this.shtPackingList.DataSource = null;
                this.shtPackingList.MaxRows = 0;
                this.shtPackingList.Enabled = false;
                this.cboHakkoSelect.SelectedIndex = 0;
                this.txtKojiShikibetsu.Focus();
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
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // エクセル出力
                DataTable header = getHeader();
                if (header.Rows.Count == 0)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
                    return;
                }

                DataSet retds = getData(header);
                if (retds == null)
                {
                    // 該当する明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return;
                }

                var conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = ComDefine.EXCEL_FILE_TEMPLATE;
                package.FileType = FileType.Template;
                // TemplateファイルのDL
                var retFile = conn.FileDownload(package);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    using (FileStream fs = new FileStream(Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMPLATE), FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(retFile.FileData, 0, retFile.FileData.Length);
                        fs.Close();
                    }
                }
                else
                {
                    // Excel出力に失敗しました。
                    this.ShowMessage("A7777777001");
                    return;
                }

                ExportKiwakuMeisai export = new ExportKiwakuMeisai();
                string msgID;
                string[] args;

                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.PackingListHakko_sfdExcel_Title;
                    frm.Filter = Resources.PackingListHakko_ExcelFilter;
                    frm.FileName = ComDefine.EXCEL_FILE_KIWAKU;
                    if (frm.ShowDialog() != DialogResult.OK) return;

                    bool ret = export.ExportExcel(frm.FileName, retds.Tables[Def_T_KIWAKU.KOJI_NO], out msgID, out args);

                    if (!ret)
                    {
                        this.ShowMessage(msgID, args);
                        return;
                    }
                }

                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.PackingListHakko_sfdExcel_Title;
                    frm.Filter = Resources.PackingListHakko_ExcelFilterXlsx;
                    foreach (DataRow dr in retds.Tables[Def_T_KIWAKU.Name].Rows)
                    {
                        var export2 = new ExportPackingList();
                        frm.FileName = string.Format(ComDefine.EXCEL_FILE_PACKING_LIST, ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME), ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP));
                        if (frm.ShowDialog() != DialogResult.OK) return;

                        bool ret = export2.ExportExcel(frm.FileName, dr, retds, out msgID, out args);
                        if (!ret)
                        {
                            this.ShowMessage(msgID, args);
                            return;
                        }
                    }
                    this.ShowMessage(msgID, args);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタンクリック


        /// --------------------------------------------------
        /// <summary>
        /// 工事No.での表示クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            // 最も左上に表示されているセルの設定
            if (0 < this.shtPackingList.MaxRows)
            {
                this.shtPackingList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPackingList.TopLeft.Row);
            }

            // 工事識別一覧表示
            if (!this.ShowKojiShikibetsuIchiran())
            {
                this.txtKojiShikibetsu.Focus();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 発行選択での表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/02/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Select_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            // 最も左上に表示されているセルの設定
            if (0 < this.shtPackingList.MaxRows)
            {
                this.shtPackingList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPackingList.TopLeft.Row);
            }

            ChangeEnabled(HakkoMode.Select);
            this.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// --------------------------------------------------
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            this.shtPackingList.Redraw = false;

            shtPackingList.CellRange = new Range("A:A");
            shtPackingList.CellValue = 1;

            this.shtPackingList.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// --------------------------------------------------
        private void btnAllNotCheck_Click(object sender, EventArgs e)
        {
            this.shtPackingList.Redraw = false;

            shtPackingList.CellRange = new Range("A:A");
            shtPackingList.CellValue = 0;

            this.shtPackingList.Redraw = true;

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
            objRanges = shtPackingList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtPackingList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtPackingList.CellRange = tmprange;
                shtPackingList.CellValue = 1;

            }
            this.shtPackingList.Redraw = true;
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
            objRanges = shtPackingList.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection);

            this.shtPackingList.Redraw = false;
            foreach (GrapeCity.Win.ElTabelle.Range range in objRanges)
            {
                GrapeCity.Win.ElTabelle.Range tmprange = new Range(0, range.TopRow, 0, range.BottomRow);

                shtPackingList.CellRange = tmprange;
                shtPackingList.CellValue = 0;

            }
            this.shtPackingList.Redraw = true;
        }
        #endregion

        #region フォーカスアウト
        /// --------------------------------------------------
        /// <summary>
        /// 工事識別Noのフォーカスアウトイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtKojiShikibetsu_Leave(object sender, EventArgs e)
        {
            kojiShikibetsu_Check();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 便のフォーカスアウトイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtShip_Leave(object sender, EventArgs e)
        {
            kojiShikibetsu_Check();
        }

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>H.Tajimi 2015/12/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtPackingList.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtPackingList.MaxRows)
            {
                this.shtPackingList.TopLeft = new Position(SHEET_COL_INSATU, this.shtPackingList.TopLeft.Row);
            }
            this.shtPackingList.DataSource = null;
            this.shtPackingList.MaxRows = 0;
            this.shtPackingList.Enabled = false;
            this.shtPackingList.Redraw = true;
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別Noか便が入力されている場合のチェック
        /// </summary>
        /// <create>H.Tsunamura 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void kojiShikibetsu_Check()
        {
            if (this.txtKojiShikibetsu.Text.Length != 0 || this.txtShip.Text.Length != 0)
            {
                this.cboHakkoSelect.SelectedIndex = 0;
                this.cboHakkoSelect.Enabled = false;
            }
            else
            {
                this.cboHakkoSelect.Enabled = true;
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの初期値を設定
        /// </summary>
        /// <create>H.Tsunamura 2010/07/27</create> 
        /// <update></update>
        /// --------------------------------------------------
        private void SetSheetComboValue()
        {
            for (int i = 0; i < this.shtPackingList.MaxRows; i++)
            {
                this.shtPackingList[SHEET_COL_LISTSELECT, i].Value = this.shtPackingList[SHEET_COL_SELECTCODE, i].Value;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ情報の取得
        /// </summary>
        /// <returns>印刷用ヘッダデータ</returns>
        /// <create>H.Tsunamura 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable getHeader()
        {
            this.ClearMessage();

            // 現在表示中のpackingList取得
            DataTable packingList = this.shtPackingList.DataSource as DataTable;

            // 印刷用ヘッダテーブル
            DataTable dtHeader = packingList.Clone();

            // packingListから印刷する管理Noを取得
            foreach (DataRow item in packingList.Select(ComDefine.FLD_INSATU + " = 1 "))
            {
                dtHeader.Rows.Add(item.ItemArray);
            }

            return dtHeader;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 明細の取得
        /// </summary>
        /// <param name="dtHeader">印刷用ヘッダデータ</param>
        /// <returns>印刷用明細データ</returns>
        /// <create>H.Tsunamura 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataSet getData(DataTable dtHeader)
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                // 明細を取得

                WsConnection.ConnK02 connK02 = new ConnK02();
                DataSet retds = connK02.GetPackingMeisai(cond, dtHeader);

                return retds;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷・プレビュー
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <param name="retds">印刷用明細データ</param>
        /// <create>H.Tsunamura 2010/08/18</create>
        /// <update>H.Tajimi 2015/11/26 ケースナンバー欠番対応</update>
        /// <update>K.Tsutsumi 2018/01/23 工事識別No 36進数化</update>
        /// <update>T.Nukaga 2021/04/07 機種桁数拡張対応</update>
        /// --------------------------------------------------
        private void printData(bool isPreview, DataSet retds)
        {
            try
            {
                if (retds == null)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                WsConnection.ConnAttachFile connAF = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                List<object> reports = new List<object>();

                // 機種分割用の列追加
                retds.Tables[Def_T_SHUKKA_MEISAI.Name].Columns.Add(ComDefine.FLD_KISHU_1, typeof(string));
                retds.Tables[Def_T_SHUKKA_MEISAI.Name].Columns.Add(ComDefine.FLD_KISHU_2, typeof(string));

                // 工事識別No分ループ
                for (int i = 0; i < retds.Tables[Def_T_KIWAKU.Name].Rows.Count; i++)
                {
                    DataSet ds = retds.Clone();

                    string kojiNo = ComFunc.GetFld(retds.Tables[Def_T_KIWAKU.Name], i, Def_T_KIWAKU.KOJI_NO);
                    string caseFile = ComFunc.GetFld(retds.Tables[Def_T_KIWAKU.Name], i, Def_T_KIWAKU.CASE_MARK_FILE);

                    if (!string.IsNullOrEmpty(caseFile))
                    {
                        package.FileName = caseFile;
                        package.FileType = FileType.CaseMark;
                        package.KojiNo = kojiNo;

                        // CASEマークのDL
                        FileDownloadResult ret = connAF.FileDownload(package);

                        if (ret.IsExistsFile)
                        {
                            using (System.IO.MemoryStream memStrm = new MemoryStream(ret.FileData))
                            using (Bitmap bmp = (Bitmap)Bitmap.FromStream(memStrm))
                            {
                                if (bmp == null) return;
                                retds.Tables[Def_T_KIWAKU.Name].Rows[i][ComDefine.FLD_CASE_MARK] = new Bitmap(bmp);
                            }
                        }
                    }

                    // ヘッダの追加
                    ds.Tables[Def_T_KIWAKU.Name].Rows.Add(retds.Tables[Def_T_KIWAKU.Name].Rows[i].ItemArray);

                    // 木枠明細の追加
                    foreach (DataRow item in retds.Tables[Def_T_KIWAKU.KOJI_NO].Select(Def_T_KIWAKU.KOJI_NO + " = '" + kojiNo + "'"))
                    {
                        string cno;
                        // 2015/11/26 H.Tajimi 印刷C/NOを出力するよう変更
                        cno = ComFunc.GetFld(item, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                        // ↑

                        item[ComDefine.FLD_SHIP_CNO] = cno;
                        string[] itemName = UtilString.DivideString(item[Def_T_KIWAKU_MEISAI.ITEM].ToString(), 15);
                        item["ITEM_1"] = itemName[0];
                        item["ITEM_2"] = itemName[1];
                        // 2012/07/04 K.tsutsumi Add 印刷日付
                        item["PRINT_DATE"] = ComFunc.GetFldToDateTime(item, "PRINT_DATE").ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
                        //
                        ds.Tables[Def_T_KIWAKU.KOJI_NO].Rows.Add(item.ItemArray);

                        // 出荷明細の追加
                        foreach (DataRow item2 in retds.Tables[Def_T_SHUKKA_MEISAI.Name].Select(Def_T_KIWAKU.KOJI_NO + " = '" + kojiNo
                            + "' AND CASE_NO = " + ComFunc.GetFld(item, Def_T_KIWAKU_MEISAI.CASE_NO)))
                        {
                            item2[ComDefine.FLD_SHIP_CNO] = cno;
                            item2[Def_T_KIWAKU_MEISAI.STYLE] = item[Def_T_KIWAKU_MEISAI.STYLE];
                            item2[Def_T_KIWAKU_MEISAI.NET_W] = item[Def_T_KIWAKU_MEISAI.NET_W];
                            item2[Def_T_KIWAKU_MEISAI.GROSS_W] = item[Def_T_KIWAKU_MEISAI.GROSS_W];
                            item2[Def_T_KIWAKU_MEISAI.MMNET] = item[Def_T_KIWAKU_MEISAI.MMNET];
                            item2[Def_T_KIWAKU_MEISAI.DIMENSION_H] = item[Def_T_KIWAKU_MEISAI.DIMENSION_H];
                            item2[Def_T_KIWAKU_MEISAI.DIMENSION_L] = item[Def_T_KIWAKU_MEISAI.DIMENSION_L];
                            item2[Def_T_KIWAKU_MEISAI.DIMENSION_W] = item[Def_T_KIWAKU_MEISAI.DIMENSION_W];
                            // 2012/07/04 K.tsutsumi Add 印刷日付
                            item2["PRINT_DATE"] = ComFunc.GetFldToDateTime(item2, "PRINT_DATE").ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
                            //

                            // 機種分割
                            string[] kishu = UtilString.DivideString(ComFunc.GetFld(item2, Def_T_SHUKKA_MEISAI.KISHU), 20);
                            item2[ComDefine.FLD_KISHU_1] = kishu[0];
                            item2[ComDefine.FLD_KISHU_2] = kishu[1];

                            ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Add(item2.ItemArray);
                        }

                        // ダミーデータの追加
                        DataRow ndr = ds.Tables[Def_T_SHUKKA_MEISAI.Name].NewRow();
                        ndr[ComDefine.FLD_SHIP_CNO] = cno;
                        ndr[Def_T_KIWAKU_MEISAI.STYLE] = item[Def_T_KIWAKU_MEISAI.STYLE];
                        ndr[Def_T_KIWAKU_MEISAI.NET_W] = item[Def_T_KIWAKU_MEISAI.NET_W];
                        ndr[Def_T_KIWAKU_MEISAI.GROSS_W] = item[Def_T_KIWAKU_MEISAI.GROSS_W];
                        ndr[Def_T_KIWAKU_MEISAI.MMNET] = item[Def_T_KIWAKU_MEISAI.MMNET];
                        ndr[Def_T_KIWAKU_MEISAI.DIMENSION_H] = item[Def_T_KIWAKU_MEISAI.DIMENSION_H];
                        ndr[Def_T_KIWAKU_MEISAI.DIMENSION_L] = item[Def_T_KIWAKU_MEISAI.DIMENSION_L];
                        ndr[Def_T_KIWAKU_MEISAI.DIMENSION_W] = item[Def_T_KIWAKU_MEISAI.DIMENSION_W];
                        ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Add(ndr.ItemArray);
                    }

                    // 現在の行数
                    int rowNo = UtilConvert.ToInt32(ComFunc.GetFld(ds.Tables[Def_T_KIWAKU.Name].Rows[0], FLD_ROW_NO, "0")) - 1;

                    if (this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.ALL_VALUE1
                        || this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.KONPOMEISAI_VALUE1)
                    {
                        // 梱包明細
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100080_CLASS_NAME, ds, Def_T_KIWAKU.KOJI_NO));
                    }

                    if (this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.ALL_VALUE1
                        || this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.MASTERPACKINGLIST_VALUE1)
                    {
                        // マスタパッキングリスト
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100090_CLASS_NAME, ds, Def_T_KIWAKU.KOJI_NO));
                    }

                    if (this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.ALL_VALUE1
                        || this.shtPackingList[SHEET_COL_LISTSELECT, rowNo].Value.ToString() == LIST_SELECT.PACKINGLIST_VALUE1)
                    {
                        // パッキングリスト
                        reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100100_CLASS_NAME, ds, Def_T_SHUKKA_MEISAI.Name));
                    }
                }

                if (isPreview)
                {
                    PreviewPrinterSetting pps = new PreviewPrinterSetting();
                    pps.Landscape = true;
                    pps.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    // プレビュー
                    ReportHelper.Preview(this, reports.ToArray(), pps);
                }
                else
                {
                    ReportHelper.Print(LocalSetting.GetNormalPrinter(), reports.ToArray());
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面表示

        #region 工事識別一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowKojiShikibetsuIchiran()
        {
            using (P02.Forms.KojiShikibetsuIchiran frm = new SMS.P02.Forms.KojiShikibetsuIchiran(base.UserInfo, "", this.txtKojiShikibetsu.Text, this.txtShip.Text, true))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    this.ClearMessage();
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtKojiShikibetsu.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    this.cboHakkoSelect.SelectedIndex = 0;
                    ChangeEnabled(HakkoMode.Koji);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">表示状態かどうか</param>
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnabled(HakkoMode hakkoMode)
        {
            bool canSearch = false;     // 検索時
            bool searchKoji = false;    // 工事識別No.検索後
            bool searchList = false;    // 発行選択検索後
            switch (hakkoMode)
            {
                case HakkoMode.Search:
                    canSearch = true;
                    break;
                case HakkoMode.Koji:
                    searchKoji = true;
                    break;
                case HakkoMode.Select:
                    searchList = true;
                    break;
                default:
                    canSearch = true;
                    break;
            }

            // 表示コントロールのロック/ロック解除
            this.shtPackingList.Enabled = !canSearch;
            this.btnAllCheck.Enabled = searchList;
            this.btnAllNotCheck.Enabled = searchList;
            this.btnRangeCheck.Enabled = searchList;
            this.btnRangeNotCheck.Enabled = searchList;
            // 検索条件のロック/ロック解除
            this.txtKojiShikibetsu.Enabled = canSearch;
            this.txtShip.Enabled = canSearch;
            this.btnDisp.Enabled = canSearch;
            this.cboHakkoSelect.Enabled = !searchKoji;
            this.btnDisp_Select.Enabled = !searchKoji;
            // ファンクションボタン制御
            this.fbrFunction.F01Button.Enabled = !canSearch;
            this.fbrFunction.F02Button.Enabled = !canSearch;
            this.fbrFunction.F06Button.Enabled = !canSearch;
            this.fbrFunction.F10Button.Enabled = !canSearch;
        }

        #endregion

    }
}
