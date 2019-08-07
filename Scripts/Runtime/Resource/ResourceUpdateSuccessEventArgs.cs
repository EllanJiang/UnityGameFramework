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
    /// 资源更新成功事件。
    /// </summary>
    public sealed class ResourceUpdateSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源更新成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceUpdateSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源更新成功事件的新实例。
        /// </summary>
        public ResourceUpdateSuccessEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            Length = 0;
            ZipLength = 0;
        }

        /// <summary>
        /// 获取资源更新成功事件编号。
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
        /// 获取资源大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取压缩包大小。
        /// </summary>
        public int ZipLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源更新成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源更新成功事件。</returns>
        public static ResourceUpdateSuccessEventArgs Create(GameFramework.Resource.ResourceUpdateSuccessEventArgs e)
        {
            ResourceUpdateSuccessEventArgs resourceUpdateSuccessEventArgs = ReferencePool.Acquire<ResourceUpdateSuccessEventArgs>();
            resourceUpdateSuccessEventArgs.Name = e.Name;
            resourceUpdateSuccessEventArgs.DownloadPath = e.DownloadPath;
            resourceUpdateSuccessEventArgs.DownloadUri = e.DownloadUri;
            resourceUpdateSuccessEventArgs.Length = e.Length;
            resourceUpdateSuccessEventArgs.ZipLength = e.ZipLength;
            return resourceUpdateSuccessEventArgs;
        }

        /// <summary>
        /// 清理资源更新成功事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            Length = 0;
            ZipLength = 0;
        }
    }
}
