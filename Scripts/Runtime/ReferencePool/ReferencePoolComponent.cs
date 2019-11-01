//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 基础组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/ReferencePool")]
    public sealed class ReferencePoolComponent : GameFrameworkComponent
    {
        [SerializeField]
        private ReferenceStrictCheckType m_StrictCheck = ReferenceStrictCheckType.AlwaysOpen;

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            switch (m_StrictCheck)
            {
                case ReferenceStrictCheckType.AlwaysOpen:
                    EnableStrictCheck = true;
                    break;
                case ReferenceStrictCheckType.OnlyWhenDevelopment:
                    EnableStrictCheck = Debug.isDebugBuild;
                    break;
                case ReferenceStrictCheckType.OnlyInEditor:
                    EnableStrictCheck = Application.isEditor;
                    break;
                default:
                    EnableStrictCheck = false;
                    break;
            }
        }

        /// <summary>
        /// 获取或设置是否开启强制检查。
        /// </summary>
        public bool EnableStrictCheck
        {
            get
            {
                return ReferencePool.EnableStrictCheck;
            }
            set
            {
                ReferencePool.EnableStrictCheck = value;
                if (value)
                {
                    Log.Info("Strict checking is enabled for the Reference Pool. It will drastically affect the performance.");
                }
            }
        }
    }
}
