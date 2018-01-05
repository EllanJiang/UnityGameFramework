//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示实体成功事件。
    /// </summary>
    public sealed class ShowEntitySuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowEntitySuccessEventArgs).GetHashCode();

        /// <summary>
        /// 获取显示实体成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
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
        /// 获取显示成功的实体。
        /// </summary>
        public Entity Entity
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
        /// 清理显示实体成功事件。
        /// </summary>
        public override void Clear()
        {
            EntityLogicType = default(Type);
            Entity = default(Entity);
            Duration = default(float);
            UserData = default(object);
        }

        /// <summary>
        /// 填充显示实体成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>显示实体成功事件。</returns>
        public ShowEntitySuccessEventArgs Fill(GameFramework.Entity.ShowEntitySuccessEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            EntityLogicType = showEntityInfo.EntityLogicType;
            Entity = (Entity)e.Entity;
            Duration = e.Duration;
            UserData = showEntityInfo.UserData;

            return this;
        }
    }
}
