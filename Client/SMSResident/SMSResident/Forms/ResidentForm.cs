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
using SMSResident.Controls;

namespace SMSResident.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ResidentForm : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        private bool _isSystemShutDown = false;

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 処理の種類
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ResidenFuncType
        {
            Start = 0,
            Stop = 1,
            Clear = 3,
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        public ResidentForm(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // コントロールの初期化。
                this.IsCloseQuestion = true;
                this.Icon = ComFunc.BitmapToIcon(ComResource.Batch);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.tbcMain.Focus();
                this.CallResidentMonitorFunction(this, ResidenFuncType.Start);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.CallResidentMonitorFunction(this.tbcMain.SelectedTab, ResidenFuncType.Clear);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region イベント

        #region フォーム

        /// --------------------------------------------------
        /// <summary>
        /// FormClosing
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            //if (this._isSystemShutDown)
            //{
            //    // 常駐監視を終了した後、シャットダウンを行って下さい。
            //    this.ShowMessage("J0100010002");
            //    e.Cancel = true;
            //    this._isSystemShutDown = false;
            //}
            //else
            //{
            //    base.OnClosing(e);
            //    if (!e.Cancel)
            //    {
            //        // アプリケーションの終了処理中
            //        this.ShowMessage("J0100010001");
            //        CallResidentMonitorFunction(this, ResidenFuncType.Stop);
            //    }
            //}
            // シャットダウンされても終了要求して終了してしまう。
            if (this._isSystemShutDown)
            {
                this.IsCloseQuestion = false;
            }
            base.OnClosing(e);
            if (!e.Cancel)
            {
                // アプリケーションの終了処理中
                this.ShowMessage("J0100010001");
                CallResidentMonitorFunction(this, ResidenFuncType.Stop);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }
            if (m.Msg == Win32Message.WM_QUERYENDSESSION)
            {
                this._isSystemShutDown = true;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region 常駐処理制御

        /// --------------------------------------------------
        /// <summary>
        /// ResidentMonitorコントロールの処理呼出し
        /// </summary>
        /// <param name="ctrl">親コントロール</param>
        /// <param name="funcType">処理の種類</param>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CallResidentMonitorFunction(Control ctrl, ResidenFuncType funcType)
        {
            foreach (Control target in ctrl.Controls)
            {
                CallResidentMonitorFunction(target, funcType);
                IResidentMonitor monitor = target as IResidentMonitor;
                if (monitor != null)
                {
                    switch (funcType)
                    {
                        case ResidenFuncType.Start:
                            monitor.MonitorStart();
                            break;
                        case ResidenFuncType.Stop:
                            monitor.MonitorStop();
                            break;
                        case ResidenFuncType.Clear:
                            monitor.MessageClear();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
