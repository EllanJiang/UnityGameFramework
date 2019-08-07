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
    /// Web 请求开始事件。
    /// </summary>
    public sealed class WebRequestStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// Web 请求开始事件编号。
        /// </summary>
        public static readonly int EventId = typeof(WebRequestStartEventArgs).GetHashCode();

        /// <summary>
        /// 初始化 Web 请求开始事件的新实例。
        /// </summary>
        public WebRequestStartEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }

        /// <summary>
        /// 获取 Web 请求开始事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取 Web 请求任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 请求地址。
        /// </summary>
        public string WebRequestUri
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
        /// 创建 Web 请求开始事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的 Web 请求开始事件。</returns>
        public static WebRequestStartEventArgs Create(GameFramework.WebRequest.WebRequestStartEventArgs e)
        {
            WWWFormInfo wwwFormInfo = (WWWFormInfo)e.UserData;
            WebRequestStartEventArgs webRequestStartEventArgs = ReferencePool.Acquire<WebRequestStartEventArgs>();
            webRequestStartEventArgs.SerialId = e.SerialId;
            webRequestStartEventArgs.WebRequestUri = e.WebRequestUri;
            webRequestStartEventArgs.UserData = wwwFormInfo.UserData;
            return webRequestStartEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求开始事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }
    }
}
