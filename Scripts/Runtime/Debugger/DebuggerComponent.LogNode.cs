//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 日志记录结点。
        /// </summary>
        public sealed class LogNode : IReference
        {
            private DateTime m_LogTime;
            private int m_LogFrameCount;
            private LogType m_LogType;
            private string m_LogMessage;
            private string m_StackTrack;

            /// <summary>
            /// 初始化日志记录结点的新实例。
            /// </summary>
            public LogNode()
            {
                m_LogTime = default(DateTime);
                m_LogFrameCount = 0;
                m_LogType = LogType.Error;
                m_LogMessage = null;
                m_StackTrack = null;
            }

            /// <summary>
            /// 获取日志时间。
            /// </summary>
            public DateTime LogTime
            {
                get
                {
                    return m_LogTime;
                }
            }

            /// <summary>
            /// 获取日志帧计数。
            /// </summary>
            public int LogFrameCount
            {
                get
                {
                    return m_LogFrameCount;
                }
            }

            /// <summary>
            /// 获取日志类型。
            /// </summary>
            public LogType LogType
            {
                get
                {
                    return m_LogType;
                }
            }

            /// <summary>
            /// 获取日志内容。
            /// </summary>
            public string LogMessage
            {
                get
                {
                    return m_LogMessage;
                }
            }

            /// <summary>
            /// 获取日志堆栈信息。
            /// </summary>
            public string StackTrack
            {
                get
                {
                    return m_StackTrack;
                }
            }

            /// <summary>
            /// 创建日志记录结点。
            /// </summary>
            /// <param name="logType">日志类型。</param>
            /// <param name="logMessage">日志内容。</param>
            /// <param name="stackTrack">日志堆栈信息。</param>
            /// <returns>创建的日志记录结点。</returns>
            public static LogNode Create(LogType logType, string logMessage, string stackTrack)
            {
                LogNode logNode = ReferencePool.Acquire<LogNode>();
                logNode.m_LogTime = DateTime.Now;
                logNode.m_LogFrameCount = Time.frameCount;
                logNode.m_LogType = logType;
                logNode.m_LogMessage = logMessage;
                logNode.m_StackTrack = stackTrack;
                return logNode;
            }

            /// <summary>
            /// 清理日志记录结点。
            /// </summary>
            public void Clear()
            {
                m_LogTime = default(DateTime);
                m_LogFrameCount = 0;
                m_LogType = LogType.Error;
                m_LogMessage = null;
                m_StackTrack = null;
            }
        }
    }
}
