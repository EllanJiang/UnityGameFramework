//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.WebRequest;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Web Request")]
    public sealed class WebRequestComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private IWebRequestManager m_WebRequestManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_WebRequestAgentHelperTypeName = "UnityGameFramework.Runtime.UnityWebRequestAgentHelper";

        [SerializeField]
        private WebRequestAgentHelperBase m_CustomWebRequestAgentHelper = null;

        [SerializeField]
        private int m_WebRequestAgentHelperCount = 1;

        [SerializeField]
        private float m_Timeout = 30f;

        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_WebRequestManager.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_WebRequestManager.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_WebRequestManager.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_WebRequestManager.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get
            {
                return m_WebRequestManager.Timeout;
            }
            set
            {
                m_WebRequestManager.Timeout = m_Timeout = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_WebRequestManager = GameFrameworkEntry.GetModule<IWebRequestManager>();
            if (m_WebRequestManager == null)
            {
                Log.Fatal("Web request manager is invalid.");
                return;
            }

            m_WebRequestManager.Timeout = m_Timeout;
            m_WebRequestManager.WebRequestStart += OnWebRequestStart;
            m_WebRequestManager.WebRequestSuccess += OnWebRequestSuccess;
            m_WebRequestManager.WebRequestFailure += OnWebRequestFailure;
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
                m_InstanceRoot = new GameObject("Web Request Agent Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_WebRequestAgentHelperCount; i++)
            {
                AddWebRequestAgentHelper(i);
            }
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="serialId">要获取信息的 Web 请求任务的序列编号。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return m_WebRequestManager.GetWebRequestInfo(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return m_WebRequestManager.GetWebRequestInfos(tag);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <param name="results">Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(string tag, List<TaskInfo> results)
        {
            m_WebRequestManager.GetAllWebRequestInfos(tag, results);
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <returns>所有 Web 请求任务的信息。</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return m_WebRequestManager.GetAllWebRequestInfos();
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <param name="results">所有 Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            m_WebRequestManager.GetAllWebRequestInfos(results);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri)
        {
            return AddWebRequest(webRequestUri, null, null, null, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            return AddWebRequest(webRequestUri, postData, null, null, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag)
        {
            return AddWebRequest(webRequestUri, null, null, tag, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority)
        {
            return AddWebRequest(webRequestUri, null, null, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, null, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority)
        {
            return AddWebRequest(webRequestUri, postData, null, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, int priority)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, null, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, object userData)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, null, null, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, tag, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, tag, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, tag, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, string tag, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, tag, priority, userData);
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return m_WebRequestManager.RemoveWebRequest(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签移除 Web 请求任务。
        /// </summary>
        /// <param name="tag">要移除 Web 请求任务的标签。</param>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveWebRequests(string tag)
        {
            return m_WebRequestManager.RemoveWebRequests(tag);
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveAllWebRequests()
        {
            return m_WebRequestManager.RemoveAllWebRequests();
        }

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="index">Web 请求代理辅助器索引。</param>
        private void AddWebRequestAgentHelper(int index)
        {
            WebRequestAgentHelperBase webRequestAgentHelper = Helper.CreateHelper(m_WebRequestAgentHelperTypeName, m_CustomWebRequestAgentHelper, index);
            if (webRequestAgentHelper == null)
            {
                Log.Error("Can not create web request agent helper.");
                return;
            }

            webRequestAgentHelper.name = Utility.Text.Format("Web Request Agent Helper - {0}", index);
            Transform transform = webRequestAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_WebRequestManager.AddWebRequestAgentHelper(webRequestAgentHelper);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        private int AddWebRequest(string webRequestUri, byte[] postData, WWWForm wwwForm, string tag, int priority, object userData)
        {
            return m_WebRequestManager.AddWebRequest(webRequestUri, postData, tag, priority, WWWFormInfo.Create(wwwForm, userData));
        }

        private void OnWebRequestStart(object sender, GameFramework.WebRequest.WebRequestStartEventArgs e)
        {
            m_EventComponent.Fire(this, WebRequestStartEventArgs.Create(e));
        }

        private void OnWebRequestSuccess(object sender, GameFramework.WebRequest.WebRequestSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, WebRequestSuccessEventArgs.Create(e));
        }

        private void OnWebRequestFailure(object sender, GameFramework.WebRequest.WebRequestFailureEventArgs e)
        {
            Log.Warning("Web request failure, web request serial id '{0}', web request uri '{1}', error message '{2}'.", e.SerialId, e.WebRequestUri, e.ErrorMessage);
            m_EventComponent.Fire(this, WebRequestFailureEventArgs.Create(e));
        }
    }
}
