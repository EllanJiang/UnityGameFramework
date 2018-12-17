//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Localization;
using System.IO;
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
        /// <param name="dictionaryAsset">字典资源。</param>
        /// <param name="loadType">字典加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadDictionary(object dictionaryAsset, LoadType loadType, object userData)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)userData;
            return LoadDictionary(loadDictionaryInfo.DictionaryName, dictionaryAsset, loadType, loadDictionaryInfo.UserData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public abstract bool ParseDictionary(string text, object userData);

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="bytes">要解析的字典二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public abstract bool ParseDictionary(byte[] bytes, object userData);

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="stream">要解析的字典二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public abstract bool ParseDictionary(Stream stream, object userData);

        /// <summary>
        /// 释放字典资源。
        /// </summary>
        /// <param name="dictionaryAsset">要释放的字典资源。</param>
        public abstract void ReleaseDictionaryAsset(object dictionaryAsset);

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryName">字典名称。</param>
        /// <param name="dictionaryAsset">字典资源。</param>
        /// <param name="loadType">字典加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadDictionary(string dictionaryName, object dictionaryAsset, LoadType loadType, object userData);
    }
}
