//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Debugger;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 调试器组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Debugger")]
    public sealed partial class DebuggerComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 默认调试器漂浮框大小。
        /// </summary>
        internal static readonly Rect DefaultIconRect = new Rect(10f, 10f, 60f, 60f);

        /// <summary>
        /// 默认调试器窗口大小。
        /// </summary>
        internal static readonly Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f);

        /// <summary>
        /// 默认调试器窗口缩放比例。
        /// </summary>
        internal static readonly float DefaultWindowScale = 1f;

        private static readonly TextEditor s_TextEditor = new TextEditor();
        private IDebuggerManager m_DebuggerManager = null;
        private Rect m_DragRect = new Rect(0f, 0f, float.MaxValue, 25f);
        private Rect m_IconRect = DefaultIconRect;
        private Rect m_WindowRect = DefaultWindowRect;
        private float m_WindowScale = DefaultWindowScale;

        [SerializeField]
        private GUISkin m_Skin = null;

        [SerializeField]
        private DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.AlwaysOpen;

        [SerializeField]
        private bool m_ShowFullWindow = false;

        [SerializeField]
        private ConsoleWindow m_ConsoleWindow = new ConsoleWindow();

        private SystemInformationWindow m_SystemInformationWindow = new SystemInformationWindow();
        private EnvironmentInformationWindow m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
        private ScreenInformationWindow m_ScreenInformationWindow = new ScreenInformationWindow();
        private GraphicsInformationWindow m_GraphicsInformationWindow = new GraphicsInformationWindow();
        private InputSummaryInformationWindow m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
        private InputTouchInformationWindow m_InputTouchInformationWindow = new InputTouchInformationWindow();
        private InputLocationInformationWindow m_InputLocationInformationWindow = new InputLocationInformationWindow();
        private InputAccelerationInformationWindow m_InputAccelerationInformationWindow = new InputAccelerationInformationWindow();
        private InputGyroscopeInformationWindow m_InputGyroscopeInformationWindow = new InputGyroscopeInformationWindow();
        private InputCompassInformationWindow m_InputCompassInformationWindow = new InputCompassInformationWindow();
        private PathInformationWindow m_PathInformationWindow = new PathInformationWindow();
        private SceneInformationWindow m_SceneInformationWindow = new SceneInformationWindow();
        private TimeInformationWindow m_TimeInformationWindow = new TimeInformationWindow();
        private QualityInformationWindow m_QualityInformationWindow = new QualityInformationWindow();
        private ProfilerInformationWindow m_ProfilerInformationWindow = new ProfilerInformationWindow();
        private WebPlayerInformationWindow m_WebPlayerInformationWindow = new WebPlayerInformationWindow();
        private RuntimeMemorySummaryWindow m_RuntimeMemorySummaryWindow = new RuntimeMemorySummaryWindow();
        private RuntimeMemoryInformationWindow<Object> m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<Object>();
        private RuntimeMemoryInformationWindow<Texture> m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
        private RuntimeMemoryInformationWindow<Mesh> m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
        private RuntimeMemoryInformationWindow<Material> m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
        private RuntimeMemoryInformationWindow<Shader> m_RuntimeMemoryShaderInformationWindow = new RuntimeMemoryInformationWindow<Shader>();
        private RuntimeMemoryInformationWindow<AnimationClip> m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
        private RuntimeMemoryInformationWindow<AudioClip> m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
        private RuntimeMemoryInformationWindow<Font> m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
        private RuntimeMemoryInformationWindow<TextAsset> m_RuntimeMemoryTextAssetInformationWindow = new RuntimeMemoryInformationWindow<TextAsset>();
        private RuntimeMemoryInformationWindow<ScriptableObject> m_RuntimeMemoryScriptableObjectInformationWindow = new RuntimeMemoryInformationWindow<ScriptableObject>();
        private ObjectPoolInformationWindow m_ObjectPoolInformationWindow = new ObjectPoolInformationWindow();
        private ReferencePoolInformationWindow m_ReferencePoolInformationWindow = new ReferencePoolInformationWindow();
        private NetworkInformationWindow m_NetworkInformationWindow = new NetworkInformationWindow();
        private SettingsWindow m_SettingsWindow = new SettingsWindow();
        private OperationsWindow m_OperationsWindow = new OperationsWindow();

        private FpsCounter m_FpsCounter = null;

        /// <summary>
        /// 获取或设置调试器窗口是否激活。
        /// </summary>
        public bool ActiveWindow
        {
            get
            {
                return m_DebuggerManager.ActiveWindow;
            }
            set
            {
                m_DebuggerManager.ActiveWindow = value;
                enabled = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示完整调试器界面。
        /// </summary>
        public bool ShowFullWindow
        {
            get
            {
                return m_ShowFullWindow;
            }
            set
            {
                m_ShowFullWindow = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器漂浮框大小。
        /// </summary>
        public Rect IconRect
        {
            get
            {
                return m_IconRect;
            }
            set
            {
                m_IconRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口大小。
        /// </summary>
        public Rect WindowRect
        {
            get
            {
                return m_WindowRect;
            }
            set
            {
                m_WindowRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口缩放比例。
        /// </summary>
        public float WindowScale
        {
            get
            {
                return m_WindowScale;
            }
            set
            {
                m_WindowScale = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_DebuggerManager = GameFrameworkEntry.GetModule<IDebuggerManager>();
            if (m_DebuggerManager == null)
            {
                Log.Fatal("Debugger manager is invalid.");
                return;
            }

            m_FpsCounter = new FpsCounter(0.5f);
        }

        private void Start()
        {
            RegisterDebuggerWindow("Console", m_ConsoleWindow);
            RegisterDebuggerWindow("Information/System", m_SystemInformationWindow);
            RegisterDebuggerWindow("Information/Environment", m_EnvironmentInformationWindow);
            RegisterDebuggerWindow("Information/Screen", m_ScreenInformationWindow);
            RegisterDebuggerWindow("Information/Graphics", m_GraphicsInformationWindow);
            RegisterDebuggerWindow("Information/Input/Summary", m_InputSummaryInformationWindow);
            RegisterDebuggerWindow("Information/Input/Touch", m_InputTouchInformationWindow);
            RegisterDebuggerWindow("Information/Input/Location", m_InputLocationInformationWindow);
            RegisterDebuggerWindow("Information/Input/Acceleration", m_InputAccelerationInformationWindow);
            RegisterDebuggerWindow("Information/Input/Gyroscope", m_InputGyroscopeInformationWindow);
            RegisterDebuggerWindow("Information/Input/Compass", m_InputCompassInformationWindow);
            RegisterDebuggerWindow("Information/Other/Scene", m_SceneInformationWindow);
            RegisterDebuggerWindow("Information/Other/Path", m_PathInformationWindow);
            RegisterDebuggerWindow("Information/Other/Time", m_TimeInformationWindow);
            RegisterDebuggerWindow("Information/Other/Quality", m_QualityInformationWindow);
            RegisterDebuggerWindow("Information/Other/Web Player", m_WebPlayerInformationWindow);
            RegisterDebuggerWindow("Profiler/Summary", m_ProfilerInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Summary", m_RuntimeMemorySummaryWindow);
            RegisterDebuggerWindow("Profiler/Memory/All", m_RuntimeMemoryAllInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Texture", m_RuntimeMemoryTextureInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Mesh", m_RuntimeMemoryMeshInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Material", m_RuntimeMemoryMaterialInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Shader", m_RuntimeMemoryShaderInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/AnimationClip", m_RuntimeMemoryAnimationClipInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/AudioClip", m_RuntimeMemoryAudioClipInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/Font", m_RuntimeMemoryFontInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/TextAsset", m_RuntimeMemoryTextAssetInformationWindow);
            RegisterDebuggerWindow("Profiler/Memory/ScriptableObject", m_RuntimeMemoryScriptableObjectInformationWindow);
            RegisterDebuggerWindow("Profiler/Object Pool", m_ObjectPoolInformationWindow);
            RegisterDebuggerWindow("Profiler/Reference Pool", m_ReferencePoolInformationWindow);
            RegisterDebuggerWindow("Profiler/Network", m_NetworkInformationWindow);
            RegisterDebuggerWindow("Other/Settings", m_SettingsWindow);
            RegisterDebuggerWindow("Other/Operations", m_OperationsWindow);

            switch (m_ActiveWindow)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    ActiveWindow = true;
                    break;

                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    ActiveWindow = Debug.isDebugBuild;
                    break;

                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    ActiveWindow = Application.isEditor;
                    break;

                default:
                    ActiveWindow = false;
                    break;
            }
        }

        private void Update()
        {
            m_FpsCounter.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnGUI()
        {
            if (m_DebuggerManager == null || !m_DebuggerManager.ActiveWindow)
            {
                return;
            }

            GUISkin cachedGuiSkin = GUI.skin;
            Matrix4x4 cachedMatrix = GUI.matrix;

            GUI.skin = m_Skin;
            GUI.matrix = Matrix4x4.Scale(new Vector3(m_WindowScale, m_WindowScale, 1f));

            if (m_ShowFullWindow)
            {
                m_WindowRect = GUILayout.Window(0, m_WindowRect, DrawWindow, "<b>GAME FRAMEWORK DEBUGGER</b>");
            }
            else
            {
                m_IconRect = GUILayout.Window(0, m_IconRect, DrawDebuggerWindowIcon, "<b>DEBUGGER</b>");
            }

            GUI.matrix = cachedMatrix;
            GUI.skin = cachedGuiSkin;
        }

        /// <summary>
        /// 注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试器窗口。</param>
        /// <param name="args">初始化调试器窗口参数。</param>
        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            m_DebuggerManager.RegisterDebuggerWindow(path, debuggerWindow, args);
        }

        /// <summary>
        /// 解除注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否解除注册调试器窗口成功。</returns>
        public bool UnregisterDebuggerWindow(string path)
        {
            return m_DebuggerManager.UnregisterDebuggerWindow(path);
        }

        /// <summary>
        /// 获取调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>要获取的调试器窗口。</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return m_DebuggerManager.GetDebuggerWindow(path);
        }

        /// <summary>
        /// 选中调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否成功选中调试器窗口。</returns>
        public bool SelectDebuggerWindow(string path)
        {
            return m_DebuggerManager.SelectDebuggerWindow(path);
        }

        /// <summary>
        /// 还原调试器窗口布局。
        /// </summary>
        public void ResetLayout()
        {
            IconRect = DefaultIconRect;
            WindowRect = DefaultWindowRect;
            WindowScale = DefaultWindowScale;
        }

        /// <summary>
        /// 获取记录的所有日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        public void GetRecentLogs(List<LogNode> results)
        {
            m_ConsoleWindow.GetRecentLogs(results);
        }

        /// <summary>
        /// 获取记录的最近日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        /// <param name="count">要获取最近日志的数量。</param>
        public void GetRecentLogs(List<LogNode> results, int count)
        {
            m_ConsoleWindow.GetRecentLogs(results, count);
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            DrawDebuggerWindowGroup(m_DebuggerManager.DebuggerWindowRoot);
        }

        private void DrawDebuggerWindowGroup(IDebuggerWindowGroup debuggerWindowGroup)
        {
            if (debuggerWindowGroup == null)
            {
                return;
            }

            List<string> names = new List<string>();
            string[] debuggerWindowNames = debuggerWindowGroup.GetDebuggerWindowNames();
            for (int i = 0; i < debuggerWindowNames.Length; i++)
            {
                names.Add(Utility.Text.Format("<b>{0}</b>", debuggerWindowNames[i]));
            }

            if (debuggerWindowGroup == m_DebuggerManager.DebuggerWindowRoot)
            {
                names.Add("<b>Close</b>");
            }

            int toolbarIndex = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, names.ToArray(), GUILayout.Height(30f), GUILayout.MaxWidth(Screen.width));
            if (toolbarIndex >= debuggerWindowGroup.DebuggerWindowCount)
            {
                m_ShowFullWindow = false;
                return;
            }

            if (debuggerWindowGroup.SelectedWindow == null)
            {
                return;
            }

            if (debuggerWindowGroup.SelectedIndex != toolbarIndex)
            {
                debuggerWindowGroup.SelectedWindow.OnLeave();
                debuggerWindowGroup.SelectedIndex = toolbarIndex;
                debuggerWindowGroup.SelectedWindow.OnEnter();
            }

            IDebuggerWindowGroup subDebuggerWindowGroup = debuggerWindowGroup.SelectedWindow as IDebuggerWindowGroup;
            if (subDebuggerWindowGroup != null)
            {
                DrawDebuggerWindowGroup(subDebuggerWindowGroup);
            }

            debuggerWindowGroup.SelectedWindow.OnDraw();
        }

        private void DrawDebuggerWindowIcon(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            GUILayout.Space(5);
            Color32 color = Color.white;
            m_ConsoleWindow.RefreshCount();
            if (m_ConsoleWindow.FatalCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Exception);
            }
            else if (m_ConsoleWindow.ErrorCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Error);
            }
            else if (m_ConsoleWindow.WarningCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Warning);
            }
            else
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Log);
            }

            string title = Utility.Text.Format("<color=#{0}{1}{2}{3}><b>FPS: {4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_FpsCounter.CurrentFps.ToString("F2"));
            if (GUILayout.Button(title, GUILayout.Width(100f), GUILayout.Height(40f)))
            {
                m_ShowFullWindow = true;
            }
        }

        private static void CopyToClipboard(string content)
        {
            s_TextEditor.text = content;
            s_TextEditor.OnFocus();
            s_TextEditor.Copy();
            s_TextEditor.text = string.Empty;
        }
    }
}
