namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class StringProcessor : GenericDataProcessor<string>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "string";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "string",
                    "system.string"
                };
            }

            public override string Parse(string value)
            {
                return value;
            }

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
