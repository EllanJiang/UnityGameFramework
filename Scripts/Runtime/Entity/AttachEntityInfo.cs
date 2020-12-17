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

        public static AttachEntityInfo Create(Transform parentTransform, object userData)
        {
            AttachEntityInfo attachEntityInfo = ReferencePool.Acquire<AttachEntityInfo>();
            attachEntityInfo.m_ParentTransform = parentTransform;
            attachEntityInfo.m_UserData = userData;
            return attachEntityInfo;
        }

        public void Clear()
        {
            m_ParentTransform = null;
            m_UserData = null;
        }
    }
}
