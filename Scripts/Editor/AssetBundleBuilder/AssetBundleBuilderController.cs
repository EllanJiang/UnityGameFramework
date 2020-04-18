//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public sealed partial class AssetBundleBuilderController
    {
        private const string DefaultExtension = "dat";
        private const string VersionListFileName = "version.dat";
        private const string ResourceListFileName = "list.dat";
        private const string NoneOptionName = "<None>";
        private static readonly int AssetsStringLength = "Assets".Length;

        private readonly string m_ConfigurationPath;
        private readonly AssetBundleCollection m_AssetBundleCollection;
        private readonly AssetBundleAnalyzerController m_AssetBundleAnalyzerController;
        private readonly SortedDictionary<string, AssetBundleData> m_AssetBundleDatas;
        private readonly BuildReport m_BuildReport;
        private readonly List<string> m_BuildEventHandlerTypeNames;
        private IBuildEventHandler m_BuildEventHandler;

        public AssetBundleBuilderController()
        {
            m_ConfigurationPath = Type.GetConfigurationPath<AssetBundleBuilderConfigPathAttribute>() ?? Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameFramework/Configs/AssetBundleBuilder.xml"));

            m_AssetBundleCollection = new AssetBundleCollection();

            m_AssetBundleCollection.OnLoadingAssetBundle += delegate (int index, int count)
            {
                if (OnLoadingAssetBundle != null)
                {
                    OnLoadingAssetBundle(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadCompleted += delegate ()
            {
                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }
            };

            m_AssetBundleAnalyzerController = new AssetBundleAnalyzerController(m_AssetBundleCollection);

            m_AssetBundleAnalyzerController.OnAnalyzingAsset += delegate (int index, int count)
            {
                if (OnAnalyzingAsset != null)
                {
                    OnAnalyzingAsset(index, count);
                }
            };

            m_AssetBundleAnalyzerController.OnAnalyzeCompleted += delegate ()
            {
                if (OnAnalyzeCompleted != null)
                {
                    OnAnalyzeCompleted();
                }
            };

            m_AssetBundleDatas = new SortedDictionary<string, AssetBundleData>();
            m_BuildReport = new BuildReport();

            m_BuildEventHandlerTypeNames = new List<string>
            {
                NoneOptionName
            };

            m_BuildEventHandlerTypeNames.AddRange(Type.GetEditorTypeNames(typeof(IBuildEventHandler)));
            m_BuildEventHandler = null;

            Platforms = Platform.Undefined;
            ZipSelected = true;
            DeterministicAssetBundleSelected = ChunkBasedCompressionSelected = true;
            UncompressedAssetBundleSelected = DisableWriteTypeTreeSelected = ForceRebuildAssetBundleSelected = IgnoreTypeTreeChangesSelected = AppendHashToAssetBundleNameSelected = false;
            OutputPackageSelected = OutputFullSelected = OutputPackedSelected = true;
            BuildEventHandlerTypeName = string.Empty;
            OutputDirectory = string.Empty;
        }

        public string ProductName
        {
            get
            {
                return PlayerSettings.productName;
            }
        }

        public string CompanyName
        {
            get
            {
                return PlayerSettings.companyName;
            }
        }

        public string GameIdentifier
        {
            get
            {
#if UNITY_5_6_OR_NEWER
                return PlayerSettings.applicationIdentifier;
#else
                return PlayerSettings.bundleIdentifier;
#endif
            }
        }

        public string ApplicableGameVersion
        {
            get
            {
                return Application.version;
            }
        }

        public int InternalResourceVersion
        {
            get;
            set;
        }

        public string UnityVersion
        {
            get
            {
                return Application.unityVersion;
            }
        }

        public Platform Platforms
        {
            get;
            set;
        }

        public bool ZipSelected
        {
            get;
            set;
        }

        public bool UncompressedAssetBundleSelected
        {
            get;
            set;
        }

        public bool DisableWriteTypeTreeSelected
        {
            get;
            set;
        }

        public bool DeterministicAssetBundleSelected
        {
            get;
            set;
        }

        public bool ForceRebuildAssetBundleSelected
        {
            get;
            set;
        }

        public bool IgnoreTypeTreeChangesSelected
        {
            get;
            set;
        }

        public bool AppendHashToAssetBundleNameSelected
        {
            get;
            set;
        }

        public bool ChunkBasedCompressionSelected
        {
            get;
            set;
        }

        public bool OutputPackageSelected
        {
            get;
            set;
        }

        public bool OutputFullSelected
        {
            get;
            set;
        }

        public bool OutputPackedSelected
        {
            get;
            set;
        }

        public string BuildEventHandlerTypeName
        {
            get;
            set;
        }

        public string OutputDirectory
        {
            get;
            set;
        }

        public bool IsValidOutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(OutputDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(OutputDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string WorkingPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Working/", OutputDirectory)).FullName);
            }
        }

        public string OutputPackagePath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Package/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString())).FullName);
            }
        }

        public string OutputFullPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Full/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString())).FullName);
            }
        }

        public string OutputPackedPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Packed/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString())).FullName);
            }
        }

        public string BuildReportPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/BuildReport/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString())).FullName);
            }
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public event GameFrameworkAction<int, int> OnAnalyzingAsset = null;

        public event GameFrameworkAction OnAnalyzeCompleted = null;

        public event GameFrameworkFunc<string, float, bool> ProcessingAssetBundle = null;

        public event GameFrameworkFunc<string, float, bool> ProcessingBinary = null;

        public event GameFrameworkAction<Platform> ProcessAssetBundleComplete = null;

        public event GameFrameworkAction<string> BuildAssetBundlesError = null;

        public bool Load()
        {
            if (!File.Exists(m_ConfigurationPath))
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(m_ConfigurationPath);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("AssetBundleBuilder");
                XmlNode xmlSettings = xmlEditor.SelectSingleNode("Settings");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlSettings.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "InternalResourceVersion":
                            InternalResourceVersion = int.Parse(xmlNode.InnerText) + 1;
                            break;

                        case "Platforms":
                            Platforms = (Platform)int.Parse(xmlNode.InnerText);
                            break;

                        case "ZipSelected":
                            ZipSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "UncompressedAssetBundleSelected":
                            UncompressedAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            if (UncompressedAssetBundleSelected)
                            {
                                ChunkBasedCompressionSelected = false;
                            }
                            break;

                        case "DisableWriteTypeTreeSelected":
                            DisableWriteTypeTreeSelected = bool.Parse(xmlNode.InnerText);
                            if (DisableWriteTypeTreeSelected)
                            {
                                IgnoreTypeTreeChangesSelected = false;
                            }
                            break;

                        case "DeterministicAssetBundleSelected":
                            DeterministicAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "ForceRebuildAssetBundleSelected":
                            ForceRebuildAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "IgnoreTypeTreeChangesSelected":
                            IgnoreTypeTreeChangesSelected = bool.Parse(xmlNode.InnerText);
                            if (IgnoreTypeTreeChangesSelected)
                            {
                                DisableWriteTypeTreeSelected = false;
                            }
                            break;

                        case "AppendHashToAssetBundleNameSelected":
                            AppendHashToAssetBundleNameSelected = false;
                            break;

                        case "ChunkBasedCompressionSelected":
                            ChunkBasedCompressionSelected = bool.Parse(xmlNode.InnerText);
                            if (ChunkBasedCompressionSelected)
                            {
                                UncompressedAssetBundleSelected = false;
                            }
                            break;

                        case "OutputPackageSelected":
                            OutputPackageSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "OutputFullSelected":
                            OutputFullSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "OutputPackedSelected":
                            OutputPackedSelected = bool.Parse(xmlNode.InnerText);
                            break;

                        case "BuildEventHandlerTypeName":
                            BuildEventHandlerTypeName = xmlNode.InnerText;
                            RefreshBuildEventHandler();
                            break;

                        case "OutputDirectory":
                            OutputDirectory = xmlNode.InnerText;
                            break;
                    }
                }
            }
            catch
            {
                File.Delete(m_ConfigurationPath);
                return false;
            }

            return true;
        }

        public bool Save()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement xmlRoot = xmlDocument.CreateElement("UnityGameFramework");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlBuilder = xmlDocument.CreateElement("AssetBundleBuilder");
                xmlRoot.AppendChild(xmlBuilder);

                XmlElement xmlSettings = xmlDocument.CreateElement("Settings");
                xmlBuilder.AppendChild(xmlSettings);

                XmlElement xmlElement = null;

                xmlElement = xmlDocument.CreateElement("InternalResourceVersion");
                xmlElement.InnerText = InternalResourceVersion.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("Platforms");
                xmlElement.InnerText = ((int)Platforms).ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ZipSelected");
                xmlElement.InnerText = ZipSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("UncompressedAssetBundleSelected");
                xmlElement.InnerText = UncompressedAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("DisableWriteTypeTreeSelected");
                xmlElement.InnerText = DisableWriteTypeTreeSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("DeterministicAssetBundleSelected");
                xmlElement.InnerText = DeterministicAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ForceRebuildAssetBundleSelected");
                xmlElement.InnerText = ForceRebuildAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("IgnoreTypeTreeChangesSelected");
                xmlElement.InnerText = IgnoreTypeTreeChangesSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AppendHashToAssetBundleNameSelected");
                xmlElement.InnerText = AppendHashToAssetBundleNameSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ChunkBasedCompressionSelected");
                xmlElement.InnerText = ChunkBasedCompressionSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputPackageSelected");
                xmlElement.InnerText = OutputPackageSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputFullSelected");
                xmlElement.InnerText = OutputFullSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputPackedSelected");
                xmlElement.InnerText = OutputPackedSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("BuildEventHandlerTypeName");
                xmlElement.InnerText = BuildEventHandlerTypeName;
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputDirectory");
                xmlElement.InnerText = OutputDirectory;
                xmlSettings.AppendChild(xmlElement);

                string configurationDirectoryName = Path.GetDirectoryName(m_ConfigurationPath);
                if (!Directory.Exists(configurationDirectoryName))
                {
                    Directory.CreateDirectory(configurationDirectoryName);
                }

                xmlDocument.Save(m_ConfigurationPath);
                AssetDatabase.Refresh();
                return true;
            }
            catch
            {
                if (File.Exists(m_ConfigurationPath))
                {
                    File.Delete(m_ConfigurationPath);
                }

                return false;
            }
        }

        public void SetBuildEventHandler(IBuildEventHandler buildEventHandler)
        {
            m_BuildEventHandler = buildEventHandler;
        }

        public string[] GetBuildEventHandlerTypeNames()
        {
            return m_BuildEventHandlerTypeNames.ToArray();
        }

        public bool IsPlatformSelected(Platform platform)
        {
            return (Platforms & platform) != 0;
        }

        public void SelectPlatform(Platform platform, bool selected)
        {
            if (selected)
            {
                Platforms |= platform;
            }
            else
            {
                Platforms &= ~platform;
            }
        }

        public bool RefreshBuildEventHandler()
        {
            bool retVal = false;
            if (!string.IsNullOrEmpty(BuildEventHandlerTypeName) && m_BuildEventHandlerTypeNames.Contains(BuildEventHandlerTypeName))
            {
                System.Type buildEventHandlerType = Utility.Assembly.GetType(BuildEventHandlerTypeName);
                if (buildEventHandlerType != null)
                {
                    IBuildEventHandler buildEventHandler = (IBuildEventHandler)Activator.CreateInstance(buildEventHandlerType);
                    if (buildEventHandler != null)
                    {
                        SetBuildEventHandler(buildEventHandler);
                        return true;
                    }
                }
            }
            else
            {
                retVal = true;
            }

            BuildEventHandlerTypeName = string.Empty;
            SetBuildEventHandler(null);
            return retVal;
        }

        public bool BuildAssetBundles()
        {
            if (!IsValidOutputDirectory)
            {
                return false;
            }

            Utility.Zip.SetZipHelper(new DefaultZipHelper());

            if (Directory.Exists(OutputPackagePath))
            {
                Directory.Delete(OutputPackagePath, true);
            }

            Directory.CreateDirectory(OutputPackagePath);

            if (Directory.Exists(OutputFullPath))
            {
                Directory.Delete(OutputFullPath, true);
            }

            Directory.CreateDirectory(OutputFullPath);

            if (Directory.Exists(OutputPackedPath))
            {
                Directory.Delete(OutputPackedPath, true);
            }

            Directory.CreateDirectory(OutputPackedPath);

            if (Directory.Exists(BuildReportPath))
            {
                Directory.Delete(BuildReportPath, true);
            }

            Directory.CreateDirectory(BuildReportPath);

            BuildAssetBundleOptions buildAssetBundleOptions = GetBuildAssetBundleOptions();
            m_BuildReport.Initialize(BuildReportPath, ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion,
                Platforms, ZipSelected, (int)buildAssetBundleOptions, m_AssetBundleDatas);

            try
            {
                m_BuildReport.LogInfo("Build Start Time: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PreprocessAllPlatforms'...");
                    m_BuildEventHandler.PreprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                }

                m_BuildReport.LogInfo("Start prepare AssetBundle collection...");
                if (!m_AssetBundleCollection.Load())
                {
                    m_BuildReport.LogError("Can not parse 'AssetBundleCollection.xml', please use 'AssetBundle Editor' tool first.");

                    if (m_BuildEventHandler != null)
                    {
                        m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                    }

                    m_BuildReport.SaveReport();
                    return false;
                }

                if (Platforms == Platform.Undefined)
                {
                    m_BuildReport.LogError("Platform undefined.");

                    if (m_BuildEventHandler != null)
                    {
                        m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                    }

                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare AssetBundle collection complete.");
                m_BuildReport.LogInfo("Start analyze assets dependency...");

                m_AssetBundleAnalyzerController.Analyze();

                m_BuildReport.LogInfo("Analyze assets dependency complete.");
                m_BuildReport.LogInfo("Start prepare build map...");

                AssetBundleBuild[] assetBundleBuildMap = null;
                BinaryBuild[] binaryBuildMap = null;
                if (!GetBuildMap(out assetBundleBuildMap, out binaryBuildMap))
                {
                    m_BuildReport.LogError("Get build map failure.");

                    if (m_BuildEventHandler != null)
                    {
                        m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                    }

                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare build map complete.");
                m_BuildReport.LogInfo("Start build AssetBundles for selected platforms...");

                bool watchResult = m_BuildEventHandler == null || !m_BuildEventHandler.ContinueOnFailure;
                bool isSuccess = false;
                isSuccess = BuildAssetBundles(Platform.Windows, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.Windows64, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.MacOS, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.Linux, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.IOS, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.Android, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.WindowsStore, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildAssetBundles(Platform.WebGL, assetBundleBuildMap, binaryBuildMap, buildAssetBundleOptions);
                }

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                    m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                }

                m_BuildReport.LogInfo("Build AssetBundles for selected platforms complete.");
                m_BuildReport.SaveReport();
                return true;
            }
            catch (Exception exception)
            {
                string errorMessage = exception.ToString();
                m_BuildReport.LogFatal(errorMessage);
                m_BuildReport.SaveReport();
                if (BuildAssetBundlesError != null)
                {
                    BuildAssetBundlesError(errorMessage);
                }

                return false;
            }
        }

        private bool BuildAssetBundles(Platform platform, AssetBundleBuild[] assetBundleBuildMap, BinaryBuild[] binaryBuildMap, BuildAssetBundleOptions buildOptions)
        {
            if (!IsPlatformSelected(platform))
            {
                return true;
            }

            string platformName = platform.ToString();
            m_BuildReport.LogInfo("Start build AssetBundles for '{0}'...", platformName);

            string workingPath = Utility.Text.Format("{0}{1}/", WorkingPath, platformName);
            m_BuildReport.LogInfo("Working path is '{0}'.", workingPath);

            string outputPackagePath = Utility.Text.Format("{0}{1}/", OutputPackagePath, platformName);
            if (OutputPackageSelected)
            {
                Directory.CreateDirectory(outputPackagePath);
                m_BuildReport.LogInfo("Output package is selected, path is '{0}'.", outputPackagePath);
            }
            else
            {
                m_BuildReport.LogInfo("Output package is not selected.");
            }

            string outputFullPath = Utility.Text.Format("{0}{1}/", OutputFullPath, platformName);
            if (OutputFullSelected)
            {
                Directory.CreateDirectory(outputFullPath);
                m_BuildReport.LogInfo("Output full is selected, path is '{0}'.", outputFullPath);
            }
            else
            {
                m_BuildReport.LogInfo("Output full is not selected.");
            }

            string outputPackedPath = Utility.Text.Format("{0}{1}/", OutputPackedPath, platformName);
            if (OutputPackedSelected)
            {
                Directory.CreateDirectory(outputPackedPath);
                m_BuildReport.LogInfo("Output packed is selected, path is '{0}'.", outputPackedPath);
            }
            else
            {
                m_BuildReport.LogInfo("Output packed is not selected.");
            }

            // Clean working path
            List<string> validNames = new List<string>();
            foreach (AssetBundleBuild i in assetBundleBuildMap)
            {
                string assetBundleName = GetAssetBundleFullName(i.assetBundleName, i.assetBundleVariant);
                validNames.Add(assetBundleName);
            }

            if (Directory.Exists(workingPath))
            {
                Uri workingUri = new Uri(workingPath, UriKind.Absolute);
                string[] fileNames = Directory.GetFiles(workingPath, "*", SearchOption.AllDirectories);
                foreach (string fileName in fileNames)
                {
                    if (fileName.EndsWith(".manifest"))
                    {
                        continue;
                    }

                    string relativeName = workingUri.MakeRelativeUri(new Uri(fileName, UriKind.Absolute)).ToString();
                    if (!validNames.Contains(relativeName))
                    {
                        File.Delete(fileName);
                    }
                }

                string[] manifestNames = Directory.GetFiles(workingPath, "*.manifest", SearchOption.AllDirectories);
                foreach (string manifestName in manifestNames)
                {
                    if (!File.Exists(manifestName.Substring(0, manifestName.LastIndexOf('.'))))
                    {
                        File.Delete(manifestName);
                    }
                }

                Utility.Path.RemoveEmptyDirectory(workingPath);
            }

            if (!Directory.Exists(workingPath))
            {
                Directory.CreateDirectory(workingPath);
            }

            if (m_BuildEventHandler != null)
            {
                m_BuildReport.LogInfo("Execute build event handler 'PreprocessPlatform' for '{0}'...", platformName);
                m_BuildEventHandler.PreprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath);
            }

            // Build AssetBundles
            m_BuildReport.LogInfo("Unity start build AssetBundles for '{0}'...", platformName);
            AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(workingPath, assetBundleBuildMap, buildOptions, GetBuildTarget(platform));
            if (assetBundleManifest == null)
            {
                m_BuildReport.LogError("Build AssetBundles for '{0}' failure.", platformName);

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PostprocessPlatform' for '{0}'...", platformName);
                    m_BuildEventHandler.PostprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath, false);
                }

                return false;
            }

            m_BuildReport.LogInfo("Unity build AssetBundles for '{0}' complete.", platformName);

            // Process AssetBundles
            for (int i = 0; i < assetBundleBuildMap.Length; i++)
            {
                string assetBundleFullName = GetAssetBundleFullName(assetBundleBuildMap[i].assetBundleName, assetBundleBuildMap[i].assetBundleVariant);
                if (ProcessingAssetBundle != null)
                {
                    if (ProcessingAssetBundle(assetBundleFullName, (float)(i + 1) / assetBundleBuildMap.Length))
                    {
                        m_BuildReport.LogWarning("The build has been canceled by user.");

                        if (m_BuildEventHandler != null)
                        {
                            m_BuildReport.LogInfo("Execute build event handler 'PostprocessPlatform' for '{0}'...", platformName);
                            m_BuildEventHandler.PostprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath, false);
                        }

                        return false;
                    }
                }

                m_BuildReport.LogInfo("Start process '{0}' for '{1}'...", assetBundleFullName, platformName);

                ProcessAssetBundle(platform, workingPath, outputPackagePath, outputFullPath, outputPackedPath, ZipSelected, assetBundleBuildMap[i].assetBundleName, assetBundleBuildMap[i].assetBundleVariant);

                m_BuildReport.LogInfo("Process '{0}' for '{1}' complete.", assetBundleFullName, platformName);
            }

            // Process Binaries
            for (int i = 0; i < binaryBuildMap.Length; i++)
            {
                string binaryFullName = GetAssetBundleFullName(binaryBuildMap[i].resourceName, binaryBuildMap[i].resourceVariant);
                if (ProcessingBinary != null)
                {
                    if (ProcessingBinary(binaryFullName, (float)(i + 1) / binaryBuildMap.Length))
                    {
                        m_BuildReport.LogWarning("The build has been canceled by user.");

                        if (m_BuildEventHandler != null)
                        {
                            m_BuildReport.LogInfo("Execute build event handler 'PostprocessPlatform' for '{0}'...", platformName);
                            m_BuildEventHandler.PostprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath, false);
                        }

                        return false;
                    }
                }

                m_BuildReport.LogInfo("Start process '{0}' for '{1}'...", binaryFullName, platformName);

                ProcessBinary(platform, workingPath, outputPackagePath, outputFullPath, outputPackedPath, ZipSelected, binaryBuildMap[i].resourceName, binaryBuildMap[i].resourceVariant);

                m_BuildReport.LogInfo("Process '{0}' for '{1}' complete.", binaryFullName, platformName);
            }

            if (OutputPackageSelected)
            {
                ProcessPackageVersionList(outputPackagePath, platform);
                m_BuildReport.LogInfo("Process package version list for '{0}' complete.", platformName);
            }

            if (OutputFullSelected)
            {
                VersionListData versionListData = ProcessUpdatableVersionList(outputFullPath, platform);
                m_BuildReport.LogInfo("Process updatable version list for '{0}' complete, updatable version list path is '{1}', length is '{2}', hash code is '{3}[0x{3:X8}]', zip length is '{4}', zip hash code is '{5}[0x{5:X8}]'.", platformName, versionListData.Path, versionListData.Length.ToString(), versionListData.HashCode, versionListData.ZipLength.ToString(), versionListData.ZipHashCode);
            }

            if (OutputPackedSelected)
            {
                ProcessReadOnlyVersionList(outputPackedPath, platform);
                m_BuildReport.LogInfo("Process read only version list for '{0}' complete.", platformName);
            }

            if (m_BuildEventHandler != null)
            {
                m_BuildReport.LogInfo("Execute build event handler 'PostprocessPlatform' for '{0}'...", platformName);
                m_BuildEventHandler.PostprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath, true);
            }

            if (ProcessAssetBundleComplete != null)
            {
                ProcessAssetBundleComplete(platform);
            }

            m_BuildReport.LogInfo("Build AssetBundles for '{0}' success.", platformName);
            return true;
        }

        private void ProcessAssetBundle(Platform platform, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, bool zip, string assetBundleName, string assetBundleVariant)
        {
            string assetBundleFullName = GetAssetBundleFullName(assetBundleName, assetBundleVariant);
            AssetBundleData assetBundleData = m_AssetBundleDatas[assetBundleFullName];
            string assetBundleFullNameWithExtension = Utility.Text.Format("{0}.{1}", assetBundleFullName, GetExtension(assetBundleData));
            string workingName = Utility.Path.GetRegularPath(Path.Combine(workingPath, assetBundleFullName));

            byte[] bytes = File.ReadAllBytes(workingName);
            int length = bytes.Length;
            int hashCode = Utility.Verifier.GetCrc32(bytes);
            int zipLength = length;
            int zipHashCode = hashCode;

            byte[] hashBytes = Utility.Converter.GetBytes(hashCode);
            if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromMemoryAndQuickDecrypt)
            {
                bytes = Utility.Encryption.GetQuickXorBytes(bytes, hashBytes);
            }
            else if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromMemoryAndDecrypt)
            {
                bytes = Utility.Encryption.GetXorBytes(bytes, hashBytes);
            }

            // Package AssetBundle
            if (OutputPackageSelected)
            {
                string packageName = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, assetBundleFullNameWithExtension));
                string packageDirectoryName = Path.GetDirectoryName(packageName);
                if (!Directory.Exists(packageDirectoryName))
                {
                    Directory.CreateDirectory(packageDirectoryName);
                }

                File.WriteAllBytes(packageName, bytes);
            }

            // Packed AssetBundle
            if (OutputPackedSelected && assetBundleData.Packed)
            {
                string packedName = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, assetBundleFullNameWithExtension));
                string packedDirectoryName = Path.GetDirectoryName(packedName);
                if (!Directory.Exists(packedDirectoryName))
                {
                    Directory.CreateDirectory(packedDirectoryName);
                }

                File.WriteAllBytes(packedName, bytes);
            }

            // Full AssetBundle
            if (OutputFullSelected)
            {
                string assetBundleFullNameWithCrc32 = assetBundleVariant != null ? Utility.Text.Format("{0}.{1}.{3:x8}.{2}", assetBundleName, assetBundleVariant, DefaultExtension, hashCode) : Utility.Text.Format("{0}.{2:x8}.{1}", assetBundleName, DefaultExtension, hashCode);
                string fullName = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, assetBundleFullNameWithCrc32));
                string fullDirectoryName = Path.GetDirectoryName(fullName);
                if (!Directory.Exists(fullDirectoryName))
                {
                    Directory.CreateDirectory(fullDirectoryName);
                }

                if (zip)
                {
                    byte[] zipBytes = Utility.Zip.Compress(bytes);
                    zipLength = zipBytes.Length;
                    zipHashCode = Utility.Verifier.GetCrc32(zipBytes);
                    File.WriteAllBytes(fullName, zipBytes);
                }
                else
                {
                    File.WriteAllBytes(fullName, bytes);
                }
            }

            assetBundleData.AddCode(platform, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessBinary(Platform platform, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, bool zip, string resourceName, string resourceVariant)
        {
            string binaryFullName = GetAssetBundleFullName(resourceName, resourceVariant);
            AssetBundleData assetBundleData = m_AssetBundleDatas[binaryFullName];
            string binaryFullNameNameWithExtension = Utility.Text.Format("{0}.{1}", binaryFullName, GetExtension(assetBundleData));
            string binaryAssetName = assetBundleData.GetAssetNames()[0];
            string binaryPath = Utility.Path.GetRegularPath(Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + binaryAssetName);

            byte[] bytes = File.ReadAllBytes(binaryPath);
            int length = bytes.Length;
            int hashCode = Utility.Verifier.GetCrc32(bytes);
            int zipLength = length;
            int zipHashCode = hashCode;

            byte[] hashBytes = Utility.Converter.GetBytes(hashCode);
            if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromBinaryAndQuickDecrypt)
            {
                bytes = Utility.Encryption.GetQuickXorBytes(bytes, hashBytes);
            }
            else if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromBinaryAndDecrypt)
            {
                bytes = Utility.Encryption.GetXorBytes(bytes, hashBytes);
            }

            // Package Binary
            if (OutputPackageSelected)
            {
                string packageName = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, binaryFullNameNameWithExtension));
                string packageDirectoryName = Path.GetDirectoryName(packageName);
                if (!Directory.Exists(packageDirectoryName))
                {
                    Directory.CreateDirectory(packageDirectoryName);
                }

                File.WriteAllBytes(packageName, bytes);
            }

            // Packed Binary
            if (OutputPackedSelected && assetBundleData.Packed)
            {
                string packedName = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, binaryFullNameNameWithExtension));
                string packedDirectoryName = Path.GetDirectoryName(packedName);
                if (!Directory.Exists(packedDirectoryName))
                {
                    Directory.CreateDirectory(packedDirectoryName);
                }

                File.WriteAllBytes(packedName, bytes);
            }

            // Full Binary
            if (OutputFullSelected)
            {
                string binaryFullNameWithCrc32 = resourceVariant != null ? Utility.Text.Format("{0}.{1}.{3:x8}.{2}", resourceName, resourceVariant, DefaultExtension, hashCode) : Utility.Text.Format("{0}.{2:x8}.{1}", resourceName, DefaultExtension, hashCode);
                string fullName = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, binaryFullNameWithCrc32));
                string fullDirectoryName = Path.GetDirectoryName(fullName);
                if (!Directory.Exists(fullDirectoryName))
                {
                    Directory.CreateDirectory(fullDirectoryName);
                }

                if (zip)
                {
                    byte[] zipBytes = Utility.Zip.Compress(bytes);
                    zipLength = zipBytes.Length;
                    zipHashCode = Utility.Verifier.GetCrc32(zipBytes);
                    File.WriteAllBytes(fullName, zipBytes);
                }
                else
                {
                    File.WriteAllBytes(fullName, bytes);
                }
            }

            assetBundleData.AddCode(platform, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessPackageVersionList(string outputPackagePath, Platform platform)
        {
            Asset[] originalAssets = m_AssetBundleCollection.GetAssets();
            PackageVersionList.Asset[] assets = new PackageVersionList.Asset[originalAssets.Length];
            for (int i = 0; i < originalAssets.Length; i++)
            {
                Asset originalAsset = originalAssets[i];
                assets[i] = new PackageVersionList.Asset(originalAsset.Name, GetDependencyAssetIndexes(originalAsset.Name));
            }

            int index = 0;
            PackageVersionList.Resource[] resources = new PackageVersionList.Resource[m_AssetBundleCollection.AssetBundleCount];
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                AssetBundleCode assetBundleCode = assetBundleData.GetCode(platform);
                resources[index++] = new PackageVersionList.Resource(assetBundleData.Name, assetBundleData.Variant, GetExtension(assetBundleData), (byte)assetBundleData.LoadType, assetBundleCode.Length, assetBundleCode.HashCode, GetAssetIndexes(assetBundleData));
            }

            string[] originalResourceGroup = GetResourceGroups();
            PackageVersionList.ResourceGroup[] resourceGroups = new PackageVersionList.ResourceGroup[originalResourceGroup.Length];
            for (int i = 0; i < originalResourceGroup.Length; i++)
            {
                resourceGroups[i] = new PackageVersionList.ResourceGroup(originalResourceGroup[i], GetResourceIndexes(originalResourceGroup[i]));
            }

            PackageVersionList versionList = new PackageVersionList(ApplicableGameVersion, InternalResourceVersion, assets, resources, resourceGroups);
            PackageVersionListSerializer serializer = new PackageVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.SerializePackageVersionListCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.SerializePackageVersionListCallback_V1);
            string packageVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, VersionListFileName));
            using (FileStream fileStream = new FileStream(packageVersionListPath, FileMode.CreateNew, FileAccess.Write))
            {
                if (!serializer.Serialize(fileStream, versionList))
                {
                    throw new GameFrameworkException("Serialize package version list failure.");
                }
            }
        }

        private VersionListData ProcessUpdatableVersionList(string outputFullPath, Platform platform)
        {
            Asset[] originalAssets = m_AssetBundleCollection.GetAssets();
            UpdatableVersionList.Asset[] assets = new UpdatableVersionList.Asset[originalAssets.Length];
            for (int i = 0; i < originalAssets.Length; i++)
            {
                Asset originalAsset = originalAssets[i];
                assets[i] = new UpdatableVersionList.Asset(originalAsset.Name, GetDependencyAssetIndexes(originalAsset.Name));
            }

            int index = 0;
            UpdatableVersionList.Resource[] resources = new UpdatableVersionList.Resource[m_AssetBundleCollection.AssetBundleCount];
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                AssetBundleCode assetBundleCode = assetBundleData.GetCode(platform);
                resources[index++] = new UpdatableVersionList.Resource(assetBundleData.Name, assetBundleData.Variant, GetExtension(assetBundleData), (byte)assetBundleData.LoadType, assetBundleCode.Length, assetBundleCode.HashCode, assetBundleCode.ZipLength, assetBundleCode.ZipHashCode, GetAssetIndexes(assetBundleData));
            }

            string[] originalResourceGroup = GetResourceGroups();
            UpdatableVersionList.ResourceGroup[] resourceGroups = new UpdatableVersionList.ResourceGroup[originalResourceGroup.Length];
            for (int i = 0; i < originalResourceGroup.Length; i++)
            {
                resourceGroups[i] = new UpdatableVersionList.ResourceGroup(originalResourceGroup[i], GetResourceIndexes(originalResourceGroup[i]));
            }

            UpdatableVersionList versionList = new UpdatableVersionList(ApplicableGameVersion, InternalResourceVersion, assets, resources, resourceGroups);
            UpdatableVersionListSerializer serializer = new UpdatableVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.SerializeUpdatableVersionListCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.SerializeUpdatableVersionListCallback_V1);
            string updatableVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, VersionListFileName));
            using (FileStream fileStream = new FileStream(updatableVersionListPath, FileMode.CreateNew, FileAccess.Write))
            {
                if (!serializer.Serialize(fileStream, versionList))
                {
                    throw new GameFrameworkException("Serialize updatable version list failure.");
                }
            }

            byte[] bytes = File.ReadAllBytes(updatableVersionListPath);
            int length = bytes.Length;
            int hashCode = Utility.Verifier.GetCrc32(bytes);
            bytes = Utility.Zip.Compress(bytes);
            int zipLength = bytes.Length;
            File.WriteAllBytes(updatableVersionListPath, bytes);
            int zipHashCode = Utility.Verifier.GetCrc32(bytes);
            int dotPosition = VersionListFileName.LastIndexOf('.');
            string versionListFullNameWithCrc32 = Utility.Text.Format("{0}.{2:x8}.{1}", VersionListFileName.Substring(0, dotPosition), VersionListFileName.Substring(dotPosition + 1), hashCode);
            string updatableVersionListPathWithCrc32 = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, versionListFullNameWithCrc32));
            File.Move(updatableVersionListPath, updatableVersionListPathWithCrc32);

            return new VersionListData(updatableVersionListPathWithCrc32, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessReadOnlyVersionList(string outputPackedPath, Platform platform)
        {
            List<AssetBundleData> packedAssetBundleDatas = new List<AssetBundleData>();
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                if (!assetBundleData.Packed)
                {
                    continue;
                }

                packedAssetBundleDatas.Add(assetBundleData);
            }

            LocalVersionList.Resource[] resources = new LocalVersionList.Resource[packedAssetBundleDatas.Count];
            for (int i = 0; i < packedAssetBundleDatas.Count; i++)
            {
                AssetBundleData assetBundleData = packedAssetBundleDatas[i];
                AssetBundleCode assetBundleCode = assetBundleData.GetCode(platform);
                resources[i] = new LocalVersionList.Resource(assetBundleData.Name, assetBundleData.Variant, GetExtension(assetBundleData), (byte)assetBundleData.LoadType, assetBundleCode.Length, assetBundleCode.HashCode);
            }

            LocalVersionList versionList = new LocalVersionList(resources);
            ReadOnlyVersionListSerializer serializer = new ReadOnlyVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.SerializeLocalVersionListCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.SerializeLocalVersionListCallback_V1);
            string readOnlyVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, ResourceListFileName));
            using (FileStream fileStream = new FileStream(readOnlyVersionListPath, FileMode.CreateNew, FileAccess.Write))
            {
                if (!serializer.Serialize(fileStream, versionList))
                {
                    throw new GameFrameworkException("Serialize read only version list failure.");
                }
            }
        }

        private int[] GetDependencyAssetIndexes(string assetName)
        {
            List<int> dependencyAssetIndexes = new List<int>();
            Asset[] assets = m_AssetBundleCollection.GetAssets();
            DependencyData dependencyData = m_AssetBundleAnalyzerController.GetDependencyData(assetName);
            foreach (Asset dependencyAsset in dependencyData.GetDependencyAssets())
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    if (assets[i] == dependencyAsset)
                    {
                        dependencyAssetIndexes.Add(i);
                        break;
                    }
                }
            }

            dependencyAssetIndexes.Sort();
            return dependencyAssetIndexes.ToArray();
        }

        private int[] GetAssetIndexes(AssetBundleData assetBundleData)
        {
            Asset[] assets = m_AssetBundleCollection.GetAssets();
            string[] assetNames = assetBundleData.GetAssetNames();
            int[] assetIndexes = new int[assetNames.Length];
            for (int i = 0; i < assetNames.Length; i++)
            {
                for (int j = 0; j < assets.Length; j++)
                {
                    if (assets[j].Name == assetNames[i])
                    {
                        assetIndexes[i] = j;
                        break;
                    }
                }
            }

            return assetIndexes;
        }

        private string[] GetResourceGroups()
        {
            List<string> resourceGroups = new List<string>();
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                foreach (string resourceGroup in assetBundleData.GetResourceGroups())
                {
                    if (resourceGroups.Contains(resourceGroup))
                    {
                        continue;
                    }

                    resourceGroups.Add(resourceGroup);
                }
            }

            resourceGroups.Sort();
            return resourceGroups.ToArray();
        }

        private int[] GetResourceIndexes(string resourceGroupName)
        {
            List<int> resourceIndexes = new List<int>();
            AssetBundleData[] assetBundleDatas = m_AssetBundleDatas.Values.ToArray();
            for (int i = 0; i < assetBundleDatas.Length; i++)
            {
                foreach (string resourceGroup in assetBundleDatas[i].GetResourceGroups())
                {
                    if (resourceGroup == resourceGroupName)
                    {
                        resourceIndexes.Add(i);
                        break;
                    }
                }
            }

            return resourceIndexes.ToArray();
        }

        private BuildAssetBundleOptions GetBuildAssetBundleOptions()
        {
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

            if (UncompressedAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.UncompressedAssetBundle;
            }

            if (DisableWriteTypeTreeSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DisableWriteTypeTree;
            }

            if (DeterministicAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DeterministicAssetBundle;
            }

            if (ForceRebuildAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
            }

            if (IgnoreTypeTreeChangesSelected)
            {
                buildOptions |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
            }

            if (AppendHashToAssetBundleNameSelected)
            {
                buildOptions |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
            }

            if (ChunkBasedCompressionSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ChunkBasedCompression;
            }

            return buildOptions;
        }

        private bool GetBuildMap(out AssetBundleBuild[] assetBundleBuildMap, out BinaryBuild[] binaryBuildMap)
        {
            assetBundleBuildMap = null;
            binaryBuildMap = null;
            m_AssetBundleDatas.Clear();

            AssetBundle[] assetBundles = m_AssetBundleCollection.GetAssetBundles();
            foreach (AssetBundle assetBundle in assetBundles)
            {
                m_AssetBundleDatas.Add(assetBundle.FullName.ToLower(), new AssetBundleData(assetBundle.Name, assetBundle.Variant, assetBundle.LoadType, assetBundle.Packed, assetBundle.GetResourceGroups()));
            }

            Asset[] assets = m_AssetBundleCollection.GetAssets();
            foreach (Asset asset in assets)
            {
                string assetName = asset.Name;
                if (string.IsNullOrEmpty(assetName))
                {
                    m_BuildReport.LogError("Can not find asset by guid '{0}'.", asset.Guid);
                    return false;
                }

                string assetFileFullName = Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + assetName;
                if (!File.Exists(assetFileFullName))
                {
                    m_BuildReport.LogError("Can not find asset '{0}'.", assetFileFullName);
                    return false;
                }

                byte[] assetBytes = File.ReadAllBytes(assetFileFullName);
                int assetHashCode = Utility.Verifier.GetCrc32(assetBytes);

                List<string> dependencyAssetNames = new List<string>();
                DependencyData dependencyData = m_AssetBundleAnalyzerController.GetDependencyData(assetName);
                Asset[] dependencyAssets = dependencyData.GetDependencyAssets();
                foreach (Asset dependencyAsset in dependencyAssets)
                {
                    dependencyAssetNames.Add(dependencyAsset.Name);
                }

                dependencyAssetNames.Sort();

                m_AssetBundleDatas[asset.AssetBundle.FullName.ToLower()].AddAssetData(asset.Guid, assetName, assetBytes.Length, assetHashCode, dependencyAssetNames.ToArray());
            }

            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                if (assetBundleData.AssetCount <= 0)
                {
                    m_BuildReport.LogError("AssetBundle '{0}' has no asset.", GetAssetBundleFullName(assetBundleData.Name, assetBundleData.Variant));
                    return false;
                }
            }

            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            List<BinaryBuild> binaryBuilds = new List<BinaryBuild>();
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                if (IsLoadFromBinary(assetBundleData.LoadType))
                {
                    BinaryBuild binaryBuild = new BinaryBuild();
                    binaryBuild.resourceName = assetBundleData.Name;
                    binaryBuild.resourceVariant = assetBundleData.Variant;
                    binaryBuilds.Add(binaryBuild);
                }
                else
                {
                    AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                    assetBundleBuild.assetBundleName = assetBundleData.Name;
                    assetBundleBuild.assetBundleVariant = assetBundleData.Variant;
                    assetBundleBuild.assetNames = assetBundleData.GetAssetNames();
                    assetBundleBuilds.Add(assetBundleBuild);
                }
            }

            assetBundleBuildMap = assetBundleBuilds.ToArray();
            binaryBuildMap = binaryBuilds.ToArray();
            return true;
        }

        private string GetAssetBundleFullName(string assetBundleName, string assetBundleVariant)
        {
            return (!string.IsNullOrEmpty(assetBundleVariant) ? Utility.Text.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName);
        }

        private BuildTarget GetBuildTarget(Platform platform)
        {
            switch (platform)
            {
                case Platform.Windows:
                    return BuildTarget.StandaloneWindows;

                case Platform.Windows64:
                    return BuildTarget.StandaloneWindows64;

                case Platform.MacOS:
#if UNITY_2017_3_OR_NEWER
                    return BuildTarget.StandaloneOSX;
#else
                    return BuildTarget.StandaloneOSXUniversal;
#endif
                case Platform.Linux:
                    return BuildTarget.StandaloneLinux64;

                case Platform.IOS:
                    return BuildTarget.iOS;

                case Platform.Android:
                    return BuildTarget.Android;

                case Platform.WindowsStore:
                    return BuildTarget.WSAPlayer;

                case Platform.WebGL:
                    return BuildTarget.WebGL;

                default:
                    throw new GameFrameworkException("Platform is invalid.");
            }
        }

        private bool IsLoadFromBinary(AssetBundleLoadType loadType)
        {
            return loadType == AssetBundleLoadType.LoadFromBinary || loadType == AssetBundleLoadType.LoadFromBinaryAndQuickDecrypt || loadType == AssetBundleLoadType.LoadFromBinaryAndDecrypt;
        }

        private string GetExtension(AssetBundleData data)
        {
            if (IsLoadFromBinary(data.LoadType))
            {
                string assetName = data.GetAssetNames()[0];
                int position = assetName.LastIndexOf('.');
                if (position >= 0)
                {
                    return assetName.Substring(position + 1);
                }
            }

            return DefaultExtension;
        }

        private struct BinaryBuild
        {
            public string resourceName;
            public string resourceVariant;
        }
    }
}
