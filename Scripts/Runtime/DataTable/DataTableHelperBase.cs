//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        public bool LoadDataTable(object dataTableAsset, LoadType loadType, object userData)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)userData;
            return LoadDataTable(loadDataTableInfo.DataRowType, loadDataTableInfo.DataTableName, loadDataTableInfo.DataTableNameInType, dataTableAsset, loadType, loadDataTableInfo.UserData);
        }

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行片段。</returns>
        public abstract IEnumerable<GameFrameworkSegment<string>> GetDataRowSegments(string text);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        public abstract IEnumerable<GameFrameworkSegment<byte[]>> GetDataRowSegments(byte[] bytes);

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        public abstract IEnumerable<GameFrameworkSegment<Stream>> GetDataRowSegments(Stream stream);

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
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected abstract bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, object dataTableAsset, LoadType loadType, object userData);
    }
}
