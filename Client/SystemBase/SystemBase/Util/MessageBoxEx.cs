using DSWUtil;
using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SystemBase.Properties;

namespace SystemBase.Util
{
    internal partial class MessageBoxEx : Form
    {
        #region Fields

        #region 定数
        private const int DETAIL_HEIGHT = 150;
        private const int ICON_WIDTH = 38;
        private const int NO_LABEL_HEIGHT = 120;
        private const int NO_LABEL_WIDTH = 30;
        private const int MARGIN_WIDTH = 10;
        private const int MARGIN_HEIGHT = 10;
        #endregion

        private DialogResult _dlgResult = DialogResult.Cancel;
        private bool _isNoClose = true;
        private bool _isClick = false;
        private bool _isErrorMode = false;
        private Exception _exception;
        private string _helpFilePath;
        private HelpNavigator _navigator;
        private object _param;
        private long _timeoutSecond;
        private long _tickCnt = 0;
        private MessageBoxDefaultButton _defaultButton;
        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <param name="isErrorMode">エラーモードかどうか</param>
        /// <param name="exception">例外情報</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        internal MessageBoxEx(IWin32Window owner, string text, string caption, bool showDetail, string detail, int timeoutSecond,
                                MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton,
                                bool showHelpButton, string helpFilePath, HelpNavigator navigator, object param, bool isErrorMode,Exception exception)
            : base()
        {
            InitializeComponent();
            this.Initialize(owner, text, caption, showDetail, detail, timeoutSecond, buttons, icon, defaultButton, showHelpButton, helpFilePath, navigator, param, isErrorMode,exception);
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="text">メッセージボックスに表示するテキスト。</param>
        /// <param name="caption">メッセージボックスのタイトルバーに表示するテキスト。</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="detail">メッセージボックスの詳細に表示するテキスト。</param>
        /// <param name="timeoutSecond">メッセージボックスを自動で閉じる時間(秒)</param>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <param name="defaultButton">メッセージボックスの既定のボタンを指定するMessageBoxDefaultButton値の1つ。</param>
        /// <param name="showHelpButton">メッセージボックスの[ヘルプ]ボタンを表示するかどうか。</param>
        /// <param name="helpFilePath">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプファイルのパスと名前。</param>
        /// <param name="navigator">HelpNavigator 値の 1 つ。</param>
        /// <param name="param">ユーザーが[ヘルプ]ボタンをクリックしたときに表示されるヘルプ トピックの数値 ID。</param>
        /// <param name="isErrorMode">エラーモードかどうか</param>
        /// <param name="exception">例外情報</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void Initialize(IWin32Window owner, string text, string caption, bool showDetail, string detail, int timeoutSecond,
                                            MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton,
                                            bool showHelpButton, string helpFilePath, HelpNavigator navigator, object param, bool isErrorMode, Exception exception)
        {
            //メッセージ設定
            this.lblMainMessage.Text = text;
            //タイトル設定
            this.Text = caption;
            //詳細設定
            this.btnDetail.Visible = showDetail;
            this.txtDetail.Visible = showDetail;
            this.txtDetail.Text = detail;
            if (showDetail == false)
            {
                this.MinimumSize = new System.Drawing.Size(0, 0);
            }
            //タイムアウト設定
            this._timeoutSecond = timeoutSecond * 10;
            if (0 < this._timeoutSecond)
            {
                this.tmrClose.Enabled = true;
            }
            //ヘルプ設定
            this.btnHelp.Visible = showHelpButton;
            this._helpFilePath = helpFilePath;
            this._navigator = navigator;
            this._param = param;
            //デフォルトボタン
            this._defaultButton = defaultButton;
            //エラーモード設定
            this._exception = exception;
            this._isErrorMode = isErrorMode;
            if (isErrorMode == false)
            {
                SaveFileToolStripMenuItem.Visible = false;
            }
            else
            {
                this.ContextMenuStrip = cmsDetail;
                if (exception != null)
                {
                    if (detail.Equals(string.Empty))
                    {
                        this.txtDetail.Text = this.GetMessageInfoErrorExceptionData();
                    }
                }
            }

            //ボタンの初期化
            this.InitializeSetButtons(buttons);
            //アイコンの初期化
            this.InitializeSetIcon(icon);
            //親ウィンドウやラベルのサイズ等の設定
            this.InitializeSetWindow(owner, showDetail, icon);
        }

        /// --------------------------------------------------
        /// <summary>
        /// ウィンドウポジション、ラベルの最大サイズの設定
        /// </summary>
        /// <param name="owner">モーダルダイアログボックスを所有するIWin32Window</param>
        /// <param name="showDetail">メッセージボックスの詳細を表示するかどうか。</param>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeSetWindow(IWin32Window owner, bool showDetail, MessageBoxIcon icon)
        {
            Rectangle rect;
            Form ownerForm = owner as Form;
            this.Owner = ownerForm;
            if (ownerForm != null)
            {
                this.StartPosition = FormStartPosition.CenterParent;
                rect = Screen.GetWorkingArea(ownerForm);
            }
            else
            {

                this.StartPosition = FormStartPosition.CenterScreen;
                rect = Screen.GetWorkingArea(this);
            }
            rect.Height -= (MARGIN_HEIGHT + NO_LABEL_HEIGHT);
            rect.Width -= (MARGIN_WIDTH + NO_LABEL_WIDTH);
            if (!icon.Equals(MessageBoxIcon.None))
            {
                rect.Width -= ICON_WIDTH;
            }
            if (showDetail == true)
            {
                rect.Height -= DETAIL_HEIGHT;
            }
            //ラベルの最大サイズ
            this.lblMainMessage.MaximumSize = new Size(rect.Width, rect.Height);
        }

        #region ボタンの初期化

        /// --------------------------------------------------
        /// <summary>
        /// ボタンの初期化
        /// </summary>
        /// <param name="buttons">メッセージボックスに表示されるボタンを指定するMessageBoxButtons値の1つ。</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeSetButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    this._isNoClose = true;
                    this._dlgResult = DialogResult.Ignore;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Abort;
                    this.btnOne.DialogResult = DialogResult.Abort;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    this.btnTwo.Text = Resources.MessageBoxEx_Retry;
                    this.btnTwo.DialogResult = DialogResult.Retry;
                    this.btnTwo.Visible = true;
                    //第三ボタン
                    this.btnThree.Text = Resources.MessageBoxEx_Ignore;
                    this.btnThree.DialogResult = DialogResult.Ignore;
                    this.btnThree.Visible = true;
                    break;
                case MessageBoxButtons.OK:
                    this._isNoClose = false;
                    this._dlgResult = DialogResult.OK;
                    this.CancelButton = this.btnOne;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Ok;
                    this.btnOne.DialogResult = DialogResult.OK;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    this.btnTwo.Text = "";
                    this.btnTwo.DialogResult = DialogResult.None;
                    this.btnTwo.Visible = false;
                    //第三ボタン
                    this.btnThree.Text = "";
                    this.btnThree.DialogResult = DialogResult.None;
                    this.btnThree.Visible = false;
                    break;
                case MessageBoxButtons.OKCancel:
                    this._isNoClose = false;
                    this._dlgResult = DialogResult.Cancel;
                    this.CancelButton = this.btnTwo;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Ok;
                    this.btnOne.DialogResult = DialogResult.OK;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    // 2011/02/28 K.Tsutsumi Change カタカナ禁止
                    //this.btnTwo.Text = "キャンセル";
                    this.btnTwo.Text = Resources.MessageBoxEx_Cancel;
                    // ↑
                    this.btnTwo.DialogResult = DialogResult.Cancel;
                    this.btnTwo.Visible = true;
                    //第三ボタン
                    this.btnThree.Text = "";
                    this.btnThree.DialogResult = DialogResult.None;
                    this.btnThree.Visible = false;
                    break;
                case MessageBoxButtons.RetryCancel:
                    this._isNoClose = false;
                    this._dlgResult = DialogResult.Cancel;
                    this.CancelButton = this.btnTwo;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Retry;
                    this.btnOne.DialogResult = DialogResult.Retry;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    // 2011/02/28 K.Tsutsumi Change カタカナ禁止
                    //this.btnTwo.Text = "キャンセル";
                    this.btnTwo.Text = Resources.MessageBoxEx_Cancel;
                    // ↑
                    this.btnTwo.DialogResult = DialogResult.Cancel;
                    this.btnTwo.Visible = true;
                    //第三ボタン
                    this.btnThree.Text = "";
                    this.btnThree.DialogResult = DialogResult.None;
                    this.btnThree.Visible = false;
                    break;
                case MessageBoxButtons.YesNo:
                    this._isNoClose = true;
                    this._dlgResult = DialogResult.No;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Yes;
                    this.btnOne.DialogResult = DialogResult.Yes;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    this.btnTwo.Text = Resources.MessageBoxEx_No;
                    this.btnTwo.DialogResult = DialogResult.No;
                    this.btnTwo.Visible = true;
                    //第三ボタン
                    this.btnThree.Text = "";
                    this.btnThree.DialogResult = DialogResult.None;
                    this.btnThree.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    this._isNoClose = false;
                    this._dlgResult = DialogResult.Cancel;
                    this.CancelButton = this.btnThree;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Yes;
                    this.btnOne.DialogResult = DialogResult.Yes;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    this.btnTwo.Text = Resources.MessageBoxEx_No;
                    this.btnTwo.DialogResult = DialogResult.No;
                    this.btnTwo.Visible = true;
                    //第三ボタン
                    // 2011/02/28 K.Tsutsumi Change カタカナ禁止
                    //this.btnThree.Text = "キャンセル";
                    this.btnThree.Text = Resources.MessageBoxEx_Cancel;
                    // ↑
                    this.btnThree.DialogResult = DialogResult.Cancel;
                    this.btnThree.Visible = true;
                    break;
                default:
                    this._isNoClose = false;
                    this._dlgResult = DialogResult.OK;
                    this.CancelButton = this.btnOne;
                    //第一ボタン
                    this.btnOne.Text = Resources.MessageBoxEx_Ok;
                    this.btnOne.DialogResult = DialogResult.OK;
                    this.btnOne.Visible = true;
                    //第二ボタン
                    this.btnTwo.Text = "";
                    this.btnTwo.DialogResult = DialogResult.None;
                    this.btnTwo.Visible = false;
                    //第三ボタン
                    this.btnThree.Text = "";
                    this.btnThree.DialogResult = DialogResult.None;
                    this.btnThree.Visible = false;
                    break;
            }
        }

        #endregion

        #region アイコンの初期化

        /// --------------------------------------------------
        /// <summary>
        /// アイコンの初期化
        /// </summary>
        /// <param name="icon">メッセージボックスに表示されるアイコンを指定するMessageBoxIcon値の1つ。</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeSetIcon(MessageBoxIcon icon)
        {
            if (icon.Equals(MessageBoxIcon.Asterisk) || icon.Equals(MessageBoxIcon.Information))
            {
                picIcon.Image = SystemBase.Properties.Resources.Infomation.ToBitmap();
            }
            else if (icon.Equals(MessageBoxIcon.Error) || icon.Equals(MessageBoxIcon.Hand) || icon.Equals(MessageBoxIcon.Stop))
            {
                picIcon.Image = SystemBase.Properties.Resources.Error.ToBitmap();
            }
            else if (icon.Equals(MessageBoxIcon.Exclamation) || icon.Equals(MessageBoxIcon.Warning))
            {
                picIcon.Image = SystemBase.Properties.Resources.Warning.ToBitmap();
            }
            else if (icon.Equals(MessageBoxIcon.Question))
            {
                picIcon.Image = SystemBase.Properties.Resources.Question.ToBitmap();
            }
            else
            {
                picIcon.Visible = false;
            }
        }

        #endregion

        #endregion

        #region デフォルトボタンのフォーカス

        /// --------------------------------------------------
        /// <summary>
        /// フォーカスの設定
        /// </summary>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetDefaultButton()
        {
            switch (this._defaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    this.btnOne.Focus();
                    break;
                case MessageBoxDefaultButton.Button2:
                    if (this.btnTwo.Visible == true)
                    {
                        this.btnTwo.Focus();
                    }
                    else
                    {
                        this.btnOne.Focus();
                    }
                    break;
                case MessageBoxDefaultButton.Button3:
                    if (this.btnTwo.Visible == true)
                    {
                        this.btnThree.Focus();
                    }
                    else
                    {
                        this.btnOne.Focus();
                    }
                    break;
                default:
                    this.btnOne.Focus();
                    break;
            }
        }

        #endregion

        #region Override Methods

        /// --------------------------------------------------
        /// <summary>
        /// コントロールの作成時に必要な情報をカプセル化します。
        /// </summary>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (this._isNoClose == true)
                {
                    cp.ClassStyle = cp.ClassStyle | Win32Message.CS_NOCLOSE;
                }
                return cp;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Closingイベントを発生させます。
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this._isClick == false)
            {
                this.DialogResult = this._dlgResult;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Shownイベントを発生させます。
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.SetDefaultButton();
        }

        #endregion

        #region 情報取得

        /// --------------------------------------------------
        /// <summary>
        /// 出力メッセージ情報取得
        /// </summary>
        /// <returns>メッセージ情報</returns>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetMessageInfo()
        {
            if (this._isErrorMode == true)
            {
                return this.GetMessageInfoError();
            }
            else
            {
                return this.GetMessageInfoNormal();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 通常用メッセージ情報
        /// </summary>
        /// <returns>メッセージ情報</returns>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetMessageInfoNormal()
        {
            return this.txtDetail.Text;
        }

        /// --------------------------------------------------
        /// <summary>
        /// エラー用メッセージ情報
        /// </summary>
        /// <returns>メッセージ情報</returns>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetMessageInfoError()
        {
            StringBuilder sb = new StringBuilder();

            //アプリケーション情報
            sb.AppendLine(Resources.MessageBoxEx_InfoErrorAppInfoTitle);
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorAppProductName, Application.ProductName).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorAppProductVersion, Application.ProductVersion).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorAppImageRuntimeVersion, Assembly.GetExecutingAssembly().ImageRuntimeVersion).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorAppImageRuntimeEnvironmentVersion, System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()).AppendLine();
            //OS情報
            DSWUtil.Entity.EntityOSInfo osInfo = DSWUtil.UtilSystem.GetOSInfo();
            sb.AppendLine(Resources.MessageBoxEx_InfoErrorOSInfoTitle);
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorOSInfo, osInfo).AppendLine();
            //ユーザー情報
            DSWUtil.Entity.EntityUserInfo userInfo = DSWUtil.UtilSystem.GetUserInfo(true);
            sb.AppendLine(Resources.MessageBoxEx_InfoErrorUserInfoTitle);
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoComputerName, userInfo.MachineName).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoDomainName, userInfo.DomainName).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoUserName, userInfo.UserName).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoUserFullName, userInfo.UserFullName).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoIsAdministrator, userInfo.IsAdministratorRole.ToString()).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoIsPowerUser, userInfo.IsPowerUserRole.ToString()).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoIsUser, userInfo.IsUserRole.ToString()).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoHostName, DSWUtil.UtilNet.GetHostName()).AppendLine();
            string[] ipAddressList = DSWUtil.UtilNet.GetHostIPAddressString();
            if (ipAddressList != null)
            {
                for (int i = 0; i < ipAddressList.Length; i++)
                {
                    sb.AppendFormat(Resources.MessageBoxEx_InfoErrorUserInfoIpAddress, i, ipAddressList[i]).AppendLine();
                }
            }
            //エラー情報
            sb.AppendLine(Resources.MessageBoxEx_InfoErrorExceptionTitle);
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorExceptionMessage, this.lblMainMessage.Text).AppendLine();
            sb.AppendFormat(Resources.MessageBoxEx_InfoErrorExceptionDetail, this.txtDetail.Text).AppendLine();
            sb.AppendLine(this.GetMessageInfoErrorExceptionData());
            return sb.ToString();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 例外情報取得        /// </summary>
        /// <returns>例外情報</returns>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string GetMessageInfoErrorExceptionData()
        {
            StringBuilder sb = new StringBuilder();

            if (this._exception != null)
            {
                sb.AppendFormat(Resources.MessageBoxEx_ExceptionMessage, this._exception.Message).AppendLine();
                sb.AppendFormat(Resources.MessageBoxEx_ExceptionSource, this._exception.Source).AppendLine();
                sb.AppendFormat(Resources.MessageBoxEx_ExceptionStackTrace, this._exception.StackTrace).AppendLine();
            }
            else
            {
                sb.AppendLine(Resources.MessageBoxEx_ExceptionNone);
            }

            return sb.ToString();
        }

        #endregion

        #region エラーファイル

        /// --------------------------------------------------
        /// <summary>
        /// エラーファイルの生成
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SaveErrorFile(string filePath)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.GetEncoding(UtilEncoding.SHIFT_JIS)))
                {
                    sw.Write(GetMessageInfo());
                }
            }
            catch
            {
                // 2011/02/28 K.Tsutsumi Change カタカナ禁止
                //MessageBox.Show(this,"ファイルの作成に失敗しました。","エラー",MessageBoxButtons.OK,MessageBoxIcon.Error);
                MessageBox.Show(this, Resources.MessageBoxEx_SaveErrorMsgDescription, Resources.MessageBoxEx_SaveErrorMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                // ↑
            }
        }

        #endregion

        #region Control Events

        #region Button

        /// --------------------------------------------------
        /// <summary>
        /// 詳細ボタンクリック時のイベント
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDetail_Click(object sender, EventArgs e)
        {
            txtDetail.Visible = !txtDetail.Visible;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 第一ボタン～第三ボタンクリック時のイベント
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClickClose(object sender, EventArgs e)
        {
            this._isClick = true;
            this.Close();
        }

        /// --------------------------------------------------
        /// <summary>
        /// ヘルプボタンクリック時のイベント
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (this._param == null)
            {
                Help.ShowHelp(this, this._helpFilePath, this._navigator);
            }
            else
            {
                Help.ShowHelp(this, this._helpFilePath, this._navigator, this._param);
            }
        }

        #endregion

        #region Timer

        /// --------------------------------------------------
        /// <summary>
        /// 指定したタイマの間隔が経過し、タイマが有効である場合に発生します。
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tmrClose_Tick(object sender, EventArgs e)
        {
            if (this._timeoutSecond <= ++this._tickCnt)
            {
                tmrClose.Enabled = false;
                this.Close();
            }
        }

        #endregion

        #region ToolStripMenuItem

        /// --------------------------------------------------
        /// <summary>
        /// クリップボードにコピー
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.GetMessageInfo(),true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// ファイルに保存
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>Y.Higuchi 2008/12/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sfdErrorFile.FileName = string.Format(Resources.MessageBoxEx_SaveFileName, DateTime.Now);
            if (DialogResult.OK == sfdErrorFile.ShowDialog(this))
            {
                this.SaveErrorFile(sfdErrorFile.FileName);
            }
        }

        #endregion

        #endregion

    }
}