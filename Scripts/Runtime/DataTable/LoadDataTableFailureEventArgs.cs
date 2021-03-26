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
            DataTableAssetName = null;
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
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
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
        public static LoadDataTableFailureEventArgs Create(ReadDataFailureEventArgs e)
        {
            LoadDataTableFailureEventArgs loadDataTableFailureEventArgs = ReferencePool.Acquire<LoadDataTableFailureEventArgs>();
            loadDataTableFailureEventArgs.DataTableAssetName = e.DataAssetName;
            loadDataTableFailureEventArgs.ErrorMessage = e.ErrorMessage;
            loadDataTableFailureEventArgs.UserData = e.UserData;
            return loadDataTableFailureEventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件。
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
