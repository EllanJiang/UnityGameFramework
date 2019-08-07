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

        public static LoadDictionaryInfo Create(string dictionaryName, object userData)
        {
            LoadDictionaryInfo loadDictionaryInfo = ReferencePool.Acquire<LoadDictionaryInfo>();
            loadDictionaryInfo.m_DictionaryName = dictionaryName;
            loadDictionaryInfo.m_UserData = userData;
            return loadDictionaryInfo;
        }

        public void Clear()
        {
            m_DictionaryName = null;
            m_UserData = null;
        }
    }
}
