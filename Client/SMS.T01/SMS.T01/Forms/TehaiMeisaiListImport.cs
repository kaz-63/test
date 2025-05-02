using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Commons;
using SystemBase.Util;
using DSWUtil;
using System.Text.RegularExpressions;
using XlsxCreatorHelper;
using System.IO;
using System.Linq;
using XlsCreatorHelper;
using System.Data;
using SMS.T01.Properties;
using WsConnection;
using WsConnection.WebRefS01;
using WsConnection.WebRefT01;
namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配List取込
    /// </summary>
    /// <create>D.Naito 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiMeisaiListImport : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細Excel格納用 DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtTehaiMeisaiExcel = null;
        /// --------------------------------------------------
        /// <summary>
        /// 手配フラグ値格納用 DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtTehaiFlag = null;
        /// --------------------------------------------------
        /// <summary>
        /// 数量単位値格納用 DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtQuantityUnit = null;

        /// --------------------------------------------------
        /// <summary>
        /// 物件マスタ DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtBukken = null;
        /// --------------------------------------------------
        /// <summary>
        /// 機種マスタ DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtKishu = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納品先マスタ DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtNohinSaki = null;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷先マスタ DataTable
        /// </summary>
        /// <create>J.Chen 2022/10/19</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtSyukkaSaki = null;


        /// --------------------------------------------------
        /// <summary>
        /// フィールド長リスト
        /// </summary>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        Dictionary<string, int> _listFieldLength = null;

        /// --------------------------------------------------
        /// <summary>
        /// 期 - 親フォームからのパラメータ受取
        /// </summary>
        /// <create>TW-Tsuji 2022/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public int ARG_EcsQuota = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ECS No. - 親フォームからのパラメータ受取
        /// </summary>
        /// <create>TW-Tsuji 2022/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ARG_EcsNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 手配が更新かどうか - 親フォームからのパラメータ受取
        /// </summary>
        /// <create>TW-Tsuji 2022/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ARG_IsUpdate;

        #endregion

        #region Excelデータ格納、引き渡しフィールド　2022/10/19 【Step15】
        /// --------------------------------------------------
        /// <summary>
        /// エクセルファイル情報
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        public class ExcelInfo
        {
            /// <summary>
            /// 結果
            /// </summary>
            public List<string> list { get; set; }

            //【Step15】手配取込Excel 2022/10/18 (TW-Tsuji)
            //
            //　結果を親フォームに返すためのResultクラスに項目追加
            //      
            //　◆手配情報のヘッダ（検索条件）部分
            public CondT01 header;
            //
            //　◆手配情報のエクセルデータ件数＆配列
            public bool meisaiAdd;
            public int meisaiRows;
            public DataTable meisai;
            //
            //　◆コンストラクタ
            //public ExcelInfo()
            //{
            //    header = new CondT01(this);
            //}            
        }
        /// --------------------------------------------------
        /// <summary>
        /// 読み込みファイル結果
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public ExcelInfo Result = new ExcelInfo();

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 期が設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ECSQUOTA_ROW_POS = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ECSNOが設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ECSNO_ROW_POS = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名が設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_BUKKEN_NAME_ROW_POS = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 製番が設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_SEIBAN_ROW_POS = 3;
        /// --------------------------------------------------
        /// <summary>
        /// CODEが設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_CODE_ROW_POS = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 機種が設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_KISHU_ROW_POS = 5;
        /// --------------------------------------------------
        /// <summary>
        /// ARNOが設定されている行位置
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_ARNO_ROW_POS = 6;
        /// --------------------------------------------------
        /// <summary>
        /// データ開始行
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_DATA_START_IDX = 9;

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// Excelの文字属性タイプ
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ExcelInputAttrType
        {
            AlphaNum = 0,
            WideString = 1,
            Numeric = 2,
            Date = 3,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ
        /// </summary>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ShoriFlag
        {
            /// --------------------------------------------------
            /// <summary>
            /// 変更なし
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            NoChange = 0,
            /// --------------------------------------------------
            /// <summary>
            /// 追加/更新
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            InsOrUpd = 1,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tajimi 2018/10/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Del = 9,
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        [Obsolete("Design time only", true)]
        public TehaiMeisaiListImport()
            : this(null)
        {

        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>D.Naito 2018/12/05</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        public TehaiMeisaiListImport(UserInfo userInfo)
            : base(userInfo)
        {
            InitializeComponent();
            this.Title = ComDefine.TITLE_T0100012;

            this.Result.header = new CondT01(this.UserInfo);
        }

        #endregion

        public TehaiMeisaiListImport(UserInfo userInfo, int ecsQuota, string ecsNo)
            : base(userInfo)
        {
            this.ARG_EcsQuota = ecsQuota;
            this.ARG_EcsNo = ecsNo;
        }

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームロード
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update>J.Chen 2022/10/17</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化
                this.txtExcel.Text = string.Empty;

                // 汎用マスタ取得
                // 出荷計画Excel状態
                this._dtTehaiMeisaiExcel = this.GetCommon(SHIPPING_PLAN_EXCEL_TYPE.GROUPCD).Tables[Def_M_COMMON.Name];
                // 手配区分
                this._dtTehaiFlag = this.GetCommon(TEHAI_FLAG.GROUPCD).Tables[Def_M_COMMON.Name];
                // 数量単位
                this._dtQuantityUnit = this.GetCommon(QUANTITY_UNIT.GROUPCD).Tables[Def_M_COMMON.Name];
                // フィールド長
                this._listFieldLength = this.GetCommon(FIELD_LENGTH.GROUPCD, Def_M_COMMON.ITEM_CD, row => ComFunc.GetFldToInt32(row, Def_M_COMMON.VALUE1));

                ConnT01 conn = new ConnT01();
                // 物件名取得
                this._dtBukken = conn.GetBukkenName().Tables[Def_M_PROJECT.Name];
                // 機種取得
                this._dtKishu = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.KISHU_VALUE1 }).Tables[Def_M_SELECT_ITEM.Name];
                // 出荷先
                this._dtSyukkaSaki = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.SYUKKA_SAKI_VALUE1 }).Tables[Def_M_SELECT_ITEM.Name];
                // 納品先
                this._dtNohinSaki = conn.GetSelectItem(new CondT01(this.UserInfo) { SelectGroupCode = SELECT_GROUP_CD.NOUHIN_SAKI_VALUE1 }).Tables[Def_M_SELECT_ITEM.Name];


                this.EditMode = SystemBase.EditMode.Insert;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォーム表示初期化
        /// </summary>
        /// <create>D.Naito 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.btnReference.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
        
        #region イベント

        /// --------------------------------------------------
        /// <summary>
        /// 参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnReference_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                ofdExcel.FileName = this.txtExcel.Text;
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

        #region ファンクション
        /// --------------------------------------------------
        /// <summary>
        /// F01押下イベント(決定ボタン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            try
            {
                if (!this.ExecuteImport())
                {

                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

            //正常終了の場合はフォームをクローズして終了
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        /// --------------------------------------------------
        /// <summary>
        /// F12押下イベント(Closeボタン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Naito 2018/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 手配明細のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細のデータテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2022/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemaTehaiMeisaiList()
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name + "List");
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ECS_QUOTA, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ECS_NO, typeof(string));
            dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NAME, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.SEIBAN, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.CODE, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.KISHU, typeof(string));
            dt.Columns.Add(Def_T_SHUKKA_MEISAI.AR_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.SETTEI_DATE, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.NOUHIN_SAKI, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.SYUKKA_SAKI, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.FLOOR, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ST_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_OIBAN, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI_JP, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_QTY, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.QUANTITY_UNIT, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG, typeof(string));

            return dt;
        }

        #endregion

        #region 取込処理

        #region 取込制御部
        /// --------------------------------------------------
        /// <summary>
        /// インポート実行処理
        /// </summary>
        /// <returns>結果：true = 成功, false = 失敗</returns>
        /// <create>J.Chen 2022/10/18</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool ExecuteImport()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 明細データ(Excel)の入力チェック
                if (string.IsNullOrEmpty(this.txtExcel.Text))
                {
                    // 手配明細(Excel)のFilesが選択されていません。
                    this.ShowMessage("T0100012023");
                    return false;
                }
                // ファイル存在チェック
                if (!File.Exists(this.txtExcel.Text))
                {
                    // 手配明細(Excel)のFilesが存在しません。
                    this.ShowMessage("T0100012023");
                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;
                var dt = this.GetSchemaTehaiMeisaiList();
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                bool ret = false;
                var bukkenName = string.Empty;
                var shipSeiban = string.Empty;
                if (Path.GetExtension(this.txtExcel.Text) == ".xls")
                {
                    ret = this.GetExcelDataXls(this.txtExcel.Text, dt, dtMessage, ref bukkenName, ref shipSeiban);
                }
                else
                {
                    ret = this.GetExcelDataXlsx(this.txtExcel.Text, dt, dtMessage, ref bukkenName, ref shipSeiban);
                }

                //致命的エラーで中断した場合（ヘッダーにエラーがあった場合）は、falseで終わる.
                if (ret == false)
                {
                    return ret;
                }

                if (0 < dt.Rows.Count)
                {
                    ret = true;

                    // ここで、データを戻す（Tsuji）
                    this.Result.meisai = dt.Copy();
                    //this.txtBukkenName.Text = bukkenName;
                    //this.txtShipSeiban.Text = shipSeiban;
                }

                //明細データに登録できないデータがあった場合
                //if (0 < dtMessage.Rows.Count)
                if (!this.Result.meisaiAdd)
                {
                    // 取込出来ないデータがありました。\r\nエラーがあった行は表示されていません。\r\n※エラーの一覧は右クリックでクリップボードにコピーできます。
                    this.ShowMultiMessage(dtMessage, "T0100012019");
                }
                else
                {
                    this.ShowMultiMessage(dtMessage, "T0100012020", this.Result.meisaiRows.ToString());
                }
                
                return ret;
            }
            catch (Exception ex)
            {
                //this.shtMeisai.Redraw = true;
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
        /// XLSXインポート処理
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>結果：null = 失敗</returns>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXlsx(string filePath, DataTable dt, DataTable dtMessage, ref string bukkenName, ref string shipSeiban)
        {
            using (var xls = new XlsxCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlMaxEndPoint.xarMaxPoint).Height;
                    return this.CheckExcelData(dt, dtMessage, maxRow, xls, true, ref bukkenName, ref shipSeiban);
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
        /// XLSインポート処理
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="dt">取り込んだデータ</param>
        /// <param name="dtMessage">エラーメッセージテーブル</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>結果：null = 失敗</returns>
        /// <create>J.Chen 2022/10/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetExcelDataXls(string filePath, DataTable dt, DataTable dtMessage, ref string bukkenName, ref string shipSeiban)
        {
            using (var xls = new XlsCreator())
            {
                try
                {
                    xls.ReadBook(filePath);
                    int maxRow = xls.MaxData(xlPoint.ptMaxPoint).Height;
                    return this.CheckExcelData(dt, dtMessage, maxRow, xls, false, ref bukkenName, ref shipSeiban);
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
        /// <param name="isXlsx">xlsxかどうか</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="shipSeiban">運賃・梱包 製番</param>
        /// <returns>エラーがあったかどうか</returns>
        /// <create>J.Chen 2022/10/13</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// <update>J.Chen 2023/02/21 桁数変更対応、品名半角英数字のみに変更</update>
        /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
        /// <update>J.Chen 2024/08/19 出荷先桁数変更</update>
        /// --------------------------------------------------
        private bool CheckExcelData(DataTable dt, DataTable dtMessage, int maxRow, object obj, bool isXlsx, ref string bukkenName, ref string shipSeiban)
        {
            Func<int, int, string> fncGetStr = (colIndex, rowIndex) =>
            {
                if (isXlsx)
                {
                    return (obj as XlsxCreator).Pos(colIndex, rowIndex).Str;
                }
                else
                {
                    return (obj as XlsCreator).Pos(colIndex, rowIndex).Str;
                }
            };
            Func<int, int, object> fncGetVal = (colIndex, rowIndex) =>
            {
                if (isXlsx)
                {
                    return (obj as XlsxCreator).Pos(colIndex, rowIndex).Value;
                }
                else
                {
                    return (obj as XlsCreator).Pos(colIndex, rowIndex).Value;
                }
            };

            if (maxRow < 1)
            {
                // 手配明細(Excel)のFilesにDataがありません。
                this.ShowMessage("T0100012023");
                return false;
            }
            //bool ret = true;
            string itemName = string.Empty;
            string field = string.Empty;
            ExcelInputAttrType attribute;
            int col = 0;
            int row = 0;
            int maxLen = 0;     //フィールド最大文字数
            bool isString;      //文字列チェックの対象かどうか
            bool isNecessary;   //必須チェックの対象かどうか
            bool isCheckLen;    //文字列入力桁数のチェック対象かどうか
            int checkLen = 0;
            DataRow dr = dt.NewRow();
            bool isAddData = true;
            bool is1st = true;

            // 期のチェック
            itemName = Resources.TehaiMeisaiList_ECSQuota;
            row = SHEET_ECSQUOTA_ROW_POS;               //Excelのデータ位置（行）0（ゼロ）から開始
            col = 1;                                    //Excelのデータ位置（桁）0（ゼロ）から開始
            maxLen = 3;                                 //最大文字数
            field = this.Result.header.EcsQuota;        //取得したデータを戻すフィールド
            attribute =  ExcelInputAttrType.Numeric;    //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = true;                         //必須チェックの対象かどうか
            isCheckLen = true;                          //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            // 修正の場合は元画面の期と一致するかチェック
            if (ARG_IsUpdate == true)
            {
                if (!field.Equals(this.ARG_EcsQuota.ToString("000")))
                {
                    // Excel{0}行目の{1}で指定されたデータが、手配情報登録画面と一致していません。
                    //　（修正なのに修正元のキー項目「期」が異なる）
                    this.ShowMessage("T0100012009", (row + 1).ToString(), itemName);
                    return false;
                }
            }
            else
            {
                this.Result.header.EcsQuota = field;    //取得したデータを戻すフィールド
            }

            // ECSNoのチェック
            itemName = Resources.TehaiMeisaiList_ECSNo;
            row = SHEET_ECSNO_ROW_POS;
            col = 1;
            maxLen = 20;                                //最大文字数
            field = this.Result.header.EcsNo;           //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.AlphaNum;    //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = true;                         //必須チェックの対象かどうか
            isCheckLen = false;                         //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }

            // 修正の場合は元画面の期と一致するかチェック
            if (ARG_IsUpdate == true)
            {
                if (!field.Equals(this.ARG_EcsNo))
                {
                    // Excel{0}行目の{1}で指定されたデータが、手配情報登録画面と一致していません。
                    //　（修正なのに修正元のキー項目「ECS No.」が異なる）
                    this.ShowMessage("T0100012009", (row + 1).ToString(), itemName);
                    return false;
                }
            }
            else
            {
                this.Result.header.EcsNo = field;       //取得したデータを戻すフィールド
            }


            //新規登録の場合は、ここで 期＋ECS No.を検索して、登録の有無を確認する.
            if (ARG_IsUpdate == false)
            {
                try
                {
                    CondT01 cond = new CondT01(this.UserInfo);
                    cond.EcsQuota = this.Result.header.EcsQuota;
                    cond.EcsNo = this.Result.header.EcsNo;
                    ConnT01 conn = new ConnT01();
                    DataSet ds = conn.GetGiren(cond);
                    if (ComFunc.IsExistsData(ds, Def_M_ECS.Name))
                    {
                        // Excelで指定された、期、ECS No　の手配明細情報は、既に登録されています。
                        this.ShowMessage("T0100012014");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                    return false;
                }
            }

            // 物件名のチェック
            itemName = Resources.TehaiMeisaiList_BukkenName;
            row = SHEET_BUKKEN_NAME_ROW_POS;
            col = 1;
            maxLen = 60;                                //最大文字数
            field = this.Result.header.BukkenName;      //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.WideString;  //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = true;                         //必須チェックの対象かどうか
            isCheckLen = false;                         //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            else
            {
                bukkenName = fncGetStr(col, row);
                // 物件マスタに存在するかどうかチェック
                bool isFind = false;
                if (UtilData.ExistsData(this._dtBukken))
                {
                    foreach (DataRow drBukken in this._dtBukken.Rows)
                    {
                        //if (ComFunc.GetFld(drBukken, Def_M_BUKKEN.BUKKEN_NAME) == ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME))
                        if (bukkenName.Equals(ComFunc.GetFld(drBukken, Def_M_BUKKEN.BUKKEN_NAME)))
                        {
                            isFind = true;
                            this.Result.header.BukkenName = ComFunc.GetFld(drBukken, Def_M_BUKKEN.PROJECT_NO);
                            break;
                        }
                    }
                }
                if (!isFind)
                {
                    // 該当する物件名はありません。
                    this.ShowMessage("S0100010006");
                    return false;
                }
            }

            // 製番のチェック
            itemName = Resources.TehaiMeisaiList_Seiban;
            row = SHEET_SEIBAN_ROW_POS;
            col = 1;
            maxLen = 12;                                //最大文字数
            field = this.Result.header.Seiban;          //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.AlphaNum;    //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = true;                         //必須チェックの対象かどうか
            isCheckLen = false;                         //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            else
            {
                this.Result.header.Seiban = field;      //取得したデータを戻すフィールド
            }

            // CODEのチェック
            itemName = Resources.TehaiMeisaiList_Code;
            row = SHEET_CODE_ROW_POS;
            col = 1;
            maxLen = 3;                                 //最大文字数
            field = this.Result.header.Code;            //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.AlphaNum;    //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = true;                         //必須チェックの対象かどうか
            isCheckLen = false;                         //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            else
            {
                this.Result.header.Code = field;        //取得したデータを戻すフィールド
            }


            // 機種のチェック
            itemName = Resources.TehaiMeisaiList_Kishu;
            row = SHEET_KISHU_ROW_POS;
            col = 1;
            maxLen = 40;                                //最大文字数
            field = this.Result.header.DispSelect;      //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.WideString;  //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = false;                        //必須チェックの対象かどうか
            isCheckLen = false;                         //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            else
            {
                // 機種マスタに存在するかどうかチェック
                bool isFind = false;
                if (UtilData.ExistsData(this._dtKishu))
                {
                    foreach (DataRow drKishu in this._dtKishu.Rows)
                    {
                        if (ComFunc.GetFld(drKishu, Def_M_COMMON.ITEM_NAME).Equals(field))
                        {
                            isFind = true;
                            this.Result.header.DispSelect = field;
                            break;
                        }
                    }
                }
                if (!isFind)
                {
                    this.Result.header.DispSelect = null;
                }
            }

            // ARNoのチェック
            itemName = Resources.TehaiMeisaiList_ARno;
            row = SHEET_ARNO_ROW_POS;
            col = 1;
            maxLen = 4;                                 //最大文字数
            field = this.Result.header.ARNo;            //取得したデータを戻すフィールド
            attribute = ExcelInputAttrType.Numeric;     //チェック属性
            isString = true;                            //文字列チェックの対象かどうか
            isNecessary = false;                        //必須チェックの対象かどうか
            isCheckLen = true;                          //文字列入力桁数のチェック対象かどうか
            if (!this.CheckAndSetExcelDataHeader(fncGetStr(col, row), row, ref field, maxLen, itemName, attribute, dtMessage, isString, isNecessary, isCheckLen))
            {
                return false;       //エラー終了
            }
            else
            {
                this.Result.header.ARNo = field;        //取得したデータを戻すフィールド
            }


            //////////////////////////////
            // 手配明細データチェック
            this.Result.meisaiRows = 0;

            for (row = SHEET_DATA_START_IDX; row <= maxRow; row++)
            {
                //if (string.IsNullOrEmpty(fncGetStr(0, row)))
                //{
                //    break;
                //}

                if (!is1st)
                {
                    dr = dt.NewRow();
                    //isAddData = true;
                }
                this.Result.meisaiRows++;

                // 設定納期のチェック
                itemName = Resources.TehaiMeisaiList_SetteiDate;
                col = 0;
                checkLen = 10;
                field = Def_T_TEHAI_MEISAI.SETTEI_DATE;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.Date, dtMessage, true, true, true, false))
                {
                    isAddData = false;
                }

                // 納品先のチェック
                itemName = Resources.TehaiMeisaiList_NouhinSaki;
                col = 1;
                checkLen = 10;
                field = Def_T_TEHAI_MEISAI.NOUHIN_SAKI;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    // 納品先マスタに存在するかどうかチェック
                    bool isFind = false;
                    if (UtilData.ExistsData(this._dtNohinSaki))
                    {
                        foreach (DataRow drNohinSaki in this._dtNohinSaki.Rows)
                        {
                            if (ComFunc.GetFld(drNohinSaki, Def_M_COMMON.ITEM_NAME) == ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.NOUHIN_SAKI))
                            {
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (!isFind)
                    {
                        // Excel{0}行目、{1}の{2}がマスタに見つかりませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012018", (row + 1).ToString(), itemName, ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.NOUHIN_SAKI));
                        dr[Def_T_TEHAI_MEISAI.NOUHIN_SAKI] = null;
                    }
                }

                // 出荷先のチェック
                itemName = Resources.TehaiMeisaiList_SyukkaSaki;
                col = 2;
                checkLen = 20;
                field = Def_T_TEHAI_MEISAI.SYUKKA_SAKI;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    // 出荷先マスタに存在するかどうかチェック
                    bool isFind = false;
                    if (UtilData.ExistsData(this._dtSyukkaSaki))
                    {
                        foreach (DataRow drSyukkaSaki in this._dtSyukkaSaki.Rows)
                        {
                            if (ComFunc.GetFld(drSyukkaSaki, Def_M_COMMON.ITEM_NAME) == ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.SYUKKA_SAKI))
                            {
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (!isFind)
                    {
                        // Excel{0}行目、{1}の{2}がマスタに見つかりませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012018", (row + 1).ToString(), itemName, ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.SYUKKA_SAKI));
                        dr[Def_T_TEHAI_MEISAI.SYUKKA_SAKI] = null;
                    }
                }

                // Floorのチェック
                itemName = Resources.TehaiMeisaiList_Floor;
                col = 3;
                checkLen = 20;
                field = Def_T_TEHAI_MEISAI.FLOOR;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // STNoのチェック
                itemName = Resources.TehaiMeisaiList_STNo;
                col = 4;
                checkLen = 16;
                field = Def_T_TEHAI_MEISAI.ST_NO;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 追番のチェック
                itemName = Resources.TehaiMeisaiList_ZumenOiban;
                col = 5;
                checkLen = 12;
                field = Def_T_TEHAI_MEISAI.ZUMEN_OIBAN;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 品名（和文）のチェック
                itemName = Resources.TehaiMeisaiList_HinmeiJp;
                col = 6;
                checkLen = 100;
                field = Def_T_TEHAI_MEISAI.HINMEI_JP;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 品名のチェック
                itemName = Resources.TehaiMeisaiList_Hinmei;
                col = 7;
                checkLen = 100;
                field = Def_T_TEHAI_MEISAI.HINMEI;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.AlphaNum, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 図番型式のチェック
                itemName = Resources.TehaiMeisaiList_ZhumenKeishiki;
                col = 8;
                checkLen = 100;
                field = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 図番型式2のチェック
                itemName = Resources.TehaiMeisaiList_ZumenKeishiki2;
                col = 9;
                checkLen = 30;
                field = Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 手配数のチェック
                itemName = Resources.TehaiMeisaiList_TehaiQty;
                col = 10;
                checkLen = 6;
                field = Def_T_TEHAI_MEISAI.TEHAI_QTY;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.Numeric, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }

                // 数量単位のチェック
                itemName = Resources.TehaiMeisaiList_QuantityUnit;
                col = 11;
                checkLen = 4;
                field = Def_T_TEHAI_MEISAI.QUANTITY_UNIT;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    var value = fncGetStr(col, row);
                    bool isFound = false;
                    foreach (DataRow drQuantityUnit in this._dtQuantityUnit.Rows)
                    {
                        if (ComFunc.GetFld(drQuantityUnit, Def_M_COMMON.ITEM_NAME) == value)
                        {
                            dr[Def_T_TEHAI_MEISAI.QUANTITY_UNIT] = ComFunc.GetFld(drQuantityUnit, Def_M_COMMON.VALUE1);
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound == false)
                    {
                        // Excel{0}行目、{1}の{2}がマスタに見つかりませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012022", (row + 1).ToString(), itemName, value);
                        isAddData = false;
                    }
                }

                // 手配区分のチェック
                itemName = Resources.TehaiMeisaiList_TehaiFlag;
                col = 12;
                checkLen = 20;
                field = Def_T_TEHAI_MEISAI.TEHAI_FLAG;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    var value = fncGetStr(col, row);
                    bool isFound = false;
                    foreach (DataRow drTehaiFlag in this._dtTehaiFlag.Rows)
                    {
                        if (ComFunc.GetFld(drTehaiFlag, Def_M_COMMON.ITEM_NAME) == value)
                        {
                            dr[Def_T_TEHAI_MEISAI.TEHAI_FLAG] = ComFunc.GetFld(drTehaiFlag, Def_M_COMMON.VALUE1);
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound == false)
                    {
                        // Excel{0}行目、{1}の{2}がマスタに見つかりませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012022", (row + 1).ToString(), itemName, value);
                        isAddData = false;
                    }
                }

                // 手配Noのチェック
                itemName = Resources.TehaiMeisaiList_TehaiNo;
                col = 13;
                checkLen = 8;   //手配Noは 8桁だが、同一セル内に複数指定するためチェックは行わない.
                                //　9番目のパラメータ isCheckLen=falseとして呼び出す.
                                //　尚、ここで設定する8桁は、分割するファンクションへ渡すパラメータ.
                field = Def_T_TEHAI_SKS.TEHAI_NO;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, false, false))
                {
                    isAddData = false;      //チェックは無いが、ルーチンは残す
                    dr[field] = null;       //Excel記載のデータが入ってしまうので
                }
                else
                {
                    if (dr[Def_T_TEHAI_MEISAI.TEHAI_FLAG].ToString() != TEHAI_FLAG.ORDERED_VALUE1 && (!string.IsNullOrEmpty(dr[Def_T_TEHAI_SKS.TEHAI_NO].ToString())))
                    {
                        // Excel {0}行目の手配区分は発注以外のため、手配No使用できません。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012024", (row + 1).ToString());
                        isAddData = false;
                    }
                    //手配Noが入力されている場合は、複数入力されている文字列を、8桁の番号に分割する
                    //
                    else if (!this.CheckAndDelimitTehaiNo(fncGetStr(col, row), row, dr,field, checkLen, itemName, dtMessage)) 
                    {
                        isAddData = false;
                    }
                }

                // 返却品のチェック
                itemName = Resources.TehaiMeisaiList_HenkyakuhinFlag;
                col = 14;
                checkLen = 1;
                field = Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG;
                if (!this.CheckAndSetExcelData(fncGetStr(col, row), row, dr, field, checkLen, itemName, ExcelInputAttrType.WideString, dtMessage, true, false, true, false))
                {
                    isAddData = false;
                }
                else
                {
                    //取り込んだ返却品が空でない場合
                    if (!string.IsNullOrEmpty(dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG].ToString()))
                    {
                        string flagValue = dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG].ToString();

                        if (flagValue.Equals(HENKYAKUHIN_FLAG.HENKYAKUHIN_VALUE1))
                        {
                            dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG] = HENKYAKUHIN_FLAG.HENKYAKUHIN_VALUE1;
                        }
                        else if (flagValue.Equals(HENKYAKUHIN_FLAG.KAKUNINCHU_VALUE1))
                        {
                            dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG] = HENKYAKUHIN_FLAG.KAKUNINCHU_VALUE1;
                        }
                        else if (flagValue.Equals(HENKYAKUHIN_FLAG.HORYU_VALUE1))
                        {
                            dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG] = HENKYAKUHIN_FLAG.HORYU_VALUE1;
                        }
                        else if (flagValue.Equals(HENKYAKUHIN_FLAG.DEFAULT_VALUE1))
                        {
                            dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG] = HENKYAKUHIN_FLAG.DEFAULT_VALUE1;
                        }
                        else
                        {
                            // Excel{0}行目、{1}の{2}がマスタに見つかりませんでした。
                            ComFunc.AddMultiMessage(dtMessage, "T0100012018", (row + 1).ToString(), itemName, flagValue);
                        }
                    }
                    else
                    {
                        // 列の値が空の場合、デフォルト値に設定
                        dr[Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG] = HENKYAKUHIN_FLAG.DEFAULT_VALUE1;
                    }
                }
                //////////////////////////////
                //　1行分のデータ読込完了
                //if (isAddData)
                //{
                //    dt.Rows.Add(dr);
                //}
                dt.Rows.Add(dr);
                is1st = false;
            }
            
            // 読み込みデータ件数を返す。
            this.Result.meisaiAdd = isAddData;
            this.Result.meisaiRows = dt.Rows.Count;
            //this.Result.meisai

            // データの状況に関わらず、処理正常終了を返す
            return true;
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
        /// <param name="isCheckFullWidth">全角検出チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>J.Chen 2022/10/18</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetExcelData(string target, int rowIndex, DataRow dr, string field, int checkLen, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary, bool isCheckLen, bool isCheckFullWidth)
        {
            object retVal = DBNull.Value;
            bool ret = true;
            string target2 = string.Empty;

            //日付チェック
            if (attrType == ExcelInputAttrType.Date)
            {
                try
                {
                    DateTime datetime;
                    if (DateTime.TryParse(target, out datetime))
                    {
                        // 日付文字列として認識した場合はそのまま（セル型が標準とか文字列とか）
                        target2 = target;
                    }
                    else
                    {
                        double dwDate;
                        if (Double.TryParse(target, out dwDate))
                        {
                            // Excelのセルが日付型だった場合、数値として帰ってくるので変換する
                            target2 = DateTime.FromOADate(UtilConvert.ToDouble(target)).Date.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            // それ以外の文字が来たら、とりあえず変換してみる
                            DateTime.ParseExact(target2, "yyyy/MM/dd", null).ToString();
                        }
                    }
                    retVal = target2;

                    // 過去日付判定
                    if (DateTime.TryParse(target2, out datetime))
                    {
                        if (datetime.Date < DateTime.Now.Date)
                        {
                            // {0}行目の{1}は過去の日付です。{2}
                            ComFunc.AddMultiMessage(dtMessage, "T0100012021", (rowIndex + 1).ToString(), itemName, target2);
                        }
                    }
                }
                catch
                {
                    // 日付として有効でない場合は以降の評価をしても無駄なのでここでエラー終了する
                    // {0}行目の{1}が日付に変換できませんでした。{2}
                    ComFunc.AddMultiMessage(dtMessage, "T0100012015", (rowIndex + 1).ToString(), itemName, target);
                }
            }
            else
            {
                //その他のチェック
                target2 = target;

                if (!this.CheckByteLength(target2, rowIndex, ref retVal, checkLen, itemName, dtMessage, isString, isCheckLen))
                {
                    ret = false;
                }
                if (!CheckRegulation(target2, rowIndex, itemName, attrType, dtMessage))
                {
                    ret = false;
                }
            }


            // 必須チェック
            if (isNecessary)
            {
                if (string.IsNullOrEmpty(target2))
                {
                    // Excel{0}行目の{1}が入力されていません。
                    ComFunc.AddMultiMessage(dtMessage, "T0100012012", (rowIndex + 1).ToString(), itemName);
                    //ret = false;
                }
            }

            //正常終了ならフィールドに値を戻す.
            if (ret)
            {
                dr[field] = retVal;
            }
            return ret;
        }

        #endregion

        #region ヘッダー取込データチェック処理（拡張版）
        /// --------------------------------------------------
        /// <summary>
        /// ヘッダー取込データチェック処理
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="field">データの戻し先</param>
        /// <param name="maxLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="attrType">文字属性</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isString">文字列かどうか</param>       ---
        /// <param name="isNecessary">必須かどうか</param>      ---
        /// <param name="isCheckLen">列長チェックするかどうか</param>   ---
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>J.Chen 2022/10/18/create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetExcelDataHeader(string target, int rowIndex, ref string field, int maxLen, string itemName, ExcelInputAttrType attrType, DataTable dtMessage, bool isString, bool isNecessary, bool isCheckLen)
        {
            object retVal = DBNull.Value;
            bool ret = true;

            //ヘッダー情報がエラーの場合は、無条件にエラー終了する
            //
            //　データレギュレーションチェック
            if (!CheckRegulationEx(target, rowIndex, itemName, attrType, dtMessage))
            {
                //手配情報のヘッダー部分が不正な場合は、メッセージ表示後に中断する.（TW-Tsuji）
                //　Excel{0}行目の{1}が登録できない文字列を含んでいます。
                this.ShowMessage("T0100012011", (rowIndex + 1).ToString(), itemName);
                return false;
            }

            //　文字数上限チェック
            if (!this.CheckByteLengthEx(target, rowIndex, ref retVal, maxLen, itemName, dtMessage, isString))
            {
                //手配情報のヘッダー部分が不正な場合は、メッセージ表示後に中断する.（TW-Tsuji）
                //　Excel{0}行目の{1}が登録できる{2}文字を超えています。
                this.ShowMessage("T0100012010", (rowIndex + 1).ToString(), itemName, maxLen.ToString());
                return false;
            }

            //　文字列の桁数厳密チェック
            if (isCheckLen == true && isString == true)
            {
                //　文字が入力されていない（Nullか空）の場合はチェックしない（別の未入力チェックを利用する）
                if (!string.IsNullOrEmpty(target))
                {
                    if (!(UtilString.GetByteCount(target) == maxLen))
                    {
                        //手配情報のヘッダー部分が不正な場合は、メッセージ表示後に中断する.（TW-Tsuji）
                        //　Excel{0}行目の{1}の文字数は、{2}文字で入力してください。
                        this.ShowMessage("T0100012013", (rowIndex + 1).ToString(), itemName, maxLen.ToString());
                        return false;
                    }
                }
            }

            //　必須入力チェック
            if (isNecessary)
            {
                if (string.IsNullOrEmpty(target))
                {
                    //手配情報のヘッダー部分が不正な場合は、メッセージ表示後に中断する.（TW-Tsuji）
                    //　Excel{0}行目の{1}が入力されていません。
                    this.ShowMessage("T0100012012", (rowIndex + 1).ToString(), itemName);
                    return false;
                }
            }

            //正常終了ならフィールドに値を戻す.
            field = target;
            return ret;
        }
        #endregion


        #region 最大バイトサイズチェック（No Messages）

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
        /// <create>J.Chen 2022/10/18</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckByteLengthEx(string target, int rowIndex, ref object value, int maxLen, string itemName, DataTable dtMessage, bool isString)
        {
            if (isString)
            {
                return this.CheckAndSetStringByteLengthEx(target, rowIndex, ref value, maxLen, itemName, dtMessage);
            }
            else
            {
                return this.CheckAndSetIntLengthEx(target, rowIndex, ref value, maxLen, itemName, dtMessage);
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
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>J.Chen 2022/10/18</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetStringByteLengthEx(string target, int rowIndex, ref object value, int maxLen, string itemName, DataTable dtMessage)
        {
            if (maxLen < UtilString.GetByteCount(target))
            {
                // {0}行目の{1}が登録できる文字数を超えています。       （TW-Tsuji）
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
        /// 全角文字列の有無チェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>TW-Tsuji 2022/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckFullWidthEx(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
        {
            if (isCheckLen && checkLen < UtilString.GetByteCount(target))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
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
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>J.Chen 2022/10/180</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetIntLengthEx(string target, int rowIndex, ref object value, int maxLen, string itemName, DataTable dtMessage)
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
                return false;
            }
            int result;
            if (!Int32.TryParse(target, System.Globalization.NumberStyles.Number, null, out result))
            {
                // {0}行目の{1}が数値に変換できませんでした。
                return false;
            }
            string checkStr = result.ToString();
            if (maxLen < UtilString.GetByteCount(checkStr))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                return false;
            }
            if (!isNullOrEmpty)
            {
                value = result;
            }
            return true;
        }

        #endregion


        #region 入力値の文字属性チェック（No Messages）

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
        /// <create>J.Chen 2022/10/18</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckRegulationEx(string target, int rowIndex, string itemName, ExcelInputAttrType attrType, DataTable dtMessage)
        {
            ComRegulation regulation = new ComRegulation();
            bool isUse = false;
            string targetType = string.Empty;

            switch (attrType)
            {
                case ExcelInputAttrType.AlphaNum:
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_LOW;
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_UP;
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    targetType += ComRegulation.REGULATION_NARROW_SIGN;
                    targetType += ComRegulation.REGULATION_NARROW_SPACE;
                    isUse = true;
                    break;
                case ExcelInputAttrType.WideString:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    break;
                case ExcelInputAttrType.Numeric:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                    isUse = true;
                    break;
                default:
                    break;
            }
            if (!regulation.CheckRegularString(target, targetType, isUse, false))
            {
                // Excel{0}行目の{1}が登録できない文字列を含んでいます。　　（TW-Tsuji）
                return false;
            }
            return true;
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
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isString, bool isCheckLen)
        {
            if (isString)
            {
                return this.CheckAndSetStringByteLength(target, rowIndex, ref value, checkLen, itemName, dtMessage, isCheckLen);
            }
            else
            {
                return this.CheckAndSetIntLength(target, rowIndex, ref value, checkLen, itemName, dtMessage, isCheckLen);
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
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetStringByteLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
        {
            if (isCheckLen && checkLen < UtilString.GetByteCount(target))
            {
                // Excel{0}行目の{1}が登録できる{2}文字を超えています。
                ComFunc.AddMultiMessage(dtMessage, "T0100012013", (rowIndex + 1).ToString(), itemName, checkLen.ToString());
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
        /// 全角文字列の有無チェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="value">取り込んだ値</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>TW-Tsuji 2022/10/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckFullWidth(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
        {
            if (isCheckLen && checkLen < UtilString.GetByteCount(target))
            {
                // {0}行目の{1}が登録できる文字数を超えています。
                ComFunc.AddMultiMessage(dtMessage, "S0100020010", (rowIndex + 1).ToString(), itemName);
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
        /// <param name="isCheckLen">列長チェックするかどうか</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckAndSetIntLength(string target, int rowIndex, ref object value, int checkLen, string itemName, DataTable dtMessage, bool isCheckLen)
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
                // Excel{0}行目の{1}が、整数値以外が入力されています。
                ComFunc.AddMultiMessage(dtMessage, "T0100012016", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            int result;
            if (!Int32.TryParse(target, System.Globalization.NumberStyles.Number, null, out result))
            {
                // Excel{0}行目の{1}が、数値に変換できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "T0100012017", (rowIndex + 1).ToString(), itemName);
                return false;
            }
            string checkStr = result.ToString();
            if (isCheckLen && checkLen < UtilString.GetByteCount(checkStr))
            {
                // Excel{0}行目の{1}が登録できる{2}文字を超えています。
                ComFunc.AddMultiMessage(dtMessage, "T0100012010", (rowIndex + 1).ToString(), itemName, checkLen.ToString());
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
        /// <create>H.Tajimi 2018/10/30</create>
        /// <update>TW-Tsuji 2022/10/25</update>
        /// --------------------------------------------------
        private bool CheckRegulation(string target, int rowIndex, string itemName, ExcelInputAttrType attrType, DataTable dtMessage)
        {
            ComRegulation regulation = new ComRegulation();
            bool isUse = false;
            string targetType = string.Empty;

            switch (attrType)
            {
                case ExcelInputAttrType.AlphaNum:
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_LOW;
                    targetType += ComRegulation.REGULATION_NARROW_ALPHA_UP;
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_ONLY;
                    targetType += ComRegulation.REGULATION_NARROW_SIGN;
                    targetType += ComRegulation.REGULATION_NARROW_SPACE;
                    isUse = true;
                    break;
                case ExcelInputAttrType.WideString:
                    targetType += ComRegulation.REGULATION_SURROGATE;
                    isUse = false;
                    break;
                case ExcelInputAttrType.Numeric:
                    targetType += ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                    isUse = true;
                    break;
                case ExcelInputAttrType.Date:
                    DateTime result;
                    if (!DateTime.TryParse(target, out result))
                    {
                        // Excel{0}行目の{1}が、日付に変換できませんでした。
                        ComFunc.AddMultiMessage(dtMessage, "T0100012015", (rowIndex + 1).ToString(), itemName);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    break;
            }
            if (!regulation.CheckRegularString(target, targetType, isUse, false))
            {
                // {0}行目の{1}が登録できない文字を含んでいます。{2}
                ComFunc.AddMultiMessage(dtMessage, "T0100012011", (rowIndex + 1).ToString(), itemName);
                return false;
            }

            return true;
        }

        #endregion


        #region 手配Noの文字列分割と存在チェック　2022/10/27 （TW-Tsuji）
        /// --------------------------------------------------
        /// <summary>
        /// 手配Noの文字列分割と存在チェック
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="dr">取込データのデータロウ</param>
        /// <param name="field">データロウのカラム名</param>
        /// <param name="checkLen">最大バイトサイズ</param>
        /// <param name="itemName">取込対象項目名</param>
        /// <param name="dtMessage">エラーテーブル</param>
        /// <returns>true:エラーなし/false:エラーあり</returns>
        /// <create>TW-Tsuji 2022/10/27</create>
        /// <update></update>
        /// <remarks>
        /// 2022/10/27　手配Noは8桁の番号を同一セル内に複数記入して登録する.
        ///             Excelに記入された番号文字列を、デリミタ毎に分割し、
        ///             それぞれの手配番号が有効かどうかチェックする.
        ///             尚、呼び出し元の画面では、ブランクで区切るため、
        ///             半角スペースのデリミタに補正して、dr（データロウ）に
        ///             直接返す.
        /// </remarks>
        /// --------------------------------------------------
        private bool CheckAndDelimitTehaiNo(string target, int rowIndex, DataRow dr,string field, int checkLen, string itemName, DataTable dtMessage)
        {
            string strReturn;

            // 加工する文字列があるかどうか
            string strBffer = target.Trim();
            if (strBffer.Length == 0)
            {
                strReturn = strBffer;
            }
            else
            {
                // 手配No文字列を、配列に変換する
                var strArr = strBffer.Split('+').ToArray();

                // 配列内の重複有無を検査
                if (!(strArr.GroupBy(i => i).Count() == strArr.Count()))
                {
                    // 重複があった場合、重複件数を引く
                    var duplicates = strArr.GroupBy(i => i)
                        .Where(i => i.Count() > 1)
                        .ToDictionary(i => i.Key, j => j.Count());

                    // Excel{0}行目の{1}、手配Noのデータに {2}件の重複データがあります。
                    ComFunc.AddMultiMessage(dtMessage, "T0100010040", (rowIndex + 1).ToString(), itemName, duplicates.Count().ToString());
                    return false;
                }

                strReturn = "";

                // 手配No検索用
                var conn = new ConnT01();
                var cond = new CondT01(this.UserInfo);
                DataSet ds = null;
                string errMsgID = null;
                string[] args = null;

                for (int i = 0; i < strArr.Length; i++)
                {
                    //文字数8桁をチェック
                    if (strArr[i].ToString().Length != checkLen)
                    {
                        // Excel {0}行目の{1}のデータが正しくありません。8桁で入力してください。{2}
                        ComFunc.AddMultiMessage(dtMessage, "T0100010037", (rowIndex + 1).ToString(), itemName, strArr[i].ToString());
                        return false;
                    }
                    //テーブルを検索
                    cond.TehaiNo = strArr[i];
                    cond.TehaiKindFlag = "";
                    ds = conn.GetSKSTehaiRenkeiTehaiSKSWork(cond, out errMsgID, out args);

                    // 連携済みチェック                         T_TEHAI_MEISAI_SKS
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        // Excel{0}行目の{1}のデータに連携済みチェックエラーがあります。{2}
                        ComFunc.AddMultiMessage(dtMessage, "T0100010038", (rowIndex + 1).ToString(), itemName, strArr[i].ToString());
                        return false;
                    }

                    // 存在チェック                             T_TEHAI_SKS_WORK 2022/10/27
                    //if (!UtilData.ExistsData(ds, Def_T_TEHAI_SKS_WORK.Name))
                    //{
                    //    // Excel{0}行目の{1}のデータに存在チェックエラーがあります。{2}
                    //    ComFunc.AddMultiMessage(dtMessage, "T0100010039", (rowIndex + 1).ToString(), itemName, strArr[i].ToString());
                    //    return false;
                    //}
                    
                    // チェック完了（文字列に追加する）
                    if (!string.IsNullOrEmpty(strArr[i]))
                    {
                        //先頭行でない場合はデリミタで区切る
                        if (i > 0)
                            strReturn += "+";    //整形するときのデリミタ
                        //１件データを追加
                        strReturn += strArr[i].ToString();                     
                    }
                }
            }
            
            //全件チェックが完了したら、データを戻す
            dr[field] = strReturn;
            return true;
        }
        #endregion

        #endregion
    }
}
