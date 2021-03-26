//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Texture 变量类。
    /// </summary>
    public sealed class VarTexture : Variable<Texture>
    {
        /// <summary>
        /// 初始化 UnityEngine.Texture 变量类的新实例。
        /// </summary>
        public VarTexture()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Texture 到 UnityEngine.Texture 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarTexture(Texture value)
        {
            VarTexture varValue = ReferencePool.Acquire<VarTexture>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Texture 变量类到 UnityEngine.Texture 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Texture(VarTexture value)
        {
            return value.Value;
        }
    }
}
