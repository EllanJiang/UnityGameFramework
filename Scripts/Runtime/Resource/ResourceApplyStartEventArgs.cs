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
    /// 资源应用开始事件。
    /// </summary>
    public sealed class ResourceApplyStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源应用开始事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceApplyStartEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源应用开始事件的新实例。
        /// </summary>
        public ResourceApplyStartEventArgs()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0L;
        }

        /// <summary>
        /// 获取资源应用开始事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
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
        /// 获取要应用资源的数量。
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要应用资源的总大小。
        /// </summary>
        public long TotalLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源应用开始事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源应用开始事件。</returns>
        public static ResourceApplyStartEventArgs Create(GameFramework.Resource.ResourceApplyStartEventArgs e)
        {
            ResourceApplyStartEventArgs resourceApplyStartEventArgs = ReferencePool.Acquire<ResourceApplyStartEventArgs>();
            resourceApplyStartEventArgs.ResourcePackPath = e.ResourcePackPath;
            resourceApplyStartEventArgs.Count = e.Count;
            resourceApplyStartEventArgs.TotalLength = e.TotalLength;
            return resourceApplyStartEventArgs;
        }

        /// <summary>
        /// 清理资源应用开始事件。
        /// </summary>
        public override void Clear()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0L;
        }
    }
}
