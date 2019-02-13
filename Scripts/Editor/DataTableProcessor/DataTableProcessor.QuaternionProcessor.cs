//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class QuaternionProcessor : GenericDataProcessor<Quaternion>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "Quaternion";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "quaternion",
                    "unityengine.quaternion"
                };
            }

            public override Quaternion Parse(string value)
            {
                string[] splitValue = value.Split(',');
                return new Quaternion(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                Quaternion quaternion = Parse(value);
                stream.Write(quaternion.x);
                stream.Write(quaternion.y);
                stream.Write(quaternion.z);
                stream.Write(quaternion.w);
            }
        }
    }
}
