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
    /// 打开界面更新事件。
    /// </summary>
    public sealed class OpenUIFormUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 打开界面更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(OpenUIFormUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化打开界面更新事件的新实例。
        /// </summary>
        public OpenUIFormUpdateEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取打开界面更新事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIFormAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取界面组名称。
        /// </summary>
        public string UIGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        public bool PauseCoveredUIForm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取打开界面进度。
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
        /// 创建打开界面更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的打开界面更新事件。</returns>
        public static OpenUIFormUpdateEventArgs Create(GameFramework.UI.OpenUIFormUpdateEventArgs e)
        {
            OpenUIFormUpdateEventArgs openUIFormUpdateEventArgs = ReferencePool.Acquire<OpenUIFormUpdateEventArgs>();
            openUIFormUpdateEventArgs.SerialId = e.SerialId;
            openUIFormUpdateEventArgs.UIFormAssetName = e.UIFormAssetName;
            openUIFormUpdateEventArgs.UIGroupName = e.UIGroupName;
            openUIFormUpdateEventArgs.PauseCoveredUIForm = e.PauseCoveredUIForm;
            openUIFormUpdateEventArgs.Progress = e.Progress;
            openUIFormUpdateEventArgs.UserData = e.UserData;
            return openUIFormUpdateEventArgs;
        }

        /// <summary>
        /// 清理打开界面更新事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            Progress = 0f;
            UserData = null;
        }
    }
}
