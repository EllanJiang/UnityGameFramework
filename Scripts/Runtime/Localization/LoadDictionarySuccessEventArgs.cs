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
    /// 加载字典成功事件。
    /// </summary>
    public sealed class LoadDictionarySuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载字典成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDictionarySuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载字典成功事件的新实例。
        /// </summary>
        public LoadDictionarySuccessEventArgs()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载字典成功事件编号。
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
        /// 创建加载字典成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载字典成功事件。</returns>
        public static LoadDictionarySuccessEventArgs Create(GameFramework.Localization.LoadDictionarySuccessEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            LoadDictionarySuccessEventArgs loadDictionarySuccessEventArgs = ReferencePool.Acquire<LoadDictionarySuccessEventArgs>();
            loadDictionarySuccessEventArgs.DictionaryName = loadDictionaryInfo.DictionaryName;
            loadDictionarySuccessEventArgs.DictionaryAssetName = e.DictionaryAssetName;
            loadDictionarySuccessEventArgs.LoadType = e.LoadType;
            loadDictionarySuccessEventArgs.Duration = e.Duration;
            loadDictionarySuccessEventArgs.UserData = loadDictionaryInfo.UserData;
            ReferencePool.Release(loadDictionaryInfo);
            return loadDictionarySuccessEventArgs;
        }

        /// <summary>
        /// 清理加载字典成功事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }
    }
}
