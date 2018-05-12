//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Config;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 配置辅助器基类。
    /// </summary>
    public abstract class ConfigHelperBase : MonoBehaviour, IConfigHelper
    {
        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configAsset">配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        public bool LoadConfig(object configAsset, object userData)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)userData;
            return LoadConfig(loadConfigInfo.ConfigName, configAsset, loadConfigInfo.UserData);
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public abstract bool ParseConfig(string text, object userData);

        /// <summary>
        /// 释放配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的配置资源。</param>
        public abstract void ReleaseConfigAsset(object configAsset);

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAsset">配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        protected abstract bool LoadConfig(string configName, object configAsset, object userData);
    }
}
