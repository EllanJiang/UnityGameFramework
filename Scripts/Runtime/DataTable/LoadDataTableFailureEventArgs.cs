//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载数据表失败事件。
    /// </summary>
    public sealed class LoadDataTableFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载数据表失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDataTableFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载数据表失败事件的新实例。
        /// </summary>
        public LoadDataTableFailureEventArgs()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取加载数据表失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public Type DataRowType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string DataTableName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表加载方式。
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载数据表失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载数据表失败事件。</returns>
        public static LoadDataTableFailureEventArgs Create(GameFramework.DataTable.LoadDataTableFailureEventArgs e)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)e.UserData;
            LoadDataTableFailureEventArgs loadDataTableFailureEventArgs = ReferencePool.Acquire<LoadDataTableFailureEventArgs>();
            loadDataTableFailureEventArgs.DataRowType = loadDataTableInfo.DataRowType;
            loadDataTableFailureEventArgs.DataTableName = loadDataTableInfo.DataTableName;
            loadDataTableFailureEventArgs.DataTableAssetName = e.DataTableAssetName;
            loadDataTableFailureEventArgs.LoadType = e.LoadType;
            loadDataTableFailureEventArgs.ErrorMessage = e.ErrorMessage;
            loadDataTableFailureEventArgs.UserData = loadDataTableInfo.UserData;
            ReferencePool.Release(loadDataTableInfo);
            return loadDataTableFailureEventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件。
        /// </summary>
        public override void Clear()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            LoadType = LoadType.Text;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
