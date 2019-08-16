//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 界面逻辑基类。
    /// </summary>
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool m_Available = false;
        private bool m_Visible = false;
        private int m_OriginalLayer = 0;

        /// <summary>
        /// 获取界面。
        /// </summary>
        public UIForm UIForm
        {
            get
            {
                return GetComponent<UIForm>();
            }
        }

        /// <summary>
        /// 获取或设置界面名称。
        /// </summary>
        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;
            }
        }

        /// <summary>
        /// 获取界面是否可用。
        /// </summary>
        public bool Available
        {
            get
            {
                return m_Available;
            }
        }

        /// <summary>
        /// 获取或设置界面是否可见。
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Available && m_Visible;
            }
            set
            {
                if (!m_Available)
                {
                    Log.Warning("UI form '{0}' is not available.", Name);
                    return;
                }

                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 获取已缓存的 Transform。
        /// </summary>
        public Transform CachedTransform
        {
            get;
            private set;
        }

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnInit(object userData)
        {
            if (CachedTransform == null)
            {
                CachedTransform = transform;
            }

            m_OriginalLayer = gameObject.layer;
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnOpen(object userData)
        {
            m_Available = true;
            Visible = true;
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnClose(bool isShutdown, object userData)
        {
            gameObject.SetLayerRecursively(m_OriginalLayer);
            Visible = false;
            m_Available = false;
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        protected internal virtual void OnPause()
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        protected internal virtual void OnResume()
        {
            Visible = true;
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        /// <summary>
        /// 设置界面的可见性。
        /// </summary>
        /// <param name="visible">界面的可见性。</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
