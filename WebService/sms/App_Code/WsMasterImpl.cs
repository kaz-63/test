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
/// マスタ処理系（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
public class WsMasterImpl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsMasterImpl()
        : base()
    {
    }

    #endregion

    #region 締めマスタ

    #region SELECT
    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region 納入先マスタ

    #region WhereText

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ用WhereText
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <param name="paramCollection">DbParameCollection</param>
    /// <param name="isApdNWhere">先頭にWhere 1 = 1を追加するかどうか</param>
    /// <returns>Where文字列</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update>M.Tsutsumi 2011/02/16</update>
    /// --------------------------------------------------
    public string WhereTextNonyusaki(INewDbParameterBasic iNewParameter, CondNonyusaki cond, DbParamCollection paramCollection, string tableName, bool isApdNWhere)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            string fieldPrefix = string.Empty;
            string fieldName = string.Empty;

            if (!string.IsNullOrEmpty(tableName))
            {
                fieldPrefix = tableName + ".";
            }

            if (isApdNWhere)
            {
                sb.ApdL(" WHERE ");
                sb.ApdL("       1 = 1");
            }
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 物件管理No
            if (cond.BukkenNo != null)
            {
                fieldName = "BUKKEN_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.BukkenNo));
            }
            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // 納入先
            if (cond.NonyusakiName != null)
            {
                fieldName = "NONYUSAKI_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.NonyusakiName));
            }
            // 出荷便
            if (cond.Ship != null)
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Ship));
            }
            // 管理フラグ
            if (cond.KanriFlag != null)
            {
                fieldName = "KANRI_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.KanriFlag));
            }
            // 登録日時
            if (cond.CreateDate != null)
            {
                fieldName = "CREATE_DATE";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.CreateDate));
            }
            // 登録ユーザーID
            if (cond.CreateUserID != null)
            {
                fieldName = "CREATE_USER_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.CreateUserID));
            }
            // 登録ユーザー名
            if (cond.CreateUserName != null)
            {
                fieldName = "CREATE_USER_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.CreateUserName));
            }
            // 更新日時
            if (cond.UpdateDate != null)
            {
                fieldName = "UPDATE_DATE";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.UpdateDate));
            }
            // 更新ユーザーID
            if (cond.UpdateUserID != null)
            {
                fieldName = "UPDATE_USER_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.UpdateUserID));
            }
            // 更新ユーザー名
            if (cond.UpdateUserName != null)
            {
                fieldName = "UPDATE_USER_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.UpdateUserName));
            }
            // 保守日時
            if (cond.MainteDate != null)
            {
                fieldName = "MAINTE_DATE";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.MainteDate));
            }
            // 保守ユーザーID
            if (cond.MainteUserID != null)
            {
                fieldName = "MAINTE_USER_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.MainteUserID));
            }
            // 保守ユーザー名
            if (cond.MainteUserName != null)
            {
                fieldName = "MAINTE_USER_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.MainteUserName));
            }
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36、37
            // リスト区分名称０
            if (cond.ListFlagName0 != null)
            {
                fieldName = "LIST_FLAG_NAME0";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName0));
            }
            // リスト区分名称１
            if (cond.ListFlagName1 != null)
            {
                fieldName = "LIST_FLAG_NAME1";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName1));
            }
            // リスト区分名称２
            if (cond.ListFlagName2 != null)
            {
                fieldName = "LIST_FLAG_NAME2";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName2));
            }
            // リスト区分名称３
            if (cond.ListFlagName3 != null)
            {
                fieldName = "LIST_FLAG_NAME3";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName3));
            }
            // リスト区分名称４
            if (cond.ListFlagName4 != null)
            {
                fieldName = "LIST_FLAG_NAME4";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName4));
            }
            // リスト区分名称５
            if (cond.ListFlagName5 != null)
            {
                fieldName = "LIST_FLAG_NAME5";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName5));
            }
            // リスト区分名称６
            if (cond.ListFlagName6 != null)
            {
                fieldName = "LIST_FLAG_NAME6";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName6));
            }
            // リスト区分名称７
            if (cond.ListFlagName7 != null)
            {
                fieldName = "LIST_FLAG_NAME7";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.ListFlagName7));
            }
            // 除外フラグ
            if (cond.RemoveFlag != null)
            {
                fieldName = "REMOVE_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.RemoveFlag));
            }
            // @@@ ↑
            // バージョン
            if (cond.Version != null)
            {
                fieldName = "VERSION";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Version));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/06/25</create>
    /// <update>K.Tsutsumi 2019/02/08 新しいフィールドを追加</update>
    /// --------------------------------------------------
    public DataSet GetNonyusaki(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       MN.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , MN.KANRI_FLAG");
            sb.ApdL("     , COM2.ITEM_NAME AS KANRI_FLAG_NAME");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36、37
            sb.ApdL("     , MN.LIST_FLAG_NAME0");
            sb.ApdL("     , MN.LIST_FLAG_NAME1");
            sb.ApdL("     , MN.LIST_FLAG_NAME2");
            sb.ApdL("     , MN.LIST_FLAG_NAME3");
            sb.ApdL("     , MN.LIST_FLAG_NAME4");
            sb.ApdL("     , MN.LIST_FLAG_NAME5");
            sb.ApdL("     , MN.LIST_FLAG_NAME6");
            sb.ApdL("     , MN.LIST_FLAG_NAME7");
            sb.ApdL("     , MN.REMOVE_FLAG");
            // @@@ ↑
            sb.ApdL("     , MN.VERSION");
            sb.ApdL("     , MN.BUKKEN_NO");
            // K.Tsutsumi 2019/02/08 Add
            sb.ApdL("     , MN.TRANSPORT_FLAG");
            sb.ApdL("     , MN.ESTIMATE_FLAG");
            sb.ApdL("     , MN.SHIP_DATE");
            sb.ApdL("     , MN.SHIP_FROM");
            sb.ApdL("     , MN.SHIP_TO");
            sb.ApdL("     , MN.SHIP_NO");
            sb.ApdL("     , MN.SHIP_SEIBAN");

            // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
            //　項目 SHIP_FROM_CD
            sb.ApdL("     , MN.SHIP_FROM_CD");


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
            sb.ApdN(this.WhereTextNonyusaki(dbHelper, cond, paramCollection, "MN", true));
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MN.SHUKKA_FLAG");
            sb.ApdL("     , MN.NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));

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

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update>M.Tsutsumi 2011/02/16</update>
    /// <update>H.Tajimi 2018/10/31 出荷計画取込対応</update>
    /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
    /// <update>R.Miyoshi 2023/07/21 処理フラグ～備考の追加</update>
    /// <update>J.Chen 2023/10/18 出荷元CD更新</update>
    /// --------------------------------------------------
    public int InsNonyusaki(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("INSERT INTO M_NONYUSAKI ");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");

            // 納入先
            if (cond.NonyusakiName != null)
            {
                fieldName = "NONYUSAKI_NAME";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiName));
            }
            // 出荷便
            if (cond.Ship != null)
            {
                fieldName = "SHIP";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Ship));
            }
            // 管理フラグ
            if (cond.KanriFlag != null)
            {
                fieldName = "KANRI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KanriFlag));
            }
            // 保守日時
            if (cond.MainteDate != null)
            {
                fieldName = "MAINTE_DATE";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.MainteDate));
            }
            // 保守ユーザーID
            if (cond.MainteUserID != null)
            {
                fieldName = "MAINTE_USER_ID";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.MainteUserID));
            }
            // 保守ユーザー名
            if (cond.MainteUserName != null)
            {
                fieldName = "MAINTE_USER_NAME";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.MainteUserName));
            }
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36、37
            // リスト区分名称０
            if (cond.ListFlagName0 != null)
            {
                fieldName = "LIST_FLAG_NAME0";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName0));
            }
            // リスト区分名称１
            if (cond.ListFlagName1 != null)
            {
                fieldName = "LIST_FLAG_NAME1";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName1));
            }
            // リスト区分名称２
            if (cond.ListFlagName2 != null)
            {
                fieldName = "LIST_FLAG_NAME2";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName2));
            }
            // リスト区分名称３
            if (cond.ListFlagName3 != null)
            {
                fieldName = "LIST_FLAG_NAME3";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName3));
            }
            // リスト区分名称４
            if (cond.ListFlagName4 != null)
            {
                fieldName = "LIST_FLAG_NAME4";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName4));
            }
            // リスト区分名称５
            if (cond.ListFlagName5 != null)
            {
                fieldName = "LIST_FLAG_NAME5";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName5));
            }
            // リスト区分名称６
            if (cond.ListFlagName6 != null)
            {
                fieldName = "LIST_FLAG_NAME6";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName6));
            }
            // リスト区分名称７
            if (cond.ListFlagName7 != null)
            {
                fieldName = "LIST_FLAG_NAME7";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName7));
            }
            // 除外フラグ
            if (cond.RemoveFlag != null)
            {
                fieldName = "REMOVE_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.RemoveFlag));
            }
            // @@@ ↑
            // 物件管理No
            if (cond.BukkenNo != null)
            {
                fieldName = "BUKKEN_NO";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BukkenNo));
            }
            // 運送区分
            if (!string.IsNullOrEmpty(cond.TransportFlag))
            {
                fieldName = "TRANSPORT_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TransportFlag));
            }
            // 有償・無償
            if (!string.IsNullOrEmpty(cond.EstimateFlag))
            {
                fieldName = "ESTIMATE_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.EstimateFlag));
            }
            // 出荷日
            if (!string.IsNullOrEmpty(cond.ShipDate))
            {
                fieldName = "SHIP_DATE";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipDate));
            }
            // 出荷元
            if (!string.IsNullOrEmpty(cond.ShipFrom))
            {
                fieldName = "SHIP_FROM";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipFrom));
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.ShipTo))
            {
                fieldName = "SHIP_TO";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipTo));
            }
            // 案件管理No
            if (cond.ShipNo != null)
            {
                fieldName = "SHIP_NO";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipNo));
            }
            // 運賃梱包製番
            if (!string.IsNullOrEmpty(cond.ShipSeiban))
            {
                fieldName = "SHIP_SEIBAN";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipSeiban));
            }
            // 出荷元CD
            if (cond.ShipFromCD != null)
            {
                if (cond.ShipFromCD == "" && !string.IsNullOrEmpty(cond.ShipFrom))
                {
                    // 出荷元CD
                    var fieldNameTemp = "SHIP_FROM";
                    sb.ApdL("     , ").ApdN(Def_M_NONYUSAKI.SHIP_FROM_CD);
                    sbValues.ApdN("     ,(SELECT SHIP_FROM_NO FROM M_SHIP_FROM WHERE SHIP_FROM_NAME = ").ApdN(this.BindPrefix).ApdL(fieldNameTemp + ") ");
                }
                else
                {
                    fieldName = "SHIP_FROM_CD";
                    sb.ApdL("     , ").ApdN(fieldName);
                    sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                    paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipFromCD));
                }
                
            }
            // 処理フラグ
            if (cond.SyoriFlag != null)
            {
                fieldName = "SYORI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SyoriFlag));
            }
            //製番 
            if (cond.Seiban != null)
            {
                fieldName = "SEIBAN";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Seiban));
            }
            // 機種
            if (cond.Kishu != null)
            {
                fieldName = "KISHU";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Kishu));
            }
            // 内容
            if (cond.Naiyo != null)
            {
                fieldName = "NAIYO";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Naiyo));
            }
            // 到着予定日
            if (!string.IsNullOrEmpty(cond.TouchakuyoteiDate))
            {
                fieldName = "TOUCHAKUYOTEI_DATE";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TouchakuyoteiDate));
            }
            // 機械Parts
            if (cond.KikaiParts != null)
            {
                fieldName = "KIKAI_PARTS";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KikaiParts));
            }
            // 制御Parts
            if (cond.SeigyoParts != null)
            {
                fieldName = "SEIGYO_PARTS";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SeigyoParts));
            }
            // 備考
            if (cond.Biko != null)
            {
                fieldName = "BIKO";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Biko));
            }
            // 荷受CD
            if (cond.ConsignName != null)
            {
                fieldName = "CONSIGN_CD";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ConsignName));
            }
            // 処理フラグ（前回値）
            if (cond.SyoriFlag != null)
            {
                fieldName = "LAST_SYORI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName);
                sbValues.ApdN("     , ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SyoriFlag));
            }
            
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN(sbValues.ToString());
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update>H.Tajimi 2018/10/31 出荷計画取込対応</update>
    /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
    /// <update>R.Miyoshi 2023/07/21 処理フラグ～備考の追加</update>
    /// <update>J.Chen 2023/10/18 出荷元CD更新</update>
    /// --------------------------------------------------
    public int UpdNonyusaki(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI ");
            sb.ApdL("SET");
            sb.ApdN("       UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            // 納入先
            if (cond.NonyusakiName != null)
            {
                fieldName = "NONYUSAKI_NAME";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiName));
            }
            // 出荷便
            if (cond.Ship != null)
            {
                fieldName = "SHIP";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Ship));
            }
            // 管理フラグ
            if (cond.KanriFlag != null)
            {
                fieldName = "KANRI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KanriFlag));
            }
            // 保守日時
            if (cond.MainteUserID != null || cond.MainteUserName != null)
            {
                fieldName = "MAINTE_DATE";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdL(this.SysTimestamp);
            }
            // 保守ユーザーID
            if (cond.MainteUserID != null)
            {
                fieldName = "MAINTE_USER_ID";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.MainteUserID));
            }
            // 保守ユーザー名
            if (cond.MainteUserName != null)
            {
                fieldName = "MAINTE_USER_NAME";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.MainteUserName));
            }
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36、37
            // リスト区分名称０
            if (cond.ListFlagName0 != null)
            {
                fieldName = "LIST_FLAG_NAME0";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName0));
            }
            // リスト区分名称１
            if (cond.ListFlagName1 != null)
            {
                fieldName = "LIST_FLAG_NAME1";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName1));
            }
            // リスト区分名称２
            if (cond.ListFlagName2 != null)
            {
                fieldName = "LIST_FLAG_NAME2";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName2));
            }
            // リスト区分名称３
            if (cond.ListFlagName3 != null)
            {
                fieldName = "LIST_FLAG_NAME3";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName3));
            }
            // リスト区分名称４
            if (cond.ListFlagName4 != null)
            {
                fieldName = "LIST_FLAG_NAME4";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName4));
            }
            // リスト区分名称５
            if (cond.ListFlagName5 != null)
            {
                fieldName = "LIST_FLAG_NAME5";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName5));
            }
            // リスト区分名称６
            if (cond.ListFlagName6 != null)
            {
                fieldName = "LIST_FLAG_NAME6";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName6));
            }
            // リスト区分名称７
            if (cond.ListFlagName7 != null)
            {
                fieldName = "LIST_FLAG_NAME7";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlagName7));
            }
            // 除外フラグ
            if (cond.RemoveFlag != null)
            {
                fieldName = "REMOVE_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.RemoveFlag));
            }
            // @@@ ↑
            // 運送区分
            if (!string.IsNullOrEmpty(cond.TransportFlag))
            {
                fieldName = "TRANSPORT_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TransportFlag));
            }
            else
            {
                fieldName = "TRANSPORT_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 有償・無償
            if (!string.IsNullOrEmpty(cond.EstimateFlag))
            {
                fieldName = "ESTIMATE_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.EstimateFlag));
            }
            else
            {
                fieldName = "ESTIMATE_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 出荷日
            if (!string.IsNullOrEmpty(cond.ShipDate))
            {
                fieldName = "SHIP_DATE";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipDate));
            }
            else
            {
                fieldName = "SHIP_DATE";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 出荷元
            if (!string.IsNullOrEmpty(cond.ShipFrom))
            {
                fieldName = "SHIP_FROM";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipFrom));
            }
            else
            {
                fieldName = "SHIP_FROM";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.ShipTo))
            {
                fieldName = "SHIP_TO";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipTo));
            }
            else
            {
                fieldName = "SHIP_TO";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 案件管理No
            if (cond.ShipNo != null)
            {
                fieldName = "SHIP_NO";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipNo));
            }
            // 運賃梱包製番
            if (!string.IsNullOrEmpty(cond.ShipSeiban))
            {
                fieldName = "SHIP_SEIBAN";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipSeiban));
            }
            // 出荷元CD
            if (cond.ShipFromCD != null)
            {
                if (!string.IsNullOrEmpty(cond.ShipFrom))
                {
                    // 出荷元CD
                    var fieldNameTemp = "SHIP_FROM";
                    sb.ApdN("     , SHIP_FROM_CD = (SELECT SHIP_FROM_NO FROM M_SHIP_FROM WHERE SHIP_FROM_NAME = ").ApdN(this.BindPrefix).ApdL(fieldNameTemp + ") ");
                }
                else
                {
                    fieldName = "SHIP_FROM_CD";
                    sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                    paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipFromCD));
                }
            }
            // 処理フラグ
            if (cond.SyoriFlag != null)
            {
                fieldName = "SYORI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SyoriFlag));
                // 処理フラグ（前回値）
                fieldName = "LAST_SYORI_FLAG";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SyoriFlag));
            }
            //製番 
            if (cond.Seiban != null)
            {
                fieldName = "SEIBAN";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Seiban));
            }
            // 機種
            if (cond.Kishu != null)
            {
                fieldName = "KISHU";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Kishu));
            }
            // 内容
            if (cond.Naiyo != null)
            {
                fieldName = "NAIYO";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Naiyo));
            }
            // 到着予定日
            if (!string.IsNullOrEmpty(cond.TouchakuyoteiDate))
            {
                fieldName = "TOUCHAKUYOTEI_DATE";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TouchakuyoteiDate));
            }
            else
            {
                fieldName = "TOUCHAKUYOTEI_DATE";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = NULL ");
            }
            // 機械Parts
            if (cond.KikaiParts != null)
            {
                fieldName = "KIKAI_PARTS";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KikaiParts));
            }
            // 制御Parts
            if (cond.SeigyoParts != null)
            {
                fieldName = "SEIGYO_PARTS";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SeigyoParts));
            }
            // 備考
            if (cond.Biko != null)
            {
                fieldName = "BIKO";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Biko));
            }
            // 荷受CD
            if (cond.ConsignName != null)
            {
                fieldName = "CONSIGN_CD";
                sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ConsignName));
            }

            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // 物件管理No
            if (cond.BukkenNo != null)
            {
                fieldName = "BUKKEN_NO";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BukkenNo));
            }


            // バインド変数設定
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

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ　処理フラグ（前回値）更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2023/09/28</create>
    /// <update>J.Chen 2023/10/18 前回値更新条件追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdNonyusakiForLastSyoriFalg(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI ");
            sb.ApdL("SET");
            sb.ApdN("       UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            // 処理フラグ（前回値）
            fieldName = "LAST_SYORI_FLAG";
            sb.ApdN("     , ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.SyoriFlag));

            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 納入先コード
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // 便
            else if (!string.IsNullOrEmpty(cond.Ship))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Ship));
            }
            // 運賃梱包製番
            if (!string.IsNullOrEmpty(cond.ShipSeiban))
            {
                fieldName = "SHIP_SEIBAN";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShipSeiban));
            }
            // 物件管理No
            if (cond.BukkenNo != null)
            {
                fieldName = "BUKKEN_NO";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BukkenNo));
            }
            //sb.ApdN("   AND ").ApdN(fieldName).ApdN(" != ").ApdL(SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE1);

            // バインド変数設定
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

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelNonyusaki(DatabaseHelper dbHelper, CondNonyusaki cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_NONYUSAKI");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

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

    #region 権限マスタ

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">権限マスタ用コンディション</param>
    /// <returns>権限マスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// --------------------------------------------------
    public DataSet GetRole(DatabaseHelper dbHelper, CondRole cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ROLE_ID");
            sb.ApdL("     , ROLE_NAME");
            sb.ApdL("     , ROLE_FLAG");
            sb.ApdL("  FROM");
            sb.ApdL("       M_ROLE");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.RoleID))
            {
                sb.ApdN("   AND ROLE_ID = ").ApdN(this.BindPrefix).ApdL("ROLE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("ROLE_ID", cond.RoleID));
            }
            if (!string.IsNullOrEmpty(cond.RoleName))
            {
                sb.ApdN("   AND ROLE_NAME = ").ApdN(this.BindPrefix).ApdL("ROLE_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("ROLE_NAME", cond.RoleName));
            }
            if (!string.IsNullOrEmpty(cond.RoleFlag))
            {
                sb.ApdN("   AND ROLE_FLAG = ").ApdN(this.BindPrefix).ApdL("ROLE_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("ROLE_FLAG", cond.RoleFlag));
            }
            if (cond.LoginInfo != null && !string.IsNullOrEmpty(cond.LoginInfo.Language))
            {
                sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
                paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ROLE_ID");

            // バインド変数設定

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_ROLE.Name);

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

}
