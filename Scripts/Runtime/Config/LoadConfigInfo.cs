//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    internal sealed class LoadConfigInfo : IReference
    {
        private string m_ConfigName;
        private object m_UserData;

        public LoadConfigInfo()
        {
            m_ConfigName = null;
            m_UserData = null;
        }

        public string ConfigName
        {
            get
            {
                return m_ConfigName;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(string configName, object userData)
        {
            m_ConfigName = configName;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_ConfigName = null;
            m_UserData = null;
        }
    }
}
