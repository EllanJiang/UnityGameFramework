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
    /// short 变量类。
    /// </summary>
    public sealed class VarShort : Variable<short>
    {
        /// <summary>
        /// 初始化 short 变量类的新实例。
        /// </summary>
        public VarShort()
        {
        }

        /// <summary>
        /// 从 short 到 short 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarShort(short value)
        {
            VarShort varValue = ReferencePool.Acquire<VarShort>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 short 变量类到 short 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator short(VarShort value)
        {
            return value.Value;
        }
    }
}
