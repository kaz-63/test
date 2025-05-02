using System;
using System.IO;
using System.Windows.Forms;

using DSWUtil;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// 設定ファイルアクセス用クラス(Singleton)
    /// DSWUtil.UtilIniFileが少しなんだかなぁなので拡張する為のクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public class LocalSetting
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private static LocalSetting _instance = null;
        /// --------------------------------------------------
        /// <summary>
        /// DSWUtil.UtilIniFileインスタンス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private UtilIniFile _ini = null;
        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイルのフルパス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _iniFilePath = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// ロック用
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private System.Threading.ReaderWriterLock _rwLock = new System.Threading.ReaderWriterLock();

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private LocalSetting()
        {
            this._ini = new UtilIniFile();
            string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ComDefine.INI_DIRNAME);
            string fileName = ComDefine.INI_FILENAME;
            this._iniFilePath = Path.Combine(dirPath, fileName);
            this._ini.IniFileName = this._iniFilePath;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// インスタンスプロパティ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private static LocalSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 初回アクセス時インスタンス化
                    _instance = new LocalSetting();
                }
                return _instance;
            }
        }

        #endregion

        #region 設定ファイルアクセス処理

        #region チェック処理

        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイル存在チェック(存在しない時は作成)
        /// </summary>
        /// <returns>true:存在/false:存在しない</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool CheckIniFile()
        {
            if (File.Exists(this._iniFilePath)) return true;
            try
            {
                // ライタロック取得
                this._rwLock.AcquireWriterLock(1000);
                try
                {
                    if (!Directory.Exists(this._ini.IniDirectoryName))
                    {
                        Directory.CreateDirectory(this._ini.IniDirectoryName);
                    }
                    File.Create(this._iniFilePath);
                }
                finally
                {
                    // ライタロック解放
                    this._rwLock.ReleaseWriterLock();
                }
            }
            catch (Exception) { }
            return true;
        }

        #endregion

        #region 読み込み処理

        /// --------------------------------------------------
        /// <summary>
        /// セクション、キーから設定ファイルから値を読み込む
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>設定値</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ReadProfile(string sectionName, string keyName, string defaultValue)
        {
            if (!this.CheckIniFile()) return defaultValue;
            string ret = defaultValue;
            try
            {
                // リーダーロック取得
                this._rwLock.AcquireReaderLock(1000);
                try
                {
                    ret = this._ini.GetProfileString(sectionName, keyName, defaultValue.ToString());
                }
                finally
                {
                    // リーダーロック解放
                    this._rwLock.ReleaseReaderLock();
                }
            }
            catch (Exception) { }
            return ret;
        }

        #endregion

        #region 書き込み処理

        /// --------------------------------------------------
        /// <summary>
        /// セクション、キーで値を設定ファイルに書き込む
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="value">設定値</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool WriteProfile(string sectionName, string keyName, string value)
        {
            if (!this.CheckIniFile()) return false;
            try
            {
                bool ret = false;
                // ライタロック取得
                this._rwLock.AcquireWriterLock(1000);
                try
                {
                    this._ini.PutProfile(sectionName, keyName, value);
                    ret = true;
                }
                finally
                {
                    // ライタロック解放
                    this._rwLock.ReleaseWriterLock();
                }
                return ret;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region 外部アクセス用 - 共通メソッド

        /// --------------------------------------------------
        /// <summary>
        /// セクション、キーから設定ファイルから値を読み込む
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>設定値</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetProfile(string sectionName, string keyName, string defaultValue)
        {
            return Instance.ReadProfile(sectionName, keyName, defaultValue);
        }

        /// --------------------------------------------------
        /// <summary>
        /// セクション、キーで値を設定ファイルに書き込む
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="value">設定値</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool SetProfile(string sectionName, string keyName, string value)
        {
            return Instance.WriteProfile(sectionName, keyName, value);
        }

        #endregion

        #region 外部アクセス用 - プリンタ

        #region 通常使用するプリンタ

        /// --------------------------------------------------
        /// <summary>
        /// 通常使用するプリンタ取得
        /// </summary>
        /// <returns>通常使用するプリンタ(プリンタが存在しない場合はWindowsの通常使うプリンタを返す。)</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetNormalPrinter()
        {
            try
            {
                // PrintDocumentの作成
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                // プリンタ名の取得
                string defaultValue = pd.PrinterSettings.PrinterName;
                string printerName = GetProfile(ComDefine.SEC_PRINTER, ComDefine.KEY_PRINTER_NORMAL, defaultValue);
                // プリンタがインストールされているかチェック
                foreach (string prin in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (printerName == prin)
                    {
                        return printerName;
                    }
                }
                return defaultValue;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 通常使用するプリンタ設定
        /// </summary>
        /// <param name="printerName">設定するプリンタ名</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool SetNormalPrinter(string printerName)
        {
            return SetProfile(ComDefine.SEC_PRINTER, ComDefine.KEY_PRINTER_NORMAL, printerName);
        }

        #endregion

        #region 現品TAGに使用するプリンタ

        /// --------------------------------------------------
        /// <summary>
        /// 現品TAGに使用するプリンタ取得
        /// </summary>
        /// <returns>現品TAGに使用するプリンタ(プリンタが存在しない場合はWindowsの通常使うプリンタを返す。)</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string GetTAGPrinter()
        {
            try
            {
                // PrintDocumentの作成
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                // プリンタ名の取得
                string defaultValue = pd.PrinterSettings.PrinterName;
                string printerName = GetProfile(ComDefine.SEC_PRINTER, ComDefine.KEY_PRINTER_TAG, defaultValue);
                // プリンタがインストールされているかチェック
                foreach (string prin in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (printerName == prin)
                    {
                        return printerName;
                    }
                }
                return defaultValue;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 現品TAGに使用するプリンタ設定
        /// </summary>
        /// <param name="printerName">プリンタ名</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool SetTAGPrinter(string printerName)
        {
            return SetProfile(ComDefine.SEC_PRINTER, ComDefine.KEY_PRINTER_TAG, printerName);
        }

        #endregion

        #endregion

    }
}
