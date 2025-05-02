using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using Commons;
using WsConnection;
using DSWUtil;
using SystemBase.Properties;
using WsConnection.WebRefCommon;

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// スプラッシュベースクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BaseSplash : Form
    {

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// グラデーション色１
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color _gradientColor1 = Color.White;

        /// --------------------------------------------------
        /// <summary>
        /// グラデーション色２
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color _gradientColor2 = Color.FromArgb(188, 183, 208);

        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザー情報
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private UserInfo _userInfo = null;

        /// --------------------------------------------------
        /// <summary>
        /// 画面終了フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isClose = false;

        /// --------------------------------------------------
        /// <summary>
        /// 処理中の「・・・・」表示個数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _loadingCnt = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 「・・・・」の最大表示個数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _loadingMaxCnt = 30;

        /// --------------------------------------------------
        /// <summary>
        /// ロード時のカウント文字
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private char _loadingChar = '・';

        /// --------------------------------------------------
        /// <summary>
        /// システム名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _systemName = Application.ProductName;

        #endregion

        #region Delegate

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ表示用デリゲート
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateSetMessage(string message);

        /// --------------------------------------------------
        /// <summary>
        /// DB接続/PC名取得等の初期化処理用デリゲート
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private delegate void DelegateInitialize();

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public BaseSplash()
        {
            InitializeComponent();

            if (DesignMode) return;

            // イベント追加
            Application.Idle += new EventHandler(Application_Idle);
        }

        #endregion

        #region Properties

        #region Protected Properties

        #region グラデーション色

        /// --------------------------------------------------
        /// <summary>
        /// グラデーション色１
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual Color GradientColor1
        {
            get { return this._gradientColor1; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// グラデーション色２
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual Color GradientColor2
        {
            get { return this._gradientColor2; }
        }

        #endregion

        #region 画面終了フラグ

        /// --------------------------------------------------
        /// <summary>
        /// 画面終了フラグ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool IsClose
        {
            get { return this._isClose; }
            set { this._isClose = value; }
        }

        #endregion

        #region 処理中の「・・・」

        /// --------------------------------------------------
        /// <summary>
        /// 処理中の「・・・・」表示個数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual int LoadingCnt
        {
            get { return this._loadingCnt; }
            set { this._loadingCnt = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 「・・・・」の最大表示個数
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual int LoadingMaxCnt
        {
            get { return this._loadingMaxCnt; }
            set { this._loadingMaxCnt = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ロード時のカウント文字
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual char LoadingChar
        {
            get { return this._loadingChar; }
            set { this._loadingChar = value; }
        }

        #endregion

        #region 処理が完了したかどうか

        /// --------------------------------------------------
        /// <summary>
        /// 処理が完了したかどうか
        /// </summary>
        /// <create> 2012/06/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected bool LoadComplete { get; set; }

        #endregion

        #endregion

        #region ログインユーザー情報

        /// --------------------------------------------------
        /// <summary>
        /// ログインユーザー情報
        /// </summary>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        public virtual UserInfo UserInfo
        {
            get { return this._userInfo; }
            protected set { this._userInfo = value; }
        }

        #endregion

        #region システム名

        /// --------------------------------------------------
        /// <summary>
        /// システム名
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string SystemName
        {
            get { return this._systemName; }
            set { this._systemName = value; }
        }

        #endregion

        #region カルチャ

        /// --------------------------------------------------
        /// <summary>
        /// カルチャ
        /// </summary>
        /// <create>H.Tajimi 2018/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        public System.Globalization.CultureInfo CultureInfo { get; set; }
        
        #endregion

        #endregion

        #region Events

        #region フォーム

        /// --------------------------------------------------
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // アプリケーション名
            this.Text = this.SystemName;
            this.lblTitle.Text = this.SystemName;

            // バージョン情報
            this.lblVersion.Text = Resources.BaseSplash_Version + ComFunc.GetAppVersion();

            // 文字列初期化
            this.lblStatus.Text = string.Empty;
            this.lblLoading.Text = string.Empty;

            this.LoadComplete = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 描画イベント
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                // 背景色描画
                using (LinearGradientBrush lgb = new LinearGradientBrush(this.Bounds, this.GradientColor1, this.GradientColor2, LinearGradientMode.Vertical))
                {
                    // ColorBlendクラスを生成
                    ColorBlend cb = new ColorBlend();
                    // 色変化位置
                    cb.Positions = new float[] { 0.0f, 0.3f, 0.7f, 1.0f };
                    // 色指定
                    cb.Colors = new Color[] { this.GradientColor1, this.GradientColor2, this.GradientColor2, this.GradientColor1 };

                    // ブラシのInterpolationColorsに設定 
                    lgb.InterpolationColors = cb;

                    // 塗りつぶし 
                    e.Graphics.FillRectangle(lgb, e.Graphics.VisibleClipBounds);
                }
            }
            catch (Exception)
            {
                // エラーが出ても無視
            }
        }

        #endregion

        #region アプリケーション

        /// --------------------------------------------------
        /// <summary>
        /// アプリケーションが処理を完了し、アイドル状態に入ろうとするタイミングで発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Application_Idle(object sender, EventArgs e)
        {
            // イベント削除
            Application.Idle -= new EventHandler(Application_Idle);

            // タイマー実行
            tmrTimer.Enabled = true;

            // デリゲートで初期化処理を実行
            DelegateInitialize initialize = new DelegateInitialize(this.SystemInitialize);
            initialize.BeginInvoke(null, null);
        }

        #endregion

        #region タイマー

        /// --------------------------------------------------
        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsClose)
            {
                this.Close();
            }

            if (this.LoadingMaxCnt <= this.LoadingCnt)
            {
                this.LoadingCnt = 0;
            }

            // 表示
            if (this.lblLoading.Visible)
            {
                lblLoading.Text = string.Empty.PadLeft(this.LoadingCnt, this.LoadingChar);
                lblLoading.Refresh();
                this.LoadingCnt++;
            }
        }

        #endregion

        #endregion

        #region ラベル表示

        /// --------------------------------------------------
        /// <summary>
        /// ステータスにメッセージを表示する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="showWait"></param>
        /// <create>Y.Higuchi 2010/04/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateSetMessage(this.SetMessage), new object[] { message });
                return;
            }
            this.lblStatus.Text = message;
            this.lblStatus.Refresh();
        }

        #endregion

        #region アプリケーション初期化処理

        /// --------------------------------------------------
        /// <summary>
        /// アプリケーション初期化処理
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update>H.Tajimi 2018/09/05 多言語対応</update>
        /// --------------------------------------------------
        protected virtual void SystemInitialize()
        {
            // デリゲートによる非同期実行ではカルチャ情報が引き継がれないため、引継ぎ処理を入れる
            ComFunc.SetThreadCultureInfo(this.CultureInfo);
            try
            {
                // サーバー接続確認中
                this.SetMessage(Resources.BaseSplash_BeginConnect);

                // システムパラメーター取得
                ConnCommon conn = new ConnCommon();
                var userInfo = new UserInfo();
                userInfo.Language = ComFunc.GetDBLangCode(this.CultureInfo);
                userInfo.CultureInfo = this.CultureInfo;
                var condCommon = new CondCommon(userInfo);
                DataSet ds = conn.GetSystemParameter(condCommon);

                bool isConnect = InnerCheckConnect();

                if (isConnect && ds != null && ds.Tables.Contains(Def_M_SYSTEM_PARAMETER.Name) && 0 < ds.Tables[Def_M_SYSTEM_PARAMETER.Name].Rows.Count)
                {
                    this.SetMessage(Resources.BaseSplash_ConnectSuccess);
                }
                else
                {
                    // サーバー接続失敗
                    this.SetMessage(Resources.BaseSplash_ConnectFailed);
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                // 初期化処理中
                this.SetMessage(Resources.BaseSplash_BeginInit);

                // ログインユーザー情報にPC情報を設定する。
                this.UserInfo = userInfo;
                if (!BaseFunc.GetSystemInitializeData(ref userInfo))
                {
                    // 初期化処理失敗
                    this.SetMessage(Resources.BaseSplash_BeginInitFailed);
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }

                this.InnerInitialize();

                this.SetMessage(Resources.BaseSplash_Complete);

                this.LoadComplete = true;

                // 少しだけ画面を表示する
                System.Threading.Thread.Sleep(500);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                // エラー時は失敗とする。
                this.DialogResult = DialogResult.Cancel;
            }
            finally
            {
                // 終了フラグをセットする。
                this.IsClose = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 接続テスト等のチェック処理
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool InnerCheckConnect()
        {
            return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <create>Y.Higuchi 2010/04/23</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InnerInitialize()
        {
        }

        #endregion
    }
}
