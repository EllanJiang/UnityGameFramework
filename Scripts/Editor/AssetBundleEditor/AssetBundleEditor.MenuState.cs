//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
