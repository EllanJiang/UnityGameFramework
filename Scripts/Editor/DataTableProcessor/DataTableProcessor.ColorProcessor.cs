using System.IO;
using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class ColorProcessor : GenericDataProcessor<Color>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "Color";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "color",
                    "unityengine.color"
                };
            }

            public override Color Parse(string value)
            {
                string[] splitValue = value.Split(',');
                return new Color(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                Color color = Parse(value);
                stream.Write(color.r);
                stream.Write(color.g);
                stream.Write(color.b);
                stream.Write(color.a);
            }
        }
    }
}
