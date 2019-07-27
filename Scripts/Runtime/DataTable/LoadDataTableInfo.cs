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
    internal sealed class LoadDataTableInfo : IReference
    {
        private Type m_DataRowType;
        private string m_DataTableName;
        private string m_DataTableNameInType;
        private object m_UserData;

        public LoadDataTableInfo()
        {
            m_DataRowType = null;
            m_DataTableName = null;
            m_DataTableNameInType = null;
            m_UserData = null;
        }

        public Type DataRowType
        {
            get
            {
                return m_DataRowType;
            }
        }

        public string DataTableName
        {
            get
            {
                return m_DataTableName;
            }
        }

        public string DataTableNameInType
        {
            get
            {
                return m_DataTableNameInType;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void Initialize(Type dataRowType, string dataTableName, string dataTableNameInType, object userData)
        {
            m_DataRowType = dataRowType;
            m_DataTableName = dataTableName;
            m_DataTableNameInType = dataTableNameInType;
            m_UserData = userData;
        }

        public void Clear()
        {
            m_DataRowType = null;
            m_DataTableName = null;
            m_DataTableNameInType = null;
            m_UserData = null;
        }
    }
}
