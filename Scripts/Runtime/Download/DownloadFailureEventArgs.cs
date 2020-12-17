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
    /// 下载失败事件。
    /// </summary>
    public sealed class DownloadFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 下载失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DownloadFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化下载失败事件的新实例。
        /// </summary>
        public DownloadFailureEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取下载失败事件编号。
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建下载失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的下载失败事件。</returns>
        public static DownloadFailureEventArgs Create(GameFramework.Download.DownloadFailureEventArgs e)
        {
            DownloadFailureEventArgs downloadFailureEventArgs = ReferencePool.Acquire<DownloadFailureEventArgs>();
            downloadFailureEventArgs.SerialId = e.SerialId;
            downloadFailureEventArgs.DownloadPath = e.DownloadPath;
            downloadFailureEventArgs.DownloadUri = e.DownloadUri;
            downloadFailureEventArgs.ErrorMessage = e.ErrorMessage;
            downloadFailureEventArgs.UserData = e.UserData;
            return downloadFailureEventArgs;
        }

        /// <summary>
        /// 清理下载失败事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
