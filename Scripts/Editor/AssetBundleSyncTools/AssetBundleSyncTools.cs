//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包同步工具。
    /// </summary>
    internal sealed class AssetBundleSyncTools : EditorWindow
    {
        private const float ButtonHeight = 60f;
        private const float ButtonSpace = 5f;
        private AssetBundleSyncToolsController m_Controller = null;

        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Sync Tools", false, 44)]
        private static void Open()
        {
            AssetBundleSyncTools window = GetWindow<AssetBundleSyncTools>("AB Sync Tools", true);
            window.minSize = new Vector2(400, 205f);
        }

        private void OnEnable()
        {
            m_Controller = new AssetBundleSyncToolsController();
            m_Controller.OnLoadingAssetBundle += OnLoadingAssetBundle;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnCompleted += OnCompleted;
            m_Controller.OnAssetBundleDataChanged += OnAssetBundleDataChanged;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(ButtonSpace);
                if (GUILayout.Button("Remove All Asset Bundle Names in Project", GUILayout.Height(ButtonHeight)))
                {
                    if (!m_Controller.RemoveAllAssetBundleNames())
                    {
                        Debug.LogWarning("Remove All Asset Bundle Names in Project failed.");
                    }
                    else
                    {
                        Debug.Log("Remove All Asset Bundle Names in Project completed.");
                    }

                    AssetDatabase.Refresh();
                }

                GUILayout.Space(ButtonSpace);
                if (GUILayout.Button("Sync AssetBundleCollection.xml to Project", GUILayout.Height(ButtonHeight)))
                {
                    if (!m_Controller.SyncToProject())
                    {
                        Debug.LogWarning("Sync AssetBundleCollection.xml to Project failed.");
                    }
                    else
                    {
                        Debug.Log("Sync AssetBundleCollection.xml to Project completed.");
                    }

                    AssetDatabase.Refresh();
                }

                GUILayout.Space(ButtonSpace);
                if (GUILayout.Button("Sync AssetBundleCollection.xml from Project", GUILayout.Height(ButtonHeight)))
                {
                    if (!m_Controller.SyncFromProject())
                    {
                        Debug.LogWarning("Sync Project to AssetBundleCollection.xml failed.");
                    }
                    else
                    {
                        Debug.Log("Sync Project to AssetBundleCollection.xml completed.");
                    }

                    AssetDatabase.Refresh();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnLoadingAssetBundle(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading AssetBundles", Utility.Text.Format("Loading AssetBundles, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", Utility.Text.Format("Loading assets, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnAssetBundleDataChanged(int index, int count, string assetName)
        {
            EditorUtility.DisplayProgressBar("Processing Assets", Utility.Text.Format("({0}/{1}) {2}", index.ToString(), count.ToString(), assetName), (float)index / count);
        }
    }
}
