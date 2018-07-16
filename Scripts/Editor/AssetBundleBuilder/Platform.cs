//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    [Flags]
    public enum Platform
    {
        Undefined = 0,

        /// <summary>
        /// 微软 Windows 32 位。
        /// </summary>
        Windows = 1 << 0,

        /// <summary>
        /// 微软 Windows 64 位。
        /// </summary>
        Windows64 = 1 << 1,

        /// <summary>
        /// 苹果 macOS。
        /// </summary>
        MacOS = 1 << 2,

        /// <summary>
        /// Linux 32 位。
        /// </summary>
        Linux = 1 << 3,

        /// <summary>
        /// Linux 64 位。
        /// </summary>
        Linux64 = 1 << 4,

        /// <summary>
        /// Linux 通用。
        /// </summary>
        LinuxUniversal = 1 << 5,

        /// <summary>
        /// 苹果 iOS。
        /// </summary>
        IOS = 1 << 6,

        /// <summary>
        /// 谷歌 Android。
        /// </summary>
        Android = 1 << 7,

        /// <summary>
        /// 微软 Windows Store。
        /// </summary>
        WindowsStore = 1 << 8,

        /// <summary>
        /// WebGL。
        /// </summary>
        WebGL = 1 << 9,
    }
}
