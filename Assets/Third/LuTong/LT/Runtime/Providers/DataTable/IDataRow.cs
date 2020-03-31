/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/17
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT.DataTable
{
    /// <summary>
    /// 数据表行接口。
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 获取数据表行的编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 数据表行二进制流解析器。
        /// </summary>
        /// <param name="dataRows">要解析的数据表行。</param>
        /// <returns>是否解析数据表行成功。</returns>
        bool ParseDataRow(string[] dataRows);
    }
}