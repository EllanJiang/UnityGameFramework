//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private sealed partial class RuntimeMemorySummaryWindow : ScrollableDebuggerWindowBase
        {
            private readonly List<Record> m_Records = new List<Record>();
            private DateTime m_SampleTime = DateTime.MinValue;
            private int m_SampleCount = 0;
            private long m_SampleSize = 0L;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Runtime Memory Summary</b>");
                GUILayout.BeginVertical("box");
                {
                    if (GUILayout.Button("Take Sample", GUILayout.Height(30f)))
                    {
                        TakeSample();
                    }

                    if (m_SampleTime <= DateTime.MinValue)
                    {
                        GUILayout.Label("<b>Please take sample first.</b>");
                    }
                    else
                    {
                        GUILayout.Label(Utility.Text.Format("<b>{0} Objects ({1}) obtained at {2}.</b>", m_SampleCount.ToString(), GetSizeString(m_SampleSize), m_SampleTime.ToString("yyyy-MM-dd HH:mm:ss")));

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("<b>Type</b>");
                            GUILayout.Label("<b>Count</b>", GUILayout.Width(120f));
                            GUILayout.Label("<b>Size</b>", GUILayout.Width(120f));
                        }
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < m_Records.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(m_Records[i].Name);
                                GUILayout.Label(m_Records[i].Count.ToString(), GUILayout.Width(120f));
                                GUILayout.Label(GetSizeString(m_Records[i].Size), GUILayout.Width(120f));
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                GUILayout.EndVertical();
            }

            private void TakeSample()
            {
                m_Records.Clear();
                m_SampleTime = DateTime.Now;
                m_SampleCount = 0;
                m_SampleSize = 0L;

                UnityEngine.Object[] samples = Resources.FindObjectsOfTypeAll<UnityEngine.Object>();
                for (int i = 0; i < samples.Length; i++)
                {
                    long sampleSize = 0L;
#if UNITY_5_6_OR_NEWER
                    sampleSize = Profiler.GetRuntimeMemorySizeLong(samples[i]);
#else
                    sampleSize = Profiler.GetRuntimeMemorySize(samples[i]);
#endif
                    string name = samples[i].GetType().Name;
                    m_SampleCount++;
                    m_SampleSize += sampleSize;

                    Record record = null;
                    foreach (Record r in m_Records)
                    {
                        if (r.Name == name)
                        {
                            record = r;
                            break;
                        }
                    }

                    if (record == null)
                    {
                        record = new Record(name);
                        m_Records.Add(record);
                    }

                    record.Count++;
                    record.Size += sampleSize;
                }

                m_Records.Sort(RecordComparer);
            }

            private string GetSizeString(long size)
            {
                if (size < 1024L)
                {
                    return Utility.Text.Format("{0} Bytes", size.ToString());
                }

                if (size < 1024L * 1024L)
                {
                    return Utility.Text.Format("{0} KB", (size / 1024f).ToString("F2"));
                }

                if (size < 1024L * 1024L * 1024L)
                {
                    return Utility.Text.Format("{0} MB", (size / 1024f / 1024f).ToString("F2"));
                }

                if (size < 1024L * 1024L * 1024L * 1024L)
                {
                    return Utility.Text.Format("{0} GB", (size / 1024f / 1024f / 1024f).ToString("F2"));
                }

                return Utility.Text.Format("{0} TB", (size / 1024f / 1024f / 1024f / 1024f).ToString("F2"));
            }

            private int RecordComparer(Record a, Record b)
            {
                int result = b.Size.CompareTo(a.Size);
                if (result != 0)
                {
                    return result;
                }

                result = a.Count.CompareTo(b.Count);
                if (result != 0)
                {
                    return result;
                }

                return a.Name.CompareTo(b.Name);
            }
        }
    }
}
