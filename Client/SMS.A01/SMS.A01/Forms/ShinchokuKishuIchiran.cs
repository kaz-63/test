using System;
using System.Linq;
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
using WsConnection.WebRefA01;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 機種ダイアログ
    /// </summary>
    /// <create>Y.Nakasato 2019/07/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuKishuIchiran : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields
        /// --------------------------------------------------
        /// <summary>
        /// 引数の選択中機種名リスト
        /// </summary>
        /// <create>Y.Nakasato 2019/07/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _kishuList = { };
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

        #endregion //Fields

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>Y.Nakasato 2019/07/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Kishu { get; private set; }

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
        public ShinchokuKishuIchiran(UserInfo userInfo, string title, string NonyusakiCD, string kishu)
            : base(userInfo, title)
        {
            InitializeComponent();

            this._separator = userInfo.SysInfo.SeparatorItem;
            this.Kishu = kishu;
            this._kishuList = kishu.Split(this._separator);
            this._nonyusakiCD = NonyusakiCD;

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
                var dt = conn.GetKishu(cond);
                int maxLen = 0;
                var kishuList = new List<string>();

                // 機種名の最長文字数を調査しながらリスト生成
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var len = UtilString.GetByteCount(ComFunc.GetFld(dt, i, Def_T_AR_GOKI.KISHU));
                    if (maxLen < len)
                    {
                        maxLen = len;
                    }
                    kishuList.Add(ComFunc.GetFld(dt, i, Def_T_AR_GOKI.KISHU));
                }

                // 機種リスト作成(ついでにチェックもつける)
                foreach (string kishu in kishuList)
                {
                    if (string.IsNullOrEmpty(kishu))
                    {
                        continue;
                    }

                    var item = new ListViewItem();
                    item.Tag = kishu;
                    item.Text = kishu + string.Empty.PadRight(maxLen - UtilString.GetByteCount(kishu));
                    item.Checked = false;
                    foreach (string selectKishu in _kishuList)
                    {
                        if (kishu == selectKishu)
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                    lstKishu.Items.Add(item);
                }
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
                lstKishu.Focus();
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
                // TODO : ベースでClearMessageの呼出しは行われています。
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

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
            List<string> list = new List<string>();

            this.Kishu = "";

            // チェックOnの機種のみでリスト作成
            foreach (ListViewItem item in lstKishu.Items)
            {
                if (item.Checked)
                {
                    list.Add((string)item.Tag);
                }
            }

            // カンマで繋げる
            if (list.Count > 0)
            {
                this.Kishu = string.Join(this._separator.ToString(), list.ToArray());
            }
            return ret;
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
        /// 号機選択画面に合わせる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void lstKishu_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if ((lstKishu.SelectedItems.Count > 0)
             && (!lstKishu.Items[e.Index].Selected))
            {
                lstKishu.SelectedItems.Clear();  // セル選択を解除
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
            foreach (ListViewItem item in lstKishu.Items)
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
            if (lstKishu.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (ListViewItem item in lstKishu.SelectedItems)
            {
                item.Checked = check;
            }
        }

        #endregion

    }
}
