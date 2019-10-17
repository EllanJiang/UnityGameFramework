//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 帮助相关的实用函数。
    /// </summary>
    public static class Help
    {
        public static void ShowComponentHelp(string componentName)
        {
            ShowHelp(Utility.Text.Format("https://gameframework.cn/document/{0}/", componentName));
        }

        [MenuItem("Game Framework/Documentation", false, 90)]
        public static void ShowDocumentation()
        {
            ShowHelp("https://gameframework.cn/document/");
        }

        [MenuItem("Game Framework/API Reference", false, 91)]
        public static void ShowApiReference()
        {
            ShowHelp("https://gameframework.cn/api/");
        }

        private static void ShowHelp(string uri)
        {
            Application.OpenURL(uri);
        }
    }
}
