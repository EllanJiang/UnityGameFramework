//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class InputLocationInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Location Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable", GUILayout.Height(30f)))
                        {
                            Input.location.Start();
                        }
                        if (GUILayout.Button("Disable", GUILayout.Height(30f)))
                        {
                            Input.location.Stop();
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Is Enabled By User", Input.location.isEnabledByUser.ToString());
                    DrawItem("Status", Input.location.status.ToString());
                    if (Input.location.status == LocationServiceStatus.Running)
                    {
                        DrawItem("Horizontal Accuracy", Input.location.lastData.horizontalAccuracy.ToString());
                        DrawItem("Vertical Accuracy", Input.location.lastData.verticalAccuracy.ToString());
                        DrawItem("Longitude", Input.location.lastData.longitude.ToString());
                        DrawItem("Latitude", Input.location.lastData.latitude.ToString());
                        DrawItem("Altitude", Input.location.lastData.altitude.ToString());
                        DrawItem("Timestamp", Input.location.lastData.timestamp.ToString());
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
