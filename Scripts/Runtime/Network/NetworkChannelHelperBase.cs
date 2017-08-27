//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Network;
using System.IO;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 网络频道辅助器基类。
    /// </summary>
    public abstract class NetworkChannelHelperBase : MonoBehaviour, INetworkChannelHelper
    {
        /// <summary>
        /// 获取消息包头长度。
        /// </summary>
        public abstract int PacketHeaderLength
        {
            get;
        }

        /// <summary>
        /// 发送心跳消息包。
        /// </summary>
        /// <returns>是否发送心跳消息包成功。</returns>
        public abstract bool SendHeartBeat();

        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <returns>序列化后的消息包字节流。</returns>
        public abstract byte[] Serialize<T>(T packet) where T : Packet;

        /// <summary>
        /// 反序列消息包头。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns></returns>
        public abstract PacketHeader DeserializePacketHeader(Stream source, out object customErrorData);

        /// <summary>
        /// 反序列化消息包。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的消息包。</returns>
        public abstract Packet DeserializePacket(Stream source, out object customErrorData);
    }
}
