//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class WebPlayerInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Web Player Information</b>");
                GUILayout.BeginVertical("box");
                {
#if !UNITY_2017_2_OR_NEWER
                    DrawItem("Is Web Player:", Application.isWebPlayer.ToString());
#endif
                    DrawItem("Absolute URL:", Application.absoluteURL);
#if !UNITY_2017_2_OR_NEWER
                    DrawItem("Source Value:", Application.srcValue);
#endif
#if !UNITY_2018_2_OR_NEWER
                    DrawItem("Streamed Bytes:", Application.streamedBytes.ToString());
#endif
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Web Security Enabled:", Application.webSecurityEnabled.ToString());
                    DrawItem("Web Security Host URL:", Application.webSecurityHostUrl.ToString());
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
