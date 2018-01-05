//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载场景失败事件。
    /// </summary>
    public sealed class LoadSceneFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载场景失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadSceneFailureEventArgs).GetHashCode();

        /// <summary>
        /// 获取加载场景失败事件编号。
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
        /// 清理加载场景失败事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = default(string);
            ErrorMessage = default(string);
            UserData = default(object);
        }

        /// <summary>
        /// 填充加载场景失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>加载场景失败事件。</returns>
        public LoadSceneFailureEventArgs Fill(GameFramework.Scene.LoadSceneFailureEventArgs e)
        {
            SceneAssetName = e.SceneAssetName;
            ErrorMessage = e.ErrorMessage;
            UserData = e.UserData;

            return this;
        }
    }
}
