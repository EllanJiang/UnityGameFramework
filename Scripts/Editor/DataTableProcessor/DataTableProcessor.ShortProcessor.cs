using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class ShortProcessor : GenericDataProcessor<short>
        {
            public override string LanguageKeyword
            {
                get
                {
                    return "short";
                }
            }

            public override string TypeName
            {
                get
                {
                    return "Int16";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "short",
                    "int16",
                    "system.int16"
                };
            }

            public override short Parse(string value)
            {
                return short.Parse(value);
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
