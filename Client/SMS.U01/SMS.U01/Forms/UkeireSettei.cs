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
using WsConnection.WebRefU01;
using SMS.U01.Properties;

namespace SMS.U01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 受入登録
    /// </summary>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class UkeireSettei : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        private bool _isShowShukkazumiMsgBox = true;

        #endregion

        #region 定数
        // 状態ボタンのカラム位置
        private const int COL_STATE_BUTTON = 0;
        // 詳細ボタンのカラム位置
        private const int COL_DETAIL_BUTTON = 1;
        // ダミーのカラム位置
        private const int COL_DETAIL_DUMMY = 2;
        // TAGNOのカラム位置
        private const int COL_TAG_NO = 3;
        // BOXNOのカラム位置
        private const int COL_BOX_NO = 4;
        // 数量のカラム位置
        private const int COL_NUM_NO = 16;
        // 備考対応
        private const int COL_BIKO = 17;
        // 通関確認状態
        private const int COL_CUSTOMS_STATUS = 18;
        // ARNo
        private const int COL_AR_NO = 20;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 6;

        /// --------------------------------------------------
        /// <summary>
        /// ＡＲ用プリフィックス
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string PREFIX_ARNO = "AR";

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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public UkeireSettei(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                this.EditMode = SystemBase.EditMode.Update;
                this.InitializeSheet(this.shtMeisai);
                this.MakeCmbBox(this.cboDispSelect, UKEIRE_TANI.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.UkeireSettei_State;
                shtMeisai.ColumnHeaders[3].Caption = Resources.UkeireSettei_TagNo;
                shtMeisai.ColumnHeaders[4].Caption = Resources.UkeireSettei_BoxNo;
                shtMeisai.ColumnHeaders[5].Caption = Resources.UkeireSettei_ProductNo;
                shtMeisai.ColumnHeaders[6].Caption = Resources.UkeireSettei_Code;
                shtMeisai.ColumnHeaders[7].Caption = Resources.UkeireSettei_DrawingAddirionalNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.UkeireSettei_Area;
                shtMeisai.ColumnHeaders[9].Caption = Resources.UkeireSettei_Floor;
                shtMeisai.ColumnHeaders[10].Caption = Resources.UkeireSettei_Model;
                shtMeisai.ColumnHeaders[11].Caption = Resources.UkeireSettei_MNo;
                shtMeisai.ColumnHeaders[12].Caption = Resources.UkeireSettei_JpName;
                shtMeisai.ColumnHeaders[13].Caption = Resources.UkeireSettei_Name;
                shtMeisai.ColumnHeaders[14].Caption = Resources.UkeireSettei_DrawingNoFormat;
                shtMeisai.ColumnHeaders[15].Caption = Resources.UkeireSettei_SectioningNo;
                shtMeisai.ColumnHeaders[16].Caption = Resources.UkeireSettei_Quantity;
                shtMeisai.ColumnHeaders[17].Caption = Resources.UkeireSettei_Memo;
                shtMeisai.ColumnHeaders[18].Caption = Resources.UkeireSettei_CustomsStatus;
                shtMeisai.ColumnHeaders[19].Caption = Resources.UkeireSettei_STNo;
                shtMeisai.ColumnHeaders[20].Caption = Resources.UkeireSettei_ARNo;

                this.dtpUkeireDate.Text = "";
                this.fbrFunction.F01Button.Enabled = false;
                // 2015/12/08 H.Tajimi クリアボタン
                this.fbrFunction.F06Button.Enabled = false;
                // ↑
                ChangeSheet();
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 検索時の出荷済みのメッセージボックスの表示
                this._isShowShukkazumiMsgBox = true;
                // 初期フォーカスの設定
                this.cboDispSelect.Focus();
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtNonyuusaki.Text = "";
                this.txtShip.Text = "";
                this.txtUkeireNo.Text = "";
                // 2015/12/08 H.Tajimi 全クリア
                // 直前の入力値保持する
                //this.dtpUkeireDate.Text = "";
                this.dtpUkeireDate.Text = "";
                // ↑

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
                this.shtMeisai.Enabled = false;
                // 2015/12/08 H.Tajimi 全クリア
                // 直前の入力値保持する
                //this.cboDispSelect.SelectedValue = UKEIRE_TANI.DEFAULT_VALUE1;
                this.cboDispSelect.SelectedValue = UKEIRE_TANI.DEFAULT_VALUE1;
                // ↑

                this.txtUkeireNo.Enabled = true;
                this.btnDisp.Enabled = true;
                this.cboDispSelect.Enabled = true;
                this.dtpUkeireDate.Enabled = false;
                this.btnAllNotCheck.Enabled = false;
                this.btnAllCheck.Enabled = false;
                this.fbrFunction.F01Button.Enabled = false;
                // 2015/12/08 H.Tajimi クリアボタン
                this.fbrFunction.F06Button.Enabled = false;
                // ↑
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtUkeireNo.Text))
                {
                    // 受入No.を入力して下さい。
                    this.ShowMessage("U0100010001");
                    this.txtUkeireNo.Focus();
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                DataSet ds;
                string tableName = string.Empty;
                string warningMsgID = string.Empty;
                string errMsgID = null;
                string[] args = null;
                ConnU01 conn = new ConnU01();
                CondU01 cond = new CondU01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_UKEIRE, ComDefine.BUTTON_TEXT_DETAIL);
                cond.UkeireNo = lblPrefix.Text + txtUkeireNo.Text;
                cond.UkeireDate = this.dtpUkeireDate.Value.ToShortDateString();


                if (cboDispSelect.SelectedValue.ToString() == UKEIRE_TANI.BOX_VALUE1)
                {
                    ds = conn.GetBoxData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                }
                else
                {
                    ds = conn.GetPalletData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                }

                txtNonyuusaki.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_DISP_NAME);
                txtShip.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_DISP_SHIP);

                if (!string.IsNullOrEmpty(errMsgID))
                {
                    // 出荷済のメッセージ
                    if (this._isShowShukkazumiMsgBox)
                    {
                        this.ShowMessage(errMsgID);
                    }
                }

                if (errMsgID == "" || errMsgID == "U0100010004" || errMsgID == "U0100010007")
                {
                    this.dtpUkeireDate.Enabled = true;
                    this.btnAllCheck.Enabled = true;
                    this.btnAllNotCheck.Enabled = true;
                    this.fbrFunction.F01Button.Enabled = true;
                    // 2015/12/08 H.Tajimi クリアボタン
                    this.fbrFunction.F06Button.Enabled = true;
                    // ↑
                }
                else
                {
                    if (ds == null || !ds.Tables.Contains(Def_T_SHUKKA_MEISAI.Name))
                    {
                        return false;
                    }

                    this.dtpUkeireDate.Enabled = false;
                    this.btnAllCheck.Enabled = false;
                    this.btnAllNotCheck.Enabled = false;
                    this.fbrFunction.F01Button.Enabled = false;
                    // 2015/12/08 H.Tajimi クリアボタン
                    this.fbrFunction.F06Button.Enabled = false;
                    // ↑
                }

                // 2011/03/10 K.Tsutsumi Change T_ARが存在しなくても続行可能
                //this.dtpUkeireDate.Value = ComFunc.GetFldToDateTime(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.UKEIRE_DATE, DateTime.Today);
                // 受入年月日は設定されていなければ入力値保持
                if (!string.IsNullOrEmpty(ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_UKEIRE_DATE)))
                {
                    this.dtpUkeireDate.Value = ComFunc.GetFldToDateTime(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_UKEIRE_DATE, DateTime.Today);
                }
                // ↑

                // デフォルトは受入を表示
                foreach (DataRow dr in ds.Tables[tableName].Rows)
                {
                    dr[ComDefine.FLD_BTN_STATE] = ComDefine.BUTTON_TEXT_UKEIRE;
                }

                this.shtMeisai.DataSource = ds.Tables[tableName];
                this.shtMeisai.Enabled = true;

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this._isShowShukkazumiMsgBox = true;
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                DataTable dt = null;
                string shukkaTani = this.cboDispSelect.SelectedValue.ToString();
                string errMsgID = null;
                string[] args = null;
                CondU01 cond = new CondU01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_UKEIRE, ComDefine.BUTTON_TEXT_DETAIL);
                ConnU01 conn = new ConnU01();
                bool ret = false;

                cond.UkeireDate = this.dtpUkeireDate.Value.ToShortDateString();
                cond.UkeireNo = this.lblPrefix.Text + this.txtUkeireNo.Text;

                dt = (this.shtMeisai.DataSource as DataTable).Copy();
                dt.AcceptChanges();
                ret = conn.UpdUkeireData(cond, dt, out errMsgID, out args);

                if (!string.IsNullOrEmpty(errMsgID))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this._isShowShukkazumiMsgBox = false;
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                this.txtNonyuusaki.Text = "";
                this.txtShip.Text = "";
                this.txtUkeireNo.Text = "";
                //2013/09/04 前回入力値を保持する
                //this.dtpUkeireDate.Text = "";
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;

                this.txtUkeireNo.Enabled = true;
                this.btnDisp.Enabled = true;
                this.cboDispSelect.Enabled = true;
                this.btnAllNotCheck.Enabled = false;
                this.btnAllCheck.Enabled = false;
                this.fbrFunction.F01Button.Enabled = false;
                // 2015/12/08 H.Tajimi クリアボタン
                this.fbrFunction.F06Button.Enabled = false;
                // ↑

                this.cboDispSelect.Focus();

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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// <create>H.Tsunamura 2010/08/20</create>
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
                this.txtNonyuusaki.Text = string.Empty;
                this.txtShip.Text = string.Empty;
                this.ChangeEnableViewMode(false);
                this.txtUkeireNo.Focus();
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
        /// <create>H.Tajimi 2015/12/08</create>
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
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();

                this.cboDispSelect.Focus();
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
        /// <create>H.Tsunamura 2010/08/20</create>
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

        /// --------------------------------------------------
        /// <summary>
        /// 受入単位の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboDispSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ClearMessage();
            this.txtUkeireNo.Text = "";
            if (this.cboDispSelect.SelectedValue.ToString() == UKEIRE_TANI.BOX_VALUE1)
            {
                lblPrefix.Text = ComDefine.PREFIX_BOXNO;
            }
            else
            {
                lblPrefix.Text = ComDefine.PREFIX_PALLETNO;
            }
            ChangeSheet();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 受入Noのフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtUkeireNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtUkeireNo.Text))
            {
                // 字埋め
                this.txtUkeireNo.Text = this.txtUkeireNo.Text.PadLeft(5, '0');
            }
        }

        #region グリッドクリック
        /// --------------------------------------------------
        /// <summary>
        /// グリッドクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                if ((this.shtMeisai.CellEditor as ButtonEditor) != null)
                {
                    //セルのイベント処理です
                    switch (e.Name)
                    {
                        case CellNotifyEvents.Click:
                            switch (e.Position.Column)
                            {
                                case COL_DETAIL_BUTTON:
                                    // 詳細ボタン
                                    if (this.cboDispSelect.SelectedValue.ToString() != SHUKKA_TANI.PALLET_VALUE1) return;
                                    DataTable dt = this.shtMeisai.DataSource as DataTable;
                                    if (dt == null || dt.Rows.Count <= e.Position.Row) return;
                                    string boxNo = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.BOX_NO);
                                    string shukkaFlag = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                                    string nonyusakiCD = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                                    string nonyusakiName = this.txtNonyuusaki.Text;
                                    string ship = this.txtShip.Text;
                                    string arNo = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.AR_NO);

                                    using (ShinchokuToiawaseMeisai frm = new ShinchokuToiawaseMeisai(this.UserInfo, shukkaFlag, nonyusakiCD, nonyusakiName, ship, boxNo))
                                    {
                                        frm.Icon = this.Icon;
                                        frm.ShowDialog();
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region グリッドの切り替え
        /// --------------------------------------------------
        /// <summary>
        /// グリッドの切り替え
        /// </summary>
        /// <create>H.Tsunamura 2010/08/06</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet()
        {
            try
            {
                this.shtMeisai.Redraw = false;
                string shukkaTani = this.cboDispSelect.SelectedValue.ToString();
                if (shukkaTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box出荷 -----
                    this.shtMeisai.Columns[COL_DETAIL_BUTTON].Hidden = true;
                    this.shtMeisai.Columns[COL_DETAIL_DUMMY].Hidden = false;
                    this.shtMeisai.Columns[COL_BOX_NO].Hidden = true;
                    this.shtMeisai.Columns[COL_TAG_NO].Hidden = false;
                    this.shtMeisai.Columns[COL_NUM_NO].Hidden = false;
                    this.shtMeisai.Columns[COL_BIKO].Hidden = false;
                    this.shtMeisai.Columns[COL_CUSTOMS_STATUS].Hidden = false;
                    this.shtMeisai.Columns[COL_AR_NO].Hidden = false;
                }
                else if (shukkaTani == SHUKKA_TANI.PALLET_VALUE1)
                {
                    // ----- パレット出荷 -----
                    this.shtMeisai.Columns[COL_DETAIL_BUTTON].Hidden = false;
                    this.shtMeisai.Columns[COL_DETAIL_DUMMY].Hidden = true;
                    this.shtMeisai.Columns[COL_TAG_NO].Hidden = true;
                    this.shtMeisai.Columns[COL_BOX_NO].Hidden = false;
                    this.shtMeisai.Columns[COL_NUM_NO].Hidden = true;
                    this.shtMeisai.Columns[COL_BIKO].Hidden = true;
                    this.shtMeisai.Columns[COL_CUSTOMS_STATUS].Hidden = true;
                    this.shtMeisai.Columns[COL_AR_NO].Hidden = true;
                }
                this.shtMeisai.FreezeColumns = 6;

                this.InitializeSheet(this.shtMeisai);
                this.shtMeisai.ClipMode = DataTransferMode.Custom;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
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
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            if (string.IsNullOrEmpty(this.txtUkeireNo.Text))
            {
                // 受入No.を入力して下さい。
                this.ShowMessage("U0100010001");
                return;
            }

            if (!this.RunSearch())
            {
                this.txtUkeireNo.Focus();
                return;
            }
            // 成功した場合「受入単位」「受入No.」「表示」を操作不可
            this.cboDispSelect.Enabled = false;
            this.txtUkeireNo.Enabled = false;
            this.btnDisp.Enabled = false;
            // 成功した場合は「受入年月日」を使用可
            this.dtpUkeireDate.Enabled = true;
            
            

        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.shtMeisai.Redraw = false;
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (this.shtMeisai[COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_SHUKKA)
                    {
                        this.shtMeisai[COL_STATE_BUTTON, i].Value = ComDefine.BUTTON_TEXT_UKEIRE;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/08/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.shtMeisai.Redraw = false;
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (this.shtMeisai[COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_UKEIRE)
                    {
                        this.shtMeisai[COL_STATE_BUTTON, i].Value = ComDefine.BUTTON_TEXT_SHUKKA;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }
        #endregion 

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>H.Tajimi 2015/12/08</create>
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

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">Enabled状態</param>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 表示コントロールのロック/ロック解除
            this.btnAllCheck.Enabled = isView;
            this.btnAllNotCheck.Enabled = isView;
            this.dtpUkeireDate.Enabled = isView;
            // 検索条件のロック/ロック解除
            this.cboDispSelect.Enabled = !isView;
            this.txtUkeireNo.Enabled = !isView;
            this.btnDisp.Enabled = !isView;
            // ファンクションボタン制御
            this.fbrFunction.F01Button.Enabled = isView;
            this.fbrFunction.F06Button.Enabled = isView;
        }

        #endregion
    }
}
