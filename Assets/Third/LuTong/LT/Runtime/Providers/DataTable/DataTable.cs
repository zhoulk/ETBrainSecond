/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/17
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT.DataTable
{
    /// <summary>
    /// 数据表基类。
    /// </summary>
    public abstract class DataTable
    {
        private readonly string name;

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        public DataTable() : this(null) { }

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        /// <param name="tableName">数据表名称。</param>
        public DataTable(string tableName)
        {
            this.name = tableName ?? string.Empty;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// 获取数据表行数。
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// 关闭并清理数据表。
        /// </summary>
        public abstract void Shutdown();
    }

    /// <summary>
    /// 数据表。
    /// </summary>
    /// <typeparam name="T">数据表行的类型。</typeparam>
    public class DataTable<T> : DataTable, IDataTable<T> where T : class, IDataRow, new()
    {
        private T minIdDataRow;
        private T maxIdDataRow;
        private readonly Dictionary<int, T> dataSet;

        /// <summary>
        /// 初始化数据表的新实例。
        /// </summary>
        /// <param name="name">数据表名称。</param>
        public DataTable() : base(typeof(T).Name)
        {
            dataSet = new Dictionary<int, T>();
            minIdDataRow = null;
            maxIdDataRow = null;
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// 获取数据表行数。
        /// </summary>
        public override int Count
        {
            get
            {
                return dataSet.Count;
            }
        }

        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>数据表行。</returns>
        public T this[int id]
        {
            get
            {
                return GetDataRow(id);
            }
        }

        /// <summary>
        /// 获取编号最小的数据表行。
        /// </summary>
        public T MinIdDataRow
        {
            get
            {
                return minIdDataRow;
            }
        }

        /// <summary>
        /// 获取编号最大的数据表行。
        /// </summary>
        public T MaxIdDataRow
        {
            get
            {
                return maxIdDataRow;
            }
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>是否存在数据表行。</returns>
        public bool HasDataRow(int id)
        {
            return dataSet.ContainsKey(id);
        }

        /// <summary>
        /// 检查是否存在数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>是否存在数据表行。</returns>
        public bool HasDataRow(Predicate<T> condition)
        {
            Guard.Requires<ArgumentException>(condition != null, "Condition is invalid.");

            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                if (condition(dataRow.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取数据表行。
        /// </summary>
        /// <param name="id">数据表行的编号。</param>
        /// <returns>数据表行。</returns>
        public T GetDataRow(int id)
        {
            T dataRow;
            if (dataSet.TryGetValue(id, out dataRow))
            {
                return dataRow;
            }

            return null;
        }

        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>符合条件的数据表行。</returns>
        /// <remarks>当存在多个符合条件的数据表行时，仅返回第一个符合条件的数据表行。</remarks>
        public T GetDataRow(Predicate<T> condition)
        {
            Guard.Requires<ArgumentException>(condition != null, "Condition is invalid.");

            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                if (condition(dataRow.Value))
                {
                    return dataRow.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <returns>符合条件的数据表行。</returns>
        public T[] GetDataRows(Predicate<T> condition)
        {
            Guard.Requires<ArgumentException>(condition != null, "Condition is invalid.");

            List<T> results = new List<T>();
            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                if (condition(dataRow.Value))
                {
                    results.Add(dataRow.Value);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="results">符合条件的数据表行。</param>
        public void GetDataRows(Predicate<T> condition, List<T> results)
        {
            Guard.Requires<ArgumentException>(condition != null, "Condition is invalid.");
            Guard.Requires<ArgumentException>(results != null, "Results is invalid.");

            results.Clear();
            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                if (condition(dataRow.Value))
                {
                    results.Add(dataRow.Value);
                }
            }
        }

        /// <summary>
        /// 获取排序后的数据表行。
        /// </summary>
        /// <param name="comparison">要排序的条件。</param>
        /// <returns>排序后的数据表行。</returns>
        public T[] GetDataRows(Comparison<T> comparison)
        {
            Guard.Requires<ArgumentException>(comparison != null, "Comparison is invalid.");

            List<T> results = new List<T>();
            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                results.Add(dataRow.Value);
            }

            results.Sort(comparison);
            return results.ToArray();
        }

        /// <summary>
        /// 获取排序后的符合条件的数据表行。
        /// </summary>
        /// <param name="condition">要检查的条件。</param>
        /// <param name="comparison">要排序的条件。</param>
        /// <returns>排序后的符合条件的数据表行。</returns>
        public T[] GetDataRows(Predicate<T> condition, Comparison<T> comparison)
        {
            Guard.Requires<ArgumentException>(condition != null, "condition is invalid.");
            Guard.Requires<ArgumentException>(comparison != null, "comparison is invalid.");

            List<T> results = new List<T>();
            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                if (condition(dataRow.Value))
                {
                    results.Add(dataRow.Value);
                }
            }

            results.Sort(comparison);
            return results.ToArray();
        }

        /// <summary>
        /// 获取所有数据表行。
        /// </summary>
        /// <returns>所有数据表行。</returns>
        public T[] GetAllDataRows()
        {
            int index = 0;
            T[] results = new T[dataSet.Count];
            foreach (KeyValuePair<int, T> dataRow in dataSet)
            {
                results[index++] = dataRow.Value;
            }

            return results;
        }


        /// <summary>
        /// 返回一个循环访问数据表的枚举器。
        /// </summary>
        /// <returns>可用于循环访问数据表的对象。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return dataSet.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dataSet.Values.GetEnumerator();
        }

        /// <summary>
        /// 关闭并清理数据表。
        /// </summary>
        public override void Shutdown()
        {
            dataSet.Clear();
        }

        public void AddDataRow(T dataRow)
        {
            Guard.Requires<ArgumentException>(!HasDataRow(dataRow.Id), $"Already exist '{dataRow.Id.ToString()}' in data table '{Utility.Text.GetFullName<T>(Name)}'.");

            dataSet.Add(dataRow.Id, dataRow);
            if (minIdDataRow == null || minIdDataRow.Id > dataRow.Id)
            {
                minIdDataRow = dataRow;
            }

            if (maxIdDataRow == null || maxIdDataRow.Id < dataRow.Id)
            {
                maxIdDataRow = dataRow;
            }
        }
    }
}