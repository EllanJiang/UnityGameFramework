//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

#if !UNITY_2018_3_OR_NEWER

using GameFramework;
using GameFramework.Download;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// WWW 下载代理辅助器。
    /// </summary>
    public class WWWDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private WWW m_WWW = null;
        private int m_LastDownloadedSize = 0;
        private bool m_Disposed = false;

        private EventHandler<DownloadAgentHelperUpdateBytesEventArgs> m_DownloadAgentHelperUpdateBytesEventHandler = null;
        private EventHandler<DownloadAgentHelperUpdateLengthEventArgs> m_DownloadAgentHelperUpdateLengthEventHandler = null;
        private EventHandler<DownloadAgentHelperCompleteEventArgs> m_DownloadAgentHelperCompleteEventHandler = null;
        private EventHandler<DownloadAgentHelperErrorEventArgs> m_DownloadAgentHelperErrorEventHandler = null;

        /// <summary>
        /// 下载代理辅助器更新数据流事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes
        {
            add
            {
                m_DownloadAgentHelperUpdateBytesEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperUpdateBytesEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载代理辅助器更新数据大小事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength
        {
            add
            {
                m_DownloadAgentHelperUpdateLengthEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperUpdateLengthEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete
        {
            add
            {
                m_DownloadAgentHelperCompleteEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError
        {
            add
            {
                m_DownloadAgentHelperErrorEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, object userData)
        {
            if (m_DownloadAgentHelperUpdateBytesEventHandler == null || m_DownloadAgentHelperUpdateLengthEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            m_WWW = new WWW(downloadUri);
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, long fromPosition, object userData)
        {
            if (m_DownloadAgentHelperUpdateBytesEventHandler == null || m_DownloadAgentHelperUpdateLengthEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            Dictionary<string, string> header = new Dictionary<string, string>
            {
                { "Range", Utility.Text.Format("bytes={0}-", fromPosition) }
            };

            m_WWW = new WWW(downloadUri, null, header);
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="toPosition">下载数据结束位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, long fromPosition, long toPosition, object userData)
        {
            if (m_DownloadAgentHelperUpdateBytesEventHandler == null || m_DownloadAgentHelperUpdateLengthEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            Dictionary<string, string> header = new Dictionary<string, string>
            {
                { "Range", Utility.Text.Format("bytes={0}-{1}", fromPosition, toPosition) }
            };

            m_WWW = new WWW(downloadUri, null, header);
        }

        /// <summary>
        /// 重置下载代理辅助器。
        /// </summary>
        public override void Reset()
        {
            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }

            m_LastDownloadedSize = 0;
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_WWW != null)
                {
                    m_WWW.Dispose();
                    m_WWW = null;
                }
            }

            m_Disposed = true;
        }

        private void Update()
        {
            if (m_WWW == null)
            {
                return;
            }

            int deltaLength = m_WWW.bytesDownloaded - m_LastDownloadedSize;
            if (deltaLength > 0)
            {
                m_LastDownloadedSize = m_WWW.bytesDownloaded;
                DownloadAgentHelperUpdateLengthEventArgs downloadAgentHelperUpdateLengthEventArgs = DownloadAgentHelperUpdateLengthEventArgs.Create(deltaLength);
                m_DownloadAgentHelperUpdateLengthEventHandler(this, downloadAgentHelperUpdateLengthEventArgs);
                ReferencePool.Release(downloadAgentHelperUpdateLengthEventArgs);
            }

            if (m_WWW == null)
            {
                return;
            }

            if (!m_WWW.isDone)
            {
                return;
            }

            if (!string.IsNullOrEmpty(m_WWW.error))
            {
                DownloadAgentHelperErrorEventArgs dodwnloadAgentHelperErrorEventArgs = DownloadAgentHelperErrorEventArgs.Create(m_WWW.error.StartsWith(RangeNotSatisfiableErrorCode.ToString(), StringComparison.Ordinal), m_WWW.error);
                m_DownloadAgentHelperErrorEventHandler(this, dodwnloadAgentHelperErrorEventArgs);
                ReferencePool.Release(dodwnloadAgentHelperErrorEventArgs);
            }
            else
            {
                byte[] bytes = m_WWW.bytes;
                DownloadAgentHelperUpdateBytesEventArgs downloadAgentHelperUpdateBytesEventArgs = DownloadAgentHelperUpdateBytesEventArgs.Create(bytes, 0, bytes.Length);
                m_DownloadAgentHelperUpdateBytesEventHandler(this, downloadAgentHelperUpdateBytesEventArgs);
                ReferencePool.Release(downloadAgentHelperUpdateBytesEventArgs);

                DownloadAgentHelperCompleteEventArgs downloadAgentHelperCompleteEventArgs = DownloadAgentHelperCompleteEventArgs.Create(bytes.LongLength);
                m_DownloadAgentHelperCompleteEventHandler(this, downloadAgentHelperCompleteEventArgs);
                ReferencePool.Release(downloadAgentHelperCompleteEventArgs);
            }
        }
    }
}

#endif
