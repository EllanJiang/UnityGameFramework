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
    /// sbyte 变量类。
    /// </summary>
    public sealed class VarSByte : Variable<sbyte>
    {
        /// <summary>
        /// 初始化 sbyte 变量类的新实例。
        /// </summary>
        public VarSByte()
        {
        }

        /// <summary>
        /// 从 sbyte 到 sbyte 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarSByte(sbyte value)
        {
            VarSByte varValue = ReferencePool.Acquire<VarSByte>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 sbyte 变量类到 sbyte 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator sbyte(VarSByte value)
        {
            return value.Value;
        }
    }
}
