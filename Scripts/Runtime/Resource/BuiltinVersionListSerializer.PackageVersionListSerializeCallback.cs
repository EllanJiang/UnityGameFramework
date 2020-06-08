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
        /// 序列化单机模式版本资源列表（版本 0）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的单机模式版本资源列表（版本 0）。</param>
        /// <returns>序列化单机模式版本资源列表（版本 0）是否成功。</returns>
        public static bool PackageVersionListSerializeCallback_V0(BinaryWriter binaryWriter, PackageVersionList versionList)
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

        /// <summary>
        /// 序列化单机模式版本资源列表（版本 1）回调函数。
        /// </summary>
        /// <param name="binaryWriter">目标流。</param>
        /// <param name="versionList">要序列化的单机模式版本资源列表（版本 1）。</param>
        /// <returns>序列化单机模式版本资源列表（版本 1）是否成功。</returns>
        public static bool PackageVersionListSerializeCallback_V1(BinaryWriter binaryWriter, PackageVersionList versionList)
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
    }
}
