//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
        /// 从 UnityEngine.Color32 到 UnityEngine.Color32 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarColor32(Color32 value)
        {
            VarColor32 varValue = ReferencePool.Acquire<VarColor32>();
            varValue.Value = value;
            return varValue;
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
