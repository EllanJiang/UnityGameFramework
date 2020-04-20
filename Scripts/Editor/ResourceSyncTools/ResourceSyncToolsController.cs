//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed class ResourceSyncToolsController
    {
        public ResourceSyncToolsController()
        {
        }

        public event GameFrameworkAction<int, int> OnLoadingResource = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnCompleted = null;

        public event GameFrameworkAction<int, int, string> OnResourceDataChanged = null;

        public string[] GetAllAssetBundleNames()
        {
            return AssetDatabase.GetAllAssetBundleNames();
        }

        public string[] GetUsedAssetBundleNames()
        {
            HashSet<string> hashSet = new HashSet<string>(GetAllAssetBundleNames());
            hashSet.ExceptWith(GetUnusedAssetBundleNames());
            return hashSet.ToArray();
        }

        public string[] GetUnusedAssetBundleNames()
        {
            return AssetDatabase.GetUnusedAssetBundleNames();
        }

        public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
        {
            return AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
        }

        public string[] GetAssetPathsFromAssetBundleAndAssetName(string assetBundleName, string assetName)
        {
            return AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
        }

        public bool RemoveAssetBundleName(string assetBundleName, bool forceRemove)
        {
            return AssetDatabase.RemoveAssetBundleName(assetBundleName, forceRemove);
        }

        public void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        public bool RemoveAllAssetBundleNames()
        {
            HashSet<string> allAssetNames = new HashSet<string>();
            string[] assetBundleNames = GetUsedAssetBundleNames();
            foreach (string assetBundleName in assetBundleNames)
            {
                string[] assetNames = GetAssetPathsFromAssetBundle(assetBundleName);
                foreach (string assetName in assetNames)
                {
                    allAssetNames.Add(assetName);
                }
            }

            int assetIndex = 0;
            int assetCount = allAssetNames.Count;
            foreach (string assetName in allAssetNames)
            {
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetName);
                if (assetImporter == null)
                {
                    if (OnCompleted != null)
                    {
                        OnCompleted();
                    }

                    return false;
                }

                assetImporter.assetBundleVariant = null;
                assetImporter.assetBundleName = null;
                assetImporter.SaveAndReimport();

                if (OnResourceDataChanged != null)
                {
                    OnResourceDataChanged(++assetIndex, assetCount, assetName);
                }
            }

            RemoveUnusedAssetBundleNames();

            if (OnCompleted != null)
            {
                OnCompleted();
            }

            return true;
        }

        public bool SyncToProject()
        {
            ResourceCollection resourceCollection = new ResourceCollection();

            resourceCollection.OnLoadingResource += delegate (int index, int count)
            {
                if (OnLoadingResource != null)
                {
                    OnLoadingResource(index, count);
                }
            };

            resourceCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            resourceCollection.OnLoadCompleted += delegate ()
            {
                if (OnCompleted != null)
                {
                    OnCompleted();
                }
            };

            if (!resourceCollection.Load())
            {
                return false;
            }

            int assetIndex = 0;
            int assetCount = resourceCollection.AssetCount;
            Resource[] resources = resourceCollection.GetResources();
            foreach (Resource resource in resources)
            {
                if (resource.IsLoadFromBinary)
                {
                    continue;
                }

                Asset[] assets = resource.GetAssets();
                foreach (Asset asset in assets)
                {
                    AssetImporter assetImporter = AssetImporter.GetAtPath(asset.Name);
                    if (assetImporter == null)
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }

                        return false;
                    }

                    assetImporter.assetBundleName = resource.Name;
                    assetImporter.assetBundleVariant = resource.Variant;
                    assetImporter.SaveAndReimport();

                    if (OnResourceDataChanged != null)
                    {
                        OnResourceDataChanged(++assetIndex, assetCount, asset.Name);
                    }
                }
            }

            if (OnCompleted != null)
            {
                OnCompleted();
            }

            return true;
        }

        public bool SyncFromProject()
        {
            ResourceCollection resourceCollection = new ResourceCollection();
            string[] assetBundleNames = GetUsedAssetBundleNames();
            foreach (string assetBundleName in assetBundleNames)
            {
                string name = assetBundleName;
                string variant = null;
                int dotPosition = assetBundleName.LastIndexOf('.');
                if (dotPosition > 0 && dotPosition < assetBundleName.Length - 1)
                {
                    name = assetBundleName.Substring(0, dotPosition);
                    variant = assetBundleName.Substring(dotPosition + 1);
                }

                if (!resourceCollection.AddResource(name, variant, LoadType.LoadFromFile, false))
                {
                    return false;
                }

                string[] assetNames = GetAssetPathsFromAssetBundle(assetBundleName);
                foreach (string assetName in assetNames)
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetName);
                    if (string.IsNullOrEmpty(guid))
                    {
                        return false;
                    }

                    if (!resourceCollection.AssignAsset(guid, name, variant))
                    {
                        return false;
                    }
                }
            }

            return resourceCollection.Save();
        }
    }
}
