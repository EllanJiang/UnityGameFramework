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
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认数据表辅助器。
    /// </summary>
    public class DefaultDataTableHelper : DataTableHelperBase
    {
        private DataTableComponent m_DataTableComponent = null;
        private ResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表行片段。</returns>
        public override IEnumerable<GameFrameworkSegment<string>> GetDataRowSegments(string text)
        {
            List<GameFrameworkSegment<string>> dataRowSegments = new List<GameFrameworkSegment<string>>();
            GameFrameworkSegment<string> dataRowSegment;
            int position = 0;
            while ((dataRowSegment = ReadLine(text, ref position)) != default(GameFrameworkSegment<string>))
            {
                if (text[dataRowSegment.Offset] == '#')
                {
                    continue;
                }

                dataRowSegments.Add(dataRowSegment);
            }

            return dataRowSegments;
        }

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        public override IEnumerable<GameFrameworkSegment<byte[]>> GetDataRowSegments(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    int stringOffset = binaryReader.ReadInt32();
                    int dataRowSegmentCount = binaryReader.Read7BitEncodedInt32();
                    GameFrameworkSegment<byte[]>[] dataRowSegments = new GameFrameworkSegment<byte[]>[dataRowSegmentCount];
                    for (int i = 0; i < dataRowSegmentCount; i++)
                    {
                        int length = binaryReader.Read7BitEncodedInt32();
                        dataRowSegments[i] = new GameFrameworkSegment<byte[]>(bytes, (int)binaryReader.BaseStream.Position, length);
                        binaryReader.BaseStream.Position += length;
                    }

                    if (binaryReader.BaseStream.Position != stringOffset)
                    {
                        throw new GameFrameworkException("Verification failed.");
                    }

                    return dataRowSegments;
                }
            }
        }

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        public override IEnumerable<GameFrameworkSegment<Stream>> GetDataRowSegments(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            int stringOffset = binaryReader.ReadInt32();
            int dataRowSegmentCount = binaryReader.Read7BitEncodedInt32();
            GameFrameworkSegment<Stream>[] dataRowSegments = new GameFrameworkSegment<Stream>[dataRowSegmentCount];
            for (int i = 0; i < dataRowSegmentCount; i++)
            {
                int length = binaryReader.Read7BitEncodedInt32();
                dataRowSegments[i] = new GameFrameworkSegment<Stream>(stream, (int)binaryReader.BaseStream.Position, length);
                binaryReader.BaseStream.Position += length;
            }

            if (binaryReader.BaseStream.Position != stringOffset)
            {
                throw new GameFrameworkException("Verification failed.");
            }

            return dataRowSegments;
        }

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="text">要解析的数据表文本。</param>
        /// <returns>数据表用户自定义数据。</returns>
        public override object GetDataTableUserData(string text)
        {
            return null;
        }

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="bytes">要解析的数据表二进制流。</param>
        /// <returns>数据表用户自定义数据。</returns>
        public override object GetDataTableUserData(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes, false))
            {
                return GetDataTableUserData(stream);
            }
        }

        /// <summary>
        /// 获取数据表用户自定义数据。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表用户自定义数据。</returns>
        public override object GetDataTableUserData(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            binaryReader.BaseStream.Position = binaryReader.ReadInt32();
            int stringCount = binaryReader.Read7BitEncodedInt32();
            string[] strings = new string[stringCount];
            for (int i = 0; i < stringCount; i++)
            {
                strings[i] = binaryReader.ReadString();
            }

            return strings;
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
        /// <param name="dataTableObject">数据表对象。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, object dataTableObject, LoadType loadType, object userData)
        {
            if (dataRowType == null)
            {
                Log.Warning("Data row type is invalid.");
                return false;
            }

            TextAsset dataTableTextAsset = dataTableObject as TextAsset;
            if (dataTableTextAsset != null)
            {
                switch (loadType)
                {
                    case LoadType.TextFromAsset:
                        m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableTextAsset.text);
                        return true;

                    case LoadType.BytesFromAsset:
                        m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableTextAsset.bytes);
                        return true;

                    case LoadType.StreamFromAsset:
                        using (MemoryStream stream = new MemoryStream(dataTableTextAsset.bytes, false))
                        {
                            m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, stream);
                            return true;
                        }

                    default:
                        Log.Warning("Not supported load type '{0}' for data table asset.");
                        return false;
                }
            }

            byte[] dataTableBytes = dataTableObject as byte[];
            if (dataTableBytes != null)
            {
                switch (loadType)
                {
                    case LoadType.TextFromBinary:
                        m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, Utility.Converter.GetString(dataTableBytes));
                        return true;

                    case LoadType.BytesFromBinary:
                        m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, dataTableBytes);
                        return true;

                    case LoadType.StreamFromBinary:
                        using (MemoryStream stream = new MemoryStream(dataTableBytes, false))
                        {
                            m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, stream);
                            return true;
                        }

                    default:
                        Log.Warning("Not supported load type '{0}' for data table binary.");
                        return false;
                }
            }

            Log.Warning("Data table object '{0}' is invalid.", dataTableName);
            return false;
        }

        private GameFrameworkSegment<string> ReadLine(string text, ref int position)
        {
            int length = text.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = text[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset - position > 0)
                        {
                            GameFrameworkSegment<string> segment = new GameFrameworkSegment<string>(text, position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && (position < length) && (text[position] == '\n'))
                            {
                                position++;
                            }

                            return segment;
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
                GameFrameworkSegment<string> segment = new GameFrameworkSegment<string>(text, position, offset - position);
                position = offset;
                return segment;
            }

            return default(GameFrameworkSegment<string>);
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
