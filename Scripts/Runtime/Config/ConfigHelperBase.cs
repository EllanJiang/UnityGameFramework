//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Config;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 全局配置辅助器基类。
    /// </summary>
    public abstract class ConfigHelperBase : MonoBehaviour, IConfigHelper
    {
        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadConfig(string configAssetName, object configAsset, object userData)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)userData;
            return LoadConfig(loadConfigInfo.ConfigName, configAssetName, configAsset, loadConfigInfo.UserData);
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制数据。</param>
        /// <param name="startIndex">全局配置二进制数据的起始位置。</param>
        /// <param name="length">全局配置二进制数据的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadConfig(string configAssetName, byte[] configBytes, int startIndex, int length, object userData)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)userData;
            return LoadConfig(loadConfigInfo.ConfigName, configAssetName, configBytes, startIndex, length, loadConfigInfo.UserData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public abstract bool ParseConfig(string configString, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制数据。</param>
        /// <param name="startIndex">全局配置二进制数据的起始位置。</param>
        /// <param name="length">全局配置二进制数据的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public abstract bool ParseConfig(byte[] configBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        public abstract void ReleaseConfigAsset(object configAsset);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configName">全局配置名称。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadConfig(string configName, string configAssetName, object configAsset, object userData);

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configName">全局配置名称。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制数据。</param>
        /// <param name="startIndex">全局配置二进制数据的起始位置。</param>
        /// <param name="length">全局配置二进制数据的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadConfig(string configName, string configAssetName, byte[] configBytes, int startIndex, int length, object userData);
    }
}
