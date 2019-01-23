namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class DoubleProcessor : GenericDataProcessor<double>
        {
            public override string StandardTypeString
            {
                get
                {
                    return "double";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "double",
                    "system.double"
                };
            }

            public override double Parse(string value)
            {
                return double.Parse(value);
            }

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {
                stream.Write(Parse(value));
            }
        }
    }
}
