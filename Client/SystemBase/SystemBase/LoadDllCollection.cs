using System;
using System.Collections.Generic;
using System.Text;

namespace SystemBase
{
    /// --------------------------------------------------
    /// <summary>
    /// メニュー表示のために読み込むDLLのコレクション
    /// </summary>
    /// <create>Y.Higuchi 2010/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public class LoadDllCollection
    {
        #region Fields

        private static LoadDllCollection _instance = null;
        private List<string> _dllList = null;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected LoadDllCollection()
        {
            this._dllList = new List<string>();
        }

        #endregion

        #region Properties

        #region インスタンス

        /// --------------------------------------------------
        /// <summary>
        /// 自インスタンス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        protected static LoadDllCollection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoadDllCollection();
                }
                return _instance;
            }
        }

        #endregion

        #region Items

        /// --------------------------------------------------
        /// <summary>
        /// 指定したインデックス位置にあるDLLパス情報を取得および設定します。
        /// (指定するDLL文字列は*,?を使用したパターンで設定可)
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static List<string> Items
        {
            get { return Instance._dllList; }
            protected set { Instance._dllList = value; }
        }

        #endregion

        #endregion

        #region Add

        /// --------------------------------------------------
        /// <summary>
        /// 末尾に要素を追加します
        /// (指定するDLL文字列は*,?を使用したパターンで設定可)
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Add(string item)
        {
            Items.Add(item);
        }

        #endregion

        #region Count

        /// --------------------------------------------------
        /// <summary>
        /// 要素の数を取得
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static int Count()
        {
            return Items.Count;
        }

        #endregion

        #region Clear

        /// --------------------------------------------------
        /// <summary>
        /// 全ての要素を削除します
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void Clear()
        {
            Items.Clear();
        }

        #endregion

        #region Remove

        /// --------------------------------------------------
        /// <summary>
        /// 最初に見つかった特定の要素を削除します
        /// </summary>
        /// <param name="item"></param>
        /// <returns>正常に削除された場合は true。それ以外の場合は false</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static bool Remove(string item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region RemoveAt

        /// --------------------------------------------------
        /// <summary>
        /// 指定したインデックスにある要素を削除します
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void RemoveAt(int index)
        {
            if (Items.Count <= index) return;
            Items.RemoveAt(index);
        }

        #endregion

        #region ToArray

        /// --------------------------------------------------
        /// <summary>
        /// 要素を新しい配列にコピーします
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public static string[] ToArray()
        {
            return Items.ToArray();
        }

        #endregion
    }
}
