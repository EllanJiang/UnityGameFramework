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
    /// 下载成功事件。
    /// </summary>
    public sealed class DownloadSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 下载成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DownloadSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化下载成功事件的新实例。
        /// </summary>
        public DownloadSuccessEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

        /// <summary>
        /// 获取下载成功事件编号。
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
        /// 创建下载成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的下载成功事件。</returns>
        public static DownloadSuccessEventArgs Create(GameFramework.Download.DownloadSuccessEventArgs e)
        {
            DownloadSuccessEventArgs downloadSuccessEventArgs = ReferencePool.Acquire<DownloadSuccessEventArgs>();
            downloadSuccessEventArgs.SerialId = e.SerialId;
            downloadSuccessEventArgs.DownloadPath = e.DownloadPath;
            downloadSuccessEventArgs.DownloadUri = e.DownloadUri;
            downloadSuccessEventArgs.CurrentLength = e.CurrentLength;
            downloadSuccessEventArgs.UserData = e.UserData;
            return downloadSuccessEventArgs;
        }

        /// <summary>
        /// 清理下载成功事件。
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
