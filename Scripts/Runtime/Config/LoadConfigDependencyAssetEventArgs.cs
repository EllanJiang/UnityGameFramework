//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载全局配置时加载依赖资源事件。
    /// </summary>
    public sealed class LoadConfigDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载全局配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载全局配置时加载依赖资源事件的新实例。
        /// </summary>
        public LoadConfigDependencyAssetEventArgs()
        {
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取加载全局配置失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取全局配置资源名称。
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
        /// 创建加载全局配置时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载全局配置时加载依赖资源事件。</returns>
        public static LoadConfigDependencyAssetEventArgs Create(ReadDataDependencyAssetEventArgs e)
        {
            LoadConfigDependencyAssetEventArgs loadConfigDependencyAssetEventArgs = ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>();
            loadConfigDependencyAssetEventArgs.ConfigAssetName = e.DataAssetName;
            loadConfigDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            loadConfigDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            loadConfigDependencyAssetEventArgs.TotalCount = e.TotalCount;
            loadConfigDependencyAssetEventArgs.UserData = e.UserData;
            return loadConfigDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载全局配置时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
