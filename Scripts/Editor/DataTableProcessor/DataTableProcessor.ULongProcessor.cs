using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class ULongProcessor : GenericDataProcessor<ulong>
        {
            public override string LanguageKeyword
            {
                get
                {
                    return "ulong";
                }
            }

            public override string TypeName
            {
                get
                {
                    return "UInt64";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "ulong",
                    "uint64",
                    "system.uint64"
                };
            }

            public override ulong Parse(string value)
            {
                return ulong.Parse(value);
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
