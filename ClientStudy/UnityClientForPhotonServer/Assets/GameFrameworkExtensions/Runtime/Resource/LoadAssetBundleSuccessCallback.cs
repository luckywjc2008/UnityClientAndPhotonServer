namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源包成功回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源包名称。</param>
    /// <param name="asset">已加载的资源包。</param>
    /// <param name="duration">加载持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetBundleSuccessCallback(string assetBundleName, object assetBundle, float duration, object userData);
}
