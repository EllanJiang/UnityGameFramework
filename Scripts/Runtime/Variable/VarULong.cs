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
    /// ulong 变量类。
    /// </summary>
    public sealed class VarULong : Variable<ulong>
    {
        /// <summary>
        /// 初始化 ulong 变量类的新实例。
        /// </summary>
        public VarULong()
        {
        }

        /// <summary>
        /// 初始化 ulong 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarULong(ulong value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 ulong 到 ulong 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarULong(ulong value)
        {
            return new VarULong(value);
        }

        /// <summary>
        /// 从 ulong 变量类到 ulong 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator ulong(VarULong value)
        {
            return value.Value;
        }
    }
}
