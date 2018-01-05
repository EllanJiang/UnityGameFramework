//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Network;
using GameFramework.Procedure;
using System;
using System.Collections.Generic;

namespace UnityGameFramework.Runtime
{
    internal static class AvoidJIT
    {
        private static void NeverCalledMethod()
        {
            new Dictionary<int, EventHandler<GameEventArgs>>();
            new Dictionary<int, EventHandler<Packet>>();
            new Dictionary<int, FsmEventHandler<IProcedureManager>>();
        }
    }
}
