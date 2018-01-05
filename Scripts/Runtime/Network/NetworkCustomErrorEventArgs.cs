//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Network;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 用户自定义网络错误事件。
    /// </summary>
    public sealed class NetworkCustomErrorEventArgs : GameEventArgs
    {
        /// <summary>
        /// 用户自定义网络错误事件编号。
        /// </summary>
        public static readonly int EventId = typeof(NetworkCustomErrorEventArgs).GetHashCode();

        /// <summary>
        /// 获取用户自定义网络错误事件编号。
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
        /// 获取用户自定义错误数据。
        /// </summary>
        public object CustomErrorData
        {
            get;
            private set;
        }

        /// <summary>
        /// 清理用户自定义网络错误事件。
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = default(INetworkChannel);
            CustomErrorData = default(object);
        }

        /// <summary>
        /// 填充用户自定义网络错误事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>用户自定义网络错误事件。</returns>
        public NetworkCustomErrorEventArgs Fill(GameFramework.Network.NetworkCustomErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            CustomErrorData = e.CustomErrorData;

            return this;
        }
    }
}
