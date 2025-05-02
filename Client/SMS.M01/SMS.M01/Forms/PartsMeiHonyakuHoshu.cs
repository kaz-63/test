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
using WsConnection.WebRefM01;
using WsConnection.WebRefMaster;
using SystemBase.Util;
using SMS.E01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタ
    /// </summary>
    /// <create>S.Furugo 2018/10/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PartsMeiHonyakuHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>T.Sakiori 2012/04/05</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 型式の列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_PARTS_CD = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)の列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI_JP = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 型式の列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI = 2;
        

        #endregion

        #region Fields
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// <create>$Creator$</create>
        /// <update>J.Chen 2024/11/11 貼り付け内容の文字コードを変換</update>
        /// <update></update>
        /// --------------------------------------------------
        public PartsMeiHonyakuHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();

            txtSearchPartNameJa.KeyDown += TextBox_KeyDown;
            txtSearchPartName.KeyDown += TextBox_KeyDown;
            txtSearchType.KeyDown += TextBox_KeyDown;
            txtPartNameJa.KeyDown += TextBox_KeyDown;
            txtPartName.KeyDown += TextBox_KeyDown;
            txtMaker.KeyDown += TextBox_KeyDown;
            txtCustomer.KeyDown += TextBox_KeyDown;
            txtNotes.KeyDown += TextBox_KeyDown;
            txtCustomsStatus.KeyDown += TextBox_KeyDown;
            txtType.KeyDown += TextBox_KeyDown;
            txtInvName.KeyDown += TextBox_KeyDown;
            txtOriginCountry.KeyDown += TextBox_KeyDown;

        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>$Creator$</create>
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
                this.IsRunEditAfterClear = false;
                // シートの初期化
                this.InitializeSheet(this.shtResult);
                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.PartsNameHonyaku_Type;
                shtResult.ColumnHeaders[1].Caption = Resources.PartsNameHonyaku_JPName;
                shtResult.ColumnHeaders[2].Caption = Resources.PartsNameHonyaku_Name;
                shtResult.ColumnHeaders[3].Caption = Resources.PartsNameHonyaku_InvName;
                shtResult.ColumnHeaders[4].Caption = Resources.PartsNameHonyaku_Maker;
                shtResult.ColumnHeaders[5].Caption = Resources.PartsNameHonyaku_OriginCountry;
                shtResult.ColumnHeaders[6].Caption = Resources.PartsNameHonyaku_Customer;
                shtResult.ColumnHeaders[7].Caption = Resources.PartsNameHonyaku_Notes;
                shtResult.ColumnHeaders[8].Caption = Resources.PartsNameHonyaku_CustomsStatus;

                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchPartNameJa.Focus();
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
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtSearchPartNameJa.Text = string.Empty;
                this.txtSearchPartName.Text = string.Empty;
                this.txtSearchType.Text = string.Empty;
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit();
                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode == SystemBase.EditMode.Delete)
                {
                    // 特にチェックする項目なし
                }
                else
                {
                    // 登録情報の品名(和文)チェック
                    if (string.IsNullOrEmpty(this.txtPartNameJa.Text))
                    {
                        // 品名(和文)を入力して下さい。
                        this.ShowMessage("M0100130005");
                        this.txtPartNameJa.Focus();
                        return false;
                    }
                    // 登録情報の品名チェック
                    if (string.IsNullOrEmpty(this.txtPartName.Text))
                    {
                        // 品名を入力して下さい。
                        this.ShowMessage("M0100130006");
                        this.txtPartName.Focus();
                        return false;
                    }
                    // 品名半角チェック
                    if (!string.IsNullOrEmpty(this.txtPartName.Text))
                    {
                        string check = this.txtPartName.Text.Trim();
                        if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(check), @"^[ -~]+$"))
                        {
                            // 品名は半角英数字記号のみ入力してください。
                            this.ShowMessage("M0100130008");
                            this.txtPartName.Focus();
                            return false;
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
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
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
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>$Creator$</create>
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
        /// <create>$Creator$</create>
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
                cond.PartNameJa = this.txtSearchPartNameJa.Text;
                cond.Type = this.txtSearchType.Text;
                cond.PartName = this.txtSearchPartName.Text;
                
                DataSet ds = conn.GetPartNameLike(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当パーツはありません。
                    this.ShowMessage("M0100130004");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_PARTS_NAME.Name;
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
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();
                if (ret)
                {
                    this.DisplayClearEdit();
                    // とりあえず検索
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
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.PartNameJa = this.txtPartNameJa.Text;
                cond.Type = this.txtType.Text;
                cond.PartName = this.txtPartName.Text;
                
                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));

                string errMsgID;
                string[] args;
                if (!conn.InsPartData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100130003")
                    {
                        this.RunSearch();
                        this.ChangeMode(DisplayMode.Insert);
                        this.txtPartNameJa.Focus();
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.Type = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.PARTS_CD);
                cond.PartNameJa = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI_JP);
                cond.PartName = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI);

                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));
                UtilData.SetFld(ds, Def_M_PARTS_NAME.Name, 0, Def_M_PARTS_NAME.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_PARTS_NAME.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.UpdPartData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100130002")
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100130001")
                    {
                        this.RunSearch();
                        var dt = (this.shtResult.DataSource as DataSet).Tables[Def_M_PARTS_NAME.Name];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (cond.Type == ComFunc.GetFld(dt, i, Def_M_PARTS_NAME.PARTS_CD)
                                && cond.PartNameJa == ComFunc.GetFld(dt, i, Def_M_PARTS_NAME.HINMEI_JP))
                            {
                                this.shtResult.TopLeft = new Position(0, i);
                                this.shtResult.ActivePosition = new Position(0, i);
                            }
                        }
                        this.ChangeMode(DisplayMode.Update);
                        this.txtPartNameJa.Focus();
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.Type = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.PARTS_CD);
                cond.PartNameJa = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI_JP);
                cond.PartName = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI);

                DataSet ds = new DataSet();
                ds.Tables.Add(this.SetEditData(this._dtDispData));
                UtilData.SetFld(ds, Def_M_PARTS_NAME.Name, 0, Def_M_PARTS_NAME.VERSION, ComFunc.GetFldToDateTime(this._dtDispData, 0, Def_M_PARTS_NAME.VERSION));

                string errMsgID;
                string[] args;
                if (!conn.DelPartData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100130002")
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
        /// <create>$Creator$</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }


        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>$Creator$</create>
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
                this.txtSearchPartNameJa.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                this.ExportExcel();
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
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                using (var frm = new PartsMeiHonyakuImport(this.UserInfo, ComDefine.TITLE_M0100131))
                {
                    var dialogRet = frm.ShowDialog();
                }
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
        /// <create>$Creator$</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
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
        /// <create>S.Furugo 2018/10/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
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
        /// <create>S.Furugo 2018/10/24</create>
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
        /// <create>S.Furugo 2018/10/24</create>
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

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
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
        /// <create>T.Sakiori 2012/04/05</create>
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

        #region データ選択

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時のチェック
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    return true;
                }

                if (this.shtResult.ActivePosition.Row < 0)
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
        /// <create>Y.Higuchi 2010/08/26</create>
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
                                // モード切替
                                this.ChangeMode(DisplayMode.Insert);
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeSelectPart();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    var conn = new ConnM01();
                    var cond = new CondM01(this.UserInfo);
                    cond.Type = this.shtResult[SHEET_COL_PARTS_CD, this.shtResult.ActivePosition.Row].Text;
                    cond.PartNameJa = this.shtResult[SHEET_COL_HINMEI_JP, this.shtResult.ActivePosition.Row].Text;
                    cond.PartName = this.shtResult[SHEET_COL_HINMEI, this.shtResult.ActivePosition.Row].Text;
                    DataSet ds = conn.GetPartName(cond);

                    if (!ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name))
                    {
                        // 他端末で削除されています。
                        this.ShowMessage("A9999999026");
                        // 消えてるのがあったから取り敢えず検索しとけ
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[Def_M_PARTS_NAME.Name];
                }
                // 表示データ設定
                this.txtPartNameJa.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI_JP);
                this.txtType.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.PARTS_CD);
                this.txtPartName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI);
                this.txtInvName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.HINMEI_INV);
                this.txtMaker.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.MAKER);
                this.txtOriginCountry.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.FREE2);
                this.txtCustomer.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.SUPPLIER);
                this.txtNotes.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.NOTE);
                this.txtCustomsStatus.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PARTS_NAME.CUSTOMS_STATUS);

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }
        #endregion

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>S.Furugo 2018/10/29</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtPartNameJa.Text = string.Empty;
                this.txtType.Text = string.Empty;
                this.txtPartName.Text = string.Empty;
                this.txtInvName.Text = string.Empty;
                this.txtMaker.Text = string.Empty;
                this.txtOriginCountry.Text = string.Empty;
                this.txtCustomer.Text = string.Empty;
                this.txtNotes.Text = string.Empty;
                this.txtCustomsStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region パーツ名翻訳マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名翻訳マスタのデータテーブル作成
        /// </summary>
        /// <returns>パーツ名翻訳マスタのデータテーブル作成</returns>
        /// <create>S.Furugo 2018/10/29</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeSelectPart()
        {
            try
            {
                var dt = new DataTable(Def_M_PARTS_NAME.Name);
                dt.Columns.Add(Def_M_PARTS_NAME.HINMEI_JP, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.PARTS_CD, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.HINMEI, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.HINMEI_INV, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.MAKER, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.FREE2, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.SUPPLIER, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.NOTE, typeof(string));
                dt.Columns.Add(Def_M_PARTS_NAME.CUSTOMS_STATUS, typeof(string));

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
        /// <param name="dt">パーツ名翻訳マスタデータテーブル</param>
        /// <returns></returns>
        /// <create>S.Furugo 2018/10/29</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable SetEditData(DataTable dt)
        {
            try
            {
                DataTable ret = dt.Clone();
                DataRow dr = ret.NewRow();
                dr[Def_M_PARTS_NAME.HINMEI_JP] = this.txtPartNameJa.Text;
                dr[Def_M_PARTS_NAME.PARTS_CD] = this.txtType.Text;
                dr[Def_M_PARTS_NAME.HINMEI] = this.txtPartName.Text;
                dr[Def_M_PARTS_NAME.HINMEI_INV] = this.txtInvName.Text;
                dr[Def_M_PARTS_NAME.MAKER] = this.txtMaker.Text;
                dr[Def_M_PARTS_NAME.FREE2] = this.txtOriginCountry.Text;
                dr[Def_M_PARTS_NAME.SUPPLIER] = this.txtCustomer.Text;
                dr[Def_M_PARTS_NAME.NOTE] = this.txtNotes.Text;
                dr[Def_M_PARTS_NAME.CUSTOMS_STATUS] = this.txtCustomsStatus.Text;

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

        #region Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExportExcel()
        {
            bool ret = false;
            try
            {
                // 全データを出力するため、データ取得
                var conn = new ConnM01();
                var cond = new CondM01(this.UserInfo);
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.UpdateUserName = this.UserInfo.UserName;

                string errMsgID;
                string[] args;

                // Excel出力データ取得
                var dsExcel = conn.GetPartsNameExcelData(cond);
                if (!UtilData.ExistsData(dsExcel, Def_M_PARTS_NAME.Name))
                {
                    // 出力するDataがありません。
                    this.ShowMessage("A0100010004");
                    return false;
                }

                using(var frm = new SaveFileDialog())
	            {
                    frm.Title = Resources.PartsNameHonyaku_sdfExcel_Title;
                    frm.Filter = Resources.PartsNameHonyaku_sdfExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_PARTS_NAME_HONYAKU;
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }

                    // Excel出力処理
                    var dtExport = dsExcel.Tables[Def_M_PARTS_NAME.Name];
                    var export = new ExportPartsName();
                    export.ExportExcel(frm.FileName, dtExport, out errMsgID, out args);
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return ret;
        }

        #endregion

        #region 貼り付け内容文字コード変換

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け内容文字コード変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <create>J.Chen 2024/11/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();

                    byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(clipboardText);
                    string utf8Text = Encoding.GetEncoding("UTF-8").GetString(bytes);

                    // 改行コードを取り除く
                    utf8Text = utf8Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

                    TextBox textBox = sender as TextBox;
                    if (textBox != null)
                    {
                        textBox.SelectedText = utf8Text;
                    }
                }
            }
        }

        #endregion

    }
}
