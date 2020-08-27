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
    /// UnityEngine.Color 变量类。
    /// </summary>
    public sealed class VarColor : Variable<Color>
    {
        /// <summary>
        /// 初始化 UnityEngine.Color 变量类的新实例。
        /// </summary>
        public VarColor()
        {
        }

        /// <summary>
        /// 初始化 UnityEngine.Color 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarColor(Color value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 UnityEngine.Color 到 UnityEngine.Color 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarColor(Color value)
        {
            return new VarColor(value);
        }

        /// <summary>
        /// 从 UnityEngine.Color 变量类到 UnityEngine.Color 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Color(VarColor value)
        {
            return value.Value;
        }
    }
}
