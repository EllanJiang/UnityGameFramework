﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Material 变量类。
    /// </summary>
    public sealed class VarMaterial : Variable<Material>
    {
        /// <summary>
        /// 初始化 UnityEngine.Material 变量类的新实例。
        /// </summary>
        public VarMaterial()
        {
        }

        /// <summary>
        /// 初始化 UnityEngine.Material 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarMaterial(Material value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 UnityEngine.Material 到 UnityEngine.Material 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarMaterial(Material value)
        {
            return new VarMaterial(value);
        }

        /// <summary>
        /// 从 UnityEngine.Material 变量类到 UnityEngine.Material 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Material(VarMaterial value)
        {
            return value.Value;
        }
    }
}
