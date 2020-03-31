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
using LT.MonoDriver;

namespace LT.DataTable
{
    /// <summary>
    /// DataTable管理。
    /// </summary>
    internal sealed partial class DataTableManager : IDataTableManager, IOnDestroy
    {
        private readonly Dictionary<string, DataTable> dataTables;

        /// <summary>
        /// 构建DataTable管理
        /// </summary>
        public DataTableManager()
        {
            this.dataTables = new Dictionary<string, DataTable>();
        }

        /// <inheritdoc />
        public int Count => dataTables.Count;

        /// <summary>
        /// 加载并创建数据表。
        /// </summary>
        /// <typeparam name="T">数据表行类型</typeparam>
        /// <param name="path">数据表资源路径</param>
        /// <returns>返回指定数据表</returns>
        public IDataTable<T> LoadDataTable<T>(string path) where T : class, IDataRow, new()
        {
            if (!HasDataTable<T>())
            {
                var asset = ResourceUtils.Load<ScriptableObjectBase<T>>(path);
                var table = CreateDataTable<T>(asset.dataSet);
                ResourceUtils.UnloadAsset(asset);
                return table;
            }
            throw new ArgumentException($"Can not load {path}.");
        }

        /// <inheritdoc />
        public bool HasDataTable<T>() where T : IDataRow
        {
            return InternalHasDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <inheritdoc />
        public bool HasDataTable(Type dataRowType)
        {
            Guard.Verify<ArgumentNullException>(dataRowType == null, "Data row type is invalid.");
            Guard.Verify<ArgumentNullException>(!typeof(IDataRow).IsAssignableFrom(dataRowType), $"Data row type '{dataRowType.FullName}' is invalid.");

            return InternalHasDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <inheritdoc />
        public IDataTable<T> GetDataTable<T>() where T : IDataRow
        {
            return (IDataTable<T>)InternalGetDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <inheritdoc />
        public DataTable GetDataTable(Type dataRowType)
        {
            Guard.Requires<ArgumentNullException>(dataRowType != null, "Data row type is invalid.");
            Guard.Requires<ArgumentNullException>(typeof(IDataRow).IsAssignableFrom(dataRowType), $"Data row type '{dataRowType.FullName}' is invalid.");

            return InternalGetDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        /// <inheritdoc />
        public DataTable[] GetAllDataTables()
        {
            int index = 0;
            DataTable[] results = new DataTable[dataTables.Count];
            foreach (KeyValuePair<string, DataTable> dataTable in dataTables)
            {
                results[index++] = dataTable.Value;
            }
            return results;
        }

        /// <inheritdoc />
        public void GetAllDataTables(List<DataTable> results)
        {
            Guard.Requires<ArgumentNullException>(results != null, "Results is invalid.");

            results.Clear();
            foreach (KeyValuePair<string, DataTable> dataTable in dataTables)
            {
                results.Add(dataTable.Value);
            }
        }

        /// <inheritdoc />
        public IDataTable<T> CreateDataTable<T>(List<T> dataSet) where T : class, IDataRow, new()
        {
            Guard.Requires<ArgumentNullException>(!HasDataTable<T>(), $"Already exist data table '{Utility.Text.GetFullName<T>(string.Empty)}'.");

            DataTable<T> dataTable = new DataTable<T>();
            foreach (var data in dataSet)
            {
                dataTable.AddDataRow(data);
            }
            dataTables.Add(Utility.Text.GetFullName<T>(string.Empty), dataTable);
            return dataTable;
        }

        /// <inheritdoc />
        public bool RemoveDataTable<T>() where T : IDataRow
        {
            return InternalRemoveDataTable(Utility.Text.GetFullName<T>(string.Empty));
        }

        /// <inheritdoc />
        public bool RemoveDataTable(Type dataRowType)
        {
            Guard.Requires<ArgumentNullException>(dataRowType != null, "Data row type is invalid.");
            Guard.Requires<ArgumentNullException>(typeof(IDataRow).IsAssignableFrom(dataRowType), $"Data row type '{dataRowType.FullName}' is invalid.");

            return InternalRemoveDataTable(Utility.Text.GetFullName(dataRowType, string.Empty));
        }

        private bool InternalHasDataTable(string fullName)
        {
            return dataTables.ContainsKey(fullName);
        }

        private DataTable InternalGetDataTable(string fullName)
        {
            DataTable dataTable;
            if (dataTables.TryGetValue(fullName, out dataTable))
            {
                return dataTable;
            }

            return null;
        }

        private bool InternalRemoveDataTable(string fullName)
        {
            DataTable dataTable;
            if (dataTables.TryGetValue(fullName, out dataTable))
            {
                dataTable.Shutdown();
                return dataTables.Remove(fullName);
            }

            return false;
        }

        /// <inheritdoc />
        public void OnDestroy()
        {
            foreach (KeyValuePair<string, DataTable> dataTable in dataTables)
            {
                dataTable.Value.Shutdown();
            }
            dataTables.Clear();
        }
    }
}