//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(EditorResourceComponent))]
    internal sealed class EditorResourceComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableCachedAssets = null;
        private SerializedProperty m_LoadAssetCountPerFrame = null;
        private SerializedProperty m_MinLoadAssetRandomDelaySeconds = null;
        private SerializedProperty m_MaxLoadAssetRandomDelaySeconds = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorResourceComponent t = (EditorResourceComponent)target;

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Load Waiting Asset Count", t.LoadWaitingAssetCount.ToString());
            }

            EditorGUILayout.PropertyField(m_EnableCachedAssets);
            EditorGUILayout.PropertyField(m_LoadAssetCountPerFrame);
            EditorGUILayout.PropertyField(m_MinLoadAssetRandomDelaySeconds);
            EditorGUILayout.PropertyField(m_MaxLoadAssetRandomDelaySeconds);

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            m_EnableCachedAssets = serializedObject.FindProperty("m_EnableCachedAssets");
            m_LoadAssetCountPerFrame = serializedObject.FindProperty("m_LoadAssetCountPerFrame");
            m_MinLoadAssetRandomDelaySeconds = serializedObject.FindProperty("m_MinLoadAssetRandomDelaySeconds");
            m_MaxLoadAssetRandomDelaySeconds = serializedObject.FindProperty("m_MaxLoadAssetRandomDelaySeconds");
        }
    }
}
