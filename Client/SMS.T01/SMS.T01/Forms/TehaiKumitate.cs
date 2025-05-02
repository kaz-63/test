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
using GrapeCity.Win.ElTabelle.Editors;
using SMS.T01.Properties;
using WsConnection.WebRefT01;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 組立実績登録画面
    /// </summary>
    /// <create>H.Tajimi 2018/11/07</create>
    /// <update>K.Tsutsumi 2019/02/12 列の位置と幅調整</update>
    /// --------------------------------------------------
    public partial class TehaiKumitate : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        #region 選択状態

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECKED_OFF = 0;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECKED_ON = 1;
        
        #endregion

        #region シートの列

        private const int SHEET_COL_CHECKED = 0;
        private const int SHEET_COL_SETTEI_NOUKI = 1;
        private const int SHEET_COL_SEIBAN = 2;
        private const int SHEET_COL_CODE = 3;
        private const int SHEET_COL_ZUBAN_KATASHIKI = 4;
        private const int SHEET_COL_HINMEI_JP = 5;
        private const int SHEET_COL_HINMEI = 6;
        private const int SHEET_COL_TEHAI_QTY = 7;
        private const int SHEET_COL_ZAN_QTY = 8;
        private const int SHEET_COL_KUMITATE_QTY = 9;
        private const int SHEET_COL_BUKKEN_NAME = 10;
        private const int SHEET_COL_ECS_NO = 11;
        private const int SHEET_COL_AR_NO = 12;

        #endregion

        #region 物件名コンボボックスのインデックス
        private enum ComboBukkenIndex
        {
            All,
            ARMishukka,
            AR
        }
        #endregion

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期のデフォルト値
        /// </summary>
        /// <create>H.Tajimi 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _defaultSetteiNouki = string.Empty;

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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiKumitate(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSetteiNoukiDate, TEHAI_ASSY_DATE_RANGE.GROUPCD);
                this._defaultSetteiNouki = this.cboSetteiNoukiDate.SelectedValue.ToString();
                this.InitializeComboBukken();

                // テキストの初期化
                this.InitializeText();

                // DateTimePickerの初期化
                this.InitializeDateTimePicker();

                this.EditMode = SystemBase.EditMode.Update;
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboSetteiNoukiDate.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Text初期化

        /// --------------------------------------------------
        /// <summary>
        /// Text初期化
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update>J.Chen 2023/02/21 ECS No.桁数変更</update>
        /// --------------------------------------------------
        private void InitializeText()
        {
            // ECS No.
            this.txtECSNo.ImeMode = ImeMode.Disable;
            this.txtECSNo.InputRegulation = "F";
            this.txtECSNo.IsInputRegulation = false;
            this.txtECSNo.MaxByteLengthMode = true;
            this.txtECSNo.MaxLength = 20;

            // 製番
            this.txtSeiban.ImeMode = ImeMode.Disable;
            this.txtSeiban.InputRegulation = "F";
            this.txtSeiban.IsInputRegulation = false;
            this.txtSeiban.MaxByteLengthMode = true;
            this.txtSeiban.MaxLength = 12;

            // CODE
            this.txtCODE.ImeMode = ImeMode.Disable;
            this.txtCODE.InputRegulation = "F";
            this.txtCODE.IsInputRegulation = false;
            this.txtCODE.MaxByteLengthMode = true;
            this.txtCODE.MaxLength = 3;

            // AR No.
            this.txtARNo.ImeMode = ImeMode.Disable;
            this.txtARNo.InputRegulation = "n";
            this.txtARNo.IsInputRegulation = true;
            this.txtARNo.MaxByteLengthMode = true;
            this.txtARNo.MaxLength = 4;
        }

        #endregion

        #region DateTimePicker初期化

        /// --------------------------------------------------
        /// <summary>
        /// DateTimePicker初期化
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeDateTimePicker()
        {
            this.dtpSetteiNoukiDate.CustomFormat = "yyyy/MM/dd";
            this.dtpSetteiNoukiDate.ImeMode = ImeMode.Disable;
            this.dtpSetteiNoukiDate.Value = DateTime.Today;
        }

        #endregion

        #region シートの初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet">ElTabelle Sheet</param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            // Enterキーを下のセルに移動するよう変更
            sheet.ShortCuts.Remove(Keys.Enter);
            sheet.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
            sheet.AllowUserToAddRows = false;
            sheet.KeepHighlighted = true;
            sheet.SelectionType = SelectionType.MultipleRanges;
            sheet.ViewMode = ViewMode.Default;
            sheet.EditType = EditType.Default;
            try
            {
                int colIndex = 0;
                var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                this.SetElTabelleColumn(sheet, colIndex, string.Empty, false, false, ComDefine.FLD_SELECT_CHK, this.GetCheckBoxEditor(), 24);
                this.shtMeisai.Columns[colIndex].AlignHorizontal = AlignHorizontal.Center;
                this.shtMeisai.Columns[colIndex].AlignVertical = AlignVertical.Middle;
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_SetteiNouki, false, true, Def_T_TEHAI_MEISAI.SETTEI_DATE, txtEditor, 80);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_Seiban, false, true, Def_M_ECS.SEIBAN, txtEditor, 80);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_Code, false, true, Def_M_ECS.CODE, txtEditor, 30);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_PartNo, false, true, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, txtEditor, 100);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_JPName, false, true, Def_T_TEHAI_MEISAI.HINMEI_JP, txtEditor, 160);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_Name, false, true, Def_T_TEHAI_MEISAI.HINMEI, txtEditor, 150);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_ArrangementQty, false, true, Def_T_TEHAI_MEISAI.TEHAI_QTY, this.GetReadOnlyNumEditor("###,##0"), 65);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_RemainQty, false, true, ComDefine.FLD_REMAIN_QTY, this.GetReadOnlyNumEditor("###,##0"), 65);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_AssemblyQty, false, false, Def_T_TEHAI_MEISAI.ASSY_QTY, this.GetNumEditor("###,##0", -999999m, 999999m), 65);
                this.shtMeisai.Columns[colIndex].Enabled = true;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_BukkenName, false, true, Def_M_BUKKEN.BUKKEN_NAME, txtEditor, 160);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_ECSNo, false, true, Def_M_ECS.ECS_NO, txtEditor, 60);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                colIndex++;
                this.SetElTabelleColumn(sheet, colIndex, Resources.TehaiKumitate_ARNo, false, true, Def_M_ECS.AR_NO, txtEditor, 60);
                this.shtMeisai.Columns[colIndex].Enabled = false;
                this.shtMeisai.FreezeColumns = SHEET_COL_CODE;

                // グリッド線
                sheet.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    sheet.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Columns[i].DisabledForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 物件名コンボボックスの初期化
        /// </summary>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update>T.Nukaga 2019/11/11 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
        /// <update>K.Tsutsumi 2020/04/26 コンボボックスの初期表示「全て(AR未出荷)」→「全て」 </update>
        /// <update>K.Tsutsumi 2020/04/26 SelectedIndex -> SelectedValue </update>
        /// --------------------------------------------------
        private void InitializeComboBukken()
        {
            var conn = new ConnT01();
            var cond = new CondT01(this.UserInfo);
            var ds = conn.GetBukkenName();
            if (!UtilData.ExistsData(ds, Def_M_PROJECT.Name))
            {
                return;
            }

            // 物件名コンボボックスの初期化
            var dt = ds.Tables[Def_M_PROJECT.Name];
            {
                // 先頭に「全て」を挿入する
                var dr = dt.NewRow();
                dr[Def_M_PROJECT.PROJECT_NO] = decimal.MinValue;
                dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_DISP;
                dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.All);
            }
            {
                // 全て(AR 未出荷)の追加
                var dr = dt.NewRow();
                dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE;
                dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_MISHUKKA_AR_DISP;
                dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.ARMishukka);
            }
            {
                // 全て(AR)の追加
                var dr = dt.NewRow();
                dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_AR_VALUE;
                dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_AR_DISP;
                dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.AR);
            }
            dt.AcceptChanges();

            this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
            this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
            this.cboBukkenName.DataSource = dt;
            if (UtilData.ExistsData(dt))
            {
                this.cboBukkenName.SelectedValue = decimal.MinValue;
            }
            else
            {
                this.cboBukkenName.SelectedIndex = -1;
            }
        }

        #endregion

        #region Sheetの列設定

        #region チェックボックス型エディタ

        /// --------------------------------------------------
        /// <summary>
        /// チェックボックス型エディタ
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private CheckBoxEditor GetCheckBoxEditor()
        {
            var chkEditor = ElTabelleSheetHelper.NewCheckBokEditor();
            chkEditor.ThreeState = false;
            chkEditor.WordWrap = false;
            chkEditor.Text = null;
            return chkEditor;
        }
        
        #endregion

        #region 数値型エディタ

        /// --------------------------------------------------
        /// <summary>
        /// 読み取り専用数値型エディタ
        /// </summary>
        /// <param name="digit">表示フォーマット</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private NumberEditor GetReadOnlyNumEditor(string digit)
        {
            var numEditor = ElTabelleSheetHelper.NewNumberEditor();
            numEditor.DisplayFormat = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
            numEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
            numEditor.ReadOnly = true;
            return numEditor;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 数値型エディタ
        /// </summary>
        /// <param name="digit">表示フォーマット</param>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private NumberEditor GetNumEditor(string digit, decimal minValue, decimal maxValue)
        {
            var numEditor = ElTabelleSheetHelper.NewNumberEditor();
            numEditor.DisplayFormat = new NumberFormat("###,###", string.Empty, string.Empty, "-", string.Empty, string.Empty, string.Empty);
            numEditor.Format = new NumberFormat(digit, string.Empty, string.Empty, "-", string.Empty, string.Empty, string.Empty);
            numEditor.SpinOnKeys = false;
            numEditor.MinValue = minValue;
            numEditor.MaxValue = maxValue;
            numEditor.SpinIncrement = 0;
            return numEditor;
        }

        #endregion

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update>K.Tsutsumi 2020/04/26 コンボボックスの初期表示「全て(AR未出荷)」→「全て」 </update>
        /// <update>K.Tsutsumi 2020/04/26 SelectedIndex -> SelectedValue </update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 設定納期
                this.cboSetteiNoukiDate.SelectedValue = this._defaultSetteiNouki;
                this.dtpSetteiNoukiDate.Value = DateTime.Today;

                // 物件名
                if (this.cboBukkenName != null && this.cboBukkenName.Items.Count > 0)
                {
                    this.cboBukkenName.SelectedValue = decimal.MinValue;
                }
                else
                {
                    this.cboBukkenName.SelectedIndex = -1;
                }

                // ECS No.
                this.txtECSNo.Text = string.Empty;
                // 製番
                this.txtSeiban.Text = string.Empty;
                // CODE
                this.txtCODE.Text = string.Empty;
                // AR No.
                this.txtARNo.Text = string.Empty;

                // グリッド
                this.SheetClear();
                // 表示クリック前の状態にする
                this.ChangeEnableViewMode(false);
                this.ChangeEnableSetteiNoukiDate();
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            try
            {
                this.shtMeisai.Redraw = false;
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(0, this.shtMeisai.TopLeft.Row);
                }
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
                this.shtMeisai.Enabled = false;
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディション取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondT01 GetCondition()
        {
            var cond = new CondT01(this.UserInfo);
            cond.AssyRange = this.cboSetteiNoukiDate.SelectedValue.ToString();
            if (cond.AssyRange != TEHAI_ASSY_DATE_RANGE.ALL_VALUE1)
            {
                cond.SetteiDate = this.dtpSetteiNoukiDate.Value.ToString("yyyy/MM/dd");
            }
            if (this.cboBukkenName.Text != ComDefine.COMBO_ALL_DISP && this.cboBukkenName.SelectedValue != null)
            {
                cond.ProjectNo = this.cboBukkenName.SelectedValue.ToString();
            }
            if (!string.IsNullOrEmpty(this.txtECSNo.Text))
            {
                cond.EcsNo = this.txtECSNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtSeiban.Text))
            {
                cond.Seiban = this.txtSeiban.Text;
            }
            if (!string.IsNullOrEmpty(this.txtCODE.Text))
            {
                cond.Code = this.txtCODE.Text;
            }
            if (!string.IsNullOrEmpty(this.txtARNo.Text))
            {
                cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
            }
            return cond;
        }

        #endregion

        #region 組立数に有効な値が設定されているかどうか

        /// --------------------------------------------------
        /// <summary>
        /// 組立数に有効な値が設定されているかどうか
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidAssyQty(DataRow dr)
        {
            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ASSY_QTY)))
            {
                return false;
            }
            else
            {
                var assyQty = ComFunc.GetFldToDecimal(dr, Def_T_TEHAI_MEISAI.ASSY_QTY);
                if (assyQty == 0M)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.shtMeisai.EditState) return false;
                var dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null) return false;
                var dtCheck = dt.Copy();
                dtCheck.AcceptChanges();
                if (dtCheck.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    return false;
                }

                bool isEntered = false;
                for (int rowIndex = 0; rowIndex < dtCheck.Rows.Count; rowIndex++)
                {
                    if (this.IsValidAssyQty(dtCheck.Rows[rowIndex]))
                    {
                        isEntered = true;
                    }
                }
                if (!isEntered)
                {
                    // いずれかの組立数を設定してください。
                    this.ShowMessage("T0100070003");
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
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                bool isEnterd = false;
                if (this.cboSetteiNoukiDate.SelectedValue.ToString() == TEHAI_ASSY_DATE_RANGE.ALL_VALUE1)
                {
                    // 検索条件入力済みチェック
                    if (this.cboBukkenName.Text != ComDefine.COMBO_ALL_DISP
                        && this.cboBukkenName.SelectedValue != null)
                    {
                        isEnterd = true;
                    }
                    else if (!string.IsNullOrEmpty(this.txtECSNo.Text))
                    {
                        isEnterd = true;
                    }
                    else if (!string.IsNullOrEmpty(this.txtECSNo.Text))
                    {
                        isEnterd = true;
                    }
                    else if (!string.IsNullOrEmpty(this.txtSeiban.Text))
                    {
                        isEnterd = true;
                    }
                    else if (!string.IsNullOrEmpty(this.txtCODE.Text))
                    {
                        isEnterd = true;
                    }
                    else if (!string.IsNullOrEmpty(this.txtARNo.Text))
                    {
                        isEnterd = true;
                    }

                    if (!isEnterd)
                    {
                        // いずれかの検索条件を入力してください。
                        this.ShowMessage("T0100070004");
                        return false;
                    }
                }
                return ret;
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var cond = this.GetCondition();
                var conn = new ConnT01();
                var ds = conn.GetKumitateJisseki(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                try
                {
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                    this.shtMeisai.ActivePosition = new Position(SHEET_COL_KUMITATE_QTY, 0);
                }
                finally
                {
                    this.shtMeisai.Redraw = true;
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

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行の制御
        /// </summary>
        /// <returns></returns>
        /// <create>K.Tsutsumi 2019/02/12</create>
        /// <update></update>
        /// <remarks>手配側は登録したら再検索するような仕組みなっているので合わせる</remarks>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            var ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 再検索
                    if (!this.RunSearch())
                    {
                        this.DisplayClear();
                        this.cboSetteiNoukiDate.Focus();
                    }
                    else
                    {
                        this.shtMeisai.Enabled = true;
                        // モード切り替え
                        this.ChangeEnableViewMode(true);
                        this.shtMeisai.Focus();
                    }
                    return ret;

                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }

            return ret;
        }

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                var dt = (this.shtMeisai.DataSource as DataTable).Copy();
                var dtUpdate = dt.Clone();
                dtUpdate.Columns.Add(ComDefine.FLD_ROW_INDEX, typeof(int));
                for (int rowInex = 0; rowInex < dt.Rows.Count; rowInex++)
                {
                    var dr = dt.Rows[rowInex];
                    if (this.IsValidAssyQty(dr))
                    {
                        var drUpdate = dtUpdate.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            drUpdate[dc.ColumnName] = dr[dc.ColumnName];
                        }
                        drUpdate[ComDefine.FLD_ROW_INDEX] = (rowInex + 1);
                        dtUpdate.Rows.Add(drUpdate);
                    }
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtUpdate);

                var cond = this.GetCondition();
                var conn = new ConnT01();

                string errMsgID;
                string[] args;
                if (!conn.UpdKumitateJisseki(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
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

        #region 選択状態変更

        /// --------------------------------------------------
        /// <summary>
        /// 全体の選択状態変更
        /// </summary>
        /// <param name="isChecked">選択状態にするかどうかするかどうか</param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllChecked(bool isChecked)
        {
            this.SetChecked(new Range[] {
                new GrapeCity.Win.ElTabelle.Range(SHEET_COL_CHECKED, 0, SHEET_COL_CHECKED, this.shtMeisai.MaxRows - 1)
            }, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択範囲の選択状態変更
        /// </summary>
        /// <param name="isChecked">選択状態にするかどうかするかどうか</param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangeChecked(bool isChecked)
        {
            this.SetChecked(this.shtMeisai.GetBlocks(BlocksType.Selection), isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定範囲のチェック状態変更
        /// </summary>
        /// <param name="ranges">範囲</param>
        /// <param name="isChecked">選択状態にするかどうかするかどうか</param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetChecked(Range[] ranges, bool isChecked)
        {
            var lstRange = new List<Range>();
            foreach (Range range in ranges)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    if (rowIndex > this.shtMeisai.MaxRows - 1)
                    {
                        break;
                    }
                    lstRange.Add(new Range(SHEET_COL_CHECKED, rowIndex, SHEET_COL_CHECKED, rowIndex));
                }
            }

            foreach (Range range in lstRange)
            {
                this.shtMeisai.CellRange = range;
                this.shtMeisai.CellValue = isChecked ? CHECKED_ON : CHECKED_OFF;
            }
        }

        #endregion

        #region 表示時のEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">表示状態かどうか</param>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 全選択、全選択解除、範囲選択、範囲選択解除
            this.btnAllSelect.Enabled = isView;
            this.btnAllDeselect.Enabled = isView;
            this.btnRangeSelect.Enabled = isView;
            this.btnRangeDeselect.Enabled = isView;

            // 検索条件のロック解除
            this.grpSearch.Enabled = !isView;
            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
            // 一括組立ボタン
            this.fbrFunction.F04Button.Enabled = isView;
            // Clearボタン
            this.fbrFunction.F06Button.Enabled = isView;
        }

        #endregion

        #region 設定納期のEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期のEnabled切替
        /// </summary>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableSetteiNoukiDate()
        {
            if (this.cboSetteiNoukiDate == null || this.cboSetteiNoukiDate.SelectedValue == null)
            {
                return;
            }

            if (this.cboSetteiNoukiDate.SelectedValue.ToString() == TEHAI_ASSY_DATE_RANGE.ALL_VALUE1)
            {
                this.dtpSetteiNoukiDate.Enabled = false;
            }
            else
            {
                this.dtpSetteiNoukiDate.Enabled = true;
            }
        }

        #endregion

        #region 検索処理実行

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行
        /// </summary>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExecRunSearch()
        {
            // 検索
            if (!this.RunSearch())
            {
                this.cboSetteiNoukiDate.Focus();
                return;
            }
            this.shtMeisai.Enabled = true;
            // モード切り替え
            this.ChangeEnableViewMode(true);
            this.shtMeisai.Focus();
        }
        
        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.shtMeisai.EditState) return;
                this.ClearMessage();
                base.fbrFunction_F01Button_Click(sender, e);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                var dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null) return;

                this.shtMeisai.Redraw = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (ComFunc.GetFldToBool(dr, ComDefine.FLD_SELECT_CHK))
                    {
                        // チェックされている行は残数→組立数にセット
                        dr[Def_T_TEHAI_MEISAI.ASSY_QTY] = dr[ComDefine.FLD_REMAIN_QTY];
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // モード切り替え
                this.ChangeEnableViewMode(false);
                this.ChangeEnableSetteiNoukiDate();
                // フォーカス切替
                this.cboSetteiNoukiDate.Focus();
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
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 検索
                this.ExecRunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetAllChecked(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetAllChecked(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetRangeChecked(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択解除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetRangeChecked(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックス

        /// --------------------------------------------------
        /// <summary>
        /// 設定納期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboSetteiNoukiDate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.ChangeEnableSetteiNoukiDate();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #endregion
    }
}
