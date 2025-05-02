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
    /// オペ履歴照会
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class OpeRirekiShokai : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/20</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/20</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
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
        // TagCodeのカラム位置
        private const int COL_TAG_CODE = 0;
        // 入庫ロケーションのカラム位置
        private const int COL_LOCATION = 1;
        // TagNoのカラム位置
        private const int COL_TAG_NO = 36;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public OpeRirekiShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // 保存は常にInsert
                this.EditMode = SystemBase.EditMode.Insert;
                this.InitializeSheet(this.shtMeisai);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboSagyoFlag, ZAIKO_SAGYO_FLAG.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.OpeRirekiShokai_WorkDate;
                shtMeisai.ColumnHeaders[1].Caption = Resources.OpeRirekiShokai_TagCode;
                shtMeisai.ColumnHeaders[2].Caption = Resources.OpeRirekiShokai_StockLocation;
                shtMeisai.ColumnHeaders[3].Caption = Resources.OpeRirekiShokai_Status;
                shtMeisai.ColumnHeaders[4].Caption = Resources.OpeRirekiShokai_WorkDescription;
                shtMeisai.ColumnHeaders[5].Caption = Resources.OpeRirekiShokai_State;
                shtMeisai.ColumnHeaders[6].Caption = Resources.OpeRirekiShokai_Worker;
                shtMeisai.ColumnHeaders[7].Caption = Resources.OpeRirekiShokai_StockCompletionDate;
                shtMeisai.ColumnHeaders[8].Caption = Resources.OpeRirekiShokai_ProductNo;
                shtMeisai.ColumnHeaders[9].Caption = Resources.OpeRirekiShokai_Code;
                shtMeisai.ColumnHeaders[10].Caption = Resources.OpeRirekiShokai_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[11].Caption = Resources.OpeRirekiShokai_Area;
                shtMeisai.ColumnHeaders[12].Caption = Resources.OpeRirekiShokai_Floor;
                shtMeisai.ColumnHeaders[13].Caption = Resources.OpeRirekiShokai_Model;
                shtMeisai.ColumnHeaders[14].Caption = Resources.OpeRirekiShokai_MNo;
                shtMeisai.ColumnHeaders[15].Caption = Resources.OpeRirekiShokai_JpName;
                shtMeisai.ColumnHeaders[16].Caption = Resources.OpeRirekiShokai_Name;
                shtMeisai.ColumnHeaders[17].Caption = Resources.OpeRirekiShokai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[18].Caption = Resources.OpeRirekiShokai_SectioningNo;
                shtMeisai.ColumnHeaders[19].Caption = Resources.OpeRirekiShokai_Quantity;
                shtMeisai.ColumnHeaders[20].Caption = Resources.OpeRirekiShokai_DeliveryDestination;
                shtMeisai.ColumnHeaders[21].Caption = Resources.OpeRirekiShokai_Ship;
                shtMeisai.ColumnHeaders[22].Caption = Resources.OpeRirekiShokai_STNo;
                shtMeisai.ColumnHeaders[23].Caption = Resources.OpeRirekiShokai_Memo;
                shtMeisai.ColumnHeaders[24].Caption = Resources.OpeRirekiShokai_ARNo;
                shtMeisai.ColumnHeaders[25].Caption = Resources.OpeRirekiShokai_AssemblyDate;
                shtMeisai.ColumnHeaders[26].Caption = Resources.OpeRirekiShokai_BoxNo;
                shtMeisai.ColumnHeaders[27].Caption = Resources.OpeRirekiShokai_PalletNo;
                shtMeisai.ColumnHeaders[28].Caption = Resources.OpeRirekiShokai_ConstructionIdentityManagementNo;
                shtMeisai.ColumnHeaders[29].Caption = Resources.OpeRirekiShokai_InternalManagementKey;
                shtMeisai.ColumnHeaders[30].Caption = Resources.OpeRirekiShokai_WoodFrameNo;
                shtMeisai.ColumnHeaders[31].Caption = Resources.OpeRirekiShokai_BoxPackingDate;
                shtMeisai.ColumnHeaders[32].Caption = Resources.OpeRirekiShokai_PalletPackingDate;
                shtMeisai.ColumnHeaders[33].Caption = Resources.OpeRirekiShokai_WoodFrameDate;
                shtMeisai.ColumnHeaders[34].Caption = Resources.OpeRirekiShokai_ShipDate;
                shtMeisai.ColumnHeaders[35].Caption = Resources.OpeRirekiShokai_ShippingCompany;
                shtMeisai.ColumnHeaders[36].Caption = Resources.OpeRirekiShokai_InvoiceNo;
                shtMeisai.ColumnHeaders[37].Caption = Resources.OpeRirekiShokai_OkurijyoNo;
                shtMeisai.ColumnHeaders[38].Caption = Resources.OpeRirekiShokai_BLNo;
                shtMeisai.ColumnHeaders[39].Caption = Resources.OpeRirekiShokai_AcceptanceDate;
                shtMeisai.ColumnHeaders[40].Caption = Resources.OpeRirekiShokai_AcceptanceStaff;

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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 2015/12/08 H.Tajimi クリア処理追加
                // 入力値のクリア
                this.txtBukkenName.Text = string.Empty;
                this.txtHinmeiJp.Text = string.Empty;
                this.txtARNo.Text = string.Empty;
                this.txtHinmei.Text = string.Empty;
                this.txtTagNo.Text = string.Empty;
                this.txtBoxNo.Text = string.Empty;
                this.txtPalletNo.Text = string.Empty;
                this.txtZumenKeishiki.Text = string.Empty;
                this.txtKiwakuNo.Text = string.Empty;
                this.txtCaseNo.Text = string.Empty;
                this.txtArea.Text = string.Empty;
                this.txtFloor.Text = string.Empty;
                this.txtKuwariNo.Text = string.Empty;

                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.cboSagyoFlag.SelectedValue = SAGYO_FLAG.DEFAULT_VALUE1;

                this.dtpJissekiDateFrom.Value = DateTime.Today;
                this.dtpJissekiDateTo.Value = DateTime.Today;
                // ↑

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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
        {
                // TAG時
                this.shtMeisai.UpdateData();

                for (int i = 0; i < this.shtMeisai.MaxRows - 1; i++)
		        {
                    if (string.IsNullOrEmpty(this.shtMeisai[COL_TAG_CODE, i].Text))
                    {
                        // TagCodeを入力して下さい。
                        this.ShowMessage("I0100020002", (i + 1).ToString());
                        this.shtMeisai.Focus();
                        this.shtMeisai.ActivePosition = new Position(COL_TAG_CODE, i);
                        return false;
                    }
                    else if (string.IsNullOrEmpty(this.shtMeisai[COL_TAG_NO, i].Text))
                    {
                        // TagCodeが不正な値です。正しい値を入力して下さい。
                        this.ShowMessage("I0100020003", (i + 1).ToString());
                        this.shtMeisai.Focus();
                        this.shtMeisai.ActivePosition = new Position(COL_TAG_CODE, i);
                        return false;
                    }
                    else if (string.IsNullOrEmpty(this.shtMeisai[COL_LOCATION, i].Text))
                    {
                        // 入庫ロケーションを入力して下さい。
                        this.ShowMessage("I0100020004", (i + 1).ToString());
                        this.shtMeisai.Focus();
                        this.shtMeisai.ActivePosition = new Position(COL_LOCATION, i);
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

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
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
                    // 2015/11/30 H.Tajimi 一覧ボタン非表示のため物件名にフォーカス設定
                    this.txtBukkenName.Focus();
                    // ↑
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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
                cond.JissekiDateFrom = this.dtpJissekiDateFrom.Text;
                // To日付は+1日して比較
                cond.JissekiDateTo = this.dtpJissekiDateTo.Value.AddDays(1).ToString("yyyy/MM/dd");

                cond.SagyoFlag = this.cboSagyoFlag.SelectedValue.ToString();
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.HinmeiJp = this.txtHinmeiJp.Text;
                cond.Hinmei = this.txtHinmei.Text;
                cond.ZumenKeishiki = this.txtZumenKeishiki.Text;
                cond.Area = this.txtArea.Text;
                cond.Floor = this.txtFloor.Text;
                cond.KuwariNo = this.txtKuwariNo.Text;
                cond.KiwakuNo = this.txtKiwakuNo.Text;
                cond.CaseNo = this.txtCaseNo.Text;
                if (!string.IsNullOrEmpty(this.txtARNo.Text))
                {
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                }
                if (!string.IsNullOrEmpty(this.txtPalletNo.Text))
                {
                    cond.PalletNo = this.lblPallet.Text + this.txtPalletNo.Text.PadLeft(5, '0');
                }
                if (!string.IsNullOrEmpty(this.txtBoxNo.Text))
                {
                    cond.BoxNo = this.lblBox.Text + this.txtBoxNo.Text.PadLeft(5, '0');
                }
                cond.TagNo = this.txtTagNo.Text;

                DataSet ds = conn.GetJisseki(cond);
                string tableName = Def_T_JISSEKI.Name;

                DataTable dt = ds.Tables[tableName];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    // 該当する履歴はありません。
                    this.ShowMessage("I0100060001");
                    dtpJissekiDateFrom.Focus();
                    return false;
                }

                // データ表示
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = dt;
                this.shtMeisai.Redraw = true;

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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/08/20</create>
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/20</create>
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

                this.dtpJissekiDateFrom.Focus();
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
        /// <create>T.Wakamatsu 2013/08/20</create>
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
                    frm.Title = Resources.OpeRirekiShokai_sfdExcel_Title;
                    frm.Filter = Resources.OpeRirekiShokai_sfdExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_OPE_RIREKI;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();

                    ExportOpeRirekiList export = new ExportOpeRirekiList();
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            // シートクリア
            this.ChangeMode(DisplayMode.Initialize);
            // 2015/11/30 H.Tajimi 物件名一覧画面表示
            if (!this.ShowBukkenMeiIchiran())
            {
                this.txtBukkenName.Focus();
                return;
            }
            // ↑
            this.ChangeMode(DisplayMode.EndSearch);
        }

        #endregion 

        #region コンボボックス変更

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBukkenNo.Text = string.Empty;
            this.txtBukkenName.Text = string.Empty;

            DSWComboBox cbo = sender as DSWComboBox;
            if (cbo.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                txtARNo.Enabled = true;
            }
            else
            {
                txtARNo.Text = string.Empty;
                txtARNo.Enabled = false;
            }
        }

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>T.Wakaatsu 2013/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtMeisai.Redraw = false;
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(0, 0);
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
        /// <create>T.Wakamatsu 2013/08/20</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
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
                        this.cboShukkaFlag.Enabled = true;
                        this.btnBukkenIchiran.Enabled = true;
                        this.dtpJissekiDateFrom.Enabled = true;
                        this.dtpJissekiDateTo.Enabled = true;
                        this.cboSagyoFlag.Enabled = true;
                        this.txtTagNo.Enabled = true;
                        this.btnDisp.Enabled = true;
                        // 2015/11/30 H.Tajimi 物件名は入力可
                        this.txtBukkenName.Enabled = true;
                        // ↑

                        // シートクリア
                        this.SheetClear();

                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = false;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.cboShukkaFlag.Enabled = true;
                        this.btnBukkenIchiran.Enabled = true;
                        this.dtpJissekiDateFrom.Enabled = true;
                        this.dtpJissekiDateTo.Enabled = true;
                        this.cboSagyoFlag.Enabled = true;
                        this.txtTagNo.Enabled = true;
                        this.btnDisp.Enabled = true;
                        // 2015/11/30 H.Tajimi 物件名は入力可
                        this.txtBukkenName.Enabled = true;
                        // ↑

                        this.shtMeisai.Enabled = true;

                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = true;
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア -----
                        this.txtBukkenName.Enabled = true;

                        // シートクリア
                        this.SheetClear();

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

        #endregion

        #region 画面表示

        #region 物件名一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowBukkenMeiIchiran()
        {
            string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            string bukkenName = this.txtBukkenName.Text;
            using (var frm = new SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName, true))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtBukkenNo.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                    this.txtBukkenName.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

    }
}
