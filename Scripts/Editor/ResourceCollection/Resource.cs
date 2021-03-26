//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;

namespace UnityGameFramework.Editor.ResourceTools
{
    /// <summary>
    /// 资源。
    /// </summary>
    public sealed class Resource
    {
        private readonly List<Asset> m_Assets;
        private readonly List<string> m_ResourceGroups;

        private Resource(string name, string variant, string fileSystem, LoadType loadType, bool packed, string[] resourceGroups)
        {
            m_Assets = new List<Asset>();
            m_ResourceGroups = new List<string>();

            Name = name;
            Variant = variant;
            AssetType = AssetType.Unknown;
            FileSystem = fileSystem;
            LoadType = loadType;
            Packed = packed;

            foreach (string resourceGroup in resourceGroups)
            {
                AddResourceGroup(resourceGroup);
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public string Variant
        {
            get;
            private set;
        }

        public string FullName
        {
            get
            {
                return Variant != null ? Utility.Text.Format("{0}.{1}", Name, Variant) : Name;
            }
        }

        public AssetType AssetType
        {
            get;
            private set;
        }

        public bool IsLoadFromBinary
        {
            get
            {
                return LoadType == LoadType.LoadFromBinary || LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || LoadType == LoadType.LoadFromBinaryAndDecrypt;
            }
        }

        public string FileSystem
        {
            get;
            set;
        }

        public LoadType LoadType
        {
            get;
            set;
        }

        public bool Packed
        {
            get;
            set;
        }

        public static Resource Create(string name, string variant, string fileSystem, LoadType loadType, bool packed, string[] resourceGroups)
        {
            return new Resource(name, variant, fileSystem, loadType, packed, resourceGroups ?? new string[0]);
        }

        public Asset[] GetAssets()
        {
            return m_Assets.ToArray();
        }

        public Asset GetFirstAsset()
        {
            return m_Assets.Count > 0 ? m_Assets[0] : null;
        }

        public void Rename(string name, string variant)
        {
            Name = name;
            Variant = variant;
        }

        public void AssignAsset(Asset asset, bool isScene)
        {
            if (asset.Resource != null)
            {
                asset.Resource.UnassignAsset(asset);
            }

            AssetType = isScene ? AssetType.Scene : AssetType.Asset;
            asset.Resource = this;
            m_Assets.Add(asset);
            m_Assets.Sort(AssetComparer);
        }

        public void UnassignAsset(Asset asset)
        {
            asset.Resource = null;
            m_Assets.Remove(asset);
            if (m_Assets.Count <= 0)
            {
                AssetType = AssetType.Unknown;
            }
        }

        public string[] GetResourceGroups()
        {
            return m_ResourceGroups.ToArray();
        }

        public bool HasResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return false;
            }

            return m_ResourceGroups.Contains(resourceGroup);
        }

        public void AddResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return;
            }

            if (m_ResourceGroups.Contains(resourceGroup))
            {
                return;
            }

            m_ResourceGroups.Add(resourceGroup);
            m_ResourceGroups.Sort();
        }

        public bool RemoveResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
            {
                return false;
            }

            return m_ResourceGroups.Remove(resourceGroup);
        }

        public void Clear()
        {
            foreach (Asset asset in m_Assets)
            {
                asset.Resource = null;
            }

            m_Assets.Clear();
            m_ResourceGroups.Clear();
        }

        private int AssetComparer(Asset a, Asset b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}
