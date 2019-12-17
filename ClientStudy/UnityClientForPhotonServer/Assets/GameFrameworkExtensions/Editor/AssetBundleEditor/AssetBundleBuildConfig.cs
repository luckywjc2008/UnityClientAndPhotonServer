using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public class AssetBundleBuildConfig : ScriptableObject
    {
        public List<AssetBundleFilter> filters = new List<AssetBundleFilter>();
    }

    [System.Serializable]
    public class AssetBundleFilter
    {
        public bool valid = true;
        public string assetBundleName = string.Empty;
        public string assetBundleVariant = null;
        public string assetBundleGroup = string.Empty;
        public string path = string.Empty;
        public string filter = "*.*";
    }
}