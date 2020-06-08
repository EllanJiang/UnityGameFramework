//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 内置版本资源列表序列化器。
    /// </summary>
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 反序列化本地版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的本地版本资源列表（版本 0）。</returns>
        public static LocalVersionList LocalVersionListDeserializeCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            int resourceCount = binaryReader.ReadInt32();
            LocalVersionList.Resource[] resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                resources[i] = new LocalVersionList.Resource(name, variant, null, loadType, length, hashCode);
            }

            return new LocalVersionList(resources);
        }

        /// <summary>
        /// 反序列化本地版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的本地版本资源列表（版本 1）。</returns>
        public static LocalVersionList LocalVersionListDeserializeCallback_V1(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            int resourceCount = binaryReader.Read7BitEncodedInt32();
            LocalVersionList.Resource[] resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                string extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.Read7BitEncodedInt32();
                int hashCode = binaryReader.ReadInt32();
                resources[i] = new LocalVersionList.Resource(name, variant, extension, loadType, length, hashCode);
            }

            return new LocalVersionList(resources);
        }
    }
}
