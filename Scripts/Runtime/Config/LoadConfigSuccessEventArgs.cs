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
    /// 加载配置成功事件。
    /// </summary>
    public sealed class LoadConfigSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载配置成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadConfigSuccessEventArgs).GetHashCode();

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
        /// 清理加载配置成功事件。
        /// </summary>
        public override void Clear()
        {
            ConfigName = default(string);
            ConfigAssetName = default(string);
            Duration = default(float);
            UserData = default(object);
        }

        /// <summary>
        /// 填充加载配置成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>加载配置成功事件。</returns>
        public LoadConfigSuccessEventArgs Fill(GameFramework.Config.LoadConfigSuccessEventArgs e)
        {
            LoadConfigInfo loadConfigInfo = (LoadConfigInfo)e.UserData;
            ConfigName = loadConfigInfo.ConfigName;
            ConfigAssetName = e.ConfigAssetName;
            Duration = e.Duration;
            UserData = loadConfigInfo.UserData;

            return this;
        }
    }
}
