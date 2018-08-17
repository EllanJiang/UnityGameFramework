//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
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
        /// <summary>
        /// 压缩数据。
        /// </summary>
        /// <param name="bytes">要压缩的数据。</param>
        /// <returns>压缩后的数据。</returns>
        public byte[] Compress(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return bytes;
            }

            byte[] result = null;
            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream();
                using (GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream))
                {
                    gZipOutputStream.Write(bytes, 0, bytes.Length);
                }

                result = memoryStream.ToArray();

                if (result.Length >= 8)
                {
                    result[4] = 25;
                    result[5] = 134;
                    result[6] = 2;
                    result[7] = 32;
                }
            }
            catch
            {

            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }

            return result;
        }

        /// <summary>
        /// 解压缩数据。
        /// </summary>
        /// <param name="bytes">要解压缩的数据。</param>
        /// <returns>解压缩后的数据。</returns>
        public byte[] Decompress(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return null;
            }

            MemoryStream decompressedStream = null;
            MemoryStream memoryStream = null;
            try
            {
                decompressedStream = new MemoryStream();
                memoryStream = new MemoryStream(bytes);
                using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
                {
                    memoryStream = null;
                    int bytesRead = 0;
                    byte[] clip = new byte[0x1000];
                    while ((bytesRead = gZipInputStream.Read(clip, 0, clip.Length)) != 0)
                    {
                        decompressedStream.Write(clip, 0, bytesRead);
                    }
                }

                return decompressedStream.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                if (decompressedStream != null)
                {
                    decompressedStream.Dispose();
                    decompressedStream = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
    }
}
