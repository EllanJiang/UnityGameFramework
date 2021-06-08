//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class SystemInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>System Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Device Unique ID", SystemInfo.deviceUniqueIdentifier);
                    DrawItem("Device Name", SystemInfo.deviceName);
                    DrawItem("Device Type", SystemInfo.deviceType.ToString());
                    DrawItem("Device Model", SystemInfo.deviceModel);
                    DrawItem("Processor Type", SystemInfo.processorType);
                    DrawItem("Processor Count", SystemInfo.processorCount.ToString());
                    DrawItem("Processor Frequency", Utility.Text.Format("{0} MHz", SystemInfo.processorFrequency));
                    DrawItem("System Memory Size", Utility.Text.Format("{0} MB", SystemInfo.systemMemorySize));
#if UNITY_5_5_OR_NEWER
                    DrawItem("Operating System Family", SystemInfo.operatingSystemFamily.ToString());
#endif
                    DrawItem("Operating System", SystemInfo.operatingSystem);
#if UNITY_5_6_OR_NEWER
                    DrawItem("Battery Status", SystemInfo.batteryStatus.ToString());
                    DrawItem("Battery Level", GetBatteryLevelString(SystemInfo.batteryLevel));
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Supports Audio", SystemInfo.supportsAudio.ToString());
#endif
                    DrawItem("Supports Location Service", SystemInfo.supportsLocationService.ToString());
                    DrawItem("Supports Accelerometer", SystemInfo.supportsAccelerometer.ToString());
                    DrawItem("Supports Gyroscope", SystemInfo.supportsGyroscope.ToString());
                    DrawItem("Supports Vibration", SystemInfo.supportsVibration.ToString());
                    DrawItem("Genuine", Application.genuine.ToString());
                    DrawItem("Genuine Check Available", Application.genuineCheckAvailable.ToString());
                }
                GUILayout.EndVertical();
            }

            private string GetBatteryLevelString(float batteryLevel)
            {
                if (batteryLevel < 0f)
                {
                    return "Unavailable";
                }

                return batteryLevel.ToString("P0");
            }
        }
    }
}
