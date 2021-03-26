//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 数据表行基类。
    /// </summary>
    public abstract class DataRowBase : IDataRow
    {
        /// <summary>
        /// 获取数据表行的编号。
        /// </summary>
        public abstract int Id
        {
            get;
        }

        /// <summary>
        /// 解析数据表行。
        /// </summary>
        /// <param name="dataRowString">要解析的数据表行字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析数据表行成功。</returns>
        public virtual bool ParseDataRow(string dataRowString, object userData)
        {
            Log.Warning("Not implemented ParseDataRow(string dataRowString, object userData).");
            return false;
        }

        /// <summary>
        /// 解析数据表行。
        /// </summary>
        /// <param name="dataRowBytes">要解析的数据表行二进制流。</param>
        /// <param name="startIndex">数据表行二进制流的起始位置。</param>
        /// <param name="length">数据表行二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析数据表行成功。</returns>
        public virtual bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            Log.Warning("Not implemented ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData).");
            return false;
        }
    }
}
