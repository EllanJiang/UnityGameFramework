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
    internal sealed class AttachEntityInfo : IReference
    {
        private Transform m_ParentTransform;
        private object m_UserData;

        public AttachEntityInfo()
        {
            m_ParentTransform = null;
            m_UserData = null;
        }

        public Transform ParentTransform
        {
            get
            {
                return m_ParentTransform;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(Transform parentTransform, object userData)
        {
            m_ParentTransform = parentTransform;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_ParentTransform = null;
            m_UserData = null;
        }
    }
}
