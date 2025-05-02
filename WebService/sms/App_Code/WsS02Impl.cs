using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 出荷情報処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsS02Impl : WsBaseImpl
{
    #region Enum

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別Noで出荷明細データ件数を取得する際の集約単位
    /// </summary>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum S02UnitFlag
    {
        /// --------------------------------------------------
        /// <summary>
        /// 木枠単位
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        Kiwaku = 0,
        /// --------------------------------------------------
        /// <summary>
        /// 木枠明細単位
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        KiwakuMeisai = 1,
        /// --------------------------------------------------
        /// <summary>
        /// パレットNo単位
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        PalletNo = 2,
        /// --------------------------------------------------
        /// <summary>
        /// BoxNo単位
        /// </summary>
        /// <create>Y.Higuchi 2010/07/27</create>
        /// <update></update>
        /// --------------------------------------------------
        BoxNo = 3,
    }

    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsS02Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region S0200010:出荷情報登録

    #region 画面下部に表示する運送会社等の情報を保持するテーブルの作成

    /// --------------------------------------------------
    /// <summary>
    /// 画面下部に表示する運送会社等の情報を保持するテーブルの作成
    /// </summary>
    /// <param name="dt">チェックで使用したAR情報を含むデータテーブル</param>
    /// <param name="shukkaDate">出荷日</param>
    /// <param name="shukkaCnt">出荷明細データに含まれる出荷日の数</param>
    /// <returns>画面下部に表示する運送会社等の情報を保持するテーブル</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CreateAddition(DataTable dt, object shukkaDate, int shukkaCnt, bool isCopyAlway)
    {
        DataTable dtAddition = new DataTable(ComDefine.DTTBL_ADDITION);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_NAME, dt.Columns[ComDefine.FLD_ADDITION_DISP_NAME].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_SHIP, dt.Columns[ComDefine.FLD_ADDITION_DISP_SHIP].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_AR_NO, dt.Columns[ComDefine.FLD_ADDITION_DISP_AR_NO].DataType);
        // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_TSM_AR_NO, dt.Columns[ComDefine.FLD_ADDITION_TSM_AR_NO].DataType);
        // ↑
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_UNSOKAISHA, dt.Columns[ComDefine.FLD_ADDITION_UNSOKAISHA].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_INVOICE_NO, dt.Columns[ComDefine.FLD_ADDITION_INVOICE_NO].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_OKURIJYO_NO, dt.Columns[ComDefine.FLD_ADDITION_OKURIJYO_NO].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_BL_NO, dt.Columns[ComDefine.FLD_ADDITION_BL_NO].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_SHUKKA_DATE, dt.Columns[ComDefine.FLD_ADDITION_SHUKKA_DATE].DataType);

        DataTable dtAR = dt.DefaultView.ToTable(true,
                    ComDefine.FLD_ADDITION_DISP_NAME,
                    ComDefine.FLD_ADDITION_DISP_SHIP,
                    ComDefine.FLD_ADDITION_DISP_AR_NO,
                    ComDefine.FLD_ADDITION_TSM_AR_NO,
                    ComDefine.FLD_ADDITION_UNSOKAISHA,
                    ComDefine.FLD_ADDITION_INVOICE_NO,
                    ComDefine.FLD_ADDITION_OKURIJYO_NO,
                    ComDefine.FLD_ADDITION_BL_NO,
                    ComDefine.FLD_ADDITION_SHUKKA_DATE);
        foreach (DataRow drAR in dtAR.Rows)
        {
            DataRow dr = dtAddition.NewRow();

            dr[ComDefine.FLD_ADDITION_DISP_NAME] = ComFunc.GetFldObject(drAR, ComDefine.FLD_ADDITION_DISP_NAME);
            dr[ComDefine.FLD_ADDITION_DISP_SHIP] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_DISP_SHIP);
            dr[ComDefine.FLD_ADDITION_DISP_AR_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_DISP_AR_NO);
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            dr[ComDefine.FLD_ADDITION_TSM_AR_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_TSM_AR_NO);
            // ↑
            //if (shukkaCnt < 1 || isCopyAlway)
            {
                dr[ComDefine.FLD_ADDITION_UNSOKAISHA] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_UNSOKAISHA);
                dr[ComDefine.FLD_ADDITION_INVOICE_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_INVOICE_NO);
                dr[ComDefine.FLD_ADDITION_OKURIJYO_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_OKURIJYO_NO);
                dr[ComDefine.FLD_ADDITION_BL_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_BL_NO);
            }
            dr[ComDefine.FLD_ADDITION_SHUKKA_DATE] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_SHUKKA_DATE);
            dtAddition.Rows.Add(dr);
        }
        return dtAddition;
    }

    #endregion

    #region Box出荷

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxData(DatabaseHelper dbHelper, CondS02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {

            DataTable dtCheck = this.GetCheckBoxData(dbHelper, cond);

            // データチェック
            if (dtCheck.Rows.Count < 1)
            {
                // 該当BoxNo.はありません。
                errMsgID = "A9999999023";
                return null;
            }

            object shukkaDate = DBNull.Value;
            int shukkaCnt = 0;
            foreach (DataRow dr in dtCheck.Rows)
            {
                // 受入日のチェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_UKEIRE_CNT))
                {
                    // 受入済です。
                    errMsgID = "S0200010002";
                    return null;
                }

                // 工事識別管理Noのチェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)))
                {
                    // 木枠梱包されています。
                    errMsgID = "S0200010004";
                    return null;
                }

                // パレットNoのチェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)))
                {
                    // パレット梱包されています。
                    errMsgID = "S0200010005";
                    return null;
                }

                // AR情報の完了チェック
                // 状況区分が完了であればエラーとする。
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, ComDefine.FLD_ADDITION_DISP_AR_NO)) &&
                    ComFunc.GetFld(dr, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                {
                    // 完了AR No.となっています。
                    errMsgID = "S0200010013";
                    return null;
                }

                // 出荷日チェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.SHUKKA_DATE)))
                {
                    shukkaDate = ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
                }
                shukkaCnt += ComFunc.GetFldToInt32(dr, ComDefine.FLD_SHUKKA_CNT);
            }

            DataSet ds = this.GetBoxDataExec(dbHelper, cond);
            ds.Tables.Add(this.CreateAddition(dtCheck, shukkaDate, shukkaCnt, false));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット出荷

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPalletData(DatabaseHelper dbHelper, CondS02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {

            DataTable dtCheck = this.GetCheckPalletData(dbHelper, cond);

            // データチェック
            if (dtCheck.Rows.Count < 1)
            {
                // 該当パレットNo.はありません。
                errMsgID = "A9999999024";
                return null;
            }

            object shukkaDate = DBNull.Value;
            int shukkaCnt = 0;
            foreach (DataRow dr in dtCheck.Rows)
            {
                // 受入日のチェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_UKEIRE_CNT))
                {
                    // 受入済です。
                    errMsgID = "S0200010002";
                    return null;
                }

                // 工事識別管理Noのチェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)))
                {
                    // 木枠梱包されています。
                    errMsgID = "S0200010004";
                    return null;
                }

                // AR情報の完了チェック
                // 状況区分が完了であればエラーとする。
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, ComDefine.FLD_ADDITION_DISP_AR_NO)) &&
                    ComFunc.GetFld(dr, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                {
                    // 完了AR No.となっています。
                    errMsgID = "S0200010013";
                    return null;
                }

                // 出荷日チェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.SHUKKA_DATE)))
                {
                    shukkaDate = ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
                }
                shukkaCnt += ComFunc.GetFldToInt32(dr, ComDefine.FLD_SHUKKA_CNT);
            }

            DataSet ds = this.GetPalletDataExec(dbHelper, cond);
            ds.Tables.Add(this.CreateAddition(dtCheck, shukkaDate, shukkaCnt, false));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠出荷

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKiwakuData(DatabaseHelper dbHelper, CondS02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {

            DataTable dtCheck = this.GetCheckKiwakuData(dbHelper, cond);

            // データチェック
            if (dtCheck.Rows.Count < 1)
            {
                // 該当木枠梱包No.はありません。
                errMsgID = "S0200010010";
                return null;
            }

            object shukkaDate = DBNull.Value;
            int shukkaCnt = 0;
            foreach (DataRow dr in dtCheck.Rows)
            {
                // 受入日のチェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_UKEIRE_CNT))
                {
                    // 受入済です。
                    errMsgID = "S0200010002";
                    return null;
                }

                // AR情報の完了チェック
                // 状況区分が完了であればエラーとする。
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, ComDefine.FLD_ADDITION_DISP_AR_NO)) &&
                    ComFunc.GetFld(dr, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                {
                    // 完了AR No.となっています。
                    errMsgID = "S0200010013";
                    return null;
                }

                // 出荷日チェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.SHUKKA_DATE)))
                {
                    shukkaDate = ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
                }
                else
                {
                    // 作業区分のチェック
                    if (ComFunc.GetFld(dr, Def_T_KIWAKU.SAGYO_FLAG) != SAGYO_FLAG.KONPOKANRYO_VALUE1)
                    {
                        // 木枠梱包未完了です。梱包登録で確認下さい。
                        errMsgID = "S0200010012";
                        return null;
                    }
                }
                shukkaCnt += ComFunc.GetFldToInt32(dr, ComDefine.FLD_SHUKKA_CNT);
            }

            DataSet ds = this.GetKiwakuDataExec(dbHelper, cond);
            ds.Tables.Add(this.CreateAddition(dtCheck, shukkaDate, shukkaCnt, true));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Box出荷更新

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public bool UpdBoxData(DatabaseHelper dbHelper, CondS02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtShukka = dt.Clone();
            DataTable dtKonpozumi = dt.Clone();
            DateTime syoriDateTime = DateTime.Now;
            
            DataSet ds = this.GetBoxData(dbHelper, cond, ref errMsgID, ref args);

            // 取得時にエラーが発生していたらそれを優先する。
            if (!string.IsNullOrEmpty(errMsgID) && errMsgID != "A9999999023")
            {
                return false;
            }
            if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            int[] notFoundIndex;
            if (0 <= this.CheckSameData(dt, ds.Tables[Def_T_SHUKKA_MEISAI.Name], out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, Def_T_SHUKKA_MEISAI.TAG_NO) || notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 出荷用コンディション作成
            CondS02 condShukka = (CondS02)cond.Clone();
            condShukka.JyotaiFlag = JYOTAI_FLAG.SHUKKAZUMI_VALUE1;
            condShukka.UpdateDate = syoriDateTime;
            condShukka.KanriFlag = KANRI_FLAG.KANRYO_VALUE1;
            // 梱包済用コンディション
            CondS02 condKonpozumi = (CondS02)cond.Clone();
            condKonpozumi.ShukkaDate = DBNull.Value;
            condKonpozumi.ShukkaUserID = DBNull.Value;
            condKonpozumi.ShukkaUserName = DBNull.Value;
            condKonpozumi.JyotaiFlag = JYOTAI_FLAG.BOXZUMI_VALUE1;
            condKonpozumi.UnsokaishaName = DBNull.Value;
            condKonpozumi.InvoiceNo = DBNull.Value;
            condKonpozumi.OkurijyoNo = DBNull.Value;
            condKonpozumi.BLNo = DBNull.Value;
            condKonpozumi.UpdateDate = syoriDateTime;
            condKonpozumi.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;

            // 出荷、梱包済の各データを抽出
            foreach (DataRow dr in dt.Rows)
            {
                if (ComFunc.GetFld(dr, ComDefine.FLD_BTN_STATE) == cond.TextShukka)
                {
                    dtShukka.Rows.Add(dr.ItemArray);
                }
                else
                {
                    dtKonpozumi.Rows.Add(dr.ItemArray);
                }
            }

            if (0 < dtShukka.Rows.Count)
            {
                // 出荷に更新
                this.UpdShukkaMeisai(dbHelper, condShukka, dtShukka);
            }

            if (0 < dtKonpozumi.Rows.Count)
            {
                // 梱包済に更新
                this.UpdShukkaMeisai(dbHelper, condKonpozumi, dtKonpozumi);
            }

            // 出荷明細データの出荷状態を取得
            DataTable dtCount = this.CheckBoxDataShukkaDate(dbHelper, cond);
            condShukka.NonyusakiCD = ComFunc.GetFld(dtCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            condKonpozumi.NonyusakiCD = ComFunc.GetFld(dtCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            // Boxリスト管理データ更新用データテーブルの作成
            decimal dataCnt = ComFunc.GetFldToDecimal(dtCount, 0, ComDefine.FLD_CNT);
            decimal shukkaDateCnt = ComFunc.GetFldToDecimal(dtCount, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
            if (dataCnt == shukkaDateCnt)
            {
                // 全て出荷
                this.UpdBoxManage(dbHelper, condShukka, cond.ShukkaNo);

                // AR情報の更新
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_ADDITION))
                {
                    DataTable dtARNO = this.CheckARDataShukkaDate(dbHelper, cond, S02UnitFlag.BoxNo);
                    foreach (DataRow dr in dtARNO.Rows)
                    {
                        condShukka.ARNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO).Trim();
                        if (string.IsNullOrEmpty(condShukka.ARNo)) continue;
                        // AR情報の取得
                        if (cond.IsKonpo2Shukka || cond.IsShukka2Shukka)
                        {
                            // AR情報の更新
                            this.UpdAR(dbHelper, condShukka);
                        }
                    }
                }
            }
            else if (shukkaDateCnt == 0)
            {
                // 全て梱包済
                this.UpdBoxManage(dbHelper, condKonpozumi, cond.ShukkaNo);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット出荷更新

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public bool UpdPalletData(DatabaseHelper dbHelper, CondS02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtShukka = dt.Clone();
            DataTable dtKonpozumi = dt.Clone();
            DateTime syoriDateTime = DateTime.Now;

            DataSet ds = this.GetPalletData(dbHelper, cond, ref errMsgID, ref args);

            // 取得時にエラーが発生していたらそれを優先する。
            if (!string.IsNullOrEmpty(errMsgID) && errMsgID != "A9999999024")
            {
                return false;
            }
            if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            int[] notFoundIndex;
            if (0 <= this.CheckSameData(dt, ds.Tables[Def_T_SHUKKA_MEISAI.Name], out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, Def_T_SHUKKA_MEISAI.BOX_NO) || notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 出荷用コンディション作成
            CondS02 condShukka = (CondS02)cond.Clone();
            condShukka.JyotaiFlag = JYOTAI_FLAG.SHUKKAZUMI_VALUE1;
            condShukka.UpdateDate = syoriDateTime;
            condShukka.KanriFlag = KANRI_FLAG.KANRYO_VALUE1;
            // 梱包済用コンディション
            CondS02 condKonpozumi = (CondS02)cond.Clone();
            condKonpozumi.ShukkaDate = DBNull.Value;
            condKonpozumi.ShukkaUserID = DBNull.Value;
            condKonpozumi.ShukkaUserName = DBNull.Value;
            condKonpozumi.JyotaiFlag = JYOTAI_FLAG.PALLETZUMI_VALUE1;
            condKonpozumi.UnsokaishaName = DBNull.Value;
            condKonpozumi.InvoiceNo = DBNull.Value;
            condKonpozumi.OkurijyoNo = DBNull.Value;
            condKonpozumi.BLNo = DBNull.Value;
            condKonpozumi.UpdateDate = syoriDateTime;
            condKonpozumi.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;

            // 出荷、梱包済の各データを抽出
            foreach (DataRow dr in dt.Rows)
            {
                if (ComFunc.GetFld(dr, ComDefine.FLD_BTN_STATE) == cond.TextShukka)
                {
                    dtShukka.Rows.Add(dr.ItemArray);
                }
                else
                {
                    dtKonpozumi.Rows.Add(dr.ItemArray);
                }
            }

            if (0 < dtShukka.Rows.Count)
            {
                // 出荷に更新
                this.UpdShukkaMeisai(dbHelper, condShukka, dtShukka);
            }

            if (0 < dtKonpozumi.Rows.Count)
            {
                // 梱包済に更新
                this.UpdShukkaMeisai(dbHelper, condKonpozumi, dtKonpozumi);
            }

            // 出荷明細データの出荷状態を取得
            DataTable dtPalletCount = this.CheckPalletDataShukkaDate(dbHelper, cond, false);
            DataTable dtBoxCount = this.CheckPalletDataShukkaDate(dbHelper, cond, true);
            condShukka.NonyusakiCD = ComFunc.GetFld(dtPalletCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            condKonpozumi.NonyusakiCD = ComFunc.GetFld(dtPalletCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            // Boxリスト管理データ更新
            foreach (DataRow dr in dtBoxCount.Rows)
            {
                // BoxNo取得
                string boxNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO);
                if (ComFunc.GetFldToDecimal(dr, ComDefine.FLD_CNT) == ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE))
                {
                    // 全て出荷
                    this.UpdBoxManage(dbHelper, condShukka, boxNo);
                }
                else if (ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE) == 0)
                {
                    // 全て梱包済
                    this.UpdBoxManage(dbHelper, condKonpozumi, boxNo);
                }
            }

            // パレットリスト管理データ更新
            decimal dataCnt = ComFunc.GetFldToDecimal(dtPalletCount, 0, ComDefine.FLD_CNT);
            decimal shukkaDateCnt = ComFunc.GetFldToDecimal(dtPalletCount, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
            if (dataCnt == shukkaDateCnt)
            {
                // 全て出荷
                this.UpdPalletManage(dbHelper, condShukka, cond.ShukkaNo);

                // AR情報の更新
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_ADDITION))
                {
                    DataTable dtARNO = this.CheckARDataShukkaDate(dbHelper, cond, S02UnitFlag.PalletNo);
                    foreach (DataRow dr in dtARNO.Rows)
                    {
                        condShukka.ARNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO).Trim();
                        if (string.IsNullOrEmpty(condShukka.ARNo)) continue;
                        // AR情報の取得
                        if (cond.IsKonpo2Shukka || cond.IsShukka2Shukka)
                        {
                            // AR情報の更新
                            this.UpdAR(dbHelper, condShukka);
                        }
                    }
                }
            }
            else if (shukkaDateCnt == 0)
            {
                // 全て梱包済
                this.UpdPalletManage(dbHelper, condKonpozumi, cond.ShukkaNo);
            }

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠出荷更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public bool UpdKiwakuData(DatabaseHelper dbHelper, CondS02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtShukka = dt.Clone();
            DataTable dtKonpozumi = dt.Clone();
            DateTime syoriDateTime = DateTime.Now;
            
            DataSet ds = this.GetKiwakuData(dbHelper, cond, ref errMsgID, ref args);

            // 取得時にエラーが発生していたらそれを優先する。
            if (!string.IsNullOrEmpty(errMsgID) && errMsgID != "S0200010010")
            {
                return false;
            }
            if (!ComFunc.IsExistsData(ds, Def_T_KIWAKU_MEISAI.Name))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            int[] notFoundIndex;
            if (0 <= this.CheckSameData(dt, ds.Tables[Def_T_KIWAKU_MEISAI.Name], out notFoundIndex, Def_T_KIWAKU_MEISAI.VERSION, Def_T_KIWAKU_MEISAI.KOJI_NO, Def_T_KIWAKU_MEISAI.CASE_ID) || notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 出荷用コンディション作成
            CondS02 condShukka = (CondS02)cond.Clone();
            condShukka.SagyoFlag = SAGYO_FLAG.SHUKKAZUMI_VALUE1;
            condShukka.JyotaiFlag = JYOTAI_FLAG.SHUKKAZUMI_VALUE1;
            condShukka.UpdateDate = syoriDateTime;
            condShukka.KanriFlag = KANRI_FLAG.KANRYO_VALUE1;
            // 梱包済用コンディション
            CondS02 condKonpozumi = (CondS02)cond.Clone();
            condKonpozumi.ShukkaDate = DBNull.Value;
            condKonpozumi.ShukkaUserID = DBNull.Value;
            condKonpozumi.ShukkaUserName = DBNull.Value;
            condKonpozumi.SagyoFlag = SAGYO_FLAG.KONPOKANRYO_VALUE1;
            condKonpozumi.JyotaiFlag = JYOTAI_FLAG.KIWAKUKONPO_VALUE1;
            condKonpozumi.UnsokaishaName = DBNull.Value;
            condKonpozumi.InvoiceNo = DBNull.Value;
            condKonpozumi.OkurijyoNo = DBNull.Value;
            condKonpozumi.BLNo = DBNull.Value;
            condKonpozumi.UpdateDate = syoriDateTime;
            condKonpozumi.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;

            // 出荷、梱包済の各データを抽出
            foreach (DataRow dr in dt.Rows)
            {
                if (ComFunc.GetFld(dr, ComDefine.FLD_BTN_STATE) == cond.TextShukka)
                {
                    dtShukka.Rows.Add(dr.ItemArray);
                }
                else
                {
                    dtKonpozumi.Rows.Add(dr.ItemArray);
                }
            }

            if (0 < dtShukka.Rows.Count)
            {
                // 出荷に更新
                this.UpdShukkaMeisai(dbHelper, condShukka, dtShukka);
            }

            if (0 < dtKonpozumi.Rows.Count)
            {
                // 梱包済に更新
                this.UpdShukkaMeisai(dbHelper, condKonpozumi, dtKonpozumi);
            }

            // 出荷明細データの出荷状態を取得
            DataTable dtKiwakuCount = this.CheckKiwakuDataShukkaDate(dbHelper, cond, S02UnitFlag.Kiwaku);
            DataTable dtKiwakuMeisaiCount = this.CheckKiwakuDataShukkaDate(dbHelper, cond, S02UnitFlag.KiwakuMeisai);
            DataTable dtPalletCount = this.CheckKiwakuDataShukkaDate(dbHelper, cond, S02UnitFlag.PalletNo);
            DataTable dtBoxCount = this.CheckKiwakuDataShukkaDate(dbHelper, cond, S02UnitFlag.BoxNo);
            condShukka.NonyusakiCD = ComFunc.GetFld(dtKiwakuCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            condKonpozumi.NonyusakiCD = ComFunc.GetFld(dtKiwakuCount, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            // Boxリスト管理データ更新
            foreach (DataRow dr in dtBoxCount.Rows)
            {
                // BoxNo取得
                string boxNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO);
                if (ComFunc.GetFldToDecimal(dr, ComDefine.FLD_CNT) == ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE))
                {
                    // 全て出荷
                    this.UpdBoxManage(dbHelper, condShukka, boxNo);
                }
                else if (ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE) == 0)
                {
                    // 全て梱包済
                    this.UpdBoxManage(dbHelper, condKonpozumi, boxNo);
                }
            }
            // パレットリスト管理データ更新
            foreach (DataRow dr in dtPalletCount.Rows)
            {
                // パレットNo取得
                string palletNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO);
                if (ComFunc.GetFldToDecimal(dr, ComDefine.FLD_CNT) == ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE))
                {
                    // 全て出荷
                    this.UpdPalletManage(dbHelper, condShukka, palletNo);
                }
                else if (ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE) == 0)
                {
                    // 全て梱包済
                    this.UpdPalletManage(dbHelper, condKonpozumi, palletNo);
                }
            }
            // 木枠明細データ更新
            foreach (DataRow dr in dtKiwakuMeisaiCount.Rows)
            {
                // 内部管理用キー取得
                string caseID = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.CASE_ID);
                if (ComFunc.GetFldToDecimal(dr, ComDefine.FLD_CNT) == ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE))
                {
                    // 全て出荷
                    this.UpdKiwakuMeisai(dbHelper, condShukka, cond.ShukkaNo, caseID);
                }
                else if (ComFunc.GetFldToDecimal(dr, Def_T_SHUKKA_MEISAI.SHUKKA_DATE) == 0)
                {
                    // 全て梱包済
                    this.UpdKiwakuMeisai(dbHelper, condKonpozumi, cond.ShukkaNo, caseID);
                }
            }
            // 木枠データ更新
            decimal dataCnt = ComFunc.GetFldToDecimal(dtKiwakuCount, 0, ComDefine.FLD_CNT);
            decimal shukkaDateCnt = ComFunc.GetFldToDecimal(dtKiwakuCount, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
            if (dataCnt == shukkaDateCnt)
            {
                // 全て出荷
                this.UpdKiwaku(dbHelper, condShukka, cond.ShukkaNo);

                // AR情報の更新
                if (ComFunc.IsExistsData(ds, ComDefine.DTTBL_ADDITION))
                {
                    DataTable dtARNO = this.CheckARDataShukkaDate(dbHelper, cond, S02UnitFlag.Kiwaku);
                    foreach (DataRow dr in dtARNO.Rows)
                    {
                        condShukka.ARNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO).Trim();
                        if (string.IsNullOrEmpty(condShukka.ARNo)) continue;
                        // AR情報の取得
                        if (cond.IsKonpo2Shukka || cond.IsShukka2Shukka)
                        {
                            // AR情報の更新
                            this.UpdAR(dbHelper, condShukka);
                        }
                    }
                }
            }
            else if (shukkaDateCnt == 0)
            {
                // 全て梱包済
                this.UpdKiwaku(dbHelper, condKonpozumi, cond.ShukkaNo);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region S0200020:出荷情報明細

    #endregion

    #region S0200030:出荷情報照会

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update>Y.Higuchi 2010/10/28</update>
    /// <update>H.Tajimi 2015/12/07 出荷情報/複数便を選択し表示対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkaMeisai(DatabaseHelper dbHelper, CondS02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 2011/03/09 K.Tsutsumi Change T_ARが存在しなくても続行可能
            //DataSet ds = null;
            DataSet ds = new DataSet();
            // ↑
            // 納入先、便、AR No.チェック
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.ShukkaFlag = cond.ShukkaFlag;
            condSms.ARNo = cond.ARNo;
            condSms.NonyusakiCD = cond.NonyusakiCD;
            string nonyusakiCD = null;
            string[] nonyusakiCDs = null;
            DataSet dsCheck;
            bool ret = false;
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                // 便 or AR No.が入力されているのでチェックする。
                using (WsSmsImpl impl = new WsSmsImpl())
                {
                    // 2011/03/09 K.Tsutsumi Change T_ARが存在しなくても続行可能
                    //ret = impl.CheckNonyusakiAndARNo(dbHelper, condSms, out errMsgID, out nonyusakiCD, out dsCheck);
                    ret = impl.CheckNonyusakiAndARNo(dbHelper, condSms, false, out errMsgID, out nonyusakiCD, out dsCheck);
                    if ((ret == true) && (string.IsNullOrEmpty(cond.ARNo) == false) && (ComFunc.IsExistsData(dsCheck, Def_T_AR.Name) == true))
                    {
                        // ＡＲ情報データ待避
                        ds.Tables.Add(dsCheck.Tables[Def_T_AR.Name].Copy());
                    }
                    // ↑
                }
            }
            else
            {
                // 便 or AR No.が入力されてい無い場合は、そのまま検索する。
                nonyusakiCD = cond.NonyusakiCD;
                nonyusakiCDs = cond.NonyusakiCDs;
                ret = true;
            }
            if (ret)
            {
                cond.NonyusakiCD = nonyusakiCD;
                cond.NonyusakiCDs = nonyusakiCDs;
                // 2011/03/09 K.Tsutsumi Change 進捗件数欄追加
                //// 出荷明細取得
                //ds = this.GetShukkaMeisaiExec(dbHelper, cond);
                //if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                //{
                //    // 該当の明細は存在しません。
                //    errMsgID = "A9999999022";
                //    ds = null;
                //}

                // 進捗件数取得
                DataSet dsProgress = this.GetProgress(dbHelper, cond);
                if (!ComFunc.IsExistsData(dsProgress, ComDefine.DTTBL_PROGRESS))
                {
                    // 進捗件数が取得できませんでした。
                    errMsgID = "S0200030007";
                }
                else
                {
                    // 進捗件数テーブル待避
                    ds.Tables.Add(dsProgress.Tables[ComDefine.DTTBL_PROGRESS].Copy());

                    // 出荷明細取得                
                    DataSet dsShukka = this.GetShukkaMeisaiExec(dbHelper, cond);
                    if (!ComFunc.IsExistsData(dsShukka, Def_T_SHUKKA_MEISAI.Name))
                    {
                        // 該当の明細は存在しません。
                        errMsgID = "A9999999022";
                    }
                    else
                    {
                        // 出荷明細テーブル待避
                        ds.Tables.Add(dsShukka.Tables[Def_T_SHUKKA_MEISAI.Name].Copy());
                    }
                }
                // ↑
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region S0200040:ShippingDocument作成

    #region Invoice作成事前確認

    /// --------------------------------------------------
    /// <summary>
    /// Invoice作成事前確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/13</create>
    /// <update>D.Okumura 2019/01/28 PL Typeの入力チェックを除外</update>
    /// <update>H.Tajimi 2020/04/21 荷姿表を入力途中で保存可能とする</update>
    /// --------------------------------------------------
    public DataTable CheckInvoice(DatabaseHelper dbHelper, CondS02 cond, DataTable dt)
    {
        try
        {
            DataTable dtMessage = ComFunc.GetSchemeMultiMessage();

            foreach (DataRow In_dr in dt.Rows)
            {
                string Line = ComFunc.GetFld(In_dr, "LINE");
                string tgtPackingeNo = ComFunc.GetFld(In_dr, Def_T_PACKING.PACKING_NO);
                string tgtInvoiceNo = ComFunc.GetFld(In_dr, Def_T_PACKING.INVOICE_NO);
                string tgtKokunaigaiFlag = ComFunc.GetFld(In_dr, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG);

                // もともと荷姿表登録でやっていた必須チェック処理をここで行う
                // 画面側でもできるチェックだが、必ずDB層も呼び出されるためエラー内容に含まれる
                // 行番号を考慮してDB層でチェックを行う
                CheckRequiredPacking(dtMessage, In_dr, Line);

                // 各種整合確認
                const string CheckTableName = "CHECK_TABLE";
                const string CheckTable_MSGID = "MSGID";
                const string CheckTable_PACK = "PACK";
                const string CheckTable_TAG_NO = "TAG_NO";
                const string CheckTable_ECS_NO = "ECS_NO";
                const string CheckTable_TEHAI_RENKEI_NO = "TEHAI_RENKEI_NO";
                const string CheckTable_ESTIMATE_NO = "ESTIMATE_NO";
                CondS02 condTmp = (CondS02)cond.Clone();
                condTmp.PackingNo = tgtPackingeNo;
                DataSet dsCheck = this.CheckShippingExec(dbHelper, condTmp, (tgtKokunaigaiFlag == KOKUNAI_GAI_FLAG.GAI_VALUE1 ? true : false));
                if (dsCheck.Tables[CheckTableName].Rows.Count > 0)
                {
                    foreach (DataRow dr_check in dsCheck.Tables[CheckTableName].Rows)
                    {
                        string MsgID = ComFunc.GetFld(dr_check, CheckTable_MSGID);
                        string Pack = ComFunc.GetFld(dr_check, CheckTable_PACK);
                        string TagNo = ComFunc.GetFld(dr_check, CheckTable_TAG_NO);
                        string EcsNo = ComFunc.GetFld(dr_check, CheckTable_ECS_NO);
                        string TehaiRenkeiNo = ComFunc.GetFld(dr_check, CheckTable_TEHAI_RENKEI_NO);
                        string EstimateNo = ComFunc.GetFld(dr_check, CheckTable_ESTIMATE_NO);
                        ComFunc.AddMultiMessage(dtMessage, MsgID, Line, Pack, TagNo, EcsNo, TehaiRenkeiNo, EstimateNo);
                    }
                }

                DataSet dsNisugataM = this.GetNisugataMeisaiExec(dbHelper, condTmp);
                if (!ComFunc.IsExistsData(dsNisugataM, Def_T_PACKING_MEISAI.Name))
                {
                    // {0}行目の荷姿明細が取得出来ませんでした。
                    ComFunc.AddMultiMessage(dtMessage, "S0200040010", Line);
                    continue;
                }
                DataTable dtNisugataM = dsNisugataM.Tables[Def_T_PACKING_MEISAI.Name];

                bool hasKonpoError = false;
                bool checkedCtQty = false;
                foreach (DataRow dr_NMeisai in dtNisugataM.Rows)
                {
                    // もともと荷姿表登録でやっていた必須チェック処理をここで行う
                    string doukonFlag = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.DOUKON_FLAG);
                    if (!checkedCtQty)
                    {
                        if (doukonFlag == DOUKON_FLAG.OFF_VALUE1)
                        {
                            if (string.IsNullOrEmpty(ComFunc.GetFld(In_dr, Def_T_PACKING.CT_QTY)))
                            {
                                // CT Qtyの未設定エラーは、１荷姿辺りで一度しか出さない
                                // {0}行目のCT Qtyが未入力のため、荷姿登録から入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040036", Line);
                                checkedCtQty = true;
                            }
                        }
                    }

                    string CaseNo = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.CASE_NO);
                    string PalletNo = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.PALLET_NO);
                    string BoxNo = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.BOX_NO);
                    string KonpoNo = string.Empty;
                    if (!string.IsNullOrEmpty(CaseNo)) KonpoNo = CaseNo;
                    else if (!string.IsNullOrEmpty(PalletNo)) KonpoNo = PalletNo;
                    else KonpoNo = BoxNo;
                    if (string.IsNullOrEmpty(KonpoNo))
                    {
                        if (!hasKonpoError)
                        {
                            // 梱包Noの未設定エラーは、１荷姿辺りで一度しか出さない
                            // {0}行目の梱包No.が未入力のため、荷姿登録から入力してください。
                            ComFunc.AddMultiMessage(dtMessage, "S0200040041", Line);
                            hasKonpoError = true;
                        }
                        continue;
                    }

                    // 荷姿明細の形式が定形外のとき、SizeL,SizeH,SizeW、GRWTのいずれかが0の場合、エラーとする
                    string SizeL = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.SIZE_L);
                    SizeL = (SizeL==string.Empty?"0":SizeL);
                    string SizeH = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.SIZE_H);
                    SizeH = (SizeH==string.Empty?"0":SizeH);
                    string SizeW = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.SIZE_W);
                    SizeW = (SizeW==string.Empty?"0":SizeW);
                    string GRWT = ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.GRWT);
                    GRWT = (GRWT==string.Empty?"0":GRWT);

                    // もともと荷姿表登録でやっていた必須チェック処理をここで行う
                    CheckRequiredPackingMeisai(dtMessage, dr_NMeisai, doukonFlag, Line, KonpoNo);

                    if (tgtKokunaigaiFlag == KOKUNAI_GAI_FLAG.GAI_VALUE1)
                    {   // 国外のみチェック
                        if (ComFunc.GetFld(dr_NMeisai, Def_T_PACKING_MEISAI.FORM_STYLE_FLAG) == FORM_STYLE_FLAG.FORM_0_VALUE1)
                        {
                            if (int.Parse(SizeL) == 0)
                            {
                                // {0}行目の梱包No.{1}のSizeLが未入力のため、荷姿登録で確認してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040012", Line, KonpoNo);
                            }
                            if (int.Parse(SizeH) == 0)
                            {
                                // {0}行目の梱包No.{1}のSizeHが未入力のため、荷姿登録から入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040013", Line, KonpoNo);
                            }
                            if (int.Parse(SizeW) == 0)
                            {
                                // {0}行目の梱包No.{1}のSizeWが未入力のため、荷姿登録から入力してください。
                                ComFunc.AddMultiMessage(dtMessage, "S0200040014", Line, KonpoNo);
                            }
                        }
                    }
                    // 荷姿明細のGRWTが0の場合、エラーとする
                    if (double.Parse(GRWT) == 0)
                    {
                        // {0}行目の梱包No.{1}のGRWTが未入力のため、荷姿登録から入力してください。
                        ComFunc.AddMultiMessage(dtMessage, "S0200040015", Line, KonpoNo);
                    }
                }
            }

            return dtMessage;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表の必須入力チェック
    /// </summary>
    /// <param name="dtMessage">エラーメッセージ</param>
    /// <param name="dr">荷姿表データ</param>
    /// <param name="line">行番号</param>
    /// <create>H.Tajimi 2020/04/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private void CheckRequiredPacking(DataTable dtMessage, DataRow dr, string line)
    {
        Func<string, bool> fncIsNullOrEmpty = (columnName) =>
        {
            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, columnName)))
            {
                return true;
            }
            return false;
        };

        if (fncIsNullOrEmpty(Def_T_PACKING.UNSOKAISHA_CD))
        {
            // {0}行目の運送会社が未入力のため、荷姿登録から入力してください。
            ComFunc.AddMultiMessage(dtMessage, "S0200040032", line);
        }

        if (fncIsNullOrEmpty(Def_T_PACKING.INVOICE_NO))
        {
            // {0}行目のINVOICE No.が未入力のため、荷姿登録から入力してください。
            ComFunc.AddMultiMessage(dtMessage, "S0200040033", line);
        }

        if (fncIsNullOrEmpty(Def_M_BUKKEN.BUKKEN_NAME))
        {
            // {0}行目のItemが未入力のため、荷姿登録から入力してください。
            ComFunc.AddMultiMessage(dtMessage, "S0200040034", line);
        }

        if (ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
        {
            if (fncIsNullOrEmpty(Def_M_NONYUSAKI.SHIP))
            {
                // {0}行目のShipが未入力のため、荷姿登録から入力してください。
                ComFunc.AddMultiMessage(dtMessage, "S0200040035", line);
            }
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表明細の必須入力チェック
    /// </summary>
    /// <param name="dtMessage">エラーメッセージ</param>
    /// <param name="dr">荷姿表明細データ</param>
    /// <param name="doukonFlag">同梱フラグ</param>
    /// <param name="line">行番号</param>
    /// <param name="konpoNo">梱包No</param>
    /// <create>H.Tajimi 2020/04/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private void CheckRequiredPackingMeisai(DataTable dtMessage, DataRow dr, string doukonFlag, string line, string konpoNo)
    {
        Func<string, bool> fncIsNullOrEmpty = (columnName) =>
        {
            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, columnName)))
            {
                return true;
            }
            return false;
        };

        if (fncIsNullOrEmpty(Def_T_PACKING_MEISAI.ATTN))
        {
            // {0}行目の梱包No.{1}の宛先が未入力のため、荷姿登録から入力してください。
            ComFunc.AddMultiMessage(dtMessage, "S0200040037", line, konpoNo);
        }

        if (doukonFlag == DOUKON_FLAG.OFF_VALUE1)
        {
            if (fncIsNullOrEmpty(Def_T_PACKING_MEISAI.CT_NO))
            {
                // {0}行目の梱包No.{1}のCT No.が未入力のため、荷姿登録から入力してください。
                ComFunc.AddMultiMessage(dtMessage, "S0200040038", line, konpoNo);
            }
            if (fncIsNullOrEmpty(Def_T_PACKING_MEISAI.FORM_STYLE_FLAG))
            {
                // {0}行目の梱包No.{1}の定形が未入力のため、荷姿登録から入力してください。
                ComFunc.AddMultiMessage(dtMessage, "S0200040039", line, konpoNo);
            }
            if (fncIsNullOrEmpty(Def_T_PACKING_MEISAI.NONYUSAKI_CD))
            {
                // {0}行目の梱包No.{1}のItemもしくはShipが未入力のため、荷姿登録から入力してください。
                ComFunc.AddMultiMessage(dtMessage, "S0200040040", line, konpoNo);
            }
        }
    }

    #endregion

    #region 各種テーブル更新

    /// --------------------------------------------------
    /// <summary>
    /// 各種テーブル更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// --------------------------------------------------
    public bool UpdShippingData(DatabaseHelper dbHelper, CondS02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // cond:ShukkaDate
        try
        {
            // Invoice単位で処理する
            foreach(DataRow In_dr in dt.Rows)
            {
                CondS02 condTmp;
                //string Line = ComFunc.GetFld(In_dr, "LINE");
                string tgtPackingeNo = ComFunc.GetFld(In_dr, Def_T_PACKING.PACKING_NO);
                string tgtInvoiceNo = ComFunc.GetFld(In_dr, Def_T_PACKING.INVOICE_NO);
                string tgtKokunaigaiFlag = ComFunc.GetFld(In_dr, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG);

                //===== ロック =====
                condTmp = (CondS02)cond.Clone();
                condTmp.PackingNo = tgtPackingeNo;
                // 荷姿取得(ロック)
                DataSet dsLockNisugata = this.LockNisugataExec(dbHelper, condTmp);
                if (!ComFunc.IsExistsData(dsLockNisugata, Def_T_PACKING.Name))
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                // 荷姿のバージョンチェック
                int index;
                int[] notFoundIndex = null;
                index = this.CheckSameData(dsLockNisugata.Tables[Def_T_PACKING.Name], dt, out notFoundIndex, Def_T_PACKING.VERSION, Def_T_PACKING.PACKING_NO);
                if (0 <= index)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                else if (notFoundIndex != null)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                // 荷姿明細取得(ロック)
                DataSet dsLockNisugataMeisai = this.LockNisugataMeisaiExec(dbHelper, condTmp);
                if (!ComFunc.IsExistsData(dsLockNisugataMeisai, Def_T_PACKING_MEISAI.Name))
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                foreach (DataRow dr_LNMeisai in dsLockNisugataMeisai.Tables[Def_T_PACKING_MEISAI.Name].Rows)
                {
                    string ShukkaFlag = ComFunc.GetFld(dr_LNMeisai, Def_T_PACKING_MEISAI.SHUKKA_FLAG);
                    string NonyusakiCD = ComFunc.GetFld(dr_LNMeisai, Def_T_PACKING_MEISAI.NONYUSAKI_CD);
                    string PalletNo = ComFunc.GetFld(dr_LNMeisai, Def_T_PACKING_MEISAI.PALLET_NO);
                    string BoxNo = ComFunc.GetFld(dr_LNMeisai, Def_T_PACKING_MEISAI.BOX_NO);
                    object C_No = ComFunc.GetFldObject(dr_LNMeisai, Def_T_PACKING_MEISAI.CASE_NO, DBNull.Value);
                    string ARNo = ComFunc.GetFld(dr_LNMeisai, Def_T_PACKING_MEISAI.AR_NO);
                    string kojiNo = null;
                    string caseID = null;

                    DataSet dsLockKiwaku = null;
                    DataSet dsLockKiwakuMeisai = null;
                    if (C_No != null && C_No != DBNull.Value)
                    {
                        // 木枠テーブル取得(ロック)
                        condTmp = (CondS02)cond.Clone();
                        condTmp.NonyusakiCD = NonyusakiCD;
                        condTmp.ShukkaFlag = ShukkaFlag;
                        condTmp.ARNo = ARNo;
                        condTmp.CaseNo = C_No.ToString();
                        dsLockKiwaku = this.LockKiwakuExec(dbHelper, condTmp);
                        if (!ComFunc.IsExistsData(dsLockKiwaku, Def_T_KIWAKU.Name))
                        {
                            // 他端末で更新された為、更新できませんでした。
                            errMsgID = "A9999999027";
                            return false;
                        }
                        kojiNo = ComFunc.GetFld(dsLockKiwaku, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.KOJI_NO);

                        // 木枠明細テーブル取得(ロック)
                        condTmp = (CondS02)cond.Clone();
                        condTmp.KojiNo = kojiNo;
                        condTmp.CaseNo = C_No.ToString();
                        dsLockKiwakuMeisai = this.LockKiwakuMeisaiExec(dbHelper, condTmp);
                        if (!ComFunc.IsExistsData(dsLockKiwakuMeisai, Def_T_KIWAKU_MEISAI.Name))
                        {
                            // 他端末で更新された為、更新できませんでした。
                            errMsgID = "A9999999027";
                            return false;
                        }
                        caseID = ComFunc.GetFld(dsLockKiwakuMeisai, Def_T_KIWAKU_MEISAI.Name, 0, Def_T_KIWAKU_MEISAI.CASE_ID);
                    }

                    // 出荷明細取得(ロック)
                    condTmp = (CondS02)cond.Clone();
                    condTmp.ShukkaFlag = ShukkaFlag;
                    condTmp.NonyusakiCD = NonyusakiCD;
                    condTmp.PalletNo = PalletNo;
                    condTmp.BoxNo = BoxNo;
                    condTmp.KojiNo = kojiNo;
                    DataSet dsLockShukkaMeisai = this.LockShukkaMeisaiExec(dbHelper, condTmp, caseID);
                    if (!ComFunc.IsExistsData(dsLockShukkaMeisai, Def_T_SHUKKA_MEISAI.Name))
                    {
                        // 他端末で更新された為、更新できませんでした。
                        errMsgID = "A9999999027";
                        return false;
                    }

                    // AR情報テーブル取得(ロック)
                    DataSet dsLockAR = null;
                    if (!string.IsNullOrEmpty(ARNo))
                    {
                        condTmp = (CondS02)cond.Clone();
                        condTmp.NonyusakiCD = NonyusakiCD;
                        condTmp.ARNo = ARNo;
                        dsLockAR = this.LockARExec(dbHelper, condTmp);
                        if (!ComFunc.IsExistsData(dsLockAR, Def_T_AR.Name))
                        {
                            // 他端末で更新された為、更新できませんでした。
                            errMsgID = "A9999999027";
                            return false;
                        }
                    }

                    //===== 各種テーブル更新 =====
                    // 荷姿
                    condTmp = (CondS02)cond.Clone();
                    condTmp.PackingNo = tgtPackingeNo;
                    condTmp.HakkouFlag = HAKKO_FLAG.COMP_VALUE1;
                    this.UpdNisugataExec(dbHelper, In_dr, condTmp);

                    // 出荷明細
                    object UpdateDate = DateTime.Now;
                    string UnsokaisyaName = ComFunc.GetFld(In_dr, Def_M_UNSOKAISHA.UNSOKAISHA_NAME);
                    condTmp = (CondS02)cond.Clone();
                    condTmp.JyotaiFlag = JYOTAI_FLAG.SHUKKAZUMI_VALUE1;
                    condTmp.ShukkaDate = cond.ShukkaDate;
                    condTmp.ShukkaUserID = this.GetUpdateUserID(cond);
                    condTmp.ShukkaUserName = this.GetUpdateUserName(cond);
                    condTmp.UnsokaishaName = UnsokaisyaName;
                    if (tgtKokunaigaiFlag == KOKUNAI_GAI_FLAG.GAI_VALUE1)
                    {
                        condTmp.InvoiceNo = tgtInvoiceNo;
                        condTmp.OkurijyoNo = DBNull.Value;
                    }
                    else
                    {
                        condTmp.InvoiceNo = DBNull.Value;
                        condTmp.OkurijyoNo = tgtInvoiceNo;
                    }
                    condTmp.BLNo = DBNull.Value;
                    condTmp.UpdateDate = UpdateDate;
                    DataTable dtTmpShukkaMeisai = dsLockShukkaMeisai.Tables[Def_T_SHUKKA_MEISAI.Name].Copy();
                    if (!string.IsNullOrEmpty(PalletNo))
                    {   // PALLET
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.BOX_NO);
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.KOJI_NO);
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.CASE_ID);
                    }
                    else if (!string.IsNullOrEmpty(BoxNo))
                    {   // BOX
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.PALLET_NO);
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.KOJI_NO);
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.CASE_ID);
                    }
                    else
                    {   // その他
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.PALLET_NO);
                        dtTmpShukkaMeisai.Columns.Remove(Def_T_SHUKKA_MEISAI.BOX_NO);
                    }
                    this.UpdShukkaMeisai(dbHelper, condTmp, dtTmpShukkaMeisai);

                    // 木枠
                    if (dsLockKiwaku != null)
                    {
                        condTmp.KojiNo = kojiNo;
                        condTmp.SagyoFlag = SAGYO_FLAG.SHUKKAZUMI_VALUE1;
                        this.UpdKiwaku(dbHelper, condTmp, kojiNo);
                    }

                    // 木枠明細
                    if (dsLockKiwakuMeisai != null)
                    {
                        this.UpdKiwakuMeisai(dbHelper, condTmp, kojiNo, caseID);
                    }

                    // AR情報
                    if (dsLockAR != null)
                    {
                        condTmp.IsKonpo2Shukka = true;
                        condTmp.ARNo = ARNo;
                        condTmp.NonyusakiCD = NonyusakiCD;
                        this.UpdAR(dbHelper, condTmp);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 帳票用データ取得A

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得A
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</create>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 複数PO No対応</update>
    /// --------------------------------------------------
    public DataSet GetShippingDocument(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet dsRet = new DataSet();

            // 帳票A：INVOICE
            dsRet.Tables.Add(this.GetTyouhyouDataA(dbHelper, cond));

            // 帳票B:荷受マスタ
            // cond:ConsignCd
            DataTable dtConsign = this.GetConsignExec(dbHelper, cond);
            dtConsign.TableName = ComDefine.DTTBL_SIPPING_B;
            dsRet.Tables.Add(dtConsign.Copy());

            /* 未使用
            // 帳票C:出荷明細
            // cond:PackingNo
            DataTable dtShukkameisai = this.GetShukkameisaiTyouhyouExec(dbHelper, cond);
            dtShukkameisai.TableName = ComDefine.DTTBL_SIPPING_C;
            dsRet.Tables.Add(dtShukkameisai.Copy());
            */
            // 帳票D:出荷明細+手配明細(帳票用)
            dsRet.Tables.Add(this.GetSmeisaiTmeisaiTyouhyouExec(dbHelper, cond));

            // 帳票E:荷姿明細+納入先マスタ
            dsRet.Tables.Add(this.GetNMeisaiNounyuTyouhyouExec(dbHelper, cond));

            // 帳票F:出荷明細+α ATTACHED SHEET
            // cond:PackingNo
            dsRet.Tables.Add(this.GetShukkameisaiplusTyouhyouExec(dbHelper, cond));

            // 帳票G:運送会社マスタ
            // cond:UnsokaishaNo
            DataTable dsUnso = this.GetUnsokaisyaTyouhyouExec(dbHelper, cond);
            dsUnso.TableName = ComDefine.DTTBL_SIPPING_G;
            dsRet.Tables.Add(dsUnso);

            // 帳票H:出荷明細(一覧用)
            DataTable dtShukka = this.GetShukkaListTyouhyouExec(dbHelper, cond);
            dsRet.Tables.Add(dtShukka);

            // 帳票I:PO#+total取得
            DataTable dtPoNoList = this.GetShukkameisaiPoNoExec(dbHelper, cond);
            dsRet.Tables.Add(dtPoNoList);
            

            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 帳票用データ取得A

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得A
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/18</create>
    /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
    /// <update>D.Okumura 2020/10/01 EFA_SMS-138 有償版 Shipping Document の現地通貨略称表示対応</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    private DataTable GetTyouhyouDataA(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            //----- データ取得 -----
            // 荷姿
            DataSet dsNisugata = this.GetNisugataExec(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsNisugata, Def_T_PACKING.Name))
            {
                return null;
            }
            DataRow drNisugatai = dsNisugata.Tables[Def_T_PACKING.Name].Rows[0];

            // 荷姿明細
            DataSet dsNisugataMeisai = this.GetNisugataMeisaiExec(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsNisugataMeisai, Def_T_PACKING_MEISAI.Name))
            {
                return null;
            }
            DataTable dtNisugataMeisai = dsNisugataMeisai.Tables[Def_T_PACKING_MEISAI.Name];
            DataRow drNisugataMeisai = dsNisugataMeisai.Tables[Def_T_PACKING_MEISAI.Name].Rows[0];//1行目を使用

            // 荷受けマスタ
            DataTable dtConsign = this.GetConsignExec(dbHelper, cond);

            // 配送先マスタ
            DataSet dsDeliver = this.GetDeliverExec(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsDeliver, Def_M_DELIVER.Name))
            {
                return null;
            }
            DataRow drDeliver = dsDeliver.Tables[Def_M_DELIVER.Name].Rows[0];

            // 納入先マスタ
            CondS02 condTmp = (CondS02)cond.Clone();
            condTmp.ShukkaFlag = ComFunc.GetFld(drNisugataMeisai, Def_T_PACKING_MEISAI.SHUKKA_FLAG);
            condTmp.NonyusakiCD = ComFunc.GetFld(drNisugataMeisai, Def_T_PACKING_MEISAI.NONYUSAKI_CD);
            DataSet dsNonyusaki = this.GetNonyusakiExec(dbHelper, condTmp);
            if (!ComFunc.IsExistsData(dsNonyusaki, Def_M_NONYUSAKI.Name))
            {
                return null;
            }

            DataRow drNonyusaki = dsNonyusaki.Tables[Def_M_NONYUSAKI.Name].Rows[0];

            // TOTAL(YEN)取得
            DataTable dtTotalYen = this.GetPackingShukkameisaiExec(dbHelper, cond, 0);

            // PO#取得
            DataTable dtPoNo = this.GetPackingShukkameisaiExec(dbHelper, cond, 1);

            // COUNTRY OF ORIGIN取得
            //DataTable dtCoO = this.GetPackingShukkameisaiExec(dbHelper, cond, 2);

            // ----- テーブル作成 -----
            DataTable dtRet = new DataTable(ComDefine.DTTBL_SIPPING_A);
            dtRet.Columns.Add(ComDefine.FLD_CONSIGNED_TO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_INVOICE_NO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_DATE, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_PARTS_FOR, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_REF, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_DELIVERY_TO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_CASEMARK, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_CREATOR, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TERMS, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TOTAL_CARTON, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TOTAL_CARTON_NAME, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TOTAL, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_PO_NO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_PLC, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_INTERNAL_PO_NO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TITLE, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_SHIP_TO, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_TOTAL_GRWT, typeof(string));
            dtRet.Columns.Add(ComDefine.FLD_SHIP_NAME, typeof(string));
            dtRet.Columns.Add(Def_M_NONYUSAKI.ESTIMATE_FLAG, typeof(string));
            dtRet.Columns.Add(Def_T_PACKING.TRADE_TERMS_FLAG, typeof(string));

            DataRow dr = dtRet.NewRow();

            const string NewLine = "\n";

            // Consigned to
            dr[ComDefine.FLD_CONSIGNED_TO] = ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.NAME);
            if (!string.IsNullOrEmpty(ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.ADDRESS)))
                dr[ComDefine.FLD_CONSIGNED_TO] += NewLine + ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.ADDRESS);
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drNisugatai, Def_T_PACKING.CONSIGN_ATTN)))
                dr[ComDefine.FLD_CONSIGNED_TO] += NewLine + ComDefine.REPORT_SIPPING_ATTN + ComFunc.GetFld(drNisugatai, Def_T_PACKING.CONSIGN_ATTN);
            if (!string.IsNullOrEmpty(ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.TEL1)))
                dr[ComDefine.FLD_CONSIGNED_TO] += (NewLine + ComDefine.REPORT_SIPPING_TEL + ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.TEL1));
            if (!string.IsNullOrEmpty(ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.TEL2)))
                dr[ComDefine.FLD_CONSIGNED_TO] += (NewLine + ComDefine.REPORT_SIPPING_TEL + ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.TEL2));
            if (!string.IsNullOrEmpty(ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.FAX)))
                dr[ComDefine.FLD_CONSIGNED_TO] += (NewLine + ComDefine.REPORT_SIPPING_FAX + ComFunc.GetFld(dtConsign, 0, Def_M_CONSIGN.FAX));
            // INVOICE NO.
            dr[ComDefine.FLD_INVOICE_NO] = cond.InvoiceNo;
            // DATE
            dr[ComDefine.FLD_DATE] = cond.ShukkaDate;
            // PARTS FOR
            dr[ComDefine.FLD_PARTS_FOR] = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.NONYUSAKI_NAME);
            // REF.
            dr[ComDefine.FLD_REF] = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.SHIP_SEIBAN);
            // DELIVERY TO
            dr[ComDefine.FLD_DELIVERY_TO] = ComFunc.GetFld(drDeliver, Def_M_DELIVER.NAME);
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drDeliver, Def_M_DELIVER.ADDRESS)))
                dr[ComDefine.FLD_DELIVERY_TO] += (NewLine + ComFunc.GetFld(drDeliver, Def_M_DELIVER.ADDRESS));
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drNisugatai, Def_T_PACKING.DELIVER_ATTN)))
                dr[ComDefine.FLD_DELIVERY_TO] += NewLine + ComDefine.REPORT_SIPPING_ATTN + ComFunc.GetFld(drNisugatai, Def_T_PACKING.DELIVER_ATTN);
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drDeliver, Def_M_DELIVER.TEL1)))
                dr[ComDefine.FLD_DELIVERY_TO] += (NewLine + ComDefine.REPORT_SIPPING_TEL + ComFunc.GetFld(drDeliver, Def_M_DELIVER.TEL1));
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drDeliver, Def_M_DELIVER.TEL2)))
                dr[ComDefine.FLD_DELIVERY_TO] += (NewLine + ComDefine.REPORT_SIPPING_TEL + ComFunc.GetFld(drDeliver, Def_M_DELIVER.TEL2));
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drDeliver, Def_M_DELIVER.FAX)))
                dr[ComDefine.FLD_DELIVERY_TO] += (NewLine + ComDefine.REPORT_SIPPING_FAX + ComFunc.GetFld(drDeliver, Def_M_DELIVER.FAX));
            // CASE MARK
            string caseMark = string.Empty;
            string ctQty = ComFunc.GetFld(drNisugatai, Def_T_PACKING.CT_QTY);
            if (!string.IsNullOrEmpty(ctQty) && ctQty != "1")
            {
                string cnoMin = dtNisugataMeisai.Compute("Min(" + Def_T_PACKING_MEISAI.CT_NO + ")", null).ToString();
                string cnoMax = dtNisugataMeisai.Compute("Max(" + Def_T_PACKING_MEISAI.CT_NO + ")", null).ToString();

                caseMark = cnoMin + "/" + ctQty + "-" + cnoMax + "/" + ctQty;
            }
            else
            {
                caseMark = "1/1";
            }
            dr[ComDefine.FLD_CASEMARK] = caseMark;
            // 発行者
            dr[ComDefine.FLD_CREATOR] = this.GetUpdateUserName(cond);
            // 貿易条件
            // dr[ComDefine.FLD_TERMS] は呼び出し元で設定
            // TOTAL(QTY)
            dr[ComDefine.FLD_TOTAL_CARTON] = ComFunc.GetFld(drNisugatai, Def_T_PACKING.CT_QTY);
            // TOTAL(QTY単位)
            DataRow[] drTmpB = dtNisugataMeisai.Select(Def_T_PACKING_MEISAI.BOX_NO + " IS NOT NULL");
            DataRow[] drTmpP = dtNisugataMeisai.Select(Def_T_PACKING_MEISAI.PALLET_NO + " IS NOT NULL");
            DataRow[] drTmpW = dtNisugataMeisai.Select(Def_T_PACKING_MEISAI.CASE_NO + " IS NOT NULL");
            int bNum = (drTmpB == null ? 0 : drTmpB.Length);
            int pNum = (drTmpP == null ? 0 : drTmpP.Length);
            int wNum = (drTmpW == null ? 0 : drTmpW.Length);
            string cartonName = string.Empty;
            if (bNum > 0 && pNum == 0 && wNum == 0)
            {
                cartonName = ComDefine.REPORT_SIPPING_TOTAL_B;
            }
            else if (bNum == 0 && pNum > 0 && wNum == 0)
            {
                cartonName = ComDefine.REPORT_SIPPING_TOTAL_P;
            }
            else if (bNum == 0 && pNum == 0 && wNum > 0)
            {
                cartonName = ComDefine.REPORT_SIPPING_TOTAL_W;
            }
            else
            {
                cartonName = ComDefine.REPORT_SIPPING_TOTAL_C;
            }
            dr[ComDefine.FLD_TOTAL_CARTON_NAME] = cartonName;
            // TOTAL(YEN)
            dr[ComDefine.FLD_TOTAL] = ComFunc.GetFld(dtTotalYen, 0, ComDefine.FLD_TOTALYEN);

            string estimateFlag = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.ESTIMATE_FLAG);
            if (estimateFlag == ESTIMATE_FLAG.ONEROUS_VALUE1)
            {
                var dtPoNoFullList = new StringBuilder();
                //PO_NOはdtPoNoの行数分取得し、後ろに空白を付けて列挙する。
                for (int dtPoNoIndex = 0; dtPoNoIndex < dtPoNo.Rows.Count; dtPoNoIndex++)
                {
                    dtPoNoFullList.ApdN(ComFunc.GetFld(dtPoNo, dtPoNoIndex, Def_T_TEHAI_ESTIMATE.PO_NO));
                    dtPoNoFullList.ApdN(" ");
                }
                // PO#
                dr[ComDefine.FLD_PO_NO] = dtPoNoFullList.ToString().TrimEnd();
                dr[ComDefine.FLD_PLC] = ComFunc.GetFld(dtPoNo, 0, ComDefine.FLD_CURRENCY_FLAG_NAME);
                // REF. O/#
                // dr[ComDefine.FLD_INTERNAL_PO_NO] は呼び出し元で設定
                // タイトル
                // dr[ComDefine.FLD_TITLE] は呼び出し元で設定
                // 出荷先
                dr[ComDefine.FLD_SHIP_TO] = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.SHIP_TO);
                // 運賃計算基礎重量
                string grwt = dtNisugataMeisai.Compute("Sum(" + Def_T_PACKING_MEISAI.GRWT + ")", string.Format("{0} <> {1}", Def_T_PACKING_MEISAI.DOUKON_FLAG, DOUKON_FLAG.ON_VALUE1)).ToString();
                dr[ComDefine.FLD_TOTAL_GRWT] = grwt;
                // 便
                dr[ComDefine.FLD_SHIP_NAME] = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.SHIP);
            }

            // ESTIMATE_FLAG
            dr[Def_M_NONYUSAKI.ESTIMATE_FLAG] = ComFunc.GetFld(drNonyusaki, Def_M_NONYUSAKI.ESTIMATE_FLAG);
            // TRADE_TERMS_FLAG
            // dr[Def_T_PACKING.TRADE_TERMS_FLAG] は呼び出し元で設定

            dtRet.Rows.Add(dr);

            return dtRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷受マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>D.Okumura 2021/01/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetConsign(DatabaseHelper dbHelper, CondS02 cond)
    {
        // cond:ConsignCd
        try
        {
            DataSet dsRet = new DataSet();

            //----- データ取得 -----
            // 荷受けマスタ
            DataTable dtConsign = this.GetConsignExec(dbHelper, cond);
            //dtConsign.TableName = ComDefine.DTTBL_SIPPING_B;
            dsRet.Tables.Add(dtConsign.Copy());
            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion


    #region 帳票用データ取得D

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得D
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTyouhyouDataD(DatabaseHelper dbHelper, CondS02 cond)
    {
        // cond:PackingNo
        try
        {
            DataSet dsRet = new DataSet();

            //----- データ取得 -----
            // 出荷明細+手配明細
            DataTable dtShukkameisai = this.GetSmeisaiTmeisaiTyouhyouExec(dbHelper, cond);
            dsRet.Tables.Add(dtShukkameisai);
            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 帳票用データ取得E

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得E
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTyouhyouDataE(DatabaseHelper dbHelper, CondS02 cond)
    {
        // cond:PackingNo
        try
        {
            DataSet dsRet = new DataSet();

            //----- データ取得 -----
            // 荷姿明細+納入先マスタ
            dsRet.Tables.Add(this.GetNMeisaiNounyuTyouhyouExec(dbHelper, cond));
            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion


    #region 帳票用データ取得G

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得G
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTyouhyouDataG(DatabaseHelper dbHelper, CondS02 cond)
    {
        // cond:UnsokaishaNo
        try
        {
            DataSet dsRet = new DataSet();

            //----- データ取得 -----
            // 運送会社マスタ
            DataTable dsUnso = this.GetUnsokaisyaTyouhyouExec(dbHelper, cond);
            dsUnso.TableName = ComDefine.DTTBL_SIPPING_G;
            dsRet.Tables.Add(dsUnso);
            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 帳票用データ取得H

    /// --------------------------------------------------
    /// <summary>
    /// 帳票用データ取得H
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTyouhyouDataH(DatabaseHelper dbHelper, CondS02 cond)
    {
        // cond:PackingNo
        try
        {
            DataSet dsRet = new DataSet();

            //----- データ取得 -----
            // 出荷明細(一覧用)
            DataTable dtShukka = this.GetShukkaListTyouhyouExec(dbHelper, cond);
            dsRet.Tables.Add(dtShukka);
            return dsRet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region SQL実行

    #region S0200010:出荷情報登録

    #region SELECT

    #region Box出荷

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷データ取得時のチェック用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetCheckBoxData(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TBM.BOX_NO");
            sb.ApdL("     , TBM.SHUKKA_DATE");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS DISP_NAME");
            sb.ApdL("     , MNS.SHIP AS DISP_SHIP");
            sb.ApdL("     , TAR.AR_NO AS DISP_AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN TAR.JP_UNSOKAISHA_NAME ELSE TSM.UNSOKAISHA_NAME END AS UNSOKAISHA_NAME");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '' ELSE TSM.INVOICE_NO END AS INVOICE_NO");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN TAR.JP_OKURIJYO_NO ELSE TSM.OKURIJYO_NO END AS OKURIJYO_NO");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '' ELSE TSM.BL_NO END AS BL_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            sb.ApdL("     , TSM.AR_NO AS TSM_AR_NO");
            // ↑
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TBM.BOX_NO");
            sb.ApdL("     , TBM.SHUKKA_DATE");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , TAR.JP_UNSOKAISHA_NAME");
            sb.ApdL("     , TAR.JP_OKURIJYO_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            sb.ApdL("     , TSM.AR_NO");
            // ↑
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       UKEIRE_CNT DESC");
            sb.ApdL("     , KOJI_NO ASC");
            sb.ApdL("     , PALLET_NO ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷データ取得実行部
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    public DataSet GetBoxDataExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '{0}' ELSE '{1}' END AS {2}",
                                                cond.TextKonpozumi,
                                                cond.TextShukka,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット出荷

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷データ取得時のチェック用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetCheckPalletData(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TPM.PALLET_NO");
            sb.ApdL("     , TPM.SHUKKA_DATE");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS DISP_NAME");
            sb.ApdL("     , MNS.SHIP AS DISP_SHIP");
            sb.ApdL("     , TAR.AR_NO AS DISP_AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN TAR.JP_UNSOKAISHA_NAME ELSE TSM.UNSOKAISHA_NAME END AS UNSOKAISHA_NAME");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '' ELSE TSM.INVOICE_NO END AS INVOICE_NO");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN TAR.JP_OKURIJYO_NO ELSE TSM.OKURIJYO_NO END AS OKURIJYO_NO");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '' ELSE TSM.BL_NO END AS BL_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.AR_NO AS TSM_AR_NO");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TPM.PALLET_NO");
            sb.ApdL("     , TPM.SHUKKA_DATE");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , TAR.JP_UNSOKAISHA_NAME");
            sb.ApdL("     , TAR.JP_OKURIJYO_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            sb.ApdL("     , TSM.AR_NO");
            // ↑
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       UKEIRE_CNT DESC");
            sb.ApdL("     , KOJI_NO ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷データ取得実行部
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public DataSet GetPalletDataExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("CASE WHEN TSM2.SHUKKA_DATE IS NULL THEN '{0}' ELSE '{1}' END AS {2}",
                                                cond.TextKonpozumi,
                                                cond.TextShukka,
                                                ComDefine.FLD_BTN_STATE);
            string buttonDetail = string.Format("'{0}' AS {1}", cond.TextShosai, ComDefine.FLD_BTN_DETAIL);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM2.SHUKKA_DATE");
            sb.ApdL("     , TBM.VERSION");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdN("     , ").ApdL(buttonDetail);
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            // BoxNo.単位でTagNo.の1件目のデータを表示する為の副問合せ
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("             SELECT");
            sb.ApdL("                    ROW_NUMBER() OVER(PARTITION BY BOX_NO ORDER BY TAG_NO) AS ROW_NO");
            sb.ApdL("                  , SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , TAG_NO");
            sb.ApdL("                  , SEIBAN");
            sb.ApdL("                  , CODE");
            sb.ApdL("                  , ZUMEN_OIBAN");
            sb.ApdL("                  , AREA");
            sb.ApdL("                  , FLOOR");
            sb.ApdL("                  , KISHU");
            sb.ApdL("                  , ST_NO");
            sb.ApdL("                  , HINMEI_JP");
            sb.ApdL("                  , HINMEI");
            sb.ApdL("                  , ZUMEN_KEISHIKI");
            sb.ApdL("                  , KUWARI_NO");
            sb.ApdL("                  , NUM");
            sb.ApdL("                  , BOX_NO");
            sb.ApdL("                  , PALLET_NO");
            sb.ApdL("                  , SHUKKA_DATE");
            sb.ApdL("                  , BIKO");
            sb.ApdL("                  , M_NO");
            sb.ApdL("               FROM T_SHUKKA_MEISAI");
            sb.ApdL("              WHERE");
            sb.ApdN("                    PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("            ) TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                 AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                 AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL("                 AND TSM.ROW_NO = 1");
            // BoxNo.単位でTagNo.に出荷日付が設定されているデータが在れば"出荷"無ければ"梱包済"とする為の副問合せ
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("             SELECT");
            sb.ApdL("                    SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , BOX_NO");
            sb.ApdL("                  , PALLET_NO");
            sb.ApdL("                  , MAX(SHUKKA_DATE) AS SHUKKA_DATE");
            sb.ApdL("               FROM T_SHUKKA_MEISAI");
            sb.ApdL("              WHERE");
            sb.ApdN("                    PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("              GROUP BY");
            sb.ApdL("                    SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , BOX_NO");
            sb.ApdL("                  , PALLET_NO");
            sb.ApdL("            ) TSM2 ON TSM.SHUKKA_FLAG = TSM2.SHUKKA_FLAG");
            sb.ApdL("                  AND TSM.NONYUSAKI_CD = TSM2.NONYUSAKI_CD");
            sb.ApdL("                  AND TSM.BOX_NO = TSM2.BOX_NO");
            sb.ApdL("                  AND TSM.PALLET_NO = TSM2.PALLET_NO");
            sb.ApdL(" INNER JOIN T_BOXLIST_MANAGE TBM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                                AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                                AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠出荷

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷データ取得時のチェック用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>Y.Higuchi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetCheckKiwakuData(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHUKKA_DATE");
            sb.ApdL("     , TK.SAGYO_FLAG");
            sb.ApdL("     , TK.KOJI_NAME AS DISP_NAME");
            sb.ApdL("     , TK.SHIP AS DISP_SHIP");
            sb.ApdL("     , TAR.AR_NO AS DISP_AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TK.UNSOKAISHA_NAME");
            sb.ApdL("     , TK.INVOICE_NO");
            sb.ApdL("     , TK.OKURIJYO_NO");
            sb.ApdL("     , CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '' ELSE TSM.BL_NO END AS BL_NO");
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            sb.ApdL("     , TSM.AR_NO AS TSM_AR_NO");
            // ↑
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU TK");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHUKKA_DATE");
            sb.ApdL("     , TK.SAGYO_FLAG");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TAR.JYOKYO_FLAG");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TK.UNSOKAISHA_NAME");
            sb.ApdL("     , TK.INVOICE_NO");
            sb.ApdL("     , TK.OKURIJYO_NO");
            // 2011/03/11 K.Tsutsumi Add T_ARが存在しなくても続行可能
            sb.ApdL("     , TSM.AR_NO");
            // ↑
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       UKEIRE_CNT DESC");
            sb.ApdL("     , KOJI_NO ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷データ取得実行部
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/07/23</create>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public DataSet GetKiwakuDataExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("CASE WHEN TKM.SHUKKA_DATE IS NULL THEN '{0}' ELSE '{1}' END AS {2}",
                                                cond.TextKonpozumi,
                                                cond.TextShukka,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TK.KOJI_NO");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , CASE WHEN LEN(RTRIM(TK.SHIP)) = 0 THEN '' ELSE RTRIM(TK.SHIP) + '-' END + RIGHT('000' + CAST(TKM.CASE_NO AS VARCHAR), 3) AS CNO");
            sb.ApdL("     , TKM.STYLE");
            sb.ApdL("     , TKM.ITEM");
            sb.ApdL("     , TKM.DESCRIPTION_1");
            sb.ApdL("     , TKM.DESCRIPTION_2");
            sb.ApdL("     , TKM.SHUKKA_DATE");
            // MultiRowを使用するため必ずカラムにセットしなければいけない。
            // さらにDateEditorに格納してもミリ秒部分が切り捨てられてしまうので
            // 文字列型にキャストして使用する。
            sb.ApdL("     , CAST( TKM.VERSION AS VARCHAR) AS VERSION");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU TK");
            sb.ApdL(" INNER JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("             SELECT");
            sb.ApdL("                    SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , KOJI_NO");
            sb.ApdL("                  , CASE_ID");
            sb.ApdL("               FROM T_SHUKKA_MEISAI");
            sb.ApdL("              GROUP BY");
            sb.ApdL("                    SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , KOJI_NO");
            sb.ApdL("                  , CASE_ID");
            sb.ApdL("            ) TSM ON TKM.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                 AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKM.CASE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Box出荷(出荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// Box出荷で使用する出荷済みのチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>件数データ</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckBoxDataShukkaDate(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.NONYUSAKI_CD");
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdN("       TSM.NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット出荷(出荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// パレット出荷で使用する出荷済みチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="isBoxNo">BoxNo単位</param>
    /// <returns>件数データ</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckPalletDataShukkaDate(DatabaseHelper dbHelper, CondS02 cond, bool isBoxNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string unitField = string.Empty;

            if (isBoxNo)
            {
                unitField = "TSM.BOX_NO";
            }
            else
            {
                unitField = "TSM.PALLET_NO";
            }

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.NONYUSAKI_CD");
            sb.ApdN("     , ").ApdL(unitField);
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdN("       TSM.NONYUSAKI_CD");
            sb.ApdL("     , ").ApdL(unitField);

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠出荷(出荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 木枠出荷で使用する出荷済みチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="unitFlag">集約単位</param>
    /// <returns>件数データ</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckKiwakuDataShukkaDate(DatabaseHelper dbHelper, CondS02 cond, S02UnitFlag unitFlag)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string unitField = string.Empty;

            switch (unitFlag)
            {
                case S02UnitFlag.Kiwaku:
                    unitField = "TKM.KOJI_NO";
                    break;
                case S02UnitFlag.KiwakuMeisai:
                    unitField = "TKM.CASE_ID";
                    break;
                case S02UnitFlag.PalletNo:
                    unitField = "TSM.PALLET_NO";
                    break;
                case S02UnitFlag.BoxNo:
                    unitField = "TSM.BOX_NO";
                    break;
            }

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.NONYUSAKI_CD");
            sb.ApdN("     , ").ApdL(unitField);
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_DATE");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TKM.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                               AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.NONYUSAKI_CD");
            sb.ApdL("     , ").ApdL(unitField);
            sb.ApdL(" ORDER BY");
            sb.ApdN("       TSM.NONYUSAKI_CD");
            sb.ApdL("     , ").ApdL(unitField);

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR出荷(出荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// AR出荷で使用する出荷済みチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="unitFlag">集約単位</param>
    /// <returns>件数データ</returns>
    /// <create>T.Wakamatsu 2016/01/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckARDataShukkaDate(DatabaseHelper dbHelper, CondS02 cond, S02UnitFlag unitFlag)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , AR_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            switch (unitFlag)
            {
                case S02UnitFlag.Kiwaku:
                    sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("SHUKKA_NO");
                    break;
                case S02UnitFlag.PalletNo:
                    sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("SHUKKA_NO");
                    break;
                case S02UnitFlag.BoxNo:
                    sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("SHUKKA_NO");
                    break;
            }
            sb.ApdL(" ORDER BY");
            sb.ApdN("       AR_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_NO", cond.ShukkaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT
    #endregion

    #region UPDATE

    #region 出荷明細データ

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="dt">更新データテーブル</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisai(DatabaseHelper dbHelper, CondS02 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            sb.ApdN("     , INVOICE_NO = ").ApdN(this.BindPrefix).ApdL("INVOICE_NO");
            sb.ApdN("     , OKURIJYO_NO = ").ApdN(this.BindPrefix).ApdL("OKURIJYO_NO");
            sb.ApdN("     , BL_NO = ").ApdN(this.BindPrefix).ApdL("BL_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.TAG_NO))
            {
                sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            }
            if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.BOX_NO))
            {
                sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            }
            if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.PALLET_NO))
            {
                sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            }
            if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.KOJI_NO))
            {
                sb.ApdN("   AND KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            }
            if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.CASE_ID))
            {
                sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            }

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", cond.JyotaiFlag));
                if (cond.ShukkaDate != DBNull.Value)
                {
                    cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
                }
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", cond.UnsokaishaName));
                paramCollection.Add(iNewParam.NewDbParameter("INVOICE_NO", cond.InvoiceNo));
                paramCollection.Add(iNewParam.NewDbParameter("OKURIJYO_NO", cond.OkurijyoNo));
                paramCollection.Add(iNewParam.NewDbParameter("BL_NO", cond.BLNo));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));

                if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.TAG_NO))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                }
                if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.BOX_NO))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));
                }
                if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.PALLET_NO))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));
                }
                if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.KOJI_NO))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KOJI_NO)));
                }
                if (dt.Columns.Contains(Def_T_SHUKKA_MEISAI.CASE_ID))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.CASE_ID)));
                }

                // SQL実行
                record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region BOXリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="boxNo">BoxNo.</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxManage(DatabaseHelper dbHelper, CondS02 cond, string boxNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BOXLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            if (cond.ShukkaDate != DBNull.Value)
            {
                cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", cond.KanriFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", boxNo));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレットリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="palletNo">パレットNo.</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletManage(DatabaseHelper dbHelper, CondS02 cond, string palletNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            if (cond.ShukkaDate != DBNull.Value)
            {
                cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", cond.KanriFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", palletNo));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠明細データ

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="kojiNo">工事識別管理NO</param>
    /// <param name="caseID">内部管理用キー</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuMeisai(DatabaseHelper dbHelper, CondS02 cond, string kojiNo, string caseID)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            // バインド変数設定
            if (cond.ShukkaDate != DBNull.Value)
            {
                cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseID));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠データ

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="kojiNo">工事識別管理NO</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwaku(DatabaseHelper dbHelper, CondS02 cond, string kojiNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU");
            sb.ApdL("SET");
            sb.ApdN("       SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            sb.ApdN("     , INVOICE_NO = ").ApdN(this.BindPrefix).ApdL("INVOICE_NO");
            sb.ApdN("     , OKURIJYO_NO = ").ApdN(this.BindPrefix).ApdL("OKURIJYO_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");

            // バインド変数設定
            object okurijyoNo = DBNull.Value;
            if (cond.BLNo == null || cond.BLNo == DBNull.Value || string.IsNullOrEmpty(cond.BLNo.ToString()))
            {
                okurijyoNo = cond.OkurijyoNo;
            }
            else
            {
                okurijyoNo = cond.BLNo;
            }
            if (cond.ShukkaDate != DBNull.Value)
            {
                cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", cond.SagyoFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", cond.UnsokaishaName));
            paramCollection.Add(iNewParam.NewDbParameter("INVOICE_NO", cond.InvoiceNo));
            paramCollection.Add(iNewParam.NewDbParameter("OKURIJYO_NO", okurijyoNo));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ARデータ

    /// --------------------------------------------------
    /// <summary>
    /// ARデータ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdAR(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR ");
            sb.ApdL("SET");
            if (cond.IsKonpo2Shukka || cond.IsShukka2Shukka)
            {
                sb.ApdN("       JP_UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("JP_UNSOKAISHA_NAME");
                sb.ApdN("     , JP_OKURIJYO_NO = ").ApdN(this.BindPrefix).ApdL("JP_OKURIJYO_NO");
                object okurijyoNo = DBNull.Value;
                if (cond.OkurijyoNo == null || cond.OkurijyoNo == DBNull.Value || string.IsNullOrEmpty(cond.OkurijyoNo.ToString()))
                {
                    okurijyoNo = cond.BLNo;
                }
                else
                {
                    okurijyoNo = cond.OkurijyoNo;
                }
                paramCollection.Add(iNewParam.NewDbParameter("JP_UNSOKAISHA_NAME", cond.UnsokaishaName));
                paramCollection.Add(iNewParam.NewDbParameter("JP_OKURIJYO_NO", okurijyoNo));
            }
            if (cond.IsKonpo2Shukka)
            {
                sb.ApdN("     , SHUKKA_USER_ID = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
                sb.ApdN("     , SHUKKA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
                sb.ApdN("     , SHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.40
                sb.ApdN("     , JP_KOJYOSHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("JP_KOJYOSHUKKA_DATE");
                // @@@ ↑
                if (cond.ShukkaDate != DBNull.Value)
                {
                    cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
                }
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", cond.ShukkaDate));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", cond.ShukkaUserID));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", cond.ShukkaUserName));
                //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.40
                if (cond.ShukkaDate != DBNull.Value)
                {
                    cond.ShukkaDate = UtilConvert.ToDateTime(cond.ShukkaDate).ToShortDateString();
                }
                paramCollection.Add(iNewParam.NewDbParameter("JP_KOJYOSHUKKA_DATE", cond.ShukkaDate));
                // @@@ ↑
            }
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #endregion

    #region DELETE
    #endregion

    #endregion

    #region S0200020:出荷情報明細

    #region SELECT

    #region BoxNoの出荷明細データを取得

    /// --------------------------------------------------
    /// <summary>
    /// BoxNoの出荷明細データを取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>BoxNoの出荷明細データ</returns>
    /// <create>Y.Higuchi 2010/07/27</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>J.Chen 2024/11/20 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxMeisai(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "TSM.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.CASE_ID");
            sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL("     , TSM.UKEIRE_DATE");
            sb.ApdL("     , TSM.UKEIRE_USER_NAME");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // Box No.
            if (cond.BoxNo != null)
            {
                fieldName = "BOX_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BoxNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region S0200030:出荷情報照会

    #region SELECT

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update>Y.Higuchi 2010/10/28</update>
    /// <update>Y.Higuchi 2010/11/25</update>
    /// <update>H.Tajimi 2015/11/18 Free1,Free2追加</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/07 出荷情報/複数便を選択し表示対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>T.Nakata 2018/12/14 手配業務対応</update>
    /// <update>K.Tsutsumi 2019/03/08 手配情報を表示</update>
    /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
    /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
    /// <update>TW-Tsuji 2022/10/07 出荷元列追加</update>
    /// <update>J.Chen 2022/12/19 TAG便名列追加</update>
    /// <update>T.Okuda 2023/07/14 表示選択修正</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加（STEP17）</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkaMeisaiExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "TSM.";
            string fieldName = string.Empty;
            const string CASE_WHEN_DISP_SHIP_ARNO = "CASE WHEN TSM.SHUKKA_FLAG = {0} THEN MNS.SHIP ELSE TSM.AR_NO END AS SHIP_AR_NO";

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS JYOTAI_NAME");
            sb.ApdN("     , ").ApdL(string.Format(CASE_WHEN_DISP_SHIP_ARNO, this.BindPrefix + "SHUKKA_FLAG_NORMAL"));
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.CASE_ID");
            sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL("     , TSM.UKEIRE_DATE");
            sb.ApdL("     , TSM.UKEIRE_USER_NAME");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , RTRIM(TK.SHIP) + '-' + CAST(TKM.CASE_NO AS VARCHAR) AS KIWAKU_NO");
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TSM.SHUKKA_FLAG");
            sb.ApdL("     , MNS.BUKKEN_NO");
            sb.ApdL("     , TSM.TAG_NONYUSAKI_CD");
            sb.ApdL("     , TTM.ECS_QUOTA");
            sb.ApdL("     , TTM.ECS_NO");
            sb.ApdL("     , TTM.SETTEI_DATE");
            sb.ApdL("     , TTM.NOUHIN_SAKI");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , MC_TF.ITEM_NAME AS TEHAI_FLAG_NAME");
            sb.ApdL("     , MC_QU.ITEM_NAME AS QUANTITY_UNIT_NAME");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TTM.MAKER");
            sb.ApdL("     , TTM.UNIT_PRICE");
            sb.ApdL("     , MC_EF.ITEM_NAME AS ESTIMATE_FLAG_NAME");
            sb.ApdL("     , CASE");
            sb.ApdL("        WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdL("               OR MZK.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdN("        ) THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdL("NOT_EXISTS_VALUE");
            sb.ApdL("        END AS EXISTS_PICTURE");
            sb.ApdL("     , MZK1.FILE_NAME AS FILE_NAME1");
            sb.ApdL("     , MZK1.SAVE_DIR AS SAVE_DIR1");
            sb.ApdL("     , MZK2.FILE_NAME AS FILE_NAME2");
            sb.ApdL("     , MZK2.SAVE_DIR AS SAVE_DIR2");
            sb.ApdL("     , TKM.PRINT_CASE_NO");

            // 出荷元列追加による 2022/10/07（TW-Tsuji）
            //　出荷元マスタ（M_SHIP_FROM）を外部結合して、項目を追加
            //　　　SHIP_FROM_NO    出荷目CD（表示しないが項目抽出） 
            //　　　SHIP_FROM_NAME  名称　  （出荷元として表示する）
            sb.ApdL("     , MSF.SHIP_FROM_NO");
            sb.ApdL("     , MSF.SHIP_FROM_NAME AS SHIP_FROM_NAME");



            //【Step15_1_2】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
            //　単価（JPY）のブランク列を追加.
            sb.ApdL("     , '' AS UNIT_PRICE_BLANK");

            sb.ApdL("     , TSM.TAG_SHIP");

            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'DISP_JYOTAI_FLAG'");
            sb.ApdL("                        AND COM.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN T_KIWAKU TK ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                               AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT JOIN M_COMMON MC_TF ON MC_TF.GROUP_CD = 'TEHAI_FLAG'"); //手配区分
            sb.ApdL("                          AND MC_TF.VALUE1 = TTM.TEHAI_FLAG");
            sb.ApdN("                          AND MC_TF.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON MC_QU ON MC_QU.GROUP_CD = 'QUANTITY_UNIT'"); //数量単位名
            sb.ApdL("                          AND MC_QU.VALUE1 = TTM.QUANTITY_UNIT");
            sb.ApdN("                          AND MC_QU.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON MC_EF ON MC_EF.GROUP_CD = 'ESTIMATE_FLAG'"); //見積区分
            sb.ApdL("                          AND MC_EF.VALUE1 = TTM.ESTIMATE_FLAG");
            sb.ApdN("                          AND MC_EF.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK1 ON MZK1.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK2 ON MZK2.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");

            // 出荷元列追加による 2022/10/07（TW-Tsuji）
            //　出荷元マスタ（M_SHIP_FROM）を、納入先テーブルと外部結合
            //　抽出条件は、未使用フラグが"使用"　定数：UNUSED_FLAG.USED_VALUE1　'0'
            sb.ApdL("  LEFT JOIN M_SHIP_FROM MSF ON MSF.SHIP_FROM_NO = MNS.SHIP_FROM_CD");
            sb.ApdN("                          AND MSF.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");


            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // 手配連携No
            if (!string.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                fieldName = "TEHAI_RENKEI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TehaiRenkeiNo));
            }
            // 2015/12/07 H.Tajimi 複数便表示対応
            else if (cond.NonyusakiCDs != null)
            {
                fieldName = "NONYUSAKI_CD";
                string value = string.Empty;
                value += "(";
                value += this.ConvArrayToString(",", cond.NonyusakiCDs);
                value += ")";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" IN ").ApdL(value);
            }
            // ↑
            else
            {
                if (cond.NonyusakiName != null)
                {
                    fieldName = "BUKKEN_NAME";
                    sb.ApdN("   AND ").ApdN("BK." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                    paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiName));
                }
            }
            // 元TAG便
            if (!string.IsNullOrEmpty(cond.TagShip))
            {
                fieldName = "TAG_SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.TagShip + "%"));
            }
            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
            }
            // 品名(和文)
            if (!string.IsNullOrEmpty(cond.HinmeiJp))
            {
                fieldName = "HINMEI_JP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.HinmeiJp + "%"));
            }
            // 品名
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                fieldName = "HINMEI";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.Hinmei + "%"));
            }
            // 図番/形式
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                fieldName = "ZUMEN_KEISHIKI";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.ZumenKeishiki + "%"));
            }
            // 木枠No/Pallet No/Box No/Tag No
            if (!string.IsNullOrEmpty(cond.KiwakuNo)
                && !string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldPrefix = "TK.";
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
                fieldPrefix = "TKM.";
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.KiwakuNo))
            {
                fieldPrefix = "TK.";
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KiwakuNo));
            }
            else if (!string.IsNullOrEmpty(cond.CaseNo))
            {
                fieldPrefix = "TKM.";
                fieldName = "CASE_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseNo));
            }
            else if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                fieldPrefix = "TSM."; 
                fieldName = "PALLET_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.PalletNo));
            }
            else if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                fieldPrefix = "TSM."; 
                fieldName = "BOX_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BoxNo));
            }
            else if (!string.IsNullOrEmpty(cond.TagNo))
            {
                fieldPrefix = "TSM.";
                fieldName = "TAG_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TagNo));
            }

            //// 表示選択
            //if (cond.DispSelect != null)
            //{
            //    if (cond.DispSelect != DISP_SELECT.ALL_VALUE1)
            //    {
            //        // 全て以外
            //        string expression = string.Empty;
            //        string value = string.Empty;
            //        if (cond.DispSelect == DISP_SELECT.SHUKAZUMI_VALUE1)
            //        {
            //            // 集荷済
            //            expression = " = ";
            //            value = UtilConvert.PutQuot(JYOTAI_FLAG.SHUKAZUMI_VALUE1);
            //        }
            //        else if (cond.DispSelect == DISP_SELECT.BOXZUMI_VALUE1)
            //        {
            //            // B梱包済
            //            expression = " = ";
            //            value = UtilConvert.PutQuot(JYOTAI_FLAG.BOXZUMI_VALUE1);
            //        }
            //        else if (cond.DispSelect == DISP_SELECT.PALLETZUMI_VALUE1)
            //        {
            //            // P梱包済
            //            expression = " = ";
            //            value = UtilConvert.PutQuot(JYOTAI_FLAG.PALLETZUMI_VALUE1);
            //        }
            //        else if (cond.DispSelect == DISP_SELECT.KIWAKUKONPO_VALUE1)
            //        {
            //            // 木枠梱包済
            //            expression = " = ";
            //            value = UtilConvert.PutQuot(JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
            //        }
            //        else
            //        {
            //            expression = " IN ";
            //            if (cond.DispSelect == DISP_SELECT.MISHUKA_VALUE1)
            //            {
            //                // 未集荷
            //                value += "(";
            //                value += UtilConvert.PutQuot(JYOTAI_FLAG.SHINKI_VALUE1);
            //                value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1);
            //                value += ")";
            //            }
            //            else if (cond.DispSelect == DISP_SELECT.KONPOZUMI_VALUE1)
            //            {
            //                // 梱包済
            //                value += "(";
            //                value += UtilConvert.PutQuot(JYOTAI_FLAG.BOXZUMI_VALUE1);
            //                value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.PALLETZUMI_VALUE1);
            //                value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
            //                value += ")";
            //            }
            //            else
            //            {
            //                // 出荷済
            //                value += "(";
            //                value += UtilConvert.PutQuot(JYOTAI_FLAG.SHUKKAZUMI_VALUE1);
            //                value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.UKEIREZUMI_VALUE1);
            //                value += ")";
            //            }
            //        }
            //        fieldPrefix = "TSM.";
            //        fieldName = "JYOTAI_FLAG";
            //        sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(expression).ApdL(value);
            //    }
            //}
            // ソート
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHIP_AR_NO");
            sb.ApdL("     , TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("NOT_EXISTS_VALUE", ComDefine.NOT_EXISTS_PICTURE_VALUE));

            // 出荷元列追加による 2022/10/07（TW-Tsuji）
            //　出荷元マスタの抽出条件をバインド変数設定
            //　抽出条件は、未使用フラグが"使用"　定数：UNUSED_FLAG.USED_VALUE1　'0'
            paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", UNUSED_FLAG.USED_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 納入先、便 or ARNo単位の進捗件数取得
    /// --------------------------------------------------
    /// <summary>
    /// 納入先、便 or ARNo単位の進捗件数取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">検索条件</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2011/03/09</create>
    /// <update>H.Tajimi 2015/12/07 出荷情報/複数便を選択し表示対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetProgress(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS COUNT_ALL");
            sb.ApdL("     , COUNT(TSM.SHUKA_DATE) AS COUNT_SHUKA");
            sb.ApdL("     , COUNT(TSM.BOXKONPO_DATE) AS COUNT_BOXKONPO");
            sb.ApdL("     , COUNT(TSM.PALLETKONPO_DATE) AS COUNT_PALLETKONPO");
            sb.ApdL("     , COUNT(TSM.KIWAKUKONPO_DATE) AS COUNT_KIWAKUKONPO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS");
            sb.ApdN("    ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdN("   AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");

            if (string.IsNullOrEmpty(cond.NonyusakiCD) == false)
            {
                sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            }
            // 2015/12/07 H.Tajimi 複数便表示対応
            else if (cond.NonyusakiCDs != null)
            {
                string value = string.Empty;
                value += "(";
                value += this.ConvArrayToString(",", cond.NonyusakiCDs);
                value += ")";
                sb.ApdN("   AND TSM.NONYUSAKI_CD IN ").ApdL(value);
            }
            // ↑
            else
            {
                sb.ApdN("   AND BK.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.NonyusakiName));
            }

            if (string.IsNullOrEmpty(cond.ARNo) == false)
            {
                sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, ComDefine.DTTBL_PROGRESS);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region S0200040:ShippingDocument作成

    #region SELECT

    #region 荷受マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update>K.Tsutsumi 2018/01/23 並び順対応</update>
    /// <update>K.Tsutsumi 2019/03/23 使い勝手の面で名前でソート</update>
    /// <update>D.Okumura 2020/10/06 名前順→ソートNo、名前順</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    private DataTable GetConsignExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_CONSIGN.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  CONSIGN_CD ");
            sb.ApdL("  , NAME ");
            sb.ApdL("  , ADDRESS ");
            sb.ApdL("  , TEL1 ");
            sb.ApdL("  , TEL2 ");
            sb.ApdL("  , FAX ");
            sb.ApdL("  , CHINA_FLAG ");
            sb.ApdL("  , USCI_CD ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , SORT_NO ");
            sb.ApdL("FROM ");
            sb.ApdL("  M_CONSIGN ");
            sb.ApdL("WHERE ");
            sb.ApdL("	1 = 1 ");

            // 荷受CD
            if (!string.IsNullOrEmpty(cond.ConsignCd))
            {
                sb.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
                paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCd));
            }

            sb.ApdL("ORDER BY ");
            sb.ApdL("  SORT_NO,NAME ");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 配送先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update>K.Tsutsumi 2018/01/23 並び順対応</update>
    /// <update>K.Tsutsumi 2019/03/23 使い勝手の面で名前でソート</update>
    /// <update>D.Okumura 2020/10/06 名前順→ソートNo、名前順</update>
    /// --------------------------------------------------
    public DataSet GetDeliverExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  DELIVER_CD ");
            sb.ApdL("  , NAME ");
            sb.ApdL("  , ADDRESS ");
            sb.ApdL("  , TEL1 ");
            sb.ApdL("  , TEL2 ");
            sb.ApdL("  , FAX ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , SORT_NO ");
            sb.ApdL("FROM ");
            sb.ApdL("  M_DELIVER ");
            sb.ApdL("WHERE ");
            sb.ApdL("	1 = 1 ");

            // 配送先CD
            if (!string.IsNullOrEmpty(cond.DeliverCd))
            {
                sb.ApdN("   AND DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");
                paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", cond.DeliverCd));
            }
            
            sb.ApdL("ORDER BY ");
            sb.ApdL("  SORT_NO,NAME ");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_DELIVER.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿情報取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿情報取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update>H.Tajimi 2020/04/21 荷姿表を入力途中で保存可能とする</update>
    /// --------------------------------------------------
    public DataSet GetNisugataExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");

            sb.ApdL("	 UM.UNSOKAISHA_NAME ");
            sb.ApdL("	,PT.INVOICE_NO ");
            sb.ApdL("	,PMT.SHUKKA_FLAG ");
            sb.ApdL("	,BM.BUKKEN_NAME ");
            sb.ApdL("	,NM.SHIP ");
            sb.ApdL("	,PMT.AR_NO ");
            sb.ApdL("	,PMT.ECS_NO ");
            sb.ApdL("	,PMT.ATTN ");
            sb.ApdL("	,PT.CONSIGN_CD ");
            sb.ApdL("	,PT.CONSIGN_ATTN ");
            sb.ApdL("	,PT.DELIVER_CD ");
            sb.ApdL("	,PT.DELIVER_ATTN ");
            sb.ApdL("	,PT.INTERNAL_PO_NO ");
            sb.ApdL("	,PT.TRADE_TERMS_FLAG ");
            sb.ApdL("	,PT.TRADE_TERMS_ATTR ");
            sb.ApdL("	,PT.SUBJECT ");
            sb.ApdL("	,PT.VERSION ");

            sb.ApdL("	,NM.ESTIMATE_FLAG ");
            sb.ApdL("	,PT.PACKING_NO ");
            sb.ApdL("	,PT.CT_QTY ");
            sb.ApdL("   ,COM1.ITEM_NAME AS DISP_TRADE_TERMS_FLAG");
            sb.ApdL("	,PT.UNSOKAISHA_CD ");
            sb.ApdL("	,UM.KOKUNAI_GAI_FLAG ");

            sb.ApdL("FROM ");
            sb.ApdL("	(SELECT pmt.PACKING_NO, MIN(pmt.NO) AS MIN_NO ");
            sb.ApdL("	 FROM T_PACKING_MEISAI pmt ");
            sb.ApdN("    WHERE pmt.CANCEL_FLAG != ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("	 GROUP BY pmt.PACKING_NO ) AS s ");
            sb.ApdL("	LEFT JOIN T_PACKING PT ON PT.PACKING_NO = s.PACKING_NO ");
            sb.ApdL("	LEFT JOIN T_PACKING_MEISAI PMT ON s.PACKING_NO = PMT.PACKING_NO AND s.MIN_NO = PMT.NO ");
            sb.ApdL("	LEFT JOIN M_UNSOKAISHA UM ON UM.UNSOKAISHA_NO = PT.UNSOKAISHA_CD ");
            sb.ApdL("	LEFT JOIN M_NONYUSAKI NM ON NM.NONYUSAKI_CD = PMT.NONYUSAKI_CD AND NM.SHUKKA_FLAG = PMT.SHUKKA_FLAG ");
            sb.ApdL("	LEFT JOIN M_BUKKEN BM ON BM.BUKKEN_NO = NM.BUKKEN_NO AND BM.SHUKKA_FLAG = NM.SHUKKA_FLAG ");
            sb.ApdL("   LEFT JOIN M_COMMON COM1");
            sb.ApdN("     ON COM1.GROUP_CD = 'TRADE_TERMS_FLAG'");
            sb.ApdL("    AND COM1.VALUE1 = PT.TRADE_TERMS_FLAG");
            sb.ApdN("    AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            sb.ApdL("WHERE ");
            sb.ApdL("	1 = 1 ");
            sb.ApdN("   AND PT.INVOICE_NO IS NOT NULL");
            // 荷姿CD
            if (!string.IsNullOrEmpty(cond.PackingNo))
            {
                sb.ApdN("   AND PT.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            }
            // 国内外
            if (!string.IsNullOrEmpty(cond.Kokunaigai))
            {
                sb.ApdN("   AND UM.KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", cond.Kokunaigai));
            }
            // 出荷日
            if (cond.ShukkaDate != null)
            {
                sb.ApdN("   AND PT.SYUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SYUKKA_DATE");
                paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_DATE", cond.ShukkaDate));
            }
            // 発行状態
            if (!string.IsNullOrEmpty(cond.HakkouFlag))
            {
                sb.ApdN("   AND PT.HAKKO_FLAG = ").ApdN(this.BindPrefix).ApdL("HAKKO_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("HAKKO_FLAG", cond.HakkouFlag));
            }
            sb.ApdL("ORDER BY ");
            sb.ApdL("	PT.INVOICE_NO ");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_PACKING.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿明細取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿情報取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/11</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// --------------------------------------------------
    public DataSet GetNisugataMeisaiExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  PACKING_NO ");
            sb.ApdL("  , NO ");
            sb.ApdL("  , CT_NO ");
            sb.ApdL("  , CANCEL_FLAG ");
            sb.ApdL("  , SHUKKA_FLAG ");
            sb.ApdL("  , NONYUSAKI_CD ");
            sb.ApdL("  , AR_NO ");
            sb.ApdL("  , ECS_NO ");
            sb.ApdL("  , SEIBAN_CODE ");
            sb.ApdL("  , DOUKON_FLAG ");
            sb.ApdL("  , FORM_STYLE_FLAG ");
            sb.ApdL("  , SIZE_L ");
            sb.ApdL("  , SIZE_W ");
            sb.ApdL("  , SIZE_H ");
            sb.ApdL("  , GRWT ");
            sb.ApdL("  , PRODUCT_NAME ");
            sb.ApdL("  , ATTN ");
            sb.ApdL("  , NOTE ");
            sb.ApdL("  , PL_TYPE ");
            sb.ApdL("  , CASE_NO ");
            sb.ApdL("  , PALLET_NO ");
            sb.ApdL("  , BOX_NO ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_PACKING_MEISAI ");
            sb.ApdL("WHERE ");
            sb.ApdL("	1 = 1 ");
            // 荷姿CD
            if (!string.IsNullOrEmpty(cond.PackingNo))
            {
                sb.ApdN("   AND PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            }
            sb.ApdN(" AND CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("ORDER BY ");
            sb.ApdL("	NO ");
            
            // パラメータ
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_PACKING_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 納入先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNonyusakiExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  SHUKKA_FLAG ");
            sb.ApdL("  , BUKKEN_NO ");
            sb.ApdL("  , NONYUSAKI_CD ");
            sb.ApdL("  , NONYUSAKI_NAME ");
            sb.ApdL("  , SHIP ");
            sb.ApdL("  , KANRI_FLAG ");
            sb.ApdL("  , LIST_FLAG_NAME0 ");
            sb.ApdL("  , LIST_FLAG_NAME1 ");
            sb.ApdL("  , LIST_FLAG_NAME2 ");
            sb.ApdL("  , LIST_FLAG_NAME3 ");
            sb.ApdL("  , LIST_FLAG_NAME4 ");
            sb.ApdL("  , LIST_FLAG_NAME5 ");
            sb.ApdL("  , LIST_FLAG_NAME6 ");
            sb.ApdL("  , LIST_FLAG_NAME7 ");
            sb.ApdL("  , REMOVE_FLAG ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , TRANSPORT_FLAG ");
            sb.ApdL("  , ESTIMATE_FLAG ");
            sb.ApdL("  , SHIP_DATE ");
            sb.ApdL("  , SHIP_FROM ");
            sb.ApdL("  , SHIP_TO ");
            sb.ApdL("  , SHIP_NO ");
            sb.ApdL("  , SHIP_SEIBAN ");
            sb.ApdL("FROM ");
            sb.ApdL("  M_NONYUSAKI ");
            sb.ApdL("WHERE ");
            sb.ApdN("   REMOVE_FLAG = ").ApdN(this.BindPrefix).ApdL("REMOVE_FLAG");
            paramCollection.Add(iNewParam.NewDbParameter("REMOVE_FLAG", REMOVE_FLAG.NORMAL_VALUE1));
            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            // 納入先コード
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            }
            sb.ApdL("ORDER BY ");
            sb.ApdL("	NONYUSAKI_NAME ");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 各種チェック処理

    /// --------------------------------------------------
    /// <summary>
    /// 各種チェック処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-92 対応</update>
    /// <update>D.Okumura 2020/10/01 同梱フラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>J.Chen 2022/10/13 EFA_SMS-263 木枠・AR出力制限修正（暫定）</update>
    /// --------------------------------------------------
    public DataSet CheckShippingExec(DatabaseHelper dbHelper, CondS02 cond, bool IsKokugai)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("--共通テーブル ");
            sb.ApdL("WITH T_PACKING_SHUKKAMEISAI (PACKING_NO, CT_NO, PACK, NONYUSAKI_CD, SHUKKA_FLAG, TAG_NO, JYOTAI_FLAG, TEHAI_RENKEI_NO, NET_WT, SAGYO_FLAG) AS ");
            sb.ApdL("( ");
            sb.ApdL("  SELECT P.PACKING_NO, M.CT_NO, M.BOX_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, NULL AS SAGYO_FLAG ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("    AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("    AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("    AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, M.PALLET_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, NULL AS SAGYO_FLAG ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("    AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("    AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("    AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, K.SAGYO_FLAG AS SAGYO_FLAG ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("      AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("      AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, K.SAGYO_FLAG AS SAGYO_FLAG ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("      AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("      AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("      AND S.AR_NO = M.AR_NO ");
            sb.ApdL(") ");
            sb.ApdL("SELECT d.* FROM ( ");
            // {0}行目の荷姿明細が取得出来ませんでした。
            sb.ApdL("SELECT 'S0200040010' AS MSGID ,P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING AS P WHERE NOT EXISTS (SELECT 1 FROM  T_PACKING_MEISAI AS M");
            sb.ApdL("  WHERE P.PACKING_NO = M.PACKING_NO");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL(" ) ");
            // {0}行目の梱包No.{1}の出荷明細を参照できません。荷姿を確認してください。(BOX)
            sb.ApdL("UNION ALL SELECT 'S0200040016' AS MSGID, P.PACKING_NO, M.BOX_NO AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING AS P INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 FROM T_SHUKKA_MEISAI AS S WHERE M.NONYUSAKI_CD = S.NONYUSAKI_CD AND M.SHUKKA_FLAG = S.SHUKKA_FLAG AND M.BOX_NO = S.BOX_NO AND S.PALLET_NO IS NULL) ");
            sb.ApdL("  AND  M.BOX_NO IS NOT NULL ");
            // {0}行目の梱包No.{1}の出荷明細を参照できません。荷姿を確認してください。(PALLET)
            sb.ApdL("UNION ALL SELECT 'S0200040016' AS MSGID, P.PACKING_NO, M.PALLET_NO AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING AS P INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 FROM T_SHUKKA_MEISAI AS S WHERE M.NONYUSAKI_CD = S.NONYUSAKI_CD AND M.SHUKKA_FLAG = S.SHUKKA_FLAG AND M.PALLET_NO = S.PALLET_NO AND S.KOJI_NO IS NULL) ");
            sb.ApdL("    AND  M.PALLET_NO IS NOT NULL ");
            // {0}行目の梱包No.{1}の出荷明細を参照できません。荷姿を確認してください。(木枠・本体)
            sb.ApdL("UNION ALL SELECT 'S0200040016' AS MSGID, P.PACKING_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING AS P INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 ");
            sb.ApdL("    FROM T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    WHERE M.NONYUSAKI_CD = K.NONYUSAKI_CD ");
            sb.ApdL("      AND M.SHUKKA_FLAG = K.SHUKKA_FLAG ");
            sb.ApdL("      AND M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("      ) ");
            sb.ApdL("    AND  M.CASE_NO IS NOT NULL ");
            sb.ApdN("    AND  M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            // {0}行目の梱包No.{1}の出荷明細を参照できません。荷姿を確認してください。(木枠・AR)
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("UNION ALL SELECT 'S0200040016' AS MSGID, P.PACKING_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING AS P INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  WHERE NOT EXISTS (SELECT 1 ");
            sb.ApdL("    FROM T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    WHERE M.NONYUSAKI_CD = K.NONYUSAKI_CD ");
            sb.ApdL("      AND M.SHUKKA_FLAG = K.SHUKKA_FLAG ");
            sb.ApdL("      AND M.CASE_NO = KM.CASE_NO ");
            //sb.ApdL("      AND M.AR_NO = S.AR_NO ");
            sb.ApdL("      ) ");
            sb.ApdL("    AND  M.CASE_NO IS NOT NULL ");
            sb.ApdN("    AND  M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            // {0}行目の梱包No.{1}-{2}の状態が梱包済みとなっていません。明細を削除するか、梱包を完了してから発行してください。
            sb.ApdL("UNION ALL SELECT 'S0200040017' AS MSGID, P.PACKING_NO, P.PACK, P.TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
            //sb.ApdL("  WHERE P.JYOTAI_FLAG <= ").ApdN(this.BindPrefix).ApdL("SHUKAZUMI_VALUE1");
            sb.ApdL("  WHERE P.JYOTAI_FLAG <= ").ApdN(this.BindPrefix).ApdN("SHUKAZUMI_VALUE1 OR P.JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("HIKIWATASHIZUMI_VALUE1");
            // {0}行目の梱包No.{1}-{2}の状態が受け入れ済みとなっているため、出荷状態へ戻すことはできません。
            sb.ApdL("UNION ALL SELECT 'S0200040018' AS MSGID, P.PACKING_NO, P.PACK, P.TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
            //sb.ApdL("  WHERE P.JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdL("UKEIREZUMI_VALUE1");
            sb.ApdL("  WHERE P.JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdL("UKEIREZUMI_VALUE1 AND P.JYOTAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("HIKIWATASHIZUMI_VALUE1");
//            // {0}行目の梱包No.{1}-{2}の重量を0以上で入力してください。
//            sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040019' AS MSGID, P.PACKING_NO, P.PACK, P.TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
//            sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
//            sb.ApdL("  WHERE P.NET_WT < 0 OR P.NET_WT IS NULL ");
            // {0}行目の梱包No.{1}-{2}の手配連携番号が未入力です。確認してください。
            sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040020' AS MSGID, P.PACKING_NO, P.PACK, P.TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
            sb.ApdL("  WHERE P.TEHAI_RENKEI_NO IS NULL");
            // {0}行目の梱包No.{1}-{2}の手配連携番号を参照できません。確認してください。
            sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040030' AS MSGID, P.PACKING_NO, P.PACK, P.TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
            sb.ApdL("  WHERE P.TEHAI_RENKEI_NO IS NOT NULL AND NOT EXISTS (SELECT 1 FROM T_TEHAI_MEISAI AS T WHERE T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO) ");
            // {0}行目の梱包No.{1}の作業状態が梱包完了となっていません。
            sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040031' AS MSGID, P.PACKING_NO, P.PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
            sb.ApdL("FROM T_PACKING_SHUKKAMEISAI AS P ");
            sb.ApdL("  WHERE 1 = 1 ");
            sb.ApdL("        AND P.SAGYO_FLAG IS NOT NULL ");
            sb.ApdN("        AND P.SAGYO_FLAG IN (").ApdN(this.BindPrefix).ApdN("KIWAKUMEISAI_VALUE1").ApdN(", ").ApdN(this.BindPrefix).ApdN("KONPOTOROKU_VALUE1").ApdL(")");

            if (IsKokugai)
            {
                // {0}行目の梱包No.{1}-技連No.{3}の有償/無償状態が異なります。手配明細の有償/無償状態を確認してください。
                sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040022' AS MSGID, P.PACKING_NO, P.PACK, NULL AS TAG_NO, T.ECS_NO, T.TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN M_NONYUSAKI AS N ");
                sb.ApdL("    ON  N.NONYUSAKI_CD = P.NONYUSAKI_CD ");
                sb.ApdL("    AND N.SHUKKA_FLAG = P.SHUKKA_FLAG ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  WHERE T.ESTIMATE_FLAG <> N.ESTIMATE_FLAG ");
                sb.ApdL("    AND N.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_1");
                // {0}行目の梱包No.{1}-技連No.{3}の有償/無償状態が異なります。手配明細の有償/無償状態を確認してください。
                sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040023' AS MSGID, P.PACKING_NO, P.PACK, NULL AS TAG_NO, T.ECS_NO, T.TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN M_NONYUSAKI AS N ");
                sb.ApdL("    ON  N.NONYUSAKI_CD = P.NONYUSAKI_CD ");
                sb.ApdL("    AND N.SHUKKA_FLAG = P.SHUKKA_FLAG ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  WHERE T.ESTIMATE_FLAG <> N.ESTIMATE_FLAG ");
                sb.ApdL("    AND N.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_0");
                // 手配連携No.{4}のMakerを入力してください。
                sb.ApdL("UNION ALL SELECT DISTINCT 'S0200040024' AS MSGID, P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, T.ECS_NO, T.TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  WHERE T.MAKER IS NULL ");
                // 手配連携No.{4}のFree2(原産国)を入力してください。
                sb.ApdL("UNION ALL SELECT  DISTINCT 'S0200040025' AS MSGID, P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, T.ECS_NO, T.TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  WHERE T.FREE2 IS NULL ");
                // 手配連携No.{4}の有償/無償を確定してください。
                sb.ApdL("UNION ALL SELECT  DISTINCT 'S0200040026' AS MSGID, P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, T.ECS_NO, T.TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  WHERE T.ESTIMATE_FLAG NOT IN (").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_0");
                sb.ApdL(" ,").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_1"); ;
                sb.ApdL(" )");
                // 見積No.{5}の手配明細見積りを参照できません。
                sb.ApdL("UNION ALL SELECT  DISTINCT 'S0200040027' AS MSGID, P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, T.ECS_NO, NULL AS TEHAI_RENKEI_NO, E.ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  LEFT OUTER JOIN T_TEHAI_ESTIMATE AS E ");
                sb.ApdL("    ON T.ESTIMATE_NO = E.ESTIMATE_NO ");
                sb.ApdL("  WHERE T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_1");
                sb.ApdL("    AND E.ESTIMATE_NO IS NULL ");
                // 見積No.{5}の受注を行ってください。 
                sb.ApdL("UNION ALL SELECT  DISTINCT 'S0200040028' AS MSGID, P.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, T.ECS_NO, NULL AS TEHAI_RENKEI_NO, E.ESTIMATE_NO ");
                sb.ApdL("  FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("    ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("  INNER JOIN T_TEHAI_ESTIMATE AS E ");
                sb.ApdL("    ON T.ESTIMATE_NO = E.ESTIMATE_NO ");
                sb.ApdL("  WHERE T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_1");
                sb.ApdL("    AND E.PO_NO IS NULL ");
                //1つのINVOICE内に複数の通貨(見積)が存在するため発行できません。
                sb.ApdL("UNION ALL SELECT  'S0200040029' AS MSGID, MULTI.PACKING_NO, NULL AS PACK, NULL AS TAG_NO, NULL AS ECS_NO, NULL AS TEHAI_RENKEI_NO, NULL AS ESTIMATE_NO ");
                sb.ApdL("  FROM ( ");
                sb.ApdL("    SELECT P.PACKING_NO, E.CURRENCY_FLAG ");
                sb.ApdL("      FROM T_PACKING_SHUKKAMEISAI AS P ");
                sb.ApdL("    INNER JOIN T_TEHAI_MEISAI AS T ");
                sb.ApdL("      ON T.TEHAI_RENKEI_NO = P.TEHAI_RENKEI_NO ");
                sb.ApdL("    INNER JOIN T_TEHAI_ESTIMATE AS E ");
                sb.ApdL("      ON T.ESTIMATE_NO = E.ESTIMATE_NO ");
                sb.ApdL("    WHERE T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_1");
                sb.ApdL("      AND E.CURRENCY_FLAG IS NOT NULL ");
                sb.ApdL("    GROUP BY P.PACKING_NO, E.CURRENCY_FLAG ");
                sb.ApdL("  ) AS MULTI ");
                sb.ApdL("  GROUP BY MULTI.PACKING_NO ");
                sb.ApdL("  HAVING COUNT(MULTI.CURRENCY_FLAG) > 1 ");
            }
            sb.ApdL(") AS d ");
            sb.ApdL("WHERE d.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            paramCollection.Add(iNewParam.NewDbParameter("SHUKAZUMI_VALUE1", JYOTAI_FLAG.SHUKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIREZUMI_VALUE1", JYOTAI_FLAG.UKEIREZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_0", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_1", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KIWAKUMEISAI_VALUE1", SAGYO_FLAG.KIWAKUMEISAI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KONPOTOROKU_VALUE1", SAGYO_FLAG.KONPOTOROKU_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("HIKIWATASHIZUMI_VALUE1", DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, "CHECK_TABLE");

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockNisugataExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  PACKING_NO ");
            sb.ApdL("  , CT_QTY ");
            sb.ApdL("  , INVOICE_NO ");
            sb.ApdL("  , SYUKKA_DATE ");
            sb.ApdL("  , HAKKO_FLAG ");
            sb.ApdL("  , UNSOKAISHA_CD ");
            sb.ApdL("  , CONSIGN_CD ");
            sb.ApdL("  , CONSIGN_ATTN ");
            sb.ApdL("  , DELIVER_CD ");
            sb.ApdL("  , DELIVER_ATTN ");
            sb.ApdL("  , PACKING_MAIL_SUBJECT ");
            sb.ApdL("  , PACKING_REV ");
            sb.ApdL("  , INTERNAL_PO_NO ");
            sb.ApdL("  , TRADE_TERMS_FLAG ");
            sb.ApdL("  , TRADE_TERMS_ATTR ");
            sb.ApdL("  , SUBJECT ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_PACKING ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("  PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_PACKING.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿明細テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿明細テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockNisugataMeisaiExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  PACKING_NO ");
            sb.ApdL("  , NO ");
            sb.ApdL("  , CT_NO ");
            sb.ApdL("  , CANCEL_FLAG ");
            sb.ApdL("  , SHUKKA_FLAG ");
            sb.ApdL("  , NONYUSAKI_CD ");
            sb.ApdL("  , AR_NO ");
            sb.ApdL("  , ECS_NO ");
            sb.ApdL("  , SEIBAN_CODE ");
            sb.ApdL("  , DOUKON_FLAG ");
            sb.ApdL("  , FORM_STYLE_FLAG ");
            sb.ApdL("  , SIZE_L ");
            sb.ApdL("  , SIZE_W ");
            sb.ApdL("  , SIZE_H ");
            sb.ApdL("  , GRWT ");
            sb.ApdL("  , PRODUCT_NAME ");
            sb.ApdL("  , ATTN ");
            sb.ApdL("  , NOTE ");
            sb.ApdL("  , PL_TYPE ");
            sb.ApdL("  , CASE_NO ");
            sb.ApdL("  , PALLET_NO ");
            sb.ApdL("  , BOX_NO ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_PACKING_MEISAI ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("  PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_PACKING_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="caseId">CASE_ID、工事識別Noを設定する場合必須</param>
    /// <remarks>
    /// caseIdをコンディションに追加したいが、Webサービスのリリースのみであるため、引数で対応する。
    /// </remarks>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// --------------------------------------------------
    private DataSet LockShukkaMeisaiExec(DatabaseHelper dbHelper, CondS02 cond, string caseId)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  SHUKKA_FLAG ");
            sb.ApdL("  , NONYUSAKI_CD ");
            sb.ApdL("  , TAG_NO ");
            sb.ApdL("  , AR_NO ");
            sb.ApdL("  , SEIBAN ");
            sb.ApdL("  , CODE ");
            sb.ApdL("  , ZUMEN_OIBAN ");
            sb.ApdL("  , AREA ");
            sb.ApdL("  , FLOOR ");
            sb.ApdL("  , KISHU ");
            sb.ApdL("  , ST_NO ");
            sb.ApdL("  , HINMEI_JP ");
            sb.ApdL("  , HINMEI ");
            sb.ApdL("  , ZUMEN_KEISHIKI ");
            sb.ApdL("  , KUWARI_NO ");
            sb.ApdL("  , NUM ");
            sb.ApdL("  , JYOTAI_FLAG ");
            sb.ApdL("  , TAGHAKKO_FLAG ");
            sb.ApdL("  , TAGHAKKO_DATE ");
            sb.ApdL("  , SHUKA_DATE ");
            sb.ApdL("  , BOX_NO ");
            sb.ApdL("  , BOXKONPO_DATE ");
            sb.ApdL("  , PALLET_NO ");
            sb.ApdL("  , PALLETKONPO_DATE ");
            sb.ApdL("  , KOJI_NO ");
            sb.ApdL("  , CASE_ID ");
            sb.ApdL("  , KIWAKUKONPO_DATE ");
            sb.ApdL("  , SHUKKA_DATE ");
            sb.ApdL("  , SHUKKA_USER_ID ");
            sb.ApdL("  , SHUKKA_USER_NAME ");
            sb.ApdL("  , UNSOKAISHA_NAME ");
            sb.ApdL("  , INVOICE_NO ");
            sb.ApdL("  , OKURIJYO_NO ");
            sb.ApdL("  , BL_NO ");
            sb.ApdL("  , UKEIRE_DATE ");
            sb.ApdL("  , UKEIRE_USER_ID ");
            sb.ApdL("  , UKEIRE_USER_NAME ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , FREE1 ");
            sb.ApdL("  , FREE2 ");
            sb.ApdL("  , TAG_NONYUSAKI_CD ");
            sb.ApdL("  , BIKO ");
            sb.ApdL("  , M_NO ");
            sb.ApdL("  , GRWT ");
            sb.ApdL("  , TEHAI_RENKEI_NO ");
            sb.ApdL("  , FILE_NAME ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_SHUKKA_MEISAI ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("  SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            sb.ApdN("  and NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            // パレットNo
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                sb.ApdN("  and PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            }
            // ボックスNo
            if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                sb.ApdN("  and BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            }
            // 工事識別管理NO、ケースID
            if (!string.IsNullOrEmpty(cond.KojiNo) && !string.IsNullOrEmpty(caseId))
            {
                sb.ApdN("  and KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
                sb.ApdN("  and CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseId));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <remarks>
    /// ARの場合は、CASE_NOおよびAR_NOが必須です
    /// </remarks>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// --------------------------------------------------
    private DataSet LockKiwakuExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  KOJI_NO ");
            sb.ApdL("  , KOJI_NAME ");
            sb.ApdL("  , SHIP ");
            sb.ApdL("  , TOROKU_FLAG ");
            sb.ApdL("  , CASE_MARK_FILE ");
            sb.ApdL("  , DELIVERY_NO ");
            sb.ApdL("  , PORT_OF_DESTINATION ");
            sb.ApdL("  , AIR_BOAT ");
            sb.ApdL("  , DELIVERY_DATE ");
            sb.ApdL("  , DELIVERY_POINT ");
            sb.ApdL("  , FACTORY ");
            sb.ApdL("  , REMARKS ");
            sb.ApdL("  , SAGYO_FLAG ");
            sb.ApdL("  , SHUKKA_DATE ");
            sb.ApdL("  , SHUKKA_USER_ID ");
            sb.ApdL("  , SHUKKA_USER_NAME ");
            sb.ApdL("  , UNSOKAISHA_NAME ");
            sb.ApdL("  , INVOICE_NO ");
            sb.ApdL("  , OKURIJYO_NO ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , SHUKKA_FLAG ");
            sb.ApdL("  , NONYUSAKI_CD ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_KIWAKU ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("      SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("  AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                sb.ApdL("  AND EXISTS (SELECT 1");
                sb.ApdL("    FROM T_SHUKKA_MEISAI");
                sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI");
                sb.ApdN("       ON T_SHUKKA_MEISAI.KOJI_NO = T_KIWAKU_MEISAI.KOJI_NO");
                sb.ApdL("      AND T_SHUKKA_MEISAI.CASE_ID = T_KIWAKU_MEISAI.CASE_ID");
                sb.ApdL("   WHERE  T_KIWAKU.KOJI_NO = T_KIWAKU_MEISAI.KOJI_NO");
                sb.ApdN("      AND T_KIWAKU_MEISAI.CASE_NO = ").ApdN(this.BindPrefix).ApdL("CASE_NO");
                sb.ApdN("      AND T_SHUKKA_MEISAI.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                sb.ApdL("  )");

                paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", cond.CaseNo));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }

            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠明細テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockKiwakuMeisaiExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  KOJI_NO ");
            sb.ApdL("  , CASE_ID ");
            sb.ApdL("  , CASE_NO ");
            sb.ApdL("  , STYLE ");
            sb.ApdL("  , ITEM ");
            sb.ApdL("  , DESCRIPTION_1 ");
            sb.ApdL("  , DESCRIPTION_2 ");
            sb.ApdL("  , DIMENSION_L ");
            sb.ApdL("  , DIMENSION_W ");
            sb.ApdL("  , DIMENSION_H ");
            sb.ApdL("  , MMNET ");
            sb.ApdL("  , NET_W ");
            sb.ApdL("  , GROSS_W ");
            sb.ApdL("  , MOKUZAI_JYURYO ");
            sb.ApdL("  , PALLET_NO_1 ");
            sb.ApdL("  , PALLET_NO_2 ");
            sb.ApdL("  , PALLET_NO_3 ");
            sb.ApdL("  , PALLET_NO_4 ");
            sb.ApdL("  , PALLET_NO_5 ");
            sb.ApdL("  , PALLET_NO_6 ");
            sb.ApdL("  , PALLET_NO_7 ");
            sb.ApdL("  , PALLET_NO_8 ");
            sb.ApdL("  , PALLET_NO_9 ");
            sb.ApdL("  , PALLET_NO_10 ");
            sb.ApdL("  , SHUKKA_DATE ");
            sb.ApdL("  , SHUKKA_USER_ID ");
            sb.ApdL("  , SHUKKA_USER_NAME ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , PRINT_CASE_NO ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_KIWAKU_MEISAI ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("  KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            sb.ApdN("  and CASE_NO = ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", cond.CaseNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR情報テーブルロック

    /// --------------------------------------------------
    /// <summary>
    /// AR情報テーブルロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet LockARExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  NONYUSAKI_CD ");
            sb.ApdL("  , LIST_FLAG ");
            sb.ApdL("  , AR_NO ");
            sb.ApdL("  , JYOKYO_FLAG ");
            sb.ApdL("  , HASSEI_DATE ");
            sb.ApdL("  , RENRAKUSHA ");
            sb.ApdL("  , KISHU ");
            sb.ApdL("  , GOKI ");
            sb.ApdL("  , GENBA_TOTYAKUKIBOU_DATE ");
            sb.ApdL("  , HUGUAI ");
            sb.ApdL("  , TAISAKU ");
            sb.ApdL("  , BIKO ");
            sb.ApdL("  , GENCHI_TEHAISAKI ");
            sb.ApdL("  , GENCHI_SETTEINOKI_DATE ");
            sb.ApdL("  , GENCHI_SHUKKAYOTEI_DATE ");
            sb.ApdL("  , GENCHI_KOJYOSHUKKA_DATE ");
            sb.ApdL("  , SHUKKAHOHO ");
            sb.ApdL("  , JP_SETTEINOKI_DATE ");
            sb.ApdL("  , JP_SHUKKAYOTEI_DATE ");
            sb.ApdL("  , JP_KOJYOSHUKKA_DATE ");
            sb.ApdL("  , JP_UNSOKAISHA_NAME ");
            sb.ApdL("  , JP_OKURIJYO_NO ");
            sb.ApdL("  , GMS_HAKKO_NO ");
            sb.ApdL("  , SHIYORENRAKU_NO ");
            sb.ApdL("  , TAIO_BUSHO ");
            sb.ApdL("  , GIREN_NO_1 ");
            sb.ApdL("  , GIREN_FILE_1 ");
            sb.ApdL("  , GIREN_NO_2 ");
            sb.ApdL("  , GIREN_FILE_2 ");
            sb.ApdL("  , GIREN_NO_3 ");
            sb.ApdL("  , GIREN_FILE_3 ");
            sb.ApdL("  , SHUKKA_DATE ");
            sb.ApdL("  , SHUKKA_USER_ID ");
            sb.ApdL("  , SHUKKA_USER_NAME ");
            sb.ApdL("  , UKEIRE_DATE ");
            sb.ApdL("  , UKEIRE_USER_ID ");
            sb.ApdL("  , UKEIRE_USER_NAME ");
            sb.ApdL("  , LOCK_USER_ID ");
            sb.ApdL("  , LOCK_STARTDATE ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , HASSEI_YOUIN ");
            sb.ApdL("  , REFERENCE_NO_1 ");
            sb.ApdL("  , REFERENCE_FILE_1 ");
            sb.ApdL("  , REFERENCE_NO_2 ");
            sb.ApdL("  , REFERENCE_FILE_2 ");
            sb.ApdL("FROM ");
            sb.ApdL("  T_AR ");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdN("  NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            sb.ApdN("  and AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細関連情報取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細関連情報取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(テーブル:ComDefine.TABLE_PACKKING_SHUKKAMEISAI)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/10/01 同梱フラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/10/01 EFA_SMS-138 有償版 Shipping Document の現地通貨略称表示対応</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    private DataTable GetPackingShukkameisaiExec(DatabaseHelper dbHelper, CondS02 cond, int mode)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.TABLE_PACKKING_SHUKKAMEISAI);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("--共通テーブル ");
            sb.ApdL("WITH T_PACKING_SHUKKAMEISAI (PACKING_NO, CT_NO, PACK, NONYUSAKI_CD, SHUKKA_FLAG, TAG_NO, JYOTAI_FLAG, TEHAI_RENKEI_NO, NUM) AS ");
            sb.ApdL("( ");
            sb.ApdL("  SELECT P.PACKING_NO, M.CT_NO, M.BOX_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.NUM ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("    AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("    AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("    AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, M.PALLET_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.NUM AS NET_WT ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("    AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("    AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("    AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.NUM ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("      AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("      AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.CT_NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.NUM ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("    INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("      ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("      AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("    INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("      ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("      AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("    INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("      ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("      AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("      AND S.AR_NO = M.AR_NO ");
            sb.ApdL(") ");

            if (mode == 0)
            {   // TOTAL(YEN)算出用
                sb.ApdL("SELECT ");
                sb.ApdL("        s.PACKING_NO");
                sb.ApdL("    ,   SUM(s.NUM * ");
                sb.ApdL("                    CASE ");
                sb.ApdN("                        WHEN ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN IsNull(t.INVOICE_UNIT_PRICE, 0)  ");
                sb.ApdN("                        WHEN ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + SGA_RATE) * IsNull(t.UNIT_PRICE, 0)) / 100)) / 100) ");
                sb.ApdL("                        ELSE 0");
                sb.ApdL("                    END ");
                sb.ApdL("                    ) AS PRICE");
                sb.ApdL("FROM T_PACKING_SHUKKAMEISAI AS s ");
                sb.ApdL("INNER JOIN T_TEHAI_MEISAI AS t ");
                sb.ApdL("ON t.TEHAI_RENKEI_NO = s.TEHAI_RENKEI_NO ");
                sb.ApdN("WHERE s.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
                sb.ApdL("GROUP BY s.PACKING_NO ");

                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));

            }
            else if (mode == 1)
            {   // PO#,PLC(CURRENCY_FLAG_NAME)
                sb.ApdL("SELECT DISTINCT ");
                sb.ApdL(" te.PO_NO ");
                sb.ApdL(",c1.ITEM_NAME AS " + ComDefine.FLD_CURRENCY_FLAG_NAME);
                sb.ApdL("FROM T_PACKING_SHUKKAMEISAI AS s ");
                sb.ApdL("INNER JOIN T_TEHAI_MEISAI AS t ");
                sb.ApdL("  ON t.TEHAI_RENKEI_NO = s.TEHAI_RENKEI_NO ");
                sb.ApdL("INNER JOIN T_TEHAI_ESTIMATE AS te ");
                sb.ApdL("  ON t.ESTIMATE_NO = te.ESTIMATE_NO ");
                sb.ApdL("LEFT JOIN M_COMMON AS c1 ");
                sb.ApdN("  ON  c1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("CURRENCY_FLAG_GROUP_CD");
                sb.ApdL("  AND c1.VALUE1 = te.CURRENCY_FLAG ");
                sb.ApdL("  AND c1.LANG = ").ApdN(this.BindPrefix).ApdL("CURRENCY_FLAG_LANG");
                sb.ApdL("WHERE s.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
                
                paramCollection.Add(iNewParam.NewDbParameter("CURRENCY_FLAG_GROUP_CD", CURRENCY_FLAG.GROUPCD));
                paramCollection.Add(iNewParam.NewDbParameter("CURRENCY_FLAG_LANG", cond.LoginInfo.Language));
            }
            else if (mode == 2)
            {   // COUNTRY OF ORIGIN
                sb.ApdL("SELECT DISTINCT ");
                sb.ApdL("t.FREE2 ");
                sb.ApdL("FROM T_PACKING_SHUKKAMEISAI AS s ");
                sb.ApdL("INNER JOIN T_TEHAI_MEISAI AS t ");
                sb.ApdL("ON t.TEHAI_RENKEI_NO = s.TEHAI_RENKEI_NO ");
                sb.ApdL("WHERE s.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            }   

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細(帳票用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細(帳票用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(T_SHUKKA_MEISAI)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/10/01 同梱フラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    private DataTable GetShukkameisaiTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT A.*, T.MAKER FROM ( ");
            sb.ApdL("  SELECT P.PACKING_NO, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("  AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("  AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("  AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("  AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    AND S.AR_NO = M.AR_NO ");
            sb.ApdL("    ) AS A ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_MEISAI AS T ");
            sb.ApdL("    ON A.TEHAI_RENKEI_NO = T.TEHAI_RENKEI_NO ");
            sb.ApdL("WHERE A.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細+手配明細(帳票用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細+手配明細(帳票用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(DTTBL_SIPPING_D)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-85 対応</update>
    /// <update>D.Okumura 2020/10/01 同梱フラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// <update>J.Chen 2022/12/19 TAG便名とInv付加名追加</update>
    /// --------------------------------------------------
    private DataTable GetSmeisaiTmeisaiTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_SIPPING_D);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("	 NM.SHIP ");
            sb.ApdL("	,CONVERT(DATETIME, A.SYUKKA_DATE) AS SYUKKA_DATE ");
            sb.ApdL("	,A.pi AS INVOICE_NO ");
            sb.ApdL("	,A.ATTN ");
            sb.ApdL("	,A.TAG_NO ");
            sb.ApdL("	,A.SEIBAN ");
            sb.ApdL("	,A.CODE ");
            sb.ApdL("	,A.ZUMEN_OIBAN ");
            sb.ApdL("	,NM.NONYUSAKI_NAME ");
            sb.ApdL("	,NM.SHIP ");
            sb.ApdL("	,A.AREA ");
            sb.ApdL("	,A.FLOOR ");
            sb.ApdL("	,A.KISHU ");
            sb.ApdL("	,A.ST_NO ");
            sb.ApdL("	,A.HINMEI_JP ");
            sb.ApdL("	,A.HINMEI ");
            sb.ApdL("	,A.ZUMEN_KEISHIKI ");
            sb.ApdL("	,A.KUWARI_NO ");
            sb.ApdL("	,A.NUM ");
            sb.ApdL("	,A.BOX_NO ");
            sb.ApdL("	,A.PALLET_NO ");
//            sb.ApdL("	,(T.INVOICE_UNIT_PRICE * A.NUM) AS PRICE ");
            sb.ApdL("    ,( A.NUM * CASE ");
            sb.ApdN("                    WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN IsNull(T.INVOICE_UNIT_PRICE, 0)  ");
            sb.ApdN("                    WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + T.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + T.SGA_RATE) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("                    ELSE 0");
            sb.ApdL("                END");
            sb.ApdL("                ) AS PRICE");

            sb.ApdL("	,A.TAG_SHIP ");
            sb.ApdL("	,T.HINMEI_INV ");

            sb.ApdL("FROM ( ");
            sb.ApdL("  SELECT P.PACKING_NO, P.SYUKKA_DATE ,P.INVOICE_NO AS pi, M.ATTN, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("  AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("  AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, P.SYUKKA_DATE ,P.INVOICE_NO AS pi, M.ATTN, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("  AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("  AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, P.SYUKKA_DATE ,P.INVOICE_NO AS pi, M.ATTN, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, P.SYUKKA_DATE ,P.INVOICE_NO AS pi, M.ATTN, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    AND S.AR_NO = M.AR_NO ");
            sb.ApdL("    ) AS A ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_MEISAI AS T ");
            sb.ApdL("    ON A.TEHAI_RENKEI_NO = T.TEHAI_RENKEI_NO ");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI NM ON NM.NONYUSAKI_CD = A.NONYUSAKI_CD AND NM.SHUKKA_FLAG = A.SHUKKA_FLAG ");
            sb.ApdL("WHERE A.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿明細+納入先マスタ(帳票用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿明細+納入先マスタ(帳票用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(DTTBL_SIPPING_E)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>M.Shimizu 2020/05/28 EFA_SMS-80 対応（荷姿表のサイズをcmとして扱う）</update>
    /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
    /// <update>D.Okumura 2020/10/01 同梱フラグの考慮漏れを修正</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// <update>J.Chen 2022/12/21 TAG便名追加、共通WITH文追加</update>
    /// <update>J.Chen 2023/01/04 INV付加名追加</update>
    /// --------------------------------------------------
    public DataTable GetNMeisaiNounyuTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_SIPPING_E);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 共通
            sb.ApdL("WITH TAG_SHIP_LIST AS ( ");
            sb.ApdL("	SELECT T.PACKING_NO, T.NO, SUM(IsNull(T.NET_WT, 0)) AS NET_WT, COUNT(T.TAG_NO) AS CNT, T.HINMEI_INV, T.TAG_SHIP ");
            sb.ApdL("	FROM ( ");
            sb.ApdL("		SELECT P.PACKING_NO, M.NO, M.BOX_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, TM.HINMEI_INV, S.TAG_SHIP ");
            sb.ApdL("		FROM T_PACKING AS P ");
            sb.ApdL("		INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("		ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("		AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("		AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("		INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("		ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("		AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("		AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("		AND S.PALLET_NO IS NULL ");
            sb.ApdL("		LEFT JOIN T_TEHAI_MEISAI AS TM ");
            sb.ApdL("		ON S.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO ");
            sb.ApdL("		UNION ALL SELECT P.PACKING_NO, M.NO, M.PALLET_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, TM.HINMEI_INV, S.TAG_SHIP ");
            sb.ApdL("		FROM T_PACKING AS P ");
            sb.ApdL("		INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("		ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("		AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("		AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("		INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("		ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("		AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("		AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("		AND S.KOJI_NO IS NULL ");
            sb.ApdL("		LEFT JOIN T_TEHAI_MEISAI AS TM ");
            sb.ApdL("		ON S.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("		UNION ALL SELECT P.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, TM.HINMEI_INV, S.TAG_SHIP ");
            sb.ApdL("		FROM T_PACKING AS P ");
            sb.ApdL("		INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("		ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("		AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("       AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("		INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("		ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("		AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("		INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("		ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("		AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("		INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("		ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("		AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("		LEFT JOIN T_TEHAI_MEISAI AS TM ");
            sb.ApdL("		ON S.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("		UNION ALL SELECT P.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT, TM.HINMEI_INV, S.TAG_SHIP ");
            sb.ApdL("		FROM T_PACKING AS P ");
            sb.ApdL("		INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("		ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("		AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("       AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("		INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("		ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("		AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("		INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("		ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("		AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("		INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("		ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("		AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("       AND S.AR_NO = M.AR_NO ");
            sb.ApdL("		LEFT JOIN T_TEHAI_MEISAI AS TM ");
            sb.ApdL("		ON S.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO ");
            sb.ApdL("	) AS T ");
            sb.ApdL("   WHERE T.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            sb.ApdL("	GROUP BY T.PACKING_NO, T.NO, T.TAG_SHIP, T.HINMEI_INV ");
            sb.ApdL("	) ");

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("	 NM.NONYUSAKI_NAME ");
            sb.ApdL("	,NM.SHIP ");
            sb.ApdL("	,NM.ESTIMATE_FLAG ");
            sb.ApdL("	,COM1.ITEM_NAME AS DISP_ESTIMATE_FLAG ");
            sb.ApdL("	,NM.TRANSPORT_FLAG ");
            sb.ApdL("	,NM.SHIP_FROM ");
            sb.ApdL("	,NM.SHIP_TO ");
            sb.ApdL("	,CONVERT(DATETIME, NM.SHIP_DATE) AS SHIP_DATE ");
            sb.ApdL("	,PMT.CT_NO ");
            sb.ApdL("	,PMT.PL_TYPE ");
            sb.ApdL("	,COM2.ITEM_NAME AS DISP_PL_TYPE ");
            sb.ApdL("	,CAST(PMT.SIZE_L AS VARCHAR(9)) + 'x'+CAST(PMT.SIZE_W AS VARCHAR(9)) + 'x'+CAST(PMT.SIZE_H AS VARCHAR(9)) AS SUNPO ");

            // ↓↓↓ M.Shimizu 2020/05/28 EFA_SMS-80 対応（荷姿表のサイズをcmとして扱う）
            // mm から cm への変換は不要なので削除
            // sb.ApdL("	,CAST(CAST(PMT.SIZE_L/10 AS decimal(9,1)) AS VARCHAR(9)) + 'x'+CAST(CAST(PMT.SIZE_W/10 AS decimal(9,1)) AS VARCHAR(9)) + 'x'+CAST(CAST(PMT.SIZE_H/10  AS decimal(9,1)) AS VARCHAR(9)) AS SUNPO_10 ");
            // ↑↑↑ M.Shimizu 2020/05/28 EFA_SMS-80 対応（荷姿表のサイズをcmとして扱う）

            sb.ApdL("	,PMT.GRWT ");
            sb.ApdL("	,PMT.SIZE_L ");
            sb.ApdL("	,PMT.SIZE_W ");
            sb.ApdL("	,PMT.SIZE_H ");

            // ↓↓↓ M.Shimizu 2020/05/28 EFA_SMS-80 対応（荷姿表のサイズをcmとして扱う）
            // sb.ApdL("	,ROUND(PMT.SIZE_L/1000*PMT.SIZE_W/1000*PMT.SIZE_H/1000,3) AS M3 ");
            sb.ApdL("	,ROUND(PMT.SIZE_L/100*PMT.SIZE_W/100*PMT.SIZE_H/100, 3) AS M3 ");
            // ↑↑↑ M.Shimizu 2020/05/28 EFA_SMS-80 対応（荷姿表のサイズをcmとして扱う）

            sb.ApdL("	,UM.UNSOKAISHA_NAME AS OTUNAKA ");
            sb.ApdL("	,CAST(CEILING(SM.NET_WT*100)/100 AS decimal(8,2)) AS NW ");
            sb.ApdL("	,CAST(CEILING(PMT.GRWT*100)/100 AS decimal(8,2)) AS GW ");
            sb.ApdL("	,SM.HINMEI_INV ");
            sb.ApdL("	,SM.TAG_SHIP ");
            sb.ApdL("FROM ");
            sb.ApdL("	T_PACKING_MEISAI AS PMT ");
            sb.ApdL("	LEFT JOIN T_PACKING PT ON PT.PACKING_NO = PMT.PACKING_NO ");
            sb.ApdL("	LEFT JOIN M_NONYUSAKI NM ON NM.NONYUSAKI_CD = PMT.NONYUSAKI_CD AND NM.SHUKKA_FLAG = PMT.SHUKKA_FLAG ");
            sb.ApdL("	LEFT JOIN M_UNSOKAISHA UM ON UM.UNSOKAISHA_NO = PT.UNSOKAISHA_CD ");
            sb.ApdL("   LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ESTIMATE_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = NM.ESTIMATE_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("   LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'PL_TYPE'");
            sb.ApdL("                         AND COM2.VALUE1 = PMT.PL_TYPE");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("	LEFT JOIN ( ");
            sb.ApdL("		SELECT T.PACKING_NO, T.NO, SUM(IsNull(T.NET_WT, 0)) AS NET_WT, COUNT(T.TAG_NO) AS CNT, STUFF((SELECT  ',' + HINMEI_INV FROM TAG_SHIP_LIST AS S WHERE HINMEI_INV IS NOT NULL AND HINMEI_INV <> '' AND T.NO = S.NO FOR xml path('')), 1, 1, '' ) AS HINMEI_INV, STUFF((SELECT  ',' + TAG_SHIP FROM TAG_SHIP_LIST AS S WHERE TAG_SHIP IS NOT NULL AND TAG_SHIP <> '' AND T.NO = S.NO FOR xml path('')), 1, 1, '' ) AS TAG_SHIP ");
            sb.ApdL("		FROM ( ");
            sb.ApdL("			SELECT P.PACKING_NO, M.NO, M.BOX_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT ");
            sb.ApdL("			FROM T_PACKING AS P ");
            sb.ApdL("			INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("			ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("			AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("			AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("			INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("			ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("			AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("			AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("			AND S.PALLET_NO IS NULL ");
            sb.ApdL("			UNION ALL SELECT P.PACKING_NO, M.NO, M.PALLET_NO AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT ");
            sb.ApdL("			FROM T_PACKING AS P ");
            sb.ApdL("			INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("			ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("			AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("			AND M.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("			INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("			ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("			AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("			AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("			AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("			UNION ALL SELECT P.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT ");
            sb.ApdL("			FROM T_PACKING AS P ");
            sb.ApdL("			INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("			ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("			AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("           AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("			INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("			ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("			AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("			INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("			ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("			AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("			INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("			ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("			AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("			UNION ALL SELECT P.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.NONYUSAKI_CD, S.SHUKKA_FLAG, S.TAG_NO, S.JYOTAI_FLAG, S.TEHAI_RENKEI_NO, S.GRWT AS NET_WT ");
            sb.ApdL("			FROM T_PACKING AS P ");
            sb.ApdL("			INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("			ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("			AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("           AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("			INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("			ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("			AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("			INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("			ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("			AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("			INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("			ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("			AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("           AND S.AR_NO = M.AR_NO ");
            sb.ApdL("		) AS T ");
            sb.ApdL("		GROUP BY T.PACKING_NO, T.NO ");
            sb.ApdL("	) AS SM ON SM.PACKING_NO = PMT.PACKING_NO AND SM.NO = PMT.NO ");
            sb.ApdL("WHERE ");
            sb.ApdL("    PMT.DOUKON_FLAG <> ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdL("AND PT.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            sb.ApdN("AND PMT.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("ORDER BY ");
            sb.ApdL("	 PMT.CT_NO ");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細+α(帳票用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細+α(帳票用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(DTTBL_SIPPING_F)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2019/01/29 キャンセルフラグの考慮漏れを修正</update>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-85 対応</update>
    /// <update>M.Shimizu 2020/08/25 ShippingDocumentのExcel改修</update>
    /// <update>T.Nukaga 2020/09/24 複数AR_NO時を考慮してデータ取得元変更(T_PACKING_MEISAI→T_SHUKKA_MEISAI)</update>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// <update>H.Iimuro 2022/10/07 ShippingDocumentのExcel改修</update>
    /// <update>J.Chen 2022/10/13 EFA_SMS-269 単価出力対応</update>
    /// --------------------------------------------------
    private DataTable GetShukkameisaiplusTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_SIPPING_F);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("	  ROW_NUMBER() OVER(ORDER BY A.TAG_NO) list_no");
            sb.ApdL("	 ,(T.HINMEI+' '+ISNULL(' '+T.HINMEI_INV,'')) AS list_description");
            sb.ApdL("	 ,T.ZUMEN_KEISHIKI AS list_parts");
            sb.ApdL("	 ,T.ZUMEN_KEISHIKI2 AS list_parts2");
            sb.ApdL("	 ,A.AR_NO AS list_ar_no");
            sb.ApdL("	 ,A.CT_NO AS list_case_no");
            sb.ApdL("	 ,A.GRWT AS list_net_weight");
            sb.ApdL("	 ,A.FREE2 AS list_made_in");
            sb.ApdL("	 ,A.NUM AS list_qty");
            sb.ApdL("	 ,CASE ");
            sb.ApdL("	    WHEN MC3.ITEM_NAME = 'pc' AND A.NUM > 1 THEN 'pcs' ");
            sb.ApdL("	    WHEN MC3.ITEM_NAME = 'set' AND A.NUM > 1 THEN 'sets' ");
            sb.ApdL("	    ELSE MC3.ITEM_NAME ");
            sb.ApdL("	 END AS list_qty_name");
            sb.ApdL("	 ,TTE.PO_NO AS list_po_no");
            sb.ApdL("    ,CASE ");
            sb.ApdN("         WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN IsNull(T.INVOICE_UNIT_PRICE, 0)  ");
            //sb.ApdN("         WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + T.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + T.SGA_RATE) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdN("         WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + IsNull(T.SHIPPING_RATE, 0)) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + IsNull(T.SGA_RATE, 15)) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("         ELSE 0");
            sb.ApdL("     END AS list_unit_price");
            sb.ApdL("    ,(A.NUM * CASE ");
            sb.ApdN("                   WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN IsNull(T.INVOICE_UNIT_PRICE, 0)  ");
            //sb.ApdN("                   WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + T.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + T.SGA_RATE) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdN("                   WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + IsNull(T.SHIPPING_RATE, 0)) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + IsNull(T.SGA_RATE, 15)) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("                   ELSE 0");
            sb.ApdL("              END");
            sb.ApdL("     ) AS list_total");
            sb.ApdL("	 ,A.TAG_NO");
            sb.ApdL("FROM ( ");
            sb.ApdL("  SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("  AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("  AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    AND S.AR_NO = M.AR_NO ");
            
            sb.ApdL("    ) AS A ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_MEISAI AS T ");
            sb.ApdL("    ON A.TEHAI_RENKEI_NO = T.TEHAI_RENKEI_NO ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_ESTIMATE AS TTE ");
            sb.ApdL("    ON T.ESTIMATE_NO = TTE.ESTIMATE_NO ");
            sb.ApdL("  LEFT OUTER JOIN M_NONYUSAKI NM ON NM.NONYUSAKI_CD = A.NONYUSAKI_CD AND NM.SHUKKA_FLAG = A.SHUKKA_FLAG ");
            sb.ApdN("  LEFT JOIN M_COMMON MC3 ON MC3.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("QUANTITY_UNIT_GROUP_CD")
                                     .ApdN(" AND MC3.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC3.VALUE1 = T.QUANTITY_UNIT");
            sb.ApdL("WHERE A.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            // 木枠の場合は同梱フラグを見ない(ARの場合抽出条件であるため)
            sb.ApdN("  AND (A.KOJI_NO IS NOT NULL OR A.DOUKON_FLAG = ").ApdN(this.BindPrefix).ApdN("DOUKON_FLAG").ApdL(")");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.OFF_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("QUANTITY_UNIT_GROUP_CD", QUANTITY_UNIT.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細(PoNo用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細(PoNo用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <returns>表示データ(DTTBL_SIPPING_I)</returns>
    /// <create>H.Kawasaki 2021/01/15 EFA_SMS-184 複数PO No対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetShukkameisaiPoNoExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_SIPPING_I);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("	 TTE.PO_NO AS list_po_no");
            sb.ApdL("    ,SUM(A.NUM * CASE ");
            sb.ApdN("                   WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN IsNull(T.INVOICE_UNIT_PRICE, 0)  ");
            sb.ApdN("                   WHEN T.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + T.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + T.SGA_RATE) * IsNull(T.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("                   ELSE 0");
            sb.ApdL("              END");
            sb.ApdL("     ) AS list_po_total");
            sb.ApdL("FROM ( ");
            sb.ApdL("  SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.BOX_NO = S.BOX_NO ");
            sb.ApdL("  AND S.PALLET_NO IS NULL ");
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("  ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("  AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("  ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD ");
            sb.ApdL("  AND M.SHUKKA_FLAG = S.SHUKKA_FLAG ");
            sb.ApdL("  AND M.PALLET_NO = S.PALLET_NO ");
            sb.ApdL("  AND S.KOJI_NO IS NULL ");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("  UNION ALL SELECT P.PACKING_NO, M.NO, M.AR_NO AS ar, M.CT_NO, M.DOUKON_FLAG, S.* ");
            sb.ApdL("  FROM T_PACKING AS P ");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI AS M ");
            sb.ApdL("    ON  P.PACKING_NO = M.PACKING_NO ");
            sb.ApdN("    AND M.CANCEL_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("    AND M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("  INNER JOIN T_KIWAKU AS K ");
            sb.ApdL("    ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD ");
            sb.ApdL("    AND K.SHUKKA_FLAG = M.SHUKKA_FLAG ");
            sb.ApdL("  INNER JOIN T_KIWAKU_MEISAI AS KM ");
            sb.ApdL("    ON  M.CASE_NO = KM.CASE_NO ");
            sb.ApdL("    AND KM.KOJI_NO = K.KOJI_NO ");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI AS S ");
            sb.ApdL("    ON  S.KOJI_NO = KM.KOJI_NO ");
            sb.ApdL("    AND S.CASE_ID = KM.CASE_ID ");
            sb.ApdL("    AND S.AR_NO = M.AR_NO ");
            
            sb.ApdL("    ) AS A ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_MEISAI AS T ");
            sb.ApdL("    ON A.TEHAI_RENKEI_NO = T.TEHAI_RENKEI_NO ");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_ESTIMATE AS TTE ");
            sb.ApdL("    ON T.ESTIMATE_NO = TTE.ESTIMATE_NO ");
            sb.ApdL("WHERE A.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            // 木枠の場合は同梱フラグを見ない(ARの場合抽出条件であるため)
            sb.ApdN("  AND (A.KOJI_NO IS NOT NULL OR A.DOUKON_FLAG = ").ApdN(this.BindPrefix).ApdN("DOUKON_FLAG").ApdL(")");
            sb.ApdL("GROUP BY TTE.PO_NO ");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", DOUKON_FLAG.OFF_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", CANCEL_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 運送会社マスタ(帳票用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社マスタ(帳票用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(M_UNSOKAISHA)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2021/01/15 EFA_SMS-184 DataTableを返却するように変更</update>
    /// --------------------------------------------------
    private DataTable GetUnsokaisyaTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_UNSOKAISHA.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("  UNSOKAISHA_NO ");
            sb.ApdL("  , KOKUNAI_GAI_FLAG ");
            sb.ApdL("  , UNSOKAISHA_NAME ");
            sb.ApdL("  , INVOICE_FLAG ");
            sb.ApdL("  , PACKINGLIST_FLAG ");
            sb.ApdL("  , EXPORTCONFIRM_FLAG ");
            sb.ApdL("  , EXPORTCONFIRM_ATTN ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("  M_UNSOKAISHA ");
            sb.ApdL("WHERE UNSOKAISHA_NO = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", cond.UnsokaishaNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細(一覧用)取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細(一覧用)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S02用コンディション</param>
    /// <param name="mode">取得切り替え</param>
    /// <returns>表示データ(DTTBL_SIPPING_H)</returns>
    /// <create>T.Nakata 2018/12/19</create>
    /// <update>D.Okumura 2020/11/11 EFA_SMS-163 木枠梱包でARを指定した場合の抽出条件を暫定修正</update>
    /// <update>J.Chen 2022/10/14 抽出データ追加</update>
    /// <update>J.Chen 2022/12/19 TAG便名追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    public DataTable GetShukkaListTyouhyouExec(DatabaseHelper dbHelper, CondS02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_SIPPING_H);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            const string CASE_WHEN_DISP_SHIP_ARNO = "CASE WHEN TSM.SHUKKA_FLAG = {0} THEN MNS.SHIP ELSE TSM.AR_NO END AS SHIP_AR_NO";
            sb.ApdL("SELECT");
            sb.ApdL("       MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS JYOTAI_NAME");
            sb.ApdN("     , ").ApdL(string.Format(CASE_WHEN_DISP_SHIP_ARNO, this.BindPrefix + "SHUKKA_FLAG_NORMAL"));
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.CASE_ID");
            sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL("     , TSM.UKEIRE_DATE");
            sb.ApdL("     , TSM.UKEIRE_USER_NAME");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.KIWAKU_NO");
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TSM.FILE_NAME");
            sb.ApdL("     , TMS.MAKER");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TMS.HINMEI_INV");
            sb.ApdL("     , TMS.NOTE");
            sb.ApdL("     , TMS.NOTE2");
            sb.ApdL("     , TMS.NOTE3");
            sb.ApdL("     , TMS.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.TAG_SHIP");

            sb.ApdL("  FROM");
            //sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("       (");
            sb.ApdL("           SELECT M.PACKING_NO, M.NO, M.BOX_NO AS PACK, S.*, NULL AS KIWAKU_NO");
            sb.ApdL("           FROM T_PACKING_MEISAI AS M");
            sb.ApdL("           INNER JOIN T_SHUKKA_MEISAI AS S");
            sb.ApdL("           ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD");
            sb.ApdL("           AND M.SHUKKA_FLAG = S.SHUKKA_FLAG");
            sb.ApdL("           AND M.BOX_NO = S.BOX_NO");
            sb.ApdL("           AND S.PALLET_NO IS NULL");
            sb.ApdL("           UNION ALL SELECT M.PACKING_NO, M.NO, M.PALLET_NO AS PACK, S.*, NULL AS KIWAKU_NO");
            sb.ApdL("           FROM T_PACKING_MEISAI AS M");
            sb.ApdL("           INNER JOIN T_SHUKKA_MEISAI AS S");
            sb.ApdL("           ON  M.NONYUSAKI_CD = S.NONYUSAKI_CD");
            sb.ApdL("           AND M.SHUKKA_FLAG = S.SHUKKA_FLAG");
            sb.ApdL("           AND M.PALLET_NO = S.PALLET_NO");
            sb.ApdL("           AND S.KOJI_NO IS NULL");
            // 本体のみの木枠情報に紐づく出荷明細
            sb.ApdL("           UNION ALL SELECT M.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.*, RTRIM(K.SHIP) + '-' + CAST(KM.CASE_NO AS VARCHAR) AS KIWAKU_NO");
            sb.ApdL("           FROM T_PACKING_MEISAI AS M");
            sb.ApdL("           INNER JOIN T_KIWAKU AS K");
            sb.ApdL("           ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD");
            sb.ApdL("           AND K.SHUKKA_FLAG = M.SHUKKA_FLAG");
            sb.ApdL("           INNER JOIN T_KIWAKU_MEISAI AS KM");
            sb.ApdL("           ON  M.CASE_NO = KM.CASE_NO");
            sb.ApdL("           AND KM.KOJI_NO = K.KOJI_NO");
            sb.ApdL("           INNER JOIN T_SHUKKA_MEISAI AS S");
            sb.ApdL("           ON  S.KOJI_NO = KM.KOJI_NO");
            sb.ApdL("           AND S.CASE_ID = KM.CASE_ID");
            sb.ApdN("           WHERE M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
            // ARのみの木枠情報に紐づく出荷明細
            // 注意: 1つのARが複数の木枠にまたがるとき、古い出荷情報も対象となってしまう
            sb.ApdL("           UNION ALL SELECT M.PACKING_NO, M.NO, CAST(M.CASE_NO AS nvarchar(6)) AS PACK, S.*, RTRIM(K.SHIP) + '-' + CAST(KM.CASE_NO AS VARCHAR) AS KIWAKU_NO");
            sb.ApdL("           FROM T_PACKING_MEISAI AS M");
            sb.ApdL("           INNER JOIN T_KIWAKU AS K");
            sb.ApdL("           ON  K.NONYUSAKI_CD = M.NONYUSAKI_CD");
            sb.ApdL("           AND K.SHUKKA_FLAG = M.SHUKKA_FLAG");
            sb.ApdL("           INNER JOIN T_KIWAKU_MEISAI AS KM");
            sb.ApdL("           ON  M.CASE_NO = KM.CASE_NO");
            sb.ApdL("           AND KM.KOJI_NO = K.KOJI_NO");
            sb.ApdL("           INNER JOIN T_SHUKKA_MEISAI AS S");
            sb.ApdL("           ON  S.KOJI_NO = KM.KOJI_NO");
            sb.ApdL("           AND S.CASE_ID = KM.CASE_ID");
            sb.ApdL("           AND S.AR_NO = M.AR_NO");
            sb.ApdN("           WHERE M.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdL("       ) AS TSM");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  INNER JOIN T_PACKING_MEISAI TP ON TSM.PACKING_NO = TP.PACKING_NO");
            sb.ApdL("                           AND TSM.NO = TP.NO");
            sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI TMS ON TSM.TEHAI_RENKEI_NO = TMS.TEHAI_RENKEI_NO");
            sb.ApdN("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("DISP_JYOTAI_FLAG");
            sb.ApdL("                        AND COM.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            //sb.ApdL("  LEFT JOIN T_KIWAKU TK ON TK.KOJI_NO = TSM.KOJI_NO");
            //sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.KOJI_NO = TSM.KOJI_NO");
            //sb.ApdL("                               AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            
            //Where
            sb.ApdN("  WHERE TSM.PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            sb.ApdN("    AND TP.CANCEL_FLAG = ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG_NORMAL");
            // 木枠の場合は同梱フラグを見ない(ARの場合抽出条件であるため)
            sb.ApdN("    AND (TSM.KOJI_NO IS NOT NULL OR TP.DOUKON_FLAG = ").ApdN(this.BindPrefix).ApdN("DOUKON_FLAG_OFF").ApdL(")");
            
            // ソート
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHIP_AR_NO");
            sb.ApdL("     , TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("DISP_JYOTAI_FLAG", DISP_JYOTAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG_NORMAL", CANCEL_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG_OFF", DOUKON_FLAG.OFF_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region UPDATE

    #region 荷姿テーブル更新

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿テーブル更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdNisugataExec(DatabaseHelper dbHelper, DataRow dr, CondS02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PACKING");
            sb.ApdL("SET");
            sb.ApdN("     HAKKO_FLAG = ").ApdN(this.BindPrefix).ApdL("HAKKO_FLAG");
            sb.ApdN("     , CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("     , CONSIGN_ATTN = ").ApdN(this.BindPrefix).ApdL("CONSIGN_ATTN");
            sb.ApdN("     , DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");
            sb.ApdN("     , DELIVER_ATTN = ").ApdN(this.BindPrefix).ApdL("DELIVER_ATTN");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , INTERNAL_PO_NO = ").ApdN(this.BindPrefix).ApdL("INTERNAL_PO_NO");
            sb.ApdN("     , TRADE_TERMS_FLAG = ").ApdN(this.BindPrefix).ApdL("TRADE_TERMS_FLAG");
            sb.ApdN("     , TRADE_TERMS_ATTR = ").ApdN(this.BindPrefix).ApdL("TRADE_TERMS_ATTR");
            sb.ApdN("     , SUBJECT = ").ApdN(this.BindPrefix).ApdL("SUBJECT");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", cond.PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("HAKKO_FLAG", cond.HakkouFlag));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("INTERNAL_PO_NO", ComFunc.GetFldObject(dr, Def_T_PACKING.INTERNAL_PO_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("TRADE_TERMS_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING.TRADE_TERMS_FLAG, TRADE_TERMS_FLAG.DEFAULT_VALUE1)));
            paramCollection.Add(iNewParam.NewDbParameter("TRADE_TERMS_ATTR", ComFunc.GetFldObject(dr, Def_T_PACKING.TRADE_TERMS_ATTR, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SUBJECT", ComFunc.GetFldObject(dr, Def_T_PACKING.SUBJECT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion
    
    #endregion

    #endregion

    #endregion

    #region String配列を指定セパレータ区切りの文字列に変換

    /// --------------------------------------------------
    /// <summary>
    /// String配列を指定セパレータ区切りの文字列に変換
    /// </summary>
    /// <param name="separator">セパレータ</param>
    /// <param name="array">String配列</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2015/12/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private string ConvArrayToString(string separator, string[] array)
    {
        var ret = string.Empty;
        if (array != null && array is string[])
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    ret += UtilConvert.PutQuot(array[i]);
                }
                else
                {
                    ret += separator + UtilConvert.PutQuot(array[i]);
                }
            }
        }
        return ret;
    }

    #endregion
}
