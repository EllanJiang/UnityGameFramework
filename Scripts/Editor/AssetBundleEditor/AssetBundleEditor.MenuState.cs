//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleEditor : EditorWindow
    {
        private enum MenuState
        {
            Normal,
            Add,
            Rename,
            Remove,
        }
    }
}
