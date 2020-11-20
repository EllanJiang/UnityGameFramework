//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// uint 变量类。
    /// </summary>
    public sealed class VarUInt : Variable<uint>
    {
        /// <summary>
        /// 初始化 uint 变量类的新实例。
        /// </summary>
        public VarUInt()
        {
        }

        /// <summary>
        /// 从 uint 到 uint 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUInt(uint value)
        {
            VarUInt varValue = ReferencePool.Acquire<VarUInt>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 uint 变量类到 uint 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator uint(VarUInt value)
        {
            return value.Value;
        }
    }
}
