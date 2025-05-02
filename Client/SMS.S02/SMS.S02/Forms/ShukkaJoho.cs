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
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using WsConnection.WebRefS02;
using SMS.P02.Forms;
using MultiRowTabelle;
using SMS.S02.Properties;

namespace SMS.S02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷情報登録
    /// </summary>
    /// <create>Y.Higuchi 2010/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaJoho : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// グリッドに追加する列のタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ElTabelleColumnType
        {
            Text = 0,
            Number = 1,
            Button = 2,
        }

        #endregion

        #region Fields

        private MultiRowFormat _multiRowFormat = null;
        private bool _isShowShukkazumiMsgBox = true;
        private string _firstDisplayState = string.Empty;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 「出荷」「梱包済」ボタン
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COL_STATE_BUTTON = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタン
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COL_DETAIL_BUTTON = 1;

        // 2012/05/17 K.Tsutsumi Delete 不要
        ///// --------------------------------------------------
        ///// <summary>
        ///// ＡＲ用プリフィックス
        ///// </summary>
        ///// <create>K.Tsutsumi 2011/03/10</create>
        ///// <update></update>
        ///// --------------------------------------------------
        //private const string PREFIX_ARNO = "AR";
        // ↑

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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaJoho(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // 2011/03/10 K.Tsutsumi Add 複数個口対応
                // 確認メッセージとクリア処理は画面側で制御する
                this.MsgUpdateEnd = string.Empty;
                this.IsRunEditAfterClear = false;
                // ↑

                // 検索時の出荷済みのメッセージボックスの表示
                this._isShowShukkazumiMsgBox = true;
                // フォームの設定
                this.IsCloseQuestion = true;
                // 更新処理固定
                this.EditMode = SystemBase.EditMode.Update;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboShukkaTani, SHUKKA_TANI.GROUPCD);

                // 出荷年月日
                this.dtpShukkaDate.Value = DateTime.Today;

                // 木枠の複数行表示のフォーマット
                this._multiRowFormat = new MultiRowFormat();
                this._multiRowFormat.SetRowNum(2);
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Button, 0, 0, 1, 2, ComDefine.FLD_BTN_STATE, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 1, 0, 1, 2, string.Empty, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 2, 0, 1, 2, ComDefine.FLD_SHIP_CNO, false, AlignHorizontal.Right, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 3, 0, 1, 2, Def_T_KIWAKU_MEISAI.STYLE, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 4, 0, 1, 2, Def_T_KIWAKU_MEISAI.ITEM, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 5, 0, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 5, 1, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 6, 0, 1, 2, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 7, 0, 1, 2, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 8, 0, 1, 2, Def_M_NONYUSAKI.NONYUSAKI_NAME, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 9, 0, 1, 2, Def_M_NONYUSAKI.SHIP, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 10, 0, 1, 2, Def_T_AR.AR_NO, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 11, 0, 1, 2, Def_T_KIWAKU.KOJI_NO, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 12, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_ID, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 13, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_NO, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 14, 0, 1, 2, Def_T_KIWAKU_MEISAI.SHUKKA_DATE, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));
                this.SetFormat(this._multiRowFormat, ElTabelleColumnType.Text, 15, 0, 1, 2, Def_T_KIWAKU_MEISAI.VERSION, false, AlignHorizontal.NotSet, MultiRowCell.GetEnterKeyActionArray(MultiRowCell.DefaultEnterKeyActions));

                // モード切替
                this.ChangeMode();
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboShukkaTani.Focus();
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
        /// 画面クリア処理（※全クリア固定）
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            this.DisplayClear(false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <param name="isContinue">true:運送会社名～ＢＬＮｏはクリアしない false:全てクリアする</param>
        /// <create>K.Tsutsumi 2011/03/10</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/03/10 K.Tsutsumi Change 複数個口対応
        //protected override void DisplayClear()
        protected virtual void DisplayClear(bool isContinue)
        // ↑
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();
                // 2013/09/04 T.Wakamatsu 出荷単位と出年月日は入力値を保持する
                // 出荷単位
                //if (0 < this.cboShukkaTani.Items.Count)
                //{
                //    this.cboShukkaTani.SelectedValue = SHUKKA_TANI.DEFAULT_VALUE1;
                //}
                //else
                //{
                //    this.cboShukkaTani.SelectedIndex = -1;
                //}
                // 出荷年月日
                //this.dtpShukkaDate.Value = DateTime.Today;
                // 出荷No.
                this.txtShukkaNo.Text = string.Empty;
                // 納入先
                this.txtNonyusakiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;

                if (isContinue == false)
                {
                    // 2015/12/08 H.Tajimi 全クリア
                    // 出荷単位
                    if (0 < this.cboShukkaTani.Items.Count)
                    {
                        this.cboShukkaTani.SelectedValue = SHUKKA_TANI.DEFAULT_VALUE1;
                    }
                    else
                    {
                        this.cboShukkaTani.SelectedIndex = -1;
                    }
                    // 出荷年月日
                    this.dtpShukkaDate.Value = DateTime.Today;
                    // ↑
                    // 運送会社
                    this.txtUnsokaishaName.Text = string.Empty;
                    // インボイスNo.
                    this.txtInvoiceNo.Text = string.Empty;
                    // 送り状No.
                    this.txtOkurijyoNo.Text = string.Empty;
                    // BLNo.
                    this.txtBLNo.Text = string.Empty;
                }
                // ↑

                // 表示クリック前の状態にする。
                this.ChangeEnableViewMode(false);
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
        /// <create>Y.Higuchi 2010/07/20</create>
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtShukkaNo.Text))
                {
                    // 出荷No.を入力して下さい。
                    this.ShowMessage("S0200010001");
                    this.txtShukkaNo.Focus();
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
        /// <create>Y.Higuchi 2010/07/20</create>
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                string tableName = string.Empty;
                string warningMsgID = string.Empty;
                string shukkaTani = this.cboShukkaTani.SelectedValue.ToString();
                string errMsgID = null;
                string[] args = null;
                DataSet ds;
                CondS02 cond = new CondS02(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_DETAIL);
                ConnS02 conn = new ConnS02();
                cond.ShukkaNo = this.lblShukka.Text + this.txtShukkaNo.Text;
                if (shukkaTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box出荷 -----
                    ds = conn.GetBoxData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                    // 出荷済BoxNo.です。
                    warningMsgID = "S0200010006";

                }
                else if (shukkaTani == SHUKKA_TANI.PALLET_VALUE1)
                {
                    // ----- パレット出荷 -----
                    ds = conn.GetPalletData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                    // 出荷済パレットNo.です。
                    warningMsgID = "S0200010009";
                }
                else
                {
                    // ----- 木枠出荷 -----
                    ds = conn.GetKiwakuData(cond, out errMsgID, out args);
                    tableName = Def_T_KIWAKU_MEISAI.Name;
                    // 出荷済木枠梱包No.です。
                    warningMsgID = "S0200010011";
                }

                // ----- 共通処理 -----
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                if (!ComFunc.IsExistsData(ds, tableName))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                // データの設定
                if (!string.IsNullOrEmpty(ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_SHUKKA_DATE)))
                {
                    // 出荷済のメッセージ
                    if (this._isShowShukkazumiMsgBox)
                    {
                        this.ShowMessage(warningMsgID);
                    }
                    this.dtpShukkaDate.Value = ComFunc.GetFldToDateTime(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_SHUKKA_DATE, DateTime.Today);
                }
                // 出荷年月日は入力値を保持する
                //else
                //{
                //    this.dtpShukkaDate.Value = DateTime.Today;
                //}
                this.txtNonyusakiName.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_DISP_NAME);
                this.txtShip.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_DISP_SHIP);

                if ((string.IsNullOrEmpty(this.txtUnsokaishaName.Text) == true) && (string.IsNullOrEmpty(this.txtInvoiceNo.Text) == true) && (string.IsNullOrEmpty(this.txtOkurijyoNo.Text) == true) && (string.IsNullOrEmpty(this.txtBLNo.Text) == true))
                {
                    // 前回の情報が全てクリアされていたときは、ＤＢの値を表示する
                    this.txtUnsokaishaName.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_UNSOKAISHA);
                    this.txtInvoiceNo.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_INVOICE_NO);
                    this.txtOkurijyoNo.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_OKURIJYO_NO);
                    this.txtBLNo.Text = ComFunc.GetFld(ds, ComDefine.DTTBL_ADDITION, 0, ComDefine.FLD_ADDITION_BL_NO);
                }
                // ↑

                this._firstDisplayState = ComFunc.GetFld(ds, tableName, 0, ComDefine.FLD_BTN_STATE);

                // デフォルトは出荷を表示
                foreach (DataRow dr in ds.Tables[tableName].Rows)
                {
                    dr[ComDefine.FLD_BTN_STATE] = ComDefine.BUTTON_TEXT_SHUKKA;
                }

                // グリッドのバインド
                this.shtMeisai.Redraw = false;
                this.shtMeisai.Enabled = true;
                if (shukkaTani == SHUKKA_TANI.KIWAKU_VALUE1)
                {
                    // ----- 木枠出荷 -----
                    this.shtMeisai.SetMultiRowDataSource(ds.Tables[tableName], this._multiRowFormat);
                }
                else
                {
                    // ----- Box出荷、パレット出荷 -----
                    this.shtMeisai.DataSource = ds.Tables[tableName];
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                this.SheetClear();
                return false;
            }
            finally
            {
                this._isShowShukkazumiMsgBox = true;
                this.shtMeisai.Redraw = true;
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 2011/03/10 K.Tsutsumi Add 複数個口対応
                    bool isContinue = false;
                    if (ComFunc.GetFld((this.shtMeisai.DataSource as DataTable), 0, ComDefine.FLD_BTN_STATE) == ComDefine.BUTTON_TEXT_SHUKKA 
                        && (!string.IsNullOrEmpty(this.txtUnsokaishaName.Text) 
                        || !string.IsNullOrEmpty(this.txtInvoiceNo.Text) 
                        || !string.IsNullOrEmpty(this.txtOkurijyoNo.Text) 
                        || !string.IsNullOrEmpty(this.txtBLNo.Text)))
                    {
                        // 続けて同じ個口の入力をしますか？
                        if (this.ShowMessage("S0200010014") == DialogResult.Yes)
                        {
                            isContinue = true;
                        }
                    }

                    this.DisplayClear(isContinue);
                    // 更新しました。
                    this.ShowMessage("FW010040004");
                    // ↑
                    this.cboShukkaTani.Focus();
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
        /// <create>Y.Higuchi 2010/07/20</create>
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                DataTable dt = null;
                string shukkaTani = this.cboShukkaTani.SelectedValue.ToString();
                string errMsgID = null;
                string[] args = null;
                CondS02 cond = new CondS02(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_DETAIL);
                ConnS02 conn = new ConnS02();
                bool ret = false;

                cond.ShukkaDate = this.dtpShukkaDate.Value;
                cond.ShukkaUserID = this.UserInfo.UserID;
                cond.ShukkaUserName = this.UserInfo.UserName;
                cond.ShukkaNo = this.lblShukka.Text + this.txtShukkaNo.Text;

                if (!string.IsNullOrEmpty(this.txtUnsokaishaName.Text))
                {
                    cond.UnsokaishaName = this.txtUnsokaishaName.Text;
                }
                if (!string.IsNullOrEmpty(this.txtInvoiceNo.Text))
                {
                    cond.InvoiceNo = this.txtInvoiceNo.Text;
                }
                if (!string.IsNullOrEmpty(this.txtOkurijyoNo.Text))
                {
                    cond.OkurijyoNo = this.txtOkurijyoNo.Text;
                }
                if (!string.IsNullOrEmpty(this.txtBLNo.Text))
                {
                    cond.BLNo = this.txtBLNo.Text;
                }

                DataTable dtTemp = null;
                if (shukkaTani == SHUKKA_TANI.KIWAKU_VALUE1)
                {
                    dtTemp = this.shtMeisai.GetMultiRowGetDataSource();
                }
                else
                {
                    dtTemp = (DataTable)this.shtMeisai.DataSource;
                }
                if (this._firstDisplayState == ComDefine.BUTTON_TEXT_KONPOZUMI
                    && ComFunc.GetFld(dtTemp, 0, ComDefine.FLD_BTN_STATE) == ComDefine.BUTTON_TEXT_SHUKKA)
                {
                    cond.IsKonpo2Shukka = true;
                }
                if (this._firstDisplayState == ComDefine.BUTTON_TEXT_SHUKKA
                    && ComFunc.GetFld(dtTemp, 0, ComDefine.FLD_BTN_STATE) == ComDefine.BUTTON_TEXT_SHUKKA)
                {
                    cond.IsShukka2Shukka = true;
                }
                // ↑

                if (shukkaTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box出荷 -----
                    dt = (this.shtMeisai.DataSource as DataTable).Copy();
                    dt.AcceptChanges();
                    ret = conn.UpdBoxData(cond, dt, out errMsgID, out args);
                }
                else if (shukkaTani == SHUKKA_TANI.PALLET_VALUE1)
                {
                    // ----- パレット出荷 -----
                    dt = (this.shtMeisai.DataSource as DataTable).Copy();
                    dt.AcceptChanges();
                    ret = conn.UpdPalletData(cond, dt, out errMsgID, out args);
                }
                else
                {
                    // ----- 木枠出荷 -----
                    dt = this.shtMeisai.GetMultiRowGetDataSource();
                    dt.AcceptChanges();
                    ret = conn.UpdKiwakuData(cond, dt, out errMsgID, out args);
                }

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
        /// <create>Y.Higuchi 2010/07/20</create>
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
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
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
                // 納入先
                this.txtNonyusakiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
                // 運送会社
                this.txtUnsokaishaName.Text = string.Empty;
                // インボイスNo.
                this.txtInvoiceNo.Text = string.Empty;
                // 送り状No.
                this.txtOkurijyoNo.Text = string.Empty;
                // BLNo.
                this.txtBLNo.Text = string.Empty;
                // 表示クリック前の状態にする。
                this.ChangeEnableViewMode(false);
                this.txtShukkaNo.Focus();
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
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboShukkaTani.Focus();
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
        /// <create>Y.Higuchi 2010/07/20</create>
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

        #region 出荷単位

        /// --------------------------------------------------
        /// <summary>
        /// 出荷単位の選択インデックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaTani_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ClearMessage();
            this.ChangeMode();
        }

        #endregion

        #region 一覧選択

        /// --------------------------------------------------
        /// <summary>
        /// 一覧選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnListSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                string torokuFlag = string.Empty;
                string kojiName = string.Empty;
                string ship = string.Empty;
                using (KojiShikibetsuIchiran frm = new KojiShikibetsuIchiran(this.UserInfo, torokuFlag, kojiName, ship))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        DataRow dr = frm.SelectedRowData;
                        if (dr == null) return;
                        // 選択データを設定
                        this.txtShukkaNo.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                        // フォーカス移動
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 表示

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // ----- 表示 -----
                if (!this.RunSearch())
                {
                    this.txtShukkaNo.Focus();
                    return;
                }
                // モード切り替え
                this.ChangeEnableViewMode(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 全選択

        /// --------------------------------------------------
        /// <summary>
        /// 全選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/22</create>
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
                    if (this.shtMeisai[COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_KONPOZUMI)
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

        #region 全選択解除

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/22</create>
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
                    if (this.shtMeisai[COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_SHUKKA)
                    {
                        this.shtMeisai[COL_STATE_BUTTON, i].Value = ComDefine.BUTTON_TEXT_KONPOZUMI;
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

        #region グリッド

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// セルのイベントが発生したときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/23</create>
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
                                    if (this.cboShukkaTani.SelectedValue.ToString() != SHUKKA_TANI.PALLET_VALUE1) return;
                                    DataTable dt = this.shtMeisai.DataSource as DataTable;
                                    if (dt == null || dt.Rows.Count <= e.Position.Row) return;
                                    string boxNo = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.BOX_NO);
                                    string shukkaFlag = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                                    string nonyusakiCD = ComFunc.GetFld(dt, e.Position.Row, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                                    string nonyusakiName = this.txtNonyusakiName.Text;
                                    string ship = this.txtShip.Text;

                                    using (ShukkaJohoMeisai frm = new ShukkaJohoMeisai(this.UserInfo, shukkaFlag, nonyusakiCD, nonyusakiName, ship, boxNo))
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

        #endregion

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// モード変更時の切り替え処理
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                string shukkaTani = this.cboShukkaTani.SelectedValue.ToString();
                if (shukkaTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box出荷 -----
                    // 入力コントロールの切り替え
                    this.lblNonyusakiName.Text = ComDefine.LABEL_CAPTION_NONYUSAKI;
                    this.lblShukka.Text = ComDefine.PREFIX_BOXNO;
                    this.txtShukkaNo.Text = string.Empty;
                    this.txtShukkaNo.AutoPad = true;
                    this.txtShukkaNo.MaxLength = 5;
                    // ボタンの切り替え
                    this.btnListSelect.Enabled = false;
                }
                else if (shukkaTani == SHUKKA_TANI.PALLET_VALUE1)
                {
                    // ----- パレット出荷 -----
                    // 入力コントロールの切り替え
                    this.lblNonyusakiName.Text = ComDefine.LABEL_CAPTION_NONYUSAKI;
                    this.lblShukka.Text = ComDefine.PREFIX_PALLETNO;
                    this.txtShukkaNo.Text = string.Empty;
                    this.txtShukkaNo.AutoPad = true;
                    this.txtShukkaNo.MaxLength = 5;
                    // ボタンの切り替え
                    this.btnListSelect.Enabled = false;
                }
                else
                {
                    // ----- 木枠出荷 -----
                    // 入力コントロールの切り替え
                    this.lblNonyusakiName.Text = ComDefine.LABEL_CAPTION_KOJI_NAME;
                    this.lblShukka.Text = string.Empty;
                    this.txtShukkaNo.Text = string.Empty;
                    this.txtShukkaNo.AutoPad = false;
                    this.txtShukkaNo.MaxLength = 4;
                    // ボタンの切り替え
                    this.btnListSelect.Enabled = true;
                }
                // グリッドの切替
                this.ChangeSheet();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">表示状態かどうか</param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 出荷年月日
            this.dtpShukkaDate.Enabled = isView;
            // 納入先
            this.txtNonyusakiName.Enabled = isView;
            // 出荷便
            this.txtShip.Enabled = isView;
            // 運送会社
            this.txtUnsokaishaName.Enabled = isView;
            // インボイスNo.
            this.txtInvoiceNo.Enabled = isView;
            // 送り状No.
            this.txtOkurijyoNo.Enabled = isView;
            // BLNo.
            this.txtBLNo.Enabled = isView;
            // 全選択
            this.btnAllSelect.Enabled = isView;
            // 全選択解除
            this.btnAllDeselect.Enabled = isView;
            // 検索条件のロック解除
            this.grpSearch.Enabled = !isView;
            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
            // 2015/12/08 H.Tajimi クリアボタン制御
            // Clearボタン
            this.fbrFunction.F06Button.Enabled = isView;
            // ↑
        }

        #region グリッドの切り替え

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの切り替え
        /// </summary>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet()
        {
            try
            {
                this.shtMeisai.Redraw = false;
                this.shtMeisai.MaxColumns = 0;
                string shukkaTani = this.cboShukkaTani.SelectedValue.ToString();
                if (shukkaTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box出荷 -----
                    this.ChangeSheet_Box();
                }
                else if (shukkaTani == SHUKKA_TANI.PALLET_VALUE1)
                {
                    // ----- パレット出荷 -----
                    this.ChangeSheet_Pallet();
                }
                else
                {
                    // ----- 木枠出荷 -----
                    this.ChangeSheet_Kiwaku();
                }
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

        /// --------------------------------------------------
        /// <summary>
        /// Box出荷のグリッド構築
        /// </summary>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet_Box()
        {
            int colIndex;
            colIndex = 0;
            this.SetElTabelleColumn(ElTabelleColumnType.Button, colIndex, Resources.ShukkaJoho_ShukkaGridState, ComDefine.FLD_BTN_STATE, 56, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, string.Empty, string.Empty, 20, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridTagNo, Def_T_SHUKKA_MEISAI.TAG_NO, 40, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridSerialNumber, Def_T_SHUKKA_MEISAI.SEIBAN, 80, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridCode, Def_T_SHUKKA_MEISAI.CODE, 30, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDrawingSequence, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, 37, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridArea, Def_T_SHUKKA_MEISAI.AREA, 55, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridFloor, Def_T_SHUKKA_MEISAI.FLOOR, 55, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridModel, Def_T_SHUKKA_MEISAI.KISHU, 35, false);
            colIndex++;
            // 2015/12/09 H.Tajimi M-No.対応
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridMNo, Def_T_SHUKKA_MEISAI.M_NO, 57, false);
            colIndex++;
            // ↑
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridJPName, Def_T_SHUKKA_MEISAI.HINMEI_JP, 160, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridName, Def_T_SHUKKA_MEISAI.HINMEI, 150, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDNumFormat, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, 40, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDivisionNum, Def_T_SHUKKA_MEISAI.KUWARI_NO, 30, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Number, colIndex, Resources.ShukkaJoho_ShukkaGridQuantity, Def_T_SHUKKA_MEISAI.NUM, 30, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridRemarks, Def_T_SHUKKA_MEISAI.BIKO, 60, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridCustomsStatus, Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS, 60, false);
            colIndex++;
            // 2015/12/09 H.Tajimi M-No.対応
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridSTNo, Def_T_SHUKKA_MEISAI.ST_NO, 57, false);
            colIndex++;
            // ↑
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridARNo, Def_T_SHUKKA_MEISAI.AR_NO, 60, false);
            // 固定列設定
            this.shtMeisai.FreezeColumns = 4;
        }

        /// --------------------------------------------------
        /// <summary>
        /// パレット出荷のグリッド構築
        /// </summary>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet_Pallet()
        {
            int colIndex;
            colIndex = 0;
            this.SetElTabelleColumn(ElTabelleColumnType.Button, colIndex, Resources.ShukkaJoho_ShukkaGridState, ComDefine.FLD_BTN_STATE, 56, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Button, colIndex, string.Empty, ComDefine.FLD_BTN_DETAIL, 45, true);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridBoxNo, Def_T_SHUKKA_MEISAI.BOX_NO, 45, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridSerialNumber, Def_T_SHUKKA_MEISAI.SEIBAN, 80, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridCode, Def_T_SHUKKA_MEISAI.CODE, 30, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDrawingSequence, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, 37, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridArea, Def_T_SHUKKA_MEISAI.AREA, 55, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridFloor, Def_T_SHUKKA_MEISAI.FLOOR, 55, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridModel, Def_T_SHUKKA_MEISAI.KISHU, 35, false);
            colIndex++;
            // 2015/12/09 H.Tajimi M-No.対応
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridMNo, Def_T_SHUKKA_MEISAI.M_NO, 57, false);
            colIndex++;
            // ↑
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridJPName, Def_T_SHUKKA_MEISAI.HINMEI_JP, 160, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridName, Def_T_SHUKKA_MEISAI.HINMEI, 150, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDNumFormat, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, 40, false);
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridDivisionNum, Def_T_SHUKKA_MEISAI.KUWARI_NO, 30, false);
            // 2015/12/09 H.Tajimi M-No.対応
            colIndex++;
            this.SetElTabelleColumn(ElTabelleColumnType.Text, colIndex, Resources.ShukkaJoho_ShukkaGridSTNo, Def_T_SHUKKA_MEISAI.ST_NO, 57, false);
            // ↑
            // 固定列設定
            this.shtMeisai.FreezeColumns = 4;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 木枠出荷のグリッド構築
        /// </summary>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet_Kiwaku()
        {
            int colIndex;
            colIndex = 0;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridState, false, false, 77);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, string.Empty, false, false, 77);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridCNo, false, false, 80);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridStyle, false, false, 60);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridItem, false, false, 280);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridDescription, false, false, 280);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridShippingDivision, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridDeliveryDestinationCode, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridDeliveryDestination, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridShippingFlights, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridARNo, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridConstructionIdentificationNumber, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridInternalManagementKey, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridCNo, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridShippingDay, true, false, 0);
            colIndex++;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, Resources.ShukkaJoho_ShukkaGridVersion, true, false, 0);
            // 固定列設定
            this.shtMeisai.FreezeColumns = 3;
        }

        #endregion

        #endregion

        #region Sheetの列設定

        /// --------------------------------------------------
        /// <summary>
        /// Sheetの列設定
        /// </summary>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumn(ElTabelleColumnType colType, int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    this.SetElTabelleColumnText(colIndex, title, dataField, width, isEnabled);
                    break;
                case ElTabelleColumnType.Number:
                    this.SetElTabelleColumnNumber(colIndex, title, dataField, width, isEnabled);
                    break;
                case ElTabelleColumnType.Button:
                    this.SetElTabelleColumnButton(colIndex, title, dataField, width, isEnabled);
                    break;
                default:
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テキスト用列設定
        /// </summary>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumnText(int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            TextEditor editor = ElTabelleSheetHelper.NewTextEditor();
            this.SetElTabelleColumn(this.shtMeisai, colIndex, title, false, true, dataField, editor, width);
            this.shtMeisai.Columns[colIndex].Enabled = isEnabled;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 数値型用列設定
        /// </summary>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumnNumber(int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            NumberEditor editor = ElTabelleSheetHelper.NewNumberEditor();
            editor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
            this.SetElTabelleColumn(this.shtMeisai, colIndex, title, false, true, dataField, editor, width);
            this.shtMeisai.Columns[colIndex].Enabled = isEnabled;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ボタン型用列設定
        /// </summary>
        /// <param name="colIndex">列のインデックス</param>
        /// <param name="title">列ヘッダタイトル</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="width">列幅</param>
        /// <param name="isEnable">列が操作可能かどうか</param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetElTabelleColumnButton(int colIndex, string title, string dataField, int width, bool isEnabled)
        {
            ButtonEditor editor = ElTabelleSheetHelper.NewButtonEditor();
            editor.Text = null;
            this.SetElTabelleColumn(this.shtMeisai, colIndex, title, false, true, dataField, editor, width);
            this.shtMeisai.Columns[colIndex].Lock = false;
            this.shtMeisai.Columns[colIndex].Enabled = isEnabled;
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(this.shtMeisai.FreezeColumns, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 複数行表示のフォーマット設定

        /// --------------------------------------------------
        /// <summary>
        /// 複数行表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, bool isEnabled, AlignHorizontal alignHorizontal, KeyAction[] actions)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    cell.AlignHorizontal = alignHorizontal;
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Number:
                    NumberEditor numberEditor = ElTabelleSheetHelper.NewNumberEditor();
                    numberEditor.DisplayFormat = new NumberFormat("###,###,###", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    cell.AlignHorizontal = alignHorizontal;
                    cell.Editor = numberEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    cell.AlignHorizontal = alignHorizontal;
                    buttonEditor.Text = null;
                    cell.Editor = buttonEditor;
                    break;
            }
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        #endregion
    }
}
