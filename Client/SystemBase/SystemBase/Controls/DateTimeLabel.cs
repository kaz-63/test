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
    /// 現在日時表示ラベルクラス
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [System.Diagnostics.DebuggerStepThrough()]
    public class DateTimeLabel : Label
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// タイマー
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private Timer _timer = null;

        /// --------------------------------------------------
        /// <summary>
        /// フォーマット
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _format = "MM/dd HH:mm";

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public DateTimeLabel()
            : base()
        {
            this.Initialize();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// --------------------------------------------------
        /// <summary>
        /// フォーマット文字列を取得またはまたは設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue("MM/dd HH:mm")]
        public string Format
        {
            get { return this._format; }
            set
            {
                string now = DateTime.Now.ToString(value);
                this.Text = now;
                this._format = value;
            }
        }

        #endregion

        #region Hidden Properties(Override)

        /// --------------------------------------------------
        /// <summary>
        /// 現在のテキストを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テキストの配置方法を取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public override ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// --------------------------------------------------
        /// <summary>
        /// 初期化を行います。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Initialize()
        {
            base.TextAlign = ContentAlignment.MiddleLeft;
            this._timer = new Timer();
            this._timer.Tick += new EventHandler(Timer_Tick);
            this._timer.Interval = 200;
            this._timer.Enabled = true;
        }

        #endregion

        #region Overrides Methods

        /// --------------------------------------------------
        /// <summary>
        ///  使用されているリソースを解放します。  
        /// </summary>
        /// <param name="disposing"></param>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion

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
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Text = DateTime.Now.ToString(this.Format);
        }

        #endregion

    }
}
