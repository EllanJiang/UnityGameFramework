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
    internal sealed class WWWFormInfo : IReference
    {
        private WWWForm m_WWWForm;
        private object m_UserData;

        public WWWFormInfo()
        {
            m_WWWForm = null;
            m_UserData = null;
        }

        public WWWForm WWWForm
        {
            get
            {
                return m_WWWForm;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(WWWForm wwwForm, object userData)
        {
            m_WWWForm = wwwForm;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_WWWForm = null;
            m_UserData = null;
        }
    }
}
