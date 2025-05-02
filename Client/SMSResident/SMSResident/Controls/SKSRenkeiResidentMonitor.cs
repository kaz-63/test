using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WsConnection;
using WsConnection.WebRefJ01;
using Commons;
using DSWUtil;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SMSResident.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// SKS連携用コントロール
    /// </summary>
    /// <create>H.Tajimi 2018/11/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class SKSRenkeiResidentMonitor : SMSResident.Controls.BaseResidentMonitor
    {
        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 1度にアップロードするレコード数
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
            private const int UPLOAD_RECORD_COUNT_PER_TIME = 10000;
        #endregion

        #region フィールド

        private DateTime _jikaiSKSRenkeiDate = new DateTime();
        private DataTable _dtTehaiSKSWork = null;
        private string _sksFilePath = string.Empty;
        private string _logPutPath = string.Empty;

        #endregion

        #region メッセージ定数

        private const string MSG_J01_PROCESSED = "既にSKS連携処理が行われています。";
        private const string MSG_J01_NOTFOUND_M_SKS = "SKS連携マスタが存在しません。";
        private const string MSG_J01_COM_START = " 開始";
        private const string MSG_J01_COM_END = " 完了";
        private const string MSG_J01_COM_ERROR = " 異常終了";
        private const string MSG_J01_COM_ERROR_READ_CSV = " 異常終了(CSV読込失敗)";
        private const string MSG_J01_COM_READ = " 読み込み件数 {0}件";
        private const string MSG_J01_COM_UPLOAD = " アップロード {0}件";
        private const string MSG_J01_COM_COUNT = " 影響を受けたレコード数 {0}件";

        private const string MSG_J01_ERROR_REQUIRED_INPUT = "{0}行目の{1}が入力されていません。";

        private const string MSG_J01_SKSRENKEI_NAME = "SKS連携処理";
        private const string MSG_J01_SKSRENKEI_REFRESH_TEHAI_SKS_WORK_NAME = "SKS手配明細WORKの洗い替え処理";
        private const string MSG_J01_SKSRENKEI_READ_SKS_CSV_NAME = "SKS手配明細CSV 読取処理";
        private const string MSG_J01_SKSRENKEI_DELETE_TEHAI_SKS_WORK_NAME = "SKS手配明細WORK 削除処理(全レコード削除)";
        private const string MSG_J01_SKSRENKEI_INSERT_TEHAI_SKS_WORK_NAME = "SKS手配明細WORK 登録処理";
        private const string MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_WORK_HACCHU_ZYOTAI_NAME = "SKS手配明細WORK 更新処理(回答納期=「11111111」=完納)";
        private const string MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_WORK_UNIT_PRICE_NAME = "SKS手配明細WORK 更新処理(見積=単価0円)";
        private const string MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_NAME = "SKS手配明細更新処理";
        private const string MSG_J01_SKSRENKEI_INSERT_TEHAI_SKS_NAME = "SKS手配明細登録処理";
        private const string MSG_J01_SKSRENKEI_UPDATE_TEHAI_MEISAI_NAME = "手配明細更新処理";

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// 前回締め処理時間設定用デリゲート
        /// </summary>
        /// <param name="date"></param>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void SetShimeDateTextDelegate(DateTime date);

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public SKSRenkeiResidentMonitor()
            : base()
        {
            InitializeComponent();
        }
        
        #endregion

        #region 締め処理時間設定

        /// --------------------------------------------------
        /// <summary>
        /// 締め処理時間設定
        /// </summary>
        /// <param name="date">前回処理時間</param>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetShimeDateText(DateTime date)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetShimeDateTextDelegate(this.SetShimeDateText), new Object[] { date });
                return;
            }
            if (this.txtSKSRenkeiDate == null)
            {
                return;
            }
            this.txtSKSRenkeiDate.Text = date.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeControl()
        {
            try
            {
                try
                {
                    // 初期化処理
                    // フラグを未処理に更新
                    var cond = new CondJ01();
                    var conn = new ConnJ01();
                    cond.ShoriFlag = SHORI_FLAG.MISHORI_VALUE1;
                    conn.UpdSKS(cond);
                }
                catch (Exception ex)
                {
                    this.WriteLog(ex);
                }

                var tmpCond = new CondJ01();
                var tmpConn = new ConnJ01();
                DataSet ds = tmpConn.GetInit();
                this._dtTehaiSKSWork = ds.Tables[Def_T_TEHAI_SKS_WORK.Name];
                var dtSKS = this.GetSKSData(ds);
                this._sksFilePath = Path.Combine(ComFunc.GetFld(dtSKS, 0, Def_M_SKS.SKS_FOLDER), ComFunc.GetFld(dtSKS, 0, Def_M_SKS.SKS_FILE_NAME));
                this._logPutPath = ComFunc.GetFld(dtSKS, 0, Def_M_SKS.SKS_LOG_FOLDER);
                DateTime sksRenkeiDate = ComFunc.GetFldToDateTime(dtSKS, 0, Def_M_SKS.LASTEST_DATE, DateTime.Now);
                this.SetShimeDateText(sksRenkeiDate);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        #endregion

        #region 常駐処理関係

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理呼出しスレッド
        /// </summary>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void AsynchronousMonitor()
        {
            try
            {
                // 常駐処理を始める前の初期処理をここに記述します。
                // InitializeControlがOnLoadで呼ばれるためタブが表示されていないと実行されないので処理に必要な分は此処で行う。
                DataTable dt = this.GetSKSData();
                this.SetSKSDateTime(dt, ref this._jikaiSKSRenkeiDate);
                DateTime sksRenkeiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SKS.LASTEST_DATE, DateTime.Now);
                this.SetShimeDateText(sksRenkeiDate);

                // 初期処理で常駐処理を抜ける場合は下記の様にして下さい。
                //this.IsStopRequest = true;
                //base.AsynchronousMonitor();
                //return;

                // 常駐処理開始
                base.AsynchronousMonitor();
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理
        /// </summary>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void MonitorExecute()
        {
            bool isStart = false;
            try
            {
                // 日時処理開始チェック
                if (DateTime.Now < this._jikaiSKSRenkeiDate) return;
                ConnJ01 conn = new ConnJ01();
                DataSet ds;
                bool isProcessed;
                if (!conn.StartSKS(out ds, out isProcessed))
                {
                    if (isProcessed)
                    {
                        this.MessageAdd(MSG_J01_PROCESSED);

                        DateTime zenkaiSKSRenkeiDate = ComFunc.GetFldToDateTime(ds, Def_M_SKS.Name, 0, Def_M_SKS.LASTEST_DATE);
                        // 他端末でSKS連携が行われていたら次回SKS連携処理日時を更新
                        if (this._jikaiSKSRenkeiDate.Date <= zenkaiSKSRenkeiDate.Date)
                        {
                            this.SetSKSDateTime(ds.Tables[Def_M_SKS.Name], ref this._jikaiSKSRenkeiDate);
                            return;
                        }
                    }
                    else
                    {
                        this.MessageAdd(MSG_J01_NOTFOUND_M_SKS);
                    }
                    return;
                }
                CondJ01 cond = new CondJ01();
                cond.ShoriFlag = SHORI_FLAG.MISHORI_VALUE1;
                isStart = true;
                DataTable dt = new DataTable();
                if (ComFunc.IsExistsTable(ds, Def_M_SKS.Name))
                {
                    dt = ds.Tables[Def_M_SKS.Name];
                }

                cond.LastestDate = DateTime.Now;
                this.RunSKSRenkei();

                // SKS連携マスタ更新
                conn.UpdSKS(cond);
                isStart = false;

                // 次回SKS連携処理日時更新
                DataSet dsNext = conn.GetInit();
                DataTable dtNext = new DataTable();
                if (ComFunc.IsExistsTable(dsNext, Def_M_SKS.Name))
                {
                    dtNext = dsNext.Tables[Def_M_SKS.Name];
                }
                this.SetSKSDateTime(dtNext, ref this._jikaiSKSRenkeiDate);
                DateTime sksRenkeiDate = ComFunc.GetFldToDateTime(dtNext, 0, Def_M_SKS.LASTEST_DATE, DateTime.Now);
                this.SetSKSDateText(sksRenkeiDate);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
            finally
            {
                if (isStart)
                {
                    try
                    {
                        // 処理フラグを更新する。
                        CondJ01 cond = new CondJ01();
                        cond.ShoriFlag = SHORI_FLAG.MISHORI_VALUE1;
                        ConnJ01 conn = new ConnJ01();
                        conn.UpdSKS(cond);
                    }
                    catch (Exception ex)
                    {
                        this.WriteLog(ex);
                    }
                }
            }
        }

        #endregion

        #region SKS連携処理時間設定

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携処理時間設定
        /// </summary>
        /// <param name="nichijiDate">前回SKS連携処理時間</param>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetSKSDateText(DateTime sksRenkeiDate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetShimeDateTextDelegate(this.SetSKSDateText), new Object[] { sksRenkeiDate });
                return;
            }
            this.txtSKSRenkeiDate.Text = sksRenkeiDate.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        #region 次回SKS連携処理時間取得

        /// --------------------------------------------------
        /// <summary>
        /// 次回SKS連携処理時間取得
        /// </summary>
        /// <param name="dt">SKS連携マスタ</param>
        /// <param name="jikaiSKSRenkeiDate">次回SKS連携処理日時</param>
        /// <create>H.Tajimi 2018/11/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetSKSDateTime(DataTable dt, ref DateTime jikaiSKSRenkeiDate)
        {
            try
            {
                DateTime sksRenkeiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SKS.LASTEST_DATE, DateTime.Now);
                string startTime = ComFunc.GetFld(dt, 0, Def_M_SKS.START_TIME, DateTime.Now.ToString("HH:mm:ss"));
                jikaiSKSRenkeiDate = UtilConvert.ToDateTime(sksRenkeiDate.Date.AddDays(1).ToString("yyyy/MM/dd") + " " + startTime);
            }
            catch { }
        }


        #endregion

        #region SKS連携処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携処理
        /// </summary>
        /// <create>H.Tajimi 2018/11/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunSKSRenkei()
        {
            string shoriName = MSG_J01_SKSRENKEI_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                // SKS連携処理開始
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                var conn = new ConnJ01();
                var cond = new CondJ01();
                cond.Lang = ComDefine.RESIDENT_LANG;

                // SKS手配明細WORKの洗い替え処理
                if (!this.RunDeleteInsertTehaiSKSWork(conn, cond))
                {
                    // ここでエラーになるのはSKS手配明細WORKの削除に失敗した時なので終了する
                    endState = MSG_J01_COM_ERROR;
                    return;
                }

                // SKS手配明細更新
                this.RunUpdTehaiSKS(conn, cond);

                // SKS手配明細登録
                this.RunInsTehaiSKS(conn, cond);

                // 手配明細更新
                this.RunUpdTehaiMeisai(conn, cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                // SKS連携処理開始
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region SKS手配明細WORKの洗い替え処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORKの洗い替え処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update>K.Tsutsumi 2018/01/24 分割アップロード</update>
        /// --------------------------------------------------
        private bool RunDeleteInsertTehaiSKSWork(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_SKSRENKEI_REFRESH_TEHAI_SKS_WORK_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                using (var dtOrg = this.ReadCSVToDataTable(this._sksFilePath))
                {
                    if (!UtilData.ExistsData(dtOrg))
                    {
                        endState = MSG_J01_COM_ERROR_READ_CSV;
                        return false;
                    }
                    this.MessageAdd(MSG_J01_SKSRENKEI_READ_SKS_CSV_NAME + string.Format(MSG_J01_COM_READ, dtOrg.Rows.Count));


                    int count = 0;
                    // SKS手配明細WORK 削除処理(全レコード削除)
                    var ret = conn.DelTehaiSKSWork(cond, out count);
                    if (!ret)
                    {
                        endState = MSG_J01_COM_ERROR;
                        return false;
                    }
                    this.MessageAdd(MSG_J01_SKSRENKEI_DELETE_TEHAI_SKS_WORK_NAME + MSG_J01_COM_END);

                    count = 0;
                    var dtTemp = dtOrg.Clone();
                    foreach (DataRow dr in dtOrg.Rows)
                    {
                        if (dtTemp.Rows.Count >= UPLOAD_RECORD_COUNT_PER_TIME)
                        {
                            // SKS手配明細WORK 登録処理
                            var ds = new DataSet();
                            conn.InsTehaiSKSWork(cond, dtTemp, out ds);
                            count += dtTemp.Rows.Count;
                            this.MessageAdd(MSG_J01_SKSRENKEI_INSERT_TEHAI_SKS_WORK_NAME + string.Format(MSG_J01_COM_UPLOAD, count));

                            // エラーメッセージ出力
                            this.AddErrorMessage(ds);
                            if (ds != null)
                            {
                                ds.Dispose();
                                ds = null;
                            }

                            // dtTemp クリア
                            if (dtTemp != null)
                            {
                                dtTemp.Dispose();
                                dtTemp = null;
                            }
                            dtTemp = dtOrg.Clone();
                        }
                        dtTemp.ImportRow(dr);
                    }
                    if (dtTemp.Rows.Count > 0)
                    {
                        // SKS手配明細WORK 登録処理
                        var ds = new DataSet();
                        conn.InsTehaiSKSWork(cond, dtTemp, out ds);
                        count += dtTemp.Rows.Count;
                        this.MessageAdd(MSG_J01_SKSRENKEI_INSERT_TEHAI_SKS_WORK_NAME + string.Format(MSG_J01_COM_UPLOAD, count));

                        // エラーメッセージ出力
                        this.AddErrorMessage(ds);
                        if (ds != null)
                        {
                            ds.Dispose();
                            ds = null;
                        }
                    }

                    // SKS手配明細WORK 更新処理(回答納期が「11111111」のときは完納とする)
                    count = 0;
                    shoriName = MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_WORK_HACCHU_ZYOTAI_NAME;
                    conn.UpdSKSTehaiWorkHacchuZyotai(cond, out count);
                    this.MessageAdd(shoriName + string.Format(MSG_J01_COM_COUNT, count));


                    // SKS手配明細WORK 更新処理(見積状態のときは単価を0円にする)
                    count = 0;
                    shoriName = MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_WORK_UNIT_PRICE_NAME;
                    conn.UpdSKSTehaiWorkTehaiUnitPrice(cond, out count);
                    this.MessageAdd(shoriName + string.Format(MSG_J01_COM_COUNT, count));
                }

                return true;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
                return false;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region SKS手配明細更新処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細更新処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunUpdTehaiSKS(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_SKSRENKEI_UPDATE_TEHAI_SKS_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                // SKS手配明細更新処理実行
                var ds = new DataSet();
                var ret = conn.UpdTehaiSKS(cond, out ds);

                // エラーメッセージ出力
                this.AddErrorMessage(ds);
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (!ret)
                {
                    endState = MSG_J01_COM_ERROR;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
                return false;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region SKS手配明細登録処理

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細登録処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunInsTehaiSKS(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_SKSRENKEI_INSERT_TEHAI_SKS_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                // SKS手配明細登録処理実行
                var ds = new DataSet();
                var ret = conn.InsTehaiSKS(cond, out ds);

                // エラーメッセージ出力
                this.AddErrorMessage(ds);
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (!ret)
                {
                    endState = MSG_J01_COM_ERROR;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
                return false;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 手配明細更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細更新処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunUpdTehaiMeisai(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_SKSRENKEI_UPDATE_TEHAI_MEISAI_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                // 手配明細更新処理実行
                var ds = new DataSet();
                var ret = conn.UpdTehaiMeisai(cond, out ds);

                // エラーメッセージ出力
                this.AddErrorMessage(ds);
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (!ret)
                {
                    endState = MSG_J01_COM_ERROR;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
                return false;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region SKS連携データ取得

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携データ取得
        /// </summary>
        /// <returns>SKS連携データ</returns>
        /// <create>H.Tajimi 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSKSData()
        {
            try
            {
                ConnJ01 conn = new ConnJ01();
                DataSet ds = conn.GetSKS();
                return this.GetSKSData(ds);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                return new DataTable(Def_M_SHIME.Name);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// SKS連携データ取得
        /// </summary>
        /// <param name="ds">DataSet[SKS連携データ]</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSKSData(DataSet ds)
        {
            DataTable dt = new DataTable(Def_M_SKS.Name);
            if (ComFunc.IsExistsTable(ds, Def_M_SKS.Name))
            {
                dt = ds.Tables[Def_M_SKS.Name];
            }
            return dt;
        }

        #endregion

        #region CSV読み込み

        /// --------------------------------------------------
        /// <summary>
        /// CSVファイルの内容から必要な情報を抽出しDataTableに変換
        /// </summary>
        /// <param name="filePath">CSVファイルパス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update>H.Tsuji 2020/06/17 入荷検品エラー Error出力</update>
        /// <update>T.Nukaga 2021/12/21 Step14 受入日/検収日追加</update>
        /// --------------------------------------------------
        private DataTable ReadCSVToDataTable(string filePath)
        {
            string fileContents = string.Empty;
            var ret = this._dtTehaiSKSWork.Clone();
            // 別プロセスからの読み書きを許可したままファイルを読み込む
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var encoding = Encoding.GetEncoding(UtilEncoding.SHIFT_JIS);
                using (var reader = new StreamReader(fs, encoding))
                {
                    fileContents = reader.ReadToEnd();
                    reader.Close();
                    using (var ms = new MemoryStream(encoding.GetBytes(fileContents), false))
                    {
                        using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(ms, encoding))
                        {
                            // カンマ区切り、引用符囲みあり、空白除去しない
                            parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                            parser.SetDelimiters(",");
                            parser.HasFieldsEnclosedInQuotes = true;
                            parser.TrimWhiteSpace = false;

                            // ヘッダ行読み飛ばし
                            parser.ReadFields();

                            while (!parser.EndOfData)
                            {
                                string[] contents;
                                // フィールド読込
                                try
                                {
                                    contents = parser.ReadFields();
                                }
                                catch
                                {
                                    // 基本的には引用符囲みありのTextFieldParserで処理をさせるが、稀に奇数個の引用符を含んでいるデータがある
                                    // 例えば、「"GA-50(W5/8")"」、おそらくデータがおかしいが引用符なしでパースして
                                    // パース後に自力で引用符を消す
                                    using (var ms2 = new MemoryStream(encoding.GetBytes(parser.ErrorLine), false))
                                    using (var parser2 = new Microsoft.VisualBasic.FileIO.TextFieldParser(ms2, encoding))
                                    {
                                        // カンマ区切り、引用符囲みなし、空白除去しない
                                        parser2.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                                        parser2.SetDelimiters(",");
                                        parser2.HasFieldsEnclosedInQuotes = false;
                                        parser2.TrimWhiteSpace = false;
                                        try
                                        {
                                            var tmpContents = parser2.ReadFields();
                                            var lstTmpContents = new List<string>();
                                            foreach (var item in tmpContents)
                                            {
                                                // 引用符を除去する
                                                lstTmpContents.Add(item.Trim(new char[] { '"' }));
                                            }
                                            contents = lstTmpContents.ToArray();
                                        }
                                        catch
                                        {
                                            // それでも失敗する場合は仕方ないのでカンマだけでSplitする
                                            contents = parser.ErrorLine.Split(new string[] { "," }, StringSplitOptions.None);
                                        }
                                    }
                                }

                                // 無変換で設定
                                var dr = ret.NewRow();
                                dr.SetFld(ComDefine.FLD_HACCHU_ZYOTAI_NAME, contents, CsvCol.発注状態);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.SEIBAN_CODE, contents, CsvCol.製番);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.HINBAN, contents, CsvCol.品番);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.PDM_WORK_NAME, contents, CsvCol.ＰＤＭ作業名);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.TEHAI_NO, contents, CsvCol.手配No);
                                dr.SetFldToDecimal(Def_T_TEHAI_SKS_WORK.TEHAI_QTY, contents, CsvCol.手配数量);
                                dr.SetFldToDateStr(Def_T_TEHAI_SKS_WORK.KAITO_DATE, contents, CsvCol.回答納期);
                                dr.SetFldToDecimal(Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE, contents, CsvCol.手配単価);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.ECS_NO, contents, CsvCol.技連No);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.ZUMEN_OIBAN, contents, CsvCol.追番);
                                dr.SetFld(Def_T_TEHAI_SKS_WORK.KATASHIKI, contents, CsvCol.型式);
                                // 付加情報なのでここで例外が発生しても握りつぶす
                                // カスタマー名
                                try
                                {
                                    string customerName = contents.GetValue(CsvCol.カスタマー名);
                                    if (encoding.GetByteCount(customerName) > ComDefine.MAX_BYTE_LENGTH_CUSTOMER_NAME)
                                    {
                                        // 全角なので文字数を半分にする
                                        customerName = customerName.Substring(0, ComDefine.MAX_BYTE_LENGTH_CUSTOMER_NAME / 2);
                                        if (encoding.GetByteCount(customerName) > ComDefine.MAX_BYTE_LENGTH_CUSTOMER_NAME)
                                        {
                                            customerName = string.Empty;
                                        }
                                    }
                                    dr[Def_T_TEHAI_SKS_WORK.CUSTOMER_NAME] = customerName;
                                }
                                catch (Exception ex) { }
                                // 納入場所
                                try
                                {
                                    string nonyubasho = contents.GetValue(CsvCol.納入場所);
                                    if (encoding.GetByteCount(nonyubasho) > ComDefine.MAX_BYTE_LENGTH_NONYUBASHO)
                                    {
                                        // 半角なので文字数そのまま
                                        nonyubasho = nonyubasho.Substring(0, ComDefine.MAX_BYTE_LENGTH_NONYUBASHO);
                                        if (encoding.GetByteCount(nonyubasho) > ComDefine.MAX_BYTE_LENGTH_NONYUBASHO)
                                        {
                                            nonyubasho = string.Empty;
                                        }
                                    }
                                    dr[Def_T_TEHAI_SKS_WORK.NONYUBASHO] = nonyubasho;
                                }
                                catch (Exception ex) { }
                                // 注文書品目名称
                                try
                                {
                                    string chumonshoHinmoku = contents.GetValue(CsvCol.注文書品目名称);
                                    if (encoding.GetByteCount(chumonshoHinmoku) > ComDefine.MAX_BYTE_LENGTH_CHUMONSHO_HINMOKU)
                                    {
                                        // 全角なので文字数を半分にする
                                        chumonshoHinmoku = chumonshoHinmoku.Substring(0, ComDefine.MAX_BYTE_LENGTH_CHUMONSHO_HINMOKU / 2);
                                        if (encoding.GetByteCount(chumonshoHinmoku) > ComDefine.MAX_BYTE_LENGTH_CHUMONSHO_HINMOKU)
                                        {
                                            chumonshoHinmoku = string.Empty;
                                        }
                                    }
                                    dr[Def_T_TEHAI_SKS_WORK.CHUMONSHO_HINMOKU] = chumonshoHinmoku;
                                }
                                catch (Exception ex) { }

                                // 全角＆大文字に変換して設定
                                dr.SetFldForKeishikiS(Def_T_TEHAI_SKS_WORK.ZUMEN_KEISHIKI_S, contents, CsvCol.ＰＤＭ作業名);

                                // 受入日/検収日
                                dr.SetFldToDateStr(Def_T_TEHAI_SKS_WORK.UKEIRE_DATE, contents, CsvCol.受入日);
                                dr.SetFldToDateStr(Def_T_TEHAI_SKS_WORK.KENSHU_DATE, contents, CsvCol.検収日);

                                ret.Rows.Add(dr);
                            }
                        }
                    }
                }
            }

            // 品番に特殊変換
            this.ConvertHinban(ret);

            return ret;
        }

        #endregion

        #region 品番の変換処理

        /// --------------------------------------------------
        /// <summary>
        /// 品番の変換処理
        /// </summary>
        /// <param name="dt"></param>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ConvertHinban(DataTable dt)
        {
            var prefix = "CF";
            var length = 13;
            var fixString = "-";
            // CFから始まる、且つ13文字以上、且つ13文字目が-ではない場合は-を付加する
            var drs = dt.AsEnumerable().Where(x => ComFunc.GetFld(x, Def_T_TEHAI_SKS_WORK.HINBAN).StartsWith(prefix)
                                                && ComFunc.GetFld(x, Def_T_TEHAI_SKS_WORK.HINBAN).Length >= length
                                                && ComFunc.GetFld(x, Def_T_TEHAI_SKS_WORK.HINBAN).Substring(length - 1, 1) != fixString);
            if (drs != null)
            {
                foreach (DataRow dr in drs)
                {
                    dr[Def_T_TEHAI_SKS_WORK.HINBAN] = ComFunc.GetFld(dr, Def_T_TEHAI_SKS_WORK.HINBAN).Insert(length - 1, fixString);
                }
            }
        }

        #endregion

        #region ログの設定パス取得

        /// --------------------------------------------------
        /// <summary>
        /// ログの設定パス取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override string GetLogPutPath()
        {
            if (!string.IsNullOrEmpty(this._logPutPath))
            {
                return this._logPutPath;
            }
            else
            {
                return base.GetLogPutPath();
            }
        }

        #endregion

        #region エラーメッセージ追加処理

        /// --------------------------------------------------
        /// <summary>
        /// エラーメッセージ追加処理
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <create>H.Tajimi 2018/11/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void AddErrorMessage(DataSet ds)
        {
            if (UtilData.ExistsTable(ds, ComDefine.DTTBL_RESULT) && UtilData.ExistsData(ds, ComDefine.DTTBL_RESULT))
            {
                foreach (DataRow dr in ds.Tables[ComDefine.DTTBL_RESULT].Rows)
                {
                    this.MessageAdd(ComFunc.GetFld(dr, ComDefine.FLD_RESULT));
                }
            }
        }

        #endregion
    }

    #region Enum

    /// --------------------------------------------------
    /// <summary>
    /// CSV列情報
    /// </summary>
    /// <create>H.Tajimi 2018/11/19</create>
    /// <update></update>
    /// --------------------------------------------------
    internal enum CsvCol
    {
        発注状態 = 0,
        製番,
        カスタマー名,
        品番,
        ＰＤＭ作業名,
        取引先コード,
        取引先名,
        手配No,
        納期,
        手配数量,
        手配日,
        担当者コード,
        担当者名,
        パラメータ,
        納入先コード,
        納入先名,
        口座,
        版数,
        枝番,
        型式,
        表面処理,
        回答納期,
        受入日,
        検収日,
        受入数,
        検収数,
        単位,
        手配単価,
        手配金額,
        注文区分,
        重量,
        検収単価,
        検収金額,
        支給区分,
        支給区分名,
        課税区分,
        課税区分名,
        ARGMS,
        技連No,
        納入場所,
        追番,
        変更回数,
        注文書品目名称,
        見積,
        原価センターコード,
        原価センターコード名,
        Ｗｋ束No,
        Ｗｋ束連番,
        Ｗｋ処理日,
        Ｗｋ海外用図面発行区分,
        Ｗｋ海外用図面発行日,
        Ｗｋ手配国区分,
        Ｗｋ組立国区分,
        Ｗｋ手配承認No,
        Ｗｋ手配承認日,
        Ｗｋ手配承認者,
        生産Ｎｏ,
        原価コード,
    }

    #endregion

    #region CSVをDataTableに設定するための拡張関数

    /// --------------------------------------------------
    /// <summary>
    /// CSVをDataTableに設定するための拡張関数
    /// </summary>
    /// <create>H.Tajimi 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    internal static class CsvToDataTableExtentsions
    {
        /// --------------------------------------------------
        /// <summary>
        /// Enumを値に変換
        /// </summary>
        /// <param name="csvCol">CSV列情報</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static int GetEnumValue(this CsvCol csvCol)
        {
            return (int)csvCol;
        }

        /// --------------------------------------------------
        /// <summary>
        /// Enum名を取得
        /// </summary>
        /// <param name="csvCol"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static string GetEnumName(this CsvCol csvCol)
        {
            return Enum.GetName(typeof(CsvCol), csvCol);
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値取得(String)
        /// </summary>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static string GetValue(this string[] values, CsvCol csvCol)
        {
            var index = csvCol.GetEnumValue();
            if (values.Length < index)
            {
                return string.Empty;
            }
            else
            {
                return values[index].Trim(new char[] { '"' });
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値取得(Decimal)
        /// </summary>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static decimal GetDecimal(this string[] values, CsvCol csvCol)
        {
            var index = csvCol.GetEnumValue();
            if (values.Length < index)
            {
                return decimal.MinValue;
            }
            else
            {
                return UtilConvert.ToDecimal(values[csvCol.GetEnumValue()]);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値をDataRowの指定フィールドに設定
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="columnName">指定フィールド</param>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static void SetFld(this DataRow dr, string columnName, string[] values, CsvCol csvCol)
        {
            dr[columnName] = values.GetValue(csvCol);
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値を大文字・全角化して指定フィールドに設定
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="columnName">指定フィールド</param>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static void SetFldForKeishikiS(this DataRow dr, string columnName, string[] values, CsvCol csvCol)
        {
#pragma warning disable 0618
            // ここでは意図的に使用しているため警告非表示
            dr[columnName] = ComFunc.ConvertPDMWorkNameToZumenKeishikiS(UtilString.SubstringForByte(values.GetValue(csvCol), 0, ComDefine.MAX_BYTE_LENGTH_ZUMEN_KEISHIKI_S));
#pragma warning restore 0618
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値をDataRowの指定フィールドに設定(Decimal)
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="columnName">指定フィールド</param>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static void SetFldToDecimal(this DataRow dr, string columnName, string[] values, CsvCol csvCol)
        {
            dr[columnName] = values.GetDecimal(csvCol);
        }

        /// --------------------------------------------------
        /// <summary>
        /// String配列内の指定位置の値をDataRowの指定フィールドに設定(日付の文字列型)
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="columnName">指定フィールド</param>
        /// <param name="values">String配列</param>
        /// <param name="csvCol">CSV列情報</param>
        /// <create>H.Tajimi 2018/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        internal static void SetFldToDateStr(this DataRow dr, string columnName, string[] values, CsvCol csvCol)
        {
            var value = values.GetValue(csvCol);
            if (value == ComDefine.KAITO_DATE_NONE)
            {
                dr[columnName] = string.Empty;
                return;
            }
            else
            {
                if (value != ComDefine.KAITO_DATE_KANNOU)
                {
                    DateTime result;
                    if (DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out result))
                    {
                        dr[columnName] = result.ToString("yyyy/MM/dd");
                        return;
                    }
                }
            }
            dr[columnName] = value;
        }
    }

    #endregion
}
