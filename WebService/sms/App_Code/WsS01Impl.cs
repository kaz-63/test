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
using System.IO;

/// --------------------------------------------------
/// <summary>
/// 出荷計画処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsS01Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsS01Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region S0100010:出荷計画登録

    #region 初期データ取得(出荷計画)

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得(出荷計画)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>H.Tajimi 2020/04/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetInitShukkaKeikaku(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
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

    #region 納入先マスタ登録・更新・削除

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ登録・更新・削除
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true;成功/false:失敗</returns>
    /// <create>H.Tajimi 2018/11/01</create>
    /// <update>H.Tajimi 2020/04/14 出荷元マスタチェック追加</update>
    /// <update>R.Miyoshi 2023/07/21 追加・更新項目の追加(SyoriFlag～Biko)</update>
    /// <update>T.SASAYAMA 2023/08/09 行削除・排他制御の追加</update>
    /// --------------------------------------------------
    public bool InsShukkaKeikaku(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 納入先の追加、更新
            using (var masterImpl = new WsMasterImpl())
            {
                DataTable dt = ds.Tables[Def_M_NONYUSAKI.Name];
                DataTable dtInsert = ds.Tables[ComDefine.DTTBL_INSERT];
                DataTable dtDelete = ds.Tables[ComDefine.DTTBL_DELETE];
                DataTable dtUpdate = ds.Tables[ComDefine.DTTBL_UPDATE];
                dtInsert.Merge(dtUpdate);

                // 新しいテーブルを作成して列の構造をコピー
                DataTable dtNew = dt.Clone();

                // dtInsert に含まれているSHIP値を取得
                HashSet<string> shipValuesInInsert = new HashSet<string>(dtInsert.AsEnumerable().Select(row => row.Field<string>(Def_M_NONYUSAKI.SHIP)));

                // dt の行を走査し、SHIP値が dtInsert に含まれていない行を新しいテーブルにコピー
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        string shipValue = row.Field<string>(Def_M_NONYUSAKI.SHIP);
                        if (!shipValuesInInsert.Contains(shipValue))
                        {
                            dtNew.ImportRow(row);
                        }
                    }
                }

                var bukkenName = cond.BukkenName;
                var shipNo = ComFunc.GetFld(dt, 0, Def_M_NONYUSAKI.SHIP_NO);
                var shipSeiban = cond.ShipSeiban;
                var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;

                // 物件名マスタ取得
                var dtBukken = this.GetBukken(dbHelper, shukkaFlag, bukkenName);
                if (!UtilData.ExistsData(dtBukken))
                {
                    // 該当する物件名はありません。
                    errMsgID = "S0100010006";
                    return false;
                }

                var bukkenNo = ComFunc.GetFld(dtBukken, 0, Def_M_BUKKEN.BUKKEN_NO);

                //データチェックのはじまり
                DataTable dtCheck = this.LockNonyusaki(dbHelper, shukkaFlag, bukkenNo);
                int index;
                int[] notFoundIndex = null;

                // 変更データのバージョンチェック
                index = this.CheckSameData(dtUpdate, dtCheck, out notFoundIndex, Def_M_NONYUSAKI.VERSION, Def_M_NONYUSAKI.NONYUSAKI_CD);
                if (0 <= index)
                {
                    // TagNo.[{0}]は他端末で更新された為、更新できませんでした。
                    errMsgID = "S0100020062";
                    args = new string[] { ComFunc.GetFld(dtUpdate, index, Def_M_NONYUSAKI.SHIP) };
                    return false;
                }
                else if (notFoundIndex != null)
                {
                    // TagNo.[{0}]は他端末で削除された為、更新できませんでした。
                    errMsgID = "S0100020063";
                    args = new string[] { ComFunc.GetFld(dtUpdate, notFoundIndex[0], Def_M_NONYUSAKI.SHIP) };
                    return false;
                }

                // 削除データのバージョンチェック
                index = this.CheckSameData(dtDelete, dtCheck, out notFoundIndex, Def_M_NONYUSAKI.VERSION, Def_M_NONYUSAKI.NONYUSAKI_CD);
                if (0 <= index)
                {
                    // TagNo.[{0}]は他端末で更新された為、更新できませんでした。
                    errMsgID = "S0100020062";
                    args = new string[] { ComFunc.GetFld(dtDelete, index, Def_M_NONYUSAKI.SHIP) };
                    return false;
                }


                // 行データの削除のはじまり
                if (dtDelete != null && dtDelete.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDelete.Rows)
                    {
                        var ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                        var nonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);

                        // 出荷明細Dataが存在するため削除できませんでした。【便:{0}】
                        if (this.ExistsShukkaMeisai(dbHelper, shukkaFlag, nonyusakiCD))
                        {
                            errMsgID = "S0100010010";
                            args = new string[] { ship };
                            return false;
                        }
                        // 荷姿Dataが存在するため削除できませんでした。【便:{0}】
                        if (this.ExistsPacking(dbHelper, shukkaFlag, nonyusakiCD))
                        {
                            errMsgID = "S0100010011";
                            args = new string[] { ship };
                            return false;
                        }
                        // 削除に失敗しました。【便:{0}】
                        if (this.DelNonyusakiRow(dbHelper, cond, nonyusakiCD) != 1)
                        {
                            errMsgID = "S0100010009";
                            args = new string[] { ship };
                            return false;
                        }
                    }
                }

                //追加・更新処理のはじまり
                foreach (DataRow dr in dtInsert.Rows)
                {
                    var shoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                    var ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    if (shoriFlag != SHIPPING_PLAN_EXCEL_TYPE.NONE_VALUE1)
                    {
                        // 追加・更新
                        // 納入先マスタ取得
                        var dtNonyusaki = this.GetNonyusaki(dbHelper, shukkaFlag, bukkenName, ship);

                        // 追加・更新の共通項目設定
                        var condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                        condNonyusaki.ShukkaFlag = shukkaFlag;
                        condNonyusaki.Ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                        condNonyusaki.TransportFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.TRANSPORT_FLAG);
                        condNonyusaki.EstimateFlag = ComFunc.GetFld(dr, Def_M_NONYUSAKI.ESTIMATE_FLAG);
                        if (condNonyusaki.EstimateFlag == "-1")
                        { condNonyusaki.EstimateFlag = ""; }
                        condNonyusaki.ShipDate = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_DATE);
                        if (condNonyusaki.ShipDate.Length > 10) 
                        { condNonyusaki.ShipDate = condNonyusaki.ShipDate.Substring(0, 10); }
                        condNonyusaki.ShipFrom = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_FROM);
                        condNonyusaki.ShipTo = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_TO);
                        condNonyusaki.ShipNo = shipNo;
                        condNonyusaki.ShipSeiban = shipSeiban;
                        condNonyusaki.ShipFromCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_FROM_CD);
                        condNonyusaki.SyoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                        condNonyusaki.Seiban = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SEIBAN);
                        condNonyusaki.Kishu = ComFunc.GetFld(dr, Def_M_NONYUSAKI.KISHU);
                        condNonyusaki.Naiyo = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NAIYO);
                        condNonyusaki.TouchakuyoteiDate = ComFunc.GetFld(dr, Def_M_NONYUSAKI.TOUCHAKUYOTEI_DATE);
                        if (condNonyusaki.TouchakuyoteiDate.Length > 10)
                        { condNonyusaki.TouchakuyoteiDate = condNonyusaki.TouchakuyoteiDate.Substring(0, 10); }
                        condNonyusaki.KikaiParts = ComFunc.GetFld(dr, Def_M_NONYUSAKI.KIKAI_PARTS);
                        condNonyusaki.SeigyoParts = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SEIGYO_PARTS);
                        condNonyusaki.Biko = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BIKO);
                        condNonyusaki.ConsignName = cond.ConsignName;
                        condNonyusaki.NonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);

                        if (UtilData.ExistsData(dtNonyusaki) || !string.IsNullOrEmpty(condNonyusaki.NonyusakiCD))
                        {
                            // 更新
                            string nonyusakiCd = null;
                            if (string.IsNullOrEmpty(condNonyusaki.NonyusakiCD))
                            {
                                nonyusakiCd = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                            }
                            else
                            {
                                nonyusakiCd = condNonyusaki.NonyusakiCD;
                            }
                            if (this.ExistsPacking(dbHelper, shukkaFlag, nonyusakiCd))
                            {
                                // 荷姿Dataが存在するため更新できませんでした。【便:{0}】
                                errMsgID = "S0100010012";
                                args = new string[] { ship };
                                return false;
                            }

                            condNonyusaki.NonyusakiCD = nonyusakiCd;
                            // 更新処理
                            if (masterImpl.UpdNonyusaki(dbHelper, condNonyusaki) != 1)
                            {
                                // 更新に失敗しました。【便:{0}】
                                errMsgID = "S0100010008";
                                args = new string[] { ship };
                                return false;
                            }
                        }
                        else
                        {
                            // 追加
                            // 納入先コードの採番
                            using (var smsImpl = new WsSmsImpl())
                            {
                                var condSms = new CondSms(cond.LoginInfo);
                                condSms.SaibanFlag = SAIBAN_FLAG.US_VALUE1;
                                string tmpNonyusakiCd;
                                if (!smsImpl.GetSaiban(dbHelper, condSms, out tmpNonyusakiCd, out errMsgID))
                                {
                                    return false;
                                }
                                condNonyusaki.NonyusakiCD = tmpNonyusakiCd;
                            }
                            condNonyusaki.BukkenNo = bukkenNo;
                            condNonyusaki.NonyusakiName = bukkenName;
                            condNonyusaki.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;
                            condNonyusaki.CreateUserID = cond.LoginInfo.UserID;
                            condNonyusaki.CreateUserName = cond.LoginInfo.UserName;
                            condNonyusaki.UpdateUserID = cond.LoginInfo.UserID;
                            condNonyusaki.UpdateUserName = cond.LoginInfo.UserName;
                            condNonyusaki.MainteDate = DateTime.Now;
                            condNonyusaki.MainteUserID = cond.LoginInfo.UserID;
                            condNonyusaki.MainteUserName = cond.LoginInfo.UserName;

                            // 登録処理
                            if (masterImpl.InsNonyusaki(dbHelper, condNonyusaki) != 1)
                            {
                                // 登録に失敗しました。【便:{0}】
                                errMsgID = "S0100010007";
                                args = new string[] { ship };
                                return false;
                            }
                        }
                    }
                    ////削除
                    //else if (shoriFlag == SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE2)
                    //{
                    //    var dtNonyusaki = this.GetNonyusaki(dbHelper, shukkaFlag, bukkenName, ship);
                    //    var condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                    //    condNonyusaki.SyoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);

                    //    // 更新
                    //    var nonyusakiCd = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    //    if (this.ExistsPacking(dbHelper, shukkaFlag, nonyusakiCd))
                    //    {
                    //        // 荷姿Dataが存在するため更新できませんでした。【便:{0}】
                    //        errMsgID = "S0100010012";
                    //        args = new string[] { ship };
                    //        return false;
                    //    }

                    //    condNonyusaki.NonyusakiCD = nonyusakiCd;
                    //    // 更新処理
                    //    if (masterImpl.UpdNonyusaki(dbHelper, condNonyusaki) != 1)
                    //    {
                    //        // 更新に失敗しました。【便:{0}】
                    //        errMsgID = "S0100010008";
                    //        args = new string[] { ship };
                    //        return false;
                    //    }
                    //}
                }
                foreach (DataRow dr in dtNew.Rows)
                {
                    var condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                    condNonyusaki.ShukkaFlag = shukkaFlag;
                    condNonyusaki.NonyusakiCD = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    condNonyusaki.BukkenNo = bukkenNo;
                    condNonyusaki.Ship = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);
                    condNonyusaki.ShipSeiban = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP_SEIBAN);
                    if (ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG) == SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE1
                        && ComFunc.GetFld(dr, Def_M_NONYUSAKI.LAST_SYORI_FLAG) == SHIPPING_PLAN_EXCEL_TYPE.DELETE_VALUE1)
                    {
                        condNonyusaki.SyoriFlag = "0";
                    }
                    else 
                    {
                        condNonyusaki.SyoriFlag = ComFunc.GetFld(dr, ComDefine.FLD_EXCEL_SHORI_FLAG);
                    }
                    masterImpl.UpdNonyusakiForLastSyoriFalg(dbHelper, condNonyusaki);
 
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

    #region 出荷計画のDB更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画のDB更新
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/09/01</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdPlanning(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtMail = ds.Tables[Def_T_MAIL.Name];

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

    #region リビジョン登録
    /// -------------------------------------------------
    /// <summary>
    /// リビジョン登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>bool</returns>
    /// <create>T.SASAYAMA 2023/08/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsertRevision(DatabaseHelper dbHelper, CondS01 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            if (!string.IsNullOrEmpty(cond.Version))
            {
                DataSet ds = this.GetNonyusaki(dbHelper, cond);
                if (ds.Tables.Contains(Def_M_MAIL_SEND_RIREKI.Name))
                {
                    var revTable = ds.Tables[Def_M_MAIL_SEND_RIREKI.Name];
                    if (revTable.Rows.Count > 0)
                    {
                        var revVersion = ComFunc.GetFldToDateTime(revTable, 0, Def_M_MAIL_SEND_RIREKI.VERSION).ToString("yyyy/MM/dd HH:mm:ss");
                        if (cond.Version != revVersion)
                        {
                            // 他端末で更新された為、更新できませんでした。
                            errMsgID = "A9999999027";
                            return false;
                        }
                    }
                }
            }
            bool ret = InsertRevision(dbHelper, cond);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #endregion

    #region S0100020:出荷計画明細登録

    #region 36進変換
    /// --------------------------------------------------
    /// <summary>
    /// 36進変換
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    private string ConvertBase36(decimal num)
    {
        string ret = string.Empty;

        // 取得No.の36進表記取得
        decimal quotient = num;
        decimal remainder = 0;
        while (quotient > 0)
        {
            remainder = quotient % 36;
            if (remainder < 10)
                ret = remainder.ToString() + ret;
            else
                ret = Encoding.ASCII.GetString(new byte[] { byte.Parse((remainder + 55).ToString()) }) + ret;

            quotient = Math.Floor(quotient / 36);
        }

        return ret.PadLeft(4, '0');
    }
    #endregion

    #region 表示条件チェック

    /// --------------------------------------------------
    /// <summary>
    /// 表示条件チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">納入先マスタ、AR情報データ</param>
    /// <param name="bukkenNO">物件管理NO</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true;成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>T.Nakata 2018/11/14</update>
    /// --------------------------------------------------
    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって納入先コードは画面から渡される)
    //public bool CheckDisplayCondition(DatabaseHelper dbHelper, CondS01 cond, ref DataSet ds, ref string nonyusakiCD, ref string errMsgID, ref string[] args)
    public bool CheckDisplayCondition(DatabaseHelper dbHelper, CondS01 cond, ref DataSet ds, ref string bukkenNO, ref string errMsgID, ref string[] args)
    // ↑
    {
        try
        {
            // 出荷区分が本体の時は納入先がなくてもエラーとしない為
            // WsSmsImplのCheckNonyusakiAndARNoを使用できない。

            // 納入先を取得する為のコンディションを設定

            CondNonyusaki condsaki = new CondNonyusaki(cond.LoginInfo);
            condsaki.ShukkaFlag = cond.ShukkaFlag;
            // 2012/04/24 K.Tsutsumi Change キーでは無くなった
            //condsaki.NonyusakiName = cond.NonyusakiName;
            condsaki.NonyusakiCD = cond.NonyusakiCD;
            // ↑
            condsaki.Ship = cond.Ship;
            // 納入先の取得
            using (WsMasterImpl master = new WsMasterImpl())
            {
                ds = master.GetNonyusaki(dbHelper, condsaki);
            }
            if (ds == null || !ds.Tables.Contains(Def_M_NONYUSAKI.Name) || ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count < 1)
            {
                if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // 該当納入先がありません。AR情報を登録してください。
                    errMsgID = "S0100020011";
                    return false;
                }
                else if (!cond.ModeInsert && !cond.ModeExcel)
                {
                    // 納入先、便に一致する納入先が存在しません。
                    errMsgID = "A9999999021";
                    return false;
                }
            }
            // 2011/02/28 K.Tsutsumi Add 削除予定納入先チェック
            else
            {
                // 取得できた場合は、削除予定となっていないかチェックし、削除予定の場合は一切の処理が不可能である
                if ((ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1) &&
                    (ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.REMOVE_FLAG) == REMOVE_FLAG.JYOGAI_VALUE1))
                {
                    // 削除予定納入先となっています。管理者に確認して下さい。
                    errMsgID = "S0100020036";
                    return false;
                }
            }
            // ↑

            if (cond.ModeInsert || cond.ModeExcel)
            {
                // 入力登録、Excel登録のチェック

                // 納入先がないことがあるのでチェック
                if (ds != null && ds.Tables.Contains(Def_M_NONYUSAKI.Name) && 0 < ds.Tables[Def_M_NONYUSAKI.Name].Rows.Count)
                {
                    // AR
                    if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1 && ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1)
                    {
                        // 完了納入先となっています。管理者に確認して下さい。
                        errMsgID = "S0100020017";
                        return false;
                    }
                }
            }
            else if (cond.ModeUpdate || cond.ModeDelete)
            {
                // 変更、削除のチェック

                // 出荷区分が本体で取得した納入先の管理区分が完了の場合エラーとする。
                if (ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1)
                {
                    if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                    {
                        if (cond.ModeUpdate)
                        {
                            // 完了納入先となっており、変更は出来ません。\r\n明細追加等は、入力登録にて行って下さい。
                            errMsgID = "S0100020022";
                        }
                        else
                        {
                            // 完了納入先となっており、削除は出来ません。
                            errMsgID = "S0100020034";
                        }
                    }
                    else
                    {
                        // 完了納入先となっています。管理者に確認して下さい。
                        errMsgID = "S0100020017";
                    }
                    return false;
                }
            }

            // 2012/04/24 K.Tsutsumi Delete 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            //nonyusakiCD = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            bukkenNO = ComFunc.GetFld(ds, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.BUKKEN_NO);
            // ↑

            // AR情報にAR Noが存在するか確認
            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1 && !string.IsNullOrEmpty(cond.ARNo))
            {
                using (WsSmsImpl impl = new WsSmsImpl())
                {
                    CondSms condSms = new CondSms(cond.LoginInfo);
                    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
                    //condSms.NonyusakiCD = nonyusakiCD;
                    condSms.NonyusakiCD = cond.NonyusakiCD;
                    // ↑
                    // ↓ 2018/11/14 T.Nakata 複数AR対応
                    string[] tmpAR = cond.ARNo.Split(',');
                    foreach (string ARNo in tmpAR)
                    {
                        if (string.IsNullOrEmpty(ARNo)) break;

                        condSms.ARNo = ARNo;
                        //condSms.ARNo = cond.ARNo;
                        DataSet dsAR = impl.GetAR(dbHelper, condSms);
                        // 2011/03/08 K.Tsutsumi Change T_ARが存在しなくても続行可能
                        //if (dsAR == null || !dsAR.Tables.Contains(Def_T_AR.Name) || dsAR.Tables[Def_T_AR.Name].Rows.Count < 1)
                        //{
                        //    // AR No.が存在しません。
                        //    errMsgID = "A9999999020";
                        //    return false;
                        //}
                        //ds.Tables.Add(dsAR.Tables[Def_T_AR.Name].Copy());
                        //
                        //// 変更、削除のチェック
                        //if (cond.ModeUpdate || cond.ModeDelete)
                        //{
                        //    // 状況区分が完了であればエラーとする。
                        //    if (ComFunc.GetFld(ds, Def_T_AR.Name, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                        //    {
                        //        // 完了AR No.となっています。
                        //        errMsgID = "S0100020018";
                        //        return false;
                        //    }
                        //}

                        // T_AR取得成功
                        if (ComFunc.IsExistsData(dsAR, Def_T_AR.Name) == true)
                        {
                            // テーブル待避
                            string tmpTableName = Def_T_AR.Name + "-" + ARNo;
                            dsAR.Tables[Def_T_AR.Name].TableName = tmpTableName;
                            ds.Tables.Add(dsAR.Tables[tmpTableName].Copy());

                            // 変更、削除のチェック
                            if (cond.ModeUpdate || cond.ModeDelete)
                            {
                                // 状況区分が完了であればエラーとする。
                                if (ComFunc.GetFld(ds, tmpTableName, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                                {
                                    // 完了AR No.となっています。
                                    errMsgID = "S0100020018";
                                    return false;
                                }
                            }
                        }
                        // ↑
                    }
                    // ↑ 2018/11/14 T.Nakata 複数AR対応
                }
            }

            // Box梱包以降のデータが存在するかチェック
            if (cond.ModeDelete)
            {
                CondS01 condDataCount = new CondS01(cond.LoginInfo);
                condDataCount.ShukkaFlag = cond.ShukkaFlag;
                // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
                //condDataCount.NonyusakiCD = nonyusakiCD;
                condDataCount.NonyusakiCD = cond.NonyusakiCD;
                // ↑
                condDataCount.ARNo = cond.ARNo;
                // 削除時のみのチェック
                if (0 < this.GetShukkaMeisaiOverBoxKonpoDataCount(dbHelper, condDataCount))
                {
                    // 削除できない状態です。\r\n集荷済の状態まで戻した後に削除してください。
                    errMsgID = "S0100020023";
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

    #region 手配明細存在チェック
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細存在チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">出荷明細データ</param>
    /// <param name="isLock">ロック有無</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckAndLockTehaiMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dt, bool isLock, ref string errMsgID, ref string[] args)
    {
        // 連携Noの有無確認(あればロックする)
        foreach (DataRow drTmp in dt.Rows)
        {
            if (!string.IsNullOrEmpty(ComFunc.GetFld(drTmp, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO)))
            {
                DataSet dsTR = new DataSet();
                DataTable dtTR = new DataTable(Def_T_SHUKKA_MEISAI.Name);
                dsTR.Tables.Add(dtTR);
                dtTR.Columns.Add(Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, typeof(string));
                DataRow drTR = dtTR.NewRow();
                dtTR.Rows.Add(drTR);
                drTR[Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO] = ComFunc.GetFld(drTmp, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO);
                DataTable dtTagRenkei = GetAndLockTagRenkeiList(dbHelper, new CondS01(cond.LoginInfo), dsTR, isLock);

                if (dtTagRenkei == null || dtTagRenkei.Rows.Count < 1)
                {
                    // 連携No[{0}]は存在していません。
                    errMsgID = "S0100020047";
                    args = new string[] { ComFunc.GetFld(drTmp, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO) };
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

    #region TagNo取得
    /// --------------------------------------------------
    /// <summary>
    /// TagNo取得
    /// </summary>
    /// <param name="SrcTagNo">変換前値</param>
    /// <param name="DstTagNo">変換後値</param>
    /// <param name="TagNo">変換後文字列</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>T.Nakata 2018/11/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetTagNo(decimal SrcTagNo, ref decimal DstTagNo, ref string TagNo)
    {
        DstTagNo = SrcTagNo;
        TagNo = string.Empty;
        if ((0 <= SrcTagNo) && (SrcTagNo <= 99999))
        {   // 10進数での取得
            TagNo = SrcTagNo.ToString();
        }
        else if ((16796160 <= SrcTagNo) && (SrcTagNo <= 2176782335))
        {   // 36進数での取得
            TagNo = ConvertBase36(SrcTagNo);
        }
        else if ((99999 < SrcTagNo) && (SrcTagNo < 16796160))
        {   // 10進→36進切り替わり:36進数は A0000 から開始
            DstTagNo = 16796160;
            TagNo = ConvertBase36(DstTagNo);
        }
        else
        {   // 取得範囲外
            return false;
        }
        TagNo = TagNo.PadLeft(5, '0');
        return true;
    }
    #endregion

    #region 写真有無取得

    /// --------------------------------------------------
    /// <summary>
    /// 写真有無取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetExistsPicture(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            var ret = this.Sql_GetExistsPicture(dbHelper, cond);
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 写真有無取得
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetExistsPicture(DatabaseHelper dbHelper, CondS01 cond, DataTable dt)
    {
        try
        {
            var ret = this.Sql_GetExistsPicture(dbHelper, cond, dt);
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷計画明細データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">出荷明細データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>T.Nakata 2018/11/14</update>
    /// --------------------------------------------------
    public bool InsShukkaKeikakuMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 2018/11/15 T.Nakata 連携Noチェック及び数量チェック追加
            // 連携Noの有無及び数量確認(あればロックする)
            if (!CheckAndLockTehaiMeisai(dbHelper, cond, dt, true, ref errMsgID, ref args))
            {
                return false;
            }
            // ↑

            // 納入先の追加、更新
            using (WsMasterImpl masterImpl = new WsMasterImpl())
            {
                // 納入先存在チェック
                CondNonyusaki condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                condNonyusaki.ShukkaFlag = cond.ShukkaFlag;
                // 2012/04/24 K.Tsutsumi Change キーでは無くなった
                //condNonyusaki.NonyusakiName = cond.NonyusakiName;
                condNonyusaki.NonyusakiCD = cond.NonyusakiCD;
                // ↑
                condNonyusaki.Ship = cond.Ship;
                condNonyusaki.LoginInfo = cond.LoginInfo;
                DataSet dsMaster = masterImpl.GetNonyusaki(dbHelper, condNonyusaki);
                if (!ComFunc.IsExistsData(dsMaster, Def_M_NONYUSAKI.Name))
                {
                    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている
                    //// 納入先コードの採番
                    //CondSms condSms = new CondSms();
                    //condSms.SaibanFlag = SAIBAN_FLAG.US_VALUE1;
                    //condSms.NonyusakiName = cond.NonyusakiName;
                    //condSms.Ship = cond.Ship;
                    //condSms.LoginInfo = cond.LoginInfo;
                    //using (WsSmsImpl impl = new WsSmsImpl())
                    //{
                    //    string nonyusakiCD;
                    //    if (!impl.GetSaiban(dbHelper, condSms, out nonyusakiCD, out errMsgID))
                    //    {
                    //        return false;
                    //    }
                    //    cond.NonyusakiCD = nonyusakiCD;
                    //}
                    //// 納入先の追加
                    //condNonyusaki.NonyusakiCD = cond.NonyusakiCD;
                    //condNonyusaki.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;
                    //// 2011/03/01 K.Tsutsumi Add No36
                    //condNonyusaki.RemoveFlag = REMOVE_FLAG.NORMAL_VALUE1;
                    //// ↑
                    //if (condNonyusaki.Ship == null)
                    //{
                    //    condNonyusaki.Ship = string.Empty;
                    //}
                    //masterImpl.InsNonyusaki(dbHelper, condNonyusaki);

                    // 納入先、便に一致する納入先が存在しません。
                    errMsgID = "A9999999021";
                    return false;
                    // ↑
                }
                else
                {
                    // 納入先の更新
                    // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、画面から渡される）
                    //cond.NonyusakiCD = ComFunc.GetFld(dsMaster, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    //condNonyusaki.NonyusakiCD = cond.NonyusakiCD;

                    cond.BukkenNO = ComFunc.GetFld(dsMaster, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                    // ↑
                    if (ComFunc.GetFld(dsMaster, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.KANRI_FLAG) == KANRI_FLAG.KANRYO_VALUE1)
                    {
                        condNonyusaki.KanriFlag = KANRI_FLAG.MIKAN_VALUE1;
                        masterImpl.UpdNonyusaki(dbHelper, condNonyusaki);
                    }
                }
            }

            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                using (WsSmsImpl impl = new WsSmsImpl())
                {
                    CondSms condSms = new CondSms(cond.LoginInfo);
                    condSms.NonyusakiCD = cond.NonyusakiCD;

                    // ↓ 2018/11/14 T.Nakata 複数AR対応
                    string bk_arno = cond.ARNo;
                    string[] tmpArNo = cond.ARNo.Split(',');
                    foreach (string ArNo in tmpArNo)
                    {
                        if (string.IsNullOrEmpty(ArNo)) break;

                        condSms.ARNo = ArNo;
                        DataSet dsAR = impl.GetAR(dbHelper, condSms);
                        // 2011/03/08 K.Tsutsumi Change T_ARが存在しなくても続行可能
                        //if (!ComFunc.IsExistsData(dsAR, Def_T_AR.Name))
                        //{
                        //    // AR No.が存在しません。
                        //    errMsgID = "A9999999020";
                        //    return false;
                        //}
                        //if (ComFunc.GetFld(dsAR, Def_T_AR.Name, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                        //{
                        //    this.UpdAR(dbHelper, cond);
                        //}

                        if (ComFunc.IsExistsData(dsAR, Def_T_AR.Name) == true)
                        {
                            if (ComFunc.GetFld(dsAR, Def_T_AR.Name, 0, Def_T_AR.JYOKYO_FLAG) == JYOKYO_FLAG.KANRYO_VALUE1)
                            {
                                cond.ARNo = ArNo;
                                this.UpdAR(dbHelper, cond);
                            }
                        }
                        // ↑
                    }
                    cond.ARNo = bk_arno;
                    // ↑ 2018/11/14 T.Nakata 複数AR対応
                }
            }

            // 出荷明細データの登録は納入先マスタの一意制約違反が出なかった場合のみ行うようにしないといけない。
            if (!this.InsShukkaMeisai(dbHelper, cond, dt, ref errMsgID, ref args))
            {
                return false;
            }

            // 2012/05/07 K.Tsutsumi Add 履歴
            // 履歴データ
            // ↓ 2018/11/14 T.Nakata 複数AR対応
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                string[] tmpArNo = cond.ARNo.Split(',');
                foreach (string ArNo in tmpArNo)
                {
                    if (string.IsNullOrEmpty(ArNo)) break;
                    cond.ARNo = ArNo;
                    this.InsRireki(dbHelper, cond);
                }
            }
            else
            {
                this.InsRireki(dbHelper, cond);
            }
            // ↑ 2018/11/14 T.Nakata 複数AR対応
            // ↑

            // ↓ 2018/11/19 T.Nakata 手配業務対応
            foreach (DataRow dr in dt.Rows)
            {
                DataTable dt_ShukkaNum = this.CheckShukkaQtyNum(dbHelper, dr, cond);
                if (dt_ShukkaNum.Rows.Count > 0)
                {
                    // 連携No[{0}]の数量({1})の合計が出荷数({2})を超えています。
                    errMsgID = "S0100020048";
                    args = new string[] { ComFunc.GetFld(dt_ShukkaNum,0,Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)
                                         ,ComFunc.GetFld(dt_ShukkaNum,0,Def_T_SHUKKA_MEISAI.NUM)
                                         ,ComFunc.GetFld(dt_ShukkaNum,0,Def_T_TEHAI_MEISAI.SHUKKA_QTY) };
                    return false;
                }
            }
            // ↑

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷明細データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">出荷明細データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update>Y.Higuchi 2010/10/28</update>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>D.Okumura 2019/01/29 不具合修正に伴い不要なコードを削除</update>
    /// --------------------------------------------------
    private bool InsShukkaMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // 2012/04/24 K.Tsutsumi Change TagNoの自動採番
        //// 出荷明細データチェック
        //DataSet ds = this.GetShukkaMeisaiTagNoCheck(dbHelper, cond);
        //if (ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
        //{
        //    DataTable dtTagNoCheck = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        string tagNo = ComFunc.GetFld(dt, i, Def_T_SHUKKA_MEISAI.TAG_NO);
        //        DataRow[] drs = dtTagNoCheck.Select(string.Format("TAG_NO = '{0}'", tagNo));
        //        if (0 < drs.Length)
        //        {
        //            // {0}行目のTagNo.[{1}]は既に存在します。
        //            errMsgID = "S0100020002";
        //            args = new string[] { (i + 1).ToString(), tagNo };
        //            return false;
        //        }
        //    }
        //}
        DataTable dtBukken = this.LockBukken(dbHelper, cond.ShukkaFlag, cond.BukkenNO);
        if ((dtBukken == null) || (dtBukken.Rows.Count < 1))
        {
            // 納入先と紐付く物件名マスタが取得できませんでした。
            errMsgID = "S0100020042";
            return false;
        }

        decimal IssuedTagNo = ComFunc.GetFldToDecimal(dtBukken, 0, Def_M_BUKKEN.ISSUED_TAG_NO);
        // ↓ 2018/11/20 T.Nakata 採番No36進数化
        //if ((1 > IssuedTagNo + dt.Rows.Count) || (IssuedTagNo + dt.Rows.Count > 99999))
        string tmpStr = string.Empty;
        decimal tmpNum = 0;
        if (!this.GetTagNo((IssuedTagNo + dt.Rows.Count), ref tmpNum, ref tmpStr))
        // ↑
        {
            // 使用可能なTagNo.が足りません。
            errMsgID = "A9999999050";
            return false;
        }
        // ↑

        // 出荷明細データ登録
        string[] tmpArNo = new string[0];
        if (!string.IsNullOrEmpty(cond.ARNo))
        {
            tmpArNo = cond.ARNo.Split(',');
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            while (true)
            {
                // ↓ 2018/11/20 T.Nakata 採番No36進数化
                // 2012/04/24 K.Tsutsumi Add TagNoの自動採番
                //if ((1 > IssuedTagNo + 1) || (IssuedTagNo + 1 > 99999))
                IssuedTagNo++;
                string TagNoStr = string.Empty;
                if (!this.GetTagNo(IssuedTagNo, ref IssuedTagNo, ref TagNoStr))
                {
                    // 使用可能なTagNo.がありません。
                    errMsgID = "A9999999051";
                    return false;
                }
                // 自動採番したTagNoをセット 
                DSWUtil.UtilData.SetFld(dt, i, Def_T_SHUKKA_MEISAI.TAG_NO, TagNoStr);
                // ↑
                // ↑ 2018/11/20 T.Nakata 採番No36進数化

                try
                {
                    // ↓2019/01/29 D.Okumura Change 不具合対応　不要なコードを削除
                    //if (tmpArNo.Length > i)
                    //{
                    //    cond.ARNo = tmpArNo[i];
                    //}
                    // ↑2019/01/29 D.Okumura Change 不具合対応　不要なコードを削除
                    this.InsShukkaMeisaiExec(dbHelper, dt.Rows[i], cond);
                    break;
                }
                catch (Exception ex)
                {
                    // 2012/04/24 K.Tsutsumi Change TagNo自動採番（新しいTagNoを再度取得）
                    //// 一意制約違反チェック
                    //if (this.IsDbDuplicationError(ex))
                    //{
                    //    // {0}行目のTagNo.[{1}]は既に存在します。
                    //    errMsgID = "S0100020002";
                    //    args = new string[] { (i + 1).ToString(), ComFunc.GetFld(dt, i, Def_T_SHUKKA_MEISAI.TAG_NO) };
                    //    return false;
                    //}
                    //else
                    //{
                    //    throw new Exception(ex.Message, ex);
                    //}

                    if (this.IsDbDuplicationError(ex) == false)
                    {
                        // 一意制約違反以外
                        throw new Exception(ex.Message, ex);
                    }
                    // ↑
                }
            }
        }

        // 物件名マスタ更新
        UtilData.SetFld(dtBukken, 0, Def_M_BUKKEN.ISSUED_TAG_NO, IssuedTagNo);
        this.UpdBukken(dbHelper, dtBukken, cond);

        return true;

    }

    #endregion

    #region 出荷計画明細データ修正

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ修正
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>T.Nakata 2018/11/15 連携Noチェック及び数量チェック追加</update>
    /// --------------------------------------------------
    public bool UpdShukkaKeikakuMeisai(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataSet dsCheck = new DataSet();
            // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            //string nonyusakiCD = string.Empty;
            //if (!this.CheckDisplayCondition(dbHelper, cond, ref dsCheck, ref nonyusakiCD, ref errMsgID, ref args))
            string bukkenNO = string.Empty;
            if (!this.CheckDisplayCondition(dbHelper, cond, ref dsCheck, ref bukkenNO, ref errMsgID, ref args))
            // ↑
            {
                return false;
            }
            // 2012/05/07 K.Tsutsumi Add 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            cond.BukkenNO = bukkenNO;
            // ↑

            DataTable dtCheck = this.LockShukkaMeisai(dbHelper, cond.ShukkaFlag, cond.NonyusakiCD, cond.ARNo);
            DataTable dtInsert = ds.Tables[ComDefine.DTTBL_INSERT];
            DataTable dtUpdate = ds.Tables[ComDefine.DTTBL_UPDATE];
            DataTable dtDelete = ds.Tables[ComDefine.DTTBL_DELETE];
            int index;

            // 変更データのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtUpdate, dtCheck, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.TAG_NO);
            if (0 <= index)
            {
                // TagNo.[{0}]は他端末で更新された為、更新できませんでした。
                errMsgID = "S0100020020";
                args = new string[] { ComFunc.GetFld(dtUpdate, index, Def_T_SHUKKA_MEISAI.TAG_NO) };
                return false;
            }
            else if (notFoundIndex != null)
            {
                // TagNo.[{0}]は他端末で削除された為、更新できませんでした。
                errMsgID = "S0100020028";
                args = new string[] { ComFunc.GetFld(dtUpdate, notFoundIndex[0], Def_T_SHUKKA_MEISAI.TAG_NO) };
                return false;
            }

            // 削除データのバージョンチェック
            index = this.CheckSameData(dtDelete, dtCheck, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.TAG_NO);
            if (0 <= index)
            {
                // TagNo.[{0}]は他端末で更新された為、更新できませんでした。
                errMsgID = "S0100020020";
                args = new string[] { ComFunc.GetFld(dtDelete, index, Def_T_SHUKKA_MEISAI.TAG_NO) };
                return false;
            }
            // 出荷明細データの削除
            foreach (DataRow dr in dtDelete.Rows)
            {
                this.DelShukkaMeisai(dbHelper, cond, ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO));
            }

            // 2018/11/15 T.Nakata 連携Noチェック及び数量チェック追加
            // 連携Noの有無及び数量確認(あればロックする)
            if (!CheckAndLockTehaiMeisai(dbHelper, cond, dtInsert, true, ref errMsgID, ref args))
            {
                return false;
            }
            if (!CheckAndLockTehaiMeisai(dbHelper, cond, dtUpdate, true, ref errMsgID, ref args))
            {
                return false;
            }
            // ↑

            // 出荷明細データの更新
            this.UpdShukkaMeisai(dbHelper, cond, dtUpdate);

            // 出荷明細データの追加
            if (0 < dtInsert.Rows.Count)
            {
                if (!this.InsShukkaMeisai(dbHelper, cond, dtInsert, ref errMsgID, ref args))
                {
                    return false;
                }
            }

            // 2015/12/09 H.Tajimi M_NO対応
            if (dtUpdate != null && dtUpdate.Rows.Count > 0)
            {
                // KOJI_NOとCASE_IDが設定済みのデータをKOJI_NOで集約
                var kojiNos = dtUpdate.AsEnumerable().Where(x => ComFunc.GetFldObject(x, Def_T_SHUKKA_MEISAI.KOJI_NO) != DBNull.Value
                                                              && ComFunc.GetFldObject(x, Def_T_SHUKKA_MEISAI.CASE_ID) != DBNull.Value)
                                                     .GroupBy(x => ComFunc.GetFld(x, Def_T_SHUKKA_MEISAI.KOJI_NO));
                foreach (var item in kojiNos)
                {
                    var kojiNo = item.Key;
                    // CASE_IDを重複なしで取得
                    var arrayCaseId = item.Select(x => ComFunc.GetFld(x, Def_T_SHUKKA_MEISAI.CASE_ID)).Distinct().ToArray();
                    foreach (var caseId in arrayCaseId)
                    {
                        using (WsK02Impl k02Impl = new WsK02Impl())
                        {
                            CondK02 condK02 = new CondK02(cond.LoginInfo);
                            condK02.KojiNo = kojiNo;
                            condK02.CaseID = caseId;
                            condK02.LoginInfo = cond.LoginInfo;
                            // DESCRIPTION_1の取得
                            var description1 = k02Impl.GetKiwakuMeisaiDescription1(dbHelper, condK02);
                            // 木枠明細の更新
                            this.UpdKiwakuMeisai(dbHelper, condK02, description1);
                        }
                    }
                }
            }
            // ↑

            // 2012/05/07 K.Tsutsumi Add 履歴
            // 履歴データ
            this.InsRireki(dbHelper, cond);
            // ↑

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷計画明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画明細データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">削除データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update>K.Tsutsumi 2019/02/09 納入先の削除において、出荷計画情報が入っているものはここでは削除しない</update>
    /// --------------------------------------------------
    public bool DelShukkaKeikakuMeisai(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataSet dsCheck = new DataSet();
            // 2012/04/24 K.Tsutsumi Change 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            //string nonyusakiCD = string.Empty;
            //if (!this.CheckDisplayCondition(dbHelper, cond, ref dsCheck, ref nonyusakiCD, ref errMsgID, ref args))
            string bukkenNO = string.Empty;
            if (!this.CheckDisplayCondition(dbHelper, cond, ref dsCheck, ref bukkenNO, ref errMsgID, ref args))
            // ↑
            {
                return false;
            }
            // 2012/05/07 K.Tsutsumi Add 納入先は事前に登録されている（よって、納入先コードは画面から渡される）
            cond.BukkenNO = bukkenNO;
            // ↑

            DataTable dtCheck = this.LockShukkaMeisai(dbHelper, cond.ShukkaFlag, cond.NonyusakiCD, cond.ARNo);
            DataTable dtDelete = ds.Tables[ComDefine.DTTBL_DELETE];
            int index;

            // データ数チェック
            if (dtCheck.Rows.Count != dtDelete.Rows.Count)
            {
                // 他端末で更新されています。
                errMsgID = "S0100020021";
            }

            // 削除データのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtDelete, dtCheck, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.TAG_NO);
            if (0 <= index)
            {
                // TagNo.[{0}]は他端末で更新された為、更新できませんでした。
                errMsgID = "S0100020020";
                args = new string[] { ComFunc.GetFld(dtDelete, index, Def_T_SHUKKA_MEISAI.TAG_NO) };
                return false;
            }

            // 出荷明細データの削除
            this.DelShukkaMeisai(dbHelper, cond, null);

            // 納入先の削除
            if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                // 出荷計画情報が入っているものはここでは削除しない
                var shipDate = UtilData.GetFld(dsCheck, Def_M_NONYUSAKI.Name, 0, Def_M_NONYUSAKI.SHIP_DATE);
                if (string.IsNullOrEmpty(shipDate))
                {
                    CondNonyusaki condNonyusaki = new CondNonyusaki(cond.LoginInfo);
                    condNonyusaki.ShukkaFlag = cond.ShukkaFlag;
                    condNonyusaki.NonyusakiCD = cond.NonyusakiCD;
                    using (WsMasterImpl impl = new WsMasterImpl())
                    {
                        impl.DelNonyusaki(dbHelper, condNonyusaki);
                    }
                }
            }

            // 2012/05/07 K.Tsutsumi Add 履歴
            // 履歴
            this.InsRireki(dbHelper, cond);
            // ↑

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region S0100030:便間移動

    #region 便間移動更新

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// --------------------------------------------------
    public bool UpdMoveShip(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataTable dtKiwaku = new DataTable();
            DataTable dtKiwMeiRem = new DataTable();
            DataTable dtKiwMeiDest = new DataTable();
            DataTable dtPallet = new DataTable();
            DataTable dtBox = new DataTable();
            DataTable dtTag = new DataTable();
            this.LockTableBinkan(dbHelper, cond, ref dtKiwaku, ref dtKiwMeiRem,
                ref dtKiwMeiDest, ref dtPallet, ref dtBox, ref dtTag);

            // 木枠明細更新
            if (cond.UpdateTani == BINKAN_IDO_TANI.KIWAKU_VALUE1)
            {
                // 木枠明細がなくなったら状態変更
                this.UpdKiwakuRemCase(dbHelper, cond);
                // 木枠明細が追加されれば状態変更
                this.UpdKiwakuSagyoFlag(dbHelper, cond, cond.KojiNoDest, SAGYO_FLAG.KONPOTOROKU_VALUE1);
                // 木枠明細の工事識別No.更新
                this.UpdKiwakuMeisaiKojiNo(dbHelper, cond);
            }
            else
            {
                RemovePalletFromKiwakuMeisai(dbHelper, cond, dtKiwMeiRem);
                AddPalletFromKiwakuMeisai(dbHelper, cond, dtKiwMeiDest);
            }

            // パレット管理データ削除
            if (!string.IsNullOrEmpty(cond.RemovePalletNo))
            {
                this.DelPalletListManage(dbHelper, cond.RemovePalletNo);
            }
            // パレット管理データ更新
            if (cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
                this.UpdPalletListManage(dbHelper, dtPallet, cond);

            // ボックス管理データ削除
            if (!string.IsNullOrEmpty(cond.RemoveBoxNo))
            {
                this.DelBoxListManage(dbHelper, cond.RemoveBoxNo);
            }
            // ボックス管理データ更新
            if (cond.UpdateTani == BINKAN_IDO_TANI.BOX_VALUE1 || cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
                this.UpdBoxListManage(dbHelper, dtBox, cond);

            // 出荷明細データの移動更新
            if (cond.UpdateTani == BINKAN_IDO_TANI.KIWAKU_VALUE1)
            {
                foreach (DataRow dr in dtTag.Rows)
                {
                    UtilData.SetFld(dr, "KOJI_NO", cond.KojiNoDest);
                }
            }
            else
            {
                // 木枠明細移動時以外は状態区分を更新する
                string konpoDt = DateTime.Today.ToString("yyyy/MM/dd");
                foreach (DataRow dr in dtTag.Rows)
                {
                    this.SetKonpoDate(cond, konpoDt, dr);
                    this.SetJyotaiFlag(cond, dr);
                }
            }

            this.UpdIdoShukkaMeisai(dbHelper, dtTag, cond);

            // RemoveCaseId, CaseIdDestのdescription更新
            if (!string.IsNullOrEmpty(cond.CaseIdOrig))
                this.SetKiwakuMeisaDescription(dbHelper, cond, cond.KojiNoOrig, cond.CaseIdOrig);
            if (!string.IsNullOrEmpty(cond.CaseIdDest))
                this.SetKiwakuMeisaDescription(dbHelper, cond, cond.KojiNoDest, cond.CaseIdDest);

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細のDescription更新
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <param name="kojiNo">工事識別No.</param>
    /// <param name="caseId">CaseID</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetKiwakuMeisaDescription(DatabaseHelper dbHelper, CondS01 cond, string kojiNo, string caseId)
    {
        using (WsK02Impl k02Impl = new WsK02Impl())
        {
            CondK02 condK02 = new CondK02(cond.LoginInfo);
            condK02.KojiNo = kojiNo;
            condK02.CaseID = caseId;
            condK02.LoginInfo = cond.LoginInfo;
            // DESCRIPTION_1の取得
            var description1 = k02Impl.GetKiwakuMeisaiDescription1(dbHelper, condK02);
            // 木枠明細の更新
            this.UpdKiwakuMeisai(dbHelper, condK02, description1);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細からパレットの追加削除
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <param name="dtKiwMeiRem">削除対象木枠明細</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void RemovePalletFromKiwakuMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dtKiwMeiRem)
    {

        string fldPalPre = Def_T_KIWAKU_MEISAI.PALLET_NO_1.Substring(0,
            Def_T_KIWAKU_MEISAI.PALLET_NO_1.Length - 1);
        string remPalletNo = "";

        // パレット削除処理
        if (!string.IsNullOrEmpty(cond.CaseIdOrig))
        {
            if (!string.IsNullOrEmpty(cond.RemovePalletNo))
                remPalletNo = cond.RemovePalletNo;
            else if (cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
                remPalletNo = cond.PalletNo;
            else
                return;

            DataRow dr = dtKiwMeiRem.Rows[0];
            int remId = 0;
            for (int i = 1; i < 11; i++)
            {
                string pno = ComFunc.GetFld(dr, fldPalPre + i);
                // 削除パレットNoの位置取得
                if (pno == remPalletNo)
                {
                    remId = i;
                    break;
                }
            }
            // 削除パレットNo以降ひとつづつずらす
            for (int i = remId; i < 10; i++)
            {
                UtilData.SetFld(dr, fldPalPre + i, ComFunc.GetFldObject(dr, fldPalPre + (i + 1)));
            }
            UtilData.SetFld(dr, fldPalPre + 10, null);

            this.UpdKiwakuMeisaiPallet(dbHelper, dtKiwMeiRem, cond);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細からパレットの追加削除
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <param name="dtKiwMeiDest">追加対象木枠明細</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void AddPalletFromKiwakuMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dtKiwMeiDest)
    {

        string fldPalPre = Def_T_KIWAKU_MEISAI.PALLET_NO_1.Substring(0,
            Def_T_KIWAKU_MEISAI.PALLET_NO_1.Length - 1);
        // パレット追加処理
        if (cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1 &&
            !string.IsNullOrEmpty(cond.CaseIdDest))
        {
            DataRow dr = dtKiwMeiDest.Rows[0];

            for (int i = 1; i < 11; i++)
            {
                string pno = ComFunc.GetFld(dr, fldPalPre + i);
                if (string.IsNullOrEmpty(pno))
                {
                    UtilData.SetFld(dr, fldPalPre + i, cond.PalletNo);
                    break;
                }
            }

            this.UpdKiwakuMeisaiPallet(dbHelper, dtKiwMeiDest, cond);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 梱包日をセット
    /// </summary>
    /// <param name="cond">更新条件</param>
    /// <param name="konpoDt">梱包日</param>
    /// <param name="dr"></param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetKonpoDate(CondS01 cond, string konpoDt, DataRow dr)
    {

        // TAG移動時はBox梱包日セット
        if (cond.UpdateTani == BINKAN_IDO_TANI.TAG_VALUE1)
        {
            if (string.IsNullOrEmpty(cond.BoxNo))
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO, DBNull.Value);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.BOXKONPO_DATE, DBNull.Value);
            }
            else
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO, cond.BoxNo);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.BOXKONPO_DATE, konpoDt);
            }
        }

        // TAG, Box移動時はPallet梱包日セット
        if (cond.UpdateTani == BINKAN_IDO_TANI.TAG_VALUE1 ||
            cond.UpdateTani == BINKAN_IDO_TANI.BOX_VALUE1)
        {
            if (string.IsNullOrEmpty(cond.PalletNo))
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO, DBNull.Value);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE, DBNull.Value);
            }
            else
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO, cond.PalletNo);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE, konpoDt);
            }
        }

        // TAG, Box, Pallet移動時は木枠梱包日セット
        if (cond.UpdateTani == BINKAN_IDO_TANI.TAG_VALUE1 ||
            cond.UpdateTani == BINKAN_IDO_TANI.BOX_VALUE1 ||
            cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
        {
            if (string.IsNullOrEmpty(cond.KojiNoDest) || string.IsNullOrEmpty(cond.CaseIdDest))
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO, DBNull.Value);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.CASE_ID, DBNull.Value);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE, DBNull.Value);
            }
            else
            {
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO, cond.KojiNoDest);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.CASE_ID, cond.CaseIdDest);
                UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE, konpoDt);
            }
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 状態区分をセット
    /// </summary>
    /// <param name="cond">更新条件</param>
    /// <param name="dr">データ行</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void SetJyotaiFlag(CondS01 cond, DataRow dr)
    {

        if (!string.IsNullOrEmpty(cond.CaseIdDest))
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
        else if (!string.IsNullOrEmpty(cond.PalletNo))
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.PALLETZUMI_VALUE1);
        else if (!string.IsNullOrEmpty(cond.BoxNo))
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.BOXZUMI_VALUE1);
        else if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKA_DATE)))
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.SHUKAZUMI_VALUE1);
        else if (!string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAGHAKKO_DATE)))
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1);
        else
            UtilData.SetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG, JYOTAI_FLAG.SHINKI_VALUE1);
    }

    /// --------------------------------------------------
    /// <summary>
    /// 対象工事にCaseID以外のデータがなければ木枠明細作成（０）に更新
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー<</param>
    /// <param name="cond">更新条件</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void UpdKiwakuRemCase(DatabaseHelper dbHelper, CondS01 cond)
    {
        int count = this.GetCountCase(dbHelper, cond.KojiNoOrig, cond.CaseId);

        if (count == 0)
        {
            this.UpdKiwakuSagyoFlag(dbHelper, cond, cond.KojiNoDest, SAGYO_FLAG.KIWAKUMEISAI_VALUE1);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動テーブルロック
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <param name="dtKiwaku">木枠テーブル</param>
    /// <param name="dtKiwMeiRem">パレット削除対象木枠明細テーブル</param>
    /// <param name="dtKiwMeiDest">パレット移動対象木枠明細テーブル</param>
    /// <param name="dtPallet">パレット管理テーブル</param>
    /// <param name="dtBox">ボックス管理テーブル</param>
    /// <param name="dtTag">出荷明細テーブル</param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private void LockTableBinkan(DatabaseHelper dbHelper, CondS01 cond,
        ref DataTable dtKiwaku, ref DataTable dtKiwMeiRem, ref DataTable dtKiwMeiDest,
        ref DataTable dtPallet, ref DataTable dtBox, ref DataTable dtTag)
    {
        try
        {
            // テーブル更新順 木枠、木枠明細、出荷明細、BoxList管理、PalletList管理
            // 木枠明細ロック
            if (cond.UpdateTani == BINKAN_IDO_TANI.KIWAKU_VALUE1)
            {
                dtKiwaku = this.LockKiwaku(dbHelper, cond.KojiNoOrig, cond.KojiNoDest);
                dtKiwMeiDest = this.LockKiwakuMeisai(dbHelper, cond.CaseId);
            }
            else
            {
                // 削除木枠明細ロック
                if (!string.IsNullOrEmpty(cond.CaseIdOrig))
                {
                    // 木枠明細からパレット削除
                    dtKiwMeiRem = this.LockKiwakuMeisai(dbHelper, cond.CaseIdOrig);
                }
                // 更新木枠明細ロック
                if (!string.IsNullOrEmpty(cond.CaseIdDest))
                {
                    // Pallet、Box移動先木枠
                    dtKiwMeiDest = this.LockKiwakuMeisai(dbHelper, cond.CaseIdDest);
                }
            }
            // 出荷明細データロック
            dtTag = this.LockIdoShukkaMeisai(dbHelper, cond);
            // パレット単位時
            if (cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
            {
                // パレット管理データロック
                dtPallet = this.LockPalletListManage(dbHelper, cond.PalletNo);
                // ボックス管理データロック
                DataView dv = dtTag.DefaultView;
                DataTable dt = dv.ToTable(Def_T_BOXLIST_MANAGE.Name, true, Def_T_BOXLIST_MANAGE.BOX_NO);
                dtBox = LockBoxListManage(dbHelper, dt);
            }
            // ボックス単位時
            if (cond.UpdateTani == BINKAN_IDO_TANI.BOX_VALUE1)
            {
                // ボックス管理データロック
                dtBox = this.LockBoxListManage(dbHelper, cond.BoxNo);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region S0100023:TAG登録連携

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/11/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdTehaimeisaiVersionData(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック
            DataTable dtTagRenkeiList = GetAndLockTagRenkeiList(dbHelper, cond, ds, true);
            if ((dtTagRenkeiList == null) || (dtTagRenkeiList.Rows.Count == 0) || (ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows.Count != dtTagRenkeiList.Rows.Count))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(ds.Tables[Def_T_SHUKKA_MEISAI.Name], dtTagRenkeiList, out notFoundIndex, Def_T_SHUKKA_MEISAI.VERSION, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO);
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
            this.UpdTehaimeisaiVersion(dbHelper, cond, ds.Tables[Def_T_SHUKKA_MEISAI.Name]);

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

    #region S0100040:一括アップロード

    #region 手配明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dsSearch">検索対象データ</param>
    /// <returns>手配明細データ</returns>
    /// <create>H.Tajimi 2019/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTehaiMeisaiDataForIkkatsuUpload(DatabaseHelper dbHelper, CondS01 cond, DataSet dsSearch)
    {
        DataSet ds = new DataSet();

        // 手配明細データ取得
        var dt = this.Sql_GetTehaiMeisaiDataForIkkatsuUpload(dbHelper, cond, dsSearch.Tables[ComDefine.DTTBL_RESULT]);
        // ファイル存在有無取得
        var ret = this.Sql_GetExistsPicture(dbHelper, cond, dt);
        ds.Merge(ret);

        return ds;
    }

    #endregion

    #region 図番/型式管理データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式管理データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="dtZumenKeishiki">図番/型式のデータテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tajimi 2019/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsManageZumenKeishiki(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref DataTable dtZumenKeishiki, ref string errMsgID, ref string[] args)
    {
        try
        {
            // サーバー日付
            var serverTime = DateTime.Now;

            // 図番/型式管理データの登録
            var dtIns = ds.Tables[ComDefine.DTTBL_INSERT];
            foreach (DataRow drIns in dtIns.Rows)
            {
                // 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の取得
                var dtTmp = this.Sql_GetManageZumenKeishiki(dbHelper, drIns);
                if (dtZumenKeishiki == null)
                {
                    dtZumenKeishiki = dtTmp.Clone();
                }

                if (UtilData.ExistsData(dtTmp))
                {
                    // 存在した場合は取得したデータをそのまま設定
                    dtZumenKeishiki.Rows.Add(dtTmp.Rows[0].ItemArray);
                }
                else
                {
                    // 存在しない場合は画面側で設定したデータ＋現在サーバー日付より生成した格納ディレクトリを設定後に設定
                    drIns[Def_T_MANAGE_ZUMEN_KEISHIKI.SAVE_DIR] = Path.Combine(serverTime.ToString("yyyy"), serverTime.ToString("MM"));
                    dtZumenKeishiki.Rows.Add(drIns.ItemArray);

                    // 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の登録
                    if (this.InsManageZumenKeishiki(dbHelper, drIns) != 1)
                    {
                        // 保存に失敗しました。
                        errMsgID = "A9999999013";
                        return false;
                    }
                }
            }
            ds.Merge(dtZumenKeishiki);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region S0100050:荷姿表登録

    #region 指定出荷日納入情報一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 指定出荷日納入情報一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNisugataBaseData(DatabaseHelper dbHelper, CondS01 cond)
    {
        var data = GetNisugataFromNonyusaki(dbHelper, cond);
        var dt = data.Tables[Def_T_PACKING_MEISAI.Name];
        foreach (DataRow dr in dt.Rows)
        {
            var nonyusakiCd = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.NONYUSAKI_CD, null);
            var shukkaFlag = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.SHUKKA_FLAG, null);
            var caseNo = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.CASE_NO, null);
            var pallet = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.PALLET_NO, null);
            var box = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.BOX_NO, null);
            // 納入先コードや出荷区分を取得できない場合はスキップする
            if (string.IsNullOrEmpty(nonyusakiCd) || string.IsNullOrEmpty(shukkaFlag))
                continue;
            DataTable dtShukkaMeisai = null;
            if (!string.IsNullOrEmpty(caseNo))
                dtShukkaMeisai = GetShukkaMeisaiSampleForNisugataByCaseNo(dbHelper, nonyusakiCd, shukkaFlag, caseNo);
            if (!string.IsNullOrEmpty(pallet))
                dtShukkaMeisai = GetShukkaMeisaiSampleForNisugataByPallet(dbHelper, nonyusakiCd, shukkaFlag, pallet);
            if (!string.IsNullOrEmpty(box))
                dtShukkaMeisai = GetShukkaMeisaiSampleForNisugataByBox(dbHelper, nonyusakiCd, shukkaFlag, box);
            // 通常あり得ないが、取得できない場合スキップする
            if (dtShukkaMeisai == null || dtShukkaMeisai.Rows.Count == 0)
                continue;
            //AREA(ECS)を設定
            dr.SetField(Def_T_PACKING_MEISAI.ECS_NO, ComFunc.GetFld(dtShukkaMeisai, 0, Def_T_SHUKKA_MEISAI.AREA));

            // ==========================================================================
            //
            // 2022/05/10　追加
            // 手配明細の出荷先を取得し、荷姿表登録画面の宛先に設定する
            //
            // ==========================================================================
            var bukkenno = ComFunc.GetFld(dr, "BUKKEN_NO", null);
            var escno = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.ECS_NO, null);
            DataTable dtTehaiMeisai = null;
            dtTehaiMeisai = GetTehaiMeisaiSampleForNisugataByESCNo(dbHelper, box, pallet, bukkenno);

            string syukka_saki = "";
            syukka_saki += ComFunc.GetFld(dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.SYUKKA_SAKI);
            dr.SetField(Def_T_PACKING_MEISAI.ATTN, string.IsNullOrEmpty(syukka_saki) ? null : syukka_saki);

            // ==========================================================================


            //製番CODEを設定
            string seibanCode = "";
            seibanCode += ComFunc.GetFld(dtShukkaMeisai, 0, Def_T_SHUKKA_MEISAI.SEIBAN);
            seibanCode += ComFunc.GetFld(dtShukkaMeisai, 0, Def_T_SHUKKA_MEISAI.CODE);
            dr.SetField(Def_T_PACKING_MEISAI.SEIBAN_CODE, string.IsNullOrEmpty(seibanCode) ? null : seibanCode);
        }


        return data;
    }

    #endregion

    #region 荷姿/明細 更新
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿/明細 更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元追加</update>
    /// --------------------------------------------------
    public bool UpdNisugata(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 指定出荷日、出荷元のレコードをロック
            DataTable dtLock = this.LockNisugataData(dbHelper, cond.ShipDate, cond.ShipFromCD);

            // Invoice単位で処理
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.TableName == ComDefine.DTTBL_DELETE)
                {   // 削除
                    foreach (DataRow dr in dt.Rows)
                    {
                        // 明細
                        this.DelNisugataMeisaiExec(dbHelper, dr, cond);
                        // 荷姿
                        this.DelNisugataExec(dbHelper, dr, cond);
                    }
                }
                else
                {   // 追加/更新
                    string tgtPNo = ComFunc.GetFld(dt, 0, Def_T_PACKING.PACKING_NO);
                    if (string.IsNullOrEmpty(tgtPNo))
                    {   // 新規
                        // 荷姿CDの採番
                        CondSms condSms = new CondSms(cond.LoginInfo);
                        condSms.SaibanFlag = SAIBAN_FLAG.PACKING_NO_VALUE1;
                        condSms.LoginInfo = cond.LoginInfo;
                        using (WsSmsImpl impl = new WsSmsImpl())
                        {
                            if (!impl.GetSaiban(dbHelper, condSms, out tgtPNo, out errMsgID)) return false;
                        }

                        // 荷姿データの追加
                        this.InsNisugataExec(dbHelper, dt.Rows[0], cond, tgtPNo);
                        // 荷姿明細データの追加
                        foreach (DataRow dr in dt.Rows)
                        {
                            this.InsNisugataMeisaiExec(dbHelper, dr, cond, tgtPNo);
                        }
                    }
                    else
                    {   // 更新
                        DataRow[] s_dr = dtLock.Select(Def_T_PACKING.PACKING_NO + " = '" + tgtPNo + "'");
                        if (!(s_dr != null && s_dr.Length > 0))
                        {
                            // 他端末で更新された為、更新できませんでした。
                            errMsgID = "A9999999027";
                            return false;
                        }
                        DataTable dtCheckSrc = new DataTable(Def_T_PACKING.Name);
                        dtCheckSrc.Columns.Add(Def_T_PACKING.VERSION);
                        DataRow dr_cs = dtCheckSrc.NewRow();
                        dr_cs[Def_T_PACKING.VERSION] = ComFunc.GetFld(s_dr[0], Def_T_PACKING.VERSION);
                        dtCheckSrc.Rows.Add(dr_cs);

                        DataTable dtCheckTgt = new DataTable(Def_T_PACKING.Name);
                        dtCheckTgt.Columns.Add(Def_T_PACKING.VERSION);
                        DataRow dr_cd = dtCheckTgt.NewRow();
                        dr_cd[Def_T_PACKING.VERSION] = ComFunc.GetFld(dt.Rows[0], Def_T_PACKING.VERSION);
                        dtCheckTgt.Rows.Add(dr_cd);

                        int index;
                        int[] notFoundIndex = null;
                        index = this.CheckSameData(dtCheckTgt, dtCheckSrc, out notFoundIndex, Def_T_PACKING.VERSION);
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

                        // 明細削除
                        foreach (DataRow dr in dt.Rows)
                        {
                            this.DelNisugataMeisaiExec(dbHelper, dr, cond);
                        }
                        // 荷姿更新
                        this.UpdNisugataExec(dbHelper, dt.Rows[0], cond);
                        // 明細追加
                        foreach (DataRow dr in dt.Rows)
                        {
                            this.InsNisugataMeisaiExec(dbHelper, dr, cond, tgtPNo);
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
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 荷姿/明細 削除
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿/明細 削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">更新データのDataSet</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元追加</update>
    /// --------------------------------------------------
    public bool DelNisugata(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 指定出荷日、出荷元のレコードをロック
            DataTable dtLock = this.LockNisugataData(dbHelper, cond.ShipDate, cond.ShipFromCD);
            DataTable dtDel = ds.Tables[Def_T_PACKING_MEISAI.Name];

            // バージョンチェック
            int index;
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtDel, dtLock, out notFoundIndex, Def_T_PACKING.VERSION, Def_T_PACKING.PACKING_NO);
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

            // 削除
            dtDel = ds.Tables[ComDefine.DTTBL_DELETE];
            foreach (DataRow dr in dtDel.Rows)
            {
                this.DelNisugataMeisaiExec(dbHelper, dr, cond);
                this.DelNisugataExec(dbHelper, dr, cond);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 荷姿更新(Excel用)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿更新(Excel用)
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="ds"></param>
    /// <param name="errMsgID"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// --------------------------------------------------
    public bool UpdPackingForExcelData(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            var dtPacking = ds.Tables[Def_T_PACKING.Name];
            var dtMail = ds.Tables[Def_T_MAIL.Name];

            // 荷姿表の行ロック
            var dtLockPacking = this.LockPackingOnPackingNo(dbHelper, dtPacking);
            if (!UtilData.ExistsData(dtLockPacking))
            {
                // 行ロック失敗
                return false;
            }

            // 荷姿の更新
            var cntUpd = this.UpdPackingForExcelData(dbHelper, cond, dtLockPacking);
            if (cntUpd != dtLockPacking.Rows.Count)
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

    #endregion

    #endregion

    #region SQL実行

    #region S0100010:出荷計画登録

    #region SELECT

    #region 物件マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 指定物件名称の物件名情報を取得(完全一致・物件名称必須)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷フラグ</param>
    /// <param name="bukkenName">物件名</param>
    /// <returns>物件マスタ</returns>
    /// <create>H.Tajimi 2018/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBukken(DatabaseHelper dbHelper, string shukkaFlag, string bukkenName)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", bukkenName));

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

    #region 納入先マスタ取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷フラグ</param>
    /// <param name="bukkenName">物件名</param>
    /// <param name="ship">便</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/31</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetNonyusaki(DatabaseHelper dbHelper, string shukkaFlag, string bukkenName, string ship)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("     , M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN");
            sb.ApdL(" INNER JOIN M_NONYUSAKI");
            sb.ApdL("         ON M_NONYUSAKI.SHUKKA_FLAG = M_BUKKEN.SHUKKA_FLAG");
            sb.ApdL("        AND M_NONYUSAKI.BUKKEN_NO = M_BUKKEN.BUKKEN_NO");
            sb.ApdN("        AND M_NONYUSAKI.SHIP = ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdL(" WHERE");
            sb.ApdN("       M_BUKKEN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND M_BUKKEN.BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", bukkenName));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", ship));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_M_NONYUSAKI.Name;

            return dt;

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
    /// <param name="shukkaFlag">出荷フラグ</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool ExistsShukkaMeisai(DatabaseHelper dbHelper, string shukkaFlag, string nonyusakiCd)
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
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL(" WHERE");
            sb.ApdN("       SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND SM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 荷姿存在確認

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿存在確認
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷フラグ</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool ExistsPacking(DatabaseHelper dbHelper, string shukkaFlag, string nonyusakiCd)
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
            sb.ApdL("       T_PACKING_MEISAI TPM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND TPM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

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
    internal DataTable Sql_GetShipFrom(DatabaseHelper dbHelper, CondS01 cond)
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
            sb.ApdL("  FROM");
            sb.ApdL("       M_SHIP_FROM");
            sb.ApdL(" WHERE");
            sb.ApdN("       M_SHIP_FROM.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       M_SHIP_FROM.DISP_NO");
            sb.ApdL("     , M_SHIP_FROM.SHIP_FROM_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UNUSED_FLAG", cond.UnusedFlag));

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

    #region 出荷明細データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="bukkenNo">物件番号</param>
    /// <returns>納入先データ</returns>
    /// <create>T.SASAYAMA 2023/08/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockNonyusaki(DatabaseHelper dbHelper, string shukkaFlag, string bukkenNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , NONYUSAKI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       M_NONYUSAKI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       NONYUSAKI_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", bukkenNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_SHUKKA_MEISAI.Name;

            return dt;

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
    /// 物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>R,Miyoshi 2023/07/14</create>
    /// <update>J.Chen 2023/09/05</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBukkenNameListForTorikomi(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT BUKKEN_NO");
            sb.ApdL("      ,BUKKEN_NAME");
            sb.ApdL("FROM M_BUKKEN");
            sb.ApdL("WHERE SHUKKA_FLAG = '0'");
            sb.ApdL("ORDER BY BUKKEN_NAME");
            sb.ApdL("        ,BUKKEN_NO");

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

    #region 荷受先名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷受先名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>R.Miyoshi 2023/07/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetConsignList(DatabaseHelper dbHelper)
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
            sb.ApdL("  FROM");
            sb.ApdL("       M_CONSIGN MC");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       MC.SORT_NO");

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

    #region 出荷元一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 出荷元一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>T.SASAYAMA 2023/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkamoto(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT SHIP_FROM_NAME");
            sb.ApdL("      ,DISP_NO");
            sb.ApdL("FROM M_SHIP_FROM");
            sb.ApdL("ORDER BY DISP_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SHIP_FROM.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 出荷先一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 出荷先一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>T.SASAYAMA 2023/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkasaki(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ITEM_NAME");
            sb.ApdL("FROM M_SELECT_ITEM");
            sb.ApdL("WHERE SELECT_GROUP_CD = 'TS'");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);

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
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.SASAYAMA 2023/08/07</create>
    /// <update>J.Chen 2023/08/28 多言語対応</update>
    /// <update>J.Chen 2024/01/16 検索条件はLIKEではなく、完全一致に変更</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetNonyusaki(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 物件no取得
            var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            var dtBukken = this.GetBukken(dbHelper, shukkaFlag, cond.BukkenName);
            var bukkenNo = ComFunc.GetFld(dtBukken, 0, Def_M_BUKKEN.BUKKEN_NO);
            dtBukken.TableName = Def_M_BUKKEN.Name;
            ds.Tables.Add(dtBukken);

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       NONYUSAKI_NAME AS BUKKEN_NAME ");
            sb.ApdL("     , CASE ");
            sb.ApdL("           WHEN SYORI_FLAG IS NULL"); //CASE文を開始
            sb.ApdL("           OR SYORI_FLAG = '1' THEN '0'");
            sb.ApdL("           ELSE SYORI_FLAG ");
            sb.ApdL("       END AS EXCEL_SHORI_FLAG");
            sb.ApdL("     , SHIP ");
            sb.ApdL("     , ESTIMATE_FLAG");
            sb.ApdL("     , CASE ESTIMATE_FLAG "); // CASE文を開始
            if (cond.LoginInfo.Language == "US")
            {
                sb.ApdL("           WHEN 0 THEN 'Non-commercial' "); // ESTIMATE_FLAGが0の時は'無償'
                sb.ApdL("           WHEN 1 THEN 'Commercial' "); // ESTIMATE_FLAGが1の時は'有償'
            }
            else
            {
                sb.ApdL("           WHEN 0 THEN '無償' "); // ESTIMATE_FLAGが0の時は'無償'
                sb.ApdL("           WHEN 1 THEN '有償' "); // ESTIMATE_FLAGが1の時は'有償'
            }
            sb.ApdL("       END AS ESTIMATE_FLAG_NAME "); // 結果をESTIMATE_FLAG_NAMEという列名で取得
            sb.ApdL("     , TRANSPORT_FLAG");
            sb.ApdL("     , SHIP_FROM");
            sb.ApdL("     , SHIP_TO");
            sb.ApdL("     , SHIP_DATE");
            sb.ApdL("     , SHIP_NO");
            sb.ApdL("     , SHIP_SEIBAN");
            sb.ApdL("     , SHIP_FROM_CD");
            sb.ApdL("     , SEIBAN");
            sb.ApdL("     , KISHU");
            sb.ApdL("     , NAIYO");
            sb.ApdL("     , TOUCHAKUYOTEI_DATE");
            sb.ApdL("     , KIKAI_PARTS");
            sb.ApdL("     , SEIGYO_PARTS");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , CONSIGN_CD");
            sb.ApdL("     , LAST_SYORI_FLAG");
            sb.ApdL("FROM ");
            sb.ApdL("       M_NONYUSAKI");
            sb.ApdL("WHERE 1 = 1");
            if (!string.IsNullOrEmpty(cond.BukkenName))
            {
                sb.ApdN("   AND NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL(Def_M_BUKKEN.BUKKEN_NAME);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_BUKKEN.BUKKEN_NAME, cond.BukkenName));
            }
            //if (!string.IsNullOrEmpty(cond.ShipSeiban))
            //{
            //    sb.ApdN("   AND SHIP_SEIBAN = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.SHIP_SEIBAN);
            //    paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.SHIP_SEIBAN, cond.ShipSeiban));
            //}
            if (!string.IsNullOrEmpty(cond.ConsignCD))
            {
                sb.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.CONSIGN_CD);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.CONSIGN_CD, cond.ConsignCD));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHIP_DATE");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);

            // SQL文 2回目
            StringBuilder sb2 = new StringBuilder();
            DbParamCollection paramCollection2 = new DbParamCollection();

            sb2.ApdL("SELECT ");
            sb2.ApdL("       TOP 1 ");
            sb2.ApdL("       BUKKEN_REV ");
            sb2.ApdL("     , ASSIGN_COMMENT ");
            sb2.ApdL("     , VERSION ");
            sb2.ApdL("FROM ");
            sb2.ApdL("       M_MAIL_SEND_RIREKI");
            sb2.ApdL("WHERE 1 = 1");
            if (!string.IsNullOrEmpty(bukkenNo))
            {
                sb2.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL(Def_M_BUKKEN.BUKKEN_NO);
                paramCollection2.Add(iNewParam.NewDbParameter(Def_M_BUKKEN.BUKKEN_NO, bukkenNo));
            }
            //if (!string.IsNullOrEmpty(cond.ShipSeiban))
            //{
            //    sb2.ApdN("   AND SHIP_SEIBAN = ").ApdN(this.BindPrefix).ApdL(Def_M_MAIL_SEND_RIREKI.SHIP_SEIBAN);
            //    paramCollection2.Add(iNewParam.NewDbParameter(Def_M_MAIL_SEND_RIREKI.SHIP_SEIBAN, cond.ShipSeiban));
            //}
            if (!string.IsNullOrEmpty(cond.ConsignCD))
            {
                sb2.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL(Def_M_MAIL_SEND_RIREKI.CONSIGN_CD);
                paramCollection2.Add(iNewParam.NewDbParameter(Def_M_MAIL_SEND_RIREKI.CONSIGN_CD, cond.ConsignCD));
            }
            sb2.ApdL(" ORDER BY");
            sb2.ApdL("       UPDATE_DATE");
            sb2.ApdL("       DESC");

            dbHelper.Fill(sb2.ToString(), paramCollection2, ds, Def_M_MAIL_SEND_RIREKI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 排他取得

    /// --------------------------------------------------
    /// <summary>
    /// 排他取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetHaitaData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("       HAITA_USER_ID");
            sb.ApdL("     , HAITA_USER_NAME");
            sb.ApdL("     , HAITA_DATE");
            sb.ApdL("  FROM");
            sb.ApdL("       M_NONYUSAKI");
            sb.ApdL(" WHERE 1 = 1");
            sb.ApdN("   AND NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdL("   AND HAITA_DATE IS NOT NULL ");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       HAITA_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_M_NONYUSAKI.Name;

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

    #region リビジョン登録
    /// --------------------------------------------------
    /// <summary>
    /// リビジョン登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>bool</returns>
    /// <create>T.SASAYAMA 2023/08/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsertRevision(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;
            cond.AssignComment = cond.AssignComment.Replace("\n", "\r\n");

            // SQL文
            sb.ApdL("MERGE INTO M_MAIL_SEND_RIREKI AS target");
            sb.ApdL("USING (VALUES (").ApdN(this.BindPrefix).ApdL("BUKKEN_NO, ")
               .ApdN(this.BindPrefix).ApdL("CONSIGN_CD, ")
               .ApdN(this.BindPrefix).ApdL("SHIP_SEIBAN, ")
               .ApdN(this.BindPrefix).ApdL("BUKKEN_REV)) AS source (BUKKEN_NO, CONSIGN_CD, SHIP_SEIBAN, BUKKEN_REV)");
            sb.ApdL("ON target.BUKKEN_NO = source.BUKKEN_NO AND target.CONSIGN_CD = source.CONSIGN_CD AND target.SHIP_SEIBAN = source.SHIP_SEIBAN AND target.BUKKEN_REV = source.BUKKEN_REV");
            sb.ApdL("WHEN MATCHED THEN")
               .ApdL("    UPDATE SET ASSIGN_COMMENT = ").ApdN(this.BindPrefix).ApdL("ASSIGN_COMMENT,")
               .ApdL("               UPDATE_DATE = ").ApdL(this.SysTimestamp).ApdL(",")
               .ApdL("               VERSION = ").ApdL(this.SysTimestamp).ApdL(",")
               .ApdL("               UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID,")
               .ApdL("               UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdL("WHEN NOT MATCHED THEN")
               .ApdL("    INSERT (BUKKEN_NO, CONSIGN_CD, SHIP_SEIBAN, BUKKEN_REV, ASSIGN_COMMENT, CREATE_DATE, CREATE_USER_ID, CREATE_USER_NAME, UPDATE_DATE, UPDATE_USER_ID, UPDATE_USER_NAME, VERSION)")
               .ApdL("    VALUES (").ApdN(this.BindPrefix).ApdL("BUKKEN_NO, ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD, ")
               .ApdN(this.BindPrefix).ApdL("SHIP_SEIBAN, ").ApdN(this.BindPrefix).ApdL("BUKKEN_REV, ").ApdN(this.BindPrefix).ApdL("ASSIGN_COMMENT, ")
               .ApdL(this.SysTimestamp).ApdL(", ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID, ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME, ")
               .ApdL(this.SysTimestamp).ApdL(", ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID, ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME, ")
               .ApdL(this.SysTimestamp).ApdL(");");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            paramCollection.Add(iNewParameter.NewDbParameter("CONSIGN_CD", cond.ConsignCD));
            paramCollection.Add(iNewParameter.NewDbParameter("SHIP_SEIBAN", cond.ShipSeiban));
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_REV", cond.BukkenRev));
            if (cond.AssignComment == null)
            {
                cond.AssignComment = string.Empty;
            }
            paramCollection.Add(iNewParameter.NewDbParameter("ASSIGN_COMMENT", cond.AssignComment));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            // SQL実行
            dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region UPDATE

    #region 排他登録
    /// <summary>
    /// 排他登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update></update>
    public int UpdateHaita(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            int resultCnt = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI");
            sb.ApdL("SET");
            sb.ApdN("       HAITA_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , HAITA_USER_ID = ").ApdN(this.BindPrefix).ApdL("HAITA_USER_ID");
            sb.ApdN("     , HAITA_USER_NAME = ").ApdN(this.BindPrefix).ApdL("HAITA_USER_NAME");
            sb.ApdL(" WHERE 1 = 1 ");
            sb.ApdN("   AND NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            sb.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HAITA_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("HAITA_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return resultCnt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 排他クリア
    /// <summary>
    /// 排他クリア
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>J.Chen 2024/01/17</create>
    /// <update>J.Chen 2024/08/07 クリア対象は案件→USERに変更</update>
    /// <update></update>
    public int UpdateNullHaita(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            int resultCnt = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_NONYUSAKI");
            sb.ApdL("SET");
            sb.ApdL("       HAITA_DATE = NULL");
            sb.ApdL("     , HAITA_USER_ID = NULL");
            sb.ApdL("     , HAITA_USER_NAME = NULL");
            sb.ApdL(" WHERE 1 = 1 ");
            //sb.ApdN("   AND NONYUSAKI_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");
            //sb.ApdN("   AND CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("   AND HAITA_USER_ID = ").ApdN(this.BindPrefix).ApdL("HAITA_USER_ID");

            // バインド変数設定
            //paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.BukkenName));
            //paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));
            paramCollection.Add(iNewParam.NewDbParameter("HAITA_USER_ID", this.GetUpdateUserID(cond)));

            resultCnt += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return resultCnt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region DELETE

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷フラグ</param>
    /// <param name="bukkenName">物件名</param>
    /// <param name="ship">便</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/11/01</create>
    /// <update>H.Tajimi 2020/05/26 対象テーブルが誤っていたためSQLエラーとなる問題を修正</update>
    /// --------------------------------------------------
    public int DelNonyusaki(DatabaseHelper dbHelper, string shukkaFlag, string bukkenNo, string ship)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE M_NONYUSAKI");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI SM");
            sb.ApdL("                WHERE");
            sb.ApdN("                      SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND SM.SHUKKA_FLAG = M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("                  AND SM.NONYUSAKI_CD = M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("                  )");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_PACKING_MEISAI TPM");
            sb.ApdL("                WHERE");
            sb.ApdN("                      TPM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("                  AND TPM.SHUKKA_FLAG = M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("                  AND TPM.NONYUSAKI_CD = M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("                  )");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND SHIP = ").ApdN(this.BindPrefix).ApdL("SHIP");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", bukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", ship));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            return record;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region 納入先行データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 納入先行データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="nonyusakiCd">nonyusakiCd.</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.SASAYAMA 2023/08/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelNonyusakiRow(DatabaseHelper dbHelper, CondS01 cond, string nonyusakiCd)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM M_NONYUSAKI");
            sb.ApdL(" WHERE ");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("nonyusakiCd");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("nonyusakiCd", nonyusakiCd));

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

    #region S0100020:出荷計画明細登録

    #region SELECT

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update>Y.Higuchi 2010/10/28</update>
    /// <update>Y.Higuchi 2010/11/25</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>T.Nakata 2018/11/13 手配業務対応</update>
    /// <update>H.Tajimi 2019/08/09 写真管理方式変更</update>
    /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
    /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
    /// <update>J.Chen 2022/12/19 TAG便名追加</update>
    /// <update>J.Chen 2024/11/08 通関確認状態追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetShukkaMeisai(DatabaseHelper dbHelper, CondS01 cond)
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
            sb.ApdL("     , COM.ITEM_NAME AS JYOTAI_NAME");
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
            sb.ApdL("     , TSM.ZUMEN_KEISHIKI2");
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
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            sb.ApdL("     , RTRIM(TK.SHIP) + '-' + CAST(TKM.CASE_NO AS VARCHAR) AS KIWAKU_NO");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , CASE");
            sb.ApdL("        WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = TSM.ZUMEN_KEISHIKI");
            sb.ApdN("        ) THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdL("        WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            INNER JOIN T_TEHAI_MEISAI TTM");
            sb.ApdL("                    ON TTM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = TTM.ZUMEN_KEISHIKI2");
            sb.ApdN("        ) THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdL("NOT_EXISTS_VALUE");
            sb.ApdL("        END AS EXISTS_PICTURE");
            sb.ApdL("     , TKM.PRINT_CASE_NO");

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
            //　内部結合されている納入先マスタ（M_NONYUSAKI)　に出荷元（M_SHIP_FROM）を更に外部結合
            //　　出荷元CD、名称を取得
            //　　　SHIP_FROM_NO    出荷目CD（表示しないが項目抽出） 
            //　　　SHIP_FROM_NAME  名称　  （出荷元として表示する）
            sb.ApdL("     , MSF.SHIP_FROM_NO");
            sb.ApdL("     , MSF.SHIP_FROM_NAME AS SHIP_FROM_NAME");

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

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
            //　内部結合されている納入先マスタ（M_NONYUSAKI)　に出荷元（M_SHIP_FROM）を更に外部結合
            //　　出荷元CD、名称を取得
            //　　　SHIP_FROM_NO    出荷目CD（表示しないが項目抽出） 
            //　　　SHIP_FROM_NAME  名称　  （出荷元として表示する）
            sb.ApdL("   LEFT JOIN M_SHIP_FROM MSF ON MSF.SHIP_FROM_NO = MNS.SHIP_FROM_CD");
            sb.ApdL("                            AND MSF.UNUSED_FLAG = ").ApdN(this.BindPrefix).ApdL("UNUSED_FLAG");


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
            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
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
                            value += ", " + UtilConvert.PutQuot(DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1);
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
                    fieldName = "JYOTAI_FLAG";
                    sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(expression).ApdL(value);
                }
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("NOT_EXISTS_VALUE", ComDefine.NOT_EXISTS_PICTURE_VALUE));

            // 出荷元列追加による 2022/10/11（TW-Tsuji）
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

    #region TagNo.チェック用データ取得(納入先名でのデータ取得)

    /// --------------------------------------------------
    /// <summary>
    /// TagNo.チェック用データ取得(納入先名でのデータ取得)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>チェックデータ</returns>
    /// <create>Y.Higuchi 2010/10/28</create>
    /// <update>K.Tsutsumi 2012/04/24</update>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/09/10 多言語化対応、LANG条件を追加</update>
    /// --------------------------------------------------
    public DataSet GetShukkaMeisaiTagNoCheck(DatabaseHelper dbHelper, CondS01 cond)
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
            sb.ApdL("     , MNS.NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS JYOTAI_NAME");
            sb.ApdL("     , RTRIM(TSM.TAG_NO) AS TAG_NO");
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
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TSM.BIKO");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'DISP_JYOTAI_FLAG'");
            sb.ApdL("                        AND COM.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                               AND TKM.CASE_ID = TSM.CASE_ID");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            // 出荷フラグ
            if (cond.ShukkaFlag != null)
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }
            // 2012/04/24 K.Tsutsumi Change キーでは無くなった
            //// 納入先名
            //if (cond.NonyusakiName != null)
            //{
            //    fieldName = "NONYUSAKI_NAME";
            //    sb.ApdN("   AND ").ApdN("MNS." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
            //    paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiName));
            //}

            // 納入先コード
            if (cond.NonyusakiCD != null)
            {
                fieldName = "NONYUSAKI_CD";
                sb.ApdN("   AND ").ApdN("MNS." + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.NonyusakiCD));
            }
            // ↑
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

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

    #region 出荷明細データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="nonyusakiCD">納入先コード</param>
    /// <returns>出荷明細データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockShukkaMeisai(DatabaseHelper dbHelper, string shukkaFlag, string nonyusakiCD, string arNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", arNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_SHUKKA_MEISAI.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Box梱包以降のデータ数

    /// --------------------------------------------------
    /// <summary>
    /// Box梱包以降のデータ数
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/07/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int GetShukkaMeisaiOverBoxKonpoDataCount(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "TSM.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            // AR No.
            if (cond.ARNo != null)
            {
                fieldName = "AR_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ARNo));
            }
            // 状態区分
            string value = string.Empty;
            value += "(";
            value += UtilConvert.PutQuot(JYOTAI_FLAG.SHINKI_VALUE1);
            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1);
            value += ", " + UtilConvert.PutQuot(DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1);
            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.SHUKAZUMI_VALUE1);
            value += ")";
            sb.ApdN("   AND TSM.JYOTAI_FLAG NOT IN ").ApdL(value);

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
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

    #region 物件名マスタ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="bukkenNO">物件管理No</param>
    /// <returns>物件名マスタ</returns>
    /// <create>K.Tsutsumi 2012/04/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockBukken(DatabaseHelper dbHelper, string shukkaFlag, string bukkenNO)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , ISSUED_TAG_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       M_BUKKEN");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", shukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", bukkenNO));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_M_BUKKEN.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 出荷数チェック用

    /// --------------------------------------------------
    /// <summary>
    /// 出荷数チェック用
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>表示データ</returns>
    /// <create>T.Nakata 2018/11/26</create>
    /// <update>K.Tsutsumi 2019/02/08 出荷数チェックで元にしているデータが２倍になる不具合修正 </update>
    /// --------------------------------------------------
    private DataTable CheckShukkaQtyNum(DatabaseHelper dbHelper, DataRow dr, CondS01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("	 TM.TEHAI_RENKEI_NO");
            sb.ApdL("	,TM.SHUKKA_QTY");
            sb.ApdL("	,S_MEISAI.NUM AS NUM");
            sb.ApdL("FROM");
            sb.ApdL("T_TEHAI_MEISAI TM");
            // m:手配明細、s:出荷明細数量合計
            sb.ApdL("INNER JOIN (");
            sb.ApdL("	SELECT m.*");
            sb.ApdL("			, IsNull(s.NUM,0) AS NUM");
            sb.ApdL("	FROM T_TEHAI_MEISAI AS m ");
            // ss:出荷明細、tt:手配明細、ee:技連マスタ、bb:物件名マスタ
            sb.ApdL("	LEFT OUTER JOIN (");
            sb.ApdL("		SELECT ss.TEHAI_RENKEI_NO, SUM(NUM) AS NUM");
            sb.ApdL("			FROM T_SHUKKA_MEISAI ss");
            sb.ApdL("			LEFT JOIN T_TEHAI_MEISAI tt ON tt.TEHAI_RENKEI_NO = ss.TEHAI_RENKEI_NO");
            sb.ApdL("			LEFT JOIN M_ECS ee ON ee.ECS_QUOTA = tt.ECS_QUOTA AND ee.ECS_NO = tt.ECS_NO");
            sb.ApdL("			LEFT JOIN M_BUKKEN bb ON bb.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG").ApdN(" AND bb.PROJECT_NO = ee.PROJECT_NO");
            sb.ApdL("			WHERE ss.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdN("NONYUSAKI_CD");
            sb.ApdL("				  AND ss.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG");
            sb.ApdL("				  AND ss.TEHAI_RENKEI_NO =").ApdN(this.BindPrefix).ApdN("TEHAI_RENKEI_NO");
            sb.ApdL("			GROUP BY ss.TEHAI_RENKEI_NO");
            sb.ApdL("	) AS s ON s.TEHAI_RENKEI_NO = m.TEHAI_RENKEI_NO");
            sb.ApdL("	WHERE IsNull(s.NUM,0) > 0");
            sb.ApdL(") AS S_MEISAI ON TM.TEHAI_RENKEI_NO = S_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("WHERE");
            sb.ApdL("	TM.TEHAI_RENKEI_NO =").ApdN(this.BindPrefix).ApdN("TEHAI_RENKEI_NO");
            sb.ApdL("	AND S_MEISAI.NUM > TM.SHUKKA_QTY");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, DBNull.Value)));

            // SQL実行
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

    #region 写真有無取得

    /// --------------------------------------------------
    /// <summary>
    /// 写真有無取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetExistsPicture(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_MANAGE_ZUMEN_KEISHIKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       CASE");
            sb.ApdL("        WHEN EXISTS (");
            sb.ApdL("           SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdN("            WHERE MZK.ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("        ) THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdL("NOT_EXISTS_VALUE");
            sb.ApdL("        END AS EXISTS_PICTURE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", cond.ZumenKeishiki));
            paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
            paramCollection.Add(iNewParam.NewDbParameter("NOT_EXISTS_VALUE", ComDefine.NOT_EXISTS_PICTURE_VALUE));

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

    #region INSERT

    #region 出荷明細データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>T.Nakata 2018/11/20 手配連携Noおよび重量列追加</update>
    /// <update>D.Okumura 2019/01/29 AR-NO複数行入力不具合対応</update>
    /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
    /// <update>J.Chen 2024/11/25 通関確認状態追加対応</update>
    /// <update></update>
    /// --------------------------------------------------
    private int InsShukkaMeisaiExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_SHUKKA_MEISAI ");
            sb.ApdL("(");
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
            sb.ApdL("     , ZUMEN_KEISHIKI2");
            sb.ApdL("     , KUWARI_NO");
            sb.ApdL("     , NUM");
            sb.ApdL("     , JYOTAI_FLAG");
            sb.ApdL("     , TAGHAKKO_FLAG");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , FREE1");
            sb.ApdL("     , FREE2");
            sb.ApdL("     , TAG_NONYUSAKI_CD");
            sb.ApdL("     , BIKO");
            sb.ApdL("     , CUSTOMS_STATUS");
            sb.ApdL("     , M_NO");
            sb.ApdL("     , GRWT");
            sb.ApdL("     , TEHAI_RENKEI_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AREA");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FLOOR");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ST_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KUWARI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NUM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAGHAKKO_FLAG");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FREE1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BIKO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CUSTOMS_STATUS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("M_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GRWT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO, DBNull.Value)));

            // ↓2019/01/29 D.Okumura Change 不具合対応
            //string arNo = string.Empty;
            //if (cond.ARNo != null)
            //{
            //    arNo = cond.ARNo;
            //}
            //paramCollection.Add(iNewParam.NewDbParameter("AR_NO", arNo));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.AR_NO)));
            // ↑2019/01/29 D.Okumura Change 不具合対応
            paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SEIBAN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CODE", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.CODE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_OIBAN", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("AREA", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.AREA, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("FLOOR", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FLOOR, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KISHU, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ST_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ST_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.HINMEI_JP, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.HINMEI, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI2", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("KUWARI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KUWARI_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("NUM", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NUM, 0)));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHINKI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TAGHAKKO_FLAG", TAGHAKKO_FLAG.MIHAKKO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("FREE1", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FREE1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FREE2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("BIKO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BIKO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CUSTOMS_STATUS", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.CUSTOMS_STATUS, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("M_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.M_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GRWT", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.GRWT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, DBNull.Value)));

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

    #region 履歴データ追加
    /// --------------------------------------------------
    /// <summary>
    /// 履歴データの追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Tsutsumi 2012/05/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsRireki(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_RIREKI");
            sb.ApdL("(");
            sb.ApdL("       GAMEN_FLAG");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , OPERATION_FLAG");
            sb.ApdL("     , UPDATE_PC_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("GAMEN_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("OPERATION_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_PC_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("GAMEN_FLAG", GAMEN_FLAG.S0200020_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            if (cond.Ship == null)
            {
                cond.Ship = string.Empty;
            }
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", cond.Ship));
            string arNo = string.Empty;
            if (cond.ARNo != null)
            {
                arNo = cond.ARNo;
            }
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", arNo));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            paramCollection.Add(iNewParam.NewDbParameter("OPERATION_FLAG", cond.OperationFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_PC_NAME", cond.UpdatePCName));
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

    #region UPDATE

    #region AR情報データの更新

    /// --------------------------------------------------
    /// <summary>
    /// AR情報データの更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/13</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdAR(DatabaseHelper dbHelper, CondS01 cond)
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
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND LIST_FLAG = ").ApdN(this.BindPrefix).ApdL("LIST_FLAG");
            sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOKYO_FLAG", "5"));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG", ComFunc.GetListFlag(cond.ARNo)));
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

    #region 出荷明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>T.Nakata 2018/11/14 手配業務対応</update>
    /// <update>H.Iimuro 2022/10/04 図面/型式２の追加対応</update>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdShukkaMeisai(DatabaseHelper dbHelper, CondS01 cond, DataTable dt)
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
            sb.ApdN("       SEIBAN = ").ApdN(this.BindPrefix).ApdL("SEIBAN");
            sb.ApdN("     , CODE = ").ApdN(this.BindPrefix).ApdL("CODE ");
            sb.ApdN("     , ZUMEN_OIBAN = ").ApdN(this.BindPrefix).ApdL("ZUMEN_OIBAN");
            sb.ApdN("     , AREA = ").ApdN(this.BindPrefix).ApdL("AREA");
            sb.ApdN("     , FLOOR = ").ApdN(this.BindPrefix).ApdL("FLOOR");
            sb.ApdN("     , KISHU = ").ApdN(this.BindPrefix).ApdL("KISHU");
            sb.ApdN("     , ST_NO = ").ApdN(this.BindPrefix).ApdL("ST_NO");
            sb.ApdN("     , HINMEI_JP = ").ApdN(this.BindPrefix).ApdL("HINMEI_JP");
            sb.ApdN("     , HINMEI = ").ApdN(this.BindPrefix).ApdL("HINMEI");
            sb.ApdN("     , ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("     , ZUMEN_KEISHIKI2 = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI2");
            sb.ApdN("     , KUWARI_NO = ").ApdN(this.BindPrefix).ApdL("KUWARI_NO");
            sb.ApdN("     , NUM = ").ApdN(this.BindPrefix).ApdL("NUM");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , FREE1 = ").ApdN(this.BindPrefix).ApdL("FREE1");
            sb.ApdN("     , FREE2 = ").ApdN(this.BindPrefix).ApdL("FREE2");
            sb.ApdN("     , BIKO = ").ApdN(this.BindPrefix).ApdL("BIKO");
            sb.ApdN("     , M_NO = ").ApdN(this.BindPrefix).ApdL("M_NO");
            sb.ApdN("     , GRWT = ").ApdN(this.BindPrefix).ApdL("GRWT");
            sb.ApdN("     , TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
            }

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SEIBAN", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.SEIBAN, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("CODE", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.CODE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_OIBAN", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_OIBAN, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("AREA", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.AREA, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("FLOOR", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FLOOR, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KISHU", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KISHU, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ST_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ST_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("HINMEI_JP", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.HINMEI_JP, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("HINMEI", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.HINMEI, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI2", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.ZUMEN_KEISHIKI2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KUWARI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KUWARI_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("NUM", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.NUM, 0)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("FREE1", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FREE1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("FREE2", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.FREE2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("BIKO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BIKO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("M_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.M_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GRWT", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.GRWT, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO, DBNull.Value)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TAG_NO, DBNull.Value)));
                if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
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

    #region 物件名マスタ更新
    /// --------------------------------------------------
    /// <summary>
    /// 物件名マスタ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新対象物件名マスタ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>K.Tsutsumi 2012/04/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBukken(DatabaseHelper dbHelper, DataTable dt, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE M_BUKKEN");
            sb.ApdL("   SET");
            sb.ApdN("       ISSUED_TAG_NO = ").ApdN(this.BindPrefix).ApdL("ISSUED_TAG_NO");
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
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_M_BUKKEN.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFldToDecimal(dr, Def_M_BUKKEN.BUKKEN_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("ISSUED_TAG_NO", ComFunc.GetFldToDecimal(dr, Def_M_BUKKEN.ISSUED_TAG_NO)));
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

    #region 木枠明細更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="description1">DESCRIPTION_1の内容</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2015/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwakuMeisai(DatabaseHelper dbHelper, CondK02 cond, string description1)
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
            sb.ApdN("       DESCRIPTION_1 = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_1");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_1", description1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));

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

    #region 出荷明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="tagNo">TagNo.</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelShukkaMeisai(DatabaseHelper dbHelper, CondS01 cond, string tagNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            // 2011/03/08 K.Tsutsumi Add 条件抜け
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND AR_NO = ").ApdN(this.BindPrefix).ApdL("AR_NO");
                paramCollection.Add(iNewParam.NewDbParameter("AR_NO", cond.ARNo));
            }
            if (!string.IsNullOrEmpty(tagNo))
            {
                sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", tagNo));
            }

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

    #endregion

    #region S0100030:便間移動

    #region SELECT

    #region ツリー明細取得

    /// --------------------------------------------------
    /// <summary>
    /// ツリー明細取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>ツリー明細データ</returns>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2016/01/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTreeMeisai(DatabaseHelper dbHelper, CondS01 cond)
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
            sb.ApdL("       TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TSM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TKM.PRINT_CASE_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN T_KIWAKU TK ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.CASE_ID = TSM.CASE_ID");
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
            // 表示選択
            string expression = string.Empty;
            string value = string.Empty;
            expression = " IN ";
            // 出荷済
            value += "(";
            value += UtilConvert.PutQuot(JYOTAI_FLAG.SHUKAZUMI_VALUE1);
            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.BOXZUMI_VALUE1);
            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.PALLETZUMI_VALUE1);
            value += ", " + UtilConvert.PutQuot(JYOTAI_FLAG.KIWAKUKONPO_VALUE1);
            value += ")";
            fieldName = "JYOTAI_FLAG";
            sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(expression).ApdL(value);

            sb.ApdL(" ORDER BY");
            sb.ApdL("       TK.SHIP");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.TAG_NO");

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

    #region 移動先木枠取得

    /// --------------------------------------------------
    /// <summary>
    /// 移動先木枠取得
    /// </summary>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>T.Wakamatsu 2016/03/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKiwaku(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT KOJI_NO");
            sb.ApdL("     , KOJI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU T1");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS(");
            sb.ApdL("        SELECT 1");
            sb.ApdL("          FROM");
            sb.ApdL("               T_SHUKKA_MEISAI T2");
            sb.ApdL("         WHERE");
            sb.ApdL("               T1.KOJI_NO = T2.KOJI_NO");
            sb.ApdN("           AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("           AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("       )");

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

    #region 便間移動出荷明細データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動出荷明細データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">取得条件</param>
    /// <returns>出荷明細データ</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockIdoShukkaMeisai(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , TAGHAKKO_DATE");
            sb.ApdL("     , SHUKA_DATE");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , BOXKONPO_DATE");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , PALLETKONPO_DATE");
            sb.ApdL("     , KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , KIWAKUKONPO_DATE");
            sb.ApdL("     , JYOTAI_FLAG");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            if (cond.UpdateTani == BINKAN_IDO_TANI.TAG_VALUE1)
            {
                sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.TagNo));
            }
            else if (cond.UpdateTani == BINKAN_IDO_TANI.BOX_VALUE1)
            {
                sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            }
            else if (cond.UpdateTani == BINKAN_IDO_TANI.PALLET_VALUE1)
            {
                sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            }
            else if (cond.UpdateTani == BINKAN_IDO_TANI.KIWAKU_VALUE1)
            {
                sb.ApdN("   AND KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNoOrig));
                sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseId));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCDOrig));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_SHUKKA_MEISAI.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="kojiNo">工事識別管理No</param>
    /// <returns>木枠データ</returns>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockKiwaku(DatabaseHelper dbHelper, string kojiNoOrig, string kojiNoDest)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , KOJI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , TOROKU_FLAG");
            sb.ApdL("     , CASE_MARK_FILE");
            sb.ApdL("     , DELIVERY_NO");
            sb.ApdL("     , PORT_OF_DESTINATION");
            sb.ApdL("     , AIR_BOAT");
            sb.ApdL("     , DELIVERY_DATE");
            sb.ApdL("     , DELIVERY_POINT");
            sb.ApdL("     , FACTORY");
            sb.ApdL("     , REMARKS");
            sb.ApdL("     , SAGYO_FLAG");
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO IN (").ApdN(this.BindPrefix).ApdN("KOJI_NO_ORIG").ApdN(",").ApdN(this.BindPrefix).ApdN("KOJI_NO_DEST").ApdN(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO_ORIG", kojiNoOrig));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO_DEST", kojiNoDest));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_KIWAKU.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠明細取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="caseId">木枠梱包内部管理キー</param>
    /// <returns>木枠明細データ</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockKiwakuMeisai(DatabaseHelper dbHelper, string caseId)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , CASE_NO");
            sb.ApdL("     , PALLET_NO_1");
            sb.ApdL("     , PALLET_NO_2");
            sb.ApdL("     , PALLET_NO_3");
            sb.ApdL("     , PALLET_NO_4");
            sb.ApdL("     , PALLET_NO_5");
            sb.ApdL("     , PALLET_NO_6");
            sb.ApdL("     , PALLET_NO_7");
            sb.ApdL("     , PALLET_NO_8");
            sb.ApdL("     , PALLET_NO_9");
            sb.ApdL("     , PALLET_NO_10");
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU_MEISAI");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseId));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_KIWAKU_MEISAI.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレット管理データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// パレット管理データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="palletNo">パレットNo</param>
    /// <returns>パレット管理データ</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockPalletListManage(DatabaseHelper dbHelper, string palletNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PALLETLIST_MANAGE");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", palletNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_PALLETLIST_MANAGE.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ボックス管理データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// ボックス管理データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="boxNo">ボックスNo</param>
    /// <returns>ボックス管理データ</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockBoxListManage(DatabaseHelper dbHelper, string boxNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BOXLIST_MANAGE");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", boxNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_BOXLIST_MANAGE.Name;

            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region ボックス管理データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// ボックス管理データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">ボックスNoデータ</param>
    /// <returns>ボックス管理データ</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockBoxListManage(DatabaseHelper dbHelper, DataTable dtCond)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BOXLIST_MANAGE");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in dtCond.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.BOX_NO)));

                // SQL実行
                DataTable dt = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dt);

                if (dt.Rows.Count > 0)
                {
                    retDt.Merge(dt);
                }
            }
            return retDt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// 指定CaseID以外の明細カウント
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="kojiNo">工事識別No.</param>
    /// <param name="caseId">CaseID</param>
    /// <returns>件数</returns>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private int GetCountCase(DatabaseHelper dbHelper, string kojiNo, string caseId)
    {
        try
        {
            DataTable retDt = new DataTable();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       COUNT(1) CASE_COUNT");
            sb.ApdL("  FROM");
            sb.ApdL("       T_KIWAKU_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID != ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            DbParamCollection paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseId));

            // SQL実行
            DataTable dt = new DataTable();
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return ComFunc.GetFldToInt32(dt, 0, "CASE_COUNT");
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
    /// 工事識別No.をキーに作業区分を梱包登録中（１）に更新
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <param name="kojiNo">工事識別No.</param>
    /// <param name="sagyoFlag">作業フラグ</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwakuSagyoFlag(DatabaseHelper dbHelper, CondS01 cond, string kojiNo, string sagyoFlag)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU");
            sb.ApdL("   SET");
            sb.ApdN("       SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", sagyoFlag));
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

    #region 木枠明細データ工事識別No.更新
    /// --------------------------------------------------
    /// <summary>
    /// CaseIDをキーに工事識別No.を更新
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">更新条件</param>
    /// <create>T.Wakamatsu 2016/04/06</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwakuMeisaiKojiNo(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseId));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNoDest));
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

    #region 木枠明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新データ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdKiwakuMeisaiPallet(DatabaseHelper dbHelper, DataTable dt, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       PALLET_NO_1 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_1");
            sb.ApdN("     , PALLET_NO_2 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_2");
            sb.ApdN("     , PALLET_NO_3 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_3");
            sb.ApdN("     , PALLET_NO_4 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_4");
            sb.ApdN("     , PALLET_NO_5 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_5");
            sb.ApdN("     , PALLET_NO_6 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_6");
            sb.ApdN("     , PALLET_NO_7 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_7");
            sb.ApdN("     , PALLET_NO_8 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_8");
            sb.ApdN("     , PALLET_NO_9 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_9");
            sb.ApdN("     , PALLET_NO_10 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_10");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.CASE_ID)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_1", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_2", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_3", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_3, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_4", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_4, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_5", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_5, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_6", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_6, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_7", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_7, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_8", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_8, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_9", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_9, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO_10", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PALLET_NO_10, DBNull.Value)));
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

    #region パレット管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレット管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新対象パレット管理データ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdPalletListManage(DatabaseHelper dbHelper, DataTable dt, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("   SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_PALLETLIST_MANAGE.PALLET_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCDDest));
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

    #region ボックス管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// ボックス管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新対象ボックス管理データ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int UpdBoxListManage(DatabaseHelper dbHelper, DataTable dt, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BOXLIST_MANAGE");
            sb.ApdL("   SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.BOX_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCDDest));
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

    #region 移動出荷明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 移動出荷明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">更新対象出荷明細データ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update>J.Chen 2022/12/19 TAG便名追加</update>
    /// --------------------------------------------------
    private int UpdIdoShukkaMeisai(DatabaseHelper dbHelper, DataTable dt, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("   SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD_DEST");
            sb.ApdL("     , BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdL("     , BOXKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("BOXKONPO_DATE");
            sb.ApdL("     , PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL("     , PALLETKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("PALLETKONPO_DATE");
            sb.ApdL("     , KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("     , CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdL("     , KIWAKUKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("KIWAKUKONPO_DATE");
            sb.ApdL("     , JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , TAG_SHIP = ").ApdN(this.BindPrefix).ApdL("TAG_SHIP");
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD_ORIG");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD_ORIG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD_DEST", cond.NonyusakiCDDest));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOX_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("BOXKONPO_DATE", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOXKONPO_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.PALLET_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLETKONPO_DATE", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.PALLETKONPO_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KOJI_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.CASE_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("KIWAKUKONPO_DATE", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KIWAKUKONPO_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_SHIP", cond.Ship == null ? "" : cond.Ship));

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

    #region パレット管理データ削除

    /// --------------------------------------------------
    /// <summary>
    /// パレット管理データ削除
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="palletNo">パレットNo</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelPalletListManage(DatabaseHelper dbHelper, string palletNo)
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

    #region ボックス管理データ削除

    /// --------------------------------------------------
    /// <summary>
    /// ボックス管理データ削除
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="boxNo">ボックスNo</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2016/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelBoxListManage(DatabaseHelper dbHelper, string boxNo)
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

    #endregion

    #endregion

    #region S0100023:TAG登録連携

    #region SELECT

    #region 物件名一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update>D.Okumura 2019/01/25 ARと本体が対になっている物件のみにする</update>
    /// --------------------------------------------------
    public DataSet GetBukkenNameList(DatabaseHelper dbHelper, CondS01 cond)
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
            // ARと本体が対になっている物件のみにする
            sb.ApdL("   AND PROJECT_NO IS NOT NULL");

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

    #region 便一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 便一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update>D.Okumura 2022/01/04 本体/ARの判断は出荷フラグで判断できるためコンディションからARフラグを削除</update>
    /// <update>2022/05/25 STEP14</update>
    /// --------------------------------------------------
    public DataSet GetShipList(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT IsNull(NM.SHIP, '-') + '     '");
            sb.ApdL("     + IsNull(RIGHT(CONVERT(NVARCHAR,NM.SHIP_DATE,11),5) + '('");
            sb.ApdL("     + CW.ITEM_NAME");
            sb.ApdL("     + ')', '-') + '  '");
            sb.ApdL("     + IsNull(CM.ITEM_NAME, '-') + '  '");
            sb.ApdL("     + IsNull(NM.TRANSPORT_FLAG, '-') + '  '");
            sb.ApdL("     + IsNull(NM.SHIP_TO, '-') + '  '");
            sb.ApdL("     + IsNull(NM.SHIP_FROM, '-')");
            sb.ApdL("       AS DISP_SHIP");
            sb.ApdL("     , NM.SHIP");
            sb.ApdL("     , NM.NONYUSAKI_CD");
            sb.ApdL("     , NM.ESTIMATE_FLAG");
            sb.ApdL("     , NM.SHIP_TO");
            sb.ApdL("  FROM M_NONYUSAKI NM");
            sb.ApdL("  LEFT JOIN M_COMMON CM ON CM.GROUP_CD = 'ESTIMATE_FLAG' AND CM.VALUE1 = NM.ESTIMATE_FLAG AND CM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON CW ON CW.GROUP_CD = 'WEEK_NAME' AND CW.VALUE1 = DATEPART(WEEKDAY, NM.SHIP_DATE) AND CW.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");

            sb.ApdL("  WHERE 1=1");

            // 本体の場合、出荷日(null以外)
            if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                sb.ApdN("     AND NM.SHIP_DATE IS NOT NULL");
            }

            // 管理区分(未完固定)
            sb.ApdN("       AND NM.KANRI_FLAG = ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            paramCollection.Add(iNewParameter.NewDbParameter("KANRI_FLAG", KANRI_FLAG.MIKAN_VALUE1));

            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND NM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            // 物件管理NO
            if (!string.IsNullOrEmpty(cond.BukkenNO))
            {
                sb.ApdN("   AND NM.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
                paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            }
            sb.ApdL(" ORDER BY NM.SHIP");

            // バインド
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
    #endregion

    #region TAG連携一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// TAG連携一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/08</create>
    /// <update>K.Tsutsumi 2019/01/07 連携している出荷明細が存在しないとデータが表示されない不具合の対応</update>
    /// <update>D.Okumura 2019/01/09 大量のデータがあるとき応答が返ってこない不具合を修正</update>
    /// <update>D.Okumura 2019/01/25 M-Noへ連携されない不具合を修正</update>
    /// <update>T.Nukaga 2019/11/18 返却品管理対応 返却品は表示しないようにSQL追加</update>
    /// <update>D.Okumura 2019/12/11 余剰品がTAG連携可能数に計上されない問題を修正</update>
    /// <update>J.Chen 2022/05/25 STEP14</update>
    /// <update>J.Chen 2022/12/21 図番型式2追加</update>
    /// <update>J.Chen 2023/09/28 返却品の区分「保留」「通関確認中」は「返却対象」と同じ処理とする</update>
    /// <update>J.Jeong 2024/08/15 同じ連携Noの手配No結合</update>
    /// --------------------------------------------------
    public DataSet GetTagRenkeiList(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文

            sb.ApdL("SELECT");
            sb.ApdL("     a.TEHAI_RENKEI_NO");
            sb.ApdL("    ,a.ZUMEN_OIBAN");
            sb.ApdL("    ,a.FLOOR");
            sb.ApdL("    ,a.ST_NO");
            sb.ApdL("    ,a.ZUMEN_KEISHIKI");
            sb.ApdL("    ,a.ZUMEN_KEISHIKI2");
            sb.ApdL("    ,a.HINMEI_JP");
            sb.ApdL("    ,a.HINMEI");
            sb.ApdL("    ,a.HINMEI_INV");
            sb.ApdL("    ,a.SHUKKA_QTY");
            sb.ApdL("    ,a.FREE1");
            sb.ApdL("    ,a.FREE2");
            sb.ApdL("    ,a.NOTE");
            sb.ApdL("    ,a.VERSION");
            sb.ApdL("    ,a.ECS_NO");
            sb.ApdL("    ,a.SEIBAN");
            sb.ApdL("    ,a.CODE");
            sb.ApdL("    ,a.KISHU");
            sb.ApdL("    ,a.AR_NO");
            sb.ApdL("    ,a.ZAN_QTY");
            sb.ApdL("    ,a.SHUKKA_QTY_SUM");
            sb.ApdL(" 	,(CASE WHEN (a.TAG_TOUROKU_ALLOW - a.SHUKKA_QTY_SUM) < 0 THEN 0");
            sb.ApdL("          ELSE (a.TAG_TOUROKU_ALLOW - a.SHUKKA_QTY_SUM)");
            sb.ApdL("      END) TAG_TOUROKU_MAX");
            sb.ApdL("    ,a.SYUKKA_SAKI");
            sb.ApdL("    ,STRING_AGG(a.TEHAI_NO, '+') AS TEHAI_NO");
            sb.ApdL("FROM (");
            sb.ApdL("SELECT");
            sb.ApdL("	 TM.TEHAI_RENKEI_NO");
            sb.ApdL("	,TM.ZUMEN_OIBAN");
            sb.ApdL("	,TM.FLOOR");
            sb.ApdL("	,TM.ST_NO");
            sb.ApdL("	,TM.ZUMEN_KEISHIKI");
            sb.ApdL("	,TM.ZUMEN_KEISHIKI2");
            sb.ApdL("	,TM.HINMEI_JP");
            sb.ApdL("	,TM.HINMEI");
            sb.ApdL("	,TM.HINMEI_INV");
            sb.ApdL("	,TM.SHUKKA_QTY");
            sb.ApdL("	,TM.FREE1");
            sb.ApdL("	,TM.FREE2");
            sb.ApdL("	,TM.NOTE");
            sb.ApdL("	,TM.VERSION");
            sb.ApdL("	,EM.ECS_NO");
            sb.ApdL("	,EM.SEIBAN");
            sb.ApdL("	,EM.CODE");
            sb.ApdL("	,EM.KISHU");
            sb.ApdL("	,EM.AR_NO");
            sb.ApdL("	,(TM.SHUKKA_QTY - IsNull(S_MEISAI.NUM, 0)) AS ZAN_QTY");
            sb.ApdL("	,IsNull(S_MEISAI.NUM, 0) AS SHUKKA_QTY_SUM");
            sb.ApdL("	,(CASE WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ORDERED");
            sb.ApdL("            THEN TM.ARRIVAL_QTY");
            sb.ApdL("		   WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_ASSY");
            sb.ApdL("            THEN TM.ASSY_QTY");
            sb.ApdL("		   WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SKS_SKIP");
            sb.ApdL("            THEN TM.ARRIVAL_QTY");
            sb.ApdL("		   WHEN TM.TEHAI_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_SURPLUS");
            sb.ApdL("            THEN TM.ARRIVAL_QTY");
            sb.ApdL("	  END) AS TAG_TOUROKU_ALLOW");
            sb.ApdL("	,TM.SYUKKA_SAKI");
            sb.ApdL("   ,TEHAI_SKS.TEHAI_NO");
            sb.ApdL("	FROM T_TEHAI_MEISAI TM");
            sb.ApdL("	INNER JOIN M_ECS EM ON EM.ECS_QUOTA = TM.ECS_QUOTA AND EM.ECS_NO = TM.ECS_NO");
            sb.ApdL("	INNER JOIN M_BUKKEN bb ON bb.PROJECT_NO = EM.PROJECT_NO");
            sb.ApdL("	LEFT JOIN (");
            sb.ApdL("		SELECT TEHAI_RENKEI_NO, SUM(NUM) AS NUM");
            sb.ApdL("			FROM T_SHUKKA_MEISAI");
            sb.ApdL("			GROUP BY TEHAI_RENKEI_NO");
            sb.ApdL("	) AS S_MEISAI ON TM.TEHAI_RENKEI_NO = S_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdL("    LEFT JOIN T_TEHAI_MEISAI_SKS TEHAI_SKS ON TM.TEHAI_RENKEI_NO = TEHAI_SKS.TEHAI_RENKEI_NO");
            sb.ApdL("	WHERE");
            sb.ApdL("	        TM.SHUKKA_QTY > IsNull(S_MEISAI.NUM,0)");
            sb.ApdN("		AND bb.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("		AND bb.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("       AND TM.TEHAI_FLAG != ").ApdN(this.BindPrefix).ApdL("TEHAI_FLAG_CANCELLED");
            sb.ApdN("       AND TM.HENKYAKUHIN_FLAG = ").ApdN(this.BindPrefix).ApdL("HENKYAKUHIN_FLAG_NORMAL");
            // 有償・無償フラグ
            if (!string.IsNullOrEmpty(cond.EstimateFlag))
            {
                sb.ApdN("   AND TM.ESTIMATE_FLAG = ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");/* 有償無償 by 納入先マスタ */
                paramCollection.Add(iNewParameter.NewDbParameter("ESTIMATE_FLAG", cond.EstimateFlag));
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.ShipTo))
            {
                sb.ApdN("   AND TM.SYUKKA_SAKI = ").ApdN(this.BindPrefix).ApdL("SYUKKA_SAKI");/* 出荷先 by 納入先マスタ */
                paramCollection.Add(iNewParameter.NewDbParameter("SYUKKA_SAKI", cond.ShipTo));
            }
            // 出荷フラグ
            if (cond.ShukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
            {
                sb.ApdL("   AND EM.AR_NO IS NULL");
            }
            else if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                sb.ApdL("   AND EM.AR_NO IS NOT NULL");
            }
            // 製番+コード
            if (!string.IsNullOrEmpty(cond.SibanCodeList))
            {
                string[] SibanArr = cond.SibanCodeList.Split(',');
                sb.ApdL("		AND (");
                for (int i = 0; i < SibanArr.Length; i++)
                {
                    if (i > 0) sb.ApdL("OR ");
                    sb.ApdN("EM.SEIBAN_CODE LIKE ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE" + i.ToString());
                    paramCollection.Add(iNewParameter.NewDbParameter("SEIBAN_CODE" + i.ToString(), SibanArr[i].Trim() + "%"));
                }
                sb.ApdL("		)");
            }
            // ECS No.
            if (!string.IsNullOrEmpty(cond.EcsNoList))
            {
                string[] EcsArr = cond.EcsNoList.Split(',');
                sb.ApdL("		AND (");
                for (int i = 0; i < EcsArr.Length; i++)
                {
                    if (i > 0) sb.ApdL("OR ");
                    sb.ApdN("TM.ECS_NO LIKE ").ApdN(this.BindPrefix).ApdL("ECS_NO" + i.ToString());
                    paramCollection.Add(iNewParameter.NewDbParameter("ECS_NO" + i.ToString(), EcsArr[i].Trim() + "%"));
                }
                sb.ApdL("		)");
            }
            sb.ApdL(") AS a");
            sb.ApdL("GROUP BY");
            sb.ApdL("    a.TEHAI_RENKEI_NO");
            sb.ApdL("   ,a.ZUMEN_OIBAN");
            sb.ApdL("   ,a.FLOOR");
            sb.ApdL("   ,a.ST_NO");
            sb.ApdL("   ,a.ZUMEN_KEISHIKI");
            sb.ApdL("   ,a.ZUMEN_KEISHIKI2");
            sb.ApdL("   ,a.HINMEI_JP");
            sb.ApdL("   ,a.HINMEI");
            sb.ApdL("   ,a.HINMEI_INV");
            sb.ApdL("   ,a.SHUKKA_QTY");
            sb.ApdL("   ,a.FREE1");
            sb.ApdL("   ,a.FREE2");
            sb.ApdL("   ,a.NOTE");
            sb.ApdL("   ,a.VERSION");
            sb.ApdL("   ,a.ECS_NO");
            sb.ApdL("   ,a.SEIBAN");
            sb.ApdL("   ,a.CODE");
            sb.ApdL("   ,a.KISHU");
            sb.ApdL("   ,a.AR_NO");
            sb.ApdL("   ,a.ZAN_QTY");
            sb.ApdL("   ,a.SHUKKA_QTY_SUM");
            sb.ApdL("   ,a.TAG_TOUROKU_ALLOW");
            sb.ApdL("   ,a.SYUKKA_SAKI");
            sb.ApdL(" ORDER BY a.ECS_NO,a.ZUMEN_KEISHIKI");
            // バインド
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_ORDERED", TEHAI_FLAG.ORDERED_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_ASSY", TEHAI_FLAG.ASSY_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_SKS_SKIP", TEHAI_FLAG.SKS_SKIP_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_SURPLUS", TEHAI_FLAG.SURPLUS_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_FLAG_CANCELLED", TEHAI_FLAG.CANCELLED_VALUE1));

            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));

            paramCollection.Add(iNewParameter.NewDbParameter("HENKYAKUHIN_FLAG_NORMAL", HENKYAKUHIN_FLAG.NORMAL_VALUE1));

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

    #region TAG連携一覧取得（ロック用）
    /// --------------------------------------------------
    /// <summary>
    /// TAG連携一覧取得（ロック用）
    /// </summary>
    /// <param name="dbHelper">DatabaseHlper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="ds">データ</param>
    /// <param name="isLock">true:ロックあり false:ロックなし</param>
    /// <returns>DataTable</returns>
    /// <create>T.Nakata 2018/11/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetAndLockTagRenkeiList(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, bool isLock)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM T_TEHAI_MEISAI");

            if (isLock == true)
            {
                sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            }

            sb.ApdL(" WHERE 1=0");

            int i = 0;
            foreach (DataRow dr in ds.Tables[Def_T_SHUKKA_MEISAI.Name].Rows)
            {
                sb.ApdN("   OR TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO" + i);
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO" + i++, ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.TEHAI_RENKEI_NO)));
            }

            sb.ApdL("ORDER BY ECS_NO, ZUMEN_KEISHIKI");

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

    #region 有償/無償取得
    /// --------------------------------------------------
    /// <summary>
    /// 有償/無償取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetEstimateFlag(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ESTIMATE_FLAG");
            sb.ApdL("  FROM M_NONYUSAKI");
            sb.ApdL(" WHERE");

            // 管理区分(未完固定)
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));

            // 出荷区分
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));

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

    #region TAG連携と手配Noの出荷先をチェック
    /// --------------------------------------------------
    /// <summary>
    /// TAG連携と手配Noの出荷先をチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataTable</returns>
    /// <create>2022/05/31 STEP14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShipToForTehaiNo(DatabaseHelper dbHelper, CondS01 cond, string tehaiNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEHAI_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT T_TEHAI_MEISAI.SYUKKA_SAKI");
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdN(" WHERE T_TEHAI_MEISAI.TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");
            sb.ApdL("   AND EXISTS (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM M_NONYUSAKI");
            sb.ApdL("        WHERE M_NONYUSAKI.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("          AND M_NONYUSAKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("          AND M_NONYUSAKI.SHIP = ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdL("          AND M_NONYUSAKI.SHIP_TO = T_TEHAI_MEISAI.SYUKKA_SAKI");
            sb.ApdL("   )");

            paramCollection.Add(iNewParameter.NewDbParameter("TEHAI_RENKEI_NO", tehaiNo));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("SHIP", cond.Ship));

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

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細バージョン更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dt">更新データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/11/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaimeisaiVersion(DatabaseHelper dbHelper, CondS01 cond, DataTable dt)
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
            sb.ApdN("       UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdN(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       TEHAI_RENKEI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_RENKEI_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_RENKEI_NO", ComFunc.GetFldObject(dr, Def_T_TEHAI_MEISAI.TEHAI_RENKEI_NO)));

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

    #region S0100040:一括アップロード

    #region SELECT

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dtSearch">検索用データ</param>
    /// <returns>表示データ</returns>
    /// <create>H.Tajimi 2019/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable Sql_GetTehaiMeisaiDataForIkkatsuUpload(DatabaseHelper dbHelper, CondS01 cond, DataTable dtSearch)
    {
        try
        {
            DataTable ret = dtSearch.Copy();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT TTM.HINMEI_JP");
            sb.ApdL("     , TTM.HINMEI");
            sb.ApdL("  FROM (");
            sb.ApdL("    SELECT ROW_NUMBER() OVER (ORDER BY TMP.ZUMEN_KEISHIKI) AS ROW_NUM");
            sb.ApdL("         , TMP.HINMEI_JP");
            sb.ApdL("         , TMP.HINMEI");
            sb.ApdL("      FROM T_TEHAI_MEISAI TMP");
            sb.ApdN("     WHERE TMP.ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("        OR TMP.ZUMEN_KEISHIKI2 = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdL("  ) TTM");
            sb.ApdL(" WHERE TTM.ROW_NUM = 1");

            foreach (DataRow dr in ret.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                var zumenKeishiki = ComFunc.GetFld(dr, ComDefine.FLD_MB_ZUMEN_KEISHIKI);
                zumenKeishiki = UtilConvert.StrConv(zumenKeishiki, Conversion.Narrow);
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", zumenKeishiki));

                // SQL実行
                var dtTmp = new DataTable(Def_T_TEHAI_MEISAI.Name);
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);

                dr.SetField<object>(Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI, zumenKeishiki);
                if (UtilData.ExistsData(dtTmp))
                {
                    dr.SetField<object>(Def_T_TEHAI_MEISAI.HINMEI_JP, UtilData.GetFldToObject(dtTmp, 0, Def_T_TEHAI_MEISAI.HINMEI_JP));
                    dr.SetField<object>(Def_T_TEHAI_MEISAI.HINMEI, UtilData.GetFldToObject(dtTmp, 0, Def_T_TEHAI_MEISAI.HINMEI));
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
    /// 画像ファイル有無取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <param name="dtSearch">検索用データ</param>
    /// <returns>表示データ</returns>
    /// <create>H.Tajimi 2019/08/21</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetExistsPicture(DatabaseHelper dbHelper, CondS01 cond, DataTable dtSearch)
    {
        try
        {
            DataTable ret = dtSearch.Copy();
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("SELECT CASE");
            sb.ApdL("        WHEN EXISTS (");
            sb.ApdL("          SELECT 1");
            sb.ApdL("             FROM T_MANAGE_ZUMEN_KEISHIKI MZK");
            sb.ApdL("            WHERE MZK.ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("        ) THEN ").ApdN(this.BindPrefix).ApdL("EXISTS_VALUE");
            sb.ApdN("        ELSE ").ApdN(this.BindPrefix).ApdN("NOT_EXISTS_VALUE").ApdL(" END AS EXISTS_PICTURE");

            foreach (DataRow dr in ret.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", UtilData.GetFldToObject(dr, Def_T_TEHAI_MEISAI.ZUMEN_KEISHIKI)));
                paramCollection.Add(iNewParam.NewDbParameter("EXISTS_VALUE", ComDefine.EXISTS_PICTURE_VALUE));
                paramCollection.Add(iNewParam.NewDbParameter("NOT_EXISTS_VALUE", ComDefine.NOT_EXISTS_PICTURE_VALUE));

                // SQL実行
                var dtTmp = new DataTable(Def_T_TEHAI_MEISAI.Name);
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                dr.SetField<object>(ComDefine.FLD_EXISTS_PICTURE, UtilData.GetFldToObject(dtTmp, 0, ComDefine.FLD_EXISTS_PICTURE));
            }
            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の取得

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">更新データ</param>
    /// <returns>DataTable</returns>
    /// <create>H.Tajimi 2019/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetManageZumenKeishiki(DatabaseHelper dbHelper, DataRow dr)
    {
        DataTable ret = new DataTable(Def_T_MANAGE_ZUMEN_KEISHIKI.Name);
        StringBuilder sb = new StringBuilder();
        INewDbParameterBasic iNewParam = dbHelper;

        sb.ApdL("SELECT");
        sb.ApdL("       T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI");
        sb.ApdL("     , T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME");
        sb.ApdL("     , T_MANAGE_ZUMEN_KEISHIKI.SAVE_DIR");
        sb.ApdL("     , T_MANAGE_ZUMEN_KEISHIKI.CREATE_DATE");
        sb.ApdL("  FROM");
        sb.ApdL("       T_MANAGE_ZUMEN_KEISHIKI");
        sb.ApdL(" WHERE");
        sb.ApdN("       T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI = ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");

        DbParamCollection paramCollection = new DbParamCollection();

        // バインド変数設定
        paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", ComFunc.GetFld(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI)));

        // SQL実行
        dbHelper.Fill(sb.ToString(), paramCollection, ret);
        return ret;
    }

    #endregion

    #endregion

    #region INSERT

    #region 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の登録

    /// --------------------------------------------------
    /// <summary>
    /// 図番/型式管理データ (T_MANAGE_ZUMEN_KEISHIKI)の登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">登録データ</param>
    /// <returns>True:登録成功/False:登録失敗</returns>
    /// <create>H.Tajimi 2019/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    internal int InsManageZumenKeishiki(DatabaseHelper dbHelper, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_MANAGE_ZUMEN_KEISHIKI");
            sb.ApdL("(");
            sb.ApdL("       ZUMEN_KEISHIKI");
            sb.ApdL("     , FILE_NAME");
            sb.ApdL("     , SAVE_DIR");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL(") VALUES (");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("ZUMEN_KEISHIKI");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FILE_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SAVE_DIR");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection = new DbParamCollection();
            paramCollection.Add(iNewParam.NewDbParameter("ZUMEN_KEISHIKI", ComFunc.GetFldObject(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.ZUMEN_KEISHIKI)));
            paramCollection.Add(iNewParam.NewDbParameter("FILE_NAME", ComFunc.GetFldObject(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.FILE_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("SAVE_DIR", ComFunc.GetFldObject(dr, Def_T_MANAGE_ZUMEN_KEISHIKI.SAVE_DIR)));

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

    #region S0100050:荷姿表登録

    #region 制御

    #region 初期データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 初期データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>初期データ一式</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update>H.Tajimi 2020/04/14 出荷元マスタ、荷姿表並び順取得</update>
    /// --------------------------------------------------
    public DataSet S0100050_Ctrl_GetInit(DatabaseHelper dbHelper, CondS01 cond)
    {
        var ds = new DataSet();

        // 汎用マスタ
        using (var impl = new WsCommonImpl())
        {
            var condCommon = new CondCommon(cond.LoginInfo);

            // 汎用マスタ(PACKING_MAIL_SUBJECT)
            condCommon.GroupCD = PACKING_MAIL_SUBJECT.GROUPCD;
            var dsPms = impl.GetCommon(dbHelper, condCommon);
            if (dsPms != null)
            {
                var dt = dsPms.Tables[Def_M_COMMON.Name];

                // テーブル名変更
                dt.TableName = condCommon.GroupCD;

                //// 空白行追加
                //var dr = dt.NewRow();
                //UtilData.SetFld(dr, Def_M_COMMON.VALUE1, null);
                //UtilData.SetFld(dr, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_BLANK_DISP);
                //dt.Rows.InsertAt(dr, 0);
                //dt.AcceptChanges();

                ds.Merge(dt);
            }

            // 汎用マスタ(FORM_STYLE_FLAG)
            condCommon.GroupCD = FORM_STYLE_FLAG.GROUPCD;
            var dsFs = impl.GetCommon(dbHelper, condCommon);
            if (dsFs != null)
            {
                var dt = dsFs.Tables[Def_M_COMMON.Name];

                // テーブル名変更
                dt.TableName = condCommon.GroupCD;

                //// 空白行追加
                //var dr = dt.NewRow();
                //UtilData.SetFld(dr, Def_M_COMMON.VALUE1, null);
                //UtilData.SetFld(dr, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_BLANK_DISP);
                //dt.Rows.InsertAt(dr, 0);
                //dt.AcceptChanges();

                ds.Merge(dt);
            }

            // 汎用マスタ(PL_TYPE)
            condCommon.GroupCD = PL_TYPE.GROUPCD;
            var dsPy = impl.GetCommon(dbHelper, condCommon);
            if (dsPy != null)
            {
                var dt = dsPy.Tables[Def_M_COMMON.Name];

                // テーブル名変更
                dt.TableName = condCommon.GroupCD;

                //// 空白行追加
                //var dr = dt.NewRow();
                //UtilData.SetFld(dr, Def_M_COMMON.VALUE1, null);
                //UtilData.SetFld(dr, Def_M_COMMON.ITEM_NAME, ComDefine.COMBO_BLANK_DISP);
                //dt.Rows.InsertAt(dr, 0);
                //dt.AcceptChanges();

                ds.Merge(dt);
            }

            // 汎用マスタ(NISUGATA_HAKKO_SORT)
            condCommon.GroupCD = NISUGATA_HAKKO_SORT.GROUPCD;
            var dsSort = impl.GetCommon(dbHelper, condCommon);
            if (dsSort != null)
            {
                var dt = dsSort.Tables[Def_M_COMMON.Name];
                // テーブル名変更
                dt.TableName = condCommon.GroupCD;
                ds.Merge(dt);
            }
        }

        // 物件名マスタ(AR)
        cond.ShukkaFlag = SHUKKA_FLAG.AR_VALUE1;
        var dsAr = this.Sql_GetBukkenData(dbHelper, cond);
        if (dsAr != null)
        {
            var dt = dsAr.Tables[Def_M_BUKKEN.Name];

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_BUKKEN.BUKKEN_NO, null);
            //UtilData.SetFld(dr, Def_M_BUKKEN.BUKKEN_NAME, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 運送会社マスタ(国内)
        cond.KokunaigaiFlag = KOKUNAI_GAI_FLAG.NAI_VALUE1;
        var dsUn = this.Sql_GetUnsokaisya(dbHelper, cond);
        if (dsUn != null)
        {
            var dt = dsUn.Tables[Def_M_UNSOKAISHA.Name];

            // テーブル名変更
            dt.TableName = Def_M_UNSOKAISHA.Name + KOKUNAI_GAI_FLAG.NAI_VALUE1;

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NO, null);
            //UtilData.SetFld(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NAME, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 運送会社マスタ(国外)
        cond.KokunaigaiFlag = KOKUNAI_GAI_FLAG.GAI_VALUE1;
        var dsUg = this.Sql_GetUnsokaisya(dbHelper, cond);
        if (dsUg != null)
        {
            var dt = dsUg.Tables[Def_M_UNSOKAISHA.Name];

            // テーブル名変更
            dt.TableName = Def_M_UNSOKAISHA.Name + KOKUNAI_GAI_FLAG.GAI_VALUE1;

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NO, null);
            //UtilData.SetFld(dr, Def_M_UNSOKAISHA.UNSOKAISHA_NAME, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 名称マスタ取得(宛先)
        cond.SelectGroupCD = SELECT_GROUP_CD.ATTN_VALUE1;
        var dsSgc = this.Sql_GetSelectItemData(dbHelper, cond);
        if (dsSgc != null)
        {
            var dt = dsSgc.Tables[Def_M_SELECT_ITEM.Name];

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_SELECT_ITEM.ITEM_NAME, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 出荷元マスタ取得
        var condShipFrom = (CondS01)cond.Clone();
        condShipFrom.UnusedFlag = UNUSED_FLAG.USED_VALUE1;
        ds.Merge(this.Sql_GetShipFrom(dbHelper, condShipFrom));

        return ds;
    }

    #endregion

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>物件一覧、納入先一覧、表示データ</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet S0100050_Ctrl_GetDisp(DatabaseHelper dbHelper, CondS01 cond)
    {
        var ds = new DataSet();

        // 出荷日指定物件名マスタ
        var dsbukken = this.Sql_GetNounyuBukkenData(dbHelper, cond);
        if (dsbukken != null)
        {
            var dt = dsbukken.Tables[Def_M_NONYUSAKI.Name];

            // テーブル名変更
            dt.TableName = Def_M_BUKKEN.Name;

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO, ComDefine.COMBO_BLANK_VALUE_INT);
            //UtilData.SetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 出荷日指定納入先マスタ
        var dsNonyusaki = this.Sql_GetNonyusakiData(dbHelper, cond);
        if (dsNonyusaki != null)
        {
            var dt = dsNonyusaki.Tables[Def_M_NONYUSAKI.Name];

            //// 空白行追加
            //var dr = dt.NewRow();
            //UtilData.SetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD, ComDefine.COMBO_BLANK_VALUE_STRING);
            //UtilData.SetFld(dr, Def_M_NONYUSAKI.SHIP, ComDefine.COMBO_BLANK_DISP);
            //dt.Rows.InsertAt(dr, 0);
            //dt.AcceptChanges();

            ds.Merge(dt);
        }

        // 宛先取得
        var dsNisugata = this.Sql_GetNisugataData(dbHelper, cond);
        if (dsNisugata != null)
        {
            var dt = dsNisugata.Tables[Def_T_PACKING_MEISAI.Name];
            foreach (DataRow dr in dt.Rows)
            {
                var attn = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.ATTN, null);
                var bukkenno = ComFunc.GetFld(dr, "BUKKEN_NO", null);
                var escno = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.ECS_NO, null);
                var pallet = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.PALLET_NO, null);
                var box = ComFunc.GetFld(dr, Def_T_PACKING_MEISAI.BOX_NO, null);
                if (string.IsNullOrEmpty(attn))
                {
                    if (string.IsNullOrEmpty(pallet) && string.IsNullOrEmpty(box)) continue;
                    DataTable dtTehaiMeisai = null;
                    dtTehaiMeisai = GetTehaiMeisaiSampleForNisugataByESCNo(dbHelper, box, pallet, bukkenno);

                    string syukka_saki = "";
                    syukka_saki += ComFunc.GetFld(dtTehaiMeisai, 0, Def_T_TEHAI_MEISAI.SYUKKA_SAKI);
                    dr.SetField(Def_T_PACKING_MEISAI.ATTN, string.IsNullOrEmpty(syukka_saki) ? null : syukka_saki);
                }
            }
        }

        // 荷姿情報一覧取得
        ds.Merge(dsNisugata);

        return ds;
    }
    #endregion

    #region C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）

    /// --------------------------------------------------
    /// <summary>
    /// C/Noより木枠の有無確認（取得できた場合はサイズと重量も取得）
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>木枠明細</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet S0100050_Ctrl_GetKiwakuMeisai(DatabaseHelper dbHelper, CondS01 cond)
    {
        var ds = new DataSet();

        ds.Merge(this.Sql_GetKiwakuMeisai(dbHelper, cond));

        return ds;
    }
    #endregion

    #region Box No.より、Boxリスト管理データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Box No.より、Boxリスト管理データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">メッセージパラメータ</param>
    /// <returns>true:存在した false:存在しない</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool S0100050_Ctrl_GetBoxListManage(DatabaseHelper dbHelper, CondS01 cond, ref string errMsgID, ref string[] args)
    {

        DataSet ds = this.Sql_GetBoxListManage(dbHelper, cond);
        if (!UtilData.ExistsData(ds, Def_T_BOXLIST_MANAGE.Name))
        {
            // Box No.は存在しません。
            errMsgID = "S0100050034";
            return false;
        }

        if (!string.IsNullOrEmpty(UtilData.GetFld(ds, Def_T_BOXLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.PALLET_NO)))
        {
            // Pallet梱包済みのBox No.です。
            errMsgID = "S0100050037";
            return false;
        }

        return true;
    }
    #endregion

    #region Pallet No.より、Palletリスト管理データ取得

    /// --------------------------------------------------
    /// <summary>
    /// Pallet No.より、Palletリスト管理データ取得
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>出荷明細</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool S0100050_Ctrl_GetPalletListManage(DatabaseHelper dbHelper, CondS01 cond, ref string errMsgID, ref string[] args)
    {

        DataSet ds = this.Sql_GetPalletListManage(dbHelper, cond);
        if (!UtilData.ExistsData(ds, Def_T_PALLETLIST_MANAGE.Name))
        {
            // Pallet No.は存在しません。
            errMsgID = "S0100050035";
            return false;
        }

        if (!string.IsNullOrEmpty(UtilData.GetFld(ds, Def_T_PALLETLIST_MANAGE.Name, 0, Def_T_SHUKKA_MEISAI.KOJI_NO)))
        {
            // 木枠梱包済みのPallet No.です。
            errMsgID = "S0100050038";
            return false;
        }

        return true;
    }
    #endregion

    #endregion

    #region SELECT

    #region 運送会社一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 運送会社一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update>K.Tsutsumi 2018/01/23 並び順対応</update>
    /// --------------------------------------------------
    private DataSet Sql_GetUnsokaisya(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("  UNSOKAISHA_NO ");
            sb.ApdL("  , KOKUNAI_GAI_FLAG ");
            sb.ApdL("  , UNSOKAISHA_NAME ");
            sb.ApdL("  , INVOICE_FLAG ");
            sb.ApdL("  , PACKINGLIST_FLAG ");
            sb.ApdL("  , EXPORTCONFIRM_FLAG ");
            sb.ApdL("  , EXPORTCONFIRM_ATTN ");
            //            sb.ApdL("  , CREATE_DATE ");
            //            sb.ApdL("  , CREATE_USER_ID ");
            //            sb.ApdL("  , CREATE_USER_NAME ");
            //            sb.ApdL("  , UPDATE_DATE ");
            //            sb.ApdL("  , UPDATE_USER_ID ");
            //            sb.ApdL("  , UPDATE_USER_NAME ");
            //            sb.ApdL("  , MAINTE_DATE ");
            //            sb.ApdL("  , MAINTE_USER_ID ");
            //            sb.ApdL("  , MAINTE_USER_NAME ");
            //            sb.ApdL("  , VERSION ");
            sb.ApdL("  , SORT_NO ");
            sb.ApdL("  FROM M_UNSOKAISHA");
            sb.ApdL(" WHERE 1 = 1");
            // 国内外
            if (!string.IsNullOrEmpty(cond.KokunaigaiFlag))
            {
                sb.ApdN("   AND KOKUNAI_GAI_FLAG = ").ApdN(this.BindPrefix).ApdL("KOKUNAI_GAI_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("KOKUNAI_GAI_FLAG", cond.KokunaigaiFlag));
            }
            sb.ApdL(" ORDER BY SORT_NO");

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

    #region 物件一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 物件一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetBukkenData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("    BM.BUKKEN_NO");
            sb.ApdL("  , BM.BUKKEN_NAME");
            sb.ApdL("  , NM.NONYUSAKI_CD");
            sb.ApdL(" FROM M_BUKKEN BM");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI NM ON BM.BUKKEN_NO = NM.BUKKEN_NO AND NM.SHUKKA_FLAG = BM.SHUKKA_FLAG");
            sb.ApdL(" WHERE 1 = 1");
            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND BM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            sb.ApdL(" ORDER BY BUKKEN_NAME");

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

    #region 出荷日指定物件一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 出荷日指定物件一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetNounyuBukkenData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("        BM.BUKKEN_NO");
            sb.ApdL("       ,BM.BUKKEN_NAME");
            sb.ApdL("FROM M_NONYUSAKI NM");
            sb.ApdL("LEFT JOIN M_BUKKEN BM ON BM.BUKKEN_NO = NM.BUKKEN_NO AND NM.SHUKKA_FLAG = BM.SHUKKA_FLAG");
            sb.ApdL("WHERE 1 = 1");
            //// 出荷日
            //if (!string.IsNullOrEmpty(cond.ShipDate))
            //{
            //    sb.ApdN("   AND NM.SHIP_DATE = ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
            //    paramCollection.Add(iNewParameter.NewDbParameter("SHIP_DATE", cond.ShipDate));
            //}
            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND NM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            //// 物件管理NO
            //if (!string.IsNullOrEmpty(cond.BukkenNO))
            //{
            //    sb.ApdN("   AND NM.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            //    paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            //}
            //// 納入先NO
            //if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            //{
            //    sb.ApdN("   AND NM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            //    paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            //}
            sb.ApdL(" ORDER BY BM.BUKKEN_NAME");

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

    #region 納入先一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 納入先一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetNonyusakiData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT  NM.SHIP");
            sb.ApdL("       ,NM.NONYUSAKI_CD");
            sb.ApdL("       ,NM.SHUKKA_FLAG");
            sb.ApdL("       ,BM.BUKKEN_NO");
            sb.ApdL("       ,BM.BUKKEN_NAME");
            sb.ApdL("FROM M_NONYUSAKI NM");
            sb.ApdL("LEFT JOIN M_BUKKEN BM ON BM.BUKKEN_NO = NM.BUKKEN_NO AND NM.SHUKKA_FLAG = BM.SHUKKA_FLAG");
            sb.ApdL("WHERE 1 = 1");
            //// 出荷日
            //if (!string.IsNullOrEmpty(cond.ShipDate))
            //{
            //    sb.ApdN("   AND NM.SHIP_DATE = ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
            //    paramCollection.Add(iNewParameter.NewDbParameter("SHIP_DATE", cond.ShipDate));
            //}
            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("   AND NM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            //// 物件管理NO
            //if (!string.IsNullOrEmpty(cond.BukkenNO))
            //{
            //    sb.ApdN("   AND NM.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            //    paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            //}
            //// 納入先NO
            //if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            //{
            //    sb.ApdN("   AND NM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            //    paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            //}
            sb.ApdL(" ORDER BY NM.SHIP");

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

    #region 名称マスタ取得
    /// --------------------------------------------------
    /// <summary>
    /// 名称マスタ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetSelectItemData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("  SELECT_GROUP_CD ");
            sb.ApdL("  , ITEM_NAME ");
            //            sb.ApdL("  , CREATE_DATE ");
            //            sb.ApdL("  , CREATE_USER_ID ");
            //            sb.ApdL("  , CREATE_USER_NAME ");
            //            sb.ApdL("  , UPDATE_DATE ");
            //            sb.ApdL("  , UPDATE_USER_ID ");
            //            sb.ApdL("  , UPDATE_USER_NAME ");
            //            sb.ApdL("  , MAINTE_DATE ");
            //            sb.ApdL("  , MAINTE_USER_ID ");
            //            sb.ApdL("  , MAINTE_USER_NAME ");
            //            sb.ApdL("  , VERSION ");
            sb.ApdL("FROM M_SELECT_ITEM");
            sb.ApdL("WHERE 1 = 1");
            // 選択グループCD
            if (!string.IsNullOrEmpty(cond.SelectGroupCD))
            {
                sb.ApdN("   AND SELECT_GROUP_CD = ").ApdN(this.BindPrefix).ApdL("SELECT_GROUP_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("SELECT_GROUP_CD", cond.SelectGroupCD));
            }
            sb.ApdL(" ORDER BY ITEM_NAME");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 指定出荷日納入情報一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 指定出荷日納入情報一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>T.Nakata 2018/11/07</create>
    /// <update>D.Okumura 2018/12/11</update>
    /// <update>K.Tsutsumi 2019/03/11 単位はmmではなくcmでよいそうです。</update>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
    /// <update>H.Tajimi 2020/04/27 NULLだと情報が欠落してしまうため修正</update>
    /// <update>2022/05/20 STEP14</update>
    /// --------------------------------------------------
    private DataSet GetNisugataFromNonyusaki(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            // 列の並びは画面のデータセットと一致させること
            sb.ApdL("SELECT ");
            sb.ApdL("	 CAST('" + KOKUNAI_GAI_FLAG.GAI_VALUE1 + "' AS NCHAR(1)) AS KOKUNAI_GAI_FLAG ");
            sb.ApdL("	,a.BUKKEN_NO ");
            sb.ApdL("	,CAST('" + CANCEL_FLAG.NORMAL_VALUE1 + "' AS NCHAR(1)) AS CANCEL_FLAG ");
            sb.ApdL("	,CAST('" + DOUKON_FLAG.OFF_VALUE1 + "' AS NCHAR(1)) AS DOUKON_FLAG ");
            sb.ApdL("	,a.SHUKKA_FLAG ");
            sb.ApdL("	,a.NONYUSAKI_CD ");

            sb.ApdL("	,NULL AS PACKING_NO ");
            //            sb.ApdL("	,MAX(a.CT_NO) OVER(PARTITION BY a.SHUKKA_FLAG,a.NONYUSAKI_CD) AS CT_QTY ");
            sb.ApdL("	,CASE WHEN a.CT_NO IS NULL THEN NULL ");
            sb.ApdL("		ELSE MAX(a.CT_NO) OVER(PARTITION BY a.SHUKKA_FLAG,a.NONYUSAKI_CD) ");
            sb.ApdL("	 END AS CT_QTY ");
            sb.ApdL("	,CAST(NULL AS NVARCHAR(15)) AS INVOICE_NO ");
            sb.ApdL("	,a.SHIP_DATE AS SYUKKA_DATE ");
            sb.ApdL("	,NULL AS HAKKO_FLAG ");
            sb.ApdL("	,CAST(NULL AS NCHAR(4)) AS UNSOKAISHA_CD ");
            sb.ApdL("	,NULL AS CONSIGN_CD ");
            sb.ApdL("	,NULL AS CONSIGN_ATTN ");
            sb.ApdL("	,NULL AS DELIVER_CD ");
            sb.ApdL("	,NULL AS DELIVER_ATTN ");
            sb.ApdL("	,NULL AS PACKING_MAIL_SUBJECT ");
            sb.ApdL("	,NULL AS PACKING_REV ");
            sb.ApdL("	,NULL AS VERSION ");

            sb.ApdL("	,NULL AS NO ");
            sb.ApdL("	,a.CT_NO AS CT_NO ");
            sb.ApdL("	,a.FORM_STYLE_FLAG ");
            sb.ApdL("	,a.SIZE_L ");
            sb.ApdL("	,a.SIZE_W ");
            sb.ApdL("	,a.SIZE_H ");
            sb.ApdL("	,a.GRWT ");
            sb.ApdL("	,CAST(NULL AS NVARCHAR(20)) AS PRODUCT_NAME ");
            sb.ApdL("	,CAST(NULL AS NVARCHAR(30)) AS ATTN ");
            sb.ApdL("	,CAST(NULL AS NVARCHAR(30)) AS NOTE ");
            sb.ApdL("	,a.PL_TYPE ");
            sb.ApdL("	,a.CASE_NO ");
            sb.ApdL("	,a.BOX_NO ");
            sb.ApdL("	,a.PALLET_NO ");
            sb.ApdL("	,CAST(NULL AS nvarchar(10)) AS ECS_NO"); //別途反映
            sb.ApdL("	,CAST(NULL AS NCHAR(6)) AS AR_NO ");
            sb.ApdL("	,CAST(NULL AS nvarchar(15)) AS SEIBAN_CODE "); //別途反映

            sb.ApdL("	,a.SHIP ");
            sb.ApdL("	,a.KONPO_NO ");
            sb.ApdL("	,'' AS SHIP_FROM_CD ");
            sb.ApdL("FROM (");

            //梱包情報あり
            sb.ApdL("SELECT ");
            sb.ApdL("	  w.*");
            sb.ApdL("	, m.BUKKEN_NO");
            sb.ApdL("	, m.SHIP");
            sb.ApdL("	, m.SHIP_DATE");
            sb.ApdL("	, ROW_NUMBER() OVER(");
            sb.ApdL("		PARTITION BY w.SHUKKA_FLAG,w.NONYUSAKI_CD");
            sb.ApdL("		ORDER BY w.SHUKKA_FLAG,w.NONYUSAKI_CD,w.CTTYPE,w.C_NO");
            sb.ApdL("		) AS CT_NO ");
            sb.ApdL("	FROM (");
            //木枠情報
            sb.ApdL("		SELECT");
            sb.ApdL("		        TSM.SHUKKA_FLAG");
            sb.ApdL("		      , TSM.NONYUSAKI_CD");
            sb.ApdL("		      , TKM.CASE_NO");
            sb.ApdL("		      , NULL AS PALLET_NO");
            sb.ApdL("		      , NULL AS BOX_NO");
            sb.ApdL("		      , CAST(TKM.CASE_NO AS NVARCHAR(6)) AS KONPO_NO");
            sb.ApdL("		      , TKM.CASE_NO AS C_NO");
            sb.ApdL("		      , CAST('" + PL_TYPE.CRATE_VALUE1 + "' AS NCHAR(2)) AS PL_TYPE");
            sb.ApdL("		      , CAST('" + FORM_STYLE_FLAG.FORM_0_VALUE1 + "' AS NCHAR(2)) AS FORM_STYLE_FLAG");
            sb.ApdL("		      , TKM.DIMENSION_L AS SIZE_L");
            sb.ApdL("		      , TKM.DIMENSION_W AS SIZE_W");
            sb.ApdL("		      , TKM.DIMENSION_H AS SIZE_H");
            sb.ApdL("		      , TKM.GROSS_W AS GRWT");
            sb.ApdL("		      , 1 AS CTTYPE");
            sb.ApdL("		FROM (");
            sb.ApdL("		    SELECT ");
            sb.ApdL("		            TSM.SHUKKA_FLAG");
            sb.ApdL("		          , TSM.NONYUSAKI_CD");
            sb.ApdL("		          , TSM.CASE_ID");
            sb.ApdL("		    FROM T_SHUKKA_MEISAI TSM ");
            sb.ApdL("		    INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD AND MN.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG AND MN.SHIP_DATE = ").ApdN(this.BindPrefix).ApdN("SHIP_DATE");
            sb.ApdL("		    WHERE");
            sb.ApdL("		            1 = 1");
            sb.ApdL("		        AND TSM.CASE_ID IS NOT NULL");
            sb.ApdL("		    GROUP BY");
            sb.ApdL("		            TSM.SHUKKA_FLAG");
            sb.ApdL("		          , TSM.NONYUSAKI_CD");
            sb.ApdL("		          , TSM.CASE_ID");
            sb.ApdL("		    ) TSM");
            sb.ApdL("		INNER JOIN T_KIWAKU_MEISAI TKM ON TKM.CASE_ID = TSM.CASE_ID");

            //パレット情報
            sb.ApdL("	UNION ALL");
            sb.ApdL("	   SELECT ");
            sb.ApdL("			  s.SHUKKA_FLAG");
            sb.ApdL("			, s.NONYUSAKI_CD");
            sb.ApdL("			, NULL AS CASE_NO");
            sb.ApdL("			, s.PALLET_NO");
            sb.ApdL("			, NULL AS BOX_NO");
            sb.ApdL("			, s.PALLET_NO AS KONPO_NO");
            sb.ApdL("			, NULL AS C_NO");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS PL_TYPE");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS FORM_STYLE_FLAG");
            sb.ApdL("			, NULL AS SIZE_L , NULL AS SIZE_W , NULL AS SIZE_H");
            sb.ApdL("			, NULL AS GRWT");
            sb.ApdL("			, 2 AS CTTYPE");
            sb.ApdL("		FROM T_SHUKKA_MEISAI AS s");
            sb.ApdL("	   WHERE KOJI_NO IS NULL");
            sb.ApdL("		 AND PALLET_NO IS NOT NULL");
            sb.ApdL("	GROUP BY SHUKKA_FLAG, NONYUSAKI_CD, PALLET_NO");
            //ボックス情報
            sb.ApdL("   UNION ALL ");
            sb.ApdL("	   SELECT ");
            sb.ApdL("			  s.SHUKKA_FLAG");
            sb.ApdL("			, s.NONYUSAKI_CD");
            sb.ApdL("			, NULL AS CASE_NO");
            sb.ApdL("			, NULL AS PALLET_NO");
            sb.ApdL("			, s.BOX_NO AS BOX_NO");
            sb.ApdL("			, s.BOX_NO AS KONPO_NO");
            sb.ApdL("			, NULL AS C_NO");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS PL_TYPE");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS FORM_STYLE_FLAG");
            sb.ApdL("			, NULL AS SIZE_L , NULL AS SIZE_W , NULL AS SIZE_H");
            sb.ApdL("			, NULL AS GRWT");
            sb.ApdL("			, 3 AS CTTYPE");
            sb.ApdL("		FROM T_SHUKKA_MEISAI AS s");
            sb.ApdL("	   WHERE PALLET_NO IS NULL");
            sb.ApdL("	     AND BOX_NO IS NOT NULL");
            sb.ApdL("	GROUP BY BOX_NO, SHUKKA_FLAG, NONYUSAKI_CD");

            sb.ApdL("	) AS w");
            sb.ApdL("	INNER JOIN M_NONYUSAKI AS m");
            sb.ApdL("		 ON w.NONYUSAKI_CD = m.NONYUSAKI_CD");
            sb.ApdL("		AND w.SHUKKA_FLAG = m.SHUKKA_FLAG");
            //絞り込みは必ずこの位置で行うこと
            sb.ApdN("	WHERE w.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("	 AND m.SHIP_DATE = ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
            if (!string.IsNullOrEmpty(cond.ShipFromCD))
            {
                sb.ApdN("	 AND m.SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("SHIP_FROM_CD", cond.ShipFromCD));
            }

            //梱包情報なし
            sb.ApdL("UNION ALL ");
            sb.ApdL("SELECT ");
            sb.ApdL("	  w.*");
            sb.ApdL("	, m.BUKKEN_NO");
            sb.ApdL("	, m.SHIP");
            sb.ApdL("	, m.SHIP_DATE");
            sb.ApdL("	, NULL AS CT_NO");
            sb.ApdL("	FROM (");

            //パレット情報&&ボックス情報なし
            sb.ApdL("	   SELECT ");
            sb.ApdL("			  s.SHUKKA_FLAG");
            sb.ApdL("			, s.NONYUSAKI_CD");
            sb.ApdL("			, NULL AS CASE_NO");
            sb.ApdL("			, NULL AS PALLET_NO");
            sb.ApdL("			, NULL AS BOX_NO");
            sb.ApdL("			, NULL AS KONPO_NO");
            sb.ApdL("			, NULL AS C_NO");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS PL_TYPE");
            sb.ApdL("			, CAST(NULL AS NVARCHAR(256)) AS FORM_STYLE_FLAG");
            sb.ApdL("			, NULL AS SIZE_L , NULL AS SIZE_W , NULL AS SIZE_H");
            sb.ApdL("			, NULL AS GRWT");
            sb.ApdL("			, NULL AS CTTYPE");
            sb.ApdL("		FROM T_SHUKKA_MEISAI AS s");
            sb.ApdL("	   WHERE PALLET_NO IS NULL");
            sb.ApdL("	     AND BOX_NO IS NULL");
            sb.ApdL("	     AND CASE_ID IS NULL");
            sb.ApdL("	GROUP BY BOX_NO, SHUKKA_FLAG, NONYUSAKI_CD");

            sb.ApdL("	) AS w");
            sb.ApdL("	INNER JOIN M_NONYUSAKI AS m");
            sb.ApdL("		 ON w.NONYUSAKI_CD = m.NONYUSAKI_CD");
            sb.ApdL("		AND w.SHUKKA_FLAG = m.SHUKKA_FLAG");
            //絞り込みは必ずこの位置で行うこと
            sb.ApdN("	WHERE w.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("	 AND m.SHIP_DATE = ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
            if (!string.IsNullOrEmpty(cond.ShipFromCD))
            {
                sb.ApdN("	 AND m.SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
            }

            sb.ApdL(") AS a");
            sb.ApdL("ORDER BY a.SHUKKA_FLAG, a.NONYUSAKI_CD, a.CTTYPE");

            // バインド設定
            paramCollection.Add(iNewParameter.NewDbParameter("SHIP_DATE", string.IsNullOrEmpty(cond.ShipDate) ? "" : cond.ShipDate));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", string.IsNullOrEmpty(cond.ShukkaFlag) ? "" : cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("PL_TYPE_GROUP_CD", PL_TYPE.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("PL_TYPE_ITEM_CD", PL_TYPE.CRATE_NAME));
            paramCollection.Add(iNewParameter.NewDbParameter("FORM_STYLE_FLAG_GROUP_CD", FORM_STYLE_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("FORM_STYLE_FLAG_ITEM_CD", FORM_STYLE_FLAG.FORM_0_NAME));
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));

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

    #region 荷姿情報一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿情報一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
    /// --------------------------------------------------
    private DataSet Sql_GetNisugataData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("	 UM.KOKUNAI_GAI_FLAG ");
            sb.ApdL("	,NM.BUKKEN_NO ");
            sb.ApdL("	,PMT.CANCEL_FLAG ");
            sb.ApdL("	,PMT.DOUKON_FLAG ");
            sb.ApdL("	,PMT.SHUKKA_FLAG ");
            sb.ApdL("	,PMT.NONYUSAKI_CD ");

            sb.ApdL("	,PT.PACKING_NO ");
            sb.ApdL("	,PT.CT_QTY ");
            sb.ApdL("	,PT.INVOICE_NO ");
            sb.ApdL("	,PT.SYUKKA_DATE ");
            sb.ApdL("	,PT.HAKKO_FLAG ");
            sb.ApdL("	,PT.UNSOKAISHA_CD ");
            sb.ApdL("	,PT.CONSIGN_CD ");
            sb.ApdL("	,PT.CONSIGN_ATTN ");
            sb.ApdL("	,PT.DELIVER_CD ");
            sb.ApdL("	,PT.DELIVER_ATTN ");
            sb.ApdL("	,PT.PACKING_MAIL_SUBJECT ");
            sb.ApdL("	,PT.PACKING_REV ");
            sb.ApdL("	,PT.VERSION ");
            sb.ApdL("	,PMT.NO ");
            sb.ApdL("	,PMT.CT_NO ");
            sb.ApdL("	,PMT.FORM_STYLE_FLAG ");
            sb.ApdL("	,PMT.SIZE_L ");
            sb.ApdL("	,PMT.SIZE_W ");
            sb.ApdL("	,PMT.SIZE_H ");
            sb.ApdL("	,PMT.GRWT ");
            sb.ApdL("	,PMT.PRODUCT_NAME ");
            sb.ApdL("	,PMT.ATTN ");
            sb.ApdL("	,PMT.NOTE ");
            sb.ApdL("	,PMT.PL_TYPE ");
            sb.ApdL("	,PMT.CASE_NO ");
            sb.ApdL("	,PMT.BOX_NO ");
            sb.ApdL("	,PMT.PALLET_NO ");
            sb.ApdL("	,PMT.ECS_NO ");
            sb.ApdL("	,PMT.AR_NO ");
            sb.ApdL("	,PMT.SEIBAN_CODE ");

            sb.ApdL("	,NM.SHIP ");
            sb.ApdL("	,IsNull(PMT.PALLET_NO, IsNull(PMT.BOX_NO,cast(PMT.CASE_NO AS nvarchar(6)))) AS KONPO_NO ");
            sb.ApdL("	,PT.SHIP_FROM_CD ");
            sb.ApdL("FROM ");
            sb.ApdL("	T_PACKING_MEISAI PMT ");
            sb.ApdL("	LEFT JOIN T_PACKING PT ON PMT.PACKING_NO = PT.PACKING_NO ");
            sb.ApdL("	LEFT JOIN M_NONYUSAKI NM ON NM.NONYUSAKI_CD = PMT.NONYUSAKI_CD AND NM.SHUKKA_FLAG = PMT.SHUKKA_FLAG ");
            sb.ApdL("	LEFT JOIN M_BUKKEN BM ON BM.BUKKEN_NO = NM.BUKKEN_NO AND BM.SHUKKA_FLAG = NM.SHUKKA_FLAG ");
            sb.ApdL("	LEFT JOIN M_UNSOKAISHA UM ON UM.UNSOKAISHA_NO = PT.UNSOKAISHA_CD ");
            sb.ApdL("	LEFT JOIN M_SHIP_FROM SFM ON SFM.SHIP_FROM_NO = PT.SHIP_FROM_CD ");
            sb.ApdL("WHERE ");
            sb.ApdL("	1=1");
            if (!string.IsNullOrEmpty(cond.ShipDate))
            {
                sb.ApdL("	AND PT.SYUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
                paramCollection.Add(iNewParameter.NewDbParameter("SHIP_DATE", cond.ShipDate));
            }
            if (!string.IsNullOrEmpty(cond.ShipFromCD))
            {
                sb.ApdN("	AND PT.SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("SHIP_FROM_CD", cond.ShipFromCD));
            }
            sb.ApdL("ORDER BY ");
            sb.ApdL("	 UM.KOKUNAI_GAI_FLAG ");
            if (!string.IsNullOrEmpty(cond.NisugataSortOrder))
            {
                if (cond.NisugataSortOrder == NISUGATA_HAKKO_SORT.SHIP_FROM_AND_INVOICE_VALUE1)
                {
                    sb.ApdL("	,SFM.DISP_NO");
                    sb.ApdL("	,SFM.SHIP_FROM_NO");
                    sb.ApdL("	,PT.INVOICE_NO ");
                }
                else
                {
                    sb.ApdL("	,PT.INVOICE_NO ");
                }
            }
            else
            {
                sb.ApdL("	,UM.UNSOKAISHA_NAME ");
                sb.ApdL("	,PT.INVOICE_NO ");
            }
            sb.ApdL("	,PMT.SHUKKA_FLAG ");
            sb.ApdL("	,PMT.NONYUSAKI_CD ");
            sb.ApdL("	,PMT.CT_NO ");
            sb.ApdL("	,PMT.NO ");

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

    #region 木枠情報取得
    /// --------------------------------------------------
    /// <summary>
    /// 木枠情報取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetKiwakuMeisai(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1 ");
            sb.ApdL("			TKM.CASE_NO");
            sb.ApdL("		  , TKM.DIMENSION_L");
            sb.ApdL("		  , TKM.DIMENSION_W");
            sb.ApdL("		  , TKM.DIMENSION_H");
            sb.ApdL("		  , TKM.GROSS_W ");
            sb.ApdL(" FROM");
            sb.ApdL(" 			T_SHUKKA_MEISAI TSM");
            sb.ApdL("INNER JOIN  T_KIWAKU TK ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("INNER JOIN  T_KIWAKU_MEISAI TKM ON TKM.KOJI_NO = TK.KOJI_NO");
            sb.ApdL("WHERE");
            sb.ApdL("        	1 = 1");
            sb.ApdL("    	AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG");
            sb.ApdL("    	AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdN("NONYUSAKI_CD");
            if (cond.ShukkaFlag == SHUKKA_FLAG.AR_VALUE1)
            {
                sb.ApdL("    	AND TSM.AR_NO = ").ApdN(this.BindPrefix).ApdN("AR_NO");
                paramCollection.Add(iNewParameter.NewDbParameter("AR_NO", cond.ARNo));

            }
            sb.ApdL("    	AND TKM.CASE_NO = ").ApdN(this.BindPrefix).ApdN("CASE_NO");

            // バインド設定
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("CASE_NO", cond.CaseNo));

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

    #region Boxリスト管理データ取得
    /// --------------------------------------------------
    /// <summary>
    /// Boxリスト管理データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetBoxListManage(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("        TSM.PALLET_NO");
            sb.ApdL("FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("INNER JOIN T_BOXLIST_MANAGE TBM ON TBM.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND TBM.NONYUSAKI_CD = TSM.NONYUSAKI_CD AND TBM.BOX_NO = TSM.BOX_NO");
            sb.ApdL("WHERE");
            sb.ApdL("        TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("    AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("    AND TSM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("BOX_NO", cond.BoxNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_BOXLIST_MANAGE.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region Palletリスト管理データ取得
    /// --------------------------------------------------
    /// <summary>
    /// Palletリスト管理データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataSet Sql_GetPalletListManage(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("        TSM.KOJI_NO");
            sb.ApdL("FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("INNER JOIN T_PALLETLIST_MANAGE TPM ON TPM.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND TPM.NONYUSAKI_CD = TSM.NONYUSAKI_CD AND TPM.PALLET_NO = TSM.PALLET_NO");
            sb.ApdL("WHERE");
            sb.ApdL("        TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("    AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("    AND TSM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParameter.NewDbParameter("PALLET_NO", cond.PalletNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_PALLETLIST_MANAGE.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 登録済み荷姿出荷明細情報取得
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿の木枠情報取得(最大1件)
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="caseNo">ケース番号</param>
    /// <returns>出荷明細テーブル(最大1件)</returns>
    /// <create>K.Tsutsumi 2019/03/14</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetShukkaMeisaiSampleForNisugataByCaseNo(DatabaseHelper dbHelper, string nonyusakiCd, string shukkaFlag, string caseNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("        TSM.SHUKKA_FLAG");
            sb.ApdL("      , TSM.NONYUSAKI_CD");
            sb.ApdL("      , TSM.SEIBAN");
            sb.ApdL("      , TSM.CODE");
            sb.ApdL("      , TSM.AREA");
            sb.ApdL("FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("INNER JOIN T_KIWAKU_MEISAI TKM ON TKM.CASE_ID = TSM.CASE_ID AND TKM.CASE_NO = ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            sb.ApdL("WHERE");
            sb.ApdL("        1 = 1");
            sb.ApdL("    AND TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("    AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("    AND TSM.SEIBAN IS NOT NULL");
            //sb.ApdL("    AND TSM.CODE IS NOT NULL");
            //sb.ApdL("    AND TSM.AREA IS NOT NULL");
            sb.ApdL("ORDER BY");
            sb.ApdL("    TSM.TAG_NO");

            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_KIWAKU.NONYUSAKI_CD, nonyusakiCd));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_KIWAKU.SHUKKA_FLAG, shukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_KIWAKU_MEISAI.CASE_NO, caseNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception)
        {
            throw;
        }

    }
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿のパレット情報取得(最大1件)
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="palletNo">パレット番号</param>
    /// <returns>出荷明細テーブル(最大1件)</returns>
    /// <create>D.Okumura 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetShukkaMeisaiSampleForNisugataByPallet(DatabaseHelper dbHelper, string nonyusakiCd, string shukkaFlag, string palletNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("	 SHUKKA_FLAG, NONYUSAKI_CD, SEIBAN, CODE, AREA ");
            sb.ApdL("FROM ");
            sb.ApdL("	T_SHUKKA_MEISAI ");
            sb.ApdL("WHERE ");
            sb.ApdN("	    NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            sb.ApdN("	AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
            sb.ApdN("	AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.PALLET_NO);
            sb.ApdL("   AND SEIBAN IS NOT NULL");
            sb.ApdL("   AND CODE IS NOT NULL");
            sb.ApdL("   AND AREA IS NOT NULL");
            sb.ApdL("ORDER BY ");
            sb.ApdL("	 TAG_NO ");

            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, nonyusakiCd));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, shukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.PALLET_NO, palletNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception)
        {
            throw;
        }

    }
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿のボックス情報取得(最大1件)
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="shukkaFlag">出荷区分</param>
    /// <param name="boxNo">ボックス番号</param>
    /// <returns>出荷明細テーブル(最大1件)</returns>
    /// <create>D.Okumura 2018/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetShukkaMeisaiSampleForNisugataByBox(DatabaseHelper dbHelper, string nonyusakiCd, string shukkaFlag, string boxNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1");
            sb.ApdL("	 SHUKKA_FLAG, NONYUSAKI_CD, SEIBAN, CODE, AREA ");
            sb.ApdL("FROM ");
            sb.ApdL("	T_SHUKKA_MEISAI ");
            sb.ApdL("WHERE ");
            sb.ApdN("	    NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
            sb.ApdN("	AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
            sb.ApdN("	AND BOX_NO = ").ApdN(this.BindPrefix).ApdL(Def_T_SHUKKA_MEISAI.BOX_NO);
            sb.ApdL("   AND SEIBAN IS NOT NULL");
            sb.ApdL("   AND CODE IS NOT NULL");
            sb.ApdL("   AND AREA IS NOT NULL");
            sb.ApdL("ORDER BY ");
            sb.ApdL("	 TAG_NO ");

            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, nonyusakiCd));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, shukkaFlag));
            paramCollection.Add(iNewParameter.NewDbParameter(Def_T_SHUKKA_MEISAI.BOX_NO, boxNo));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);

            return dt;

        }
        catch (Exception)
        {
            throw;
        }

    }

    #endregion

    #region 手配明細情報取得(最大1件)

    /// =====================================================================
    /// <summary>
    /// 手配明細情報取得(最大1件)
    /// </summary>
    /// <param name="dbHelper">ヘルパ</param>
    /// <param name="BoxNo">ボックス番号</param>
    /// <param name="PalletNo">パレット番号</param>
    /// <param name="BukkenNo">物件番号</param>
    /// <returns>手配明細テーブル(最大1件)</returns>
    /// <create>2022/05/10 STEP14</create>
    /// <update></update>
    /// =====================================================================
    private DataTable GetTehaiMeisaiSampleForNisugataByESCNo(DatabaseHelper dbHelper, string BoxNo, string PalletNo, string BukkenNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT TOP 1 MAIN.*");
            sb.ApdL("  FROM (");
            sb.ApdL("SELECT");
            sb.ApdL("       TTM.TEHAI_RENKEI_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , ME.SEIBAN");
            sb.ApdL("     , ME.CODE");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , ME.ECS_QUOTA");
            sb.ApdL("     , NULL AS BOX_NO");
            sb.ApdL("     , NULL AS PALLET_NO");
            sb.ApdL("");
            sb.ApdL("  FROM ");
            sb.ApdL("        T_TEHAI_MEISAI TTM");
            sb.ApdL(" INNER JOIN M_ECS ME ON TTM.ECS_NO = ME.ECS_NO AND TTM.ECS_QUOTA = ME.ECS_QUOTA");
            sb.ApdL(" INNER JOIN M_PROJECT MP ON ME.PROJECT_NO = MP.PROJECT_NO");
            sb.ApdL(" INNER JOIN M_BUKKEN BM ON BM.BUKKEN_NAME = MP.BUKKEN_NAME");
            sb.ApdL("  LEFT JOIN (SELECT TEHAI_RENKEI_NO, SUM(NUM) AS CNT FROM T_SHUKKA_MEISAI WHERE TEHAI_RENKEI_NO IS NOT NULL GROUP BY TEHAI_RENKEI_NO) AS WSM1 ON WSM1.TEHAI_RENKEI_NO = TTM.TEHAI_RENKEI_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       TTM.TEHAI_QTY - IsNull(WSM1.CNT, 0) > 0 ");
            sb.ApdN("   AND BM.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("");
            sb.ApdL(" UNION ALL");
            sb.ApdL("");
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , MP.BUKKEN_NAME");
            sb.ApdL("     , TTM.SYUKKA_SAKI");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , ME.ECS_NO");
            sb.ApdL("     , TTM.ECS_QUOTA");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL(" INNER JOIN M_ECS ME ON ME.ECS_NO = TTM.ECS_NO AND ME.ECS_QUOTA = TTM.ECS_QUOTA");
            sb.ApdL(" INNER JOIN M_PROJECT MP ON MP.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdL(" INNER JOIN M_BUKKEN BM ON BM.BUKKEN_NAME = MP.BUKKEN_NAME");
            sb.ApdL(" WHERE (TSM.TEHAI_RENKEI_NO IS NOT NULL AND LTRIM(RTRIM(TSM.TEHAI_RENKEI_NO)) != '')");
            if (!string.IsNullOrEmpty(BoxNo))
            {
                sb.ApdN("   AND TSM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            }
            if (!string.IsNullOrEmpty(PalletNo))
            {
                sb.ApdN("   AND TSM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            }
            sb.ApdN("   AND BM.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("  ) AS MAIN");
            if (!string.IsNullOrEmpty(BoxNo))
            {
                sb.ApdL("ORDER BY");
                sb.ApdL("       MAIN.BOX_NO DESC");
            }
            if (!string.IsNullOrEmpty(PalletNo))
            {
                sb.ApdL("ORDER BY");
                sb.ApdL("       MAIN.PALLET_NO DESC");
            }

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("BUKKEN_NO", BukkenNo));
            if (!string.IsNullOrEmpty(BoxNo))
            {
                paramCollection.Add(iNewParameter.NewDbParameter("BOX_NO", BoxNo));
            }
            if (!string.IsNullOrEmpty(PalletNo))
            {
                paramCollection.Add(iNewParameter.NewDbParameter("PALLET_NO", PalletNo));
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

    #region 荷姿データ取得＆ロック
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="shukkaDate">出荷日</param>
    /// <param name="shipFromCD">出荷元CD</param>
    /// <returns>荷姿データ</returns>
    /// <create>T.Nakata 2018/11/26</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元追加</update>
    /// <update>H.Tajimi 2020/05/26 検索条件の出荷元に「すべて」が選択されていた場合にSQLエラーとなる問題を修正</update>
    /// --------------------------------------------------
    private DataTable LockNisugataData(DatabaseHelper dbHelper, string shukkaDate, string shipFromCD)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("	PACKING_NO ");
            sb.ApdL("	,INVOICE_NO ");
            sb.ApdL("	,SYUKKA_DATE ");
            sb.ApdL("	,VERSION ");
            sb.ApdL("FROM ");
            sb.ApdL("	T_PACKING ");
            sb.ApdL("WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL("WHERE ");
            sb.ApdL("	SYUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SYUKKA_DATE");
            if (!string.IsNullOrEmpty(shipFromCD))
            {
                sb.ApdL("	AND SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_CD", shipFromCD));
            }
            paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_DATE", shukkaDate));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            dt.TableName = Def_T_PACKING.Name;

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 荷姿表Excel出力データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿表Excel出力データ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元、並び順追加</update>
    /// <update>K.Harada 2022/03/07 EXCEL出力列変更</update>
    /// --------------------------------------------------
    public DataSet GetNisugataExcelData(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("        0 AS ROW_INDEX");
            sb.ApdL("      , T_PACKING.PACKING_NO");
            sb.ApdL("      , T_PACKING_MEISAI.NOTE");
            sb.ApdL("      , T_PACKING_MEISAI.NO");
            sb.ApdL("      , M_UNSOKAISHA.UNSOKAISHA_NAME");
            sb.ApdL("      , T_PACKING.INVOICE_NO");
            sb.ApdL("      , M_NONYUSAKI.NONYUSAKI_NAME");
            sb.ApdL("      , T_PACKING_MEISAI.SEIBAN_CODE");
            sb.ApdN("      , CASE WHEN T_PACKING_MEISAI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("             THEN T_PACKING_MEISAI.AR_NO");
            sb.ApdL("             ELSE M_NONYUSAKI.SHIP");
            sb.ApdL("         END AS AR_NO");
            sb.ApdL("      , T_PACKING_MEISAI.ECS_NO");
            sb.ApdL("      , T_PACKING_MEISAI.ATTN");
            sb.ApdL("      , CASE WHEN T_PACKING_MEISAI.BOX_NO IS NOT NULL");
            sb.ApdL("             THEN 1");
            sb.ApdL("         END AS CARTON_QTY");
            sb.ApdL("      , CASE WHEN T_PACKING.CT_QTY > 1");
            sb.ApdL("             THEN CONVERT(NVARCHAR, T_PACKING_MEISAI.NO) + '/' + CONVERT(NVARCHAR, T_PACKING.CT_QTY)");
            sb.ApdL("             ELSE CONVERT(NVARCHAR, T_PACKING.CT_QTY)");
            sb.ApdL("         END AS CARTON_NO");
            sb.ApdL("      , CASE WHEN T_PACKING_MEISAI.PALLET_NO IS NOT NULL");
            sb.ApdL("             THEN '1(' + COM1.ITEM_NAME + ')'");
            sb.ApdL("         END AS PALLET_QTY");
            sb.ApdL("      , ISNULL(  T_PACKING_MEISAI.PALLET_NO");
            sb.ApdL("               , ISNULL(  T_PACKING_MEISAI.BOX_NO");
            sb.ApdL("                        , CAST( T_PACKING_MEISAI.CASE_NO AS NVARCHAR(6) )");
            sb.ApdL("                       )");
            sb.ApdL("              ) AS KONPO_NO");
            sb.ApdL("      , T_PACKING_MEISAI.SIZE_L");
            sb.ApdL("      , T_PACKING_MEISAI.SIZE_W");
            sb.ApdL("      , T_PACKING_MEISAI.SIZE_H");
            sb.ApdL("      , T_PACKING_MEISAI.GRWT AS GRWT");
            sb.ApdL("      , T_PACKING_MEISAI.PRODUCT_NAME");
            sb.ApdL("      , T_PACKING.PACKING_MAIL_SUBJECT");
            sb.ApdL("      , T_PACKING.PACKING_REV");
            sb.ApdL("      , T_PACKING.SYUKKA_DATE");
            sb.ApdL("      , M_SHIP_FROM.SHIP_FROM_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("        T_PACKING");
            sb.ApdL(" INNER JOIN T_PACKING_MEISAI");
            sb.ApdL("         ON T_PACKING_MEISAI.PACKING_NO = T_PACKING.PACKING_NO");
            sb.ApdL("  LEFT JOIN M_UNSOKAISHA");
            sb.ApdL("         ON M_UNSOKAISHA.UNSOKAISHA_NO = T_PACKING.UNSOKAISHA_CD");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI");
            sb.ApdL("         ON M_NONYUSAKI.SHUKKA_FLAG = T_PACKING_MEISAI.SHUKKA_FLAG");
            sb.ApdL("        AND M_NONYUSAKI.NONYUSAKI_CD = T_PACKING_MEISAI.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_SHIP_FROM");
            sb.ApdL("         ON M_SHIP_FROM.SHIP_FROM_NO = T_PACKING.SHIP_FROM_CD");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_PL_TYPE");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("        AND COM1.VALUE1 = T_PACKING_MEISAI.PL_TYPE");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("         ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_FORM_STYLE_FLAG");
            sb.ApdN("        AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("        AND COM2.VALUE1 = T_PACKING_MEISAI.FORM_STYLE_FLAG");
            sb.ApdN("        AND REPLACE(COM2.VALUE2, ").ApdN(this.BindPrefix).ApdN("SPLIT_SIZE_CHAR").ApdL(", '') <> ''");
            sb.ApdL(" WHERE");
            sb.ApdL("        1 = 1");
            if (!string.IsNullOrEmpty(cond.ShipDate))
            {
                sb.ApdN("    AND T_PACKING.SYUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SYUKKA_DATE");
                paramCollection.Add(iNewParameter.NewDbParameter("SYUKKA_DATE", cond.ShipDate));
            }
            if (!string.IsNullOrEmpty(cond.ShipFromCD))
            {
                sb.ApdN("    AND T_PACKING.SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
                paramCollection.Add(iNewParameter.NewDbParameter("SHIP_FROM_CD", cond.ShipFromCD));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("        M_UNSOKAISHA.KOKUNAI_GAI_FLAG");
            if (!string.IsNullOrEmpty(cond.NisugataSortOrder))
            {
                if (cond.NisugataSortOrder == NISUGATA_HAKKO_SORT.SHIP_FROM_AND_INVOICE_VALUE1)
                {
                    sb.ApdL("	   , M_SHIP_FROM.DISP_NO");
                    sb.ApdL("	   , M_SHIP_FROM.SHIP_FROM_NO");
                    sb.ApdL("	   , T_PACKING.INVOICE_NO ");
                }
                else
                {
                    sb.ApdL("	   , T_PACKING.INVOICE_NO ");
                }
            }
            else
            {
                sb.ApdL("      , M_UNSOKAISHA.UNSOKAISHA_NAME");
                sb.ApdL("      , T_PACKING.INVOICE_NO");
            }
            sb.ApdL("      , T_PACKING_MEISAI.SHUKKA_FLAG");
            sb.ApdL("      , M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("      , T_PACKING_MEISAI.CT_NO");
            sb.ApdL("      , T_PACKING_MEISAI.NO");

            // バインド変数
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", SHUKKA_FLAG.AR_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("GC_PL_TYPE", PL_TYPE.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParameter.NewDbParameter("GC_FORM_STYLE_FLAG", FORM_STYLE_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("TEIKEIGAI_VALUE", FORM_STYLE_FLAG.FORM_0_VALUE1));
            paramCollection.Add(iNewParameter.NewDbParameter("SPLIT_SIZE_CHAR", ComDefine.SPLIT_SIZE_CHAR));
            paramCollection.Add(iNewParameter.NewDbParameter("GRWT_UNIT", ComDefine.GRWT_UNIT));

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

    #region 荷姿ロック(梱包No)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿ロック(梱包No)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">DataTable</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockPackingOnPackingNo(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_PACKING.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       PACKING_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PACKING_MAIL_SUBJECT AS PACKING_MAIL_SUBJECT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PACKING_REV AS PACKING_REV");
            sb.ApdL("  FROM");
            sb.ApdL("       T_PACKING");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            var lstPackingNo = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                // 明細ベースになっており同一PACKING_NOが複数存在するため、同一PACKING_NOは一度だけ
                var packingNo = ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_NO);
                if (lstPackingNo.Contains(packingNo))
                {
                    continue;
                }
                lstPackingNo.Add(packingNo);

                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", packingNo));
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_MAIL_SUBJECT", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_MAIL_SUBJECT)));
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_REV", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_REV)));

                // SQL実行
                var dtTmp = new DataTable();
                dbHelper.Fill(sb.ToString(), paramCollection, dtTmp);
                if (dtTmp.Rows.Count == 1)
                {
                    ret.Merge(dtTmp);
                }
                else
                {
                    return ret.Clone();
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

    #endregion

    #region INSERT
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿テーブル追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元追加</update>
    /// --------------------------------------------------
    private int InsNisugataExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond, string PackingNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_PACKING ");
            sb.ApdL("(");
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
            sb.ApdL("  , CREATE_DATE ");
            sb.ApdL("  , CREATE_USER_ID ");
            sb.ApdL("  , CREATE_USER_NAME ");
            sb.ApdL("  , UPDATE_DATE ");
            sb.ApdL("  , UPDATE_USER_ID ");
            sb.ApdL("  , UPDATE_USER_NAME ");
            sb.ApdL("  , VERSION ");
            sb.ApdL("  , SHIP_FROM_CD ");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CT_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("INVOICE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SYUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HAKKO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CONSIGN_ATTN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DELIVER_ATTN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PACKING_MAIL_SUBJECT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PACKING_REV");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("CT_QTY", ComFunc.GetFldObject(dr, Def_T_PACKING.CT_QTY, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("INVOICE_NO", ComFunc.GetFldObject(dr, Def_T_PACKING.INVOICE_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_PACKING.SYUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HAKKO_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING.HAKKO_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.UNSOKAISHA_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_MAIL_SUBJECT", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_MAIL_SUBJECT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_REV", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_REV, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.SHIP_FROM_CD, DBNull.Value)));

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
    /// 荷姿明細追加
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    private int InsNisugataMeisaiExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond, string PackingNo)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_PACKING_MEISAI ");
            sb.ApdL("(");
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
            sb.ApdL("  , CREATE_DATE ");
            sb.ApdL("  , CREATE_USER_ID ");
            sb.ApdL("  , CREATE_USER_NAME ");
            sb.ApdL("  , UPDATE_DATE ");
            sb.ApdL("  , UPDATE_USER_ID ");
            sb.ApdL("  , UPDATE_USER_NAME ");
            sb.ApdL("  , VERSION ");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("PACKING_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CT_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CANCEL_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AR_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ECS_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SEIBAN_CODE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DOUKON_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FORM_STYLE_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SIZE_L");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SIZE_W");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SIZE_H");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GRWT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PRODUCT_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ATTN");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NOTE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PL_TYPE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", PackingNo));
            paramCollection.Add(iNewParam.NewDbParameter("NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CT_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.CT_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CANCEL_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.CANCEL_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.SHUKKA_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.NONYUSAKI_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("AR_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.AR_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ECS_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.ECS_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SEIBAN_CODE", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.SEIBAN_CODE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DOUKON_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.DOUKON_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("FORM_STYLE_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.FORM_STYLE_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SIZE_L", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.SIZE_L, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SIZE_W", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.SIZE_W, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SIZE_H", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.SIZE_H, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("GRWT", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.GRWT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PRODUCT_NAME", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.PRODUCT_NAME, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("NOTE", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.NOTE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PL_TYPE", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.PL_TYPE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.CASE_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.PALLET_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.BOX_NO, DBNull.Value)));
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
    /// 荷姿テーブル更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update>H.Tajimi 2020/04/14 検索条件に出荷元追加</update>
    /// --------------------------------------------------
    private int UpdNisugataExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond)
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
            sb.ApdN("       CT_QTY = ").ApdN(this.BindPrefix).ApdL("CT_QTY");
            sb.ApdN("     , INVOICE_NO = ").ApdN(this.BindPrefix).ApdL("INVOICE_NO");
            sb.ApdN("     , SYUKKA_DATE = ").ApdN(this.BindPrefix).ApdL("SYUKKA_DATE");
            sb.ApdN("     , HAKKO_FLAG = ").ApdN(this.BindPrefix).ApdL("HAKKO_FLAG");
            sb.ApdN("     , UNSOKAISHA_CD = ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_CD");
            sb.ApdN("     , CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL("CONSIGN_CD");
            sb.ApdN("     , CONSIGN_ATTN = ").ApdN(this.BindPrefix).ApdL("CONSIGN_ATTN");
            sb.ApdN("     , DELIVER_CD = ").ApdN(this.BindPrefix).ApdL("DELIVER_CD");
            sb.ApdN("     , DELIVER_ATTN = ").ApdN(this.BindPrefix).ApdL("DELIVER_ATTN");
            sb.ApdN("     , PACKING_MAIL_SUBJECT = ").ApdN(this.BindPrefix).ApdL("PACKING_MAIL_SUBJECT");
            sb.ApdN("     , PACKING_REV = ").ApdN(this.BindPrefix).ApdL("PACKING_REV");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , SHIP_FROM_CD = ").ApdN(this.BindPrefix).ApdL("SHIP_FROM_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CT_QTY", ComFunc.GetFldObject(dr, Def_T_PACKING.CT_QTY, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("INVOICE_NO", ComFunc.GetFldObject(dr, Def_T_PACKING.INVOICE_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SYUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_PACKING.SYUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("HAKKO_FLAG", ComFunc.GetFldObject(dr, Def_T_PACKING.HAKKO_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.UNSOKAISHA_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CONSIGN_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.CONSIGN_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVER_ATTN", ComFunc.GetFldObject(dr, Def_T_PACKING.DELIVER_ATTN, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_MAIL_SUBJECT", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_MAIL_SUBJECT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_REV", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_REV, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM_CD", ComFunc.GetFldObject(dr, Def_T_PACKING.SHIP_FROM_CD, DBNull.Value)));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);
            return record;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region 荷姿更新(Excel出力用)

    /// --------------------------------------------------
    /// <summary>
    /// 荷姿更新(Excel出力用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    internal int UpdPackingForExcelData(DatabaseHelper dbHelper, CondS01 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PACKING");
            sb.ApdL("SET");
            sb.ApdN("       PACKING_MAIL_SUBJECT = ").ApdN(this.BindPrefix).ApdL("PACKING_MAIL_SUBJECT");
            sb.ApdN("     , PACKING_REV = ").ApdN(this.BindPrefix).ApdL("PACKING_REV");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_MAIL_SUBJECT", ComFunc.GetFld(dr, Def_T_PACKING.PACKING_MAIL_SUBJECT)));
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_REV", ComFunc.GetFld(dr, Def_T_PACKING.PACKING_REV)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", ComFunc.GetFld(dr, Def_T_PACKING.PACKING_NO)));

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
    /// --------------------------------------------------
    /// <summary>
    /// 荷姿テーブル削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">データロウ</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelNisugataExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_PACKING");
            sb.ApdL(" WHERE ");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", ComFunc.GetFldObject(dr, Def_T_PACKING.PACKING_NO, DBNull.Value)));

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
    /// 荷姿明細削除
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dr"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Nakata 2018/12/03</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelNisugataMeisaiExec(DatabaseHelper dbHelper, DataRow dr, CondS01 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_PACKING_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdN("       PACKING_NO = ").ApdN(this.BindPrefix).ApdL("PACKING_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PACKING_NO", ComFunc.GetFldObject(dr, Def_T_PACKING_MEISAI.PACKING_NO, DBNull.Value)));

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

    #region S0100060:Mail送信履歴

    #region 制御

    #endregion

    #region SELECT

    #region Mail送信履歴取得
    /// --------------------------------------------------
    /// <summary>
    /// Mail送信履歴取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">S01用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Gwon 2023/08/17</create>
    /// <update>J.Chen 2024/01/16 運賃梱包製番を条件から外す</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMailSousinRireki(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection pc = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            //SQL文追加
            sb.ApdL("SELECT");
            sb.ApdL("       MSR.BUKKEN_REV");
            sb.ApdL("     , MSR.ASSIGN_COMMENT");
            sb.ApdL("     , MSR.UPDATE_DATE");
            sb.ApdL("     , M_USER.USER_NAME");
            sb.ApdL("  FROM");
            sb.ApdL("       M_MAIL_SEND_RIREKI MSR");
            sb.ApdL(" INNER JOIN M_USER");
            sb.ApdL("    ON M_USER.USER_ID = MSR.UPDATE_USER_ID");
            sb.ApdL(" WHERE MSR.BUKKEN_NO = @BUKKEN_NO");
            sb.ApdL("   AND MSR.CONSIGN_CD = @CONSIGN_CD");

            sb.ApdL(" ORDER BY");
            sb.ApdL("       MSR.UPDATE_DATE");
            sb.ApdL("       DESC");

            //パラメータに値代入
            pc.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNO));
            pc.Add(iNewParam.NewDbParameter("CONSIGN_CD", cond.ConsignCD));

            // SQL実行
            dbHelper.Fill(sb.ToString(), pc, ds, Def_M_MAIL_SEND_RIREKI.Name);

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

    #region S0100070:出荷計画照会

    #region 制御

    #region 現場用ステータス更新
    /// --------------------------------------------------
    /// <summary>
    /// 現場用ステータス更新
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
    public bool UpdNonyusakiForGenbayo(DatabaseHelper dbHelper, CondS01 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            // ロック
            DataTable dtNonyusakiList = GetAndLockNonyusakiList(dbHelper, cond, ds.Tables[ComDefine.DTTBL_UPDATE], true);
            if ((dtNonyusakiList == null)
                || (dtNonyusakiList.Rows.Count == 0)
                || (ds.Tables[ComDefine.DTTBL_UPDATE].Rows.Count != dtNonyusakiList.Rows.Count))
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // バージョンチェック
            int index;
            int[] notFoundIndex;
            index = this.CheckSameData(ds.Tables[ComDefine.DTTBL_UPDATE], dtNonyusakiList, out notFoundIndex, Def_M_NONYUSAKI.VERSION, Def_M_NONYUSAKI.NONYUSAKI_CD);
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
            this.UpdNonyusakiGenbaStatus(dbHelper, cond, ds.Tables[ComDefine.DTTBL_UPDATE]);

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
    private void SetupWherePhraseNonyusakiCd(StringBuilder sb, INewDbParameterBasic iNewParam, DbParamCollection paramCollection, DataTable dt, string tblAlias)
    {
        sb.ApdL(" (");
        sb.ApdN("   ");
        if (!string.IsNullOrEmpty(tblAlias))
            sb.ApdN(tblAlias).ApdN(".");
        sb.ApdL("NONYUSAKI_CD IN (");

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

                    sb.ApdL("NONYUSAKI_CD IN (");
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

            sb.ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD" + i);
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD" + i, ComFunc.GetFldObject(dr, Def_M_NONYUSAKI.NONYUSAKI_CD)));
            i++;
        }
        sb.ApdL("   )");
        sb.ApdL(" )");
    }
    #endregion

    #endregion

    #region SELECT

    #region 製番一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 製番一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetSeiban(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT LTRIM(RTRIM(value)) AS SEIBAN");
            sb.ApdL("FROM (");
            sb.ApdL("    SELECT CAST('<M>' + REPLACE(REPLACE(SEIBAN, ',', ' '), ' ', '</M><M>') + '</M>' AS XML) AS xmlData");
            sb.ApdL("    FROM M_NONYUSAKI");
            sb.ApdL(") x");
            sb.ApdL("CROSS APPLY (");
            sb.ApdL("    SELECT N.value('.', 'VARCHAR(500)') AS value");
            sb.ApdL("    FROM x.xmlData.nodes('M') AS T(N)");
            sb.ApdL(") y");
            sb.ApdL("WHERE LTRIM(RTRIM(value)) <> ''");
            sb.ApdL("ORDER BY SEIBAN;");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 機種一覧取得
    /// --------------------------------------------------
    /// <summary>
    /// 機種一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKishu(DatabaseHelper dbHelper)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ITEM_NAME");
            sb.ApdL("FROM M_SELECT_ITEM");
            sb.ApdL("WHERE SELECT_GROUP_CD = '00'");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_SELECT_ITEM.Name);

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region 出荷計画一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷計画一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">納入先マスタ用コンディション</param>
    /// <returns>DataSet</returns>
    /// <create>J.Chen 2023/08/25</create>
    /// <update>J.Chen 2024/10/03 現場用ステータス更新欄追加</update>
    /// <update></update>
    /// <remarks>注意：SELECT順番は出荷計画照会の列インデックスに影響しているため</remarks>
    /// <remarks>　　　項目を追加する際は下から追加するようにしてください。</remarks>
    /// --------------------------------------------------
    public DataSet GetNonyusakiForShokai(DatabaseHelper dbHelper, CondS01 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // 物件no取得
            var shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            var dtBukken = this.GetBukken(dbHelper, shukkaFlag, cond.BukkenName);
            var bukkenNo = ComFunc.GetFld(dtBukken, 0, Def_M_BUKKEN.BUKKEN_NO);
            dtBukken.TableName = Def_M_BUKKEN.Name;
            ds.Tables.Add(dtBukken);

            // SQL文
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("       MNS.NONYUSAKI_NAME AS BUKKEN_NAME ");
            sb.ApdL("     , MNS.SHIP_SEIBAN ");
            sb.ApdL("     , MNS.SHIP ");
            sb.ApdL("     , CASE MNS.ESTIMATE_FLAG "); // CASE文を開始
            if (cond.LoginInfo.Language == "US")
            {
                sb.ApdL("           WHEN 0 THEN 'Non-commercial' "); // ESTIMATE_FLAGが0の時は'無償'
                sb.ApdL("           WHEN 1 THEN 'Commercial' "); // ESTIMATE_FLAGが1の時は'有償'
            }
            else
            {
                sb.ApdL("           WHEN 0 THEN '無償' "); // ESTIMATE_FLAGが0の時は'無償'
                sb.ApdL("           WHEN 1 THEN '有償' "); // ESTIMATE_FLAGが1の時は'有償'
            }
            sb.ApdL("       END AS ESTIMATE_FLAG_NAME ");
            sb.ApdL("     , MNS.TRANSPORT_FLAG ");
            sb.ApdL("     , MNS.SHIP_FROM ");
            sb.ApdL("     , MNS.SHIP_TO ");
            sb.ApdL("     , MNS.SHIP_DATE ");
            sb.ApdL("     , MNS.SEIBAN ");
            sb.ApdL("     , MNS.KISHU ");
            sb.ApdL("     , MNS.NAIYO ");
            sb.ApdL("     , MNS.TOUCHAKUYOTEI_DATE ");
            sb.ApdL("     , MNS.KIKAI_PARTS ");
            sb.ApdL("     , MNS.SEIGYO_PARTS ");
            sb.ApdL("     , MNS.BIKO ");
            sb.ApdL("     , CASE WHEN MNS.SYORI_FLAG = 9 THEN NULL ELSE TSM_COUNT.TAG_NUM END AS TAG_NUM ");
            sb.ApdL("     , CASE WHEN MNS.SYORI_FLAG = 9 THEN 'Cancel' ELSE COM.ITEM_NAME END AS TAG_STATUS ");
            sb.ApdL("     , MNS.GENBA_YO_STATUS_FLAG ");
            sb.ApdL("     , MNS.GENBA_YO_BUTSURYO ");
            sb.ApdL("     , CASE ");
            sb.ApdL("           WHEN MNS.SYORI_FLAG IS NULL ");
            sb.ApdL("           OR MNS.SYORI_FLAG = '1' THEN 0 ");
            sb.ApdL("           ELSE MNS.SYORI_FLAG ");
            sb.ApdL("       END AS SYORI_FLAG ");
            sb.ApdL("     , MJF.MIN_JYOTAI_FLAG ");
            sb.ApdL("     , MNS.CONSIGN_CD ");
            sb.ApdL("     , CONVERT(NCHAR(27), MNS.VERSION, 121) AS VERSION ");
            sb.ApdL("     , MNS.ESTIMATE_FLAG ");
            sb.ApdL("     , MNS.SHIP_FROM_CD ");
            sb.ApdL("     , MNS.NONYUSAKI_CD ");
            sb.ApdL("     , MC.NAME ");
            sb.ApdL("     , COM2.ITEM_NAME AS GENBA_YO_STATUS_NAME ");

            sb.ApdL("FROM ");
            sb.ApdL("       M_NONYUSAKI AS MNS ");
            sb.ApdL("LEFT JOIN ( ");
            sb.ApdL("    SELECT ");
            sb.ApdL("        TSM.NONYUSAKI_CD, ");
            sb.ApdL("        MIN(CASE WHEN TSM.JYOTAI_FLAG = '8' THEN '0' ELSE TSM.JYOTAI_FLAG END) AS MIN_JYOTAI_FLAG ");
            sb.ApdL("    FROM T_SHUKKA_MEISAI TSM ");
            sb.ApdL("    GROUP BY TSM.NONYUSAKI_CD ");
            sb.ApdL(") AS MJF ");
            sb.ApdL("       ON MNS.NONYUSAKI_CD = MJF.NONYUSAKI_CD ");
            sb.ApdL("LEFT JOIN ( ");
            sb.ApdL("    SELECT ");
            sb.ApdL("        NONYUSAKI_CD, ");
            sb.ApdL("        COUNT(CASE WHEN TAG_NO IS NOT NULL AND TAG_NO <> '' THEN 1 ELSE NULL END) AS TAG_NUM ");
            sb.ApdL("    FROM T_SHUKKA_MEISAI ");
            sb.ApdL("    GROUP BY NONYUSAKI_CD ");
            sb.ApdL(") AS TSM_COUNT ");
            sb.ApdL("       ON MNS.NONYUSAKI_CD = TSM_COUNT.NONYUSAKI_CD ");
            sb.ApdL("LEFT JOIN M_CONSIGN MC ");
            sb.ApdL("       ON MC.CONSIGN_CD = MNS.CONSIGN_CD ");
            sb.ApdL("LEFT JOIN M_COMMON COM ");
            sb.ApdL("       ON COM.GROUP_CD = 'DISP_JYOTAI_FLAG' ");
            sb.ApdL("      AND COM.VALUE1 = MJF.MIN_JYOTAI_FLAG ");
            sb.ApdN("      AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("LEFT JOIN M_COMMON COM2 ");
            sb.ApdL("       ON COM2.GROUP_CD = 'DISP_GENBA_YO_STATUS' ");
            sb.ApdL("      AND COM2.VALUE1 = MNS.GENBA_YO_STATUS_FLAG ");
            sb.ApdN("      AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("WHERE 1 = 1 ");
            // 物件名
            if (!string.IsNullOrEmpty(cond.BukkenNO))
            {
                if (cond.BukkenNO != ComDefine.COMBO_ALL_VALUE_DECIMAL.ToString())
                {
                    sb.ApdN("   AND MNS.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL(Def_M_BUKKEN.BUKKEN_NO);
                    paramCollection.Add(iNewParam.NewDbParameter(Def_M_BUKKEN.BUKKEN_NO, cond.BukkenNO));
                }
            }
            // 出荷元
            if (!string.IsNullOrEmpty(cond.ShipFrom))
            {
                sb.ApdN("   AND MNS.SHIP_FROM = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.SHIP_FROM_CD);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.SHIP_FROM_CD, cond.ShipFrom));
            }
            // 出荷先
            if (!string.IsNullOrEmpty(cond.ShipTo))
            {
                sb.ApdN("   AND MNS.SHIP_TO = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.SHIP_TO);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.SHIP_TO, cond.ShipTo));
            }
            // 荷受先
            if (!string.IsNullOrEmpty(cond.ConsignCD))
            {
                sb.ApdN("   AND MNS.CONSIGN_CD = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.CONSIGN_CD);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.CONSIGN_CD, cond.ConsignCD));
            }
            // 出荷日
            if (!string.IsNullOrEmpty(cond.ShipDateStart) && !string.IsNullOrEmpty(cond.ShipDateEnd))
            {
                sb.ApdN("   AND MNS.SHIP_DATE BETWEEN ").ApdN(this.BindPrefix).ApdN("ShipDateStart AND ").ApdN(this.BindPrefix).ApdL("ShipDateEnd");
            }
            else if (string.IsNullOrEmpty(cond.ShipDateStart) && !string.IsNullOrEmpty(cond.ShipDateEnd))
            {
                sb.ApdN("   AND MNS.SHIP_DATE <= ").ApdN(this.BindPrefix).ApdL("ShipDateEnd");
            }
            else if (!string.IsNullOrEmpty(cond.ShipDateStart) && string.IsNullOrEmpty(cond.ShipDateEnd))
            {
                sb.ApdN("   AND MNS.SHIP_DATE >= ").ApdN(this.BindPrefix).ApdL("ShipDateStart");
            }
            if (!string.IsNullOrEmpty(cond.ShipDateStart))
            {
                paramCollection.Add(iNewParam.NewDbParameter("ShipDateStart", cond.ShipDateStart));
            }
            if (!string.IsNullOrEmpty(cond.ShipDateEnd))
            {
                paramCollection.Add(iNewParam.NewDbParameter("ShipDateEnd", cond.ShipDateEnd));
            }
            // 製番
            if (!string.IsNullOrEmpty(cond.Seiban))
            {
                sb.ApdL("   AND ( ");
                sb.ApdN("       ',' + REPLACE(MNS.SEIBAN, ' ', '') + ',' LIKE '%,' + ").ApdN(this.BindPrefix).ApdN(Def_M_NONYUSAKI.SEIBAN).ApdL(" + ',%'");
                sb.ApdN("       OR ',' + REPLACE(MNS.SEIBAN, ' ', '') + ',' LIKE '%,' + ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.SEIBAN);
                sb.ApdN("       OR ',' + REPLACE(MNS.SEIBAN, ' ', '') + ',' LIKE ").ApdN(this.BindPrefix).ApdN(Def_M_NONYUSAKI.SEIBAN).ApdL(" + ',%'");
                sb.ApdL("   ) ");
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.SEIBAN, cond.Seiban));
            }
            // 機種
            if (!string.IsNullOrEmpty(cond.Kishu))
            {
                sb.ApdN("   AND MNS.KISHU = ").ApdN(this.BindPrefix).ApdL(Def_M_NONYUSAKI.KISHU);
                paramCollection.Add(iNewParam.NewDbParameter(Def_M_NONYUSAKI.KISHU, cond.Kishu));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       MNS.SHIP_DATE");
            sb.ApdL("     , MNS.NONYUSAKI_NAME");

            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

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
    private DataTable GetAndLockNonyusakiList(DatabaseHelper dbHelper, CondS01 cond, DataTable dtNonyusaki, bool isLock)
    {
        try
        {
            DataTable resultDt = null;
            List<DataTable> dtList = ChunkDataTable(dtNonyusaki, 1000);

            foreach (DataTable chunkDt in dtList)
            {
                DataTable dt = new DataTable(Def_M_NONYUSAKI.Name);
                StringBuilder sb = new StringBuilder();
                DbParamCollection paramCollection = new DbParamCollection();
                INewDbParameterBasic iNewParam = dbHelper;

                // SQL文(納入先マスタ行ロック)
                // MN:納入先マスタ
                sb.ApdL("SELECT DISTINCT");
                sb.ApdL("    MN.NONYUSAKI_CD");
                sb.ApdL("  , CONVERT(NCHAR(27), MN.VERSION, 121) AS VERSION");    // 編集行において桁落ちが発生するため、文字列で取得
                sb.ApdL("  FROM M_NONYUSAKI AS MN");

                if (isLock)
                {
                    sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
                }

                sb.ApdL(" WHERE ");
                // 納入先コード
                SetupWherePhraseNonyusakiCd(sb, iNewParam, paramCollection, chunkDt, "MN");

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
    public int UpdNonyusakiGenbaStatus(DatabaseHelper dbHelper, CondS01 cond, DataTable dt)
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

                // SQL文
                sb.ApdL("UPDATE M_NONYUSAKI");
                sb.ApdL("SET");
                sb.ApdN("       GENBA_YO_STATUS_FLAG = ").ApdN(this.BindPrefix).ApdL("GENBA_YO_STATUS_FLAG");
                sb.ApdN("     , GENBA_YO_BUTSURYO = ").ApdN(this.BindPrefix).ApdL("GENBA_YO_BUTSURYO");
                sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
                sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
                sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
                sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
                sb.ApdL(" WHERE");

                // 納入先コード
                SetupWherePhraseNonyusakiCd(sb, iNewParam, paramCollection, chunkDt, "");

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("GENBA_YO_STATUS_FLAG", ComFunc.GetFldObject(chunkDt.Rows[0], Def_M_NONYUSAKI.GENBA_YO_STATUS_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("GENBA_YO_BUTSURYO", ComFunc.GetFldObject(chunkDt.Rows[0], Def_M_NONYUSAKI.GENBA_YO_BUTSURYO)));

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

    #region　Privateメソッド

    /// --------------------------------------------------
    /// <summary>
    /// 指定されたサイズにデータテーブルを分割します。
    /// </summary>
    /// <param name="dt">元データテーブル</param>
    /// <param name="chunkSize">分割するサイズ</param>
    /// <returns>指定されたデータテーブル</returns>
    /// <create>J.Chen 2024/10/22</create>
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
    /// <create>J.Chen 2024/10/22</create>
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

    #endregion
}
