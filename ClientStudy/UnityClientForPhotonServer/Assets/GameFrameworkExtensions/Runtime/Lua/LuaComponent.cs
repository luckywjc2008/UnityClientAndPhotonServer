using GameFramework;
using GameFramework.Resource;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime.Lua
{
    /// <summary>
    /// Lua 组件。将 ToLua 插件集成到 UnityGameFramework 中。本类的实现参考 ToLua 中的 <see cref="LuaClient"/> 类。
    /// </summary>
    public class LuaComponent : GameFrameworkComponent
    {
        private LuaState m_LuaState = null;
        private LuaLooper m_LuaLooper = null;
      

        private Dictionary<string, byte[]> m_CachedLuaScripts = new Dictionary<string, byte[]>();

        [SerializeField]
        private bool m_UseLuaSocket = true;

        [SerializeField, Tooltip("Lua script search paths relative to 'Assets/' for editor use.")]
        private string[] m_EditorSearchPaths = null;

        public string[] EditorSearchPaths
        {
            get { return m_EditorSearchPaths; }
            set { m_EditorSearchPaths = value; }
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        [SerializeField, Tooltip("Try to connect to ZeroBraneStudio if editor resource mode is used.")]
        private bool m_UseZeroBraneStudioDebugger = true;

        [SerializeField, Tooltip("ZeroBraneStudio debug path.")]
        private string m_ZeroBraneStudioDebugPath = "C:/ZeroBraneStudio/lualibs/mobdebug";
#endif
    
        public delegate void OnLoadScriptSuccess(string fileName);
        public delegate void OnLoadScriptFailure(string fileName, LoadResourceStatus status, string errorMessage);

        /// <summary>
        /// 获取当前使用的 Lua 虚拟机实例。
        /// </summary>
        public LuaState LuaState
        {
            get
            {
                return m_LuaState;
            }
        }

        /// <summary>
        /// 启动 Lua 虚拟机。
        /// </summary>
        public void StartLuaVM()
        {
            m_LuaState.Start();
            StartLooper();
        }

        public void DoMainFile()
        {
            DoFile("Main.lua");
        }

        public void CollectGarbarge()
        {
            m_LuaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        /// <summary>
        /// 清理 Lua 虚拟机。
        /// </summary>
        /// <remarks>重启游戏时调用。</remarks>
        public void ClearLuaVM()
        {
            if(m_LuaState != null)
            {
                CollectGarbarge();
                Destroy();
            }
          
        }

        /// <summary>
        /// 加载 Lua 脚本文件。
        /// </summary>
        /// <param name="assetPath">Lua 脚本的资源路径。</param>
        /// <param name="fileName">Lua 脚本文件名。</param>
        /// <param name="onSuccess">加载成功回调。</param>
        /// <param name="onFailure">加载失败回调。</param>
        public void LoadFile(string assetPath, string fileName, OnLoadScriptSuccess onSuccess, OnLoadScriptFailure onFailure = null)
        {
            if (m_CachedLuaScripts.ContainsKey(fileName) || Application.isEditor && GameEntry.GetComponent<BaseComponent>().EditorResourceMode)
            {
                if (onSuccess != null)
                {
                    onSuccess(fileName);
                }

                return;
            }

            // Load lua script from AssetBundle.
            var innerCallbacks = new LoadAssetCallbacks(
                loadAssetSuccessCallback: OnLoadAssetSuccess,
                loadAssetFailureCallback: OnLoadAssetFailure);
            var userData = new LoadLuaScriptUserData { FileName = fileName, OnSuccess = onSuccess, OnFailure = onFailure };

            assetPath += ".bytes";
            GameEntry.GetComponent<ResourceComponent>().LoadAsset(assetPath, innerCallbacks, userData);
        }

        /// <summary>
        /// 卸载 Lua 脚本文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void UnloadFile(string fileName)
        {
            if (Application.isEditor && GameEntry.GetComponent<BaseComponent>().EditorResourceMode)
            {
                m_CachedLuaScripts.Remove(fileName);
            }
        }

        /// <summary>
        /// 执行 Lua 脚本字符串。
        /// </summary>
        /// <param name="chunk">代码块。</param>
        /// <param name="chunkName">代码块名称。</param>
        /// <returns>返回值列表。</returns>
        public object[] DoString(string chunk, string chunkName = "")
        {
            if (string.IsNullOrEmpty(chunkName))
            {
                return m_LuaState.DoString(chunk);
            }

            return m_LuaState.DoString(chunk, chunkName);
        }

        /// <summary>
        /// 执行 Lua 脚本文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns>返回值列表。</returns>
        public object[] DoFile(string fileName)
        {
            return m_LuaState.DoFile(fileName);
        }

        #region 执行Lua方法
        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="funcName">Lua方法名(完全限定)</param>
        /// <param name="args">Lua方法参数</param>
        /// <returns></returns>
        //public object[] CallFunction(string funcName, params object[] args)
        //{
        //    LuaFunction func = m_LuaState.GetFunction(funcName);
        //    if (func != null)
        //    {
        //        return func.Call(args);
        //    }
        //    return null;
        //}

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="funcName">Lua方法名(完全限定)</param>
        /// <param name="args">Lua方法参数</param>
        /// <returns></returns>
        public void CallFunction<T>(string funcName, T arg)
        {
            if (m_LuaState == null)
                return;

            LuaFunction func = m_LuaState.GetFunction(funcName);
            if (func != null)
            {
                func.BetterCall(arg);
            }
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="funcName">Lua方法名(完全限定)</param>
        /// <param name="arg0">Lua方法参数</param>
        /// <param name="arg1">Lua方法参数</param>
        /// <returns></returns>
        public void CallFunction<T0, T1>(string funcName, T0 arg0, T1 arg1)
        {
            if (m_LuaState == null)
                return;

            LuaFunction func = m_LuaState.GetFunction(funcName);
            if (func != null)
            {
                func.BetterCall(arg0, arg1);
            }
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="funcName">Lua方法名(完全限定)</param>
        /// <param name="arg0">Lua方法参数</param>
        /// <param name="arg1">Lua方法参数</param>
        /// <param name="arg2">Lua方法参数</param>
        /// <returns></returns>
        public void CallFunction<T0, T1, T2>(string funcName, T0 arg0, T1 arg1, T2 arg2)
        {
            if (m_LuaState == null)
                return;

            LuaFunction func = m_LuaState.GetFunction(funcName);
            if (func != null)
            {
                func.BetterCall(arg0, arg1, arg2);
            }
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        /// <param name="funcName">Lua方法名(完全限定)</param>
        /// <param name="arg0">Lua方法参数</param>
        /// <param name="arg1">Lua方法参数</param>
        /// <param name="arg2">Lua方法参数</param>
        /// <param name="arg3">Lua方法参数</param>
        /// <returns></returns>
        public void CallFunction<T0, T1, T2, T3>(string funcName, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (m_LuaState == null)
                return;
            LuaFunction func = m_LuaState.GetFunction(funcName);
            if (func != null)
            {
                func.BetterCall(arg0, arg1, arg2, arg3);
            }
        }
        #endregion

        #region MonoBehaviour

        private void Start()
        {
            //Init();//QQ游戏大厅，需要异步加载libtolua，所以Lua模块延迟初始化Lua VM
        }

        private void OnDestroy()
        {
            Destroy();
        }

        #endregion MonoBehaviour

        public void Init()
        {
            new CustomLuaLoader(GetScriptContent);
            m_LuaState = new LuaState();
            AddSearchPaths();
            OpenLibs();
            m_LuaState.LuaSetTop(0);
            Bind();
        }

        private void Destroy()
        {
            if(m_CachedLuaScripts != null)
                m_CachedLuaScripts.Clear();

            if (m_LuaLooper != null)
            {
                m_LuaLooper.Destroy();
                m_LuaLooper = null;
            }

            if (m_LuaState != null)
            {
                m_LuaState.Dispose();
                m_LuaState = null;
            }
        }

        void OnLevelLoaded(int level)
        {
            if (m_LuaState != null)
            {
                //luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
                m_LuaState.RefreshDelegateMap();
            }
        }

        private void OpenLibs()
        {
            m_LuaState.OpenLibs(LuaDLL.luaopen_pb);
            m_LuaState.OpenLibs(LuaDLL.luaopen_struct);
            m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);
            OpenCJson();
            //Luajit already owns bitop lib
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            m_LuaState.OpenLibs(LuaDLL.luaopen_bit);
#endif

            if (m_UseLuaSocket)
            {
                OpenLuaSocket();
            }

#if UNITY_EDITOR
            if (m_UseZeroBraneStudioDebugger)
            {
                StartZbsDebugger();
            }
#endif
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            m_LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson);
            m_LuaState.LuaSetField(-2, "cjson");

            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
            m_LuaState.LuaSetField(-2, "cjson.safe");
        }

        private void Bind()
        {
            LuaBinder.Bind(m_LuaState);
            LuaCoroutine.Register(m_LuaState, this);
        }

        private void StartLooper()
        {
            m_LuaLooper = gameObject.AddComponent<LuaLooper>();
            m_LuaLooper.luaState = m_LuaState;
        }

        private void AddSearchPaths()
        {
#if UNITY_EDITOR
            if (GameEntry.GetComponent<BaseComponent>().EditorResourceMode)
            {
                for (int i = 0; i < m_EditorSearchPaths.Length; ++i)
                {
                    m_LuaState.AddSearchPath(Utility.Path.GetCombinePath(Application.dataPath, m_EditorSearchPaths[i]));
                }
            }
#elif UNITY_STANDALONE_WIN
           
                for (int i = 0; i < EditorSearchPaths.Length; ++i)
                {
                    m_LuaState.AddSearchPath(Utility.Path.GetCombinePath(Application.streamingAssetsPath, EditorSearchPaths[i]));
                }
#endif
        }

        private void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            var myUserData = userData as LoadLuaScriptUserData;
            if (myUserData == null) return;

            if (myUserData.OnFailure != null)
            {
                myUserData.OnFailure(myUserData.FileName, status, errorMessage);
            }
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            var myUserData = userData as LoadLuaScriptUserData;
            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                throw new GameFramework.GameFrameworkException("The loaded asset should be a text asset.");
            }

            if (!m_CachedLuaScripts.ContainsKey(myUserData.FileName))
            {
                m_CachedLuaScripts.Add(myUserData.FileName, textAsset.bytes);
            }

            if (myUserData.OnSuccess != null)
            {
                myUserData.OnSuccess(myUserData.FileName);
            }
        }

        public void AddScriptContent(string fileName,byte[] buffer)
        {
            if (!m_CachedLuaScripts.ContainsKey(fileName))
            {
                m_CachedLuaScripts.Add(fileName, buffer);
            }
        }

        private bool GetScriptContent(string fileName, out byte[] buffer)
        {
            return m_CachedLuaScripts.TryGetValue(fileName, out buffer);
        }

#if UNITY_EDITOR
        private void StartZbsDebugger(string ip = "localhost")
        {
            if (!GameEntry.GetComponent<BaseComponent>().EditorResourceMode)
            {
                return;
            }

            if (!Directory.Exists(m_ZeroBraneStudioDebugPath))
            {
                Log.Warning("ZeroBraneStudio not install or LuaConst.zbsDir not right.");
                return;
            }

            if(!m_UseLuaSocket)
            {
                OpenLuaSocket();
            }
           

            if (!string.IsNullOrEmpty(m_ZeroBraneStudioDebugPath))
            {
                m_LuaState.AddSearchPath(m_ZeroBraneStudioDebugPath);
            }

            m_LuaState.LuaDoString(string.Format("DebugServerIp = '{0}'", ip));
        }
#endif

        #region LuaSocket

        private void OpenLuaSocket()
        {
            m_UseLuaSocket = true;

            m_LuaState.BeginPreLoad();
            m_LuaState.RegFunction("socket.core", LuaOpen_Socket_Core);
            m_LuaState.RegFunction("mime.core", LuaOpen_Mime_Core);
            m_LuaState.EndPreLoad();
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        #endregion


        private class LoadLuaScriptUserData
        {
            public string FileName;
            public OnLoadScriptSuccess OnSuccess;
            public OnLoadScriptFailure OnFailure;
        }
    }
}
