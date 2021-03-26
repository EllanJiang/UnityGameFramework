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
    /// 资源应用失败事件。
    /// </summary>
    public sealed class ResourceApplyFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源应用失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceApplyFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源应用失败事件的新实例。
        /// </summary>
        public ResourceApplyFailureEventArgs()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
        }

        /// <summary>
        /// 获取资源应用失败事件编号。
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
        /// 获取资源包路径。
        /// </summary>
        public string ResourcePackPath
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
        /// 创建资源应用失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源应用失败事件。</returns>
        public static ResourceApplyFailureEventArgs Create(GameFramework.Resource.ResourceApplyFailureEventArgs e)
        {
            ResourceApplyFailureEventArgs resourceApplyFailureEventArgs = ReferencePool.Acquire<ResourceApplyFailureEventArgs>();
            resourceApplyFailureEventArgs.Name = e.Name;
            resourceApplyFailureEventArgs.ResourcePackPath = e.ResourcePackPath;
            resourceApplyFailureEventArgs.ErrorMessage = e.ErrorMessage;
            return resourceApplyFailureEventArgs;
        }

        /// <summary>
        /// 清理资源应用失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
        }
    }
}
