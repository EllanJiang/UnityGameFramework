//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed class DependencyData
    {
        private List<Resource> m_DependencyResources;
        private List<Asset> m_DependencyAssets;
        private List<string> m_ScatteredDependencyAssetNames;

        public DependencyData()
        {
            m_DependencyResources = new List<Resource>();
            m_DependencyAssets = new List<Asset>();
            m_ScatteredDependencyAssetNames = new List<string>();
        }

        public int DependencyResourceCount
        {
            get
            {
                return m_DependencyResources.Count;
            }
        }

        public int DependencyAssetCount
        {
            get
            {
                return m_DependencyAssets.Count;
            }
        }

        public int ScatteredDependencyAssetCount
        {
            get
            {
                return m_ScatteredDependencyAssetNames.Count;
            }
        }

        public void AddDependencyAsset(Asset asset)
        {
            if (!m_DependencyResources.Contains(asset.Resource))
            {
                m_DependencyResources.Add(asset.Resource);
            }

            m_DependencyAssets.Add(asset);
        }

        public void AddScatteredDependencyAsset(string dependencyAssetName)
        {
            m_ScatteredDependencyAssetNames.Add(dependencyAssetName);
        }

        public Resource[] GetDependencyResources()
        {
            return m_DependencyResources.ToArray();
        }

        public Asset[] GetDependencyAssets()
        {
            return m_DependencyAssets.ToArray();
        }

        public string[] GetScatteredDependencyAssetNames()
        {
            return m_ScatteredDependencyAssetNames.ToArray();
        }

        public void RefreshData()
        {
            m_DependencyResources.Sort(DependencyResourcesComparer);
            m_DependencyAssets.Sort(DependencyAssetsComparer);
            m_ScatteredDependencyAssetNames.Sort();
        }

        private int DependencyResourcesComparer(Resource a, Resource b)
        {
            return a.FullName.CompareTo(b.FullName);
        }

        private int DependencyAssetsComparer(Asset a, Asset b)
        {
            return a.Name.CompareTo(b.Name);
        }
    }
}
