//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 数据表辅助器基类。
    /// </summary>
    public abstract class DataTableHelperBase : MonoBehaviour, IDataTableHelper
    {
        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataTableAsset">数据表资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        public bool LoadDataTable(object dataTableAsset, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            return LoadDataTable(loadDataTableInfo.DataRowType, loadDataTableInfo.DataTableName, loadDataTableInfo.DataTableNameInType, dataTableAsset, loadDataTableInfo.UserData);
        }

        /// <summary>
        /// 将要解析的数据表文本分割为数据表行文本。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行文本。</returns>
        public abstract string[] SplitToDataRows(string text);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        public abstract void ReleaseDataTableAsset(object dataTableAsset);

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAsset">数据表资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        protected abstract bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, object dataTableAsset, object userData);
    }
}
