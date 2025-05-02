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

using WsConnection.WebRefK02;
using SMS.K02.Properties;

namespace SMS.K02.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細登録
    /// </summary>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class ShagaiKiwakuKonpoMeisai : KiwakuKonpoMeisai
    {
        #region Fields
        /// --------------------------------------------------
        /// <summary>
        /// シートで入力エラーがあったかどうか
        /// </summary>
        /// <create>Y.Higuchi 2010/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool _isSheetLeaveEditError = false;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 現品TagNoの長さ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int LEN_GENPINTAGNO = 10;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <param name="kojiNo">工事識別NO</param>
        /// <param name="caseID">内部管理用キー</param>
        /// <create>Y.Higuchi 2010/07/30</create>
        /// <update></update>
        /// --------------------------------------------------
        public ShagaiKiwakuKonpoMeisai(UserInfo userInfo, string kojiNo, string caseID)
            : base(userInfo, kojiNo, caseID)
        {
            InitializeComponent();
            this.Title = ComDefine.TITLE_K0200070;
        }

        #endregion

        #region Properties

        /// --------------------------------------------------
        /// <summary>
        /// 登録区分
        /// </summary>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override string TorokuFlag
        {
            get
            {
                return TOROKU_FLAG.GAI_VALUE1;
            }
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // ----- メッセージの設定 -----
                // {0}行目の現品TAGNo.は10桁に満たないので10桁入力して下さい。
                this.SetDispMessageID(DispMessageType.CheckInputLen, "K0200070003");
                // 現品TAGNo.をクリアします。\r\nよろしいですか？
                this.SetDispMessageID(DispMessageType.ClearQuestion, "K0200070002");
                // {0}行目の現品TAGNo.[{1}]は存在しています。
                this.SetDispMessageID(DispMessageType.CheckDuplication, "K0200070004");
                // 他端末で更新された為、更新できませんでした。
                this.SetDispMessageID(DispMessageType.UupdateVersionError, "A9999999027");

                // シートのタイトルを設定
                shtMeisai.ColumnHeaders[0].Caption = Resources.ShagaiKiwakuKonpoMeisai_MarkingTagNo;
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
        /// <create>Y.Higuchi 2010/08/03</create>
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

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
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
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                return base.RunSearchExec();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 編集内容実行

        #region 修正処理

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                return base.RunEditUpdate();
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
        /// <create>Y.Higuchi 2010/08/03</create>
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
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F03Button_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            base.fbrFunction_F03Button_Click(sender, e);
            try
            {
                // 行がないか行追加時の最終行の場合は処理を抜ける。
                if (this.shtMeisai.Rows.Count < 1 ||
                            (this.shtMeisai.AllowUserToAddRows &&
                            this.shtMeisai.Rows.Count - 1 == this.shtMeisai.ActivePosition.Row))
                {
                    return;
                }
                // 現品TagNo.[{0}]を削除してもよろしいですか？
                string tagNo = this.shtMeisai[0, this.shtMeisai.ActivePosition.Row].Text;
                if (this.ShowMessage("K0200070001", tagNo) == DialogResult.OK)
                {
                    this.shtMeisai.Redraw = false;
                    this.shtMeisai.RemoveRow(this.shtMeisai.ActivePosition.Row, false);
                    this.shtMeisai.Redraw = true;
                }
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
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F04Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F04Button_Click(sender, e);
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
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/03</create>
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

        #region 明細グリッド

        #region LeaveEdit

        /// --------------------------------------------------
        /// <summary>
        /// セルが非編集モードに入る場合に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void shtMeisai_LeaveEdit(object sender, LeaveEditEventArgs e)
        {
            try
            {
                // Enter押下時にEndEdit→NextCellWithWrapとしているのでEndEditでe.Cancel=trueにすると
                // 2回LeaveEditが実行されるのでエラーが2回出てしまう対応

                // EndEditであればエラーを初期化
                if (e.MoveStatus == MoveStatus.NoAction)
                {
                    this._isSheetLeaveEditError = false;
                }

                // EndEditでキャンセルした場合はNextCellWithWrapもキャンセルしチェック処理を呼ばない
                if (this._isSheetLeaveEditError)
                {
                    e.Cancel = true;
                    return;
                }

                base.shtMeisai_LeaveEdit(sender, e);
                if (e.Cancel)
                {
                    // キャンセルされていた場合はエラーとする。
                    this._isSheetLeaveEditError = true;
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

        #region メッセージIDチェック

        /// --------------------------------------------------
        /// <summary>
        /// 画面を再描画させる必要のあるメッセージかチェック
        /// </summary>
        /// <param name="messageID">メッセージID</param>
        /// <returns>true:必要/false:不要</returns>
        /// <create>Y.Higuchi 2010/08/02</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool IsRefresh(string messageID)
        {
            List<string> msgs = new List<string>();
            msgs.Add(this.DispMessages[DispMessageType.UupdateVersionError]);

            if (msgs.Contains(messageID))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region グリッドで使用するデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// グリッドで使用するデータテーブル
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override DataTable GetSchemeSheetDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(FLD_TARGET_NO, typeof(string));
            return dt;
        }

        #endregion

        #region シートの入力値のフォーマット

        /// --------------------------------------------------
        /// <summary>
        /// シートの入力値のフォーマット
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override string GetSheetInputFormat(string value)
        {
            return value;
        }

        #endregion

        #region グリッドの行入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの行入力チェック
        /// </summary>
        /// <param name="targetRow">入力された行インデックス</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>H.Tajimi 2019/01/08 木枠梱包業務改善</update>
        /// --------------------------------------------------
        protected override bool CheckSheetLine(int targetRow)
        {
            try
            {
                string shukkaFlag = string.Empty;
                string nonyusakiCD = string.Empty;
                string tagNo = string.Empty;
                for (int i = 0; i < this.shtMeisai.MaxRows; i++)
                {
                    string inputString = this.shtMeisai[COL_TARGET_NO, i].Text;
                    if (i == targetRow || string.IsNullOrEmpty(inputString)) continue;
                    if (string.IsNullOrEmpty(shukkaFlag) || string.IsNullOrEmpty(nonyusakiCD))
                    {
                        ComFunc.AnalyzeGenpinTagNo(inputString, out shukkaFlag, out nonyusakiCD, out tagNo);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(shukkaFlag) || string.IsNullOrEmpty(nonyusakiCD)) return true;
                string inputShukkaFlag;
                string inputNonyusakiCD;
                string inputTagNo;
                string targetString = this.shtMeisai.ActiveCell.Text;
                ComFunc.AnalyzeGenpinTagNo(targetString, out inputShukkaFlag, out inputNonyusakiCD, out inputTagNo);
                if (shukkaFlag != inputShukkaFlag)
                {
                    // 入力済みの現品TAGNo.と出荷区分(現品TAGNo.の1桁目)が違う為、入力できません。
                    this.ShowMessage("K0200070005");
                    return false;
                }
                if (this.UserInfo.SysInfo.KiwakuKonpoMoveShipFlag != KIWAKU_KONPO_MOVE_SHIP.ENABLE_VALUE1)
                {
                    if (nonyusakiCD != inputNonyusakiCD)
                    {
                        // 入力済みの現品TAGNo.と納入先が違う為、入力できません。
                        this.ShowMessage("K0200070006");
                        return false;
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

        #region グリッドの1行目の入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの1行目の入力チェック
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update>H.Tajimi 2015/11/24 M-No対応</update>
        /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
        /// --------------------------------------------------
        protected override bool CheckSheetLineOne()
        {
            try
            {
                CondK02 cond = new CondK02(this.UserInfo);
                ConnK02 conn = new ConnK02();
                string shukkaFalg;
                string nonyusakiCD;
                string tagNo;

                ComFunc.AnalyzeGenpinTagNo(this.shtMeisai.ActiveCell.Text, out shukkaFalg, out nonyusakiCD, out tagNo);
                cond.ShukkaFlag = shukkaFalg;
                cond.NonyusakiCD = nonyusakiCD;
                cond.TagNo = tagNo;
                DataSet ds = conn.GetKiwakuKonpoShukkaMeisaiFirstRowData(cond);

                // 1行目で入力した内容と木枠の納入先/便を比較
                if (!this.CheckSheetListOneDiffKiwakuAndShukka(ds, this.shtMeisai.ActiveCell.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.txtItem.Text))
                {
                    this.txtItem.Text = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KISHU);
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

        #region グリッドの入力値の長さチェック

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの入力値の長さチェック
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckSheetInputLength(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length < LEN_GENPINTAGNO)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region グリッドの表示データ作成

        /// --------------------------------------------------
        /// <summary>
        /// グリッドの表示データ作成
        /// </summary>
        /// <param name="dt">表示するデータ</param>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void SetSheetDispData(DataTable dt)
        {
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = this.DtMeisai.NewRow();
                string shukkaFlag = ComFunc.GetFld(item, Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG);
                string nonyusakiCD = ComFunc.GetFld(item, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD);
                string tagNo = ComFunc.GetFld(item, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO);
                dr[FLD_TARGET_NO] = ComFunc.GetGenpinTagNo(shukkaFlag, nonyusakiCD, tagNo);

                this.DtMeisai.Rows.Add(dr);
            }
        }

        #endregion

        #region 画面表示内容取得

        /// --------------------------------------------------
        /// <summary>
        /// 画面表示内容取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/03</create>
        /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
        /// --------------------------------------------------
        protected override DataSet GetDisplayData()
        {
            DataTable dt = this.GetSchemeKiwakuMeisai();
            DataRow dr = dt.NewRow();
            dr[Def_T_KIWAKU_MEISAI.KOJI_NO] = this.KojiNo;
            dr[Def_T_KIWAKU_MEISAI.CASE_ID] = this.CaseID;
            dr[Def_T_KIWAKU_MEISAI.ITEM] = this.txtItem.Text;
            dr[Def_T_KIWAKU_MEISAI.DESCRIPTION_1] = this.txtDescription1.Text;
            dr[Def_T_KIWAKU_MEISAI.DESCRIPTION_2] = this.txtDescription2.Text;
            dr[Def_T_KIWAKU_MEISAI.NET_W] = this.txtNetW.Value;
            dr[Def_T_KIWAKU_MEISAI.GROSS_W] = this.nudGrossW.Value;
            dr[Def_T_KIWAKU_MEISAI.VERSION] = ComFunc.GetFldObject(this.DsGetData, Def_T_KIWAKU_MEISAI.Name, 0, Def_T_KIWAKU_MEISAI.VERSION);
            dt.Rows.Add(dr);

            // 社外用データを作成する。
            DataTable dtShagaiKiwakuMeisai = this.GetSchemeShagaiKiwakuMeisai();
            for (int i = 0; i < this.DtMeisai.Rows.Count; i++)
            {
                DataRow drTagNo = dtShagaiKiwakuMeisai.NewRow();
                string genpinTagNo = ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO);
                if (!string.IsNullOrEmpty(genpinTagNo))
                {
                    string shukkaFlag;
                    string nonyusakiCD;
                    string tagNo;
                    ComFunc.AnalyzeGenpinTagNo(ComFunc.GetFld(this.DtMeisai, i, FLD_TARGET_NO), out shukkaFlag, out nonyusakiCD, out tagNo);
                    drTagNo[Def_T_SHAGAI_KIWAKU_MEISAI.KOJI_NO] = this.KojiNo;
                    drTagNo[Def_T_SHAGAI_KIWAKU_MEISAI.CASE_ID] = this.CaseID;
                    drTagNo[Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG] = shukkaFlag;
                    drTagNo[Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD] = nonyusakiCD;
                    drTagNo[Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO] = tagNo;
                }
                // エラーの行数を合わせるために未入力の行も追加する。
                dtShagaiKiwakuMeisai.Rows.Add(drTagNo);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.Tables.Add(dtShagaiKiwakuMeisai);
            return ds;
        }

        #endregion

        #region 更新用木枠明細のデータテーブル

        /// --------------------------------------------------
        /// <summary>
        /// 更新用社外木枠明細のデータテーブル
        /// </summary>
        /// <returns>更新用社外木枠明細のデータテーブル</returns>
        /// <create>Y.Higuchi 2010/08/09</create>
        /// <update>K.Tsutsumi 2019/01/13 工事識別No.36進数化</update>
        /// --------------------------------------------------
        protected virtual DataTable GetSchemeShagaiKiwakuMeisai()
        {
            DataTable dt = new DataTable(Def_T_SHAGAI_KIWAKU_MEISAI.Name);

            dt.Columns.Add(Def_T_SHAGAI_KIWAKU_MEISAI.KOJI_NO, typeof(string));
            dt.Columns.Add(Def_T_SHAGAI_KIWAKU_MEISAI.CASE_ID, typeof(string));
            dt.Columns.Add(Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG, typeof(string));
            dt.Columns.Add(Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD, typeof(string));
            dt.Columns.Add(Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO, typeof(string));

            return dt;
        }

        #endregion

        #region 処理継続確認メッセージを表示

        /// --------------------------------------------------
        /// <summary>
        /// 処理継続確認メッセージを表示
        /// </summary>
        /// <param name="dsMoveShip">便間移動データ</param>
        /// <returns></returns>
        /// <create>H.Tajimi 2019/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override DialogResult ShowConfirmMoveShip(DataSet dsMoveShip)
        {
            var ret = DialogResult.OK;
            if (ComFunc.IsExistsData(dsMoveShip, ComDefine.DTTBL_MOVE_SHIP_TAG))
            {
                // 便間移動確認メッセージを表示する場合は、既存の確認メッセージを非表示にする
                this.MsgUpdateConfirm = string.Empty;
                var dtTag = dsMoveShip.Tables[ComDefine.DTTBL_MOVE_SHIP_TAG];
                var dtMultiMsg = ComFunc.GetSchemeMultiMessage();
                foreach (DataRow drTag in dtTag.Rows)
                {
                    var shukkaFlag = ComFunc.GetFld(drTag, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                    var nonyusakiCd = ComFunc.GetFld(drTag, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                    var tagNo = ComFunc.GetFld(drTag, Def_T_SHUKKA_MEISAI.TAG_NO);
                    ComFunc.AddMultiMessage(dtMultiMsg, "K0200070016", ComFunc.GetGenpinTagNo(shukkaFlag, nonyusakiCd, tagNo));
                }
                ret = this.ShowMultiMessage(dtMultiMsg, "K0200070015");
            }
            return ret;
        }

        #endregion

        #region 木枠と出荷明細の納入先/便が異なる場合のエラーメッセージ出力

        /// --------------------------------------------------
        /// <summary>
        /// 木枠と出荷明細の納入先/便が異なる場合のエラーメッセージ出力
        /// </summary>
        /// <param name="no">現品TagNo</param>
        /// <create>H.Tajimi 2019/09/12</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void ShowMessageDiffKiwakuAndShukka(string no)
        {
            // 木枠と現品Tag No.[{1}]は異なる納入先、便です。
            this.ShowMessage("K0200070018", no);
        }

        #endregion
    }
}
