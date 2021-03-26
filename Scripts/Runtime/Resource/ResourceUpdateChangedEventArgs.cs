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
    /// 资源更新改变事件。
    /// </summary>
    public sealed class ResourceUpdateChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateChangedEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源更新改变事件的新实例。
        /// </summary>
        public ResourceUpdateChangedEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }

        /// <summary>
        /// 获取资源更新改变事件编号。
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
        public int CompressedLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源更新改变事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源更新改变事件。</returns>
        public static ResourceUpdateChangedEventArgs Create(GameFramework.Resource.ResourceUpdateChangedEventArgs e)
        {
            ResourceUpdateChangedEventArgs resourceUpdateChangedEventArgs = ReferencePool.Acquire<ResourceUpdateChangedEventArgs>();
            resourceUpdateChangedEventArgs.Name = e.Name;
            resourceUpdateChangedEventArgs.DownloadPath = e.DownloadPath;
            resourceUpdateChangedEventArgs.DownloadUri = e.DownloadUri;
            resourceUpdateChangedEventArgs.CurrentLength = e.CurrentLength;
            resourceUpdateChangedEventArgs.CompressedLength = e.CompressedLength;
            return resourceUpdateChangedEventArgs;
        }

        /// <summary>
        /// 清理资源更新改变事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }
    }
}
