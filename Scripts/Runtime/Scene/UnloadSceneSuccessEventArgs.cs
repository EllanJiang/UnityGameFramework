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
    /// 卸载场景成功事件。
    /// </summary>
    public sealed class UnloadSceneSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载场景成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(UnloadSceneSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化卸载场景成功事件的新实例。
        /// </summary>
        public UnloadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
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
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建卸载场景成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的卸载场景成功事件。</returns>
        public static UnloadSceneSuccessEventArgs Create(GameFramework.Scene.UnloadSceneSuccessEventArgs e)
        {
            UnloadSceneSuccessEventArgs unloadSceneSuccessEventArgs = ReferencePool.Acquire<UnloadSceneSuccessEventArgs>();
            unloadSceneSuccessEventArgs.SceneAssetName = e.SceneAssetName;
            unloadSceneSuccessEventArgs.UserData = e.UserData;
            return unloadSceneSuccessEventArgs;
        }

        /// <summary>
        /// 清理卸载场景成功事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            UserData = null;
        }
    }
}
