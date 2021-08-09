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
    /// 资源校验失败事件。
    /// </summary>
    public sealed class ResourceVerifyFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源校验失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceVerifyFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源校验失败事件的新实例。
        /// </summary>
        public ResourceVerifyFailureEventArgs()
        {
            Name = null;
        }

        /// <summary>
        /// 获取资源校验失败事件编号。
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
        /// 创建资源校验失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源校验失败事件。</returns>
        public static ResourceVerifyFailureEventArgs Create(GameFramework.Resource.ResourceVerifyFailureEventArgs e)
        {
            ResourceVerifyFailureEventArgs resourceVerifyFailureEventArgs = ReferencePool.Acquire<ResourceVerifyFailureEventArgs>();
            resourceVerifyFailureEventArgs.Name = e.Name;
            return resourceVerifyFailureEventArgs;
        }

        /// <summary>
        /// 清理资源校验失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
        }
    }
}
