using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using DSWUtil;
using WsConnection;
using Commons;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using MultiRowTabelle;

using SMS.P02.Forms;
using WsConnection.WebRefAttachFile;
using SMS.K02.Properties;
using WsConnection.WebRefK02;
using ActiveReportsHelper;
using System.Globalization;
using System.Diagnostics;
using SMS.E01;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠まとめ印刷
    /// </summary>
    /// <create>D.Okumura 2019/09/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KiwakuMatomeInsatsu : SystemBase.Forms.CustomOrderForm
    {

        #region 定義

        /// --------------------------------------------------
        /// <summary>
        /// マルチrowのブロック行数
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ROW_BLOCK = 2;

        /// --------------------------------------------------
        /// <summary>
        /// 順序セル
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ORDER_NUM = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 印刷C/NOセル
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PRINT_CASE_NO = 1;

        /// --------------------------------------------------
        /// <summary>
        /// SHIPセル
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHIP = 2;

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別Noセル
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KOJI_NO = 9;

        /// --------------------------------------------------
        /// <summary>
        /// C/NOセル
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CASE_NO = 0;


        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 1;

        /// --------------------------------------------------
        /// <summary>
        /// 順番の列名
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FLD_ORDER_NUM = "ORDER_NUM";

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// ElTabelleのカラムタイプ
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ElTabelleColumnType
        {
            Text = 0,
            Number = 1,
            Button = 2
        }

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// マルチrowのフォーマット
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private MultiRowFormat _multiRowFormat = null;

        /// --------------------------------------------------
        /// <summary>
        /// 納入先データ(取得待避用、物件マスタ情報含む)
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtNonyusaki = null;

        /// --------------------------------------------------
        /// <summary>
        /// 順序連番
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _printOrder = 0;

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
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public KiwakuMatomeInsatsu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ（デザイナ用）
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private KiwakuMatomeInsatsu()
            : this(new UserInfo(), "", "", "")
        {
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ここにコントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。

                // 登録処理のメッセージを変更する場合はセット
                // 登録確認メッセージ
                //this.MsgInsertConfirm = "";
                // 登録完了メッセージ
                //this.MsgInsertEnd = "";

                // 修正処理のメッセージを変更する場合はセット
                // 修正確認メッセージ
                //this.MsgUpdateConfirm = "";
                // 修正完了メッセージ
                //this.MsgUpdateEnd = "";

                // 削除処理のメッセージを変更する場合はセット
                // 削除確認メッセージ
                //this.MsgDeleteConfirm = "";
                // 削除完了メッセージ
                //this.MsgDeleteEnd = "";

                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                // シートのタイトルを設定
                int colIndex = 0;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_Order;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_PrintCNo;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_Ship;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_CNo;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_PkgStyle;
                shtMeisai.ColumnHeaders[colIndex, 0].Caption = Resources.KiwakuMatomeInsatsu_Mment;
                shtMeisai.ColumnHeaders[colIndex++, 1].Caption = Resources.KiwakuMatomeInsatsu_WoodWeight;
                shtMeisai.ColumnHeaders[colIndex, 0].Caption = Resources.KiwakuMatomeInsatsu_GrossW;
                shtMeisai.ColumnHeaders[colIndex++, 1].Caption = Resources.KiwakuMatomeInsatsu_NetW;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_Item;
                shtMeisai.ColumnHeaders[colIndex++, 0].Caption = Resources.KiwakuMatomeInsatsu_Description;

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
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // フォーカス設定
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// Sheet初期化
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update>2023/12/12 J.Chen 重量小数点第2位まで対応</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                // フォーマット管理クラス生成
                this._multiRowFormat = new MultiRowFormat();

                // １レコード内の行数指定
                this._multiRowFormat.SetRowNum(SHEET_ROW_BLOCK);

                // セルの設定
                int colIndex = 0;
                Debug.Assert(SHEET_COL_ORDER_NUM == colIndex); //インデックスのチェック(Releaseではチェックしない)
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, FLD_ORDER_NUM, "###", true, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 3);
                Debug.Assert(SHEET_COL_PRINT_CASE_NO == colIndex);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, "^Ｔ", true, ImeMode.Off, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions), 14, true);
                Debug.Assert(SHEET_COL_SHIP == colIndex);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU.SHIP, null, false, ImeMode.Off, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_NO, "###,###,###", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.STYLE, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex, 0, 1, 1, Def_T_KIWAKU_MEISAI.MMNET, "###,##0.000", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 1, 1, 1, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex, 0, 1, 1, Def_T_KIWAKU_MEISAI.GROSS_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 1, 1, 1, Def_T_KIWAKU_MEISAI.NET_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.ITEM, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex, 0, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 1, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));

                // 非表示セルの設定
                Debug.Assert(SHEET_COL_KOJI_NO == colIndex);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.KOJI_NO, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_ID, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.SHUKKA_DATE, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.VERSION, null, false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.NET_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Number, colIndex++, 0, 1, 2, Def_T_KIWAKU_MEISAI.GROSS_W, "##,##0.00", false, ImeMode.Disable, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion


        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ベースでClearMessageの呼出しは行われています。
                // 工事識別NO
                this.txtKojiName.Clear();
                // 便
                this.txtShip.Clear();
                // 入力項目のクリア
                this.DisplayPartClear();

                // 検索条件のロック解除
                this.ChangeEnableViewMode(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力部分だけクリア
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayPartClear()
        {
            try
            {
                // データテーブルクリア
                this._dtNonyusaki = null;

                // 工事識別名称(印刷用)
                this.txtKojiNamePrint.Clear();
                // 便(印刷用)
                this.txtShipPrint.Clear();

                // (A)CASEMARK
                this.txtCaseMark.Clear();
                this.picCaseMark.Image = null;
                // (B)DELIVERY NO
                this.txtDeliveryNo.Clear();
                // (C)PORT OF DESTINATION
                this.txtPortOfDestination.Clear();
                // (D)AIR / BOAT
                this.txtAirBoat.Clear();
                // (E)DELIVERY DATE
                this.txtDeliveryDate.Clear();
                // (F)DELIVERY POINT
                this.txtDeliveryPoint.Clear();
                // (G)FACTORY
                this.txtFactory.Clear();
                // *REMARKS
                this.txtRemarks.Clear();
                // グリッド
                this.SheetClear();

                // 変数
                this._printOrder = 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (string.IsNullOrEmpty(this.txtKojiNamePrint.Text))
                {
                    // 印刷設定の工事識別Noを入力して下さい。
                    this.ShowMessage("K0200110004");
                    this.txtKojiNamePrint.Focus();
                    return false;
                }
                var shipName = this.txtShipPrint.Text;
                if (string.IsNullOrEmpty(shipName))
                {
                    // 印刷設定の便名を入力してください。
                    this.ShowMessage("K0200110005");
                    this.txtShipPrint.Focus();
                    return false;
                }

                // 編集内容実行時の入力チェック
                this.shtMeisai.Update();
                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();

                if (dt == null ||
                    dt.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.txtKojiName.Focus();
                    return false;
                }

                var dtMeisai = dt.Copy();
                // 印刷C/Noを反映する
                this.FillPrintCNo(dtMeisai);

                var dtEnum = dtMeisai.AsEnumerable().Select((dr, i) => new { DataRow = dr, Line = i + 1, Index = i });
                // 順番の未入力チェック
                var checkEmpty = dtEnum.FirstOrDefault(w => string.IsNullOrEmpty(ComFunc.GetFld(w.DataRow, FLD_ORDER_NUM)));
                if (checkEmpty != null)
                {
                    // {0}行目の{1}が入力されていません。
                    this.ShowMessage("A9999999043", checkEmpty.Line.ToString(), Resources.KiwakuMatomeInsatsu_Order);
                    this.shtMeisai.Focus();
                    return false;
                }
                // 順番の重複チェック
                var checkOrder = dtEnum
                    // 便ごとに1件のデータを抽出:工事識別Noでグループ化し、最初の1件を取得する
                    .GroupBy(w => ComFunc.GetFld(w.DataRow, Def_T_KIWAKU_MEISAI.KOJI_NO))
                    .Select(w => w.OrderBy(o => o.Index).First())
                    // 順番の重複チェック:順番でグループ化し、2件以上含まれるものを取得する
                    .GroupBy(w => ComFunc.GetFld(w.DataRow, FLD_ORDER_NUM))
                    .FirstOrDefault(w => w.Count() > 1);
                if (checkOrder != null)
                {
                    // {0}が{1}行目と{2}行目で重複しています。変更して下さい。
                    this.ShowMessage("A9999999084", Resources.KiwakuMatomeInsatsu_Order,
                        checkOrder.ElementAt(0).Line.ToString(), checkOrder.ElementAt(1).Line.ToString());
                    this.shtMeisai.Focus();
                    return false;
                }
                // 印刷C/NOの重複チェック
                var checkDup = dtEnum.GroupBy(w => ComFunc.GetFld(w.DataRow, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO))
                    .FirstOrDefault(w => w.Count() > 1);
                if (checkDup != null)
                {
                    // {0}が{1}行目と{2}行目で重複しています。変更して下さい。
                    this.ShowMessage("A9999999084", Resources.KiwakuMatomeInsatsu_PrintCNo,
                        checkDup.ElementAt(0).Line.ToString(), checkDup.ElementAt(1).Line.ToString());
                    this.shtMeisai.Focus();
                    return false;
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
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
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
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondK02 GetSearchCondition()
        {
            CondK02 cond = new CondK02(this.UserInfo);

            cond.KojiName = this.txtKojiName.Text;
            cond.Ship = this.txtShip.Text;

            return cond;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>D.Okumura 2019/09/02</create>
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
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {

                string errMsgID;
                string[] args;
                CondK02 cond = this.GetSearchCondition();
                ConnK02 conn = new ConnK02();

                DataSet ds = conn.GetKiwakuMatomeData(cond, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                if (ds == null || !ds.Tables.Contains(Def_T_KIWAKU.Name) ||
                                  !ds.Tables.Contains(Def_T_KIWAKU_MEISAI.Name) ||
                                  ds.Tables[Def_T_KIWAKU.Name].Rows.Count < 1)
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }
                // データの設定
                if (this._dtNonyusaki == null)
                {
                    // 画面の入力部分をクリア
                    this.DisplayPartClear();

                    // 画面項目設定
                    this._dtNonyusaki = ds.Tables[Def_M_NONYUSAKI.Name].Copy();
                    var dtKiwaku = ds.Tables[Def_T_KIWAKU.Name];

                    this.txtKojiNamePrint.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NAME);
                    this.txtShipPrint.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SHIP);
                    this.txtCaseMark.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                    this.txtDeliveryNo.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_NO);
                    this.txtPortOfDestination.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.PORT_OF_DESTINATION);
                    this.txtAirBoat.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.AIR_BOAT);
                    this.txtDeliveryDate.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_DATE);
                    this.txtDeliveryPoint.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_POINT);
                    this.txtFactory.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.FACTORY);
                    this.txtRemarks.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.REMARKS);

                    string kojiNo = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                    string fileName = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                    if (!string.IsNullOrEmpty(fileName) && this.FileDownload(kojiNo, fileName))
                    {
                        this.txtCaseMark.Text = fileName;
                        this.btnDelete.Enabled = true;
                        this.btnSelect.Enabled = false;
                    }
                    else
                    {
                        this.btnDelete.Enabled = false;
                        this.btnSelect.Enabled = true;
                    }
                    var dtKiwakuMeisai = ds.Tables[Def_T_KIWAKU_MEISAI.Name];
                    // データ反映
                    RunSearchExecFillResult(dtKiwakuMeisai, false);

                }
                else
                {
                    var nonyusaki = ds.Tables[Def_M_NONYUSAKI.Name];
                    if (!string.Equals(ComFunc.GetFld(this._dtNonyusaki, 0, Def_M_BUKKEN.BUKKEN_NAME),
                            ComFunc.GetFld(nonyusaki, 0, Def_M_BUKKEN.BUKKEN_NAME)))
                    {
                        // 物件が異なるため、追加することはできません。
                        this.ShowMessage("K0200110001");
                        return false;
                    }

                    var dtKiwakuMeisai = ds.Tables[Def_T_KIWAKU_MEISAI.Name];
                    var kojiNo = ComFunc.GetFld(dtKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.KOJI_NO);
                    if (this.shtMeisai.GetMultiRowGetDataSource().AsEnumerable()
                        .Any(w => string.Equals(kojiNo, ComFunc.GetFld(w, Def_T_KIWAKU_MEISAI.KOJI_NO))))
                    {
                        // 既に追加されている便(SHIP)です。
                        this.ShowMessage("K0200110002");
                        return false;
                    }

                    if (!string.Equals(ComFunc.GetFld(this._dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD),
                            ComFunc.GetFld(nonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD)))
                    {
                        // 納入先が異なりますが、追加しますか？
                        if (this.ShowMessage("K0200110003") != DialogResult.OK)
                        {
                            return false;
                        }
                    }

                    // データ反映
                    RunSearchExecFillResult(dtKiwakuMeisai, true);
                }
                this.grpPrintSettings.Enabled = true;

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
        /// 検索結果加工(表示用列の追加)
        /// </summary>
        /// <param name="dtKiwakuMeisai">木枠明細テーブル</param>
        /// <param name="isAppend">追加有無</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// <remarks>呼び出し元でtry/catchを行うこと</remarks>
        /// --------------------------------------------------
        private void RunSearchExecFillResult(DataTable dtInput, bool isAppend)
        {
            DataTable dtKiwakuMeisai = dtInput.Copy();
            dtKiwakuMeisai.Columns.Add(FLD_ORDER_NUM, typeof(int));
            bool isNewPrintOrder = true;
            // 空きが見つかるまでループする
            while (isNewPrintOrder)
            {
                isNewPrintOrder = false;
                this._printOrder++;
                for (var i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_ORDER_NUM, i].Text) == this._printOrder)
                        isNewPrintOrder = true;
                }
            }
            // 順序を設定する
            for (var i = 0; i < dtKiwakuMeisai.Rows.Count; i++)
            {
                dtKiwakuMeisai.Rows[i].SetField<int>(FLD_ORDER_NUM, this._printOrder);
            }

            try
            {
                this.shtMeisai.Redraw = false;
                if (isAppend)
                {
                    DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                    dt.Merge(dtKiwakuMeisai);
                    this.shtMeisai.SetMultiRowDataSource(dt, this._multiRowFormat);
                }
                else
                {
                    this.shtMeisai.SetMultiRowDataSource(dtKiwakuMeisai, this._multiRowFormat);
                }
                // 表示状態を反映
                // データセットを更新すると、CellのEnabled状態も吹き飛ぶので、再設定する
                // ValueChangedイベントで該当CellのEnabledを判定して値を設定している
                string lastKojiNo = "";
                int maxRow =  this.shtMeisai.Rows.Count;
                for (var i = 0; i < maxRow; i++)
                {
                    var currentKojiNo = this.shtMeisai[SHEET_COL_KOJI_NO, i].Text;
                    if (string.IsNullOrEmpty(currentKojiNo))
                        continue;
                    this.shtMeisai[SHEET_COL_ORDER_NUM, i].Enabled = !string.Equals(lastKojiNo, currentKojiNo);
                    lastKojiNo = currentKojiNo;
                }
                // 状態設定
                this.shtMeisai.TransformEditor = true;
                this.shtMeisai.Invalidate();
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }

        }

        #endregion

        #region 印刷処理
        /// --------------------------------------------------
        /// <summary>
        /// 明細の取得
        /// </summary>
        /// <returns>印刷用明細データ</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataSet GetData()
        {
            // 現在表示中のpackingList取得
            DataTable packingList = this.shtMeisai.GetMultiRowGetDataSource();
            if (packingList.Rows.Count < 0)
            {
                return null;
            }

            // 印刷用ヘッダテーブル
            DataTable dtHeader = packingList.Clone();
            // 並び替えて要求を発行する：順番＞ケース番号
            foreach (DataRow drData in packingList.Select("", string.Join(",", new string[] { FLD_ORDER_NUM, Def_T_KIWAKU_MEISAI.CASE_NO })))
            {
                dtHeader.ImportRow(drData);
            }
            // 印刷C/NO反映
            this.FillPrintCNo(dtHeader);
            CondK02 cond = new CondK02(this.UserInfo);
            // 明細を取得
            WsConnection.ConnK02 connK02 = new ConnK02();
            DataSet retds = connK02.GetMatomePackingMeisai(cond, dtHeader);

            return retds;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 印刷C/Noを反映する
        /// </summary>
        /// <param name="dt">対象データ(木枠明細)</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void FillPrintCNo(DataTable dt)
        {
            int i = 0;
            var shipPrint = this.txtShipPrint.Text;
            // 並び替え：順番＞ケース番号
            foreach (DataRow dr in dt.Select("", string.Join(",", new string[] { FLD_ORDER_NUM, Def_T_KIWAKU_MEISAI.CASE_NO })))
            {
                i++;
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO)))
                    continue;
                dr.SetField<string>(Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, string.Format(Resources.KiwakuMatomeInsatsu_FillCNoFormat, shipPrint, i));
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷・プレビュー
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <param name="retds">印刷用明細データ</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update>T.Nukaga 2021/04/15 機種桁数拡張対応</update>
        /// --------------------------------------------------
        private void PrintData(bool isPreview, DataSet retds)
        {
            List<object> reports = new List<object>();

            // 機種分割用の列追加
            retds.Tables[Def_T_SHUKKA_MEISAI.Name].Columns.Add(ComDefine.FLD_KISHU_1, typeof(string));
            retds.Tables[Def_T_SHUKKA_MEISAI.Name].Columns.Add(ComDefine.FLD_KISHU_2, typeof(string));

            // 工事識別No分ループ
            DataSet ds = retds.Clone();
            DataTable dtKiwaku = this.GetKiwakuNewInput();
            ds.Merge(dtKiwaku);
            // 画像を設定
            ds.Tables[Def_T_KIWAKU.Name].Rows[0][ComDefine.FLD_CASE_MARK] = this.picCaseMark.Image;

            // 木枠明細の追加
            foreach (DataRow item in retds.Tables[Def_T_KIWAKU.KOJI_NO].Rows)
            {
                string cno = ComFunc.GetFld(item, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                string kojiNo = ComFunc.GetFld(item, Def_T_KIWAKU_MEISAI.KOJI_NO);

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
                    string[] kishu = UtilString.DivideString(ComFunc.GetFld(item2, Def_T_SHUKKA_MEISAI.KISHU), 16);
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

            // 梱包明細
            reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100080_CLASS_NAME, ds, Def_T_KIWAKU.KOJI_NO));

            // マスタパッキングリスト
            reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100090_CLASS_NAME, ds, Def_T_KIWAKU.KOJI_NO));

            // パッキングリスト
            reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100100_CLASS_NAME, ds, Def_T_SHUKKA_MEISAI.Name));

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

        #endregion //印刷処理

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック[印刷]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            //base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                if (!this.CheckInput())
                    return;

                DataSet retds = GetData();

                if (retds == null)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                // 印刷してよろしいですか？ダイアログ
                if (this.ShowMessage("A9999999035") != DialogResult.OK)
                {
                    return;
                }

                PrintData(false, retds);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック[Preview]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                if (!this.CheckInput())
                    return;

                DataSet retds = GetData();

                if (retds == null)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                PrintData(true, retds);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック[便削除]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 最後の便の場合はClear動作にする
                this.shtMeisai.Update();
                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                if (dt == null || dt.Rows.Count == 0)
                {
                    this.shtMeisai.Focus();
                    return;
                }
                // 最後の1便である場合は、Clear動作をする
                if (dt.AsEnumerable().GroupBy(w => ComFunc.GetFld(w, Def_T_KIWAKU_MEISAI.KOJI_NO)).Count() <= 1)
                {
                    this.fbrFunction.F06Button.PerformClick();
                    return;
                }

                // 便(SHIP)[{0}]を削除してもよろしいですか？
                string ship = this.shtMeisai[SHEET_COL_SHIP, this.shtMeisai.ActivePosition.Row].Text;
                string kojiNo = this.shtMeisai[SHEET_COL_KOJI_NO, this.shtMeisai.ActivePosition.Row].Text;
                if (this.ShowMessage("K0200110006", ship) == DialogResult.OK)
                {
                    try
                    {
                        this.shtMeisai.Redraw = false;
                        var list = new List<int>();
                        for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                        {
                            if (string.Equals(kojiNo, this.shtMeisai[SHEET_COL_KOJI_NO, i].Text))
                            {
                                list.Add(i);
                            }
                        }
                        list.Reverse();
                        foreach (var row in list)
                        {
                            this.shtMeisai.MultiRowRemove(row);
                        }
                    }
                    finally
                    {
                        this.shtMeisai.Redraw = true;
                    }
                }
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック[クリア]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                // 印刷内容をClearします。よろしいですか？
                if (this.ShowMessage("K0200110008") != DialogResult.OK) return;
                // 入力項目のクリア
                this.DisplayPartClear();
                // 検索条件のロック解除
                this.ChangeEnableViewMode(false);
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック[All Clear]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をClearします。よろしいですか？
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック[補完C/No]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                var shipName = this.txtShipPrint.Text;
                if (string.IsNullOrEmpty(shipName))
                {
                    // 印刷設定の便名を入力してください。
                    this.ShowMessage("K0200110005");
                    this.txtShipPrint.Focus();
                    return;
                }
                // 順序の重複チェック
                this.shtMeisai.Update();
                DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();

                if (dt == null ||
                    dt.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.txtKojiName.Focus();
                    return;
                }

                var dtEnum = dt.AsEnumerable().Select((dr, index) => new { DataRow = dr, Line = index + 1, Index = index });
                var checkOrder = dtEnum
                    // 便ごとに1件のデータを抽出:工事識別Noでグループ化し、最初の1件を取得する
                    .GroupBy(w => ComFunc.GetFld(w.DataRow, Def_T_KIWAKU_MEISAI.KOJI_NO))
                    .Select(w => w.OrderBy(o => o.Index).First())
                    // 順番の重複チェック:順番でグループ化し、2件以上含まれるものを取得する
                    .GroupBy(w => ComFunc.GetFld(w.DataRow, FLD_ORDER_NUM))
                    .FirstOrDefault(w => w.Count() > 1);
                if (checkOrder != null)
                {
                    // {0}が{1}行目と{2}行目で重複しています。変更して下さい。
                    this.ShowMessage("A9999999084", Resources.KiwakuMatomeInsatsu_Order,
                        checkOrder.ElementAt(0).Line.ToString(), checkOrder.ElementAt(1).Line.ToString());
                    this.shtMeisai.Focus();
                    return;
                }

                // 印刷C/NOを補完します。よろしいですか？
                if (this.ShowMessage("K0200110007") != DialogResult.OK)
                    return;


                // 順序＞C/Noの順に連番を印刷C/NOへ記載
                int i = 0;
                foreach (var item in Enumerable.Range(0, this.shtMeisai.Rows.Count)
                    .Where(row => !string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_KOJI_NO, row].Text))
                    .Select(row => new {
                        Row = row,
                        CaseNo = UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_CASE_NO, row].Text),
                        OrderNum = UtilConvert.ToInt32(this.shtMeisai[SHEET_COL_ORDER_NUM, row].Text),
                    })
                    .OrderBy(w => w.CaseNo)
                    .OrderBy(w => w.OrderNum)
                    )
                {
                    i++;
                    // 印刷C/Noに入力がある場合はSkip
                    if (!string.IsNullOrEmpty(this.shtMeisai[SHEET_COL_PRINT_CASE_NO, item.Row].Text))
                        continue;
                    this.shtMeisai[SHEET_COL_PRINT_CASE_NO, item.Row].Text = string.Format(Resources.KiwakuMatomeInsatsu_FillCNoFormat, shipName, i);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック[Excel]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 入力チェック
                if (!this.CheckInput())
                    return;

                DataSet retds = GetData();
                if (retds == null)
                {
                    // 印刷するデータがありません。
                    this.ShowMessage("A9999999025");
                    return;
                }

                DataTable dtKiwaku = this.GetKiwakuNewInput();
                // TemplateファイルのDL
                var conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = ComDefine.EXCEL_FILE_TEMPLATE;
                package.FileType = FileType.Template;
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
                    }
                }
                else
                {
                    // TemplateのDownloadに失敗しました。
                    this.ShowMessage("A7777777003");
                    return;
                }


                ExportKiwakuMeisai export = new ExportKiwakuMeisai();
                ExportPackingList export2 = new ExportPackingList();
                string msgID;
                string[] args;

                string saveFileName;
                string saveFileName2;
                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.PackingListHakko_sfdExcel_Title;
                    frm.Filter = Resources.PackingListHakko_ExcelFilter;
                    frm.FileName = ComDefine.EXCEL_FILE_KIWAKU;
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;
                    saveFileName = frm.FileName;
                }
                bool ret = export.ExportExcel(saveFileName, retds.Tables[Def_T_KIWAKU.KOJI_NO], out msgID, out args);

                if (!ret)
                {
                    this.ShowMessage(msgID, args);
                    return;
                }

                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.PackingListHakko_sfdExcel_Title;
                    frm.Filter = Resources.PackingListHakko_ExcelFilterXlsx;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_PACKING_LIST, ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NAME), ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SHIP));
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;
                    saveFileName2 = frm.FileName;
                }
                string tempFile = null;
                try
                {
                    // CASE MARKがある場合は一時保存する
                    if (this.picCaseMark.Image != null)
                    {
                        tempFile = Path.Combine(Path.GetTempPath(), string.Format("{0}{1}", Path.GetRandomFileName(), Path.GetExtension(ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE))));
                        this.picCaseMark.Image.Save(tempFile);
                    }
                    bool ret2 = export2.ExportExcel(saveFileName2, dtKiwaku.Rows[0], retds, null, tempFile, out msgID, out args);
                    if (!ret2)
                    {
                        this.ShowMessage(msgID, args);
                        return;
                    }
                }
                finally
                {
                    if (tempFile != null)
                        File.Delete(tempFile);
                }
                this.ShowMessage(msgID, args);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック[閉じる]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
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


        #region 追加ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 追加ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// <update></update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();

                bool ret = false;
                ret = this.CheckInputSearch();
                if (!ret)
                {
                    this.txtKojiName.Focus();
                    return;
                }

                // 工事識別一覧
                ret = this.ShowKojiShikibetsuIchiran();
                if (!ret)
                {
                    this.txtKojiName.Focus();
                    return;
                }
                ret = this.RunSearch();
                if (ret)
                {
                    // 検索状態の反映
                    this.ChangeEnableViewMode(true);
                    this.txtKojiNamePrint.Focus();
                }
                else
                {
                    this.txtKojiName.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 選択ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                string fileFullName;
                string fileName;
                using (OpenFileDialog frm = new OpenFileDialog())
                {
                    frm.Filter = Resources.KiwakuMatomeInsatsu_ImageFile;
                    frm.Title = Resources.KiwakuMatomeInsatsu_FileOpen;
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    fileFullName = frm.FileName;
                    fileName = frm.SafeFileName;
                }
                if (string.IsNullOrEmpty(fileFullName))
                {
                    return;
                }

                this.txtCaseMark.Text = fileName;
                this.picCaseMark.Image = Image.FromFile(fileFullName);
                this.btnSelect.Enabled = false;
                this.btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 削除ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                // {0}を削除してもよろしいですか？
                if (this.ShowMessage("A9999999042", "CASE MARK FILE") == DialogResult.OK)
                {
                    this.txtCaseMark.Text = string.Empty;
                    this.picCaseMark.Image = null;
                    this.btnSelect.Enabled = true;
                    this.btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region グリッド

        #region ValueChanged

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの値が変わった時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                int row = e.Position.Row;

                switch (e.Position.Column)
                {
                    case SHEET_COL_ORDER_NUM:
                        if (this.shtMeisai[SHEET_COL_ORDER_NUM, row].Enabled)
                        {
                            var maxRows = this.shtMeisai.Rows.Count;
                            var kojiNo = this.shtMeisai[SHEET_COL_KOJI_NO, row].Text;
                            var currentData = this.shtMeisai[SHEET_COL_ORDER_NUM, row].Text;
                            for (var i = 0; i < maxRows; i++)
                            {
                                // 工事識別番号が異なる場合はスキップ
                                if (!string.Equals(kojiNo, this.shtMeisai[SHEET_COL_KOJI_NO, i].Text))
                                    continue;
                                // 順序編集可否が可の場合はスキップ
                                if (this.shtMeisai[SHEET_COL_ORDER_NUM, i].Enabled)
                                    continue;
                                this.shtMeisai[SHEET_COL_ORDER_NUM, i].Text = currentData;
                            }
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


        #endregion //グリッド

        #endregion

        #region モード切替操作

        /// --------------------------------------------------
        /// <summary>
        /// コントロールのEnabled切替
        /// </summary>
        /// <param name="isView"></param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 表示コントロールのロック/ロック解除
            this.shtMeisai.Enabled = isView;
            // 検索条件のロック/ロック解除
            //this.grpSearch.Enabled = !isView;
            // 印刷設定のロック/ロック解除
            this.grpPrintSettings.Enabled = isView;
            this.fbrFunction.F01Button.Enabled = isView; //印刷
            this.fbrFunction.F02Button.Enabled = isView; //Preview
            this.fbrFunction.F03Button.Enabled = isView; //便削除
            this.fbrFunction.F06Button.Enabled = isView; //Clear
            this.fbrFunction.F08Button.Enabled = isView; //補完C/NO
            this.fbrFunction.F10Button.Enabled = isView; //Excel
        }

        #endregion

        #region 登録用のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 木枠のデータテーブル作成
        /// </summary>
        /// <returns></returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaKiwaku()
        {
            DataTable dt = new DataTable(Def_T_KIWAKU.Name);
            dt.Columns.Add(Def_T_KIWAKU.KOJI_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.KOJI_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHIP, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.TOROKU_FLAG, typeof(string));
            dt.Columns.Add(ComDefine.FLD_CASE_MARK, typeof(Image));
            dt.Columns.Add(Def_T_KIWAKU.CASE_MARK_FILE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.PORT_OF_DESTINATION, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.AIR_BOAT, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_DATE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.DELIVERY_POINT, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.FACTORY, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.REMARKS, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SAGYO_FLAG, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_DATE, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_USER_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.UNSOKAISHA_NAME, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.INVOICE_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.OKURIJYO_NO, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.SHUKKA_FLAG, typeof(string));
            dt.Columns.Add(Def_T_KIWAKU.NONYUSAKI_CD, typeof(string));

            return dt;
        }
        #endregion

        #region 画面から木枠データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 画面から木枠データ取得
        /// </summary>
        /// <returns></returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetKiwakuNewInput()
        {
            DataTable dt = GetSchemaKiwaku();

            DataRow dr = dt.NewRow();

            dr[Def_T_KIWAKU.KOJI_NAME] = this.txtKojiNamePrint.Text;
            dr[Def_T_KIWAKU.SHIP] = this.txtShipPrint.Text;
            dr[Def_T_KIWAKU.TOROKU_FLAG] = null;
            dr[Def_T_KIWAKU.SAGYO_FLAG] = SAGYO_FLAG.KIWAKUMEISAI_VALUE1;
            dr[Def_T_KIWAKU.SHUKKA_DATE] = null;
            dr[Def_T_KIWAKU.SHUKKA_USER_ID] = null;
            dr[Def_T_KIWAKU.SHUKKA_USER_NAME] = null;
            dr[Def_T_KIWAKU.UNSOKAISHA_NAME] = null;
            dr[Def_T_KIWAKU.INVOICE_NO] = null;
            dr[Def_T_KIWAKU.OKURIJYO_NO] = null;
            dr[Def_T_KIWAKU.SHUKKA_FLAG] = SHUKKA_FLAG.NORMAL_VALUE1;

            if (!string.IsNullOrEmpty(this.txtCaseMark.Text))
            {
                dr[Def_T_KIWAKU.CASE_MARK_FILE] = this.txtCaseMark.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.CASE_MARK_FILE] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryNo.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_NO] = this.txtDeliveryNo.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_NO] = null;
            }
            if (!string.IsNullOrEmpty(this.txtPortOfDestination.Text))
            {
                dr[Def_T_KIWAKU.PORT_OF_DESTINATION] = this.txtPortOfDestination.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.PORT_OF_DESTINATION] = null;
            }
            if (!string.IsNullOrEmpty(this.txtAirBoat.Text))
            {
                dr[Def_T_KIWAKU.AIR_BOAT] = this.txtAirBoat.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.AIR_BOAT] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryDate.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_DATE] = this.txtDeliveryDate.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_DATE] = null;
            }
            if (!string.IsNullOrEmpty(this.txtDeliveryPoint.Text))
            {
                dr[Def_T_KIWAKU.DELIVERY_POINT] = this.txtDeliveryPoint.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.DELIVERY_POINT] = null;
            }
            if (!string.IsNullOrEmpty(this.txtFactory.Text))
            {
                dr[Def_T_KIWAKU.FACTORY] = this.txtFactory.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.FACTORY] = null;
            }
            if (!string.IsNullOrEmpty(this.txtRemarks.Text))
            {
                dr[Def_T_KIWAKU.REMARKS] = this.txtRemarks.Text;
            }
            else
            {
                dr[Def_T_KIWAKU.REMARKS] = null;
            }

            dt.Rows.Add(dr);

            return dt;
        }
        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>D.Okumura 2019/09/02</create>
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
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 複数行表示のフォーマット設定

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions)
        {
            SetFormat(format, colType, col, row, colSpan, rowSpan, dataField, digit, isEnabled, imeMode, actions, null, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <param name="maxLength">MaxLength</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions, int? maxLength)
        {
            SetFormat(format, colType, col, row, colSpan, rowSpan, dataField, digit, isEnabled, imeMode, actions, maxLength, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数表表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="digit">フォーマット文</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="imeMode">IMEモード</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <param name="maxLength">MaxLength</param>
        /// <param name="isLengthAsByte">最大文字数をバイト単位と文字単位のどちらで指定するか</param>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, string digit, bool isEnabled, ImeMode imeMode, KeyAction[] actions, int? maxLength, bool? isLengthAsByte)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    if (!string.IsNullOrEmpty(digit))
                    {
                        textEditor.Format = digit;
                    }
                    if (maxLength != null)
                    {
                        textEditor.MaxLength = maxLength.Value;
                        if (isLengthAsByte != null)
                        {
                            textEditor.LengthAsByte = isLengthAsByte.Value;
                        }
                    }
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    if (string.IsNullOrEmpty(digit))
                    {
                        numberEditor.Format = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    else
                    {
                        numberEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                        numberEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    }
                    if (maxLength != null && maxLength.Value != 0)
                    {
                        string maxValue = "9";
                        maxValue = maxValue.PadLeft(maxLength.Value, '9');
                        numberEditor.MaxValue = DSWUtil.UtilConvert.ToDecimal(maxValue);
                        numberEditor.MinValue = 0;
                    }
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = null;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.ImeMode = imeMode;
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        #endregion

        #region 画像ファイル処理

        #region Download

        /// --------------------------------------------------
        /// <summary>
        /// ファイルのダウンロード処理
        /// </summary>
        /// <param name="kojiNo">工事識別番号</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool FileDownload(string kojiNo, string fileName)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.CaseMark;
                package.KojiNo = kojiNo;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    // ファイルがありません。
                    return false;
                }
                using (MemoryStream memStrm = new MemoryStream(result.FileData))
                {
                    Bitmap bmp = null;
                    bmp = (Bitmap)Bitmap.FromStream(memStrm);
                    if (bmp == null)
                    {
                        // null
                        return false;
                    }
                    this.picCaseMark.Image = bmp;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 重複チェック

        /// --------------------------------------------------
        /// <summary>
        /// 重複データのインデックス取得
        /// </summary>
        /// <remarks>
        /// 配列の中から引数に指定された値を検索し、見つかった場合は
        /// その値の配列内でのインデックスを返します。
        /// 見つからなかった場合は、-1を返します。
        /// </remarks>
        /// <param name="array">重複チェック対象データの配列</param>
        /// <param name="value">重複チェックしたい値</param>
        /// <param name="index">重複チェックしたい値のリスト内のインデックス</param>
        /// <returns>重複データのインデックス</returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetDuplicateDataIndex(string[] array, string value, int index)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i != index && array[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region 画面表示

        #region 工事識別一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>D.Okumura 2019/09/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowKojiShikibetsuIchiran()
        {
            string kojiName = this.txtKojiName.Text;
            string ship = this.txtShip.Text;
            using (KojiShikibetsuIchiran frm = new KojiShikibetsuIchiran(this.UserInfo, null, kojiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtKojiName.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #endregion
    }
}
