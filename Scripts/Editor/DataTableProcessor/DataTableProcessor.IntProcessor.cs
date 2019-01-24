using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class IntProcessor : GenericDataProcessor<int>
        {
            public override string LanguageKeyword
            {
                get
                {
                    return "int";
                }
            }

            public override string TypeName
            {
                get
                {
                    return "Int32";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "int",
                    "int32",
                    "system.int32"
                };
            }

            public override int Parse(string value)
            {
                return int.Parse(value);
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
