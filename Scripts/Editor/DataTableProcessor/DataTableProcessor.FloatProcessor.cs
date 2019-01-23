namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class FloatProcessor : GenericDataProcessor<float>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "float";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "float",
                    "single",
                    "system.single"
                };
            }

            public override float Parse(string value)
            {
                return float.Parse(value);
            }

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
