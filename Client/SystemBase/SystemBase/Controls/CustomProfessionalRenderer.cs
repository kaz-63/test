using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SystemBase.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// Microsoft Officeの表示要素に使用する色を拡張したクラス
    /// </summary>
    /// <create>R.Katsuo 2010/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CustomProfessionalRenderer : ProfessionalColorTable
    {
        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomProfessionalRenderer()
            : base()
        {
        }

        #endregion

        /// --------------------------------------------------
        /// <summary>
        /// ToolStrip の背景で使用するグラデーションの開始色を取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color ToolStripGradientBegin
        {
            get
            {
                //return Color.FromArgb(167, 159, 193);
                return Color.FromArgb(65, 58, 138);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ToolStrip の背景で使用するグラデーションの中間色を取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color ToolStripGradientMiddle
        {
            get
            {
                return Color.FromArgb(65, 58, 138);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ToolStrip の背景で使用するグラデーションの終了色を取得します。 
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color ToolStripGradientEnd
        {
            get
            {
                //return Color.FromArgb(167, 159, 193);
                return Color.FromArgb(65, 58, 138);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// StatusStrip で使用するグラデーションの開始色を取得します。 
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color StatusStripGradientBegin
        {
            get
            {
                return Color.FromArgb(65, 58, 138);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// StatusStrip で使用するグラデーションの終了色を取得します。 
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color StatusStripGradientEnd
        {
            get
            {
                return Color.FromArgb(65, 58, 138);
                //return Color.FromArgb(167, 159, 193);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ToolStripPanel で使用するグラデーションの開始色を取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color ToolStripPanelGradientBegin
        {
            get
            {
                return Color.Gold;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ToolStripPanel で使用するグラデーションの終了色を取得します。
        /// </summary>
        /// <create>R.Katsuo 2010/01/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public override Color ToolStripPanelGradientEnd
        {
            get
            {
                return Color.Ivory;
            }
        }
    }
}
