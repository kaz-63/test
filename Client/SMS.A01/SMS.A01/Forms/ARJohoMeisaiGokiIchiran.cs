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
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
using System.Linq;
using WsConnection.WebRefA01;
using SMS.A01.Properties;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// AR情報号機選択画面
    /// </summary>
    /// <create>Y.Nakasato 2019/07/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ARJohoMeisaiGokiIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields
        /// --------------------------------------------------
        /// <summary>
        /// 引数の選択中機種名
        /// </summary>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _kishuArg = "";

        /// --------------------------------------------------
        /// <summary>
        /// 引数の選択中号機名リスト(編集前の一覧)
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string[] _gokiArgList;

        /// --------------------------------------------------
        /// <summary>
        /// 引数の日付の登録されている号機名リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string[] _gokiDateList = null;

        /// --------------------------------------------------
        /// <summary>
        /// 機種リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly List<string> _kishuList = new List<string>();

        /// --------------------------------------------------
        /// <summary>
        /// 号機選択画面
        /// </summary>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private ShinchokuGokiIchiran _gokiIchiranInst = null;

        /// --------------------------------------------------
        /// <summary>
        /// 引数の選択された納入先コード
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _nonyusakiCD;

        /// --------------------------------------------------
        /// <summary>
        /// 号機リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly List<ComFunc.GokiInfoList> _GokiInfoList = new List<ComFunc.GokiInfoList>();

        /// --------------------------------------------------
        /// <summary>
        /// 号機文字列の最大長
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _gokiMaxLen = 0;

        /// --------------------------------------------------
        /// <summary>
        /// 機種の項目区切り(カンマ等)
        /// </summary>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly char _separator;

        /// --------------------------------------------------
        /// <summary>
        /// 範囲の区切り文字
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly char _separatorRange;

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの「選択中のみ」
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _cmbSelected = Resources.ShinchokuGoki_SelectedGoki;

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの「全て」
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _cmbAll = ComDefine.COMBO_ALL_DISP;

        /// --------------------------------------------------
        /// <summary>
        /// 機種コンボボックス前回選択値
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _latestItem = null;

        /// --------------------------------------------------
        /// <summary>
        /// リストビューのチェックＯＦＦ処理中フラグ
        /// </summary>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isChecking = false;


        /// --------------------------------------------------
        /// <summary>
        /// 選択外号機の文字列色
        /// </summary>
        /// <create>Y.Nakasato 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color? _ExcludedGokiFontColor = null;

        /// --------------------------------------------------
        /// <summary>
        /// 選択外号機の背景色
        /// </summary>
        /// <create>Y.Nakasato 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private Color? _ExcludedGokiBackColor = null;

        #endregion // Fields

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 選択号機
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string[] SelectedGoki { get; private set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="kishu"></param>
        /// <param name="goki"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public ARJohoMeisaiGokiIchiran(UserInfo userInfo, string title, string kishu, string[] goki, string nonyusakiCD, string[] gokiShinchokuList, SystemBase.EditMode editMode)
            : base(userInfo, title)
        {
            InitializeComponent();
            this.EditMode = editMode;

            this.txtKishu.Text = kishu;
            this._kishuArg = kishu;
            this.txtGoki.Text = string.Empty;
            this._nonyusakiCD = nonyusakiCD;
            this._separator = userInfo.SysInfo.SeparatorItem;
            this._separatorRange = userInfo.SysInfo.SeparatorRange;
            if (goki == null)
                goki = new string[0];
            this.SelectedGoki = goki.Clone() as string[];
            this._gokiArgList = goki.Clone() as string[];
            this._gokiDateList = gokiShinchokuList;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update>D.Okumura 2019/08/22 未選択時は選択された号機一覧を表示する</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                var conn = new ConnA01();
                var cond = new CondA1(this.UserInfo);
                cond.NonyusakiCD = _nonyusakiCD;
                cond.Kishu = null; // txtKishu.Text;

                { // 選択外号機の表示色取得
                    DataTable dt = this.GetCommon(AR_SHINCHOKU_SETTING.GROUPCD).Tables[Def_M_COMMON.Name];
                    DataRow dr = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + AR_SHINCHOKU_SETTING.EXCLUDED_DEVICE_NAME + "'").FirstOrDefault();
                    var rgb = ComFunc.GetFld(dr, Def_M_COMMON.VALUE3).Split(',');
                    this._ExcludedGokiFontColor = ComFunc.GetColorFromRgb(rgb[0]);
                    this._ExcludedGokiBackColor = ComFunc.GetColorFromRgb(rgb[1]);
                } // 選択外号機の表示色取得

                { // 機種一覧作成
                    var dt = conn.GetKishu(cond);
                    foreach (DataRow dr in dt.Rows)
                    {
                        this._kishuList.Add(ComFunc.GetFld(dr, Def_T_AR_GOKI.KISHU));
                    }
                } // 機種一覧作成

                { // 号機一覧作成
                    var dt = conn.GetGoki(cond);
                    ComFunc.CreateGokiInfoListFromDt(dt, this._GokiInfoList, out this._gokiMaxLen);
                } // 号機一覧作成

                { // 引数の抽出条件を号機一覧に反映
                    int maxLen = 0;
                    if ((ComFunc.CheckGokiInfoSelected(this._GokiInfoList, this._gokiArgList, false, out maxLen) > 0)
                     && (maxLen > this._gokiMaxLen))
                    {
                        this._gokiMaxLen = maxLen;
                    }
                } // 引数の抽出条件を号機一覧に反映

                // コンボボックス設定
                { // コンボボックス設定
                    cboKishu.Items.Add(this._cmbSelected);
                    cboKishu.Items.Add(this._cmbAll);
                    cboKishu.Items.AddRange(this._kishuList.ToArray());

                } // コンボボックス設定

                // 選択済み号機がない場合は、既存の項目を選択させる
                if (this.EditMode != SystemBase.EditMode.View && this.SelectedGoki.Length == 0 && this._kishuList.Contains(this._kishuArg))
                {
                    this.cboKishu.SelectedIndex = this.cboKishu.Items.IndexOf(this._kishuArg);
                    this.RunSearch();
                }
                else
                {
                    this.cboKishu.SelectedIndex = 0;
                }
                // 照会モード時
                if (this.EditMode == SystemBase.EditMode.View)
                {
                    this.chkCheckResult.Enabled = false;
                    this.btnAllSelect.Enabled = false;
                    this.btnAllRelease.Enabled = false;
                    this.btnRangeSelect.Enabled = false;
                    this.btnRangeRelease.Enabled = false;
                    this.btnSelect.Enabled = false;
                    this.IsCloseQuestion = false;
                }
                else
                {
                    this.IsCloseQuestion = true;
                }

                this.MsgInsertConfirm = string.Empty;
                this.MsgInsertEnd = string.Empty;
                this.MsgUpdateConfirm = string.Empty;
                this.MsgUpdateEnd = string.Empty;
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
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboKishu.Focus();
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
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ベースでClearMessageの呼出しは行われています。
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック: なし
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/19</create>
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
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            lstGoki.Items.Clear();
            try
            {
                this.UpdateListView( this._latestItem, 
                                this.chkCheckResult.Checked,
                                ComFunc.GokiStringToArray(this.txtGoki.Text, this._separator, this._separatorRange));
                if (this.lstGoki.Items.Count < 1)
                {
                    // 該当する号機はありません。
                    this.ShowMessage("A0100022005");
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region イベント

        #region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 号機一覧クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnGoki_Click(object sender, EventArgs e)
        {
            try
            {
                if (_gokiIchiranInst != null)
                {
                    _gokiIchiranInst.Activate();
                    return;
                }

                using (var form = new ShinchokuGokiIchiran(this.UserInfo, ComDefine.TITLE_A0100053, this._nonyusakiCD, txtGoki.Text))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        txtGoki.Text = form.Goki;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 号機選択が閉じられたときに実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void GokiIchiranClosed(object sender, System.EventArgs e)
        {
            // すでにクローズ済みの場合は処理しない
            if (_gokiIchiranInst != null)
            {
                // cboGoki.Text = 
                _gokiIchiranInst.FormClosed -= new FormClosedEventHandler(this.GokiIchiranClosed);
                _gokiIchiranInst = null;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 機種コンボボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboKishu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newItem = cboKishu.SelectedItem.ToString();
            if (this._latestItem == newItem)
                return ;

            if (newItem == this._cmbSelected)
            {
                txtGoki.Enabled = false;
                btnGoki.Enabled = false;
                chkCheckResult.Enabled = false;
                UpdateListView(newItem, false, null);
            }
            else
            {
                txtGoki.Enabled = true;
                btnGoki.Enabled = true;
                if (this.EditMode != SystemBase.EditMode.View)
                    chkCheckResult.Enabled = true;
            }
            this._latestItem = newItem;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            try
            {
                _isChecking = true;
                this.ControlCheck(true);
                _isChecking = false;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllRelease_Click(object sender, EventArgs e)
        {
            try
            {
                var dateGoki = IsGokiDateInLstView(lstGoki, this._gokiDateList, false);
                if (dateGoki != null)
                {
                    // 日付が設定されている号機を含んでいます。進捗管理から除外しますか?
                    if (this.ShowMessage("A0100022002", dateGoki) != DialogResult.Yes)
                        return;
                }
                _isChecking = true;
                this.ControlCheck(false);
                _isChecking = false;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeSelect_Click(object sender, EventArgs e)
        {
            this.RangeSelectExec();
        }
        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックする処理の実行
        /// </summary>
        /// <create>Y.Nakasato 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RangeSelectExec()
        {
            try
            {
                _isChecking = true;
                this.ControlCheckInSelected(true);
                _isChecking = false;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }
        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeRelease_Click(object sender, EventArgs e)
        {
            this.RangeReleaseExec(true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずす処理の実行
        /// </summary>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RangeReleaseExec(bool dateCheck)
        {
            try
            {
                if (dateCheck)
                {
                    var dateGoki = IsGokiDateInLstView(lstGoki, this._gokiDateList, true);
                    if (dateGoki != null)
                    {
                        // 日付が設定されている号機を含んでいます。進捗管理から除外しますか?
                        if (this.ShowMessage("A0100022002", dateGoki) != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }
                _isChecking = true;
                this.ControlCheckInSelected(false);
                _isChecking = false;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        private int SelectecdCheckingRemain = 0;
        /// --------------------------------------------------
        /// <summary>
        /// グリッド上のチェックボックス変化イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void lstGoki_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                var item = this.lstGoki.Items[e.Index].Tag as ComFunc.GokiInfoList;
                if (item == null)
                    return;

                // 編集モード
                if (this.EditMode == SystemBase.EditMode.View)
                {
                    e.NewValue = (item.Selected ? CheckState.Checked : CheckState.Unchecked);    // 変化させない
                    return;
                }

                // ボタンによるチェックボックス操作中はそのまま処理
                if (_isChecking)
                {
                    item.Selected = (e.NewValue == CheckState.Checked);
                }
                // チェックONならそのまま
                else if (e.NewValue == CheckState.Checked)
                {
                    // セル選択していないチェックボックスをＯＮ
                    if ((lstGoki.SelectedItems.Count > 0)
                     && (!this.lstGoki.Items[e.Index].Selected))
                    {
                        this.lstGoki.SelectedItems.Clear(); // セル選択を解除
                    }
                    item.Selected = true;
                }
                // リスト内のチェックボックスをＯＦＦ
                else
                {
                    // セル選択していないチェックボックスをＯＦＦ
                    if ((lstGoki.SelectedItems.Count > 0)
                     && (!this.lstGoki.Items[e.Index].Selected))
                    {
                        this.lstGoki.SelectedItems.Clear(); // セル選択を解除
                    }

                    // セル選択なしでチェックボックスを１個だけチェックＯＦＦ
                    if (lstGoki.SelectedItems.Count == 0)
                    {
                        if (item.Selected && IsGokiDate(item.Goki, this._gokiDateList))
                        {
                            // 日付が設定されています。進捗管理から除外しますか?
                            if (this.ShowMessage("A0100022001", item.Goki) == DialogResult.Yes)
                            {
                                item.Selected = false;
                            }
                            else
                            {
                                e.NewValue = e.CurrentValue;    // 変化させない
                            }
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    // セル選択あり(セル選択が１セルだけだとしてもここで処理する)
                    else
                    {
                        e.NewValue = e.CurrentValue;    // 変化させない

                        // まとめ処理の始まり
                        if (SelectecdCheckingRemain == 0)
                        {
                            foreach (ListViewItem selItem in lstGoki.SelectedItems)
                            {
                                if ((selItem.Tag as ComFunc.GokiInfoList).Selected)
                                    SelectecdCheckingRemain++;
                            }
                        }
                        // まとめ処理中
                        else
                        {
                            SelectecdCheckingRemain--;
                        }

                        // まとめ処理の最後
                        if (SelectecdCheckingRemain == 1)
                        {
                            var dateGoki = IsGokiDateInLstView(lstGoki, this._gokiDateList, true);
                            if (dateGoki != null)
                            {
                                // 日付が設定されている号機を含んでいます。進捗管理から除外しますか?
                                if (this.ShowMessage("A0100022002", dateGoki) == DialogResult.Yes)
                                {
                                    this.BeginInvoke(new Action(() => { RangeReleaseExec(false); }));
                                }
                            }
                            else
                            {
                                this.BeginInvoke(new Action(() => { RangeReleaseExec(false); }));
                            }
                            SelectecdCheckingRemain = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// アイテムのチェックをON/OFFする
        /// </summary>
        /// <param name="check"></param>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ControlCheck(bool check)
        {
            foreach (ListViewItem item in lstGoki.Items)
            {
                item.Checked = check;
                (item.Tag as ComFunc.GokiInfoList).Selected = check;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 選択中アイテムのチェックをON/OFFする
        /// </summary>
        /// <param name="check"></param>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ControlCheckInSelected(bool check)
        {
            if (lstGoki.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (ListViewItem item in lstGoki.SelectedItems)
            {
                item.Checked = check;
                (item.Tag as ComFunc.GokiInfoList).Selected = check;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// ListViewの中から号機リストにマッチする号機文字列をセパレータ区切りで連結
        /// </summary>
        /// <param name="lstView"></param>
        /// <param name="gokiDateList"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string IsGokiDateInLstView(ListView lstView, string[] gokiDateList, bool selectedOnly)
        {
            List<string> matchGokiList = new List<string>();

            foreach (ListViewItem item in lstView.CheckedItems)
            {
                if (!selectedOnly || (selectedOnly && item.Selected))
                {
                    string goki = (item.Tag as ComFunc.GokiInfoList).Goki;
                    if (IsGokiDate(goki, gokiDateList))
                    {
                        matchGokiList.Add(goki);
                    }
                }
            }
            if (matchGokiList.Count > 0)
                return string.Join(_separator.ToString(), matchGokiList.ToArray());
            else
                return null;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機に日付設定されているかどうか
        /// </summary>
        /// <param name="goki"></param>
        /// <param name="gokiList"></param>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsGokiDate(string goki, string[] gokiList)
        {
            if (gokiList == null)
                return false;

            if (string.IsNullOrEmpty(goki))
                return false;

            return (Array.IndexOf(gokiList, goki) == -1 ? false : true);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            try
            {
                bool ret = true;
                var otherGoki = (this._GokiInfoList.FindAll(x => x.Selected && x.Kishu != this._kishuArg).ToArray()).Select(w => w.Goki).ToArray();

                if (otherGoki.Length > 0)
                {
                    this.cboKishu.SelectedIndex = 0;
                    var others = string.Join(_separator.ToString(), otherGoki);
                    // {0}は選択された機種ではありません。継続してもよいですか？
                    if (this.ShowMessage("A0100022003", others) != DialogResult.Yes)
                    {
                        ret = false;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F01(決定)ボタンイベント
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();

                if (ret)
                {
                    this.DialogResult = DialogResult.OK;
                    var list = this._GokiInfoList.FindAll(x => x.Selected).ToArray();
                    string[] GokiArray = list.Select(w => w.Goki).ToArray();
                    this.SelectedGoki = GokiArray;

                    this.IsCloseQuestion = false;
                    this.Close();
                }
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面Closeイベント
        /// </summary>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                var restore = this.IsCloseQuestion;
                // F01(決定)経由ではない。(F12か[X]ボタン)
                if (this.IsCloseQuestion)
                {
                    var selectedGoki = (this._GokiInfoList.FindAll(x => x.Selected).ToArray())
                                            .Select(w => w.Goki).ToArray();

                    // 前画面から指定された号機一覧と今回選択されている号機一覧とで差異がある
                    if (selectedGoki.Length != this._gokiArgList.Length
                     || selectedGoki.Any(w => !this._gokiArgList.Contains(w))
                     || this._gokiArgList.Any(w => !selectedGoki.Contains(w))
                        )
                    {
                        // 号機の選択状態が変更されていますが、反映せずに閉じますか?
                        if (this.ShowMessage("A0100022004") != DialogResult.Yes)
                        {
                            e.Cancel = true;
                        }
                    }
                }

                // Close確認ダイアログは常に出さない(すでにチェック済みの認識)
                this.IsCloseQuestion = false;
                base.OnClosing(e);
                this.IsCloseQuestion = restore;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F01(決定)ボタン クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnSelect_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F01Button_Click(sender, e);
        }

        #endregion


        #region 検索ボタン

        //// --------------------------------------------------
        // <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunSearch();
                this.shtResult.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion


        /// --------------------------------------------------
        /// <summary>
        /// 号機リスト表示更新処理
        /// </summary>
        /// <param name="kishu">機種選択状態</param>
        /// <param name="selectedCheck">抽出結果を全て選択済みにするかどうか</param>
        /// <param name="gokiRangeList">選択済み号機範囲リスト</param>
        /// <create>Y.Nakasato 2019/07/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void UpdateListView(string kishu, bool selectedCheck, string[] gokiRangeList)
        {
            this.lstGoki.Items.Clear();

            this._isChecking = true;
            foreach (var goki in this._GokiInfoList)
            {
                if ((((kishu == this._cmbSelected) && (goki.Selected))
                  || (kishu == this._cmbAll)
                  || ((kishu != this._cmbSelected) && (kishu != this._cmbAll) && (kishu == goki.Kishu)))
                 && ((kishu == this._cmbSelected)
                  || (gokiRangeList == null)
                  || ((gokiRangeList != null) && (Array.IndexOf(gokiRangeList, goki.Goki) != -1))))
                {
                    if (selectedCheck)
                    {
                        goki.Selected = true;
                    }
                    var item = new ListViewItem();
                    item.Text = goki.Goki + string.Empty.PadRight(this._gokiMaxLen - UtilString.GetByteCount(goki.Goki));
                    item.Tag = goki;
                    item.Checked = goki.Selected;
                    if (goki.Kishu != this._kishuArg)
                    {
                        if (this._ExcludedGokiFontColor != null)
                            item.ForeColor = (Color)this._ExcludedGokiFontColor;

                        if (this._ExcludedGokiBackColor != null)
                            item.BackColor = (Color)this._ExcludedGokiBackColor;
                    }
                    this.lstGoki.Items.Add(item);
                }
            }
            this._isChecking = false;
        }
    }
}
