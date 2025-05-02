using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// ファイルアップロード、ダウンロード(データアクセス層)
/// </summary>
/// <create>Y.Higuchi 2010/07/05</create>
/// <update></update>
/// --------------------------------------------------
public class WsAttachFileImpl : WsBaseImpl
{
    #region Constructors

    public WsAttachFileImpl()
    {
    }

    #endregion

    #region ファイル共通

    /// --------------------------------------------------
    /// <summary>
    /// ファイル取得
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="fileData">ファイルデータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetFile(string filePath, out byte[] fileData)
    {
        try
        {
            fileData = null;
            string targetPath = string.Empty;
            if (Path.IsPathRooted(filePath))
            {
                targetPath = filePath;
            }
            else
            {
                targetPath = Path.Combine(ComDefine.WEB_DATA_DIR_ROOT, filePath);
            }

            if (!File.Exists(targetPath))
            {
                return false;
            }

            using (FileStream fs = new FileStream(targetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                fs.Close();
            }
            return true;
        }
        catch (Exception)
        {
            fileData = null;
            return false;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ファイルパスの取得
    /// </summary>
    /// <param name="fileType">ファイル種類</param>
    /// <param name="nonyusakiCD">納入先コード</param>
    /// <param name="arNo">AR No.</param>
    /// <param name="girenType">技連種類</param>
    /// <param name="kojiNo">工事識別NO</param>
    /// <param name="fileName">ファイル名</param>
    /// <param name="folderName">フォルダ名</param>
    /// <returns>ファイルパス</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2019/01/23 写真の保存先変更</update>
    /// <update>D.Okumura 2019/06/18 ARリッチテキスト化対応</update>
    /// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
    /// <update>D.Okumura 2019/12/12 凡例対応</update>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    public string GetFilePath(FileType fileType, string nonyusakiCD, string arNo, GirenType girenType ,string kojiNo, string fileName, string folderName)
    {
        try
        {
            string filePath = ComDefine.WEB_DATA_DIR_ROOT;
            switch (fileType)
            {
                case FileType.ARGiren:
                case FileType.ARRef:
                case FileType.ARAttachments:
                    // 技連のルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_GIREN);
                    // 納入先コード付与
                    if (!string.IsNullOrEmpty(nonyusakiCD))
                    {
                        filePath = Path.Combine(filePath, nonyusakiCD);
                    }
                    // ARNo.付与
                    if (!string.IsNullOrEmpty(arNo))
                    {
                        filePath = Path.Combine(filePath, arNo);
                    }
                    // 技連種類付与
                    if (girenType != GirenType.None)
                    {
                        filePath = Path.Combine(filePath, ((int)girenType).ToString());
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.CaseMark:
                    // CaseMarkのルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_CASEMARK);
                    // 工事識別管理NO付与
                    if (!string.IsNullOrEmpty(kojiNo))
                    {
                        filePath = Path.Combine(filePath, kojiNo);
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.Template:
                    // Templateのルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_TEMPLATE);
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.TagPictures:
                    // 画像用のルートを再設定
                    filePath = ComDefine.PICTURE_DATA_DIR_ROOT;
                    // 画像のルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_KATASHIKI_PICTURES);
                    // フォルダ名付与
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        filePath = Path.Combine(filePath, folderName);
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.Attachments:
                    // 添付ファイルのルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_ATTACHMENTS);
                    // フォルダ名付与
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        filePath = Path.Combine(filePath, folderName);
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.Legend:
                    // 凡例ファイルのルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_LEGEND);
                    // フォルダ名付与
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        filePath = Path.Combine(filePath, folderName);
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
                case FileType.MailTemplate:
                    // 凡例ファイルのルート
                    filePath = Path.Combine(filePath, ComDefine.WEB_DATA_DIR_NAME_MAIL_TEMPLATE);
                    // フォルダ名付与
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        filePath = Path.Combine(filePath, folderName);
                    }
                    // ファイル名付与
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = Path.Combine(filePath, fileName);
                    }
                    break;
            }
            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ファイルの削除
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ディレクトリの削除
    /// </summary>
    /// <param name="dirPath">ディレクトリパス</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DeleteDirectory(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath,true);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// ファイルの保存
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="fileData">ファイルデータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveFile(string filePath, byte[] fileData)
    {
        try
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            int size = 0;
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(fileData, 0, fileData.Length);
                size = (int)fs.Length;
                fs.Close();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion
}
