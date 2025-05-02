using System;
using System.Media;
using System.Windows.Forms;

namespace SystemBase
{

    #region Enum(SoundType)

    /// --------------------------------------------------
    /// <summary>
    /// サウンドの種類
    /// </summary>
    /// <create>Y.Higuchi 2010/07/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public enum SoundType
    {
        /// --------------------------------------------------
        /// <summary>
        /// なし
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        None = 0,
        /// --------------------------------------------------
        /// <summary>
        /// 情報や問い合せで使用する音
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        Ding = 1,
        /// --------------------------------------------------
        /// <summary>
        /// 警告、エラーで使用する音
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        Chord = 2,
    }

    #endregion

    /// --------------------------------------------------
    /// <summary>
    /// メッセージ音クラス
    /// </summary>
    /// <create>Y.Higuchi 2010/07/09</create>
    /// <update></update>
    /// --------------------------------------------------
    public class MessageSound
    {
        #region Fields

        private SoundPlayer _soundPlayerDing = null;
        private SoundPlayer _soundPlayerChord = null;
        
        #endregion

        #region Delegate

        private delegate void DelegatePlaySound(SoundType type);

        #endregion

        #region Constructors

        /// --------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        private MessageSound()
        {
            this._soundPlayerDing = new SoundPlayer(SystemBase.Properties.Resources.Ding);
            this._soundPlayerDing.LoadAsync();
            this._soundPlayerChord = new SoundPlayer(SystemBase.Properties.Resources.Chord);
            this._soundPlayerChord.LoadAsync();
        }

        /// --------------------------------------------------
        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        ~MessageSound()
        {
            this._soundPlayerDing.Dispose();
            this._soundPlayerChord.Dispose();
        }

        #endregion

        #region インスタンスメソッド

        /// --------------------------------------------------
        /// <summary>
        /// 非同期でファイルを再生します。
        /// </summary>
        /// <param name="type">メッセージ音の種類</param>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public void InnerPlaySound(SoundType type)
        {
            try
            {
                switch (type)
                {
                    case SoundType.None:
                        break;
                    case SoundType.Ding:
                        _soundPlayerDing.Play();
                        break;
                    case SoundType.Chord:
                        _soundPlayerChord.Play();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception) { }
        }

        #endregion

        #region メッセージ音再生

        /// --------------------------------------------------
        /// <summary>
        /// メッセージ音再生
        /// </summary>
        /// <param name="type">メッセージ音の種類</param>
        /// <create>Y.Higuchi 2010/07/09</create>
        /// <update></update>
        /// --------------------------------------------------
        public static void PlaySound(SoundType type)
        {
            MessageSound instance = new MessageSound();
            DelegatePlaySound delegatePlaySound = new DelegatePlaySound(instance.InnerPlaySound);
            // デリゲートで非同期呼び出し
            delegatePlaySound.BeginInvoke(type, null, null);
        }

        #endregion

    }
}
