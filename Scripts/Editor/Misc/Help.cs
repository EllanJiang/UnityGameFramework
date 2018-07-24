//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

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
            ShowHelp(string.Format("http://gameframework.cn/archives/category/module/buildin/{0}/", componentName));
        }

        [MenuItem("Game Framework/Documentation", false, 90)]
        public static void ShowDocumentation()
        {
            ShowHelp("http://gameframework.cn/");
        }

        [MenuItem("Game Framework/API Reference", false, 91)]
        public static void ShowAPI()
        {
            ShowHelp("http://gameframework.cn/api/");
        }

        private static void ShowHelp(string uri)
        {
            Application.OpenURL(uri);
        }
    }
}
