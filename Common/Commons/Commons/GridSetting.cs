using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

using DSWUtil;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// グリッド設定ファイルアクセス用クラス(Singleton)
    /// </summary>
    /// <create>H.Tsuji 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public class GridSetting
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private static GridSetting _instance = null;
        /// --------------------------------------------------
        /// <summary>
        /// DSWUtil.UtilIniFileインスタンス
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private UtilIniFile _ini = null;
        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイルのフルパス
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _iniFilePath = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// ロック用
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private System.Threading.ReaderWriterLock _rwLock = new System.Threading.ReaderWriterLock();

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private GridSetting()
        {
            this._ini = new UtilIniFile();
            string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ComDefine.INI_DIRNAME);
            string fileName = ComDefine.INI_GRID_FILENAME;
            this._iniFilePath = Path.Combine(dirPath, fileName);
            this._ini.IniFileName = this._iniFilePath;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// インスタンスプロパティ
        /// </summary>
        /// <create>H.Tsuji 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private static GridSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 初回アクセス時インスタンス化
                    _instance = new GridSetting();
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
        /// <create>H.Tsuji 2019/07/30</create>
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
        /// 設定ファイルから指定したセクションを読み込む
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <returns>データテーブル</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable ReadSection(string section)
        {
            DataTable ret = new DataTable();
            if (!this.CheckIniFile()) return ret;
            try
            {
                // リーダーロック取得
                this._rwLock.AcquireReaderLock(1000);
                try
                {
                    ret = this._ini.LoadSection(section);
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
        /// 設定ファイルに指定したセクションを書き込む
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="dt">データテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool WriteSection(string section, DataTable dt)
        {
            if (!this.CheckIniFile()) return false;
            try
            {
                bool ret = false;
                // ライタロック取得
                this._rwLock.AcquireWriterLock(1000);
                try
                {
                    this._ini.SaveSection(section, dt);
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
        /// 設定ファイルから指定したセクションの内容を読み込む
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <returns>データテーブル</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static DataTable LoadSection(string section)
        {
            return Instance.ReadSection(section);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 設定ファイルに指定したセクションの内容を書き込む
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="dt">データテーブル</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tsuji 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool SaveSection(string section, DataTable dt)
        {
            return Instance.WriteSection(section, dt);
        }

        #endregion
    }
}
