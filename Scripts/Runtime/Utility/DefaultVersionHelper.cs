//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认版本号辅助器。
    /// </summary>
    public class DefaultVersionHelper : Version.IVersionHelper
    {
        /// <summary>
        /// 获取游戏版本号。
        /// </summary>
        public string GameVersion
        {
            get
            {
                return Application.version;
            }
        }

        /// <summary>
        /// 获取内部游戏版本号。
        /// </summary>
        public int InternalGameVersion
        {
            get
            {
                return 0;
            }
        }
    }
}
