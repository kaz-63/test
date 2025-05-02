using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SystemBase
{
    /// --------------------------------------------------
    /// <summary>
    /// Typeロードクラス
    /// </summary>
    /// <create>Y.Higuchi 2010/04/26</create>
    /// <update></update>
    /// --------------------------------------------------
    public class TypeLoader
    {
        #region Fields

        private string _searchDirPath;
        private SearchOption _searchOption;

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public TypeLoader()
            : this(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, SearchOption.TopDirectoryOnly)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchDirPath">検索フォルダパス</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public TypeLoader(string searchDirPath)
            : this(searchDirPath, SearchOption.TopDirectoryOnly)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchOption">フォルダ検索オプション</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public TypeLoader(SearchOption searchOption)
            : this(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, searchOption)
        {
        }

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchDirPath">検索フォルダパス</param>
        /// <param name="searchOption">フォルダ検索オプション</param>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public TypeLoader(string searchDirPath, SearchOption searchOption)
        {
            this.SearchDirPath = searchDirPath;
            this.SearchOption = searchOption;
        }

        #endregion

        #region Properties

        #region 検索フォルダパス

        /// --------------------------------------------------
        /// <summary>
        /// 検索フォルダパス
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public string SearchDirPath
        {
            get { return this._searchDirPath; }
            set { this._searchDirPath = value; }
        }

        #endregion

        #region ディレクトリ検索オプション

        /// --------------------------------------------------
        /// <summary>
        /// ディレクトリ検索オプション
        /// </summary>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public SearchOption SearchOption
        {
            get { return this._searchOption; }
            set { this._searchOption = value; }
        }

        #endregion

        #endregion

        #region GetClassType

        /// --------------------------------------------------
        /// <summary>
        /// 指定クラスのTypeを取得
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <returns>Type(クラスがない場合はnull)</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public Type GetClassType(string className)
        {
            return this.GetClassType(className, string.Empty);
        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定クラスのTypeを取得
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="dllSearchPattern">検索パターン</param>
        /// <returns>Type(クラスがない場合はnull)</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public Type GetClassType(string className, string dllSearchPattern)
        {
            try
            {
                string pattern = dllSearchPattern;
                if (!string.IsNullOrEmpty(pattern) && string.IsNullOrEmpty(Path.GetExtension(pattern)))
                {
                    pattern += ".dll";
                }
                Type retType = null;
                retType = Type.GetType(className);
                // Typeが取得できたか判定
                if (retType != null)
                {
                    // Typeが取得できればそのまま処理を抜ける
                    return retType;
                }

                // Typeを取得できない場合、AppDomain内のアセンブルにクラスが無いかのでDLLを検索
                foreach (string fileName in Directory.GetFiles(this.SearchDirPath, dllSearchPattern, this.SearchOption))
                {
                    // .NET 以外のDllを読み込んだ場合Exceptionが発生するのでtry～catchしておく
                    try
                    {
                        // Dllをロードし、ロードしたアセンブリにTypeがあるか検索
                        Assembly loadAsm = Assembly.LoadFile(fileName);
                        retType = loadAsm.GetType(className, false, false);
                        // Typeが取得できたか判定
                        if (retType != null)
                        {
                            return retType;
                        }
                    }
                    catch (Exception){}
                }
                // 全てのアセンブリより検索
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    retType = asm.GetType(className, false, false);
                    if (retType != null)
                    {
                        break;
                    }
                }

                return retType;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// --------------------------------------------------
        /// <summary>
        /// 指定クラスのTypeを取得
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="dllSearchPatterns">検索パターン配列</param>
        /// <returns>Type(クラスがない場合はnull)</returns>
        /// <create>Y.Higuchi 2010/04/26</create>
        /// <update></update>
        /// --------------------------------------------------
        public Type GetClassType(string className, string[] dllSearchPatterns)
        {
            try
            {
                if (dllSearchPatterns == null || dllSearchPatterns.Length < 1)
                {
                    return this.GetClassType(className, string.Empty);
                }
                else
                {
                    foreach (string dllSearchPattern in dllSearchPatterns)
                    {
                        Type retType = this.GetClassType(className, dllSearchPattern);
                        if (retType != null)
                        {
                            return retType;
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion
    }
}
