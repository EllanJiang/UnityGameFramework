//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private sealed class VersionListData
        {
            public VersionListData(string path, int length, int hashCode, int zipLength, int zipHashCode)
            {
                Path = path;
                Length = length;
                HashCode = hashCode;
                ZipLength = zipLength;
                ZipHashCode = zipHashCode;
            }

            public string Path
            {
                get;
                private set;
            }

            public int Length
            {
                get;
                private set;
            }

            public int HashCode
            {
                get;
                private set;
            }

            public int ZipLength
            {
                get;
                private set;
            }

            public int ZipHashCode
            {
                get;
                private set;
            }
        }
    }
}
