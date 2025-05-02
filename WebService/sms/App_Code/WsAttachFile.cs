using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Commons;
using DSWUtil;

// メモ: ここでクラス名 "WsAttachFile" を変更する場合は、Web.config で "WsAttachFile" への参照も更新する必要があります。
/// --------------------------------------------------
/// <summary>
/// ファイルアップロード、ダウンロード用クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/05</create>
/// <update></update>
/// --------------------------------------------------
public class WsAttachFile : IWsAttachFile
{

    #region AR Help

    /// --------------------------------------------------
    /// <summary>
    /// ARHelpファイルダウンロード用メソッド
    /// </summary>
    /// <param name="lang">言語</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update>D.Okumura 2019/12/04 HELP多言語化対応</update>
    /// --------------------------------------------------
    public ARHelpDownloadResult ARHelpDownload(string lang)
    {
        try
        {
            ARHelpDownloadResult result = new ARHelpDownloadResult();
            string baseDir = Path.Combine(ComDefine.WEB_DATA_DIR_AR, lang);
            using (WsAttachFileImpl impl = new WsAttachFileImpl())
            {
                string path = string.Empty;
                // ARListHelpファイル取得
                byte[] arListHelp = null;
                result.ARListHelpFileName = ComDefine.AR_LIST_HELP_FILE_NAME_1;
                path = Path.Combine(baseDir, result.ARListHelpFileName);
                result.IsExistsARListHelpFile = impl.GetFile(path, out arListHelp);
                if (!result.IsExistsARListHelpFile)
                {
                    result.ARListHelpFileName = ComDefine.AR_LIST_HELP_FILE_NAME_2;
                    path = Path.Combine(baseDir, result.ARListHelpFileName);
                    result.IsExistsARListHelpFile = impl.GetFile(path, out arListHelp);
                }
                result.ARListHelpFileData = arListHelp;

                // AR情報登録画面Helpファイル取得
                byte[] arTorokuHelp = null;
                result.ARTorokuHelpFileName = ComDefine.AR_TOROKU_HELP_FILE_NAME_1;
                path = Path.Combine(baseDir, result.ARTorokuHelpFileName);
                result.IsExistsARTorokuHelpFile = impl.GetFile(path, out arTorokuHelp);
                if (!result.IsExistsARTorokuHelpFile)
                {
                    result.ARTorokuHelpFileName = ComDefine.AR_TOROKU_HELP_FILE_NAME_2;
                    path = Path.Combine(baseDir, result.ARTorokuHelpFileName);
                    result.IsExistsARTorokuHelpFile = impl.GetFile(path, out arTorokuHelp);
                }
                result.ARTorokuHelpFileData = arTorokuHelp;
            }
            return result;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 単一ファイル

    /// --------------------------------------------------
    /// <summary>
    /// 単一ファイルの削除用メソッド
    /// </summary>
    /// <param name="package">単一ファイル削除用引数クラス</param>
    /// <returns>単一ファイル削除用戻り値クラス</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// <update>H.Tajimi 2019/07/30 パーツ写真管理方式変更</update>
    /// --------------------------------------------------
    public FileDeleteResult FileDelete(FileDeletePackage package)
    {
        FileDeleteResult result = new FileDeleteResult();
        try
        {
            result.IsSuccess = false;
            using (WsAttachFileImpl impl = new WsAttachFileImpl())
            {
                string delFilePath = string.Empty;
                string dirPath = string.Empty;
                delFilePath = impl.GetFilePath(package.FileType, package.NonyusakiCD, package.ARNo, package.GirenType, package.KojiNo, package.FileName, package.FolderName);
                if (!string.IsNullOrEmpty(package.FileName))
                {
                    if (!impl.DeleteFile(delFilePath))
                    {
                        return result;
                    }
                }
                else
                {
                    dirPath = delFilePath;
                }
                string basePath = impl.GetFilePath(package.FileType, string.Empty, string.Empty, GirenType.None, string.Empty, string.Empty, string.Empty);
                if (string.IsNullOrEmpty(dirPath) && dirPath != basePath)
                {
                    if (!impl.DeleteDirectory(dirPath))
                    {
                        return result;
                    }
                }

            }
            result.IsSuccess = true;
            return result;
        }
        catch (Exception)
        {
            result.IsSuccess = false;
            return result;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 単一ファイルのアップロード用メソッド
    /// </summary>
    /// <param name="package">単一ファイルアップロード用引数クラス</param>
    /// <returns>単一ファイルアップロード用戻り値クラス</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// <update>H.Tajimi 2019/07/30 パーツ写真管理方式変更</update>
    /// <update>J.Chen 2024/08/07 メール添付ファイル時、古いフォルダを削除</update>
    /// --------------------------------------------------
    public FileUploadResult FileUpload(FileUploadPackage package)
    {
        FileUploadResult result = new FileUploadResult();
        try
        {
            result.IsSuccess = false;
            using (WsAttachFileImpl impl = new WsAttachFileImpl())
            {
                if (!string.IsNullOrEmpty(package.DeleteFileName))
                {
                    string delFilePath = string.Empty;
                    delFilePath = impl.GetFilePath(package.FileType, package.NonyusakiCD, package.ARNo, package.GirenType, package.KojiNo, package.DeleteFileName, package.FolderName);
                    // アップロード時はファイル削除できなくてもエラーとしない。
                    impl.DeleteFile(delFilePath);
                    //if (!impl.DeleteFile(delFilePath))
                    //{
                    //    return result;
                    //}
                }

                // メール添付ファイルの場合、念のため古いフォルダを削除
                if (package.FileType == FileType.Attachments)
                {
                    string delFolderPath = string.Empty;
                    delFolderPath = impl.GetFilePath(package.FileType, package.NonyusakiCD, package.ARNo, package.GirenType, package.KojiNo, string.Empty, package.FolderName);
                    // アップロード時はフォルダ削除できなくてもエラーとしない。
                    impl.DeleteDirectory(delFolderPath);
                }

                string saveFilePath = string.Empty;
                saveFilePath = impl.GetFilePath(package.FileType, package.NonyusakiCD, package.ARNo, package.GirenType, package.KojiNo, package.FileName, package.FolderName);
                if (!impl.SaveFile(saveFilePath, package.FileData))
                {
                    return result;
                }
            }
            result.IsSuccess = true;
            return result;
        }
        catch (Exception)
        {
            result.IsSuccess = false;
            return result;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 単一ファイルのダウンロード用メソッド
    /// </summary>
    /// <param name="package">単一ファイルダウンロード用引数クラス</param>
    /// <returns>単一ファイルダウンロード用戻り値クラス</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// <update>H.Tajimi 2019/07/30 パーツ写真管理方式変更</update>
    /// --------------------------------------------------
    public FileDownloadResult FileDownload(FileDownloadPackage package)
    {
        FileDownloadResult result = new FileDownloadResult();
        try
        {
            result.IsExistsFile = false;
            using (WsAttachFileImpl impl = new WsAttachFileImpl())
            {
                string FilePath = string.Empty;
                FilePath = impl.GetFilePath(package.FileType, package.NonyusakiCD, package.ARNo, package.GirenType, package.KojiNo, package.FileName, package.FolderName);

                byte[] fileData;
                if (!impl.GetFile(FilePath, out fileData))
                {
                    return result;
                }
                result.FileData = fileData;
                result.FileName = package.FileName;
            }
            result.IsExistsFile = true;
            return result;
        }
        catch (Exception)
        {
            result.IsExistsFile = false;
            return result;
        }
    }

    #endregion
}
