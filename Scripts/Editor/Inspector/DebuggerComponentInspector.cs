//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(DebuggerComponent))]
    internal sealed class DebuggerComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_Skin = null;
        private SerializedProperty m_ActiveWindow = null;
        private SerializedProperty m_ShowFullWindow = null;
        private SerializedProperty m_ConsoleWindow = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            DebuggerComponent t = (DebuggerComponent)target;

            EditorGUILayout.PropertyField(m_Skin);

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                bool activeWindow = EditorGUILayout.Toggle("Active Window", t.ActiveWindow);
                if (activeWindow != t.ActiveWindow)
                {
                    t.ActiveWindow = activeWindow;
                }
            }
            else
            {
                EditorGUILayout.PropertyField(m_ActiveWindow);
            }

            EditorGUILayout.PropertyField(m_ShowFullWindow);

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Reset Layout"))
                {
                    t.ResetLayout();
                }
            }

            EditorGUILayout.PropertyField(m_ConsoleWindow, true);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            m_Skin = serializedObject.FindProperty("m_Skin");
            m_ActiveWindow = serializedObject.FindProperty("m_ActiveWindow");
            m_ShowFullWindow = serializedObject.FindProperty("m_ShowFullWindow");
            m_ConsoleWindow = serializedObject.FindProperty("m_ConsoleWindow");
        }
    }
}
