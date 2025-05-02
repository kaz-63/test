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

namespace SystemBase.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 検索ダイアログベースフォーム
    /// </summary>
    /// <create>Y.Higuchi 2010/05/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class CustomOrderSearchDialog : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 初期検索モード
        /// </summary>
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum InitSearchMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 条件検索
            /// </summary>
            /// <create>H.Tajimi 2015/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            CONDITION_SEARCH,
            /// --------------------------------------------------
            /// <summary>
            /// 全件検索
            /// </summary>
            /// <create>H.Tajimi 2015/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            ALL_SEARCH,
            /// --------------------------------------------------
            /// <summary>
            /// 何もしない
            /// </summary>
            /// <create>H.Tajimi 2015/12/08</create>
            /// <update></update>
            /// --------------------------------------------------
            NONE,
        }

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 全件検索かどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isSearchAll = false;

        /// --------------------------------------------------
        /// <summary>
        /// 選択行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _selectedRowIndex = -1;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ（デザイナ用）
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderSearchDialog()
            : this(new UserInfo())
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderSearchDialog(UserInfo userInfo)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            // ここで画面タイトルを設定します。
            this.Title = "";
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        public CustomOrderSearchDialog(UserInfo userInfo, string title)
            : base(userInfo, title)
        {
            InitializeComponent();
        }

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 全件検索かどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected bool IsSearchAll
        {
            get { return this._isSearchAll; }
            set { this._isSearchAll = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択行インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        protected int SelectedRowIndex
        {
            get { return this._selectedRowIndex; }
            set { this._selectedRowIndex = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択行データ
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        [Browsable(false)]
        public DataRow SelectedRowData
        {
            get { return this.GetSelectedRowData(this.SelectedRowIndex); }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update>H.Tajimi 2015/11/18 </update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // コントロールの初期化を記述する。
                // ベースでDisplayClearの呼出しは行われています。
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;
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
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定を記述する。
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 画面クリア処理
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
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
        /// <create>Y.Higuchi 2010/05/10</create>
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
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            // 検索処理を記述する。
            return true;
        }

        #endregion

        #region 選択行取得

        /// --------------------------------------------------
        /// <summary>
        /// 選択行取得
        /// </summary>
        /// <param name="rowIndex">選択行インデックス</param>
        /// <returns>選択行のDataRow</returns>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual DataRow GetSelectedRowData(int rowIndex)
        {
            try
            {
                DataRow dr = null;
                if (0 <= rowIndex)
                {
                    DataTable dt = null;
                    // DataSourceがDataSetかDataTableであれば抽出
                    if (this.shtResult.DataSource as DataSet != null)
                    {
                        DataSet ds = this.shtResult.DataSource as DataSet;
                        if (ds.Tables.Contains(this.shtResult.DataMember))
                        {
                            dt = ds.Tables[this.shtResult.DataMember];
                        }
                    }
                    else if (this.shtResult.DataSource as DataTable != null)
                    {
                        dt = this.shtResult.DataSource as DataTable;
                    }
                    // DataTableが取得できたか判定
                    if (dt != null)
                    {
                        if (rowIndex < dt.Rows.Count)
                        {
                            dr = dt.Rows[rowIndex];
                        }
                    }
                }
                return dr;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region イベント

        #region EventHandler

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのダブルクリックイベントハンドル追加
        /// </summary>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void AddEventHandlerSheetDoubleClick()
        {
            this.shtResult.DoubleClick += new EventHandler(this.shtResult_DoubleClick);
        }

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのダブルクリックイベントハンドル削除
        /// </summary>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        protected void RemoveEventHandlerSheetDoubleClick()
        {
            this.shtResult.DoubleClick -= new EventHandler(this.shtResult_DoubleClick);
        }

        #endregion

        #region グリッド

        #region DoubleClick

        /// --------------------------------------------------
        /// <summary>
        /// ダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void shtResult_DoubleClick(object sender, EventArgs e)
        {
            GrapeCity.Win.ElTabelle.Range objRange;
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                var sheetArea = this.shtResult.HitTest(new Point(mouseEventArgs.X, mouseEventArgs.Y), out objRange);
                switch (sheetArea)
                {
                    case GrapeCity.Win.ElTabelle.SheetArea.Cell:
                        // セルでダブルクリックされた場合のみ[決定]ボタンと同じ動作を行う
                        this.btnSelect.PerformClick();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region 全件検索

        /// --------------------------------------------------
        /// <summary>
        /// 全権検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnSearchAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.IsSearchAll = true;
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 検索

        /// --------------------------------------------------
        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.IsSearchAll = false;
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 選択

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                int row = this.shtResult.ActivePosition.Row;
                if (0 <= row && this.shtResult.Rows[row].Selected)
                {
                    // 行が選択されていればセットし画面を閉じる
                    this.SelectedRowIndex = row;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // 検索結果の行が選択されていません。
                    this.ShowMessage("FW010050001");
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 閉じる

        /// --------------------------------------------------
        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/05/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion
    }
}
