namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源包失败回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源包名称。</param>
    /// <param name="status">加载资源包状态。</param>
    /// <param name="errorMessage">错误信息。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetBundleFailureCallback(string assetBundleName, LoadResourceStatus status, string errorMessage, object userData);
}
