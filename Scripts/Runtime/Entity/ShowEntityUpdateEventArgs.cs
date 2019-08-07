//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示实体更新事件。
    /// </summary>
    public sealed class ShowEntityUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowEntityUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示实体更新事件的新实例。
        /// </summary>
        public ShowEntityUpdateEventArgs()
        {
            EntityId = 0;
            EntityLogicType = null;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取显示实体更新事件编号。
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
        /// 获取实体逻辑类型。
        /// </summary>
        public Type EntityLogicType
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
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取显示实体进度。
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
        /// 创建显示实体更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示实体更新事件。</returns>
        public static ShowEntityUpdateEventArgs Create(GameFramework.Entity.ShowEntityUpdateEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            ShowEntityUpdateEventArgs showEntityUpdateEventArgs = ReferencePool.Acquire<ShowEntityUpdateEventArgs>();
            showEntityUpdateEventArgs.EntityId = e.EntityId;
            showEntityUpdateEventArgs.EntityLogicType = showEntityInfo.EntityLogicType;
            showEntityUpdateEventArgs.EntityAssetName = e.EntityAssetName;
            showEntityUpdateEventArgs.EntityGroupName = e.EntityGroupName;
            showEntityUpdateEventArgs.Progress = e.Progress;
            showEntityUpdateEventArgs.UserData = showEntityInfo.UserData;
            return showEntityUpdateEventArgs;
        }

        /// <summary>
        /// 清理显示实体更新事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityLogicType = null;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
