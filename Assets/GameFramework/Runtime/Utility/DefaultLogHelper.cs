
using GameFramework;
using UnityEngine;


namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认游戏框架日志辅助器。
    /// </summary>
    public class DefaultLogHelper : GameFrameworkLog.ILogHelper
    {
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    Debug.Log(Utility.Text.Format("<color=#000000>{0}</color>", message.ToString()));
                    break;

                case GameFrameworkLogLevel.Info:
                    Debug.Log(Utility.Text.Format("<color=#0000ff>{0}</color>", message.ToString()));
                    break;

                case GameFrameworkLogLevel.Warning:
                    Debug.Log(Utility.Text.Format("<color=#ff6600>{0}</color>", message.ToString()));
                    break;

                case GameFrameworkLogLevel.Error:
                    Debug.Log(Utility.Text.Format("<color=#ff0000>{0}</color>", message.ToString()));
                    break;

                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }
    }
}
