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
/// 取込結果処理（データアクセス層） 
/// </summary>
/// <create>Y.Higuchi 2010/06/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsK04Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>Y.Higuchi 2010/06/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsK04Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region TEMP_IDの採番及び更新

    #region 一時取込データにTEMP_IDを採番しセットする。

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データにTEMP_IDを採番しセットする。
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">一時取込データ、一時取込明細データを格納しているDataSet</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SetTempID(DatabaseHelper dbHelper, CondK04 cond, DataSet ds, DataTable dtMessage)
    {
        try
        {
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.LoginInfo = cond.LoginInfo.Clone();
            condSms.SaibanFlag = SAIBAN_FLAG.TEMP_ID_VALUE1;
            using (WsSmsImpl impl = new WsSmsImpl())
            {
                foreach (DataRow dr in ds.Tables[Def_T_TEMPWORK.Name].Rows)
                {
                    // 一時取込IDの採番
                    string tempID = string.Empty;
                    string errMsgID = string.Empty;
                    if (!impl.GetSaiban(dbHelper, condSms, out tempID, out errMsgID))
                    {
                        ComFunc.AddMultiMessage(dtMessage, errMsgID, null);
                        return false;
                    }
                    // 一時取込明細データのTEMP_IDに置き換え
                    string oldTempID = ComFunc.GetFld(dr, Def_T_TEMPWORK.TEMP_ID);
                    if (!this.SetTempID_Meisai(oldTempID, tempID, ds))
                    {
                        ComFunc.AddMultiMessage(dtMessage, errMsgID, null);
                        return false;
                    }
                    // 明細にTempIDが設定できたのでヘッダにも設定
                    dr[Def_T_TEMPWORK.TEMP_ID] = tempID;
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

    #region 一時取込明細データの仮TEMP_IDから採番したTEMP_IDをセットする。

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データの仮TEMP_IDから採番したTEMP_IDをセットする。
    /// </summary>
    /// <param name="oldTempID"></param>
    /// <param name="newTempID"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool SetTempID_Meisai(string oldTempID, string newTempID, DataSet ds)
    {
        try
        {
            bool ret = false;
            foreach (DataRow dr in ds.Tables[Def_T_TEMPWORK_MEISAI.Name].Select(Def_T_TEMPWORK_MEISAI.TEMP_ID + " = " + UtilConvert.PutQuot(oldTempID)))
            {
                dr[Def_T_TEMPWORK_MEISAI.TEMP_ID] = newTempID;
                ret = true;
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

    #region 一時取込データ、一時取込明細データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ、一時取込明細データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">一時取込データ、一時取込明細データを格納しているDataSet</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsTempworkData(DatabaseHelper dbHelper, CondK04 cond, DataSet ds)
    {
        try
        {
            cond.TorikomiDate = DateTime.Now;
            this.InsTempwork(dbHelper, cond, ds.Tables[Def_T_TEMPWORK.Name]);
            this.InsTempworkMeisai(dbHelper, cond, ds.Tables[Def_T_TEMPWORK_MEISAI.Name]);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 検品OK分の一時取込データ、一時取込明細データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 検品OK分の一時取込データ、一時取込明細データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="ds">一時取込データ、一時取込明細データを格納しているDataSet</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsTempworkDataKenpinOK(DatabaseHelper dbHelper, CondK04 cond, DataSet ds)
    {
        try
        {
            // 登録
            this.InsTempworkData(dbHelper, cond, ds);
            // 旧一時取込明細データのOKレコードを削除
            this.DelOldKenpinMeisaiOK(dbHelper, cond);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 取込

    #region 対象データのロック

    /// --------------------------------------------------
    /// <summary>
    /// 対象データのロック&ステータス変更
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/24</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool LockImportData(DatabaseHelper dbHelper, CondK04 cond, DataTable dtMessage)
    {
        try
        {
            CondK04 condK04 = (CondK04)cond.Clone();
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }

            // TEMP_IDから一時取込データを取得
            DataTable dt = this.LockTempwork(dbHelper, condK04);
            if (dt.Rows.Count < 1)
            {
                // 他端末で処理中です。
                ComFunc.AddMultiMessage(dtMessage, "K0400010002");
                return false;
            }
            // 一時取込データのステータス更新
            condK04.StatusFlag = STATUS_FLAG.TORIKOMICHU_VALUE1;
            this.UpdTempworkStatusFlagOnly(dbHelper, condK04);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 取込データチェック

    /// --------------------------------------------------
    /// <summary>
    /// 取込データチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <param name="isError">取込時データで取込めない物があったかどうか</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
    /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
    /// --------------------------------------------------
    public bool ImportCheckData(DatabaseHelper dbHelper, CondK04 cond, DataTable dtMessage, ref bool isImportError)
    {
        try
        {
            CondK04 condK04 = (CondK04)cond.Clone();
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }

            DataTable dt = this.GetTempwork(dbHelper, condK04).Tables[Def_T_TEMPWORK.Name];
            // 一時取込明細データの初期化
            this.UpdInitializeTempworkMeisai(dbHelper, condK04);
            // 取込処理
            condK04.TorikomiFlag = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.TORIKOMI_FLAG);
            if (condK04.TorikomiFlag == TORIKOMI_FLAG.HIKIWATASHI_VALUE1)
            {
                // 引渡
                if (!this.ImportCheckDataHikiwatashi(dbHelper, condK04))
                {
                    isImportError = true;
                }
                // 納入先、便/AR No.セット
                this.UpdTempWorkMeisaiNonyusaki(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.SHUKA_VALUE1)
            {
                // 現品集荷
                if (!this.ImportCheckDataShuka(dbHelper, condK04))
                {
                    isImportError = true;
                }
                // 納入先、便/AR No.セット
                this.UpdTempWorkMeisaiNonyusaki(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.BOX_VALUE1)
            {
                // Box梱包
                condK04.BoxNo = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.DATA_NO);
                if (!this.ImportCheckDataBox(dbHelper, condK04))
                {
                    isImportError = true;
                }
                // 納入先、便/AR No.セット
                this.UpdTempWorkMeisaiNonyusaki(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.KENPIN_VALUE1)
            {
                // 検品
                if (!this.ImportCheckDataKenpin(dbHelper, condK04))
                {
                    isImportError=true;
                }
                // 納入先、便/AR No.セット
                this.UpdTempWorkMeisaiNonyusakiKenpin(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.MEASURE_VALUE1)
            {
                // 計測
                if (!this.ImportCheckDataMeasure(dbHelper, condK04))
                {
                    isImportError = true;
                }
                // 納入先、便/AR No.セット
                this.UpdTempWorkMeisaiNonyusaki(dbHelper, condK04);
            }
            else
            {
                // パレット梱包
                condK04.PalletNo = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.DATA_NO);
                if (!this.ImportCheckDataPallet(dbHelper, condK04))
                {
                    isImportError = true;
                }
                // 納入先、便/AR No.セット（パレット梱包用）
                this.UpdTempWorkMeisaiNonyusakiPallet(dbHelper, condK04);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 取込(引渡)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(引渡)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.SASAYAMA 2023/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckDataHikiwatashi(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // TagNoチェック
            using (DataTable dtCheck = GetTorikomi_TagNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTagNoです。
                    condCommon.MessageID = "K0400010013";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            //引渡日チェック
            using (DataTable dtCheck = this.GetTorikomi_HikiwatashiDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に引渡済みのTagNoです。
                    condCommon.MessageID = "K0400010076";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 集荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に集荷済みのTagNoです。
                    condCommon.MessageID = "K0400010014";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 木枠梱包日チェック
            using (DataTable dtCheck = this.GetTorikomi_KiwakuKonpoDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に木枠梱包済のTagNo.です。
                    condCommon.MessageID = "K0400010054";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 出荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukkaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 出荷済です。
                    condCommon.MessageID = "A9999999031";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
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

    #region 取込(現品集荷)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(現品集荷)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckDataShuka(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // TagNoチェック
            using (DataTable dtCheck = GetTorikomi_TagNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTagNoです。
                    condCommon.MessageID = "K0400010013";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 集荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に集荷済みのTagNoです。
                    condCommon.MessageID = "K0400010014";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 木枠梱包日チェック
            using (DataTable dtCheck = this.GetTorikomi_KiwakuKonpoDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に木枠梱包済のTagNo.です。
                    condCommon.MessageID = "K0400010054";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 出荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukkaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 出荷済です。
                    condCommon.MessageID = "A9999999031";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
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

    #region 取込(Box梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(Box梱包)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>K.Tsutsumi 2018/09/05</update>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckDataBox(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // TagNoチェック
            using (DataTable dtCheck = GetTorikomi_TagNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTagNoです。
                    condCommon.MessageID = "K0400010013";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 未集荷チェック
            using (DataTable dtCheck = this.GetTorikomi_MiShukaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    //未集荷のTagNoです。
                    condCommon.MessageID = "K0400010015";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // Box梱包チェック
            using (DataTable dtCheck = this.GetTorikomi_BoxKonpoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既にBox梱包済みのTagNoです。
                    condCommon.MessageID = "K0400010016";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // BoxNoチェック
            using (DataTable dtCheck = this.GetTorikomi_BoxNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に使用しているBoxNoです。
                    condCommon.MessageID = "K0400010017";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 納入先・物件番号チェック
            using (DataTable dtCheck = this.GetTorikomi_BoxNonyusakiCheck(dbHelper, cond))
            {
                // 納入先・物件番号の複数チェック
                string shukkaFlag = ComFunc.GetFld(dtCheck, 0, Def_M_NONYUSAKI.SHUKKA_FLAG);

                if (shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    // 本体は納入先が一つ
                    DataTable dtNonyusaki = dtCheck.DefaultView.ToTable(true,
                                                        Def_M_NONYUSAKI.SHUKKA_FLAG,
                                                        Def_M_NONYUSAKI.NONYUSAKI_CD);
                    if (1 < dtNonyusaki.Rows.Count)
                    {
                        // Box内に納入先が複数存在します。
                        condCommon.MessageID = "K0400010024";
                        string message = this.GetMessage(dbHelper, condCommon);
                        this.UpdTempworkMeisaiError(dbHelper, cond, message);
                        ret = false;
                    }
                }
                else if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // ARは物件管理Noが一つ
                    DataTable dtBukken = dtCheck.DefaultView.ToTable(true,
                                                        Def_M_NONYUSAKI.SHUKKA_FLAG,
                                                        Def_M_NONYUSAKI.BUKKEN_NO);
                    if (1 < dtBukken.Rows.Count)
                    {
                        // Box内に物件が複数存在します。
                        condCommon.MessageID = "K0400010056";
                        string message = this.GetMessage(dbHelper, condCommon);
                        this.UpdTempworkMeisaiError(dbHelper, cond, message);
                        ret = false;
                    }
                }
            }
            // 木枠梱包日チェック
            using (DataTable dtCheck = this.GetTorikomi_KiwakuKonpoDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に木枠梱包済のTagNo.です。
                    condCommon.MessageID = "K0400010054";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 出荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukkaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 出荷済です。
                    condCommon.MessageID = "A9999999031";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
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

    #region 取込(パレット梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(パレット梱包)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>K.Tsutsumi 2018/09/05</update>
    /// --------------------------------------------------
    public bool ImportCheckDataPallet(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // BoxNo登録チェック
            using (DataTable dtCheck = this.GetTorikomi_BoxNoNoResistCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないBoxNoです。
                    condCommon.MessageID = "K0400010018";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // パレット梱包チェック
            using (DataTable dtCheck = this.GetTorikomi_PalletCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既にパレット梱包済みのBoxNoです。
                    condCommon.MessageID = "K0400010019";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // パレットNoチェック
            using (DataTable dtCheck = this.GetTorikomi_PalletNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に使用しているパレットNoです。
                    condCommon.MessageID = "K0400010020";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 納入先・物件番号チェック
            using (DataTable dtCheck = this.GetTorikomi_PalletNonyusakiCheck(dbHelper, cond))
            {
                // 納入先・物件番号の複数チェック
                string shukkaFlag = ComFunc.GetFld(dtCheck, 0, Def_M_NONYUSAKI.SHUKKA_FLAG);

                if (shukkaFlag == SHUKKA_FLAG.NORMAL_VALUE1)
                {
                    // 本体は納入先が一つ
                    DataTable dtNonyusaki = dtCheck.DefaultView.ToTable(true,
                                        Def_T_SHUKKA_MEISAI.SHUKKA_FLAG,
                                        Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);

                    if (1 < dtNonyusaki.Rows.Count)
                    {
                        // パレット内に納入先が複数存在します。
                        condCommon.MessageID = "K0400010026";
                        string message = this.GetMessage(dbHelper, condCommon);
                        this.UpdTempworkMeisaiError(dbHelper, cond, message);
                        ret = false;
                    }
                }
                else if (shukkaFlag == SHUKKA_FLAG.AR_VALUE1)
                {
                    // ARは物件管理Noが一つ
                    DataTable dtBukken = dtCheck.DefaultView.ToTable(true,
                                                        Def_M_NONYUSAKI.SHUKKA_FLAG,
                                                        Def_M_NONYUSAKI.BUKKEN_NO);
                    if (1 < dtBukken.Rows.Count)
                    {
                        // Box内に物件が複数存在します。
                        condCommon.MessageID = "K0400010057";
                        string message = this.GetMessage(dbHelper, condCommon);
                        this.UpdTempworkMeisaiError(dbHelper, cond, message);
                        ret = false;
                    }
                }
            }
            // 出荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_PalletShukkaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 出荷済です。
                    condCommon.MessageID = "A9999999031";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
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

    #region 取込(検品)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(検品)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update>T.SASAYAMA 2023/07/21</update> 検品保留対応
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckDataKenpin(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // 手配Noチェック
            using (var dtCheck = this.GetTorikomi_TehaiNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTehaiNoです。
                    condCommon.MessageID = "K0400010068";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 手配種別チェック
            using (var dtCheck = this.GetTorikomi_TehaiKindFlagCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // まとめ発注、または見積もり都合のためハンディでは取込できません。
                    condCommon.MessageID = "K0400010069";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 入荷数チェック
            using (var dtCheck = this.GetTorikomi_NyukaQtyCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 入荷数が手配数より多くなっています。
                    condCommon.MessageID = "K0400010070";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 保留再試行ではない場合
            if (cond.HoryuRetry == false)
            {
                // 完納時の入荷数チェック
                using (var dtCheck = this.GetTorikomi_NyukaQtyKannouCheck(dbHelper, cond))
                {
                    if (0 < dtCheck.Rows.Count)
                    {
                        // 完納状態の入荷検品において、検品数が手配数を満たしていません。
                        condCommon.MessageID = "K0400010080";
                        string message = this.GetMessage(dbHelper, condCommon);
                        this.UpdTempworkMeisaiErrorHoryu(dbHelper, dtCheck, message);
                        ret = false;
                    }
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

    #region 取込(計測)

    /// --------------------------------------------------
    /// <summary>
    /// 取込(計測)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckDataMeasure(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            bool ret = true;
            CondCommon condCommon = new CondCommon(cond.LoginInfo);

            // TagNoチェック
            using (DataTable dtCheck = GetTorikomi_TagNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTagNoです。
                    condCommon.MessageID = "K0400010013";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 未集荷チェック
            using (DataTable dtCheck = this.GetTorikomi_MiShukaDateCheckForMeasure(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    //未集荷のTagNoです。
                    condCommon.MessageID = "K0400010015";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 木枠梱包日チェック
            using (DataTable dtCheck = this.GetTorikomi_KiwakuKonpoDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 既に木枠梱包済のTagNo.です。
                    condCommon.MessageID = "K0400010054";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 出荷日チェック
            using (DataTable dtCheck = this.GetTorikomi_ShukkaDateCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 出荷済です。
                    condCommon.MessageID = "A9999999031";
                    string message = this.GetMessage(dbHelper, condCommon);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
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

    #region 取込結果反映(他テーブル)

    /// --------------------------------------------------
    /// <summary>
    /// 取込結果反映(他テーブル)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <param name="isDuplicationError">一意制約違反があったかどうか</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>Y.Higuchi 2010/10/27</update>
    /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
    /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
    /// <update>H.Tajimi 2020/04/17 計測の取り込みはエラーがないものだけでもできるよう変更</update>
    /// <update>T.SASAYAMA 2023/07/14 引渡ファイル対応</update>
    /// --------------------------------------------------
    public bool ImportResultUpdateOtherTable(DatabaseHelper dbHelper, CondK04 cond, DataTable dtMessage, out bool isDuplicationError)
    {
        try
        {
            isDuplicationError = false;
            bool isReturn = false;
            string torokuFlag;
            CondK04 condK04 = (CondK04)cond.Clone();
            DataTable dt = this.GetTempwork(dbHelper, condK04).Tables[Def_T_TEMPWORK.Name];
            torokuFlag = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.TORIKOMI_FLAG);
            if (torokuFlag == TORIKOMI_FLAG.SHUKA_VALUE1 || torokuFlag == TORIKOMI_FLAG.MEASURE_VALUE1 || torokuFlag == TORIKOMI_FLAG.HIKIWATASHI_VALUE1)
            {
                CondK04 condNum = (CondK04)cond.Clone();
                condNum.Result = RESULT.OK_VALUE1;
                int successNum = this.GetTempworkMeisaiResultNum(dbHelper, condNum);
                if (successNum < 1) isReturn = true;
            }
            else
            {
                CondK04 condNum = (CondK04)cond.Clone();
                condNum.Result = RESULT.NG_VALUE1;
                int errorNum = this.GetTempworkMeisaiResultNum(dbHelper, condNum);
                if (0 < errorNum) isReturn = true;
            }
            if (isReturn) return true;
            condK04.TorikomiFlag = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.TORIKOMI_FLAG);
            condK04.UpdateDate = DateTime.Now;
            if (condK04.TorikomiFlag == TORIKOMI_FLAG.HIKIWATASHI_VALUE1)
            {
                // ----- 引渡 -----
                // 引渡をセット
                this.UpdShukkaMeisaiHikiwatashi(dbHelper, condK04);

                // -- 操作履歴 --
                this.InsHandyRireki_Hikiwatashi(dbHelper, condK04);

            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.SHUKA_VALUE1)
            {
                // ----- 現品集荷 -----
                // 集荷日をセット
                this.UpdShukkaMeisaiGenpinShuka(dbHelper, condK04);

                // -- 操作履歴 --
                this.InsHandyRireki_GenpinShuka(dbHelper, condK04);

            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.BOX_VALUE1)
            {
                // ----- Box梱包 -----
                // Box梱包日をセット
                condK04.BoxNo = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.DATA_NO);
                this.UpdShukkaMeisaiBox(dbHelper, condK04);
                // 納入先を取得し、BOXリスト管理データを作成
                DataTable dtNonyusaki = this.GetNonyusaki(dbHelper, condK04);
                condK04.ShukkaFlag = ComFunc.GetFld(dtNonyusaki, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                condK04.NonyusakiCD = ComFunc.GetFld(dtNonyusaki, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                this.InsBoxlistManage(dbHelper, condK04);

                // -- 操作履歴 --
                this.InsHandyRireki_Box(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.MEASURE_VALUE1)
            {
                // ----- 計測 -----
                // 重量をセット
                this.UpdShukkaMeisaiMeasure(dbHelper, condK04);

                // -- 操作履歴 --
                this.InsHandyRireki_Measure(dbHelper, condK04);
            }
            else if (condK04.TorikomiFlag == TORIKOMI_FLAG.KENPIN_VALUE1)
            {
                // ----- 検品 -----
                var dtMeisai = this.GetTempworkMeisai(dbHelper, condK04).Tables[Def_T_TEMPWORK_MEISAI.Name];
                foreach (DataRow dr in dtMeisai.Rows)
                {
                    condK04.TehaiNo = ComFunc.GetFld(dr, Def_T_TEMPWORK_MEISAI.TEHAI_NO);
                    condK04.NyukaQty = ComFunc.GetFldToDecimal(dr, Def_T_TEMPWORK_MEISAI.NYUKA_QTY);
                    // 入荷数をセット
                    this.UpdTehaiMeisaiKenpin(dbHelper, condK04);
                    this.UpdTehaiSksKenpin(dbHelper, condK04);
                }

                // -- 操作履歴 --
                this.InsHandyRireki_Kenpin(dbHelper, condK04);
            }
            else
            {
                // ----- パレット梱包 -----
                // パレット梱包日をセット
                condK04.PalletNo = ComFunc.GetFld(dt, 0, Def_T_TEMPWORK.DATA_NO);
                this.UpdShukkaMeisaiPallet(dbHelper, condK04);
                // 納入先を取得し、パレットリスト管理データを作成
                DataTable dtNonyusaki = this.GetNonyusaki(dbHelper, condK04);
                condK04.ShukkaFlag = ComFunc.GetFld(dtNonyusaki, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                condK04.NonyusakiCD = ComFunc.GetFld(dtNonyusaki, 0, Def_T_SHUKKA_MEISAI.NONYUSAKI_CD);
                this.InsPalletlistManage(dbHelper, condK04);

                // -- 操作履歴 --
                this.InsHandyRireki_Pallet(dbHelper, condK04);
            }

            return true;
        }
        catch (Exception ex)
        {
            if (this.IsDbDuplicationError(ex))
            {
                // 一意制約違反
                isDuplicationError = true;
                return false;
            }
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時取込データの状態区分を処理が終わった状態とする。

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの状態区分を処理が終わった状態とする。
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dtMessage"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportEndStatus(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            CondK04 condK04 = (CondK04)cond.Clone();
            condK04.Result = RESULT.NG_VALUE1;
            condK04.ErrorNum= this.GetTempworkMeisaiResultNum(dbHelper, condK04);
            if (0 < condK04.ErrorNum)
            {
                condK04.StatusFlag = STATUS_FLAG.ERROR_VALUE1;
            }
            else
            {
                condK04.StatusFlag = STATUS_FLAG.KANRYO_VALUE1;
            }
            this.UpdTempworkResult(dbHelper, condK04);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion
    
    #region 破棄

    /// --------------------------------------------------
    /// <summary>
    /// データの破棄
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>Y.Higuchi 2010/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DestroyData(DatabaseHelper dbHelper, CondK04 cond, DataTable dtMessage)
    {
        try
        {
            // TEMP_IDから一時取込データを取得
            DataTable dt = this.LockTempwork(dbHelper, cond);
            if (dt.Rows.Count < 1)
            {
                // 他端末で処理中です。
                ComFunc.AddMultiMessage(dtMessage, "K0400010002", null);
                return false;
            }
            cond.StatusFlag = STATUS_FLAG.KANRYO_VALUE1;
            this.UpdTempworkStatusFlagOnly(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 初期表示(Handy操作履歴照会)

    /// --------------------------------------------------
    /// <summary>
    /// 初期表示(Handy操作履歴照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetInitHandyOpeRirekiShokai(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            using (WsCommonImpl comImpl = new WsCommonImpl())
            {
                var ds = new DataSet();
                {
                    CondCommon condCommon = new CondCommon(cond.LoginInfo);
                    condCommon.GroupCD = HANDY_OPERATION_FLAG.GROUPCD;
                    var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                    dt.TableName = HANDY_OPERATION_FLAG.GROUPCD;
                    ds.Merge(dt);
                }
                {
                    CondCommon condCommon = new CondCommon(cond.LoginInfo);
                    condCommon.GroupCD = SHUKKA_FLAG.GROUPCD;
                    var dt = comImpl.GetCommon(dbHelper, condCommon).Tables[Def_M_COMMON.Name];
                    dt.TableName = SHUKKA_FLAG.GROUPCD;
                    ds.Merge(dt);
                }

                //プロジェクトマスタ取得
                ds.Merge(this.Sql_GetProjectList(dbHelper, cond));
                //物件マスタ取得
                ds.Merge(this.Sql_GetBukkenList(dbHelper, cond));
                return ds;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 表示データ取得(Handy操作履歴照会)

    /// --------------------------------------------------
    /// <summary>
    /// 表示データ取得(Handy操作履歴照会)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetHandyOpeRireki(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            using (WsK04Impl impl = new WsK04Impl())
            {
                var ds = new DataSet();
                //プロジェクトマスタ取得
                ds.Merge(impl.Sql_GetHandyOpeRireki(dbHelper, cond));
                return ds;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region メッセージ取得

    /// --------------------------------------------------
    /// <summary>
    /// メッセージ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">取得するメッセージID</param>
    /// <returns>メッセージ</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private string GetMessage(DatabaseHelper dbHelper, CondCommon cond)
    {
        try
        {
            string ret = string.Empty;
            using (WsCommonImpl impl = new WsCommonImpl())
            {
                DataSet ds = impl.GetMessage(dbHelper, cond);
                ret = ComFunc.GetFld(ds, Def_M_MESSAGE.Name, 0, Def_M_MESSAGE.MESSAGE, string.Empty);
            }
            return ret;
        }
        catch
        {
            return string.Empty;
        }
    }

    #endregion

    #endregion

    #region SQL実行

    #region SELECT

    #region 一時取込データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTempwork(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "TTW.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TTW.TEMP_ID");
            sb.ApdL("     , TTW.TORIKOMI_DATE");
            sb.ApdL("     , TTW.TORIKOMI_FLAG");
            sb.ApdL("     , COM.ITEM_NAME AS TORIKOMI_SAGYO");
            sb.ApdL("     , TTW.DATA_NO");
            sb.ApdL("     , TTW.ERROR_NUM");
            sb.ApdL("     , TTW.STATUS_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_TEMPWORK TTW");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'TORIKOMI_FLAG'");
            sb.ApdL("                        AND COM.VALUE1 = TTW.TORIKOMI_FLAG");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.TempID))
            {
                fieldName = "TEMP_ID";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TempID));
            }
            if (!string.IsNullOrEmpty(cond.TorikomiFlag))
            {
                fieldName = "TORIKOMI_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.TorikomiFlag));
            }
            if (!string.IsNullOrEmpty(cond.StatusFlag))
            {
                fieldName = "STATUS_FLAG";
                sb.ApdN("   AND ").ApdN(fieldPrefix + fieldName).ApdN(" = ").ApdN(this.BindPrefix).ApdL(fieldName);
                paramCollection.Add(iNewParam.NewDbParameter(fieldName, cond.StatusFlag));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTW.TEMP_ID DESC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEMPWORK.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時取込データ取得(行レベルロック)

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ取得(行レベルロック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockTempwork(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , TORIKOMI_DATE");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , ERROR_NUM");
            sb.ApdL("     , STATUS_FLAG");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK");
            sb.ApdL("  WITH (ROWLOCK,UPDLOCK)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("   AND (STATUS_FLAG = 0");
            sb.ApdL("    OR STATUS_FLAG = 1 )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 集荷・ボックス梱包・計測共用

    #region 取込チェック(TagNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(TagNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_TagNoCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                      AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                      AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 集荷用

    #region 取込チェック(引渡日チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(引渡日チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.SASAYAMA 2023/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_HikiwatashiDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.HIKIWATASHI_DATE IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(集荷日チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(集荷日チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_ShukaDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.SHUKA_DATE IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(木枠梱包日チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(木枠梱包日チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_KiwakuKonpoDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.KIWAKUKONPO_DATE IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(出荷日チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(出荷日チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_ShukkaDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.SHUKKA_DATE IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region ボックス梱包チェック用

    #region 取込チェック(未集荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(未集荷チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_MiShukaDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.SHUKA_DATE IS NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(Box梱包チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(Box梱包チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_BoxKonpoCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.BOX_NO IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(BoxNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(BoxNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_BoxNoCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_BOXLIST_MANAGE TBM");
            sb.ApdL("                WHERE TBM.BOX_NO = TTM.BOX_NO");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(納入先・物件番号チェック)(Box梱包用)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(納入先・物件番号チェック)(Box梱包用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_BoxNonyusakiCheck(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       M_NONYUSAKI MN ON TSM.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                     AND TSM.NONYUSAKI_CD = MN.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("              )");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MN.BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region パレット梱包チェック用

    #region 取込チェック(BoxNo登録チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(BoxNo登録チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_BoxNoNoResistCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_BOXLIST_MANAGE TBM");
            sb.ApdL("                    WHERE TBM.BOX_NO = TTM.BOX_NO");
            sb.ApdL("                  )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(パレット梱包チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(パレット梱包チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_PalletCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.BOX_NO = TTM.BOX_NO");
            sb.ApdL("                  AND TSM.PALLET_NO IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(パレットNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(パレットNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_PalletNoCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_PALLETLIST_MANAGE TBM");
            sb.ApdL("                WHERE TBM.PALLET_NO = TTM.PALLET_NO");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(納入先・物件番号チェック)(パレット梱包用)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(納入先・物件番号チェック)(パレット梱包用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_PalletNonyusakiCheck(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       M_NONYUSAKI MN ON TSM.SHUKKA_FLAG = MN.SHUKKA_FLAG");
            sb.ApdL("                     AND TSM.NONYUSAKI_CD = MN.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE TSM.BOX_NO = TTM.BOX_NO");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("              )");
            sb.ApdL(" GROUP BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TSM.SHUKKA_FLAG");
            sb.ApdL("     , TSM.NONYUSAKI_CD");
            sb.ApdL("     , MN.BUKKEN_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(出荷日チェック)(パレット梱包用)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(出荷日チェック)(パレット梱包用)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>Y.Higuchi 2010/09/22</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_PalletShukkaDateCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_SHUKKA_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.BOX_NO = TTM.BOX_NO");
            sb.ApdL("                  AND TSM.SHUKKA_DATE IS NOT NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 検品用

    #region 取込チェック(TehaiNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(TehaiNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_TehaiNoCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TEMPM");
            sb.ApdL(" WHERE");
            sb.ApdL("       NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_TEHAI_SKS TTS");
            sb.ApdL("                    INNER JOIN T_TEHAI_MEISAI_SKS TTMS");
            sb.ApdL("                            ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("                    INNER JOIN T_TEHAI_MEISAI TTM");
            sb.ApdL("                            ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdL("                    WHERE TTS.TEHAI_NO = TEMPM.TEHAI_NO");
            sb.ApdN("                      AND TTS.KENPIN_UMU = ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdL("                  )");
            sb.ApdN("   AND TEMPM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TEMPM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TEMPM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(手配種別Noチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(手配種別Noチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_TehaiKindFlagCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TEMPM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEHAI_SKS TTS");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI_SKS TTMS");
            sb.ApdL("                        ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI TTM");
            sb.ApdL("                        ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdN("                       AND (TTM.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_AGGRIGATE");
            sb.ApdN("                         OR TTM.TEHAI_KIND_FLAG = ").ApdN(this.BindPrefix).ApdL("TEHAI_KIND_ESTIMATE)");
            sb.ApdL("                WHERE TTS.TEHAI_NO = TEMPM.TEHAI_NO");
            sb.ApdN("                  AND TTS.KENPIN_UMU = ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdL("              )");
            sb.ApdN("   AND TEMPM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TEMPM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TEMPM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_AGGRIGATE", TEHAI_KIND_FLAG.AGGRIGATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_KIND_ESTIMATE", TEHAI_KIND_FLAG.ESTIMATE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(入荷数超過チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(入荷数超過チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/11/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_NyukaQtyCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TEMPM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEHAI_SKS TTS");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI_SKS TTMS");
            sb.ApdL("                        ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI TTM");
            sb.ApdL("                        ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdL("                WHERE TTS.TEHAI_NO = TEMPM.TEHAI_NO");
            sb.ApdN("                  AND TTS.KENPIN_UMU = ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdL("                  AND TTS.TEHAI_QTY < (TTS.ARRIVAL_QTY + TEMPM.NYUKA_QTY)");
            sb.ApdL("              )");
            sb.ApdN("   AND TEMPM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TEMPM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TEMPM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 取込チェック(完納時入荷数チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(完納時入荷数チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.SASAYAMA 2023/07/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_NyukaQtyKannouCheck(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT *");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TEMPM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       T_TEHAI_SKS TTS");
            sb.ApdL("       ON TTS.TEHAI_NO = TEMPM.TEHAI_NO");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEHAI_SKS TTS");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI_SKS TTMS");
            sb.ApdL("                        ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("                INNER JOIN T_TEHAI_MEISAI TTM");
            sb.ApdL("                        ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdL("                WHERE TTS.TEHAI_NO = TEMPM.TEHAI_NO");
            sb.ApdN("                  AND TTS.KENPIN_UMU = ").ApdN(this.BindPrefix).ApdL("KENPIN_UMU");
            sb.ApdL("                  AND TTS.TEHAI_QTY > TEMPM.NYUKA_QTY");
            sb.ApdL("                  AND TTS.HACCHU_ZYOTAI_FLAG = '2'");
            sb.ApdL("              )");
            sb.ApdN("   AND TEMPM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TEMPM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TEMPM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("KENPIN_UMU", KENPIN_UMU.ON_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 計測用

    #region 取込チェック(未集荷チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(未集荷チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_MiShukaDateCheckForMeasure(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                WHERE TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("                  AND TSM.SHUKA_DATE IS NULL");
            sb.ApdL("              )");
            sb.ApdN("   AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 一時取込明細の指定状態区分のデータ数取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細の指定状態区分のデータ数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>データ数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>Y.Higuchi 2010/10/27</update>
    /// <update>T.SASAYAMA 2023/07/21</update> 検品保留対応
    /// <update></update>
    /// --------------------------------------------------
    public int GetTempworkMeisaiResultNum(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdL("       T_TEMPWORK_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" OR");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_HORYU");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", cond.Result));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT_HORYU", RESULT.HORYU_VALUE1));

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

    #region 一時取込明細データ

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込明細データ</returns>
    /// <create>Y.Higuchi 2010/08/10</create>
    /// <update>H.Tajimi 2018/10/29 HT検品、計測対応</update>
    /// --------------------------------------------------
    public DataSet GetTempworkMeisai(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TTM.TEMP_ID");
            sb.ApdL("     , TTM.ROW_NO");
            sb.ApdL("     , TTM.RESULT");
            sb.ApdL("     , COM.ITEM_NAME AS RESULT_STRING");
            sb.ApdL("     , TTM.PALLET_NO");
            sb.ApdL("     , TTM.BOX_NO");
            sb.ApdL("     , TTM.DATA_NO");
            sb.ApdL("     , TTM.DESCRIPTION");
            sb.ApdL("     , TTM.NONYUSAKI_NAME");
            sb.ApdL("     , LTRIM(COALESCE(TTM.SHIP,'')) + LTRIM(COALESCE(TTM.AR_NO,'')) AS SHIP_AR_NO");
            sb.ApdL("     , TTM.TEHAI_NO");
            sb.ApdL("     , TTM.NYUKA_QTY");
            sb.ApdL("     , TTM.WEIGHT");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'RESULT'");
            sb.ApdL("                        AND COM.VALUE1 = TTM.RESULT");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdN("       TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEMPWORK_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region 一時取込明細データ(検品)取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ(検品)取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>一時取込明細データ(検品)</returns>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTempworkMeisaiKenpin(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TTM.TEMP_ID");
            sb.ApdL("     , TTM.ROW_NO");
            sb.ApdL("     , TTM.RESULT");
            sb.ApdL("     , COM.ITEM_NAME AS RESULT_STRING");
            sb.ApdL("     , TTM.PALLET_NO");
            sb.ApdL("     , TTM.BOX_NO");
            sb.ApdL("     , TTM.DATA_NO");
            sb.ApdL("     , TTM.DESCRIPTION");
            sb.ApdL("     , TTM.NONYUSAKI_NAME");
            sb.ApdL("     , LTRIM(COALESCE(TTM.SHIP,'')) + LTRIM(COALESCE(TTM.AR_NO,'')) AS SHIP_AR_NO");
            sb.ApdL("     , TTM.TEHAI_NO");
            sb.ApdL("     , TTM.NYUKA_QTY");
            sb.ApdL("     , TTM.WEIGHT");
            sb.ApdL("     , TTM.HANDY_LOGIN_ID");
            sb.ApdL("     , TTSW.CUSTOMER_NAME");
            sb.ApdL("     , TTSW.NONYUBASHO");
            sb.ApdL("     , TTSW.SEIBAN_CODE");
            sb.ApdL("     , TTSW.ECS_NO");
            sb.ApdL("     , TTSW.HINBAN");
            sb.ApdL("     , TTSW.KATASHIKI");
            sb.ApdL("     , TTSW.CHUMONSHO_HINMOKU");
            sb.ApdL("     , TTSW.TEHAI_QTY");
            sb.ApdL("     , TT.TORIKOMI_DATE");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'RESULT'");
            sb.ApdL("                        AND COM.VALUE1 = TTM.RESULT");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN T_TEHAI_SKS_WORK TTSW ON TTSW.TEHAI_NO = TTM.TEHAI_NO");
            sb.ApdL("  LEFT JOIN T_TEMPWORK TT ON TT.TEMP_ID = TTM.TEMP_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       TTM.ROW_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_TEMPWORK_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    #region 納入先取得

    /// --------------------------------------------------
    /// <summary>
    /// 納入先の取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetNonyusaki(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");
            if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                sb.ApdN("   AND BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            }
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                sb.ApdN("   AND PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            }
            sb.ApdL(" GROUP BY");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");

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

    #region 物件名一覧取得

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetProjectList(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable ret = new DataTable(Def_M_PROJECT.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT PROJECT_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("FROM M_PROJECT");
            sb.ApdL("ORDER BY");
            sb.ApdL("       BUKKEN_NAME");
            sb.ApdL("     , PROJECT_NO");

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ret);

            return ret;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 物件名一覧取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetBukkenList(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable ret = new DataTable(Def_M_BUKKEN.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT BUKKEN_NAME");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("  FROM M_BUKKEN");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BUKKEN_NAME");
            sb.ApdL("     , BUKKEN_NO");

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

    #region Handy操作履歴取得

    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns></returns>
    /// <create>H.Tajimi 2019/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    internal DataTable Sql_GetHandyOpeRireki(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            DataTable ret = new DataTable(Def_T_HANDY_RIREKI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParameter = dbHelper;

            // SQL文
            sb.ApdL("SELECT T_HANDY_RIREKI.*");
            sb.ApdL("     , COM1.ITEM_NAME AS SHUKKA_FLAG_NAME");
            sb.ApdL("     , COM2.ITEM_NAME AS HANDY_OPERATION_FLAG_NAME");
            sb.ApdL("  FROM T_HANDY_RIREKI");
            sb.ApdL("  LEFT JOIN M_COMMON COM1");
            sb.ApdN("         ON COM1.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_SHUKKA_FLAG");
            sb.ApdN("        AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("        AND COM1.VALUE1 = T_HANDY_RIREKI.SHUKKA_FLAG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2");
            sb.ApdN("         ON COM2.GROUP_CD = ").ApdN(this.BindPrefix).ApdL("GC_HANDY_OPERATION_FLAG");
            sb.ApdN("        AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("        AND COM2.VALUE1 = T_HANDY_RIREKI.HANDY_OPERATION_FLAG");
            sb.ApdN(" WHERE HANDY_OPERATION_FLAG = ").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG");
            sb.ApdN("   AND T_HANDY_RIREKI.IMPORT_DATE BETWEEN ").ApdN(this.BindPrefix).ApdN("DATE_FROM AND ").ApdN(this.BindPrefix).ApdL("DATE_TO");
            if (cond.HandyOpeFlag == HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1)
            {
                sb.ApdN("   AND T_HANDY_RIREKI.PROJECT_NO = ").ApdN(this.BindPrefix).ApdL("ProjectNo");
                paramCollection.Add(iNewParameter.NewDbParameter("ProjectNo", cond.ProjectNo));
            }
            else
            {
                sb.ApdN("   AND T_HANDY_RIREKI.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                sb.ApdN("   AND T_HANDY_RIREKI.BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BukkenNo");
                paramCollection.Add(iNewParameter.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
                paramCollection.Add(iNewParameter.NewDbParameter("BukkenNo", cond.BukkenNo));
            }
            if (!string.IsNullOrEmpty(cond.HandyLoginID))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.HANDY_LOGIN_ID = ").ApdN(this.BindPrefix).ApdL("HandyLoginID");
                paramCollection.Add(iNewParameter.NewDbParameter("HandyLoginID", cond.HandyLoginID));
            }
            if (!string.IsNullOrEmpty(cond.TehaiNo))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TehaiNo");
                paramCollection.Add(iNewParameter.NewDbParameter("TehaiNo", cond.TehaiNo));
            }
            if (!string.IsNullOrEmpty(cond.Ship))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.SHIP = ").ApdN(this.BindPrefix).ApdL("Ship");
                paramCollection.Add(iNewParameter.NewDbParameter("Ship", cond.Ship));
            }
            if (!string.IsNullOrEmpty(cond.ARNo))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.AR_NO = ").ApdN(this.BindPrefix).ApdL("ARNo");
                paramCollection.Add(iNewParameter.NewDbParameter("ARNo", cond.ARNo));
            }
            if (!string.IsNullOrEmpty(cond.TagNo))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TagNo");
                paramCollection.Add(iNewParameter.NewDbParameter("TagNo", cond.TagNo));
            }
            if (!string.IsNullOrEmpty(cond.BoxNo))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BoxNo");
                paramCollection.Add(iNewParameter.NewDbParameter("BoxNo", cond.BoxNo));
            }
            if (!string.IsNullOrEmpty(cond.PalletNo))
            {
                sb.ApdN("   AND T_HANDY_RIREKI.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PalletNo");
                paramCollection.Add(iNewParameter.NewDbParameter("PalletNo", cond.PalletNo));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       IMPORT_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParameter.NewDbParameter("HANDY_OPERATION_FLAG", cond.HandyOpeFlag));
            paramCollection.Add(iNewParameter.NewDbParameter("DATE_FROM", cond.DateFrom));
            paramCollection.Add(iNewParameter.NewDbParameter("DATE_TO", cond.DateTo));
            paramCollection.Add(iNewParameter.NewDbParameter("GC_SHUKKA_FLAG", SHUKKA_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("GC_HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.GROUPCD));
            paramCollection.Add(iNewParameter.NewDbParameter("LANG", cond.LoginInfo.Language));

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

    #region 一時取込データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dt">一時取込データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsTempwork(DatabaseHelper dbHelper, CondK04 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_TEMPWORK");
            sb.ApdL("(");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , TORIKOMI_DATE");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , ERROR_NUM");
            sb.ApdL("     , STATUS_FLAG");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TORIKOMI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DATA_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ERROR_NUM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS_FLAG");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_TEMPWORK.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_DATE", cond.TorikomiDate));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_TEMPWORK.TORIKOMI_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("DATA_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK.DATA_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ERROR_NUM", ComFunc.GetFldObject(dr, Def_T_TEMPWORK.ERROR_NUM, 0)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS_FLAG", ComFunc.GetFldObject(dr, Def_T_TEMPWORK.STATUS_FLAG, 0)));

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

    #region 一時取込明細データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <param name="dt">一時取込明細データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update>H.Tajimi 2018/10/25 HT検品、計測追加対応</update>
    /// <update>K.Tsutsumi 2019/08/29 Handy操作履歴対応</update>
    /// --------------------------------------------------
    public int InsTempworkMeisai(DatabaseHelper dbHelper, CondK04 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_TEMPWORK_MEISAI");
            sb.ApdL("(");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , DATA_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ROW_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DATA_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WEIGHT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("HANDY_LOGIN_ID");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("RESULT", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.RESULT, RESULT.OK_VALUE1)));
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.PALLET_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.BOX_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("DATA_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.DATA_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.DESCRIPTION, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.TEHAI_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("NYUKA_QTY", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.NYUKA_QTY, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("WEIGHT", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.WEIGHT, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("HANDY_LOGIN_ID", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.HANDY_LOGIN_ID, DBNull.Value)));

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

    #region Boxリスト管理データ登録

    /// --------------------------------------------------
    /// <summary>
    /// Boxリスト管理データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsBoxlistManage(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_BOXLIST_MANAGE");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , LISTHAKKO_FLAG");
            sb.ApdL("     , KANRI_FLAG");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("BOX_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LISTHAKKO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            paramCollection.Add(iNewParam.NewDbParameter("LISTHAKKO_FLAG", LISTHAKKO_FLAG.MIHAKKO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.MIKAN_VALUE1));
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

    #region パレットリスト管理データ登録

    /// --------------------------------------------------
    /// <summary>
    /// パレットリスト管理データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsPalletlistManage(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_PALLETLIST_MANAGE");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , LISTHAKKO_FLAG");
            sb.ApdL("     , KANRI_FLAG");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LISTHAKKO_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRI_FLAG");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL(")");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", cond.ShukkaFlag));
            paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", cond.NonyusakiCD));
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            paramCollection.Add(iNewParam.NewDbParameter("LISTHAKKO_FLAG", LISTHAKKO_FLAG.MIHAKKO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("KANRI_FLAG", KANRI_FLAG.MIKAN_VALUE1));
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

    #region Handy操作履歴データ登録(引渡)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(引渡)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>T.SASAYAMA 2023/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsHandyRireki_Hikiwatashi(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(4)) AS PROJECT_NO");
            sb.ApdL("     , TSM.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN NULL ELSE MN.SHIP END AS SHIP");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE NULL END AS AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS BOX_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(7)) AS WEIGHT");
            sb.ApdL("     , CAST(NULL AS NCHAR(8)) AS TEHAI_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(6)) AS NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TSM.SHUKKA_FLAG = SUBSTRING(TTWM.DATA_NO, 1, 1) AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTWM.DATA_NO, 2, 4) AND TSM.TAG_NO = SUBSTRING(TTWM.DATA_NO, 6, 5)");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.HIKIWATASHI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region Handy操作履歴データ登録(現品集荷)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(現品集荷)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsHandyRireki_GenpinShuka(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");           
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(4)) AS PROJECT_NO");
            sb.ApdL("     , TSM.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN NULL ELSE MN.SHIP END AS SHIP");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE NULL END AS AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS BOX_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(7)) AS WEIGHT");
            sb.ApdL("     , CAST(NULL AS NCHAR(8)) AS TEHAI_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(6)) AS NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TSM.SHUKKA_FLAG = SUBSTRING(TTWM.DATA_NO, 1, 1) AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTWM.DATA_NO, 2, 4) AND TSM.TAG_NO = SUBSTRING(TTWM.DATA_NO, 6, 5)");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.SHUKA_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region Handy操作履歴データ登録(Box梱包)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(Box梱包)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsHandyRireki_Box(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(4)) AS PROJECT_NO");
            sb.ApdL("     , TSM.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN NULL ELSE MN.SHIP END AS SHIP");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE NULL END AS AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , TTWM.BOX_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(7)) AS WEIGHT");
            sb.ApdL("     , CAST(NULL AS NCHAR(8)) AS TEHAI_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(6)) AS NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TSM.SHUKKA_FLAG = SUBSTRING(TTWM.DATA_NO, 1, 1) AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTWM.DATA_NO, 2, 4) AND TSM.TAG_NO = SUBSTRING(TTWM.DATA_NO, 6, 5)");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.BOX_KONPO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region Handy操作履歴データ登録(Pallet梱包)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(Pallet梱包)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
    /// <update></update>
    /// <remarks>パレット梱包では、AR No.を１つに絞れないので保持しない</remarks>
    /// --------------------------------------------------
    public int InsHandyRireki_Pallet(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(4)) AS PROJECT_NO");
            sb.ApdL("     , TBM.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN NULL ELSE MN.SHIP END AS SHIP");
            sb.ApdN("     , CAST(NULL AS NCHAR(6)) AS AR_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(5)) AS TAG_NO");
            sb.ApdL("     , TTWM.BOX_NO");
            sb.ApdL("     , TTWM.PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(7)) AS WEIGHT");
            sb.ApdL("     , CAST(NULL AS NCHAR(8)) AS TEHAI_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(6)) AS NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN T_BOXLIST_MANAGE TBM ON TBM.BOX_NO = TTWM.BOX_NO AND TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TBM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TBM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.PALLET_KONPO_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region Handy操作履歴データ登録(計測)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(計測)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsHandyRireki_Measure(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(4)) AS PROJECT_NO");
            sb.ApdL("     , TSM.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , MN.NONYUSAKI_CD");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN NULL ELSE MN.SHIP END AS SHIP");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN TSM.AR_NO ELSE NULL END AS AR_NO");
            sb.ApdL("     , TSM.TAG_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS BOX_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , TTWM.WEIGHT");
            sb.ApdL("     , CAST(NULL AS NCHAR(8)) AS TEHAI_NO");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(6)) AS NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI TSM ON TSM.SHUKKA_FLAG = SUBSTRING(TTWM.DATA_NO, 1, 1) AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTWM.DATA_NO, 2, 4) AND TSM.TAG_NO = SUBSTRING(TTWM.DATA_NO, 6, 5)");
            sb.ApdL(" INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.SHUKKA_FLAG = MN.SHUKKA_FLAG AND MB.BUKKEN_NO = MN.BUKKEN_NO");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.MEASURE_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region Handy操作履歴データ登録(検品)
    /// --------------------------------------------------
    /// <summary>
    /// Handy操作履歴データ登録(検品)
    /// </summary>
    /// <param name="dbHelper">データベースヘルパー</param>
    /// <param name="cond">条件</param>
    /// <returns>件数</returns>
    /// <create>K.Tsutsumi 2019/09/07</create>
    /// <update></update>
    /// <remarks>K.Tsutsumi 2019/09/07 複数の物件に対し１つの手配No.が設定される可能性がある。この場合は、両方の物件に対し履歴を残す。この状態が異常だとしても異常を探すという意味でこの方法が良いと考える。</remarks>
    /// --------------------------------------------------
    public int InsHandyRireki_Kenpin(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_HANDY_RIREKI (");
            sb.ApdL("       HANDY_OPERATION_FLAG");
            sb.ApdL("     , HANDY_LOGIN_ID");
            sb.ApdL("     , PROJECT_NO");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , BUKKEN_NAME");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , SHIP");
            sb.ApdL("     , AR_NO");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , BOX_NO");
            sb.ApdL("     , PALLET_NO");
            sb.ApdL("     , WEIGHT");
            sb.ApdL("     , TEHAI_NO");
            sb.ApdL("     , NYUKA_QTY");
            sb.ApdL("     , IMPORT_DATE");
            sb.ApdL("     , IMPORT_USER_ID");
            sb.ApdL(")");
            sb.ApdL("SELECT");
            sb.ApdN("       CAST(").ApdN(this.BindPrefix).ApdL("HANDY_OPERATION_FLAG AS NCHAR(2)) AS HANDY_OPERATION_FLAG");
            sb.ApdL("     , RTRIM(TTWM.HANDY_LOGIN_ID) AS HANDY_LOGIN_ID");
            sb.ApdL("     , MB.PROJECT_NO");
            sb.ApdL("     , MB.SHUKKA_FLAG");
            sb.ApdL("     , MB.BUKKEN_NO");
            sb.ApdL("     , MB.BUKKEN_NAME");
            sb.ApdL("     , CAST(NULL AS NCHAR(4)) AS NONYUSAKI_CD");
            sb.ApdL("     , CAST(NULL AS NVARCHAR(10)) AS SHIP");
            sb.ApdN("     , CASE MB.SHUKKA_FLAG WHEN ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR THEN RTRIM(ME.AR_NO) ELSE NULL END AS AR_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(5)) AS TAG_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS BOX_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(6)) AS PALLET_NO");
            sb.ApdL("     , CAST(NULL AS NCHAR(7)) AS WEIGHT");
            sb.ApdL("     , TTWM.TEHAI_NO");
            sb.ApdL("     , TTWM.NYUKA_QTY");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_DATE AS IMPORT_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("IMPORT_USER_ID AS IMPORT_USER_ID");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("       SELECT");
            sb.ApdL("              TTM.ECS_QUOTA");
            sb.ApdL("            , TTM.ECS_NO");
            sb.ApdL("            , TTS.TEHAI_NO");
            sb.ApdL("         FROM T_TEHAI_SKS TTS");
            sb.ApdN("        INNER JOIN T_TEMPWORK_MEISAI TTWM ON TTWM.TEHAI_NO = TTS.TEHAI_NO AND TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI_SKS TTMS ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdL("        GROUP BY");
            sb.ApdL("            TTM.ECS_QUOTA");
            sb.ApdL("          , TTM.ECS_NO");
            sb.ApdL("          , TTS.TEHAI_NO");
            sb.ApdL("        ) AS TTM ON TTM.TEHAI_NO = TTWM.TEHAI_NO AND TTWM.TEMP_ID = @TEMP_ID");
            sb.ApdL(" INNER JOIN M_ECS ME ON ME.ECS_QUOTA = TTM.ECS_QUOTA AND ME.ECS_NO = TTM.ECS_NO");
            sb.ApdL(" INNER JOIN M_BUKKEN MB ON MB.PROJECT_NO = ME.PROJECT_NO AND MB.SHUKKA_FLAG = CASE WHEN RTRIM(LTRIM(ME.AR_NO))='' OR ME.AR_NO IS NULL THEN ").ApdN(this.BindPrefix).ApdN("SHUKKA_FLAG_NORMAL ELSE ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG_AR END");

            sb.ApdL(" WHERE");
            sb.ApdN("       TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TTWM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("HANDY_OPERATION_FLAG", HANDY_OPERATION_FLAG.NYUKA_KENPIN_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("IMPORT_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    #region 出荷明細データ集荷日更新(引渡)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ集荷日更新(引渡)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.SASAYAMA 2023/07/14</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiHikiwatashi(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdN("     , HIKIWATASHI_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("                  AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("              )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", DISP_JYOTAI_FLAG.HIKIWATASHIZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 出荷明細データ集荷日更新(現品集荷)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ集荷日更新(現品集荷)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiGenpinShuka(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdN("     , SHUKA_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("                  AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("              )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.SHUKAZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 出荷明細データBox梱包日更新(Box梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ集荷日更新(Box梱包)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiBox(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdN("     , BOXKONPO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE T_SHUKKA_MEISAI.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND T_SHUKKA_MEISAI.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND T_SHUKKA_MEISAI.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("              )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.BOXZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", cond.BoxNo));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 出荷明細データBox梱包日更新(パレット梱包)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ集荷日更新(パレット梱包)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiPallet(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdN("     , PALLETKONPO_DATE = CONVERT(NVARCHAR, ").ApdN(this.SysDate).ApdL(", 111)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE T_SHUKKA_MEISAI.BOX_NO = TTM.BOX_NO");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("              )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("JYOTAI_FLAG", JYOTAI_FLAG.PALLETZUMI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", cond.PalletNo));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 出荷明細データ重量更新(計測)

    /// --------------------------------------------------
    /// <summary>
    /// 出荷明細データ重量更新(計測)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update>H.Tajimi 2020/04/17 計測の取り込みはエラーがないものだけでもできるよう変更</update>
    /// --------------------------------------------------
    public int UpdShukkaMeisaiMeasure(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdL("       GRWT = (");
            sb.ApdL("               SELECT TTM.WEIGHT");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("                  AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("              )");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL(" WHERE");
            sb.ApdL("       EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM T_TEMPWORK_MEISAI TTM");
            sb.ApdL("                WHERE SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("                  AND TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("                  AND TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdN("                  AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("                  AND TTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("              )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));

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

    #region 手配明細データ更新(検品)

    /// --------------------------------------------------
    /// <summary>
    /// 手配明細データ更新(検品)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/10/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiMeisaiKenpin(DatabaseHelper dbHelper, CondK04 cond)
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
            sb.ApdN("       T_TEHAI_MEISAI.ARRIVAL_QTY = (T_TEHAI_MEISAI.ARRIVAL_QTY + ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY)");
            sb.ApdN("     , T_TEHAI_MEISAI.UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , T_TEHAI_MEISAI.UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , T_TEHAI_MEISAI.UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_TEHAI_MEISAI");
            sb.ApdL(" INNER JOIN T_TEHAI_MEISAI_SKS");
            sb.ApdL("         ON T_TEHAI_MEISAI_SKS.TEHAI_RENKEI_NO = T_TEHAI_MEISAI.TEHAI_RENKEI_NO");
            sb.ApdN("        AND T_TEHAI_MEISAI_SKS.TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", cond.NyukaQty));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
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

    #region SKS手配明細データ更新(検品)

    /// --------------------------------------------------
    /// <summary>
    /// SKS手配明細データ更新(検品)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>H.Tajimi 2018/11/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTehaiSksKenpin(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEHAI_SKS");
            sb.ApdL("SET");
            sb.ApdN("       ARRIVAL_QTY = (ARRIVAL_QTY + ").ApdN(this.BindPrefix).ApdL("ARRIVAL_QTY)");
            sb.ApdN("     , UPDATE_DATE = ").ApdN(this.BindPrefix).ApdL("UPDATE_DATE");
            sb.ApdN("     , UPDATE_USER_ID = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , UPDATE_USER_NAME = ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , VERSION = ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM T_TEHAI_SKS");
            sb.ApdL(" WHERE TEHAI_NO = ").ApdN(this.BindPrefix).ApdL("TEHAI_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ARRIVAL_QTY", cond.NyukaQty));
            paramCollection.Add(iNewParam.NewDbParameter("TEHAI_NO", cond.TehaiNo));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_DATE", cond.UpdateDate));
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

    #region 一時取込データの状態区分のみ更新

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの状態区分のみ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkStatusFlagOnly(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK");
            sb.ApdL("SET");
            sb.ApdN("       STATUS_FLAG = ").ApdN(this.BindPrefix).ApdL("STATUS_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("STATUS_FLAG", cond.StatusFlag));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 一時取込データの結果更新

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの結果更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkResult(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK");
            sb.ApdL("SET");
            sb.ApdN("       ERROR_NUM = ").ApdN(this.BindPrefix).ApdL("ERROR_NUM");
            sb.ApdN("     , STATUS_FLAG = ").ApdN(this.BindPrefix).ApdL("STATUS_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");


            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("ERROR_NUM", cond.ErrorNum));
            paramCollection.Add(iNewParam.NewDbParameter("STATUS_FLAG", cond.StatusFlag));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 一時取込データの初期化

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データの初期化
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdInitializeTempworkMeisai(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", string.Empty));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

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

    #region 一時取込明細データのエラー更新

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データのエラー更新
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkMeisaiError(DatabaseHelper dbHelper, DataTable dt, string message)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND ROW_NO = ").ApdN(this.BindPrefix).ApdL("ROW_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.NG_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", message));
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));

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
    /// 一時取込明細データのエラー更新(一括)
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <create>Y.Higuchi 2010/08/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkMeisaiError(DatabaseHelper dbHelper, CondK04 cond, string message)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_OK");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.NG_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", message));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.OK_VALUE1));

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
    /// 一時取込明細データのエラー更新保留用
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dt"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <create>T.SASAYAMA 2023/07/31</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkMeisaiErrorHoryu(DatabaseHelper dbHelper, DataTable dt, string message)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND ROW_NO = ").ApdN(this.BindPrefix).ApdL("ROW_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.HORYU_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", message));
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));

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

    #region 一時取込明細データの納入先更新（集荷・ボックス梱包・計測用）

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データの納入先更新（集荷・ボックス梱包・計測用）
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2016/01/19</create>
    /// <update>J.Chen 2024/11/11 結果OKのデータも更新する</update>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempWorkMeisaiNonyusaki(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE TTM");
            sb.ApdL("SET");
            sb.ApdL("       NONYUSAKI_NAME = MN.NONYUSAKI_NAME");
            sb.ApdL("     , AR_NO = TSM.AR_NO");
            sb.ApdL("     , SHIP = MN.SHIP");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL("  JOIN");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("             ON TSM.SHUKKA_FLAG = SUBSTRING(TTM.DATA_NO, 1, 1)");
            sb.ApdL("            AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(TTM.DATA_NO, 2, 4)");
            sb.ApdL("            AND TSM.TAG_NO = SUBSTRING(TTM.DATA_NO, 6, 5)");
            sb.ApdL("  JOIN");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL("             ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("            AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            //sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_OK");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            //paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.NG_VALUE1));

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

    #region 一時取込明細データの納入先更新（パレット用）

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データの納入先更新（パレット用）
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2016/01/19</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempWorkMeisaiNonyusakiPallet(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE TTM");
            sb.ApdL("SET");
            sb.ApdL("       NONYUSAKI_NAME = MN.NONYUSAKI_NAME");
            sb.ApdL("     , AR_NO = TSM.AR_NO");
            sb.ApdL("     , SHIP = MN.SHIP");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTM");
            sb.ApdL("  JOIN");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("             ON TSM.BOX_NO = TTM.BOX_NO");
            sb.ApdN("            AND TTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("  JOIN");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL("             ON MN.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("            AND MN.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_OK");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.NG_VALUE1));

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

    #region 一時取込明細データの納入先更新（検品用）

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データの納入先更新（検品用）
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>H.Tajimi 2018/10/30</create>
    /// <update>T.SASAYAMA 2023/07/21</update> 検品保留対応
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempWorkMeisaiNonyusakiKenpin(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE TTWM");
            sb.ApdL("SET");
            sb.ApdL("       NONYUSAKI_NAME = MB.BUKKEN_NAME");
            sb.ApdL("     , AR_NO = ME.AR_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_TEMPWORK_MEISAI TTWM");
            sb.ApdL(" INNER JOIN (");
            sb.ApdL("       SELECT");
            sb.ApdL("              TTM.ECS_QUOTA");
            sb.ApdL("            , TTM.ECS_NO");
            sb.ApdL("            , TTS.TEHAI_NO");
            sb.ApdL("         FROM T_TEHAI_SKS TTS");
            sb.ApdN("        INNER JOIN T_TEMPWORK_MEISAI TTWM ON TTWM.TEHAI_NO = TTS.TEHAI_NO AND TTWM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI_SKS TTMS ON TTMS.TEHAI_NO = TTS.TEHAI_NO");
            sb.ApdL("        INNER JOIN T_TEHAI_MEISAI TTM ON TTM.TEHAI_RENKEI_NO = TTMS.TEHAI_RENKEI_NO");
            sb.ApdL("        GROUP BY");
            sb.ApdL("            TTM.ECS_QUOTA");
            sb.ApdL("          , TTM.ECS_NO");
            sb.ApdL("          , TTS.TEHAI_NO");
            sb.ApdL("        ) AS TTM ON TTM.TEHAI_NO = TTWM.TEHAI_NO AND TTWM.TEMP_ID = @TEMP_ID");
            sb.ApdL("  JOIN");
            sb.ApdL("       M_ECS ME");
            sb.ApdL("             ON ME.ECS_QUOTA = TTM.ECS_QUOTA");
            sb.ApdL("            AND ME.ECS_NO = TTM.ECS_NO");
            sb.ApdL("  JOIN M_BUKKEN MB");
            sb.ApdL("             ON MB.PROJECT_NO = ME.PROJECT_NO");
            sb.ApdL("            AND MB.SHUKKA_FLAG = CASE WHEN RTRIM(LTRIM(ME.AR_NO))='' OR ME.AR_NO IS NULL THEN @SHUKKA_FLAG_NORMAL ELSE @SHUKKA_FLAG_AR END");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_OK");
            sb.ApdL(" OR");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_HORYU");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.NG_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT_HORYU", RESULT.HORYU_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_NORMAL", SHUKKA_FLAG.NORMAL_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG_AR", SHUKKA_FLAG.AR_VALUE1));

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

    /// --------------------------------------------------
    /// <summary>
    /// 旧一時取込明細データのOKレコードを削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">K04用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>H.Tsuji 2020/06/17</create>
    /// <update>T.SASAYAMA 2023/07/21</update> 検品保留対応
    /// <update></update>
    /// --------------------------------------------------
    public int DelOldKenpinMeisaiOK(DatabaseHelper dbHelper, CondK04 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE FROM T_TEMPWORK_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT_OK");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            if (cond.HoryuRetry == false)
            {
                paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.OK_VALUE1));
            }
            else if (cond.HoryuRetry == true)
            {
                paramCollection.Add(iNewParam.NewDbParameter("RESULT_OK", RESULT.HORYU_VALUE1));
            }

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
}
