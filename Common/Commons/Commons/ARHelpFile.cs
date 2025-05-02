using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// AR Helpファイルクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ARHelpFile
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private static ARHelpFile _instance = null;
        /// --------------------------------------------------
        /// <summary>
        /// Helpフォルダパス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _helpDirPath = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// ロック用
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private System.Threading.ReaderWriterLock _rwLock = new System.Threading.ReaderWriterLock();

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private ARHelpFile()
        {
            this._helpDirPath = Path.Combine(Application.StartupPath, ComDefine.AR_HELP_DIR_NAME);
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// インスタンスプロパティ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private static ARHelpFile Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 初回アクセス時インスタンス化
                    _instance = new ARHelpFile();
                }
                return _instance;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Helpフォルダパス
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public string HelpDirPath
        {
            get { return this._helpDirPath; }
        }

        #endregion

        #region ファイルアクセス処理

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// AR Helpフォルダ初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public void InnerInitializeHelpDir()
        {
            try
            {
                // ライタロック取得
                this._rwLock.AcquireWriterLock(1000);
                try
                {
                    // フォルダの存在チェック
                    if (!Directory.Exists(this.HelpDirPath))
                    {
                        Directory.CreateDirectory(this.HelpDirPath);
                    }
                    foreach (string fileName in Directory.GetFiles(this.HelpDirPath, ComDefine.AR_HELP_FILE_DELETE_PATTERN))
                    {
                        File.Delete(fileName);
                    }
                }
                finally
                {
                    // ライタロック解放
                    this._rwLock.ReleaseWriterLock();
                }
            }
            catch (Exception) { }
        }

        #endregion

        #region 読み込み処理

        /// --------------------------------------------------
        /// <summary>
        /// AR Help画像ファイル読込処理
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>Bitmap</returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public Bitmap InnerGetImageFile(string fileName)
        {
            try
            {
                // リーダーロック取得
                this._rwLock.AcquireReaderLock(1000);
                try
                {
                    string path = Path.Combine(this.HelpDirPath, fileName);
                    if (!File.Exists(path)) return null;
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (System.IO.Stream strm = new System.IO.MemoryStream())
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                        fs.Close();
                        strm.Write(data, 0, data.Length);
                        return new Bitmap(strm);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    // リーダーロック解放
                    this._rwLock.ReleaseReaderLock();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 保存処理

        /// --------------------------------------------------
        /// <summary>
        /// AR Help画像ファイル保存処理
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="imageData">画像データ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool InnerSaveImageFile(string fileName, byte[] imageData)
        {
            try
            {
                // ライタロック取得
                this._rwLock.AcquireWriterLock(1000);
                try
                {
                    string path = Path.Combine(this.HelpDirPath, fileName);
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(imageData, 0, imageData.Length);
                        fs.Close();
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    // ライタロック解放
                    this._rwLock.ReleaseWriterLock();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region 外部アクセス用

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// AR Helpフォルダ初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void InitializeHelpDir()
        {
            Instance.InnerInitializeHelpDir();
        }

        #endregion

        #region 読み込み処理

        /// --------------------------------------------------
        /// <summary>
        /// AR Listヘルプファイル取得
        /// </summary>
        /// <returns>Bitmap</returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static Bitmap GetARList()
        {
            Bitmap bmp = Instance.InnerGetImageFile(ComDefine.AR_LIST_HELP_FILE_NAME_1);
            if (bmp == null)
            {
                bmp = Instance.InnerGetImageFile(ComDefine.AR_LIST_HELP_FILE_NAME_2);
            }
            return bmp;
        }

        /// --------------------------------------------------
        /// <summary>
        /// AR情報登録画面ヘルプファイル取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static Bitmap GetARTouroku()
        {
            Bitmap bmp = Instance.InnerGetImageFile(ComDefine.AR_TOROKU_HELP_FILE_NAME_1);
            if (bmp == null)
            {
                bmp = Instance.InnerGetImageFile(ComDefine.AR_TOROKU_HELP_FILE_NAME_2);
            }
            return bmp;
        }

        #endregion

        #region 保存処理

        /// --------------------------------------------------
        /// <summary>
        /// ARヘルプファイル保存
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="imageData">画像データ</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool SaveARHelpFile(string fileName, byte[] imageData)
        {
            return Instance.InnerSaveImageFile(fileName, imageData);
        }

        #endregion

        #endregion

    }
}
