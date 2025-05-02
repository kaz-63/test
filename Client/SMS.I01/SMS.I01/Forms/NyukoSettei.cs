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
using SMS.I01.Properties;

namespace SMS.I01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 入庫設定
    /// </summary>
    /// <create>T.Wakamatsu 2013/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NyukoSettei : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後（在庫単位はB or P）
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// Tag登録時
            /// </summary>
            /// <create>T.Wakamatsu 2013/07/30</create>
            /// <update></update>
            /// --------------------------------------------------
            TagInsert,
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
        private const int COL_TAG_NO = 39;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        public NyukoSettei(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/07/31</create>
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
                this.MakeCmbBox(this.cboZaikoTani, ZAIKO_TANI.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.NyukoSettei_TagCode;
                shtMeisai.ColumnHeaders[1].Caption = Resources.NyukoSettei_Location;
                shtMeisai.ColumnHeaders[2].Caption = Resources.NyukoSettei_StockLocation;
                shtMeisai.ColumnHeaders[3].Caption = Resources.NyukoSettei_Status;
                shtMeisai.ColumnHeaders[4].Caption = Resources.NyukoSettei_State;
                shtMeisai.ColumnHeaders[5].Caption = Resources.NyukoSettei_Date;
                shtMeisai.ColumnHeaders[6].Caption = Resources.NyukoSettei_ProductNo;
                shtMeisai.ColumnHeaders[7].Caption = Resources.NyukoSettei_Code;
                shtMeisai.ColumnHeaders[8].Caption = Resources.NyukoSettei_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[9].Caption = Resources.NyukoSettei_Name;
                shtMeisai.ColumnHeaders[10].Caption = Resources.NyukoSettei_DrawingNoFormat;
                shtMeisai.ColumnHeaders[11].Caption = Resources.NyukoSettei_Quantity;
                shtMeisai.ColumnHeaders[12].Caption = Resources.NyukoSettei_JpName;
                shtMeisai.ColumnHeaders[13].Caption = Resources.NyukoSettei_SectioningNo;
                shtMeisai.ColumnHeaders[14].Caption = Resources.NyukoSettei_Area;
                shtMeisai.ColumnHeaders[15].Caption = Resources.NyukoSettei_Floor;
                shtMeisai.ColumnHeaders[16].Caption = Resources.NyukoSettei_Model;
                shtMeisai.ColumnHeaders[17].Caption = Resources.NyukoSettei_MNo;
                shtMeisai.ColumnHeaders[18].Caption = Resources.NyukoSettei_DeliveryDestination;
                shtMeisai.ColumnHeaders[19].Caption = Resources.NyukoSettei_Ship;
                shtMeisai.ColumnHeaders[20].Caption = Resources.NyukoSettei_STNo;
                shtMeisai.ColumnHeaders[21].Caption = Resources.NyukoSettei_Memo;
                shtMeisai.ColumnHeaders[22].Caption = Resources.NyukoSettei_ARNo;
                shtMeisai.ColumnHeaders[23].Caption = Resources.NyukoSettei_AssemblyDate;
                shtMeisai.ColumnHeaders[24].Caption = Resources.NyukoSettei_BoxNo;
                shtMeisai.ColumnHeaders[25].Caption = Resources.NyukoSettei_PalletNo;
                shtMeisai.ColumnHeaders[26].Caption = Resources.NyukoSettei_ConstructionIdentityManagementNo;
                shtMeisai.ColumnHeaders[27].Caption = Resources.NyukoSettei_InternalManagementKey;
                shtMeisai.ColumnHeaders[28].Caption = Resources.NyukoSettei_WoodFrameNo;
                shtMeisai.ColumnHeaders[29].Caption = Resources.NyukoSettei_BoxPackingDate;
                shtMeisai.ColumnHeaders[30].Caption = Resources.NyukoSettei_PalletPackingDate;
                shtMeisai.ColumnHeaders[31].Caption = Resources.NyukoSettei_WoodFramePackingDate;
                shtMeisai.ColumnHeaders[32].Caption = Resources.NyukoSettei_ShipDate;
                shtMeisai.ColumnHeaders[33].Caption = Resources.NyukoSettei_ShippingCompany;
                shtMeisai.ColumnHeaders[34].Caption = Resources.NyukoSettei_InvoiceNo;
                shtMeisai.ColumnHeaders[35].Caption = Resources.NyukoSettei_OkurijyoNo;
                shtMeisai.ColumnHeaders[36].Caption = Resources.NyukoSettei_BLNo;
                shtMeisai.ColumnHeaders[37].Caption = Resources.NyukoSettei_AcceptanceDate;
                shtMeisai.ColumnHeaders[38].Caption = Resources.NyukoSettei_AcceptanceStaff;


                this.txtZaikoNo.Text = string.Empty;

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
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtZaikoNo.Text = string.Empty;
                // 2015/12/08 H.Tajimi クリア処理追加
                this.txtBukkenName.Text = string.Empty;
                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.cboLocation.DataSource = null;
                this.dtpStockDate.Value = DateTime.Now;
                // ↑

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

                if (this.cboZaikoTani.Items.Count > 0 && this.cboZaikoTani.SelectedValue.ToString() == ZAIKO_TANI.TAG_VALUE1)
                {
                    cboZaikoTani.SelectedValue = ZAIKO_TANI.PALLET_VALUE1;
                    // ChangeModeを２回走らせない
                    return;
                }

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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.lblZaikoNoPrefix.Text == ZAIKO_TANI.PALLET_VALUE1 ||
                    this.lblZaikoNoPrefix.Text == ZAIKO_TANI.BOX_VALUE1)
                {
                    // BOX、Pallet時
                    // ロケーションが入っているか
                    if (this.cboLocation.SelectedIndex == -1)
                    {
                        // ロケーションを入力して下さい。
                        this.ShowMessage("I0100020001");
                        this.cboLocation.Focus();
                        return false;
                    }
                }
                else
                {
                    // TAG時
                    this.shtMeisai.UpdateData();

                    if (this.shtMeisai.MaxRows < 2)
                    {
                        // 明細が入力されていません。
                        this.ShowMessage("A9999999028");
                        this.shtMeisai.Focus();
                        return false;
                    }

                    for (int i = 0; i < this.shtMeisai.MaxRows - 1; i++)
			        {
                        if (string.IsNullOrEmpty(this.shtMeisai[COL_TAG_CODE, i].Text))
                        {
                            // {0}行目のTagCodeを入力して下さい。
                            this.ShowMessage("I0100020002", (i + 1).ToString());
                            this.shtMeisai.Focus();
                            this.shtMeisai.ActivePosition = new Position(COL_TAG_CODE, i);
                            return false;
                        }
                        else if (string.IsNullOrEmpty(this.shtMeisai[COL_TAG_NO, i].Text))
                        {
                            // {0}行目のTagCodeが不正な値です。正しい値を入力して下さい。
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
                        for (int j = i + 1; j < this.shtMeisai.MaxRows - 1; j++)
                        {
                            if (this.shtMeisai[COL_TAG_CODE, i].Text == this.shtMeisai[COL_TAG_CODE, j].Text)
                            {
                                // {0}行目のTagCodeは重複しています。行削除して下さい。
                                this.ShowMessage("I0100020011", (j + 1).ToString());
                                this.shtMeisai.Focus();
                                this.shtMeisai.ActivePosition = new Position(COL_TAG_CODE, j);
                                return false;
                            }
                        }
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
        /// <create>T.Wakamatsu 2013/07/31</create>
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

                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtZaikoNo.Text))
                {
                    // 在庫No.を入力して下さい。
                    this.ShowMessage("I0100020006");
                    this.txtZaikoNo.Focus();
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

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
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
                cond.ZaikoNo = lblZaikoNoPrefix.Text + txtZaikoNo.Text;


                DataSet ds = conn.GetShukkaZaiko(cond);
                string tableName = Def_T_STOCK.Name;

                DataTable dt = ds.Tables[tableName];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    this.ShowMessage("I0100020007");
                    txtZaikoNo.Focus();
                    return false;
                }

                //ロケーションコンボボックスセット
                this.SetComboLocation();

                // データ表示
                this.shtMeisai.DataSource = dt;

                if (cboLocation.SelectedIndex != -1)
                {
                    for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                    {
                        this.shtMeisai[COL_LOCATION, i].Text = cboLocation.SelectedValue.ToString();
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

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                bool ret = false;

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                if (this.lblZaikoNoPrefix.Text == ZAIKO_TANI.PALLET_VALUE1 ||
                    this.lblZaikoNoPrefix.Text == ZAIKO_TANI.BOX_VALUE1)
                {
                    cond.Location = this.cboLocation.SelectedValue.ToString();
                }
                cond.ZaikoNo = lblZaikoNoPrefix.Text + txtZaikoNo.Text;
                cond.StockDate = this.dtpStockDate.Value.ToShortDateString();
                cond.WorkUserID = cond.LoginInfo.UserID;

                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();

                // シート上のコンボボックスでは選択インデックスしか取れないため、
                // 入庫先ロケーションセット
                if (this.lblZaikoNoPrefix.Text == ZAIKO_TANI.PALLET_VALUE1 ||
                    this.lblZaikoNoPrefix.Text == ZAIKO_TANI.BOX_VALUE1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[ComDefine.FLD_NYUKO_LOCATION] = cboLocation.SelectedValue;
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++ )
                    {
                        dt.Rows[i][ComDefine.FLD_NYUKO_LOCATION] = this.shtMeisai[COL_LOCATION, i].Text;
                    }
                }
                dt.AcceptChanges();

                string errMsgID;
                string[] args;
                ret = conn.InsZaikoData(cond, dt, out errMsgID, out args);
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// <create>T.Wakamatsu 2013/07/31</create>
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
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                
                int row = this.shtMeisai.ActivePosition.Row;
                if (this.shtMeisai.MaxRows > 1 && row < this.shtMeisai.MaxRows - 1)
                {
                    // 選択行を削除してもよろしいですか？ダイアログ表示
                    if (this.ShowMessage("I0100020008") == DialogResult.OK)
                    {
                        this.shtMeisai.RemoveRow(row, false);
                        this.shtMeisai.UpdateData();
                        this.shtMeisai.Focus();
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
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

                if (this.cboZaikoTani.Items.Count > 0 && this.cboZaikoTani.SelectedValue.ToString() == ZAIKO_TANI.TAG_VALUE1)
                {
                    cboZaikoTani.SelectedValue = ZAIKO_TANI.PALLET_VALUE1;
                }
                else
                {
                    this.ChangeMode(DisplayMode.ResultClear);
                    this.txtZaikoNo.Focus();
                }
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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region グリッドクリック
        /// --------------------------------------------------
        /// <summary>
        /// グリッドクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                //セルのイベント処理です
                switch (e.Name)
                {
                    case CellNotifyEvents.TextChanged:
                        if (e.Position.Column == COL_TAG_CODE)
                        {
                            string str = shtMeisai.ActiveCell.Text;
                            if (str.Length == 10)
                            {
                                //データ取得
                                ConnI01 conn = new ConnI01();
                                CondI01 cond = new CondI01(this.UserInfo);
                                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                                cond.BukkenNo = this.txtBukkenNo.Text;
                                cond.ZaikoNo = str;


                                DataSet ds = conn.GetShukkaZaiko(cond);
                                string tableName = Def_T_STOCK.Name;

                                DataTable dt = ds.Tables[tableName];

                                // 対象データがない場合はメッセージ
                                if (dt.Rows.Count == 0)
                                {
                                    this.ShowMessage("I0100020009");

                                    for (int i = 0; i < this.shtMeisai.MaxColumns; i++)
                                    {
                                        if (i != COL_TAG_CODE)
                                            this.shtMeisai[i, e.Position.Row].Text = "";
                                    }
                                    return;
                                }

                                // データ表示
                                DataTable dtMeisai = this.shtMeisai.DataSource as DataTable;
                                if (dtMeisai == null)
                                {
                                    this.shtMeisai.DataSource = dt;
                                }
                                else
                                {
                                    for (int i = 0; i < shtMeisai.Columns.Count; i++)
			                        {
                                        if (!string.IsNullOrEmpty(shtMeisai.Columns[i].DataField))
                                        {
                                            shtMeisai[i, e.Position.Row].Text = dt.Rows[0][shtMeisai.Columns[i].DataField].ToString();
                                        }
			                        }
                                    this.shtMeisai.UpdateRowToDataSet(e.Position.Row);
                                }

                                return;
                            }
                            else
                            {
                                for (int i = 0; i < this.shtMeisai.MaxColumns; i++)
                                {
                                    if (i != COL_TAG_CODE)
                                        this.shtMeisai[i, e.Position.Row].Text = "";
                                }
                                DataTable dt = this.shtMeisai.DataSource as DataTable;
                                this.shtMeisai.UpdateRowToDataSet(e.Position.Row);
                                return;
                            }
                        }
                        break;
                    case CellNotifyEvents.SelectedIndexChanged:
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

        #region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            // 2015/11/30 H.Tajimi 物件名一覧表示
            if (!this.ShowBukkenMeiIchiran(true))
            {
                this.txtBukkenName.Focus();
                return;
            }
            // ↑

            // 成功した場合検索部を操作不可に
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
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBukkenNo.Text = string.Empty;
            this.txtBukkenName.Text = string.Empty;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 在庫単位コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void cboZaikoTani_SelectedIndexChanged(object sender, EventArgs e)
        {
            string zaikoTani = cboZaikoTani.SelectedValue.ToString();

            lblZaikoNoPrefix.Text = zaikoTani;
            txtZaikoNo.Text = string.Empty;

            if (zaikoTani == ZAIKO_TANI.TAG_VALUE1)
            {
                // 2015/11/30 H.Tajimi 一覧ボタン非表示のため、物件一覧を表示
                if (!this.ShowBukkenMeiIchiran(false))
                {
                    return;
                }
                // ↑
                if (string.IsNullOrEmpty(txtBukkenNo.Text))
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("I0100020005");
                    cboZaikoTani.SelectedValue = ZAIKO_TANI.PALLET_VALUE1;
                    // 2015/11/30 H.Tajimi 一覧ボタン非表示のため物件名にフォーカス設定
                    this.txtBukkenName.Focus();
                    // ↑
                    return;
                }

                //ロケーションコンボボックスセット
                this.SetComboLocation();

                this.ChangeMode(DisplayMode.TagInsert);
            }
            else
            {
                this.ChangeMode(DisplayMode.Initialize);

                shtMeisai.AllowUserToAddRows = false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションコンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboZaikoTani.SelectedValue.ToString() == ZAIKO_TANI.PALLET_VALUE1 ||
                cboZaikoTani.SelectedValue.ToString() == ZAIKO_TANI.BOX_VALUE1)
            {
                if (cboLocation.SelectedIndex != -1)
                {
                    for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                    {
                        this.shtMeisai[COL_LOCATION, i].Text = cboLocation.SelectedValue.ToString();
                    }
                }
            }
        }

        #endregion

        #region フォーカスアウト

        /// --------------------------------------------------
        /// <summary>
        /// 在庫No.のフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtZaikoNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtZaikoNo.Text))
            {
                // 字埋め
                this.txtZaikoNo.Text = this.txtZaikoNo.Text.PadLeft(5, '0');
            }
        }

        #endregion

        #region キー操作による書込・削除禁止

        /// --------------------------------------------------
        /// <summary>
        /// キー操作による書込・削除禁止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/09/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || (e.Control && e.KeyCode == Keys.V))
            {
                e.Handled = true;
            }
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
            this.shtMeisai.AllowUserToAddRows = false;
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #endregion

        #region ロケーションコンボボックスセット

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションコンボボックスセット
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboLocation()
        {
            // 物件に紐付くロケーションをコンボボックスにセット
            ConnI01 conn = new ConnI01();
            CondI01 cond = new CondI01(this.UserInfo);
            cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            cond.BukkenNo = this.txtBukkenNo.Text;

            DataSet ds = conn.GetLocationCombo(cond);
            string tableName = Def_M_LOCATION.Name;

            DataTable dt = ds.Tables[tableName];
            if (dt.Rows.Count > 0)
                MakeCmbBox(this.cboLocation, dt, Def_M_LOCATION.LOCATION, Def_M_LOCATION.LOCATION, string.Empty, false);
            else
                this.cboLocation.DataSource = null;

            // シートのコンボボックスセット
            List<string> items = new List<string>();
            items.Add(string.Empty);
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(ComFunc.GetFld(dr, Def_M_LOCATION.LOCATION));
            }

            ComboBoxEditor cbo = ElTabelleSheetHelper.NewComboBoxEditor(false, items.ToArray());
            this.shtMeisai.Columns[COL_LOCATION].Editor = cbo;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Wakamatsu 2013/08/01</create>
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
                        this.cboZaikoTani.Enabled = true;
                        this.txtZaikoNo.Enabled = true;
                        this.btnDisp.Enabled = true;
                        // 2015/11/30 H.Tajimi 物件名は入力可
                        this.txtBukkenName.Enabled = true;
                        // ↑

                        this.cboLocation.Enabled = false;
                        this.dtpStockDate.Enabled = false;

                        this.shtMeisai.DataSource = null;
                        this.shtMeisai.MaxRows = 0;
                        this.shtMeisai.Enabled = false;
                        this.shtMeisai.AllowUserToAddRows = false;
 
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = false;
                        // ↑
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.cboShukkaFlag.Enabled = false;
                        this.btnBukkenIchiran.Enabled = false;
                        this.cboZaikoTani.Enabled = false;
                        this.txtZaikoNo.Enabled = false;
                        this.btnDisp.Enabled = false;
                        // 2015/11/30 H.Tajimi 物件名は入力不可
                        this.txtBukkenName.Enabled = false;
                        // ↑

                        this.cboLocation.Enabled = true;
                        this.dtpStockDate.Enabled = true;

                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = false;
                        for (int i = 0; i < 2; i++)
                        {
                            this.shtMeisai.Columns[i].Enabled = false;
                        }

                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.TagInsert:
                        // ----- TAG登録時 -----
                        this.cboShukkaFlag.Enabled = false;
                        this.btnBukkenIchiran.Enabled = false;
                        this.cboZaikoTani.Enabled = false;
                        this.txtZaikoNo.Enabled = false;
                        this.btnDisp.Enabled = false;
                        // 2015/11/30 H.Tajimi 物件名は入力不可
                        this.txtBukkenName.Enabled = false;
                        // ↑

                        this.cboLocation.Enabled = false;
                        this.dtpStockDate.Enabled = true;

                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        for (int i = 0; i < 2; i++)
                        {
                            this.shtMeisai.Columns[i].Enabled = true;
                        }

                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア時 -----
                        this.cboShukkaFlag.Enabled = true;
                        this.cboZaikoTani.Enabled = true;
                        this.txtZaikoNo.Enabled = true;
                        this.btnDisp.Enabled = true;
                        this.txtBukkenName.Enabled = true;

                        this.cboLocation.Enabled = false;
                        this.dtpStockDate.Enabled = false;

                        // シートのクリア
                        this.SheetClear();

                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
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
        /// <param name="isSearch">検索するかどうか</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowBukkenMeiIchiran(bool isSearch)
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
                    if (isSearch)
                    {
                        return this.RunSearch();
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #endregion

    }
}
