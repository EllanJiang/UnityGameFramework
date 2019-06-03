//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    public sealed partial class AssetBundleBuilderController
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
            private string m_ApplicableGameVersion = null;
            private int m_InternalResourceVersion = 0;
            private string m_UnityVersion = null;
            private Platform m_Platforms = Platform.Undefined;
            private bool m_ZipSelected = false;
            private int m_BuildAssetBundleOptions = 0;
            private StringBuilder m_LogBuilder = null;
            private SortedDictionary<string, AssetBundleData> m_AssetBundleDatas = null;

            public void Initialize(string buildReportPath, string productName, string companyName, string gameIdentifier, string applicableGameVersion, int internalResourceVersion, string unityVersion,
                Platform platforms, bool zipSelected, int buildAssetBundleOptions, SortedDictionary<string, AssetBundleData> assetBundleDatas)
            {
                if (string.IsNullOrEmpty(buildReportPath))
                {
                    throw new GameFrameworkException("Build report path is invalid.");
                }

                m_BuildReportName = Utility.Path.GetCombinePath(buildReportPath, BuildReportName);
                m_BuildLogName = Utility.Path.GetCombinePath(buildReportPath, BuildLogName);
                m_ProductName = productName;
                m_CompanyName = companyName;
                m_GameIdentifier = gameIdentifier;
                m_ApplicableGameVersion = applicableGameVersion;
                m_UnityVersion = unityVersion;
                m_InternalResourceVersion = internalResourceVersion;
                m_Platforms = platforms;
                m_ZipSelected = zipSelected;
                m_BuildAssetBundleOptions = buildAssetBundleOptions;
                m_LogBuilder = new StringBuilder();
                m_AssetBundleDatas = assetBundleDatas;
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
                xmlElement = xmlDocument.CreateElement("ApplicableGameVersion");
                xmlElement.InnerText = m_ApplicableGameVersion;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("InternalResourceVersion");
                xmlElement.InnerText = m_InternalResourceVersion.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("UnityVersion");
                xmlElement.InnerText = m_UnityVersion;
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("Platforms");
                xmlElement.InnerText = m_Platforms.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ZipSelected");
                xmlElement.InnerText = m_ZipSelected.ToString();
                xmlSummary.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("BuildAssetBundleOptions");
                xmlElement.InnerText = m_BuildAssetBundleOptions.ToString();
                xmlSummary.AppendChild(xmlElement);

                XmlElement xmlAssetBundles = xmlDocument.CreateElement("AssetBundles");
                xmlAttribute = xmlDocument.CreateAttribute("Count");
                xmlAttribute.Value = m_AssetBundleDatas.Count.ToString();
                xmlAssetBundles.Attributes.SetNamedItem(xmlAttribute);
                xmlBuildReport.AppendChild(xmlAssetBundles);
                foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
                {
                    XmlElement xmlAssetBundle = xmlDocument.CreateElement("AssetBundle");
                    xmlAttribute = xmlDocument.CreateAttribute("Name");
                    xmlAttribute.Value = assetBundleData.Name;
                    xmlAssetBundle.Attributes.SetNamedItem(xmlAttribute);
                    if (assetBundleData.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("Variant");
                        xmlAttribute.Value = assetBundleData.Variant;
                        xmlAssetBundle.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlAttribute = xmlDocument.CreateAttribute("LoadType");
                    xmlAttribute.Value = ((int)assetBundleData.LoadType).ToString();
                    xmlAssetBundle.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("Packed");
                    xmlAttribute.Value = assetBundleData.Packed.ToString();
                    xmlAssetBundle.Attributes.SetNamedItem(xmlAttribute);
                    string[] resourceGroups = assetBundleData.GetResourceGroups();
                    if (resourceGroups.Length > 0)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("ResourceGroups");
                        xmlAttribute.Value = string.Join(",", resourceGroups);
                        xmlAssetBundle.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlAssetBundles.AppendChild(xmlAssetBundle);

                    AssetData[] assetDatas = assetBundleData.GetAssetDatas();
                    XmlElement xmlAssets = xmlDocument.CreateElement("Assets");
                    xmlAttribute = xmlDocument.CreateAttribute("Count");
                    xmlAttribute.Value = assetDatas.Length.ToString();
                    xmlAssets.Attributes.SetNamedItem(xmlAttribute);
                    xmlAssetBundle.AppendChild(xmlAssets);
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
                    xmlAssetBundle.AppendChild(xmlCodes);
                    foreach (AssetBundleCode assetBundleCode in assetBundleData.GetCodes())
                    {
                        XmlElement xmlCode = xmlDocument.CreateElement(assetBundleCode.Platform.ToString());
                        xmlAttribute = xmlDocument.CreateAttribute("Length");
                        xmlAttribute.Value = assetBundleCode.Length.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("HashCode");
                        xmlAttribute.Value = assetBundleCode.HashCode.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("ZipLength");
                        xmlAttribute.Value = assetBundleCode.ZipLength.ToString();
                        xmlCode.Attributes.SetNamedItem(xmlAttribute);
                        xmlAttribute = xmlDocument.CreateAttribute("ZipHashCode");
                        xmlAttribute.Value = assetBundleCode.ZipHashCode.ToString();
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
                m_LogBuilder.Append(DateTime.Now.ToString("HH:mm:ss.fff"));
                m_LogBuilder.Append("][");
                m_LogBuilder.Append(type);
                m_LogBuilder.Append("] ");
                m_LogBuilder.AppendFormat(format, args);
                m_LogBuilder.AppendLine();
            }
        }
    }
}
