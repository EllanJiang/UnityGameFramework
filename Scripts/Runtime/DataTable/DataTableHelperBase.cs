//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
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
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableObject">数据表对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadDataTable(string dataTableAssetName, object dataTableObject, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            return LoadDataTable(loadDataTableInfo.DataRowType, loadDataTableInfo.DataTableName, loadDataTableInfo.DataTableNameInType, dataTableAssetName, dataTableObject, loadDataTableInfo.UserData);
        }

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表行片段。</returns>
        public abstract GameFrameworkDataSegment[] GetDataRowSegments(object dataTableData);

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表用户自定义数据。</returns>
        public abstract object GetDataTableUserData(object dataTableData);

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
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableObject">数据表对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName, object dataTableObject, object userData);
    }
}
