//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

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
        /// 清理加载字典时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryName = default(string);
            DictionaryAssetName = default(string);
            DependencyAssetName = default(string);
            LoadedCount = default(int);
            TotalCount = default(int);
            UserData = default(object);
        }

        /// <summary>
        /// 填充加载字典时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>加载字典时加载依赖资源事件。</returns>
        public LoadDictionaryDependencyAssetEventArgs Fill(GameFramework.Localization.LoadDictionaryDependencyAssetEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            DictionaryName = loadDictionaryInfo.DictionaryName;
            DictionaryAssetName = e.DictionaryAssetName;
            DependencyAssetName = e.DependencyAssetName;
            LoadedCount = e.LoadedCount;
            TotalCount = e.TotalCount;
            UserData = loadDictionaryInfo.UserData;

            return this;
        }
    }
}
