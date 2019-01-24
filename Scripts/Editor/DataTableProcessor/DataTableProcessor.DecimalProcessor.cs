using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class DecimalProcessor : GenericDataProcessor<decimal>
        {
            public override string LanguageKeyword
            {
                get
                {
                    return "decimal";
                }
            }

            public override string TypeName
            {
                get
                {
                    return "Decimal";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "decimal",
                    "system.decimal"
                };
            }

            public override decimal Parse(string value)
            {
                return decimal.Parse(value);
            }

            public override void WriteToStream(BinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
