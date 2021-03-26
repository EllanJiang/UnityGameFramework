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
    /// System.Int64 变量类。
    /// </summary>
    public sealed class VarInt64 : Variable<long>
    {
        /// <summary>
        /// 初始化 System.Int64 变量类的新实例。
        /// </summary>
        public VarInt64()
        {
        }

        /// <summary>
        /// 从 System.Int64 到 System.Int64 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarInt64(long value)
        {
            VarInt64 varValue = ReferencePool.Acquire<VarInt64>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int64 变量类到 System.Int64 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator long(VarInt64 value)
        {
            return value.Value;
        }
    }
}
