using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SystemBase.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// SplitContainerにフォーカスがあるとき、閉じるボタンを押すとエラーが出るので対応したクラス
    /// </summary>
    /// <create>R.Katsuo 2010/02/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public class CustomSplitContainer : SplitContainer
    {
        #region Overrides Methods

        /// --------------------------------------------------
        /// <summary>
        /// OnKeyDown
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        /// <create>R.Katsuo 2010/02/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.IsDisposed) return;
            base.OnKeyDown(e);
        }

        /// --------------------------------------------------
        /// <summary>
        /// OnKeyUp
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        /// <create>R.Katsuo 2010/02/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (this.IsDisposed) return;
            base.OnKeyUp(e);
        }

        #endregion
    }
}
