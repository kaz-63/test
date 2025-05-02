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
using SMS.M01.Properties;
using SystemBase.Util;
using WsConnection.WebRefM01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷元保守
    /// </summary>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkamotoHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tajimi 2020/04/14</create>
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 名称の列インデックス
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHIP_FROM_COL = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷元CDの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHIP_FROM_CD = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 社内外フラグの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHANAIGAI_FLAG_COL = 5;

        #endregion

        #region フィールド変数

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkamotoHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
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
                int colIndex = 0;
                shtResult.ColumnHeaders[colIndex++].Caption = Resources.ShukkamotoHoshu_ShanaiGaiFlag;
                shtResult.ColumnHeaders[colIndex++].Caption = Resources.ShukkamotoHoshu_ShipFrom;
                shtResult.ColumnHeaders[colIndex++].Caption = Resources.ShukkamotoHoshu_UnusedFlag;
                shtResult.ColumnHeaders[colIndex++].Caption = Resources.ShukkamotoHoshu_DispNo;

                // コンボボックスの初期化
                InitializeComboBox();

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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.scboShanaigaiFlag.SelectedValue = ComDefine.COMBO_FIRST_VALUE;
                this.scboUnusedFlag.SelectedValue = ComDefine.COMBO_FIRST_VALUE;
                this.scboUnusedFlag.Focus();
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
        /// コンボボックスの初期化
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboBox()
        {
            Func<DataSet, string, DataTable> fncInsAll = (ds, tableName) =>
            {
                if (ComFunc.IsExistsData(ds, tableName))
                {
                    var dt = ds.Tables[tableName].Copy();
                    var dr = dt.NewRow();
                    dr[Def_M_COMMON.VALUE1] = ComDefine.COMBO_FIRST_VALUE;
                    dr[Def_M_COMMON.ITEM_NAME] = ComDefine.COMBO_ALL_DISP;
                    dt.Rows.InsertAt(dr, 0);
                    dt.AcceptChanges();
                    return dt;
                }
                else
                {
                    return ds.Tables[tableName].Clone();
                }
            };

            try
            {
                var cond = new CondM01(this.UserInfo);
                var conn = new ConnM01();
                var ds = conn.GetInitShukkamotoHoshu(cond);
                {
                    // 社内外コンボボックスの初期化
                    var dtContainsAll = fncInsAll(ds, SHANAIGAI_FLAG.GROUPCD);
                    this.MakeCmbBox(this.scboShanaigaiFlag, dtContainsAll, Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_FIRST_VALUE, false);
                    this.MakeCmbBox(this.cboShanaigaiFlag, ds.Tables[SHANAIGAI_FLAG.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_FIRST_VALUE, false);
                }
                {
                    // 未使用コンボボックスの初期化
                    var dtContainsAll = fncInsAll(ds, UNUSED_FLAG.GROUPCD);
                    this.MakeCmbBox(this.scboUnusedFlag, dtContainsAll, Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_FIRST_VALUE, false);
                    this.MakeCmbBox(this.cboUnusedFlag, ds.Tables[UNUSED_FLAG.GROUPCD], Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_FIRST_VALUE, false);
                }
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.scboShanaigaiFlag.SelectedValue = ComDefine.COMBO_FIRST_VALUE;
                this.scboUnusedFlag.SelectedValue = ComDefine.COMBO_FIRST_VALUE;
                this.stxtShipFrom.Text = string.Empty;
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                //if (this.cboShanaigaiFlag.SelectedIndex < 0)
                //{
                //    // 社内外を選択して下さい。
                //    this.ShowMessage("M0100190007");
                //    this.cboShanaigaiFlag.Focus();
                //    return false;
                //}

                // 名称チェック
                if (string.IsNullOrEmpty(this.txtShipFrom.Text))
                {
                    // 名称を入力してください。
                    this.ShowMessage("M0100190001");
                    this.txtShipFrom.Focus();
                    return false;
                }
                if (Encoding.GetEncoding("Shift-JIS").GetByteCount(this.txtShipFrom.Text) > this.txtShipFrom.MaxLength)
                {
                    // {0}には{1}Byte以下で入力して下さい。
                    this.ShowMessage("A9999999052", this.lblShipFrom.Text, this.txtShipFrom.MaxLength.ToString()  );
                    this.txtShipFrom.Focus();
                    return false;
                }

                if (this.cboUnusedFlag.SelectedIndex < 0)
                {
                    // 未使用を選択して下さい。
                    this.ShowMessage("M0100190002");
                    this.cboUnusedFlag.Focus();
                    return false;
                }

                // 並び順チェック
                if (string.IsNullOrEmpty(this.numDispNo.Text))
                {
                    // 並び順を入力してください。
                    this.ShowMessage("M0100190003");
                    this.numDispNo.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                //if (this.scboShanaigaiFlag.SelectedValue != null && this.scboShanaigaiFlag.SelectedValue.ToString() != ComDefine.COMBO_FIRST_VALUE)
                //{
                //    cond.ShanaigaiFlag = this.scboShanaigaiFlag.SelectedValue.ToString();
                //}
                if (this.scboUnusedFlag.SelectedValue != null && this.scboUnusedFlag.SelectedValue.ToString() != ComDefine.COMBO_FIRST_VALUE)
                {
                    cond.UnusedFlag = this.scboUnusedFlag.SelectedValue.ToString();
                }
                cond.ShipFromName = this.stxtShipFrom.Text;

                var ds = conn.GetShipFrom(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_SHIP_FROM.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当する出荷元はありません。
                    this.ShowMessage("M0100190006");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_SHIP_FROM.Name;

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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    this.DisplayClearEdit();
                    this.RunSearch();
                }
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                //cond.ShanaigaiFlag = ComFunc.GetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHANAIGAI_FLAG);
                cond.ShipFromName = ComFunc.GetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHIP_FROM_NAME);

                string errMsgID;
                string[] args;
                if (!conn.InsShipFrom(cond, ds, out errMsgID, out args))
                {
                    if (errMsgID == "A9999999027" || errMsgID == "A9999999086")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    this.txtShipFrom.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShipFromCd = ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHIP_FROM_NO);
                //cond.ShanaigaiFlag = ComFunc.GetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHANAIGAI_FLAG);
                cond.ShipFromName = ComFunc.GetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHIP_FROM_NAME);

                UtilData.SetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHIP_FROM_NO, ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHIP_FROM_NO));
                UtilData.SetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SHIP_FROM.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.UpdShipFrom(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) || errMsgID == "M0100190005")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    this.txtShipFrom.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShipFromCd = ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHIP_FROM_NO);

                UtilData.SetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.SHIP_FROM_NO, ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHIP_FROM_NO));
                UtilData.SetFld(ds, Def_M_SHIP_FROM.Name, 0, Def_M_SHIP_FROM.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_SHIP_FROM.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.DelShipFrom(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID) || errMsgID == "M0100190005")
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tajimi 2020/04/14</create>
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
        /// <create>H.Tajimi 2020/04/14</create>
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
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.cboShanaigaiFlag.SelectedValue = SHANAIGAI_FLAG.DEFAULT_VALUE1;
                this.cboUnusedFlag.SelectedValue = UNUSED_FLAG.DEFAULT_VALUE1;
                this.txtShipFrom.Text = string.Empty;
                this.numDispNo.Clear();
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (this.shtResult.ActivePosition.Row < 0 && selectType != DataSelectType.Insert)
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
        /// <create>H.Tajimi 2020/04/14</create>
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
                                this.txtShipFrom.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                this.txtShipFrom.Focus();
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeShipFrom();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    var conn = new ConnM01();
                    var cond = new CondM01(this.UserInfo);
                    cond.ShipFromCd = this.shtResult[SHEET_COL_SHIP_FROM_CD, this.shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetShipFrom(cond);
                    if (!ComFunc.IsExistsData(ds, Def_M_SHIP_FROM.Name))
                    {
                        // 他端末で削除されています。
                        this.ShowMessage("A9999999026");
                        // 消えてるのがあったから取り敢えず検索
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_SHIP_FROM.Name];
                    // 表示データ設定
                    this.cboShanaigaiFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHANAIGAI_FLAG);
                    this.txtShipFrom.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.SHIP_FROM_NAME);
                    this.cboUnusedFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_SHIP_FROM.UNUSED_FLAG);
                    this.numDispNo.Value = ComFunc.GetFldToDecimal(this._dtDispData, 0, Def_M_SHIP_FROM.DISP_NO);
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

        #region 出荷元マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元マスタのデータテーブル作成
        /// </summary>
        /// <returns>出荷元マスタのデータテーブル</returns>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeShipFrom()
        {
            try
            {
                var dt = new DataTable(Def_M_SHIP_FROM.Name);
                dt.Columns.Add(Def_M_SHIP_FROM.SHIP_FROM_NO, typeof(string));
                dt.Columns.Add(Def_M_SHIP_FROM.SHANAIGAI_FLAG, typeof(string));
                dt.Columns.Add(Def_M_SHIP_FROM.SHIP_FROM_NAME, typeof(string));
                dt.Columns.Add(Def_M_SHIP_FROM.UNUSED_FLAG, typeof(string));
                dt.Columns.Add(Def_M_SHIP_FROM.DISP_NO, typeof(decimal));
                dt.Columns.Add(Def_M_SHIP_FROM.VERSION, typeof(object));
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
        /// <param name="dt">出荷元マスタ登録データ</param>
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable SetEditData(DataTable dt)
        {
            try
            {
                var ret = dt.Clone();
                var dr = ret.NewRow();
                dr[Def_M_SHIP_FROM.SHANAIGAI_FLAG] = SHANAIGAI_FLAG.DEFAULT_VALUE1;
                dr[Def_M_SHIP_FROM.SHIP_FROM_NAME] = this.txtShipFrom.Text;
                dr[Def_M_SHIP_FROM.UNUSED_FLAG] = this.cboUnusedFlag.SelectedValue.ToString();
                dr[Def_M_SHIP_FROM.DISP_NO] = this.numDispNo.Value;
                ret.Rows.Add(dr);
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2020/04/14</create>
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
                this.cboShanaigaiFlag.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tajimi 2020/04/14</create>
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
        /// <create>H.Tajimi 2020/04/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.DisplayClearEdit();
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
        /// <create>H.Tajimi 2020/04/14</create>
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
        /// <create>H.Tajimi 2020/04/14</create>
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
    }
}
