using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Commons;
using DSWControl.DSWRichTextBox;
using WsConnection;
using WsConnection.WebRefAttachFile;
using System.IO;

namespace SMS.A01.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// AR用テキストボックス
    /// </summary>
    /// <create>D.Okumura 2019/06/13</create>
    /// <update></update>
    /// <remarks>
    /// コンテキストメニューの多言語化対応や、テーブルに強く紐づいた処理を行うため、
    /// コントーロールをカスタマイズしている
    /// </remarks>
    /// --------------------------------------------------
    internal partial class ARJohoRichTextBox : DSWControl.DSWRichTextBox.DSWRichTextBox
    {

        #region 内部クラス: 画像情報
        /// --------------------------------------------------
        /// <summary>
        /// 画像情報
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private class ImageInfo
        {
            /// --------------------------------------------------
            /// <summary>
            /// ARNo
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            public string ARNo { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// 納入先コード
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            public string NonyusakiCd { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// ファイル名
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            public string FileName { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// 画像位置
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// --------------------------------------------------
            public int Position { get; set; }
            /// --------------------------------------------------
            /// <summary>
            /// 画像データ
            /// </summary>
            /// <create>D.Okumura 2019/06/13</create>
            /// <update></update>
            /// <remarks>
            /// Download()からLoadImages()コールまで、
            /// SetupFilesToDataTable()からUpload()までのみデータを保持。
            /// ほかのタイミングではnull状態となるため注意すること。
            /// </remarks>
            /// --------------------------------------------------
            public byte[] Image { get; set; }
        }
        #endregion

        #region フィールド
        /// --------------------------------------------------
        /// <summary>
        /// アップロードファイル一覧
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly List<ImageInfo> UploadImages = new List<ImageInfo>();
        /// --------------------------------------------------
        /// <summary>
        /// ダウンロードファイル一覧
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly List<ImageInfo> DownloadImages = new List<ImageInfo>();
        #endregion

        #region コンストラクタ
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoRichTextBox()
            : base()
        {
            InitializeComponent();
            this.Initialize();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">コンテナ</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoRichTextBox(IContainer container)
            : base()
        {
            container.Add(this);

            InitializeComponent();
            this.Initialize();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Initialize()
        {
            // 貼り付け動作: 画像のみ貼り付け
            this.PastableType = PastableTypeInner.PicturesOnly;
            // URLの検出を無効化
            this.DetectUrls = false;
            // 貼り付け可能サイズのチェック
            this.MaxPastableObjectSize = ComDefine.GIREN_FILE_MAX_SIZE;
        }
        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 貼り付け動作
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// <remarks>
        /// 上位コントロールから操作を隠します。
        /// </remarks>
        /// --------------------------------------------------
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new PastableTypeInner PastableType
        {
            get { return base.PastableType; }
            private set { base.PastableType = value; }
        }


        /// --------------------------------------------------
        /// <summary>
        /// URL検出
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// <remarks>
        /// 上位コントロールから操作を隠します。
        /// </remarks>
        /// --------------------------------------------------
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool DetectUrls
        {
            get { return base.DetectUrls; }
            private set { base.DetectUrls = value; }
        }


        /// --------------------------------------------------
        /// <summary>
        /// 最大貼り付け可能サイズ
        /// </summary>
        /// <create>D.Okumura 2019/06/21</create>
        /// <update></update>
        /// <remarks>
        /// 上位コントロールから操作を隠します。
        /// </remarks>
        /// --------------------------------------------------
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override long MaxPastableObjectSize
        {
            get
            {
                return base.MaxPastableObjectSize;
            }
            set
            {
                base.MaxPastableObjectSize = value;
            }
        }

        
        #endregion

        #region コンテキストメニュー
        /// --------------------------------------------------
        /// <summary>
        /// コンテキストメニュー
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemUndo.Enabled = this.CanUndo;
            toolStripMenuItemCut.Enabled = !this.ReadOnly && this.SelectionLength != 0;
            toolStripMenuItemCopy.Enabled = this.SelectionLength != 0;
            toolStripMenuItemPaste.Enabled = this.CanPaste();
            toolStripMenuItemDelete.Enabled = !this.ReadOnly && this.SelectionLength != 0;
            toolStripMenuItemSelectAll.Enabled = this.SelectionLength != this.TextLength;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンテキストメニュークリックイベント
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripMenuItemUndo)
            {
                this.Undo();
            }
            else if (e.ClickedItem == toolStripMenuItemCut)
            {
                this.Cut();
            }
            else if (e.ClickedItem == toolStripMenuItemCopy)
            {
                this.Copy();
            }
            else if (e.ClickedItem == toolStripMenuItemPaste)
            {
                this.Paste();
            }
            else if (e.ClickedItem == toolStripMenuItemDelete)
            {
                this.SelectedText = string.Empty;
            }
            else if (e.ClickedItem == toolStripMenuItemSelectAll)
            {
                this.SelectAll();
            }
        }
        #endregion

        #region クリア

        /// --------------------------------------------------
        /// <summary>
        /// クリア処理
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public new void Clear()
        {
            base.Clear();
            this.ClearFiles();

        }


        /// --------------------------------------------------
        /// <summary>
        /// ファイル一覧をクリア
        /// </summary>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public void ClearFiles()
        {
            this.DownloadImages.Clear();
            this.UploadImages.Clear();
        }
        #endregion

        #region チェック処理
        /// <summary>
        /// ファイルサイズをチェックする
        /// </summary>
        /// <param name="max">最大サイズ</param>
        /// <param name="isSelectFirst">超過時に対象ファイルを選択</param>
        /// <returns></returns>
        public bool CheckCompressedSize(long max, bool isSelectFirst)
        {
            foreach (var item in this.GetImageData())
            {
                long len = item.CompressedData.LongLength;
                if (len > max)
                {
                    if (isSelectFirst)
                        this.Select(item.Position, 1);
                    return false;
                }
            }
            return true;
        }


        #endregion

        #region ダウンロード処理
        /// --------------------------------------------------
        /// <summary>
        /// 画像情報を取得する
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="fileType">画像種別</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public void GetFilesFromDataTable(DataTable dt, string fileType)
        {
            // 種別でフィルタリングする
            this.DownloadImages.Clear();
            foreach (DataRow dr in dt.Select(string.Format("{0} = '{1}'", Def_T_AR_FILE.FILE_KIND, fileType)))
            {
                var info = new ImageInfo()
                {
                    NonyusakiCd = ComFunc.GetFld(dr, Def_T_AR_FILE.NONYUSAKI_CD),
                    ARNo = ComFunc.GetFld(dr, Def_T_AR_FILE.AR_NO),
                    Position = ComFunc.GetFldToInt32(dr, Def_T_AR_FILE.POSITION),
                    FileName = ComFunc.GetFld(dr, Def_T_AR_FILE.FILE_NAME),
                };
                this.DownloadImages.Add(info);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// ダウンロード処理
        /// </summary>
        /// <returns>ダウンロード結果</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool Download()
        {
            var result = true;
            ConnAttachFile conn = new ConnAttachFile();
            foreach (var item in this.DownloadImages)
            {
                try
                {
                    FileDownloadPackage package = new FileDownloadPackage();

                    package.FileName = item.FileName;
                    package.FileType = FileType.ARAttachments;
                    package.NonyusakiCD = item.NonyusakiCd;
                    package.ARNo = item.ARNo;
                    package.GirenType = GirenType.None;

                    var ret = conn.FileDownload(package);
                    if (!ret.IsExistsFile)
                    {
                        result = false;
                    }
                    else
                    {
                        item.Image = ret.FileData;
                    }
                }
                catch (Exception ex)
                {
                    // 例外は投げない
                    Debug.WriteLine(string.Format("ARJohoRichTextBox.Download exception: {0}", ex.ToString()));
                    result = false;
                }
            }
            return result;
        }
        /// --------------------------------------------------
        /// <summary>
        /// ダウンロード画像のロード
        /// </summary>
        /// <exception cref="OutOfMemoryException">貼り付け中のメモリ不足</exception>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public void LoadImages()
        {
            var list = new List<DSWRichTextBoxImageItem>();
            foreach (var item in this.DownloadImages)
            {
                if (item.Image == null)
                    continue;
                var img = new DSWRichTextBoxImageItem();
                img.Position = item.Position;
                img.CompressedData = item.Image;
                img.Validate();
                list.Add(img);
                // ダウンロードした情報は破棄する
                item.Image = null;
            }
            if (list.Count < 1)
                return;
            this.SetImageData(list);
        }
        #endregion

        #region アップロード処理
        /// --------------------------------------------------
        /// <summary>
        /// 画像情報を取得し、テーブルへ情報を設定します
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="listKbn">区分</param>
        /// <param name="arno">ARNo</param>
        /// <param name="fileType">ファイル種別</param>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetupFilesToDataTable(DataTable dt, string nonyusakiCd, string listKbn, string arno, string fileType)
        {
            UploadImages.Clear();
            var list = GetImageData();
            foreach (var item in list)
            {
                var info = new ImageInfo() {
                    NonyusakiCd = nonyusakiCd,
                    ARNo = arno,
                    Position = item.Position,
                    Image = item.CompressedData,
                    FileName = string.Format("{0}_{1:0000}.gz", fileType, item.Position),
                };
                UploadImages.Add(info);

                var dr = dt.NewRow();
                dr[Def_T_AR_FILE.NONYUSAKI_CD] = nonyusakiCd;
                dr[Def_T_AR_FILE.LIST_FLAG] = listKbn;
                dr[Def_T_AR_FILE.AR_NO] = info.ARNo;
                dr[Def_T_AR_FILE.POSITION] = info.Position.ToString();
                dr[Def_T_AR_FILE.FILE_KIND] = fileType;
                dr[Def_T_AR_FILE.FILE_NAME] = info.FileName;
                dt.Rows.Add(dr);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// ファイルをアップロードおよび不要なファイルを削除
        /// </summary>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arno">ARNo</param>
        /// <returns>成功可否</returns>
        /// <create>D.Okumura 2019/06/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool Upload(string nonyusakiCd, string arno)
        {
            var result = true;
            ConnAttachFile conn = new ConnAttachFile();
            foreach (var item in UploadImages)
            {
                // ありえないが、念のためガードする
                if (item.Image == null)
                    continue;
                try
                {
                    FileUploadPackage package = new FileUploadPackage();

                    package.FileName = item.FileName;
                    package.FileType = FileType.ARAttachments;
                    package.NonyusakiCD = nonyusakiCd;
                    package.ARNo = arno;
                    package.GirenType = GirenType.None;
                    package.FileData = item.Image;
                    // 同一のファイル名が指定されている場合、削除対象とする
                    if (this.DownloadImages.Any(w => w.FileName == item.FileName))
                        package.DeleteFileName = item.FileName;
                    var ret = conn.FileUpload(package);
                    if (!ret.IsSuccess)
                        result = false;
                }
                catch (Exception ex)
                {
                    // 例外は投げない
                    Debug.WriteLine(string.Format("ARJohoRichTextBox.Upload exception: {0}", ex.ToString()));
                    result = false;
                }
                // 成功可否にかかわらず、画像情報を破棄する
                item.Image = null;
            }
            // 全てアップロードに成功したら、シュリンクする
            if (result)
            {
                // 不要なファイル(存在しなくなったファイル)を削除する
                foreach (var item in this.DownloadImages.Where(w => !this.UploadImages.Any(a => a.FileName == w.FileName)))
                {

                    try
                    {
                        FileDeletePackage package = new FileDeletePackage();

                        package.FileName = item.FileName;
                        package.FileType = FileType.ARAttachments;
                        package.NonyusakiCD = nonyusakiCd;
                        package.ARNo = arno;
                        package.GirenType = GirenType.None;
                        // 無条件に削除する
                        // 結果のハンドリングは行わない
                        conn.FileDelete(package);
                    }
                    catch (Exception ex)
                    {
                        // 例外は投げない
                        Debug.WriteLine(string.Format("ARJohoRichTextBox.Upload exception: {0}", ex.ToString()));
                        // 削除エラーは無視する
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
