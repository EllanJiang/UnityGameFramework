//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Fsm;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(FsmComponent))]
    internal sealed class FsmComponentInspector : GameFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            FsmComponent t = (FsmComponent)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("FSM Count", t.Count.ToString());

                FsmBase[] fsms = t.GetAllFsms();
                foreach (FsmBase fsm in fsms)
                {
                    DrawFsm(fsm);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
        }

        private void DrawFsm(FsmBase fsm)
        {
            EditorGUILayout.LabelField(fsm.FullName, fsm.IsRunning ? Utility.Text.Format("{0}, {1:F1} s", fsm.CurrentStateName, fsm.CurrentStateTime) : (fsm.IsDestroyed ? "Destroyed" : "Not Running"));
        }
    }
}
