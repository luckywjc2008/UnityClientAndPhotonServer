using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameFramework
{
    public static partial class Log
    {
        //捕获日志列表
        private static LinkedList<LogNode> m_Logs = new LinkedList<LogNode>();
        //缓存最大数量，然后写入文件
        private static int m_MaxCountToWriteFile = 50;
        //日志时间格式
        private static string m_DateTimeFormat = "[HH:mm:ss.fff] ";
        //日志文件夹名称
        private static string m_SaveFilePath = "Debugger";
        //日志扩展名称
        private static string m_ExpandName = "txt";
        //日志删除最晚时间
        private static int m_DeleteFileInterval = 2 * 24 * 3600;

        public static int DeleteFileInterval
        {
            get
            {
                return m_DeleteFileInterval;
            }
            set
            {
                //删除间隔最短时间为1天,因为记录时间是以天为单位记录
                if (value >= 1*24*3600)
                {
                    m_DeleteFileInterval = value;
                }
            }
        }


        public static void Initialize(params object[] args)
        {
            Application.logMessageReceived += OnLogMessageReceived;
            DeletePastDueLog();
        }

        public static void Shutdown()
        {
            Application.logMessageReceived -= OnLogMessageReceived;

            SaveLogToFile();
            Clear();
        }

        private static void Clear()
        {
            m_Logs.Clear();
        }

        private static void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
        {
            if (!s_LogRecord)
            {
                return;
            }

            if (logType == LogType.Assert)
            {
                logType = LogType.Error;
            }

            m_Logs.AddLast(new LogNode(logType, logMessage, stackTrace));
            while (m_Logs.Count >= m_MaxCountToWriteFile)
            {
                SaveLogToFile();
            }
        }

        private static string GetLogString(LogNode logNode)
        {
            string logString = null;
            switch (logNode.LogType)
            {
                case LogType.Log:
                case LogType.Warning:
                    logString = string.Format("{0} {1} {2}", logNode.LogType.ToString(), logNode.LogTime.ToString(m_DateTimeFormat), logNode.LogMessage);
                    break;
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                default:
                    logString = string.Format("{0} {1} {2} {3}", logNode.LogType.ToString(),logNode.LogTime.ToString(m_DateTimeFormat),logNode.LogMessage,logNode.StackTrack);
                    break;
            }

            return logString;

        }

        private static string GetLogDirectoryPath()
        {
            return Utility.Path.GetCombinePath(UnityUtility.Path.PersistentDataPath,
                UnityGameFramework.Runtime.Constant.DevicePlatform.GetCurDevicePlatformPath(), m_SaveFilePath);
        }

        private static string GetLogSavePath()
        {
            DateTime curDay = DateTime.Today;
            string fileName = String.Format("{0}_{1}_{2}.{3}",curDay.Year,curDay.Month,curDay.Day, m_ExpandName);
            string fileFullPath = Utility.Path.GetCombinePath(GetLogDirectoryPath(), fileName);
            return fileFullPath;
        }

        private static void SaveLogToFile()
        {
            if (!s_LogRecord)
            {
                return;
            }

            string fileFullPath = GetLogSavePath();

            StringBuilder SBString = new StringBuilder();
            for (int i = 0; i < m_MaxCountToWriteFile && m_Logs.Count > 0; i++)
            {
                string logString = GetLogString(m_Logs.First.Value);
                SBString.Append(logString);
                SBString.Append("\r\n\r\n");
                m_Logs.RemoveFirst();
            }

            if (!File.Exists(fileFullPath))
            {
                string startText = "Log Start...\r\n";
                FileTool.CreatFilePath(fileFullPath);
                File.WriteAllText(fileFullPath, startText,Encoding.UTF8);
            }

            File.AppendAllText(fileFullPath, SBString.ToString(),Encoding.UTF8);
        }

        private static void DeletePastDueLog()
        {
            string DirectoryPath = GetLogDirectoryPath();

            if (!Directory.Exists(DirectoryPath))
            {
                return;
            }

            string[] files = Directory.GetFiles(DirectoryPath);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string fileExpandName = Path.GetExtension(files[i]);

                string[] dataTimeSplit = fileName.Split('_');
                if (dataTimeSplit.Length >= 3 && fileExpandName == m_ExpandName)
                {
                    DateTime fileDate = new DateTime(int.Parse(dataTimeSplit[0]), int.Parse(dataTimeSplit[1]), int.Parse(dataTimeSplit[2]));
                    DateTime curDate = DateTime.Today;

                    TimeSpan intervalTime = curDate - fileDate;
                    if (intervalTime.Ticks >= DeleteFileInterval )
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }
    }

}