namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class CommentProcessor : DataProcessor
        {
            public override System.Type Type
            {
                get
                {
                    return typeof(CommentProcessor);
                }
            }

            public override bool IsComment
            {
                get
                {
                    return true;
                }
            }

            public override string StandardTypeString
            {
                get
                {
                    return null;
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    string.Empty,
                    "#",
                    "comment"
                };
            }

            public override void WriteToStream(DataTableBinaryWriter stream, string value)
            {

            }
        }
    }
}
