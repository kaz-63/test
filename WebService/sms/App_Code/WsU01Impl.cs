using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

//// --------------------------------------------------
/// <summary>
/// 受入作業処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsU01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsU01Impl()
        : base()
    {
    }

    #endregion

    #region U0100010:受入情報登録
    
    #region 受入データ取得制御

    #region 画面ヘッダ部に表示する情報を保持するテーブルの作成

    /// --------------------------------------------------
    /// <summary>
    /// 画面ヘッダ部に表示する情報を保持するテーブルの作成
    /// </summary>
    /// <param name="dt">チェックで使用したAR情報を含むデータテーブル</param>
    /// <returns>画面ヘッダ部に表示する情報を保持するテーブル</returns>
    /// <create>K.Tsutsumi 2011/03/10</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CreateAddition(DataTable dt)
    {
        DataTable dtAddition = new DataTable(ComDefine.DTTBL_ADDITION);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_NAME, dt.Columns[ComDefine.FLD_ADDITION_DISP_NAME].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_SHIP, dt.Columns[ComDefine.FLD_ADDITION_DISP_SHIP].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_DISP_AR_NO, dt.Columns[ComDefine.FLD_ADDITION_DISP_AR_NO].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_TSM_AR_NO, dt.Columns[ComDefine.FLD_ADDITION_TSM_AR_NO].DataType);
        dtAddition.Columns.Add(ComDefine.FLD_ADDITION_UKEIRE_DATE, dt.Columns[ComDefine.FLD_ADDITION_UKEIRE_DATE].DataType);
        DataRow dr = dtAddition.NewRow();

        dr[ComDefine.FLD_ADDITION_DISP_NAME] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_DISP_NAME);
        dr[ComDefine.FLD_ADDITION_DISP_SHIP] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_DISP_SHIP);
        dr[ComDefine.FLD_ADDITION_DISP_AR_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_DISP_AR_NO);
        dr[ComDefine.FLD_ADDITION_TSM_AR_NO] = ComFunc.GetFldObject(dt, 0, ComDefine.FLD_ADDITION_TSM_AR_NO);
        dr[ComDefine.FLD_ADDITION_UKEIRE_DATE] = ComFunc.GetFldObject(dt, 0, Def_T_BOXLIST_MANAGE.UKEIRE_DATE);
        dtAddition.Rows.Add(dr);
        return dtAddition;
    }

    #endregion

    #region BOX単位
    /// --------------------------------------------------
    /// <summary>
    /// BoxNoから明細を取得します
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxData(DatabaseHelper dbHelper, CondU01 cond, ref string errMsgID, ref string[] args)
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

            foreach (DataRow dr in dtCheck.Rows)
            {
                // 木枠データの作業区分チェック
                string sagyoFlag = ComFunc.GetFld(dr, Def_T_KIWAKU.SAGYO_FLAG);
                if (!string.IsNullOrEmpty(sagyoFlag) && sagyoFlag != SAGYO_FLAG.KONPOKANRYO_VALUE1 && sagyoFlag != SAGYO_FLAG.SHUKKAZUMI_VALUE1)
                {
                    // 木枠梱包未完了です。梱包登録で確認して下さい。
                    errMsgID = "U0100010008";
                    break;
                }

                // 受入日のチェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.UKEIRE_DATE)))
                {
                    // 受入済BoxNo.です。
                    errMsgID = "U0100010004";
                }
            }

            DataSet ds = this.GetBoxDataExec(dbHelper, cond);
            if (!ds.Tables.Contains(Def_T_SHUKKA_MEISAI.Name) || ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Count < 1)
            {
                // 該当明細無し
                errMsgID = "A9999999022";
                return null;
            }

            // 2011/03/10 K.Tsutsumi Add T_ARが存在しなくても続行可能
            ds.Tables.Add(this.CreateAddition(dtCheck));
            // ↑

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット単位
    /// --------------------------------------------------
    /// <summary>
    /// パレットNoから明細を取得します
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// --------------------------------------------------
    public DataSet GetPalletData(DatabaseHelper dbHelper, CondU01 cond, ref string errMsgID, ref string[] args)
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

            foreach (DataRow dr in dtCheck.Rows)
            {
                // 木枠データの作業区分チェック
                string sagyoFlag = ComFunc.GetFld(dr, Def_T_KIWAKU.SAGYO_FLAG);
                if (!string.IsNullOrEmpty(sagyoFlag) && sagyoFlag != SAGYO_FLAG.KONPOKANRYO_VALUE1 && sagyoFlag != SAGYO_FLAG.SHUKKAZUMI_VALUE1)
                {
                    // 木枠梱包未完了です。梱包登録で確認して下さい。
                    errMsgID = "U0100010008";
                    break;
                }

                // 受入日のチェック
                if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.UKEIRE_DATE)))
                {
                    // 受入済パレットNo.です。
                    errMsgID = "U0100010007";
                }
            }

            DataSet ds = this.GetPalletDataExec(dbHelper, cond);
            if (!ds.Tables.Contains(Def_T_SHUKKA_MEISAI.Name) || ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Count < 1)
            {
                // 該当明細無し
                errMsgID = "A9999999022";
                return null;
            }
            // 2011/03/10 K.Tsutsumi Add T_ARが存在しなくても続行可能
            ds.Tables.Add(this.CreateAddition(dtCheck));
            // ↑

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region アップデート制御

    /// --------------------------------------------------
    /// <summary>
    /// 受入保存
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/09/04</create>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public bool UpdUkeireData(DatabaseHelper dbHelper, CondU01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataSet ds = this.GetUkeireData(dbHelper, cond);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                return false;
            }
            if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            if (cond.UkeireNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
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
            }
            else if (cond.UkeireNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                DataSet dsPallet = this.GetPalletData(dbHelper, cond, ref errMsgID, ref args);
                errMsgID = string.Empty;
                int[] notFoundIndex;
                if (0 <= this.CheckSameData(dt, dsPallet.Tables[Def_T_SHUKKA_MEISAI.Name], out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, Def_T_SHUKKA_MEISAI.BOX_NO) || notFoundIndex != null)
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

            if (ComFunc.GetFld(dt, 0, ComDefine.FLD_BTN_STATE) == cond.TextShukka)
            {
                // 出荷処理
                UpdShukkaData(dbHelper, cond, ds.Tables[Def_T_SHUKKA_MEISAI.Name], ref errMsgID, ref args);
            }
            else
            {
                // 受入処理
                UpdShukkaUkeireData(dbHelper, cond, ds.Tables[Def_T_SHUKKA_MEISAI.Name], ref errMsgID, ref args);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 出荷保存
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/09/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdShukkaData(DatabaseHelper dbHelper, CondU01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtPallet;
            DataTable dtBox;
            // 上位階層のデータもマージして取得する。
            GetRelateTable(dbHelper, dt, out dtPallet, out dtBox);

            // 出荷明細データ更新 
            this.UpdShukkaMeisaiShukka(dbHelper, cond, dt);

            // AR情報データ更新 
            this.UpdARShukka(dbHelper, cond, dt);

            // ボックスリスト管理データ更新
            this.UpdBoxListManageShukka(dbHelper, cond, dtBox);

            // パレットリスト管理データ更新
            this.UpdPalletListManageShukka(dbHelper, cond, dtPallet);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 関連テーブル取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">データ</param>
    /// <param name="merged">上位階層からのデータをマージするか</param>
    /// <param name="dt">木枠データ</param>
    /// <param name="dt">パレットデータ</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    private void GetRelateTable(DatabaseHelper dbHelper, DataTable dt, out DataTable dtPallet, out DataTable dtBox)
    {
        // 出荷対象データを集める
        DataView dv = dt.DefaultView;

        dtPallet = dv.ToTable(Def_T_PALLETLIST_MANAGE.Name, true,
            Def_T_SHUKKA_MEISAI.PALLET_NO,
            Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE);
        dtPallet.Columns[Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE].ColumnName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;

        dtBox = dv.ToTable(Def_T_BOXLIST_MANAGE.Name, true,
            Def_T_SHUKKA_MEISAI.BOX_NO,
            Def_T_SHUKKA_MEISAI.BOXKONPO_DATE);
        dtBox.Columns[Def_T_SHUKKA_MEISAI.BOXKONPO_DATE].ColumnName = Def_T_SHUKKA_MEISAI.SHUKKA_DATE;
    }

    /// --------------------------------------------------
    /// <summary>
    /// 出荷受入保存
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/09/04</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdShukkaUkeireData(DatabaseHelper dbHelper, CondU01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            bool ret = false;

            CondI01 condI01 = new CondI01(cond.LoginInfo);
            condI01.LoginInfo = cond.LoginInfo.Clone();
            condI01.ZaikoNo = cond.UkeireNo;
            condI01.WorkUserID = cond.LoginInfo.UserID;
            condI01.StockDate = cond.UkeireDate.ToString();

            using (WsI01Impl impl = new WsI01Impl())
            {
                ret = impl.UpdShukkaUkeire(dbHelper, condI01, dt, ref errMsgID, ref args);
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

    #region SELECT

    #region BOX単位

    /// --------------------------------------------------
    /// <summary>
    /// BoxNoから共通のデータとチェック項目の取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetCheckBoxData(DatabaseHelper dbHelper, CondU01 cond)
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
            sb.ApdL("     , TBM.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS DISP_NAME");
            sb.ApdL("     , MNS.SHIP AS DISP_SHIP");
            sb.ApdL("     , TAR.AR_NO AS DISP_AR_NO");
            sb.ApdL("     , TBM.UKEIRE_DATE AS UKEIRE_DATE");
            sb.ApdL("     , TSM.AR_NO AS TSM_AR_NO");
            sb.ApdL("     , KW.SAGYO_FLAG");
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
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TBM.BOX_NO");
            sb.ApdL("     , TBM.SHUKKA_DATE");
            sb.ApdL("     , TBM.UKEIRE_DATE");
            sb.ApdL("     , TBM.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TAR.JP_UNSOKAISHA_NAME");
            sb.ApdL("     , TAR.JP_OKURIJYO_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , KW.SAGYO_FLAG");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.KOJI_NO ASC");
            sb.ApdL("     , PALLET_NO ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.UkeireNo));

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
    /// BOXNoから出荷明細を取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/06</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    public DataSet GetBoxDataExec(DatabaseHelper dbHelper, CondU01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("CASE WHEN TSM.SHUKKA_DATE IS NULL THEN '{0}' WHEN TSM.UKEIRE_DATE IS NULL THEN '{1}' ELSE '{2}' END AS {3}",
                                                cond.TextKonpozumi,
                                                cond.TextShukka,
                                                cond.TextUkeire,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
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
            sb.ApdL("     , TSM.UKEIRE_DATE");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN T_BOXLIST_MANAGE TBM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
           // sb.ApdL("                               AND TSM.JYOTAI_FLAG = '6'");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.UkeireNo));
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

    #region パレット単位

    /// --------------------------------------------------
    /// <summary>
    /// パレットNoから共通のデータとチェック項目の取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// --------------------------------------------------
    private DataTable GetCheckPalletData(DatabaseHelper dbHelper, CondU01 cond)
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
            sb.ApdL("     , TPM.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS DISP_NAME");
            sb.ApdL("     , MNS.SHIP AS DISP_SHIP");
            sb.ApdL("     , TAR.AR_NO AS DISP_AR_NO");
            sb.ApdL("     , TPM.UKEIRE_DATE AS UKEIRE_DATE");
            sb.ApdL("     , TSM.AR_NO AS TSM_AR_NO");
            sb.ApdL("     , KW.SAGYO_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN T_PALLETLIST_MANAGE TPM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TPM.PALLET_NO");
            sb.ApdL("     , TPM.SHUKKA_DATE");
            sb.ApdL("     , TPM.UKEIRE_DATE");
            sb.ApdL("     , TPM.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , TAR.JP_UNSOKAISHA_NAME");
            sb.ApdL("     , TAR.JP_OKURIJYO_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , KW.SAGYO_FLAG");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.KOJI_NO ASC");
            sb.ApdL("     , PALLET_NO ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.UkeireNo));

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
    /// パレットNoから出荷明細を取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/06</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public DataSet GetPalletDataExec(DatabaseHelper dbHelper, CondU01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("CASE WHEN TSM2.SHUKKA_DATE IS NULL THEN '{0}' WHEN TSM2.UKEIRE_DATE IS NULL THEN '{1}' ELSE '{2}' END AS {3}",
                                                cond.TextKonpozumi,
                                                cond.TextShukka,
                                                cond.TextUkeire,
                                                ComDefine.FLD_BTN_STATE);

            string buttonDetail = string.Format("'{0}' AS {1}", cond.TextShosai, ComDefine.FLD_BTN_DETAIL);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
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
            sb.ApdL("     , TSM2.UKEIRE_DATE");
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
            sb.ApdL("                  , UKEIRE_DATE");
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
            sb.ApdL("                  , MAX(UKEIRE_DATE) AS UKEIRE_DATE");
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
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.UkeireNo));

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

    #region 受入データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 受入データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <returns>受入データ</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetUkeireData(DatabaseHelper dbHelper, CondU01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            string stockNo = cond.UkeireNo;
            if (stockNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                sb.ApdN("   AND SM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", stockNo));
            }
            else if (stockNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("   AND SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", stockNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

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

    #region UPDATE

    #region チェック

    /// --------------------------------------------------
    /// <summary>
    /// Box受入で使用する受け入れ済みのチェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckBoxDataUkeireDate(DatabaseHelper dbHelper, CondU01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.UkeireNo));

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
    /// パレット受入で使用する出荷済みのチェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="isBoxNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckPalletDataUkeireDate(DatabaseHelper dbHelper, CondU01 cond, bool isBoxNo)
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
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdN("     , ").ApdL(unitField);
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdN("     , ").ApdL(unitField);

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.UkeireNo));

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
    /// Box受入で使用するパレットマネージの受け入れ済みのチェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable CheckPalletDataBoxNoUkeireDate(DatabaseHelper dbHelper, CondU01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , COUNT(1) AS CNT");
            sb.ApdL("     , COUNT(TSM.UKEIRE_DATE) AS UKEIRE_DATE");
            sb.ApdL("     , TPM.UKEIRE_USER_ID");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL(" WHERE ");
            sb.ApdL("      EXISTS (");
            sb.ApdL("              SELECT");
            sb.ApdL("                     1");
            sb.ApdL("               FROM");
            sb.ApdL("                     T_SHUKKA_MEISAI TSM2 ");
            sb.ApdL("               WHERE");
            sb.ApdN("                     TSM2.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL("                 AND TSM2.PALLET_NO = TPM.PALLET_NO");
            sb.ApdL("      )");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TPM.UKEIRE_USER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.UkeireNo));

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

    #region パレットリスト管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <param name="dt">パレットデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletListManageShukka(DatabaseHelper dbHelper, CondU01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_PALLETLIST_MANAGE TPM");
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.PALLET_NO = TPM.PALLET_NO");
            sb.ApdL("                      AND TSM.UKEIRE_DATE IS NOT NULL");
            sb.ApdL("                  )");
            sb.ApdN("   AND UKEIRE_DATE IS NOT NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));

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

    #region BOXリスト管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxListManageShukka(DatabaseHelper dbHelper, CondU01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE TBM");
            sb.ApdL("SET");
            sb.ApdN("       UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_BOXLIST_MANAGE TBM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.BOX_NO = TBM.BOX_NO");
            sb.ApdL("                      AND TSM.UKEIRE_DATE IS NOT NULL");
            sb.ApdL("                  )");
            sb.ApdN("   AND UKEIRE_DATE IS NOT NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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

    #region AR情報更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdARShukka(DatabaseHelper dbHelper, CondU01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE AR");
            sb.ApdL("SET");
            sb.ApdN("       AR.UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , AR.UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , AR.UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , AR.UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , AR.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , AR.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , AR.VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR AR");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON SM.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("                              AND SM.AR_NO = AR.AR_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("   AND AR.UKEIRE_DATE IS NOT NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #region 出荷明細更新
    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細出荷更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">U01用コンディション</param>
    /// <param name="dt">ボックスデータ</param>
    /// <returns>更新件数</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiShukka(DatabaseHelper dbHelper, CondU01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , UKEIRE_DATE = ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , UKEIRE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , UKEIRE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("   AND UKEIRE_DATE IS NOT NULL");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHUKKAZUMI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #region U0100020:受入情報明細

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 明細取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/20</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxMeisai(DatabaseHelper dbHelper, CondU01 cond)
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
            //sb.ApdL("       MNS.NONYUSAKI_CD");
            sb.ApdL("       BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            //sb.ApdL("     , MNS.KANRI_FLAG");
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
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.M_NO");
            //sb.ApdL("     , TSM.JYOTAI_FLAG");
            //sb.ApdL("     , TSM.SHUKA_DATE");
            //sb.ApdL("     , TSM.BOX_NO");
            //sb.ApdL("     , TSM.BOXKONPO_DATE");
            //sb.ApdL("     , TSM.PALLET_NO");
            //sb.ApdL("     , TSM.PALLETKONPO_DATE");
            //sb.ApdL("     , TSM.KOJI_NO");
            //sb.ApdL("     , TSM.CASE_ID");
            //sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            //sb.ApdL("     , TSM.SHUKKA_DATE");
            //sb.ApdL("     , TSM.UNSOKAISHA_NAME");
            //sb.ApdL("     , TSM.INVOICE_NO");
            //sb.ApdL("     , TSM.OKURIJYO_NO");
            //sb.ApdL("     , TSM.BL_NO");
            //sb.ApdL("     , TSM.UKEIRE_DATE");
            //sb.ApdL("     , TSM.UKEIRE_USER_NAME");
            //sb.ApdL("     , TSM.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 納入先コード
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // Box No.
            if (!string.IsNullOrEmpty(cond.UkeireNo))
            {
                fieldName = "BOX_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.UkeireNo));
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
}
