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

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private const string RemoteVersionListFileName = "GameFrameworkVersion.dat";
        private const string LocalVersionListFileName = "GameFrameworkList.dat";
        private const string DefaultExtension = "dat";
        private const string NoneOptionName = "<None>";
        private static readonly int AssetsStringLength = "Assets".Length;

        private readonly string m_ConfigurationPath;
        private readonly ResourceCollection m_ResourceCollection;
        private readonly ResourceAnalyzerController m_ResourceAnalyzerController;
        private readonly SortedDictionary<string, ResourceData> m_ResourceDatas;
        private readonly BuildReport m_BuildReport;
        private readonly List<string> m_BuildEventHandlerTypeNames;
        private IBuildEventHandler m_BuildEventHandler;

        public ResourceBuilderController()
        {
            m_ConfigurationPath = Type.GetConfigurationPath<ResourceBuilderConfigPathAttribute>() ?? Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameFramework/Configs/ResourceBuilder.xml"));

            m_ResourceCollection = new ResourceCollection();
            Utility.Zip.SetZipHelper(new DefaultZipHelper());

            m_ResourceCollection.OnLoadingResource += delegate (int index, int count)
            {
                if (OnLoadingResource != null)
                {
                    OnLoadingResource(index, count);
                }
            };

            m_ResourceCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            m_ResourceCollection.OnLoadCompleted += delegate ()
            {
                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }
            };

            m_ResourceAnalyzerController = new ResourceAnalyzerController(m_ResourceCollection);

            m_ResourceAnalyzerController.OnAnalyzingAsset += delegate (int index, int count)
            {
                if (OnAnalyzingAsset != null)
                {
                    OnAnalyzingAsset(index, count);
                }
            };

            m_ResourceAnalyzerController.OnAnalyzeCompleted += delegate ()
            {
                if (OnAnalyzeCompleted != null)
                {
                    OnAnalyzeCompleted();
                }
            };

            m_ResourceDatas = new SortedDictionary<string, ResourceData>();
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

        public string GameFrameworkVersion
        {
            get
            {
                return GameFramework.Version.GameFrameworkVersion;
            }
        }

        public string UnityVersion
        {
            get
            {
                return Application.unityVersion;
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

        public event GameFrameworkAction<int, int> OnLoadingResource = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public event GameFrameworkAction<int, int> OnAnalyzingAsset = null;

        public event GameFrameworkAction OnAnalyzeCompleted = null;

        public event GameFrameworkFunc<string, float, bool> ProcessingAssetBundle = null;

        public event GameFrameworkFunc<string, float, bool> ProcessingBinary = null;

        public event GameFrameworkAction<Platform> ProcessResourceComplete = null;

        public event GameFrameworkAction<string> BuildResourceError = null;

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
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("ResourceBuilder");
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

                XmlElement xmlBuilder = xmlDocument.CreateElement("ResourceBuilder");
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

        public bool BuildResources()
        {
            if (!IsValidOutputDirectory)
            {
                return false;
            }

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
            m_BuildReport.Initialize(BuildReportPath, ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion,
                Platforms, ZipSelected, (int)buildAssetBundleOptions, m_ResourceDatas);

            try
            {
                m_BuildReport.LogInfo("Build Start Time: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PreprocessAllPlatforms'...");
                    m_BuildEventHandler.PreprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                }

                m_BuildReport.LogInfo("Start prepare resource collection...");
                if (!m_ResourceCollection.Load())
                {
                    m_BuildReport.LogError("Can not parse 'ResourceCollection.xml', please use 'Resource Editor' tool first.");

                    if (m_BuildEventHandler != null)
                    {
                        m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
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
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                    }

                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare resource collection complete.");
                m_BuildReport.LogInfo("Start analyze assets dependency...");

                m_ResourceAnalyzerController.Analyze();

                m_BuildReport.LogInfo("Analyze assets dependency complete.");
                m_BuildReport.LogInfo("Start prepare build map...");

                AssetBundleBuild[] assetBundleBuildMap = null;
                BinaryBuild[] binaryBuildMap = null;
                if (!GetResourceBuildMap(out assetBundleBuildMap, out binaryBuildMap))
                {
                    m_BuildReport.LogError("Get resource build map failure.");

                    if (m_BuildEventHandler != null)
                    {
                        m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                        m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                    }

                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare resource build map complete.");
                m_BuildReport.LogInfo("Start build resources for selected platforms...");

                bool watchResult = m_BuildEventHandler == null || !m_BuildEventHandler.ContinueOnFailure;
                bool isSuccess = false;
                isSuccess = BuildResources(Platform.Windows, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.Windows64, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.MacOS, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.Linux, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.IOS, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.Android, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.WindowsStore, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (!watchResult || isSuccess)
                {
                    isSuccess = BuildResources(Platform.WebGL, assetBundleBuildMap, buildAssetBundleOptions, binaryBuildMap);
                }

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PostprocessAllPlatforms'...");
                    m_BuildEventHandler.PostprocessAllPlatforms(ProductName, CompanyName, GameIdentifier, GameFrameworkVersion, UnityVersion, ApplicableGameVersion, InternalResourceVersion, buildAssetBundleOptions, ZipSelected, OutputDirectory, WorkingPath, OutputPackageSelected, OutputPackagePath, OutputFullSelected, OutputFullPath, OutputPackedSelected, OutputPackedPath, BuildReportPath);
                }

                m_BuildReport.LogInfo("Build resources for selected platforms complete.");
                m_BuildReport.SaveReport();
                return true;
            }
            catch (Exception exception)
            {
                string errorMessage = exception.ToString();
                m_BuildReport.LogFatal(errorMessage);
                m_BuildReport.SaveReport();
                if (BuildResourceError != null)
                {
                    BuildResourceError(errorMessage);
                }

                return false;
            }
        }

        private bool BuildResources(Platform platform, AssetBundleBuild[] assetBundleBuildMap, BuildAssetBundleOptions buildAssetBundleOptions, BinaryBuild[] binaryBuildMap)
        {
            if (!IsPlatformSelected(platform))
            {
                return true;
            }

            string platformName = platform.ToString();
            m_BuildReport.LogInfo("Start build resources for '{0}'...", platformName);

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
                validNames.Add(GetResourceFullName(i.assetBundleName, i.assetBundleVariant).ToLower());
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
            m_BuildReport.LogInfo("Unity start build asset bundles for '{0}'...", platformName);
            AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(workingPath, assetBundleBuildMap, buildAssetBundleOptions, GetBuildTarget(platform));
            if (assetBundleManifest == null)
            {
                m_BuildReport.LogError("Build asset bundles for '{0}' failure.", platformName);

                if (m_BuildEventHandler != null)
                {
                    m_BuildReport.LogInfo("Execute build event handler 'PostprocessPlatform' for '{0}'...", platformName);
                    m_BuildEventHandler.PostprocessPlatform(platform, workingPath, OutputPackageSelected, outputPackagePath, OutputFullSelected, outputFullPath, OutputPackedSelected, outputPackedPath, false);
                }

                return false;
            }

            m_BuildReport.LogInfo("Unity build asset bundles for '{0}' complete.", platformName);

            // Process AssetBundles
            for (int i = 0; i < assetBundleBuildMap.Length; i++)
            {
                string fullName = GetResourceFullName(assetBundleBuildMap[i].assetBundleName, assetBundleBuildMap[i].assetBundleVariant);
                if (ProcessingAssetBundle != null)
                {
                    if (ProcessingAssetBundle(fullName, (float)(i + 1) / assetBundleBuildMap.Length))
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

                m_BuildReport.LogInfo("Start process asset bundle '{0}' for '{1}'...", fullName, platformName);

                ProcessAssetBundle(platform, workingPath, outputPackagePath, outputFullPath, outputPackedPath, ZipSelected, assetBundleBuildMap[i].assetBundleName, assetBundleBuildMap[i].assetBundleVariant);

                m_BuildReport.LogInfo("Process asset bundle '{0}' for '{1}' complete.", fullName, platformName);
            }

            // Process Binaries
            for (int i = 0; i < binaryBuildMap.Length; i++)
            {
                string fullName = GetResourceFullName(binaryBuildMap[i].resourceName, binaryBuildMap[i].resourceVariant);
                if (ProcessingBinary != null)
                {
                    if (ProcessingBinary(fullName, (float)(i + 1) / binaryBuildMap.Length))
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

                m_BuildReport.LogInfo("Start process binary '{0}' for '{1}'...", fullName, platformName);

                ProcessBinary(platform, workingPath, outputPackagePath, outputFullPath, outputPackedPath, ZipSelected, binaryBuildMap[i].resourceName, binaryBuildMap[i].resourceVariant);

                m_BuildReport.LogInfo("Process binary '{0}' for '{1}' complete.", fullName, platformName);
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

            if (ProcessResourceComplete != null)
            {
                ProcessResourceComplete(platform);
            }

            m_BuildReport.LogInfo("Build resources for '{0}' success.", platformName);
            return true;
        }

        private void ProcessAssetBundle(Platform platform, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, bool zip, string name, string variant)
        {
            string fullName = GetResourceFullName(name, variant);
            ResourceData resourceData = m_ResourceDatas[fullName];
            string fullNameWithExtension = Utility.Text.Format("{0}.{1}", fullName, GetExtension(resourceData));
            string workingName = Utility.Path.GetRegularPath(Path.Combine(workingPath, fullName.ToLower()));

            byte[] bytes = File.ReadAllBytes(workingName);
            int length = bytes.Length;
            int hashCode = Utility.Verifier.GetCrc32(bytes);
            int zipLength = length;
            int zipHashCode = hashCode;

            byte[] hashBytes = Utility.Converter.GetBytes(hashCode);
            if (resourceData.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt)
            {
                bytes = Utility.Encryption.GetQuickXorBytes(bytes, hashBytes);
            }
            else if (resourceData.LoadType == LoadType.LoadFromMemoryAndDecrypt)
            {
                bytes = Utility.Encryption.GetXorBytes(bytes, hashBytes);
            }

            // Package AssetBundle
            if (OutputPackageSelected)
            {
                string packagePath = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, fullNameWithExtension));
                string packageDirectoryName = Path.GetDirectoryName(packagePath);
                if (!Directory.Exists(packageDirectoryName))
                {
                    Directory.CreateDirectory(packageDirectoryName);
                }

                File.WriteAllBytes(packagePath, bytes);
            }

            // Packed AssetBundle
            if (OutputPackedSelected && resourceData.Packed)
            {
                string packedPath = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, fullNameWithExtension));
                string packedDirectoryName = Path.GetDirectoryName(packedPath);
                if (!Directory.Exists(packedDirectoryName))
                {
                    Directory.CreateDirectory(packedDirectoryName);
                }

                File.WriteAllBytes(packedPath, bytes);
            }

            // Full AssetBundle
            if (OutputFullSelected)
            {
                string fullNameWithCrc32 = variant != null ? Utility.Text.Format("{0}.{1}.{2:x8}.{3}", name, variant, hashCode, DefaultExtension) : Utility.Text.Format("{0}.{1:x8}.{2}", name, hashCode, DefaultExtension);
                string fullPath = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, fullNameWithCrc32));
                string fullDirectoryName = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(fullDirectoryName))
                {
                    Directory.CreateDirectory(fullDirectoryName);
                }

                if (zip)
                {
                    byte[] zipBytes = Utility.Zip.Compress(bytes);
                    zipLength = zipBytes.Length;
                    zipHashCode = Utility.Verifier.GetCrc32(zipBytes);
                    File.WriteAllBytes(fullPath, zipBytes);
                }
                else
                {
                    File.WriteAllBytes(fullPath, bytes);
                }
            }

            resourceData.AddCode(platform, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessBinary(Platform platform, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, bool zip, string name, string variant)
        {
            string fullName = GetResourceFullName(name, variant);
            ResourceData resourceData = m_ResourceDatas[fullName];
            string fullNameWithExtension = Utility.Text.Format("{0}.{1}", fullName, GetExtension(resourceData));
            string assetName = resourceData.GetAssetNames()[0];
            string assetPath = Utility.Path.GetRegularPath(Application.dataPath.Substring(0, Application.dataPath.Length - AssetsStringLength) + assetName);

            byte[] bytes = File.ReadAllBytes(assetPath);
            int length = bytes.Length;
            int hashCode = Utility.Verifier.GetCrc32(bytes);
            int zipLength = length;
            int zipHashCode = hashCode;

            byte[] hashBytes = Utility.Converter.GetBytes(hashCode);
            if (resourceData.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
            {
                bytes = Utility.Encryption.GetQuickXorBytes(bytes, hashBytes);
            }
            else if (resourceData.LoadType == LoadType.LoadFromBinaryAndDecrypt)
            {
                bytes = Utility.Encryption.GetXorBytes(bytes, hashBytes);
            }

            // Package Binary
            if (OutputPackageSelected)
            {
                string packagePath = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, fullNameWithExtension));
                string packageDirectoryName = Path.GetDirectoryName(packagePath);
                if (!Directory.Exists(packageDirectoryName))
                {
                    Directory.CreateDirectory(packageDirectoryName);
                }

                File.WriteAllBytes(packagePath, bytes);
            }

            // Packed Binary
            if (OutputPackedSelected && resourceData.Packed)
            {
                string packedPath = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, fullNameWithExtension));
                string packedDirectoryName = Path.GetDirectoryName(packedPath);
                if (!Directory.Exists(packedDirectoryName))
                {
                    Directory.CreateDirectory(packedDirectoryName);
                }

                File.WriteAllBytes(packedPath, bytes);
            }

            // Full Binary
            if (OutputFullSelected)
            {
                string fullNameWithCrc32 = variant != null ? Utility.Text.Format("{0}.{1}.{2:x8}.{3}", name, variant, hashCode, DefaultExtension) : Utility.Text.Format("{0}.{1:x8}.{2}", name, hashCode, DefaultExtension);
                string fullPath = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, fullNameWithCrc32));
                string fullDirectoryName = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(fullDirectoryName))
                {
                    Directory.CreateDirectory(fullDirectoryName);
                }

                if (zip)
                {
                    byte[] zipBytes = Utility.Zip.Compress(bytes);
                    zipLength = zipBytes.Length;
                    zipHashCode = Utility.Verifier.GetCrc32(zipBytes);
                    File.WriteAllBytes(fullPath, zipBytes);
                }
                else
                {
                    File.WriteAllBytes(fullPath, bytes);
                }
            }

            resourceData.AddCode(platform, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessPackageVersionList(string outputPackagePath, Platform platform)
        {
            Asset[] originalAssets = m_ResourceCollection.GetAssets();
            PackageVersionList.Asset[] assets = new PackageVersionList.Asset[originalAssets.Length];
            for (int i = 0; i < originalAssets.Length; i++)
            {
                Asset originalAsset = originalAssets[i];
                assets[i] = new PackageVersionList.Asset(originalAsset.Name, GetDependencyAssetIndexes(originalAsset.Name));
            }

            int index = 0;
            PackageVersionList.Resource[] resources = new PackageVersionList.Resource[m_ResourceCollection.ResourceCount];
            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                ResourceCode resourceCode = resourceData.GetCode(platform);
                resources[index++] = new PackageVersionList.Resource(resourceData.Name, resourceData.Variant, GetExtension(resourceData), (byte)resourceData.LoadType, resourceCode.Length, resourceCode.HashCode, GetAssetIndexes(resourceData));
            }

            string[] originalResourceGroup = GetResourceGroups();
            PackageVersionList.ResourceGroup[] resourceGroups = new PackageVersionList.ResourceGroup[originalResourceGroup.Length];
            for (int i = 0; i < originalResourceGroup.Length; i++)
            {
                resourceGroups[i] = new PackageVersionList.ResourceGroup(originalResourceGroup[i], GetResourceIndexes(originalResourceGroup[i]));
            }

            PackageVersionList versionList = new PackageVersionList(ApplicableGameVersion, InternalResourceVersion, assets, resources, resourceGroups);
            PackageVersionListSerializer serializer = new PackageVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.PackageVersionListSerializeCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.PackageVersionListSerializeCallback_V1);
            string packageVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputPackagePath, RemoteVersionListFileName));
            using (FileStream fileStream = new FileStream(packageVersionListPath, FileMode.Create, FileAccess.Write))
            {
                if (!serializer.Serialize(fileStream, versionList))
                {
                    throw new GameFrameworkException("Serialize package version list failure.");
                }
            }
        }

        private VersionListData ProcessUpdatableVersionList(string outputFullPath, Platform platform)
        {
            Asset[] originalAssets = m_ResourceCollection.GetAssets();
            UpdatableVersionList.Asset[] assets = new UpdatableVersionList.Asset[originalAssets.Length];
            for (int i = 0; i < originalAssets.Length; i++)
            {
                Asset originalAsset = originalAssets[i];
                assets[i] = new UpdatableVersionList.Asset(originalAsset.Name, GetDependencyAssetIndexes(originalAsset.Name));
            }

            int index = 0;
            UpdatableVersionList.Resource[] resources = new UpdatableVersionList.Resource[m_ResourceCollection.ResourceCount];
            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                ResourceCode resourceCode = resourceData.GetCode(platform);
                resources[index++] = new UpdatableVersionList.Resource(resourceData.Name, resourceData.Variant, GetExtension(resourceData), (byte)resourceData.LoadType, resourceCode.Length, resourceCode.HashCode, resourceCode.ZipLength, resourceCode.ZipHashCode, GetAssetIndexes(resourceData));
            }

            string[] originalResourceGroup = GetResourceGroups();
            UpdatableVersionList.ResourceGroup[] resourceGroups = new UpdatableVersionList.ResourceGroup[originalResourceGroup.Length];
            for (int i = 0; i < originalResourceGroup.Length; i++)
            {
                resourceGroups[i] = new UpdatableVersionList.ResourceGroup(originalResourceGroup[i], GetResourceIndexes(originalResourceGroup[i]));
            }

            UpdatableVersionList versionList = new UpdatableVersionList(ApplicableGameVersion, InternalResourceVersion, assets, resources, resourceGroups);
            UpdatableVersionListSerializer serializer = new UpdatableVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.UpdatableVersionListSerializeCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.UpdatableVersionListSerializeCallback_V1);
            string updatableVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, RemoteVersionListFileName));
            using (FileStream fileStream = new FileStream(updatableVersionListPath, FileMode.Create, FileAccess.Write))
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
            int dotPosition = RemoteVersionListFileName.LastIndexOf('.');
            string versionListFullNameWithCrc32 = Utility.Text.Format("{0}.{2:x8}.{1}", RemoteVersionListFileName.Substring(0, dotPosition), RemoteVersionListFileName.Substring(dotPosition + 1), hashCode);
            string updatableVersionListPathWithCrc32 = Utility.Path.GetRegularPath(Path.Combine(outputFullPath, versionListFullNameWithCrc32));
            File.Move(updatableVersionListPath, updatableVersionListPathWithCrc32);

            return new VersionListData(updatableVersionListPathWithCrc32, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessReadOnlyVersionList(string outputPackedPath, Platform platform)
        {
            List<ResourceData> packedResourceDatas = new List<ResourceData>();
            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                if (!resourceData.Packed)
                {
                    continue;
                }

                packedResourceDatas.Add(resourceData);
            }

            LocalVersionList.Resource[] resources = new LocalVersionList.Resource[packedResourceDatas.Count];
            for (int i = 0; i < packedResourceDatas.Count; i++)
            {
                ResourceData resourceData = packedResourceDatas[i];
                ResourceCode resourceCode = resourceData.GetCode(platform);
                resources[i] = new LocalVersionList.Resource(resourceData.Name, resourceData.Variant, GetExtension(resourceData), (byte)resourceData.LoadType, resourceCode.Length, resourceCode.HashCode);
            }

            LocalVersionList versionList = new LocalVersionList(resources);
            ReadOnlyVersionListSerializer serializer = new ReadOnlyVersionListSerializer();
            serializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.LocalVersionListSerializeCallback_V0);
            serializer.RegisterSerializeCallback(1, BuiltinVersionListSerializer.LocalVersionListSerializeCallback_V1);
            string readOnlyVersionListPath = Utility.Path.GetRegularPath(Path.Combine(outputPackedPath, LocalVersionListFileName));
            using (FileStream fileStream = new FileStream(readOnlyVersionListPath, FileMode.Create, FileAccess.Write))
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
            Asset[] assets = m_ResourceCollection.GetAssets();
            DependencyData dependencyData = m_ResourceAnalyzerController.GetDependencyData(assetName);
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

        private int[] GetAssetIndexes(ResourceData resourceData)
        {
            Asset[] assets = m_ResourceCollection.GetAssets();
            string[] assetGuids = resourceData.GetAssetGuids();
            int[] assetIndexes = new int[assetGuids.Length];
            for (int i = 0; i < assetGuids.Length; i++)
            {
                assetIndexes[i] = Array.BinarySearch(assets, m_ResourceCollection.GetAsset(assetGuids[i]));
                if (assetIndexes[i] < 0)
                {
                    throw new GameFrameworkException("Asset is invalid.");
                }
            }

            return assetIndexes;
        }

        private string[] GetResourceGroups()
        {
            List<string> resourceGroups = new List<string>();
            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                foreach (string resourceGroup in resourceData.GetResourceGroups())
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
            ResourceData[] resourceDatas = m_ResourceDatas.Values.ToArray();
            for (int i = 0; i < resourceDatas.Length; i++)
            {
                foreach (string resourceGroup in resourceDatas[i].GetResourceGroups())
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

        private bool GetResourceBuildMap(out AssetBundleBuild[] assetBundleBuildMap, out BinaryBuild[] binaryBuildMap)
        {
            assetBundleBuildMap = null;
            binaryBuildMap = null;
            m_ResourceDatas.Clear();

            Resource[] resources = m_ResourceCollection.GetResources();
            foreach (Resource resource in resources)
            {
                m_ResourceDatas.Add(resource.FullName, new ResourceData(resource.Name, resource.Variant, resource.LoadType, resource.Packed, resource.GetResourceGroups()));
            }

            Asset[] assets = m_ResourceCollection.GetAssets();
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
                DependencyData dependencyData = m_ResourceAnalyzerController.GetDependencyData(assetName);
                Asset[] dependencyAssets = dependencyData.GetDependencyAssets();
                foreach (Asset dependencyAsset in dependencyAssets)
                {
                    dependencyAssetNames.Add(dependencyAsset.Name);
                }

                dependencyAssetNames.Sort();

                m_ResourceDatas[asset.Resource.FullName].AddAssetData(asset.Guid, assetName, assetBytes.Length, assetHashCode, dependencyAssetNames.ToArray());
            }

            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                if (resourceData.AssetCount <= 0)
                {
                    m_BuildReport.LogError("Resource '{0}' has no asset.", GetResourceFullName(resourceData.Name, resourceData.Variant));
                    return false;
                }
            }

            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            List<BinaryBuild> binaryBuilds = new List<BinaryBuild>();
            foreach (ResourceData resourceData in m_ResourceDatas.Values)
            {
                if (IsLoadFromBinary(resourceData.LoadType))
                {
                    BinaryBuild build = new BinaryBuild();
                    build.resourceName = resourceData.Name;
                    build.resourceVariant = resourceData.Variant;
                    binaryBuilds.Add(build);
                }
                else
                {
                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = resourceData.Name;
                    build.assetBundleVariant = resourceData.Variant;
                    build.assetNames = resourceData.GetAssetNames();
                    assetBundleBuilds.Add(build);
                }
            }

            assetBundleBuildMap = assetBundleBuilds.ToArray();
            binaryBuildMap = binaryBuilds.ToArray();
            return true;
        }

        private string GetResourceFullName(string name, string variant)
        {
            return !string.IsNullOrEmpty(variant) ? Utility.Text.Format("{0}.{1}", name, variant) : name;
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

        private bool IsLoadFromBinary(LoadType loadType)
        {
            return loadType == LoadType.LoadFromBinary || loadType == LoadType.LoadFromBinaryAndQuickDecrypt || loadType == LoadType.LoadFromBinaryAndDecrypt;
        }

        private string GetExtension(ResourceData data)
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
    }
}
