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
    /// 加载配置更新事件。
    /// </summary>
    public sealed class LoadConfigUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置更新事件的新实例。
        /// </summary>
        public LoadConfigUpdateEventArgs()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
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
        /// 获取配置加载方式。
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载配置进度。
        /// </summary>
        public float Progress
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
        /// 创建加载配置更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置更新事件。</returns>
        public static LoadConfigUpdateEventArgs Create(GameFramework.Config.LoadConfigUpdateEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            LoadConfigUpdateEventArgs loadConfigUpdateEventArgs = ReferencePool.Acquire<LoadConfigUpdateEventArgs>();
            loadConfigUpdateEventArgs.ConfigName = loadConfigInfo.ConfigName;
            loadConfigUpdateEventArgs.ConfigAssetName = e.ConfigAssetName;
            loadConfigUpdateEventArgs.LoadType = e.LoadType;
            loadConfigUpdateEventArgs.Progress = e.Progress;
            loadConfigUpdateEventArgs.UserData = loadConfigInfo.UserData;
            return loadConfigUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载配置更新事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
            UserData = null;
        }
    }
}
