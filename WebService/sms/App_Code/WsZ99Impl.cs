using System;
using System.Data;
using System.Text;
using Commons;
using Condition;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// ツール処理クラス（トランザクション層）
/// </summary>
/// <create>T.Sakiori 2012/04/09</create>
/// <update></update>
/// --------------------------------------------------
public class WsZ99Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsZ99Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region Z9900010:物件名マスタ作成

    #region 物件名マスタデータ作成

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタデータ作成
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">引数</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsBukken(DatabaseHelper dbHelper, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dt = this.GetBaseData(dbHelper);
            foreach (DataRow dr in dt.Rows)
            {
                var cond = new CondSms(new LoginInfo());
                cond.CreateUserID = "system";
                cond.CreateUserName = "sysadmin2";
                cond.UpdateUserID = "system";
                cond.UpdateUserName = "sysadmin2";
                cond.MainteUserID = "system";
                cond.MainteUserName = "sysadmin2";
                if (ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    cond.SaibanFlag = SAIBAN_FLAG.BKUS_VALUE1;
                }
                else if (ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHUKKA_FLAG) == SHUKKA_FLAG.AR_VALUE1)
                {
                    cond.SaibanFlag = SAIBAN_FLAG.BKARUS_VALUE1;
                }
                string saiban;
                using (var impl = new WsSmsImpl())
                {
                    if (!impl.GetSaiban(dbHelper, cond, out saiban, out errMsgID))
                    {
                        return false;
                    }
                }
                this.InsBukkenExec(dbHelper, dr, saiban, cond);
                this.UpdNonyusaki(dbHelper, dr, saiban, cond);
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

    #endregion

    #region SQL実行

    #region Z9900010:出荷計画明細登録

    #region SELECT

    #region 追加の元となるデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 追加の元となるデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetBaseData(DatabaseHelper dbHelper)
    {
        try
        {
            var sb = new StringBuilder();
            var dt = new DataTable();

            sb.ApdL("SELECT M1.SHUKKA_FLAG");
            sb.ApdL("      ,M1.NONYUSAKI_NAME");
            sb.ApdL("      ,CASE");
            sb.ApdL("         WHEN MAX(D1.TAG_NO) IS NULL THEN 0");
            sb.ApdL("         ELSE MAX(D1.TAG_NO)");
            sb.ApdL("       END AS ISSUED_TAG_NO");
            sb.ApdL("  FROM M_NONYUSAKI M1");
            sb.ApdL("  LEFT JOIN T_SHUKKA_MEISAI D1");
            sb.ApdL("         ON M1.SHUKKA_FLAG = D1.SHUKKA_FLAG");
            sb.ApdL("        AND M1.NONYUSAKI_CD = D1.NONYUSAKI_CD");
            sb.ApdL(" GROUP BY M1.SHUKKA_FLAG");
            sb.ApdL("         ,M1.NONYUSAKI_NAME");
            sb.ApdL(" ORDER BY M1.SHUKKA_FLAG");
            sb.ApdL("         ,M1.NONYUSAKI_NAME");

            dbHelper.Fill(sb.ToString(), dt);
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

    #region 物件名マスタデータ追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタデータ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">追加データ</param>
    /// <param name="saiban">採番値</param>
    /// <param name="cond">登録ユーザー</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsBukkenExec(DatabaseHelper dbHelper, DataRow dr, string saiban, CondSms cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO M_BUKKEN (");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("      ,BUKKEN_NO");
            sb.ApdL("      ,BUKKEN_NAME");
            sb.ApdL("      ,ISSUED_TAG_NO");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,MAINTE_DATE");
            sb.ApdL("      ,MAINTE_USER_ID");
            sb.ApdL("      ,MAINTE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL("      ,MAINTE_VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("ISSUED_TAG_NO");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", saiban));
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME, DBNull.Value)));
            paramCollection.Add(iNewParameter.NewDbParameter("ISSUED_TAG_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.ISSUED_TAG_NO, DBNull.Value)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region UPDATE

    #region 納入先マスタデータ更新

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタデータ更新
    /// </summary>
    /// <param name="dbHelper">DatabaesHelper</param>
    /// <param name="dr">更新データ</param>
    /// <param name="saiban">採番値</param>
    /// <param name="cond">更新ユーザー</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/09</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdNonyusaki(DatabaseHelper dbHelper, DataRow dr, string saiban, CondSms cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE M_NONYUSAKI");
            sb.ApdN("   SET BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_NAME");

            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", saiban));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_NAME", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME)));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
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

    #endregion
}
