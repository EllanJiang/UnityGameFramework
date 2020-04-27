//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 本地化辅助器基类。
    /// </summary>
    public abstract class LocalizationHelperBase : MonoBehaviour, ILocalizationHelper
    {
        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public abstract Language SystemLanguage
        {
            get;
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="dictionaryObject">字典对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadDictionary(string dictionaryAssetName, object dictionaryObject, object userData)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)userData;
            return LoadDictionary(loadDictionaryInfo.DictionaryName, dictionaryAssetName, dictionaryObject, loadDictionaryInfo.UserData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryData">要解析的字典数据。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public abstract bool ParseDictionary(object dictionaryData, object userData);

        /// <summary>
        /// 释放字典资源。
        /// </summary>
        /// <param name="dictionaryAsset">要释放的字典资源。</param>
        public abstract void ReleaseDictionaryAsset(object dictionaryAsset);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryName">字典名称。</param>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="dictionaryObject">字典对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadDictionary(string dictionaryName, string dictionaryAssetName, object dictionaryObject, object userData);
    }
}
