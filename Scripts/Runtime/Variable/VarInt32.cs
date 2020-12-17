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
    /// System.Int32 变量类。
    /// </summary>
    public sealed class VarInt32 : Variable<int>
    {
        /// <summary>
        /// 初始化 System.Int32 变量类的新实例。
        /// </summary>
        public VarInt32()
        {
        }

        /// <summary>
        /// 从 System.Int32 到 System.Int32 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarInt32(int value)
        {
            VarInt32 varValue = ReferencePool.Acquire<VarInt32>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int32 变量类到 System.Int32 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator int(VarInt32 value)
        {
            return value.Value;
        }
    }
}
