using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.I02
{
    /// --------------------------------------------------
    /// <summary>
    /// ハンディのファイル情報
    /// </summary>
    /// <create>Y.Higuchi 2010/08/12</create>
    /// <update></update>
    /// --------------------------------------------------
    public class HandyFileInfo : ICloneable
    {
        #region Fields

        /// --------------------------------------------------
        /// <summary>
        /// クレードルタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _cradleType = null;
        /// --------------------------------------------------
        /// <summary>
        /// ハンディのポート番号
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private int _portNo = 0;
        /// --------------------------------------------------
        /// <summary>
        /// ハンディのドライブ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _drive = null;
        /// --------------------------------------------------
        /// <summary>
        /// 拡張子無しファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _fileNameWithoutExt = null;
        /// --------------------------------------------------
        /// <summary>
        /// 拡張子
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        private string _ext = null;

        #endregion

        #region Properties

        #region クレードルタイプ

        /// --------------------------------------------------
        /// <summary>
        /// クレードルタイプ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string CradleType
        {
            get { return this._cradleType; }
            set { this._cradleType = value; }
        }

        #endregion

        #region ハンディのポート番号

        /// --------------------------------------------------
        /// <summary>
        /// ハンディのポート番号
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public int PortNo
        {
            get { return this._portNo; }
            set { this._portNo = value; }
        }

        #endregion

        #region ハンディのドライブ

        /// --------------------------------------------------
        /// <summary>
        /// ハンディのドライブ
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Drive
        {
            get { return this._drive; }
            set { this._drive = value; }
        }

        #endregion

        #region 拡張子無しファイル名

        /// --------------------------------------------------
        /// <summary>
        /// 拡張子無しファイル名
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string FileNameWithoutExt
        {
            get { return this._fileNameWithoutExt; }
            set { this._fileNameWithoutExt = value; }
        }

        #endregion

        #region 拡張子

        /// --------------------------------------------------
        /// <summary>
        /// 拡張子
        /// </summary>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string Ext
        {
            get { return this._ext; }
            set { this._ext = value; }
        }

        #endregion

        #endregion
        
        #region ハンディのファイルのフルパス取得

        /// --------------------------------------------------
        /// <summary>
        /// ハンディのファイルのフルパス取得
        /// </summary>
        /// <returns>ハンディのファイルのフルパス</returns>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetHandyFileName()
        {
            return this.Drive + this.FileNameWithoutExt + "." + this.Ext;
        }

        #endregion

        #region ローカル用ファイル名取得(ポート番号+"_"+ファイル名+"."+拡張子)

        /// --------------------------------------------------
        /// <summary>
        /// ローカル用ファイル名取得(ポート番号+"_"+ファイル名+"."+拡張子)
        /// </summary>
        /// <returns>ローカル用ファイル名</returns>
        /// <create>Y.Higuchi 2010/08/12</create>
        /// <update></update>
        /// --------------------------------------------------
        public string GetLocalFileName()
        {
            return this.PortNo.ToString() + "_" + this.FileNameWithoutExt + "." + this.Ext;
        }

        #endregion

        #region Clone

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        public HandyFileInfo Clone()
        {
            return (HandyFileInfo)this.MemberwiseClone();
        }

        #region ICloneable メンバ

        /// --------------------------------------------------
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        /// <create>Y.Higuchi 2010/08/18</create>
        /// <update></update>
        /// --------------------------------------------------
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #endregion

    }
}
