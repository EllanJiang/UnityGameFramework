//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 引用强制检查类型。
    /// </summary>
    public enum ReferenceStrictCheckType
    {
        /// <summary>
        /// 总是关闭。
        /// </summary>
        AlwaysOpen = 0,

        /// <summary>
        /// 仅在开发模式时打开。
        /// </summary>
        OnlyWhenDevelopment,

        /// <summary>
        /// 仅在编辑器中打开。
        /// </summary>
        OnlyInEditor,

        /// <summary>
        /// 总是打开。
        /// </summary>
        AlwaysClose,
    }
}
