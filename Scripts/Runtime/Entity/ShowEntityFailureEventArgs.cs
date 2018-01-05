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
    /// 显示实体失败事件。
    /// </summary>
    public sealed class ShowEntityFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示实体失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowEntityFailureEventArgs).GetHashCode();

        /// <summary>
        /// 获取显示实体失败事件编号。
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
        /// 清理显示实体失败事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = default(int);
            EntityLogicType = default(Type);
            EntityAssetName = default(string);
            EntityGroupName = default(string);
            ErrorMessage = default(string);
            UserData = default(object);
        }

        /// <summary>
        /// 填充显示实体失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>显示实体失败事件。</returns>
        public ShowEntityFailureEventArgs Fill(GameFramework.Entity.ShowEntityFailureEventArgs e)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)e.UserData;
            EntityId = e.EntityId;
            EntityLogicType = showEntityInfo.EntityLogicType;
            EntityAssetName = e.EntityAssetName;
            EntityGroupName = e.EntityGroupName;
            ErrorMessage = e.ErrorMessage;
            UserData = showEntityInfo.UserData;

            return this;
        }
    }
}
