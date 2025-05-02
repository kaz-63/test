using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Commons;
using Condition;
using DSWUtil.DbUtil;
using DSWUtil;
using System.Reflection;
using System.Linq;

/// --------------------------------------------------
/// <summary>
/// マスタ&保守処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsM01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsM01Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region M0100020:担当者マスタ

    #region ユーザーの追加

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メールアドレス重複チェック追加</update>
    /// <update>J.Chen 2024/02/28 計画取込一括設定追加</update>
    /// --------------------------------------------------
    public bool InsUserData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetUser(dbHelper, cond);
            if (ComFunc.IsExistsData(ds, Def_M_USER.Name))
            {
                // 既に登録されているユーザーコードです。
                errMsgID = "M0100020004";
                return false;
            }
            // メールアドレス重複チェック
            if (this.CheckMailAddress(dbHelper, dt))
            {
                // メールアドレスが他ユーザーと重複しています。
                errMsgID = "M0100020011";
                return false;
            }
            // 登録実行
            this.InsUser(dbHelper, cond, dt);

            // 計画取込一括設定が対象の場合、各荷受メールに登録する
            if (ComFunc.GetFld(dt, 0, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG) == MAIL_SHUKKAKEIKAKU_FLAG.TARGET_VALUE1)
            {
                // 荷受メール取得
                var dtMail = this.GetConsignMailHeaderId(dbHelper);
                if (dtMail.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMail.Rows.Count; i++)
                    {
                        var dsMailTmp = new DataSet();
                        string consigCD = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.CONSIGN_CD);
                        string mailHeaderId = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                        // 荷受メールマスタ
                        {
                            var dtMailTmp = new DataTable(Def_M_CONSIGN_MAIL.Name);
                            dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.CONSIGN_CD);
                            dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                            dtMailTmp.Rows.Add(consigCD, mailHeaderId);
                            dsMailTmp.Tables.Add(dtMailTmp);
                        }
                        // 荷受メール明細取得
                        {
                            var dtMailUser = this.GetConsignMailUser(dbHelper, mailHeaderId);

                            // 登録するUSER_ID
                            string userId = ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID);

                            // USER_IDが存在しているかどうかの確認
                            if (dtMailUser.AsEnumerable().Any(row => row.Field<string>(Def_M_USER.USER_ID) == userId))
                            {
                                // USER_IDが存在している場合、下の処理をスキップ
                                continue;
                            }

                            // MAIL_ADDRESS_FLAGが0の行数を取得
                            int countMailAddressFlag = dtMailUser.AsEnumerable()
                                .Count(row => row.Field<string>(Def_M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == MAIL_ADDRESS_FLAG.TO_VALUE1);

                            // USER_IDを荷受メール通知対象に追加
                            dtMailUser.Rows.Add(MAIL_ADDRESS_FLAG.TO_VALUE1, countMailAddressFlag, userId, string.Empty);
                            dsMailTmp.Tables.Add(dtMailUser);
                        }

                        if (!this.SaveKeikakuTorikomiNotify(dbHelper, cond, dsMailTmp, ref errMsgID, ref args))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているユーザーコードです。
                errMsgID = "M0100020004";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ユーザーの更新

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メールアドレス重複チェック追加</update>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// <update>J.Chen 2024/01/31 計画取込一括設定処理追加</update>
    /// --------------------------------------------------
    public bool UpdUserData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetUser(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_USER.Name))
            {
                // 既に削除されたユーザーコードです。
                errMsgID = "M0100020005";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_USER.Name], out notFoundIndex, Def_M_USER.VERSION, Def_M_USER.USER_ID);
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

            // 修正前のユーザーが最低1人は残しておかなければいけないユーザーか確認
            if (ComFunc.GetFld(ds, Def_M_USER.Name, 0, Def_M_ROLE.USER_DELETE_FLAG) == USER_DELETE_FLAG.LIMITONE_VALUE1)
            {
                CondM01 condRole = new CondM01(cond.LoginInfo);
                condRole.RoleID = ComFunc.GetFld(dt, 0, Def_M_USER.ROLE_ID);
                condRole.LoginInfo = cond.LoginInfo;
                DataTable dtRole = this.GetRole(dbHelper, condRole);
                if (ComFunc.GetFld(dtRole, 0, Def_M_ROLE.USER_DELETE_FLAG, USER_DELETE_FLAG.FREE_VALUE1) == USER_DELETE_FLAG.FREE_VALUE1)
                {
                    if (this.LockRemoveRestrictionsUserCount(dbHelper, cond) == 0)
                    {
                        // 他にシステム管理者権限の担当者がいない為、修正できません。
                        errMsgID = "M0100020008";
                        return false;
                    }
                }
            }
            // メールアドレス重複チェック
            if (this.CheckMailAddress(dbHelper, dt))
            {
                // メールアドレスが他ユーザーと重複しています。
                errMsgID = "M0100020011";
                return false;
            }

            // 更新実行
            this.UpdUser(dbHelper, cond, dt);

            // 計画取込一括設定に変更がある場合
            var beforeShukkaMail = ComFunc.GetFld(ds, Def_M_USER.Name, 0, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG);
            var afterShukkaMail = ComFunc.GetFld(dt, 0, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG);
            if (beforeShukkaMail != afterShukkaMail)
            {
                // 荷受メール取得
                var dtMail = this.GetConsignMailHeaderId(dbHelper);
                if (dtMail.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMail.Rows.Count; i++)
                    {
                        var dsMailTmp = new DataSet();
                        string consigCD = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.CONSIGN_CD);
                        string mailHeaderId = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                        // 荷受メールマスタ
                        {
                            var dtMailTmp = new DataTable(Def_M_CONSIGN_MAIL.Name);
                            dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.CONSIGN_CD);
                            dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                            dtMailTmp.Rows.Add(consigCD, mailHeaderId);
                            dsMailTmp.Tables.Add(dtMailTmp);
                        }
                        // 荷受メール明細取得
                        {
                            var dtMailUser = this.GetConsignMailUser(dbHelper, mailHeaderId);

                            // 登録するUSER_ID
                            string userId = ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID);

                            // 対象を選んだ場合
                            if (afterShukkaMail == MAIL_SHUKKAKEIKAKU_FLAG.TARGET_VALUE1)
                            {
                                // USER_IDが存在しているかどうかの確認
                                if (dtMailUser.AsEnumerable().Any(row => row.Field<string>(Def_M_USER.USER_ID) == userId))
                                {
                                    // USER_IDが存在している場合、下の処理をスキップ
                                    continue;
                                }

                                // MAIL_ADDRESS_FLAGが0の行数を取得
                                int countMailAddressFlag = dtMailUser.AsEnumerable()
                                    .Count(row => row.Field<string>(Def_M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG) == MAIL_ADDRESS_FLAG.TO_VALUE1);

                                // USER_IDを荷受メール通知対象に追加
                                dtMailUser.Rows.Add(MAIL_ADDRESS_FLAG.TO_VALUE1, countMailAddressFlag, userId, string.Empty);
                                dsMailTmp.Tables.Add(dtMailUser);
                            }
                            // 対象外を選んだ場合
                            else if (afterShukkaMail == MAIL_SHUKKAKEIKAKU_FLAG.NOT_TARGET_VALUE1)
                            {
                                // USER_IDが存在しているかどうかの確認
                                if (dtMailUser.AsEnumerable().Any(row => row.Field<string>(Def_M_USER.USER_ID) == userId))
                                {

                                    // 一行だけの場合、そのまま削除
                                    if (dtMailUser.Rows.Count == 1)
                                    {
                                        var mailHeaderIdDel = ComFunc.GetFld(dsMailTmp, Def_M_CONSIGN_MAIL.Name, 0, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);
                                        this.DelConsignMailMeisai(dbHelper, mailHeaderIdDel);
                                        this.DelConsignMail(dbHelper, mailHeaderIdDel);
                                        continue;
                                    }
                                    else
                                    {
                                        // 処理するUSER_IDのある行を削除
                                        dtMailUser = dtMailUser.AsEnumerable()
                                            .Where(row => row.Field<string>(Def_M_USER.USER_ID) != userId)
                                            .CopyToDataTable();
                                        dtMailUser.TableName = Def_M_CONSIGN_MAIL_MEISAI.Name;

                                        // ORDER_NOを振り直し
                                        var groups = dtMailUser.AsEnumerable().GroupBy(row => row.Field<string>(Def_M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG));

                                        foreach (var group in groups)
                                        {
                                            int orderNo = 0;
                                            foreach (var row in group)
                                            {
                                                row.SetField(Def_M_CONSIGN_MAIL_MEISAI.ORDER_NO, orderNo++);
                                            }
                                        }
                                        dsMailTmp.Tables.Add(dtMailUser);
                                    }
                                }
                                else
                                {
                                    // USER_IDが存在しない場合、スキップ
                                    continue;
                                }

                            }
                            else
                            {
                                // 対象と対象外以外の場合、スキップ（念のため）
                                continue;
                            }
                        }

                        if (!this.SaveKeikakuTorikomiNotify(dbHelper, cond, dsMailTmp, ref errMsgID, ref args))
                        {
                            return false;
                        }
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

    #region ユーザーの削除

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="ds">複数エラーの場合はtrue</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>H.Tsuji 2019/06/24 メール送信対象者の削除チェック</update>
    /// <update>K.Tsutsumi 2019/07/23 担当者マスタ内で設定する通知設定はチェックしなくても良い</update>
    /// <update>D.Okumura 2019/08/07 AR進捗通知対応</update>
    /// <update>J.Chen 2024/03/04 計画取込Mail通知先の削除処理追加</update>
    /// --------------------------------------------------
    public bool DelUserData(DatabaseHelper dbHelper, CondM01 cond, ref DataSet ds, ref bool isMultiError)
    {
        try
        {
            DataTable dtDispUser = ds.Tables[Def_M_USER.Name];
            DataTable dtMessage = ds.Tables[ComDefine.DTTBL_MULTIMESSAGE];

            // 存在チェック
            DataSet dsUser = this.GetUser(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsUser, Def_M_USER.Name))
            {
                // 既に削除されたユーザーコードです。
                ComFunc.AddMultiMessage(dtMessage, "M0100020005");
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dtDispUser, dsUser.Tables[Def_M_USER.Name], out notFoundIndex, Def_M_USER.VERSION, Def_M_USER.USER_ID);
            if (0 <= index)
            {
                // 他端末で更新された為、更新できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "A9999999027");
                return false;
            }
            else if (notFoundIndex != null)
            {
                // 他端末で更新された為、更新できませんでした。
                ComFunc.AddMultiMessage(dtMessage, "A9999999027");
                return false;
            }

            // ユーザーが最低1人は残しておかなければいけないユーザーか確認
            if (ComFunc.GetFld(dsUser, Def_M_USER.Name, 0, Def_M_ROLE.USER_DELETE_FLAG) == USER_DELETE_FLAG.LIMITONE_VALUE1)
            {
                if (this.LockRemoveRestrictionsUserCount(dbHelper, cond) == 0)
                {
                    // 他にシステム管理者権限の担当者がいない為、削除できません。
                    ComFunc.AddMultiMessage(dtMessage, "M0100020009");
                    return false;
                }
            }

            // 物件メール明細マスタのユーザーIDをチェック(メール送信対象者は担当者削除させない)
            DataTable dtBukkenMail = this.GetBukkenMail(dbHelper, cond);
            if (dtBukkenMail.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBukkenMail.Rows)
                {
                    string mailKbn = dr[Def_M_BUKKEN_MAIL.MAIL_KBN].ToString();
                    string mailAddressFlag = dr[ComDefine.FLD_MAIL_ADDRESS_FLAG_NAME].ToString();
                    string bukkenName = dr[Def_M_BUKKEN.BUKKEN_NAME].ToString();
                    string listKbn = dr[ComDefine.FLD_LIST_FLAG_NAME].ToString();
                    if (mailKbn == MAIL_KBN.BUKKEN_VALUE1)
                    {
                        // 物件追加通知に登録されています。【宛先入力欄：{0}】
                        ComFunc.AddMultiMessage(dtMessage, "M0100020017", mailAddressFlag);
                    }
                    if (mailKbn == MAIL_KBN.COMMON_VALUE1)
                    {
                        // 共通通知に登録されています。【物件名：{0}、宛先入力欄：{1}】
                        ComFunc.AddMultiMessage(dtMessage, "M0100020018", bukkenName, mailAddressFlag);
                    }
                    if (mailKbn == MAIL_KBN.ARLIST_VALUE1)
                    {
                        // AR List単位通知に登録されています。【物件名：{0}、List区分：{1}、宛先入力欄：{2}】
                        ComFunc.AddMultiMessage(dtMessage, "M0100020019", bukkenName, listKbn, mailAddressFlag);
                    }
                    if (mailKbn == MAIL_KBN.ARSHINCHOKU_VALUE1)
                    {
                        // 進捗管理通知設定(Default)に登録されています。【宛先入力欄：{0}】
                        ComFunc.AddMultiMessage(dtMessage, "M0100020021",  mailAddressFlag);
                    }
                    if (mailKbn == MAIL_KBN.ARSHINCHOKU_KOBETU_VALUE1)
                    {
                        //  進捗管理通知設定に登録されています。【物件名：{0}、宛先入力欄：{1}】
                        ComFunc.AddMultiMessage(dtMessage, "M0100020022", bukkenName, mailAddressFlag);
                    }
                }
                isMultiError = true;
            }

            //// 担当者マスタの荷姿表送信対象をチェック(メール送信対象者は担当者削除させない)
            //if (ComFunc.GetFld(dsUser, Def_M_USER.Name, 0, Def_M_USER.MAIL_PACKING_FLAG) == MAIL_PACKING_FLAG.TARGET_VALUE1)
            //{
            //    // 荷姿表送信対象者として登録されています。担当者マスタ画面から変更して下さい。
            //    ComFunc.AddMultiMessage(dtMessage, "M0100020020");
            //    isMultiError = true;
            //}


            // 荷受メールマスタから該当ユーザーを削除
            var dtMail = this.GetConsignMailHeaderId(dbHelper);
            if (dtMail.Rows.Count > 0)
            {
                for (int i = 0; i < dtMail.Rows.Count; i++)
                {
                    var dsMailTmp = new DataSet();
                    string consigCD = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.CONSIGN_CD);
                    string mailHeaderId = ComFunc.GetFld(dtMail, i, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                    // 荷受メールマスタ
                    {
                        var dtMailTmp = new DataTable(Def_M_CONSIGN_MAIL.Name);
                        dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.CONSIGN_CD);
                        dtMailTmp.Columns.Add(Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);

                        dtMailTmp.Rows.Add(consigCD, mailHeaderId);
                        dsMailTmp.Tables.Add(dtMailTmp);
                    }
                    // 荷受メール明細取得
                    {
                        var dtMailUser = this.GetConsignMailUser(dbHelper, mailHeaderId);

                        // 削除するUSER_ID
                        string userId = cond.UserID;

                        // USER_IDが存在しているかどうかの確認
                        if (dtMailUser.AsEnumerable().Any(row => row.Field<string>(Def_M_USER.USER_ID) == userId))
                        {

                            // 一行だけの場合、そのまま削除
                            if (dtMailUser.Rows.Count == 1)
                            {
                                var mailHeaderIdDel = ComFunc.GetFld(dsMailTmp, Def_M_CONSIGN_MAIL.Name, 0, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);
                                this.DelConsignMailMeisai(dbHelper, mailHeaderIdDel);
                                this.DelConsignMail(dbHelper, mailHeaderIdDel);
                                continue;
                            }
                            else
                            {
                                // 処理するUSER_IDのある行を削除
                                dtMailUser = dtMailUser.AsEnumerable()
                                    .Where(row => row.Field<string>(Def_M_USER.USER_ID) != userId)
                                    .CopyToDataTable();
                                dtMailUser.TableName = Def_M_CONSIGN_MAIL_MEISAI.Name;

                                // ORDER_NOを振り直し
                                var groups = dtMailUser.AsEnumerable().GroupBy(row => row.Field<string>(Def_M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG));

                                foreach (var group in groups)
                                {
                                    int orderNo = 0;
                                    foreach (var row in group)
                                    {
                                        row.SetField(Def_M_CONSIGN_MAIL_MEISAI.ORDER_NO, orderNo++);
                                    }
                                }
                                dsMailTmp.Tables.Add(dtMailUser);
                            }
                        }
                        else
                        {
                            // USER_IDが存在しない場合、スキップ
                            continue;
                        }
                    }

                    string errMsgID = null;
                    string[] args = null;
                    if (!this.SaveKeikakuTorikomiNotify(dbHelper, cond, dsMailTmp, ref errMsgID, ref args))
                    {
                        return false;
                    }

                }
            }


            // 複数エラーメッセージがある場合はここで終了する
            if (isMultiError)
            {
                return false;
            }

            // 削除実行
            this.DelUser(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メールアドレス重複チェック

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレス重複チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">データ</param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckMailAddress(DatabaseHelper dbHelper, DataTable dt)
    {
        // メールアドレス未入力ならOK
        if (string.IsNullOrEmpty(ComFunc.GetFld(dt, 0, Def_M_USER.MAIL_ADDRESS)))
        {
            return false;
        }
        return this.DuplicateMailAddress(dbHelper, dt);
    }

    #endregion

    #endregion

    #region M0100040:納入先保守

    #region 納入先の登録

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsNonyusakiData(DatabaseHelper dbHelper, CondM01 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetNonyusaki(dbHelper, cond);
            if (0 < ComFunc.GetFldToInt32(ds, Def_M_NONYUSAKI.Name, 0, ComDefine.FLD_CNT))
            {
                // 既に登録されている納入先です。
                errMsgID = "M0100040009";
                return false;
            }
            var condNon = new CondNonyusaki(cond.LoginInfo);
            // 納入先コードの採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.US_VALUE1;
            using (var impl = new WsSmsImpl())
            {
                string nonyusakiCD;
                if (!impl.GetSaiban(dbHelper, condSms, out nonyusakiCD, out errMsgID))
                {
                    return false;
                }
                condNon.NonyusakiCD = nonyusakiCD;
            }
            condNon.ShukkaFlag = cond.ShukkaFlag;
            condNon.BukkenNo = cond.BukkenNo;
            condNon.NonyusakiName = cond.NonyusakiName;
            condNon.Ship = cond.Ship;
            condNon.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;
            condNon.CreateUserID = cond.LoginInfo.UserID;
            condNon.CreateUserName = cond.LoginInfo.UserName;
            condNon.UpdateUserID = cond.LoginInfo.UserID;
            condNon.UpdateUserName = cond.LoginInfo.UserName;
            condNon.MainteDate = DateTime.Now;
            condNon.MainteUserID = cond.LoginInfo.UserID;
            condNon.MainteUserName = cond.LoginInfo.UserName;

            // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
            //　出荷元追加により項目追加
            condNon.ShipFromCD = cond.ShipFromCd;

            using (var impl = new WsMasterImpl())
            {
                impl.InsNonyusaki(dbHelper, condNon);
            }
            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている納入先です。
                errMsgID = "M0100040009";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 納入先の更新

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdNonyusakiData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            using (WsMasterImpl impl = new WsMasterImpl())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CondNonyusaki condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                    condNonyusaki.LoginInfo = cond.LoginInfo.Clone();
                    // 納入先マスタ用コンディションに検索用の値をセット
                    condNonyusaki.ShukkaFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHUKKA_FLAG);
                    condNonyusaki.NonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    // 存在チェック
                    DataSet ds = impl.GetNonyusaki(dbHelper, condNonyusaki);
                    if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                    {
                        // 他端末で削除されています。
                        errMsgID = "A9999999026";
                        return false;
                    }
                    // バージョンチェック
                    int index;
                    int[] notFoundIndex;
                    // @@@ 2011/02/16 M.Tsutsumi Change 間違い発見！
                    //index = this.CheckSameData(ds, ds.Tables[Def_M_NONYUSAKI.Name], out notFoundIndex, Def_M_USER.VERSION, Def_M_USER.USER_ID);
                    index = this.CheckSameData(dt, ds.Tables[Def_M_NONYUSAKI.Name], out notFoundIndex, Def_M_NONYUSAKI.VERSION, Def_M_NONYUSAKI.SHUKKA_FLAG, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    // @@@ ↑
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

                    //@@@ 2011/02/24 M.Tsutsumi Add 
                    // リスト区分使用チェック
                    if (condNonyusaki.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                    {
                        if (!this.CheckListFlag(dbHelper, dr, ref errMsgID, ref args))
                        {
                            return false;
                        }
                    }
                    // @@@ ↑

                    // 納入先マスタ用コンディションに更新用の値をセット
                    condNonyusaki.NonyusakiName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    condNonyusaki.Ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    condNonyusaki.KanriFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.KANRI_FLAG);

                    // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
                    //　コンボボックスの設定値
                    if (condNonyusaki.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                        //ARの場合は、値 NULLを設定
                        condNonyusaki.ShipFromCD = null;
                    else
                        //本体の場合は、コンボボックスの選択値を設定
                        condNonyusaki.ShipFromCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_FROM_CD);
                    //----------（修正ここまで）

                    //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.36
                    condNonyusaki.RemoveFlag = REMOVE_FLAG.NORMAL_VALUE1;
                    if (condNonyusaki.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                    {
                        condNonyusaki.ListFlagName0 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
                        condNonyusaki.ListFlagName1 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
                        condNonyusaki.ListFlagName2 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
                        condNonyusaki.ListFlagName3 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
                        condNonyusaki.ListFlagName4 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
                        condNonyusaki.ListFlagName5 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
                        condNonyusaki.ListFlagName6 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
                        condNonyusaki.ListFlagName7 = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
                    }
                    // @@@ ↑
                    condNonyusaki.MainteDate = DateTime.Now;
                    condNonyusaki.MainteUserID = cond.LoginInfo.UserID;
                    condNonyusaki.MainteUserName = cond.LoginInfo.UserName;
                    // 更新実行
                    impl.UpdNonyusaki(dbHelper, condNonyusaki);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている納入先です。
                errMsgID = "M0100040009";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #region リスト区分のチェック

    /// --------------------------------------------------
    /// <summary>
    /// リスト区分のチェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="dr">対象データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>M.Tsutsumi 2011/03/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckListFlag(DatabaseHelper dbHelper, DataRow dr, ref string errMsgID, ref string[] args)
    {
        try
        {
            string nonyusakiCd = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
            string listFlagName;
            string errListFlag = string.Empty;
            bool ret = true;

            // リスト区分０
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME0);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_0_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_0_VALUE1 + "]";
                }
            }

            // リスト区分１
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME1);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_1_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_1_VALUE1 + "]";
                }
            }

            // リスト区分２
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME2);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_2_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_2_VALUE1 + "]";
                }
            }

            // リスト区分３
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME3);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_3_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_3_VALUE1 + "]";
                }
            }

            // リスト区分４
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME4);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_4_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_4_VALUE1 + "]";
                }
            }

            // リスト区分５
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME5);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_5_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_5_VALUE1 + "]";
                }
            }

            // リスト区分６
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME6);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_6_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_6_VALUE1 + "]";
                }
            }

            // リスト区分７
            listFlagName = ComFunc.GetFld(dr, Def_M_NONYUSAKI.LIST_FLAG_NAME7);
            if (string.IsNullOrEmpty(listFlagName))
            {
                if (!this.CheckListFlagExec(dbHelper, nonyusakiCd, LIST_FLAG.FLAG_7_VALUE1))
                {
                    ret = false;
                    // メッセージ作成
                    errListFlag = errListFlag + "[" + LIST_FLAG.FLAG_7_VALUE1 + "]";
                }
            }

            if (!ret)
            {
                // {0}行目のパレットNo.[{1}]は他の木枠に梱包済です。
                // 使用中のリスト区分の名称は削除できません。【リスト区分：{0}】
                errMsgID = "M0100040007";
                args = new string[] { errListFlag };
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
    /// リスト区分のチェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="listFlag">リスト区分</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>M.Tsutsumi 2011/03/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckListFlagExec(DatabaseHelper dbHelper, string nonyusakiCd, string listFlag)
    {
        try
        {
            DataSet ds = this.GetARListFlagCount(dbHelper, nonyusakiCd, listFlag);
            if (!ComFunc.IsExistsData(ds, Def_T_AR.Name))
            {
                return false;
            }

            int count = ComFunc.GetFldToInt32(ds, Def_T_AR.Name, 0, ComDefine.FLD_CNT);
            if (0 < count)
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

    #endregion

    #region 納入先の削除

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の削除( = 除外フラグ更新) (Step2 No.37)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>M.Tsutsumi 2011/02/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelNonyusakiData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            using (WsMasterImpl impl = new WsMasterImpl())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CondNonyusaki condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                    condNonyusaki.LoginInfo = cond.LoginInfo.Clone();
                    // 納入先マスタ用コンディションに検索用の値をセット
                    condNonyusaki.ShukkaFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHUKKA_FLAG);
                    condNonyusaki.NonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    // 存在チェック
                    DataSet ds = impl.GetNonyusaki(dbHelper, condNonyusaki);
                    if (!ComFunc.IsExistsData(ds, Def_M_NONYUSAKI.Name))
                    {
                        // 他端末で削除されています。
                        errMsgID = "A9999999026";
                        return false;
                    }
                    // バージョンチェック
                    int index;
                    int[] notFoundIndex;
                    index = this.CheckSameData(dt, ds.Tables[Def_M_NONYUSAKI.Name], out notFoundIndex, Def_M_NONYUSAKI.VERSION, Def_M_NONYUSAKI.SHUKKA_FLAG, Def_M_NONYUSAKI.NONYUSAKI_CD);
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

                    // 納入先マスタ用コンディションに更新用の値をセット
                    condNonyusaki.KanriFlag = KANRI_FLAG.KANRYO_VALUE1;
                    condNonyusaki.RemoveFlag = REMOVE_FLAG.JYOGAI_VALUE1;
                    condNonyusaki.MainteDate = DateTime.Now;
                    condNonyusaki.MainteUserID = cond.LoginInfo.UserID;
                    condNonyusaki.MainteUserName = cond.LoginInfo.UserName;
                    // 更新実行
                    impl.UpdNonyusaki(dbHelper, condNonyusaki);
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

    #endregion

    #region M0100050:梱包情報保守

    #region Box梱包

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxData(DatabaseHelper dbHelper, CondM01 cond, ref string errMsgID, ref string[] args)
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
                // 出荷日チェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_SHUKKA_CNT))
                {
                    // 出荷済です。
                    errMsgID = "A9999999031";
                    return null;
                }

                // パレット梱包チェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_PALLET_NO_CNT))
                {
                    // パレット梱包済です。解除できません。
                    errMsgID = "M0100050004";
                    return null;
                }
            }

            DataSet ds = this.GetBoxDataExec(dbHelper, cond);

            // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
            ds.Tables.Add(this.GetAndLockBoxListManage(dbHelper, cond, false));
            // ↑

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット梱包

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPalletData(DatabaseHelper dbHelper, CondM01 cond, ref string errMsgID, ref string[] args)
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
            foreach (DataRow dr in dtCheck.Rows)
            {
                // 出荷日チェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_SHUKKA_CNT))
                {
                    // 出荷済です。
                    errMsgID = "A9999999031";
                    return null;
                }

                // 木枠梱包チェック
                if (0 < ComFunc.GetFldToInt32(dr, ComDefine.FLD_KOJI_NO_CNT))
                {
                    // 木枠梱包済です。解除できません。
                    errMsgID = "M0100050007";
                    return null;
                }
            }

            DataSet ds = this.GetPalletDataExec(dbHelper, cond);

            // 2012/05/18 K.Tsutsumi Add パレットリスト管理データも取得
            ds.Tables.Add(this.GetAndLockPalletListManage(dbHelper, cond, false));
            // ↑

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Box梱包更新

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dtManage">ボックスリスト管理データ</param>
    /// <param name="ds">更新データ</param>
    /// <param name="dtKonpo">追加梱包データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
    //public bool UpdBoxData(DatabaseHelper dbHelper, CondM01 cond, DataTable ds, DataTable dtKonpo, ref string errMsgID, ref string[] args)
    public bool UpdBoxData(DatabaseHelper dbHelper, CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, ref string errMsgID, ref string[] args)
    // ↑
    {
        try
        {
            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            DateTime syoriDate = DateTime.Now;
            // @@@ ↑

            // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
            DataTable dtBoxManage = this.GetAndLockBoxListManage(dbHelper, cond, true);
            if ((dtBoxManage == null) || (dtBoxManage.Rows.Count == 0))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            else
            {
                int[] notFoundIndex;
                if (0 <= this.CheckSameData(dtManage, dtBoxManage, out notFoundIndex, Def_T_BOXLIST_MANAGE.VERSION, Def_T_BOXLIST_MANAGE.BOX_NO))
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
            // ↑

            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            if (dt.Rows.Count > 0)
            {
                // @@@ ↑
                DataSet ds = this.GetBoxData(dbHelper, cond, ref errMsgID, ref args);
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

                // 梱包解除用コンディション作成
                CondM01 condKaijyo = (CondM01)cond.Clone();
                condKaijyo.JyotaiFlag = JYOTAI_FLAG.SHUKAZUMI_VALUE1;
                // @@@ 2011/03/07 M.Tsutsumi Change No.44
                //condKaijyo.UpdateDate = DateTime.Now;
                condKaijyo.UpdateDate = syoriDate;
                // @@@ ↑

                // 集荷済に更新
                this.UpdShukkaMeisaiBoxKaijyo(dbHelper, condKaijyo, dt);
                //@@@ 2011/03/07 M.Tsutsumi Add No.44
            }
            // @@@ ↑

            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            if (dtKonpo.Rows.Count > 0)
            {
                DataTable dtCheck = dtKonpo.Clone();
                foreach (DataRow dr in dtKonpo.Rows)
                {
                    CondM01 checkCond = new CondM01(cond.LoginInfo);
                    checkCond.ShukkaFlag = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                    checkCond.NonyusakiCD = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                    checkCond.TagNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO);

                    DataTable dtWk = this.GetShukkaMeisaiBoxKonpoAdd(dbHelper, checkCond);
                    if (dtWk == null || dtWk.Rows.Count <= 0)
                    {
                        // 他端末で削除されています。
                        errMsgID = "A9999999026";
                        return false;
                    }
                    dtCheck.Merge(dtWk);
                }
                int[] notFoundIndex;
                if (0 <= this.CheckSameData(dtKonpo, dtCheck, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, Def_T_SHUKKA_MEISAI.TAG_NO) || notFoundIndex != null)
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

                // 梱包追加用コンディション作成
                CondM01 condKonpo = (CondM01)cond.Clone();
                condKonpo.JyotaiFlag = JYOTAI_FLAG.BOXZUMI_VALUE1;
                condKonpo.UpdateDate = syoriDate;

                // BOX梱包済に更新
                this.UpdShukkaMeisaiBoxKonpo(dbHelper, condKonpo, dtKonpo);
            }
            // @@@ ↑

            // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
            this.UpdBoxListManage(dbHelper, cond);
            // ↑

            // 出荷明細データのデータ数を取得
            int dataCnt = this.GetBoxDataMeisaiCount(dbHelper, cond);
            // 出荷明細データが0件であればBoxリスト管理データを削除
            if (dataCnt == 0)
            {
                // 全て出荷
                this.DelBoxListManage(dbHelper, cond);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット梱包更新

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dtManage">パレットリスト管理データ</param>
    /// <param name="ds">更新データ</param>
    /// <param name="dtKonpo">追加梱包データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
    //public bool UpdPalletData(DatabaseHelper dbHelper, CondM01 cond, DataTable ds, DataTable dtKonpo, ref string errMsgID, ref string[] args)
    public bool UpdPalletData(DatabaseHelper dbHelper, CondM01 cond, DataTable dtManage, DataTable dt, DataTable dtKonpo, ref string errMsgID, ref string[] args)
    // ↑
    {
        try
        {
            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            DateTime syoriDate = DateTime.Now;
            // @@@ ↑

            // 2012/05/18 K.Tsutsumi Change 管理データのバージョンチェック
            DataTable dtPalletManage = this.GetAndLockPalletListManage(dbHelper, cond, true);
            if ((dtPalletManage == null) || (dtPalletManage.Rows.Count == 0))
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            else
            {
                int[] notFoundIndex;
                if (0 <= this.CheckSameData(dtManage, dtPalletManage, out notFoundIndex, Def_T_PALLETLIST_MANAGE.VERSION, Def_T_PALLETLIST_MANAGE.PALLET_NO))
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
            // ↑

            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            if (dt.Rows.Count > 0)
            {
                // @@@ ↑
                DataSet ds = this.GetPalletData(dbHelper, cond, ref errMsgID, ref args);
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

                // 梱包解除用コンディション作成
                CondM01 condKaijyo = (CondM01)cond.Clone();
                condKaijyo.JyotaiFlag = JYOTAI_FLAG.BOXZUMI_VALUE1;
                // @@@ 2011/03/07 M.Tsutsumi Change No.44
                //condKaijyo.UpdateDate = DateTime.Now;
                condKaijyo.UpdateDate = syoriDate;
                // @@@ ↑

                // Box梱包済に更新
                this.UpdShukkaMeisaiPalletKaijyo(dbHelper, condKaijyo, dt);
                //@@@ 2011/03/07 M.Tsutsumi Add No.44
            }
            // @@@ ↑

            //@@@ 2011/03/07 M.Tsutsumi Add No.44
            if (dtKonpo.Rows.Count > 0)
            {
                DataTable dtCheck = dtKonpo.Clone();
                foreach (DataRow dr in dtKonpo.Rows)
                {
                    CondM01 checkCond = new CondM01(cond.LoginInfo);
                    checkCond.ShukkaFlag = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                    checkCond.NonyusakiCD = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                    checkCond.BoxNo = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO);

                    DataTable dtWk = this.GetShukkaMeisaiPalletKonpoAdd(dbHelper, checkCond);
                    if (dtWk == null || dtWk.Rows.Count <= 0)
                    {
                        // 他端末で削除されています。
                        errMsgID = "A9999999026";
                        return false;
                    }
                    dtCheck.Merge(dtWk);
                }
                int[] notFoundIndex;
                if (0 <= this.CheckSameData(dtKonpo, dtCheck, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, Def_T_SHUKKA_MEISAI.BOX_NO) || notFoundIndex != null)
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

                // 梱包追加用コンディション作成
                CondM01 condKonpo = (CondM01)cond.Clone();
                condKonpo.JyotaiFlag = JYOTAI_FLAG.PALLETZUMI_VALUE1;
                condKonpo.UpdateDate = syoriDate;

                // パレット梱包済に更新
                this.UpdShukkaMeisaiPalletKonpo(dbHelper, condKonpo, dtKonpo);
            }
            // @@@ ↑

            // 2012/05/18 K.Tsutsumi Add 管理データのバージョンチェック
            this.UpdPalletListManage(dbHelper, cond);
            // ↑

            // 出荷明細データのデータ数を取得
            int dataCnt = this.GetPalletDataMeisaiCount(dbHelper, cond);
            // 出荷明細データが0件であればパレットリスト管理データを削除
            if (dataCnt == 0)
            {
                // 全て出荷
                this.DelPalletListManage(dbHelper, cond);
            }
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Box梱包(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するBox梱包データを取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxDataAdd(DatabaseHelper dbHelper, CondM01 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // Boxリスト管理データ取得
            DataTable dt = this.GetBoxListManage(dbHelper, cond.KonpoNo);

            // データチェック
            if (dt.Rows.Count < 1)
            {
                // 該当BoxNo.はありません。
                errMsgID = "A9999999023";
                return null;
            }

            cond.ShukkaFlag = ComFunc.GetFld(dt, 0, Def_T_BOXLIST_MANAGE.SHUKKA_FLAG);
            cond.NonyusakiCD = ComFunc.GetFld(dt, 0, Def_T_BOXLIST_MANAGE.NONYUSAKI_CD);

            // Tag情報取得
            DataSet ds = this.GetBoxDataAddExec(dbHelper, cond);

            if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
            {
                // 登録されていないTagNo.です。
                errMsgID = "M0100050012";
                return null;
            }

            string shukaDate = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKA_DATE);
            if (string.IsNullOrEmpty(shukaDate))
            {
                // 未集荷のTagNo.です。
                errMsgID = "M0100050013";
                return null;
            }

            string boxkonpoDate = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.BOXKONPO_DATE);
            if (!string.IsNullOrEmpty(boxkonpoDate))
            {
                // 既にBox梱包済みのTagNo.です。
                errMsgID = "M0100050014";
                return null;
            }

            string kiwakukonpoDate = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE);
            if (!string.IsNullOrEmpty(kiwakukonpoDate))
            {
                // 既に木枠梱包済みのTagNo.です。
                errMsgID = "M0100050015";
                return null;
            }

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット梱包(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するパレット梱包データを取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="errMsgID">エラーメッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPalletDataAdd(DatabaseHelper dbHelper, CondM01 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // パレットリスト管理データ取得
            DataTable dt = this.GetPalletListManage(dbHelper, cond.KonpoNo);

            // データチェック
            if (dt.Rows.Count < 1)
            {
                // 該当パレットNo.はありません。
                errMsgID = "A9999999024";
                return null;
            }

            cond.ShukkaFlag = ComFunc.GetFld(dt, 0, Def_T_PALLETLIST_MANAGE.SHUKKA_FLAG);
            cond.NonyusakiCD = ComFunc.GetFld(dt, 0, Def_T_PALLETLIST_MANAGE.NONYUSAKI_CD);

            // Box情報取得
            DataSet ds = this.GetPalletDataAddExec(dbHelper, cond);

            if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
            {
                // 登録されていないBoxNo.です。
                errMsgID = "M0100050017";
                return null;
            }

            string palletkonpoDate = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE);
            if (!string.IsNullOrEmpty(palletkonpoDate))
            {
                // 既にPallet梱包済みのBoxNo.です。
                errMsgID = "M0100050019";
                return null;
            }

            string shukkaDate = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE);
            if (!string.IsNullOrEmpty(shukkaDate))
            {
                // 既に出荷済みのBoxNo.です。
                errMsgID = "M0100050020";
                return null;
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

    #region M0100060:物件名保守

    #region 物件名の追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsBukkenData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dt = ds.Tables[Def_M_BUKKEN.Name];
            // 物件名の存在チェック
            if (this.GetBukkenNameCount(dbHelper, cond) > 0)
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100060003";
                return false;
            }

            // 物件管理No採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            if (SHUKKA_FLAG.AR_VALUE1.Equals(cond.ShukkaFlag))
            {
                condSms.SaibanFlag = SAIBAN_FLAG.BKARUS_VALUE1;
            }
            else
            {
                condSms.SaibanFlag = SAIBAN_FLAG.BKUS_VALUE1;
            }
            condSms.LoginInfo = cond.LoginInfo;
            string bukkenNo;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out bukkenNo, out errMsgID))
                {
                    return false;
                }
            }

            // データ作成
            DataRow dr = dt.Rows[0];
            dr[Def_M_BUKKEN.BUKKEN_NO] = bukkenNo;

            // 登録実行
            this.InsBukken(dbHelper, cond, dr);

            // メールデータ登録
            if (UtilData.ExistsTable(ds, Def_T_MAIL.Name))
            {
                using (var comImpl = new WsCommonImpl())
                {
                    var condCom = new CondCommon(cond.LoginInfo);
                    condCom.LoginInfo = cond.LoginInfo;
                    var drMail = ds.Tables[Def_T_MAIL.Name].Rows[0];
                    drMail.SetField<object>(Def_T_MAIL.TITLE, drMail.Field<string>(Def_T_MAIL.TITLE).Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, bukkenNo));
                    drMail.SetField<object>(Def_T_MAIL.NAIYO, drMail.Field<string>(Def_T_MAIL.NAIYO).Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, bukkenNo));
                    comImpl.SaveMail(dbHelper, condCom, ds.Tables[Def_T_MAIL.Name].Rows[0], ref errMsgID, ref args);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100060003";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名の更新

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdBukkenData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetBukken(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_BUKKEN.Name))
            {
                // 既に削除された物件名です。
                errMsgID = "M0100060004";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_BUKKEN.Name], out notFoundIndex, Def_M_BUKKEN.MAINTE_VERSION, Def_M_BUKKEN.SHUKKA_FLAG, Def_M_BUKKEN.BUKKEN_NO);
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
            // 物件名の存在チェック
            if (this.GetBukkenNameCount(dbHelper, cond) > 0)
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100060003";
                return false;
            }

            // 更新実行
            this.UpdBukken(dbHelper, cond, dt);
            this.UpdBukkenNonyusaki(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100060003";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名の削除

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelBukkenData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetBukken(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_BUKKEN.Name))
            {
                // 既に削除された物件名です。
                errMsgID = "M0100060004";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_BUKKEN.Name], out notFoundIndex, Def_M_BUKKEN.MAINTE_VERSION, Def_M_BUKKEN.SHUKKA_FLAG, Def_M_BUKKEN.BUKKEN_NO);
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

            // 納入先の存在チェック
            if (this.GetBukkenNonyusakiCount(dbHelper, cond) > 0)
            {
                // 納入先マスタで使用されている物件名は削除できません。
                errMsgID = "M0100060005";
                return false;
            }

            // ロケーションの存在チェック
            if (this.GetLocationCount(dbHelper, cond) > 0)
            {
                // ロケーションマスタで使用されている物件名は削除できません。
                errMsgID = "M0100060008";
                return false;
            }

            // 削除実行
            this.DelBukken(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100070:名称マスタ保守

    #region 名称の追加

    /// --------------------------------------------------
    /// <summary>
    /// 名称の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsSelectItemData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetSelectItem(dbHelper, cond);
            if (ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                // 既に登録されている名称です。
                errMsgID = "M0100070004";
                return false;
            }
            // 登録実行
            this.InsSelectItem(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100070004";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 名称の更新

    /// --------------------------------------------------
    /// <summary>
    /// 名称の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdSelectItemData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 表示データの存在チェック
            DataSet ds = this.GetSelectItem(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                // 既に削除された名称です。
                errMsgID = "M0100070005";
                return false;
            }

            // バージョンチェック
            //下記のような VERSION の渡し方は、標準時間関係の情報が消えてしまうので行ってはいけない
            //if (ComFunc.GetFldToDateTime(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION) != UtilConvert.ToDateTime(cond.Version))
            if (ComFunc.GetFldToDateTime(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION) != ComFunc.GetFldToDateTime(dt, 0, Def_M_SELECT_ITEM.VERSION))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 登録データの存在チェック
            if (cond.ItemName != ComFunc.GetFld(dt, 0, Def_M_SELECT_ITEM.ITEM_NAME))
            {
                var cond2 = new CondM01(cond.LoginInfo);
                cond2.SelectGroupCD = ComFunc.GetFld(dt, 0, Def_M_SELECT_ITEM.SELECT_GROUP_CD);
                cond2.ItemName = ComFunc.GetFld(dt, 0, Def_M_SELECT_ITEM.ITEM_NAME);
                ds = this.GetSelectItem(dbHelper, cond2);
                if (ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
                {
                    // 既に登録されている物件名です。
                    errMsgID = "M0100070004";
                    return false;
                }
            }

            // 更新実行
            this.UpdSelectItem(dbHelper, cond, dt);
            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100070004";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 名称の削除

    /// --------------------------------------------------
    /// <summary>
    /// 名称の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelSelectItemData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetSelectItem(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_SELECT_ITEM.Name))
            {
                // 既に削除された名称です。
                errMsgID = "M0100070005";
                return false;
            }

            // バージョンチェック
            //下記のような VERSION の渡し方は、標準時間関係の情報が消えてしまうので行ってはいけない
            //if (ComFunc.GetFldToDateTime(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION) != UtilConvert.ToDateTime(cond.Version))
            if (ComFunc.GetFldToDateTime(ds, Def_M_SELECT_ITEM.Name, 0, Def_M_SELECT_ITEM.VERSION) != ComFunc.GetFldToDateTime(dt, 0, Def_M_SELECT_ITEM.VERSION))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 削除実行
            this.DelSelectItem(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100120:物件名保守

    #region 物件名の追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/14</create>
    /// <update>J.Chen 2022/09/22 受注No.追加</update>
    /// --------------------------------------------------
    public bool InsBukkenIkkatsuData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 受注No.の存在チェック
            DataSet dsExistJuchu = this.GetAndLockJuchu(dbHelper, cond, false);
            if (ComFunc.IsExistsData(dsExistJuchu, Def_M_BUKKEN.Name))
            {
                // 既に登録されている受注No.です。
                errMsgID = "M0100120008";
                return false;
            }
            // 物件名の存在チェック
            DataSet dsExistBukken = this.GetAndLockBukken(dbHelper, cond, false);
            if (ComFunc.IsExistsData(dsExistBukken, Def_M_BUKKEN.Name))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100120003";
                return false;
            }

            // ProjectNoを採番
            string projectNo;
            if (!this.GetSaibanBukkenIkkatsu(dbHelper, cond, SAIBAN_FLAG.PROJECT_NO_VALUE1, out projectNo, out errMsgID))
            {
                return false;
            }
            // Projectデータ作成
            DataRow drProject = ds.Tables[Def_M_PROJECT.Name].Rows[0];
            drProject[Def_M_PROJECT.PROJECT_NO] = projectNo;
            // Projectデータ登録
            this.InsProject(dbHelper, cond, drProject);

            // 物件管理Noを採番(本体物件・AR物件)
            string normalBukkenNo = "";
            string arBukkenNo = "";
            if (!this.GetSaibanBukkenIkkatsu(dbHelper, cond, SAIBAN_FLAG.BKUS_VALUE1, out normalBukkenNo, out errMsgID)
                || !this.GetSaibanBukkenIkkatsu(dbHelper, cond, SAIBAN_FLAG.BKARUS_VALUE1, out arBukkenNo, out errMsgID))
            {
                return false;
            }
            // 物件名データ登録(本体物件・AR物件)
            DataTable dtBukken = ds.Tables[Def_M_BUKKEN.Name];
            foreach(DataRow drBukken in dtBukken.Rows)
            {
                // 登録データセット
                drBukken[Def_M_BUKKEN.PROJECT_NO] = projectNo;
                if (ComFunc.GetFld(drBukken, Def_M_BUKKEN.SHUKKA_FLAG) == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    drBukken[Def_M_BUKKEN.BUKKEN_NO] = normalBukkenNo;
                }
                else
                {
                    drBukken[Def_M_BUKKEN.BUKKEN_NO] = arBukkenNo;
                }

                // 登録実行
                this.InsBukkenIkkatsu(dbHelper, cond, drBukken);
            }

            // メールデータ登録
            if (UtilData.ExistsTable(ds, Def_T_MAIL.Name))
            {
                using (var comImpl = new WsCommonImpl())
                {
                    var condCom = new CondCommon(cond.LoginInfo);
                    condCom.LoginInfo = cond.LoginInfo;
                    var drMail = ds.Tables[Def_T_MAIL.Name].Rows[0];
                    drMail.SetField<object>(Def_T_MAIL.TITLE, drMail.Field<string>(Def_T_MAIL.TITLE).Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, arBukkenNo));
                    drMail.SetField<object>(Def_T_MAIL.NAIYO, drMail.Field<string>(Def_T_MAIL.NAIYO).Replace(MAIL_RESERVE.BUKKEN_NO_VALUE1, arBukkenNo));
                    comImpl.SaveMail(dbHelper, condCom, ds.Tables[Def_T_MAIL.Name].Rows[0], ref errMsgID, ref args);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100120003";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件保守一括の採番処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="saibanFlag">採番フラグ</param>
    /// <param name="saiban">採番した値</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool GetSaibanBukkenIkkatsu(DatabaseHelper dbHelper, CondM01 cond, string saibanFlag, out string saiban, out string errMsgID)
    {
        try
        {
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = saibanFlag;
            condSms.LoginInfo = cond.LoginInfo;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out saiban, out errMsgID))
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

    #endregion

    #region 物件名の更新

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/17</create>
    /// <update>J.Chen 2022/11/08 受注No.追加</update>
    /// --------------------------------------------------
    public bool UpdBukkenIkkatsuData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 物件名更新データ
            DataTable dt = ds.Tables[Def_M_BUKKEN.Name];

            // 更新元の物件名の存在チェック & ロック
            DataSet dsSrc = this.GetAndLockBukken(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(dsSrc, Def_M_BUKKEN.Name))
            {
                // 既に削除された物件名です。
                errMsgID = "M0100120004";
                return false;
            }
            // 本体物件とAR物件の両方が存在しなければエラー
            if (dsSrc.Tables[Def_M_BUKKEN.Name].Rows.Count != 2)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dsSrc.Tables[Def_M_BUKKEN.Name], out notFoundIndex, Def_M_BUKKEN.MAINTE_VERSION, Def_M_BUKKEN.SHUKKA_FLAG, Def_M_BUKKEN.BUKKEN_NO);
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

            // 更新先の物件名
            CondM01 condDest = new CondM01(cond.LoginInfo);
            condDest.ProjectNo = ComFunc.GetFld(dt, 0, Def_M_BUKKEN.PROJECT_NO);

            // 受注No.チェック
            condDest.BukkenName = null;
            condDest.JuchuNo = ComFunc.GetFld(dt, 0, Def_M_BUKKEN.JUCHU_NO);
            // 更新先の物件名の存在チェック
            if (this.GetBukkenNameUpdateDestCount(dbHelper, condDest) > 0)
            {
                // 既に登録されている受注No.です。
                errMsgID = "M0100120008";
                return false;
            }

            // 物件名チェック
            condDest.BukkenName = ComFunc.GetFld(dt, 0, Def_M_BUKKEN.BUKKEN_NAME);
            condDest.JuchuNo = null;
            // 更新先の物件名の存在チェック
            if (this.GetBukkenNameUpdateDestCount(dbHelper, condDest) > 0)
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100120003";
                return false;
            }

            // 更新実行
            this.UpdBukken(dbHelper, cond, dt);
            this.UpdProject(dbHelper, cond, ds.Tables[Def_M_PROJECT.Name]);
            this.UpdBukkenNonyusaki(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている物件名です。
                errMsgID = "M0100120003";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名の削除

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelBukkenIkkatsuData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 物件名更新データ
            DataTable dt = ds.Tables[Def_M_BUKKEN.Name];

            // 物件名の存在チェック & ロック
            DataSet dsExistBukken = this.GetAndLockBukken(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(dsExistBukken, Def_M_BUKKEN.Name))
            {
                // 既に削除された物件名です。
                errMsgID = "M0100120004";
                return false;
            }
            // 本体物件とAR物件の両方が存在しなければエラー
            if (dsExistBukken.Tables[Def_M_BUKKEN.Name].Rows.Count != 2)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dsExistBukken.Tables[Def_M_BUKKEN.Name], out notFoundIndex, Def_M_BUKKEN.MAINTE_VERSION, Def_M_BUKKEN.SHUKKA_FLAG, Def_M_BUKKEN.BUKKEN_NO);
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

            // 納入先・ロケーションの存在チェック
            foreach (DataRow dr in dt.Rows)
            {
                CondM01 condDel = new CondM01(cond.LoginInfo);
                condDel.ShukkaFlag = ComFunc.GetFld(dr, Def_M_BUKKEN.SHUKKA_FLAG);
                condDel.BukkenNo = ComFunc.GetFld(dr, Def_M_BUKKEN.BUKKEN_NO);
                if (this.GetBukkenNonyusakiCount(dbHelper, condDel) > 0)
                {
                    // 納入先マスタで使用されている物件名は削除できません。
                    errMsgID = "M0100120005";
                    return false;
                }
                if (this.GetLocationCount(dbHelper, condDel) > 0)
                {
                    // ロケーションマスタで使用されている物件名は削除できません。
                    errMsgID = "M0100120007";
                    return false;
                }
            }

            // 技連マスタの存在チェック
            if (this.GetEcsCount(dbHelper, cond) > 0)
            {
                // 技連マスタで使用されている物件名は削除できません。
                errMsgID = "M0100120006";
                return false;
            }

            // 削除実行
            this.DelBukken(dbHelper, cond, dt);
            this.DelProject(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100131:パーツ名翻訳マスタ保守(取込)

    #region パーツ名の追加

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の追加(取込)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="dtMessage">エラーメッセージ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsImportedPartsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref DataTable dtMessage, ref string errMsgID, ref string[] args)
    {
        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                var condExists = new CondM01(cond.LoginInfo);
                condExists.PartNameJa = ComFunc.GetFld(dr, Def_M_PARTS_NAME.HINMEI_JP);
                condExists.Type = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);

                // 型式存在チェック
                if (!string.IsNullOrEmpty(condExists.Type) && this.ExistsImportedPartsCd(dbHelper, condExists))
                {
                    // 既に登録されています。(型式:{0}、品名(和文):{1})
                    ComFunc.AddMultiMessage(dtMessage, "M0100131012", condExists.Type, condExists.PartNameJa);
                    continue;
                }

                // 品名(和文)存在チェック
                if (this.ExistsImportedPartsName(dbHelper, condExists))
                {
                    // 既に登録されています。(型式:{0}、品名(和文):{1})
                    ComFunc.AddMultiMessage(dtMessage, "M0100131012", condExists.Type, condExists.PartNameJa);
                    continue;
                }

                try
                {
                    var ret = this.InsImportedPartsName(dbHelper, cond, dr);
                    if (ret != 1)
                    {
                        // 登録に失敗しました。(型式:{0}、品名(和文):{1})
                        ComFunc.AddMultiMessage(dtMessage, "M0100131013", condExists.Type, condExists.PartNameJa);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    // 一意制約違反チェック
                    if (this.IsDbDuplicationError(ex))
                    {
                        // 既に登録されています。(型式:{0}、品名(和文):{1})
                        ComFunc.AddMultiMessage(dtMessage, "M0100131012", condExists.Type, condExists.PartNameJa);
                        continue;
                    }
                    throw new Exception(ex.Message, ex);
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

    #endregion

    #region M0100140:荷受保守

    #region 荷受の追加

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsConsignData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 荷受CD採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.CONSIGN_CD_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string ConsignCD;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out ConsignCD, out errMsgID))
                {
                    return false;
                }
            }

            // データ作成
            DataRow dr = dt.Rows[0];
            dr[Def_M_CONSIGN.CONSIGN_CD] = ConsignCD;

            // 登録実行
            this.InsConsign(dbHelper, cond, dr);

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

    #region 荷受の更新

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdConsignData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 荷受情報の存在チェックとロック
            DataSet ds = this.GetAndLockConsign(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_CONSIGN.Name))
            {
                // 既に削除された荷受情報です。
                errMsgID = "M0100140006";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_CONSIGN.Name], out notFoundIndex, Def_M_CONSIGN.VERSION, Def_M_CONSIGN.CONSIGN_CD);
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
            this.UpdConsign(dbHelper, cond, dt);

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
    
    #region 荷受の削除

    /// --------------------------------------------------
    /// <summary>
    /// 荷受の削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelConsignData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 荷受情報の存在チェックとロック
            DataSet ds = this.GetAndLockConsign(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_CONSIGN.Name))
            {
                // 既に削除された荷受情報です。
                errMsgID = "M0100140006";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_CONSIGN.Name], out notFoundIndex, Def_M_CONSIGN.VERSION, Def_M_CONSIGN.CONSIGN_CD);
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

            // 荷姿の存在チェック
            if (this.GetPackingCount(dbHelper, cond) > 0)
            {
                // 荷姿表で使用されている荷受情報は削除できません。
                errMsgID = "M0100140007";
                return false;
            }

            // 削除実行
            this.DelConsign(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100150:配送先保守

    #region 配送先の追加

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsDeliverData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 配送先CD採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.DELIVER_CD_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string DeliverCD;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out DeliverCD, out errMsgID))
                {
                    return false;
                }
            }

            // データ作成
            DataRow dr = dt.Rows[0];
            dr[Def_M_DELIVER.DELIVER_CD] = DeliverCD;

            // 登録実行
            this.InsDeliver(dbHelper, cond, dr);

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

    #region 配送先の更新

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdDeliverData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 配送先情報の存在チェックとロック
            DataSet ds = this.GetAndLockDeliver(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_DELIVER.Name))
            {
                // 既に削除された配送先情報です。
                errMsgID = "M0100150005";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_DELIVER.Name], out notFoundIndex, Def_M_DELIVER.VERSION, Def_M_DELIVER.DELIVER_CD);
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
            this.UpdDeliver(dbHelper, cond, dt);

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

    #region 配送先の削除

    /// --------------------------------------------------
    /// <summary>
    /// 配送先の削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelDeliverData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 配送先情報の存在チェックとロック
            DataSet ds = this.GetAndLockDeliver(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_DELIVER.Name))
            {
                // 既に削除された配送先情報です。
                errMsgID = "M0100150005";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_DELIVER.Name], out notFoundIndex, Def_M_DELIVER.VERSION, Def_M_DELIVER.DELIVER_CD);
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

            // 荷姿の存在チェック
            if (this.GetPackingCountDeliver(dbHelper, cond) > 0)
            {
                // 荷姿表で使用されている配送先情報は削除できません。
                errMsgID = "M0100150006";
                return false;
            }

            // 削除実行
            this.DelDeliver(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100160:運送会社保守

    #region 運送会社の追加

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsUnsoKaisyaData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dt = ds.Tables[Def_M_UNSOKAISHA.Name];
            // 運送会社名の存在チェック
            if (this.GetUnsoKaisyaNameCount(dbHelper, cond) > 0)
            {
                // 既に登録されている運送会社です。
                errMsgID = "M0100160001";
                return false;
            }

            // 運送会社管理No採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.UNSOKAISHA_NO_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string UnsoKaisyaNo;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out UnsoKaisyaNo, out errMsgID))
                {
                    return false;
                }
            }

            // データ作成
            DataRow dr = dt.Rows[0];
            dr[Def_M_UNSOKAISHA.UNSOKAISHA_NO] = UnsoKaisyaNo;

            // 登録実行
            this.InsUnsoKaisya(dbHelper, cond, dr);

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

    #region 運送会社の更新

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdUnsoKaisyaData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataTable dtUnsokaisya = this.GetAndLockUnsokaisya(dbHelper, cond, true);
            if ((dtUnsokaisya == null) || (dtUnsokaisya.Rows.Count == 0))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dtUnsokaisya, out notFoundIndex, Def_M_UNSOKAISHA.VERSION, Def_M_UNSOKAISHA.UNSOKAISHA_NO);
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
            // 運送会社の存在チェック
            if (this.GetUnsoKaisyaNameCount(dbHelper, cond) > 0)
            {
                // 既に登録されている運送会社です。
                errMsgID = "M0100160001";
                return false;
            }

            // 更新実行
            this.UpdUnsoKaisya(dbHelper, cond, dt);

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

    #region 運送会社の削除

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelUnsoKaisyaData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 荷姿情報の存在チェック
            if (this.GetUnsokaisyaNisugataCount(dbHelper, cond, true) > 0)
            {
                // 選択行には削除できない状態のものが含まれます。荷姿登録画面で運送会社の選択を解除してください。
                errMsgID = "M0100160003";
                return false;
            }

            // 存在チェック
            DataTable dtUnsokaisya = this.GetAndLockUnsokaisya(dbHelper, cond, true);
            if ((dtUnsokaisya == null) || (dtUnsokaisya.Rows.Count == 0))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dtUnsokaisya, out notFoundIndex, Def_M_UNSOKAISHA.VERSION, Def_M_UNSOKAISHA.UNSOKAISHA_NO);
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

            // 削除実行
            this.DelUnsoKaisya(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion
    
    #region M0100170:技連保守

    #region 技連の追加
    /// --------------------------------------------------
    /// <summary>
    /// 技連の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsEcsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 技連存在チェック
            DataSet ds = this.GetEcs(dbHelper, cond);
            if (ComFunc.IsExistsData(ds, Def_M_ECS.Name))
            {
                // 既に登録されている技連です。
                errMsgID = "M0100170007";
                return false;
            }
            // 物件名存在チェック
            DataSet dsProject = this.GetProject(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsProject, Def_M_PROJECT.Name))
            {
                // 該当する物件名はありません。
                errMsgID = "M0100170008";
                return false;
            }
            // 登録実行
            this.InsEcs(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている技連です。
                errMsgID = "M0100170007";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 技連の更新
    /// --------------------------------------------------
    /// <summary>
    /// 技連の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdEcsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 技連存在チェック
            DataSet ds = this.GetEcs(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_ECS.Name))
            {
                // 既に削除された技連です。
                errMsgID = "M0100170009";
                return false;
            }
            // 物件名存在チェック
            DataSet dsProject = this.GetProject(dbHelper, cond);
            if (!ComFunc.IsExistsData(dsProject, Def_M_PROJECT.Name))
            {
                // 該当する物件名はありません。
                errMsgID = "M0100170008";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_ECS.Name], out notFoundIndex, Def_M_ECS.VERSION, Def_M_ECS.ECS_QUOTA, Def_M_ECS.ECS_NO);
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
            this.UpdEcs(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 技連の削除
    /// --------------------------------------------------
    /// <summary>
    /// 技連の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelEcsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 技連存在チェック
            DataSet ds = this.GetEcs(dbHelper, cond);
            if (!ComFunc.IsExistsData(ds, Def_M_ECS.Name))
            {
                // 既に削除された技連です。
                errMsgID = "M0100170009";
                return false;
            }
            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, ds.Tables[Def_M_ECS.Name], out notFoundIndex, Def_M_ECS.VERSION, Def_M_ECS.ECS_QUOTA, Def_M_ECS.ECS_NO);
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

            // 手配明細の存在チェック
            if (this.GetGirenTehaiMeisaiCount(dbHelper, cond) > 0)
            {
                // 手配明細で使用されている技連情報は削除できません。
                errMsgID = "M0100170006";
                return false;
            }

            // 削除実行
            this.DelEcs(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region M0100190:出荷元保守

    #region 初期データ取得(出荷元保守)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(出荷元保守)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetInitShukkamotoHoshu(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            using (WsCommonImpl comImpl = new WsCommonImpl())
            {
                var ds = new DataSet();
                {
                    // 社内外取得
                    var condCommon = new CondCommon(cond.LoginInfo);
                    condCommon.GroupCD = SHANAIGAI_FLAG.GROUPCD;
                    var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                    dt.TableName = SHANAIGAI_FLAG.GROUPCD;
                    ds.Merge(dt);
                }
                {
                    // 未使用取得
                    var condCommon = new CondCommon(cond.LoginInfo);
                    condCommon.GroupCD = UNUSED_FLAG.GROUPCD;
                    var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                    dt.TableName = UNUSED_FLAG.GROUPCD;
                    ds.Merge(dt);
                }
                return ds;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #region 表示データ取得(出荷元保守)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShipFrom(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var ds = new DataSet();
            // 出荷元マスタデータ取得
            ds.Merge(this.Sql_GetShipFrom(dbHelper, cond));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷元マスタの追加

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            if (this.Sql_ExistsShipFrom(dbHelper, cond))
            {
                // 既に登録されている出荷元です。
                errMsgID = "M0100190004";
                return false;
            }

            // 出荷元CD採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.SHIP_FROM_CD_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string shipFromCd;

            using (WsSmsImpl impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out shipFromCd, out errMsgID))
                {
                    return false;
                }
            }

            // 出荷元CD設定
            UtilData.SetFld(dt, 0, Def_M_SHIP_FROM.SHIP_FROM_NO, shipFromCd);

            // 登録実行
            this.Sql_InsShipFrom(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている出荷元です。
                errMsgID = "M0100190004";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷元マスタの更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 出荷元マスタ行ロック
            var dtShipFrom = this.Sql_LockShipFrom(dbHelper, cond);
            if (!UtilData.ExistsData(dtShipFrom))
            {
                // 既に削除されている出荷元です。
                errMsgID = "M0100190005";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dtShipFrom, out notFoundIndex, Def_M_SHIP_FROM.VERSION, Def_M_SHIP_FROM.SHIP_FROM_NO);
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

            // 存在チェック
            if (this.Sql_ExistsShipFrom(dbHelper, cond))
            {
                // 既に登録されている出荷元です。
                errMsgID = "M0100190004";
                return false;
            }

            // 更新実行
            this.Sql_UpdShipFrom(dbHelper, cond, dt);

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

    #region 出荷元マスタの削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 出荷元マスタ行ロック
            var dtShipFrom = this.Sql_LockShipFrom(dbHelper, cond);
            if (!UtilData.ExistsData(dtShipFrom))
            {
                // 既に削除されている出荷元です。
                errMsgID = "M0100190005";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(dt, dtShipFrom, out notFoundIndex, Def_M_SHIP_FROM.VERSION, Def_M_SHIP_FROM.SHIP_FROM_NO);
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

            // 荷姿の存在チェック
            if (this.Sql_ExistsPackingForShipFrom(dbHelper, cond))
            {
                // 荷姿表で使用されている出荷元情報は削除できません。
                errMsgID = "M0100190008";
                return false;
            }

            // 削除実行
            this.Sql_DelShipFrom(dbHelper, cond, dt);

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

    #region M0100020:担当者マスタ

    #region SELECT

    #region ユーザー取得(完全一致・ユーザーID必須)

    /// --------------------------------------------------
    /// <summary>
    /// ユーザー取得(完全一致・ユーザーID必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メール関係追加</update>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// <update>H.Tajimi 2018/10/16 FE要望</update>
    /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
    /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
    /// <update>J.Chen 2024/01/31 計画取込一括設定処理追加</update>
    /// --------------------------------------------------
    public DataSet GetUser(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MU.USER_ID");
            sb.ApdL("     , MU.USER_NAME");
            sb.ApdL("     , MU.PASSWORD");
            sb.ApdL("     , MU.ROLE_ID");
            sb.ApdL("     , MR.ROLE_NAME");
            sb.ApdL("     , MR.USER_DELETE_FLAG");
            sb.ApdL("     , MU.USER_NOTE");
            sb.ApdL("     , MU.USER_FLAG");
            sb.ApdL("     , MU.PASSWORD_CHANGE_DATE");
            sb.ApdL("     , MU.MAIL_ADDRESS");
            sb.ApdL("     , MU.MAIL_CHANGE_ROLE");
            sb.ApdL("     , COM.ITEM_NAME AS MAIL_CHANGE_ROLE_NAME");
            sb.ApdL("     , MU.STAFF_KBN");
            sb.ApdL("     , COM2.ITEM_NAME AS STAFF_KBN_NAME");
            sb.ApdL("     , MU.MAIL_PACKING_FLAG");
            sb.ApdL("     , COM3.ITEM_NAME AS MAIL_PACKING_FLAG_NAME");
            sb.ApdL("     , MU.MAIL_TAG_RENKEI_FLAG");
            sb.ApdL("     , COM4.ITEM_NAME AS MAIL_TAG_RENKEI_FLAG_NAME");
            sb.ApdL("     , MU.MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdL("     , COM5.ITEM_NAME AS MAIL_SHUKKAKEIKAKU_FLAG_NAME");
            sb.ApdL("     , MU.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_USER MU");
            sb.ApdL("  LEFT JOIN M_ROLE MR ON MR.ROLE_ID = MU.ROLE_ID");
            sb.ApdN("       AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_CHANGE_ROLE AND COM.VALUE1 = MU.MAIL_CHANGE_ROLE");
            sb.ApdN("       AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("STAFF_KBN AND COM2.VALUE1 = MU.STAFF_KBN");
            sb.ApdN("       AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM3 ON COM3.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_PACKING_FLAG AND COM3.VALUE1 = MU.MAIL_PACKING_FLAG");
            sb.ApdN("       AND COM3.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM4 ON COM4.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_TAG_RENKEI_FLAG AND COM4.VALUE1 = MU.MAIL_TAG_RENKEI_FLAG");
            sb.ApdN("       AND COM4.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM5 ON COM5.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_SHUKKAKEIKAKU_FLAG AND COM5.VALUE1 = MU.MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdN("       AND COM5.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       MU.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MU.USER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_CHANGE_ROLE", Def_M_USER.MAIL_CHANGE_ROLE));
            paramCollection.Add(iNewParam.NewDbParameter("STAFF_KBN", STAFF_KBN.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_PACKING_FLAG", MAIL_PACKING_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_TAG_RENKEI_FLAG", MAIL_TAG_RENKEI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_SHUKKAKEIKAKU_FLAG", MAIL_SHUKKAKEIKAKU_FLAG.GROUPCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_USER.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ユーザー取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// ユーザー取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メール関係追加</update>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
    /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
    /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
    /// <update>J.Chen 2024/01/31 計画取込一括設定処理追加</update>
    /// --------------------------------------------------
    public DataSet GetUserLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MU.USER_ID");
            sb.ApdL("     , MU.USER_NAME");
            sb.ApdL("     , MU.PASSWORD");
            sb.ApdL("     , MU.ROLE_ID");
            sb.ApdL("     , MR.ROLE_NAME");
            sb.ApdL("     , MR.USER_DELETE_FLAG");
            sb.ApdL("     , MU.USER_NOTE");
            sb.ApdL("     , MU.USER_FLAG");
            sb.ApdL("     , MU.PASSWORD_CHANGE_DATE");
            sb.ApdL("     , MU.MAIL_ADDRESS");
            sb.ApdL("     , MU.MAIL_CHANGE_ROLE");
            sb.ApdL("     , COM.ITEM_NAME AS MAIL_CHANGE_ROLE_NAME");
            sb.ApdL("     , MU.STAFF_KBN");
            sb.ApdL("     , COM2.ITEM_NAME AS STAFF_KBN_NAME");
            sb.ApdL("     , MU.MAIL_PACKING_FLAG");
            sb.ApdL("     , COM3.ITEM_NAME AS MAIL_PACKING_FLAG_NAME");
            sb.ApdL("     , MU.MAIL_TAG_RENKEI_FLAG");
            sb.ApdL("     , COM4.ITEM_NAME AS MAIL_TAG_RENKEI_FLAG_NAME");
            sb.ApdL("     , MU.MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdL("     , COM5.ITEM_NAME AS MAIL_SHUKKAKEIKAKU_FLAG_NAME");
            sb.ApdL("     , MU.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_USER MU");
            sb.ApdL("  LEFT JOIN M_ROLE MR ON MR.ROLE_ID = MU.ROLE_ID");
            sb.ApdN("       AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_CHANGE_ROLE AND COM.VALUE1 = MU.MAIL_CHANGE_ROLE");
            sb.ApdN("       AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("STAFF_KBN AND COM2.VALUE1 = MU.STAFF_KBN");
            sb.ApdN("       AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM3 ON COM3.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_PACKING_FLAG AND COM3.VALUE1 = MU.MAIL_PACKING_FLAG");
            sb.ApdN("       AND COM3.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM4 ON COM4.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_TAG_RENKEI_FLAG AND COM4.VALUE1 = MU.MAIL_TAG_RENKEI_FLAG");
            sb.ApdN("       AND COM4.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN("  LEFT JOIN M_COMMON COM5 ON COM5.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_SHUKKAKEIKAKU_FLAG AND COM5.VALUE1 = MU.MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdN("       AND COM5.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       MU.USER_FLAG = ").ApdN(this.BindPrefix).ApdL("USER_FLAG");
            if (!string.IsNullOrEmpty(cond.UserID))
            {
                sb.ApdN("   AND MU.USER_ID LIKE ").ApdN(this.BindPrefix).ApdL("USER_ID");
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID + "%"));
            }
            if (!string.IsNullOrEmpty(cond.UserName))
            {
                sb.ApdN("   AND MU.USER_NAME LIKE ").ApdN(this.BindPrefix).ApdL("USER_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("USER_NAME", cond.UserName + "%"));
            }
            if (!string.Equals(cond.MailPackingFlag, DISP_MAIL_PACKING_FLAG.DEFAULT_VALUE1))
            {
                sb.ApdN("   AND MU.MAIL_PACKING_FLAG = ").ApdN(this.BindPrefix).ApdL("COND_MAIL_PACKING_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("COND_MAIL_PACKING_FLAG", cond.MailPackingFlag));
            }
            if (!string.Equals(cond.MailTagRenkeiFlag, DISP_MAIL_TAG_RENKEI_FLAG.DEFAULT_VALUE1))
            {
                sb.ApdN("   AND MU.MAIL_TAG_RENKEI_FLAG = ").ApdN(this.BindPrefix).ApdL("COND_MAIL_TAG_RENKEI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("COND_MAIL_TAG_RENKEI_FLAG", cond.MailTagRenkeiFlag));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MU.USER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("USER_FLAG", USER_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_CHANGE_ROLE", Def_M_USER.MAIL_CHANGE_ROLE));
            paramCollection.Add(iNewParam.NewDbParameter("STAFF_KBN", STAFF_KBN.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_PACKING_FLAG", MAIL_PACKING_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_TAG_RENKEI_FLAG", MAIL_TAG_RENKEI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_SHUKKAKEIKAKU_FLAG", MAIL_SHUKKAKEIKAKU_FLAG.GROUPCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_USER.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 指定ユーザーID以外の他ユーザーでの権限数を取得(完全一致・ユーザーID必須)

    /// --------------------------------------------------
    /// <summary>
    /// 指定ユーザーID以外の削除制限ユーザー数を取得(完全一致・ユーザーID必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ユーザーマスタ</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// --------------------------------------------------
    public int LockRemoveRestrictionsUserCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_USER MU");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("  LEFT JOIN M_ROLE MR ON MR.ROLE_ID = MU.ROLE_ID");
            sb.ApdN("                     AND MR.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       MU.USER_ID <> ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("   AND MR.USER_DELETE_FLAG = ").ApdN(this.BindPrefix).ApdL("USER_DELETE_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(iNewParam.NewDbParameter("USER_DELETE_FLAG", USER_DELETE_FLAG.LIMITONE_VALUE1));

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

    #region 権限マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>権限マスタ</returns>
    /// <create>Y.Higuchi 2010/09/12</create>
    /// <update>H.Tajimi 2018/09/05 多言語対応</update>
    /// --------------------------------------------------
    private DataTable GetRole(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ROLE_ID");
            sb.ApdL("     , ROLE_NAME");
            sb.ApdL("     , ROLE_FLAG");
            sb.ApdL("     , USER_DELETE_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_ROLE");
            sb.ApdL(" WHERE");
            sb.ApdN("       ROLE_ID = ").ApdN(this.BindPrefix).ApdL("ROLE_ID");
            sb.ApdN("   AND LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ROLE_ID", cond.RoleID));
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

    #region メールアドレス重複チェック

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレス重複チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">保存データ</param>
    /// <create>R.Katsuo 2017/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool DuplicateMailAddress(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT 1");
            sb.ApdL("  FROM M_USER");
            sb.ApdN(" WHERE MAIL_ADDRESS = ").ApdN(this.BindPrefix).ApdL("MAIL_ADDRESS");
            sb.ApdN("   AND USER_ID <> ").ApdN(this.BindPrefix).ApdL("USER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS", ComFunc.GetFld(dt, 0, Def_M_USER.MAIL_ADDRESS)));
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", ComFunc.GetFld(dt, 0, Def_M_USER.USER_ID)));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ret);

            return 0 < ret.Rows.Count;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region 物件メール明細マスタを取得(完全一致・ユーザーID必須)

    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタを取得(完全一致・ユーザーID必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件メール明細マスタ</returns>
    /// <create>H.Tsuji 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukkenMail(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT MBM.MAIL_HEADER_ID");
            sb.ApdL("     , MBM.MAIL_KBN");
            sb.ApdL("     , MBM.LIST_FLAG");
            sb.ApdL("     , MC1.ITEM_NAME AS LIST_FLAG_NAME");
            sb.ApdL("     , MBMM.MAIL_ADDRESS_FLAG");
            sb.ApdL("     , MC2.ITEM_NAME AS MAIL_ADDRESS_FLAG_NAME");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("  FROM M_BUKKEN_MAIL MBM");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("         SELECT *");
            sb.ApdL("           FROM M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("          WHERE M_BUKKEN_MAIL_MEISAI.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdL("       ) MBMM");
            sb.ApdL("    ON MBM.MAIL_HEADER_ID = MBMM.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_COMMON MC1");
            sb.ApdL("    ON MC1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdL("   AND MC1.VALUE1 = MBM.LIST_FLAG");
            sb.ApdN("   AND MC1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON MC2");
            sb.ApdL("    ON MC2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("MAIL_ADDRESS_FLAG");
            sb.ApdL("   AND MC2.VALUE1 = MBMM.MAIL_ADDRESS_FLAG");
            sb.ApdN("   AND MC2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN MB");
            sb.ApdL("    ON MB.SHUKKA_FLAG = MBM.SHUKKA_FLAG");
            sb.ApdL("   AND MB.BUKKEN_NO = MBM.BUKKEN_NO");
            sb.ApdL(" ORDER BY MBM.MAIL_KBN, MBMM.MAIL_ADDRESS_FLAG, MBM.MAIL_HEADER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", LIST_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS_FLAG", MAIL_ADDRESS_FLAG.GROUPCD));

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

    #region 荷受メール取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受メール取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetConsignMailHeaderId(DatabaseHelper dbHelper)
    {
        try
        {
            var dt = new DataTable(Def_M_CONSIGN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT ");
            sb.ApdL("       M_CONSIGN.CONSIGN_CD");
            sb.ApdL("     , M_CONSIGN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  FROM M_CONSIGN");
            sb.ApdL("  LEFT JOIN M_CONSIGN_MAIL");
            sb.ApdL("    ON M_CONSIGN.CONSIGN_CD = M_CONSIGN_MAIL.CONSIGN_CD");
            sb.ApdL(" WHERE 1 = 1");
            sb.ApdL(" ORDER BY M_CONSIGN.CONSIGN_CD");

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷受メール明細取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受メール明細取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="MailHeaderId">メールヘッダーID</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetConsignMailUser(DatabaseHelper dbHelper, string mailHeaderId)
    {
        try
        {
            var dt = new DataTable(Def_M_CONSIGN_MAIL_MEISAI.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT ");
            sb.ApdL("       M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("     , M_CONSIGN_MAIL_MEISAI.ORDER_NO");
            sb.ApdL("     , M_CONSIGN_MAIL_MEISAI.USER_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(12)) AS MAIL_HEADER_ID");
            sb.ApdL("  FROM M_CONSIGN_MAIL_MEISAI");
            sb.ApdL(" WHERE 1 = 1");
            if (string.IsNullOrEmpty(mailHeaderId))
            {
                sb.ApdN("   AND M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID IS NULL");
            }
            else
            {
                sb.ApdN("   AND M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID = ").ApdN(this.BindPrefix).ApdL("MAIL_HEADER_ID");
                pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", mailHeaderId));
            }
            sb.ApdL(" ORDER BY M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_CONSIGN_MAIL_MEISAI.ORDER_NO ");

            dbHelper.Fill(sb.ToString(), pc, dt);
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
    /// ユーザーの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メール関係追加</update>
    /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
    /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
    /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
    /// <update>J.Chen 2024/01/31 計画取込一括設定処理追加</update>
    /// --------------------------------------------------
    public int InsUser(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_USER");
            sb.ApdL("(");
            sb.ApdL("       USER_ID");
            sb.ApdL("     , USER_NAME");
            sb.ApdL("     , PASSWORD");
            sb.ApdL("     , ROLE_ID");
            sb.ApdL("     , USER_NOTE");
            sb.ApdL("     , USER_FLAG");
            sb.ApdL("     , PASSWORD_CHANGE_DATE");
            sb.ApdL("     , CREATE_USER");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , UPDATE_USER");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , MAIL_ADDRESS");
            sb.ApdL("     , MAIL_CHANGE_ROLE");
            sb.ApdL("     , STAFF_KBN");
            sb.ApdL("     , MAIL_PACKING_FLAG");
            sb.ApdL("     , MAIL_TAG_RENKEI_FLAG");
            sb.ApdL("     , MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PASSWORD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ROLE_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_NOTE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_FLAG");
            sb.ApdN("     , CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_ADDRESS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_CHANGE_ROLE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STAFF_KBN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_PACKING_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_TAG_RENKEI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", ComFunc.GetFldObject(dr, Def_M_USER.USER_ID, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_NAME", ComFunc.GetFldObject(dr, Def_M_USER.USER_NAME, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("PASSWORD", ComFunc.GetFldObject(dr, Def_M_USER.PASSWORD, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("ROLE_ID", ComFunc.GetFldObject(dr, Def_M_USER.ROLE_ID, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_NOTE", ComFunc.GetFldObject(dr, Def_M_USER.USER_NOTE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_FLAG", USER_FLAG.NORMAL_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER", this.GetUpdateUserID(cond)));
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS", DBNull.Value));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_ADDRESS, string.Empty)));
                }
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_CHANGE_ROLE", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_CHANGE_ROLE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("STAFF_KBN", ComFunc.GetFldObject(dr, Def_M_USER.STAFF_KBN, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_PACKING_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_PACKING_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_TAG_RENKEI_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_TAG_RENKEI_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_SHUKKAKEIKAKU_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG, string.Empty)));

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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// ユーザーの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update>R.Katsuo 2017/09/05 メール関係追加</update>
    /// <update>H.Tajimi 2018/10/16 FE要望対応</update>
    /// <update>H.Tsuji 2018/12/11 荷姿表送信対象フラグ追加</update>
    /// <update>H.Tajimi 2019/08/14 TAG連携送信対象フラグ追加</update>
    /// <update>J.Chen 2024/01/31 計画取込一括設定処理追加</update>
    /// --------------------------------------------------
    public int UpdUser(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_USER");
            sb.ApdL("SET");
            sb.ApdN("       USER_NAME = ").ApdN(this.BindPrefix).ApdL("USER_NAME");
            sb.ApdN("     , PASSWORD = ").ApdN(this.BindPrefix).ApdL("PASSWORD");
            sb.ApdN("     , ROLE_ID = ").ApdN(this.BindPrefix).ApdL("ROLE_ID");
            sb.ApdN("     , USER_NOTE = ").ApdN(this.BindPrefix).ApdL("USER_NOTE");
            sb.ApdN("     , PASSWORD_CHANGE_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_USER = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER");
            sb.ApdL("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdL("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAIL_ADDRESS = ").ApdN(this.BindPrefix).ApdL("MAIL_ADDRESS");
            sb.ApdN("     , MAIL_CHANGE_ROLE = ").ApdN(this.BindPrefix).ApdL("MAIL_CHANGE_ROLE");
            sb.ApdN("     , STAFF_KBN = ").ApdN(this.BindPrefix).ApdL("STAFF_KBN");
            sb.ApdN("     , MAIL_PACKING_FLAG = ").ApdN(this.BindPrefix).ApdL("MAIL_PACKING_FLAG");
            sb.ApdN("     , MAIL_TAG_RENKEI_FLAG = ").ApdN(this.BindPrefix).ApdL("MAIL_TAG_RENKEI_FLAG");
            sb.ApdN("     , MAIL_SHUKKAKEIKAKU_FLAG = ").ApdN(this.BindPrefix).ApdL("MAIL_SHUKKAKEIKAKU_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("       USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("USER_NAME", ComFunc.GetFldObject(dr, Def_M_USER.USER_NAME, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("PASSWORD", ComFunc.GetFldObject(dr, Def_M_USER.PASSWORD, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("ROLE_ID", ComFunc.GetFldObject(dr, Def_M_USER.ROLE_ID, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_NOTE", ComFunc.GetFldObject(dr, Def_M_USER.USER_NOTE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER", this.GetUpdateUserID(cond)));
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_M_USER.MAIL_ADDRESS)))
                {
                    paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS", DBNull.Value));
                }
                else
                {
                    paramCollection.Add(iNewParam.NewDbParameter("MAIL_ADDRESS", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_ADDRESS, string.Empty)));
                }
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_CHANGE_ROLE", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_CHANGE_ROLE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("STAFF_KBN", ComFunc.GetFldObject(dr, Def_M_USER.STAFF_KBN, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_PACKING_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_PACKING_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_TAG_RENKEI_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_TAG_RENKEI_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", ComFunc.GetFldObject(dr, Def_M_USER.USER_ID, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_SHUKKAKEIKAKU_FLAG", ComFunc.GetFldObject(dr, Def_M_USER.MAIL_SHUKKAKEIKAKU_FLAG, string.Empty)));

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
    /// ユーザーの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelUser(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_USER");
            sb.ApdL(" WHERE");
            sb.ApdN("       USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));

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

    #region M0100040:納入先保守

    #region SELECT

    #region 納入先取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>納入先マスタ</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNonyusakiLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       MN.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MN.SHIP");
            sb.ApdL("     , MN.KANRI_FLAG");
            sb.ApdL("     , COM2.ITEM_NAME AS KANRI_FLAG_NAME");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
            sb.ApdL("     , MN.REMOVE_FLAG");
            sb.ApdL("     , COM3.ITEM_NAME AS REMOVE_FLAG_NAME");
            // @@@ ↑

            // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
            //　SQL分の追加、出荷元コードと出荷元名（出荷元マスタより）
            sb.ApdL("     , MN.SHIP_FROM_CD");
            sb.ApdL("     , COM4.SHIP_FROM_NAME AS SHIP_FROM_NAME");


            sb.ApdL("     , MN.VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = MN.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'KANRI_FLAG'");
            sb.ApdL("                         AND COM2.VALUE1 = MN.KANRI_FLAG");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            //@@@ 2011/02/16 M.Tsutsumi Add Step2 No.37
            sb.ApdL("  LEFT JOIN M_COMMON COM3 ON COM3.GROUP_CD = 'REMOVE_FLAG'");
            sb.ApdL("                         AND COM3.VALUE1 = MN.REMOVE_FLAG");
            // @@@ ↑
            sb.ApdN("                         AND COM3.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MN.BUKKEN_NO");

            // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
            //　SQL文の追加　出荷元マスタを外部結合（条件は未使用フラグが '0'）
            sb.ApdL("  LEFT JOIN M_SHIP_FROM COM4 ON COM4.SHIP_FROM_NO = MN.SHIP_FROM_CD");
            sb.ApdL("                            AND COM4.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");


            sb.ApdL(" WHERE");
            sb.ApdN("       MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            if (!string.IsNullOrEmpty(cond.NonyusakiName))
            {
                sb.ApdN("   AND BK.BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.NonyusakiName + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                sb.ApdN("   AND MN.SHIP LIKE ").ApdN(this.BindPrefix).ApdL("SHIP");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP", cond.Ship + "%"));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MN.SHUKKA_FLAG");
            sb.ApdL("     , MN.NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));

            // 出荷元コンボボックス追加による 2022/10/04（TW-Tsuji）
            //　バインド変数追加　未使用フラグが"使用"　定数：UNUSED_FLAG.USED_VALUE1　'0'
            paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", UNUSED_FLAG.USED_VALUE1));

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

    #region 未完了AR情報件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 未完了AR情報件数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>未完了AR情報件数</returns>
    /// <create>Y.Higuchi 2010/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMikanryoAR(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND JYOKYO_FLAG <> ").ApdN(this.BindPrefix).ApdL("JYOKYO_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", JYOKYO_FLAG.KANRYO_VALUE1));

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

    #region AR情報リスト区分取得

    /// --------------------------------------------------
    /// <summary>
    /// AR情報リスト区分取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="listFlag">リストフラグ</param>
    /// <returns>データセット[T_AR(カウントだけ)]</returns>
    /// <create>M.Tsutsumi 2011/03/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetARListFlagCount(DatabaseHelper dbHelper, string nonyusakiCd, string listFlag)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_AR");
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", listFlag));

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

    #region 納入先取得(存在確認用)

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得(存在確認用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNonyusaki(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM M_NONYUSAKI");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND SHIP = ").ApdN(this.BindPrefix).ApdL("SHIP");
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                sb.ApdN("   AND NONYUSAKI_CD <> ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            }

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParameter.NewDbParameter("SHIP", cond.Ship));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 未出荷明細件数取得

    /// --------------------------------------------------
    /// <summary>
    /// 未出荷明細件数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2012/04/16</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMishukkaMeisai(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM T_SHUKKA_MEISAI");
            sb.ApdN(" WHERE SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND JYOTAI_FLAG < ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("JYOTAI_FLAG", JYOKYO_FLAG.SHUKKAZUMI_VALUE1));

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

    #region M0100050:梱包情報保守

    #region SELECT

    #region Box梱包

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包データ取得時のチェック用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update>K.Tsutsumi 2018/01/26 同じ条件を記載しているので削除</update>
    /// --------------------------------------------------
    private DataTable GetCheckBoxData(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , COUNT(TSM.PALLET_NO) AS PALLET_NO_CNT");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_CNT");
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
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TBM.BOX_NO");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHUKKA_CNT DESC");
            sb.ApdL("     , PALLET_NO_CNT DESC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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
    /// Box梱包データ取得実行部
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// --------------------------------------------------
    public DataSet GetBoxDataExec(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("'{0}' AS {1}",
                                                cond.TextKonpo,
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
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdN("     , ").ApdL(buttonState);
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
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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

    #region パレット梱包

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包データ取得時のチェック用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetCheckPalletData(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL("     , COUNT(TSM.KOJI_NO) AS KOJI_NO_CNT");
            sb.ApdL("     , COUNT(TSM.SHUKKA_DATE) AS SHUKKA_CNT");
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
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TPM.PALLET_NO");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHUKKA_CNT DESC");
            sb.ApdL("     , KOJI_NO_CNT DESC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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
    /// パレット梱包データ取得実行部
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// <update>K.Tsutsumi 2018/01/26 遅い</update>
    /// --------------------------------------------------
    public DataSet GetPalletDataExec(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("'{0}' AS {1}",
                                                cond.TextKonpo,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TAR.AR_NO");
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
            sb.ApdL("     , TBM.VERSION");
            sb.ApdN("     , ").ApdL(buttonState);
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
            sb.ApdL("                  , AR_NO");
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
            sb.ApdL("                  , M_NO");
            sb.ApdL("               FROM T_SHUKKA_MEISAI TSM2");
            sb.ApdL("              WHERE");
            sb.ApdN("                    TSM2.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("            ) TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                 AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                 AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL("                 AND TSM.ROW_NO = 1");
            sb.ApdL(" INNER JOIN T_BOXLIST_MANAGE TBM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                                AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                                AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_AR TAR ON TSM.SHUKKA_FLAG = '1'");
            sb.ApdL("                    AND TSM.NONYUSAKI_CD = TAR.NONYUSAKI_CD");
            sb.ApdL("                    AND TSM.AR_NO = TAR.AR_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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

    #region Box梱包(出荷明細データ数取得)

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包でBoxNo.に紐付く出荷明細データ数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>件数データ</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetBoxDataMeisaiCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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

    #region パレット梱包(出荷明細データ数取得)

    /// --------------------------------------------------
    /// <summary>
    /// パレット梱包でBoxNo.に紐付く出荷明細データ数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>件数データ</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetPalletDataMeisaiCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TPM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                               AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                               AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL(" WHERE ");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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

    #region Box梱包(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するBox梱包データ取得SQL
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxDataAddExec(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("'{0}' AS {1}",
                                                cond.TextTouroku,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
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
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.KIWAKUKONPO_DATE");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TSM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.TagNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region BOXリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="boxNo">BoxNo</param>
    /// <returns>DataTable</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetBoxListManage(DatabaseHelper dbHelper, string boxNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BOXLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", boxNo));

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

    #region 出荷明細データ(チェック用)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ(チェック用)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>M.Tsutsumi 2011/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShukkaMeisaiBoxKonpoAdd(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , JYOTAI_FLAG");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.TagNo));

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

    #region パレット梱包(追加)

    /// --------------------------------------------------
    /// <summary>
    /// 追加するパレット梱包データ取得SQL
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/10/10 多言語化対応</update>
    /// <update>D.Okumura 2020/06/29 EFA_SMS-120 遅延処理対応</update>
    /// --------------------------------------------------
    public DataSet GetPalletDataAddExec(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            string buttonState = string.Format("'{0}' AS {1}",
                                                cond.TextTouroku,
                                                ComDefine.FLD_BTN_STATE);

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.BOX_NO");
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
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TBM.VERSION");
            sb.ApdN("     , ").ApdL(buttonState);
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            // BoxNo.単位でTagNo.の1件目のデータを表示する為の副問合せ
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("             SELECT");
            sb.ApdL("                    ROW_NUMBER() OVER(PARTITION BY BOX_NO ORDER BY TAG_NO) AS ROW_NO");
            sb.ApdL("                  , SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , TAG_NO");
            sb.ApdL("                  , AR_NO");
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
            sb.ApdL("                  , PALLETKONPO_DATE");
            sb.ApdL("                  , SHUKKA_DATE");
            sb.ApdL("                  , M_NO");
            sb.ApdL("               FROM T_SHUKKA_MEISAI");
            sb.ApdL("               WHERE");
            sb.ApdN("                       T_SHUKKA_MEISAI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("                   AND T_SHUKKA_MEISAI.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("                   AND T_SHUKKA_MEISAI.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL("            ) TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                 AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                 AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("                 AND TSM.ROW_NO = 1");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND TBM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TBM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHUKKA_MEISAI.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region パレットリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="palletNo">パレットNo</param>
    /// <returns>DataTable</returns>
    /// <create>M.Tsutsumi 2011/03/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetPalletListManage(DatabaseHelper dbHelper, string palletNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_PALLETLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", palletNo));

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

    #region 出荷明細データ(チェック用)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ(チェック用)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>データテーブル</returns>
    /// <create>M.Tsutsumi 2011/03/10</create>
    /// <update>M.Shimizu 2020/06/30 EFA_SMS-120 遅延処理対応</update>
    /// --------------------------------------------------
    public DataTable GetShukkaMeisaiPalletKonpoAdd(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TBM.VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            // BoxNo.単位でTagNo.の1件目のデータを表示する為の副問合せ
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("             SELECT");
            sb.ApdL("                    ROW_NUMBER() OVER(PARTITION BY BOX_NO ORDER BY TAG_NO) AS ROW_NO");
            sb.ApdL("                  , SHUKKA_FLAG");
            sb.ApdL("                  , NONYUSAKI_CD");
            sb.ApdL("                  , BOX_NO");
            sb.ApdL("                  , JYOTAI_FLAG");
            sb.ApdL("               FROM T_SHUKKA_MEISAI");
            // ↓↓↓ M.Shimizu 2020/06/30 EFA_SMS-120 遅延処理対応
            sb.ApdL("               WHERE");
            sb.ApdN("                       T_SHUKKA_MEISAI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("                   AND T_SHUKKA_MEISAI.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("                   AND T_SHUKKA_MEISAI.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            // ↑↑↑ M.Shimizu 2020/06/30 EFA_SMS-120 遅延処理対応
            sb.ApdL("            ) TSM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("                 AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("                 AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("                 AND TSM.ROW_NO = 1");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND TBM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TBM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));

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

    #region パレットリスト管理データ（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ、ロック取得用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>K.Tsutsumi 2012/05/18</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockPalletListManage(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_PALLETLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("	  SHUKKA_FLAG");
            sb.ApdL("	, NONYUSAKI_CD");
            sb.ApdL("	, PALLET_NO");
            sb.ApdL("	, LISTHAKKO_FLAG");
            sb.ApdL("	, LISTHAKKO_DATE");
            sb.ApdL("	, LISTHAKKO_USER");
            sb.ApdL("	, SHUKKA_DATE");
            sb.ApdL("	, SHUKKA_USER_ID");
            sb.ApdL("	, SHUKKA_USER_NAME");
            sb.ApdL("	, UKEIRE_DATE");
            sb.ApdL("	, UKEIRE_USER_ID");
            sb.ApdL("	, UKEIRE_USER_NAME");
            sb.ApdL("	, KANRI_FLAG");
            sb.ApdL("	, CREATE_DATE");
            sb.ApdL("	, CREATE_USER_ID");
            sb.ApdL("	, CREATE_USER_NAME");
            sb.ApdL("	, UPDATE_DATE");
            sb.ApdL("	, UPDATE_USER_ID");
            sb.ApdL("	, UPDATE_USER_NAME");
            sb.ApdL("	, VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PALLETLIST_MANAGE");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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

    #region ボックスリスト管理データ（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// ボックスリスト管理データ、ロック取得用SQL
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>K.Tsutsumi 2012/05/18</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockBoxListManage(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BOXLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("	  SHUKKA_FLAG");
            sb.ApdL("	, NONYUSAKI_CD");
            sb.ApdL("	, BOX_NO");
            sb.ApdL("	, LISTHAKKO_FLAG");
            sb.ApdL("	, LISTHAKKO_DATE");
            sb.ApdL("	, LISTHAKKO_USER");
            sb.ApdL("	, SHUKKA_DATE");
            sb.ApdL("	, SHUKKA_USER_ID");
            sb.ApdL("	, SHUKKA_USER_NAME");
            sb.ApdL("	, UKEIRE_DATE");
            sb.ApdL("	, UKEIRE_USER_ID");
            sb.ApdL("	, UKEIRE_USER_NAME");
            sb.ApdL("	, KANRI_FLAG");
            sb.ApdL("	, CREATE_DATE");
            sb.ApdL("	, CREATE_USER_ID");
            sb.ApdL("	, CREATE_USER_NAME");
            sb.ApdL("	, UPDATE_DATE");
            sb.ApdL("	, UPDATE_USER_ID");
            sb.ApdL("	, UPDATE_USER_NAME");
            sb.ApdL("	, VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BOXLIST_MANAGE");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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

    #region 出荷明細データ(Box梱包解除)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新(Box梱包解除)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データテーブル</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiBoxKaijyo(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
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
            sb.ApdN("     , BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("     , BOXKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("BOXKONPO_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", cond.JyotaiFlag));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("BOXKONPO_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #region 出荷明細データ(パレット梱包解除)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新(パレット梱包解除)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データテーブル</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>Y.Higuchi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiPalletKaijyo(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
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
            sb.ApdN("     , PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("     , PALLETKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("PALLETKONPO_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", cond.JyotaiFlag));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("PALLETKONPO_DATE", DBNull.Value));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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

    #region 出荷明細データ(Box梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新(Box梱包)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データテーブル</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>M.Tsutsumi 2011/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiBoxKonpo(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
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
            sb.ApdN("     , BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("     , BOXKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("BOXKONPO_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", cond.JyotaiFlag));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));
                paramCollection.Add(iNewParam.NewDbParameter("BOXKONPO_DATE", cond.UpdateDate == null ? null : UtilConvert.ToDateTime(cond.UpdateDate).ToShortDateString()));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

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

    #region 出荷明細データ(パレット梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新(パレット梱包)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データテーブル</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>M.Tsutsumi 2011/03/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiPalletKonpo(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
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
            sb.ApdN("     , PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("     , PALLETKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("PALLETKONPO_DATE");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", cond.JyotaiFlag));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));
                paramCollection.Add(iNewParam.NewDbParameter("PALLETKONPO_DATE", cond.UpdateDate == null ? null : UtilConvert.ToDateTime(cond.UpdateDate).ToShortDateString()));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOX_NO)));

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

    #region パレットリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ更新(パレット梱包)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>K.Tsutsumi 2012/05/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletListManage(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdN("       VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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

    #region ボックスリスト管理データ

    /// --------------------------------------------------
    /// <summary>
    /// ボックスリスト管理データ更新(パレット梱包)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を及ぼした行数</returns>
    /// <create>K.Tsutsumi 2012/05/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxListManage(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdN("       VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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

    #region Boxリスト管理データ削除

    /// --------------------------------------------------
    /// <summary>
    /// Boxリスト管理データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBoxListManage(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_BOXLIST_MANAGE");
            sb.ApdL(" WHERE ");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.KonpoNo));

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

    #region パレットリスト管理データ削除

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelPalletListManage(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_PALLETLIST_MANAGE");
            sb.ApdL(" WHERE ");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.KonpoNo));

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

    #region M0100060:物件名保守

    #region SELECT

    #region 物件名取得(完全一致・物件管理No必須)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名取得(完全一致・物件管理No必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukken(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MB.ISSUED_TAG_NO");
            sb.ApdL("     , MB.VERSION");
            sb.ApdL("     , MB.MAINTE_VERSION");
            sb.ApdL("     , MB.MAIL_NOTIFY");
            sb.ApdL("     , COM2.ITEM_NAME AS MAIL_NOTIFY_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN MB");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = MB.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'MAIL_NOTIFY'");
            sb.ApdL("                         AND COM2.VALUE1 = MB.MAIL_NOTIFY");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       MB.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MB.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

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

    #region 物件名取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukkenLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MB.ISSUED_TAG_NO");
            sb.ApdL("     , MB.VERSION");
            sb.ApdL("     , MB.MAINTE_VERSION");
            sb.ApdL("     , MB.MAIL_NOTIFY");
            sb.ApdL("     , COM2.ITEM_NAME AS MAIL_NOTIFY_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN MB");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SHUKKA_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = MB.SHUKKA_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'MAIL_NOTIFY'");
            sb.ApdL("                         AND COM2.VALUE1 = MB.MAIL_NOTIFY");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       MB.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND MB.BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName + "%"));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));

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

    #region 指定物件名称の物件名情報数を取得(完全一致・物件名称必須)

    /// --------------------------------------------------
    /// <summary>
    /// 指定物件名称の物件名情報数を取得(完全一致・物件名称必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ件数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetBukkenNameCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN MB");
            sb.ApdL(" WHERE");
            sb.ApdN("       MB.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MB.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            if (!string.IsNullOrEmpty(cond.BukkenNo))
            {
                sb.ApdN("   AND MB.BUKKEN_NO <> ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));

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

    #region 指定物件管理Noの納入先情報数を取得(完全一致・物件管理No必須)

    /// --------------------------------------------------
    /// <summary>
    /// 指定物件管理Noの納入先情報数を取得(完全一致・物件管理No必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>納入先マスタ件数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetBukkenNonyusakiCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL(" WHERE");
            sb.ApdN("       MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND MN.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

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

    #region 物件メールマスタ(共通)の取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ(共通)の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="kbn">MAIL区分</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/20</create>
    /// <update>Y.Nakasato 2019/07/09 メール区分を指定できるように修正</update>
    /// --------------------------------------------------
    public int GetBukkenMailCount(DatabaseHelper dbHelper, CondM01 cond, string kbn)
    {
        try
        {
            var dt = new DataTable();
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" WHERE SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND MAIL_KBN = @MAIL_KBN");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", kbn));

            dbHelper.Fill(sb.ToString(), pc, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 基本メール設定宛先数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="kbn"></param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetBasicNotifyCount(DatabaseHelper dbHelper, CondM01 cond, string kbn)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT COUNT(1) AS CNT");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" INNER JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdN(" WHERE M_BUKKEN_MAIL.MAIL_KBN IN (").ApdN(this.BindPrefix).ApdN("MAIL_KBN").ApdL(")");

            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", kbn));

            dbHelper.Fill(string.Format(sb.ToString(), this.BindPrefix), pc, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT);
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
    /// 物件名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsBukken(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_BUKKEN");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , ISSUED_TAG_NO");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , MAINTE_VERSION");
            sb.ApdL("     , MAIL_NOTIFY");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ISSUED_TAG_NO");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_NOTIFY");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("ISSUED_TAG_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.ISSUED_TAG_NO, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_NOTIFY", ComFunc.GetFldObject(dr, Def_M_BUKKEN.MAIL_NOTIFY)));

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
    /// 物件名の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update>J.Chen 2022/09/22 受注No.追加</update>
    /// --------------------------------------------------
    public int UpdBukken(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_BUKKEN");
            sb.ApdL("SET");
            sb.ApdN("       BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , MAINTE_VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAIL_NOTIFY = ").ApdN(this.BindPrefix).ApdL("MAIL_NOTIFY");
            sb.ApdN("     , JUCHU_NO = ").ApdN(this.BindPrefix).ApdL("JUCHU_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_NOTIFY", ComFunc.GetFldObject(dr, Def_M_BUKKEN.MAIL_NOTIFY)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("JUCHU_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.JUCHU_NO)));

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
    /// 物件名(納入先名)の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBukkenNonyusaki(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI");
            sb.ApdL("SET");
            sb.ApdN("       NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_NAME", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NO)));

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
    /// 物件名の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Saeki 2012/03/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBukken(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_BUKKEN");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NO)));

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

    #region M0100070:名称マスタ保守

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// 名称取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isUseVersion">バージョンを用いて取得を行うかどうか</param>
    /// <returns>名称マスタ</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSelectItem(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT SELECT_GROUP_CD");
            sb.ApdL("      ,ITEM_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL("  FROM M_SELECT_ITEM");
            sb.ApdN(" WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            sb.ApdN("   AND ITEM_NAME = ").ApdN(this.BindPrefix).ApdL("ITEM_NAME");
            sb.ApdL(" ORDER BY ITEM_NAME");

            paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCD));
            paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME", cond.ItemName));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 名称取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>名称マスタ</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSelectItemLike(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var ds = new DataSet();
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("SELECT SELECT_GROUP_CD");
            sb.ApdL("      ,ITEM_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL("  FROM M_SELECT_ITEM");
            sb.ApdN(" WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            if (!string.IsNullOrEmpty(cond.ItemName))
            {
                sb.ApdN("   AND ITEM_NAME LIKE ").ApdN(this.BindPrefix).ApdL("ITEM_NAME");
                paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME", cond.ItemName + "%"));
            }
            sb.ApdL(" ORDER BY ITEM_NAME");

            paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCD));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);
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
    /// 名称の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">追加データ</param>
    /// <returns>追加行数</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsSelectItem(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO M_SELECT_ITEM (");
            sb.ApdL("       SELECT_GROUP_CD");
            sb.ApdL("      ,ITEM_NAME");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,MAINTE_USER_ID");
            sb.ApdL("      ,MAINTE_USER_NAME");
            sb.ApdL("      ,MAINTE_DATE");
            sb.ApdL("      ,VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("ITEM_NAME");
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

            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", ComFunc.GetFldObject(dr, Def_M_SELECT_ITEM.SELECT_GROUP_CD)));
                paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME", ComFunc.GetFldObject(dr, Def_M_SELECT_ITEM.ITEM_NAME)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));

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
    /// 名称の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>更新行数</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdSelectItem(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE M_SELECT_ITEM");
            sb.ApdN("   SET SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            sb.ApdN("      ,ITEM_NAME = ").ApdN(this.BindPrefix).ApdL("ITEM_NAME");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("      ,VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD2");
            sb.ApdN("   AND ITEM_NAME = ").ApdN(this.BindPrefix).ApdL("ITEM_NAME2");

            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", ComFunc.GetFldObject(dr, Def_M_SELECT_ITEM.SELECT_GROUP_CD)));
                paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME", ComFunc.GetFldObject(dr, Def_M_SELECT_ITEM.ITEM_NAME)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD2", cond.SelectGroupCD));
                paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME2", cond.ItemName));

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

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 名称の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>削除行数</returns>
    /// <create>T.Sakiori 2012/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelSelectItem(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_SELECT_ITEM");
            sb.ApdN(" WHERE SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
            sb.ApdN("   AND ITEM_NAME = ").ApdN(this.BindPrefix).ApdL("ITEM_NAME");

            paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCD));
            paramCollection.Add(iNewParameter.NewDbParameter("ITEM_NAME", cond.ItemName));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100080:基本通知設定

    /// --------------------------------------------------
    /// <summary>
    /// メール設定取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/06</create>
    /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
    /// --------------------------------------------------
    public DataTable GetMailSetting(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_MAIL_SETTING.Name);
            var sb = new StringBuilder();

            sb.ApdL("SELECT BUKKEN_ADD_EVENT");
            sb.ApdL("     , AR_ADD_EVENT");
            sb.ApdL("     , AR_UPDATE_EVENT");
            sb.ApdL("     , TAG_RENKEI_EVENT");
            sb.ApdL("  FROM M_MAIL_SETTING");

            dbHelper.Fill(sb.ToString(), dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/07</create>
    /// <update>Y.Nakasato 2019/07/08 AR進捗通知対応</update>
    /// --------------------------------------------------
    public DataTable GetBasicNotify(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdN(" WHERE M_BUKKEN_MAIL.MAIL_KBN IN (").ApdN(this.BindPrefix).ApdN("MAIL_KBN_BUKKEN").ApdN(",").ApdN(this.BindPrefix).ApdN("MAIL_KBN_ARSHINCHOKU").ApdL(")");
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("MAIL_KBN_BUKKEN", MAIL_KBN.BUKKEN_VALUE1));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN_ARSHINCHOKU", MAIL_KBN.ARSHINCHOKU_VALUE1));

            dbHelper.Fill(string.Format(sb.ToString(), this.BindPrefix), pc, dt);
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
    /// <param name="kbn"></param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBasicNotifySelect(DatabaseHelper dbHelper, CondM01 cond, string kbn)
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
            sb.ApdN(" WHERE M_BUKKEN_MAIL.MAIL_KBN IN (").ApdN(this.BindPrefix).ApdN("MAIL_KBN").ApdL(")");
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", kbn));

            dbHelper.Fill(string.Format(sb.ToString(), this.BindPrefix), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 基本通知設定の保存
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">保存データ</param>
    /// <param name="errMsgId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update>D.Okumura 2019/07/08 AR進捗通知対応</update>
    /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
    /// --------------------------------------------------
    public bool SaveBasicNotify(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgId, ref string[] args)
    {
        try
        {

            // メール設定マスタ更新
            this.UpdMailSetting(dbHelper, cond, ds.Tables[Def_M_MAIL_SETTING.Name].Rows[0]);

            // メールヘッダID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;

            using (var impl = new WsSmsImpl())
            {
                // 物件メールマスタを基準に採番・更新準備を行う
                foreach (DataRow row in ds.Tables[Def_M_BUKKEN_MAIL.Name].Rows)
                {
                    string mailHeaderId;
                    if (!impl.GetSaiban(dbHelper, condSms, out mailHeaderId, out errMsgId))
                    {
                        return false;
                    }
                    // 旧メールヘッダIDの取得と、新メールヘッダIDの設定
                    string oldMailHeaderId = ComFunc.GetFld(row, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);
                    string mailKbn = ComFunc.GetFld(row, Def_M_BUKKEN_MAIL.MAIL_KBN);
                    UtilData.SetFld(row, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID, mailHeaderId);
                    foreach (DataRow meisai in ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name].Rows)
                    {
                        // 登録時は明細にメール区分を付与しているので、その値をキーにして判別する
                        string kbn = UtilData.GetFld(meisai, Def_M_BUKKEN_MAIL.MAIL_KBN, null);
                        if (kbn != null && !mailKbn.Equals(kbn))
                            continue;
                        UtilData.SetFld(meisai, Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID, mailHeaderId);
                    }
                    // 物件メールマスタ更新
                    this.UpdBukkenMail(dbHelper, cond, row);

                    // 物件メール明細マスタ削除
                    if (!string.IsNullOrEmpty(oldMailHeaderId))
                    {
                        this.DelBukkenMailMeisai(dbHelper, oldMailHeaderId);
                    }
                }
            }

            // 物件メール明細マスタ登録
            this.InsBukkenMailMeisai(dbHelper, cond, ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name]);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているメールヘッダIDです。
                errMsgId = "M0100080001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// メール設定マスタの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">更新データ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update>H.Tajimi 2019/08/16 TAG連携メール通知対応</update>
    /// --------------------------------------------------
    public int UpdMailSetting(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("UPDATE M_MAIL_SETTING");
            sb.ApdL("   SET BUKKEN_ADD_EVENT = @BUKKEN_ADD_EVENT");
            sb.ApdL("     , AR_ADD_EVENT = @AR_ADD_EVENT");
            sb.ApdL("     , AR_UPDATE_EVENT = @AR_UPDATE_EVENT");
            sb.ApdL("     , TAG_RENKEI_EVENT = @TAG_RENKEI_EVENT");
            sb.ApdL("     , UPDATE_DATE = SYSDATETIME()");
            sb.ApdL("     , UPDATE_USER_ID = @USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME = @USER_NAME");
            sb.ApdL("     , VERSION = SYSDATETIME()");

            pc.Add(iNewParam.NewDbParameter("BUKKEN_ADD_EVENT", ComFunc.GetFldObject(dr, Def_M_MAIL_SETTING.BUKKEN_ADD_EVENT)));
            pc.Add(iNewParam.NewDbParameter("AR_ADD_EVENT", ComFunc.GetFldObject(dr, Def_M_MAIL_SETTING.AR_ADD_EVENT)));
            pc.Add(iNewParam.NewDbParameter("AR_UPDATE_EVENT", ComFunc.GetFldObject(dr, Def_M_MAIL_SETTING.AR_UPDATE_EVENT)));
            pc.Add(iNewParam.NewDbParameter("TAG_RENKEI_EVENT", ComFunc.GetFldObject(dr, Def_M_MAIL_SETTING.TAG_RENKEI_EVENT)));
            pc.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("USER_NAME", this.GetUpdateUserName(cond)));

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">更新データ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBukkenMail(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("UPDATE M_BUKKEN_MAIL");
            sb.ApdL("   SET MAIL_HEADER_ID = @MAIL_HEADER_ID");
            sb.ApdL("     , UPDATE_DATE = SYSDATETIME()");
            sb.ApdL("     , UPDATE_USER_ID = @USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME = @USER_NAME");
            sb.ApdL("     , VERSION = SYSDATETIME()");
            sb.ApdL(" WHERE MAIL_KBN = @MAIL_KBN");
            if (ComFunc.GetFld(dr, Def_M_BUKKEN_MAIL.MAIL_KBN) == MAIL_KBN.COMMON_VALUE1)
            {
                sb.ApdL("   AND SHUKKA_FLAG = @SHUKKA_FLAG");
                sb.ApdL("   AND BUKKEN_NO = @BUKKEN_NO");

                pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.SHUKKA_FLAG)));
                pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.BUKKEN_NO)));
            }

            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID)));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.MAIL_KBN)));
            pc.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("USER_NAME", this.GetUpdateUserName(cond)));

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="mailHeaderId">メールヘッダID</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelBukkenMailMeisai(DatabaseHelper dbHelper, string mailHeaderId)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_BUKKEN_MAIL_MEISAI");
            sb.ApdL(" WHERE MAIL_HEADER_ID = @MAIL_HEADER_ID");

            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", mailHeaderId));

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">登録データ</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsBukkenMailMeisai(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("INSERT INTO M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("     (");
            sb.ApdL("       MAIL_HEADER_ID");
            sb.ApdL("     , MAIL_ADDRESS_FLAG");
            sb.ApdL("     , ORDER_NO");
            sb.ApdL("     , USER_ID");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     ) VALUES (");
            sb.ApdL("       @MAIL_HEADER_ID");
            sb.ApdL("     , @MAIL_ADDRESS_FLAG");
            sb.ApdL("     , @ORDER_NO");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @CREATE_USER_ID");
            sb.ApdL("     , @CREATE_USER_NAME");
            sb.ApdL("     )");

            int rec = 0;
            foreach (DataRow dr in dt.Rows)
            {
                pc = new DbParamCollection();

                pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID)));
                pc.Add(iNewParam.NewDbParameter("MAIL_ADDRESS_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG)));
                pc.Add(iNewParam.NewDbParameter("ORDER_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL_MEISAI.ORDER_NO)));
                pc.Add(iNewParam.NewDbParameter("USER_ID", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL_MEISAI.USER_ID)));
                pc.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetUpdateUserID(cond)));
                pc.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetUpdateUserName(cond)));
                rec += dbHelper.ExecuteNonQuery(sb.ToString(), pc);
            }

            return rec;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// メールアドレス変更権限を保持しているか
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">ログインユーザー</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ExistsMailChangeRole(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_USER.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT MAIL_CHANGE_ROLE");
            sb.ApdL("  FROM M_USER");
            sb.ApdL(" WHERE USER_ID = @USER_ID");

            pc.Add(iNewParam.NewDbParameter("USER_ID", cond.LoginInfo.UserID));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return UtilData.GetFld(dt, 0, Def_M_USER.MAIL_CHANGE_ROLE) == MAIL_CHANGE_ROLE.ARI_VALUE1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region M0100090:共通通知設定

    /// --------------------------------------------------
    /// <summary>
    /// 共通通知設定取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetCommonNotify(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" INNER JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.COMMON_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 共通通知設定の保存
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">保存データ</param>
    /// <param name="errMsgId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveCommonNotify(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgId, ref string[] args)
    {
        try
        {
            // メールヘッダID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string mailHeaderId;

            using (var impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out mailHeaderId, out errMsgId))
                {
                    return false;
                }
            }

            // 旧メールヘッダIDの取得と、新メールヘッダIDの設定
            string oldMailHeaderId = ComFunc.GetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);
            UtilData.SetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID, mailHeaderId);
            for (int i = 0; i < ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name].Rows.Count; i++)
            {
                UtilData.SetFld(ds, Def_M_BUKKEN_MAIL_MEISAI.Name, i, Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID, mailHeaderId);
            }

            // 物件メールマスタ更新
            this.MergeBukkenMail(dbHelper, cond, ds.Tables[Def_M_BUKKEN_MAIL.Name].Rows[0], oldMailHeaderId);

            // 物件メール明細マスタ削除
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                this.DelBukkenMailMeisai(dbHelper, oldMailHeaderId);
            }

            // 物件メール明細マスタ登録
            this.InsBukkenMailMeisai(dbHelper, cond, ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name]);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているメールヘッダIDです。
                errMsgId = "M0100080001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタの登録/更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">登録/更新内容</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public int MergeBukkenMail(DatabaseHelper dbHelper, CondM01 cond, DataRow dr, string oldMailHeaderId)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL(" MERGE INTO M_BUKKEN_MAIL");
            sb.ApdL(" USING (SELECT @SHUKKA_FLAG AS SHUKKA_FLAG");
            sb.ApdL("             , @BUKKEN_NO AS BUKKEN_NO");
            sb.ApdL("             , @MAIL_KBN AS MAIL_KBN");
            if (ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.LIST_FLAG) == DBNull.Value)
            {
                sb.ApdL("             , NULL AS LIST_FLAG");
            }
            else
            {
                sb.ApdL("             , @LIST_FLAG AS LIST_FLAG");
            }
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                sb.ApdL("             , @OLD_MAIL_HEADER_ID AS OLD_MAIL_HEADER_ID");
            }
            sb.ApdL("    ) AS TMP");
            sb.ApdL("    ON (");
            sb.ApdL("       M_BUKKEN_MAIL.SHUKKA_FLAG = TMP.SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = TMP.BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = TMP.MAIL_KBN");
            if (ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.LIST_FLAG) == DBNull.Value)
            {
                sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG IS NULL");
            }
            else
            {
                sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG = TMP.LIST_FLAG");
            }
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_HEADER_ID = TMP.OLD_MAIL_HEADER_ID");
            }
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = TMP.MAIL_KBN");
            sb.ApdL("       )");
            sb.ApdL("  WHEN MATCHED THEN");
            sb.ApdL("UPDATE");
            sb.ApdL("   SET MAIL_HEADER_ID = @MAIL_HEADER_ID");
            sb.ApdL("     , UPDATE_DATE = SYSDATETIME()");
            sb.ApdL("     , UPDATE_USER_ID = @USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME = @USER_NAME");
            sb.ApdL("     , VERSION = SYSDATETIME()");
            sb.ApdL("  WHEN NOT MATCHED THEN");
            sb.ApdL("INSERT (");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , MAIL_KBN");
            sb.ApdL("     , LIST_FLAG");
            sb.ApdL("     , MAIL_HEADER_ID");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("       )");
            sb.ApdL("VALUES (");
            sb.ApdL("       @SHUKKA_FLAG");
            sb.ApdL("     , @BUKKEN_NO");
            sb.ApdL("     , @MAIL_KBN");
            if (ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.LIST_FLAG) == DBNull.Value)
            {
                sb.ApdL("     , NULL");
            }
            else
            {
                sb.ApdL("     , @LIST_FLAG");
            }
            sb.ApdL("     , @MAIL_HEADER_ID");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , @USER_NAME");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , @USER_NAME");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("       );");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.SHUKKA_FLAG)));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.BUKKEN_NO)));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.MAIL_KBN)));
            if (ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.LIST_FLAG) != DBNull.Value)
            {
                pc.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.LIST_FLAG)));
            }
            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", ComFunc.GetFldObject(dr, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID)));
            pc.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("USER_NAME", this.GetUpdateUserName(cond)));
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                pc.Add(iNewParam.NewDbParameter("OLD_MAIL_HEADER_ID", oldMailHeaderId));
            }

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region M0100200:計画取込Mail通知先設定

    /// --------------------------------------------------
    /// <summary>
    /// 計画取組通知設定取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetKeikakuTorikomiNotify(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_CONSIGN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT M_CONSIGN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("     , M_CONSIGN_MAIL_MEISAI.USER_ID");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("     , M_USER.MAIL_ADDRESS");
            sb.ApdL("     , M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("  FROM M_CONSIGN_MAIL");
            sb.ApdL(" INNER JOIN M_CONSIGN_MAIL_MEISAI");
            sb.ApdL("    ON M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID = M_CONSIGN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_CONSIGN_MAIL_MEISAI.USER_ID");
            sb.ApdL(" WHERE M_CONSIGN_MAIL.CONSIGN_CD = @CONSIGN_CD");
            sb.ApdL(" ORDER BY M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_CONSIGN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 計画取組通知設定の保存
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">保存データ</param>
    /// <param name="errMsgId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveKeikakuTorikomiNotify(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgId, ref string[] args)
    {
        try
        {
            // メールヘッダID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string mailHeaderId;

            using (var impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out mailHeaderId, out errMsgId))
                {
                    return false;
                }
            }

            // 旧メールヘッダIDの取得と、新メールヘッダIDの設定
            string oldMailHeaderId = ComFunc.GetFld(ds, Def_M_CONSIGN_MAIL.Name, 0, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID);
            UtilData.SetFld(ds, Def_M_CONSIGN_MAIL.Name, 0, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID, mailHeaderId);
            for (int i = 0; i < ds.Tables[Def_M_CONSIGN_MAIL_MEISAI.Name].Rows.Count; i++)
            {
                UtilData.SetFld(ds, Def_M_CONSIGN_MAIL_MEISAI.Name, i, Def_M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID, mailHeaderId);
            }

            // 荷受メールマスタ更新
            this.MergeKeikakuTorikomiMail(dbHelper, cond, ds.Tables[Def_M_CONSIGN_MAIL.Name].Rows[0], oldMailHeaderId);

            // 荷受メール明細マスタ削除
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                this.DelConsignMailMeisai(dbHelper, oldMailHeaderId);
            }

            // 荷受メール明細マスタ登録
            this.InsConsignMailMeisai(dbHelper, cond, ds.Tables[Def_M_CONSIGN_MAIL_MEISAI.Name]);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているメールヘッダIDです。
                errMsgId = "M0100080001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 荷受メールマスタの登録/更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">登録/更新内容</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public int MergeKeikakuTorikomiMail(DatabaseHelper dbHelper, CondM01 cond, DataRow dr, string oldMailHeaderId)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL(" MERGE INTO M_CONSIGN_MAIL");
            sb.ApdL(" USING (SELECT @CONSIGN_CD AS CONSIGN_CD");

            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                sb.ApdL("             , @OLD_MAIL_HEADER_ID AS OLD_MAIL_HEADER_ID");
            }
            sb.ApdL("    ) AS TMP");
            sb.ApdL("    ON (");
            sb.ApdL("       M_CONSIGN_MAIL.CONSIGN_CD = TMP.CONSIGN_CD");

            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                sb.ApdL("   AND M_CONSIGN_MAIL.MAIL_HEADER_ID = TMP.OLD_MAIL_HEADER_ID");
            }
            sb.ApdL("       )");
            sb.ApdL("  WHEN MATCHED THEN");
            sb.ApdL("UPDATE");
            sb.ApdL("   SET MAIL_HEADER_ID = @MAIL_HEADER_ID");
            sb.ApdL("     , UPDATE_DATE = SYSDATETIME()");
            sb.ApdL("     , UPDATE_USER_ID = @USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME = @USER_NAME");
            sb.ApdL("     , VERSION = SYSDATETIME()");
            sb.ApdL("  WHEN NOT MATCHED THEN");
            sb.ApdL("INSERT (");
            sb.ApdL("       CONSIGN_CD");
            sb.ApdL("     , MAIL_HEADER_ID");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("       )");
            sb.ApdL("VALUES (");
            sb.ApdL("       @CONSIGN_CD");
            sb.ApdL("     , @MAIL_HEADER_ID");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , @USER_NAME");


            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , @USER_NAME");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("       );");

            pc.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL.CONSIGN_CD)));
            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL.MAIL_HEADER_ID)));
            pc.Add(iNewParam.NewDbParameter("USER_ID", this.GetUpdateUserID(cond)));
            pc.Add(iNewParam.NewDbParameter("USER_NAME", this.GetUpdateUserName(cond)));
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                pc.Add(iNewParam.NewDbParameter("OLD_MAIL_HEADER_ID", oldMailHeaderId));
            }

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region 荷受メールDML

    #region　荷受メール明細マスタ削除
    /// --------------------------------------------------
    /// <summary>
    /// 物件メール明細マスタの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="mailHeaderId">メールヘッダID</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelConsignMailMeisai(DatabaseHelper dbHelper, string mailHeaderId)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_CONSIGN_MAIL_MEISAI");
            sb.ApdL(" WHERE MAIL_HEADER_ID = @MAIL_HEADER_ID");

            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", mailHeaderId));

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region　荷受メールマスタ削除
    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="mailHeaderId">メールヘッダID</param>
    /// <returns></returns>
    /// <create>J.Chen 2024/01/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelConsignMail(DatabaseHelper dbHelper, string mailHeaderId)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_CONSIGN_MAIL");
            sb.ApdL(" WHERE MAIL_HEADER_ID = @MAIL_HEADER_ID");

            pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", mailHeaderId));

            return dbHelper.ExecuteNonQuery(sb.ToString(), pc);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region　荷受メール明細マスタの登録
    /// --------------------------------------------------
    /// <summary>
    /// 荷受メール明細マスタの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">登録データ</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsConsignMailMeisai(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("INSERT INTO M_CONSIGN_MAIL_MEISAI");
            sb.ApdL("     (");
            sb.ApdL("       MAIL_HEADER_ID");
            sb.ApdL("     , MAIL_ADDRESS_FLAG");
            sb.ApdL("     , ORDER_NO");
            sb.ApdL("     , USER_ID");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     ) VALUES (");
            sb.ApdL("       @MAIL_HEADER_ID");
            sb.ApdL("     , @MAIL_ADDRESS_FLAG");
            sb.ApdL("     , @ORDER_NO");
            sb.ApdL("     , @USER_ID");
            sb.ApdL("     , SYSDATETIME()");
            sb.ApdL("     , @CREATE_USER_ID");
            sb.ApdL("     , @CREATE_USER_NAME");
            sb.ApdL("     )");

            int rec = 0;
            foreach (DataRow dr in dt.Rows)
            {
                pc = new DbParamCollection();

                pc.Add(iNewParam.NewDbParameter("MAIL_HEADER_ID", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL_MEISAI.MAIL_HEADER_ID)));
                pc.Add(iNewParam.NewDbParameter("MAIL_ADDRESS_FLAG", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL_MEISAI.MAIL_ADDRESS_FLAG)));
                pc.Add(iNewParam.NewDbParameter("ORDER_NO", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL_MEISAI.ORDER_NO)));
                pc.Add(iNewParam.NewDbParameter("USER_ID", ComFunc.GetFldObject(dr, Def_M_CONSIGN_MAIL_MEISAI.USER_ID)));
                pc.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetUpdateUserID(cond)));
                pc.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetUpdateUserName(cond)));
                rec += dbHelper.ExecuteNonQuery(sb.ToString(), pc);
            }

            return rec;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region M0100100:AR List単位通知設定

    #region SELECT

    #region リスト区分取得(項目が空のデータ除く)

    /// --------------------------------------------------
    /// <summary>
    /// リスト区分取得(項目が空のデータ除く)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/15</create>
    /// <update>K.Tsutsumi 2019/07/23 AR用納入先も見る</update>
    /// --------------------------------------------------
    public DataTable GetListFlag(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(LIST_FLAG.GROUPCD);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("     LF.GROUP_CD");
            sb.ApdL("    ,LF.GROUP_NAME");
            sb.ApdL("    ,ITEM_CD");
            sb.ApdL("    ,CASE");
            sb.ApdL("        WHEN ISNULL(LF.LIST_FLAG_NAME, '') != '' THEN LF.LIST_FLAG_NAME");
            sb.ApdL("        ELSE LF.ITEM_NAME");
            sb.ApdL("    END ITEM_NAME");
            sb.ApdL("    ,LF.VALUE1");
            sb.ApdL("    ,LF.VALUE2");
            sb.ApdL("    ,LF.VALUE3");
            sb.ApdL("    ,LF.DISP_NO");
            sb.ApdL("    ,LF.DEFAULT_VALUE");
            sb.ApdL("    ,LF.VERSION");
            sb.ApdL("FROM");
            sb.ApdL("    (");
            sb.ApdL("        SELECT");
            sb.ApdL("             MC.GROUP_CD");
            sb.ApdL("            ,MC.GROUP_NAME");
            sb.ApdL("            ,MC.ITEM_CD");
            sb.ApdL("            ,MC.ITEM_NAME");
            sb.ApdL("            ,MC.VALUE1");
            sb.ApdL("            ,MC.VALUE2");
            sb.ApdL("            ,MC.VALUE3");
            sb.ApdL("            ,MC.DISP_NO");
            sb.ApdL("            ,MC.DEFAULT_VALUE");
            sb.ApdL("            ,MC.VERSION");
            sb.ApdL("            ,CASE MC.VALUE1");
            sb.ApdL("                WHEN @FLAG_0_VALUE1 THEN MN.LIST_FLAG_NAME0");
            sb.ApdL("                WHEN @FLAG_1_VALUE1 THEN MN.LIST_FLAG_NAME1");
            sb.ApdL("                WHEN @FLAG_2_VALUE1 THEN MN.LIST_FLAG_NAME2");
            sb.ApdL("                WHEN @FLAG_3_VALUE1 THEN MN.LIST_FLAG_NAME3");
            sb.ApdL("                WHEN @FLAG_4_VALUE1 THEN MN.LIST_FLAG_NAME4");
            sb.ApdL("                WHEN @FLAG_5_VALUE1 THEN MN.LIST_FLAG_NAME5");
            sb.ApdL("                WHEN @FLAG_6_VALUE1 THEN MN.LIST_FLAG_NAME6");
            sb.ApdL("                ELSE MN.LIST_FLAG_NAME7");
            sb.ApdL("            END AS LIST_FLAG_NAME");
            sb.ApdL("        FROM");
            sb.ApdL("            M_COMMON MC");
            sb.ApdL("            LEFT JOIN M_NONYUSAKI MN");
            sb.ApdL("            ON  MN.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("            AND MN.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("        WHERE");
            sb.ApdL("            MC.GROUP_CD = @GROUP_CD");
            sb.ApdL("        AND LANG = @LANG");
            sb.ApdL("    ) AS LF");
            sb.ApdL("WHERE");
            sb.ApdL("    (");
            sb.ApdL("        ISNULL(LF.ITEM_NAME, '') != ''");
            sb.ApdL("    OR  ISNULL(LF.LIST_FLAG_NAME, '') != ''");
            sb.ApdL("    )");
            sb.ApdL("ORDER BY");
            sb.ApdL("    DISP_NO");

            pc.Add(iNewParam.NewDbParameter("FLAG_0_VALUE1", LIST_FLAG.FLAG_0_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_1_VALUE1", LIST_FLAG.FLAG_1_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_2_VALUE1", LIST_FLAG.FLAG_2_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_3_VALUE1", LIST_FLAG.FLAG_3_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_4_VALUE1", LIST_FLAG.FLAG_4_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_5_VALUE1", LIST_FLAG.FLAG_5_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_6_VALUE1", LIST_FLAG.FLAG_6_VALUE1));
            pc.Add(iNewParam.NewDbParameter("FLAG_7_VALUE1", LIST_FLAG.FLAG_7_VALUE1));
            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            pc.Add(iNewParam.NewDbParameter("GROUP_CD", LIST_FLAG.GROUPCD));

            dbHelper.Fill(sb.ToString(), pc, dt);
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
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>T.Sakiori 2017/09/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetARListNotify(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            sb.ApdL(" INNER JOIN M_BUKKEN_MAIL_MEISAI");
            sb.ApdL("    ON M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID = M_BUKKEN_MAIL.MAIL_HEADER_ID");
            sb.ApdL("  LEFT JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = M_BUKKEN_MAIL_MEISAI.USER_ID");
            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
            sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG = @LIST_FLAG");
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.ARLIST_VALUE1));
            pc.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 物件メールマスタ取得（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// 物件メールマスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2020/01/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetAndLockARListNotify(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT *");
            sb.ApdL("  FROM M_BUKKEN_MAIL");
            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }
            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
            sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG = @LIST_FLAG");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.ARLIST_VALUE1));
            pc.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));

            dbHelper.Fill(sb.ToString(), pc, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region DELETE

    #region AR List単位通知設定削除(制御)
    
    /// --------------------------------------------------
    /// <summary>
    /// AR List単位通知設定削除(制御)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">保存データ</param>
    /// <param name="errMsgId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>K.Tsutsumi 2020/01/03</create>
    /// <update></update>
    /// <remarks>このマスタは後勝ちの洗い替えマスタなのでバージョンチェックは行わないこととする</remarks>
    /// --------------------------------------------------
    public bool DelARListNotifyData(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgId, ref string[] args)
    {
        try
        {
            cond.BukkenNo = ComFunc.GetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.BUKKEN_NO);
            cond.ListFlag = ComFunc.GetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.LIST_FLAG);

            // 存在チェック＆ヘッダのロック
            DataTable dtARListNotify = this.GetAndLockARListNotify(dbHelper, cond, true);
            if ((dtARListNotify == null) || (dtARListNotify.Rows.Count == 0))
            {
                // 他端末で削除されています。
                errMsgId = "A9999999026";
                return false;
            }

            // 旧メールヘッダIDの取得
            string oldMailHeaderId = ComFunc.GetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);

            // 物件メール明細マスタ削除
            this.DelBukkenMailMeisai(dbHelper, oldMailHeaderId);

            // 物件メールマスタ削除(AR List指定)
            this.DelARListNotify(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region AR List単位通知設定削除(AR List指定)
    /// --------------------------------------------------
    /// <summary>
    /// AR List単位通知設定削除(AR List指定)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Tsutsumi 2020/01/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelARListNotify(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_BUKKEN_MAIL");
            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
            sb.ApdL("   AND M_BUKKEN_MAIL.LIST_FLAG = @LIST_FLAG");

            // バインド変数設定
            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.ARLIST_VALUE1));
            pc.Add(iNewParam.NewDbParameter("LIST_FLAG", cond.ListFlag));


            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), pc);
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

    #region M0100110:メール送信情報メンテナンス

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update>H.Tajimi 2018/12/07 添付ファイル対応</update>
    /// --------------------------------------------------
    public DataTable GetMail(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_T_MAIL.Name);
            var sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT T_MAIL.MAIL_ID");
            sb.ApdL("     , T_MAIL.MAIL_SEND");
            sb.ApdL("     , T_MAIL.MAIL_SEND_DISPLAY");
            sb.ApdL("     , T_MAIL.MAIL_TO");
            sb.ApdL("     , T_MAIL.MAIL_TO_DISPLAY");
            sb.ApdL("     , T_MAIL.MAIL_CC");
            sb.ApdL("     , T_MAIL.MAIL_CC_DISPLAY");
            sb.ApdL("     , T_MAIL.MAIL_BCC");
            sb.ApdL("     , T_MAIL.MAIL_BCC_DISPLAY");
            sb.ApdL("     , T_MAIL.TITLE");
            sb.ApdL("     , T_MAIL.NAIYO");
            sb.ApdL("     , T_MAIL.MAIL_STATUS");
            sb.ApdL("     , M_COMMON.ITEM_NAME AS MAIL_STATUS_NAME");
            sb.ApdL("     , T_MAIL.RETRY_COUNT");
            sb.ApdL("     , T_MAIL.REASON");
            sb.ApdL("     , T_MAIL.CREATE_DATE");
            sb.ApdL("     , T_MAIL.CREATE_USER_ID");
            sb.ApdL("     , T_MAIL.CREATE_USER_NAME");
            sb.ApdL("     , T_MAIL.UPDATE_DATE");
            sb.ApdL("     , T_MAIL.UPDATE_USER_ID");
            sb.ApdL("     , T_MAIL.UPDATE_USER_NAME");
            sb.ApdL("     , T_MAIL.VERSION");
            sb.ApdL("     , COM2.ITEM_NAME AS DISP_REASON");
            sb.ApdL("     , T_MAIL.APPENDIX_FILE_PATH");
            sb.ApdL("  FROM T_MAIL");
            sb.ApdL("  LEFT JOIN M_COMMON");
            sb.ApdL("    ON M_COMMON.GROUP_CD = 'MAIL_STATUS'");
            sb.ApdL("   AND M_COMMON.VALUE1 = T_MAIL.MAIL_STATUS");
            sb.ApdN("   AND M_COMMON.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("    ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_SEND_MAIL_FAILURE_REASON");
            sb.ApdL("   AND COM2.VALUE1 = T_MAIL.DISP_REASON");
            sb.ApdN("   AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE T_MAIL.CREATE_DATE BETWEEN ").ApdN(this.BindPrefix).ApdN("DATE_FROM AND ").ApdN(this.BindPrefix).ApdL("DATE_TO");
            if (cond.MailStatus != MAIL_STATUS.ALL_VALUE1)
            {
                sb.ApdN("   AND T_MAIL.MAIL_STATUS = ").ApdN(this.BindPrefix).ApdL("MAIL_STATUS");
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_STATUS", cond.MailStatus));
            }
            sb.ApdL(" ORDER BY T_MAIL.CREATE_DATE");
            sb.ApdL("     , T_MAIL.MAIL_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("GC_SEND_MAIL_FAILURE_REASON", SEND_MAIL_FAILURE_REASON.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("DATE_FROM", cond.DateFrom));
            paramCollection.Add(iNewParam.NewDbParameter("DATE_TO", cond.DateTo));

            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メール再送信

    /// --------------------------------------------------
    /// <summary>
    /// メール再送信
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">追加データ</param>
    /// <param name="mailID">メールID</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update>H.Tajimi 2018/12/07 添付ファイル対応</update>
    /// --------------------------------------------------
    public int InsMailRetry(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, decimal mailID)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_MAIL");
            sb.ApdL("(");
            sb.ApdL("       MAIL_ID");
            sb.ApdL("     , MAIL_SEND");
            sb.ApdL("     , MAIL_SEND_DISPLAY");
            sb.ApdL("     , MAIL_TO");
            sb.ApdL("     , MAIL_TO_DISPLAY");
            sb.ApdL("     , MAIL_CC");
            sb.ApdL("     , MAIL_CC_DISPLAY");
            sb.ApdL("     , MAIL_BCC");
            sb.ApdL("     , MAIL_BCC_DISPLAY");
            sb.ApdL("     , TITLE");
            sb.ApdL("     , NAIYO");
            sb.ApdL("     , MAIL_STATUS");
            sb.ApdL("     , RETRY_COUNT");
            sb.ApdL("     , REASON");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , LANG");
            sb.ApdL("     , APPENDIX_FILE_PATH");
            sb.ApdL("     , DISP_REASON");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("MAIL_ID");
            sb.ApdL("     , MAIL_SEND");
            sb.ApdL("     , MAIL_SEND_DISPLAY");
            sb.ApdL("     , MAIL_TO");
            sb.ApdL("     , MAIL_TO_DISPLAY");
            sb.ApdL("     , MAIL_CC");
            sb.ApdL("     , MAIL_CC_DISPLAY");
            sb.ApdL("     , MAIL_BCC");
            sb.ApdL("     , MAIL_BCC_DISPLAY");
            sb.ApdL("     , TITLE");
            sb.ApdL("     , NAIYO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("RETRY_COUNT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REASON");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , LANG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("APPENDIX_FILE_PATH");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DISP_REASON");
            sb.ApdL("  FROM T_MAIL");
            sb.ApdN(" WHERE MAIL_ID = ").ApdN(this.BindPrefix).ApdL("MAIL_ID_FROM");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_ID", mailID));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_STATUS", MAIL_STATUS.MI_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("RETRY_COUNT", 0));
                paramCollection.Add(iNewParam.NewDbParameter("REASON", string.Empty));
                paramCollection.Add(iNewParam.NewDbParameter("DISP_REASON", string.Empty));
                paramCollection.Add(iNewParam.NewDbParameter("USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("APPENDIX_FILE_PATH", ComFunc.GetFldObject(dr, Def_T_MAIL.APPENDIX_FILE_PATH, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_ID_FROM", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_ID, string.Empty)));

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

    #region メール強制終了

    /// --------------------------------------------------
    /// <summary>
    /// メール強制終了
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>R.Katsuo 2017/09/13</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdMailAbort(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_MAIL");
            sb.ApdN("   SET MAIL_STATUS = ").ApdN(this.BindPrefix).ApdL("MAIL_STATUS");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN(" WHERE MAIL_ID = ").ApdN(this.BindPrefix).ApdL("MAIL_ID");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_STATUS", MAIL_STATUS.ABORT_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAIL_ID", ComFunc.GetFldObject(dr, Def_T_MAIL.MAIL_ID, string.Empty)));

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

    #region M0100120:物件名保守

    #region SELECT

    #region 物件名保守一括の物件名取得(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の物件名取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tsuji 2018/12/14</create>
    /// <update>J.Chen 2022/09/22 受注No.追加</update>
    /// --------------------------------------------------
    public DataSet GetBukkenIkkatsuLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MP.PROJECT_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdN("     , MB1.ISSUED_TAG_NO AS ").ApdL(ComDefine.FLD_NORMAL_ISSUED_TAG_NO);
            sb.ApdN("     , MB2.ISSUED_TAG_NO AS ").ApdL(ComDefine.FLD_AR_ISSUED_TAG_NO);
            sb.ApdN("     , MB2.MAIL_NOTIFY AS ").ApdL(ComDefine.FLD_AR_MAIL_NOTIFY);
            sb.ApdN("     , COM.ITEM_NAME AS ").ApdL(ComDefine.FLD_AR_MAIL_NOTIFY_NAME);
            sb.ApdL("     , MB2.JUCHU_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PROJECT MP");
            sb.ApdL("  LEFT JOIN M_BUKKEN MB1");
            sb.ApdL("         ON MB1.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdN("        AND MB1.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("MB1_SHUKKA_FLAG");
            sb.ApdL("  LEFT JOIN M_BUKKEN MB2");
            sb.ApdL("         ON MB2.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdN("        AND MB2.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("MB2_SHUKKA_FLAG");
            sb.ApdN("  LEFT JOIN M_COMMON COM");
            sb.ApdN("         ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("COM_GROUP_CD");
            sb.ApdL("        AND COM.VALUE1 = MB2.MAIL_NOTIFY");
            sb.ApdN("        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND MP.BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("MP_BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("MP_BUKKEN_NAME", cond.BukkenName + "%"));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MP.PROJECT_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("MB1_SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MB2_SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("COM_GROUP_CD", MAIL_NOTIFY.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, ComDefine.DTTBL_DISP_BUKKEN);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名保守一括の物件名取得(完全一致・ProjectNo必須)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の物件名取得(完全一致・ProjectNo必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tsuji 2018/12/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukkenIkkatsuSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MP.PROJECT_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , MP.VERSION");
            sb.ApdN("     , MB1.BUKKEN_NO AS ").ApdL(ComDefine.FLD_NORMAL_BUKKEN_NO);
            sb.ApdN("     , MB1.ISSUED_TAG_NO AS ").ApdL(ComDefine.FLD_NORMAL_ISSUED_TAG_NO);
            sb.ApdN("     , MB1.MAINTE_VERSION AS ").ApdL(ComDefine.FLD_NORMAL_MAINTE_VERSION);
            sb.ApdN("     , MB1.MAIL_NOTIFY AS ").ApdL(ComDefine.FLD_NORMAL_MAIL_NOTIFY);
            sb.ApdN("     , MB2.BUKKEN_NO AS ").ApdL(ComDefine.FLD_AR_BUKKEN_NO);
            sb.ApdN("     , MB2.ISSUED_TAG_NO AS ").ApdL(ComDefine.FLD_AR_ISSUED_TAG_NO);
            sb.ApdN("     , MB2.MAINTE_VERSION AS ").ApdL(ComDefine.FLD_AR_MAINTE_VERSION);
            sb.ApdN("     , MB2.MAIL_NOTIFY AS ").ApdL(ComDefine.FLD_AR_MAIL_NOTIFY);

            //【Step_1_11】納入先の物件名登録　重複登録防止　2022/10/12（TW-Tsuji）
            //　フィールド 受注No（JUCHU_NO） をクエリに追加
            sb.ApdN("     , MB2.JUCHU_NO AS ").ApdL(Def_M_BUKKEN.JUCHU_NO);

            sb.ApdL("  FROM");
            sb.ApdL("       M_PROJECT MP");
            sb.ApdL("  LEFT JOIN M_BUKKEN MB1");
            sb.ApdL("         ON MB1.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdN("        AND MB1.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("MB1_SHUKKA_FLAG");
            sb.ApdL("  LEFT JOIN M_BUKKEN MB2");
            sb.ApdL("         ON MB2.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdN("        AND MB2.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("MB2_SHUKKA_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("        MP.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("MP_PROJECT_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("MB1_SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MB2_SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("MP_PROJECT_NO", cond.ProjectNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, ComDefine.DTTBL_DISP_BUKKEN);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件名保守一括の物件名取得(ロック用)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の物件名取得(ロック用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet GetAndLockBukken(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MB.MAINTE_VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN MB");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       MB.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MB.SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            
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

    #region 物件名保守一括の受注No取得(ロック用)

    /// --------------------------------------------------
    /// <summary>
    /// 物件名保守一括の受注No取得(ロック用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>J.Chen 2022/10/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet GetAndLockJuchu(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MB.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.MAINTE_VERSION");
            sb.ApdL("     , MB.JUCHU_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN MB");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       MB.JUCHU_NO = ").ApdN(this.BindPrefix).ApdL("JUCHU_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MB.SHUKKA_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JUCHU_NO", cond.JuchuNo));

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

    #region 更新先の物件名数を取得

    /// --------------------------------------------------
    /// <summary>
    /// 更新先の物件名数を取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>プロジェクトマスタ件数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update>J.Chen 2022/11/08 受注Noチェック修正</update>
    /// --------------------------------------------------
    public int GetBukkenNameUpdateDestCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PROJECT MP");
            sb.ApdL(" WHERE");
            sb.ApdN("       MP.PROJECT_NO <> ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");

            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND MP.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            }

            if (!string.IsNullOrEmpty(cond.JuchuNo))
            {
                sb.ApdN("   AND MP.JUCHU_NO = ").ApdN(this.BindPrefix).ApdL("JUCHU_NO");
                paramCollection.Add(iNewParam.NewDbParameter("JUCHU_NO", cond.JuchuNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));

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

    #region 技連マスタの情報数を取得(完全一致・ProjectNo必須)

    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタの情報数を取得(完全一致・ProjectNo必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>技連マスタ件数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetEcsCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_ECS ME");
            sb.ApdL(" WHERE");
            sb.ApdN("       ME.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));

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

    #region ロケーションマスタの情報数を取得(完全一致・出荷区分・物件管理NO必須)

    /// --------------------------------------------------
    /// <summary>
    /// ロケーションマスタの情報数を取得(完全一致・出荷区分・物件管理NO必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>ロケーションマスタ件数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetLocationCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_LOCATION ML");
            sb.ApdL(" WHERE");
            sb.ApdN("       ML.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND ML.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));

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

    #endregion

    #region INSERT

    #region Projectマスタの追加

    /// --------------------------------------------------
    /// <summary>
    /// Projectマスタの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update>J.Chen 2022/11/08 受注No.追加</update>
    /// --------------------------------------------------
    public int InsProject(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_PROJECT");
            sb.ApdL("(");
            sb.ApdL("       PROJECT_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , JUCHU_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JUCHU_NO");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", ComFunc.GetFldObject(dr, Def_M_PROJECT.PROJECT_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_PROJECT.BUKKEN_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("JUCHU_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.JUCHU_NO)));

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

    #region 物件名の追加

    /// --------------------------------------------------
    /// <summary>
    /// 物件名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update>J.Chen 2022/09/22 受注No.追加</update>
    /// --------------------------------------------------
    public int InsBukkenIkkatsu(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_BUKKEN");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , ISSUED_TAG_NO");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , MAINTE_VERSION");
            sb.ApdL("     , MAIL_NOTIFY");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , JUCHU_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ISSUED_TAG_NO");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAIL_NOTIFY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JUCHU_NO");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_BUKKEN.BUKKEN_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("ISSUED_TAG_NO", 0));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAIL_NOTIFY", ComFunc.GetFldObject(dr, Def_M_BUKKEN.MAIL_NOTIFY)));
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.PROJECT_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("JUCHU_NO", ComFunc.GetFldObject(dr, Def_M_BUKKEN.JUCHU_NO)));

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

    /// --------------------------------------------------
    /// <summary>
    /// Projectマスタの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdProject(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_PROJECT");
            sb.ApdL("SET");
            sb.ApdN("       BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", ComFunc.GetFldObject(dr, Def_M_PROJECT.BUKKEN_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", ComFunc.GetFldObject(dr, Def_M_PROJECT.PROJECT_NO)));

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
    /// Projectの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelProject(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_PROJECT");
            sb.ApdL(" WHERE");
            sb.ApdN("       PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", cond.ProjectNo));

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

    #region M0100130:パーツ名翻訳マスタ

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名表示データ取得
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <returns>データセット</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPartsName(DatabaseHelper dbHelper, CondM01 cond)
    {
        return GetPartsName(dbHelper, cond, false);
    }
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名データ取得(ロック有無)
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <param name="isForUpdate">ロック有無</param>
    /// <returns>データセット</returns>
    /// <create>D.Okumura 2018/11/30</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet GetPartsName(DatabaseHelper dbHelper, CondM01 cond, bool isForUpdate)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("     , HINMEI_JP");
            sb.ApdL("     , HINMEI");
            sb.ApdL("     , HINMEI_INV");
            sb.ApdL("     , MAKER");
            sb.ApdL("     , FREE2");
            sb.ApdL("     , SUPPLIER");
            sb.ApdL("     , NOTE");
            sb.ApdL("     , CUSTOMS_STATUS");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PARTS_NAME");
            if (isForUpdate)
            {
                sb.ApdL("    WITH (ROWLOCK,UPDLOCK)");
            }
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("HINMEI_JP", cond.PartNameJa));

            // 追加条件
            if (string.IsNullOrEmpty(cond.Type))
            {
                sb.ApdL("   AND PARTS_CD IS NULL");
            }
            else
            {
                sb.ApdN("   AND PARTS_CD = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
                paramCollection.Add(iNewParam.NewDbParameter("PARTS_CD", cond.Type));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名データ取得(Excel)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">条件</param>
    /// <returns>DataSet</returns>
    /// <create>H.Tajimi 2019/07/03</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    internal DataSet GetPartsNameExcelData(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("     , HINMEI_JP");
            sb.ApdL("     , HINMEI");
            sb.ApdL("     , HINMEI_INV");
            sb.ApdL("     , MAKER");
            sb.ApdL("     , FREE2");
            sb.ApdL("     , SUPPLIER");
            sb.ApdL("     , NOTE");
            sb.ApdL("     , CUSTOMS_STATUS");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PARTS_NAME");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタの型式の有無
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <returns></returns>
    /// <create>D.Okumura 2018/11/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool HasPartsType(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PARTS_NAME");
            sb.ApdN(" WHERE PARTS_CD = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("PARTS_CD", cond.Type));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);

            return ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタの品名(JP)の有無
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <returns></returns>
    /// <create>D.Okumura 2018/11/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool HasPartsName(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       HINMEI_JP");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PARTS_NAME");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");

            // バインド変数
            paramCollection.Add(iNewParam.NewDbParameter("HINMEI_JP", cond.PartNameJa));

            // 追加条件
            if (string.IsNullOrEmpty(cond.Type))
            {
                sb.ApdL("   AND PARTS_CD IS NULL");
            }
            else
            {
                sb.ApdN("   AND PARTS_CD = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
                paramCollection.Add(iNewParam.NewDbParameter("PARTS_CD", cond.Type));

            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);

            return ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名取得(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>パーツ名マスタ</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPartsNameSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("     , HINMEI_JP");
            sb.ApdL("     , HINMEI");
            sb.ApdL("     , HINMEI_INV");
            sb.ApdL("     , MAKER");
            sb.ApdL("     , FREE2");
            sb.ApdL("     , SUPPLIER");
            sb.ApdL("     , NOTE");
            sb.ApdL("     , CUSTOMS_STATUS");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PARTS_NAME");
            sb.ApdN(" WHERE 1 = 1");

            if (!string.IsNullOrEmpty(cond.Type))
            {
                sb.ApdN("   AND PARTS_CD LIKE ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
                paramCollection.Add(iNewParam.NewDbParameter("PARTS_CD", cond.Type + "%"));
            }
            if (!string.IsNullOrEmpty(cond.PartNameJa))
            {
                sb.ApdN("   AND HINMEI_JP LIKE ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
                paramCollection.Add(iNewParam.NewDbParameter("HINMEI_JP", cond.PartNameJa + "%"));
            }
            if (!string.IsNullOrEmpty(cond.PartName))
            {
                sb.ApdN("   AND HINMEI LIKE ").ApdN(this.BindPrefix).ApdL("HINMEI");
                paramCollection.Add(iNewParam.NewDbParameter("HINMEI", cond.PartName + "%"));
            }

            sb.ApdL(" ORDER BY PARTS_CD");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_PARTS_NAME.Name);

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
    /// パーツ名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">追加データ</param>
    /// <returns>追加行数</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int InsPart(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO M_PARTS_NAME (");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("      ,HINMEI_JP");
            sb.ApdL("      ,HINMEI");
            sb.ApdL("      ,HINMEI_INV");
            sb.ApdL("      ,MAKER");
            sb.ApdL("      ,FREE2");
            sb.ApdL("      ,SUPPLIER");
            sb.ApdL("      ,NOTE");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL("      ,CUSTOMS_STATUS");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("MAKER");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("SUPPLIER");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdL(")");

            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));

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
    /// パーツ名の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>更新行数</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdParts(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE M_PARTS_NAME");
            sb.ApdN("   SET PARTS_CD = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,HINMEI_INV = ").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,MAKER = ").ApdN(this.BindPrefix).ApdL("MAKER");
            sb.ApdN("      ,FREE2 = ").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("      ,SUPPLIER = ").ApdN(this.BindPrefix).ApdL("SUPPLIER");
            sb.ApdN("      ,NOTE = ").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,CUSTOMS_STATUS = ").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP2");
            sb.ApdL("   AND (");
            sb.ApdN("     (PARTS_CD = ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdN(" AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NOT NULL)");
            sb.ApdN("     OR (PARTS_CD IS NULL AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NULL)");
            sb.ApdL("   ) ");


            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD2", string.IsNullOrEmpty(cond.Type) ? (object)DBNull.Value : (object)cond.Type));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP2", cond.PartNameJa));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI2", cond.PartName));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));

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
    /// パーツ名の更新(手配明細用T_TEHAI_MEISAI)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>更新行数</returns>
    /// <create>J.Chen 2022/12/16</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdPartsFromTehaimeisai(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE T_TEHAI_MEISAI");
            sb.ApdN("   SET ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,ZUMEN_KEISHIKI_S = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI_S");
            sb.ApdN("      ,HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,HINMEI_INV = ").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,MAKER = ").ApdN(this.BindPrefix).ApdL("MAKER");
            sb.ApdN("      ,FREE2 = ").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("      ,NOTE = ").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,CUSTOMS_STATUS = ").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP2");
            sb.ApdL("   AND (");
            sb.ApdN("     (ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdN(" AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NOT NULL)");
            sb.ApdN("     OR (ZUMEN_KEISHIKI IS NULL AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NULL)");
            sb.ApdL("   ) ");


            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD2", string.IsNullOrEmpty(cond.Type) ? (object)DBNull.Value : (object)cond.Type));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP2", cond.PartNameJa));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI2", cond.PartName));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));
                var zumenKeishiki = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
#pragma warning disable 0618
                object zumenKeishikiS = string.IsNullOrEmpty(zumenKeishiki) ? DBNull.Value : (object)ComFunc.ConvertPDMWorkNameToZumenKeishikiS(zumenKeishiki);
#pragma warning restore 0618
                paramCollection.Add(iNewParameter.NewDbParameter("ZUMEN_KEISHIKI_S", zumenKeishikiS));

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
    /// パーツ名の更新(出荷明細用T_SHUKKA_MEISAI)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>更新行数</returns>
    /// <create>J.Chen 2022/12/16</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdPartsFromShukkameisai(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdN("   SET ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,M_NO = ").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,FREE2 = ").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("      ,BIKO = ").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,CUSTOMS_STATUS = ").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP2");
            sb.ApdL("   AND (");
            sb.ApdN("     (ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdN(" AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NOT NULL)");
            sb.ApdN("     OR (ZUMEN_KEISHIKI IS NULL AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NULL)");
            sb.ApdL("   ) ");


            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD2", string.IsNullOrEmpty(cond.Type) ? (object)DBNull.Value : (object)cond.Type));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP2", cond.PartNameJa));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI2", cond.PartName));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));

                //ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

                // SQL実行
                using (DbCommand cmd = dbHelper.NewDbCommand(sb.ToString(), CommandType.Text))
                {
                    cmd.CommandTimeout = 600;
                    this.SetDbCommandParameters(cmd, paramCollection, dbHelper.BindByName);
                    ret += cmd.ExecuteNonQuery();
                }
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
    /// パーツ名の更新(実績用T_JISSEKI)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>更新行数</returns>
    /// <create>J.Chen 2022/12/16</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdPartsFromJisseki(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("UPDATE T_JISSEKI");
            sb.ApdN("   SET ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,M_NO = ").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,BIKO = ").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("      ,UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,CUSTOMS_STATUS = ").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP2");
            sb.ApdL("   AND (");
            sb.ApdN("     (ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdN(" AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NOT NULL)");
            sb.ApdN("     OR (ZUMEN_KEISHIKI IS NULL AND ").ApdN(this.BindPrefix).ApdN("PARTS_CD2").ApdL(" IS NULL)");
            sb.ApdL("   ) ");


            int ret = 0;
            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();
                var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
                paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
                paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
                paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
                paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD2", string.IsNullOrEmpty(cond.Type) ? (object)DBNull.Value : (object)cond.Type));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP2", cond.PartNameJa));
                paramCollection.Add(iNewParameter.NewDbParameter("HINMEI2", cond.PartName));
                paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));

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

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>削除行数</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelParts(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("DELETE");
            sb.ApdL("  FROM M_PARTS_NAME");
            sb.ApdN(" WHERE HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("   AND HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            if (string.IsNullOrEmpty(cond.Type))
            {
                sb.ApdL("   AND PARTS_CD IS NULL");
            }
            else
            {
                sb.ApdN("   AND PARTS_CD = ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", cond.Type));
            }

            paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", cond.PartNameJa));
            paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", cond.PartName));

            return dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パーツ名の追加

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsPartsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            //ロック、戻り値は取得しない
            this.GetPartsName(dbHelper, cond, true);

            var cond2 = new CondM01(cond.LoginInfo);
            cond2.PartNameJa = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.HINMEI_JP);
            cond2.Type = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.PARTS_CD);
            // 存在チェック
            if (!string.IsNullOrEmpty(cond2.Type) && this.HasPartsType(dbHelper, cond2))
            {
                // 既に登録されている型式です。
                errMsgID = "M0100130001";
                return false;
            }
            if (this.HasPartsName(dbHelper, cond2))
            {
                // 既に登録されている品名(和文)です。
                errMsgID = "M0100130003";
                return false;
            }

            // 登録実行
            this.InsPart(dbHelper, cond, dt);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている型式です。
                errMsgID = "M0100130001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パーツ名の更新

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdPartsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 表示データの存在チェック
            DataSet ds = this.GetPartsName(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name))
            {
                // 既に削除された型式です。
                errMsgID = "M0100130002";
                return false;
            }

            var cond2 = new CondM01(cond.LoginInfo);
            cond2.PartNameJa = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.HINMEI_JP);
            cond2.Type = ComFunc.GetFld(dt, 0, Def_M_PARTS_NAME.PARTS_CD);
            var version = ComFunc.GetFldToDateTime(ds, Def_M_PARTS_NAME.Name, 0, Def_M_PARTS_NAME.VERSION);
            // 存在チェック
            if (cond.Type != cond2.Type && !string.IsNullOrEmpty(cond2.Type) && this.HasPartsType(dbHelper, cond2))
            {
                // 既に登録されている型式です。
                errMsgID = "M0100130001";
                return false;
            }
            if (cond.PartNameJa != cond2.PartNameJa && this.HasPartsName(dbHelper, cond2))
            {
                // 既に登録されている品名(和文)です。
                errMsgID = "M0100130003";
                return false;
            }

            // バージョンチェック
            if (ComFunc.GetFldToDateTime(ds, Def_M_PARTS_NAME.Name, 0, Def_M_PARTS_NAME.VERSION) != ComFunc.GetFldToDateTime(dt, 0, Def_M_PARTS_NAME.VERSION))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 更新実行
            this.UpdParts(dbHelper, cond, dt);
            this.UpdPartsFromTehaimeisai(dbHelper, cond, dt);
            this.UpdPartsFromShukkameisai(dbHelper, cond, dt);
            this.UpdPartsFromJisseki(dbHelper, cond, dt);
            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されている型式です。
                errMsgID = "M0100130001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パーツ名の削除

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>S.Furugo 2018/10/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelPartsData(DatabaseHelper dbHelper, CondM01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 存在チェック
            DataSet ds = this.GetPartsName(dbHelper, cond, true);
            if (!ComFunc.IsExistsData(ds, Def_M_PARTS_NAME.Name))
            {
                // 既に削除された型式です。
                errMsgID = "M0100130002";
                return false;
            }

            // バージョンチェック
            if (ComFunc.GetFldToDateTime(ds, Def_M_PARTS_NAME.Name, 0, Def_M_PARTS_NAME.VERSION) != ComFunc.GetFldToDateTime(dt, 0, Def_M_PARTS_NAME.VERSION))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 削除実行
            this.DelParts(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region M0100131:パーツ名翻訳マスタ

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタの型式の有無
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool ExistsImportedPartsCd(DatabaseHelper dbHelper, CondM01 cond)
    {
        return this.HasPartsType(dbHelper, cond);
    }

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名翻訳マスタの品名(JP)の有無
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="cond">条件</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool ExistsImportedPartsName(DatabaseHelper dbHelper, CondM01 cond)
    {
        return this.HasPartsName(dbHelper, cond);
    }

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// パーツ名の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>追加行数</returns>
    /// <create>H.Tajimi 2019/07/26</create>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private int InsImportedPartsName(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            var sb = new StringBuilder();
            var paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            sb.ApdL("INSERT INTO M_PARTS_NAME (");
            sb.ApdL("       PARTS_CD");
            sb.ApdL("      ,HINMEI_JP");
            sb.ApdL("      ,HINMEI");
            sb.ApdL("      ,HINMEI_INV");
            sb.ApdL("      ,MAKER");
            sb.ApdL("      ,FREE2");
            sb.ApdL("      ,SUPPLIER");
            sb.ApdL("      ,NOTE");
            sb.ApdL("      ,CREATE_DATE");
            sb.ApdL("      ,CREATE_USER_ID");
            sb.ApdL("      ,CREATE_USER_NAME");
            sb.ApdL("      ,UPDATE_DATE");
            sb.ApdL("      ,UPDATE_USER_ID");
            sb.ApdL("      ,UPDATE_USER_NAME");
            sb.ApdL("      ,VERSION");
            sb.ApdL("      ,CUSTOMS_STATUS");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("PARTS_CD");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("HINMEI_INV");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("MAKER");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("SUPPLIER");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("      ,").ApdL(this.SysTimestamp);
            sb.ApdN("      ,").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdL(")");

            int ret = 0;
            paramCollection = new DbParamCollection();
            var partsCd = ComFunc.GetFld(dr, Def_M_PARTS_NAME.PARTS_CD);
            paramCollection.Add(iNewParameter.NewDbParameter("PARTS_CD", string.IsNullOrEmpty(partsCd) ? (object)DBNull.Value : (object)partsCd));
            paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_JP)));
            paramCollection.Add(iNewParameter.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI)));
            paramCollection.Add(iNewParameter.NewDbParameter("HINMEI_INV", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.HINMEI_INV)));
            paramCollection.Add(iNewParameter.NewDbParameter("MAKER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.MAKER)));
            paramCollection.Add(iNewParameter.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.FREE2)));
            paramCollection.Add(iNewParameter.NewDbParameter("SUPPLIER", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.SUPPLIER)));
            paramCollection.Add(iNewParameter.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.NOTE)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_M_PARTS_NAME.CUSTOMS_STATUS)));

            ret += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    
    #endregion

    #endregion

    #region M0100140:荷受保守

    #region SELECT

    #region 荷受マスタ(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷受マスタ</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public DataSet GetConsignLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MC.CONSIGN_CD");
            sb.ApdL("     , MC.NAME");
            sb.ApdL("     , MC.ADDRESS");
            sb.ApdL("     , MC.TEL1");
            sb.ApdL("     , MC.TEL2");
            sb.ApdL("     , MC.FAX");
            sb.ApdL("     , MC.CHINA_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS CHINA_FLAG_NAME");
            sb.ApdL("     , MC.USCI_CD");
            sb.ApdL("     , MC.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_CONSIGN MC");
            sb.ApdN(" INNER JOIN M_COMMON COM ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD_CHINA_FLAG");
            sb.ApdL("       AND COM.VALUE1 = MC.CHINA_FLAG");
            sb.ApdN("       AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.ConsignName))
            {
                sb.ApdN("   AND MC.NAME LIKE ").ApdN(this.BindPrefix).ApdL("CONSIGN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_NAME", cond.ConsignName + "%"));
            }
            
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MC.SORT_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD_CHINA_FLAG", CHINA_FLAG.GROUPCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_CONSIGN.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷受マスタ(完全一致・荷受CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ(完全一致・荷受CD必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷受マスタ</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public DataSet GetConsign(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MC.CONSIGN_CD");
            sb.ApdL("     , MC.NAME");
            sb.ApdL("     , MC.ADDRESS");
            sb.ApdL("     , MC.TEL1");
            sb.ApdL("     , MC.TEL2");
            sb.ApdL("     , MC.FAX");
            sb.ApdL("     , MC.CHINA_FLAG");
            sb.ApdL("     , MC.USCI_CD");
            sb.ApdL("     , MC.VERSION");
            sb.ApdL("     , MC.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_CONSIGN MC");
            sb.ApdL(" WHERE");
            sb.ApdN("       MC.CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_CONSIGN.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷受マスタ（ロック用）

    /// --------------------------------------------------
    /// <summary>
    /// 荷受マスタ（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    private DataSet GetAndLockConsign(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MC.CONSIGN_CD");
            sb.ApdL("     , MC.NAME");
            sb.ApdL("     , MC.ADDRESS");
            sb.ApdL("     , MC.TEL1");
            sb.ApdL("     , MC.TEL2");
            sb.ApdL("     , MC.FAX");
            sb.ApdL("     , MC.CHINA_FLAG");
            sb.ApdL("     , MC.USCI_CD");
            sb.ApdL("     , MC.VERSION");
            sb.ApdL("     , MC.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_CONSIGN MC");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       MC.CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_CONSIGN.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿件数を取得(完全一致・荷受CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿件数を取得(完全一致・荷受CD必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷姿件数</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetPackingCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PACKING TP");
            sb.ApdL(" WHERE");
            sb.ApdN("       TP.CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

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

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// 荷受情報の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public int InsConsign(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_CONSIGN");
            sb.ApdL("(");
            sb.ApdL("       CONSIGN_CD");
            sb.ApdL("     , NAME");
            sb.ApdL("     , ADDRESS");
            sb.ApdL("     , TEL1");
            sb.ApdL("     , TEL2");
            sb.ApdL("     , FAX");
            sb.ApdL("     , CHINA_FLAG");
            sb.ApdL("     , USCI_CD");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , SORT_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ADDRESS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEL1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEL2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FAX");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CHINA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("USCI_CD");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_M_CONSIGN.CONSIGN_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("NAME", ComFunc.GetFldObject(dr, Def_M_CONSIGN.NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("ADDRESS", ComFunc.GetFldObject(dr, Def_M_CONSIGN.ADDRESS)));
            paramCollection.Add(iNewParam.NewDbParameter("TEL1", ComFunc.GetFldObject(dr, Def_M_CONSIGN.TEL1)));
            paramCollection.Add(iNewParam.NewDbParameter("TEL2", ComFunc.GetFldObject(dr, Def_M_CONSIGN.TEL2)));
            paramCollection.Add(iNewParam.NewDbParameter("FAX", ComFunc.GetFldObject(dr, Def_M_CONSIGN.FAX)));
            paramCollection.Add(iNewParam.NewDbParameter("CHINA_FLAG", ComFunc.GetFldObject(dr, Def_M_CONSIGN.CHINA_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("USCI_CD", ComFunc.GetFldObject(dr, Def_M_CONSIGN.USCI_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_CONSIGN.SORT_NO)));

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
    /// 荷受の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public int UpdConsign(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_CONSIGN");
            sb.ApdL("SET");
            sb.ApdN("       NAME = ").ApdN(this.BindPrefix).ApdL("NAME");
            sb.ApdN("     , ADDRESS = ").ApdN(this.BindPrefix).ApdL("ADDRESS");
            sb.ApdN("     , TEL1 = ").ApdN(this.BindPrefix).ApdL("TEL1");
            sb.ApdN("     , TEL2 = ").ApdN(this.BindPrefix).ApdL("TEL2");
            sb.ApdN("     , FAX = ").ApdN(this.BindPrefix).ApdL("FAX");
            sb.ApdN("     , CHINA_FLAG = ").ApdN(this.BindPrefix).ApdL("CHINA_FLAG");
            sb.ApdN("     , USCI_CD = ").ApdN(this.BindPrefix).ApdL("USCI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
            sb.ApdN("     , SORT_NO = ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdL("WHERE");
            sb.ApdN("       CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NAME", ComFunc.GetFldObject(dr, Def_M_CONSIGN.NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("ADDRESS", ComFunc.GetFldObject(dr, Def_M_CONSIGN.ADDRESS)));
                paramCollection.Add(iNewParam.NewDbParameter("TEL1", ComFunc.GetFldObject(dr, Def_M_CONSIGN.TEL1)));
                paramCollection.Add(iNewParam.NewDbParameter("TEL2", ComFunc.GetFldObject(dr, Def_M_CONSIGN.TEL2)));
                paramCollection.Add(iNewParam.NewDbParameter("FAX", ComFunc.GetFldObject(dr, Def_M_CONSIGN.FAX)));
                paramCollection.Add(iNewParam.NewDbParameter("CHINA_FLAG", ComFunc.GetFldObject(dr, Def_M_CONSIGN.CHINA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("USCI_CD", ComFunc.GetFldObject(dr, Def_M_CONSIGN.USCI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_CONSIGN.SORT_NO)));

                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_M_CONSIGN.CONSIGN_CD)));

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
    /// 荷受の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelConsign(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_CONSIGN");
            sb.ApdL(" WHERE");
            sb.ApdN("       CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

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

    #region M0100150:配送先保守

    #region SELECT

    #region 配送先マスタ(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>配送先マスタ</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// <update>J.Jeong 2024/07/30 出荷関連項目追加</update>
    /// --------------------------------------------------
    public DataSet GetDeliverLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MD.DELIVER_CD");
            sb.ApdL("     , MD.NAME");
            sb.ApdL("     , MD.ADDRESS");
            sb.ApdL("     , MD.TEL1");
            sb.ApdL("     , MD.TEL2");
            sb.ApdL("     , MD.FAX");
            sb.ApdL("     , MD.SHIPPING_ITEM");
            sb.ApdL("     , MD.SHIPPING_TYPE");
            sb.ApdL("     , MD.SHIPPING_CONTACT");
            sb.ApdL("     , MD.UNAVAIL_NORM");
            sb.ApdL("     , MD.UNAVAIL_NY");
            sb.ApdL("     , MD.UNAVAIL_MAY");
            sb.ApdL("     , MD.UNAVAIL_AUG");
            sb.ApdL("     , MD.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_DELIVER MD");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.DeliverName))
            {
                sb.ApdN("   AND MD.NAME LIKE ").ApdN(this.BindPrefix).ApdL("DELIVER_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("DELIVER_NAME", cond.DeliverName + "%"));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       MD.SORT_NO");

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

    #region 配送先マスタ(完全一致・配送先CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ(完全一致・配送先CD必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>配送先マスタ</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// <update>J.Jeong 2024/07/30 出荷関連項目追加</update>
    /// --------------------------------------------------
    public DataSet GetDeliver(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MD.DELIVER_CD");
            sb.ApdL("     , MD.NAME");
            sb.ApdL("     , MD.ADDRESS");
            sb.ApdL("     , MD.TEL1");
            sb.ApdL("     , MD.TEL2");
            sb.ApdL("     , MD.FAX");
            sb.ApdL("     , MD.VERSION");
            sb.ApdL("     , MD.SHIPPING_ITEM");
            sb.ApdL("     , MD.SHIPPING_TYPE");
            sb.ApdL("     , MD.SHIPPING_CONTACT");
            sb.ApdL("     , MD.UNAVAIL_NORM");
            sb.ApdL("     , MD.UNAVAIL_NY");
            sb.ApdL("     , MD.UNAVAIL_MAY");
            sb.ApdL("     , MD.UNAVAIL_AUG");
            sb.ApdL("     , MD.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_DELIVER MD");
            sb.ApdL(" WHERE");
            sb.ApdN("       MD.DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", cond.DeliverCD));

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

    #region 配送先マスタ（ロック用）

    /// --------------------------------------------------
    /// <summary>
    /// 配送先マスタ（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update>J.Jeong 2024/07/30 出荷関連項目追加</update>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet GetAndLockDeliver(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MD.DELIVER_CD");
            sb.ApdL("     , MD.NAME");
            sb.ApdL("     , MD.ADDRESS");
            sb.ApdL("     , MD.TEL1");
            sb.ApdL("     , MD.TEL2");
            sb.ApdL("     , MD.FAX");
            sb.ApdL("     , MD.VERSION");
            sb.ApdL("     , MD.SHIPPING_ITEM");
            sb.ApdL("     , MD.SHIPPING_TYPE");
            sb.ApdL("     , MD.SHIPPING_CONTACT");
            sb.ApdL("     , MD.UNAVAIL_NORM");
            sb.ApdL("     , MD.UNAVAIL_NY");
            sb.ApdL("     , MD.UNAVAIL_MAY");
            sb.ApdL("     , MD.UNAVAIL_AUG");
            sb.ApdL("     , MD.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_DELIVER MD");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("       MD.DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", cond.DeliverCD));

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

    #region 荷姿件数を取得(完全一致・配送先CD必須)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿件数を取得(完全一致・配送先CD必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷姿件数</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetPackingCountDeliver(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PACKING TP");
            sb.ApdL(" WHERE");
            sb.ApdN("       TP.DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", cond.DeliverCD));

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

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// 配送先情報の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// <update>J.jeong 2024/07/29 出荷関連項目追加</update>
    /// --------------------------------------------------
    public int InsDeliver(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_DELIVER");
            sb.ApdL("(");
            sb.ApdL("       DELIVER_CD");
            sb.ApdL("     , NAME");
            sb.ApdL("     , ADDRESS");
            sb.ApdL("     , TEL1");
            sb.ApdL("     , TEL2");
            sb.ApdL("     , FAX");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , SORT_NO");
            sb.ApdL("     , SHIPPING_ITEM");
            sb.ApdL("     , SHIPPING_TYPE");
            sb.ApdL("     , SHIPPING_CONTACT");
            sb.ApdL("     , UNAVAIL_NORM");
            sb.ApdL("     , UNAVAIL_NY");
            sb.ApdL("     , UNAVAIL_MAY");
            sb.ApdL("     , UNAVAIL_AUG");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ADDRESS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEL1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEL2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FAX");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIPPING_ITEM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIPPING_TYPE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIPPING_CONTACT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNAVAIL_NORM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNAVAIL_NY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNAVAIL_MAY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNAVAIL_AUG");

            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", ComFunc.GetFldObject(dr, Def_M_DELIVER.DELIVER_CD)));
            paramCollection.Add(iNewParam.NewDbParameter("NAME", ComFunc.GetFldObject(dr, Def_M_DELIVER.NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("ADDRESS", ComFunc.GetFldObject(dr, Def_M_DELIVER.ADDRESS)));
            paramCollection.Add(iNewParam.NewDbParameter("TEL1", ComFunc.GetFldObject(dr, Def_M_DELIVER.TEL1)));
            paramCollection.Add(iNewParam.NewDbParameter("TEL2", ComFunc.GetFldObject(dr, Def_M_DELIVER.TEL2)));
            paramCollection.Add(iNewParam.NewDbParameter("FAX", ComFunc.GetFldObject(dr, Def_M_DELIVER.FAX)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_DELIVER.SORT_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_ITEM", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_ITEM)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_TYPE", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_TYPE)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_CONTACT", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_CONTACT)));
            paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_NORM", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_NORM)));
            paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_NY", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_NY)));
            paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_MAY", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_MAY)));
            paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_AUG", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_AUG)));

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
    /// 配送先の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// <update>J.Jeong 2024/07/30 出荷関連項目追加</update>
    /// --------------------------------------------------
    public int UpdDeliver(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_DELIVER");
            sb.ApdL("SET");
            sb.ApdN("       NAME = ").ApdN(this.BindPrefix).ApdL("NAME");
            sb.ApdN("     , ADDRESS = ").ApdN(this.BindPrefix).ApdL("ADDRESS");
            sb.ApdN("     , TEL1 = ").ApdN(this.BindPrefix).ApdL("TEL1");
            sb.ApdN("     , TEL2 = ").ApdN(this.BindPrefix).ApdL("TEL2");
            sb.ApdN("     , FAX = ").ApdN(this.BindPrefix).ApdL("FAX");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
            sb.ApdN("     , SORT_NO = ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdN("     , SHIPPING_ITEM = ").ApdN(this.BindPrefix).ApdL("SHIPPING_ITEM");
            sb.ApdN("     , SHIPPING_TYPE = ").ApdN(this.BindPrefix).ApdL("SHIPPING_TYPE");
            sb.ApdN("     , SHIPPING_CONTACT = ").ApdN(this.BindPrefix).ApdL("SHIPPING_CONTACT");
            sb.ApdN("     , UNAVAIL_NORM = ").ApdN(this.BindPrefix).ApdL("UNAVAIL_NORM");
            sb.ApdN("     , UNAVAIL_NY = ").ApdN(this.BindPrefix).ApdL("UNAVAIL_NY");
            sb.ApdN("     , UNAVAIL_MAY = ").ApdN(this.BindPrefix).ApdL("UNAVAIL_MAY");
            sb.ApdN("     , UNAVAIL_AUG = ").ApdN(this.BindPrefix).ApdL("UNAVAIL_AUG");
            sb.ApdL("WHERE");
            sb.ApdN("       DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NAME", ComFunc.GetFldObject(dr, Def_M_DELIVER.NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("ADDRESS", ComFunc.GetFldObject(dr, Def_M_DELIVER.ADDRESS)));
                paramCollection.Add(iNewParam.NewDbParameter("TEL1", ComFunc.GetFldObject(dr, Def_M_DELIVER.TEL1)));
                paramCollection.Add(iNewParam.NewDbParameter("TEL2", ComFunc.GetFldObject(dr, Def_M_DELIVER.TEL2)));
                paramCollection.Add(iNewParam.NewDbParameter("FAX", ComFunc.GetFldObject(dr, Def_M_DELIVER.FAX)));
                paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_DELIVER.SORT_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_ITEM", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_ITEM)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_TYPE", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_TYPE)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIPPING_CONTACT", ComFunc.GetFldObject(dr, Def_M_DELIVER.SHIPPING_CONTACT)));
                paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_NORM", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_NORM)));
                paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_NY", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_NY)));
                paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_MAY", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_MAY)));
                paramCollection.Add(iNewParam.NewDbParameter("UNAVAIL_AUG", ComFunc.GetFldObject(dr, Def_M_DELIVER.UNAVAIL_AUG)));

                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", ComFunc.GetFldObject(dr, Def_M_DELIVER.DELIVER_CD)));

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
    /// 配送先の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/12/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelDeliver(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_DELIVER");
            sb.ApdL(" WHERE");
            sb.ApdN("       DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", cond.DeliverCD));

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

    #region M0100160:運送会社保守

    #region SELECT

    #region 運送会社取得(運送会社CD/国内外フラグ/運送会社名)

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社取得(運送会社CD/国内外フラグ/運送会社名)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>運送会社マスタ</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public DataSet GetUnsokaisya(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("       MU.UNSOKAISHA_NO");
            sb.ApdL("     , MU.KOKUNAI_GAI_FLAG");
            sb.ApdL("     , MU.UNSOKAISHA_NAME");
            sb.ApdL("     , MU.INVOICE_FLAG");
            sb.ApdL("     , MU.PACKINGLIST_FLAG");
            sb.ApdL("     , MU.EXPORTCONFIRM_FLAG");
            sb.ApdL("     , MU.EXPORTCONFIRM_ATTN");
            sb.ApdL("     , MU.VERSION");
            sb.ApdL("     , MU.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_UNSOKAISHA MU");
            sb.ApdN(" WHERE 1 = 1");

            if (!string.IsNullOrEmpty(cond.UnsoKaishaNo))
            {
                sb.ApdN("   AND MU.UNSOKAISHA_NO = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", cond.UnsoKaishaNo));
            }
            if (!string.IsNullOrEmpty(cond.KokunaigaiFlag))
            {
                sb.ApdN("   AND MU.KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", cond.KokunaigaiFlag));
            }
            if (!string.IsNullOrEmpty(cond.UnsokaishaName))
            {
                sb.ApdN("   AND MU.UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", cond.UnsokaishaName));
            }

            sb.ApdL(" ORDER BY MU.SORT_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_UNSOKAISHA.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 運送会社取得(あいまい検索:国内外フラグ/運送会社名)

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社取得(あいまい検索:国内外フラグ/運送会社名)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>運送会社マスタ</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public DataSet GetUnsokaisyaLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT");
            sb.ApdL("       MU.UNSOKAISHA_NO");
            sb.ApdL("     , COM1.ITEM_NAME AS KOKUNAI_GAI_FLAG");
            sb.ApdL("     , MU.UNSOKAISHA_NAME");
            sb.ApdL("     , COM2.ITEM_NAME AS INVOICE_FLAG");
            sb.ApdL("     , COM3.ITEM_NAME AS PACKINGLIST_FLAG");
            sb.ApdL("     , COM4.ITEM_NAME AS EXPORTCONFIRM_FLAG");
            sb.ApdL("     , MU.EXPORTCONFIRM_ATTN");
            sb.ApdL("     , MU.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_UNSOKAISHA MU");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'KOKUNAI_GAI_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = MU.KOKUNAI_GAI_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'UNSO_INVOICE_FLAG'");
            sb.ApdL("                         AND COM2.VALUE1 = MU.INVOICE_FLAG");
            sb.ApdN("                         AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM3 ON COM3.GROUP_CD = 'UNSO_PACKINGLIST_FLAG'");
            sb.ApdL("                         AND COM3.VALUE1 = MU.PACKINGLIST_FLAG");
            sb.ApdN("                         AND COM3.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM4 ON COM4.GROUP_CD = 'UNSO_EXPORTCONFIRM_FLAG'");
            sb.ApdL("                         AND COM4.VALUE1 = MU.EXPORTCONFIRM_FLAG");
            sb.ApdN("                         AND COM4.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdN(" WHERE 1 = 1");

            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            if (!string.IsNullOrEmpty(cond.KokunaigaiFlag))
            {
                sb.ApdN("   AND MU.KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", cond.KokunaigaiFlag));
            }
            if (!string.IsNullOrEmpty(cond.UnsokaishaName))
            {
                sb.ApdN("   AND MU.UNSOKAISHA_NAME LIKE ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", cond.UnsokaishaName + "%"));
            }

            sb.ApdL(" ORDER BY MU.SORT_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_UNSOKAISHA.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 運送会社マスタ（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// 運送会社マスタ（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetAndLockUnsokaisya(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_UNSOKAISHA.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MU.UNSOKAISHA_NO");
            sb.ApdL("     , MU.KOKUNAI_GAI_FLAG");
            sb.ApdL("     , MU.UNSOKAISHA_NAME");
            sb.ApdL("     , MU.INVOICE_FLAG");
            sb.ApdL("     , MU.PACKINGLIST_FLAG");
            sb.ApdL("     , MU.EXPORTCONFIRM_FLAG");
            sb.ApdL("     , MU.EXPORTCONFIRM_ATTN");
            sb.ApdL("	  , MU.CREATE_DATE");
            sb.ApdL("	  , MU.CREATE_USER_ID");
            sb.ApdL("	  , MU.CREATE_USER_NAME");
            sb.ApdL("	  , MU.UPDATE_DATE");
            sb.ApdL("	  , MU.UPDATE_USER_ID");
            sb.ApdL("	  , MU.UPDATE_USER_NAME");
            sb.ApdL("	  , MU.MAINTE_DATE");
            sb.ApdL("	  , MU.MAINTE_USER_ID");
            sb.ApdL("	  , MU.MAINTE_USER_NAME");
            sb.ApdL("     , MU.VERSION");
            sb.ApdL("     , MU.SORT_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_UNSOKAISHA MU");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE");
            sb.ApdN("   MU.UNSOKAISHA_NO = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", cond.UnsoKaishaNo));

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

    #region 指定運送会社名の運送会社情報数を取得(完全一致・国内外フラグ/運送会社名必須)

    /// --------------------------------------------------
    /// <summary>
    /// 指定運送会社名の運送会社情報数を取得(完全一致・国内外フラグ/運送会社名必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>運送会社マスタ件数</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetUnsoKaisyaNameCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       M_UNSOKAISHA MU");
            sb.ApdL(" WHERE");
            sb.ApdN("       MU.KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
            sb.ApdN("   AND MU.UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            if (!string.IsNullOrEmpty(cond.UnsoKaishaNo))
            {
                sb.ApdN("   AND MU.UNSOKAISHA_NO <> ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", cond.UnsoKaishaNo));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", cond.KokunaigaiFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", cond.UnsokaishaName));

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

    #region 指定運送会社CDの荷姿情報数を取得(完全一致・運送会社CD必須/ロック用)

    /// --------------------------------------------------
    /// <summary>
    /// 指定運送会社CDの荷姿情報数を取得(完全一致・運送会社CD必須/ロック用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>荷姿情報件数</returns>
    /// <create>T.Nakata 2018/11/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetUnsokaisyaNisugataCount(DatabaseHelper dbHelper, CondM01 cond, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PACKING TP");
            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }
            sb.ApdL(" WHERE");
            sb.ApdN("       TP.UNSOKAISHA_CD = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", cond.UnsoKaishaNo));

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

    #endregion

    #region INSERT

    /// --------------------------------------------------
    /// <summary>
    /// 運送会社情報の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dr">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public int InsUnsoKaisya(DatabaseHelper dbHelper, CondM01 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_UNSOKAISHA");
            sb.ApdL("(");
            sb.ApdL("       UNSOKAISHA_NO");
            sb.ApdL("     , KOKUNAI_GAI_FLAG");
            sb.ApdL("     , UNSOKAISHA_NAME");
            sb.ApdL("     , INVOICE_FLAG");
            sb.ApdL("     , PACKINGLIST_FLAG");
            sb.ApdL("     , EXPORTCONFIRM_FLAG");
            sb.ApdL("     , EXPORTCONFIRM_ATTN");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , MAINTE_DATE");
            sb.ApdL("     , MAINTE_USER_ID");
            sb.ApdL("     , MAINTE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , SORT_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("INVOICE_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PACKINGLIST_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("EXPORTCONFIRM_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("EXPORTCONFIRM_ATTN");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG)));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("INVOICE_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.INVOICE_FLAG, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("PACKINGLIST_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.PACKINGLIST_FLAG, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("EXPORTCONFIRM_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("EXPORTCONFIRM_ATTN", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.EXPORTCONFIRM_ATTN, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.SORT_NO, 0)));

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
    /// 運送会社の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/11/05</create>
    /// <update>K.Tsutsumi 2018/01/22 並び順対応</update>
    /// --------------------------------------------------
    public int UpdUnsoKaisya(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_UNSOKAISHA");
            sb.ApdL("SET");
            sb.ApdN("       KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
            sb.ApdN("     , UNSOKAISHA_NAME = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            sb.ApdN("     , INVOICE_FLAG = ").ApdN(this.BindPrefix).ApdL("INVOICE_FLAG");
            sb.ApdN("     , PACKINGLIST_FLAG = ").ApdN(this.BindPrefix).ApdL("PACKINGLIST_FLAG");
            sb.ApdN("     , EXPORTCONFIRM_FLAG = ").ApdN(this.BindPrefix).ApdL("EXPORTCONFIRM_FLAG");
            sb.ApdN("     , EXPORTCONFIRM_ATTN = ").ApdN(this.BindPrefix).ApdL("EXPORTCONFIRM_ATTN");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
            sb.ApdN("     , SORT_NO = ").ApdN(this.BindPrefix).ApdL("SORT_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       UNSOKAISHA_NO = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("KOKUNAI_GAI_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.KOKUNAI_GAI_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("INVOICE_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.INVOICE_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("PACKINGLIST_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.PACKINGLIST_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("EXPORTCONFIRM_FLAG", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.EXPORTCONFIRM_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("EXPORTCONFIRM_ATTN", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.EXPORTCONFIRM_ATTN)));
                paramCollection.Add(iNewParam.NewDbParameter("SORT_NO", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.SORT_NO)));

                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NO)));

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
    /// 運送会社の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelUnsoKaisya(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_UNSOKAISHA");
            sb.ApdL(" WHERE");
            sb.ApdN("       UNSOKAISHA_NO = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NO", ComFunc.GetFldObject(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NO)));

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

    #region M0100170:技連保守

    #region SELECT

    #region プロジェクトマスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// プロジェクトマスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>プロジェクトマスタ</returns>
    /// <create>H.Tsuji 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetProject(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MP.PROJECT_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_PROJECT MP");
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdL(" WHERE MP.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MP.PROJECT_NO");

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

    #region 名称マスタ取得(機種一覧用)

    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得(機種一覧用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>名称マスタ</returns>
    /// <create>H.Tsuji 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSelectItemForKishu(DatabaseHelper dbHelper, CondM01 cond)
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

            paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCD));

            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 技連マスタ(完全一致・期、ECSNo.必須)
    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ(完全一致・期、ECSNo.必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>技連マスタ</returns>
    /// <create>H.Tsuji 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetEcs(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ME.ECS_QUOTA");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , ME.PROJECT_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , ME.AR_NO");
            sb.ApdL("     , ME.SEIBAN");
            sb.ApdL("     , ME.CODE");
            sb.ApdL("     , ME.KISHU");
            sb.ApdL("     , ME.SEIBAN_CODE");
            sb.ApdL("     , ME.KANRI_FLAG");
            sb.ApdL("     , ME.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_ECS ME");
            sb.ApdL("  LEFT JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       ME.ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("   AND ME.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       ME.ECS_QUOTA");
            sb.ApdL("     , ME.ECS_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_ECS.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 技連マスタ(あいまい検索)

    /// --------------------------------------------------
    /// <summary>
    /// 技連マスタ(あいまい検索)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>技連マスタ</returns>
    /// <create>H.Tsuji 2018/11/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetEcsLikeSearch(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ME.ECS_QUOTA");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , ME.PROJECT_NO");
            sb.ApdL("     , ME.SEIBAN");
            sb.ApdL("     , ME.CODE");
            sb.ApdL("     , ME.KISHU");
            sb.ApdL("     , ME.SEIBAN_CODE");
            sb.ApdL("     , ME.AR_NO");
            sb.ApdL("     , ME.KANRI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS KANRI_FLAG_NAME");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_ECS ME");
            sb.ApdL(" INNER JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdN(" INNER JOIN M_COMMON COM ON COM.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GROUP_CD_KANRI_FLAG");
            sb.ApdL("       AND COM.VALUE1 = ME.KANRI_FLAG");
            sb.ApdN("       AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.EcsQuota))
            {
                sb.ApdN("   AND ME.ECS_QUOTA LIKE ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota + "%"));
            }
            if (!string.IsNullOrEmpty(cond.EcsNo))
            {
                sb.ApdN("   AND ME.ECS_NO LIKE ").ApdN(this.BindPrefix).ApdL("ECS_NO");
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo + "%"));
            }
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND MP.BUKKEN_NAME LIKE ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdN("   AND ME.SEIBAN LIKE ").ApdN(this.BindPrefix).ApdL("SEIBAN");
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", cond.Seiban + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Code))
            {
                sb.ApdN("   AND ME.CODE LIKE ").ApdN(this.BindPrefix).ApdL("CODE");
                paramCollection.Add(iNewParam.NewDbParameter("CODE", cond.Code + "%"));
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND ME.AR_NO LIKE ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo + "%"));
            }
            if (!string.IsNullOrEmpty(cond.Kishu))
            {
                sb.ApdN("   AND ME.KISHU LIKE ").ApdN(this.BindPrefix).ApdL("KISHU");
                paramCollection.Add(iNewParam.NewDbParameter("KISHU", cond.Kishu + "%"));
            }
            if (!string.Equals(cond.KanriFlag, DISP_KANRI_FLAG.DEFAULT_VALUE1))
            {
                sb.ApdN("   AND ME.KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", cond.KanriFlag));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       ME.ECS_QUOTA");
            sb.ApdL("     , ME.ECS_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("GROUP_CD_KANRI_FLAG", KANRI_FLAG.GROUPCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_ECS.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 指定技連の手配明細情報数を取得(完全一致、期・ECSNo)
    /// --------------------------------------------------
    /// <summary>
    /// 指定技連の手配明細情報数を取得(完全一致、期・ECSNo)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>手配明細件数</returns>
    /// <create>H.Tsuji 2018/11/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public int GetGirenTehaiMeisaiCount(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEHAI_MEISAI TM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TM.ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("   AND TM.ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

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

    #endregion

    #region INSERT
    /// --------------------------------------------------
    /// <summary>
    /// 技連の追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">追加データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/11/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsEcs(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO M_ECS");
            sb.ApdL("(");
            sb.ApdL("       ECS_QUOTA");
            sb.ApdL("     , ECS_NO");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SEIBAN");
            sb.ApdL("     , CODE");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , SEIBAN_CODE");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , KANRI_FLAG");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", ComFunc.GetFldObject(dr, Def_M_ECS.ECS_QUOTA, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", ComFunc.GetFldObject(dr, Def_M_ECS.ECS_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", ComFunc.GetFldObject(dr, Def_M_ECS.PROJECT_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("CODE", ComFunc.GetFldObject(dr, Def_M_ECS.CODE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_M_ECS.KISHU, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN_CODE", ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN_CODE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_M_ECS.AR_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", ComFunc.GetFldObject(dr, Def_M_ECS.KANRI_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
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

    #region UPDATE
    /// --------------------------------------------------
    /// <summary>
    /// 技連の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdEcs(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_ECS");
            sb.ApdL("SET");
            sb.ApdN("       PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("PROJECT_NO");
            sb.ApdN("     , SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            sb.ApdN("     , CODE = ").ApdN(this.BindPrefix).ApdL("CODE");
            sb.ApdN("     , KISHU = ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , SEIBAN_CODE = ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE");
            sb.ApdN("     , AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , MAINTE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , MAINTE_USER_ID = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_ID");
            sb.ApdN("     , MAINTE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("MAINTE_USER_NAME");
            sb.ApdL("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("   AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PROJECT_NO", ComFunc.GetFldObject(dr, Def_M_ECS.PROJECT_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("CODE", ComFunc.GetFldObject(dr, Def_M_ECS.CODE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_M_ECS.KISHU, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN_CODE", ComFunc.GetFldObject(dr, Def_M_ECS.SEIBAN_CODE, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_M_ECS.AR_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", ComFunc.GetFldObject(dr, Def_M_ECS.KANRI_FLAG, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetMainteUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetMainteUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
                paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

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
    /// 技連の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2018/11/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelEcs(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_ECS");
            sb.ApdL(" WHERE");
            sb.ApdN("       ECS_QUOTA = ").ApdN(this.BindPrefix).ApdL("ECS_QUOTA");
            sb.ApdN("   AND ECS_NO = ").ApdN(this.BindPrefix).ApdL("ECS_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ECS_QUOTA", cond.EcsQuota));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", cond.EcsNo));

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

    #region M0100180:進捗管理通知設定

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>個別が登録されていれば個別を返し、個別が未登録ならDefaultを返す</returns>
    /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShinchokuKanriNotify(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            var dt = new DataTable(Def_M_BUKKEN_MAIL.Name);
            var sb = new StringBuilder();
            var pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 個別登録があればそれを採用
            dt = this.GetShinchokuKanriNotifyKobetsu(dbHelper, cond);
            
            // 個別登録がなければ進捗管理通知設定(Default)を採用
            if (dt.Rows.Count == 0)
            {
                dt = this.GetBasicNotifySelect(dbHelper, cond, MAIL_KBN.ARSHINCHOKU_VALUE1);
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
    /// 進捗管理通知設定取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShinchokuKanriNotifyKobetsu(DatabaseHelper dbHelper, CondM01 cond)
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
            sb.ApdL(" WHERE M_BUKKEN_MAIL.SHUKKA_FLAG = @SHUKKA_FLAG");
            sb.ApdL("   AND M_BUKKEN_MAIL.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND M_BUKKEN_MAIL.MAIL_KBN = @MAIL_KBN");
            sb.ApdL(" ORDER BY M_BUKKEN_MAIL_MEISAI.MAIL_ADDRESS_FLAG");
            sb.ApdL("        , M_BUKKEN_MAIL_MEISAI.ORDER_NO");

            pc.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            pc.Add(iNewParam.NewDbParameter("MAIL_KBN", MAIL_KBN.ARSHINCHOKU_KOBETU_VALUE1));

            dbHelper.Fill(sb.ToString(), pc, dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 進捗管理通知設定の保存
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="ds">保存データ</param>
    /// <param name="errMsgId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>Y.Nakasato 2019/07/08 AR進捗通知対応</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SaveShinchokuKanriNotify(DatabaseHelper dbHelper, CondM01 cond, DataSet ds, ref string errMsgId, ref string[] args)
    {
        try
        {
            // メールヘッダID採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.MAIL_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            string mailHeaderId;

            using (var impl = new WsSmsImpl())
            {
                if (!impl.GetSaiban(dbHelper, condSms, out mailHeaderId, out errMsgId))
                {
                    return false;
                }
            }

            // 旧メールヘッダIDの取得と、新メールヘッダIDの設定
            string oldMailHeaderId = ComFunc.GetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID);
            UtilData.SetFld(ds, Def_M_BUKKEN_MAIL.Name, 0, Def_M_BUKKEN_MAIL.MAIL_HEADER_ID, mailHeaderId);
            for (int i = 0; i < ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name].Rows.Count; i++)
            {
                UtilData.SetFld(ds, Def_M_BUKKEN_MAIL_MEISAI.Name, i, Def_M_BUKKEN_MAIL_MEISAI.MAIL_HEADER_ID, mailHeaderId);
            }

            // 物件メールマスタ更新
            this.MergeBukkenMail(dbHelper, cond, ds.Tables[Def_M_BUKKEN_MAIL.Name].Rows[0], oldMailHeaderId);

            // 物件メール明細マスタ削除
            if (!string.IsNullOrEmpty(oldMailHeaderId))
            {
                this.DelBukkenMailMeisai(dbHelper, oldMailHeaderId);
            }

            // 物件メール明細マスタ登録
            this.InsBukkenMailMeisai(dbHelper, cond, ds.Tables[Def_M_BUKKEN_MAIL_MEISAI.Name]);

            return true;
        }
        catch (Exception ex)
        {
            // 一意制約違反チェック
            if (this.IsDbDuplicationError(ex))
            {
                // 既に登録されているメールヘッダIDです。
                errMsgId = "M0100080001";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region M0100190:出荷元保守

    #region SELECT

    #region 出荷元マスタ (M_SHIP_FROM)の取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetShipFrom(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_SHIP_FROM.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_SHIP_FROM.SHIP_FROM_NO");
            sb.ApdL("     , M_SHIP_FROM.SHANAIGAI_FLAG");
            sb.ApdL("     , COM1.ITEM_NAME AS SHANAIGAI_FLAG_NAME");
            sb.ApdL("     , M_SHIP_FROM.SHIP_FROM_NAME");
            sb.ApdL("     , M_SHIP_FROM.UNUSED_FLAG");
            sb.ApdL("     , COM2.ITEM_NAME AS UNUSED_FLAG_NAME");
            sb.ApdL("     , M_SHIP_FROM.DISP_NO");
            sb.ApdL("     , M_SHIP_FROM.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_SHIP_FROM");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("    ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_SHANAIGAI_FLAG");
            sb.ApdL("   AND COM1.VALUE1 = M_SHIP_FROM.SHANAIGAI_FLAG");
            sb.ApdN("   AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("    ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_UNUSED_FLAG");
            sb.ApdL("   AND COM2.VALUE1 = M_SHIP_FROM.UNUSED_FLAG");
            sb.ApdN("   AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.ShipFromCd))
            {
                sb.ApdN("   AND M_SHIP_FROM.SHIP_FROM_NO = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", cond.ShipFromCd));
            }
            if (!string.IsNullOrEmpty(cond.ShanaigaiFlag))
            {
                sb.ApdN("   AND M_SHIP_FROM.SHANAIGAI_FLAG = ").ApdN(this.BindPrefix).ApdL("SHANAIGAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHANAIGAI_FLAG", cond.ShanaigaiFlag));
            }
            if (!string.IsNullOrEmpty(cond.ShipFromName))
            {
                sb.ApdN("   AND M_SHIP_FROM.SHIP_FROM_NAME LIKE ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NAME", cond.ShipFromName + "%"));
            }
            if (!string.IsNullOrEmpty(cond.UnusedFlag))
            {
                sb.ApdN("   AND M_SHIP_FROM.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", cond.UnusedFlag));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       M_SHIP_FROM.DISP_NO");
            sb.ApdL("     , M_SHIP_FROM.SHIP_FROM_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GC_SHANAIGAI_FLAG", SHANAIGAI_FLAG.GROUPCD));
            paramCollection.Add(iNewParam.NewDbParameter("GC_UNUSED_FLAG", UNUSED_FLAG.GROUPCD));
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

    #region 出荷元マスタ (M_SHIP_FROM)の存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal bool Sql_ExistsShipFrom(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_SHIP_FROM.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT 1");
            sb.ApdL("  FROM");
            sb.ApdL("       M_SHIP_FROM");
            sb.ApdL(" WHERE");
            sb.ApdN("       M_SHIP_FROM.SHIP_FROM_NAME = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NAME");
            if (!string.IsNullOrEmpty(cond.ShanaigaiFlag))
            {
                sb.ApdL("   AND M_SHIP_FROM.SHANAIGAI_FLAG = ").ApdN(this.BindPrefix).ApdL("SHANAIGAI_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHANAIGAI_FLAG", cond.ShanaigaiFlag));
            }
            if (!string.IsNullOrEmpty(cond.ShipFromCd))
            {
                sb.ApdL("   AND M_SHIP_FROM.SHIP_FROM_NO <> ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", cond.ShipFromCd));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NAME", cond.ShipFromName));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            return UtilData.ExistsData(dt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷元マスタ (M_SHIP_FROM)の行ロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_LockShipFrom(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_SHIP_FROM.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_SHIP_FROM.SHIP_FROM_NO");
            sb.ApdL("     , M_SHIP_FROM.SHANAIGAI_FLAG");
            sb.ApdL("     , M_SHIP_FROM.SHIP_FROM_NAME");
            sb.ApdL("     , M_SHIP_FROM.UNUSED_FLAG");
            sb.ApdL("     , M_SHIP_FROM.DISP_NO");
            sb.ApdL("     , M_SHIP_FROM.VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_SHIP_FROM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdL("       M_SHIP_FROM.SHIP_FROM_NO = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", cond.ShipFromCd));

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

    #region 荷姿 (T_PACKING)の存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿 (T_PACKING)の存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <returns>出荷元マスタ</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal bool Sql_ExistsPackingForShipFrom(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_SHIP_FROM.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT 1");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PACKING");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_PACKING.SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_CD", cond.ShipFromCd));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            return UtilData.ExistsData(dt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region INSERT

    #region 出荷元マスタ (M_SHIP_FROM)の登録

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">登録データ</param>
    /// <returns>True:登録成功/False:登録失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal bool Sql_InsShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            var recCnt = 0;

            sb.ApdL("INSERT INTO M_SHIP_FROM");
            sb.ApdL("(");
            sb.ApdL("       SHIP_FROM_NO");
            sb.ApdL("     , SHANAIGAI_FLAG");
            sb.ApdL("     , SHIP_FROM_NAME");
            sb.ApdL("     , UNUSED_FLAG");
            sb.ApdL("     , DISP_NO");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHANAIGAI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DISP_NO");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                // バインド変数設定
                var paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHIP_FROM_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("SHANAIGAI_FLAG", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHANAIGAI_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NAME", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHIP_FROM_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.UNUSED_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("DISP_NO", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.DISP_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", cond.LoginInfo.UserID));
                paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", cond.LoginInfo.UserName));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", cond.LoginInfo.UserID));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", cond.LoginInfo.UserName));

                // SQL実行
                recCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return dt.Rows.Count == recCnt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region UPDATE

    #region 出荷元マスタ (M_SHIP_FROM)の更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>True:更新成功/False:更新失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal bool Sql_UpdShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        try
        {
            var sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;
            var recCnt = 0;

            sb.ApdL("UPDATE M_SHIP_FROM");
            sb.ApdL("   SET");
            sb.ApdL("       SHANAIGAI_FLAG = ").ApdN(this.BindPrefix).ApdL("SHANAIGAI_FLAG");
            sb.ApdL("     , SHIP_FROM_NAME = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NAME");
            sb.ApdN("     , UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");
            sb.ApdN("     , DISP_NO = ").ApdN(this.BindPrefix).ApdL("DISP_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       M_SHIP_FROM.SHIP_FROM_NO = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");

            foreach (DataRow dr in dt.Rows)
            {
                // バインド変数設定
                var paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("SHANAIGAI_FLAG", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHANAIGAI_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NAME", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHIP_FROM_NAME)));
                paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.UNUSED_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("DISP_NO", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.DISP_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", cond.LoginInfo.UserID));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", cond.LoginInfo.UserName));

                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHIP_FROM_NO)));

                // SQL実行
                recCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            }
            return dt.Rows.Count == recCnt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region DELETE

    #region 出荷元マスタ (M_SHIP_FROM)の削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷元マスタ (M_SHIP_FROM)の削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">M01用コンディション</param>
    /// <param name="dt">削除データ</param>
    /// <returns>True:削除成功/False:削除失敗</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    internal bool Sql_DelShipFrom(DatabaseHelper dbHelper, CondM01 cond, DataTable dt)
    {
        var sb = new StringBuilder();
        INewDbParameterBasic iNewParam = dbHelper;
        var recCnt = 0;

        sb.ApdL("DELETE");
        sb.ApdL("  FROM M_SHIP_FROM");
        sb.ApdL(" WHERE");
        sb.ApdL("       M_SHIP_FROM.SHIP_FROM_NO = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_NO");

        foreach (DataRow dr in dt.Rows)
        {
            // バインド変数設定
            var paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_NO", ComFunc.GetFldObject(dr, Def_M_SHIP_FROM.SHIP_FROM_NO)));

            recCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
        }
        return dt.Rows.Count == recCnt;
    }

    #endregion

    #endregion

    #endregion

    #region　権限付与の確認（権限マスタ、権限マップマスタ）2022/10/14
    /// --------------------------------------------------
    /// <summary>
    /// 権限マスタの個別検索
    /// </summary>
    /// <create>TW-Tsuji 2012/10/14</create>
    /// <update></update>
    /// <notes>
    /// 特定の機能について利用権限が付与されているかを
    /// 確認する.
    /// </notes>
    /// --------------------------------------------------
    internal bool ExistsRoleAndRolemap(DatabaseHelper dbHelper, CondM01 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_ROLE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT 1");
            sb.ApdL("  FROM");
            sb.ApdL("       M_ROLE");
            sb.ApdL("  INNER JOIN M_ROLE_MAP COM1");
            sb.ApdL("     ON COM1.ROLE_ID = M_ROLE.ROLE_ID");
            sb.ApdL("  INNER JOIN M_USER COM2");
            sb.ApdL("     ON COM2.ROLE_ID = M_ROLE.ROLE_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       COM2.USER_ID = ").ApdN(this.BindPrefix).ApdL("USER_ID");                    //ユーザID
            sb.ApdL("   AND");
            sb.ApdN("       M_ROLE.ROLE_FLAG = ").ApdN(this.BindPrefix).ApdL("ROLE_FLAG");              //ロールフラグ
            sb.ApdL("   AND");
            sb.ApdN("       COM1.MENU_CATEGORY_ID = ").ApdN(this.BindPrefix).ApdL("MENU_CATEGORY_ID");  //カテゴリID
            sb.ApdL("   AND");
            sb.ApdN("       COM1.MENU_ITEM_ID = ").ApdN(this.BindPrefix).ApdL("MENU_ITEM_ID");          //アイテムID

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("USER_ID", cond.UserID));              //ログインしているユーザID
            paramCollection.Add(iNewParam.NewDbParameter("ROLE_FLAG", ROLE_FLAG.USER_VALUE1));  //権限種別（使用している）
            paramCollection.Add(iNewParam.NewDbParameter("MENU_CATEGORY_ID", cond.JyotaiFlag)); //状態フラグを流用して、カテゴリIDを渡す
            paramCollection.Add(iNewParam.NewDbParameter("MENU_ITEM_ID", cond.KanriFlag));      //管理フラグを流用して、アイテムIDを渡す

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            return UtilData.ExistsData(dt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region SetBindByName(DbCommand cmd, bool value)

    /// --------------------------------------------------
    /// <summary>
    /// BindByNameプロパティがある場合プロパティの設定を行う。
    /// </summary>
    /// <param name="cmd">DbCommand</param>
    /// <param name="value">プロパティの設定値</param>
    /// <create>2013/12/06 K.Tsutsumi</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetBindByName(DbCommand cmd, bool value)
    {
        Type cmdType = cmd.GetType();
        PropertyInfo propInfo = cmdType.GetProperty("BindByName");
        if (propInfo != null)
        {
            //プロパティがある場合
            propInfo.SetValue(cmd, value, null);
        }
    }

    #endregion

    #region SetDbCommandParameters(DbCommand cmd, DbParamCollection parameterCollection)

    /// --------------------------------------------------
    /// <summary>
    /// DbCommandにパラメータを設定する時の共通設定
    /// </summary>
    /// <param name="cmd">DbCommand</param>
    /// <param name="parameterCollection">Parameterのコレクション</param>
    /// <create>2013/12/06 K.Tsutsumi</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetDbCommandParameters(DbCommand cmd, DbParamCollection parameterCollection, bool bindByName)
    {
        //Parameterコレクションがnullであれば処理を抜ける
        if (parameterCollection == null)
        {
            return;
        }

        this.SetBindByName(cmd, bindByName);
        for (int i = 0; i < parameterCollection.Count; i++)
        {
            cmd.Parameters.Add(parameterCollection[i]);
        }
    }

    #endregion
}
