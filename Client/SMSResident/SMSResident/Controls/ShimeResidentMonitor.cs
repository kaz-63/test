using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using Commons;
using WsConnection;
using WsConnection.WebRefJ01;

namespace SMSResident.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// 締め処理用コントロール
    /// </summary>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShimeResidentMonitor : SMSResident.Controls.BaseResidentMonitor
    {
        #region Fields

        private DateTime _jikaiNichijiDate = new DateTime();
        private DateTime _jikaiGetsujiDate = new DateTime();
        private string _nichijiBackupPath = string.Empty;
        private string _getsujiBackupPath = string.Empty;

        #endregion

        #region 定数

        private const string XCOPY_EXE = "xcopy.exe";
        private const string XCOPY_ARGUMENTS = "\"{0}\" \"{1}\" /D /E /Q /Y /C";

        /// --------------------------------------------------
        /// <summary>
        /// 実行バッチファイル名
        /// </summary>
        /// <create>K.Tsutsumi 2020/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string EXEC_FILENAME = "00.実行.bat";

        #endregion

        #region メッセージ定数

        /// --------------------------------------------------
        /// <summary>
        /// 既に締め処理が行われています。
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_PROCESSED = "既に締め処理が行われています。";
        /// --------------------------------------------------
        /// <summary>
        /// 締めマスタが存在しません。
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NOTFOUND_M_SHIME = "締めマスタが存在しません。";
        /// --------------------------------------------------
        /// <summary>
        /// 開始
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_COM_START = "開始";
        /// --------------------------------------------------
        /// <summary>
        /// 完了
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_COM_END = "完了";
        /// --------------------------------------------------
        /// <summary>
        /// 異常終了
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_COM_ERROR = "異常終了";
        /// --------------------------------------------------
        /// <summary>
        /// 月次ファイルバックアップ処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_GETSUJI_FILEBACKUP_NAME = "月次ファイルバックアップ処理";
        private const string MSG_J01_GETSUJI_FILEBACKUP_NAME = "月次Files Backup処理";
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// 日次処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_NAME = "日次処理";
        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ(本体)完了処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_UPD_HONTAI_NONYUSAKI_KANRYO_NAME = "納入先マスタ(本体)完了処理";
        private const string MSG_J01_NICHIJI_UPD_HONTAI_NONYUSAKI_KANRYO_NAME = "納入先Master(本体)完了処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ(本体)削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_HONTAI_NONYUSAKI_DATA_NAME = "納入先マスタ(本体)削除処理";
        private const string MSG_J01_NICHIJI_DEL_HONTAI_NONYUSAKI_DATA_NAME = "納入先Master(本体)削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 納入先Master(本体)経過年数削除処理
        /// </summary>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_HONTAI_NONYUSAKI_DATA_UNCONDITION_NAME = "納入先Master(本体)経過年数削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名Master(本体)削除処理
        /// </summary>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_HONTAI_BUKKEN_DATA_NAME = "物件名Master(本体)削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データ(本体)削除処理開始
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_HONTAI_SHUKKAMEISAI_DATA_NAME = "出荷明細データ(本体)削除処理開始";
        private const string MSG_J01_NICHIJI_DEL_HONTAI_SHUKKAMEISAI_DATA_NAME = "出荷明細Data(本体)削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 納入先マスタ(AR)削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_AR_NONYUSAKI_DATA_NAME = "納入先マスタ(AR)削除処理";
        private const string MSG_J01_NICHIJI_DEL_AR_NONYUSAKI_DATA_NAME = "納入先Master(AR)削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 納入先Master(AR)経過年数削除処理
        /// </summary>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_AR_NONYUSAKI_DATA_UNCONDITION_NAME = "納入先Master(AR)経過年数削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 物件名Master(AR)削除処理
        /// </summary>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_AR_BUKKEN_DATA_NAME = "物件名Master(AR)削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_AR_DATA_NAME = "AR情報データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_AR_DATA_NAME = "AR情報Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 出荷明細データ(AR)削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_AR_SHUKKAMEISAI_DATA_NAME = "出荷明細データ(AR)削除処理";
        private const string MSG_J01_NICHIJI_DEL_AR_SHUKKAMEISAI_DATA_NAME = "出荷明細Data(AR)削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// BOXリスト管理データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_BOXLISTMANAGE_DATA_NAME = "BOXリスト管理データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_BOXLISTMANAGE_DATA_NAME = "BOX List管理Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// パレットリスト管理データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_PALLETLISTMANAGE_DATA_NAME = "パレットリスト管理データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_PALLETLISTMANAGE_DATA_NAME = "PALLET List管理Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_KIWAKU_DATA_NAME = "木枠データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_KIWAKU_DATA_NAME = "木枠Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_KIWAKUMEISAI_DATA_NAME = "木枠明細データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_KIWAKUMEISAI_DATA_NAME = "木枠明細Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 社外用木枠明細データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_SHAGAIKIWAKUMEISAI_DATA_NAME = "社外用木枠明細データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_SHAGAIKIWAKUMEISAI_DATA_NAME = "社外用木枠明細Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 採番データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_SAIBAN_DATA_NAME = "採番データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_SAIBAN_DATA_NAME = "採番Data削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_TEMPWORK_DATA_NAME = "一時作業データ削除処理";
        private const string MSG_J01_NICHIJI_DEL_TEMPWORK_DATA_NAME = "一時作業Data削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 履歴データ(本体)削除処理
        /// </summary>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_HONTAI_RIREKI_DATA_NAME = "履歴Data(本体)削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 履歴データ(AR)削除処理
        /// </summary>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_AR_RIREKI_DATA_NAME = "履歴Data(AR)削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// ロケーションマスタ削除処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_LOCATION_DATA_NAME = "LocationMaster削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 在庫データ削除処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_STOCK_DATA_NAME = "在庫Data削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 棚卸データ削除処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_INVENT_DATA_NAME = "棚卸Data削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 実績データ削除処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_JISSEKI_DATA_NAME = "実績Data削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ(現地部品管理)削除処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_BUHIN_TEMPWORK_DATA_NAME = "一時作業Data(現地部品管理)削除処理";
        /// --------------------------------------------------
        /// <summary>
        /// 技連ファイル削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_GIREN_FILE_NAME = "技連ファイル削除処理";
        private const string MSG_J01_NICHIJI_DEL_GIREN_FILE_NAME = "技連Files削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// CASE MARKファイル削除処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/22 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DEL_CASEMARK_FILE_NAME = "CASE MARKファイル削除処理";
        private const string MSG_J01_NICHIJI_DEL_CASEMARK_FILE_NAME = "CASE MARK Files削除処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// DBバックアップ処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update>K.Tsutsumi 2020/06/26 トランザクションログ切り捨てと圧縮</update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_DBBACKUP_NAME = "DBバックアップ処理";
        private const string MSG_J01_NICHIJI_DBBACKUP_NAME = "DB BackupとTransaction Logの切り捨てと圧縮処理";
        // ↑
        /// --------------------------------------------------
        /// <summary>
        /// DBバックアップの圧縮と保管期間管理
        /// </summary>
        /// <create>K.Tsutsumi 2020/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_ZIP_AND_DELETE_NAME = "DB Backupの圧縮と保管期間管理処理";
        /// --------------------------------------------------
        /// <summary>
        /// ファイルバックアップ処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        // 2011/02/21 K.Tsutsumi Change カタカナ禁止
        //private const string MSG_J01_NICHIJI_FILEBACKUP_NAME = "ファイルバックアップ処理";
        private const string MSG_J01_NICHIJI_FILEBACKUP_NAME = "Files Backup処理";
        // ↑

        /// --------------------------------------------------
        /// <summary>
        /// メールデータ削除処理
        /// </summary>
        /// <create>T.Sakiori 2017/09/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private const string MSG_J01_NICHIJI_DEL_MAIL_NAME = "Mail Data削除処理";

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// 前回締め処理時間設定用デリゲート
        /// </summary>
        /// <param name="message"></param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void SetShimeDateTextDelegate(DateTime nichijiDate);

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShimeResidentMonitor()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeControl()
        {
            try
            {
                // 初期化処理
                // 取り敢えずフラグを未処理にする。
                try
                {
                    // 処理フラグを更新する。
                    CondJ01 cond = new CondJ01();
                    cond.ShoriFlag = SHORI_FLAG.MISHORI_VALUE1;
                    ConnJ01 conn = new ConnJ01();
                    conn.UpdShime(cond);
                }
                catch (Exception ex)
                {
                    this.WriteLog(ex);
                }
                DataTable dt = this.GetShimeData();
                DateTime nichijiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SHIME.NICHIJI_DATE, DateTime.Now);
                this.SetShimeDateText(nichijiDate);
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
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void AsynchronousMonitor()
        {
            try
            {
                // 常駐処理を始める前の初期処理をここに記述します。
                // InitializeControlがOnLoadで呼ばれるためタブが表示されていないと実行されないので処理に必要な分は此処で行う。
                DataTable dt = this.GetShimeData();
                this.SetShimeDateTime(dt, ref this._jikaiNichijiDate, ref this._jikaiGetsujiDate);
                DateTime nichijiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SHIME.NICHIJI_DATE, DateTime.Now);
                this.SetShimeDateText(nichijiDate);

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
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void MonitorExecute()
        {
            bool isStart = false;
            try
            {
                // 日時処理開始チェック
                if (DateTime.Now < this._jikaiNichijiDate) return;
                ConnJ01 conn = new ConnJ01();
                DataSet ds;
                bool isProcessed;
                if (!conn.StartShime(out ds, out isProcessed))
                {
                    if (isProcessed)
                    {
                        this.MessageAdd(MSG_J01_PROCESSED);

                        DateTime zenkaiNichijiDate = ComFunc.GetFldToDateTime(ds, Def_M_SHIME.Name, 0, Def_M_SHIME.NICHIJI_DATE);
                        // 他端末で締めが行われていたら次回日次処理日時を更新
                        if (this._jikaiNichijiDate.Date <= zenkaiNichijiDate.Date)
                        {
                            this.SetShimeDateTime(ds.Tables[Def_M_SHIME.Name], ref this._jikaiNichijiDate, ref this._jikaiGetsujiDate);
                            return;
                        }
                    }
                    else
                    {
                        this.MessageAdd(MSG_J01_NOTFOUND_M_SHIME);
                    }
                    return;
                }
                CondJ01 cond = new CondJ01();
                cond.ShoriFlag = SHORI_FLAG.MISHORI_VALUE1;
                isStart = true;
                DataTable dt = new DataTable();
                if (ComFunc.IsExistsTable(ds, Def_M_SHIME.Name))
                {
                    dt = ds.Tables[Def_M_SHIME.Name];
                }
                this.SetFolderInfo(dt);

                // 月次処理チェック
                if (this._jikaiGetsujiDate.Date <= DateTime.Now.Date)
                {
                    cond.GetsujiDate = DateTime.Now;
                    this.RunGetsuji(dt);
                }

                cond.NichijiDate = DateTime.Now;
                this.RunNichiji(dt);

                // 締めマスタ更新
                conn.UpdShime(cond);
                isStart = false;

                // 次回締め処理日時更新
                DataSet dsNext = conn.GetShime();
                DataTable dtNext = new DataTable();
                if (ComFunc.IsExistsTable(dsNext, Def_M_SHIME.Name))
                {
                    dtNext = dsNext.Tables[Def_M_SHIME.Name];
                }
                this.SetShimeDateTime(dtNext, ref this._jikaiNichijiDate, ref this._jikaiGetsujiDate);
                DateTime nichijiDate = ComFunc.GetFldToDateTime(dtNext, 0, Def_M_SHIME.NICHIJI_DATE, DateTime.Now);
                this.SetShimeDateText(nichijiDate);
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
                        conn.UpdShime(cond);
                    }
                    catch (Exception ex)
                    {
                        this.WriteLog(ex);
                    }
                }
            }
        }

        #endregion

        #region 締め処理時間設定

        /// --------------------------------------------------
        /// <summary>
        /// 締め処理時間設定
        /// </summary>
        /// <param name="nichijiDate">前回日次処理時間</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetShimeDateText(DateTime nichijiDate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetShimeDateTextDelegate(this.SetShimeDateText), new Object[] { nichijiDate });
                return;
            }
            this.txtNichijiDate.Text = nichijiDate.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        #region 次回締め処理時間取得

        /// --------------------------------------------------
        /// <summary>
        /// 次回締め処理時間取得
        /// </summary>
        /// <param name="dt">締めマスタ</param>
        /// <param name="jikaiNichijiDate">次回日時処理日時</param>
        /// <param name="jikaiGetsujiDate">次回月次処理日時</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetShimeDateTime(DataTable dt, ref DateTime jikaiNichijiDate, ref DateTime jikaiGetsujiDate)
        {
            try
            {
                DateTime nichijiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SHIME.NICHIJI_DATE, DateTime.Now);
                DateTime getsujiDate = ComFunc.GetFldToDateTime(dt, 0, Def_M_SHIME.NICHIJI_DATE, DateTime.Now);
                string startTime = ComFunc.GetFld(dt, 0, Def_M_SHIME.START_TIME, DateTime.Now.ToString("HH:mm:ss"));
                jikaiNichijiDate = UtilConvert.ToDateTime(nichijiDate.Date.AddDays(1).ToString("yyyy/MM/dd") + " " + startTime);
                jikaiGetsujiDate = UtilConvert.ToDateTime(nichijiDate.Date.AddMonths(1).ToString("yyyy/MM/01") + " " + startTime);
            }
            catch { }
        }


        #endregion

        #region フォルダ情報設定

        /// --------------------------------------------------
        /// <summary>
        /// フォルダ情報設定
        /// </summary>
        /// <param name="dt">締めマスタ</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFolderInfo(DataTable dt)
        {
            try
            {
                string fileBackup = ComFunc.GetFld(dt, 0, Def_M_SHIME.FILE_BACKUP_PATH);
                this._nichijiBackupPath = Path.Combine(fileBackup, ComDefine.WEB_DATA_DIR_NAME_DAILY);
                this._getsujiBackupPath = Path.Combine(fileBackup, ComDefine.WEB_DATA_DIR_NAME_MONTHLY);
            }
            catch { }
        }

        #endregion

        #region 月次処理

        /// --------------------------------------------------
        /// <summary>
        /// 月次処理
        /// </summary>
        /// <param name="dt">締めマスタ</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunGetsuji(DataTable dt)
        {
            string shoriName = MSG_J01_GETSUJI_FILEBACKUP_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                string targetDirName = DateTime.Today.AddMonths(-1).ToString("yyyyMM");
                string targetPath = Path.Combine(this._nichijiBackupPath, targetDirName);
                string destPath = Path.Combine(this._getsujiBackupPath, targetDirName);

                // 移動元が無ければ抜ける。
                if (!Directory.Exists(targetPath)) return;
                // 移動先存在チェック
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                // 移動先の過去分を削除
                foreach (string dirName in Directory.GetDirectories(this._getsujiBackupPath))
                {
                    try
                    {
                        Directory.Delete(dirName, true);
                    }
                    catch (Exception exIO)
                    {
                        this.WriteLog(exIO);
                    }
                }
                // 月次フォルダに移動
                Directory.Move(targetPath, destPath);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 日次処理

        /// --------------------------------------------------
        /// <summary>
        /// 日次処理
        /// </summary>
        /// <param name="dt">締めマスタ</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update>T.Sakiori 2015/06/11 指定年で無条件削除処理を追加</update>
        /// <update>K.Tsutsumi 2020/06/26 DBBackupの圧縮と12日間の保存処理をバッチで行う</update>
        /// --------------------------------------------------
        private void RunNichiji(DataTable dt)
        {
            string shoriName = MSG_J01_NICHIJI_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                // 日次処理開始
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                // 削除対象日取得
                DateTime targetDateTime = DateTime.Today.AddMonths(ComFunc.GetFldToInt32(dt, 0, Def_M_SHIME.HOJIKIKAN) * -1);
                ConnJ01 conn = new ConnJ01();
                CondJ01 cond = new CondJ01();
                cond.TargetDate = targetDateTime;
                cond.ExpBackupPath = ComFunc.GetFld(dt, 0, Def_M_SHIME.EXP_BACKUP_PATH);

                // 本体の納入先マスタ完了処理
                this.RunNichiji_UpdHontaiNonyusakiKanryo(conn, cond);

                // 本体の納入先マスタ削除処理
                this.RunNichiji_DelHontaiNonyusakiData(conn, cond);

                // 本体の納入先マスタ削除処理(指定年で無条件)
                this.RunNichiji_DelHontaiNonyusakiDataUncondition(conn, cond);

                // 本体の物件名マスタ削除処理
                this.RunNichiji_DelHontaiBukkenData(conn, cond);

                // 本体の出荷明細データ削除処理
                this.RunNichiji_DelHontaiShukkaMeisaiData(conn, cond);

                // 本体の履歴データ削除処理
                this.RunNichiji_DelHontaiRirekiData(conn, cond);

                // ARの納入先マスタ削除処理
                this.RunNichiji_DelARNonyusakiData(conn, cond);

                // ARの納入先マスタ削除処理(指定年で無条件)
                this.RunNichiji_DelARNonyusakiDataUncondition(conn, cond);

                // ARの物件名マスタ削除処理
                this.RunNichiji_DelARBukkenData(conn, cond);

                // AR情報データ削除処理
                DataSet dsAR;
                this.RunNichiji_DelARData(conn, cond, out dsAR);

                // ARの出荷明細データ削除処理
                this.RunNichiji_DelARShukkaMeisaiData(conn, cond);

                // ARの履歴データ削除処理
                this.RunNichiji_DelARRirekiData(conn, cond);

                // BOXリスト管理データ削除処理
                this.RunNichiji_DelBoxlistManageData(conn, cond);

                // パレットリスト管理データ削除処理
                this.RunNichiji_DelPalletlistManageData(conn, cond);

                // 木枠データ削除処理
                DataSet dsKiwaku;
                this.RunNichiji_DelKiwakuData(conn, cond, out dsKiwaku);

                // 木枠明細データ削除処理
                this.RunNichiji_DelKiwakuMeisaiData(conn, cond);

                // 社外木枠明細データ削除処理
                this.RunNichiji_DelShagaiKiwakuMeisaiData(conn, cond);

                // 採番データ削除処理
                this.RunNichiji_DelSaibanData(conn, cond);

                // 一時作業データ削除処理
                this.RunNichiji_DelTempworkData(conn, cond);

                // ロケーションマスタ削除処理
                this.RunNichiji_DelLocationData(conn, cond);

                // 在庫データ削除処理
                this.RunNichiji_DelStockData(conn, cond);

                // 棚卸データ削除処理
                this.RunNichiji_DelInventData(conn, cond);

                // 実績データ削除処理
                this.RunNichiji_DelJissekiData(conn, cond);

                // 一時作業データ（現地部品管理）削除処理
                this.RunNichiji_DelBuhinTempworkData(conn, cond);

                // 技連ファイル削除処理
                this.RunNichiji_DelGirenFile(dsAR);

                // メールデータ削除処理
                this.RunNichiji_DelMail(conn, cond);

                // CASE MARKファイル削除処理
                this.RunNichiji_DelCasemarkFile(dsKiwaku);

                // DBバックアップ処理
                this.RunNichiji_DBBackup(conn, cond);

                // DBバックアップの圧縮と保管期間管理
                this.RunNichiji_Zip_and_Delete(cond);


                // ↓ 2010/09/19 ファイルバックアップはAcronisでするので締めではしない。
                //// ファイルバックアップ
                //this.RunNichiji_FileBackup(dt);
                // ここまで 2010/09/19

            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                // 日次処理終了
                this.MessageAdd(shoriName + endState);
            }
        }

        #region 本体の納入先マスタ完了処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ完了処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_UpdHontaiNonyusakiKanryo(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_UPD_HONTAI_NONYUSAKI_KANRYO_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.UpdHontaiNonyusakiKanryo(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 本体の納入先マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelHontaiNonyusakiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_HONTAI_NONYUSAKI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelHontaiNonyusakiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 本体の納入先マスタ削除処理(指定年で無条件)

        /// --------------------------------------------------
        /// <summary>
        /// 本体の納入先マスタ削除処理(指定年で無条件)
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelHontaiNonyusakiDataUncondition(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_HONTAI_NONYUSAKI_DATA_UNCONDITION_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelHontaiNonyusakiDataUncondition(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 本体の物件名マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の物件名マスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelHontaiBukkenData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_HONTAI_BUKKEN_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                conn.DelHontaiBukkenData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 本体の出荷明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の出荷明細データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelHontaiShukkaMeisaiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_HONTAI_SHUKKAMEISAI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelHontaiShukkaMeisaiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ARの納入先マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの納入先マスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARNonyusakiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_AR_NONYUSAKI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelARNonyusakiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ARの納入先マスタ削除処理(指定年で無条件)

        /// --------------------------------------------------
        /// <summary>
        /// ARの納入先マスタ削除処理(指定年で無条件)
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2015/06/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARNonyusakiDataUncondition(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_AR_NONYUSAKI_DATA_UNCONDITION_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelARNonyusakiDataUncondition(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ARの物件名マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの物件名マスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARBukkenData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_AR_BUKKEN_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                conn.DelARBukkenData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region AR情報データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// AR情報データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <param name="ds">削除対象データ</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARData(ConnJ01 conn, CondJ01 cond, out DataSet ds)
        {
            ds = new DataSet();
            string shoriName = MSG_J01_NICHIJI_DEL_AR_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelARData(cond, out ds);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ARの出荷明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの出荷明細データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARShukkaMeisaiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_AR_SHUKKAMEISAI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelARShukkaMeisaiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region BOXリスト管理データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// BOXリスト管理データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelBoxlistManageData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_BOXLISTMANAGE_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelBoxlistManageData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region パレットリスト管理データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// パレットリスト管理データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelPalletlistManageData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_PALLETLISTMANAGE_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelPalletlistManageData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 木枠データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <param name="ds">削除対象データ</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelKiwakuData(ConnJ01 conn, CondJ01 cond, out DataSet ds)
        {
            ds = new DataSet();
            string shoriName = MSG_J01_NICHIJI_DEL_KIWAKU_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelKiwakuData(cond, out ds);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 木枠明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelKiwakuMeisaiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_KIWAKUMEISAI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelKiwakuMeisaiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 社外木枠明細データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 社外木枠明細データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelShagaiKiwakuMeisaiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_SHAGAIKIWAKUMEISAI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelShagaiKiwakuMeisaiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 採番マスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 採番マスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelSaibanData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_SAIBAN_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelSaibanARNo(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 一時作業データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/09/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelTempworkData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_TEMPWORK_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelTempworkData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 本体の履歴データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 本体の履歴データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelHontaiRirekiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_HONTAI_RIREKI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelHontaiRirekiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ARの履歴データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ARの履歴データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2012/05/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelARRirekiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_AR_RIREKI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelARRirekiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region ロケーションマスタ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションマスタ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelLocationData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_LOCATION_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelLocationData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 在庫データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 在庫データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelStockData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_STOCK_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelStockData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 棚卸データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 棚卸データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelInventData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_INVENT_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelInventData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 実績データ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 実績データ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelJissekiData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_JISSEKI_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelJissekiData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 一時作業データ（現地部品管理）削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データ（現地部品管理）削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Wakamatsu 2013/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelBuhinTempworkData(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_BUHIN_TEMPWORK_DATA_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DelBuhinTempworkData(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region 技連ファイル削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 技連ファイル削除処理
        /// </summary>
        /// <param name="ds">AR削除対象データ</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelGirenFile(DataSet ds)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_GIREN_FILE_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                string path = Path.Combine(ComDefine.WEB_DATA_DIR_ROOT, ComDefine.WEB_DATA_DIR_NAME_GIREN);
                if (!ComFunc.IsExistsData(ds, Def_T_AR.Name)) return;
                foreach (DataRow dr in ds.Tables[Def_T_AR.Name].Rows)
                {
                    try
                    {
                        string nonyusakiPath = Path.Combine(path, ComFunc.GetFld(dr, Def_T_AR.NONYUSAKI_CD));
                        string arnoPath = Path.Combine(nonyusakiPath, ComFunc.GetFld(dr, Def_T_AR.AR_NO));
                        Directory.Delete(arnoPath, true);
                        if (Directory.GetDirectories(nonyusakiPath).Length == 0)
                        {
                            Directory.Delete(nonyusakiPath, true);
                        }
                    }
                    catch (Exception exIO)
                    {
                        this.WriteLog(exIO);
                    }
                }

            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region CASE MARKファイル削除処理

        /// --------------------------------------------------
        /// <summary>
        /// CASE MARKファイル削除処理
        /// </summary>
        /// <param name="ds">木枠削除対象データ</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelCasemarkFile(DataSet ds)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_CASEMARK_FILE_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                string path = Path.Combine(ComDefine.WEB_DATA_DIR_ROOT, ComDefine.WEB_DATA_DIR_NAME_CASEMARK);
                if (!ComFunc.IsExistsData(ds, Def_T_KIWAKU.Name)) return;
                foreach (DataRow dr in ds.Tables[Def_T_KIWAKU.Name].Rows)
                {
                    try
                    {
                        string kojiNoPath = Path.Combine(path, ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO));
                        Directory.Delete(kojiNoPath, true);
                    }
                    catch (Exception exIO)
                    {
                        this.WriteLog(exIO);
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region DBバックアップ処理

        /// --------------------------------------------------
        /// <summary>
        /// DBバックアップ処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DBBackup(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DBBACKUP_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);

                conn.DBBackup(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region DBバックアップの圧縮と保管期間管理

        /// --------------------------------------------------
        /// <summary>
        /// DBバックアップの圧縮と保管期間管理
        /// </summary>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>K.Tsutsumi 2020/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_Zip_and_Delete(CondJ01 cond)
        {

            string shoriName = MSG_J01_NICHIJI_ZIP_AND_DELETE_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                var lst = new List<string>();
                lst.Add(cond.ExpBackupPath);
                this.ControlBatchExec(lst.ToArray());
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #region フォルダ内のバッチ実行制御処理

        /// --------------------------------------------------
        /// <summary>
        /// フォルダ内のバッチ実行制御処理
        /// </summary>
        /// <param name="targetDirectories">対象フォルダの配列</param>
        /// <create>K.Tsutsumi 2020/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ControlBatchExec(string[] targetDirectories)
        {
            try
            {
                foreach (string beforeExecDir in targetDirectories)
                {
                    // 実行バッチファイル名取得
                    string fileName = Path.Combine(beforeExecDir, EXEC_FILENAME);
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }

                    // バッチファイル実行
                    this.ExecBatch(fileName);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region バッチファイル実行

        /// --------------------------------------------------
        /// <summary>
        /// バッチファイルを実行する
        /// </summary>
        /// <param name="fileName">バッチファイル名</param>
        /// <create>K.Tsutsumi 2020/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ExecBatch(string fileName)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = fileName;

                // 別ウィンドウは表示しない
                psi.CreateNoWindow = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                // 処理が終了するまで待つ
                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    proc.WaitForExit();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region ファイルバックアップ処理

        /// --------------------------------------------------
        /// <summary>
        /// ファイルバックアップ処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_FileBackup(DataTable dt)
        {
            string shoriName = MSG_J01_NICHIJI_FILEBACKUP_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                string path = Path.Combine(this._nichijiBackupPath, DateTime.Today.ToString("yyyyMM"));
                string caseMarkPath = Path.Combine(ComDefine.WEB_DATA_DIR_ROOT, ComDefine.WEB_DATA_DIR_NAME_CASEMARK);
                string girenPath = Path.Combine(ComDefine.WEB_DATA_DIR_ROOT, ComDefine.WEB_DATA_DIR_NAME_GIREN);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // 技連フォルダのバックアップ
                this.XCopy(girenPath, Path.Combine(path, ComDefine.WEB_DATA_DIR_NAME_GIREN));
                // CASE MARKフォルダのバックアップ
                this.XCopy(caseMarkPath, Path.Combine(path, ComDefine.WEB_DATA_DIR_NAME_CASEMARK));

            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// xcopy実行
        /// </summary>
        /// <param name="sourcePath">コピー元パス</param>
        /// <param name="destPath">コピー先パス</param>
        /// <create>Y.Higuchi 2010/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void XCopy(string sourcePath, string destPath)
        {
            try
            {
                string cmdString = string.Empty;
                string source = Path.GetDirectoryName(sourcePath);
                string dest = Path.GetDirectoryName(destPath) + Path.DirectorySeparatorChar;
                cmdString = string.Format(XCOPY_ARGUMENTS, source, dest);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.System), XCOPY_EXE);
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments = cmdString;
                process.Start();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        #endregion

        #region メールデータ削除処理

        /// --------------------------------------------------
        /// <summary>
        /// メールデータ削除処理
        /// </summary>
        /// <param name="conn">J01用のWsコネクション</param>
        /// <param name="cond">J01用のコンディション</param>
        /// <create>T.Sakiori 2017/09/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RunNichiji_DelMail(ConnJ01 conn, CondJ01 cond)
        {
            string shoriName = MSG_J01_NICHIJI_DEL_MAIL_NAME;
            string endState = MSG_J01_COM_END;
            try
            {
                this.MessageAdd(shoriName + MSG_J01_COM_START);
                conn.DelMail(cond);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                endState = MSG_J01_COM_ERROR;
            }
            finally
            {
                this.MessageAdd(shoriName + endState);
            }
        }

        #endregion

        #endregion

        #region 締めデータ取得

        /// --------------------------------------------------
        /// <summary>
        /// 締めデータ取得
        /// </summary>
        /// <returns>締めデータ</returns>
        /// <create>Y.Higuchi 2010/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetShimeData()
        {
            try
            {
                ConnJ01 conn = new ConnJ01();
                DataSet ds = conn.GetShime();
                DataTable dt = new DataTable(Def_M_SHIME.Name);
                if (ComFunc.IsExistsTable(ds, Def_M_SHIME.Name))
                {
                    dt = ds.Tables[Def_M_SHIME.Name];
                }
                return dt;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
                return new DataTable(Def_M_SHIME.Name);
            }
        }

        #endregion
    }
}
