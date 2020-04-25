//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

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

            public abstract bool IsId
            {
                get;
            }

            public abstract bool IsComment
            {
                get;
            }

            public abstract bool IsSystem
            {
                get;
            }

            public abstract string LanguageKeyword
            {
                get;
            }

            public abstract string[] GetTypeStrings();

            public abstract void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value);
        }
    }
}
