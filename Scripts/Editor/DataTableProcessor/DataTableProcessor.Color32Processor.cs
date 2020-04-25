//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Color32Processor : GenericDataProcessor<Color32>
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
                    return "Color32";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "color32",
                    "unityengine.color32"
                };
            }

            public override Color32 Parse(string value)
            {
                string[] splitValue = value.Split(',');
                return new Color32(byte.Parse(splitValue[0]), byte.Parse(splitValue[1]), byte.Parse(splitValue[2]), byte.Parse(splitValue[3]));
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                Color32 color32 = Parse(value);
                binaryWriter.Write(color32.r);
                binaryWriter.Write(color32.g);
                binaryWriter.Write(color32.b);
                binaryWriter.Write(color32.a);
            }
        }
    }
}
