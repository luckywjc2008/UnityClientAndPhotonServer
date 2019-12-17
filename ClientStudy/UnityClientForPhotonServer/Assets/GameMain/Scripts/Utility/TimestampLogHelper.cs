/*
 * Author:      NOW
 * CreateTime:  2017.10.17
 * Description:
 * 
*/
using GameFramework;
using System;
using System.Text;
using UnityEngine;


namespace StarForce
{
    /// <summary>
    /// 带时间戳的日志辅助器
    /// </summary>
    public class TimestampLogHelper :Log.ILogHelper
    {
        private StringBuilder m_LogBuilder;
        private string resultMessageStr;

        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(LogLevel level, object message)
        {
            m_LogBuilder = new StringBuilder();
            m_LogBuilder.Append("[");
            m_LogBuilder.Append(DateTime.Now.ToString("HH:mm:ss.fff"));
            m_LogBuilder.Append("] ");
            m_LogBuilder.Append(message.ToString());
            resultMessageStr = m_LogBuilder.ToString();

            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(string.Format("<color=#888888>{0}</color>", resultMessageStr));
                    break;
                case LogLevel.Info:
                    Debug.Log(resultMessageStr);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(resultMessageStr);
                    break;
                case LogLevel.Error:
                    Debug.LogError(resultMessageStr);
                    break;
                default:
                    throw new GameFrameworkException(resultMessageStr);
            }
        }
    }
}