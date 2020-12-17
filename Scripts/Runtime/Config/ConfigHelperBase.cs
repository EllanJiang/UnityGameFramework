//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Config;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 全局配置辅助器基类。
    /// </summary>
    public abstract class ConfigHelperBase : MonoBehaviour, IDataProviderHelper<IConfigManager>, IConfigHelper
    {
        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取全局配置成功。</returns>
        public abstract bool ReadData(IConfigManager configManager, string configAssetName, object configAsset, object userData);

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取全局配置成功。</returns>
        public abstract bool ReadData(IConfigManager configManager, string configAssetName, byte[] configBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public abstract bool ParseData(IConfigManager configManager, string configString, object userData);

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public abstract bool ParseData(IConfigManager configManager, byte[] configBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        public abstract void ReleaseDataAsset(IConfigManager configManager, object configAsset);
    }
}
