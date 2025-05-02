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

using WsConnection.WebRefP02;
using SMS.P02.Properties;

namespace SMS.P02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 履歴照会
    /// </summary>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class RirekiShokai : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 画面区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _gamenFlag = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ship = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _arNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 初期時の納入先コード
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCdFst = string.Empty;

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
        /// <create>T.Sakiori 2012/04/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public RirekiShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="ship">便</param>
        /// <param name="arNo">ARNo</param>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        public RirekiShokai(UserInfo userInfo, string gamenFlag, string shukkaFlag, string nonyusakiCd, string bukkenName, string ship, string arNo)
            : base(userInfo, ComDefine.TITLE_P0200050)
        {
            this._gamenFlag = gamenFlag;
            this._shukkaFlag = shukkaFlag;
            this._nonyusakiCd = nonyusakiCd;
            this._nonyusakiCdFst = nonyusakiCd;
            this._bukkenName = bukkenName;
            this._ship = ship;
            this._arNo = arNo;
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // シートの初期化
                this.InitializeSheet(this.shtResult);

                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.RirekiShokai_ModifiedDateTime;
                shtResult.ColumnHeaders[1].Caption = Resources.RirekiShokai_ModifiedUser;
                shtResult.ColumnHeaders[2].Caption = Resources.RirekiShokai_ProcessName;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboGamenFlag, GAMEN_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    this.cboGamenFlag.SelectedValue = this._gamenFlag;
                    this.cboShukkaFlag.SelectedValue = this._shukkaFlag;
                }
                else
                {
                    this.cboGamenFlag.SelectedValue = GAMEN_FLAG.DEFAULT_VALUE1;
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                }

                this.ChangeShukkaFlag();
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
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboGamenFlag.Focus();
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
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // シートのクリア
                this.SheetClear();
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    this.cboGamenFlag.SelectedValue = this._gamenFlag;
                    this.cboShukkaFlag.SelectedValue = this._shukkaFlag;
                    this.txtNonyusakiName.Text = this._bukkenName;
                    this.txtShip.Text = this._ship;
                    this.txtARNo.Text = this._arNo;
                    this._nonyusakiCd = this._nonyusakiCdFst;

                    this.cboGamenFlag.Enabled = false;
                    this.cboShukkaFlag.Enabled = false;
                    this.txtARNo.ReadOnly = true;
                    this.btnListSelect.Enabled = false;
                    this.SetOperationFlagData(this._gamenFlag);
                }
                else
                {
                    this.cboGamenFlag.SelectedValue = GAMEN_FLAG.DEFAULT_VALUE1;
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                    this.txtNonyusakiName.Text = string.Empty;
                    this.txtShip.Text = string.Empty;
                    this.txtARNo.Text = string.Empty;

                    this.SetOperationFlagData(GAMEN_FLAG.DEFAULT_VALUE1);
                }
                this.txtUpdateUserName.Text = string.Empty;
                int month = UtilData.GetFldToInt32(new ConnP02().GetShime(), Def_M_SHIME.Name, 0, Def_M_SHIME.HOJIKIKAN) * -1;
                this.dtpUpdateDateFrom.Value = DateTime.Today.AddMonths(month);
                this.dtpUpdateDateTo.Value = DateTime.Today;

                this.ChangeShukkaFlag();
                // 2015/12/02 H.Tajimi ファンクションボタンEnabled設定
                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
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
        /// 検索用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.cboGamenFlag.SelectedIndex == -1)
                {
                    // 画面区分を選択して下さい。
                    this.ShowMessage("P0200050001");
                    this.cboGamenFlag.Focus();
                    return false;
                }
                if (this.cboShukkaFlag.SelectedIndex == -1)
                {
                    // 出荷区分を選択して下さい。
                    this.ShowMessage("P0200050002");
                    this.cboShukkaFlag.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("P0200050003");
                    this.txtNonyusakiName.Focus();
                    return false;
                }
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1
                    && string.IsNullOrEmpty(this.txtARNo.Text)
                    && !this.txtARNo.ReadOnly)
                {
                    // AR No.を入力して下さい。
                    this.ShowMessage("A9999999018");
                    this.txtARNo.Focus();
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
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
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
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var cond = new CondP02(this.UserInfo);
                cond.GamenFlag = this.cboGamenFlag.SelectedValue.ToString();
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.NonyusakiCd = this._nonyusakiCd;
                if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                }
                cond.UpdateUserName = this.txtUpdateUserName.Text;
                cond.OperationFlag = this.cboOperationFlag.SelectedValue.ToString();
                if (this.dtpUpdateDateFrom.Value == null)
                {
                    cond.UpdateDateFrom = null;
                }
                else
                {
                    cond.UpdateDateFrom = UtilConvert.ToDateTime(this.dtpUpdateDateFrom.Value).Date;
                }
                if (this.dtpUpdateDateTo.Value == null)
                {
                    cond.UpdateDateTo = null;
                }
                else
                {
                    cond.UpdateDateTo = UtilConvert.ToDateTime(this.dtpUpdateDateTo.Value).Date.AddDays(1).AddMilliseconds(-1);
                }

                var ds = new ConnP02().GetRireki(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_RIREKI.Name))
                {
                    this.ShowMessage("P0200050004");
                    return false;
                }
                this.shtResult.Redraw = false;
                this.shtResult.DataSource = ds.Tables[Def_T_RIREKI.Name];
                this.shtResult.Enabled = true;
                // 2015/12/02 H.Tajimi ファンクションボタンEnabled設定
                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(true);
                // ↑
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtResult.Redraw = true;
            }
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
        /// <create>T.Sakiori 2012/04/16</create>
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
                // シートのクリア
                this.SheetClear();
                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
                this.txtNonyusakiName.Focus();
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
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboGamenFlag.Focus();
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
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // シートクリア
                this.SheetClear();
                // 2015/11/30 H.Tajimi 納入先一覧表示
                if (!this.ShowNonyusakiIchiran())
                {
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // フォーカス
                this.shtResult.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面区分

        /// --------------------------------------------------
        /// <summary>
        /// 画面区分コンボボックスSelectionChangeCommitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboGamenFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.cboShukkaFlag.Enabled = true;

                this.SetOperationFlagData(this.cboGamenFlag.SelectedValue.ToString());

                if (this.cboGamenFlag.SelectedValue.ToString() == GAMEN_FLAG.A0100010_VALUE1)
                {
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.AR_VALUE1;
                    this.cboShukkaFlag.Enabled = false;
                }

                // 納入先と便のクリア
                this.txtNonyusakiName.Text = string.Empty;
                this.txtShip.Text = string.Empty;

                this.ChangeShukkaFlag();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックスSelectionChangeCommitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag();

            // 納入先と便のクリア
            this.txtNonyusakiName.Text = string.Empty;
            this.txtShip.Text = string.Empty;
        }

        #endregion

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtResult.Redraw = false;
            if (0 < this.shtResult.MaxRows)
            {
                this.shtResult.TopLeft = new Position(0, 0);
            }
            this.shtResult.DataSource = null;
            this.shtResult.MaxRows = 0;
            this.shtResult.Enabled = false;
            this.shtResult.Redraw = true;
        }

        #endregion

        #region 処理名コンボボックス作成

        /// --------------------------------------------------
        /// <summary>
        /// 処理名コンボボックス作成
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetOperationFlagData(string selectedValue)
        {
            try
            {
                var cond = new CondP02(this.UserInfo);
                cond.Value2 = selectedValue;

                var ds = new ConnP02().GetOperationFlag(cond);
                this.cboOperationFlag.DisplayMember = Def_M_COMMON.ITEM_NAME;
                this.cboOperationFlag.ValueMember = Def_M_COMMON.VALUE1;
                this.cboOperationFlag.DataSource = ds.Tables[Def_M_COMMON.Name];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 出荷区分切り替え

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分切り替え
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {
            if (this.cboShukkaFlag.SelectedValue == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(this._gamenFlag))
            {
                this.txtARNo.ReadOnly = true;
            }
            else if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                this.txtARNo.Text = string.Empty;
                this.txtARNo.Enabled = false;
                // 2015/11/30 H.Tajimi 便を有効にする
                this.txtShip.Enabled = true;
                // ↑
            }
            else
            {
                this.txtARNo.Enabled = true;
                // 2015/11/30 H.Tajimi 便を無効にする
                this.txtShip.Enabled = false;
                // ↑
            }
        }

        #endregion

        #region 画面表示

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran()
        {
            string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            string nonyusakiName = this.txtNonyusakiName.Text;
            string ship = this.txtShip.Text;
            using (var frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, ship, this.cboGamenFlag.SelectedValue.ToString(), true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    this._nonyusakiCd = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region モード切替操作

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            this.fbrFunction.F06Button.Enabled = isEnabled;
        }

        #endregion
    }
}
