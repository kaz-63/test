using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

using WsConnection.WebRefI02;
using System.Globalization;

namespace SMS.I02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ImportProgress : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 取込ファイルタイプ
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ImportFileType
        {
            Location,
            Kanryo,
            Tanaoroshi,
            Shkkakonpo,
            Other,
        }

        #endregion

        #region Fields

        private string _tempID = string.Empty;
        private DataTable _dtMessage = null;

        #endregion

        #region 定数

        private const int HANDY_DRIVE_NO = 2;
        private const int LENGTH_TAG_NO = 10;
        private const int LENGTH_BOX_NO = 6;
        private const int LENGTH_PALLET_NO = 6;

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示用デリゲート
        /// </summary>
        /// <param name="msgType">MessageImageType</param>
        /// <param name="message">メッセージ</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateSetMessage(MessageImageType msgType, string message);

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="message">Enabled状態</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateChangeFunctionButton(bool isEnabled);

        /// --------------------------------------------------
        /// <summary>
        /// DB接続/PC名取得等の初期化処理用デリゲート
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateStartImport();

        /// --------------------------------------------------
        /// <summary>
        /// フォームクローズ用デリゲート
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateFormClose();

        private delegate void DelegateShowError(string text, Exception exception);

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public ImportProgress(UserInfo userInfo)
            : base(userInfo, ComDefine.TITLE_I0200011)
        {
            InitializeComponent();
            if (DesignMode) return;
            // イベント追加
            Application.Idle += new EventHandler(Application_Idle);
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public ImportProgress(UserInfo userInfo, string tempID)
            : this(userInfo)
        {
            InitializeComponent();
            this._tempID = tempID;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                this.ChangeFunctionButton(false);
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                // 取込時にErrorがありました。確認して下さい。
                ComFunc.AddMultiMessage(dtMessage, "I0200010008", null);
                // Handy接続【 CradleType：{0}、PortNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010009", null);
                // Handy接続に失敗しました。再度取込を行って下さい。【 CradleType：{0}、PortNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010010", null);
                // Filesの取得に失敗しました。再度取込を行って下さい。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010011", null);
                // Filesを取得しました。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010012", null);
                // Filesの取込に失敗しました。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010013", null);
                // 取込めるDataが存在しませんでした。【 CradleType：{0}、PortNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010014", null);
                // Handy切断【 CradleType：{0}、PortNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "I0200010015", null);
                // 取込処理開始
                ComFunc.AddMultiMessage(dtMessage, "I0200010016", null);
                // Handyが準備されていません。Handyが準備されている場合は再度取込を行って下さい。
                ComFunc.AddMultiMessage(dtMessage, "I0200010017", null);
                // 取込処理終了
                ComFunc.AddMultiMessage(dtMessage, "I0200010018", null);
                // 取込処理異常終了
                ComFunc.AddMultiMessage(dtMessage, "I0200010019", null);
                // 取込Menuの選択を間違っています。確認して下さい。【 CradleType：{0}、PortNo：{1}】
                ComFunc.AddMultiMessage(dtMessage, "I0200010020", null);

                // Location FilesにPalletNo.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010021", null);
                // Location FilesのPalletNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010022", null);
                // Location FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010023", null);
                // 完了FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010024", null);
                // 棚卸FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010025", null);
                // Location FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010026", null);
                // 完了FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】

                ComFunc.AddMultiMessage(dtMessage, "I0200010027", null);
                // 棚卸FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010028", null);
                // Location Filesに重複した在庫No.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010029", null);
                // 完了Filesに重複した在庫No.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010030", null);
                // 棚卸Filesに重複した在庫No.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010031", null);
                // 棚卸FilesにPalletNo.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                ComFunc.AddMultiMessage(dtMessage, "I0200010035", null);

                // メッセージ取得
                DataSet ds = this.GetMultiMessage(dtMessage, "");
                this._dtMessage = ds.Tables[ComDefine.DTTBL_MULTIRESULT].Copy();

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
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.lsvProgress.Focus();
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
        /// 画面クリア処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.lsvProgress.Items.Clear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region イベント

        #region アプリケーション

        /// --------------------------------------------------
        /// <summary>
        /// アプリケーションが処理を完了し、アイドル状態に入ろうとするタイミングで発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Application_Idle(object sender, EventArgs e)
        {
            // イベント削除
            Application.Idle -= new EventHandler(Application_Idle);

            // デリゲートで初期化処理を実行
            DelegateStartImport startImport = new DelegateStartImport(this.StartImport);
            startImport.BeginInvoke(null, null);
        }

        #endregion

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region ステータスメッセージ設定

        /// --------------------------------------------------
        /// <summary>
        /// ステータスメッセージ設定
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <param name="args">パラメーター</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetProgressMessage(string messageID, params object[] args)
        {
            string notFoundMsg = ComDefine.MSG_NOTFOUND_TEXT + "【" + messageID + "】";
            try
            {
                string msg = string.Empty;
                MessageImageType msgType = MessageImageType.None;
                DataRow[] drs = this._dtMessage.Select("MESSAGE_ID = " + UtilConvert.PutQuot(messageID));
                // 取得失敗
                if (drs.Length == 0)
                {
                    msg = notFoundMsg;
                }
                else
                {
                    // メッセージとアイコンを設定
                    DataRow dr = drs[0];
                    msgType = this.GetMessageImageType(ComFunc.GetFld(dr, Def_M_MESSAGE.MESSAGE_LEVEL));
                    msg = ComFunc.GetFld(dr, Def_M_MESSAGE.MESSAGE, notFoundMsg);
                    msg = FormatMessage(msg, ConvertArrayObjectToString(args));
                }
                this.SetProgressMessage(msgType, msg);
            }
            catch
            {
                this.SetProgressMessage(MessageImageType.Error, notFoundMsg);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// リストにメッセージを追加
        /// </summary>
        /// <param name="msgType">イメージの種類</param>
        /// <param name="message">メッセージ</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetProgressMessage(MessageImageType msgType, string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new DelegateSetMessage(this.SetProgressMessage), new object[] { msgType, message });
                    return;
                }
                int imageWidth = this.imlImage.ImageSize.Width;
                int imageIndex = this.imlImage.Images.IndexOfKey(msgType.ToString());
                this.lsvProgress.Items.Insert(0, message, imageIndex);
                Graphics graphic = this.lsvProgress.CreateGraphics();
                SizeF size = graphic.MeasureString(message, this.lsvProgress.Font);
                int stringWidth = UtilConvert.ToInt32(UtilConvert.Round4Float(size.Width, 0, AdjustFormat.Celling));
                stringWidth += this.lsvProgress.Margin.Left + this.lsvProgress.Margin.Right + imageWidth;
                if (this.colImportProgress.Width < stringWidth)
                {
                    this.colImportProgress.Width = stringWidth;
                }
            }
            catch { }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Object配列をString配列に変換
        /// </summary>
        /// <param name="args">Object配列</param>
        /// <returns>String配列</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] ConvertArrayObjectToString(params object[] args)
        {
            try
            {
                string[] stringArgs = null;
                if (args != null)
                {
                    stringArgs = new string[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        stringArgs[i] = args[i].ToString();
                    }
                }
                return stringArgs;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateChangeFunctionButton(this.ChangeFunctionButton), new object[] { isEnabled });
                return;
            }
            this.fbrFunction.F12Button.Enabled = isEnabled;
        }

        #endregion

        #region フォームクローズ

        /// --------------------------------------------------
        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void FormClose()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateFormClose(this.FormClose), null);
                return;
            }
            this.Close();
        }

        #endregion

        #region 取込処理

        /// --------------------------------------------------
        /// <summary>
        /// 取込処理
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void StartImport()
        {
            ComFunc.SetThreadCultureInfo(this.UserInfo);
            bool isError = false;
            try
            {
                int handyNum = 0;
                List<HandyFileInfo> targetFileList = new List<HandyFileInfo>();
                string baseFolder = Path.Combine(Application.StartupPath, ComDefine.IMPORT_BUHIN_TEMP_BASE_DIR + Path.DirectorySeparatorChar);
                this.ImportTempDirDeleteAll(baseFolder);
                // ハンディは最後に\が無いとフォルダとして認識しない。
                string tempFolder = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                string recvFolder = System.IO.Path.Combine(baseFolder, tempFolder + Path.DirectorySeparatorChar);
                // 取込処理開始
                this.SetProgressMessage("I0200010016");

                using (HandyBT bt = new HandyBT())
                try
                {
                    // 本番時:true, テスト時:false
#if DEBUG
                    bool isTest = true;
#else
                    bool isTest = false;
#endif

                    if (isTest)
                    {
                        handyNum = 1;
                        if (!Directory.Exists(recvFolder))
                            Directory.CreateDirectory(recvFolder);
                        for (int i = 0; i < 3; i++)
			            {
                            HandyFileInfo hf = new HandyFileInfo();
                            hf.CradleType = "SAMPLE";
                            hf.PortNo = 1;
                            hf.Drive = @"c:\sms\HandyData\";
                            hf.FileNameWithoutExt = "GENPIN";
                            if (i == 0)
                                hf.Ext =ComDefine.HANDY_LOCATION_FILE_EXT;
                            else if (i == 1)
                                hf.Ext =ComDefine.HANDY_KANRYO_FILE_EXT;
                            else if (i == 2)
                                hf.Ext =ComDefine.HANDY_TANAOROSHI_FILE_EXT;
                            string hfn = hf.GetHandyFileName();
                            string lfn = Path.Combine(recvFolder, hf.GetLocalFileName());
                            if (File.Exists(hfn))
                            {
                                File.Copy(hfn, lfn, true);
                                targetFileList.Add(hf);
                            }
                        }
                    }
                    else
                    {
                        handyNum = bt.SearchHandy();
                        for (int i = 0; i < handyNum; i++)
                        {
                            if (!this.HandyConnect(bt, recvFolder))
                            {
                                isError = true;
                                break;
                            }
                            // ハンディからファイル取得
                            List<HandyFileInfo> getFileList;
                            if (GetHandyFiles(bt, out getFileList))
                            {
                                // 取込んだファイルがあったかチェック
                                if (getFileList.Count == 0)
                                {
                                    // 取込めるDataが存在しませんでした。【 CradleType：{0}、PortNo：{1} 】
                                    this.SetProgressMessage("I0200010014", bt.CradleType, bt.PortNo);
                                    isError = true;
                                }
                                else
                                {
                                    // ハンディからファイルを削除
                                    this.DeleteHandyFiles(bt, getFileList);
                                    targetFileList.AddRange(getFileList.ToArray());
                                }
                            }
                            else
                            {
                                isError = true;
                                // ファイル取得で失敗があったので次のハンディへ
                                continue;
                            }
                            // Handy切断【 CradleType：{0}、PortNo：{1} 】
                            this.SetProgressMessage("I0200010015", bt.CradleType, bt.PortNo);
                            bt.Disconnect();
                        }
                    }

                    if (0 < targetFileList.Count)
                    {
                        DataSet dsTemp;
                        if (!this.CreateImportTempworkData(targetFileList, recvFolder, out dsTemp))
                        {
                            isError = true;
                        }
                        // 何れかのTEMPWORKにデータがあれば取込処理を行う。
                        if (ComFunc.IsExistsData(dsTemp, Def_T_BUHIN_TEMPWORK.Name))
                        {
                            DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                            ConnI02 conn = new ConnI02();
                            CondI02 cond = new CondI02(this.UserInfo);

                            // 棚卸日付はクライアント側日付で登録する。
                            cond.InventDate = DateTime.Today.ToString("yyyy/MM/dd");
                            if (!conn.ImportData(cond, dsTemp, ref dtMessage))
                            {
                                if (0 < dtMessage.Rows.Count)
                                {
                                    // 処理に失敗しました。
                                    this.ShowMultiMessage(dtMessage, "I0200010007");
                                }
                                else
                                {
                                    // 取込時にErrorがありました。確認して下さい。
                                    this.SetProgressMessage("I0200010008", null);
                                }
                                isError = true;
                            }
                        }
                    }
                }
                catch (Exception impEx)
                {
                    this.ShowErrorThread(ComDefine.MSG_ERROR_TEXT, impEx);
                    // 取込処理異常終了
                    this.SetProgressMessage("I0200010019", null);
                    try
                    {
                        bt.Disconnect();
                    }
                    catch { }
                    isError = true;
                }
                finally
                {
                    this.ImportTempDirDelete(recvFolder);
                }
                if (handyNum == 0)
                {
                    // Handyが準備されていません。Handyが準備されている場合は再度取込を行って下さい。
                    this.SetProgressMessage("I0200010017", null);
                    isError = true;
                }
                // 取込処理終了
                this.SetProgressMessage("I0200010018");
            }
            catch (Exception ex)
            {
                this.ShowErrorThread(ComDefine.MSG_ERROR_TEXT, ex);
                // 取込処理異常終了
                this.SetProgressMessage("I0200010019", null);
                isError = true;
            }
            finally
            {
                if (isError)
                {
                    this.ChangeFunctionButton(true);
                }
                else
                {
                    this.FormClose();
                }
            }
        }

        #region ハンディ関係

        #region ハンディ接続

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ接続
        /// </summary>
        /// <param name="bt">ハンディクラス</param>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <returns>接続状態</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool HandyConnect(HandyBT bt, string recvFolder)
        {
            try
            {
                // 接続前にハンディの情報を取得する必要がある。
                bt.GetHandy();
                // Handy接続【 CradleType：{0}、PortNo：{1} 】
                this.SetProgressMessage("I0200010009", bt.CradleType, bt.PortNo);
                if (!bt.Connect(recvFolder))
                {
                    // Handy接続に失敗しました。再度取込を行って下さい。【 CradleType：{0}、PortNo：{1} 】
                    this.SetProgressMessage("I0200010010", bt.CradleType, bt.PortNo);
                    bt.Disconnect();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディからファイルを取得

        /// --------------------------------------------------
        /// <summary>
        /// ハンディからファイルを取得
        /// </summary>
        /// <param name="bt">ハンディクラス</param>
        /// <param name="getFileList">取得したファイルのリスト</param>
        /// <returns>true:取得時エラーなし/false:取得時エラー有り</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetHandyFiles(HandyBT bt, out List<HandyFileInfo> getFileList)
        {
            try
            {
                getFileList = new List<HandyFileInfo>();
                // ファイルのリストを取得
                List<HandyFileInfo> fileList = bt.GetFileNames(HANDY_DRIVE_NO);
                // 取込用にファイルのリストを退避
                foreach (var item in fileList)
                {
                    ImportFileType ift = this.GetImportFileType(item);
                    if (ift == ImportFileType.Shkkakonpo)
                    {
                        // 取込Menuの選択を間違っています。確認して下さい。【 CradleType：{0}、PortNo：{1} 】
                        this.SetProgressMessage("I0200010020", item.CradleType, item.PortNo);
                        return false;
                    }
                    if (ift == ImportFileType.Other) continue;
                    if (!bt.GetFile(item))
                    {
                        // Filesの取得に失敗しました。再度取込を行って下さい。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                        this.SetProgressMessage("I0200010011", item.CradleType, item.PortNo, item.GetHandyFileName());
                        return false;
                    }
                    // Filesを取得しました。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                    this.SetProgressMessage("I0200010012", item.CradleType, item.PortNo, item.GetHandyFileName());
                    getFileList.Add(item.Clone());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region ハンディからファイル削除

        /// --------------------------------------------------
        /// <summary>
        /// ハンディからファイル削除
        /// </summary>
        /// <param name="bt500">ハンディクラス</param>
        /// <param name="fileList">削除ファイルのリスト</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DeleteHandyFiles(HandyBT bt, List<HandyFileInfo> fileList)
        {
            try
            {
                foreach (var item in fileList)
                {
                    if (this.GetImportFileType(item) == ImportFileType.Other) continue;
                    bt.RemoveFile(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 一時取込データ作成

        /// --------------------------------------------------
        /// <summary>
        /// 一時取込データ作成
        /// </summary>
        /// <param name="targetFileList">対象ファイルリスト</param>
        /// <param name="recvFolder">受信フォルダ</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CreateImportTempworkData(List<HandyFileInfo> targetFileList, string recvFolder, out DataSet dsTemp)
        {
            try
            {
                bool isError = false;
                dsTemp = new DataSet();
                dsTemp.Tables.Add(this.GetSchemeTempwork(Def_T_BUHIN_TEMPWORK.Name));
                dsTemp.Tables.Add(this.GetSchemeTempworkMeisai(Def_T_BUHIN_TEMPWORK_MEISAI.Name));
                // 対象ファイルがあれば取込
                foreach (var item in targetFileList)
                {
                    ImportFileType fileType = this.GetImportFileType(item);
                    if (!this.CreateImportData(fileType, recvFolder, item, ref dsTemp))
                    {
                        isError = true;
                    }
                }
                return !isError;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// 現品集荷ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="fileType">ファイルタイプ</param>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CreateImportData(ImportFileType fileType, string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_BUHIN_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // Filesの取込に失敗しました。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                    this.SetProgressMessage("I0200010013", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                bool isError = false;
                int rowNo = 0;

                string torikomiFlag = string.Empty;
                if (fileType == ImportFileType.Location)
                    torikomiFlag = ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1;
                else if (fileType == ImportFileType.Kanryo)
                    torikomiFlag = ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1;
                else
                    torikomiFlag = ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        DateTime workDate = DateTime.Now;
                        string workerId = string.Empty;
                        string location = string.Empty;
                        string stockNo = string.Empty;

                        if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1 || torikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                        {
                            // Location登録ファイル一時取込、棚卸登録ファイル一時取込
                            workDate = DateTime.Parse(GetSplitValue(split, 0, string.Empty));
                            workerId = GetSplitValue(split, 1, string.Empty).Trim();
                            location = GetSplitValue(split, 2, string.Empty).Trim();
                            stockNo = GetSplitValue(split, 3, string.Empty).Trim();
                        }
                        else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                        {
                            // 完了登録ファイル一時取込
                            workDate = DateTime.Parse(GetSplitValue(split, 0, string.Empty));
                            workerId = GetSplitValue(split, 1, string.Empty).Trim();
                            stockNo = GetSplitValue(split, 2, string.Empty).Trim();
                        }
                        // 桁数チェック
                        if (stockNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
                        {
                            if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                            {
                                // 完了FilesにPalletNo.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3} 】
                                this.SetProgressMessage("I0200010021", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                isError = true;
                                break;
                            }
                            else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                            {
                                // 棚卸FilesにPalletNo.が存在する為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                this.SetProgressMessage("I0200010035", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                isError = true;
                                break;
                            }
                            else if (stockNo.Length != LENGTH_PALLET_NO)
                            {
                                // Location FilesのPalletNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                this.SetProgressMessage("I0200010022", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                isError = true;
                                break;
                            }
                        }
                        else if (stockNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
                        {
                            if (stockNo.Length != LENGTH_BOX_NO)
                            {
                                if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1)
                                {
                                    // Location FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010023", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                                {
                                    // 完了FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010035", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                                {
                                    // 棚卸FilesのBoxNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010025", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                isError = true;
                                break;
                            }
                        }
                        else
                        {
                            if (stockNo.Length != LENGTH_TAG_NO)
                            {
                                if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1)
                                {
                                    // Location FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010026", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                                {
                                    // 完了FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010027", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                                {
                                    // 棚卸FilesのTagNo.の桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files：{2}、StockNo：{3}  】
                                    this.SetProgressMessage("I0200010028", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName(), stockNo);
                                }
                                isError = true;
                                break;
                            }
                        }
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID] = workerId;
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG] = torikomiFlag;
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.WORK_DATE] = workDate;
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION] = location;
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO] = stockNo;
                        // 取り敢えずOKにしておく。
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取り込みデータ作成
                        DataView dv = dtTempworkMeisai.DefaultView;
                        dv.Sort = Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID + ", " +
                            Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION + "," +
                            Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO;
                        DataTable dt = dv.ToTable(true, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID, Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION,
                            Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);

                        DataRow dr = dtTempworkMeisai.Rows[0];

                        // 一時取込データ作成
                        DataRow drNew = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        drNew[Def_T_BUHIN_TEMPWORK.TEMP_ID] = fileInfo.GetLocalFileName();
                        drNew[Def_T_BUHIN_TEMPWORK.WORK_USER_ID] = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID);
                        drNew[Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG] = torikomiFlag;
                        drNew[Def_T_BUHIN_TEMPWORK.LOCATION] = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION);
                        drNew[Def_T_BUHIN_TEMPWORK.STOCK_NO] = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);
                        drNew[Def_T_BUHIN_TEMPWORK.ERROR_NUM] = 0;
                        drNew[Def_T_BUHIN_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(drNew);
                    }
                    if (dsTemp.Tables.Contains(Def_T_BUHIN_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_BUHIN_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_BUHIN_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_BUHIN_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
                    }
                }
                return !isError;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 取込一時フォルダ全削除

        /// --------------------------------------------------
        /// <summary>
        /// 取込一時フォルダ全削除(サブフォルダを削除)
        /// </summary>
        /// <param name="dirPath">取込のベースフォルダパス</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ImportTempDirDeleteAll(string dirPath)
        {
            try
            {
                foreach (var childPath in Directory.GetDirectories(dirPath))
                {
                    this.ImportTempDirDelete(childPath);
                }
            }
            catch { }
        }

        #endregion

        #region 取込一時フォルダ指定削除

        /// --------------------------------------------------
        /// <summary>
        /// 取込一時フォルダ指定削除
        /// </summary>
        /// <param name="dirPath">フォルダパス</param>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ImportTempDirDelete(string dirPath)
        {
            try
            {
                Directory.Delete(dirPath, true);
            }
            catch { }
        }

        #endregion

        #region 取込ファイルタイプ取得

        /// --------------------------------------------------
        /// <summary>
        /// 取込ファイルタイプ取得
        /// </summary>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <returns>取込ファイルタイプ</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private ImportFileType GetImportFileType(HandyFileInfo fileInfo)
        {
            ImportFileType fileType = ImportFileType.Other;
            try
            {
                if (fileInfo.Ext == ComDefine.HANDY_LOCATION_FILE_EXT)
                {
                    // Location登録ファイル
                    fileType = ImportFileType.Location;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_KANRYO_FILE_EXT)
                {
                    // 完了登録ファイル
                    fileType = ImportFileType.Kanryo;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_TANAOROSHI_FILE_EXT)
                {
                    // 棚卸登録ファイル
                    fileType = ImportFileType.Tanaoroshi;
                }
                else if (fileInfo.FileNameWithoutExt == ComDefine.HANDY_SHUKA_FILENAME ||
                    fileInfo.Ext == ComDefine.HANDY_BOX_FILEEXT ||
                    fileInfo.Ext == ComDefine.HANDY_PALLETEXT ||
                    fileInfo.Ext == ComDefine.HANDY_HIKIWATASHIEXT)
                {
                    // 集荷・梱包のハンディファイル
                    fileType = ImportFileType.Shkkakonpo;
                }
            }
            catch { }
            return fileType;
        }

        #endregion

        #endregion

        #region データテーブル取得

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業データテーブル
        /// </summary>
        /// <param name="tableName">データテーブル名</param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempwork(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.WORK_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.TORIKOMI_DATE, typeof(DateTime));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.LOCATION, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.STOCK_NO, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.ERROR_NUM, typeof(decimal));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK.STATUS_FLAG, typeof(string));
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業明細データテーブル
        /// </summary>
        /// <param name="tableName">データテーブル名</param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempworkMeisai(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, typeof(decimal));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.RESULT, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.WORK_DATE, typeof(DateTime));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO, typeof(string));
            dt.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.DESCRIPTION, typeof(string));
            return dt;
        }

        #endregion

        #region 配列値取得

        /// --------------------------------------------------
        /// <summary>
        /// 配列値の取得
        /// </summary>
        /// <param name="splitValues">配列</param>
        /// <param name="index">インデックス</param>
        /// <param name="defaultValue">初期値</param>
        /// <returns>配列の値</returns>
        /// <create>T.Wakamatsu 2013/08/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetSplitValue(string[] splitValues, int index, string defaultValue)
        {
            try
            {
                if (splitValues.Length <= index)
                {
                    return defaultValue;
                }
                return splitValues[index];
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region エラーメッセージ表示

        private void ShowErrorThread(string message, Exception exception)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateShowError(this.ShowErrorThread), new object[] { message, exception });
                return;
            }
            CustomMsgBoxEx.ShowError(this, message, exception);
        }

        #endregion
    }
}
