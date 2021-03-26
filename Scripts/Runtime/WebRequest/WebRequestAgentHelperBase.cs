﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.WebRequest;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求代理辅助器基类。
    /// </summary>
    public abstract class WebRequestAgentHelperBase : MonoBehaviour, IWebRequestAgentHelper
    {
        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public abstract event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete;

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        public abstract event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError;

        /// <summary>
        /// 通过 Web 请求代理辅助器发送 Web 请求。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Request(string webRequestUri, object userData);

        /// <summary>
        /// 通过 Web 请求代理辅助器发送 Web 请求。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Request(string webRequestUri, byte[] postData, object userData);

        /// <summary>
        /// 重置 Web 请求代理辅助器。
        /// </summary>
        public abstract void Reset();
    }
}
