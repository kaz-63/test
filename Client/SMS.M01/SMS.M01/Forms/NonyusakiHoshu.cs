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

using WsConnection.WebRefM01;
using WsConnection.WebRefMaster;
using SMS.M01.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 納入先保守
    /// </summary>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class NonyusakiHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>Y.Higuchi 2010/08/26</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
        }

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 納入先コードのカラムインデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NONYUSAKI_CD = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分のカラムインデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update>J.Chen 2022/11/21 4→5に変更</update>
        /// --------------------------------------------------
        private const int SHEET_COL_SHUKKA_FLAG = 5;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;

        #endregion


        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックス追加による（出荷元マスタ取得用構造体）
        /// </summary>
        /// <create>TW-Tsuji 2022/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtShipFrom = null;


        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public NonyusakiHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
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
                // 更新メッセージを変更
                // 画面の内容で保存します。\r\nよろしいですか？
                this.MsgUpdateConfirm = "A9999999011";
                // 保存しました。
                this.MsgUpdateEnd = "A9999999045";
                // シートの初期化
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;
                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.NonyusakiHoshu_ManagementNo;
                shtResult.ColumnHeaders[1].Caption = Resources.NonyusakiHoshu_DeliveryDestination;
                shtResult.ColumnHeaders[2].Caption = Resources.NonyusakiHoshu_Ship;
                shtResult.ColumnHeaders[3].Caption = Resources.NounyusakiHoshu_SipFromName;
                shtResult.ColumnHeaders[4].Caption = Resources.NonyusakiHoshu_ManagementDivision;
                shtResult.ColumnHeaders[5].Caption = Resources.NonyusakiHoshu_ShippingDivision;
                shtResult.ColumnHeaders[6].Caption = Resources.NonyusakiHoshu_ExclusionDivision;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboSearchShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboKanriFlag, KANRI_FLAG.GROUPCD);


                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　　DBコネクションで、出荷元マスタ[T_SHIP_FROM]からデータを取得する
                //　　出荷元は「使用」「未使用」の区分があるため、「使用」とされているリストのみ取得する
                var cond = new CondM01(this.UserInfo);
                var conn = new ConnM01();
                cond.UnusedFlag = UNUSED_FLAG.USED_VALUE1;      //定数「使用」の値（"0"）」（デフォルトの値ってのもあるが。。）
                DataSet ds = conn.GetShipFrom(cond);
                //
                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　　出荷元マスタの値を、コンボボックスにセットする
                this._dtShipFrom = ds.Tables[Def_M_SHIP_FROM.Name].Copy();  
                this.MakeCmbBox(this.scboShipFrom, this._dtShipFrom, Def_M_SHIP_FROM.SHIP_FROM_NO, Def_M_SHIP_FROM.SHIP_FROM_NAME, string.Empty, true);
                //----------（修正ここまで）


                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboSearchShukkaFlag.Focus();
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.cboSearchShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.txtSearchNonyusakiName.Text = string.Empty;
                this.txtSearchShip.Text = string.Empty;
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit();
                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode == SystemBase.EditMode.Insert)
                {
                    // 納入先チェック
                    if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                    {
                        // 物件一覧から納入先を選択して下さい。
                        this.ShowMessage("M0100040008");
                        this.btnBukkenIchiran.Focus();
                        return false;
                    }
                    // 出荷区分が本体で「便」が空でないか
                    if (string.IsNullOrEmpty(this.txtShip.Text))
                    {
                        // 便を入力してください。
                        this.ShowMessage("A9999999017");
                        this.txtShip.Focus();
                        return false;
                    }

                    // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                    //　出荷元情報は、必須入力項目のため入力チェックを行う（登録時）
                    if (this.scboShipFrom.SelectedValue == null || this.scboShipFrom.SelectedValue.ToString().Length == 0) 
                    {
                        // 出荷元を入力してください。
                        this.ShowMessage("M0100040011");
                        this.scboShipFrom.Focus();
                        return false;
                    }
                    //----------（修正ここまで）

                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.BukkenNo = this.txtBukkenNo.Text;
                    cond.Ship = this.txtShip.Text;
                    cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                    DataSet ds = conn.GetNonyusaki(cond);
                    if (0 < ComFunc.GetFldToInt32(ds, Def_M_NONYUSAKI.Name, 0, ComDefine.FLD_CNT))
                    {
                        // 既に登録されている納入先です。
                        this.ShowMessage("M0100040009");
                        if (this.cboSearchShukkaFlag.SelectedValue.ToString () == SHUKKA_FLAG.NORMAL_VALUE1)
                        {
                            this.txtShip.Focus();
                        }
                        else
                        {
                            bool isFind = false;
                            foreach (var item in ComDefine.ROLE_KANRISYA)
                            {
                                if (item == this.UserInfo.RoleID)
                                {
                                    this.cboKanriFlag.Focus();
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                this.txtListFlagName0.Focus();
                            }
                        }
                        return false;
                    }
                }
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
                else if (this.EditMode != SystemBase.EditMode.Update)
                {
                    // 特にチェックする項目なし
                }
                else
                {
                    // @@@ ↑
                    // 更新確認メッセージを再設定
                    // 画面の内容で保存します。\r\nよろしいですか？
                    this.MsgUpdateConfirm = "A9999999011";

                    // 登録されているデータかどうかのチェックを行う
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.NonyusakiCD = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    cond.ShukkaFlag = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.SHUKKA_FLAG);
                    cond.BukkenNo = this.txtBukkenNo.Text;
                    cond.Ship = this.txtShip.Text;
                    if (0 < ComFunc.GetFldToInt32(conn.GetNonyusaki(cond), Def_M_NONYUSAKI.Name, 0, ComDefine.FLD_CNT, 0))
                    {
                        // 既に登録されている納入先です。
                        this.ShowMessage("M0100040009");
                        if (this.cboSearchShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                        {
                            this.txtShip.Focus();
                        }
                        else
                        {
                            bool isFind = false;
                            foreach (var item in ComDefine.ROLE_KANRISYA)
                            {
                                if (item == this.UserInfo.RoleID)
                                {
                                    this.cboKanriFlag.Focus();
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                this.txtListFlagName0.Focus();
                            }
                        }
                        return false;
                    }

                    if (ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        // 本体の場合
                        if (string.IsNullOrEmpty(this.txtShip.Text))
                        {
                            // 便を入力して下さい。
                            this.ShowMessage("A9999999017");
                            this.txtShip.Focus();
                            return false;
                        }

                        // 未出荷明細があるかどうかのチェックを行う
                        if (this.cboKanriFlag.SelectedValue.ToString() == KANRI_FLAG.KANRYO_VALUE1)
                        {
                            var ds = conn.GetMishukkaMeisai(cond);
                            if (0 < ComFunc.GetFldToInt32(ds, Def_T_SHUKKA_MEISAI.Name, 0, ComDefine.FLD_CNT, 0))
                            {
                                // 更新確認メッセージ変更
                                // 未出荷明細があります。\r\n強制完了でよろしいですか？
                                this.MsgUpdateConfirm = "M0100040010";
                            }
                        }

                        // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                        //　（本体の場合のみ）出荷元情報は、必須入力項目のため入力チェックを行う（更新時）
                        if (this.scboShipFrom.SelectedValue == null || this.scboShipFrom.SelectedValue.ToString().Length == 0)
                        {
                            // 出荷元を入力してください。
                            this.ShowMessage("M0100040011");
                            this.scboShipFrom.Focus();
                            return false;
                        }
                        //----------（修正ここまで）

                    }
                    else
                    {
                        // ARの場合
                        if (this.cboKanriFlag.SelectedValue.ToString() == KANRI_FLAG.KANRYO_VALUE1)
                        {
                            DataSet ds = conn.GetMikanryoAR(cond);
                            if (0 < ComFunc.GetFldToInt32(ds, Def_T_AR.Name, 0, ComDefine.FLD_CNT, 0))
                            {
                                // 更新確認メッセージを変更
                                // 未完了AR No.があります。\r\n強制完了でよろしいですか？
                                this.MsgUpdateConfirm = "M0100040005";
                            }
                        }
                    }
                    //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
                }
                // @@@ ↑
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
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

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ShukkaFlag = this.cboSearchShukkaFlag.SelectedValue.ToString();
                cond.NonyusakiName = this.txtSearchNonyusakiName.Text;
                cond.Ship = this.txtSearchShip.Text;
                DataSet ds = conn.GetNonyusakiLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当納入先はありません。
                    this.ShowMessage("M0100040002");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = Def_M_NONYUSAKI.Name;
                // モード切り替え
                this.ChangeMode(DisplayMode.EndSearch);
                this.shtResult.Enabled = true;
                // 最も左上に表示されているセルの設定
                if (0 < this.shtResult.MaxRows)
                {
                    this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            try
            {
                bool ret = base.RunEdit();
                if (ret)
                {
                    this.DisplayClearEdit();
                    // とりあえず検索
                    this.RunSearch();
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
        /// <returns></returns>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                // サーバー側で他の条件は設定しているので下記のみで良い
                cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.NonyusakiName = this.txtNonyusakiName.Text;
                cond.Ship = this.txtShip.Text;

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　画面上のコンボボックスで設定されている値を構造体へセットする
                cond.ShipFromCd = this.scboShipFrom.SelectedValue.ToString();

                string errMsgID;
                string[] args;
                if (!conn.InsNonyusakiData(cond, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                // サーバー側で他の条件は設定しているので下記のみで良い

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.UpdNonyusakiData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
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
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                // サーバー側で他の条件は設定しているので下記のみで良い

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                string errMsgID;
                string[] args;
                if (!conn.DelNonyusakiData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit();
                        this.RunSearch();
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

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
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
        /// F4ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/02/18</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                // 待避
                SystemBase.EditMode editMode = this.EditMode;
                // 削除処理
                this.EditMode = SystemBase.EditMode.Delete;
                this.RunEdit();
                // 戻す
                this.EditMode = editMode;
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
        /// <create>H.Tajimi 2015/12/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // 画面をクリアします。\r\nよろしいですか？
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboSearchShukkaFlag.Focus();
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
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
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

        #region 検索ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Update);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Insert);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 出荷区分(検索条件)

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分(検索条件)コンボボックスSelectedIndexChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboSearchShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ChangeShukkaFlag();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 物件一覧

        /// --------------------------------------------------
        /// <summary>
        /// 物件一覧ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2012/04/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnBukkenIchiran_Click(object sender, EventArgs e)
        {
            try
            {
                string shukkaFlag = this.cboSearchShukkaFlag.SelectedValue.ToString();
                string bukkenName = this.txtNonyusakiName.Text;
                using (var frm =new  SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        this.txtNonyusakiName.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NAME);
                        this.txtBukkenNo.Text = ComFunc.GetFld(frm.SelectedRowData, Def_M_BUKKEN.BUKKEN_NO);
                    }
                    this.txtShip.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtResult.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtResult.MaxRows)
            {
                this.shtResult.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtResult.TopLeft.Row);
            }
            this.shtResult.DataSource = null;
            this.shtResult.MaxRows = 0;
            this.shtResult.Enabled = false;
            this.shtResult.Redraw = true;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode(DisplayMode mode)
        {
            try
            {
                switch (mode)
                {
                    case DisplayMode.Initialize:
                        // ----- 初期 -----
                        this.grpSearch.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnInsert.Enabled = this.cboSearchShukkaFlag.SelectedValue == null ? true : this.cboSearchShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
                        this.fbrFunction.F04Button.Enabled = false;
                        // @@@ ↑
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnInsert.Enabled = this.cboSearchShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
                        this.fbrFunction.F04Button.Enabled = false;
                        // @@@ ↑
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F04Button.Enabled = false;
                        this.cboKanriFlag.Enabled = false;
                        this.btnBukkenIchiran.Enabled = true;
                        this.grpARListName.Enabled = this.cboSearchShukkaFlag.SelectedValue.ToString() != SHUKKA_FLAG.NORMAL_VALUE1;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
                        this.fbrFunction.F04Button.Enabled = true;
                        // @@@ ↑
                        this.btnBukkenIchiran.Enabled = false;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の切替
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {
            if (this.cboSearchShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                // 便
                this.txtSearchShip.Enabled = true;

                this.btnInsert.Enabled = true;

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　本体の時は、出荷元コンボボックスを有効
                this.scboShipFrom.Enabled = true;

            }
            else
            {
                // ----- AR -----
                // 便
                this.txtSearchShip.Text = string.Empty;
                this.txtSearchShip.Enabled = false;

                this.btnInsert.Enabled = false;

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　ARの時は、出荷元コンボボックスを無効
                this.scboShipFrom.Enabled = false;

            }
        }

        #endregion

        #endregion

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayClearEdit()
        {
            try
            {
                this.txtShukkaFlagName.Text = string.Empty;
                this.txtNonyusakiCD.Text = string.Empty;
                // 納入先はクリアしない
                //this.txtNonyusakiName.Text = string.Empty;
                //this.txtBukkenNo.Text = string.Empty;
                this.txtShip.Text = string.Empty;
                this.cboKanriFlag.Text = KANRI_FLAG.DEFAULT_VALUE1;
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                this.txtListFlagName0.Text = string.Empty;
                this.txtListFlagName1.Text = string.Empty;
                this.txtListFlagName2.Text = string.Empty;
                this.txtListFlagName3.Text = string.Empty;
                this.txtListFlagName4.Text = string.Empty;
                this.txtListFlagName5.Text = string.Empty;
                this.txtListFlagName6.Text = string.Empty;
                this.txtListFlagName7.Text = string.Empty;
                // @@@ ↑

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                //　出荷元コンボボックスをクリアする
                this.scboShipFrom.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region データ選択

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時のチェック
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    return true;
                }

                if (this.shtResult.ActivePosition.Row < 0)
                {
                    // データが選択されていません。
                    this.ShowMessage("A9999999019");
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

        /// --------------------------------------------------
        /// <summary>
        /// データ選択制御部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelect(DataSelectType selectType)
        {
            Cursor preCursor = Cursor.Current;
            try
            {
                if (this.CheckInputDataSelect(selectType))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool ret = this.RunDataSelectExec(selectType);

                    if (ret)
                    {
                        switch (selectType)
                        {
                            case DataSelectType.Insert:
                                this.EditMode = SystemBase.EditMode.Insert;
                                // モード切替
                                this.ChangeMode(DisplayMode.Insert);
                                this.txtShip.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                if (this.cboSearchShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                                {
                                    this.txtShip.Focus();
                                }
                                else
                                {
                                    bool isFind = false;
                                    foreach (var item in ComDefine.ROLE_KANRISYA)
                                    {
                                        if (item == this.UserInfo.RoleID)
                                        {
                                            this.cboKanriFlag.Focus();
                                            isFind = true;
                                            break;
                                        }
                                    }
                                    if (!isFind)
                                    {
                                        this.txtListFlagName0.Focus();
                                    }
                                }
                                break;
                            case DataSelectType.Delete:
                                break;
                            default:
                                break;
                        }
                    }

                    return ret;
                }
                return false;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                Cursor.Current = preCursor;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択実行部
        /// </summary>
        /// <param name="selectType">データ選択時の状態</param>
        /// <returns>true:成功/false:失敗</returns>
        /// <create>Y.Higuchi 2010/08/26</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    this.txtShukkaFlagName.Text = Resources.NonyusakiHoshu_FlagName;
                    return true;
                }
                if (selectType != DataSelectType.Update) return false;

                // ----- 変更 -----
                ConnMaster conn = new ConnMaster();
                CondNonyusaki cond = new CondNonyusaki(this.UserInfo);
                cond.ShukkaFlag = this.shtResult[SHEET_COL_SHUKKA_FLAG, shtResult.ActivePosition.Row].Text;
                cond.NonyusakiCD = this.shtResult[SHEET_COL_NONYUSAKI_CD, shtResult.ActivePosition.Row].Text;
                DataSet ds = conn.GetNonyusaki(cond);
                if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                {
                    // 他端末で削除されています。
                    this.ShowMessage("A9999999026");
                    // 消えてるのがあったから取り敢えず検索しとけ
                    this.RunSearch();
                    return false;
                }
                this._dtDispData = ds.Tables[Def_M_NONYUSAKI.Name];
                // 表示データ設定
                this.txtShukkaFlagName.Text = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_SHUKKA_FLAG_NAME);
                this.txtNonyusakiCD.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                this.txtBukkenNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.BUKKEN_NO);
                this.txtNonyusakiName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                this.txtShip.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.SHIP);
                this.cboKanriFlag.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.KANRI_FLAG);

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                // （コンボボックスに、DBから取得したデータをセット）
                this.scboShipFrom.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.SHIP_FROM_CD);
                
                bool isAR = true;
                if (ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    isAR = false;
                }

                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                if (isAR)
                {
                    this.txtListFlagName0.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
                    this.txtListFlagName1.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
                    this.txtListFlagName2.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
                    this.txtListFlagName3.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
                    this.txtListFlagName4.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
                    this.txtListFlagName5.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
                    this.txtListFlagName6.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
                    this.txtListFlagName7.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
                }
                this.grpARListName.Enabled = isAR;
                // @@@ ↑
                this.txtShip.Enabled = !isAR;
                this.cboKanriFlag.Enabled = false;
                foreach (var item in ComDefine.ROLE_KANRISYA)
                {
                    if (this.UserInfo.RoleID == item)
                    {
                        this.cboKanriFlag.Enabled = true;
                        break;
                    }
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

        #region 登録データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <param name="dt">納入先マスタデータテーブル</param>
        /// <create>Y.Higuchi 2010/08/27</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEditData(DataTable dt)
        {
            try
            {
                // 更新しか無いので必ずデータがある。
                DataRow dr = dt.Rows[0];
                // 更新しか無いので変更できるデータのみ設定
                dr[Def_M_NONYUSAKI.NONYUSAKI_NAME] = this.txtNonyusakiName.Text;
                dr[Def_M_NONYUSAKI.SHIP] = this.txtShip.Text;
                dr[Def_M_NONYUSAKI.KANRI_FLAG] = this.cboKanriFlag.SelectedValue.ToString();
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                string shukkaFlag = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.SHUKKA_FLAG);
                if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME0] = this.txtListFlagName0.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME1] = this.txtListFlagName1.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME2] = this.txtListFlagName2.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME3] = this.txtListFlagName3.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME4] = this.txtListFlagName4.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME5] = this.txtListFlagName5.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME6] = this.txtListFlagName6.Text;
                    dr[Def_M_NONYUSAKI.LIST_FLAG_NAME7] = this.txtListFlagName7.Text;
                }
                // @@@ ↑

                // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                // 　DB更新用のデータセットに、コンボボックスで選択された値をセット）
                //
                if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                    // ARの場合はnull（更新しない）
                    dr[Def_M_NONYUSAKI.SHIP_FROM_CD] = null;
                else
                    // 本体の場合はコンボボックスの選択値をセット
                    dr[Def_M_NONYUSAKI.SHIP_FROM_CD] = this.scboShipFrom.SelectedValue.ToString();


            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion
    }
}
