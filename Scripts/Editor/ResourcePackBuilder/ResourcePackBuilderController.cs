//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed class ResourcePackBuilderController
    {
        private const string DefaultResourcePackName = "GameFrameworkResourcePack";
        private const string DefaultExtension = "dat";
        private const string NoneOptionName = "<None>";
        private static readonly string[] EmptyStringArray = new string[0];
        private static readonly UpdatableVersionList.Resource[] EmptyResourceArray = new UpdatableVersionList.Resource[0];

        private readonly string m_ConfigurationPath;
        private readonly List<string> m_CompressionHelperTypeNames;
        private readonly UpdatableVersionListSerializer m_UpdatableVersionListSerializer;
        private readonly ResourcePackVersionListSerializer m_ResourcePackVersionListSerializer;

        public ResourcePackBuilderController()
        {
            m_ConfigurationPath = Type.GetConfigurationPath<ResourceBuilderConfigPathAttribute>() ?? Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameFramework/Configs/ResourceBuilder.xml"));

            m_UpdatableVersionListSerializer = new UpdatableVersionListSerializer();
            m_UpdatableVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V0);
            m_UpdatableVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V1);
            m_UpdatableVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V2);

            m_ResourcePackVersionListSerializer = new ResourcePackVersionListSerializer();
            m_ResourcePackVersionListSerializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.ResourcePackVersionListSerializeCallback_V0);

            m_CompressionHelperTypeNames = new List<string>
            {
                NoneOptionName
            };

            m_CompressionHelperTypeNames.AddRange(Type.GetRuntimeOrEditorTypeNames(typeof(Utility.Compression.ICompressionHelper)));

            Platform = Platform.Windows;
            CompressionHelperTypeName = string.Empty;
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

        public string WorkingDirectory
        {
            get;
            set;
        }

        public Platform Platform
        {
            get;
            set;
        }

        public string CompressionHelperTypeName
        {
            get;
            set;
        }

        public bool BackupDiff
        {
            get;
            set;
        }

        public bool BackupVersion
        {
            get;
            set;
        }

        public int LengthLimit
        {
            get;
            set;
        }

        public bool IsValidWorkingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(WorkingDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(WorkingDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string SourcePath
        {
            get
            {
                if (!IsValidWorkingDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Full/", WorkingDirectory)).FullName);
            }
        }

        public string SourcePathForDisplay
        {
            get
            {
                if (!IsValidWorkingDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/Full/*/{1}/", WorkingDirectory, Platform.ToString())).FullName);
            }
        }

        public string OutputPath
        {
            get
            {
                if (!IsValidWorkingDirectory)
                {
                    return string.Empty;
                }

                return Utility.Path.GetRegularPath(new DirectoryInfo(Utility.Text.Format("{0}/ResourcePack/{1}/", WorkingDirectory, Platform.ToString())).FullName);
            }
        }

        public event GameFrameworkAction<int> OnBuildResourcePacksStarted = null;

        public event GameFrameworkAction<int, int> OnBuildResourcePacksCompleted = null;

        public event GameFrameworkAction<int, int, string, string> OnBuildResourcePackSuccess = null;

        public event GameFrameworkAction<int, int, string, string> OnBuildResourcePackFailure = null;

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
                        case "CompressionHelperTypeName":
                            CompressionHelperTypeName = xmlNode.InnerText;
                            break;

                        case "OutputDirectory":
                            WorkingDirectory = xmlNode.InnerText;
                            break;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string[] GetCompressionHelperTypeNames()
        {
            return m_CompressionHelperTypeNames.ToArray();
        }

        public string[] GetVersionNames()
        {
            if (Platform == Platform.Undefined || !IsValidWorkingDirectory)
            {
                return EmptyStringArray;
            }

            string platformName = Platform.ToString();
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(SourcePath);
            if (!sourceDirectoryInfo.Exists)
            {
                return EmptyStringArray;
            }

            List<string> versionNames = new List<string>();
            foreach (DirectoryInfo directoryInfo in sourceDirectoryInfo.GetDirectories())
            {
                string[] splitedVersionNames = directoryInfo.Name.Split('_');
                if (splitedVersionNames.Length < 2)
                {
                    continue;
                }

                bool invalid = false;
                int value = 0;
                for (int i = 0; i < splitedVersionNames.Length; i++)
                {
                    if (!int.TryParse(splitedVersionNames[i], out value))
                    {
                        invalid = true;
                        break;
                    }
                }

                if (invalid)
                {
                    continue;
                }

                DirectoryInfo platformDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, platformName));
                if (!platformDirectoryInfo.Exists)
                {
                    continue;
                }

                FileInfo[] versionListFiles = platformDirectoryInfo.GetFiles("GameFrameworkVersion.*.dat", SearchOption.TopDirectoryOnly);
                if (versionListFiles.Length != 1)
                {
                    continue;
                }

                versionNames.Add(directoryInfo.Name);
            }

            versionNames.Sort((x, y) =>
            {
                return int.Parse(x.Substring(x.LastIndexOf('_') + 1)).CompareTo(int.Parse(y.Substring(y.LastIndexOf('_') + 1)));
            });

            return versionNames.ToArray();
        }

        public bool RefreshCompressionHelper()
        {
            bool retVal = false;
            if (!string.IsNullOrEmpty(CompressionHelperTypeName) && m_CompressionHelperTypeNames.Contains(CompressionHelperTypeName))
            {
                System.Type compressionHelperType = Utility.Assembly.GetType(CompressionHelperTypeName);
                if (compressionHelperType != null)
                {
                    Utility.Compression.ICompressionHelper compressionHelper = (Utility.Compression.ICompressionHelper)Activator.CreateInstance(compressionHelperType);
                    if (compressionHelper != null)
                    {
                        Utility.Compression.SetCompressionHelper(compressionHelper);
                        return true;
                    }
                }
            }
            else
            {
                retVal = true;
            }

            CompressionHelperTypeName = string.Empty;
            Utility.Compression.SetCompressionHelper(null);
            return retVal;
        }

        public void BuildResourcePacks(string[] sourceVersions, string targetVersion)
        {
            int count = sourceVersions.Length;
            if (OnBuildResourcePacksStarted != null)
            {
                OnBuildResourcePacksStarted(count);
            }

            int successCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (BuildResourcePack(sourceVersions[i], targetVersion))
                {
                    successCount++;
                    if (OnBuildResourcePackSuccess != null)
                    {
                        OnBuildResourcePackSuccess(i, count, sourceVersions[i], targetVersion);
                    }
                }
                else
                {
                    if (OnBuildResourcePackFailure != null)
                    {
                        OnBuildResourcePackFailure(i, count, sourceVersions[i], targetVersion);
                    }
                }
            }

            if (OnBuildResourcePacksCompleted != null)
            {
                OnBuildResourcePacksCompleted(successCount, count);
            }
        }

        public bool BuildResourcePack(string sourceVersion, string targetVersion)
        {
            try
            {
                if (!Directory.Exists(OutputPath))
                {
                    Directory.CreateDirectory(OutputPath);
                }

                string defaultBackupDiffPath = Path.Combine(OutputPath, DefaultResourcePackName);
                string defaultResourcePackName = Utility.Text.Format("{0}.{1}", defaultBackupDiffPath, DefaultExtension);
                if (File.Exists(defaultResourcePackName))
                {
                    File.Delete(defaultResourcePackName);
                }

                if (BackupDiff)
                {
                    if (Directory.Exists(defaultBackupDiffPath))
                    {
                        Directory.Delete(defaultBackupDiffPath, true);
                    }

                    Directory.CreateDirectory(defaultBackupDiffPath);
                }

                UpdatableVersionList sourceUpdatableVersionList = default(UpdatableVersionList);
                if (sourceVersion != null)
                {
                    DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(Path.Combine(Path.Combine(SourcePath, sourceVersion), Platform.ToString()));
                    FileInfo[] sourceVersionListFiles = sourceDirectoryInfo.GetFiles("GameFrameworkVersion.*.dat", SearchOption.TopDirectoryOnly);
                    byte[] sourceVersionListBytes = File.ReadAllBytes(sourceVersionListFiles[0].FullName);
                    sourceVersionListBytes = Utility.Compression.Decompress(sourceVersionListBytes);
                    using (Stream stream = new MemoryStream(sourceVersionListBytes))
                    {
                        sourceUpdatableVersionList = m_UpdatableVersionListSerializer.Deserialize(stream);
                    }
                }

                UpdatableVersionList targetUpdatableVersionList = default(UpdatableVersionList);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(Path.Combine(Path.Combine(SourcePath, targetVersion), Platform.ToString()));
                FileInfo[] targetVersionListFiles = targetDirectoryInfo.GetFiles("GameFrameworkVersion.*.dat", SearchOption.TopDirectoryOnly);
                byte[] targetVersionListBytes = File.ReadAllBytes(targetVersionListFiles[0].FullName);
                targetVersionListBytes = Utility.Compression.Decompress(targetVersionListBytes);
                using (Stream stream = new MemoryStream(targetVersionListBytes))
                {
                    targetUpdatableVersionList = m_UpdatableVersionListSerializer.Deserialize(stream);
                }

                List<ResourcePackVersionList.Resource> resources = new List<ResourcePackVersionList.Resource>();
                UpdatableVersionList.Resource[] sourceResources = sourceUpdatableVersionList.IsValid ? sourceUpdatableVersionList.GetResources() : EmptyResourceArray;
                UpdatableVersionList.Resource[] targetResources = targetUpdatableVersionList.GetResources();
                long offset = 0L;
                foreach (UpdatableVersionList.Resource targetResource in targetResources)
                {
                    bool ready = false;
                    foreach (UpdatableVersionList.Resource sourceResource in sourceResources)
                    {
                        if (sourceResource.Name != targetResource.Name || sourceResource.Variant != targetResource.Variant || sourceResource.Extension != targetResource.Extension)
                        {
                            continue;
                        }

                        if (sourceResource.LoadType == targetResource.LoadType && sourceResource.Length == targetResource.Length && sourceResource.HashCode == targetResource.HashCode)
                        {
                            ready = true;
                        }

                        break;
                    }

                    if (!ready)
                    {
                        resources.Add(new ResourcePackVersionList.Resource(targetResource.Name, targetResource.Variant, targetResource.Extension, targetResource.LoadType, offset, targetResource.Length, targetResource.HashCode, targetResource.CompressedLength, targetResource.CompressedHashCode));
                        offset += targetResource.CompressedLength;
                    }
                }

                ResourcePackVersionList.Resource[] resourceArray = resources.ToArray();
                using (FileStream fileStream = new FileStream(defaultResourcePackName, FileMode.Create, FileAccess.Write))
                {
                    if (!m_ResourcePackVersionListSerializer.Serialize(fileStream, new ResourcePackVersionList(0, 0L, 0, resourceArray)))
                    {
                        return false;
                    }
                }

                int position = 0;
                int hashCode = 0;
                string targetDirectoryPath = targetDirectoryInfo.FullName;
                using (FileStream fileStream = new FileStream(defaultResourcePackName, FileMode.Open, FileAccess.ReadWrite))
                {
                    position = (int)fileStream.Length;
                    fileStream.Position = position;
                    foreach (ResourcePackVersionList.Resource resource in resourceArray)
                    {
                        string resourceName = Path.Combine(targetDirectoryPath, GetResourceFullName(resource.Name, resource.Variant, resource.HashCode));
                        if (!File.Exists(resourceName))
                        {
                            return false;
                        }

                        byte[] resourceBytes = File.ReadAllBytes(resourceName);
                        fileStream.Write(resourceBytes, 0, resourceBytes.Length);
                        if (BackupDiff)
                        {
                            string backupDiffName = Path.Combine(defaultBackupDiffPath, GetResourceFullName(resource.Name, resource.Variant, resource.HashCode));
                            string directoryName = Path.GetDirectoryName(backupDiffName);
                            if (!Directory.Exists(directoryName))
                            {
                                Directory.CreateDirectory(directoryName);
                            }

                            File.WriteAllBytes(backupDiffName, resourceBytes);
                        }
                    }

                    if (fileStream.Position - position != offset)
                    {
                        return false;
                    }

                    fileStream.Position = position;
                    hashCode = Utility.Verifier.GetCrc32(fileStream);

                    fileStream.Position = 0L;
                    if (!m_ResourcePackVersionListSerializer.Serialize(fileStream, new ResourcePackVersionList(position, offset, hashCode, resourceArray)))
                    {
                        return false;
                    }
                }

                string backupDiffPath = Path.Combine(OutputPath, Utility.Text.Format("{0}-{1}-{2}", DefaultResourcePackName, sourceVersion ?? GetNoneVersion(targetVersion), targetVersion));
                string resourcePackName = Utility.Text.Format("{0}.{1:x8}.{2}", backupDiffPath, hashCode, DefaultExtension);
                if (File.Exists(resourcePackName))
                {
                    File.Delete(resourcePackName);
                }

                File.Move(defaultResourcePackName, resourcePackName);

                if (BackupDiff)
                {
                    if (BackupVersion)
                    {
                        File.Copy(targetVersionListFiles[0].FullName, Path.Combine(defaultBackupDiffPath, Path.GetFileName(targetVersionListFiles[0].FullName)));
                    }

                    if (Directory.Exists(backupDiffPath))
                    {
                        Directory.Delete(backupDiffPath, true);
                    }

                    Directory.Move(defaultBackupDiffPath, backupDiffPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetNoneVersion(string targetVersion)
        {
            string[] splitedVersionNames = targetVersion.Split('_');
            for (int i = 0; i < splitedVersionNames.Length; i++)
            {
                splitedVersionNames[i] = "0";
            }

            return string.Join("_", splitedVersionNames);
        }

        private string GetResourceFullName(string name, string variant, int hashCode)
        {
            return !string.IsNullOrEmpty(variant) ? Utility.Text.Format("{0}.{1}.{2:x8}.{3}", name, variant, hashCode, DefaultExtension) : Utility.Text.Format("{0}.{1:x8}.{2}", name, hashCode, DefaultExtension);
        }
    }
}
