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
    /// System.UInt32 变量类。
    /// </summary>
    public sealed class VarUInt32 : Variable<uint>
    {
        /// <summary>
        /// 初始化 System.UInt32 变量类的新实例。
        /// </summary>
        public VarUInt32()
        {
        }

        /// <summary>
        /// 从 System.UInt32 到 System.UInt32 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUInt32(uint value)
        {
            VarUInt32 varValue = ReferencePool.Acquire<VarUInt32>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.UInt32 变量类到 System.UInt32 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator uint(VarUInt32 value)
        {
            return value.Value;
        }
    }
}
