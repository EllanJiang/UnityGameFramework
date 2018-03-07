//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    internal sealed class LoadConfigInfo
    {
        private readonly string m_ConfigName;
        private readonly object m_UserData;

        public LoadConfigInfo(string configName, object userData)
        {
            m_ConfigName = configName;
            m_UserData = userData;
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
    }
}
