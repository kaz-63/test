using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Commons;
using ElTabelleHelper;
using GrapeCity.Win.ElTabelle;
using GrapeCity.Win.ElTabelle.Editors;
using MultiRowTabelle;
using SMS.E01;
using SMS.P02.Forms;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefA01;
using WsConnection.WebRefCommon;
using DSWUtil;
using SMS.A01.Properties;
using XlsxCreatorHelper;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ情報員数表取込
    /// </summary>
    /// <create>Y.Nakasato 2019/07/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuInzuhyoImport : SystemBase.Forms.CustomOrderForm
    {

        #region 定義

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 1;

        /// --------------------------------------------------
        /// <summary>
        /// データ開始行
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_DATA_START_IDX = 1;

        /// --------------------------------------------------
        /// <summary>
        /// 表示用処理フラグ名
        /// </summary>
        /// <create>D.Okumura 2019/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string SHEET_FLAG_NAME = "FLAG_NAME";

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// Excelの文字属性タイプ
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ExcelInputAttrType
        {
            Flag,
            Kishu,
            KishuOya,
            KishuKo,
            GoukiName,
        }

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 納入先CD
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先名
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiName = string.Empty;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">ウィンドウタイトル</param>
        /// <param name="txtNonyusakiName"></param>
        /// <param name="bukkenNo"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuInzuhyoImport(UserInfo userInfo, string title, string txtNonyusakiName, string NonyusakiCd)
            : base(userInfo, title)
        {
            InitializeComponent();
            this._nonyusakiCd = NonyusakiCd;
            this._nonyusakiName = txtNonyusakiName;
        }

        #endregion

        #region 初期化

        #region InitializeLoadControl

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // 更新モード固定
                this.EditMode = SystemBase.EditMode.Insert;

                // シートの初期化
                this.InitializeSheet(shtMeisai);
                shtMeisai.MultiRowAllowUserToAddRows = false;
                this.SheetClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region InitializeShownControl

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtNonyusakiName.Text = this._nonyusakiName;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// Sheet初期化
        /// </summary>
        /// <param name="sheet"></param>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                sheet.EditType = EditType.ReadOnly;
                sheet.AllowUserToAddRows = false;

                // シートのタイトルを設定
                sheet.ColumnHeaders[0].Caption = Resources.ShinchokuInzuhyoImport_h_flag;
                sheet.ColumnHeaders[1].Caption = Resources.ShinchokuInzuhyoImport_h_kishu;
                sheet.ColumnHeaders[2].Caption = Resources.ShinchokuInzuhyoImport_h_gouki;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion //初期化
        
        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>T.Nakata 2019/07/08</create>
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
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 編集処理
        /// --------------------------------------------------
        /// <summary>
        /// 編集処理
        /// </summary>
        /// <returns>処理結果 true:処理完了/false:NGあり</returns>
        /// <create>D.Okumura 2019/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            var ret =  base.RunEdit();
            if (ret)
            {
                // 正常終了：画面を閉じる
                this.DialogResult = DialogResult.OK;
                // 終了時メッセージ OFF
                this.IsCloseQuestion = false;
                this.Close();
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック処理
        /// </summary>
        /// <returns>処理結果 true:入力チェックOK/false:入力チェックNG</returns>
        /// <create>D.Okumura 2019/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            if (!base.CheckInput())
            {
                return false;
            }

            if (this.shtMeisai.MaxRows < 1)
            {
                // 取り込むDataがありません。
                this.ShowMessage("A0100040011");
                return false;
            }

            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 編集処理(Update)
        /// </summary>
        /// <returns>処理結果 true:処理成功/false:処理失敗</returns>
        /// <create>D.Okumura 2019/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            if (!base.RunEditInsert())
            {
                return false;
            }
            // DB反映
            DataTable tmpDt = (this.shtMeisai.DataSource as DataTable).Copy();
            CondA1 cond = new CondA1(this.UserInfo);
            ConnA01 conn = new ConnA01();
            string errMsgID = string.Empty;
            DataTable dtMsg;
            string[] args = null;
            cond.NonyusakiCD = _nonyusakiCd;
            if (!conn.InsInzuhyo(cond, tmpDt, out errMsgID, out dtMsg, out args))
            {
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                }
                if (dtMsg.Rows.Count > 0)
                {   // 員数表(Excel)のDataにErrorがあります。
                    this.ShowMultiMessage(dtMsg, "A0100040004");
                }
                return false;
            }
            else
            {
                return true;
            }
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
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                base.fbrFunction_F01Button_Click(sender, e);
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
        /// <create>T.Nakata 2019/07/08</create>
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

        #region 参照ボタン
        /// --------------------------------------------------
        /// <summary>
        /// 参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnFileSelect_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                ofdExcel.FileName = this.txtExcel.Text;
                ofdExcel.Title = Resources.ShinchokuInzuhyoImport_RefDlgTitle;
                ofdExcel.Filter = Resources.ShinchokuInzuhyoImport_RefDlgFilter;
                if (ofdExcel.ShowDialog() == DialogResult.OK)
                {
                    this.txtExcel.Text = ofdExcel.FileName;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region 読込ボタン
        /// --------------------------------------------------
        /// <summary>
        /// 読込ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRead_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            this.SheetClear();
            try
            {
                if (!this.ExecuteImport())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
        #endregion


        #endregion //イベント

        #region 員数表データテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 員数表データテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update>D.Okumura 2019/08/22 処理フラグの名称を表示するように修正</update>
        /// --------------------------------------------------
        private DataTable GetSchemeInzuhyo()
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI_TEMPWORK.Name);
            dt.Columns.Add(Def_T_AR_GOKI_TEMPWORK.FLAG, typeof(string));
            dt.Columns.Add(SHEET_FLAG_NAME, typeof(string));
            dt.Columns.Add(Def_T_AR_GOKI_TEMPWORK.KISHU, typeof(string));
            dt.Columns.Add(Def_T_AR_GOKI_TEMPWORK.GOKI, typeof(string));

            return dt;
        }

        #endregion

        #region 取込処理

        #region 取込制御部

        /// --------------------------------------------------
        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns></returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExecuteImport()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 員数表(Excel)の入力チェック
                if (string.IsNullOrEmpty(this.txtExcel.Text))
                {
                    // 員数表(Excel)のFileが存在しません。
                    this.ShowMessage("A0100040001");
                    return false;
                }
                // ファイル存在チェック
                if (!File.Exists(this.txtExcel.Text))
                {
                    // 員数表(Excel)のFileが存在しません。
                    this.ShowMessage("A0100040001");
                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;
                var dt = this.GetSchemeInzuhyo();
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = false;

                ret = this.GetExcelDataXlsx(this.txtExcel.Text, dt,dtMessage);
                if (ret)
                {
                    // DBチェック
                    DataTable tmpDt = dt.Copy();
                    CondA1 cond = new CondA1(this.UserInfo);
                    ConnA01 conn = new ConnA01();
                    string errMsgID = string.Empty;
                    DataTable dtMsg;
                    string[] args = null;
                    cond.NonyusakiCD = _nonyusakiCd;
                    if (!conn.CheckGoki(cond, tmpDt, out errMsgID, out dtMsg, out args))
                    {
                        if (!string.IsNullOrEmpty(errMsgID))
                        {
                            this.ShowMessage(errMsgID, args);
                        }
                        if (dtMsg.Rows.Count > 0)
                        {   // 員数表(Excel)のDataにErrorがあります。
                            this.ShowMultiMessage(dtMsg, "A0100040004");
                        }
                    }
                    else
                    {   // 反映
                        this.shtMeisai.DataSource = dt;
                        this.shtMeisai.Enabled = true;
                        ret = true;
                    }
                }
                else
                {   // 員数表(Excel)のDataにErrorがあります。
                    this.ShowMultiMessage(dtMessage, "A0100040004");
                }

                return ret;
            }
            catch (Exception ex)
            {
                this.shtMeisai.Redraw = true;
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
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXlsx(string filePath, DataTable dt, DataTable dtMessage)
        {
            using (var xls = new XlsxCreator())
            {
                try
                {
                    if (xls.ReadBook(filePath) >= 0)
                    {
                        int maxRow = xls.MaxData(xlMaxEndPoint.xarMaxPoint).Height;
                        return this.CheckExcelData(dt, dtMessage, maxRow, xls);
                    }
                    else
                    {
                        // 員数表(Excel)の読み取り中にErrorが発生しました。Fileの内容を確認してください。
                        ComFunc.AddMultiMessage(dtMessage, "A0100040003");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    xls.CloseBook(false);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 値チェック
        /// </summary>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="maxRow">最大行位置</param>
        /// <param name="obj">xlsオブジェクト</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update>D.Okumura 2019/08/22 処理フラグの名称を表示するように修正</update>
        /// <upddate>T.Nukaga 2021/04/02 機種名の長さ指定を定数に変更</upddate>
        /// --------------------------------------------------
        private bool CheckExcelData(DataTable dt, DataTable dtMessage, int maxRow, object obj)
        {
            Func<int, int, string> fncGetStr = (colIndex, rowIndex) =>
            {
                return (obj as XlsxCreator).Pos(colIndex, rowIndex).Str;
            };
            Func<int, int, object> fncGetVal = (colIndex, rowIndex) =>
            {
                return (obj as XlsxCreator).Pos(colIndex, rowIndex).Value;
            };
            // 処理フラグ名称取得
            var dicFlag = this.GetCommonItemNameByValue1(AR_INZU_IMPORT_FLAG.GROUPCD);

            if (maxRow < 1)
            {
                // 員数表(Excel)のFileにDataがありません。
                ComFunc.AddMultiMessage(dtMessage, "A0100040002");
                return false;
            }
            string itemName = string.Empty;
            string field = string.Empty;
            int row = 0;
            int col = 0;
            bool isError = false;
            bool isAddData = false;

            for (row = SHEET_DATA_START_IDX; row <= maxRow; row++)
            {
                DataRow dr = dt.NewRow();

                isAddData = true;
                col = 0;

                // フラグのチェック
                itemName = Resources.ShinchokuInzuhyoImport_h_flag;
                string inFlagStr = fncGetStr(col, row);
                if (!this.CheckAndSetExcelData(inFlagStr, row, dr, Def_T_AR_GOKI_TEMPWORK.FLAG, 1, itemName, ExcelInputAttrType.Flag, dtMessage, false, true, true))
                {
                    isAddData = false;
                    inFlagStr = string.Empty;
                    dr[SHEET_FLAG_NAME] = string.Empty;
                }
                else
                {
                    dr[SHEET_FLAG_NAME] = dicFlag.ContainsKey(inFlagStr) ? dicFlag[inFlagStr] : string.Empty;
                }

                // 機種のチェック
                col++;
                itemName = Resources.ShinchokuInzuhyoImport_h_kishu;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, Def_T_AR_GOKI_TEMPWORK.KISHU, ComDefine.MAX_BYTE_LENGTH_KISHU, itemName, ExcelInputAttrType.Kishu, dtMessage, true, true, true))
                {
                    isAddData = false;
                }

                // 号機名のチェック
                col++;
                itemName = Resources.ShinchokuInzuhyoImport_h_gouki;
                string inGoukiStr = fncGetStr(col, row);
                if (!this.CheckAndSetExcelData(inGoukiStr, row, dr, Def_T_AR_GOKI_TEMPWORK.GOKI, 40, itemName, ExcelInputAttrType.GoukiName, dtMessage, true, true, true))
                {
                    isAddData = false;
                }
                else
                {// 号機重複チェック
                    if (!string.IsNullOrEmpty(inFlagStr) && int.Parse(inFlagStr) == 1)
                    {
                        string expression = Def_T_AR_GOKI_TEMPWORK.FLAG + " = " + AR_INZU_IMPORT_FLAG.UPDATE_VALUE1 + " and " + Def_T_AR_GOKI_TEMPWORK.GOKI + " = '" + inGoukiStr + "'";
                        DataRow[] tmpRow = dt.Select(expression);
                        if (tmpRow != null && tmpRow.Length > 0)
                        {// 重複アリ
                            // {0}行目の号機が重複しています。
                            ComFunc.AddMultiMessage(dtMessage, "A0100040009", (row + 1).ToString());
                            isAddData = false;
                        }
                    }
                }

                if (isAddData)
                {
                    dt.Rows.Add(dr);
                }
                else
                {
                    isError = true;
                }
            }
            return !isError;
        }

        #endregion


        #region 取込チェック処理

        /// --------------------------------------------------
        /// <summary>
        /// 取込チェック処理
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="dr">取込データのデータロウ</param>
        /// <param name="field">データロウのカラム名</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>
        /// <param name="isNecessary">必須かどうか</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckAndSetExcelData(string target, int rowIndex, DataRow dr, string field, int checkLen, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary, bool isCheckLen)
        {
            object retVal = DBNull.Value;
            bool ret = true;
            // 文字数チェック
            if (!this.CheckByteLength(target, rowIndex, ref retVal, checkLen, itemName, dtMessage, isString, isCheckLen))
            {
                // {0}行目の{1}は{2}Bytes以下で入力してください。
                ComFunc.AddMultiMessage(dtMessage, "A0100040006", (rowIndex + 1).ToString(), itemName, checkLen.ToString());
                ret = false;
            }
            // 入力チェック
            if (retVal != DBNull.Value)
            {
                if (!CheckRegulation(target, rowIndex, itemName, attrType, dtMessage))
                {
                    ret = false;
                }
            }
            // 必須チェック
            if (isNecessary)
            {
                if (string.IsNullOrEmpty(target))
                {
                    // {0}行目の{1}を入力してください。
                    ComFunc.AddMultiMessage(dtMessage, "A0100040007", (rowIndex + 1).ToString(), itemName);
                    ret = false;
                }
            }
            dr[field] = retVal;
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
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>T.Nakata 2019/07/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isString, bool isCheckLen)
        {
            if (isCheckLen && checkLen < UtilString.GetByteCount(target))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(target))
            {
                value = target;
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
        /// <create>T.Nakata 2019/07/08</create>
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
                case ExcelInputAttrType.Flag:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    isUse = true;
                    msgExtend = Resources.ShinchokuInzuhyoImport_h_flag;
                    break;
                case ExcelInputAttrType.Kishu:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    msgExtend = Resources.ShinchokuInzuhyoImport_h_kishu;
                    break;
                case ExcelInputAttrType.GoukiName:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    msgExtend = Resources.ShinchokuInzuhyoImport_h_gouki;
                    break;
                default:
                    break;
            }
            // 禁止文字チェック
            if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(targetType))
            {
                if (!regulation.CheckRegularString(target, targetType, isUse, false))
                {
                    // {0}行目の{1}に使用できない文字が含まれています。
                    ComFunc.AddMultiMessage(dtMessage, "A0100040008", (rowIndex + 1).ToString(), msgExtend);
                    return false;
                }
            }

            // 個別チェック
            if (attrType == ExcelInputAttrType.Flag)
            {
                int Flag = -1;
                bool result = int.TryParse(target, out Flag);
                if (!result || (Flag != 0 && Flag != 1 && Flag != 9))
                {
                    // {0}行目のFlagの入力が不正です。
                    ComFunc.AddMultiMessage(dtMessage, "A0100040005", (rowIndex + 1).ToString());
                    return false;
                }
            }
            else
            {   // 機種/号機名に禁則文字が無いかを確認する
                char[] invalidCharactors = new[] {
                    this.UserInfo.SysInfo.SeparatorRange,// 区切り文字：カンマ
                    this.UserInfo.SysInfo.SeparatorItem,  // ハイフン
                    '\r',
                    '\n',
                };
                if (target.IndexOfAny(invalidCharactors) != -1)
                {
                    // {0}行目の{1}に使用できない文字が含まれています。
                    ComFunc.AddMultiMessage(dtMessage, "A0100040008", (rowIndex + 1).ToString(), msgExtend);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #endregion
    }
}
