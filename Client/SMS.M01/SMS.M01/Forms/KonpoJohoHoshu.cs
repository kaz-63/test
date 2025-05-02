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

using WsConnection.WebRefM01;
using Commons.Properties;

namespace SMS.M01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 梱包情報保守
    /// </summary>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class KonpoJohoHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 状態ボタン
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_STATE_BUTTON = 0;
        /// --------------------------------------------------
        /// <summary>
        /// TagNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TAG_NO = 1;
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_BOX_NO = 2;
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_SEIBAN = 3;
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_CODE = 4;
        /// --------------------------------------------------
        /// <summary>
        /// 図面追番
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_OIBAN = 5;
        /// --------------------------------------------------
        /// <summary>
        /// Area
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_AREA = 6;
        /// --------------------------------------------------
        /// <summary>
        /// Floor
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FLOOR = 7;
        /// --------------------------------------------------
        /// <summary>
        /// 機種
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KISHU = 8;
        /// --------------------------------------------------
        /// <summary>
        /// MNO
        /// </summary>
        /// <create>H.Tajimi 2015/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_M_NO = 9;
        /// --------------------------------------------------
        /// <summary>
        /// 品名(和文)
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI_JP = 10;
        /// --------------------------------------------------
        /// <summary>
        /// 品名
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_HINMEI = 11;
        /// --------------------------------------------------
        /// <summary>
        /// 図面/形式
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ZUMEN_KEISHIKI = 12;
        /// --------------------------------------------------
        /// <summary>
        /// 区割NO
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_KUWARI_NO = 13;
        /// --------------------------------------------------
        /// <summary>
        /// 数量
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_NUM = 14;
        /// --------------------------------------------------
        /// <summary>
        /// 備考
        /// </summary>
        /// <create>H.Tajimi 2015/11/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_BIKO = 15;
        /// --------------------------------------------------
        /// <summary>
        /// STNO
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_ST_NO = 16;

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 4;

        /// --------------------------------------------------
        /// <summary>
        /// TagNo桁数
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int LENGTH_TAG_NO = 5;

        /// --------------------------------------------------
        /// <summary>
        /// BoxNo桁数
        /// </summary>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int LENGTH_BOX_NO = 6;

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// ボックス／パレットリスト管理データ退避用
        /// </summary>
        /// <create>K.Tsutsumi 2012/05/18</create>
        /// <update></update>
        /// --------------------------------------------------
        DataTable _dtManage = null;

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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public KonpoJohoHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック 
                this._dtManage = null;
                // ↑

                // フォームの設定
                this.IsCloseQuestion = true;
                // 更新処理固定
                this.EditMode = SystemBase.EditMode.Update;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // コンボボックスの初期化
                this.MakeCmbBox(this.cboKonpoTani, KONPO_TANI.GROUPCD);
                // モード切替
                this.ChangeMode();

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[1].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_TagNo;
                shtMeisai.ColumnHeaders[2].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_BoxNo;
                shtMeisai.ColumnHeaders[3].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_ProductNo;
                shtMeisai.ColumnHeaders[4].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Code;
                shtMeisai.ColumnHeaders[5].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[6].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Area;
                shtMeisai.ColumnHeaders[7].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Floor;
                shtMeisai.ColumnHeaders[8].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Model;
                shtMeisai.ColumnHeaders[9].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_MNo;
                shtMeisai.ColumnHeaders[10].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_JpName;
                shtMeisai.ColumnHeaders[11].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Name;
                shtMeisai.ColumnHeaders[12].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_DrawingNoFormat;
                shtMeisai.ColumnHeaders[13].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_SectioningNo;
                shtMeisai.ColumnHeaders[14].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Quantity;
                shtMeisai.ColumnHeaders[15].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_Memo;
                shtMeisai.ColumnHeaders[16].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_STNo;
                shtMeisai.ColumnHeaders[17].Caption = SMS.M01.Properties.Resources.KonpoJohoHoshu_ARNo;
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboKonpoTani.Focus();
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();
                // 梱包単位
                if (0 < this.cboKonpoTani.Items.Count)
                {
                    this.cboKonpoTani.SelectedValue = KONPO_TANI.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboKonpoTani.SelectedIndex = -1;
                }
                // 梱包No.
                this.txtKonpoNo.Text = string.Empty;
                // 表示クリック前の状態にする。
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // 編集内容実行時の入力チェック
                DataTable dt = this.shtMeisai.DataSource as DataTable;
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                bool isExistKaijyo = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (ComFunc.GetFld(dr, ComDefine.FLD_BTN_STATE) == ComDefine.BUTTON_TEXT_KAIJYO)
                    {
                        isExistKaijyo = true;
                        // @@@ 2011/03/07 M.Tsutsumi Delete No.44
                        //break;
                        // @@@ ↑
                    }
                    //@@@ 2011/03/07 M.Tsutsumi Add No.44
                    else if (ComFunc.GetFld(dr, ComDefine.FLD_BTN_STATE) == ComDefine.BUTTON_TEXT_TOUROKU)
                    {
                        // 追加登録のだけでもOK
                        isExistKaijyo = true;
                        if (konpoTani == KONPO_TANI.BOX_VALUE1)
                        {
                            // ----- Box梱包 -----
                            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)))
                            {
                                // TagNo.が入力されていません。
                                this.ShowMessage("M0100050021");
                                this.shtMeisai.Focus();
                                ret = false;
                                break;
                            }
                        }
                        else
                        {
                            // ----- パレット梱包 -----
                            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)))
                            {
                                // BoxNo.が入力されていません。
                                this.ShowMessage("M0100050022");
                                this.shtMeisai.Focus();
                                ret = false;
                                break;
                            }
                        }
                    }
                    // @@@ ↑
                }
                if (!isExistKaijyo)
                {
                    // 解除するデータがありません。
                    this.ShowMessage("M0100050008");
                    this.shtMeisai.Focus();
                    ret = false;
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 検索用入力チェック
                if (string.IsNullOrEmpty(this.txtKonpoNo.Text))
                {
                    // 梱包No.を入力して下さい。
                    this.ShowMessage("M0100050001");
                    this.txtKonpoNo.Focus();
                    ret = false;
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
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>D.Okumura 2018/10/10 多言語化対応</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                string tableName = string.Empty;
                // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
                string tableName_Manage = string.Empty;
                // ↑
                string warningMsgID = string.Empty;
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                string errMsgID = null;
                string[] args = null;
                DataSet ds;
                CondM01 cond = new CondM01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPO, ComDefine.BUTTON_TEXT_TOUROKU);
                ConnM01 conn = new ConnM01();
                cond.KonpoNo = this.lblKonpo.Text + this.txtKonpoNo.Text;
                if (konpoTani == KONPO_TANI.BOX_VALUE1)
                {
                    // ----- Box梱包 -----
                    ds = conn.GetBoxData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                    // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
                    tableName_Manage = Def_T_BOXLIST_MANAGE.Name;
                    // ↑                    　

                }
                else 
                {
                    // ----- パレット梱包 -----
                    ds = conn.GetPalletData(cond, out errMsgID, out args);
                    tableName = Def_T_SHUKKA_MEISAI.Name;
                    // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
                    tableName_Manage = Def_T_PALLETLIST_MANAGE.Name;
                    // ↑                    　
                }

                // ----- 共通処理 -----
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                //if (!ComFunc.IsExistsData(ds, tableName))
                if ((!ComFunc.IsExistsData(ds, tableName)) || (!ComFunc.IsExistsData(ds, tableName_Manage)))
                // ↑
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
                this._dtManage = ds.Tables[tableName_Manage];
                // ↑

                // グリッドのバインド
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = ds.Tables[tableName];
                this.shtMeisai.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtMeisai.Redraw = true;
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
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    this.cboKonpoTani.Focus();
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

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>D.Okumura 2018/10/10 多言語化対応</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                // 更新データ作成
                DataTable dtDispData = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtUpdate = dtDispData.Clone();
                foreach (DataRow dr in dtDispData.Select(ComDefine.FLD_BTN_STATE + " = " + UtilConvert.PutQuot(ComDefine.BUTTON_TEXT_KAIJYO)))
                {
                    dtUpdate.Rows.Add(dr.ItemArray);
                }
                //@@@ 2011/03/07 M.Tsutsumi Add No.44
                DataTable dtInsert = dtDispData.Clone();
                foreach (DataRow dr in dtDispData.Select(ComDefine.FLD_BTN_STATE + " = " + UtilConvert.PutQuot(ComDefine.BUTTON_TEXT_TOUROKU)))
                {
                    dtInsert.Rows.Add(dr.ItemArray);
                }
                // @@@ ↑

                // 更新処理
                bool ret = false;
                string errMsgID = null;
                string[] args = null;
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                ConnM01 conn = new ConnM01();
                CondM01 cond = new CondM01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPO, ComDefine.BUTTON_TEXT_TOUROKU);
                cond.KonpoNo = this.lblKonpo.Text + this.txtKonpoNo.Text;

                if (konpoTani == KONPO_TANI.BOX_VALUE1)
                {
                    // ----- Box梱包 -----
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //ret = conn.UpdBoxData(cond, dtUpdate, dtInsert, out errMsgID, out args);
                    ret = conn.UpdBoxData(cond, this._dtManage, dtUpdate, dtInsert, out errMsgID, out args);
                    // ↑
                }
                else
                {
                    // ----- パレット梱包 -----
                    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
                    //ret = conn.UpdPalletData(cond, dtUpdate, dtInsert, out errMsgID, out args);
                    ret = conn.UpdPalletData(cond, this._dtManage, dtUpdate, dtInsert, out errMsgID, out args);
                    // ↑
                }

                if (!ret)
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
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

        #endregion

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/25</create>
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
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 行がない場合は処理を抜ける
                if (this.shtMeisai.Rows.Count < 1)
                {
                    return;
                }

                // 選択されているセル範囲をすべて取得する
                Range[] objRanges = shtMeisai.GetBlocks(BlocksType.Selection);

                foreach (Range range in objRanges)
                {
                    // Range内行ループ
                    for (int row = range.TopRow; row <= range.BottomRow; row++)
                    {
                        // 状態区分チェック
                        string jyotaiFlag = this.shtMeisai[SHEET_COL_STATE_BUTTON, row].Text;
                        if (jyotaiFlag != ComDefine.BUTTON_TEXT_TOUROKU)
                        {
                            // 登録行以外は、削除することはできません。
                            this.ShowMessage("M0100050011");
                            return;
                        }
                    }
                }

                // 選択行を削除してもよろしいですか？
                if (this.ShowMessage("M0100050010") != DialogResult.OK)
                {
                    return;
                }

                this.shtMeisai.Redraw = false;
                for (int i = objRanges.Length - 1; i >= 0; i--)
                {
                    // 行数を求める
                    int rowCount = objRanges[i].BottomRow - objRanges[i].TopRow + 1;
                    // 行削除
                    this.shtMeisai.RemoveRow(objRanges[i].TopRow, rowCount, false);
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
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
                if (ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();
                this.cboKonpoTani.Focus();
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
        /// <create>Y.Higuchi 2010/08/25</create>
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

        #region 梱包単位

        /// --------------------------------------------------
        /// <summary>
        /// 梱包単位の選択インデックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaTani_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ClearMessage();
            this.ChangeMode();
        }

        #endregion

        #region 表示

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // ----- 表示 -----
                if (!this.RunSearch())
                {
                    this.txtKonpoNo.Focus();
                    return;
                }
                // モード切り替え
                this.ChangeEnableViewMode(true);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 全選択

        /// --------------------------------------------------
        /// <summary>
        /// 全選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.shtMeisai.Redraw = false;
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (this.shtMeisai[SHEET_COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_KONPO)
                    {
                        this.shtMeisai[SHEET_COL_STATE_BUTTON, i].Value = ComDefine.BUTTON_TEXT_KAIJYO;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 全選択解除

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.shtMeisai.Redraw = false;
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (this.shtMeisai[SHEET_COL_STATE_BUTTON, i].Text == ComDefine.BUTTON_TEXT_KAIJYO)
                    {
                        this.shtMeisai[SHEET_COL_STATE_BUTTON, i].Value = ComDefine.BUTTON_TEXT_KONPO;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 梱包追加

        /// --------------------------------------------------
        /// <summary>
        /// 梱包追加ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnKonpoAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                this.shtMeisai.Redraw = false;
                // １行追加
                this.shtMeisai.InsertRow(this.shtMeisai.MaxRows, false);
                int row = this.shtMeisai.MaxRows - 1;
                this.shtMeisai[SHEET_COL_STATE_BUTTON, row].Text = ComDefine.BUTTON_TEXT_TOUROKU;
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                if (konpoTani == KONPO_TANI.BOX_VALUE1)
                {
                    // ----- BOX梱包 -----
                    this.shtMeisai[SHEET_COL_TAG_NO, row].Enabled = true;
                    this.shtMeisai[SHEET_COL_TAG_NO, row].Lock = false;
                }
                else
                {
                    // ----- パレット梱包 -----
                    this.shtMeisai[SHEET_COL_BOX_NO, row].Enabled = true;
                    this.shtMeisai[SHEET_COL_BOX_NO, row].Lock = false;
                }
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
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
        /// <update></update>
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
                            switch (e.Position.Column)
                            {
                                case SHEET_COL_STATE_BUTTON:
                                    // 状態ボタン
                                    if (this.shtMeisai[SHEET_COL_STATE_BUTTON, e.Position.Row].Text == ComDefine.BUTTON_TEXT_KONPO)
                                    {
                                        this.shtMeisai[SHEET_COL_STATE_BUTTON, e.Position.Row].Value = ComDefine.BUTTON_TEXT_KAIJYO;
                                    }
                                    // @@@ 2011/03/07 M.Tsutsumi Change No.44
                                    //else
                                    else if (this.shtMeisai[SHEET_COL_STATE_BUTTON, e.Position.Row].Text == ComDefine.BUTTON_TEXT_KAIJYO)
                                    // @@@ ↑
                                    {
                                        this.shtMeisai[SHEET_COL_STATE_BUTTON, e.Position.Row].Value = ComDefine.BUTTON_TEXT_KONPO;
                                    }
                                    this.shtMeisai.KeyAction(KeyAction.EndEdit);
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

        #region ClippingData

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリップボード操作が行われたときに発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_ClippingData(object sender, ClippingDataEventArgs e)
        {
            try
            {
                switch (e.ClippingAction)
                {
                    case ClippingAction.Paste:
                        switch (this.shtMeisai.ActivePosition.Column)
                        {
                            case SHEET_COL_BOX_NO:
                            case SHEET_COL_TAG_NO:
                                // BoxNo、TagNo列の貼り付けはキャンセルする。
                                e.Cancel = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region LeaveEdit

        /// --------------------------------------------------
        /// <summary>
        /// セルが非編集モードに入る場合に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                if (((this.shtMeisai.ActivePosition.Column != SHEET_COL_BOX_NO) &&
                     (this.shtMeisai.ActivePosition.Column != SHEET_COL_TAG_NO)) ||
                    (string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Text)))
                {
                    return;
                }
                this.ClearMessage();
                // 入力値の整形
                this.shtMeisai.ActiveCell.Text = this.GetSheetInputFormat(this.shtMeisai.ActiveCell.Text);
                this.shtMeisai.Refresh();
                int col = this.shtMeisai.ActivePosition.Column;
                int row = this.shtMeisai.ActivePosition.Row;
                for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                {
                    if (i == row) continue;
                    if (this.shtMeisai[col, i].Text == this.shtMeisai.ActiveCell.Text)
                    {
                        if (col == SHEET_COL_TAG_NO)
                        {
                            // 入力されたTagNo.は、{0}行目に存在します。
                            this.ShowMessage("M0100050009", (i + 1).ToString());
                        }
                        else
                        {
                            // 入力されたBoxNo.は、{0}行目に存在します。
                            this.ShowMessage("M0100050016", (i + 1).ToString());
                        }
                        e.Cancel = true;
                        return;
                    }
                }

                // 入力値チェック
                if (!this.CheckSheetInput(this.shtMeisai.ActiveCell.Text, row))
                {
                    e.Cancel = true;
                    return;
                }

                // ロック
                this.shtMeisai[col, row].Enabled = false;
                this.shtMeisai[col, row].Lock = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                e.Cancel = true;
            }
        }

        #endregion

        #region RowInserted

        /// --------------------------------------------------
        /// <summary>
        /// 行を挿入した後に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>M.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_RowInserted(object sender, RowInsertedEventArgs e)
        {
            try
            {
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
        /// モード変更時の切り替え処理
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeMode()
        {
            try
            {
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                if (konpoTani == KONPO_TANI.BOX_VALUE1)
                {
                    // ----- Box梱包 -----
                    // 入力コントロールの切り替え
                    this.lblKonpo.Text = ComDefine.PREFIX_BOXNO;
                    this.txtKonpoNo.Text = string.Empty;
                }
                else
                {
                    // ----- パレット梱包 -----
                    // 入力コントロールの切り替え
                    this.lblKonpo.Text = ComDefine.PREFIX_PALLETNO;
                    this.txtKonpoNo.Text = string.Empty;
                }
                // グリッドの切替
                this.ChangeSheet();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView">表示状態かどうか</param>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 全選択
            this.btnAllSelect.Enabled = isView;
            // 全選択解除
            this.btnAllDeselect.Enabled = isView;
            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            // 梱包追加
            this.btnKonpoAdd.Enabled = isView;
            // @@@ ↑
            // 検索条件のロック解除
            this.grpSearch.Enabled = !isView;
            // 保存ボタン
            this.fbrFunction.F01Button.Enabled = isView;
            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            // 行削除ボタン
            this.fbrFunction.F03Button.Enabled = isView;
            // @@@ ↑
        }

        #region グリッドの切り替え

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの切り替え
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeSheet()
        {
            try
            {
                this.shtMeisai.Redraw = false;
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                if (konpoTani == SHUKKA_TANI.BOX_VALUE1)
                {
                    // ----- Box梱包 -----
                    this.shtMeisai.Columns[SHEET_COL_TAG_NO].Hidden = false;
                    this.shtMeisai.Columns[SHEET_COL_BOX_NO].Hidden = true;
                    this.shtMeisai.Columns[SHEET_COL_NUM].Hidden = false;
                    // 2015/11/20 H.Tajimi 備考対応
                    this.shtMeisai.Columns[SHEET_COL_BIKO].Hidden = false;
                    // ↑
                }
                else
                {
                    // ----- パレット梱包 -----
                    this.shtMeisai.Columns[SHEET_COL_TAG_NO].Hidden = true;
                    this.shtMeisai.Columns[SHEET_COL_BOX_NO].Hidden = false;
                    this.shtMeisai.Columns[SHEET_COL_NUM].Hidden = true;
                    // 2015/11/20 H.Tajimi 備考対応
                    this.shtMeisai.Columns[SHEET_COL_BIKO].Hidden = true;
                    // ↑
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/08/25</create>
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
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;
            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region シートの入力値のフォーマット

        /// --------------------------------------------------
        /// <summary>
        /// シートの入力値のフォーマット
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns></returns>
        /// <create>M.Tsutsumi 2011/03/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetSheetInputFormat(string value)
        {
            string ret = value;
            string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
            if (konpoTani == KONPO_TANI.BOX_VALUE1)
            {
                // ----- BOX梱包 -----
                if (value.Length < LENGTH_TAG_NO)
                {
                    ret = value.PadLeft(LENGTH_TAG_NO, '0');
                }
            }
            else
            {
                // ----- パレット梱包 -----
                if (!value.StartsWith(ComDefine.PREFIX_BOXNO) && value.Length < LENGTH_BOX_NO)
                {
                    ret = ComDefine.PREFIX_BOXNO + value.PadLeft(LENGTH_BOX_NO - 1, '0');
                }
            }
            return ret;
        }

        #endregion

        #region シートの入力値のチェック

        /// --------------------------------------------------
        /// <summary>
        /// シートの入力値のチェック
        /// </summary>
        /// <param name="no">入力値</param>
        /// <param name="row">行No</param>
        /// <returns>true:OK/false:NG</returns>
        /// <create>M.Tsutsumi 2011/03/08</create>
        /// <update>H.Tajimi 2015/11/20 備考対応</update>
        /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
        /// <update>D.Okumura 2018/10/10 多言語化対応</update>
        /// --------------------------------------------------
        private bool CheckSheetInput(string no, int row)
        {
            try
            {
                string konpoTani = this.cboKonpoTani.SelectedValue.ToString();
                string errMsgID = null;
                string[] args = null;
                DataSet ds;
                CondM01 cond = new CondM01(this.UserInfo, ComDefine.BUTTON_TEXT_KONPO, ComDefine.BUTTON_TEXT_TOUROKU);
                ConnM01 conn = new ConnM01();
                cond.KonpoNo = this.lblKonpo.Text + this.txtKonpoNo.Text;
                if (konpoTani == KONPO_TANI.BOX_VALUE1)
                {
                    // ----- Box梱包 -----
                    cond.TagNo = no;
                    ds = conn.GetBoxDataAdd(cond, out errMsgID, out args);
                }
                else
                {
                    // ----- パレット梱包 -----
                    cond.BoxNo = no;
                    ds = conn.GetPalletDataAdd(cond, out errMsgID, out args);
                }

                // ----- エラー処理 -----
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }
                if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                // ----- 表示 -----
                this.shtMeisai.Redraw = false;
                if (0 <= row)
                {
                    DataTable dt = this.shtMeisai.DataSource as DataTable;
                    DataRow dr = dt.Rows[row];
                    dr[Def_T_SHUKKA_MEISAI.SHUKKA_FLAG] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                    dr[Def_T_SHUKKA_MEISAI.NONYUSAKI_CD] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                    dr[Def_M_NONYUSAKI.NONYUSAKI_NAME] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    dr[Def_M_NONYUSAKI.SHIP] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.SHIP);
                    dr[Def_T_SHUKKA_MEISAI.AR_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.AR_NO);
                    if (konpoTani == KONPO_TANI.BOX_VALUE1)
                    {
                        dr[Def_T_SHUKKA_MEISAI.TAG_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.TAG_NO);
                        // 2015/11/20 H.Tajimi 備考列追加
                        dr[Def_T_SHUKKA_MEISAI.BIKO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.BIKO);
                        // ↑
                    }
                    else
                    {
                        dr[Def_T_SHUKKA_MEISAI.BOX_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.BOX_NO);
                    }
                    dr[Def_T_SHUKKA_MEISAI.SEIBAN] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SEIBAN);
                    dr[Def_T_SHUKKA_MEISAI.CODE] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.CODE);
                    dr[Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN);
                    dr[Def_T_SHUKKA_MEISAI.AREA] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.AREA);
                    dr[Def_T_SHUKKA_MEISAI.FLOOR] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.FLOOR);
                    dr[Def_T_SHUKKA_MEISAI.KISHU] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KISHU);
                    dr[Def_T_SHUKKA_MEISAI.ST_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.ST_NO);
                    dr[Def_T_SHUKKA_MEISAI.HINMEI_JP] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.HINMEI_JP);
                    dr[Def_T_SHUKKA_MEISAI.HINMEI] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.HINMEI);
                    dr[Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI);
                    dr[Def_T_SHUKKA_MEISAI.KUWARI_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KUWARI_NO);
                    dr[Def_T_SHUKKA_MEISAI.NUM] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NUM);
                    dr[Def_T_SHUKKA_MEISAI.VERSION] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.VERSION);
                    // 2015/12/09 H.Tajimi M_NO対応
                    dr[Def_T_SHUKKA_MEISAI.M_NO] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.M_NO);
                    // ↑
                    dr[ComDefine.FLD_BTN_STATE] = ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, ComDefine.FLD_BTN_STATE);
                    this.shtMeisai.DataSource = dt;
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

    }
}
