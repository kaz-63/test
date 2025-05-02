using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSWUtil;

using CommLib;
using SMS.K04.Properties;

namespace SMS.K04
{
    /// --------------------------------------------------
    /// <summary>
    /// BT500用クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    public class HandyBT500 : IDisposable
    {
        #region Fields

        private CommClient _client = null;
        private bool _isEndSearch = false;
        private bool _isEndConnect = false;
        private bool _isEndFileFound = false;
        private bool _isEndDisconnect = false;
        private bool _isEndProcedureCompleted = false;
        private BTCOMM_RESULT _resultProcedureCompleted = BTCOMM_RESULT.BTCOMM_OTHER;
        private string _cradleType = string.Empty;
        private int _portNo = 0;
        private int _handyNum = 0;
        private string _errorString = string.Empty;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyBT500()
        {
        }

        #endregion

        #region Properties

        #region Client
        
        /// --------------------------------------------------
        /// <summary>
        /// クライアント
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private CommClient Client
        {
            get 
            {
                if (this._client == null)
                {
                    this._client = new CommClient();
                    this._client.ShowDialog = false;
                    this._client.OnSearched += new SearchedEventHandler(_client_OnSearched);
                    this._client.OnConnected += new ConnectedEventHandler(_client_OnConnected);
                    this._client.OnDisconnected += new DisconnectedEventHandler(_client_OnDisconnected);
                    this._client.OnFileFound += new FileFoundEventHandler(_client_OnFileFound);
                    this._client.OnProcedureCompleted += new ProcedureCompletedEventHandler(_client_OnProcedureCompleted);
                }
                return this._client; 
            }
        }
        
        #endregion

        #region CradleType
        
        /// --------------------------------------------------
        /// <summary>
        /// クレードルタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CradleType
        {
            get { return this._cradleType; }
            set { this._cradleType = value; }
        }

        #endregion

        #region PortNo
        
        /// --------------------------------------------------
        /// <summary>
        /// ポート番号
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public int PortNo
        {
            get { return this._portNo; }
            set { this._portNo = value; }
        }

        #endregion

        #region ErrorString

        /// --------------------------------------------------
        /// <summary>
        /// エラー文字列
        /// </summary>
        /// <create>Y.Higuchi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ErrorString
        {
            get { return this._errorString; }
            set { this._errorString = value; }
        }

        #endregion

        #endregion

        #region Events

        #region OnConnected

        /// --------------------------------------------------
        /// <summary>
        /// 接続完了
        /// </summary>
        /// <param name="result"></param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        void _client_OnConnected(object sender, CommEventArgs e)
        {
            this._isEndConnect = true;
            if (e.result != BTCOMM_RESULT.BTCOMM_OK)
            {
                this.Client.Connect = false;
                return;
            }
        }

        #endregion

        #region OnDisconnected
        
        /// --------------------------------------------------
        /// <summary>
        /// 切断完了
        /// </summary>
        /// <param name="result"></param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        void _client_OnDisconnected(object sender, CommEventArgs e)
        {
            _isEndDisconnect = true;
            this.ErrorString = this.GetErrorString(e.result);
        }

        #endregion

        #region OnFileFound
        
        /// --------------------------------------------------
        /// <summary>
        /// ファイル検索完了
        /// </summary>
        /// <param name="num"></param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        void _client_OnFileFound(object sender, CommNumEventArgs e)
        {
            this._isEndFileFound = true;
        }

        #endregion

        #region OnProcedureCompleted
        
        /// --------------------------------------------------
        /// <summary>
        /// 処理完了
        /// </summary>
        /// <param name="result"></param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        void _client_OnProcedureCompleted(object sender, CommEventArgs e)
        {
            this._resultProcedureCompleted = e.result;
            this._isEndProcedureCompleted = true;
            this.ErrorString = this.GetErrorString(e.result);
        }

        #endregion

        #region OnSearched
        
        /// --------------------------------------------------
        /// <summary>
        /// BT-500 の検索完了
        /// </summary>
        /// <param name="num"></param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        void _client_OnSearched(object sender, CommNumEventArgs e)
        {
            this._isEndSearch = true;
            this._handyNum = e.num;
        }

        #endregion

        #endregion

        #region ハンディの検索

        /// --------------------------------------------------
        /// <summary>
        /// ハンディの検索
        /// </summary>
        /// <returns>ハンディ数</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public int SearchHandy()
        {
            try
            {
                this._isEndSearch = false;
                this.Client.SearchHT(Search_type.SEARCH_CRADLE_USB_ALL);
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndSearch);
                return this._handyNum;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディ情報取得

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ情報取得
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public void GetHandy()
        {
            try
            {
                this.CradleType = string.Empty;
                this.PortNo = 0;
                this.Client.GetHTNext();
                this.CradleType = this.Client.CradleType.ToString();
                this.PortNo = this.Client.PortNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディの受信フォルダ設定(接続前に設定する必要がある)

        /// --------------------------------------------------
        /// <summary>
        /// ハンディの受信フォルダ設定(接続前に設定する必要がある)
        /// </summary>
        /// <param name="recvFolder">受信フォルダ</param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetRecvFolder(string recvFolder)
        {
            try
            {
                if (!System.IO.Directory.Exists(recvFolder))
                {
                    System.IO.Directory.CreateDirectory(recvFolder);
                }
                this.Client.RecvFolder = recvFolder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディ接続

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ接続
        /// </summary>
        /// <returns>接続状態</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool Connect()
        {
            try
            {
                this._isEndConnect = false;
                this.Client.Connect = true;
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndConnect);
                return this.Client.Connect;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ接続
        /// </summary>
        /// <param name="recvFolder">受信フォルダ</param>
        /// <returns>接続状態</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool Connect(string recvFolder)
        {
            this.SetRecvFolder(recvFolder);
            return this.Connect();
        }

        #endregion

        #region ハンディ切断

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ切断
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Disconnect()
        {
            try
            {
                this._isEndDisconnect = false;
                if (!this.Client.Connect) return;
                this.Client.Connect = false;
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndDisconnect && this.Client.Connect == true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディ内のファイル名全取得

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ内のファイル名全取得
        /// </summary>
        /// <param name="deviceNo">ドライブ番号(1～4)</param>
        /// <returns>ファイル情報の配列</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>T.SASAYAMA 2023/07/19 引渡ファイル対応</update>
        /// <update></update>
        /// --------------------------------------------------
        public List<HandyFileInfo> GetFileNames(int driveNo)
        {
            try
            {
                long fileSize = 0;
                string fileName = string.Empty;
                DateTime fileStamp = DateTime.Now;
                this._isEndFileFound = false;
                this.Client.FindFile(driveNo);
                List<HandyFileInfo> fileList = new List<HandyFileInfo>();
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndFileFound);
                // ファイルを検索
                while (this.Client.FindFileNext(ref fileSize, ref fileName, ref fileStamp))
                {
                    HandyFileInfo fileInfo = new HandyFileInfo();
                    fileInfo.CradleType = this.CradleType;
                    fileInfo.PortNo = this.PortNo;
                    fileInfo.Drive = UtilString.Substring(fileName, 0, 2);

                    string[] fn = fileName.Substring(2).Split('.');
                    if (fn.Length > 0)
                        fileInfo.FileNameWithoutExt = fn[0].TrimEnd(' ');
                    if (fn.Length > 1)
                        fileInfo.Ext = fn[1].TrimEnd(' ');

                    // 取込みエラーが発生したため引渡ファイルの拡張子を追加
                    if (fileInfo.FileNameWithoutExt == Commons.ComDefine.HANDY_HIKIWATASHI_FILENAME)
                    {
                        fileInfo.Ext = Commons.ComDefine.HANDY_HIKIWATASHIEXT.TrimEnd(' ');
                    }
                    fileList.Add(fileInfo);

                    fileSize = 0;
                    fileName = string.Empty;
                    fileStamp = DateTime.Now;
                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイル受信

        /// --------------------------------------------------
        /// <summary>
        /// ファイル受信
        /// </summary>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool GetFile(HandyFileInfo fileInfo)
        {
            try
            {
                this._isEndProcedureCompleted = false;
                this.Client.GetFile(fileInfo.GetHandyFileName(), fileInfo.GetLocalFileName());
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndProcedureCompleted);
                if (this._resultProcedureCompleted != BTCOMM_RESULT.BTCOMM_OK)
                {
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

        #region ファイル削除

        /// --------------------------------------------------
        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool RemoveFile(HandyFileInfo fileInfo)
        {
            try
            {
                this._isEndProcedureCompleted = false;
                this.Client.RemoveFile(fileInfo.GetHandyFileName());
                // イベントの終了待ち
                do
                {
                    System.Windows.Forms.Application.DoEvents();
                } while (!this._isEndProcedureCompleted);
                if (this._resultProcedureCompleted != BTCOMM_RESULT.BTCOMM_OK)
                {
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

        #region IDisposable メンバ

        /// --------------------------------------------------
        /// <summary>
        /// Dispose
        /// </summary>
        /// <create>Y.Higuchi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        /// <create>Y.Higuchi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (this._client != null))
                {
                    if (this.Client != null)
                    {
                        if (this.Client.Connect)
                        {
                            this.Disconnect();
                        }
                        this._client = null;
                    }
                    GC.SuppressFinalize(this);
                    GC.Collect();
                }
            }
            catch { }
        }

        #endregion

        #region エラー文字列取得

        /// --------------------------------------------------
        /// <summary>
        /// エラー文字列取得
        /// </summary>
        /// <param name="result">BTCOMM_RESULT</param>
        /// <returns>エラー文字列</returns>
        /// <create>Y.Higuchi 2010/09/15</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetErrorString(BTCOMM_RESULT result)
        {
            try
            {
                string ret = result.ToString() + ":";
                switch (result)
                {
                    case BTCOMM_RESULT.BTCOMM_BIGDATA:
                        ret = Resources.HandyBT500_BigData;
                        break;
                    case BTCOMM_RESULT.BTCOMM_CANCELED:
                        ret = Resources.HandyBT500_Canceled;
                        break;
                    case BTCOMM_RESULT.BTCOMM_CONVERT_FAILED:
                        ret = Resources.HandyBT500_ConvertFailed;
                        break;
                    case BTCOMM_RESULT.BTCOMM_FILENOTFOUND:
                        ret = Resources.HandyBT500_FileNotFound;
                        break;
                    case BTCOMM_RESULT.BTCOMM_INCOMPLETE:
                        ret = Resources.HandyBT500_Incomplete;
                        break;
                    case BTCOMM_RESULT.BTCOMM_INUSE:
                        ret = Resources.HandyBT500_Inuse;
                        break;
                    case BTCOMM_RESULT.BTCOMM_INVALID_VALUE:
                        ret = Resources.HandyBT500_InvalidValue;
                        break;
                    case BTCOMM_RESULT.BTCOMM_NETDOWN:
                        ret = Resources.HandyBT500_NetDown;
                        break;
                    case BTCOMM_RESULT.BTCOMM_NOTCONNECT:
                        ret = Resources.HandyBT500_NetConnect;
                        break;
                    case BTCOMM_RESULT.BTCOMM_NOTFOUND:
                        ret += Resources.HandyBT500_NotFound;
                        break;
                    case BTCOMM_RESULT.BTCOMM_NOTINITIALISED:
                        ret += Resources.HandyBT500_NotInitialised;
                        break;
                    case BTCOMM_RESULT.BTCOMM_OK:
                        ret = string.Empty;
                        break;
                    case BTCOMM_RESULT.BTCOMM_OTHER:
                        ret += Resources.HandyBT500_Other;
                        break;
                    case BTCOMM_RESULT.BTCOMM_REFUSED:
                        ret += Resources.HandyBT500_Refused;
                        break;
                    case BTCOMM_RESULT.BTCOMM_TIMEOUT:
                        ret += Resources.HandyBT500_TimeOut;
                        break;
                    case BTCOMM_RESULT.BTCOMM_WOULDBLOCK:
                        ret += Resources.HandyBT500_WouldBlock;
                        break;
                    default:
                        ret = string.Empty;
                        break;
                }
                return ret;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

    }
}
