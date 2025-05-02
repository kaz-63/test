using System;
using System.Data;
using System.Drawing;

using Commons;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;

using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefT01;
using SMS.T01.Properties;

namespace SMS.T01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 手配明細更新履歴画面
    /// </summary>
    /// <create>J.Chen 2024/11/06</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class TehaiMeisaiRireki : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// ECS 期
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsQuota;
        /// --------------------------------------------------
        /// <summary>
        /// ECSNo
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ecsNo;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private const int SHEET_COL_TOPLEFT_COL = 0;

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public TehaiMeisaiRireki(UserInfo userInfo, string ecsQuota, string ecsNo)
            : base(userInfo)
        {
            InitializeComponent();

            // 画面タイトル設定
            this.Title = ComDefine.TITLE_T0100100;
            
            //変数初期化
            this._ecsQuota = ecsQuota;
            this._ecsNo = ecsNo;

        }
        
        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisaiRireki);

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示されるたびにコントロールの初期化するメソッド
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                // 初期フォーカスの設定
                this.shtMeisaiRireki.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region シート初期化

        /// --------------------------------------------------
        /// <summary>
        /// シート初期化
        /// </summary>
        /// <param name="sheet">ElTabelleSheet</param>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            try
            {
                base.InitializeSheet(sheet);
                int col = 0;
                sheet.ColumnHeaders[col++].Caption = Resources.TehaiMeisaiRireki_UpdateDate;
                sheet.ColumnHeaders[col++].Caption = Resources.TehaiMeisaiRireki_UpdateUser;
                sheet.ColumnHeaders[col++].Caption = Resources.TehaiMeisaiRireki_Naiyo;

            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 画面クリア

        /// --------------------------------------------------
        /// <summary>
        /// 画面クリア
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.SheetClear();
                this.shtMeisaiRireki.Focus();

                //履歴検索
                this.RunSearch();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region Sheetのクリア

        /// --------------------------------------------------
        /// <summary>
        /// Sheetのクリア
        /// </summary>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            try
            {
                this.shtMeisaiRireki.Redraw = false;
                if (0 < this.shtMeisaiRireki.MaxRows)
                {
                    this.shtMeisaiRireki.TopLeft = new Position(0, this.shtMeisaiRireki.TopLeft.Row);  
                }
                this.shtMeisaiRireki.DataSource = null;
                this.shtMeisaiRireki.MaxRows = 0;
            }
            finally           
            {
                this.shtMeisaiRireki.Redraw = true;
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearch()
        {
            return base.RunSearch();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理実行部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>J.Chen 2024/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnT01 conn = new ConnT01();
                CondT01 cond = new CondT01(this.UserInfo);

                cond.EcsQuota = this._ecsQuota;
                cond.EcsNo = this._ecsNo;

                //手配明細履歴取得
                DataSet ds = conn.GetTehaiMeisaiRireki(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_TEHAI_MEISAI_RIREKI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    return false;
                }
                
                //シートに履歴データを入れる
                this.shtMeisaiRireki.DataSource = ds;
                this.shtMeisaiRireki.DataMember = Def_M_TEHAI_MEISAI_RIREKI.Name;

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisaiRireki.MaxRows)
                {
                    this.shtMeisaiRireki.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisaiRireki.TopLeft.Row);
                }
                this.shtMeisaiRireki.Focus();

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

    }
}
