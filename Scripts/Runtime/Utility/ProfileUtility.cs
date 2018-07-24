//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Diagnostics;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 性能分析工具集。
    /// </summary>
    public static class ProfileUtility
    {
        /// <summary>
        /// 开始采样。
        /// </summary>
        /// <param name="name">采样名称。</param>
        [Conditional("ENABLE_PROFILER")]
        public static void BeginSample(string name)
        {
            Utility.Profiler.BeginSample(name);
        }

        /// <summary>
        /// 结束采样。
        /// </summary>
        [Conditional("ENABLE_PROFILER")]
        public static void EndSample()
        {
            Utility.Profiler.EndSample();
        }
    }
}
