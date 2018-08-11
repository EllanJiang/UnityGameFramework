//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        public sealed class LogNode : IReference
        {
            private DateTime m_LogTime;
            private LogType m_LogType;
            private string m_LogMessage;
            private string m_StackTrack;

            public LogNode Fill(LogType logType, string logMessage, string stackTrack)
            {
                m_LogTime = DateTime.Now;
                m_LogType = logType;
                m_LogMessage = logMessage;
                m_StackTrack = stackTrack;

                return this;
            }

            public DateTime LogTime
            {
                get
                {
                    return m_LogTime;
                }
            }

            public LogType LogType
            {
                get
                {
                    return m_LogType;
                }
            }

            public string LogMessage
            {
                get
                {
                    return m_LogMessage;
                }
            }

            public string StackTrack
            {
                get
                {
                    return m_StackTrack;
                }
            }

            public void Clear()
            {
                m_LogTime = default(DateTime);
                m_LogType = default(LogType);
                m_LogMessage = default(string);
                m_StackTrack = default(string);
            }
        }
    }
}
