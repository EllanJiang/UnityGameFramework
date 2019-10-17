//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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

        public static LoadConfigInfo Create(string configName, object userData)
        {
            LoadConfigInfo loadConfigInfo = ReferencePool.Acquire<LoadConfigInfo>();
            loadConfigInfo.m_ConfigName = configName;
            loadConfigInfo.m_UserData = userData;
            return loadConfigInfo;
        }

        public void Clear()
        {
            m_ConfigName = null;
            m_UserData = null;
        }
    }
}
