﻿using GameFramework;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public static class ConfigExtension
    {
        public static void LoadConfig(this ConfigComponent configComponent, string configName, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("ConfigCustom name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName), userData);
        }
    }
}
