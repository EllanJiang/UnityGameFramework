//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 下载代理辅助器基类。
    /// </summary>
    public abstract class DownloadAgentHelperBase : MonoBehaviour, IDownloadAgentHelper
    {
        /// <summary>
        /// 范围不适用错误码。
        /// </summary>
        protected const int RangeNotSatisfiableErrorCode = 416;

        /// <summary>
        /// 下载代理辅助器更新数据流事件。
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes;

        /// <summary>
        /// 下载代理辅助器更新数据大小事件。
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength;

        /// <summary>
        /// 下载代理辅助器完成事件。
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete;

        /// <summary>
        /// 下载代理辅助器错误事件。
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError;

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Download(string downloadUri, object userData);

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Download(string downloadUri, long fromPosition, object userData);

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="toPosition">下载数据结束位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Download(string downloadUri, long fromPosition, long toPosition, object userData);

        /// <summary>
        /// 重置下载代理辅助器。
        /// </summary>
        public abstract void Reset();
    }
}
