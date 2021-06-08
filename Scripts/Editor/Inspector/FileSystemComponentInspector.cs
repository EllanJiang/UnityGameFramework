//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.FileSystem;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(FileSystemComponent))]
    internal sealed class FileSystemComponentInspector : GameFrameworkInspector
    {
        private HelperInfo<FileSystemHelperBase> m_FileSystemHelperInfo = new HelperInfo<FileSystemHelperBase>("FileSystem");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FileSystemComponent t = (FileSystemComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_FileSystemHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("File System Count", t.Count.ToString());

                IFileSystem[] fileSystems = t.GetAllFileSystems();
                foreach (IFileSystem fileSystem in fileSystems)
                {
                    DrawFileSystem(fileSystem);
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_FileSystemHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_FileSystemHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFileSystem(IFileSystem fileSystem)
        {
            EditorGUILayout.LabelField(fileSystem.FullPath, Utility.Text.Format("{0}, {1} / {2} Files", fileSystem.Access, fileSystem.FileCount, fileSystem.MaxFileCount));
        }
    }
}
