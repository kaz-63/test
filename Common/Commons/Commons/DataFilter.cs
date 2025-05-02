using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DSWUtil;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Commons
{
    /// --------------------------------------------------
    /// <summary>
    /// データ絞込み用クラス
    /// </summary>
    /// <create>T.Sakiori 2012/09/21</create>
    /// <update></update>
    /// --------------------------------------------------
    public class DataFilter
    {
        #region フィールド

        /// --------------------------------------------------
        /// <summary>
        /// 絞込み条件を格納
        /// </summary>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private Dictionary<string, string> _dict = null;

        #endregion

        #region 列挙型

        /// --------------------------------------------------
        /// <summary>
        /// 曖昧検索の仕方
        /// </summary>
        /// <create>K.Tsutsumi 2013/03/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public enum LikeSearchMatchPattern
        {
            /// --------------------------------------------------
            /// <summary>
            /// 前方一致
            /// </summary>
            /// <create>H.Tajimi 2019/08/29</create>
            /// <update></update>
            /// --------------------------------------------------
            LeftHandMatch = 0,
            /// --------------------------------------------------
            /// <summary>
            /// 後方一致
            /// </summary>
            /// <create>H.Tajimi 2019/08/29</create>
            /// <update></update>
            /// --------------------------------------------------
            RightHandMatch = 1,
            /// --------------------------------------------------
            /// <summary>
            /// 部分一致
            /// </summary>
            /// <create>H.Tajimi 2019/08/29</create>
            /// <update></update>
            /// --------------------------------------------------
            PartialMatch = 2
        }

        #endregion

        #region コンストラクタ

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>T.Sakiori 2013/01/28</create>
        /// <update></update>
        /// --------------------------------------------------
        public DataFilter()
        {
            this._dict = new Dictionary<string, string>();
        }

        #endregion

        #region ClearFilter

        /// --------------------------------------------------
        /// <summary>
        /// 内部で保持する絞り込み条件をクリアします
        /// </summary>
        /// <create>T.Sakiori 2012/11/07</create>
        /// <update></update>
        /// --------------------------------------------------
        public void ClearFilter()
        {
            this._dict.Clear();
        }

        #endregion

        #region SetFilterFromIndex

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromIndex(object dataSource, string dataMember, string column, ComboBox ctrl)
        {
            this.SetFilterFromIndex(dataSource, dataMember, column, ctrl, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromIndex(object dataSource, string dataMember, string column, ComboBox ctrl, bool likeSearch)
        {
            if (dataSource == null)
            {
                return;
            }
            if (dataSource is DataSet)
            {
                this.SetFilterFromIndex((dataSource as DataSet).Tables[dataMember], column, ctrl, likeSearch);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromIndex(object dataSource, string column, ComboBox ctrl)
        {
            this.SetFilterFromIndex(dataSource, column, ctrl, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromIndex(object dataSource, string column, ComboBox ctrl, bool likeSearch)
        {
            this.SetFilterFromIndex(dataSource, column, ctrl, likeSearch, LikeSearchMatchPattern.PartialMatch);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>T.Sakiori 2013/03/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromIndex(object dataSource, string column, ComboBox ctrl, LikeSearchMatchPattern matchPattern)
        {
            this.SetFilterFromIndex(dataSource, column, ctrl, true, matchPattern);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>T.Sakiori 2013/03/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFilterFromIndex(object dataSource, string column, ComboBox ctrl, bool likeSearch, LikeSearchMatchPattern matchPattern)
        {
            // データソースが空だったら終了な
            if (dataSource == null)
            {
                return;
            }
            string value = string.Empty;
            if (ctrl.DataSource is DataTable)
            {
                value = ComFunc.GetFld((ctrl.DataSource as DataTable), ctrl.SelectedIndex, ctrl.DisplayMember);
            }
            else if (ctrl.DataSource is List<string>)
            {
                value = (ctrl.DataSource as List<string>)[ctrl.SelectedIndex];
            }
            this.SetFilterExec(dataSource as DataTable, column, value, likeSearch, matchPattern, false);
        }

        #endregion

        #region SetFilterFromText

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string dataMember, string column, Control ctrl)
        {
            this.SetFilterFromText(dataSource, dataMember, column, ctrl, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string dataMember, string column, Control ctrl, bool likeSearch)
        {
            this.SetFilterFromText((dataSource as DataSet).Tables[dataMember], column, ctrl, likeSearch, LikeSearchMatchPattern.PartialMatch);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string dataMember, string column, Control ctrl, LikeSearchMatchPattern matchPattern)
        {
            if (dataSource == null)
            {
                return;
            }
            if (dataSource is DataSet)
            {
                this.SetFilterFromText((dataSource as DataSet).Tables[dataMember], column, ctrl, true, matchPattern);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string column, Control ctrl)
        {
            this.SetFilterFromText(dataSource, column, ctrl, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update>K.Tsutsumi 2013/03/11 LikeSearchAlignment対応</update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string column, Control ctrl, bool likeSearch)
        {
            this.SetFilterExec(dataSource as DataTable, column, ctrl.Text, likeSearch, LikeSearchMatchPattern.PartialMatch, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む内容が記述 or 選択されているコントロールのvalue</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>J.Chen 2023/10/03</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromValue(object dataSource, string column, string value, bool likeSearch)
        {
            this.SetFilterExec(dataSource as DataTable, column, value, likeSearch, LikeSearchMatchPattern.PartialMatch, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="values">絞り込む内容のリスト</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.SASAYAMA 2023/07/24</create>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string column, List<string> values, bool likeSearch)
        {
            this.SetFilterExec(dataSource as DataTable, column, values, likeSearch, LikeSearchMatchPattern.PartialMatch, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>K.Tsutsumi 2013/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromText(object dataSource, string column, Control ctrl, LikeSearchMatchPattern matchPattern)
        {
            this.SetFilterExec(dataSource as DataTable, column, ctrl.Text, true, matchPattern, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="ctrl">絞り込む内容が記述 or 選択されているコントロール</param>
        /// <param name="likeSearch">曖昧検索を行う場合は True</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>K.Tsutsumi 2013/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFilterFromText(object dataSource, string column, Control ctrl, bool likeSearch, LikeSearchMatchPattern matchPattern)
        {
            // データソースが空だったら終了な
            if (dataSource == null)
            {
                return;
            }
            this.SetFilterExec(dataSource as DataTable, column, ctrl.Text, likeSearch, matchPattern, false);
        }

        #endregion

        #region SetFilterFromConstant

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <create>T.Sakiori 2012/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromConstant(object dataSource, string dataMember, string column, string value)
        {
            this.SetFilterFromConstant(dataSource, dataMember, column, value, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataSet)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="dataMember">データソース内のテーブル名</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromConstant(object dataSource, string dataMember, string column, string value, bool likeSearch)
        {
            if (dataSource == null)
            {
                return;
            }
            if (dataSource is DataSet)
            {
                this.SetFilterFromConstant((dataSource as DataSet).Tables[dataMember], column, value, likeSearch);
            }
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <create>T.Sakiori 2012/11/06</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromConstant(object dataSource, string column, string value)
        {
            this.SetFilterFromConstant(dataSource, column, value, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <create>T.Sakiori 2012/11/06</create>
        /// <update>K.Tsutsumi 2013/03/11 LikeSearchMatchPattern対応</update>
        /// --------------------------------------------------
        public void SetFilterFromConstant(object dataSource, string column, string value, bool likeSearch)
        {
            this.SetFilterExec(dataSource as DataTable, column, value, likeSearch, LikeSearchMatchPattern.PartialMatch, false);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <param name="matchPattern">曖昧検索パターン</param>
        /// <create>K.Tsutsumi 2013/03/11</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromConstant(object dataSource, string column, string value, LikeSearchMatchPattern matchPattern)
        {
            // データソースが空だったら終了な
            if (dataSource == null)
            {
                return;
            }
            this.SetFilterExec(dataSource as DataTable, column, value, true, matchPattern, false);
        }

        #endregion

        #region SetFilterFromFree

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む値</param>
        /// <create>T.Sakiori 2017/02/21</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromFree(object dataSource, string column, string value)
        {
            this.SetFilterExec(dataSource as DataTable, column, value, false, LikeSearchMatchPattern.LeftHandMatch, true);
        }

        #endregion

        #region SetFilterFromBetween

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む(データソースがDataTable)
        /// </summary>
        /// <param name="dataSource">絞り込まれるデータソース</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="valueFrom">絞込範囲開始</param>
        /// <param name="valueTo">絞込範囲終了</param>
        /// <create>T.Sakiori 2017/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public void SetFilterFromBetween(object dataSource, string column, string valueFrom, string valueTo)
        {
            this.SetFilterBetweenExec(dataSource as DataTable, column, valueFrom, valueTo);
        }

        #endregion

        #region SetFilterExec

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む
        /// </summary>
        /// <param name="dt">絞り込まれるデータテーブル</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="value">絞り込む内容</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <param name="matchPattern"></param>
        /// <param name="isFree">IN句を使用するかどうか</param>
        /// <create>T.Sakiori 2012/09/21</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFilterExec(DataTable dt, string column, string value, bool likeSearch, LikeSearchMatchPattern matchPattern, bool isFree)
        {
            // データソースが空だったら終了な
            if (dt == null)
            {
                return;
            }

            if (isFree)
            {
                // 条件追加 or 変更
                if (!this._dict.ContainsKey(column))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        this._dict.Add(column, value);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this._dict.Remove(column);
                    }
                    else
                    {
                        this._dict[column] = value;
                    }
                }
            }
            else
            {
                // エスケープ文字を置換
                value = Regex.Replace(value, @"(\~|\(|\)|\#|\\|\/|\=|\>|\<|\+|\-|\*|\%|\&|\'|\||\^|""|\[|\])", "[$1]");

                // 条件追加 or 変更
                if (!this._dict.ContainsKey(column))
                {
                    if (value != ComDefine.COMBO_ALL_DISP && value != ComDefine.COMBO_FIRST_VALUE)
                    {
                        if (likeSearch)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (matchPattern == LikeSearchMatchPattern.LeftHandMatch)
                                {
                                    this._dict.Add(column, " LIKE " + UtilConvert.PutQuot(value + "%"));
                                }
                                else if (matchPattern == LikeSearchMatchPattern.RightHandMatch)
                                {
                                    this._dict.Add(column, " LIKE " + UtilConvert.PutQuot("%" + value));
                                }
                                else
                                {
                                    this._dict.Add(column, " LIKE " + UtilConvert.PutQuot("%" + value + "%"));
                                }
                            }
                            else
                            {
                                this._dict.Add(column, " IS NULL ");
                            }
                        }
                        else
                        {
                            this._dict.Add(column, " = " + UtilConvert.PutQuot(value));
                        }
                    }
                }
                else
                {
                    if (value == ComDefine.COMBO_ALL_DISP || value == ComDefine.COMBO_FIRST_VALUE)
                    {
                        this._dict.Remove(column);
                    }
                    else
                    {
                        if (likeSearch)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (matchPattern == LikeSearchMatchPattern.LeftHandMatch)
                                {
                                    this._dict[column] = " LIKE " + UtilConvert.PutQuot(value + "%");
                                }
                                else if (matchPattern == LikeSearchMatchPattern.RightHandMatch)
                                {
                                    this._dict[column] = " LIKE " + UtilConvert.PutQuot("%" + value);
                                }
                                else
                                {
                                    this._dict[column] = " LIKE " + UtilConvert.PutQuot("%" + value + "%");
                                }
                            }
                            else
                            {
                                this._dict[column] = " IS NULL ";
                            }
                        }
                        else
                        {
                            this._dict[column] = " = " + UtilConvert.PutQuot(value);
                        }
                    }
                }
            }

            // フィルタの作成(全ての時は無視！)
            var rowFilter = new StringBuilder();
            foreach (var item in this._dict)
            {
                if (rowFilter.Length != 0)
                {
                    rowFilter.ApdN(" AND ");
                }
                rowFilter.ApdN(item.Key + item.Value);
            }
            // 作成したフィルタを設定
            string filter = rowFilter.ToString();
            dt.DefaultView.RowFilter = filter;
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む
        /// </summary>
        /// <param name="dt">絞り込まれるデータテーブル</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="valueFrom">絞込範囲開始</param>
        /// <param name="valueTo">絞込範囲終了</param>
        /// <create>T.Sakiori 2017/03/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private void SetFilterBetweenExec(DataTable dt, string column, string valueFrom, string valueTo)
        {
            // データソースが空だったら終了な
            if (dt == null)
            {
                return;
            }

            this._dict[column] = string.Format(" >= '{0}' AND '{1}' >= {2}", valueFrom, valueTo, column);

            // フィルタの作成(全ての時は無視！)
            var rowFilter = new StringBuilder();
            foreach (var item in this._dict)
            {
                if (rowFilter.Length != 0)
                {
                    rowFilter.ApdN(" AND ");
                }
                rowFilter.ApdN(item.Key + item.Value);
            }
            // 作成したフィルタを設定
            string filter = rowFilter.ToString();
            dt.DefaultView.RowFilter = filter;
        }

        #endregion

        #region SetFilterExec(複数検索)

        /// --------------------------------------------------
        /// <summary>
        /// 指定したデータソースを絞り込む
        /// </summary>
        /// <param name="dt">絞り込まれるデータテーブル</param>
        /// <param name="column">絞り込みの対象列名</param>
        /// <param name="values">絞り込む内容</param>
        /// <param name="likeSearch">部分一致検索を行う場合は True</param>
        /// <param name="matchPattern"></param>
        /// <param name="isFree">IN句を使用するかどうか</param>
        /// <create>T.SASAYAMA 2023/07/24</create>
        /// <update></update>
        /// --------------------------------------------------
        /// 

        private void SetFilterExec(DataTable dt, string column, List<string> values, bool likeSearch, LikeSearchMatchPattern matchPattern, bool isFree)
        {
            // データソースが空だったら終了な
            if (dt == null)
            {
                return;
            }

            var rowFilter = new StringBuilder();
            foreach (var value in values)
            {
                // 条件追加 or 変更
                string newValue = "";
                if (!string.IsNullOrEmpty(value))
                {
                    // エスケープ文字を置換
                    newValue = Regex.Replace(value, @"(\~|\(|\)|\#|\\|\/|\=|\>|\<|\+|\-|\*|\%|\&|\'|\||\^|""|\[|\])", "[$1]");

                    if (likeSearch)
                    {
                        if (matchPattern == LikeSearchMatchPattern.LeftHandMatch)
                        {
                            newValue = column + " LIKE " + UtilConvert.PutQuot(newValue + "%");
                        }
                        else if (matchPattern == LikeSearchMatchPattern.RightHandMatch)
                        {
                            newValue = column + " LIKE " + UtilConvert.PutQuot("%" + newValue);
                        }
                        else
                        {
                            newValue = column + " LIKE " + UtilConvert.PutQuot("%" + newValue + "%");
                        }
                    }
                    else
                    {
                        newValue = column + " = " + UtilConvert.PutQuot(newValue);
                    }
                }
                else
                {
                    newValue = column + " IS NULL OR " + column + " = ''";
                }

                if (rowFilter.Length != 0)
                {
                    rowFilter.Append(" OR ");
                }
                rowFilter.Append(newValue);
            }

            // フィルタの作成(全ての時は無視！)
            // 作成したフィルタを設定
            string filter = rowFilter.ToString();
            dt.DefaultView.RowFilter = filter;
        }



        #endregion
    }
}
