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
    /// string 变量类。
    /// </summary>
    public sealed class VarString : Variable<string>
    {
        /// <summary>
        /// 初始化 string 变量类的新实例。
        /// </summary>
        public VarString()
        {
        }

        /// <summary>
        /// 从 string 到 string 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarString(string value)
        {
            VarString varValue = ReferencePool.Acquire<VarString>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 string 变量类到 string 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator string(VarString value)
        {
            return value.Value;
        }
    }
}
