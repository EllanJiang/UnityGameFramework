//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif

namespace UnityGameFramework.Runtime
{
    public partial class UnityWebRequestDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private sealed class DownloadHandler : DownloadHandlerScript
        {
            private readonly UnityWebRequestDownloadAgentHelper m_Owner;

            public DownloadHandler(UnityWebRequestDownloadAgentHelper owner)
                : base(owner.m_DownloadCache)
            {
                m_Owner = owner;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (m_Owner != null && dataLength > 0)
                {
                    m_Owner.m_DownloadAgentHelperUpdateBytesEventHandler(this, new DownloadAgentHelperUpdateBytesEventArgs(data, 0, dataLength));
                    m_Owner.m_DownloadAgentHelperUpdateLengthEventHandler(this, new DownloadAgentHelperUpdateLengthEventArgs(dataLength));
                }

                return base.ReceiveData(data, dataLength);
            }
        }
    }
}
