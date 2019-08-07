//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
            DictionaryName = null;
            DictionaryAssetName = null;
            LoadType = LoadType.Text;
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
        /// 获取字典名称。
        /// </summary>
        public string DictionaryName
        {
            get;
            private set;
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
        /// 获取字典加载方式。
        /// </summary>
        public LoadType LoadType
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
        public static LoadDictionaryUpdateEventArgs Create(GameFramework.Localization.LoadDictionaryUpdateEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            LoadDictionaryUpdateEventArgs loadDictionaryUpdateEventArgs = ReferencePool.Acquire<LoadDictionaryUpdateEventArgs>();
            loadDictionaryUpdateEventArgs.DictionaryName = loadDictionaryInfo.DictionaryName;
            loadDictionaryUpdateEventArgs.DictionaryAssetName = e.DictionaryAssetName;
            loadDictionaryUpdateEventArgs.LoadType = e.LoadType;
            loadDictionaryUpdateEventArgs.Progress = e.Progress;
            loadDictionaryUpdateEventArgs.UserData = loadDictionaryInfo.UserData;
            return loadDictionaryUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载字典更新事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
            UserData = null;
        }
    }
}
