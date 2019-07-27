//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;

namespace UnityGameFramework.Runtime
{
    internal sealed class ShowEntityInfo : IReference
    {
        private Type m_EntityLogicType;
        private object m_UserData;

        public ShowEntityInfo()
        {
            m_EntityLogicType = null;
            m_UserData = null;
        }

        public Type EntityLogicType
        {
            get
            {
                return m_EntityLogicType;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(Type entityLogicType, object userData)
        {
            m_EntityLogicType = entityLogicType;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_EntityLogicType = null;
            m_UserData = null;
        }
    }
}
