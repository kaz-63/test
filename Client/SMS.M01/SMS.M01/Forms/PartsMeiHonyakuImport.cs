using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
using System.ComponentModel.Design;
using SystemBase.Util;
using XlsxCreatorHelper;
using System.IO;
using WsConnection.WebRefM01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタ(取込)
    /// </summary>
    /// <create>H.Tajimi 2019/07/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class PartsMeiHonyakuImport : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// Excelの文字属性タイプ
        /// </summary>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ExcelInputAttrType
        {
            AlphaNum = 0,
            WideString = 1,
            Numeric = 2,
        }
        
        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        
        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public PartsMeiHonyakuImport(UserInfo userInfo, string title)
            : base(userInfo, title)
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public PartsMeiHonyakuImport(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tajimi 2019/07/25</create>
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
                this.InitializeSheet(this.shtResult);

                // 画面の編集モード設定
                this.EditMode = SystemBase.EditMode.Insert;
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
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                this.btnSelect.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シートの初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void  InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            sheet.AllowUserToAddRows = false;
            sheet.KeepHighlighted = false;
            sheet.SelectionType = SelectionType.Single;
            sheet.ViewMode = ViewMode.DisplayOnly;

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
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // グリッドのクリア
                this.SheetClear();
                // 明細データ
                this.txtExcel.Text = string.Empty;

                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.btnSelect.Focus();
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
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            try
            {
                this.shtResult.Redraw = false;
                // 最も左上に表示されているセルの設定
                if (0 < this.shtResult.MaxRows)
                {
                    this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
                }
                this.shtResult.DataSource = null;
                this.shtResult.MaxRows = 0;
                this.shtResult.Enabled = false;
            }
            finally
            {
                this.shtResult.Redraw = true;
            }
        }

        #endregion

        #region オールクリア

        /// --------------------------------------------------
        /// <summary>
        /// オールクリア
        /// </summary>
        /// <create>H.Tajimi 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void AllClear()
        {
            // グリッドのクリア
            this.SheetClear();
            // 検索条件のロック解除
            this.grpSearch.Enabled = true;
            // ファンクションボタンの切替
            this.ChangeFunctionButton(false);
            // フォーカス移動
            this.btnSelect.Focus();
        }

        #endregion

        #region ファンクションバーのEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            this.fbrFunction.F01Button.Enabled = isEnabled;
            this.fbrFunction.F06Button.Enabled = isEnabled;
        }

        #endregion

        #region パーツ名翻訳保守のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// パーツ名翻訳保守のデータテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemePartsName()
        {
            var dt = new DataTable(Def_M_PARTS_NAME.Name);
            dt.Columns.Add(Def_M_PARTS_NAME.PARTS_CD, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.HINMEI_JP, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.HINMEI, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.HINMEI_INV, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.MAKER, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.FREE2, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.SUPPLIER, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.NOTE, typeof(string));
            dt.Columns.Add(Def_M_PARTS_NAME.CUSTOMS_STATUS, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.CREATE_DATE, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.CREATE_USER_ID, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.CREATE_USER_NAME, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.UPDATE_DATE, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.UPDATE_USER_ID, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.UPDATE_USER_NAME, typeof(string));
            //dt.Columns.Add(Def_M_PARTS_NAME.VERSION, typeof(string));
            return dt;
        }

        #endregion

        #region 取込処理

        #region 取込処理制御部

        /// --------------------------------------------------
        /// <summary>
        /// 取込処理制御部
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExecuteImport()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 明細データ(Excel)の入力チェック
                if (string.IsNullOrEmpty(this.txtExcel.Text))
                {
                    // パーツ名翻訳Data(Excel)のFilesが選択されていません。
                    this.ShowMessage("M0100131001");
                    return false;
                }
                // ファイル存在チェック
                if (!File.Exists(this.txtExcel.Text))
                {
                    // パーツ名翻訳Data(Excel)のFilesが存在しません。
                    this.ShowMessage("M0100131002");
                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;
                var dt = this.GetSchemePartsName();
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = false;

                // Excelファイル読込
                ret = this.GetExcelDataXlsx(this.txtExcel.Text, dt, dtMessage);
                if (0 < dt.Rows.Count)
                {
                    ret = true;
                }
                this.shtResult.DataSource = dt;
                if (0 < dtMessage.Rows.Count)
                {
                    // 取込出来ないDataがありました。\r\nErrorがあった行は表示されていません。\r\n※Errorの一覧は右ClickでClipboardにCopyできます。
                    this.ShowMultiMessage(dtMessage, "M0100131003");
                }
                else
                {
                    this.fbrFunction.F01Button.Enabled = true;
                }
                this.fbrFunction.F06Button.Enabled = true;

                return ret;
            }
            catch (Exception ex)
            {
                this.shtResult.Redraw = true;
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        #endregion

        #region 取込実行部

        /// --------------------------------------------------
        /// <summary>
        /// 取込実行部(xlsx)
        /// </summary>
        /// <param name="filePath">Excelファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXlsx(string filePath, DataTable dt, DataTable dtMessage)
        {
            using (var xlsx = new XlsxCreator())
            {
                try
                {
                    xlsx.ReadBook(filePath);
                    return this.CheckExcelData(dt, dtMessage, xlsx);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    xlsx.CloseBook(false);
                }
            }
        }

        #endregion

        #region チェック処理

        #region チェック処理制御部

        /// --------------------------------------------------
        /// <summary>
        /// チェック処理制御部
        /// </summary>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="xlsx">XlsxCreator</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update>J.Chen 2023/02/16 取込上限桁数変更</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckExcelData(DataTable dt, DataTable dtMessage, XlsxCreator xlsx)
        {
            int maxRow = xlsx.MaxData(xlMaxEndPoint.xarMaxPoint).Height;
            if (maxRow < 1)
            {
                // パーツ名翻訳保守(Excel)のFilesにDataがありません。
                this.ShowMessage("M0100131004");
                return false;
            }

            bool ret = true;
            string itemName = string.Empty;
            string field = string.Empty;
            int col = 0;
            int checkLen = 0;

            // 1行目はヘッダなので2行目から評価を行う
            for (int rowIndex = 1; rowIndex <= maxRow; rowIndex++)
            {
                DataRow dr = dt.NewRow();
                bool isAddData = true;

                // 型式のチェック
                itemName = Resources.PartsNameHonyaku_Type;
                col = 0;
                checkLen = 100;
                field = Def_M_PARTS_NAME.PARTS_CD;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // 品名(和文)のチェック
                itemName = Resources.PartsNameHonyaku_JPName;
                col = 1;
                checkLen = 100;
                field = Def_M_PARTS_NAME.HINMEI_JP;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, true, true))
                {
                    isAddData = false;
                }
                // 品名のチェック
                itemName = Resources.PartsNameHonyaku_Name;
                col = 2;
                checkLen = 100;
                field = Def_M_PARTS_NAME.HINMEI;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, true, true))
                {
                    isAddData = false;
                }
                // INV付加名のチェック
                itemName = Resources.PartsNameHonyaku_InvName;
                col = 3;
                checkLen = 40;
                field = Def_M_PARTS_NAME.HINMEI_INV;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // Makerのチェック
                itemName = Resources.PartsNameHonyaku_Maker;
                col = 4;
                checkLen = 40;
                field = Def_M_PARTS_NAME.MAKER;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // 原産国のチェック
                itemName = Resources.PartsNameHonyaku_OriginCountry;
                col = 5;
                checkLen = 30;
                field = Def_M_PARTS_NAME.FREE2;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // 取引先のチェック
                itemName = Resources.PartsNameHonyaku_Customer;
                col = 6;
                checkLen = 60;
                field = Def_M_PARTS_NAME.SUPPLIER;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // 備考のチェック
                itemName = Resources.PartsNameHonyaku_Notes;
                col = 7;
                checkLen = 60;
                field = Def_M_PARTS_NAME.NOTE;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }
                // 通関確認状態のチェック
                itemName = Resources.PartsNameHonyaku_CustomsStatus;
                col = 8;
                checkLen = 60;
                field = Def_M_PARTS_NAME.CUSTOMS_STATUS;
                if (!this.CheckAndSetExcelData(xlsx.Pos(col, rowIndex).Str, rowIndex, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true))
                {
                    isAddData = false;
                }

                if (isAddData)
                {
                    var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                    if (!string.IsNullOrEmpty(partsCd))
                    {
                        if (dt.AsEnumerable().Any(x => ComFunc.GetFld(x, Def_M_PARTS_NAME.PARTS_CD) == partsCd))
                        {
                            // {0}行目の型式[{1}]は既に存在します。
                            ComFunc.AddMultiMessage(dtMessage, "M0100131010", this.GetRowIndex(rowIndex), partsCd);
                            ret = false;
                            continue;
                        }
                    }
                    var hinmeiJP = ComFunc.GetFld(dr, Def_M_PARTS_NAME.HINMEI_JP);
                    if (dt.AsEnumerable().Any(x => ComFunc.GetFld(x, Def_M_PARTS_NAME.PARTS_CD) == partsCd
                                                && ComFunc.GetFld(x, Def_M_PARTS_NAME.HINMEI_JP) == hinmeiJP))
                    {
                        // {0}行目の型式[{1}]/品名(和文)[{2}]は既に存在します。
                        ComFunc.AddMultiMessage(dtMessage, "M0100131011", this.GetRowIndex(rowIndex), partsCd, hinmeiJP);
                        ret = false;
                        continue;
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    ret = false;
                }
            }
            // 表示データがない場合は失敗とする。
            if (dt.Rows.Count == 0)
            {
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// セルの内容をチェックし、エラーがなければDataRowの指定フィールドに値を設定
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="dr">DataRow</param>
        /// <param name="field">フィールド名</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="isNecessary">必須項目かどうか</param>
        /// <param name="isCheckLen">長さチェックが必要かどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetExcelData(string target, int rowIndex, DataRow dr, string field, int checkLen, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary, bool isCheckLen)
        {
            object retVal = DBNull.Value;
            bool ret = true;
            if (!this.CheckByteLength(target, rowIndex, ref retVal, checkLen, itemName, dtMessage, isString, attrType))
            {
                ret = false;
            }
            if (!CheckRegulation(target, rowIndex, itemName, attrType, dtMessage))
            {
                ret = false;
            }
            if (isNecessary)
            {
                // 必須チェック
                if (string.IsNullOrEmpty(target))
                {
                    // {0}行目の{1}が入力されていません。
                    ComFunc.AddMultiMessage(dtMessage, "M0100131005", this.GetRowIndex(rowIndex), itemName);
                    ret = false;
                }
            }
            if (ret)
            {
                dr[field] = retVal;
            }
            return ret;
        }

        #endregion

        #region 最大バイトサイズチェック

        /// --------------------------------------------------
        /// <summary>
        /// 最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="attrType">文字属性</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isString, ExcelInputAttrType attrType)
        {
            if (isString)
            {
                return this.CheckAndSetStringByteLength(target, rowIndex, ref value, checkLen, itemName, dtMessage);
            }
            else
            {
                return this.CheckAndSetIntLength(target, rowIndex, ref value, checkLen, itemName, dtMessage);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 文字列の最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetStringByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage)
        {
            if (checkLen < UtilString.GetByteCount(target))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "M0100131006", this.GetRowIndex(rowIndex), itemName);
                return false;
            }
            if (!string.IsNullOrEmpty(target))
            {
                value = target;
            }
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 数値の最大バイトサイズチェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetIntLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage)
        {
            bool isNullOrEmpty = false;
            if (string.IsNullOrEmpty(target))
            {
                isNullOrEmpty = true;
                target = "0";
            }
            string[] number = target.Split('.');
            string decimalsStr = string.Empty;
            if (1 < number.Length)
            {
                decimalsStr = number[1];
            }
            if (0 < UtilString.GetByteCount(decimalsStr))
            {
                // {0}行目の{1}が整数値以外が入力されています。
                ComFunc.AddMultiMessage(dtMessage, "M0100131007", this.GetRowIndex(rowIndex), itemName);
                return false;
            }
            int result;
            if (!Int32.TryParse(target, System.Globalization.NumberStyles.Number, null, out result))
            {
                // {0}行目の{1}が数値に変換できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "M0100131008", this.GetRowIndex(rowIndex), itemName);
                return false;
            }
            string checkStr = result.ToString();
            if (checkLen < UtilString.GetByteCount(checkStr))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "M0100131006", this.GetRowIndex(rowIndex), itemName);
                return false;
            }
            if (!isNullOrEmpty)
            {
                value = result;
            }
            return true;
        }

        #endregion

        #region 入力値の文字属性チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力値の文字属性チェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2019/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckRegulation(string target, int rowIndex, string itemName, ExcelInputAttrType attrType, DataTable dtMessage)
        {
            ComRegulation regulation = new ComRegulation();
            bool isUse = false;
            string targetType = string.Empty;
            string msgExtend = string.Empty;

            switch (attrType)
            {
                case ExcelInputAttrType.AlphaNum:
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_LOW;
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_UP;
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    targetType += ComRegulation.REGULATION_NARROW_SIGN;
                    targetType += ComRegulation.REGULATION_NARROW_SPACE;
                    isUse = true;
                    msgExtend = Resources.PartsNameHonyaku_Singlebyte;
                    break;
                case ExcelInputAttrType.WideString:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    msgExtend = Resources.PartsNameHonyaku_PlatformDependentChar;
                    break;
                case ExcelInputAttrType.Numeric:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                    isUse = true;
                    msgExtend = Resources.PartsNameHonyaku_Numeric;
                    break;
                default:
                    break;
            }
            if (!regulation.CheckRegularString(target, targetType, isUse, false))
            {
                // {0}行目の{1}が登録できない文字を含んでいます。{2}
                ComFunc.AddMultiMessage(dtMessage, "M0100131009", this.GetRowIndex(rowIndex), itemName, msgExtend);
                return false;
            }
            return true;
        }

        #endregion

        #endregion

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            var ret = base.RunEdit();
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
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                var cond = new CondM01(this.UserInfo);
                var conn = new ConnM01();
                var ds = new DataSet();
                var dt = this.shtResult.DataSource as DataTable;
                if (dt == null) return false;
                ds.Tables.Add(dt.Copy());

                string errMsgID;
                string[] args;
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                if (!conn.InsImportedPartsData(cond, ds, ref dtMessage, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                if (UtilData.ExistsData(dtMessage))
                {
                    // 保存に失敗しました。
                    this.ShowMultiMessage(dtMessage, "A9999999013");
                    return false;
                }
                else
                {
                    // 保存しました。
                    this.ShowMessage("A9999999045");
                    this.AllClear();
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

        #region 行取得

        /// --------------------------------------------------
        /// <summary>
        /// Excel内の行位置取得
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetRowIndex(int rowIndex)
        {
            return (rowIndex + 1).ToString();
        }

        #endregion

        #region イベント

        #region ファンクションボタン

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/07/26</create>
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
        /// <create>H.Tajimi 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.AllClear();
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
        /// <create>H.Tajimi 2019/07/25</create>
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
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタンクリック

        #region 選択ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var frm = new OpenFileDialog())
                {
                    frm.Filter = Resources.PartsNameHonyaku_ofdExcel_Filter;
                    frm.Title = Resources.PartsNameHonyaku_ofdExcel_Title;
                    frm.FileName = this.txtExcel.Text;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.txtExcel.Text = frm.FileName;
                    }
                }
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
        /// <create>H.Tajimi 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // グリッドクリア
                this.SheetClear();

                // 取込処理
                if (!this.ExecuteImport())
                {
                    return;
                }
                
                // 検索条件のロック
                this.grpSearch.Enabled = false;
                this.shtResult.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
