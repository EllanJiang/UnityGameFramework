//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求失败事件。
    /// </summary>
    public sealed class WebRequestFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// Web 请求失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(WebRequestFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化 Web 请求失败事件的新实例。
        /// </summary>
        public WebRequestFailureEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取 Web 请求失败事件编号。
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
        /// 创建 Web 请求失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的 Web 请求失败事件。</returns>
        public static WebRequestFailureEventArgs Create(GameFramework.WebRequest.WebRequestFailureEventArgs e)
        {
            WWWFormInfo wwwFormInfo = (WWWFormInfo)e.UserData;
            WebRequestFailureEventArgs webRequestFailureEventArgs = ReferencePool.Acquire<WebRequestFailureEventArgs>();
            webRequestFailureEventArgs.SerialId = e.SerialId;
            webRequestFailureEventArgs.WebRequestUri = e.WebRequestUri;
            webRequestFailureEventArgs.ErrorMessage = e.ErrorMessage;
            webRequestFailureEventArgs.UserData = wwwFormInfo.UserData;
            ReferencePool.Release(wwwFormInfo);
            return webRequestFailureEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求失败事件。
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
