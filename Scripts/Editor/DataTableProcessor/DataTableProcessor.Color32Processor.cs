using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Color32Processor : GenericDataProcessor<Color32>
        {
            public override string StandardTypeString
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

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {
                Color32 color32 = Parse(value);
                stream.Write(color32.r);
                stream.Write(color32.g);
                stream.Write(color32.b);
                stream.Write(color32.a);
            }
        }
    }
}
