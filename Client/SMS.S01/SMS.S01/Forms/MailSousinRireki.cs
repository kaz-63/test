using System;
using System.Data;
using System.Drawing;

using Commons;
using SystemBase.Util;
using GrapeCity.Win.ElTabelle;

using WsConnection;
using WsConnection.WebRefCommon;
using WsConnection.WebRefS01;
using SMS.S01.Properties;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// Mail送信履歴画面
    /// </summary>
    /// <create>Y.Gwon 2023/08/17</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class MailSousinRireki : SystemBase.Forms.CustomOrderForm
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 物件管理No
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _bukkenNo;
        /// --------------------------------------------------
        /// <summary>
        /// 荷受CD
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _consignCD;
        /// --------------------------------------------------
        /// <summary>
        /// 運賃梱包製番
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _shipSeiban;

        #endregion

        #region 定数

        /// --------------------------------------------------
        /// <summary>
        /// 最も左上に表示されているセルの列インデックス
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
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
        /// <param name="menuCategoryID">メニュー種別ID</param>
        /// <param name="menuItemID">メニュー項目ID</param>
        /// <param name="title">画面タイトル</param>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        //public MailSousinRireki(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
        //    : base(userInfo, menuCategoryID, menuItemID, title)
        //{
        //    InitializeComponent();
        //}

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo">ログインユーザー情報</param>
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        public MailSousinRireki(UserInfo userInfo, string bukkenNo, string consignCD, string shipSeiban)
            : base(userInfo)
        {
            InitializeComponent();

            // 画面タイトル設定
            this.Title = ComDefine.TITLE_S0100060;
			
			//変数初期化
            this._bukkenNo = bukkenNo;
            this._consignCD = consignCD;
            this._shipSeiban = shipSeiban;

        }
        
        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Gwon 2023/08/17</create>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();
            
            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;

                // シートの初期化
                this.InitializeSheet(this.shtMeisai);

                //アサイン送信時のコメントのデータにあわせて行の高さ自動調整
                shtMeisai.AutofitContent = AutofitContent.All;
                shtMeisai.AutofitRowHeight();
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
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();
            try
            {
                // 初期フォーカスの設定
                this.shtMeisai.Focus();
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
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeSheet(Sheet sheet)
        {
            base.InitializeSheet(sheet);

            sheet.KeepHighlighted = true;
            sheet.SelectionType = SelectionType.Single;
            sheet.ViewMode = ViewMode.Row;

            try
            {
                // グリッド線
                sheet.GridLine = new GridLine(GridLineStyle.Thin, Color.DarkGray);
                // Disable時の設定
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    sheet.Columns[i].DisabledBackColor = Color.FromArgb(223, 223, 223);
                    sheet.Columns[i].DisabledForeColor = Color.Black;
                }
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
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void DisplayClear()
        {
            base.DisplayClear();
            try
            {
                this.SheetClear();
                this.shtMeisai.Focus();

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
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SheetClear()
        {
            // グリッドクリア
            try
            {
                this.shtMeisai.Redraw = false;
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(0, this.shtMeisai.TopLeft.Row);  
                }
                this.shtMeisai.DataSource = null;
                this.shtMeisai.MaxRows = 0;
            }
            finally           
            {
                this.shtMeisai.Redraw = true;
            }
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>Y.Gwon 2023/08/17</create>
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
        /// <create>Y.Gwon 2023/08/17</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                ConnS01 conn = new ConnS01();
                CondS01 cond = new CondS01(this.UserInfo);

                //履歴照会に使う持ってきたデータをセーブする変数
                cond.BukkenNO = this._bukkenNo;
                cond.ConsignCD = this._consignCD;
                cond.ShipSeiban = this._shipSeiban;

                //送信履歴取得
                DataSet ds = conn.GetMailSousinRireki(cond);

                if (!ComFunc.IsExistsData(ds, Def_M_MAIL_SEND_RIREKI.Name))
                {
                    // シートのクリア
                    this.SheetClear();
                    return false;
                }
				
				//シートに履歴データを入れる
                this.shtMeisai.DataSource = ds;
                this.shtMeisai.DataMember = Def_M_MAIL_SEND_RIREKI.Name;

                // 最も左上に表示されているセルの設定
                if (0 < this.shtMeisai.MaxRows)
                {
                    this.shtMeisai.TopLeft = new Position(SHEET_COL_TOPLEFT_COL, this.shtMeisai.TopLeft.Row);
                }
                this.shtMeisai.Focus();

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
