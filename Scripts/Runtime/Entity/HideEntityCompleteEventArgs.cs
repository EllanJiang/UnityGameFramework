//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Entity;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 隐藏实体完成事件。
    /// </summary>
    public sealed class HideEntityCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 隐藏实体完成事件编号。
        /// </summary>
        public static readonly int EventId = typeof(HideEntityCompleteEventArgs).GetHashCode();

        /// <summary>
        /// 初始化隐藏实体完成事件的新实例。
        /// </summary>
        public HideEntityCompleteEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }

        /// <summary>
        /// 获取隐藏实体完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup
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
        /// 创建隐藏实体完成事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的隐藏实体完成事件。</returns>
        public static HideEntityCompleteEventArgs Create(GameFramework.Entity.HideEntityCompleteEventArgs e)
        {
            HideEntityCompleteEventArgs hideEntityCompleteEventArgs = ReferencePool.Acquire<HideEntityCompleteEventArgs>();
            hideEntityCompleteEventArgs.EntityId = e.EntityId;
            hideEntityCompleteEventArgs.EntityAssetName = e.EntityAssetName;
            hideEntityCompleteEventArgs.EntityGroup = e.EntityGroup;
            hideEntityCompleteEventArgs.UserData = e.UserData;
            return hideEntityCompleteEventArgs;
        }

        /// <summary>
        /// 清理隐藏实体完成事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }
    }
}
