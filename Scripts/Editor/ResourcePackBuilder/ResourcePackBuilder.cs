//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.ResourceTools
{
    /// <summary>
    /// 资源包生成器。
    /// </summary>
    internal sealed class ResourcePackBuilder : EditorWindow
    {
        private static readonly string[] PlatformForDisplay = new string[] { "Windows", "Windows x64", "macOS", "Linux", "iOS", "Android", "Windows Store", "WebGL" };
        private static readonly int[] LengthLimit = new int[] { 0, 128, 256, 512, 1024, 2048, 4096 };
        private static readonly string[] LengthLimitForDisplay = new string[] { "<Unlimited>", "128 MB", "256 MB", "512 MB", "1 GB", "2 GB", "4 GB", "<Custom>" };

        private ResourcePackBuilderController m_Controller = null;
        private string[] m_VersionNames = null;
        private string[] m_VersionNamesForTargetDisplay = null;
        private string[] m_VersionNamesForSourceDisplay = null;
        private int m_PlatformIndex = 0;
        private int m_CompressionHelperTypeNameIndex = 0;
        private int m_LengthLimitIndex = 0;
        private int m_TargetVersionIndex = 0;
        private bool[] m_SourceVersionIndexes = null;
        private int m_SourceVersionCount = 0;

        [MenuItem("Game Framework/Resource Tools/Resource Pack Builder", false, 43)]
        private static void Open()
        {
            ResourcePackBuilder window = GetWindow<ResourcePackBuilder>("Resource Pack Builder", true);
            window.minSize = new Vector2(800f, 400f);
        }

        private void OnEnable()
        {
            m_Controller = new ResourcePackBuilderController();
            m_Controller.OnBuildResourcePacksStarted += OnBuildResourcePacksStarted;
            m_Controller.OnBuildResourcePacksCompleted += OnBuildResourcePacksCompleted;
            m_Controller.OnBuildResourcePackSuccess += OnBuildResourcePackSuccess;
            m_Controller.OnBuildResourcePackFailure += OnBuildResourcePackFailure;

            m_Controller.Load();
            RefreshVersionNames();

            m_CompressionHelperTypeNameIndex = 0;
            string[] compressionHelperTypeNames = m_Controller.GetCompressionHelperTypeNames();
            for (int i = 0; i < compressionHelperTypeNames.Length; i++)
            {
                if (m_Controller.CompressionHelperTypeName == compressionHelperTypeNames[i])
                {
                    m_CompressionHelperTypeNameIndex = i;
                    break;
                }
            }

            m_Controller.RefreshCompressionHelper();
        }

        private void Update()
        {
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Environment Information", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Product Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.ProductName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Company Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.CompanyName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Game Identifier", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.GameIdentifier);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Game Framework Version", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.GameFrameworkVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Unity Version", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.UnityVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.ApplicableGameVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Working Directory", GUILayout.Width(160f));
                        string directory = EditorGUILayout.TextField(m_Controller.WorkingDirectory);
                        if (m_Controller.WorkingDirectory != directory)
                        {
                            m_Controller.WorkingDirectory = directory;
                            RefreshVersionNames();
                        }
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseWorkingDirectory();
                            RefreshVersionNames();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Platform", GUILayout.Width(160f));
                        int platformIndex = EditorGUILayout.Popup(m_PlatformIndex, PlatformForDisplay);
                        if (m_PlatformIndex != platformIndex)
                        {
                            m_PlatformIndex = platformIndex;
                            m_Controller.Platform = (Platform)(1 << platformIndex);
                            RefreshVersionNames();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Compression Helper", GUILayout.Width(160f));
                        string[] names = m_Controller.GetCompressionHelperTypeNames();
                        int selectedIndex = EditorGUILayout.Popup(m_CompressionHelperTypeNameIndex, names);
                        if (selectedIndex != m_CompressionHelperTypeNameIndex)
                        {
                            m_CompressionHelperTypeNameIndex = selectedIndex;
                            m_Controller.CompressionHelperTypeName = selectedIndex <= 0 ? string.Empty : names[selectedIndex];
                            if (m_Controller.RefreshCompressionHelper())
                            {
                                Debug.Log("Set compression helper success.");
                            }
                            else
                            {
                                Debug.LogWarning("Set compression helper failure.");
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (m_Controller.Platform == Platform.Undefined || string.IsNullOrEmpty(m_Controller.CompressionHelperTypeName) || !m_Controller.IsValidWorkingDirectory)
                    {
                        string message = string.Empty;
                        if (!m_Controller.IsValidWorkingDirectory)
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                message += Environment.NewLine;
                            }

                            message += "Working directory is invalid.";
                        }

                        if (m_Controller.Platform == Platform.Undefined)
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                message += Environment.NewLine;
                            }

                            message += "Platform is invalid.";
                        }

                        if (string.IsNullOrEmpty(m_Controller.CompressionHelperTypeName))
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                message += Environment.NewLine;
                            }

                            message += "Compression helper is invalid.";
                        }

                        EditorGUILayout.HelpBox(message, MessageType.Error);
                    }
                    else if (m_VersionNamesForTargetDisplay.Length <= 0)
                    {
                        EditorGUILayout.HelpBox("No version was found in the specified working directory and platform.", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Source Path", GUILayout.Width(160f));
                            GUILayout.Label(m_Controller.SourcePathForDisplay);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Output Path", GUILayout.Width(160f));
                            GUILayout.Label(m_Controller.OutputPath);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Backup Diff", GUILayout.Width(160f));
                            m_Controller.BackupDiff = EditorGUILayout.Toggle(m_Controller.BackupDiff);
                        }
                        EditorGUILayout.EndHorizontal();
                        if (m_Controller.BackupDiff)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Backup Version", GUILayout.Width(160f));
                                m_Controller.BackupVersion = EditorGUILayout.Toggle(m_Controller.BackupVersion);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Length Limit", GUILayout.Width(160f));
                            EditorGUILayout.BeginVertical();
                            {
                                int lengthLimitIndex = EditorGUILayout.Popup(m_LengthLimitIndex, LengthLimitForDisplay);
                                if (m_LengthLimitIndex != lengthLimitIndex)
                                {
                                    m_LengthLimitIndex = lengthLimitIndex;
                                    if (m_LengthLimitIndex < LengthLimit.Length)
                                    {
                                        m_Controller.LengthLimit = LengthLimit[m_LengthLimitIndex];
                                    }
                                }

                                if (m_LengthLimitIndex >= LengthLimit.Length)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        m_Controller.LengthLimit = EditorGUILayout.IntField(m_Controller.LengthLimit);
                                        if (m_Controller.LengthLimit < 0)
                                        {
                                            m_Controller.LengthLimit = 0;
                                        }

                                        GUILayout.Label(" MB", GUILayout.Width(30f));
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Target Version", GUILayout.Width(160f));
                            int value = EditorGUILayout.Popup(m_TargetVersionIndex, m_VersionNamesForTargetDisplay);
                            if (m_TargetVersionIndex != value)
                            {
                                m_TargetVersionIndex = value;
                                RefreshSourceVersionCount();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Source Version", GUILayout.Width(160f));
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(m_SourceVersionCount.ToString() + (m_SourceVersionCount > 1 ? " items" : " item") + " selected.");
                                    if (GUILayout.Button("Select All Except <None>", GUILayout.Width(180f)))
                                    {
                                        m_SourceVersionIndexes[0] = false;
                                        for (int i = 1; i < m_SourceVersionIndexes.Length; i++)
                                        {
                                            m_SourceVersionIndexes[i] = true;
                                        }

                                        RefreshSourceVersionCount();
                                    }
                                    if (GUILayout.Button("Select All", GUILayout.Width(100f)))
                                    {
                                        for (int i = 0; i < m_SourceVersionIndexes.Length; i++)
                                        {
                                            m_SourceVersionIndexes[i] = true;
                                        }

                                        RefreshSourceVersionCount();
                                    }
                                    if (GUILayout.Button("Select None", GUILayout.Width(100f)))
                                    {
                                        for (int i = 0; i < m_SourceVersionIndexes.Length; i++)
                                        {
                                            m_SourceVersionIndexes[i] = false;
                                        }

                                        RefreshSourceVersionCount();
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                {
                                    int count = m_VersionNamesForSourceDisplay.Length;
                                    int column = 5;
                                    int row = (count - 1) / column + 1;
                                    for (int i = 0; i < column && i < count; i++)
                                    {
                                        EditorGUILayout.BeginVertical();
                                        {
                                            for (int j = 0; j < row; j++)
                                            {
                                                int index = j * column + i;
                                                if (index < count)
                                                {
                                                    bool isTarget = index - 1 == m_TargetVersionIndex;
                                                    EditorGUI.BeginDisabledGroup(isTarget);
                                                    {
                                                        bool selected = GUILayout.Toggle(m_SourceVersionIndexes[index], isTarget ? m_VersionNamesForSourceDisplay[index] + " [Target]" : m_VersionNamesForSourceDisplay[index], "button");
                                                        if (m_SourceVersionIndexes[index] != selected)
                                                        {
                                                            m_SourceVersionIndexes[index] = selected;
                                                            RefreshSourceVersionCount();
                                                        }
                                                    }
                                                    EditorGUI.EndDisabledGroup();
                                                }
                                            }
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    GUILayout.Space(2f);
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(2f);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(m_Controller.Platform == Platform.Undefined || string.IsNullOrEmpty(m_Controller.CompressionHelperTypeName) || !m_Controller.IsValidWorkingDirectory || m_SourceVersionCount <= 0);
                    {
                        if (GUILayout.Button("Start Build Resource Packs"))
                        {
                            string[] sourceVersions = new string[m_SourceVersionCount];
                            int count = 0;
                            for (int i = 0; i < m_SourceVersionIndexes.Length; i++)
                            {
                                if (m_SourceVersionIndexes[i])
                                {
                                    sourceVersions[count++] = i > 0 ? m_VersionNames[i - 1] : null;
                                }
                            }

                            m_Controller.BuildResourcePacks(sourceVersions, m_VersionNames[m_TargetVersionIndex]);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void BrowseWorkingDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Working Directory", m_Controller.WorkingDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_Controller.WorkingDirectory = directory;
            }
        }

        private void RefreshVersionNames()
        {
            m_VersionNames = m_Controller.GetVersionNames();
            m_VersionNamesForTargetDisplay = new string[m_VersionNames.Length];
            m_VersionNamesForSourceDisplay = new string[m_VersionNames.Length + 1];
            m_VersionNamesForSourceDisplay[0] = "<None>";
            for (int i = 0; i < m_VersionNames.Length; i++)
            {
                string versionNameForDisplay = GetVersionNameForDisplay(m_VersionNames[i]);
                m_VersionNamesForTargetDisplay[i] = versionNameForDisplay;
                m_VersionNamesForSourceDisplay[i + 1] = versionNameForDisplay;
            }

            m_TargetVersionIndex = m_VersionNames.Length - 1;
            m_SourceVersionIndexes = new bool[m_VersionNames.Length + 1];
            m_SourceVersionCount = 0;
        }

        private void RefreshSourceVersionCount()
        {
            m_SourceVersionIndexes[m_TargetVersionIndex + 1] = false;
            m_SourceVersionCount = 0;
            if (m_SourceVersionIndexes == null)
            {
                return;
            }

            for (int i = 0; i < m_SourceVersionIndexes.Length; i++)
            {
                if (m_SourceVersionIndexes[i])
                {
                    m_SourceVersionCount++;
                }
            }
        }

        private string GetVersionNameForDisplay(string versionName)
        {
            if (string.IsNullOrEmpty(versionName))
            {
                return "<None>";
            }

            string[] splitedVersionNames = versionName.Split('_');
            if (splitedVersionNames.Length < 2)
            {
                return null;
            }

            string text = splitedVersionNames[0];
            for (int i = 1; i < splitedVersionNames.Length - 1; i++)
            {
                text += "." + splitedVersionNames[i];
            }

            return Utility.Text.Format("{0} ({1})", text, splitedVersionNames[splitedVersionNames.Length - 1]);
        }

        private void OnBuildResourcePacksStarted(int count)
        {
            Debug.Log(Utility.Text.Format("Build resource packs started, '{0}' items to be built.", count.ToString()));
            EditorUtility.DisplayProgressBar("Build Resource Packs", Utility.Text.Format("Build resource packs, {0} items to be built.", count.ToString()), 0f);
        }

        private void OnBuildResourcePacksCompleted(int successCount, int count)
        {
            int failureCount = count - successCount;
            string str = Utility.Text.Format("Build resource packs completed, '{0}' items, '{1}' success, '{2}' failure.", count.ToString(), successCount.ToString(), failureCount.ToString());
            if (failureCount > 0)
            {
                Debug.LogWarning(str);
            }
            else
            {
                Debug.Log(str);
            }

            EditorUtility.ClearProgressBar();
        }

        private void OnBuildResourcePackSuccess(int index, int count, string sourceVersion, string targetVersion)
        {
            Debug.Log(Utility.Text.Format("Build resource packs success, source version '{0}', target version '{1}'.", GetVersionNameForDisplay(sourceVersion), GetVersionNameForDisplay(targetVersion)));
            EditorUtility.DisplayProgressBar("Build Resource Packs", Utility.Text.Format("Build resource packs, {0}/{1} completed.", (index + 1).ToString(), count.ToString()), (float)index / count);
        }

        private void OnBuildResourcePackFailure(int index, int count, string sourceVersion, string targetVersion)
        {
            Debug.LogWarning(Utility.Text.Format("Build resource packs failure, source version '{0}', target version '{1}'.", GetVersionNameForDisplay(sourceVersion), GetVersionNameForDisplay(targetVersion)));
            EditorUtility.DisplayProgressBar("Build Resource Packs", Utility.Text.Format("Build resource packs, {0}/{1} completed.", (index + 1).ToString(), count.ToString()), (float)index / count);
        }
    }
}
