using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using DSWUtil;
using DSWUtil.Log;
using Commons;

namespace SMSResident.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// 常駐処理ベースコントロールクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BaseResidentMonitor : UserControl, IResidentMonitor
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 処理呼出しする時間間隔（秒）
        /// </summary>    
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _checkTime = 30;
        /// --------------------------------------------------
        /// <summary>
        /// スレッドの実行状態
        /// </summary>    
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _residentState = false;
        /// --------------------------------------------------
        /// <summary>
        /// 非同期常駐処理
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private DelegateMonitor _asynCall = null;
        /// --------------------------------------------------
        /// <summary>
        /// 非同期スレッドのIAsyncResult
        /// </summary>    
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private IAsyncResult _ar = null;
        /// --------------------------------------------------
        /// <summary>
        /// 終了要求フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isStopRequest = false;
        /// --------------------------------------------------
        /// <summary>
        /// ロック用
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private ReaderWriterLock _rwStopRequestLock = new ReaderWriterLock();
        /// --------------------------------------------------
        /// <summary>
        /// ロック用
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private ReaderWriterLock _rwCheckTimeLock = new ReaderWriterLock();
        /// --------------------------------------------------
        /// <summary>
        /// ログ用
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update>K.Tsutsumi 2011/01/24</update>
        /// --------------------------------------------------
        // 2011/01/24 K.Tsutsumi Change 
        //private UtilLog _log = new UtilLog();
        private UtilLog _log = null;
        // ↑
        #endregion

        #region Delegate
        /// --------------------------------------------------
        /// <summary>
        /// 非同期常駐処理用デリゲート
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateMonitor();
        /// --------------------------------------------------
        /// <summary>
        /// メッセージのクリア用デリゲート
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void MessageClearDelegate();
        /// --------------------------------------------------
        /// <summary>
        /// メッセージの追加用デリゲート
        /// </summary>
        /// <param name="message"></param>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void MessageAddDelegate(string message);
        /// --------------------------------------------------
        /// <summary>
        /// 実行ラジオボタンのチェック変更用デリゲート
        /// </summary>
        /// <param name="value"></param>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void ChangeStartCheckedDelegate(bool value);

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseResidentMonitor()
        {
            InitializeComponent();

            // 2011/01/24 K.Tsutsumi Add 複数常駐が存在するため移動
            this.InitializeControl();
            // ↑
        }

        #endregion

        #region Properties

        #region 処理呼出しする時間間隔（秒）

        /// --------------------------------------------------
        /// <summary>
        /// 処理呼出しする時間間隔（秒）
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        [Category("カスタム動作")]
        [Description("処理呼出しする時間間隔（秒）")]
        public virtual int CheckTime
        {
            get
            {
                try
                {
                    // リーダーロック取得
                    this._rwCheckTimeLock.AcquireReaderLock(Timeout.Infinite);
                    try
                    {
                        return this._checkTime;
                    }
                    finally
                    {
                        //リーダーロック解放
                        this._rwCheckTimeLock.ReleaseReaderLock();
                    }
                }
                catch (Exception)
                {
                    return 30;
                }
            }
            set
            {
                try
                {
                    // ライタロック取得
                    this._rwCheckTimeLock.AcquireWriterLock(Timeout.Infinite);
                    try
                    {
                        this._checkTime = value;
                    }
                    finally
                    {
                        // ライタロック解放
                        this._rwCheckTimeLock.ReleaseWriterLock();
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

        #region 常駐処理状態

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理状態
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public virtual bool ResidentState
        {
            get { return this._residentState; }
        }

        #endregion

        #region 終了要求プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 終了要求プロパティ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool IsStopRequest
        {
            get
            {
                try
                {
                    // リーダーロック取得
                    this._rwStopRequestLock.AcquireReaderLock(Timeout.Infinite);
                    try
                    {
                        return this._isStopRequest;
                    }
                    finally
                    {
                        // リーダーロック解放
                        this._rwStopRequestLock.ReleaseReaderLock();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    // ライタロック取得
                    this._rwStopRequestLock.AcquireWriterLock(Timeout.Infinite);
                    try
                    {
                        this._isStopRequest = value;
                    }
                    finally
                    {
                        // ライタロック解放
                        this._rwStopRequestLock.ReleaseWriterLock();
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

        #region ログファイル名

        /// --------------------------------------------------
        /// <summary>
        /// ログファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string LogFileName
        {
            get { return this.Name; }
        }

        #endregion

        #endregion

        #region Methods

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeControl()
        {
            this.rdoStart.Checked = false;
            this.rdoStop.Checked = true;
            this._residentState = false;
            this.lstMessage.Items.Clear();
            // 2011/01/24 K.Tsutsumi Delete ここだと、LogFileNameが取得できない
            //this._log.Logger.Setting.LogPutPath = ComDefine.LOG_PUT_PATH;
            //this._log.Logger.Setting.LogBaseName = this.LogFileName;
            //this._log.LogPutMode = LoggerPutMode.TextLogFile;

            if (this.rdoStart.Visible
                && this.rdoStart.Enabled)
            {
                //this.rdoStart.CheckedChanged += new System.EventHandler(this.rdoStart_CheckedChanged);
            }
        }

        #endregion

        #region メッセージ関係

        /// --------------------------------------------------
        /// <summary>
        /// メッセージのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public void MessageClear()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MessageClearDelegate(this.MessageClear));
                    return;
                }
                this.lstMessage.Items.Clear();
                this.lstMessage.Refresh();
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージの追加
        /// </summary>
        /// <param name="message"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void MessageAdd(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MessageAddDelegate(this.MessageAdd), new Object[] { message });
                    return;
                }
                this.lstMessage.Items.Insert(0, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + message);
                this.WriteLog(message);
                for (int i = this.lstMessage.Items.Count - 1; 999 < i; i--)
                {
                    this.lstMessage.Items.RemoveAt(i);
                }
                this.lstMessage.Refresh();
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        #endregion

        #region ラジオボタン関係

        /// --------------------------------------------------
        /// <summary>
        /// 実行ラジオボタンの値変更
        /// </summary>
        /// <param name="value"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void ChangeStartChecked(bool value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ChangeStartCheckedDelegate(this.ChangeStartChecked), new Object[] { value });
                    return;
                }
                if (this.rdoStart.Checked == value) return;
                this.rdoStart.Checked = value;
                this.rdoStop.Checked = !this.rdoStart.Checked;
            }
            catch (Exception ex)
            {
                this.WriteLog(ex);
            }
        }

        #endregion

        #region コントロールイベント関係

        /// --------------------------------------------------
        /// <summary>
        /// 設定ボタンクリック
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void OnSettingClick(EventArgs e)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// クリアボタンクリック
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void OnClearClick(EventArgs e)
        {
            this.MessageClear();
        }

        #endregion

        #region 常駐処理開始、終了

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理開始
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public virtual void MonitorStart()
        {
            if (DesignMode) return;

            if (this._ar != null && !this._ar.IsCompleted)
            {
                this.MonitorStop();
            }
            this.MessageAdd("監視処理開始");
            this._residentState = true;
            this.IsStopRequest = false;
            this._asynCall = new DelegateMonitor(this.AsynchronousMonitor);
            this._ar = this._asynCall.BeginInvoke(null, null);
            this.ChangeStartChecked(true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理終了
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public virtual void MonitorStop()
        {
            if (DesignMode) return;
            // スレッド終了要求
            this.MessageAdd("監視処理終了要求中");
            this.IsStopRequest = true;
            while (this.ResidentState)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(200);
            }
            this._asynCall = null;
            this._ar = null;
        }

        #endregion

        #region 常駐処理関係

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理呼出しスレッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void AsynchronousMonitor()
        {

#if (DEBUG )
            System.Diagnostics.Debug.WriteLine("常駐スレッド開始:" + this.Name);
#endif
            int timeCnt = 0;
            int secondCnt = 0;

            while (!this.IsStopRequest)
            {
                // 経過秒計算
                if (5 <= timeCnt)
                {
                    secondCnt++;
                    timeCnt = 0;
                }

                // 指定時間チェック(秒単位)
                if (this.CheckTime <= secondCnt)
                {
                    secondCnt = 0;
                    try
                    {
#if(DEBUG )
                        System.Diagnostics.Debug.WriteLine("常駐実行　　　　:" + this.Name);
#endif
                        // 常駐処理
                        try
                        {
                            this.MonitorExecute();
                        }
                        catch (Exception ex)
                        {
                            this.WriteLog(ex);
                        }

                        // 終了要求確認
                        if (this.IsStopRequest)
                        {
                            break;
                        }
                    }
#if( DEBUG )
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }
#else
                    catch (Exception)
                    {
                    }
#endif
                }

                // 200ミリ秒待機
                Thread.Sleep(200);
                timeCnt++;

                // 終了要求確認
                if (this.IsStopRequest)
                {
                    break;
                }
            }
#if( DEBUG )
            System.Diagnostics.Debug.WriteLine("常駐スレッド終了:" + this.Name);
#endif
            this.IsStopRequest = false;
            this._residentState = false;
            this.ChangeStartChecked(false);
            this._asynCall = null;
            this._ar = null;

        }
        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void MonitorExecute()
        {
#if( DEBUG )
            System.Diagnostics.Debug.WriteLine("MonitorExecute　:" + this.Name);
#endif
        }

        #endregion

        #region ログ出力

        #region ログ設定

        /// --------------------------------------------------
        /// <summary>
        /// ログ設定
        /// </summary>
        /// <create>K.Tsutsumi 2011/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void LogSettings()
        {
            if (this._log != null)
            {
                return;
            }
            this._log = new UtilLog();

            this._log.Logger.Setting.LogPutPath = this.GetLogPutPath();
            this._log.Logger.Setting.LogBaseName = this.LogFileName;
            this._log.LogPutMode = LoggerPutMode.TextLogFile;

#if(DEBUG)
            this._log.Logger.Setting.LogPutPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            this._log.Logger.Setting.LogLifeCycle = 1;
#endif
        }

        /// --------------------------------------------------
        /// <summary>
        /// ログの設定パス取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/21</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetLogPutPath()
        {
            return ComDefine.LOG_PUT_PATH;
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void WriteLog(string message)
        {
            this._log.WriteLog(message);
        }

        /// --------------------------------------------------
        /// <summary>
        /// ログ出力
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void WriteLog(Exception ex)
        {
            if (ex == null) return;
            this.LogSettings();
            this._log.WriteLog(ex.Message);
            this._log.WriteLog(ex.StackTrace);
        }

        #endregion

        #endregion

        #region Form Events

        /// --------------------------------------------------
        /// <summary>
        /// ロード時のイベント
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeControl();
        }

        #endregion

        #region Control Events

        /// --------------------------------------------------
        /// <summary>
        /// 設定ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSetting_Click(object sender, EventArgs e)
        {
            this.OnSettingClick(e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// クリアボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.OnClearClick(e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 実行ラジオボタン値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoStart_CheckedChanged(object sender, EventArgs e)
        {
            ChangeStartChecked(this.rdoStart.Checked);
            if (this.rdoStart.Checked != this.ResidentState)
            {
                if (this.rdoStart.Checked)
                {
                    this.MonitorStart();
                }
                else
                {
                    this.MonitorStop();
                }
            }
        }

        #endregion

    }
}
