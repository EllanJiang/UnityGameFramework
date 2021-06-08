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
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(BaseComponent))]
    internal sealed class BaseComponentInspector : GameFrameworkInspector
    {
        private const string NoneOptionName = "<None>";
        private static readonly float[] GameSpeed = new float[] { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };
        private static readonly string[] GameSpeedForDisplay = new string[] { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        private SerializedProperty m_EditorResourceMode = null;
        private SerializedProperty m_EditorLanguage = null;
        private SerializedProperty m_TextHelperTypeName = null;
        private SerializedProperty m_VersionHelperTypeName = null;
        private SerializedProperty m_LogHelperTypeName = null;
        private SerializedProperty m_CompressionHelperTypeName = null;
        private SerializedProperty m_JsonHelperTypeName = null;
        private SerializedProperty m_FrameRate = null;
        private SerializedProperty m_GameSpeed = null;
        private SerializedProperty m_RunInBackground = null;
        private SerializedProperty m_NeverSleep = null;

        private string[] m_TextHelperTypeNames = null;
        private int m_TextHelperTypeNameIndex = 0;
        private string[] m_VersionHelperTypeNames = null;
        private int m_VersionHelperTypeNameIndex = 0;
        private string[] m_LogHelperTypeNames = null;
        private int m_LogHelperTypeNameIndex = 0;
        private string[] m_CompressionHelperTypeNames = null;
        private int m_CompressionHelperTypeNameIndex = 0;
        private string[] m_JsonHelperTypeNames = null;
        private int m_JsonHelperTypeNameIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            BaseComponent t = (BaseComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_EditorResourceMode.boolValue = EditorGUILayout.BeginToggleGroup("Editor Resource Mode", m_EditorResourceMode.boolValue);
                {
                    EditorGUILayout.HelpBox("Editor resource mode option is only for editor mode. Game Framework will use editor resource files, which you should validate first.", MessageType.Warning);
                    EditorGUILayout.PropertyField(m_EditorLanguage);
                    EditorGUILayout.HelpBox("Editor language option is only use for localization test in editor mode.", MessageType.Info);
                }
                EditorGUILayout.EndToggleGroup();

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Global Helpers", EditorStyles.boldLabel);

                    int textHelperSelectedIndex = EditorGUILayout.Popup("Text Helper", m_TextHelperTypeNameIndex, m_TextHelperTypeNames);
                    if (textHelperSelectedIndex != m_TextHelperTypeNameIndex)
                    {
                        m_TextHelperTypeNameIndex = textHelperSelectedIndex;
                        m_TextHelperTypeName.stringValue = textHelperSelectedIndex <= 0 ? null : m_TextHelperTypeNames[textHelperSelectedIndex];
                    }

                    int versionHelperSelectedIndex = EditorGUILayout.Popup("Version Helper", m_VersionHelperTypeNameIndex, m_VersionHelperTypeNames);
                    if (versionHelperSelectedIndex != m_VersionHelperTypeNameIndex)
                    {
                        m_VersionHelperTypeNameIndex = versionHelperSelectedIndex;
                        m_VersionHelperTypeName.stringValue = versionHelperSelectedIndex <= 0 ? null : m_VersionHelperTypeNames[versionHelperSelectedIndex];
                    }

                    int logHelperSelectedIndex = EditorGUILayout.Popup("Log Helper", m_LogHelperTypeNameIndex, m_LogHelperTypeNames);
                    if (logHelperSelectedIndex != m_LogHelperTypeNameIndex)
                    {
                        m_LogHelperTypeNameIndex = logHelperSelectedIndex;
                        m_LogHelperTypeName.stringValue = logHelperSelectedIndex <= 0 ? null : m_LogHelperTypeNames[logHelperSelectedIndex];
                    }

                    int compressionHelperSelectedIndex = EditorGUILayout.Popup("Compression Helper", m_CompressionHelperTypeNameIndex, m_CompressionHelperTypeNames);
                    if (compressionHelperSelectedIndex != m_CompressionHelperTypeNameIndex)
                    {
                        m_CompressionHelperTypeNameIndex = compressionHelperSelectedIndex;
                        m_CompressionHelperTypeName.stringValue = compressionHelperSelectedIndex <= 0 ? null : m_CompressionHelperTypeNames[compressionHelperSelectedIndex];
                    }

                    int jsonHelperSelectedIndex = EditorGUILayout.Popup("JSON Helper", m_JsonHelperTypeNameIndex, m_JsonHelperTypeNames);
                    if (jsonHelperSelectedIndex != m_JsonHelperTypeNameIndex)
                    {
                        m_JsonHelperTypeNameIndex = jsonHelperSelectedIndex;
                        m_JsonHelperTypeName.stringValue = jsonHelperSelectedIndex <= 0 ? null : m_JsonHelperTypeNames[jsonHelperSelectedIndex];
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            int frameRate = EditorGUILayout.IntSlider("Frame Rate", m_FrameRate.intValue, 1, 120);
            if (frameRate != m_FrameRate.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.FrameRate = frameRate;
                }
                else
                {
                    m_FrameRate.intValue = frameRate;
                }
            }

            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("Game Speed", m_GameSpeed.floatValue, 0f, 8f);
                int selectedGameSpeed = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), GameSpeedForDisplay, 5);
                if (selectedGameSpeed >= 0)
                {
                    gameSpeed = GetGameSpeed(selectedGameSpeed);
                }

                if (gameSpeed != m_GameSpeed.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.GameSpeed = gameSpeed;
                    }
                    else
                    {
                        m_GameSpeed.floatValue = gameSpeed;
                    }
                }
            }
            EditorGUILayout.EndVertical();

            bool runInBackground = EditorGUILayout.Toggle("Run in Background", m_RunInBackground.boolValue);
            if (runInBackground != m_RunInBackground.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.RunInBackground = runInBackground;
                }
                else
                {
                    m_RunInBackground.boolValue = runInBackground;
                }
            }

            bool neverSleep = EditorGUILayout.Toggle("Never Sleep", m_NeverSleep.boolValue);
            if (neverSleep != m_NeverSleep.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.NeverSleep = neverSleep;
                }
                else
                {
                    m_NeverSleep.boolValue = neverSleep;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_EditorResourceMode = serializedObject.FindProperty("m_EditorResourceMode");
            m_EditorLanguage = serializedObject.FindProperty("m_EditorLanguage");
            m_TextHelperTypeName = serializedObject.FindProperty("m_TextHelperTypeName");
            m_VersionHelperTypeName = serializedObject.FindProperty("m_VersionHelperTypeName");
            m_LogHelperTypeName = serializedObject.FindProperty("m_LogHelperTypeName");
            m_CompressionHelperTypeName = serializedObject.FindProperty("m_CompressionHelperTypeName");
            m_JsonHelperTypeName = serializedObject.FindProperty("m_JsonHelperTypeName");
            m_FrameRate = serializedObject.FindProperty("m_FrameRate");
            m_GameSpeed = serializedObject.FindProperty("m_GameSpeed");
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            m_NeverSleep = serializedObject.FindProperty("m_NeverSleep");

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            List<string> textHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            textHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Utility.Text.ITextHelper)));
            m_TextHelperTypeNames = textHelperTypeNames.ToArray();
            m_TextHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_TextHelperTypeName.stringValue))
            {
                m_TextHelperTypeNameIndex = textHelperTypeNames.IndexOf(m_TextHelperTypeName.stringValue);
                if (m_TextHelperTypeNameIndex <= 0)
                {
                    m_TextHelperTypeNameIndex = 0;
                    m_TextHelperTypeName.stringValue = null;
                }
            }

            List<string> versionHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            versionHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Version.IVersionHelper)));
            m_VersionHelperTypeNames = versionHelperTypeNames.ToArray();
            m_VersionHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_VersionHelperTypeName.stringValue))
            {
                m_VersionHelperTypeNameIndex = versionHelperTypeNames.IndexOf(m_VersionHelperTypeName.stringValue);
                if (m_VersionHelperTypeNameIndex <= 0)
                {
                    m_VersionHelperTypeNameIndex = 0;
                    m_VersionHelperTypeName.stringValue = null;
                }
            }

            List<string> logHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            logHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(GameFrameworkLog.ILogHelper)));
            m_LogHelperTypeNames = logHelperTypeNames.ToArray();
            m_LogHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_LogHelperTypeName.stringValue))
            {
                m_LogHelperTypeNameIndex = logHelperTypeNames.IndexOf(m_LogHelperTypeName.stringValue);
                if (m_LogHelperTypeNameIndex <= 0)
                {
                    m_LogHelperTypeNameIndex = 0;
                    m_LogHelperTypeName.stringValue = null;
                }
            }

            List<string> compressionHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            compressionHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Utility.Compression.ICompressionHelper)));
            m_CompressionHelperTypeNames = compressionHelperTypeNames.ToArray();
            m_CompressionHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_CompressionHelperTypeName.stringValue))
            {
                m_CompressionHelperTypeNameIndex = compressionHelperTypeNames.IndexOf(m_CompressionHelperTypeName.stringValue);
                if (m_CompressionHelperTypeNameIndex <= 0)
                {
                    m_CompressionHelperTypeNameIndex = 0;
                    m_CompressionHelperTypeName.stringValue = null;
                }
            }

            List<string> jsonHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            jsonHelperTypeNames.AddRange(Type.GetRuntimeTypeNames(typeof(Utility.Json.IJsonHelper)));
            m_JsonHelperTypeNames = jsonHelperTypeNames.ToArray();
            m_JsonHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_JsonHelperTypeName.stringValue))
            {
                m_JsonHelperTypeNameIndex = jsonHelperTypeNames.IndexOf(m_JsonHelperTypeName.stringValue);
                if (m_JsonHelperTypeNameIndex <= 0)
                {
                    m_JsonHelperTypeNameIndex = 0;
                    m_JsonHelperTypeName.stringValue = null;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return GameSpeed[0];
            }

            if (selectedGameSpeed >= GameSpeed.Length)
            {
                return GameSpeed[GameSpeed.Length - 1];
            }

            return GameSpeed[selectedGameSpeed];
        }

        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < GameSpeed.Length; i++)
            {
                if (gameSpeed == GameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
