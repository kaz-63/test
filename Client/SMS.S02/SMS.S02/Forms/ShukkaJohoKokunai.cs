using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefAttachFile;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using SMS.E01;
using WsConnection.WebRefS02;
using SMS.S02.Forms;
using SMS.S02.Properties;

namespace SMS.S02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <create>S.Furugo 2018/12/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaJohoKokunai : SystemBase.Forms.CustomOrderForm
    {
        #region enum

        /// --------------------------------------------------
        /// <summary>
        /// DB取得時のフィールドカラム番号
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        enum DB_FIELD
        {
            UNSOKAISHA_NAME = 0,
            INVOICE_NO,
            SHUKKA_FLAG,
            BUKKEN_NAME,
            SHIP,
            AR_NO,
            ECS_NO,
            ATTN,
            CONSIGNEE,
            CONSIGNEE_ATTN,
            DELIVERYTO,
            DELIVERYTO_ATTN,
            INTERNAL_PO_NO,
            TRADE_TERMS_FLAG,
            TRADE_TERMS_ATTR,
            SUBJECT,
            VERSION,
            ESTIMATE_FLAG,
            PACKING_NO,
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートインデックス
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        enum SHEET_COL
        {
            PRINT = 0,
            UNSOKAISYA,
            INVOICE,
            AR,
            ITEM,
            SHIP,
            AR_NO,
            ECS_NO,
            ATTN,
            LINE,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 帳票保存ダイアログのファイルフィルター
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string FILE_FILTER = "Excel Files(*.xlsx)|*.xlsx";
        /// --------------------------------------------------
        /// <summary>
        /// シートフィールド名：印刷チェックボックス
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string DB_FLD_PRINT = "PRINT";
        /// --------------------------------------------------
        /// <summary>
        /// シートフィールド名：行数
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string DB_FLD_LINE = "LINE";
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int TOPLEFT_COL = 1;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// シート更新カウント
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _SheetRedrawCount = 0;
        /// --------------------------------------------------
        /// <summary>
        /// シート更新フラグ
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _ForceFlag = false;
        /// --------------------------------------------------
        /// <summary>
        /// 一覧表示用データ退避
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSheet = null;
        /// --------------------------------------------------
        /// <summary>
        /// 更新用テーブル
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtUpdTable = null;
        /// --------------------------------------------------
        /// <summary>
        /// 帳票テンプレート
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _DownloadFileList = new string[] { ComDefine.EXCEL_FILE_TEMP_INVOICE
                                                          , ComDefine.EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT
                                                          , ComDefine.EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT};
        /// --------------------------------------------------
        /// <summary>
        /// 帳票保存パス
        /// </summary>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _SaveFilePath = string.Empty;
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
        /// <create>S.Furugo 2018/12/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaJohoKokunai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>S.Furugo 2018/12/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtShipping);
                this.shtShipping.DataBindingMode = DataBindingMode.Tight;

                // シートのタイトルを設定
                int i = 0;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Print;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Unsokaisha;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Invoice;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Ar;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Item;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Ship;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_ArNo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_EcsNo;
                this.shtShipping.ColumnHeaders[i++].Caption = Resources.ShukkaJohoKokunai_Attn;
                // シートのEnterキー挙動変更
                this.shtShipping.ShortCuts.Add(Keys.Enter, new[] { KeyAction.NextRow });
                this.shtShipping.ShortCuts.Add(Keys.Enter | Keys.Shift, new[] { KeyAction.PrevRow });

                this.EditMode = SystemBase.EditMode.None;
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
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                dtpShukkaDateFrom.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 出荷日のクリア
                this.dtpShukkaDateFrom.Value = DateTime.Today;

                // グリッドのクリア
                this.SheetClear();

                // フォーカス移動
                this.dtpShukkaDateFrom.Focus();

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtShipping.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtShipping.MaxRows)
            {
                this.shtShipping.TopLeft = new Position(TOPLEFT_COL, this.shtShipping.TopLeft.Row);
            }
            this.shtShipping.DataSource = null;
            this.shtShipping.AllowUserToAddRows = false;
            this.shtShipping.MaxRows = 0;
            this.shtShipping.Enabled = false;
            this.shtShipping.Redraw = true;

            // 保持データのクリア
            _dtSheet = null;
        }

        #endregion

        #region ファンクションバーのEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            this.fbrFunction.F01Button.Enabled = isEnabled;
            this.fbrFunction.F06Button.Enabled = isEnabled;
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
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.ClearMessage();

                // テンプレートダウンロード
                if (!this.ExcelTemplateFileDownload())
                {
                    // TemplateのDownloadに失敗しました。
                    this.ShowMessage("A7777777003");
                    return;
                }

                this.SuspendLayout();
                this.SetSheetRedraw(false, true);
                this.EditMode = SystemBase.EditMode.Insert;
                base.fbrFunction_F01Button_Click(sender, e);
                this.EditMode = SystemBase.EditMode.None;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.SetSheetRedraw(true, true);
                this.ResumeLayout();
                Cursor.Current = Cursors.Arrow;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                this.SuspendLayout();
                this.SetSheetRedraw(false, true);

                // グリッドのクリア
                this.SheetClear();

                // フラグのクリア

                // 保持データのクリア

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.SetSheetRedraw(true, true);
                this.ResumeLayout();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.SuspendLayout();

                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2018/12/20</create>
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
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.SuspendLayout();

                string ShukkaDate = this.dtpShukkaDateFrom.Text;
                //string HakkouFlag = (this.cboHakkoFlag.SelectedValue == null ? HAKKO_FLAG.DEFAULT_VALUE1 : this.cboHakkoFlag.SelectedValue.ToString());
                string HakkouFlag = HAKKO_FLAG.ALL_VALUE1;

                this.ClearMessage();

                // グリッドクリア
                this.SheetClear();

                // 検索条件のロック
                this.grpSearch.Enabled = false;

                DataTable dtNisugata = this.GetNisugataList(ShukkaDate, HakkouFlag);
                if (dtNisugata == null || (dtNisugata != null && dtNisugata.Rows.Count <= 0))
                {
                    // 該当する荷姿情報はありません。
                    this.ShowMessage("S0200040001");
                    // 検索条件のアンロック
                    this.grpSearch.Enabled = true;
                    // フォーカス
                    this.dtpShukkaDateFrom.Focus();
                    return;
                }
                dtNisugata.Columns.Add(DB_FLD_PRINT, typeof(decimal));//印刷チェックボックス用FIELD
                dtNisugata.Columns.Add(DB_FLD_LINE, typeof(decimal));//行数用FIELD
                _dtSheet = dtNisugata.Copy();//退避
                this.shtShipping.DataSource = dtNisugata;
                this.shtShipping.Enabled = true;

                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);

                // 先頭行のチェックボックスを全て有効にする
                for (int i = 0; i < this.shtShipping.Rows.Count; i++ )
                {
                    this.shtShipping[(int)SHEET_COL.PRINT, i].Value = 1;
                }

                // フォーカス
                this.shtShipping.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
                Cursor.Current = Cursors.Arrow;
            }
        }

        #endregion

        #endregion

        #region シート操作

        /// --------------------------------------------------
        /// <summary>
        /// シートの更新設定
        /// </summary>
        /// <param name="setflag"></param>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetSheetRedraw(bool setflag, bool forceflag)
        {
//#if DEBUG
//            var callingMethod = new System.Diagnostics.StackTrace(1, false).GetFrame(0).GetMethod();
//            string dbgmsg = "SetSheetRedraw() [START] Set:" + setflag.ToString().PadLeft(5, ' ') + " force:" + forceflag.ToString().PadLeft(5, ' ') + " ForceFlag:" + _ForceFlag.ToString().PadLeft(5, ' ') + " Count:" + _SheetRedrawCount.ToString().PadLeft(3, ' ') + " Redeaw:" + this.shtShipping.Redraw.ToString().PadLeft(5, ' ') + " Call:" + callingMethod.Name;
//            System.Diagnostics.Debug.WriteLine(dbgmsg);
//#endif
            if (forceflag)
            {
                if (setflag)
                    _SheetRedrawCount = 0;
                else
                    _SheetRedrawCount = 99;
                this.shtShipping.Redraw = setflag;
                _ForceFlag = !setflag;
            }
            else if (!_ForceFlag)
            {
                if (setflag)
                {
                    if (_SheetRedrawCount <= 1)
                    {
                        this.shtShipping.Redraw = true;
                    }
                    _SheetRedrawCount--;
                }
                else
                {
                    if (_SheetRedrawCount == 0)
                    {
                        this.shtShipping.Redraw = false;
                    }
                    _SheetRedrawCount++;
                }
                if (_SheetRedrawCount < 0) _SheetRedrawCount = 0;
            }
//#if DEBUG
//            dbgmsg = "SetSheetRedraw() [ END ] Set:" + setflag.ToString().PadLeft(5, ' ') + " force:" + forceflag.ToString().PadLeft(5, ' ') + " ForceFlag:" + _ForceFlag.ToString().PadLeft(5, ' ') + " Count:" + _SheetRedrawCount.ToString().PadLeft(3, ' ') + " Redeaw:" + this.shtShipping.Redraw.ToString().PadLeft(5, ' ') + " Call:" + callingMethod.Name;
//            System.Diagnostics.Debug.WriteLine(dbgmsg);
//#endif
        }

        #endregion

        #region データ取得

        /// --------------------------------------------------
        /// <summary>
        /// 荷姿情報取得
        /// </summary>
        /// <param name="ShukkaDate">出荷日</param>
        /// <param name="HakkouFlag">状態</param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetNisugataList(string ShukkaDate, string HakkouFlag)
        {
            try
            {
                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                cond.Kokunaigai = KOKUNAI_GAI_FLAG.NAI_VALUE1;
                cond.ShukkaDate = ShukkaDate;
                if (HakkouFlag != HAKKO_FLAG.ALL_VALUE1)
                {
                    cond.HakkouFlag = HakkouFlag;
                }
                DataSet ds = conn.GetNisugata(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_PACKING.Name))
                {
                    // 取得NG
                    return null;
                }
                return ds.Tables[Def_T_PACKING.Name].Copy();

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion



        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode != SystemBase.EditMode.Insert) return ret;

                // 初期化
                _SaveFilePath = string.Empty;
                _dtUpdTable = null;

                string File_Invoice = string.Empty;
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                int PrintCount = 0;
                for (int i = 0; i < this.shtShipping.Rows.Count; i++)
                {
                    int PrintFlag = (this.shtShipping[(int)SHEET_COL.PRINT, i].Value == null ? 0 : int.Parse(this.shtShipping[(int)SHEET_COL.PRINT, i].Value.ToString()));
                    if (PrintFlag == 1)
                    {
                        PrintCount++;

                        this.shtShipping[(int)SHEET_COL.LINE, i].Value = (i + 1);

                        if (string.IsNullOrEmpty(File_Invoice))
                        {   // 保存ファイル用に先頭のInvoiceNoを退避する
                            File_Invoice = this.shtShipping[(int)SHEET_COL.INVOICE, i].Value.ToString();
                        }
                    }
                }

                if (PrintCount > 0)
                {
                    // シートのデータをデータソースに反映する。
                    this.shtShipping.UpdateData();
                    
                    // 対象のデータを抜き出しサーバー側のチェックを実施する
                    DataTable dtSheet = (this.shtShipping.DataSource as DataTable).Copy();
                    dtSheet.AcceptChanges();
                    DataTable dt = dtSheet.Clone();
                    foreach (DataRow dr in dtSheet.Rows)
                    {
                        string tmpFlag = ComFunc.GetFld(dr, DB_FLD_PRINT);
                        int PrintFlag = (tmpFlag == string.Empty ? 0 : int.Parse(tmpFlag));
                        if (PrintFlag == 1)
                        {
                            DataRow dr_dst = dt.NewRow();
                            dr_dst.ItemArray = dr.ItemArray;
                            dt.Rows.Add(dr_dst);
                        }
                    }
                    CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                    ConnS02 conn = new ConnS02();
                    DataTable dtMessageS = conn.CheckInvoice(cond, dt);

                    // エラーメッセージのマージ
                    DataTable dtMessageM = ComFunc.GetSchemeMultiMessage();
                    dtMessageM.Merge(dtMessage);
                    dtMessageM.Merge(dtMessageS);

                    if (dtMessageM.Rows.Count > 0)
                    {
                        // 入力値にエラーがあります。\r\n下記のエラーを修正し再度作成を実施して下さい。
                        this.ShowMultiMessage(dtMessageM, "S0200040002");
                        ret = false;
                    }
                    else
                    {
                        // 保存フォルダの選択
                        string invoice_filename = string.Format(ComDefine.EXCEL_FILE_PARTSLISTDETAIL, File_Invoice);
                        string FilePath = string.Empty;
                        if (this.GetSaveFilepath(invoice_filename, false, out FilePath))
                        {
                            ret = true;

                            //保存パスを退避
                            _SaveFilePath = (Path.GetDirectoryName(FilePath) + "\\");

                            // 更新用テーブルにデータ退避
                            _dtUpdTable = dt.Copy();
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                }
                else
                {
                    // 作成対象が選択されていません
                    this.ShowMessage("S0200040009");
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Nakata 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            this.IsRunEditAfterClear = false;
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドのクリア
                    this.SheetClear();
                    // 検索条件のロック解除
                    this.grpSearch.Enabled = true;
                    // ファンクションボタンの切替
                    this.ChangeFunctionButton(false);
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
        /// <create>T.Nakata 2018/12/20</create>
        /// <update>D.Okumura 2020/10/06 ファイル名変更に伴う出力エラーを修正</update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                if (_dtUpdTable == null) return false;

                CondS02 cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                ConnS02 conn = new ConnS02();
                cond.ShukkaDate = this.dtpShukkaDateFrom.Text;

                // 帳票出力
                foreach (DataRow dr in _dtUpdTable.Rows)
                {
                    //----- データ収集 -----
                    // テーブルA取得
                    cond.PackingNo = ComFunc.GetFld(dr, Def_T_PACKING.PACKING_NO);
                    cond.InvoiceNo = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);
                    cond.ConsignCd = ComFunc.GetFld(dr, Def_T_PACKING.CONSIGN_CD);
                    cond.DeliverCd = ComFunc.GetFld(dr, Def_T_PACKING.DELIVER_CD);
                    //DataSet dsDataA = conn.GetTyouhyouDataA(cond);
                    //if (dsDataA == null)
                    //{
                    //    // データ取得エラー
                    //    continue;
                    //}
                    //DataTable dtDataA = (dsDataA.Tables[ComDefine.DTTBL_SIPPING_A]).Copy();

                    //// テーブルB取得
                    //DataSet dsDataB = conn.GetTyouhyouDataB(cond);
                    //if (dsDataB == null)
                    //{
                    //    // データ取得エラー
                    //    continue;
                    //}
                    //DataTable dtDataB = (dsDataB.Tables[ComDefine.DTTBL_SIPPING_B]).Copy();

                    //// テーブルC取得
                    //DataSet dsDataC = conn.GetTyouhyouDataC(cond);
                    //if (dsDataC == null)
                    //{
                    //    // データ取得エラー
                    //    continue;
                    //}
                    //DataTable dtDataC = (dsDataC.Tables[ComDefine.DTTBL_SIPPING_C]).Copy();

                    // テーブルD取得
                    DataSet dsDataD = conn.GetTyouhyouDataD(cond);
                    if (dsDataD == null)
                    {
                        // データ取得エラー
                        continue;
                    }
                    DataTable dtDataD = (dsDataD.Tables[ComDefine.DTTBL_SIPPING_D]).Copy();

                    // テーブルE取得
                    DataSet dsDataE = conn.GetTyouhyouDataE(cond);
                    if (dsDataE == null)
                    {
                        // データ取得エラー
                        continue;
                    }
                    DataTable dtDataE = (dsDataE.Tables[ComDefine.DTTBL_SIPPING_E]).Copy();

                    //// テーブルF取得
                    //DataSet dsDataF = conn.GetTyouhyouDataF(cond);
                    //if (dsDataF == null)
                    //{
                    //    // データ取得エラー
                    //    continue;
                    //}
                    //DataTable dtDataF = (dsDataF.Tables[ComDefine.DTTBL_SIPPING_F]).Copy();

                    // テーブルG取得
                    cond.UnsokaishaNo = ComFunc.GetFld(dr, Def_T_PACKING.UNSOKAISHA_CD);
                    DataSet dsDataG = conn.GetTyouhyouDataG(cond);
                    if (dsDataG == null)
                    {
                        // データ取得エラー
                        continue;
                    }
                    DataTable dtDataG = (dsDataG.Tables[ComDefine.DTTBL_SIPPING_G]).Copy();

                    // テーブルH取得
                    DataSet dsDataH = conn.GetTyouhyouDataH(cond);
                    if (dsDataH == null)
                    {
                        // データ取得エラー
                        continue;
                    }
                    DataTable dtDataH = (dsDataH.Tables[ComDefine.DTTBL_SIPPING_H]).Copy();

                    // テーブル再構築
                    DataSet dsDataGr1 = new DataSet();
                    dsDataGr1.Tables.Add(dtDataH.Copy());

                    DataSet dsDataGr2 = new DataSet();
                    dsDataGr2.Tables.Add(dtDataD.Copy());

                    DataSet dsDataGr3 = new DataSet();
                    dsDataGr3.Tables.Add(dtDataE.Copy());

                    //----- 帳票出力 -----
                    //===================================================================================================
                    //_SaveFilePath:保存パス
                    //=======================
                    //EXCEL_FILE_TEMP_INVOICE:INVOICEのExcelテンプレートファイル名
                    //EXCEL_FILE_TEMP_QUANTITY_OF_PARTSSHIPMENT:まとめ表パーツ便物量実績のExcelテンプレートファイル名
                    //EXCEL_FILE_TEMP_SUMMARY_OF_PARTSSHIPMENT:まとめ表パーツ便出荷のExcelテンプレートファイル名
                    //=======================
                    //EXCEL_FILE_PARTSLISTDETAIL:INVOICE(国内)のExcelファイル名
                    //EXCEL_FILE_QUANTITY_OF_PARTSSHIPMENT:まとめ表パーツ便物量実績のExcelファイル名
                    //EXCEL_FILE_SUMMARY_OF_PARTSSHIPMENT:まとめ表パーツ便出荷のExcelファイル名
                    //===================================================================================================
                    string invoiceNo = ComFunc.GetFld(dr, Def_T_PACKING.INVOICE_NO);
                    object[] fileNameParams = new object[] { 
                        //0:宛先
                        "",
                        //1:Invoice
                        invoiceNo,
                        //2:Item
                        "",
                        //3:SHIP
                        "",
                        //4:AR_NO
                        "",
                        //5:出荷日
                        "",
                    };
                    string[] SaveFile = new string[] { string.Format(ComDefine.EXCEL_FILE_PARTSLISTDETAIL, invoiceNo),
                                                       string.Format(ComDefine.EXCEL_FILE_SUMMARY_OF_PARTSSHIPMENT, fileNameParams),
                                                       string.Format(ComDefine.EXCEL_FILE_QUANTITY_OF_PARTSSHIPMENT, fileNameParams)};

                    // INVOICE
                    ExportShippingDocument exportSipping = new ExportShippingDocument();
                    string Excel_msgID;
                    string[] Excel_args;
                    exportSipping.ExportExcel(_SaveFilePath + SaveFile[0], dsDataGr1, false, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }

                    // パーツ便出荷まとめ
                    ExportPartsMatomeList exportpartsMatomeList = new ExportPartsMatomeList();
                    exportpartsMatomeList.ExportExcel(_SaveFilePath + SaveFile[1], dsDataGr2, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }

                    // パーツ便出荷物量実績
                    ExportPartsJissekiList exportpartsJissekiList = new ExportPartsJissekiList();
                    exportpartsJissekiList.ExportExcel(_SaveFilePath + SaveFile[2], dsDataGr3, out Excel_msgID, out Excel_args);
                    if (!string.IsNullOrEmpty(Excel_msgID))
                    {
                        this.ShowMessage(Excel_msgID, Excel_args);
                        return false;
                    }
                }


                // DB更新
                string errMsgID;
                string[] args;
                cond = new CondS02(this.UserInfo, string.Empty, string.Empty, string.Empty);
                conn = new ConnS02();
                cond.ShukkaDate = this.dtpShukkaDateFrom.Text;
                if (!conn.UpdShippingData(cond, _dtUpdTable, out errMsgID, out args))
                {
                    // エラーメッセージ出力
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

        #region ファイルダウンロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update>K.Tsutsumi 2020/04/26 複数のファイルを関係ない固定のファイル名で保存している不具合を修正</update>
        /// --------------------------------------------------
        private bool ExcelTemplateFileDownload()
        {
            bool ret = false;
            try
            {
                if (_DownloadFileList.Length <= 0) return false;

                var conn = new ConnAttachFile();
                foreach (string FileName in _DownloadFileList)
                {
                    var dlPackage = new FileDownloadPackage();
                    dlPackage.FileName = FileName;
                    dlPackage.FileType = FileType.Template;

                    // TemplateファイルのDL
                    var retFile = conn.FileDownload(dlPackage);
                    if (retFile.IsExistsFile)
                    {
                        if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                        {
                            Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                        }
                        using (var fs = new FileStream(Path.Combine(ComDefine.DOWNLOAD_DIR, FileName), FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(retFile.FileData, 0, retFile.FileData.Length);
                            fs.Close();
                        }
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                        break;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル保存ダイアログ

        /// --------------------------------------------------
        /// <summary>
        /// ファイル保存ダイアログ
        /// </summary>
        /// <param name="SetFilename">デフォルト表示ファイル名</param>
        /// <param name="noDialog">ダイアログ表示有無</param>
        /// <param name="RetFileName">選択されたファイルパス</param>
        /// <returns></returns>
        /// <create>T.Nakata 2018/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetSaveFilepath(string SetFilename, bool noDialog, out string RetFileName)
        {
            RetFileName = string.Empty;

            // 保存パス指定
            SaveFileDialog frm = new SaveFileDialog();
            if (!noDialog)
            {
                frm.Filter = FILE_FILTER;
                frm.FileName = SetFilename;
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(frm.FileName))
                {
                    return false;
                }
            }
            else
            {
                frm.FileName = Path.Combine(ComDefine.DOWNLOAD_DIR, SetFilename);
                if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                {
                    Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                }
            }
            RetFileName = frm.FileName;
            return true;
        }

        #endregion
 
    }
}
