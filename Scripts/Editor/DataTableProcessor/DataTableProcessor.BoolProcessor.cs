namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class BoolProcessor : GenericDataProcessor<bool>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "bool";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "bool",
                    "boolean",
                    "system.boolean"
                };
            }

            public override bool Parse(string value)
            {
                return bool.Parse(value);
            }

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
