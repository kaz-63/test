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

using SMS.A01.Properties;
using WsConnection.WebRefA01;

namespace SMS.A01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ進捗管理日付登録
    /// </summary>
    /// <create>T.Nakata 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShinchokuKanriDateTouroku : SystemBase.Forms.CustomOrderSearchDialog
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// ARリスト
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtARShinchokuRireki = null;

        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataSet _dsARShinchokuInfo = null;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = null;
        /// --------------------------------------------------
        /// <summary>
        /// 物件No
        /// </summary>
        /// <create>D.Okumura 2020/06/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo = null;

        /// --------------------------------------------------
        /// <summary>
        /// 通知用：エラーメッセージ
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string ErrMsgID { get; private set; }

        /// --------------------------------------------------
        /// <summary>
        /// 通知用：エラーメッセージパラメータ
        /// </summary>
        /// <create>T.Nakata 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public string[] ErrMsgArgs { get; private set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShinchokuKanriDateTouroku()
            : base()
        {
            InitializeComponent();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>T.Nakata 2019/07/26</create>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        public ShinchokuKanriDateTouroku(UserInfo userInfo, string title, DataTable dtARList, string bukkenNo)
            : base(userInfo, title)
        {
            InitializeComponent();

            this._dtARShinchokuRireki = dtARList.Copy();
            this._nonyusakiCd = ComFunc.GetFld(dtARList, 0, Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD);
            this._bukkenNo = bukkenNo;
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update>D.Okumura 2020/01/23 AR進捗クリア対応</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            // 不要な部品の無効化
            this.shtResult.Enabled = false;
            this.shtResult.Visible = false;
            this.fbrFunction.Enabled = false;
            this.fbrFunction.Visible = false;
            this.btnSearchAll.Enabled = false;
            this.btnSearchAll.Visible = false;
            this.btnSearch.Enabled = false;
            this.btnSearch.Visible = false;

            // 更新完了時にメッセージを表示しない: 親画面で表示するためこの画面では不要
            this.MsgUpdateEnd = string.Empty;

            // エラー発生時にウィンドウがチラつく為非表示&透明化で対処
            this.Visible = false;

            try
            {
                this._dsARShinchokuInfo = null;

                // AR情報のインターンロックを取得
                CondA1 cond = new CondA1(this.UserInfo);
                ConnA01 conn = new ConnA01();
                cond.NonyusakiCD = this._nonyusakiCd;

                // AR進捗情報取得
                string errMsgId;
                string[] errMsgArgs;
                _dsARShinchokuInfo = conn.GetARInterLockAndShinchokuInfo(cond, _dtARShinchokuRireki.Copy(), out errMsgId, out errMsgArgs);
                if (!string.IsNullOrEmpty(errMsgId))
                {   // エラーがある場合はCancelを返却し画面を閉じる
                    this.Opacity = 0; // ウィンドウが一瞬チラつく為透明化
                    if (errMsgId.Equals("A9999999080")) // 他端末で削除されています。
                        this.DialogResult = DialogResult.Retry;
                    else
                        this.DialogResult = DialogResult.Cancel;
                    this.ErrMsgID = errMsgId;
                    this.ErrMsgArgs = errMsgArgs;
                    return;
                }

                // 日付を設定
                string datestr = ComFunc.GetFld(this._dsARShinchokuInfo, ComDefine.DTTBL_ARSHINCHOKU_DT, 0, ComDefine.FLD_ARSHINCHOKU_DT_DATE);
                DateTime dateValue;
                if (DateTime.TryParse(datestr, out dateValue))
                {
                    this.dtpDate.Value = dateValue;
                    this.lblDate.IsNecessary = false;
                }
                else
                {
                    this.dtpDate.Value = null;
                    this.lblDate.IsNecessary = true;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

            // 閉じ確認フラグ設定(初期化完了後のみ)
            this.IsCloseQuestion = true;
            this.Visible = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.dtpDate.Focus();
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
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.dtpDate.Value = null;
                this.txtBiko.Text = string.Empty;
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
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>D.Okumura 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret =  base.CheckInput();
            if (!ret)
                return false;

            if (this.dtpDate.Value == null && this.lblDate.IsNecessary)
            {   // 日付を入力してください。
                this.ShowMessage("A0100051002");
                return false;
            }
            return ret;
        }
        #endregion

        #region 編集処理

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Okumura 2019/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            if (ret)
            {
                this.DialogResult = DialogResult.OK;
                this.IsCloseQuestion = false;
                this.Close();
            }
            return ret;
        }
        #endregion

        #region 修正処理
        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>D.Okumura 2019/07/30</create>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            bool ret = base.RunEditUpdate();
            if (!ret)
                return ret;

            // 登録処理
            CondA1 cond = new CondA1(this.UserInfo);
            ConnA01 conn = new ConnA01();
            cond.NonyusakiCD = this._nonyusakiCd;
            cond.BukkenNo = this._bukkenNo;

            string errMsgID;
            string[] args;

            DataSet ds = this.SetDtArShinchokuRireki();

            ret = conn.UpdARShinchokuInfo(cond, ds, out errMsgID, out args);
            if (!ret)
            {
                this.ShowMessage(errMsgID, args);
                return false;
            }

            return ret;
        }
        #endregion

        #region イベント

        #region ファンクションボタンクリック

        #endregion

        #region 決定ボタン
        /// --------------------------------------------------
        /// <summary>
        /// 決定ボタン
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnSelect_Click(object sender, EventArgs e)
        {
            this.EditMode = SystemBase.EditMode.Update;
            RunEdit();
        }
        #endregion

        #region Closeボタン
        /// --------------------------------------------------
        /// <summary>
        /// Closeボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 終了処理
        /// --------------------------------------------------
        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                if (!e.Cancel && this._dsARShinchokuInfo != null && this._dsARShinchokuInfo.Tables.Contains(Def_T_AR.Name))
                {
                    // インターンロック解除
                    CondA1 cond = new CondA1(this.UserInfo);
                    ConnA01 conn = new ConnA01();
                    cond.NonyusakiCD = this._nonyusakiCd;
                    string errMsgID;
                    string[] args;

                    bool ret = conn.UpdARUnLockShinchoku(cond, this._dsARShinchokuInfo.Tables[Def_T_AR.Name].Copy(), out errMsgID, out args);
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        e.Cancel = true;
                        return;
                    }
                    if (this.IsCloseQuestion)
                    {
                        this.DialogResult = DialogResult.Cancel;
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

        #region DataTable関連

        #region AR進捗データ履歴テーブル作成
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗データ履歴テーブル作成
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable makeDtArShinchokuRireki()
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU_RIREKI.Name);

            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD, typeof(string)); // 納入先コード
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.LIST_FLAG, typeof(string));    // リスト区分
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.AR_NO, typeof(string));        // ARNO
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.GOKI, typeof(string));         // 号機
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND, typeof(string));    // 日付種別
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_BEFORE, typeof(string));  // 変更前日付
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_AFTER, typeof(string));   // 変更後日付
            dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.NOTE, typeof(string));         // 備考

            return dt;
        }
        #endregion

        #region AR進捗情報設定
        /// --------------------------------------------------
        /// <summary>
        /// AR進捗情報設定
        /// </summary>
        /// <create>T.Nakata 2019/07/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataSet SetDtArShinchokuRireki()
        {
            DataSet ds = new DataSet();
            DataTable dtRireki = this.makeDtArShinchokuRireki();
            DataTable dtShinchoku = this._dsARShinchokuInfo.Tables[Def_T_AR_SHINCHOKU.Name].Copy();
            DataTable dtAr = this._dsARShinchokuInfo.Tables[Def_T_AR.Name].Copy();

            // 日付種別(入力データの1件目から取得)
            string DateKind = _dtARShinchokuRireki.Rows[0][Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND].ToString();
            string DateKindName = string.Empty;
            if (DateKind == AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1) DateKindName = Def_T_AR_SHINCHOKU.DATE_SITE_REQ;
            else if (DateKind == AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1) DateKindName = Def_T_AR_SHINCHOKU.DATE_LOCAL;
            else DateKindName = Def_T_AR_SHINCHOKU.DATE_JP;

            // テーブル更新
            string aftDTimeStr = this.dtpDate.Value == null ? "" : DateTime.Parse(this.dtpDate.Value.ToString()).ToString("yyyy/MM/dd");
            for (int i = 0; i < dtShinchoku.Rows.Count; i++)
            {   // 進捗履歴
                DataRow drRireki = dtRireki.NewRow();
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD]= dtShinchoku.Rows[i][Def_T_AR_SHINCHOKU.NONYUSAKI_CD];
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.LIST_FLAG]   = dtShinchoku.Rows[i][Def_T_AR_SHINCHOKU.LIST_FLAG];
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.AR_NO]       = dtShinchoku.Rows[i][Def_T_AR_SHINCHOKU.AR_NO];
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.GOKI]        = dtShinchoku.Rows[i][Def_T_AR_SHINCHOKU.GOKI];
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND]   = DateKind;
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.DATE_BEFORE] = dtShinchoku.Rows[i][DateKindName];
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.DATE_AFTER]  = aftDTimeStr;
                drRireki[Def_T_AR_SHINCHOKU_RIREKI.NOTE]        = this.txtBiko.Text;
                dtRireki.Rows.Add(drRireki);

                // AR進捗の日付変更
                dtShinchoku.Rows[i][DateKindName] = aftDTimeStr;
            }

            ds.Merge(dtAr);
            ds.Merge(dtRireki);
            ds.Merge(dtShinchoku);

            return ds;
        }
        #endregion

        #endregion

    }
}
