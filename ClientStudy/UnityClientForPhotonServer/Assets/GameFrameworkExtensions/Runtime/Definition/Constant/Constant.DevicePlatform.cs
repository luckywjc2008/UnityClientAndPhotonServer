using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public static partial class Constant
    {
        /// <summary>
        /// 设备平台枚举。
        /// </summary>
        public static class DevicePlatform
        {
            /// <summary>
            /// 设备平台枚举
            /// </summary>
            public enum DevicePlamformEnum
            {
                /// <summary>
                /// 未知平台
                /// </summary>
                NULL = 0,
                /// <summary>
                /// Windows平台
                /// </summary>
                Windows = 1,
                /// <summary>
                /// iOS平台
                /// </summary>
                iOS = 2,
                /// <summary>
                /// Android平台
                /// </summary>
                Android = 3,
            }

            /// <summary>
            /// 获取当前设备平台枚举
            /// </summary>
            public static DevicePlamformEnum GetCurDevicePlamform()
            {
#if UNITY_ANDROID
                {
                    return DevicePlamformEnum.Android;
                }
#elif UNITY_IOS
                 {
                    return DevicePlamformEnum.iOS;
                 }
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                {
                    return DevicePlamformEnum.Windows;
                }
#else
                {
                    return DevicePlamformEnum.NULL;
                }
#endif
            }

            /// <summary>
            /// 获取当前设备平台路径字符串
            /// </summary>
            /// <returns></returns>
            public static string GetCurDevicePlatformPath()
            {
#if UNITY_ANDROID
        {
            return "android";
        }
#elif UNITY_IOS
        {
            return "ios";
        }
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                {
                    return "windows";
                }
#else
        {
            return "null";
        }
#endif
            }

        }
    }
}
