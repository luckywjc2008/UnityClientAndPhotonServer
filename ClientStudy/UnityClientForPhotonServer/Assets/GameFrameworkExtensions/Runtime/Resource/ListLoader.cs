using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Resource;
using GameFramework;

/************************************
* 类    名:ListLoader
* 类 功 能:队列加载器
* 作    者:lizhu
* 创建时间:2014-07-04
* 最后一次修改作者:
* 最后一次修改时间:
*************************************/
namespace UnityGameFramework.Runtime
{
    public class ListLoader
    {
        public enum LoadMode
        {
            SingleCoroutineMode = 1,//单协程加载
            MultiCoroutineMode //多协程加载
        }

        /// <summary>
        /// 队列加载线程模式
        /// </summary>
        private LoadMode m_mode;

        private List<string> m_NeedLoadDic;

        private int m_completeIndex = 0;
        private int m_currentIndex = 0;
        private int m_total = 0;
        private bool m_isPaused = false;

        /// <summary>
        /// 最大开启协程数量
        /// </summary>
        private int m_MaxCoroutineNum = 3;
        /// <summary>
        /// 当前空闲协程数量
        /// </summary>
        private int m_CurFreeCoroutineNum;
        /// <summary>
        /// 当前已启动协程数量
        /// </summary>
        private int m_CurStartCoroutineNum;

        public delegate void FinishedHandler(bool manual);
        public event FinishedHandler OneFinished;


        public delegate void ListFinishedHandler(bool manual);
        public event ListFinishedHandler ListFinished;

        public LoadAssetCallbacks m_loadAssetCallbacks;

        public LoadAssetBundleCallbacks m_loadAssetBundleCallbacks;

        public ListLoader(LoadMode mode = LoadMode.SingleCoroutineMode, LoadAssetCallbacks loadAssetCallbacks = null)
        {
            m_mode = mode;
            m_loadAssetCallbacks = new LoadAssetCallbacks(
                        loadAssetSuccessCallback: delegate (string assetName, object asset, float duration, object userData)
                        {
                            Log.Info("ListLoader :asset(" + assetName + ") load success");
                            loadAssetCallbacks.LoadAssetSuccessCallback(assetName, asset, duration, userData);
                            CallbackOneLoadFinished(true);
                        },
                        loadAssetFailureCallback: delegate (string assetName, LoadResourceStatus status, string errorMessage, object userData)
                        {
                            Log.Info("ListLoader :asset (" + assetName + ")load failure: " + errorMessage);
                            CallbackOneLoadFinished(true);
                        });

            m_loadAssetBundleCallbacks = new LoadAssetBundleCallbacks(
                        loadAssetBundleSuccessCallback: delegate (string assetName, object asset, float duration, object userData)
                        {
                            Log.Info("ListLoader :asset bundle(" + assetName + ") load success");
                            loadAssetCallbacks.LoadAssetSuccessCallback(assetName, asset, duration, userData);
                            CallbackOneLoadFinished(true);
                        },
                        loadAssetBundleFailureCallback: delegate (string assetName, LoadResourceStatus status, string errorMessage, object userData)
                        {
                            Log.Info("ListLoader :asset bundle(" + assetName + ")load failure: " + errorMessage);
                            CallbackOneLoadFinished(true);
                        });

            if (m_mode == LoadMode.SingleCoroutineMode)
            {
                m_CurFreeCoroutineNum = 1;
            }
            else
            {
                m_MaxCoroutineNum = GameEntry.GetComponent<ResourceComponent>().LoadResourceAgentHelperCount;
                m_CurFreeCoroutineNum = m_MaxCoroutineNum;
            }
            m_NeedLoadDic = new List<string>();
        }

        /// <summary>
        /// 增加下载项
        /// </summary>
        public void Add(string name)
        {
            if (!m_NeedLoadDic.Contains(name))
            {
                m_NeedLoadDic.Add(name);
            }
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public void StartLoad()
        {
            if (m_isPaused)
                return;
            m_total = m_NeedLoadDic.Count;

            if (m_CurFreeCoroutineNum > 0)
            {
                CallLoad();
            }
        }

        private void CallLoad()
        {
            if (m_isPaused)
                return;
            int counter = m_CurFreeCoroutineNum;
            while (m_CurStartCoroutineNum < m_NeedLoadDic.Count && m_CurStartCoroutineNum < m_MaxCoroutineNum)
            {
                LoadNext();
                counter--;
                if (counter <= 0)
                {
                    //确保不会出现死循环
                    break;
                }
            }
            //如果没有可加载的，直接队列加载完成回调
            if (m_NeedLoadDic.Count == 0)
            {
                CallbackListLoadFinished(false);
            }

        }

        private void LoadNext()
        {
            if (m_isPaused)
                return;
            //没有更多的空闲协程可用
            if (m_CurFreeCoroutineNum <= 0)
                return;
            if (!isDone())
            {
                if (m_currentIndex < m_total && m_NeedLoadDic.Count > m_CurStartCoroutineNum)
                {
                    m_CurStartCoroutineNum++;
                    Task task = new Task(LoadAssetIEnumerator(), false);
                    if (task != null)
                    {
                        task.Start();
                    }
                }
            }
        }

        private IEnumerator LoadAssetIEnumerator()
        {
            m_CurFreeCoroutineNum--;
            if (GameEntry.GetComponent<BaseComponent>().EditorResourceMode)
                GameEntry.GetComponent<ResourceComponent>().LoadAsset(m_NeedLoadDic[m_currentIndex], m_loadAssetCallbacks);
            else
                GameEntry.GetComponent<ResourceComponent>().LoadAssetBundle(m_NeedLoadDic[m_currentIndex], m_loadAssetBundleCallbacks, null);
            m_currentIndex++;
            yield return null;
        }

        /// <summary>
        /// 每个资源加载完成回调函数
        /// </summary>
        private void CallbackOneLoadFinished(bool manual)
        {
            m_CurFreeCoroutineNum++;
            m_completeIndex++;
            FinishedHandler handler = OneFinished;
            if (handler != null)
            {
                handler(manual);
            }

            if (isDone())
            {
                CallbackListLoadFinished(manual);
            }
            else
            {
                LoadNext();
            }
        }

        /// <summary>
        /// 队列加载完成回调函数
        /// </summary>
        private void CallbackListLoadFinished(bool manual)
        {
            m_NeedLoadDic = null;

            ListFinishedHandler handler = ListFinished;
            if (handler != null)
            {
                handler(manual);
            }
        }

        /// <summary>
        /// 判断是否已完成队列加载
        /// </summary>
        /// <returns></returns>
        private bool isDone()
        {
            return m_completeIndex >= m_total;
        }
    }
}
