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
        private sealed class ResourceCode
        {
            private readonly Platform m_Platform;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_CompressedLength;
            private readonly int m_CompressedHashCode;

            public ResourceCode(Platform platform, int length, int hashCode, int compressedLength, int compressedHashCode)
            {
                m_Platform = platform;
                m_Length = length;
                m_HashCode = hashCode;
                m_CompressedLength = compressedLength;
                m_CompressedHashCode = compressedHashCode;
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

            public int CompressedLength
            {
                get
                {
                    return m_CompressedLength;
                }
            }

            public int CompressedHashCode
            {
                get
                {
                    return m_CompressedHashCode;
                }
            }
        }
    }
}
