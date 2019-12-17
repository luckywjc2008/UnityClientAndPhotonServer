using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.Collections;

namespace StarForce
{
    public class ConfigComponentCustom : GameFrameworkComponent
    {
        [SerializeField]
        private QualityLevelType m_DefaultQualityLevel = QualityLevelType.Fastest;

        [SerializeField]
        private bool m_PlaySplashVideo = false;

        [SerializeField]
        private DeviceModelConfig m_DeviceModelConfigCustom = null;

        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        private BuildInfo m_BuildInfo = null;

        /// <summary>
        /// 游戏全局默认品质
        /// </summary>
        public QualityLevelType DefaultQualityLevel
        {
            get
            {
                return m_DefaultQualityLevel;
            }
        }

        /// <summary>
        /// 播放闪屏视频
        /// </summary>
        public bool PlaySplashVedio
        {
            get
            {
                return m_PlaySplashVideo;
            }
        }

        /// <summary>
        /// 移动设备画质配置信息
        /// </summary>
        public DeviceModelConfig DeviceModelConfigCustom
        {
            get
            {
                return m_DeviceModelConfigCustom;
            }
        }

        /// <summary>
        /// 安装包打包信息
        /// </summary>
        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        /// <summary>
        /// 全局服务器信息
        /// </summary>
        public GlobalInfo GlobalInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏版本信息
        /// </summary>
        public GameVersionInfo GameVersionInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 资源版本信息
        /// </summary>
        public ResourceVersionInfo ResourceVersionInfo
        {
            get;
            set;
        }

        public IEnumerator InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                yield return null;
            }

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                yield return null;
            }

            GameEntry.Base.GameVersion = GameEntry.ConfigCustom.BuildInfo.GameVersion;
            GameEntry.Base.InternalApplicationVersion = GameEntry.ConfigCustom.BuildInfo.InternalGameVersion;
#elif UNITY_STANDALONE_WIN 
            yield return StartCoroutine(GetExternalBuildInfo());
#endif
        }

        /// <summary>
        /// 获取外部打包信息
        /// </summary>
        /// <returns></returns>
        IEnumerator GetExternalBuildInfo()
        {
            //程序目录程序版本文件路
            string externalBuildInfoFile = "file://" + Application.streamingAssetsPath + "/" + UnityGameFramework.Runtime.Constant.DevicePlatform.GetCurDevicePlatformPath() + "/" + "BuildInfo.txt";
            WWW buildInfoWWW = new WWW(externalBuildInfoFile);
            yield return buildInfoWWW;

            if (string.IsNullOrEmpty(buildInfoWWW.error))
            {
                if (buildInfoWWW.isDone)
                {
                    if (buildInfoWWW.text != "" && buildInfoWWW.bytesDownloaded >= 10)
                    {
                        m_BuildInfo = Utility.Json.ToObject<BuildInfo>(buildInfoWWW.text);
                        if (m_BuildInfo == null)
                        {
                            Log.Warning("Parse build info failure.");
                            yield return null;
                        }

                        GameEntry.Base.GameVersion = GameEntry.ConfigCustom.BuildInfo.GameVersion;
                        GameEntry.Base.InternalApplicationVersion = GameEntry.ConfigCustom.BuildInfo.InternalGameVersion;
                    }
                }
            }
            else
            {
                Log.Info("Load extelnal buildInfo error: " + buildInfoWWW.error);
            }

            buildInfoWWW = null;
        }


        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!GameEntry.Localization.ParseDictionary(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }
    }
}
