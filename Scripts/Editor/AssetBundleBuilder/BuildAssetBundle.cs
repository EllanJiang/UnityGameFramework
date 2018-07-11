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
            Run(null, null, null);
        }

        public static void Run(int internalResourceVersion)
        {
            Run(internalResourceVersion, null, null);
        }

        public static void Run(string outputDirectory)
        {
            Run(null, outputDirectory, null);
        }

        public static void Run(int internalResourceVersion, string outputDirectory)
        {
            Run(internalResourceVersion, outputDirectory, null);
        }

        public static void Run(int internalResourceVersion, string outputDirectory, string buildEventHandlerTypeName)
        {
            Run(internalResourceVersion, outputDirectory, buildEventHandlerTypeName);
        }

        private static void Run(int? internalResourceVersion, string outputDirectory, string buildEventHandlerTypeName)
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
