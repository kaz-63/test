using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using XlsCreatorHelper;

using WsConnection.WebRefS01;
using SMS.P02.Forms;
using SMS.E01;
using XlsxCreatorHelper;
using SMS.S01.Properties;
using GrapeCity.Win.ElTabelle.Editors;
using WsConnection.WebRefAttachFile;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 一括アップロード
    /// </summary>
    /// <create>H.Tajimi 2018/11/02</create>
    /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
    /// --------------------------------------------------
    public partial class IkkatsuUpload : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        // シート内の列インデックス
        protected const int SHEET_COL_ZUMEN_KEISHIKI = 0;
        protected const int SHEET_COL_HINMEI_JP = 1;
        protected const int SHEET_COL_HINMEI = 2;
        protected const int SHEET_COL_EXISTS_PICTURE = 3;
        protected const int SHEET_COL_FILE_NAME = 4;
        protected const int SHEET_COL_BTN_OPERATION = 5;

        // 画像ファイルの拡張子
        protected const string PICTURE_EXT = @".jpg";

        #endregion

        #region enum

        /// --------------------------------------------------
        /// <summary>
        /// 操作タイプ
        /// </summary>
        /// <create>H.Tajimi 2019/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum OperationType
        {
            Upload,
            Cancel,
        }

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 検索用データテーブル
        /// </summary>
        /// <create>H.Tajimi 2019/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtSearch = null;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        public IkkatsuUpload(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                this.EditMode = SystemBase.EditMode.Update;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                // 初期フォーカスの設定
                this.btnReference.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 終了処理

        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                this.CleanFilesData();
            }
        }

        #endregion

        #region シート初期化

        /// --------------------------------------------------
        /// <summary>
        /// シート初期化
        /// </summary>
        /// <param name="sheet">ElTabelleSheet</param>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);
            // Enterキーを次の行に移動するよう変更
            sheet.ShortCuts.Remove(Keys.Enter);
            sheet.ShortCuts.Add(Keys.Enter, new KeyAction[] { KeyAction.EndEdit, KeyAction.NextRow });
            sheet.AllowUserToAddRows = false;
            sheet.KeepHighlighted = true;
            sheet.SelectionType = SelectionType.Single;
            sheet.ViewMode = ViewMode.Default;
            try
            {
                sheet.MaxColumns = 0;
                sheet.MaxRows = 1;
                int colIndex = 0;
                var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                var numEditor = ElTabelleSheetHelper.NewNumberEditor();
                numEditor.DisplayFormat = new NumberFormat("##,##0", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                numEditor.Format = new NumberFormat("##,##0", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                numEditor.ReadOnly = true;

                this.SetElTabelleColumn(sheet, colIndex++, Resources.IkkatsuUpload_DrawingNoFormat, false, true, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, txtEditor, 150);
                this.SetElTabelleColumn(sheet, colIndex++, Resources.IkkatsuUpload_JpName, false, true, Def_T_TEHAI_MEISAI.HINMEI_JP, txtEditor, 190);
                this.SetElTabelleColumn(sheet, colIndex++, Resources.IkkatsuUpload_Name, false, true, Def_T_TEHAI_MEISAI.HINMEI, txtEditor, 190);
                this.SetElTabelleColumn(sheet, colIndex++, Resources.IkkatsuUpload_Picture, false, true, ComDefine.FLD_EXISTS_PICTURE, txtEditor, 50);
                this.SetElTabelleColumn(sheet, colIndex++, Resources.IkkatsuUpload_FileName, false, true, Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME, txtEditor, 330);
                this.SetElTabelleColumn(sheet, colIndex++, string.Empty, false, false, string.Empty, this.GetButtonEditor(OperationType.Cancel), 60);
                
                // 列ヘッダをマージ(列方向結合)
                sheet.ColumnHeaders.Merge(new Range(SHEET_COL_FILE_NAME, 0, SHEET_COL_BTN_OPERATION, 0, RangeStyle.Cells));

                // 写真列はMiddleCenter
                this.shtMeisai.Columns[SHEET_COL_EXISTS_PICTURE].AlignHorizontal = AlignHorizontal.Center;
                this.shtMeisai.Columns[SHEET_COL_EXISTS_PICTURE].AlignVertical = AlignVertical.Middle;

                // グリッド線
                sheet.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    sheet.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Columns[i].DisabledForeColor = Color.Black;
                }
                sheet.MaxRows = 0;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア
        /// </summary>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();

                // 写真フォルダ
                this.txtPicture.Text = string.Empty;
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.btnReference.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(0, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.shtMeisai.EditState) return false;
                DataTable dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null) return false;
                DataTable dtCheck = dt.Copy();
                dtCheck.AcceptChanges();
                if (dtCheck.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    return false;
                }

                for (int rowIndex = 0; rowIndex < dtCheck.Rows.Count; rowIndex++)
                {
                    if (!this.ExistsFile(ComFunc.GetFld(dtCheck.Rows[rowIndex], Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME)))
                    {
                        // {0}行目の{1}が存在しません。
                        this.ShowMessage("S0100040004", (rowIndex + 1).ToString(), Resources.IkkatsuUpload_FileName);
                        this.shtMeisai.ActivePosition = new Position(SHEET_COL_FILE_NAME, rowIndex);
                        this.shtMeisai.Focus();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update>H.Tajimi 2019/09/11 半角文字が含まれているファイルがアップロードされる問題を修正</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (string.IsNullOrEmpty(this.txtPicture.Text))
                {
                    // 写真フォルダが選択されていません。
                    this.ShowMessage("S0100040001");
                    return false;
                }
                if (!Directory.Exists(this.txtPicture.Text))
                {
                    // 写真フォルダが存在しません。
                    this.ShowMessage("S0100040002");
                    return false;
                }

                this.CleanFilesData();
                this._dtSearch = this.GetSchemeDispData();
                var target = new DirectoryInfo(this.txtPicture.Text);
                var regulation = new ComRegulation();
                var regulationType = ComRegulation.REGULATION_NARROW_CHAR + ComRegulation.REGULATION_NARROW_SIGN + ComRegulation.REGULATION_NARROW_SPACE + ComRegulation.REGULATION_NARROW_NUMERIC_SIGN;
                foreach (FileInfo file in target.GetFiles("*" + PICTURE_EXT))
                {
                    // ファイル名(拡張子除く)に半角文字が含まれている場合は取込対象外
                    var fileName = file.Name;
                    var zumenKeishiki = Path.GetFileNameWithoutExtension(fileName);
                    if (!regulation.CheckRegularString(zumenKeishiki, regulationType, false, false))
                    {
                        continue;
                    }

                    var dr = this._dtSearch.NewRow();
                    dr[Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME] = file.Name;
                    dr[ComDefine.FLD_MB_ZUMEN_KEISHIKI] = Path.GetFileNameWithoutExtension(file.Name);
                    this._dtSearch.Rows.Add(dr);
                }
                if (!UtilData.ExistsData(this._dtSearch))
                {
                    // 写真が存在しません。
                    this.ShowMessage("S0100040008");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region ファンクションバーのEnabled切替

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            this.fbrFunction.F01Button.Enabled = isEnabled;
            this.fbrFunction.F06Button.Enabled = isEnabled;
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディション取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected CondS01 GetCondition()
        {
            var cond = new CondS01(this.UserInfo);
            cond.UpdatePCName = UtilSystem.GetUserInfo(false).MachineName;
            return cond;
        }

        #endregion

        #region 画像ファイル処理

        #region ファイル更新

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dtMessage">DataTable(エラーメッセージ)</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/06</create>
        /// <update>K.Tsutsumi 2018/01/20 圧縮処理対応</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        private bool PictureUpdate(DataTable dt, ref DataTable dtMessage)
        {
            try
            {
                // 既にリサイズ用フォルダが存在したら削除
                if (!Directory.Exists(ComDefine.RESIZE_DIR))
                {
                    // リサイズ用フォルダ作成
                    Directory.CreateDirectory(ComDefine.RESIZE_DIR);
                }
                else
                {
                    // リサイズ用フォルダ内クリア
                    DirectoryInfo target = new DirectoryInfo(ComDefine.RESIZE_DIR);
                    foreach (FileInfo file in target.GetFiles())
                    {
                        file.Delete();
                    }
                }

                // コーデック情報
                var ici = GetEncoderInfo(ComDefine.RESIZE_ENCODE);

                // エンコードパラメータ
                // EncoderParameterオブジェクトを1つ格納できる
                // EncoderParametersクラスの新しいインスタンスを初期化
                // ここでは品質のみ指定するため1つだけ用意する
                EncoderParameters eps = new EncoderParameters(1);
                // 品質を指定
                EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)90);
                // EncoderParametersにセットする
                eps.Param[0] = ep;

                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    var fileName = ComFunc.GetFld(dt, rowIndex, Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME);
                    // リサイズ処理実行
                    var srcFilePath = Path.Combine(this.txtPicture.Text, fileName);
                    var dstFilePath = Path.Combine(ComDefine.RESIZE_DIR, fileName);
                    this.ResChange(srcFilePath, dstFilePath, ici, eps);

                    var delFileName = string.Empty;
                    var folderName = ComFunc.GetFld(dt, rowIndex, Def_T_MANAGE_ZUMEN_KEISHIKI.SAVE_DIR);
                    var createDate = ComFunc.GetFldObject(dt, rowIndex, Def_T_MANAGE_ZUMEN_KEISHIKI.CREATE_DATE);
                    if (!(createDate == null || createDate == DBNull.Value))
                    {
                        delFileName = fileName;
                    }
                    if (!this.PictureUpdate(dstFilePath, fileName, delFileName, folderName))
                    {
                        // {0}行目の{1}({2})のアップロードに失敗しました。
                        ComFunc.AddMultiMessage(dtMessage, "S0100040009", (rowIndex + 1).ToString(), Resources.IkkatsuUpload_FileName, fileName);
                    }
                }
                return !UtilData.ExistsData(dtMessage);
            }
            catch
            {
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイル更新
        /// </summary>
        /// <param name="fullPath">アップロードファイルフルパス</param>
        /// <param name="fileName">アップロードファイル</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/11/06</create>
        /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        private bool PictureUpdate(string fullPath, string fileName, string delFileName, string folderName)
        {
            try
            {
                bool ret = true;
                if (!string.IsNullOrEmpty(fullPath) && !string.IsNullOrEmpty(fileName))
                {
                    // 新しいファイルあり
                    ret = this.PictureFileUpload(fullPath, fileName, delFileName, folderName);
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
        #endregion

        #region Resize

        /// --------------------------------------------------
        /// <summary>
        /// ファイルのリサイズ処理
        /// </summary>
        /// <param name="srcFullPath">元ファイルのフルパス</param>
        /// <param name="dstFullPath">リサイズ後ファイルのフルパス</param>
        /// <param name="ici">コーデック情報</param>
        /// <param name="eps">エンコードパラメータ</param>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ResChange(string srcFullPath, string dstFullPath, ImageCodecInfo ici, EncoderParameters eps)
        {
            // 画像を読み込む
            Bitmap bmpOrg = Bitmap.FromFile(srcFullPath) as Bitmap;

            // 画像解像度を取得する
            float hRes = bmpOrg.HorizontalResolution;
            float vRes = bmpOrg.VerticalResolution;
            float height = bmpOrg.Height;
            float width = bmpOrg.Width;

            if (hRes != ComDefine.RESIZE_HORIZONTAL_RESOLUSION ||
                vRes != ComDefine.RESIZE_VERTICAL_RESOLUSION ||
                width != ComDefine.RESIZE_WIDTH ||
                height != ComDefine.RESIZE_HEIGHT)
            {
                // 画像解像度を変更して新しいBitmapオブジェクトを作成
                Bitmap bmpNew = new Bitmap(ComDefine.RESIZE_WIDTH, ComDefine.RESIZE_HEIGHT);
                bmpNew.SetResolution(ComDefine.RESIZE_HORIZONTAL_RESOLUSION, ComDefine.RESIZE_VERTICAL_RESOLUSION);


                // 新しいBitmapオブジェクトに元の画像内容を描画
                Graphics g = Graphics.FromImage(bmpNew);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmpOrg, 0, 0, ComDefine.RESIZE_WIDTH, ComDefine.RESIZE_HEIGHT);
                g.Dispose();

                bmpNew.Save(dstFullPath, ici, eps);
                bmpNew.Dispose();
            }
            else
            {
                bmpOrg.Save(dstFullPath, ici, eps);
            }
            // 画像リソースを解放
            bmpOrg.Dispose();
        }
    
        /// --------------------------------------------------
        /// <summary>
        /// MimeTypeで指定されたImageCodecInfoを探して返す
        /// </summary>
        /// <param name="mimeType">MIME</param>
        /// <returns></returns>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
            System.Drawing.Imaging.ImageCodecInfo[] encs =
                System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            //指定されたMimeTypeを探して見つかれば返す
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            {
                if (enc.MimeType == mimeType)
                {
                    return enc;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region ファイル(upload)

        /// --------------------------------------------------
        /// <summary>
        /// ファイル upload処理
        /// </summary>
        /// <param name="fileName">アップロードファイル</param>
        /// <param name="delFileName">デリートファイル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="arNo">ARNo</param>
        /// <param name="bukkenNo">物件管理No.</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <returns>true:成功 false:失敗</returns>
        /// <create>H.Tajimi 2018/11/06</create>
        /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        private bool PictureFileUpload(string fullPath, string fileName, string delFileName, string folderName)
        {
            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Open))
                {
                    ConnAttachFile conn = new ConnAttachFile();
                    FileUploadPackage package = new FileUploadPackage();

                    byte[] data = new byte[fs.Length];
                    fs.Position = 0;
                    fs.Read(data, 0, (int)fs.Length);
                    package.FileData = data;
                    package.FileName = fileName;
                    package.DeleteFileName = delFileName;
                    package.FolderName = folderName;
                    package.FileType = FileType.TagPictures;
                    package.GirenType = GirenType.None;

                    FileUploadResult result = conn.FileUpload(package);
                    if (!result.IsSuccess)
                    {
                        // 失敗
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 検索処理

        #region 検索処理実行

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var cond = this.GetCondition();
                var conn = new ConnS01();
                var dsSearch = new DataSet();
                dsSearch.Merge(this._dtSearch);
                var ds = conn.GetTehaiMeisaiDataForIkkatsuUpload(cond, dsSearch);
                if (!ComFunc.IsExistsData(ds, ComDefine.DTTBL_RESULT))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                try
                {
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.DataSource = ds.Tables[ComDefine.DTTBL_RESULT];
                    if (UtilData.ExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                    {
                        this.shtMeisai.ActivePosition = new Position(SHEET_COL_BTN_OPERATION, this.shtMeisai.TopLeft.Row);
                    }
                }
                finally
                {
                    this.shtMeisai.Redraw = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 修正処理
                var dt = (this.shtMeisai.DataSource as DataTable).Copy();
                dt.AcceptChanges();
                var dtIns = this.GetSchemeManageZumenKeishiki();
                dtIns.TableName = ComDefine.DTTBL_INSERT;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME)))
                    {
                        var drIns = dtIns.NewRow();
                        drIns[Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI] = ComFunc.GetFldObject(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI);
                        drIns[Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME] = ComFunc.GetFldObject(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME);
                        dtIns.Rows.Add(drIns);
                    }
                }
                var ds = new DataSet();
                ds.Merge(dtIns);

                // DB登録
                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();
                string errMsgID;
                string[] args;
                DataTable dtZumenKeishiki = null;
                if (!conn.InsManageZumenKeishiki(cond, ds, out dtZumenKeishiki, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        if (ComFunc.IsVersionError(errMsgID))
                        {
                            this.RunSearch();
                        }
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
                }

                // uploadは成否に関わらずtrue固定
                // 画像ファイルupload
                var dtMessage = ComFunc.GetSchemeMultiMessage();
                if (!this.PictureUpdate(dtZumenKeishiki, ref dtMessage))
                {
                    if (UtilData.ExistsData(dtMessage))
                    {
                        // ファイルのアップロードに失敗しました。\r\n再度保存して下さい。
                        this.ShowMultiMessage(dtMessage, "A9999999039");
                    }
                    else
                    {
                        // ファイルのアップロードに失敗しました。\r\n再度保存して下さい。
                        this.ShowMessage("A9999999039");
                    }
                    // エラーにはしない
                    // 修正完了メッセージ変更
                    this.MsgUpdateEnd = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region ファイル名設定

        /// --------------------------------------------------
        /// <summary>
        /// ファイル名設定
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="fieldName">フィールド名</param>
        /// <param name="errMsgID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2019/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool SetFileName(DataTable dt, int rowIndex, string fieldName, out string errMsgID, out string[] args)
        {
            Func<string, object> fncGetFileName = (fileName) =>
            {
                object ret = DBNull.Value;
                if (!string.IsNullOrEmpty(fileName))
                {
                    ret = fileName + PICTURE_EXT;
                }
                return ret;
            };

            errMsgID = string.Empty;
            args = null;
            var dr = dt.Rows[rowIndex];
            var tmpFileName = fncGetFileName(ComFunc.GetFld(dr, fieldName));
            if (tmpFileName != DBNull.Value)
            {
                if (!this.ExistsFile(tmpFileName.ToString()))
                {
                    // {0}行目のファイルが存在しません。
                    errMsgID = "S0100040007";
                    args = new string[] { (rowIndex + 1).ToString() };
                    return false;
                }
            }
            dr[Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME] = tmpFileName;
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイル名クリア
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>H.Tajimi 2019/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ClearFileName(DataTable dt, int rowIndex)
        {
            var dr = dt.Rows[rowIndex];
            dr[Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME] = string.Empty;
            return true;
        }

        #endregion

        #region ボタンエディタ取得

        /// --------------------------------------------------
        /// <summary>
        /// ボタンエディタ取得
        /// </summary>
        /// <param name="isInit">初期状態かどうか</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected ButtonEditor GetButtonEditor(OperationType type)
        {
            if (type == OperationType.Upload)
            {
                return ElTabelleSheetHelper.NewButtonEditor(Resources.IkkatsuUpload_Upload);
            }
            else
            {
                return ElTabelleSheetHelper.NewButtonEditor(Resources.IkkatsuUpload_Cancel);
            }
        }

        #endregion

        #region ファイル存在確認

        /// --------------------------------------------------
        /// <summary>
        /// ファイル存在確認
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExistsFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return true;
            }

            var filePath = Path.Combine(this.txtPicture.Text, fileName);
            if (!File.Exists(filePath))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region スキーム(図番/型式管理データ)

        /// --------------------------------------------------
        /// <summary>
        /// 図番/型式管理データ
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeManageZumenKeishiki()
        {
            var dt = new DataTable(Def_T_MANAGE_ZUMEN_KEISHIKI.Name);
            dt.Columns.Add(Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI, typeof(object));
            dt.Columns.Add(Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME, typeof(object));
            dt.Columns.Add(Def_T_MANAGE_ZUMEN_KEISHIKI.SAVE_DIR, typeof(object));
            dt.Columns.Add(Def_T_MANAGE_ZUMEN_KEISHIKI.CREATE_DATE, typeof(object));
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 表示データ取得用スキーマ取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/08/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeDispData()
        {
            var dt = new DataTable(ComDefine.DTTBL_RESULT);
            dt.Columns.Add(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, typeof(object));
            dt.Columns.Add(ComDefine.FLD_MB_ZUMEN_KEISHIKI, typeof(object));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI_JP, typeof(object));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HINMEI, typeof(object));
            dt.Columns.Add(ComDefine.FLD_EXISTS_PICTURE, typeof(object));
            dt.Columns.Add(Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME, typeof(object));
            return dt;
        }

        #endregion

        #region DataTableクリア

        /// --------------------------------------------------
        /// <summary>
        /// DataTableクリア
        /// </summary>
        /// <create>H.Tajimi 2019/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CleanFilesData()
        {
            if (this._dtSearch != null)
            {
                this._dtSearch.Dispose();
                this._dtSearch = null;
            }
        }

        #endregion

        #region イベント

        #region ファンクションバー

        /// --------------------------------------------------
        /// <summary>
        /// F01ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.shtMeisai.EditState) return;
                this.ClearMessage();
                base.fbrFunction_F01Button_Click(sender, e);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F06ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
                // 検索条件のロック解除
                this.grpSearch.Enabled = true;
                // ファンクションボタンの切替
                this.ChangeFunctionButton(false);
                // フォーカス移動
                this.btnStart.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F07ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタン

        #region 参照ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnReference_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var frm = new FolderBrowserDialog())
                {
                    frm.Description = Resources.IkkatsuUpload_PictureFolderDescription;
                    frm.ShowNewFolderButton = false;
                    frm.SelectedPath = this.txtPicture.Text;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.txtPicture.Text = frm.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/11/05</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.SheetClear();
                if (!this.RunSearch())
                {
                    this.btnReference.Focus();
                    return;
                }
                // 検索条件のロック
                this.grpSearch.Enabled = false;
                this.shtMeisai.Enabled = true;
                this.ChangeFunctionButton(true);
                this.shtMeisai.Focus();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion

        #endregion

        #region グリッド

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// セルのイベントが発生したときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/07/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                int rowIndex = e.Position.Row;
                int colIndex = e.Position.Column;
                var cellEditor = this.shtMeisai.CellEditor as ButtonEditor;
                if (cellEditor != null)
                {
                    // セルのイベント処理
                    switch (e.Name)
                    {
                        case CellNotifyEvents.Click:
                            this.ClearMessage();
                            switch (e.Position.Column)
                            {
                                case SHEET_COL_BTN_OPERATION:
                                    var dt = this.shtMeisai.DataSource as DataTable;
                                    if (dt == null || dt.Rows.Count < rowIndex)
                                    {
                                        return;
                                    }

                                    string errMsgId;
                                    string[] args;
                                    if (cellEditor.Text == Resources.IkkatsuUpload_Upload)
                                    {
                                        // ファイル名設定
                                        if (!this.SetFileName(dt, rowIndex, ComDefine.FLD_MB_ZUMEN_KEISHIKI, out errMsgId, out args))
                                        {
                                            this.ShowMessage(errMsgId, args);
                                            return;
                                        }
                                        this.shtMeisai[colIndex, rowIndex].Editor = this.GetButtonEditor(OperationType.Cancel);
                                        // 一度他にフォーカスを遷移しないとボタンテキスト名は変更されない
                                        this.fbrFunction.F12Button.Focus();
                                        Application.Idle += new EventHandler(Application_Idle_ChangeButtonText);
                                    }
                                    else
                                    {
                                        // ファイル名クリア
                                        if (this.ClearFileName(dt, rowIndex))
                                        {
                                            this.shtMeisai[colIndex, rowIndex].Editor = this.GetButtonEditor(OperationType.Upload);
                                            // 一度他にフォーカスを遷移しないとボタンテキスト名は変更されない
                                            this.fbrFunction.F12Button.Focus();
                                            Application.Idle += new EventHandler(Application_Idle_ChangeButtonText);
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        
        #endregion

        #endregion

        #region アプリケーションアイドル

        /// --------------------------------------------------
        /// <summary>
        /// ボタンテキスト名変更
        /// </summary>
        /// <remarks>
        /// 処理が落ち着いた頃にフォーカスを戻す
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        void Application_Idle_ChangeButtonText(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(Application_Idle_ChangeButtonText);
            this.shtMeisai.Focus();
        }
        
        #endregion

        #endregion
    }
}
