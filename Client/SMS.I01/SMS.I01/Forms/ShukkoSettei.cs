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
    /// 出庫設定
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkoSettei : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/12</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後（在庫単位はB or P）
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/12</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// Tag登録時
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/12</create>
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
        // TagNoのカラム位置
        private const int COL_TAG_NO = 38;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 5;

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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkoSettei(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
                this.EditMode = SystemBase.EditMode.Update;
                this.InitializeSheet(this.shtMeisai);
                this.MakeCmbBox(this.cboKanryoTani, KANRYO_TANI.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.ShukkoSettei_TagCode;
                shtMeisai.ColumnHeaders[1].Caption = Resources.ShukkoSettei_StockLocation;
                shtMeisai.ColumnHeaders[2].Caption = Resources.ShukkoSettei_Status;
                shtMeisai.ColumnHeaders[3].Caption = Resources.ShukkoSettei_State;
                shtMeisai.ColumnHeaders[4].Caption = Resources.ShukkoSettei_Date;
                shtMeisai.ColumnHeaders[5].Caption = Resources.ShukkoSettei_ProductNo;
                shtMeisai.ColumnHeaders[6].Caption = Resources.ShukkoSettei_Code;
                shtMeisai.ColumnHeaders[7].Caption = Resources.ShukkoSettei_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.ShukkoSettei_Name;
                shtMeisai.ColumnHeaders[9].Caption = Resources.ShukkoSettei_DrawingNoFormat;
                shtMeisai.ColumnHeaders[10].Caption = Resources.ShukkoSettei_Quantity;
                shtMeisai.ColumnHeaders[11].Caption = Resources.ShukkoSettei_JpName;
                shtMeisai.ColumnHeaders[12].Caption = Resources.ShukkoSettei_SectioningNo;
                shtMeisai.ColumnHeaders[13].Caption = Resources.ShukkoSettei_Area;
                shtMeisai.ColumnHeaders[14].Caption = Resources.ShukkoSettei_Floor;
                shtMeisai.ColumnHeaders[15].Caption = Resources.ShukkoSettei_Model;
                shtMeisai.ColumnHeaders[16].Caption = Resources.ShukkoSettei_MNo;
                shtMeisai.ColumnHeaders[17].Caption = Resources.ShukkoSettei_DeliveryDestination;
                shtMeisai.ColumnHeaders[18].Caption = Resources.ShukkoSettei_Ship;
                shtMeisai.ColumnHeaders[19].Caption = Resources.ShukkoSettei_STNo;
                shtMeisai.ColumnHeaders[20].Caption = Resources.ShukkoSettei_Memo;
                shtMeisai.ColumnHeaders[21].Caption = Resources.ShukkoSettei_ARNo;
                shtMeisai.ColumnHeaders[22].Caption = Resources.ShukkoSettei_AssemblyDate;
                shtMeisai.ColumnHeaders[23].Caption = Resources.ShukkoSettei_BoxNo;
                shtMeisai.ColumnHeaders[24].Caption = Resources.ShukkoSettei_PalletNo;
                shtMeisai.ColumnHeaders[25].Caption = Resources.ShukkoSettei_ConstructionIdentityManagementNo;
                shtMeisai.ColumnHeaders[26].Caption = Resources.ShukkoSettei_InternalManagementKey;
                shtMeisai.ColumnHeaders[27].Caption = Resources.ShukkoSettei_WoodFrameNo;
                shtMeisai.ColumnHeaders[28].Caption = Resources.ShukkoSettei_BoxPackingDate;
                shtMeisai.ColumnHeaders[29].Caption = Resources.ShukkoSettei_PalletPackingDate;
                shtMeisai.ColumnHeaders[30].Caption = Resources.ShukkoSettei_WoodFrameDate;
                shtMeisai.ColumnHeaders[31].Caption = Resources.ShukkoSettei_ShipDate;
                shtMeisai.ColumnHeaders[32].Caption = Resources.ShukkoSettei_ShippingCompany;
                shtMeisai.ColumnHeaders[33].Caption = Resources.ShukkoSettei_InvoiceNo;
                shtMeisai.ColumnHeaders[34].Caption = Resources.ShukkoSettei_OkurijyoNo;
                shtMeisai.ColumnHeaders[35].Caption = Resources.ShukkoSettei_BLNo;
                shtMeisai.ColumnHeaders[36].Caption = Resources.ShukkoSettei_AcceptanceDate;
                shtMeisai.ColumnHeaders[37].Caption = Resources.ShukkoSettei_AcceptanceStaff;

                this.txtKanryoNo.Text = string.Empty;

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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboKanryoTani.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>T.Wakamatsu 2013/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                KeyAction[] aryKeyActions = new KeyAction[1];
                aryKeyActions[0] = KeyAction.NextRow;
                shtMeisai.ShortCuts.Add(Keys.Enter, aryKeyActions);
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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtKanryoNo.Text = string.Empty;

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

                if (this.cboKanryoTani.Items.Count > 0 && this.cboKanryoTani.SelectedValue.ToString() == ZAIKO_TANI.TAG_VALUE1)
                {
                    cboKanryoTani.SelectedValue = ZAIKO_TANI.BOX_VALUE1;
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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.lblKanryoNoPrefix.Text == ZAIKO_TANI.BOX_VALUE1)
                {
                    return true;
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
                    {                                // 納入先コードは数値
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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtKanryoNo.Text))
                {
                    // 在庫No.を入力して下さい。
                    this.ShowMessage("I0100020006");
                    this.txtKanryoNo.Focus();
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                //データ取得
                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ZaikoNo = lblKanryoNoPrefix.Text + txtKanryoNo.Text;


                DataSet ds = conn.GetKanryoData(cond);
                string tableName = Def_T_STOCK.Name;

                DataTable dt = ds.Tables[tableName];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    this.ShowMessage("I0100030001");
                    txtKanryoNo.Focus();
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                bool ret = false;

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ZaikoNo = lblKanryoNoPrefix.Text + txtKanryoNo.Text;
                cond.StockDate = this.dtpKanryoDate.Value.ToShortDateString();
                cond.WorkUserID = cond.LoginInfo.UserID;

                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                dt.AcceptChanges();

                string errMsgID;
                string[] args;
                ret = conn.DelKanryoData(cond, dt, out errMsgID, out args);
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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

                if (this.cboKanryoTani.Items.Count > 0 && this.cboKanryoTani.SelectedValue.ToString() == ZAIKO_TANI.TAG_VALUE1)
                {
                    cboKanryoTani.SelectedValue = ZAIKO_TANI.BOX_VALUE1;
                }
                else
                {
                    this.ChangeMode(DisplayMode.ResultClear);
                    this.txtKanryoNo.Focus();
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

                this.cboKanryoTani.Focus();
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
        /// <create>T.Wakamatsu 2013/08/12</create>
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
                                cond.ZaikoNo = str;


                                DataSet ds = conn.GetKanryoData(cond);
                                string tableName = Def_T_STOCK.Name;

                                DataTable dt = ds.Tables[tableName];

                                // 対象データがない場合はメッセージ
                                if (dt.Rows.Count == 0)
                                {
                                    // 該当TagCodeはありません。
                                    this.ShowMessage("I0100030002");

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
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            if (!this.RunSearch())
            {
                return;
            }

            // 成功した場合検索部を操作不可に
            this.ChangeMode(DisplayMode.EndSearch);
        }

        #endregion 

        #region コンボボックス変更

        /// --------------------------------------------------
        /// <summary>
        /// 完了単位コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboKanryoTani_SelectedIndexChanged(object sender, EventArgs e)
        {
            string zaikoTani = cboKanryoTani.SelectedValue.ToString();

            lblKanryoNoPrefix.Text = zaikoTani;
            txtKanryoNo.Text = string.Empty;

            if (zaikoTani == ZAIKO_TANI.TAG_VALUE1)
            {
                this.ChangeMode(DisplayMode.TagInsert);
            }
            else
            {
                this.ChangeMode(DisplayMode.Initialize);

                shtMeisai.AllowUserToAddRows = false;
            }
        }

        #endregion

        #region フォーカスアウト

        /// --------------------------------------------------
        /// <summary>
        /// 完了No.のフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtKanryoNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtKanryoNo.Text))
            {
                // 字埋め
                this.txtKanryoNo.Text = this.txtKanryoNo.Text.PadLeft(5, '0');
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

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Wakamatsu 2013/08/12</create>
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
                        this.cboKanryoTani.Enabled = true;
                        this.txtKanryoNo.Enabled = true;
                        this.btnDisp.Enabled = true;

                        this.dtpKanryoDate.Enabled = false;

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
                        this.cboKanryoTani.Enabled = false;
                        this.txtKanryoNo.Enabled = false;
                        this.btnDisp.Enabled = false;

                        this.dtpKanryoDate.Enabled = true;

                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = false;
                        this.shtMeisai.Columns[COL_TAG_CODE].Enabled = false;

                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.TagInsert:
                        // ----- TAG登録時 -----
                        this.cboKanryoTani.Enabled = false;
                        this.txtKanryoNo.Enabled = false;
                        this.btnDisp.Enabled = false;

                        this.dtpKanryoDate.Enabled = true;

                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        this.shtMeisai.Columns[COL_TAG_CODE].Enabled = true;

                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア時 -----
                        this.cboKanryoTani.Enabled = true;
                        this.txtKanryoNo.Enabled = true;
                        this.btnDisp.Enabled = true;

                        this.dtpKanryoDate.Enabled = false;

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

    }
}
