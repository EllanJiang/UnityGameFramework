//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Network;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class NetworkInformationWindow : ScrollableDebuggerWindowBase
        {
            private NetworkComponent m_NetworkComponent = null;

            public override void Initialize(params object[] args)
            {
                m_NetworkComponent = GameEntry.GetComponent<NetworkComponent>();
                if (m_NetworkComponent == null)
                {
                    Log.Fatal("Network component is invalid.");
                    return;
                }
            }

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Network Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Network Channel Count", m_NetworkComponent.NetworkChannelCount.ToString());
                }
                GUILayout.EndVertical();
                INetworkChannel[] networkChannels = m_NetworkComponent.GetAllNetworkChannels();
                for (int i = 0; i < networkChannels.Length; i++)
                {
                    DrawNetworkChannel(networkChannels[i]);
                }
            }

            private void DrawNetworkChannel(INetworkChannel networkChannel)
            {
                GUILayout.Label(Utility.Text.Format("<b>Network Channel: {0} ({1})</b>", networkChannel.Name, networkChannel.Connected ? "Connected" : "Disconnected"));
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Service Type", networkChannel.ServiceType.ToString());
                    DrawItem("Address Family", networkChannel.AddressFamily.ToString());
                    DrawItem("Local Address", networkChannel.Connected ? networkChannel.Socket.LocalEndPoint.ToString() : "Unavailable");
                    DrawItem("Remote Address", networkChannel.Connected ? networkChannel.Socket.RemoteEndPoint.ToString() : "Unavailable");
                    DrawItem("Send Packet", Utility.Text.Format("{0} / {1}", networkChannel.SendPacketCount, networkChannel.SentPacketCount));
                    DrawItem("Receive Packet", Utility.Text.Format("{0} / {1}", networkChannel.ReceivePacketCount, networkChannel.ReceivedPacketCount));
                    DrawItem("Miss Heart Beat Count", networkChannel.MissHeartBeatCount.ToString());
                    DrawItem("Heart Beat", Utility.Text.Format("{0:F2} / {1:F2}", networkChannel.HeartBeatElapseSeconds, networkChannel.HeartBeatInterval));
                    if (networkChannel.Connected)
                    {
                        if (GUILayout.Button("Disconnect", GUILayout.Height(30f)))
                        {
                            networkChannel.Close();
                        }
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
