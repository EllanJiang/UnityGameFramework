//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private sealed class AssetData
        {
            private readonly string m_Guid;
            private readonly string m_Name;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly string[] m_DependencyAssetNames;

            public AssetData(string guid, string name, int length, int hashCode, string[] dependencyAssetNames)
            {
                m_Guid = guid;
                m_Name = name;
                m_Length = length;
                m_HashCode = hashCode;
                m_DependencyAssetNames = dependencyAssetNames;
            }

            public string Guid
            {
                get
                {
                    return m_Guid;
                }
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public int Length
            {
                get
                {
                    return m_Length;
                }
            }

            public int HashCode
            {
                get
                {
                    return m_HashCode;
                }
            }

            public string[] GetDependencyAssetNames()
            {
                return m_DependencyAssetNames;
            }
        }
    }
}
