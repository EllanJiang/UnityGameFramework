//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Debugger;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        private abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
        {
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            public virtual void Initialize(params object[] args)
            {
            }

            public virtual void Shutdown()
            {
            }

            public virtual void OnEnter()
            {
            }

            public virtual void OnLeave()
            {
            }

            public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
            }

            public void OnDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    OnDrawScrollableWindow();
                }
                GUILayout.EndScrollView();
            }

            protected abstract void OnDrawScrollableWindow();

            protected static void DrawItem(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title, GUILayout.Width(TitleWidth));
                    if (GUILayout.Button(content, "label"))
                    {
                        CopyToClipboard(content);
                    }
                }
                GUILayout.EndHorizontal();
            }

            protected static string GetByteLengthString(long byteLength)
            {
                if (byteLength < 1024L) // 2 ^ 10
                {
                    return Utility.Text.Format("{0} Bytes", byteLength);
                }

                if (byteLength < 1048576L) // 2 ^ 20
                {
                    return Utility.Text.Format("{0:F2} KB", byteLength / 1024f);
                }

                if (byteLength < 1073741824L) // 2 ^ 30
                {
                    return Utility.Text.Format("{0:F2} MB", byteLength / 1048576f);
                }

                if (byteLength < 1099511627776L) // 2 ^ 40
                {
                    return Utility.Text.Format("{0:F2} GB", byteLength / 1073741824f);
                }

                if (byteLength < 1125899906842624L) // 2 ^ 50
                {
                    return Utility.Text.Format("{0:F2} TB", byteLength / 1099511627776f);
                }

                if (byteLength < 1152921504606846976L) // 2 ^ 60
                {
                    return Utility.Text.Format("{0:F2} PB", byteLength / 1125899906842624f);
                }

                return Utility.Text.Format("{0:F2} EB", byteLength / 1152921504606846976f);
            }
        }
    }
}
