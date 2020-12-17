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
            DictionaryAssetName = null;
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
        /// 获取字典资源名称。
        /// </summary>
        public string DictionaryAssetName
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
        public static LoadDictionarySuccessEventArgs Create(ReadDataSuccessEventArgs e)
        {
            LoadDictionarySuccessEventArgs loadDictionarySuccessEventArgs = ReferencePool.Acquire<LoadDictionarySuccessEventArgs>();
            loadDictionarySuccessEventArgs.DictionaryAssetName = e.DataAssetName;
            loadDictionarySuccessEventArgs.Duration = e.Duration;
            loadDictionarySuccessEventArgs.UserData = e.UserData;
            return loadDictionarySuccessEventArgs;
        }

        /// <summary>
        /// 清理加载字典成功事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
