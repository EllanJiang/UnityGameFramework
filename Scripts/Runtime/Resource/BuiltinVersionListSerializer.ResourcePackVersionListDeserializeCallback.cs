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
        /// 反序列化资源包版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的资源包版本资源列表（版本 0）。</returns>
        public static ResourcePackVersionList ResourcePackVersionListDeserializeCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            int dataOffset = binaryReader.ReadInt32();
            long dataLength = binaryReader.ReadInt64();
            int dataHashCode = binaryReader.ReadInt32();
            int resourceCount = binaryReader.Read7BitEncodedInt32();
            ResourcePackVersionList.Resource[] resources = resourceCount > 0 ? new ResourcePackVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                string extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                byte loadType = binaryReader.ReadByte();
                long offset = binaryReader.Read7BitEncodedInt64();
                int length = binaryReader.Read7BitEncodedInt32();
                int hashCode = binaryReader.ReadInt32();
                int zipLength = binaryReader.Read7BitEncodedInt32();
                int zipHashCode = binaryReader.ReadInt32();
                resources[i] = new ResourcePackVersionList.Resource(name, variant, extension, loadType, offset, length, hashCode, zipLength, zipHashCode);
            }

            return new ResourcePackVersionList(dataOffset, dataLength, dataHashCode, resources);
        }
    }
}
