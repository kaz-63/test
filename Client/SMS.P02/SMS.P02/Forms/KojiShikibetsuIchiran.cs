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

using WsConnection.WebRefP02;
using SMS.P02.Properties;

namespace SMS.P02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 工事識別一覧
    /// </summary>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KojiShikibetsuIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields

        private string _kojiName = string.Empty;
        private string _ship = string.Empty;
        private string _torokuFlag = string.Empty;
        private bool _isInitSearch = false;
        private bool _isKonpo = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        private KojiShikibetsuIchiran()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200030;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="torokuFlag">登録区分</param>
        /// <param name="kojiName">工事識別名称</param>
        /// <param name="ship">出荷便</param>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        public KojiShikibetsuIchiran(UserInfo userInfo, string torokuFlag,string kojiName, string ship)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200030;
            this._torokuFlag = torokuFlag;
            this._kojiName = kojiName;
            this._ship = ship;
            this._isInitSearch = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="torokuFlag">登録区分</param>
        /// <param name="kojiName">工事識別名称</param>
        /// <param name="ship">出荷便</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public KojiShikibetsuIchiran(UserInfo userInfo, string torokuFlag, string kojiName, string ship, bool isInitSearch)
            :base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200030;
            this._torokuFlag = torokuFlag;
            this._kojiName = kojiName;
            this._ship = ship;
            this._isInitSearch = isInitSearch;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="isKonpo">木枠梱包チェック</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public KojiShikibetsuIchiran(UserInfo userInfo, bool isKonpo)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200030;
            this._torokuFlag = TOROKU_FLAG.NAI_VALUE1;
            this._kojiName = "";
            this._ship = "";
            this._isInitSearch = false;
            this._isKonpo = isKonpo;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            // 2015/11/30 H.Tajimi グリッドのダブルクリックイベント追加
            this.AddEventHandlerSheetDoubleClick();
            // ↑

            try
            {
                // コントロールの初期化
                // アイコンの設定
                this.Icon = ComFunc.BitmapToIcon(ComResource.Search);
                // 工事識別名称
                this.txtKojiName.Text = this._kojiName;

                // 便
                this.txtShip.Text = this._ship;

                // シートの列設定
                // 工事識別名称
                this.SetElTabelleColumn(this.shtResult, 0, Resources.KojiShikibetsuIchiran_ConstructionIdentificationNo, false, true, Def_T_KIWAKU.KOJI_NAME, 370);
                // 便
                this.SetElTabelleColumn(this.shtResult, 1, Resources.KojiShikibetsuIchiran_Ship, false, true, Def_T_KIWAKU.SHIP, 70);
                // 工事識別NO
                this.SetElTabelleColumn(this.shtResult, 2, Resources.KojiShikibetsuIchiran_ManagementNo, false, true, Def_T_KIWAKU.KOJI_NO, 70);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtKojiName.Focus();
                // 2015/11/30 H.Tajimi 初期検索が必要且つ、工事識別NOまたは便が入力されている場合は検索を行う
                if (this._isInitSearch && (!string.IsNullOrEmpty(this._kojiName) || !string.IsNullOrEmpty(this._ship)))
                {
                    if (this.RunSearch())
                    {
                        var ds = this.shtResult.DataSource as DataSet;
                        if (ds != null && ds.Tables[this.shtResult.DataMember] != null)
                        {
                            if (ds.Tables[this.shtResult.DataMember].Rows.Count == 1)
                            {
                                this.btnSelect.PerformClick();
                            }
                        }
                    }
                }
                // ↑
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            // 2015/11/30 H.Tajimi 初期検索フラグは落とす
            finally
            {
                this._isInitSearch = false;
            }
            // ↑
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/06/28</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtResult.DataSource = null;
                this.shtResult.MaxRows = 0;
                CondKiwaku cond = new CondKiwaku(this.UserInfo);
                // 共通
                cond.TorokuFlag = this._torokuFlag;
                if (!this.IsSearchAll)
                {
                    // 検索の場合
                    cond.KojiName = this.txtKojiName.Text;
                    cond.Ship = this.txtShip.Text;
                }
                cond.KiwakuKonpo = this._isKonpo;

                ConnP02 conn = new ConnP02();
                DataSet ds = conn.GetKojiShikibetsuIchiran(cond);

                if (!ds.Tables.Contains(Def_M_NONYUSAKI.Name) || ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
                {
                    // 該当する工事識別Noはありません。
                    this.ShowMessage("P0200030001");
                    return false;
                }
                else
                {
                    this.shtResult.DataMember = Def_M_NONYUSAKI.Name;
                    this.shtResult.DataSource = ds;
                }
                // 2015/11/30 H.Tajimi グリッドにフォーカス設定
                this.shtResult.Focus();
                // ↑
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region イベント

        #region テキストボックス

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別Noのキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtKojiName_KeyDown(object sender, KeyEventArgs e)
        {
            // Enterキーが押下された時は、検索ボタンにフォーカス遷移する
            if ((e.KeyCode == Keys.Enter) && !e.Alt && !e.Control)
            {
                this.btnSearch.Focus();
            }
        }

        #endregion

        #endregion
    }
}
