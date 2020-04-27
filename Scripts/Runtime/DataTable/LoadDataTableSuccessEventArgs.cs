//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载数据表成功事件。
    /// </summary>
    public sealed class LoadDataTableSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载数据表成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDataTableSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载数据表成功事件的新实例。
        /// </summary>
        public LoadDataTableSuccessEventArgs()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载数据表成功事件编号。
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
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建加载数据表成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载数据表成功事件。</returns>
        public static LoadDataTableSuccessEventArgs Create(GameFramework.DataTable.LoadDataTableSuccessEventArgs e)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)e.UserData;
            LoadDataTableSuccessEventArgs loadDataTableSuccessEventArgs = ReferencePool.Acquire<LoadDataTableSuccessEventArgs>();
            loadDataTableSuccessEventArgs.DataRowType = loadDataTableInfo.DataRowType;
            loadDataTableSuccessEventArgs.DataTableName = loadDataTableInfo.DataTableName;
            loadDataTableSuccessEventArgs.DataTableAssetName = e.DataTableAssetName;
            loadDataTableSuccessEventArgs.Duration = e.Duration;
            loadDataTableSuccessEventArgs.UserData = loadDataTableInfo.UserData;
            ReferencePool.Release(loadDataTableInfo);
            return loadDataTableSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载数据表成功事件。
        /// </summary>
        public override void Clear()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
