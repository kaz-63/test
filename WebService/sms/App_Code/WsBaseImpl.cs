using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Configuration;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// Webサービスデータアクセス層のベースクラス
/// </summary>
/// <create>Y.Higuchi 2010/04/15</create>
/// <update></update>
/// --------------------------------------------------
public class WsBaseImpl : IDisposable
{
    #region Fields
    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsBaseImpl()
    {
    }

    #endregion

    #region Dispose

    #region IDisposable メンバ

    /// --------------------------------------------------
    /// <summary>
    /// Dispose
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual void Dispose(bool disposing)
    {
        try
        {
            if (disposing)
            {
            }
        }
        catch { }
    }

    #endregion

    #region Properties

    /// --------------------------------------------------
    /// <summary>
    /// バインド変数に使用する接頭辞
    /// </summary>
    /// <create>Y.Higuchi 2010/04/19</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual string BindPrefix
    {
        get
        {
            string ret = string.Empty;
            switch (this.DbProviderName)
            {
                case DbProviderNames.DbProviderName_Oracle:
                    ret = ":";
                    break;
                default:
                    ret = "@";
                    break;
            }
            return ret;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// プロバイダ名
    /// </summary>
    /// <create>Y.Higuchi 2010/04/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private string DbProviderName
    {
        get
        {
            return WebConfigurationManager.AppSettings["DbProviderName"].ToString();
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 当日
    /// </summary>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string SysDate
    {
        get
        {
            string ret = string.Empty;
            switch (this.DbProviderName)
            {
                case DbProviderNames.DbProviderName_Oracle:
                    ret = "SYSDATE";
                    break;
                case DbProviderNames.DbProviderName_SQLServer:
                    ret = "GETDATE()";
                    break;
                default:
                    ret = DateTime.Now.ToString("yyyy/MM/dd");
                    break;
            }
            return ret;
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// タイムスタンプ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string SysTimestamp
    {
        get
        {
            string ret = string.Empty;
            switch (this.DbProviderName)
            {
                case DbProviderNames.DbProviderName_Oracle:
                    ret = "SYSTIMESTAMP";
                    break;
                case DbProviderNames.DbProviderName_SQLServer:
                    ret = "SYSDATETIME()";
                    break;
                default:
                    ret = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff");
                    break;
            }
            return ret;
        }
    }

    #endregion

    #region 登録ユーザーID取得

    /// --------------------------------------------------
    /// <summary>
    /// 登録ユーザーID取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>登録ユーザーID</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetCreateUserID(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserID))
            {
                ret = cond.LoginInfo.UserID;
            }
        }
        if (!string.IsNullOrEmpty(cond.CreateUserID))
        {
            ret = cond.CreateUserID;
        }
        return ret;
    }

    #endregion

    #region 登録ユーザー名取得

    /// --------------------------------------------------
    /// <summary>
    /// 登録ユーザー名取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>登録ユーザー名</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetCreateUserName(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserName))
            {
                ret = cond.LoginInfo.UserName;
            }
        }
        if (!string.IsNullOrEmpty(cond.CreateUserName))
        {
            ret = cond.CreateUserName;
        }
        return ret;
    }

    #endregion

    #region 更新ユーザーID

    /// --------------------------------------------------
    /// <summary>
    /// 更新ユーザーID取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>登録ユーザーID</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetUpdateUserID(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserID))
            {
                ret = cond.LoginInfo.UserID;
            }
        }
        if (!string.IsNullOrEmpty(cond.UpdateUserID))
        {
            ret = cond.UpdateUserID;
        }
        return ret;
    }

    #endregion

    #region 更新ユーザー名取得

    /// --------------------------------------------------
    /// <summary>
    /// 更新ユーザー名取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>更新ユーザー名</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetUpdateUserName(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserName))
            {
                ret = cond.LoginInfo.UserName;
            }
        }
        if (!string.IsNullOrEmpty(cond.UpdateUserName))
        {
            ret = cond.UpdateUserName;
        }
        return ret;
    }

    #endregion

    #region 保守ユーザーID取得

    /// --------------------------------------------------
    /// <summary>
    /// 保守ユーザーID取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>保守ユーザーID</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetMainteUserID(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserID))
            {
                ret = cond.LoginInfo.UserID;
            }
        }
        if (!string.IsNullOrEmpty(cond.MainteUserID))
        {
            ret = cond.MainteUserID;
        }
        return ret;
    }

    #endregion

    #region 保守ユーザー名取得

    /// --------------------------------------------------
    /// <summary>
    /// 保守ユーザー名取得
    /// </summary>
    /// <param name="cond">ベースコンディション</param>
    /// <returns>保守ユーザー名</returns>
    /// <create>Y.Higuchi 2010/06/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetMainteUserName(CondBase cond)
    {
        string ret = " ";
        if (cond.LoginInfo != null)
        {
            if (!string.IsNullOrEmpty(cond.LoginInfo.UserName))
            {
                ret = cond.LoginInfo.UserName;
            }
        }
        if (!string.IsNullOrEmpty(cond.MainteUserName))
        {
            ret = cond.MainteUserName;
        }
        return ret;
    }

    #endregion

    #region バージョンチェック

    /// --------------------------------------------------
    /// <summary>
    /// データ同一チェック(同一データの場合は -1 が戻ってくる。)
    /// </summary>
    /// <param name="dtTarget">対象データテーブル</param>
    /// <param name="dtOriginal">検査元データテーブル</param>
    /// <param name="notFoundIndex">対象データが検査元になかったデータのインデックス配列</param>
    /// <param name="checkField">同一データとみなすためのフィールド名配列</param>
    /// <param name="keyFields">同じか比較するフィールド名</param>
    /// <returns>同一でないデータのインデックス</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    protected virtual int CheckSameData(DataTable dtTarget, DataTable dtOriginal, out int[] notFoundIndex,string checkField, params string[] keyFields)
    {
        int index = -1;
        notFoundIndex = null;
        if (keyFields == null || keyFields.Length < 1) return index;
        for (int i = 0; i < dtTarget.Rows.Count; i++)
        {
            bool isExists = false;
            foreach (DataRow dr in dtOriginal.Rows)
            {
                isExists = true;
                foreach (string field in keyFields)
                {
                    if (!object.Equals(ComFunc.GetFldObject(dr, field), ComFunc.GetFldObject(dtTarget, i, field)))
                    {
                        isExists = false;
                        break;
                    }
                }
                if (isExists && !object.Equals(ComFunc.GetFldObject(dr, checkField), ComFunc.GetFldObject(dtTarget, i, checkField)))
                {
                    index = i;
                    return index;
                }
                else if (isExists)
                {
                    break;
                }
            }
            if (!isExists)
            {
                if (notFoundIndex == null)
                {
                    notFoundIndex = new int[] { i };
                }
                else
                {
                    Array.Resize<int>(ref notFoundIndex, notFoundIndex.Length + 1);
                    notFoundIndex[notFoundIndex.Length - 1] = i;
                }
            }
        }

        return index;
    }

    #endregion

    #region 一意制約違反チェック

    /// --------------------------------------------------
    /// <summary>
    /// 一意制約違反チェック(trueなら一意制約違反)
    /// </summary>
    /// <param name="exception">例外</param>
    /// <returns>true:一意制約違反/false:それ以外</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool IsDbDuplicationError(Exception exception)
    {
        try
        {
            if (exception == null) return false;
            System.Data.SqlClient.SqlException sqlEx = exception as System.Data.SqlClient.SqlException;
            if (sqlEx != null && (sqlEx.Number == ComDefine.DB_DUPLICATION_ERRORNO || sqlEx.Number == ComDefine.DB_DUPLICATION_ERRORNO_UNIQUEINDEX))
            {
                // 一意制約違反
                return true;
            }
            return this.IsDbDuplicationError(exception.InnerException);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion
}
