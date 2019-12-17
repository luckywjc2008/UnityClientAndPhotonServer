using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityGameFramework.Editor
{
    public static partial class UnityUtility
    {
        public static class EditorPath
        {
            public static string GetBuildTargetName(BuildTarget buildTarget)
            {
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneWindows:
                        return "windows";
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
#else
                    case BuildTarget.StandaloneOSXUniversal:
#endif
                        return "osx";
                    case BuildTarget.iOS:
                        return "ios";
                    case BuildTarget.Android:
                        return "android";
                    case BuildTarget.WSAPlayer:
                        return "winstore";
                    default:
                        return "notsupported";
                }
            }
        }
    }

}
