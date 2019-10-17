//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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

        public static LoadDataTableInfo Create(Type dataRowType, string dataTableName, string dataTableNameInType, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = ReferencePool.Acquire<LoadDataTableInfo>();
            loadDataTableInfo.m_DataRowType = dataRowType;
            loadDataTableInfo.m_DataTableName = dataTableName;
            loadDataTableInfo.m_DataTableNameInType = dataTableNameInType;
            loadDataTableInfo.m_UserData = userData;
            return loadDataTableInfo;
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
