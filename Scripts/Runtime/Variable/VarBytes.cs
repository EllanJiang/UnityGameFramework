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
    /// byte[] 变量类。
    /// </summary>
    public sealed class VarBytes : Variable<byte[]>
    {
        /// <summary>
        /// 初始化 byte[] 变量类的新实例。
        /// </summary>
        public VarBytes()
        {
        }

        /// <summary>
        /// 初始化 byte[] 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarBytes(byte[] value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 byte[] 到 byte[] 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarBytes(byte[] value)
        {
            return new VarBytes(value);
        }

        /// <summary>
        /// 从 byte[] 变量类到 byte[] 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator byte[](VarBytes value)
        {
            return value.Value;
        }
    }
}
