//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor.ResourceTools
{
    public sealed class ResourcePackBuilderController
    {
        private const string DefaultResourcePackName = "GameFrameworkResourcePack";
        private const string DefaultExtension = "dat";
        private static readonly string[] EmptyStringArray = new string[0];
        private static readonly UpdatableVersionList.Resource[] EmptyResourceArray = new UpdatableVersionList.Resource[0];

        private readonly UpdatableVersionListSerializer m_UpdatableVersionListSerializer;
        private readonly ResourcePackVersionListSerializer m_ResourcePackVersionListSerializer;

        public ResourcePackBuilderController()
        {
            m_UpdatableVersionListSerializer = new UpdatableVersionListSerializer();
            m_UpdatableVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V0);
            m_UpdatableVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.UpdatableVersionListDeserializeCallback_V1);

            m_ResourcePackVersionListSerializer = new ResourcePackVersionListSerializer();
            m_ResourcePackVersionListSerializer.RegisterSerializeCallback(0, BuiltinVersionListSerializer.ResourcePackVersionListSerializeCallback_V0);

            Utility.Zip.SetZipHelper(new DefaultZipHelper());
            Platform = Platform.Windows;
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
                return Version.GameFrameworkVersion;
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

        public Platform Platform
        {
            get;
            set;
        }

        public int LengthLimit
        {
            get;
            set;
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
                string[] splitedVersion = directoryInfo.Name.Split('_');
                if (splitedVersion.Length != 4)
                {
                    continue;
                }

                int value = 0;
                if (!int.TryParse(splitedVersion[0], out value) || !int.TryParse(splitedVersion[1], out value) || !int.TryParse(splitedVersion[2], out value) || !int.TryParse(splitedVersion[3], out value))
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

        public void BuildResourcePacks(string sourceVersion, string[] targetVersions)
        {
            int count = targetVersions.Length;
            if (OnBuildResourcePacksStarted != null)
            {
                OnBuildResourcePacksStarted(count);
            }

            int successCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (BuildResourcePack(sourceVersion, targetVersions[i]))
                {
                    successCount++;
                    if (OnBuildResourcePackSuccess != null)
                    {
                        OnBuildResourcePackSuccess(i, count, sourceVersion, targetVersions[i]);
                    }
                }
                else
                {
                    if (OnBuildResourcePackFailure != null)
                    {
                        OnBuildResourcePackFailure(i, count, sourceVersion, targetVersions[i]);
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
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }

            string defaultResourcePackName = Path.Combine(OutputPath, Utility.Text.Format("{0}.{1}", DefaultResourcePackName, DefaultExtension));
            if (File.Exists(defaultResourcePackName))
            {
                File.Delete(defaultResourcePackName);
            }

            UpdatableVersionList sourceUpdatableVersionList = default(UpdatableVersionList);
            if (sourceVersion != null)
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(Path.Combine(Path.Combine(SourcePath, sourceVersion), Platform.ToString()));
                FileInfo[] sourceVersionListFiles = sourceDirectoryInfo.GetFiles("GameFrameworkVersion.*.dat", SearchOption.TopDirectoryOnly);
                byte[] sourceVersionListBytes = File.ReadAllBytes(sourceVersionListFiles[0].FullName);
                sourceVersionListBytes = Utility.Zip.Decompress(sourceVersionListBytes);
                using (Stream stream = new MemoryStream(sourceVersionListBytes))
                {
                    sourceUpdatableVersionList = m_UpdatableVersionListSerializer.Deserialize(stream);
                }
            }

            UpdatableVersionList targetUpdatableVersionList = default(UpdatableVersionList);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(Path.Combine(Path.Combine(SourcePath, targetVersion), Platform.ToString()));
            FileInfo[] targetVersionListFiles = targetDirectoryInfo.GetFiles("GameFrameworkVersion.*.dat", SearchOption.TopDirectoryOnly);
            byte[] targetVersionListBytes = File.ReadAllBytes(targetVersionListFiles[0].FullName);
            targetVersionListBytes = Utility.Zip.Decompress(targetVersionListBytes);
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
                    resources.Add(new ResourcePackVersionList.Resource(targetResource.Name, targetResource.Variant, targetResource.Extension, targetResource.LoadType, offset, targetResource.Length, targetResource.HashCode, targetResource.ZipLength, targetResource.ZipHashCode));
                    offset += targetResource.ZipLength;
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
                    string resourcePath = Path.Combine(targetDirectoryPath, GetResourceFullName(resource.Name, resource.Variant, resource.HashCode));
                    if (!File.Exists(resourcePath))
                    {
                        return false;
                    }

                    byte[] resourceBytes = File.ReadAllBytes(resourcePath);
                    fileStream.Write(resourceBytes, 0, resourceBytes.Length);
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

            string targetResourcePackName = Path.Combine(OutputPath, Utility.Text.Format("{0}-{1}-{2}.{3:x8}.{4}", DefaultResourcePackName, sourceVersion ?? "0_0_0_0", targetVersion, hashCode, DefaultExtension));
            if (File.Exists(targetResourcePackName))
            {
                File.Delete(targetResourcePackName);
            }

            File.Move(defaultResourcePackName, targetResourcePackName);
            return true;
        }

        private string GetResourceFullName(string name, string variant, int hashCode)
        {
            return !string.IsNullOrEmpty(variant) ? Utility.Text.Format("{0}.{1}.{3:x8}.{2}", name, variant, DefaultExtension, hashCode) : Utility.Text.Format("{0}.{2:x8}.{1}", name, DefaultExtension, hashCode);
        }
    }
}
