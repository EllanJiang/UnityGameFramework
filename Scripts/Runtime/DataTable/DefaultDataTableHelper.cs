//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认数据表辅助器。
    /// </summary>
    public class DefaultDataTableHelper : DataTableHelperBase
    {
        private static readonly string[] EmptyStringArray = new string[] { };
        private static readonly string BytesAssetExtension = ".bytes";

        private DataTableComponent m_DataTableComponent = null;
        private ResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表行片段。</returns>
        public override GameFrameworkDataSegment[] GetDataRowSegments(object dataTableData)
        {
            try
            {
                string dataTableText = dataTableData as string;
                if (dataTableText != null)
                {
                    List<GameFrameworkDataSegment> dataRowSegments = new List<GameFrameworkDataSegment>();
                    GameFrameworkDataSegment dataRowSegment;
                    int position = 0;
                    while ((dataRowSegment = ReadDataRowSegment(dataTableText, ref position)) != default(GameFrameworkDataSegment))
                    {
                        if (dataTableText[dataRowSegment.Offset] == '#')
                        {
                            continue;
                        }

                        dataRowSegments.Add(dataRowSegment);
                    }

                    return dataRowSegments.ToArray();
                }

                byte[] dataTableBytes = dataTableData as byte[];
                if (dataTableBytes != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(dataTableBytes, false))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                        {
                            int stringsOffset = binaryReader.ReadInt32();
                            int dataRowSegmentCount = binaryReader.Read7BitEncodedInt32();
                            GameFrameworkDataSegment[] dataRowSegments = new GameFrameworkDataSegment[dataRowSegmentCount];
                            for (int i = 0; i < dataRowSegmentCount; i++)
                            {
                                int length = binaryReader.Read7BitEncodedInt32();
                                dataRowSegments[i] = new GameFrameworkDataSegment(dataTableBytes, (int)binaryReader.BaseStream.Position, length);
                                binaryReader.BaseStream.Position += length;
                            }

                            if (binaryReader.BaseStream.Position != stringsOffset)
                            {
                                throw new GameFrameworkException("Verification failure.");
                            }

                            return dataRowSegments;
                        }
                    }
                }

                Log.Warning("Can not get data row segments which type '{0}' is invalid.", dataTableData.GetType().FullName);
                return null;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not get data row segments with exception '{0}'.", exception.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="dataTableData">要解析的数据表数据。</param>
        /// <returns>数据表用户自定义数据。</returns>
        public override object GetDataTableUserData(object dataTableData)
        {
            try
            {
                string dataTableText = dataTableData as string;
                if (dataTableText != null)
                {
                    return null;
                }

                byte[] dataTableBytes = dataTableData as byte[];
                if (dataTableBytes != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(dataTableBytes, false))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                        {
                            binaryReader.BaseStream.Position = binaryReader.ReadInt32();
                            int stringCount = binaryReader.Read7BitEncodedInt32();
                            string[] strings = stringCount > 0 ? new string[stringCount] : EmptyStringArray;
                            for (int i = 0; i < stringCount; i++)
                            {
                                strings[i] = binaryReader.ReadString();
                            }

                            return strings;
                        }
                    }
                }

                Log.Warning("Can not get data table user data which type '{0}' is invalid.", dataTableData.GetType().FullName);
                return null;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not get data table user data with exception '{0}'.", exception.ToString());
                return null;
            }
        }

        /// <summary>
        /// 释放数据表资源。
        /// </summary>
        /// <param name="dataTableAsset">要释放的数据表资源。</param>
        public override void ReleaseDataTableAsset(object dataTableAsset)
        {
            m_ResourceComponent.UnloadAsset(dataTableAsset);
        }

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <param name="dataTableName">数据表名称。</param>
        /// <param name="dataTableNameInType">数据表类型下的名称。</param>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="dataTableObject">数据表对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, string dataTableAssetName, object dataTableObject, object userData)
        {
            if (dataRowType == null)
            {
                Log.Warning("Data row type is invalid.");
                return false;
            }

            TextAsset dataTableTextAsset = dataTableObject as TextAsset;
            if (dataTableTextAsset != null)
            {
                if (dataTableAssetName.EndsWith(BytesAssetExtension))
                {
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableTextAsset.bytes);
                }
                else
                {
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableTextAsset.text);
                }

                return true;
            }

            byte[] dataTableBytes = dataTableObject as byte[];
            if (dataTableBytes != null)
            {
                if (dataTableAssetName.EndsWith(BytesAssetExtension))
                {
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableBytes);
                }
                else
                {
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, Utility.Converter.GetString(dataTableBytes));
                }

                return true;
            }

            Log.Warning("Data table object '{0}' is invalid.", dataTableName);
            return false;
        }

        private GameFrameworkDataSegment ReadDataRowSegment(string dataTableText, ref int position)
        {
            int length = dataTableText.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = dataTableText[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset - position > 0)
                        {
                            GameFrameworkDataSegment dataRowSegment = new GameFrameworkDataSegment(dataTableText, position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && (position < length) && (dataTableText[position] == '\n'))
                            {
                                position++;
                            }

                            return dataRowSegment;
                        }

                        offset++;
                        position++;
                        break;

                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                GameFrameworkDataSegment dataRowSegment = new GameFrameworkDataSegment(dataTableText, position, offset - position);
                position = offset;
                return dataRowSegment;
            }

            return default(GameFrameworkDataSegment);
        }

        private void Start()
        {
            m_DataTableComponent = GameEntry.GetComponent<DataTableComponent>();
            if (m_DataTableComponent == null)
            {
                Log.Fatal("Data table component is invalid.");
                return;
            }

            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
