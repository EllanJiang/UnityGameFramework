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
        private sealed class QualityInformationWindow : ScrollableDebuggerWindowBase
        {
            private bool m_ApplyExpensiveChanges = false;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Quality Level</b>");
                GUILayout.BeginVertical("box");
                {
                    int currentQualityLevel = QualitySettings.GetQualityLevel();

                    DrawItem("Current Quality Level:", QualitySettings.names[currentQualityLevel]);
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
                    DrawItem("Active Color Space:", QualitySettings.activeColorSpace.ToString());
                    DrawItem("Desired Color Space:", QualitySettings.desiredColorSpace.ToString());
                    DrawItem("Max Queued Frames:", QualitySettings.maxQueuedFrames.ToString());
                    DrawItem("Pixel Light Count:", QualitySettings.pixelLightCount.ToString());
                    DrawItem("Master Texture Limit:", QualitySettings.masterTextureLimit.ToString());
                    DrawItem("Anisotropic Filtering:", QualitySettings.anisotropicFiltering.ToString());
                    DrawItem("Anti Aliasing:", QualitySettings.antiAliasing.ToString());
                    DrawItem("Realtime Reflection Probes:", QualitySettings.realtimeReflectionProbes.ToString());
                    DrawItem("Billboards Face Camera Position:", QualitySettings.billboardsFaceCameraPosition.ToString());
#if UNITY_2017_1_OR_NEWER
                    DrawItem("Resolution Scaling Fixed DPI Factor:", QualitySettings.resolutionScalingFixedDPIFactor.ToString());
#endif
#if UNITY_2018_2_OR_NEWER
                    DrawItem("Streaming Mipmaps Active", QualitySettings.streamingMipmapsActive.ToString());
                    DrawItem("Streaming Mipmaps Add All Cameras", QualitySettings.streamingMipmapsAddAllCameras.ToString());
                    DrawItem("Streaming Mipmaps Memory Budget", QualitySettings.streamingMipmapsMemoryBudget.ToString());
                    DrawItem("Streaming Mipmaps Renderers Per Frame", QualitySettings.streamingMipmapsRenderersPerFrame.ToString());
                    DrawItem("Streaming Mipmaps Max Level Reduction", QualitySettings.streamingMipmapsMaxLevelReduction.ToString());
                    DrawItem("Streaming Mipmaps Max File IO Requests", QualitySettings.streamingMipmapsMaxFileIORequests.ToString());
#endif
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Shadows Information</b>");
                GUILayout.BeginVertical("box");
                {
#if UNITY_5_4_OR_NEWER
                    DrawItem("Shadow Resolution:", QualitySettings.shadowResolution.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Shadow Quality:", QualitySettings.shadows.ToString());
#endif
                    DrawItem("Shadow Projection:", QualitySettings.shadowProjection.ToString());
                    DrawItem("Shadow Distance:", QualitySettings.shadowDistance.ToString());
#if UNITY_2017_1_OR_NEWER
                    DrawItem("Shadowmask Mode:", QualitySettings.shadowmaskMode.ToString());
#endif
                    DrawItem("Shadow Near Plane Offset:", QualitySettings.shadowNearPlaneOffset.ToString());
                    DrawItem("Shadow Cascades:", QualitySettings.shadowCascades.ToString());
                    DrawItem("Shadow Cascade 2 Split:", QualitySettings.shadowCascade2Split.ToString());
                    DrawItem("Shadow Cascade 4 Split:", QualitySettings.shadowCascade4Split.ToString());
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Other Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Blend Weights:", QualitySettings.blendWeights.ToString());
                    DrawItem("VSync Count:", QualitySettings.vSyncCount.ToString());
                    DrawItem("LOD Bias:", QualitySettings.lodBias.ToString());
                    DrawItem("Maximum LOD Level:", QualitySettings.maximumLODLevel.ToString());
                    DrawItem("Particle Raycast Budget:", QualitySettings.particleRaycastBudget.ToString());
                    DrawItem("Async Upload Time Slice:", string.Format("{0} ms", QualitySettings.asyncUploadTimeSlice.ToString()));
                    DrawItem("Async Upload Buffer Size:", string.Format("{0} MB", QualitySettings.asyncUploadBufferSize.ToString()));
#if UNITY_5_5_OR_NEWER
                    DrawItem("Soft Particles:", QualitySettings.softParticles.ToString());
#endif
                    DrawItem("Soft Vegetation:", QualitySettings.softVegetation.ToString());
                }
                GUILayout.EndVertical();
            }
        }
    }
}
