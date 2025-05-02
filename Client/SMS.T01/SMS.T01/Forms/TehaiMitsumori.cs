using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using DSWUtil;
using WsConnection;
using Commons;
using GrapeCity.Win.ElTabelle;
using SMS.T01.Properties;
using WsConnection.WebRefT01;
using SMS.E01;
using WsConnection.WebRefAttachFile;
using System.IO;
using WsConnection.WebRefCommon;
using SystemBase.Util;
using WsConnection.WebRefS01;
using Ionic.Zip;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積
    /// </summary>
    /// <create>S.Furugo 2018/11/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiMitsumori : SystemBase.Forms.CustomOrderForm
    {
        #region プロパティ
        /// --------------------------------------------------
        /// <summary>
        /// InitializeShownControl後に呼び出す処理
        /// </summary>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected Action AfterInitializeShownControl { get; set; }
        #endregion

        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>S.Furugo 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>S.Furugo 2018/11/30</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>S.Furugo 2018/11/30</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch
        }
        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>S.Furugo 2018/12/7</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_BUKKEN_NAME = 0;
        /// --------------------------------------------------
        /// <summary>
        /// AR No.
        /// </summary>
        /// <create>S.Furugo 2018/12/7</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_AR_NO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_KEISHIKI = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI_JP = 3;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 発注数の列インデックス
        /// </summary>
        /// <create>J.Chen 2023/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HACCHU_QTY = 5;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷数の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_QTY = 6;
        /// --------------------------------------------------
        /// <summary>
        /// 単価の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNIT_PRICE = 7;
        /// --------------------------------------------------
        /// <summary>
        /// 単価 販管の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNIT_PRICE_SALSE = 8;
        /// --------------------------------------------------
        /// <summary>
        /// 単価 RMBの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_UNIT_PRICE_RMB = 9;
        /// --------------------------------------------------
        /// <summary>
        /// 合計の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SUM_RMB = 10;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃込単価 RMBの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ROB_UNIT_PRICE_RMB = 11;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃込合計 RMBの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ROB_SUM_RMB = 12;
        /// --------------------------------------------------
        /// <summary>
        /// 変更履歴の列インデックス
        /// </summary>
        /// <create>J.Chen 2024/10/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ESTIMATE_RIREKI = 13;
        /// --------------------------------------------------
        /// <summary>
        /// バージョン
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update>J.Chen 2024/10/29 変更履歴列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_VERSION = 14;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷済み明細件数
        /// </summary>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update>J.Chen 2024/10/29 変更履歴列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CNT = 15;
        /// --------------------------------------------------
        /// <summary>
        /// 手配連携Noの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/5</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update>J.Chen 2024/10/29 変更履歴列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_RENKEI_NO = 16;
        /// --------------------------------------------------
        /// <summary>
        /// INVOICE単価の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/20</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update>J.Chen 2024/10/29 変更履歴列の追加によりインデックスが一つ増やす</update>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_INVOICE_UNIT_PRICE = 17;
        /// --------------------------------------------------
        /// <summary>
        /// 手配Flagの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TEHAI_FALG = 18;
        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタVALUE2の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COMMON_VALUE2 = 5;
        /// --------------------------------------------------
        /// <summary>
        /// 汎用マスタVALUE3の列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int COMMON_VALUE3 = 6;
        /// --------------------------------------------------
        /// <summary>
        /// 売上予定月NULL状態
        /// </summary>
        /// <create>J.Chen 2023/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string NULL_PROJECTED_SALES_MONTH = "　　　/　　";
        #endregion

        #region フィールド定義

        /// --------------------------------------------------
        /// <summary>
        /// 手配情報登録画面からの起動
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isFromTehaiMeisai = false;
        /// --------------------------------------------------
        /// <summary>
        /// メール送信必要かどうか
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool isMailRequired = false;
        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザのメールアドレス
        /// </summary>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 見積メール用ユーザマスタ
        /// </summary>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtQuotationUser = null;
        /// --------------------------------------------------
        /// <summary>
        /// ProjectNo
        /// </summary>
        /// <create>J.Chen 2024/08/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _projectNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 累計契約金額（メール送信用）
        /// </summary>
        /// <create>J.Chen 2024/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private decimal _totalPOAmount = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 累計仕切り金額（メール送信用）
        /// </summary>
        /// <create>J.Chen 2024/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private decimal _totalPartitionAmount = 0;

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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMitsumori(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen 2023/12/20 MailTitle、荷受先コンボボックス追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {

                // コンボボックス
                this.MakeCmbBox(this.cboCurrency, CURRENCY_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboMailTitle, ESTIMATE_MAIL_SUBJECT.GROUPCD);
                // 荷受先コンボボックスの初期化
                this.InitializeComboNiukesaki();

                //this.cboCurrency.SelectedIndex = -1;
                this.cboMailTitle.SelectedIndex = -1;
                this.cboNiukesaki.SelectedIndex = -1;

                this.ChangeMode(DisplayMode.Initialize);

                // シート設定
                this.InitializeSheet(shtTehaiMitsumori);

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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // ラジオボタンの初期チェック位置が新規で見積No.のテキストボックスを無効に
                rdoInsert.Checked = true;
                txtEstimateNo.Enabled = false;

                // 追加の初期化処理がある場合は初期設定が完了した後で処理を行う
                if (AfterInitializeShownControl != null)
                    AfterInitializeShownControl();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>D.Okumura 2018/12/16</create>
        /// <update>J.Chen 2023/12/18 発注数列の追加によりインデックスが一つ増やす</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            sheet.KeepHighlighted = true;
            sheet.AllowUserToAddRows = false;

            // シートのタイトルを設定
            sheet.ColumnHeaders[0].Caption = Resources.TehaiMitsumori_BukkenName;
            sheet.ColumnHeaders[1].Caption = Resources.TehaiMitsumori_ARNo;
            sheet.ColumnHeaders[2].Caption = Resources.TehaiMitsumori_ZumenKeishiki;
            sheet.ColumnHeaders[3].Caption = Resources.TehaiMitsumori_HinmeiJp;
            sheet.ColumnHeaders[4].Caption = Resources.TehaiMitsumori_Hinmei;
            sheet.ColumnHeaders[5].Caption = Resources.TehaiMitsumori_HacchuQty;
            sheet.ColumnHeaders[6].Caption = Resources.TehaiMitsumori_ShukkaQty;
            sheet.ColumnHeaders[7].Caption = Resources.TehaiMitsumori_UnitPrice;
            sheet.ColumnHeaders[8].Caption = Resources.TehaiMitsumori_UnitPriceSalsesPer;
            sheet.ColumnHeaders[9].Caption = Resources.TehaiMitsumori_UnitPriceRMB;
            sheet.ColumnHeaders[10].Caption = Resources.TehaiMitsumori_SumRMB;
            sheet.ColumnHeaders[11].Caption = Resources.TehaiMitsumori_ROBUnitPriceRMB;
            sheet.ColumnHeaders[12].Caption = Resources.TehaiMitsumori_ROBSumRMB;
            sheet.ColumnHeaders[13].Caption = Resources.TehaiMitsumori_EstimateRireki;
        }
        #endregion

        #region 荷受先コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboNiukesaki()
        {
            var conn = new ConnS01();
            var cond = new CondT01(this.UserInfo);
            var ds = conn.GetConsignList();
            if (!UtilData.ExistsData(ds, Def_M_CONSIGN.Name))
            {
                return;
            }

            // 荷受先コンボボックスの初期化
            var dt = ds.Tables[Def_M_CONSIGN.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_CONSIGN.CONSIGN_CD, null);
            UtilData.SetFld(dr, Def_M_CONSIGN.NAME, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboNiukesaki.ValueMember = Def_M_CONSIGN.CONSIGN_CD;
            this.cboNiukesaki.DisplayMember = Def_M_CONSIGN.NAME;
            this.cboNiukesaki.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {
                this.cboNiukesaki.SelectedValue = decimal.MinValue;
            }
            else
            {
                this.cboNiukesaki.SelectedIndex = -1;
            }
        }

        #endregion

        #region 画面連携
        /// --------------------------------------------------
        /// <summary>
        /// 手配見積詳細画面から呼び出される処理(F4)
        /// </summary>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void StartEdit(string estimateNo)
        {
            AfterInitializeShownControl = () =>
            {
                // 編集ボタンを選択し、連携された見積番号を入力する
                rdoEdit.Checked = true;
                txtEstimateNo.Text = estimateNo;
                // 開始ボタンを押下する
                RunSearch();
                // 念のためイベントをクリアする
                AfterInitializeShownControl = null;
            };
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積詳細画面から呼び出される処理(F1)
        /// </summary>
        /// <param name="datarows">行データ</param>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void StartEdit(IEnumerable<DataRow> datarows)
        {
            AfterInitializeShownControl = () =>
            {
                // 新規ボタンを選択する
                rdoInsert.Checked = true;
                // 開始ボタンを押下する
                RunSearch();
                string msgId;
                string[] msgParams;
                if (!AddDataRow(datarows, out msgId, out msgParams))
                {
                    //処理なし、原則エラーとなることはない
                }
                // 念のためイベントをクリアする
                AfterInitializeShownControl = null;
            };
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配見積明細テーブルからのデータ連携
        /// </summary>
        /// <param name="datarow">データ行</param>
        /// <param name="msgId">エラーメッセージID</param>
        /// <param name="msgParams">エラーメッセージのパラメータ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update>J.Chen 2024/03/14 見積チェック追加</update>
        /// <update></update>
        /// --------------------------------------------------
        public bool AddDataRow(IEnumerable<DataRow> datarows, out string msgId, out string[] msgParams)
        {
            msgId = null;
            msgParams = null;

            var datarow = datarows.FirstOrDefault();
            var sheet = this.shtTehaiMitsumori;
            DataTable dt = sheet.DataSource as DataTable;
            //データテーブルがない場合は呼び出し元のデータシートからデータソースを生成する
            if (dt == null)
            {
                dt = datarow.Table.Clone();
                this.shtTehaiMitsumori.DataSource = dt;
            }
            // 宛先の列情報を取得する
            var to = Enumerable.Range(0, sheet.Columns.Count)
                .Select(i => sheet.Columns[i])
                .ToDictionary(k => k.DataField, v => v.Index);

            // 両方に存在する列の一覧を作成する
            var fromCols = datarow.Table.Columns.Cast<DataColumn>()
                .Select(item => item.ColumnName)
                .Where(col => to.ContainsKey(col))
                .ToArray();

            // 既に登録されているものがないかチェックする
            //var tehaiRenkeiNos = sheet.Columns[SHEET_COL_TEHAI_RENKEI_NO].ValueBlock.Cast<string>().ToArray();
            var tehaiRenkeiNos = dt.AsEnumerable().Select(item => item.Field<string>(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)).ToArray();
            var error = datarows.FirstOrDefault(w => tehaiRenkeiNos.Contains(w.Field<string>(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            if (error != null)
            {
                // 手配連携No{0}は既に追加されています。
                msgId = "T0100040010";
                msgParams = new[] { error.Field<string>(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) };
                return false;
            }

            // dtの図番型式、単価、品名を保存する
            var dtData = dt.AsEnumerable().Select(item => new
            {
                ZUBAN = item.Field<string>(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI),
                TANKA = item.Field<decimal>(Def_T_TEHAI_MEISAI.UNIT_PRICE),
                HINMEI = item.Field<string>(Def_T_TEHAI_MEISAI.HINMEI)
            }).ToArray();

            // エラーデータを保存するリスト
            List<object[]> errorDataList = new List<object[]>();

            // datarowsのZUBANがdtのZUBANと一致し、かつTANKAまたはHINMEIが異なるデータを保存する
            foreach (var row in datarows)
            {
                var zuban = row.Field<string>(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
                var matchingData = dtData.FirstOrDefault(d => d.ZUBAN == zuban);

                if (matchingData != null)
                {
                    var tanka = row.Field<decimal>(Def_T_TEHAI_MEISAI.UNIT_PRICE);
                    var hinmei = row.Field<string>(Def_T_TEHAI_MEISAI.HINMEI);
                    var tehaiRenkeiNo = row.Field<string>(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);

                    if (matchingData.TANKA != tanka || matchingData.HINMEI != hinmei)
                    {
                        // エラーデータを配列として保存
                        errorDataList.Add(new object[] { tehaiRenkeiNo, zuban, tanka, hinmei });
                    }
                }
            }

            if (errorDataList.Count > 0)
            {
                // 既に追加されたデータと同型式品で単価・品名が一致しない項目があるため見積もりを作成できません。\r\n手配連携No.:{0}
                msgId = "T0100050013";
                msgParams = new[] { errorDataList[0][0].ToString() };
                return false;
            }


            // 新規行を追加する
            sheet.Redraw = false;
            try
            {

                var currentRow = (sheet.MaxRows == 0) ? 0 : sheet.MaxRows - (sheet.AllowUserToAddRows ? 1 : 0);
                sheet.InsertRow(currentRow, datarows.Count(), false);

                // 情報を反映する(dataRow→sheet)
                int cnt = 0;
                foreach (var row in datarows)
                {
                    foreach (var col in fromCols)
                    {
                        sheet[to[col], currentRow + cnt].Value = row[col];
                    }
                    cnt++;
                }
            }
            finally
            {
                sheet.Redraw = true;
            }

            //再計算を行う(強制)
            Calculate(true);
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配情報登録画面から呼び出される処理
        /// </summary>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public void StartEditForTehaiMeisai(string estimateNo)
        {
            AfterInitializeShownControl = () =>
            {
                // 編集ボタンを選択し、連携された見積番号を入力する
                rdoEdit.Checked = true;
                txtEstimateNo.Text = estimateNo;

                isFromTehaiMeisai = true;
                // 一部のコントロールを無効化する
                SetControlsDisable();

                // 開始ボタンを押下する
                RunSearch();

                // 受注金額が変更されました。見積差し替えのメールを送信してください。
                this.AppendMessage("T0100040016");

                // 念のためイベントをクリアする
                AfterInitializeShownControl = null;
            };
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen 2022/04/21 STEP14</update>
        /// <update>J.Chen 2023/12/18 項目追加、コントロール無効制御</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                txtEstimateName.Clear();
                txtRateJPY.Clear();
                txtSalsesPer.Clear();
                txtRobPer.Clear();

                txtPrePartsCost.Clear();
                txtPrePOAmount.Clear();
                txtPrePartitionAmount.Clear();
                txtPreROB.Clear();
                txtPrePartitionAmount.Clear();
                txtCurrentPartsCost.Clear();
                txtCurrentPOAmount.Clear();
                txtCurrentROB.Clear();
                txtEstimateVersion.Clear();
                txtCurrentPartitionAmount.Clear();

                txtPONo.Clear();

                txtUpdateUserName.Clear();

                this.ChangeMode(DisplayMode.Initialize);

                //cboCurrency.Text = null;
                cboCurrency.SelectedIndex = -1;
                cboMailTitle.Text = null;
                cboMailTitle.SelectedIndex = -1;
                cboNiukesaki.Text = null;
                cboNiukesaki.SelectedIndex = -1;

                txtRev.Clear();
                txtRatePartition.Clear();
                // 売上予定月NULL設定
                mpProjectedSalesMonth.Value = DateTime.Now;
                this.mpProjectedSalesMonth.CustomFormat = NULL_PROJECTED_SALES_MONTH;

                SheetClear();

                // 一部のコントロールを無効化する
                SetControlsDisable();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearAll()
        {
            try
            {
                this.rdoInsert.Checked = true;
                txtEstimateNo.Enabled = false;
                txtEstimateNo.Clear();

                this.DisplayClear();
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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (!ret)
                    return false;
                if (string.IsNullOrEmpty(txtEstimateName.Text))
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblEstimateName.Text);
                    txtEstimateName.Focus();
                    return false;
                }

                if (this.shtTehaiMitsumori.Rows.Count == 0)
                {
                    // シートにデータが存在しません。
                    this.ShowMessage("T0100040003");
                    return false;
                }
                // 計算関連の入力チェック
                ret = CheckInputInner(true);
                if (!ret)
                    return false;
                // 再計算を実施
                ret = Calculate(true);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="isShowMsg">エラー時のメッセージ表示有無</param>
        /// <returns>true:OK/false:NG</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update>J.Chen 2024/02/07 通貨、仕切りレートのチェックを追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected bool CheckInputInner(bool isShowMsg)
        {
            if (string.IsNullOrEmpty(cboCurrency.Text))
            {
                if (isShowMsg)
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblCurrency.Text);
                    this.cboCurrency.Focus();
                }
                return false;
            }
            if (string.IsNullOrEmpty(txtRateJPY.Text))
            {
                if (isShowMsg)
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblRateJPY.Text);
                    this.txtRateJPY.Focus();
                }
                return false;
            }

            if (string.IsNullOrEmpty(txtSalsesPer.Text))
            {
                if (isShowMsg)
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblSalsesPer.Text);
                    this.txtSalsesPer.Focus();
                }
                return false;
            }
            if (string.IsNullOrEmpty(txtRobPer.Text))
            {
                if (isShowMsg)
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblRobPer.Text);
                    this.txtRobPer.Focus();
                }
                return false;
            }
            if (txtRateJPY.Value == 0m)
            {
                if (isShowMsg)
                {
                    // ER(JPY)は0以外を入力してください。
                    this.ShowMessage("T0100040002");
                    this.txtRateJPY.Focus();
                }
                return false;
            }
            //if (txtRatePartition.Value == 0m)
            //{
            //    if (isShowMsg)
            //    {
            //        // 仕切りレートは0以外を入力してください。
            //        this.ShowMessage("T0100040021");
            //        this.txtRatePartition.Focus();
            //    }
            //    return false;
            //}
            return true;

        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if ((rdoEdit.Checked
                    || rdoView.Checked
                    || rdoAllDelete.Checked)
                    && string.IsNullOrEmpty(txtEstimateNo.Text))
                {
                    // シートのクリア
                    this.SheetClear();
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblEstimateNo.Text);
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
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>S.Furugo 2018/11/30</create>
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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen 2022/04/21 STEP14</update>
        /// <update>J.Chen 2023/12/18 項目追加、金額整合性チェック、コントロール無効制御</update>
        /// <update>J.Chen 2024/08/08 メール用累計金額追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                if (rdoInsert.Checked)
                {
                    // 新規追加の場合
                    var dr = GetCommonEstimateRate();
                    var dt = this.GetCommon(PARTITION_RATE.GROUPCD).Tables[Def_M_COMMON.Name];

                    // 初期値を設定
                    txtSalsesPer.Text = dr == null ? "" : ComFunc.GetFld(dr, Def_M_COMMON.VALUE2);
                    txtRobPer.Text = dr == null ? "" : ComFunc.GetFld(dr, Def_M_COMMON.VALUE3);
                    txtRatePartition.Text = dt.Rows.Count > 0 ? ComFunc.GetFld(dt.Rows[0], Def_M_COMMON.VALUE1) : PARTITION_RATE.PARTITION_RATE_VALUE1;
                    ChangeMode(DisplayMode.EndSearch);
                }
                else
                {
                    CondT01 cond = new CondT01(this.UserInfo);
                    ConnT01 conn = new ConnT01();
                    // 見積番号
                    cond.MitsumoriNo = txtEstimateNo.Text;

                    DataSet dsEstimate = conn.GetEstimateInformation(cond);

                    if (!ComFunc.IsExistsData(dsEstimate, Def_T_TEHAI_ESTIMATE.Name))
                    {
                        // 該当する手配明細見積データはありません。
                        this.ShowMessage("T0100040004");
                        return false;
                    }
                    var dtEstimate = dsEstimate.Tables[Def_T_TEHAI_ESTIMATE.Name];

                    if (!ComFunc.IsExistsData(dsEstimate, Def_T_TEHAI_MEISAI.Name))
                    {
                        // シートのクリア
                        this.SheetClear();
                        // 該当する手配明細はありません。
                        this.ShowMessage("T0100040005");
                        return false;
                    }

                    // 画面状態を変更
                    ChangeMode(DisplayMode.EndSearch);

                    var txtPoNo = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.PO_NO);
                    var hasPoNo = !string.IsNullOrEmpty(txtPoNo);

                    switch (EditMode)
                    {
                        case SystemBase.EditMode.Delete:
                        case SystemBase.EditMode.Update:
                            if (hasPoNo)
                            {
                                // 初期状態へ戻す
                                this.DisplayClear();
                                // 受注済みです。
                                this.ShowMessage("T0100040006");
                                return false;
                            }
                            txtPONo.Enabled = false;
                            this.fbrFunction.F09Button.Enabled = false;
                            this.fbrFunction.F10Button.Enabled = false;
                            break;
                        case SystemBase.EditMode.View:
                            txtPONo.Enabled = !hasPoNo;
                            this.fbrFunction.F09Button.Enabled = !hasPoNo;
                            this.fbrFunction.F10Button.Enabled = hasPoNo;
                            break;
                        default:

                            // 画面状態を変更
                            ChangeMode(DisplayMode.Initialize);
                            return false;
                    }

                    this.txtPONo.Text = txtPoNo;
                    this.txtUpdateUserName.Text = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.UPDATE_USER_NAME);
                    this.txtEstimateName.Text = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.NAME);
                    this.cboCurrency.SelectedValue = dtEstimate.Rows[0][Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG];
                    this.txtRateJPY.Value = ComFunc.GetFldToDecimal(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.RATE_JPY);
                    this.txtSalsesPer.Value = ComFunc.GetFldToDecimal(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.SALSES_PER);
                    this.txtRobPer.Value = ComFunc.GetFldToDecimal(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.ROB_PER);
                    this.txtEstimateVersion.Text = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.VERSION);

                    // 売上予定月
                    string projectedSalesMonthValue = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH);
                    if (string.IsNullOrEmpty(projectedSalesMonthValue))
                    {
                        this.mpProjectedSalesMonth.CustomFormat = NULL_PROJECTED_SALES_MONTH;
                    }
                    else 
                    {
                        this.mpProjectedSalesMonth.CustomFormat = "yyyy/MM";
                        this.mpProjectedSalesMonth.Text = projectedSalesMonthValue;
                    }
                    

                    this.cboMailTitle.SelectedValue = dtEstimate.Rows[0][Def_T_TEHAI_ESTIMATE.MAIL_TITLE];
                    this.txtRev.Text = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.REV);
                    this.cboNiukesaki.SelectedValue = dtEstimate.Rows[0][Def_T_TEHAI_ESTIMATE.CONSIGN_CD];
                    this.txtRatePartition.Text = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.RATE_PARTITION);


                    this.shtTehaiMitsumori.DataSource = dsEstimate.Tables[Def_T_TEHAI_MEISAI.Name];

                    // 再計算前に手配明細を保存
                    var dtTehaimeisai = dsEstimate.Tables[Def_T_TEHAI_MEISAI.Name].Copy();

                    //再計算を行う(強制)
                    Calculate(true);


                    // 編集の場合のみ前回値へ反映
                    if (EditMode == SystemBase.EditMode.Update)
                    {
                        this.txtPrePartsCost.Value = txtCurrentPartsCost.Value;
                        this.txtPrePOAmount.Value = txtCurrentPOAmount.Value;
                        this.txtPrePartitionAmount.Value = txtCurrentPartitionAmount.Value;
                        this.txtPreROB.Value = txtCurrentROB.Value;
                        this.txtPrePartitionAmount.Value = txtCurrentPartitionAmount.Value;
                    }
                    else
                    {
                        this.txtPrePartsCost.Clear();
                        this.txtPrePOAmount.Clear();
                        this.txtPrePartitionAmount.Clear();
                        this.txtPreROB.Clear();
                        this.txtPrePartitionAmount.Clear();
                    }

                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtTehaiMitsumori.MaxRows)
                    {
                        this.shtTehaiMitsumori.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTehaiMitsumori.TopLeft.Row);
                    }

                    // 照会モードの場合、DBとの金額をチェック
                    if (EditMode == SystemBase.EditMode.View)
                    {
                        // ProjectNo取得
                        this._projectNo = ComFunc.GetFld(dsEstimate.Tables[Def_T_TEHAI_MEISAI.Name], 0, Def_M_PROJECT.PROJECT_NO);

                        // PO金額と仕切り金額を取得
                        var poAmount = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.PO_AMOUNT);
                        var partitionAmount = ComFunc.GetFld(dtEstimate, 0, Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT);

                        // PO金額と仕切り金額で比較する
                        if (!string.IsNullOrEmpty(poAmount) && !string.IsNullOrEmpty(partitionAmount))
                        {
                            // 画面のPO金額と仕切り金額を取得
                            string currentPOAmount = string.IsNullOrEmpty(this.txtCurrentPOAmount.Text) ? "" : this.txtCurrentPOAmount.Text.Replace(",", "");
                            string currentPartitionAmount = string.IsNullOrEmpty(this.txtCurrentPartitionAmount.Text) ? "" : this.txtCurrentPartitionAmount.Text.Replace(",", "");

                            if (poAmount != currentPOAmount || partitionAmount != currentPartitionAmount)
                            {
                                // システムの表示金額がデータベースに保存された金額と一致していません。見積金額確定後に「編集」より保存してください。※保存すると一致します。
                                this.ShowMessage("T0100040017");
                            }
                        }
                        else // Invoice単価で比較する
                        {
                            // 現在のデータを取得（再計算後）
                            DataTable dtCurrent = this.shtTehaiMitsumori.DataSource as DataTable;

                            if (dtTehaimeisai.Rows.Count == dtCurrent.Rows.Count)
                            {
                                for (int i = 0; i < dtTehaimeisai.Rows.Count; i++)
                                {
                                    var sourcePrice = ComFunc.GetFld(dtTehaimeisai, i, Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE);
                                    var currentPrice = ComFunc.GetFld(dtCurrent, i, Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE);
                                    if (sourcePrice != currentPrice)
                                    {
                                        // システムの表示金額がデータベースに保存された金額と一致していません。見積金額確定後に「編集」より保存してください。※保存すると一致します。
                                        this.ShowMessage("T0100040017");
                                        break;
                                    }
                                }
                            }
                        }
                    
                    }


                }
                // 一部のコントロールを無効化する
                SetControlsDisable();

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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>D.Okumura 2019/02/22 再検索実行処理追加</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 手配情報登録から起動、かつ保存を実行した場合、メール送信が必要になる
                    if (isFromTehaiMeisai)
                    {
                        isMailRequired = true;
                    }

                    this.flpSearchCondition.Focus();
                    // 再検索を実行、EditModeはNoneになっているので、画面状態から判断する
                    if (rdoInsert.Checked)
                        rdoEdit.Checked = true;

                    // 全削除の場合はクリアする
                    if (rdoAllDelete.Checked)
                    {
                        this.DisplayClearAll();
                        return ret;
                    }
                    RunSearch();
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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                // 登録データ作成
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();
                DataSet ds = new DataSet();
                DataTable dtEstimate = this.GetTehaiEstimateFromDisplay();
                DataTable dtSht = this.shtTehaiMitsumori.DataSource as DataTable;
                if (dtSht == null)
                    return false;
                dtSht = dtSht.Copy();
                dtSht.TableName = ComDefine.DTTBL_INSERT;
                ds.Tables.Add(dtEstimate);
                ds.Tables.Add(dtSht);

                string errMsgID;
                string[] args;
                string estimateNo;
                if (!conn.InsTehaiMitsumori(cond, ds, out estimateNo, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                txtEstimateNo.Text = estimateNo;
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
        /// <create>S.Furugo 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 更新用データテーブル取得
                DataTable dt = this.shtTehaiMitsumori.DataSource as DataTable;
                if (dt == null) return false;

                DataTable dtInsert = this.GetDataTehaiMeisai(dt, DataRowState.Added);      // 追加データ抽出
                DataTable dtUpdate = this.GetDataTehaiMeisai(dt, DataRowState.Modified);   // 更新データ抽出
                DataTable dtDelete = this.GetDataTehaiMeisai(dt, DataRowState.Deleted);    // 削除データ抽出

                DataSet ds = new DataSet();
                if (dtInsert != null)
                    ds.Tables.Add(dtInsert);
                if (dtUpdate != null)
                    ds.Tables.Add(dtUpdate);
                if (dtDelete != null)
                    ds.Tables.Add(dtDelete);

                ds.Tables.Add(this.GetTehaiEstimateFromDisplay());
                
                // DB更新
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                cond.MitsumoriNo = this.txtEstimateNo.Text;

                string errMsgID;
                string[] args;
                if (!conn.UpdTehaiMitsumori(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
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
        /// --------------------------------------------------
        /// <summary>
        /// 修正処理(受注状態反映)
        /// </summary>
        /// <returns>処理結果</returns>
        /// <create>D.Okumura 2018/12/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool RunEditUpdatePoNo()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtSht = this.shtTehaiMitsumori.DataSource as DataTable;
                if (dtSht == null)
                    return false;
                dtSht = dtSht.Copy();
                dtSht.TableName = ComDefine.DTTBL_UPDATE;
                ds.Tables.Add(dtSht); // 念のため手配明細もチェックする
                ds.Tables.Add(this.GetTehaiEstimateFromDisplay());

                // DB更新
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                cond.MitsumoriNo = txtEstimateNo.Text;
                cond.PONo = txtPONo.Text;

                string errMsgID;
                string[] args;
                if (!conn.UpdTehaiMitsumoriOrder(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
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
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                // 削除データ作成
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();
                DataSet ds = new DataSet();
                DataTable dtEstimate = this.GetTehaiEstimateFromDisplay();
                DataTable dtSht = this.shtTehaiMitsumori.DataSource as DataTable;
                if (dtSht == null)
                    return false;
                dtSht = dtSht.Copy();
                dtSht.TableName = ComDefine.DTTBL_DELETE;
                ds.Tables.Add(dtEstimate);
                ds.Tables.Add(dtSht);

                string errMsgID;
                string[] args;
                if (!conn.DelTehaiMitsumori(cond, ds, out errMsgID, out args))
                {
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

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック(保存)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen メール送信フラグ追加</update>
        /// <update></update>
        /// <remarks>
        /// 既定の処理に任せてRunEditが呼ばれるように仕向ける
        /// </remarks>
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
        /// F2ボタンクリック(明細追加)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                using (TehaiMitsumoriMeisai frm = new TehaiMitsumoriMeisai(this.UserInfo, true, ComDefine.TITLE_T0100050))
                {
                    // 見積明細にて、親フォームを辿るため、情報の受け渡しは不要
                    if (frm.ShowDialog(this) != DialogResult.OK)
                    {
                        //フォーム処理内で、AddDataRowが呼び出されるため、ここでは何もしない
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック(行削除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            base.fbrFunction_F03Button_Click(sender, e);
            try
            {

                int row = this.shtTehaiMitsumori.ActivePosition.Row;
                // 新規追加行の場合、なにもしない
                if (this.shtTehaiMitsumori.AllowUserToAddRows)
                {
                    var max = this.shtTehaiMitsumori.Rows.Count;
                    if (max <= 1 || ((max - 1) <= row)) return;
                }

                // {0}行目を削除してもよろしいですか？
                //string renkeiNo = this.shtTehaiMitsumori[SHEET_COL_TEHAI_RENKEI_NO, row].Text;
                if (this.ShowMessage("T0100040008", (row + 1).ToString()) == DialogResult.OK)
                {
                    this.shtTehaiMitsumori.Redraw = false;
                    this.shtTehaiMitsumori.RemoveRow(row, false);
                    //this.shtTehaiMitsumori.UpdateData();  // レコードの状態が分からなくなるため実施しない
                    this.shtTehaiMitsumori.Redraw = true;
                    this.shtTehaiMitsumori.Focus();
                }

                Calculate(true);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック(MAIL)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                bool ret = this.RunMail();

                // メール送信成功後
                if (ret)
                {
                    if (isMailRequired)
                    {
                        isMailRequired = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック(見積出力)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                this.RunExcelAndPdfOnly();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック(Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック(All Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }

                this.DisplayClearAll();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F8ボタンクリック (Excel出力)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
        /// <update>J.Chen メール送信対応のため、メソッドを抽出</update>
        /// <remarks>
        /// 必ずSheetへデータセットを反映してから出力可能にすること
        /// </remarks>
        /// --------------------------------------------------
        protected override void fbrFunction_F08Button_Click(object sender, EventArgs e)
        {
            ClearMessage();
            base.fbrFunction_F08Button_Click(sender, e);
            try
            {
                this.RunExcelOnly();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F9ボタンクリック(受注)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F09Button_Click(object sender, EventArgs e)
        {
            ClearMessage();
            base.fbrFunction_F09Button_Click(sender, e);
            try
            {
                if (string.IsNullOrEmpty(txtPONo.Text))
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblPONo.Text);
                    this.txtPONo.Focus();
                    return;
                }
                // 受注しますか？
                if (this.ShowMessage("T0100040011") != DialogResult.OK)
                    return;

                // 更新を実施
                if (this.RunEditUpdatePoNo())
                {
                    // 保存しました
                    this.ShowMessage("A9999999045");

                    // 再検索
                    RunSearch();
                }

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック(受注解除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen 2022/04/21 STEP14</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 受注を解除しますか？
                if (this.ShowMessage("T0100040012") != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    txtPONo.Clear();
                }


                // 更新を実施
                if (this.RunEditUpdatePoNo())
                {
                    // 保存しました
                    this.ShowMessage("A9999999045");

                    // 再検索
                    RunSearch();
                }

            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタンクリック

        #region 開始ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 開始クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/26</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 計算ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 計算クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessage();
                if (!CheckInputInner(true))
                    return;
                Calculate(true);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 運賃等が変更の時
        /// --------------------------------------------------
        /// <summary>
        /// 運賃等の値が変化したときの動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtEstimateValue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Calculate(false);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
        #endregion

        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtTehaiMitsumori.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtTehaiMitsumori.MaxRows)
            {
                this.shtTehaiMitsumori.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtTehaiMitsumori.TopLeft.Row);
            }
            this.shtTehaiMitsumori.DataSource = null;
            this.shtTehaiMitsumori.MaxRows = 0;
            this.shtTehaiMitsumori.Enabled = false;
            this.shtTehaiMitsumori.Redraw = true;
        }

        #endregion

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <update>J.Chen 2024/01/19 F04MAIL、F05見積出力を追加</update>
        /// <update>J.Chen 2024/10/29 変更履歴追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            bool isEditable = false;
            bool isSaveable = false;
            bool isSearchable = false;
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        // PO No.
                        this.txtPONo.Enabled = false;
                        // 状態
                        isEditable = false;
                        isSaveable = false;
                        isSearchable = true;
                        this.EditMode = SystemBase.EditMode.None;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        isSearchable = false;

                        if (rdoView.Checked)
                        {
                            // 照会:開始押下後
                            // 状態
                            isEditable = false;
                            isSaveable = false;
                            this.EditMode = SystemBase.EditMode.View;
                        }
                        else if (rdoAllDelete.Checked)
                        {
                            // 全削除:開始押下後
                            isEditable = false;
                            isSaveable = true;
                            this.EditMode = SystemBase.EditMode.Delete;
                        }
                        else if (rdoEdit.Checked)
                        {
                            // 編集:開始押下後
                            isEditable = true;
                            isSaveable = true;
                            this.EditMode = SystemBase.EditMode.Update;
                        }
                        else
                        {
                            // 新規:開始押下後
                            isEditable = true;
                            isSaveable = true;
                            this.EditMode = SystemBase.EditMode.Insert;
                        }
                        break;
                    default:
                        break;
                }
                bool isViewMode = !isSearchable && !(isSaveable || isEditable);
                // ファンクションボタン
                this.fbrFunction.F01Button.Enabled = isSaveable;
                this.fbrFunction.F02Button.Enabled = isEditable;
                this.fbrFunction.F03Button.Enabled = isEditable;
                this.fbrFunction.F04Button.Enabled = isViewMode; //MAIL
                this.fbrFunction.F05Button.Enabled = isViewMode; //見積出力
                this.fbrFunction.F06Button.Enabled = !isSearchable; //Clear
                this.fbrFunction.F07Button.Enabled = true; //All clear
                this.fbrFunction.F08Button.Enabled = isViewMode; //Excel
                this.fbrFunction.F09Button.Enabled = false; //この状態は後程判断
                this.fbrFunction.F10Button.Enabled = false; //この状態は後程判断
                this.fbrFunction.F12Button.Enabled = true;
                // 検索条件
                this.grpSearch.Enabled = isSearchable;
                this.lblEstimateNo.Enabled = isSearchable;
                this.btnStart.Enabled = isSearchable;
                // 見積情報
                SetEstimateInfoEnabled(isEditable, isViewMode);
                // シート
                this.shtTehaiMitsumori.Enabled = !isSearchable;
                this.shtTehaiMitsumori.Columns[SHEET_COL_UNIT_PRICE].Enabled = isEditable;
                this.shtTehaiMitsumori.Columns[SHEET_COL_ESTIMATE_RIREKI].Enabled = isEditable;
                this.shtTehaiMitsumori.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ラジオボタン切り替え
        /// --------------------------------------------------
        /// <summary>
        /// ラジオボタン切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <create>S.Furugo 2018/11/30</create>
        /// <param name="e"></param>
        /// --------------------------------------------------
        private void rdoMode_Checked_Changed(object sender, EventArgs e)
        {
            if (!grpSearch.Enabled)
                return;
            if (rdoInsert.Checked)
            {
                txtEstimateNo.Enabled = false;
                txtEstimateNo.Text = string.Empty;
            }
            else
            {
                txtEstimateNo.Enabled = true;
            }
        }
        #endregion

        #region 手配見積レート設定の取得
        /// --------------------------------------------------
        /// <summary>
        /// 手配見積レート設定の取得
        /// </summary>
        /// <returns>汎用マスタ販管(%),運賃(%)を含む行</returns>
        /// --------------------------------------------------
        private DataRow GetCommonEstimateRate()
        {
            return (
                from row in this.GetCommon(ESTIMATE_RATE.GROUPCD).Tables[Def_M_COMMON.Name].AsEnumerable()
                where ComFunc.GetFld(row, Def_M_COMMON.ITEM_CD) == ESTIMATE_RATE.ONEROUS_NAME
                 select row
                ).FirstOrDefault();

        }
        #endregion

        #region 計算処理
        /// --------------------------------------------------
        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="isForce">強制計算有無</param>
        /// <create>S.Furugo 2018/12/3</create>
        /// <update>T.Zhou 2023/12/11 見積計算方法変更</update>
        /// <update>J.Chen 2023/12/18 仕切り金額追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool Calculate(bool isForce)
        {
            //計算ボタンが表示の場合は自動計算させない
            if (!isForce && btnCalculate.Visible) return false;
            if (!CheckInputInner(false)) return false;

            decimal RateJPY = txtRateJPY.Value;
            decimal SalsesPer = txtSalsesPer.Value;
            decimal ROBPer = txtRobPer.Value;
            decimal RatePartition = txtRatePartition.Value;
            decimal CurrentPartsCost = 0;
            decimal CurrentPOAmount = 0;

            // グリッド項目の再計算
            for (int i = 0; i < this.shtTehaiMitsumori.MaxRows; i++)
            {
                var UnitPrice = 0m;
                var Quantity = 0m;

                // 見積データ取得
                var tmDt = this.shtTehaiMitsumori.DataSource as DataTable;

                // 手配フラグ取得
                var tehaiFlag = ComFunc.GetFld(tmDt, i, Def_T_TEHAI_MEISAI.TEHAI_FLAG);

                // 発注数取得
                var hacchuQty = ComFunc.GetFldToDecimal(tmDt, i, Def_T_TEHAI_MEISAI.HACCHU_QTY); 

                if (!string.IsNullOrEmpty(this.shtTehaiMitsumori[SHEET_COL_UNIT_PRICE, i].Text))
                {
                    UnitPrice = (decimal)(double)this.shtTehaiMitsumori[SHEET_COL_UNIT_PRICE, i].Value;
                    // 手配区分SKS Skipの場合、発注数が0の時、出荷数で計算します。
                    if (tehaiFlag == TEHAI_FLAG.SKS_SKIP_VALUE1 && hacchuQty == 0)
                    {
                        Quantity = (decimal)(double)this.shtTehaiMitsumori[SHEET_COL_SHUKKA_QTY, i].Value;
                    }
                    else
                    {
                        Quantity = hacchuQty;
                    }
                }

                var UnitPriceSalse = Math.Ceiling(UnitPrice * (SalsesPer + 100) / 100);
                var UnitPriceRmb = Math.Ceiling(UnitPriceSalse / RateJPY);
                var SumRmb = Quantity * UnitPriceRmb; //合計RMB
                var RobUnitPriceRmb = Math.Ceiling(UnitPriceRmb * (ROBPer + 100) / 100);
                var RobSumRmb = Quantity * RobUnitPriceRmb;
                if (RobSumRmb > 999999999)
                {
                    // 桁あふれの際、データの更新ができないため、エラーにする。
                    // {0}行目で桁あふれが発生したため計算を中断しました。
                    ShowMessage("T0100040013", (i + 1).ToString());
                    return false;
                }

                this.shtTehaiMitsumori[SHEET_COL_UNIT_PRICE_SALSE, i].Value = UnitPriceSalse;
                this.shtTehaiMitsumori[SHEET_COL_UNIT_PRICE_RMB, i].Value = UnitPriceRmb;
                this.shtTehaiMitsumori[SHEET_COL_SUM_RMB, i].Value = SumRmb;
                this.shtTehaiMitsumori[SHEET_COL_ROB_UNIT_PRICE_RMB, i].Value = RobUnitPriceRmb;
                this.shtTehaiMitsumori[SHEET_COL_ROB_SUM_RMB, i].Value = RobSumRmb;
                // 運賃込単価RMBをINVOICE単価とする
                this.shtTehaiMitsumori[SHEET_COL_INVOICE_UNIT_PRICE, i].Value = RobUnitPriceRmb;

                CurrentPartsCost += SumRmb;
                CurrentPOAmount += RobSumRmb;
            }
            txtCurrentPartsCost.Value = CurrentPartsCost;
            txtCurrentPOAmount.Value = CurrentPOAmount;
            txtCurrentROB.Value = (CurrentPOAmount - CurrentPartsCost);
            var CurrentPartitionAmount = Math.Ceiling(CurrentPOAmount * RateJPY * RatePartition);

            if (CurrentPartitionAmount > 99999999999)
            {
                // 桁あふれの際、データの更新ができないため、エラーにする。
                // 仕切り金額において桁あふれが発生したため、計算を中断しました。
                ShowMessage("T0100040015", lblRatePartition.Text);
                return false;
            }

            txtCurrentPartitionAmount.Value = CurrentPartitionAmount;

            return true;
        }

        #endregion

        #region 登録データ取得
        
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細情報取得(全データ)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>手配明細テーブル</returns>
        /// <create>S.Furugo 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetAllDataTehaiMeisai(DataTable dtSrc, DataRowState state)
        {
            string name = string.Empty;

            switch (state)
            {
                case DataRowState.Added:
                    name = ComDefine.DTTBL_INSERT;
                    break;
                case DataRowState.Modified:
                    name = ComDefine.DTTBL_UPDATE;
                    break;
                case DataRowState.Deleted:
                    name = ComDefine.DTTBL_DELETE;
                    break;
                default:
                    break;
            }
            DataTable dt = dtSrc.Clone();
            dt.TableName = name;
            return dt;
        }
        /// --------------------------------------------------
        /// <summary>
        /// 手配明細情報取得(変更時)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>手配明細テーブル</returns>
        /// <create>S.Furugo 2018/12/4</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDataTehaiMeisai(DataTable dtSrc, DataRowState state)
        {
            DataTable dt = null;
            string name = string.Empty;

            switch (state)
            {
                case DataRowState.Added:
                    dt = dtSrc.GetChanges(DataRowState.Added);
                    if (dt == null) return null;
                    name = ComDefine.DTTBL_INSERT;
                    break;
                case DataRowState.Modified:
                    dt = dtSrc.GetChanges(DataRowState.Modified);
                    if (dt == null) return null;
                    name = ComDefine.DTTBL_UPDATE;
                    break;
                case DataRowState.Deleted:
                    DataView dv = dtSrc.Copy().DefaultView;
                    dv.RowStateFilter = DataViewRowState.Deleted;
                    if (dv == null || dv.Count < 1) return null;
                    dt = dtSrc.Clone();

                    for (int i = 0; i < dv.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            dr[j] = dv[i][j];
                        }
                        dt.Rows.Add(dr);
                    }
                    name = ComDefine.DTTBL_DELETE;
                    break;
                default:
                    dt = dtSrc.Clone();
                    break;
            }
            dt.TableName = name;

            return dt;
        }

        #endregion

        #region シート内データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 画面内の見積情報を取得する
        /// </summary>
        /// <returns>見積情報テーブル</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update>R.Sumi 2022/3/11</update>
        /// <update>J.Chen 2023/12/20 項目追加</update>
        /// --------------------------------------------------
        private DataTable GetTehaiEstimateFromDisplay()
        {
            var dt = GetSchemeTehaiEstimate();
            var dr = dt.NewRow();
            var ProjectedSalesMonth = (mpProjectedSalesMonth.CustomFormat == NULL_PROJECTED_SALES_MONTH) ? null : mpProjectedSalesMonth.Value.ToString("yyyy/MM/01");
            dr[Def_T_TEHAI_ESTIMATE.ESTIMATE_NO] = txtEstimateNo.Text;
            dr[Def_T_TEHAI_ESTIMATE.NAME] = txtEstimateName.Text;
            dr[Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG] = cboCurrency.SelectedValue;
            var row = cboCurrency.SelectedItem as DataRowView;
            dr[ComDefine.FLD_CURRENCY_FLAG_NAME] = (row == null) ? null : row.Row.Field<string>(Def_M_COMMON.ITEM_NAME);
            dr[Def_T_TEHAI_ESTIMATE.RATE_JPY] = txtRateJPY.Value;
            dr[Def_T_TEHAI_ESTIMATE.SALSES_PER] = txtSalsesPer.Value;
            dr[Def_T_TEHAI_ESTIMATE.ROB_PER] = txtRobPer.Value;
            dr[Def_T_TEHAI_ESTIMATE.VERSION] = txtEstimateVersion.Text;
            dr[Def_T_TEHAI_ESTIMATE.PO_NO] = string.IsNullOrEmpty(txtPONo.Text) ? null : txtPONo.Text;
            //dr[Def_T_TEHAI_ESTIMATE.TAG_NO] = txtTagNo.Text;
            dr[Def_T_TEHAI_ESTIMATE.PO_AMOUNT] = string.IsNullOrEmpty(txtCurrentPOAmount.Text) ? null : txtCurrentPOAmount.Text;
            dr[Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT] = string.IsNullOrEmpty(txtCurrentPartitionAmount.Text) ? null : txtCurrentPartitionAmount.Text;
            dr[Def_T_TEHAI_ESTIMATE.RATE_PARTITION] = string.IsNullOrEmpty(txtRatePartition.Text) ? null : txtRatePartition.Text;
            dr[Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH] = string.IsNullOrEmpty(ProjectedSalesMonth) ? null : ProjectedSalesMonth;
            dr[Def_T_TEHAI_ESTIMATE.MAIL_TITLE] = cboMailTitle.SelectedValue;
            dr[Def_T_TEHAI_ESTIMATE.REV] = string.IsNullOrEmpty(txtRev.Text) ? null : txtRev.Text;
            dr[Def_T_TEHAI_ESTIMATE.CONSIGN_CD] = cboNiukesaki.SelectedValue;

            dt.Rows.Add(dr);
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面内の合計情報を取得する
        /// </summary>
        /// <returns>合計情報テーブル</returns>
        /// <create>D.Okumura 2018/12/20</create>
        /// <update>J.Chen 2023/12/20 仕切り金額追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetTehaiEstimateSumFromDisplay()
        {
            var dt = GetSchemeTehaiEstimateSum();
            var dr = dt.NewRow();
            dr[ComDefine.FLD_SUM_RMB] = txtCurrentPartsCost.Value;
            dr[ComDefine.FLD_ROB_SUM_RMB] = txtCurrentPOAmount.Value;
            dr[ComDefine.FLD_SUM_ROB] = txtCurrentROB.Value;
            dr[ComDefine.FLD_PAMOUNT_SUM_RMB] = txtCurrentPartitionAmount.Value;
            dt.Rows.Add(dr);
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面内の見積書情報を取得する
        /// </summary>
        /// <returns>見積書情報テーブル</returns>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetTehaiEstimateForQuotationFromDisplay()
        {
            var dt = GetSchemeTehaiEstimateForQuotation();
            var dr = dt.NewRow();
            dr[Def_T_TEHAI_ESTIMATE.ESTIMATE_NO] = txtEstimateNo.Text;
            dr[ComDefine.FLD_CONSIGN_NAME] = cboNiukesaki.Text;
            dr[Def_T_TEHAI_ESTIMATE.NAME] = txtEstimateName.Text;
            dr[Def_T_TEHAI_ESTIMATE.RATE_JPY] = txtRateJPY.Value;
            dr[ComDefine.FLD_CURRENCY_FLAG_NAME] = cboCurrency.Text;
            dr[ComDefine.FLD_ISSUE_DATE] = DateTime.Now.ToString("yyyy/M/d");
            dr[ComDefine.FLD_ISSUE_YEARMONTH] = DateTime.Now.ToString("yyyy.M");
            dr[ComDefine.FLD_SUM_RMB] = txtCurrentPartsCost.Value;
            dr[ComDefine.FLD_SUM_ROB] = txtCurrentROB.Value;
            dr[ComDefine.FLD_ROB_SUM_RMB] = txtCurrentPOAmount.Value;
            dr[ComDefine.FLD_SHIPPING_LOCATION] = txtShippingLocation.Text;

            dt.Rows.Add(dr);
            return dt;
        }
        #endregion

        #region 手配見積明細情報用のフィールド
        /// --------------------------------------------------
        /// <summary>
        /// データのフィールド取得
        /// </summary>
        /// <returns></returns>
        /// <create>S.Furugo 2018/12/07</create>
        /// <update>J.Chen 2023/12/20 項目追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTehaiEstimate()
        {
            DataTable dt = new DataTable(Def_T_TEHAI_ESTIMATE.Name);

            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.NAME, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.PO_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.RATE_JPY, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.ROB_PER, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.SALSES_PER, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG, typeof(string));
            dt.Columns.Add(ComDefine.FLD_CURRENCY_FLAG_NAME, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.VERSION, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.PO_AMOUNT, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.RATE_PARTITION, typeof(decimal));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.MAIL_TITLE, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.REV, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.CONSIGN_CD, typeof(string));

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データのフィールド取得
        /// </summary>
        /// <returns></returns>
        /// <create>S.Furugo 2018/12/07</create>
        /// <update>J.Chen 2023/12/20 仕切り金額追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTehaiEstimateSum()
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_TEHAI_ESTIMATE_SUM);
            dt.Columns.Add(ComDefine.FLD_SUM_RMB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_ROB_SUM_RMB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_SUM_ROB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_PAMOUNT_SUM_RMB, typeof(decimal));

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// データのフィールド取得
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTehaiEstimateForQuotation()
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_QUOTATION_TABLE);

            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, typeof(string));
            dt.Columns.Add(ComDefine.FLD_CONSIGN_NAME, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.NAME, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_ESTIMATE.RATE_JPY, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_CURRENCY_FLAG_NAME, typeof(string));
            dt.Columns.Add(ComDefine.FLD_ISSUE_DATE, typeof(string));
            dt.Columns.Add(ComDefine.FLD_ISSUE_YEARMONTH, typeof(string));
            dt.Columns.Add(ComDefine.FLD_SUM_RMB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_SUM_ROB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_ROB_SUM_RMB, typeof(decimal));
            dt.Columns.Add(ComDefine.FLD_SHIPPING_LOCATION, typeof(string));

            return dt;
        }
        #endregion

        #region 単価が変更時の処理
        /// --------------------------------------------------
        /// <summary>
        /// セル入力内容変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtTehaiMitsumori_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                if (e.Position.Column == 6) //単価
                    Calculate(false);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        #endregion

        #region 通貨が変更時の処理
        /// --------------------------------------------------
        /// <summary>
        /// 通貨変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 出荷国初期化
            var currencyRow = cboCurrency.SelectedItem as DataRowView;
            txtShippingLocation.Text = (currencyRow == null) ? null : currencyRow.Row.Field<string>(Def_M_COMMON.VALUE2);
        }
        #endregion

        #region 売上予定月NULL対応

        #region 売上予定月が変更されたとき
        /// <summary>
        /// 売上予定月が変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/12/19</create>
        /// <update></update>
        private void mpProjectedSalesMonth_ValueChanged(object sender, EventArgs e)
        {
            if (mpProjectedSalesMonth.Text == NULL_PROJECTED_SALES_MONTH)
            {
                mpProjectedSalesMonth.CustomFormat = "yyyy/MM";
            }
        }
        #endregion

        #region 売上予定月にキーが押されたとき
        /// <summary>
        /// 売上予定月にキーが押されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/12/19</create>
        /// <update></update>
        private void mpProjectedSalesMonth_KeyDown(object sender, KeyEventArgs e)
        {
            // DeleteキーまたはBackキーが押された場合
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                mpProjectedSalesMonth.Value = DateTime.Now;
                mpProjectedSalesMonth.CustomFormat = NULL_PROJECTED_SALES_MONTH;
            }
        }
        #endregion

        #region 売上予定月に数字が入力されたとき
        /// <summary>
        /// 売上予定月に数字が入力されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/12/19</create>
        /// <update></update>
        private void mpProjectedSalesMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && mpProjectedSalesMonth.Text == NULL_PROJECTED_SALES_MONTH)
            {
                mpProjectedSalesMonth.CustomFormat = "yyyy/MM";
                this.mpProjectedSalesMonth.ShowUpDown = true; // 入力モードにする
                this.mpProjectedSalesMonth.ShowUpDown = false; // 入力モードにする
            }
        }
        #endregion

        #endregion

        #region コントロールを無効
        /// --------------------------------------------------
        /// <summary>
        /// コントロールを無効
        /// </summary>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetControlsDisable()
        {
            // 手配情報登録から起動時
            if (isFromTehaiMeisai)
            {
                this.rdoInsert.Enabled = false;
                this.rdoAllDelete.Enabled = false;
                this.txtEstimateNo.Enabled = false;
                this.fbrFunction.F07Button.Enabled = false;
            }
        }
        #endregion

        #region 画面閉じる時
        /// --------------------------------------------------
        /// <summary>
        /// 画面閉じる時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void TehaiMitsumori_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMailRequired)
            {
                // 受注金額が変更されました。見積差し替えのメールを送信してください。
                this.ShowMessage("T0100040016");
                e.Cancel = true;
            }
        }

        #endregion

        #region 見積情報有効無効設定
        /// --------------------------------------------------
        /// <summary>
        /// 見積情報有効無効設定
        /// </summary>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEstimateInfoEnabled(bool isEditable, bool isViewMode)
        {
            // 見積情報
            this.lblEstimateName.Enabled = isEditable;
            this.lblShippingLocation.Enabled = isEditable;
            this.lblCurrency.Enabled = isEditable;
            this.lblRateJPY.Enabled = isEditable;
            this.lblSalsesPer.Enabled = isEditable;
            this.lblRobPer.Enabled = isEditable;
            this.lblRatePartition.Enabled = isEditable;
            this.btnCalculate.Enabled = isEditable;

            this.lblProjectedSalesMonth.Enabled = isViewMode ? isViewMode : isEditable;
            this.lblMailTitle.Enabled = isViewMode ? isViewMode : isEditable;
            this.lblRev.Enabled = isViewMode ? isViewMode : isEditable;
            this.lblNiukesaki.Enabled = isViewMode ? isViewMode : isEditable;
        }
        #endregion

        #region メール送信実行

        #region 制御メソッド(見積書作成＋メール送信)

        /// --------------------------------------------------
        /// <summary>
        /// メール送信実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunMail()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                // 入力チェック
                if (!this.CheckInputMail())
                {
                    // 入力チェックNG
                    return false;
                }

                // 確認メッセージ
                // 画面の内容でメール送信します。\r\nよろしいですか？
                if (!this.ShowMessage("S0100050020").Equals(DialogResult.OK))
                {
                    return false;
                }

                // メール送信実行
                Cursor.Current = Cursors.WaitCursor;

                string fileName = string.Format(ComDefine.EXCEL_FILE_QUOTATION
                        , this.txtEstimateNo.Text
                        , this.txtEstimateName.Text
                        , this.txtRev.Text
                        );

                if (this.ExportExcelAndPdf(true, ComDefine.QUOTATION_OUTPUT_DIR, fileName))
                {
                    // メール送信しました。
                    this.ShowMessage("S0100050026");
                }
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
            return true;
        }

        #endregion

        #region 制御メソッド(見積書作成のみ)

        /// --------------------------------------------------
        /// <summary>
        /// 見積書作成のみ実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update>J.Jeong 2024/07/09 出力ファイル名変更</update>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunExcelAndPdfOnly()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.TehaiMitsumori_sfdFilesForQuotation_Title;
                    frm.Filter = Resources.TehaiMitsumori_sfdExcelAndAll_Filter;
                    frm.InitialDirectory = Resources.TehaiMitsumori_sfdExcel_InitialDirectory;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_QUOTATION
                        , this.txtEstimateNo.Text
                        , this.txtEstimateName.Text
                        , this.txtRev.Text
                        );
                    if (frm.ShowDialog() != DialogResult.OK)
                        return false;

                    // Export Excel&PDF file
                    Cursor.Current = Cursors.WaitCursor;
                    var path = Path.GetDirectoryName(frm.FileName);
                    var fileName = Path.GetFileName(frm.FileName);
                    if (this.ExportExcelAndPdf(false, path, fileName))
                    {
                        // 見積書Excel及びPDFファイルを出力しました。
                        this.ShowMessage("T0100040018");
                    }
                }
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
            return true;
        }

        #endregion

        #region メール送信用入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// メール送信用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputMail()
        {
            bool ret = false;
            try
            {
                // 売上予定月
                if (string.IsNullOrEmpty(mpProjectedSalesMonth.Text) || mpProjectedSalesMonth.Text == NULL_PROJECTED_SALES_MONTH)
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblProjectedSalesMonth.Text);
                    this.mpProjectedSalesMonth.Focus();
                    return false;
                }

                // Mail Title
                if (string.IsNullOrEmpty(cboMailTitle.Text))
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblMailTitle.Text);
                    this.cboMailTitle.Focus();
                    return false;
                }

                // Rev
                if (string.IsNullOrEmpty(txtRev.Text))
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblRev.Text);
                    this.txtRev.Focus();
                    return false;
                }

                // 荷受先
                if (string.IsNullOrEmpty(cboNiukesaki.Text))
                {
                    // {0}を入力して下さい。
                    this.ShowMessage("T0100040001", lblNiukesaki.Text);
                    this.cboNiukesaki.Focus();
                    return false;
                }

                // メールアドレスチェック
                var conn = new ConnCommon();
                var cond = new CondCommon(this.UserInfo);
                cond.ConsignCD = this.cboNiukesaki.SelectedValue.ToString();
                var ds = conn.CheckPlanningMail(cond);
                if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                {
                    // 担当者にMailAddressが設定されていません。
                    this.ShowMessage("A0100010010");
                    return false;
                }

                if (!UtilData.ExistsData(ds, Def_M_CONSIGN_MAIL.Name))
                {
                    // 該当荷受先のメール送信対象者が設定されていません。
                    this.ShowMessage("T0100040020");
                    return false;
                }

                this._dtQuotationUser = ds.Tables[Def_M_CONSIGN_MAIL.Name];
                this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
                ret = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region ExcelとPDF出力

        /// --------------------------------------------------
        /// <summary>
        /// ExcelとPDF出力
        /// </summary>
        /// <param name="isMail">true:Excel+Mail, false:Excel only</param>
        /// <param name="path">保存フォルダ</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update>J.Jeong 2024/07/09 出力ファイル名変更</update>
        /// <update>J.Chen 2024/08/08 メール用累計金額追加</update>
        /// --------------------------------------------------
        private bool ExportExcelAndPdf(bool isMail, string path, string fileName)
        {
            bool ret = false;
            string mailID = string.Empty;
            string pdfFileName = string.Empty;
            string excelFileName = string.Empty;
            string compressedFileName = string.Empty;
            string estimateName = this.txtEstimateName.Text;

            //ファイル名に使用できない文字を取得
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

            if (fileName.IndexOfAny(invalidChars) >= 0)
            {
                foreach (char invalidChar in invalidChars)
                {
                    fileName = fileName.Replace(invalidChar.ToString(), "");
                    estimateName = estimateName.Replace(invalidChar.ToString(), "");
                }
                // ファイル名に利用できない文字が含まれています。\r\n続行する場合、無効な文字は空白文字に置き換えられます。\r\n続行しますか？
                DialogResult retSelect = this.ShowMessage("T0100040022");
                if (retSelect == DialogResult.No)
                {
                    return false;
                }
            }

            var filePath = Path.Combine(path, fileName);

            try
            {
                // Excelファイルダウンロード
                if (!this.ExcelTemplateFileDownload())
                {
                    // テンプレートファイルのダウンロードに失敗しました。
                    this.ShowMessage("S0100050024");
                    return false;
                }

                string errMsgID;
                string[] args;

                // ファイル出力するデータを準備
                DataSet ds = new DataSet();
                // 見積書情報取得
                ds.Tables.Add(this.GetTehaiEstimateForQuotationFromDisplay());

                if (!UtilData.ExistsData(ds, ComDefine.DTTBL_QUOTATION_TABLE))
                {
                    // 出力するDataがありません。
                    this.ShowMessage("A0100010004");
                    return false;
                }

                // MAIL情報取得
                var dtQutationMail = this.GetTehaiEstimateFromDisplay();

                // Excel出力
                var export = new ExportQuotation();
                var retExcel = export.ExportExcel(filePath, ds);
                if (!retExcel)
                {
                    // Excel出力に失敗しました。
                    this.ShowMessage("A7777777001");
                    return false;
                }

                // PDF変換
                var retPdf = export.ConvertExcelToPdf(filePath);
                if (!retPdf)
                {
                    // PDFの出力に失敗しました。Excelがインストールされていることを確認してください。
                    this.ShowMessage("T0100040019");
                    return false;
                }

                // Excel出力のみであれば処理を抜ける
                if (!isMail) return true;

                // 有償支給部品Excel出力
                excelFileName = string.Format(ComDefine.EXCEL_FILE_ESTIMATE_PARTS
                        , this.txtEstimateNo.Text
                        , estimateName
                        , this.txtRev.Text
                        );
                string excelFilePath = Path.Combine(path, excelFileName);

                if (!this.ExportExcel(excelFilePath))
                {
                    // Excel出力に失敗しました。
                    this.ShowMessage("A7777777001");
                    return false;
                }

                var conn = new ConnS01();
                var cond = new CondS01(this.UserInfo);
                cond.UpdateUserID = this.UserInfo.UserID;
                cond.UpdateUserName = this.UserInfo.UserName;

                // MAIL_ID採番
                mailID = conn.GetMailID(cond, out errMsgID, out args);
                if (string.IsNullOrEmpty(mailID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                // PDFファイルパスへ変換
                string pdfFilePath = Path.ChangeExtension(filePath, "pdf");
                pdfFileName = Path.ChangeExtension(fileName, "pdf");

                //// ファイルアップロード(PDF)
                //if (!this.QuotationFileUpload(pdfFilePath, pdfFileName, mailID))
                //{
                //    // アップロードに失敗しました。
                //    this.ShowMessage("S0100050025");
                //    return false;
                //}

                //// ファイルアップロード(Excel)
                //if (!this.QuotationFileUpload(excelFilePath, excelFileName, mailID))
                //{
                //    // アップロードに失敗しました。
                //    this.ShowMessage("S0100050025");
                //    return false;
                //}

                // 添付するファイル
                List<string> attachments = new List<string>();
                attachments.Add(excelFilePath);
                attachments.Add(pdfFilePath);


                // zipファイル名
                compressedFileName = string.Format(ComDefine.EXCEL_FILE_QUOTATION_FOR_ZIP
                        , this.txtEstimateNo.Text
                        , estimateName
                        , DateTime.Now.ToString("yyyyMMdd")
                        );
                string compressedFilePath = Path.Combine(path, compressedFileName);

                // ファイルの圧縮
                if (!CompressFilesAsAttachments(attachments, compressedFilePath))
                {
                    // アップロードに失敗しました。
                    this.ShowMessage("S0100050025");
                    return false;
                }

                // ファイルアップロード(ZIP)
                if (!this.QuotationFileUpload(compressedFilePath, compressedFileName, mailID))
                {
                    // アップロードに失敗しました。
                    this.ShowMessage("S0100050025");
                    return false;
                }

                // 累計金額計算
                CalculateTotalAmountForMail();

                // メールデータ作成
                var dsMail = new DataSet();
                dsMail.Merge(this.CreateMailData(mailID, compressedFileName));
                dsMail.Merge(dtQutationMail);

                var connT01 = new ConnT01();
                var condT01 = new CondT01(this.UserInfo);
                condT01.UpdateUserID = this.UserInfo.UserID;
                condT01.UpdateUserName = this.UserInfo.UserName;

                // DB更新
                if (!connT01.UpdTehaiEstimateMail(condT01, dsMail, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (!ret)
                {
                    this.QuotationFileDelete(pdfFileName, mailID);
                    this.QuotationFileDelete(excelFileName, mailID);
                    this.QuotationFileDelete(compressedFileName, mailID);
                }
            }
            return ret;
        }

        #endregion

        #region ファイルダウンロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExcelTemplateFileDownload()
        {
            bool ret = false;
            try
            {
                var connFile = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = ComDefine.EXCEL_FILE_QUOTATION_TEMPLATE;
                package.FileType = FileType.Template;

                // TemplateファイルのDL
                var retFile = connFile.FileDownload(package);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    var path = Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_QUOTATION_TEMPLATE);
                    File.WriteAllBytes(path, retFile.FileData);
                    ret = true;
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイルアップロード

        /// --------------------------------------------------
        /// <summary>
        /// ファイルアップロード
        /// </summary>
        /// <param name="filePath">アップロード元ファイル</param>
        /// <param name="fileName">アップロード先ファイル名</param>
        /// <param name="mailID">メールID</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool QuotationFileUpload(string filePath, string fileName, string mailID)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var conn = new ConnAttachFile();
                    var updPackage = new FileUploadPackage();

                    var data = new byte[fs.Length];
                    fs.Position = 0;
                    fs.Read(data, 0, (int)fs.Length);
                    updPackage.FileData = data;
                    updPackage.FileName = fileName;
                    updPackage.FileType = FileType.Attachments;
                    updPackage.FolderName = mailID;
                    updPackage.GirenType = GirenType.None;

                    var updResult = conn.FileUpload(updPackage);
                    if (!updResult.IsSuccess)
                    {
                        // 失敗
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル削除

        /// --------------------------------------------------
        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="fileName">アップロード先ファイル名</param>
        /// <param name="mailID">メールID</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool QuotationFileDelete(string fileName, string mailID)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(mailID))
            {
                return true;
            }

            try
            {
                var conn = new ConnAttachFile();
                var delPackage = new FileDeletePackage();

                delPackage.FileName = fileName;
                delPackage.FileType = FileType.Attachments;
                delPackage.FolderName = mailID;
                delPackage.GirenType = GirenType.None;

                var result = conn.FileDelete(delPackage);
                if (!result.IsSuccess)
                {
                    // 失敗
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region メール登録用データ作成

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable CreateMailData(string mailID, string fileName)
        {
            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(MAIL_FILE.QUOTATION_TITLE_VALUE1);
            string naiyo = attachFile.GetMailTemplate(MAIL_FILE.QUOTATION_VALUE1);

            var dt = ComFunc.GetSchemeMail();
            var dr = dt.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_ID, mailID);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, ComFunc.GetMailUser(this._dtQuotationUser, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.TITLE, this.ReplaceMailContents(title));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.ReplaceMailContents(naiyo));
            dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
            dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
            dr.SetField<object>(Def_T_MAIL.APPENDIX_FILE_PATH, Path.Combine(mailID, fileName));
            dt.Rows.Add(dr);

            return dt;
        }

        #endregion

        #region テンプレートのデータを置換

        /// --------------------------------------------------
        /// <summary>
        /// テンプレートのデータを置換
        /// </summary>
        /// <param name="mailContents"></param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update>J.jeong 2024/07/22 通貨単位の値追加</update>
        /// <update>J.Chen 2024/08/08 メール用累計金額追加</update>
        /// --------------------------------------------------
        private string ReplaceMailContents(string mailContents)
        {
            return mailContents
                .Replace(MAIL_RESERVE.ESTIMATE_NAME_VALUE1, this.txtEstimateName.Text)
                .Replace(MAIL_RESERVE.REVISION_VALUE1, this.txtRev.Text)
                .Replace(MAIL_RESERVE.MAIL_TITLE_VALUE1, this.cboMailTitle.Text)
                .Replace(MAIL_RESERVE.PO_AMOUNT_VALUE1, this.txtCurrentPOAmount.Text)
                .Replace(MAIL_RESERVE.CURRENCY_UNIT_VALUE1, this.cboCurrency.Text)
                .Replace(MAIL_RESERVE.PARTITION_AMOUNT_VALUE1, this.txtCurrentPartitionAmount.Text)
                .Replace(MAIL_RESERVE.PROJECTED_SALES_MONTH_VALUE1, this.mpProjectedSalesMonth.Text)
                .Replace(MAIL_RESERVE.SENDER_VALUE1, this.UserInfo.UserName)
                .Replace(MAIL_RESERVE.TOTAL_PO_AMOUNT_VALUE1, this._totalPOAmount.ToString("N0"))
                .Replace(MAIL_RESERVE.TOTAL_PARTITION_AMOUNT_VALUE1, this._totalPartitionAmount.ToString("N0"));
        }

        #endregion

        #endregion

        #region 有償支給部品Excelの出力

        #region 制御メソッド(有償支給部品Excel作成)

        /// --------------------------------------------------
        /// <summary>
        /// 有償支給部品Excel作成実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/01/24</create>
        /// <update>J.Jeong 2024/07/09 出力ファイル名変更</update>
        /// --------------------------------------------------
        private bool RunExcelOnly()
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.TehaiMitsumori_sfdExcel_Title;
                    frm.Filter = Resources.TehaiMitsumori_sfdExcel_Filter;
                    frm.InitialDirectory = Resources.TehaiMitsumori_sfdExcel_InitialDirectory;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_ESTIMATE_PARTS
                        , this.txtEstimateNo.Text
                        , this.txtEstimateName.Text
                        , this.txtRev.Text
                        );
                    if (frm.ShowDialog() != DialogResult.OK)
                        return false;

                    // Export Excel file
                    Cursor.Current = Cursors.WaitCursor;
                    if (this.ExportExcel(frm.FileName))
                    {
                        // 有償支給部品Excelファイルを出力しました。
                        this.ShowMessage("E0100120001");
                    }
                }
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
            return true;
        }

        #endregion

        #region Excel出力

        /// --------------------------------------------------
        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="isMail">true:Excel+Mail, false:Excel only</param>
        /// <param name="path">保存フォルダ</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExportExcel(string fileName)
        {
            bool ret = false;
            try
            {
                // ファイル出力するデータを準備
                DataSet ds = new DataSet();
                // 見積情報
                ds.Tables.Add(this.GetTehaiEstimateFromDisplay());
                //　手配明細一覧(計算済み)
                DataTable dtCal = this.shtTehaiMitsumori.DataSource as DataTable;
                dtCal = dtCal.Copy();
                dtCal.AcceptChanges();
                ds.Tables.Add(dtCal);
                // 集計
                ds.Tables.Add(GetTehaiEstimateSumFromDisplay());

                // テンプレートファイルをダウンロード
                var connFile = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();
                package.FileName = ComDefine.EXCEL_FILE_PARTS_LIST_TEMPLATE;
                package.FileType = FileType.Template;
                // TemplateファイルのDL
                var retFile = connFile.FileDownload(package);
                if (retFile.IsExistsFile)
                {
                    if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                    {
                        Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                    }
                    var path = Path.Combine(ComDefine.DOWNLOAD_DIR, ComDefine.EXCEL_FILE_TEMP_ESTIMATE_PARTS);
                    File.WriteAllBytes(path, retFile.FileData);
                }
                else
                {
                    // TemplateのDownloadに失敗しました。
                    this.ShowMessage("A7777777003");
                    return false;
                }

                // Excelファイルを出力
                var export = new ExportEstimatePartsList();
                var retExcel = export.ExportExcel(fileName, ds);
                if (!retExcel)
                {
                    // Excel出力に失敗しました。
                    this.ShowMessage("A7777777001");
                    return false;
                }

                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return ret;
        }

        #endregion

        #endregion

        #region ファイル圧縮

        /// --------------------------------------------------
        /// <summary>
        /// ファイル圧縮
        /// </summary>
        /// <param name="sourceFilePaths">対象ファイルパス</param>
        /// <param name="compressedFilePath">保存パス</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/01/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CompressFilesAsAttachments(List<string> sourceFilePaths, string compressedFilePath)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    // 古い形式が有効のため、ProvisionalAlternateEncodingを使用します
                    zip.ProvisionalAlternateEncoding = System.Text.Encoding.GetEncoding("shift_jis");
                    foreach (string sourceFilePath in sourceFilePaths)
                    {
                        zip.AddFile(sourceFilePath, "");
                    }

                    // 圧縮ファイルを保存
                    zip.Save(compressedFilePath);

                    // 成功時にtrueを返す
                    return true;
                }
            }
            catch (Exception ex)
            {
                // 失敗時にfalseを返す
                return false; 
            }
        }

        #endregion

        #region メール送信用入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// メール送信用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>J.Chen 2024/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CalculateTotalAmountForMail()
        {
            CondT01 cond = new CondT01(this.UserInfo);
            ConnT01 conn = new ConnT01();
            // 見積番号
            cond.MitsumoriNo = txtEstimateNo.Text;
            cond.ProjectNo = this._projectNo;

            DataSet ds = conn.GetTotalAmountForMail(cond);

            // 合計金額の計算
            this._totalPOAmount = 0;
            this._totalPartitionAmount = 0;

            if (ComFunc.IsExistsData(ds, Def_T_TEHAI_ESTIMATE.Name))
            {
                // その他の見積済み累計金額計算
                foreach (DataRow row in ds.Tables[Def_T_TEHAI_ESTIMATE.Name].Rows)
                {
                    this._totalPOAmount += Convert.ToDecimal(row[Def_T_TEHAI_ESTIMATE.PO_AMOUNT]);
                    this._totalPartitionAmount += Convert.ToDecimal(row[Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT]);
                }
            }

            // 今回の金額を追加
            this._totalPOAmount += Convert.ToDecimal(this.txtCurrentPOAmount.Text);
            this._totalPartitionAmount += Convert.ToDecimal(this.txtCurrentPartitionAmount.Text);
        }

        #endregion

    }
}
