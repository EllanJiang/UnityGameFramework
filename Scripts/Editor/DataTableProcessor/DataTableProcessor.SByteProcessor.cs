using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class SByteProcessor : GenericDataProcessor<sbyte>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "sbyte";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "sbyte",
                    "system.sbyte"
                };
            }

            public override sbyte Parse(string value)
            {
                return sbyte.Parse(value);
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
