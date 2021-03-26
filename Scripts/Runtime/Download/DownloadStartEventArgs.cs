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
    /// 下载开始事件。
    /// </summary>
    public sealed class DownloadStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// 下载开始事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DownloadStartEventArgs).GetHashCode();

        /// <summary>
        /// 初始化下载开始事件的新实例。
        /// </summary>
        public DownloadStartEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

        /// <summary>
        /// 获取下载开始事件编号。
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
        public long CurrentLength
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
        /// 创建下载开始事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的下载开始事件。</returns>
        public static DownloadStartEventArgs Create(GameFramework.Download.DownloadStartEventArgs e)
        {
            DownloadStartEventArgs downloadStartEventArgs = ReferencePool.Acquire<DownloadStartEventArgs>();
            downloadStartEventArgs.SerialId = e.SerialId;
            downloadStartEventArgs.DownloadPath = e.DownloadPath;
            downloadStartEventArgs.DownloadUri = e.DownloadUri;
            downloadStartEventArgs.CurrentLength = e.CurrentLength;
            downloadStartEventArgs.UserData = e.UserData;
            return downloadStartEventArgs;
        }

        /// <summary>
        /// 清理下载开始事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }
    }
}
