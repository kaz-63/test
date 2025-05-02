using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報Help
    /// </summary>
    /// <create>Y.Higuchi 2010/07/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ARJohoHelp : SystemBase.Forms.CustomOrderForm
    {
        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoHelp(UserInfo userInfo)
            : base(userInfo, ComDefine.TITLE_A0100030)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // アイコン変更
                this.Icon = ComFunc.BitmapToIcon(ComResource.Help);

                this.picHelp1.Image = ARHelpFile.GetARList();
                this.picHelp2.Image = ARHelpFile.GetARTouroku();

                // サイズ調整
                this.AjustSize();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region イベント

        #region 閉じるボタン

        /// --------------------------------------------------
        /// <summary>
        ///閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region サイズ調整

        /// --------------------------------------------------
        /// <summary>
        /// サイズ調整
        /// </summary>
        /// <create>Y.Higuchi 2010/07/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private void AjustSize()
        {
            // 画像の最大サイズを取得
            int imageHeight = this.picHelp1.Height;
            int imageWidth = this.picHelp1.Width;
            if (imageHeight < this.picHelp2.Height)
            {
                imageHeight = this.picHelp2.Height;
            }
            if (imageWidth < this.picHelp2.Width)
            {
                imageWidth = this.picHelp2.Width;
            }
            // 差分からフォームのサイズを求める。
            Size formSize = new Size(imageWidth, imageHeight);
            formSize.Height = this.Height + (formSize.Height - tbpARList.Height);
            formSize.Width = this.Width + (formSize.Width - tbpARList.Width);

            // 最大、最小の調整
            // 高さ
            if (formSize.Height < this.MinimumSize.Height)
            {
                formSize.Height = this.MinimumSize.Height;
                formSize.Width += SystemInformation.VerticalScrollBarWidth;
            }
            if (0 < this.MaximumSize.Height && this.MaximumSize.Height < formSize.Height)
            {
                formSize.Height = this.MaximumSize.Height;
                formSize.Width += SystemInformation.VerticalScrollBarWidth;
            }
            // 幅
            if (formSize.Width < this.MinimumSize.Width)
            {
                formSize.Width = this.MinimumSize.Width;
                formSize.Height += SystemInformation.HorizontalScrollBarHeight;
            }
            if (0 < this.MaximumSize.Width && this.MaximumSize.Width < formSize.Width)
            {
                formSize.Width = this.MaximumSize.Width;
                formSize.Height += SystemInformation.HorizontalScrollBarHeight;
            }
            // スクロールバーの分を再度調整
            // 高さ
            if (formSize.Height < this.MinimumSize.Height)
            {
                formSize.Height = this.MinimumSize.Height;
            }
            if (0 < this.MaximumSize.Height && this.MaximumSize.Height < formSize.Height)
            {
                formSize.Height = this.MaximumSize.Height;
            }
            // 高さ
            if (formSize.Height < this.MinimumSize.Height)
            {
                formSize.Height = this.MinimumSize.Height;
            }
            if (0 < this.MaximumSize.Height && this.MaximumSize.Height < formSize.Height)
            {
                formSize.Height = this.MaximumSize.Height;
            }
            this.Size = formSize;
        }

        #endregion
    }
}
