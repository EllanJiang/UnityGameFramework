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
    /// 加载场景时加载依赖资源事件。
    /// </summary>
    public sealed class LoadSceneDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载场景时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadSceneDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载场景时加载依赖资源事件的新实例。
        /// </summary>
        public LoadSceneDependencyAssetEventArgs()
        {
            SceneAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取加载场景时加载依赖资源事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
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
        /// 创建加载场景时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载场景时加载依赖资源事件。</returns>
        public static LoadSceneDependencyAssetEventArgs Create(GameFramework.Scene.LoadSceneDependencyAssetEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs loadSceneDependencyAssetEventArgs = ReferencePool.Acquire<LoadSceneDependencyAssetEventArgs>();
            loadSceneDependencyAssetEventArgs.SceneAssetName = e.SceneAssetName;
            loadSceneDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            loadSceneDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            loadSceneDependencyAssetEventArgs.TotalCount = e.TotalCount;
            loadSceneDependencyAssetEventArgs.UserData = e.UserData;
            return loadSceneDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载场景时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
