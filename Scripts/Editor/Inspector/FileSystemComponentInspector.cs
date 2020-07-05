//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
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
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            FileSystemComponent t = (FileSystemComponent)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("File System Count", t.Count.ToString());

                IFileSystem[] fileSystems = t.GetAllFileSystems();
                foreach (IFileSystem fileSystem in fileSystems)
                {
                    DrawFileSystem(fileSystem);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
        }

        private void DrawFileSystem(IFileSystem fileSystem)
        {
            EditorGUILayout.LabelField(fileSystem.FullPath, Utility.Text.Format("{0}, {1} / {2} Files", fileSystem.Access.ToString(), fileSystem.FileCount.ToString(), fileSystem.MaxFileCount.ToString()));
        }
    }
}
