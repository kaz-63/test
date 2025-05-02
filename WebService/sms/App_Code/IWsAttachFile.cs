using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// メモ: ここでインターフェイス名 "IWsAttachFile" を変更する場合は、Web.config で "IWsAttachFile" への参照も更新する必要があります。

/// --------------------------------------------------
/// <summary>
/// ファイルアップロード、ダウンロード用インターフェイス
/// </summary>
/// <create>Y.Higuchi 2010/07/05</create>
/// <update></update>
/// --------------------------------------------------
[ServiceContract(Namespace="http://smssrv")]
public interface IWsAttachFile
{
    #region AR Help

    /// --------------------------------------------------
    /// <summary>
    /// ARHelpファイルダウンロード用メソッド
    /// </summary>
    /// <returns></returns>
    /// <param name="lang">言語</param>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update>D.Okumura 2019/12/04 HELP多言語化対応</update>
    /// --------------------------------------------------
    [OperationContract]
    ARHelpDownloadResult ARHelpDownload(string lang);

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
    [OperationContract]
    FileDeleteResult FileDelete(FileDeletePackage package);

    /// --------------------------------------------------
    /// <summary>
    /// 単一ファイルのアップロード用メソッド
    /// </summary>
    /// <param name="package">単一ファイルアップロード用引数クラス</param>
    /// <returns>単一ファイルアップロード用戻り値クラス</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [OperationContract]
    FileUploadResult FileUpload(FileUploadPackage package);

    /// --------------------------------------------------
    /// <summary>
    /// 単一ファイルのダウンロード用メソッド
    /// </summary>
    /// <param name="package">単一ファイルダウンロード用引数クラス</param>
    /// <returns>単一ファイルダウンロード用戻り値クラス</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [OperationContract]
    FileDownloadResult FileDownload(FileDownloadPackage package);

    #endregion
}
