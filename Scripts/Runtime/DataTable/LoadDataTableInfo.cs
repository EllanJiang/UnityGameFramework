//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace UnityGameFramework.Runtime
{
    internal sealed class LoadDataTableInfo
    {
        private readonly Type m_DataRowType;
        private readonly string m_DataTableName;
        private readonly string m_DataTableNameInType;
        private readonly object m_UserData;

        public LoadDataTableInfo(Type dataRowType, string dataTableName, string dataTableNameInType, object userData)
        {
            m_DataRowType = dataRowType;
            m_DataTableName = dataTableName;
            m_DataTableNameInType = dataTableNameInType;
            m_UserData = userData;
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
    }
}
