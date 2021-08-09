//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源校验成功事件。
    /// </summary>
    public sealed class ResourceVerifySuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源校验成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceVerifySuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源校验成功事件的新实例。
        /// </summary>
        public ResourceVerifySuccessEventArgs()
        {
            Name = null;
            Length = 0;
        }

        /// <summary>
        /// 获取资源校验成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源校验成功事件。</returns>
        public static ResourceVerifySuccessEventArgs Create(GameFramework.Resource.ResourceVerifySuccessEventArgs e)
        {
            ResourceVerifySuccessEventArgs resourceVerifySuccessEventArgs = ReferencePool.Acquire<ResourceVerifySuccessEventArgs>();
            resourceVerifySuccessEventArgs.Name = e.Name;
            resourceVerifySuccessEventArgs.Length = e.Length;
            return resourceVerifySuccessEventArgs;
        }

        /// <summary>
        /// 清理资源校验成功事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            Length = 0;
        }
    }
}
