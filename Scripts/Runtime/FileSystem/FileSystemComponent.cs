﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.FileSystem;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 文件系统件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/File System")]
    public sealed class FileSystemComponent : GameFrameworkComponent
    {
        private IFileSystemManager m_FileSystemManager = null;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_FileSystemManager = GameFrameworkEntry.GetModule<IFileSystemManager>();
            if (m_FileSystemManager == null)
            {
                Log.Fatal("File system manager is invalid.");
                return;
            }
        }

        private void Start()
        {
        }

        /// <summary>
        /// 获取文件系统数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_FileSystemManager.Count;
            }
        }

        /// <summary>
        /// 创建文件系统。
        /// </summary>
        /// <param name="fullPath">要创建的文件系统的完整路径。</param>
        /// <param name="access">要创建的文件系统的访问方式。</param>
        /// <param name="maxFileCount">要创建的文件系统的最大文件数量。</param>
        /// <param name="maxBlockCount">要创建的文件系统的最大块数据数量。</param>
        /// <returns>创建的文件系统。</returns>
        public IFileSystem CreateFileSystem(string fullPath, FileSystemAccess access, int maxFileCount, int maxBlockCount)
        {
            return m_FileSystemManager.CreateFileSystem(fullPath, access, maxFileCount, maxBlockCount);
        }

        /// <summary>
        /// 加载文件系统。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <returns>加载的文件系统。</returns>
        public IFileSystem LoadFileSystem(string fullPath, FileSystemAccess access)
        {
            return m_FileSystemManager.LoadFileSystem(fullPath, access);
        }

        /// <summary>
        /// 销毁文件系统。
        /// </summary>
        /// <param name="fileSystem">要销毁的文件系统。</param>
        /// <param name="deletePhysicalFile">是否删除文件系统对应的物理文件。</param>
        public void DestroyFileSystem(IFileSystem fileSystem, bool deletePhysicalFile)
        {
            m_FileSystemManager.DestroyFileSystem(fileSystem, deletePhysicalFile);
        }

        /// <summary>
        /// 注册文件系统。
        /// </summary>
        /// <param name="name">要注册的文件系统的名称。</param>
        /// <param name="fileSystem">要注册的文件系统。</param>
        /// <returns>注册的文件系统是否成功。</returns>
        public bool RegisterFileSystem(string name, IFileSystem fileSystem)
        {
            return m_FileSystemManager.RegisterFileSystem(name, fileSystem);
        }

        /// <summary>
        /// 解除注册文件系统。
        /// </summary>
        /// <param name="name">要解除注册的文件系统的名称。</param>
        /// <param name="fileSystem">要解除注册的文件系统。</param>
        /// <returns>解除注册的文件系统是否成功。</returns>
        public bool UnregisterFileSystem(string name, IFileSystem fileSystem)
        {
            return m_FileSystemManager.UnregisterFileSystem(name, fileSystem);
        }

        /// <summary>
        /// 获取文件系统。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <returns>获取的文件系统。</returns>
        public IFileSystem GetFileSystem(string name)
        {
            return m_FileSystemManager.GetFileSystem(name);
        }

        /// <summary>
        /// 获取文件系统。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <returns>获取的文件系统。</returns>
        public IFileSystem GetFileSystem(string name, FileSystemAccess access)
        {
            return m_FileSystemManager.GetFileSystem(name, access);
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <returns>获取的文件系统集合。</returns>
        public IFileSystem[] GetFileSystems(string name)
        {
            return m_FileSystemManager.GetFileSystems(name);
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <returns>获取的文件系统集合。</returns>
        public IFileSystem[] GetFileSystems(string name, FileSystemAccess access)
        {
            return m_FileSystemManager.GetFileSystems(name, access);
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="results">获取的文件系统集合。</param>
        public void GetFileSystems(string name, List<IFileSystem> results)
        {
            m_FileSystemManager.GetFileSystems(name, results);
        }

        /// <summary>
        /// 获取文件系统集合。
        /// </summary>
        /// <param name="name">要获取的文件系统的名称。</param>
        /// <param name="access">要获取的文件系统的访问方式。</param>
        /// <param name="results">获取的文件系统集合。</param>
        private void GetFileSystems(string name, FileSystemAccess access, List<IFileSystem> results)
        {
            m_FileSystemManager.GetFileSystems(name, access, results);
        }

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <returns>获取的所有文件系统集合。</returns>
        public IFileSystem[] GetAllFileSystems()
        {
            return m_FileSystemManager.GetAllFileSystems();
        }

        /// <summary>
        /// 获取所有文件系统集合。
        /// </summary>
        /// <param name="results">获取的所有文件系统集合。</param>
        public void GetAllFileSystems(List<IFileSystem> results)
        {
            m_FileSystemManager.GetAllFileSystems(results);
        }
    }
}
