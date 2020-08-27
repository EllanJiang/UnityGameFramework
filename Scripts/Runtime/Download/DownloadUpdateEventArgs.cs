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
    /// 下载更新事件。
    /// </summary>
    public sealed class DownloadUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 下载更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DownloadUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化下载更新事件的新实例。
        /// </summary>
        public DownloadUpdateEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取下载更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取下载任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载后存放路径。
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
        /// 获取当前大小。
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建下载更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的下载更新事件。</returns>
        public static DownloadUpdateEventArgs Create(GameFramework.Download.DownloadUpdateEventArgs e)
        {
            DownloadUpdateEventArgs downloadUpdateEventArgs = ReferencePool.Acquire<DownloadUpdateEventArgs>();
            downloadUpdateEventArgs.SerialId = e.SerialId;
            downloadUpdateEventArgs.DownloadPath = e.DownloadPath;
            downloadUpdateEventArgs.DownloadUri = e.DownloadUri;
            downloadUpdateEventArgs.CurrentLength = e.CurrentLength;
            downloadUpdateEventArgs.UserData = e.UserData;
            return downloadUpdateEventArgs;
        }

        /// <summary>
        /// 清理下载更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            UserData = null;
        }
    }
}
