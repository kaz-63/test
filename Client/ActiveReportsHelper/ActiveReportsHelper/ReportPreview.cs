using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataDynamics.ActiveReports.Document;
using ActiveReportsHelper.Properties;

namespace ActiveReportsHelper
{
    /// --------------------------------------------------
    /// <summary>
    /// プレビューフォーム
    /// </summary>
    /// <create> 2010/06/30</create>
    /// <update></update>
    /// --------------------------------------------------
    internal partial class ReportPreview : Form
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// カスタムの印刷ボタン
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const long CUSTOM_PRINT_BUTTON = 333;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 印刷したかどうか
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isPrint = false;

        /// --------------------------------------------------
        /// <summary>
        /// 印刷ボタンを表示するかどうか
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _showPrintButton = true;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public ReportPreview()
        {
            InitializeComponent();
            vwrView.TableOfContents.Text = Resources.ReportPreview_TableOfContentsText;
            vwrView.Document.Name = Resources.ReportPreview_DocumentName;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// ビューワコントロールで表示するActiveReportsドキュメント
        /// </summary>
        /// <create> 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public Document Document
        {
            get { return this.vwrView.Document; }
            set { this.vwrView.Document = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷したかどうか
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool IsPrint
        {
            get { return this._isPrint; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷ボタンを表示するかどうか
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public bool ShowPrintButton
        {
            get { return this._showPrintButton; }
            set { this._showPrintButton = value; }
        }

        #endregion

        #region Events

        #region フォーム

        /// --------------------------------------------------
        /// <summary>
        /// ロード時のイベント
        /// </summary>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // 標準の印刷ボタンを削除します。
            this.vwrView.Toolbar.Tools.RemoveAt(2);

            if (this.ShowPrintButton)
            {
                // カスタムボタンを追加
                DataDynamics.ActiveReports.Toolbar.Button btn = new DataDynamics.ActiveReports.Toolbar.Button();
                btn.Caption = Resources.ReportPreview_Print;
                btn.ToolTip = Resources.ReportPreview_Print;
                btn.ImageIndex = 1;
                btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.TextAndIcon;
                btn.Id = CUSTOM_PRINT_BUTTON;
                this.vwrView.Toolbar.Tools.Insert(2, btn);
            }
            else
            {
                this.vwrView.Toolbar.Tools.RemoveAt(2);
            }
        }

        #endregion

        #region ビューアー

        /// --------------------------------------------------
        /// <summary>
        /// ツールバーのアイコンがクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void vwrView_ToolClick(object sender, DataDynamics.ActiveReports.Toolbar.ToolClickEventArgs e)
        {
            if (e.Tool.Id == CUSTOM_PRINT_BUTTON)
            {
                this.PrintReport();
            }
        }

        #endregion

        #endregion

        #region 印刷

        /// --------------------------------------------------
        /// <summary>
        /// 印刷処理
        /// </summary>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void PrintReport()
        {
            try
            {
                using (PrintDialog pd = new PrintDialog())
                {
                    // 印刷ダイアログの設定
                    pd.AllowSomePages = true;
                    pd.Document = this.vwrView.Document.Printer;
                    pd.PrinterSettings.FromPage = this.vwrView.ReportViewer.CurrentPage;
                    pd.PrinterSettings.ToPage = this.vwrView.ReportViewer.CurrentPage;
                    if (pd.ShowDialog() == DialogResult.OK)
                    {
                        this._isPrint = true;
                        this.vwrView.Document.Print(false, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
