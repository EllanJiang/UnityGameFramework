//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(ReferencePoolComponent))]
    internal sealed class ReferencePoolComponentInspector : GameFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            ReferencePoolComponent t = (ReferencePoolComponent)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Reference Pool Count", ReferencePool.Count.ToString());

                ReferencePoolInfo[] referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
                foreach (ReferencePoolInfo referencePoolInfo in referencePoolInfos)
                {
                    DrawReferencePoolInfo(referencePoolInfo);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {

        }

        private void DrawReferencePoolInfo(ReferencePoolInfo referencePoolInfo)
        {
            EditorGUILayout.LabelField(referencePoolInfo.TypeName, Utility.Text.Format("[Unused]{0} [Using]{1} [Acquire]{2} [Release]{3} [Add]{4} [Remove]{5}", referencePoolInfo.UnusedReferenceCount.ToString(), referencePoolInfo.UsingReferenceCount.ToString(), referencePoolInfo.AcquireReferenceCount.ToString(), referencePoolInfo.ReleaseReferenceCount.ToString(), referencePoolInfo.AddReferenceCount.ToString(), referencePoolInfo.RemoveReferenceCount.ToString()));
        }
    }
}
