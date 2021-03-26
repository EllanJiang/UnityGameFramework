//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Localization;
using GameFramework.Resource;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 本地化组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Localization")]
    public sealed class LocalizationComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private ILocalizationManager m_LocalizationManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableLoadDictionaryUpdateEvent = false;

        [SerializeField]
        private bool m_EnableLoadDictionaryDependencyAssetEvent = false;

        [SerializeField]
        private string m_LocalizationHelperTypeName = "UnityGameFramework.Runtime.DefaultLocalizationHelper";

        [SerializeField]
        private LocalizationHelperBase m_CustomLocalizationHelper = null;

        [SerializeField]
        private int m_CachedBytesSize = 0;

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language Language
        {
            get
            {
                return m_LocalizationManager.Language;
            }
            set
            {
                m_LocalizationManager.Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public Language SystemLanguage
        {
            get
            {
                return m_LocalizationManager.SystemLanguage;
            }
        }

        /// <summary>
        /// 获取字典数量。
        /// </summary>
        public int DictionaryCount
        {
            get
            {
                return m_LocalizationManager.DictionaryCount;
            }
        }

        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        public int CachedBytesSize
        {
            get
            {
                return m_LocalizationManager.CachedBytesSize;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_LocalizationManager = GameFrameworkEntry.GetModule<ILocalizationManager>();
            if (m_LocalizationManager == null)
            {
                Log.Fatal("Localization manager is invalid.");
                return;
            }

            m_LocalizationManager.ReadDataSuccess += OnReadDataSuccess;
            m_LocalizationManager.ReadDataFailure += OnReadDataFailure;

            if (m_EnableLoadDictionaryUpdateEvent)
            {
                m_LocalizationManager.ReadDataUpdate += OnReadDataUpdate;
            }

            if (m_EnableLoadDictionaryDependencyAssetEvent)
            {
                m_LocalizationManager.ReadDataDependencyAsset += OnReadDataDependencyAsset;
            }
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_LocalizationManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_LocalizationManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            LocalizationHelperBase localizationHelper = Helper.CreateHelper(m_LocalizationHelperTypeName, m_CustomLocalizationHelper);
            if (localizationHelper == null)
            {
                Log.Error("Can not create localization helper.");
                return;
            }

            localizationHelper.name = "Localization Helper";
            Transform transform = localizationHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_LocalizationManager.SetDataProviderHelper(localizationHelper);
            m_LocalizationManager.SetLocalizationHelper(localizationHelper);
            m_LocalizationManager.Language = baseComponent.EditorResourceMode && baseComponent.EditorLanguage != Language.Unspecified ? baseComponent.EditorLanguage : m_LocalizationManager.SystemLanguage;
            if (m_CachedBytesSize > 0)
            {
                EnsureCachedBytesSize(m_CachedBytesSize);
            }
        }

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            m_LocalizationManager.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        public void FreeCachedBytes()
        {
            m_LocalizationManager.FreeCachedBytes();
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        public void ReadData(string dictionaryAssetName)
        {
            m_LocalizationManager.ReadData(dictionaryAssetName);
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        public void ReadData(string dictionaryAssetName, int priority)
        {
            m_LocalizationManager.ReadData(dictionaryAssetName, priority);
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string dictionaryAssetName, object userData)
        {
            m_LocalizationManager.ReadData(dictionaryAssetName, userData);
        }

        /// <summary>
        /// 读取字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string dictionaryAssetName, int priority, object userData)
        {
            m_LocalizationManager.ReadData(dictionaryAssetName, priority, userData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryString">要解析的字典字符串。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(string dictionaryString)
        {
            return m_LocalizationManager.ParseData(dictionaryString);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryString">要解析的字典字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(string dictionaryString, object userData)
        {
            return m_LocalizationManager.ParseData(dictionaryString, userData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryBytes">要解析的字典二进制流。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(byte[] dictionaryBytes)
        {
            return m_LocalizationManager.ParseData(dictionaryBytes);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryBytes">要解析的字典二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(byte[] dictionaryBytes, object userData)
        {
            return m_LocalizationManager.ParseData(dictionaryBytes, userData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryBytes">要解析的字典二进制流。</param>
        /// <param name="startIndex">字典二进制流的起始位置。</param>
        /// <param name="length">字典二进制流的长度。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(byte[] dictionaryBytes, int startIndex, int length)
        {
            return m_LocalizationManager.ParseData(dictionaryBytes, startIndex, length);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryBytes">要解析的字典二进制流。</param>
        /// <param name="startIndex">字典二进制流的起始位置。</param>
        /// <param name="length">字典二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseData(byte[] dictionaryBytes, int startIndex, int length, object userData)
        {
            return m_LocalizationManager.ParseData(dictionaryBytes, startIndex, length, userData);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key)
        {
            return m_LocalizationManager.GetString(key);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0)
        {
            return m_LocalizationManager.GetString(key, arg0);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0, object arg1)
        {
            return m_LocalizationManager.GetString(key, arg0, arg1);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="arg0">字典参数 0。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, object arg0, object arg1, object arg2)
        {
            return m_LocalizationManager.GetString(key, arg0, arg1, arg2);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            return m_LocalizationManager.GetString(key, args);
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            return m_LocalizationManager.HasRawString(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            return m_LocalizationManager.GetRawString(key);
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            return m_LocalizationManager.RemoveRawString(key);
        }

        /// <summary>
        /// 清空所有字典。
        /// </summary>
        public void RemoveAllRawStrings()
        {
            m_LocalizationManager.RemoveAllRawStrings();
        }

        private void OnReadDataSuccess(object sender, ReadDataSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDictionarySuccessEventArgs.Create(e));
        }

        private void OnReadDataFailure(object sender, ReadDataFailureEventArgs e)
        {
            Log.Warning("Load dictionary failure, asset name '{0}', error message '{1}'.", e.DataAssetName, e.ErrorMessage);
            m_EventComponent.Fire(this, LoadDictionaryFailureEventArgs.Create(e));
        }

        private void OnReadDataUpdate(object sender, ReadDataUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDictionaryUpdateEventArgs.Create(e));
        }

        private void OnReadDataDependencyAsset(object sender, ReadDataDependencyAssetEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDictionaryDependencyAssetEventArgs.Create(e));
        }
    }
}
