using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;

namespace ActiveReportsHelper
{
    /// --------------------------------------------------
    /// <summary>
    /// プレビュー用プリンタ設定
    /// </summary>
    /// <create>Y.Higuchi 2010/07/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public class PreviewPrinterSetting
    {
        #region Fields

        // 用紙サイズ
        private PaperKind _paperKind = PaperKind.A4;
        // 印刷方向-横
        private bool _landscape = false;

        #endregion
        
        #region Properties

        #region 用紙サイズ

        /// --------------------------------------------------
        /// <summary>
        /// 用紙サイズ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public PaperKind PaperKind
        {
            get { return this._paperKind; }
            set { this._paperKind = value; }
        }

        #endregion

        #region 印刷方向-横

        /// --------------------------------------------------
        /// <summary>
        /// 印刷方向-横(true:横/false:縦)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool Landscape
        {
            get { return this._landscape; }
            set { this._landscape = value; }
        }

        #endregion
        
        #endregion
    }
}
