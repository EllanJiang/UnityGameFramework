//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 内置版本资源列表序列化器。
    /// </summary>
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本 0）获取指定键的值回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns></returns>
        public static bool UpdatableVersionListTryGetValueCallback_V0(BinaryReader binaryReader, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            binaryReader.BaseStream.Position += CachedHashBytesLength;
            byte stringLength = binaryReader.ReadByte();
            binaryReader.BaseStream.Position += stringLength;
            value = binaryReader.ReadInt32();
            return true;
        }

        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本 1 或版本 2）获取指定键的值回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns></returns>
        public static bool UpdatableVersionListTryGetValueCallback_V1_V2(BinaryReader binaryReader, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            binaryReader.BaseStream.Position += CachedHashBytesLength;
            byte stringLength = binaryReader.ReadByte();
            binaryReader.BaseStream.Position += stringLength;
            value = binaryReader.Read7BitEncodedInt32();
            return true;
        }
    }
}
