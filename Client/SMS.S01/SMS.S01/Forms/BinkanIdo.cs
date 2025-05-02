using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DSWUtil;
using DSWControl.DSWComboBox;
using WsConnection;
using WsConnection.WebRefCommon;
using Commons;
using SystemBase.Controls;
using SystemBase.Util;

using WsConnection.WebRefS01;
using SMS.P02.Forms;
using SMS.S01.Properties;

namespace SMS.S01.Forms
{
    /// --------------------------------------------------
    /// <summary>
    /// 便間移動
    /// </summary>
    /// <create>T.Wakamatsu 2015/12/25</create>
    /// <update></update>
    /// --------------------------------------------------
    public partial class BinkanIdo : SystemBase.Forms.CustomOrderForm
    {
        #region 定数

        static string PREFIX_KIWAKU { get { return Resources.BinkanIdo_WoodFrame; } }
        static string PREFIX_PALLET { get { return Resources.BinkanIdo_Pallet; } }
        static string PREFIX_BOX { get { return Resources.BinkanIdo_Box; } }
        static string PREFIX_TAG { get { return Resources.BinkanIdo_Tag; } }

        enum TreeSide { Orig, Dest };

        #endregion

        #region Field

        // 現在選択ツリー
        private TreeSide CurrentTreeSide { get; set; }
        // 移動元物件No.
        private string BukkenNoOrig { get; set; }
        // 移動先物件No.
        private string BukkenNoDest { get; set; }
        // 更新条件
        private CondS01 UpdateCond { get; set; }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="menuCategoryID"></param>
        /// <param name="menuItemID"></param>
        /// <param name="title"></param>
        /// <create>T.Wakamatsu 2015/12/25</create>
        /// <update></update>
        /// --------------------------------------------------
        public BinkanIdo(UserInfo userInfo, string menuCategoryID, string menuItemID, string title)
            : base(userInfo, menuCategoryID, menuItemID, title)
        {
            InitializeComponent();

            this.UpdateCond = new CondS01(userInfo);
        }

        #endregion

        #region 初期化

        /// --------------------------------------------------
        /// <summary>
        /// フォームが初めて表示される直前にコントロールの初期化するメソッド
        /// </summary>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeLoadControl()
        {
            base.InitializeLoadControl();

            try
            {
                // フォームの設定
                this.IsCloseQuestion = true;
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
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void InitializeShownControl()
        {
            base.InitializeShownControl();

            try
            {
                // 初期フォーカスの設定
                this.txtNonyusakiCDOrig.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 入力チェック

        /// --------------------------------------------------
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInput()
        {
            bool ret = base.CheckInput();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 検索用入力チェック
        /// </summary>
        /// <returns>true:OK/false:NG</returns>
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool CheckInputSearch()
        {
            bool ret = base.CheckInputSearch();
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                ret = false;
            }
            return ret;
        }

        #endregion

        #region 検索処理

        /// --------------------------------------------------
        /// <summary>
        /// 検索処理制御部
        /// </summary>
        /// <returns>true:検索成功/false:検索失敗</returns>
        /// <create>T.Wakamatsu 2016/01/06</create>
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
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// --------------------------------------------------
        protected override bool RunSearchExec()
        {
            try
            {
                CondS01 cond = this.GetCondition();
                ConnS01 conn = new ConnS01();
                // 変更・削除・照会
                DataSet ds = conn.GetTreeMeisai(cond);
                if (this.CurrentTreeSide == TreeSide.Orig &&
                    !ComFunc.IsExistsData(ds, Def_T_SHUKKA_MEISAI.Name))
                {
                    // 該当の明細は存在しません。
                    this.ShowMessage("A9999999022");
                    return false;
                }

                DisplayTree(ds);

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region 更新処理

        /// --------------------------------------------------
        /// <summary>
        /// 編集内容実行制御処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2016/01/08</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override bool RunEdit()
        {
            return this.RunEditUpdate();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 修正処理
        /// </summary>
        /// <returns>true:正常処理/false:正常以外</returns>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// --------------------------------------------------
        protected override bool RunEditUpdate()
        {
            try
            {
                CondS01 cond = this.UpdateCond;
                ConnS01 conn = new ConnS01();

                if (!conn.UpdMoveShip(cond))
                {
                    return false;
                }

                // 条件クリア
                this.UpdateCond = new CondS01(this.UserInfo);

                this.CurrentTreeSide = TreeSide.Dest;
                this.RunSearch();

                if (this.txtShipOrig.Text == this.txtShipDest.Text)
                {
                    // 同一便の場合は移動元も検索する
                    this.CurrentTreeSide = TreeSide.Orig;
                    this.RunSearch();
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
                return false;
            }
        }

        #endregion

        #region コンディション取得

        /// --------------------------------------------------
        /// <summary>
        /// コンディションの取得
        /// </summary>
        /// <returns>コンディション</returns>
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private CondS01 GetCondition()
        {
            CondS01 cond = new CondS01(this.UserInfo);

            cond.ShukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;

            if (this.CurrentTreeSide == TreeSide.Orig)
            {
                cond.NonyusakiCD = txtNonyusakiCDOrig.Text;
                cond.Ship = txtShipOrig.Text;
            }
            else
            {
                cond.NonyusakiCD = txtNonyusakiCDDest.Text;
                cond.Ship = txtShipDest.Text;
            }
            
            // 更新PC名
            cond.UpdatePCName = UtilSystem.GetUserInfo(false).MachineName;

            return cond;
        }

        #endregion

        #region ファンクションボタンクリック

        /// --------------------------------------------------
        /// <summary>
        /// F07ボタンクリック（Clear）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F06Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F06Button_Click(sender, e);
            try
            {
                // 物件No.クリア
                this.BukkenNoOrig = "";
                this.BukkenNoDest = "";

                // ツリービュークリア
                tvOrig.Nodes.Clear();
                tvOrig.Enabled = false;
                tvDest.Nodes.Clear();
                tvDest.Enabled = false;

                // 検索条件編集可
                grpOrig.Enabled = true;
                grpDest.Enabled = true;

                txtNonyusakiNameOrig.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F07ボタンクリック（All Clear）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F07Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F07Button_Click(sender, e);
            try
            {
                // ツリービュー初期化
                tvOrig.Nodes.Clear();
                tvOrig.Enabled = false;
                tvDest.Nodes.Clear();
                tvDest.Enabled = false;

                // 検索条件編集可
                grpOrig.Enabled = true;
                grpDest.Enabled = true;

                // 検索条件クリア
                txtNonyusakiNameOrig.Text = "";
                txtShipOrig.Text = "";
                txtNonyusakiCDOrig.Text = "";
                txtNonyusakiNameDest.Text = "";
                txtShipDest.Text = "";
                txtNonyusakiCDDest.Text = "";

                txtNonyusakiNameOrig.Focus();
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// F12ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>Y.Higuchi 2010/07/06</create>
        /// <update></update>
        /// --------------------------------------------------
        protected override void fbrFunction_F12Button_Click(object sender, EventArgs e)
        {
            base.fbrFunction_F12Button_Click(sender, e);
            try
            {
            }
            catch (Exception ex)
            {
                CustomMsgBoxEx.ShowError(this, ComDefine.MSG_ERROR_TEXT, ex);
            }
        }

        #endregion

        #region 開始ボタン

        /// --------------------------------------------------
        /// <summary>
        /// 移動元開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStartOrig_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            CurrentTreeSide = TreeSide.Orig;
            if (!this.ShowShip())
            {
                this.txtNonyusakiNameOrig.Focus();
                return;
            }

            // 検索条件編集不可
            grpOrig.Enabled = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 移動先開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnStartDest_Click(object sender, EventArgs e)
        {
            this.ClearMessage();
            CurrentTreeSide = TreeSide.Dest;
            if (!this.ShowShip())
            {
                this.txtNonyusakiNameDest.Focus();
                return;
            }

            // 検索条件編集不可
            grpDest.Enabled = false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 便データ表示
        /// </summary>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool ShowShip()
        {
            // 出荷フラグは「本体」
            string shukkaFlag = SHUKKA_FLAG.NORMAL_VALUE1;
            DSWControl.DSWTextBox.DSWTextBox txtNonyusakiName;
            DSWControl.DSWTextBox.DSWTextBox txtShip;
            DSWControl.DSWTextBox.DSWTextBox txtNonyusakiCD;

            if (this.CurrentTreeSide == TreeSide.Orig)
            {
                txtNonyusakiName = this.txtNonyusakiNameOrig;
                txtShip = this.txtShipOrig;
                txtNonyusakiCD = this.txtNonyusakiCDOrig;
            }
            else
            {
                txtNonyusakiName = this.txtNonyusakiNameDest;
                txtShip = this.txtShipDest;
                txtNonyusakiCD = this.txtNonyusakiCDDest;
            }
            string nonyusakiName = txtNonyusakiName.Text;
            string ship = txtShip.Text;

            using (NonyusakiIchiran frm = new NonyusakiIchiran(this.UserInfo, shukkaFlag, nonyusakiName, ship, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataRow dr = frm.SelectedRowData;
                    if (dr == null) return false;
                    
                    if (this.CurrentTreeSide == TreeSide.Orig)
                        this.BukkenNoOrig = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO);
                    else
                        this.BukkenNoDest = ComFunc.GetFld(dr, Def_M_NONYUSAKI.BUKKEN_NO);

                    if (!EqualsBukkenNo())
                    {
                        // 物件No.が異なります。
                        this.ShowMessage("S0100030001");
                        return false;
                    }
                    // 選択データを設定
                    txtNonyusakiCD.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_CD);
                    txtNonyusakiName.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.NONYUSAKI_NAME);
                    txtShip.Text = ComFunc.GetFld(dr, Def_M_NONYUSAKI.SHIP);

                    return this.RunSearch();
                }
            }

            return false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 物件No.が等しいかどうか
        /// </summary>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private bool EqualsBukkenNo()
        {
            if (string.IsNullOrEmpty(this.BukkenNoOrig) || string.IsNullOrEmpty(this.BukkenNoDest))
                return true;
            else if (this.BukkenNoOrig == this.BukkenNoDest)
                return true;
            else
                return false;
        }

        #endregion

        #region ツリー表示

        /// --------------------------------------------------
        /// <summary>
        /// ツリー表示
        /// </summary>
        /// <param name="ds"></param>
        /// <create>T.Wakamatsu 2016/01/06</create>
        /// <update></update>
        /// --------------------------------------------------
        private void DisplayTree(DataSet ds)
        {
            DataTable dt = ds.Tables[Def_T_SHUKKA_MEISAI.Name];
            TreeNode root;

            // ドラッグを受け入れる
            if (this.CurrentTreeSide == TreeSide.Orig)
            {
                tvOrig.Enabled = true;
                tvOrig.Nodes.Clear();
            }
            else
            {
                tvDest.Enabled = true;
                tvDest.Nodes.Clear();
            }

            // 初期ノードを追加する
            if (this.CurrentTreeSide == TreeSide.Orig)
            {
                root = tvOrig.Nodes.Add(txtNonyusakiNameOrig.Text);
            }
            else
            {
                root = tvDest.Nodes.Add(txtNonyusakiNameDest.Text);
            }
            TreeNode ct = root;
            TreeNode pt = root;
            TreeNode bt = root;

            string ship = string.Empty;
            string cno = string.Empty;
            string pno = string.Empty;
            string bno = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                string cship = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                string ccase = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.CASE_NO);
                string cpal = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.PALLET_NO);
                string cbox = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.BOX_NO);
                string ctag = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.TAG_NO);
                string cprcs = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.PRINT_CASE_NO);
                string hinmei = ComFunc.GetFld(dr, Def_T_SHUKKA_MEISAI.HINMEI_JP);

                // 便またはC/NOが変わる場合
                if (ship != cship || cno != ccase)
                {
                    string kojiNo = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                    string caseId = ComFunc.GetFld(dr, Def_T_KIWAKU_MEISAI.CASE_ID);
                    ct = root.Nodes.Add(kojiNo + "|" + caseId + "|" + ccase + "|" + cprcs, PREFIX_KIWAKU + cship + "-" + ccase);
                }

                // パレットNo.が変わる場合
                if (!string.IsNullOrEmpty(cpal) && pno != cpal)
                    pt = ct.Nodes.Add(PREFIX_PALLET + cpal);

                // ボックスNo.が変わる場合
                if (!string.IsNullOrEmpty(cbox) && bno != cbox)
                    bt = pt.Nodes.Add(PREFIX_BOX + cbox);

                if (!string.IsNullOrEmpty(cbox))
                    // ボックスNo.がある場合
                    bt.Nodes.Add(PREFIX_TAG + ctag + " " + hinmei);
                else if (!string.IsNullOrEmpty(ccase))
                    // 便またはC/NOがある場合
                    ct.Nodes.Add(PREFIX_TAG + ctag + " " + hinmei);
                else
                    // どこにもぶら下がっていない場合
                    root.Nodes.Add(PREFIX_TAG + ctag + " " + hinmei);
                ship = cship;
                cno = ccase;
                pno = cpal;
                bno = cbox;
            }
        }

        #endregion

        #region ドラッグ＆ドロップ処理

        /// --------------------------------------------------
        /// <summary>
        /// 移動元ノードドラッグ開始処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tvOrig_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            tv.SelectedNode = (TreeNode)e.Item;
            tv.Focus();

            if (IsDraggable(tv.SelectedNode))
            {
                //ノードのドラッグを開始する
                DragDropEffects dde =
                    tv.DoDragDrop(e.Item, DragDropEffects.All);
                //移動した時は、ドラッグしたノードを削除する
                if ((dde & DragDropEffects.Move) == DragDropEffects.Move)
                {
                    // 親ノード削除処理
                    RemoveParent((TreeNode)e.Item, true);
                }
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 移動先ドロップ処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tvDest_DragDrop(object sender, DragEventArgs e)
        {
            //ドロップされたデータがTreeNodeか調べる
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeView tv = (TreeView)sender;
                //ドロップされたデータ(TreeNode)を取得
                TreeNode source =
                    (TreeNode)e.Data.GetData(typeof(TreeNode));
                //ドロップ先のTreeNodeを取得する
                TreeNode target =
                    tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                //マウス下のNodeがドロップ先として適切か調べる
                if (target != null && target != source &&
                    IsDroppable(source, target))
                {
                    // 木枠の場合
                    if (source.Text.StartsWith(PREFIX_KIWAKU))
                    {
                        string[] s = source.Name.Split('|');
                        string kojiNoOrig = s[0];
                        string caseIdOrig = s[1];
                        string caseNoOrig = s[2];
                        string printCaseNoOrig = s[3];
                        CondS01 cond = new CondS01(this.UserInfo);
                        cond.ShukkaFlag = Commons.SHUKKA_FLAG.NORMAL_VALUE1;
                        cond.NonyusakiCD = this.txtNonyusakiCDDest.Text;

                        ConnS01 conn = new ConnS01();
                        DataSet ds = conn.GetKiwaku(cond);

                        string kojiNoDest = "";
                        string kojiNameDest = "";
                        string shipDest = "";
                        if (ds.Tables[Def_T_KIWAKU.Name].Rows.Count == 0)
                        {
                            using (KojiShikibetsuIchiran frm = new KojiShikibetsuIchiran(this.UserInfo, true))
                            {
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    DataRow dr = frm.SelectedRowData;
                                    if (dr == null) return;
                                    // 選択データを設定
                                    kojiNoDest = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NO);
                                    kojiNameDest = ComFunc.GetFld(dr, Def_T_KIWAKU.KOJI_NAME);
                                    shipDest = ComFunc.GetFld(dr, Def_T_KIWAKU.SHIP);
                                }
                                else
                                    return;
                            }
                        }
                        else
                        {
                            kojiNoDest = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.KOJI_NO);
                            kojiNameDest = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.KOJI_NAME);
                            shipDest = ComFunc.GetFld(ds, Def_T_KIWAKU.Name, 0, Def_T_KIWAKU.SHIP);
                        }
                        using (KiwakuCaseInput frm = new KiwakuCaseInput(this.UserInfo, kojiNoDest, kojiNameDest, shipDest, caseNoOrig, printCaseNoOrig))
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                // CASE IDは重複がないので特に変更しない
                                this.UpdateCond.KojiNoOrig = kojiNoOrig;
                                this.UpdateCond.KojiNoDest = frm.KojiNo;
                                this.UpdateCond.CaseId = caseIdOrig;
                                this.UpdateCond.CaseNo = frm.CaseNo;
                                this.UpdateCond.PrintCaseNo = frm.PrintCaseNo;
                            }
                            else
                            {
                                e.Effect = DragDropEffects.None;
                                return;
                            }
                        }

                        //ドロップされたNodeのコピーを作成
                        TreeNode cln = (TreeNode)source.Clone();
                        //Nodeを追加
                        target.Nodes.Add(cln);
                        //ドロップ先のNodeを展開
                        target.Expand();
                        //追加されたNodeを選択
                        tv.SelectedNode = cln;
                        // 移動処理
                        MoveNode(tv.SelectedNode);
                    }
                    if (this.ShowMessage("S0100030002", source.Text, target.Text).Equals(DialogResult.OK))
                    {
                        //ドロップされたNodeのコピーを作成
                        TreeNode cln = (TreeNode)source.Clone();
                        //Nodeを追加
                        target.Nodes.Add(cln);
                        //ドロップ先のNodeを展開
                        target.Expand();
                        //追加されたNodeを選択
                        tv.SelectedNode = cln;
                        // 移動処理
                        MoveNode(tv.SelectedNode);
                    }
                    else
                        e.Effect = DragDropEffects.None;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 移動先ドラッグオーバー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tvDest_DragOver(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがTreeNodeか調べる
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                if ((e.KeyState & 8) == 8 &&
                    (e.AllowedEffect & DragDropEffects.Copy) ==
                    DragDropEffects.Copy)
                    //Ctrlキーが押されていればCopy
                    //"8"はCtrlキーを表す
                    e.Effect = DragDropEffects.Copy;
                else if ((e.AllowedEffect & DragDropEffects.Move) ==
                    DragDropEffects.Move)
                    //何も押されていなければMove
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                //TreeNodeでなければ受け入れない
                e.Effect = DragDropEffects.None;

            //マウス下のNodeを選択する
            if (e.Effect != DragDropEffects.None)
            {
                TreeView tv = (TreeView)sender;
                //マウスのあるNodeを取得する
                TreeNode target =
                    tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                //ドラッグされているNodeを取得する
                TreeNode source =
                    (TreeNode)e.Data.GetData(typeof(TreeNode));
                //マウス下のNodeがドロップ先として適切か調べる
                if (target != null && target != source &&
                        IsDroppable(source, target))
                {
                    //Nodeを選択する
                    if (target.IsSelected == false)
                        tv.SelectedNode = target;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 移動元ドラッグオーバー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private void tvOrig_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ドラッグ可能かチェックする
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool IsDraggable(TreeNode target)
        {
            // 移動できるのはルート以外
            if (target.Parent == null)
                return false;
            else
                return true;
        }

        /// --------------------------------------------------
        /// <summary>
        /// ドロップ可能かチェックする
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private static bool IsDroppable(TreeNode childNode, TreeNode parentNode)
        {
            // 移動先がルートの場合は無条件でOK
            if (parentNode.Parent == null)
                return true;
            // 移動先が同じ場合はNG
            if ((childNode.Parent.Text == parentNode.Text) ||
                (childNode.Parent.Text.StartsWith(PREFIX_KIWAKU) &&
                parentNode.Text.StartsWith(PREFIX_KIWAKU) &&
                childNode.Parent.Name == parentNode.Name))
                return false;
            if (parentNode.Text.StartsWith(PREFIX_KIWAKU) && childNode.Text.StartsWith(PREFIX_PALLET))
            {
                // 木枠明細でパレットはMAX10件までOK
                if (CountPallets(parentNode) < 10)
                    return true;
                else
                    return false;
            }
            else if (parentNode.Text.StartsWith(PREFIX_KIWAKU) &&
                childNode.Text.StartsWith(PREFIX_TAG))
                // 木枠明細の下にタグ移動はOK
                return true;
            else if (parentNode.Text.StartsWith(PREFIX_PALLET) && childNode.Text.StartsWith(PREFIX_BOX))
                // パレットの下にボックス移動はOK
                return true;
            else if (parentNode.Text.StartsWith(PREFIX_BOX) && childNode.Text.StartsWith(PREFIX_TAG))
                // ボックスの下にタグ移動はOK
                return true;
            else
                return false;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 木枠の下のパレットの数を取得
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        /// <create>T.Wakamatsu 2016/01/05</create>
        /// <update></update>
        /// --------------------------------------------------
        private static int CountPallets(TreeNode parentNode)
        {
            int count = 0;
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Text.Contains(PREFIX_PALLET))
                    count++;
            }
            return count;
        }

        #endregion

        #region 親ノード削除処理

        /// --------------------------------------------------
        /// <summary>
        /// 親ノード削除処理
        /// </summary>
        /// <param name="tn"></param>
        /// <create>T.Sakiori 2017/09/20</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RemoveParent(TreeNode tn)
        {
            RemoveParent(tn, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 親ノード削除処理
        /// </summary>
        /// <param name="tn"></param>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update></update>
        /// --------------------------------------------------
        private void RemoveParent(TreeNode tn, bool first)
        {
            // ツリービューおよび親ノードセット
            TreeView tv = tn.TreeView;
            TreeNode parent = tn.Parent;
            
            // 対象ノード削除
            tn.Remove();

            // 木枠明細移動でない場合は更新条件セット
            if (!first || !tn.Text.StartsWith(PREFIX_KIWAKU))
            {
                // 親ノードがルートの場合
                if (parent.Parent == null)
                {
                    // 処理無し
                }
                else if (parent.Text.StartsWith(PREFIX_KIWAKU))
                {
                    // 親ノードが木枠に梱包されている場合、パレット削除の可能性あり
                    string[] koji = parent.Name.Split('|');
                    this.UpdateCond.KojiNoOrig = koji[0];
                    this.UpdateCond.CaseIdOrig = koji[1];

                    if (parent.Nodes.Count == 0)
                        // 親ノードが対象ノード以外の子ノードを持たない場合削除
                        parent.Remove();
                }
                // 親ノードが対象ノード以外の子ノードを持たない場合
                else if (parent.Nodes.Count == 0)
                {
                    // 親ノードデータ削除
                    if (parent.Text.StartsWith(PREFIX_PALLET))
                        this.UpdateCond.RemovePalletNo = parent.Text.Replace(PREFIX_PALLET, "");
                    else if (parent.Text.StartsWith(PREFIX_BOX))
                        this.UpdateCond.RemoveBoxNo = parent.Text.Replace(PREFIX_BOX, "");
                    // 必要なら親ノード削除
                    RemoveParent(parent);
                }
                else
                {
                    parent = parent.Parent;
                    while (parent != null)
                    {

                        // 上位の木枠明細があればセット
                        if (parent.Text.StartsWith(PREFIX_KIWAKU))
                        {
                            // 木枠に梱包されている場合、セット
                            string[] koji = parent.Name.Split('|');
                            this.UpdateCond.KojiNoOrig = koji[0];
                            this.UpdateCond.CaseIdOrig = koji[1];
                            break;
                        }
                        parent = parent.Parent;
                    }
                }
            }
            // 更新は一度だけ
            if (string.IsNullOrEmpty(this.UpdateCond.NonyusakiCDOrig)) return;
            // 更新処理実行
            this.EditMode = SystemBase.EditMode.Update;
            this.RunEdit();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 移動処理（）
        /// </summary>
        /// <param name="tn"></param>
        /// <create>T.Wakamatsu 2016/01/07</create>
        /// <update>J.Chen 2022/12/19 TAG便名追加</update>
        /// --------------------------------------------------
        private void MoveNode(TreeNode tn)
        {
            // ツリービューおよび親ノードセット
            TreeView tv = tn.TreeView;

            this.UpdateCond.NonyusakiCDOrig = txtNonyusakiCDOrig.Text;
            this.UpdateCond.NonyusakiCDDest = txtNonyusakiCDDest.Text;

            // 移動元便名
            this.UpdateCond.Ship = txtShipOrig.Text;

            if (tn.Text.StartsWith(PREFIX_TAG))
            {
                // タグ移動時
                this.UpdateCond.TagNo = tn.Text.Replace(PREFIX_TAG, "").Split(' ')[0];
                this.UpdateCond.UpdateTani = BINKAN_IDO_TANI.TAG_VALUE1;
            }
            else if (tn.Text.StartsWith(PREFIX_BOX))
            {
                // ボックス移動時
                this.UpdateCond.BoxNo = tn.Text.Replace(PREFIX_BOX, "");
                this.UpdateCond.UpdateTani = BINKAN_IDO_TANI.BOX_VALUE1;
            }
            else if (tn.Text.StartsWith(PREFIX_PALLET))
            {
                // パレット移動時
                this.UpdateCond.PalletNo = tn.Text.Replace(PREFIX_PALLET, "");
                this.UpdateCond.UpdateTani = BINKAN_IDO_TANI.PALLET_VALUE1;
            }
            else if (tn.Text.StartsWith(PREFIX_KIWAKU))
            {
                // 木枠移動時
                // 必要な情報は前段階（tvDest_DragDrop）でセット
                this.UpdateCond.UpdateTani = BINKAN_IDO_TANI.KIWAKU_VALUE1;
            }

            string[] ts = tn.FullPath.Split('\\');
            for (int i = 1; i < ts.Length - 1; i++)
            {
                // BoxNo, PalletNoのprefix削除
                if (ts[i].StartsWith(PREFIX_BOX))
                    this.UpdateCond.BoxNo = ts[i].Replace(PREFIX_BOX, "");
                else if (ts[i].StartsWith(PREFIX_PALLET))
                    this.UpdateCond.PalletNo = ts[i].Replace(PREFIX_PALLET, "");
            }

            if (tn.FullPath.Contains(PREFIX_KIWAKU))
            {
                // 木枠取得
                TreeNode parent = tn.Parent;
                while (!parent.Text.StartsWith(PREFIX_KIWAKU))
                    parent = parent.Parent;

                // 木枠に梱包される場合
                string[] koji = parent.Name.Split('|');
                this.UpdateCond.KojiNoDest = koji[0];
                this.UpdateCond.CaseIdDest = koji[1];
            }
        }

        #endregion
    }
}
