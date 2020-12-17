//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载数据表更新事件。
    /// </summary>
    public sealed class LoadDataTableUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载数据表更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDataTableUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载数据表更新事件的新实例。
        /// </summary>
        public LoadDataTableUpdateEventArgs()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载数据表更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
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
        /// 获取加载数据表进度。
        /// </summary>
        public float Progress
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
        /// 创建加载数据表更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载数据表更新事件。</returns>
        public static LoadDataTableUpdateEventArgs Create(ReadDataUpdateEventArgs e)
        {
            LoadDataTableUpdateEventArgs loadDataTableUpdateEventArgs = ReferencePool.Acquire<LoadDataTableUpdateEventArgs>();
            loadDataTableUpdateEventArgs.DataTableAssetName = e.DataAssetName;
            loadDataTableUpdateEventArgs.Progress = e.Progress;
            loadDataTableUpdateEventArgs.UserData = e.UserData;
            return loadDataTableUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载数据表更新事件。
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
