using System;
using System.IO;
using System.Text;

using WsConnection.WebRefAttachFile;

namespace WsConnection
{
    /// --------------------------------------------------
    /// <summary>
    /// ファイルアップロード、ダウンロードコネクションクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class ConnAttachFile : ConnBase
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public ConnAttachFile()
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="language">言語設定</param>
        /// <create>M.Shimizu 2020/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public ConnAttachFile(string language)
        {
            this.Language = language;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 言語設定
        /// </summary>
        /// <create>M.Shimizu 2020/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Language { get; set; }

        #endregion

        #region AR Helpファイルダウンロード

        /// --------------------------------------------------
        /// <summary>
        /// AR Helpファイルダウンロード
        /// </summary>
        /// <param name="lang">言語</param>
        /// <returns>ARHelpファイルダウンロード用戻り値クラス</returns>
        /// <create>Y.Higuchi 2010/07/05</create>
        /// <update>D.Okumura 2019/12/04 HELP多言語化対応</update>
        /// --------------------------------------------------
        public ARHelpDownloadResult ARHelpDownload(string lang)
        {
            try
            {
                using (WsAttachFileClient ws = this.GetWsAttachFileClient())
                {
                    try
                    {
                        WsConnection.WebRefAttachFile.ARHelpDownloadResult ret = ws.ARHelpDownload(lang);
                        ws.Close();
                        return ret;
                    }
                    catch (Exception exWs)
                    {
                        ws.Abort();
                        throw new Exception(exWs.Message, exWs);
                    }
                }
            }
            catch (Exception ex)
            {
#if(DEBUG)
                return null;
#endif
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 単一ファイル

        /// --------------------------------------------------
        /// <summary>
        /// 単一ファイルのデリート用メソッド
        /// </summary>
        /// <param name="package">単一ファイルデリート用引数クラス</param>
        /// <returns>単一ファイルデリート用戻り値クラス</returns>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public FileDeleteResult FileDelete(FileDeletePackage package)
        {
            try
            {
                using (WsAttachFileClient ws = this.GetWsAttachFileClient())
                {
                    try
                    {
                        WsConnection.WebRefAttachFile.FileDeleteResult ret = ws.FileDelete(package);
                        ws.Close();
                        return ret;
                    }
                    catch (Exception exWs)
                    {
                        ws.Abort();
                        throw new Exception(exWs.Message, exWs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 単一ファイルのアップロード用メソッド
        /// </summary>
        /// <param name="package">単一ファイルアップロード用引数クラス</param>
        /// <returns>単一ファイルアップロード用戻り値クラス</returns>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public FileUploadResult FileUpload(FileUploadPackage package)
        {
            try
            {
                using (WsAttachFileClient ws = this.GetWsAttachFileClient())
                {
                    try
                    {
                        WsConnection.WebRefAttachFile.FileUploadResult ret = ws.FileUpload(package);
                        ws.Close();
                        return ret;
                    }
                    catch (Exception exWs)
                    {
                        ws.Abort();
                        throw new Exception(exWs.Message, exWs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 単一ファイルのダウンロード用メソッド
        /// </summary>
        /// <param name="package">単一ファイルダウンロード用引数クラス</param>
        /// <returns>単一ファイルダウンロード用戻り値クラス</returns>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public FileDownloadResult FileDownload(FileDownloadPackage package)
        {
            try
            {
                using (WsAttachFileClient ws = this.GetWsAttachFileClient())
                {
                    try
                    {
                        WsConnection.WebRefAttachFile.FileDownloadResult ret = ws.FileDownload(package);
                        ws.Close();
                        return ret;
                    }
                    catch (Exception exWs)
                    {
                        ws.Abort();
                        throw new Exception(exWs.Message, exWs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ファイルの内容を取得

        /// --------------------------------------------------
        /// <summary>
        /// ファイルをダウンロードし、内容を取得します
        /// </summary>
        /// <param name="folderName">フォルダ名</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="fileType">ファイルタイプ</param>
        /// <returns>ファイルの内容</returns>
        /// <create>M.Shimizu 2020/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetFileContents(string folderName, string fileName, FileType fileType)
        {
            // 設定
            FileDownloadPackage package = new FileDownloadPackage()
            {
                FolderName = folderName,
                FileName = fileName,
                FileType = fileType
            };

            // ファイルのダウンロード
            FileDownloadResult result = FileDownload(package);
            
            // ファイルが取得できたら内容を返し、取得できなかったら空文字を返す。
            if (result.IsExistsFile)
            {
                using (MemoryStream stream = new MemoryStream(result.FileData, false))
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メールテンプレートを取得します
        /// </summary>
        /// <param name="language">言語設定</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>メールテンプレート</returns>
        /// <create>M.Shimizu 2020/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetMailTemplate(string fileName)
        {
            if (string.IsNullOrEmpty(this.Language))
            {
                this.Language = string.Empty;
            }

            return GetFileContents(this.Language, fileName, FileType.MailTemplate);
        }

        #endregion
    }
}
