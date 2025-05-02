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
/// 取込結果処理（部品管理用）（データアクセス層） 
/// </summary>
/// <create>T.Wakamatsu 2013/08/23</create>
/// <update></update>
/// --------------------------------------------------
public class WsI02Impl : WsBaseImpl
{
    #region Constructors

    /// --------------------------------------------------
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public WsI02Impl()
        : base()
    {
    }

    #endregion

    #region 制御

    #region ハンディ一時取込データの取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>T.Wakamatsu 2013/09/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTempworkData(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = this.GetTempwork(dbHelper, cond).Tables[Def_T_BUHIN_TEMPWORK.Name];

            ds.Tables.Add(dt.Clone());
            // 表示データを入出庫と棚卸で整理する。

            string nyuhukko = string.Empty;
            using (WsCommonImpl impl = new WsCommonImpl())
            {
                CondCommon condCom = new CondCommon(cond.LoginInfo);
                condCom.GroupCD = ZAIKO_TORIKOMI_FLAG.GROUPCD;
                DataSet dsCom = impl.GetCommon(dbHelper, condCom);
                
                DataRow dr = dsCom.Tables[Def_M_COMMON.Name].Select(
                    Def_M_COMMON.VALUE1 + " = '" + ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1 + "'")[0];
                nyuhukko = ComFunc.GetFld(dr,Def_M_COMMON.ITEM_NAME) + "/";
                dr = dsCom.Tables[Def_M_COMMON.Name].Select(
                    Def_M_COMMON.VALUE1 + " = '" + ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1 + "'")[0];
                nyuhukko += ComFunc.GetFld(dr, Def_M_COMMON.ITEM_NAME);
            }
            string curFlag = string.Empty;
            string curId = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                DataTable dtTemp = ds.Tables[Def_T_BUHIN_TEMPWORK.Name];
                string tempId = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID);
                string torikomiFlag = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG);
                if (curId != tempId)
                {
                    if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1 || torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                    {
                        dr[ComDefine.FLD_TORIKOMI_SAGYO] = nyuhukko;
                    }
                    dtTemp.Rows.Add(dr.ItemArray);
                    curId = tempId;
                    curFlag = torikomiFlag;
                }
                else if ((curFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1 || curFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1) &&
                    torikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                {
                    dtTemp.Rows.Add(dr.ItemArray);
                    curFlag = torikomiFlag;
                }
                else
                {
                    dtTemp.Rows[dtTemp.Rows.Count - 1][Def_T_BUHIN_TEMPWORK.ERROR_NUM] = ComFunc.GetFldToDecimal(dr, Def_T_BUHIN_TEMPWORK.ERROR_NUM) +
                        ComFunc.GetFldToDecimal(dtTemp, dtTemp.Rows.Count - 1, Def_T_BUHIN_TEMPWORK.ERROR_NUM);
                }
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region TEMP_IDの採番及び更新

    #region 一時取込データにTEMP_IDを採番しセットする。

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データにTEMP_IDを採番しセットする。
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="ds">一時取込データ、一時取込明細データを格納しているDataSet</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool SetTempID(DatabaseHelper dbHelper, CondI02 cond, DataSet ds, DataTable dtMessage)
    {
        try
        {
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }
            CondSms condSms = new CondSms(cond.LoginInfo);
            condSms.LoginInfo = cond.LoginInfo.Clone();
            condSms.SaibanFlag = SAIBAN_FLAG.ZAIKO_TEMP_ID_VALUE1;
            using (WsSmsImpl impl = new WsSmsImpl())
            {
                // 一時取込IDの採番
                string tempIDNyushukko = string.Empty;
                string tempIDTanaoroshi = string.Empty;
                string errMsgID = string.Empty;
                if (!impl.GetSaiban(dbHelper, condSms, out tempIDNyushukko, out errMsgID))
                {
                    ComFunc.AddMultiMessage(dtMessage, errMsgID, null);
                    return false;
                }
                if (!impl.GetSaiban(dbHelper, condSms, out tempIDTanaoroshi, out errMsgID))
                {
                    ComFunc.AddMultiMessage(dtMessage, errMsgID, null);
                    return false;
                }
                foreach (DataRow dr in ds.Tables[Def_T_BUHIN_TEMPWORK.Name].Rows)
                {
                    // 明細にTempIDが設定できたのでヘッダにも設定
                    if (ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG) == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                    {
                        dr[Def_T_BUHIN_TEMPWORK.TEMP_ID] = tempIDTanaoroshi;
                    }
                    else
                    {
                        dr[Def_T_BUHIN_TEMPWORK.TEMP_ID] = tempIDNyushukko;
                    }
                }
                foreach (DataRow dr in ds.Tables[Def_T_BUHIN_TEMPWORK_MEISAI.Name].Rows)
                {
                    // 明細にTempIDが設定できたのでヘッダにも設定
                    if (ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG) == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
                    {
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID] = tempIDTanaoroshi;
                    }
                    else
                    {
                        dr[Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID] = tempIDNyushukko;
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

    #endregion

    #region 一時取込データ、一時取込明細データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ、一時取込明細データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="ds">一時取込データ、一時取込明細データを格納しているDataSet</param>
    /// <returns>true:成功/false:失敗</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool InsTempworkData(DatabaseHelper dbHelper, CondI02 cond, DataSet ds)
    {
        try
        {
            this.InsTempwork(dbHelper, cond, ds.Tables[Def_T_BUHIN_TEMPWORK.Name]);
            this.InsTempworkMeisai(dbHelper, cond, ds.Tables[Def_T_BUHIN_TEMPWORK_MEISAI.Name]);

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
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool LockImportData(DatabaseHelper dbHelper, CondI02 cond, DataTable dtMessage)
    {
        try
        {
            CondI02 condI02 = (CondI02)cond.Clone();
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }

            // TEMP_IDから一時取込データを取得
            DataTable dt = this.LockTempwork(dbHelper, condI02);
            if (dt.Rows.Count < 1)
            {
                // 他端末で処理中です。
                ComFunc.AddMultiMessage(dtMessage, "I0200010001");
                return false;
            }
            // 一時取込データのステータス更新
            condI02.StatusFlag = STATUS_FLAG.TORIKOMICHU_VALUE1;
            this.UpdTempworkStatusFlagOnly(dbHelper, condI02);
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
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <param name="isError">取込時データで取込めない物があったかどうか</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportCheckData(DatabaseHelper dbHelper, CondI02 cond, DataTable dtMessage, ref bool isImportError)
    {
        try
        {
            CondI02 condI02 = (CondI02)cond.Clone();
            if (dtMessage == null)
            {
                dtMessage = ComFunc.GetSchemeMultiMessage();
            }

            DataTable dt = this.GetTempwork(dbHelper, condI02).Tables[Def_T_BUHIN_TEMPWORK.Name];
            // 一時取込明細データの初期化
            this.UpdInitializeTempworkMeisai(dbHelper, condI02);

            // インデックスデータ更新
            this.UpdTempworkMeisaiIndex(dbHelper, condI02);

            // 一斉チェック
            if (!this.CheckImportData(dbHelper, condI02))
            {
                isImportError = true;
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
    /// 取込データチェック
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.Wakamatsu 2013/08/26</create>
    /// <update>K.Tsutsumi 2018/09/05</update>
    /// <update></update>
    /// --------------------------------------------------
    public bool CheckImportData(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            bool ret = true;
            // MessageIDプロパティを使い回すので、別途宣言
            CondI02 condMsg = new CondI02(cond.LoginInfo);
            // ユーザーチェック
            using (DataTable dtCheck = GetTorikomi_UserCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないユーザーです。
                    condMsg.MessageID = "I0200010002";
                    string message = this.GetMessage(dbHelper, condMsg);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // PalletNoチェック
            using (DataTable dtCheck = GetTorikomi_PalletNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないPalletNo.です。
                    condMsg.MessageID = "I0200010003";
                    string message = this.GetMessage(dbHelper, condMsg);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // BoxNoチェック
            using (DataTable dtCheck = GetTorikomi_BoxNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないBoxNo.です。
                    condMsg.MessageID = "I0200010004";
                    string message = this.GetMessage(dbHelper, condMsg);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // TagNoチェック
            using (DataTable dtCheck = GetTorikomi_TagNoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 登録されていないTagNoです。
                    condMsg.MessageID = "I0200010005";
                    string message = this.GetMessage(dbHelper, condMsg);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 木枠梱包チェック
            using (DataTable dtCheck = GetTorikomi_KiwakuKonpoCheck(dbHelper, cond))
            {
                if (0 < dtCheck.Rows.Count)
                {
                    // 木枠梱包未完了です。
                    condMsg.MessageID = "I0200010032";
                    string message = this.GetMessage(dbHelper, condMsg);
                    this.UpdTempworkMeisaiError(dbHelper, dtCheck, message);
                    ret = false;
                }
            }
            // 入庫先Locationはなければ作成するのでLocationのチェックは必要ない

            // 在庫にない場合Location処理チェックは実行中チェック

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
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dtMessage">メッセージテーブル</param>
    /// <param name="dtErr">完了エラーデータ</param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportResultUpdateOtherTable(DatabaseHelper dbHelper, CondI02 cond, DataTable dtMessage, ref DataTable dtErr)
    {
        try
        {
            CondI02 condI02 = (CondI02)cond.Clone();
            condI02.UpdateDate = DateTime.Now;

            // ロケーション登録
            this.InsLocation(dbHelper, condI02);
            
            // 完了エラーテーブルスキーム作成
            dtErr.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, typeof(string));
            dtErr.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID, typeof(string));
            dtErr.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, typeof(string));
            dtErr.Columns.Add(Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, typeof(decimal));

            // 入庫・出庫
            this.ExecNyushukko(dbHelper, condI02, ref dtErr);

            if (dtErr.Rows.Count > 0)
            {
                return false;
            }

            // 棚卸削除データ収集
            DataTable dt = this.GetTanaoroshiForDelete(dbHelper, condI02);

            // 棚卸
            this.DelTanaoroshi(dbHelper, condI02, dt);
            this.InsTanaoroshi(dbHelper, condI02);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// --------------------------------------------------
    /// <summary>
    /// 入出庫
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dtErr">完了失敗データ</param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    private bool ExecNyushukko(DatabaseHelper dbHelper, CondI02 cond, ref DataTable dtErr)
    {
        try
        {
            CondI01 condI01 = new CondI01(cond.LoginInfo);
            condI01.LoginInfo = cond.LoginInfo.Clone();
            condI01.StockDate = DateTime.Now.ToString("yyyy/MM/dd");

            string errMsgID = string.Empty;
            string[] args = null;

            DataTable dt = this.GetTempworkMeisaiNyushukko(dbHelper, cond);

            using (WsI01Impl impl = new WsI01Impl())
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string torikomiFlag = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG);

                    if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1)
                    {
                        DataTable dtZaiko = this.GetZaiko(dbHelper, dr, cond.LoginInfo.Language);
                        // 入庫処理
                        condI01.ZaikoNo = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);
                        condI01.WorkUserID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID);
                        condI01.ShukkaFlag = ComFunc.GetFld(dtZaiko, 0, Def_T_SHUKKA_MEISAI.SHUKKA_FLAG);
                        condI01.BukkenNo = ComFunc.GetFld(dtZaiko, 0, Def_M_NONYUSAKI.BUKKEN_NO);

                        impl.InsZaikoData(dbHelper, condI01, dtZaiko, ref errMsgID, ref args);
                    }
                    else if (torikomiFlag == ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1)
                    {
                        // 在庫がない場合エラー、在庫があるデータのみ更新
                        DataTable dtKan = this.GetKanryo(dbHelper, dr, cond.LoginInfo.Language);
                        if (dtKan.Rows.Count == 0)
                        {
                            DataRow drNew = dtErr.NewRow();
                            drNew[Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID] = dr[Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID];
                            drNew[Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID] = dr[Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID];
                            drNew[Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG] = dr[Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG];
                            drNew[Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO] = dr[Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO];
                            dtErr.Rows.Add(drNew);
                        }
                        else
                        {
                            condI01.ZaikoNo = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);
                            condI01.WorkUserID = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID);
                            impl.DelKanryoData(dbHelper, condI01, dtKan, ref errMsgID, ref args);
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

    #region 一時取込データの状態区分を処理が終わった状態とする。

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの状態区分を処理が終わった状態とする。
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dtMessage"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool ImportEndStatus(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            CondI02 condI02 = (CondI02)cond.Clone();
            condI02.Result = RESULT.NG_VALUE1;
            DataTable dt = this.GetTempworkMeisaiResultNum(dbHelper, condI02);

            foreach (DataRow dr in dt.Rows)
            {
                if (ComFunc.GetFldToInt32(dr, Def_T_BUHIN_TEMPWORK.ERROR_NUM, 0) == 0)
                {
                    dr[Def_T_BUHIN_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.KANRYO_VALUE1;
                }
                else
                {
                    dr[Def_T_BUHIN_TEMPWORK.STATUS_FLAG] = STATUS_FLAG.ERROR_VALUE1;
                }
            }
            this.UpdTempworkResult(dbHelper, dt);
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
    /// <param name="cond">I02用</param>
    /// <param name="dtMessage">メッセージデータテーブル</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DestroyData(DatabaseHelper dbHelper, CondI02 cond, DataTable dtMessage)
    {
        try
        {
            // TEMP_IDから一時取込データを取得
            DataTable dt = this.LockTempwork(dbHelper, cond);
            if (dt.Rows.Count < 1)
            {
                // 他端末で処理中です。
                ComFunc.AddMultiMessage(dtMessage, "I0200010001", null);
                return false;
            }
            cond.StatusFlag = STATUS_FLAG.KANRYO_VALUE1;
            this.UpdTempworkStatusKanryo(dbHelper, cond);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #region 一時取込明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用</param>
    /// <param name="dt">削除データ</param>
    /// <returns>true:エラー無/false:エラー有り</returns>
    /// <create>T.Wakamatsu 2013/09/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public bool DelTempWorkMeisai(DatabaseHelper dbHelper, CondI02 cond, DataTable dt)
    {
        try
        {
            this.DelTempWorkMeisaiExec(dbHelper, cond, dt);
            this.DelTempWorkExec(dbHelper, cond);
            return true;
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
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update>K.Tsutsumi 2018/09/05</update>
    /// <update></update>
    /// --------------------------------------------------
    public string GetMessage(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            string ret = string.Empty;
            using (WsCommonImpl impl = new WsCommonImpl())
            {
                CondCommon condCommon = new CondCommon(cond.LoginInfo);
                condCommon.MessageID = cond.MessageID;
                DataSet ds = impl.GetMessage(dbHelper, condCommon);
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
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTempwork(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;
            string fieldPrefix = "BTW.";
            string fieldName = string.Empty;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       BTW.TEMP_ID");
            sb.ApdL("     , BTW.WORK_USER_ID");
            sb.ApdL("     , USR.USER_NAME");
            sb.ApdL("     , BTW.TORIKOMI_FLAG");
            sb.ApdL("     , BTW.TORIKOMI_DATE");
            sb.ApdL("     , COM.ITEM_NAME AS TORIKOMI_SAGYO");
            sb.ApdL("     , BTW.LOCATION");
            sb.ApdL("     , BTW.STOCK_NO");
            sb.ApdL("     , BTW.ERROR_NUM");
            sb.ApdL("     , BTW.STATUS_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BUHIN_TEMPWORK BTW");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'ZAIKO_TORIKOMI_FLAG'");
            sb.ApdL("                        AND COM.VALUE1 = BTW.TORIKOMI_FLAG");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER USR ON USR.USER_ID = BTW.WORK_USER_ID");
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
            sb.ApdL("       BTW.TEMP_ID DESC");
            sb.ApdL("     , BTW.TORIKOMI_FLAG ASC");
            sb.ApdL("     , BTW.WORK_USER_ID ASC");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));

            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_BUHIN_TEMPWORK.Name);

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
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込データ</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    private DataTable LockTempwork(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , TORIKOMI_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , ERROR_NUM");
            sb.ApdL("     , STATUS_FLAG");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK");
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

    #region 取込チェック(ユーザーチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(ユーザーチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.Wakamatsu 2013/08/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_UserCheck(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM M_USER MU");
            sb.ApdL("                    WHERE MU.USER_ID = BTM.WORK_USER_ID");
            sb.ApdL("                  )");

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

    #region 取込チェック(PalletNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(PalletNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.Wakamatsu 2013/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_PalletNoCheck(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) = 'P' ");
            sb.ApdL("   AND BTM.PALLET_NO IS NULL");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BTM.ROW_NO");

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
    /// <param name="cond">I02用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.Wakamatsu 2013/08/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_BoxNoCheck(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) = 'B' ");
            sb.ApdL("   AND BTM.BOX_NO IS NULL");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BTM.ROW_NO");

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

    #region 取込チェック(TagNoチェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(TagNoチェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_TagNoCheck(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) NOT IN ('B', 'P') ");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    WHERE TSM.SHUKKA_FLAG = BTM.SHUKKA_FLAG");
            sb.ApdL("                      AND TSM.NONYUSAKI_CD = BTM.NONYUSAKI_CD");
            sb.ApdL("                      AND TSM.TAG_NO = BTM.TAG_NO");
            sb.ApdL("                  )");

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

    #region 取込チェック(木枠梱包チェック)

    /// --------------------------------------------------
    /// <summary>
    /// 取込チェック(木枠梱包チェック)
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>チェック結果</returns>
    /// <create>T.Wakamatsu 2013/09/05</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTorikomi_KiwakuKonpoCheck(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable(Def_T_BUHIN_TEMPWORK_MEISAI.Name);
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND BTM.TAG_NO IS NOT NULL");
            sb.ApdL("   AND EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    INNER JOIN T_KIWAKU TKW ON TKW.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                    WHERE TSM.SHUKKA_FLAG = BTM.SHUKKA_FLAG");
            sb.ApdL("                      AND TSM.NONYUSAKI_CD = BTM.NONYUSAKI_CD");
            sb.ApdL("                      AND TSM.TAG_NO = BTM.TAG_NO");
            sb.ApdN("                      AND TKW.SAGYO_FLAG IN (").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG1").ApdN(", ").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG2").ApdL(")");
            sb.ApdL("                  )");
            sb.ApdL("UNION");
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND BTM.BOX_NO IS NOT NULL");
            sb.ApdL("   AND EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    INNER JOIN T_KIWAKU TKW ON TKW.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                    WHERE TSM.BOX_NO = BTM.BOX_NO");
            sb.ApdN("                      AND TKW.SAGYO_FLAG IN (").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG1").ApdN(", ").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG2").ApdL(")");
            sb.ApdL("                  )");
            sb.ApdL("UNION");
            sb.ApdL("SELECT");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND BTM.PALLET_NO IS NOT NULL");
            sb.ApdL("   AND EXISTS (");
            sb.ApdL("                   SELECT 1");
            sb.ApdL("                     FROM T_SHUKKA_MEISAI TSM");
            sb.ApdL("                    INNER JOIN T_KIWAKU TKW ON TKW.KOJI_NO = TSM.KOJI_NO");
            sb.ApdL("                    WHERE TSM.PALLET_NO = BTM.PALLET_NO");
            sb.ApdN("                      AND TKW.SAGYO_FLAG IN (").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG1").ApdN(", ").ApdN(this.BindPrefix).ApdN("SAGYO_FLAG2").ApdL(")");
            sb.ApdL("                  )");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG1", SAGYO_FLAG.KIWAKUMEISAI_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("SAGYO_FLAG2", SAGYO_FLAG.KONPOTOROKU_VALUE1));

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

    #region 一時取込明細の指定状態区分のデータ数取得

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細の指定状態区分のデータ数取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>データ数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTempworkMeisaiResultNum(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       BTW.TEMP_ID");
            sb.ApdL("     , BTW.WORK_USER_ID");
            sb.ApdL("     , BTW.TORIKOMI_FLAG");
            sb.ApdL("     , COALESCE(BTM.CNT, 0) AS ERROR_NUM");
            sb.ApdL("     , '' AS STATUS_FLAG");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BUHIN_TEMPWORK BTW");
            sb.ApdL("  LEFT JOIN (");
            sb.ApdL("       SELECT");
            sb.ApdL("              TEMP_ID");
            sb.ApdL("            , WORK_USER_ID");
            sb.ApdL("            , TORIKOMI_FLAG");
            sb.ApdL("            , COUNT(1) AS CNT");
            sb.ApdL("            , '' AS STATUS_FLAG");
            sb.ApdL("         FROM ");
            sb.ApdL("              T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL("        WHERE");
            sb.ApdN("              TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("          AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("        GROUP BY");
            sb.ApdL("              TEMP_ID");
            sb.ApdL("            , WORK_USER_ID");
            sb.ApdL("            , TORIKOMI_FLAG");
            sb.ApdL("      ) BTM ON BTM.TEMP_ID = BTW.TEMP_ID");
            sb.ApdL("           AND BTM.WORK_USER_ID = BTW.WORK_USER_ID");
            sb.ApdL("           AND BTM.TORIKOMI_FLAG = BTW.TORIKOMI_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTW.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", cond.Result));

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

    #region 一時取込明細データ

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込明細データ</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataSet GetTempworkMeisai(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       BTM.TEMP_ID");
            sb.ApdL("     , BTM.WORK_USER_ID");
            sb.ApdL("     , USR.USER_NAME");
            sb.ApdL("     , BTM.TORIKOMI_FLAG");
            sb.ApdL("     , COM2.ITEM_NAME AS TORIKOMI_SAGYO");
            sb.ApdL("     , BTM.ROW_NO");
            sb.ApdL("     , BTM.RESULT");
            sb.ApdL("     , COM.ITEM_NAME AS RESULT_STRING");
            sb.ApdL("     , BTM.WORK_DATE");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdL("     , BTM.STOCK_NO");
            sb.ApdL("     , BTM.DESCRIPTION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'RESULT'");
            sb.ApdL("                        AND COM.VALUE1 = BTM.RESULT");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_COMMON COM2 ON COM2.GROUP_CD = 'ZAIKO_TORIKOMI_FLAG'");
            sb.ApdL("                        AND COM2.VALUE1 = BTM.TORIKOMI_FLAG");
            sb.ApdN("                        AND COM2.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER USR ON USR.USER_ID = BTM.WORK_USER_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.TORIKOMI_FLAG IN (").ApdN(this.BindPrefix).ApdN("TORIKOMI_FLAG1, ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG2)");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BTM.WORK_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            if (cond.TorikomiFlag == ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1)
            {
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG1", cond.TorikomiFlag));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG2", cond.TorikomiFlag));
            }
            else
            {
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG1", ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG2", ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1));
            }
            // SQL実行
            dbHelper.Fill(sb.ToString(), paramCollection, ds, Def_T_BUHIN_TEMPWORK_MEISAI.Name);

            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion
    
    #region 一時取込明細データ（入出庫）

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ（入出庫）
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>一時取込明細データ</returns>
    /// <create>T.Wakamatsu 2013/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTempworkMeisaiNyushukko(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       BTM.TEMP_ID");
            sb.ApdL("     , BTM.WORK_USER_ID");
            sb.ApdL("     , USR.USER_NAME");
            sb.ApdL("     , BTM.TORIKOMI_FLAG");
            sb.ApdL("     , BTM.ROW_NO");
            sb.ApdL("     , BTM.RESULT");
            sb.ApdL("     , COM.ITEM_NAME AS RESULT_STRING");
            sb.ApdL("     , BTM.WORK_DATE");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdL("     , BTM.STOCK_NO");
            sb.ApdL("     , BTM.DESCRIPTION");
            sb.ApdL("  FROM ");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL("  LEFT JOIN M_COMMON COM ON COM.GROUP_CD = 'RESULT'");
            sb.ApdL("                        AND COM.VALUE1 = BTM.RESULT");
            sb.ApdN("                        AND COM.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL("  LEFT JOIN M_USER USR ON USR.USER_ID = BTM.WORK_USER_ID");
            sb.ApdL(" WHERE");
            sb.ApdN("       BTM.TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND BTM.TORIKOMI_FLAG IN (").ApdN(this.BindPrefix).ApdN("TORIKOMI_FLAG1, ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG2)");
            sb.ApdN("   AND BTM.RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL(" ORDER BY");
            sb.ApdL("       BTM.WORK_DATE");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", cond.LoginInfo.Language));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG1", ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG2", ZAIKO_TORIKOMI_FLAG.KANRYO_VALUE1));
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

    #region 入庫データ取得
    /// --------------------------------------------------
    /// <summary>
    /// 入庫データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">対象行</param>
    /// <param name="language">言語</param>
    /// <returns>入庫データ</returns>
    /// <create>T.Wakamatsu 2013/08/28</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/08/31 多言語化対応</update>
    /// --------------------------------------------------
    public DataTable GetZaiko(DatabaseHelper dbHelper, DataRow dr, String language)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdL("       ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID AS WORK_USER_ID");
            sb.ApdL("     , SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , ").ApdN(this.BindPrefix).ApdL("NYUKO_LOCATION AS NYUKO_LOCATION");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , MN.BUKKEN_NO");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , KW.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_SHUKKA_MEISAI SM");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("  LEFT JOIN T_STOCK STK ON STK.SHUKKA_FLAG = SM.SHUKKA_FLAG");
            sb.ApdL("                         AND STK.NONYUSAKI_CD = SM.NONYUSAKI_CD");
            sb.ApdL("                         AND STK.TAG_NO = SM.TAG_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = SM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            string stockNo = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);
            if (stockNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                sb.ApdN("   AND SM.PALLET_NO = ").ApdN(this.BindPrefix).ApdL("PALLET_NO");
                paramCollection.Add(iNewParam.NewDbParameter("PALLET_NO", stockNo));
            }
            else if (stockNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("   AND SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", stockNo));
            }
            else
            {
                sb.ApdN("   AND SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG2");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG2", stockNo.Substring(0, 1)));
                sb.ApdN("   AND SM.TAG_NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", stockNo.Substring(1, 4)));
                sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", stockNo.Substring(5)));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", language));
            paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID)));
            paramCollection.Add(iNewParam.NewDbParameter("NYUKO_LOCATION", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION)));

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

    #region 完了データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 完了データ取得
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="dr">対象行</param>
    /// <param name="language">言語</param>
    /// <returns>完了データ</returns>
    /// <create>T.Wakamatsu 2013/08/28</create>
    /// <update>H.Tajimi 2015/11/20 備考対応</update>
    /// <update>H.Tajimi 2015/12/09 M-NO対応</update>
    /// <update>D.Okumura 2018/08/31 多言語化対応</update>
    /// --------------------------------------------------
    public DataTable GetKanryo(DatabaseHelper dbHelper, DataRow dr, String language)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID AS WORK_USER_ID");
            sb.ApdL("     , SM.SHUKKA_FLAG + RIGHT('0000' + CONVERT(nvarchar, SM.TAG_NONYUSAKI_CD), 4) + SM.TAG_NO AS TAG_CODE");
            sb.ApdL("     , STK.STATUS");
            sb.ApdL("     , COM1.ITEM_NAME AS STATUS_NAME");
            sb.ApdL("     , STK.LOCATION");
            sb.ApdL("     , STK.STOCK_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , SM.SEIBAN");
            sb.ApdL("     , SM.CODE");
            sb.ApdL("     , SM.ZUMEN_OIBAN");
            sb.ApdL("     , SM.AREA");
            sb.ApdL("     , SM.FLOOR");
            sb.ApdL("     , SM.KISHU");
            sb.ApdL("     , SM.ST_NO");
            sb.ApdL("     , SM.HINMEI_JP");
            sb.ApdL("     , SM.HINMEI");
            sb.ApdL("     , SM.ZUMEN_KEISHIKI");
            sb.ApdL("     , SM.KUWARI_NO");
            sb.ApdL("     , SM.NUM");
            sb.ApdL("     , MN.NONYUSAKI_NAME");
            sb.ApdL("     , KW.SHIP");
            sb.ApdL("     , SM.AR_NO");
            sb.ApdL("     , SM.SHUKA_DATE");
            sb.ApdL("     , SM.BOX_NO");
            sb.ApdL("     , SM.PALLET_NO");
            sb.ApdL("     , SM.KOJI_NO");
            sb.ApdL("     , SM.CASE_ID");
            sb.ApdL("     , KW.SHIP + '-' + CONVERT(nvarchar, KM.CASE_NO) AS KIWAKU_NO");
            sb.ApdL("     , SM.BOXKONPO_DATE");
            sb.ApdL("     , SM.PALLETKONPO_DATE");
            sb.ApdL("     , SM.KIWAKUKONPO_DATE");
            sb.ApdL("     , SM.SHUKKA_DATE");
            sb.ApdL("     , SM.UNSOKAISHA_NAME");
            sb.ApdL("     , SM.INVOICE_NO");
            sb.ApdL("     , SM.OKURIJYO_NO");
            sb.ApdL("     , SM.BL_NO");
            sb.ApdL("     , SM.UKEIRE_DATE");
            sb.ApdL("     , SM.UKEIRE_USER_NAME");
            sb.ApdL("     , STK.VERSION");
            sb.ApdL("     , SM.BIKO");
            sb.ApdL("     , SM.M_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_STOCK STK");
            sb.ApdL("  INNER JOIN M_NONYUSAKI MN ON MN.SHUKKA_FLAG = STK.SHUKKA_FLAG");
            sb.ApdL("                         AND MN.NONYUSAKI_CD = STK.NONYUSAKI_CD");
            sb.ApdL("  INNER JOIN T_SHUKKA_MEISAI SM ON SM.SHUKKA_FLAG = STK.SHUKKA_FLAG");
            sb.ApdL("                         AND SM.NONYUSAKI_CD = STK.NONYUSAKI_CD");
            sb.ApdL("                         AND SM.TAG_NO = STK.TAG_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU KW ON KW.KOJI_NO = SM.KOJI_NO");
            sb.ApdL("  LEFT JOIN T_KIWAKU_MEISAI KM ON KM.CASE_ID = SM.CASE_ID");
            sb.ApdL("  LEFT JOIN M_COMMON COM1 ON COM1.GROUP_CD = 'ZAIKO_STATUS'");
            sb.ApdL("                         AND COM1.VALUE1 = STK.STATUS");
            sb.ApdN("                         AND COM1.LANG = ").ApdN(this.BindPrefix).ApdL("LANG");
            sb.ApdL(" WHERE");
            sb.ApdL("       1 = 1");

            string stockNo = ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO);
            if (stockNo.StartsWith(ZAIKO_TANI.PALLET_VALUE1))
            {
                return new DataTable();
            }
            else if (stockNo.StartsWith(ZAIKO_TANI.BOX_VALUE1))
            {
                sb.ApdN("   AND SM.BOX_NO = ").ApdN(this.BindPrefix).ApdL("BOX_NO");
                paramCollection.Add(iNewParam.NewDbParameter("BOX_NO", stockNo));
            }
            else
            {
                sb.ApdN("   AND SM.SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
                sb.ApdN("   AND SM.TAG_NONYUSAKI_CD = ").ApdN(this.BindPrefix).ApdL("NONYUSAKI_CD");
                sb.ApdN("   AND SM.TAG_NO = ").ApdN(this.BindPrefix).ApdL("TAG_NO");
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", stockNo.Substring(0, 1)));
                paramCollection.Add(iNewParam.NewDbParameter("NONYUSAKI_CD", stockNo.Substring(1, 4)));
                paramCollection.Add(iNewParam.NewDbParameter("TAG_NO", stockNo.Substring(5)));
            }
            sb.ApdL(" ORDER BY");
            sb.ApdL("       SM.TAG_NO");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("LANG", language));
            paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID)));

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

    #region 棚卸削除データ取得

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸削除データ収集
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">一時取込データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public DataTable GetTanaoroshiForDelete(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       BTM.SHUKKA_FLAG");
            sb.ApdL("     , BTM.BUKKEN_NO");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1));

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

    #region 一時取込データの登録

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">一時取込データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsTempwork(DatabaseHelper dbHelper, CondI02 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_BUHIN_TEMPWORK");
            sb.ApdL("(");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , TORIKOMI_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , ERROR_NUM");
            sb.ApdL("     , STATUS_FLAG");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TORIKOMI_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ERROR_NUM");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STATUS_FLAG");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.WORK_USER_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_DATE", cond.TorikomiDate));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.LOCATION, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.STOCK_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ERROR_NUM", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.ERROR_NUM, 0)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK.STATUS_FLAG, 0)));

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
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">一時取込明細データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsTempworkMeisai(DatabaseHelper dbHelper, CondI02 cond, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL("(");
            sb.ApdL("       TEMP_ID");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , TORIKOMI_FLAG");
            sb.ApdL("     , ROW_NO");
            sb.ApdL("     , RESULT");
            sb.ApdL("     , WORK_DATE");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , STOCK_NO");
            sb.ApdL("     , DESCRIPTION");
            sb.ApdL(")");
            sb.ApdL("VALUES");
            sb.ApdL("(");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("ROW_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("WORK_DATE");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("STOCK_NO");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(")");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("WORK_USER_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_USER_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("RESULT", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.RESULT, RESULT.OK_VALUE1)));
                paramCollection.Add(iNewParam.NewDbParameter("WORK_DATE", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.WORK_DATE, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.LOCATION, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("STOCK_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.STOCK_NO, string.Empty)));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.DESCRIPTION, string.Empty)));

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

    #region ロケーション登録

    /// --------------------------------------------------
    /// <summary>
    /// ロケーション登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">一時取込データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/27</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsLocation(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            sb.ApdL("INSERT INTO M_LOCATION");
            sb.ApdL("(");
            sb.ApdL("       SHUKKA_FLAG");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("SELECT DISTINCT");
            sb.ApdL("       BTM.SHUKKA_FLAG");
            sb.ApdL("     , BTM.BUKKEN_NO");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("CREATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("   AND TORIKOMI_FLAG IN (").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG1, ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG2)");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("               SELECT 1");
            sb.ApdL("                 FROM M_LOCATION ML");
            sb.ApdL("                WHERE ML.SHUKKA_FLAG = BTM.SHUKKA_FLAG");
            sb.ApdN("                  AND ML.BUKKEN_NO = BTM.BUKKEN_NO");
            sb.ApdN("                  AND ML.LOCATION = BTM.LOCATION");
            sb.ApdL("              )");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_ID", this.GetCreateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("CREATE_USER_NAME", this.GetCreateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG1", ZAIKO_TORIKOMI_FLAG.LOCATION_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG2", ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1));

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

    #region 棚卸データ登録

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ登録
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">一時取込データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int InsTanaoroshi(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("INSERT INTO T_INVENT");
            sb.ApdL("(");
            sb.ApdL("       INVENT_DATE");
            sb.ApdL("     , TAG_CODE");
            sb.ApdL("     , BUKKEN_NO");
            sb.ApdL("     , LOCATION");
            sb.ApdL("     , KANRYO_FLAG");
            sb.ApdL("     , SHUKKA_FLAG");
            sb.ApdL("     , NONYUSAKI_CD");
            sb.ApdL("     , TAG_NO");
            sb.ApdL("     , WORK_USER_ID");
            sb.ApdL("     , CREATE_DATE");
            sb.ApdL("     , CREATE_USER_ID");
            sb.ApdL("     , CREATE_USER_NAME");
            sb.ApdL("     , UPDATE_DATE");
            sb.ApdL("     , UPDATE_USER_ID");
            sb.ApdL("     , UPDATE_USER_NAME");
            sb.ApdL("     , VERSION");
            sb.ApdL(")");
            sb.ApdL("SELECT DISTINCT");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG + RIGHT('0000' + CAST(SM.TAG_NONYUSAKI_CD AS VARCHAR), 4) + SM.TAG_NO");
            sb.ApdL("     , BTM.BUKKEN_NO");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRYO_FLAG");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , BTM.WORK_USER_ID");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON SM.BOX_NO = BTM.STOCK_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdL("UNION");
            sb.ApdL("SELECT DISTINCT");
            sb.ApdN("       ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdL("     , SM.SHUKKA_FLAG + RIGHT('0000' + CAST(SM.TAG_NONYUSAKI_CD AS VARCHAR), 4) + SM.TAG_NO");
            sb.ApdL("     , BTM.BUKKEN_NO");
            sb.ApdL("     , BTM.LOCATION");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("KANRYO_FLAG");
            sb.ApdL("     , SM.SHUKKA_FLAG");
            sb.ApdL("     , SM.NONYUSAKI_CD");
            sb.ApdL("     , SM.TAG_NO");
            sb.ApdL("     , BTM.WORK_USER_ID");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_ID");
            sb.ApdN("     , ").ApdN(this.BindPrefix).ApdL("UPDATE_USER_NAME");
            sb.ApdN("     , ").ApdL(this.SysTimestamp);
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN T_SHUKKA_MEISAI SM ON");
            sb.ApdL("                          SM.SHUKKA_FLAG = BTM.SHUKKA_FLAG");
            sb.ApdL("                      AND SM.NONYUSAKI_CD = BTM.NONYUSAKI_CD");
            sb.ApdL("                      AND SM.TAG_NO = BTM.TAG_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
            paramCollection.Add(iNewParam.NewDbParameter("KANRYO_FLAG", ZAIKO_KANRYO_FLAG.MIKAN_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_ID", this.GetUpdateUserID(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("UPDATE_USER_NAME", this.GetUpdateUserName(cond)));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ZAIKO_TORIKOMI_FLAG.TANAOROSHI_VALUE1));

            // SQL実行
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

    #region 一時取込データの状態区分のみ更新

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データの状態区分のみ更新
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkStatusFlagOnly(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK");
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

    #region 一時取込データ破棄

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ破棄
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/20</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkStatusKanryo(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK");
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
    /// <param name="dt">更新内容データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkResult(DatabaseHelper dbHelper, DataTable dt)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK");
            sb.ApdL("SET");
            sb.ApdN("       ERROR_NUM = ").ApdN(this.BindPrefix).ApdL("ERROR_NUM");
            sb.ApdN("     , STATUS_FLAG = ").ApdN(this.BindPrefix).ApdL("STATUS_FLAG");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("ERROR_NUM", ComFunc.GetFldToInt32(dr, Def_T_BUHIN_TEMPWORK.ERROR_NUM, 0)));
                paramCollection.Add(iNewParam.NewDbParameter("STATUS_FLAG", ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.STATUS_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TEMP_ID)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFld(dr, Def_T_BUHIN_TEMPWORK.TORIKOMI_FLAG)));

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

    #region 一時取込明細データのインデックス更新

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データのインデックス更新
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/09/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkMeisaiIndex(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // TagNo情報
            // SQL文
            sb.ApdL("UPDATE BTM");
            sb.ApdL("SET");
            sb.ApdL("       BTM.SHUKKA_FLAG = TSM.SHUKKA_FLAG");
            sb.ApdL("     , BTM.NONYUSAKI_CD = TSM.NONYUSAKI_CD");
            sb.ApdL("     , BTM.TAG_NO = TSM.TAG_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       T_SHUKKA_MEISAI TSM");
            sb.ApdL("    ON");
            sb.ApdL("       TSM.SHUKKA_FLAG = SUBSTRING(BTM.STOCK_NO, 1, 1)");
            sb.ApdL("   AND TSM.TAG_NONYUSAKI_CD = SUBSTRING(BTM.STOCK_NO, 2, 4)");
            sb.ApdL("   AND TSM.TAG_NO = SUBSTRING(BTM.STOCK_NO, 6, 5)");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) != 'B' ");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) != 'P' ");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            // BoxNo情報
            sb = new StringBuilder();
            paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("UPDATE BTM");
            sb.ApdL("SET");
            sb.ApdL("       BTM.SHUKKA_FLAG = BM.SHUKKA_FLAG");
            sb.ApdL("     , BTM.NONYUSAKI_CD = BM.NONYUSAKI_CD");
            sb.ApdL("     , BTM.BOX_NO = BM.BOX_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       T_BOXLIST_MANAGE BM");
            sb.ApdL("    ON");
            sb.ApdL("       BM.BOX_NO = BTM.STOCK_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) = 'B' ");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            // PalletNo情報
            sb = new StringBuilder();
            paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("UPDATE BTM");
            sb.ApdL("SET");
            sb.ApdL("       BTM.SHUKKA_FLAG = PM.SHUKKA_FLAG");
            sb.ApdL("     , BTM.NONYUSAKI_CD = PM.NONYUSAKI_CD");
            sb.ApdL("     , BTM.PALLET_NO = PM.PALLET_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       T_PALLETLIST_MANAGE PM");
            sb.ApdL("    ON");
            sb.ApdL("       PM.PALLET_NO = BTM.STOCK_NO");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdL("   AND SUBSTRING(BTM.STOCK_NO, 1, 1) = 'P' ");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));

            // SQL実行
            record += dbHelper.ExecuteNonQuery(sb.ToString(), paramCollection);

            // 物件情報
            sb = new StringBuilder();
            paramCollection = new DbParamCollection();

            // SQL文
            sb.ApdL("UPDATE BTM");
            sb.ApdL("SET");
            sb.ApdL("       BTM.BUKKEN_NO = MN.BUKKEN_NO");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI BTM");
            sb.ApdL(" INNER JOIN");
            sb.ApdL("       M_NONYUSAKI MN");
            sb.ApdL("    ON");
            sb.ApdL("       MN.SHUKKA_FLAG = BTM.SHUKKA_FLAG");
            sb.ApdL("   AND MN.NONYUSAKI_CD = BTM.NONYUSAKI_CD");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.OK_VALUE1));
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
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdInitializeTempworkMeisai(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK_MEISAI");
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
    /// <create>T.Wakamatsu 2013/08/23</create>
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
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("   AND ROW_NO = ").ApdN(this.BindPrefix).ApdL("ROW_NO");

            foreach (DataRow dr in dt.Rows)
            {
                paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.NG_VALUE1));
                paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", message));
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));

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
    /// 一時取込明細データのエラー更新
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="dr"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    public int UpdTempworkMeisaiError(DatabaseHelper dbHelper, DataRow dr, string message)
    {
        try
        {
            int record = 0;
            StringBuilder sb = new StringBuilder();
            DbParamCollection paramCollection = new DbParamCollection();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("UPDATE T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL("SET");
            sb.ApdN("       RESULT = ").ApdN(this.BindPrefix).ApdL("RESULT");
            sb.ApdN("     , DESCRIPTION = ").ApdN(this.BindPrefix).ApdL("DESCRIPTION");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("   AND ROW_NO = ").ApdN(this.BindPrefix).ApdL("ROW_NO");

            paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("RESULT", RESULT.NG_VALUE1));
            paramCollection.Add(iNewParam.NewDbParameter("DESCRIPTION", message));
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, DBNull.Value)));
            paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));

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

    #region 棚卸データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 棚卸データ削除
    /// </summary>
    /// <param name="dbHelper">DatabaseHelper</param>
    /// <param name="cond">I02用コンディション</param>
    /// <param name="dt">削除対象データ</param>
    /// <returns>影響を与えた行数</returns>
    /// <create>T.Wakamatsu 2013/09/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public int DelTanaoroshi(DatabaseHelper dbHelper, CondI02 cond, DataTable dt)
    {
        try
        {
            int ret = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_INVENT");
            sb.ApdL(" WHERE");
            sb.ApdN("       INVENT_DATE = ").ApdN(this.BindPrefix).ApdL("INVENT_DATE");
            sb.ApdN("   AND SHUKKA_FLAG = ").ApdN(this.BindPrefix).ApdL("SHUKKA_FLAG");
            sb.ApdN("   AND BUKKEN_NO = ").ApdN(this.BindPrefix).ApdL("BUKKEN_NO");
            sb.ApdN("   AND LOCATION = ").ApdN(this.BindPrefix).ApdL("LOCATION");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("INVENT_DATE", cond.InventDate));
                paramCollection.Add(iNewParam.NewDbParameter("SHUKKA_FLAG", ComFunc.GetFld(dr, Def_T_INVENT.SHUKKA_FLAG)));
                paramCollection.Add(iNewParam.NewDbParameter("BUKKEN_NO", ComFunc.GetFld(dr, Def_T_INVENT.BUKKEN_NO)));
                paramCollection.Add(iNewParam.NewDbParameter("LOCATION", ComFunc.GetFld(dr, Def_T_INVENT.LOCATION)));

                // SQL実行
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

    #region 一時取込明細データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込明細データ削除
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelTempWorkMeisaiExec(DatabaseHelper dbHelper, CondI02 cond, DataTable dt)
    {
        try
        {
            int ret = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK_MEISAI");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdN("   AND TORIKOMI_FLAG = ").ApdN(this.BindPrefix).ApdL("TORIKOMI_FLAG");
            sb.ApdN("   AND ROW_NO = ").ApdN(this.BindPrefix).ApdL("ROW_NO");

            foreach (DataRow dr in dt.Rows)
            {
                DbParamCollection paramCollection = new DbParamCollection();

                // バインド変数設定
                paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TEMP_ID, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("TORIKOMI_FLAG", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.TORIKOMI_FLAG, DBNull.Value)));
                paramCollection.Add(iNewParam.NewDbParameter("ROW_NO", ComFunc.GetFldObject(dr, Def_T_BUHIN_TEMPWORK_MEISAI.ROW_NO, DBNull.Value)));
                // SQL実行
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

    #region 一時取込データ削除

    /// --------------------------------------------------
    /// <summary>
    /// 一時取込データ削除
    /// </summary>
    /// <param name="dbHelper"></param>
    /// <param name="cond"></param>
    /// <returns></returns>
    /// <create>T.Wakamatsu 2013/08/23</create>
    /// <update></update>
    /// --------------------------------------------------
    private int DelTempWorkExec(DatabaseHelper dbHelper, CondI02 cond)
    {
        try
        {
            int ret = 0;
            StringBuilder sb = new StringBuilder();
            INewDbParameterBasic iNewParam = dbHelper;

            // SQL文
            sb.ApdL("DELETE BTW");
            sb.ApdL("  FROM");
            sb.ApdL("       T_BUHIN_TEMPWORK BTW");
            sb.ApdL(" WHERE");
            sb.ApdN("       TEMP_ID = ").ApdN(this.BindPrefix).ApdL("TEMP_ID");
            sb.ApdL("   AND NOT EXISTS (");
            sb.ApdL("           SELECT 1 FROM T_BUHIN_TEMPWORK_MEISAI BTWM");
            sb.ApdL("            WHERE");
            sb.ApdL("                      BTWM.TEMP_ID = BTW.TEMP_ID");
            sb.ApdL("                  AND BTWM.WORK_USER_ID = BTW.WORK_USER_ID");
            sb.ApdL("                  AND BTWM.TORIKOMI_FLAG = BTW.TORIKOMI_FLAG");
            sb.ApdL("       )");

            DbParamCollection paramCollection = new DbParamCollection();

            // バインド変数設定
            paramCollection.Add(iNewParam.NewDbParameter("TEMP_ID", cond.TempID));
            // SQL実行
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

    #endregion
}
