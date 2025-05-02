using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.IO;

using Condition;
using Commons;
using DSWUtil;
using DSWUtil.DbUtil;

//// --------------------------------------------------
/// <summary>
/// 木枠作成処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
//// --------------------------------------------------
public class WsK03Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK03Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region K0300010:木枠登録 & K0300020:木枠登録(社外)

    #region データチェック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データの存在チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>true:存在する false:存在しない</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool IsExistenceKiwaku(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataTable dt = new DataTable();

            // 木枠データ取得
            dt = this.GetTKiwaku(dbHelper, cond).Copy();

            if (dt.Rows.Count == 0)
            {
                // 存在しない
                return false;
            }
            // 存在する
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データの状態チェック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckCondition(DatabaseHelper dbHelper, CondK03 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 木枠データ取得
            if (dt.Rows.Count == 0)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return false;
            }
            // 登録区分
            string torokuFlag = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.TOROKU_FLAG);
            if (cond.torokuFlag == TOROKU_FLAG.NAI_VALUE1)
            {
                // 0:社内以外の場合はエラー
                if (torokuFlag != TOROKU_FLAG.NAI_VALUE1)
                {
                    // 変更不可、社外梱包です。
                    errMsgID = "K0300010005";
                    return false;
                }
            }
            else
            {
                // 1:社外以外の場合はエラー
                if (torokuFlag != TOROKU_FLAG.GAI_VALUE1)
                {
                    // 社外梱包ではありません。
                    errMsgID = "K0300020005";
                    return false;
                }
            }
            // 作業区分
            string sagyoFlag = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.SAGYO_FLAG);
            // 2:梱包完了、9:出荷済みの場合はエラー
            if (sagyoFlag == SAGYO_FLAG.KONPOKANRYO_VALUE1)
            {
                // 梱包済みです。
                errMsgID = "A9999999040";
                return false;
            }
            else if (sagyoFlag == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済みです。
                errMsgID = "A9999999031";
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

    #region 表示データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>表示データセット</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update>H.Tajimi 2018/11/06 一括Upload対応</update>
    /// <update>H.Tajimi 2018/11/29 添付ファイル対応</update>
    /// <update>K.Tsutsumi 2018/01/23 写真の保存先変更</update>
    /// <update>H.Tajimi 2019/07/30 写真管理方式変更</update>
    /// --------------------------------------------------
    public DataSet GetKiwaku(DatabaseHelper dbHelper, CondK03 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            // 木枠データ
            dt = GetTKiwaku(dbHelper, cond).Copy();
            if (!this.CheckCondition(dbHelper, cond, dt, ref errMsgID, ref args))
            {
                return null;
            }

            if (dt.Rows.Count != 0)
            {
                // コンディション作成
                cond.KojiNo = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.KOJI_NO);

                // CASE MARK File確認
                string fileName = ComFunc.GetFld(dt, 0, Def_T_KIWAKU.CASE_MARK_FILE);
                if (!string.IsNullOrEmpty(fileName))
                {
                    WsAttachFileImpl impl = new WsAttachFileImpl();
                    string filePath = impl.GetFilePath(FileType.CaseMark, null, null, GirenType.None, cond.KojiNo, fileName, null);
                    if (!File.Exists(filePath))
                    {
                        // サーバー上に画像ファイルがない場合、ファイル名もクリア
                        dt.Rows[0][Def_T_KIWAKU.CASE_MARK_FILE] = string.Empty;
                    }
                }
            }

            ds.Tables.Add(dt.Copy());

            // 木枠明細データ
            dt = GetTKiwakuMeisai(dbHelper, cond).Copy();

            ds.Tables.Add(dt.Copy());

            if (cond.torokuFlag == TOROKU_FLAG.GAI_VALUE1)
            {
                // 社外用木枠明細データ
                dt = GetTShagaiKiwakuMeisai(dbHelper, cond).Copy();

                ds.Tables.Add(dt.Copy());
            }

            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region 木枠登録

    /// --------------------------------------------------
    /// <summary>
    /// 木枠登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="rKojiNo">工事識別管理No</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
    /// <update>K.Tsutsumi 2018/01/20 AR対応</update>
    /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
    /// --------------------------------------------------
    public bool InsKiwaku(DatabaseHelper dbHelper, CondK03 cond, DataSet ds, ref string rKojiNo, ref string errMsgID, ref string[] args)
    {
        try
        {
            bool ret;

            DateTime syoriDateTime = DateTime.Now;
            DataTable dtKiwaku = ds.Tables[Def_T_KIWAKU.Name];
            DataTable dtKiwakuMeisai = ds.Tables[Def_T_KIWAKU_MEISAI.Name];

            // コンディション作成
            cond.UpdateDate = syoriDateTime;

            // 存在チェック
            ret = this.IsExistenceKiwaku(dbHelper, cond);
            if (ret)
            {
                // 既に登録されています。
                errMsgID = "A9999999038";
                return false;
            }

            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                // 物件マスタ取得＆ロック
                var dtBukken = this.LockMBukken(dbHelper, cond);
                if (!UtilData.ExistsData(dtBukken))
                {
                    // 該当工事識別名が物件マスタに存在しません。
                    errMsgID = "K0300010013";
                    return false;
                }
            }

            // 工事識別管理NO採番
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.KOJI_NO_VALUE1;
            condSms.LoginInfo = cond.LoginInfo;
            using (WsSmsImpl impl = new WsSmsImpl())
            {
                string kojiNo;
                if (!impl.GetSaiban(dbHelper, condSms, out kojiNo, out errMsgID))
                {
                    return false;
                }
                cond.KojiNo = kojiNo;
                rKojiNo = kojiNo;
            }

            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                // 納入先マスタ存在チェック
                var nonyusakiCd = string.Empty;
                var condNonyusaki = (CondK03)cond.Clone();
                if (cond.KiwakuInsertType == KIWAKU_INSERT_TYPE.AR_VALUE1)
                {
                    condNonyusaki.Ship = string.Empty;
                }
                var dtNonyusaki = this.GetMNonyusaki(dbHelper, condNonyusaki);
                if (!UtilData.ExistsData(dtNonyusaki))
                {
                    if (cond.KiwakuInsertType == KIWAKU_INSERT_TYPE.REGULAR_VALUE1)
                    {
                        // 登録(本体)の場合は、納入先マスタが必須
                        // 該当納入先がありません。\r\n納入先保守で登録してください。
                        errMsgID = "K0300010014";
                        return false;
                    }
                    else if (cond.KiwakuInsertType == KIWAKU_INSERT_TYPE.AR_VALUE1)
                    {
                        // 登録(AR)の場合は、納入先マスタが必須
                        // 該当納入先がありません。\r\nAR情報の「ITEM新規登録」で登録してください。
                        errMsgID = "K0300010015";
                        return false;
                    }
                    // 納入先マスタ登録
                    this.InsMNonyusaki(dbHelper, cond, ref nonyusakiCd, ref errMsgID, ref args);
                }
                else
                {
                    nonyusakiCd = ComFunc.GetFld(dtNonyusaki, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                }
                UtilData.SetFld(dtKiwaku, 0, Def_T_KIWAKU.NONYUSAKI_CD, nonyusakiCd);
            }
            else
            {
                UtilData.SetFld(dtKiwaku, 0, Def_T_KIWAKU.NONYUSAKI_CD, null);
            }

            // 木枠の登録
            ret = InsTKiwaku(dbHelper, cond, dtKiwaku, ref errMsgID, ref args);
            if (!ret)
            {
                return false;
            }

            // 木枠明細の登録
            ret = InsTKiwakuMeisai(dbHelper, cond, dtKiwakuMeisai, ref errMsgID, ref args);
            if (!ret)
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

    #region 木枠データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsTKiwaku(DatabaseHelper dbHelper, CondK03 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // 木枠データ登録
        try
        {
            // Insert
            this.InsKiwakuExec(dbHelper, cond, dt.Rows[0]);
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

    #region 木枠明細データ追加

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/26</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsTKiwakuMeisai(DatabaseHelper dbHelper, CondK03 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // 木枠明細データ登録
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            try
            {
                CondSms condSms = new CondSms(cond.LoginInfo);
                condSms.SaibanFlag = SAIBAN_FLAG.CASE_ID_VALUE1;
                condSms.LoginInfo = cond.LoginInfo;
                using (WsSmsImpl impl = new WsSmsImpl())
                {
                    string caseID;
                    // 内部管理用キー採番
                    if (!impl.GetSaiban(dbHelper, condSms, out caseID, out errMsgID))
                    {
                        return false;
                    }
                    // Insert
                    this.InsKiwakuMeisaiExec(dbHelper, cond, dt.Rows[i], caseID);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //}
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
        }
        return true;
    }


    #endregion

    #region 納入先マスタ追加

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ追加
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>H.Tajimi 2018/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool InsMNonyusaki(DatabaseHelper dbHelper, CondK03 cond, ref string nonyusakiCd, ref string errMsgID, ref string[] args)
    {
        // 納入先マスタ登録
        try
        {
            // 納入先コード採番
            var condSms = new CondSms(cond.LoginInfo);
            condSms.SaibanFlag = SAIBAN_FLAG.US_VALUE1;
            string tmpNonyusakiCd;
            using (var smsImpl = new WsSmsImpl())
            {
                if (!smsImpl.GetSaiban(dbHelper, condSms, out tmpNonyusakiCd, out errMsgID))
                {
                    return false;
                }
                nonyusakiCd = tmpNonyusakiCd;
            }

            this.InsMNonyusakiExec(dbHelper, cond, nonyusakiCd);
        }
        catch (Exception ex)
        {
            if (ex.InnerException.GetType() == typeof(System.Data.DuplicateNameException))
            {
                // 既に登録されている場合はOK
                return true;
            }
            else
            {
                throw new Exception(ex.Message, ex);
            }
        }
        return true;
    }

    #endregion

    #region 木枠変更

    /// --------------------------------------------------
    /// <summary>
    /// 木枠変更
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdKiwaku(DatabaseHelper dbHelper, CondK03 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            bool ret;

            DateTime syoriDateTime = DateTime.Now;
            // コンディション作成
            cond.UpdateDate = syoriDateTime;
            cond.KojiNo = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.KOJI_NO);

            // ロック＆取得
            DataTable dtKiwaku = this.LockKiwaku(dbHelper, cond.KojiNo);

            // チェック
            ret = this.CheckCondition(dbHelper, cond, dtKiwaku, ref errMsgID, ref args);
            if (!ret)
            {
                return ret;
            }

            DataTable dtUpd = ds.Tables[Def_T_KIWAKU.Name];
            DataTable dtDelMei = ds.Tables[Def_T_KIWAKU_MEISAI.Name];
            DataTable dtInsMei = ds.Tables[ComDefine.DTTBL_INSERT];
            int index;

            // 木枠データのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtUpd, dtKiwaku, out notFoundIndex, Def_T_KIWAKU.VERSION, Def_T_KIWAKU.KOJI_NO);
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

            // 木枠明細データのバージョンチェック
            DataTable dtKiwakuMeisai = this.GetTKiwakuMeisai(dbHelper, cond).Copy();

            // データのバージョンチェック
            index = this.CheckSameData(dtDelMei, dtKiwakuMeisai, out notFoundIndex, Def_T_KIWAKU_MEISAI.VERSION, Def_T_KIWAKU_MEISAI.KOJI_NO, Def_T_KIWAKU_MEISAI.CASE_ID);
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

            // 木枠の更新
            this.UpdKiwakuExec(dbHelper, cond, dtUpd);

            // 木枠明細の削除
            this.DelKiwakuMeisaiExec(dbHelper, cond, null);

            // 木枠明細の追加
            if (0 < dtInsMei.Rows.Count)
            {
                if (!this.UpdTKiwakuMeisai(dbHelper, cond, dtInsMei, ref errMsgID, ref args))
                {
                    return false;
                }
            }

            // 社外用木枠明細データ削除
            foreach (DataRow dr in dtDelMei.Rows)
            {
                this.DelShagaiKiwakuMeisaiExec(dbHelper, cond, ComFunc.GetFld(dr,Def_T_SHAGAI_KIWAKU_MEISAI.CASE_ID));
            }

            // 出荷明細データから木枠情報削除
            this.UpdShukkaMeisaiExec(dbHelper, cond);

            return true;

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
    /// 木枠明細データ追加(既存データをもう一度Insert)
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">データテーブル</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/28</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool UpdTKiwakuMeisai(DatabaseHelper dbHelper, CondK03 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        // 木枠明細データ登録
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            try
            {
                string caseID;
                caseID = ComFunc.GetFld(dt, i, Def_T_KIWAKU_MEISAI.CASE_ID);

                if (string.IsNullOrEmpty(caseID))
                {
                    // 内部管理用キー採番
                    CondSms condSms = new CondSms(cond.LoginInfo);
                    condSms.SaibanFlag = SAIBAN_FLAG.CASE_ID_VALUE1;
                    condSms.LoginInfo = cond.LoginInfo;
                    using (WsSmsImpl impl = new WsSmsImpl())
                    {
                        if (!impl.GetSaiban(dbHelper, condSms, out caseID, out errMsgID))
                        {
                            return false;
                        }
                    }
                }

                // Insert
                this.InsKiwakuMeisaiExec(dbHelper, cond, dt.Rows[i], caseID);
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
        return true;
    }

    #endregion

    #region 木枠削除

    /// --------------------------------------------------
    /// <summary>
    /// 木枠削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="ds">データセット</param>
    /// <param name="errMsgID">メッセージ</param>
    /// <param name="args">パラメータ</param>
    /// <returns>true:成功 false:失敗</returns>
    /// <create>M.Tsutsumi 2010/07/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelKiwaku(DatabaseHelper dbHelper, CondK03 cond, DataSet ds, ref string errMsgID, ref string[] args)
    {
        try
        {
            bool ret;

            DateTime syoriDateTime = DateTime.Now;
            // コンディション作成
            cond.UpdateDate = syoriDateTime;
            cond.KojiNo = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.KOJI_NO);

            // ロック＆取得
            DataTable dtKiwaku = this.LockKiwaku(dbHelper, cond.KojiNo);

            // チェック
            ret = this.CheckCondition(dbHelper, cond, dtKiwaku, ref errMsgID, ref args);
            if (!ret)
            {
                return ret;
            }

            DataTable dtDel = ds.Tables[Def_T_KIWAKU.Name];
            DataTable dtDelMei = ds.Tables[ComDefine.DTTBL_DELETE];
            int index;

            // 木枠データのバージョンチェック
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtDel, dtKiwaku, out notFoundIndex, Def_T_KIWAKU.VERSION, Def_T_KIWAKU.KOJI_NO);
            if (0 <= index)
            {
                // 他端末で更新された為、更新出来ませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 木枠明細データ
            DataTable dtKiwakuMeisai = this.GetTKiwakuMeisai(dbHelper, cond).Copy();

            // データ数チェック
            if (dtKiwakuMeisai.Rows.Count != dtDelMei.Rows.Count)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 削除データのバージョンチェック
            index = this.CheckSameData(dtDelMei, dtKiwakuMeisai, out notFoundIndex, Def_T_KIWAKU_MEISAI.VERSION, Def_T_KIWAKU_MEISAI.KOJI_NO, Def_T_KIWAKU_MEISAI.CASE_ID);
            if (0 <= index)
            {
                // 他端末で更新された為、更新できませんでした。
                errMsgID = "A9999999027";
                return false;
            }

            // 木枠の削除
            this.DelKiwakuExec(dbHelper, cond);

            // 木枠明細の削除
            this.DelKiwakuMeisaiExec(dbHelper, cond, null);

            // 社外用木枠明細データ削除
            this.DelShagaiKiwakuMeisaiExec(dbHelper, cond, null);

            // 出荷明細データから木枠情報削除
            this.UpdShukkaMeisaiExec(dbHelper, cond);

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

    #region K0300010:木枠登録 & K0300020:木枠登録(社外)

    #region SELECT

    #region 木枠データ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="kojiNo">工事識別管理No</param>
    /// <returns>データテーブル</returns>
    /// <create>M.Tsutsumi 2010/07/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockKiwaku(DatabaseHelper dbHelper, string kojiNo)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , KOJI_NAME");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , TOROKU_FLAG");
            sb.ApdL("     , SAGYO_FLAG");
            sb.ApdL("     , VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       KOJI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", kojiNo));

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

    #region T_KIWAKU

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>木枠データテーブル</returns>
    /// <create>M.Tsutsumi 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTKiwaku(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "T_KIWAKU.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       T_KIWAKU.KOJI_NO");
            sb.ApdL("     , T_KIWAKU.KOJI_NAME");
            sb.ApdL("     , T_KIWAKU.SHIP");
            sb.ApdL("     , T_KIWAKU.TOROKU_FLAG");
            sb.ApdL("     , T_KIWAKU.CASE_MARK_FILE");
            sb.ApdL("     , T_KIWAKU.DELIVERY_NO");
            sb.ApdL("     , T_KIWAKU.PORT_OF_DESTINATION");
            sb.ApdL("     , T_KIWAKU.AIR_BOAT");
            sb.ApdL("     , T_KIWAKU.DELIVERY_DATE");
            sb.ApdL("     , T_KIWAKU.DELIVERY_POINT");
            sb.ApdL("     , T_KIWAKU.FACTORY");
            sb.ApdL("     , T_KIWAKU.REMARKS");
            sb.ApdL("     , T_KIWAKU.SAGYO_FLAG");
            sb.ApdL("     , M_COMMON.ITEM_NAME SAGYO_FLAG_NAME");
            //sb.ApdL("     , T_KIWAKU.SHUKKA_DATE");
            //sb.ApdL("     , T_KIWAKU.SHUKKA_USER_ID");
            //sb.ApdL("     , T_KIWAKU.SHUKKA_USER_NAME");
            //sb.ApdL("     , T_KIWAKU.UNSOKAISHA_NAME");
            //sb.ApdL("     , T_KIWAKU.INVOICE_NO");
            //sb.ApdL("     , T_KIWAKU.OKURIJYO_NO");
            //sb.ApdL("     , T_KIWAKU.CREATE_DATE");
            //sb.ApdL("     , T_KIWAKU.CREATE_USER_ID");
            //sb.ApdL("     , T_KIWAKU.CREATE_USER_NAME");
            //sb.ApdL("     , T_KIWAKU.UPDATE_DATE");
            //sb.ApdL("     , T_KIWAKU.UPDATE_USER_ID");
            //sb.ApdL("     , T_KIWAKU.UPDATE_USER_NAME");
            sb.ApdL("     , T_KIWAKU.VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU");
            sb.ApdL("  LEFT JOIN M_COMMON ON M_COMMON.GROUP_CD = 'SAGYO_FLAG'");
            sb.ApdL("                  AND M_COMMON.VALUE1 = T_KIWAKU.SAGYO_FLAG");
            sb.ApdN("                  AND M_COMMON.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            // 工事識別管理NO
            if (!string.IsNullOrEmpty(cond.KojiNo))
            {
                fieldName = "KOJI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KojiNo));
            }

            // 工事識別名称
            if (!string.IsNullOrEmpty(cond.KojiName))
            {
                fieldName = "KOJI_NAME";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KojiName));
            }

            // 出荷便
            if (cond.Ship != null)
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Ship));
            }

            //sb.ApdL(" ORDER BY");
            //sb.ApdL("       FIELD1");
            //sb.ApdL("     , FIELD2");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU.Name);

            return ds.Tables[Def_T_KIWAKU.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region T_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>木枠明細データテーブル</returns>
    /// <create>M.Tsutsumi 2010/07/21</create>
    /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応 PRINT_CASE_NO追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTKiwakuMeisai(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "T_KIWAKU_MEISAI.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       T_KIWAKU_MEISAI.KOJI_NO");
            sb.ApdL("     , T_KIWAKU_MEISAI.CASE_ID");
            sb.ApdL("     , T_KIWAKU_MEISAI.CASE_NO");
            sb.ApdL("     , T_KIWAKU_MEISAI.STYLE");
            sb.ApdL("     , T_KIWAKU_MEISAI.ITEM");
            sb.ApdL("     , T_KIWAKU_MEISAI.DESCRIPTION_1");
            sb.ApdL("     , T_KIWAKU_MEISAI.DESCRIPTION_2");
            sb.ApdL("     , T_KIWAKU_MEISAI.DIMENSION_L");
            sb.ApdL("     , T_KIWAKU_MEISAI.DIMENSION_W");
            sb.ApdL("     , T_KIWAKU_MEISAI.DIMENSION_H");
            sb.ApdL("     , T_KIWAKU_MEISAI.MMNET");
            sb.ApdL("     , T_KIWAKU_MEISAI.NET_W");
            sb.ApdL("     , T_KIWAKU_MEISAI.GROSS_W");
            sb.ApdL("     , T_KIWAKU_MEISAI.MOKUZAI_JYURYO");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_1");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_2");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_3");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_4");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_5");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_6");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_7");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_8");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_9");
            sb.ApdL("     , T_KIWAKU_MEISAI.PALLET_NO_10");
            sb.ApdL("     , T_KIWAKU_MEISAI.SHUKKA_DATE");
            sb.ApdL("     , T_KIWAKU_MEISAI.SHUKKA_USER_ID");
            sb.ApdL("     , T_KIWAKU_MEISAI.SHUKKA_USER_NAME");
            //sb.ApdL("     , T_KIWAKU_MEISAI.UPDATE_DATE");
            //sb.ApdL("     , T_KIWAKU_MEISAI.UPDATE_USER_ID");
            //sb.ApdL("     , T_KIWAKU_MEISAI.UPDATE_USER_NAME");
            sb.ApdL("     , T_KIWAKU_MEISAI.VERSION");
            sb.ApdL("     , T_KIWAKU_MEISAI.PRINT_CASE_NO");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            // 工事識別管理NO
            if (!string.IsNullOrEmpty(cond.KojiNo))
            {
                fieldName = "KOJI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KojiNo));
            }

            // 内部管理用キー
            if (!string.IsNullOrEmpty(cond.CaseId))
            {
                fieldName = "CASE_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseId));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       T_KIWAKU_MEISAI.CASE_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU_MEISAI.Name);

            return ds.Tables[Def_T_KIWAKU_MEISAI.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region T_SHAGAI_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 社外用木枠明細データ取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>社外用木枠明細データテーブル</returns>
    /// <create>M.Tsutsumi 2010/09/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTShagaiKiwakuMeisai(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "T_SHAGAI_KIWAKU_MEISAI.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       T_SHAGAI_KIWAKU_MEISAI.KOJI_NO");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.CASE_ID");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.TAG_NO");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.UPDATE_DATE");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.UPDATE_USER_ID");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.UPDATE_USER_NAME");
            sb.ApdL("     , T_SHAGAI_KIWAKU_MEISAI.VERSION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHAGAI_KIWAKU_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 工事識別管理NO
            if (!string.IsNullOrEmpty(cond.KojiNo))
            {
                fieldName = "KOJI_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.KojiNo));
            }

            // 内部管理用キー
            if (!string.IsNullOrEmpty(cond.CaseId))
            {
                fieldName = "CASE_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.CaseId));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_SHAGAI_KIWAKU_MEISAI.Name);

            return ds.Tables[Def_T_SHAGAI_KIWAKU_MEISAI.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region M_NONYUSAKI

    /// --------------------------------------------------
    /// <summary>
    /// 納入先取得
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>納入先データテーブル</returns>
    /// <create>H.Tajimi 2018/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetMNonyusaki(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "M_NONYUSAKI.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       M_NONYUSAKI.SHUKKA_FLAG");
            sb.ApdL("     , M_NONYUSAKI.BUKKEN_NO");
            sb.ApdL("     , M_NONYUSAKI.NONYUSAKI_CD");
            sb.ApdL("     , M_NONYUSAKI.NONYUSAKI_NAME");
            sb.ApdL("     , M_NONYUSAKI.SHIP");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_NONYUSAKI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            // 出荷区分
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                fieldName = "SHUKKA_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.ShukkaFlag));
            }

            // 物件NO
            if (!string.IsNullOrEmpty(cond.BukkenNo))
            {
                fieldName = "BUKKEN_NO";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.BukkenNo));
            }

            // 便
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.Ship));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_M_NONYUSAKI.Name);

            return ds.Tables[Def_M_NONYUSAKI.Name];

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 物件マスタ取得＆ロック

    /// --------------------------------------------------
    /// <summary>
    /// 物件マスタ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="bukkenNo">物件No</param>
    /// <param name="bukkenName">物件名</param>
    /// <returns>データテーブル</returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockMBukken(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("  FROM ");
            sb.ApdL("       M_BUKKEN");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE ");
            sb.ApdL("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdL("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdL("   AND BUKKEN_NAME = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NAME");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NAME", cond.KojiName));

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

    #endregion

    #region INSERT

    #region T_KIWAKU

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dr">木枠データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update>H.Tajimi 2018/12/25 木枠梱包業務改善</update>
    /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
    /// --------------------------------------------------
    public int InsKiwakuExec(DatabaseHelper dbHelper, CondK03 cond, DataRow dr)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_KIWAKU ");
            sb.ApdL("(");
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
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UNSOKAISHA_NAME");
            sb.ApdL("     , INVOICE_NO");
            sb.ApdL("     , OKURIJYO_NO");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , INSERT_TYPE");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KOJI_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TOROKU_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CASE_MARK_FILE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DELIVERY_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PORT_OF_DESTINATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("AIR_BOAT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DELIVERY_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DELIVERY_POINT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("FACTORY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("REMARKS");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UNSOKAISHA_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("INVOICE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("OKURIJYO_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("INSERT_TYPE");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NAME", ComFunc.GetFldObject(dr, Def_T_KIWAKU.KOJI_NAME)));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SHIP)));
            paramCollection.Add(iNewParam.NewDbParameter("TOROKU_FLAG", ComFunc.GetFldObject(dr, Def_T_KIWAKU.TOROKU_FLAG, TOROKU_FLAG.DEFAULT_VALUE1)));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_MARK_FILE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.CASE_MARK_FILE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("PORT_OF_DESTINATION", ComFunc.GetFldObject(dr, Def_T_KIWAKU.PORT_OF_DESTINATION, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("AIR_BOAT", ComFunc.GetFldObject(dr, Def_T_KIWAKU.AIR_BOAT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_DATE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_POINT", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_POINT, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("FACTORY", ComFunc.GetFldObject(dr, Def_T_KIWAKU.FACTORY, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("REMARKS", ComFunc.GetFldObject(dr, Def_T_KIWAKU.REMARKS, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SAGYO_FLAG, SAGYO_FLAG.DEFAULT_VALUE1)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SHUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SHUKKA_USER_ID, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SHUKKA_USER_NAME, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UNSOKAISHA_NAME", ComFunc.GetFldObject(dr, Def_T_KIWAKU.UNSOKAISHA_NAME, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("INVOICE_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU.INVOICE_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("OKURIJYO_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU.OKURIJYO_NO, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_KIWAKU.SHUKKA_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_KIWAKU.NONYUSAKI_CD, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("INSERT_TYPE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.INSERT_TYPE, DBNull.Value)));

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

    #region T_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ 登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dr">木枠明細データ</param>
    /// <param name="caseID">内部管理用キー</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応 PRINT_CASE_NO追加</update>
    /// <update></update>
    /// --------------------------------------------------
    public int InsKiwakuMeisaiExec(DatabaseHelper dbHelper, CondK03 cond, DataRow dr, string caseID)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_KIWAKU_MEISAI ");
            sb.ApdL("(");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , CASE_NO");
            sb.ApdL("     , STYLE");
            sb.ApdL("     , ITEM");
            sb.ApdL("     , DESCRIPTION_1");
            sb.ApdL("     , DESCRIPTION_2");
            sb.ApdL("     , DIMENSION_L");
            sb.ApdL("     , DIMENSION_W");
            sb.ApdL("     , DIMENSION_H");
            sb.ApdL("     , MMNET");
            sb.ApdL("     , NET_W");
            sb.ApdL("     , GROSS_W");
            sb.ApdL("     , MOKUZAI_JYURYO");
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
            sb.ApdL("     , SHUKKA_DATE");
            sb.ApdL("     , SHUKKA_USER_ID");
            sb.ApdL("     , SHUKKA_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , PRINT_CASE_NO");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STYLE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ITEM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DIMENSION_L");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DIMENSION_W");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DIMENSION_H");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MMNET");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NET_W");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("GROSS_W");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("MOKUZAI_JYURYO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_3");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_4");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_5");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_6");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_7");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_8");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_9");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO_10");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_USER_NAME");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PRINT_CASE_NO");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseID));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.CASE_NO)));
            paramCollection.Add(iNewParam.NewDbParameter("STYLE", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.STYLE)));
            paramCollection.Add(iNewParam.NewDbParameter("ITEM", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.ITEM, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_1", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_2", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_L", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_L)));
            paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_W)));
            paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_H", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_H)));
            paramCollection.Add(iNewParam.NewDbParameter("MMNET", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.MMNET)));
            paramCollection.Add(iNewParam.NewDbParameter("NET_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.NET_W)));
            paramCollection.Add(iNewParam.NewDbParameter("GROSS_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.GROSS_W, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("MOKUZAI_JYURYO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO)));
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
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_DATE", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.SHUKKA_DATE, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_ID", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.SHUKKA_USER_ID, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_USER_NAME", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.SHUKKA_USER_NAME, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("PRINT_CASE_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO)));

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

    #region M_NONYUSAKI

    /// --------------------------------------------------
    /// <summary>
    /// 納入先マスタ登録
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">納入先用コンディション</param>
    /// <param name="nonyusakiCd">納入先コード</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsMNonyusakiExec(DatabaseHelper dbHelper, CondK03 cond, string nonyusakiCd)
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
            sb.ApdL("     , LIST_FLAG_NAME0");
            sb.ApdL("     , LIST_FLAG_NAME1");
            sb.ApdL("     , LIST_FLAG_NAME2");
            sb.ApdL("     , LIST_FLAG_NAME3");
            sb.ApdL("     , LIST_FLAG_NAME4");
            sb.ApdL("     , LIST_FLAG_NAME5");
            sb.ApdL("     , LIST_FLAG_NAME6");
            sb.ApdL("     , LIST_FLAG_NAME7");
            sb.ApdL("     , REMOVE_FLAG");
            sb.ApdL("     , VERSION");
            sb.ApdL("     , TRANSPORT_FLAG");
            sb.ApdL("     , ESTIMATE_FLAG");
            sb.ApdL("     , SHIP_DATE");
            sb.ApdL("     , SHIP_FROM");
            sb.ApdL("     , SHIP_TO");
            sb.ApdL("     , SHIP_NO");
            sb.ApdL("     , SHIP_SEIBAN");
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
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME0");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME1");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME2");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME3");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME4");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME5");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME6");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LIST_FLAG_NAME7");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdL("REMOVE_FLAG");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TRANSPORT_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ESTIMATE_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_FROM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_TO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHIP_SEIBAN");
            sb.ApdL(")");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", cond.BukkenNo));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_NAME", cond.KojiName));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP", cond.Ship));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.MIKAN_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("MAINTE_USER_NAME", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME0", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME1", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME2", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME3", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME4", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME5", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME6", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("LIST_FLAG_NAME7", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("REMOVE_FLAG", REMOVE_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TRANSPORT_FLAG", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("ESTIMATE_FLAG", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_FROM", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_TO", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_NO", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("SHIP_SEIBAN", DBNull.Value));

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

    #region T_KIWAKU

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">木枠データテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuExec(DatabaseHelper dbHelper, CondK03 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU ");
            sb.ApdL("SET");
            sb.ApdN("       CASE_MARK_FILE = ").ApdN(this.BindPrefix).ApdL("CASE_MARK_FILE");
            sb.ApdN("     , DELIVERY_NO = ").ApdN(this.BindPrefix).ApdL("DELIVERY_NO");
            sb.ApdN("     , PORT_OF_DESTINATION = ").ApdN(this.BindPrefix).ApdL("PORT_OF_DESTINATION");
            sb.ApdN("     , AIR_BOAT = ").ApdN(this.BindPrefix).ApdL("AIR_BOAT");
            sb.ApdN("     , DELIVERY_DATE = ").ApdN(this.BindPrefix).ApdL("DELIVERY_DATE");
            sb.ApdN("     , DELIVERY_POINT = ").ApdN(this.BindPrefix).ApdL("DELIVERY_POINT");
            sb.ApdN("     , FACTORY = ").ApdN(this.BindPrefix).ApdL("FACTORY");
            sb.ApdN("     , REMARKS = ").ApdN(this.BindPrefix).ApdL("REMARKS");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("CASE_MARK_FILE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.CASE_MARK_FILE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("PORT_OF_DESTINATION", ComFunc.GetFldObject(dr, Def_T_KIWAKU.PORT_OF_DESTINATION, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("AIR_BOAT", ComFunc.GetFldObject(dr, Def_T_KIWAKU.AIR_BOAT, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_DATE", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DELIVERY_POINT", ComFunc.GetFldObject(dr, Def_T_KIWAKU.DELIVERY_POINT, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("FACTORY", ComFunc.GetFldObject(dr, Def_T_KIWAKU.FACTORY, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("REMARKS", ComFunc.GetFldObject(dr, Def_T_KIWAKU.REMARKS, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region T_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="dt">木枠明細データテーブル</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/22</create>
    /// <update>H.Tajimi 2015/11/25 ケースナンバーの欠番対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuMeisaiExec(DatabaseHelper dbHelper, CondK03 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU_MEISAI ");
            sb.ApdL("SET");
            sb.ApdN("       CASE_NO = ").ApdN(this.BindPrefix).ApdL("CASE_NO");
            sb.ApdN("     , STYLE = ").ApdN(this.BindPrefix).ApdL("STYLE");
            sb.ApdN("     , ITEM = ").ApdN(this.BindPrefix).ApdL("ITEM");
            sb.ApdN("     , DESCRIPTION_1 = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_1");
            sb.ApdN("     , DESCRIPTION_2 = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_2");
            sb.ApdN("     , DIMENSION_L = ").ApdN(this.BindPrefix).ApdL("DIMENSION_L");
            sb.ApdN("     , DIMENSION_W = ").ApdN(this.BindPrefix).ApdL("DIMENSION_W");
            sb.ApdN("     , DIMENSION_H = ").ApdN(this.BindPrefix).ApdL("DIMENSION_H");
            sb.ApdN("     , MMNET = ").ApdN(this.BindPrefix).ApdL("MMNET");
            sb.ApdN("     , MOKUZAI_JYURYO = ").ApdN(this.BindPrefix).ApdL("MOKUZAI_JYURYO");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , PRINT_CASE_NO = ").ApdN(this.BindPrefix).ApdL("PRINT_CASE_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("CASE_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.CASE_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("STYLE", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.STYLE)));
                paramCollection.Add(iNewParam.NewDbParameter("ITEM", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.ITEM, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_1", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_2", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_L", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_L)));
                paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_W)));
                paramCollection.Add(iNewParam.NewDbParameter("DIMENSION_H", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DIMENSION_H)));
                paramCollection.Add(iNewParam.NewDbParameter("MMNET", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.MMNET)));
                paramCollection.Add(iNewParam.NewDbParameter("MOKUZAI_JYURYO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.MOKUZAI_JYURYO)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.CASE_ID)));
                paramCollection.Add(iNewParam.NewDbParameter("PRINT_CASE_NO", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO)));

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

    #region T_SHUKKA_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ 更新
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/09/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiExec(DatabaseHelper dbHelper, CondK03 cond)
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
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("CLEAR_KOJI_NO");
            sb.ApdN("     , CASE_ID = ").ApdN(this.BindPrefix).ApdL("CLEAR_CASE_ID");
            sb.ApdN("     , KIWAKUKONPO_DATE = ").ApdN(this.BindPrefix).ApdL("CLEAR_KIWAKUKONPO_DATE");
            sb.ApdL("     , JYOTAI_FLAG = (");
            sb.ApdN("           CASE    WHEN PALLETKONPO_DATE IS NOT NULL THEN ").ApdL(JYOTAI_FLAG.PALLETZUMI_VALUE1);
            sb.ApdN("                   WHEN BOXKONPO_DATE IS NOT NULL THEN ").ApdL(JYOTAI_FLAG.BOXZUMI_VALUE1);
            sb.ApdN("                   WHEN SHUKA_DATE IS NOT NULL THEN ").ApdL(JYOTAI_FLAG.SHUKAZUMI_VALUE1);
            sb.ApdN("                   ELSE ").ApdL(JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1);
            sb.ApdL("           END");
            sb.ApdL("       ) ");
            sb.ApdL(" WHERE");
            sb.ApdN("       T_SHUKKA_MEISAI.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("   AND NOT EXISTS");
            sb.ApdL("       (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM ");
            sb.ApdL("               T_KIWAKU_MEISAI");
            sb.ApdL("         WHERE ");
            sb.ApdL("               T_KIWAKU_MEISAI.KOJI_NO = T_SHUKKA_MEISAI.KOJI_NO");
            sb.ApdL("           AND T_KIWAKU_MEISAI.CASE_ID = T_SHUKKA_MEISAI.CASE_ID");
            sb.ApdN("           AND T_KIWAKU_MEISAI.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("       )");
            sb.ApdL("   AND NOT EXISTS");
            sb.ApdL("       (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM ");
            sb.ApdL("               T_SHAGAI_KIWAKU_MEISAI");
            sb.ApdL("         WHERE ");
            sb.ApdL("               T_SHAGAI_KIWAKU_MEISAI.KOJI_NO = T_SHUKKA_MEISAI.KOJI_NO");
            sb.ApdL("           AND T_SHAGAI_KIWAKU_MEISAI.CASE_ID = T_SHUKKA_MEISAI.CASE_ID");
            sb.ApdN("           AND T_SHAGAI_KIWAKU_MEISAI.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("       )");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CLEAR_KOJI_NO", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("CLEAR_CASE_ID", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("CLEAR_KIWAKUKONPO_DATE", DBNull.Value));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region T_KIWAKU

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelKiwakuExec(DatabaseHelper dbHelper, CondK03 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_KIWAKU");
            sb.ApdL(" WHERE ");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region T_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="caseID">内部管理用キー</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelKiwakuMeisaiExec(DatabaseHelper dbHelper, CondK03 cond, string caseID)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_KIWAKU_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            if (!string.IsNullOrEmpty(caseID))
            {
                sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseID));
            }

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region T_SHAGAI_KIWAKU_MEISAI

    /// --------------------------------------------------
    /// <summary>
    /// 社外用木枠明細データ削除
    /// </summary>
    /// <param name="dbHelper">DBヘルパー</param>
    /// <param name="cond">K03用コンディション</param>
    /// <param name="caseID">内部管理用キー</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>M.Tsutsumi 2010/09/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelShagaiKiwakuMeisaiExec(DatabaseHelper dbHelper, CondK03 cond, string caseID)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE SKM FROM T_SHAGAI_KIWAKU_MEISAI SKM");
            sb.ApdL(" WHERE SKM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            if (!string.IsNullOrEmpty(caseID))
            {
                sb.ApdN("   AND SKM.CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", caseID));
            }
            sb.ApdL("   AND NOT EXISTS");
            sb.ApdL("       (");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM ");
            sb.ApdL("               T_KIWAKU_MEISAI TKM");
            sb.ApdL("         WHERE ");
            sb.ApdL("               TKM.KOJI_NO = SKM.KOJI_NO");
            sb.ApdL("           AND TKM.CASE_ID = SKM.CASE_ID");
            sb.ApdL("       )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

}
