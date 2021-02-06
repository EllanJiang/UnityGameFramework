//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private sealed class BuildReport
        {
            private const string BuildReportName = "BuildReport.xml";
            private const string BuildLogName = "BuildLog.txt";

            private string m_BuildReportName = null;
            private string m_BuildLogName = null;
            private string m_ProductName = null;
            private string m_CompanyName = null;
            private string m_GameIdentifier = null;
            private string m_GameFrameworkVersion = null;
            private string m_UnityVersion = null;
            private string m_ApplicableGameVersion = null;
            private int m_InternalResourceVersion = 0;
            private Platform m_Platforms = Platform.Undefined;
            private AssetBundleCompressionType m_AssetBundleCompression;
            private string m_CompressionHelperTypeName;
            private bool m_AdditionalCompressionSelected = false;
            private bool m_ForceRebuildAssetBundleSelected = false;
            private string m_BuildEventHandlerTypeName;
            private string m_OutputDirectory;
            private BuildAssetBundleOptions m_BuildAssetBundleOptions = BuildAssetBundleOptions.None;
            private StringBuilder m_LogBuilder = null;
            private SortedDictionary<string, ResourceData> m_ResourceDatas = null;

            public void Initialize(string buildReportPath, string productName, string companyName, string gameIdentifier, string gameFrameworkVersion, string unityVersion, string applicableGameVersion, int internalResourceVersion,
                Platform platforms, AssetBundleCompressionType assetBundleCompression, string compressionHelperTypeName, bool additionalCompressionSelected, bool forceRebuildAssetBundleSelected, string buildEventHandlerTypeName, string outputDirectory, BuildAssetBundleOptions buildAssetBundleOptions, SortedDictionary<string, ResourceData> resourceDatas)
            {
                if (string.IsNullOrEmpty(buildReportPath))
                {
                    throw new GameFrameworkException("Build report path is invalid.");
                }

                m_BuildReportName = Utility.Path.GetRegularPath(Path.Combine(buildReportPath, BuildReportName));
                m_BuildLogName = Utility.Path.GetRegularPath(Path.Combine(buildReportPath, BuildLogName));
                m_ProductName = productName;
                m_CompanyName = companyName;
                m_GameIdentifier = gameIdentifier;
                m_GameFrameworkVersion = gameFrameworkVersion;
                m_UnityVersion = unityVersion;
                m_ApplicableGameVersion = applicableGameVersion;
                m_InternalResourceVersion = internalResourceVersion;
                m_Platforms = platforms;
                m_AssetBundleCompression = assetBundleCompression;
                m_CompressionHelperTypeName = compressionHelperTypeName;
                m_AdditionalCompressionSelected = additionalCompressionSelected;
                m_ForceRebuildAssetBundleSelected = forceRebuildAssetBundleSelected;
                m_BuildEventHandlerTypeName = buildEventHandlerTypeName;
                m_OutputDirectory = outputDirectory;
                m_BuildAssetBundleOptions = buildAssetBundleOptions;
                m_LogBuilder = new StringBuilder();
                m_ResourceDatas = resourceDatas;
            }

            public void LogInfo(string format, params object[] args)
            {
                LogInternal("INFO", format, args);
            }

            public void LogWarning(string format, params object[] args)
            {
                LogInternal("WARNING", format, args);
            }

            public void LogError(string format, params object[] args)
            {
                LogInternal("ERROR", format, args);
            }

            public void LogFatal(string format, params object[] args)
            {
                LogInternal("FATAL", format, args);
            }

            public void SaveReport()
            {
                XmlElement xmlElement = null;
                XmlAttribute xmlAttribute = null;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement xmlRoot = xmlDocument.CreateElement("UnityGameFramework");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlBuildReport = xmlDocument.CreateElement("BuildReport");
                xmlRoot.AppendChild(xmlBuildReport);

                XmlElement xmlSummary = xmlDocument.CreateElement("Summary");
                xmlBuildReport.AppendChild(xmlSummary);

                xmlElement = xmlDocument.CreateElement("ProductName");
                xmlElement.InnerText = m_ProductName;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("CompanyName");
                xmlElement.InnerText = m_CompanyName;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("GameIdentifier");
                xmlElement.InnerText = m_GameIdentifier;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("GameFrameworkVersion");
                xmlElement.InnerText = m_GameFrameworkVersion;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("UnityVersion");
                xmlElement.InnerText = m_UnityVersion;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ApplicableGameVersion");
                xmlElement.InnerText = m_ApplicableGameVersion;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("InternalResourceVersion");
                xmlElement.InnerText = m_InternalResourceVersion.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("Platforms");
                xmlElement.InnerText = m_Platforms.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AssetBundleCompression");
                xmlElement.InnerText = m_AssetBundleCompression.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("CompressionHelperTypeName");
                xmlElement.InnerText = m_CompressionHelperTypeName;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AdditionalCompressionSelected");
                xmlElement.InnerText = m_AdditionalCompressionSelected.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ForceRebuildAssetBundleSelected");
                xmlElement.InnerText = m_ForceRebuildAssetBundleSelected.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("BuildEventHandlerTypeName");
                xmlElement.InnerText = m_BuildEventHandlerTypeName;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputDirectory");
                xmlElement.InnerText = m_OutputDirectory;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("BuildAssetBundleOptions");
                xmlElement.InnerText = m_BuildAssetBundleOptions.ToString();
                xmlSummary.AppendChild(xmlElement);

                XmlElement xmlResources = xmlDocument.CreateElement("Resources");
                xmlAttribute = xmlDocument.CreateAttribute("Count");
                xmlAttribute.Value = m_ResourceDatas.Count.ToString();
                xmlResources.Attributes.SetNamedItem(xmlAttribute);
                xmlBuildReport.AppendChild(xmlResources);
                foreach (ResourceData resourceData in m_ResourceDatas.Values)
                {
                    XmlElement xmlResource = xmlDocument.CreateElement("Resource");
                    xmlAttribute = xmlDocument.CreateAttribute("Name");
                    xmlAttribute.Value = resourceData.Name;
                    xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    if (resourceData.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("Variant");
                        xmlAttribute.Value = resourceData.Variant;
                        xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("Extension");
                    xmlAttribute.Value = GetExtension(resourceData);
                    xmlResource.Attributes.SetNamedItem(xmlAttribute);

                    if (resourceData.FileSystem != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("FileSystem");
                        xmlAttribute.Value = resourceData.FileSystem;
                        xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("LoadType");
                    xmlAttribute.Value = ((byte)resourceData.LoadType).ToString();
                    xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("Packed");
                    xmlAttribute.Value = resourceData.Packed.ToString();
                    xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    string[] resourceGroups = resourceData.GetResourceGroups();
                    if (resourceGroups.Length > 0)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("ResourceGroups");
                        xmlAttribute.Value = string.Join(",", resourceGroups);
                        xmlResource.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlResources.AppendChild(xmlResource);

                    AssetData[] assetDatas = resourceData.GetAssetDatas();
                    XmlElement xmlAssets = xmlDocument.CreateElement("Assets");
                    xmlAttribute = xmlDocument.CreateAttribute("Count");
                    xmlAttribute.Value = assetDatas.Length.ToString();
                    xmlAssets.Attributes.SetNamedItem(xmlAttribute);
                    xmlResource.AppendChild(xmlAssets);
                    foreach (AssetData assetData in assetDatas)
                    {
                        XmlElement xmlAsset = xmlDocument.CreateElement("Asset");
                        xmlAttribute = xmlDocument.CreateAttribute("Guid");
                        xmlAttribute.Value = assetData.Guid;
                        xmlAsset.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("Name");
                        xmlAttribute.Value = assetData.Name;
                        xmlAsset.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("Length");
                        xmlAttribute.Value = assetData.Length.ToString();
                        xmlAsset.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("HashCode");
                        xmlAttribute.Value = assetData.HashCode.ToString();
                        xmlAsset.Attributes.SetNamedItem(xmlAttribute);
                        xmlAssets.AppendChild(xmlAsset);
                        string[] dependencyAssetNames = assetData.GetDependencyAssetNames();
                        if (dependencyAssetNames.Length > 0)
                        {
                            XmlElement xmlDependencyAssets = xmlDocument.CreateElement("DependencyAssets");
                            xmlAttribute = xmlDocument.CreateAttribute("Count");
                            xmlAttribute.Value = dependencyAssetNames.Length.ToString();
                            xmlDependencyAssets.Attributes.SetNamedItem(xmlAttribute);
                            xmlAsset.AppendChild(xmlDependencyAssets);
                            foreach (string dependencyAssetName in dependencyAssetNames)
                            {
                                XmlElement xmlDependencyAsset = xmlDocument.CreateElement("DependencyAsset");
                                xmlAttribute = xmlDocument.CreateAttribute("Name");
                                xmlAttribute.Value = dependencyAssetName;
                                xmlDependencyAsset.Attributes.SetNamedItem(xmlAttribute);
                                xmlDependencyAssets.AppendChild(xmlDependencyAsset);
                            }
                        }
                    }

                    XmlElement xmlCodes = xmlDocument.CreateElement("Codes");
                    xmlResource.AppendChild(xmlCodes);
                    foreach (ResourceCode resourceCode in resourceData.GetCodes())
                    {
                        XmlElement xmlCode = xmlDocument.CreateElement(resourceCode.Platform.ToString());
                        xmlAttribute = xmlDocument.CreateAttribute("Length");
                        xmlAttribute.Value = resourceCode.Length.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("HashCode");
                        xmlAttribute.Value = resourceCode.HashCode.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("CompressedLength");
                        xmlAttribute.Value = resourceCode.CompressedLength.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("CompressedHashCode");
                        xmlAttribute.Value = resourceCode.CompressedHashCode.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlCodes.AppendChild(xmlCode);
                    }
                }

                xmlDocument.Save(m_BuildReportName);
                File.WriteAllText(m_BuildLogName, m_LogBuilder.ToString());
            }

            private void LogInternal(string type, string format, object[] args)
            {
                m_LogBuilder.Append("[");
                m_LogBuilder.Append(DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss.fff"));
                m_LogBuilder.Append("][");
                m_LogBuilder.Append(type);
                m_LogBuilder.Append("] ");
                m_LogBuilder.AppendFormat(format, args);
                m_LogBuilder.AppendLine();
            }
        }
    }
}
