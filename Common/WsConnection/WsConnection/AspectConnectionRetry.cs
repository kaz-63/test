using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Web.Services.Protocols;

using DSWUtil.AOP;
using DSWUtil.Log;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// WsConnectionのリトライ用割り込み処理
    /// </summary>
    /// <create>Y.Higuchi 2010/11/18</create>
    /// <update></update>
    /// --------------------------------------------------
    class AspectConnectionRetry : AspectBaseProxy
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 接続のリトライ回数
        /// </summary>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CONNECT_RETRY_CNT = 20;

        /// --------------------------------------------------
        /// <summary>
        /// WAIT時間(ミリ秒)
        /// </summary>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int WAIT_TIME = 500;

        /// --------------------------------------------------
        /// <summary>
        /// WAIT_TIMEを何度繰り返すか
        /// </summary>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int WAIT_COUNT = 2;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// デバッグ時に使用するログ
        /// </summary>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private UtilLog _log = null;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">対象のオブジェクト</param>
        /// <param name="classToProxy">型情報</param>
        /// <param name="isAllAspectExecute">AspectExecute属性がない場合に割込み処理を行うかどうかのフラグ</param>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public AspectConnectionRetry(MarshalByRefObject target, Type classToProxy, bool isAllAspectExecute)
            : base(target, classToProxy, isAllAspectExecute)
        {
#if(DEBUG)
            this._log = new UtilLog(LoggerPutMode.TextLogFile);
            this._log.Logger.Setting.LogLifeCycle = 1;
            this._log.Logger.Setting.LogPutPath = @"C:\temp\";
#endif
        }

        #endregion

        #region メソッド呼び出し時に割込み処理を行う

        /// --------------------------------------------------
        /// <summary>
        /// メソッド呼び出し時に割込み処理を行う
        /// </summary>
        /// <param name="ctorMsg">オブジェクトの構築呼び出し要求</param>
        /// <param name="call">メソッド呼び出しメッセージのインターフェイス</param>
        /// <returns>IMessage</returns>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override IMessage InvokeAspect(IConstructionCallMessage ctorMsg, IMethodCallMessage call)
        {
            IMessage ret = null;

            for (int i = 0; i < 10; i++)
            {
                // メソッド呼び出し時に割込み処理
                ret = base.InvokeAspect(ctorMsg, call);

                // 例外があったかチェック
                IMethodReturnMessage mrm = ret as IMethodReturnMessage;
                if(mrm != null)
                {
                    SoapException se = GetSoapException(mrm.Exception);
                    // SoapExceptionの場合リトライ
                    if (se != null)
                    {
                        this.DebugLog("===== SoapExceptionHandling 開始 =====");
#if(DEBUG)
                        string methodName = string.Empty;
                        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                        if (1 < st.FrameCount)
                        {
                            System.Diagnostics.StackFrame sf = st.GetFrame(1);
                            methodName = sf.GetMethod().ReflectedType.FullName + "." + sf.GetMethod().Name;
                            this.DebugLog("メソッド名:" + methodName);
                        }
#endif
                        this.DebugLog("SoapException.Message:" + se.Message);
                        this.DebugLog("SoapException.StackTrace:" + se.StackTrace);
                        this.DebugLog("===== SoapExceptionHandling 終了 =====");

                        // Wait処理
                        for (int wait = 0; wait < WAIT_COUNT; wait++)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            System.Threading.Thread.Sleep(WAIT_TIME);
                        }
                        // リトライ
                        continue;
                    }
                }
                break;
            }

            return ret;
        }

        #endregion

        #region SoapException取得

        /// --------------------------------------------------
        /// <summary>
        /// SoapException取得
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>SoapException</returns>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private SoapException GetSoapException(Exception ex)
        {
            try
            {
                SoapException ret = ex as SoapException;
                if (ret == null && ex.InnerException != null)
                {
                    ret = this.GetSoapException(ex.InnerException);
                }
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region デバッグ用ログ出力

        /// --------------------------------------------------
        /// <summary>
        /// デバッグ用ログ出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <create>Y.Higuchi 2010/11/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void DebugLog(string message)
        {
            if (this._log == null) return;
#if(DEBUG)
            this._log.WriteLog(message);
#endif
        }

        #endregion
    }
}
