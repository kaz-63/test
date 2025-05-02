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
using GrapeCity.Win.ElTabelle.Editors;
using WsConnection.WebRefI01;
using SMS.E01;
using SMS.I01.Properties;

namespace SMS.I01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 在庫問合せ／メンテ
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ZaikoHoshu : SystemBase.Forms.CustomOrderForm
    {
        #region Fields

        #endregion

        #region Enum

        /// --------------------------------------------------
        /// <summary>
        /// 画面の表示モード
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private enum DisplayMode
        {
            /// --------------------------------------------------
            /// <summary>
            /// 初期
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Initialize,
            /// --------------------------------------------------
            /// <summary>
            /// 表示
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Display,
            /// --------------------------------------------------
            /// <summary>
            /// メンテ
            /// </summary>
            /// <create>T.Wakamatsu 2013/08/13</create>
            /// <update></update>
            /// --------------------------------------------------
            Maintenance,
            /// --------------------------------------------------
            /// <summary>
            /// 検索結果クリア
            /// </summary>
            /// <create>H.Tajimi 2015/12/01</create>
            /// <update></update>
            /// --------------------------------------------------
            ResultClear,
        }

        #endregion

        #region 定数
        // 入庫ロケーションのカラム位置
        private const int COL_LOCATION = 1;
        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 5;

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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        public ZaikoHoshu(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // デフォルトはUpdate
                this.EditMode = SystemBase.EditMode.Update;
                this.InitializeSheet(this.shtMeisai);
                this.MakeCmbBox(this.cboShukkaFlag, SHUKKA_FLAG.GROUPCD);

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.ZaikoHoshu_TagCode;
                shtMeisai.ColumnHeaders[1].Caption = Resources.ZaikoHoshu_StockLocation;
                shtMeisai.ColumnHeaders[2].Caption = Resources.ZaikoHoshu_Status;
                shtMeisai.ColumnHeaders[3].Caption = Resources.ZaikoHoshu_State;
                shtMeisai.ColumnHeaders[4].Caption = Resources.ZaikoHoshu_Date;
                shtMeisai.ColumnHeaders[5].Caption = Resources.ZaikoHoshu_Product;
                shtMeisai.ColumnHeaders[6].Caption = Resources.ZaikoHoshu_Code;
                shtMeisai.ColumnHeaders[7].Caption = Resources.ZaikoHoshu_DrawingAdditionalNo;
                shtMeisai.ColumnHeaders[8].Caption = Resources.ZaikoHoshu_Name;
                shtMeisai.ColumnHeaders[9].Caption = Resources.ZaikoHoshu_DrawingNoFormat;
                shtMeisai.ColumnHeaders[10].Caption = Resources.ZaikoHoshu_JpName;
                shtMeisai.ColumnHeaders[11].Caption = Resources.ZaikoHoshu_SectioningNo;
                shtMeisai.ColumnHeaders[12].Caption = Resources.ZaikoHoshu_Quantity;
                shtMeisai.ColumnHeaders[13].Caption = Resources.ZaikoHoshu_Area;
                shtMeisai.ColumnHeaders[14].Caption = Resources.ZaikoHoshu_Floor;
                shtMeisai.ColumnHeaders[15].Caption = Resources.ZaikoHoshu_Model;
                shtMeisai.ColumnHeaders[16].Caption = Resources.ZaikoHoshu_MNo;
                shtMeisai.ColumnHeaders[17].Caption = Resources.ZaikoHoshu_DeliveryDestination;
                shtMeisai.ColumnHeaders[18].Caption = Resources.ZaikoHoshu_Ship;
                shtMeisai.ColumnHeaders[19].Caption = Resources.ZaikoHoshu_STNo;
                shtMeisai.ColumnHeaders[20].Caption = Resources.ZaikoHoshu_Memo;
                shtMeisai.ColumnHeaders[21].Caption = Resources.ZaikoHoshu_ARNo;
                shtMeisai.ColumnHeaders[22].Caption = Resources.ZaikoHoshu_AssemblyDate;
                shtMeisai.ColumnHeaders[23].Caption = Resources.ZaikoHoshu_BoxNo;
                shtMeisai.ColumnHeaders[24].Caption = Resources.ZaikoHoshu_PalletNo;
                shtMeisai.ColumnHeaders[25].Caption = Resources.ZaikoHoshu_ConstructionIdentityManagementNo;
                shtMeisai.ColumnHeaders[26].Caption = Resources.ZaikoHoshu_InternalManagementKey;
                shtMeisai.ColumnHeaders[27].Caption = Resources.ZaikoHoshu_WoodFrameNo;
                shtMeisai.ColumnHeaders[28].Caption = Resources.ZaikoHoshu_BoxPackingDate;
                shtMeisai.ColumnHeaders[29].Caption = Resources.ZaikoHoshu_PalletPackingDate;
                shtMeisai.ColumnHeaders[30].Caption = Resources.ZaikoHoshu_WoodFrameDate;
                shtMeisai.ColumnHeaders[31].Caption = Resources.ZaikoHoshu_ShippingDate;
                shtMeisai.ColumnHeaders[32].Caption = Resources.ZaikoHoshu_ShippingCompany;
                shtMeisai.ColumnHeaders[33].Caption = Resources.ZaikoHoshu_InvoiceNo;
                shtMeisai.ColumnHeaders[34].Caption = Resources.ZaikoHoshu_OkurijyoNo;
                shtMeisai.ColumnHeaders[35].Caption = Resources.ZaikoHoshu_BLNo;
                shtMeisai.ColumnHeaders[36].Caption = Resources.ZaikoHoshu_AcceptanceDate;
                shtMeisai.ColumnHeaders[37].Caption = Resources.ZaikoHoshu_AcceptanceStaff;
                
                ConnI01 conn = new ConnI01();
                if (conn.GetZaikoMainteRole(this.UserInfo.RoleID))
                    this.grpMode.Enabled = true;
                else
                    this.grpMode.Enabled = false;

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
        /// <create>T.Wakamatsu 2013/08/13</create>
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

        /// --------------------------------------------------
        /// <summary>
        /// シートの初期化を行うメソッド
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <create>T.Wakamatsu 2013/09/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            try
            {
                KeyAction[] aryKeyActions = new KeyAction[1];
                aryKeyActions[0] = KeyAction.NextRow;
                shtMeisai.ShortCuts.Add(Keys.Enter, aryKeyActions);
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                // 2015/12/08 H.Tajimi クリア処理追加
                // 入力値のクリア
                this.txtBukkenName.Text = string.Empty;
                this.txtHinmeiJp.Text = string.Empty;
                this.txtARNo.Text = string.Empty;
                this.txtHinmei.Text = string.Empty;
                this.txtTagNo.Text = string.Empty;
                this.txtBoxNo.Text = string.Empty;
                this.txtPalletNo.Text = string.Empty;
                this.txtZumenKeishiki.Text = string.Empty;
                this.txtKiwakuNo.Text = string.Empty;
                this.txtCaseNo.Text = string.Empty;
                this.txtArea.Text = string.Empty;
                this.txtFloor.Text = string.Empty;
                this.txtKuwariNo.Text = string.Empty;

                this.cboShukkaFlag.SelectedValue = SHUKKA_FLAG.DEFAULT_VALUE1;
                this.cboLocation.DataSource = null;

                this.rdoToiawase.Checked = true;
                this.rdoMainte.Checked = false;
                // ↑
                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }

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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
                // TAG時
                this.shtMeisai.UpdateData();

                DataTable dt = this.shtMeisai.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    // 明細が入力されていません。
                    this.ShowMessage("A9999999028");
                    this.shtMeisai.Focus();
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
                // 物件名チェック
                if (string.IsNullOrEmpty(this.txtBukkenNo.Text))
                {
                    // 物件名を入力して下さい。
                    this.ShowMessage("I0100020005");
                    // 2015/11/30 H.Tajimi 一覧ボタン非表示のため物件名にフォーカス設定
                    this.txtBukkenName.Focus();
                    // ↑
                    return false;
                }

                // 検索用入力チェック
                if (!string.IsNullOrEmpty(this.txtTagNo.Text))
                {
                    if (this.txtTagNo.Text.Length != 5)
                    {
                        // 該当TagCodeはありません。
                        this.ShowMessage("I0100030002");
                        this.txtTagNo.Focus();
                        ret = false;
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
        /// <create>T.Wakamatsu 2013/08/13</create>
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
        /// <returns>true:検索成功/false:検索失敗</returns
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                //データ取得
                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.HinmeiJp = this.txtHinmeiJp.Text;
                cond.Hinmei = this.txtHinmei.Text;
                cond.ZumenKeishiki = this.txtZumenKeishiki.Text;
                cond.Area = this.txtArea.Text;
                cond.Floor = this.txtFloor.Text;
                cond.KuwariNo = this.txtKuwariNo.Text;
                cond.KiwakuNo = this.txtKiwakuNo.Text;
                cond.CaseNo = this.txtCaseNo.Text;
                if (!string.IsNullOrEmpty(this.txtARNo.Text))
                {
                    cond.ARNo = this.lblAR.Text + this.txtARNo.Text;
                }
                if (!string.IsNullOrEmpty(this.txtPalletNo.Text))
                {
                    cond.PalletNo = this.lblPallet.Text + this.txtPalletNo.Text.PadLeft(5, '0');
                }
                if (!string.IsNullOrEmpty(this.txtBoxNo.Text))
                {
                    cond.BoxNo = this.lblBox.Text + this.txtBoxNo.Text.PadLeft(5, '0');
                }
                cond.TagNo = this.txtTagNo.Text;
                if (this.cboLocation.SelectedIndex != -1)
                    cond.Location = this.cboLocation.SelectedValue.ToString();
                else
                    cond.Location = string.Empty;


                DataSet ds = conn.GetZaikoHoshu(cond);
                DataTable dt = ds.Tables[Def_T_STOCK.Name];

                // 対象データがない場合はメッセージ
                if (dt.Rows.Count == 0)
                {
                    // 該当する在庫データはありません。
                    this.ShowMessage("I0100050001");
                    txtArea.Focus();
                    return false;
                }

                //ロケーションコンボボックスセット
                this.SetComboLocation();

                // データ表示
                this.shtMeisai.DataSource = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    shtMeisai[COL_LOCATION, i].Text = dt.Rows[i][Def_T_STOCK.LOCATION].ToString();
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

        #region 編集内容実行

        #region 制御メソッド

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            bool ret = base.RunEdit();
            try
            {
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
        /// <create>T.Wakamatsu 2013/08/13</create>
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                bool ret = false;

                ConnI01 conn = new ConnI01();
                CondI01 cond = new CondI01(this.UserInfo);
                cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                cond.BukkenNo = this.txtBukkenNo.Text;
                cond.WorkUserID = cond.LoginInfo.UserID;

                this.shtMeisai.UpdateData();
                DataTable dtUpd = (this.shtMeisai.DataSource as DataTable).Copy();
                DataTable dtDel = dtUpd.Clone();

                foreach (DataRow dr in dtUpd.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        dr.RejectChanges();
                        dtDel.Rows.Add(dr.ItemArray);
                        dr.Delete();
                    }
                }
                dtDel.AcceptChanges();
                dtUpd.AcceptChanges();

                for (int i = 0; i < dtUpd.Rows.Count; i++)
                {
                    DataRow dr = dtUpd.Rows[i];

                    string nloc = this.shtMeisai[COL_LOCATION, i].Text;
                    // Locationに変更がなければ更新しない
                    if (ComFunc.GetFld(dr, Def_T_STOCK.LOCATION) == nloc)
                        dr.Delete();
                    else
                        dr[ComDefine.FLD_NYUKO_LOCATION] = nloc;
                }
                dtUpd.AcceptChanges();

                if (dtDel.Rows.Count == 0 && dtUpd.Rows.Count == 0)
                {
                    // 更新対象となる行がありません。
                    this.ShowMessage("I0100050002");
                    return false;
                }

                string errMsgID;
                string[] args;
                ret = conn.UpdZaikoHoshu(cond, dtUpd, dtDel, out errMsgID, out args);
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    if (ComFunc.IsVersionError(errMsgID))
                    {
                        this.RunSearch();
                    }
                    this.ShowMessage(errMsgID, args);
                    return false;
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

        #region 削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2013/08/13</create>
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F01Button_Click(object sender, EventArgs e)
        {
            // 通常はUpdate
            this.EditMode = SystemBase.EditMode.Update;
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                this.ClearMessage();

                int row = this.shtMeisai.ActivePosition.Row;
                if (this.shtMeisai.MaxRows >= 1)
                {
                    // 選択行を削除してもよろしいですか？ダイアログ表示
                    if (this.ShowMessage("I0100020008") == DialogResult.OK)
                    {
                        this.shtMeisai.RemoveRow(row, false);
                        this.shtMeisai.UpdateData();
                        this.shtMeisai.Focus();
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
        /// F6ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/12/08 System全般/Clearボタン押下時の範囲</update>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                if (this.ShowMessage("A9999999053") != DialogResult.OK) return;

                this.cboLocation.DataSource = null;
                this.ChangeMode(DisplayMode.ResultClear);
                this.txtArea.Focus();
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
                if (this.ShowMessage("A9999999001") != DialogResult.OK) return;
                this.DisplayClear();

                this.cboShukkaFlag.Focus();
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
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/12/03 物件名をDBから取得するよう変更</update>
        /// <update>K.Tsutsumi 2019/02/06 64 bit OSにてF10イベントが２回発生する不具合を対応</update>
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
                    frm.FileName = ComDefine.EXCEL_FILE_ZAIKO;
                    if (0 < this.shtMeisai.MaxRows && frm.ShowDialog() != DialogResult.OK) return;
                    // Excel出力処理
                    DataTable dtExport = (this.shtMeisai.DataSource as DataTable).Copy();

                    ExportZaikoList export = new ExportZaikoList();
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
        #endregion

        #region グリッドクリック
        /// --------------------------------------------------
        /// <summary>
        /// グリッドクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void shtMeisai_CellNotify(object sender, CellNotifyEventArgs e)
        {
            try
            {
                this.shtMeisai.CellPosition = e.Position;
                //セルのイベント処理です
                switch (e.Name)
                {
                    case CellNotifyEvents.CheckedChanged:
                        break;
                    case CellNotifyEvents.SelectedIndexChanged:
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

        #region ボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/11/30 納入先選択のUI改善</update>
        /// <update></update>
        /// --------------------------------------------------
        private void btnDisp_Click(object sender, EventArgs e)
        {
            this.ClearMessage();

            // シートクリア
            this.ChangeMode(DisplayMode.Initialize);

            // 2015/11/30 H.Tajimi 物件名一覧表示
            if (!this.ShowBukkenMeiIchiran())
            {
                this.txtBukkenName.Focus();
                return;
            }
            // ↑

            // 成功した場合検索部を操作不可に
            if (rdoToiawase.Checked)
                this.ChangeMode(DisplayMode.Display);
            else
                this.ChangeMode(DisplayMode.Maintenance);
        }

        #endregion

        #region コンボボックス変更

        /// --------------------------------------------------
        /// <summary>
        /// 出荷区分コンボボックス変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void cboShukkaFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBukkenNo.Text = string.Empty;
            this.txtBukkenName.Text = string.Empty;

            DSWComboBox cbo = sender as DSWComboBox;
            if (cbo.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                txtARNo.Enabled = true;
            }
            else
            {
                txtARNo.Text = string.Empty;
                txtARNo.Enabled = false;
            }
        }

        #endregion

        #region フォーカスアウト

        /// --------------------------------------------------
        /// <summary>
        /// Box No.のフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtBoxNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtBoxNo.Text))
            {
                // 字埋め
                this.txtBoxNo.Text = this.txtBoxNo.Text.PadLeft(5, '0');
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// Pallet No.のフォーカスアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void txtPalletNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPalletNo.Text))
            {
                // 字埋め
                this.txtPalletNo.Text = this.txtPalletNo.Text.PadLeft(5, '0');
            }
        }

        #endregion

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

        #endregion

        #region ロケーションコンボボックスセット

        /// --------------------------------------------------
        /// <summary>
        /// ロケーションコンボボックスセット
        /// </summary>
        /// <create>T.Wakamatsu 2013/09/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetComboLocation()
        {
            // 物件に紐付くロケーションをコンボボックスにセット
            ConnI01 conn = new ConnI01();
            CondI01 cond = new CondI01(this.UserInfo);
            cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            cond.BukkenNo = this.txtBukkenNo.Text;

            DataSet ds = conn.GetLocationCombo(cond);
            string tableName = Def_M_LOCATION.Name;

            DataTable dt = ds.Tables[tableName];

            // シートのコンボボックスセット
            List<string> items = new List<string>();
            items.Add(string.Empty);
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(ComFunc.GetFld(dr, Def_M_LOCATION.LOCATION));
            }

            ComboBoxEditor cbo = ElTabelleSheetHelper.NewComboBoxEditor(false, items.ToArray());
            this.shtMeisai.Columns[COL_LOCATION].Editor = cbo;
        }

        #endregion

        #region モード切り替え

        /// --------------------------------------------------
        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode">画面の表示モード</param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update>H.Tajimi 2015/12/02 System全般/Clearボタン押下時の範囲</update>
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
                        ChangeModeCond(true);

                        this.shtMeisai.DataSource = null;
                        this.shtMeisai.MaxRows = 0;
                        this.shtMeisai.Enabled = false;

                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア無効
                        this.fbrFunction.F06Button.Enabled = false;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = false;
                        // 2015/11/30 H.Tajimi 物件名は入力可
                        this.txtBukkenName.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.Display:
                        // ----- 問合せ時 -----
                        ChangeModeCond(true);
                        this.shtMeisai.Columns[COL_LOCATION].Enabled = false;
                        this.shtMeisai.Enabled = true;

                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = true;
                        // 2015/11/30 H.Tajimi 物件名は入力可
                        this.txtBukkenName.Enabled = true;
                        // ↑
                        break;
                    case DisplayMode.Maintenance:
                        // ----- メンテ時 -----
                        ChangeModeCond(false);
                        this.shtMeisai.Columns[COL_LOCATION].Enabled = true;
                        this.shtMeisai.Enabled = true;

                        this.fbrFunction.F01Button.Enabled = true;
                        this.fbrFunction.F03Button.Enabled = true;
                        // 2015/12/02 H.Tajimi クリア有効
                        this.fbrFunction.F06Button.Enabled = true;
                        // ↑
                        this.fbrFunction.F10Button.Enabled = false;
                        // 2015/11/30 H.Tajimi 物件名は入力不可
                        this.txtBukkenName.Enabled = false;
                        // ↑
                        break;
                    // 2015/12/02 H.Tajimi 検索結果クリア時のシート、ボタン制御
                    case DisplayMode.ResultClear:
                        // ----- 検索結果クリア時 -----
                        ChangeModeCond(true);
                        this.SheetClear();

                        this.fbrFunction.F01Button.Enabled = false;
                        this.fbrFunction.F03Button.Enabled = false;
                        this.fbrFunction.F06Button.Enabled = false;
                        this.fbrFunction.F10Button.Enabled = false;
                        this.txtBukkenName.Enabled = true;
                        break;
                    // ↑
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索条件モード切替
        /// </summary>
        /// <param name="enabled">検索条件部分の使用可フラグ</param>
        /// <create>T.Wakamatsu 2013/08/13</create>
        /// <update></update>
        /// --------------------------------------------------
        private void ChangeModeCond(bool enabled)
        {
            this.rdoToiawase.Enabled = enabled;
            this.rdoMainte.Enabled = enabled;

            this.cboShukkaFlag.Enabled = enabled;
            this.btnBukkenIchiran.Enabled = enabled;
            this.btnDisp.Enabled = enabled;

            this.txtHinmeiJp.Enabled = enabled;
            this.txtHinmei.Enabled = enabled;
            this.txtZumenKeishiki.Enabled = enabled;
            this.txtArea.Enabled = enabled;
            this.txtFloor.Enabled = enabled;
            this.txtKuwariNo.Enabled = enabled;


            if (cboShukkaFlag.SelectedValue!= null && cboShukkaFlag.SelectedValue.ToString() == SHUKKA_FLAG.AR_VALUE1)
            {
                this.txtARNo.Enabled = enabled;
            }
            else
            {
                this.txtARNo.Enabled = false;
            }
            this.txtPalletNo.Enabled = enabled;
            this.txtBoxNo.Enabled = enabled;
            this.txtTagNo.Enabled = enabled;
            this.txtKiwakuNo.Enabled = enabled;
            this.txtCaseNo.Enabled = enabled;
            this.cboLocation.Enabled = enabled;
        }

        #endregion

        #region 画面表示

        #region 物件名一覧画面表示

        /// --------------------------------------------------
        /// <summary>
        /// 物件名一覧表示
        /// </summary>
        /// <returns></returns>
        /// <create>H.Tajimi 2015/11/30</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowBukkenMeiIchiran()
        {
            string shukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
            string bukkenName = this.txtBukkenName.Text;
            using (var frm = new SMS.P02.Forms.BukkenMeiIchiran(this.UserInfo, shukkaFlag, bukkenName, true))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    // 選択データを設定
                    this.txtBukkenNo.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                    this.txtBukkenName.Text = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NAME);

                    // 物件に紐付くロケーションをコンボボックスにセット
                    ConnI01 conn = new ConnI01();
                    CondI01 cond = new CondI01(this.UserInfo);
                    cond.ShukkaFlag = this.cboShukkaFlag.SelectedValue.ToString();
                    cond.BukkenNo = this.txtBukkenNo.Text;

                    DataSet ds = conn.GetLocationCombo(cond);
                    string tableName = Def_M_LOCATION.Name;

                    DataTable dt = ds.Tables[tableName];
                    MakeCmbBox(this.cboLocation, dt, Def_M_LOCATION.LOCATION, Def_M_LOCATION.LOCATION, string.Empty, true);
                    return this.RunSearch();
                }
            }
            return false;
        }

        #endregion

        #endregion

    }
}
