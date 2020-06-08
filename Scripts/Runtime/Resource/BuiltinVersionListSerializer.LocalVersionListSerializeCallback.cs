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
        /// <summary>
        /// 序列化本地版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的本地版本资源列表（版本 0）。</param>
        /// <returns>序列化本地版本资源列表（版本 0）是否成功。</returns>
        public static bool LocalVersionListSerializeCallback_V0(BinaryWriter binaryWriter, LocalVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            LocalVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (LocalVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 序列化本地版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的本地版本资源列表（版本 1）。</param>
        /// <returns>序列化本地版本资源列表（版本 1）是否成功。</returns>
        public static bool LocalVersionListSerializeCallback_V1(BinaryWriter binaryWriter, LocalVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            LocalVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write7BitEncodedInt32(resources.Length);
            foreach (LocalVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write7BitEncodedInt32(resource.Length);
                binaryWriter.Write(resource.HashCode);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }
    }
}
