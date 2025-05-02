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

using SystemBase.Util;
using SMS.M01.Properties;
using WsConnection.WebRefM01;
using WsConnection.WebRefMaster;
using SMS.E01;
using WsConnection.WebRefAttachFile;
using System.IO;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 配送先保守
    /// </summary>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HaisosakiHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/10</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 配送先CDの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DELIVER_CD = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 名称の列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_DELIVER_NAME = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 住所の列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ADDRESS = 2;
        /// --------------------------------------------------
        /// <summary>
        /// TELの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEL1 = 3;
        /// --------------------------------------------------
        /// <summary>
        /// TEL2の列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEL2 = 4;
        /// --------------------------------------------------
        /// <summary>
        /// FAXの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FAX = 5;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷物の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_SHIPPING_ITEM = 6;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_SHIPPING_TYPE = 7;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先担当者の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_SHIPPING_CONTACT = 8;
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日通常の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_UNAVAIL_NORM = 9;
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日正月連休の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_UNAVAIL_NY = 10;
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日5月連休の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_UNAVAIL_MAY = 11;
        /// --------------------------------------------------
        /// <summary>
        /// 受取不可日8月連休の列インデックス
        /// </summary>
        /// <create>J.Jeong 2024/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_UNAVAIL_AUG = 12;
        /// --------------------------------------------------
        /// <summary>
        /// 並び順の列インデックス
        /// </summary>
        /// <create>K.Tsutsumi 2018/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SORT_NO = 13;


        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public HaisosakiHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>J.Jeong 2024/07/29 出荷関連項目追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;

                // シートの初期化
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;

                // シートのタイトルを設定
                shtResult.ColumnHeaders[SHEET_COL_DELIVER_NAME].Caption = Resources.HaisosakiHoshu_Name;
                shtResult.ColumnHeaders[SHEET_COL_ADDRESS].Caption = Resources.HaisosakiHoshu_Address;
                shtResult.ColumnHeaders[SHEET_COL_TEL1].Caption = Resources.HaisosakiHoshu_Tel1;
                shtResult.ColumnHeaders[SHEET_COL_TEL2].Caption = Resources.HaisosakiHoshu_Tel2;
                shtResult.ColumnHeaders[SHEET_COL_FAX].Caption = Resources.HaisosakiHoshu_Fax;
                shtResult.ColumnHeaders[SHEET_COL_SORT_SHIPPING_ITEM].Caption = Resources.HaisosakiHoshu_ShippingItem;
                shtResult.ColumnHeaders[SHEET_COL_SORT_SHIPPING_TYPE].Caption = Resources.HaisosakiHoshu_ShippingType;
                shtResult.ColumnHeaders[SHEET_COL_SORT_SHIPPING_CONTACT].Caption = Resources.HaisosakiHoshu_ShippingContact;
                shtResult.ColumnHeaders[SHEET_COL_SORT_UNAVAIL_NORM].Caption = Resources.HaisosakiHoshu_UnavailNorm;
                shtResult.ColumnHeaders[SHEET_COL_SORT_UNAVAIL_NY].Caption = Resources.HaisosakiHoshu_UnavailNy;
                shtResult.ColumnHeaders[SHEET_COL_SORT_UNAVAIL_MAY].Caption = Resources.HaisosakiHoshu_UnavailMay;
                shtResult.ColumnHeaders[SHEET_COL_SORT_UNAVAIL_AUG].Caption = Resources.HaisosakiHoshu_UnavailAug;
                shtResult.ColumnHeaders[SHEET_COL_SORT_NO].Caption = Resources.HaisosakiHoshu_SortNo;

                // 住所欄の行数(ATTN用に一行確保する)
                this.txtAddress.MaxLineCount = ComDefine.MAX_LINE_LENGTH_DELIVERY - 1;

                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchName.Focus();
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtSearchName.Text = string.Empty;

                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit();
                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 名称入力チェック
                if (string.IsNullOrEmpty(this.txtName.Text))
                {
                    // 名称を入力して下さい。
                    this.ShowMessage("M0100150001");
                    this.txtName.Focus();
                    return false;
                }

                // 住所入力チェック
                if (string.IsNullOrEmpty(this.txtAddress.Text))
                {
                    // 住所を入力して下さい。
                    this.ShowMessage("M0100150002");
                    this.txtAddress.Focus();
                    return false;
                }

                // 登録行数チェック
                int lineLength = this.txtAddress.Lines.Length
                    + this.txtTel.Lines.Length
                    + this.txtTel2.Lines.Length
                    + this.txtFax.Lines.Length;
                if (lineLength >= ComDefine.MAX_LINE_LENGTH_DELIVERY)
                {
                    // {0}から{1}まで{2}行以内で設定して下さい。
                    string[] args = { this.lblAddress.Text, this.lblFax.Text, (ComDefine.MAX_LINE_LENGTH_DELIVERY - 1).ToString() };
                    this.ShowMessage("M0100150003", args);
                    this.txtAddress.Focus();
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
        /// <create>H.Tsuji 2018/12/10</create>
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
        /// <create>H.Tsuji 2018/12/10</create>
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);

                // 配送先情報の検索
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                cond.DeliverName = this.txtSearchName.Text;
                DataSet ds = conn.GetDeliverLikeSearch(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_DELIVER.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する配送先情報はありません。
                    this.ShowMessage("M0100150004");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_DELIVER.Name;
                // モード切り替え
                this.ChangeMode(DisplayMode.EndSearch);
                this.shtResult.Enabled = true;
                // 最も左上に表示されているセルの設定
                if (0 < this.shtResult.MaxRows)
                {
                    this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
                }
                this.shtResult.Focus();
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();
                if (ret)
                {
                    this.DisplayClearEdit();
                    // とりあえず検索
                    this.RunSearch();
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.InsDeliverData(cond, ds, out errMsgID, out args))
                {
                    if (errMsgID == "A9999999027" || errMsgID == "A9999999063")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.DeliverCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.DELIVER_CD);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdDeliverData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) || errMsgID == "M0100150005")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.DeliverCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.DELIVER_CD);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelDeliverData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100150005"
                        || errMsgID == "M0100150006")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
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

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/10</create>
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
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.txtSearchName.Focus();
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
        /// <create>J,Chen 2024/8/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // ファイル出力するデータを準備
                DataSet ds = new DataSet();
                //　出力データ取得
                var dt = (this.shtResult.DataSource as DataSet).Tables[Def_M_DELIVER.Name].Copy();
                dt = dt.Copy();
                dt.AcceptChanges();
                ds.Tables.Add(dt);

                // テンプレートファイルをダウンロード
                var connFile = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();
                package.FileName = ComDefine.EXCEL_FILE_DELIVER_TEMPLATE;
                package.FileType = FileType.Template;
                // TemplateファイルのDL
                var retFile = connFile.FileDownload(package);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    var path = Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_DELIVER_TEMPLATE);
                    File.WriteAllBytes(path, retFile.FileData);
                }
                else
                {
                    // TemplateのDownloadに失敗しました。
                    this.ShowMessage("A7777777003");
                    return;
                }

                // ファイル保存ダイアログを表示
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.HaisosakiHoshu_sdfExcel_Title;
                    frm.Filter = Resources.HaisosakiHoshu_sdfExcel_Filter;
                    frm.InitialDirectory = Resources.HaisosakiHoshu_sdfExcel_InitialDirectory;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_DELIVER
                        , DateTime.Now.ToString("yyyyMMdd")
                        );
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    // Excelファイルを出力
                    var export = new ExportHaisosakiHoshu();
                    var ret = export.ExportExcel(frm.FileName, ds);
                    if (ret)
                    {
                        // 配送先Excelファイルを出力しました。
                        this.ShowMessage("M0100150007");
                    }
                    else
                    {
                        // Excel出力に失敗しました。
                        this.ShowMessage("A7777777001");
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
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/10</create>
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
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tsuji 2018/12/10</create>
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

        #region 登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Insert);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Update);
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Delete);
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
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtResult.Redraw = false;
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

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>J.Jeong 2024/07/29 出荷関連項目追加</update>
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
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = true;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtName.Enabled = true;
                        this.txtAddress.Enabled = true;
                        this.txtTel.Enabled = true;
                        this.txtTel2.Enabled = true;
                        this.txtFax.Enabled = true;
                        this.txtShippingItem.Enabled = true;
                        this.txtShippingType.Enabled = true;
                        this.txtShippingContact.Enabled = true;
                        this.txtUnvailNorm.Enabled = true;
                        this.txtUnvailNy.Enabled = true;
                        this.txtUnvailMay.Enabled = true;
                        this.txtUnvailAug.Enabled = true;
                        this.numSortNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtName.Enabled = true;
                        this.txtAddress.Enabled = true;
                        this.txtTel.Enabled = true;
                        this.txtTel2.Enabled = true;
                        this.txtFax.Enabled = true;
                        this.txtShippingItem.Enabled = true;
                        this.txtShippingType.Enabled = true;
                        this.txtShippingContact.Enabled = true;
                        this.txtUnvailNorm.Enabled = true;
                        this.txtUnvailNy.Enabled = true;
                        this.txtUnvailMay.Enabled = true;
                        this.txtUnvailAug.Enabled = true;
                        this.numSortNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
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

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>J.Jeong 2024/07/29 出荷関連項目追加</update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtName.Text = string.Empty;
                this.txtAddress.Text = string.Empty;
                this.txtTel.Text = string.Empty;
                this.txtTel2.Text = string.Empty;
                this.txtFax.Text = string.Empty;
                this.txtShippingItem.Text = string.Empty;
                this.txtShippingType.Text = string.Empty;
                this.txtShippingContact.Text = string.Empty;
                this.txtUnvailNorm.Text = string.Empty;
                this.txtUnvailNy.Text = string.Empty;
                this.txtUnvailMay.Text = string.Empty;
                this.txtUnvailAug.Text = string.Empty;
                
                this.numSortNo.Clear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region データ選択

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時のチェック
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (this.shtResult.ActivePosition.Row < 0
                    && selectType != DataSelectType.Insert)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
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

        /// --------------------------------------------------
        /// <summary>
        /// データ選択制御部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelect(DataSelectType selectType)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                if (this.CheckInputDataSelect(selectType))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool ret = this.RunDataSelectExec(selectType);

                    if (ret)
                    {
                        switch (selectType)
                        {
                            case DataSelectType.Insert:
                                this.EditMode = SystemBase.EditMode.Insert;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Insert);
                                this.txtName.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtName.Focus();
                                break;
                            case DataSelectType.Delete:
                                this.EditMode = SystemBase.EditMode.Delete;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Delete);
                                this.fbrFunction.F01Button.Focus();
                                break;
                            default:
                                break;
                        }
                    }

                    return ret;
                }
                return false;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択実行部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>J.Jeong 2024/07/29 出荷関連項目追加</update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeDeliver();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    var conn = new ConnM01();
                    var cond = new CondM01(this.UserInfo);
                    cond.DeliverCD = this.shtResult[SHEET_COL_DELIVER_CD, this.shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetDeliver(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_DELIVER.Name))
                    {
                        // 他端末で削除されています。
                        this.ShowMessage("A9999999026");
                        // 消えてるのがあったから取り敢えず検索しとけ
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_DELIVER.Name];
                }

                // 表示データ設定
                this.txtName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.NAME);
                this.txtAddress.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.ADDRESS);
                this.txtTel.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.TEL1);
                this.txtTel2.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.TEL2);
                this.txtFax.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.FAX);
                this.txtShippingItem.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.SHIPPING_ITEM);
                this.txtShippingType.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.SHIPPING_TYPE);
                this.txtShippingContact.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.SHIPPING_CONTACT);
                this.txtUnvailNorm.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.UNAVAIL_NORM);
                this.txtUnvailNy.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.UNAVAIL_NY);
                this.txtUnvailMay.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.UNAVAIL_MAY);
                this.txtUnvailAug.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_DELIVER.UNAVAIL_AUG);
                this.numSortNo.Value = ComFunc.GetFldToDecimal(this._dtDispData, 0, Def_M_DELIVER.SORT_NO);

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 配送先マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 配送先マスタのデータテーブル作成
        /// </summary>
        /// <returns>配送先マスタのデータテーブル</returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>J.jeong 2024/07/29 出荷関連項目追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeDeliver()
        {
            try
            {
                var dt = new DataTable(Def_M_DELIVER.Name);
                dt.Columns.Add(Def_M_DELIVER.DELIVER_CD, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.NAME, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.ADDRESS, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.TEL1, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.TEL2, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.FAX, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.VERSION, typeof(object));
                dt.Columns.Add(Def_M_DELIVER.SHIPPING_ITEM, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.SHIPPING_TYPE, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.SHIPPING_CONTACT, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.UNAVAIL_NORM, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.UNAVAIL_NY, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.UNAVAIL_MAY, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.UNAVAIL_AUG, typeof(string));
                dt.Columns.Add(Def_M_DELIVER.SORT_NO, typeof(decimal));
                
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 登録データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <param name="dt">配送先マスタデータテーブル</param>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/10</create>
        /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
        /// <update>J.jeong 2024/07/29 出荷関連項目追加</update>
        /// --------------------------------------------------
        private void SetEditData(DataTable dt)
        {
            try
            {
                DataRow dr;
                if (0 < dt.Rows.Count)
                {
                    dr = dt.Rows[0];
                }
                else
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                // 名称
                dr[Def_M_DELIVER.NAME] = this.txtName.Text;
                // 住所
                dr[Def_M_DELIVER.ADDRESS] = this.txtAddress.Text;
                // TEL
                if (string.IsNullOrEmpty(this.txtTel.Text))
                {
                    dr[Def_M_DELIVER.TEL1] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.TEL1] = this.txtTel.Text;
                }
                // TEL2
                if (string.IsNullOrEmpty(this.txtTel2.Text))
                {
                    dr[Def_M_DELIVER.TEL2] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.TEL2] = this.txtTel2.Text;
                }
                // FAX
                if (string.IsNullOrEmpty(this.txtFax.Text))
                {
                    dr[Def_M_DELIVER.FAX] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.FAX] = this.txtFax.Text;
                }
                // 出荷物
                if (string.IsNullOrEmpty(this.txtShippingItem.Text))
                {
                    dr[Def_M_DELIVER.SHIPPING_ITEM] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.SHIPPING_ITEM] = this.txtShippingItem.Text;
                }
                // 出荷区分
                if (string.IsNullOrEmpty(this.txtShippingType.Text))
                {
                    dr[Def_M_DELIVER.SHIPPING_TYPE] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.SHIPPING_TYPE] = this.txtShippingType.Text;
                }
                // 出荷先担当者
                if (string.IsNullOrEmpty(this.txtShippingContact.Text))
                {
                    dr[Def_M_DELIVER.SHIPPING_CONTACT] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.SHIPPING_CONTACT] = this.txtShippingContact.Text;
                }
                // 受取不可日通常
                if (string.IsNullOrEmpty(this.txtUnvailNorm.Text))
                {
                    dr[Def_M_DELIVER.UNAVAIL_NORM] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.UNAVAIL_NORM] = this.txtUnvailNorm.Text;
                }
                // 受取不可日正月連休
                if (string.IsNullOrEmpty(this.txtUnvailNy.Text))
                {
                    dr[Def_M_DELIVER.UNAVAIL_NY] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.UNAVAIL_NY] = this.txtUnvailNy.Text;
                }
                // 受取不可日5月連休
                if (string.IsNullOrEmpty(this.txtUnvailMay.Text))
                {
                    dr[Def_M_DELIVER.UNAVAIL_MAY] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.UNAVAIL_MAY] = this.txtUnvailMay.Text;
                }
                // 受取不可日8月連休
                if (string.IsNullOrEmpty(this.txtUnvailAug.Text))
                {
                    dr[Def_M_DELIVER.UNAVAIL_AUG] = DBNull.Value;
                }
                else
                {
                    dr[Def_M_DELIVER.UNAVAIL_AUG] = this.txtUnvailAug.Text;
                }
                // 並び順
                dr[Def_M_DELIVER.SORT_NO] = this.numSortNo.Value;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

    }
}
