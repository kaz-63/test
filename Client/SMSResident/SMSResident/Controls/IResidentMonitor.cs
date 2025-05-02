namespace SMSResident.Controls
{
    /// --------------------------------------------------
    /// <summary>
    /// 常駐用インターフェイス
    /// </summary>
    /// <create>Y.Higuchi 2010/06/30</create>
    /// <update></update>
    /// --------------------------------------------------
    public interface IResidentMonitor
    {
        /// --------------------------------------------------
        /// <summary>
        /// スレッドの実行状態
        /// </summary>
        /// <create>Y.Higuchi 2010/06/30</create>
        /// <update></update>
        /// --------------------------------------------------
        bool ResidentState { get; }

        /// --------------------------------------------------
        /// <summary>
        /// メッセージのクリア
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        void MessageClear();

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理開始
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        void MonitorStart();

        /// --------------------------------------------------
        /// <summary>
        /// 常駐処理終了
        /// </summary>
        /// <create>Y.Higuchi 2010/07/01</create>
        /// <update></update>
        /// --------------------------------------------------
        void MonitorStop();
    }
}
