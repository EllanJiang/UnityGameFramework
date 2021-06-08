//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.ResourceTools
{
    /// <summary>
    /// 资源同步工具。
    /// </summary>
    internal sealed class ResourceSyncTools : EditorWindow
    {
        private const float ButtonHeight = 60f;
        private const float ButtonSpace = 5f;
        private ResourceSyncToolsController m_Controller = null;

        [MenuItem("Game Framework/Resource Tools/Resource Sync Tools", false, 44)]
        private static void Open()
        {
            ResourceSyncTools window = GetWindow<ResourceSyncTools>("Resource Sync Tools", true);
#if UNITY_2019_3_OR_NEWER
            window.minSize = new Vector2(400, 195f);
#else
            window.minSize = new Vector2(400, 205f);
#endif
        }

        private void OnEnable()
        {
            m_Controller = new ResourceSyncToolsController();
            m_Controller.OnLoadingResource += OnLoadingResource;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnCompleted += OnCompleted;
            m_Controller.OnResourceDataChanged += OnResourceDataChanged;
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
                        Debug.LogWarning("Remove All Asset Bundle Names in Project failure.");
                    }
                    else
                    {
                        Debug.Log("Remove All Asset Bundle Names in Project completed.");
                    }

                    AssetDatabase.Refresh();
                }

                GUILayout.Space(ButtonSpace);
                if (GUILayout.Button("Sync ResourceCollection.xml to Project", GUILayout.Height(ButtonHeight)))
                {
                    if (!m_Controller.SyncToProject())
                    {
                        Debug.LogWarning("Sync ResourceCollection.xml to Project failure.");
                    }
                    else
                    {
                        Debug.Log("Sync ResourceCollection.xml to Project completed.");
                    }

                    AssetDatabase.Refresh();
                }

                GUILayout.Space(ButtonSpace);
                if (GUILayout.Button("Sync ResourceCollection.xml from Project", GUILayout.Height(ButtonHeight)))
                {
                    if (!m_Controller.SyncFromProject())
                    {
                        Debug.LogWarning("Sync Project to ResourceCollection.xml failure.");
                    }
                    else
                    {
                        Debug.Log("Sync Project to ResourceCollection.xml completed.");
                    }

                    AssetDatabase.Refresh();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnLoadingResource(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Resources", Utility.Text.Format("Loading resources, {0}/{1} loaded.", index, count), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", Utility.Text.Format("Loading assets, {0}/{1} loaded.", index, count), (float)index / count);
        }

        private void OnCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnResourceDataChanged(int index, int count, string assetName)
        {
            EditorUtility.DisplayProgressBar("Processing Assets", Utility.Text.Format("({0}/{1}) {2}", index, count, assetName), (float)index / count);
        }
    }
}
