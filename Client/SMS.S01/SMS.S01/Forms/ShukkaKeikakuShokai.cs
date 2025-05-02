using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
using XlsCreatorHelper;

using WsConnection.WebRefS01;
using SMS.P02.Forms;
using SMS.E01;
using XlsxCreatorHelper;
using SMS.S01.Properties;
using SystemBase.Forms;
using SMS.K01.Forms;

using GrapeCity.Win.ElTabelle.Editors;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画照会
    /// </summary>
    /// <create>J.Chen 2023/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShukkaKeikakuShokai : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 処理フラグ（削除）
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int DeletedFlag = 9;

        /// --------------------------------------------------
        /// <summary>
        /// 出荷済み
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int ShukkazumiFlag = 6;

        /// --------------------------------------------------
        /// <summary>
        /// 引渡済み
        /// </summary>
        /// <create>J.Chen 2023/09/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int HikiwatashiFlag = 8;

        #endregion

        #region Fields

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

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 列インデックス
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/03 現場用ステータス更新欄追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private enum SHEET_COL
        {
            BUKKEN_NAME = 0,        // 物件名
            SHIP_SEIBAN,            // 製番
            SHIP,                   // 便名
            ESTIMATE_FLAG,          // 有償/無償
            TRANSPORT_FLAG,         // AIR/SHIP
            SHUKKA_FROM,            // 出荷元
            SHUKKA_TO,              // 出荷先
            SHUKKA_DATE,            // 出荷日
            SEIBAN,                 // 製番
            KISHU,                  // 機種
            NAIYO,                  // 内容
            TOUCHAKU_YOTEI_DATE,    // 到着予定日
            KIKAI_PARTS,            // 機械Parts
            SEIGYO_PARTS,           // 制御Parts
            BIKO,                   // 備考
            TAG_NUM,                // TAG発行状況
            TAG_STATUS,             // ステータス
            GENBA_YO_STATUS,        // 現場用状態
            GENBA_YO_BUTSURYO,      // 現場用物量
            SHORI_FLAG,             // 処理フラグ
            MIN_JYOTAI_FLAG,        // 出荷フラグ最低値
            CONSIGN_CD,             // 荷受先CD
        }

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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShukkaKeikakuShokai(UserInfo userInfo, string menuCategoryID, string menuItemID, string title, TagRenkeiData TagRenkeiData)
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/03 現場用ステータス更新欄追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // 物件名コンボボックスの初期化
                this.InitializeComboBukken();

                // 出荷元コンボボックスの初期化
                this.InitializeComboShukkamoto();

                // 出荷先コンボボックスの初期化
                this.InitializeComboShukkasaki();

                // 荷受先コンボボックスの初期化
                this.InitializeComboNiukesaki();

                // 出荷日の初期化
                this.InitializeComboShukkaDate();

                // 製番コンボボックスの初期化
                this.InitializeComboSeiban();

                // 機種コンボボックスの初期化
                this.InitializeComboKishu();

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);
                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[(int)SHEET_COL.BUKKEN_NAME].Caption = Resources.ShukkaKeikakuShokai_BukkenName;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHIP_SEIBAN].Caption = Resources.ShukkaKeikakuShokai_ShipSeiban;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHIP].Caption = Resources.ShukkaKeikakuShokai_Ship;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.ESTIMATE_FLAG].Caption = Resources.ShukkaKeikakuShokai_EstimateFlag;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.TRANSPORT_FLAG].Caption = Resources.ShukkaKeikakuShokai_TransportFlag;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHUKKA_FROM].Caption = Resources.ShukkaKeikakuShokai_ShukkaFrom;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHUKKA_TO].Caption = Resources.ShukkaKeikakuShokai_ShukkaTo;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHUKKA_DATE].Caption = Resources.ShukkaKeikakuShokai_ShukkaDate;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SEIBAN].Caption = Resources.ShukkaKeikakuShokai_Seiban;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.KISHU].Caption = Resources.ShukkaKeikakuShokai_KiShu;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.NAIYO].Caption = Resources.ShukkaKeikakuShokai_Naiyo;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.TOUCHAKU_YOTEI_DATE].Caption = Resources.ShukkaKeikakuShokai_TouchakuyoteiDate;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.KIKAI_PARTS].Caption = Resources.ShukkaKeikakuShokai_KikaiParts;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SEIGYO_PARTS].Caption = Resources.ShukkaKeikakuShokai_SeigyoParts;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.BIKO].Caption = Resources.ShukkaKeikakuShokai_Biko;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.TAG_NUM].Caption = Resources.ShukkaKeikakuShokai_TagNum;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.TAG_STATUS].Caption = Resources.ShukkaKeikakuShokai_TagStatus;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.GENBA_YO_STATUS].Caption = Resources.ShukkaKeikakuShokai_GenbayoStatus;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.GENBA_YO_BUTSURYO].Caption = Resources.ShukkaKeikakuShokai_GenbayoButsuryo;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.SHORI_FLAG].Caption = Resources.ShukkaKeikakuShokai_ShoriFlag;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.MIN_JYOTAI_FLAG].Caption = Resources.ShukkaKeikakuShokai_Min_JyotaiFlag;
                shtMeisai.ColumnHeaders[(int)SHEET_COL.CONSIGN_CD].Caption = Resources.ShukkaKeikakuShokai_ConsignCD;

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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region コンボボックスの初期化

        #region 物件名コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 物件名コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboBukken()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetBukkenName();
            if (!UtilData.ExistsData(ds, Def_M_BUKKEN.Name))
            {
                return;
            }

            // 物件名コンボボックスの初期化
            var dt = ds.Tables[Def_M_BUKKEN.Name];

            // 全ての追加
            var dr = dt.NewRow();
            dr[Def_M_BUKKEN.BUKKEN_NO] = ComDefine.COMBO_ALL_VALUE_DECIMAL;
            dr[Def_M_BUKKEN.BUKKEN_NAME] = ComDefine.COMBO_ALL_DISP;
            dt.Rows.InsertAt(dr, 0);

            this.cboBukkenName.ValueMember = Def_M_BUKKEN.BUKKEN_NO;
            this.cboBukkenName.DisplayMember = Def_M_BUKKEN.BUKKEN_NAME;
            this.cboBukkenName.DataSource = dt;

            if (!UtilData.ExistsData(dt))
            {
                this.cboBukkenName.SelectedValue = -1;
            }
        }

        #endregion

        #region 出荷元コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 出荷元コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboShukkamoto()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetShukkamoto();
            if (!UtilData.ExistsData(ds, Def_M_SHIP_FROM.Name))
            {
                return;
            }

            // 出荷元コンボボックスの初期化
            var dt = ds.Tables[Def_M_SHIP_FROM.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_SHIP_FROM.SHIP_FROM_NAME, null);
            UtilData.SetFld(dr, Def_M_SHIP_FROM.DISP_NO, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboShukkamoto.ValueMember = Def_M_SHIP_FROM.SHIP_FROM_NAME;
            this.cboShukkamoto.DisplayMember = Def_M_SHIP_FROM.SHIP_FROM_NAME;
            this.cboShukkamoto.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {

            }
            else
            {
                this.cboShukkamoto.SelectedIndex = -1;
            }
        }

        #endregion

        #region 出荷先コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 出荷先コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboShukkasaki()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetShukkasaki();
            if (!UtilData.ExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                return;
            }

            // 出荷先コンボボックスの初期化
            var dt = ds.Tables[Def_M_SELECT_ITEM.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_SELECT_ITEM.ITEM_NAME, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboShukkasaki.ValueMember = Def_M_SELECT_ITEM.ITEM_NAME;
            this.cboShukkasaki.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
            this.cboShukkasaki.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {

            }
            else
            {
                this.cboShukkasaki.SelectedIndex = -1;
            }
        }

        #endregion

        #region 荷受先コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 荷受先コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboNiukesaki()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetConsignList();
            if (!UtilData.ExistsData(ds, Def_M_CONSIGN.Name))
            {
                return;
            }

            // 荷受先コンボボックスの初期化
            var dt = ds.Tables[Def_M_CONSIGN.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_CONSIGN.CONSIGN_CD, null);
            UtilData.SetFld(dr, Def_M_CONSIGN.NAME, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboNiukesaki.ValueMember = Def_M_CONSIGN.CONSIGN_CD;
            this.cboNiukesaki.DisplayMember = Def_M_CONSIGN.NAME;
            this.cboNiukesaki.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {
                this.cboNiukesaki.SelectedValue = decimal.MinValue;
            }
            else
            {
                this.cboNiukesaki.SelectedIndex = -1;
            }
        }

        #endregion

        #region 出荷日の初期化

        /// --------------------------------------------------
        /// <summary>
        /// 出荷日の初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboShukkaDate()
        {
            // 現在の日時から6ヶ月後の日時を計算します
            DateTime currentDate = DateTime.Now;
            DateTime sixMonthsLater = currentDate.AddMonths(6);

            // DateTimePickerコントロールのValueプロパティに設定します
            dtpShukkaDateStart.Value = currentDate;
            dtpShukkaDateEnd.Value = sixMonthsLater;
        }

        #endregion

        #region 製番コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 製番コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboSeiban()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetSeiban();
            if (!UtilData.ExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                return;
            }

            // 製番コンボボックスの初期化
            var dt = ds.Tables[Def_M_SELECT_ITEM.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_NONYUSAKI.SEIBAN, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboSeiban.ValueMember = Def_M_NONYUSAKI.SEIBAN;
            this.cboSeiban.DisplayMember = Def_M_NONYUSAKI.SEIBAN;
            this.cboSeiban.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {

            }
            else
            {
                this.cboSeiban.SelectedIndex = -1;
            }
        }

        #endregion

        #region 機種コンボボックスの初期化

        /// --------------------------------------------------
        /// <summary>
        /// 機種コンボボックスの初期化
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeComboKishu()
        {
            var conn = new ConnS01();
            var cond = new CondS01(this.UserInfo);
            var ds = conn.GetKishu();
            if (!UtilData.ExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                return;
            }

            // 製番コンボボックスの初期化
            var dt = ds.Tables[Def_M_SELECT_ITEM.Name];

            // 空白行追加
            var dr = dt.NewRow();
            UtilData.SetFld(dr, Def_M_SELECT_ITEM.ITEM_NAME, null);
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            this.cboKishu.ValueMember = Def_M_SELECT_ITEM.ITEM_NAME;
            this.cboKishu.DisplayMember = Def_M_SELECT_ITEM.ITEM_NAME;
            this.cboKishu.DataSource = dt;

            if (UtilData.ExistsData(dt))
            {

            }
            else
            {
                this.cboKishu.SelectedIndex = -1;
            }
        }

        #endregion

        #region コンボボックスセット
        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスセット
        /// </summary>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            this.shtMeisai.Columns[(int)SHEET_COL.GENBA_YO_STATUS].Editor = this.CreateSuperiorComboEditor(this.GetCommon(DISP_GENBA_YO_STATUS.GROUPCD).Tables[Def_M_COMMON.Name],
                false, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.VALUE1, false);
        }

        #endregion

        #region

        /// --------------------------------------------------
        /// <summary>
        /// 明細用コンボボックス作成(データバインド)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isTopEmpty"></param>
        /// <param name="displyMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="isEditable"></param>
        /// <returns></returns>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private SuperiorComboEditor CreateSuperiorComboEditor(DataTable dt, bool isTopEmpty, string displyMember, string valueMember, bool isEditable)
        {
            try
            {
                if (isTopEmpty)
                    dt.Rows.InsertAt(dt.NewRow(), 0);
                var cboEditor = ElTabelleSheetHelper.NewSuperiorComboEditor(dt, displyMember, valueMember, isEditable);
                cboEditor.ValueAsIndex = false;
                return cboEditor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/22 再検索時条件をクリアしないように変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                if (EditMode == SystemBase.EditMode.Update) return;

                // クリアComDefine.COMBO_ALL_DISP
                this.cboBukkenName.SelectedIndex = -1;
                this.cboBukkenName.Text = ComDefine.COMBO_ALL_DISP;
                this.cboShukkamoto.SelectedIndex = -1;
                this.cboShukkamoto.Text = null;
                this.cboShukkasaki.SelectedIndex = -1;
                this.cboShukkasaki.Text = null;
                this.cboNiukesaki.SelectedIndex = -1;
                this.cboNiukesaki.Text = null;
                this.InitializeComboShukkaDate();
                this.cboSeiban.SelectedIndex = -1;
                this.cboSeiban.Text = null;
                this.cboKishu.SelectedIndex = -1;
                this.cboKishu.Text = null;
                // グリッドのクリア
                this.SheetClear();

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
        /// <create>J.Chen 2023/08/24</create>
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.CheckInputSerch_Header() == false)
                {
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

        /// --------------------------------------------------
        /// <summary>
        /// ヘッダ入力チェック
        /// </summary>
        /// <returns>true:OK false:NG</returns>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CheckInputSerch_Header()
        {
            bool ret = false;
            try
            {

                ret = true;
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
        /// 検索処理制御部(表示位置復元あり)
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <param name="pos">以前の検索位置</param>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunSearch(Position pos)
        {
            var result = this.RunSearch();
            if (result)
            {
                // 検索が成功し、件数が所定以上なら、位置を復元する
                if (this.shtMeisai.Rows.Count > pos.Row)
                {
                    this.shtMeisai.Enabled = true;
                    this.shtMeisai.TopLeft = pos;
                }
            }
            return result;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>J.Chen 2023/08/24</create>
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/03 現場用ステータス更新欄追加、更新制限追加</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS01 cond = new CondS01(this.UserInfo);
                ConnS01 conn = new ConnS01();

                //物件入力チェック
                if (cboBukkenName.SelectedValue != null)
                {
                    cond.BukkenName = this.cboBukkenName.Text;
                    cond.BukkenNO = this.cboBukkenName.SelectedValue.ToString();
                }
                else
                {
                    //物件が選択されていません。
                    this.ShowMessage("S0100010015");
                    return false;
                }

                // 出荷元
                if (this.cboShukkamoto.SelectedValue != null && this.cboShukkamoto.SelectedIndex > 0)
                {
                    cond.ShipFrom = this.cboShukkamoto.SelectedValue.ToString();
                }
                // 出荷先
                if (this.cboShukkasaki.SelectedValue != null && this.cboShukkasaki.SelectedIndex > 0)
                {
                    cond.ShipTo = this.cboShukkasaki.SelectedValue.ToString();
                }
                // 荷受先
                if (cboNiukesaki.SelectedValue != null && this.cboNiukesaki.SelectedIndex > 0)
                {
                    cond.ConsignCD = this.cboNiukesaki.SelectedValue.ToString();
                }
                // 出荷日
                cond.ShipDateStart = this.dtpShukkaDateStart.Value.ToString("yyyy/MM/dd");
                cond.ShipDateEnd = this.dtpShukkaDateEnd.Value.ToString("yyyy/MM/dd");
                // 製番
                if (this.cboSeiban.SelectedValue != null && this.cboSeiban.SelectedIndex > 0)
                {
                    cond.Seiban = this.cboSeiban.SelectedValue.ToString();
                }  
                // 機種
                if (this.cboKishu.SelectedValue != null && this.cboKishu.SelectedIndex > 0)
                {
                    cond.Kishu = this.cboKishu.SelectedValue.ToString();
                }

                DataSet ds = conn.GetNonyusakiForShokai(cond);

                // シートのクリア
                this.SheetClear();
                if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                {
                    // 該当する出荷計画はありません。
                    this.ShowMessage("S0100070001");
                    return false;
                }
                this.shtMeisai.Redraw = false;

                this.shtMeisai.AllowUserToAddRows = true;

                this.SetComboBox();
                DataTable dt = ds.Tables[Def_M_NONYUSAKI.Name];
                this.shtMeisai.DataSource = dt;

                this.shtMeisai.AllowUserToAddRows = false;

                this.cboBukkenName.Enabled = false;
                this.dtpShukkaDateStart.Enabled = false;
                this.dtpShukkaDateEnd.Enabled = false;
                this.fbrFunction.F03Button.Enabled = true;
                this.fbrFunction.F04Button.Enabled = true;
                this.fbrFunction.F06Button.Enabled = true;
                this.fbrFunction.F10Button.Enabled = true;

                SetAllRowColor();

                this.fbrFunction.F01Button.Enabled = true;

                if (dt.Rows.Count > 500)
                {
                    // 検索結果が500件を超えています。状態更新が必要な場合は、条件を変更して再度検索してください。
                    this.ShowMessage("S0100070002");
                    var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                    txtEditor.ReadOnly = true;
                    this.shtMeisai.Columns[(int)SHEET_COL.GENBA_YO_STATUS].Enabled = false;
                    this.shtMeisai.Columns[(int)SHEET_COL.GENBA_YO_BUTSURYO].Editor = txtEditor;
                    this.fbrFunction.F02Button.Enabled = false;
                }
                else
                {
                    var txtEditor = ElTabelleSheetHelper.NewTextEditor();
                    txtEditor.ReadOnly = false;
                    txtEditor.MaxLength = 100;
                    this.shtMeisai.Columns[(int)SHEET_COL.GENBA_YO_STATUS].Enabled = true;
                    this.shtMeisai.Columns[(int)SHEET_COL.GENBA_YO_BUTSURYO].Editor = txtEditor;
                    this.fbrFunction.F02Button.Enabled = true;
                }

                this.shtMeisai.Redraw = true;
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F01Button_Click(sender, e);
            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var bukkenName = this.shtMeisai[(int)SHEET_COL.BUKKEN_NAME, row].Text;
                var shipSeiban = this.shtMeisai[(int)SHEET_COL.SHIP_SEIBAN, row].Text;
                var consignCD = this.shtMeisai[(int)SHEET_COL.CONSIGN_CD, row].Text;

                using (var frm = new ShukkaKeikaku(this.UserInfo, ComDefine.TITLE_S0100010, bukkenName, shipSeiban, consignCD))
                {
                    if (DialogResult.OK == frm.ShowDialog())
                    {
                        // 再検索を行いますか？
                        if (DialogResult.OK == this.ShowMessage("T0100030003"))
                        {
                            // 再検索を行う
                            this.btnStart_Click(sender, e);
                        }
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
        /// F2ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                EditMode = SystemBase.EditMode.Update;
                var pos = this.shtMeisai.TopLeft;
                if (RunEdit())
                {
                    // 処理が成功したとき、再検索を行う
                    RunSearch(pos);
                }
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var bukkenName = this.shtMeisai[(int)SHEET_COL.BUKKEN_NAME, row].Text;
                var ship = this.shtMeisai[(int)SHEET_COL.SHIP, row].Text;

                CondS01 cond = new CondS01(this.UserInfo);

                this.ClearMessage();
                //ここに既に表示されているかを判定し
                //表示されているなら一番前に
                foreach (Form form in Application.OpenForms)
                {
                    BaseForm frm = form as BaseForm;
                    if (frm != null && frm.MenuCategoryID.Equals("S01") && frm.MenuItemID.Equals("S0100020"))
                    {
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        // 画面をTopへ
                        frm.Activate();
                        return;
                    }
                }
                // 連携状況画面を新規で開く
                var workFrm = new ShukkaKeikakuMeisai(this.UserInfo, "S01", "S0100020", ComDefine.TITLE_S0100020);
                ShukkaKeikakuMeisai.ShukkaKeikakuMeisaiInstance = workFrm;
                ShukkaKeikakuMeisai.ShukkaKeikakuMeisaiInstance.BukkenNameText = bukkenName;
                ShukkaKeikakuMeisai.ShukkaKeikakuMeisaiInstance.ShipText = ship;

                // フォームの表示
                ShukkaKeikakuMeisai.ShukkaKeikakuMeisaiInstance.Show();
                ShukkaKeikakuMeisai.ShukkaKeikakuMeisaiInstance.Activate();
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                // 検索結果がなければ処理を抜ける
                if (this.shtMeisai.MaxRows < 1) return;

                var row = this.shtMeisai.ActivePosition.Row;
                var bukkenName = this.shtMeisai[(int)SHEET_COL.BUKKEN_NAME, row].Text;
                var ship = this.shtMeisai[(int)SHEET_COL.SHIP, row].Text;

                CondS01 cond = new CondS01(this.UserInfo);

                this.ClearMessage();
                //ここに既に表示されているかを判定し
                //表示されているなら一番前に
                foreach (Form form in Application.OpenForms)
                {
                    BaseForm frm = form as BaseForm;
                    if (frm != null && frm.MenuCategoryID.Equals("K01") && frm.MenuItemID.Equals("K0100010"))
                    {
                        if (frm.WindowState == FormWindowState.Minimized)
                        {
                            frm.WindowState = FormWindowState.Normal;
                        }
                        // 画面をTopへ
                        frm.Activate();
                        return;
                    }
                }
                // 連携状況画面を新規で開く
                var workFrm = new ShukaKaishi(this.UserInfo, "K01", "K0100010", ComDefine.TITLE_K0100010);
                ShukaKaishi.ShukaKaishiInstance = workFrm;
                ShukaKaishi.ShukaKaishiInstance.BukkenNameText = bukkenName;
                ShukaKaishi.ShukaKaishiInstance.ShipText = ship;

                // フォームの表示
                ShukaKaishi.ShukaKaishiInstance.Show();
                ShukaKaishi.ShukaKaishiInstance.Activate();
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK) return;
                // グリッドのクリア
                this.SheetClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F07ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
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
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/22 現場用データ出力処理追加</update>
        /// <update>J.Chen 2024/11/07 ファイル名変更</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F10Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F10Button_Click(sender, e);
            try
            {
                DataTable dtUpdCheck = null;
                DataTable dtSrc = (this.shtMeisai.DataSource as DataTable).DefaultView.ToTable();
                dtUpdCheck = dtSrc.GetChanges(DataRowState.Modified);
                if (dtUpdCheck != null)
                {
                    // 内容が変更されているため、Excel出力できません。
                    this.ShowMessage("S0100070004");
                    return;
                }

                if (dtSrc.Rows.Count > 100000)
                {
                    // 出力できる行数が上限値に達しました。システム管理者に確認して下さい。
                    this.ShowMessage("T0100030013");
                    return;
                }
                else if (dtSrc.Rows.Count <= 0)
                {
                    // 該当する出荷計画はありません。
                    this.ShowMessage("S0100070001");
                    return;
                }

                // 画面に貼り付けてある SaveFileDialog を使用するとボタン連打でStackOverFlowが発生する
                using (SaveFileDialog frm = new SaveFileDialog())
                {
                    string fileNameTemp = this.cboBukkenName.Text;
                    if (!string.IsNullOrEmpty(this.cboShukkasaki.Text))
                    {
                        fileNameTemp = fileNameTemp + "_" + this.cboShukkasaki.Text;
                    }

                    frm.Title = Resources.ShukkaKeikakuShokai_sfdExcel_Title;
                    frm.Filter = Resources.ShukkaKeikakuShokai_sfdExcel_Filter;
                    frm.FileName = string.Format(ComDefine.EXCEL_FILE_SHUKKA_KEIKAKU_SHOKAI
                        , fileNameTemp
                        , DateTime.Now.ToString("yyyyMMdd")
                        );

                    if (0 < dtSrc.Rows.Count && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).DefaultView.ToTable();

                    // 処理フラグが削除の場合、出力しない
                    for (int i = dtExport.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dtExport.Rows[i][(int)SHEET_COL.SHORI_FLAG].ToString() == DeletedFlag.ToString())
                        {
                            dtExport.Rows.RemoveAt(i);
                        }
                    }
                    
                    // 日付を曜日付けるように
                    foreach (DataRow row in dtExport.Rows)
                    {
                        if (row[(int)SHEET_COL.SHUKKA_DATE] != DBNull.Value)
                        {

                            DateTime shukkaDate = DateTime.ParseExact((string)row[(int)SHEET_COL.SHUKKA_DATE], "yyyy/MM/dd", null);
                            string formattedDate = shukkaDate.ToString("yy/MM/dd(ddd)");
                            row[(int)SHEET_COL.SHUKKA_DATE] = formattedDate;
                        }
                        if (row[(int)SHEET_COL.TOUCHAKU_YOTEI_DATE] != DBNull.Value)
                        {

                            DateTime shukkaDate = DateTime.ParseExact((string)row[(int)SHEET_COL.TOUCHAKU_YOTEI_DATE], "yyyy/MM/dd", null);
                            string formattedDate = shukkaDate.ToString("yy/MM/dd(ddd)");
                            row[(int)SHEET_COL.TOUCHAKU_YOTEI_DATE] = formattedDate;
                        }
                    }


                    ExportShukkaKeikakuShokai export = new ExportShukkaKeikakuShokai();
                    string msgID;
                    string[] args;

                    export.ExportExcel(frm.FileName, dtExport, out msgID, out args);
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
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/08/24</create>
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

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            // グリッドクリア
            this.SheetClear();

            try
            {
                this.RunSearch();

                this.shtMeisai.Enabled = true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

            // フォーカス
            this.shtMeisai.Focus();
        }

        #endregion

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondS01 GetCondition()
        {
            CondS01 cond = new CondS01(this.UserInfo);

            return cond;
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update>J.Chen 2024/10/03 現場用ステータス更新欄追加</update>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            this.shtMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtMeisai.MaxRows)
            {
                //this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
            }
            this.shtMeisai.AllowUserToAddRows = false;
            this.shtMeisai.DataSource = null;
            this.shtMeisai.MaxRows = 0;
            this.shtMeisai.Enabled = false;

            this.cboBukkenName.Enabled = true;
            this.dtpShukkaDateStart.Enabled = true;
            this.dtpShukkaDateEnd.Enabled = true;

            this.fbrFunction.F01Button.Enabled = false;
            this.fbrFunction.F02Button.Enabled = false;
            this.fbrFunction.F03Button.Enabled = false;
            this.fbrFunction.F04Button.Enabled = false;
            this.fbrFunction.F06Button.Enabled = false;
            this.fbrFunction.F10Button.Enabled = false;

            this.shtMeisai.Redraw = true;
        }

        #endregion

        #region 背景色の設定

        /// --------------------------------------------------
        /// <summary>
        /// 行の背景色、及び前景色を変更する
        /// </summary>
        /// <param name="row">列</param>
        /// <param name="input">入力文字列</param>
        /// <param name="baseGridLine">基準となる罫線設定</param>
        /// <param name="border">罫線設定</param>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------    
        private void SetupRowColor(Row row, string input, GridLine baseGridLine, Borders border)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var cols = input.Split(',');
            if (cols.Length < 2)
            {
                return;
            }

            var backcolor = ComFunc.GetColorFromRgb(cols[1]);
            if (backcolor != null)
            {
                row.BackColor = backcolor ?? row.BackColor;
                row.DisabledBackColor = backcolor ?? row.BackColor;
                // 背景初期を設定すると、罫線がなくなってしまうため、手動設定する
                row.SetBorder(new BorderLine()
                {
                    Color = baseGridLine.Color,
                    LineStyle = (BorderLineStyle)Enum.Parse(typeof(BorderLineStyle), Enum.GetName(typeof(GridLineStyle), baseGridLine.Style)),
                }, border);
            }

            var forecolor = ComFunc.GetColorFromRgb(cols[0]);
            if (forecolor != null)
            {
                row.ForeColor = forecolor ?? row.ForeColor;
                row.DisabledForeColor = forecolor ?? row.ForeColor;
            }
        }

        #endregion

        #region シート全体対象行背景色変更

        /// --------------------------------------------------
        /// <summary>
        /// シート全体対象行背景色変更
        /// </summary>
        /// <create>J.Chen 2023/08/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllRowColor()
        {
            var rows = shtMeisai.MaxRows;      // 行数を取得
            var columns = shtMeisai.MaxColumns; // 列数を取得

            var grycolor = ComDefine.GRY_COLOR;
            var bluecolor = ComDefine.BLUE_COLOR;
            var gratiscolor = ComDefine.GRATIS_COLOR;
            var onerouscolor = ComDefine.ONEROUS_COLOR;

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                var shoriFlag = this.shtMeisai[(int)SHEET_COL.SHORI_FLAG, rowIndex].Value;
                var shukkaFlag = this.shtMeisai[(int)SHEET_COL.MIN_JYOTAI_FLAG, rowIndex].Value;

                var estimateFlagValue = this.shtMeisai[(int)SHEET_COL.ESTIMATE_FLAG, rowIndex].Value;
                var estimateFlag = (estimateFlagValue != null) ? estimateFlagValue.ToString() : null;

                //無償/有償
                if (estimateFlag != null && estimateFlag == Resources.ShukkaKeikakuShokai_EstimateFlag_Noncommercial)
                {
                    //無償の色
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], gratiscolor, this.shtMeisai.GridLine, Borders.All);
                }
                else if (estimateFlag != null && estimateFlag == Resources.ShukkaKeikakuShokai_EstimateFlag_Commercial)
                {
                    //有償の色
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], onerouscolor, this.shtMeisai.GridLine, Borders.All);
                }
                if (shukkaFlag != null && Convert.ToInt32(shukkaFlag) >= ShukkazumiFlag && Convert.ToInt32(shukkaFlag) != HikiwatashiFlag)
                {
                    //色を青に設定する
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], bluecolor, this.shtMeisai.GridLine, Borders.All);
                }

                if (shoriFlag != null && Convert.ToInt32(shoriFlag) == DeletedFlag)
                {
                    //色をグレーに設定する
                    SetupRowColor(this.shtMeisai.Rows[rowIndex], grycolor, this.shtMeisai.GridLine, Borders.All);

                    //// TAG発行状況とステータスを空白に設定する　→　Cancelの空白設定をデータ取得時に実施するように変更
                    //this.shtMeisai[(int)SHEET_COL.TAG_NUM, rowIndex].Value = null;
                    //this.shtMeisai[(int)SHEET_COL.TAG_STATUS, rowIndex].Value = "Cancel";
                }
            }
        }

        #endregion

        #region 表示選択コンボボックスによるデータの絞込
        /// --------------------------------------------------
        /// <summary>
        /// 表示選択コンボボックスの値が変わった時の処理
        /// </summary>
        /// <create>J.Chen 2023/08/31</create>
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
        /// <create>J.Chen 2023/08/31</create>
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
        /// <create>J.Chen 2023/08/31</create>
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

                if (!string.IsNullOrEmpty(this.cboBukkenName.Text))
                {
                    if (!UtilData.ExistsData(sheet.DataSource as DataTable))
                    {
                        return;
                    }
                    // 出荷元
                    if (!string.IsNullOrEmpty(this.cboShukkamoto.Text))
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SHIP_FROM, this.cboShukkamoto, true);
                    }
                    else 
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SHIP_FROM, ChangeLanguage(this.UserInfo.Language), true);
                    }
                    // 出荷先
                    if (!string.IsNullOrEmpty(this.cboShukkasaki.Text))
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SHIP_TO, this.cboShukkasaki, true);
                    }
                    else
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SHIP_TO, ChangeLanguage(this.UserInfo.Language), true);
                    }
                    // 荷受先
                    if (this.cboNiukesaki.SelectedValue != null && !string.IsNullOrEmpty(this.cboNiukesaki.SelectedValue.ToString()))
                    {
                        this._filter.SetFilterFromValue(sheet.DataSource, Def_M_CONSIGN.CONSIGN_CD, this.cboNiukesaki.SelectedValue.ToString(), true);
                    }
                    else
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_CONSIGN.CONSIGN_CD, ChangeLanguage(this.UserInfo.Language), true);
                    }
                    // 製番
                    if (!string.IsNullOrEmpty(this.cboSeiban.Text))
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SEIBAN, this.cboSeiban, true);
                    }
                    else
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.SEIBAN, ChangeLanguage(this.UserInfo.Language), true);
                    }
                    // 機種
                    if (!string.IsNullOrEmpty(this.cboKishu.Text))
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.KISHU, this.cboKishu, true);
                    }
                    else
                    {
                        this._filter.SetFilterFromText(sheet.DataSource, Def_M_NONYUSAKI.KISHU, ChangeLanguage(this.UserInfo.Language), true);
                    }

                    // 背景色再設定
                    SetAllRowColor();
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

        /// --------------------------------------------------
        /// <summary>
        /// 空白の場合は全てへ変更
        /// </summary>
        /// <create>J.Chen 2023/08/31</create>
        /// <update></update>
        /// --------------------------------------------------
        private Control ChangeLanguage(string language)
        {
            Control control = new Control();

            control.Text = Resources.ShukkaKeikakuShokai_SelectAll;

            return control;
        }
        #endregion

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドクリア
                    this.SheetClear();
                }
                EditMode = SystemBase.EditMode.None; //ファンクションキー押下時に決定するため、状態を元に戻す
                return ret;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                DataTable dt = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtUpdate = this.GetDataShukkaKeikakuShokaiMeisaiFilter(dt);   // 更新データ抽出

                if (dt == null || dtUpdate == null)
                {
                    // 更新対象となる行がありません。
                    this.ShowMessage("S0100070003");
                    return false;
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtUpdate);

                // DB更新
                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();
                string errMsgID;
                string[] args;

                if (!conn.UpdNonyusakiForGenbayo(cond, ds, out errMsgID, out args))
                {
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                    }
                    return false;
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
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 登録データ取得
        /// --------------------------------------------------
        /// <summary>
        /// 出荷計画情報取得(フィルター抽出)
        /// </summary>
        /// <param name="dtSrc">取得元明細情報</param>
        /// <param name="state">抽出条件</param>
        /// <returns>納入先マスタテーブル</returns>
        /// <create>J.Chen 2024/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetDataShukkaKeikakuShokaiMeisaiFilter(DataTable dtSrc)
        {
            try
            {
                DataTable dt = null;
                string name = string.Empty;

                dt = dtSrc.GetChanges(DataRowState.Modified);
                if (dt == null) return null;
                name = ComDefine.DTTBL_UPDATE;
                
                dt.TableName = name;

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #endregion
    }
}
