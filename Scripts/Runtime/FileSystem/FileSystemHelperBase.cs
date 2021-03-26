//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.FileSystem;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 文件系统辅助器基类。
    /// </summary>
    public abstract class FileSystemHelperBase : MonoBehaviour, IFileSystemHelper
    {
        /// <summary>
        /// 创建文件系统流。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <param name="createNew">是否创建新的文件系统流。</param>
        /// <returns>创建的文件系统流。</returns>
        public abstract FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew);
    }
}
