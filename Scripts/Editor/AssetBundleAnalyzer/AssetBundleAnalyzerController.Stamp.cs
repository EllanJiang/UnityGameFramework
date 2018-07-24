//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public sealed partial class AssetBundleAnalyzerController
    {
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
