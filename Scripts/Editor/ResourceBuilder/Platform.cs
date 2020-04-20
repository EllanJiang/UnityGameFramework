//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace UnityGameFramework.Editor.ResourceTools
{
    [Flags]
    public enum Platform : byte
    {
        Undefined = 0,

        /// <summary>
        /// Windows 32 位。
        /// </summary>
        Windows = 1 << 0,

        /// <summary>
        /// Windows 64 位。
        /// </summary>
        Windows64 = 1 << 1,

        /// <summary>
        /// macOS。
        /// </summary>
        MacOS = 1 << 2,

        /// <summary>
        /// Linux。
        /// </summary>
        Linux = 1 << 3,

        /// <summary>
        /// iOS。
        /// </summary>
        IOS = 1 << 4,

        /// <summary>
        /// Android。
        /// </summary>
        Android = 1 << 5,

        /// <summary>
        /// Windows Store。
        /// </summary>
        WindowsStore = 1 << 6,

        /// <summary>
        /// WebGL。
        /// </summary>
        WebGL = 1 << 7,
    }
}
