/// ---
/// <注意事項>
/// ・シートのフィールド名をデザイナーで設定しており、変更の際には注意すること
/// ---
//#define __DEBUG_ENABLE_CODE__     // デバッグ用コードのため、定義状態でアップしないこと

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
using SMS.T01.Properties;
using SystemBase.Util;
using WsConnection.WebRefT01;
using ActiveReportsHelper;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配入荷検品
    /// </summary>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiKenpin : SystemBase.Forms.CustomOrderForm
    {
        #region Enum
        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>S.Furugo 2018/11/16</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>S.Furugo 2018/11/16</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
        }

        /// --------------------------------------------------
        /// <summary>
        /// 列インデックス(DBと順不同)
        /// </summary>
        /// <create>D.Naito 2018/12/07</create>
        /// <update>K.Tsutsumi 2019/02/11 列の配置変更</update>
        /// <update>Y.Shioshi 2022/05/18 STEP14 列追加対応</update>
        /// <update>R.Miyoshi 2023/07/13 連携No.追加</update>
        /// --------------------------------------------------
        private enum SHEET_COL : int
        {
              IKKATU = 0
            , RENKEI_NO
            , NOUHIN_SAKI
            , TEHAI_FLAG_NAME
            , TEHAI_NO
            , SEIBAN
            , CODE
            , ZUMEN_KEISHIKI
            , HINMEI_JP
            , HINMEI
            , HACCHU_QTY
            , ZAN_QTY
            , ARRIVAL_QTY           // 手配数(入荷数)
            , BUKKEN_NAME
            , ECS_NO
            , ARRIVAL_ACTUAL_QTY    // 手配数(入荷数)実績
            , TM_VERSION        // 手配明細テーブルのバージョン
            , TMS_VERSION       // SKS手配明細テーブルのバージョン
        }
        #endregion

        #region 物件名コンボボックスのインデックス
        /// --------------------------------------------------
        /// <summary>
        /// 物件名コンボボックスのインデックス
        /// </summary>
        /// <create>T.Nukaga 2019/12/12 STEP12 AR特殊検索条件対応</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum ComboBukkenIndex
        {
            All,
            ARMishukka,
            AR
        }
        #endregion

        #region 定数
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_TOPLEFT_COL = (int)SHEET_COL.IKKATU;
        /// --------------------------------------------------
        /// <summary>
        /// 固定された列数
        /// </summary>
        /// <create>K.Tsutsumi 2019/02/11</create>
        /// <update>2022/06/02 STEP14 手配区分追加　4→5に変更</update>
        /// <update>R.Miyoshi 2023/07/13 連携No.追加　5→6に変更</update>
        /// --------------------------------------------------
        private const int SHEET_FREEZE_COLUMNS = 6;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスONの値
        /// </summary>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_ON = 1;
        /// --------------------------------------------------
        /// <summary>
        /// チェックボックスOFFの値
        /// </summary>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int CHECK_OFF = 0;
        #endregion

        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// 印刷用データ(検索時取得データを保持)
        /// </summary>
        /// <create>D.Naito 2018/12/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtPrintData = null;
        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public static TehaiKenpin TehaiKenpinInstance { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 物件名
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string BukkenNameText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// EcsNo
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string EcsNoText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 製番
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string SeibanText { get; set; }
        /// --------------------------------------------------
        /// /// --------------------------------------------------
        /// <summary>
        /// ARNo
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string ARNoText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string CodeText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// KeysArray
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/04</create>
        /// --------------------------------------------------
        public string[] keysArray = null;

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
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>2022/04/28 STEP14</update>
        /// --------------------------------------------------
        public TehaiKenpin(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();
            base.InitializeLoadControl();

            try
            {
                // シート設定
                this.InitializeSheet(shtTehaiMeisai);

                // フォームの状態を初期化
                this.DisplayClear();
                this.txtNouhinsaki.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }

        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // シート設定
                this.InitializeSheet(shtTehaiMeisai);

                // フォームの状態を初期化
                this.DisplayClear();
                this.txtNouhinsaki.Focus();
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
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスを設定する。
                this.txtNouhinsaki.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #region Sheet初期化

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>D.Naito 2018/12/07</create>
        /// <update>K.Tsutsumi 2019/02/11 列の固定位置を指定、列の配置変更</update>
        /// <update>Y.Shioshi 2022/05/18 STEP14 列追加対応</update>
        /// <update>R.Miyoshi 2023/07/13 連携No.追加</update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            try
            {
                base.InitializeSheet(sheet);

                // シートのタイトルを設定
                sheet.ColumnHeaders[(int)SHEET_COL.IKKATU].Caption = "";
                sheet.ColumnHeaders[(int)SHEET_COL.RENKEI_NO].Caption = Resources.TehaiKenpin_CooperationNo;
                sheet.ColumnHeaders[(int)SHEET_COL.NOUHIN_SAKI].Caption = Resources.TehaiKenpin_Nouhinsaki;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_FLAG_NAME].Caption = Resources.TehaiKenpin_TehaiFlagName;
                sheet.ColumnHeaders[(int)SHEET_COL.TEHAI_NO].Caption = Resources.TehaiKenpin_TehaiNo;
                sheet.ColumnHeaders[(int)SHEET_COL.SEIBAN].Caption = Resources.TehaiKenpin_Seiban;
                sheet.ColumnHeaders[(int)SHEET_COL.CODE].Caption = Resources.TehaiKenpin_Code;
                sheet.ColumnHeaders[(int)SHEET_COL.ZUMEN_KEISHIKI].Caption = Resources.TehaiKenpin_ZumenKeishiki;
                sheet.ColumnHeaders[(int)SHEET_COL.HINMEI_JP].Caption = Resources.TehaiKenpin_HinmeiJp;
                sheet.ColumnHeaders[(int)SHEET_COL.HINMEI].Caption = Resources.TehaiKenpin_Hinmei;
                sheet.ColumnHeaders[(int)SHEET_COL.HACCHU_QTY].Caption = Resources.TehaiKenpin_HacchuQty;
                sheet.ColumnHeaders[(int)SHEET_COL.ZAN_QTY].Caption = Resources.TehaiKenpin_RemainQty;
                sheet.ColumnHeaders[(int)SHEET_COL.ARRIVAL_QTY].Caption = Resources.TehaiKenpin_NyukaQty;
                sheet.ColumnHeaders[(int)SHEET_COL.BUKKEN_NAME].Caption = Resources.TehaiKenpin_BukkenName;
                sheet.ColumnHeaders[(int)SHEET_COL.ECS_NO].Caption = Resources.TehaiKenpin_EcsNo;

                //列の固定位置を指定
                sheet.FreezeColumns = SHEET_FREEZE_COLUMNS;

                // シートを縦方向に移動する
                sheet.ShortCuts.Add(Keys.Enter, new[] { KeyAction.NextRow });
                sheet.ShortCuts.Add(Keys.Enter | Keys.Shift, new[] { KeyAction.PrevRow });
#if __DEBUG_ENABLE_CODE__
                // デバッグ時は隠しも表示
                for (int index = 0; index < sheet.Columns.Count; index++)
                {
                    sheet.Columns[index].Hidden = false;
                }
#endif
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>R.Sumi 2022/03/01</update>
        /// <update>2022/05/12 STEP14</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.SuspendLayout();
                this.SetComboBox();
                this.SearchCondClear();
                this.SheetClear();

                this.GetSksLastLink();

                this.ChangeMode(DisplayMode.Initialize);
                this.ShowMessage("T0100060006");

                if (this.BukkenNameText != null)
                {
                    this.cboBukkenName.Text = this.BukkenNameText;
                }

                if (this.EcsNoText == null)
                {
                    this.txtEcsNo.Clear();
                }
                else
                {
                    this.txtEcsNo.Text = this.EcsNoText;
                }

                if (this.CodeText == null)
                {
                    this.txtCode.Clear();
                }
                else
                {
                    this.txtCode.Text = this.CodeText;
                }

                if (this.SeibanText == null)
                {
                    this.txtSeiban.Clear();
                }
                else
                {
                    this.txtSeiban.Text = this.SeibanText;
                } 
                if (this.CodeText == null)
                {
                    this.txtCode.Clear();
                }
                else
                {
                    this.txtCode.Text = this.CodeText;
                }

                if (this.ARNoText == null)
                {
                    this.txtARNo.Clear();
                }
                else
                {
                    this.txtARNo.Text = this.ARNoText;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件クリア処理
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>T.Nukaga 2019/11/13 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
        /// <update>K.Tsutsumi 2020/04/26 コンボボックスの初期表示「全て(AR未出荷)」→「全て」 </update>
        /// <update>K.Tsutsumi 2020/04/26 SelectedIndex -> SelectedValue </update>
        /// --------------------------------------------------
        protected void SearchCondClear()
        {
            try
            {
                this.txtNouhinsaki.Text = string.Empty;
                this.txtTehaiNo.Text = string.Empty;
                if (this.cboBukkenName.DataSource != null)
                    this.cboBukkenName.SelectedValue = Resources.cboAll;
                this.txtEcsNo.Text = string.Empty;
                this.txtSeiban.Text = string.Empty;
                this.txtCode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// シートのクリア
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            this.shtTehaiMeisai.Redraw = false;
            // 最も左上に表示されているセルの設定
            if (0 < this.shtTehaiMeisai.Rows.Count)
            {
                this.shtTehaiMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
            }
            this.shtTehaiMeisai.DataSource = null;
            this.shtTehaiMeisai.MaxRows = 0;
            // 退避データクリア
            this._dtPrintData = null;
            this.shtTehaiMeisai.Redraw = true;
        }

        #endregion

        #region 画面クリアメッセージ無し

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア処理
        /// </summary>
        /// <create>T.SASAYAMA 2023/07/07</create>
        /// --------------------------------------------------
        protected void DisplayClearNoMessage()
        {
            try
            {
                this.SuspendLayout();
                this.SetComboBox();
                this.SearchCondClear();
                this.SheetClear();

                this.GetSksLastLink();

                this.ChangeMode(DisplayMode.Initialize);

                if (this.BukkenNameText != null)
                {
                    this.cboBukkenName.Text = this.BukkenNameText;
                }

                if (this.EcsNoText == null)
                {
                    this.txtEcsNo.Clear();
                }
                else
                {
                    this.txtEcsNo.Text = this.EcsNoText;
                }

                if (this.CodeText == null)
                {
                    this.txtCode.Clear();
                }
                else
                {
                    this.txtCode.Text = this.CodeText;
                }

                if (this.SeibanText == null)
                {
                    this.txtSeiban.Clear();
                }
                else
                {
                    this.txtSeiban.Text = this.SeibanText;
                }
                if (this.CodeText == null)
                {
                    this.txtCode.Clear();
                }
                else
                {
                    this.txtCode.Text = this.CodeText;
                }

                if (this.ARNoText == null)
                {
                    this.txtARNo.Clear();
                }
                else
                {
                    this.txtARNo.Text = this.ARNoText;
                }

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }
        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                int cnt = 0;
                for (int i = 0; i < this.shtTehaiMeisai.Rows.Count; i++)
                {
                    if (DSWUtil.UtilConvert.ToDecimal(this.shtTehaiMeisai[(int)SHEET_COL.ARRIVAL_QTY, i].Value, 0m) != 0m)   // 0の場合も未入力扱いにする
                    {
                        decimal arvAct = DSWUtil.UtilConvert.ToDecimal(this.shtTehaiMeisai[(int)SHEET_COL.ARRIVAL_ACTUAL_QTY, i].Value , 0m) * (-1);
                        decimal arv = DSWUtil.UtilConvert.ToDecimal(this.shtTehaiMeisai[(int)SHEET_COL.ARRIVAL_QTY, i].Value);
                        decimal zan = DSWUtil.UtilConvert.ToDecimal(this.shtTehaiMeisai[(int)SHEET_COL.ZAN_QTY, i].Value);

                        cnt++;
                        if (arvAct > arv || arv > zan)
                        {
                            // {0}行目の入荷数を{1}～{2}の間で入力して下さい。
                            this.ShowMessage("T0100060002", (i + 1).ToString(), arvAct.ToString(), zan.ToString());
                            this.shtTehaiMeisai.ActivePosition = new Position((int)SHEET_COL.ARRIVAL_QTY, i);
                            this.shtTehaiMeisai.Focus();
                            return　false;
                        }

                    }
                }

                if (cnt <= 0)
                {
                    // 入荷数が入力されていません。
                    this.ShowMessage("T0100060003");
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
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>K.Tsutsumi 2020/05/31 修正モード対応</update>
        /// <update>2022/06/10 STEP14 ARNo桁数チェック</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.cboSearchMode.SelectedValue.ToString() == TEHAI_NYUKA_SEARCH_MODE.CORRECT_MODE_VALUE1
                    && (this.cboBukkenName.Text == Resources.cboAll || this.cboBukkenName.SelectedValue == null)
                    && string.IsNullOrEmpty(this.txtNouhinsaki.Text)
                    && string.IsNullOrEmpty(this.txtTehaiNo.Text)
                    && string.IsNullOrEmpty(this.txtEcsNo.Text)
                    && string.IsNullOrEmpty(this.txtSeiban.Text)
                    && string.IsNullOrEmpty(this.txtCode.Text))
                {
                    // シートのクリア
                    this.SheetClear();
                    // いずれかの検索条件を入力してください。
                    this.ShowMessage("T0100060005");
                    return false;
                }

                if(!string.IsNullOrEmpty(this.txtARNo.Text))
                {
                    // ARNo文字数4桁以下の場合
                    if(this.txtARNo.Text.Length < 4)
                    {
                        this.ShowMessage("A9999999077");
                        return false;
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

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>S.Furugo 2018/11/16</create>
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
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>K.Tsutsumi 2020/05/31 修正モード対応</update>
        /// <update>2022/04/19 STEP14</update>
        /// <update>2022/06/03 STEP14 検索条件に手配区分追加</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();

                if (keysArray != null && keysArray.Length > 0)
                {
                    DataSet ds = conn.GetTehaiKenpinCsv(cond, keysArray);
                    if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                    {
                        // 該当する手配明細はありません。
                        this.ShowMessage("T0100060001");
                        return false;
                    }
                    this.shtTehaiMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                    this._dtPrintData = ds.Tables[Def_T_TEHAI_MEISAI.Name].Copy();     // 印刷用データ退避
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtTehaiMeisai.Rows.Count)
                    {
                        this.shtTehaiMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
                    }
                }
                else
                {
                    cond.Nouhinsaki = txtNouhinsaki.Text;
                    cond.TehaiNo = txtTehaiNo.Text;
                    cond.EcsNo = txtEcsNo.Text;
                    cond.ARNo = txtARNo.Text;
                    cond.Seiban = txtSeiban.Text;
                    cond.Code = txtCode.Text;
                    cond.TehaiNyukaSearchMode = cboSearchMode.SelectedValue.ToString();

                    if (Resources.cboAll.Equals(cboBukkenName.SelectedValue))
                    {
                        // 全選択時は条件対象外とする
                        cond.ProjectNo = null;
                    }
                    else
                    {
                        cond.ProjectNo = cboBukkenName.SelectedValue as string;
                    }

                    // 手配区分
                    if (Resources.cboAll.Equals(fcboTehaiFlag.SelectedValue))
                    {
                        // 全選択時
                        cond.TehaiKubun = null;
                    }
                    else
                    {
                        cond.TehaiKubun = fcboTehaiFlag.SelectedValue as string;
                    }

                    DataSet ds = conn.GetTehaiKenpin(cond);

                    if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                    {
                        // 該当する手配明細はありません。
                        this.ShowMessage("T0100060001");
                        return false;
                    }

                    this.shtTehaiMeisai.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                    this._dtPrintData = ds.Tables[Def_T_TEHAI_MEISAI.Name].Copy();     // 印刷用データ退避
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtTehaiMeisai.Rows.Count)
                    {
                        this.shtTehaiMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
                    }

                }
                this.ChangeMode(DisplayMode.EndSearch);

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
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>J.Jeong 2024/08/08 検索条件保存</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // 再検索
                    if (!this.RunSearchExec())
                    {
                        base.DisplayClear();
                        this.SheetClear();
                        this.GetSksLastLink();
                        this.ChangeMode(DisplayMode.Initialize);
                        this.txtNouhinsaki.Focus();
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        #endregion

        #region 登録処理

        /// --------------------------------------------------
        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>S.Furugo 2018/11/16</create>
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
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>K.Tsutsumi 2019/02/11 入荷数が未入力のものもサーバー側へ渡している部分を修正</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                //DataTable dt = (this.shtTehaiMeisai.DataSource as DataTable).Copy().GetChanges(DataRowState.Modified); // 更新データのみ取得
                DataTable dtOrg = this.shtTehaiMeisai.DataSource as DataTable;
                DataTable dt = dtOrg.Clone();

                foreach (DataRow drOrg in dtOrg.Rows)
                {
                    // 入荷数が設定されているものを取得
                    var qty = UtilData.GetFldToInt32(drOrg, Def_T_TEHAI_MEISAI.ARRIVAL_QTY);
                    if (qty != 0)
                    {
                        // 行をコピー
                        dt.Rows.Add(drOrg.ItemArray);
                    }
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 conn = new ConnT01();
                string errMsgID;
                string[] args;
                if (!conn.UpdTehaiKenpin(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID);
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
        /// <create>S.Furugo 2018/11/16</create>
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
        /// <create>S.Furugo 2018/11/16</create>
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
        /// F2ボタンクリック(Preview)
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2018/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F02Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F02Button_Click(sender, e);
            try
            {
                this.printData(true, this._dtPrintData);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック(検品List)
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">イベント</param>
        /// <create>D.Okumura 2018/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                this.printData(false, this._dtPrintData);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F4ボタンクリック(一括検品)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/21</create>
        /// <update>K.Tsutsumi 2020/05/31 修正モード対応</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
            try
            {
                this.ClearMessage();
                // チェック数確認
                if (this.GetCountIkkatu() <= 0)
                {
                    // 選択されている手配明細がありません。
                    this.ShowMessage("T0100060004");
                    return;
                }

                this.shtTehaiMeisai.Redraw = false;
                for (int i = 0; i < this.shtTehaiMeisai.Rows.Count; i++)
                {
                    if (DSWUtil.UtilConvert.ToInt32(this.shtTehaiMeisai[(int)SHEET_COL.IKKATU, i].Value, CHECK_OFF) == CHECK_ON)
                    {
                        if (this.cboSearchMode.SelectedValue.ToString() == TEHAI_NYUKA_SEARCH_MODE.KENPIN_MODE_VALUE1)
                        {
                            this.shtTehaiMeisai[(int)SHEET_COL.ARRIVAL_QTY, i].Value = this.shtTehaiMeisai[(int)SHEET_COL.ZAN_QTY, i].Value;
                        }
                        else
                        {
                            this.shtTehaiMeisai[(int)SHEET_COL.ARRIVAL_QTY, i].Value = DSWUtil.UtilConvert.ToDecimal(this.shtTehaiMeisai[(int)SHEET_COL.HACCHU_QTY, i].Value) * (-1);
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
                this.shtTehaiMeisai.Redraw = true;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F5ボタンクリック(CSV取込み)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.SASAYAMA 2023/07/04</create>
        /// --------------------------------------------------
        protected override void fbrFunction_F05Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F05Button_Click(sender, e);
            try
            {
                OpenFileDialog ofdCsv = new OpenFileDialog();
                ofdCsv.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                if (ofdCsv.ShowDialog() == DialogResult.OK)
                {
                    this.DisplayClearNoMessage();
                    string csvFilePath = ofdCsv.FileName;
                    if (this.ExecuteImport(csvFilePath))
                    {
                        // CSVファイルを取込みました。
                        this.ShowMessage("S0100020058");
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
        /// F6ボタンクリック(Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;
                // 検索結果クリア
                this.SheetClear();
                this.keysArray = null;
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック(All Clear)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>2022/04/19 STEP14</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;

                // データクリア
                this.BukkenNameText = null;
                this.EcsNoText = null;
                this.CodeText = null;
                this.SeibanText = null;
                this.CodeText = null;
                this.ARNoText = null;
                this.keysArray = null;

                // 表示クリア
                this.DisplayClear();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタンクリック
        /// --------------------------------------------------
        /// <summary>
        /// 開始クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
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

        /// --------------------------------------------------
        /// <summary>
        /// 全選択クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetAllInsatsuCheck(true);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 全選択解除クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnAllRelease_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetAllInsatsuCheck(false);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドをチェックします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeSelect_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetRangeInsatsuCheck(true);

            this.shtTehaiMeisai.Redraw = true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択されたグリッドからチェックをはずします。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnRangeRelease_Click(object sender, EventArgs e)
        {
            this.shtTehaiMeisai.Redraw = false;

            this.SetRangeInsatsuCheck(false);

            this.shtTehaiMeisai.Redraw = true;
        }

        #endregion

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>S.Furugo 2018/11/16</create>
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
                        this.btnAllSelect.Enabled = false;
                        this.btnAllRelease.Enabled = false;
                        this.btnRangeSelect.Enabled = false;
                        this.btnRangeRelease.Enabled = false;
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F02Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.fbrFunction.F04Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        this.shtTehaiMeisai.Enabled = false;
                        this.EditMode = SystemBase.EditMode.None;
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索後 -----
                        this.grpSearch.Enabled = false;
                        this.btnAllSelect.Enabled = true;
                        this.btnAllRelease.Enabled = true;
                        this.btnRangeSelect.Enabled = true;
                        this.btnRangeRelease.Enabled = true;
                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F02Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = true;
                        this.fbrFunction.F04Button.Enabled = true;
                        this.fbrFunction.F06Button.Enabled = true;
                        this.shtTehaiMeisai.Enabled = true;
                        this.EditMode = SystemBase.EditMode.Update;
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

        #region コンボボックスの設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの設定
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update>T.Nukaga 2019/11/11 検索条件追加対応(全て(AR未出荷)、全て(AR)</update>
        /// <update>K.Tsutsumi 2020/04/26 コンボボックスの初期表示「全て(AR未出荷)」→「全て」 </update>
        /// <update>K.Tsutsumi 2020/04/26 SelectedIndex -> SelectedValue </update>
        /// <update>K.Tsutsumi 2020/05/31 初期化処理へ検索モード追加</update>
        /// <update>2022/06/03 STEP14 検索条件　手配区分追加</update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            try
            {
                CondT01 cond = new CondT01(this.UserInfo);
                ConnT01 connT01 = new ConnT01();

                DataSet ds = connT01.GetInitTehaiKenpin(cond);
                DataTable dt = ds.Tables[Def_M_PROJECT.Name];
                DataTable dtSearchMode = ds.Tables[TEHAI_NYUKA_SEARCH_MODE.GROUPCD];
                DataTable dtTehaiFlag = ds.Tables[TEHAI_FLAG.GROUPCD];

                if ((UtilData.ExistsData(ds, Def_M_PROJECT.Name)) || (UtilData.ExistsData(ds, TEHAI_NYUKA_SEARCH_MODE.GROUPCD)))
                {
                    this.SuspendLayout();

                    if (UtilData.ExistsData(ds, Def_M_PROJECT.Name))
                    {

                        // 先頭行に「全て」、「全て(AR未出荷)」、「全て(AR)」追加
                        {
                            // 先頭行に「全て」を追加
                            var dr = dt.NewRow();
                            dr[Def_M_PROJECT.PROJECT_NO] = Resources.cboAll;
                            dr[Def_M_PROJECT.BUKKEN_NAME] = Resources.cboAll;
                            dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.All);
                        }
                        {
                            // 全て(AR 未出荷)の追加
                            var dr = dt.NewRow();
                            dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE;
                            dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_MISHUKKA_AR_DISP;
                            dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.ARMishukka);
                        }
                        {
                            // 全て(AR)の追加
                            var dr = dt.NewRow();
                            dr[Def_M_PROJECT.PROJECT_NO] = ComDefine.COMBO_ALL_AR_VALUE;
                            dr[Def_M_PROJECT.BUKKEN_NAME] = ComDefine.COMBO_ALL_AR_DISP;
                            dt.Rows.InsertAt(dr, (int)ComboBukkenIndex.AR);
                        }
                        dt.AcceptChanges();                    //// 先頭行に「全て」を追加

                        this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                        this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
                        this.cboBukkenName.DataSource = dt;

                        this.cboBukkenName.SelectedValue = Resources.cboAll;   // デフォルト選択
                    }

                    if (UtilData.ExistsData(ds, TEHAI_NYUKA_SEARCH_MODE.GROUPCD))
                    {
                        // 検索モード
                        this.MakeCmbBox(this.cboSearchMode, dtSearchMode, Def_M_COMMON.VALUE1, Def_M_COMMON.ITEM_NAME, Def_M_COMMON.DEFAULT_VALUE, false); 
                    }

                    if (UtilData.ExistsData(ds, TEHAI_FLAG.GROUPCD))
                    {
                        // 先頭行に「全て」を追加
                        var dr = dtTehaiFlag.NewRow();
                        dr[Def_M_COMMON.ITEM_NAME] = Resources.cboAll;
                        dr[Def_M_COMMON.VALUE1] = Resources.cboAll;
                        dtTehaiFlag.Rows.InsertAt(dr, (int)ComboBukkenIndex.All);

                        // 手配区分
                        this.fcboTehaiFlag.DisplayMember = Def_M_COMMON.ITEM_NAME;
                        this.fcboTehaiFlag.ValueMember = Def_M_COMMON.VALUE1;
                        this.fcboTehaiFlag.DataSource = dtTehaiFlag;

                        this.fcboTehaiFlag.SelectedValue = Resources.cboAll;   // デフォルト選択
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }
        #endregion

        #region SKS最終連携日時取得
        /// --------------------------------------------------
        /// <summary>
        /// SKS最終連携日時取得
        /// </summary>
        /// <create>S.Furugo 2018/11/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void GetSksLastLink()
        {
            ConnT01 connT01 = new ConnT01();
            DataSet ds = connT01.GetSksLastLink();

            if (UtilData.ExistsData(ds, Def_M_SKS.Name))
            {
                this.txtLastLink.Text = ComFunc.GetFld(ds.Tables[Def_M_SKS.Name], 0, Def_M_SKS.LASTEST_DATE);
            }
            else this.txtLastLink.Text = "";
        }

        #endregion

        #region IKKATU列のチェックボックスON/OFF制御用

        /// --------------------------------------------------
        /// <summary>
        /// 全選択・全選択解除時のIKKATU列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">IKKATU列チェックボックスをONするかどうか</param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetAllInsatsuCheck(bool isChecked)
        {
            var rows = this.GetSelectRowsIndex(new Range[] { new Range((int)SHEET_COL.IKKATU, 0, (int)SHEET_COL.IKKATU, this.shtTehaiMeisai.Rows.Count - 1) });
            this.SetIkkatuRows(rows, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 範囲選択・範囲選択解除時のIKKATU列チェックボックスON/OFF設定
        /// </summary>
        /// <param name="isChecked">IKKATU列チェックボックスをONするかどうか</param>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetRangeInsatsuCheck(bool isChecked)
        {
            var rows = this.GetSelectRowsIndex(this.shtTehaiMeisai.GetBlocks(BlocksType.Selection));
            this.SetIkkatuRows(rows, isChecked);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定行の一括チェックボックス値設定
        /// </summary>
        /// <param name="rows">行インデックス配列</param>
        /// <param name="isCheck">チェックボックスをONするかどうか</param>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetIkkatuRows(int[] rows, bool isChecked)
        {
            foreach (var row in rows)
            {
                this.shtTehaiMeisai[(int)SHEET_COL.IKKATU, row].Value = isChecked ? CHECK_ON : CHECK_OFF;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定Rangeで有効となっているRange配列を取得
        /// </summary>
        /// <param name="ranges">Range配列</param>
        /// <returns>選択行のインデックス配列</returns>
        /// <create>D.Naito 2018/12/10</create>
        /// <update></update>
        /// --------------------------------------------------
        private int[] GetSelectRowsIndex(Range[] ranges)
        {
            var lstRows = new List<int>();
            foreach (var range in ranges)
            {
                for (int rowIndex = range.TopRow; rowIndex <= range.BottomRow; rowIndex++)
                {
                    lstRows.Add(rowIndex);
                }
            }
            return lstRows.ToArray();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 一括にチェックがついている数を取得
        /// </summary>
        /// <returns></returns>
        /// <create>S.Furugo 2018/11/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private int GetCountIkkatu()
        {
            if (this.shtTehaiMeisai.Rows.Count <= 0) return 0;

            int ret = 0;
            for (int i = 0; i < this.shtTehaiMeisai.Rows.Count; i++)
            {
                if (DSWUtil.UtilConvert.ToInt32(this.shtTehaiMeisai[(int)SHEET_COL.IKKATU, i].Value, CHECK_OFF) == CHECK_ON)
                    ret++;
            }
            return ret;
        }

        #endregion

        #region 検品List
        /// --------------------------------------------------
        /// <summary>
        /// 明細取得・印刷/プレビュー
        /// </summary>
        /// <param name="isPreview">true:プレビュー/false:印刷</param>
        /// <param name="dtHeader">印刷情報</param>
        /// <create>D.Okumura 2018/12/02</create>
        /// <update></update>
        /// --------------------------------------------------
        private void printData(bool isPreview, DataTable dtHeader)
        {
            if (dtHeader == null) return;

            if (!isPreview)
            {
                // 印刷してよろしいですか？ダイアログ
                if (this.ShowMessage("A9999999035") != DialogResult.OK)
                {
                    return;
                }
            }
            var reports = new List<object>();

            reports.Add(ReportHelper.GetReport(ComDefine.REPORT_R0100120_CLASS_NAME, dtHeader));

            if (isPreview)
            {
                PreviewPrinterSetting pps = new PreviewPrinterSetting();
                pps.Landscape = true;
                pps.PaperKind = System.Drawing.Printing.PaperKind.A4;
                ReportHelper.Preview(this, reports.ToArray(), pps);
            }
            else
            {
                ReportHelper.Print(LocalSetting.GetNormalPrinter(), reports.ToArray());
            }
        }

        #endregion

        #region CSV取込み実行
        /// --------------------------------------------------
        /// <summary>
        /// CSV取込み実行
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.SASAYAMA 2023/07/04</create>
        /// --------------------------------------------------
        private bool ExecuteImport(string csvFilePath)
        {
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                try
                {
                    DataTable dtMessage = ComFunc.GetSchemeMultiMessage();

                    Dictionary<string, string> csvValue = CheckCsvValue(sr, dtMessage);

                    if (csvValue.Count == 0)
                    {
                        // 該当する手配明細はありません。
                        this.ShowMessage("T0100060001");
                        return false;
                    }
                    else if (0 < dtMessage.Rows.Count)
                    {
                        // 取込出来ないデータがありました。
                        this.ShowMultiMessage(dtMessage, "T0100012019");
                        // CSVファイルを取り込めませんでした。
                        this.ShowMessage("S0100020059");
                        return false;
                    }

                    //SQLの実行
                    
                    CondT01 cond = new CondT01(this.UserInfo);
                    ConnT01 conn = new ConnT01();

                    keysArray = new List<string>(csvValue.Keys).ToArray();

                    DataSet ds = conn.GetTehaiKenpinCsv(cond, keysArray);
                    if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                    {
                        // 該当する手配明細はありません。
                        this.ShowMessage("T0100060001");
                        return false;
                    }

                    // 印刷用データ退避
                    this._dtPrintData = ds.Tables[Def_T_TEHAI_MEISAI.Name].Copy();

                    // データソースを取得
                    DataTable dt = ds.Tables[Def_T_TEHAI_MEISAI.Name];

                    //取り込めないデータ処理

                    foreach (string key in csvValue.Keys)
                    {
                        bool exists = false;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["TEHAI_NO"].ToString() == key)
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            // 手配No.{0}はマスタに見つかりませんでした。
                            ComFunc.AddMultiMessage(dtMessage, "S0100020057", key);
                            if (0 < dtMessage.Rows.Count)
                            {
                                // 取込出来ないデータがありました。
                                this.ShowMultiMessage(dtMessage, "S0100020033");
                            }
                            // CSVファイルを取り込めませんでした。
                            this.ShowMessage("S0100020059");
                            return false;
                        }
                    }

                    //テーブルに数量を入れる
                    if (dt != null)
                    {
                        // Key (手配No)に該当する列名
                        string keyColumn = "TEHAI_NO";
                        // 数量に該当する列名
                        string quantityColumn = "ARRIVAL_QTY";

                        // Key-Valueペアをループで処理
                        foreach (KeyValuePair<string, string> kvp in csvValue)
                        {
                            string key = kvp.Key;
                            string quantity = kvp.Value;

                            // keyに該当する行を取得
                            DataRow row = dt.AsEnumerable().FirstOrDefault(r => r[keyColumn].ToString() == key);
                            if (row != null)
                            {
                                // 数量を更新
                                row[quantityColumn] = quantity;
                            }
                        }

                        // 更新後のデータテーブルをデータソースにセット
                        this.shtTehaiMeisai.DataSource = dt;
                    }

                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtTehaiMeisai.Rows.Count)
                    {
                        this.shtTehaiMeisai.TopLeft = new Position(SHEET_TOPLEFT_COL, this.shtTehaiMeisai.TopLeft.Row);
                    }

                    this.ChangeMode(DisplayMode.EndSearch);


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            return true;
        }
        #endregion

        #region CSV取込みチェック
        /// --------------------------------------------------
        /// <summary>
        /// CSVをDictionaryに書き込み
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        /// <create>T.SASAYAMA 2023/07/04</create>
        /// --------------------------------------------------
        private Dictionary<string, string> CheckCsvValue(StreamReader sr, DataTable dtMessage)
        {
            try
            {
                //辞書を作って手配Noと数量を取得する
                Dictionary<string, string> csvValue = new Dictionary<string, string>();
                int lineNumber = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    lineNumber++;
                    List<string> values = new List<string>();

                    int pos = 0; // 現在の文字位置
                    while (pos < line.Length)
                    {
                        string value;

                        if (line[pos] == '"')
                        {
                            // ダブルクォートで始まるフィールド
                            int start = pos + 1; // 始まりの位置
                            pos = line.IndexOf('"', start);
                            if (pos == -1)
                            {
                                // ダブルクォートが閉じられていない
                                throw new Exception("Unclosed");
                            }

                            value = line.Substring(start, pos - start);
                            pos += 2; // ダブルクォートと次のカンマをスキップ
                        }
                        else
                        {
                            // ダブルクォートで始まらないフィールド
                            int start = pos;
                            pos = line.IndexOf(',', start);
                            if (pos == -1)
                            {
                                pos = line.Length; // 行の終わり
                            }

                            value = line.Substring(start, pos - start);
                            pos++; // 次のカンマをスキップ
                        }

                        values.Add(value);
                    }
                    if (values.Count >= 2)
                    {
                        string key = values[1]; //2列目 (手配No)
                        string quantityStr;

                        if (values.Count >= 3)
                        {
                            quantityStr = values[2]; //3列目 (数量)
                        }
                        else
                        {
                            quantityStr = "0"; // 3列目（数量）が空の場合0にする
                        }

                        if (key.Length < 8) // keyの桁数が8桁未満であればエラーメッセージを出力
                        {
                            ComFunc.AddMultiMessage(dtMessage, "S0100020060", lineNumber.ToString());
                        }
                        else
                        {
                            long parsedQuantity;
                            string cleanedQuantityStr = quantityStr.Replace(",", "");

                            if (long.TryParse(cleanedQuantityStr, out parsedQuantity))
                            {
                                if (parsedQuantity >= 999999999)
                                {
                                    // 数量が999999999以上の場合、エラーメッセージを出力
                                    ComFunc.AddMultiMessage(dtMessage, "S0100020064", lineNumber.ToString());
                                }
                                else
                                {
                                    //8桁以上ある場合は最初の8桁を取得
                                    key = key.Substring(0, 8);

                                    if (csvValue.ContainsKey(key))
                                    {
                                        // quantityStrを数値に変換してから加算
                                        csvValue[key] = (int.Parse(csvValue[key]) + int.Parse(cleanedQuantityStr)).ToString();
                                    }
                                    else
                                    {
                                        csvValue[key] = cleanedQuantityStr; //カンマを削除して保存
                                    }
                                }
                            }
                            else
                            {
                                // 数値への変換が失敗した場合、エラーメッセージを出力
                                ComFunc.AddMultiMessage(dtMessage, "S0100020061", lineNumber.ToString());
                            }
                        }
                    }
                }
                return csvValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

    }
}
