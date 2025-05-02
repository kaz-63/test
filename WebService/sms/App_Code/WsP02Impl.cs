using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// 共通ﾀﾞｲｱﾛｸﾞ処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsP02Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsP02Impl()
        : base()
    {
    }

    #endregion

    #region P0200010:納入先一覧

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 納入先一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNonyusakiIchiran(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldPrefix = string.Empty;
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;
            fieldPrefix = "MN.";

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       MN.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , MN.KANRI_FLAG");
            sb.ApdL("     , COM2.ITEM_NAME AS KANRI_FLAG_NAME");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            sb.ApdL("     , MN.LIST_FLAG_NAME0");
            sb.ApdL("     , MN.LIST_FLAG_NAME1");
            sb.ApdL("     , MN.LIST_FLAG_NAME2");
            sb.ApdL("     , MN.LIST_FLAG_NAME3");
            sb.ApdL("     , MN.LIST_FLAG_NAME4");
            sb.ApdL("     , MN.LIST_FLAG_NAME5");
            sb.ApdL("     , MN.LIST_FLAG_NAME6");
            sb.ApdL("     , MN.LIST_FLAG_NAME7");
            // @@@ ↑
            sb.ApdL("     , MN.VERSION");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL("     , MN.LOCK_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = MN.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'KANRI_FLAG'");
            sb.ApdL("                         AND COM2.VALUE1 = MN.KANRI_FLAG");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI SM ON SM.TAG_SHIP IS NOT NULL");
            sb.ApdL("                         AND SM.NONYUSAKI_CD = MN.NONYUSAKI_CD");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            // 管理フラグ
            if (!string.IsNullOrEmpty(cond.KanriFlag))
            {
                fieldName = "KANRI_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.KanriFlag));
            }
            // 出荷フラグ
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 出荷便
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Ship + "%"));
            }
            // TAG便
            if (!string.IsNullOrEmpty(cond.TagShip))
            {
                fieldName = "TAG_SHIP";
                sb.ApdN("   AND ").ApdN("SM." + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.TagShip + "%"));
            }
            // 納入先
            if (!string.IsNullOrEmpty(cond.NonyusakiName))
            {
                fieldPrefix = "BK.";
                fieldName = "BUKKEN_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);

                string nsnm = Regex.Replace(cond.NonyusakiName, @"[%_\[]", "[$0]");
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, nsnm + "%"));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BK.BUKKEN_NAME");
            sb.ApdL("     , MN.SHIP");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 履歴データに紐付く物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetRirekiBukkenIchiran(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT TR.NONYUSAKI_CD");
            sb.ApdL("      ,MB.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("      ,TR.SHIP");
            sb.ApdL("  FROM T_RIREKI TR");
            sb.ApdL(" INNER JOIN M_BUKKEN MB");
            sb.ApdL("         ON MB.SHUKKA_FLAG = TR.SHUKKA_FLAG");
            sb.ApdL("        AND MB.BUKKEN_NO = TR.BUKKEN_NO");
            sb.ApdL(" WHERE 1 = 1");
            // 出荷フラグ
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND TR.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            // 納入先
            if (!string.IsNullOrEmpty(cond.NonyusakiName))
            {
                sb.ApdN("   AND MB.BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");

                string nsnm = Regex.Replace(cond.NonyusakiName, @"[%_\[]", "[$0]");
                paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NAME", nsnm + "%"));
            }
            // 出荷便
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                sb.ApdN("   AND TR.SHIP LIKE ").ApdN(this.BindPrefix).ApdL("SHIP");
                paramCollection.Add(iNewParameter.NewDbParameter("SHIP", cond.Ship + "%"));
            }
            // 画面区分
            if (!string.IsNullOrEmpty(cond.GamenFlag))
            {
                sb.ApdN("   AND TR.GAMEN_FLAG = ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("GAMEN_FLAG", cond.GamenFlag));
            }
            sb.ApdL(" GROUP BY TR.NONYUSAKI_CD");
            sb.ApdL("         ,TR.SHIP");
            sb.ApdL("         ,MB.BUKKEN_NAME");
            sb.ApdL(" ORDER BY MB.BUKKEN_NAME");
            sb.ApdL("         ,TR.SHIP");

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region P0200030:工事識別一覧

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">木枠データ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKojiShikibetsuIchiran(DatabaseHelper dbHelper, CondKiwaku cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldPrefix = string.Empty;
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , KOJI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , TOROKU_FLAG");
            sb.ApdL("     , SAGYO_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU T1");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            // 登録区分
            if (!string.IsNullOrEmpty(cond.TorokuFlag))
            {
                fieldName = "TOROKU_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.TorokuFlag));
            }
            // 工事識別名称
            if (!string.IsNullOrEmpty(cond.KojiName))
            {
                fieldName = "KOJI_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.KojiName + "%"));
            }
            // 出荷便
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Ship + "%"));
            }
            // 木枠梱包
            if (cond.KiwakuKonpo)
            {
                sb.ApdL("   AND");
                sb.ApdL("   NOT EXISTS(");
                sb.ApdL("SELECT ");
                sb.ApdL("       1");
                sb.ApdL("  FROM ");
                sb.ApdL("       T_SHUKKA_MEISAI T2");
                sb.ApdL(" WHERE ");
                sb.ApdL("       T2.KOJI_NO = T1.KOJI_NO");
                sb.ApdL("       )");

            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       KOJI_NAME");
            sb.ApdL("     , SHIP");

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

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region P0200040:物件名一覧

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">物件データ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Sakiori 2012/04/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukkenNameIchiran(DatabaseHelper dbHelper, CondBukken cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT BUKKEN_NAME");
            sb.ApdL("      ,SHUKKA_FLAG");
            sb.ApdL("      ,BUKKEN_NO");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdL(" WHERE 1 = 1");
            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            // 物件名
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");

                string bknm = Regex.Replace(cond.BukkenName, @"[%_\[]", "[$0]");
                paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NAME", bknm + "%"));
            }
            sb.ApdL(" ORDER BY BUKKEN_NAME");
            sb.ApdL("         ,BUKKEN_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_BUKKEN.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region P0200050:履歴照会

    #region SELECT

    #region 処理名コンボボックスデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 処理名コンボボックスデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetOperationFlag(DatabaseHelper dbHelper, CondP02 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT ITEM_NAME");
            sb.ApdN("      ,VALUE1");
            sb.ApdL("  FROM M_COMMON");
            sb.ApdN(" WHERE GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD");
            sb.ApdN("   AND VALUE2 IN (").ApdN(this.BindPrefix).ApdL("ALL_VALUE, ").ApdN(this.BindPrefix).ApdL("VALUE2)");
            sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" ORDER BY DISP_NO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("GROUP_CD", OPERATION_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("ALL_VALUE", OPERATION_FLAG.ALL_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("VALUE2", cond.Value2));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_COMMON.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 履歴一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 履歴一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetRireki(DatabaseHelper dbHelper, CondP02 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT CONVERT(NVARCHAR, TR.UPDATE_DATE, 111) + ' ' + CONVERT(NVARCHAR, TR.UPDATE_DATE, 108) AS UPDATE_DATE");
            sb.ApdL("      ,TR.UPDATE_USER_NAME");
            sb.ApdL("      ,MC.ITEM_NAME AS OPERATION_FLAG");
            sb.ApdL("  FROM T_RIREKI TR");
            sb.ApdL("  LEFT JOIN M_COMMON MC");
            sb.ApdN("         ON MC.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG_GROUPCD");
            sb.ApdL("        AND MC.VALUE1 = TR.OPERATION_FLAG");
            sb.ApdN("        AND MC.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE TR.GAMEN_FLAG = ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
            sb.ApdN("   AND TR.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TR.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND TR.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParameter.NewDbParameter("AR_NO", cond.ARNo));
            }
            if (!string.IsNullOrEmpty(cond.UpdateUserName))
            {
                sb.ApdN("   AND TR.UPDATE_USER_NAME LIKE ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", cond.UpdateUserName + "%"));
            }
            if (cond.OperationFlag != OPERATION_FLAG.ALL_VALUE1)
            {
                sb.ApdN("   AND TR.OPERATION_FLAG = ").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("OPERATION_FLAG", cond.OperationFlag));
            }
            if (cond.UpdateDateFrom != null)
            {
                sb.ApdN("   AND TR.UPDATE_DATE >= ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE_FROM");
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE_FROM", cond.UpdateDateFrom));
            }
            if (cond.UpdateDateTo != null)
            {
                sb.ApdN("   AND TR.UPDATE_DATE <= ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE_TO");
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE_TO", cond.UpdateDateTo));
            }
            sb.ApdL(" ORDER BY TR.UPDATE_DATE DESC, TR.OPERATION_FLAG DESC");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("OPERATION_FLAG_GROUPCD", OPERATION_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("GAMEN_FLAG", cond.GamenFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCd));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_RIREKI.Name);
            return ds;  
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 締めマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 締めマスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/06/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShime(DatabaseHelper dbHelper)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
 
            sb.ApdL("SELECT HOJIKIKAN");
            sb.ApdL("  FROM M_SHIME");

            dbHelper.Fill(sb.ToString(), ds, Def_M_SHIME.Name);
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

    #region P0200060:送信先設定

    /// --------------------------------------------------
    /// <summary>
    /// 送信先設定
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">P02用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetSendUser(DatabaseHelper dbHelper, CondP02 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_USER.Name);
            var sb = new StringBuilder();

            sb.ApdL("SELECT USER_ID");
            sb.ApdL("     , USER_NAME");
            sb.ApdL("     , MAIL_ADDRESS");
            sb.ApdL("  FROM M_USER");
            sb.ApdL(" WHERE MAIL_ADDRESS IS NOT NULL");
            sb.ApdL(" ORDER BY USER_ID");

            dbHelper.Fill(sb.ToString(), dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion
}
