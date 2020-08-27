//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class QualityInformationWindow : ScrollableDebuggerWindowBase
        {
            private bool m_ApplyExpensiveChanges = false;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Quality Level</b>");
                GUILayout.BeginVertical("box");
                {
                    int currentQualityLevel = QualitySettings.GetQualityLevel();

                    DrawItem("Current Quality Level", QualitySettings.names[currentQualityLevel]);
                    m_ApplyExpensiveChanges = GUILayout.Toggle(m_ApplyExpensiveChanges, "Apply expensive changes on quality level change.");

                    int newQualityLevel = GUILayout.SelectionGrid(currentQualityLevel, QualitySettings.names, 3, "toggle");
                    if (newQualityLevel != currentQualityLevel)
                    {
                        QualitySettings.SetQualityLevel(newQualityLevel, m_ApplyExpensiveChanges);
                    }
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Rendering Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Active Color Space", QualitySettings.activeColorSpace.ToString());
                    DrawItem("Desired Color Space", QualitySettings.desiredColorSpace.ToString());
                    DrawItem("Max Queued Frames", QualitySettings.maxQueuedFrames.ToString());
                    DrawItem("Pixel Light Count", QualitySettings.pixelLightCount.ToString());
                    DrawItem("Master Texture Limit", QualitySettings.masterTextureLimit.ToString());
                    DrawItem("Anisotropic Filtering", QualitySettings.anisotropicFiltering.ToString());
                    DrawItem("Anti Aliasing", QualitySettings.antiAliasing.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Soft Particles", QualitySettings.softParticles.ToString());
#endif
                    DrawItem("Soft Vegetation", QualitySettings.softVegetation.ToString());
                    DrawItem("Realtime Reflection Probes", QualitySettings.realtimeReflectionProbes.ToString());
                    DrawItem("Billboards Face Camera Position", QualitySettings.billboardsFaceCameraPosition.ToString());
#if UNITY_2017_1_OR_NEWER
                    DrawItem("Resolution Scaling Fixed DPI Factor", QualitySettings.resolutionScalingFixedDPIFactor.ToString());
#endif
#if UNITY_2018_2_OR_NEWER
                    DrawItem("Texture Streaming Enabled", QualitySettings.streamingMipmapsActive.ToString());
                    DrawItem("Texture Streaming Add All Cameras", QualitySettings.streamingMipmapsAddAllCameras.ToString());
                    DrawItem("Texture Streaming Memory Budget", QualitySettings.streamingMipmapsMemoryBudget.ToString());
                    DrawItem("Texture Streaming Renderers Per Frame", QualitySettings.streamingMipmapsRenderersPerFrame.ToString());
                    DrawItem("Texture Streaming Max Level Reduction", QualitySettings.streamingMipmapsMaxLevelReduction.ToString());
                    DrawItem("Texture Streaming Max File IO Requests", QualitySettings.streamingMipmapsMaxFileIORequests.ToString());
#endif
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Shadows Information</b>");
                GUILayout.BeginVertical("box");
                {
#if UNITY_2017_1_OR_NEWER
                    DrawItem("Shadowmask Mode", QualitySettings.shadowmaskMode.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Shadow Quality", QualitySettings.shadows.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Shadow Resolution", QualitySettings.shadowResolution.ToString());
#endif
                    DrawItem("Shadow Projection", QualitySettings.shadowProjection.ToString());
                    DrawItem("Shadow Distance", QualitySettings.shadowDistance.ToString());
                    DrawItem("Shadow Near Plane Offset", QualitySettings.shadowNearPlaneOffset.ToString());
                    DrawItem("Shadow Cascades", QualitySettings.shadowCascades.ToString());
                    DrawItem("Shadow Cascade 2 Split", QualitySettings.shadowCascade2Split.ToString());
                    DrawItem("Shadow Cascade 4 Split", QualitySettings.shadowCascade4Split.ToString());
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Other Information</b>");
                GUILayout.BeginVertical("box");
                {
#if UNITY_2019_1_OR_NEWER
                    DrawItem("Skin Weights", QualitySettings.skinWeights.ToString());
#else
                    DrawItem("Blend Weights", QualitySettings.blendWeights.ToString());
#endif
                    DrawItem("VSync Count", QualitySettings.vSyncCount.ToString());
                    DrawItem("LOD Bias", QualitySettings.lodBias.ToString());
                    DrawItem("Maximum LOD Level", QualitySettings.maximumLODLevel.ToString());
                    DrawItem("Particle Raycast Budget", QualitySettings.particleRaycastBudget.ToString());
                    DrawItem("Async Upload Time Slice", Utility.Text.Format("{0} ms", QualitySettings.asyncUploadTimeSlice.ToString()));
                    DrawItem("Async Upload Buffer Size", Utility.Text.Format("{0} MB", QualitySettings.asyncUploadBufferSize.ToString()));
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Async Upload Persistent Buffer", QualitySettings.asyncUploadPersistentBuffer.ToString());
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
