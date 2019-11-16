//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包同步工具。
    /// </summary>
    internal sealed class AssetBundleSyncTools : EditorWindow
    {
        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Sync Tools", false, 44)]
        private static void Open()
        {
            AssetBundleSyncTools window = GetWindow<AssetBundleSyncTools>("AB Sync Tools", true);
            window.minSize = new Vector2(400, 300f);
        }

        private void OnEnable()
        {
        }

        private void Update()
        {
        }

        private void OnGUI()
        {
        }
    }
}
