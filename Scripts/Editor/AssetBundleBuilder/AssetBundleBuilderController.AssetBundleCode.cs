//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public sealed partial class AssetBundleBuilderController
    {
        private sealed class AssetBundleCode
        {
            private readonly Platform m_Platform;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_ZipLength;
            private readonly int m_ZipHashCode;

            public AssetBundleCode(Platform platform, int length, int hashCode, int zipLength, int zipHashCode)
            {
                m_Platform = platform;
                m_Length = length;
                m_HashCode = hashCode;
                m_ZipLength = zipLength;
                m_ZipHashCode = zipHashCode;
            }

            public Platform Platform
            {
                get
                {
                    return m_Platform;
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

            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }

            public int ZipHashCode
            {
                get
                {
                    return m_ZipHashCode;
                }
            }
        }
    }
}
