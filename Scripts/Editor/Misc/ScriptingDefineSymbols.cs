//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 脚本宏定义。
    /// </summary>
    public static class ScriptingDefineSymbols
    {
        private static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android,
            BuildTargetGroup.WSA,
            BuildTargetGroup.WebGL
        };

        /// <summary>
        /// 检查指定平台是否存在指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要检查脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要检查的脚本宏定义。</param>
        /// <returns>指定平台是否存在指定的脚本宏定义。</returns>
        public static bool HasScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return false;
            }

            string[] scriptingDefineSymbols = GetScriptingDefineSymbols(buildTargetGroup);
            foreach (string i in scriptingDefineSymbols)
            {
                if (i == scriptingDefineSymbol)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 为指定平台增加指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要增加脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要增加的脚本宏定义。</param>
        public static void AddScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup))
            {
                scriptingDefineSymbol
            };

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// 为指定平台移除指定的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要移除脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbol">要移除的脚本宏定义。</param>
        public static void RemoveScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (!HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup));
            while (scriptingDefineSymbols.Contains(scriptingDefineSymbol))
            {
                scriptingDefineSymbols.Remove(scriptingDefineSymbol);
            }

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// 为所有平台增加指定的脚本宏定义。
        /// </summary>
        /// <param name="scriptingDefineSymbol">要增加的脚本宏定义。</param>
        public static void AddScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                AddScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// 为所有平台移除指定的脚本宏定义。
        /// </summary>
        /// <param name="scriptingDefineSymbol">要移除的脚本宏定义。</param>
        public static void RemoveScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                RemoveScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// 获取指定平台的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要获取脚本宏定义的平台。</param>
        /// <returns></returns>
        public static string[] GetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
        }

        /// <summary>
        /// 设置指定平台的脚本宏定义。
        /// </summary>
        /// <param name="buildTargetGroup">要设置脚本宏定义的平台。</param>
        /// <param name="scriptingDefineSymbols">要设置的脚本宏定义。</param>
        public static void SetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string[] scriptingDefineSymbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", scriptingDefineSymbols));
        }
    }
}
