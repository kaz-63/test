using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

/// --------------------------------------------------
/// <summary>
/// AR品管理処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsA01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsA01Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region A0100010:AR情報登録 & A0100020:AR情報明細登録 共通

    #region 納入先マスタ 存在チェック

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの存在チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="kanriFlag">管理区分</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool IsExistenceNonyusaki(DatabaseHelper dbHelper, CondA1 cond, ref string kanriFlag)
    {
        try
        {
            DataSet ds = new DataSet();

            // コンディション作成
            CondNonyusaki condN = new CondNonyusaki(cond.LoginInfo);
            condN.ShukkaFlag = cond.ShukkaFlag;
            condN.NonyusakiCD = cond.NonyusakiCD;
            condN.LoginInfo = cond.LoginInfo;

            using (WsMasterImpl impl = new WsMasterImpl())
            {
                ds = impl.GetNonyusaki(dbHelper, condN);
            }

            if (ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count == 0)
            {
                // 存在しない
                return false;
            }

            // 管理区分
            kanriFlag = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG);

            // 存在する。
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region A0100010:AR情報登録

    #region 納入先マスタの存在確認後、Excelデータの件数を取得する

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの存在確認後、Excelデータの件数を取得する
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ExistNonyusakiAndExcle(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID)
    {
        try
        {
            // 納入先マスタの存在確認
            string kanriFlag = string.Empty;
            if (!this.IsExistenceNonyusaki(dbHelper, cond, ref kanriFlag))
            {
                errMsgID = "A9999999044";
                return false;
            }

            // Excelデータの存在確認
            if (ComFunc.GetFldToInt32(this.GetAllARListCount(dbHelper, cond), 0, ComDefine.FLD_CNT) == 0)
            {
                errMsgID = "A0100010004";
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

    #region 納入先マスタの存在確認後、費用Excelデータの件数を取得する

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタの存在確認後、費用Excelデータの件数を取得する
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ExistNonyusakiAndHiyouExcel(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID)
    {
        try
        {
            // 納入先マスタの存在確認
            string kanriFlag = string.Empty;
            if (!this.IsExistenceNonyusaki(dbHelper, cond, ref kanriFlag))
            {
                errMsgID = "A9999999044";
                return false;
            }

            // Excelデータの存在確認
            if (ComFunc.GetFldToInt32(this.GetAllARCostListCount(dbHelper, cond), 0, ComDefine.FLD_CNT) == 0)
            {
                errMsgID = "A0100010004";
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

    #region AR情報データ 一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データセット</returns>
    /// <create>M.Tsutsumi 2010/08/12</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// --------------------------------------------------
    public DataSet GetArDataList(DatabaseHelper dbHelper, CondA1 cond)
    {
        DataSet ds = new DataSet();

        // AR情報データ
        DataTable dt = this.GetARList(dbHelper, cond);
        ds.Tables.Add(dt.Copy());

        // AR対応費用データ
        var dtCost = this.GetARCostList(dbHelper, cond, dt);
        ds.Merge(dtCost);

        return ds;
    }

    #endregion

    #region メール送信情報を取得

    /// --------------------------------------------------
    /// <summary>
    /// メール送信情報を取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>データセット</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSendMailInfo(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID, ref string[] args)
    {
        DataSet ds = new DataSet();

        // メール設定マスタ取得
        var dtMailSetting = this.GetMailSetting(dbHelper);
        ds.Merge(dtMailSetting);

        // メールマスタの設定で、メール送信対象になっているか判別
        // →メール通知しないなら正常終了
        string colName = cond.IsToroku ? Def_M_MAIL_SETTING.AR_ADD_EVENT : Def_M_MAIL_SETTING.AR_UPDATE_EVENT;
        if (UtilData.GetFld(dtMailSetting, 0, colName) == AR_ADD_EVENT.NO_VALUE1)
        {
            return ds;
        }

        // 物件マスタの設定で、メール送信対象になっているか判別
        // →メール通知しないなら正常終了
        var dtBukken = this.GetBukken(dbHelper, cond);
        if (UtilData.ExistsData(dtBukken)
            && UtilData.GetFld(dtBukken, 0, Def_M_BUKKEN.MAIL_NOTIFY) == MAIL_NOTIFY.STOP_VALUE1)
        {
            return ds;
        }

        // ユーザーID未定義ならエラー
        if (string.IsNullOrEmpty(cond.UpdateUserID))
        {
            // 担当者が存在しません。
            errMsgID = "A0100010009";
            return ds;
        }

        // ユーザーマスタ取得
        var dtUser = this.GetUser(dbHelper, cond);
        ds.Merge(dtUser);
        if (!UtilData.ExistsData(dtUser))
        {
            // 担当者が存在しません。
            errMsgID = "A0100010009";
            return ds;
        }

        // メールアドレスチェック
        if (string.IsNullOrEmpty(UtilData.GetFld(dtUser, 0, Def_M_USER.MAIL_ADDRESS)))
        {
            // 担当者にMailAddressが設定されていません。
            errMsgID = "A0100010010";
            return ds;
        }

        // リスト別メール情報チェック(AR + 物件No + メール区分(ARList) + リスト区分)
        var dtBukkenMail = this.GetBukkenMail(dbHelper, cond, true);

        // 共通メール情報チェック(AR + 物件No + メール区分(共通))
        dtBukkenMail.Merge(this.GetBukkenMail(dbHelper, cond, false));
        
        if (!UtilData.ExistsData(dtBukkenMail))
        {
            // 送信先MailAddressが設定されていません。
            errMsgID = "A0100010011";
            return ds;
        }

        return ds;
    }

    #endregion

    #endregion

    #region A0100020:AR情報明細登録

    #region AR情報データ １レコード取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2019/01/23 写真の保存先変更</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
    /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
    /// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
    /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
    /// --------------------------------------------------
    public DataSet GetArData(DatabaseHelper dbHelper, CondA1 cond)
    {
        DataSet ds = new DataSet();

        // AR情報データ 取得
        DataTable dt = this.GetAndLockAR(dbHelper, cond, false);

        if (dt.Rows.Count != 0)
        {
            string fileName;
            string filePath = string.Empty;
            WsAttachFileImpl impl = new WsAttachFileImpl();

            string nonyusakiCd = ComFunc.GetFld(dt, 0, Def_T_AR.NONYUSAKI_CD);
            string listFlag = ComFunc.GetFld(dt, 0, Def_T_AR.LIST_FLAG);
            string arNo = ComFunc.GetFld(dt, 0, Def_T_AR.AR_NO);

            // GIREN_FILE File確認
            // ①
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.GIREN_FILE_1);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARGiren, nonyusakiCd, arNo, GirenType.No1, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.GIREN_FILE_1] = string.Empty;
                }
            }
            // ②
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.GIREN_FILE_2);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARGiren, nonyusakiCd, arNo, GirenType.No2, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.GIREN_FILE_2] = string.Empty;
                }
            }
            // ③
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.GIREN_FILE_3);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARGiren, nonyusakiCd, arNo, GirenType.No3, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.GIREN_FILE_3] = string.Empty;
                }
            }
            // ④
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.GIREN_FILE_4);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARGiren, nonyusakiCd, arNo, GirenType.No4, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.GIREN_FILE_4] = string.Empty;
                }
            }
            // ⑤
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.GIREN_FILE_5);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARGiren, nonyusakiCd, arNo, GirenType.No5, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.GIREN_FILE_5] = string.Empty;
                }
            }

            // REFERENCE_FILE File確認
            // ①
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.REFERENCE_FILE_1);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARRef, nonyusakiCd, arNo, GirenType.RefNo1, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.REFERENCE_FILE_1] = string.Empty;
                }
            }
            // ②
            fileName = ComFunc.GetFld(dt, 0, Def_T_AR.REFERENCE_FILE_2);
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = impl.GetFilePath(FileType.ARRef, nonyusakiCd, arNo, GirenType.RefNo2, null, fileName, null);
                if (!File.Exists(filePath))
                {
                    // サーバー上に画像ファイルがない場合、ファイル名もクリア
                    dt.Rows[0][Def_T_AR.REFERENCE_FILE_2] = string.Empty;
                }
            }
        }

        ds.Tables.Add(dt.Copy());

        // AR進捗データ取得
        DataTable dtShinchoku = this.GetArShinchoku(dbHelper, cond);
        ds.Merge(dtShinchoku);

        // AR対応費用データ 取得
        DataTable dtCost = this.GetARCost(dbHelper, cond);
        ds.Merge(dtCost);

        // AR添付ファイル 取得
        DataTable dtFile = this.GetARFile(dbHelper, cond);
        ds.Merge(dtFile);

        // 出荷状況 取得
        DataTable dtShukka = this.GetShukka(dbHelper, cond);
        ds.Merge(dtShukka);

        return ds;
    }

    #endregion

    #region 納入先マスタ 取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ 取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>納入先マスタ</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetNonyusaki(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataSet ds = new DataSet();

            // コンディション作成
            CondNonyusaki condN = new CondNonyusaki(cond.LoginInfo);
            condN.ShukkaFlag = cond.ShukkaFlag;
            condN.BukkenNo = cond.BukkenNo;
            condN.NonyusakiCD = cond.NonyusakiCD;
            condN.LoginInfo = cond.LoginInfo;

            using (WsMasterImpl impl = new WsMasterImpl())
            {
                ds = impl.GetNonyusaki(dbHelper, condN);
            }

            return ds.Tables[Def_M_NONYUSAKI.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細データ 存在チェック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ 存在チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool IsExistenceShukkaMeisai(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            // 取得
            DataTable dt = this.GetShukkaMeisai(dbHelper, cond, false);

            if (dt.Rows.Count == 0)
            {
                // 存在しない
                return false;
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
    /// 出荷日が設定されていない出荷明細の存在チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool IsNonShukkaDateShukkaMeisai(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            // 取得
            DataTable dt = this.GetShukkaMeisai(dbHelper, cond, true);

            if (dt.Rows.Count == 0)
            {
                // 存在しない
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

    #region AR情報データ インターロック

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ARInterLock(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID)
    {
        try
        {
            // コンディション作成
            DateTime syoriDateTime = DateTime.Now;
            cond.UpdateDate = syoriDateTime;

            double timeOut = 0;

            // タイムアウト時間取得
            using (WsCommonImpl impl = new WsCommonImpl())
            {
                CondCommon condC = new CondCommon(cond.LoginInfo);
                condC.GroupCD = INTERLOCK_TIMEOUT.GROUPCD;
                condC.ItemCD = INTERLOCK_TIMEOUT.VALUE_NAME;
                condC.LoginInfo = cond.LoginInfo;
                DataSet ds = impl.GetCommon(dbHelper, condC);
                timeOut = UtilConvert.ToDouble(ComFunc.GetFld(ds, Def_M_COMMON.Name, 0, Def_M_COMMON.VALUE1));
            }

            // AR情報データ 取得(行ロック)
            DataTable dt = this.GetAndLockAR(dbHelper, cond, true);

            if (dt.Rows.Count == 0)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return false;
            }

            bool interLock = false;

            string lockUserId = ComFunc.GetFld(dt, 0, Def_T_AR.LOCK_USER_ID);
            if (string.IsNullOrEmpty(lockUserId))
            {
                // このままロックしてOK
                interLock = true;
            }
            else
            {
                // タイムアウトチェック
                DateTime lockStartDate = ComFunc.GetFldToDateTime(dt, 0, Def_T_AR.LOCK_STARTDATE);
                DateTime timeoutDateTime = lockStartDate.AddMinutes(timeOut);
                if (timeoutDateTime < syoriDateTime)
                {
                    // タイムアウトです。
                    interLock = true;
                }
            }

            if (!interLock)
            {
                // 現在別のユーザーが編集中です。
                errMsgID = "A9999999002";
                return false;
            }

            // インターロック
            int cnt = UpdARInterLock(dbHelper, cond);

            if (cnt == 0)
            {
                // 更新に失敗しました。
                errMsgID = "A9999999014";
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

    #region AR情報データ インターロック解除

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック解除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">ARデータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ARInterUnLock(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            // コンディション作成
            DateTime syoriDateTime = DateTime.Now;
            cond.UpdateDate = syoriDateTime;

            // インターロック解除
            int cnt = UpdARInterUnLock(dbHelper, cond, dt);

            if (cnt == 0)
            {
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

    #region 出荷明細データのチェック
    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データのチェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>H.Tsunamura 2010/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetCheckMeisaiData(DatabaseHelper dbHelper, CondA1 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        DataTable dt = ds.Tables[ComDefine.DTTBL_UPDATE];   // 更新用データ
        DataTable dtCheck = ds.Tables[Def_T_AR.Name];       // 表示時取得データ

        // 行ロック
        DataTable dtAR = this.GetAndLockAR(dbHelper, cond, true);

        if (dtAR.Rows.Count == 0)
        {
            // 該当の明細は存在しません。
            errMsgID = "A9999999022";
            return false;
        }

        // インターロックユーザーのチェック
        string lockUserID = ComFunc.GetFld(dtAR, 0, Def_T_AR.LOCK_USER_ID);
        if (!string.IsNullOrEmpty(lockUserID))
        {
            DateTime myLockStartDate = ComFunc.GetFldToDateTime(dtCheck, 0, Def_T_AR.LOCK_STARTDATE);
            DateTime nowLockStartDate = ComFunc.GetFldToDateTime(dtAR, 0, Def_T_AR.LOCK_STARTDATE);

            if (myLockStartDate != nowLockStartDate)
            {
                // 編集のタイムアウトが発生し、現在別のユーザーが編集中です。
                errMsgID = "A9999999003";
                return false;
            }
        }

        string jyokyoFlag = ComFunc.GetFld(dt, 0, Def_T_AR.JYOKYO_FLAG);
        if (jyokyoFlag == JYOKYO_FLAG_AR.KANRYO_VALUE1)
        {
            // 出荷明細データ存在チェック
            if (!IsExistenceShukkaMeisai(dbHelper, cond))
            {
                // 梱包未登録です。完了してもよろしいですか？
                errMsgID = "A0100020008";
                return true;
            }
            // 出荷明細データ出荷日チェック
            if (IsNonShukkaDateShukkaMeisai(dbHelper, cond))
            {
                // 全ての出荷が終わっていません。完了してもよろしいですか？
                errMsgID = "A0100020003";
                return true;
            }
        }
        return true;
    }
    #endregion

    #region 元ARNOチェック
    /// --------------------------------------------------
    /// <summary>
    /// 元ARNOチェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル(元ARNO取得用)</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">エラーメッセージ引数</param>
    /// <returns>true:元ARNO未設定、または重複無しで存在する、false:重複、または存在しない</returns>
    /// <create>T.Nukaga 2019/11/22 STEP12 AR7000番運用対応</create>
    /// <update>D.Okumura 2019/11/22 STEP12 AR7000番運用対応メソッドのリファクタリング</update>
    /// --------------------------------------------------
    private bool CheckMotoArNo(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // 念のため、データ有無チェック
        if (dt == null || dt.Rows.Count < 1)
        {
            return true;
        }

        string motoARNo = ComFunc.GetFld(dt, 0, Def_T_AR.MOTO_AR_NO);

        // 元ARNOが未設定であれば無条件でOKとする
        if (String.IsNullOrEmpty(motoARNo))
        {
            return true;
        }
        // 存在チェック (ロックあり)
        CondA1 checkCond = new CondA1(cond.LoginInfo);
        checkCond.ArNo = motoARNo;
        checkCond.NonyusakiCD = ComFunc.GetFld(dt, 0, Def_T_AR.NONYUSAKI_CD);
        DataTable dtCheck = this.GetAndLockAR(dbHelper, checkCond, true);
        if (dtCheck.Rows.Count == 0)
        {
            // 存在しない
            // 元ARNOは存在しません。確認してください。
            errMsgID = "A0100020027";
            return false;
        }

        // 重複チェック
        CondA1 tmpCond = new CondA1(cond.LoginInfo);
        tmpCond.ArNo = ComFunc.GetFld(dt, 0, Def_T_AR.AR_NO);
        tmpCond.NonyusakiCD = ComFunc.GetFld(dt, 0, Def_T_AR.NONYUSAKI_CD);
        DataTable dtCheckDup = this.GetMotoArNo(dbHelper, tmpCond, motoARNo);
        if (dtCheckDup.Rows.Count != 0)
        {
            // 元ARNOは他のAR情報明細で登録されているため設定できません。
            errMsgID = "A0100020026";
            return false;
        }
        return true;
    }

    #endregion

    #region 登録

    /// --------------------------------------------------
    /// <summary>
    /// AR情報 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="rNonyusakiCd">採番/取得した納入先コード</param>
    /// <param name="rArNo">採番したARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル追加対応</update>
    /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    public bool InsArMeisai(DatabaseHelper dbHelper, CondA1 cond, DataSet ds, ref string rNonyusakiCd, ref string rArNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // コンディション作成
            DateTime syoriDateTime = DateTime.Now;
            cond.UpdateDate = syoriDateTime;

            // 納入先マスタ存在チェック
            DataTable dtNonyusaki = this.GetNonyusaki(dbHelper, cond);
            string nonyusakiCd = string.Empty;
            bool isNonyusakiInsert = false;

            if (dtNonyusaki.Rows.Count == 0)
            {
                // 納入先マスタ作成
                if (!this.InsNonyusaki(dbHelper, cond, dtNonyusaki, ref nonyusakiCd, ref errMsgID, ref args))
                {
                    return false;
                }

                isNonyusakiInsert = true;
            }
            else
            {
                // 管理区分チェック
                string kanriFlag = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.KANRI_FLAG);
                if (kanriFlag == KANRI_FLAG.KANRYO_VALUE1)
                {
                    // 完了納入先となっています。管理者に確認して下さい。
                    errMsgID = "A0100020002";
                    return false;
                }
                // 納入先コード
                nonyusakiCd = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            }

            // コンディション作成
            cond.NonyusakiCD = nonyusakiCd;

            // AR情報データ作成
            if (!this.InsAR(dbHelper, cond, ds.Tables[Def_T_AR.Name], ref rArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // AR進捗データ作成
            if (!this.InsArShinchoku(dbHelper, cond, ds.Tables[ComDefine.DTTBL_ARSHINCHOKU_ADD], rArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // AR対応費用データ作成
            if (!this.InsARCost(dbHelper, cond, ds.Tables[Def_T_AR_COST.Name], rArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // AR添付ファイルデータ作成
            if (!this.InsARFile(dbHelper, cond, ds.Tables[Def_T_AR_FILE.Name], rArNo, ref errMsgID, ref args))
            {
                return false;
            }

            if (isNonyusakiInsert)
            {
                // 履歴作成
                if (!this.InsRireki(dbHelper, cond, dtNonyusaki, rArNo, OPERATION_FLAG.A0100010_REGIST_VALUE1, ref errMsgID, ref args))
                {
                    return false;
                }
            }

            // 履歴作成
            if (!this.InsRireki(dbHelper, cond, dtNonyusaki, rArNo, OPERATION_FLAG.A0100020_REGIST_VALUE1, ref errMsgID, ref args))
            {
                return false;
            }


            // ARメール通知を行う
            if (!this.SendArMail(dbHelper, cond, dtNonyusaki, rArNo, ds.Tables[Def_T_AR.Name], true, ref errMsgID, ref args))
            {
                return false;
            }

            // 納入先コードを返す
            rNonyusakiCd = nonyusakiCd;

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region 納入先マスタ 登録

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="rNonyusakiCd">納入先コード</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsNonyusaki(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string rNonyusakiCd, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 納入先コード採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.ARUS_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string nonyusakiCd;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out nonyusakiCd, out errMsgID))
                {
                    return false;
                }
                rNonyusakiCd = nonyusakiCd;
            }

            // データ作成
            DataRow dr = dt.NewRow();
            dr[Def_M_NONYUSAKI.SHUKKA_FLAG] = SHUKKA_FLAG.AR_VALUE1;
            dr[Def_M_NONYUSAKI.NONYUSAKI_CD] = nonyusakiCd;
            dr[Def_M_NONYUSAKI.NONYUSAKI_NAME] = cond.NonyusakiName;
            dr[Def_M_NONYUSAKI.SHIP] = "";
            dr[Def_M_NONYUSAKI.KANRI_FLAG] = KANRI_FLAG.MIKAN_VALUE1;
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME0] = cond.ListFlagName0;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME1] = cond.ListFlagName1;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME2] = cond.ListFlagName2;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME3] = cond.ListFlagName3;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME4] = cond.ListFlagName4;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME5] = cond.ListFlagName5;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME6] = cond.ListFlagName6;
            dr[Def_M_NONYUSAKI.LIST_FLAG_NAME7] = cond.ListFlagName7;
            // @@@ ↑
            dr[Def_M_NONYUSAKI.BUKKEN_NO] = cond.BukkenNo;
            dt.Rows.Add(dr);

            CondNonyusaki condN = new CondNonyusaki(cond.LoginInfo);
            condN.UpdateDate = cond.UpdateDate;
            condN.LoginInfo = cond.LoginInfo;

            // Insert
            this.InsNonyusakiExec(dbHelper, condN, dr);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR情報データ 登録

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="rArNo">採番したARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update>D.Okumura 2019/08/07 AR進捗対応</update>
    /// --------------------------------------------------
    private bool InsAR(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string rArNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ARNo採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.AR_NO_VALUE1;
            condSms.ARUS = cond.NonyusakiCD;
            //condSms.NonyusakiCD = cond.NonyusakiCD;
            condSms.ListFlag = cond.ListFlag;
            string arNo;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out arNo, out errMsgID))
                {
                    return false;
                }
            }

            // データ作成
            DataRow dr = dt.Rows[0];
            dr[Def_T_AR.NONYUSAKI_CD] = cond.NonyusakiCD;
            dr[Def_T_AR.LIST_FLAG] = cond.ListFlag;
            dr[Def_T_AR.AR_NO] = arNo;

            // 元ARNOチェック
            if (!CheckMotoArNo(dbHelper, cond, dt, ref errMsgID, ref args))
            {
                return false;
            }

            // Insert
            this.InsARExec(dbHelper, cond, dr);

            // 機種の存在チェック
            if (AR_SHINCHOKU_FLAG.ON_VALUE1.Equals(ComFunc.GetFld(dr, Def_T_AR.SHINCHOKU_FLAG)) && !string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_AR.KISHU)))
            {
                CondA1 condN = new CondA1(cond.LoginInfo);
                condN.NonyusakiCD = cond.NonyusakiCD;
                condN.Kishu = ComFunc.GetFld(dr, Def_T_AR.KISHU);

                DataTable dtKisyu = this.GetKishu(dbHelper, condN);
                if (!UtilData.ExistsData(dtKisyu))
                {
                    // 機種が存在しません。再度選択してください。
                    errMsgID = "A0100020024";
                    return false;
                }
            }

            // AR Noを返す
            rArNo = arNo;

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion


    #region AR進捗データ 登録
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="ArNo">ARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsArShinchoku(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string arNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // データ作成
            foreach (DataRow dr in dt.Rows)
            {
                // 情報を補完する
                dr[Def_T_AR_SHINCHOKU.NONYUSAKI_CD] = cond.NonyusakiCD;
                dr[Def_T_AR_SHINCHOKU.LIST_FLAG] = cond.ListFlag;
                dr[Def_T_AR_SHINCHOKU.AR_NO] = arNo;

            }

            // Insert
            this.InsArShinchokuExec(dbHelper, cond, dt);

            // 号機の存在チェック
            DataTable dtNotFound = this.GetGokiNotExists(dbHelper, cond, arNo);
            if (UtilData.ExistsData(dtNotFound))
            {
                // 存在しない号機があります。号機を再度選択してください。
                errMsgID = "A0100020023";
                args = new[] { string.Join(cond.SeparatorItem.ToString(), dtNotFound.AsEnumerable().Select(w => ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.GOKI)).ToArray()) };
                return false;
            }
            // AR新規登録では進捗登録のメールは不要

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR対応費用データ 登録

    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="ArNo">ARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsARCost(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string arNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // データ作成
            foreach (DataRow dr in dt.Rows)
            {
                dr[Def_T_AR_COST.NONYUSAKI_CD] = cond.NonyusakiCD;
                dr[Def_T_AR_COST.LIST_FLAG] = cond.ListFlag;
                dr[Def_T_AR_COST.AR_NO] = arNo;
            }

            // Insert
            this.InsARCostExec(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 添付ファイルデータ 登録
    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイルデータ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="ArNo">ARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/06/18 添付ファイル対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsARFile(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string arNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 登録対象なしの場合は何もしない
            if (dt.Rows.Count < 1)
                return true;
            // データ作成
            foreach (DataRow dr in dt.Rows)
            {
                dr[Def_T_AR_FILE.NONYUSAKI_CD] = cond.NonyusakiCD;
                dr[Def_T_AR_FILE.LIST_FLAG] = cond.ListFlag;
                dr[Def_T_AR_FILE.AR_NO] = arNo;
            }

            // Insert
            if (this.InsARFileExec(dbHelper, cond, dt) > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 履歴データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 履歴データ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="arNo">ARNO</param>
    /// <param name="operationFlag">操作区分</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsRireki(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string arNo, string operationFlag, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtRireki = new DataTable(Def_T_RIREKI.Name);
            dtRireki.Columns.Add(Def_T_RIREKI.GAMEN_FLAG);
            dtRireki.Columns.Add(Def_T_RIREKI.SHUKKA_FLAG);
            dtRireki.Columns.Add(Def_T_RIREKI.NONYUSAKI_CD);
            dtRireki.Columns.Add(Def_T_RIREKI.SHIP);
            dtRireki.Columns.Add(Def_T_RIREKI.AR_NO);
            dtRireki.Columns.Add(Def_T_RIREKI.BUKKEN_NO);
            dtRireki.Columns.Add(Def_T_RIREKI.OPERATION_FLAG);
            dtRireki.Columns.Add(Def_T_RIREKI.UPDATE_PC_NAME);
            dtRireki.Columns.Add(Def_T_RIREKI.UPDATE_DATE);
            dtRireki.Columns.Add(Def_T_RIREKI.UPDATE_USER_ID);
            dtRireki.Columns.Add(Def_T_RIREKI.UPDATE_USER_NAME);

            // データ作成
            DataRow dr = dtRireki.NewRow();
            dr[Def_T_RIREKI.GAMEN_FLAG] = GAMEN_FLAG.A0100010_VALUE1;
            dr[Def_T_RIREKI.SHUKKA_FLAG] = SHUKKA_FLAG.AR_VALUE1;
            dr[Def_T_RIREKI.NONYUSAKI_CD] = ComFunc.GetFldObject(dt, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            dr[Def_T_RIREKI.SHIP] = ComFunc.GetFldObject(dt, 0, Def_M_NONYUSAKI.SHIP, string.Empty);
            dr[Def_T_RIREKI.AR_NO] = arNo;
            dr[Def_T_RIREKI.BUKKEN_NO] = cond.BukkenNo;
            dr[Def_T_RIREKI.OPERATION_FLAG] = operationFlag;
            dr[Def_T_RIREKI.UPDATE_PC_NAME] = UtilSystem.GetUserInfo(false).MachineName;
            dr[Def_T_RIREKI.UPDATE_DATE] = ComFunc.GetFldObject(dt, 0, Def_M_NONYUSAKI.UPDATE_DATE);
            dr[Def_T_RIREKI.UPDATE_USER_ID] = ComFunc.GetFldObject(dt, 0, Def_M_NONYUSAKI.UPDATE_USER_ID);
            dr[Def_T_RIREKI.UPDATE_USER_NAME] = ComFunc.GetFldObject(dt, 0, Def_M_NONYUSAKI.UPDATE_USER_NAME);

            // Insert
            this.InsRirekiExec(dbHelper, cond, dr);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region 更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    public bool UpdArMeisai(DatabaseHelper dbHelper, CondA1 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // コンディション作成
            DateTime syoriDateTime = DateTime.Now;
            cond.UpdateDate = syoriDateTime;

            DataTable dt = ds.Tables[ComDefine.DTTBL_UPDATE];   // 更新用データ
            DataTable dtCheck = ds.Tables[Def_T_AR.Name];       // 表示時取得データ

            // 行ロック
            DataTable dtAR = this.GetAndLockAR(dbHelper, cond, true);

            if (dtAR.Rows.Count == 0)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return false;
            }

            // インターロックユーザーのチェック
            string lockUserID = ComFunc.GetFld(dtAR, 0, Def_T_AR.LOCK_USER_ID);
            if (!string.IsNullOrEmpty(lockUserID))
            {
                DateTime myLockStartDate = ComFunc.GetFldToDateTime(dtCheck, 0, Def_T_AR.LOCK_STARTDATE);
                DateTime nowLockStartDate = ComFunc.GetFldToDateTime(dtAR, 0, Def_T_AR.LOCK_STARTDATE);

                if (myLockStartDate != nowLockStartDate)
                {
                    // 編集のタイムアウトが発生し、現在別のユーザーが編集中です。
                    errMsgID = "A9999999003";
                    return false;
                }
            }

            // バージョンチェック
            int index;
            int[] notFountIndex = null;
            index = this.CheckSameData(dtCheck, dtAR, out notFountIndex, Def_T_AR.VERSION, Def_T_AR.NONYUSAKI_CD, Def_T_AR.LIST_FLAG, Def_T_AR.AR_NO);
            if (0 <= index)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            else if (notFountIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            /* 2010/10/29 綱村　明細データに関係なく完了させる。
            string jyokyoFlag = ComFunc.GetFld(dt, 0, Def_T_AR.JYOKYO_FLAG);
            if (jyokyoFlag == JYOKYO_FLAG_AR.KANRYO_VALUE1)
            {
                // 出荷明細データ存在チェック
                if (!IsExistenceShukkaMeisai(dbHelper, cond))
                {
                    // 梱包未登録です。完了出来ません。
                    errMsgID = "A0100020008";
                    return false;
                }
                // 出荷明細データ出荷日チェック
                if (IsNonShukkaDateShukkaMeisai(dbHelper, cond))
                {
                    // 全ての出荷が終わっていないので完了にできません。
                    errMsgID = "A0100020003";
                    return false;
                }
            }
            */

            // 納入先マスタ存在チェック
            DataTable dtNonyusaki;
            {
                var condN = new CondA1(cond.LoginInfo);
                condN.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
                condN.NonyusakiCD = cond.NonyusakiCD;
                condN.LoginInfo = cond.LoginInfo;
                dtNonyusaki = this.GetNonyusaki(dbHelper, condN);
                if (!UtilData.ExistsData(dtNonyusaki))
                {
                    // 納入先マスタが削除されています。
                    errMsgID = "A0100020011";
                    return false;
                }
                if (string.Equals(KANRI_FLAG.KANRYO_VALUE1, ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.KANRI_FLAG)))
                {
                    // 完了納入先となっています。管理者に確認して下さい。
                    errMsgID = "A0100020002";
                    return false;
                }
            }

            // AR情報データ更新
            if (!this.UpdAR(dbHelper, cond, dt, ref errMsgID, ref args))
            {
                return false;
            }

            // AR対応費用データ洗い替え
            var dtArCost = this.ExistsArCost(dbHelper, cond);
            if (UtilData.ExistsData(dtArCost))
            {
                // 削除
                var delCnt = this.DelArCost(dbHelper, dtArCost.Rows[0]);
                if (delCnt != dtArCost.Rows.Count)
                {
                    errMsgID = "A9999999015";
                    return false;
                }
            }

            // AR進捗データ作成
            if (!this.UpdArShinchoku(dbHelper, cond, ds.Tables[ComDefine.DTTBL_ARSHINCHOKU_ADD], ds.Tables[ComDefine.DTTBL_ARSHINCHOKU_DEL], cond.ArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // AR対応費用データ登録
            if (!this.InsARCost(dbHelper, cond, ds.Tables[Def_T_AR_COST.Name], cond.ArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // AR添付ファイル登録
            if (!this.UpdARFile(dbHelper, cond, ds.Tables[Def_T_AR_FILE.Name], cond.ArNo, ref errMsgID, ref args))
            {
                return false;
            }

            // 履歴作成
            if (!this.InsRireki(dbHelper, cond, dt, ComFunc.GetFld(dt, 0, Def_T_AR.AR_NO), OPERATION_FLAG.A0100020_EDIT_VALUE1, ref errMsgID, ref args))
            {
                return false;
            }

            // ARメール通知を行う
            if (!this.SendArMail(dbHelper, cond, dtNonyusaki, null, dt, false, ref errMsgID, ref args))
            {
                return false;
            }
            

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region AR情報データ 更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update>D.Okumura 2019/08/07 AR進捗対応</update>
    /// --------------------------------------------------
    private bool UpdAR(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 元ARNOチェック
            if (!CheckMotoArNo(dbHelper, cond, dt, ref errMsgID, ref args))
            {
                return false;
            }

            // データ作成

            // Update
            this.UpdARExec(dbHelper, cond, dt);

            foreach (DataRow dr in dt.Rows)
            {
                // 機種の存在チェック
                if (AR_SHINCHOKU_FLAG.ON_VALUE1.Equals(ComFunc.GetFld(dr, Def_T_AR.SHINCHOKU_FLAG)) && !string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_AR.KISHU)))
                {
                    CondA1 condN = new CondA1(cond.LoginInfo);
                    condN.NonyusakiCD = cond.NonyusakiCD;
                    condN.Kishu = ComFunc.GetFld(dr, Def_T_AR.KISHU);

                    DataTable dtKisyu = this.GetKishu(dbHelper, condN);
                    if (!UtilData.ExistsData(dtKisyu))
                    {
                        // 機種が存在しません。再度選択してください。
                        errMsgID = "A0100020024";
                        return false;
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


    #region AR進捗データ 登録
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dtInsert">データテーブル</param>
    /// <param name="dtDelete">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    private bool UpdArShinchoku(DatabaseHelper dbHelper, CondA1 cond, DataTable dtInsert, DataTable dtDelete, string arNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // データ作成
            foreach (DataRow dr in dtInsert.Rows)
            {
                // 情報を補完する
                dr[Def_T_AR_SHINCHOKU.NONYUSAKI_CD] = cond.NonyusakiCD;
                dr[Def_T_AR_SHINCHOKU.LIST_FLAG] = cond.ListFlag;
                dr[Def_T_AR_SHINCHOKU.AR_NO] = arNo;

            }

            // Insert
            this.InsArShinchokuExec(dbHelper, cond, dtInsert);

            // 号機の存在チェック
            DataTable dtNotFound = this.GetGokiNotExists(dbHelper, cond, arNo);
            if (UtilData.ExistsData(dtNotFound))
            {
                // 存在しない号機があります。号機を再度選択してください。
                errMsgID = "A0100020023";
                args = new[] { string.Join(cond.SeparatorItem.ToString(), dtNotFound.AsEnumerable().Select(w => ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.GOKI)).ToArray()) };
                return false;
            }

            if (dtDelete.Rows.Count > 0)
            {
                // 削除分はロックしてから操作を行う
                DataTable dtLockedData = this.GetArShinchokuInfoExec(dbHelper, cond, dtDelete, true);

                // バージョンチェック
                int index;
                int[] notFountIndex = null;
                index = this.CheckSameData(dtLockedData, dtDelete, out notFountIndex, Def_T_AR_SHINCHOKU.VERSION, Def_T_AR_SHINCHOKU.NONYUSAKI_CD, Def_T_AR_SHINCHOKU.LIST_FLAG, Def_T_AR_SHINCHOKU.AR_NO, Def_T_AR_SHINCHOKU.GOKI);
                if (0 <= index || notFountIndex != null)
                {
                    // 他端末で更新された為、更新できませんでした。
                    errMsgID = "A9999999027";
                    return false;
                }

                // 履歴データ作成
                DataTable dtDeleteHistory = this.CreateArShinchokuRirekiOnDelete(dtDelete);

                // Delete
                this.DelArShinchokuExec(dbHelper, cond, dtDelete);

                // 削除対象の履歴
                this.InsArShinchokuRirekiExec(dbHelper, cond, dtDeleteHistory);
            }
            // メール送信処理
            if (dtDelete.Rows.Count > 0 && dtDelete.AsEnumerable()
                .Any(w => !string.IsNullOrEmpty(ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.DATE_SITE_REQ).Trim())))
            {
                // 現地到着希望日に記入があり、対象の号機が削除されたとき、変更通知を発行する
                if (!this.SendArShinchokuMail(dbHelper, cond, new string[] { arNo }, ref errMsgID, ref args))
                {
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

    /// --------------------------------------------------
    /// <summary>
    /// 進捗履歴データスキーマ作成
    /// </summary>
    /// <returns>データテーブル(T_AR_SHINCHOKU_RIREKI)</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetSchemaArShinchokuRireki()
    {
        var dt = new DataTable(Def_T_AR_SHINCHOKU_RIREKI.Name);
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.LIST_FLAG, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.AR_NO, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.GOKI, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_BEFORE, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.DATE_AFTER, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.NOTE, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.UPDATE_DATE, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.UPDATE_USER_ID, typeof(string));
        dt.Columns.Add(Def_T_AR_SHINCHOKU_RIREKI.UPDATE_USER_NAME, typeof(string));
        return dt;
    }

    /// --------------------------------------------------
    /// <summary>
    /// 削除対象の履歴データ作成
    /// </summary>
    /// <param name="dtInput">データテーブル(T_AR_SHINCHOKU)</param>
    /// <returns>データテーブル(T_AR_SHINCHOKU_RIREKI)</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CreateArShinchokuRirekiOnDelete(DataTable dtInput)
    {
        var dt = this.GetSchemaArShinchokuRireki();
        var fields = new Dictionary<string, string>();
        fields.Add(AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1, Def_T_AR_SHINCHOKU.DATE_SITE_REQ);
        fields.Add(AR_DATE_KIND_DISP_FLAG.JP_VALUE1, Def_T_AR_SHINCHOKU.DATE_JP);
        fields.Add(AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1, Def_T_AR_SHINCHOKU.DATE_LOCAL);
        foreach (DataRow drInput in dtInput.Rows)
        {
            foreach (var item in fields)
            {
                string date = ComFunc.GetFld(drInput, item.Value);
                if (string.IsNullOrEmpty(date.Trim()))
                    continue;
                DataRow dr = dt.NewRow();
                dr[Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD] = ComFunc.GetFld(drInput, Def_T_AR_SHINCHOKU.NONYUSAKI_CD);
                dr[Def_T_AR_SHINCHOKU_RIREKI.LIST_FLAG] = ComFunc.GetFld(drInput, Def_T_AR_SHINCHOKU.LIST_FLAG);
                dr[Def_T_AR_SHINCHOKU_RIREKI.AR_NO] = ComFunc.GetFld(drInput, Def_T_AR_SHINCHOKU.AR_NO);
                dr[Def_T_AR_SHINCHOKU_RIREKI.GOKI] = ComFunc.GetFld(drInput, Def_T_AR_SHINCHOKU.GOKI);
                dr[Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND] = item.Key;
                dr[Def_T_AR_SHINCHOKU_RIREKI.DATE_BEFORE] = date;
                dr[Def_T_AR_SHINCHOKU_RIREKI.DATE_AFTER] = string.Empty;
                dr[Def_T_AR_SHINCHOKU_RIREKI.NOTE] = DBNull.Value;
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    #endregion

    #region 添付ファイルデータ 更新
    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイルデータ 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="ArNo">ARNo</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool UpdARFile(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string arNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            // AR添付ファイル洗い替え
            this.DelARFile(dbHelper, cond);
            // 登録対象なしの場合は何もしない
            if (dt.Rows.Count < 1)
                return true;
            // データ作成
            foreach (DataRow dr in dt.Rows)
            {
                dr[Def_T_AR_FILE.NONYUSAKI_CD] = cond.NonyusakiCD;
                dr[Def_T_AR_FILE.LIST_FLAG] = cond.ListFlag;
                dr[Def_T_AR_FILE.AR_NO] = arNo;
            }

            // Insert
            if (this.InsARFileExec(dbHelper, cond, dt) > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ＡＲ対応費用 (T_AR_COST)の存在チェック

    /// --------------------------------------------------
    /// <summary>
    /// ＡＲ対応費用 (T_AR_COST)の存在チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>キー情報が存在する場合はtrue、それ以外はfalse</returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable ExistsArCost(DatabaseHelper dbHelper, CondA1 cond)
    {
        var ret = new DataTable(Def_T_AR_COST.Name);
        StringBuilder sb = new StringBuilder();
        DbParamCollection paramCollection = new DbParamCollection();
        INewDbParameterBasic iNewParam = dbHelper;

        sb.ApdL("SELECT");
        sb.ApdL("       T_AR_COST.NONYUSAKI_CD");
        sb.ApdL("     , T_AR_COST.LIST_FLAG");
        sb.ApdL("     , T_AR_COST.AR_NO");
        sb.ApdL("  FROM");
        sb.ApdL("       T_AR_COST");
        sb.ApdL(" WHERE");
        sb.ApdL("       T_AR_COST.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
        sb.ApdL("   AND T_AR_COST.LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
        sb.ApdL("   AND T_AR_COST.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

        // バインド変数設定
        paramCollection = new DbParamCollection();
        paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
        paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
        paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));

        // SQL実行
        dbHelper.Fill(sb.ToString(), paramCollection, ret);

        return ret;
    }

    #endregion

    #region AR対応費用 削除

    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用 削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="dr">データロウ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelArCost(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_AR_COST");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_COST.NONYUSAKI_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_COST.LIST_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_COST.AR_NO)));

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

    #region AR添付ファイル 削除

    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイル 削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>D.Okumura 2019/06/18 添付ファイル追加対応</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARFile(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_AR_FILE");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));

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
    
    #region AR情報メール設定取得
    /// --------------------------------------------------
    /// <summary>
    /// AR情報メール設定取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション(bukkenNo,listFlag,IsToroku)</param>
    /// <param name="errMsgID">エラーメッセージ</param>
    /// <param name="args">エラーパラメータ</param>
    /// <returns>データセット</returns>
    /// <create>D.Okumura 2019/08/07 AR進捗対応</create>
    /// <update>J.Chen 2024/08/22 メール通知フラグ取得</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetArMailInfo(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID, ref string[] args, ref bool _isNotify)
    {
        try
        {
            // ARの基本通知設定を取得する
            bool isNotify;
            var ds = this.GetArMailBaseInfo(dbHelper, cond, cond.IsToroku, cond.BukkenNo, out isNotify, ref errMsgID, ref args);
            _isNotify = isNotify;
            if (isNotify)
            {
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    return ds;
                }
                // リスト区分ごとにチェックする
                var dic = this.GetArMailBukkenInfo(dbHelper, cond, cond.BukkenNo, new [] {cond.ListFlag}, ref errMsgID, ref args);
                if (dic.ContainsKey(cond.ListFlag))
                {
                    var dt = dic[cond.ListFlag];
                    dt.TableName = ComDefine.DTTBL_AR_MAILINFO;
                    ds.Merge(dt);
                }
                if (!string.IsNullOrEmpty(errMsgID))
                {
                    return ds;
                }
            }
            // 進捗メール通知設定を取得する
            if (cond.IsShinchoku)
            {
                var dtShinchoku = this.GetArShinchokuMailInfo(dbHelper, cond, cond.BukkenNo, ref errMsgID, ref args);
                dtShinchoku.TableName = ComDefine.DTTBL_ARSHINCHOKU_MAILINFO;
                ds.Merge(dtShinchoku);
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

    #region A0100040:員数表取込

    #region 号機チェック
    /// --------------------------------------------------
    /// <summary>
    /// 号機チェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="dtMessage"></param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>T.Nakata 2019/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool CheckGoki(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref DataTable dtMessage)
    {
        bool retVal = true;
        DateTime syoriDateTime = DateTime.Now;
        cond.UpdateDate = syoriDateTime;
        // 一時ID 採番
        CondSms condSms = new CondSms(cond.LoginInfo);
        condSms.SaibanFlag = SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1;
        condSms.LoginInfo = cond.LoginInfo;
        string tempId;
        using (WsSmsImpl impl = new WsSmsImpl())
        {
            string saibanErrorMsgId;
            if (!impl.GetSaiban(dbHelper, condSms, out tempId, out saibanErrorMsgId))
            {
                ComFunc.AddMultiMessage(dtMessage, saibanErrorMsgId);
                return false;
            }
        }
        // 一時取込
        this.InsInzuhyoWorkExec(dbHelper, cond, dt, tempId);

        // 入力チェック
        retVal &= this.CheckGokiInner(dbHelper, cond, dtMessage, tempId);

        // 一時取込クリア
        this.DelInzuhyoWorkExec(dbHelper, tempId);

        return retVal;
    }
    /// --------------------------------------------------
    /// <summary>
    /// 号機チェック(共通)
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond">A01条件</param>
    /// <param name="dtMessage">マルチメッセージ</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>true:チェックOK false:失敗</returns>
    /// <create>D.Okumura 2019/07/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckGokiInner(DatabaseHelper dbHelper, CondA1 cond, DataTable dtMessage, string tempId)
    {
        bool retVal = true;
        // 削除対象で存在しないものを抽出
        var dtNotFound = this.CheckInzuhyoWorkNotFoundOnDelete(dbHelper, cond, tempId);
        foreach (DataRow dr in dtNotFound.Rows)
        {
            // 号機{0}は既に削除されています。
            ComFunc.AddMultiMessage(dtMessage, "A0100040012", ComFunc.GetFld(dr, Def_T_AR_GOKI_TEMPWORK.GOKI));
            retVal = false;
        }

        // 削除対象で使用中のものを抽出
        var dtDelete = this.CheckInzuhyoWorkUsedOnDelete(dbHelper, cond, tempId);
        foreach (DataRow dr in dtDelete.Rows)
        {
            // 号機{0}は既に使用されています。
            ComFunc.AddMultiMessage(dtMessage, "A0100040010", ComFunc.GetFld(dr, Def_T_AR_GOKI_TEMPWORK.GOKI));
            retVal = false;
        }

        // 使用中で存在しなくなる機種を抽出
        var dtKishu = this.CheckInzuhyoWorkUsedOnUpdate(dbHelper, cond, tempId);
        foreach (DataRow dr in dtKishu.Rows)
        {
            // 機種{0}が存在しなくなります。使用中のため、1件以上残してください。
            ComFunc.AddMultiMessage(dtMessage, "A0100040013", ComFunc.GetFld(dr, Def_T_AR.KISHU));
            retVal = false;
        }


        return retVal;
    }

    #endregion

    #region 員数表登録
    /// --------------------------------------------------
    /// <summary>
    /// 員数表登録
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="dtMessage"></param>
    /// <param name="args"></param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>T.Nakata 2019/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsInzuhyo(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string errMsgID, ref DataTable dtMessage, ref string[] args)
    {
        bool retVal = true;
        DateTime syoriDateTime = DateTime.Now;
        cond.UpdateDate = syoriDateTime;
        // 一時ID 採番
        CondSms condSms = new CondSms(cond.LoginInfo);
        condSms.SaibanFlag = SAIBAN_FLAG.TEMP_AR_GOKI_VALUE1;
        condSms.LoginInfo = cond.LoginInfo;
        string tempId;
        using (WsSmsImpl impl = new WsSmsImpl())
        {
            string saibanErrorMsgId;
            if (!impl.GetSaiban(dbHelper, condSms, out tempId, out saibanErrorMsgId))
            {
                ComFunc.AddMultiMessage(dtMessage, saibanErrorMsgId);
                return false;
            }
        }
        // 一時取込
        this.InsInzuhyoWorkExec(dbHelper, cond, dt, tempId);

        // ロック
        this.GetInzuhyoLock(dbHelper, cond, tempId, true);

        // 入力チェック
        retVal &= this.CheckGokiInner(dbHelper, cond, dtMessage, tempId);

        // 更新処理
        if (retVal)
        {
            // 削除
            this.DelInzuhyoExec(dbHelper, cond, tempId);
            // 挿入または更新
            this.MergeInzuhyoExec(dbHelper, cond, tempId);
        }
        // 一時取込クリア
        this.DelInzuhyoWorkExec(dbHelper, tempId);

        return retVal;
    }
    #endregion

    #endregion

    #region A0100050:ＡＲ進捗管理

    #region 進捗情報検索
    /// --------------------------------------------------
    /// <summary>
    /// 進捗情報検索
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetARShinchokuList(DatabaseHelper dbHelper, CondA1 cond)
    {
        return GetArShinchokuListExec(dbHelper, cond);
    }
    #endregion

    #region AR進捗のメール送信
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗のメール送信
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データセット</returns>
    /// <remarks>進捗画面Close時の操作。送信条件は画面にて判断。</remarks>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    public bool SendArShinchokuMail(DatabaseHelper dbHelper, CondA1 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        string[] arList = ds.Tables[0].AsEnumerable()
            .Select(w => ComFunc.GetFld(w, ComDefine.FLD_ARSHINCHOKU_ARNO))
            .Where(w => !string.IsNullOrEmpty(w))
            .OrderBy(w => w)
            .ToArray();
        return this.SendArShinchokuMail(dbHelper, cond, arList, ref errMsgID, ref args);
    }

    #endregion

    #endregion //A0100050:ＡＲ進捗管理

    #region A0100051:ＡＲ進捗管理日付登録

    #region ARインターンロックおよび進捗情報取得
    /// --------------------------------------------------
    /// <summary>
    /// ARインターンロックおよび進捗情報取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">選択済みデータテーブル(T_AR_SHINCHOKU_RIREKI)</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データセット</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetArInterLockAndShinchokuInfo(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        DataSet ds = new DataSet();
        DataTable dtAr;

        // フィールド名を取得
        string dateKbn = ComFunc.GetFld(dt, 0, Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND);
        string dateField = SetupConditionGetShinchokuDateFieldFromDateKind(dateKbn);

        // リスト区分一覧を作成
        string[] listFlagList = dt.AsEnumerable()
            .Select(w => ComFunc.GetFld(w, Def_T_AR_SHINCHOKU_RIREKI.AR_NO))
            .Select(w => w.Substring(2, 1))
            .Distinct()
            .OrderBy(w => w)
            .ToArray();

        // 納入先状態を確認
        string bukkenNo;
        {
            var condN = new CondA1(cond.LoginInfo);
            condN.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
            condN.NonyusakiCD = cond.NonyusakiCD;
            condN.LoginInfo = cond.LoginInfo;
            DataTable　dtNonyusaki = this.GetNonyusaki(dbHelper, condN);
            if (!UtilData.ExistsData(dtNonyusaki))
            {
                // 納入先マスタが削除されています。
                errMsgID = "A0100020011";
                return ds;
            }
            if (string.Equals(KANRI_FLAG.KANRYO_VALUE1, ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.KANRI_FLAG)))
            {
                // 完了納入先となっています。管理者に確認して下さい。
                errMsgID = "A0100020002";
                return ds;
            }
            bukkenNo = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.BUKKEN_NO);
        }
        // メール通知状態をチェックする
        bool isNotify;
        this.GetArMailBaseInfo(dbHelper, cond, false, bukkenNo, out isNotify, ref errMsgID, ref args);
        if (!string.IsNullOrEmpty(errMsgID))
        {
            return ds;
        }
        if (isNotify)
        {
            // リスト区分ごとにチェックする
            this.GetArMailBukkenInfo(dbHelper, cond, bukkenNo, listFlagList, ref errMsgID, ref args);
        }
        if (!string.IsNullOrEmpty(errMsgID))
        {
            return ds;
        }

        // 現地日付の場合、メール登録状況をチェックする
        if (AR_DATE_KIND_FLAG.SITE_REQ_VALUE1.Equals(dateKbn))
        {
            this.GetArShinchokuMailInfo(dbHelper, cond, bukkenNo, ref errMsgID, ref args);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                return ds;
            }
        }

        // インターンロック
        if (!GetARAndInterLockShinchoku(dbHelper, cond, dt, out dtAr, ref errMsgID, ref args))
        {
            return ds;
        }
        ds.Merge(dtAr);

        // 進捗情報取得
        DataTable dtArShinchoku = GetArShinchokuInfoExec(dbHelper, cond, dt, false);

        // 取得件数確認: 取得に失敗したレコードがある場合、ほかの端末により削除されているとみなす
        if (dtArShinchoku.Rows.Count != dt.Rows.Count)
        {
            // 他の端末で削除されています
            errMsgID = "A9999999079";
            return ds;
        }
        ds.Merge(dtArShinchoku);

        // 取得した進捗情報データから画面表示用の日付を計算する
        string minDate = dtArShinchoku.AsEnumerable()
            .Select(row => ComFunc.GetFld(row, dateField))
            .Where(w => !String.IsNullOrEmpty(w.Trim()))
            .Min() ?? string.Empty;

        // フォーム表示用データ
        DataTable dtArFormData = new DataTable(ComDefine.DTTBL_ARSHINCHOKU_DT);
        dtArFormData.Columns.Add(ComDefine.FLD_ARSHINCHOKU_DT_DATE, typeof(string)); // 日付
        DataRow drDate = dtArFormData.NewRow();
        drDate[ComDefine.FLD_ARSHINCHOKU_DT_DATE] = minDate;
        dtArFormData.Rows.Add(drDate);
        ds.Merge(dtArFormData);

        return ds;
    }
    #endregion


    #region AR情報のメールチェック処理
    /// --------------------------------------------------
    /// <summary>
    /// AR情報のメール基本情報取得処理
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="condA">A01用コンディション</param>
    /// <param name="isInsert">true:登録/false:更新</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <param name="isNotify">通知有無</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データセット</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet GetArMailBaseInfo(DatabaseHelper dbHelper, CondA1 condA, bool isInsert, string bukkenNo, out bool isNotify, ref string errMsgID, ref string[] args)
    {
        DataSet ds = new DataSet();
        CondA1 cond = new CondA1(condA.LoginInfo);
        cond.BukkenNo = bukkenNo;
        cond.IsToroku = !isInsert;
        cond.UpdateUserID = condA.LoginInfo.UserID;

        // メール設定マスタ取得
        var dtMailSetting = this.GetMailSetting(dbHelper);
        ds.Merge(dtMailSetting);

        // メールマスタの設定で、メール送信対象になっているか判別
        // →メール通知しないなら正常終了
        string colName = isInsert ? Def_M_MAIL_SETTING.AR_ADD_EVENT : Def_M_MAIL_SETTING.AR_UPDATE_EVENT;
        if (UtilData.GetFld(dtMailSetting, 0, colName) == AR_ADD_EVENT.NO_VALUE1)
        {
            isNotify = false;
        }
        else
        {
            // 物件マスタの設定で、メール送信対象になっているか判別
            // →メール通知しないなら正常終了
            var dtBukken = this.GetBukken(dbHelper, cond);
            if (UtilData.ExistsData(dtBukken)
                && UtilData.GetFld(dtBukken, 0, Def_M_BUKKEN.MAIL_NOTIFY) == MAIL_NOTIFY.STOP_VALUE1)
            {
                isNotify = false;
            }
            else
            {
                isNotify = true;
            }
        }

        // ユーザーID未定義ならエラー
        if (string.IsNullOrEmpty(cond.UpdateUserID))
        {
            // 担当者が存在しません。
            errMsgID = "A9999999080";
            return ds;
        }

        // ユーザーマスタ取得
        var dtUser = this.GetUser(dbHelper, cond);
        ds.Merge(dtUser);
        if (!UtilData.ExistsData(dtUser))
        {
            // 担当者が存在しません。
            errMsgID = "A9999999080";
            return ds;
        }

        // メールアドレスチェック
        if (string.IsNullOrEmpty(UtilData.GetFld(dtUser, 0, Def_M_USER.MAIL_ADDRESS)))
        {
            // 担当者にMailAddressが設定されていません。
            errMsgID = "A9999999081";
            return ds;
        }

        // メールテンプレートを取得(ファイル操作)
        string fileTitle;
        string fileBody;
        if (isInsert)
        {
            fileTitle = MAIL_FILE.ADD_TITLE_VALUE1;
            fileBody = MAIL_FILE.ADD_VALUE1;
        }
        else
        {
            fileTitle = MAIL_FILE.UPD_TITLE_VALUE1;
            fileBody = MAIL_FILE.UPD_VALUE1;
        }
        ds.Merge(this.GetMailTemplate(condA, fileTitle, fileBody, ref errMsgID, ref args));

        return ds;
    }

    /// --------------------------------------------------
    /// <summary>
    /// AR情報の物件メール情報取得処理
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <param name="listFlagList">リスト区分の一覧</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データセット(List区分,テーブル)</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private Dictionary<string, DataTable> GetArMailBukkenInfo(DatabaseHelper dbHelper, CondA1 cond, string bukkenNo, string[] listFlagList, ref string errMsgID, ref string[] args)
    {
        var dic = new Dictionary<string, DataTable>();

        foreach (var listFlag in listFlagList)
        {
            // リスト別メール情報チェック(AR + 物件No + メール区分(ARList) + リスト区分)
            var dtBukkenMail = this.GetMailNotification(dbHelper, cond, MAIL_KBN.ARLIST_VALUE1, bukkenNo, listFlag);

            if (!UtilData.ExistsData(dtBukkenMail))
            {
                // 共通メール情報チェック(AR + 物件No + メール区分(共通))
                dtBukkenMail = this.GetMailNotification(dbHelper, cond, MAIL_KBN.COMMON_VALUE1, bukkenNo, null);

                if (!UtilData.ExistsData(dtBukkenMail))
                {
                    // 共通通知設定で送信先が登録されていません。
                    errMsgID = "A9999999082";
                    return dic;
                }
            }

            foreach (DataRow dr in dtBukkenMail.Rows)
            {
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
                {
                    // MailAddress未設定のUserがいます。【ID：{0}、Name：{1}】
                    errMsgID = "A9999999060";
                    args = new[] { ComFunc.GetFld(dr, Def_M_USER.USER_ID), ComFunc.GetFld(dr, Def_M_USER.USER_NAME) };
                    return dic;
                }
            }
            dic.Add(listFlag, dtBukkenMail);
        }
        return dic;
    }
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報のメール情報取得処理
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データテーブル</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetArShinchokuMailInfo(DatabaseHelper dbHelper, CondA1 cond, ref string errMsgID, ref string[] args)
    {
        var ds = new DataSet();
        ds.Merge(this.GetArShinchokuMailInfo(dbHelper, cond, cond.BukkenNo, ref errMsgID, ref args));
        return ds;
    }
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報のメール情報取得処理
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データテーブル</returns>
    /// <create>D.Okumura 2019/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetArShinchokuMailInfo(DatabaseHelper dbHelper, CondA1 cond, string bukkenNo, ref string errMsgID, ref string[] args)
    {
        var dic = new Dictionary<string, DataTable>();

        // 個別メール情報チェック(AR + 物件No + メール区分(個別) + リスト区分)
        var dtBukkenMail = this.GetMailNotification(dbHelper, cond, MAIL_KBN.ARSHINCHOKU_KOBETU_VALUE1, bukkenNo, null);

        if (!UtilData.ExistsData(dtBukkenMail))
        {
            // Defaultメール情報チェック(メール区分(共通))
            dtBukkenMail = this.GetMailNotification(dbHelper, cond, MAIL_KBN.ARSHINCHOKU_VALUE1, null, null);

            if (!UtilData.ExistsData(dtBukkenMail))
            {
                // 基本通知設定で送信先が登録されていません。
                errMsgID = "A9999999057";
                return dtBukkenMail;
            }
        }

        foreach (DataRow dr in dtBukkenMail.Rows)
        {
            if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
            {
                // MailAddress未設定のUserがいます。【ID：{0}、Name：{1}】
                errMsgID = "A9999999060";
                args = new[] { ComFunc.GetFld(dr, Def_M_USER.USER_ID), ComFunc.GetFld(dr, Def_M_USER.USER_NAME) };
                return dtBukkenMail;
            }
        }
        return dtBukkenMail;
    }
    #endregion

    #region AR情報のインターンロックを取得 (内部処理)
    /// --------------------------------------------------
    /// <summary>
    /// AR情報のインターンロックを取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dtInput">ロック取得用データテーブル(T_AR_SHINCHOKU_RIREKI)</param>
    /// <param name="dt">データテーブル(T_AR)</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>ロック結果</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool GetARAndInterLockShinchoku(DatabaseHelper dbHelper, CondA1 cond, DataTable dtInput, out DataTable dt, ref string errMsgID, ref string[] args)
    {
        // コンディション作成
        DateTime syoriDateTime = DateTime.Now;
        cond.UpdateDate = syoriDateTime;
        cond.NonyusakiCD = ComFunc.GetFld(dtInput, 0, Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD);
        double timeOut = 0;

        // タイムアウト時間取得
        using (WsCommonImpl impl = new WsCommonImpl())
        {
            CondCommon condC = new CondCommon(cond.LoginInfo);
            condC.GroupCD = INTERLOCK_TIMEOUT.GROUPCD;
            condC.ItemCD = INTERLOCK_TIMEOUT.VALUE_NAME;
            condC.LoginInfo = cond.LoginInfo;
            DataSet ds = impl.GetCommon(dbHelper, condC);
            timeOut = UtilConvert.ToDouble(ComFunc.GetFld(ds, Def_M_COMMON.Name, 0, Def_M_COMMON.VALUE1));
        }

        // ARNoリストを作成
        string[] arList = dtInput.AsEnumerable()
            .Select(w => ComFunc.GetFld(w, Def_T_AR_SHINCHOKU_RIREKI.AR_NO))
            .Distinct()
            .OrderBy(w => w)
            .ToArray();

        // AR情報データ 取得(行ロック)
        dt = this.GetAndLockARMultiple(dbHelper, cond, true, arList);

        // 全件取得確認
        if (dt.Rows.Count != arList.Length)
        {
            // 該当の明細は存在しません。
            errMsgID = "A9999999022";
            return false;
        }

        // ロック確認
        List<string> lockFailedList = new List<string>();
        foreach (DataRow dr in dt.Rows)
        {
            bool interLock = false;
            string lockUserId = ComFunc.GetFld(dr, Def_T_AR.LOCK_USER_ID);
            if (string.IsNullOrEmpty(lockUserId))
            {
                // このままロックしてOK
                interLock = true;
            }
            else
            {
                // タイムアウトチェック
                DateTime lockStartDate = ComFunc.GetFldToDateTime(dr, Def_T_AR.LOCK_STARTDATE);
                DateTime timeoutDateTime = lockStartDate.AddMinutes(timeOut);
                if (timeoutDateTime < syoriDateTime)
                {
                    // タイムアウトです。
                    interLock = true;
                }
            }

            if (!interLock)
            {
                // 失敗リストへ追加する
                lockFailedList.Add(ComFunc.GetFld(dr, Def_T_AR.AR_NO));
            }
        }

        if (lockFailedList.Count > 0)
        {
            // {0}は現在別の担当者が編集中です。
            errMsgID = "A0100051001";
            args = new string[] { string.Join(",", lockFailedList.ToArray()) };
            return false;
        }
        // インターロック
        int cnt = UpdArInterLockMultiple(dbHelper, cond, arList);

        if (cnt != arList.Length)
        {
            // 更新に失敗しました。
            errMsgID = "A9999999014";
            return false;
        }

        // 再取得(更新用)
        dt = this.GetAndLockARMultiple(dbHelper, cond, false, arList);
        return true;
    }
    #endregion

    #region AR情報のインターンロックを解除
    /// --------------------------------------------------
    /// <summary>
    /// AR情報のインターンロックを解除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>処理結果</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdArUnLockShinchoku(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        cond.NonyusakiCD = ComFunc.GetFld(dt, 0, Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD);
        // インターンロックを解除
        UpdArInterUnLockMultiple(dbHelper, cond, dt);
        // 状態によらず成功を返す
        return true;
    }
    #endregion

    #region AR進捗情報登録
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>状態 true:成功/false:失敗</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update>D.Okumura 2020/01/23 AR進捗クリア対応</update>
    /// --------------------------------------------------
    public bool UpdArShinchokuInfo(DatabaseHelper dbHelper, CondA1 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        // パラメータを設定
        cond.UpdateDate = DateTime.Now;
        // ARNoリストを作成
        string[] arList = ds.Tables[Def_T_AR.Name].AsEnumerable()
            .Select(w => ComFunc.GetFld(w, Def_T_AR.AR_NO))
            .OrderBy(w => w)
            .ToArray();

        // 納入先状態を確認(メール通知用)
        DataTable dtNonyusaki;
        string bukkenNo;
        {
            var condN = new CondA1(cond.LoginInfo);
            condN.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
            condN.NonyusakiCD = cond.NonyusakiCD;
            condN.LoginInfo = cond.LoginInfo;
            dtNonyusaki = this.GetNonyusaki(dbHelper, condN);
            bukkenNo = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.BUKKEN_NO);
            if (!UtilData.ExistsData(dtNonyusaki))
            {
                // 納入先マスタが削除されています。
                errMsgID = "A0100020011";
                return false;
            }
            if (string.Equals(KANRI_FLAG.KANRYO_VALUE1, ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.KANRI_FLAG)))
            {
                // 完了納入先となっています。管理者に確認して下さい。
                errMsgID = "A0100020002";
                return false;
            }
        }
        // 状況区分一覧を取得(メール通知用)
        Dictionary<string, string> commonJokyoFlag;
        using (WsCommonImpl impl = new WsCommonImpl())
        {
            CondCommon condC = new CondCommon(cond.LoginInfo);
            condC.GroupCD = JYOKYO_FLAG.GROUPCD;
            condC.LoginInfo = cond.LoginInfo;
            DataTable dsCommon = impl.GetCommon(dbHelper, condC).Tables[Def_M_COMMON.Name];
            commonJokyoFlag = dsCommon.AsEnumerable().ToDictionary(k => ComFunc.GetFld(k, Def_M_COMMON.VALUE1),
                v => ComFunc.GetFld(v, Def_M_COMMON.ITEM_NAME));
        }

        // AR情報データ 取得(行ロック)
        DataTable dtAr = this.GetAndLockARMultiple(dbHelper, cond, true, arList);

        // ロック確認
        int[] notFoundIndex = null;
        int index;
        index = this.CheckSameData(ds.Tables[Def_T_AR.Name], dtAr, out notFoundIndex, Def_T_AR.VERSION, Def_T_AR.NONYUSAKI_CD, Def_T_AR.LIST_FLAG, Def_T_AR.AR_NO);
        if (0 <= index || notFoundIndex != null)
        {
            // 他端末で更新された為、更新出来ませんでした。
            errMsgID = "A9999999027";
            return false;
        }

        // 進捗データ取得
        DataTable dtShinchoku = this.GetArShinchokuInfoExec(dbHelper, cond, ds.Tables[Def_T_AR_SHINCHOKU.Name], true);

        // ロック確認
        index = this.CheckSameData(ds.Tables[Def_T_AR_SHINCHOKU.Name], dtShinchoku, out notFoundIndex, Def_T_AR_SHINCHOKU.VERSION, Def_T_AR_SHINCHOKU.NONYUSAKI_CD, Def_T_AR_SHINCHOKU.LIST_FLAG, Def_T_AR_SHINCHOKU.AR_NO, Def_T_AR_SHINCHOKU.GOKI);
        if (0 <= index || notFoundIndex != null)
        {
            // 他端末で更新された為、更新出来ませんでした。
            errMsgID = "A9999999027";
            return false;
        }

        // 進捗データを更新
        UpdArShinchokuExec(dbHelper, cond, ds.Tables[Def_T_AR_SHINCHOKU.Name]);

        // 進捗履歴データ作成
        InsArShinchokuRirekiExec(dbHelper, cond, ds.Tables[Def_T_AR_SHINCHOKU_RIREKI.Name]);

        // AR情報更新有無確認
        string dateKind = ComFunc.GetFld(ds, Def_T_AR_SHINCHOKU_RIREKI.Name, 0, Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND);
        string arDateField = SetupConditionGetArDateFieldFromDateKind(dateKind);
        string shinchokuDateField = SetupConditionGetShinchokuDateFieldFromDateKind(dateKind);
        DataTable dtArShinchokuMin = GetArShinchokuMinDate(dbHelper, cond, ds.Tables[Def_T_AR.Name], shinchokuDateField);
        DataTable dtArUpd = dtAr.Clone(); // 更新用AR情報テーブル

        // 日付に相違のあるAR情報を抽出する
        foreach (DataRow drAr in ds.Tables[Def_T_AR.Name].Rows)
        {
            DataRow drMin = dtArShinchokuMin.AsEnumerable()
                .Where(w => string.Equals(ComFunc.GetFld(drAr, Def_T_AR.NONYUSAKI_CD), ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.NONYUSAKI_CD)))
                .Where(w => string.Equals(ComFunc.GetFld(drAr, Def_T_AR.LIST_FLAG), ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.LIST_FLAG)))
                .Where(w => string.Equals(ComFunc.GetFld(drAr, Def_T_AR.AR_NO), ComFunc.GetFld(w, Def_T_AR_SHINCHOKU.AR_NO)))
                .FirstOrDefault(); // 日付がクリアされた場合、進捗最小データはnullとなる
            // 日付が同じ場合は操作しない
            if (string.Equals(ComFunc.GetFld(drMin, shinchokuDateField), ComFunc.GetFld(drAr, arDateField)))
                continue;
            // AR情報の日付情報を更新する
            dtArUpd.ImportRow(drAr);
            DataRow drArUpd = dtArUpd.Rows[dtArUpd.Rows.Count - 1];
            drArUpd[arDateField] = drMin == null ? "" : drMin[shinchokuDateField];
            drArUpd[Def_T_AR.UPDATE_DATE] = cond.UpdateDate;
            drArUpd[Def_T_AR.UPDATE_USER_ID] = this.GetUpdateUserID(cond);
            drArUpd[Def_T_AR.UPDATE_USER_NAME] = this.GetUpdateUserName(cond);
        }

        if (dtArUpd.Rows.Count > 0)
        {
            // AR情報更新
            this.UpdArForShinchoku(dbHelper, cond, dtArUpd);

            // AR情報履歴更新
            this.InsRirekiFromAr(dbHelper, cond, dtArUpd, bukkenNo, GAMEN_FLAG.A0100010_VALUE1, OPERATION_FLAG.A0100051_EDIT_VALUE1);

            // AR更新メールを送信する
            if (!this.SendArMail(dbHelper, cond, dtNonyusaki, null, dtArUpd, false, ref errMsgID, ref args))
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region メールデータ作成処理 (内部処理)
    /// --------------------------------------------------
    /// <summary>
    /// AR情報 メールテンプレート取得
    /// </summary>
    /// <param name="cond">コンディション</param>
    /// <param name="fileTitle">タイトルのファイル名</param>
    /// <param name="fileBody">本文のファイル名</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>テンプレート</returns>
    /// <create>D.Okumura 2020/06/01 メール送信Webサービス化対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetMailTemplate(CondBase cond, string fileTitle, string fileBody, ref string errMsgID, ref string[] args)
    {
        var dt = ComFunc.GetSchemeMail();
        var dr = dt.NewRow();

        using (WsAttachFileImpl impl = new WsAttachFileImpl())
        {
            string path;
            // タイトルを読み込み
            path = impl.GetFilePath(FileType.MailTemplate, null, null, GirenType.None, null, fileTitle, cond.LoginInfo.Language);
            if (File.Exists(path))
            {
                dr.SetField(Def_T_MAIL.TITLE, File.ReadAllText(path));
            }
            else
            {
                // ファイルが存在しない場合、エラーとしていない。
            }
            // 本文を読み込み
            path = impl.GetFilePath(FileType.MailTemplate, null, null, GirenType.None, null, fileBody, cond.LoginInfo.Language);
            if (File.Exists(path))
            {
                dr.SetField(Def_T_MAIL.NAIYO, File.ReadAllText(path));
            }
            else
            {
                // ファイルが存在しない場合、エラーとしていない。
            }
        }
        dt.Rows.Add(dr);
        return dt;
    }
    /// --------------------------------------------------
    /// <summary>
    /// AR情報 メール送信
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="CondA1">A01用コンディション</param>
    /// <param name="dtNonyusaki">納入先のデータテーブル</param>
    /// <param name="arNo">採番した納ARNo(nullの場合はデータ行から取得)</param>
    /// <param name="dtAr">AR一覧</param>
    /// <param name="isInsert">true:insert, false:update</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2020/06/01 ARメール送信共通化</create>
    /// <update>M.Shimizu 2020/06/09 メールの本文に登録者、更新者が設定されていない</update>
    /// <update>J.Chen 2024/08/06 ARメールに添付ファイルの追加を対応</update>
    /// --------------------------------------------------
    private bool SendArMail(DatabaseHelper dbHelper, CondA1 cond, DataTable dtNonyusaki, string arNo, DataTable dtAr, bool isInsert, ref string errMsgID, ref string[] args)
    {

        // リスト区分一覧を作成
        string[] listFlagList = dtAr.AsEnumerable()
            .Select(w => ComFunc.GetFld(w, Def_T_AR.LIST_FLAG))
            .OrderBy(w => w)
            .Distinct()
            .ToArray();
        Dictionary<string, string> commonJokyoFlag;
        using (WsCommonImpl impl = new WsCommonImpl())
        {
            // 状況区分一覧を取得(メール通知用)
            CondCommon condC = new CondCommon(cond.LoginInfo);
            condC.GroupCD = JYOKYO_FLAG.GROUPCD;
            condC.LoginInfo = cond.LoginInfo;
            DataTable dsCommon = impl.GetCommon(dbHelper, condC).Tables[Def_M_COMMON.Name];
            commonJokyoFlag = dsCommon.AsEnumerable().ToDictionary(k => ComFunc.GetFld(k, Def_M_COMMON.VALUE1),
                v => ComFunc.GetFld(v, Def_M_COMMON.ITEM_NAME));

            // メール情報を取得
            bool isNotify;
            var dsMail = this.GetArMailBaseInfo(dbHelper, cond, isInsert, cond.BukkenNo, out isNotify, ref errMsgID, ref args);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                return false;
            }
            // 通知なしの場合は成功
            if (!isNotify)
            {
                return true;
            }
            string mailAddress = ComFunc.GetFld(dsMail, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);
            // AR情報の物件メール情報取得処理
            Dictionary<string, DataTable> mailList = this.GetArMailBukkenInfo(dbHelper, cond, cond.BukkenNo, listFlagList, ref errMsgID, ref args);

            if (!string.IsNullOrEmpty(errMsgID))
            {
                return false;
            }

            // テンプレートを取得
            string templateTitle = ComFunc.GetFld(dsMail, Def_T_MAIL.Name, 0, Def_T_MAIL.TITLE);
            string templateBody = ComFunc.GetFld(dsMail, Def_T_MAIL.Name, 0, Def_T_MAIL.NAIYO);

            // メール送信用データテーブル
            DataTable dtMail = ComFunc.GetSchemeMail();
            // AR情報について処理する
            foreach (DataRow drAr in dtAr.Rows)
            {
                // メール通知データ作成
                string listKbn = ComFunc.GetFld(drAr, Def_T_AR.LIST_FLAG);
                if (mailList.ContainsKey(listKbn))
                {
                    // ↓↓↓ M.Shimizu 2020/06/09 メールの本文に登録者、更新者が設定されていない
                    if (string.IsNullOrEmpty(ComFunc.GetFld(drAr, Def_T_AR.UPDATE_USER_NAME)))
                    {
                        drAr.SetField<string>(Def_T_AR.UPDATE_USER_NAME, this.GetUpdateUserName(cond));
                    }
                    // ↑↑↑ M.Shimizu 2020/06/09 メールの本文に登録者、更新者が設定されていない

                    var body = this.CreateMailArUpdateReplace(templateBody, dtNonyusaki, commonJokyoFlag, drAr, arNo);
                    var title = this.CreateMailArUpdateReplace(templateTitle, dtNonyusaki, commonJokyoFlag, drAr, arNo);
                    this.CreateMailArUpdateNotification(dtMail, mailList[listKbn], mailAddress, cond.LoginInfo.UserName, title, body);
                }
            }
            // メールデータがある場合登録する
            foreach (DataRow dr in dtMail.Rows)
            {
                var condCommon = new CondCommon(cond.LoginInfo);

                // 事前取得したARNoと実際登録するARNoが一致するかをチェックする
                if (arNo == cond.ArNoTemp)
                {
                    if (!impl.SaveMailWithCheck(dbHelper, condCommon, dr, cond.MailIDTemp, cond.FilePathTemp, ref errMsgID, ref args))
                        return false;
                }
                else
                {
                    if (!impl.SaveMail(dbHelper, condCommon, dr, ref errMsgID, ref args))
                        return false;
                }
            }
        }
        return true;
    }

    /// --------------------------------------------------
    /// <summary>
    /// AR進捗のメール送信
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="arList">通知対象のARNoリスト</param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns>データセット</returns>
    /// <create>T.Nakata 2019/07/22</create>
    /// <update>D.Okumura 2020/06/01 サーバー側でメールデータを作成</update>
    /// --------------------------------------------------
    private bool SendArShinchokuMail(DatabaseHelper dbHelper, CondA1 cond, string[] arList, ref string errMsgID, ref string[] args)
    {
        DataTable dtMailKobetsu = this.GetArShinchokuMailInfo(dbHelper, cond, cond.BukkenNo, ref errMsgID, ref args);
        //----- メール設定(個別)取得 -----
        if (!UtilData.ExistsData(dtMailKobetsu))
        {
            return false;
        }

        //----- メール送信内容作成 -----
        using (WsCommonImpl impl = new WsCommonImpl())
        {
            // メール情報取得
            var condCommon = new CondCommon(cond.LoginInfo);
            DataSet dsMailInfo = impl.CheckMail(dbHelper, condCommon);

            // メールアドレス取得
            string mailAddress = UtilData.GetFld(dsMailInfo, Def_M_USER.Name, 0, Def_M_USER.MAIL_ADDRESS);

            // テンプレート取得
            var dtTemplate = this.GetMailTemplate(cond, MAIL_FILE.SHINCHOKU_TITLE_VALUE1, MAIL_FILE.SHINCHOKU_VALUE1, ref errMsgID, ref args);
            if (!string.IsNullOrEmpty(errMsgID))
            {
                return false;
            }
            string tpMailTitle = ComFunc.GetFld(dtTemplate, 0, Def_T_MAIL.TITLE);
            string tpMailBody = ComFunc.GetFld(dtTemplate, 0, Def_T_MAIL.NAIYO);

            // 置換
            tpMailTitle = tpMailTitle.Replace(MAIL_RESERVE.BUKKEN_NAME_VALUE1, cond.NonyusakiName);
            tpMailBody = tpMailBody.Replace(MAIL_RESERVE.NONYUSAKI_NAME_VALUE1, cond.NonyusakiName);
            tpMailBody = tpMailBody.Replace(MAIL_RESERVE.BUKKEN_CREATE_USER_VALUE1, cond.LoginInfo.UserName);
            tpMailBody = tpMailBody.Replace(MAIL_RESERVE.AR_NO_LIST_VALUE1, string.Join("\r\n", arList));

            // メールデータ作成
            var dt = ComFunc.GetSchemeMail();
            this.CreateMailArUpdateNotification(dt, dtMailKobetsu, mailAddress, cond.LoginInfo.UserName, tpMailTitle, tpMailBody);

            // メール送信データ登録
            if (!impl.SaveMail(dbHelper, condCommon, dt.Rows[0], ref errMsgID, ref args))
            {   // 登録失敗
                return false;
            }
        }

        return true;
    }
    /// --------------------------------------------------
    /// <summary>
    /// メールデータ作成
    /// </summary>
    /// <param name="dt">メールテーブル(T_MAIL)</param>
    /// <param name="dtMailUserInfo">送信先情報</param>
    /// <param name="mailaddress">送信者のメールアドレス</param>
    /// <param name="templateFolder">テンプレート格納先</param>
    /// <create>D.Okumura 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private void CreateMailArUpdateNotification(DataTable dt, DataTable dtMailUserInfo, string mailaddress, string userName, string mailTitle, string mailBody)
    {
        // 送信先がない場合はデータを作成しない
        if (dtMailUserInfo.Rows.Count < 1)
            return;
        var dr = dt.NewRow();
        dr.SetField<object>(Def_T_MAIL.MAIL_SEND, mailaddress);
        dr.SetField<object>(Def_T_MAIL.MAIL_SEND_DISPLAY, userName);
        dr.SetField<object>(Def_T_MAIL.MAIL_TO, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.MAIL_ADDRESS));
        dr.SetField<object>(Def_T_MAIL.MAIL_TO_DISPLAY, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.TO_VALUE1, Def_M_USER.USER_NAME));
        dr.SetField<object>(Def_T_MAIL.MAIL_CC, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.MAIL_ADDRESS));
        dr.SetField<object>(Def_T_MAIL.MAIL_CC_DISPLAY, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.CC_VALUE1, Def_M_USER.USER_NAME));
        dr.SetField<object>(Def_T_MAIL.MAIL_BCC, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.MAIL_ADDRESS));
        dr.SetField<object>(Def_T_MAIL.MAIL_BCC_DISPLAY, ComFunc.GetMailUser(dtMailUserInfo, MAIL_ADDRESS_FLAG.BCC_VALUE1, Def_M_USER.USER_NAME));
        dr.SetField<object>(Def_T_MAIL.TITLE, mailTitle);
        dr.SetField<object>(Def_T_MAIL.NAIYO, mailBody);
        dr.SetField<object>(Def_T_MAIL.MAIL_STATUS, MAIL_STATUS.MI_VALUE1);
        dr.SetField<object>(Def_T_MAIL.RETRY_COUNT, 0);
        dt.Rows.Add(dr);
    }
    /// --------------------------------------------------
    /// <summary>
    /// テンプレートのデータを置換
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dtNonyusaki">納入先情報</param>
    /// <param name="drAr"></param>
    /// <param name="arNo">採番した納ARNo(nullの場合はデータ行から取得)</param>
    /// <returns></returns>
    /// <create>D.Okumura 2019/07/31 添付ファイル対応</create>
    /// <update>D.Okumura 2020/06/01 メール送信Webサービス化対応</update>
    /// --------------------------------------------------
    private string CreateMailArUpdateReplace(string source, DataTable dtNonyusaki, Dictionary<string, string> dicJokyoFlag, DataRow drAr, string arNo)
    {
        // リストフラグ名
        string listFlagFieldName = "";
        var listFlag = ComFunc.GetFld(drAr, Def_T_AR.LIST_FLAG);
        if (LIST_FLAG.FLAG_0_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME0;
        else if (LIST_FLAG.FLAG_1_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME1;
        else if (LIST_FLAG.FLAG_2_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME2;
        else if (LIST_FLAG.FLAG_3_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME3;
        else if (LIST_FLAG.FLAG_4_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME4;
        else if (LIST_FLAG.FLAG_5_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME5;
        else if (LIST_FLAG.FLAG_6_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME6;
        else if (LIST_FLAG.FLAG_7_VALUE1.Equals(listFlag))
            listFlagFieldName = Def_M_NONYUSAKI.LIST_FLAG_NAME7;
        // 状況フラグ
        string jokyoFlag = ComFunc.GetFld(drAr, Def_T_AR.JYOKYO_FLAG);
        string jokyoFlagName = "";
        if (dicJokyoFlag.ContainsKey(jokyoFlag))
            jokyoFlagName = dicJokyoFlag[jokyoFlag];
        return source
            .Replace(MAIL_RESERVE.NONYUSAKI_NAME_VALUE1, ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_NAME))
            .Replace(MAIL_RESERVE.LIST_FLAG_VALUE1, ComFunc.GetFld(dtNonyusaki, 0, listFlagFieldName))
            .Replace(MAIL_RESERVE.AR_NO_VALUE1, string.IsNullOrEmpty(arNo) ? ComFunc.GetFld(drAr, Def_T_AR.AR_NO) : arNo)
            .Replace(MAIL_RESERVE.JYOKYO_FLAG_VALUE1, jokyoFlagName)
            .Replace(MAIL_RESERVE.HASSEI_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.HASSEI_DATE))
            .Replace(MAIL_RESERVE.RENRAKUSHA_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.RENRAKUSHA))
            .Replace(MAIL_RESERVE.KISHU_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.KISHU))
            .Replace(MAIL_RESERVE.GOKI_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GOKI))
            .Replace(MAIL_RESERVE.GENBA_TOTYAKUKIBOU_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE))
            .Replace(MAIL_RESERVE.HASSEI_YOUIN_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.HASSEI_YOUIN))
            .Replace(MAIL_RESERVE.HUGUAI_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.HUGUAI))
            .Replace(MAIL_RESERVE.TAISAKU_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.TAISAKU))
            .Replace(MAIL_RESERVE.BIKO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.BIKO))
            .Replace(MAIL_RESERVE.GENCHI_TEHAISAKI_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GENCHI_TEHAISAKI))
            .Replace(MAIL_RESERVE.GENCHI_SETTEINOKI_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GENCHI_SETTEINOKI_DATE))
            .Replace(MAIL_RESERVE.GENCHI_SHUKKAYOTEI_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE))
            .Replace(MAIL_RESERVE.GENCHI_KOJYOSHUKKA_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GENCHI_KOJYOSHUKKA_DATE))
            .Replace(MAIL_RESERVE.SHUKKAHOHO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.SHUKKAHOHO))
            .Replace(MAIL_RESERVE.JP_SETTEINOKI_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.JP_SETTEINOKI_DATE))
            .Replace(MAIL_RESERVE.JP_SHUKKAYOTEI_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.JP_SHUKKAYOTEI_DATE))
            .Replace(MAIL_RESERVE.JP_KOJYOSHUKKA_DATE_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.JP_KOJYOSHUKKA_DATE))
            .Replace(MAIL_RESERVE.JP_UNSOKAISHA_NAME_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.JP_UNSOKAISHA_NAME))
            .Replace(MAIL_RESERVE.JP_OKURIJYO_NO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.JP_OKURIJYO_NO))
            .Replace(MAIL_RESERVE.GMS_HAKKO_NO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GMS_HAKKO_NO))
            .Replace(MAIL_RESERVE.SHIYORENRAKU_NO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.SHIYORENRAKU_NO))
            .Replace(MAIL_RESERVE.TAIO_BUSHO_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.TAIO_BUSHO))
            .Replace(MAIL_RESERVE.GIREN_NO_1_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GIREN_NO_1))
            .Replace(MAIL_RESERVE.GIREN_NO_2_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GIREN_NO_2))
            .Replace(MAIL_RESERVE.GIREN_NO_3_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GIREN_NO_3))
            .Replace(MAIL_RESERVE.GIREN_NO_4_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GIREN_NO_4))
            .Replace(MAIL_RESERVE.GIREN_NO_5_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.GIREN_NO_5))
            .Replace(MAIL_RESERVE.REFERENCE_NO_1_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.REFERENCE_NO_1))
            .Replace(MAIL_RESERVE.REFERENCE_NO_2_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.REFERENCE_NO_2))
            .Replace(MAIL_RESERVE.AR_CREATE_USER_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.UPDATE_USER_NAME))
            .Replace(MAIL_RESERVE.AR_UPDATE_USER_VALUE1, ComFunc.GetFld(drAr, Def_T_AR.UPDATE_USER_NAME));
    }
    #endregion //メールデータ作成処理 (内部処理)

    #endregion //A0100051:ＡＲ進捗管理日付登録

    #endregion

    #region SQL実行

    #region A0100010:AR情報登録

    #region SELECT

    #region AR情報データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>M.Tsutsumi 2010/08/12</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
    /// <update>T.Nukaga 2019/11/20 STEP12 AR7000番運用対応</update>
    /// --------------------------------------------------
    public DataTable GetARList(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "T_AR.";
            string fieldPrefixM = "M_NONYUSAKI.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_AR.NONYUSAKI_CD");
            sb.ApdL("     , M_BUKKEN.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , T_AR.LIST_FLAG");
            // @@@ 2011/02/24 M.Tsutsumi Change 
            //sb.ApdL("     , M_COM1.ITEM_NAME LIST_FLAG_NAME");
            sb.ApdL("     , CASE");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_0_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME0");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_1_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME1");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_2_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME2");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_3_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME3");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_4_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME4");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_5_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME5");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_6_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME6");
            sb.ApdN("           WHEN T_AR.LIST_FLAG = '").ApdN(LIST_FLAG.FLAG_7_VALUE1).ApdL("' THEN M_NONYUSAKI.LIST_FLAG_NAME7");
            sb.ApdL("           ELSE M_COM1.ITEM_NAME");
            sb.ApdL("       END LIST_FLAG_NAME");
            // @@@ ↑
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME0");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME1");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME2");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME3");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME4");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME5");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME6");
            sb.ApdL("     , M_NONYUSAKI.LIST_FLAG_NAME7");
            // @@@ ↑
            sb.ApdL("     , T_AR.AR_NO");
            sb.ApdL("     , T_AR.JYOKYO_FLAG");
            sb.ApdL("     , M_COM2.ITEM_NAME JYOKYO_FLAG_NAME");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.HASSEI_DATE, 111) AS HASSEI_DATE");
            sb.ApdL("     , T_AR.RENRAKUSHA");
            sb.ApdL("     , T_AR.KISHU");
            sb.ApdL("     , T_AR.GOKI");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.GENBA_TOTYAKUKIBOU_DATE, 111) AS GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("     , SUBSTRING(T_AR.HUGUAI,1,CASE WHEN HUGUAI_LEN_2LINE > 0 THEN HUGUAI_LEN_2LINE ELSE HUGUAI_LEN_ALL END ) AS HUGUAI");
            sb.ApdL("     , T_AR.HUGUAI AS HUGUAI_ALL");
            sb.ApdL("     , SUBSTRING(T_AR.TAISAKU,1,CASE WHEN TAISAKU_LEN_2LINE > 0 THEN TAISAKU_LEN_2LINE ELSE TAISAKU_LEN_ALL END ) AS TAISAKU");
            sb.ApdL("     , T_AR.TAISAKU AS TAISAKU_ALL");
            sb.ApdL("     , SUBSTRING(T_AR.BIKO,1,CASE WHEN BIKO_LEN_2LINE > 0 THEN BIKO_LEN_2LINE ELSE BIKO_LEN_ALL END ) AS BIKO");
            sb.ApdL("     , T_AR.BIKO AS BIKO_ALL");
            sb.ApdL("     , T_AR.GENCHI_TEHAISAKI");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.GENCHI_SETTEINOKI_DATE, 111) AS GENCHI_SETTEINOKI_DATE");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.GENCHI_SHUKKAYOTEI_DATE, 111) AS GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.GENCHI_KOJYOSHUKKA_DATE, 111) AS GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("     , T_AR.SHUKKAHOHO");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.JP_SETTEINOKI_DATE, 111) AS JP_SETTEINOKI_DATE");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.JP_SHUKKAYOTEI_DATE, 111) AS JP_SHUKKAYOTEI_DATE");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.JP_KOJYOSHUKKA_DATE, 111) AS JP_KOJYOSHUKKA_DATE");
            sb.ApdL("     , T_AR.JP_UNSOKAISHA_NAME");
            sb.ApdL("     , T_AR.JP_OKURIJYO_NO");
            sb.ApdL("     , T_AR.GMS_HAKKO_NO");
            sb.ApdL("     , T_AR.SHIYORENRAKU_NO");
            sb.ApdL("     , T_AR.TAIO_BUSHO");
            sb.ApdL("     , T_AR.GIREN_NO_1");
            sb.ApdL("     , T_AR.GIREN_NO_2");
            sb.ApdL("     , T_AR.GIREN_NO_3");
            sb.ApdL("     , T_AR.GIREN_NO_4");
            sb.ApdL("     , T_AR.GIREN_NO_5");
            sb.ApdL("     , T_AR.HASSEI_YOUIN");
            sb.ApdL("     , T_AR.REFERENCE_NO_1");
            sb.ApdL("     , T_AR.REFERENCE_NO_2");
            sb.ApdL("     , CONVERT(VARCHAR, T_AR.UPDATE_DATE, 111) AS UPDATE_DATE");
            sb.ApdL("     , T_AR.UPDATE_USER_NAME");
            sb.ApdL("     , T_AR.MOTO_AR_NO");
            sb.ApdL("     , (SELECT AR_NO FROM T_AR ta1 WHERE ta1.MOTO_AR_NO = T_AR.AR_NO AND ta1.NONYUSAKI_CD = T_AR.NONYUSAKI_CD) AS KEKKA_AR_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       (");
            sb.ApdL("       SELECT");
            sb.ApdL("             NONYUSAKI_CD");
            sb.ApdL("           , LIST_FLAG");
            sb.ApdL("           , AR_NO");
            sb.ApdL("           , JYOKYO_FLAG");
            sb.ApdL("           , HASSEI_DATE");
            sb.ApdL("           , RENRAKUSHA");
            sb.ApdL("           , KISHU");
            sb.ApdL("           , GOKI");
            sb.ApdL("           , GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("           , HUGUAI");
            sb.ApdL("           , CHARINDEX(CHAR(13) + CHAR(10), HUGUAI,CHARINDEX(CHAR(13) + CHAR(10), HUGUAI) + 1 ) AS HUGUAI_LEN_2LINE");
            sb.ApdL("           , LEN(HUGUAI) AS HUGUAI_LEN_ALL");
            sb.ApdL("           , TAISAKU");
            sb.ApdL("           , CHARINDEX(CHAR(13) + CHAR(10), TAISAKU,CHARINDEX(CHAR(13) + CHAR(10), TAISAKU) + 1 ) AS TAISAKU_LEN_2LINE");
            sb.ApdL("           , LEN(TAISAKU) AS TAISAKU_LEN_ALL");
            sb.ApdL("           , BIKO");
            sb.ApdL("           , CHARINDEX(CHAR(13) + CHAR(10), BIKO,CHARINDEX(CHAR(13) + CHAR(10), BIKO) + 1 ) AS BIKO_LEN_2LINE");
            sb.ApdL("           , LEN(BIKO) AS BIKO_LEN_ALL");
            sb.ApdL("           , GENCHI_TEHAISAKI");
            sb.ApdL("           , GENCHI_SETTEINOKI_DATE");
            sb.ApdL("           , GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("           , GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("           , SHUKKAHOHO");
            sb.ApdL("           , JP_SETTEINOKI_DATE");
            sb.ApdL("           , JP_SHUKKAYOTEI_DATE");
            sb.ApdL("           , JP_KOJYOSHUKKA_DATE");
            sb.ApdL("           , JP_UNSOKAISHA_NAME");
            sb.ApdL("           , JP_OKURIJYO_NO");
            sb.ApdL("           , GMS_HAKKO_NO");
            sb.ApdL("           , SHIYORENRAKU_NO");
            sb.ApdL("           , TAIO_BUSHO");
            sb.ApdL("           , GIREN_NO_1");
            sb.ApdL("           , GIREN_NO_2");
            sb.ApdL("           , GIREN_NO_3");
            sb.ApdL("           , GIREN_NO_4");
            sb.ApdL("           , GIREN_NO_5");
            sb.ApdL("           , HASSEI_YOUIN");
            sb.ApdL("           , REFERENCE_NO_1");
            sb.ApdL("           , REFERENCE_NO_2");
            sb.ApdL("           , UPDATE_DATE");
            sb.ApdL("           , UPDATE_USER_NAME");
            sb.ApdL("           , MOTO_AR_NO");
            sb.ApdL("           , (SELECT AR_NO FROM T_AR ta1 WHERE ta1.MOTO_AR_NO = T_AR.AR_NO AND ta1.NONYUSAKI_CD = T_AR.NONYUSAKI_CD) AS KEKKA_AR_NO");
            sb.ApdL("       FROM T_AR");
            sb.ApdL("       )");
            sb.ApdL("       T_AR");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI ON M_NONYUSAKI.NONYUSAKI_CD = T_AR.NONYUSAKI_CD");
            sb.ApdN("                  AND M_NONYUSAKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("  LEFT JOIN M_COMMON M_COM1 ON M_COM1.GROUP_CD = 'LIST_FLAG'");
            sb.ApdL("                  AND M_COM1.VALUE1 = T_AR.LIST_FLAG");
            sb.ApdN("                  AND M_COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON M_COM2 ON M_COM2.GROUP_CD = 'JYOKYO_FLAG'");
            sb.ApdL("                  AND M_COM2.VALUE1 = T_AR.JYOKYO_FLAG");
            sb.ApdN("                  AND M_COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN ON M_BUKKEN.SHUKKA_FLAG = M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("                  AND M_BUKKEN.BUKKEN_NO = M_NONYUSAKI.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));

            // 納入先コード
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }

            // 納入先
            if (!string.IsNullOrEmpty(cond.NonyusakiName))
            {
                fieldName = "NONYUSAKI_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefixM + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiName));
            }

            // リスト区分
            if (!string.IsNullOrEmpty(cond.ListFlag))
            {
                fieldName = "LIST_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlag));
            }

            // 状況区分
            if (!string.IsNullOrEmpty(cond.JyokyoFlagAR))
            {
                if (cond.JyokyoFlagAR != JYOKYO_FLAG_AR.ALL_VALUE1)
                {
                    if (cond.JyokyoFlagAR != JYOKYO_FLAG_AR.MISHORI_VALUE1)
                    {
                        fieldName = "JYOKYO_FLAG";
                        sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                        // バインド変数設定
                        paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.JyokyoFlagAR));
                    }
                    else
                    {
                        fieldName = "JYOKYO_FLAG";
                        sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" <> ").ApdN(this.BindPrefix).ApdL(fieldName);
                        // バインド変数設定
                        paramCollection.Add(iNewParam.NewDbParameter(fieldName, JYOKYO_FLAG.KANRYO_VALUE1));
                    }
                }
            }
            else if (!string.IsNullOrEmpty(cond.JyokyoFlag))
            {
                fieldName = "JYOKYO_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.JyokyoFlag));
            }

            // 対策内容
            if (!string.IsNullOrEmpty(cond.Taisaku))
            {
                fieldName = "TAISAKU";
                sb.ApdN("   AND (").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.Taisaku + "%"));
                fieldName = "HUGUAI";
                sb.ApdN("   OR ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdN(fieldName).ApdL(")");
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.Taisaku + "%"));
            }

            // 不具合内容
            if (!string.IsNullOrEmpty(cond.Taisaku))
            {
            }

            // 現地・手配先
            if (!string.IsNullOrEmpty(cond.GenchiTehaisaki))
            {
                fieldName = "GENCHI_TEHAISAKI";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, "%" + cond.GenchiTehaisaki + "%"));
            }

            // 技連
            if (!string.IsNullOrEmpty(cond.GirenNo))
            {
                fieldName = "GIREN_NO_";
                string fieldName2 = "REFERENCE_NO_";
                sb.ApdL("   AND ");
                sb.ApdL("   (");
                // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.33 含む検索に変更
                //sb.ApdN("       ").ApdN(fieldPrefix + fieldName + "1").ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName + "1");
                //sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "2").ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName + "2");
                //sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "3").ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName + "3");
                sb.ApdN("       ").ApdN(fieldPrefix + fieldName + "1").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName + "1");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "2").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName + "2");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "3").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName + "3");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "4").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName + "4");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName + "5").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName + "5");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName2 + "1").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName2 + "1");
                sb.ApdN("   OR  ").ApdN(fieldPrefix + fieldName2 + "2").ApdN(" LIKE ").ApdN(this.BindPrefix).ApdL(fieldName2 + "2");
                // @@@ ↑
                sb.ApdL("   )");
                // バインド変数設定
                // @@@ 2011/02/16 M.Tsutsumi Change Step2 No.33 含む検索に変更
                //paramCollection.Add(iNewParam.NewDbParameter(fieldName + "1", cond.GirenNo));
                //paramCollection.Add(iNewParam.NewDbParameter(fieldName + "2", cond.GirenNo));
                //paramCollection.Add(iNewParam.NewDbParameter(fieldName + "3", cond.GirenNo));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName + "1", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName + "2", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName + "3", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName + "4", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName + "5", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName2 + "1", "%" + cond.GirenNo + "%"));
                paramCollection.Add(iNewParam.NewDbParameter(fieldName2 + "2", "%" + cond.GirenNo + "%"));
                // @@@ ↑
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       T_AR.AR_NO");
            //sb.ApdL("     , FIELD2");

            // バインド変数設定
            fieldName = "SHUKKA_FLAG";
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);

            return ds.Tables[Def_T_AR.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR対応費用取得

    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">検索用データテーブル</param>
    /// <returns>データテーブル</returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
    /// --------------------------------------------------
    public DataTable GetARCostList(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_AR_COST.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , T_AR_COST.LIST_FLAG");
            sb.ApdL("     , T_AR_COST.AR_NO");
            sb.ApdL("     , T_AR_COST.LINE_NO");
            sb.ApdL("     , COM1.ITEM_NAME AS ITEM_CD");
            sb.ApdL("     , T_AR_COST.WORK_TIME");
            sb.ApdL("     , T_AR_COST.WORKERS");
            sb.ApdL("     , T_AR_COST.NUMBER");
            sb.ApdL("     , CASE M_USER.STAFF_KBN");
            sb.ApdL("        WHEN '0' THEN T_AR_COST.RATE");
            sb.ApdL("        ELSE 0");
            sb.ApdL("       END RATE");
            sb.ApdL("     , CASE M_USER.STAFF_KBN");
            sb.ApdL("        WHEN '0' THEN T_AR_COST.TOTAL");
            sb.ApdL("        ELSE 0");
            sb.ApdL("       END TOTAL");
            sb.ApdL("     , T_AR_COST.CREATE_DATE");
            sb.ApdL("     , T_AR_COST.CREATE_USER_ID");
            sb.ApdL("     , T_AR_COST.CREATE_USER_NAME");
            sb.ApdL("     , T_AR.HASSEI_YOUIN");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR_COST");
            sb.ApdL(" INNER JOIN T_AR");
            sb.ApdL("         ON T_AR.NONYUSAKI_CD = T_AR_COST.NONYUSAKI_CD");
            sb.ApdL("        AND T_AR.LIST_FLAG = T_AR_COST.LIST_FLAG");
            sb.ApdL("        AND T_AR.AR_NO = T_AR_COST.AR_NO");
            sb.ApdL(" INNER JOIN M_NONYUSAKI NON");
            sb.ApdL("         ON NON.NONYUSAKI_CD = T_AR_COST.NONYUSAKI_CD");
            sb.ApdN("        AND NON.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL(" INNER JOIN M_BUKKEN BK");
            sb.ApdL("         ON BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdN("        AND BK.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(AR_COST_ITEM.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = T_AR_COST.ITEM_CD");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdN("         ON M_USER.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_AR_COST.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND T_AR_COST.LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND T_AR_COST.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_COST.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_COST.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_COST.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UpdateUserID));

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

    #region 出荷状況データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷状況データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">検索用データテーブル</param>
    /// <returns>データテーブル</returns>
    /// <create>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</create>
    /// <create>J.Chen 2022/11/04 多言語対応修正</create>
    /// <create>J.Chen 2022/12/23 出荷状況自動反映取得対象追加</create>
    /// --------------------------------------------------
    public DataTable GetShukka(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            var iNewParam = dbHelper;

            // SQL文
            sb.ApdL("--出荷済件数 手配中0 出荷済1 一部出荷済2 ");
            sb.ApdL("WITH T_SHUKKA_JYOTAI AS ");
            sb.ApdL("( ");
            sb.ApdL("   SELECT ");
            sb.ApdL("         COUNT(1) AS MAX_ROWS ");
            sb.ApdL("       , COUNT(CASE WHEN SK.JYOTAI_FLAG = '" + JYOTAI_FLAG.SHUKKAZUMI_VALUE1 + "' THEN 1 ELSE NULL END) AS SHUKKA ");
            sb.ApdL("   FROM ( ");



            sb.ApdL("       SELECT  TTM.TEHAI_RENKEI_NO ");
            sb.ApdL("             , ME.ECS_NO");
            sb.ApdL("             , CAST(NULL AS NCHAR(1)) AS JYOTAI_FLAG");
            sb.ApdL("             , ME.ECS_QUOTA");
            sb.ApdL("             , CASE");
            sb.ApdL("                    WHEN (ME.AR_NO IS NULL OR LTRIM(RTRIM(ME.AR_NO)) = '') THEN CAST(NULL AS NVARCHAR(10))");
            sb.ApdL("                    ELSE ME.AR_NO");
            sb.ApdL("               END AS AR_NO");
            sb.ApdL("             , CASE ");
            sb.ApdN("                  WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ORDERED");
            sb.ApdL("                     THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("                    WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY");
            sb.ApdL("                   THEN TTM.ASSY_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("                   WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("                      THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdN("                    WHEN TTM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SURPLUS");
            sb.ApdL("                    THEN TTM.ARRIVAL_QTY - IsNull(WSM1.CNT, 0)");
            sb.ApdL("                 ELSE 0");
            sb.ApdL("                END AS TAG_TOUROKU_MAX");
            sb.ApdL("           FROM ");
            sb.ApdL("               T_TEHAI_MEISAI TTM");
            sb.ApdL("         INNER JOIN M_ECS ME ON TTM.ECS_NO = ME.ECS_NO AND TTM.ECS_QUOTA = ME.ECS_QUOTA");
            sb.ApdL("               AND (ME.AR_NO IS NOT NULL AND LTRIM(RTRIM(ME.AR_NO)) != '')");
            sb.ApdN("               AND ME.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdL("         LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE TEHAI_RENKEI_NO IS NOT NULL GROUP BY TEHAI_RENKEI_NO) AS WSM1 ON WSM1.TEHAI_RENKEI_NO = TTM.TEHAI_RENKEI_NO");
            sb.ApdN("         LEFT JOIN M_NONYUSAKI MN ON MN.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("         LEFT JOIN M_BUKKEN MB ON MB.BUKKEN_NO = MN.BUKKEN_NO AND MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdN("         LEFT JOIN M_PROJECT MP ON MP.PROJECT_NO = MB.PROJECT_NO");
            sb.ApdL("         WHERE");
            sb.ApdL("                TTM.TEHAI_QTY - IsNull(WSM1.CNT, 0) > 0 ");
            sb.ApdL("            AND ME.PROJECT_NO = MB.PROJECT_NO ");

            sb.ApdL("       UNION ALL ");
            sb.ApdL("       SELECT  TSM.TEHAI_RENKEI_NO ");
            sb.ApdL("             , ME.ECS_NO");
            sb.ApdL("             , TSM.JYOTAI_FLAG");
            sb.ApdL("             , TTM.ECS_QUOTA");
            sb.ApdN("             , CASE TSM.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE CAST(NULL AS NVARCHAR(6)) END AS AR_NO");
            sb.ApdN("             , 0 AS TAG_TOUROKU_MAX");
            sb.ApdL("           FROM ");
            sb.ApdL("               T_SHUKKA_MEISAI TSM");
            sb.ApdL("           INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("           INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL("           INNER JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL("           INNER JOIN M_ECS ME ON ME.ECS_NO = TTM.ECS_NO AND ME.ECS_QUOTA = TTM.ECS_QUOTA");
            sb.ApdL("           INNER JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdL("        WHERE (TSM.TEHAI_RENKEI_NO IS NOT NULL AND LTRIM(RTRIM(TSM.TEHAI_RENKEI_NO)) != '')");
            sb.ApdN("           AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR");
            sb.ApdN("           AND TSM.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("           AND MN.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            sb.ApdL("       ) AS SK ");
            sb.ApdL("   WHERE SK.TEHAI_RENKEI_NO IS NOT NULL ");
            sb.ApdL(") ");

            sb.ApdL("SELECT");
            sb.ApdL("       TSJ.MAX_ROWS");
            sb.ApdL("     , TSJ.SHUKKA");
            sb.ApdL("     , COM1.ITEM_NAME");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_JYOTAI TSJ");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(JYOTAI_FLAG_AR.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = (");
            sb.ApdL("         CASE ");
            sb.ApdL("             WHEN SHUKKA < MAX_ROWS AND SHUKKA <> 0 THEN '" + JYOTAI_FLAG_AR.ICHIBUSHUKKAZUMI_VALUE1 + "' ");
            sb.ApdL("             WHEN SHUKKA = MAX_ROWS AND SHUKKA <> 0 THEN '" + JYOTAI_FLAG_AR.SHUKKAZUMI_VALUE1 + "' ");
            sb.ApdL("             ELSE '" + JYOTAI_FLAG_AR.TEHAICHU_VALUE1 + "' ");
            sb.ApdL("         END)");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_ORDERED", TEHAI_FLAG.ORDERED_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_ASSY", TEHAI_FLAG.ASSY_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SKS_SKIP", TEHAI_FLAG.SKS_SKIP_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_FLAG_SURPLUS", TEHAI_FLAG.SURPLUS_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);
            return ds.Tables[Def_T_SHUKKA_MEISAI.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 全体Excel件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体Excel件数取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetAllARListCount(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM T_AR AR");
            sb.ApdL(" INNER JOIN M_NONYUSAKI NON");
            sb.ApdL("         ON NON.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdN("        AND NON.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL(" INNER JOIN M_BUKKEN BK");
            sb.ApdL("         ON BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdN("        AND BK.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(LIST_FLAG.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = AR.LIST_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("         ON COM2.GROUP_CD = '").ApdN(JYOKYO_FLAG.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM2.VALUE1 = AR.JYOKYO_FLAG");
            sb.ApdN("        AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE AR.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);
            return ds.Tables[Def_T_AR.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 全体Excel用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体Excel用データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/26</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル追加対応</update>
    /// <update>T.Nukaga 2019/11/20 STEP12 AR7000番運用対応</update>
    /// --------------------------------------------------
    public DataTable GetAllARList(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT BK.BUKKEN_NAME");
            sb.ApdL("      ,COM1.ITEM_NAME AS LIST_FLAG");
            sb.ApdL("      ,AR.AR_NO");
            sb.ApdL("      ,COM2.ITEM_NAME AS JYOKYO_FLAG");
            sb.ApdL("      ,CONVERT(NVARCHAR, AR.UPDATE_DATE, 111) AS UPDATE_DATE");
            sb.ApdL("      ,AR.UPDATE_USER_NAME");
            sb.ApdL("      ,AR.HASSEI_DATE");
            sb.ApdL("      ,AR.RENRAKUSHA");
            sb.ApdL("      ,AR.KISHU");
            sb.ApdL("      ,AR.GOKI");
            sb.ApdL("      ,AR.GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("      ,AR.HUGUAI");
            sb.ApdL("      ,AR.TAISAKU");
            sb.ApdL("      ,AR.TAIO_BUSHO");
            sb.ApdL("      ,AR.GIREN_NO_1");
            sb.ApdL("      ,AR.GIREN_NO_2");
            sb.ApdL("      ,AR.GIREN_NO_3");
            sb.ApdL("      ,AR.GIREN_NO_4");
            sb.ApdL("      ,AR.GIREN_NO_5");
            sb.ApdL("      ,AR.HASSEI_YOUIN");
            sb.ApdL("      ,AR.REFERENCE_NO_1");
            sb.ApdL("      ,AR.REFERENCE_NO_2");
            sb.ApdL("      ,AR.GENCHI_TEHAISAKI");
            sb.ApdL("      ,AR.GENCHI_SETTEINOKI_DATE");
            sb.ApdL("      ,AR.GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("      ,AR.GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("      ,AR.SHUKKAHOHO");
            sb.ApdL("      ,AR.JP_SETTEINOKI_DATE");
            sb.ApdL("      ,AR.JP_SHUKKAYOTEI_DATE");
            sb.ApdL("      ,AR.JP_KOJYOSHUKKA_DATE");
            sb.ApdL("      ,AR.JP_UNSOKAISHA_NAME");
            sb.ApdL("      ,AR.JP_OKURIJYO_NO");
            sb.ApdL("      ,AR.GMS_HAKKO_NO");
            sb.ApdL("      ,AR.SHIYORENRAKU_NO");
            sb.ApdL("      ,AR.BIKO");
            sb.ApdL("     , AR.MOTO_AR_NO");
            sb.ApdL("     , (SELECT AR_NO FROM T_AR ta1 WHERE ta1.MOTO_AR_NO = AR.AR_NO AND ta1.NONYUSAKI_CD = AR.NONYUSAKI_CD) AS KEKKA_AR_NO");
            sb.ApdL("  FROM T_AR AR");
            sb.ApdL(" INNER JOIN M_NONYUSAKI NON");
            sb.ApdL("         ON NON.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdN("        AND NON.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL(" INNER JOIN M_BUKKEN BK");
            sb.ApdL("         ON BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdN("        AND BK.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(LIST_FLAG.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = AR.LIST_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("         ON COM2.GROUP_CD = '").ApdN(JYOKYO_FLAG.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM2.VALUE1 = AR.JYOKYO_FLAG");
            sb.ApdN("        AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE AR.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL(" ORDER BY AR.AR_NO");

            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);
            return ds.Tables[Def_T_AR.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 全体費用Excel件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体費用Excel件数取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetAllARCostListCount(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM T_AR_COST AR_COST");
            sb.ApdL(" INNER JOIN T_AR");
            sb.ApdL("         ON T_AR.NONYUSAKI_CD = AR_COST.NONYUSAKI_CD");
            sb.ApdL("        AND T_AR.LIST_FLAG = AR_COST.LIST_FLAG");
            sb.ApdL("        AND T_AR.AR_NO = AR_COST.AR_NO");
            sb.ApdL(" INNER JOIN M_NONYUSAKI NON");
            sb.ApdL("         ON NON.NONYUSAKI_CD = AR_COST.NONYUSAKI_CD");
            sb.ApdN("        AND NON.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL(" INNER JOIN M_BUKKEN BK");
            sb.ApdL("         ON BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdN("        AND BK.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(AR_COST_ITEM.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = AR_COST.ITEM_CD");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdN("         ON M_USER.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN(" WHERE AR_COST.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("USER_ID", cond.UpdateUserID));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR_COST.Name);
            return ds.Tables[Def_T_AR_COST.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 全体費用Excel用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 全体費用Excel用データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetAllARCostList(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT BK.BUKKEN_NAME");
            sb.ApdL("      ,AR_COST.AR_NO");
            sb.ApdL("      ,AR_COST.LINE_NO");
            sb.ApdL("      ,COM1.ITEM_NAME AS ITEM_CD");
            sb.ApdL("      ,AR_COST.WORK_TIME");
            sb.ApdL("      ,AR_COST.WORKERS");
            sb.ApdL("      ,AR_COST.NUMBER");
            sb.ApdL("      ,CASE M_USER.STAFF_KBN");
            sb.ApdL("        WHEN '0' THEN AR_COST.RATE");
            sb.ApdL("        ELSE 0");
            sb.ApdL("       END RATE");
            sb.ApdL("      ,CASE M_USER.STAFF_KBN");
            sb.ApdL("        WHEN '0' THEN AR_COST.TOTAL");
            sb.ApdL("        ELSE 0");
            sb.ApdL("       END TOTAL");
            sb.ApdL("      ,AR.HASSEI_YOUIN");
            sb.ApdL("  FROM T_AR_COST AR_COST");
            sb.ApdL(" INNER JOIN T_AR AR");
            sb.ApdL("         ON AR.NONYUSAKI_CD = AR_COST.NONYUSAKI_CD");
            sb.ApdL("        AND AR.LIST_FLAG = AR_COST.LIST_FLAG");
            sb.ApdL("        AND AR.AR_NO = AR_COST.AR_NO");
            sb.ApdL(" INNER JOIN M_NONYUSAKI NON");
            sb.ApdL("         ON NON.NONYUSAKI_CD = AR_COST.NONYUSAKI_CD");
            sb.ApdN("        AND NON.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL(" INNER JOIN M_BUKKEN BK");
            sb.ApdL("         ON BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdN("        AND BK.SHUKKA_FLAG = '").ApdN(SHUKKA_FLAG.AR_VALUE1).ApdL("'");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = '").ApdN(AR_COST_ITEM.GROUPCD).ApdL("'");
            sb.ApdL("        AND COM1.VALUE1 = AR_COST.ITEM_CD");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdN("         ON M_USER.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN(" WHERE AR_COST.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL(" ORDER BY AR_COST.AR_NO");

            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("USER_ID", cond.UpdateUserID));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR_COST.Name);
            return ds.Tables[Def_T_AR_COST.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メール設定マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// メール設定マスタ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <returns>データテーブル</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMailSetting(DatabaseHelper dbHelper)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_MAIL_SETTING.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_MAIL_SETTING");
            
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

    #region ユーザーマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーマスタ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetUser(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_USER.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            
            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_USER");
            sb.ApdN(" WHERE M_USER.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            
            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UpdateUserID));

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

    #region 物件メールマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="isAR">ARリスト別で検索の場合はtrue、共通の場合はfalse</param>
    /// <returns>データテーブル</returns>
    /// <create>R.Katsuo 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukkenMail(DatabaseHelper dbHelper, CondA1 cond, bool isList)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdN(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND M_BUKKEN_MAIL.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND M_BUKKEN_MAIL.MAIL_KBN = ").ApdN(this.BindPrefix).ApdL("MAIL_KBN");
            if (isList)
            {
                sb.ApdN("   AND M_BUKKEN_MAIL.LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_KBN", isList ? MAIL_KBN.ARLIST_VALUE1 : MAIL_KBN.COMMON_VALUE1));

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

    #region 物件名マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>R.Katsuo 2017/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukken(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_BUKKEN.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdN(" WHERE M_BUKKEN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND M_BUKKEN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            
            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            
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

    #endregion //SELECT

    #endregion //A0100010:AR情報登録

    #region A0100020:AR情報明細登録

    #region SELECT

    #region AR情報データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="isLock">true:Lockする false:Lockしない</param>
    /// <returns>データテーブル</returns>
    /// <create>M.Tsutsumi 2010/08/19</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
    /// <update>D.Okumura 2019/07/30 AR進捗対応</update>
    /// <update>T.Nukaga 2019/11/21 AR7000番運用対応</update>
    /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
    /// --------------------------------------------------
    public DataTable GetAndLockAR(DatabaseHelper dbHelper, CondA1 cond, bool isLock)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , JYOKYO_FLAG");
            sb.ApdL("     , HASSEI_DATE");
            sb.ApdL("     , RENRAKUSHA");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , GOKI");
            sb.ApdL("     , GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("     , HUGUAI");
            sb.ApdL("     , TAISAKU");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , GENCHI_TEHAISAKI");
            sb.ApdL("     , GENCHI_SETTEINOKI_DATE");
            sb.ApdL("     , GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("     , GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("     , SHUKKAHOHO");
            sb.ApdL("     , JP_SETTEINOKI_DATE");
            sb.ApdL("     , JP_SHUKKAYOTEI_DATE");
            sb.ApdL("     , JP_KOJYOSHUKKA_DATE");
            sb.ApdL("     , JP_UNSOKAISHA_NAME");
            sb.ApdL("     , JP_OKURIJYO_NO");
            sb.ApdL("     , GMS_HAKKO_NO");
            sb.ApdL("     , SHIYORENRAKU_NO");
            sb.ApdL("     , TAIO_BUSHO");
            sb.ApdL("     , GIREN_NO_1");
            sb.ApdL("     , GIREN_FILE_1");
            sb.ApdL("     , GIREN_NO_2");
            sb.ApdL("     , GIREN_FILE_2");
            sb.ApdL("     , GIREN_NO_3");
            sb.ApdL("     , GIREN_FILE_3");
            sb.ApdL("     , GIREN_NO_4");
            sb.ApdL("     , GIREN_FILE_4");
            sb.ApdL("     , GIREN_NO_5");
            sb.ApdL("     , GIREN_FILE_5");
            sb.ApdL("     , SHINCHOKU_FLAG");
            sb.ApdL("     , HASSEI_YOUIN");
            sb.ApdL("     , REFERENCE_NO_1");
            sb.ApdL("     , REFERENCE_FILE_1");
            sb.ApdL("     , REFERENCE_NO_2");
            sb.ApdL("     , REFERENCE_FILE_2");
            sb.ApdL("     , MOTO_AR_NO");
            sb.ApdL("     , (SELECT AR_NO FROM T_AR ta1 WHERE ta1.MOTO_AR_NO = T_AR.AR_NO AND ta1.NONYUSAKI_CD = T_AR.NONYUSAKI_CD) AS KEKKA_AR_NO");
            sb.ApdL("     , LOCK_USER_ID");
            sb.ApdL("     , LOCK_STARTDATE");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , GENCHI_SHUKKAJYOKYO_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR");

            // LOCK
            if (isLock)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 納入先コード
            fieldName = "NONYUSAKI_CD";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));

            // リスト区分
            if (!string.IsNullOrEmpty(cond.ListFlag))
            {
                fieldName = "LIST_FLAG";
                sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlag));
            }

            // ARNO
            fieldName = "AR_NO";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ArNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);

            return ds.Tables[Def_T_AR.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR対応費用取得

    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetARCost(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_AR_COST.NONYUSAKI_CD");
            sb.ApdL("     , T_AR_COST.LIST_FLAG");
            sb.ApdL("     , T_AR_COST.AR_NO");
            sb.ApdL("     , T_AR_COST.LINE_NO");
            sb.ApdL("     , T_AR_COST.ITEM_CD");
            sb.ApdL("     , T_AR_COST.WORK_TIME");
            sb.ApdL("     , T_AR_COST.WORKERS");
            sb.ApdL("     , T_AR_COST.NUMBER");
            sb.ApdL("     , T_AR_COST.RATE");
            sb.ApdL("     , T_AR_COST.TOTAL");
            sb.ApdL("     , T_AR_COST.CREATE_DATE");
            sb.ApdL("     , T_AR_COST.CREATE_USER_ID");
            sb.ApdL("     , T_AR_COST.CREATE_USER_NAME");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR_COST");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 納入先コード
            fieldName = "NONYUSAKI_CD";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));

            // リスト区分
            fieldName = "LIST_FLAG";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlag));

            // ARNO
            fieldName = "AR_NO";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ArNo));

            sb.ApdL(" ORDER BY");
            sb.ApdL("       LINE_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR_COST.Name);

            return ds.Tables[Def_T_AR_COST.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR添付ファイル取得

    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイル取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetARFile(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_AR_FILE.NONYUSAKI_CD");
            sb.ApdL("     , T_AR_FILE.LIST_FLAG");
            sb.ApdL("     , T_AR_FILE.AR_NO");
            sb.ApdL("     , T_AR_FILE.FILE_KIND");
            sb.ApdL("     , T_AR_FILE.POSITION");
            sb.ApdL("     , T_AR_FILE.FILE_NAME");
            sb.ApdL("     , T_AR_FILE.CREATE_DATE");
            sb.ApdL("     , T_AR_FILE.CREATE_USER_ID");
            sb.ApdL("     , T_AR_FILE.CREATE_USER_NAME");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR_FILE");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 納入先コード
            fieldName = "NONYUSAKI_CD";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));

            // リスト区分
            fieldName = "LIST_FLAG";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ListFlag));

            // ARNO
            fieldName = "AR_NO";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ArNo));

            sb.ApdL(" ORDER BY");
            sb.ApdL("       FILE_KIND, POSITION");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR_FILE.Name);

            return ds.Tables[Def_T_AR_FILE.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="isNullShukkaDate">true:出荷日付をチェックする false:しない</param>
    /// <returns>出荷明細データ</returns>
    /// <create>M.Tsutsumi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShukkaMeisai(DatabaseHelper dbHelper, CondA1 cond, bool isNullShukkaDate)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , SEIBAN");
            sb.ApdL("     , CODE");
            sb.ApdL("     , ZUMEN_OIBAN");
            sb.ApdL("     , AREA");
            sb.ApdL("     , FLOOR");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , ST_NO");
            sb.ApdL("     , HINMEI_JP");
            sb.ApdL("     , HINMEI");
            sb.ApdL("     , ZUMEN_KEISHIKI");
            sb.ApdL("     , KUWARI_NO");
            sb.ApdL("     , NUM");
            sb.ApdL("     , JYOTAI_FLAG");
            sb.ApdL("     , TAGHAKKO_FLAG");
            sb.ApdL("     , TAGHAKKO_DATE");
            sb.ApdL("     , SHUKA_DATE");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , BOXKONPO_DATE");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , PALLETKONPO_DATE");
            sb.ApdL("     , KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , KIWAKUKONPO_DATE");
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UNSOKAISHA_NAME");
            sb.ApdL("     , INVOICE_NO");
            sb.ApdL("     , OKURIJYO_NO");
            sb.ApdL("     , BL_NO");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , UKEIRE_USER_ID");
            sb.ApdL("     , UKEIRE_USER_NAME");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 出荷区分
            fieldName = "SHUKKA_FLAG";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));

            // 納入先コード
            fieldName = "NONYUSAKI_CD";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));

            // ARNO
            fieldName = "AR_NO";
            sb.ApdN("   AND ").ApdN(fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ArNo));

            // 出荷日付
            if (isNullShukkaDate)
            {
                fieldName = "SHUKKA_DATE";
                sb.ApdN("   AND ").ApdN(fieldName).ApdL(" IS NULL");
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds.Tables[Def_T_SHUKKA_MEISAI.Name];

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
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetSelectItem(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT SELECT_GROUP_CD");
            sb.ApdL("      ,ITEM_NAME");
            sb.ApdL("  FROM M_SELECT_ITEM");
            sb.ApdN(" WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            sb.ApdL(" ORDER BY ITEM_NAME");

            paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCode));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);
            return ds.Tables[Def_M_SELECT_ITEM.Name];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region リスト区分取得

    /// --------------------------------------------------
    /// <summary>
    /// リスト区分取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetCommonListFlag(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_COMMON.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        GROUP_CD");
            sb.ApdL("       ,GROUP_NAME");
            sb.ApdL("       ,ITEM_CD");
            sb.ApdL("       ,ITEM_NAME");
            sb.ApdL("       ,VALUE1");
            sb.ApdL("       ,VALUE2");
            sb.ApdL("       ,(SELECT ITEM_NAME");
            sb.ApdL("           FROM M_COMMON COM2");
            sb.ApdL("          WHERE COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("            AND COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_SHIJI_GROUP_CD");
            sb.ApdL("            AND COM2.VALUE1 = M_COMMON.VALUE1");
            sb.ApdL("        ) AS VALUE3");
            sb.ApdL("       ,DISP_NO");
            sb.ApdL("       ,DEFAULT_VALUE");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_COMMON");
            sb.ApdL(" WHERE LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("   AND GROUP_CD = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_GROUP_CD");
            sb.ApdN("   AND VALUE1 = ").ApdN(this.BindPrefix).ApdL("VALUE1");
            sb.ApdL(" ORDER BY");
            sb.ApdL("        GROUP_CD");
            sb.ApdL("       ,DISP_NO");
            sb.ApdL("       ,VALUE1");

            // バインド変数設定
            paramCollection.Add(dbHelper.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(dbHelper.NewDbParameter("VALUE1", cond.ListFlag));
            paramCollection.Add(dbHelper.NewDbParameter("LIST_FLAG_GROUP_CD", LIST_FLAG.GROUPCD));
            paramCollection.Add(dbHelper.NewDbParameter("LIST_FLAG_SHIJI_GROUP_CD", LIST_FLAG_SHIJI.GROUPCD));

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

    #region 登録号機数の取得

    /// --------------------------------------------------
    /// <summary>
    /// 登録号機数の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">検索条件</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetGokiNum(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("FROM");
            sb.ApdL("       T_AR_GOKI");
            sb.ApdL("WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR進捗情報取得

    /// --------------------------------------------------
    /// <summary>
    /// AR進捗情報取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetArShinchoku(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("     NONYUSAKI_CD");
            sb.ApdL("    ,LIST_FLAG");
            sb.ApdL("    ,AR_NO");
            sb.ApdL("    ,GOKI");
            sb.ApdL("    ,DATE_SITE_REQ");
            sb.ApdL("    ,DATE_JP");
            sb.ApdL("    ,DATE_LOCAL");
            sb.ApdL("    ,CREATE_DATE");
            sb.ApdL("    ,CREATE_USER_ID");
            sb.ApdL("    ,CREATE_USER_NAME");
            sb.ApdL("    ,UPDATE_DATE");
            sb.ApdL("    ,UPDATE_USER_ID");
            sb.ApdL("    ,UPDATE_USER_NAME");
            sb.ApdL("    ,CONVERT(NCHAR(27), VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("  FROM");
            sb.ApdL("       T_AR_SHINCHOKU");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdL("ORDER BY");
            sb.ApdL("       GOKI");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));

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

    #region 進捗管理通知設定取得

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>個別が登録されていれば個別を返し、個別が未登録ならDefaultを返す</returns>
    /// <create>Y.Nakasato 2019/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShinchokuKanriNotify(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 個別登録があればそれを採用
            dt = this.GetMailNotification(dbHelper, cond, SHUKKA_FLAG.AR_VALUE1, cond.BukkenNo, null);

            // 個別登録がなければ進捗管理通知設定(Default)を採用
            if (dt.Rows.Count == 0)
            {
                dt = this.GetMailNotification(dbHelper, cond, MAIL_KBN.ARSHINCHOKU_VALUE1, null, null);
                // defaultを取得する場合、更新時に削除しないようにMailHeaderIDをクリアしておく
                foreach (DataRow dr in dt.Rows)
                {
                    dr.SetField(Def_M_BUKKEN_MAIL.MAIL_HEADER_ID, DBNull.Value);
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 基本設定通知取得(区分指定可)
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="mailKbn">メール区分</param>
    /// <param name="bukkenNo">物件番号(null許容)</param>
    /// <returns>データテーブル(M_BUKKEN_MAIL+M_USER)</returns>
    /// <create>Y.Nakasato 2019/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMailNotification(DatabaseHelper dbHelper, CondA1 cond, string mailKbn, string bukkenNo, string listKbn)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("     , M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("     , M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("     , M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("     , M_BUKKEN_MAIL.MAIL_KBN");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" INNER JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");

            // 条件およびパラメータを設定
            sb.ApdN(" WHERE M_BUKKEN_MAIL.MAIL_KBN = ").ApdN(this.BindPrefix).ApdL("MAIL_KBN");
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", mailKbn));
            // 物件の指定があるときは出荷フラグをARに限定して検索する
            if (!string.IsNullOrEmpty(bukkenNo))
            {
                sb.ApdN("   AND M_BUKKEN_MAIL.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                sb.ApdN("   AND M_BUKKEN_MAIL.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
                pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
                pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", bukkenNo));
            }
            if (!string.IsNullOrEmpty(listKbn))
            {
                sb.ApdN("   AND M_BUKKEN_MAIL.LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
                pc.Add(iNewParam.NewDbParameter("LIST_FLAG", listKbn));
            }
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");


            dbHelper.Fill(string.Format(sb.ToString(), this.BindPrefix), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 存在しない号機データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 存在しない号機データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション(NonyusakiCD,ListFlag,ArNo)</param>
    /// <param name="isLock">true:Lockする false:Lockしない</param>
    /// <returns>データテーブル</returns>
    /// <create>D.Okumura 2019/08/07 AR進捗対応</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetGokiNotExists(DatabaseHelper dbHelper, CondA1 cond, string arNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("     , SHINCHOKU.LIST_FLAG");
            sb.ApdL("     , SHINCHOKU.AR_NO");
            sb.ApdL("     , SHINCHOKU.GOKI");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR_SHINCHOKU AS SHINCHOKU");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (SELECT GOKI.GOKI ");
            sb.ApdL("       FROM T_AR_GOKI AS GOKI");
            sb.ApdL("       WHERE GOKI.NONYUSAKI_CD = SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("         AND GOKI.GOKI = SHINCHOKU.GOKI");
            sb.ApdL("       )");
            sb.ApdN("       AND SHINCHOKU.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("       AND SHINCHOKU.AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("       AND SHINCHOKU.LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");

            // バインド
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", arNo));

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

    #region 元ARNO重複チェック
    /// --------------------------------------------------
    /// <summary>
    /// 元ARNO重複チェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="motoArNo">元ARNo</param>
    /// <returns>true:他のARで元ARNOに設定済、false:未設定</returns>
    /// <create>T.Nukaga 2019/11/22 STEP12 AR7000番運用対応</create>
    /// <update>D.Okumura 2019/12/18 元ARNoをコンディションを用いずに連携するように修正</update>
    /// --------------------------------------------------
    private DataTable GetMotoArNo(DatabaseHelper dbHelper, CondA1 cond, string motoArNo)
    {
        DataTable dt = new DataTable(Def_T_AR.Name);
        StringBuilder sb = new StringBuilder();
        DbParamCollection paramCollection = new DbParamCollection();
        INewDbParameterBasic iNewParam = dbHelper;
        string fieldName = string.Empty;

        // SQL文
        sb.ApdL("SELECT");
        sb.ApdL("       AR_NO");
        sb.ApdL("     , MOTO_AR_NO");
        sb.ApdL("  FROM ");
        sb.ApdL("       T_AR");
        sb.ApdL(" WHERE");
        sb.ApdL("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
        sb.ApdL("   AND MOTO_AR_NO = ").ApdN(this.BindPrefix).ApdL("MOTO_AR_NO");
        sb.ApdN("   AND AR_NO <> ").ApdN(this.BindPrefix).ApdL("AR_NO");

        // バインド
        paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
        paramCollection.Add(iNewParam.NewDbParameter("MOTO_AR_NO", motoArNo));
        paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));

        // SQL実行
        dbHelper.Fill(sb.ToString(), paramCollection, dt);

        return dt;
    }
    #endregion

    #endregion //SELECT

    #region INSERT

    #region 納入先マスタ登録

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">納入先用コンディション</param>
    /// <param name="dr">納入先マスタ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/08/20</create>
    /// <update>M.Tsutsumi 2011/02/16</update>
    /// --------------------------------------------------
    public int InsNonyusakiExec(DatabaseHelper dbHelper, CondNonyusaki cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_NONYUSAKI");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , NONYUSAKI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , KANRI_FLAG");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            sb.ApdL("     , LIST_FLAG_NAME0");
            sb.ApdL("     , LIST_FLAG_NAME1");
            sb.ApdL("     , LIST_FLAG_NAME2");
            sb.ApdL("     , LIST_FLAG_NAME3");
            sb.ApdL("     , LIST_FLAG_NAME4");
            sb.ApdL("     , LIST_FLAG_NAME5");
            sb.ApdL("     , LIST_FLAG_NAME6");
            sb.ApdL("     , LIST_FLAG_NAME7");
            sb.ApdL("     , REMOVE_FLAG");
            // @@@ ↑
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME0");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME3");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME4");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME5");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME6");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME7");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdL("REMOVE_FLAG");
            // @@@ ↑
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.SHUKKA_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.BUKKEN_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.NONYUSAKI_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_NAME", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.SHIP)));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.KANRI_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", DBNull.Value));
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME0", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME0)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME1", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME1)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME2", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME2)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME3", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME3)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME4", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME4)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME5", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME5)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME6", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME6)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME7", ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME7)));
            paramCollection.Add(iNewParam.NewDbParameter("REMOVE_FLAG", REMOVE_FLAG.NORMAL_VALUE1));
            // @@@ ↑

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

    #region AR情報データ登録

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dr">AR情報データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/08/20</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
    /// <update>D.Okumura 2019/08/05 AR進捗応</update>
    /// <update>N.Kawamura 2022/10/06 出荷状況自動反映機能追加</update>
    /// --------------------------------------------------
    public int InsARExec(DatabaseHelper dbHelper, CondA1 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            //Null回避
            if (string.IsNullOrEmpty(cond.IsGenchiShukkaJyokyo))
            {
                cond.IsGenchiShukkaJyokyo = "";
            }

            // SQL文
            sb.ApdL("INSERT INTO T_AR");
            sb.ApdL("(");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , JYOKYO_FLAG");
            sb.ApdL("     , HASSEI_DATE");
            sb.ApdL("     , RENRAKUSHA");
            sb.ApdL("     , SHINCHOKU_FLAG");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , GOKI");
            sb.ApdL("     , GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("     , HUGUAI");
            sb.ApdL("     , TAISAKU");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , GENCHI_TEHAISAKI");
            sb.ApdL("     , GENCHI_SETTEINOKI_DATE");
            sb.ApdL("     , GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("     , GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("     , SHUKKAHOHO");
            sb.ApdL("     , JP_SETTEINOKI_DATE");
            sb.ApdL("     , JP_SHUKKAYOTEI_DATE");
            sb.ApdL("     , JP_KOJYOSHUKKA_DATE");
            sb.ApdL("     , JP_UNSOKAISHA_NAME");
            sb.ApdL("     , JP_OKURIJYO_NO");
            sb.ApdL("     , GMS_HAKKO_NO");
            sb.ApdL("     , SHIYORENRAKU_NO");
            sb.ApdL("     , TAIO_BUSHO");
            sb.ApdL("     , GIREN_NO_1");
            sb.ApdL("     , GIREN_FILE_1");
            sb.ApdL("     , GIREN_NO_2");
            sb.ApdL("     , GIREN_FILE_2");
            sb.ApdL("     , GIREN_NO_3");
            sb.ApdL("     , GIREN_FILE_3");
            sb.ApdL("     , GIREN_NO_4");
            sb.ApdL("     , GIREN_FILE_4");
            sb.ApdL("     , GIREN_NO_5");
            sb.ApdL("     , GIREN_FILE_5");
            sb.ApdL("     , HASSEI_YOUIN");
            sb.ApdL("     , REFERENCE_NO_1");
            sb.ApdL("     , REFERENCE_FILE_1");
            sb.ApdL("     , REFERENCE_NO_2");
            sb.ApdL("     , REFERENCE_FILE_2");
            sb.ApdL("     , MOTO_AR_NO");
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UKEIRE_DATE");
            sb.ApdL("     , UKEIRE_USER_ID");
            sb.ApdL("     , UKEIRE_USER_NAME");
            sb.ApdL("     , LOCK_USER_ID");
            sb.ApdL("     , LOCK_STARTDATE");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , GENCHI_SHUKKAJYOKYO_FLAG");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JYOKYO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HASSEI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("RENRAKUSHA");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHINCHOKU_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GOKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HUGUAI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAISAKU");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BIKO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENCHI_TEHAISAKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENCHI_SETTEINOKI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKAHOHO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JP_SETTEINOKI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JP_SHUKKAYOTEI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JP_KOJYOSHUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JP_UNSOKAISHA_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JP_OKURIJYO_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GMS_HAKKO_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIYORENRAKU_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAIO_BUSHO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_NO_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_NO_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_NO_3");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_3");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_NO_4");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_4");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_NO_5");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_5");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HASSEI_YOUIN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REFERENCE_NO_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REFERENCE_FILE_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REFERENCE_NO_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REFERENCE_FILE_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MOTO_AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UKEIRE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UKEIRE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCK_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCK_STARTDATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GENCHI_SHUKKAJYOKYO_FLAG");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR.NONYUSAKI_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.LIST_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.AR_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.JYOKYO_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("HASSEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.HASSEI_DATE)));
            paramCollection.Add(iNewParam.NewDbParameter("RENRAKUSHA", ComFunc.GetFldObject(dr, Def_T_AR.RENRAKUSHA, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHINCHOKU_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.SHINCHOKU_FLAG, AR_SHINCHOKU_FLAG.DEFAULT_VALUE1)));
            paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_T_AR.KISHU, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR.GOKI, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GENBA_TOTYAKUKIBOU_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HUGUAI", ComFunc.GetFldObject(dr, Def_T_AR.HUGUAI, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("TAISAKU", ComFunc.GetFldObject(dr, Def_T_AR.TAISAKU, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("BIKO", ComFunc.GetFldObject(dr, Def_T_AR.BIKO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GENCHI_TEHAISAKI", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_TEHAISAKI, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SETTEINOKI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SETTEINOKI_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GENCHI_KOJYOSHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_KOJYOSHUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKAHOHO", ComFunc.GetFldObject(dr, Def_T_AR.SHUKKAHOHO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("JP_SETTEINOKI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_SETTEINOKI_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("JP_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_SHUKKAYOTEI_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("JP_KOJYOSHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_KOJYOSHUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("JP_UNSOKAISHA_NAME", ComFunc.GetFldObject(dr, Def_T_AR.JP_UNSOKAISHA_NAME, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("JP_OKURIJYO_NO", ComFunc.GetFldObject(dr, Def_T_AR.JP_OKURIJYO_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GMS_HAKKO_NO", ComFunc.GetFldObject(dr, Def_T_AR.GMS_HAKKO_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIYORENRAKU_NO", ComFunc.GetFldObject(dr, Def_T_AR.SHIYORENRAKU_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("TAIO_BUSHO", ComFunc.GetFldObject(dr, Def_T_AR.TAIO_BUSHO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_1", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_1", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_2", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_2", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_3", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_3, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_3", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_3, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_4", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_4, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_4", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_4, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_5", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_5, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_5", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_5, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HASSEI_YOUIN", ComFunc.GetFldObject(dr, Def_T_AR.HASSEI_YOUIN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_NO_1", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_NO_1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_FILE_1", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_FILE_1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_NO_2", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_NO_2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_FILE_2", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_FILE_2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("MOTO_AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.MOTO_AR_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("UKEIRE_USER_NAME", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_STARTDATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", ComFunc.GetFldObject(dr, Def_T_AR.CREATE_DATE, cond.UpdateDate)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", ComFunc.GetFldObject(dr, Def_T_AR.CREATE_USER_ID, this.GetCreateUserID(cond))));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", ComFunc.GetFldObject(dr, Def_T_AR.CREATE_USER_NAME, this.GetCreateUserName(cond))));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_DATE, cond.UpdateDate)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_USER_ID, this.GetUpdateUserID(cond))));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_USER_NAME, this.GetUpdateUserName(cond))));
            paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SHUKKAJYOKYO_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SHUKKAJYOKYO_FLAG, cond.IsGenchiShukkaJyokyo)));

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

    #region AR対応費用データ登録

    /// --------------------------------------------------
    /// <summary>
    /// AR対応費用データ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dr">AR情報データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/10/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsARCostExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_AR_COST");
            sb.ApdL("(");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , LINE_NO");
            sb.ApdL("     , ITEM_CD");
            sb.ApdL("     , WORK_TIME");
            sb.ApdL("     , WORKERS");
            sb.ApdL("     , NUMBER");
            sb.ApdL("     , RATE");
            sb.ApdL("     , TOTAL");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LINE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ITEM_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_TIME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORKERS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NUMBER");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("RATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TOTAL");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdL(")");


            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_COST.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_COST.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_COST.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LINE_NO", ComFunc.GetFldObject(dr, Def_T_AR_COST.LINE_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("ITEM_CD", ComFunc.GetFldObject(dr, Def_T_AR_COST.ITEM_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("WORK_TIME", ComFunc.GetFldObject(dr, Def_T_AR_COST.WORK_TIME)));
                paramCollection.Add(iNewParam.NewDbParameter("WORKERS", ComFunc.GetFldObject(dr, Def_T_AR_COST.WORKERS)));
                paramCollection.Add(iNewParam.NewDbParameter("NUMBER", ComFunc.GetFldObject(dr, Def_T_AR_COST.NUMBER)));

                var rate = ComFunc.GetFldObject(dr, Def_T_AR_COST.RATE);
                if (rate != null && DSWUtil.UtilConvert.ToDecimal(rate) > 0.0m)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("RATE", rate));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("RATE", DBNull.Value));
                }
                var total = ComFunc.GetFldObject(dr, Def_T_AR_COST.TOTAL);
                if (total != null && DSWUtil.UtilConvert.ToDecimal(total) > 0.0m)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("TOTAL", total));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("TOTAL", DBNull.Value));
                }
                
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", ComFunc.GetFldObject(dr, Def_T_AR_COST.CREATE_DATE, cond.UpdateDate)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", ComFunc.GetFldObject(dr, Def_T_AR_COST.CREATE_USER_ID, this.GetCreateUserID(cond))));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", ComFunc.GetFldObject(dr, Def_T_AR_COST.CREATE_USER_NAME, this.GetCreateUserName(cond))));

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

    #region AR添付ファイルデータ登録

    /// --------------------------------------------------
    /// <summary>
    /// AR添付ファイルデータ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dr">AR情報データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/06/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsARFileExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_AR_FILE");
            sb.ApdL("(");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , FILE_KIND");
            sb.ApdL("     , POSITION");
            sb.ApdL("     , FILE_NAME");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FILE_KIND");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("POSITION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FILE_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdL(")");


            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_FILE.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_FILE.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_FILE.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("FILE_KIND", ComFunc.GetFldObject(dr, Def_T_AR_FILE.FILE_KIND)));
                paramCollection.Add(iNewParam.NewDbParameter("POSITION", ComFunc.GetFldObject(dr, Def_T_AR_FILE.POSITION)));
                paramCollection.Add(iNewParam.NewDbParameter("FILE_NAME", ComFunc.GetFldObject(dr, Def_T_AR_FILE.FILE_NAME)));

                paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", ComFunc.GetFldObject(dr, Def_T_AR_FILE.CREATE_DATE, cond.UpdateDate)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", ComFunc.GetFldObject(dr, Def_T_AR_FILE.CREATE_USER_ID, this.GetCreateUserID(cond))));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", ComFunc.GetFldObject(dr, Def_T_AR_FILE.CREATE_USER_NAME, this.GetCreateUserName(cond))));

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

    #region 履歴データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 履歴データ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dr">AR情報データ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/05/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsRirekiExec(DatabaseHelper dbHelper, CondA1 cond, DataRow dr)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO T_RIREKI (");
            sb.ApdL("       GAMEN_FLAG");
            sb.ApdL("      ,SHUKKA_FLAG");
            sb.ApdL("      ,NONYUSAKI_CD");
            sb.ApdL("      ,SHIP");
            sb.ApdL("      ,AR_NO");
            sb.ApdL("      ,BUKKEN_NO");
            sb.ApdL("      ,OPERATION_FLAG");
            sb.ApdL("      ,UPDATE_PC_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdL("       ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_PC_NAME");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdL("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL("      ,").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            paramCollection.Add(iNewParameter.NewDbParameter("GAMEN_FLAG", ComFunc.GetFldObject(dr, Def_T_RIREKI.GAMEN_FLAG)));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_RIREKI.SHUKKA_FLAG)));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_RIREKI.NONYUSAKI_CD)));
            paramCollection.Add(iNewParameter.NewDbParameter("SHIP", ComFunc.GetFldObject(dr, Def_T_RIREKI.SHIP)));
            paramCollection.Add(iNewParameter.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_RIREKI.AR_NO)));
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_T_RIREKI.BUKKEN_NO)));
            paramCollection.Add(iNewParameter.NewDbParameter("OPERATION_FLAG", ComFunc.GetFldObject(dr, Def_T_RIREKI.OPERATION_FLAG)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_PC_NAME", ComFunc.GetFldObject(dr, Def_T_RIREKI.UPDATE_PC_NAME)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_DATE, cond.UpdateDate)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_USER_ID, this.GetUpdateUserID(cond))));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", ComFunc.GetFldObject(dr, Def_T_AR.UPDATE_USER_NAME, this.GetUpdateUserName(cond))));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region AR進捗データ挿入

    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データ更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">AR進捗データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsArShinchokuExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_AR_SHINCHOKU");
            sb.ApdL("(");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , GOKI");
            sb.ApdL("     , DATE_SITE_REQ");
            sb.ApdL("     , DATE_LOCAL");
            sb.ApdL("     , DATE_JP");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GOKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DATE_SITE_REQ");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DATE_LOCAL");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DATE_JP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.GOKI)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_SITE_REQ", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_SITE_REQ)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_LOCAL", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_LOCAL)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_JP", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_JP)));

                paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #endregion //INSERT

    #region UPDATE

    #region AR情報データ更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/08/20</create>
    /// <update>H.Tajimi 2018/10/15 FE要望対応</update>
    /// <update>D.Okumura 2019/06/18 添付ファイル対応</update>
    /// <update>D.Okumura 2019/08/05 AR進捗応</update>
    /// <create>T.Nukaga 2019/11/21 AR7000番運用対応</create>
    /// --------------------------------------------------
    public int UpdARExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       JYOKYO_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOKYO_FLAG");
            sb.ApdN("     , HASSEI_DATE = ").ApdN(this.BindPrefix).ApdL("HASSEI_DATE");
            sb.ApdN("     , RENRAKUSHA = ").ApdN(this.BindPrefix).ApdL("RENRAKUSHA");
            sb.ApdN("     , SHINCHOKU_FLAG = ").ApdN(this.BindPrefix).ApdL("SHINCHOKU_FLAG");
            sb.ApdN("     , KISHU = ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , GOKI = ").ApdN(this.BindPrefix).ApdL("GOKI");
            sb.ApdN("     , GENBA_TOTYAKUKIBOU_DATE = ").ApdN(this.BindPrefix).ApdL("GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdN("     , HUGUAI = ").ApdN(this.BindPrefix).ApdL("HUGUAI");
            sb.ApdN("     , TAISAKU = ").ApdN(this.BindPrefix).ApdL("TAISAKU");
            sb.ApdN("     , BIKO = ").ApdN(this.BindPrefix).ApdL("BIKO");
            sb.ApdN("     , GENCHI_TEHAISAKI = ").ApdN(this.BindPrefix).ApdL("GENCHI_TEHAISAKI");
            sb.ApdN("     , GENCHI_SETTEINOKI_DATE = ").ApdN(this.BindPrefix).ApdL("GENCHI_SETTEINOKI_DATE");
            sb.ApdN("     , GENCHI_SHUKKAYOTEI_DATE = ").ApdN(this.BindPrefix).ApdL("GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdN("     , GENCHI_KOJYOSHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdN("     , SHUKKAHOHO = ").ApdN(this.BindPrefix).ApdL("SHUKKAHOHO");
            sb.ApdN("     , JP_SETTEINOKI_DATE = ").ApdN(this.BindPrefix).ApdL("JP_SETTEINOKI_DATE");
            sb.ApdN("     , JP_SHUKKAYOTEI_DATE = ").ApdN(this.BindPrefix).ApdL("JP_SHUKKAYOTEI_DATE");
            sb.ApdN("     , JP_KOJYOSHUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("JP_KOJYOSHUKKA_DATE");
            sb.ApdN("     , JP_UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("JP_UNSOKAISHA_NAME");
            sb.ApdN("     , JP_OKURIJYO_NO = ").ApdN(this.BindPrefix).ApdL("JP_OKURIJYO_NO");
            sb.ApdN("     , GMS_HAKKO_NO = ").ApdN(this.BindPrefix).ApdL("GMS_HAKKO_NO");
            sb.ApdN("     , SHIYORENRAKU_NO = ").ApdN(this.BindPrefix).ApdL("SHIYORENRAKU_NO");
            sb.ApdN("     , TAIO_BUSHO = ").ApdN(this.BindPrefix).ApdL("TAIO_BUSHO");
            sb.ApdN("     , GIREN_NO_1 = ").ApdN(this.BindPrefix).ApdL("GIREN_NO_1");
            sb.ApdN("     , GIREN_FILE_1 = ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_1");
            sb.ApdN("     , GIREN_NO_2 = ").ApdN(this.BindPrefix).ApdL("GIREN_NO_2");
            sb.ApdN("     , GIREN_FILE_2 = ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_2");
            sb.ApdN("     , GIREN_NO_3 = ").ApdN(this.BindPrefix).ApdL("GIREN_NO_3");
            sb.ApdN("     , GIREN_FILE_3 = ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_3");
            sb.ApdN("     , GIREN_NO_4 = ").ApdN(this.BindPrefix).ApdL("GIREN_NO_4");
            sb.ApdN("     , GIREN_FILE_4 = ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_4");
            sb.ApdN("     , GIREN_NO_5 = ").ApdN(this.BindPrefix).ApdL("GIREN_NO_5");
            sb.ApdN("     , GIREN_FILE_5 = ").ApdN(this.BindPrefix).ApdL("GIREN_FILE_5");
            sb.ApdN("     , HASSEI_YOUIN = ").ApdN(this.BindPrefix).ApdL("HASSEI_YOUIN");
            sb.ApdN("     , REFERENCE_NO_1 = ").ApdN(this.BindPrefix).ApdL("REFERENCE_NO_1");
            sb.ApdN("     , REFERENCE_FILE_1 = ").ApdN(this.BindPrefix).ApdL("REFERENCE_FILE_1");
            sb.ApdN("     , REFERENCE_NO_2 = ").ApdN(this.BindPrefix).ApdL("REFERENCE_NO_2");
            sb.ApdN("     , REFERENCE_FILE_2 = ").ApdN(this.BindPrefix).ApdL("REFERENCE_FILE_2");
            sb.ApdN("     , MOTO_AR_NO = ").ApdN(this.BindPrefix).ApdL("MOTO_AR_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , GENCHI_SHUKKAJYOKYO_FLAG = ").ApdN(this.BindPrefix).ApdL("GENCHI_SHUKKAJYOKYO_FLAG");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.JYOKYO_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("HASSEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.HASSEI_DATE)));
                paramCollection.Add(iNewParam.NewDbParameter("RENRAKUSHA", ComFunc.GetFldObject(dr, Def_T_AR.RENRAKUSHA, "")));
                paramCollection.Add(iNewParam.NewDbParameter("SHINCHOKU_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.SHINCHOKU_FLAG, AR_SHINCHOKU_FLAG.DEFAULT_VALUE1)));
                paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_T_AR.KISHU, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR.GOKI, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENBA_TOTYAKUKIBOU_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("HUGUAI", ComFunc.GetFldObject(dr, Def_T_AR.HUGUAI, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TAISAKU", ComFunc.GetFldObject(dr, Def_T_AR.TAISAKU, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("BIKO", ComFunc.GetFldObject(dr, Def_T_AR.BIKO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_TEHAISAKI", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_TEHAISAKI, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SETTEINOKI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SETTEINOKI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_KOJYOSHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_KOJYOSHUKKA_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKAHOHO", ComFunc.GetFldObject(dr, Def_T_AR.SHUKKAHOHO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_SETTEINOKI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_SETTEINOKI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_SHUKKAYOTEI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_KOJYOSHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_KOJYOSHUKKA_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_UNSOKAISHA_NAME", ComFunc.GetFldObject(dr, Def_T_AR.JP_UNSOKAISHA_NAME, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_OKURIJYO_NO", ComFunc.GetFldObject(dr, Def_T_AR.JP_OKURIJYO_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GMS_HAKKO_NO", ComFunc.GetFldObject(dr, Def_T_AR.GMS_HAKKO_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIYORENRAKU_NO", ComFunc.GetFldObject(dr, Def_T_AR.SHIYORENRAKU_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TAIO_BUSHO", ComFunc.GetFldObject(dr, Def_T_AR.TAIO_BUSHO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_1", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_1", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_2", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_2", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_3", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_3, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_3", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_3, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_4", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_4, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_4", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_4, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_NO_5", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_NO_5, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GIREN_FILE_5", ComFunc.GetFldObject(dr, Def_T_AR.GIREN_FILE_5, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("HASSEI_YOUIN", ComFunc.GetFldObject(dr, Def_T_AR.HASSEI_YOUIN, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_NO_1", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_NO_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_FILE_1", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_FILE_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_NO_2", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_NO_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("REFERENCE_FILE_2", ComFunc.GetFldObject(dr, Def_T_AR.REFERENCE_FILE_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("MOTO_AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.MOTO_AR_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SHUKKAJYOKYO_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SHUKKAJYOKYO_FLAG, DBNull.Value)));

                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.AR_NO)));

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

    #region AR情報データ インターロック

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdARInterLock(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("LOCK_USER_ID");
            sb.ApdN("     , LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("LOCK_STARTDATE");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_STARTDATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));

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

    #region AR情報データ インターロック解除

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック解除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">ARデータ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdARInterUnLock(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("LOCK_USER_ID");
            sb.ApdN("     , LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("LOCK_STARTDATE");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("   AND LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("NOW_LOCK_USER_ID");
            sb.ApdN("   AND LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("NOW_LOCK_STARTDATE");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_STARTDATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ArNo));
            paramCollection.Add(iNewParam.NewDbParameter("NOW_LOCK_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("NOW_LOCK_STARTDATE", dt.Rows[0][Def_T_AR.LOCK_STARTDATE]));

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

    #endregion //UPDATE

    #region DELETE

    #region AR進捗データ削除
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">AR進捗データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelArShinchokuExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_AR_SHINCHOKU");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("   AND GOKI = ").ApdN(this.BindPrefix).ApdL("GOKI");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.GOKI)));

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
    #endregion //DELETE

    #endregion //A0100020:AR情報明細登録

    #region A0100030:AR情報Help

    #region SELECT
    #endregion

    #region INSERT
    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    #endregion

    #endregion

    #region A0100040:員数表取込


    #region SELECT
    /// --------------------------------------------------
    /// <summary>
    /// 員数表一時取込テーブル削除対象使用中データ抽出
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01条件</param>
    /// <param name="tempId">一時取込ID</param>
    /// <param name="isLock">ロック有無</param>
    /// <returns>使用中のデータテーブル(T_AR_GOKI)</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetInzuhyoLock(DatabaseHelper dbHelper, CondA1 cond, string tempId, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI.Name);
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("    GOKI.NONYUSAKI_CD");
            sb.ApdL("  , GOKI.GOKI");
            sb.ApdL("  , GOKI.KISHU");
            sb.ApdL(" FROM T_AR_GOKI AS GOKI");
            // LOCK
            if (isLock)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL("    WHERE EXISTS (SELECT WORK.GOKI");
            sb.ApdL("      FROM T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdL("      WHERE WORK.NONYUSAKI_CD = GOKI.NONYUSAKI_CD");
            sb.ApdL("        AND WORK.GOKI = GOKI.GOKI");
            sb.ApdN("        AND WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("      )");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
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
    /// 員数表一時取込テーブル削除対象使用中データ抽出
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01条件</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>使用中のデータテーブル(T_AR_GOKI_TEMPWORK)</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CheckInzuhyoWorkUsedOnDelete(DatabaseHelper dbHelper, CondA1 cond, string tempId)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI_TEMPWORK.Name);
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("    WORK.TEMP_ID");
            sb.ApdL("  , WORK.GOKI");
            sb.ApdL("  , WORK.KISHU");
            sb.ApdL("  , WORK.NONYUSAKI_CD");
            sb.ApdL(" FROM T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdL("    WHERE EXISTS (SELECT SHINCHOKU.GOKI");
            sb.ApdL("      FROM T_AR_SHINCHOKU AS SHINCHOKU");
            sb.ApdL("      WHERE WORK.NONYUSAKI_CD = SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("        AND WORK.GOKI = SHINCHOKU.GOKI");
            sb.ApdL("      )");
            sb.ApdN("    AND WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("    AND WORK.FLAG = ").ApdN(BindPrefix).ApdL("FLAG");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("FLAG", AR_INZU_IMPORT_FLAG.DELETE_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
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
    /// 員数表一時取込テーブル削除対象使用中データ抽出
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01条件</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>使用中のデータテーブル(T_AR_GOKI_TEMPWORK)</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CheckInzuhyoWorkNotFoundOnDelete(DatabaseHelper dbHelper, CondA1 cond, string tempId)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI_TEMPWORK.Name);
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("    WORK.TEMP_ID");
            sb.ApdL("  , WORK.GOKI");
            sb.ApdL("  , WORK.KISHU");
            sb.ApdL("  , WORK.NONYUSAKI_CD");
            sb.ApdL(" FROM T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdL("    WHERE NOT EXISTS (SELECT GOKI.GOKI");
            sb.ApdL("      FROM T_AR_GOKI AS GOKI");
            sb.ApdL("      WHERE WORK.NONYUSAKI_CD = GOKI.NONYUSAKI_CD");
            sb.ApdL("        AND WORK.GOKI = GOKI.GOKI");
            sb.ApdL("      )");
            sb.ApdN("    AND WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("    AND WORK.FLAG = ").ApdN(BindPrefix).ApdL("FLAG");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("FLAG", AR_INZU_IMPORT_FLAG.DELETE_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
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
    /// 員数表一時取込テーブル機種使用中データ抽出(更新)
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01条件</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>使用中のデータテーブル(T_AR,KISHU列のみ)</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable CheckInzuhyoWorkUsedOnUpdate(DatabaseHelper dbHelper, CondA1 cond, string tempId)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR.Name);
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT DISTINCT AR.KISHU FROM T_AR AS AR");
            sb.ApdN("  WHERE AR.SHINCHOKU_FLAG = ").ApdN(BindPrefix).ApdL("SHINCHOKU_FLAG");
            sb.ApdN("  AND AR.NONYUSAKI_CD = ").ApdN(BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("  AND AR.KISHU <> ''");
            // 存在しない機種: 取込対象データを除いたAR号機に登録されている機種一覧
            sb.ApdL("  AND NOT EXISTS (");
            sb.ApdL("    SELECT GOKI.KISHU FROM T_AR_GOKI AS GOKI");
            sb.ApdL("    WHERE NOT EXISTS (SELECT WORK.GOKI");
            sb.ApdL("        FROM T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdN("        WHERE WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("          AND WORK.NONYUSAKI_CD = GOKI.NONYUSAKI_CD");
            sb.ApdL("          AND WORK.GOKI = GOKI.GOKI");
            sb.ApdL("      ) ");
            sb.ApdL("      AND GOKI.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("      AND GOKI.KISHU = AR.KISHU");
            sb.ApdL("  )");
            // 存在しない機種: 取込対象データの機種一覧
            sb.ApdL("  AND NOT EXISTS (");
            sb.ApdL("    SELECT WORK.KISHU FROM T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdN("    WHERE WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("      AND WORK.FLAG = ").ApdN(BindPrefix).ApdL("FLAG_UPDATE");
            sb.ApdL("      AND WORK.KISHU = AR.KISHU");
            sb.ApdL("  )");
            // 存在する機種: 取込対象データの機種一覧
            sb.ApdL("  AND EXISTS (");
            sb.ApdL("    SELECT GOKI.KISHU FROM T_AR_GOKI AS GOKI");
            sb.ApdL("    INNER JOIN T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdL("         ON WORK.NONYUSAKI_CD = GOKI.NONYUSAKI_CD");
            sb.ApdL("        AND WORK.GOKI = GOKI.GOKI");
            sb.ApdN("    WHERE WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("      AND GOKI.NONYUSAKI_CD = AR.NONYUSAKI_CD");
            sb.ApdL("      AND GOKI.KISHU = AR.KISHU");
            sb.ApdL("  )");


            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("FLAG_UPDATE", AR_INZU_IMPORT_FLAG.UPDATE_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("SHINCHOKU_FLAG", AR_SHINCHOKU_FLAG.ON_VALUE1));
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

    #region INSERT
    /// --------------------------------------------------
    /// <summary>
    /// 員数表INSERT
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="dt">員数表の取込用テーブル</param>
    /// <param name="cond">A01条件</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsInzuhyoWorkExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string tempId)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO T_AR_GOKI_TEMPWORK (");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("      ,GOKI");
            sb.ApdL("      ,KISHU");
            sb.ApdL("      ,FLAG");
            sb.ApdL("      ,TEMP_ID");
            
            sb.ApdL("    ) VALUES (");
            sb.ApdN("       ").ApdN(BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("GOKI");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("KISHU");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("FLAG");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("    )");

            int affectedRows = 0;
            foreach (DataRow dr in dt.Rows)
            {
                var paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
                paramCollection.Add(iNewParameter.NewDbParameter("GOKI", dr[Def_T_AR_GOKI_TEMPWORK.GOKI]));
                paramCollection.Add(iNewParameter.NewDbParameter("KISHU", dr[Def_T_AR_GOKI_TEMPWORK.KISHU]));
                paramCollection.Add(iNewParameter.NewDbParameter("FLAG", dr[Def_T_AR_GOKI_TEMPWORK.FLAG]));
                paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
                affectedRows += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return affectedRows;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// 員数表INSERT
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="dt">員数表の取込用テーブル</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private int MergeInzuhyoExec(DatabaseHelper dbHelper, CondA1 cond, string tempId)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL(" MERGE INTO T_AR_GOKI");
            sb.ApdL(" USING (SELECT NONYUSAKI_CD, GOKI, KISHU FROM T_AR_GOKI_TEMPWORK");
            sb.ApdN("        WHERE TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("          AND FLAG = ").ApdN(BindPrefix).ApdL("FLAG_UPDATE");
            sb.ApdL("    ) AS TMP");
            sb.ApdL("    ON T_AR_GOKI.NONYUSAKI_CD = TMP.NONYUSAKI_CD");
            sb.ApdL("   AND T_AR_GOKI.GOKI = TMP.GOKI");
            sb.ApdL("  WHEN MATCHED THEN");
            sb.ApdL("    UPDATE");
            sb.ApdL("    SET KISHU = TMP.KISHU");
            sb.ApdN("      , UPDATE_DATE = ").ApdN(BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("      , UPDATE_USER_ID = ").ApdN(BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      , UPDATE_USER_NAME = ").ApdN(BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      , VERSION = ").ApdN(BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("  WHEN NOT MATCHED THEN");
            sb.ApdL("    INSERT (");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("      ,GOKI");
            sb.ApdL("      ,KISHU");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");

            sb.ApdL("    ) VALUES (");
            sb.ApdL("       TMP.NONYUSAKI_CD");
            sb.ApdL("      ,TMP.GOKI");
            sb.ApdL("      ,TMP.KISHU");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,").ApdN(BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdL("    )");
            sb.ApdL(";");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("FLAG_UPDATE", AR_INZU_IMPORT_FLAG.UPDATE_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region UPDATE
    #endregion

    #region DELETE
    /// --------------------------------------------------
    /// <summary>
    /// 員数表Delete
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="dt">員数表の取込用テーブル</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelInzuhyoExec(DatabaseHelper dbHelper, CondA1 cond, string tempId)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE GOKI FROM T_AR_GOKI AS GOKI");
            sb.ApdL(" INNER JOIN T_AR_GOKI_TEMPWORK AS WORK");
            sb.ApdL("   ON  GOKI.NONYUSAKI_CD = WORK.NONYUSAKI_CD");
            sb.ApdL("   AND GOKI.GOKI = WORK.GOKI");
            sb.ApdN(" WHERE WORK.TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND WORK.FLAG = ").ApdN(BindPrefix).ApdL("FLAG");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
            paramCollection.Add(iNewParameter.NewDbParameter("FLAG", AR_INZU_IMPORT_FLAG.DELETE_VALUE1));
            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// 員数表一時テーブル削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="tempId">一時取込ID</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelInzuhyoWorkExec(DatabaseHelper dbHelper, string tempId)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE FROM T_AR_GOKI_TEMPWORK");
            sb.ApdN("       WHERE TEMP_ID = ").ApdN(BindPrefix).ApdL("TEMP_ID");

            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParameter.NewDbParameter("TEMP_ID", tempId));
            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection); ;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region A0100050:ＡＲ進捗管理
    
    #region SELECT

    #region 進捗情報検索
    /// --------------------------------------------------
    /// <summary>
    /// 進捗情報検索
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01用検索条件</param>
    /// <returns>データテーブル(T_AR_SHINCHOKU)</returns>
    /// <create>D.Okumura 2019/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetArShinchokuListExec(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("     T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("    ,T_AR_SHINCHOKU.LIST_FLAG");
            sb.ApdL("    ,T_AR_SHINCHOKU.AR_NO");
            sb.ApdL("    ,T_AR.JYOKYO_FLAG");
            sb.ApdL("    ,T_AR_GOKI.KISHU");
            sb.ApdL("    ,T_AR_SHINCHOKU.GOKI");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_SITE_REQ");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_JP");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_LOCAL");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_DATE");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_USER_ID");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_USER_NAME");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_DATE");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_USER_ID");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_USER_NAME");
            sb.ApdL("    ,CONVERT(NCHAR(27), T_AR_SHINCHOKU.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("FROM T_AR_SHINCHOKU");
            sb.ApdL("LEFT JOIN T_AR_GOKI");
            sb.ApdL("    ON  T_AR_GOKI.NONYUSAKI_CD = T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("    AND T_AR_GOKI.GOKI = T_AR_SHINCHOKU.GOKI");
            sb.ApdL("LEFT JOIN T_AR");
            sb.ApdL("    ON  T_AR.NONYUSAKI_CD = T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("    AND T_AR.LIST_FLAG = T_AR_SHINCHOKU.LIST_FLAG");
            sb.ApdL("    AND T_AR.AR_NO = T_AR_SHINCHOKU.AR_NO");
            sb.ApdN("WHERE T_AR_SHINCHOKU.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // AR状態
            if (string.IsNullOrEmpty(cond.JyokyoFlagAR) || JYOKYO_FLAG_AR.ALL_VALUE1.Equals(cond.JyokyoFlagAR))
            {
                // 条件付与なし
            }
            else if (JYOKYO_FLAG_AR.MISHORI_VALUE1.Equals(cond.JyokyoFlagAR))
            {
                sb.ApdN("    AND T_AR.JYOKYO_FLAG <> ").ApdN(this.BindPrefix).ApdL("JOKYO_FLAG_NEGATIVE");
                paramCollection.Add(iNewParam.NewDbParameter("JOKYO_FLAG_NEGATIVE", JYOKYO_FLAG_AR.KANRYO_VALUE1));
            }
            else
            {
                sb.ApdN("    AND T_AR.JYOKYO_FLAG = ").ApdN(this.BindPrefix).ApdL("JOKYO_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("JOKYO_FLAG", cond.JyokyoFlagAR));
            }

            // ARNO(条件)
            SetupConditionArNo(sb, iNewParam, paramCollection, "T_AR_SHINCHOKU.AR_NO", cond.ArNo, cond.SeparatorRange);

            // 機種
            SetupConditionKishu(sb, iNewParam, paramCollection, "T_AR_GOKI.KISHU", cond.Kishu, cond.SeparatorItem);

            // 号機
            SetupConditionGoki(sb, iNewParam, paramCollection, "T_AR_SHINCHOKU.GOKI", cond.Goki, cond.SeparatorItem, cond.SeparatorRange);

            // 日時検索条件
            if (cond.UpdateDateFrom != null || cond.UpdateDateTo != null)
            {
                var dateFrom = cond.UpdateDateFrom;
                var dateTo = cond.UpdateDateTo;
                string[] targetFields = { };
                // 日時検索条件
                if (AR_DATE_KIND_DISP_FLAG.ALL_VALUE1.Equals(cond.DateKubun))
                {
                    targetFields = new[] {
                        "T_AR_SHINCHOKU.DATE_SITE_REQ",
                        "T_AR_SHINCHOKU.DATE_JP",
                        "T_AR_SHINCHOKU.DATE_LOCAL",
                    };
                }
                else if (AR_DATE_KIND_DISP_FLAG.SITE_REQ_VALUE1.Equals(cond.DateKubun))
                {
                    targetFields = new[] {
                        "T_AR_SHINCHOKU.DATE_SITE_REQ",
                    };
                }
                else if (AR_DATE_KIND_DISP_FLAG.LOCAL_VALUE1.Equals(cond.DateKubun))
                {
                    targetFields = new[] {
                        "T_AR_SHINCHOKU.DATE_LOCAL",
                    };
                }
                else if (AR_DATE_KIND_DISP_FLAG.JP_VALUE1.Equals(cond.DateKubun))
                {
                    targetFields = new[] {
                        "T_AR_SHINCHOKU.DATE_JP",
                    };
                }
                if (targetFields.Length > 0)
                {
                    // SQL条件作成
                    sb.ApdL("    AND (");
                    bool isFirst = true;
                    foreach (var field in targetFields)
                    {
                        if (isFirst)
                        {
                            sb.ApdN("       ");
                            isFirst = false;
                        }
                        else
                        {
                            sb.ApdN("    OR ");
                        }
                        sb.ApdN("(");
                        // 日付け未設定は除外する
                        sb.ApdN(field).ApdN(" <> CAST('' as nchar(10))");
                        if (dateFrom != null)
                        {
                            sb.ApdN(" AND ").ApdN(field).ApdN(" >= ").ApdN(this.BindPrefix).ApdN("DATE_START");
                        }
                        if (dateTo != null)
                        {
                            sb.ApdN(" AND ").ApdN(field).ApdN(" <= ").ApdN(this.BindPrefix).ApdL("DATE_END");
                        }
                        sb.ApdL(")");
                    }
                    sb.ApdL("    )");
                    // パラメータ追加
                    if (dateFrom != null)
                    {
                        paramCollection.Add(iNewParam.NewDbParameter("DATE_START", dateFrom.Value.Date.ToString("yyyy/MM/dd")));
                    }
                    if (dateTo != null)
                    {
                        paramCollection.Add(iNewParam.NewDbParameter("DATE_END", dateTo.Value.Date.ToString("yyyy/MM/dd")));
                    }
                }
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

    #endregion

    #endregion //SELECT

    #endregion // A0100050:ＡＲ進捗管理

    #region A0100051:ＡＲ進捗管理日付登録

    #region SELECT

    #region AR情報データ取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="isLock">true:Lockする false:Lockしない</param>
    /// <param name="arList">AR番号リスト</param>
    /// <returns>データテーブル</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockARMultiple(DatabaseHelper dbHelper, CondA1 cond, bool isLock, string[] arList)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , JYOKYO_FLAG");
            sb.ApdL("     , HASSEI_DATE");
            sb.ApdL("     , RENRAKUSHA");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , GOKI");
            sb.ApdL("     , GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdL("     , HUGUAI");
            sb.ApdL("     , TAISAKU");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , GENCHI_TEHAISAKI");
            sb.ApdL("     , GENCHI_SETTEINOKI_DATE");
            sb.ApdL("     , GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdL("     , GENCHI_KOJYOSHUKKA_DATE");
            sb.ApdL("     , SHUKKAHOHO");
            sb.ApdL("     , JP_SETTEINOKI_DATE");
            sb.ApdL("     , JP_SHUKKAYOTEI_DATE");
            sb.ApdL("     , JP_KOJYOSHUKKA_DATE");
            sb.ApdL("     , JP_UNSOKAISHA_NAME");
            sb.ApdL("     , JP_OKURIJYO_NO");
            sb.ApdL("     , GMS_HAKKO_NO");
            sb.ApdL("     , SHIYORENRAKU_NO");
            sb.ApdL("     , TAIO_BUSHO");
            sb.ApdL("     , GIREN_NO_1");
            sb.ApdL("     , GIREN_FILE_1");
            sb.ApdL("     , GIREN_NO_2");
            sb.ApdL("     , GIREN_FILE_2");
            sb.ApdL("     , GIREN_NO_3");
            sb.ApdL("     , GIREN_FILE_3");
            sb.ApdL("     , GIREN_NO_4");
            sb.ApdL("     , GIREN_FILE_4");
            sb.ApdL("     , GIREN_NO_5");
            sb.ApdL("     , GIREN_FILE_5");
            sb.ApdL("     , SHINCHOKU_FLAG");
            sb.ApdL("     , HASSEI_YOUIN");
            sb.ApdL("     , REFERENCE_NO_1");
            sb.ApdL("     , REFERENCE_FILE_1");
            sb.ApdL("     , REFERENCE_NO_2");
            sb.ApdL("     , REFERENCE_FILE_2");
            sb.ApdL("     , LOCK_USER_ID");
            sb.ApdL("     , LOCK_STARTDATE");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR");

            // LOCK
            if (isLock)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 納入先コード
            sb.ApdN("   AND ").ApdN("NONYUSAKI_CD").ApdN(" = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // AR条件設定
            sb.ApdL("   AND (");
            this.SetupConditionArNoList(sb, iNewParam, paramCollection, "LIST_FLAG", "AR_NO", arList);
            sb.ApdL("   )");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_AR.Name);

            return ds.Tables[Def_T_AR.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 進捗情報検索
    /// --------------------------------------------------
    /// <summary>
    /// 進捗情報検索
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01用検索条件</param>
    /// <param name="dtInput">取得対象リスト</param>
    /// <param name="isLock">ロック有無</param>
    /// <returns>データテーブル(T_AR_SHINCHOKU)</returns>
    /// <create>D.Okumura 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetArShinchokuInfoExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dtInput, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("     T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("    ,T_AR_SHINCHOKU.LIST_FLAG");
            sb.ApdL("    ,T_AR_SHINCHOKU.AR_NO");
            sb.ApdL("    ,T_AR_SHINCHOKU.GOKI");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_SITE_REQ");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_JP");
            sb.ApdL("    ,T_AR_SHINCHOKU.DATE_LOCAL");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_DATE");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_USER_ID");
            sb.ApdL("    ,T_AR_SHINCHOKU.CREATE_USER_NAME");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_DATE");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_USER_ID");
            sb.ApdL("    ,T_AR_SHINCHOKU.UPDATE_USER_NAME");
            sb.ApdL("    ,CONVERT(NCHAR(27), T_AR_SHINCHOKU.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
            sb.ApdL("FROM T_AR_SHINCHOKU");

            // LOCK
            if (isLock)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            // 納入先コード
            sb.ApdN("WHERE T_AR_SHINCHOKU.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dtInput, 0, "NONYUSAKI_CD")));

            // レコード取得条件設定
            sb.ApdL("  AND (");
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                DataRow dr = dtInput.Rows[i];
                string arNo = ComFunc.GetFld(dr, "AR_NO");
                if (i == 0)
                    sb.ApdN("    ");
                else
                    sb.ApdN("  OR");

                sb.ApdN(" (T_AR_SHINCHOKU.LIST_FLAG = ").ApdN(this.BindPrefix).ApdF("LIST_FLAG_{0}", i);
                sb.ApdN("  AND T_AR_SHINCHOKU.AR_NO = ").ApdN(this.BindPrefix).ApdF("AR_NO_{0}", i);
                sb.ApdN("  AND T_AR_SHINCHOKU.GOKI = ").ApdN(this.BindPrefix).ApdF("GOKI_{0}", i);
                sb.ApdL("  )");
                
                paramCollection.Add(iNewParam.NewDbParameter(string.Format("LIST_FLAG_{0}", i), arNo.Substring(2, 1)));
                paramCollection.Add(iNewParam.NewDbParameter(string.Format("AR_NO_{0}", i), arNo));
                paramCollection.Add(iNewParam.NewDbParameter(string.Format("GOKI_{0}", i), ComFunc.GetFld(dr, "GOKI")));
            }
            sb.ApdL("  )");


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

    #region 進捗情報最小日付検索
    /// --------------------------------------------------
    /// <summary>
    /// 進捗情報最小日付検索
    /// </summary>
    /// <param name="dbHelper">DBヘルパ</param>
    /// <param name="cond">A01用検索条件</param>
    /// <param name="dtInput">取得対象リスト(T_AR)</param>
    /// <param name="targetField">対象のフィールド名(注意: 未サニタイズ)</param>
    /// <returns>データテーブル(T_AR_SHINCHOKU,日付のみ)</returns>
    /// <create>D.Okumura 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetArShinchokuMinDate(DatabaseHelper dbHelper, CondA1 cond, DataTable dtInput, string targetField)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("     T_AR_SHINCHOKU.NONYUSAKI_CD");
            sb.ApdL("    ,T_AR_SHINCHOKU.LIST_FLAG");
            sb.ApdL("    ,T_AR_SHINCHOKU.AR_NO");
            sb.ApdF("    ,MIN(T_AR_SHINCHOKU.{0}) AS {0}", targetField).ApdL();
            sb.ApdL("FROM T_AR_SHINCHOKU");

            // 納入先コード
            sb.ApdN("WHERE T_AR_SHINCHOKU.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dtInput, 0, "NONYUSAKI_CD")));

            // 日付の入力
            sb.ApdF("  AND T_AR_SHINCHOKU.{0} IS NOT NULL", targetField).ApdL();
            sb.ApdF("  AND T_AR_SHINCHOKU.{0} <> CAST('' as nchar(9))", targetField).ApdL();

            // レコード取得条件設定
            sb.ApdL("  AND (");
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                DataRow dr = dtInput.Rows[i];
                string arNo = ComFunc.GetFld(dr, "AR_NO");
                if (i == 0)
                    sb.ApdN("    ");
                else
                    sb.ApdN("  OR");

                sb.ApdN(" (T_AR_SHINCHOKU.LIST_FLAG = ").ApdN(this.BindPrefix).ApdF("LIST_FLAG_{0}", i);
                sb.ApdN(" AND T_AR_SHINCHOKU.AR_NO = ").ApdN(this.BindPrefix).ApdF("AR_NO_{0}", i);
                sb.ApdL(")");

                paramCollection.Add(iNewParam.NewDbParameter(string.Format("LIST_FLAG_{0}", i), arNo.Substring(2, 1)));
                paramCollection.Add(iNewParam.NewDbParameter(string.Format("AR_NO_{0}", i), arNo));
            }
            sb.ApdL("  )");
            sb.ApdL("GROUP BY T_AR_SHINCHOKU.NONYUSAKI_CD,T_AR_SHINCHOKU.LIST_FLAG,T_AR_SHINCHOKU.AR_NO");

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

    #endregion //SELECT

    #region INSERT

    #region AR進捗履歴データ追加
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗履歴データ追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">AR進捗履歴データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsArShinchokuRirekiExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_AR_SHINCHOKU_RIREKI (");
            sb.ApdL("       NONYUSAKI_CD");
            sb.ApdL("      ,LIST_FLAG");
            sb.ApdL("      ,AR_NO");
            sb.ApdL("      ,GOKI");
            sb.ApdL("      ,DATE_KIND");
            sb.ApdL("      ,DATE_BEFORE");
            sb.ApdL("      ,DATE_AFTER");
            sb.ApdL("      ,NOTE");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("GOKI");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("DATE_KIND");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("DATE_BEFORE");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("DATE_AFTER");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.GOKI)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_KIND", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.DATE_KIND)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_BEFORE", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.DATE_BEFORE)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_AFTER", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.DATE_AFTER)));
                paramCollection.Add(iNewParam.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU_RIREKI.NOTE)));

                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #region AR情報データ履歴追加

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ履歴追加 (AR進捗用)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル(T_AR)</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <param name="gamenFlag">画面種別</param>
    /// <param name="operationFlag">操作種別</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>>D.Okumura 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsRirekiFromAr(DatabaseHelper dbHelper, CondA1 cond, DataTable dt, string bukkenNo, string gamenFlag, string operationFlag)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_RIREKI (");
            sb.ApdL("       GAMEN_FLAG");
            sb.ApdL("      ,SHUKKA_FLAG");
            sb.ApdL("      ,NONYUSAKI_CD");
            sb.ApdL("      ,SHIP");
            sb.ApdL("      ,AR_NO");
            sb.ApdL("      ,BUKKEN_NO");
            sb.ApdL("      ,OPERATION_FLAG");
            sb.ApdL("      ,UPDATE_PC_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_PC_NAME");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            var currentDate = cond.UpdateDate as DateTime? ?? DateTime.Now;
            foreach (DataRow dr in dt.Rows)
            {
            retry:
                try
                {
                    paramCollection = new DbParamCollection();

                    // バインド変数設定
                    paramCollection.Add(iNewParam.NewDbParameter("GAMEN_FLAG", gamenFlag));
                    paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
                    paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR.NONYUSAKI_CD)));
                    paramCollection.Add(iNewParam.NewDbParameter("SHIP", string.Empty));
                    paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.AR_NO)));
                    paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", bukkenNo));
                    paramCollection.Add(iNewParam.NewDbParameter("OPERATION_FLAG", operationFlag));

                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_PC_NAME", cond.LoginInfo.PcName));
                    // 同一の更新日時だと重複するので、クエリ毎に異なる値となるようにする
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", currentDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff")));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                    paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                    // SQL実行
                    record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
                    currentDate = currentDate.AddTicks(1);
                }
                catch (Exception ex)
                {
                    // 重複エラーが発生したときリトライする
                    if (this.IsDbDuplicationError(ex))
                    {
                        currentDate = currentDate.AddTicks(1);
                        goto retry;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion
    #endregion //INSERT

    #region UPDATE

    #region AR情報データ インターロック

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="arList">AR番号リスト</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdArInterLockMultiple(DatabaseHelper dbHelper, CondA1 cond, string[] arList)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("LOCK_USER_ID");
            sb.ApdN("     , LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("LOCK_STARTDATE");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("LOCK_STARTDATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // AR条件設定
            sb.ApdL("   AND (");
            this.SetupConditionArNoList(sb, iNewParam, paramCollection, "LIST_FLAG", "AR_NO", arList);
            sb.ApdL("   )");

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

    #region AR情報データ インターロック解除

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ インターロック解除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">ARデータ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdArInterUnLockMultiple(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("LOCK_USER_ID");
            sb.ApdN("     , LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("LOCK_STARTDATE");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("   AND LOCK_USER_ID = ").ApdN(this.BindPrefix).ApdL("NOW_LOCK_USER_ID");
            sb.ApdN("   AND LOCK_STARTDATE = ").ApdN(this.BindPrefix).ApdL("NOW_LOCK_STARTDATE");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("LOCK_USER_ID", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("LOCK_STARTDATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("NOW_LOCK_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("NOW_LOCK_STARTDATE", ComFunc.GetFldObject(dr, Def_T_AR.LOCK_STARTDATE)));

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


    #region AR情報データ更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データ更新 (AR進捗用)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">データテーブル(T_AR)</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>>D.Okumura 2019/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdArForShinchoku(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR");
            sb.ApdL("SET");
            sb.ApdN("       GENBA_TOTYAKUKIBOU_DATE = ").ApdN(this.BindPrefix).ApdL("GENBA_TOTYAKUKIBOU_DATE");
            sb.ApdN("     , GENCHI_SHUKKAYOTEI_DATE = ").ApdN(this.BindPrefix).ApdL("GENCHI_SHUKKAYOTEI_DATE");
            sb.ApdN("     , JP_SHUKKAYOTEI_DATE = ").ApdN(this.BindPrefix).ApdL("JP_SHUKKAYOTEI_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("GENBA_TOTYAKUKIBOU_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENBA_TOTYAKUKIBOU_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GENCHI_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.GENCHI_SHUKKAYOTEI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JP_SHUKKAYOTEI_DATE", ComFunc.GetFldObject(dr, Def_T_AR.JP_SHUKKAYOTEI_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR.AR_NO)));

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

    #region AR進捗データ更新
    /// --------------------------------------------------
    /// <summary>
    /// AR進捗データ更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <param name="dt">AR進捗データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdArShinchokuExec(DatabaseHelper dbHelper, CondA1 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_AR_SHINCHOKU");
            sb.ApdL("SET");
            sb.ApdN("       DATE_SITE_REQ = ").ApdN(this.BindPrefix).ApdL("DATE_SITE_REQ");
            sb.ApdN("     , DATE_LOCAL = ").ApdN(this.BindPrefix).ApdL("DATE_LOCAL");
            sb.ApdN("     , DATE_JP = ").ApdN(this.BindPrefix).ApdL("DATE_JP");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("   AND GOKI = ").ApdN(this.BindPrefix).ApdL("GOKI");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.LIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.AR_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("GOKI", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.GOKI)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_SITE_REQ", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_SITE_REQ)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_LOCAL", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_LOCAL)));
                paramCollection.Add(iNewParam.NewDbParameter("DATE_JP", ComFunc.GetFldObject(dr, Def_T_AR_SHINCHOKU.DATE_JP)));

                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

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

    #endregion //UPDATE


    #endregion // A0100051:ＡＲ進捗管理日付登録

    #region A0100052:機種ダイアログ

    #region 機種取得

    /// --------------------------------------------------
    /// <summary>
    /// 機種取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション<</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetKishu(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            paramCollection = new DbParamCollection();

            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       KISHU");
            sb.ApdL("FROM");
            sb.ApdL("       T_AR_GOKI");
            sb.ApdL("WHERE");


            // バインド変数設定
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // 機種の絞り込み条件がある場合絞り込む
            this.SetupConditionKishu(sb, iNewParam, paramCollection, "KISHU", cond.Kishu, cond.SeparatorItem);

            sb.ApdL("ORDER BY");
            sb.ApdL("       KISHU");



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

    #region A0100053:号機ダイアログ

    #region 号機取得

    /// --------------------------------------------------
    /// <summary>
    /// 機種取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション<</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetGoki(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_GOKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT ");
            sb.ApdL("       KISHU");
            sb.ApdL("     , GOKI");
            sb.ApdL("FROM");
            sb.ApdL("       T_AR_GOKI");
            sb.ApdL("WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection = new DbParamCollection();
            SetupConditionKishu(sb, iNewParam, paramCollection, "KISHU", cond.Kishu, cond.SeparatorItem);
            sb.ApdL("ORDER BY");
            sb.ApdL("       GOKI");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

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

    #region A0100060:変更履歴

    #region 変更履歴取得

    /// --------------------------------------------------
    /// <summary>
    /// 変更履歴取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">A01用コンディション</param>
    /// <returns>テーブル</returns>
    /// <create>Y.Nakasato 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetRireki(DatabaseHelper dbHelper, CondA1 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_AR_SHINCHOKU_RIREKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;


            sb.ApdL("SELECT");
            sb.ApdL("     T_AR_SHINCHOKU_RIREKI.AR_NO");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.GOKI");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.DATE_KIND");
            sb.ApdL("    ,M_COMMON.ITEM_NAME AS DATE_KIND_NAME");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.DATE_BEFORE");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.DATE_AFTER");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.NOTE");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.UPDATE_DATE");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.UPDATE_USER_ID");
            sb.ApdL("    ,T_AR_SHINCHOKU_RIREKI.UPDATE_USER_NAME");
            sb.ApdL("FROM T_AR_SHINCHOKU_RIREKI");
            sb.ApdL("LEFT JOIN T_AR_GOKI");
            sb.ApdL("    ON  T_AR_GOKI.NONYUSAKI_CD = T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD");
            sb.ApdL("    AND T_AR_GOKI.GOKI = T_AR_SHINCHOKU_RIREKI.GOKI");
            sb.ApdL("LEFT JOIN M_COMMON");
            sb.ApdL("    ON  M_COMMON.VALUE1 = T_AR_SHINCHOKU_RIREKI.DATE_KIND");
            sb.ApdN("    AND M_COMMON.GROUP_CD = '").ApdN(AR_DATE_KIND_FLAG.GROUPCD).ApdL("'");
            sb.ApdN("    AND M_COMMON.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("WHERE T_AR_SHINCHOKU_RIREKI.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // ARNO(条件)
            SetupConditionArNo(sb, iNewParam, paramCollection, "T_AR_SHINCHOKU_RIREKI.AR_NO", cond.ArNo, cond.SeparatorRange);

            // 日付条件
            string dateFlag = cond.DateKubun;
            if (!string.IsNullOrEmpty(dateFlag) && AR_DATE_KIND_DISP_FLAG.ALL_VALUE1 != dateFlag)
            {
                sb.ApdN("    AND T_AR_SHINCHOKU_RIREKI.DATE_KIND = ").ApdN(this.BindPrefix).ApdL("DATE_KIND");
                paramCollection.Add(iNewParam.NewDbParameter("DATE_KIND", dateFlag));
            }

            // 機種
            SetupConditionKishu(sb, iNewParam, paramCollection, "T_AR_GOKI.KISHU", cond.Kishu, cond.SeparatorItem);

            // 号機
            SetupConditionGoki(sb, iNewParam, paramCollection, "T_AR_SHINCHOKU_RIREKI.GOKI", cond.Goki, cond.SeparatorItem, cond.SeparatorRange);

            // 日時検索条件
            if (cond.UpdateDateFrom != null)
            {
                sb.ApdN("    AND T_AR_SHINCHOKU_RIREKI.UPDATE_DATE >= ").ApdN(this.BindPrefix).ApdL("UPDATE_START");
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_START", cond.UpdateDateFrom));
            }
            if (cond.UpdateDateTo != null)
            {
                sb.ApdN("    AND T_AR_SHINCHOKU_RIREKI.UPDATE_DATE < ").ApdN(this.BindPrefix).ApdL("UPDATE_END");
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_END", cond.UpdateDateTo));
            }
            sb.ApdL(" ORDER BY T_AR_SHINCHOKU_RIREKI.UPDATE_DATE DESC");

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

    #endregion // A0100060:変更履歴

    #region AR進捗共通部品

    /// --------------------------------------------------
    /// <summary>
    /// ARNOの検索条件を追加(範囲)
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ生成用</param>
    /// <param name="paramCollection">パラメータ</param>
    /// <param name="field">フィールド名</param>
    /// <param name="arNo">ARNo検索条件</param>
    /// <param name="separatorRange">検索条件区切り文字</param>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetupConditionArNo(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, string field, string arNo, char separatorRange)
    {
        if (string.IsNullOrEmpty(arNo))
        {
        }
        else if (arNo.Contains(separatorRange))
        {
            var arNos = arNo.Split(new[] { separatorRange }, 2);
            sb.ApdN("    AND (");

            sb.ApdN(field).ApdN(" BETWEEN ")
                .ApdN(this.BindPrefix).ApdN("ARNO_START")
                .ApdN(" AND ")
                .ApdN(this.BindPrefix).ApdN("ARNO_END")
                .ApdL(")")
                ;
            paramCollection.Add(iNewParam.NewDbParameter("ARNO_START", "AR" + arNos[0]));
            paramCollection.Add(iNewParam.NewDbParameter("ARNO_END", "AR" + arNos[1]));
        }
        else
        {
            sb.ApdN("    AND ").ApdN(field).ApdN(" = ").ApdN(this.BindPrefix).ApdL("ARNO");
            paramCollection.Add(iNewParam.NewDbParameter("ARNO", "AR" + arNo));
        }

    }
    /// --------------------------------------------------
    /// <summary>
    /// ARNOの検索条件を追加(リスト)
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ生成用</param>
    /// <param name="paramCollection">パラメータ</param>
    /// <param name="fieldArNo">ARNoフィールド名</param>
    /// <param name="fieldListKbn">リスト区分フィールド名</param>
    /// <param name="arNoList">ARNo検索条件</param>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetupConditionArNoList(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, string fieldListKbn, string fieldArNo, string[] arNoList)
    {
        for (var i = 0; i < arNoList.Length; i++)
        {
            string paramName;
            sb.ApdN(i == 0 ? "      " : "   OR ").ApdN("(");
            if (!string.IsNullOrEmpty(fieldListKbn))
            {
                // リスト区分
                paramName = string.Format("LIST_FLAG_{0}", i);
                sb.ApdN("       ").ApdN(fieldListKbn).ApdN(" = ").ApdN(this.BindPrefix).ApdN(paramName);
                paramCollection.Add(iNewParam.NewDbParameter(paramName, arNoList[i].Substring(2, 1)));
            }

            // ARNO
            paramName = string.Format("AR_NO_{0}", i);
            sb.ApdN("   AND ").ApdN(fieldArNo).ApdN(" = ").ApdN(this.BindPrefix).ApdN(paramName);
            paramCollection.Add(iNewParam.NewDbParameter(paramName, arNoList[i]));

            sb.ApdL(")");
        }


    }
    /// --------------------------------------------------
    /// <summary>
    /// 機種の検索条件を追加
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ生成用</param>
    /// <param name="paramCollection">パラメータ</param>
    /// <param name="field">フィールド名</param>
    /// <param name="kishu">機種名</param>
    /// <param name="separatorItem">項目区切り文字</param>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetupConditionKishu(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, string field, string kishu, char separatorItem)
    {
        if (string.IsNullOrEmpty(kishu))
            return;
        int i = 0;
        sb.ApdL("    AND (");
        foreach (var itemKishu in kishu.Split(separatorItem))
        {
            i++;
            if (i != 1)
                sb.ApdN("    OR ");
            else
                sb.ApdN("       ");

            sb.ApdN(field).ApdN(" = ").ApdN(this.BindPrefix).ApdL("KISHU" + i.ToString());
            paramCollection.Add(iNewParam.NewDbParameter("KISHU" + i.ToString(), itemKishu));
        }
        sb.ApdL("    )");
    }
    /// --------------------------------------------------
    /// <summary>
    /// 号機の検索条件を追加
    /// </summary>
    /// <param name="sb">SQL</param>
    /// <param name="iNewParam">パラメータ生成用</param>
    /// <param name="paramCollection">パラメータ</param>
    /// <param name="field">フィールド名</param>
    /// <param name="goki">号機</param>
    /// <param name="separatorItem">項目区切り文字</param>
    /// <param name="separatorRange">範囲区切り文字</param>
    /// <create>D.Okumura 2019/07/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetupConditionGoki(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, string field, string goki, char separatorItem, char separatorRange)
    {
        if (string.IsNullOrEmpty(goki))
            return;
        int i = 0;
        sb.ApdL("    AND (");
        foreach (var itemGoki in goki.Split(separatorItem))
        {
            i++;
            if (i != 1)
                sb.ApdN("    OR ");
            else
                sb.ApdN("       ");
            if (itemGoki.Contains(separatorRange))
            {
                var list = itemGoki.Split(new[] { separatorRange }, 2);
                var lastLen = list[1].Length;
                if (lastLen < list[0].Length)
                {

                    sb.ApdN(field).ApdN(" BETWEEN ")
                        .ApdN(this.BindPrefix).ApdL("GOKI_START_" + i.ToString())
                        .ApdN(" AND ")
                        .ApdN(this.BindPrefix).ApdL("GOKI_END_" + i.ToString());
                    paramCollection.Add(iNewParam.NewDbParameter("GOKI_START_" + i.ToString(), list[0]));
                    paramCollection.Add(iNewParam.NewDbParameter("GOKI_END_" + i.ToString(), list[0].Substring(0, list[0].Length - lastLen) + list[1]));
                }
                else
                {
                    sb.ApdL("1 = 0"); // dummy sql
                }
            }
            else
            {
                sb.ApdN(field).ApdN(" = ").ApdN(this.BindPrefix).ApdL("GOKI" + i.ToString());
                paramCollection.Add(iNewParam.NewDbParameter("GOKI" + i.ToString(), itemGoki));

            }
        }
        sb.ApdL("    )");
    }
    /// --------------------------------------------------
    /// <summary>
    /// 進捗日付フィールドを取得する
    /// </summary>
    /// <param name="flag">日付区分(AR_DATE_KIND_FLAG)</param>
    /// <returns>フィールド名</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private string SetupConditionGetShinchokuDateFieldFromDateKind(string flag)
    {
        string dateField;
        // どのフィールドを表示するかを判断する
        if (AR_DATE_KIND_FLAG.JP_VALUE1.Equals(flag))
            dateField = Def_T_AR_SHINCHOKU.DATE_JP;
        else if (AR_DATE_KIND_FLAG.LOCAL_VALUE1.Equals(flag))
            dateField = Def_T_AR_SHINCHOKU.DATE_LOCAL;
        else if (AR_DATE_KIND_FLAG.SITE_REQ_VALUE1.Equals(flag))
            dateField = Def_T_AR_SHINCHOKU.DATE_SITE_REQ;
        else
            dateField = "";
        return dateField;

    }
    /// --------------------------------------------------
    /// <summary>
    /// AR日付フィールドを取得する
    /// </summary>
    /// <param name="flag">日付区分(AR_DATE_KIND_FLAG)</param>
    /// <returns>フィールド名</returns>
    /// <create>D.Okumura 2019/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private string SetupConditionGetArDateFieldFromDateKind(string flag)
    {
        string dateField;
        // どのフィールドを表示するかを判断する
        if (AR_DATE_KIND_FLAG.JP_VALUE1.Equals(flag))
            dateField = Def_T_AR.JP_SHUKKAYOTEI_DATE;
        else if (AR_DATE_KIND_FLAG.LOCAL_VALUE1.Equals(flag))
            dateField = Def_T_AR.GENCHI_SHUKKAYOTEI_DATE;
        else if (AR_DATE_KIND_FLAG.SITE_REQ_VALUE1.Equals(flag))
            dateField = Def_T_AR.GENBA_TOTYAKUKIBOU_DATE;
        else
            dateField = "";
        return dateField;

    }
    #endregion //AR進捗共通部品

    #endregion //SQL実行

}
