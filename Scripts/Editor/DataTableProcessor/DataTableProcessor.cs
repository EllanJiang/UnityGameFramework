using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private const char DataSplitSeparator = '\t';
        private const string CommentLineSeparator = "#";

        private readonly string m_FileName;
        private readonly int m_RawRowCount;
        private readonly int m_RawColumnCount;

        private readonly string[] m_NameRow;
        private readonly string[] m_TypeRow;
        private readonly string[] m_DefaultValueRow;
        private readonly string[] m_CommentRow;
        private readonly int m_ContentStartRow;
        private readonly int m_IdColumn;

        private readonly string[][] m_RawValues;
        private readonly DataColumnInfo[] m_DataColumnInfo;

        private string m_CodeTemplate;
        private DataTableCodeGenerator m_CodeGenerator;

        public DataTableProcessor(string dataTableFileName, Encoding encoding, int nameRow, int typeRow, int? defaultValueRow, int? commentRow, int contentStartRow, int idColumn)
        {
            if (string.IsNullOrEmpty(dataTableFileName))
            {
                throw new GameFrameworkException("Data table file name is invalid.");
            }

            if (!dataTableFileName.EndsWith(".txt"))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data table file '{0}' is not a txt.", dataTableFileName));
            }

            if (!File.Exists(dataTableFileName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data table file '{0}' is not exist.", dataTableFileName));
            }

            m_FileName = dataTableFileName;
            string[] lines = File.ReadAllLines(dataTableFileName, encoding);
            m_RawRowCount = lines.Length;

            List<string[]> rawValues = new List<string[]>();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] rawValue = lines[i].Split(DataSplitSeparator);
                if (i == 0)
                {
                    m_RawColumnCount = rawValue.Length;
                }
                else if (rawValue.Length != m_RawColumnCount)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Raw Column is '{1}', but line '{0}' column is '{2}'.", i.ToString(), m_RawColumnCount.ToString(), rawValue.Length.ToString()));
                }

                rawValues.Add(rawValue);
            }

            m_RawValues = rawValues.ToArray();

            if (nameRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name row '{0}' is invalid.", nameRow.ToString()));
            }

            if (typeRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Type row '{0}' is invalid.", typeRow.ToString()));
            }

            if (contentStartRow < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Content start row '{0}' is invalid.", contentStartRow.ToString()));
            }

            if (idColumn < 0)
            {
                throw new GameFrameworkException(Utility.Text.Format("Id column '{0}' is invalid.", idColumn.ToString()));
            }

            if (nameRow >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Name row '{0}' >= raw row count '{1}' is not allow.", nameRow.ToString(), m_RawRowCount.ToString()));
            }

            if (typeRow >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Type row '{0}' >= raw row count '{1}' is not allow.", typeRow.ToString(), m_RawRowCount.ToString()));
            }

            if (defaultValueRow.HasValue && defaultValueRow.Value >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Default value row '{0}' >= raw row count '{1}' is not allow.", defaultValueRow.Value.ToString(), m_RawRowCount.ToString()));
            }

            if (commentRow.HasValue && commentRow.Value >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Comment row '{0}' >= raw row count '{1}' is not allow.", commentRow.Value.ToString(), m_RawRowCount.ToString()));
            }

            if (contentStartRow > m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Content start row '{0}' > raw row count '{1}' is not allow.", contentStartRow.ToString(), m_RawRowCount.ToString()));
            }

            if (idColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Id column '{0}' >= raw column count '{1}' is not allow.", idColumn.ToString(), m_RawColumnCount.ToString()));
            }

            m_NameRow = m_RawValues[nameRow];
            m_TypeRow = m_RawValues[typeRow];
            m_DefaultValueRow = defaultValueRow.HasValue ? m_RawValues[defaultValueRow.Value] : null;
            m_CommentRow = commentRow.HasValue ? m_RawValues[commentRow.Value] : null;
            m_ContentStartRow = contentStartRow;
            m_IdColumn = idColumn;

            m_DataColumnInfo = new DataColumnInfo[m_RawColumnCount];
            for (int i = 0; i < m_RawColumnCount; i++)
            {
                if (i == m_IdColumn)
                {
                    m_DataColumnInfo[i] = new DataColumnInfo(GetComment(i));
                }
                else
                {
                    m_DataColumnInfo[i] = new DataColumnInfo(GetName(i), GetType(i), GetDefaultValue(i), GetComment(i));
                }
            }

            m_CodeTemplate = null;
            m_CodeGenerator = null;
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
        }

        public int RawRowCount
        {
            get
            {
                return m_RawRowCount;
            }
        }

        public int RawColumnCount
        {
            get
            {
                return m_RawColumnCount;
            }
        }

        public int ContentStartRow
        {
            get
            {
                return m_ContentStartRow;
            }
        }

        public int IdColumn
        {
            get
            {
                return m_IdColumn;
            }
        }

        public string GetName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_NameRow[rawColumn];
        }

        public string GetType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_TypeRow[rawColumn];
        }

        public string GetStandardType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_DataColumnInfo[rawColumn].DataProcessor.StandardTypeString;
        }

        public string GetDefaultValue(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_DefaultValueRow != null ? m_DefaultValueRow[rawColumn] : null;
        }

        public string GetComment(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_CommentRow != null ? m_CommentRow[rawColumn] : null;
        }

        public string GetValue(int rawRow, int rawColumn)
        {
            if (rawRow < 0 || rawRow >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow.ToString()));
            }

            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_RawValues[rawRow][rawColumn];
        }

        public bool IsCommentRow(int rawRow)
        {
            if (rawRow < 0 || rawRow >= m_RawRowCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw row '{0}' is out of range.", rawRow.ToString()));
            }

            return GetValue(rawRow, 0).StartsWith(CommentLineSeparator);
        }

        public bool IsCommentColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= m_RawColumnCount)
            {
                throw new GameFrameworkException(Utility.Text.Format("Raw column '{0}' is out of range.", rawColumn.ToString()));
            }

            return m_DataColumnInfo[rawColumn].DataProcessor.IsComment;
        }

        public bool GenerateDataFile(string outputFileName, Encoding encoding)
        {
            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameFrameworkException("Output file name is invalid.");
            }

            try
            {
                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create))
                {
                    using (DataTableBinaryWriter stream = new DataTableBinaryWriter(fileStream, encoding))
                    {
                        for (int i = m_ContentStartRow; i < m_RawRowCount; i++)
                        {
                            if (IsCommentRow(i))
                            {
                                continue;
                            }

                            for (int j = 0; j < m_RawColumnCount; j++)
                            {
                                if (!m_DataColumnInfo[j].ValidColumn)
                                {
                                    continue;
                                }

                                try
                                {
                                    m_DataColumnInfo[j].DataProcessor.WriteToStream(stream, GetValue(i, j));
                                }
                                catch
                                {
                                    if (string.IsNullOrEmpty(m_DataColumnInfo[j].DefaultValue))
                                    {
                                        Debug.LogError(Utility.Text.Format("Parse raw value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' RawValue='{3}'", outputFileName, i.ToString(), j.ToString(), GetValue(i, j)));
                                        return false;
                                    }
                                    else
                                    {
                                        Debug.LogWarning(Utility.Text.Format("Parse raw value failure, will try default value. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' RawValue='{3}'", outputFileName, i.ToString(), j.ToString(), GetValue(i, j)));
                                        try
                                        {
                                            m_DataColumnInfo[j].DataProcessor.WriteToStream(stream, m_DataColumnInfo[j].DefaultValue);
                                        }
                                        catch
                                        {
                                            Debug.LogError(Utility.Text.Format("Parse default value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' RawValue='{3}'", outputFileName, i.ToString(), j.ToString(), m_DataColumnInfo[j].DefaultValue));
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.Log(Utility.Text.Format("Parse data table '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Parse data table '{0}' failure, exception is '{1}'.", outputFileName, exception.Message));
                return false;
            }
        }

        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                m_CodeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
                Debug.Log(Utility.Text.Format("Set code template '{0}' success.", codeTemplateFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Set code template '{0}' failure, exception is '{1}'.", codeTemplateFileName, exception.Message));
                return false;
            }
        }

        public void SetCodeGenerator(DataTableCodeGenerator codeGenerator)
        {
            m_CodeGenerator = codeGenerator;
        }

        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData)
        {
            if (string.IsNullOrEmpty(m_CodeTemplate))
            {
                throw new GameFrameworkException("You must set code template first.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new GameFrameworkException("Output file name is invalid.");
            }

            try
            {
                StringBuilder stringBuilder = new StringBuilder(m_CodeTemplate);
                if (m_CodeGenerator != null)
                {
                    m_CodeGenerator(this, stringBuilder, userData);
                }

                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create))
                {
                    using (StreamWriter stream = new StreamWriter(fileStream, encoding))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log(Utility.Text.Format("Generate code file '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(Utility.Text.Format("Generate code file '{0}' failure, exception is '{1}'.", outputFileName, exception.Message));
                return false;
            }
        }
    }
}
