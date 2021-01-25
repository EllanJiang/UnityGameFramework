//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.ResourceTools
{
    /// <summary>
    /// 资源编辑器。
    /// </summary>
    internal sealed partial class ResourceEditor : EditorWindow
    {
        private ResourceEditorController m_Controller = null;
        private MenuState m_MenuState = MenuState.Normal;
        private Resource m_SelectedResource = null;
        private ResourceFolder m_ResourceRoot = null;
        private HashSet<string> m_ExpandedResourceFolderNames = null;
        private HashSet<Asset> m_SelectedAssetsInSelectedResource = null;
        private HashSet<SourceFolder> m_ExpandedSourceFolders = null;
        private HashSet<SourceAsset> m_SelectedSourceAssets = null;
        private Texture m_MissingSourceAssetIcon = null;

        private HashSet<SourceFolder> m_CachedSelectedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedUnselectedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedAssignedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedUnassignedSourceFolders = null;
        private HashSet<SourceAsset> m_CachedAssignedSourceAssets = null;
        private HashSet<SourceAsset> m_CachedUnassignedSourceAssets = null;

        private Vector2 m_ResourcesViewScroll = Vector2.zero;
        private Vector2 m_ResourceViewScroll = Vector2.zero;
        private Vector2 m_SourceAssetsViewScroll = Vector2.zero;
        private string m_InputResourceName = null;
        private string m_InputResourceVariant = null;
        private bool m_HideAssignedSourceAssets = false;
        private int m_CurrentResourceContentCount = 0;
        private int m_CurrentResourceRowOnDraw = 0;
        private int m_CurrentSourceRowOnDraw = 0;

        [MenuItem("Game Framework/Resource Tools/Resource Editor", false, 41)]
        private static void Open()
        {
            ResourceEditor window = GetWindow<ResourceEditor>("Resource Editor", true);
            window.minSize = new Vector2(1400f, 600f);
        }

        private void OnEnable()
        {
            m_Controller = new ResourceEditorController();
            m_Controller.OnLoadingResource += OnLoadingResource;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnLoadCompleted += OnLoadCompleted;
            m_Controller.OnAssetAssigned += OnAssetAssigned;
            m_Controller.OnAssetUnassigned += OnAssetUnassigned;

            m_MenuState = MenuState.Normal;
            m_SelectedResource = null;
            m_ResourceRoot = new ResourceFolder("Resources", null);
            m_ExpandedResourceFolderNames = new HashSet<string>();
            m_SelectedAssetsInSelectedResource = new HashSet<Asset>();
            m_ExpandedSourceFolders = new HashSet<SourceFolder>();
            m_SelectedSourceAssets = new HashSet<SourceAsset>();
            m_MissingSourceAssetIcon = EditorGUIUtility.IconContent("console.warnicon.sml").image;

            m_CachedSelectedSourceFolders = new HashSet<SourceFolder>();
            m_CachedUnselectedSourceFolders = new HashSet<SourceFolder>();
            m_CachedAssignedSourceFolders = new HashSet<SourceFolder>();
            m_CachedUnassignedSourceFolders = new HashSet<SourceFolder>();
            m_CachedAssignedSourceAssets = new HashSet<SourceAsset>();
            m_CachedUnassignedSourceAssets = new HashSet<SourceAsset>();

            m_ResourcesViewScroll = Vector2.zero;
            m_ResourceViewScroll = Vector2.zero;
            m_SourceAssetsViewScroll = Vector2.zero;
            m_InputResourceName = null;
            m_InputResourceVariant = null;
            m_HideAssignedSourceAssets = false;
            m_CurrentResourceContentCount = 0;
            m_CurrentResourceRowOnDraw = 0;
            m_CurrentSourceRowOnDraw = 0;

            if (m_Controller.Load())
            {
                Debug.Log("Load configuration success.");
            }
            else
            {
                Debug.LogWarning("Load configuration failure.");
            }

            EditorUtility.DisplayProgressBar("Prepare Resource Editor", "Processing...", 0f);
            RefreshResourceTree();
            EditorUtility.ClearProgressBar();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(2f);
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField(Utility.Text.Format("Resource List ({0})", m_Controller.ResourceCount.ToString()), EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawResourcesView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawResourcesMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField(Utility.Text.Format("Resource Content ({0})", m_CurrentResourceContentCount.ToString()), EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawResourceView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawResourceMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f - 16f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField("Asset List", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawSourceAssetsView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawSourceAssetsMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(5f);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawResourcesView()
        {
            m_CurrentResourceRowOnDraw = 0;
            m_ResourcesViewScroll = EditorGUILayout.BeginScrollView(m_ResourcesViewScroll);
            {
                DrawResourceFolder(m_ResourceRoot);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawResourceFolder(ResourceFolder folder)
        {
            bool expand = IsExpandedResourceFolder(folder);
            EditorGUILayout.BeginHorizontal();
            {
#if UNITY_2019_3_OR_NEWER
                bool foldout = EditorGUI.Foldout(new Rect(18f + 14f * folder.Depth, 20f * m_CurrentResourceRowOnDraw + 4f, int.MaxValue, 14f), expand, string.Empty, true);
#else
                bool foldout = EditorGUI.Foldout(new Rect(18f + 14f * folder.Depth, 20f * m_CurrentResourceRowOnDraw + 2f, int.MaxValue, 14f), expand, string.Empty, true);
#endif
                if (expand != foldout)
                {
                    expand = !expand;
                    SetExpandedResourceFolder(folder, expand);
                }

#if UNITY_2019_3_OR_NEWER
                GUI.DrawTexture(new Rect(32f + 14f * folder.Depth, 20f * m_CurrentResourceRowOnDraw + 3f, 16f, 16f), ResourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(44f + 14f * folder.Depth), GUILayout.Height(18f));
#else
                GUI.DrawTexture(new Rect(32f + 14f * folder.Depth, 20f * m_CurrentResourceRowOnDraw + 1f, 16f, 16f), ResourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(40f + 14f * folder.Depth), GUILayout.Height(18f));
#endif
                EditorGUILayout.LabelField(folder.Name);
            }
            EditorGUILayout.EndHorizontal();

            m_CurrentResourceRowOnDraw++;

            if (expand)
            {
                foreach (ResourceFolder subFolder in folder.GetFolders())
                {
                    DrawResourceFolder(subFolder);
                }

                foreach (ResourceItem resourceItem in folder.GetItems())
                {
                    DrawResourceItem(resourceItem);
                }
            }
        }

        private void DrawResourceItem(ResourceItem resourceItem)
        {
            EditorGUILayout.BeginHorizontal();
            {
                string title = resourceItem.Name;
                if (resourceItem.Resource.Packed)
                {
                    title = "[Packed] " + title;
                }

                float emptySpace = position.width;
                if (EditorGUILayout.Toggle(m_SelectedResource == resourceItem.Resource, GUILayout.Width(emptySpace - 12f)))
                {
                    ChangeSelectedResource(resourceItem.Resource);
                }
                else if (m_SelectedResource == resourceItem.Resource)
                {
                    ChangeSelectedResource(null);
                }

                GUILayout.Space(-emptySpace + 24f);
#if UNITY_2019_3_OR_NEWER
                GUI.DrawTexture(new Rect(32f + 14f * resourceItem.Depth, 20f * m_CurrentResourceRowOnDraw + 3f, 16f, 16f), resourceItem.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(30f + 14f * resourceItem.Depth), GUILayout.Height(18f));
#else
                GUI.DrawTexture(new Rect(32f + 14f * resourceItem.Depth, 20f * m_CurrentResourceRowOnDraw + 1f, 16f, 16f), resourceItem.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * resourceItem.Depth), GUILayout.Height(18f));
#endif
                EditorGUILayout.LabelField(title);
            }
            EditorGUILayout.EndHorizontal();
            m_CurrentResourceRowOnDraw++;
        }

        private void DrawResourcesMenu()
        {
            switch (m_MenuState)
            {
                case MenuState.Normal:
                    DrawResourcesMenu_Normal();
                    break;

                case MenuState.Add:
                    DrawResourcesMenu_Add();
                    break;

                case MenuState.Rename:
                    DrawResourcesMenu_Rename();
                    break;

                case MenuState.Remove:
                    DrawResourcesMenu_Remove();
                    break;
            }
        }

        private void DrawResourcesMenu_Normal()
        {
            if (GUILayout.Button("Add", GUILayout.Width(65f)))
            {
                m_MenuState = MenuState.Add;
                m_InputResourceName = null;
                m_InputResourceVariant = null;
                GUI.FocusControl(null);
            }
            EditorGUI.BeginDisabledGroup(m_SelectedResource == null);
            {
                if (GUILayout.Button("Rename", GUILayout.Width(65f)))
                {
                    m_MenuState = MenuState.Rename;
                    m_InputResourceName = m_SelectedResource != null ? m_SelectedResource.Name : null;
                    m_InputResourceVariant = m_SelectedResource != null ? m_SelectedResource.Variant : null;
                    GUI.FocusControl(null);
                }
                if (GUILayout.Button("Remove", GUILayout.Width(65f)))
                {
                    m_MenuState = MenuState.Remove;
                }
                if (m_SelectedResource == null)
                {
                    EditorGUILayout.EnumPopup(LoadType.LoadFromFile);
                }
                else
                {
                    LoadType loadType = (LoadType)EditorGUILayout.EnumPopup(m_SelectedResource.LoadType);
                    if (loadType != m_SelectedResource.LoadType)
                    {
                        SetResourceLoadType(loadType);
                    }
                }
                bool packed = EditorGUILayout.ToggleLeft("Packed", m_SelectedResource != null && m_SelectedResource.Packed, GUILayout.Width(65f));
                if (m_SelectedResource != null && packed != m_SelectedResource.Packed)
                {
                    SetResourcePacked(packed);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawResourcesMenu_Add()
        {
            GUI.SetNextControlName("NewResourceNameTextField");
            m_InputResourceName = EditorGUILayout.TextField(m_InputResourceName);
            GUI.SetNextControlName("NewResourceVariantTextField");
            m_InputResourceVariant = EditorGUILayout.TextField(m_InputResourceVariant, GUILayout.Width(60f));

            if (GUI.GetNameOfFocusedControl() == "NewResourceNameTextField" || GUI.GetNameOfFocusedControl() == "NewResourceVariantTextField")
            {
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                {
                    EditorUtility.DisplayProgressBar("Add Resource", "Processing...", 0f);
                    AddResource(m_InputResourceName, m_InputResourceVariant, true);
                    EditorUtility.ClearProgressBar();
                    Repaint();
                }
            }

            if (GUILayout.Button("Add", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Add Resource", "Processing...", 0f);
                AddResource(m_InputResourceName, m_InputResourceVariant, true);
                EditorUtility.ClearProgressBar();
            }

            if (GUILayout.Button("Back", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawResourcesMenu_Rename()
        {
            if (m_SelectedResource == null)
            {
                m_MenuState = MenuState.Normal;
                return;
            }

            GUI.SetNextControlName("RenameResourceNameTextField");
            m_InputResourceName = EditorGUILayout.TextField(m_InputResourceName);
            GUI.SetNextControlName("RenameResourceVariantTextField");
            m_InputResourceVariant = EditorGUILayout.TextField(m_InputResourceVariant, GUILayout.Width(60f));

            if (GUI.GetNameOfFocusedControl() == "RenameResourceNameTextField" || GUI.GetNameOfFocusedControl() == "RenameResourceVariantTextField")
            {
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                {
                    EditorUtility.DisplayProgressBar("Rename Resource", "Processing...", 0f);
                    RenameResource(m_SelectedResource, m_InputResourceName, m_InputResourceVariant);
                    EditorUtility.ClearProgressBar();
                    Repaint();
                }
            }

            if (GUILayout.Button("OK", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Rename Resource", "Processing...", 0f);
                RenameResource(m_SelectedResource, m_InputResourceName, m_InputResourceVariant);
                EditorUtility.ClearProgressBar();
            }

            if (GUILayout.Button("Back", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawResourcesMenu_Remove()
        {
            if (m_SelectedResource == null)
            {
                m_MenuState = MenuState.Normal;
                return;
            }

            GUILayout.Label(Utility.Text.Format("Remove '{0}' ?", m_SelectedResource.FullName));

            if (GUILayout.Button("Yes", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Remove Resource", "Processing...", 0f);
                RemoveResource();
                EditorUtility.ClearProgressBar();
                m_MenuState = MenuState.Normal;
            }

            if (GUILayout.Button("No", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawResourceView()
        {
            m_ResourceViewScroll = EditorGUILayout.BeginScrollView(m_ResourceViewScroll);
            {
                if (m_SelectedResource != null)
                {
                    int index = 0;
                    Asset[] assets = m_Controller.GetAssets(m_SelectedResource.Name, m_SelectedResource.Variant);
                    m_CurrentResourceContentCount = assets.Length;
                    foreach (Asset asset in assets)
                    {
                        SourceAsset sourceAsset = m_Controller.GetSourceAsset(asset.Guid);
                        string assetName = sourceAsset != null ? (m_Controller.AssetSorter == AssetSorterType.Path ? sourceAsset.Path : (m_Controller.AssetSorter == AssetSorterType.Name ? sourceAsset.Name : sourceAsset.Guid)) : asset.Guid;
                        EditorGUILayout.BeginHorizontal();
                        {
                            float emptySpace = position.width;
                            bool select = IsSelectedAssetInSelectedResource(asset);
                            if (select != EditorGUILayout.Toggle(select, GUILayout.Width(emptySpace - 12f)))
                            {
                                select = !select;
                                SetSelectedAssetInSelectedResource(asset, select);
                            }

                            GUILayout.Space(-emptySpace + 24f);
#if UNITY_2019_3_OR_NEWER
                            GUI.DrawTexture(new Rect(20f, 20f * index++ + 3f, 16f, 16f), sourceAsset != null ? sourceAsset.Icon : m_MissingSourceAssetIcon);
                            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(16f), GUILayout.Height(18f));
#else
                            GUI.DrawTexture(new Rect(20f, 20f * index++ + 1f, 16f, 16f), sourceAsset != null ? sourceAsset.Icon : m_MissingSourceAssetIcon);
                            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(14f), GUILayout.Height(18f));
#endif
                            EditorGUILayout.LabelField(assetName);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    m_CurrentResourceContentCount = 0;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawResourceMenu()
        {
            if (GUILayout.Button("All", GUILayout.Width(50f)) && m_SelectedResource != null)
            {
                Asset[] assets = m_Controller.GetAssets(m_SelectedResource.Name, m_SelectedResource.Variant);
                foreach (Asset asset in assets)
                {
                    SetSelectedAssetInSelectedResource(asset, true);
                }
            }
            if (GUILayout.Button("None", GUILayout.Width(50f)))
            {
                m_SelectedAssetsInSelectedResource.Clear();
            }
            m_Controller.AssetSorter = (AssetSorterType)EditorGUILayout.EnumPopup(m_Controller.AssetSorter, GUILayout.Width(60f));
            GUILayout.Label(string.Empty);
            EditorGUI.BeginDisabledGroup(m_SelectedResource == null || m_SelectedAssetsInSelectedResource.Count <= 0);
            {
                if (GUILayout.Button(Utility.Text.Format("{0} >>", m_SelectedAssetsInSelectedResource.Count.ToString()), GUILayout.Width(80f)))
                {
                    foreach (Asset asset in m_SelectedAssetsInSelectedResource)
                    {
                        UnassignAsset(asset);
                    }

                    m_SelectedAssetsInSelectedResource.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSourceAssetsView()
        {
            m_CurrentSourceRowOnDraw = 0;
            m_SourceAssetsViewScroll = EditorGUILayout.BeginScrollView(m_SourceAssetsViewScroll);
            {
                DrawSourceFolder(m_Controller.SourceAssetRoot);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawSourceAssetsMenu()
        {
            HashSet<SourceAsset> selectedSourceAssets = GetSelectedSourceAssets();
            EditorGUI.BeginDisabledGroup(m_SelectedResource == null || selectedSourceAssets.Count <= 0);
            {
                if (GUILayout.Button(Utility.Text.Format("<< {0}", selectedSourceAssets.Count.ToString()), GUILayout.Width(80f)))
                {
                    foreach (SourceAsset sourceAsset in selectedSourceAssets)
                    {
                        AssignAsset(sourceAsset, m_SelectedResource);
                    }

                    m_SelectedSourceAssets.Clear();
                    m_CachedSelectedSourceFolders.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(selectedSourceAssets.Count <= 0);
            {
                if (GUILayout.Button(Utility.Text.Format("<<< {0}", selectedSourceAssets.Count.ToString()), GUILayout.Width(80f)))
                {
                    int index = 0;
                    int count = selectedSourceAssets.Count;
                    foreach (SourceAsset sourceAsset in selectedSourceAssets)
                    {
                        EditorUtility.DisplayProgressBar("Add Resources", Utility.Text.Format("{0}/{1} processing...", (++index).ToString(), count.ToString()), (float)index / count);
                        int dotIndex = sourceAsset.FromRootPath.IndexOf('.');
                        string name = dotIndex > 0 ? sourceAsset.FromRootPath.Substring(0, dotIndex) : sourceAsset.FromRootPath;
                        AddResource(name, null, false);
                        Resource resource = m_Controller.GetResource(name, null);
                        if (resource == null)
                        {
                            continue;
                        }

                        AssignAsset(sourceAsset, resource);
                    }

                    EditorUtility.DisplayProgressBar("Add Resources", "Complete processing...", 1f);
                    RefreshResourceTree();
                    EditorUtility.ClearProgressBar();
                    m_SelectedSourceAssets.Clear();
                    m_CachedSelectedSourceFolders.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
            bool hideAssignedSourceAssets = EditorGUILayout.ToggleLeft("Hide Assigned", m_HideAssignedSourceAssets, GUILayout.Width(100f));
            if (hideAssignedSourceAssets != m_HideAssignedSourceAssets)
            {
                m_HideAssignedSourceAssets = hideAssignedSourceAssets;
                m_CachedSelectedSourceFolders.Clear();
                m_CachedUnselectedSourceFolders.Clear();
                m_CachedAssignedSourceFolders.Clear();
                m_CachedUnassignedSourceFolders.Clear();
            }

            GUILayout.Label(string.Empty);
            if (GUILayout.Button("Clean", GUILayout.Width(80f)))
            {
                EditorUtility.DisplayProgressBar("Clean", "Processing...", 0f);
                CleanResource();
                EditorUtility.ClearProgressBar();
            }
            if (GUILayout.Button("Save", GUILayout.Width(80f)))
            {
                EditorUtility.DisplayProgressBar("Save", "Processing...", 0f);
                SaveConfiguration();
                EditorUtility.ClearProgressBar();
            }
        }

        private void DrawSourceFolder(SourceFolder sourceFolder)
        {
            if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(sourceFolder))
            {
                return;
            }

            bool expand = IsExpandedSourceFolder(sourceFolder);
            EditorGUILayout.BeginHorizontal();
            {
                bool select = IsSelectedSourceFolder(sourceFolder);
                if (select != EditorGUILayout.Toggle(select, GUILayout.Width(12f + 14f * sourceFolder.Depth)))
                {
                    select = !select;
                    SetSelectedSourceFolder(sourceFolder, select);
                }

                GUILayout.Space(-14f * sourceFolder.Depth);
#if UNITY_2019_3_OR_NEWER
                bool foldout = EditorGUI.Foldout(new Rect(18f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 4f, int.MaxValue, 14f), expand, string.Empty, true);
#else
                bool foldout = EditorGUI.Foldout(new Rect(18f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 2f, int.MaxValue, 14f), expand, string.Empty, true);
#endif
                if (expand != foldout)
                {
                    expand = !expand;
                    SetExpandedSourceFolder(sourceFolder, expand);
                }

#if UNITY_2019_3_OR_NEWER
                GUI.DrawTexture(new Rect(32f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 3f, 16f, 16f), SourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(30f + 14f * sourceFolder.Depth), GUILayout.Height(18f));
#else
                GUI.DrawTexture(new Rect(32f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 1f, 16f, 16f), SourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * sourceFolder.Depth), GUILayout.Height(18f));
#endif
                EditorGUILayout.LabelField(sourceFolder.Name);
            }
            EditorGUILayout.EndHorizontal();

            m_CurrentSourceRowOnDraw++;

            if (expand)
            {
                foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
                {
                    DrawSourceFolder(subSourceFolder);
                }

                foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
                {
                    DrawSourceAsset(sourceAsset);
                }
            }
        }

        private void DrawSourceAsset(SourceAsset sourceAsset)
        {
            if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            {
                float emptySpace = position.width;
                bool select = IsSelectedSourceAsset(sourceAsset);
                if (select != EditorGUILayout.Toggle(select, GUILayout.Width(emptySpace - 12f)))
                {
                    select = !select;
                    SetSelectedSourceAsset(sourceAsset, select);
                }

                GUILayout.Space(-emptySpace + 24f);
#if UNITY_2019_3_OR_NEWER
                GUI.DrawTexture(new Rect(32f + 14f * sourceAsset.Depth, 20f * m_CurrentSourceRowOnDraw + 3f, 16f, 16f), sourceAsset.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(30f + 14f * sourceAsset.Depth), GUILayout.Height(18f));
#else
                GUI.DrawTexture(new Rect(32f + 14f * sourceAsset.Depth, 20f * m_CurrentSourceRowOnDraw + 1f, 16f, 16f), sourceAsset.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * sourceAsset.Depth), GUILayout.Height(18f));
#endif
                EditorGUILayout.LabelField(sourceAsset.Name);
                Asset asset = m_Controller.GetAsset(sourceAsset.Guid);
                EditorGUILayout.LabelField(asset != null ? GetResourceFullName(asset.Resource.Name, asset.Resource.Variant) : string.Empty, GUILayout.Width(position.width * 0.15f));
            }
            EditorGUILayout.EndHorizontal();
            m_CurrentSourceRowOnDraw++;
        }

        private void ChangeSelectedResource(Resource resource)
        {
            if (m_SelectedResource == resource)
            {
                return;
            }

            m_SelectedResource = resource;
            m_SelectedAssetsInSelectedResource.Clear();
        }

        private void SaveConfiguration()
        {
            if (m_Controller.Save())
            {
                Debug.Log("Save configuration success.");
            }
            else
            {
                Debug.LogWarning("Save configuration failure.");
            }
        }

        private void AddResource(string name, string variant, bool refresh)
        {
            if (variant == string.Empty)
            {
                variant = null;
            }

            string fullName = GetResourceFullName(name, variant);
            if (m_Controller.AddResource(name, variant, null, LoadType.LoadFromFile, false))
            {
                if (refresh)
                {
                    RefreshResourceTree();
                }

                Debug.Log(Utility.Text.Format("Add resource '{0}' success.", fullName));
                m_MenuState = MenuState.Normal;
            }
            else
            {
                Debug.LogWarning(Utility.Text.Format("Add resource '{0}' failure.", fullName));
            }
        }

        private void RenameResource(Resource resource, string newName, string newVariant)
        {
            if (resource == null)
            {
                Debug.LogWarning("Resource is invalid.");
                return;
            }

            if (newVariant == string.Empty)
            {
                newVariant = null;
            }

            string oldFullName = resource.FullName;
            string newFullName = GetResourceFullName(newName, newVariant);
            if (m_Controller.RenameResource(resource.Name, resource.Variant, newName, newVariant))
            {
                RefreshResourceTree();
                Debug.Log(Utility.Text.Format("Rename resource '{0}' to '{1}' success.", oldFullName, newFullName));
                m_MenuState = MenuState.Normal;
            }
            else
            {
                Debug.LogWarning(Utility.Text.Format("Rename resource '{0}' to '{1}' failure.", oldFullName, newFullName));
            }
        }

        private void RemoveResource()
        {
            string fullName = m_SelectedResource.FullName;
            if (m_Controller.RemoveResource(m_SelectedResource.Name, m_SelectedResource.Variant))
            {
                ChangeSelectedResource(null);
                RefreshResourceTree();
                Debug.Log(Utility.Text.Format("Remove resource '{0}' success.", fullName));
            }
            else
            {
                Debug.LogWarning(Utility.Text.Format("Remove resource '{0}' failure.", fullName));
            }
        }

        private void SetResourceLoadType(LoadType loadType)
        {
            string fullName = m_SelectedResource.FullName;
            if (m_Controller.SetResourceLoadType(m_SelectedResource.Name, m_SelectedResource.Variant, loadType))
            {
                Debug.Log(Utility.Text.Format("Set resource '{0}' load type to '{1}' success.", fullName, loadType.ToString()));
            }
            else
            {
                Debug.LogWarning(Utility.Text.Format("Set resource '{0}' load type to '{1}' failure.", fullName, loadType.ToString()));
            }
        }

        private void SetResourcePacked(bool packed)
        {
            string fullName = m_SelectedResource.FullName;
            if (m_Controller.SetResourcePacked(m_SelectedResource.Name, m_SelectedResource.Variant, packed))
            {
                Debug.Log(Utility.Text.Format("{1} resource '{0}' success.", fullName, packed ? "Pack" : "Unpack"));
            }
            else
            {
                Debug.LogWarning(Utility.Text.Format("{1} resource '{0}' failure.", fullName, packed ? "Pack" : "Unpack"));
            }
        }

        private void AssignAsset(SourceAsset sourceAsset, Resource resource)
        {
            if (!m_Controller.AssignAsset(sourceAsset.Guid, resource.Name, resource.Variant))
            {
                Debug.LogWarning(Utility.Text.Format("Assign asset '{0}' to resource '{1}' failure.", sourceAsset.Name, resource.FullName));
            }
        }

        private void UnassignAsset(Asset asset)
        {
            if (!m_Controller.UnassignAsset(asset.Guid))
            {
                Debug.LogWarning(Utility.Text.Format("Unassign asset '{0}' from resource '{1}' failure.", asset.Guid, m_SelectedResource.FullName));
            }
        }

        private void CleanResource()
        {
            int unknownAssetCount = m_Controller.RemoveUnknownAssets();
            int unusedResourceCount = m_Controller.RemoveUnusedResources();
            RefreshResourceTree();

            Debug.Log(Utility.Text.Format("Clean complete, {0} unknown assets and {1} unused resources has been removed.", unknownAssetCount.ToString(), unusedResourceCount.ToString()));
        }

        private void RefreshResourceTree()
        {
            m_ResourceRoot.Clear();
            Resource[] resources = m_Controller.GetResources();
            foreach (Resource resource in resources)
            {
                string[] splitedPath = resource.Name.Split('/');
                ResourceFolder folder = m_ResourceRoot;
                for (int i = 0; i < splitedPath.Length - 1; i++)
                {
                    ResourceFolder subFolder = folder.GetFolder(splitedPath[i]);
                    folder = subFolder == null ? folder.AddFolder(splitedPath[i]) : subFolder;
                }

                string fullName = resource.Variant != null ? Utility.Text.Format("{0}.{1}", splitedPath[splitedPath.Length - 1], resource.Variant) : splitedPath[splitedPath.Length - 1];
                folder.AddItem(fullName, resource);
            }
        }

        private bool IsExpandedResourceFolder(ResourceFolder folder)
        {
            return m_ExpandedResourceFolderNames.Contains(folder.FromRootPath);
        }

        private void SetExpandedResourceFolder(ResourceFolder folder, bool expand)
        {
            if (expand)
            {
                m_ExpandedResourceFolderNames.Add(folder.FromRootPath);
            }
            else
            {
                m_ExpandedResourceFolderNames.Remove(folder.FromRootPath);
            }
        }

        private bool IsSelectedAssetInSelectedResource(Asset asset)
        {
            return m_SelectedAssetsInSelectedResource.Contains(asset);
        }

        private void SetSelectedAssetInSelectedResource(Asset asset, bool select)
        {
            if (select)
            {
                m_SelectedAssetsInSelectedResource.Add(asset);
            }
            else
            {
                m_SelectedAssetsInSelectedResource.Remove(asset);
            }
        }

        private bool IsExpandedSourceFolder(SourceFolder sourceFolder)
        {
            return m_ExpandedSourceFolders.Contains(sourceFolder);
        }

        private void SetExpandedSourceFolder(SourceFolder sourceFolder, bool expand)
        {
            if (expand)
            {
                m_ExpandedSourceFolders.Add(sourceFolder);
            }
            else
            {
                m_ExpandedSourceFolders.Remove(sourceFolder);
            }
        }

        private bool IsSelectedSourceFolder(SourceFolder sourceFolder)
        {
            if (m_CachedSelectedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_CachedUnselectedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                if (!IsSelectedSourceAsset(sourceAsset))
                {
                    m_CachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(sourceFolder))
                {
                    continue;
                }

                if (!IsSelectedSourceFolder(subSourceFolder))
                {
                    m_CachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_CachedSelectedSourceFolders.Add(sourceFolder);
            return true;
        }

        private void SetSelectedSourceFolder(SourceFolder sourceFolder, bool select)
        {
            if (select)
            {
                m_CachedSelectedSourceFolders.Add(sourceFolder);
                m_CachedUnselectedSourceFolders.Remove(sourceFolder);

                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedUnselectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
            else
            {
                m_CachedSelectedSourceFolders.Remove(sourceFolder);
                m_CachedUnselectedSourceFolders.Add(sourceFolder);

                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                SetSelectedSourceAsset(sourceAsset, select);
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(subSourceFolder))
                {
                    continue;
                }

                SetSelectedSourceFolder(subSourceFolder, select);
            }
        }

        private bool IsSelectedSourceAsset(SourceAsset sourceAsset)
        {
            return m_SelectedSourceAssets.Contains(sourceAsset);
        }

        private void SetSelectedSourceAsset(SourceAsset sourceAsset, bool select)
        {
            if (select)
            {
                m_SelectedSourceAssets.Add(sourceAsset);

                SourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_CachedUnselectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
            else
            {
                m_SelectedSourceAssets.Remove(sourceAsset);

                SourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
        }

        private bool IsAssignedSourceAsset(SourceAsset sourceAsset)
        {
            if (m_CachedAssignedSourceAssets.Contains(sourceAsset))
            {
                return true;
            }

            if (m_CachedUnassignedSourceAssets.Contains(sourceAsset))
            {
                return false;
            }

            return m_Controller.GetAsset(sourceAsset.Guid) != null;
        }

        private bool IsAssignedSourceFolder(SourceFolder sourceFolder)
        {
            if (m_CachedAssignedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_CachedUnassignedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    m_CachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (!IsAssignedSourceFolder(subSourceFolder))
                {
                    m_CachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_CachedAssignedSourceFolders.Add(sourceFolder);
            return true;
        }

        private HashSet<SourceAsset> GetSelectedSourceAssets()
        {
            if (!m_HideAssignedSourceAssets)
            {
                return m_SelectedSourceAssets;
            }

            HashSet<SourceAsset> selectedUnassignedSourceAssets = new HashSet<SourceAsset>();
            foreach (SourceAsset sourceAsset in m_SelectedSourceAssets)
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    selectedUnassignedSourceAssets.Add(sourceAsset);
                }
            }

            return selectedUnassignedSourceAssets;
        }

        private string GetResourceFullName(string name, string variant)
        {
            return variant != null ? Utility.Text.Format("{0}.{1}", name, variant) : name;
        }

        private void OnLoadingResource(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Resources", Utility.Text.Format("Loading resources, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", Utility.Text.Format("Loading assets, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnAssetAssigned(SourceAsset[] sourceAssets)
        {
            HashSet<SourceFolder> affectedFolders = new HashSet<SourceFolder>();
            foreach (SourceAsset sourceAsset in sourceAssets)
            {
                m_CachedAssignedSourceAssets.Add(sourceAsset);
                m_CachedUnassignedSourceAssets.Remove(sourceAsset);

                affectedFolders.Add(sourceAsset.Folder);
            }

            foreach (SourceFolder sourceFolder in affectedFolders)
            {
                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedUnassignedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
        }

        private void OnAssetUnassigned(SourceAsset[] sourceAssets)
        {
            HashSet<SourceFolder> affectedFolders = new HashSet<SourceFolder>();
            foreach (SourceAsset sourceAsset in sourceAssets)
            {
                m_CachedAssignedSourceAssets.Remove(sourceAsset);
                m_CachedUnassignedSourceAssets.Add(sourceAsset);

                affectedFolders.Add(sourceAsset.Folder);
            }

            foreach (SourceFolder sourceFolder in affectedFolders)
            {
                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    m_CachedAssignedSourceFolders.Remove(folder);
                    m_CachedUnassignedSourceFolders.Add(folder);
                    folder = folder.Folder;
                }
            }
        }
    }
}
