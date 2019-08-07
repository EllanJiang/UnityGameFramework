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
    /// 资源更新失败事件。
    /// </summary>
    public sealed class ResourceUpdateFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源更新失败事件的新实例。
        /// </summary>
        public ResourceUpdateFailureEventArgs()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }

        /// <summary>
        /// 获取资源更新失败事件编号。
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
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取已重试次数。
        /// </summary>
        public int RetryCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取设定的重试次数。
        /// </summary>
        public int TotalRetryCount
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
        /// 创建资源更新失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源更新失败事件。</returns>
        public static ResourceUpdateFailureEventArgs Create(GameFramework.Resource.ResourceUpdateFailureEventArgs e)
        {
            ResourceUpdateFailureEventArgs resourceUpdateFailureEventArgs = ReferencePool.Acquire<ResourceUpdateFailureEventArgs>();
            resourceUpdateFailureEventArgs.Name = e.Name;
            resourceUpdateFailureEventArgs.DownloadUri = e.DownloadUri;
            resourceUpdateFailureEventArgs.RetryCount = e.RetryCount;
            resourceUpdateFailureEventArgs.TotalRetryCount = e.TotalRetryCount;
            resourceUpdateFailureEventArgs.ErrorMessage = e.ErrorMessage;
            return resourceUpdateFailureEventArgs;
        }

        /// <summary>
        /// 清理资源更新失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }
    }
}
