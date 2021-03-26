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
    /// 加载字典更新事件。
    /// </summary>
    public sealed class LoadDictionaryUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载字典更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDictionaryUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载字典更新事件的新实例。
        /// </summary>
        public LoadDictionaryUpdateEventArgs()
        {
            DictionaryAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载字典更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取字典资源名称。
        /// </summary>
        public string DictionaryAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载字典进度。
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
        /// 创建加载字典更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载字典更新事件。</returns>
        public static LoadDictionaryUpdateEventArgs Create(ReadDataUpdateEventArgs e)
        {
            LoadDictionaryUpdateEventArgs loadDictionaryUpdateEventArgs = ReferencePool.Acquire<LoadDictionaryUpdateEventArgs>();
            loadDictionaryUpdateEventArgs.DictionaryAssetName = e.DataAssetName;
            loadDictionaryUpdateEventArgs.Progress = e.Progress;
            loadDictionaryUpdateEventArgs.UserData = e.UserData;
            return loadDictionaryUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载字典更新事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
