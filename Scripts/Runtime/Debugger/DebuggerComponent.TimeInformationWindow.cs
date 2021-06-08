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
        private sealed class TimeInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Time Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Time Scale", Utility.Text.Format("{0} [{1}]", Time.timeScale, GetTimeScaleDescription(Time.timeScale)));
                    DrawItem("Realtime Since Startup", Time.realtimeSinceStartup.ToString());
                    DrawItem("Time Since Level Load", Time.timeSinceLevelLoad.ToString());
                    DrawItem("Time", Time.time.ToString());
                    DrawItem("Fixed Time", Time.fixedTime.ToString());
                    DrawItem("Unscaled Time", Time.unscaledTime.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Fixed Unscaled Time", Time.fixedUnscaledTime.ToString());
#endif
                    DrawItem("Delta Time", Time.deltaTime.ToString());
                    DrawItem("Fixed Delta Time", Time.fixedDeltaTime.ToString());
                    DrawItem("Unscaled Delta Time", Time.unscaledDeltaTime.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Fixed Unscaled Delta Time", Time.fixedUnscaledDeltaTime.ToString());
#endif
                    DrawItem("Smooth Delta Time", Time.smoothDeltaTime.ToString());
                    DrawItem("Maximum Delta Time", Time.maximumDeltaTime.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Maximum Particle Delta Time", Time.maximumParticleDeltaTime.ToString());
#endif
                    DrawItem("Frame Count", Time.frameCount.ToString());
                    DrawItem("Rendered Frame Count", Time.renderedFrameCount.ToString());
                    DrawItem("Capture Framerate", Time.captureFramerate.ToString());
#if UNITY_2019_2_OR_NEWER
                    DrawItem("Capture Delta Time", Time.captureDeltaTime.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("In Fixed Time Step", Time.inFixedTimeStep.ToString());
#endif
                }
                GUILayout.EndVertical();
            }

            private string GetTimeScaleDescription(float timeScale)
            {
                if (timeScale <= 0f)
                {
                    return "Pause";
                }

                if (timeScale < 1f)
                {
                    return "Slower";
                }

                if (timeScale > 1f)
                {
                    return "Faster";
                }

                return "Normal";
            }
        }
    }
}
