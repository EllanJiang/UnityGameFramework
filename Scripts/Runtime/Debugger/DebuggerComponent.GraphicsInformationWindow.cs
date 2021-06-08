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
        private sealed class GraphicsInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Graphics Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Device ID", SystemInfo.graphicsDeviceID.ToString());
                    DrawItem("Device Name", SystemInfo.graphicsDeviceName);
                    DrawItem("Device Vendor ID", SystemInfo.graphicsDeviceVendorID.ToString());
                    DrawItem("Device Vendor", SystemInfo.graphicsDeviceVendor);
                    DrawItem("Device Type", SystemInfo.graphicsDeviceType.ToString());
                    DrawItem("Device Version", SystemInfo.graphicsDeviceVersion);
                    DrawItem("Memory Size", Utility.Text.Format("{0} MB", SystemInfo.graphicsMemorySize));
                    DrawItem("Multi Threaded", SystemInfo.graphicsMultiThreaded.ToString());
#if UNITY_2019_3_OR_NEWER
                    DrawItem("Rendering Threading Mode", SystemInfo.renderingThreadingMode.ToString());
#endif
#if UNITY_2020_1_OR_NEWER
                    DrawItem("HRD Display Support Flags", SystemInfo.hdrDisplaySupportFlags.ToString());
#endif
                    DrawItem("Shader Level", GetShaderLevelString(SystemInfo.graphicsShaderLevel));
                    DrawItem("Global Maximum LOD", Shader.globalMaximumLOD.ToString());
#if UNITY_5_6_OR_NEWER
                    DrawItem("Global Render Pipeline", Shader.globalRenderPipeline);
#endif
#if UNITY_2020_2_OR_NEWER
                    DrawItem("Min OpenGLES Version", Graphics.minOpenGLESVersion.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Active Tier", Graphics.activeTier.ToString());
#endif
#if UNITY_2017_2_OR_NEWER
                    DrawItem("Active Color Gamut", Graphics.activeColorGamut.ToString());
#endif
#if UNITY_2019_2_OR_NEWER
                    DrawItem("Preserve Frame Buffer Alpha", Graphics.preserveFramebufferAlpha.ToString());
#endif
                    DrawItem("NPOT Support", SystemInfo.npotSupport.ToString());
                    DrawItem("Max Texture Size", SystemInfo.maxTextureSize.ToString());
                    DrawItem("Supported Render Target Count", SystemInfo.supportedRenderTargetCount.ToString());
#if UNITY_2019_3_OR_NEWER
                    DrawItem("Supported Random Write Target Count", SystemInfo.supportedRandomWriteTargetCount.ToString());
#endif
#if UNITY_5_4_OR_NEWER
                    DrawItem("Copy Texture Support", SystemInfo.copyTextureSupport.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Uses Reversed ZBuffer", SystemInfo.usesReversedZBuffer.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Max Cubemap Size", SystemInfo.maxCubemapSize.ToString());
                    DrawItem("Graphics UV Starts At Top", SystemInfo.graphicsUVStartsAtTop.ToString());
#endif
#if UNITY_2020_2_OR_NEWER
                    DrawItem("Constant Buffer Offset Alignment", SystemInfo.constantBufferOffsetAlignment.ToString());
#elif UNITY_2019_1_OR_NEWER
                    DrawItem("Min Constant Buffer Offset Alignment", SystemInfo.minConstantBufferOffsetAlignment.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Has Hidden Surface Removal On GPU", SystemInfo.hasHiddenSurfaceRemovalOnGPU.ToString());
                    DrawItem("Has Dynamic Uniform Array Indexing In Fragment Shaders", SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders.ToString());
#endif
#if UNITY_2019_2_OR_NEWER
                    DrawItem("Has Mip Max Level", SystemInfo.hasMipMaxLevel.ToString());
#endif
#if UNITY_2019_3_OR_NEWER
                    DrawItem("Uses Load Store Actions", SystemInfo.usesLoadStoreActions.ToString());
                    DrawItem("Max Compute Buffer Inputs Compute", SystemInfo.maxComputeBufferInputsCompute.ToString());
                    DrawItem("Max Compute Buffer Inputs Domain", SystemInfo.maxComputeBufferInputsDomain.ToString());
                    DrawItem("Max Compute Buffer Inputs Fragment", SystemInfo.maxComputeBufferInputsFragment.ToString());
                    DrawItem("Max Compute Buffer Inputs Geometry", SystemInfo.maxComputeBufferInputsGeometry.ToString());
                    DrawItem("Max Compute Buffer Inputs Hull", SystemInfo.maxComputeBufferInputsHull.ToString());
                    DrawItem("Max Compute Buffer Inputs Vertex", SystemInfo.maxComputeBufferInputsVertex.ToString());
                    DrawItem("Max Compute Work Group Size", SystemInfo.maxComputeWorkGroupSize.ToString());
                    DrawItem("Max Compute Work Group Size X", SystemInfo.maxComputeWorkGroupSizeX.ToString());
                    DrawItem("Max Compute Work Group Size Y", SystemInfo.maxComputeWorkGroupSizeY.ToString());
                    DrawItem("Max Compute Work Group Size Z", SystemInfo.maxComputeWorkGroupSizeZ.ToString());
#endif
#if UNITY_5_3 || UNITY_5_4
                    DrawItem("Supports Stencil", SystemInfo.supportsStencil.ToString());
                    DrawItem("Supports Render Textures", SystemInfo.supportsRenderTextures.ToString());
#endif
                    DrawItem("Supports Sparse Textures", SystemInfo.supportsSparseTextures.ToString());
                    DrawItem("Supports 3D Textures", SystemInfo.supports3DTextures.ToString());
                    DrawItem("Supports Shadows", SystemInfo.supportsShadows.ToString());
                    DrawItem("Supports Raw Shadow Depth Sampling", SystemInfo.supportsRawShadowDepthSampling.ToString());
#if !UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Render To Cubemap", SystemInfo.supportsRenderToCubemap.ToString());
                    DrawItem("Supports Image Effects", SystemInfo.supportsImageEffects.ToString());
#endif
                    DrawItem("Supports Compute Shader", SystemInfo.supportsComputeShaders.ToString());
                    DrawItem("Supports Instancing", SystemInfo.supportsInstancing.ToString());
#if UNITY_5_4_OR_NEWER
                    DrawItem("Supports 2D Array Textures", SystemInfo.supports2DArrayTextures.ToString());
                    DrawItem("Supports Motion Vectors", SystemInfo.supportsMotionVectors.ToString());
#endif
#if UNITY_5_5_OR_NEWER
                    DrawItem("Supports Cubemap Array Textures", SystemInfo.supportsCubemapArrayTextures.ToString());
#endif
#if UNITY_5_6_OR_NEWER
                    DrawItem("Supports 3D Render Textures", SystemInfo.supports3DRenderTextures.ToString());
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
                    DrawItem("Supports Multi-sampled Textures", SystemInfo.supportsMultisampledTextures.ToString());
#endif
#if UNITY_2018_1_OR_NEWER
                    DrawItem("Supports Async GPU Readback", SystemInfo.supportsAsyncGPUReadback.ToString());
                    DrawItem("Supports 32bits Index Buffer", SystemInfo.supports32bitsIndexBuffer.ToString());
                    DrawItem("Supports Hardware Quad Topology", SystemInfo.supportsHardwareQuadTopology.ToString());
#endif
#if UNITY_2018_2_OR_NEWER
                    DrawItem("Supports Mip Streaming", SystemInfo.supportsMipStreaming.ToString());
                    DrawItem("Supports Multi-sample Auto Resolve", SystemInfo.supportsMultisampleAutoResolve.ToString());
#endif
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Supports Separated Render Targets Blend", SystemInfo.supportsSeparatedRenderTargetsBlend.ToString());
#endif
#if UNITY_2019_1_OR_NEWER
                    DrawItem("Supports Set Constant Buffer", SystemInfo.supportsSetConstantBuffer.ToString());
#endif
#if UNITY_2019_3_OR_NEWER
                    DrawItem("Supports Geometry Shaders", SystemInfo.supportsGeometryShaders.ToString());
                    DrawItem("Supports Ray Tracing", SystemInfo.supportsRayTracing.ToString());
                    DrawItem("Supports Tessellation Shaders", SystemInfo.supportsTessellationShaders.ToString());
#endif
#if UNITY_2020_1_OR_NEWER
                    DrawItem("Supports Compressed 3D Textures", SystemInfo.supportsCompressed3DTextures.ToString());
                    DrawItem("Supports Conservative Raster", SystemInfo.supportsConservativeRaster.ToString());
                    DrawItem("Supports GPU Recorder", SystemInfo.supportsGpuRecorder.ToString());
#endif
#if UNITY_2020_2_OR_NEWER
                    DrawItem("Supports Multi-sampled 2D Array Textures", SystemInfo.supportsMultisampled2DArrayTextures.ToString());
                    DrawItem("Supports Multiview", SystemInfo.supportsMultiview.ToString());
                    DrawItem("Supports Render Target Array Index From Vertex Shader", SystemInfo.supportsRenderTargetArrayIndexFromVertexShader.ToString());
#endif
                }
                GUILayout.EndVertical();
            }

            private string GetShaderLevelString(int shaderLevel)
            {
                return Utility.Text.Format("Shader Model {0}.{1}", shaderLevel / 10, shaderLevel % 10);
            }
        }
    }
}
