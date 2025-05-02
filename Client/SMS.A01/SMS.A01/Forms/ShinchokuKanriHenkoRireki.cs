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
using SMS.A01.Properties;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 変更履歴
    /// </summary>
    /// <create>Y.Nakasato 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuKanriHenkoRireki : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = string.Empty;

        /// --------------------------------------------------
        /// <summary>
        /// 納入先名
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiName = string.Empty;

        /// --------------------------------------------------
        /// <summary>
        /// 範囲指定時のセパレータ
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private char _rangeStrings = ' ';

        /// --------------------------------------------------
        /// <summary>
        /// 複数指定時区切り文字列
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private char _separatorStrings = ' ';

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件の日付範囲指定の開始日付
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime? _rangeDateFrom = null;

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件の日付範囲指定の終了日付
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private DateTime? _rangeDateTo = null;

        #endregion

        #region コンストラクタ
        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="title">フォームタイトル</param>
        /// <param name="nonyusakiCd">納入先コード</param>
        /// <param name="nonyusakiName">納入先名</param>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuKanriHenkoRireki(UserInfo userInfo, string title, string nonyusakiCd, string nonyusakiName)
            : base(userInfo, title)
        {
            InitializeComponent();
            this._nonyusakiCd = nonyusakiCd;
            this._nonyusakiName = nonyusakiName;
            this._rangeStrings = this.UserInfo.SysInfo.SeparatorRange;
            this._separatorStrings = this.UserInfo.SysInfo.SeparatorItem;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // シートの初期化
                this.InitializeSheet(this.shtResult);
                this.SheetClear();

                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.ShinchokuRireki_KoushinDate;
                shtResult.ColumnHeaders[1].Caption = Resources.ShinchokuRireki_KoushinUserID;
                shtResult.ColumnHeaders[2].Caption = Resources.ShinchokuRireki_ARNo;
                shtResult.ColumnHeaders[3].Caption = Resources.ShinchokuRireki_Goki;
                shtResult.ColumnHeaders[4].Caption = Resources.ShinchokuRireki_Date;
                shtResult.ColumnHeaders[5].Caption = Resources.ShinchokuRireki_BeforeDate;
                shtResult.ColumnHeaders[6].Caption = Resources.ShinchokuRireki_AfterDate;
                shtResult.ColumnHeaders[7].Caption = Resources.ShinchokuRireki_Biko;

                // コンボボックスの初期化
                this.MakeCmbBox(cboDatekubun, AR_DATE_KIND_DISP_FLAG.GROUPCD);

                // 更新日時絞り込み
                DataTable dt = this.GetCommon(AR_SHINCHOKU_SETTING.GROUPCD).Tables[Def_M_COMMON.Name];
                DataRow dr = dt.Select(Def_M_COMMON.ITEM_CD + " = '" + AR_SHINCHOKU_SETTING.HISTORY_NAME + "'").FirstOrDefault();
                int searchDateFromAdd = ComFunc.GetFldToInt32(dr, Def_M_COMMON.VALUE1);
                dtpUpdateDateFrom.Value = System.DateTime.Now.Date.AddMonths(searchDateFromAdd * -1);
                dtpUpdateDateTo.Value = System.DateTime.Now.Date;

                // 凡例
                DataTable dtHanrei = this.GetCommon(AR_HANREI.GROUPCD).Tables[Def_M_COMMON.Name];
                var arNoHanrei = ComFunc.GetFld(dtHanrei.AsEnumerable().FirstOrDefault(w => string.Equals(ComFunc.GetFld(w, Def_M_COMMON.ITEM_CD), AR_HANREI.AR_NO_RANGE_NAME)), Def_M_COMMON.ITEM_NAME);
                this.SetToolTip(txtARNo, arNoHanrei);

                // テキストボックスの初期化
                txtNonyusakiName.Text = _nonyusakiName;
                txtARNo.Text = "";
                txtKishu.Text = "";
                txtGoki.Text = "";

                // コンボボックスの選択状態
                cboDatekubun.SelectedIndex = 0;
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
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {

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
        /// <returns></returns>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                { // 1:「期間」の大小チェックエラーの場合、次のメッセージを表示し処理を抜ける。																			
                    _rangeDateFrom = this.dtpUpdateDateFrom.Value as DateTime?;
                    _rangeDateTo = this.dtpUpdateDateTo.Value as DateTime?;
                    if ((_rangeDateFrom != null) && (_rangeDateTo != null) && (_rangeDateFrom > _rangeDateTo))
                    {
                        // 期間の範囲が不正です。期間を確認してください。
                        this.ShowMessage("A9999999078");
                        this.dtpUpdateDateFrom.Focus();
                        return false;
                    }
                }
                { // 2:「ARNO」がハイフンで分割後、数値でないまたは、4桁以外で無い場合、次のメッセージを表示し処理を抜ける。																			
                    if (!ComFunc.CheckARNo(txtARNo.Text, _rangeStrings))
                    {
                        // ARNoの入力が不正です。確認してください。
                        this.ShowMessage("A9999999077");
                        this.txtARNo.Focus();
                        return false;
                    }
                }
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
            try
            {
                var conn = new ConnA01();
                var cond = new CondA1(this.UserInfo);

                cond.NonyusakiCD = this._nonyusakiCd;
                cond.NonyusakiName = this._nonyusakiName;
                cond.ArNo = this.txtARNo.Text;
                cond.UpdateDateFrom = this._rangeDateFrom == null ? (DateTime?)null : 
                                            UtilConvert.ToDateTime(this._rangeDateFrom.Value).Date;
                cond.UpdateDateTo = this._rangeDateTo == null ? (DateTime?)null : 
                                            UtilConvert.ToDateTime(this._rangeDateTo.Value).Date.AddDays(1);
                cond.Kishu = this.txtKishu.Text;
                cond.Goki = this.txtGoki.Text;
                cond.SeparatorRange = this._rangeStrings;
                cond.SeparatorItem = this._separatorStrings;
                cond.DateKubun = (string)this.cboDatekubun.SelectedValue;
                var ds = conn.GetRireki(cond);
                if (ds.Rows.Count == 0)
                {
                    // 該当する進捗履歴はありません。
                    this.ShowMessage("A0100060001");
                }
                else
                {

                    this.shtResult.Redraw = false;
                    this.shtResult.DataSource = ds;
                    this.shtResult.Enabled = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtResult.Redraw = true;
            }
        }

        #endregion

        #region イベント

        #region ファンクションボタンクリック
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.SheetClear();

                this.RunSearch();

                this.shtResult.Focus();
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 機種一覧

        /// --------------------------------------------------
        /// <summary>
        /// 機種一覧ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnKishu_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var form = new ShinchokuKishuIchiran(this.UserInfo, ComDefine.TITLE_A0100052, this._nonyusakiCd, this.txtKishu.Text))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        this.txtKishu.Text = form.Kishu;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
        #endregion

        #region 号機一覧
        /// --------------------------------------------------
        /// <summary>
        /// 号機一覧ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnGoki_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                using (var form = new ShinchokuGokiIchiran(this.UserInfo, ComDefine.TITLE_A0100053, this._nonyusakiCd, this.txtGoki.Text))
                {
                    form.Icon = this.Icon;
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        this.txtGoki.Text = form.Goki;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
        #endregion
        #endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>Y.Nakasato 2019/07/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtResult.Redraw = false;
            if (0 < this.shtResult.MaxRows)
            {
                this.shtResult.TopLeft = new Position(0, 0);
            }
            this.shtResult.DataSource = null;
            this.shtResult.MaxRows = 0;
            this.shtResult.Enabled = false;
            this.shtResult.Redraw = true;
        }

        #endregion

    }
}
