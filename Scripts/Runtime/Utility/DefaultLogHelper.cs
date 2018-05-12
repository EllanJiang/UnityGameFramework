//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 日志辅助器。
    /// </summary>
    public class DefaultLogHelper : Log.ILogHelper
    {
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(string.Format("<color=#888888>{0}</color>", message.ToString()));
                    break;
                case LogLevel.Info:
                    Debug.Log(message.ToString());
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message.ToString());
                    break;
                case LogLevel.Error:
                    Debug.LogError(message.ToString());
                    break;
                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }
    }
}
