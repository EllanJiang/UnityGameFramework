//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Editor.ResourceTools
{
    /// <summary>
    /// 资源类型。
    /// </summary>
    public enum AssetType : byte
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 存放资源。
        /// </summary>
        Asset,

        /// <summary>
        /// 存放场景。
        /// </summary>
        Scene,
    }
}
