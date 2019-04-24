//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Color32 变量类。
    /// </summary>
    public sealed class VarColor32 : Variable<Color32>
    {
        /// <summary>
        /// 初始化 UnityEngine.Color32 变量类的新实例。
        /// </summary>
        public VarColor32()
        {
        }

        /// <summary>
        /// 初始化 UnityEngine.Color32 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarColor32(Color32 value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 UnityEngine.Color32 到 UnityEngine.Color32 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarColor32(Color32 value)
        {
            return new VarColor32(value);
        }

        /// <summary>
        /// 从 UnityEngine.Color32 变量类到 UnityEngine.Color32 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Color32(VarColor32 value)
        {
            return value.Value;
        }
    }
}
