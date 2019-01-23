using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class QuaternionProcessor : GenericDataProcessor<Quaternion>
        {
            public override string StandardTypeString
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

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
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
