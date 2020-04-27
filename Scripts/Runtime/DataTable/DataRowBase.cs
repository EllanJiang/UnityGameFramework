//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
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
        /// 数据表行解析器。
        /// </summary>
        /// <param name="dataRowSegment">要解析的数据表行片段。</param>
        /// <param name="dataTableUserData">数据表用户自定义数据。</param>
        /// <returns>是否解析数据表行成功。</returns>
        public virtual bool ParseDataRow(GameFrameworkDataSegment dataRowSegment, object dataTableUserData)
        {
            Log.Warning("Not implemented ParseDataRow.");
            return false;
        }
    }
}
