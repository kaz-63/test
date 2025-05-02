using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Commons;
using DSWUtil.Log;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// コネクションベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    [DSWUtil.AOP.AspectClass(typeof(AspectConnectionRetry), true)]
    public class ConnBase : ContextBoundObject
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// タイムアウト時間
        /// </summary>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int WS_TIMEOUT = 600000;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        [DSWUtil.AOP.AspectExecute(false)]
        public ConnBase()
        {
        }

        #endregion

        #region インスタンス取得

        /// --------------------------------------------------
        /// <summary>
        /// WsA01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefA01.WsA01 GetWsA01()
        {
            WebRefA01.WsA01 ws = new WsConnection.WebRefA01.WsA01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsCommonのインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefCommon.WsCommon GetWsCommon()
        {
            WebRefCommon.WsCommon ws = new WsConnection.WebRefCommon.WsCommon();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsI01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>T.Wakamatsu 2013/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefI01.WsI01 GetWsI01()
        {
            WebRefI01.WsI01 ws = new WsConnection.WebRefI01.WsI01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsI02のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>T.Wakamatsu 2013/08/23</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefI02.WsI02 GetWsI02()
        {
            WebRefI02.WsI02 ws = new WsConnection.WebRefI02.WsI02();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsJ01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefJ01.WsJ01 GetWsJ01()
        {
            WebRefJ01.WsJ01 ws = new WsConnection.WebRefJ01.WsJ01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsK01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefK01.WsK01 GetWsK01()
        {
            WebRefK01.WsK01 ws = new WsConnection.WebRefK01.WsK01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsK02のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefK02.WsK02 GetWsK02()
        {
            WebRefK02.WsK02 ws = new WsConnection.WebRefK02.WsK02();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsK03のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefK03.WsK03 GetWsK03()
        {
            WebRefK03.WsK03 ws = new WsConnection.WebRefK03.WsK03();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsK04のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefK04.WsK04 GetWsK04()
        {
            WebRefK04.WsK04 ws = new WsConnection.WebRefK04.WsK04();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsM01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefM01.WsM01 GetWsM01()
        {
            WebRefM01.WsM01 ws = new WsConnection.WebRefM01.WsM01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsMasterのインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefMaster.WsMaster GetWsMaster()
        {
            WebRefMaster.WsMaster ws = new WsConnection.WebRefMaster.WsMaster();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsP01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefP01.WsP01 GetWsP01()
        {
            WebRefP01.WsP01 ws = new WsConnection.WebRefP01.WsP01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsP02のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefP02.WsP02 GetWsP02()
        {
            WebRefP02.WsP02 ws = new WsConnection.WebRefP02.WsP02();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsS01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefS01.WsS01 GetWsS01()
        {
            WebRefS01.WsS01 ws = new WsConnection.WebRefS01.WsS01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsS02のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefS02.WsS02 GetWsS02()
        {
            WebRefS02.WsS02 ws = new WsConnection.WebRefS02.WsS02();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsSmsのインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefSms.WsSms GetWsSms()
        {
            WebRefSms.WsSms ws = new WsConnection.WebRefSms.WsSms();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsT01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>S.Furugo 2018/10/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefT01.WsT01 GetWsT01()
        {
            WebRefT01.WsT01 ws = new WsConnection.WebRefT01.WsT01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsU01のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefU01.WsU01 GetWsU01()
        {
            WebRefU01.WsU01 ws = new WsConnection.WebRefU01.WsU01();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsAttachFileのインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        /// <create>Y.Higuchi 2010/07/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefAttachFile.WsAttachFileClient GetWsAttachFileClient()
        {
            // WCFサービスなのでTimeoutプロパティがない
            WebRefAttachFile.WsAttachFileClient ws = new WsConnection.WebRefAttachFile.WsAttachFileClient();
#if(!DEBUG)
            string domain;
            string userName;
            string password;
            ComFunc.GetClientCredential(out domain, out userName, out password);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                ws.ClientCredentials.Windows.ClientCredential.Domain = domain;
                ws.ClientCredentials.Windows.ClientCredential.UserName = userName;
                ws.ClientCredentials.Windows.ClientCredential.Password = password;
            }
#endif
            return ws;
        }

        /// --------------------------------------------------
        /// <summary>
        /// WsZ99のインスタンス取得
        /// </summary>
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public WebRefZ99.WsZ99 GetWsZ99()
        {
            WebRefZ99.WsZ99 ws = new WsConnection.WebRefZ99.WsZ99();
            ws.Timeout = WS_TIMEOUT;
            return ws;
        }

        #endregion

    }
}
