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
    /// bool 变量类。
    /// </summary>
    public sealed class VarBool : Variable<bool>
    {
        /// <summary>
        /// 初始化 bool 变量类的新实例。
        /// </summary>
        public VarBool()
        {
        }

        /// <summary>
        /// 初始化 bool 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarBool(bool value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 bool 到 bool 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarBool(bool value)
        {
            return new VarBool(value);
        }

        /// <summary>
        /// 从 bool 变量类到 bool 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator bool(VarBool value)
        {
            return value.Value;
        }
    }
}
