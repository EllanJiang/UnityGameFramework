using System.IO;
using System.Text;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        public sealed class DataTableBinaryWriter : BinaryWriter
        {
            public DataTableBinaryWriter(Stream output)
                : base(output)
            {

            }

            public DataTableBinaryWriter(Stream output, Encoding encoding)
                : base(output, encoding)
            {

            }
        }
    }
}
