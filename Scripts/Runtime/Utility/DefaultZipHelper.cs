//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认压缩解压缩辅助器。
    /// </summary>
    public class DefaultZipHelper : Utility.Zip.IZipHelper
    {
        private readonly byte[] m_BytesCache = new byte[0x10000];

        /// <summary>
        /// 压缩数据。
        /// </summary>
        /// <param name="bytes">要压缩的数据的二进制流。</param>
        /// <param name="offset">要压缩的数据的二进制流的偏移。</param>
        /// <param name="length">要压缩的数据的二进制流的长度。</param>
        /// <param name="compressedStream">压缩后的数据的二进制流。</param>
        /// <returns>是否压缩数据成功。</returns>
        public bool Compress(byte[] bytes, int offset, int length, Stream compressedStream)
        {
            if (bytes == null)
            {
                return false;
            }

            if (offset < 0)
            {
                return false;
            }

            if (length > bytes.Length)
            {
                return false;
            }

            if (compressedStream == null)
            {
                return false;
            }

            try
            {
                using (GZipOutputStream gZipOutputStream = new GZipOutputStream(compressedStream))
                {
                    gZipOutputStream.Write(bytes, offset, length);
                    if (compressedStream.Length >= 8L)
                    {
                        long current = compressedStream.Position;
                        compressedStream.Position = 4L;
                        compressedStream.WriteByte(25);
                        compressedStream.WriteByte(134);
                        compressedStream.WriteByte(2);
                        compressedStream.WriteByte(32);
                        compressedStream.Position = current;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 解压缩数据。
        /// </summary>
        /// <param name="bytes">要解压缩的数据的二进制流。</param>
        /// <param name="offset">要解压缩的数据的二进制流的偏移。</param>
        /// <param name="length">要解压缩的数据的二进制流的长度。</param>
        /// <param name="decompressedStream">解压缩后的数据的二进制流。</param>
        /// <returns>是否解压缩数据成功。</returns>
        public bool Decompress(byte[] bytes, int offset, int length, Stream decompressedStream)
        {
            if (bytes == null)
            {
                return false;
            }

            if (offset < 0)
            {
                return false;
            }

            if (length > bytes.Length)
            {
                return false;
            }

            if (decompressedStream == null)
            {
                return false;
            }

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream(bytes, offset, length, false);
                using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
                {
                    int bytesRead = 0;
                    while ((bytesRead = gZipInputStream.Read(m_BytesCache, 0, m_BytesCache.Length)) > 0)
                    {
                        decompressedStream.Write(m_BytesCache, 0, bytesRead);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
    }
}
