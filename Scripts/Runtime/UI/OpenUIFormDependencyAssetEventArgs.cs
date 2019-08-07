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
    /// 打开界面时加载依赖资源事件。
    /// </summary>
    public sealed class OpenUIFormDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 打开界面时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(OpenUIFormDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化打开界面时加载依赖资源事件的新实例。
        /// </summary>
        public OpenUIFormDependencyAssetEventArgs()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取打开界面时加载依赖资源事件编号。
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
        /// 创建打开界面时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的打开界面时加载依赖资源事件。</returns>
        public static OpenUIFormDependencyAssetEventArgs Create(GameFramework.UI.OpenUIFormDependencyAssetEventArgs e)
        {
            OpenUIFormDependencyAssetEventArgs openUIFormDependencyAssetEventArgs = ReferencePool.Acquire<OpenUIFormDependencyAssetEventArgs>();
            openUIFormDependencyAssetEventArgs.SerialId = e.SerialId;
            openUIFormDependencyAssetEventArgs.UIFormAssetName = e.UIFormAssetName;
            openUIFormDependencyAssetEventArgs.UIGroupName = e.UIGroupName;
            openUIFormDependencyAssetEventArgs.PauseCoveredUIForm = e.PauseCoveredUIForm;
            openUIFormDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            openUIFormDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            openUIFormDependencyAssetEventArgs.TotalCount = e.TotalCount;
            openUIFormDependencyAssetEventArgs.UserData = e.UserData;
            return openUIFormDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理打开界面时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            UIFormAssetName = null;
            UIGroupName = null;
            PauseCoveredUIForm = false;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
