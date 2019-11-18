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

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public sealed class AssetBundleSyncToolsController
    {
        public AssetBundleSyncToolsController()
        {
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnCompleted = null;

        public event GameFrameworkAction<int, int, string> OnAssetBundleDataChanged = null;

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

                if (OnAssetBundleDataChanged != null)
                {
                    OnAssetBundleDataChanged(++assetIndex, assetCount, assetName);
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
            AssetBundleCollection assetBundleCollection = new AssetBundleCollection();

            assetBundleCollection.OnLoadingAssetBundle += delegate (int index, int count)
            {
                if (OnLoadingAssetBundle != null)
                {
                    OnLoadingAssetBundle(index, count);
                }
            };

            assetBundleCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            assetBundleCollection.OnLoadCompleted += delegate ()
            {
                if (OnCompleted != null)
                {
                    OnCompleted();
                }
            };

            if (!assetBundleCollection.Load())
            {
                return false;
            }

            int assetIndex = 0;
            int assetCount = assetBundleCollection.AssetCount;
            AssetBundle[] assetBundles = assetBundleCollection.GetAssetBundles();
            foreach (AssetBundle assetBundle in assetBundles)
            {
                Asset[] assets = assetBundle.GetAssets();
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

                    assetImporter.assetBundleName = assetBundle.Name;
                    assetImporter.assetBundleVariant = assetBundle.Variant;
                    assetImporter.SaveAndReimport();

                    if (OnAssetBundleDataChanged != null)
                    {
                        OnAssetBundleDataChanged(++assetIndex, assetCount, asset.Name);
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
            AssetBundleCollection assetBundleCollection = new AssetBundleCollection();
            string[] assetBundleNames = GetUsedAssetBundleNames();
            foreach (string assetBundleFullName in assetBundleNames)
            {
                string assetBundleName = assetBundleFullName;
                string assetBundleVariant = null;
                int dotPosition = assetBundleFullName.LastIndexOf('.');
                if (dotPosition > 0 && dotPosition < assetBundleFullName.Length - 1)
                {
                    assetBundleName = assetBundleFullName.Substring(0, dotPosition);
                    assetBundleVariant = assetBundleFullName.Substring(dotPosition + 1);
                }

                if (!assetBundleCollection.AddAssetBundle(assetBundleName, assetBundleVariant, AssetBundleLoadType.LoadFromFile, false))
                {
                    return false;
                }

                string[] assetNames = GetAssetPathsFromAssetBundle(assetBundleFullName);
                foreach (string assetName in assetNames)
                {
                    string assetGuid = AssetDatabase.AssetPathToGUID(assetName);
                    if (string.IsNullOrEmpty(assetGuid))
                    {
                        return false;
                    }

                    if (!assetBundleCollection.AssignAsset(assetGuid, assetBundleName, assetBundleVariant))
                    {
                        return false;
                    }
                }
            }

            return assetBundleCollection.Save();
        }
    }
}
