namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private sealed class LoadAssetBundleTask : LoadResourceTaskBase
            {
                private readonly LoadAssetBundleCallbacks m_LoadAssetBundleCallbacks;

                public override bool IsScene
                {
                    get
                    {
                        return false;
                    }
                }

                public LoadAssetBundleTask(string assetBundleName, ResourceInfo resourceInfo, LoadAssetBundleCallbacks loadAssetBundleCallbacks, string resourceChildName, object userData)
                    : base("", assetBundleName,resourceInfo, null,null, resourceChildName, userData)
                {
                    m_LoadAssetBundleCallbacks = loadAssetBundleCallbacks;
                }

                public override void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                    base.OnLoadAssetSuccess(agent, asset, duration);
                    if (m_LoadAssetBundleCallbacks.LoadAssetBundleSuccessCallback != null)
                    {
                        m_LoadAssetBundleCallbacks.LoadAssetBundleSuccessCallback(AssetBundleName, asset, duration, UserData);
                    }
                }

                public override void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status, string errorMessage)
                {
                    base.OnLoadAssetFailure(agent, status, errorMessage);
                    if (m_LoadAssetBundleCallbacks.LoadAssetBundleFailureCallback != null)
                    {
                        m_LoadAssetBundleCallbacks.LoadAssetBundleFailureCallback(AssetBundleName, status, errorMessage, UserData);
                    }
                }

                public override void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type, float progress)
                {
                    base.OnLoadAssetUpdate(agent, type, progress);
                    if (type == LoadResourceProgress.LoadAsset)
                    {
                        if (m_LoadAssetBundleCallbacks.LoadAssetBundleUpdateCallback != null)
                        {
                            m_LoadAssetBundleCallbacks.LoadAssetBundleUpdateCallback(AssetBundleName, progress, UserData);
                        }
                    }
                }

                //public override void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName, object dependencyAsset)
                //{
                //    base.OnLoadDependencyAsset(agent, dependencyAssetName, dependencyAsset);
                //    if (m_LoadAssetBundleCallbacks.LoadAssetBundleDependencyAssetCallback != null)
                //    {
                //        m_LoadAssetBundleCallbacks.LoadAssetBundleDependencyAssetCallback(AssetName, dependencyAssetName, LoadedDependencyAssetCount, TotalDependencyAssetCount, UserData);
                //    }
                //}
            }
        }
    }
}
