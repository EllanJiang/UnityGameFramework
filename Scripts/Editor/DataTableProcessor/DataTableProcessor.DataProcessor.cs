using System.IO;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        public abstract class DataProcessor
        {
            public abstract System.Type Type
            {
                get;
            }

            public abstract bool IsComment
            {
                get;
            }

            public abstract string StandardTypeString
            {
                get;
            }

            public abstract string[] GetTypeStrings();

            public abstract void WriteToStream(BinaryWriter stream, string value);
        }
    }
}
