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

using WsConnection.WebRefK04;

namespace SMS.K04.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <create>Y.Higuchi 2010/08/18</create>
    /// <update>K.Tsutsumi 2019/08/29 ハンディ操作履歴対応</update>
    /// --------------------------------------------------
    public partial class ImportProgress : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 取込ファイルタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
        /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
        /// --------------------------------------------------
        private enum ImportFileType
        {
            Hikiwatashi,
            Shuka,
            Box,
            Pallet,
            Genpin,
            Other,
            Measure,
            Kenpin,
        }

        #endregion

        #region Fields

        private string _tempID = string.Empty;
        private DataTable _dtMessage = null;

        #endregion

        #region 定数

        private const int HANDY_DRIVE_NO = 2;
        private const int LENGTH_GENPINTAGNO = 10;
        private const int LENGTH_BOX_NO = 6;
        private const int LENGTH_PALLET_NO = 6;
        private const int LENGTH_TEHAI_NO = 8;
        private const int LENGTH_NYUKA_NUM = 6;
        private const int LENGTH_WEIGHT = 7;
        private const int LENGTH_USER_ID = 20;  // 作業者コード
        private const decimal ERR_WEIGHT_VALUE = decimal.MinValue;
        private const decimal ERR_NYUKA_NUM_VALUE = decimal.MinValue;

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示用デリゲート
        /// </summary>
        /// <param name="msgType">MessageImageType</param>
        /// <param name="message">メッセージ</param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateSetMessage(MessageImageType msgType, string message);

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="message">Enabled状態</param>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateChangeFunctionButton(bool isEnabled);

        /// --------------------------------------------------
        /// <summary>
        /// DB接続/PC名取得等の初期化処理用デリゲート
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateStartImport();

        /// --------------------------------------------------
        /// <summary>
        /// フォームクローズ用デリゲート
        /// </summary>
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public ImportProgress(UserInfo userInfo)
            : base(userInfo, ComDefine.TITLE_K0400011)
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>T.SASAYAMA 2023/07/14</update> 引渡ファイル関連のメッセージ追加
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                this.ChangeFunctionButton(false);
                DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                // 取込時にエラーがありました。確認して下さい。
                ComFunc.AddMultiMessage(dtMessage, "K0400010032", null);
                // ハンディ接続【 クレードルタイプ：{0}、ポートNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010033", null);
                // ハンディ接続に失敗しました。再度取込を行ってください。【 クレードルタイプ：{0}、ポートNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010034", null);
                // ファイルの取得に失敗しました。再度取込を行ってください。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010035", null);
                // ファイルを取得しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010036", null);
                // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010037", null);
                // 現品集荷ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010038", null);
                // 現品集荷ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010039", null);
                // Box梱包ファイルのBoxNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010040", null);
                // Box梱包ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010041", null);
                // Box梱包ファイルのBoxNo.が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010042", null);
                // Box梱包ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010043", null);
                // パレット梱包ファイルのパレットNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010044", null);
                // パレット梱包ファイルのBoxNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010045", null);
                // パレット梱包ファイルのパレットNo.が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010046", null);
                // パレット梱包ファイルに重複したBoxNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010047", null);
                // 取り込めるデータが存在しませんでした。【 クレードルタイプ：{0}、ポートNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010048", null);
                // ハンディ切断【 クレードルタイプ：{0}、ポートNo：{1} 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010049", null);
                // 取込処理開始
                ComFunc.AddMultiMessage(dtMessage, "K0400010050", null);
                // ハンディが準備されていません。
                ComFunc.AddMultiMessage(dtMessage, "K0400010051", null);
                // 取込処理終了
                ComFunc.AddMultiMessage(dtMessage, "K0400010052", null);
                // 取込処理異常終了
                ComFunc.AddMultiMessage(dtMessage, "K0400010053", null);
                // 取込Ｍｅｎｕの選択を間違っています。確認して下さい。【 クレードルタイプ：{0}、ポートNo：{1}】
                ComFunc.AddMultiMessage(dtMessage, "K0400010055", null);
                // 検品ファイルの手配No.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010058", null);
                // 検品ファイルの入荷数の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010059", null);
                // 検品ファイルの入荷数が数値ではない為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010060", null);
                // 検品ファイルの入荷数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010061", null);
                // 検品ファイルに重複した手配No.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010062", null);
                // 計測ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010063", null);
                // 計測ファイルの重量の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010064", null);
                // 計測ファイルの重量が数値ではない為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010065", null);
                // 計測ファイルの重量が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010066", null);
                // 計測ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010067", null);
                // 引渡Filesの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010077", null);
                // 引渡Filesに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010078", null);
                // 引渡Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010079", null);
                // 現品集荷Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010039", null);
                // Box梱包Files荷Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010072", null);
                // Pallet梱包Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010073", null);
                // 検品Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010074", null);
                // 計測Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                ComFunc.AddMultiMessage(dtMessage, "K0400010075", null);
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/19</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
        /// <create>Y.Higuchi 2010/08/19</create>
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
        /// <create>Y.Higuchi 2010/08/19</create>
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
        /// <create>Y.Higuchi 2010/08/24</create>
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
                string baseFolder = Path.Combine(Application.StartupPath, ComDefine.IMPORT_TEMP_BASE_DIR + Path.DirectorySeparatorChar);
                this.ImportTempDirDeleteAll(baseFolder);
                // ハンディは最後に\が無いとフォルダとして認識しない。
                string tempFolder = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                string recvFolder = System.IO.Path.Combine(baseFolder, tempFolder + Path.DirectorySeparatorChar);
                // 取込処理開始
                this.SetProgressMessage("K0400010050");

                using(HandyBT500 bt500 = new HandyBT500())
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
                        var targetFolder = @"c:\sms\HandyData\";
                        var files = Directory.GetFiles(targetFolder);
                        foreach (var file in files)
                        {
                            
                            HandyFileInfo hf = new HandyFileInfo();
                            hf.CradleType = "SAMPLE";
                            hf.PortNo = 1;
                            hf.Drive = targetFolder;
                            hf.FileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                            hf.Ext = Path.GetExtension(file).Substring(1);
                            
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
                        handyNum = bt500.SearchHandy();
                        for (int i = 0; i < handyNum; i++)
                        {
                            if (!this.HandyConnect(bt500, recvFolder))
                            {
                                isError = true;
                                break;
                            }
                            // ハンディからファイル取得
                            List<HandyFileInfo> getFileList;
                            if (GetHandyFiles(bt500, out getFileList))
                            {
                                // 取込んだファイルがあったかチェック
                                if (getFileList.Count == 0)
                                {
                                    // 取込めるデータが存在しませんでした。【 クレードルタイプ：{0}、ポートNo：{1} 】
                                    this.SetProgressMessage("K0400010048", bt500.CradleType, bt500.PortNo);
                                    isError = true;
                                }
                                else
                                {
                                    // ハンディからファイルを削除
                                    this.DeleteHandyFiles(bt500, getFileList);
                                    getFileList.Sort(this.FileListSort);
                                    targetFileList.AddRange(getFileList.ToArray());
                                }
                            }
                            else
                            {
                                isError = true;
                                // ファイル取得で失敗があったので次のハンディへ
                                continue;
                            }
                            // ハンディ切断【 クレードルタイプ：{0}、ポートNo：{1} 】
                            this.SetProgressMessage("K0400010049", bt500.CradleType, bt500.PortNo);
                            bt500.Disconnect();
                        }
                    }
                    if (0 < targetFileList.Count)
                    {
                        DataSet dsTemp;
                        if (!this.CreateImporotTempworkData(targetFileList, recvFolder, out dsTemp))
                        {
                            isError = true;
                        }
                        // 何れかのTEMPWORKにデータがあれば取込処理を行う。
                        if (ComFunc.IsExistsData(dsTemp, Def_T_TEMPWORK.Name))
                        {
                            DataTable dtMessage = ComFunc.GetSchemeMultiMessage();
                            ConnK04 conn = new ConnK04();
                            CondK04 cond = new CondK04(this.UserInfo);
                            if (!conn.ImportData(cond, dsTemp, ref dtMessage))
                            {
                                if (0 < dtMessage.Rows.Count)
                                {
                                    // 処理に失敗しました。
                                    this.ShowMultiMessage(dtMessage, "K0400010031");
                                }
                                else
                                {
                                    // 取込時にエラーがありました。確認して下さい。
                                    this.SetProgressMessage("K0400010032", null);
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
                    this.SetProgressMessage("K0400010053", null);
                    try
                    {
                        bt500.Disconnect();
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
                    // ハンディが準備されていません。
                    this.SetProgressMessage("K0400010051", null);
                    isError = true;
                }
                // 取込処理終了
                this.SetProgressMessage("K0400010052");
            }
            catch (Exception ex)
            {
                this.ShowErrorThread(ComDefine.MSG_ERROR_TEXT, ex);
                // 取込処理異常終了
                this.SetProgressMessage("K0400010053", null);
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

        #region ファイル名のソート

        /// --------------------------------------------------
        /// <summary>
        /// ファイル名のソート(集荷→Box→パレットの順にする)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>T.SASAYAMA 2023/07/19 引渡ファイル対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private int FileListSort(HandyFileInfo x, HandyFileInfo y)
        {
            if (x.FileNameWithoutExt == ComDefine.HANDY_HIKIWATASHI_FILENAME)
            {
                if (y.FileNameWithoutExt == ComDefine.HANDY_HIKIWATASHI_FILENAME)
                {
                    // 両方が引き渡しファイルなら0を返す
                    return 0;
                }
                // xが引き渡しファイルなら-1を返す
                return -1;
            }
            else if (y.FileNameWithoutExt == ComDefine.HANDY_HIKIWATASHI_FILENAME)
            {
                // yが引き渡しファイルなら1を返す
                return 1;
            }
            else if (x.FileNameWithoutExt == ComDefine.HANDY_SHUKA_FILENAME)
            {
                if (y.FileNameWithoutExt == ComDefine.HANDY_SHUKA_FILENAME)
                {
                    // 両方が集荷ファイルなら0を返す
                    return 0;
                }
                // xが集荷ファイルなら-1を返す
                return -1;
            }
            else if (y.FileNameWithoutExt == ComDefine.HANDY_SHUKA_FILENAME)
            {
                // yが集荷ファイルなら1を返す
                return 1;
            }
            else if (x.Ext == ComDefine.HANDY_BOX_FILEEXT && y.Ext == ComDefine.HANDY_PALLETEXT)
            {
                // xがBox梱包ファイルでyがパレット梱包ファイルなら-1を返す
                return -1;
            }
            else if (x.Ext == ComDefine.HANDY_PALLETEXT && y.Ext == ComDefine.HANDY_BOX_FILEEXT)
            {
                // xがパレット梱包ファイルでyがBox梱包ファイルなら1を返す
                return 1;
            }
            // 集荷ファイルと引き渡しファイル以外で拡張子が同じならファイル名で比較
            return x.FileNameWithoutExt.CompareTo(y.FileNameWithoutExt);
        }

        #endregion

        #region ハンディ関係

        #region ハンディ接続

        /// --------------------------------------------------
        /// <summary>
        /// ハンディ接続
        /// </summary>
        /// <param name="bt500">ハンディクラス</param>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <returns>接続状態</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool HandyConnect(HandyBT500 bt500, string recvFolder)
        {
            try
            {
                // 接続前にハンディの情報を取得する必要がある。
                bt500.GetHandy();
                // ハンディ接続【 クレードルタイプ：{0}、ポートNo：{1} 】
                this.SetProgressMessage("K0400010033", bt500.CradleType, bt500.PortNo);
                if (!bt500.Connect(recvFolder))
                {
                    // ハンディ接続に失敗しました。再度取込を行ってください。【 クレードルタイプ：{0}、ポートNo：{1} 】
                    this.SetProgressMessage("K0400010034", bt500.CradleType, bt500.PortNo);
                    bt500.Disconnect();
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
        /// <param name="bt500">ハンディクラス</param>
        /// <param name="getFileList">取得したファイルのリスト</param>
        /// <returns>true:取得時エラーなし/false:取得時エラー有り</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool GetHandyFiles(HandyBT500 bt500, out List<HandyFileInfo> getFileList)
        {
            try
            {
                getFileList = new List<HandyFileInfo>();
                // ファイルのリストを取得
                List<HandyFileInfo> fileList = bt500.GetFileNames(HANDY_DRIVE_NO);
                // 取込用にファイルのリストを退避
                foreach (var item in fileList)
                {
                    ImportFileType ift = this.GetImportFileType(item);
                    if (ift == ImportFileType.Genpin)
                    {
                        // 取込Ｍｅｎｕの選択を間違っています。確認して下さい。【 クレードルタイプ：{0}、ポートNo：{1}】
                        this.SetProgressMessage("K0400010055", item.CradleType, item.PortNo);
                        return false;
                    }
                    if (ift == ImportFileType.Other) continue;
                    if (!bt500.GetFile(item))
                    {
                        // ファイルの取得に失敗しました。再度取込を行ってください。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                        this.SetProgressMessage("K0400010035", item.CradleType, item.PortNo, item.GetHandyFileName());
                        return false;
                    }
                    // ファイルを取得しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010036", item.CradleType, item.PortNo, item.GetHandyFileName());
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
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DeleteHandyFiles(HandyBT500 bt500, List<HandyFileInfo> fileList)
        {
            try
            {
                foreach (var item in fileList)
                {
                    if (this.GetImportFileType(item) == ImportFileType.Other) continue;
                    bt500.RemoveFile(item);
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
        /// <create>Y.Higuchi 2010/08/19</create>
        /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
        /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
        /// --------------------------------------------------
        private bool CreateImporotTempworkData(List<HandyFileInfo> targetFileList, string recvFolder, out DataSet dsTemp)
        {
            try
            {
                bool isError = false;
                dsTemp = new DataSet();
                dsTemp.Tables.Add(this.GetSchemeTempwork(Def_T_TEMPWORK.Name));
                dsTemp.Tables.Add(this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name));
                // 対象ファイルがあれば取込
                foreach (var item in targetFileList)
                {
                    ImportFileType fileType = this.GetImportFileType(item);
                    switch (fileType)
                    {
                        case ImportFileType.Hikiwatashi:
                            // 引渡ファイル一時取込
                            if (!this.CreateImportTempHikiwatashiData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Shuka:
                            // 現品集荷ファイル一時取込
                            if (!this.CreateImportTempGenpinShukaData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Box:
                            // Box梱包ファイル一時取込
                            if (!this.CreateImportTempBoxData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Pallet:
                            // パレット梱包ファイル一時取込
                            if (!this.CreateImportTempPalletData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Measure:
                            // 計測ファイル一時取込
                            if (!this.CreateImportTempMeasureData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Kenpin:
                            // 検品ファイル一時取込
                            if (!this.CreateImportTempKenpinData(recvFolder, item, ref dsTemp))
                            {
                                isError = true;
                            }
                            break;
                        case ImportFileType.Other:
                            break;
                        default:
                            break;
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

        #region 引渡ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// 引渡ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>T.SASAYAMA 2023/07/14</create>
        /// --------------------------------------------------
        private bool CreateImportTempHikiwatashiData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> tagNoList = new List<string>();
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string tagNo = this.GetSplitValue(split, 0, string.Empty);
                        string handyLoginId = null;

                        // 桁数チェック
                        if (UtilString.GetByteCount(tagNo) != LENGTH_GENPINTAGNO)
                        {
                            // 引渡Filesの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010077", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 1)
                        {
                            handyLoginId = this.GetSplitValue(split, 1, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // 引渡Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010079", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        // 重複チェック
                        if (tagNoList.Contains(tagNo))
                        {
                            // 引渡Filesに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010078", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        tagNoList.Add(tagNo);
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.DATA_NO] = tagNo;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.HIKIWATASHI_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.DATA_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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

        #region 現品集荷ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// 現品集荷ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private bool CreateImportTempGenpinShukaData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> tagNoList = new List<string>();
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string tagNo = this.GetSplitValue(split, 0, string.Empty);
                        string handyLoginId = null;

                        // 桁数チェック
                        if (UtilString.GetByteCount(tagNo) != LENGTH_GENPINTAGNO)
                        {
                            // 現品集荷ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010038", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 1)
                        {
                            handyLoginId = this.GetSplitValue(split, 1, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // 現品集荷Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010071", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        // 重複チェック
                        if (tagNoList.Contains(tagNo))
                        {
                            // 現品集荷ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010039", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        tagNoList.Add(tagNo);
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.DATA_NO] = tagNo;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.SHUKA_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.DATA_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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

        #region Box梱包ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// Box梱包ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private bool CreateImportTempBoxData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> tagNoList = new List<string>();
                string pastBoxNo = string.Empty;
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string boxNo = this.GetSplitValue(split, 0, string.Empty);
                        string tagNo = this.GetSplitValue(split, 1, string.Empty);
                        string handyLoginId = null;

                        // BoxNoの桁数チェック
                        if (UtilString.GetByteCount(boxNo) != LENGTH_BOX_NO)
                        {
                            // Box梱包ファイルのBoxNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010040", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        // 現品TagNoの桁数チェック
                        if (UtilString.GetByteCount(tagNo) != LENGTH_GENPINTAGNO)
                        {
                            // Box梱包ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010041", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 2)
                        {
                            handyLoginId = this.GetSplitValue(split, 2, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // Box梱包Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010072", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        // BoxNo同一チェック
                        if (0 < pastBoxNo.Length && pastBoxNo != boxNo)
                        {
                            // Box梱包ファイルのBoxNo.が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010042", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        // 現品TagNo重複チェック
                        if (tagNoList.Contains(tagNo))
                        {
                            // Box梱包ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010043", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        pastBoxNo = boxNo;
                        tagNoList.Add(tagNo);
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.BOX_NO] = boxNo;
                        dr[Def_T_TEMPWORK_MEISAI.DATA_NO] = tagNo;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.BOX_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.BOX_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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

        #region パレット梱包ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// パレット梱包ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private bool CreateImportTempPalletData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> boxNoList = new List<string>();
                string pastPalletNo = string.Empty;
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string palletNo = this.GetSplitValue(split, 0, string.Empty);
                        string boxNo = this.GetSplitValue(split, 1, string.Empty);
                        string handyLoginId = null;

                        // パレットNoの桁数チェック
                        if (UtilString.GetByteCount(palletNo) != LENGTH_PALLET_NO)
                        {
                            // パレット梱包ファイルのパレットNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010044", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        // BoxNoの桁数チェック
                        if (UtilString.GetByteCount(boxNo) != LENGTH_BOX_NO)
                        {
                            // パレット梱包ファイルのBoxNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010045", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 2)
                        {
                            handyLoginId = this.GetSplitValue(split, 2, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // Pallet梱包Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010073", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        // パレットNo重複チェック
                        if (0 < pastPalletNo.Length && pastPalletNo != palletNo)
                        {
                            // パレット梱包ファイルのパレットNo.が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010046", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        // BoxNo重複チェック
                        if (boxNoList.Contains(boxNo))
                        {
                            // パレット梱包ファイルに重複したBoxNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010047", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        boxNoList.Add(boxNo);
                        pastPalletNo = palletNo;
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.BOX_NO] = boxNo;
                        dr[Def_T_TEMPWORK_MEISAI.PALLET_NO] = palletNo;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.PALLET_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.PALLET_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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

        #region 検品ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// 検品ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private bool CreateImportTempKenpinData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> tehaiNoList = new List<string>();
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string tehaiNo = this.GetSplitValue(split, 0, string.Empty);
                        string nyukaQty = this.GetSplitValue(split, 1, string.Empty);
                        string handyLoginId = null;

                        // 桁数チェック
                        if (UtilString.GetByteCount(tehaiNo) != LENGTH_TEHAI_NO)
                        {
                            // 検品ファイルの手配No.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010058", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (UtilString.GetByteCount(nyukaQty) != LENGTH_NYUKA_NUM)
                        {
                            // 検品ファイルの入荷数の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010059", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 2)
                        {
                            handyLoginId = this.GetSplitValue(split, 2, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // 検品Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010074", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        var nyukaQtyValue = UtilConvert.ToDecimal(nyukaQty, ERR_NYUKA_NUM_VALUE);
                        if (nyukaQtyValue == ERR_NYUKA_NUM_VALUE)
                        {
                            // 検品ファイルの入荷数が数値ではない為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010060", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (nyukaQtyValue < 1)
                        {
                            // 検品ファイルの入荷数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010061", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }

                        // 重複チェック
                        if (tehaiNoList.Contains(tehaiNo))
                        {
                            // 検品ファイルに重複した手配No.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010062", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        tehaiNoList.Add(tehaiNo);
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.TEHAI_NO] = tehaiNo;
                        dr[Def_T_TEMPWORK_MEISAI.NYUKA_QTY] = nyukaQtyValue;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.KENPIN_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.TEHAI_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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

        #region 計測ファイル一時取込データ作成処理

        /// --------------------------------------------------
        /// <summary>
        /// 計測ファイル一時取込データ作成処理
        /// </summary>
        /// <param name="recvFolder">受信フォルダパス</param>
        /// <param name="fileInfo">ハンディのファイル情報</param>
        /// <param name="dsTemp">取込んだデータを格納するDataSet</param>
        /// <returns>true:エラーなし/false:エラー有り</returns>
        /// <create>H.Tajimi 2018/10/25</create>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private bool CreateImportTempMeasureData(string recvFolder, HandyFileInfo fileInfo, ref DataSet dsTemp)
        {
            try
            {
                DataTable dtTempwork = this.GetSchemeTempwork(Def_T_TEMPWORK.Name);
                DataTable dtTempworkMeisai = this.GetSchemeTempworkMeisai(Def_T_TEMPWORK_MEISAI.Name);
                string filePath = Path.Combine(recvFolder, fileInfo.GetLocalFileName());
                if (!File.Exists(filePath))
                {
                    // ファイルの取込に失敗しました。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                    this.SetProgressMessage("K0400010037", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                    return false;
                }
                List<string> tagNoList = new List<string>();
                bool isError = false;
                int rowNo = 0;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        string tagNo = this.GetSplitValue(split, 0, string.Empty);
                        string weight = this.GetSplitValue(split, 1, string.Empty);
                        string handyLoginId = null;
                        // 桁数チェック
                        if (UtilString.GetByteCount(tagNo) != LENGTH_GENPINTAGNO)
                        {
                            // 計測ファイルの現品TagNo.の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010063", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (UtilString.GetByteCount(weight) != LENGTH_WEIGHT)
                        {
                            // 計測ファイルの重量の桁数が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010064", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        if (split.Length > 2)
                        {
                            handyLoginId = this.GetSplitValue(split, 2, null);
                            // 桁数チェック(※作業者コードは、古いバージョンのハンディには存在しないので、存在したときのみチェックとする)
                            if (UtilString.GetByteCount(handyLoginId) != LENGTH_USER_ID)
                            {
                                // 計測Filesの作業者Codeの桁数が不正な為、取込を行えませんでした。【 CradleType：{0}、PortNo：{1}、Files[{2}] 】
                                this.SetProgressMessage("K0400010075", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                                isError = true;
                                break;
                            }
                        }
                        var weightVal = UtilConvert.ToDecimal(weight, ERR_WEIGHT_VALUE);
                        if (weightVal == ERR_WEIGHT_VALUE)
                        {
                            // 計測ファイルの重量が数値ではない為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010065", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        var values = weight.Split('.');
                        if (values.Length != 2 || values[0].Length < 1 || values[1].Trim().Length != 2)
                        {
                            // 計測ファイルの重量が不正な為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010066", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }

                        // 重複チェック
                        if (tagNoList.Contains(tagNo))
                        {
                            // 計測ファイルに重複した現品TagNo.が存在する為、取込を行えませんでした。【 クレードルタイプ：{0}、ポートNo：{1}、ファイル[{2}] 】
                            this.SetProgressMessage("K0400010067", fileInfo.CradleType, fileInfo.PortNo, fileInfo.GetHandyFileName());
                            isError = true;
                            break;
                        }
                        tagNoList.Add(tagNo);
                        rowNo++;
                        // 一時取込明細データ作成
                        DataRow dr = dtTempworkMeisai.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK_MEISAI.ROW_NO] = rowNo;
                        dr[Def_T_TEMPWORK_MEISAI.DATA_NO] = tagNo;
                        dr[Def_T_TEMPWORK_MEISAI.WEIGHT] = weightVal;
                        dr[Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID] = handyLoginId;

                        // 取り敢えずOKにしておく。
                        dr[Def_T_TEMPWORK_MEISAI.RESULT] = RESULT.OK_VALUE1;
                        dtTempworkMeisai.Rows.Add(dr);
                    }
                }
                if (!isError)
                {
                    if (0 < rowNo)
                    {
                        // 一時取込データ作成
                        DataRow dr = dtTempwork.NewRow();
                        // 取込単位ではファイルで一意になるはずなので仮で一時取込IDとする。
                        dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = fileInfo.GetLocalFileName();
                        dr[Def_T_TEMPWORK.TORIKOMI_FLAG] = TORIKOMI_FLAG.MEASURE_VALUE1;
                        dr[Def_T_TEMPWORK.DATA_NO] = ComFunc.GetFld(dtTempworkMeisai, 0, Def_T_TEMPWORK_MEISAI.DATA_NO);
                        dr[Def_T_TEMPWORK.ERROR_NUM] = 0;
                        dr[Def_T_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.MITORIKOMI_VALUE1;
                        dtTempwork.Rows.Add(dr);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK.Name].Merge(dtTempwork);
                    }
                    if (dsTemp.Tables.Contains(Def_T_TEMPWORK_MEISAI.Name))
                    {
                        dsTemp.Tables[Def_T_TEMPWORK_MEISAI.Name].Merge(dtTempworkMeisai);
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
        /// <create>Y.Higuchi 2010/08/12</create>
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
        /// <create>Y.Higuchi 2010/08/12</create>
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
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update>H.Tajimi 2018/10/25 計測、検品対応</update>
        /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
        /// --------------------------------------------------
        private ImportFileType GetImportFileType(HandyFileInfo fileInfo)
        {
            ImportFileType fileType = ImportFileType.Other;
            try
            {
                if (fileInfo.FileNameWithoutExt == ComDefine.HANDY_HIKIWATASHI_FILENAME)
                {
                    // 引渡ファイル
                    fileType = ImportFileType.Hikiwatashi;
                }
                else if (fileInfo.FileNameWithoutExt == ComDefine.HANDY_SHUKA_FILENAME)
                {
                    // 集荷ファイル
                    fileType = ImportFileType.Shuka;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_BOX_FILEEXT)
                {
                    // Box梱包ファイル
                    fileType = ImportFileType.Box;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_PALLETEXT)
                {
                    // パレット梱包ファイル
                    fileType = ImportFileType.Pallet;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_KEISOKUEXT)
                {
                    // 計測ファイル
                    fileType = ImportFileType.Measure;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_KENPINEXT)
                {
                    // 検品ファイル
                    fileType = ImportFileType.Kenpin;
                }
                else if (fileInfo.Ext == ComDefine.HANDY_LOCATION_FILE_EXT ||
                    fileInfo.Ext == ComDefine.HANDY_KANRYO_FILE_EXT ||
                    fileInfo.Ext == ComDefine.HANDY_TANAOROSHI_FILE_EXT)
                {
                    // 現地部品管理ファイル
                    fileType = ImportFileType.Genpin;
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
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempwork(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_TEMPWORK.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.TORIKOMI_DATE, typeof(DateTime));
            dt.Columns.Add(Def_T_TEMPWORK.TORIKOMI_FLAG, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.DATA_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK.ERROR_NUM, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK.STATUS_FLAG, typeof(string));
            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一時作業明細データテーブル
        /// </summary>
        /// <param name="tableName">データテーブル名</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
        /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
        /// --------------------------------------------------
        private DataTable GetSchemeTempworkMeisai(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.TEMP_ID, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.ROW_NO, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.RESULT, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.PALLET_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.BOX_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.DATA_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.DESCRIPTION, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.TEHAI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.NYUKA_QTY, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.WEIGHT, typeof(decimal));
            dt.Columns.Add(Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID, typeof(string));
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
        /// <create>Y.Higuchi 2010/08/18</create>
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
