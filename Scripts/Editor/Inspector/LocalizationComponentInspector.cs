﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(LocalizationComponent))]
    internal sealed class LocalizationComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDictionaryUpdateEvent = null;
        private SerializedProperty m_EnableLoadDictionaryDependencyAssetEvent = null;

        private HelperInfo<LocalizationHelperBase> m_LocalizationHelperInfo = new HelperInfo<LocalizationHelperBase>("Localization");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            LocalizationComponent t = (LocalizationComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnableLoadDictionaryUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableLoadDictionaryDependencyAssetEvent);
                m_LocalizationHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Language", t.Language.ToString());
                EditorGUILayout.LabelField("System Language", t.SystemLanguage.ToString());
                EditorGUILayout.LabelField("Dictionary Count", t.DictionaryCount.ToString());
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
            m_EnableLoadDictionaryUpdateEvent = serializedObject.FindProperty("m_EnableLoadDictionaryUpdateEvent");
            m_EnableLoadDictionaryDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadDictionaryDependencyAssetEvent");

            m_LocalizationHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_LocalizationHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
