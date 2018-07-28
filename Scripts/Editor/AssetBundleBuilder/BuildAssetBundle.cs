//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 生成资源包。
    /// </summary>
    public static class BuildAssetBundle
    {
        /// <summary>
        /// 运行生成资源包。
        /// </summary>
        [MenuItem("Game Framework/AssetBundle Tools/Build AssetBundle", false, 30)]
        public static void Run()
        {
            Run(null, Platform.Undefined, null, null);
        }

        public static void Run(int internalResourceVersion)
        {
            Run((int?)internalResourceVersion, Platform.Undefined, null, null);
        }

        public static void Run(Platform platforms)
        {
            Run(null, platforms, null, null);
        }

        public static void Run(string outputDirectory)
        {
            Run(null, Platform.Undefined, outputDirectory, null);
        }

        public static void Run(int internalResourceVersion, Platform platforms)
        {
            Run((int?)internalResourceVersion, platforms, null, null);
        }

        public static void Run(int internalResourceVersion, string outputDirectory)
        {
            Run((int?)internalResourceVersion, Platform.Undefined, outputDirectory, null);
        }

        public static void Run(Platform platforms, string outputDirectory)
        {
            Run(null, platforms, outputDirectory, null);
        }

        public static void Run(string outputDirectory, string buildEventHandlerTypeName)
        {
            Run(null, Platform.Undefined, outputDirectory, buildEventHandlerTypeName);
        }

        public static void Run(int internalResourceVersion, Platform platforms, string outputDirectory)
        {
            Run((int?)internalResourceVersion, platforms, outputDirectory, null);
        }

        public static void Run(int internalResourceVersion, string outputDirectory, string buildEventHandlerTypeName)
        {
            Run((int?)internalResourceVersion, Platform.Undefined, outputDirectory, buildEventHandlerTypeName);
        }

        public static void Run(Platform platforms, string outputDirectory, string buildEventHandlerTypeName)
        {
            Run(null, platforms, outputDirectory, buildEventHandlerTypeName);
        }

        public static void Run(int internalResourceVersion, Platform platforms, string outputDirectory, string buildEventHandlerTypeName)
        {
            Run((int?)internalResourceVersion, platforms, outputDirectory, buildEventHandlerTypeName);
        }

        private static void Run(int? internalResourceVersion, Platform platforms, string outputDirectory, string buildEventHandlerTypeName)
        {
            AssetBundleBuilderController controller = new AssetBundleBuilderController();
            if (!controller.Load())
            {
                throw new GameFrameworkException("Load configuration failure.");
            }
            else
            {
                Debug.Log("Load configuration success.");
            }

            if (platforms != Platform.Undefined)
            {
                controller.Platforms = platforms;
            }

            if (internalResourceVersion.HasValue)
            {
                controller.InternalResourceVersion = internalResourceVersion.Value;
            }

            if (outputDirectory != null)
            {
                controller.OutputDirectory = outputDirectory;
            }

            if (buildEventHandlerTypeName != null)
            {
                controller.BuildEventHandlerTypeName = buildEventHandlerTypeName;
            }

            if (!controller.IsValidOutputDirectory)
            {
                throw new GameFrameworkException(string.Format("Output directory '{0}' is invalid.", controller.OutputDirectory));
            }

            if (!controller.BuildAssetBundles())
            {
                throw new GameFrameworkException("Build AssetBundles failure.");
            }
            else
            {
                Debug.Log("Build AssetBundles success.");
                controller.Save();
            }
        }
    }
}
