//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(WebRequestComponent))]
    internal sealed class WebRequestComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_WebRequestAgentHelperCount = null;
        private SerializedProperty m_Timeout = null;

        private HelperInfo<WebRequestAgentHelperBase> m_WebRequestAgentHelperInfo = new HelperInfo<WebRequestAgentHelperBase>("WebRequestAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            WebRequestComponent t = (WebRequestComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);

                m_WebRequestAgentHelperInfo.Draw();

                m_WebRequestAgentHelperCount.intValue = EditorGUILayout.IntSlider("Web Request Agent Helper Count", m_WebRequestAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            float timeout = EditorGUILayout.Slider("Timeout", m_Timeout.floatValue, 0f, 120f);
            if (timeout != m_Timeout.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.Timeout = timeout;
                }
                else
                {
                    m_Timeout.floatValue = timeout;
                }
            }

            if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Total Agent Count", t.TotalAgentCount.ToString());
                EditorGUILayout.LabelField("Free Agent Count", t.FreeAgentCount.ToString());
                EditorGUILayout.LabelField("Working Agent Count", t.WorkingAgentCount.ToString());
                EditorGUILayout.LabelField("Waiting Agent Count", t.WaitingTaskCount.ToString());
                EditorGUILayout.BeginVertical("box");
                {
                    TaskInfo[] webRequestInfos = t.GetAllWebRequestInfos();
                    if (webRequestInfos.Length > 0)
                    {
                        foreach (TaskInfo webRequestInfo in webRequestInfos)
                        {
                            DrawWebRequestInfo(webRequestInfo);
                        }

                        if (GUILayout.Button("Export CSV Data"))
                        {
                            string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, "WebRequest Task Data.csv", string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[webRequestInfos.Length + 1];
                                    data[index++] = "WebRequest Uri,Serial Id,Tag,Priority,Status";
                                    foreach (TaskInfo webRequestInfo in webRequestInfos)
                                    {
                                        data[index++] = Utility.Text.Format("{0},{1},{2},{3},{4}", webRequestInfo.Description, webRequestInfo.SerialId.ToString(), webRequestInfo.Tag ?? string.Empty, webRequestInfo.Priority.ToString(), webRequestInfo.Status.ToString());
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Debug.Log(Utility.Text.Format("Export web request task CSV data to '{0}' success.", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Debug.LogError(Utility.Text.Format("Export web request task CSV data to '{0}' failure, exception is '{1}'.", exportFileName, exception.ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        GUILayout.Label("WebRequset Task is Empty ...");
                    }
                }
                EditorGUILayout.EndVertical();
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
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_WebRequestAgentHelperCount = serializedObject.FindProperty("m_WebRequestAgentHelperCount");
            m_Timeout = serializedObject.FindProperty("m_Timeout");

            m_WebRequestAgentHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void DrawWebRequestInfo(TaskInfo webRequestInfo)
        {
            EditorGUILayout.LabelField(webRequestInfo.Description, Utility.Text.Format("[SerialId]{0} [Tag]{1} [Priority]{2} [Status]{3}", webRequestInfo.SerialId.ToString(), webRequestInfo.Tag ?? "<None>", webRequestInfo.Priority.ToString(), webRequestInfo.Status.ToString()));
        }

        private void RefreshTypeNames()
        {
            m_WebRequestAgentHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
