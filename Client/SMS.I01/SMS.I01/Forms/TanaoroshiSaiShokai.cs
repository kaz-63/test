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
using GrapeCity.Win.ElTabelle.Editors;
using WsConnection.WebRefI01;
using SMS.E01;
using SMS.I01.Properties;

namespace SMS.I01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 在庫問合せ／メンテ
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TanaoroshiSaiShokai : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/21</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 棚卸
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/21</create>
            /// <update></update>
            /// --------------------------------------------------
            Tanaoroshi,
            /// --------------------------------------------------
            /// <summary>
            /// 検索結果クリア
            /// </summary>
            /// <create>H.Tajimi 2015/12/01</create>
            /// <update></update>
            /// --------------------------------------------------
            ResultClear,
        }

        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 6;

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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public TanaoroshiSaiShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // デフォルトはUpdate
                this.EditMode = SystemBase.EditMode.Update;
                this.InitializeSheet(this.shtMeisai);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.TanaoroshiSaiShokai_TagCode;
                shtMeisai.ColumnHeaders[1].Caption = Resources.TanaoroshiSaiShokai_BoxNo;
                shtMeisai.ColumnHeaders[2].Caption = Resources.TanaoroshiSaiShokai_StocktakingLocation;
                shtMeisai.ColumnHeaders[3].Caption = Resources.TanaoroshiSaiShokai_StockLocation;
                shtMeisai.ColumnHeaders[4].Caption = Resources.TanaoroshiSaiShokai_Status;
                shtMeisai.ColumnHeaders[5].Caption = Resources.TanaoroshiSaiShokai_State;
                shtMeisai.ColumnHeaders[6].Caption = Resources.TanaoroshiSaiShokai_Date;
                shtMeisai.ColumnHeaders[7].Caption = Resources.TanaoroshiSaiShokai_ProductNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.TanaoroshiSaiShokai_Code;
                shtMeisai.ColumnHeaders[9].Caption = Resources.TanaoroshiSaiShokai_DrawingAddtionalNo;
                shtMeisai.ColumnHeaders[10].Caption = Resources.TanaoroshiSaiShokai_Name;
                shtMeisai.ColumnHeaders[11].Caption = Resources.TanaoroshiSaiShokai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[12].Caption = Resources.TanaoroshiSaiShokai_Quantity;
                shtMeisai.ColumnHeaders[13].Caption = Resources.TanaoroshiSaiShokai_JpName;
                shtMeisai.ColumnHeaders[14].Caption = Resources.TanaoroshiSaiShokai_SectioningNo;
                shtMeisai.ColumnHeaders[15].Caption = Resources.TanaoroshiSaiShokai_Area;
                shtMeisai.ColumnHeaders[16].Caption = Resources.TanaoroshiSaiShokai_Floor;
                shtMeisai.ColumnHeaders[17].Caption = Resources.TanaoroshiSaiShokai_Model;
                shtMeisai.ColumnHeaders[18].Caption = Resources.TanaoroshiSaiShokai_MNo;
                shtMeisai.ColumnHeaders[19].Caption = Resources.TanaoroshiSaiShokai_DeliveryDestination;
                shtMeisai.ColumnHeaders[20].Caption = Resources.TanaoroshiSaiShokai_Ship;
                shtMeisai.ColumnHeaders[21].Caption = Resources.TanaoroshiSaiShokai_STNo;
                shtMeisai.ColumnHeaders[22].Caption = Resources.TanaoroshiSaiShokai_Memo;
                shtMeisai.ColumnHeaders[23].Caption = Resources.TanaoroshiSaiShokai_ARno;

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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboShukkaFlag.Focus();
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 2015/12/08 H.Tajimi クリア処理追加
                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.txtBukkenName.Text = string.Empty;
                // ↑
                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

                this.ChangeMode(DisplayMode.Initialize);
                // 画面再表示時はメッセージを表示しない
                MakeLocationCombo(false);
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // TAG時
                this.shtMeisai.UpdateData();
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 物件名チェック
                if (string.IsNullOrEmpty(this.txtBukkenNo.Text))
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("I0100020005");
                    this.btnBukkenIchiran.Focus();
                    return false;
                }

                // 検索用入力チェック
                // ロケーションが入っているか
                if (this.cboLocation.SelectedIndex == -1)
                {
                    // ロケーションを入力して下さい。
                    this.ShowMessage("I0100020001");
                    this.cboLocation.Focus();
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

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
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
        /// <returns>true:検索成功/false:検索失敗</returns
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                //データ取得
                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.Location = this.cboLocation.SelectedValue.ToString();
                // 棚卸日付はクライアント側日付で比較する。
                cond.InventDate = DateTime.Today.ToString("yyyy/MM/dd");

                DataSet ds = conn.GetTanaoroshiData(cond);
                DataTable dt = ds.Tables[Def_T_STOCK.Name];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    // 該当する差異データはありません。
                    this.ShowMessage("I0100040001");
                    this.cboLocation.Focus();
                    return false;
                }

                // データ表示
                this.shtMeisai.DataSource = dt;

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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
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
        /// <create>T.Wakamatsu 2013/08/21</create>
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                bool ret = false;

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.Location = this.cboLocation.SelectedValue.ToString();
                cond.WorkUserID = cond.LoginInfo.UserID;
                // 棚卸日付はクライアント側日付で比較する。
                cond.InventDate = DateTime.Today.ToString("yyyy/MM/dd");

                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();

                string errMsgID;
                string[] args;
                ret = conn.UpdTanaoroshiData(cond, dt, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    return false;
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/08/21</create>
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            // 通常はUpdate
            this.EditMode = SystemBase.EditMode.Update;
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;

                this.ChangeMode(DisplayMode.ResultClear);
                this.cboLocation.Focus();
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
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();

                this.cboShukkaFlag.Focus();
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
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
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
                    frm.Title = Resources.TanaoroshiSaiShokai_sfdExcel_Title;
                    frm.Filter = Resources.TanaoroshiSaiShokai_sfdExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_TANAOROSHI;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();

                    ExportTanaoroshiList export = new ExportTanaoroshiList();
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
        #endregion

        #region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            if (!this.RunSearch())
            {
                return;
            }
            this.ChangeMode(DisplayMode.Tanaoroshi);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 物件一覧ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnBukkenIchiran_Click(object sender, EventArgs e)
        {
            try
            {
                string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                string bukkenName = this.txtBukkenName.Text;
                using (var frm = new SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        this.txtBukkenNo.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NO);
                        this.txtBukkenName.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NAME);
                        MakeLocationCombo(true);
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
        /// Locationコンボボックス作成
        /// </summary>
        /// <param name="doMsg">メッセージを表示するか？</param>
        /// <create>T.Wakamatsu 2013/10/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void MakeLocationCombo(bool doMsg)
        {
            if (this.cboShukkaFlag.SelectedValue == null) return;
            // 物件に紐付くロケーションをコンボボックスにセット
            ConnI01 conn = new ConnI01();
            CondI01 cond = new CondI01(this.UserInfo);
            cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            cond.BukkenNo = this.txtBukkenNo.Text;
            // 棚卸日付はクライアント側日付で比較する。
            cond.InventDate = DateTime.Today.ToString("yyyy/MM/dd");

            DataSet ds = conn.GetTanaoroshiLocationCombo(cond);
            string tableName = Def_M_LOCATION.Name;
            DataTable dt = ds.Tables[tableName];

            if (dt.Rows.Count > 0)
            {
                this.ClearMessage();
                MakeCmbBox(this.cboLocation, dt, Def_M_LOCATION.LOCATION, Def_M_LOCATION.LOCATION, string.Empty, false);
                this.cboLocation.Focus();
            }
            else
            {
                this.cboLocation.DataSource = null;
                if (doMsg)
                {
                    // 該当する棚卸データはありません。
                    this.ShowMessage("I0100040003");
                    this.btnBukkenIchiran.Focus();
                }
            }
        }

        #endregion

        #region コンボボックス変更

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBukkenNo.Text = string.Empty;
            this.txtBukkenName.Text = string.Empty;
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

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
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
                        ChangeModeCond(true);

                        this.shtMeisai.DataSource = null;
                        this.shtMeisai.MaxRows = 0;
                        this.shtMeisai.Enabled = false;

                        this.fbrFunction.F01Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = false;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    case DisplayMode.Tanaoroshi:
                        // ----- メンテ時 -----
                        ChangeModeCond(false);

                        this.fbrFunction.F01Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = true;
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア時 -----
                        ChangeModeCond(true);

                        this.SheetClear();

                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    // ↑
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件モード切替
        /// </summary>
        /// <param name="enabled">検索条件部分の使用可フラグ</param>
        /// <create>T.Wakamatsu 2013/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeModeCond(bool enabled)
        {
            this.cboShukkaFlag.Enabled = enabled;
            this.btnBukkenIchiran.Enabled = enabled;
            this.cboLocation.Enabled = enabled;
            this.btnDisp.Enabled = enabled;

            this.shtMeisai.Enabled = !enabled;
        }

        #endregion

    }
}
