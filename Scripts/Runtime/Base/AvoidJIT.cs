//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    internal static class AvoidJIT
    {
        private static void NeverCalledMethod()
        {
            new Dictionary<int, FsmEventHandler<IProcedureManager>>();
        }
    }
}
