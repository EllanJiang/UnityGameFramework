//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(ConfigComponent))]
    internal sealed class ConfigComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadConfigSuccessEvent = null;
        private SerializedProperty m_EnableLoadConfigFailureEvent = null;
        private SerializedProperty m_EnableLoadConfigUpdateEvent = null;
        private SerializedProperty m_EnableLoadConfigDependencyAssetEvent = null;

        private HelperInfo<ConfigHelperBase> m_ConfigHelperInfo = new HelperInfo<ConfigHelperBase>("Config");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ConfigComponent t = (ConfigComponent)target;

            EditorGUILayout.PropertyField(m_EnableLoadConfigSuccessEvent);
            EditorGUILayout.PropertyField(m_EnableLoadConfigFailureEvent);
            EditorGUILayout.PropertyField(m_EnableLoadConfigUpdateEvent);
            EditorGUILayout.PropertyField(m_EnableLoadConfigDependencyAssetEvent);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_ConfigHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && PrefabUtility.GetPrefabType(t.gameObject) != PrefabType.Prefab)
            {
                EditorGUILayout.LabelField("Config Count", t.ConfigCount.ToString());
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
            m_EnableLoadConfigSuccessEvent = serializedObject.FindProperty("m_EnableLoadConfigSuccessEvent");
            m_EnableLoadConfigFailureEvent = serializedObject.FindProperty("m_EnableLoadConfigFailureEvent");
            m_EnableLoadConfigUpdateEvent = serializedObject.FindProperty("m_EnableLoadConfigUpdateEvent");
            m_EnableLoadConfigDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadConfigDependencyAssetEvent");

            m_ConfigHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_ConfigHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
