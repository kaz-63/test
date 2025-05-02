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
    /// 納入先一覧
    /// </summary>
    /// <remarks>
    /// 納入先を複数選択できるI/FでNonyusakiIchiranExに
    /// 同様の処理が実装されているので、修正時は注意してください
    /// </remarks>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NonyusakiIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields

        private string _shukkaFlag = string.Empty;
        private string _nonyusakiName = string.Empty;
        private string _ship = string.Empty;
        private string _gamenFlag = string.Empty;
        private bool _isInitSearch = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private NonyusakiIchiran()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200010;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiran(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, string.Empty)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <create>T.Sakiori 2012/05/09</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiran(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string gamenFlag)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, gamenFlag, false)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiran(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, bool isInitSearch)
            : this(userInfo, shukkaFlag, nonyusakiName, ship, string.Empty, isInitSearch)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="nonyusakiName">納入先</param>
        /// <param name="ship">出荷便</param>
        /// <param name="gamenFlag">画面区分</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiIchiran(UserInfo userInfo, string shukkaFlag, string nonyusakiName, string ship, string gamenFlag, bool isInitSearch)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200010;

            this._shukkaFlag = shukkaFlag;
            this._nonyusakiName = nonyusakiName;
            this._ship = ship;
            this._gamenFlag = gamenFlag;
            this._isInitSearch = isInitSearch;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/06/25</create>
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
                // 納入先
                this.txtNonyusakiName.Text = this._nonyusakiName;

                // 便
                this.txtShip.Text = this._ship;

                // 出荷区分
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.cboShukkaFlag.SelectedValue = this._shukkaFlag;
                if (this._shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    this.txtShip.Text = string.Empty;
                    this.txtShip.Enabled = false;
                }

                // 管理区分
                this.MakeCmbBox(this.cboKanriFlag, KANRI_FLAG.GROUPCD, true);
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    this.lblKanriFlag.Visible = false;
                }

                // シートの列設定
                //納入先
                this.SetElTabelleColumn(this.shtResult, 0, Resources.NonyusakiIchiran_DeliveryDestination, false, true, Def_M_NONYUSAKI.NONYUSAKI_NAME, 370);
                //便
                this.SetElTabelleColumn(this.shtResult, 1, Resources.NonyusakiIchiran_Ship, false, true, Def_M_NONYUSAKI.SHIP, 70);
                //納入先コード
                this.SetElTabelleColumn(this.shtResult, 2, Resources.NonyusakiIchiran_ManagementNo, false, true, Def_M_NONYUSAKI.NONYUSAKI_CD, 70);
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
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update>H.Tajimi 2015/11/27 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtNonyusakiName.Focus();

                // 2015/11/27 H.Tajimi 初期検索処理追加
                var initSearchMode = InitSearchMode.NONE;
                if (this._shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // ARの場合は、納入先または便入力時は条件検索。それ以外は全件検索
                    if (this._isInitSearch && (!string.IsNullOrEmpty(this._nonyusakiName) || !string.IsNullOrEmpty(this._ship)))
                    {
                        // 条件検索
                        initSearchMode = InitSearchMode.CONDITION_SEARCH;
                    }
                    else
                    {
                        // 全件検索
                        initSearchMode = InitSearchMode.ALL_SEARCH;
                    }
                }
                else
                {
                    // 本体の場合は、納入先または便入力時は条件検索。それ以外は何もしない
                    if (this._isInitSearch && (!string.IsNullOrEmpty(this._nonyusakiName) || !string.IsNullOrEmpty(this._ship)))
                    {
                        // 条件検索
                        initSearchMode = InitSearchMode.CONDITION_SEARCH;
                    }
                }

                switch (initSearchMode)
                {
                    case InitSearchMode.ALL_SEARCH:
                        this.IsSearchAll = true;
                        this.RunSearch();
                        break;

                    case InitSearchMode.CONDITION_SEARCH:
                        this.IsSearchAll = false;
                        var ret = this.RunSearch();
                        if (ret && this._isInitSearch)
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
                        break;

                    default:
                        break;
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
        /// <create>Y.Higuchi 2010/06/25</create>
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
        /// <create>Y.Higuchi 2010/06/25</create>
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
        /// <create>Y.Higuchi 2010/06/25</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtResult.DataSource = null;
                this.shtResult.MaxRows = 0;
                CondNonyusaki cond = new CondNonyusaki(this.UserInfo);
                // 共通
                cond.ShukkaFlag = this._shukkaFlag;

                if (!this.IsSearchAll)
                {
                    // 検索の場合
                    cond.KanriFlag = this.cboKanriFlag.SelectedValue.ToString();
                    cond.NonyusakiName = this.txtNonyusakiName.Text;
                    if (this._shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        cond.Ship = this.txtShip.Text;
                    }
                }
                if (!string.IsNullOrEmpty(this._gamenFlag))
                {
                    cond.GamenFlag = this._gamenFlag;
                }

                ConnP02 conn = new ConnP02();
                DataSet ds = string.IsNullOrEmpty(this._gamenFlag) ? conn.GetNonyusakiIchiran(cond) : conn.GetRirekiBukkenIchiran(cond);

                if (!ds.Tables.Contains(Def_M_NONYUSAKI.Name) || ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("P0200010001");
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
        /// 納入先(User)のキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtNonyusakiName_KeyDown(object sender, KeyEventArgs e)
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
