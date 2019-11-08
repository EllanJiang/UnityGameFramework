//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Profiler Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Supported", Profiler.supported.ToString());
                    DrawItem("Enabled", Profiler.enabled.ToString());
                    DrawItem("Enable Binary Log", Profiler.enableBinaryLog ? Utility.Text.Format("True, {0}", Profiler.logFile) : "False");
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Area Count", Profiler.areaCount.ToString());
#endif
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Max Samples Number Per Frame", Profiler.maxNumberOfSamplesPerFrame.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Max Used Memory", GetByteLengthString(Profiler.maxUsedMemory));
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Mono Used Size", GetByteLengthString(Profiler.GetMonoUsedSizeLong()));
                    DrawItem("Mono Heap Size", GetByteLengthString(Profiler.GetMonoHeapSizeLong()));
                    DrawItem("Used Heap Size", GetByteLengthString(Profiler.usedHeapSizeLong));
                    DrawItem("Total Allocated Memory", GetByteLengthString(Profiler.GetTotalAllocatedMemoryLong()));
                    DrawItem("Total Reserved Memory", GetByteLengthString(Profiler.GetTotalReservedMemoryLong()));
                    DrawItem("Total Unused Reserved Memory", GetByteLengthString(Profiler.GetTotalUnusedReservedMemoryLong()));
#else
                    DrawItem("Mono Used Size", GetByteLengthString(Profiler.GetMonoUsedSize()));
                    DrawItem("Mono Heap Size", GetByteLengthString(Profiler.GetMonoHeapSize()));
                    DrawItem("Used Heap Size", GetByteLengthString(Profiler.usedHeapSize));
                    DrawItem("Total Allocated Memory", GetByteLengthString(Profiler.GetTotalAllocatedMemory()));
                    DrawItem("Total Reserved Memory", GetByteLengthString(Profiler.GetTotalReservedMemory()));
                    DrawItem("Total Unused Reserved Memory", GetByteLengthString(Profiler.GetTotalUnusedReservedMemory()));
#endif
#if UNITY_2018_1_OR_NEWER
                    DrawItem("Allocated Memory For Graphics Driver", GetByteLengthString(Profiler.GetAllocatedMemoryForGraphicsDriver()));
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Temp Allocator Size", GetByteLengthString(Profiler.GetTempAllocatorSize()));
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
