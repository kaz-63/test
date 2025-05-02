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
    /// 物件名一覧
    /// </summary>
    /// <create>T.Sakiori 2012/04/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BukkenMeiIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shukkaFlag = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenName = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 初期検索を行うかどうか
        /// </summary>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isInitSearch = false;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private BukkenMeiIchiran()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200040;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="bukkenName">物件名</param>
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        public BukkenMeiIchiran(UserInfo userInfo, string shukkaFlag, string bukkenName)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200040;

            this._shukkaFlag = shukkaFlag;
            this._bukkenName = bukkenName;
            this._isInitSearch = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="shukkaFlag">出荷区分</param>
        /// <param name="bukkenName">物件名</param>
        /// <param name="isInitSearch">初期検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public BukkenMeiIchiran(UserInfo userInfo, string shukkaFlag, string bukkenName, bool isInitSearch)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200040;

            this._shukkaFlag = shukkaFlag;
            this._bukkenName = bukkenName;
            this._isInitSearch = isInitSearch;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Sakiori 2012/04/05</create>
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
                this.txtBukkenName.Text = this._bukkenName;

                // 出荷区分
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.cboShukkaFlag.SelectedValue = this._shukkaFlag;

                // シートの列設定
                //物件名
                this.SetElTabelleColumn(this.shtResult, 0, Resources.BukkenMeiIchiran_PropertyName, false, true, Def_M_BUKKEN.BUKKEN_NAME, 370);
                //出荷区分
                this.SetElTabelleColumn(this.shtResult, 1, Resources.BukkenMeiIchiran_ShippingDivision, true, true, Def_M_BUKKEN.SHUKKA_FLAG, 70);
                //物件管理No.
                this.SetElTabelleColumn(this.shtResult, 2, Resources.BukkenMeiIchiran_PropertyManagementNo, true, true, Def_M_BUKKEN.BUKKEN_NO, 70);
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカス
                this.txtBukkenName.Focus();
                // 2015/11/30 H.Tajimi 初期検索が必要且つ、物件名が入力されている場合は検索を行う
                if (this._isInitSearch && !string.IsNullOrEmpty(this._bukkenName))
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
        /// <create>T.Sakiori 2012/04/05</create>
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
        /// <create>T.Sakiori 2012/04/05</create>
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
        /// <create>T.Sakiori 2012/04/05</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                this.shtResult.DataSource = null;
                this.shtResult.MaxRows = 0;
                var cond = new CondBukken(this.UserInfo);
                // 共通
                cond.ShukkaFlag = this._shukkaFlag;

                if (!this.IsSearchAll)
                {
                    // 検索の場合
                    cond.BukkenName = this.txtBukkenName.Text;
                }

                ConnP02 conn = new ConnP02();
                DataSet ds = conn.GetBukkenName(cond);

                if (!UtilData.ExistsData(ds, Def_M_BUKKEN.Name))
                {
                    // 該当する納入先はありません。
                    this.ShowMessage("P0200040001");
                    return false;
                }
                else
                {
                    this.shtResult.DataMember = Def_M_BUKKEN.Name;
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
        /// 物件名のキーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtBukkenName_KeyDown(object sender, KeyEventArgs e)
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
