//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed class ReferencePoolInformationWindow : ScrollableDebuggerWindowBase
        {
            private readonly Dictionary<string, List<ReferencePoolInfo>> m_ReferencePoolInfos = new Dictionary<string, List<ReferencePoolInfo>>();
            private bool m_ShowFullClassName = false;

            public override void Initialize(params object[] args)
            {
            }

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Reference Pool Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Reference Pool Count", ReferencePool.Count.ToString());
                }
                GUILayout.EndVertical();

                m_ShowFullClassName = GUILayout.Toggle(m_ShowFullClassName, "Show Full Class Name");
                m_ReferencePoolInfos.Clear();
                ReferencePoolInfo[] referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
                foreach (ReferencePoolInfo referencePoolInfo in referencePoolInfos)
                {
                    string assemblyName = referencePoolInfo.Type.Assembly.GetName().Name;
                    List<ReferencePoolInfo> results = null;
                    if (!m_ReferencePoolInfos.TryGetValue(assemblyName, out results))
                    {
                        results = new List<ReferencePoolInfo>();
                        m_ReferencePoolInfos.Add(assemblyName, results);
                    }

                    results.Add(referencePoolInfo);
                }

                foreach (KeyValuePair<string, List<ReferencePoolInfo>> assemblyReferencePoolInfo in m_ReferencePoolInfos)
                {
                    GUILayout.Label(Utility.Text.Format("<b>Assembly: {0}</b>", assemblyReferencePoolInfo.Key));
                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(m_ShowFullClassName ? "<b>Full Class Name</b>" : "<b>Class Name</b>");
                            GUILayout.Label("<b>Unused</b>", GUILayout.Width(60f));
                            GUILayout.Label("<b>Using</b>", GUILayout.Width(60f));
                            GUILayout.Label("<b>Acquire</b>", GUILayout.Width(60f));
                            GUILayout.Label("<b>Release</b>", GUILayout.Width(60f));
                            GUILayout.Label("<b>Add</b>", GUILayout.Width(60f));
                            GUILayout.Label("<b>Remove</b>", GUILayout.Width(60f));
                        }
                        GUILayout.EndHorizontal();

                        if (assemblyReferencePoolInfo.Value.Count > 0)
                        {
                            assemblyReferencePoolInfo.Value.Sort(Comparison);
                            foreach (ReferencePoolInfo referencePoolInfo in assemblyReferencePoolInfo.Value)
                            {
                                DrawReferencePoolInfo(referencePoolInfo);
                            }
                        }
                        else
                        {
                            GUILayout.Label("<i>Reference Pool is Empty ...</i>");
                        }
                    }
                    GUILayout.EndVertical();
                }
            }

            private void DrawReferencePoolInfo(ReferencePoolInfo referencePoolInfo)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(m_ShowFullClassName ? referencePoolInfo.Type.FullName : referencePoolInfo.Type.Name);
                    GUILayout.Label(referencePoolInfo.UnusedReferenceCount.ToString(), GUILayout.Width(60f));
                    GUILayout.Label(referencePoolInfo.UsingReferenceCount.ToString(), GUILayout.Width(60f));
                    GUILayout.Label(referencePoolInfo.AcquireReferenceCount.ToString(), GUILayout.Width(60f));
                    GUILayout.Label(referencePoolInfo.ReleaseReferenceCount.ToString(), GUILayout.Width(60f));
                    GUILayout.Label(referencePoolInfo.AddReferenceCount.ToString(), GUILayout.Width(60f));
                    GUILayout.Label(referencePoolInfo.RemoveReferenceCount.ToString(), GUILayout.Width(60f));
                }
                GUILayout.EndHorizontal();
            }

            private int Comparison(ReferencePoolInfo a, ReferencePoolInfo b)
            {
                if (m_ShowFullClassName)
                {
                    return a.Type.FullName.CompareTo(b.Type.FullName);
                }
                else
                {
                    return a.Type.Name.CompareTo(b.Type.Name);
                }
            }
        }
    }
}
