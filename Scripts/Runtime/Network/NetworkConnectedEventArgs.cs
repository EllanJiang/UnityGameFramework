//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Network;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 网络连接成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        /// <summary>
        /// 初始化网络连接成功事件的新实例。
        /// </summary>
        public NetworkConnectedEventArgs()
        {
            NetworkChannel = null;
            UserData = null;
        }

        /// <summary>
        /// 获取网络连接成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
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
        /// 创建网络连接成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的网络连接成功事件。</returns>
        public static NetworkConnectedEventArgs Create(GameFramework.Network.NetworkConnectedEventArgs e)
        {
            NetworkConnectedEventArgs networkConnectedEventArgs = ReferencePool.Acquire<NetworkConnectedEventArgs>();
            networkConnectedEventArgs.NetworkChannel = e.NetworkChannel;
            networkConnectedEventArgs.UserData = e.UserData;
            return networkConnectedEventArgs;
        }

        /// <summary>
        /// 清理网络连接成功事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            UserData = null;
        }
    }
}
