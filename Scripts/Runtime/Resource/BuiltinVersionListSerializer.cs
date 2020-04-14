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
    public static class BuiltinVersionListSerializer
    {
        private const int CachedHashBytesLength = 4;
        private static readonly byte[] s_CachedHashBytes = new byte[CachedHashBytesLength];
        private static readonly byte[] s_CachedBytesForEncryptedString = new byte[byte.MaxValue];

        #region Version 0

#if UNITY_EDITOR

        /// <summary>
        /// 序列化单机模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的单机模式版本资源列表（版本 0）。</param>
        /// <returns>序列化单机模式版本资源列表（版本 0）是否成功。</returns>
        public static bool SerializePackageVersionListCallback_V0(BinaryWriter binaryWriter, PackageVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            WriteEncryptedString(binaryWriter, versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            PackageVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            PackageVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (PackageVersionList.Resource resource in resources)
            {
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write(assetIndexes.Length);
                byte[] hashBytes = new byte[CachedHashBytesLength];
                foreach (int assetIndex in assetIndexes)
                {
                    Utility.Converter.GetBytes(resource.HashCode, hashBytes);
                    PackageVersionList.Asset asset = assets[assetIndex];
                    WriteEncryptedString(binaryWriter, asset.Name, hashBytes);
                    int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                    binaryWriter.Write(dependencyAssetIndexes.Length);
                    foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        WriteEncryptedString(binaryWriter, assets[dependencyAssetIndex].Name, hashBytes);
                    }
                }
            }

            PackageVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (PackageVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                WriteEncryptedString(binaryWriter, resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write(resourceIndexes.Length);
                foreach (ushort resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write(resourceIndex);
                }
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

#endif

        /// <summary>
        /// 反序列化单机模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的单机模式版本资源列表（版本 0）。</returns>
        public static PackageVersionList DeserializePackageVersionListCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = ReadEncryptedString(binaryReader, encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            PackageVersionList.Asset[] assets = assetCount > 0 ? new PackageVersionList.Asset[assetCount] : null;
            int resourceCount = binaryReader.ReadInt32();
            PackageVersionList.Resource[] resources = resourceCount > 0 ? new PackageVersionList.Resource[resourceCount] : null;
            List<string[]> resourceToAssetNames = new List<string[]>();
            SortedDictionary<string, string[]> assetNameToDependencyAssetNames = new SortedDictionary<string, string[]>();
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                Utility.Converter.GetBytes(hashCode, s_CachedHashBytes);

                int assetNameCount = binaryReader.ReadInt32();
                string[] assetNames = new string[assetNameCount];
                for (int j = 0; j < assetNameCount; j++)
                {
                    assetNames[j] = ReadEncryptedString(binaryReader, s_CachedHashBytes);
                    int dependencyAssetNameCount = binaryReader.ReadInt32();
                    string[] dependencyAssetNames = dependencyAssetNameCount > 0 ? new string[dependencyAssetNameCount] : null;
                    for (int k = 0; k < dependencyAssetNameCount; k++)
                    {
                        dependencyAssetNames[k] = ReadEncryptedString(binaryReader, s_CachedHashBytes);
                    }

                    assetNameToDependencyAssetNames.Add(assetNames[j], dependencyAssetNames);
                }

                resourceToAssetNames.Add(assetNames);
                resources[i] = new PackageVersionList.Resource(name, variant, loadType, length, hashCode, assetNameCount > 0 ? new int[assetNameCount] : null);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            int index = 0;
            foreach (KeyValuePair<string, string[]> i in assetNameToDependencyAssetNames)
            {
                if (i.Value != null)
                {
                    int[] dependencyAssetIndexes = new int[i.Value.Length];
                    for (int j = 0; j < i.Value.Length; j++)
                    {
                        int position = 0;
                        foreach (KeyValuePair<string, string[]> k in assetNameToDependencyAssetNames)
                        {
                            if (k.Key == i.Value[j])
                            {
                                break;
                            }

                            position++;
                        }

                        dependencyAssetIndexes[j] = position;
                    }

                    assets[index++] = new PackageVersionList.Asset(i.Key, dependencyAssetIndexes);
                }
                else
                {
                    assets[index++] = new PackageVersionList.Asset(i.Key, null);
                }
            }

            for (int i = 0; i < resources.Length; i++)
            {
                int[] assetIndexes = resources[i].GetAssetIndexes();
                for (int j = 0; j < assetIndexes.Length; j++)
                {
                    int position = 0;
                    foreach (KeyValuePair<string, string[]> k in assetNameToDependencyAssetNames)
                    {
                        if (k.Key == resourceToAssetNames[i][j])
                        {
                            break;
                        }

                        position++;
                    }

                    assetIndexes[j] = position;
                }
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            PackageVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new PackageVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                int resourceIndexCount = binaryReader.ReadInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.ReadUInt16();
                }

                resourceGroups[i] = new PackageVersionList.ResourceGroup(name, resourceIndexes);
            }

            return new PackageVersionList(applicableGameVersion, internalResourceVersion, assets, resources, resourceGroups);
        }

#if UNITY_EDITOR

        /// <summary>
        /// 序列化可更新模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的可更新模式版本资源列表（版本 0）。</param>
        /// <returns>序列化可更新模式版本资源列表（版本 0）是否成功。</returns>
        public static bool SerializeUpdatableVersionListCallback_V0(BinaryWriter binaryWriter, UpdatableVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            WriteEncryptedString(binaryWriter, versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            UpdatableVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            UpdatableVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (UpdatableVersionList.Resource resource in resources)
            {
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
                binaryWriter.Write(resource.ZipLength);
                binaryWriter.Write(resource.ZipHashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write(assetIndexes.Length);
                byte[] hashBytes = new byte[CachedHashBytesLength];
                foreach (int assetIndex in assetIndexes)
                {
                    Utility.Converter.GetBytes(resource.HashCode, hashBytes);
                    UpdatableVersionList.Asset asset = assets[assetIndex];
                    WriteEncryptedString(binaryWriter, asset.Name, hashBytes);
                    int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                    binaryWriter.Write(dependencyAssetIndexes.Length);
                    foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        WriteEncryptedString(binaryWriter, assets[dependencyAssetIndex].Name, hashBytes);
                    }
                }
            }

            UpdatableVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (UpdatableVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                WriteEncryptedString(binaryWriter, resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write(resourceIndexes.Length);
                foreach (ushort resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write(resourceIndex);
                }
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

#endif

        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的可更新模式版本资源列表（版本 0）。</returns>
        public static UpdatableVersionList DeserializeUpdatableVersionListCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = ReadEncryptedString(binaryReader, encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            UpdatableVersionList.Asset[] assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
            int resourceCount = binaryReader.ReadInt32();
            UpdatableVersionList.Resource[] resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
            List<string[]> resourceToAssetNames = new List<string[]>();
            SortedDictionary<string, string[]> assetNameToDependencyAssetNames = new SortedDictionary<string, string[]>();
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                int zipLength = binaryReader.ReadInt32();
                int zipHashCode = binaryReader.ReadInt32();
                Utility.Converter.GetBytes(hashCode, s_CachedHashBytes);

                int assetNameCount = binaryReader.ReadInt32();
                string[] assetNames = new string[assetNameCount];
                for (int j = 0; j < assetNameCount; j++)
                {
                    assetNames[j] = ReadEncryptedString(binaryReader, s_CachedHashBytes);
                    int dependencyAssetNameCount = binaryReader.ReadInt32();
                    string[] dependencyAssetNames = dependencyAssetNameCount > 0 ? new string[dependencyAssetNameCount] : null;
                    for (int k = 0; k < dependencyAssetNameCount; k++)
                    {
                        dependencyAssetNames[k] = ReadEncryptedString(binaryReader, s_CachedHashBytes);
                    }

                    assetNameToDependencyAssetNames.Add(assetNames[j], dependencyAssetNames);
                }

                resourceToAssetNames.Add(assetNames);
                resources[i] = new UpdatableVersionList.Resource(name, variant, loadType, length, hashCode, zipLength, zipHashCode, assetNameCount > 0 ? new int[assetNameCount] : null);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            int index = 0;
            foreach (KeyValuePair<string, string[]> i in assetNameToDependencyAssetNames)
            {
                if (i.Value != null)
                {
                    int[] dependencyAssetIndexes = new int[i.Value.Length];
                    for (int j = 0; j < i.Value.Length; j++)
                    {
                        int position = 0;
                        foreach (KeyValuePair<string, string[]> k in assetNameToDependencyAssetNames)
                        {
                            if (k.Key == i.Value[j])
                            {
                                break;
                            }

                            position++;
                        }

                        dependencyAssetIndexes[j] = position;
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
                    int position = 0;
                    foreach (KeyValuePair<string, string[]> k in assetNameToDependencyAssetNames)
                    {
                        if (k.Key == resourceToAssetNames[i][j])
                        {
                            break;
                        }

                        position++;
                    }

                    assetIndexes[j] = position;
                }
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            UpdatableVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
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
        /// 序列化本地版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的本地版本资源列表（版本 0）。</param>
        /// <returns>序列化本地版本资源列表（版本 0）是否成功。</returns>
        public static bool SerializeLocalVersionListCallback_V0(BinaryWriter binaryWriter, LocalVersionList versionList)
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
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 反序列化本地版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的本地版本资源列表（版本 0）。</returns>
        public static LocalVersionList DeserializeLocalVersionListCallback_V0(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            int resourceCount = binaryReader.ReadInt32();
            LocalVersionList.Resource[] resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                resources[i] = new LocalVersionList.Resource(name, variant, loadType, length, hashCode);
            }

            return new LocalVersionList(resources);
        }

        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本 0）获取指定键的值回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns></returns>
        public static bool TryGetValueUpdatableVersionListCallback_V0(BinaryReader binaryReader, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            binaryReader.BaseStream.Position += CachedHashBytesLength;
            binaryReader.BaseStream.Position += binaryReader.ReadByte();
            value = binaryReader.ReadInt32();
            return true;
        }

        #endregion Version 0

        #region Version 1

#if UNITY_EDITOR

        /// <summary>
        /// 序列化单机模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的单机模式版本资源列表（版本 1）。</param>
        /// <returns>序列化单机模式版本资源列表（版本 1）是否成功。</returns>
        public static bool SerializePackageVersionListCallback_V1(BinaryWriter binaryWriter, PackageVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            WriteEncryptedString(binaryWriter, versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            PackageVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            foreach (PackageVersionList.Asset asset in assets)
            {
                WriteEncryptedString(binaryWriter, asset.Name, s_CachedHashBytes);
                int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                binaryWriter.Write(dependencyAssetIndexes.Length);
                foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                {
                    binaryWriter.Write(dependencyAssetIndex);
                }
            }

            PackageVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (PackageVersionList.Resource resource in resources)
            {
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write(assetIndexes.Length);
                foreach (int assetIndex in assetIndexes)
                {
                    binaryWriter.Write(assetIndex);
                }
            }

            PackageVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (PackageVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                WriteEncryptedString(binaryWriter, resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write(resourceIndexes.Length);
                foreach (ushort resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write(resourceIndex);
                }
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

#endif

        /// <summary>
        /// 反序列化单机模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的单机模式版本资源列表（版本 1）。</returns>
        public static PackageVersionList DeserializePackageVersionListCallback_V1(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = ReadEncryptedString(binaryReader, encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            PackageVersionList.Asset[] assets = assetCount > 0 ? new PackageVersionList.Asset[assetCount] : null;
            for (int i = 0; i < assetCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                int dependencyAssetCount = binaryReader.ReadInt32();
                int[] dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                for (int j = 0; j < dependencyAssetCount; j++)
                {
                    dependencyAssetIndexes[j] = binaryReader.ReadInt32();
                }

                assets[i] = new PackageVersionList.Asset(name, dependencyAssetIndexes);
            }

            int resourceCount = binaryReader.ReadInt32();
            PackageVersionList.Resource[] resources = resourceCount > 0 ? new PackageVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                int assetIndexCount = binaryReader.ReadInt32();
                int[] assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                for (int j = 0; j < assetIndexCount; j++)
                {
                    assetIndexes[j] = binaryReader.ReadInt32();
                }

                resources[i] = new PackageVersionList.Resource(name, variant, loadType, length, hashCode, assetIndexes);
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            PackageVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new PackageVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                int resourceIndexCount = binaryReader.ReadInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.ReadInt32();
                }

                resourceGroups[i] = new PackageVersionList.ResourceGroup(name, resourceIndexes);
            }

            return new PackageVersionList(applicableGameVersion, internalResourceVersion, assets, resources, resourceGroups);
        }

#if UNITY_EDITOR

        /// <summary>
        /// 序列化可更新模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的可更新模式版本资源列表（版本 1）。</param>
        /// <returns>序列化可更新模式版本资源列表（版本 1）是否成功。</returns>
        public static bool SerializeUpdatableVersionListCallback_V1(BinaryWriter binaryWriter, UpdatableVersionList versionList)
        {
            if (!versionList.IsValid)
            {
                return false;
            }

            Utility.Random.GetRandomBytes(s_CachedHashBytes);
            binaryWriter.Write(s_CachedHashBytes);
            WriteEncryptedString(binaryWriter, versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            UpdatableVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            foreach (UpdatableVersionList.Asset asset in assets)
            {
                WriteEncryptedString(binaryWriter, asset.Name, s_CachedHashBytes);
                int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                binaryWriter.Write(dependencyAssetIndexes.Length);
                foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                {
                    binaryWriter.Write(dependencyAssetIndex);
                }
            }

            UpdatableVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (UpdatableVersionList.Resource resource in resources)
            {
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
                binaryWriter.Write(resource.ZipLength);
                binaryWriter.Write(resource.ZipHashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write(assetIndexes.Length);
                foreach (int assetIndex in assetIndexes)
                {
                    binaryWriter.Write(assetIndex);
                }
            }

            UpdatableVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (UpdatableVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                WriteEncryptedString(binaryWriter, resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write(resourceIndexes.Length);
                foreach (ushort resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write(resourceIndex);
                }
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

#endif

        /// <summary>
        /// 反序列化可更新模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的可更新模式版本资源列表（版本 1）。</returns>
        public static UpdatableVersionList DeserializeUpdatableVersionListCallback_V1(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            string applicableGameVersion = ReadEncryptedString(binaryReader, encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            UpdatableVersionList.Asset[] assets = assetCount > 0 ? new UpdatableVersionList.Asset[assetCount] : null;
            for (int i = 0; i < assetCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                int dependencyAssetCount = binaryReader.ReadInt32();
                int[] dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                for (int j = 0; j < dependencyAssetCount; j++)
                {
                    dependencyAssetIndexes[j] = binaryReader.ReadInt32();
                }

                assets[i] = new UpdatableVersionList.Asset(name, dependencyAssetIndexes);
            }

            int resourceCount = binaryReader.ReadInt32();
            UpdatableVersionList.Resource[] resources = resourceCount > 0 ? new UpdatableVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                int zipLength = binaryReader.ReadInt32();
                int zipHashCode = binaryReader.ReadInt32();
                int assetIndexCount = binaryReader.ReadInt32();
                int[] assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                for (int j = 0; j < assetIndexCount; j++)
                {
                    assetIndexes[j] = binaryReader.ReadInt32();
                }

                resources[i] = new UpdatableVersionList.Resource(name, variant, loadType, length, hashCode, zipLength, zipHashCode, assetIndexes);
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            UpdatableVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new UpdatableVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                int resourceIndexCount = binaryReader.ReadInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.ReadInt32();
                }

                resourceGroups[i] = new UpdatableVersionList.ResourceGroup(name, resourceIndexes);
            }

            return new UpdatableVersionList(applicableGameVersion, internalResourceVersion, assets, resources, resourceGroups);
        }

        /// <summary>
        /// 序列化本地版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的本地版本资源列表（版本 1）。</param>
        /// <returns>序列化本地版本资源列表（版本 1）是否成功。</returns>
        public static bool SerializeLocalVersionListCallback_V1(BinaryWriter binaryWriter, LocalVersionList versionList)
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
                WriteEncryptedString(binaryWriter, resource.Name, s_CachedHashBytes);
                WriteEncryptedString(binaryWriter, resource.Variant, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write(resource.Length);
                binaryWriter.Write(resource.HashCode);
            }

            Array.Clear(s_CachedHashBytes, 0, CachedHashBytesLength);
            return true;
        }

        /// <summary>
        /// 反序列化本地版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的本地版本资源列表（版本 1）。</returns>
        public static LocalVersionList DeserializeLocalVersionListCallback_V1(BinaryReader binaryReader)
        {
            byte[] encryptBytes = binaryReader.ReadBytes(CachedHashBytesLength);
            int resourceCount = binaryReader.ReadInt32();
            LocalVersionList.Resource[] resources = resourceCount > 0 ? new LocalVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = ReadEncryptedString(binaryReader, encryptBytes);
                string variant = ReadEncryptedString(binaryReader, encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                resources[i] = new LocalVersionList.Resource(name, variant, loadType, length, hashCode);
            }

            return new LocalVersionList(resources);
        }

        /// <summary>
        /// 尝试从可更新模式版本资源列表（版本 1）获取指定键的值回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <param name="key">指定键。</param>
        /// <param name="value">指定键的值。</param>
        /// <returns></returns>
        public static bool TryGetValueUpdatableVersionListCallback_V1(BinaryReader binaryReader, string key, out object value)
        {
            value = null;
            if (key != "InternalResourceVersion")
            {
                return false;
            }

            binaryReader.BaseStream.Position += CachedHashBytesLength;
            binaryReader.BaseStream.Position += binaryReader.ReadByte();
            value = binaryReader.ReadInt32();
            return true;
        }

        #endregion Version 1

        public static string ReadEncryptedString(BinaryReader binaryReader, byte[] encryptBytes)
        {
            byte length = binaryReader.ReadByte();
            if (length <= 0)
            {
                return null;
            }

            for (byte i = 0; i < length; i++)
            {
                s_CachedBytesForEncryptedString[i] = binaryReader.ReadByte();
            }

            Utility.Encryption.GetSelfXorBytes(s_CachedBytesForEncryptedString, encryptBytes, length);
            string str = Utility.Converter.GetString(s_CachedBytesForEncryptedString, 0, length);
            Array.Clear(s_CachedBytesForEncryptedString, 0, length);
            return str;
        }

        public static void WriteEncryptedString(BinaryWriter binaryWriter, string str, byte[] encryptBytes)
        {
            if (string.IsNullOrEmpty(str))
            {
                binaryWriter.Write((byte)0);
                return;
            }

            byte[] encryptedStringBytes = Utility.Converter.GetBytes(str);
            if (encryptedStringBytes.Length > byte.MaxValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("String '{0}' is too long.", str));
            }

            Utility.Encryption.GetSelfXorBytes(encryptedStringBytes, encryptBytes);
            binaryWriter.Write((byte)encryptedStringBytes.Length);
            binaryWriter.Write(encryptedStringBytes);
        }
    }
}
