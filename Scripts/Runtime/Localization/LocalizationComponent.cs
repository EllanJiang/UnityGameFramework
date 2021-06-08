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
        /// <typeparam name="T">字典参数的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T>(string key, T arg)
        {
            return m_LocalizationManager.GetString(key, arg);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2>(string key, T1 arg1, T2 arg2)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字典参数 15 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <param name="arg15">字典参数 15。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <typeparam name="T1">字典参数 1 的类型。</typeparam>
        /// <typeparam name="T2">字典参数 2 的类型。</typeparam>
        /// <typeparam name="T3">字典参数 3 的类型。</typeparam>
        /// <typeparam name="T4">字典参数 4 的类型。</typeparam>
        /// <typeparam name="T5">字典参数 5 的类型。</typeparam>
        /// <typeparam name="T6">字典参数 6 的类型。</typeparam>
        /// <typeparam name="T7">字典参数 7 的类型。</typeparam>
        /// <typeparam name="T8">字典参数 8 的类型。</typeparam>
        /// <typeparam name="T9">字典参数 9 的类型。</typeparam>
        /// <typeparam name="T10">字典参数 10 的类型。</typeparam>
        /// <typeparam name="T11">字典参数 11 的类型。</typeparam>
        /// <typeparam name="T12">字典参数 12 的类型。</typeparam>
        /// <typeparam name="T13">字典参数 13 的类型。</typeparam>
        /// <typeparam name="T14">字典参数 14 的类型。</typeparam>
        /// <typeparam name="T15">字典参数 15 的类型。</typeparam>
        /// <typeparam name="T16">字典参数 16 的类型。</typeparam>
        /// <param name="key">字典主键。</param>
        /// <param name="arg1">字典参数 1。</param>
        /// <param name="arg2">字典参数 2。</param>
        /// <param name="arg3">字典参数 3。</param>
        /// <param name="arg4">字典参数 4。</param>
        /// <param name="arg5">字典参数 5。</param>
        /// <param name="arg6">字典参数 6。</param>
        /// <param name="arg7">字典参数 7。</param>
        /// <param name="arg8">字典参数 8。</param>
        /// <param name="arg9">字典参数 9。</param>
        /// <param name="arg10">字典参数 10。</param>
        /// <param name="arg11">字典参数 11。</param>
        /// <param name="arg12">字典参数 12。</param>
        /// <param name="arg13">字典参数 13。</param>
        /// <param name="arg14">字典参数 14。</param>
        /// <param name="arg15">字典参数 15。</param>
        /// <param name="arg16">字典参数 16。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return m_LocalizationManager.GetString(key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
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
