//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 数据表组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Data Table")]
    public sealed class DataTableComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private IDataTableManager m_DataTableManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableLoadDataTableUpdateEvent = false;

        [SerializeField]
        private bool m_EnableLoadDataTableDependencyAssetEvent = false;

        [SerializeField]
        private string m_DataTableHelperTypeName = "UnityGameFramework.Runtime.DefaultDataTableHelper";

        [SerializeField]
        private DataTableHelperBase m_CustomDataTableHelper = null;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_DataTableManager = GameFrameworkEntry.GetModule<IDataTableManager>();
            if (m_DataTableManager == null)
            {
                Log.Fatal("Data table manager is invalid.");
                return;
            }

            m_DataTableManager.LoadDataTableSuccess += OnLoadDataTableSuccess;
            m_DataTableManager.LoadDataTableFailure += OnLoadDataTableFailure;

            if (m_EnableLoadDataTableUpdateEvent)
            {
                m_DataTableManager.LoadDataTableUpdate += OnLoadDataTableUpdate;
            }

            if (m_EnableLoadDataTableDependencyAssetEvent)
            {
                m_DataTableManager.LoadDataTableDependencyAsset += OnLoadDataTableDependencyAsset;
            }
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_DataTableManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_DataTableManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            DataTableHelperBase dataTableHelper = Helper.CreateHelper(m_DataTableHelperTypeName, m_CustomDataTableHelper);
            if (dataTableHelper == null)
            {
                Log.Error("Can not create data table helper.");
                return;
            }

            dataTableHelper.name = "Data Table Helper";
            Transform transform = dataTableHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_DataTableManager.SetDataTableHelper(dataTableHelper);
        }

        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_DataTableManager.Count;
            }
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableAssetName) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, null, dataTableAssetName, DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableAssetName)
        {
            LoadDataTable(dataRowType, dataTableName, null, dataTableAssetName, DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableAssetName, int priority) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, null, dataTableAssetName, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableAssetName, int priority)
        {
            LoadDataTable(dataRowType, dataTableName, null, dataTableAssetName, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableAssetName, object userData) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, null, dataTableAssetName, DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableAssetName, object userData)
        {
            LoadDataTable(dataRowType, dataTableName, null, dataTableAssetName, DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableAssetName, int priority, object userData) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, null, dataTableAssetName, priority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableAssetName, int priority, object userData)
        {
            LoadDataTable(dataRowType, dataTableName, null, dataTableAssetName, priority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableNameInType, string dataTableAssetName) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, dataTableNameInType, dataTableAssetName, DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName)
        {
            LoadDataTable(dataRowType, dataTableName, dataTableNameInType, dataTableAssetName, DefaultPriority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableNameInType, string dataTableAssetName, int priority) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, dataTableNameInType, dataTableAssetName, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName, int priority)
        {
            LoadDataTable(dataRowType, dataTableName, dataTableNameInType, dataTableAssetName, priority, null);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableNameInType, string dataTableAssetName, object userData) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, dataTableNameInType, dataTableAssetName, DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName, object userData)
        {
            LoadDataTable(dataRowType, dataTableName, dataTableNameInType, dataTableAssetName, DefaultPriority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable<T>(string dataTableName, string dataTableNameInType, string dataTableAssetName, int priority, object userData) where T : IDataRow
        {
            LoadDataTable(typeof(T), dataTableName, dataTableNameInType, dataTableAssetName, priority, userData);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="priority">加载数据表资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName, int priority, object userData)
        {
            if (dataRowType == null)
            {
                Log.Error("Data row type is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Error("Data table name is invalid.");
                return;
            }

            m_DataTableManager.LoadDataTable(dataTableAssetName, priority, LoadDataTableInfo.Create(dataRowType, dataTableName, dataTableNameInType, userData));
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable<T>() where T : IDataRow
        {
            return m_DataTableManager.HasDataTable<T>();
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable(Type dataRowType)
        {
            return m_DataTableManager.HasDataTable(dataRowType);
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable<T>(string name) where T : IDataRow
        {
            return m_DataTableManager.HasDataTable<T>(name);
        }

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否存在数据表。</returns>
        public bool HasDataTable(Type dataRowType, string name)
        {
            return m_DataTableManager.HasDataTable(dataRowType, name);
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        public IDataTable<T> GetDataTable<T>() where T : IDataRow
        {
            return m_DataTableManager.GetDataTable<T>();
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        public DataTableBase GetDataTable(Type dataRowType)
        {
            return m_DataTableManager.GetDataTable(dataRowType);
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public IDataTable<T> GetDataTable<T>(string name) where T : IDataRow
        {
            return m_DataTableManager.GetDataTable<T>(name);
        }

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>要获取的数据表。</returns>
        public DataTableBase GetDataTable(Type dataRowType, string name)
        {
            return m_DataTableManager.GetDataTable(dataRowType, name);
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        public DataTableBase[] GetAllDataTables()
        {
            return m_DataTableManager.GetAllDataTables();
        }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        public void GetAllDataTables(List<DataTableBase> results)
        {
            m_DataTableManager.GetAllDataTables(results);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(object dataTableData) where T : class, IDataRow, new()
        {
            return m_DataTableManager.CreateDataTable<T>(dataTableData);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, object dataTableData)
        {
            return m_DataTableManager.CreateDataTable(dataRowType, dataTableData);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>要创建的数据表。</returns>
        public IDataTable<T> CreateDataTable<T>(string name, object dataTableData) where T : class, IDataRow, new()
        {
            return m_DataTableManager.CreateDataTable<T>(name, dataTableData);
        }

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>要创建的数据表。</returns>
        public DataTableBase CreateDataTable(Type dataRowType, string name, object dataTableData)
        {
            return m_DataTableManager.CreateDataTable(dataRowType, name, dataTableData);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable<T>() where T : IDataRow, new()
        {
            return m_DataTableManager.DestroyDataTable<T>();
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable(Type dataRowType)
        {
            return m_DataTableManager.DestroyDataTable(dataRowType);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable<T>(string name) where T : IDataRow
        {
            return m_DataTableManager.DestroyDataTable<T>(name);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="name">数据表名称。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable(Type dataRowType, string name)
        {
            return m_DataTableManager.DestroyDataTable(dataRowType, name);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <param name="dataTable">要销毁的数据表。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable<T>(IDataTable<T> dataTable) where T : IDataRow
        {
            return m_DataTableManager.DestroyDataTable(dataTable);
        }

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataTable">要销毁的数据表。</param>
        /// <returns>是否销毁数据表成功。</returns>
        public bool DestroyDataTable(DataTableBase dataTable)
        {
            return m_DataTableManager.DestroyDataTable(dataTable);
        }

        private void OnLoadDataTableSuccess(object sender, GameFramework.DataTable.LoadDataTableSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDataTableSuccessEventArgs.Create(e));
        }

        private void OnLoadDataTableFailure(object sender, GameFramework.DataTable.LoadDataTableFailureEventArgs e)
        {
            Log.Warning("Load data table failure, asset name '{0}', error message '{1}'.", e.DataTableAssetName, e.ErrorMessage);
            m_EventComponent.Fire(this, LoadDataTableFailureEventArgs.Create(e));
        }

        private void OnLoadDataTableUpdate(object sender, GameFramework.DataTable.LoadDataTableUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDataTableUpdateEventArgs.Create(e));
        }

        private void OnLoadDataTableDependencyAsset(object sender, GameFramework.DataTable.LoadDataTableDependencyAssetEventArgs e)
        {
            m_EventComponent.Fire(this, LoadDataTableDependencyAssetEventArgs.Create(e));
        }
    }
}
