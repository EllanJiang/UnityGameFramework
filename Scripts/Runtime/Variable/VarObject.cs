﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// object 变量类。
    /// </summary>
    public sealed class VarObject : Variable<object>
    {
        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        public VarObject()
        {
        }

        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarObject(object value)
            : base(value)
        {
        }
    }
}
