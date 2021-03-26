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
    /// System.Single 变量类。
    /// </summary>
    public sealed class VarSingle : Variable<float>
    {
        /// <summary>
        /// 初始化 System.Single 变量类的新实例。
        /// </summary>
        public VarSingle()
        {
        }

        /// <summary>
        /// 从 System.Single 到 System.Single 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarSingle(float value)
        {
            VarSingle varValue = ReferencePool.Acquire<VarSingle>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Single 变量类到 System.Single 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator float(VarSingle value)
        {
            return value.Value;
        }
    }
}
