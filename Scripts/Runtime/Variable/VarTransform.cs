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
    /// UnityEngine.Transform 变量类。
    /// </summary>
    public sealed class VarTransform : Variable<Transform>
    {
        /// <summary>
        /// 初始化 UnityEngine.Transform 变量类的新实例。
        /// </summary>
        public VarTransform()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Transform 到 UnityEngine.Transform 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarTransform(Transform value)
        {
            VarTransform varValue = ReferencePool.Acquire<VarTransform>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Transform 变量类到 UnityEngine.Transform 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Transform(VarTransform value)
        {
            return value.Value;
        }
    }
}
