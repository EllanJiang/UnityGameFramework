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
    /// 加载配置成功事件。
    /// </summary>
    public sealed class LoadConfigSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置成功事件编号的新实例。
        /// </summary>
        public LoadConfigSuccessEventArgs()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载配置成功事件编号。
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
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建加载配置成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置成功事件。</returns>
        public static LoadConfigSuccessEventArgs Create(GameFramework.Config.LoadConfigSuccessEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = ReferencePool.Acquire<LoadConfigSuccessEventArgs>();
            loadConfigSuccessEventArgs.ConfigName = loadConfigInfo.ConfigName;
            loadConfigSuccessEventArgs.ConfigAssetName = e.ConfigAssetName;
            loadConfigSuccessEventArgs.LoadType = e.LoadType;
            loadConfigSuccessEventArgs.Duration = e.Duration;
            loadConfigSuccessEventArgs.UserData = loadConfigInfo.UserData;
            ReferencePool.Release(loadConfigInfo);
            return loadConfigSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载配置成功事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }
    }
}
