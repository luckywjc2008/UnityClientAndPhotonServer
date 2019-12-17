using GameFramework;
using GameFramework.Download;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源组件。
    /// </summary>
    public sealed partial class ResourceComponent : GameFrameworkComponent
    {
        private bool m_IsLoadResourceReady;
       
        /// <summary>
        /// 正在加载中的资源名称列表
        /// </summary>
        private Dictionary<string, LoadAssetBundleCallbacks> m_LoadingAssetList;

        /// <summary>
        /// 已加载的资源包名-资源包字典
        /// </summary>
        private Dictionary<string, AssetBundle> m_LoadedAssetBundleDic;

        /// <summary>
        /// 是否已准备好资源加载
        /// </summary>
        public bool IsLoadResourceReady
        {
            get { return m_IsLoadResourceReady; }
            set { m_IsLoadResourceReady = value; }
        }

        void InitComponent()
        {
            m_LoadingAssetList = new Dictionary<string, LoadAssetBundleCallbacks>();
            m_LoadedAssetBundleDic = new Dictionary<string, AssetBundle>();
        }

        /// <summary>
        /// 异步加载资源包
        /// </summary>
        /// <param name="assetBundleName">要加载的资源包的名称。</param>
        /// <param name="loadAssetBundleCallbacks">加载回调函数。</param>
        /// <param name="userData">自定义数据。</param>
        public void LoadAssetBundle(string assetBundleName, LoadAssetBundleCallbacks loadAssetBundleCallbacks, object userData)
        {
            if (IsLoadResourceReady)
            {
                m_ResourceManager.LoadAssetBundle(assetBundleName, loadAssetBundleCallbacks, userData);
            }
            else
            {
                if (m_LoadedAssetBundleDic.ContainsKey(assetBundleName))
                {
                    if (loadAssetBundleCallbacks != null)
                    {
                        if (loadAssetBundleCallbacks.LoadAssetBundleSuccessCallback != null)
                        {
                            loadAssetBundleCallbacks.LoadAssetBundleSuccessCallback(assetBundleName, GetAssetBundle(assetBundleName), 0, userData);
                        }
                    }
                    return;
                }

                if (m_LoadingAssetList.ContainsKey(assetBundleName))
                    return;

                m_LoadingAssetList.Add(assetBundleName, loadAssetBundleCallbacks);
                string filePath = Utility.Path.GetCombinePath(m_ResourceManager.ReadWritePath, Utility.Path.GetResourceNameWithSuffix(assetBundleName));

                if (File.Exists(filePath))
                {
                    filePath = Utility.Path.GetRemotePath(m_ResourceManager.ReadWritePath, Utility.Path.GetResourceNameWithSuffix(assetBundleName));
                }
                else
                {
                    filePath = Utility.Path.GetRemotePath(m_ResourceManager.ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(assetBundleName));
                }

                StartCoroutine(LoadAssetBundleCo(assetBundleName, filePath, loadAssetBundleCallbacks, userData));
            }
        }

        /// <summary>
        /// 非框架托管式资源包加载
        /// </summary>
        /// <param name="assetBundleName">资源包名称。</param>
        /// <param name="fileUri">资源包文件路径。</param>
        /// <param name="loadAssetBundleCallback">加载回调函数。</param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundleCo(string assetBundleName, string fileUri, LoadAssetBundleCallbacks loadAssetBundleCallbacks, object userData)
        {
            WWW www = new WWW(fileUri);
            yield return www;

            AssetBundle assetBundle = www.assetBundle;
            string errorMessage = www.error;
            www.Dispose();

            if (loadAssetBundleCallbacks != null)
            {
                if (string.IsNullOrEmpty(errorMessage) && assetBundle != null)
                {
                    m_LoadedAssetBundleDic.Add(assetBundleName, assetBundle);
                    if (loadAssetBundleCallbacks.LoadAssetBundleSuccessCallback != null)
                    {
                        loadAssetBundleCallbacks.LoadAssetBundleSuccessCallback(assetBundleName, assetBundle, 0, userData);
                    }
                }
                else
                {
                    if (loadAssetBundleCallbacks.LoadAssetBundleFailureCallback != null)
                    {
                        loadAssetBundleCallbacks.LoadAssetBundleFailureCallback(assetBundleName, LoadResourceStatus.NotExist, errorMessage, userData);
                    }
                }
            }
            m_LoadingAssetList.Remove(assetBundleName);
        }

        /// <summary>
        /// 获取资源包
        /// </summary>
        /// <param name="assetBundleName">资源包名称。</param>
        /// <returns>资源包。</returns>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
            AssetBundle assetBundle;
            if (IsLoadResourceReady)
            {
                object bundle = m_ResourceManager.GetAssetBundle(assetBundleName);
                assetBundle = bundle != null ? (AssetBundle)bundle : null;
            }
            else
            {
                if (m_LoadedAssetBundleDic.ContainsKey(assetBundleName))
                    assetBundle = m_LoadedAssetBundleDic[assetBundleName];
                else
                    assetBundle = null;
            }

            return assetBundle;
        }

        /// <summary>
        /// 卸载资源包。
        /// </summary>
        /// <param name="assetBundleName">资源包名称。</param>
        /// <param name="allowDestroyLoadedAssets">是否销毁从资源包中已加载的资源</param>
        public void UnloadAssetBundle(string assetBundleName, bool allowDestroyLoadedAssets)
        {
            if (IsLoadResourceReady)
            {
                m_ResourceManager.UnloadAssetBundle(assetBundleName, allowDestroyLoadedAssets);
            }
            else
            {
                if (m_LoadedAssetBundleDic.ContainsKey(assetBundleName))
                {
                    AssetBundle assetBundle = m_LoadedAssetBundleDic[assetBundleName];
                    if (assetBundle != null)
                    {
                        assetBundle.Unload(allowDestroyLoadedAssets);
                        assetBundle = null;
                    }
                    m_LoadedAssetBundleDic.Remove(assetBundleName);
                }
            }
        }

        public string GetReadOnlyPath()
        {
            return m_ResourceManager.ReadOnlyPath;
        }

        public string GetReadWritePath()
        {
            return m_ResourceManager.ReadWritePath;
        }

        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
     
        /// <summary>
        /// 直接从指定文件路径读取数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallback">读取数据流回调函数。</param>
        public void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback)
        {
            m_ResourceHelper.LoadBytes(fileUri, loadBytesCallback);
        }

        /// <summary>
        /// 解析版本资源列表。
        /// </summary>
        /// <param name="fileUri">版本资源列表文件路径。</param>
        /// <param name="bytes">要解析的数据。</param>
        /// <param name="errorMessage">错误信息。</param>
        public int GetLocalResourceVersion(string fileUri, byte[] bytes, string errorMessage)
        {
            int _internalResourceVersion = 0;
            
            if (bytes == null || bytes.Length <= 0)
            {
                throw new GameFrameworkException(string.Format("Version list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
            }

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(bytes);
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    memoryStream = null;
                    char[] header = binaryReader.ReadChars(3);
                    if (header[0] != VersionListHeader[0] || header[1] != VersionListHeader[1] || header[2] != VersionListHeader[2])
                    {
                        throw new GameFrameworkException("Version list header is invalid.");
                    }

                    byte listVersion = binaryReader.ReadByte();

                    if (listVersion == 0)
                    {
                        byte[] encryptBytes = binaryReader.ReadBytes(4);
                        Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes)); //applicableGameVersion
                        _internalResourceVersion = binaryReader.ReadInt32();
                    }
                    else
                    {
                        throw new GameFrameworkException("Version list version is invalid.");
                    }
                }

                return _internalResourceVersion;
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(string.Format("Parse version list exception '{0}'.", exception.Message), exception);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
    }
}
