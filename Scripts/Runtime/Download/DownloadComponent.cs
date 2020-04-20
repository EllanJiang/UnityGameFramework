//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Download;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 下载组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Download")]
    public sealed class DownloadComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;
        private const int OneMegaBytes = 1024 * 1024;

        private IDownloadManager m_DownloadManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_DownloadAgentHelperTypeName = "UnityGameFramework.Runtime.UnityWebRequestDownloadAgentHelper";

        [SerializeField]
        private DownloadAgentHelperBase m_CustomDownloadAgentHelper = null;

        [SerializeField]
        private int m_DownloadAgentHelperCount = 3;

        [SerializeField]
        private float m_Timeout = 30f;

        [SerializeField]
        private int m_FlushSize = OneMegaBytes;

        /// <summary>
        /// 获取或设置下载是否被暂停。
        /// </summary>
        public bool Paused
        {
            get
            {
                return m_DownloadManager.Paused;
            }
            set
            {
                m_DownloadManager.Paused = value;
            }
        }

        /// <summary>
        /// 获取下载代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_DownloadManager.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用下载代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_DownloadManager.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中下载代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_DownloadManager.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待下载任务数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_DownloadManager.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置下载超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get
            {
                return m_DownloadManager.Timeout;
            }
            set
            {
                m_DownloadManager.Timeout = m_Timeout = value;
            }
        }

        /// <summary>
        /// 获取或设置将缓冲区写入磁盘的临界大小，仅当开启断点续传时有效。
        /// </summary>
        public int FlushSize
        {
            get
            {
                return m_DownloadManager.FlushSize;
            }
            set
            {
                m_DownloadManager.FlushSize = m_FlushSize = value;
            }
        }

        /// <summary>
        /// 获取当前下载速度。
        /// </summary>
        public float CurrentSpeed
        {
            get
            {
                return m_DownloadManager.CurrentSpeed;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_DownloadManager = GameFrameworkEntry.GetModule<IDownloadManager>();
            if (m_DownloadManager == null)
            {
                Log.Fatal("Download manager is invalid.");
                return;
            }

            m_DownloadManager.DownloadStart += OnDownloadStart;
            m_DownloadManager.DownloadUpdate += OnDownloadUpdate;
            m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
            m_DownloadManager.DownloadFailure += OnDownloadFailure;
            m_DownloadManager.FlushSize = m_FlushSize;
            m_DownloadManager.Timeout = m_Timeout;
        }

        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("Download Agent Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_DownloadAgentHelperCount; i++)
            {
                AddDownloadAgentHelper(i);
            }
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri)
        {
            return AddDownload(downloadPath, downloadUri, DefaultPriority, null);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, int priority)
        {
            return AddDownload(downloadPath, downloadUri, priority, null);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, object userData)
        {
            return AddDownload(downloadPath, downloadUri, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        public int AddDownload(string downloadPath, string downloadUri, int priority, object userData)
        {
            return m_DownloadManager.AddDownload(downloadPath, downloadUri, priority, userData);
        }

        /// <summary>
        /// 移除下载任务。
        /// </summary>
        /// <param name="serialId">要移除下载任务的序列编号。</param>
        public void RemoveDownload(int serialId)
        {
            m_DownloadManager.RemoveDownload(serialId);
        }

        /// <summary>
        /// 移除所有下载任务。
        /// </summary>
        public void RemoveAllDownloads()
        {
            m_DownloadManager.RemoveAllDownloads();
        }

        /// <summary>
        /// 获取所有下载任务的信息。
        /// </summary>
        /// <returns>所有下载任务的信息。</returns>
        public TaskInfo[] GetAllDownloadInfos()
        {
            return m_DownloadManager.GetAllDownloadInfos();
        }

        /// <summary>
        /// 增加下载代理辅助器。
        /// </summary>
        /// <param name="index">下载代理辅助器索引。</param>
        private void AddDownloadAgentHelper(int index)
        {
            DownloadAgentHelperBase downloadAgentHelper = Helper.CreateHelper(m_DownloadAgentHelperTypeName, m_CustomDownloadAgentHelper, index);
            if (downloadAgentHelper == null)
            {
                Log.Error("Can not create download agent helper.");
                return;
            }

            downloadAgentHelper.name = Utility.Text.Format("Download Agent Helper - {0}", index.ToString());
            Transform transform = downloadAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_DownloadManager.AddDownloadAgentHelper(downloadAgentHelper);
        }

        private void OnDownloadStart(object sender, GameFramework.Download.DownloadStartEventArgs e)
        {
            m_EventComponent.Fire(this, DownloadStartEventArgs.Create(e));
        }

        private void OnDownloadUpdate(object sender, GameFramework.Download.DownloadUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, DownloadUpdateEventArgs.Create(e));
        }

        private void OnDownloadSuccess(object sender, GameFramework.Download.DownloadSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, DownloadSuccessEventArgs.Create(e));
        }

        private void OnDownloadFailure(object sender, GameFramework.Download.DownloadFailureEventArgs e)
        {
            Log.Warning("Download failure, download serial id '{0}', download path '{1}', download uri '{2}', error message '{3}'.", e.SerialId.ToString(), e.DownloadPath, e.DownloadUri, e.ErrorMessage);
            m_EventComponent.Fire(this, DownloadFailureEventArgs.Create(e));
        }
    }
}
