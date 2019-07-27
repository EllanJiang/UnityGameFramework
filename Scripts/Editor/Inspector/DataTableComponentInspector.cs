//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(DataTableComponent))]
    internal sealed class DataTableComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDataTableUpdateEvent = null;
        private SerializedProperty m_EnableLoadDataTableDependencyAssetEvent = null;

        private HelperInfo<DataTableHelperBase> m_DataTableHelperInfo = new HelperInfo<DataTableHelperBase>("DataTable");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            DataTableComponent t = (DataTableComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnableLoadDataTableUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableLoadDataTableDependencyAssetEvent);
                m_DataTableHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Data Table Count", t.Count.ToString());

                DataTableBase[] dataTables = t.GetAllDataTables();
                foreach (DataTableBase dataTable in dataTables)
                {
                    DrawDataTable(dataTable);
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
            m_EnableLoadDataTableUpdateEvent = serializedObject.FindProperty("m_EnableLoadDataTableUpdateEvent");
            m_EnableLoadDataTableDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadDataTableDependencyAssetEvent");

            m_DataTableHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void DrawDataTable(DataTableBase dataTable)
        {
            EditorGUILayout.LabelField(Utility.Text.GetFullName(dataTable.Type, dataTable.Name), Utility.Text.Format("{0} Rows", dataTable.Count.ToString()));
        }

        private void RefreshTypeNames()
        {
            m_DataTableHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
