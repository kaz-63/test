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
using GrapeCity.Win.ElTabelle.Editors;
using ElTabelleHelper;

using WsConnection.WebRefK02;
using SMS.P02.Forms;
using MultiRowTabelle;
using SMS.K02.Properties;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包登録
    /// </summary>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KiwakuKonpo : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// グリッドに追加する列のタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update>2023/12/12 J.Chen 小数点第2位まで対応のDecimalを追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private enum ElTabelleColumnType
        {
            Text = 0,
            Int = 1,
            Decimal = 2,
            Button = 3,
            Decimal2 = 4,
        }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージの種類
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected enum DispMessageType
        {
            CheckInputSearchKojiNo,
            Shukkazumi,
            CheckEditDataDelete,
            CheckEditShukkazumi,
            CheckEditNoData,
            CheckEditKonpokanryo,
        }

        #endregion

        #region Fields

        private MultiRowFormat _multiFormat = null;
        private Dictionary<DispMessageType, string> _dispMessages = new Dictionary<DispMessageType, string>();

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 編集ボタン
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected const int COL_EDIT_BUTTON = 0;
        /// --------------------------------------------------
        /// <summary>
        /// PT明細ボタンの行
        /// </summary>
        /// <create>T.Sakiori 2012/04/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ROW_BUTTON_PT_MEISAI = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 2;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ(デザイナ用)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private KiwakuKonpo()
            : base(new UserInfo())
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
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        public KiwakuKonpo(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 画面メッセージ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual Dictionary<DispMessageType, string> DispMessages
        {
            get { return this._dispMessages; }
            set { this._dispMessages = value; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual string TorokuFlag
        {
            get { return TOROKU_FLAG.NAI_VALUE1; }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数行シートのフォーマット
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual MultiRowFormat MultiFormat
        {
            get { return this._multiFormat; }
            set { this._multiFormat = value; }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
                this.IsRunEditAfterClear = false;
                // 更新処理固定
                this.EditMode = SystemBase.EditMode.Update;
                // ----- メッセージの設定 -----
                // 工事識別NOを入力して下さい。
                this.SetDispMessageID(DispMessageType.CheckInputSearchKojiNo, "A9999999029");
                // 出荷済です。
                this.SetDispMessageID(DispMessageType.Shukkazumi, "A9999999031");
                // 他端末で削除されています。
                this.SetDispMessageID(DispMessageType.CheckEditDataDelete, "A9999999026");
                // 出荷済です。編集出来ません。
                this.SetDispMessageID(DispMessageType.CheckEditShukkazumi, "A9999999032");
                // 存在チェック
                this.SetDispMessageID(DispMessageType.CheckEditNoData, "A9999999030");
                // 梱包完了です。編集できません。
                this.SetDispMessageID(DispMessageType.CheckEditKonpokanryo, "A9999999033");

                // 複数行グリッドの初期化
                this.InitializeMultiRowSheet();

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[1, 0].Caption = Resources.KiwakuKonpo_CNo;
                shtMeisai.ColumnHeaders[2, 0].Caption = Resources.KiwakuKonpo_PkgStyle;
                shtMeisai.ColumnHeaders[3, 0].Caption = Resources.KiwakuKonpo_Mment;
                shtMeisai.ColumnHeaders[3, 1].Caption = Resources.KiwakuKonpo_WoodFrameWeight;
                shtMeisai.ColumnHeaders[4, 0].Caption = Resources.KiwakuKonpo_GrossW;
                shtMeisai.ColumnHeaders[4, 1].Caption = Resources.KiwakuKonpo_NetW;
                shtMeisai.ColumnHeaders[5, 0].Caption = Resources.KiwakuKonpo_Item;
                shtMeisai.ColumnHeaders[6, 0].Caption = Resources.KiwakuKonpo_Description;
                shtMeisai.ColumnHeaders[7, 0].Caption = Resources.KiwakuKonpo_Pallet;
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
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtKojiName.Focus();
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
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // 入力値のクリア
                this.txtKojiName.Text = string.Empty;
                this.txtShip.Text = string.Empty;
                this.txtKojiNo.Text = string.Empty;
                this.txtCaseMarkFile.Text = string.Empty;
                this.txtDeliveryNo.Text = string.Empty;
                this.txtPortOfDestination.Text = string.Empty;
                this.txtAirBoat.Text = string.Empty;
                this.txtDeliveryDate.Text = string.Empty;
                this.txtDeliveryPoint.Text = string.Empty;
                this.txtFactory.Text = string.Empty;
                this.txtRemarks.Text = string.Empty;
                // チェックの解除   
                this.rdoKonpoToroku.Checked = false;
                this.rdoKonpoKanryo.Checked = false;
                // グリッド
                this.SheetClear();
                // モードの切替
                this.ChangeEnableViewMode(false);
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
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtKojiName.Text) && !string.IsNullOrEmpty(this.DispMessages[DispMessageType.CheckInputSearchKojiNo]))
                {
                    this.ShowMessage(this.DispMessages[DispMessageType.CheckInputSearchKojiNo]);
                    this.txtKojiName.Focus();
                    return false;
                }
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
        /// <create>Y.Higuchi 2010/07/29</create>
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
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();

                cond.TorokuFlag = this.TorokuFlag;
                cond.KojiName = this.txtKojiName.Text;
                cond.Ship = this.txtShip.Text;
                string errMsgID;
                string[] args;
                DataSet ds = conn.GetKiwakuKonpoTorokuData(cond, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                DataTable dtKiwaku = ds.Tables[Def_T_KIWAKU.Name];

                // 木枠データのセット
                this.txtKojiNo.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
                this.txtCaseMarkFile.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                this.txtDeliveryNo.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_NO);
                this.txtPortOfDestination.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.PORT_OF_DESTINATION);
                this.txtAirBoat.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.AIR_BOAT);
                this.txtDeliveryDate.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_DATE);
                this.txtDeliveryPoint.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.DELIVERY_POINT);
                this.txtFactory.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.FACTORY);
                this.txtRemarks.Text = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.REMARKS);
                if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.KONPOKANRYO_VALUE1)
                {
                    this.rdoKonpoKanryo.Checked = true;
                }
                else
                {
                    this.rdoKonpoToroku.Checked = true;
                }
                // 木枠明細データのセット
                this.shtMeisai.SetMultiRowDataSource(ds, Def_T_KIWAKU_MEISAI.Name, this.MultiFormat);
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 再描画
                    this.DisplayRefresh();
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();
                cond.KojiNo = this.txtKojiNo.Text;
                cond.TorokuFlag = this.TorokuFlag;
                if (this.rdoKonpoKanryo.Checked)
                {
                    cond.SagyoFlag = SAGYO_FLAG.KONPOKANRYO_VALUE1;
                }
                else
                {
                    cond.SagyoFlag = SAGYO_FLAG.KONPOTOROKU_VALUE1;
                }

                string errMsgID;
                string[] args;
                if (!conn.UpdKiwakuKonpoToroku(cond, out errMsgID, out args))
                {
                    if (errMsgID == "A9999999034")
                    {
                        this.DisplayRefresh();
                    }
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // 入力値のクリア
                this.txtKojiNo.Text = string.Empty;
                this.txtCaseMarkFile.Text = string.Empty;
                this.txtDeliveryNo.Text = string.Empty;
                this.txtPortOfDestination.Text = string.Empty;
                this.txtAirBoat.Text = string.Empty;
                this.txtDeliveryDate.Text = string.Empty;
                this.txtDeliveryPoint.Text = string.Empty;
                this.txtFactory.Text = string.Empty;
                this.txtRemarks.Text = string.Empty;
                // チェックの解除   
                this.rdoKonpoToroku.Checked = false;
                this.rdoKonpoKanryo.Checked = false;
                // グリッドのクリア
                this.SheetClear();
                // モードの切替
                this.ChangeEnableViewMode(false);
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.txtKojiName.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 表示

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/20</create>
        /// <update>H.Tajimi 2015/11/30 納入先のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // ----- 表示 -----
                // 2015/11/30 H.Tajimi 納入先のUI改善
                if (!this.ShowKojiShikibetsuIchiran())
                {
                    this.txtKojiName.Focus();
                    return;
                }
                // ↑
                // 検索条件のロック
                this.ChangeEnableViewMode(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region グリッド

        #region CellNotify

        /// --------------------------------------------------
        /// <summary>
        /// セルのイベントが発生したときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update>H.Tajimi 2019/01/07 木枠梱包画面で明細表示前のグリッド位置復元</update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                if ((this.shtMeisai.CellEditor as ButtonEditor) != null)
                {
                    //セルのイベント処理です
                    switch (e.Name)
                    {
                        case CellNotifyEvents.Click:
                            this.ClearMessage();
                            switch (e.Position.Column)
                            {
                                case COL_EDIT_BUTTON:
                                    // 編集ボタン
                                    DataTable dt = this.shtMeisai.GetMultiRowGetDataSource();
                                    int dataIndex = UtilConvert.ToInt32(Math.Floor((decimal)(e.Position.Row / this.MultiFormat.Rows.Count)));
                                    if (dt == null || dt.Rows.Count <= dataIndex) return;
                                    string kojiNo = ComFunc.GetFld(dt, dataIndex, Def_T_KIWAKU.KOJI_NO);
                                    string caseID = ComFunc.GetFld(dt, dataIndex, Def_T_KIWAKU_MEISAI.CASE_ID);

                                    CondK02 cond = new CondK02(this.UserInfo);
                                    ConnK02 conn = new ConnK02();
                                    cond.KojiNo = kojiNo;
                                    cond.CaseID = caseID;

                                    string errMsgID;
                                    string[] args;
                                    if (!conn.CheckKiwakuKonpoTorokuEdit(cond, out errMsgID, out args))
                                    {
                                        if (!string.IsNullOrEmpty(errMsgID))
                                        {
                                            // 再描画が必要かチェック
                                            if (this.IsRefresh(errMsgID))
                                            {
                                                // 再描画
                                                this.DisplayRefresh();
                                            }
                                            this.ShowMessage(errMsgID, args);
                                        }
                                        return;
                                    }

                                    // 木枠梱包明細登録を呼び出す
                                    KiwakuKonpoMeisai frm = null;
                                    try
                                    {
                                        if (e.Position.Row % 2 == ROW_BUTTON_PT_MEISAI)
                                        {
                                            frm = new KiwakuKonpoMeisai(this.UserInfo, kojiNo, caseID);
                                        }
                                        else
                                        {
                                            frm = new ShagaiKiwakuKonpoMeisai(this.UserInfo, kojiNo, caseID);
                                        }
                                        frm.Icon = this.Icon;
                                        if (frm.ShowDialog() == DialogResult.OK)
                                        {
                                            // 再描画
                                            this.DisplayRefresh(frm);
                                        }
                                    }
                                    finally
                                    {
                                        if (frm != null)
                                        {
                                            frm.Dispose();
                                        }
                                    }

                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">表示状態かどうか</param>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 表示コントロールのロック/ロック解除
            this.pnlView.Enabled = isView;
            this.shtMeisai.Enabled = isView;
            // 検索条件のロック/ロック解除
            this.grpSearch.Enabled = !isView;
            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
            // 2015/12/02 H.Tajimi クリアボタン制御
            // Clearボタン
            this.fbrFunction.F06Button.Enabled = isView;
            // ↑
        }

        #endregion

        #region MultiTabelleのフォーマット作成

        /// --------------------------------------------------
        /// <summary>
        /// 複数行表示グリッドの初期化
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update>2023/12/12 J.Chen 小数点第2位まで対応修正</update>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void InitializeMultiRowSheet()
        {
            this.InitializeSheet(this.shtMeisai);

            MultiRowFormat format = new MultiRowFormat();
            format.SetRowNum(2);
            // 表示列部分の設定
            // ボタン名は固定出力のため、フィールド名はダミー。
            this.SetFormat(format, ElTabelleColumnType.Button, 0, 0, 1, 1, "BUTTON_PT_MEISAI", true, MultiRowCell.DefaultEnterKeyActions, Resources.KiwakuKonpo_ButtonTextPtMeisai);
            this.SetFormat(format, ElTabelleColumnType.Button, 0, 1, 1, 1, "BUTTON_TAG_MEISAI", true, MultiRowCell.DefaultEnterKeyActions, Resources.KiwakuKonpo_ButtonTextTagMeisai);
            this.SetFormat(format, ElTabelleColumnType.Int, 1, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_NO, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 2, 0, 1, 2, Def_T_KIWAKU_MEISAI.STYLE, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Decimal, 3, 0, 1, 1, Def_T_KIWAKU_MEISAI.MMNET, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
            this.SetFormat(format, ElTabelleColumnType.Decimal2, 3, 1, 1, 1, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextCell, KeyAction.PrevRow));
            this.SetFormat(format, ElTabelleColumnType.Decimal2, 4, 0, 1, 1, Def_T_KIWAKU_MEISAI.GROSS_W, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
            this.SetFormat(format, ElTabelleColumnType.Decimal2, 4, 1, 1, 1, Def_T_KIWAKU_MEISAI.NET_W, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 5, 0, 1, 2, Def_T_KIWAKU_MEISAI.ITEM, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextCell, KeyAction.PrevRow));
            this.SetFormat(format, ElTabelleColumnType.Text, 6, 0, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, false, MultiRowCell.GetEnterKeyActionArray(KeyAction.NextRow));
            this.SetFormat(format, ElTabelleColumnType.Text, 6, 1, 1, 1, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 7, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_1, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 8, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_2, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 9, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_3, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 10, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_4, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 11, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_5, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 12, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_6, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 13, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_7, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 14, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_8, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 15, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_9, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 16, 0, 1, 2, Def_T_KIWAKU_MEISAI.PALLET_NO_10, false, MultiRowCell.DefaultEnterKeyActions);
            // 非表示部分の設定
            this.SetFormat(format, ElTabelleColumnType.Text, 17, 0, 1, 2, Def_T_KIWAKU.KOJI_NO, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 18, 0, 1, 2, Def_T_KIWAKU.KOJI_NAME, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 19, 0, 1, 2, Def_T_KIWAKU.SHIP, false, MultiRowCell.DefaultEnterKeyActions);
            this.SetFormat(format, ElTabelleColumnType.Text, 20, 0, 1, 2, Def_T_KIWAKU_MEISAI.CASE_ID, false, MultiRowCell.DefaultEnterKeyActions);
            this.MultiFormat = format;

            if (this.TorokuFlag == TOROKU_FLAG.GAI_VALUE1)
            {
                // パレット1～10を非表示にする
                this.shtMeisai.Columns[7].Hidden = true;
                this.shtMeisai.Columns[8].Hidden = true;
                this.shtMeisai.Columns[9].Hidden = true;
                this.shtMeisai.Columns[10].Hidden = true;
                this.shtMeisai.Columns[11].Hidden = true;
                this.shtMeisai.Columns[12].Hidden = true;
                this.shtMeisai.Columns[13].Hidden = true;
                this.shtMeisai.Columns[14].Hidden = true;
                this.shtMeisai.Columns[15].Hidden = true;
                this.shtMeisai.Columns[16].Hidden = true;
            }
        }

        #endregion

        #region 複数行表示のフォーマット設定

        /// --------------------------------------------------
        /// <summary>
        /// 複数行表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, bool isEnabled, KeyAction[] actions)
        {
            SetFormat(format, colType, col, row, colSpan, rowSpan, dataField, isEnabled, actions, null);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 複数行表示のフォーマット設定
        /// </summary>
        /// <param name="format">MultiRowFormat</param>
        /// <param name="colType">グリッドに追加する列のタイプ</param>
        /// <param name="col">列のインデックス</param>
        /// <param name="row">行のインデックス</param>
        /// <param name="colSpan">列の連結数</param>
        /// <param name="rowSpan">行の連結数</param>
        /// <param name="dataField">データフィールド</param>
        /// <param name="isEnabled">セルが操作可能かどうか</param>
        /// <param name="actions">Enterキー押下時のアクション</param>
        /// <param name="buttonText">ボタンの名称</param>
        /// <create>Y.Higuchi 2010/07/23</create>
        /// <update>2023/12/12 J.Chen 小数点第2位まで対応のDecimalを追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFormat(MultiRowFormat format, ElTabelleColumnType colType, int col, int row, int colSpan, int rowSpan, string dataField, bool isEnabled, KeyAction[] actions, string buttonText)
        {
            MultiRowCell cell = new MultiRowCell(col, colSpan, rowSpan, dataField, actions);

            switch (colType)
            {
                case ElTabelleColumnType.Text:
                    TextEditor textEditor = ElTabelleSheetHelper.NewTextEditor();
                    cell.Editor = textEditor;
                    break;
                case ElTabelleColumnType.Int:
                    NumberEditor intEditor = ElTabelleSheetHelper.NewNumberEditor();
                    intEditor.DisplayFormat = new NumberFormat("###,###,##0", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    intEditor.Format = new NumberFormat("###,###,##0", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    cell.Editor = intEditor;
                    break;
                case ElTabelleColumnType.Decimal:
                    NumberEditor decimalEditor = ElTabelleSheetHelper.NewNumberEditor();
                    decimalEditor.DisplayFormat = new NumberFormat("###,###,##0.000", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    decimalEditor.Format = new NumberFormat("###,###,##0.000", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    cell.Editor = decimalEditor;
                    break;
                case ElTabelleColumnType.Button:
                    ButtonEditor buttonEditor = ElTabelleSheetHelper.NewButtonEditor();
                    buttonEditor.Text = buttonText;
                    if (!string.IsNullOrEmpty(buttonText))
                        cell.Text = buttonText;
                    cell.Editor = buttonEditor;
                    break;
                case ElTabelleColumnType.Decimal2:
                    NumberEditor decimalEditor2 = ElTabelleSheetHelper.NewNumberEditor();
                    decimalEditor2.DisplayFormat = new NumberFormat("###,###,##0.00", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    decimalEditor2.Format = new NumberFormat("###,###,##0.00", string.Empty, string.Empty, "-", string.Empty, "0", string.Empty);
                    cell.Editor = decimalEditor2;
                    break;
            }
            cell.Enabled = isEnabled;
            format.Rows[row].Cells.Add(cell);
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.MultiRowAllClear();
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 表示用メッセージIDの設定

        /// --------------------------------------------------
        /// <summary>
        /// 表示用メッセージの設定
        /// </summary>
        /// <param name="type">メッセージ種類</param>
        /// <param name="messageID">メッセージID</param>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void SetDispMessageID(DispMessageType type, string messageID)
        {
            if (!this.DispMessages.ContainsKey(type))
            {
                this.DispMessages.Add(type, messageID);
            }
            else
            {
                this.DispMessages[type] = messageID;
            }
        }
        #endregion

        #region メッセージIDチェック

        /// --------------------------------------------------
        /// <summary>
        /// 画面を再描画させる必要のあるメッセージかチェック
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>true:必要/false:不要</returns>
        /// <create>Y.Higuchi 2010/07/29</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual bool IsRefresh(string messageID)
        {
            List<string> msgs = new List<string>();
            msgs.Add(this.DispMessages[DispMessageType.CheckEditDataDelete]);

            if (msgs.Contains(messageID))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 画面再描画(再検索)

        /// --------------------------------------------------
        /// <summary>
        /// 画面再描画(再検索)
        /// </summary>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void DisplayRefresh()
        {
            // 再描画
            if (!this.RunSearch())
            {
                // 再描画時に失敗したので画面をクリアする。
                this.DisplayClear();
            }
            else
            {
                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 画面再描画(再検索) ※カーソル位置復元
        /// </summary>
        /// <param name="frm">明細(PT,TAG)のフォーム</param>
        /// <create>H.Tajimi 2019/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected virtual void DisplayRefresh(KiwakuKonpoMeisai frm)
        {
            if (frm == null)
            {
                this.DisplayRefresh();
            }
            else
            {
                // 再描画
                if (!this.RunSearch())
                {
                    // 再描画時に失敗したので画面をクリアする。
                    this.DisplayClear();
                }
                else
                {
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtMeisai.MaxRows)
                    {
                        var dt = this.shtMeisai.GetMultiRowGetDataSource();
                        if (dt == null)
                        {
                            return;
                        }

                        for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                        {
                            if (ComFunc.GetFld(dt, rowIndex, Def_T_KIWAKU_MEISAI.KOJI_NO) == frm.CondKojiNo
                             && ComFunc.GetFld(dt, rowIndex, Def_T_KIWAKU_MEISAI.CASE_ID) == frm.CondCaseID)
                            {
                                this.shtMeisai.Redraw = false;
                                this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, rowIndex * 2);
                                this.shtMeisai.Redraw = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 画面表示

        #region 工事識別一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 工事識別一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowKojiShikibetsuIchiran()
        {
            string torokuFlag = this.TorokuFlag;
            string kojiName = this.txtKojiName.Text;
            string ship = this.txtShip.Text;
            using (KojiShikibetsuIchiran frm = new KojiShikibetsuIchiran(this.UserInfo, torokuFlag, kojiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtKojiName.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                    this.txtKojiNo.Text = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion
    }
}
