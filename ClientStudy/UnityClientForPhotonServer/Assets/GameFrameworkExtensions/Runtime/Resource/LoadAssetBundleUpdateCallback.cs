namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源包更新回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源包名称。</param>
    /// <param name="progress">加载资源包进度。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetBundleUpdateCallback(string assetBundleName, float progress, object userData);
}
