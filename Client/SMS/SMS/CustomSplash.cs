using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Commons;

namespace SMS
{
    /// --------------------------------------------------
    /// <summary>
    /// 製番用スプラッシュ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CustomSplash : SystemBase.Forms.BaseSplash
    {

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomSplash()
            : base()
        {
            InitializeComponent();
            SystemName = SMS.Properties.Resources.CustomSplash_SystemName;
        }

        #endregion

        #region 初期化処理

        /// --------------------------------------------------
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <create>Y.Higuchi 2010/06/22</create>
        /// <update>D.Okumura 2019/12/04 HELP多言語化対応</update>
        /// --------------------------------------------------
        protected override void InnerInitialize()
        {
            try
            {
                // ARヘルプ画像取得処理
                // 初期化
                ARHelpFile.InitializeHelpDir();

                WsConnection.ConnAttachFile conn = new WsConnection.ConnAttachFile();
                WsConnection.WebRefAttachFile.ARHelpDownloadResult ret = conn.ARHelpDownload(this.UserInfo.Language);

                // ARListHelpファイル
                if (ret.IsExistsARListHelpFile)
                {
                    ARHelpFile.SaveARHelpFile(ret.ARListHelpFileName, ret.ARListHelpFileData);
                }
                // AR情報登録画面Helpファイル
                if (ret.IsExistsARTorokuHelpFile)
                {
                    ARHelpFile.SaveARHelpFile(ret.ARTorokuHelpFileName, ret.ARTorokuHelpFileData);
                }
            }
            catch (Exception)
            {
                // エラーが出ても無視
            }
            base.InnerInitialize();

        }

        #endregion

        #region 再起動処理

        private delegate void ChangeButtonDelegate(bool enabled);

        /// --------------------------------------------------
        /// <summary>
        /// 再起動ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create> 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (!this.LoadComplete)
            {
                Application.Restart();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 1秒後にボタンを押下可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create> 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tmrRestart_Tick(object sender, EventArgs e)
        {
            if (this.tmrRestart != null)
            {
                this.tmrRestart.Stop();
            }
            this.ChangeButton(true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// ボタンを押下可能にする
        /// </summary>
        /// <create> 2012/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeButton(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ChangeButtonDelegate(this.ChangeButton), enabled);
                return;
            }
            this.btnRestart.Enabled = enabled;
        }

        #endregion
    }
}
