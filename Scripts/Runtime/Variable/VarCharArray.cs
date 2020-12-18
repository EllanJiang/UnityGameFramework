//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// System.Char 数组变量类。
    /// </summary>
    public sealed class VarCharArray : Variable<char[]>
    {
        /// <summary>
        /// 初始化 System.Char 数组变量类的新实例。
        /// </summary>
        public VarCharArray()
        {
        }

        /// <summary>
        /// 从 System.Char 数组到 System.Char 数组变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarCharArray(char[] value)
        {
            VarCharArray varValue = ReferencePool.Acquire<VarCharArray>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Char 数组变量类到 System.Char 数组的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator char[](VarCharArray value)
        {
            return value.Value;
        }
    }
}
