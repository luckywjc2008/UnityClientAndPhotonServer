//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源回调函数集。
    /// </summary>
    public sealed class LoadAssetBundleCallbacks
    {
        private readonly LoadAssetBundleSuccessCallback m_LoadAssetBundleSuccessCallback;
        private readonly LoadAssetBundleFailureCallback m_LoadAssetBundleFailureCallback;
        private readonly LoadAssetBundleUpdateCallback m_LoadAssetBundleUpdateCallback;
       

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetBundleSuccessCallback">加载资源成功回调函数。</param>
        public LoadAssetBundleCallbacks(LoadAssetBundleSuccessCallback loadAssetBundleSuccessCallback)
            : this(loadAssetBundleSuccessCallback, null, null)
        {

        }

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetBundleSuccessCallback">加载资源成功回调函数。</param>
        /// <param name="loadAssetBundleFailureCallback">加载资源失败回调函数。</param>
        /// <param name="loadAssetBundleDependencyAssetBundleCallback">加载资源时加载依赖资源回调函数。</param>
        public LoadAssetBundleCallbacks(LoadAssetBundleSuccessCallback loadAssetBundleSuccessCallback, LoadAssetBundleFailureCallback loadAssetBundleFailureCallback)
            : this(loadAssetBundleSuccessCallback, loadAssetBundleFailureCallback, null)
        {

        }

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetBundleSuccessCallback">加载资源成功回调函数。</param>
        /// <param name="loadAssetBundleUpdateCallback">加载资源更新回调函数。</param>
        public LoadAssetBundleCallbacks(LoadAssetBundleSuccessCallback loadAssetBundleSuccessCallback, LoadAssetBundleUpdateCallback loadAssetBundleUpdateCallback)
            : this(loadAssetBundleSuccessCallback, null, loadAssetBundleUpdateCallback)
        {

        }

        /// <summary>
        /// 初始化加载资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadAssetBundleSuccessCallback">加载资源成功回调函数。</param>
        /// <param name="loadAssetBundleFailureCallback">加载资源失败回调函数。</param>
        /// <param name="loadAssetBundleUpdateCallback">加载资源更新回调函数。</param>
        /// <param name="loadAssetBundleDependencyAssetBundleCallback">加载资源时加载依赖资源回调函数。</param>
        public LoadAssetBundleCallbacks(LoadAssetBundleSuccessCallback loadAssetBundleSuccessCallback, LoadAssetBundleFailureCallback loadAssetBundleFailureCallback, LoadAssetBundleUpdateCallback loadAssetBundleUpdateCallback)
        {
            if (loadAssetBundleSuccessCallback == null)
            {
                throw new GameFrameworkException("Load AssetBundle success callback is invalid.");
            }

            m_LoadAssetBundleSuccessCallback = loadAssetBundleSuccessCallback;
            m_LoadAssetBundleFailureCallback = loadAssetBundleFailureCallback;
            m_LoadAssetBundleUpdateCallback = loadAssetBundleUpdateCallback;
           
        }

        /// <summary>
        /// 获取加载资源成功回调函数。
        /// </summary>
        public LoadAssetBundleSuccessCallback LoadAssetBundleSuccessCallback
        {
            get
            {
                return m_LoadAssetBundleSuccessCallback;
            }
        }

        /// <summary>
        /// 获取加载资源失败回调函数。
        /// </summary>
        public LoadAssetBundleFailureCallback LoadAssetBundleFailureCallback
        {
            get
            {
                return m_LoadAssetBundleFailureCallback;
            }
        }

        /// <summary>
        /// 获取加载资源更新回调函数。
        /// </summary>
        public LoadAssetBundleUpdateCallback LoadAssetBundleUpdateCallback
        {
            get
            {
                return m_LoadAssetBundleUpdateCallback;
            }
        }

    }
}
