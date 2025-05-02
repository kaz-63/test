using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;

using Condition;
using DSWUtil.DbUtil;
using Commons;

//TODO: web.config のデバッグ設定を true にしたので、リリース時に注意する。

/// --------------------------------------------------
/// <summary>
/// Webサービス トランザクション層のベースクラス
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
public class WsBase : System.Web.Services.WebService
{
    #region Fields

    /// --------------------------------------------------
    /// <summary>
    /// 暗号鍵
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private readonly byte[] _rgbKey = Encoding.UTF8.GetBytes("gB0'9e3#KpPpPWQwvRh(>DCxIw|Oq?><");

    /// --------------------------------------------------
    /// <summary>
    /// 初期ベクター
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private readonly byte[] _rgbIV = Encoding.UTF8.GetBytes("A9b'!Hn}1^@jwQMK");

    private string _connStr = string.Empty;

    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsBase()
    {
        this.Server.ScriptTimeout = 600;
    }

    #endregion

    #region Properties

    /// --------------------------------------------------
    /// <summary>
    /// 接続文字列
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected string ConnStr
    {
        get
        {
            if (string.IsNullOrEmpty(this._connStr))
            {
                this._connStr = this.GetConnStr();
            }
            return this._connStr;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// データベース接続文字列の設定名
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual string ConnStrSettingName
    {
        get
        {
            return "ConnectionString";
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// プロバイダ名
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual string DbProviderName
    {
        get
        {
            return WebConfigurationManager.AppSettings["DbProviderName"].ToString();
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 暗号鍵
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual byte[] RgbKey
    {
        get
        {
            return this._rgbKey;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 初期ベクター
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual byte[] RgbIV
    {
        get
        {
            return this._rgbIV;
        }
    }

    #endregion

    #region 複合化

    /// --------------------------------------------------
    /// <summary>
    /// 暗号化された文字列を複合化する。
    /// </summary>
    /// <param name="decryptingString">暗号化文字列</param>
    /// <returns>複合化文字列</returns>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected string DecryptByAes(string decryptingString)
    {
        try
        {
            string decryptedString = string.Empty;
            byte[] decryptingBytes = Convert.FromBase64String(decryptingString);
            RijndaelManaged aes = new RijndaelManaged();
            using (MemoryStream memStrm = new MemoryStream())
            using (CryptoStream cryptoStrm = new CryptoStream(memStrm, aes.CreateDecryptor(this.RgbKey, this.RgbIV), CryptoStreamMode.Write))
            {
                cryptoStrm.Write(decryptingBytes, 0, decryptingBytes.Length);
                cryptoStrm.FlushFinalBlock();
                byte[] decriptedBytes = memStrm.ToArray();
                decryptedString = Encoding.UTF8.GetString(decriptedBytes);
            }
            return decryptedString;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    #endregion

    #region 接続文字列取得

    /// --------------------------------------------------
    /// <summary>
    /// 接続文字列取得
    /// </summary>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private string GetConnStr()
    {
        string connStr = string.Empty;

        string decryptingString = WebConfigurationManager.AppSettings[this.ConnStrSettingName].ToString();
        connStr = this.DecryptByAes(decryptingString);
#if(DEBUG)
        try
        {
            string path = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "setting.xml");
            DataTable dt = new DataTable();
            if (File.Exists(path))
            {
                dt.ReadXml(path);
                string debugConnStr = ComFunc.GetFld(dt, 0, "ConnectionString", connStr);
                if (!string.IsNullOrEmpty(debugConnStr.Trim()))
                {
                    connStr = debugConnStr;
                }
            }
            else
            {
                dt.Columns.Add("ConnectionString");
                dt.Columns.Add("BackupConnectionString");
                dt.Rows.Add("", "");
                dt.TableName = "Setting";
                dt.WriteXml(path, XmlWriteMode.WriteSchema);
            }
        }
        catch (Exception) { }
#endif
        return connStr;
    }

    #endregion

    #region DatabaseHelper

    public DatabaseHelper GetDatabaseHelper()
    {
        DatabaseHelper dbHelper = new DatabaseHelper(this.DbProviderName, this.ConnStr);
#if(DEBUG)
        dbHelper.LogPutMode = DSWUtil.Log.LoggerPutMode.TextLogFile;
        dbHelper.LogSetting.LogBaseName = "SQLExecuteLog";
        dbHelper.LogSetting.LogPutPath = @"c:\temp\";
        dbHelper.LogSetting.LogLifeCycle = 1;
#endif
        return dbHelper;
    }

    #endregion

    #region CondBase定義用

    /// --------------------------------------------------
    /// <summary>
    /// 各WebサービスのCondBaseをクライアントで使用できるように定義する為のメソッド
    /// </summary>
    /// <param name="cond"></param>
    /// <create>Y.Higuchi 2010/06/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [WebMethod(Description = "各WebサービスのCondBaseをクライアントで使用できるように定義する為のメソッド")]
    public void SendCondBase(CondBase cond)
    {
        // 何もしない。
    }


    #endregion

    #region DBオープン

    /// --------------------------------------------------
    /// <summary>
    /// DBオープン
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <create>Y.Higuchi 2010/10/28</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual void DbOpen(DatabaseHelper dbHelper)
    {
        try
        {
            bool isRetry = false;
            int retryCnt = 0;
            do
            {
                try
                {
                    isRetry = false;
                    dbHelper.Open();
                }
                catch (Exception)
                {
                    // 2010/11/18 リトライしても改善しなかったのでSOAP通信自体をリトライするようにしたのでリトライ数を減らす。
                    if (retryCnt <= ComDefine.DB_OPEN_RETRY_COUNT )
                    {
                        isRetry = true;
                    }
                    else
                    {
                        throw;
                    }
                    retryCnt++;
                }
            } while (isRetry);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion
}
