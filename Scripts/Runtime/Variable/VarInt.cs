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
    /// int 变量类。
    /// </summary>
    public sealed class VarInt : Variable<int>
    {
        /// <summary>
        /// 初始化 int 变量类的新实例。
        /// </summary>
        public VarInt()
        {
        }

        /// <summary>
        /// 初始化 int 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarInt(int value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 int 到 int 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarInt(int value)
        {
            return new VarInt(value);
        }

        /// <summary>
        /// 从 int 变量类到 int 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator int(VarInt value)
        {
            return value.Value;
        }
    }
}
