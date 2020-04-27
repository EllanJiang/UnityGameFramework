//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载字典失败事件。
    /// </summary>
    public sealed class LoadDictionaryFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载字典失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDictionaryFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载字典失败事件的新实例。
        /// </summary>
        public LoadDictionaryFailureEventArgs()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取加载字典失败事件编号。
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
        /// 创建加载字典失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载字典失败事件。</returns>
        public static LoadDictionaryFailureEventArgs Create(GameFramework.Localization.LoadDictionaryFailureEventArgs e)
        {
            LoadDictionaryInfo loadDictionaryInfo = (LoadDictionaryInfo)e.UserData;
            LoadDictionaryFailureEventArgs loadDictionaryFailureEventArgs = ReferencePool.Acquire<LoadDictionaryFailureEventArgs>();
            loadDictionaryFailureEventArgs.DictionaryName = loadDictionaryInfo.DictionaryName;
            loadDictionaryFailureEventArgs.DictionaryAssetName = e.DictionaryAssetName;
            loadDictionaryFailureEventArgs.ErrorMessage = e.ErrorMessage;
            loadDictionaryFailureEventArgs.UserData = loadDictionaryInfo.UserData;
            ReferencePool.Release(loadDictionaryInfo);
            return loadDictionaryFailureEventArgs;
        }

        /// <summary>
        /// 清理加载字典失败事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryName = null;
            DictionaryAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
