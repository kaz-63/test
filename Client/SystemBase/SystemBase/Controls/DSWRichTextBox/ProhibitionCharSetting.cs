using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DSWControl.DSWRichTextBox
{
    /// --------------------------------------------------
    /// <summary>
    /// 禁止文字を設定するクラス
    /// </summary>
    /// <create>T.Sakiori 2008/12/19</create>
    /// <update></update>
    /// --------------------------------------------------
    internal partial class ProhibitionCharSetting : Form
    {
        /// --------------------------------------------------
        /// <summary>
        /// ProhibitionCharフィールド
        /// </summary>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private Dictionary<Int32, String> _ProhibitionChar;

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public ProhibitionCharSetting()
        {
            InitializeComponent();

            InitializeControl();
        }

        /// --------------------------------------------------
        /// <summary>
        /// コントロールの初期化
        /// </summary>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void InitializeControl()
        {
            // コンボボックスに表示するためのリストを作成
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Display", typeof(string));
            dt.Columns.Add("Value", typeof(int));
            dt.Rows.Add("直接指定", 0);
            dt.Rows.Add("正規表現", 1);

            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            column.DataPropertyName = "dt";
            column.DataSource = dt;
            column.DisplayMember = "Display";
            column.ValueMember = "Value";
            column.HeaderText = "入力種別";
            grd.Columns.Remove("Column2");
            grd.Columns.Add(column);
        }

        /// --------------------------------------------------
        /// <summary>
        /// OKボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnOK_Click(object sender, EventArgs e)
        {
            Dictionary<Int32, String> dict = new Dictionary<Int32, String>();
            for (int i = 0; i < grd.RowCount; i++)
            {
                if (grd.Rows[i].Cells[0].Value != null && grd.Rows[i].Cells[1].Value != null)
                {
                    if (Convert.ToInt32(grd.Rows[i].Cells[1].Value) == 0)
                    {
                        dict.Add(i, grd.Rows[i].Cells[0].Value.ToString());
                    }
                    else
                    {
                        dict.Add((i + 1) * -1, grd.Rows[i].Cells[0].Value.ToString());
                    }
                }
            }

            _ProhibitionChar = dict;

            this.Close();
        }

        /// --------------------------------------------------
        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// --------------------------------------------------
        /// <summary>
        /// 禁止文字を保持するプロパティ
        /// </summary>
        /// <create>T.Sakiori 2008/12/19</create>
        /// <update></update>
        /// --------------------------------------------------
        public Dictionary<Int32, String> ProhibitionChar
        {
            get { return _ProhibitionChar; }
            set
            { 
                _ProhibitionChar = value;
                if (_ProhibitionChar != null)
                {
                    foreach (KeyValuePair<int, string> kvp in _ProhibitionChar)
                    {
                        if (kvp.Key >= 0)
                        {
                            grd.Rows.Add(kvp.Value, 0);
                        }
                        else
                        {
                            grd.Rows.Add(kvp.Value, 1);
                        }
                    }
                }
            }
        }
    }
}