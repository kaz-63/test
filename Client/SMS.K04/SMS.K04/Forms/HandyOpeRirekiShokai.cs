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
using SMS.K04.Properties;
using WsConnection.WebRefK04;

namespace SMS.K04.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// Handyオペ履歴照会
    /// </summary>
    /// <create>H.Tajimi 2019/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class HandyOpeRirekiShokai : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 物件名コンボボックス用データセット
        /// </summary>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        DataSet _dsBukkenCombo = null;

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
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyOpeRirekiShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // 画面の編集モード変更
                this.EditMode = SystemBase.EditMode.View;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトル設定
                int col = 0;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_UpdateDate;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_OpeFlag;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_WorkerName;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_TagNo;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_BoxNo;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_PalletNo;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_TehaiNo;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_Num;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_Weight;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_ShukkaFlag;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_Ship;
                this.shtMeisai.ColumnHeaders[col++].Caption = Resources.HandyOpeRirekiShokai_ARNo;

                // コンボボックスの初期化
                this.MakeComboBoxes();
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
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboHandyOpeFlag.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 終了処理

        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                if (this._dsBukkenCombo != null)
                {
                    this._dsBukkenCombo.Dispose();
                    this._dsBukkenCombo = null;
                }
            }
        }
        
        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();
                // 操作区分
                if (0 < this.cboHandyOpeFlag.Items.Count)
                {
                    this.cboHandyOpeFlag.SelectedValue = OPERATION_FLAG.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboHandyOpeFlag.SelectedIndex = -1;
                }
                // 日付範囲
                var today = DateTime.Today;
                this.dtpUpdateDateFrom.Value = today.AddMonths(-2);
                this.dtpUpdateDateTo.Value = today;
                // 作業者
                this.txtWorkerName.Text = string.Empty;
                // 出荷区分
                if (0 < this.cboShukkaFlag.Items.Count)
                {
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboShukkaFlag.SelectedIndex = -1;
                }
                // 物件名
                this.cboBukkenName.SelectedIndex = -1;
                // 手配No
                this.txtTehaiNo.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
                // AR No.
                this.txtARNo.Text = string.Empty;
                // TAG No.
                this.txtTagNo.Text = string.Empty;
                // Box No.
                this.txtBoxNo.Text = string.Empty;
                // Pallet No.
                this.txtPalletNo.Text = string.Empty;

                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tajimi 2019/08/22</create>
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
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (this.cboHandyOpeFlag.SelectedIndex < 0)
                {
                    // 操作区分を選択してください。
                    this.ShowMessage("K0400070001");
                    this.cboHandyOpeFlag.Focus();
                    return false;
                }

                if (this.dtpUpdateDateFrom.Value == null)
                {
                    // 日付範囲を選択してください。
                    this.ShowMessage("K0400070002");
                    this.dtpUpdateDateFrom.Focus();
                    return false;
                }

                if (this.dtpUpdateDateTo.Value == null)
                {
                    // 日付範囲を選択してください。
                    this.ShowMessage("K0400070002");
                    this.dtpUpdateDateTo.Focus();
                    return false;
                }

                if (this.cboShukkaFlag.Enabled)
                {
                    if (this.cboShukkaFlag.SelectedIndex < 0)
                    {
                        // 出荷区分を選択してください。
                        this.ShowMessage("K0400070003");
                        this.cboShukkaFlag.Focus();
                        return false;
                    }
                }

                if (this.cboBukkenName.SelectedIndex < 0)
                {
                    // 物件名を入力してください。
                    this.ShowMessage("K0400070004");
                    this.cboShukkaFlag.Focus();
                    return false;
                }

                return true;
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
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // コンディションの設定
                var cond = new CondK04(this.UserInfo);
                cond.HandyOpeFlag = this.cboHandyOpeFlag.SelectedValue.ToString();
                cond.DateFrom = this.dtpUpdateDateFrom.Value.Date;
                var e = this.dtpUpdateDateTo.Value.Date;
                cond.DateTo = new DateTime(e.Year, e.Month, e.Day, 23, 59, 59, 999);
                cond.HandyLoginID = this.GetConditionText(this.txtWorkerName);
                if (this.cboHandyOpeFlag.SelectedValue.ToString() == HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1)
                {
                    cond.ProjectNo = this.cboBukkenName.SelectedValue.ToString();
                }
                else
                {
                    cond.ShukkaFlag = this.GetConditionSelectedValue(this.cboShukkaFlag);
                    cond.BukkenNo = this.cboBukkenName.SelectedValue.ToString();
                }
                cond.TehaiNo = this.GetConditionText(this.txtTehaiNo);
                cond.Ship = this.GetConditionText(this.txtShip);
                cond.ARNo = this.GetTextWithPrefix(this.txtARNo, this.lblAR.Text);
                cond.TagNo = this.GetConditionText(this.txtTagNo);
                cond.BoxNo = this.GetTextWithPrefixAndPadLeft(this.txtBoxNo, this.lblBox.Text, this.txtBoxNo.MaxLength);
                cond.PalletNo = this.GetTextWithPrefixAndPadLeft(this.txtPalletNo, this.lblPallet.Text, this.txtPalletNo.MaxLength);

                // 表示データ取得
                var conn = new ConnK04();
                var ds = conn.GetHandyOpeRireki(cond);
                this.shtMeisai.Enabled = false;
                if (!ComFunc.IsExistsData(ds, Def_T_HANDY_RIREKI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                this.shtMeisai.DataSource = ds.Tables[Def_T_HANDY_RIREKI.Name];
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                    this.shtMeisai.Enabled = true;
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

        #region 操作区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 操作区分切替
        /// </summary>
        /// <param name="needsUpdBukkenCombo">物件コンボボックスを更新するかどうか</param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeOpeFlag(bool needsUpdBukkenCombo)
        {
            if (this.cboHandyOpeFlag.SelectedValue.ToString() == HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1)
            {
                this.cboShukkaFlag.Enabled = false;
                this.txtShip.Enabled = false;
                this.txtARNo.Enabled = false;
                this.txtTagNo.Enabled = false;
                this.txtBoxNo.Enabled = false;
                this.txtPalletNo.Enabled = false;

                this.txtTehaiNo.Enabled = true;
            }
            else
            {
                this.cboShukkaFlag.Enabled = true;
                this.txtShip.Enabled = true;
                this.txtARNo.Enabled = true;
                this.txtTagNo.Enabled = true;
                this.txtBoxNo.Enabled = true;
                this.txtPalletNo.Enabled = true;

                this.txtTehaiNo.Enabled = false;
            }
            this.ChangeShukkaFlag(false);

            if (needsUpdBukkenCombo)
            {
                // 物件コンボボックスの設定
                this.SetBukkenComboBox();
            }
        }

        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分切替
        /// </summary>
        /// <param name="needsUpdBukkenCombo">物件コンボボックスを更新するかどうか</param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag(bool needsUpdBukkenCombo)
        {
            if (this.cboHandyOpeFlag.SelectedValue.ToString() == HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1)
            {
                // 入荷検品が選択されている場合は出荷区分はEnabled＝Falseなので何もしない
                return;
            }

            // 便
            this.txtShip.Text = string.Empty;
            // ARNo.
            this.txtARNo.Text = string.Empty;
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                // AR No.
                this.txtARNo.Enabled = false;
                this.txtShip.Enabled = true;
            }
            else
            {
                // ----- AR -----
                // AR No.
                if (this.cboHandyOpeFlag.SelectedValue.ToString() == HANDY_OPERATION_FLAG.PALLET_KONPO_VALUE1)
                {
                    this.txtARNo.Enabled = false;
                }
                else
                {
                    this.txtARNo.Enabled = true;
                }
                this.txtShip.Enabled = false;
            }

            if (needsUpdBukkenCombo)
            {
                // 物件コンボボックスの設定
                this.SetBukkenComboBox();
            }
        }

        #endregion

        #region コンボボックスのDataSourceを作成＆設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスのDataSourceを作成する。
        /// </summary>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void MakeComboBoxes()
        {
            var cond = new CondK04(this.UserInfo);
            var conn = new ConnK04();
            var ds = conn.GetInitHandyOpeRirekiShokai(cond);
            if (this._dsBukkenCombo != null)
            {
                this._dsBukkenCombo.Dispose();
                this._dsBukkenCombo = null;
            }
            this._dsBukkenCombo = ds;

            // 操作区分コンボボックスの設定
            this.MakeCmbBox(this.cboHandyOpeFlag, this._dsBukkenCombo.Tables[HANDY_OPERATION_FLAG.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, false);
            // 出荷区分コンボボックスの設定
            this.MakeCmbBox(this.cboShukkaFlag, this._dsBukkenCombo.Tables[SHUKKA_FLAG.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, false);

            // 操作区分コンボボックスの内容に伴うコントロールEnabled設定
            this.ChangeOpeFlag(false);
            // 出荷区分コンボボックスの内容に伴うコントロールEnabled設定
            this.ChangeShukkaFlag(false);
            // 物件コンボボックスの設定
            this.SetBukkenComboBox();
        }

        #endregion

        #region ファンクションバーのEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="enabled"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool enabled)
        {
            this.fbrFunction.F06Button.Enabled = enabled;
        }

        #endregion

        #region 物件コンボボックス設定

        /// --------------------------------------------------
        /// <summary>
        /// 物件コンボボックス設定
        /// </summary>
        /// <create>H.Tajimi 2019/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetBukkenComboBox()
        {
            var opeFlag = this.cboHandyOpeFlag.SelectedValue.ToString();
            if (opeFlag == HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1)
            {
                if (UtilData.ExistsData(this._dsBukkenCombo, Def_M_PROJECT.Name))
                {
                    var dt = this._dsBukkenCombo.Tables[Def_M_PROJECT.Name].Copy();
                    this.cboBukkenName.DataSource = dt;
                    this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
                    this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                }
            }
            else
            {
                var shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                var drc = this._dsBukkenCombo.Tables[Def_M_BUKKEN.Name].AsEnumerable()
                    .Where(x => ComFunc.GetFld(x, Def_M_BUKKEN.SHUKKA_FLAG) == shukkaFlag);
                if (drc != null)
                {
                    this.cboBukkenName.DataSource = drc.CopyToDataTable();
                    this.cboBukkenName.ValueMember = Def_M_BUKKEN.BUKKEN_NO;
                    this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                }
            }
        }

        #endregion

        #region 検索条件の値取得

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件の値取得
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetConditionText(Control ctrl)
        {
            if (!this.IsValidCondition(ctrl))
            {
                return string.Empty;
            }
            return ctrl.Text;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件の値取得
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <param name="prefix">プレフィックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetConditionText(Control ctrl, string prefix)
        {
            if (!this.IsValidCondition(ctrl))
            {
                return string.Empty;
            }
            return prefix + ctrl.Text;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件の値取得
        /// </summary>
        /// <param name="cboCtrl">コンボボックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetConditionSelectedValue(DSWComboBox cboCtrl)
        {
            if (!this.IsValidCondition(cboCtrl))
            {
                return string.Empty;
            }
            return cboCtrl.SelectedValue.ToString();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件に値が設定されているかどうか
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidCondition(Control ctrl)
        {
            if (!ctrl.Enabled)
            {
                return false;
            }
            return !string.IsNullOrEmpty(ctrl.Text);
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッド
                this.SheetClear();
                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
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
        /// <create>H.Tajimi 2019/08/22</create>
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
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // グリッドクリア
                this.SheetClear();

                // 検索
                if (!this.RunSearch())
                {
                    // フォーカスのセット
                    this.cboHandyOpeFlag.Focus();
                    return;
                }

                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);
                // フォーカス
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 操作区分

        /// --------------------------------------------------
        /// <summary>
        /// 操作区分コンボボックスSelectionChangeCommittedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboOpeFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeOpeFlag(true);
        }
        
        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag(true);
        }

        #endregion

        #endregion
    }
}
