//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    internal sealed class LoadDictionaryInfo : IReference
    {
        private string m_DictionaryName;
        private object m_UserData;

        public LoadDictionaryInfo()
        {
            m_DictionaryName = null;
            m_UserData = null;
        }

        public string DictionaryName
        {
            get
            {
                return m_DictionaryName;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(string dictionaryName, object userData)
        {
            m_DictionaryName = dictionaryName;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_DictionaryName = null;
            m_UserData = null;
        }
    }
}
