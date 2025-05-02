using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SystemBase.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// メッセージ表示ラベルクラス
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [DefaultProperty("Message")]
    [System.Diagnostics.DebuggerStepThrough()]
    public partial class MessageLabel : UserControl
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// メッセージアイコンタイプ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private MessageImageType _messageImage = MessageImageType.None;

        /// --------------------------------------------------
        /// <summary>
        /// アイコンの点滅を行う場合は true とする。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isBlink = true;

        /// --------------------------------------------------
        /// <summary>
        /// Timer_Tickのイベントが実行されるまでの間隔
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _blinkTime = 1000;

        /// --------------------------------------------------
        /// <summary>
        /// アイコン点滅時の表示/非表示を切り替えるフラグ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isImageChange = false;

        #endregion

        #region Constructor

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public MessageLabel()
        {
            InitializeComponent();
            Initialize();
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// アイコン点滅を行うか取得または設定します。
        /// true と設定すると点滅を行います。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(true)]
        public bool isBlink
        {
            get { return this._isBlink; }
            set
            {
                this._isBlink = value;
                if (this._isBlink)
                {
                    this.tmrBlink.Enabled = true;
                }
                else
                {
                    this.tmrBlink.Enabled = false;
                    this.SetImage(this.MessageImage, false);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// アイコン点滅までの間隔を取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(1000)]
        public int BlinkTime
        {
            get { return this._blinkTime; }
            set
            {
                if (value < 50)
                {
                    this._blinkTime = 50;
                }
                else
                {
                    this._blinkTime = value;
                }
                    this.tmrBlink.Interval = this._blinkTime;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue("")]
        public string Message
        {
            get { return this.lblMessage.Text; }
            set { this.lblMessage.Text = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// アイコンを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(typeof(MessageImageType), "None")]
        public MessageImageType MessageImage
        {
            get { return this._messageImage; }
            set
            {
                if (this._messageImage == value) return;
                this._messageImage = value;
                this.SetImage(value, true);
            }
        }

        #endregion

        #region Methods

        /// --------------------------------------------------
        /// <summary>
        /// 初期化を行います。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Initialize()
        {
            this.isBlink = true;
            this.BlinkTime = 1000;
        }

        /// --------------------------------------------------
        /// <summary>
        /// アイコンを表示します。
        /// </summary>
        /// <param name="messageImage">アイコンタイプ</param>
        /// <param name="isChangeVisible">アイコン点滅を行う場合は true とする。</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetImage(MessageImageType messageImage, bool isChangeVisible)
        {

            switch (messageImage)
            {
                case MessageImageType.None:
                    picImage.BackgroundImage = null;
                    if (isChangeVisible)
                    {
                        picImage.Visible = false;
                    }
                    break;
                case MessageImageType.Error:
                    picImage.BackgroundImage = imlMessage.Images["Error"];
                    if (isChangeVisible)
                    {
                        picImage.Visible = true;
                    }
                    break;
                case MessageImageType.Information:
                    picImage.BackgroundImage = imlMessage.Images["Information"];
                    if (isChangeVisible)
                    {
                        picImage.Visible = true;
                    }
                    break;
                case MessageImageType.Question:
                    picImage.BackgroundImage = imlMessage.Images["Question"];
                    if (isChangeVisible)
                    {
                        picImage.Visible = true;
                    }
                    break;
                case MessageImageType.Warning:
                    picImage.BackgroundImage = imlMessage.Images["Warning"];
                    if (isChangeVisible)
                    {
                        picImage.Visible = true;
                    }
                    break;
                default:
                    picImage.Image = null;
                    if (isChangeVisible)
                    {
                        picImage.Visible = false;
                    }
                    break;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージをクリアします。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public void ClearMessage()
        {
            this.SetMessage(MessageImageType.None, "");
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージを設定します。
        /// </summary>
        /// <param name="messageImage">アイコンタイプ</param>
        /// <param name="message">メッセージ</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetMessage(MessageImageType messageImage, string message)
        {
            this.MessageImage = messageImage;
            this.Message = message;
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージを追加します。
        /// </summary>
        /// <param name="messageImage">アイコンタイプ</param>
        /// <param name="message">メッセージ</param>
        /// <create>J.Chen 2024/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public void AddMessage(MessageImageType messageImage, string message)
        {
            if (!string.IsNullOrEmpty(this.Message))
            {
                this.Message = this.Message + message;
            }
            else 
            {
                this.MessageImage = messageImage;
                this.Message = message;
            }
        }

        #endregion

        #region Control Events

        /// --------------------------------------------------
        /// <summary>
        /// 指定したタイマの間隔が経過し、タイマが有効である場合に発生します。
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tmrBlink_Tick(object sender, EventArgs e)
        {
            if (this._isImageChange)
            {
                this.SetImage(this.MessageImage, false);
                this._isImageChange = false;
            }
            else
            {
                this.SetImage(MessageImageType.None, false);
                this._isImageChange = true;
            }
        }

        #endregion
    }
}
