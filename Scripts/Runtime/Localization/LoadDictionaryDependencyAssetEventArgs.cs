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
    /// 加载字典时加载依赖资源事件。
    /// </summary>
    public sealed class LoadDictionaryDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载字典时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDictionaryDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载字典时加载依赖资源事件的新实例。
        /// </summary>
        public LoadDictionaryDependencyAssetEventArgs()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取加载字典时加载依赖资源事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取字典名称。
        /// </summary>
        public string DictionaryName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取字典资源名称。
        /// </summary>
        public string DictionaryAssetName
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
        /// 创建加载字典时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载字典时加载依赖资源事件。</returns>
        public static LoadDictionaryDependencyAssetEventArgs Create(GameFramework.Localization.LoadDictionaryDependencyAssetEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            LoadDictionaryDependencyAssetEventArgs loadDictionaryDependencyAssetEventArgs = ReferencePool.Acquire<LoadDictionaryDependencyAssetEventArgs>();
            loadDictionaryDependencyAssetEventArgs.DictionaryName = loadDictionaryInfo.DictionaryName;
            loadDictionaryDependencyAssetEventArgs.DictionaryAssetName = e.DictionaryAssetName;
            loadDictionaryDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            loadDictionaryDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            loadDictionaryDependencyAssetEventArgs.TotalCount = e.TotalCount;
            loadDictionaryDependencyAssetEventArgs.UserData = loadDictionaryInfo.UserData;
            return loadDictionaryDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载字典时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
