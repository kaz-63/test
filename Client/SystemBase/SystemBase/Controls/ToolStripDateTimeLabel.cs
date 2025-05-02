using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SystemBase.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// ToolStripにDateTimeLabelを表示するクラス
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public partial class ToolStripDateTimeLabel : ToolStripControlHost
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public ToolStripDateTimeLabel()
            : base(new DateTimeLabel())
        {
            this.HostControl.BackColor = Color.Transparent;
        }

        #endregion

        #region Properties

        #region Private Properties

        /// --------------------------------------------------
        /// <summary>
        /// DateTimeLabelを取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTimeLabel HostControl
        {
            get { return (DateTimeLabel)this.Control; }
        }

        #endregion

        #region Public Properties

        /// --------------------------------------------------
        /// <summary>
        /// コントロールの境界線スタイルを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(typeof(BorderStyle), "None")]
        public BorderStyle LabelBorderStyle
        {
            get { return this.HostControl.BorderStyle; }
            set { this.HostControl.BorderStyle = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 日付フォーマットを取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue("MM/dd HH:mm")]
        [Localizable(true)]
        public String LabelFormat
        {
            get { return this.HostControl.Format; }
            set { this.HostControl.Format = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// テキストの配置方法を取得または設定します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public ContentAlignment LabelTextAlign
        {
            get { return this.HostControl.TextAlign; }
            set { this.HostControl.TextAlign = value; }
        }

        #endregion

        #region Hidden Properties(Override)

        /// --------------------------------------------------
        /// <summary>
        /// コントロールに表示されるテキストを取得または設定します。
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
            set { }
        }

        #endregion

        #endregion
    }
}
