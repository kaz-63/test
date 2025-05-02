using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using SMS.T01.Properties;
using WsConnection.WebRefT01;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// ＳＫＳ連携状況確認
    /// </summary>
    /// <create>K.Tsutsumi 2019/01/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiRenkei : SystemBase.Forms.CustomOrderForm
    {
        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面モード
        /// </summary>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 検索後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            EndSearch,
            /// --------------------------------------------------
            /// <summary>
            /// わたり発注ボタン押下後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            Watari,
            /// --------------------------------------------------
            /// <summary>
            /// まとめ発注(一括)ボタン押下後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            MatomeAll,
            /// --------------------------------------------------
            /// <summary>
            /// まとめ発注(単価)ボタン押下後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            Matome,
            /// --------------------------------------------------
            /// <summary>
            /// 見積もり都合(一括)ボタン押下後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            EstimateAll,
            /// --------------------------------------------------
            /// <summary>
            /// その他ボタン押下後
            /// </summary>
            /// <create>H.Tajimi 2019/01/17</create>
            /// <update></update>
            /// --------------------------------------------------
            Other,
        }

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

        #endregion

        #region 定数

        private const int MEISAI_SHEET_COL_TEHAI_NO = 0;
        private const int MEISAI_SHEET_COL_TEHAI_QTY = 1;
        private const int MEISAI_SHEET_COL_TEHAI_UNIT_PRICE = 2;
        private const int MEISAI_SHEET_COL_KAITO_DATE = 3;
        private const int MEISAI_SHEET_COL_HACCHU_FLAG_NAME = 4;
        private const int MEISAI_SHEET_COL_KENPIN_UMU = 5;

        private const int TEHAI_NO_LENGTH = 8;

        #endregion

        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 画面表示モード
        /// </summary>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DisplayMode _displayMode = DisplayMode.Initialize;
        /// --------------------------------------------------
        /// <summary>
        /// 選択されている手配明細
        /// </summary>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTehaiMeisai = null;
        /// --------------------------------------------------
        /// <summary>
        /// 取得した手配明細SKS
        /// </summary>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable _dtTehaiSKS = null;
        /// --------------------------------------------------
        /// <summary>
        /// オブジェクト変数
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        private static TehaiRenkei _tehaiRenkeiForm = null;
        /// --------------------------------------------------
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public static TehaiRenkei TehaiRenkeiInstance  { get; set; }
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
        /// <summary>
        /// CODE
        /// </summary>
        /// <create>R.Sumi 2022/02/28</create>
        /// --------------------------------------------------
        public string CodeText { get; set; }
        /// --------------------------------------------------
        /// <summary>
        /// 見積状態チェック
        /// </summary>
        /// <create>J.Chen 2023/02/10</create>
        /// --------------------------------------------------
        public bool isMitsumori = false;

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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiRenkei(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/17</update>
        /// <update>K.Tsutsumi 2019/02/13 コンボボックスの選択肢を作成するタイミングを変更</update>
        /// <update>T.Nukaga 2021/11/29 Step14 上段グリッドに単価列追加</update>
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

                // シートの設定
                this.InitializeSheet(this.shtResult);
                this.InitializeSheet(this.shtMeisai);

                // シートのタイトル設定
                var index = 1;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_BukkenName;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_ECSNo;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_ProductNo;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_Code;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_TehaiKind;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_JPName;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_Name;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_PartNo;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_PartNo2;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_AddNo;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_OrderQty;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_ShukkaQty;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_UnitPrice;
                shtResult.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_TehaiNo;

                index = 0;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_TehaiNo;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_SKSOrderQty;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_SKSUnitPrice;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_ReplyDue;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_SKSStatus;
                shtMeisai.ColumnHeaders[index++].Caption = Resources.TehaiRenkei_NeedInspection;

                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                this.cboCond.Focus();
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/17</update>
        /// <update>K.Tsutsumi 2019/02/13 コンボボックスの選択肢を作成するタイミングを変更</update>
        /// <update>R.Sumi 2022/02/28 検索条件の引継ぎ対応</update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // ヘッダ部
                this.SetComboBox();
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
                this.txtZumenKeishiki.Clear();
                this.txtTehaiNo.Clear();

                this.SheetClear(this.shtResult);
                this.txtLastLink.Clear();
                this.SheetClear(this.shtMeisai);

                // モード切り替え
                this.ChangeMode(DisplayMode.Initialize);
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/17</update>
        /// <update>K.Tsutsumi 2019/02/13 わたり発注の検品未設定検知を追加</update>
        /// <update>J.Chen 2023/02/10 見積済みの場合修正できないように変更（まとめ発注（単価））</update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                var dtSKSMeisai = this.shtMeisai.DataSource as DataTable;
                if (!UtilData.ExistsData(dtSKSMeisai))
                {
                    if (!UtilData.ExistsData(this._dtTehaiSKS))
                    {
                        // 明細が入力されていません。
                        this.ShowMessage("A9999999028");
                        this.shtMeisai.Focus();
                        return false;
                    }
                    else
                    {
                        // 保存前データが存在しない、且つ検索直後データが存在するのは全削除であり
                        // 必須入力項目などは必要ないため、チェック不要
                        return true;
                    }
                }

                // 指定テキストボックスに有効な値が設定されているか取得
                Func<DSWControl.DSWNumericTextBox, string, bool> fncRequiredText = (ctrlNumText, msgID) =>
                {
                    if (string.IsNullOrEmpty(ctrlNumText.Text))
                    {
                        this.ShowMessage(msgID);
                        ctrlNumText.Focus();
                        return false;
                    }
                    return true;
                };

                // 手配Noの重複チェック
                Func<bool> fncDuplicateTehaiNo = () =>
                {
                    for (int rowIndex = 0; rowIndex < this.shtMeisai.MaxRows - 1; rowIndex++)
                    {
                        var tehaiNo = this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, rowIndex].Text;
                        for (int rowIndex2 = rowIndex + 1; rowIndex2 < this.shtMeisai.MaxRows - 1; rowIndex2++)
                        {
                            if (tehaiNo == this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, rowIndex2].Text)
                            {
                                // 手配Noが{0}行目と{1}行目で重複しています。変更してください。
                                this.ShowMessage("T0100020009", (rowIndex + 1).ToString(), (rowIndex2 + 1).ToString());
                                this.shtMeisai.Focus();
                                this.shtMeisai.ActivePosition = new Position(MEISAI_SHEET_COL_TEHAI_NO, rowIndex2);
                                return false;
                            }
                        }
                    }
                    return true;
                };


                // 検品未設定検知
                Func<bool> fncNoCheckKenpinUmu = () =>
                {
                    var blnChecked = false;
                    for (int rowIndex = 0; rowIndex < this.shtMeisai.MaxRows - 1; rowIndex++)
                    {
                        if (!string.IsNullOrEmpty(this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, rowIndex].Text))
                        {
                            if (CheckState.Checked.ToString() == this.shtMeisai[MEISAI_SHEET_COL_KENPIN_UMU, rowIndex].Text)
                            {
                                blnChecked = true;
                                break;
                            }
                        }
                    }
                    return blnChecked;
                };
                
                switch (this._displayMode)
                {
                    case DisplayMode.Watari:
                        if (!fncRequiredText(this.numHacchuQty, "T0100020001"))
                        {
                            // 発注数を入力して下さい。
                            return false;
                        }
                        if (!fncRequiredText(this.numShukkaQty, "T0100020002"))
                        {
                            // 出荷数を入力して下さい。
                            return false;
                        }
                        // 手配No重複入力チェック
                        if (!fncDuplicateTehaiNo())
                        {
                            return false;
                        }
                        // 検品未設定検出
                        if (!fncNoCheckKenpinUmu())
                        {
                            // 検品の指定がないことを通知し、処理を続行するか確認
                            // 検品対象が設定されておりません。\r\n
                            // 最終的に納品されるSKS手配明細が既に存在するのであれば「ｷｬﾝｾﾙ」を押してチェックを付けてください。\r\n
                            // まだSKS手配明細に存在しないのであれば、「OK」を押して処理を続行することができます。\r\n
                            // 処理を続行しますか？
                            if (DialogResult.Cancel == this.ShowMessage("T0100020028"))
                            {
                                return false;
                            }
                        }
                        break;
                    case DisplayMode.MatomeAll:
                        if (dtSKSMeisai.Rows.Count != 1)
                        {
                            // まとめ発注(一括)の場合、SKS手配明細の入力は1件のみにして下さい。
                            this.ShowMessage("T0100020004");
                            this.shtMeisai.Focus();
                            return false;
                        }
                        break;
                    case DisplayMode.Matome:
                        if (dtSKSMeisai.Rows.Count != 1)
                        {
                            // まとめ発注(個別)の場合、SKS手配明細の入力は1件のみにして下さい。
                            this.ShowMessage("T0100020005");
                            this.shtMeisai.Focus();
                            return false;
                        }
                        if (isMitsumori)
                        {
                            // 見積済みのデータが含まれているため、更新できませんでした。
                            this.ShowMessage("T0100020034");
                            return false;
                        }
                        if (!fncRequiredText(this.numTehaiUnitPrice, "T0100020003"))
                        {
                            // 単価を入力して下さい。
                            return false;
                        }
                        break;
                    case DisplayMode.EstimateAll:
                        if (dtSKSMeisai.Rows.Count != 1)
                        {
                            // 見積もり都合(一括)の場合、SKS手配明細の入力は1件のみにして下さい。
                            this.ShowMessage("T0100020006");
                            this.shtMeisai.Focus();
                            return false;
                        }
                        break;
                    case DisplayMode.Other:
                        if (!fncRequiredText(this.numHacchuQty, "T0100020001"))
                        {
                            // 発注数を入力して下さい。
                            return false;
                        }
                        if (!fncRequiredText(this.numShukkaQty, "T0100020002"))
                        {
                            // 出荷数を入力して下さい。
                            return false;
                        }
                        var checkQty = this.numCheckQty.Value;
                        var hacchuQty = this.numHacchuQty.Value;
                        if (checkQty != hacchuQty)
                        {
                            // 発注数は数量チェック対象と同じ値を入力して下さい。
                            this.ShowMessage("T0100020007");
                            this.numHacchuQty.Focus();
                            return false;
                        }
                        var shukkaQty = this.numShukkaQty.Value;
                        if (hacchuQty < shukkaQty)
                        {
                            // 出荷数は発注数以下の値を入力して下さい。
                            this.ShowMessage("T0100020008");
                            this.numShukkaQty.Focus();
                            return false;
                        }
                        // 手配No重複入力チェック
                        if (!fncDuplicateTehaiNo())
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/17</update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                if (this.cboCond.SelectedValue.ToString() == TEHAI_SKS_RENKEI_FLAG.ALL_VALUE1)
                {
                    // 全てが選択されている場合はいずれかの検索条件が必須
                    if (!this.HasAnyCondition())
                    {
                        // いずれかの検索条件を入力してください。
                        this.ShowMessage("T0100070004");
                        this.cboBukkenName.Focus();
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

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細検索用入力チェック
        /// </summary>
        /// <param name="mode">画面表示モード</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update>K.Tsutsumi 2019/02/13 手配種別が設定済みのものに限り、まとめ発注や見積都合でも１件で呼び出せるように対応</update>
        /// <update>J.Chen 2023/02/10 見積済みの場合修正できないように変更（まとめ発注（単価））</update>
        /// --------------------------------------------------
        private bool CheckInputSearchSKS(DisplayMode mode)
        {
            bool ret = true;
            try
            {
                var dtTehaiMeisai = this.GetCheckedTehaiMeisai();
                isMitsumori = false;
                if (!UtilData.ExistsData(dtTehaiMeisai))
                {
                    // Dataが選択されていません。
                    this.ShowMessage("A9999999019");
                    this.shtResult.Focus();
                    return false;
                }

                Func<string, bool> fncCheckTehaiKind = (tehaiKindFlag) =>
                {
                    // 手配明細の手配種別と押下したボタンの整合性チェック
                    var dtTehaiMeisaiExcludeNone = dtTehaiMeisai.Clone();
                    foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                    {
                        if (ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG) != TEHAI_KIND_FLAG.NONE_VALUE1)
                        {
                            // 未設定以外を集める
                            dtTehaiMeisaiExcludeNone.Rows.Add(drTehaiMeisai.ItemArray);
                        }

                        // 見積Noあるかどうか
                        if (!string.IsNullOrEmpty(ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.ESTIMATE_NO)) && !isMitsumori)
                        {
                            isMitsumori = true;
                        }

                    }

                    if (!UtilData.ExistsData(dtTehaiMeisaiExcludeNone))
                    {
                        // 未設定しかない場合はOK
                        return true;
                    }

                    var tmpTehaiKindFlag = ComFunc.GetFld(dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG);
                    foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                    {
                        if (tmpTehaiKindFlag != ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG))
                        {
                            // 選択された手配明細間で手配種別が異なります。同じ手配種別のみ複数選択できます。
                            this.ShowMessage("T0100020020");
                            this.shtResult.Focus();
                            return false;
                        }
                    }
                    if (tmpTehaiKindFlag != tehaiKindFlag)
                    {
                        // 手配明細の手配種別と操作が異なります。
                        this.ShowMessage("T0100020010");
                        this.shtResult.Focus();
                        return false;
                    }
                    return true;
                };

                // 選択された複数の手配明細間で同一型式チェック
                Func<bool> fncCheckSameKatashiki = () =>
                {
                    var zumenKeishiki = ComFunc.GetFld(dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
                    foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                    {
                        if (zumenKeishiki != ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI))
                        {
                            // 選択された手配明細間で型式が異なります。同じ型式のみ複数選択できます。
                            this.ShowMessage("T0100020011");
                            this.shtResult.Focus();
                            return false;
                        }
                    }
                    return true;
                };

                switch (mode)
                {
                    case DisplayMode.Watari:
                        // 手配明細の手配種別と操作の整合性チェック
                        if (!fncCheckTehaiKind(TEHAI_KIND_FLAG.ACROSS_VALUE1))
                        {
                            return false;
                        }
                        // 選択数チェック
                        if (dtTehaiMeisai.Rows.Count != 1)
                        {
                            // わたり発注の場合、手配明細の選択は1件のみにして下さい。
                            this.ShowMessage("T0100020012");
                            this.shtResult.Focus();
                            return false;
                        }
                        break;
                    case DisplayMode.MatomeAll:
                        // 手配明細の手配種別と操作の整合性チェック
                        if (!fncCheckTehaiKind(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1))
                        {
                            return false;
                        }
                        // 選択数チェック
                        // 手配種別が未設定が含まれるときは２件以上必要とする。
                        if (dtTehaiMeisai.Rows.Count < 2)
                        {
                            // 手配明細が1件しか選択されていません。\r\nまとめ発注は複数の手配明細に対して1つの手配No.を設定する機能です。\r\nこのまま処理を続けますか？
                            if (DialogResult.Cancel == this.ShowMessage("T0100020029"))
                            {
                                this.shtResult.Focus();
                                return false;
                            }
                        }
                        break;
                    case DisplayMode.Matome:
                        // 手配明細の手配種別と操作の整合性チェック
                        if (!fncCheckTehaiKind(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1))
                        {
                            return false;
                        }
                        // 選択数チェック
                        if (dtTehaiMeisai.Rows.Count > 1)
                        {
                            // 同一型式チェック
                            if (!fncCheckSameKatashiki())
                            {
                                return false;
                            }
                        }
                        break;
                    case DisplayMode.EstimateAll:
                        // 手配明細の手配種別と操作の整合性チェック
                        if (!fncCheckTehaiKind(TEHAI_KIND_FLAG.ESTIMATE_VALUE1))
                        {
                            return false;
                        }
                        // 選択数チェック
                        if (dtTehaiMeisai.Rows.Count < 2)
                        {
                            // 手配明細が1件しか選択されていません。\r\n見積都合は複数の手配明細(同一品)に対して1つの手配No.を設定する機能です。\r\nこのまま処理を続けますか？
                            if (DialogResult.Cancel == this.ShowMessage("T0100020030"))
                            {
                                this.shtResult.Focus();
                                return false;
                            }
                        }
                        // 同一型式チェック
                        if (!fncCheckSameKatashiki())
                        {
                            return false;
                        }
                        break;
                    case DisplayMode.Other:
                        // 手配明細の手配種別と操作の整合性チェック
                        if (!fncCheckTehaiKind(TEHAI_KIND_FLAG.ANOTHER_VALUE1))
                        {
                            return false;
                        }
                        if (dtTehaiMeisai.Rows.Count != 1)
                        {
                            // その他の場合、手配明細の選択は1件のみにして下さい。
                            this.ShowMessage("T0100020015");
                            this.shtResult.Focus();
                            return false;
                        }
                        break;
                    default:
                        return false;
                }
                if (this._dtTehaiMeisai != null)
                {
                    this._dtTehaiMeisai.Dispose();
                    this._dtTehaiMeisai = null;
                }
                this._dtTehaiMeisai = dtTehaiMeisai.Copy();
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
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/16</update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                // モード切り替え(初期状態に戻す)
                this.ChangeMode(DisplayMode.Initialize);
                var cond = this.GetCondition();
                var conn = new ConnT01();
                var ds = conn.GetSKSTehaiRenkeiDispData(cond);
                if (!ComFunc.IsExistsData(ds, Def_T_TEHAI_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                this.txtLastLink.Text = ComFunc.GetFld(ds.Tables[Def_M_SKS.Name], 0, Def_M_SKS.LASTEST_DATE);
                try
                {
                    this.shtResult.Redraw = false;
                    this.shtResult.DataSource = ds.Tables[Def_T_TEHAI_MEISAI.Name];
                    // モード切り替え
                    this.ChangeMode(DisplayMode.EndSearch);
                    // 最も左上に表示されているセルの設定
                    if (0 < this.shtResult.MaxRows)
                    {
                        this.shtResult.TopLeft = new Position(0, this.shtResult.TopLeft.Row);
                    }
                }
                finally
                {
                    this.shtResult.Redraw = true;
                }
                this.shtResult.Focus();
                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 検索処理(SKS)

        #region 制御部

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理(SKS)制御部
        /// </summary>
        /// <param name="mode">画面表示モード</param>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tajimi 2019/01/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool RunSearchSKS(DisplayMode mode)
        {
            var preCursor = Cursor.Current;
            try
            {
                if (!this.CheckInputSearchSKS(mode))
                {
                    return false;
                }

                Cursor.Current = Cursors.WaitCursor;
                return this.RunSearchSKSExec(mode);
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

        #endregion

        #region 実行部

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理(SKS)実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>H.Tajimi 2019/01/21</create>
        /// <update>H.Tajimi 2019/02/12 手配種別取得方法を変更</update>
        /// --------------------------------------------------
        private bool RunSearchSKSExec(DisplayMode mode)
        {
            try
            {
                // モード切り替え(検索後状態に戻す)
                this.ChangeMode(DisplayMode.EndSearch);
                var conn = new ConnT01();
                var cond = new CondT01(this.UserInfo);
                cond.TehaiKindFlag = this.GetTehaiKindFlag(mode);

                var ds = new DataSet();
                ds.Merge(this._dtTehaiMeisai);
                var dt = conn.GetSKSTehaiRenkeiTehaiSKS(cond, ds);
                if (mode == DisplayMode.Matome)
                {
                    // まとめ発注個別の場合はデータがないとエラー
                    if (!UtilData.ExistsData(dt))
                    {
                        // まとめ発注(一括)を実施後に行って下さい。
                        this.ShowMessage("T0100020024");
                        return false;
                    }
                    else
                    {
                        if (dt.Rows.Count > 1)
                        {
                            // 別々のまとめ発注の設定は1度に呼び出すことはできません。
                            this.ShowMessage("T0100020031");
                            return false;
                        }
                    }
                }
                else if (mode == DisplayMode.MatomeAll)
                {
                    // まとめ発注(一括)の場合は複数件取得できるとエラー
                    if (UtilData.ExistsData(dt))
                    {
                        if (dt.Rows.Count > 1)
                        {
                            // 別々のまとめ発注の設定は1度に呼び出すことはできません。
                            this.ShowMessage("T0100020031");
                            return false;
                        }
                    }
                }
                else if (mode == DisplayMode.EstimateAll)
                {
                    // 見積都合(一括)の場合は複数件取得できるとエラー
                    if (UtilData.ExistsData(dt))
                    {
                        if (dt.Rows.Count > 1)
                        {
                            // 別々の見積都合の設定は1度に呼び出すことはできません。
                            this.ShowMessage("T0100020032");
                            return false;
                        }
                    }
                }
                

                if (this._dtTehaiSKS != null)
                {
                    this._dtTehaiSKS.Dispose();
                    this._dtTehaiSKS = null;
                }
                this._dtTehaiSKS = dt.Copy();

                try
                {
                    this.shtMeisai.Redraw = false;
                    if (!UtilData.ExistsData(dt))
                    {
                        this.shtMeisai.DataSource = dt.Clone();
                    }
                    else
                    {
                        this.shtMeisai.DataSource = dt.Copy();
                    }
                }
                finally
                {
                    this.shtMeisai.Redraw = true;
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/18</update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
                if (ret)
                {
                    // グリッドクリア
                    this.SheetClear(this.shtResult);
                    this.txtLastLink.Clear();
                    this.ChangeMode(DisplayMode.Initialize);
                }
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/17</update>
        /// <update>H.Tajimi 2019/02/12 手配種別取得方法を変更</update>
        /// <update>T.Nukaga 2022/02/27 わたり発注でSKS手配明細に1件でも見積があれば手配明細の単価を更新しないように修正</update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                var conn = new ConnT01();
                var cond = this.GetCondition();
                cond.TehaiKindFlag = this.GetTehaiKindFlag();

                bool isExistMitsumori = false;   // SKS手配明細に1件でも見積があればtrue(わたり発注で使用)

                var ds = new DataSet();
                var dt = this.shtMeisai.DataSource as DataTable;
                var dtSave = dt.Copy();
                dtSave.AcceptChanges();
                if (!UtilData.ExistsData(dtSave))
                {
                    // 保存前データが何も存在しないということは全削除
                    var dtDel = this._dtTehaiSKS.Copy();
                    dtDel.TableName = ComDefine.DTTBL_DELETE;
                    ds.Merge(dtDel);
                }
                else
                {
                    var dtIns = this.GetSchemeInsertData();
                    var dtUpd = this.GetSchemeUpdateData();
                    var dtDel = this._dtTehaiSKS.Clone();
                    dtDel.TableName = ComDefine.DTTBL_DELETE;
                    foreach (DataRow drSave in dtSave.Rows)
                    {
                        var tehaiNo = ComFunc.GetFld(drSave, Def_T_TEHAI_SKS.TEHAI_NO);
                        if (string.IsNullOrEmpty(tehaiNo) || string.IsNullOrEmpty(tehaiNo.Trim()))
                        {
                            continue;
                        }
                        if (this.ExistsData(drSave, this._dtTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO))
                        {
                            // 保存データに存在し、検索直後データに存在する場合は変更行
                            var drUpd = dtUpd.NewRow();
                            UtilData.SetFld(drUpd, Def_T_TEHAI_SKS.TEHAI_NO, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS.TEHAI_NO));
                            UtilData.SetFld(drUpd, Def_T_TEHAI_SKS.VERSION, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS.VERSION));
                            UtilData.SetFld(drUpd, Def_T_TEHAI_SKS.KENPIN_UMU, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS.KENPIN_UMU));
                            dtUpd.Rows.Add(drUpd);
                        }
                        else
                        {
                            // 保存データに存在し、検索直後データに存在しない場合は新規行
                            var drIns = dtIns.NewRow();
                            UtilData.SetFld(drIns, Def_T_TEHAI_SKS_WORK.TEHAI_NO, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS_WORK.TEHAI_NO));
                            // SKS手配明細に構造を合せるためにVERSIONにしていたのをCREATE_DATEに戻す
                            UtilData.SetFld(drIns, Def_T_TEHAI_SKS_WORK.CREATE_DATE, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS.VERSION));
                            UtilData.SetFld(drIns, Def_T_TEHAI_SKS.KENPIN_UMU, ComFunc.GetFldObject(drSave, Def_T_TEHAI_SKS.KENPIN_UMU));
                            dtIns.Rows.Add(drIns);
                        }

                        if (UtilData.GetFld(drSave, Def_T_TEHAI_SKS.HACCHU_ZYOTAI_FLAG) == HACCHU_FLAG.STATUS_1_VALUE1)
                        {
                            isExistMitsumori = true;
                        }
                    }
                    foreach (DataRow drTehaiSKS in this._dtTehaiSKS.Rows)
                    {
                        if (!this.ExistsData(drTehaiSKS, dtSave, Def_T_TEHAI_SKS.TEHAI_NO))
                        {
                            // 検索直後データに存在し、保存データに存在しない場合は削除行
                            dtDel.Rows.Add(drTehaiSKS.ItemArray);
                        }
                    }
                    ds.Merge(dtIns);
                    ds.Merge(dtUpd);
                    ds.Merge(dtDel);
                }
                var dtTehaiMeisai = this.GetShemeTehaiMeisai();
                foreach (DataRow drDisp in this._dtTehaiMeisai.Rows)
                {
                    var drTehaiMeisai = dtTehaiMeisai.NewRow();
                    UtilData.SetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, ComFunc.GetFldObject(drDisp, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO));
                    UtilData.SetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.HACCHU_QTY, this.GetHacchuQty());
                    UtilData.SetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.SHUKKA_QTY, this.GetShukkaQty());
                    UtilData.SetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.UNIT_PRICE, this.GetUnitPrice(isExistMitsumori));
                    UtilData.SetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.VERSION, ComFunc.GetFldObject(drDisp, Def_T_TEHAI_MEISAI.VERSION));
                    dtTehaiMeisai.Rows.Add(drTehaiMeisai);
                }
                ds.Merge(dtTehaiMeisai);

                string errMsgID;
                string[] args;
                if (!conn.UpdSKSTehaiRenkei(cond, ds, out errMsgID, out args))
                {
                    this.ShowMessage(errMsgID, args);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
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
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/18</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.shtMeisai.EditState)
                {
                    return;
                }
                this.ClearMessage();
                base.fbrFunction_F01Button_Click(sender, e);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F3ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/18</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                if (!UtilData.ExistsData(this._dtTehaiMeisai))
                {
                    return;
                }

                // 行がない場合は処理を抜ける
                if (this.shtMeisai.Rows.Count < 1)
                {
                    return;
                }

                if (this._displayMode == DisplayMode.Matome)
                {
                    // 見積もり都合(単価)の場合、行削除はできません。
                    this.ShowMessage("T0100020027");
                    return;
                }

                // 選択されているセル範囲をすべて取得して、新規行(*)が含まれていないかチェックする
                var ranges = this.shtMeisai.GetBlocks(BlocksType.Selection);
                foreach (Range range in ranges)
                {
                    // 行追加時の最終行の場合は処理を抜ける
                    if ((this.shtMeisai.AllowUserToAddRows)
                      && (this.shtMeisai.Rows.Count - 1 == range.TopRow || this.shtMeisai.Rows.Count - 1 == range.BottomRow))
                    {
                        // 行削除を行う場合は、最終行を含まないようにして下さい。
                        this.ShowMessage("T0100020017");
                        return;
                    }
                }

                // SKS手配明細に存在するデータを行削除しようとしているかどうか
                // →新規登録しようとしているデータは、削除してもDBに何ら影響を
                // 及ぼさないため、画面からいつでも削除できるべき
                var dtTehaiSKS = this.GetExistingTehaiSKS(ranges);
                if (UtilData.ExistsData(dtTehaiSKS))
                {
                    foreach (DataRow drTehaiMeisai in this._dtTehaiMeisai.Rows)
                    {
                        if (ComFunc.GetFldToInt32(drTehaiMeisai, ComDefine.FLD_CNT) > 0)
                        {
                            // 出荷済みのデータが含まれているため削除できません。
                            this.ShowMessage("T0100020016");
                            return;
                        }
                    }

                    foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
                    {
                        if (ComFunc.GetFld(drTehaiSKS, Def_T_TEHAI_SKS.KENPIN_UMU) == KENPIN_UMU.ON_VALUE1
                         && ComFunc.GetFldToInt32(drTehaiSKS, Def_T_TEHAI_SKS.ARRIVAL_QTY) > 0)
                        {
                            // 検品済みのデータが含まれているため削除できません。
                            this.ShowMessage("T0100020026");
                            return;
                        }
                    }
                }

                // 選択行を削除してもよろしいですか？
                if (this.ShowMessage("T0100020018") == DialogResult.OK)
                {
                    try
                    {
                        this.shtMeisai.Redraw = false;
                        for (int i = ranges.Length - 1; i >= 0; i--)
                        {
                            // 行数を求める
                            var rowCnt = ranges[i].BottomRow - ranges[i].TopRow + 1;
                            // 行削除
                            this.shtMeisai.RemoveRow(ranges[i].TopRow, rowCnt, false);
                        }
                        (this.shtMeisai.DataSource as DataTable).AcceptChanges();
                        this.UpdateCheckQtyAndUnitPrice();
                        this.shtMeisai.Focus();
                    }
                    finally
                    {
                        this.shtMeisai.Redraw = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/21</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (ShowMessage("A9999999053") != DialogResult.OK)
                {
                    return;
                }
                // グリッドクリア
                this.SheetClear(this.shtResult);
                this.txtLastLink.Clear();
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F7ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>H.Tajimi 2019/01/21</update>
        /// <update>2022/04/19 STEP14</update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // クリアしてもいいですか？ダイアログ表示
                if (this.ShowMessage("A9999999001") != DialogResult.OK)
                {
                    return;
                }

                // データクリア
                this.BukkenNameText = null;
                this.EcsNoText = null;
                this.CodeText = null;
                this.SeibanText = null;

                // 表示クリア
                this.DisplayClear();
                this.ChangeMode(DisplayMode.Initialize);
            }
            catch (Exception ex)
            {
                MsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region ボタン

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/16</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // グリッドクリア
                this.SheetClear(this.shtResult);
                this.txtLastLink.Clear();
                // 検索
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region わたり発注ボタン

        /// --------------------------------------------------
        /// <summary>
        /// わたり発注ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnWatari_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 明細検索
                var mode = DisplayMode.Watari;
                if (this.RunSearchSKS(mode))
                {
                    this.ChangeMode(mode);
                    this.UpdateCheckQtyAndUnitPrice();
                    this.SetHacchuAndShukkaQty();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region まとめ発注(一括)ボタン

        /// --------------------------------------------------
        /// <summary>
        /// まとめ発注(一括)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnMatomeAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 明細検索
                var mode = DisplayMode.MatomeAll;
                if (this.RunSearchSKS(mode))
                {
                    this.ChangeMode(mode);
                    this.UpdateCheckQtyAndUnitPrice();
                    this.SetHacchuAndShukkaQty();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region まとめ発注(単価)ボタン

        /// --------------------------------------------------
        /// <summary>
        /// まとめ発注(単価)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnMatome_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 明細検索
                var mode = DisplayMode.Matome;
                if (this.RunSearchSKS(mode))
                {
                    this.ChangeMode(mode);
                    this.UpdateCheckQtyAndUnitPrice();
                    this.SetHacchuAndShukkaQty();
                    this.numTehaiUnitPrice.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 見積もり都合(一括)ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 見積もり都合(一括)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnEstimateAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 明細検索
                var mode = DisplayMode.EstimateAll;
                if (this.RunSearchSKS(mode))
                {
                    this.ChangeMode(mode);
                    this.UpdateCheckQtyAndUnitPrice();
                    this.SetHacchuAndShukkaQty();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region その他ボタン

        /// --------------------------------------------------
        /// <summary>
        /// その他ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOther_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearMessage();
                // 明細検索
                var mode = DisplayMode.Other;
                if (this.RunSearchSKS(mode))
                {
                    this.ChangeMode(mode);
                    this.UpdateCheckQtyAndUnitPrice();
                    this.SetHacchuAndShukkaQty();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #endregion

        #region テキストボックス

        #region 手配No

        /// --------------------------------------------------
        /// <summary>
        /// 手配Noのフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtTehaiNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtTehaiNo.Text))
            {
                this.txtTehaiNo.Text = this.txtTehaiNo.Text.ToUpper();
            }
        }
        
        #endregion

        #endregion

        #region Sheet

        #region LeaveEdit

        /// --------------------------------------------------
        /// <summary>
        /// セルが非編集モードに入る場合に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>H.Tajimi 2019/01/23</create>
        /// <update>H.Tajimi 2019/02/12 手配種別取得方法を変更</update>
        /// --------------------------------------------------
        private void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                // 手配Noを変更前の状態に戻す
                Action actUndo = () =>
                {
                    if (this.shtMeisai.ActiveCell.Tag == null || string.IsNullOrEmpty(this.shtMeisai.ActiveCell.Tag.ToString()))
                    {
                        this.shtMeisai.ActiveCell.Text = string.Empty;
                        return;
                    }
                    this.shtMeisai.ActiveCell.Text = this.shtMeisai.Tag.ToString();
                };

                var rowIndex = this.shtMeisai.ActivePosition.Row;
                var colIndex = this.shtMeisai.ActivePosition.Column;
                if (colIndex == MEISAI_SHEET_COL_TEHAI_NO)
                {
                    // 手配No列の場合
                    if (!this.CellTextChanged(this.shtMeisai.ActiveCell))
                    {
                        // セルの内容に変更がない場合は何もしない
                        return;
                    }

                    this.ClearMessage();
                    // 入力値の整形
                    this.shtMeisai.ActiveCell.Text = this.GetSheetInputFormat(this.shtMeisai.ActiveCell.Text);
                    this.shtMeisai.Refresh();
                    var tehaiNo = this.shtMeisai.ActiveCell.Text;
                    if (string.IsNullOrEmpty(tehaiNo))
                    {
                        // 手配Noを入力して下さい。
                        this.ShowMessage("T0100020022");
                        actUndo();
                        e.Cancel = true;
                        return;
                    }

                    int maxLength = TEHAI_NO_LENGTH;
                    var txtEditor = this.shtMeisai.ActiveCell.Editor as GrapeCity.Win.ElTabelle.Editors.TextEditor;
                    if (txtEditor != null)
                    {
                        maxLength = txtEditor.MaxLength;
                    }
                    if (tehaiNo.Length < maxLength)
                    {
                        // {0}行目の手配No.は8桁に満たないので8桁入力して下さい。
                        this.ShowMessage("T0100020025", (rowIndex + 1).ToString());
                        actUndo();
                        e.Cancel = true;
                        return;
                    }

                    // 重複チェック
                    for (int i = 0; i < this.shtMeisai.Rows.Count; i++)
                    {
                        if (i == this.shtMeisai.ActivePosition.Row)
                        {
                            continue;
                        }

                        if (this.IsSameTehaiNo(this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, i].Text, tehaiNo))
                        {
                            // 入力された手配Noは、{0}行目に存在します。
                            this.ShowMessage("T0100020019", (i + 1).ToString());
                            actUndo();
                            e.Cancel = true;
                            return;
                        }
                    }

                    // 手配No検索
                    var conn = new ConnT01();
                    var cond = new CondT01(this.UserInfo);
                    cond.TehaiKindFlag = this.GetTehaiKindFlag();
                    cond.TehaiNo = tehaiNo;
                    string errMsgID;
                    string[] args;
                    var ds = conn.GetSKSTehaiRenkeiTehaiSKSWork(cond, out errMsgID, out args);
                    if (!string.IsNullOrEmpty(errMsgID))
                    {
                        this.ShowMessage(errMsgID, args);
                        actUndo();
                        e.Cancel = true;
                        return;
                    }

                    if (!UtilData.ExistsData(ds, Def_T_TEHAI_SKS_WORK.Name))
                    {
                        // 該当手配Noは存在しません。
                        this.ShowMessage("T0100020021");
                        actUndo();
                        e.Cancel = true;
                        return;
                    }

                    // 取得した手配NoをSheetにセットし、DataTableに反映させる
                    var dtTehaiSKSWork = ds.Tables[Def_T_TEHAI_SKS_WORK.Name];
                    var dtBackupMeisai = (this.shtMeisai.DataSource as DataTable).Copy();
                    this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, rowIndex].Value = ComFunc.GetFld(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                    this.shtMeisai.UpdateRowToDataSet(rowIndex);

                    // 手配No以外の値を更新する
                    if (!this.UpdateMeisaiFromTehaiSKSWork(dtTehaiSKSWork))
                    {
                        // 該当手配Noは存在しません。
                        this.ShowMessage("T0100020021");
                        actUndo();
                        e.Cancel = true;
                        this.shtMeisai.DataSource = dtBackupMeisai;
                        return;
                    }

                    this.shtMeisai.ActiveCell.Tag = this.shtMeisai.ActiveCell.Text;
                    this.UpdateCheckQtyAndUnitPrice();
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                e.Cancel = true;
            }
        }

        #endregion

        #endregion

        #endregion

        #region コンボボックスの設定

        /// --------------------------------------------------
        /// <summary>
        /// コンボボックスの設定
        /// </summary>
        /// <create>K.Tsutsumi 2019/01/14</create>
        /// <update>T.Nukaga 2019/11/11 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
        /// <update>K.Tsutsumi 2020/04/26 コンボボックスの初期表示「全て(AR未出荷)」→「全て」 </update>
        /// <update>K.Tsutsumi 2020/04/26 SelectedIndex -> SelectedValue </update>
        /// <update>N.Ikari 2022/05/31 STEP14 </update>
        /// --------------------------------------------------
        private void SetComboBox()
        {
            try
            {
                ConnT01 connT01 = new ConnT01();

                DataSet ds = connT01.GetBukkenName();
                DataTable dt = ds.Tables[Def_M_PROJECT.Name];

                this.SuspendLayout();
                if (UtilData.ExistsData(ds, Def_M_PROJECT.Name))
                {
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
                    dt.AcceptChanges();

                    this.cboBukkenName.DisplayMember = Def_M_PROJECT.BUKKEN_NAME;
                    this.cboBukkenName.ValueMember = Def_M_PROJECT.PROJECT_NO;
                    this.cboBukkenName.DataSource = dt;

                    this.cboBukkenName.SelectedValue = Resources.cboAll;
                }

                this.MakeCmbBox(this.cboCond, TEHAI_SKS_RENKEI_FLAG.GROUPCD);
                if (this.Modal)
                {
                    this.cboCond.SelectedValue = Resources.cboAll;
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

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディション取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondT01 GetCondition()
        {
            var cond = new CondT01(this.UserInfo);
            if (this.cboCond.SelectedValue != null)
            {
                cond.TehaiSKSRenkeiFlag = this.cboCond.SelectedValue.ToString();
            }
            if (this.cboBukkenName.Text != ComDefine.COMBO_ALL_DISP && this.cboBukkenName.SelectedValue != null)
            {
                cond.ProjectNo = this.cboBukkenName.SelectedValue.ToString();
            }
            if (!string.IsNullOrEmpty(this.txtEcsNo.Text))
            {
                cond.EcsNo = this.txtEcsNo.Text;
            }
            if (!string.IsNullOrEmpty(this.txtSeiban.Text))
            {
                cond.Seiban = this.txtSeiban.Text;
            }
            if (!string.IsNullOrEmpty(this.txtCode.Text))
            {
                cond.Code = this.txtCode.Text;
            }
            if (!string.IsNullOrEmpty(this.txtZumenKeishiki.Text))
            {
                cond.ZumenKeishiki = this.txtZumenKeishiki.Text;
            }
            if (!string.IsNullOrEmpty(this.txtTehaiNo.Text))
            {
                cond.TehaiNo = this.txtTehaiNo.Text;
            }
            return cond;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切り替え
        /// </summary>
        /// <param name="mode">画面表示モード</param>
        /// <create>H.Tajimi 2019/01/18</create>
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
                        this.grpOperation.Enabled = false;
                        this.shtResult.Enabled = false;
                        this.ChangeDetailContols(mode);
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.cboCond.Focus();
                        break;
                    case DisplayMode.EndSearch:
                        // ----- 検索直後 -----
                        this.grpSearch.Enabled = false;
                        this.grpOperation.Enabled = true;
                        this.shtResult.Enabled = true;
                        this.ChangeDetailContols(mode);
                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.shtResult.Focus();
                        break;
                    case DisplayMode.Watari:
                    case DisplayMode.MatomeAll:
                    case DisplayMode.Matome:
                    case DisplayMode.EstimateAll:
                    case DisplayMode.Other:
                        // ----- わたり発注ボタン押下 -----
                        // ----- まとめ発注(一括)ボタン押下 -----
                        // ----- まとめ発注(単価)ボタン押下 -----
                        // ----- 見積もり都合(一括)ボタン押下 -----
                        // ----- その他ボタン押下 -----
                        this.grpSearch.Enabled = false;
                        this.grpOperation.Enabled = false;
                        this.shtResult.Enabled = false;
                        this.ChangeDetailContols(mode);
                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = true;
                        this.shtMeisai.Focus();
                        break;
                }
                this._displayMode = mode;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シートクリア

        /// --------------------------------------------------
        /// <summary>
        /// シートクリア
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear(Sheet sheet)
        {
            try
            {
                if (sheet.Name == this.shtResult.Name)
                {
                    if (this._dtTehaiMeisai != null)
                    {
                        this._dtTehaiMeisai.Dispose();
                        this._dtTehaiMeisai = null;
                    }
                }
                if (sheet.Name == this.shtMeisai.Name)
                {
                    if (this._dtTehaiSKS != null)
                    {
                        this._dtTehaiSKS.Dispose();
                        this._dtTehaiSKS = null;
                    }
                }
                // グリッドクリア
                sheet.Redraw = false;
                // 最も左上に表示されているセルの設定
                if (0 < sheet.MaxRows)
                {
                    sheet.TopLeft = new Position(0, sheet.TopLeft.Row);
                }
                sheet.DataSource = null;
                sheet.MaxRows = 0;
                sheet.Enabled = false;
            }
            finally
            {
                sheet.Redraw = true;
            }
        }

        #endregion

        #region SKS手配明細部のコントロール制御

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細部のコントロール制御
        /// </summary>
        /// <param name="mode">画面表示モード</param>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeDetailContols(DisplayMode mode)
        {
            // テキストボックスのEnabled状態変更
            Action<DSWControl.DSWNumericTextBox, bool> actChangeTextEnabled = (numTextBox, enabled) =>
            {
                if (!enabled)
                {
                    numTextBox.Clear();
                }
                numTextBox.Enabled = enabled;
                numTextBox.ReadOnly = !enabled;
            };

            // シートの指定列のEnabled状態変更
            Action<int, bool> actChangeSheetEnabled = (colIndex, enabled) =>
            {
                try
                {
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.Columns[colIndex].Enabled = enabled;
                    this.shtMeisai.Columns[colIndex].Lock = !enabled;
                }
                finally
                {
                    this.shtMeisai.Redraw = true;
                }
            };

            try
            {
                switch (mode)
                {
                    case DisplayMode.Watari:
                        // ----- わたり発注ボタン押下 -----
                        this.grpDetail.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        actChangeSheetEnabled(MEISAI_SHEET_COL_TEHAI_NO, true);
                        actChangeSheetEnabled(MEISAI_SHEET_COL_KENPIN_UMU, true);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, true);
                        actChangeTextEnabled(this.numShukkaQty, true);
                        actChangeTextEnabled(this.numTehaiUnitPrice, false);
                        break;
                    case DisplayMode.MatomeAll:
                        // ----- まとめ発注(一括)ボタン押下 -----
                        this.grpDetail.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        actChangeSheetEnabled(MEISAI_SHEET_COL_TEHAI_NO, true);
                        actChangeSheetEnabled(MEISAI_SHEET_COL_KENPIN_UMU, false);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, false);
                        actChangeTextEnabled(this.numShukkaQty, false);
                        actChangeTextEnabled(this.numTehaiUnitPrice, false);
                        break;
                    case DisplayMode.Matome:
                        // ----- まとめ発注(単価)ボタン押下 -----
                        this.grpDetail.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = false;
                        actChangeSheetEnabled(MEISAI_SHEET_COL_TEHAI_NO, false);
                        actChangeSheetEnabled(MEISAI_SHEET_COL_KENPIN_UMU, false);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, false);
                        actChangeTextEnabled(this.numShukkaQty, false);
                        actChangeTextEnabled(this.numTehaiUnitPrice, true);
                        break;
                    case DisplayMode.EstimateAll:
                        // ----- 見積もり都合(一括)ボタン押下 -----
                        this.grpDetail.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        actChangeSheetEnabled(MEISAI_SHEET_COL_TEHAI_NO, true);
                        actChangeSheetEnabled(MEISAI_SHEET_COL_KENPIN_UMU, false);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, false);
                        actChangeTextEnabled(this.numShukkaQty, false);
                        actChangeTextEnabled(this.numTehaiUnitPrice, false);
                        break;
                    case DisplayMode.Other:
                        // ----- その他ボタン押下 -----
                        this.grpDetail.Enabled = true;
                        this.shtMeisai.Enabled = true;
                        this.shtMeisai.AllowUserToAddRows = true;
                        actChangeSheetEnabled(MEISAI_SHEET_COL_TEHAI_NO, true);
                        actChangeSheetEnabled(MEISAI_SHEET_COL_KENPIN_UMU, false);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, true);
                        actChangeTextEnabled(this.numShukkaQty, true);
                        actChangeTextEnabled(this.numTehaiUnitPrice, false);
                        break;
                    default:
                        this.SheetClear(this.shtMeisai);
                        actChangeTextEnabled(this.numCheckQty, false);
                        actChangeTextEnabled(this.numHacchuQty, false);
                        actChangeTextEnabled(this.numShukkaQty, false);
                        actChangeTextEnabled(this.numTehaiUnitPrice, false);
                        this.shtMeisai.AllowUserToAddRows = false;
                        this.shtMeisai.Enabled = false;
                        this.grpDetail.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region いずれかの検索条件が設定されているか

        /// --------------------------------------------------
        /// <summary>
        /// いずれかの検索条件が設定されているか
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool HasAnyCondition()
        {
            if (this.cboBukkenName.Text != ComDefine.COMBO_ALL_DISP
                && this.cboBukkenName.SelectedValue != null)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(this.txtEcsNo.Text))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(this.txtSeiban.Text))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(this.txtCode.Text))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(this.txtZumenKeishiki.Text))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(this.txtTehaiNo.Text))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region チェックが付けられている手配明細取得

        /// --------------------------------------------------
        /// <summary>
        /// チェックが付けられている手配明細取得
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetCheckedTehaiMeisai()
        {
            var dtTehaiMeisai = this.shtResult.DataSource as DataTable;
            if (dtTehaiMeisai == null)
            {
                return null;
            }

            var ret = dtTehaiMeisai.Clone();
            foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
            {
                if (ComFunc.GetFldToBool(drTehaiMeisai, ComDefine.FLD_SELECT_CHK))
                {
                    var dr = ret.NewRow();
                    dr.ItemArray = drTehaiMeisai.ItemArray;
                    ret.Rows.Add(dr);
                }
            }
            return ret;
        }

        #endregion

        #region セルの値が変化したかどうか

        /// --------------------------------------------------
        /// <summary>
        /// セルの値が変化したかどうか
        /// </summary>
        /// <param name="cell">セル</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool CellTextChanged(Cell cell)
        {
            var current = cell.Text;
            var previous = cell.Tag;

            if (string.IsNullOrEmpty(current))
            {
                if (previous == null || string.IsNullOrEmpty(previous.ToString()))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (previous == null || string.IsNullOrEmpty(previous.ToString()))
                {
                    return true;
                }
                return !this.IsSameTehaiNo(previous.ToString(), current);
            }
        }

        #endregion

        #region 手配Noの比較

        /// --------------------------------------------------
        /// <summary>
        /// 手配Noの比較
        /// </summary>
        /// <param name="value1">手配No1</param>
        /// <param name="value2">手配No2</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool IsSameTehaiNo(string tehaiNo1, string tehaiNo2)
        {
            return tehaiNo1.Trim() == tehaiNo2.Trim();
        }

        #endregion

        #region 検品有無のデフォルトチェック状態設定

        /// --------------------------------------------------
        /// <summary>
        /// 検品有無のデフォルトチェック状態設定
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetDefaultKenpinUmu(int rowIndex)
        {
            if (this.shtMeisai.Rows.Count < rowIndex)
            {
                return;
            }

            switch (this._displayMode)
            {
                case DisplayMode.MatomeAll:
                case DisplayMode.Matome:
                case DisplayMode.EstimateAll:
                case DisplayMode.Other:
                    this.shtMeisai[MEISAI_SHEET_COL_KENPIN_UMU, rowIndex].Value = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region SKS手配明細用の値設定、更新

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細用の値設定
        /// ※下段グリッド横の発注数、出荷数
        /// </summary>
        /// <create>H.Tajimi 2019/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetHacchuAndShukkaQty()
        {
            if (this._displayMode != DisplayMode.Watari
             && this._displayMode != DisplayMode.Other)
            {
                return;
            }

            this.numHacchuQty.Value = ComFunc.GetFldToDecimal(this._dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.HACCHU_QTY, 0m);
            this.numShukkaQty.Value = ComFunc.GetFldToDecimal(this._dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.SHUKKA_QTY, 0m);
        }

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細用の値更新
        /// ※下段グリッド横の数量チェック対象、単価
        /// </summary>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void UpdateCheckQtyAndUnitPrice()
        {
            var dtMeisai = this.shtMeisai.DataSource as DataTable;
            if (dtMeisai == null || dtMeisai.Rows.Count < 1)
            {
                this.numCheckQty.Clear();
                this.numTehaiUnitPrice.Clear();
                return;
            }

            this.UpdateCheckQty(dtMeisai);
            this.UpdateTehaiUnitPrice(dtMeisai);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 数量チェック対象更新
        /// </summary>
        /// <param name="dtMeisai">SKS手配明細</param>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void UpdateCheckQty(DataTable dtMeisai)
        {
            if (this._displayMode != DisplayMode.Other)
            {
                this.numCheckQty.Clear();
                return;
            }

            decimal qty = 0m;
            foreach (DataRow drMeisai in dtMeisai.Rows)
            {
                qty += ComFunc.GetFldToDecimal(drMeisai, Def_T_TEHAI_SKS.TEHAI_QTY, 0m);
            }
            this.numCheckQty.Value = qty;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 単価更新
        /// </summary>
        /// <param name="dtMeisai">SKS手配明細</param>
        /// <create>H.Tajimi 2019/01/18</create>
        /// <update></update>
        /// --------------------------------------------------
        private void UpdateTehaiUnitPrice(DataTable dtMeisai)
        {
            if (this._displayMode != DisplayMode.Watari
             && this._displayMode != DisplayMode.Matome
             && this._displayMode != DisplayMode.EstimateAll
             && this._displayMode != DisplayMode.Other)
            {
                this.numTehaiUnitPrice.Clear();
                return;
            }

            decimal unitPrice = 0m;
            if (this._displayMode == DisplayMode.Watari)
            {
                foreach (DataRow drMeisai in dtMeisai.Rows)
                {
                    unitPrice += ComFunc.GetFldToDecimal(drMeisai, Def_T_TEHAI_SKS.TEHAI_UNIT_PRICE, 0m);
                }
            }
            else if (this._displayMode == DisplayMode.EstimateAll)
            {
                unitPrice = ComFunc.GetFldToDecimal(dtMeisai, 0, Def_T_TEHAI_SKS.TEHAI_UNIT_PRICE, 0m);
            }
            else if (this._displayMode == DisplayMode.Other)
            {
                foreach (DataRow drMeisai in dtMeisai.Rows)
                {
                    decimal tmpUnitPrice = ComFunc.GetFldToDecimal(drMeisai, Def_T_TEHAI_SKS.TEHAI_UNIT_PRICE, 0m);
                    if (unitPrice < tmpUnitPrice)
                    {
                        unitPrice = tmpUnitPrice;
                    }
                }
            }
            this.numTehaiUnitPrice.Value = unitPrice;
        }

        #endregion

        #region 指定したものをDataTable内から検索

        #region 指定値が、対象のDataTableに存在するかどうか

        /// --------------------------------------------------
        /// <summary>
        /// 指定値が、対象のDataTableに存在するかどうか
        /// </summary>
        /// <param name="value">指定値</param>
        /// <param name="dtTarget">対象DataTable</param>
        /// <param name="columnName">対象カラム名</param>
        /// <returns>true:存在する false:存在しない</returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExistsData(string value, DataTable dtTarget, string columnName)
        {
            if (!dtTarget.Columns.Contains(columnName))
            {
                return false;
            }
            return dtTarget.AsEnumerable().Any(x => ComFunc.GetFld(x, columnName) == value);
        }

        #endregion

        #region 指定行の対象カラム名の値が、対象のDataTableに存在するかどうか

        /// --------------------------------------------------
        /// <summary>
        /// 指定行の対象カラム名の値が、対象のDataTableに存在するかどうか
        /// </summary>
        /// <param name="dr">指定行</param>
        /// <param name="dtTarget">対象DataTable</param>
        /// <param name="columnName">対象カラム名</param>
        /// <returns>true:存在する false:存在しない</returns>
        /// <create>H.Tajimi 2019/01/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ExistsData(DataRow dr, DataTable dtTarget, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName) || !dtTarget.Columns.Contains(columnName))
            {
                return false;
            }

            var value = ComFunc.GetFld(dr, columnName);
            return dtTarget.AsEnumerable().Any(x => ComFunc.GetFld(x, columnName) == value);
        }
        
        #endregion

        #region 指定値を条件に対象DataTableから取得する

        /// --------------------------------------------------
        /// <summary>
        /// 指定値を条件に対象DataTableから取得する
        /// </summary>
        /// <param name="value">指定値</param>
        /// <param name="dtTarget">対象DataTable</param>
        /// <param name="columnName">対象カラム名</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataRow GetFirstRow(string value, DataTable dtTarget, string columnName)
        {
            if (!dtTarget.Columns.Contains(columnName))
            {
                return null;
            }
            return dtTarget.AsEnumerable().FirstOrDefault(x => ComFunc.GetFld(x, columnName) == value);
        }

        #endregion

        #endregion

        #region SKS手配明細に存在するデータを取得

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細に存在するデータを
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetExistingTehaiSKS(Range[] ranges)
        {
            var dt = this._dtTehaiSKS.Clone();
            for (int i = 0; i < ranges.Length; i++)
            {
                for (int j = ranges[i].TopRow; j <= ranges[i].BottomRow; j++)
                {
                    var tehaiNo = this.shtMeisai[MEISAI_SHEET_COL_TEHAI_NO, j].Text;
                    var dr = this.GetFirstRow(tehaiNo, this._dtTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO);
                    if (dr != null)
                    {
                        dt.Rows.Add(dr.ItemArray);
                    }
                }
            }
            return dt;
        }

        #endregion

        #region スキーマ取得

        #region 手配明細

        /// --------------------------------------------------
        /// <summary>
        /// 手配明細
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetShemeTehaiMeisai()
        {
            var dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            dt.Columns.Add(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_MEISAI.HACCHU_QTY);
            dt.Columns.Add(Def_T_TEHAI_MEISAI.SHUKKA_QTY);
            dt.Columns.Add(Def_T_TEHAI_MEISAI.UNIT_PRICE);
            dt.Columns.Add(Def_T_TEHAI_MEISAI.VERSION, typeof(object));
            return dt;
        }

        #endregion

        #region SKS手配明細の新規行

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細の新規行
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeInsertData()
        {
            var dt = new DataTable(ComDefine.DTTBL_INSERT);
            dt.Columns.Add(Def_T_TEHAI_SKS_WORK.TEHAI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_SKS_WORK.CREATE_DATE, typeof(object));
            dt.Columns.Add(Def_T_TEHAI_SKS.KENPIN_UMU, typeof(string));
            return dt;
        }

        #endregion

        #region SKS手配明細の変更行

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細の変更行
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private DataTable GetSchemeUpdateData()
        {
            var dt = new DataTable(ComDefine.DTTBL_UPDATE);
            dt.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
            dt.Columns.Add(Def_T_TEHAI_SKS.VERSION, typeof(object));
            dt.Columns.Add(Def_T_TEHAI_SKS.KENPIN_UMU, typeof(string));
            return dt;
        }

        #endregion

        #endregion

        #region 発注数取得

        /// --------------------------------------------------
        /// <summary>
        /// 発注数取得
        /// ※DB保存時の値をモードにより切り分ける
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private object GetHacchuQty()
        {
            switch (this._displayMode)
            {
                case DisplayMode.Watari:
                case DisplayMode.Other:
                    return this.numHacchuQty.Value;
                default:
                    return DBNull.Value;
            }
        }

        #endregion

        #region 出荷数取得

        /// --------------------------------------------------
        /// <summary>
        /// 出荷数取得
        /// ※DB保存時の値をモードにより切り分ける
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update></update>
        /// --------------------------------------------------
        private object GetShukkaQty()
        {
            switch (this._displayMode)
            {
                case DisplayMode.Watari:
                case DisplayMode.Other:
                    return this.numShukkaQty.Value;
                default:
                    return DBNull.Value;
            }
        }

        #endregion

        #region 単価取得

        /// --------------------------------------------------
        /// <summary>
        /// 単価取得
        /// ※DB保存時の値をモードにより切り分ける
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/22</create>
        /// <update>T.Nukaga 2022/02/26 引数追加、わたり発注時、見積が1件でもあれば更新対象外になるように値設定</update>
        /// --------------------------------------------------
        private object GetUnitPrice(bool isExistMitsumori)
        {
            switch (this._displayMode)
            {
                case DisplayMode.Watari:
                    if (isExistMitsumori)
                    {
                        return DBNull.Value;
                    }
                    return this.numTehaiUnitPrice.Value;
                case DisplayMode.Matome:
                case DisplayMode.EstimateAll:
                case DisplayMode.Other:
                    return this.numTehaiUnitPrice.Value;
                default:
                    return DBNull.Value;
            }
        }

        #endregion

        #region シートの入力値のフォーマット

        /// --------------------------------------------------
        /// <summary>
        /// シートの入力値のフォーマット
        /// </summary>
        /// <param name="value">セル入力値</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/24</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetSheetInputFormat(string value)
        {
            string ret = value.Trim().ToUpper();
            return ret;
        }

        #endregion

        #region SKS手配明細WORKのデータをSKS手配明細シートに反映する

        /// --------------------------------------------------
        /// <summary>
        /// SKS手配明細WORKのデータをSKS手配明細シートに反映する
        /// </summary>
        /// <param name="dtTehaiSKSWork">SKS手配明細WORK</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/23</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool UpdateMeisaiFromTehaiSKSWork(DataTable dtTehaiSKSWork)
        {
            try
            {
                var dtMeisai = this.shtMeisai.DataSource as DataTable;
                if (dtMeisai == null)
                {
                    return false;
                }

                var tehaiNo = ComFunc.GetFld(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                var dr = dtMeisai.AsEnumerable().FirstOrDefault(x => ComFunc.GetFld(x, Def_T_TEHAI_SKS.TEHAI_NO) == tehaiNo);
                if (dr == null)
                {
                    return false;
                }

                UtilData.SetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_QTY, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.TEHAI_QTY));
                UtilData.SetFld(dr, Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE));
                UtilData.SetFld(dr, Def_T_TEHAI_SKS_WORK.KAITO_DATE, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.KAITO_DATE));
                UtilData.SetFld(dr, Def_T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG));
                UtilData.SetFld(dr, ComDefine.FLD_HACCHU_FLAG_NAME, ComFunc.GetFldObject(dtTehaiSKSWork, 0, ComDefine.FLD_HACCHU_FLAG_NAME));
                UtilData.SetFld(dr, Def_T_TEHAI_SKS.KENPIN_UMU, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS.KENPIN_UMU));
                UtilData.SetFld(dr, Def_T_TEHAI_SKS.VERSION, ComFunc.GetFldObject(dtTehaiSKSWork, 0, Def_T_TEHAI_SKS.VERSION));
                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 手配種別取得

        /// --------------------------------------------------
        /// <summary>
        /// 手配種別取得
        /// </summary>
        /// <returns></returns>
        /// <create> 2019/02/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetTehaiKindFlag()
        {
            return this.GetTehaiKindFlag(this._displayMode);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 手配種別取得
        /// </summary>
        /// <param name="mode">DisplayMode</param>
        /// <returns></returns>
        /// <create> 2019/02/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string GetTehaiKindFlag(DisplayMode mode)
        {
            if (mode == DisplayMode.Watari)
            {
                return TEHAI_KIND_FLAG.ACROSS_VALUE1;
            }
            else if (mode == DisplayMode.Matome || mode == DisplayMode.MatomeAll)
            {
                return TEHAI_KIND_FLAG.AGGRIGATE_VALUE1;
            }
            else if (mode == DisplayMode.EstimateAll)
            {
                return TEHAI_KIND_FLAG.ESTIMATE_VALUE1;
            }
            else if (mode == DisplayMode.Other)
            {
                return TEHAI_KIND_FLAG.ANOTHER_VALUE1;
            }
            else
            {
                return TEHAI_KIND_FLAG.NONE_VALUE1;
            }
        }

        #endregion
    }
}
