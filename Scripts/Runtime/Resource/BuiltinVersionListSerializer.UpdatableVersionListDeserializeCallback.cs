//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 内置版本资源列表序列化器。
    /// </summary>
    public static partial class BuiltinVersionListSerializer
    {
        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的可更新模式版本资源列表（版本 0）。</returns>
        public static UpdatableVersionList UpdatableVersionListDeserializeCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = binaryReader.ReadEncryptedString(encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            UpdatableVersionList.Asset[] assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
            int resourceCount = binaryReader.ReadInt32();
            UpdatableVersionList.Resource[] resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
            string[][] resourceToAssetNames = new string[resourceCount][];
            List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames = new List<KeyValuePair<string, string[]>>(assetCount);
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                int zipLength = binaryReader.ReadInt32();
                int zipHashCode = binaryReader.ReadInt32();
                Utility.Converter.GetBytes(hashCode, s_CachedHashBytes);

                int assetNameCount = binaryReader.ReadInt32();
                string[] assetNames = assetNameCount > 0 ? new string[assetNameCount] : null;
                for (int j = 0; j < assetNameCount; j++)
                {
                    assetNames[j] = binaryReader.ReadEncryptedString(s_CachedHashBytes);
                    int dependencyAssetNameCount = binaryReader.ReadInt32();
                    string[] dependencyAssetNames = dependencyAssetNameCount > 0 ? new string[dependencyAssetNameCount] : null;
                    for (int k = 0; k < dependencyAssetNameCount; k++)
                    {
                        dependencyAssetNames[k] = binaryReader.ReadEncryptedString(s_CachedHashBytes);
                    }

                    assetNameToDependencyAssetNames.Add(new KeyValuePair<string, string[]>(assetNames[j], dependencyAssetNames));
                }

                resourceToAssetNames[i] = assetNames;
                resources[i] = new UpdatableVersionList.Resource(name, variant, null, loadType, length, hashCode, zipLength, zipHashCode, assetNameCount > 0 ? new int[assetNameCount] : null);
            }

            assetNameToDependencyAssetNames.Sort(AssetNameToDependencyAssetNamesComparer);
            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            int index = 0;
            foreach (KeyValuePair<string, string[]> i in assetNameToDependencyAssetNames)
            {
                if (i.Value != null)
                {
                    int[] dependencyAssetIndexes = new int[i.Value.Length];
                    for (int j = 0; j < i.Value.Length; j++)
                    {
                        dependencyAssetIndexes[j] = GetAssetNameIndex(assetNameToDependencyAssetNames, i.Value[j]);
                    }

                    assets[index++] = new UpdatableVersionList.Asset(i.Key, dependencyAssetIndexes);
                }
                else
                {
                    assets[index++] = new UpdatableVersionList.Asset(i.Key, null);
                }
            }

            for (int i = 0; i < resources.Length; i++)
            {
                int[] assetIndexes = resources[i].GetAssetIndexes();
                for (int j = 0; j < assetIndexes.Length; j++)
                {
                    assetIndexes[j] = GetAssetNameIndex(assetNameToDependencyAssetNames, resourceToAssetNames[i][j]);
                }
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            UpdatableVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                int resourceIndexCount = binaryReader.ReadInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.ReadUInt16();
                }

                resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
            }

            return new UpdatableVersionList(applicableGameVersion, internalResourceVersion, assets, resources, resourceGroups);
        }

        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的可更新模式版本资源列表（版本 1）。</returns>
        public static UpdatableVersionList UpdatableVersionListDeserializeCallback_V1(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = binaryReader.ReadEncryptedString(encryptBytes);
            int internalResourceVersion = binaryReader.Read7BitEncodedInt32();
            int assetCount = binaryReader.Read7BitEncodedInt32();
            UpdatableVersionList.Asset[] assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
            for (int i = 0; i < assetCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                int dependencyAssetCount = binaryReader.Read7BitEncodedInt32();
                int[] dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                for (int j = 0; j < dependencyAssetCount; j++)
                {
                    dependencyAssetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                }

                assets[i] = new UpdatableVersionList.Asset(name, dependencyAssetIndexes);
            }

            int resourceCount = binaryReader.Read7BitEncodedInt32();
            UpdatableVersionList.Resource[] resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                string extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.Read7BitEncodedInt32();
                int hashCode = binaryReader.ReadInt32();
                int zipLength = binaryReader.Read7BitEncodedInt32();
                int zipHashCode = binaryReader.ReadInt32();
                int assetIndexCount = binaryReader.Read7BitEncodedInt32();
                int[] assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                for (int j = 0; j < assetIndexCount; j++)
                {
                    assetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                }

                resources[i] = new UpdatableVersionList.Resource(name, variant, extension, loadType, length, hashCode, zipLength, zipHashCode, assetIndexes);
            }

            int resourceGroupCount = binaryReader.Read7BitEncodedInt32();
            UpdatableVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                int resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
                }

                resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
            }

            return new UpdatableVersionList(applicableGameVersion, internalResourceVersion, assets, resources, resourceGroups);
        }
    }
}
