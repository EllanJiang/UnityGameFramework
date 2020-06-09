//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 内置版本资源列表序列化器。
    /// </summary>
    public static partial class BuiltinVersionListSerializer
    {
#if UNITY_EDITOR

        /// <summary>
        /// 序列化资源包版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的资源包版本资源列表（版本 0）。</param>
        /// <returns>序列化资源包版本资源列表（版本 0）是否成功。</returns>
        public static bool ResourcePackVersionListSerializeCallback_V0(BinaryWriter binaryWriter, ResourcePackVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            binaryWriter.Write(versionList.Offset);
            binaryWriter.Write(versionList.Length);
            binaryWriter.Write(versionList.HashCode);
            ResourcePackVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write7BitEncodedInt32(resources.Length);
            foreach (ResourcePackVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write7BitEncodedInt64(resource.Offset);
                binaryWriter.Write7BitEncodedInt32(resource.Length);
                binaryWriter.Write(resource.HashCode);
                binaryWriter.Write7BitEncodedInt32(resource.ZipLength);
                binaryWriter.Write(resource.ZipHashCode);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

#endif
    }
}
