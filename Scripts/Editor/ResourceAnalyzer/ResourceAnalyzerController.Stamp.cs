//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed partial class ResourceAnalyzerController
    {
        [StructLayout(LayoutKind.Auto)]
        private struct Stamp
        {
            private readonly string m_HostAssetName;
            private readonly string m_DependencyAssetName;

            public Stamp(string hostAssetName, string dependencyAssetName)
            {
                m_HostAssetName = hostAssetName;
                m_DependencyAssetName = dependencyAssetName;
            }

            public string HostAssetName
            {
                get
                {
                    return m_HostAssetName;
                }
            }

            public string DependencyAssetName
            {
                get
                {
                    return m_DependencyAssetName;
                }
            }
        }
    }
}
