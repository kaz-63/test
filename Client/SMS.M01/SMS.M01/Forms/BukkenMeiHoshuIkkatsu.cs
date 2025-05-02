using System;
using System.Data;
using System.Windows.Forms;

using Commons;
using DSWUtil;
using GrapeCity.Win.ElTabelle;
using SMS.M01.Properties;
using SystemBase.Util;
using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefM01;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 物件名一括保守
    /// </summary>
    /// <create>H.Tsuji 2018/12/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BukkenMeiHoshuIkkatsu : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// 編集
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Edit,
        }

        /// --------------------------------------------------
        /// <summary>
        /// データ選択時の状態
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DataSelectType
        {
            /// --------------------------------------------------
            /// <summary>
            /// 登録
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Insert,
            /// --------------------------------------------------
            /// <summary>
            /// 変更
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Update,
            /// --------------------------------------------------
            /// <summary>
            /// 削除
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Delete,
            /// --------------------------------------------------
            /// <summary>
            /// メール設定
            /// </summary>
            /// <create>H.Tsuji 2018/12/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Mail,
        }

        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;
        /// --------------------------------------------------
        /// <summary>
        /// 物件名のカラムインデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update>TW-Tsuji 2022/10/12</update>
        /// --------------------------------------------------
        private const int SHEET_COL_BUKKEN_NAME = 1;        //列変更による（0->1)
        /// --------------------------------------------------
        /// <summary>
        /// ProjectNoのカラムインデックス
        /// </summary>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update>TW-Tsuji 2022/10/12</update>
        /// --------------------------------------------------
        private const int SHEET_COL_PROJECT_NO = 5;         //列変更による（4->5)
        /// --------------------------------------------------
        /// <summary>
        /// 受注Noのカラムインデックス
        /// </summary>
        /// <create>J.Chen 2022/09/22</create>
        /// <update>TW-Tsuji 2022/10/12</update>
        /// --------------------------------------------------
        private const int SHEET_COL_JUCHU_NO = 0;           //列変更による（5->0)
        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 表示時に使用したデータ
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtDispData = null;

        // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
        /// --------------------------------------------------
        /// <summary>
        /// メール送信判定
        /// </summary>
        /// <create>M.Shimizu 2020/07/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isMailSend = false;
        // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

        /// --------------------------------------------------
        /// <summary>
        /// メールアドレス
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _mailAddress = string.Empty;

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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public BukkenMeiHoshuIkkatsu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsuji 2018/12/13</create>
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

                // シートの初期化
                this.InitializeSheet(this.shtResult);
                this.shtResult.KeepHighlighted = true;

                //【Step_1_11】納入先の物件名登録　重複登録防止　2022/10/12（TW-Tsuji）
                //　シートに「受注No」（JUCHU_NO）を追加する
                //
                //---元のコードを削除
                //
                // // シートのタイトルを設定
                // shtResult.ColumnHeaders[0].Caption = Resources.BukkenMeiHoshuIkkatsu_BukkenName;
                // shtResult.ColumnHeaders[1].Caption = Resources.BukkenMeiHoshuIkkatsu_NormalTagNo;
                // shtResult.ColumnHeaders[2].Caption = Resources.BukkenMeiHoshuIkkatsu_ArTagNo;
                // shtResult.ColumnHeaders[3].Caption = Resources.BukkenMeiHoshuIkkatsu_MailNotify;
                //
                //---新たなコードを追加（表示は最左列に追加するため）
                //
                // シートのタイトルを設定
                shtResult.ColumnHeaders[0].Caption = Resources.BukkenmeiHosyuIkkatsu_JuchuNo;       //受注No
                shtResult.ColumnHeaders[1].Caption = Resources.BukkenMeiHoshuIkkatsu_BukkenName;    
                shtResult.ColumnHeaders[2].Caption = Resources.BukkenMeiHoshuIkkatsu_NormalTagNo;
                shtResult.ColumnHeaders[3].Caption = Resources.BukkenMeiHoshuIkkatsu_ArTagNo;
                shtResult.ColumnHeaders[4].Caption = Resources.BukkenMeiHoshuIkkatsu_MailNotify;
                //
                //---修正ここまで

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboMailNotify, MAIL_NOTIFY.GROUPCD);

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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.txtSearchBukkenName.Focus();
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.txtSearchBukkenName.Text = string.Empty;
                // グリッド
                this.SheetClear();
                // 登録情報部分のクリア
                this.DisplayClearEdit(DisplayMode.Initialize);
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// <update>J.Chen 2022/09/21 受注Noチェック追加</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                if (this.EditMode == SystemBase.EditMode.Delete)
                {
                    // 特にチェックする項目なし
                }
                else
                {
                    // 受注Noチェック
                    if (string.IsNullOrEmpty(this.txtJuchuNo.Text))
                    {
                        // 受注Noを入力して下さい。
                        this.ShowMessage("M0100120009");
                        this.txtJuchuNo.Focus();
                        return false;
                    }

                    // 物件名チェック
                    if (string.IsNullOrEmpty(this.txtBukkenName.Text))
                    {
                        // 物件名を入力して下さい。
                        this.ShowMessage("M0100120001");
                        this.txtBukkenName.Focus();
                        return false;
                    }

                    // 登録処理のみメール登録チェックを行う
                    if (this.EditMode == SystemBase.EditMode.Insert)
                    {
                        var conn = new ConnCommon();
                        var cond = new CondCommon(this.UserInfo);
                        var ds = conn.CheckMail(cond);

                        if (UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.BUKKEN_ADD_EVENT) == BUKKEN_ADD_EVENT.YES_VALUE1)
                        {
                            if (UtilData.GetFldToInt32(ds, Def_M_BUKKEN_MAIL.Name, 0, ComDefine.FLD_CNT) == 0)
                            {
                                // 基本通知設定で送信先が登録されていません。
                                this.ShowMessage("A9999999057");
                                return false;
                            }
                            if (string.IsNullOrEmpty(UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS)))
                            {
                                // 担当者にMailAddressが設定されていません。
                                this.ShowMessage("A0100010010");
                                return false;
                            }
                            foreach (DataRow dr in ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name].Rows)
                            {
                                if (string.IsNullOrEmpty(UtilData.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
                                {
                                    // MailAddress未設定のUserがいます。【ID：{0}、Name：{1}】
                                    this.ShowMessage("A9999999060", UtilData.GetFld(dr, Def_M_USER.USER_ID), UtilData.GetFld(dr, Def_M_USER.USER_NAME));
                                    return false;
                                }
                            }

                            // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
                            // メール送信判定は物件追加イベントで判定する
                            // this._templateFolder = UtilData.GetFld(ds, Def_M_MAIL_SETTING.Name, 0, Def_M_MAIL_SETTING.TEMPLATE_FOLDER);
                            this._isMailSend = true;
                            // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

                            this._mailAddress = UtilData.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
                        }
                    }
                }
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
        /// <create>H.Tsuji 2018/12/13</create>
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
        /// <create>H.Tsuji 2018/12/13</create>
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
        /// <create>H.Tsuji 2018/12/13</create>
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
                cond.BukkenName = this.txtSearchBukkenName.Text;
                DataSet ds = conn.GetBukkenIkkatsuLikeSearch(cond);

                if (!ComFunc.IsExistsData(ds, ComDefine.DTTBL_DISP_BUKKEN))
                {
                    // シートのクリア
                    this.SheetClear();
                    // 該当物件名はありません。
                    this.ShowMessage("M0100120002");
                    return false;
                }

                this.shtResult.DataSource = ds;
                this.shtResult.DataMember = ComDefine.DTTBL_DISP_BUKKEN;
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    this.DisplayClearEdit(DisplayMode.Edit);
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
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditInsert()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.BukkenName = ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME);
                cond.JuchuNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.JUCHU_NO);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                // Projectマスタデータ追加
                DataTable dtProject = this.CreateProjectData(this._dtDispData);
                ds.Tables.Add(dtProject);

                // 物件マスタデータ追加
                DataTable dtBukken = this.CreateBukkenData(this._dtDispData);
                ds.Tables.Add(dtBukken);

                // メールデータ追加
                DataTable dtMail = this.CreateMailData();
                if (dtMail != null)
                {
                    ds.Tables.Add(dtMail);
                }

                string errMsgID;
                string[] args;
                if (!conn.InsBukkenIkkatsuData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.DisplayClearEdit(DisplayMode.Insert);
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100120003")
                    {
                        this.RunSearch();
                        this.ChangeMode(DisplayMode.Insert);
                        this.txtBukkenName.Focus();
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                // 更新元の物件名
                cond.BukkenName = this.shtResult[SHEET_COL_BUKKEN_NAME, shtResult.ActivePosition.Row].Text;
                cond.JuchuNo = this.shtResult[SHEET_COL_JUCHU_NO, shtResult.ActivePosition.Row].Text;

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                // Projectマスタデータ追加
                DataTable dtProject = this.CreateProjectData(this._dtDispData);
                ds.Tables.Add(dtProject);

                // 物件マスタデータ追加
                DataTable dtBukken = this.CreateBukkenData(this._dtDispData);
                ds.Tables.Add(dtBukken);

                string errMsgID;
                string[] args;
                if (!conn.UpdBukkenIkkatsuData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100120004")
                    {
                        this.DisplayClearEdit(DisplayMode.Update);
                        this.RunSearch();
                    }
                    else if (errMsgID == "M0100120003")
                    {
                        // シートを更新後、重複項目を表示する
                        this.RunSearch();
                        var dt = (this.shtResult.DataSource as DataSet).Tables[ComDefine.DTTBL_DISP_BUKKEN];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (cond.BukkenName == ComFunc.GetFld(dt, i, Def_M_BUKKEN.BUKKEN_NAME))
                            {
                                this.shtResult.TopLeft = new Position(0, i);
                                this.shtResult.ActivePosition = new Position(0, i);
                            }
                        }

                        this.ChangeMode(DisplayMode.Update);
                        this.txtBukkenName.Focus();
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditDelete()
        {
            try
            {
                this.SetEditData(this._dtDispData);

                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo);
                cond.ProjectNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.PROJECT_NO);
                cond.BukkenName = ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME);
                cond.JuchuNo = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.JUCHU_NO);

                DataSet ds = new DataSet();
                ds.Tables.Add(this._dtDispData.Copy());

                // 物件マスタデータ追加
                DataTable dtBukken = this.CreateBukkenData(this._dtDispData);
                ds.Tables.Add(dtBukken);

                string errMsgID;
                string[] args;
                if (!conn.DelBukkenIkkatsuData(cond, ds, out errMsgID, out args))
                {
                    if (ComFunc.IsVersionError(errMsgID)
                        || errMsgID == "M0100120004")
                    {
                        this.DisplayClearEdit(DisplayMode.Delete);
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
        /// <create>H.Tsuji 2018/12/13</create>
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
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
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
                this.txtSearchBukkenName.Focus();
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
        /// <create>H.Tsuji 2018/12/13</create>
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
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>H.Tsuji 2018/12/13</create>
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

        #region 登録ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
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

        #region 変更ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 変更ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
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

        #region 削除ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.RunDataSelect(DataSelectType.Delete);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 共通メールボタン

        /// --------------------------------------------------
        /// <summary>
        /// 共通メールボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnCommonNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new CommonNotify(this.UserInfo, ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        #endregion

        #region ARList名単位メール設定ボタン

        /// --------------------------------------------------
        /// <summary>
        /// ARList名単位メール設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnARListNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_BUKKEN_NO);
            if (!conn.ExistsBukkenMail(cond))
            {
                this.ShowMessage("M0100060006");
                return;
            }
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new ARListNotify(this.UserInfo, ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 進捗管理Mail設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Nakasato 2019/07/04</create>
        /// <update>D.Okumura 2019/08/07 メッセージを変更(M0100120008→A9999999057)</update>
        /// --------------------------------------------------
        private void btnShinchokuKanriNotify_Click(object sender, EventArgs e)
        {
            if (!this.RunDataSelect(DataSelectType.Mail))
            {
                return;
            }

            var conn = new ConnM01();
            var cond = new CondM01(this.UserInfo);
            cond.BukkenNo = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_BUKKEN_NO);
            if (!conn.ExistsShinchokuKanriMail(cond))
            {
                // 基本通知設定で送信先が登録されていません。
                this.ShowMessage("A9999999057");
                return;
            }
            if (!conn.ExistsMailChangeRole(cond))
            {
                // MailAddress変更権限を持っていないため設定できません。
                this.ShowMessage("A9999999059");
                return;
            }

            using (var f = new ShinchokuNotify(this.UserInfo, ComDefine.TITLE_M0100180, ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_BUKKEN_NO), ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME)))
            {
                f.ShowDialog();
            }
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>H.Tsuji 2018/12/13</create>
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
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update>J.Chen 2022/09/21 受注No追加</update>
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
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = false;
                        this.btnDelete.Enabled = false;
                        this.btnCommonNotify.Enabled = false;
                        this.btnARListNotify.Enabled = false;
                        this.btnShinchokuKanriNotify.Enabled = false;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = true;
                        this.btnInsert.Enabled = true;
                        this.btnUpdate.Enabled = true;
                        this.btnDelete.Enabled = true;
                        this.btnCommonNotify.Enabled = true;
                        this.btnARListNotify.Enabled = true;
                        this.btnShinchokuKanriNotify.Enabled = true;
                        this.grpEdit.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        break;
                    case DisplayMode.Insert:
                        // ----- 登録 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtJuchuNo.Enabled = true;
                        this.txtBukkenName.Enabled = true;
                        this.cboMailNotify.Enabled = true;
                        this.txtNormalTagNo.Enabled = true;
                        this.txtArTagNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Update:
                        // ----- 変更 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtJuchuNo.Enabled = true;
                        this.txtBukkenName.Enabled = true;
                        this.cboMailNotify.Enabled = true;
                        this.txtNormalTagNo.Enabled = true;
                        this.txtArTagNo.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        break;
                    case DisplayMode.Delete:
                        // ----- 削除 -----
                        this.grpSearch.Enabled = false;
                        this.grpEdit.Enabled = true;
                        this.txtJuchuNo.Enabled = false;
                        this.txtBukkenName.Enabled = false;
                        this.cboMailNotify.Enabled = false;
                        this.txtNormalTagNo.Enabled = false;
                        this.txtArTagNo.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = true;
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

        #endregion

        #region 登録情報クリア

        /// --------------------------------------------------
        /// <summary>
        /// 登録情報クリア
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update>J.Chen 2022/09/21 受注No追加</update>
        /// --------------------------------------------------
        private void DisplayClearEdit(DisplayMode mode)
        {
            try
            {
                this.cboMailNotify.SelectedValue = MAIL_NOTIFY.DEFAULT_VALUE1;
                this.txtJuchuNo.Text = string.Empty;
                this.txtBukkenName.Text = string.Empty;
                this.txtNormalTagNo.Text = string.Empty;
                this.txtArTagNo.Text = string.Empty;
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
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputDataSelect(DataSelectType selectType)
        {
            try
            {
                if (this.shtResult.ActivePosition.Row < 0 && selectType != DataSelectType.Insert)
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
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update>J.Chen 2022/09/21 受注No追加</update>
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
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Insert);
                                //this.txtBukkenName.Focus();
                                this.txtJuchuNo.Focus();
                                break;
                            case DataSelectType.Update:
                                this.EditMode = SystemBase.EditMode.Update;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Update);
                                //this.txtBukkenName.Focus();
                                this.txtJuchuNo.Focus();
                                break;
                            case DataSelectType.Delete:
                                this.EditMode = SystemBase.EditMode.Delete;
                                // モード切り替え
                                this.ChangeMode(DisplayMode.Delete);
                                this.fbrFunction.F01Button.Focus();
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
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunDataSelectExec(DataSelectType selectType)
        {
            try
            {
                if (selectType == DataSelectType.Insert)
                {
                    // ----- 登録 -----
                    this._dtDispData = this.GetSchemeDisp();
                    // 1行追加しておく
                    this._dtDispData.Rows.Add(this._dtDispData.NewRow());
                }
                else
                {
                    // ----- 変更、削除 -----
                    ConnM01 conn = new ConnM01();
                    CondM01 cond = new CondM01(this.UserInfo);
                    cond.ProjectNo = this.shtResult[SHEET_COL_PROJECT_NO, shtResult.ActivePosition.Row].Text;
                    cond.JuchuNo = this.shtResult[SHEET_COL_JUCHU_NO, shtResult.ActivePosition.Row].Text;

                    DataSet ds = conn.GetBukkenIkkatsuSearch(cond);
                    if (!ComFunc.IsExistsData(ds, ComDefine.DTTBL_DISP_BUKKEN))
                    {
                        // 既に削除された物件名です。
                        this.ShowMessage("M0100120004");
                        // 消えてるのがあったから取り敢えず検索
                        this.RunSearch();
                        return false;
                    }
                    this._dtDispData = ds.Tables[ComDefine.DTTBL_DISP_BUKKEN];
                }

                // メール設定以外はコントロールに値をセットする
                if (selectType != DataSelectType.Mail)
                {
                    this.txtJuchuNo.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_BUKKEN.JUCHU_NO);
                    this.txtBukkenName.Text = ComFunc.GetFld(this._dtDispData, 0, Def_M_PROJECT.BUKKEN_NAME);
                    this.txtNormalTagNo.Text = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_NORMAL_ISSUED_TAG_NO);
                    this.txtArTagNo.Text = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_ISSUED_TAG_NO);
                    if (!string.IsNullOrEmpty(ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_MAIL_NOTIFY)))
                    {
                        this.cboMailNotify.SelectedValue = ComFunc.GetFld(this._dtDispData, 0, ComDefine.FLD_AR_MAIL_NOTIFY);
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

        #region データテーブル作成

        #region 表示データのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 表示データのデータテーブル作成
        /// </summary>
        /// <returns>表示データのデータテーブル</returns>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update>K.Tsutsumi 2020/05/31 タイムゾーンに影響されるコードの修正</update>
        /// <update>J.Chen 2022/09/22 受注No.追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeDisp()
        {
            try
            {
                DataTable dt = new DataTable(ComDefine.DTTBL_DISP_BUKKEN);

                // Projectマスタ
                dt.Columns.Add(Def_M_PROJECT.PROJECT_NO, typeof(string));
                dt.Columns.Add(Def_M_PROJECT.BUKKEN_NAME, typeof(string));
                dt.Columns.Add(Def_M_PROJECT.VERSION, typeof(DateTime));
                // 受注No.
                dt.Columns.Add(Def_M_BUKKEN.JUCHU_NO, typeof(string));
                // 本体物件
                dt.Columns.Add(ComDefine.FLD_NORMAL_BUKKEN_NO, typeof(decimal));
                dt.Columns.Add(ComDefine.FLD_NORMAL_ISSUED_TAG_NO, typeof(decimal));
                dt.Columns.Add(ComDefine.FLD_NORMAL_MAINTE_VERSION, typeof(DateTime));
                dt.Columns.Add(ComDefine.FLD_NORMAL_MAIL_NOTIFY, typeof(string));
                // AR物件
                dt.Columns.Add(ComDefine.FLD_AR_BUKKEN_NO, typeof(decimal));
                dt.Columns.Add(ComDefine.FLD_AR_ISSUED_TAG_NO, typeof(decimal));
                dt.Columns.Add(ComDefine.FLD_AR_MAINTE_VERSION, typeof(DateTime));
                dt.Columns.Add(ComDefine.FLD_AR_MAIL_NOTIFY, typeof(string));
                dt.Columns.Add(ComDefine.FLD_AR_MAIL_NOTIFY_NAME, typeof(string));

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 物件マスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// 物件マスタのデータテーブル作成(本体・AR物件)
        /// </summary>
        /// <returns>物件マスタのデータテーブル</returns>
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update>K.Tsutsumi 2020/05/31 タイムゾーンに影響されるコードの修正</update>
        /// <update>J.Chen 2022/09/22 受注No.追加</update>
        /// --------------------------------------------------
        private DataTable GetSchemeBukken()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_BUKKEN.Name);
                dt.Columns.Add(Def_M_BUKKEN.SHUKKA_FLAG, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NO, typeof(decimal));
                dt.Columns.Add(Def_M_BUKKEN.BUKKEN_NAME, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.MAINTE_VERSION, typeof(DateTime));
                dt.Columns.Add(Def_M_BUKKEN.MAIL_NOTIFY, typeof(object));
                dt.Columns.Add(Def_M_BUKKEN.PROJECT_NO, typeof(string));
                dt.Columns.Add(Def_M_BUKKEN.JUCHU_NO, typeof(string));
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Projectマスタのデータテーブル作成

        /// --------------------------------------------------
        /// <summary>
        /// Projectマスタのデータテーブル作成
        /// </summary>
        /// <returns>Projectマスタのデータテーブル</returns>
        /// <create>H.Tsuji 2018/12/17</create>
        /// <update>K.Tsutsumi 2020/05/31 タイムゾーンに影響されるコードの修正</update>
        /// --------------------------------------------------
        private DataTable GetSchemeProject()
        {
            try
            {
                DataTable dt = new DataTable(Def_M_PROJECT.Name);
                dt.Columns.Add(Def_M_PROJECT.PROJECT_NO, typeof(string));
                dt.Columns.Add(Def_M_PROJECT.BUKKEN_NAME, typeof(string));
                dt.Columns.Add(Def_M_PROJECT.VERSION, typeof(DateTime));
                dt.Columns.Add(Def_M_PROJECT.JUCHU_NO, typeof(string));
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 登録データの取得

        /// --------------------------------------------------
        /// <summary>
        /// 登録データの取得
        /// </summary>
        /// <param name="dt">物件マスタデータテーブル</param>
        /// <create>H.Tsuji 2018/12/13</create>
        /// <update>J.Chen 2022/09/22 DTに受注No.を追加</update>
        /// --------------------------------------------------
        private void SetEditData(DataTable dt)
        {
            try
            {
                DataRow dr;
                if (0 < dt.Rows.Count)
                {
                    dr = dt.Rows[0];
                }
                else
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                dr[Def_M_PROJECT.BUKKEN_NAME] = this.txtBukkenName.Text;
                dr[ComDefine.FLD_NORMAL_MAIL_NOTIFY] = MAIL_NOTIFY.STOP_VALUE1;
                dr[ComDefine.FLD_AR_MAIL_NOTIFY] = this.cboMailNotify.SelectedValue;
                dr[Def_M_BUKKEN.JUCHU_NO] = this.txtJuchuNo.Text;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region メール関連

        /// --------------------------------------------------
        /// <summary>
        /// メール登録用データ作成
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update>M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化</update>
        /// <update>M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）</update>
        /// --------------------------------------------------
        public DataTable CreateMailData()
        {
            // ↓↓↓ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）
            // 物件追加イベントが'1'でない場合、メール送信データは作成しない
            // if (string.IsNullOrEmpty(this._templateFolder))
            if (!this._isMailSend)
            {
                return null;
            }
            // ↑↑↑ M.Shimizu 2020/07/02 EFA_SMS-91 メール通知のWebService化（メール設定マスタのメールテンプレートフォルダは未使用にする）

            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            // メールテンプレートの内容を取得
            ConnAttachFile attachFile = new ConnAttachFile(this.UserInfo.Language);
            string title = attachFile.GetMailTemplate(MAIL_FILE.NEW_TITLE_VALUE1);
            string naiyo = attachFile.GetMailTemplate(MAIL_FILE.NEW_VALUE1);
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化

            var conn = new ConnCommon();
            var cond = new CondCommon(this.UserInfo);
            cond.MailKbn = MAIL_KBN.BUKKEN_VALUE1;
            var ret = conn.GetMailData(cond);

            var dt = ComFunc.GetSchemeMail();
            var dr = dt.NewRow();
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND, this._mailAddress);
            dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, this.UserInfo.UserName);
            dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.MAIL_ADDRESS));
            dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, ComFunc.GetMailUser(ret, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME));
            // ↓↓↓ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            dr.SetField<object>(Def_T_MAIL.TITLE, this.MailReplace(title));
            dr.SetField<object>(Def_T_MAIL.NAIYO, this.MailReplace(naiyo));
            // ↑↑↑ M.Shimizu 2020/06/01 EFA_SMS-91 メール通知のWebService化
            dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
            dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
            dt.Rows.Add(dr);

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// テンプレートのデータを置換
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/14</create>
        /// <update></update>
        /// --------------------------------------------------
        private string MailReplace(string source)
        {
            // 物件管理NoはDBで採番した後に置換
            return source
                .Replace(MAIL_RESERVE.BUKKEN_NAME_VALUE1, this.txtBukkenName.Text)
                //.Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, this.txtBukkenNo.Text)
                .Replace(MAIL_RESERVE.BUKKEN_CREATE_USER_VALUE1, this.UserInfo.UserName);
        }

        #endregion

        #region 登録データ作成

        /// --------------------------------------------------
        /// <summary>
        /// Projectマスタデータを作成する
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable CreateProjectData(DataTable dtDisp)
        {
            DataTable dt = this.GetSchemeProject();

            DataRow dr = dt.NewRow();
            dr[Def_M_PROJECT.PROJECT_NO] = dtDisp.Rows[0][Def_M_PROJECT.PROJECT_NO];
            dr[Def_M_PROJECT.BUKKEN_NAME] = dtDisp.Rows[0][Def_M_PROJECT.BUKKEN_NAME];
            dr[Def_M_PROJECT.VERSION] = dtDisp.Rows[0][Def_M_PROJECT.VERSION];
            dr[Def_M_PROJECT.JUCHU_NO] = dtDisp.Rows[0][Def_M_PROJECT.JUCHU_NO];
            dt.Rows.Add(dr);

            return dt;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 物件マスタデータを作成する
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <create>H.Tsuji 2018/12/18</create>
        /// <update>J.Chen 2022/09/22 受注No.追加</update>
        /// --------------------------------------------------
        private DataTable CreateBukkenData(DataTable dtDisp)
        {
            DataTable dt = this.GetSchemeBukken();

            DataRow drNormal = dt.NewRow();
            drNormal[Def_M_BUKKEN.SHUKKA_FLAG] = SHUKKA_FLAG.NORMAL_VALUE1;
            drNormal[Def_M_BUKKEN.BUKKEN_NO] = dtDisp.Rows[0][ComDefine.FLD_NORMAL_BUKKEN_NO];
            drNormal[Def_M_BUKKEN.BUKKEN_NAME] = dtDisp.Rows[0][Def_M_PROJECT.BUKKEN_NAME];
            drNormal[Def_M_BUKKEN.MAINTE_VERSION] = dtDisp.Rows[0][ComDefine.FLD_NORMAL_MAINTE_VERSION];
            drNormal[Def_M_BUKKEN.MAIL_NOTIFY] = dtDisp.Rows[0][ComDefine.FLD_NORMAL_MAIL_NOTIFY];
            drNormal[Def_M_BUKKEN.PROJECT_NO] = dtDisp.Rows[0][Def_M_PROJECT.PROJECT_NO];
            drNormal[Def_M_BUKKEN.JUCHU_NO] = dtDisp.Rows[0][Def_M_BUKKEN.JUCHU_NO];
            dt.Rows.Add(drNormal);

            DataRow drAR = dt.NewRow();
            drAR[Def_M_BUKKEN.SHUKKA_FLAG] = SHUKKA_FLAG.AR_VALUE1;
            drAR[Def_M_BUKKEN.BUKKEN_NO] = dtDisp.Rows[0][ComDefine.FLD_AR_BUKKEN_NO];
            drAR[Def_M_BUKKEN.BUKKEN_NAME] = dtDisp.Rows[0][Def_M_PROJECT.BUKKEN_NAME];
            drAR[Def_M_BUKKEN.MAINTE_VERSION] = dtDisp.Rows[0][ComDefine.FLD_AR_MAINTE_VERSION];
            drAR[Def_M_BUKKEN.MAIL_NOTIFY] = dtDisp.Rows[0][ComDefine.FLD_AR_MAIL_NOTIFY];
            drAR[Def_M_BUKKEN.PROJECT_NO] = dtDisp.Rows[0][Def_M_PROJECT.PROJECT_NO];
            drAR[Def_M_BUKKEN.JUCHU_NO] = dtDisp.Rows[0][Def_M_BUKKEN.JUCHU_NO];
            dt.Rows.Add(drAR);

            return dt;
        }

        #endregion

    }
}
