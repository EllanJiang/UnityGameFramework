//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源更新开始事件。
    /// </summary>
    public sealed class ResourceUpdateStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新开始事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateStartEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源更新开始事件的新实例。
        /// </summary>
        public ResourceUpdateStartEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
            RetryCount = 0;
        }

        /// <summary>
        /// 获取资源更新开始事件编号。
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
        /// 获取资源下载后存放路径。
        /// </summary>
        public string DownloadPath
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
        /// 获取当前下载大小。
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取压缩后大小。
        /// </summary>
        public int ZipLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取已重试下载次数。
        /// </summary>
        public int RetryCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源更新开始事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源更新开始事件。</returns>
        public static ResourceUpdateStartEventArgs Create(GameFramework.Resource.ResourceUpdateStartEventArgs e)
        {
            ResourceUpdateStartEventArgs resourceUpdateStartEventArgs = ReferencePool.Acquire<ResourceUpdateStartEventArgs>();
            resourceUpdateStartEventArgs.Name = e.Name;
            resourceUpdateStartEventArgs.DownloadPath = e.DownloadPath;
            resourceUpdateStartEventArgs.DownloadUri = e.DownloadUri;
            resourceUpdateStartEventArgs.CurrentLength = e.CurrentLength;
            resourceUpdateStartEventArgs.ZipLength = e.ZipLength;
            resourceUpdateStartEventArgs.RetryCount = e.RetryCount;
            return resourceUpdateStartEventArgs;
        }

        /// <summary>
        /// 清理资源更新开始事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            ZipLength = 0;
            RetryCount = 0;
        }
    }
}
