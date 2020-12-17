//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 数据表辅助器基类。
    /// </summary>
    public abstract class DataTableHelperBase : MonoBehaviour, IDataProviderHelper<DataTableBase>, IDataTableHelper
    {
        /// <summary>
        /// 读取数据表。
        /// </summary>
        /// <param name="dataTable">数据表。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableAsset">数据表资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取数据表成功。</returns>
        public abstract bool ReadData(DataTableBase dataTable, string dataTableAssetName, object dataTableAsset, object userData);

        /// <summary>
        /// 读取数据表。
        /// </summary>
        /// <param name="dataTable">数据表。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableBytes">数据表二进制流。</param>
        /// <param name="startIndex">数据表二进制流的起始位置。</param>
        /// <param name="length">数据表二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取数据表成功。</returns>
        public abstract bool ReadData(DataTableBase dataTable, string dataTableAssetName, byte[] dataTableBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 解析数据表。
        /// </summary>
        /// <param name="dataTable">数据表。</param>
        /// <param name="dataTableString">要解析的数据表字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析数据表成功。</returns>
        public abstract bool ParseData(DataTableBase dataTable, string dataTableString, object userData);

        /// <summary>
        /// 解析数据表。
        /// </summary>
        /// <param name="dataTable">数据表。</param>
        /// <param name="dataTableBytes">要解析的数据表二进制流。</param>
        /// <param name="startIndex">数据表二进制流的起始位置。</param>
        /// <param name="length">数据表二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析数据表成功。</returns>
        public abstract bool ParseData(DataTableBase dataTable, byte[] dataTableBytes, int startIndex, int length, object userData);

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTable">数据表。</param>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        public abstract void ReleaseDataAsset(DataTableBase dataTable, object dataTableAsset);
    }
}
