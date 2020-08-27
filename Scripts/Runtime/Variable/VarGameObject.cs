﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.GameObject 变量类。
    /// </summary>
    public sealed class VarGameObject : Variable<GameObject>
    {
        /// <summary>
        /// 初始化 UnityEngine.GameObject 变量类的新实例。
        /// </summary>
        public VarGameObject()
        {
        }

        /// <summary>
        /// 初始化 UnityEngine.GameObject 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarGameObject(GameObject value)
            : base(value)
        {
        }

        /// <summary>
        /// 从 UnityEngine.GameObject 到 UnityEngine.GameObject 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarGameObject(GameObject value)
        {
            return new VarGameObject(value);
        }

        /// <summary>
        /// 从 UnityEngine.GameObject 变量类到 UnityEngine.GameObject 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator GameObject(VarGameObject value)
        {
            return value.Value;
        }
    }
}
