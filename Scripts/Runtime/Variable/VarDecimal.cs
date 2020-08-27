﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// decimal 变量类。
    /// </summary>
    public sealed class VarDecimal : Variable<decimal>
    {
        /// <summary>
        /// 初始化 decimal 变量类的新实例。
        /// </summary>
        public VarDecimal()
        {
        }

        /// <summary>
        /// 初始化 decimal 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarDecimal(decimal value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 decimal 到 decimal 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarDecimal(decimal value)
        {
            return new VarDecimal(value);
        }

        /// <summary>
        /// 从 decimal 变量类到 decimal 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator decimal(VarDecimal value)
        {
            return value.Value;
        }
    }
}
