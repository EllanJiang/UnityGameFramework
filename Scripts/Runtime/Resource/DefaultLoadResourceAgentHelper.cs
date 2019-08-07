//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.SceneManagement;
using Utility = GameFramework.Utility;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认加载资源代理辅助器。
    /// </summary>
    public class DefaultLoadResourceAgentHelper : LoadResourceAgentHelperBase, IDisposable
    {
        private string m_FileFullPath = null;
        private string m_BytesFullPath = null;
        private int m_LoadType = 0;
        private string m_AssetName = null;
        private float m_LastProgress = 0f;
        private bool m_Disposed = false;
#if UNITY_5_4_OR_NEWER
        private UnityWebRequest m_UnityWebRequest = null;
#else
        private WWW m_WWW = null;
#endif
        private AssetBundleCreateRequest m_FileAssetBundleCreateRequest = null;
        private AssetBundleCreateRequest m_BytesAssetBundleCreateRequest = null;
        private AssetBundleRequest m_AssetBundleRequest = null;
        private AsyncOperation m_AsyncOperation = null;

        private EventHandler<LoadResourceAgentHelperUpdateEventArgs> m_LoadResourceAgentHelperUpdateEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> m_LoadResourceAgentHelperReadFileCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> m_LoadResourceAgentHelperReadBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> m_LoadResourceAgentHelperParseBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> m_LoadResourceAgentHelperLoadCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperErrorEventArgs> m_LoadResourceAgentHelperErrorEventHandler = null;

        /// <summary>
        /// 加载资源代理辅助器异步加载资源更新事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
        {
            add
            {
                m_LoadResourceAgentHelperUpdateEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步读取资源文件完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
        {
            add
            {
                m_LoadResourceAgentHelperReadFileCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperReadFileCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
        {
            add
            {
                m_LoadResourceAgentHelperReadBytesCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperReadBytesCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
        {
            add
            {
                m_LoadResourceAgentHelperParseBytesCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperParseBytesCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
        {
            add
            {
                m_LoadResourceAgentHelperLoadCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperLoadCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
        {
            add
            {
                m_LoadResourceAgentHelperErrorEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源文件。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        public override void ReadFile(string fullPath)
        {
            if (m_LoadResourceAgentHelperReadFileCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_FileFullPath = fullPath;
            m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(m_FileFullPath);
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源二进制流。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        /// <param name="loadType">资源加载方式。</param>
        public override void ReadBytes(string fullPath, int loadType)
        {
            if (m_LoadResourceAgentHelperReadBytesCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_BytesFullPath = fullPath;
            m_LoadType = loadType;
#if UNITY_5_4_OR_NEWER
            m_UnityWebRequest = UnityWebRequest.Get(Utility.Path.GetRemotePath(fullPath));
#if UNITY_2017_2_OR_NEWER
            m_UnityWebRequest.SendWebRequest();
#else
            m_UnityWebRequest.Send();
#endif
#else
            m_WWW = new WWW(Utility.Path.GetRemotePath(fullPath));
#endif
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步将资源二进制流转换为加载对象。
        /// </summary>
        /// <param name="bytes">要加载资源的二进制流。</param>
        public override void ParseBytes(byte[] bytes)
        {
            if (m_LoadResourceAgentHelperParseBytesCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_BytesAssetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bytes);
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步加载资源。
        /// </summary>
        /// <param name="resource">资源。</param>
        /// <param name="assetName">要加载的资源名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="isScene">要加载的资源是否是场景。</param>
        public override void LoadAsset(object resource, string assetName, Type assetType, bool isScene)
        {
            if (m_LoadResourceAgentHelperLoadCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            AssetBundle assetBundle = resource as AssetBundle;
            if (assetBundle == null)
            {
                LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.TypeError, "Can not load asset bundle from loaded resource which is not an asset bundle.");
                m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, "Can not load asset from asset bundle which child name is invalid.");
                m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                return;
            }

            m_AssetName = assetName;
            if (isScene)
            {
                int sceneNamePositionStart = assetName.LastIndexOf('/');
                int sceneNamePositionEnd = assetName.LastIndexOf('.');
                if (sceneNamePositionStart <= 0 || sceneNamePositionEnd <= 0 || sceneNamePositionStart > sceneNamePositionEnd)
                {
                    LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Scene name '{0}' is invalid.", assetName));
                    m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    return;
                }

                string sceneName = assetName.Substring(sceneNamePositionStart + 1, sceneNamePositionEnd - sceneNamePositionStart - 1);
                m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                if (assetType != null)
                {
                    m_AssetBundleRequest = assetBundle.LoadAssetAsync(assetName, assetType);
                }
                else
                {
                    m_AssetBundleRequest = assetBundle.LoadAssetAsync(assetName);
                }
            }
        }

        /// <summary>
        /// 重置加载资源代理辅助器。
        /// </summary>
        public override void Reset()
        {
            m_FileFullPath = null;
            m_BytesFullPath = null;
            m_LoadType = 0;
            m_AssetName = null;
            m_LastProgress = 0f;

#if UNITY_5_4_OR_NEWER
            if (m_UnityWebRequest != null)
            {
                m_UnityWebRequest.Dispose();
                m_UnityWebRequest = null;
            }
#else
            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }
#endif

            m_FileAssetBundleCreateRequest = null;
            m_BytesAssetBundleCreateRequest = null;
            m_AssetBundleRequest = null;
            m_AsyncOperation = null;
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
#if UNITY_5_4_OR_NEWER
                if (m_UnityWebRequest != null)
                {
                    m_UnityWebRequest.Dispose();
                    m_UnityWebRequest = null;
                }
#else
                if (m_WWW != null)
                {
                    m_WWW.Dispose();
                    m_WWW = null;
                }
#endif
            }

            m_Disposed = true;
        }

        private void Update()
        {
#if UNITY_5_4_OR_NEWER
            UpdateUnityWebRequest();
#else
            UpdateWWW();
#endif
            UpdateFileAssetBundleCreateRequest();
            UpdateBytesAssetBundleCreateRequest();
            UpdateAssetBundleRequest();
            UpdateAsyncOperation();
        }

#if UNITY_5_4_OR_NEWER
        private void UpdateUnityWebRequest()
        {
            if (m_UnityWebRequest != null)
            {
                if (m_UnityWebRequest.isDone)
                {
                    if (string.IsNullOrEmpty(m_UnityWebRequest.error))
                    {
                        LoadResourceAgentHelperReadBytesCompleteEventArgs loadResourceAgentHelperReadBytesCompleteEventArgs = LoadResourceAgentHelperReadBytesCompleteEventArgs.Create(m_UnityWebRequest.downloadHandler.data, m_LoadType);
                        m_LoadResourceAgentHelperReadBytesCompleteEventHandler(this, loadResourceAgentHelperReadBytesCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperReadBytesCompleteEventArgs);
                        m_UnityWebRequest.Dispose();
                        m_UnityWebRequest = null;
                        m_BytesFullPath = null;
                        m_LoadType = 0;
                        m_LastProgress = 0f;
                    }
                    else
                    {
                        bool isError = false;
#if UNITY_2017_1_OR_NEWER
                        isError = m_UnityWebRequest.isNetworkError;
#else
                        isError = m_UnityWebRequest.isError;
#endif
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, Utility.Text.Format("Can not load asset bundle '{0}' with error message '{1}'.", m_BytesFullPath, isError ? m_UnityWebRequest.error : null));
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_UnityWebRequest.downloadProgress != m_LastProgress)
                {
                    m_LastProgress = m_UnityWebRequest.downloadProgress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.ReadResource, m_UnityWebRequest.downloadProgress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }
#else
        private void UpdateWWW()
        {
            if (m_WWW != null)
            {
                if (m_WWW.isDone)
                {
                    if (string.IsNullOrEmpty(m_WWW.error))
                    {
                        LoadResourceAgentHelperReadBytesCompleteEventArgs loadResourceAgentHelperReadBytesCompleteEventArgs = LoadResourceAgentHelperReadBytesCompleteEventArgs.Create(m_WWW.bytes, m_LoadType);
                        m_LoadResourceAgentHelperReadBytesCompleteEventHandler(this, loadResourceAgentHelperReadBytesCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperReadBytesCompleteEventArgs);
                        m_WWW.Dispose();
                        m_WWW = null;
                        m_BytesFullPath = null;
                        m_LoadType = 0;
                        m_LastProgress = 0f;
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, Utility.Text.Format("Can not load asset bundle '{0}' with error message '{1}'.", m_BytesFullPath, m_WWW.error));
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_WWW.progress != m_LastProgress)
                {
                    m_LastProgress = m_WWW.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.ReadResource, m_WWW.progress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }
#endif

        private void UpdateFileAssetBundleCreateRequest()
        {
            if (m_FileAssetBundleCreateRequest != null)
            {
                if (m_FileAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = m_FileAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldFileAssetBundleCreateRequest = m_FileAssetBundleCreateRequest;
                        LoadResourceAgentHelperReadFileCompleteEventArgs loadResourceAgentHelperReadFileCompleteEventArgs = LoadResourceAgentHelperReadFileCompleteEventArgs.Create(assetBundle);
                        m_LoadResourceAgentHelperReadFileCompleteEventHandler(this, loadResourceAgentHelperReadFileCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperReadFileCompleteEventArgs);
                        if (m_FileAssetBundleCreateRequest == oldFileAssetBundleCreateRequest)
                        {
                            m_FileAssetBundleCreateRequest = null;
                            m_LastProgress = 0f;
                        }
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, Utility.Text.Format("Can not load asset bundle from file '{0}' which is not a valid asset bundle.", m_FileFullPath));
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_FileAssetBundleCreateRequest.progress != m_LastProgress)
                {
                    m_LastProgress = m_FileAssetBundleCreateRequest.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadResource, m_FileAssetBundleCreateRequest.progress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }

        private void UpdateBytesAssetBundleCreateRequest()
        {
            if (m_BytesAssetBundleCreateRequest != null)
            {
                if (m_BytesAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = m_BytesAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldBytesAssetBundleCreateRequest = m_BytesAssetBundleCreateRequest;
                        LoadResourceAgentHelperParseBytesCompleteEventArgs loadResourceAgentHelperParseBytesCompleteEventArgs = LoadResourceAgentHelperParseBytesCompleteEventArgs.Create(assetBundle);
                        m_LoadResourceAgentHelperParseBytesCompleteEventHandler(this, loadResourceAgentHelperParseBytesCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperParseBytesCompleteEventArgs);
                        if (m_BytesAssetBundleCreateRequest == oldBytesAssetBundleCreateRequest)
                        {
                            m_BytesAssetBundleCreateRequest = null;
                            m_LastProgress = 0f;
                        }
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.NotExist, "Can not load asset bundle from memory which is not a valid asset bundle.");
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_BytesAssetBundleCreateRequest.progress != m_LastProgress)
                {
                    m_LastProgress = m_BytesAssetBundleCreateRequest.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadResource, m_BytesAssetBundleCreateRequest.progress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }

        private void UpdateAssetBundleRequest()
        {
            if (m_AssetBundleRequest != null)
            {
                if (m_AssetBundleRequest.isDone)
                {
                    if (m_AssetBundleRequest.asset != null)
                    {
                        LoadResourceAgentHelperLoadCompleteEventArgs loadResourceAgentHelperLoadCompleteEventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(m_AssetBundleRequest.asset);
                        m_LoadResourceAgentHelperLoadCompleteEventHandler(this, loadResourceAgentHelperLoadCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperLoadCompleteEventArgs);
                        m_AssetName = null;
                        m_LastProgress = 0f;
                        m_AssetBundleRequest = null;
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Can not load asset '{0}' from asset bundle which is not exist.", m_AssetName));
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_AssetBundleRequest.progress != m_LastProgress)
                {
                    m_LastProgress = m_AssetBundleRequest.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadAsset, m_AssetBundleRequest.progress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }

        private void UpdateAsyncOperation()
        {
            if (m_AsyncOperation != null)
            {
                if (m_AsyncOperation.isDone)
                {
                    if (m_AsyncOperation.allowSceneActivation)
                    {
                        LoadResourceAgentHelperLoadCompleteEventArgs loadResourceAgentHelperLoadCompleteEventArgs = LoadResourceAgentHelperLoadCompleteEventArgs.Create(new SceneAsset());
                        m_LoadResourceAgentHelperLoadCompleteEventHandler(this, loadResourceAgentHelperLoadCompleteEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperLoadCompleteEventArgs);
                        m_AssetName = null;
                        m_LastProgress = 0f;
                        m_AsyncOperation = null;
                    }
                    else
                    {
                        LoadResourceAgentHelperErrorEventArgs loadResourceAgentHelperErrorEventArgs = LoadResourceAgentHelperErrorEventArgs.Create(LoadResourceStatus.AssetError, Utility.Text.Format("Can not load scene asset '{0}' from asset bundle.", m_AssetName));
                        m_LoadResourceAgentHelperErrorEventHandler(this, loadResourceAgentHelperErrorEventArgs);
                        ReferencePool.Release(loadResourceAgentHelperErrorEventArgs);
                    }
                }
                else if (m_AsyncOperation.progress != m_LastProgress)
                {
                    m_LastProgress = m_AsyncOperation.progress;
                    LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = LoadResourceAgentHelperUpdateEventArgs.Create(LoadResourceProgress.LoadScene, m_AsyncOperation.progress);
                    m_LoadResourceAgentHelperUpdateEventHandler(this, loadResourceAgentHelperUpdateEventArgs);
                    ReferencePool.Release(loadResourceAgentHelperUpdateEventArgs);
                }
            }
        }
    }
}
