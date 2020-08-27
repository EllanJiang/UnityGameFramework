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
    /// char 变量类。
    /// </summary>
    public sealed class VarChar : Variable<char>
    {
        /// <summary>
        /// 初始化 char 变量类的新实例。
        /// </summary>
        public VarChar()
        {
        }

        /// <summary>
        /// 初始化 char 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarChar(char value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 char 到 char 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarChar(char value)
        {
            return new VarChar(value);
        }

        /// <summary>
        /// 从 char 变量类到 char 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator char(VarChar value)
        {
            return value.Value;
        }
    }
}
