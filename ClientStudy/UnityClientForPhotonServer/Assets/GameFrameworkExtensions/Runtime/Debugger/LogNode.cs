using System;
using UnityEngine;

namespace GameFramework
{
    public static partial class Log
    {
        private sealed class LogNode
        {
            private readonly DateTime m_LogTime;
            private readonly LogType m_LogType;
            private readonly string m_LogMessage;
            private readonly string m_StackTrack;

            public LogNode(LogType logType, string logMessage, string stackTrack)
            {
                m_LogTime = DateTime.Now;
                m_LogType = logType;
                m_LogMessage = logMessage;
                m_StackTrack = stackTrack;
            }

            public DateTime LogTime
            {
                get { return m_LogTime; }
            }

            public LogType LogType
            {
                get { return m_LogType; }
            }

            public string LogMessage
            {
                get { return m_LogMessage; }
            }

            public string StackTrack
            {
                get { return m_StackTrack; }
            }
        }
    }
}
