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
    /// 加载配置失败事件。
    /// </summary>
    public sealed class LoadConfigFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载配置失败事件的新实例。
        /// </summary>
        public LoadConfigFailureEventArgs()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建加载配置失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载配置失败事件。</returns>
        public static LoadConfigFailureEventArgs Create(GameFramework.Config.LoadConfigFailureEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            LoadConfigFailureEventArgs loadConfigFailureEventArgs = ReferencePool.Acquire<LoadConfigFailureEventArgs>();
            loadConfigFailureEventArgs.ConfigName = loadConfigInfo.ConfigName;
            loadConfigFailureEventArgs.ConfigAssetName = e.ConfigAssetName;
            loadConfigFailureEventArgs.LoadType = e.LoadType;
            loadConfigFailureEventArgs.ErrorMessage = e.ErrorMessage;
            loadConfigFailureEventArgs.UserData = loadConfigInfo.UserData;
            ReferencePool.Release(loadConfigInfo);
            return loadConfigFailureEventArgs;
        }

        /// <summary>
        /// 清理加载配置失败事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = null;
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
