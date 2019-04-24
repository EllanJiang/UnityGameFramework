//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
            List<GameFrameworkSegment<byte[]>> dataRowSegments = new List<GameFrameworkSegment<byte[]>>();
            using (MemoryStream stream = new MemoryStream(bytes, false))
            {
                while (stream.Position < stream.Length)
                {
                    int length = ReadInt32(stream);
                    dataRowSegments.Add(new GameFrameworkSegment<byte[]>(bytes, (int)stream.Position, length));
                    stream.Position += length;
                }
            }

            return dataRowSegments;
        }

        /// <summary>
        /// 获取数据表行片段。
        /// </summary>
        /// <param name="stream">要解析的数据表二进制流。</param>
        /// <returns>数据表行片段。</returns>
        public override IEnumerable<GameFrameworkSegment<Stream>> GetDataRowSegments(Stream stream)
        {
            List<GameFrameworkSegment<Stream>> dataRowSegments = new List<GameFrameworkSegment<Stream>>();
            while (stream.Position < stream.Length)
            {
                int length = ReadInt32(stream);
                dataRowSegments.Add(new GameFrameworkSegment<Stream>(stream, (int)stream.Position, length));
                stream.Position += length;
            }

            return dataRowSegments;
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
        /// <param name="dataTableAsset">数据表资源。</param>
        /// <param name="loadType">数据表加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameInType, object dataTableAsset, LoadType loadType, object userData)
        {
            TextAsset textAsset = dataTableAsset as TextAsset;
            if (textAsset == null)
            {
                Log.Warning("Data table asset '{0}' is invalid.", dataTableName);
                return false;
            }

            if (dataRowType == null)
            {
                Log.Warning("Data row type is invalid.");
                return false;
            }

            switch (loadType)
            {
                case LoadType.Text:
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, textAsset.text);
                    break;

                case LoadType.Bytes:
                    m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, textAsset.bytes);
                    break;

                case LoadType.Stream:
                    using (MemoryStream stream = new MemoryStream(textAsset.bytes, false))
                    {
                        m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, stream);
                    }
                    break;

                default:
                    Log.Warning("Unknown load type.");
                    return false;
            }

            return true;
        }

        private int ReadInt32(Stream stream)
        {
            return stream.ReadByte() | (stream.ReadByte() << 8) | (stream.ReadByte() << 16) | (stream.ReadByte() << 24);
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
                            if (((ch == '\r') && (position < length)) && (text[position] == '\n'))
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
