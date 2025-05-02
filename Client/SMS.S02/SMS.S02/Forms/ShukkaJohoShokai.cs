using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

using WsConnection.WebRefS02;
using SMS.P02.Forms;
using SMS.E01;
using SMS.S02.Properties;
using WsConnection.WebRefAttachFile;


namespace SMS.S02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷情報照会
    /// </summary>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaJohoShokai : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Higuchi 2010/09/22</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>K.Tsutsumi 2019/03/08 手配情報表示</update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 7;

        /// --------------------------------------------------
        /// <summary>
        /// 列番号
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/20</create>
        /// <update>K.Tsutsumi 2019/03/08 手配情報表示</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// <update>K.Tsutsumi 2019/09/07 写真表示</update>
        /// --------------------------------------------------
        private const int SHEET_COL_AR_NO = 25;
        private const int SHEET_COL_TEHAI_RENKEI_NO = 43;
        private const int SHEET_COL_SHUKKA_FLAG = 44;
        private const int SHEET_COL_TAG_NONYUSAKI_CD = 45;
        private const int SHEET_COL_BUKKEN_NO = 46;

        private const int SHEET_COL_FILE_NAME1 = 51;
        private const int SHEET_COL_SAVE_DIR1 = 52;
        private const int SHEET_COL_FILE_NAME2 = 53;
        private const int SHEET_COL_SAVE_DIR2 = 54;

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
        /// 一覧選択で選択した納入先コード
        /// </summary>
        /// <create>T.Sakiori 2012/04/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _nonyusakiCd = string.Empty;
        /// --------------------------------------------------
        /// <summary>
        /// 一覧選択で選択した納入先コード（複数）
        /// </summary>
        /// <create>H.Tajimi 2015/12/04</create>
        /// <update></update>
        /// --------------------------------------------------
        private string[] _nonyusakiCds = null;

        //【Step_1_2】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
        //　単価（JPY）列について、表示しない様にしたか否かのフラグ。
        //　Excel出力時に利用する.
        protected bool _flgUnitPriceDisable = false;
        /// --------------------------------------------------
        /// <summary>
        /// データ絞込用クラス
        /// </summary>
        /// <create>T.SASAYAMA 2023/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataFilter _filter = new DataFilter();
        /// --------------------------------------------------
        /// <summary>
        /// アイドル発生待ちになったかどうか
        /// </summary>
        /// <create>T.SASAYAMA 2023/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _idleStart = false;


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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaJohoShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>K.Tsutsumi 2018/01/20 ファイル名列追加</update>
        /// <update>K.Tsutsumi 2019/03/08 手配情報表示</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// <update>H.Iimuro 2022/10/04 図面/型式２の並び替え対応</update>
        /// <update>J.Chen 2022/12/19 TAG便名追加</update>
        /// <update>J.Chen 2024/11/07 通関確認状態追加（STEP17）</update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // 画面の編集モード変更
                this.EditMode = SystemBase.EditMode.View;
                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトルを設定
                // 列移動の際は、デザイン側も合わせて修正すること!!
                int row = 0;
                // 0
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_State;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ShipARNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SetteiDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_NouhinSaki;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SyukkaSaki;
                // 5
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_TagNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ProductNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Code;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Area;
                // 10
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Floor;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Model;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_MNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_JpName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Name;
                // 15
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_DrawingNoFormat;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ZumenKeishiki2;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SectioningNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Quantity;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Free1;
                // 20
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Free2;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Maker;

                //【Step15】出荷情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                //
                //　単価（JPY）列について、以下のユーザは参照できない様、制御する.
                //　　　以下の権限（M_ROLE）を想定
                //　　　　管理部User        ROLE_ID=008 土山倉庫
                //　　　　パートナー管理者  ROLE_ID=012 パートナー管理者
                //　　　　梱包管理者	001 梱包管理者
                //　　　　木枠梱包User	002 木枠梱包USER
                //　　　　一般User	003 一般USER	
                //　　　　工事現場User	005 FE
                //　　　　パートナー管理者012 パートナー管理者
                //
                //　　　メニュー識別ID: S02　　 （手配）
                //　    メニュー項目ID: S0290030（手配情報照会の4桁目 '9' で決め打ち）
                if (SystemBase.MenuAuthorityCheck.ExistsRoleAndRolemap(this.UserInfo, "S02", "S0290030") == true)
                {
                    shtMeisai.Columns[row].DataField = "";
                    this._flgUnitPriceDisable = true;                //フラグをONにしておく（Excel出力時に参照）
                }
                //---修正ここまで

                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_UnitPrice;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_Memo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_CustomsStatus;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_STNo;
                // 25
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ARNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_AssemblyDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_BoxNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_PalletNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_WoodFrameNo;
                // 30
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_BoxPackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_PalletPackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_WoodFramePackingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ShippingDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ShippingCompany;
                // 35
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_InvoiceNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_OkurijyoNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_BLNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_AcceptanceDate;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_AcceptanceStaff;
                // 40
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_GWRT;
                shtMeisai.ColumnHeaders[row].Caption = Resources.ShukkaJohoShokai_Picture;
                shtMeisai.Columns[row].AlignHorizontal = AlignHorizontal.Center;
                shtMeisai.Columns[row].AlignVertical = AlignVertical.Middle;
                row++;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_JyotaiFlag;                  // JYOTAI_FLAG(HIDE)
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_TehaiRenkeiNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_ShukkaFlag;
                // 45 
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_TagNonyusakiCd;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_BukkenNo;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_EcsQuota;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_TehaiFlagName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_QuantityUnitName;
                // 50
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_EstimateFlagName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_FileName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SaveDir;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_FileName2;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SaveDir2;

                // 55
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_SipFromName;
                shtMeisai.ColumnHeaders[row++].Caption = Resources.ShukkaJohoShokai_TagShip;

                // コンボボックスの初期化
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);
                this.MakeCmbBox(this.cboDispSelect, DISP_SELECT.GROUPCD);

                // 納入先をまたがって検索するようになったのでとりあえず非表示
                this.lblNonyusakiCD.Visible = false;
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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.cboShukkaFlag.Focus();
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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>K.Tsutsumi 2012/05/15</update>
        /// <update>H.Tajimi 2015/12/07 出荷情報/複数便を選択し表示対応</update>
        /// <update>K.Tsutsumi 2019/08/07 手配連携No.が「All Clear」でクリアされない不具合対応</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ----- クリア -----
                // グリッドのクリア
                this.SheetClear();
                // 出荷区分
                if (0 < this.cboShukkaFlag.Items.Count)
                {
                    this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboShukkaFlag.SelectedIndex = -1;
                }
                // 納入先
                this.txtNonyusakiName.Text = string.Empty;
                // 便
                this.txtShip.Text = string.Empty;
                // TAG便
                this.txtTagShip.Text = string.Empty;
                // AR No.
                this.txtARNo.Text = string.Empty;
                // 品名(和文)
                this.txtHinmeiJp.Text = string.Empty;
                // 品名
                this.txtHinmei.Text = string.Empty;
                // 図番/形式
                this.txtZumenKeishiki.Text = string.Empty;
                // 2012/05/15 K.Tsutsumi Change 納入先は必ず選択してもらう
                //// ↓ 2010/10/28 Y.Higuchi Delete Start
                //// 納入先をまたがって検索するのでとりあえずコメントアウト
                ////// 管理No.
                ////this.txtNonyusakiCD.Text = string.Empty;
                //// ↑ 2010/10/28 Y.Higuchi Delete End
                //管理No.
                this.txtNonyusakiCD.Text = string.Empty;
                // ↑

                // 表示選択
                if (0 < this.cboDispSelect.Items.Count)
                {
                    this.cboDispSelect.SelectedValue = DISP_SELECT.DEFAULT_VALUE1;
                }
                else
                {
                    this.cboDispSelect.SelectedIndex = -1;
                }

                // 2011/03/09 K.Tsutsumi Add T_ARが存在しなくても続行可能
                this._isConfirmNoAR = false;
                this._beforeShukkaFlag = string.Empty;
                this._beforeNonyusakiName = string.Empty;
                this._beforeARNo = string.Empty;
                // ↑

                // 2011/03/09 K.Tsutsumi Add 進捗件数取得
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";
                // ↑

                // 2012/05/15 K.Tsutsumi Add 絞込条件
                this.txtTagNo.Text = string.Empty;
                this.txtBoxNo.Text = string.Empty;
                this.txtPalletNo.Text = string.Empty;
                this.txtKiwakuNo.Text = string.Empty;
                this.txtCaseNo.Text = string.Empty;
                // ↑

                // 2015/12/07 H.Tajimi 複数便対応
                this.chkAllShip.Checked = false;
                this.txtDispShip.Text = string.Empty;
                // ↑

                this.txtRenkeiNo.Text = string.Empty;

                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // ----- 検索用入力チェック -----
                // 納入先チェック
                if (string.IsNullOrEmpty(this.txtNonyusakiName.Text))
                {
                    // 納入先一覧から納入先を選択して下さい。
                    this.ShowMessage("S0100020041");
                    this.txtNonyusakiName.Focus();
                    return false;
                }
                // ↓ 2010/10/28 Y.Higuchi Delete Start
                // 便、AR No.が必須じゃなくなるので、とりあえずコメントアウト
                //// 便入力チェック
                //if (this.txtShip.Enabled && string.IsNullOrEmpty(this.txtShip.Text))
                //{
                //    // 便を入力して下さい。
                //    this.ShowMessage("A9999999017");
                //    this.txtShip.Focus();
                //    return false;
                //}
                //// AR No.チェック
                //if (this.txtARNo.Enabled && string.IsNullOrEmpty(this.txtARNo.Text))
                //{
                //    // AR No.を入力してください。
                //    this.ShowMessage("A9999999018");
                //    this.txtARNo.Focus();
                //    return false;
                //}
                // ↑ 2010/10/28 Y.Higuchi Delete End
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
        /// <create>Y.Higuchi 2010/08/09</create>
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
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>H.Tajimi 2015/12/07 出荷情報/複数便を選択し表示対応</update>
        /// <update>T.Okuda 2023/07/14 表示選択修正</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS02 cond = new CondS02(this.UserInfo, ComDefine.BUTTON_TEXT_KONPOZUMI, ComDefine.BUTTON_TEXT_SHUKKA, ComDefine.BUTTON_TEXT_DETAIL);
                ConnS02 conn = new ConnS02();

                // 出荷区分が本体の場合TAG便の設定
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    cond.TagShip = this.txtTagShip.Text;
                }

                // コンディションの設定
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();

                // 本体かつ全便対象の場合null
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1 &&
                    chkAllShip.Checked)
                {
                    // 2015/12/07 H.Tajimi 複数便対応
                    cond.NonyusakiCD = null;
                    cond.NonyusakiCDs = null;
                    // ↑
                }
                else
                {
                    // 2015/12/07 H.Tajimi 複数便対応
                    cond.NonyusakiCD = this._nonyusakiCd;
                    cond.NonyusakiCDs = this._nonyusakiCds;
                    // ↑
                }

                // ↓ 2010/10/28 Y.Higuchi Delete Start
                // 納入先をまたがって検索するのでとりあえずコメントアウト
                //if (!string.IsNullOrEmpty(this.txtNonyusakiCD.Text))
                //{
                //    cond.NonyusakiCD = this.txtNonyusakiCD.Text;
                //}
				// ↑ 2010/10/28 Y.Higuchi Delete End
				cond.NonyusakiName = this.txtNonyusakiName.Text;
                if (!string.IsNullOrEmpty(this.txtARNo.Text))
                {
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                }
                cond.HinmeiJp = this.txtHinmeiJp.Text;
                cond.Hinmei = this.txtHinmei.Text;
                cond.ZumenKeishiki = this.txtZumenKeishiki.Text;
                //if (this.cboDispSelect.Enabled)
                //{
                //    cond.DispSelect = this.cboDispSelect.SelectedValue.ToString();
                //}
                cond.KiwakuNo = this.txtKiwakuNo.Text;
                cond.CaseNo = this.txtCaseNo.Text;
                if (!string.IsNullOrEmpty(this.txtBoxNo.Text))
                {
                    cond.BoxNo = this.lblBox.Text + this.txtBoxNo.Text.PadLeft(5, '0');
                }
                if (!string.IsNullOrEmpty(this.txtPalletNo.Text))
                {
                    cond.PalletNo = this.lblPallet.Text + this.txtPalletNo.Text.PadLeft(5, '0');
                }
                cond.TagNo = this.txtTagNo.Text;

                cond.TehaiRenkeiNo = this.txtRenkeiNo.Text;

                // 出荷明細データ取得
                string errMsgID;
                string[] args;

                // 2011/03/09 K.Tsutsumi Add 進捗件数取得
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";
                // ↑

                DataSet ds = conn.GetShukkaMeisai(cond, out errMsgID, out args);

                // 2011/03/09 K.Tsutsumi Add 進捗件数欄追加
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_PROGRESS) == true)
                {
                    DataTable dt = ds.Tables[ComDefine.DTTBL_PROGRESS];

                    this.txtShuka.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_SHUKA).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtBoxKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_BOXKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtPalletKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_PALLETKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                    this.txtKiwakuKonpo.Text = ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_KIWAKUKONPO).ToString("###,##0") + "/" + ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_COUNT_ALL).ToString("###,##0");
                }
                // ↑

                // 2011/03/09 K.Tsutsumi Add T_ARが存在しなくても続行可能
                if (string.IsNullOrEmpty(errMsgID) == false)
                {
                    this.ShowMessage(errMsgID, args);
                }
                else if ((string.IsNullOrEmpty(errMsgID) == true) && (string.IsNullOrEmpty(cond.ARNo) == false))
                {
                    // T_ARは取得できたか？
                    if (ComFunc.IsExistsData(ds, Def_T_AR.Name) == false)
                    {
                        // 前回と検索条件が違うか？
                        if ((this._beforeShukkaFlag != cond.ShukkaFlag) || (this._beforeNonyusakiName != cond.NonyusakiName) || (this._beforeARNo != cond.ARNo))
                        {
                            // 確認させる
                            this._isConfirmNoAR = false;
                        }

                        if (this._isConfirmNoAR == false)
                        {
                            // 問い合わせを行うときの検索条件待避
                            this._beforeShukkaFlag = cond.ShukkaFlag;
                            this._beforeNonyusakiName = cond.NonyusakiName;
                            this._beforeARNo = cond.ARNo;

                            // AR No.[{0}]のAR情報が登録されていません。\r\nこのまま処理を続けますか？
                            if (this.ShowMessage("K0100010005", this.txtARNo.Text) != DialogResult.OK)
                            {
                                return false;
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

                if (!string.IsNullOrEmpty(errMsgID))
                {
                    // 2011/03/09 K.Tsutsumi Delete T_ARが存在しなくても続行可能
                    //this.ShowMessage(errMsgID, args);
                    // ↑
                    return false;
                }
                this.shtMeisai.Redraw = false;
                this.shtMeisai.DataSource = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
                this.shtMeisai.Enabled = true;
                this.CallIdle();

                // ↓ 2010/10/28 Y.Higuchi Delete Start
                // 納入先をまたがって検索するのでとりあえずコメントアウト
                // 納入先コードを再設定
                //this.txtNonyusakiCD.Text = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                // ↑ 2010/10/28 Y.Higuchi Delete End

                // 2015/12/07 H.Tajimi 複数便対応
                // 便の重複を除いて表示対象便を作成
                if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    var ships = ds.Tables[Def_T_SHUKKA_MEISAI.Name].AsEnumerable().Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.SHIP)).Distinct().ToArray();
                    this.txtDispShip.Text = string.Join(", ", ships);
                }
                // ↑

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

        #region イベント

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/09</create>
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
                // グリッド
                this.SheetClear();
                // フラグ
                this._isConfirmNoAR = false;
                this._beforeShukkaFlag = string.Empty;
                this._beforeNonyusakiName = string.Empty;
                this._beforeARNo = string.Empty;
                // 進捗件数
                this.txtShuka.Text = "0/0";
                this.txtBoxKonpo.Text = "0/0";
                this.txtPalletKonpo.Text = "0/0";
                this.txtKiwakuKonpo.Text = "0/0";

                this.txtDispShip.Text = string.Empty;

                // ファンクションボタンEnabled切替
                this.ChangeFunctionButton(false);
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
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F10ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
        /// <update>K.Tsutsumi 2019/03/08 手配情報を追加でEXCEL出力</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    frm.Title = Resources.ShukkaJohoShokai_sfdExcel_Title;
                    frm.Filter = Resources.ShukkaJohoShokai_sfdExcel_Filter;
                    frm.FileName = ComDefine.EXCEL_FILE_SHUKKA_MEISAI;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();
                    ExportShukkaMeisai export = new ExportShukkaMeisai();
                    string msgID;
                    string[] args;

                    //【Step_1_2】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
                    //　単価（JPY）列の出力をフラグで切り替える.
                    export.IsUnitPriceDisable = _flgUnitPriceDisable;

                    export.ExportExcelwithProcurement(frm.FileName, dtExport, out msgID, out args);
                    if (!string.IsNullOrEmpty(msgID))
                    {
                        this.ShowMessage(msgID, args);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F11ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2018/01/20</create>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F11Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F11Button_Click(sender, e);
            try
            {
                // 検索結果が存在しない場合は処理を抜ける。
                if (this.shtMeisai.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.shtMeisai.Focus();
                    return;
                }

                // カーソル位置の行取得
                var row =  this.shtMeisai.ActivePosition.Row;
                // 写真がアップロードされていない場合は処理を抜ける。
                var fileName1 = this.shtMeisai[SHEET_COL_FILE_NAME1, row].Text;
                var fileName2 = this.shtMeisai[SHEET_COL_FILE_NAME2, row].Text;
                if (string.IsNullOrEmpty(fileName1) && string.IsNullOrEmpty(fileName2))
                {
                    // {0}行目の写真は登録されていません。
                    this.ShowMessage("S0200030008", new string[] { (row + 1).ToString() });
                    this.shtMeisai.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(fileName1))
                {
                    // ダウンロード1
                    var saveDir = this.shtMeisai[SHEET_COL_SAVE_DIR1, row].Text;
                    if (!this.FileDownload(saveDir, fileName1))
                    {
                        // 写真のダウンロードに失敗しました。
                        this.ShowMessage("S0200030009");
                        this.shtMeisai.Focus();
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(fileName2))
                {
                    // ダウンロード2
                    var saveDir = this.shtMeisai[SHEET_COL_SAVE_DIR1, row].Text;
                    if (!this.FileDownload(saveDir, fileName2))
                    {
                        // 写真のダウンロードに失敗しました。
                        this.ShowMessage("S0200030009");
                        this.shtMeisai.Focus();
                        return;
                    }
                }
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
        /// <create>Y.Higuchi 2010/08/09</create>
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

        #region 表示ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // グリッドクリア
                this.SheetClear();
                // 2015/11/30 H.Tajimi 納入先一覧画面を表示
                if (!this.ShowNonyusakiIchiran())
                {
                    // フォーカスのセット
                    this.txtNonyusakiName.Focus();
                    return;
                }
                // ↑
                // ファンクションボタンの切替
                this.ChangeFunctionButton(true);
                // フォーカス
                this.shtMeisai.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 出荷区分

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックスSelectedIndexChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeShukkaFlag();
        }

        #endregion

        #endregion

        #region 出荷区分切替

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分の切替
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>Y.Higuchi 2010/10/28</update>
        /// <update>K.Tsutsumi 2012/05/15</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeShukkaFlag()
        {

            // 管理No.
            this.txtNonyusakiCD.Text = string.Empty;
            // 納入先名
            this.txtNonyusakiName.Text = string.Empty;
            // 便
            this.txtShip.Text = string.Empty;
            // TAG便
            this.txtTagShip.Text = string.Empty;
            // ARNo.
            this.txtARNo.Text = string.Empty;
            
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // ----- 本体 -----
                // 全便対象
                this.chkAllShip.Enabled  = true;
                // AR No.
                this.txtARNo.Enabled = false;
                // 元TAG便
                this.txtTagShip.Enabled = true;
            }
            else
            {
                // ----- AR -----
                // 全便対象
                this.chkAllShip.Enabled = false;
                // AR No.
                this.txtARNo.Enabled = true;
                // 元TAG便
                this.txtTagShip.Enabled = false;
            }
        }

        #endregion

        #region モード切り替え操作

        /// --------------------------------------------------
        /// <summary>
        /// ファンクションバーのEnabled切替
        /// </summary>
        /// <param name="isEnabled">Enabled状態</param>
        /// <create>Y.Higuchi 2010/08/10</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeFunctionButton(bool isEnabled)
        {
            // ----- 照会 -----
            // 2015/12/02 H.Tajimi Clearボタン
            this.fbrFunction.F06Button.Enabled = isEnabled;
            // ↑
            this.fbrFunction.F10Button.Enabled = isEnabled;
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/08/10</create>
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

        #region 画面表示

        #region 納入先一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 納入先一覧画面表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowNonyusakiIchiran()
        {
            string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            string nonyusakiName = this.txtNonyusakiName.Text;
            string ship = this.txtShip.Text;
            string tagShip = this.txtTagShip.Text;
            if (this.cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                using (NonyusakiIchiranEx frm = new NonyusakiIchiranEx(this.UserInfo, shukkaFlag, nonyusakiName, ship, tagShip, string.Empty, true, this.chkAllShip.Checked))
                {
                    var ret = frm.ShowDialog(this);
                    if (ret == DialogResult.OK)
                    {
                        this.ClearMessage();
                        var drCollection = frm.SelectedRowsData;
                        if (drCollection == null) return false;
                        // 選択データを設定
                        this.txtNonyusakiName.Text = drCollection.Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.NONYUSAKI_NAME)).FirstOrDefault();
                        this._nonyusakiCd = null;
                        this._nonyusakiCds = drCollection.Select(x => ComFunc.GetFld(x, Def_M_NONYUSAKI.NONYUSAKI_CD)).ToArray();
                        return this.RunSearch();
                    }
                }
            }
            else
            {
                using (NonyusakiIchiran frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, ship, true))
                {
                    var ret = frm.ShowDialog(this);
                    if (ret == DialogResult.OK)
                    {
                        this.ClearMessage();
                        var dr = frm.SelectedRowData;
                        if (dr == null) return false;
                        // 選択データを設定
                        this.txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                        this._nonyusakiCd = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                        this._nonyusakiCds = null;
                        return this.RunSearch();
                    }
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region 写真表示

        /// --------------------------------------------------
        /// <summary>
        /// 写真表示処理
        /// </summary>
        /// <param name="saveDir">格納フォルダ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>OK/NG</returns>
        /// <remarks>出荷区分が本体の場合は、AR No.はstring.emptyでOK</remarks>
        /// <create>K.Tsutsumi 2018/01/20</create>
        /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
        /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
        /// --------------------------------------------------
        private bool FileDownload(string saveDir, string fileName)
        {
            try
            {
                ConnAttachFile conn = new ConnAttachFile();
                FileDownloadPackage package = new FileDownloadPackage();

                package.FileName = fileName;
                package.FileType = FileType.TagPictures;
                package.FolderName = saveDir;

                FileDownloadResult result = conn.FileDownload(package);
                if (!result.IsExistsFile)
                {
                    return false;
                }

                if (!Directory.Exists(ComDefine.DOWNLOAD_DIR))
                {
                    Directory.CreateDirectory(ComDefine.DOWNLOAD_DIR);
                }
                var dir = Path.Combine(ComDefine.DOWNLOAD_DIR, saveDir);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                //var downloadedFileName = Path.Combine(ComDefine.DOWNLOAD_DIR, shukkaFlag + nonyusakiCd + fileName);
                var downloadedFileName = Path.Combine(dir, fileName);
                if (File.Exists(downloadedFileName))
                {
                    // 同一ファイルが存在する場合は削除する
                    File.Delete(downloadedFileName);
                }
                using (FileStream fs = new FileStream(downloadedFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(result.FileData, 0, result.FileData.Length);
                    fs.Close();
                }
                // ファイルを関連付けられたアプリケーションで開く
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = downloadedFileName;
                    p.Start();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region 表示選択コンボボックスによるデータの絞込
        /// --------------------------------------------------
        /// <summary>
        /// 表示選択コンボボックスの値が変わった時の処理
        /// </summary>
        /// <create>T.SASAYAMA 2023/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ComboBox_Filter(object sender, EventArgs e)
        {
            try
            {
                this.CallIdle();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// idleStartフラグによる実行管理
        /// </summary>
        /// <create>T.SASAYAMA 2023/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void CallIdle()
        {
            if (this._idleStart)
            {
                return;
            }
            this._idleStart = true;
            Application.Idle += new EventHandler(Application_Idle);
        }

        /// --------------------------------------------------
        /// <summary>
        /// shtMeisaiのフィルター処理実行部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/06/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void Application_Idle(object sender, EventArgs e)
        {
            Sheet sheet = this.shtMeisai;

            try
            {
                this._idleStart = false;
                Application.Idle -= new EventHandler(Application_Idle);

                sheet.Redraw = false;

                if (!string.IsNullOrEmpty(this.txtNonyusakiName.Text) && !string.IsNullOrEmpty(this.txtDispShip.Text))
                {
                    if (!UtilData.ExistsData(sheet.DataSource as DataTable))
                    {
                        return;
                    }

                    if (this.cboDispSelect.SelectedValue.ToString() == DISP_SELECT.MISHUKA_VALUE1)
                    {
                        List<string> list = new List<string>();
                        if (UserInfo.Language == ComDefine.LANG_JP)
                        {
                            list.Add(ComDefine.SHINKI_JP_NAME);
                            list.Add(ComDefine.TAGHAKKOZUMI_JP_NAME);
                            list.Add(ComDefine.HIKIWATASHIZUMI_JP_NAME);
                        }
                        else if (UserInfo.Language == ComDefine.LANG_US)
                        {
                            list.Add(ComDefine.SHINKI_JP_NAME);
                            list.Add(ComDefine.TAGHAKKOZUMI_US_NAME);
                            list.Add(ComDefine.HIKIWATASHIZUMI_US_NAME);
                        }
                        this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_JYOTAI_NAME, list, true);
                    }
                    else if (this.cboDispSelect.SelectedValue.ToString() == DISP_SELECT.SHUKKAZUMI_VALUE1)
                    {
                        List<string> list = new List<string>();
                        if (UserInfo.Language == ComDefine.LANG_JP)
                        {
                            list.Add(ComDefine.SHUKKAZUMI_JP_NAME);
                            list.Add(ComDefine.UKEIREZUMI_JP_NAME);
                        }
                        else if (UserInfo.Language == ComDefine.LANG_US)
                        {
                            list.Add(ComDefine.SHUKKAZUMI_US_NAME);
                            list.Add(ComDefine.UKEIREZUMI_US_NAME);
                        }
                        this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_JYOTAI_NAME, list, true);
                    }
                    else
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, ComDefine.FLD_JYOTAI_NAME, this.cboDispSelect, true);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                sheet.Redraw = true;
            }
        }
        #endregion
    }
}
