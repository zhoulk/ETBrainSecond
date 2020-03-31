/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/17
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LT.DataTable
{
    /// <summary>
    /// 数据表管理接口。
    /// </summary>
    public interface IDataTableManager
    {
        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 加载并创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行类型</typeparam>
        /// <param name="path">数据表资源路径</param>
        /// <returns>返回指定数据表</returns>
        IDataTable<T> LoadDataTable<T>(string path) where T : class, IDataRow, new();

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 是否存在数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否存在数据表。</returns>
        bool HasDataTable(Type dataRowType);

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>要获取的数据表。</returns>
        IDataTable<T> GetDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 获取数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>要获取的数据表。</returns>
        DataTable GetDataTable(Type dataRowType);

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        DataTable[] GetAllDataTables();

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <param name="results">所有数据表。</param>
        void GetAllDataTables(List<DataTable> results);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        IDataTable<T> CreateDataTable<T>(List<T> dataSet) where T : class, IDataRow, new();

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <typeparam name="T">数据表行的类型。</typeparam>
        /// <returns>是否销毁数据表成功。</returns>
        bool RemoveDataTable<T>() where T : IDataRow;

        /// <summary>
        /// 销毁数据表。
        /// </summary>
        /// <param name="dataRowType">数据表行的类型。</param>
        /// <returns>是否销毁数据表成功。</returns>
        bool RemoveDataTable(Type dataRowType);
    }
}