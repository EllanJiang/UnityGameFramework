//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class ProfilerInformationWindow : ScrollableDebuggerWindowBase
        {
            private const int OneMegaBytes = 1024 * 1024;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Profiler Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Supported:", Profiler.supported.ToString());
                    DrawItem("Enabled:", Profiler.enabled.ToString());
                    DrawItem("Enable Binary Log:", Profiler.enableBinaryLog ? Utility.Text.Format("True, {0}", Profiler.logFile) : "False");
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Area Count:", Profiler.areaCount.ToString());
#endif
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Max Samples Number Per Frame:", Profiler.maxNumberOfSamplesPerFrame.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Max Used Memory:", Profiler.maxUsedMemory.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Mono Used Size:", Utility.Text.Format("{0} MB", (Profiler.GetMonoUsedSizeLong() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Mono Heap Size:", Utility.Text.Format("{0} MB", (Profiler.GetMonoHeapSizeLong() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Used Heap Size:", Utility.Text.Format("{0} MB", (Profiler.usedHeapSizeLong / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Allocated Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalAllocatedMemoryLong() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Reserved Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalReservedMemoryLong() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Unused Reserved Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalUnusedReservedMemoryLong() / (float)OneMegaBytes).ToString("F3")));
#else
                    DrawItem("Mono Used Size:", Utility.Text.Format("{0} MB", (Profiler.GetMonoUsedSize() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Mono Heap Size:", Utility.Text.Format("{0} MB", (Profiler.GetMonoHeapSize() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Used Heap Size:", Utility.Text.Format("{0} MB", (Profiler.usedHeapSize / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Allocated Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalAllocatedMemory() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Reserved Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalReservedMemory() / (float)OneMegaBytes).ToString("F3")));
                    DrawItem("Total Unused Reserved Memory:", Utility.Text.Format("{0} MB", (Profiler.GetTotalUnusedReservedMemory() / (float)OneMegaBytes).ToString("F3")));
#endif
#if UNITY_2018_1_OR_NEWER
                    DrawItem("Allocated Memory For Graphics Driver:", Utility.Text.Format("{0} MB", (Profiler.GetAllocatedMemoryForGraphicsDriver() / (float)OneMegaBytes).ToString("F3")));
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Temp Allocator Size:", Utility.Text.Format("{0} MB", (Profiler.GetTempAllocatorSize() / (float)OneMegaBytes).ToString("F3")));
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
