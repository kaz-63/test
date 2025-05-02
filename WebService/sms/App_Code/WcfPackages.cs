using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Text;

#region ARHelpファイル

/// --------------------------------------------------
/// <summary>
/// ARHelpファイルダウンロード用戻り値クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/05</create>
/// <update></update>
/// --------------------------------------------------
[DataContract]
public class ARHelpDownloadResult
{
    #region Fields

    private bool _isExistsARListHelpFile = false;
    private bool _isExistsARTorokuHelpFile = false;
    private string _arListHelpFileName = string.Empty;
    private string _arTorokuHelpFileName = string.Empty;
    private byte[] _arListHelpFileData = null;
    private byte[] _arTorokuHelpFileData = null;

    #endregion

    #region ARListHelpファイル

    #region ARListHelpファイル存在フラグ

    /// --------------------------------------------------
    /// <summary>
    /// ARListHelpファイル存在フラグ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public bool IsExistsARListHelpFile
    {
        get { return this._isExistsARListHelpFile; }
        set { this._isExistsARListHelpFile = value; }
    }

    #endregion

    #region ARListHelpファイル名

    /// --------------------------------------------------
    /// <summary>
    /// ARListHelpファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string ARListHelpFileName
    {
        get { return this._arListHelpFileName; }
        set { this._arListHelpFileName = value; }
    }

    #endregion

    #region ARListHelpファイルデータ

    /// --------------------------------------------------
    /// <summary>
    /// ARListHelpファイルデータ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public byte[] ARListHelpFileData
    {
        get { return this._arListHelpFileData; }
        set { this._arListHelpFileData = value; }
    }

    #endregion

    #endregion

    #region AR情報登録画面Helpファイル

    #region AR情報登録画面Helpファイル存在フラグ

    /// --------------------------------------------------
    /// <summary>
    /// AR情報登録画面Helpファイル存在フラグ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public bool IsExistsARTorokuHelpFile
    {
        get { return this._isExistsARTorokuHelpFile; }
        set { this._isExistsARTorokuHelpFile = value; }
    }

    #endregion

    #region AR情報登録画面Helpファイル名

    /// --------------------------------------------------
    /// <summary>
    /// AR情報登録画面Helpファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string ARTorokuHelpFileName
    {
        get { return this._arTorokuHelpFileName; }
        set { this._arTorokuHelpFileName = value; }
    }

    #endregion

    #region AR情報登録画面Helpファイルデータ

    /// --------------------------------------------------
    /// <summary>
    /// AR情報登録画面Helpファイルデータ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/05</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public byte[] ARTorokuHelpFileData
    {
        get { return this._arTorokuHelpFileData; }
        set { this._arTorokuHelpFileData = value; }
    }

    #endregion

    #endregion
}

#endregion

#region ファイル種類の列挙型

/// --------------------------------------------------
/// <summary>
/// ファイル種類の列挙型
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update></update>
/// --------------------------------------------------
[DataContract]
public enum FileType
{
    /// --------------------------------------------------
    /// <summary>
    /// AR技連ファイル
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    ARGiren = 0,

    /// --------------------------------------------------
    /// <summary>
    /// CaseMarkファイル
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    CaseMark = 1,

    /// --------------------------------------------------
    /// <summary>
    /// Templateファイル
    /// </summary>
    /// <create> 2012/04/24</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    Template = 2,

    /// --------------------------------------------------
    /// <summary>
    /// AR参考ファイル
    /// </summary>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    ARRef = 3,

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細画像ファイル
    /// </summary>
    /// <create>H.Tajimi 2018/11/06</create>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// --------------------------------------------------
    [EnumMember]
    TagPictures = 4,

    /// --------------------------------------------------
    /// <summary>
    /// 添付ファイル
    /// </summary>
    /// <create>H.Tajimi 2018/11/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    Attachments = 5,
    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイル
    /// </summary>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    ARAttachments = 6,
    /// --------------------------------------------------
    /// <summary>
    /// 凡例ファイル
    /// </summary>
    /// <create>D.Okumura 2019/12/12</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    Legend = 7,
    /// --------------------------------------------------
    /// <summary>
    /// Mailテンプレートファイル
    /// </summary>
    /// <create>D.Okumura 2020/05/18</create>
    /// <update></update>
    /// --------------------------------------------------
    [EnumMember]
    MailTemplate = 8,
}

#endregion

#region 技連種類

/// --------------------------------------------------
/// <summary>
/// 技連の種類
/// </summary>
/// <create>Y.Higuchi 2010/08/31</create>
/// <update></update>
/// --------------------------------------------------
public enum GirenType
{
    /// --------------------------------------------------
    /// <summary>
    /// 無し
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    None = 0,
    /// --------------------------------------------------
    /// <summary>
    /// 技連No(1)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    No1 = 1,
    /// --------------------------------------------------
    /// <summary>
    /// 技連No(2)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    No2 = 2,
    /// --------------------------------------------------
    /// <summary>
    /// 技連No(3)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    No3 = 3,
    /// --------------------------------------------------
    /// <summary>
    /// 参考資料No(1)
    /// </summary>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    RefNo1 = 4,
    /// --------------------------------------------------
    /// <summary>
    /// 参考資料No(2)
    /// </summary>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    RefNo2 = 5,
    /// --------------------------------------------------
    /// <summary>
    /// 技連No(4)
    /// </summary>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    No4 = 6,
    /// --------------------------------------------------
    /// <summary>
    /// 技連No(5)
    /// </summary>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    No5 = 7,
}

#endregion

#region 単一ファイル

#region FileDeletePackage

/// --------------------------------------------------
/// <summary>
/// 単一ファイル削除用引数クラス
/// </summary>
/// <create>Y.Higuchi 2010/08/03</create>
/// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
/// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
/// --------------------------------------------------
[DataContract]
public class FileDeletePackage
{
    #region Fields

    private FileType _fileType = FileType.ARGiren;
    private string _nonyusakiCD = null;
    private string _arNo = null;
    private GirenType _girenType = GirenType.None;
    private string _kojiNo = null;
    private string _fileName = string.Empty;
    private string _folderName = string.Empty;

    #endregion

    #region ファイル種類

    /// --------------------------------------------------
    /// <summary>
    /// ファイル種類
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public FileType FileType
    {
        get { return this._fileType; }
        set { this._fileType = value; }
    }

    #endregion

    #region 納入先コード

    /// --------------------------------------------------
    /// <summary>
    /// 納入先コード(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string NonyusakiCD
    {
        get { return this._nonyusakiCD; }
        set { this._nonyusakiCD = value; }
    }

    #endregion

    #region AR No.

    /// --------------------------------------------------
    /// <summary>
    /// AR No.(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string ARNo
    {
        get { return this._arNo; }
        set { this._arNo = value; }
    }

    #endregion

    #region 技連種類

    /// --------------------------------------------------
    /// <summary>
    /// 技連種類
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public GirenType GirenType
    {
        get { return this._girenType; }
        set { this._girenType = value; }
    }

    #endregion

    #region 工事識別管理NO

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別管理NO(FileTypeがCaseMarkの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string KojiNo
    {
        get { return this._kojiNo; }
        set { this._kojiNo = value; }
    }

    #endregion

    #region ファイル名

    /// --------------------------------------------------
    /// <summary>
    /// 削除ファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FileName
    {
        get { return this._fileName; }
        set { this._fileName = value; }
    }

    #endregion

    #region フォルダ名

    /// --------------------------------------------------
    /// <summary>
    /// フォルダ名
    /// </summary>
    /// <create>H.Tajimi 2018/11/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FolderName
    {
        get { return this._folderName; }
        set { this._folderName = value; }
    }

    #endregion
}

#endregion

#region FileDeleteResult

/// --------------------------------------------------
/// <summary>
/// 単一ファイル削除用戻り値クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update></update>
/// --------------------------------------------------
[DataContract]
public class FileDeleteResult
{
    #region Fields

    private bool _isSuccess = false;

    #endregion

    #region 処理結果が成功かどうか

    /// --------------------------------------------------
    /// <summary>
    /// 処理結果が成功かどうか
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public bool IsSuccess
    {
        get { return this._isSuccess; }
        set { this._isSuccess = value; }
    }

    #endregion
}

#endregion

#region FileUploadPackage

/// --------------------------------------------------
/// <summary>
/// 単一ファイルアップロード用引数クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
/// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
/// --------------------------------------------------
[DataContract]
public class FileUploadPackage
{
    #region Fields

    private FileType _fileType = FileType.ARGiren;
    private string _nonyusakiCD = null;
    private string _arNo = null;
    private GirenType _girenType = GirenType.None;
    private string _kojiNo = null;
    private string _fileName = string.Empty;
    private byte[] _fileData = null;
    private string _deleteFileName = null;
    private string _folderName = string.Empty;

    #endregion

    #region ファイル種類

    /// --------------------------------------------------
    /// <summary>
    /// ファイル種類
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public FileType FileType
    {
        get { return this._fileType; }
        set { this._fileType = value; }
    }

    #endregion

    #region 納入先コード

    /// --------------------------------------------------
    /// <summary>
    /// 納入先コード(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string NonyusakiCD
    {
        get { return this._nonyusakiCD; }
        set { this._nonyusakiCD = value; }
    }

    #endregion

    #region AR No.

    /// --------------------------------------------------
    /// <summary>
    /// AR No.(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string ARNo
    {
        get { return this._arNo; }
        set { this._arNo = value; }
    }

    #endregion

    #region 技連種類

    /// --------------------------------------------------
    /// <summary>
    /// 技連種類
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public GirenType GirenType
    {
        get { return this._girenType; }
        set { this._girenType = value; }
    }

    #endregion

    #region 工事識別管理NO

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別管理NO(FileTypeがCaseMarkの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string KojiNo
    {
        get { return this._kojiNo; }
        set { this._kojiNo = value; }
    }

    #endregion

    #region ファイル名

    /// --------------------------------------------------
    /// <summary>
    /// ファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FileName
    {
        get { return this._fileName; }
        set { this._fileName = value; }
    }

    #endregion

    #region ファイルデータ

    /// --------------------------------------------------
    /// <summary>
    /// ファイルデータ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public byte[] FileData
    {
        get { return this._fileData; }
        set { this._fileData = value; }
    }

    #endregion

    #region 削除ファイル名

    /// --------------------------------------------------
    /// <summary>
    /// 削除ファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string DeleteFileName
    {
        get { return this._deleteFileName; }
        set { this._deleteFileName = value; }
    }

    #endregion

    #region フォルダ名

    /// --------------------------------------------------
    /// <summary>
    /// フォルダ名
    /// </summary>
    /// <create>H.Tajimi 2018/11/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FolderName
    {
        get { return this._folderName; }
        set { this._folderName = value; }
    }

    #endregion
}

#endregion

#region FileUploadResult

/// --------------------------------------------------
/// <summary>
/// 単一ファイルアップロード用戻り値クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update></update>
/// --------------------------------------------------
[DataContract]
public class FileUploadResult
{
    #region Fields

    private bool _isSuccess = false;

    #endregion

    #region 処理結果が成功かどうか

    /// --------------------------------------------------
    /// <summary>
    /// 処理結果が成功かどうか
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public bool IsSuccess
    {
        get { return this._isSuccess; }
        set { this._isSuccess = value; }
    }

    #endregion
}

#endregion

#region FileDownloadPackage

/// --------------------------------------------------
/// <summary>
/// 単一ファイルダウンロード用引数クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
/// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
/// --------------------------------------------------
[DataContract]
public class FileDownloadPackage
{
    #region Fields

    private FileType _fileType = FileType.ARGiren;
    private string _nonyusakiCD = null;
    private string _arNo = null;
    private GirenType _girenType = GirenType.None;
    private string _kojiNo = null;
    private string _fileName = string.Empty;
    private string _folderName = string.Empty;

    #endregion

    #region ファイル種類

    /// --------------------------------------------------
    /// <summary>
    /// ファイル種類
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public FileType FileType
    {
        get { return this._fileType; }
        set { this._fileType = value; }
    }

    #endregion

    #region 納入先コード

    /// --------------------------------------------------
    /// <summary>
    /// 納入先コード(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string NonyusakiCD
    {
        get { return this._nonyusakiCD; }
        set { this._nonyusakiCD = value; }
    }

    #endregion

    #region AR No.

    /// --------------------------------------------------
    /// <summary>
    /// AR No.(FileTypeがARGiren/ARRefの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string ARNo
    {
        get { return this._arNo; }
        set { this._arNo = value; }
    }

    #endregion

    #region 技連種類

    /// --------------------------------------------------
    /// <summary>
    /// 技連種類
    /// </summary>
    /// <create>Y.Higuchi 2010/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public GirenType GirenType
    {
        get { return this._girenType; }
        set { this._girenType = value; }
    }

    #endregion

    #region 工事識別管理NO

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別管理NO(FileTypeがCaseMarkの時に設定)
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string KojiNo
    {
        get { return this._kojiNo; }
        set { this._kojiNo = value; }
    }

    #endregion

    #region ファイル名

    /// --------------------------------------------------
    /// <summary>
    /// ファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FileName
    {
        get { return this._fileName; }
        set { this._fileName = value; }
    }

    #endregion

    #region フォルダ名

    /// --------------------------------------------------
    /// <summary>
    /// フォルダ名
    /// </summary>
    /// <create>H.Tajimi 2018/11/29</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FolderName
    {
        get { return this._folderName; }
        set { this._folderName = value; }
    }

    #endregion
}

#endregion

#region FileDownloadResult

/// --------------------------------------------------
/// <summary>
/// 単一ファイルダウンロード用戻り値クラス
/// </summary>
/// <create>Y.Higuchi 2010/07/30</create>
/// <update></update>
/// --------------------------------------------------
[DataContract]
public class FileDownloadResult
{
    #region Fields

    private bool _isExistsFile = false;
    private string _fileName = string.Empty;
    private byte[] _fileData = null;

    #endregion

    #region ファイル存在フラグ

    /// --------------------------------------------------
    /// <summary>
    /// ファイル存在フラグ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public bool IsExistsFile
    {
        get { return this._isExistsFile; }
        set { this._isExistsFile = value; }
    }

    #endregion

    #region ファイル名

    /// --------------------------------------------------
    /// <summary>
    /// ファイル名
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public string FileName
    {
        get { return this._fileName; }
        set { this._fileName = value; }
    }

    #endregion

    #region ファイルデータ

    /// --------------------------------------------------
    /// <summary>
    /// ファイルデータ
    /// </summary>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    [DataMember]
    public byte[] FileData
    {
        get { return this._fileData; }
        set { this._fileData = value; }
    }

    #endregion
}

#endregion

#endregion