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
        private sealed class InputGyroscopeInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Gyroscope Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable", GUILayout.Height(30f)))
                        {
                            Input.gyro.enabled = true;
                        }
                        if (GUILayout.Button("Disable", GUILayout.Height(30f)))
                        {
                            Input.gyro.enabled = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Enabled", Input.gyro.enabled.ToString());
                    if (Input.gyro.enabled)
                    {
                        DrawItem("Update Interval", Input.gyro.updateInterval.ToString());
                        DrawItem("Attitude", Input.gyro.attitude.eulerAngles.ToString());
                        DrawItem("Gravity", Input.gyro.gravity.ToString());
                        DrawItem("Rotation Rate", Input.gyro.rotationRate.ToString());
                        DrawItem("Rotation Rate Unbiased", Input.gyro.rotationRateUnbiased.ToString());
                        DrawItem("User Acceleration", Input.gyro.userAcceleration.ToString());
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
