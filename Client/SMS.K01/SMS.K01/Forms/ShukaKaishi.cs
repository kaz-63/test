using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using DSWUtil;
using WsConnection;
using Commons;
using WsConnection.WebRefK01;
using ActiveReportsHelper;
using GrapeCity.Win.ElTabelle;
using SystemBase.Util;
using Commons.Properties;

namespace SMS.K01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 集荷開始
    /// </summary>
    /// <create>H.Tsunamura 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukaKaishi : SystemBase.Forms.CustomOrderForm
    {

        #region 定数
        // 印刷カラム番号
        private const int SHEET_COL_INSATU = 0;
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
        /// Floorの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_FLOOR_COL = 7;
        /// --------------------------------------------------
        /// <summary>
        /// M-Noの列インデックス
        /// </summary>
        /// <create>H.Tajimi 2015/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_M_NO_COL = 9;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_ON = 1;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_OFF = 0;
        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// T_ARが存在しなくても続行するフラグ
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isConfirmNoAR = false;
        /// --------------------------------------------------
        /// <summary>
        /// 続行するかどうか問い合わせたときの出荷区分
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _beforeShukkaFlag = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 続行するかどうか問い合わせたときの納入先名
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _beforeNonyusakiName = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 続行するかどうか問い合わせたときのAR No.
        /// </summary>
        /// <create>K.Tsutsumi 2011/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _beforeARNo = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 納入先コード
        /// </summary>
        /// <create>T.Sakiori 2012/04/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 初回起動
        /// </summary>
        /// <create>J.Chen 2023/08/29</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isShukkaKeikaku = false;

        #endregion

        #region プロパティ

        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public static ShukaKaishi ShukaKaishiInstance { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public string BukkenNameText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 便
        /// </summary>
        /// <create>J.Chen 2023/08/28</create>
        /// --------------------------------------------------
        public string ShipText { get; set; }

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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukaKaishi(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>H.Tajimi 2015/11/17 AutoFilter対応</update>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update>H.Iimuro 2022/10/04 列：図面/型式2の追加</update>
        /// <update>J.Chen 2022/12/19 TAG便名追加</update>
        /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                this.MakeCmbBox(this.cboDispSelect, DISP_SELECT_SYUKA.GROUPCD);
                this.MakeCmbBox(this.cboIssueSelect, HAKKOU_SELECT.GROUPCD);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.InitializeSheet(shtMeisai);
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                this.rdoLock.Checked = false;
                this.rdoUnLock.Checked = false;
                this.grpKeyAction.Enabled = false;
                // 2015/12/02 H.Tajimi リスト無効
                this.fbrFunction.F06Button.Enabled = false;
                // ↑
                this.shtMeisai.KeepHighlighted = true;
                // 2015/11/17 H.Tajimi
                // El TabelleのAutoFilterは行の高さを0とすることでフィルタリング状態を作り出しており
                // ユーザ操作により行の高さ変更が行われると、フィルタリング状態と表示データの不整合が
                // 発生する可能性があるため、ユーザ操作による行の高さ変更を不可とする
                this.shtMeisai.RowHeaders.AllowResize = false;
                // ↑

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Print;
                shtMeisai.ColumnHeaders[1].Caption = SMS.K01.Properties.Resources.ShukaKaishi_State;
                shtMeisai.ColumnHeaders[2].Caption = SMS.K01.Properties.Resources.ShukaKaishi_TagNo;
                shtMeisai.ColumnHeaders[3].Caption = SMS.K01.Properties.Resources.ShukaKaishi_ProductNo;
                shtMeisai.ColumnHeaders[4].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Code;
                shtMeisai.ColumnHeaders[5].Caption = SMS.K01.Properties.Resources.ShukaKaishi_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[6].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Area;
                shtMeisai.ColumnHeaders[7].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Floor;
                shtMeisai.ColumnHeaders[8].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Model;
                shtMeisai.ColumnHeaders[9].Caption = SMS.K01.Properties.Resources.ShukaKaishi_MNo;
                shtMeisai.ColumnHeaders[10].Caption = SMS.K01.Properties.Resources.ShukaKaishi_JpName;
                shtMeisai.ColumnHeaders[11].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Name;
                shtMeisai.ColumnHeaders[12].Caption = SMS.K01.Properties.Resources.ShukaKaishi_DrawingNoFormat;
                shtMeisai.ColumnHeaders[13].Caption = SMS.K01.Properties.Resources.ShukaKaishi_ZUMEN_KEISHIKI2;
                shtMeisai.ColumnHeaders[14].Caption = SMS.K01.Properties.Resources.ShukaKaishi_SectioningNo;
                shtMeisai.ColumnHeaders[15].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Quantity;
                shtMeisai.ColumnHeaders[16].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Free1;
                shtMeisai.ColumnHeaders[17].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Free2;
                shtMeisai.ColumnHeaders[18].Caption = SMS.K01.Properties.Resources.ShukaKaishi_Memo;
                shtMeisai.ColumnHeaders[19].Caption = SMS.K01.Properties.Resources.ShukaKaishi_CustomsStatus;
                shtMeisai.ColumnHeaders[20].Caption = SMS.K01.Properties.Resources.ShukaKaishi_STNo;
                shtMeisai.ColumnHeaders[21].Caption = SMS.K01.Properties.Resources.ShukaKaishi_AssemblyDate;
                shtMeisai.ColumnHeaders[22].Caption = SMS.K01.Properties.Resources.ShukaKaishi_BoxNo;
                shtMeisai.ColumnHeaders[23].Caption = SMS.K01.Properties.Resources.ShukaKaishi_PalletNo;

                // 出荷元列追加による 2022/10/11（TW-Tsuji）
                //　【注】 SMS.K01の Resources.resx に追加   ShukaKaishi_SipFromName 出荷元
                //
                //　シートの最右列に出荷元表示列を追加
                shtMeisai.ColumnHeaders[23].Caption = SMS.K01.Properties.Resources.ShukaKaishi_SipFromName;

                // TAG便名
                shtMeisai.ColumnHeaders[24].Caption = SMS.K01.Properties.Resources.ShukaKaishi_TagShip;

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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>J.Chen 2023/08/29 画面起動連携</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスを設定する。
                this.rdoIssue.Focus();

                if (_isShukkaKeikaku)
                {
                    //this.rdoNote.Focus();
                    //this.rdoNote.Checked = true;
                    ////ChangeMode();
                    this.btnDisp_Click(null, null);
                }
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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update>J.Chen 2023/08/29 画面起動連携</update>
        /// <update>R.Kubota 2023/12/06 「引渡済」のTAG状態集計を追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 検索条件のクリア
                this.txtARNo.Text = "";
                this.txtCODE.Text = "";
                this.txtNonyusakiCD.Text = "";
                this.txtNonyusakiCD.Enabled = false;
                this.txtNonyuusaki.Text = "";
                this.txtSeiban.Text = "";
                this.txtShip.Text = "";
                this.pnlSeach.Enabled = true;

                // 2011/03/09 K.Tsutsumi Add 進捗件数取得
                this.txtHikiwatashiShuka.Text = "0/0";
                this.txtHikiwatashi.Text = "0/0";
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                // ↑

                // グリッドに対するボタン状態のクリア
                this.btnAllCheck.Enabled = false;
                this.btnAllNotCheck.Enabled = false;
                this.btnRangeCheck.Enabled = false;
                this.btnRangeNotCheck.Enabled = false;

                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;
                // 2015/12/02 H.Tajimi クリア無効
                this.fbrFunction.F06Button.Enabled = false;
                // ↑

                this.cboDispSelect.SelectedValue = DISP_SELECT_SYUKA.DEFAULT_VALUE1;
                this.cboIssueSelect.SelectedValue = HAKKOU_SELECT.DEFAULT_VALUE1;
                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.cboIssueSelect.Enabled = false;
                this.nudPrintPos.Enabled = false;
                this.btnListSelect.Enabled = true;
                this.nudPrintPos.Value = 1;
                this.rdoIssue.Checked = true;
                this.rdoNote.Checked = false;

                // 2011/03/09 K.Tsutsumi Add T_ARが存在しなくても続行可能
                this._isConfirmNoAR = false;
                this._beforeShukkaFlag = string.Empty;
                this._beforeNonyusakiName = string.Empty;
                this._beforeARNo = string.Empty;
                // ↑

                this.txtNonyuusaki.Clear();
                this.txtShip.Clear();

                this.txtNonyuusaki.Text = this.BukkenNameText ?? this.txtNonyuusaki.Text;
                this.txtShip.Text = this.ShipText ?? this.txtShip.Text;

                if (this.BukkenNameText != null && this.ShipText != null)
                {
                    this.BukkenNameText = null;
                    this.ShipText = null;
                    _isShukkaKeikaku = true;
                }

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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
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

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>K.Tsutsumi 2012/05/14</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // コントロールの値が入っているかどうかのチェック
                if (string.IsNullOrEmpty(this.txtNonyuusaki.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("K0100010007");
                    this.txtNonyuusaki.Focus();
                    return false;
                }

                if (this.txtARNo.Enabled)
                {
                    if (string.IsNullOrEmpty(this.txtARNo.Text))
                    {
                        // ARNoを入力して下さい。
                        this.ShowMessage("A9999999018");
                        this.txtARNo.Focus();
                        return false;
                    }
                }
                // 2012/05/14 K.Tsutsumi Delete 不要
                //else
                //{
                //    if (string.IsNullOrEmpty(this.txtShip.Text))
                //    {
                //        // 便を入力して下さい。
                //        this.ShowMessage("A9999999017");
                //        this.txtShip.Focus();
                //        return false;
                //    }
                //}
                // ↑
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
        /// <create>H.Tsunamura 2010/06/28</create>
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
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update>R.Kubota 2023/12/06 「引渡済」のTAG状態集計を追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondK01 condK01 = new CondK01(this.UserInfo);
                WsConnection.ConnK01 connK01 = new ConnK01();

                condK01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                condK01.Seiban = this.txtSeiban.Text;
                condK01.Code = this.txtCODE.Text;
                condK01.NounyusakiCD = this.txtNonyusakiCD.Text;
                condK01.DisplaySelect = this.cboDispSelect.SelectedValue.ToString();

                if (this.txtARNo.Enabled)
                {
                    condK01.ARNo = /*this.lblAR.Text*/"AR" + this.txtARNo.Text;
                }


                DataSet ds;
                string errMsgID;
                string kanriNo;

                // 2011/03/09 K.Tsutsumi Add 進捗件数取得
                this.txtHikiwatashiShuka.Text = "0/0";
                this.txtHikiwatashi.Text = "0/0";
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                // ↑


                bool ret = connK01.GetShukaData(condK01, out ds, out errMsgID, out kanriNo);

                // 管理Ｎｏの表示
                if (!string.IsNullOrEmpty(kanriNo))
                {
                    this.txtNonyusakiCD.Text = kanriNo;
                }
                else
                {
                    this.txtNonyusakiCD.Text = "";
                }

                // 2011/03/09 K.Tsutsumi Add 進捗件数欄追加
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_PROGRESS) == true)
                {
                    DataTable dt = ds.Tables[ComDefine.DTTBL_PROGRESS];

                    int countShuka = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA);
                    int countHikiwatashiNoShuka = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_HIKIWATASHI_NO_SHUKA);
                    int totalCountShukaHikiwatashi = countShuka + countHikiwatashiNoShuka;

                    this.txtHikiwatashiShuka.Text = totalCountShukaHikiwatashi.ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtHikiwatashi.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_HIKIWATASHI).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtShuka.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtBoxKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_BOXKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtPalletKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_PALLETKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                }
                // ↑

                // 2011/03/09 K.Tsutsumi Add T_ARが存在しなくても続行可能
                if (ret == false)
                {
                    this.ShowMessage(errMsgID);
                }
                else if ((ret == true) && (string.IsNullOrEmpty(condK01.ARNo) == false))
                {
                    // T_ARは取得できたか？
                    if (ComFunc.IsExistsData(ds, Def_T_AR.Name) == false)
                    {
                        // 前回と検索条件が違うか？
                        if ((this._beforeShukkaFlag != condK01.ShukkaFlag) || (this._beforeNonyusakiName != condK01.NonyusakiName) || (this._beforeARNo != condK01.ARNo))
                        {
                            // 確認させる
                            this._isConfirmNoAR = false;
                        }

                        if (this._isConfirmNoAR == false)
                        {
                            // 問い合わせを行うときの検索条件待避
                            this._beforeShukkaFlag = condK01.ShukkaFlag;
                            this._beforeNonyusakiName = condK01.NonyusakiName;
                            this._beforeARNo = condK01.ARNo;

                            // AR No.[{0}]のAR情報が登録されていません。\r\nこのまま処理を続けますか？
                            if (this.ShowMessage("K0100010005", this.txtARNo.Text) != DialogResult.OK)
                            {
                                ret = false;
                            }
                            else
                            {
                                this._isConfirmNoAR = true;
                            }
                        }
                    }
                    else
                    {
                        this._beforeShukkaFlag = string.Empty;
                        this._beforeNonyusakiName = string.Empty;
                        this._beforeARNo = string.Empty;
                    }
                }
                else
                {
                    this._beforeShukkaFlag = string.Empty;
                    this._beforeNonyusakiName = string.Empty;
                    this._beforeARNo = string.Empty;
                }
                // ↑

                if (!ret)
                {
                    // 2011/03/09 K.Tsutsumi Delete T_ARが存在しなくても続行可能
                    //this.ShowMessage(errMsgID);
                    // ↑
                    this.btnAllCheck.Enabled = false;
                    this.btnAllNotCheck.Enabled = false;
                    this.btnRangeCheck.Enabled = false;
                    this.btnRangeNotCheck.Enabled = false;
                    this.cboIssueSelect.Enabled = false;
                    this.nudPrintPos.Enabled = false;
                    this.shtMeisai.Enabled = false;
                    this.shtMeisai.MaxRows = 0;
                    this.fbrFunction.F01Button.Enabled = false;
                    this.fbrFunction.F02Button.Enabled = false;
                    // 2015/12/02 H.Tajimi クリア無効
                    this.fbrFunction.F06Button.Enabled = false;
                    // ↑

                    return false;
                }

                this.shtMeisai.DataSource = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
                this.grpMode.Enabled = false;
                this.pnlSeach.Enabled = false;

                this.shtMeisai.Enabled = true;
                this.btnListSelect.Enabled = false;

                if (this.rdoIssue.Checked)
                {
                    this.btnAllCheck.Enabled = true;
                    this.btnAllNotCheck.Enabled = true;
                    this.btnRangeCheck.Enabled = true;
                    this.btnRangeNotCheck.Enabled = true;
                    this.cboIssueSelect.Enabled = true;
                    this.nudPrintPos.Enabled = true;
                    this.shtMeisai.Columns[SHEET_COL_INSATU].Enabled = true;
                    this.grpKeyAction.Enabled = true;
                    
                    this.fbrFunction.F01Button.Enabled = true;
                    this.fbrFunction.F02Button.Enabled = true;
                    // 2015/12/02 H.Tajimi クリア有効
                    this.fbrFunction.F06Button.Enabled = true;
                    // ↑
                }
                else
                {
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtMeisai.MaxRows)
                    {
                        this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                    }
                    this.shtMeisai.Columns[SHEET_COL_INSATU].Enabled = false;
                    // 2015/12/02 H.Tajimi クリア有効
                    this.fbrFunction.F06Button.Enabled = true;
                    // ↑
                }
                // 2015/11/17 H.Tajimi AutoFilter設定
                this.SetEnableAutoFilter(false);
                // ↑

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

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F01Button_Click(sender, e);
                printData(false, getData());
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.fbrFunction.F01Button.Enabled = false;
                this.fbrFunction.F02Button.Enabled = false;

                base.fbrFunction_F02Button_Click(sender, e);
                printData(true, getData());
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.fbrFunction.F01Button.Enabled = true;
                this.fbrFunction.F02Button.Enabled = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/28</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update>R.Kubota 2023/12/06 「引渡済」のTAG状態集計を追加</update>
        /// <update>J.Chen 2024/10/02 「引渡/集荷済」のTAG状態集計を追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                // グリッド
                this.SheetClear();
                // AutoFilter設定
                this.SetEnableAutoFilter(false);
                // フラグ
                this._isConfirmNoAR = false;
                this._beforeShukkaFlag = string.Empty;
                this._beforeNonyusakiName = string.Empty;
                this._beforeARNo = string.Empty;
                this.grpKeyAction.Enabled = false;
                this.rdoLock.Checked = false;
                this.rdoUnLock.Checked = false;
                // 進捗件数
                this.txtHikiwatashiShuka.Text = "0/0";
                this.txtHikiwatashi.Text = "0/0";
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.ChangeEnableViewMode(false);
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
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }

                this.grpMode.Enabled = true;
                this.grpSearch.Enabled = true;
                this.grpKeyAction.Enabled = false;
                this.rdoLock.Checked = false;
                this.rdoUnLock.Checked = false;
                this.DisplayClear();
                // グリッド
                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }
                this.shtMeisai.MaxRows = 0;
                this.shtMeisai.Enabled = false;
                this.SetEnableAutoFilter(false);
                this.rdoIssue.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        # region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 表示クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/25</create>
        /// <update>H.Tajimi 2015/11/30 納入先(User)選択を早くする</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            try
            {
                // 2015/11/30 H.Tajimi 納入先一覧画面を表示
                if (!this.ShowNonyusakiIchiran())
                {
                    // フォーカスのセット
                    this.txtNonyuusaki.Focus();
                }
                // ↑
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/26</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// <update>H.Tajimi 2015/11/17 AutoFilter対応</update>
        /// --------------------------------------------------
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            this.shtMeisai.Redraw = false;

            this.SetAllPrintCheck(true);

            this.shtMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック 2010/11/25 Rangeで変更するよう修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/26</create>
        /// <update>H.Tsunamura 2010/11/25</update>
        /// <update>H.Tajimi 2015/11/17 AutoFilter対応</update>
        /// --------------------------------------------------
        private void btnAllNotCheck_Click(object sender, EventArgs e)
        {
            this.shtMeisai.Redraw = false;

            this.SetAllPrintCheck(false);

            this.shtMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/11/25</create>
        /// <update>H.Tajimi 2015/11/17 AutoFilter対応</update>
        /// --------------------------------------------------
        private void btnRangeCheck_Click(object sender, EventArgs e)
        {
            this.shtMeisai.Redraw = false;

            this.SetRangePrintCheck(true);

            this.shtMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/11/25</create>
        /// <update>H.Tajimi 2015/11/17 AutoFilter対応</update>
        /// --------------------------------------------------
        private void btnRangeNotCheck_Click(object sender, EventArgs e)
        {
            this.shtMeisai.Redraw = false;

            this.SetRangePrintCheck(false);

            this.shtMeisai.Redraw = true;
        }

        # endregion

        #region シートのクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>H.Tajimi 2015/12/01</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
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

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/25</create>
        /// <update>K.Tsutsumi 2012/05/14</update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 2012/05/14 K.Tsutsumi Change クリアするフィールドが不足している
            //if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            //{
            //    this.txtARNo.Enabled = false;
            //    this.txtARNo.Text = "";
            //}
            //else
            //{
            //    this.txtARNo.Enabled = true;
            //    this.txtShip.Text = "";
            //}
            this.ChangeShukkaFlag();
            // ↑

        }

        /// --------------------------------------------------
        /// <summary>
        /// モード選択時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tsunamura 2010/06/25</create>
        /// <update></update>
        /// --------------------------------------------------
        private void rdoIssue_CheckedChanged(object sender, EventArgs e)
        {
            // 表示ボタンクリック後の制御リセット
            if (!this.rdoIssue.Checked)
            {
                this.btnAllCheck.Enabled = false;
                this.btnAllNotCheck.Enabled = false;
                this.btnRangeCheck.Enabled = false;
                this.btnRangeNotCheck.Enabled = false;
                this.cboIssueSelect.Enabled = false;
                this.nudPrintPos.Enabled = false;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷用データ取得
        /// </summary>
        /// <returns>印刷用明細データ</returns>
        /// <create>H.Tsunamura 2010/07/07</create>
        /// <update>D.Okumura 2020/10/26 EFA_SMS-150 QRコードのSHIP欄にARNoを出力する対応</update>
        /// <update>T.Nukaga 2021/04/15 機種桁数拡張対応</update>
        /// <update>J.Chen 2023/02/22 分割用桁数変更</update>
        /// <update>J.Chen 2024/08/08 TAG発行可能チェック用行番号追加</update>
        /// --------------------------------------------------
        private DataSet getData()
        {
            this.ClearMessage();

            // 現在表示中の明細データ取得
            DataTable dtMeisai = new DataTable();
            dtMeisai = this.shtMeisai.DataSource as DataTable;

            // 図番/形式分割用の列追加
            if (!dtMeisai.Columns.Contains(ComDefine.FLD_ZUMEN_KEISHIKI_1))
            {
                dtMeisai.Columns.Add(ComDefine.FLD_ZUMEN_KEISHIKI_1, typeof(string));
                dtMeisai.Columns.Add(ComDefine.FLD_ZUMEN_KEISHIKI_2, typeof(string));
            }

            // 行番号追加
            if (!dtMeisai.Columns.Contains(ComDefine.FLD_CNT))
            {
                dtMeisai.Columns.Add(ComDefine.FLD_CNT, typeof(string));
            }
            // 行番号振分
            int rowNumber = 1;
            foreach (DataRow drTemp in dtMeisai.Rows)
            {
                drTemp[ComDefine.FLD_CNT] = rowNumber;
                rowNumber++;
            }

            DataSet ds = new DataSet();
            // 現品TAG用テーブル
            DataTable dtGenpin = dtMeisai.Clone();
            dtGenpin.TableName = ComDefine.DTTBL_GENPIN;
            // TAGLIST用テーブル
            DataTable dtTagList = dtMeisai.Clone();
            dtTagList.TableName = ComDefine.DTTBL_TAGLIST;

            // 明細データから印刷するデータを取得
            foreach (DataRow item in dtMeisai.Select(ComDefine.FLD_INSATU + " = 1 "))
            {
               // string nonyusakiName = ComFunc.GetFld(item, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                string shukkaFlg = ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                string nounyusakiCD = ComFunc.GetFld(item, Def_M_NONYUSAKI.NONYUSAKI_CD);
                string tagNo = ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.TAG_NO);
                string nounyusakiName = ComFunc.GetFld(item, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                string shipName = ComFunc.GetFld(item, Def_M_NONYUSAKI.SHIP);
                string arNo = ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.AR_NO);

                // 2011/03/01 K.Tsutsumi Change 見た目で分割
                //string[] nonyusaki = UtilString.DivideString(ComFunc.GetFld(item, Def_M_NONYUSAKI.NONYUSAKI_NAME), 25);
                string[] nonyusaki = ComFunc.DivideStringEx(ComFunc.GetFld(item, Def_M_NONYUSAKI.NONYUSAKI_NAME), 35);
                // ↑
                string[] area = UtilString.DivideString(ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.AREA), 10);
                string[] floor = UtilString.DivideString(ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.FLOOR), 20);
                item[ComDefine.FLD_NONYUSAKI_NAME_1] = nonyusaki[0];
                item[ComDefine.FLD_NONYUSAKI_NAME_2] = nonyusaki[1];
                item[ComDefine.FLD_AREA_1] = area[0];
                item[ComDefine.FLD_AREA_2] = area[1];
                item[ComDefine.FLD_FLOOR_1] = floor[0];
                item[ComDefine.FLD_FLOOR_2] = floor[1];

                string[] zumenKeishiki = UtilString.DivideString(ComFunc.GetFld(item, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI), 15);
                item[ComDefine.FLD_ZUMEN_KEISHIKI_1] = zumenKeishiki[0];
                item[ComDefine.FLD_ZUMEN_KEISHIKI_2] = zumenKeishiki[1];

                item[ComDefine.FLD_BERCODE] = shukkaFlg + nounyusakiCD.PadLeft(4, '0') + tagNo;

                if (shukkaFlg == SHUKKA_FLAG.AR_VALUE1)
                {
                    item[ComDefine.FLD_QRCODE] = tagNo + " " + arNo + " " + nounyusakiName;
                    // Shio NoにAR Noを表示する
                    item[Def_M_NONYUSAKI.SHIP] = arNo;
                }
                else
                {
                    item[ComDefine.FLD_QRCODE] = tagNo + " " + shipName + " " + nounyusakiName;
                }

                string[] tehaino = ComFunc.GetFld(item, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO).Split(' ');

                if (tehaino.Length >= 2)
                {
                    item[ComDefine.FLD_TEHAINO_1] = tehaino[0];
                    item[ComDefine.FLD_TEHAINO_2] = tehaino[1];
                }

                DataRow repDr = dtTagList.Rows.Add(item.ItemArray);

                // 現品TAGは同じデータを2個印刷する
                dtGenpin.Rows.Add(repDr.ItemArray);
                dtGenpin.Rows.Add(repDr.ItemArray);
            }

            if (dtGenpin.Rows.Count == 0)
            {
                // 印刷するデータがありません。
                this.ShowMessage("A9999999025");
                return null;
            }

            // confテーブルの作成
            DataTable dt = new DataTable(ComDefine.DTTBL_CONF);
            dt.Columns.Add(ComDefine.FLD_POS, typeof(int));
            DataRow dr = dt.NewRow();
            dr[ComDefine.FLD_POS] = this.nudPrintPos.Value;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);

            ds.Tables.Add(dtGenpin.Copy());
            ds.Tables.Add(dtTagList.Copy());
            return ds;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 印刷
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <param name="ds">印刷用明細データ</param>
        /// <create>H.Tsunamura 2010/08/18</create>
        /// <update>J.Chen 2024/08/08 TAG発行可能チェック追加</update>
        /// <update>J.Chen 履歴登録用データ追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void printData(bool isPreview, DataSet ds)
        {
            if (ds == null) return;
            
            // 現品TAG用
            Object genpin = null;
            // TAGリスト用
            Object tagList = null;


            // 発行区分がリストのみ以外
            if (this.cboIssueSelect.SelectedValue.ToString() != HAKKOU_SELECT.LIST_VALUE1)
            {
                // 発行可能チェック
                if (!this.IsCanPrint(ds.Tables[ComDefine.DTTBL_GENPIN])) return;

                // 現品TAGを追加
                genpin = ReportHelper.GetReport(ComDefine.REPORT_R0100010_CLASS_NAME, ds, ComDefine.DTTBL_GENPIN);
            }

            // 発行区分がTAGのみ以外
            if (this.cboIssueSelect.SelectedValue.ToString() != HAKKOU_SELECT.TAG_VALUE1)
            {
                // 発行可能チェック
                if (!this.IsCanPrint(ds.Tables[ComDefine.DTTBL_TAGLIST])) return;

                // TAGリスト追加
                tagList = ReportHelper.GetReport(ComDefine.REPORT_R0100020_CLASS_NAME, ds, ComDefine.DTTBL_TAGLIST);
            }


            try
            {
                if (isPreview)
                {
                    // 一括プレビュー用
                    List<object> reports = new List<object>();

                    if (genpin != null)
                    {
                        // 現品TAG追加
                        reports.Add(genpin);
                    }

                    if (tagList != null)
                    {
                        // TAGリスト追加
                        reports.Add(tagList);
                    }

                    PreviewPrinterSetting pps = new PreviewPrinterSetting();
                    pps.Landscape = true;
                    pps.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    // プレビュー
                    ReportHelper.Preview(this, reports.ToArray(), pps);
                }
                else
                {
                    // 印刷してよろしいですか？ダイアログ
                    if (this.ShowMessage("A9999999035") != DialogResult.OK)
                    {
                        return;
                    }

                    if (genpin != null)
                    {
                        // 現品TAG印刷
                        ReportHelper.Print(LocalSetting.GetTAGPrinter(), genpin);

                        // 印刷時発行日アップデート
                        CondK01 condK01 = new CondK01(this.UserInfo);
                        WsConnection.ConnK01 connK01 = new ConnK01();


                        condK01.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                        condK01.NounyusakiCD = this.txtNonyusakiCD.Text;

                        // 履歴登録用
                        // 便
                        condK01.Ship = this.txtShip.Text;
                        // ARNo.
                        condK01.ARNo = "AR" + this.txtARNo.Text;
                        // 操作区分
                        if (this.rdoIssue.Checked)
                        {
                            // 入力登録
                            condK01.OperationFlag = OPERATION_FLAG.K0100010_ISSUE_VALUE1;
                        }
                        // 更新PC名
                        condK01.UpdatePCName = UtilSystem.GetUserInfo(false).MachineName;
                        // TAGNoリストを渡して発効日のアップデート 
                        connK01.UpdMeisai(condK01, ds.Tables[ComDefine.DTTBL_TAGLIST]);

                        // グリッド更新
                        this.RunSearch();
                    }

                    if (tagList != null)
                    {
                        // TAGリスト印刷
                        ReportHelper.Print(LocalSetting.GetNormalPrinter(), tagList);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }
        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の切替
        /// </summary>
        /// <create>K.Tsutsumi 2012/05/14</create>
        /// <update>J.Chen 2023/08/29 画面起動連携</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {

            // 管理No.
            this.txtNonyusakiCD.Text = string.Empty;

            if (!_isShukkaKeikaku)
            {
                // 納入先名
                this.txtNonyuusaki.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
            }

            // AR No.
            this.txtARNo.Text = string.Empty;
            
            if (this.cboShukkaFlag.SelectedValue == null || this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                // AR No.
                this.txtARNo.Enabled = false;
                // 2015/11/30 H.Tajimi 便を有効にする
                this.txtShip.Enabled = true;
                // ↑
            }
            else
            {
                // ----- AR -----
                // AR No.
                this.txtARNo.Enabled = true;
                // 2015/11/30 H.Tajimi 便を無効にする
                this.txtShip.Enabled = false;
                // ↑
            }
        }

        #endregion

        #region AutoFilter設定

        /// --------------------------------------------------
        /// <summary>
        /// AutoFilter設定
        /// </summary>
        /// <param name="isForceClear">強制的にAutoFilter設定をクリアするかどうか</param>
        /// <create>H.Tajimi 2015/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetEnableAutoFilter(bool isForceClear)
        {
            this.shtMeisai.Redraw = false;

            if (isForceClear)
            {
                this.shtMeisai.ColumnHeaders[SHEET_COL_FLOOR_COL].DropDown = null;
                this.shtMeisai.ColumnHeaders[SHEET_COL_M_NO_COL].DropDown = null;
            }
            else
            {
                // 2行以上データが存在する場合は、AutoFilterを表示
                if (this.shtMeisai.Rows.Count > 1)
                {
                    var headerDropDown = new HeaderDropDown();
                    headerDropDown.EnableAutoFilter = true;
                    headerDropDown.EnableAutoSort = false;  // ソート系の項目は非表示
                    this.shtMeisai.ColumnHeaders[SHEET_COL_FLOOR_COL].DropDown = headerDropDown;
                    this.shtMeisai.ColumnHeaders[SHEET_COL_M_NO_COL].DropDown = headerDropDown;
                }
                else
                {
                    this.shtMeisai.ColumnHeaders[SHEET_COL_FLOOR_COL].DropDown = null;
                    this.shtMeisai.ColumnHeaders[SHEET_COL_M_NO_COL].DropDown = null;
                }
            }

            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region Print列のチェックボックスON/OFF制御用（フィルタリングを考慮）

        /// --------------------------------------------------
        /// <summary>
        /// 全選択・全選択解除時のPrint列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">Print列チェックボックスをONするかどうか</param>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllPrintCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(new GrapeCity.Win.ElTabelle.Range[] { new GrapeCity.Win.ElTabelle.Range(SHEET_COL_INSATU, 0, SHEET_COL_INSATU, this.shtMeisai.MaxRows - 1) });
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択・範囲選択解除時のPrint列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">Print列チェックボックスをONするかどうか</param>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangePrintCheck(bool isChecked)
        {
            var ranges = this.GetValidRanges(this.shtMeisai.GetBlocks(GrapeCity.Win.ElTabelle.BlocksType.Selection));
            this.SetCellRangeValueForCheckBox(ranges, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeのチェックボックス値設定
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <param name="isCheck">チェックボックスをONするかどうか</param>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetCellRangeValueForCheckBox(GrapeCity.Win.ElTabelle.Range[] ranges, bool isChecked)
        {
            foreach (var range in ranges)
            {
                this.shtMeisai.CellRange = range;
                this.shtMeisai.CellValue = isChecked ? CHECK_ON : CHECK_OFF;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeで有効となっているRange配列を取得
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private GrapeCity.Win.ElTabelle.Range[] GetValidRanges(GrapeCity.Win.ElTabelle.Range[] ranges)
        {
            var lstRanges = new List<GrapeCity.Win.ElTabelle.Range>();
            foreach (var range in ranges)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    if (IsValidRow(rowIndex))
                    {
                        lstRanges.Add(new Range(0, rowIndex, 0, rowIndex));
                    }
                }
            }
            return lstRanges.ToArray();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定行が有効かどうか
        /// </summary>
        /// <remarks>
        /// El TablledのAutoFilterは行の高さを0にすることでフィルタリングを
        /// 実現しているので、行の高さでフィルタリングされているかどうか判定
        /// </remarks>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsValidRow(int rowIndex)
        {
            if (this.shtMeisai.MaxRows - 1 < rowIndex)
            {
                return false;
            }

            if (this.shtMeisai.Rows[rowIndex].Height < 1)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 画面表示

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <param name="isSearch">検索を行うかどうか</param>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran()
        {
            using (P02.Forms.NonyusakiIchiran frm = new SMS.P02.Forms.NonyusakiIchiran(base.UserInfo, this.cboShukkaFlag.SelectedIndex.ToString(), this.txtNonyuusaki.Text, this.txtShip.Text, true))
            {
                var ret = frm.ShowDialog(this);
                if (ret == DialogResult.OK)
                {
                    this.ClearMessage();
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;

                    //LOCK_FLAGの取得
                    var lockFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LOCK_FLAG);
                    if (lockFlag == "1")
                    {
                        this.rdoLock.Checked = true;
                    }
                    else if (lockFlag == "0" || lockFlag == "")
                    {
                        this.rdoUnLock.Checked = true;
                    }

                    // 選択データを設定
                    this.txtNonyusakiCD.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    this.txtNonyuusaki.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    this.txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// 表示時のEnabled切替
        /// </summary>
        /// <param name="isView"></param>
        /// <create>H.Tajimi 2015/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeEnableViewMode(bool isView)
        {
            // 表示コントロールのロック/ロック解除
            this.btnAllCheck.Enabled = isView;
            this.btnAllNotCheck.Enabled = isView;
            this.btnRangeCheck.Enabled = isView;
            this.btnRangeNotCheck.Enabled = isView;
            if (!isView)
            {
                this.cboIssueSelect.SelectedValue = HAKKOU_SELECT.DEFAULT_VALUE1;
                this.cboIssueSelect.Enabled = isView;
                this.nudPrintPos.Value = 1;
                this.nudPrintPos.Enabled = isView;
            }
            // 検索条件のロック/ロック解除
            this.grpMode.Enabled = !isView;
            this.grpSearch.Enabled = !isView;
            this.pnlSeach.Enabled = !isView;
            this.grpKeyAction.Enabled = isView;
            // ファンクションボタン制御
            this.fbrFunction.F01Button.Enabled = isView;
            this.fbrFunction.F02Button.Enabled = isView;
            this.fbrFunction.F06Button.Enabled = isView;
        }

        #endregion

        #region ロック/解除更新処理

        /// --------------------------------------------------
        /// <summary>
        /// ロック/解除更新処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/06/30</create>
        /// <update></update>
        /// --------------------------------------------------

        private void LockUnLock(bool ret)
        {
            string messageId = ret ? "K0100010008" : "K0100010009";
            // ロックor解除の確認メッセージ
            if (this.ShowMessage(messageId) != DialogResult.OK)
            {
                this.grpKeyAction.Enabled = false;
                this.rdoLock.Checked = !ret;
                this.rdoUnLock.Checked = ret;
                this.grpKeyAction.Enabled = true;
                return;
            }
            try
            {
                ConnK01 conn = new ConnK01();
                CondK01 cond = new CondK01(this.UserInfo);
                cond.NounyusakiCD = this.txtNonyusakiCD.Text;
                cond.NonyusakiName = this.txtNonyuusaki.Text;
                cond.Ship = this.txtShip.Text;
                conn.LockUnLock(cond, ret);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            this.ShowMessage("FW010040004");
        }

        #endregion

        #region TAG登録ロック選択時

        /// --------------------------------------------------
        /// <summary>
        /// TAG登録ロック選択時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private void grpKeyAction_CheckedChanged(object sender, EventArgs e)
        {
            if (this.grpKeyAction.Enabled == true)
            {
                try
                {
                    this.ClearMessage();
                    bool ret = this.rdoLock.Checked;
                    LockUnLock(ret);
                }
                catch (Exception ex)
                {
                    CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                }
            }
        }

        #endregion

        #region 発行可能チェック

        /// --------------------------------------------------
        /// <summary>
        /// 発行可能チェック
        /// </summary>
        /// <param name="dt">印刷用明細データ</param>
        /// <returns></returns>
        /// <create>J.Chen 2024/08/08</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsCanPrint(DataTable dt)
        {
            // 手配連携Noのある有償データに、PONoなし（未受注）の場合、発行できないとする
            foreach (DataRow dr in dt.Rows)
            {
                string tehairenkeiNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO);
                string poNo = ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PO_NO);
                string estimateFlag = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG);
                string rowNumber = ComFunc.GetFld(dr, ComDefine.FLD_CNT);

                if (!string.IsNullOrEmpty(tehairenkeiNo) && estimateFlag == ESTIMATE_FLAG.ONEROUS_VALUE1)
                {
                    if (string.IsNullOrEmpty(poNo))
                    {
                        this.ShowMessage("K0100010010", rowNumber);
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
