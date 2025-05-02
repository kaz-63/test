using System;
using System.Data;
using System.Windows.Forms;
using Commons;
using DSWUtil;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefP02;
using System.Collections.Generic;
using System.Linq;
using SMS.P02.Properties;
using System.Text.RegularExpressions;
using GrapeCity.Win.ElTabelle;

namespace SMS.P02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 送信先設定
    /// </summary>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class SenderSetting : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 選択済みのユーザー
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private List<string> _userList = new List<string>();

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// 送信先データ
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataTable SendData { get { return this.shtSend.DataSource as DataTable; } }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// 送信先設定(デザイナ用)
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private SenderSetting()
            : base()
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = ComDefine.TITLE_P0200060;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 送信先設定
        /// </summary>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="userList">選択済みのユーザー</param>
        /// <param name="mailAddressFlag">メールフラグ</param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public SenderSetting(UserInfo userInfo, object userList, string mailAddressFlag)
            : base(userInfo)
        {
            InitializeComponent();
            // 画面タイトル設定
            this.Title = string.Format("{0}({1})", ComDefine.TITLE_P0200060, mailAddressFlag);

            this._userList = userList as List<string>;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update>H.Tajimi 2018/10/17 複数宛先を選択して一括選択を可能とする</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            try
            {
                // コントロールの初期化
                // アイコンの設定
                this.Icon = ComFunc.BitmapToIcon(ComResource.Search);

                this.InitializeSheet(this.shtSend);
                this.shtResult.SelectionType = GrapeCity.Win.ElTabelle.SelectionType.MultipleRanges;

                this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.SenderSetting_UserName;
                shtSend.ColumnHeaders[0].Caption = Resources.SenderSetting_Sender;
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
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.RunSearch();
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
        /// <create>T.Sakiori 2017/09/14</create>
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
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                var conn = new ConnP02();
                var cond = new CondP02(this.UserInfo);
                var dt = conn.GetSendUser(cond);

                if (this.shtSend.DataSource != null)
                {
                    var dtSend = this.shtSend.DataSource as DataTable;
                    var tmp = dt.Clone();
                    if (this.IsSearchAll)
                    {
                        dt.AsEnumerable().ToList().ForEach(x =>
                        {
                            if (!dtSend.AsEnumerable().Any(y => UtilData.GetFld(y, Def_M_USER.USER_ID) == UtilData.GetFld(x, Def_M_USER.USER_ID)))
                            {
                                tmp.Rows.Add(x.ItemArray);
                            }
                        });
                    }
                    else
                    {
                        dt.AsEnumerable().Where(x => UtilData.GetFld(x, Def_M_USER.USER_NAME).Contains(this.txtSearchUserName.Text)).ToList().ForEach(x =>
                        {
                            if (!dtSend.AsEnumerable().Any(y => UtilData.GetFld(y, Def_M_USER.USER_ID) == UtilData.GetFld(x, Def_M_USER.USER_ID)))
                            {
                                tmp.Rows.Add(x.ItemArray);
                            }
                        });
                    }
                    this.shtResult.DataSource = tmp;
                }
                else
                {
                    // 前画面から持ってきたユーザーは選択済みデータ
                    var dtSend = dt.Clone();
                    this._userList.ForEach(userId =>
                    {
                        var dr = dt.AsEnumerable().Where(x => UtilData.GetFld(x, Def_M_BUKKEN_MAIL_MEISAI.USER_ID) == userId).FirstOrDefault();
                        if (dr != null)
                        {
                            dtSend.Rows.Add(dr.ItemArray);
                            dt.Rows.Remove(dr);
                        }
                    });

                    // 残りが未選択ユーザー
                    if (!this.IsSearchAll)
                    {
                        var tmp = dt.Clone();
                        dt.AsEnumerable().Where(x => UtilData.GetFld(x, Def_M_USER.USER_NAME).Contains(this.txtSearchUserName.Text)).ToList().ForEach(x => tmp.Rows.Add(x.ItemArray));
                        dt = tmp;
                    }

                    this.shtResult.DataSource = dt;
                    this.shtSend.DataSource = dtSend;
                }

                this.shtResult.Focus();
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
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtSearchUserName_KeyDown(object sender, KeyEventArgs e)
        {
            // Enterキーが押下された時は、検索ボタンにフォーカス遷移する
            if ((e.KeyCode == Keys.Enter) && !e.Alt && !e.Control)
            {
                this.btnSearch.Focus();
            }
        }

        #endregion

        #region ボタンイベント

        /// --------------------------------------------------
        /// <summary>
        /// 選択ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update>H.Tajimi 2018/10/17 複数宛先を選択して一括選択を可能とする</update>
        /// --------------------------------------------------
        private void btnSet_Click(object sender, EventArgs e)
        {
            int index = this.shtResult.ActivePosition.Row;
            if (index == -1)
            {
                return;
            }

            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtResult.DataSource as DataTable;
                var dtDest = this.shtSend.DataSource as DataTable;
                var dv = dtSource.DefaultView;

                // 複数選択されていた場合、下から削除しないと行がずれてしまうため
                // 選択されている行インデックスを降順に並べ替える
                var lstRowIndex = this.GetSelectedIndexList(this.shtResult, BlocksType.SelectedRows, true);
                foreach (var rowIndex in lstRowIndex)
                {
                    dtDest.Rows.Add(dv[rowIndex].Row.ItemArray);
                    dv[rowIndex].Delete();
                }
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 行削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRowDelete_Click(object sender, EventArgs e)
        {
            int index = this.shtSend.ActivePosition.Row;
            if (index == -1)
            {
                return;
            }

            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtSend.DataSource as DataTable;
                var dtDest = this.shtResult.DataSource as DataTable;
                dtDest.Rows.Add(dtSource.Rows[index].ItemArray);
                var dtDestCopy = dtDest.Clone();
                dtDest.AsEnumerable().OrderBy(x => x.Field<string>(Def_M_USER.USER_ID)).ToList().ForEach(x => dtDestCopy.Rows.Add(x.ItemArray));
                this.shtResult.DataSource = dtDestCopy;
                dtSource.Rows.RemoveAt(index);
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// All Clearボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtSend.DataSource as DataTable;
                var dtDest = this.shtResult.DataSource as DataTable;
                for (int index = 0; index < dtSource.Rows.Count; )
                {
                    dtDest.Rows.Add(dtSource.Rows[index].ItemArray);
                    dtSource.Rows.RemoveAt(index);
                }
                var dtDestCopy = dtDest.Clone();
                dtDest.AsEnumerable().OrderBy(x => x.Field<string>(Def_M_USER.USER_ID)).ToList().ForEach(x => dtDestCopy.Rows.Add(x.ItemArray));
                this.shtResult.DataSource = dtDestCopy;
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
            }
        }

        #endregion

        #region シートイベント

        /// --------------------------------------------------
        /// <summary>
        /// 左側シートのセルダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtResult_CellDoubleClick(object sender, GrapeCity.Win.ElTabelle.ClickEventArgs e)
        {
            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtResult.DataSource as DataTable;
                var dtDest = this.shtSend.DataSource as DataTable;
                dtDest.Rows.Add(dtSource.Rows[e.Row].ItemArray);
                dtSource.Rows.RemoveAt(e.Row);
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 右側シートのセルダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2017/09/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtSend_CellDoubleClick(object sender, GrapeCity.Win.ElTabelle.ClickEventArgs e)
        {
            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtSend.DataSource as DataTable;
                var dtDest = this.shtResult.DataSource as DataTable;
                dtDest.Rows.Add(dtSource.Rows[e.Row].ItemArray);
                var dtDestCopy = dtDest.Clone();
                dtDest.AsEnumerable().OrderBy(x => x.Field<string>(Def_M_USER.USER_ID)).ToList().ForEach(x => dtDestCopy.Rows.Add(x.ItemArray));
                this.shtResult.DataSource = dtDestCopy;
                dtSource.Rows.RemoveAt(e.Row);
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
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
        /// <create>T.Sakiori 2017/09/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                // 左右のデータの如何を問わず
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 一括追加

        /// --------------------------------------------------
        /// <summary>
        /// 一括追加ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnMassAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMassAdd.Text))
            {
                return;
            }

            this.shtResult.Redraw = false;
            this.shtSend.Redraw = false;
            try
            {
                var dtSource = this.shtResult.DataSource as DataTable;
                var dtDest = this.shtSend.DataSource as DataTable;
                var separators = ",";
                foreach (var item in this.txtMassAdd.Text.Split(separators.ToCharArray()))
                {
                    if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(item.Trim()))
                    {
                        // 空もしくは、空白のみは次へ
                        continue;
                    }
                    
                    var name = item.Trim();
                    if (!dtSource.AsEnumerable().Any(x => x.Field<string>(Def_M_USER.USER_NAME) == name))
                    {
                        // 担当者名に存在しない場合は何もしない
                        continue;
                    }
                    if (dtDest.AsEnumerable().Any(x => x.Field<string>(Def_M_USER.USER_NAME) == name))
                    {
                        // 送信先に既に含まれている場合は何もしない
                        continue;
                    }

                    for (int rowIndex = 0; rowIndex < this.shtResult.Rows.Count - 1; rowIndex++)
                    {
                        var text = this.shtResult[0, rowIndex].Text;
                        if (name == text)
                        {
                            dtDest.Rows.Add(dtSource.Rows[rowIndex].ItemArray);
                            dtSource.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtResult.Redraw = true;
                this.shtSend.Redraw = true;
            }
        }
        
        #endregion

        #endregion

        #region ElTabelle Sheet用メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 選択されている行インデックスリストを取得
        /// </summary>
        /// <param name="sheet">ElTabelle</param>
        /// <param name="blocksType">選択タイプ</param>
        /// <param name="isDesc">降順に並べ替えるかどうか</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2018/10/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual List<int> GetSelectedIndexList(Sheet sheet, BlocksType blocksType, bool isDesc)
        {
            var selectedRange = sheet.GetBlocks(blocksType);
            var lstSelected = new List<int>();
            foreach (var range in selectedRange)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    if (!lstSelected.Contains(rowIndex))
                    {
                        lstSelected.Add(rowIndex);
                    }
                }
            }
            lstSelected.Sort();

            if (isDesc)
            {
                lstSelected.Reverse();
            }
            return lstSelected;
        }

        #endregion
    }
}
