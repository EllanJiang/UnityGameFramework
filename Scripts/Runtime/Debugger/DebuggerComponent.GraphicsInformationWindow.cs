//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class GraphicsInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Graphics Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Device ID:", SystemInfo.graphicsDeviceID.ToString());
                    DrawItem("Device Name:", SystemInfo.graphicsDeviceName);
                    DrawItem("Device Vendor ID:", SystemInfo.graphicsDeviceVendorID.ToString());
                    DrawItem("Device Vendor:", SystemInfo.graphicsDeviceVendor);
                    DrawItem("Device Type:", SystemInfo.graphicsDeviceType.ToString());
                    DrawItem("Device Version:", SystemInfo.graphicsDeviceVersion);
                    DrawItem("Memory Size:", Utility.Text.Format("{0} MB", SystemInfo.graphicsMemorySize.ToString()));
                    DrawItem("Multi Threaded:", SystemInfo.graphicsMultiThreaded.ToString());
                    DrawItem("Shader Level:", GetShaderLevelString(SystemInfo.graphicsShaderLevel));
                    DrawItem("Global Maximum LOD:", Shader.globalMaximumLOD.ToString());
#if UNITY_5_5_OR_NEWER
                    DrawItem("Active Tier", Graphics.activeTier.ToString());
#endif
#if UNITY_2017_2_OR_NEWER
                    DrawItem("Active Color Gamut", Graphics.activeColorGamut.ToString());
#endif
                    DrawItem("NPOT Support:", SystemInfo.npotSupport.ToString());
                    DrawItem("Max Texture Size:", SystemInfo.maxTextureSize.ToString());
                    DrawItem("Supported Render Target Count:", SystemInfo.supportedRenderTargetCount.ToString());
#if UNITY_5_4_OR_NEWER
                    DrawItem("Copy Texture Support:", SystemInfo.copyTextureSupport.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Uses Reversed ZBuffer:", SystemInfo.usesReversedZBuffer.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Max Cubemap Size:", SystemInfo.maxCubemapSize.ToString());
                    DrawItem("Graphics UV Starts At Top:", SystemInfo.graphicsUVStartsAtTop.ToString());
#endif
#if UNITY_2019_1_OR_NEWER
                    DrawItem("Min Constant Buffer Offset Alignment:", SystemInfo.minConstantBufferOffsetAlignment.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Has Hidden Surface Removal On GPU:", SystemInfo.hasHiddenSurfaceRemovalOnGPU.ToString());
                    DrawItem("Has Dynamic Uniform Array Indexing In Fragment Shaders:", SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders.ToString());
#endif
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Supports Stencil:", SystemInfo.supportsStencil.ToString());
                    DrawItem("Supports Render Textures:", SystemInfo.supportsRenderTextures.ToString());
#endif
                    DrawItem("Supports Sparse Textures:", SystemInfo.supportsSparseTextures.ToString());
                    DrawItem("Supports 3D Textures:", SystemInfo.supports3DTextures.ToString());
                    DrawItem("Supports Shadows:", SystemInfo.supportsShadows.ToString());
                    DrawItem("Supports Raw Shadow Depth Sampling:", SystemInfo.supportsRawShadowDepthSampling.ToString());
#if !UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Render To Cubemap:", SystemInfo.supportsRenderToCubemap.ToString());
#endif
                    DrawItem("Supports Compute Shader:", SystemInfo.supportsComputeShaders.ToString());
                    DrawItem("Supports Instancing:", SystemInfo.supportsInstancing.ToString());
#if !UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Image Effects:", SystemInfo.supportsImageEffects.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Supports 2D Array Textures:", SystemInfo.supports2DArrayTextures.ToString());
                    DrawItem("Supports Motion Vectors:", SystemInfo.supportsMotionVectors.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Supports Cubemap Array Textures:", SystemInfo.supportsCubemapArrayTextures.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Supports 3D Render Textures:", SystemInfo.supports3DRenderTextures.ToString());
#endif
#if UNITY_2017_2_OR_NEWER && !UNITY_2017_2_0 || UNITY_2017_1_4
                    DrawItem("Supports Texture Wrap Mirror Once", SystemInfo.supportsTextureWrapMirrorOnce.ToString());
#endif
#if UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Graphics Fence", SystemInfo.supportsGraphicsFence.ToString());
#elif UNITY_2017_3_OR_NEWER
                    DrawItem("Supports GPU Fence", SystemInfo.supportsGPUFence.ToString());
#endif
#if UNITY_2017_3_OR_NEWER
                    DrawItem("Supports Async Compute", SystemInfo.supportsAsyncCompute.ToString());
                    DrawItem("Supports Multisampled Textures", SystemInfo.supportsMultisampledTextures.ToString());
#endif
#if UNITY_2018_1_OR_NEWER
                    DrawItem("Supports Async GPU Readback", SystemInfo.supportsAsyncGPUReadback.ToString());
                    DrawItem("Supports 32bits Index Buffer", SystemInfo.supports32bitsIndexBuffer.ToString());
                    DrawItem("Supports Hardware Quad Topology", SystemInfo.supportsHardwareQuadTopology.ToString());
#endif
#if UNITY_2018_2_OR_NEWER
                    DrawItem("Supports Mip Streaming", SystemInfo.supportsMipStreaming.ToString());
                    DrawItem("Supports Multisample Auto Resolve", SystemInfo.supportsMultisampleAutoResolve.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Supports Separated Render Targets Blend:", SystemInfo.supportsSeparatedRenderTargetsBlend.ToString());
#endif
#if UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Set Constant Buffer:", SystemInfo.supportsSetConstantBuffer.ToString());
#endif
                }
                GUILayout.EndVertical();
            }

            private string GetShaderLevelString(int shaderLevel)
            {
                return Utility.Text.Format("Shader Model {0}.{1}", (shaderLevel / 10).ToString(), (shaderLevel % 10).ToString());
            }
        }
    }
}
