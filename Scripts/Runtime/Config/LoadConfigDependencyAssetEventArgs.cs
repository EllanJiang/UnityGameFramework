//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载配置时加载依赖资源事件。
    /// </summary>
    public sealed class LoadConfigDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置时加载依赖资源事件的新实例。
        /// </summary>
        public LoadConfigDependencyAssetEventArgs()
        {
            ConfigName = null;
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取加载配置失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取配置名称。
        /// </summary>
        public string ConfigName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置资源名称。
        /// </summary>
        public string ConfigAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载配置时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置时加载依赖资源事件。</returns>
        public static LoadConfigDependencyAssetEventArgs Create(GameFramework.Config.LoadConfigDependencyAssetEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            LoadConfigDependencyAssetEventArgs loadConfigDependencyAssetEventArgs = ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>();
            loadConfigDependencyAssetEventArgs.ConfigName = loadConfigInfo.ConfigName;
            loadConfigDependencyAssetEventArgs.ConfigAssetName = e.ConfigAssetName;
            loadConfigDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            loadConfigDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            loadConfigDependencyAssetEventArgs.TotalCount = e.TotalCount;
            loadConfigDependencyAssetEventArgs.UserData = loadConfigInfo.UserData;
            return loadConfigDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载配置时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = null;
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
