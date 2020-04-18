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
        private const string DefaultExtension = "dat";
        private const int CachedHashBytesLength = 4;
        private static readonly byte[] s_CachedHashBytes = new byte[CachedHashBytesLength];

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
            binaryWriter.WriteEncryptedString(versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            PackageVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            PackageVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (PackageVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
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
                    binaryWriter.WriteEncryptedString(asset.Name, hashBytes);
                    int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                    binaryWriter.Write(dependencyAssetIndexes.Length);
                    foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        binaryWriter.WriteEncryptedString(assets[dependencyAssetIndex].Name, hashBytes);
                    }
                }
            }

            PackageVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (PackageVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                binaryWriter.WriteEncryptedString(resourceGroup.Name, s_CachedHashBytes);
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
            string applicableGameVersion = binaryReader.ReadEncryptedString(encryptBytes);
            int internalResourceVersion = binaryReader.ReadInt32();
            int assetCount = binaryReader.ReadInt32();
            PackageVersionList.Asset[] assets = assetCount > 0 ? new PackageVersionList.Asset[assetCount] : null;
            int resourceCount = binaryReader.ReadInt32();
            PackageVersionList.Resource[] resources = resourceCount > 0 ? new PackageVersionList.Resource[resourceCount] : null;
            string[][] resourceToAssetNames = new string[resourceCount][];
            List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames = new List<KeyValuePair<string, string[]>>(assetCount);
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.ReadInt32();
                int hashCode = binaryReader.ReadInt32();
                Utility.Converter.GetBytes(hashCode, s_CachedHashBytes);

                int assetNameCount = binaryReader.ReadInt32();
                string[] assetNames = new string[assetNameCount];
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
                resources[i] = new PackageVersionList.Resource(name, variant, null, loadType, length, hashCode, assetNameCount > 0 ? new int[assetNameCount] : null);
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
                    assetIndexes[j] = GetAssetNameIndex(assetNameToDependencyAssetNames, resourceToAssetNames[i][j]);
                }
            }

            int resourceGroupCount = binaryReader.ReadInt32();
            PackageVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new PackageVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
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
            binaryWriter.WriteEncryptedString(versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write(versionList.InternalResourceVersion);
            UpdatableVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write(assets.Length);
            UpdatableVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write(resources.Length);
            foreach (UpdatableVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
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
                    binaryWriter.WriteEncryptedString(asset.Name, hashBytes);
                    int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                    binaryWriter.Write(dependencyAssetIndexes.Length);
                    foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                    {
                        binaryWriter.WriteEncryptedString(assets[dependencyAssetIndex].Name, hashBytes);
                    }
                }
            }

            UpdatableVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write(resourceGroups.Length);
            foreach (UpdatableVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                binaryWriter.WriteEncryptedString(resourceGroup.Name, s_CachedHashBytes);
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

        private static int AssetNameToDependencyAssetNamesComparer(KeyValuePair<string, string[]> a, KeyValuePair<string, string[]> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private static int GetAssetNameIndex(List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames, string assetName)
        {
            return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, 0, assetNameToDependencyAssetNames.Count - 1);
        }

        private static int GetAssetNameIndexWithBinarySearch(List<KeyValuePair<string, string[]>> assetNameToDependencyAssetNames, string assetName, int leftIndex, int rightIndex)
        {
            if (leftIndex > rightIndex)
            {
                return -1;
            }

            int middleIndex = (leftIndex + rightIndex) / 2;
            if (assetNameToDependencyAssetNames[middleIndex].Key == assetName)
            {
                return middleIndex;
            }

            if (assetNameToDependencyAssetNames[middleIndex].Key.CompareTo(assetName) > 0)
            {
                return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, leftIndex, middleIndex - 1);
            }
            else
            {
                return GetAssetNameIndexWithBinarySearch(assetNameToDependencyAssetNames, assetName, middleIndex + 1, rightIndex);
            }
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
            binaryWriter.WriteEncryptedString(versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write7BitEncodedInt32(versionList.InternalResourceVersion);
            PackageVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write7BitEncodedInt32(assets.Length);
            foreach (PackageVersionList.Asset asset in assets)
            {
                binaryWriter.WriteEncryptedString(asset.Name, s_CachedHashBytes);
                int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                binaryWriter.Write7BitEncodedInt32(dependencyAssetIndexes.Length);
                foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(dependencyAssetIndex);
                }
            }

            PackageVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write7BitEncodedInt32(resources.Length);
            foreach (PackageVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write7BitEncodedInt32(resource.Length);
                binaryWriter.Write(resource.HashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write7BitEncodedInt32(assetIndexes.Length);
                foreach (int assetIndex in assetIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(assetIndex);
                }
            }

            PackageVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write7BitEncodedInt32(resourceGroups.Length);
            foreach (PackageVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                binaryWriter.WriteEncryptedString(resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write7BitEncodedInt32(resourceIndexes.Length);
                foreach (int resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(resourceIndex);
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
            string applicableGameVersion = binaryReader.ReadEncryptedString(encryptBytes);
            int internalResourceVersion = binaryReader.Read7BitEncodedInt32();
            int assetCount = binaryReader.Read7BitEncodedInt32();
            PackageVersionList.Asset[] assets = assetCount > 0 ? new PackageVersionList.Asset[assetCount] : null;
            for (int i = 0; i < assetCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                int dependencyAssetCount = binaryReader.Read7BitEncodedInt32();
                int[] dependencyAssetIndexes = dependencyAssetCount > 0 ? new int[dependencyAssetCount] : null;
                for (int j = 0; j < dependencyAssetCount; j++)
                {
                    dependencyAssetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                }

                assets[i] = new PackageVersionList.Asset(name, dependencyAssetIndexes);
            }

            int resourceCount = binaryReader.Read7BitEncodedInt32();
            PackageVersionList.Resource[] resources = resourceCount > 0 ? new PackageVersionList.Resource[resourceCount] : null;
            for (int i = 0; i < resourceCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                string variant = binaryReader.ReadEncryptedString(encryptBytes);
                string extension = binaryReader.ReadEncryptedString(encryptBytes) ?? DefaultExtension;
                byte loadType = binaryReader.ReadByte();
                int length = binaryReader.Read7BitEncodedInt32();
                int hashCode = binaryReader.ReadInt32();
                int assetIndexCount = binaryReader.Read7BitEncodedInt32();
                int[] assetIndexes = assetIndexCount > 0 ? new int[assetIndexCount] : null;
                for (int j = 0; j < assetIndexCount; j++)
                {
                    assetIndexes[j] = binaryReader.Read7BitEncodedInt32();
                }

                resources[i] = new PackageVersionList.Resource(name, variant, extension, loadType, length, hashCode, assetIndexes);
            }

            int resourceGroupCount = binaryReader.Read7BitEncodedInt32();
            PackageVersionList.ResourceGroup[] resourceGroups = resourceGroupCount > 0 ? new PackageVersionList.ResourceGroup[resourceGroupCount] : null;
            for (int i = 0; i < resourceGroupCount; i++)
            {
                string name = binaryReader.ReadEncryptedString(encryptBytes);
                int resourceIndexCount = binaryReader.Read7BitEncodedInt32();
                int[] resourceIndexes = resourceIndexCount > 0 ? new int[resourceIndexCount] : null;
                for (int j = 0; j < resourceIndexCount; j++)
                {
                    resourceIndexes[j] = binaryReader.Read7BitEncodedInt32();
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
            binaryWriter.WriteEncryptedString(versionList.ApplicableGameVersion, s_CachedHashBytes);
            binaryWriter.Write7BitEncodedInt32(versionList.InternalResourceVersion);
            UpdatableVersionList.Asset[] assets = versionList.GetAssets();
            binaryWriter.Write7BitEncodedInt32(assets.Length);
            foreach (UpdatableVersionList.Asset asset in assets)
            {
                binaryWriter.WriteEncryptedString(asset.Name, s_CachedHashBytes);
                int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                binaryWriter.Write7BitEncodedInt32(dependencyAssetIndexes.Length);
                foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(dependencyAssetIndex);
                }
            }

            UpdatableVersionList.Resource[] resources = versionList.GetResources();
            binaryWriter.Write7BitEncodedInt32(resources.Length);
            foreach (UpdatableVersionList.Resource resource in resources)
            {
                binaryWriter.WriteEncryptedString(resource.Name, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Variant, s_CachedHashBytes);
                binaryWriter.WriteEncryptedString(resource.Extension != DefaultExtension ? resource.Extension : null, s_CachedHashBytes);
                binaryWriter.Write(resource.LoadType);
                binaryWriter.Write7BitEncodedInt32(resource.Length);
                binaryWriter.Write(resource.HashCode);
                binaryWriter.Write7BitEncodedInt32(resource.ZipLength);
                binaryWriter.Write(resource.ZipHashCode);
                int[] assetIndexes = resource.GetAssetIndexes();
                binaryWriter.Write7BitEncodedInt32(assetIndexes.Length);
                foreach (int assetIndex in assetIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(assetIndex);
                }
            }

            UpdatableVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
            binaryWriter.Write7BitEncodedInt32(resourceGroups.Length);
            foreach (UpdatableVersionList.ResourceGroup resourceGroup in resourceGroups)
            {
                binaryWriter.WriteEncryptedString(resourceGroup.Name, s_CachedHashBytes);
                int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                binaryWriter.Write7BitEncodedInt32(resourceIndexes.Length);
                foreach (int resourceIndex in resourceIndexes)
                {
                    binaryWriter.Write7BitEncodedInt32(resourceIndex);
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

        /// <summary>
        /// 反序列化本地版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryReader">指定流。</param>
        /// <returns>反序列化的本地版本资源列表（版本 1）。</returns>
        public static LocalVersionList DeserializeLocalVersionListCallback_V1(BinaryReader binaryReader)
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
            value = binaryReader.Read7BitEncodedInt32();
            return true;
        }

        #endregion Version 1
    }
}
