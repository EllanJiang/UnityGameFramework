//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace GameFramework.FileSystem
{
    /// <summary>
    /// 安卓文件系统流。
    /// </summary>
    public sealed class AndroidFileSystemStream : FileSystemStream
    {
        /// <summary>
        /// 初始化安卓文件系统流的新实例。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <param name="createNew">是否创建新的文件系统流。</param>
        public AndroidFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new GameFrameworkException("Full path is invalid.");
            }

            if (access != FileSystemAccess.Read)
            {
                throw new GameFrameworkException(Utility.Text.Format("'{0}' is not supported in AndroidFileSystemStream.", access.ToString()));
            }

            if (createNew)
            {
                throw new GameFrameworkException("Create new is not supported in AndroidFileSystemStream.");
            }

            // TODO: AndroidFileSystemStream.ctor
            throw new System.NotImplementedException("AndroidFileSystemStream.ctor");
        }

        /// <summary>
        /// 获取或设置文件系统流位置。
        /// </summary>
        protected override long Position
        {
            get
            {
                // TODO: long GetPosition()
                throw new System.NotImplementedException("Position.get");
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 获取文件系统流长度。
        /// </summary>
        protected override long Length
        {
            get
            {
                // TODO: long GetLength()
                throw new System.NotImplementedException("Length.get");
            }
        }

        /// <summary>
        /// 设置文件系统流长度。
        /// </summary>
        /// <param name="length">要设置的文件系统流的长度。</param>
        protected override void SetLength(long length)
        {
            throw new GameFrameworkException("SetLength is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 定位文件系统流位置。
        /// </summary>
        /// <param name="offset">要定位的文件系统流位置的偏移。</param>
        /// <param name="origin">要定位的文件系统流位置的方式。</param>
        protected override void Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    // TODO: Seek(long offset)
                    throw new System.NotImplementedException("Seek(long offset)");
                    break;

                case SeekOrigin.Current:
                    Seek(Position + offset, SeekOrigin.Begin);
                    break;

                case SeekOrigin.End:
                    Seek(Length + offset, SeekOrigin.Begin);
                    break;
            }
        }

        /// <summary>
        /// 从文件系统流中读取一个字节。
        /// </summary>
        /// <returns>读取的字节。</returns>
        protected override byte ReadByte()
        {
            // TODO: byte ReadByte()
            throw new System.NotImplementedException("ReadByte()");
        }

        /// <summary>
        /// 从文件系统流中读取二进制流。
        /// </summary>
        /// <param name="buffer">存储读取文件内容的二进制流。</param>
        /// <param name="startIndex">存储读取文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储读取文件内容的二进制流的长度。</param>
        /// <returns>实际读取了多少字节。</returns>
        protected override int Read(byte[] buffer, int startIndex, int length)
        {
            // TODO: int Read(byte[] buffer, int startIndex, int length)
            throw new System.NotImplementedException("Read(byte[] buffer, int startIndex, int length)");
        }

        /// <summary>
        /// 向文件系统流中写入一个字节。
        /// </summary>
        /// <param name="value">要写入的字节。</param>
        protected override void WriteByte(byte value)
        {
            throw new GameFrameworkException("WriteByte is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 向文件系统流中写入二进制流。
        /// </summary>
        /// <param name="buffer">存储写入文件内容的二进制流。</param>
        /// <param name="startIndex">存储写入文件内容的二进制流的起始位置。</param>
        /// <param name="length">存储写入文件内容的二进制流的长度。</param>
        protected override void Write(byte[] buffer, int startIndex, int length)
        {
            throw new GameFrameworkException("Write is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 将文件系统流立刻更新到存储介质中。
        /// </summary>
        protected override void Flush()
        {
            throw new GameFrameworkException("Flush is not supported in AndroidFileSystemStream.");
        }

        /// <summary>
        /// 关闭文件系统流。
        /// </summary>
        protected override void Close()
        {
            // TODO: void Close()
            throw new System.NotImplementedException("Close()");
        }
    }
}
