using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using GrapeCity.Win.ElTabelle;
using ElTabelleHelper;
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
    public partial class ShinchokuGokiIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 引数の選択中号機名リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _gokiList = null;

        /// --------------------------------------------------
        /// <summary>
        /// 引数の納入先CD
        /// </summary>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCD = "";

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
        /// 機種リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        List<string> _kishuList = new List<string>();

        /// --------------------------------------------------
        /// <summary>
        /// 号機リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        List<ComFunc.GokiInfoList> _GokiInfoList = new List<ComFunc.GokiInfoList>();

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの「選択中のみ」
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private readonly string _cmbSelected = "";

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
        /// 号機文字列の最大長
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _gokiMaxLen = 0;

        #endregion //Fields


        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 号機
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Goki { get; private set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="NonyusakiCD">納入先コード</param>
        /// <param name="goki"></param>
        /// <create>Y.Nakasato 2019/07/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuGokiIchiran(UserInfo userInfo, string title, string NonyusakiCD, string goki)
            : base(userInfo, title)
        {
            this._separator = userInfo.SysInfo.SeparatorItem;
            this._separatorRange = userInfo.SysInfo.SeparatorRange;
            this._gokiList = !string.IsNullOrEmpty(goki) ? ComFunc.GokiStringToArray(goki, this._separator, this._separatorRange) : null;
            this._nonyusakiCD = NonyusakiCD;
            this._cmbSelected = Resources.ShinchokuGoki_SelectedGoki;

            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                var conn = new ConnA01();
                var cond = new CondA1(this.UserInfo);
                cond.NonyusakiCD = _nonyusakiCD;
                cond.Kishu = null;

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
                    ComFunc.CheckGokiInfoSelected(this._GokiInfoList, this._gokiList, false, out maxLen);
                } // 引数の抽出条件を号機一覧に反映

                { // コンボボックス設定
                    cboKishu.Items.Add(_cmbSelected);               // 先頭は"空白"
                    cboKishu.Items.Add(_cmbAll);                   // ２番目は"全て"
                    cboKishu.Items.AddRange(_kishuList.ToArray());
                    cboKishu.SelectedIndex = 0;
                } // コンボボックス設定

                this.EditMode = SystemBase.EditMode.Update;
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
                // TODO : ここに初期フォーカスの設定を記述する。
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア
        #endregion

        #region 入力チェック
        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            this.Goki = "";

            // 号機文字列生成
            if (_GokiInfoList.Count(x => x.Selected) > 0)
            {
                string fstGoki = string.Empty, fstNum = string.Empty, preNum = string.Empty;
                int ifstNum = -1, ipreNum = -1;
                List<string> listGoki = new List<string> { };

                foreach (var goki in _GokiInfoList)
                {
                    if (goki.Selected)
                    {
                        string nowGoki = string.Empty;
                        string nowNum = string.Empty;
                        int inowNum = -1;

                        // 今回連番付き
                        if ((ComFunc.GetNameAndNum(goki.Goki, out nowGoki, out nowNum))
                         && (int.TryParse(nowNum, out inowNum)))
                        {
                            // 連番の始まり(暫定)
                            if (ifstNum == -1)
                            {
                                fstGoki = nowGoki;
                                fstNum = preNum = nowNum;
                                ifstNum = ipreNum = inowNum;
                            }
                            // 連番継続
                            else if ((fstGoki == nowGoki)
                                  && (preNum.Length == nowNum.Length)
                                  && ((ipreNum + 1) == inowNum))
                            {
                                preNum = nowNum;
                                ipreNum = inowNum;
                            }
                            // 連番終了
                            else
                            {
                                CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum);

                                // 連番の始まり(暫定)
                                fstGoki = nowGoki;
                                fstNum = preNum = nowNum;
                                ifstNum = ipreNum = inowNum;
                            }
                        }
                        // 今回連番なし
                        else
                        {
                            // 連番終了
                            if (ifstNum != -1)
                            {
                                CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum);
                            }
                            fstGoki = fstNum = preNum = string.Empty;
                            ifstNum = ipreNum = -1;
                            listGoki.Add(goki.Goki);
                        }
                    }
                }

                // 連番終了
                if (ifstNum != -1)
                {
                    CreateGokiRenban(listGoki, fstGoki, fstNum, preNum, ifstNum, ipreNum);
                }

                this.Goki = string.Join(_separator.ToString(), listGoki.ToArray());
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 号機連番の文字列生成
        /// </summary>
        /// <param name="listGoki"></param>
        /// <param name="fstGoki"></param>
        /// <param name="fstNum"></param>
        /// <param name="preNum"></param>
        /// <param name="ifstNum"></param>
        /// <param name="ipreNum"></param>
        /// <create>Y.Nakasato 2019/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CreateGokiRenban(List<string> listGoki, string fstGoki, string fstNum, string preNum, int ifstNum, int ipreNum)
        {
            // 結局連番しなかった
            if (ifstNum == ipreNum)
            {
                listGoki.Add(fstGoki + fstNum);
            }
            // 連番した
            else
            {
                listGoki.Add(fstGoki + fstNum + _separatorRange + preNum);
            }
        }
        #endregion

        #region イベント

        #region ボタンクリック

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
            this.ControlCheck(true);
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
            this.ControlCheck(false);
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
            this.ControlCheckInSelected(true);
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
            this.ControlCheckInSelected(false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 機種コンボボックス前回選択値
        /// </summary>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private string latestItem = null;
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックス選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboKishu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newItem = cboKishu.SelectedItem.ToString();
            if (latestItem == newItem)
                return;

            lstGoki.Items.Clear();

            try
            {
                foreach (var goki in _GokiInfoList)
                {
                    if ((newItem == _cmbAll)
                     || ((newItem == _cmbSelected) && (goki.Selected))
                     || ((newItem != _cmbAll) && (newItem != _cmbSelected) && (goki.Kishu == newItem)))
                    {
                        var item = new ListViewItem();
                        item.Text = goki.Goki + string.Empty.PadRight(_gokiMaxLen - UtilString.GetByteCount(goki.Goki));
                        item.Tag = goki;
                        item.Checked = goki.Selected;
                        lstGoki.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

            latestItem = newItem;
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

        /// --------------------------------------------------
        /// <summary>
        /// グリッド上のチェックボックス変化イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void lstGoki_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = lstGoki.Items[e.Index].Tag as ComFunc.GokiInfoList;
            if (item == null)
                return;

            item.Selected = (e.NewValue == CheckState.Checked);

            if ((lstGoki.SelectedItems.Count > 0)
             && (!lstGoki.Items[e.Index].Selected))
            {
                lstGoki.SelectedItems.Clear();  // セル選択を解除
            }
        }

        #endregion

        #region シートのクリア

        #endregion

        #region シートイベント

        #endregion

        #endregion

        #region 選択行取得

        /// --------------------------------------------------
        /// <summary>
        /// 選択行取得
        /// </summary>
        /// <param name="rowIndex">選択行インデックス</param>
        /// <returns>選択行のDataRow</returns>
        /// <create>Y.Nakasato 2019/07/10</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override DataRow GetSelectedRowData(int rowIndex)
        {
            DataRow dr = base.GetSelectedRowData(rowIndex);
            try
            {
                // TODO : ここで選択行のデータを設定を変更する。
                // shtResultのDataSourceにDataSetかDataTableを設定している場合は変更不要です。
                // rowIndexには未選択時には-1が設定されています。
                return dr;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return null;
            }
        }

        #endregion

        #region リスト操作

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
            }
        }

        #endregion

    }
}
