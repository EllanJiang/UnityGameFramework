//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 激活场景被改变事件。
    /// </summary>
    public sealed class ActiveSceneChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 激活场景被改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ActiveSceneChangedEventArgs).GetHashCode();

        /// <summary>
        /// 初始化激活场景被改变事件的新实例。
        /// </summary>
        public ActiveSceneChangedEventArgs()
        {
            LastActiveScene = default(Scene);
            ActiveScene = default(Scene);
        }

        /// <summary>
        /// 获取激活场景被改变事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取上一个被激活的场景。
        /// </summary>
        public Scene LastActiveScene
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被激活的场景。
        /// </summary>
        public Scene ActiveScene
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建激活场景被改变事件。
        /// </summary>
        /// <param name="lastActiveScene">上一个被激活的场景。</param>
        /// <param name="activeScene">被激活的场景。</param>
        /// <returns>创建的激活场景被改变事件。</returns>
        public static ActiveSceneChangedEventArgs Create(Scene lastActiveScene, Scene activeScene)
        {
            ActiveSceneChangedEventArgs activeSceneChangedEventArgs = ReferencePool.Acquire<ActiveSceneChangedEventArgs>();
            activeSceneChangedEventArgs.LastActiveScene = lastActiveScene;
            activeSceneChangedEventArgs.ActiveScene = activeScene;
            return activeSceneChangedEventArgs;
        }

        /// <summary>
        /// 清理激活场景被改变事件。
        /// </summary>
        public override void Clear()
        {
            LastActiveScene = default(Scene);
            ActiveScene = default(Scene);
        }
    }
}
