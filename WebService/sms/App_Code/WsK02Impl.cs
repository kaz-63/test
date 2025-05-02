using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Linq;

using Condition;
using Commons;

using DSWUtil;
using DSWUtil.DbUtil;
using System.IO;
using System.Drawing;

//// --------------------------------------------------
/// <summary>
/// 梱包作業処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsK02Impl : WsBaseImpl
{
    #region 定数

    /// --------------------------------------------------
    /// <summary>
    /// DESCRIPTION_1の長さ
    /// </summary>
    /// <create>H.Tajimi 2015/11/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private const int LEN_DESCRIPTION_1 = 30;

    #endregion

    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK02Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region K0200010:ラベル発行

    /// --------------------------------------------------
    /// <summary>
    /// ボックスNo・パレットNoのNoListを取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="condK02"></param>
    /// <param name="dt"></param>
    /// <param name="errorMsgID"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool GetNoList(DatabaseHelper dbHelper, CondK02 condK02, out DataTable dt, out string errorMsgID)
    {
        bool result = false;
        errorMsgID = string.Empty;
        dt = new DataTable();

        dt.TableName = Def_M_SAIBAN.CURRENT_NO;
        dt.Columns.Add(Def_M_SAIBAN.CURRENT_NO, typeof(string));

        WsSmsImpl sms = new WsSmsImpl();
        CondSms cond = new CondSms(condK02.LoginInfo);
        cond.SaibanFlag = condK02.SaibanFlag;

        for (int i = 0; i < condK02.Count; i++)
        {
            string saiban;
            DataRow dr = dt.NewRow();
            result = sms.GetSaiban(dbHelper, cond, out saiban, out errorMsgID);
            dr[0] = saiban;
            dt.Rows.Add(dr);
        }

        return result;
    }

    #endregion

    #region BOXリスト発行
    /// --------------------------------------------------
    /// <summary>
    /// BOXNoListからそれぞれの明細を取得します。
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMeisai(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string boxNo = ComFunc.GetFld(dt, i, Def_T_SHUKKA_MEISAI.BOX_NO);
                // SQLをBOXNOごとに発行するためBOXNOをテーブル名にしている
                ds.Tables.Add(GetBoxMeisai(dbHelper, boxNo));
            }
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region パレットリスト発行
    /// --------------------------------------------------
    /// <summary>
    /// 取得したパレットNoリストからそれぞれのBoxのリストを取得します。
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetBoxData(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string palletNo = ComFunc.GetFld(dt, i, Def_T_SHUKKA_MEISAI.PALLET_NO);
                // SQLをパレットNOごとに発行するためパレットNOをテーブル名にしている
                ds.Tables.Add(GetPalletMeisai(dbHelper, palletNo));
            }
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region パッキングリスト発行
    /// --------------------------------------------------
    /// <summary>
    /// パッキングリスト発行用のデータ取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="language">言語</param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/28</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPackingMeisai(DatabaseHelper dbHelper, DataTable dt, String language)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            DataTable dtt = new DataTable(Def_T_KIWAKU.KOJI_NO);
            DataTable dtm = new DataTable(Def_T_SHUKKA_MEISAI.Name);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string kojiNo = ComFunc.GetFld(dt, i, Def_T_KIWAKU.KOJI_NO);

                dtt.Merge(GetPackingKiwaku(dbHelper, kojiNo));
                dtm.Merge(GetPackingData(dbHelper, kojiNo, null, language));
            }

            ds.Tables.Add(dtt);
            ds.Tables.Add(dtm);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region K0200040:木枠梱包登録 & K0200060:木枠梱包登録(社外)

    #region 木枠梱包登録用データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包登録用データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKiwakuKonpoTorokuData(DatabaseHelper dbHelper, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);
            if (dtKiwaku.Rows.Count < 1)
            {
                // 該当する工事識別Noはありません。
                errMsgID = "A9999999030";
                return null;
            }

            // 出荷済チェック
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return null;
            }


            CondK02 condK02 = (CondK02)cond.Clone();
            condK02.KojiNo = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
            DataTable dtKiwakuMeisai = this.GetKiwakuKonpoKiwakuMeisaiData(dbHelper, condK02);
            if (dtKiwakuMeisai.Rows.Count < 1)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return null;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtKiwaku);
            ds.Tables.Add(dtKiwakuMeisai);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 編集時チェック

    /// --------------------------------------------------
    /// <summary>
    /// 編集時チェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool CheckKiwakuKonpoTorokuEdit(DatabaseHelper dbHelper, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwakuMeisai = this.GetKiwakuKonpoKiwakuMeisaiData(dbHelper, cond);
            // 存在チェック
            if (dtKiwakuMeisai.Rows.Count < 1)
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            // 出荷日付チェック
            if (ComFunc.GetFldObject(dtKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.SHUKKA_DATE) != DBNull.Value && ComFunc.GetFldObject(dtKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.SHUKKA_DATE) != null)
            {
                // 出荷済です。編集出来ません。
                errMsgID = "A9999999032";
                return false;
            }

            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);
            // 存在チェック
            if (dtKiwaku.Rows.Count < 1)
            {
                // 該当する工事識別Noはありません。
                errMsgID = "A9999999030";
                return false;
            }
            // 出荷済チェック
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return false;
            }
            // 梱包完了チェック
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.KONPOKANRYO_VALUE1)
            {
                // 梱包完了です。編集できません。
                errMsgID = "A9999999033";
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

    #region 木枠梱包登録更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包登録更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdKiwakuKonpoToroku(DatabaseHelper dbHelper, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);
            // 存在チェック
            if (dtKiwaku.Rows.Count < 1)
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }
            // 出荷済チェック
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return false;
            }

            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == cond.SagyoFlag)
            {
                // 作業区分が更新するデータと同じなので以降の処理は行わない。
                return true;
            }

            if (cond.SagyoFlag == SAGYO_FLAG.KONPOKANRYO_VALUE1)
            {
                DataTable dtCheck;
                dtCheck = this.GetKiwakuMeisaiPalletNoNullData(dbHelper, cond);
                if (0 < dtCheck.Rows.Count)
                {
                    // 梱包未登録明細があるので作業状況は更新出来ません。
                    errMsgID = "A9999999034";
                    return false;
                }
            }

            this.UpdKiwakuKonpoKiwaku(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region K0200050:木枠梱包明細登録 & K0200070:木枠梱包明細登録(社外)

    #region 木枠梱包明細データの取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包明細データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ</returns>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
    /// --------------------------------------------------
    public DataSet GetKiwakuKonpoMeisaiData(DatabaseHelper dbHelper, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dt = this.GetKiwakuKonpoKiwakuMeisaiData(dbHelper, cond);
            DataTable dtShagai = null;
            if (cond.TorokuFlag == TOROKU_FLAG.GAI_VALUE1)
            {
                dtShagai = this.GetKiwakuKonpoShagaiKiwakuMeisaiData(dbHelper, cond);
            }
            // 木枠データを取得
            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);

            if (dt.Rows.Count < 1)
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return null;
            }

            // 出荷済チェック
            if (ComFunc.GetFld(dt, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return null;
            }

            // 出荷日付チェック
            if (ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.SHUKKA_DATE) != DBNull.Value && ComFunc.GetFldObject(dt, 0, Def_T_KIWAKU_MEISAI.SHUKKA_DATE) != null)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return null;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (dtShagai != null)
            {
                ds.Tables.Add(dtShagai);
            }
            ds.Tables.Add(dtKiwaku);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 木枠梱包明細登録更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠梱包明細登録更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool UpdKiwakuKonpoMeisaiToroku(DatabaseHelper dbHelper, DataSet ds, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtInputKiwakuMeisai = ds.Tables[Def_T_KIWAKU_MEISAI.Name];
            DataTable dtInputShagaiKiwakuMeisai = null;
            // 更新日時の設定
            cond.UpdateDate = DateTime.Now;
            if (ds.Tables.Contains(Def_T_SHAGAI_KIWAKU_MEISAI.Name))
            {
                dtInputShagaiKiwakuMeisai = ds.Tables[Def_T_SHAGAI_KIWAKU_MEISAI.Name];
            }
            // 木枠データの作業区分チェック
            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.KONPOKANRYO_VALUE1)
            {
                // 梱包完了です。編集できません。
                errMsgID = "A9999999033";
                return false;
            }
            else if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.SHUKKAZUMI_VALUE1)
            {
                // 出荷済です。
                errMsgID = "A9999999031";
                return false;
            }

            // 木枠明細データチェック
            DataTable dtCheck = this.GetKiwakuKonpoKiwakuMeisaiData(dbHelper, cond);
            if (dtCheck.Rows.Count < 1)
            {
                // 他端末で削除されています。
                errMsgID = "A9999999026";
                return false;
            }

            // データのバージョンチェック
            int index;
            int[] notFoundIndex = null;
            index = this.CheckSameData(dtInputKiwakuMeisai, dtCheck, out notFoundIndex, Def_T_KIWAKU_MEISAI.VERSION, Def_T_KIWAKU_MEISAI.KOJI_NO, Def_T_KIWAKU_MEISAI.CASE_ID);
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

            if (cond.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
            {
                // ----- 社内用処理 -----
                // 便間移動が必要かどうか
                if (this.IsNeedMoveShipCheck(dbHelper, cond))
                {
                    // 便間移動
                    if (!this.MoveShipExecForPallet(dbHelper, dtInputKiwakuMeisai, cond, ref errMsgID, ref args))
                    {
                        return false;
                    }
                }

                // 木枠明細データに紐付けられている出荷明細データの解除処理
                this.UpdKiwakuKonpoMeisaiShukkaMeisaiKaijyoShanai(dbHelper, cond);
                // パレットNoチェック
                if (!this.CheckKiwakuKonpoMeisaiPalletNo(dbHelper, cond, dtInputKiwakuMeisai, ref errMsgID, ref args))
                {
                    return false;
                }
            }
            else
            {
                // ----- 社外用処理 -----
                // 木枠明細データに紐付けられている出荷明細データの解除処理
                // 便間移動が必要かどうか
                if (this.IsNeedMoveShipCheck(dbHelper, cond))
                {
                    // 便間移動
                    if (!this.MoveShipExecForTag(dbHelper, dtInputShagaiKiwakuMeisai, cond, ref errMsgID, ref args))
                    {
                        return false;
                    }
                }

                this.UpdKiwakuKonpoMeisaiShukkaMeisaiKaijyoShagai(dbHelper, cond);
                // 現品TagNoチェック
                if (!this.CheckKiwakuKonpoMeisaiShagaiTagNo(dbHelper, cond, dtInputShagaiKiwakuMeisai, ref errMsgID, ref args))
                {
                    return false;
                }
            }

            // 木枠データ更新
            if (ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.SAGYO_FLAG) == SAGYO_FLAG.KIWAKUMEISAI_VALUE1)
            {
                CondK02 condKiwaku = (CondK02)cond.Clone();
                condKiwaku.SagyoFlag = SAGYO_FLAG.KONPOTOROKU_VALUE1;
                this.UpdKiwakuKonpoKiwaku(dbHelper, condKiwaku);
            }

            if (cond.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
            {
                // ----- 社内用処理 -----
                // 出荷明細データの更新
                this.UpdKiwakuKonpoMeisaiShukkaMeisaiShanai(dbHelper, dtInputKiwakuMeisai, cond);
            }
            else
            {
                // ----- 社外用処理 -----
                // 社外木枠データの削除
                this.DelShagaiKiwakuMeisai(dbHelper, cond);
                // 社外木枠データの登録
                this.InsShagaiKiwakuMeisai(dbHelper, dtInputShagaiKiwakuMeisai, cond);
                // 出荷明細データの更新
                this.UpdKiwakuKonpoMeisaiShukkaMeisaiShagai(dbHelper, dtInputShagaiKiwakuMeisai, cond);
            }

            // DESCRIPTION_1の内容差し替え
            if (dtInputKiwakuMeisai.Rows.Count > 0)
            {
                string description1 = this.GetKiwakuMeisaiDescription1(dbHelper, cond);
                UtilData.SetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, description1);
            }

            // 木枠明細データの更新
            this.UpdKiwakuKonpoMeisaiKiwakuMeisai(dbHelper, dtInputKiwakuMeisai, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region パレットNoチェック

    /// --------------------------------------------------
    /// <summary>
    /// パレットNoチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="dt">木枠明細データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckKiwakuKonpoMeisaiPalletNo(DatabaseHelper dbHelper, CondK02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            List<string> palletNoList = new List<string>();
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_1));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_2));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_3));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_4));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_5));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_6));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_7));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_8));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_9));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_10));

            CondK02 condPallet = (CondK02)cond.Clone();
            string shukkaFlag = null;
            string nonyusakiCD = null;
            for (int i = 0; i < palletNoList.Count; i++)
            {
                if (string.IsNullOrEmpty(palletNoList[i])) continue;
                condPallet.PalletNo = palletNoList[i];
                DataSet ds = this.GetKiwakuKonpoShukkaMeisaiFirstRowData(dbHelper, condPallet);
                // パレットNoの存在チェック
                if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // {0}行目のパレットNo.[{1}]はパレット情報未登録です。
                    errMsgID = "K0200050005";
                    args = new string[] { (i + 1).ToString(), palletNoList[i] };
                    return false;
                }
                // パレットNoが他の木枠で使用さえているかチェック
                string kojiNo = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KOJI_NO, string.Empty);
                string caseID = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.CASE_ID, string.Empty);
                if (!string.IsNullOrEmpty(kojiNo) && (kojiNo != condPallet.KojiNo || caseID != condPallet.CaseID))
                {
                    // {0}行目のパレットNo.[{1}]は他の木枠に梱包済です。
                    errMsgID = "K0200050006";
                    args = new string[] { (i + 1).ToString(), palletNoList[i] };
                    return false;
                }

                // 出荷日チェック
                if (ComFunc.GetFldObject(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_DATE, DBNull.Value) != DBNull.Value)
                {
                    // {0}行目のパレットNo.[{1}]は出荷済です。
                    errMsgID = "K0200050009";
                    args = new string[] { (i + 1).ToString(), palletNoList[i] };
                    return false;
                }

                // 出荷区分の退避
                if (shukkaFlag == null)
                {
                    shukkaFlag = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, string.Empty);
                }
                // 納入先コードの退避
                if (nonyusakiCD == null)
                {
                    nonyusakiCD = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, string.Empty);
                }

                // 出荷区分、納入先コードのチェック
                if (shukkaFlag != ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG) ||
                        nonyusakiCD != ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD))
                {
                    // {0}行目のパレットNo.[{1}]は異なる納入先、便です。
                    errMsgID = "K0200050007";
                    args = new string[] { (i + 1).ToString(), palletNoList[i] };
                    return false;
                }
            }

            // 同一木枠内チェック
            if (!string.IsNullOrEmpty(shukkaFlag) && !string.IsNullOrEmpty(nonyusakiCD))
            {
                CondK02 condDiff = (CondK02)cond.Clone();
                condDiff.ShukkaFlag = shukkaFlag;
                condDiff.NonyusakiCD = nonyusakiCD;
                DataTable dtDiff = this.GetKiwakuKonpoDifferentData(dbHelper, condDiff);
                foreach (DataRow drDiff in dtDiff.Rows)
                {
                    // 出荷区分、納入先コードのチェック
                    if (shukkaFlag != ComFunc.GetFld(drDiff, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG) ||
                            nonyusakiCD != ComFunc.GetFld(drDiff, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD))
                    {
                        // 木枠内に異なる納入先、便は登録できません。
                        errMsgID = "K0200050010";
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

    #region 現品TagNoチェック

    /// --------------------------------------------------
    /// <summary>
    /// 現品TagNoチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="dt">社外木枠明細データテーブル</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool CheckKiwakuKonpoMeisaiShagaiTagNo(DatabaseHelper dbHelper, CondK02 cond, DataTable dt, ref string errMsgID, ref string[] args)
    {
        try
        {
            CondK02 condShagai = (CondK02)cond.Clone();
            string shukkaFlag = null;
            string nonyusakiCD = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(ComFunc.GetFld(dt, i, Def_T_SHAGAI_KIWAKU_MEISAI.KOJI_NO))) continue;
                condShagai.ShukkaFlag = ComFunc.GetFld(dt, i, Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG);
                condShagai.NonyusakiCD = ComFunc.GetFld(dt, i, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD);
                condShagai.TagNo = ComFunc.GetFld(dt, i, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO);
                string genpinTagNo = ComFunc.GetGenpinTagNo(condShagai.ShukkaFlag, condShagai.NonyusakiCD, condShagai.TagNo);
                DataSet ds = this.GetKiwakuKonpoShukkaMeisaiFirstRowData(dbHelper, condShagai);
                // TagNoの存在チェック
                if (!ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // {0}行目の現品TAGNo.[{1}]は現品TAG情報未登録です。
                    errMsgID = "K0200070009";
                    args = new string[] { (i + 1).ToString(), genpinTagNo };
                    return false;
                }

                // 発行済チェック
                if (ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG) == JYOTAI_FLAG.SHINKI_VALUE1)
                {
                    // {0}行目の現品TAGNo.[{1}]は発行されていません。
                    errMsgID = "K0200070010";
                    args = new string[] { (i + 1).ToString(), genpinTagNo };
                    return false;
                }

                // 梱包済チェック
                if (!(ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG) == JYOTAI_FLAG.SHINKI_VALUE1 ||
                        ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG) == JYOTAI_FLAG.TAGHAKKOZUMI_VALUE1 ||
                        ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.JYOTAI_FLAG) == JYOTAI_FLAG.SHUKAZUMI_VALUE1))
                {
                    // {0}行目の現品TAGNo.[{1}]は梱包済です。
                    errMsgID = "K0200070011";
                    args = new string[] { (i + 1).ToString(), genpinTagNo };
                    return false;
                }

                // TagNoが他の木枠で使用さえているかチェック
                string kojiNo = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.KOJI_NO, string.Empty);
                string caseID = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.CASE_ID, string.Empty);
                if (!string.IsNullOrEmpty(kojiNo) && (kojiNo != condShagai.KojiNo || caseID != condShagai.CaseID))
                {
                    // {0}行目の現品TAGNo.[{1}]は他の木枠に梱包済です。
                    errMsgID = "K0200070014";
                    args = new string[] { (i + 1).ToString(), genpinTagNo };
                    return false;
                }

                // 出荷区分の退避
                if (shukkaFlag == null)
                {
                    shukkaFlag = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG, string.Empty);
                }
                // 納入先コードの退避
                if (nonyusakiCD == null)
                {
                    nonyusakiCD = ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD, string.Empty);
                }

                // 出荷区分、納入先コードのチェック
                if (shukkaFlag != ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG) ||
                        nonyusakiCD != ComFunc.GetFld(ds, Def_T_SHUKKA_MEISAI.Name, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD))
                {
                    // {0}行目の現品TAGNo.[{1}]は異なる納入先、便です。
                    errMsgID = "K0200070012";
                    args = new string[] { (i + 1).ToString(), genpinTagNo };
                    return false;
                }
            }

            // 同一木枠内チェック
            if (!string.IsNullOrEmpty(shukkaFlag) && !string.IsNullOrEmpty(nonyusakiCD))
            {
                CondK02 condDiff = (CondK02)cond.Clone();
                condDiff.ShukkaFlag = shukkaFlag;
                condDiff.NonyusakiCD = nonyusakiCD;
                DataTable dtDiff = this.GetKiwakuKonpoDifferentData(dbHelper, condDiff);
                foreach (DataRow drDiff in dtDiff.Rows)
                {
                    // 出荷区分、納入先コードのチェック
                    if (shukkaFlag != ComFunc.GetFld(drDiff, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG) ||
                            nonyusakiCD != ComFunc.GetFld(drDiff, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD))
                    {
                        // 木枠内に異なる納入先、便は登録できません。
                        errMsgID = "K0200050010";
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

    #region 便間移動データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="ds">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMoveShipData(DatabaseHelper dbHelper, DataSet ds, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        if (cond.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
        {
            return this.GetMoveShipPalletData(dbHelper, ds.Tables[Def_T_KIWAKU_MEISAI.Name], cond, ref errMsgID, ref args);
        }
        else
        {
            return this.GetMoveShipTagData(dbHelper, ds.Tables[Def_T_SHAGAI_KIWAKU_MEISAI.Name], cond, ref errMsgID, ref args);
        }
    }

    #endregion

    #endregion

    #region K0200050:木枠梱包明細登録

    #region 便間移動(Pallet)

    #region 便間移動データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtInputKiwakuMeisai">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMoveShipPalletData(DatabaseHelper dbHelper, DataTable dtInputKiwakuMeisai, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 便間移動対象として、木枠の物件Noと同じ物件Noが設定されていて
            // 木枠の納入先コードと異なる納入先コードが設定されているパレットを取得する

            // 木枠取得
            var dtKiwaku = this.GetKiwakuDataForMoveShip(dbHelper, cond);
            DataTable dtPallet = new DataTable();
            if (UtilData.ExistsData(dtKiwaku))
            {
                var kiwakuBukkenNo = ComFunc.GetFld(dtKiwaku, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                var kiwakuNonyusakiCd = ComFunc.GetFld(dtKiwaku, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                var listPalletNo = new List<string>();
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_1));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_2));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_3));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_4));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_5));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_6));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_7));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_8));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_9));
                listPalletNo.Add(ComFunc.GetFld(dtInputKiwakuMeisai, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_10));

                var condPallet = (CondK02)cond.Clone();
                for (int palletIdx = 0; palletIdx < listPalletNo.Count; palletIdx++)
                {
                    if (string.IsNullOrEmpty(listPalletNo[palletIdx]))
                    {
                        continue;
                    }

                    condPallet.PalletNo = listPalletNo[palletIdx];
                    var dtTmpPallet = this.GetPalletDataForMoveShip(dbHelper, condPallet);
                    if (!UtilData.ExistsData(dtTmpPallet))
                    {
                        continue;
                    }

                    var palletBukkenNo = UtilData.GetFld(dtTmpPallet, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                    if (palletBukkenNo != kiwakuBukkenNo)
                    {
                        // 木枠の物件Noとパレットの物件Noが異なる場合はエラー
                        errMsgID = "K0200050014";
                        return null;
                    }

                    var palletNonyusakiCd = UtilData.GetFld(dtTmpPallet, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    if (palletNonyusakiCd != kiwakuNonyusakiCd)
                    {
                        // 木枠の納入先コードとパレットの納入先コードが異なる場合は便間移動対象
                        if (!UtilData.ExistsData(dtPallet))
                        {
                            dtPallet = dtTmpPallet.Clone();
                            dtPallet.TableName = ComDefine.DTTBL_MOVE_SHIP_PALLET;
                        }
                        dtPallet.Rows.Add(dtTmpPallet.Rows[0].ItemArray);
                    }
                }
            }

            var ds = new DataSet();
            ds.Merge(dtPallet);
            ds.Merge(dtKiwaku);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 便間移動

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtInputKiwaku">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool MoveShipExecForPallet(DatabaseHelper dbHelper, DataTable dtInputKiwaku, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        bool ret = false;
        try
        {
            // 便間移動データ取得
            var ds = this.GetMoveShipPalletData(dbHelper, dtInputKiwaku, cond, ref errMsgID, ref args);
            if (!UtilData.ExistsData(ds, ComDefine.DTTBL_MOVE_SHIP_PALLET))
            {
                return true;
            }

            // 移動先納入先コード
            var dstNonyusakiCd = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            // 移動元データ
            var dtSrc = ds.Tables[ComDefine.DTTBL_MOVE_SHIP_PALLET];

            // 出荷明細データ取得＆行ロック
            var dtShukkaMeisai = this.LockShukkaMeisaiForPalletMoveShip(dbHelper, dtSrc);

            // パレットリスト管理データ取得＆行ロック
            var dtPalletList = this.LockPalletListManageForPalletMoveShip(dbHelper, dtSrc);

            // BOXリスト管理データ取得＆行ロック
            var dtBoxList = this.LockBoxListManageForPalletMoveShip(dbHelper, dtShukkaMeisai);

            // パレットリスト管理データ更新
            this.UpdPalletListManageForPalletMoveShip(dbHelper, dtPalletList, cond, dstNonyusakiCd);

            // BOXリスト管理データ更新
            this.UpdBoxListManageForPalletMoveShip(dbHelper, dtBoxList, cond, dstNonyusakiCd);

            // 出荷明細データ更新
            this.UpdShukkaMeisaiForPalletMoveShip(dbHelper, dtShukkaMeisai, cond, dstNonyusakiCd);

            ret = true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        return ret;
    }
    
    #endregion

    #endregion

    #endregion

    #region K0200070:木枠梱包明細登録(社外)

    #region 便間移動データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtInputKiwakuMeisai">更新データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMoveShipTagData(DatabaseHelper dbHelper, DataTable dtInputShagaiKiwakuMeisai, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            // 便間移動対象として、木枠の物件Noと同じ物件Noが設定されていて
            // 木枠の納入先コードと異なる納入先コードが設定されているタグを取得する

            // 木枠取得
            var dtKiwaku = this.GetKiwakuDataForMoveShip(dbHelper, cond);
            DataTable dtTag = new DataTable();
            if (UtilData.ExistsData(dtKiwaku))
            {
                var kiwakuBukkenNo = ComFunc.GetFld(dtKiwaku, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                var kiwakuNonyusakiCd = ComFunc.GetFld(dtKiwaku, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);

                var condTag = (CondK02)cond.Clone();
                foreach (DataRow dr in dtInputShagaiKiwakuMeisai.Rows)
                {
                    if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.KOJI_NO)))
                    {
                        continue;
                    }

                    condTag.ShukkaFlag = ComFunc.GetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG);
                    condTag.NonyusakiCD = ComFunc.GetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD);
                    condTag.TagNo = ComFunc.GetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO);
                    var dsTmpTag = this.GetKiwakuKonpoShukkaMeisaiFirstRowData(dbHelper, condTag);
                    if (!ComFunc.IsExistsData(dsTmpTag, Def_T_SHUKKA_MEISAI.Name))
                    {
                        continue;
                    }

                    var tagBukkenNo = UtilData.GetFld(dsTmpTag, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.BUKKEN_NO);
                    if (tagBukkenNo != kiwakuBukkenNo)
                    {
                        // 木枠の物件Noとタグの物件Noが異なる場合はエラー
                        errMsgID = "K0200070017";
                        return null;
                    }

                    var tagNonyusakiCd = UtilData.GetFld(dsTmpTag, Def_T_SHUKKA_MEISAI.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    if (tagNonyusakiCd != kiwakuNonyusakiCd)
                    {
                        // 木枠の納入先コードとタグの納入先コードが異なる場合は便間移動対象
                        var dtShukkaMeisai = dsTmpTag.Tables[Def_T_SHUKKA_MEISAI.Name];
                        if (!UtilData.ExistsData(dtTag))
                        {
                            dtTag = dtShukkaMeisai.Clone();
                            dtTag.TableName = ComDefine.DTTBL_MOVE_SHIP_TAG;
                        }
                        dtTag.Rows.Add(dtShukkaMeisai.Rows[0].ItemArray);
                    }
                }
            }

            var ds = new DataSet();
            ds.Merge(dtTag);
            ds.Merge(dtKiwaku);
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 便間移動

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dtInputShagaiKiwaku">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update>K.Tsutsumi 2019/09/20 社外用木枠明細データを移行先へ移動してしまう不具合修正</update>
    /// --------------------------------------------------
    public bool MoveShipExecForTag(DatabaseHelper dbHelper, DataTable dtInputShagaiKiwaku, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        bool ret = false;
        try
        {
            // 便間移動データ取得
            var ds = this.GetMoveShipTagData(dbHelper, dtInputShagaiKiwaku, cond, ref errMsgID, ref args);
            if (!UtilData.ExistsData(ds, ComDefine.DTTBL_MOVE_SHIP_TAG))
            {
                return true;
            }

            // 移動先納入先コード
            var dstNonyusakiCd = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_M_NONYUSAKI.NONYUSAKI_CD);
            // 移動元データ
            var dtSrc = ds.Tables[ComDefine.DTTBL_MOVE_SHIP_TAG];

            // 出荷明細データ取得＆行ロック
            var dtShukkaMeisai = this.LockShukkaMeisaiForTagMoveShip(dbHelper, dtSrc);

            // 出荷明細データ更新
            this.UpdShukkaMeisaiForTagMoveShip(dbHelper, dtShukkaMeisai, cond, dstNonyusakiCd);

            //// 変更後の納入先で更新
            //foreach (DataRow dr in dtInputShagaiKiwaku.Rows)
            //{
            //    UtilData.SetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD, dstNonyusakiCd);
            //}

            ret = true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        return ret;
    }

    #endregion

    #endregion

    #region K0200110:木枠まとめ発行


    #region 木枠まとめデータの取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠まとめデータの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="errMsgID">メッセージID</param>
    /// <param name="args">パラメーター</param>
    /// <returns>表示データ(T_KIWAKU, T_KIWAKU_MEISAI, M_NONYUSAKI)</returns>
    /// <create>D.Okumura 2019/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetKiwakuMatomeData(DatabaseHelper dbHelper, CondK02 cond, ref string errMsgID, ref string[] args)
    {
        try
        {
            DataTable dtKiwaku = this.GetKiwakuKonpoKiwakuData(dbHelper, cond);
            if (dtKiwaku.Rows.Count < 1)
            {
                // 該当する工事識別Noはありません。
                errMsgID = "A9999999030";
                return null;
            }

            CondK02 condK02 = (CondK02)cond.Clone();
            condK02.KojiNo = ComFunc.GetFld(dtKiwaku, 0, Def_T_KIWAKU.KOJI_NO);
            DataTable dtKiwakuMeisai = this.GetKiwakuKonpoKiwakuMeisaiData(dbHelper, condK02);
            if (dtKiwakuMeisai.Rows.Count < 1)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return null;
            }
            // 木枠データから出荷明細を参照し、納入先/物件を取得する(Distinct)
            DataTable dtBukken = this.GetKiwakuMatomeBukkenData(dbHelper, condK02);
            if (dtBukken.Rows.Count < 1)
            {
                // 該当の明細は存在しません。
                errMsgID = "A9999999022";
                return null;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtKiwaku);
            ds.Tables.Add(dtKiwakuMeisai);
            ds.Tables.Add(dtBukken);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion


    #region 木枠まとめ印刷用データの取得
    /// --------------------------------------------------
    /// <summary>
    /// 木枠まとめ印刷用データの取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">取得用データテーブル(T_KIWAKU_MEISAI)</param>
    /// <param name="language">言語</param>
    /// <returns>印刷用データ(T_KIWAKU_MEISAI, KOJI_NO, T_SHUKKA_MEISAI)</returns>
    /// <create>D.Okumura 2019/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetMatomePackingMeisai(DatabaseHelper dbHelper, DataTable dt, String language)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            DataTable dtt = new DataTable(Def_T_KIWAKU.KOJI_NO);
            DataTable dtm = new DataTable(Def_T_SHUKKA_MEISAI.Name);

            foreach (string kojiNo in dt.AsEnumerable()
                .Select(dr => ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.KOJI_NO))
                .Distinct())
            {
                dtt.Merge(GetPackingKiwaku(dbHelper, kojiNo));
            }
                
                
            foreach (DataRow dr in dt.Rows)
            {
                string kojiNo = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.KOJI_NO);
                string caseID = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.CASE_ID);
                string printCaseNo = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                DataTable dtData = GetPackingData(dbHelper, kojiNo, caseID, language);
                // 印刷C/NOを上書きする
                foreach (DataRow drData in dtData.Rows)
                {
                    drData.SetField<string>(Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, printCaseNo);
                }
                foreach (DataRow drMeisai in dtt.AsEnumerable()
                    .Where(drMeisai => string.Equals(ComFunc.GetFld(drMeisai, Def_T_KIWAKU_MEISAI.KOJI_NO), kojiNo))
                    .Where(drMeisai => string.Equals(ComFunc.GetFld(drMeisai, Def_T_KIWAKU_MEISAI.CASE_ID), caseID))
                    )
                {
                    drMeisai.SetField<string>(Def_T_KIWAKU_MEISAI.PRINT_CASE_NO, printCaseNo);
                }
                dtm.Merge(dtData);
            }

            ds.Tables.Add(dtt);
            ds.Tables.Add(dtm);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion
    #endregion //K0200110:木枠まとめ発行

    #endregion

    #region SQL実行

    #region K0200020:BOXリスト発行

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// BoxNoから納入先データを取得します。
    /// BoxNoが無ければBoxListを発行していない納入先データを取得します。
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="boxNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/15</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応 ST_NO->M_NOに変更</update>
    /// <update>K.Tsutsumi 2020/10/02 M-NO２行表示対応</update>
    /// <update>J.Chen 2023/02/27 M-NO追加</update>
    /// --------------------------------------------------
    public DataTable GetBoxNo(DatabaseHelper dbHelper, string boxNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BOXLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        1 AS INSATU");
            sb.ApdL("      , TBM.BOX_NO");
            sb.ApdL("      , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("      , NON.SHIP");
            sb.ApdL("      , TBM.SHUKKA_FLAG");
            sb.ApdL("      , TBM.LISTHAKKO_FLAG");
            sb.ApdL("      , '' AS AREA");
            sb.ApdL("      , '' AS AREA_1");
            sb.ApdL("      , '' AS AREA_2");
            sb.ApdL("      , '' AS FLOOR");
            sb.ApdL("      , '' AS FLOOR_1");
            sb.ApdL("      , '' AS FLOOR_2");
            sb.ApdL("      , '' AS KISHU");
            sb.ApdL("      , '' AS M_NO");
            sb.ApdL("      , '' AS M_NO_1");
            sb.ApdL("      , '' AS M_NO_2");
            sb.ApdL("      , '' AS NONYUSAKI_NAME_1");
            sb.ApdL("      , '' AS NONYUSAKI_NAME_2");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BOXLIST_MANAGE TBM");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI NON ON NON.SHUKKA_FLAG = TBM.SHUKKA_FLAG");
            sb.ApdL("                         AND NON.NONYUSAKI_CD = TBM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = NON.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            if (!string.IsNullOrEmpty(boxNo))
            {
                sb.ApdN("   AND TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParameter.NewDbParameter("BOX_NO", boxNo));
            }
            else
            {
                sb.ApdL("   AND TBM.LISTHAKKO_FLAG = 0");
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       TBM.BOX_NO");

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
    /// BoxNoから明細を取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="boxNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/16</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応 ST_NO->M_NOに変更</update>
    /// <update>D.Okumura 2020/06/29 EFA_SMS-119 CODE = NULL のときに Box List の Job No. が印字されない件を修正</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetBoxMeisai(DatabaseHelper dbHelper, string boxNo)
    {
        DataTable dt = new DataTable(boxNo);

        StringBuilder sb = new StringBuilder();
        DbParamCollection paramCollection = new DbParamCollection();
        string fieldName = string.Empty;
        INewDbParameterBasic iNewParameter = dbHelper;

        // SQL文
        sb.ApdL("SELECT ");
        sb.ApdL("        ROW_NUMBER() OVER(PARTITION BY BOX_NO ORDER BY TAG_NO) AS ROW_NO");
        sb.ApdL("      , BOX_NO");
        sb.ApdL("      , TAG_NO");
        sb.ApdL("      , ZUMEN_OIBAN");
        sb.ApdL("      , AREA");
        sb.ApdL("      , FLOOR");
        sb.ApdL("      , KISHU");
        sb.ApdL("      , M_NO");
        sb.ApdL("      , HINMEI_JP");
        sb.ApdL("      , HINMEI");
        sb.ApdL("      , ZUMEN_KEISHIKI");
        sb.ApdL("      , KUWARI_NO");
        sb.ApdL("      , NUM");
        sb.ApdL("      , (ISNULL(SEIBAN, '') + ISNULL(CODE,'')) AS JOB_NO");

        sb.ApdL("  FROM ");
        sb.ApdL("       T_SHUKKA_MEISAI");
        sb.ApdL(" WHERE ");
        sb.ApdL("       1 = 1");
        sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
        // TAGNO順
        sb.ApdL(" ORDER BY");
        sb.ApdL("       TAG_NO");

        paramCollection.Add(iNewParameter.NewDbParameter("BOX_NO", boxNo));
        // SQL実行
        dbHelper.Fill(sb.ToString(), paramCollection, dt);

        return dt;
    }

    #endregion

    #region UPDATE
    /// --------------------------------------------------
    /// <summary>
    /// ボックスリストマネージャーアップデート
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="boxList"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxNoKanri(DatabaseHelper dbHelper, CondK02 cond, DataTable boxList)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BOXLIST_MANAGE ");
            sb.ApdL("SET");
            sb.ApdL("       LISTHAKKO_FLAG = 1");
            sb.ApdN("     , LISTHAKKO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysTimestamp).ApdL(", 111)");
            sb.ApdN("     , LISTHAKKO_USER = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE ");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in boxList.Rows)
            {
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetCreateUserName(cond)));

                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.BOX_NO, "")));

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

    #region K0200030:パレットリスト発行

    #region SELECT

    /// --------------------------------------------------
    /// <summary>
    /// パレットNoから納入先・便を取得します
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="palletNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPalletNo(DatabaseHelper dbHelper, string palletNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_PALLETLIST_MANAGE.Name);

            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        1 AS INSATU");
            sb.ApdL("      , TBM.PALLET_NO");
            sb.ApdL("      , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("      , NON.SHIP");

            sb.ApdL("      , TBM.SHUKKA_FLAG");
            sb.ApdL("      , TBM.LISTHAKKO_FLAG");
            sb.ApdL("      , '' AS COUNT_BOX");

            sb.ApdL("  FROM ");
            sb.ApdL("       T_PALLETLIST_MANAGE TBM");
            sb.ApdL("  LEFT JOIN M_NONYUSAKI NON ON NON.SHUKKA_FLAG = TBM.SHUKKA_FLAG");
            sb.ApdL("                         AND NON.NONYUSAKI_CD = TBM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = NON.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = NON.BUKKEN_NO");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            if (!string.IsNullOrEmpty(palletNo))
            {
                sb.ApdN("   AND TBM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParameter.NewDbParameter("PALLET_NO", palletNo));
            }
            else
            {
                sb.ApdL("   AND TBM.LISTHAKKO_FLAG = 0");
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       TBM.PALLET_NO");

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
    /// 印刷時パレットNoからボックスNoリストを取得します
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="palletNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPalletMeisai(DatabaseHelper dbHelper, string palletNo)
    {
        try
        {
            DataTable dt = new DataTable(palletNo);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        ROW_NUMBER() OVER(PARTITION BY PALLET_NO ORDER BY PALLET_NO) AS ROW_NO");
            sb.ApdL("      , BOX_NO");
            sb.ApdL("      , SEIBAN");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , SEIBAN");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BOX_NO");
            sb.ApdL("     , SEIBAN");

            paramCollection.Add(iNewParameter.NewDbParameter("PALLET_NO", palletNo));

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

    #region UPDATE

    /// --------------------------------------------------
    /// <summary>
    /// パレットリストマネージャーアップデート
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="palletList"></param>
    /// <create>H.Tsunamura 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletNoKanri(DatabaseHelper dbHelper, CondK02 cond, DataTable palletList)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE ");
            sb.ApdL("SET");
            sb.ApdL("       LISTHAKKO_FLAG = 1");
            sb.ApdN("     , LISTHAKKO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysTimestamp).ApdL(", 111)");
            sb.ApdN("     , LISTHAKKO_USER = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE ");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            foreach (DataRow dr in palletList.Rows)
            {
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_ID", this.GetCreateUserID(cond)));
                paramCollection.Add(iNewParameter.NewDbParameter("UPDATE_USER_NAME", this.GetCreateUserName(cond)));

                // バインド変数設定
                paramCollection.Add(dbHelper.NewDbParameter("PALLET_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.PALLET_NO, "")));

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

    #region K0200040:木枠梱包登録 & K0200060:木枠梱包登録(社外)

    #region SELECT

    #region 木枠データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
    /// --------------------------------------------------
    public DataTable GetKiwakuKonpoKiwakuData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_KIWAKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TK.TOROKU_FLAG");
            sb.ApdL("     , TK.SAGYO_FLAG");
            sb.ApdL("     , TK.CASE_MARK_FILE");
            sb.ApdL("     , TK.DELIVERY_NO");
            sb.ApdL("     , TK.PORT_OF_DESTINATION");
            sb.ApdL("     , TK.AIR_BOAT");
            sb.ApdL("     , TK.DELIVERY_DATE");
            sb.ApdL("     , TK.DELIVERY_POINT");
            sb.ApdL("     , TK.FACTORY");
            sb.ApdL("     , TK.REMARKS");
            sb.ApdL("     , TK.VERSION");
            sb.ApdL("     , TK.NONYUSAKI_CD");
            sb.ApdL("     , TK.INSERT_TYPE");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.KojiNo))
            {
                sb.ApdN("   AND TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            }
            if (!string.IsNullOrEmpty(cond.KojiName))
            {
                sb.ApdN("   AND TK.KOJI_NAME = ").ApdN(this.BindPrefix).ApdL("KOJI_NAME");
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NAME", cond.KojiName));
            }
            if (cond.Ship != null)
            {
                sb.ApdN("   AND TK.SHIP = ").ApdN(this.BindPrefix).ApdL("SHIP");
                paramCollection.Add(iNewParam.NewDbParameter("SHIP", cond.Ship));
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

    #region 木枠明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update>D.Okumura 2019/09/02 印刷ケース番号を取得</update>
    /// --------------------------------------------------
    public DataTable GetKiwakuKonpoKiwakuMeisaiData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_KIWAKU_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TK.SAGYO_FLAG");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TKM.STYLE");
            sb.ApdL("     , TKM.MMNET");
            sb.ApdL("     , TKM.MOKUZAI_JYURYO");
            sb.ApdL("     , TKM.GROSS_W");
            sb.ApdL("     , TKM.NET_W");
            sb.ApdL("     , TKM.ITEM");
            sb.ApdL("     , TKM.DESCRIPTION_1");
            sb.ApdL("     , TKM.DESCRIPTION_2");
            sb.ApdL("     , TKM.DIMENSION_L");
            sb.ApdL("     , TKM.DIMENSION_W");
            sb.ApdL("     , TKM.DIMENSION_H");
            sb.ApdL("     , TKM.PALLET_NO_1");
            sb.ApdL("     , TKM.PALLET_NO_2");
            sb.ApdL("     , TKM.PALLET_NO_3");
            sb.ApdL("     , TKM.PALLET_NO_4");
            sb.ApdL("     , TKM.PALLET_NO_5");
            sb.ApdL("     , TKM.PALLET_NO_6");
            sb.ApdL("     , TKM.PALLET_NO_7");
            sb.ApdL("     , TKM.PALLET_NO_8");
            sb.ApdL("     , TKM.PALLET_NO_9");
            sb.ApdL("     , TKM.PALLET_NO_10");
            sb.ApdL("     , TKM.SHUKKA_DATE");
            sb.ApdL("     , TKM.VERSION");
            sb.ApdL("     , TKM.PRINT_CASE_NO");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" INNER JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            if (!string.IsNullOrEmpty(cond.CaseID))
            {
                sb.ApdN("   AND TKM.CASE_ID =").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKM.CASE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region パレットNoが全てNullの木枠明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// パレットNoが全てNullの木枠明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetKiwakuMeisaiPalletNoNullData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TKM.KOJI_NO");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TKM.STYLE");
            sb.ApdL("     , TKM.MMNET");
            sb.ApdL("     , TKM.MOKUZAI_JYURYO");
            sb.ApdL("     , TKM.GROSS_W");
            sb.ApdL("     , TKM.NET_W");
            sb.ApdL("     , TKM.ITEM");
            sb.ApdL("     , TKM.DESCRIPTION_1");
            sb.ApdL("     , TKM.DESCRIPTION_2");
            sb.ApdL("     , TKM.PALLET_NO_1");
            sb.ApdL("     , TKM.PALLET_NO_2");
            sb.ApdL("     , TKM.PALLET_NO_3");
            sb.ApdL("     , TKM.PALLET_NO_4");
            sb.ApdL("     , TKM.PALLET_NO_5");
            sb.ApdL("     , TKM.PALLET_NO_6");
            sb.ApdL("     , TKM.PALLET_NO_7");
            sb.ApdL("     , TKM.PALLET_NO_8");
            sb.ApdL("     , TKM.PALLET_NO_9");
            sb.ApdL("     , TKM.PALLET_NO_10");
            sb.ApdL("     , TKM.SHUKKA_DATE");
            sb.ApdL("     , TKM.VERSION");
            sb.ApdL("  FROM T_KIWAKU_MEISAI TKM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TKM.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            if (!string.IsNullOrEmpty(cond.CaseID))
            {
                sb.ApdN("   AND TKM.CASE_ID =").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            }
            sb.ApdL("   AND TKM.PALLET_NO_1 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_2 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_3 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_4 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_5 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_6 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_7 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_8 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_9 IS NULL");
            sb.ApdL("   AND TKM.PALLET_NO_10 IS NULL");
            sb.ApdL("   AND NOT EXISTS(");
            sb.ApdL("       SELECT 1");
            sb.ApdL("         FROM T_SHAGAI_KIWAKU_MEISAI TSKM");
            sb.ApdL("        WHERE TKM.KOJI_NO = TSKM.KOJI_NO");
            sb.ApdL("          AND TKM.CASE_ID = TSKM.CASE_ID");
            sb.ApdL("       )");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKM.CASE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 工事識別NO、内部管理用キー単位の社外木枠明細データが0件数のデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// 工事識別NO、内部管理用キー単位の社外木枠明細データが0件数のデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>社外木枠明細データが0件の工事識別NO、内部管理用キー単位のデータ</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetShagaiKiwakuMeisaiNullData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TKM.KOJI_NO");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , COUNT(TSKM.TAG_NO) AS CNT");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" INNER JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_SHAGAI_KIWAKU_MEISAI TSKM ON TKM.KOJI_NO = TSKM.KOJI_NO");
            sb.ApdL("                                       AND TKM.CASE_ID = TSKM.CASE_ID");
            sb.ApdN(" WHERE TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TKM.KOJI_NO");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("HAVING COUNT(TSKM.TAG_NO) = 0");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 木枠データ更新処理

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ更新処理
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/07/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoKiwaku(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_KIWAKU");
            sb.ApdL("SET");
            sb.ApdN("       SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL("SAGYO_FLAG");
            if (cond.UpdateDate == null || cond.UpdateDate == DBNull.Value)
            {
                sb.ApdN("     , UPDATE_DATE = ").ApdL(this.SysTimestamp);
            }
            else
            {
                sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            }
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND TOROKU_FLAG = ").ApdN(this.BindPrefix).ApdL("TOROKU_FLAG");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG", cond.SagyoFlag));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("TOROKU_FLAG", cond.TorokuFlag));

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
    #endregion

    #endregion

    #region K0200050:木枠梱包明細登録 & K0200070:木枠梱包明細登録(社外)

    #region SELECT

    #region 社外木枠明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetKiwakuKonpoShagaiKiwakuMeisaiData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHAGAI_KIWAKU_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TSKM.SHUKKA_FLAG");
            sb.ApdL("     , TSKM.NONYUSAKI_CD");
            sb.ApdL("     , TSKM.TAG_NO");
            sb.ApdL("     , TSKM.VERSION");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" INNER JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            sb.ApdL(" INNER JOIN T_SHAGAI_KIWAKU_MEISAI TSKM ON TKM.KOJI_NO = TSKM.KOJI_NO");
            sb.ApdL("                                       AND TKM.CASE_ID = TSKM.CASE_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            if (!string.IsNullOrEmpty(cond.CaseID))
            {
                sb.ApdN("   AND TKM.CASE_ID =").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKM.CASE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 出荷明細データ1件取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ1件取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>H.Tajimi 2018/12/18 木枠梱包業務改善</update>
    /// <update>K.Tsutsumi 2019/09/20 社外用木枠明細の納入先はT_SHUKKA_MEISAI.TAG_NONYUSAKI_CDと比較する必要がある</update>
    /// --------------------------------------------------
    public DataSet GetKiwakuKonpoShukkaMeisaiFirstRowData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.KISHU");
            sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.KOJI_NO");
            sb.ApdL("     , TSM.CASE_ID");
            sb.ApdL("     , TSM.SHUKKA_DATE");
            sb.ApdL("     , TSM.VERSION");
            sb.ApdL("     , TSM.M_NO");
            sb.ApdL("     , MNS.BUKKEN_NO");
            sb.ApdL("  FROM");
            sb.ApdL("     (");
            sb.ApdL("       SELECT");
            sb.ApdL("              ROW_NUMBER() OVER(PARTITION BY PALLET_NO ORDER BY TAG_NO) AS ROW_NO");
            sb.ApdL("            , SHUKKA_FLAG");
            sb.ApdL("            , NONYUSAKI_CD");
            sb.ApdL("            , TAG_NO");
            sb.ApdL("            , AR_NO");
            sb.ApdL("            , KISHU");
            sb.ApdL("            , ST_NO");
            sb.ApdL("            , JYOTAI_FLAG");
            sb.ApdL("            , SHUKA_DATE");
            sb.ApdL("            , BOX_NO");
            sb.ApdL("            , PALLET_NO");
            sb.ApdL("            , KOJI_NO");
            sb.ApdL("            , CASE_ID");
            sb.ApdL("            , SHUKKA_DATE");
            sb.ApdL("            , VERSION");
            sb.ApdL("            , M_NO");
            sb.ApdL("         FROM T_SHUKKA_MEISAI");
            sb.ApdL("        WHERE");
            sb.ApdL("              1 = 1");
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                sb.ApdN("          AND PALLET_NO =").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            }
            if (!string.IsNullOrEmpty(cond.ShukkaFlag))
            {
                sb.ApdN("          AND SHUKKA_FLAG =").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            }
            if (!string.IsNullOrEmpty(cond.NonyusakiCD))
            {
                sb.ApdN("          AND TAG_NONYUSAKI_CD =").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            }
            if (!string.IsNullOrEmpty(cond.TagNo))
            {
                sb.ApdN("          AND TAG_NO =").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", cond.TagNo));
            }
            sb.ApdL("     ) TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                       AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       TSM.ROW_NO = 1");

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

    #region 出荷明細データ同一工事識別管理NOの納入先、便、ARNo.取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ同一工事識別管理NOの納入先、便、ARNo.取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/08/02</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetKiwakuKonpoDifferentData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            sb.ApdL("   AND (");
            sb.ApdN("       MNS.SHUKKA_FLAG <> ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("    OR MNS.NONYUSAKI_CD <> ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdL("       )");
            sb.ApdN("   AND TSM.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.SHIP");

            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 出荷明細データ M_NO取得

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ M_NO取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>H.Tajimi 2015/11/24</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable GetKiwakuKonpoShukkaMeisaiMNo(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TSM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND TSM.M_NO IS NOT NULL");
            if (!string.IsNullOrEmpty(cond.CaseID))
            {
                sb.ApdN("   AND TSM.CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            }
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.M_NO");

            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 木枠明細データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データ取得＆ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠明細データ</returns>
    /// <create>Y.Higuchi 2010/07/29</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockKiwakuMeisai(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_KIWAKU_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TKM.CASE_ID");
            sb.ApdL("     , TKM.CASE_NO");
            sb.ApdL("     , TKM.PALLET_NO_1");
            sb.ApdL("     , TKM.PALLET_NO_2");
            sb.ApdL("     , TKM.PALLET_NO_3");
            sb.ApdL("     , TKM.PALLET_NO_4");
            sb.ApdL("     , TKM.PALLET_NO_5");
            sb.ApdL("     , TKM.PALLET_NO_6");
            sb.ApdL("     , TKM.PALLET_NO_7");
            sb.ApdL("     , TKM.PALLET_NO_8");
            sb.ApdL("     , TKM.PALLET_NO_9");
            sb.ApdL("     , TKM.PALLET_NO_10");
            sb.ApdL("     , TKM.VERSION");
            sb.ApdL("  FROM T_KIWAKU_MEISAI TKM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TKM.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND TKM.CASE_ID =").ApdN(this.BindPrefix).ApdL("CASE_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));

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

    #region 便間移動関連メソッド

    #region 便間移動チェックの要否取得

    /// --------------------------------------------------
    /// <summary>
    /// 便間移動チェックの要否取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>true:要/false:不要</returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update>H.Tajimi 2019/09/10 本体用とAR用の登録を分離</update>
    /// --------------------------------------------------
    public bool IsNeedMoveShipCheck(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            if (cond.KiwakuKonpoMoveShipFlag != KIWAKU_KONPO_MOVE_SHIP.ENABLE_VALUE1)
            {
                // 便間移動チェックSQL実行不要
                return false;
            }

            if (!string.IsNullOrEmpty(cond.KiwakuNonyusakiCD))
            {
                if (cond.KiwakuInsertType == KIWAKU_INSERT_TYPE.AR_VALUE1 || cond.KiwakuInsertType == KIWAKU_INSERT_TYPE.REGULAR_VALUE1)
                {
                    // 登録(AR)、登録(本体)の場合は便間移動不可なのでここで終了(便間移動チェックSQL実行不要)
                    return false;
                }
            }

            DataTable dt = new DataTable(Def_T_SHAGAI_KIWAKU_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TK.NONYUSAKI_CD");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdL("   AND TK.NONYUSAKI_CD IS NOT NULL");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region 木枠データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>木枠データ</returns>
    /// <create>H.Tajimi 2018/12/18</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetKiwakuDataForMoveShip(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_KIWAKU.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            //sb.ApdL("SELECT");
            //sb.ApdL("       TK.KOJI_NO");
            //sb.ApdL("     , TK.KOJI_NAME");
            //sb.ApdL("     , TK.SHIP");
            //sb.ApdL("     , TK.SAGYO_FLAG");
            //sb.ApdL("     , TKM.CASE_ID");
            //sb.ApdL("     , TKM.CASE_NO");
            //sb.ApdL("     , TKM.STYLE");
            //sb.ApdL("     , TKM.MMNET");
            //sb.ApdL("     , TKM.MOKUZAI_JYURYO");
            //sb.ApdL("     , TKM.GROSS_W");
            //sb.ApdL("     , TKM.NET_W");
            //sb.ApdL("     , TKM.ITEM");
            //sb.ApdL("     , TKM.DESCRIPTION_1");
            //sb.ApdL("     , TKM.DESCRIPTION_2");
            //sb.ApdL("     , TKM.DIMENSION_L");
            //sb.ApdL("     , TKM.DIMENSION_W");
            //sb.ApdL("     , TKM.DIMENSION_H");
            //sb.ApdL("     , TKM.PALLET_NO_1");
            //sb.ApdL("     , TKM.PALLET_NO_2");
            //sb.ApdL("     , TKM.PALLET_NO_3");
            //sb.ApdL("     , TKM.PALLET_NO_4");
            //sb.ApdL("     , TKM.PALLET_NO_5");
            //sb.ApdL("     , TKM.PALLET_NO_6");
            //sb.ApdL("     , TKM.PALLET_NO_7");
            //sb.ApdL("     , TKM.PALLET_NO_8");
            //sb.ApdL("     , TKM.PALLET_NO_9");
            //sb.ApdL("     , TKM.PALLET_NO_10");
            //sb.ApdL("     , TKM.SHUKKA_DATE");
            //sb.ApdL("     , TKM.VERSION");
            //sb.ApdL("     , MNS.SHUKKA_FLAG");
            //sb.ApdL("     , MNS.NONYUSAKI_CD");
            //sb.ApdL("     , MNS.BUKKEN_NO");
            //sb.ApdL("  FROM T_KIWAKU TK");
            //sb.ApdL(" INNER JOIN T_KIWAKU_MEISAI TKM ON TK.KOJI_NO = TKM.KOJI_NO");
            //sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            //sb.ApdL("                           AND TK.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            //sb.ApdL(" WHERE");
            //sb.ApdN("       TK.KOJI_NO =").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            //if (!string.IsNullOrEmpty(cond.CaseID))
            //{
            //    sb.ApdN("   AND TKM.CASE_ID =").ApdN(this.BindPrefix).ApdL("CASE_ID");
            //    paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            //}
            //sb.ApdL(" ORDER BY");
            //sb.ApdL("       TKM.CASE_NO");

            sb.ApdL("SELECT");
            sb.ApdL("       TK.KOJI_NO");
            sb.ApdL("     , TK.KOJI_NAME");
            sb.ApdL("     , TK.SHIP");
            sb.ApdL("     , TK.SAGYO_FLAG");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.BUKKEN_NO");
            sb.ApdL("  FROM T_KIWAKU TK");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TK.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TK.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #region INSERT

    #region 社外木枠明細データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠明細データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsShagaiKiwakuMeisai(DatabaseHelper dbHelper, DataTable dt, CondK02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_SHAGAI_KIWAKU_MEISAI");
            sb.ApdL("(");
            sb.ApdL("       KOJI_NO");
            sb.ApdL("     , CASE_ID");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.KOJI_NO))) continue;
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", ComFunc.GetFldObject(dr, Def_T_SHUKKA_MEISAI.KOJI_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.CASE_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO, DBNull.Value)));
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

    #endregion

    #region UPDATE

    #region 出荷明細データの木枠梱包解除(社内)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの木枠梱包解除(社内)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoMeisaiShukkaMeisaiKaijyoShanai(DatabaseHelper dbHelper, CondK02 cond)
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
            sb.ApdL("       JYOTAI_FLAG = '4'");
            sb.ApdL("     , KOJI_NO = NULL");
            sb.ApdL("     , CASE_ID = NULL");
            sb.ApdL("     , KIWAKUKONPO_DATE = NULL");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("   AND JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdL("   AND PALLET_NO IS NOT NULL");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.KIWAKUKONPO_VALUE1));

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

    #region 出荷明細データの木枠梱包解除(社外)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの木枠梱包解除(社外)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoMeisaiShukkaMeisaiKaijyoShagai(DatabaseHelper dbHelper, CondK02 cond)
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
            sb.ApdL("       JYOTAI_FLAG = CASE WHEN SHUKA_DATE IS NULL THEN '1' ELSE '2' END");
            sb.ApdL("     , KOJI_NO = NULL");
            sb.ApdL("     , CASE_ID = NULL");
            sb.ApdL("     , KIWAKUKONPO_DATE = NULL");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("   AND JYOTAI_FLAG = ").ApdN(this.BindPrefix).ApdL("JYOTAI_FLAG");
            sb.ApdL("   AND BOX_NO IS NULL");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
            paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.KIWAKUKONPO_VALUE1));

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

    #region 木枠明細データの更新

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細データの更新
    /// </summary>
    /// <param name="dbHelper">dbHelper</param>
    /// <param name="dt">木枠明細データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoMeisaiKiwakuMeisai(DatabaseHelper dbHelper, DataTable dt, CondK02 cond)
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
            sb.ApdN("       ITEM = ").ApdN(this.BindPrefix).ApdL("ITEM");
            sb.ApdN("     , DESCRIPTION_1 = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_1");
            sb.ApdN("     , DESCRIPTION_2 = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION_2");
            sb.ApdN("     , NET_W = ").ApdN(this.BindPrefix).ApdL("NET_W");
            sb.ApdN("     , GROSS_W = ").ApdN(this.BindPrefix).ApdL("GROSS_W");
            if (cond.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
            {
                sb.ApdN("     , PALLET_NO_1 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_1");
                sb.ApdN("     , PALLET_NO_2 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_2");
                sb.ApdN("     , PALLET_NO_3 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_3");
                sb.ApdN("     , PALLET_NO_4 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_4");
                sb.ApdN("     , PALLET_NO_5 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_5");
                sb.ApdN("     , PALLET_NO_6 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_6");
                sb.ApdN("     , PALLET_NO_7 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_7");
                sb.ApdN("     , PALLET_NO_8 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_8");
                sb.ApdN("     , PALLET_NO_9 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_9");
                sb.ApdN("     , PALLET_NO_10 = ").ApdN(this.BindPrefix).ApdL("PALLET_NO_10");
            }
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ITEM", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.ITEM, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_1", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_1, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION_2", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.DESCRIPTION_2, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("NET_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.NET_W, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("GROSS_W", ComFunc.GetFldObject(dr, Def_T_KIWAKU_MEISAI.GROSS_W, DBNull.Value)));
                if (cond.TorokuFlag == TOROKU_FLAG.NAI_VALUE1)
                {
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
                }
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));

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

    #region 出荷明細データの木枠梱包(社内)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの木枠梱包(社内)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠明細データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/03</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoMeisaiShukkaMeisaiShanai(DatabaseHelper dbHelper, DataTable dt, CondK02 cond)
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
            sb.ApdL("       JYOTAI_FLAG = '5'");
            sb.ApdN("     , KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("     , KIWAKUKONPO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            List<string> palletNoList = new List<string>();
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_1));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_2));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_3));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_4));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_5));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_6));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_7));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_8));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_9));
            palletNoList.Add(ComFunc.GetFld(dt, 0, Def_T_KIWAKU_MEISAI.PALLET_NO_10));
            for (int i = 0; i < palletNoList.Count; i++)
            {
                if (string.IsNullOrEmpty(palletNoList[i])) continue;
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", palletNoList[i]));

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

    #region 出荷明細データの木枠梱包(社外)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの木枠梱包(社外)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">社外木枠明細データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdKiwakuKonpoMeisaiShukkaMeisaiShagai(DatabaseHelper dbHelper, DataTable dt, CondK02 cond)
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
            sb.ApdL("       JYOTAI_FLAG = '5'");
            sb.ApdN("     , KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("     , CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
            sb.ApdN("     , KIWAKUKONPO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            // バインド変数設定
            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(ComFunc.GetFld(dr, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO))) continue;
                paramCollection = new DbParamCollection();
                paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));
                paramCollection.Add(iNewParam.NewDbParameter("CASE_ID", cond.CaseID));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFldObject(dr, Def_T_SHAGAI_KIWAKU_MEISAI.TAG_NO)));

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

    #region 社外木枠明細データの削除

    /// --------------------------------------------------
    /// <summary>
    /// 社外木枠明細データの削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelShagaiKiwakuMeisai(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_SHAGAI_KIWAKU_MEISAI");
            sb.ApdL(" WHERE ");
            sb.ApdN("       KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            sb.ApdN("   AND CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");

            // バインド変数設定
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

    #endregion

    #region K0200050:木枠梱包明細登録

    #region SELECT

    #region パレットデータ取得

    /// --------------------------------------------------
    /// <summary>
    /// パレットデータ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPalletDataForMoveShip(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(ComDefine.DTTBL_MOVE_SHIP_PALLET);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TPM.PALLET_NO");
            sb.ApdL("     , MNS.SHUKKA_FLAG");
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , MNS.BUKKEN_NO");
            sb.ApdL("  FROM T_PALLETLIST_MANAGE TPM");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS ON TPM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("                           AND TPM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));

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

    #region パレットリスト管理データの取得＆行ロック

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データの取得＆行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockPalletListManageForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_PALLETLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TPM.SHUKKA_FLAG");
            sb.ApdL("     , TPM.NONYUSAKI_CD");
            sb.ApdL("     , TPM.PALLET_NO");
            sb.ApdL("  FROM T_PALLETLIST_MANAGE TPM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TPM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            foreach (DataRow dr in dt.Rows)
            {
                string palletNo = ComFunc.GetFld(dr, Def_T_PALLETLIST_MANAGE.PALLET_NO);
                if (string.IsNullOrEmpty(palletNo))
                {
                    continue;
                }

                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", palletNo));

                var dtTmp = new DataTable();
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

    #region BOXリスト管理データの取得＆行ロック

    /// --------------------------------------------------
    /// <summary>
    /// BOXリスト管理データの取得＆行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockBoxListManageForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_BOXLIST_MANAGE.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TBM.SHUKKA_FLAG");
            sb.ApdL("     , TBM.NONYUSAKI_CD");
            sb.ApdL("     , TBM.BOX_NO");
            sb.ApdL("  FROM T_BOXLIST_MANAGE TBM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TBM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            var lstBoxNo = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string boxNo = ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.BOX_NO);
                if (string.IsNullOrEmpty(boxNo))
                {
                    continue;
                }
                if (lstBoxNo.Contains(boxNo))
                {
                    continue;
                }
                lstBoxNo.Add(boxNo);

                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", boxNo));

                var dtTmp = new DataTable();
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

    #region 出荷明細データの取得＆行ロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの取得＆行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockShukkaMeisaiForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("  FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TSM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO)));

                var dtTmp = new DataTable();
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
    
    #endregion

    #region UPDATE

    #region パレットリスト管理データ更新

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="nonyusakiCd">移動先納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdPalletListManageForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt, CondK02 cond, string nonyusakiCd)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_PALLETLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFld(dr, Def_T_PALLETLIST_MANAGE.PALLET_NO)));

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
    /// BOXリスト管理データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="nonyusakiCd">移動先納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdBoxListManageForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt, CondK02 cond, string nonyusakiCd)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BOXLIST_MANAGE");
            sb.ApdL("SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", nonyusakiCd));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFld(dr, Def_T_BOXLIST_MANAGE.BOX_NO)));

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

    #region 出荷明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="nonyusakiCd">移動先納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiForPalletMoveShip(DatabaseHelper dbHelper, DataTable dt, CondK02 cond, string nonyusakiCd)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("DST_NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("SRC_NONYUSAKI_CD");
            sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("DST_NONYUSAKI_CD", nonyusakiCd));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("SRC_NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
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
    
    #endregion

    #endregion

    #region K0200070:木枠梱包明細登録(社外)

    #region SELECT

    #region 出荷明細データの取得＆行ロック

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データの取得＆行ロック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable LockShukkaMeisaiForTagMoveShip(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("  FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TSM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND TSM.NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("   AND TSM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , TSM.TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO)));

                var dtTmp = new DataTable();
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
    
    #endregion

    #region UPDATE

    #region 出荷明細データ更新

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dt">木枠データ</param>
    /// <param name="cond">K02用コンディション</param>
    /// <param name="nonyusakiCd">移動先納入先コード</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/01/08</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiForTagMoveShip(DatabaseHelper dbHelper, DataTable dt, CondK02 cond, string nonyusakiCd)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_SHUKKA_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("DST_NONYUSAKI_CD");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdN("       SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("SRC_NONYUSAKI_CD");
            sb.ApdN("   AND TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("DST_NONYUSAKI_CD", nonyusakiCd));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
                paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));

                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("SRC_NONYUSAKI_CD", ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD)));
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

    #endregion

    #region K0200100:パッキングリスト発行

    #region SELECT
    /// --------------------------------------------------
    /// <summary>
    /// 工事識別No・便もしくは発行選択区分から木枠情報を取得
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetPackingList(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        1 AS INSATU");
            sb.ApdL("      , TKW.SHIP");
            sb.ApdL("      , TKW.KOJI_NAME");
            sb.ApdL("      , COM1.ITEM_NAME AS JYOTAI");
            sb.ApdL("      , TKW.KOJI_NO");
            sb.ApdL("      , '0' AS LIST_SELECT");
            sb.ApdL("      , TKW.CASE_MARK_FILE");
            sb.ApdL("      , ROW_NUMBER() OVER(ORDER BY TKW.KOJI_NO) AS ROW_NO");
            sb.ApdL("      , TKW.DELIVERY_NO");
            sb.ApdL("      , TKW.PORT_OF_DESTINATION");
            sb.ApdL("      , TKW.AIR_BOAT");
            sb.ApdL("      , TKW.DELIVERY_DATE");
            sb.ApdL("      , TKW.DELIVERY_POINT");
            sb.ApdL("      , TKW.FACTORY");
            sb.ApdL("      , TKW.REMARKS");

            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU TKW");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'SAGYO_FLAG'");
            sb.ApdL("                         AND COM1.VALUE1 = TKW.SAGYO_FLAG");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            if (!string.IsNullOrEmpty(cond.KojiName))
            {
                fieldName = "KOJI_NAME";
                sb.ApdN("   AND TKW.KOJI_NAME = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.KojiName));


                if (cond.Ship != null)
                {
                    fieldName = "SHIP";
                    sb.ApdN("   AND TKW.SHIP = ").ApdN(this.BindPrefix).ApdL(fieldName);
                    paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Ship));
                }
            }

            if (!string.IsNullOrEmpty(cond.HakkoSelect) && cond.HakkoSelect != "10")
            {
                fieldName = "SAGYO_FLAG";
                sb.ApdN("   AND TKW.SAGYO_FLAG = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.HakkoSelect));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKW.KOJI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_KIWAKU.Name);

            // object型の追加
            ds.Tables[Def_T_KIWAKU.Name].Columns.Add("CASE_MARK", typeof(object));
            return ds;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 木枠・木枠明細を工事識別Noで検索
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="kojiNo"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/28</create>
    /// <update>H.Tajimi 2015/11/26 ケースナンバーの欠番対応</update>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetPackingKiwaku(DatabaseHelper dbHelper, String kojiNo)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_KIWAKU.KOJI_NO);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = Def_T_KIWAKU.KOJI_NO;
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        TKM.CASE_ID");
            sb.ApdL("      , TKM.CASE_NO");
            sb.ApdL("      , '' AS CNO");
            sb.ApdL("      , '' AS CNO_CODE");
            sb.ApdL("      , TKM.KOJI_NO");
            sb.ApdL("      , TKM.STYLE");
            sb.ApdL("      , TKM.ITEM");
            sb.ApdL("      , '' AS ITEM_1");
            sb.ApdL("      , '' AS ITEM_2");
            sb.ApdL("      , TKM.DESCRIPTION_1");
            sb.ApdL("      , TKM.DESCRIPTION_2");
            sb.ApdL("      , '1' AS QTY");
            sb.ApdL("      , TKM.DIMENSION_L");
            sb.ApdL("      , TKM.DIMENSION_W");
            sb.ApdL("      , TKM.DIMENSION_H");
            sb.ApdL("      , TKM.MMNET");
            sb.ApdL("      , TKM.NET_W");
            sb.ApdL("      , TKM.GROSS_W");
            sb.ApdL("      , TKW.SHIP");
            sb.ApdL("      , TKW.KOJI_NAME");
            sb.ApdL("      , TKM.MOKUZAI_JYURYO");
            // 2012/07/04 Add K.Tsutsumi 印刷日付
            sb.ApdL("      , SQ1.KIWAKUKONPO_DATE AS PRINT_DATE");
            // ↑
            // 2015/11/26 H.Tajimi ケースナンバー欠番対応
            sb.ApdL("      , TKM.PRINT_CASE_NO");
            // ↑
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU_MEISAI TKM");
            sb.ApdN("  LEFT JOIN T_KIWAKU TKW ON TKW.KOJI_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
            // 2012/07/04 Add K.Tsutsumi 印刷日付
            sb.ApdL("  LEFT JOIN ");
            sb.ApdL("        (");
            sb.ApdL("        SELECT ");
            sb.ApdL("                MAX(KIWAKUKONPO_DATE) AS KIWAKUKONPO_DATE");
            sb.ApdL("        FROM ");
            sb.ApdL("               T_SHUKKA_MEISAI");
            sb.ApdL("        WHERE");
            sb.ApdN("                KOJI_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
            sb.ApdL("        ) SQ1 ON 1 = 1 ");
            // ↑
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");
            sb.ApdN("   AND TKM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TKW.KOJI_NO , TKM.CASE_NO");

            paramCollection.Add(iNewParameter.NewDbParameter(fieldName, kojiNo));
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
    /// 出荷明細を工事識別Noで検索
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="kojiNo">工事識別No</param>
    /// <param name="caseId">内部管理用キー(null許容)</param>
    /// <param name="language">言語</param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/07/28</create>
    /// <update>T.Sakiori 2014/02/19</update>
    /// <update>H.Tajimi 2015/11/18 Free1,Free2追加</update>
    /// <update>H.Tajimi 2015/11/20 備考追加</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/08/31 多言語化対応</update>
    /// <update>D.Okumura 2019/08/30 木枠まとめ発行対応</update>
    /// <update>H.Tajimi 2019/09/09 印刷C/NO対応</update>
    /// <update>J.Chen 2023/01/04 INV付加名、TAG便名追加</update>
    /// <update>J.Chen 2024/11/07 通関確認状態追加</update>
    /// --------------------------------------------------
    public DataTable GetPackingData(DatabaseHelper dbHelper, String kojiNo, String caseId, String language)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            string fieldName = string.Empty;
            INewDbParameterBasic iNewParameter = dbHelper;
            const string CASE_WHEN_DISP_SHIP_ARNO = "CASE WHEN TSM.SHUKKA_FLAG = {0} THEN MNS.SHIP ELSE TSM.AR_NO END AS SHIP_AR_NO";

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("        TSM.KOJI_NO");
            sb.ApdL("      , TKM.CASE_NO");
            sb.ApdL("      , TSM.ST_NO");
            sb.ApdL("      , TSM.KISHU");
            sb.ApdL("      , TSM.TAG_NO");
            sb.ApdL("      , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("      , TSM.NUM");
            sb.ApdL("      , TSM.HINMEI");
            sb.ApdL("      , TSM.BOX_NO");
            sb.ApdL("      , TSM.FLOOR");
            sb.ApdL("      , '' AS CNO");
            sb.ApdL("      , '' AS STYLE");
            sb.ApdL("      , '' AS NET_W");
            sb.ApdL("      , '' AS GROSS_W");
            sb.ApdL("      , '' AS MMNET");
            sb.ApdL("      , '' AS DIMENSION_L");
            sb.ApdL("      , '' AS DIMENSION_W");
            sb.ApdL("      , '' AS DIMENSION_H");
            // 2012/07/04 Add K.Tsutsumi 印刷日付
            sb.ApdL("      , SQ1.KIWAKUKONPO_DATE AS PRINT_DATE");
            // ↑
            // 2014/02/19 Add T.Sakiori 出荷明細用
            sb.ApdL("     , MNS.NONYUSAKI_CD");
            sb.ApdL("     , BK.BUKKEN_NAME AS NONYUSAKI_NAME");
            sb.ApdL("     , MNS.SHIP");
            sb.ApdL("     , MNS.KANRI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS JYOTAI_NAME");
            sb.ApdN("     , ").ApdL(string.Format(CASE_WHEN_DISP_SHIP_ARNO, this.BindPrefix + "SHUKKA_FLAG_NORMAL"));
            //sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TSM.AR_NO");
            sb.ApdL("     , TSM.SEIBAN");
            sb.ApdL("     , TSM.CODE");
            sb.ApdL("     , TSM.ZUMEN_OIBAN");
            sb.ApdL("     , TSM.AREA");
            //sb.ApdL("     , TSM.FLOOR");
            //sb.ApdL("     , TSM.KISHU");
            //sb.ApdL("     , TSM.ST_NO");
            sb.ApdL("     , TSM.HINMEI_JP");
            //sb.ApdL("     , TSM.HINMEI");
            //sb.ApdL("     , TSM.ZUMEN_KEISHIKI");
            sb.ApdL("     , TSM.KUWARI_NO");
            //sb.ApdL("     , TSM.NUM");
            sb.ApdL("     , TSM.JYOTAI_FLAG");
            sb.ApdL("     , TSM.SHUKA_DATE");
            //sb.ApdL("     , TSM.BOX_NO");
            sb.ApdL("     , TSM.BOXKONPO_DATE");
            sb.ApdL("     , TSM.PALLET_NO");
            sb.ApdL("     , TSM.PALLETKONPO_DATE");
            //sb.ApdL("     , TSM.KOJI_NO");
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
            // ↑
            // 2015/11/18 H.Tajimi 出荷明細用(Free1,Free2追加)
            sb.ApdL("     , TSM.FREE1");
            sb.ApdL("     , TSM.FREE2");
            // ↑
            // 2015/11/20 H.Tajimi 出荷明細用(備考追加)
            sb.ApdL("     , TSM.BIKO");
            // ↑
            sb.ApdL("     , TSM.CUSTOMS_STATUS");
            // ↑
            // 2015/12/09 H.Tajimi 出荷明細用(M_NO追加)
            sb.ApdL("     , TSM.M_NO");
            // ↑
            sb.ApdL("     , TSM.GRWT");
            sb.ApdL("     , TSM.TEHAI_RENKEI_NO");
            sb.ApdL("     , TKM.PRINT_CASE_NO");
            sb.ApdL("     , TM.HINMEI_INV");
            sb.ApdL("     , TSM.TAG_SHIP");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI TKM ON TKM.CASE_ID = TSM.CASE_ID");

            fieldName = "KOJI_NO";

            // 2012/07/04 Add K.Tsutsumi 印刷日付
            sb.ApdL("  LEFT JOIN ");
            sb.ApdL("        (");
            sb.ApdL("        SELECT ");
            sb.ApdL("                MAX(KIWAKUKONPO_DATE) AS KIWAKUKONPO_DATE");
            sb.ApdL("        FROM ");
            sb.ApdL("               T_SHUKKA_MEISAI");
            sb.ApdL("        WHERE");
            sb.ApdN("                KOJI_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
            sb.ApdL("        ) SQ1 ON 1 = 1 ");
            // ↑
            // 2014/02/19 Add T.Sakiori 出荷明細用
            sb.ApdL(" INNER JOIN M_NONYUSAKI MNS");
            sb.ApdL("    ON TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("   AND TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN M_COMMON COM");
            sb.ApdL("    ON COM.GROUP_CD = 'DISP_JYOTAI_FLAG'");
            sb.ApdL("   AND COM.VALUE1 = TSM.JYOTAI_FLAG");
            sb.ApdN("   AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN T_KIWAKU TK");
            sb.ApdL("    ON TK.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("  LEFT JOIN M_BUKKEN BK");
            sb.ApdL("    ON BK.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("   AND BK.BUKKEN_NO = MNS.BUKKEN_NO");
            // ↑
            sb.ApdL("  LEFT JOIN T_TEHAI_MEISAI TM");
            sb.ApdL("    ON TM.TEHAI_RENKEI_NO = TSM.TEHAI_RENKEI_NO");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            sb.ApdN("   AND TSM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL(fieldName);
            paramCollection.Add(iNewParameter.NewDbParameter(fieldName, kojiNo));

            if (!string.IsNullOrEmpty(caseId))
            {
                sb.ApdN("   AND TKM.CASE_ID = ").ApdN(this.BindPrefix).ApdL("CASE_ID");
                paramCollection.Add(iNewParameter.NewDbParameter("CASE_ID", caseId));
            }

            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.KOJI_NO");
            sb.ApdL("     , TKM.CASE_NO");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", language));
            paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));

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
    /// 工事識別Noの存在チェック
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tsunamura 2010/08/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool kojiCheck(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT ");
            sb.ApdL("       COUNT(1) AS CNT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_KIWAKU ");
            sb.ApdL(" WHERE ");
            sb.ApdL("       1 = 1");

            if (!string.IsNullOrEmpty(cond.KojiName))
            {
                fieldName = "KOJI_NAME";
                sb.ApdN("   AND KOJI_NAME = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.KojiName));
            }

            if (!string.IsNullOrEmpty(cond.Ship))
            {
                fieldName = "SHIP";
                sb.ApdN("   AND SHIP = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParameter.NewDbParameter(fieldName, cond.Ship));
            }

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, dt);
            if (0 == ComFunc.GetFldToInt32(dt, 0, ComDefine.FLD_CNT))
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region K0200110:木枠まとめ発行

    #region SELECT

    #region 物件データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション(KojiNo)</param>
    /// <returns>木枠データ</returns>
    /// <create>D.Okumura 2019/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetKiwakuMatomeBukkenData(DatabaseHelper dbHelper, CondK02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_M_NONYUSAKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT ");
            sb.ApdL("    MNS.NONYUSAKI_CD");
            sb.ApdL("  , MNS.SHUKKA_FLAG");
            sb.ApdL("  , MB.BUKKEN_NO");
            sb.ApdL("  , MB.BUKKEN_NAME");
            sb.ApdL("  , MB.PROJECT_NO");
            sb.ApdL("  FROM T_SHUKKA_MEISAI AS TSM");
            sb.ApdL("  INNER JOIN M_NONYUSAKI AS MNS");
            sb.ApdL("    ON  TSM.NONYUSAKI_CD = MNS.NONYUSAKI_CD");
            sb.ApdL("    AND TSM.SHUKKA_FLAG = MNS.SHUKKA_FLAG");
            sb.ApdL("  INNER JOIN M_BUKKEN AS MB");
            sb.ApdL("    ON MNS.BUKKEN_NO = MB.BUKKEN_NO");
            sb.ApdL("    AND MNS.SHUKKA_FLAG = MB.SHUKKA_FLAG");
            sb.ApdL("  WHERE");
            sb.ApdN("        TSM.KOJI_NO = ").ApdN(this.BindPrefix).ApdL("KOJI_NO");
            
            paramCollection.Add(iNewParam.NewDbParameter("KOJI_NO", cond.KojiNo));

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

    #endregion //K0200110:木枠まとめ発行

    #endregion

    #region 木枠明細のDESCRIPTION_1用文字列取得

    /// --------------------------------------------------
    /// <summary>
    /// 木枠明細のDESCRIPTION_1用文字列取得
    /// </summary>
    /// <remarks>
    /// 出荷明細より工事識別管理NOとケースIDに合致するM_NOを取得し
    /// カンマ区切りで連結して、30文字を超えた分は破棄して返却します
    /// </remarks>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K02用コンディション</param>
    /// <returns>カンマ区切りのM_NO</returns>
    /// <create>H.Tajimi 2015/12/11</create>
    /// <update></update>
    /// --------------------------------------------------
    public string GetKiwakuMeisaiDescription1(DatabaseHelper dbHelper, CondK02 cond)
    {
        // M_NO取得
        var dtMNo = this.GetKiwakuKonpoShukkaMeisaiMNo(dbHelper, cond);
        if (dtMNo != null && dtMNo.Rows.Count > 0)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < dtMNo.Rows.Count; i++)
            {
                var mNo = UtilData.GetFld(dtMNo, i, Def_T_SHUKKA_MEISAI.M_NO);
                if (!string.IsNullOrEmpty(mNo))
                {
                    if (sb.Length == 0)
                    {
                        sb.ApdN(mNo);
                    }
                    else
                    {
                        sb.ApdN("," + mNo);
                    }
                }
            }

            if (sb.Length > 0)
            {
                var description = sb.ToString();
                if (description.Length > LEN_DESCRIPTION_1)
                {
                    description = description.Substring(0, LEN_DESCRIPTION_1);
                }
                return description;
            }
        }
        return string.Empty;
    }

    #endregion

}
