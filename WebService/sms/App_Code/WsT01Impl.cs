using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 手配明細処理 
/// </summary>
/// <create>S.Furugo 2018/10/26</create>
/// <update></update>
/// --------------------------------------------------
public class WsT01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>S.Furugo 2018/10/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsT01Impl()
        : base()
    {
    }

    #endregion
    
    #region T0100010:手配明細情報登録

    #region 制御

    #region 手配明細追加
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D. Naito 2018/11/26</create>
    /// <update>J.Chen 2022/10/31 手配No登録</update>
    /// <update>J.Chen 2024/11/06 手配明細履歴追加</update>
    /// --------------------------------------------------
    public bool InsTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            cond.UpdateDate = DateTime.Now;

            // 技連マスタの登録
            if (!this.InsGiren(dbHelper, cond, ds.Tables[Def_M_ECS.Name], ref errMsgID, ref args))
                return false;

            // 手配明細の登録
            if (!this.InsTehaiMeisai(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name], ref errMsgID, ref args))
                return false;

            // 無償有償レート設定
            foreach (DataRow dr in ds.Tables[Def_T_TEHAI_MEISAI.Name].Rows)
            {
                var yusho = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG).ToString();
                if (yusho == ESTIMATE_FLAG.ONEROUS_VALUE1)
                    this.UpdTehaiMeisaiYushoVersion(dbHelper, cond, dr);
                else if (yusho == ESTIMATE_FLAG.GRATIS_VALUE1)
                    this.UpdTehaiMeisaiMushoVersion(dbHelper, cond, dr);
                else if (yusho == ESTIMATE_FLAG.NEUTRAL_VALUE1)
                    this.UpdTehaiMeisaiMisetteiVersion(dbHelper, cond, dr);
            }

            DataTable dtIns = ds.Tables[Def_T_TEHAI_MEISAI.Name]; 
            DataTable dtSKS = new DataTable(Def_T_TEHAI_SKS.Name);
            DataRow drSKS;
            dtSKS.Columns.Add(Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, typeof(string));
            dtSKS.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
            dtSKS.Columns.Add(Def_T_TEHAI_SKS.KENPIN_UMU, typeof(string));
            dtSKS.Columns.Add(Def_T_TEHAI_SKS_WORK.CREATE_DATE, typeof(object));

            string[] tehaiNoArr = null;
            string tehaiRenkeiNo = "";

            foreach (DataRow drIns in dtIns.Rows)
            {
                tehaiRenkeiNo = ComFunc.GetFld(drIns, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                tehaiNoArr = ComFunc.GetFld(drIns, Def_T_TEHAI_SKS.TEHAI_NO).Split('+').ToArray();

                for (int i = 0; i < tehaiNoArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tehaiNoArr[i]))
                    {
                        cond.TehaiNo = tehaiNoArr[i];
                        DataTable dtTehaiSKSWorkVersion = this.GetTehaiSKSWorkVersionData(dbHelper, cond);

                        drSKS = dtSKS.NewRow();
                        drSKS[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = tehaiRenkeiNo;
                        drSKS[Def_T_TEHAI_SKS.TEHAI_NO] = tehaiNoArr[i];
                        // 検品あり
                        drSKS[Def_T_TEHAI_SKS.KENPIN_UMU] = KENPIN_UMU.ON_VALUE1;

                        if (dtTehaiSKSWorkVersion != null && dtTehaiSKSWorkVersion.Rows.Count > 0)
                        {
                            UtilData.SetFld(drSKS, Def_T_TEHAI_SKS_WORK.CREATE_DATE, ComFunc.GetFldObject(dtTehaiSKSWorkVersion.Rows[0], Def_T_TEHAI_SKS.CREATE_DATE));
                        }

                        dtSKS.Rows.Add(drSKS);
                    }
                }
            }

            if (dtSKS != null && dtSKS.Rows.Count > 0)
            {
                // 手配明細SKSに同じ手配Noデータが存在していればNG(他で既に連携済)
                string tmpStr = this.CheckExistTehaiMeisaiSKSForInsert(dbHelper, dtIns, cond);

                // 手配Noがあれば連携済のため使用不可
                if (!string.IsNullOrEmpty(tmpStr))
                {
                    // 手配No{0}は既に連携済です。
                    errMsgID = "T0100020033";
                    args = new string[] { tmpStr };
                    return false;
                }

                // SKS手配明細WORKの行ロック
                var dtChkTehaiSKSWork = this.LockSKSTehaiRenkeiTehaiSKSWork(dbHelper, dtSKS);

                // SKS手配明細WORKのバージョンチェック
                int[] notFoundIndexSKSIns = null;
                var indexSKSIns = this.CheckSameData(dtSKS, dtChkTehaiSKSWork, out notFoundIndexSKSIns, Def_T_TEHAI_SKS_WORK.CREATE_DATE, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                if (0 <= indexSKSIns)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                // 手配No存在しなくても登録可能に（コメントアウト）
                //else if (notFoundIndexSKSIns != null)
                //{
                //    // 他端末で更新された為、更新できませんでした。
                //    errMsgID = "A9999999027";
                //    return false;
                //}

                // SKS手配明細の登録
                this.InsSKSTehaiRenkeiTehaiSKS(dbHelper, cond, dtSKS);

                // 手配SKS連携の登録
                this.InsSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtIns, dtSKS);

                // 手配連携No取得
                var tehairenkeinoList = dtSKS.AsEnumerable().Select(row => row[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO].ToString()).Distinct().ToList<string>();
                // 単価(JPY)、手配種別更新
                foreach (string tehairenkeino in tehairenkeinoList)
                {
                    UpdTehaiMeisaiUnitPriceForAnother(dbHelper, tehairenkeino);
                }
            }

            // 手配明細履歴追加
            this.InsTehaiMeisaiRireki(dbHelper, cond, dtIns, PROCUREMENT_ENTRY_STATUS.ADDED_VALUE1);

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dr">データレコード</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D. Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsGiren(DatabaseHelper dbHelper, CondT01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            if (dt == null || dt.Rows.Count != 1) return false;
            this.InsGiren(dbHelper, cond, dt);

        }
        catch (Exception ex)
        {
            if (ex.InnerException.GetType() == typeof(System.Data.DuplicateNameException))
            {
                // 既に登録されています。
                errMsgID = "A9999999038";
                return false;
            }
            else
            {
                throw new Exception(ex.Message, ex);
            }
        }
        return true;

    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D. Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            string tehaiRenkei;
            using (WsSmsImpl impl = new WsSmsImpl())
            {
                CondSms condSms = new CondSms(cond.LoginInfo);
                condSms.SaibanFlag = SAIBAN_FLAG.TEHAI_RENKEI_VALUE1;
                condSms.LoginInfo = cond.LoginInfo;

                // 採番
                foreach (DataRow dr in dt.Rows)
                {
                    if (!impl.GetSaiban(dbHelper, condSms, out tehaiRenkei, out errMsgID))
                        return false;
                    dr[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = tehaiRenkei;
                }
                this.InsTehaiMeisai(dbHelper, cond, dt);
            }

        }
        catch (Exception ex)
        {
            if (ex.InnerException.GetType() == typeof(System.Data.DuplicateNameException))
            {
                // 既に登録されています。
                errMsgID = "A9999999038";
                return false;
            }
            else
            {
                throw new Exception(ex.Message, ex);
            }
        }
        return true;
    }

    #endregion

    #region 手配明細更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細変更
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/11/07</create>
    /// <update>D.Okumura 2019/11/27 メッセージIDの不備を修正(T0100010024,T0100010025)</update>
    /// <update>J.Chen 2022/10/31 手配No変更・削除処理追加</update>
    /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
    /// <update>J.Chen 2024/11/06 手配明細履歴追加</update>
    /// --------------------------------------------------
    public bool UpdTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック＆取得
            DataTable dtGirenCheck = this.LockEcs(dbHelper, cond);
            DataTable dtCheck = this.GetTehaiMeisai(dbHelper, cond, true);

            DataTable dtGiren = ds.Tables[Def_M_ECS.Name];
            DataTable dtUpd = ds.Tables[ComDefine.DTTBL_UPDATE];
            DataTable dtDel = ds.Tables[ComDefine.DTTBL_DELETE];
            DataTable dtIns = ds.Tables[ComDefine.DTTBL_INSERT];
            DataTable dtUpdSKS = ds.Tables["UPDSKS"];
            DataTable dtDelSKS = ds.Tables["DELSKS"];
            int index;

            // 技連マスタのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtGiren, dtGirenCheck, out notFoundIndex, Def_M_ECS.VERSION, Def_M_ECS.ECS_QUOTA, Def_M_ECS.ECS_NO);
            if (0 <= index || notFoundIndex != null)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            //技連マスタの更新
            this.UpdGiren(dbHelper, cond, dtGiren);

            // 更新
            if (dtUpd != null && dtUpd.Rows.Count > 0)
            {
                // 更新データのバージョンチェック
                index = this.CheckSameData(dtUpd, dtCheck, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                if (0 <= index || notFoundIndex != null)
                {
                    // 他端末で更新された為、更新出来ませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // 出荷数 < 出荷明細数量
                foreach (DataRow drUpd in dtUpd.Rows)
                {
                    foreach (DataRow drCheck in dtCheck.Rows)
                    {
                        if (ComFunc.GetFld(drUpd, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) == ComFunc.GetFld(drCheck, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                        {
                            // TAG連携済で、かつ更新内容が返却品対象であれば更新不可
                            if ((ComFunc.GetFldToDecimal(drCheck, ComDefine.FLD_SHUKKA_MEISAI_CNT, 0m) > 0m)
                                && (ComFunc.GetFld(drUpd, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG) != HENKYAKUHIN_FLAG.DEFAULT_VALUE1))
                            {
                                // TAG連携済み情報を返却品対象に設定しているため保存出来ません。
                                errMsgID = "T0100010026";
                                return false;
                            }
                            if (ComFunc.GetFldToDecimal(drUpd, Def_T_TEHAI_MEISAI.SHUKKA_QTY, 0m) < ComFunc.GetFldToDecimal(drUpd, ComDefine.FLD_SHUKKA_MEISAI_QTY, 0m))
                            {
                                // 出荷数を出荷明細数量以上の値で入力して下さい。
                                errMsgID = "T0100010024";
                                return false;
                            }
                        }
                    }
                }

                // 手配明細の更新
                if (this.UpdTehaiMeisai(dbHelper, cond, dtUpd) < 1)
                    return false;

                // 手配明細履歴追加
                this.InsTehaiMeisaiRireki(dbHelper, cond, dtUpd, PROCUREMENT_ENTRY_STATUS.UPDATED_VALUE1);

                // 無償有償レート設定
                foreach (DataRow dr in dtUpd.Rows)
                {
                    var yusho = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG).ToString();
                    if (yusho == ESTIMATE_FLAG.ONEROUS_VALUE1)
                        this.UpdTehaiMeisaiYushoVersion(dbHelper, cond, dr);
                    else if (yusho == ESTIMATE_FLAG.GRATIS_VALUE1)
                        this.UpdTehaiMeisaiMushoVersion(dbHelper, cond, dr);
                    else if (yusho == ESTIMATE_FLAG.NEUTRAL_VALUE1)
                        this.UpdTehaiMeisaiMisetteiVersion(dbHelper, cond, dr);
                }

                // 連携中の手配No削除
                if (dtDelSKS != null && dtDelSKS.Rows.Count > 0)
                {
                    // 手配SKS連携の行ロック
                    var dtTeahiMeisaiSKS = this.LockSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtDelSKS, dtDelSKS);

                    // 手配SKS連携削除
                    this.DelSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtDelSKS);
                }

                if (dtUpdSKS != null && dtUpdSKS.Rows.Count > 0)
                {
                    // SKS手配明細WORKの行ロック
                    var dtChkTehaiSKSWork = this.LockSKSTehaiRenkeiTehaiSKSWork(dbHelper, dtUpdSKS);

                    // SKS手配明細WORKのバージョンチェック
                    int[] notFoundIndexSKSUpd = null;
                    var indexSKSUpd = this.CheckSameData(dtUpdSKS, dtChkTehaiSKSWork, out notFoundIndexSKSUpd, Def_T_TEHAI_SKS_WORK.CREATE_DATE, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                    if (0 <= indexSKSUpd)
                    {
                        // 他端末で更新された為、更新できませんでした。
                        errMsgID = "A9999999027";
                        return false;
                    }
                    // 手配No存在しなくても登録可能に（コメントアウト）
                    //else if (notFoundIndexSKSUpd != null)
                    //{
                    //    // 他端末で更新された為、更新できませんでした。
                    //    errMsgID = "A9999999027";
                    //    return false;
                    //}

                    // SKS手配明細の登録
                    this.InsSKSTehaiRenkeiTehaiSKS(dbHelper, cond, dtUpdSKS);

                    // 手配SKS連携の登録
                    this.InsSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtUpd, dtUpdSKS);
                }

                // 手配連携No取得
                var list1 = dtUpdSKS == null ? null : dtUpdSKS.AsEnumerable().Select(row => row[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO].ToString()).Distinct().ToList<string>();
                var list2 = dtDelSKS == null ? null : dtDelSKS.AsEnumerable().Select(row => row[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO].ToString()).Distinct().ToList<string>();
                if (list1 != null || list2 != null)
                {
                    // 単価(JPY)、手配種別更新
                    var list3 = list1 != null && list2 != null ? list1.Union(list2) : null;
                    list3 = list3 != null ? list3 : list1;
                    list3 = list3 != null ? list3 : list2;

                    if (list3.Count() > 0)
                    {
                        foreach (string renkeiNo in list3)
                        {
                            UpdTehaiMeisaiUnitPriceForAnother(dbHelper, renkeiNo);
                        }
                    }
                }
            }

            // 削除
            if (dtDel != null && dtDel.Rows.Count > 0)
            {
                // 削除データのバージョンチェック
                index = this.CheckSameData(dtDel, dtCheck, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                if (0 <= index)
                {
                    // 他端末で更新された為、更新出来ませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // TAG連携済み
                foreach (DataRow drDel in dtDel.Rows)
                {
                    foreach (DataRow drCheck in dtCheck.Rows)
                    {
                        if (ComFunc.GetFld(drDel, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) == ComFunc.GetFld(drCheck, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                        {
                            if (ComFunc.GetFldToDecimal(drCheck, ComDefine.FLD_SHUKKA_MEISAI_CNT, 0m) > 0m)
                            {
                                // 削除対象にTAG連携済み情報を含んでいるため、保存出来ません。
                                errMsgID = "T0100010025";
                                return false;
                            }
                        }
                    }
                }
                // 削除対象の手配Noを取得
                DataTable sks = this.GetTehaiMeisaiSks(dbHelper, dtDel);
                // 手配SKS連携データを削除
                this.DelTehaiMeisaiSks(dbHelper, dtDel);
                // SKS手配明細データを削除(参照無しのみ)
                this.DelTehaiSksNotReferenced(dbHelper, sks);

                // 手配明細の削除
                this.DelTehaiMeisai(dbHelper, cond, dtDel);

                // 手配明細履歴追加
                this.InsTehaiMeisaiRireki(dbHelper, cond, dtDel, PROCUREMENT_ENTRY_STATUS.REMOVED_VALUE1);
            }

            // 手配明細の登録
            if (dtIns != null && dtIns.Rows.Count > 0)
            {
                if (!InsTehaiMeisai(dbHelper, cond, dtIns, ref errMsgID, ref args))
                    return false;

                // 無償有償レート設定
                foreach (DataRow dr in dtIns.Rows)
                {
                    var yusho = ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG).ToString();
                    if (yusho == ESTIMATE_FLAG.ONEROUS_VALUE1)
                        this.UpdTehaiMeisaiYushoVersion(dbHelper, cond, dr);
                    else if (yusho == ESTIMATE_FLAG.GRATIS_VALUE1)
                        this.UpdTehaiMeisaiMushoVersion(dbHelper, cond, dr);
                    else if (yusho == ESTIMATE_FLAG.NEUTRAL_VALUE1)
                        this.UpdTehaiMeisaiMisetteiVersion(dbHelper, cond, dr);
                }

                DataTable dtSKS = new DataTable(Def_T_TEHAI_SKS.Name);
                DataRow drSKS;
                dtSKS.Columns.Add(Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, typeof(string));
                dtSKS.Columns.Add(Def_T_TEHAI_SKS.TEHAI_NO, typeof(string));
                dtSKS.Columns.Add(Def_T_TEHAI_SKS.KENPIN_UMU, typeof(string));
                dtSKS.Columns.Add(Def_T_TEHAI_SKS_WORK.CREATE_DATE, typeof(object));

                string[] tehaiNoArr = null;
                string tehaiRenkeiNo = "";

                foreach (DataRow drIns in dtIns.Rows)
                {
                    tehaiRenkeiNo = ComFunc.GetFld(drIns, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                    tehaiNoArr = ComFunc.GetFld(drIns, Def_T_TEHAI_SKS.TEHAI_NO).Split('+').ToArray();

                    for (int i = 0; i < tehaiNoArr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tehaiNoArr[i]))
                        {
                            cond.TehaiNo = tehaiNoArr[i];
                            DataTable dtTehaiSKSWorkVersion = this.GetTehaiSKSWorkVersionData(dbHelper, cond);

                            drSKS = dtSKS.NewRow();
                            drSKS[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO] = tehaiRenkeiNo;
                            drSKS[Def_T_TEHAI_SKS.TEHAI_NO] = tehaiNoArr[i];
                            // 検品あり
                            drSKS[Def_T_TEHAI_SKS.KENPIN_UMU] = KENPIN_UMU.ON_VALUE1;

                            if (dtTehaiSKSWorkVersion != null && dtTehaiSKSWorkVersion.Rows.Count > 0)
                            {
                                UtilData.SetFld(drSKS, Def_T_TEHAI_SKS_WORK.CREATE_DATE, ComFunc.GetFldObject(dtTehaiSKSWorkVersion.Rows[0], Def_T_TEHAI_SKS.CREATE_DATE));
                            }

                            dtSKS.Rows.Add(drSKS);
                        }
                    }
                }

                if (dtSKS != null && dtSKS.Rows.Count > 0)
                {
                    // 手配明細SKSに同じ手配Noデータが存在していればNG(他で既に連携済)
                    string tmpStr = this.CheckExistTehaiMeisaiSKSForInsert(dbHelper, dtIns, cond);

                    // 手配Noがあれば連携済のため使用不可
                    if (!string.IsNullOrEmpty(tmpStr))
                    {
                        // 手配No{0}は既に連携済です。
                        errMsgID = "T0100020033";
                        args = new string[] { tmpStr };
                        return false;
                    }

                    // SKS手配明細WORKの行ロック
                    var dtChkTehaiSKSWork = this.LockSKSTehaiRenkeiTehaiSKSWork(dbHelper, dtSKS);

                    // SKS手配明細WORKのバージョンチェック
                    int[] notFoundIndexSKSIns = null;
                    var indexSKSIns = this.CheckSameData(dtSKS, dtChkTehaiSKSWork, out notFoundIndexSKSIns, Def_T_TEHAI_SKS_WORK.CREATE_DATE, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
                    if (0 <= indexSKSIns)
                    {
                        // 他端末で更新された為、更新できませんでした。
                        errMsgID = "A9999999027";
                        return false;
                    }
                    // 手配No存在しなくても登録可能に（コメントアウト）
                    //else if (notFoundIndexSKSIns != null)
                    //{
                    //    // 他端末で更新された為、更新できませんでした。
                    //    errMsgID = "A9999999027";
                    //    return false;
                    //}

                    // SKS手配明細の登録
                    this.InsSKSTehaiRenkeiTehaiSKS(dbHelper, cond, dtSKS);

                    // 手配SKS連携の登録
                    this.InsSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtIns, dtSKS);

                    // 手配連携No取得
                    var tehairenkeinoList = dtSKS.AsEnumerable().Select(row => row[Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO].ToString()).Distinct().ToList<string>();
                    // 単価(JPY)、手配種別更新
                    foreach (string tehairenkeino in tehairenkeinoList)
                    {
                        UpdTehaiMeisaiUnitPriceForAnother(dbHelper, tehairenkeino);
                    }
                }

                // 手配明細履歴追加
                this.InsTehaiMeisaiRireki(dbHelper, cond, dtIns, PROCUREMENT_ENTRY_STATUS.ADDED_VALUE1);

            }

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配明細削除
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Naito 2018/11/27</create>
    /// <update>D.Okumura 2019/11/27 メッセージIDの不備を修正(T0100010025)</update>
    /// <update>J.Chen 2024/11/06 手配明細履歴追加</update>
    /// --------------------------------------------------
    public bool DelTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック＆取得
            DataTable dtGirenCheck = this.LockEcs(dbHelper, cond);
            DataTable dtCheck = this.GetTehaiMeisai(dbHelper, cond, true);

            DataTable dtGiren = ds.Tables[Def_M_ECS.Name];
            DataTable dtDel = ds.Tables[Def_T_TEHAI_MEISAI.Name]; 
            int index;

            // 技連マスタのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtGiren, dtGirenCheck, out notFoundIndex, Def_M_ECS.VERSION, Def_M_ECS.ECS_QUOTA, Def_M_ECS.ECS_NO);
            if (0 <= index || notFoundIndex != null)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            // 技連マスタの削除
            this.DelGiren(dbHelper, cond);


            // 削除
            if (dtDel != null && dtDel.Rows.Count > 0)
            {
                // 削除データのバージョンチェック
                index = this.CheckSameData(dtDel, dtCheck, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                if (0 <= index)
                {
                    // 他端末で更新された為、更新出来ませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // TAG連携済み
                foreach (DataRow drDel in dtDel.Rows)
                {
                    foreach (DataRow drCheck in dtCheck.Rows)
                    {
                        if (ComFunc.GetFld(drDel, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) == ComFunc.GetFld(drCheck, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                        {
                            if (ComFunc.GetFldToDecimal(drCheck, ComDefine.FLD_SHUKKA_MEISAI_CNT, 0m) > 0m)
                            {
                                // 削除対象にTAG連携済み情報を含んでいるため、保存出来ません。
                                errMsgID = "T0100010025";
                                return false;
                            }
                        }
                    }
                }

                // 削除対象の手配Noを取得
                DataTable sks = this.GetTehaiMeisaiSks(dbHelper, dtDel);
                // 手配SKS連携データを削除
                this.DelTehaiMeisaiSks(dbHelper, dtDel);
                // SKS手配明細データを削除(参照無しのみ)
                this.DelTehaiSksNotReferenced(dbHelper, sks);

                // 手配明細の削除
                this.DelTehaiMeisai(dbHelper, cond);

                // 手配明細履歴追加
                this.InsTehaiMeisaiRireki(dbHelper, cond, dtDel, PROCUREMENT_ENTRY_STATUS.REMOVED_VALUE1);
            }

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 図番/型式画像ファイル管理データ取得(IN検索)

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式画像ファイル管理データ取得(IN検索)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">コンディション</param>
    /// <returns>データセット</returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetManageZumenKeishikiInSearch(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();

            // 図番/型式画像ファイル管理データ取得(IN検索)
            if (string.IsNullOrEmpty(cond.ZumenKeishiki) && string.IsNullOrEmpty(cond.ZumenKeishiki2))
            {
                DataTable dt = new DataTable(Def_T_MANAGE_ZUMEN_KEISHIKI.Name);
                ds.Merge(dt);
            }
            else
            {
                List<string> listZumenKeishiki = new List<string>();
                if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
                {
                    listZumenKeishiki.Add(cond.ZumenKeishiki);
                }
                if (!string.IsNullOrEmpty(cond.ZumenKeishiki2))
                {
                    listZumenKeishiki.Add(cond.ZumenKeishiki2);
                }
                ds.Merge(this.GetManageZumenKeishikiInSearch(dbHelper, listZumenKeishiki));
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 図番/型式の入力補助関連データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式の入力補助関連データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetZumenKeishikiInputAssistance(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            var ds = new DataSet();

            // 品名が入力されていない場合はパーツ名翻訳マスタ取得
            if (string.IsNullOrEmpty(cond.Hinmei))
            {
                ds.Merge(this.GetPartsName(dbHelper, cond));
            }

            // 図番/型式画像ファイル管理データ取得(IN検索)
            ds.Merge(this.GetManageZumenKeishikiInSearch(dbHelper, cond).Tables[Def_T_MANAGE_ZUMEN_KEISHIKI.Name]);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    //#region Assy単価計算データ取得

    ///// --------------------------------------------------
    ///// <summary>
    ///// Assy単価計算データ取得
    ///// </summary>
    ///// <param name="dbHelper">DatabaseHelper</param>
    ///// <param name="cond">T01用コンディション</param>
    ///// <returns></returns>
    ///// <create>Y.Shioshi 2022/05/12</create>
    ///// <update></update>
    ///// --------------------------------------------------
    //public DataSet GetAssyUnitPrice(DatabaseHelper dbHelper, CondT01 cond)
    //{
    //    try
    //    {
    //        var ds = new DataSet();

    //        // Assy単価計算データ取得
    //        ds.Merge(this.GetAssyUnitPriceData(dbHelper, cond));

    //        return ds;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message, ex);
    //    }
    //}

    //#endregion


    #region SKS手配明細WORKバージョン取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKバージョン取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2022/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiSKSWorkVersion(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            var ds = new DataSet();

            // SKS手配明細WORKバージョン取得
            ds.Merge(this.GetTehaiSKSWorkVersionData(dbHelper, cond));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion


    #endregion

    #region SQL実行

    #region SELECT

    #region 技連データ取得＆ロック
    /// --------------------------------------------------
    /// <summary>
    /// Ecsデータ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="CondT01">T01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>D.Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockEcs(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("SELECT ");
            sb.ApdL("      M_ECS.ECS_QUOTA");
            sb.ApdL("    , M_ECS.ECS_NO");
            sb.ApdL("    , M_ECS.PROJECT_NO");
            sb.ApdL("    , M_ECS.SEIBAN");
            sb.ApdL("    , M_ECS.CODE");
            sb.ApdL("    , M_ECS.KISHU");
            sb.ApdL("    , M_ECS.SEIBAN_CODE");
            sb.ApdL("    , M_ECS.AR_NO");
            sb.ApdL("    , M_ECS.KANRI_FLAG");
            sb.ApdL("    , M_ECS.CREATE_DATE");
            sb.ApdL("    , M_ECS.CREATE_USER_ID");
            sb.ApdL("    , M_ECS.CREATE_USER_NAME");
            sb.ApdL("    , M_ECS.UPDATE_DATE");
            sb.ApdL("    , M_ECS.UPDATE_USER_ID");
            sb.ApdL("    , M_ECS.UPDATE_USER_NAME");
            //sb.ApdL("    , M_ECS.MAINTE_DATE");
            //sb.ApdL("    , M_ECS.MAINTE_USER_ID");
            //sb.ApdL("    , M_ECS.MAINTE_USER_NAME");
            sb.ApdL("    , M_ECS.VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("    M_ECS");
            sb.ApdL("    WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN("WHERE ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("    AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");

            // バインド変数設定
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

            // SQL実行
            DataTable dt = new DataTable();
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_M_ECS.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配明細データ取得＆ロック
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="CondT01">T01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>D.Naito 2018/11/26</create>
    /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
    /// <update>T.Nukaga 2019/11/15 STEP12 返却品対応 返却品フラグ取得SQL追加</update>
    /// <update>J.Chen 2022/04/12 STEP14 登録日時取得追加</update>
    /// <update>J.Chen 2022/05/18 STEP14</update>
    /// <update>H.Iimuro 2022/10/19 STEP15 色取得追加</update>
    /// <update>J.Chen 2022/10/25 手配No追加</update>
    /// <update>J.Chen 2022/11/14 チェックボックス追加　順番統一</update>
    /// <update>J.Chen 2022/11/14 出荷済み数カウント追加</update>
    /// <update>J.Chen 2024/10/23 履歴更新列追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <remarks>
    /// <p>
    /// RowFillingイベントやRowUpdatingイベントなどで列インデックスを使用して値を取得する箇所がある。
    /// 列インデックス、シートの列順、表示用に取得した手配明細データは必ず順番を統一する事。
    /// </p>
    /// </remarks>
    /// --------------------------------------------------
    public DataTable GetTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, bool isLock)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("SELECT ");
            sb.ApdL("      '0' AS TEHAI_CHECK");
            sb.ApdL("    , T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("    , T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("    , T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("    , T_TEHAI_MEISAI.SETTEI_DATE");
            sb.ApdL("    , T_TEHAI_MEISAI.NOUHIN_SAKI");
            sb.ApdL("    , T_TEHAI_MEISAI.SYUKKA_SAKI");
            sb.ApdL("    , T_TEHAI_MEISAI.FLOOR");
            sb.ApdL("    , T_TEHAI_MEISAI.ST_NO");
            sb.ApdL("    , T_TEHAI_MEISAI.ZUMEN_OIBAN");
            sb.ApdL("    , T_TEHAI_MEISAI.HINMEI_JP");
            sb.ApdL("    , T_TEHAI_MEISAI.HINMEI");
            sb.ApdL("    , T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            sb.ApdL("    , T_TEHAI_MEISAI.ZUMEN_KEISHIKI_S");
            sb.ApdL("    , T_TEHAI_MEISAI.ZUMEN_KEISHIKI2");
            sb.ApdL("    , T_TEHAI_MEISAI.HINMEI_INV");
            sb.ApdL("    , T_TEHAI_MEISAI.TEHAI_QTY");
            sb.ApdL("    , T_TEHAI_MEISAI.QUANTITY_UNIT");
            sb.ApdL("    , T_TEHAI_MEISAI.TEHAI_FLAG");

            sb.ApdL("    , STUFF((");
            sb.ApdL("        SELECT");
            sb.ApdL("                '+' + T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("          FROM");
            sb.ApdL("                T_TEHAI_MEISAI_SKS");
            sb.ApdL("         WHERE");
            sb.ApdL("                T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("           FOR XML PATH ('')");
            sb.ApdL("       ),1,1,'')  AS TEHAI_NO");


            sb.ApdL("    , T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdL("    , T_TEHAI_MEISAI.HACCHU_QTY");
            sb.ApdL("    , T_TEHAI_MEISAI.SHUKKA_QTY");
            sb.ApdL("    , T_TEHAI_MEISAI.ASSY_NO");
            sb.ApdL("    , T_TEHAI_MEISAI.FREE1");
            sb.ApdL("    , T_TEHAI_MEISAI.FREE2");
            sb.ApdL("    , T_TEHAI_MEISAI.NOTE");
            sb.ApdL("    , T_TEHAI_MEISAI.NOTE2");
            sb.ApdL("    , T_TEHAI_MEISAI.NOTE3");
            sb.ApdL("    , T_TEHAI_MEISAI.CUSTOMS_STATUS");
            sb.ApdL("    , T_TEHAI_MEISAI.MAKER");
            sb.ApdL("    , T_TEHAI_MEISAI.UNIT_PRICE");
            if (!isLock)
            {
                sb.ApdN("    , (CASE WHEN TZK.ZUMEN_KEISHIKI IS NOT NULL THEN ").ApdN(this.BindPrefix).ApdN("EXISTS_VALUE").ApdL(" END) AS PHOTO");
            }
            sb.ApdL("    , T_TEHAI_MEISAI.CREATE_USER_NAME");
            sb.ApdL("    , T_TEHAI_MEISAI.CREATE_DATE");
            sb.ApdL("    , T_TEHAI_MEISAI.ARRIVAL_QTY");
            sb.ApdL("    , T_TEHAI_MEISAI.ASSY_QTY");
            sb.ApdL("    , T_TEHAI_MEISAI.ESTIMATE_FLAG");
            sb.ApdL("    , T_TEHAI_MEISAI.ESTIMATE_NO");
            //sb.ApdL("    , T_TEHAI_MEISAI.CREATE_DATE");
            //sb.ApdL("    , T_TEHAI_MEISAI.CREATE_USER_ID");
            //sb.ApdL("    , T_TEHAI_MEISAI.UPDATE_DATE");
            //sb.ApdL("    , T_TEHAI_MEISAI.UPDATE_USER_ID");
            //sb.ApdL("    , T_TEHAI_MEISAI.UPDATE_USER_NAME");
            sb.ApdL("    , CONVERT(NCHAR(27), T_TEHAI_MEISAI.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            //sb.ApdL("    , T_TEHAI_MEISAI.VERSION ");
            sb.ApdN("    , IsNull(s.SHUKKA_RENKEI, 0) AS ").ApdL(ComDefine.FLD_SHUKKA_MEISAI_CNT);
            sb.ApdL("    , IsNull(s.NUM, 0) AS ").ApdL(ComDefine.FLD_SHUKKA_MEISAI_QTY);
            sb.ApdL("    , T_TEHAI_MEISAI.HENKYAKUHIN_FLAG");
            sb.ApdL("    , T_TEHAI_MEISAI.DISP_NO");
            sb.ApdL("    , ce.VALUE3 AS ESTIMATE_COLOR");
            sb.ApdL("    , IsNull(ss.CNT, 0) AS SHUKKAZUMI_CNT");
            sb.ApdL("    , T_TEHAI_MEISAI.TEHAI_RIREKI");
            sb.ApdL("FROM ");
            sb.ApdL("    T_TEHAI_MEISAI");
            if (isLock)
                sb.ApdL("    WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("      INNER JOIN M_COMMON AS ce");
            sb.ApdN("        ON  ce.GROUP_CD = '").ApdN(ESTIMATE_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("        AND ce.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("        AND ce.VALUE1 = T_TEHAI_MEISAI.ESTIMATE_FLAG");
            sb.ApdL("      LEFT JOIN (SELECT TEHAI_RENKEI_NO, COUNT(NUM) AS SHUKKA_RENKEI, SUM(NUM) AS NUM  FROM T_SHUKKA_MEISAI GROUP BY TEHAI_RENKEI_NO) AS s");
            sb.ApdL("        ON T_TEHAI_MEISAI.TEHAI_RENKEI_NO = s.TEHAI_RENKEI_NO");
            if (!isLock)
            {
                sb.ApdL("      LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI AS TZK");
                sb.ApdL("        ON T_TEHAI_MEISAI.ZUMEN_KEISHIKI = TZK.ZUMEN_KEISHIKI");
            }
            sb.ApdN("      LEFT JOIN (SELECT TEHAI_RENKEI_NO, COUNT(*) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= '").ApdN(JYOTAI_FLAG.SHUKKAZUMI_VALUE1).ApdL("' GROUP BY TEHAI_RENKEI_NO) AS ss");
            sb.ApdL("        ON ss.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdN("WHERE ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("    AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            //sb.ApdN("    AND ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_MODE");
            //sb.ApdL("ORDER BY TEHAI_RENKEI_NO");
            sb.ApdL("ORDER BY DISP_NO");
            sb.ApdL("       , TEHAI_RENKEI_NO");

            // バインド変数設定
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            DataTable dt = new DataTable();
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_TEHAI_MEISAI.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 手配SKS連携データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携データ取得
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="dataTehaiMeisai">手配連携Noを含むテーブル</param>
    /// <returns>手配SKS連携データ</returns>
    /// <create>D.Okumura 2018/12/26</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// --------------------------------------------------
    private DataTable GetTehaiMeisaiSks(DatabaseHelper dbHelper, DataTable dataTehaiMeisai)
    {
        try
        {
            DataTable resultDt = null;
            List<DataTable> dtList = ChunkDataTable(dataTehaiMeisai, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                // SQL文
                StringBuilder sb = new StringBuilder();
                sb.ApdL("SELECT DISTINCT TEHAI_NO");
                sb.ApdL("FROM T_TEHAI_MEISAI_SKS ");
                sb.ApdL("WHERE ");

                // バインド変数設定
                DbParamCollection paramCollection = new DbParamCollection();
                if (chunkDt.Rows.Count < 1)
                {
                    sb.ApdN("1 = 0"); //ありえない条件を設定
                }
                else
                {
                    INewDbParameterBasic iNewParam = dbHelper;
                    sb.ApdL("(");
                    sb.ApdL("    TEHAI_RENKEI_NO IN (");

                    for (int i = 0; i < chunkDt.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (i % 10 == 0)
                            {
                                sb.ApdL();
                            }

                            if (i % 1000 == 0)
                            {
                                sb.ApdL("    )");
                                sb.ApdL("    OR TEHAI_RENKEI_NO IN (");
                                sb.ApdN("        ");
                            }
                            else
                            {
                                sb.ApdN("      , ");
                            }
                        }
                        else
                        {
                            sb.ApdN("        ");
                        }

                        var bindName = "TEHAI_RENKEI_NO" + i;
                        sb.ApdN(this.BindPrefix).ApdL(bindName);
                        paramCollection.Add(iNewParam.NewDbParameter(bindName, ComFunc.GetFld(chunkDt, i, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));
                    }
                    sb.ApdL("    )");
                    sb.ApdL(")");
                }
                // SQL実行
                DataTable dt = new DataTable(Def_T_TEHAI_MEISAI_SKS.Name);
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (resultDt == null)
                {
                    resultDt = dt.Clone();
                }

                foreach (DataRow dr in dt.Rows)
                {
                    resultDt.ImportRow(dr);
                }
            }

            return resultDt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }
    #endregion

    #region 表示データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update>D.Naito 2018/11/23 取得内容の修正</update>
    /// --------------------------------------------------
    public DataSet GetTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(GetTehaiMeisai(dbHelper, cond, false));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 技連マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/11/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetGiren(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("SELECT");
            sb.ApdL("      M_ECS.ECS_QUOTA");
            sb.ApdL("    , M_ECS.ECS_NO");
            sb.ApdL("    , M_ECS.PROJECT_NO");
            sb.ApdL("    , M_ECS.SEIBAN");
            sb.ApdL("    , M_ECS.CODE");
            sb.ApdL("    , M_ECS.KISHU");
            sb.ApdL("    , M_ECS.SEIBAN_CODE");
            sb.ApdL("    , M_ECS.AR_NO");
            sb.ApdL("    , M_ECS.KANRI_FLAG");
            //sb.ApdL("    , M_ECS.CREATE_DATE");
            //sb.ApdL("    , M_ECS.CREATE_USER_ID");
            //sb.ApdL("    , M_ECS.CREATE_USER_NAME");
            //sb.ApdL("    , M_ECS.UPDATE_DATE");
            //sb.ApdL("    , M_ECS.UPDATE_USER_ID");
            //sb.ApdL("    , M_ECS.UPDATE_USER_NAME");
            //sb.ApdL("    , M_ECS.MAINTE_DATE");
            //sb.ApdL("    , M_ECS.MAINTE_USER_ID");
            //sb.ApdL("    , M_ECS.MAINTE_USER_NAME");
            sb.ApdL("    , M_ECS.VERSION ");
            sb.ApdL("    , M_PROJECT.BUKKEN_NAME");
            sb.ApdL("FROM");
            sb.ApdL("    M_ECS");
            sb.ApdL("      LEFT JOIN M_PROJECT ON M_ECS.PROJECT_NO = M_PROJECT.PROJECT_NO");
            sb.ApdN("WHERE ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("    AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdL(" ORDER BY ECS_QUOTA");

            // バインド変数
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

            // SQL実行
            DataSet ds = new DataSet();
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_ECS.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 名称マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">CondT01用コンディション</param>
    /// <returns>データセット</returns>
    /// <create>D.Naito 2018/11/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSelectItem(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("SELECT");
            sb.ApdL("      SELECT_GROUP_CD");
            sb.ApdL("    , ITEM_NAME");
            sb.ApdL("FROM M_SELECT_ITEM");
            sb.ApdN("WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL(Def_M_SELECT_ITEM.SELECT_GROUP_CD);
            sb.ApdL("ORDER BY ITEM_NAME");

            // バインド変数
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;
            paramCollection.Add(iNewParameter.NewDbParameter(Def_M_SELECT_ITEM.SELECT_GROUP_CD, cond.SelectGroupCode));

            // SQL実行
            DataSet ds = new DataSet();
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パーツ名翻訳マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">CondT01用コンディション</param>
    /// <returns>データセット</returns>
    /// <create>D.Naito 2018/11/28</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPartsName(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("      HINMEI_JP");
            sb.ApdL("    , PARTS_CD");
            sb.ApdL("    , HINMEI");
            sb.ApdL("    , HINMEI_INV");
            sb.ApdL("    , MAKER");
            sb.ApdL("    , FREE2");
            sb.ApdL("    , NOTE");
            sb.ApdL("    , CUSTOMS_STATUS");
            sb.ApdL("FROM M_PARTS_NAME");
            sb.ApdL("WHERE 1 = 1");
            if (!string.IsNullOrEmpty(cond.PartsCode))
            {
                sb.ApdN("    AND PARTS_CD = ").ApdN(this.BindPrefix).ApdL(Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_PARTS_NAME.PARTS_CD, cond.PartsCode));

            }
            else
            {
                sb.ApdL("    AND PARTS_CD IS NULL");
            }
            if (!string.IsNullOrEmpty(cond.HinmeiJp))
            {
                sb.ApdN("    AND HINMEI_JP = ").ApdN(this.BindPrefix).ApdL(Def_M_PARTS_NAME.HINMEI_JP);
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_PARTS_NAME.HINMEI_JP, cond.HinmeiJp));
            }
            sb.ApdL("ORDER BY HINMEI_JP");


            // SQL実行
            DataSet ds = new DataSet();
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 図番/型式画像ファイル管理データ取得(IN検索)

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式画像ファイル管理データ取得(IN検索)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="listZumenKeishiki">図番/型式リスト</param>
    /// <returns>データテーブル</returns>
    /// <create>H.Tsuji 2019/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetManageZumenKeishikiInSearch(DatabaseHelper dbHelper, List<string> listZumenKeishiki)
    {
        try
        {
            // SQL文
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("       ZUMEN_KEISHIKI");
            sb.ApdL("     , FILE_NAME");
            sb.ApdL("     , SAVE_DIR");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("  FROM T_MANAGE_ZUMEN_KEISHIKI");
            sb.ApdL(" WHERE");
            sb.ApdN("       ZUMEN_KEISHIKI IN (");
            for (int index = 0; index < listZumenKeishiki.Count; index++)
            {
                // 検索条件が2件以上ある場合は連結
                if (index != 0) sb.ApdN(", ");

                // バインド変数は連番
                string paramName = Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI + "_" + index.ToString();
                sb.ApdN(this.BindPrefix).ApdN(paramName);
                paramCollection.Add(iNewParameter.NewDbParameter(paramName, listZumenKeishiki[index]));
            }
            sb.ApdL(")");

            // SQL実行
            DataTable dt = new DataTable(Def_T_MANAGE_ZUMEN_KEISHIKI.Name);
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    //#region Assy単価計算データ取得

    ///// --------------------------------------------------
    ///// <summary>
    ///// Assy単価計算データ取得
    ///// </summary>
    ///// <param name="dbHelper">データベースヘルパー</param>
    ///// <param name="assyNo">Assy_No</param>
    ///// <returns>データテーブル</returns>
    ///// <create>Y.Shioshi 2022/05/12</create>
    ///// <update></update>
    ///// --------------------------------------------------
    //private DataTable GetAssyUnitPriceData(DatabaseHelper dbHelper, CondT01 cond)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        DbParamCollection paramCollection = new DbParamCollection();
    //        INewDbParameterBasic iNewParameter = dbHelper;

    //        // SQL文
    //        StringBuilder sb = new StringBuilder();
    //        sb.ApdL("SELECT");
    //        sb.ApdL("      SUM(TTM.TEHAI_QTY * TTM.UNIT_PRICE) AS ASSY_TOTAL");
    //        sb.ApdL("    , (SELECT ");
    //        sb.ApdL("             TTMASSY.TEHAI_QTY ");
    //        sb.ApdL("       FROM T_TEHAI_MEISAI TTMASSY ");
    //        sb.ApdL("       WHERE TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY");
    //        sb.ApdL("             AND ASSY_NO = ").ApdN(this.BindPrefix).ApdL("ASSY_NO");
    //        sb.ApdL("      ) AS OYA_ASSY");
    //        sb.ApdL("FROM T_TEHAI_MEISAI TTM");
    //        sb.ApdN("WHERE ASSY_NO = ").ApdN(this.BindPrefix).ApdL("ASSY_NO");
    //        sb.ApdN("    AND TEHAI_FLAG != ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY");

    //        // バインド変数設定
    //        paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_ASSY", TEHAI_FLAG.ASSY_VALUE1));
    //        paramCollection.Add(iNewParameter.NewDbParameter("ASSY_NO", cond.AssyNo));

    //        // SQL実行
    //        dbHelper.Fill(sb.ToString(), paramCollection, dt);
    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message, ex);
    //    }
    //}

    //#endregion

    #region SKS手配明細WORKバージョン取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKバージョン取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="assyNo">Assy_No</param>
    /// <returns>データテーブル</returns>
    /// <create>J.Chen 2022/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetTehaiSKSWorkVersionData(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("SELECT");
            sb.ApdL("      T_TEHAI_SKS_WORK.CREATE_DATE");
            sb.ApdL("FROM T_TEHAI_SKS_WORK");
            sb.ApdN("WHERE T_TEHAI_SKS_WORK.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_NO", cond.TehaiNo));

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
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタの追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">技連マスタテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsGiren(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("INSERT INTO M_ECS (");
            sb.ApdL("       ECS_QUOTA");
            sb.ApdL("      ,ECS_NO");
            sb.ApdL("      ,PROJECT_NO");
            sb.ApdL("      ,SEIBAN");
            sb.ApdL("      ,CODE");
            sb.ApdL("      ,KISHU");
            sb.ApdL("      ,SEIBAN_CODE");
            sb.ApdL("      ,AR_NO");
            sb.ApdL("      ,KANRI_FLAG");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            //sb.ApdL("      ,MAINTE_DATE");
            //sb.ApdL("      ,MAINT_USER_ID");
            //sb.ApdL("      ,MAINT_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.ECS_QUOTA);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.ECS_NO);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.PROJECT_NO);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.SEIBAN);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CODE);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.KISHU);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.SEIBAN_CODE);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.AR_NO);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.KANRI_FLAG);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CREATE_USER_ID);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CREATE_USER_NAME);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.UPDATE_USER_ID);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.UPDATE_USER_NAME);
            //sb.ApdN("      ,").ApdL(this.SysTimestamp);
            //sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.MAINT_USER_ID);
            //sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_ECS.MAINT_USER_NAME);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.ECS_QUOTA, ComFunc.GetFldObject(dr, Def_M_ECS.ECS_QUOTA)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.ECS_NO, ComFunc.GetFldObject(dr, Def_M_ECS.ECS_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.PROJECT_NO, ComFunc.GetFldObject(dr, Def_M_ECS.PROJECT_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.SEIBAN, ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CODE, ComFunc.GetFldObject(dr, Def_M_ECS.CODE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.KISHU, ComFunc.GetFldObject(dr, Def_M_ECS.KISHU)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.SEIBAN_CODE, ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN_CODE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.AR_NO, ComFunc.GetFldObject(dr, Def_M_ECS.AR_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.KANRI_FLAG, ComFunc.GetFldObject(dr, Def_M_ECS.KANRI_FLAG, KANRI_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CREATE_USER_ID, this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CREATE_USER_NAME, this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/02</create>
    /// <update>D.Okumura 2019/01/22 図番/型式が空白のとき、検索用フィールドにはNULLを設定する</update>
    /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
    /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
    /// <update>J.Chen 2022/05/18 STEP14</update>
    /// <update>J.Chen 2024/10/23 履歴更新列追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    private int InsTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("INSERT INTO T_TEHAI_MEISAI (");
            sb.ApdL("     TEHAI_RENKEI_NO");
            sb.ApdL("    ,ECS_QUOTA");
            sb.ApdL("    ,ECS_NO");
            sb.ApdL("    ,SETTEI_DATE");
            sb.ApdL("    ,NOUHIN_SAKI");
            sb.ApdL("    ,SYUKKA_SAKI");
            sb.ApdL("    ,ZUMEN_OIBAN");
            sb.ApdL("    ,FLOOR");
            sb.ApdL("    ,ST_NO");
            sb.ApdL("    ,HINMEI_JP");
            sb.ApdL("    ,HINMEI");
            sb.ApdL("    ,HINMEI_INV");
            sb.ApdL("    ,ZUMEN_KEISHIKI");
            sb.ApdL("    ,ZUMEN_KEISHIKI_S");
            sb.ApdL("    ,TEHAI_QTY");
            sb.ApdL("    ,TEHAI_FLAG");
            sb.ApdL("    ,TEHAI_KIND_FLAG");
            sb.ApdL("    ,HACCHU_QTY");
            sb.ApdL("    ,SHUKKA_QTY");
            sb.ApdL("    ,ASSY_NO");
            sb.ApdL("    ,FREE1");
            sb.ApdL("    ,FREE2");
            sb.ApdL("    ,QUANTITY_UNIT");
            sb.ApdL("    ,ZUMEN_KEISHIKI2");
            sb.ApdL("    ,NOTE");
            sb.ApdL("    ,NOTE2");
            sb.ApdL("    ,NOTE3");
            sb.ApdL("    ,CUSTOMS_STATUS");
            sb.ApdL("    ,MAKER");
            sb.ApdL("    ,UNIT_PRICE");
            sb.ApdL("    ,ARRIVAL_QTY");
            sb.ApdL("    ,ASSY_QTY");
            sb.ApdL("    ,ESTIMATE_FLAG");
            sb.ApdL("    ,ESTIMATE_NO");
            sb.ApdL("    ,HENKYAKUHIN_FLAG");
            sb.ApdL("    ,DISP_NO");
            sb.ApdL("    ,CREATE_DATE");
            sb.ApdL("    ,CREATE_USER_ID");
            sb.ApdL("    ,CREATE_USER_NAME");
            sb.ApdL("    ,UPDATE_DATE");
            sb.ApdL("    ,UPDATE_USER_ID");
            sb.ApdL("    ,UPDATE_USER_NAME");
            sb.ApdL("    ,VERSION");
            sb.ApdL("    ,TEHAI_RIREKI");
            sb.ApdL(") VALUES (");
            sb.ApdN("     ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("SETTEI_DATE");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("NOUHIN_SAKI");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("SYUKKA_SAKI");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("FLOOR");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ST_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI_S");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("TEHAI_QTY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("HACCHU_QTY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("SHUKKA_QTY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ASSY_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("FREE1");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("QUANTITY_UNIT");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI2");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("NOTE2");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("NOTE3");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("MAKER");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("UNIT_PRICE");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ASSY_QTY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("HENKYAKUHIN_FLAG");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("DISP_NO");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("TEHAI_RIREKI");
            sb.ApdL(")");

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("ECS_QUOTA", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ECS_QUOTA)));
                paramCollection.Add(iNewParameter.NewDbParameter("ECS_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ECS_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("SETTEI_DATE", ComFunc.GetFldToDateTime(dr, Def_T_TEHAI_MEISAI.SETTEI_DATE).ToString("yyyy/MM/dd")));
                paramCollection.Add(iNewParameter.NewDbParameter("NOUHIN_SAKI", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOUHIN_SAKI)));
                paramCollection.Add(iNewParameter.NewDbParameter("SYUKKA_SAKI", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SYUKKA_SAKI)));
                paramCollection.Add(iNewParameter.NewDbParameter("ZUMEN_OIBAN", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_OIBAN)));
                paramCollection.Add(iNewParameter.NewDbParameter("FLOOR", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FLOOR)));
                paramCollection.Add(iNewParameter.NewDbParameter("ST_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ST_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("ZUMEN_KEISHIKI", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI)));
                var zumenKeishiki = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
#pragma warning disable 0618
                object zumenKeishikiS = string.IsNullOrEmpty(zumenKeishiki) ? DBNull.Value : (object)ComFunc.ConvertPDMWorkNameToZumenKeishikiS(zumenKeishiki);
#pragma warning restore 0618
                paramCollection.Add(iNewParameter.NewDbParameter("ZUMEN_KEISHIKI_S", zumenKeishikiS));
                paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_QTY)));
                paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_KIND_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG, TEHAI_KIND_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParameter.NewDbParameter("HACCHU_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HACCHU_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SHUKKA_QTY)));
                paramCollection.Add(iNewParameter.NewDbParameter("ASSY_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ASSY_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE1", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FREE1)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("QUANTITY_UNIT", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.QUANTITY_UNIT)));
                paramCollection.Add(iNewParameter.NewDbParameter("ZUMEN_KEISHIKI2", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE2", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE3", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE3)));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.CUSTOMS_STATUS)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("UNIT_PRICE", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.UNIT_PRICE)));
                paramCollection.Add(iNewParameter.NewDbParameter("ARRIVAL_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ARRIVAL_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter("ASSY_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ASSY_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter("ESTIMATE_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG, ESTIMATE_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParameter.NewDbParameter("ESTIMATE_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("HENKYAKUHIN_FLAG", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter("DISP_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.DISP_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_RIREKI", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RIREKI)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細履歴マスタの追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">技連マスタテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/11/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsTehaiMeisaiRireki(DatabaseHelper dbHelper, CondT01 cond, DataTable dt, String statusFlag)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("INSERT INTO M_TEHAI_MEISAI_RIREKI (");
            sb.ApdL("       ECS_QUOTA");
            sb.ApdL("      ,ECS_NO");
            sb.ApdL("      ,TEHAI_RENKEI_NO");
            sb.ApdL("      ,STATUS_FLAG");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.ECS_QUOTA);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.ECS_NO);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.TEHAI_RENKEI_NO);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.STATUS_FLAG);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.UPDATE_USER_ID);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL(Def_M_TEHAI_MEISAI_RIREKI.UPDATE_USER_NAME);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.ECS_QUOTA, ComFunc.GetFldObject(dr, Def_M_TEHAI_MEISAI_RIREKI.ECS_QUOTA)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.ECS_NO, ComFunc.GetFldObject(dr, Def_M_TEHAI_MEISAI_RIREKI.ECS_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.TEHAI_RENKEI_NO, ComFunc.GetFldObject(dr, Def_M_TEHAI_MEISAI_RIREKI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.STATUS_FLAG, statusFlag));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_TEHAI_MEISAI_RIREKI.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }


    #endregion

    #region UPDATE
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタの更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">技連マスタテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Naito 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdGiren(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("UPDATE M_ECS");
            sb.ApdL("SET");
            sb.ApdL("      PROJECT_NO = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.PROJECT_NO);
            sb.ApdL("    , SEIBAN = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.SEIBAN);
            sb.ApdL("    , CODE = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CODE);
            sb.ApdL("    , KISHU = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.KISHU);
            sb.ApdL("    , SEIBAN_CODE = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.SEIBAN_CODE);
            sb.ApdL("    , AR_NO = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.AR_NO);
            sb.ApdL("    , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.KANRI_FLAG);
            //sb.ApdL("    , CREATE_DATE = ").ApdL(this.SysTimestamp);
            //sb.ApdL("    , CREATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CREATE_USER_ID);
            //sb.ApdL("    , CREATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.CREATE_USER_NAME);
            sb.ApdL("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdL("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.UPDATE_USER_ID);
            sb.ApdL("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.UPDATE_USER_NAME);
            sb.ApdL("    , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("WHERE");
            sb.ApdL("    ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_QUOTA);
            sb.ApdL("    and ECS_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.PROJECT_NO, ComFunc.GetFldObject(dr, Def_M_ECS.PROJECT_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.SEIBAN, ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CODE, ComFunc.GetFldObject(dr, Def_M_ECS.CODE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.KISHU, ComFunc.GetFldObject(dr, Def_M_ECS.KISHU)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.SEIBAN_CODE, ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN_CODE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.AR_NO, ComFunc.GetFldObject(dr, Def_M_ECS.AR_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.KANRI_FLAG, ComFunc.GetFldObject(dr, Def_M_ECS.KANRI_FLAG, KANRI_FLAG.DEFAULT_VALUE1)));
                //paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CREATE_USER_ID, this.GetCreateUserID(cond)));
                //paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.CREATE_USER_NAME, this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.ECS_QUOTA, ComFunc.GetFldObject(dr, Def_M_ECS.ECS_QUOTA)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_M_ECS.ECS_NO, ComFunc.GetFldObject(dr, Def_M_ECS.ECS_NO)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細の更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Naito 2018/11/26</create>
    /// <update>D.Okumura 2019/01/22 図番/型式が空白のとき、検索用フィールドにはNULLを設定する</update>
    /// <update>H.Tsuji 2019/08/26 STEP10 組立パーツの識別処理を追加</update>
    /// <update>T.Nukaga 2019/11/18 STEP12 返却品管理対応</update>
    /// <update>J.Chen 2022/05/18 STEP14</update>
    /// <update>J.Chen 2024/10/23 履歴更新列追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    private int UpdTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("      ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_QUOTA);
            sb.ApdN("    , ECS_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_NO);
            sb.ApdN("    , SETTEI_DATE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.SETTEI_DATE);
            sb.ApdN("    , NOUHIN_SAKI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.NOUHIN_SAKI);
            sb.ApdN("    , SYUKKA_SAKI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.SYUKKA_SAKI);
            sb.ApdN("    , ZUMEN_OIBAN = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ZUMEN_OIBAN);
            sb.ApdN("    , FLOOR = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.FLOOR);
            sb.ApdN("    , ST_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ST_NO);
            sb.ApdN("    , HINMEI_JP = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.HINMEI_JP);
            sb.ApdN("    , HINMEI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.HINMEI);
            sb.ApdN("    , HINMEI_INV = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.HINMEI_INV);
            sb.ApdN("    , ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
            sb.ApdN("    , ZUMEN_KEISHIKI_S = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI_S);
            sb.ApdN("    , TEHAI_QTY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_QTY);
            sb.ApdN("    , TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_FLAG);
            sb.ApdN("    , TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG);
            sb.ApdN("    , HACCHU_QTY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.HACCHU_QTY);
            sb.ApdN("    , SHUKKA_QTY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.SHUKKA_QTY);
            sb.ApdN("    , ASSY_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ASSY_NO);
            sb.ApdN("    , FREE1 = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.FREE1);
            sb.ApdN("    , FREE2 = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.FREE2);
            sb.ApdN("    , QUANTITY_UNIT = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.QUANTITY_UNIT);
            sb.ApdN("    , ZUMEN_KEISHIKI2 = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2);
            sb.ApdN("    , NOTE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.NOTE);
            sb.ApdN("    , NOTE2 = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.NOTE2);
            sb.ApdN("    , NOTE3 = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.NOTE3);
            sb.ApdN("    , CUSTOMS_STATUS = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.CUSTOMS_STATUS);
            sb.ApdN("    , MAKER = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.MAKER);
            sb.ApdN("    , UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UNIT_PRICE);
            sb.ApdN("    , ARRIVAL_QTY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ARRIVAL_QTY);
            sb.ApdN("    , ASSY_QTY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ASSY_QTY);
            sb.ApdN("    , ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ESTIMATE_FLAG);
            sb.ApdN("    , ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ESTIMATE_NO);
            sb.ApdN("    , HENKYAKUHIN_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG);
            //sb.ApdN("    , CREATE_DATE = ").ApdL(this.SysTimestamp);
            //sb.ApdN("    , CREATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.CREATE_USER_ID);
            //sb.ApdN("    , CREATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.CREATE_USER_NAME);
            sb.ApdN("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_ID);
            sb.ApdN("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME);
            sb.ApdN("    , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , DISP_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.DISP_NO);
            sb.ApdN("    , TEHAI_RIREKI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_RIREKI);
            sb.ApdL("WHERE ");
            sb.ApdN("    TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ECS_QUOTA, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ECS_QUOTA)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ECS_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ECS_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.SETTEI_DATE, ComFunc.GetFldToDateTime(dr, Def_T_TEHAI_MEISAI.SETTEI_DATE).ToString("yyyy/MM/dd")));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.NOUHIN_SAKI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOUHIN_SAKI)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.SYUKKA_SAKI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SYUKKA_SAKI)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ZUMEN_OIBAN, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_OIBAN)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.FLOOR, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FLOOR)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ST_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ST_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.HINMEI_JP, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.HINMEI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.HINMEI_INV, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI)));
                var zumenKeishiki = ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI);
#pragma warning disable 0618
                object zumenKeishikiS = string.IsNullOrEmpty(zumenKeishiki) ? DBNull.Value : (object)ComFunc.ConvertPDMWorkNameToZumenKeishikiS(zumenKeishiki);
#pragma warning restore 0618
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI_S, zumenKeishikiS));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_QTY, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_QTY)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_KIND_FLAG, TEHAI_KIND_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.HACCHU_QTY, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HACCHU_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.SHUKKA_QTY, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.SHUKKA_QTY)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ASSY_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ASSY_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.FREE1, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FREE1)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.FREE2, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.QUANTITY_UNIT, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.QUANTITY_UNIT)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI2)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.NOTE, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.NOTE2, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE2)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.NOTE3, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.NOTE3)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.CUSTOMS_STATUS, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.CUSTOMS_STATUS)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.MAKER, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UNIT_PRICE, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.UNIT_PRICE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ARRIVAL_QTY, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ARRIVAL_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ASSY_QTY, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ASSY_QTY, 0m)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ESTIMATE_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG, ESTIMATE_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ESTIMATE_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_NO)));
                //paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.CREATE_USER_ID, this.GetCreateUserID(cond)));
                //paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.CREATE_USER_NAME, this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.HENKYAKUHIN_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.DISP_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.DISP_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_RIREKI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RIREKI)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新処理(単価(JPY)、手配種別 [その他])
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">DataRow</param>
    /// <returns></returns>
    /// <create>J.Chen 2022/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiUnitPriceForAnother(DatabaseHelper dbHelper, string tehairenkeino)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdL("       UNIT_PRICE = (SELECT MAX(T_TEHAI_SKS.TEHAI_UNIT_PRICE)");
            sb.ApdL("                       FROM T_TEHAI_SKS");
            sb.ApdL("                      INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("                              ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdN("                             AND T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO)");
            sb.ApdN("     , TEHAI_KIND_FLAG = (SELECT CASE WHEN COUNT(T_TEHAI_SKS.TEHAI_NO) > 0 THEN '" + TEHAI_KIND_FLAG.ANOTHER_VALUE1 + "'");
            sb.ApdN("                                 ELSE  '" + TEHAI_KIND_FLAG.NONE_VALUE1 + "'");
            sb.ApdN("                                 END AS TEHAI_KIND_FLAG");
            sb.ApdL("                       FROM T_TEHAI_SKS");
            sb.ApdL("                      INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("                              ON T_TEHAI_SKS.TEHAI_NO = T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdN("                             AND T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO)");

            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComDefine.SKS_RENKEI_USER_ID));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComDefine.SKS_RENKEI_USER_NAME));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", tehairenkeino));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新(有償)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2022/11/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiYushoVersion(DatabaseHelper dbHelper, CondT01 cond, DataRow dr)
    {
        try
        {
            int resultCnt = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
            sb.ApdL("     , INVOICE_UNIT_PRICE = ");
            sb.ApdL("       CASE WHEN ESTIMATE_NO IS NULL");
            sb.ApdL("               THEN NULL");
            sb.ApdL("           ELSE INVOICE_UNIT_PRICE");
            sb.ApdL("       END");
            sb.ApdL("     , SGA_RATE = NULL");
            sb.ApdL("     , SHIPPING_RATE = NULL");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));

            resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新(無償)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2022/11/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiMushoVersion(DatabaseHelper dbHelper, CondT01 cond, DataRow dr)
    {
        try
        {
            int resultCnt = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // t:手配明細、c:汎用マスタ(手配見積レート設定)
            sb.ApdL("UPDATE t");
            sb.ApdL("SET");
            sb.ApdL("     ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
            sb.ApdL("     , INVOICE_UNIT_PRICE = NULL"); // 無償では、未使用フィールドとする
            sb.ApdL("     , SGA_RATE = CAST(c.VALUE2 AS decimal(3, 0))");
            sb.ApdL("     , SHIPPING_RATE = CAST(c.VALUE3 AS decimal(3, 0))");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
            sb.ApdL(" FROM T_TEHAI_MEISAI AS t");
            sb.ApdL(" INNER JOIN M_COMMON AS c");
            sb.ApdN(" ON c.GROUP_CD = '").ApdN(ESTIMATE_RATE.GROUPCD).ApdL("' ");
            sb.ApdN(" AND c.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("ITEM_CD");
            sb.ApdN(" AND c.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
            paramCollection.Add(iNewParam.NewDbParameter("ITEM_CD", ESTIMATE_RATE.GRATIS_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新(未設定)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2022/11/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiMisetteiVersion(DatabaseHelper dbHelper, CondT01 cond, DataRow dr)
    {
        try
        {
            int resultCnt = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_NEUTRAL");
            sb.ApdL("     , INVOICE_UNIT_PRICE = NULL");
            sb.ApdL("     , SGA_RATE = NULL");
            sb.ApdL("     , SHIPPING_RATE = NULL");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_NEUTRAL", ESTIMATE_FLAG.NEUTRAL_VALUE1));

            resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region DELETE
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタデータ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelGiren(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            int ret = 0;

            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("DELETE FROM M_ECS");
            sb.ApdL("WHERE ");
            sb.ApdN("    ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.ECS_QUOTA);
            sb.ApdN("    AND ECS_NO = ").ApdN(this.BindPrefix).ApdL(Def_M_ECS.ECS_NO);

            // バインド変数設定
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParam.NewDbParameter(Def_M_ECS.ECS_QUOTA, cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter(Def_M_ECS.ECS_NO, cond.EcsNo));

            ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Naito 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            int ret = 0;

            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("DELETE FROM T_TEHAI_MEISAI");
            sb.ApdL("WHERE ");
            sb.ApdN("    ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_QUOTA);
            sb.ApdN("    AND ECS_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ECS_NO);

            // バインド変数設定
            INewDbParameterBasic iNewParam = dbHelper;
            DbParamCollection paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParam.NewDbParameter(Def_T_TEHAI_MEISAI.ECS_QUOTA, cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter(Def_T_TEHAI_MEISAI.ECS_NO, cond.EcsNo));

            ret = dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            StringBuilder sb = new StringBuilder();
            sb.ApdL("DELETE FROM T_TEHAI_MEISAI");
            sb.ApdL("WHERE ");
            sb.ApdN("    TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);

            // バインド変数設定
            INewDbParameterBasic iNewParam = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携削除
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="dataTehaiMeisai">削除対象の手配連携Noを含むテーブル</param>
    /// <returns>削除した件数</returns>
    /// <create>D.Okumura 2018/12/26</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// --------------------------------------------------
    private int DelTehaiMeisaiSks(DatabaseHelper dbHelper, DataTable dataTehaiMeisai)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dataTehaiMeisai, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                // SQL文
                StringBuilder sb = new StringBuilder();
                sb.ApdL("DELETE s FROM T_TEHAI_MEISAI_SKS AS s ");
                sb.ApdL("WHERE ");

                // バインド変数設定
                DbParamCollection paramCollection = new DbParamCollection();
                if (chunkDt.Rows.Count < 1)
                {
                    sb.ApdL("1 = 0"); //ありえない条件を指定
                }
                else
                {
                    INewDbParameterBasic iNewParam = dbHelper;
                    sb.ApdL("(");
                    sb.ApdL("    s.TEHAI_RENKEI_NO IN (");

                    for (int i = 0; i < chunkDt.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (i % 10 == 0)
                            {
                                sb.ApdL();
                            }

                            if (i % 1000 == 0)
                            {
                                sb.ApdL("    )");
                                sb.ApdL("    OR s.TEHAI_RENKEI_NO IN (");
                                sb.ApdN("        ");
                            }
                            else
                            {
                                sb.ApdN("      , ");
                            }
                        }
                        else
                        {
                            sb.ApdN("        ");
                        }

                        var bindName = "TEHAI_RENKEI_NO" + i;
                        sb.ApdN(this.BindPrefix).ApdL(bindName);
                        paramCollection.Add(iNewParam.NewDbParameter(bindName, ComFunc.GetFld(chunkDt, i, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));
                    }
                    sb.ApdL("    )");
                    sb.ApdL(")");
                }

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// 未参照のSKS手配明細を削除
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="dataTehaiMeisaiSks">削除対象の手配Noを含むデータテーブル</param>
    /// <returns>削除した件数</returns>
    /// <create>D.Okumura 2018/12/26</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// --------------------------------------------------
    private int DelTehaiSksNotReferenced(DatabaseHelper dbHelper, DataTable dataTehaiMeisaiSks)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dataTehaiMeisaiSks, 1000);

            foreach (DataTable chunkDt in dtList)
            {

                // SQL文
                StringBuilder sb = new StringBuilder();
                sb.ApdL("DELETE s FROM T_TEHAI_SKS AS s ");
                sb.ApdL("WHERE ");

                // バインド変数設定
                DbParamCollection paramCollection = new DbParamCollection();
                if (chunkDt.Rows.Count < 1)
                {
                    sb.ApdL("1 = 0"); //ありえない条件を指定
                }
                else
                {
                    INewDbParameterBasic iNewParam = dbHelper;

                    sb.ApdL("(");
                    sb.ApdN("    s.TEHAI_NO IN (");

                    for (int i = 0; i < chunkDt.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (i % 10 == 0)
                            {
                                sb.ApdL();
                            }

                            if (i % 1000 == 0)
                            {
                                sb.ApdL("    )");
                                sb.ApdL("    OR s.TEHAI_NO IN (");
                                sb.ApdN("      ");
                            }
                            else
                            {
                                sb.ApdN("    , ");
                            }
                        }
                        else
                        {
                            sb.ApdN("      ");
                        }

                        var bindName = "TEHAI_NO" + i;
                        sb.ApdN(this.BindPrefix).ApdL(bindName);
                        paramCollection.Add(iNewParam.NewDbParameter(bindName, ComFunc.GetFld(chunkDt, i, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO)));
                    }
                    sb.ApdL("    )");
                    sb.ApdL(")");
                    sb.ApdL(" AND NOT EXISTS(SELECT 1 FROM T_TEHAI_MEISAI_SKS AS t WHERE t.TEHAI_NO = s.TEHAI_NO)");
                }

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #endregion

    #region T0100020:SKS手配連携

    #region 制御

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSKSTehaiRenkeiDispData(DatabaseHelper dbHelper, CondT01 cond)
    {
        var ds = new DataSet();

        // 手配明細データ取得
        ds.Merge(this.GetSKSTehaiRenkeiTehaiMeisai(dbHelper, cond));
        // 最終連携日時取得
        ds.Merge(this.GetSksLastLink(dbHelper).Tables[Def_M_SKS.Name]);

        return ds;
    }

    #endregion

    #region SKS手配明細WORK取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORK取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>SKS手配明細WORK</returns>
    /// <create>H.Tajimi 2019/01/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSKSTehaiRenkeiTehaiSKSWork(DatabaseHelper dbHelper, CondT01 cond, ref string errMsgID, ref string[] args)
    {
        var ds = new DataSet();

        // 既に連携済のデータが存在するかどうか
        var dtTehaiMeisaiSKS = this.ExistsSKSTehaiRenkeiAlreadyRenkei(dbHelper, cond);
        if (UtilData.ExistsData(dtTehaiMeisaiSKS))
        {
            // 該当手配Noは既に連携済です。
            errMsgID = "T0100020023";
            return ds;
        }

        // SKS手配明細WORKデータ取得
        ds.Merge(this.GetSKSTehaiRenkeiTehaiSKSWorkExec(dbHelper, cond));

        return ds;
    }

    #endregion

    #region SKS手配連携更新

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配連携更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update>K.Tsutsumi 2019/02/13 手配種別もクリアするように変更、合わせて関数名も変更</update>
    /// <update>T.Nukaga 2021/11/30 Step14 新規登録時に重複チェック追加</update>
    /// --------------------------------------------------
    public bool UpdSKSTehaiRenkei(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtTehaiMeisai = ds.Tables[Def_T_TEHAI_MEISAI.Name];
            var dtIns = ds.Tables[ComDefine.DTTBL_INSERT];
            var dtUpd = ds.Tables[ComDefine.DTTBL_UPDATE];
            var dtDel = ds.Tables[ComDefine.DTTBL_DELETE];

            // 手配明細の行ロック
            var dtChkTehaiMeisai = this.LockSKSTehaiRenkeiTehaiMeisai(dbHelper, dtTehaiMeisai);
            {
                // 手配明細のバージョンチェック
                int[] notFoundIndex = null;
                var index = this.CheckSameData(dtTehaiMeisai, dtChkTehaiMeisai, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
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
            }

            // 削除行がある場合
            if (UtilData.ExistsData(dtDel))
            {
                // 出荷明細の存在チェック
                var dtShukkaMeisai = this.ExistsSKSTehaiRenkeiShukkaMeisai(dbHelper, cond, dtTehaiMeisai);
                if (UtilData.ExistsData(dtShukkaMeisai))
                {
                    // 出荷済みのデータが含まれているため削除できません。
                    errMsgID = "T0100020016";
                    return false;
                }

                // 手配SKS連携の行ロック
                var dtTeahiMeisaiSKS = this.LockSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtTehaiMeisai, dtDel);

                //// SKS手配明細の行ロック
                //var dtChkTehaiSKS = this.LockSKSTehaiRenkeiTehaiSKS(dbHelper, dtDel);
                //// SKS手配明細のバージョンチェック
                //int[] notFoundIndex = null;
                //var index = this.CheckSameData(dtDel, dtChkTehaiSKS, out notFoundIndex, Def_T_TEHAI_SKS.VERSION, Def_T_TEHAI_SKS.TEHAI_NO);
                //if (0 <= index)
                //{
                //    // 他端末で更新された為、更新できませんでした。
                //    errMsgID = "A9999999027";
                //    return false;
                //}
                //else if (notFoundIndex != null)
                //{
                //    // 他端末で更新された為、更新できませんでした。
                //    errMsgID = "A9999999027";
                //    return false;
                //}
                
                // 手配SKS連携削除
                this.DelSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtTeahiMeisaiSKS);

                //// SKS手配明細削除
                //this.DelSKSTehaiRenkeiTehaiSKS(dbHelper, dtDel);
            }

            bool isUpdate = false;
            // 変更行がある場合
            if (UtilData.ExistsData(dtUpd))
            {
                // SKS手配明細の行ロック
                var dtChkTehaiSKS = this.LockSKSTehaiRenkeiTehaiSKS(dbHelper, dtUpd);
                // SKS手配明細のバージョンチェック
                int[] notFoundIndex = null;
                var index = this.CheckSameData(dtUpd, dtChkTehaiSKS, out notFoundIndex, Def_T_TEHAI_SKS.VERSION, Def_T_TEHAI_SKS.TEHAI_NO);
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

                // SKS手配明細の更新
                this.UpdSKSTehaiRenkeiTehaiSKS(dbHelper, cond, dtUpd);

                isUpdate = true;
            }

            // 新規行がある場合
            if (UtilData.ExistsData(dtIns))
            {
                // 手配明細SKSに同じ手配Noデータが存在していればNG(他で既に連携済)
                string tmpStr = this.CheckExistTehaiMeisaiSKSForInsert(dbHelper, dtIns, cond);

                // 手配Noがあれば連携済のため使用不可
                if (!string.IsNullOrEmpty(tmpStr))
                {
                    // 手配No{0}は既に連携済です。
                    errMsgID = "T0100020033";
                    args = new string[]{tmpStr};
                    return false;
                }

                // SKS手配明細WORKの行ロック
                var dtChkTehaiSKSWork = this.LockSKSTehaiRenkeiTehaiSKSWork(dbHelper, dtIns);

                // SKS手配明細WORKのバージョンチェック
                int[] notFoundIndex = null;
                var index = this.CheckSameData(dtIns, dtChkTehaiSKSWork, out notFoundIndex, Def_T_TEHAI_SKS_WORK.CREATE_DATE, Def_T_TEHAI_SKS_WORK.TEHAI_NO);
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

                // SKS手配明細の登録
                this.InsSKSTehaiRenkeiTehaiSKS(dbHelper, cond, dtIns);

                // 手配SKS連携の登録
                this.InsSKSTehaiRenkeiTehaiMeisaiSKS(dbHelper, dtTehaiMeisai, dtIns);

                isUpdate = true;
            }

            if (isUpdate)
            {
                // 全削除以外の場合は、手配明細を更新
                foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                {
                    // 手配明細の更新
                    this.UpdSKSTehaiRenkeiTehaiMeisai(dbHelper, cond, drTehaiMeisai);
                }
            }
            else
            {
                // 全削除の場合は、手配種別に応じて手配明細単価クリア
                if (cond.TehaiKindFlag == TEHAI_KIND_FLAG.ACROSS_VALUE1
                 || cond.TehaiKindFlag == TEHAI_KIND_FLAG.AGGRIGATE_VALUE1
                 || cond.TehaiKindFlag == TEHAI_KIND_FLAG.ESTIMATE_VALUE1
                 || cond.TehaiKindFlag == TEHAI_KIND_FLAG.ANOTHER_VALUE1)
                {
                    foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
                    {
                        // 手配明細の更新
                        this.UpdSKSTehaiRenkeiTehaiMeisaiClear(dbHelper, cond, drTehaiMeisai);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        return true;
    }

    #endregion

    #region 手配SKS連携データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiMeisai">手配明細</param>
    /// <param name="dtTehaiSKSWork">SKS手配明細WORK</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update>J.Chen 2022/10/31 手配情報登録画面用修正追加</update>
    /// --------------------------------------------------
    private bool InsSKSTehaiRenkeiTehaiMeisaiSKS(DatabaseHelper dbHelper, DataTable dtTehaiMeisai, DataTable dtTehaiSKSWork)
    {
        foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
        {
            foreach (DataRow drTehaiSKSWork in dtTehaiSKSWork.Rows)
            {
                try
                {
                    if ((ComFunc.GetFld(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO) == ComFunc.GetFld(drTehaiSKSWork, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO))
                        || (!dtTehaiSKSWork.Columns.Contains(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)))
                    {
                        // Insert
                        this.InsSKSTehaiRenkeiTehaiSKSExec(dbHelper, drTehaiMeisai, drTehaiSKSWork);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.GetType() == typeof(System.Data.DuplicateNameException))
                    {
                        // 通常ありえないが、連携データを作ることが目的なのでエラーとしない
                        continue;
                    }
                    else
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }
        return true;
    }

    #endregion

    #region SKS手配明細の登録

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細の登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiSKSWork">SKS手配明細WORK</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiSKSWork)
    {
        foreach (DataRow drTehaiSKSWork in dtTehaiSKSWork.Rows)
        {
            var dtTehaiSKS = this.ExistsSKSTehaiRenkeiTehaiSKS(dbHelper, drTehaiSKSWork);
            if (!UtilData.ExistsData(dtTehaiSKS))
            {
                // 存在しない場合はInsert
                this.InsSKSTehaiRenkeiTehaiSKSExec(dbHelper, cond, drTehaiSKSWork);
            }
        }
        return true;
    }

    #endregion

    #region 手配SKS連携(T_TEHAI_MEISAI_SKS)の登録チェック(わたり発注/その他対象)
    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携(T_TEHAI_MEISAI_SKS)の登録チェック(わたり発注/その他対象)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="dt">チェック対象データ</param>
    /// <param name="cond">手配種別</param>
    /// <returns>string.Empty or 登録済手配No</returns>
    /// <create>T.Nukaga 2021/11/29 Step14</create>
    /// <update></update>
    /// --------------------------------------------------
    private string CheckExistTehaiMeisaiSKSForInsert(DatabaseHelper dbHelper, DataTable dt, CondT01 cond)
    {
        string rtnStr = string.Empty;

        // 手配種別がわたり発注/その他でなければ対象外
        if (cond.TehaiKindFlag != TEHAI_KIND_FLAG.ACROSS_VALUE1 && cond.TehaiKindFlag != TEHAI_KIND_FLAG.ANOTHER_VALUE1)
        {
            return rtnStr;
        }

        // データがあればNG
        foreach (DataRow drTehaiMeisaiSKS in dt.Rows)
        {
            CondT01 tmpCond = new CondT01(cond.LoginInfo);
            tmpCond.TehaiKindFlag = cond.TehaiKindFlag;
            tmpCond.TehaiNo = UtilData.GetFld(drTehaiMeisaiSKS, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO);

            var dtTehaiMeisaiSKS = this.ExistsSKSTehaiRenkeiAlreadyRenkei(dbHelper, tmpCond);
            if (UtilData.ExistsData(dtTehaiMeisaiSKS))
            {
                // データがあれば既に他で登録済
                rtnStr = tmpCond.TehaiNo;
                break;
            }
        }

        return rtnStr;
    }

    #endregion 手配SKS連携(T_TEHAI_MEISAI_SKS)の登録チェック(わたり発注/その他対象)

    #endregion 制御

    #region SQL

    #region SELECT

    #region 手配明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/16</create>
    /// <update>K.Tsutsumi 2019/02/13 SMS:SKS=1:Nで同じ手配情報が複数行表示される件を対応</update>
    /// <update>T.Nukaga 2019/11/12 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
    /// <update>J.Chen 2022/05/18 STEP14</update>
    /// <update>J.Chen 2023/02/10 見積No追加取得</update>
    /// --------------------------------------------------
    public DataTable GetSKSTehaiRenkeiTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            var dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT 0 AS SELECT_CHK");
            sb.ApdL("     , M_PROJECT.BUKKEN_NAME");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("     , M_ECS.SEIBAN");
            sb.ApdL("     , M_ECS.CODE");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS TEHAI_KIND_FLAG_NAME");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_KEISHIKI2");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI_JP");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_OIBAN");
            sb.ApdL("     , T_TEHAI_MEISAI.HACCHU_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.SHUKKA_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.UNIT_PRICE");
            //sb.ApdL("     , T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("     , (");
            sb.ApdL("         SELECT");
            sb.ApdL("                 T_TEHAI_MEISAI_SKS.TEHAI_NO + ' '");
            sb.ApdL("           FROM");
            sb.ApdL("                 T_TEHAI_MEISAI_SKS");
            sb.ApdL("          WHERE");
            sb.ApdL("                 T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("            FOR XML PATH ('')");
            sb.ApdL("        ) AS TEHAI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.VERSION");
            sb.ApdL("     , (SELECT COUNT(1)");
            sb.ApdL("          FROM T_SHUKKA_MEISAI");
            sb.ApdL("         WHERE T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO) AS CNT");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("     , T_TEHAI_MEISAI.DISP_NO");
            // 見積No追加
            sb.ApdL("     , T_TEHAI_MEISAI.ESTIMATE_NO");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN M_ECS");
            sb.ApdL("         ON M_ECS.ECS_QUOTA = T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("        AND M_ECS.ECS_NO = T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL(" INNER JOIN M_PROJECT");
            sb.ApdL("         ON M_PROJECT.PROJECT_NO = M_ECS.PROJECT_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_TEHAI_KIND_FLAG");
            sb.ApdL("        AND COM1.VALUE1 = T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            //sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI_SKS");
            //sb.ApdL("         ON T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL(" LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= "
                        ).ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" GROUP BY TEHAI_RENKEI_NO) AS WSM2"
                        ).ApdL(" ON WSM2.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));

                }
            }
            sb.ApdL(" WHERE ");
            sb.ApdN("       T_TEHAI_MEISAI.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            if (!string.IsNullOrEmpty(cond.TehaiSKSRenkeiFlag) && cond.TehaiSKSRenkeiFlag == TEHAI_SKS_RENKEI_FLAG.PROC_VALUE1)
            {
                sb.ApdL("   AND NOT EXISTS (");
                sb.ApdL("       SELECT 1");
                sb.ApdL("         FROM T_TEHAI_MEISAI_SKS");
                sb.ApdL("        WHERE T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
                sb.ApdL("   )");
            }
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                    sb.ApdL("   AND T_TEHAI_MEISAI.SHUKKA_QTY - IsNull(WSM2.CNT, 0) > 0");
                }
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                }
                else
                {
                    sb.ApdN("   AND M_PROJECT.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
                }
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND T_TEHAI_MEISAI.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND M_ECS.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND M_ECS.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                sb.ApdL("   AND (");
                sb.ApdN("        T_TEHAI_MEISAI.ZUMEN_KEISHIKI LIKE ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
                sb.ApdN("     OR T_TEHAI_MEISAI.ZUMEN_KEISHIKI2 LIKE ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
                sb.ApdL("   )");
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", cond.ZumenKeishiki + "%"));
            }
            if (!string.IsNullOrEmpty(cond.TehaiNo))
            {
                //sb.ApdN("   AND T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
                sb.ApdN("   AND EXISTS (");
                sb.ApdN("       SELECT 1");
                sb.ApdN("         FROM T_TEHAI_MEISAI_SKS");
                sb.ApdN("         WHERE T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
                sb.ApdN("           AND T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
                sb.ApdN("   )");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));
            }
            sb.ApdL("ORDER BY");
            sb.ApdL("       T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("        ,T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("        ,T_TEHAI_MEISAI.DISP_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GC_TEHAI_KIND_FLAG", TEHAI_KIND_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));

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

    #region SKS手配明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiMeisai">DataTable(手配明細)</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiMeisai)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS.KAITO_DATE");
            sb.ApdL("     , COM1.ITEM_NAME AS HACCHU_FLAG_NAME");
            sb.ApdL("     , T_TEHAI_SKS.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS.KENPIN_UMU");
            sb.ApdL("     , T_TEHAI_SKS.ARRIVAL_QTY");
            sb.ApdL("     , T_TEHAI_SKS.VERSION");
            sb.ApdL("  FROM T_TEHAI_SKS");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("     SELECT T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("       FROM T_TEHAI_MEISAI_SKS");
            sb.ApdL("      INNER JOIN T_TEHAI_MEISAI");
            sb.ApdL("              ON T_TEHAI_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO");
            sb.ApdL("      WHERE");
            if (dtTehaiMeisai.Rows.Count < 1)
            {
                sb.ApdL("            1 = 0");
            }
            else
            {
                sb.ApdN("            T_TEHAI_MEISAI.TEHAI_RENKEI_NO IN (");
                for (int rowIndex = 0; rowIndex < dtTehaiMeisai.Rows.Count; rowIndex++)
                {
                    string bindName = Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO + (rowIndex + 1).ToString();
                    sb.ApdN(this.BindPrefix).ApdN(bindName);
                    if (rowIndex != dtTehaiMeisai.Rows.Count - 1)
                    {
                        sb.ApdN(",");
                    }
                    paramCollection.Add(iNewParam.NewDbParameter(bindName, ComFunc.GetFld(dtTehaiMeisai, rowIndex, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                }
                sb.ApdL(")");
            }
            sb.ApdL("      GROUP BY");
            sb.ApdL("            T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL(" ) MAIN ON MAIN.TEHAI_NO = T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_HACCHU_FLAG");
            sb.ApdL("        AND COM1.VALUE1 = T_TEHAI_SKS.HACCHU_ZYOTAI_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GC_HACCHU_FLAG", HACCHU_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            DataTable dtTmp = new DataTable();
            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
            ret.Merge(dtTmp);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS手配明細WORKデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetSKSTehaiRenkeiTehaiSKSWorkExec(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KAITO_DATE");
            sb.ApdL("     , COM1.ITEM_NAME AS HACCHU_FLAG_NAME");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdN("     , CASE ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            sb.ApdN("         WHEN ").ApdN(this.BindPrefix).ApdN("WATARI_TEHAI_KIND_FLAG")
                      .ApdN(" THEN ").ApdN(this.BindPrefix).ApdL("KENPIN_OFF");
            sb.ApdN("         ELSE ").ApdN(this.BindPrefix).ApdL("KENPIN_ON");
            sb.ApdL("        END AS KENPIN_UMU");
            sb.ApdL("     , 0 AS ARRIVAL_QTY");
            // SKS手配明細と構造を合せるためにVERSIONに名称を変更する
            sb.ApdL("     , T_TEHAI_SKS_WORK.CREATE_DATE AS VERSION");
            sb.ApdL("  FROM T_TEHAI_SKS_WORK");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_HACCHU_FLAG");
            sb.ApdL("        AND COM1.VALUE1 = T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE T_TEHAI_SKS_WORK.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", cond.TehaiKindFlag));
            paramCollection.Add(iNewParam.NewDbParameter("WATARI_TEHAI_KIND_FLAG", TEHAI_KIND_FLAG.ACROSS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_ON", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_OFF", KENPIN_UMU.OFF_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("GC_HACCHU_FLAG", HACCHU_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));

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

    #region 手配明細データ行ロック

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiMeisai">手配明細</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockSKSTehaiRenkeiTehaiMeisai(DatabaseHelper dbHelper, DataTable dtTehaiMeisai)
    {
        try
        {
            var ret = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_FLAG");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_KIND_FLAG");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_KEISHIKI2");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI_JP");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_OIBAN");
            sb.ApdL("     , T_TEHAI_MEISAI.HACCHU_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.SHUKKA_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_MEISAI.VERSION");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));

                DataTable dtTmp = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                ret.Merge(dtTmp);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS手配明細データ行ロック

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細データ行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiSKS">SKS手配明細</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, DataTable dtTehaiSKS)
    {
        try
        {
            var ret = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS.VERSION");
            sb.ApdL("  FROM T_TEHAI_SKS");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO)));

                DataTable dtTmp = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                ret.Merge(dtTmp);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SKS手配明細WORKデータ行ロック

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細WORKデータ行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiSKSWork">SKS手配明細WORK</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockSKSTehaiRenkeiTehaiSKSWork(DatabaseHelper dbHelper, DataTable dtTehaiSKSWork)
    {
        try
        {
            var ret = new DataTable(Def_T_TEHAI_SKS_WORK.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.CREATE_DATE");
            sb.ApdL("  FROM T_TEHAI_SKS_WORK");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_SKS_WORK.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiSKSWork in dtTehaiSKSWork.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKSWork, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));

                DataTable dtTmp = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                ret.Merge(dtTmp);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配SKS連携データ行ロック

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携データ行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiMeisai">手配明細</param>
    /// <param name="dtTehaiSKS">SKS手配明細</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockSKSTehaiRenkeiTehaiMeisaiSKS(DatabaseHelper dbHelper, DataTable dtTehaiMeisai, DataTable dtTehaiSKS)
    {
        try
        {
            var ret = new DataTable(Def_T_TEHAI_MEISAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("  FROM T_TEHAI_MEISAI_SKS");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdN(" WHERE T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdN("   AND T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
            {
                foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
                {
                    DbParamCollection paramCollection = new DbParamCollection();

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKS, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO)));

                    DataTable dtTmp = new DataTable();
                    // SQL実行
                    dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                    ret.Merge(dtTmp);
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiMeisai">手配明細</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable ExistsSKSTehaiRenkeiShukkaMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiMeisai)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_SHUKKA_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("  FROM T_SHUKKA_MEISAI");
            sb.ApdN(" WHERE T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow drTehaiMeisai in dtTehaiMeisai.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));

                DataTable dtTmp = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                ret.Merge(dtTmp);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 既に連携済の手配明細存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 既に連携済の手配明細存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/23</create>
    /// <update>T.Nukaga 2021/11/30 Step14 手配種別がわたり発注/その他は同じ手配種別のデータも比較対象に変更</update>
    /// <update>D.Okumura 2022/01/26 Step14 EFA_SMS-237 手配種別がその他は同じ手配種別のデータも比較対象に変更</update>
    /// <update>J.Chen 2022/10/26 手配登録画面用の手配連携No比較追加</update>
    /// --------------------------------------------------
    public DataTable ExistsSKSTehaiRenkeiAlreadyRenkei(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("  FROM T_TEHAI_MEISAI_SKS");
            sb.ApdN(" WHERE T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            sb.ApdL("   AND EXISTS (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM T_TEHAI_MEISAI");
            sb.ApdL("        WHERE T_TEHAI_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO");
            if (cond.TehaiKindFlag != TEHAI_KIND_FLAG.ANOTHER_VALUE1 && String.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                sb.ApdN("          AND T_TEHAI_MEISAI.TEHAI_KIND_FLAG <> ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", cond.TehaiKindFlag));
            }
            if (!String.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                sb.ApdN("          AND T_TEHAI_MEISAI.TEHAI_RENKEI_NO <> ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", cond.TehaiRenkeiNo));
            }
            if (!String.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("          AND T_TEHAI_MEISAI.ECS_NO <> ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            sb.ApdL("   )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));

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

    #region SKS手配明細存在確認

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="drTehaiSKSWork">SKS手配明細</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable ExistsSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, DataRow drTehaiSKSWork)
    {
        try
        {
            var dt = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_TEHAI_SKS.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS.VERSION");
            sb.ApdL("  FROM T_TEHAI_SKS");
            sb.ApdN(" WHERE T_TEHAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKSWork, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));

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

    #region SKS手配明細登録

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="drTehaiSKSWork">SKS手配明細WORK</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update>K.Tsutsumi 2020/11/18 EFA_SMS-158 T_TEHAI_SKS_WORK.KATASHIKI 格納領域拡張</update>
    /// <update>D.Okumura 2022/01/04 HACCHU_FLAG/ZENKAI_HACCHU_FLAG/UKEIRE_DATE/KENSHU_DATEを転送するように変更</update>
    /// --------------------------------------------------
    public int InsSKSTehaiRenkeiTehaiSKSExec(DatabaseHelper dbHelper, CondT01 cond, DataRow drTehaiSKSWork)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_TEHAI_SKS");
            sb.ApdL("(");
            sb.ApdL("       TEHAI_NO");
            sb.ApdL("     , TEHAI_QTY");
            sb.ApdL("     , TEHAI_UNIT_PRICE");
            sb.ApdL("     , KAITO_DATE");
            sb.ApdL("     , ZUMEN_OIBAN");
            sb.ApdL("     , HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , SEIBAN_CODE");
            sb.ApdL("     , HINBAN");
            sb.ApdL("     , PDM_WORK_NAME");
            sb.ApdL("     , KATASHIKI");
            sb.ApdL("     , ECS_NO");
            sb.ApdL("     , KENPIN_UMU");
            sb.ApdL("     , ARRIVAL_QTY");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , HACCHU_FLAG");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , KENSHU_DATE");
            sb.ApdL("     , ZENKAI_HACCHU_FLAG");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_SKS_WORK.TEHAI_NO");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_SKS_WORK.TEHAI_UNIT_PRICE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KAITO_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.ZUMEN_OIBAN");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.SEIBAN_CODE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HINBAN");
            sb.ApdL("     , T_TEHAI_SKS_WORK.PDM_WORK_NAME");
            // SKS側の型式は、40文字制限であるため、T_TEHAI_SKS_WORK.KATASHIKIは、NVARCHAR(40)へ拡張した。
            // SMS側の型式は、30文字制限であるため、調整する。
            sb.ApdN("     , SUBSTRING(T_TEHAI_SKS_WORK.KATASHIKI, 1, ").ApdN(this.BindPrefix).ApdL("KATASHIKI_LENGTH").ApdL(") AS KATASHIKI");
            sb.ApdL("     , T_TEHAI_SKS_WORK.ECS_NO");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_FLAG");
            sb.ApdL("     , T_TEHAI_SKS_WORK.UKEIRE_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.KENSHU_DATE");
            sb.ApdL("     , T_TEHAI_SKS_WORK.HACCHU_FLAG");
            sb.ApdL("  FROM T_TEHAI_SKS_WORK");
            sb.ApdN(" WHERE T_TEHAI_SKS_WORK.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KATASHIKI_LENGTH", ComDefine.MAX_BYTE_LENGTH_ZUMEN_KEISHIKI));
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", ComFunc.GetFldObject(drTehaiSKSWork, Def_T_TEHAI_SKS.KENPIN_UMU)));
            paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", 0m));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKSWork, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));

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

    #region 手配SKS連携登録

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="drTehaiMeisai">手配明細</param>
    /// <param name="drTehaiSKSWork">SKS手配明細WORK</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsSKSTehaiRenkeiTehaiSKSExec(DatabaseHelper dbHelper, DataRow drTehaiMeisai, DataRow drTehaiSKSWork)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_TEHAI_MEISAI_SKS");
            sb.ApdL("(");
            sb.ApdL("       TEHAI_RENKEI_NO");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKSWork, Def_T_TEHAI_SKS_WORK.TEHAI_NO)));

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

    #region UPDATE

    #region 手配明細更新

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="drTehaiMeisai">手配明細</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update>H.Tajimi 2019/02/12 手配種別更新</update>
    /// --------------------------------------------------
    private int UpdSKSTehaiRenkeiTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond, DataRow drTehaiMeisai)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            if (this.HasColumnValue(drTehaiMeisai, Def_T_TEHAI_MEISAI.HACCHU_QTY))
            {
                sb.ApdN("     , HACCHU_QTY = ").ApdN(this.BindPrefix).ApdL("HACCHU_QTY");
                paramCollection.Add(iNewParam.NewDbParameter("HACCHU_QTY", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.HACCHU_QTY)));
            }
            if (this.HasColumnValue(drTehaiMeisai, Def_T_TEHAI_MEISAI.SHUKKA_QTY))
            {
                sb.ApdN("     , SHUKKA_QTY = ").ApdN(this.BindPrefix).ApdL("SHUKKA_QTY");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_QTY", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.SHUKKA_QTY)));
            }
            if (this.HasColumnValue(drTehaiMeisai, Def_T_TEHAI_MEISAI.UNIT_PRICE))
            {
                sb.ApdN("     , UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL("UNIT_PRICE");
                paramCollection.Add(iNewParam.NewDbParameter("UNIT_PRICE", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.UNIT_PRICE)));
            }
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", cond.TehaiKindFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新(手配種別と単価クリア専用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="drTehaiMeisai">手配明細</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/24</create>
    /// <update>K.Tsutsumi 2019/02/13 手配種別もクリアするように変更、合わせて関数名も変更</update>
    /// --------------------------------------------------
    private int UpdSKSTehaiRenkeiTehaiMeisaiClear(DatabaseHelper dbHelper, CondT01 cond, DataRow drTehaiMeisai)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL("UNIT_PRICE");
            sb.ApdN("     , TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UNIT_PRICE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG", TEHAI_KIND_FLAG.NONE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisai, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region SKS手配明細更新

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiSKS">SKS手配明細</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiSKS)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_SKS");
            sb.ApdL("   SET");
            sb.ApdN("       KENPIN_UMU = ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", ComFunc.GetFldObject(drTehaiSKS, Def_T_TEHAI_SKS.KENPIN_UMU)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO)));

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

    #endregion

    #region DELETE

    #region SKS手配明細削除

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiSKS">SKS手配明細</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelSKSTehaiRenkeiTehaiSKS(DatabaseHelper dbHelper, DataTable dtTehaiSKS)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_TEHAI_SKS");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiSKS in dtTehaiSKS.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiSKS, Def_T_TEHAI_SKS.TEHAI_NO)));

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

    #region 手配SKS連携削除

    /// --------------------------------------------------
    /// <summary>
    /// 手配SKS連携削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiMeisaiSKS">手配SKS連携</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2019/01/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelSKSTehaiRenkeiTehaiMeisaiSKS(DatabaseHelper dbHelper, DataTable dtTehaiMeisaiSKS)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_TEHAI_MEISAI_SKS");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdN("   AND TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow drTehaiMeisaiSKS in dtTehaiMeisaiSKS.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(drTehaiMeisaiSKS, Def_T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(drTehaiMeisaiSKS, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO)));

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

    #endregion

    #endregion

    #region メソッド

    /// --------------------------------------------------
    /// <summary>
    /// 指定カラムに値が設定されているかどうか
    /// </summary>
    /// <param name="drTehaiMeisai">DataRow</param>
    /// <param name="columnNames">列名</param>
    /// <returns>true:設定済 false:未設定</returns>
    /// <create>H.Tajimi 2019/01/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool HasColumnValue(DataRow dr, params string[] columnNames)
    {
        foreach (string columnName in columnNames)
	    {
            if (dr.Table.Columns.Contains(columnName))
            {
                if (ComFunc.GetFldObject(dr, columnName) != DBNull.Value && ComFunc.GetFldObject(dr, columnName) != null)
                {
                    return true;
                }
            }
	    }
        return false;
    }

    #endregion

    #endregion

    #region T0100030:手配明細照会

    #region 制御

    #region 初期表示(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 初期表示(手配情報照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/28</create>
    /// <update>K.Tsutsumi 2019/09/11 「全て(AR)」と「全て(AR未出荷)」の設定はローカル側へ移動</update>
    /// --------------------------------------------------
    public DataSet GetInitTehaiJohoShokai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            using (WsCommonImpl comImpl = new WsCommonImpl())
            {
                var ds = new DataSet();
                {
                    // 出荷区分取得
                    CondCommon condCommon = new CondCommon(cond.LoginInfo);
                    condCommon.GroupCD = SHUKKA_FLAG.GROUPCD;
                    var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                    dt.TableName = SHUKKA_FLAG.GROUPCD;
                    ds.Merge(dt);
                }

                //プロジェクトマスタ取得
                ds.Merge(this.GetBukkenNameList(dbHelper));
                return ds;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #region 表示データ取得(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(手配情報照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/29</create>
    /// <update>K.Tsutsumi 2019/09/07 進捗追加</update>
    /// <update>D.Okumura 2021/07/29 EFA_SMS-210 納入状態・手配Noの結合を別SQLで実行するように変更</update>
    /// --------------------------------------------------
    public DataSet GetTehaiJohoShokai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(this.GetTehaiJohoShokaiWithSks(dbHelper, cond).Copy());
            ds.Tables.Add(this.Sql_GetProgress(dbHelper, cond).Copy());

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(SKS付き手配情報照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>D.Okumura 2021/07/29 EFA_SMS-210 納入状態・手配Noの結合を別SQLで実行するように変更</create>
    /// <update>J.Chen 2022/07/04 受入日追加</update>
    /// --------------------------------------------------
    internal DataTable GetTehaiJohoShokaiWithSks(DatabaseHelper dbHelper, CondT01 cond)
    {
        var dt = this.Sql_GetTehaiJohoShokai(dbHelper, cond);
        foreach (DataRow dr in dt.Rows)
        {
            // 発注以外はスキップ
            if (!TEHAI_FLAG.ORDERED_VALUE1.Equals(ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_FLAG)))
                continue;
            var dtSks = this.Sql_GetTehaiSksStatus(dbHelper, cond, ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO));
            if (dtSks.Rows.Count < 1)
            {
                dr.SetField("NONYU_JYOTAI", "");
                dr.SetField("TEHAI_NO", "");
            }
            else
            {
                var enumSks = dtSks.AsEnumerable();
                var fldTehaiStatus = string.Join("+", enumSks.Select(w => ComFunc.GetFld(w, "NONYU_JYOTAI")).ToArray());
                var fldTehaiNo = string.Join("+", enumSks.Select(w => ComFunc.GetFld(w, "TEHAI_NO")).ToArray());
                var fldUkeiredate = enumSks.Select(w => ComFunc.GetFld(w, "UKEIRE_DATE")).ToArray();

                dr.SetField("NONYU_JYOTAI", fldTehaiStatus);
                dr.SetField("TEHAI_NO", fldTehaiNo);
                if (fldUkeiredate.Length > 0)
                {
                    DateTime dTime;
                    if (DateTime.TryParse(fldUkeiredate[0].ToString(), out dTime))
                    {
                        dr.SetField("UKEIRE_DATE", dTime);
                    }
                }
            }
        }
        return dt;
    }
    #endregion

    #region 手配履歴更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配履歴更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>J.Chen 2024/10/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTehaiJohoRireki(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック
            DataTable dtTehaiJohoList = GetAndLockTehaiJohoList(dbHelper, cond, ds.Tables[ComDefine.DTTBL_UPDATE]);
            if ((dtTehaiJohoList == null)
                || (dtTehaiJohoList.Rows.Count == 0)
                || (ds.Tables[ComDefine.DTTBL_UPDATE].Rows.Count != dtTehaiJohoList.Rows.Count))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(ds.Tables[ComDefine.DTTBL_UPDATE], dtTehaiJohoList, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, Def_T_SHUKKA_MEISAI.TAG_NO);
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

            // 更新実行
            this.UpdTehaiRireki(dbHelper, cond, ds.Tables[ComDefine.DTTBL_UPDATE]);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region ヘルパー

    /// --------------------------------------------------
    /// <summary>
    /// Where句に手配連携番号の配列を設定する（SQLパラメータ上限対応：1000件に分割して実行）
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ</param>
    /// <param name="paramCollection">パラメータ配列</param>
    /// <param name="dt">データセット</param>
    /// <param name="tblAlias">納入先マスタテーブルに付与する別名</param>
    /// <create>J.Chen 2024/10/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetupWherePhraseTehaiJoho(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, DataTable dt, string tblAlias)
    {
        sb.ApdL(" (");
        sb.ApdL("   1 = 0 ");

        int i = 0;
        bool isTTMData = false;
        foreach (DataRow dr in dt.Rows)
        {
            if ((tblAlias == "TTM" && Convert.ToInt32(dr["DATA_TYPE"]) != 0) ||
            (tblAlias == "TSM" && Convert.ToInt32(dr["DATA_TYPE"]) != 1))
            {
                continue;
            }

            if (i == 0)
            {
                if (tblAlias == "TTM")
                {
                    sb.ApdN("   OR ");
                    sb.ApdN(tblAlias).ApdN(".");
                    sb.ApdL("TEHAI_RENKEI_NO IN (");
                    sb.ApdN("       ");

                    isTTMData = true;
                }
                else
                {
                    sb.ApdN("  OR ");
                }
            }
            else
            {
                if (i % 10 == 0)
                {
                    sb.ApdL();
                }

                if (i % 1000 == 0)
                {
                    sb.ApdL("   )");
                    sb.ApdN("   OR ");

                    if (tblAlias == "TTM")
                    {
                        sb.ApdN(tblAlias).ApdN(".");
                        sb.ApdL("TEHAI_RENKEI_NO IN (");
                        sb.ApdN("       ");
                    }

                }
                else
                {
                    if (tblAlias == "TTM")
                    {
                        sb.ApdN("     , ");
                    }
                    else if (tblAlias == "TSM")
                    {
                        sb.ApdN("        OR ");
                    }
                }
            }
            
            if (tblAlias == "TTM")
            {
                sb.ApdN(this.BindPrefix).ApdL(tblAlias + "TEHAI_RENKEI_NO" + i);
                paramCollection.Add(iNewParam.NewDbParameter(tblAlias + "TEHAI_RENKEI_NO" + i, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            }
            else if (tblAlias == "TSM")
            {
                sb.ApdN("(");
                sb.ApdN(tblAlias).ApdN(".TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdN(tblAlias + "TEHAI_RENKEI_NO" + i);
                sb.ApdN(" AND ");
                sb.ApdN(tblAlias).ApdN(".TAG_NO = ").ApdN(this.BindPrefix).ApdN(tblAlias + "TAG_NO" + i);
                sb.ApdL(")");

                paramCollection.Add(iNewParam.NewDbParameter(tblAlias + "TEHAI_RENKEI_NO" + i, ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParam.NewDbParameter(tblAlias + "TAG_NO" + i, ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
            }
            i++;
        }
        if (tblAlias == "TTM" && isTTMData)
        {
            sb.ApdL("   )");
        }
        sb.ApdL(" )");
    }
    #endregion

    #endregion

    #endregion

    #region SQL実行

    #region SELECT

    #region 表示データ取得 注意）改修のため使用しなくなった
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/12/13</create>
    /// <update>K.Tsutsumi 2019/03/09 半角SPACE1文字で連結置換すると英語表記で支障が出る</update>
    /// <update>K.Tsutsumi 2019/09/07 改修のため使用しなくなった</update>
    /// --------------------------------------------------
    public DataSet GetTehaiShokai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            const string NONE = "-";

            // SQL文
            // a:一時テーブル、cn:汎用(手配入荷状況)、ca:汎用(手配組立状況)、cr:汎用(TAG登録状況)、cs:汎用(TAG登録状況)
            sb.ApdL("SELECT a.*");
            sb.ApdL("    , IsNull(cn.ITEM_NAME,a.TEHAI_NYUKA_FLAG) AS TEHAI_NYUKA_FLAG_NAME"); //入荷状況
            sb.ApdL("    , IsNull(ca.ITEM_NAME,a.TEHAI_ASSY_FLAG) AS TEHAI_ASSY_FLAG_NAME"); //組立状況
            sb.ApdL("    , IsNull(cr.ITEM_NAME,a.TEHAI_TAG_TOUROKU_FLAG) AS TEHAI_TAG_TOUROKU_FLAG_NAME"); //TAG登録状況
            sb.ApdL("    , IsNull(cs.ITEM_NAME,a.TEHAI_SYUKKA_FLAG) AS TEHAI_SYUKKA_FLAG_NAME"); //TAG登録状況
            sb.ApdL("FROM (");

            // tm:手配明細、e:技連マスタ、p:プロジェクトマスタ、ce:汎用(見積区分)、
            // ct:汎用(手配区分)、cq:汎用(数量単位)、st:出荷明細(全体)、sm:出荷明細(出荷済み)
            sb.ApdL("SELECT ");
            sb.ApdL("      tm.TEHAI_RENKEI_NO");
            sb.ApdL("    , tm.SETTEI_DATE");
            // 納入状態
            sb.ApdL("    , CASE tm.TEHAI_FLAG");
            sb.ApdN("      WHEN '").ApdN(TEHAI_FLAG.ORDERED_VALUE1).ApdL("' THEN"); //発注の場合のみ
            sb.ApdL("        REPLACE(RTRIM("); //副問い合わせでデータを生成する
            sb.ApdL("        (SELECT");
            sb.ApdL("         CASE ");
            sb.ApdN("             WHEN cs.VALUE3 = '").ApdN(ComDefine.SURPPLIES_ORDER_FLAG_VALUE3_COMPLETE).ApdL("' THEN cs.ITEM_NAME"); //SURPPLIES_ORDER_FLAGの値3が完了の場合は完了の名称を表示
            sb.ApdL("             WHEN ISDATE(k.KAITO_DATE) = 1 THEN k.KAITO_DATE"); //SURPPLIES_ORDER_FLAGの値3が完了以外で回答納期が日付形式の場合は回答納期を出力
            sb.ApdL("             ELSE cs.ITEM_NAME"); //上記以外は項目名を出力
            sb.ApdL("           END + '  ' AS TEHAI_STATUS"); //連結のため半角スペース*2を出力
            sb.ApdL("        FROM T_TEHAI_MEISAI_SKS AS r");
            sb.ApdL("          INNER JOIN  T_TEHAI_SKS AS k");
            sb.ApdL("            ON  k.TEHAI_NO = r.TEHAI_NO");
            sb.ApdL("          INNER JOIN M_COMMON AS cs");
            sb.ApdL("            ON  cs.GROUP_CD = 'SURPPLIES_ORDER_FLAG'");  //納入状態
            sb.ApdN("            AND cs.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("            AND cs.VALUE1 = k.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("        WHERE ");
            sb.ApdL("          r.TEHAI_RENKEI_NO = tm.TEHAI_RENKEI_NO");
            sb.ApdL("        ORDER BY k.TEHAI_NO "); //手配番号順に結合する
            sb.ApdL("        FOR XML PATH('') , TYPE).value('.','nvarchar(max)'))"); // SQL Serverの機能を使用して単純に結合する
            sb.ApdL("         , '  ', '+') "); //半角スペース*2で連結されるため、前方TRIMして記号へ置換する
            sb.ApdN("      ELSE '").ApdN(NONE).ApdL("' END AS NONYU_JYOTAI");

            // 入荷状況
            sb.ApdL("    , CASE tm.TEHAI_FLAG");
            sb.ApdN("      WHEN '").ApdN(TEHAI_FLAG.ORDERED_VALUE1).ApdL("' THEN"); //発注の場合のみ
            sb.ApdN("        CASE WHEN tm.HACCHU_QTY <= tm.ARRIVAL_QTY THEN '").ApdN(TEHAI_NYUKA_FLAG.COMP_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.HACCHU_QTY >  tm.ARRIVAL_QTY THEN '").ApdN(TEHAI_NYUKA_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("        ELSE '").ApdN(NONE).ApdL("' END");
            sb.ApdN("      ELSE '").ApdN(NONE).ApdL("' END AS TEHAI_NYUKA_FLAG");


            // 組立状況
            sb.ApdL("    , CASE tm.TEHAI_FLAG");
            sb.ApdN("      WHEN '").ApdN(TEHAI_FLAG.ASSY_VALUE1).ApdL("' THEN");
            sb.ApdN("        CASE WHEN tm.TEHAI_QTY <= tm.ASSY_QTY THEN '").ApdN(TEHAI_ASSY_FLAG.COMP_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.TEHAI_QTY >  tm.ASSY_QTY THEN '").ApdN(TEHAI_ASSY_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("        ELSE '").ApdN(NONE).ApdL("' END");
            sb.ApdN("      ELSE '").ApdN(NONE).ApdL("' END AS TEHAI_ASSY_FLAG");

            // TAG登録状況
            sb.ApdN("    , CASE tm.TEHAI_FLAG WHEN '").ApdN(TEHAI_FLAG.CANCELLED_VALUE1).ApdN("' THEN '").ApdN(NONE).ApdL("' ELSE");
            sb.ApdN("      CASE tm.SHUKKA_QTY WHEN 0 THEN '").ApdN(NONE).ApdL("'");
            sb.ApdL("      ELSE");
            sb.ApdN("        CASE WHEN st.CNT IS NULL THEN '").ApdN(TEHAI_TAG_TOUROKU_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY < st.CNT THEN '").ApdN(TEHAI_TAG_TOUROKU_FLAG.OVER_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY = st.CNT THEN '").ApdN(TEHAI_TAG_TOUROKU_FLAG.COMP_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY > st.CNT THEN '").ApdN(TEHAI_TAG_TOUROKU_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("        ELSE '").ApdN(NONE).ApdL("' END");
            sb.ApdL("      END END AS TEHAI_TAG_TOUROKU_FLAG");

            // 出荷状況
            sb.ApdN("    , CASE tm.TEHAI_FLAG WHEN '").ApdN(TEHAI_FLAG.CANCELLED_VALUE1).ApdN("' THEN '").ApdN(NONE).ApdL("' ELSE");
            sb.ApdN("      CASE tm.SHUKKA_QTY WHEN 0 THEN '").ApdN(NONE).ApdL("'");
            sb.ApdL("      ELSE");
            sb.ApdN("        CASE WHEN sm.CNT IS NULL THEN '").ApdN(TEHAI_SYUKKA_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY < sm.CNT THEN '").ApdN(TEHAI_SYUKKA_FLAG.OVER_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY = sm.CNT THEN '").ApdN(TEHAI_SYUKKA_FLAG.COMP_VALUE1).ApdL("'");
            sb.ApdN("             WHEN tm.SHUKKA_QTY > sm.CNT THEN '").ApdN(TEHAI_SYUKKA_FLAG.PROC_VALUE1).ApdL("'");
            sb.ApdN("        ELSE '").ApdN(NONE).ApdL("' END");
            sb.ApdL("      END END AS TEHAI_SYUKKA_FLAG");

            sb.ApdL("    , p.BUKKEN_NAME");
            sb.ApdL("    , tm.NOUHIN_SAKI");
            sb.ApdL("    , tm.SYUKKA_SAKI");
            sb.ApdL("    , e.SEIBAN");
            sb.ApdL("    , e.CODE");
            sb.ApdL("    , tm.ZUMEN_OIBAN");
            sb.ApdL("    , e.AR_NO");
            sb.ApdL("    , tm.ECS_QUOTA");
            sb.ApdL("    , tm.ECS_NO");
            sb.ApdL("    , tm.FLOOR");
            sb.ApdL("    , e.KISHU");
            sb.ApdL("    , tm.ST_NO");
            sb.ApdL("    , tm.HINMEI_JP");
            sb.ApdL("    , tm.HINMEI");
            sb.ApdL("    , tm.HINMEI_INV");
            sb.ApdL("    , tm.ZUMEN_KEISHIKI");
            sb.ApdL("    , tm.TEHAI_QTY");
            sb.ApdL("    , tm.TEHAI_FLAG");
            sb.ApdL("    , ct.ITEM_NAME AS TEHAI_FLAG_NAME");
            sb.ApdL("    , tm.HACCHU_QTY");
            sb.ApdL("    , tm.ARRIVAL_QTY");
            sb.ApdL("    , tm.ASSY_QTY");
            sb.ApdL("    , tm.SHUKKA_QTY");
            sb.ApdL("    , IsNull(st.CNT, 0) AS TAG_TOROKU_QTY");
            sb.ApdL("    , IsNull(sm.CNT, 0) AS SHUKKA_JISSEKI_QTY");
            sb.ApdL("    , tm.FREE1");
            sb.ApdL("    , tm.FREE2");
            sb.ApdL("    , tm.QUANTITY_UNIT");
            sb.ApdL("    , cq.ITEM_NAME AS QUANTITY_UNIT_NAME");
            sb.ApdL("    , tm.ZUMEN_KEISHIKI2");
            sb.ApdL("    , tm.NOTE");
            sb.ApdL("    , tm.MAKER");
            sb.ApdL("    , tm.UNIT_PRICE");
            sb.ApdL("    , tm.ESTIMATE_FLAG");
            sb.ApdL("    , ce.ITEM_NAME AS ESTIMATE_FLAG_NAME");
            sb.ApdL("    , ce.VALUE3 AS ESTIMATE_COLOR");
            sb.ApdL("FROM ");
            sb.ApdL("    T_TEHAI_MEISAI tm");
            sb.ApdL("  INNER JOIN M_ECS e ON tm.ECS_NO = e.ECS_NO AND tm.ECS_QUOTA = e.ECS_QUOTA");
            sb.ApdL("  LEFT JOIN M_PROJECT p ON e.PROJECT_NO = p.PROJECT_NO");
            sb.ApdL("  INNER JOIN M_COMMON AS ce"); //見積区分
            sb.ApdN("    ON  ce.GROUP_CD = 'ESTIMATE_FLAG'");
            sb.ApdN("    AND ce.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND ce.VALUE1 = tm.ESTIMATE_FLAG");
            sb.ApdL("  INNER JOIN M_COMMON ct"); //手配区分
            sb.ApdN("    ON  ct.GROUP_CD = 'TEHAI_FLAG' ");
            sb.ApdL("    AND ct.VALUE1 = tm.TEHAI_FLAG");
            sb.ApdN("    AND ct.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  INNER JOIN M_COMMON cq"); //数量単位名
            sb.ApdN("    ON  cq.GROUP_CD = 'QUANTITY_UNIT' ");
            sb.ApdL("    AND cq.VALUE1 = tm.QUANTITY_UNIT");
            sb.ApdN("    AND cq.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT OUTER JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI GROUP BY TEHAI_RENKEI_NO) AS st");
            sb.ApdL("    ON st.TEHAI_RENKEI_NO = tm.TEHAI_RENKEI_NO");
            sb.ApdN("  LEFT OUTER JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= '").ApdN(JYOTAI_FLAG.SHUKKAZUMI_VALUE1).ApdL("' GROUP BY TEHAI_RENKEI_NO) AS sm");
            sb.ApdL("    ON sm.TEHAI_RENKEI_NO = tm.TEHAI_RENKEI_NO");
            sb.ApdN("WHERE 1 = 1");

            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                sb.ApdN("   AND e.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
            }
            if (!string.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                sb.ApdN("   AND tm.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", cond.TehaiRenkeiNo));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND e.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND e.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND tm.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND e.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            if (!string.IsNullOrEmpty(cond.Oiban))
            {
                sb.ApdN("   AND tm.ZUMEN_OIBAN = ").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_OIBAN", cond.Oiban));
            }
            if (!string.IsNullOrEmpty(cond.Nouhinsaki))
            {
                sb.ApdN("   AND tm.NOUHIN_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("NOUHIN_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("NOUHIN_SAKI", cond.Nouhinsaki + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Shukkasaki))
            {
                sb.ApdN("   AND tm.SYUKKA_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("SYUKKA_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_SAKI", cond.Shukkasaki + "%"));
            }

            // 手配区分
            if (cond.TehaiKubun == DISP_TEHAI_FLAG.ALL_EXCEPT_CANCEL_VALUE1)
            {
                // キャンセルを除く全て
                sb.ApdN("   AND tm.TEHAI_FLAG <> '").ApdN(TEHAI_FLAG.CANCELLED_VALUE1).ApdL("'");
            }
            else if (!string.IsNullOrEmpty(cond.TehaiKubun) && cond.TehaiKubun != DISP_TEHAI_FLAG.ALL_VALUE1)
            {
                // 全てを除く手配区分
                sb.ApdN("   AND tm.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", cond.TehaiKubun));
            }
            //elseは無し

            // 有償・無償(全てを除く)
            if (!string.IsNullOrEmpty(cond.Yusho) && cond.Yusho != DISP_ESTIMATE_FLAG.ALL_VALUE1)
            {
                sb.ApdN("   AND tm.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", cond.Yusho));
            }
            // 入荷状況
            if (cond.NyukaFlag == TEHAI_NYUKA_FLAG.COMP_VALUE1)
            {
                sb.ApdL("   AND tm.HACCHU_QTY <= tm.ARRIVAL_QTY ");
                // 発注のみ
                sb.ApdN("   AND tm.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));
            }
            else if (cond.NyukaFlag == TEHAI_NYUKA_FLAG.PROC_VALUE1)
            {
                sb.ApdL("   AND tm.HACCHU_QTY > tm.ARRIVAL_QTY ");
                // 発注のみ
                sb.ApdN("   AND tm.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_TEHAI_FLAG", TEHAI_FLAG.ORDERED_VALUE1));
            }
            //elseは無し

            // 組立状況
            if (cond.AssyFlag == TEHAI_ASSY_FLAG.COMP_VALUE1)
            {
                sb.ApdL("   AND tm.TEHAI_QTY <= tm.ASSY_QTY ");
                // 組立のみ
                sb.ApdN("   AND tm.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_TEHAI_FLAG", TEHAI_FLAG.ASSY_VALUE1));
            }
            else if (cond.AssyFlag == TEHAI_ASSY_FLAG.PROC_VALUE1)
            {
                sb.ApdL("   AND tm.TEHAI_QTY > tm.ASSY_QTY ");
                // 組立のみ
                sb.ApdN("   AND tm.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_TEHAI_FLAG", TEHAI_FLAG.ASSY_VALUE1));
            }
            //elseは無し

            //TAG登録状況
            if (cond.TagTourokuFlag == TEHAI_TAG_TOUROKU_FLAG.COMP_VALUE1)
            {
                sb.ApdL("   AND tm.SHUKKA_QTY = st.CNT ");
            }
            else if (cond.TagTourokuFlag == TEHAI_TAG_TOUROKU_FLAG.OVER_VALUE1)
            {
                sb.ApdL("   AND tm.SHUKKA_QTY < st.CNT ");
            }
            else if (cond.TagTourokuFlag == TEHAI_TAG_TOUROKU_FLAG.PROC_VALUE1)
            {
                //出荷明細データがなく、出荷数があるものも対象にする
                sb.ApdL("   AND (tm.SHUKKA_QTY > st.CNT OR (tm.SHUKKA_QTY <> 0 AND st.CNT IS NULL)) ");
            }
            //elseは無し

            //出荷状況
            if (cond.ShukkaFlag == TEHAI_SYUKKA_FLAG.COMP_VALUE1)
            {
                sb.ApdL("   AND tm.SHUKKA_QTY = sm.CNT ");
            }
            else if (cond.ShukkaFlag == TEHAI_SYUKKA_FLAG.OVER_VALUE1)
            {
                sb.ApdL("   AND tm.SHUKKA_QTY < sm.CNT ");
            }
            else if (cond.ShukkaFlag == TEHAI_SYUKKA_FLAG.PROC_VALUE1)
            {
                //出荷明細データがなく、出荷数があるものも対象にする
                sb.ApdL("   AND (tm.SHUKKA_QTY > sm.CNT OR (tm.SHUKKA_QTY <> 0 AND sm.CNT IS NULL)) ");
            }
            //elseは無し

            sb.ApdL(") AS a ");
            sb.ApdL("  LEFT JOIN M_COMMON cn"); //手配入荷状況
            sb.ApdN("    ON  cn.GROUP_CD = 'TEHAI_NYUKA_FLAG' ");
            sb.ApdL("    AND cn.VALUE1 = a.TEHAI_NYUKA_FLAG");
            sb.ApdN("    AND cn.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON ca"); //手配組立状況
            sb.ApdL("    ON  ca.GROUP_CD = 'TEHAI_ASSY_FLAG' ");
            sb.ApdL("    AND ca.VALUE1 = a.TEHAI_ASSY_FLAG");
            sb.ApdN("    AND ca.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON cr"); //TAG登録状況
            sb.ApdL("    ON  cr.GROUP_CD = 'TEHAI_TAG_TOUROKU_FLAG' ");
            sb.ApdL("    AND cr.VALUE1 = a.TEHAI_TAG_TOUROKU_FLAG");
            sb.ApdN("    AND cr.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON cs"); //TAG登録状況
            sb.ApdL("    ON  cs.GROUP_CD = 'TEHAI_SYUKKA_FLAG' ");
            sb.ApdL("    AND cs.VALUE1 = a.TEHAI_SYUKKA_FLAG");
            sb.ApdN("    AND cs.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("ORDER BY a.TEHAI_RENKEI_NO");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));


            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEHAI_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 表示データ取得(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(手配情報照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/29</create>
    /// <update>D.Okumura 2019/12/11 手配情報照会背景色対応</update>
    /// <update>D.Okumura 2020/01/06 AssyParts対応</update>
    /// <update>D.Okumura 2020/07/03 EFA_SMS-122 SQLタイムアウト改善 サブクエリ(T_SHUKKA_MEISAI)に手配NOの条件を追加</update>
    /// <update>K.Tsutsumi 2020/07/13 EFA_SMS-123 出荷日に集荷日が表示されている</update>
    /// <update>D.Okumura 2020/10/20 EFA_SMS-148 AR(未出荷)のときAssyにぶら下がる部品が表示されない問題を修正</update>
    /// <update>D.Okumura 2021/07/29 EFA_SMS-210 納入状態・手配Noの結合を別SQLで実行するように変更</update>
    /// <update>R.Sumi 2022/02/25 STEP14 物件名に「全て」追加対応</update>
    /// <update>R.sumi 2022/03/16 出荷数を参照する為にSELECT句に出荷数を追加</update>
    /// <update>Y.Shioshi 2022/05/09 STEP14 登録日時・登録者取得対応</update>
    /// <update>J.Chen 2022/05/18 STEP14</update>
    /// <update>T.Zhou 2023/12/07 検索条件追加</update>
    /// <update>J.Chen 2024/10/28 変更履歴追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    internal DataTable Sql_GetTehaiJohoShokai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            const string NONE = "-";

            // SQL文
            sb.ApdL("SELECT MAIN.*");
            sb.ApdL("     , IsNull(COM10.ITEM_NAME, MAIN.TEHAI_NYUKA_FLAG) AS TEHAI_NYUKA_FLAG_NAME"); //入荷状況
            sb.ApdL("     , IsNull(COM11.ITEM_NAME, MAIN.TEHAI_ASSY_FLAG) AS TEHAI_ASSY_FLAG_NAME"); //組立状況
            sb.ApdL("     , IsNull(COM12.ITEM_NAME, MAIN.TEHAI_TAG_TOUROKU_FLAG) AS TEHAI_TAG_TOUROKU_FLAG_NAME"); //TAG登録状況
            sb.ApdL("     , IsNull(COM13.ITEM_NAME, MAIN.TEHAI_SYUKKA_FLAG) AS TEHAI_SYUKKA_FLAG_NAME"); //TAG登録状況

            //【Step_1_1】手配情報照会画面の権限制御 2022/10/17（TW-Tsuji）
            //　単価（JPY）のブランク列を追加.
            sb.ApdL("     , '' AS UNIT_PRICE_BLANK");

            sb.ApdL("  FROM (");
            sb.ApdL("SELECT");
            sb.ApdL("       CAST('0' AS NCHAR(1)) AS DATA_TYPE");
            sb.ApdL("     , TTM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TTE.PO_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , TTM.SETTEI_DATE");
            // -- 納入状態
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS NONYU_JYOTAI");
            sb.ApdL("     , TTM.NOUHIN_SAKI");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , ME.SEIBAN");
            sb.ApdL("     , ME.CODE");
            // -- SHIP OR AR_NO
            sb.ApdL("     , CASE");
            sb.ApdL("           WHEN (ME.AR_NO IS NULL OR LTRIM(RTRIM(ME.AR_NO)) = '') THEN CAST(NULL AS NVARCHAR(10))");
            sb.ApdL("           ELSE ME.AR_NO");
            sb.ApdL("       END AS SHIP_AR_NO");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , TTM.ZUMEN_OIBAN");
            sb.ApdL("     , TTM.FLOOR");
            sb.ApdL("     , ME.KISHU");
            sb.ApdL("     , TTM.ST_NO");
            sb.ApdL("     , TTM.HINMEI_JP");
            sb.ApdL("     , TTM.HINMEI");
            sb.ApdL("     , TTM.HINMEI_INV");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(8)) AS KUWARI_NO");
            sb.ApdL("     , TTM.TEHAI_QTY - IsNull(WSM1.CNT, 0) AS TEHAI_QTY");
            sb.ApdL("     , TTM.FREE1");
            sb.ApdL("     , TTM.FREE2");
            sb.ApdL("     , CAST(NULL AS DECIMAL(7,2)) AS GRWT");
            sb.ApdL("     , MC3.ITEM_NAME AS QUANTITY_UNIT_NAME");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TTM.NOTE");
            sb.ApdL("     , TTM.NOTE2");
            sb.ApdL("     , TTM.NOTE3");
            sb.ApdL("     , TTM.CUSTOMS_STATUS");
            sb.ApdL("     , TTM.MAKER");
            sb.ApdL("     , TTM.UNIT_PRICE");
            sb.ApdL("     , TTM.SHUKKA_QTY");
            // -- 手配No.
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS TEHAI_NO");
            sb.ApdL("     , MC2.ITEM_NAME AS TEHAI_FLAG_NAME");
            //  -- 入荷状況
            //sb.ApdL("     , CASE TTM.TEHAI_FLAG");
            //sb.ApdN("       WHEN ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_ORDERED").ApdL(" THEN");
            sb.ApdL("     , CASE WHEN");
            sb.ApdN("       TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ORDERED");
            sb.ApdN("       OR TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdN("       OR TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_SURPLUS").ApdL(" THEN");
            sb.ApdN("         CASE WHEN TTM.HACCHU_QTY <= TTM.ARRIVAL_QTY THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_COMP");
            sb.ApdN("              WHEN TTM.HACCHU_QTY >  TTM.ARRIVAL_QTY THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_PROC");
            sb.ApdN("         ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END");
            sb.ApdN("       ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END AS TEHAI_NYUKA_FLAG");
            //  -- 組立状況
            sb.ApdL("     , CASE TTM.TEHAI_FLAG");
            sb.ApdN("       WHEN ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_ASSY").ApdL(" THEN");
            sb.ApdN("         CASE WHEN TTM.TEHAI_QTY <= TTM.ASSY_QTY THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_ASSY_FLAG_COMP");
            sb.ApdN("              WHEN TTM.TEHAI_QTY >  TTM.ASSY_QTY THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_ASSY_FLAG_PROC");
            sb.ApdN("         ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END ");
            sb.ApdN("       ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END AS TEHAI_ASSY_FLAG");
            // -- TAG登録
            sb.ApdL("     , CASE TTM.TEHAI_FLAG");
            sb.ApdN("         WHEN ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_CANCELLED").ApdN(" THEN ").ApdN(this.BindPrefix).ApdL("NO_DATA");
            sb.ApdL("       ELSE");
            sb.ApdL("         CASE TTM.SHUKKA_QTY");
            sb.ApdN("           WHEN 0 THEN ").ApdN(this.BindPrefix).ApdL("NO_DATA");
            sb.ApdL("         ELSE");
            sb.ApdL("           CASE ");
            sb.ApdN("             WHEN WSM1.CNT IS NULL THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_TAG_TOUROKU_FLAG_PROC");
            sb.ApdN("             WHEN TTM.SHUKKA_QTY < WSM1.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_TAG_TOUROKU_FLAG_OVER");
            sb.ApdN("             WHEN TTM.SHUKKA_QTY = WSM1.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_TAG_TOUROKU_FLAG_COMP");
            sb.ApdN("             WHEN TTM.SHUKKA_QTY > WSM1.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_TAG_TOUROKU_FLAG_PROC");
            sb.ApdN("           ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END END END AS TEHAI_TAG_TOUROKU_FLAG");
            //  -- 出荷状況
            sb.ApdL("     , CASE TTM.TEHAI_FLAG");
            sb.ApdN("         WHEN  ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_CANCELLED").ApdN(" THEN ").ApdN(this.BindPrefix).ApdL("NO_DATA");
            sb.ApdL("       ELSE");
            sb.ApdL("         CASE TTM.SHUKKA_QTY ");
            sb.ApdN("           WHEN 0 THEN ").ApdN(this.BindPrefix).ApdL("NO_DATA");
            sb.ApdL("         ELSE");
            sb.ApdL("           CASE");
            sb.ApdN("              WHEN WSM2.CNT IS NULL THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_SYUKKA_FLAG_PROC");
            sb.ApdN("              WHEN TTM.SHUKKA_QTY < WSM2.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_SYUKKA_FLAG_OVER");
            sb.ApdN("              WHEN TTM.SHUKKA_QTY = WSM2.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_SYUKKA_FLAG_COMP");
            sb.ApdN("              WHEN TTM.SHUKKA_QTY > WSM2.CNT THEN ").ApdN(this.BindPrefix).ApdL("TEHAI_SYUKKA_FLAG_PROC");
            sb.ApdN("           ELSE ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" END END END AS TEHAI_SYUKKA_FLAG");
            sb.ApdL("     , MC1.ITEM_NAME AS ESTIMATE_FLAG_NAME");
            // -- 行背景色
            sb.ApdL("     , CASE");
            sb.ApdN("         WHEN TTM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY AND (TTM.ASSY_NO <>'' AND TTM.ASSY_NO IS NOT NULL) THEN MC6.VALUE3");
            sb.ApdL("         ELSE MC1.VALUE3");
            sb.ApdL("       END AS ESTIMATE_COLOR");
            sb.ApdL("     , CASE");
            sb.ApdL("         WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI");
            sb.ApdL("               OR MZK.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdN("        )THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdN("NOT_EXISTS_VALUE").ApdL(" END AS EXISTS_PICTURE");
            sb.ApdL("     , MZK1.FILE_NAME AS FILE_NAME1");
            sb.ApdL("     , MZK1.SAVE_DIR AS SAVE_DIR1");
            sb.ApdL("     , MZK2.FILE_NAME AS FILE_NAME2");
            sb.ApdL("     , MZK2.SAVE_DIR AS SAVE_DIR2");
            sb.ApdL("     , CAST(NULL AS NCHAR(1)) AS JYOTAI_FLAG");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(256)) AS JYOTAI_NAME");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS SHUKKA_DATE");
            sb.ApdL("     , CAST(NULL AS NCHAR(5)) AS TAG_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(15)) AS AREA");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(16)) AS M_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS SHUKA_DATE");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS BOX_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS BOXKONPO_DATE");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS PALLETKONPO_DATE");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS KIWAKU_NO");
            sb.ApdL("     , CAST(NULL AS DECIMAL(3)) AS CASE_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS KIWAKUKONPO_DATE");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS UNSOKAISHA_NAME");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(15)) AS INVOICE_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(15)) AS OKURIJYO_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(15)) AS BL_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS UKEIRE_DATE");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(40)) AS UKEIRE_USER_NAME");
            sb.ApdL("     , ME.ECS_QUOTA");
            sb.ApdL("     , TTM.CREATE_USER_NAME");
            sb.ApdL("     , TTM.CREATE_DATE");
            sb.ApdL("     , TTM.TEHAI_FLAG");
            sb.ApdL("     , TTM.ASSY_NO");
            sb.ApdL("     , TTM.ESTIMATE_FLAG");
            sb.ApdL("     , CAST(NULL AS NCHAR(10)) AS SHIP");
            sb.ApdL("     , CASE");
            sb.ApdL("           WHEN (ME.AR_NO IS NULL OR LTRIM(RTRIM(ME.AR_NO)) = '') THEN CAST(NULL AS NVARCHAR(10))");
            sb.ApdL("           ELSE ME.AR_NO");
            sb.ApdL("       END AS AR_NO");
            sb.ApdL("     , CASE ");
            sb.ApdN("           WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ORDERED");
            sb.ApdL("             THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("           WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY");
            sb.ApdL("             THEN TTM.ASSY_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("           WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("             THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("           WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SURPLUS");
            sb.ApdL("             THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdL("           ELSE 0");
            sb.ApdL("       END AS TAG_TOUROKU_MAX");
            sb.ApdL("     , MC7.VALUE3 AS COLOR02");
            sb.ApdL("     , MC10.VALUE3 AS COLOR04");
            sb.ApdL("     , TTM.HENKYAKUHIN_FLAG");
            sb.ApdL("     , LTRIM(MC9.ITEM_NAME) AS HENKYAKUHIN_FLAG_NAME");
            sb.ApdN("     , CASE WHEN IsNull(TTM.ASSY_NO, '') = '' OR TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_ASSY").ApdL(" THEN NULL");
            sb.ApdL("            ELSE IsNull(TTMASSY.TEHAI_QTY - TTMASSY.ASSY_QTY, 0)");
            sb.ApdL("       END AS ASSY_OYA_ZAN");
            sb.ApdL("     , TTM.DISP_NO");
            sb.ApdL("     , TTM.TEHAI_RIREKI");
            sb.ApdL("     , CONVERT(NCHAR(27), TTM.VERSION, 121) AS VERSION");
            sb.ApdL("");
            sb.ApdL("  FROM ");
            sb.ApdL("        T_TEHAI_MEISAI TTM");
            sb.ApdL(" INNER JOIN M_ECS ME ON TTM.ECS_NO = ME.ECS_NO AND TTM.ECS_QUOTA = ME.ECS_QUOTA");
            if (!string.IsNullOrEmpty(cond.EcsQuota))
            {
                sb.ApdN("   AND ME.ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");

            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND ME.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            }
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                sb.ApdN("   AND TTM.HINMEI LIKE ").ApdN(this.BindPrefix).ApdL("HINMEI");
            }
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                sb.ApdN("   AND TTM.ZUMEN_KEISHIKI LIKE ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            }
            if (!string.IsNullOrEmpty(cond.CreateDateStart))
            {
                sb.ApdN("   AND TTM.CREATE_DATE >=  ").ApdN(this.BindPrefix).ApdL("CREATE_DATE_START");
            }
            if (!string.IsNullOrEmpty(cond.CreateDateEnd))
            {
                sb.ApdN("   AND TTM.CREATE_DATE <= ").ApdN(this.BindPrefix).ApdL("CREATE_DATE_END");
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateStart))
            {
                sb.ApdN("   AND TTM.SETTEI_DATE >= ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE_START");
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateEnd))
            {
                sb.ApdN("   AND TTM.SETTEI_DATE <= ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE_END");
            }
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    sb.ApdL("   AND (ME.AR_NO IS NULL OR LTRIM(RTRIM(ME.AR_NO)) = '')");
                }
                else if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    sb.ApdL("   AND (ME.AR_NO IS NOT NULL AND LTRIM(RTRIM(ME.AR_NO)) != '')");
                }
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND ME.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND ME.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND ME.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
            }
            sb.ApdL(" INNER JOIN M_PROJECT MP ON ME.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdN("  LEFT JOIN M_COMMON MC1 ON  MC1.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GROUP_CD")
                                     .ApdN(" AND MC1.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC1.VALUE1 = TTM.ESTIMATE_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MC2 ON  MC2.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_GROUP_CD")
                                     .ApdN(" AND MC2.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC2.VALUE1 = TTM.TEHAI_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MC3 ON MC3.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("QUANTITY_UNIT_GROUP_CD")
                                     .ApdN(" AND MC3.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC3.VALUE1 = TTM.QUANTITY_UNIT");
            sb.ApdN("  LEFT JOIN M_COMMON MC9 ON  MC9.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("HENKYAKUHIN_FLAG_GROUP_CD")
                                     .ApdN(" AND MC9.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC9.VALUE1 = TTM.HENKYAKUHIN_FLAG");
            //sb.ApdL("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI GROUP BY TEHAI_RENKEI_NO) AS WSM1 ON WSM1.TEHAI_RENKEI_NO = TTM.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE TEHAI_RENKEI_NO IS NOT NULL GROUP BY TEHAI_RENKEI_NO) AS WSM1 ON WSM1.TEHAI_RENKEI_NO = TTM.TEHAI_RENKEI_NO");
            //sb.ApdN("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" GROUP BY TEHAI_RENKEI_NO) AS WSM2")
            sb.ApdN("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" AND  TEHAI_RENKEI_NO IS NOT NULL GROUP BY TEHAI_RENKEI_NO) AS WSM2")
                      .ApdL(" ON WSM2.TEHAI_RENKEI_NO = TTM.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK1 ON MZK1.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK2 ON MZK2.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdN("  LEFT JOIN M_COMMON MC6 ON MC6.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC6.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC6.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR01");
            sb.ApdN("  LEFT JOIN M_COMMON MC7 ON MC7.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC7.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC7.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR02");
            sb.ApdN("  LEFT JOIN M_COMMON MC8 ON MC8.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC8.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC8.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR03");
            sb.ApdN("  LEFT JOIN M_COMMON MC10 ON MC10.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC10.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC10.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR04");
            sb.ApdN("  LEFT JOIN T_TEHAI_MEISAI TTMASSY") // 親ASSYを取得
                                    .ApdN("  ON TTMASSY.ECS_QUOTA = TTM.ECS_QUOTA")
                                    .ApdN(" AND TTMASSY.ECS_NO = TTM.ECS_NO")
                                    .ApdN(" AND TTMASSY.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_ASSY")
                                    .ApdN(" AND TTMASSY.ASSY_NO = TTM.ASSY_NO")
                                    .ApdN(" AND TTM.ASSY_NO IS NOT NULL")
                                    .ApdL();
            // 親ASSYの出荷状態を取得
            sb.ApdN("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI").ApdN(" WHERE JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" AND  TEHAI_RENKEI_NO IS NOT NULL GROUP BY TEHAI_RENKEI_NO) AS WSM3")
                      .ApdL(" ON WSM3.TEHAI_RENKEI_NO = TTMASSY.TEHAI_RENKEI_NO");
            sb.ApdL("   LEFT JOIN T_TEHAI_ESTIMATE TTE ON TTM.ESTIMATE_NO = TTE.ESTIMATE_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       TTM.TEHAI_QTY - IsNull(WSM1.CNT, 0) > 0 ");
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL("   AND (");
                    sb.ApdL("        (TTMASSY.TEHAI_RENKEI_NO IS     NULL AND TTM.SHUKKA_QTY - IsNull(WSM2.CNT, 0) > 0)");
                    sb.ApdL("     OR (TTMASSY.TEHAI_RENKEI_NO IS NOT NULL AND TTMASSY.SHUKKA_QTY - IsNull(WSM3.CNT, 0) > 0)");
                    sb.ApdL("       )");
                }
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_AR_VALUE)
                {
                    // NOP
                }
                // STEP14_2_11 2022/02/25 鷲見追加部分 
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_VALUE)
                {
                    // NOP
                }
                else
                {
                    sb.ApdN("   AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                }
            }
            sb.ApdL("");
            sb.ApdL(" UNION ALL");
            sb.ApdL("");
            sb.ApdL("SELECT");
            sb.ApdL("       CAST('1' AS NCHAR(1)) AS DATA_TYPE");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TTE.PO_NO AS PO_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , TTM.SETTEI_DATE");
            // -- 納入状態
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS NONYU_JYOTAI");
            sb.ApdL("     , TTM.NOUHIN_SAKI");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            //  -- SHIP OR AR_NO
            sb.ApdL("     , CASE");
            sb.ApdN("           WHEN TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG_NORMAL").ApdL(" THEN MN.SHIP");
            sb.ApdL("           ELSE TSM.AR_NO");
            sb.ApdL("       END AS SHIP_AR_NO");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.FLOOR");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("     , TSM.HINMEI");
            sb.ApdL("     , TTM.HINMEI_INV");
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            sb.ApdL("     , TSM.NUM AS TEHAI_QTY");
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , MC3.ITEM_NAME AS QUANTITY_UNIT_NAME");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TSM.BIKO AS NOTE");
            sb.ApdL("     , TTM.NOTE2");
            sb.ApdL("     , TTM.NOTE3");
            sb.ApdL("     , TTM.CUSTOMS_STATUS");
            sb.ApdL("     , TTM.MAKER");
            sb.ApdL("     , TTM.UNIT_PRICE");
            sb.ApdL("     , TTM.SHUKKA_QTY");
            // -- 手配No.
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS TEHAI_NO");
            sb.ApdL("     , MC2.ITEM_NAME AS TEHAI_FLAG_NAME");
            sb.ApdN("     , CAST(").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS VARCHAR(256)) AS TEHAI_NYUKA_FLAG");
            sb.ApdN("     , CAST(").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS VARCHAR(256)) AS TEHAI_ASSY_FLAG");
            sb.ApdN("     , CAST(").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS VARCHAR(256)) AS TEHAI_TAG_TOUROKU_FLAG");
            sb.ApdN("     , CAST(").ApdN(this.BindPrefix).ApdN("NO_DATA").ApdL(" AS VARCHAR(256)) AS TEHAI_SYUKKA_FLAG");
            sb.ApdL("     , MC1.ITEM_NAME AS ESTIMATE_FLAG_NAME");
            // -- 行背景色
            sb.ApdL("     , CASE");
            sb.ApdN("         WHEN TSM.JYOTAI_FLAG >= ").ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI AND TSM.JYOTAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG_HIKIWATASHIZUMI THEN MC8.VALUE3");
            sb.ApdN("         WHEN TTM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY AND (TTM.ASSY_NO <>'' AND TTM.ASSY_NO IS NOT NULL) THEN MC6.VALUE3");
            sb.ApdL("         ELSE MC1.VALUE3");
            sb.ApdL("       END AS ESTIMATE_COLOR");
            sb.ApdL("     , CASE");
            sb.ApdL("         WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdL("               OR MZK.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdN("        )THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdN("NOT_EXISTS_VALUE").ApdL(" END AS EXISTS_PICTURE");
            sb.ApdL("     , MZK1.FILE_NAME AS FILE_NAME1");
            sb.ApdL("     , MZK1.SAVE_DIR AS SAVE_DIR1");
            sb.ApdL("     , MZK2.FILE_NAME AS FILE_NAME2");
            sb.ApdL("     , MZK2.SAVE_DIR AS SAVE_DIR2");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , MC5.ITEM_NAME AS JYOTAI_NAME");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AREA");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            sb.ApdL("     , RTRIM(TK.SHIP) + '-' + CAST(TKM.CASE_NO AS VARCHAR) AS KIWAKU_NO");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            sb.ApdL("     , TSM.INVOICE_NO");
            sb.ApdL("     , TSM.OKURIJYO_NO");
            sb.ApdL("     , TSM.BL_NO");
            sb.ApdL("     , TSM.UKEIRE_DATE");
            sb.ApdL("     , TSM.UKEIRE_USER_NAME");
            sb.ApdL("     , TTM.ECS_QUOTA");
            sb.ApdL("     , TTM.CREATE_USER_NAME");
            sb.ApdL("     , TTM.CREATE_DATE");
            sb.ApdL("     , TTM.TEHAI_FLAG");
            sb.ApdL("     , TTM.ASSY_NO");
            sb.ApdL("     , TTM.ESTIMATE_FLAG");
            sb.ApdN("     , CASE TSM.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL THEN MN.SHIP ELSE CAST(NULL AS NCHAR(10)) END AS SHIP");
            sb.ApdN("     , CASE TSM.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE CAST(NULL AS NVARCHAR(6)) END AS AR_NO");
            //sb.ApdL("     , MC7.VALUE3 AS COLOR02");
            sb.ApdL("     , 0 AS TAG_TOUROKU_MAX");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(40)) AS COLOR02");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(40)) AS COLOR04");
            sb.ApdL("     , TTM.HENKYAKUHIN_FLAG");
            sb.ApdL("     , LTRIM(MC9.ITEM_NAME) AS HENKYAKUHIN_FLAG_NAME");
            sb.ApdL("     , CAST(NULL AS decimal(6,0)) AS ASSY_OYA_ZAN");
            sb.ApdL("     , TTM.DISP_NO");
            sb.ApdL("     , TSM.TEHAI_RIREKI");
            sb.ApdL("     , CONVERT(NCHAR(27), TSM.VERSION, 121) AS VERSION");
            sb.ApdL("");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" INNER JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL(" INNER JOIN M_ECS ME ON ME.ECS_NO = TTM.ECS_NO AND ME.ECS_QUOTA = TTM.ECS_QUOTA");
            sb.ApdL(" INNER JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdN("  LEFT JOIN M_COMMON MC1 ON  MC1.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GROUP_CD")
                                     .ApdN(" AND MC1.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC1.VALUE1 = TTM.ESTIMATE_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MC2 ON  MC2.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_GROUP_CD")
                                     .ApdN(" AND MC2.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC2.VALUE1 = TTM.TEHAI_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MC3 ON MC3.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("QUANTITY_UNIT_GROUP_CD")
                                     .ApdN(" AND MC3.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC3.VALUE1 = TTM.QUANTITY_UNIT");
            sb.ApdN("  LEFT JOIN M_COMMON MC5 ON MC5.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("DISP_JYOTAI_FLAG_GROUP_CD")
                                     .ApdN(" AND MC5.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC5.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MC9 ON  MC9.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("HENKYAKUHIN_FLAG_GROUP_CD")
                                     .ApdN(" AND MC9.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC9.VALUE1 = TTM.HENKYAKUHIN_FLAG");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK1 ON MZK1.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdL("  LEFT JOIN T_MANAGE_ZUMEN_KEISHIKI MZK2 ON MZK2.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdL("  LEFT JOIN T_KIWAKU TK ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.KOJI_NO = TSM.KOJI_NO AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdN("  LEFT JOIN M_COMMON MC6 ON MC6.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC6.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC6.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR01");
            sb.ApdN("  LEFT JOIN M_COMMON MC7 ON MC7.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC7.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC7.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR02");
            sb.ApdN("  LEFT JOIN M_COMMON MC8 ON MC8.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_COLOR_GROUP_CD AND MC8.LANG = ").ApdN(this.BindPrefix).ApdN("LANG AND MC8.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_COLOR_ITEM_CD_COLOR03");
            sb.ApdL("  LEFT JOIN T_TEHAI_ESTIMATE TTE ON TTM.ESTIMATE_NO = TTE.ESTIMATE_NO");

            
            sb.ApdL(" WHERE (TSM.TEHAI_RENKEI_NO IS NOT NULL AND LTRIM(RTRIM(TSM.TEHAI_RENKEI_NO)) != '')");
            if (!string.IsNullOrEmpty(cond.EcsQuota))
            {
                sb.ApdN("   AND ME.ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND ME.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            }
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                sb.ApdN("   AND TSM.HINMEI LIKE ").ApdN(this.BindPrefix).ApdL("HINMEI");
            }
            if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                sb.ApdN("   AND TSM.ZUMEN_KEISHIKI LIKE ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            }
            if (!string.IsNullOrEmpty(cond.CreateDateStart))
            {
                sb.ApdN("   AND TTM.CREATE_DATE >=  ").ApdN(this.BindPrefix).ApdL("CREATE_DATE_START");
            }
            if (!string.IsNullOrEmpty(cond.CreateDateEnd))
            {
                sb.ApdN("   AND TTM.CREATE_DATE <= ").ApdN(this.BindPrefix).ApdL("CREATE_DATE_END");
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateStart))
            {
                sb.ApdN("   AND TTM.SETTEI_DATE >= ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE_START");
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateEnd))
            {
                sb.ApdN("   AND TTM.SETTEI_DATE <= ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE_END");
            }
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            }
            if (cond.NonyusakiCDs != null)
            {
                string value = string.Empty;
                value += "(";
                value += this.ConvArrayToString(",", cond.NonyusakiCDs);
                value += ")";
                sb.ApdN("   AND MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_NORMAL");
                sb.ApdN("   AND ").ApdN("MN.NONYUSAKI_CD").ApdN(" IN ").ApdL(value);
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND TSM.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND TSM.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND TSM.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
            }
            if (!string.IsNullOrEmpty(cond.TagNo))
            {
                sb.ApdN("   AND TSM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            }
            if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                sb.ApdN("   AND TSM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            }
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                sb.ApdN("   AND TSM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            }
            // 表示選択
            if (cond.DispSelect != null)
            {
                if (cond.DispSelect != DISP_SELECT.ALL_VALUE1)
                {
                    // 全て以外
                    string expression = string.Empty;
                    string value = string.Empty;
                    if (cond.DispSelect == DISP_SELECT.SHUKAZUMI_VALUE1)
                    {
                        // 集荷済
                        expression = " = ";
                        value = UtilConvert.PutQuot(JYOTAI_FLAG.SHUKAZUMI_VALUE1);
                    }
                    else if (cond.DispSelect == DISP_SELECT.BOXZUMI_VALUE1)
                    {
                        // B梱包済
                        expression = " = ";
                        value = UtilConvert.PutQuot(JYOTAI_FLAG.BOXZUMI_VALUE1);
                    }
                    else if (cond.DispSelect == DISP_SELECT.PALLETZUMI_VALUE1)
                    {
                        // P梱包済
                        expression = " = ";
                        value = UtilConvert.PutQuot(JYOTAI_FLAG.PALLETZUMI_VALUE1);
                    }
                    else if (cond.DispSelect == DISP_SELECT.KIWAKUKONPO_VALUE1)
                    {
                        // 木枠梱包済
                        expression = " = ";
                        value = UtilConvert.PutQuot(JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
                    }
                    else
                    {
                        expression = " IN ";
                        if (cond.DispSelect == DISP_SELECT.MISHUKA_VALUE1)
                        {
                            // 未集荷
                            value += "(";
                            value += UtilConvert.PutQuot(JYOTAI_FLAG.SHINKI_VALUE1);
                            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1);
                            value += ")";
                        }
                        else if (cond.DispSelect == DISP_SELECT.KONPOZUMI_VALUE1)
                        {
                            // 梱包済
                            value += "(";
                            value += UtilConvert.PutQuot(JYOTAI_FLAG.BOXZUMI_VALUE1);
                            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.PALLETZUMI_VALUE1);
                            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
                            value += ")";
                        }
                        else
                        {
                            // 出荷済
                            value += "(";
                            value += UtilConvert.PutQuot(JYOTAI_FLAG.SHUKKAZUMI_VALUE1);
                            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.UKEIREZUMI_VALUE1);
                            value += ")";
                        }
                    }
                    string fieldPrefix = "TSM.";
                    string fieldName = "JYOTAI_FLAG";
                    sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(expression).ApdL(value);
                }
            }
            if (!string.IsNullOrEmpty(cond.KiwakuShip))
            {
                sb.ApdN("   AND TK.SHIP = ").ApdN(this.BindPrefix).ApdL("KIWAKU_SHIP");
            }
            if (!string.IsNullOrEmpty(cond.CaseNo))
            {
                sb.ApdN("   AND TKM.CASE_NO = ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            }
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdN("   AND TSM.JYOTAI_FLAG NOT IN (").ApdN(this.BindPrefix).ApdN("SHUKKA").ApdN(" ,").ApdN(this.BindPrefix).ApdN("UKEIRE").ApdL(")");
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                    paramCollection.Add(iNewParam.NewDbParameter("UKEIRE", JYOTAI_FLAG.UKEIREZUMI_VALUE1));
                }
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_AR_VALUE)
                {
                    // NOP
                }
                // STEP14_2_11 2022/02/25 鷲見追加部分 
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_VALUE)
                {
                    // NOP
                }
                else
                {
                    sb.ApdN("   AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                }
            }
            sb.ApdL("  ) AS MAIN");
            sb.ApdL("  LEFT JOIN M_COMMON COM10"); //手配入荷状況
            sb.ApdN("    ON COM10.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_NYUKA_FLAG_GROUP_CD");
            sb.ApdL("   AND COM10.VALUE1 = MAIN.TEHAI_NYUKA_FLAG");
            sb.ApdN("   AND COM10.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM11"); //手配組立状況
            sb.ApdN("    ON COM11.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_ASSY_FLAG_GROUP_CD");
            sb.ApdL("   AND COM11.VALUE1 = MAIN.TEHAI_ASSY_FLAG");
            sb.ApdN("   AND COM11.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM12"); //TAG登録状況
            sb.ApdN("    ON COM12.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_TAG_TOUROKU_FLAG_GROUP_CD");
            sb.ApdL("   AND COM12.VALUE1 = MAIN.TEHAI_TAG_TOUROKU_FLAG");
            sb.ApdN("   AND COM12.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM13"); //TAG登録状況
            sb.ApdN("    ON COM13.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("TEHAI_SYUKKA_FLAG_GROUP_CD");
            sb.ApdL("   AND COM13.VALUE1 = MAIN.TEHAI_SYUKKA_FLAG");
            sb.ApdN("   AND COM13.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("ORDER BY");
            sb.ApdL("       MAIN.ECS_QUOTA");
            sb.ApdL("        ,MAIN.ECS_NO");
            sb.ApdL("        ,MAIN.DISP_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_KENSHU_KANNOU", HACCHU_FLAG.STATUS_2_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("NO_DATA", NONE));
            paramCollection.Add(iNewParam.NewDbParameter("SURPPLIES_ORDER_FLAG_GROUP_CD", SURPPLIES_ORDER_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("QUANTITY_UNIT_GROUP_CD", QUANTITY_UNIT.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_GROUP_CD", TEHAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GROUP_CD", ESTIMATE_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("DISP_JYOTAI_FLAG_GROUP_CD", DISP_JYOTAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_ORDERED", TEHAI_FLAG.ORDERED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_ASSY", TEHAI_FLAG.ASSY_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SKS_SKIP", TEHAI_FLAG.SKS_SKIP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SURPLUS", TEHAI_FLAG.SURPLUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_CANCELLED", TEHAI_FLAG.CANCELLED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_COMP", TEHAI_NYUKA_FLAG.COMP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_PROC", TEHAI_NYUKA_FLAG.PROC_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_ASSY_FLAG_COMP", TEHAI_ASSY_FLAG.COMP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_ASSY_FLAG_PROC", TEHAI_ASSY_FLAG.PROC_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_TAG_TOUROKU_FLAG_COMP", TEHAI_TAG_TOUROKU_FLAG.COMP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_TAG_TOUROKU_FLAG_PROC", TEHAI_TAG_TOUROKU_FLAG.PROC_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_TAG_TOUROKU_FLAG_OVER", TEHAI_TAG_TOUROKU_FLAG.OVER_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_SYUKKA_FLAG_COMP", TEHAI_SYUKKA_FLAG.COMP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_SYUKKA_FLAG_PROC", TEHAI_SYUKKA_FLAG.PROC_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_SYUKKA_FLAG_OVER", TEHAI_SYUKKA_FLAG.OVER_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("NOT_EXISTS_VALUE", ComDefine.NOT_EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NYUKA_FLAG_GROUP_CD", TEHAI_NYUKA_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_ASSY_FLAG_GROUP_CD", TEHAI_ASSY_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_TAG_TOUROKU_FLAG_GROUP_CD", TEHAI_TAG_TOUROKU_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_SYUKKA_FLAG_GROUP_CD", TEHAI_SYUKKA_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_COLOR_GROUP_CD", TEHAI_COLOR.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_COLOR_ITEM_CD_COLOR01", TEHAI_COLOR.COLOR01_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_COLOR_ITEM_CD_COLOR02", TEHAI_COLOR.COLOR02_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_COLOR_ITEM_CD_COLOR03", TEHAI_COLOR.COLOR03_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_COLOR_ITEM_CD_COLOR04", TEHAI_COLOR.COLOR04_NAME));
            paramCollection.Add(iNewParam.NewDbParameter("HENKYAKUHIN_FLAG_GROUP_CD", HENKYAKUHIN_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("HENKYAKUHIN_FLAG_HENKYAKUHIN", HENKYAKUHIN_FLAG.HENKYAKUHIN_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_HIKIWATASHIZUMI", DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1));

            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                // STEP14_2_11 2022/02/25 鷲見修正部分
                if (cond.ProjectNo != ComDefine.COMBO_ALL_AR_VALUE && cond.ProjectNo != ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE && cond.ProjectNo != ComDefine.COMBO_ALL_VALUE)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
                }
            }
            if (!string.IsNullOrEmpty(cond.EcsQuota))
            {
                paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            if (!string.IsNullOrEmpty(cond.Hinmei))
            {
                paramCollection.Add(iNewParam.NewDbParameter("HINMEI", cond.Hinmei + "%"));
            }
			if (!string.IsNullOrEmpty(cond.ZumenKeishiki))
            {
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", cond.ZumenKeishiki + "%"));
            }
            if (!string.IsNullOrEmpty(cond.CreateDateStart))
            {
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE_START", cond.CreateDateStart));
            }
            if (!string.IsNullOrEmpty(cond.CreateDateEnd))
            {
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE_END", cond.CreateDateEnd));
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateStart))
            {
                paramCollection.Add(iNewParam.NewDbParameter("SETTEI_DATE_START", cond.SetteiDateStart));
                //cond.SetteiDateStart = "1753/01/01";
            }
            if (!string.IsNullOrEmpty(cond.SetteiDateEnd))
            {
                paramCollection.Add(iNewParam.NewDbParameter("SETTEI_DATE_END", cond.SetteiDateEnd));
                //cond.SetteiDateEnd = "9998/12/31";
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            if (!string.IsNullOrEmpty(cond.KiwakuShip))
            {
                paramCollection.Add(iNewParam.NewDbParameter("KIWAKU_SHIP", cond.KiwakuShip));
            }
            if (!string.IsNullOrEmpty(cond.CaseNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", cond.CaseNo));
            }
            if (!string.IsNullOrEmpty(cond.TagNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.TagNo));
            }
            if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            }
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region String配列を指定セパレータ区切りの文字列に変換

    /// --------------------------------------------------
    /// <summary>
    /// String配列を指定セパレータ区切りの文字列に変換
    /// </summary>
    /// <param name="separator">セパレータ</param>
    /// <param name="array">String配列</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
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
    
    #region 表示データ取得(手配情報照会)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(手配情報照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="tehaiRenkeiNo">手配連携No</param>
    /// <returns></returns>
    /// <create>D.Okumura 2021/07/30 手配情報照会背景色対応</create>
    /// <update>N.Ikari 2022/05/12 納入状態に受入日を紐づける対応</update>
    /// --------------------------------------------------
    internal DataTable Sql_GetTehaiSksStatus(DatabaseHelper dbHelper, CondT01 cond, string tehaiRenkeiNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_SKS.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            // -- 納入状態
            sb.ApdL("      CASE ");
            sb.ApdN("        WHEN MC4.VALUE3 = ").ApdN(this.BindPrefix).ApdN("HACCHU_KENSHU_KANNOU").ApdL(" THEN CONCAT(MC4.ITEM_NAME,CASE WHEN TTS.UKEIRE_DATE IS NOT NULL AND TTS.UKEIRE_DATE != '' THEN'('+TTS.UKEIRE_DATE+')' END)");
            sb.ApdL("        WHEN ISDATE(TTS.KAITO_DATE) = 1 THEN TTS.KAITO_DATE");
            sb.ApdL("        ELSE MC4.ITEM_NAME");
            sb.ApdL("      END AS NONYU_JYOTAI");
            // -- 手配No
            sb.ApdL("    , TTMS.TEHAI_NO AS TEHAI_NO");
            // -- 受入日
            sb.ApdL("    , TTS.UKEIRE_DATE AS UKEIRE_DATE");

            sb.ApdL(" FROM");
            sb.ApdL("      T_TEHAI_MEISAI_SKS AS TTMS");
            sb.ApdL("INNER JOIN T_TEHAI_SKS AS TTS ON TTS.TEHAI_NO = TTMS.TEHAI_NO");
            sb.ApdN("INNER JOIN M_COMMON AS MC4 ON MC4.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("SURPPLIES_ORDER_FLAG_GROUP_CD")
                                       .ApdN(" AND MC4.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                       .ApdL(" AND MC4.VALUE1 = TTS.HACCHU_ZYOTAI_FLAG");
            sb.ApdL("WHERE ");
            sb.ApdN("      TTMS.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdL("ORDER BY ");
            sb.ApdL("      TTS.TEHAI_NO ");

            paramCollection.Add(iNewParam.NewDbParameter("SURPPLIES_ORDER_FLAG_GROUP_CD", SURPPLIES_ORDER_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("HACCHU_KENSHU_KANNOU", HACCHU_FLAG.STATUS_2_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", tehaiRenkeiNo));
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

    #region 物件名指定進捗件数取得
    /// --------------------------------------------------
    /// <summary>
    /// 物件名指定進捗件数取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2019/09/09</create>
    /// <update></update>
    /// <remarks>物件をまたぐ全て(AR)と全て(AR未出荷)は、0件とする</remarks>
    /// --------------------------------------------------
    internal DataTable Sql_GetProgress(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable ret = new DataTable(ComDefine.DTTBL_PROGRESS);
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
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       (TSM.TEHAI_RENKEI_NO IS NOT NULL AND LTRIM(RTRIM(TSM.TEHAI_RENKEI_NO)) != '')");
            sb.ApdN("   AND MB.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
           
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND MB.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            if (cond.NonyusakiCDs != null)
            {
                string value = string.Empty;
                value += "(";
                value += this.ConvArrayToString(",", cond.NonyusakiCDs);
                value += ")";
                sb.ApdN("   AND TSM.NONYUSAKI_CD IN ").ApdL(value);
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND TSM.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ret);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region 納入先マスタ一覧取得（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ一覧取得（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dtTehaiMeisai">データ</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>J.Chen 2024/10/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockTehaiJohoList(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiJoho)
    {
        try
        {
            DataTable resultDt = null;
            List<DataTable> dtList = ChunkDataTable(dtTehaiJoho, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                DataTable dt = new DataTable(Def_M_NONYUSAKI.Name);
                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文(手配明細行ロック)
                // TTM:手配明細
                sb.ApdL("SELECT DISTINCT");
                sb.ApdL("    TTM.TEHAI_RENKEI_NO");
                sb.ApdL("  , CAST(NULL AS NCHAR(5)) AS TAG_NO");
                sb.ApdL("  , CONVERT(NCHAR(27), TTM.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
                sb.ApdL("  FROM T_TEHAI_MEISAI AS TTM");
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
                sb.ApdL(" WHERE ");
                // 手配連携No
                SetupWherePhraseTehaiJoho(sb, iNewParam, paramCollection, chunkDt, "TTM");

                sb.ApdL("UNION ALL");

                // SQL文(出荷明細行ロック)
                // TSM:出荷明細
                sb.ApdL("SELECT DISTINCT");
                sb.ApdL("    TSM.TEHAI_RENKEI_NO");
                sb.ApdL("  , TSM.TAG_NO");
                sb.ApdL("  , CONVERT(NCHAR(27), TSM.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
                sb.ApdL("  FROM T_SHUKKA_MEISAI AS TSM");
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
                sb.ApdL(" WHERE ");
                // 納入先コード
                SetupWherePhraseTehaiJoho(sb, iNewParam, paramCollection, chunkDt, "TSM");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));

                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (resultDt == null)
                {
                    resultDt = dt.Clone();
                }

                foreach (DataRow dr in dt.Rows)
                {
                    resultDt.ImportRow(dr);
                }
            }

            return resultDt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion //SQL実行 T0100030:手配明細照会

    #region UPDATE

    #region 納入先マスタバージョン更新(現場用ステータス)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタバージョン更新(現場用ステータス)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/10/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiRireki(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dt, 1); // 一件ずつ更新する

            foreach (DataTable chunkDt in dtList)
            {

                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                int dataType = Convert.ToInt32(ComFunc.GetFldObject(chunkDt.Rows[0], "DATA_TYPE")); // 0:手配明細、1:出荷明細

                if (dataType == 0)
                {
                    // SQL文
                    sb.ApdL("UPDATE TTM");
                    sb.ApdL("SET");
                    sb.ApdN("       TEHAI_RIREKI = ").ApdN(this.BindPrefix).ApdL("TEHAI_RIREKI");
                    sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                    sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                    sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                    sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
                    sb.ApdL("FROM T_TEHAI_MEISAI AS TTM");
                    sb.ApdL(" WHERE");

                    // 納入先コード
                    SetupWherePhraseTehaiJoho(sb, iNewParam, paramCollection, chunkDt, "TTM");

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RIREKI", ComFunc.GetFldObject(chunkDt.Rows[0], Def_T_TEHAI_MEISAI.TEHAI_RIREKI)));

                    resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
                }

                if (dataType == 1)
                {
                    // SQL文
                    sb.ApdL("UPDATE TSM");
                    sb.ApdL("SET");
                    sb.ApdN("       TEHAI_RIREKI = ").ApdN(this.BindPrefix).ApdL("TEHAI_RIREKI");
                    sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                    sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                    sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                    sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
                    sb.ApdL("FROM T_SHUKKA_MEISAI AS TSM");
                    sb.ApdL(" WHERE");

                    // 納入先コード
                    SetupWherePhraseTehaiJoho(sb, iNewParam, paramCollection, chunkDt, "TSM");

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RIREKI", ComFunc.GetFldObject(chunkDt.Rows[0], Def_T_SHUKKA_MEISAI.TEHAI_RIREKI)));

                    resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
                }
                
            }

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #endregion

    #endregion //T0100030:手配明細照会

    #region T0100040:手配見積

    #region 制御

    #region 手配見積取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積取得
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">取得条件</param>
    /// <returns>取得結果</returns>
    /// <create>D.Okumura 2018/12/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiEstimate(DatabaseHelper dbHelper, CondT01 cond)
    {
        try{
            var ds = new DataSet();
            ds.Tables.Add(GetAndLockTehaiEstimate(dbHelper, cond, false));
            // 見積明細画面の処理を流用
            ds.Tables.Add(GetTehaiEstimateMeisai(dbHelper, cond));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配見積追加
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">追加対象のデータテーブル</param>
    /// <param name="estimateNo">採番した見積番号</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/3</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsTehaiMitsumori(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, string estimateNo, ref string errMsgID, ref string[] args)
    {
        try
        {

            // 手配テーブルを取得
            DataTable dtIns = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_INSERT) ? ds.Tables[ComDefine.DTTBL_INSERT] : null;

            // 手配明細更新対象
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtIns, out errMsgID))
                return false;

            // 手配明細見積の登録
            if (this.InsTehaiEstimate(dbHelper, cond, ds.Tables[Def_T_TEHAI_ESTIMATE.Name], estimateNo) < 1)
            {
                // 保存に失敗しました
                errMsgID = "A9999999013";
                return false;
            }

            // 手配明細更新対象
            if (dtIns != null)
            {
                // 手配明細の更新
                if (this.UpdTehaiEstimateMeisai(dbHelper, cond, dtIns, estimateNo) < 1)
                {
                    // 保存に失敗しました
                    errMsgID = "A9999999013";
                    return false;
                }
            }
            return true;

        }
        catch (Exception ex)
        {
            if (ex.InnerException.GetType() == typeof(System.Data.DuplicateNameException))
            {
                // 既に登録されています。
                errMsgID = "A9999999038";
                return false;
            }
            else
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
    #endregion

    #region 手配見積更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積変更
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTehaiMitsumori(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtUpd = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_UPDATE) ? ds.Tables[ComDefine.DTTBL_UPDATE] : null;
            DataTable dtDel = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_DELETE) ? ds.Tables[ComDefine.DTTBL_DELETE] : null;
            DataTable dtIns = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_INSERT) ? ds.Tables[ComDefine.DTTBL_INSERT] : null;
            int index;
            int[] notFoundIndex = null;


            // 手配見積テーブルのロック
            if (!string.IsNullOrEmpty(cond.MitsumoriNo))
            {
                // 更新対象データ
                DataTable dtEstimate = this.GetAndLockTehaiEstimate(dbHelper, cond, true);
                index = this.CheckSameData(ds.Tables[Def_T_TEHAI_ESTIMATE.Name], dtEstimate, out notFoundIndex, Def_T_TEHAI_ESTIMATE.VERSION, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);
                if (0 <= index || notFoundIndex != null)
                {
                    // 他端末で更新された為、更新出来ませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
            }

            // 手配明細更新対象
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtIns, out errMsgID))
                return false;
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtUpd, out errMsgID))
                return false;
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtDel, out errMsgID))
                return false;

            // 手配見積更新
            if (this.UpdTehaiEstimate(dbHelper, cond, ds.Tables[Def_T_TEHAI_ESTIMATE.Name]) < 1)
            {
                // 保存に失敗しました
                errMsgID = "A9999999013";
                return false;
            }


            // 手配明細更新対象
            if (dtIns != null)
            {
                // 手配明細の更新
                if (this.UpdTehaiEstimateMeisai(dbHelper, cond, dtIns, cond.MitsumoriNo) < 1)
                {
                    // 保存に失敗しました
                    errMsgID = "A9999999013";
                    return false;
                }
            }
            if (dtUpd != null)
            {
                // 手配明細の更新
                if (this.UpdTehaiEstimateMeisai(dbHelper, cond, dtUpd, cond.MitsumoriNo) < 1)
                {
                    // 保存に失敗しました
                    errMsgID = "A9999999013";
                    return false;
                }
            }
            // 削除
            if (dtDel != null)
            {
                // 手配明細の削除
                if (this.DelTehaiEstimateMeisai(dbHelper, cond, dtDel) < 1)
                {
                    // 保存に失敗しました
                    errMsgID = "A9999999013";
                    return false;
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

    #region 受注状態更新
    /// --------------------------------------------------
    /// <summary>
    /// 受注状態更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">更新用データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTehaiEstimateOrder(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtUpd = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_UPDATE) ? ds.Tables[ComDefine.DTTBL_UPDATE] : null;
            // ロック＆取得
            DataTable dtCheck = this.GetAndLockTehaiEstimate(dbHelper, cond, true);

            var dt = ds.Tables[Def_T_TEHAI_ESTIMATE.Name];
            int[] notFoundIndex = null;

            // 更新データのバージョンチェック
            var index = this.CheckSameData(dt, dtCheck, out notFoundIndex, Def_T_TEHAI_ESTIMATE.VERSION, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);
            if (0 <= index || notFoundIndex != null)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }


            // 手配明細更新対象
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtUpd, out errMsgID))
                return false;


            // 手配見積の更新
            if (this.UpdTehaiEstimateOrder(dbHelper, cond, dt) < 1)
                return false;

            // 手配明細の更新は無し(見積のみの更新)

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 手配見積削除
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelTehaiMitsumori(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtDel = ComFunc.IsExistsTable(ds, ComDefine.DTTBL_DELETE) ? ds.Tables[ComDefine.DTTBL_DELETE] : null;
            int index;
            int[] notFoundIndex = null;


            // 手配見積テーブルのロック
            if (!string.IsNullOrEmpty(cond.MitsumoriNo))
            {
                // 更新対象データ
                DataTable dtEstimate = this.GetAndLockTehaiEstimate(dbHelper, cond, true);
                index = this.CheckSameData(ds.Tables[Def_T_TEHAI_ESTIMATE.Name], dtEstimate, out notFoundIndex, Def_T_TEHAI_ESTIMATE.VERSION, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);
                if (0 <= index || notFoundIndex != null)
                {
                    // 他端末で更新された為、更新出来ませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
            }

            // 手配明細更新対象
            if (!CheckAndLockTehaiEstimateMeisai(dbHelper, cond, dtDel, out errMsgID))
                return false;
            // 手配見積更新
            if (this.DelTehaiEstimate(dbHelper, cond, ds.Tables[Def_T_TEHAI_ESTIMATE.Name]) < 1)
            {
                // 保存に失敗しました
                errMsgID = "A9999999013";
                return false;
            }


            // 手配明細更新対象
            // 削除
            if (dtDel != null)
            {
                // 手配明細の削除
                if (this.DelTehaiEstimateMeisai(dbHelper, cond, dtDel) < 1)
                {
                    // 保存に失敗しました
                    errMsgID = "A9999999013";
                    return false;
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

    #region 見積MAIL配信

    /// --------------------------------------------------
    /// <summary>
    /// 見積MAIL配信
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTehaiEstimateMail(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtQutation = ds.Tables[Def_T_TEHAI_ESTIMATE.Name];
            var dtMail = ds.Tables[Def_T_MAIL.Name];

            // 手配見積の行ロック
            var dtLockEstimate = this.LockTehaiEstimateOnEstimateNo(dbHelper, dtQutation);
            if (!UtilData.ExistsData(dtLockEstimate))
            {
                // 行ロック失敗
                return false;
            }

            // 手配見積の更新
            var cntUpd = this.UpdTehaiEstimatForMail(dbHelper, cond, dtLockEstimate);
            if (cntUpd != dtLockEstimate.Rows.Count)
            {
                // 更新失敗
                return false;
            }

            using (var commImpl = new WsCommonImpl())
            using (var attachFileImpl = new WsAttachFileImpl())
            {
                var path = ComFunc.GetFld(dtMail, 0, Def_T_MAIL.APPENDIX_FILE_PATH).Split('\\');
                if (path != null && path.Length == 2)
                {
                    // サーバー上の絶対パスに変更
                    var appFilePath = attachFileImpl.GetFilePath(FileType.Attachments, null, null, GirenType.None, null, path[1], path[0]);
                    UtilData.SetFld(dtMail, 0, Def_T_MAIL.APPENDIX_FILE_PATH, appFilePath);
                }
                // メール送信データ登録
                var condCommon = new CondCommon(cond.LoginInfo);
                if (commImpl.InsMail(dbHelper, dtMail.Rows[0], condCommon) != 1)
                {
                    // 登録失敗
                    return false;
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

    #region MAIL配信用累計金額取得

    /// --------------------------------------------------
    /// <summary>
    /// MAIL配信用累計金額取得
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">取得条件</param>
    /// <returns>取得結果</returns>
    /// <create>J.Chen 2024/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTotalAmountForMail(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            var ds = new DataSet();
            ds.Tables.Add(GetTehaiEstimateTotalAmountForMail(dbHelper, cond));

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 
    
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細(見積)の排他状態を確認する
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dtInput">チェック対象テーブル</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <returns>更新可否</returns>
    /// <create>D.Okumura 2018/12/21</create>
    /// <update>J.Chen 2022/04/21 STEP14</update>
    /// --------------------------------------------------
    private bool CheckAndLockTehaiEstimateMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dtInput, out string errMsgID)
    {
        // データがない場合はOKを返す
        if (dtInput == null || dtInput.Rows.Count < 1)
        {
            errMsgID = "";
            return true;
        }
        int[] notFoundIndex = null;
        // 対象データ取得
        var dtCheck = GetAndLockTehaiMitsumoriMeisaiList(dbHelper, cond, dtInput, true);
        // データのバージョンチェック
        var index = this.CheckSameData(dtInput, dtCheck, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
        if (0 <= index || notFoundIndex != null)
        {
            // 他端末で更新された為、更新出来ませんでした。
            errMsgID = "A9999999027";
            return false;
        }
        // ここで対象データの調整
        foreach (DataRow row in dtCheck.Rows)
        {
            if (row.Field<int>("CNT") > 0 && row.IsNull(ESTIMATE_CANCEL_ROLE.GROUPCD) && string.IsNullOrEmpty(cond.PONo))
            {
                // 出荷済みの場合エラーにする
                //// 他端末で更新された為、更新出来ませんでした。
                //errMsgID = "A9999999027";
                // 出荷済みのデータが含まれているため、見積管理者以外は受注解除できません。
                errMsgID = "T0100040014";
                return false;
            }
        }
        errMsgID = "";
        return true;
    }
    #endregion

    #endregion

    #region SQL実行

    #region SELECT

    #region 表示データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="isLock">ロック有無</param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/11/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockTehaiEstimateMeisai(DatabaseHelper dbHelper, CondT01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // m:手配見積(ロック)、p:プロジェクトマスタ、e:技連マスタ、t:手配明細、s:出荷明細(集荷済み以降)
            sb.ApdL("SELECT");
            sb.ApdL("    p.BUKKEN_NAME");
            sb.ApdL("  , e.AR_NO");
            sb.ApdL("  , t.ZUMEN_KEISHIKI");
            sb.ApdL("  , t.HINMEI_JP");
            sb.ApdL("  , t.HINMEI");
            sb.ApdL("  , t.SHUKKA_QTY");
            sb.ApdL("  , t.UNIT_PRICE");
            sb.ApdL("  , IsNull(s.CNT, 0) AS CNT");
            sb.ApdL("  , t.TEHAI_RENKEI_NO");
            sb.ApdL("  , CONVERT(NCHAR(27), t.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("FROM T_TEHAI_ESTIMATE AS m");
            if (isLock)
            {
                sb.ApdL("    WITH (ROWLOCK,UPDLOCK)");
            }
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_MEISAI AS t");
            sb.ApdL("    ON  t.ESTIMATE_NO = m.ESTIMATE_NO");
            sb.ApdL("  INNER JOIN M_ECS AS e");
            sb.ApdL("    ON  e.ECS_QUOTA = t.ECS_QUOTA");
            sb.ApdL("    AND e.ECS_NO = t.ECS_NO");
            sb.ApdL("  INNER JOIN M_PROJECT AS p");
            sb.ApdL("    ON  p.PROJECT_NO = e.PROJECT_NO");
            sb.ApdN("  LEFT OUTER JOIN (SELECT TEHAI_RENKEI_NO, COUNT(*) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= '").ApdN(JYOTAI_FLAG.SHUKAZUMI_VALUE1).ApdL("' GROUP BY TEHAI_RENKEI_NO) AS s");
            sb.ApdL("    ON s.TEHAI_RENKEI_NO = t.TEHAI_RENKEI_NO");
            sb.ApdL("WHERE ").ApdN("   m.ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");

            sb.ApdL(" ORDER BY m.ESTIMATE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", cond.MitsumoriNo));

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

    #region 見積情報取得
    /// --------------------------------------------------
    /// <summary>
    /// 見積情報取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/12/3</create>
    /// <update>J.Chen 2022/04/21 STEP14</update>
    /// <update>J.Chen 2023/12/20 項目追加</update>
    /// --------------------------------------------------
    private DataTable GetAndLockTehaiEstimate(DatabaseHelper dbHelper, CondT01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_ESTIMATE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("    ESTIMATE_NO");
            sb.ApdL("  , NAME");
            sb.ApdL("  , CURRENCY_FLAG");
            sb.ApdL("  , RATE_JPY");
            sb.ApdL("  , SALSES_PER");
            sb.ApdL("  , ROB_PER");
            sb.ApdL("  , PO_NO");
            sb.ApdL("  , UPDATE_USER_NAME");
            sb.ApdL("  , CONVERT(NCHAR(27), VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("  , PO_AMOUNT");
            sb.ApdL("  , PARTITION_AMOUNT");
            sb.ApdL("  , RATE_PARTITION");
            sb.ApdL("  , PROJECTED_SALES_MONTH");
            sb.ApdL("  , MAIL_TITLE");
            sb.ApdL("  , REV");
            sb.ApdL("  , CONSIGN_CD");
            sb.ApdL("FROM T_TEHAI_ESTIMATE");
            if (isLock)
            {
                sb.ApdL("    WITH (ROWLOCK,UPDLOCK)");
            }
            sb.ApdL("WHERE ").ApdN("   ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", cond.MitsumoriNo));

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

    #region 手配明細見積ロック(見積No)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細見積ロック(見積No)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">DataTable</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockTehaiEstimateOnEstimateNo(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_TEHAI_ESTIMATE.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ESTIMATE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PROJECTED_SALES_MONTH AS PROJECTED_SALES_MONTH");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_TITLE AS MAIL_TITLE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REV AS REV");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD AS CONSIGN_CD");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_ESTIMATE");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("PROJECTED_SALES_MONTH", ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_TITLE", ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.MAIL_TITLE)));
                paramCollection.Add(iNewParam.NewDbParameter("REV", ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.REV)));
                paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.CONSIGN_CD)));

                // SQL実行
                var dtTmp = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                if (dtTmp.Rows.Count > 0)
                {
                    ret.Merge(dtTmp);
                }
            }
            return ret;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region MAIL配信用累計金額取得
    /// --------------------------------------------------
    /// <summary>
    /// MAIL配信用累計金額取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>データテーブル(見積情報)</returns>
    /// <create>J.Chen 2024/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetTehaiEstimateTotalAmountForMail(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_ESTIMATE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 各見積の仕切り金額を集計する
            sb.ApdL("WITH TOTAL_AMOUNT AS (");
            sb.ApdL("    SELECT");
            sb.ApdL("        TTE.ESTIMATE_NO");
            sb.ApdL("      , SUM(PARTITION_AMOUNT) AS TOTAL_PARTITION_AMOUNT");
            sb.ApdL("    FROM");
            sb.ApdL("        (SELECT");
            sb.ApdL("            TTE.ESTIMATE_NO");
            sb.ApdL("          , CEILING(SUM(");
            sb.ApdL("                CASE");
            sb.ApdN("                    WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("SKS_SKIP_VALUE").ApdL(" THEN TTM.SHUKKA_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("                    ELSE TTM.HACCHU_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("                END");
            sb.ApdL("            ) * MAX(TTE.RATE_JPY) * MAX(TTE.RATE_PARTITION)) AS PARTITION_AMOUNT");
            sb.ApdL("        FROM T_TEHAI_ESTIMATE AS TTE");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI TTM ON TTE.ESTIMATE_NO = TTM.ESTIMATE_NO");
            sb.ApdL("        INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO");
            sb.ApdL("        WHERE 1 = 1");
            sb.ApdN("        AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdL("        GROUP BY TTE.ESTIMATE_NO) AS TTE");
            sb.ApdL("    GROUP BY TTE.ESTIMATE_NO");
            sb.ApdL(")");

            // SQL文
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("    TE.ESTIMATE_NO");
            sb.ApdL("  , SUM(ISNULL(TE.PO_AMOUNT, 0)) AS PO_AMOUNT");
            sb.ApdL("  , MAX(ISNULL(TE.PARTITION_AMOUNT, 0)) AS PARTITION_AMOUNT");

            sb.ApdL("FROM (");
            sb.ApdL("    SELECT ");
            sb.ApdL("        TTE.ESTIMATE_NO");
            sb.ApdL("      , TTM.TEHAI_RENKEI_NO");
            sb.ApdL("      , SUM(");
            sb.ApdL("          CASE");
            sb.ApdN("            WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("SKS_SKIP_VALUE").ApdL(" THEN TTM.SHUKKA_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("            ELSE TTM.HACCHU_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("          END");
            sb.ApdL("        ) AS PO_AMOUNT");
            sb.ApdL("      , MAX(TA.TOTAL_PARTITION_AMOUNT) AS PARTITION_AMOUNT");
            sb.ApdL("    FROM T_TEHAI_ESTIMATE AS TTE");
            sb.ApdL("     INNER JOIN T_TEHAI_MEISAI TTM ON TTE.ESTIMATE_NO = TTM.ESTIMATE_NO");
            sb.ApdL("     INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO ");
            sb.ApdL("      LEFT JOIN TOTAL_AMOUNT TA ON TTE.ESTIMATE_NO = TA.ESTIMATE_NO ");
            sb.ApdL("    WHERE 1 = 1");
            sb.ApdN("      AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdN("      AND TTE.ESTIMATE_NO <> ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");
            sb.ApdL("    GROUP BY");
            sb.ApdL("        TTE.ESTIMATE_NO");
            sb.ApdL("      , TTM.TEHAI_RENKEI_NO");
            sb.ApdL("     )TE");

            sb.ApdL(" GROUP BY TE.ESTIMATE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", cond.MitsumoriNo));
            paramCollection.Add(iNewParam.NewDbParameter("SKS_SKIP_VALUE", TEHAI_FLAG.SKS_SKIP_VALUE1));

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
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <param name="estimateNo">見積番号</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/03</create>
    /// <update>J.Chen 2023/12/20 項目追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int InsTehaiEstimate(DatabaseHelper dbHelper, CondT01 cond, DataTable dt, string estimateNo)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("INSERT INTO T_TEHAI_ESTIMATE (");
            sb.ApdL("     ESTIMATE_NO");
            sb.ApdL("    ,NAME");
            sb.ApdL("    ,CURRENCY_FLAG");
            sb.ApdL("    ,RATE_JPY");
            sb.ApdL("    ,SALSES_PER");
            sb.ApdL("    ,ROB_PER");
            sb.ApdL("    ,PO_NO");
            sb.ApdL("    ,CREATE_DATE");
            sb.ApdL("    ,CREATE_USER_ID");
            sb.ApdL("    ,CREATE_USER_NAME");
            sb.ApdL("    ,UPDATE_DATE");
            sb.ApdL("    ,UPDATE_USER_ID");
            sb.ApdL("    ,UPDATE_USER_NAME");
            sb.ApdL("    ,VERSION");
            sb.ApdL("    ,PO_AMOUNT");
            sb.ApdL("    ,PARTITION_AMOUNT");
            sb.ApdL("    ,RATE_PARTITION");
            sb.ApdL("    ,PROJECTED_SALES_MONTH");
            sb.ApdL("    ,MAIL_TITLE");
            sb.ApdL("    ,REV");
            sb.ApdL("    ,CONSIGN_CD");
            sb.ApdL(") VALUES (");
            sb.ApdN("     ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("NAME");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CURRENCY_FLAG");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("RATE_JPY");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("SALSES_PER");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("ROB_PER");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("PO_NO");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("    ,").ApdL(this.SysTimestamp);
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("PO_AMOUNT");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("PARTITION_AMOUNT");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("RATE_PARTITION");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("PROJECTED_SALES_MONTH");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("MAIL_TITLE");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("REV");
            sb.ApdN("    ,").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdL(")");

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, estimateNo));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.NAME, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.NAME)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.RATE_JPY, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.RATE_JPY)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.SALSES_PER, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.SALSES_PER)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ROB_PER, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.ROB_PER)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PO_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.PO_NO)));

                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PO_AMOUNT, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.PO_AMOUNT)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.RATE_PARTITION, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.RATE_PARTITION)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.MAIL_TITLE, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.MAIL_TITLE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.REV, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.REV)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.CONSIGN_CD, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.CONSIGN_CD)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細の更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <param name="estimateNo">見積番号</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update>J.Chen 2024/10/29 変更履歴追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdTehaiEstimateMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt, string estimateNo)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("      ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ESTIMATE_NO);
            sb.ApdN("    , INVOICE_UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE);
            sb.ApdN("    , UNIT_PRICE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UNIT_PRICE);
            sb.ApdN("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_ID);
            sb.ApdN("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME);
            sb.ApdN("    , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , INVOICE_VALUE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.INVOICE_VALUE);
            sb.ApdN("    , ESTIMATE_RIREKI = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI);
            sb.ApdL("WHERE ");
            sb.ApdN("    TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ESTIMATE_NO, estimateNo));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.INVOICE_UNIT_PRICE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UNIT_PRICE, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.UNIT_PRICE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.INVOICE_VALUE, ComFunc.GetFldObject(dr, ComDefine.FLD_ROB_SUM_RMB)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// --------------------------------------------------
    /// <summary>
    /// 見積状態更新(PO No除く)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/10</create>
    /// <update>J.Chen 2023/12/20 項目追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdTehaiEstimate(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("UPDATE T_TEHAI_ESTIMATE");
            sb.ApdL("SET");
            sb.ApdN("      NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.NAME);
            sb.ApdN("    , CURRENCY_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG);
            sb.ApdN("    , RATE_JPY = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.RATE_JPY);
            sb.ApdN("    , SALSES_PER = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.SALSES_PER);
            sb.ApdN("    , ROB_PER = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.ROB_PER);
            sb.ApdN("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_ID);
            sb.ApdN("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME);
            sb.ApdN("    , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , PO_AMOUNT = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.PO_AMOUNT);
            sb.ApdN("    , PARTITION_AMOUNT = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT);
            sb.ApdN("    , RATE_PARTITION = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.RATE_PARTITION);
            sb.ApdN("    , PROJECTED_SALES_MONTH = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH);
            sb.ApdN("    , MAIL_TITLE = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.MAIL_TITLE);
            sb.ApdN("    , REV = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.REV);
            sb.ApdN("    , CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.CONSIGN_CD);
            sb.ApdL("WHERE ");
            sb.ApdN("    ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;

            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, cond.MitsumoriNo));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.NAME, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.NAME)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.CURRENCY_FLAG)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.RATE_JPY, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.RATE_JPY)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.SALSES_PER, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.SALSES_PER)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ROB_PER, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.ROB_PER)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PO_AMOUNT, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PO_AMOUNT)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PARTITION_AMOUNT)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.RATE_PARTITION, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.RATE_PARTITION)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.MAIL_TITLE, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.MAIL_TITLE)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.REV, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.REV)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.CONSIGN_CD, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.CONSIGN_CD)));

                ret = dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// --------------------------------------------------
    /// <summary>
    /// 受注状態更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdTehaiEstimateOrder(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("UPDATE T_TEHAI_ESTIMATE");
            sb.ApdL("SET");
            sb.ApdN("      PO_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.PO_NO);
            sb.ApdN("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_ID);
            sb.ApdN("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME);
            sb.ApdN("    , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("WHERE ");
            sb.ApdN("    ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;

            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.PO_NO, ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PO_NO)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                ret = dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積更新(MAIL送信用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/23</create>
    /// <update></update>
    /// --------------------------------------------------
    internal int UpdTehaiEstimatForMail(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_ESTIMATE");
            sb.ApdL("SET");
            sb.ApdN("       PROJECTED_SALES_MONTH = ").ApdN(this.BindPrefix).ApdL("PROJECTED_SALES_MONTH");
            sb.ApdN("     , MAIL_TITLE = ").ApdN(this.BindPrefix).ApdL("MAIL_TITLE");
            sb.ApdN("     , REV = ").ApdN(this.BindPrefix).ApdL("REV");
            sb.ApdN("     , CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PROJECTED_SALES_MONTH", ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PROJECTED_SALES_MONTH)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_TITLE", ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.MAIL_TITLE)));
                paramCollection.Add(iNewParam.NewDbParameter("REV", ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.REV)));
                paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.CONSIGN_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO)));

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

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細データ関係解除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配明細テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/04</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// <update>J.Jeong 2024/08/07 InvoiceValueクリア</update>
    /// <remaks>
    /// メソッド名はDeleteだが、内部的にはUPDATE文を発行している
    /// </remaks>
    /// --------------------------------------------------
    private int DelTehaiEstimateMeisai(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dt, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParameter = dbHelper;

                // SQL文
                StringBuilder sb = new StringBuilder();
                sb.ApdL("UPDATE T_TEHAI_MEISAI");
                sb.ApdL("SET");
                sb.ApdL("      ESTIMATE_NO = NULL");
                sb.ApdL("    , INVOICE_UNIT_PRICE = NULL");
                sb.ApdL("    , INVOICE_VALUE = NULL");
                sb.ApdN("    , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                sb.ApdN("    , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_ID);
                sb.ApdN("    , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME);
                sb.ApdN("    , VERSION = ").ApdL(this.SysTimestamp);
                sb.ApdL("WHERE ");
                // 手配連携番号を条件に追加
                SetupWherePhraseTehaiRenkeiNo(sb, iNewParameter, paramCollection, chunkDt, "");

                // バインド変数設定
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_ID, this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_MEISAI.UPDATE_USER_NAME, this.GetUpdateUserName(cond)));

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細の削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">手配見積テーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/12/21</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelTehaiEstimate(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int ret = 0;

            // SQL文
            var sb = new StringBuilder();
            sb.ApdL("DELETE FROM T_TEHAI_ESTIMATE");
            sb.ApdL("WHERE ");
            sb.ApdN("    ESTIMATE_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO);

            // バインド変数の設定
            INewDbParameterBasic iNewParameter = dbHelper;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();

                paramCollection.Add(iNewParameter.NewDbParameter(Def_T_TEHAI_ESTIMATE.ESTIMATE_NO, ComFunc.GetFldObject(dr, Def_T_TEHAI_ESTIMATE.ESTIMATE_NO)));

                ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    #endregion

    #endregion

    #endregion

    #region T0100050:手配見積明細

    #region 制御

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/11/09</create>
    /// <update>J.Chen 2024/10/29 履歴更新処理追加</update>
    /// --------------------------------------------------
    public bool UpdTehaiMitsumoriMeisaiVersionData(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック
            DataTable dtTehaiMitsumoriList = GetAndLockTehaiMitsumoriMeisaiList(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name], true);
            if ((dtTehaiMitsumoriList == null)
                || (dtTehaiMitsumoriList.Rows.Count == 0)
                || (ds.Tables[Def_T_TEHAI_MEISAI.Name].Rows.Count != dtTehaiMitsumoriList.Rows.Count))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(ds.Tables[Def_T_TEHAI_MEISAI.Name], dtTehaiMitsumoriList, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
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

            // 履歴更新更新の場合
            if (cond.RirekiUpdate)
            {
                this.UpdTehaiMitsumoriMeisaiRirekiVersion(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name]);
                return true;
            }

            // 更新実行
            var yusho = ComFunc.GetFld(ds.Tables[Def_T_TEHAI_MEISAI.Name], 0, Def_T_TEHAI_MEISAI.ESTIMATE_FLAG);
            if (yusho == ESTIMATE_FLAG.ONEROUS_VALUE1)
                this.UpdTehaiMitsumoriMeisaiYushoVersion(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name]);
            else if (yusho == ESTIMATE_FLAG.GRATIS_VALUE1)
                this.UpdTehaiMitsumoriMeisaiMushoVersion(dbHelper, cond, ds.Tables[Def_T_TEHAI_MEISAI.Name]);
            else
                return false;

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 他端末で更新された為、更新出来ません。
                errMsgID = "A9999999027";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ヘルパー

    /// --------------------------------------------------
    /// <summary>
    /// Where句に手配連携番号の配列を設定する
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ</param>
    /// <param name="paramCollection">パラメータ配列</param>
    /// <param name="dt">データセット</param>
    /// <param name="tblAlias">手配明細テーブルに付与する別名</param>
    /// <create>D.Okumura 2018/12/19</create>
    /// <update>M.Shimizu 2020/05/12 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// --------------------------------------------------
    private void SetupWherePhraseTehaiRenkeiNo(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, DataTable dt, string tblAlias)
    {
        sb.ApdL(" (");
        sb.ApdN("   ");
        if (!string.IsNullOrEmpty(tblAlias))
            sb.ApdN(tblAlias).ApdN(".");
        sb.ApdL("TEHAI_RENKEI_NO IN (");

        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (i != 0)
            {
                if (i % 10 == 0)
                {
                    sb.ApdL();
                }

                if (i % 1000 == 0)
                {
                    sb.ApdL("   )");
                    sb.ApdN("   OR ");
                    
                    if (!string.IsNullOrEmpty(tblAlias))
                    {
                        sb.ApdN(tblAlias).ApdN(".");
                    }
                    
                    sb.ApdL("TEHAI_RENKEI_NO IN (");
                    sb.ApdN("       ");
                }
                else
                {
                    sb.ApdN("     , ");
                }
            }
            else
            {
                sb.ApdN("       ");
            }

            sb.ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO" + i);
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO" + i, ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
            i++;
        }
        sb.ApdL("   )");
        sb.ApdL(" )");
    }
    #endregion

    #region SQL実行

    #region SELECT

    #region 表示データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>T_TEHAI_MEISAIを含むデータセット</returns>
    /// <create>D.Okumura 2018/12/21</create>
    /// <update>J.Chen 2024/03/19 出荷明細情報をマージ</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiMitsumoriMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            //ds.Tables.Add(GetTehaiEstimateMeisai(dbHelper, cond));

            DataTable dtTem = new DataTable();
            dtTem = GetTehaiEstimateMeisai(dbHelper, cond);
            
            DataTable dtTemSM = new DataTable();
            dtTemSM = GetTehaiEstimateMeisaiForShukkaMeisai(dbHelper, cond);

            DataTable result = dtTem.Copy();
            foreach (DataRow row in result.Rows)
            {
                string tehNo = row[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO].ToString();
                string shukkaDates = "";
                string ships = "";
                string tagNos = "";
                string invoiceNos = "";

                foreach (DataRow dr in dtTemSM.Rows)
                {
                    if (dr[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO].ToString() == tehNo)
                    {
                        if (!shukkaDates.Contains(dr[Def_T_SHUKKA_MEISAI.SHUKKA_DATE].ToString()))
                            shukkaDates += dr[Def_T_SHUKKA_MEISAI.SHUKKA_DATE] + ",";

                        if (!ships.Contains(dr[Def_M_NONYUSAKI.SHIP].ToString()))
                            ships += dr[Def_M_NONYUSAKI.SHIP] + ",";

                        if (!tagNos.Contains(dr[Def_T_SHUKKA_MEISAI.TAG_NO].ToString()))
                            tagNos += dr[Def_T_SHUKKA_MEISAI.TAG_NO] + ",";

                        if (!invoiceNos.Contains(dr[Def_T_SHUKKA_MEISAI.INVOICE_NO].ToString()))
                            invoiceNos += dr[Def_T_SHUKKA_MEISAI.INVOICE_NO] + ",";
                    }
                }

                row[Def_T_SHUKKA_MEISAI.SHUKKA_DATE] = shukkaDates.TrimEnd(',');
                row[Def_M_NONYUSAKI.SHIP] = ships.TrimEnd(',');
                row[Def_T_SHUKKA_MEISAI.TAG_NO] = tagNos.TrimEnd(',');
                row[Def_T_SHUKKA_MEISAI.INVOICE_NO] = invoiceNos.TrimEnd(',');
            }

            ds.Tables.Add(result);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>データテーブル(T_TEHAI_MEISAI,見積情報含む)</returns>
    /// <create>D.Okumura 2018/12/21</create>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-85 対応</update>
    /// <update>T.Nukaga 2020/09/17 バインド変数(ESTIMATE_FLAG_ONEROUS)設定修正</update>
    /// <update>J.Chen 2022/04/21 STEP14</update>
    /// <update>J.Chen 2024/02/09 取得データ追加</update>
    /// <update>J.Chen 2024/10/29 変更履歴追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    private DataTable GetTehaiEstimateMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // t:手配明細、p:プロジェクトマスタ、e:技連マスタ、uu:ユーザーマスタ、ce:汎用(手配有償無償)、cr:汎用(手配見積レート)、
            // ct:汎用(手配区分)、cq:汎用(数量単位)、cc:汎用(通貨)、cec:汎用(受注解除権限)、cc:汎用(通貨)、ch:汎用(出荷制限)、cj:汎用(出荷状況)
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("    t.TEHAI_RENKEI_NO");
            sb.ApdL("  , p.BUKKEN_NAME");
            sb.ApdL("  , t.ECS_QUOTA");
            sb.ApdL("  , t.ECS_NO");
            sb.ApdL("  , t.SETTEI_DATE");
            sb.ApdL("  , t.NOUHIN_SAKI");
            sb.ApdL("  , t.SYUKKA_SAKI");
            sb.ApdL("  , e.PROJECT_NO");
            sb.ApdL("  , e.SEIBAN");
            sb.ApdL("  , e.CODE");
            sb.ApdL("  , e.KISHU");
            sb.ApdL("  , e.AR_NO");
            sb.ApdL("  , e.KANRI_FLAG");
            sb.ApdL("  , t.ZUMEN_OIBAN");
            sb.ApdL("  , t.FLOOR");
            sb.ApdL("  , t.ST_NO");
            sb.ApdL("  , t.HINMEI_JP");
            sb.ApdL("  , t.HINMEI");
            sb.ApdL("  , t.HINMEI_INV");
            sb.ApdL("  , t.ZUMEN_KEISHIKI");
            sb.ApdL("  , t.TEHAI_QTY");
            sb.ApdL("  , t.TEHAI_FLAG");
            sb.ApdL("  , ct.ITEM_NAME AS TEHAI_FLAG_NAME");
            sb.ApdL("  , t.TEHAI_KIND_FLAG");
            sb.ApdL("  , t.HACCHU_QTY");
            sb.ApdL("  , t.SHUKKA_QTY");
            sb.ApdL("  , t.FREE1");
            sb.ApdL("  , t.FREE2");
            sb.ApdL("  , t.QUANTITY_UNIT");
            sb.ApdL("  , cq.ITEM_NAME AS QUANTITY_UNIT_NAME");
            sb.ApdL("  , t.ZUMEN_KEISHIKI2");
            sb.ApdL("  , t.NOTE");
            sb.ApdL("  , t.CUSTOMS_STATUS");
            sb.ApdL("  , STUFF((");
            sb.ApdL("      SELECT");
            sb.ApdL("              '+' +T_TEHAI_MEISAI_SKS.TEHAI_NO");
            sb.ApdL("       FROM");
            sb.ApdL("              T_TEHAI_MEISAI_SKS");
            sb.ApdL("       WHERE");
            sb.ApdL("              T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = t.TEHAI_RENKEI_NO");
            sb.ApdL("         FOR XML PATH ('')");
            sb.ApdL("     ),1,1,'') AS TEHAI_NO");
            sb.ApdL("  , t.MAKER");
            sb.ApdL("  , t.UNIT_PRICE");
            sb.ApdL("  , t.INVOICE_VALUE");
            sb.ApdL("  , t.ARRIVAL_QTY");
            sb.ApdL("  , t.ASSY_QTY");
            sb.ApdL("  , t.ESTIMATE_FLAG");
            sb.ApdL("  , ce.ITEM_NAME AS ESTIMATE_FLAG_NAME");
            sb.ApdL("  , CASE cr.VALUE1 WHEN '1' THEN m.CURRENCY_FLAG ELSE NULL END AS CURRENCY_FLAG");
            sb.ApdL("  , CASE cr.VALUE1 WHEN '1' THEN cc.ITEM_NAME ELSE NULL END AS CURRENCY_FLAG_NAME");
            sb.ApdL("  , CASE cr.VALUE1 WHEN '1' THEN m.RATE_JPY ELSE NULL END AS RATE_JPY");
            sb.ApdN("  , CASE t.ESTIMATE_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN m.SALSES_PER");
            sb.ApdN("      WHEN ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_GRATIS THEN CAST(cr.VALUE2 AS decimal(3, 0))");
            sb.ApdL("      ELSE CAST(NULL AS decimal(3, 0))");
            sb.ApdL("    END AS SALSES_PER");
            sb.ApdN("  , CASE t.ESTIMATE_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN m.ROB_PER");
            sb.ApdN("      WHEN ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_GRATIS THEN CAST(cr.VALUE3 AS decimal(3, 0))");
            sb.ApdL("      ELSE CAST(NULL AS decimal(3, 0))");
            sb.ApdL("    END AS ROB_PER");
            sb.ApdL("  , CASE cr.VALUE1 WHEN '1' THEN m.ESTIMATE_NO ELSE NULL END AS ESTIMATE_NO");
            sb.ApdL("  , m.NAME AS ESTIMATE_NAME");
            sb.ApdL("  , CASE cr.VALUE1 WHEN '1' THEN m.PO_NO ELSE NULL END AS PO_NO");

            // 出荷制限
            sb.ApdL("  , t.HENKYAKUHIN_FLAG");
            sb.ApdL("  , ch.ITEM_NAME AS HENKYAKUHIN_FLAG_NAME");

            // 出荷状況
            //sb.ApdL("  , STUFF((");
            //sb.ApdL("      SELECT");
            //sb.ApdL("              ', ' + M_COMMON.ITEM_NAME");
            //sb.ApdL("       FROM");
            //sb.ApdL("              M_COMMON");
            //sb.ApdL("       WHERE");
            //sb.ApdL("              M_COMMON.VALUE1 IN (");
            //sb.ApdL("           SELECT");
            //sb.ApdL("                  T_SHUKKA_MEISAI.JYOTAI_FLAG");
            //sb.ApdL("             FROM");
            //sb.ApdL("                  T_SHUKKA_MEISAI");
            //sb.ApdL("            WHERE");
            //sb.ApdL("                  T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = t.TEHAI_RENKEI_NO )");
            //sb.ApdN("          AND M_COMMON.GROUP_CD = '").ApdN(DISP_JYOTAI_FLAG.GROUPCD).ApdL("'");
            //sb.ApdN("          AND M_COMMON.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            //sb.ApdL("         FOR XML PATH ('')");
            //sb.ApdL("     ),1,1,'') AS JYOTAI_NAME");
            sb.ApdL("  , cj.ITEM_NAME AS JYOTAI_NAME");

            // 出荷日
            sb.ApdL("  , CONVERT(nvarchar(MAX), NULL) AS SHUKKA_DATE");

            // 便名
            sb.ApdL("  , CONVERT(nvarchar(MAX), NULL) AS SHIP");

            // TAG NO
            sb.ApdL("  , CONVERT(nvarchar(MAX), NULL) AS TAG_NO");

            // INVOICE NO
            sb.ApdL("  , CONVERT(nvarchar(MAX), NULL) AS INVOICE_NO");

//            sb.ApdL("  , t.INVOICE_UNIT_PRICE");
            sb.ApdL("  , CASE ");
            sb.ApdN("        WHEN t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN t.INVOICE_UNIT_PRICE");
            sb.ApdN("        WHEN t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + t.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + t.SGA_RATE) * IsNull(t.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("        ELSE t.INVOICE_UNIT_PRICE");
            sb.ApdL("    END AS INVOICE_UNIT_PRICE");
            sb.ApdL("  , CONVERT(NCHAR(27), t.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("  , IsNull(s.CNT, 0) AS CNT");
            sb.ApdL("  , ce.VALUE3 AS ESTIMATE_COLOR");
            sb.ApdL("  , CAST(0 AS decimal(9, 0)) AS UNIT_PRICE_SALSE");    // 手配見積画面表示用
            sb.ApdL("  , CAST(0 AS decimal(9, 0)) AS UNIT_PRICE_RMB");      // 手配見積画面表示用
            sb.ApdL("  , CAST(0 AS decimal(9, 0)) AS SUM_RMB");             // 手配見積画面表示用
            sb.ApdL("  , CAST(0 AS decimal(9, 0)) AS ROB_UNIT_PRICE_RMB");  // 手配見積画面表示用
            sb.ApdL("  , CAST(0 AS decimal(9, 0)) AS ROB_SUM_RMB");         // 手配見積画面表示用
            sb.ApdL("  , CASE uu.ROLE_ID WHEN cec.VALUE1 THEN '1' ELSE NULL END AS ESTIMATE_CANCEL_ROLE");
            sb.ApdL("  , t.ESTIMATE_RIREKI");
            sb.ApdL("FROM T_TEHAI_MEISAI AS t");
            sb.ApdL("  INNER JOIN M_ECS AS e");
            sb.ApdL("    ON  e.ECS_QUOTA = t.ECS_QUOTA");
            sb.ApdL("    AND e.ECS_NO = t.ECS_NO");
            sb.ApdL("  INNER JOIN M_PROJECT AS p");
            sb.ApdL("    ON  p.PROJECT_NO = e.PROJECT_NO");
            sb.ApdL("  INNER JOIN M_COMMON AS ce");
            sb.ApdN("    ON  ce.GROUP_CD = '").ApdN(ESTIMATE_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("    AND ce.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND ce.VALUE1 = t.ESTIMATE_FLAG");
            sb.ApdL("  INNER JOIN M_COMMON AS cr");
            sb.ApdN("    ON  cr.GROUP_CD = '").ApdN(ESTIMATE_RATE.GROUPCD).ApdL("'");
            sb.ApdL("    AND cr.LANG = ce.LANG");
            sb.ApdL("    AND cr.ITEM_CD = ce.ITEM_CD");
            sb.ApdL("  INNER JOIN M_COMMON AS ct");
            sb.ApdN("    ON  ct.GROUP_CD = '").ApdN(TEHAI_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("    AND ct.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND ct.VALUE1 = t.TEHAI_FLAG");
            sb.ApdL("  LEFT OUTER JOIN M_COMMON AS cq");
            sb.ApdN("    ON  cq.GROUP_CD = '").ApdN(QUANTITY_UNIT.GROUPCD).ApdL("'");
            sb.ApdL("    AND cq.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND cq.VALUE1 = t.QUANTITY_UNIT");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_ESTIMATE AS m");
            sb.ApdL("    ON  t.ESTIMATE_NO = m.ESTIMATE_NO");
            sb.ApdL("  LEFT OUTER JOIN M_COMMON AS cc");
            sb.ApdN("    ON  cc.GROUP_CD = '").ApdN(CURRENCY_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("    AND cc.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND cc.VALUE1 = m.CURRENCY_FLAG");
            sb.ApdL("  LEFT OUTER JOIN (SELECT");
            sb.ApdL("                      TEHAI_RENKEI_NO");
            sb.ApdL("                    , SUM(CASE WHEN JYOTAI_FLAG >= '6' THEN 1 ELSE 0 END) AS CNT");
            sb.ApdL("                    , COUNT(*) AS CNTALL");
            sb.ApdL("                    , CASE");
            sb.ApdL("                          WHEN SUM(CASE WHEN JYOTAI_FLAG >= '6' THEN 1 ELSE 0 END) = 0 THEN 0");
            sb.ApdL("                          WHEN SUM(CASE WHEN JYOTAI_FLAG >= '6' THEN 1 ELSE 0 END) = COUNT(*) THEN 1");
            sb.ApdL("                          ELSE 2");
            sb.ApdL("                      END AS CNT_STATUS");
            sb.ApdL("                   FROM T_SHUKKA_MEISAI");
            sb.ApdL("                   GROUP BY TEHAI_RENKEI_NO) AS s");
            sb.ApdL("    ON s.TEHAI_RENKEI_NO = t.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT OUTER JOIN M_USER AS uu");
            sb.ApdN("    ON  uu.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdL("  LEFT OUTER JOIN M_COMMON AS cec");
            sb.ApdN("    ON  cec.GROUP_CD = '").ApdN(ESTIMATE_CANCEL_ROLE.GROUPCD).ApdL("'");
            sb.ApdL("  LEFT OUTER JOIN M_COMMON AS ch");
            sb.ApdN("    ON  ch.GROUP_CD = '").ApdN(HENKYAKUHIN_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("    AND ch.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND ch.VALUE1 = t.HENKYAKUHIN_FLAG");
            sb.ApdL("  LEFT OUTER JOIN M_COMMON AS cj");
            sb.ApdN("    ON  cj.GROUP_CD = '").ApdN(JYOTAI_FLAG_AR.GROUPCD).ApdL("'");
            sb.ApdN("    AND cj.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    AND cj.VALUE1 = s.CNT_STATUS");

            sb.ApdL("WHERE 1 = 1");

            // プロジェクトNo
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                sb.ApdN("   AND e.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
            }
            // 製番
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND e.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            // CODE
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND e.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            // 技連番号
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND t.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            // AR番号
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND e.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            // 納入先
            if (!string.IsNullOrEmpty(cond.Nouhinsaki))
            {
                sb.ApdN("   AND t.NOUHIN_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("NOUHIN_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("NOUHIN_SAKI", cond.Nouhinsaki+ "%"));
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.Shukkasaki))
            {
                sb.ApdN("   AND t.SYUKKA_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("SYUKKA_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_SAKI", cond.Shukkasaki + "%"));
            }

            // 手配区分
            if (cond.TehaiKubun == DISP_TEHAI_FLAG.ALL_EXCEPT_CANCEL_VALUE1)
            {
                // キャンセルを除く
                sb.ApdN("   AND t.TEHAI_FLAG <> '").ApdN(TEHAI_FLAG.CANCELLED_VALUE1).ApdL("'");
            }
            else if (cond.TehaiKubun == DISP_TEHAI_FLAG.ALL_VALUE1)
            {
                // 全て
            }
            else if (!string.IsNullOrEmpty(cond.TehaiKubun))
            {
                // 手配区分で抽出
                sb.ApdN("   AND t.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", cond.TehaiKubun));
            }
            // 有償/無償
            if (!string.IsNullOrEmpty(cond.Yusho) && cond.Yusho != DISP_ESTIMATE_FLAG.ALL_VALUE1)
            {
                sb.ApdN("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", cond.Yusho));
            }

            // 見積状況
            if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.NONE_VALUE1)
            {
                // 見積状況が未着手の時
                sb.ApdN("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND t.ESTIMATE_NO IS NULL");
            }
            else if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.ESTIMATE_VALUE1)
            {
                // 見積状況が見積の時
                sb.ApdL("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND t.ESTIMATE_NO IS NOT NULL");
                sb.ApdL("   AND m.PO_NO IS NULL ");
            }
            else if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.ORDER_VALUE1)
            {
                // 見積状況が受注の時
                sb.ApdL("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND m.ESTIMATE_NO IS NOT NULL");
                sb.ApdL("   AND m.PO_NO IS NOT NULL ");
            }
            // 見積No
            if (!string.IsNullOrEmpty(cond.MitsumoriNo))
            {
                sb.ApdN("   AND t.ESTIMATE_NO LIKE ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", cond.MitsumoriNo + "%"));
            }
            // PO No
            if (!string.IsNullOrEmpty(cond.PONo))
            {
                sb.ApdN("   AND m.PO_NO = ").ApdN(this.BindPrefix).ApdL("PO_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PO_NO", cond.PONo));
            }

            // ソート順
            sb.ApdL(" ORDER BY TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));

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
    /// 表示データ取得（出荷情報）
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>データテーブル(T_T_SHUKKA_MEISAI,出荷情報含む)</returns>
    /// <create>J.Chen 2024/03/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetTehaiEstimateMeisaiForShukkaMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("    t.TEHAI_RENKEI_NO");
            sb.ApdL("  , s.SHUKKA_DATE");
            sb.ApdL("  , s.NONYUSAKI_CD");
            sb.ApdL("  , s.TAG_NO");
            sb.ApdL("  , s.INVOICE_NO");
            sb.ApdL("  , n.SHIP");

            sb.ApdL("FROM T_TEHAI_MEISAI AS t");
            sb.ApdL("  INNER JOIN M_ECS AS e");
            sb.ApdL("    ON  e.ECS_QUOTA = t.ECS_QUOTA");
            sb.ApdL("    AND e.ECS_NO = t.ECS_NO");
            sb.ApdL("  INNER JOIN M_PROJECT AS p");
            sb.ApdL("    ON  p.PROJECT_NO = e.PROJECT_NO");

            sb.ApdL("  LEFT OUTER JOIN T_SHUKKA_MEISAI AS s");
            sb.ApdL("    ON  t.TEHAI_RENKEI_NO = s.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT OUTER JOIN M_NONYUSAKI AS n");
            sb.ApdL("    ON  s.NONYUSAKI_CD = n.NONYUSAKI_CD");
            sb.ApdL("  LEFT OUTER JOIN T_TEHAI_ESTIMATE AS m");
            sb.ApdL("    ON  t.ESTIMATE_NO = m.ESTIMATE_NO");

            sb.ApdL("WHERE 1 = 1");

            // プロジェクトNo
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                sb.ApdN("   AND e.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
            }
            // 製番
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND e.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            // CODE
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND e.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            // 技連番号
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND t.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            // AR番号
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND e.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            // 納入先
            if (!string.IsNullOrEmpty(cond.Nouhinsaki))
            {
                sb.ApdN("   AND t.NOUHIN_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("NOUHIN_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("NOUHIN_SAKI", cond.Nouhinsaki + "%"));
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.Shukkasaki))
            {
                sb.ApdN("   AND t.SYUKKA_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("SYUKKA_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_SAKI", cond.Shukkasaki + "%"));
            }

            // 手配区分
            if (cond.TehaiKubun == DISP_TEHAI_FLAG.ALL_EXCEPT_CANCEL_VALUE1)
            {
                // キャンセルを除く
                sb.ApdN("   AND t.TEHAI_FLAG <> '").ApdN(TEHAI_FLAG.CANCELLED_VALUE1).ApdL("'");
            }
            else if (cond.TehaiKubun == DISP_TEHAI_FLAG.ALL_VALUE1)
            {
                // 全て
            }
            else if (!string.IsNullOrEmpty(cond.TehaiKubun))
            {
                // 手配区分で抽出
                sb.ApdN("   AND t.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", cond.TehaiKubun));
            }
            // 有償/無償
            if (!string.IsNullOrEmpty(cond.Yusho) && cond.Yusho != DISP_ESTIMATE_FLAG.ALL_VALUE1)
            {
                sb.ApdN("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", cond.Yusho));
            }

            // 見積状況
            if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.NONE_VALUE1)
            {
                // 見積状況が未着手の時
                sb.ApdN("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND t.ESTIMATE_NO IS NULL");
            }
            else if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.ESTIMATE_VALUE1)
            {
                // 見積状況が見積の時
                sb.ApdL("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND t.ESTIMATE_NO IS NOT NULL");
                sb.ApdL("   AND m.PO_NO IS NULL ");
            }
            else if (cond.Mitsumorizyokyo == ESTIMATE_STATUS_FLAG.ORDER_VALUE1)
            {
                // 見積状況が受注の時
                sb.ApdL("   AND t.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("   AND m.ESTIMATE_NO IS NOT NULL");
                sb.ApdL("   AND m.PO_NO IS NOT NULL ");
            }
            // 見積No
            if (!string.IsNullOrEmpty(cond.MitsumoriNo))
            {
                sb.ApdN("   AND t.ESTIMATE_NO LIKE ").ApdN(this.BindPrefix).ApdL("ESTIMATE_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_NO", cond.MitsumoriNo + "%"));
            }
            // PO No
            if (!string.IsNullOrEmpty(cond.PONo))
            {
                sb.ApdN("   AND m.PO_NO = ").ApdN(this.BindPrefix).ApdL("PO_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PO_NO", cond.PONo));
            }

            // ソート順
            sb.ApdL(" ORDER BY TEHAI_RENKEI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));

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

    #region 手配見積一覧取得（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// 手配見積一覧取得（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiMeisai">データ</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>S.Furugo 2018/11/27</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行し1つのDataTableへまとめる</update>
    /// <update>J.Chen 2022/04/21 STEP14</update>
    /// --------------------------------------------------
    private DataTable GetAndLockTehaiMitsumoriMeisaiList(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiMeisai, bool isLock)
    {
        try
        {
            DataTable resultDt = null;
            List<DataTable> dtList = ChunkDataTable(dtTehaiMeisai, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文(手配明細行ロック)
                // t:手配明細、s:出荷明細(出荷済み)
                sb.ApdL("SELECT DISTINCT");
                sb.ApdL("    t.TEHAI_RENKEI_NO");
                sb.ApdL("  , CONVERT(NCHAR(27), t.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
                sb.ApdL("  , IsNull(s.CNT, 0) AS CNT");
                sb.ApdL("  , CASE uu.ROLE_ID WHEN cec.VALUE1 THEN '1' ELSE NULL END AS ESTIMATE_CANCEL_ROLE");
                sb.ApdL("  FROM T_TEHAI_MEISAI AS t");

                if (isLock)
                {
                    sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
                }
                sb.ApdN("  LEFT OUTER JOIN (SELECT TEHAI_RENKEI_NO, COUNT(*) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= '").ApdN(JYOTAI_FLAG.SHUKKAZUMI_VALUE1).ApdL("' GROUP BY TEHAI_RENKEI_NO) AS s");
                sb.ApdL("    ON s.TEHAI_RENKEI_NO = t.TEHAI_RENKEI_NO");
                sb.ApdL("  LEFT OUTER JOIN M_USER AS uu");
                sb.ApdN("    ON  uu.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
                sb.ApdL("  LEFT OUTER JOIN M_COMMON AS cec");
                sb.ApdN("    ON  cec.GROUP_CD = '").ApdN(ESTIMATE_CANCEL_ROLE.GROUPCD).ApdL("'");

                sb.ApdL(" WHERE ");
                // 手配連携No
                SetupWherePhraseTehaiRenkeiNo(sb, iNewParam, paramCollection, chunkDt, "t");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));

                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (resultDt == null)
                {
                    resultDt = dt.Clone();
                }

                foreach (DataRow dr in dt.Rows)
                {
                    resultDt.ImportRow(dr);
                }
            }

            return resultDt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region UPDATE

    #region 手配見積明細バージョン更新(有償)

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細バージョン更新(有償)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/27</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-85 対応</update>
    /// --------------------------------------------------
    public int UpdTehaiMitsumoriMeisaiYushoVersion(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dt, 1000);

            foreach (DataTable chunkDt in dtList)
            {

                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文
                sb.ApdL("UPDATE T_TEHAI_MEISAI");
                sb.ApdL("SET");
                sb.ApdN("       ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS");
                sb.ApdL("     , INVOICE_UNIT_PRICE = ");
                sb.ApdL("       CASE WHEN ESTIMATE_NO IS NULL");
                sb.ApdL("               THEN NULL");
                sb.ApdL("           ELSE INVOICE_UNIT_PRICE");
                sb.ApdL("       END");
                sb.ApdL("     , SGA_RATE = NULL");
                sb.ApdL("     , SHIPPING_RATE = NULL");
                sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
                sb.ApdL(" WHERE");

                // 手配連携No
                SetupWherePhraseTehaiRenkeiNo(sb, iNewParam, paramCollection, chunkDt, "");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region 手配見積明細バージョン更新(無償)

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細バージョン更新(無償)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/28</create>
    /// <update>M.Shimizu 2020/05/13 SQLパラメータ上限対応：1000件に分割して実行</update>
    /// <update>K.Tsutsumi 2020/06/14 EFA_DPM-85 対応</update>
    /// --------------------------------------------------
    public int UpdTehaiMitsumoriMeisaiMushoVersion(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dt, 1000);

            foreach (DataTable chunkDt in dtList)
            {

                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文
                // t:手配明細、c:汎用マスタ(手配見積レート設定)
                sb.ApdL("UPDATE t");
                sb.ApdL("SET");
                sb.ApdL("     ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
//                sb.ApdN("     ,INVOICE_UNIT_PRICE = CEILING((100 + CAST(c.VALUE3 AS decimal(9, 0))) * ");
//                sb.ApdN("         CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) *");
//                sb.ApdN("           CEILING(((100 + CAST(c.VALUE2 AS decimal(9, 0))) * IsNull(t.UNIT_PRICE, 0)) / 100)");
//                sb.ApdL("              )");
//                sb.ApdL(" / 100)");
                sb.ApdL("     , INVOICE_UNIT_PRICE = NULL"); // 無償では、未使用フィールドとする
                sb.ApdL("     , SGA_RATE = CAST(c.VALUE2 AS decimal(3, 0))");
                sb.ApdL("     , SHIPPING_RATE = CAST(c.VALUE3 AS decimal(3, 0))");
                sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
                sb.ApdL(" FROM T_TEHAI_MEISAI AS t");
                sb.ApdL(" INNER JOIN M_COMMON AS c");
                sb.ApdN(" ON c.GROUP_CD = '").ApdN(ESTIMATE_RATE.GROUPCD).ApdL("' ");
                sb.ApdN(" AND c.ITEM_CD = ").ApdN(this.BindPrefix).ApdL("ITEM_CD");
                sb.ApdN(" AND c.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
                sb.ApdL(" WHERE");
                // 手配連携No
                SetupWherePhraseTehaiRenkeiNo(sb, iNewParam, paramCollection, chunkDt, "t");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", ESTIMATE_FLAG.GRATIS_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));
                paramCollection.Add(iNewParam.NewDbParameter("ITEM_CD", ESTIMATE_RATE.GRATIS_NAME));
                paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region 手配見積明細バージョン更新(履歴)

    /// --------------------------------------------------
    /// <summary>
    /// 手配見積明細バージョン更新(履歴)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMitsumoriMeisaiRirekiVersion(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int resultCnt = 0;
            List<DataTable> dtList = ChunkDataTable(dt, 1);

            foreach (DataTable chunkDt in dtList)
            {

                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文
                // t:手配明細
                sb.ApdL("UPDATE t");
                sb.ApdL("SET");
                sb.ApdL("     ESTIMATE_RIREKI = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_RIREKI");
                sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
                sb.ApdL(" FROM T_TEHAI_MEISAI AS t");
                sb.ApdL(" WHERE");
                // 手配連携No
                SetupWherePhraseTehaiRenkeiNo(sb, iNewParam, paramCollection, chunkDt, "t");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_RIREKI", ComFunc.GetFldObject(chunkDt.Rows[0], Def_T_TEHAI_MEISAI.ESTIMATE_RIREKI)));

                resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }

            return resultCnt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    
    #endregion

    #endregion

    #endregion //T0100050:手配見積明細

    #region T0100060:手配入荷検品

    #region 制御

    #region 初期データ取得(入荷検品)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(入荷検品)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2020/05/31</create>
    /// <update>J.Chen 2022/06/03 STEP14 手配区分データ追加</update>
    /// --------------------------------------------------
    public DataSet GetInitTehaiKenpin(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            // 物件名一覧取得
            ds.Merge(this.GetBukkenNameList(dbHelper));

            using (WsCommonImpl comImpl = new WsCommonImpl())
            {
                // 検索モード
                CondCommon condCommon = new CondCommon(cond.LoginInfo);
                condCommon.GroupCD = TEHAI_NYUKA_SEARCH_MODE.GROUPCD;
                var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                dt.TableName = TEHAI_NYUKA_SEARCH_MODE.GROUPCD;
                ds.Merge(dt);

                // 手配区分
                condCommon.GroupCD = TEHAI_FLAG.GROUPCD;
                dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                // 発注/社内調達/SKS Skipのみを選択する
                string selectRows = Def_M_COMMON.VALUE1 + " = " + TEHAI_FLAG.ORDERED_VALUE1 + " or " 
                                    + Def_M_COMMON.VALUE1 + " = " + TEHAI_FLAG.SURPLUS_VALUE1 + " or "
                                    + Def_M_COMMON.VALUE1 + " = " + TEHAI_FLAG.SKS_SKIP_VALUE1 + "";
                DataRow[] dr = dt.Select(selectRows);
                dt = dr.CopyToDataTable();
                dt.TableName = TEHAI_FLAG.GROUPCD;
                ds.Merge(dt);
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region バージョン更新
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/11/09</create>
    /// <update>D.Okumura 2019/02/12 入荷検品でのまとめ発注等の1:Nを更新できない不具合を修正</update>
    /// --------------------------------------------------
    public bool UpdTehaiKenpin(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {

            // ロック
            DataTable dt = ds.Tables[Def_T_TEHAI_MEISAI.Name];
            bool sksBool = true;
            foreach (DataRow dr in dt.Rows)
            {
                // SKSSkipの入荷数のみが変更されているもの
                var sta = UtilData.GetFld(dr, ComDefine.FLD_TEHAI_FLAG_NAME);
                if (!sta.Equals(ComDefine.FLD_SKS_Skip))
                {
                    sksBool = false;
                    break;
                }
            }
            if (sksBool)
            {
                // 手配明細
                DataTable dtTM = LockTehaiMeisaiKenpin(dbHelper, cond, dt); // 手配明細
                // データを取得できるか確認する。ただし、1:NやN:1の場合があるため、
                // 件数のチェックは実施しない(以後のバージョンチェックでレコードの妥当性チェックで確認される)。
                if ((dtTM == null) || (dtTM.Rows.Count == 0))
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // バージョンチェック
                // 1:NやN:1の場合があるが、タイムスタンプは同じとなるため、画面:DB=N:1となっても下記のチェックは通る認識
                int indexTM;
                int[] notFoundIndexTM;
                indexTM = this.CheckSameData(dt, dtTM, out notFoundIndexTM, ComDefine.FLD_TM_VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                if (0 <= indexTM)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                else if (notFoundIndexTM != null)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // 更新実行
                // 同一行に対して複数回更新が行われるが、
                // 調整値で入荷数の更新を行っているため、結果として正しい値となる
                if (this.UpdTehaiMeisaiKenpin(dbHelper, cond, dt) < 0)
                    return false;
            }
            else
            {

                // 手配明細、SKS手配明細の順に行う
                // 手配明細SKS連携テーブルはロック対象外(連結テーブルでかつ、PKであるため)
                DataTable dtTM = LockTehaiMeisaiKenpin(dbHelper, cond, dt); // 手配明細
                DataTable dtTS = LockTehaiMeisaiSKSKenpin(dbHelper, cond, dt);    // SKS手配明細
                // データを取得できるか確認する。ただし、1:NやN:1の場合があるため、
                // 件数のチェックは実施しない(以後のバージョンチェックでレコードの妥当性チェックで確認される)。
                if ((dtTM == null) || (dtTM.Rows.Count == 0)
                    || (dtTS == null) || (dtTS.Rows.Count == 0))
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // バージョンチェック
                // 1:NやN:1の場合があるが、タイムスタンプは同じとなるため、画面:DB=N:1となっても下記のチェックは通る認識
                int indexTM;
                int indexTS;
                int[] notFoundIndexTM;
                int[] notFoundIndexTS;
                indexTM = this.CheckSameData(dt, dtTM, out notFoundIndexTM, ComDefine.FLD_TM_VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
                indexTS = this.CheckSameData(dt, dtTS, out notFoundIndexTS, ComDefine.FLD_TS_VERSION, Def_T_TEHAI_MEISAI_SKS.TEHAI_NO);
                if (0 <= indexTM || 0 <= indexTS)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }
                else if (notFoundIndexTM != null || notFoundIndexTS != null)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // 更新実行
                // 同一行に対して複数回更新が行われるが、
                // 調整値で入荷数の更新を行っているため、結果として正しい値となる
                if (this.UpdTehaiMeisaiKenpin(dbHelper, cond, dt) < 0)
                    return false;
                if (this.UpdTehaiMeisaiSKSKenpin(dbHelper, cond, dt) < 0)
                    return false;

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

    #region SQL

    #region SELECT

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update>T.Nukaga 2019/11/13 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
    /// <update>T.Nukaga 2019/11/19 STEP12 入荷検品表示条件緩和対応 発注状態フラグチェック削除</update>
    /// <update>Y.Shioshi 2022/05/18 STEP14 手配区分取得追加</update>
    /// <update>J.Chen 2022/06/03 STEP14 検索条件に手配区分追加、PC_ONLY条件修正</update>
    /// <update>J.Chen 2022/06/18 STEP14 印刷用の英語表記変更</update>
    /// <update>N.Ikari 2022/06/29 STEP14 SKSSkipSKSと連携を必要としない処理追加</update>
    /// --------------------------------------------------
    public DataSet GetTehaiKenpin(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // TM:手配明細、TMS:手配明細SKS、TS:手配SKS連携、MC:汎用マスタ
            sb.ApdL("SELECT");
            sb.ApdL("       TM.TEHAI_RENKEI_NO");
            sb.ApdL("     , MC.ITEM_NAME AS TEHAI_FLAG_NAME");
            //sb.ApdL("     , MCM.ITEM_NAME AS TEHAI_KIND_FLAG_NAME");
            // 印刷するとき、英語名称が長いため、VALUE2に略称を追加しました。
            sb.ApdL("     , CASE ");
            sb.ApdN("             WHEN MC.LANG = 'US' THEN MCM.VALUE2");
            sb.ApdL("             ELSE MCM.ITEM_NAME ");
            sb.ApdN("       END AS TEHAI_KIND_FLAG_NAME　");
            sb.ApdL("     , TM.NOUHIN_SAKI");
            sb.ApdL("     , TS.TEHAI_NO");
            sb.ApdL("     , M_PROJECT.BUKKEN_NAME");
            sb.ApdL("     , M_ECS.PROJECT_NO");
            sb.ApdL("     , TM.ECS_NO");
            sb.ApdL("     , M_ECS.SEIBAN");
            sb.ApdL("     , M_ECS.CODE");
            sb.ApdL("     , M_ECS.SEIBAN_CODE");
            sb.ApdL("     , M_ECS.AR_NO");
            sb.ApdL("     , CASE ");
            sb.ApdN("             WHEN TM.TEHAI_KIND_FLAG = '").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdL("' THEN 1 ");
            sb.ApdN("             WHEN TM.TEHAI_KIND_FLAG = '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdL("' THEN 1 ");
            sb.ApdN("             WHEN TM.TEHAI_FLAG != '").ApdN(TEHAI_FLAG.ORDERED_VALUE1).ApdL("' THEN 2 ");
            sb.ApdL("             ELSE 0 ");
            sb.ApdN("       END AS ").ApdL(ComDefine.FLD_PC_ONLY);
            sb.ApdL("     , TM.HINMEI_JP");
            sb.ApdL("     , TM.HINMEI");
            sb.ApdL("     , TM.ZUMEN_KEISHIKI");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN TM.HACCHU_QTY");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN TM.HACCHU_QTY");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdN("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN TM.SHUKKA_QTY");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   TS.TEHAI_QTY");
            sb.ApdL("                 END");
            sb.ApdL("        END) AS HACCHU_QTY");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdN("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN (TM.SHUKKA_QTY - TM.ARRIVAL_QTY)");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   (TS.TEHAI_QTY - TS.ARRIVAL_QTY)");
            sb.ApdL("                 END");
            sb.ApdN("        END) AS ").ApdL(ComDefine.FLD_ZAN_QTY);
            sb.ApdL("     , NULL AS ARRIVAL_QTY");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN TM.ARRIVAL_QTY");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN TM.ARRIVAL_QTY");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdN("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN TM.ARRIVAL_QTY");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   TS.ARRIVAL_QTY");
            sb.ApdL("                 END");
            sb.ApdN("        END) AS ").ApdL(ComDefine.FLD_ARRIVAL_ACTUAL_QTY);
            sb.ApdN("     , CONVERT(NCHAR(27), TM.VERSION, 121) AS ").ApdL(ComDefine.FLD_TM_VERSION);
            sb.ApdN("     , CONVERT(NCHAR(27), TS.VERSION, 121) AS ").ApdL(ComDefine.FLD_TS_VERSION);
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI TM");
            sb.ApdL("  INNER JOIN M_ECS ON TM.ECS_NO = M_ECS.ECS_NO AND TM.ECS_QUOTA = M_ECS.ECS_QUOTA");
            if (cond.TehaiKubun == TEHAI_FLAG.SKS_SKIP_VALUE1 || (string.IsNullOrEmpty(cond.TehaiKubun)))
            {
                sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI_SKS TMS ON TM.TEHAI_RENKEI_NO = TMS.TEHAI_RENKEI_NO");
                sb.ApdL("  LEFT JOIN T_TEHAI_SKS TS ON TMS.TEHAI_NO = TS.TEHAI_NO");
            }
            else
            {
                sb.ApdL("  INNER JOIN T_TEHAI_MEISAI_SKS TMS ON TM.TEHAI_RENKEI_NO = TMS.TEHAI_RENKEI_NO");
                sb.ApdL("  INNER JOIN T_TEHAI_SKS TS ON TMS.TEHAI_NO = TS.TEHAI_NO");
            }
            sb.ApdL("  LEFT JOIN M_PROJECT ON M_ECS.PROJECT_NO = M_PROJECT.PROJECT_NO");
            sb.ApdN("  LEFT JOIN M_COMMON MC ON  MC.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_GROUP_CD")
                                     .ApdN(" AND MC.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC.VALUE1 = TM.TEHAI_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MCM ON  MCM.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_KIND_FLAG_GROUP_CD")
                                     .ApdN(" AND MCM.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MCM.VALUE1 = TM.TEHAI_KIND_FLAG");
            if (cond.TehaiNyukaSearchMode == TEHAI_NYUKA_SEARCH_MODE.CORRECT_MODE_VALUE1)
            {
                sb.ApdL("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS TAG_CNT FROM T_SHUKKA_MEISAI GROUP BY TEHAI_RENKEI_NO) AS WSM1 ON WSM1.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO");
            }
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL(" LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= "
                        ).ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" GROUP BY TEHAI_RENKEI_NO) AS WSM2"
                        ).ApdL(" ON WSM2.TEHAI_RENKEI_NO = TM.TEHAI_RENKEI_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                }
            }
            sb.ApdL("WHERE 1 = 1");

            // 検索条件に手配区分追加
            if (!string.IsNullOrEmpty(cond.TehaiKubun))
            {
                if (cond.TehaiKubun == TEHAI_FLAG.ORDERED_VALUE1)
                {
                    sb.ApdN("  AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ORDERED");
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_ORDERED", TEHAI_FLAG.ORDERED_VALUE1));
                }
                if (cond.TehaiKubun == TEHAI_FLAG.SURPLUS_VALUE1)
                {
                    sb.ApdN("  AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SURPLUS");
                    paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SURPLUS", TEHAI_FLAG.SURPLUS_VALUE1));
                }
                if (cond.TehaiKubun == TEHAI_FLAG.SKS_SKIP_VALUE1)
                {
                    sb.ApdN("  AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
                }
            }

            if (cond.TehaiNyukaSearchMode == TEHAI_NYUKA_SEARCH_MODE.KENPIN_MODE_VALUE1)
            {
                sb.ApdN("  AND ((TS.TEHAI_QTY <> TS.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG NOT IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdN("')").ApdN(" AND TM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_SKS_SKIP").ApdN(" AND TM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP)");
                sb.ApdN("      OR (TM.HACCHU_QTY <> TM.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdN("')").ApdN(" AND TM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_SKS_SKIP").ApdL(")");
                sb.ApdN("      OR (TM.SHUKKA_QTY <> TM.ARRIVAL_QTY AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");

                sb.ApdL("     AND (CASE TM.TEHAI_KIND_FLAG");
                sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
                sb.ApdL("               THEN TM.HACCHU_QTY");
                sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
                sb.ApdL("               THEN TM.HACCHU_QTY");
                sb.ApdL("             ELSE ");
                sb.ApdL("               CASE ");
                sb.ApdN("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
                sb.ApdL("                   THEN TM.SHUKKA_QTY");
                sb.ApdL("                 ELSE ");
                sb.ApdL("                   TS.TEHAI_QTY");
                sb.ApdL("                 END");
                sb.ApdL("        END) <> 0");

                sb.ApdL("     AND (CASE TM.TEHAI_KIND_FLAG");
                sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
                sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
                sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
                sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
                sb.ApdL("             ELSE ");
                sb.ApdL("               CASE ");
                sb.ApdN("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
                sb.ApdL("                   THEN (TM.SHUKKA_QTY - TM.ARRIVAL_QTY)");
                sb.ApdL("                 ELSE ");
                sb.ApdL("                   (TS.TEHAI_QTY - TS.ARRIVAL_QTY)");
                sb.ApdL("                 END");
                sb.ApdN("        END) <> 0 ");

                sb.ApdL("      ))");
            }
            else
            {
                sb.ApdN("  AND ((TS.TEHAI_QTY = TS.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG NOT IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdN("') AND TAG_CNT IS NULL AND TM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP)");
                sb.ApdN("      OR (TM.HACCHU_QTY = TM.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdL("') AND TAG_CNT IS NULL)");
                sb.ApdN("      OR (TM.SHUKKA_QTY = TM.ARRIVAL_QTY AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
                sb.ApdL("      ))");
            }
            if (cond.TehaiKubun != TEHAI_FLAG.SKS_SKIP_VALUE1 && (!string.IsNullOrEmpty(cond.TehaiKubun)))
                sb.ApdN("  AND TS.KENPIN_UMU = '").ApdN(KENPIN_UMU.ON_VALUE1).ApdL("'");

            if (!string.IsNullOrEmpty(cond.Nouhinsaki))
            {
                sb.ApdN("   AND TM.NOUHIN_SAKI LIKE ").ApdN(this.BindPrefix).ApdL("NOUHIN_SAKI");
                paramCollection.Add(iNewParam.NewDbParameter("NOUHIN_SAKI", cond.Nouhinsaki + "%"));

            }
            if (!string.IsNullOrEmpty(cond.TehaiNo))
            {
                sb.ApdN("   AND TS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));
            }
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                    sb.ApdL("   AND TM.SHUKKA_QTY - IsNull(WSM2.CNT, 0) > 0");
                }
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                }
                else
                {
                    sb.ApdN("   AND M_ECS.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
                }
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                // ECS_No複数検索処理
                string[] array = cond.EcsNo.Split(',');

                sb.ApdN("   AND TM.ECS_NO IN ( ");
                for (int i = 0; i < array.Count(); i++)
                {
                    if (i < (array.Count() - 1))
                    {
                        sb.ApdN(this.BindPrefix).ApdL("ECS_NO" + i + ", ");
                    }
                    else
                    {
                        sb.ApdN(this.BindPrefix).ApdL("ECS_NO" + i + ")");
                    }
                }
                for (int j = 0; j < array.Count(); j++)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("ECS_NO" + j, array[j]));
                }
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND M_ECS.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", "AR" + cond.ARNo));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND M_ECS.SEIBAN LIKE ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND M_ECS.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            sb.ApdL(" ORDER BY TM.NOUHIN_SAKI,TS.TEHAI_NO, TM.ECS_NO");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_AGGRIGATE", TEHAI_KIND_FLAG.AGGRIGATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_ESTIMATE", TEHAI_KIND_FLAG.ESTIMATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_GROUP_CD", TEHAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_GROUP_CD", TEHAI_KIND_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SKS_SKIP", TEHAI_FLAG.SKS_SKIP_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEHAI_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region CSV表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// CSV表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <create>T.SASAYAMA 2023/07/04</create>
    /// --------------------------------------------------
    public DataSet GetTehaiKenpinCsv(DatabaseHelper dbHelper, CondT01 cond, string[] values)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // TM:手配明細、TMS:手配明細SKS、TS:手配SKS連携、MC:汎用マスタ
            sb.ApdL("SELECT");
            sb.ApdL("       TM.TEHAI_RENKEI_NO");
            sb.ApdL("     , MC.ITEM_NAME AS TEHAI_FLAG_NAME");
            //sb.ApdL("     , MCM.ITEM_NAME AS TEHAI_KIND_FLAG_NAME");
            // 印刷するとき、英語名称が長いため、VALUE2に略称を追加しました。
            sb.ApdL("     , CASE ");
            sb.ApdN("             WHEN MC.LANG = 'US' THEN MCM.VALUE2");
            sb.ApdL("             ELSE MCM.ITEM_NAME ");
            sb.ApdN("       END AS TEHAI_KIND_FLAG_NAME　");
            sb.ApdL("     , TM.NOUHIN_SAKI");
            sb.ApdL("     , TS.TEHAI_NO");
            sb.ApdL("     , M_PROJECT.BUKKEN_NAME");
            sb.ApdL("     , M_ECS.PROJECT_NO");
            sb.ApdL("     , TM.ECS_NO");
            sb.ApdL("     , M_ECS.SEIBAN");
            sb.ApdL("     , M_ECS.CODE");
            sb.ApdL("     , M_ECS.SEIBAN_CODE");
            sb.ApdL("     , M_ECS.AR_NO");
            sb.ApdL("     , CASE ");
            sb.ApdN("             WHEN TM.TEHAI_KIND_FLAG = '").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdL("' THEN 1 ");
            sb.ApdN("             WHEN TM.TEHAI_KIND_FLAG = '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdL("' THEN 1 ");
            sb.ApdN("             WHEN TM.TEHAI_FLAG != '").ApdN(TEHAI_FLAG.ORDERED_VALUE1).ApdL("' THEN 2 ");
            sb.ApdL("             ELSE 0 ");
            sb.ApdN("       END AS ").ApdL(ComDefine.FLD_PC_ONLY);
            sb.ApdL("     , TM.HINMEI_JP");
            sb.ApdL("     , TM.HINMEI");
            sb.ApdL("     , TM.ZUMEN_KEISHIKI");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN TM.HACCHU_QTY");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN TM.HACCHU_QTY");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdL("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN TM.SHUKKA_QTY");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   TS.TEHAI_QTY");
            sb.ApdL("                 END");
            sb.ApdL("        END) AS HACCHU_QTY");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN (TM.HACCHU_QTY - TM.ARRIVAL_QTY)");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdL("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN (TM.SHUKKA_QTY - TM.ARRIVAL_QTY)");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   (TS.TEHAI_QTY - TS.ARRIVAL_QTY)");
            sb.ApdL("                 END");
            sb.ApdN("        END) AS ").ApdL(ComDefine.FLD_ZAN_QTY);
            sb.ApdL("     , NULL AS ARRIVAL_QTY");
            sb.ApdL("     , (CASE TM.TEHAI_KIND_FLAG");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdL("               THEN TM.ARRIVAL_QTY");
            sb.ApdN("             WHEN ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdL("               THEN TM.ARRIVAL_QTY");
            sb.ApdL("             ELSE ");
            sb.ApdL("               CASE ");
            sb.ApdL("                 WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                   THEN TM.ARRIVAL_QTY");
            sb.ApdL("                 ELSE ");
            sb.ApdL("                   TS.ARRIVAL_QTY");
            sb.ApdL("                 END");
            sb.ApdN("        END) AS ").ApdL(ComDefine.FLD_ARRIVAL_ACTUAL_QTY);
            sb.ApdN("     , CONVERT(NCHAR(27), TM.VERSION, 121) AS ").ApdL(ComDefine.FLD_TM_VERSION);
            sb.ApdN("     , CONVERT(NCHAR(27), TS.VERSION, 121) AS ").ApdL(ComDefine.FLD_TS_VERSION);
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI TM");
            sb.ApdL("  INNER JOIN M_ECS ON TM.ECS_NO = M_ECS.ECS_NO AND TM.ECS_QUOTA = M_ECS.ECS_QUOTA");

            sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI_SKS TMS ON TM.TEHAI_RENKEI_NO = TMS.TEHAI_RENKEI_NO");
            sb.ApdL("  LEFT JOIN T_TEHAI_SKS TS ON TMS.TEHAI_NO = TS.TEHAI_NO");
            
            sb.ApdL("  LEFT JOIN M_PROJECT ON M_ECS.PROJECT_NO = M_PROJECT.PROJECT_NO");
            sb.ApdN("  LEFT JOIN M_COMMON MC ON  MC.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_FLAG_GROUP_CD")
                                     .ApdN(" AND MC.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC.VALUE1 = TM.TEHAI_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON MCM ON  MCM.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("TEHAI_KIND_FLAG_GROUP_CD")
                                     .ApdN(" AND MCM.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MCM.VALUE1 = TM.TEHAI_KIND_FLAG");

            sb.ApdL("WHERE 1 = 1");

            if (values.Length > 0)
            {
                sb.ApdN("  AND (");
                for (int i = 0; i < values.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.ApdN("   OR ");
                    }
                    string paramName = "TEHAI_NO" + i.ToString();
                    sb.ApdN("TS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL(paramName);
                    paramCollection.Add(iNewParam.NewDbParameter(paramName, values[i]));
                }
                sb.ApdL(")");
            }

            sb.ApdN("  AND ((TS.TEHAI_QTY <> TS.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG NOT IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdL("'))");
            sb.ApdN("      OR (TM.HACCHU_QTY <> TM.ARRIVAL_QTY AND TM.TEHAI_KIND_FLAG IN ('").ApdN(TEHAI_KIND_FLAG.AGGRIGATE_VALUE1).ApdN("', '").ApdN(TEHAI_KIND_FLAG.ESTIMATE_VALUE1).ApdL("'))");
            sb.ApdN("      OR (TM.SHUKKA_QTY <> TM.ARRIVAL_QTY AND TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("      ))");

            sb.ApdL(" ORDER BY TM.NOUHIN_SAKI,TS.TEHAI_NO, TM.ECS_NO");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_AGGRIGATE", TEHAI_KIND_FLAG.AGGRIGATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_ESTIMATE", TEHAI_KIND_FLAG.ESTIMATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_GROUP_CD", TEHAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_GROUP_CD", TEHAI_KIND_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SKS_SKIP", TEHAI_FLAG.SKS_SKIP_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEHAI_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// c
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukkenNameList(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT PROJECT_NO");
            sb.ApdL("      ,BUKKEN_NAME");
            sb.ApdL("FROM M_PROJECT");
            sb.ApdL("ORDER BY BUKKEN_NAME");
            sb.ApdL("        ,PROJECT_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PROJECT.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region SKS最終連携日時取得
    /// --------------------------------------------------
    /// <summary>
    /// SKS最終連携日時取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>S.Furugo 2018/11/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSksLastLink(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT SHORI_FLAG");
            sb.ApdL("      ,LASTEST_DATE");
            sb.ApdL("      ,START_TIME");
            sb.ApdL("FROM M_SKS");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SKS.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 手配明細一覧取得（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細一覧取得（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>DataTable</returns>
    /// <create>D.Naito 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockTehaiMeisaiKenpin(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文(手配明細行ロック)
            sb.ApdL("SELECT");
            sb.ApdL("      TEHAI_RENKEI_NO");
            sb.ApdN("    , CONVERT(NCHAR(27), VERSION, 121) AS ").ApdL(ComDefine.FLD_TM_VERSION);
            sb.ApdL("FROM T_TEHAI_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE 1=0");

            int i = 0;
            foreach (DataRow dr in dtTM.Rows)
            {
                sb.ApdN("    OR TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO" + "_" + i.ToString());
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO" + "_" + i.ToString(), ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                i++;
            }
            sb.ApdL("ORDER BY NOUHIN_SAKI, ECS_NO");

            // SQL実行
            DataTable dt = new DataTable();
            dt.TableName = Def_T_TEHAI_MEISAI.Name;
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
    /// SKS手配明細一覧取得（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <returns>DataTable</returns>
    /// <create>D.Naito 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockTehaiMeisaiSKSKenpin(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTS)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文(SKS手配明細行ロック)
            sb.ApdL("SELECT");
            sb.ApdL("      TEHAI_NO");
            sb.ApdN("    , CONVERT(NCHAR(27), VERSION, 121) AS ").ApdL(ComDefine.FLD_TS_VERSION);
            sb.ApdL("FROM T_TEHAI_SKS");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE 1=0");

            int i = 0;
            foreach (DataRow dr in dtTS.Rows)
            {
                sb.ApdN("   OR TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO" + "_" + i.ToString());
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO" + "_" + i.ToString(), ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS.TEHAI_NO)));
                i++;
            }

            sb.ApdL("ORDER BY TEHAI_NO");

            // SQL実行
            DataTable dt = new DataTable();
            dt.TableName = Def_T_TEHAI_MEISAI_SKS.Name;
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

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdTehaiMeisaiKenpin(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       ARRIVAL_QTY = ").ApdN("ARRIVAL_QTY + ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ARRIVAL_QTY)));

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

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>S.Furugo 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdTehaiMeisaiSKSKenpin(DatabaseHelper dbHelper, CondT01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            // TS:手配SKS連携、TM:手配明細、TMS:手配明細SKS
            sb.ApdL("UPDATE TS");
            sb.ApdL("SET");
            sb.ApdL("     ARRIVAL_QTY = ");
            sb.ApdN("      CASE WHEN TM.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_AGGRIGATE");
            sb.ApdN("             THEN ").ApdN(this.BindPrefix).ApdL("ARRIVAL_ZERO");
            sb.ApdN("           WHEN TM.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_FLAG_ESTIMATE");
            sb.ApdN("             THEN ").ApdN(this.BindPrefix).ApdL("ARRIVAL_ZERO");
            sb.ApdN("           ELSE ").ApdN("TS.ARRIVAL_QTY + ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY");
            sb.ApdL("      END");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" FROM");
            sb.ApdL("      T_TEHAI_SKS TS");
            sb.ApdL("      INNER JOIN T_TEHAI_MEISAI_SKS TMS ON TMS.TEHAI_NO = TS.TEHAI_NO");
            sb.ApdL("      INNER JOIN T_TEHAI_MEISAI TM ON TM.TEHAI_RENKEI_NO = TMS.TEHAI_RENKEI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_AGGRIGATE", TEHAI_KIND_FLAG.AGGRIGATE_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_FLAG_ESTIMATE", TEHAI_KIND_FLAG.ESTIMATE_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_SKS.TEHAI_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.ARRIVAL_QTY)));
                paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_ZERO", "0"));

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

    #endregion

    #endregion

    #region T0100070:組立実績登録

    #region 制御

    #region 組立実績更新

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdKumitateJisseki(DatabaseHelper dbHelper, CondT01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtUpdate = ds.Tables[Def_T_TEHAI_MEISAI.Name];

            // ロック＆取得
            DataTable dtTehaiMeisai = this.LockKumitateJisseki(dbHelper, dtUpdate);

            // 手配明細データのバージョンチェック
            int[] notFoundIndex = null;
            var index = this.CheckSameData(dtUpdate, dtTehaiMeisai, out notFoundIndex, Def_T_TEHAI_MEISAI.VERSION, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO);
            if (0 <= index)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            foreach (DataRow dr in dtTehaiMeisai.Rows)
            {
                // 算出後の組立数がマイナスにならないかどうかチェック
                var newAssyQty = ComFunc.GetFldToDecimal(dr, ComDefine.FLD_NEW_ASSY_QTY);
                if (newAssyQty < 0)
                {
                    // {0}行目の組立数をマイナスにすることはできません。
                    errMsgID = "T0100070001";
                    args = new string[] { ComFunc.GetFld(dr, ComDefine.FLD_ROW_INDEX) };
                    return false;
                }
                var tehaiQty = ComFunc.GetFldToDecimal(dr, Def_T_TEHAI_MEISAI.TEHAI_QTY);
                if (tehaiQty < newAssyQty)
                {
                    // {0}行目の組立数を手配数より多くすることはできません。
                    errMsgID = "T0100070005";
                    args = new string[] { ComFunc.GetFld(dr, ComDefine.FLD_ROW_INDEX) };
                    return false;
                }
                // 既に連携が始まっていた場合は実績数未満にならないかどうかチェック
                var cntRenkei = ComFunc.GetFldToDecimal(dr, ComDefine.FLD_CNT);
                if (cntRenkei > 0M)
                {
                    if (cntRenkei > newAssyQty)
                    {
                        // {0}行目の組立数をTAG連携実績数より少なくすることはできません。
                        errMsgID = "T0100070002";
                        args = new string[] { ComFunc.GetFld(dr, ComDefine.FLD_ROW_INDEX) };
                        return false;
                    }
                }
            }

            // 手配明細の更新
            this.UpdKumitateJisseki(dbHelper, cond, dtUpdate);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #endregion

    #region SQL

    #region 物件名一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 物件名取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tajimi 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukken(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_BUKKEN.BUKKEN_NO");
            sb.ApdL("     , M_BUKKEN.BUKKEN_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN");
            sb.ApdL(" WHERE");
            sb.ApdN("       M_BUKKEN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       M_BUKKEN.BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_BUKKEN.Name);

            return ds.Tables[Def_M_BUKKEN.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #region 組立実績データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>組立実績データ</returns>
    /// <create>H.Tajimi 2018/11/07</create>
    /// <update>T.Nukaga 2019/11/11 検索条件追加対応(全て(AR未出荷)、全て(AR))</update>
    /// --------------------------------------------------
    public DataSet GetKumitateJisseki(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       0 AS SELECT_CHK");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.SETTEI_DATE");
            sb.ApdL("     , M_PROJECT.BUKKEN_NAME");
            sb.ApdL("     , M_ECS.SEIBAN");
            sb.ApdL("     , M_ECS.CODE");
            sb.ApdL("     , M_ECS.AR_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.ZUMEN_KEISHIKI");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI_JP");
            sb.ApdL("     , T_TEHAI_MEISAI.HINMEI");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_QTY");
            sb.ApdL("     , (T_TEHAI_MEISAI.TEHAI_QTY - T_TEHAI_MEISAI.ASSY_QTY) AS REMAIN_QTY");
            sb.ApdL("     , 0 AS ASSY_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN M_ECS");
            sb.ApdL("         ON M_ECS.ECS_QUOTA = T_TEHAI_MEISAI.ECS_QUOTA");
            sb.ApdL("        AND M_ECS.ECS_NO = T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL(" INNER JOIN M_PROJECT");
            sb.ApdN("         ON M_PROJECT.PROJECT_NO = M_ECS.PROJECT_NO");
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL(" LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE JYOTAI_FLAG >= "
                        ).ApdN(this.BindPrefix).ApdN("JYOTAI_FLAG_SHUKKAZUMI").ApdN(" GROUP BY TEHAI_RENKEI_NO) AS WSM2"
                        ).ApdL(" ON WSM2.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG_SHUKKAZUMI", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));

                }
            }
            sb.ApdL(" WHERE");
            sb.ApdN("       T_TEHAI_MEISAI.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG");
            sb.ApdL("   AND (T_TEHAI_MEISAI.TEHAI_QTY - T_TEHAI_MEISAI.ASSY_QTY) > 0");
            if (!string.IsNullOrEmpty(cond.AssyRange) && !string.IsNullOrEmpty(cond.SetteiDate))
            {
                if (cond.AssyRange == TEHAI_ASSY_DATE_RANGE.BEFORE_VALUE1)
                {
                    sb.ApdN("   AND T_TEHAI_MEISAI.SETTEI_DATE <= ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE");
                    paramCollection.Add(iNewParam.NewDbParameter("SETTEI_DATE", cond.SetteiDate));
                }
                else if (cond.AssyRange == TEHAI_ASSY_DATE_RANGE.EQUALS_VALUE1)
                {
                    sb.ApdN("   AND T_TEHAI_MEISAI.SETTEI_DATE = ").ApdN(this.BindPrefix).ApdL("SETTEI_DATE");
                    paramCollection.Add(iNewParam.NewDbParameter("SETTEI_DATE", cond.SetteiDate));
                }
            }
            if (!string.IsNullOrEmpty(cond.ProjectNo))
            {
                if (cond.ProjectNo == ComDefine.COMBO_ALL_MISHUKKA_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                    sb.ApdL("   AND T_TEHAI_MEISAI.SHUKKA_QTY - IsNull(WSM2.CNT, 0) > 0");
                }
                else if (cond.ProjectNo == ComDefine.COMBO_ALL_AR_VALUE)
                {
                    sb.ApdL("   AND M_ECS.AR_NO IS NOT NULL ");
                    sb.ApdL("   AND LTRIM(RTRIM(M_ECS.AR_NO)) <> '' ");
                }
                else
                {
                    sb.ApdN("   AND M_PROJECT.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
                    paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
                }
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND T_TEHAI_MEISAI.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND M_ECS.SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND M_ECS.CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code));
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND M_ECS.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       T_TEHAI_MEISAI.SETTEI_DATE");
            sb.ApdL("     , M_PROJECT.BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG", TEHAI_FLAG.ASSY_VALUE1));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEHAI_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #region 組立実績データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtTehaiMeisai">手配明細データ</param>
    /// <returns>組立実績データ</returns>
    /// <create>H.Tajimi 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockKumitateJisseki(DatabaseHelper dbHelper, DataTable dtTehaiMeisai)
    {
        try
        {
            DataTable ret = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("     , T_TEHAI_MEISAI.TEHAI_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.ASSY_QTY");
            sb.ApdN("     , (T_TEHAI_MEISAI.ASSY_QTY + ").ApdN(this.BindPrefix).ApdL("ASSY_QTY) AS NEW_ASSY_QTY");
            sb.ApdL("     , T_TEHAI_MEISAI.SETTEI_DATE");
            sb.ApdL("     , T_TEHAI_MEISAI.ECS_NO");
            sb.ApdL("     , (SELECT COUNT(1)");
            sb.ApdL("          FROM T_SHUKKA_MEISAI");
            sb.ApdL("         WHERE T_SHUKKA_MEISAI.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO) AS CNT");
            sb.ApdL("     , T_TEHAI_MEISAI.VERSION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ROW_INDEX AS ROW_INDEX");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_TEHAI_MEISAI.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow dr in dtTehaiMeisai.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ASSY_QTY", ComFunc.GetFldToDecimal(dr, Def_T_TEHAI_MEISAI.ASSY_QTY)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_INDEX", ComFunc.GetFldToInt32(dr, ComDefine.FLD_ROW_INDEX)));

                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

                var dtTemp = new DataTable();
                // SQL実行
                dbHelper.Fill(sb.ToString(), paramCollection, dtTemp);

                ret.Merge(dtTemp);
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #endregion

    #region UPDATE

    #region 組立実績データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 組立実績データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="dtTehaiMeisai">手配明細データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKumitateJisseki(DatabaseHelper dbHelper, CondT01 cond, DataTable dtTehaiMeisai)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       ASSY_QTY = (ASSY_QTY + ").ApdN(this.BindPrefix).ApdL("ASSY_QTY)");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp); ;
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow dr in dtTehaiMeisai.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ASSY_QTY", ComFunc.GetFldToDecimal(dr, Def_T_TEHAI_MEISAI.ASSY_QTY)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFld(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #endregion

    #endregion

    #region T0100080:Ship照会

    #region 制御

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>K.Tsutsumi 2019/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet Ctrl_GetShukkaMeisai(DatabaseHelper dbHelper, CondT01 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            var ds = new DataSet();

            // 手配明細取得
            ds.Merge(Sql_GetTehaiMeisai(dbHelper, cond).Tables[Def_T_TEHAI_MEISAI.Name]);
            if (!UtilData.ExistsData(ds, Def_T_TEHAI_MEISAI.Name))
            {
                // 手配情報が取得できませんでした。
                errMsgID = "T0100080001";
                return ds;
            }

            // 出荷明細取得
            ds.Merge(Sql_GetShukkaMeisai(dbHelper, cond).Tables[Def_T_SHUKKA_MEISAI.Name]);
            if (!UtilData.ExistsData(ds, Def_T_TEHAI_MEISAI.Name))
            {
                // 出荷情報が取得できませんでした。
                errMsgID = "T0100080002";
                return ds;
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SQL実行

    #region SELECT

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>K.Tsutsumi 2019/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet Sql_GetShukkaMeisai(DatabaseHelper dbHelper, CondT01 cond)
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
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TSM.FILE_NAME");
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
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TTM.MAKER");
            sb.ApdL("     , TTM.UNIT_PRICE");
            sb.ApdL("     , MC_EF.ITEM_NAME AS ESTIMATE_FLAG_NAME");
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
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 手配連携No
            if (!string.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                fieldName = "TEHAI_RENKEI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TehaiRenkeiNo));
            }

            // ソート
            sb.ApdL(" ORDER BY");
            sb.ApdL("        SHUKA_DATE");
            sb.ApdL("     ,  SHIP_AR_NO");
            sb.ApdL("     , TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));

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

    #region 手配明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>K.Tsutsumi 2019/03/09</create>
    /// <update>K.Tsutsumi 2020/06/14 EFA_SMS-85 対応</update>
    /// --------------------------------------------------
    public DataSet Sql_GetTehaiMeisai(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "TTM.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TTM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TTM.ECS_QUOTA");
            sb.ApdL("     , TTM.ECS_NO");
            sb.ApdL("     , TTM.SETTEI_DATE");
            sb.ApdL("     , TTM.NOUHIN_SAKI");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , TTM.ZUMEN_OIBAN");
            sb.ApdL("     , TTM.FLOOR");
            sb.ApdL("     , TTM.ST_NO");
            sb.ApdL("     , TTM.HINMEI_JP");
            sb.ApdL("     , TTM.HINMEI");
            sb.ApdL("     , TTM.HINMEI_INV");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI_S");
            sb.ApdL("     , TTM.TEHAI_QTY");
            sb.ApdL("     , TTM.TEHAI_FLAG");
            sb.ApdL("     , TTM.TEHAI_KIND_FLAG");
            sb.ApdL("     , TTM.HACCHU_QTY");
            sb.ApdL("     , TTM.SHUKKA_QTY");
            sb.ApdL("     , TTM.FREE1");
            sb.ApdL("     , TTM.FREE2");
            sb.ApdL("     , TTM.QUANTITY_UNIT");
            sb.ApdL("     , TTM.ZUMEN_KEISHIKI2");
            sb.ApdL("     , TTM.NOTE");
            sb.ApdL("     , TTM.MAKER");
            sb.ApdL("     , TTM.UNIT_PRICE");
//            sb.ApdL("     , TTM.INVOICE_UNIT_PRICE");
            sb.ApdL("     , CASE ");
            sb.ApdN("           WHEN TTM.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG_ONEROUS THEN TTM.INVOICE_UNIT_PRICE");
            sb.ApdN("           WHEN TTM.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdN("ESTIMATE_FLAG_GRATIS THEN CEILING((100 + TTM.SHIPPING_RATE) * CEILING(IsNull(").ApdN(this.BindPrefix).ApdL("RATE_JPY, 1) * CEILING(((100 + TTM.SGA_RATE) * IsNull(TTM.UNIT_PRICE, 0)) / 100)) / 100) ");
            sb.ApdL("           ELSE TTM.INVOICE_UNIT_PRICE");
            sb.ApdL("       END AS INVOICE_UNIT_PRICE");
            sb.ApdL("     , TTM.ARRIVAL_QTY");
            sb.ApdL("     , TTM.ASSY_QTY");
            sb.ApdL("     , TTM.ESTIMATE_FLAG");
            sb.ApdL("     , TTM.ESTIMATE_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI TTM");
            sb.ApdL("INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO");
            sb.ApdL("INNER JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");

            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 手配連携No
            if (!string.IsNullOrEmpty(cond.TehaiRenkeiNo))
            {
                fieldName = "TEHAI_RENKEI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TehaiRenkeiNo));
            }

            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_ONEROUS", ESTIMATE_FLAG.ONEROUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG_GRATIS", ESTIMATE_FLAG.GRATIS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RATE_JPY", ComDefine.RATE_JPY));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEHAI_MEISAI.Name);

            return ds;

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

    #endregion

    #region　Privateメソッド

    /// --------------------------------------------------
    /// <summary>
    /// 指定されたサイズにデータテーブルを分割します。
    /// </summary>
    /// <param name="dt">元データテーブル</param>
    /// <param name="chunkSize">分割するサイズ</param>
    /// <returns>指定されたデータテーブル</returns>
    /// <create>M.Shimizu  2020/05/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private List<DataTable> ChunkDataTable(DataTable dt, int chunkSize)
    {
        List<DataTable> dtList = new List<DataTable>();

        IEnumerable<IEnumerable<DataRow>> collection = Chunk<DataRow>(dt.AsEnumerable(), chunkSize);

        foreach (IEnumerable<DataRow> rows in collection)
        {
            DataTable dtn = dt.Clone();

            foreach (DataRow dr in rows)
            {
                dtn.ImportRow(dr);
            }

            dtList.Add(dtn);
        }

        return dtList;
    }

    /// --------------------------------------------------
    /// <summary>
    /// 指定されたサイズに分割します。
    /// </summary>
    /// <typeparam name="T">ジェネリック型</typeparam>
    /// <param name="source">元データ</param>
    /// <param name="chunkSize">分割するサイズ</param>
    /// <returns>分割されたリスト</returns>
    /// <create>M.Shimizu 2020/05/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunkSize)
    {
        while (source.Any())
        {
            yield return source.Take(chunkSize);
            source = source.Skip(chunkSize);
        }
    }

    #endregion Privateメソッド

    #region T0100090:VALUE照会

    #region SQL実行

    #region SELECT

    #region 制御

    #region 表示データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>T_TEHAI_ESTIMATEを含むデータセット</returns>
    /// <create>J.Chen 2024/02/15</create>
    /// <update>J.Chen 2024/11/05 インボイスNo改行コード除外</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiMitsumoriValue(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(GetTehaiEstimateValue(dbHelper, cond));

            string[] invoiceNumbers = ds.Tables[Def_T_TEHAI_ESTIMATE.Name].AsEnumerable()
                                     .Select(row => row.Field<string>(Def_T_SHUKKA_MEISAI.INVOICE_NO))
                                     .Where(invoice => !string.IsNullOrEmpty(invoice)) 
                                     .SelectMany(invoice => invoice.Split(','))
                                     .Select(invoice => invoice.Trim())
                                     .OrderBy(invoice => invoice) // INVOICE_NOを昇順で並び替える
                                     .Distinct() // 重複を除去する
                                     .ToArray();
            for (int i = 0; i < invoiceNumbers.Length; i++)
            {
                DataColumn newColumn = new DataColumn(invoiceNumbers[i], typeof(decimal));
                newColumn.DefaultValue = 0;
                ds.Tables[Def_T_TEHAI_ESTIMATE.Name].Columns.Add(newColumn);
            }

            // InvoiceValue取得
            foreach (DataRow dr in ds.Tables[Def_T_TEHAI_ESTIMATE.Name].Rows)
            {
                var PONo = ComFunc.GetFld(dr, Def_T_TEHAI_ESTIMATE.PO_NO);
                DataSet dsTmp = new DataSet();

                if (!string.IsNullOrEmpty(PONo))
                {
                    dsTmp.Tables.Add(GetInvoiceValueForPONo(dbHelper, PONo));
                    if (ComFunc.IsExistsData(dsTmp, PONo))
                    {
                        foreach (DataRow drTmp in dsTmp.Tables[PONo].Rows)
                        {
                            var invoiceNo = ComFunc.GetFld(drTmp, Def_T_SHUKKA_MEISAI.INVOICE_NO);
                            if (string.IsNullOrEmpty(invoiceNo)) continue;
                            invoiceNo = invoiceNo.Replace("\n", "");
                            invoiceNo = invoiceNo.Replace("\r", "");
                            dr[invoiceNo] = ComFunc.GetFldToDecimal(drTmp, Def_T_TEHAI_MEISAI.INVOICE_VALUE);
                        }
                    }
                }
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

    #region SQL

    #region 表示データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns>データテーブル(見積情報)</returns>
    /// <create>J.Chen 2024/02/15</create>
    /// <update>J.Jeong 2024/07/19 通貨関連下段ラベル追加</update>
    /// --------------------------------------------------
    private DataTable GetTehaiEstimateValue(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_ESTIMATE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 各見積の仕切り金額を集計する
            sb.ApdL("WITH TOTAL_AMOUNT AS (");
            sb.ApdL("    SELECT");
            sb.ApdL("        TTE.PO_NO");
            sb.ApdL("      , SUM(PARTITION_AMOUNT) AS TOTAL_PARTITION_AMOUNT");
            sb.ApdL("    FROM");
            sb.ApdL("        (SELECT");
            sb.ApdL("            TTE.PO_NO");
            sb.ApdL("          , TTE.ESTIMATE_NO");
            sb.ApdL("          , CEILING(SUM(");
            sb.ApdL("                CASE");
            sb.ApdN("                    WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("SKS_SKIP_VALUE").ApdL(" THEN TTM.SHUKKA_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("                    ELSE TTM.HACCHU_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("                END");
            sb.ApdL("            ) * MAX(TTE.RATE_JPY) * MAX(TTE.RATE_PARTITION)) AS PARTITION_AMOUNT");
            sb.ApdL("        FROM T_TEHAI_ESTIMATE AS TTE");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI TTM ON TTE.ESTIMATE_NO = TTM.ESTIMATE_NO");
            sb.ApdL("        INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO");
            sb.ApdL("        WHERE 1 = 1");
            sb.ApdN("        AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdL("        AND TTE.PO_NO IS NOT NULL");
            sb.ApdL("        AND TTE.PO_NO <> ''");
            sb.ApdL("        GROUP BY TTE.PO_NO,TTE.ESTIMATE_NO) AS TTE");
            sb.ApdL("    GROUP BY TTE.PO_NO");
            sb.ApdL(")");

            // SQL文
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("    CONCAT(ROW_NUMBER() OVER (ORDER BY MAX(TE.UPDATE_DATE)), '回目') AS CNT");
            sb.ApdL("  , TE.PO_NO");
            sb.ApdL("  , SUM(ISNULL(TE.PO_AMOUNT, 0)) AS PO_AMOUNT");
            sb.ApdL("  , MAX(ISNULL(TE.RATE_JPY, 0)) AS RATE_JPY");
            sb.ApdL("  , MAX(ISNULL(TE.ITEM_NAME, '')) AS CURRENCY_FLAG");
            //sb.ApdL("  , SUM(TE.PARTITION_AMOUNT) AS PARTITION_AMOUNT");
            sb.ApdL("  , FORMAT(MAX(ISNULL(TE.PARTITION_AMOUNT, 0)), '#,0') AS PARTITION_AMOUNT");
            sb.ApdL("  , STUFF((");
            sb.ApdL("      SELECT DISTINCT");
            sb.ApdL("              ',' + SM.INVOICE_NO");
            sb.ApdL("       FROM");
            sb.ApdL("              T_TEHAI_ESTIMATE TE_INNER");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI TM ON TE_INNER.ESTIMATE_NO = TM.ESTIMATE_NO");
            sb.ApdL("        INNER JOIN T_SHUKKA_MEISAI SM ON TM.TEHAI_RENKEI_NO = SM.TEHAI_RENKEI_NO");
            sb.ApdL("       WHERE");
            sb.ApdL("              TE_INNER.PO_NO = TE.PO_NO");
            sb.ApdL("         FOR XML PATH ('')");
            sb.ApdL("     ),1,1,'') AS INVOICE_NO");
            sb.ApdL("  , SUM(ISNULL(TE.INVOICE_VALUE, 0)) AS USED_VALUE");
            sb.ApdL("  , NULL AS REMAINING_VALUE");
            sb.ApdL("  , MAX(ISNULL(TE.UPDATE_DATE, NULL)) AS UPDATE_DATE");

            sb.ApdL("FROM (");
            sb.ApdL("    SELECT ");
            sb.ApdL("        TTE.ESTIMATE_NO");
            sb.ApdL("      , TTM.TEHAI_RENKEI_NO");
            sb.ApdL("      , TTE.PO_NO");
            sb.ApdL("      , SUM(");
            sb.ApdL("          CASE");
            sb.ApdN("            WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdN("SKS_SKIP_VALUE").ApdL(" THEN TTM.SHUKKA_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("            ELSE TTM.HACCHU_QTY * CEILING(CEILING(CEILING(TTM.UNIT_PRICE * (TTE.SALSES_PER + 100) / 100) / TTE.RATE_JPY) * (TTE.ROB_PER + 100) / 100)");
            sb.ApdL("          END");
            sb.ApdL("        ) AS PO_AMOUNT");
            sb.ApdL("      , TTE.RATE_JPY");
            sb.ApdL("      , MC.ITEM_NAME");
            sb.ApdL("      , TTE.RATE_PARTITION");
            sb.ApdL("      , MAX(TA.TOTAL_PARTITION_AMOUNT) AS PARTITION_AMOUNT");
            sb.ApdL("      , TTM.INVOICE_VALUE");
            sb.ApdL("      , TTE.UPDATE_DATE");
            sb.ApdL("    FROM T_TEHAI_ESTIMATE AS TTE");
            sb.ApdL("     INNER JOIN T_TEHAI_MEISAI TTM ON TTE.ESTIMATE_NO = TTM.ESTIMATE_NO");
            sb.ApdL("     INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO ");
            sb.ApdL("      LEFT JOIN TOTAL_AMOUNT TA ON TTE.PO_NO = TA.PO_NO ");
            sb.ApdL("      LEFT JOIN M_COMMON MC");
            sb.ApdN("         ON MC.GROUP_CD = '").ApdN(CURRENCY_FLAG.GROUPCD).ApdL("'");
            sb.ApdL("        AND MC.VALUE1 = TTE.CURRENCY_FLAG");
            sb.ApdN("        AND MC.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("    WHERE 1 = 1");
            sb.ApdN("      AND ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdL("      AND TTE.PO_NO IS NOT NULL");
            sb.ApdL("      AND TTE.PO_NO <> ''");
            //sb.ApdN("      AND TTM.TEHAI_FLAG <> ").ApdN(this.BindPrefix).ApdL("CANCELLED_VALUE"); // キャンセル除外
            sb.ApdL("    GROUP BY");
            sb.ApdL("        TTE.ESTIMATE_NO");
            sb.ApdL("      , TTM.TEHAI_RENKEI_NO");
            sb.ApdL("      , TTE.PO_NO");
            sb.ApdL("      , TTE.RATE_JPY");
            sb.ApdL("      , MC.ITEM_NAME");
            sb.ApdL("      , TTE.RATE_PARTITION");
            sb.ApdL("      , TTM.INVOICE_VALUE");
            sb.ApdL("      , TTE.UPDATE_DATE");
            sb.ApdL("     )TE");

            sb.ApdL(" GROUP BY TE.PO_NO");
            sb.ApdL(" ORDER BY UPDATE_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));
            paramCollection.Add(iNewParam.NewDbParameter("SKS_SKIP_VALUE", TEHAI_FLAG.SKS_SKIP_VALUE1));
            //paramCollection.Add(iNewParam.NewDbParameter("CANCELLED_VALUE", TEHAI_FLAG.CANCELLED_VALUE1)); // キャンセル除外
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

    #region Invoice Value取得
    /// --------------------------------------------------
    /// <summary>
    /// Invoice Value取得
    /// </summary>
    /// <param name="dbHelper">dbHelper</param>
    /// <param name="PONo">PONo</param>
    /// <create>J.Chen 2024/02/15</create>
    /// <update>J.Chen 2024/03/18 分割時不具合修正</update>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetInvoiceValueForPONo(DatabaseHelper dbHelper, string PONo)
    {
        try
        {
            DataTable dt = new DataTable(PONo);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("   INVOICE_NO");
            sb.ApdL(" , SUM(INVOICE_VALUE) AS INVOICE_VALUE");
            sb.ApdL("FROM ( ");
            sb.ApdL("    SELECT DISTINCT");
            sb.ApdL("       SM.INVOICE_NO");
            sb.ApdL("     , SM.TEHAI_RENKEI_NO");
            sb.ApdL("     , SUM(SM.NUM * CEILING(CEILING(CEILING(TM.UNIT_PRICE * (TE_INNER.SALSES_PER + 100) / 100) / TE_INNER.RATE_JPY) * (TE_INNER.ROB_PER + 100) / 100)) AS INVOICE_VALUE");
            sb.ApdL("    FROM T_TEHAI_ESTIMATE TE_INNER");
            sb.ApdL("    INNER JOIN T_TEHAI_MEISAI TM ON TE_INNER.ESTIMATE_NO = TM.ESTIMATE_NO");
            sb.ApdL("    INNER JOIN (");
            sb.ApdL("        SELECT TEHAI_RENKEI_NO, INVOICE_NO, NUM");
            sb.ApdL("        FROM T_SHUKKA_MEISAI");
            sb.ApdL("    ) SM ON TM.TEHAI_RENKEI_NO = SM.TEHAI_RENKEI_NO");
            sb.ApdL("     WHERE 1 = 1");
            sb.ApdN("       AND TE_INNER.PO_NO = ").ApdN(this.BindPrefix).ApdL("PO_NO"); ;
            sb.ApdL("     GROUP BY SM.INVOICE_NO, SM.TEHAI_RENKEI_NO");
            sb.ApdL(") AS SUBQUERY ");
            sb.ApdL("GROUP BY INVOICE_NO ");
            sb.ApdL("ORDER BY INVOICE_NO ");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PO_NO", PONo));
            paramCollection.Add(iNewParam.NewDbParameter("SKS_SKIP_VALUE", TEHAI_FLAG.SKS_SKIP_VALUE1));

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

    #endregion

    #endregion

    #endregion

    #region T0100100:手配明細履歴

    #region SQL実行

    #region SELECT

    #region 手配明細履歴取得
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細履歴取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">T01用コンディション</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/11/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiMeisaiRireki(DatabaseHelper dbHelper, CondT01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            //SQL文追加
            sb.ApdL("SELECT");
            sb.ApdL("       MTMR.UPDATE_DATE");
            sb.ApdL("     , MTMR.UPDATE_USER_NAME");
            sb.ApdL("     , CASE");
            sb.ApdL("           WHEN MC.LANG = 'JP' THEN '連携No[' + MTMR.TEHAI_RENKEI_NO + '] '");
            sb.ApdL("           ELSE 'Linkage Number[' + MTMR.TEHAI_RENKEI_NO + '] '");
            sb.ApdL("       END + MC.ITEM_NAME AS NAIYO");
            sb.ApdL("  FROM");
            sb.ApdL("        M_TEHAI_MEISAI_RIREKI MTMR");
            sb.ApdN("  LEFT JOIN M_COMMON MC ON  MC.GROUP_CD = ").ApdN(this.BindPrefix).ApdN("PROCUREMENT_ENTRY_STATUS_GROUP_CD")
                                     .ApdN(" AND MC.LANG = ").ApdN(this.BindPrefix).ApdN("LANG")
                                     .ApdL(" AND MC.VALUE1 = MTMR.STATUS_FLAG");
            sb.ApdL(" WHERE MTMR.ECS_QUOTA = @ECS_QUOTA");
            sb.ApdL("   AND MTMR.ECS_NO = @ECS_NO");

            sb.ApdL(" ORDER BY");
            sb.ApdL("       MTMR.UPDATE_DATE");
            sb.ApdL("       DESC");

            //パラメータに値代入
            pc.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            pc.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));
            pc.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            pc.Add(iNewParam.NewDbParameter("PROCUREMENT_ENTRY_STATUS_GROUP_CD", PROCUREMENT_ENTRY_STATUS.GROUPCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), pc, ds, Def_M_TEHAI_MEISAI_RIREKI.Name);

            return ds;

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
}
