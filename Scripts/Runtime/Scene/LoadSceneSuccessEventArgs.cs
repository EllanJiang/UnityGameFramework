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
    /// 加载场景成功事件。
    /// </summary>
    public sealed class LoadSceneSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载场景成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadSceneSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载场景成功事件的新实例。
        /// </summary>
        public LoadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取加载场景成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
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
        /// 创建加载场景成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载场景成功事件。</returns>
        public static LoadSceneSuccessEventArgs Create(GameFramework.Scene.LoadSceneSuccessEventArgs e)
        {
            LoadSceneSuccessEventArgs loadSceneSuccessEventArgs = ReferencePool.Acquire<LoadSceneSuccessEventArgs>();
            loadSceneSuccessEventArgs.SceneAssetName = e.SceneAssetName;
            loadSceneSuccessEventArgs.Duration = e.Duration;
            loadSceneSuccessEventArgs.UserData = e.UserData;
            return loadSceneSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载场景成功事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
