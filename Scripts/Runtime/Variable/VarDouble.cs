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
    /// double 变量类。
    /// </summary>
    public sealed class VarDouble : Variable<double>
    {
        /// <summary>
        /// 初始化 double 变量类的新实例。
        /// </summary>
        public VarDouble()
        {
        }

        /// <summary>
        /// 从 double 到 double 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarDouble(double value)
        {
            VarDouble varValue = ReferencePool.Acquire<VarDouble>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 double 变量类到 double 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator double(VarDouble value)
        {
            return value.Value;
        }
    }
}
