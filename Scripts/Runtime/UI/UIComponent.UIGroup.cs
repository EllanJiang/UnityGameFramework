﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class UIComponent : GameFrameworkComponent
    {
        [Serializable]
        private sealed class UIGroup
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private int m_Depth = 0;

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public int Depth
            {
                get
                {
                    return m_Depth;
                }
            }
        }
    }
}
