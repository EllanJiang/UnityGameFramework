//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class PathInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Path Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Data Path", Application.dataPath);
                    DrawItem("Persistent Data Path", Application.persistentDataPath);
                    DrawItem("Streaming Assets Path", Application.streamingAssetsPath);
                    DrawItem("Temporary Cache Path", Application.temporaryCachePath);
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Console Log Path", Application.consoleLogPath);
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
