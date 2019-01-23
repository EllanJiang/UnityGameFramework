namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class DataColumnInfo
        {
            private readonly string m_Name;
            private readonly bool m_IdColumn;
            private readonly string m_DefaultValue;
            private readonly string m_Comment;
            private readonly DataProcessor m_DataProcessor;

            public DataColumnInfo(string comment)
                : this("Id", "int", true, null, comment)
            {

            }

            public DataColumnInfo(string name, string type)
                : this(name, type, false, null, null)
            {

            }

            public DataColumnInfo(string name, string type, string comment)
                : this(name, type, false, null, comment)
            {

            }

            public DataColumnInfo(string name, string type, string defaultValue, string comment)
                : this(name, type, false, defaultValue, comment)
            {

            }

            private DataColumnInfo(string name, string type, bool idColumn, string defaultValue, string comment)
            {
                m_Name = name;
                m_DataProcessor = DataProcessorUtility.GetDataProcessor(type);
                m_IdColumn = idColumn;
                m_DefaultValue = defaultValue;
                m_Comment = comment;
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public bool ValidColumn
            {
                get
                {
                    return !string.IsNullOrEmpty(m_Name) && !m_DataProcessor.IsComment;
                }
            }

            public bool IdColumn
            {
                get
                {
                    return m_IdColumn;
                }
            }

            public string DefaultValue
            {
                get
                {
                    return m_DefaultValue;
                }
            }

            public string Comment
            {
                get
                {
                    return m_Comment;
                }
            }

            public DataProcessor DataProcessor
            {
                get
                {
                    return m_DataProcessor;
                }
            }
        }
    }
}
