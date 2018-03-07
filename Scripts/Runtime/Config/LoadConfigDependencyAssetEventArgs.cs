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
    /// 加载配置时加载依赖资源事件。
    /// </summary>
    public sealed class LoadConfigDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigDependencyAssetEventArgs).GetHashCode();

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
        /// 清理加载配置时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = default(string);
            ConfigAssetName = default(string);
            DependencyAssetName = default(string);
            LoadedCount = default(int);
            TotalCount = default(int);
            UserData = default(object);
        }

        /// <summary>
        /// 填充加载配置时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>加载配置时加载依赖资源事件。</returns>
        public LoadConfigDependencyAssetEventArgs Fill(GameFramework.Config.LoadConfigDependencyAssetEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            ConfigName = loadConfigInfo.ConfigName;
            ConfigAssetName = e.ConfigAssetName;
            DependencyAssetName = e.DependencyAssetName;
            LoadedCount = e.LoadedCount;
            TotalCount = e.TotalCount;
            UserData = loadConfigInfo.UserData;

            return this;
        }
    }
}
