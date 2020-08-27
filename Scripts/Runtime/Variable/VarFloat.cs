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
    /// float 变量类。
    /// </summary>
    public sealed class VarFloat : Variable<float>
    {
        /// <summary>
        /// 初始化 float 变量类的新实例。
        /// </summary>
        public VarFloat()
        {
        }

        /// <summary>
        /// 初始化 float 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarFloat(float value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 float 到 float 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarFloat(float value)
        {
            return new VarFloat(value);
        }

        /// <summary>
        /// 从 float 变量类到 float 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator float(VarFloat value)
        {
            return value.Value;
        }
    }
}
